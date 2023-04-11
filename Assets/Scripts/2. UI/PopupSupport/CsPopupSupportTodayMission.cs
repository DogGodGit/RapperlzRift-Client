using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport
{
    [SerializeField] GameObject m_goTodayMissionItem;

    Transform m_trTodayMission;
    Transform m_trTodayMissionList;

    Text m_textTimer;

    #region EventHandler

    // 미션 보상 수령
    //---------------------------------------------------------------------------------------------------
    void OnEventTodayMissionRewardReceive(CsHeroTodayMission csHeroTodayMission)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
        {
            UpdateTodayMissionItem(i);
        }

        UpdateNotice();
    }

    // 날짜가 바뀌어 오늘의 미션 아이템 리스트 업데이트
    //---------------------------------------------------------------------------------------------------
    void OnEventTodayMissionListChanged()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
        {
            UpdateTodayMissionItem(i);
        }

        UpdateNotice();
    }

    // 오늘의 미션 진행도 업데이트
    //---------------------------------------------------------------------------------------------------
    void OnEventTodayMissionUpdated(CsHeroTodayMission csHeroTodayMission)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
        {
            UpdateTodayMissionItem(i);
        }

        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceiveTodayMissionReward(System.DateTime dtDate, int nMissionId)
    {
        CsCommandEventManager.Instance.SendTodayMissionRewardReceive(dtDate, nMissionId);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeTodayMission(Transform trTodayMission)
    {
        // UI Initialize
        m_trTodayMission = trTodayMission;
        m_trTodayMissionList = m_trTodayMission.Find("Scroll View/Viewport/Content");

        Text textRemainingTime = m_trTodayMission.Find("TextRemainingTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemainingTime);
        textRemainingTime.text = CsConfiguration.Instance.GetString("A37_TXT_00005");

        m_textTimer = m_trTodayMission.Find("TextTimer").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTimer);
        UpdateTextRemainingTimer();

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
        {
            CreateTodayMissionItem(i);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateTodayMissionItem(int nIndex)
    {
        CsHeroTodayMission csHeroTodayMission = CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[nIndex];

        Transform trTodayMissionItem = Instantiate(m_goTodayMissionItem, m_trTodayMissionList).transform;
        trTodayMissionItem.name = "TodayMissionItem" + nIndex;

        Text textMission = trTodayMissionItem.Find("TextMission").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMission);
        textMission.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01005"), csHeroTodayMission.TodayMission.Name, csHeroTodayMission.TodayMission.TargetCount);

        Text textProgress = trTodayMissionItem.Find("TextProgress").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgress);
        textProgress.text = CsConfiguration.Instance.GetString("A37_TXT_01006");

        Text textProgressCount = trTodayMissionItem.Find("TextProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textProgressCount);
        textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01007"), csHeroTodayMission.ProgressCount, csHeroTodayMission.TodayMission.TargetCount);

        Text textReceiveComplete = trTodayMissionItem.Find("TextReceiveComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReceiveComplete);
        textReceiveComplete.text = CsConfiguration.Instance.GetString("A37_TXT_00003");

        Button buttonReceive = trTodayMissionItem.Find("ButtonReceive").GetComponent<Button>();
        buttonReceive.onClick.RemoveAllListeners();
        int nMissionId = csHeroTodayMission.TodayMission.MissionId;
        System.DateTime dtDate = csHeroTodayMission.Date;
        buttonReceive.onClick.AddListener(() => OnClickReceiveTodayMissionReward(dtDate, nMissionId));
        buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A37_BTN_00001");

        for (int i = 0; i < csHeroTodayMission.TodayMission.TodayMissionRewardList.Count; i++)
        {
            CsTodayMissionReward csTodayMissionReward = csHeroTodayMission.TodayMission.TodayMissionRewardList[i];
            CsItemReward csItemReward = CsGameData.Instance.GetItemReward(csTodayMissionReward.ItemReward.ItemRewardId);

            Transform trItemSlot = trTodayMissionItem.Find("ItemSlot" + i);
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(csItemReward));
            buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            trItemSlot.gameObject.SetActive(true);
        }

        UpdateTodayMissionItem(nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayMissionItem(int nIndex)
    {
        CsHeroTodayMission csHeroTodayMission = CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[nIndex];

        Transform trTodayMissionItem = m_trTodayMissionList.Find("TodayMissionItem" + nIndex);

        // 미션 제목 업데이트
        Text textMission = trTodayMissionItem.Find("TextMission").GetComponent<Text>();
        textMission.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01005"), csHeroTodayMission.TodayMission.Name, csHeroTodayMission.TodayMission.TargetCount);

        // 미션 진행도 텍스트 업데이트
        Text textProgressCount = trTodayMissionItem.Find("TextProgressCount").GetComponent<Text>();

        if (csHeroTodayMission.TodayMission.MissionId == (int)EnTodayMissionType.Tutorial)
        {
            textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01007"), csHeroTodayMission.TodayMission.TargetCount, csHeroTodayMission.TodayMission.TargetCount);
        }
        else
        {
            if (csHeroTodayMission.ProgressCount <= csHeroTodayMission.TodayMission.TargetCount)
            {
                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01007"), csHeroTodayMission.ProgressCount, csHeroTodayMission.TodayMission.TargetCount);
            }
        }

        Text textReceiveComplete = trTodayMissionItem.Find("TextReceiveComplete").GetComponent<Text>();

        // 미션 보상 버튼 업데이트
        Button buttonReceive = trTodayMissionItem.Find("ButtonReceive").GetComponent<Button>();
        buttonReceive.onClick.RemoveAllListeners();

        int nMissionId = csHeroTodayMission.TodayMission.MissionId;
        System.DateTime dtDate = csHeroTodayMission.Date;
        
        buttonReceive.onClick.AddListener(() => OnClickReceiveTodayMissionReward(dtDate, nMissionId));
        buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        // 이미 수령함
        if (csHeroTodayMission.RewardReceived)
        {
            // 아이템 슬롯 비활성화
            for (int i = 0; i < csHeroTodayMission.TodayMission.TodayMissionRewardList.Count; i++)
            {
                Transform trItemSlot = trTodayMissionItem.Find("ItemSlot" + i);

                Image imageDim = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                imageDim.fillAmount = 1.0f;
                imageDim.gameObject.SetActive(true);
            }

            buttonReceive.gameObject.SetActive(false);
            textReceiveComplete.gameObject.SetActive(true);

            trTodayMissionItem.SetAsLastSibling();
        }
        // 아직 수령 안함
        else
        {
            // 아이템 슬롯 비활성화
            for (int i = 0; i < csHeroTodayMission.TodayMission.TodayMissionRewardList.Count; i++)
            {
                Transform trItemSlot = trTodayMissionItem.Find("ItemSlot" + i);

                Image imageDim = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                imageDim.gameObject.SetActive(false);
            }

            // 미션 성공 상태
            if (csHeroTodayMission.TodayMission.MissionId == (int)EnTodayMissionType.Tutorial)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
                trTodayMissionItem.SetAsFirstSibling();
            }
            else
            {
                if (csHeroTodayMission.TodayMission.TargetCount <= csHeroTodayMission.ProgressCount)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
                    trTodayMissionItem.SetAsFirstSibling();
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
                }
            }

            textReceiveComplete.gameObject.SetActive(false);
            buttonReceive.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextRemainingTimer()
    {
        System.TimeSpan tsElapseTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;
        System.TimeSpan tsRemainingTime = System.TimeSpan.FromSeconds(86400 - tsElapseTime.TotalSeconds);

        m_textTimer.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01008"), tsRemainingTime.Hours.ToString("0#"), tsRemainingTime.Minutes.ToString("0#"), tsRemainingTime.Seconds.ToString("0#"));
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeTodayMission()
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroTodayMissionList.Count; i++)
        {
            CsHeroTodayMission csHeroTodayMission = CsGameData.Instance.MyHeroInfo.HeroTodayMissionList[i];

            if (csHeroTodayMission.TodayMission.TargetCount <= csHeroTodayMission.ProgressCount && !csHeroTodayMission.RewardReceived)
            {
                return true;
            }
        }

        return false;
    }
}