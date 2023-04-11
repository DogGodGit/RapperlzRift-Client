using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupSupportLimitationGift : CsPopupSub 
{
    [SerializeField] GameObject m_goItemSlot;
    GameObject m_goPopupItemInfo;

    Transform m_trPopupList;
    Transform m_trPopupItemInfo;

    int m_nReceiveLimitationGiftScheduleId = 0;

    IEnumerator m_IEnumeratorLoadPopupItemInfo;

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
        {
            UpdateLimitationGift(CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i].ScheduleId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive += OnEventLimitationGiftRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventLimitationGiftRewardReceive -= OnEventLimitationGiftRewardReceive;
    }

    #region Event
    
    //---------------------------------------------------------------------------------------------------
    void OnEventLimitationGiftRewardReceive()
    {
        UpdateLimitationGift(m_nReceiveLimitationGiftScheduleId);
    }

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickLimitationGiftReward(int nScheduleId)
    {
        m_nReceiveLimitationGiftScheduleId = nScheduleId;
        CsCommandEventManager.Instance.SendLimitationGiftRewardReceive(nScheduleId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickItemslot(CsItem csItem)
    {
        if (m_IEnumeratorLoadPopupItemInfo != null)
        {
            StopCoroutine(m_IEnumeratorLoadPopupItemInfo);
            m_IEnumeratorLoadPopupItemInfo = null;
        }

        m_IEnumeratorLoadPopupItemInfo = LoadPopupItemInfo(csItem);
        StartCoroutine(m_IEnumeratorLoadPopupItemInfo);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        Text textDescription = transform.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString(CsGameData.Instance.LimitationGift.ScheduleText);

        Transform trLimitationGift = null;

        for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
        {
            CsLimitationGiftRewardSchedule csLimitationGiftRewardSchedule = CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i];

            if (csLimitationGiftRewardSchedule == null)
            {
                continue;
            }
            else
            {
                trLimitationGift = transform.Find("ImageLimitationGift" + csLimitationGiftRewardSchedule.ScheduleId);

                if (trLimitationGift == null)
                {
                    continue;
                }
                else
                {
                    Transform trImageReceiveAllowTime = trLimitationGift.Find("ImageReciveAllowTime");

                    Text textTitle = trImageReceiveAllowTime.Find("TextTitle").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textTitle);
                    textTitle.text = CsConfiguration.Instance.GetString("A102_TXT_00007");

                    Text textRewardSchedule = trImageReceiveAllowTime.Find("TextRewardSchedule").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRewardSchedule);

                    TimeSpan tsStartTime = TimeSpan.FromSeconds(csLimitationGiftRewardSchedule.StartTime);
                    TimeSpan tsEndTime = TimeSpan.FromSeconds(csLimitationGiftRewardSchedule.EndTime);
                    textRewardSchedule.text = string.Format(CsConfiguration.Instance.GetString("A102_TXT_00013"), tsStartTime.Hours.ToString("00"), tsStartTime.Minutes.ToString("00"), tsEndTime.Hours.ToString("00"), tsEndTime.Minutes.ToString("00"));

                    Transform trLimitedRewardList = trLimitationGift.Find("LimitedRewardList");
                    Transform trItemSlot = null;

                    for (int j = 0; j < csLimitationGiftRewardSchedule.LimitationGiftAvailableRewardList.Count; j++)
                    {
                        trItemSlot = trLimitedRewardList.Find("ItemSlot" + j);

                        if (trItemSlot == null)
                        {
                            trItemSlot = Instantiate(m_goItemSlot, trLimitedRewardList).transform;
                            trItemSlot.name = "ItemSlot" + j;
                        }
                        else
                        {
                            trItemSlot.gameObject.SetActive(true);
                        }

                        CsItem csItem = csLimitationGiftRewardSchedule.LimitationGiftAvailableRewardList[j].Item;
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, false, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                        Button buttonItemSlot = trItemSlot.GetComponent<Button>();
                        buttonItemSlot.onClick.RemoveAllListeners();
                        buttonItemSlot.onClick.AddListener(() => OnClickItemslot(csItem));
                        buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    }

                    Button buttonReward = trLimitationGift.Find("ButtonReward").GetComponent<Button>();
                    buttonReward.onClick.RemoveAllListeners();
                    buttonReward.onClick.AddListener(() => OnClickLimitationGiftReward(csLimitationGiftRewardSchedule.ScheduleId));
                    buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                    Text textButtonReward = buttonReward.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textButtonReward);
                    textButtonReward.text = CsConfiguration.Instance.GetString("A102_TXT_00008");

                    Text textReceiveNotAllow = trLimitationGift.Find("TextReceiveNotAllow").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textReceiveNotAllow);
                    textReceiveNotAllow.text = CsConfiguration.Instance.GetString("A102_TXT_00009");

                    UpdateLimitationGift(csLimitationGiftRewardSchedule.ScheduleId);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLimitationGift(int nScheduleId)
    {
        Transform trLimitationGift = transform.Find("ImageLimitationGift" + nScheduleId);

        if (trLimitationGift == null)
        {
            return;
        }
        else
        {
            Transform trTextReceiveNotAllow = trLimitationGift.Find("TextReceiveNotAllow");
            Transform trButtonReward = trLimitationGift.Find("ButtonReward");
            Transform trImageLock = trLimitationGift.Find("ImageLock");

            Text textLock = trImageLock.Find("TextLock").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLock);

            if (CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.FindIndex(a => a == nScheduleId) < 0)
            {
                // 안 받음

                CsLimitationGiftRewardSchedule csLimitationGiftRewardSchedule = CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Find(a => a.ScheduleId == nScheduleId);

                if (csLimitationGiftRewardSchedule == null)
                {
                    return;
                }
                else
                {
                    int nSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                    if (nSecond < csLimitationGiftRewardSchedule.StartTime)
                    {
                        // 수령 시간 전
                        trImageLock.gameObject.SetActive(false);
                        trButtonReward.gameObject.SetActive(false);
                        trTextReceiveNotAllow.gameObject.SetActive(true);
                    }
                    else if (csLimitationGiftRewardSchedule.EndTime <= nSecond)
                    {
                        // 수령 시간 초과
                        trButtonReward.gameObject.SetActive(false);
                        trTextReceiveNotAllow.gameObject.SetActive(false);
                        trImageLock.gameObject.SetActive(true);

                        textLock.color = new Color32(229, 115, 115, 255);
                        textLock.text = CsConfiguration.Instance.GetString("A102_TXT_00010");
                    }
                    else
                    {
                        // 수령 가능
                        trImageLock.gameObject.SetActive(false);
                        trTextReceiveNotAllow.gameObject.SetActive(false);
                        trButtonReward.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                // 받음
                trButtonReward.gameObject.SetActive(false);
                trTextReceiveNotAllow.gameObject.SetActive(false);
                trImageLock.gameObject.SetActive(true);

                textLock.color = CsUIData.Instance.ColorGreen;
                textLock.text = CsConfiguration.Instance.GetString("A102_TXT_00011");
            }
        }
    }

    #region PopupItemInfo

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csItem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csItem);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem)
    {
        m_trPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList).transform;

        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, 0, false, -1, false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trPopupItemInfo.gameObject);
        m_trPopupItemInfo = null;
    }

    #endregion PopupItemInfo
}