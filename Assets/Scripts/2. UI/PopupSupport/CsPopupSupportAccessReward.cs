using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport
{
    [SerializeField] GameObject m_goAccessRewardItem;

    Transform m_trAccessReward;
    Transform m_trAccessRewardList;

    Text m_textAccessTimer;

    int m_nReceiveAccessRewardIndex;

    //---------------------------------------------------------------------------------------------------
    void OnEventDailyAccessTimeRewardReceive()
    {
        UpdateAccessRewardItem(m_nReceiveAccessRewardIndex);
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceiveAccessReward(int nIndex)
    {
        CsCommandEventManager.Instance.SendDailyAccessTimeRewardReceive(CsGameData.Instance.AccessRewardEntryList[nIndex].EntryId);
        m_nReceiveAccessRewardIndex = nIndex;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeAccessReward(Transform trAccessReward)
    {
        // UI Initialize
        m_trAccessReward = trAccessReward;
        m_trAccessRewardList = m_trAccessReward.Find("Scroll View/Viewport/Content");

        // 오늘 접속 시간 텍스트
        Text textTodayAccessTime = m_trAccessReward.Find("TextTodayAccessTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTodayAccessTime);
        textTodayAccessTime.text = CsConfiguration.Instance.GetString("A37_TXT_00004");

        m_textAccessTimer = m_trAccessReward.Find("TextAccessTimer").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAccessTimer);

        for (int i = 0; i < CsGameData.Instance.AccessRewardEntryList.Count; i++)
        {
            int nIndex = i;
            CreateAccessRewardItem(nIndex);
        }

        UpdateTextAccessTime();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateAccessRewardItem(int nIndex)
    {
        Transform trAccessRewardItem = Instantiate(m_goAccessRewardItem, m_trAccessRewardList).transform;
        trAccessRewardItem.name = "AccessRewardItem" + nIndex;

        List<CsAccessRewardItem> listAccessRewardItem = CsGameData.Instance.AccessRewardEntryList[nIndex].AccessRewardItemList;

        for (int i = 0; i < listAccessRewardItem.Count; i++)
        {
            Transform trItemSlot = trAccessRewardItem.Find("ItemSlotReward" + i);
            CsItemReward csItemReward = listAccessRewardItem[i].ItemReward;
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(csItemReward));
            buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            trItemSlot.gameObject.SetActive(true);
        }

        Text textRemainingTime = trAccessRewardItem.Find("TextRemainingTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemainingTime);
        textRemainingTime.text = CsConfiguration.Instance.GetString("A37_TXT_00005");

        Text textRemainingTimer = trAccessRewardItem.Find("TextTimer").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRemainingTimer);
        textRemainingTimer.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01003"), "00", "00", "00");

        Text textReceiveComplete = trAccessRewardItem.Find("TextReceiptComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReceiveComplete);
        textReceiveComplete.text = CsConfiguration.Instance.GetString("A37_TXT_00003");

        Button buttonReceive = trAccessRewardItem.Find("ButtonReceipt").GetComponent<Button>();
        buttonReceive.onClick.RemoveAllListeners();
        buttonReceive.onClick.AddListener(() => OnClickReceiveAccessReward(nIndex));
        buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A37_BTN_00001");

        UpdateAccessRewardItem(nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAccessRewardItem(int nIndex)
    {
        List<CsAccessRewardEntry> listAccessRewardEntry = CsGameData.Instance.AccessRewardEntryList;
        Transform trAccessRewardItem = m_trAccessRewardList.Find("AccessRewardItem" + nIndex);

        Text textReceiveComplete = trAccessRewardItem.Find("TextReceiptComplete").GetComponent<Text>();

        Button buttonReceive = trAccessRewardItem.Find("ButtonReceipt").GetComponent<Button>();
        //Text textButtonRecive = buttonReceive.transform.Find("Text").GetComponent<Text>();

        // 보상을 받았다면
        if (IsReceiveAccessReward(listAccessRewardEntry[nIndex].EntryId))
        {
            // 아이템 슬롯 딤 처리
            List<CsAccessRewardItem> listAccessRewardItem = listAccessRewardEntry[nIndex].AccessRewardItemList;

            for (int j = 0; j < listAccessRewardItem.Count; j++)
            {
                Transform trItemSlot = trAccessRewardItem.Find("ItemSlotReward" + j);

                Image imageDim = trItemSlot.Find("ImageCooltime").GetComponent<Image>();
                imageDim.fillAmount = 1.0f;
                imageDim.gameObject.SetActive(true);
            }

            // 버튼 비활성화
            buttonReceive.gameObject.SetActive(false);

            textReceiveComplete.gameObject.SetActive(true);
        }
        // 보상을 받지 않았다면
        else
        {
            // 시간이 되지 않았다면
            if (CsGameData.Instance.MyHeroInfo.DailyAccessTime < listAccessRewardEntry[nIndex].AccessTime)
            {
                // 수령 버튼 비활성화
                CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonReceive, true);
            }

            buttonReceive.gameObject.SetActive(true);
            textReceiveComplete.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool IsReceiveAccessReward(int nEntryId)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList[i] == nEntryId)
            {
                return true;
            }
            else
            {
                continue;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextAccessTime()
    {
        System.TimeSpan tsDailyAccessTime = System.TimeSpan.FromSeconds(CsGameData.Instance.MyHeroInfo.DailyAccessTime);
        m_textAccessTimer.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01002"), tsDailyAccessTime.Hours.ToString("0#"), tsDailyAccessTime.Minutes.ToString("0#"), tsDailyAccessTime.Seconds.ToString("0#"));

        for (int i = 0; i < CsGameData.Instance.AccessRewardEntryList.Count; i++)
        {
            Transform trAccessRewardItem = m_trAccessRewardList.Find("AccessRewardItem" + i);

            Text textRemainingTimer = trAccessRewardItem.Find("TextTimer").GetComponent<Text>();

            float fRemainingTime = CsGameData.Instance.AccessRewardEntryList[i].AccessTime - Mathf.FloorToInt((float)tsDailyAccessTime.TotalSeconds);

            Button buttonReceive = trAccessRewardItem.Find("ButtonReceipt").GetComponent<Button>();

            if (buttonReceive.interactable)
            {
                // 수령 버튼 활성화
            }
            else
            {
                TimeSpan tsRemainingTime;

                if (fRemainingTime < 1.0f)
                {
                    fRemainingTime = 0;
                    tsRemainingTime = TimeSpan.FromSeconds(fRemainingTime);

                    UpdateAccessRewardItem(i);
                }
                else
                {
                    tsRemainingTime = TimeSpan.FromSeconds(fRemainingTime);
                }

                textRemainingTimer.text = string.Format(CsConfiguration.Instance.GetString("A37_TXT_01003"), tsRemainingTime.Hours.ToString("0#"), tsRemainingTime.Minutes.ToString("0#"), tsRemainingTime.Seconds.ToString("0#"));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNoticeAccessReward()
    {
        List<CsAccessRewardEntry> list = CsGameData.Instance.AccessRewardEntryList.FindAll(a => a.AccessTime <= CsGameData.Instance.MyHeroInfo.DailyAccessTime);

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList.Count; i++)
        {
            list.RemoveAll(a => a.EntryId == CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList[i]);
        }

        if (list.Count > 0)
        {
            return true;
        }

        return false;
    }
}