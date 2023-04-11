using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport
{
    [SerializeField] GameObject m_goAttendRewardItem;

    Transform m_trAttendReward;
    Transform m_trAttendRewardList;

    Button m_buttonReceive;

    int m_nAttendDay = 0;

    //---------------------------------------------------------------------------------------------------
    void OnEventAttendRewardReceive()
    {
        UpdateAttendRewardItem(m_nAttendDay - 1);
        UpdateReceiveButton();
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceiveAttendReward()
    {
        CsCommandEventManager.Instance.SendAttendRewardReceive(m_nAttendDay);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeAttendReward(Transform trAttendReward)
    {
        // UI Initialize
        m_trAttendReward = trAttendReward;
        m_trAttendRewardList = m_trAttendReward.Find("AttendRewardList");

        Text textReceiveComplete = m_trAttendReward.Find("TextReceiveComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReceiveComplete);
        textReceiveComplete.text = CsConfiguration.Instance.GetString("A37_TXT_00006");

        // 보상 수령 버튼
        m_buttonReceive = m_trAttendReward.Find("ButtonReceive").GetComponent<Button>();
        m_buttonReceive.onClick.RemoveAllListeners();
        m_buttonReceive.onClick.AddListener(() => OnClickReceiveAttendReward());
        m_buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceive = m_buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A37_BTN_00002");

        // 받은 날짜가 달라지면 출석 보상 일차 업데이트
        if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date, CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date) == 1)
        {
            int nMaxDay = CsGameData.Instance.DailyAttendRewardEntryList[CsGameData.Instance.DailyAttendRewardEntryList.Count - 1].Day;

            if (CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay == nMaxDay)
            {
                CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay = 0;
            }
        }

        for (int i = 0; i <= CsGameData.Instance.DailyAttendRewardEntryList.Count; i++)
        {
            int nIndex = i;
            CreateAttendRewardItem(nIndex);
        }

        UpdateReceiveButton();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateAttendRewardItem(int nIndex)
    {
        Transform trAttendRewardItem = Instantiate(m_goAttendRewardItem, m_trAttendRewardList).transform;
        trAttendRewardItem.name = "AttendRewardItem" + nIndex;

        Text textDay = trAttendRewardItem.Find("TextDay").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDay);

        Transform trItemSlot = trAttendRewardItem.Find("ItemSlotReward");

        Button buttonAttendRewardItem = trAttendRewardItem.GetComponent<Button>();
        buttonAttendRewardItem.onClick.RemoveAllListeners();

        if (nIndex == CsGameData.Instance.DailyAttendRewardEntryList.Count)
        {
            textDay.text = CsConfiguration.Instance.GetString("A37_TXT_00002");

            CsItemReward csItemRewardWeek = CsGameConfig.Instance.ItemRewardWeekendAttend;
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemRewardWeek.Item, csItemRewardWeek.ItemOwned, csItemRewardWeek.ItemCount, csItemRewardWeek.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            buttonAttendRewardItem.onClick.AddListener(() => OnClickItemSlot(csItemRewardWeek));
            buttonAttendRewardItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }
        else
        {
            textDay.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01001"), CsGameData.Instance.DailyAttendRewardEntryList[nIndex].Day);
            textDay.color = CsUIData.Instance.ColorWhite;

            CsItemReward csItemRewardAttend = CsGameData.Instance.DailyAttendRewardEntryList[nIndex].ItemReward;
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemRewardAttend.Item, csItemRewardAttend.ItemOwned, csItemRewardAttend.ItemCount, csItemRewardAttend.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            buttonAttendRewardItem.onClick.AddListener(() => OnClickItemSlot(csItemRewardAttend));
            buttonAttendRewardItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            UpdateAttendRewardItem(nIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttendRewardItem(int nIndex)
    {
        Transform trAttendRewardItem = m_trAttendRewardList.Find("AttendRewardItem" + nIndex);

        Transform trImageMark = trAttendRewardItem.Find("ImageMark");
        Transform trImageCheck = trAttendRewardItem.Find("ImageCheck");
        Image imageDim = trAttendRewardItem.Find("ImageCooltime").GetComponent<Image>();

        // 오늘 출석 보상을 받았다면
        if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
        {
            trImageMark.gameObject.SetActive(false);

            // 받은 날짜까지 어둡게 처리
            if (CsGameData.Instance.DailyAttendRewardEntryList[nIndex].Day <= CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay)
            {
                trImageCheck.gameObject.SetActive(true);

                imageDim.fillAmount = 1.0f;
                imageDim.gameObject.SetActive(true);
            }
            else
            {
                trImageCheck.gameObject.SetActive(false);

                imageDim.gameObject.SetActive(false);
            }
        }
        else
        {
            if (CsGameData.Instance.DailyAttendRewardEntryList[nIndex].Day <= CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay)
            {
                trImageMark.gameObject.SetActive(false);
                trImageCheck.gameObject.SetActive(true);

                imageDim.fillAmount = 1.0f;
                imageDim.gameObject.SetActive(true);
            }
            else if (CsGameData.Instance.DailyAttendRewardEntryList[nIndex].Day == (CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay + 1))
            {
                trImageMark.gameObject.SetActive(true);
                trImageCheck.gameObject.SetActive(false);
                imageDim.gameObject.SetActive(false);

                m_nAttendDay = CsGameData.Instance.DailyAttendRewardEntryList[nIndex].Day;
            }
            else
            {
                trImageMark.gameObject.SetActive(false);
                trImageCheck.gameObject.SetActive(false);
                imageDim.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateReceiveButton()
    {
        Text textReceiveComplete = m_trAttendReward.Find("TextReceiveComplete").GetComponent<Text>();

        // 오늘 출석 보상을 받았다면
        if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
        {
            m_buttonReceive.gameObject.SetActive(false);
            textReceiveComplete.gameObject.SetActive(true);
        }
        else
        {
            m_buttonReceive.gameObject.SetActive(true);
            textReceiveComplete.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeAttendReward()
    {
        // 오늘 출석 보상을 받았다면
        if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}