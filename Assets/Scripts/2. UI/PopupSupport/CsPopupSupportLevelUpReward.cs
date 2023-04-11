using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class CsPopupSupport
{
    [SerializeField] GameObject m_goLevelUpRewardItem;

    Transform m_trLevelUpReward;
    Transform m_trLevelUpRewardList;

    int m_nReceiveLevelUpRewardIndex;

    //---------------------------------------------------------------------------------------------------
    void OnEventLevelUpRewardReceive()
    {
        UpdateLevelUpRewardItem(m_nReceiveLevelUpRewardIndex);
        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        for (int i = 0; i < CsGameData.Instance.LevelUpRewardEntryList.Count; i++)
        {
            UpdateLevelUpRewardItem(i);
        }

        UpdateNotice();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceiveLevelUpReward(int nIndex)
    {
        CsCommandEventManager.Instance.SendLevelUpRewardReceive(CsGameData.Instance.LevelUpRewardEntryList[nIndex].EntryId);
        m_nReceiveLevelUpRewardIndex = nIndex;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeLevelUpReward(Transform trLevelUpReward)
    {
        // UI Initialize
        m_trLevelUpReward = trLevelUpReward;
        m_trLevelUpRewardList = m_trLevelUpReward.Find("Scroll View/Viewport/Content");

        Text textInfo = m_trLevelUpReward.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfo);
        textInfo.text = CsConfiguration.Instance.GetString("A37_TXT_00001");

        for (int i = 0; i < CsGameData.Instance.LevelUpRewardEntryList.Count; i++)
        {
            int nIndex = i;
            CreateLevelUpRewardItem(nIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateLevelUpRewardItem(int nIndex)
    {
        Transform trLevelUpRewardItem = Instantiate(m_goLevelUpRewardItem, m_trLevelUpRewardList).transform;
        trLevelUpRewardItem.name = "LevelUpRewardItem" + nIndex;

        List<CsLevelUpRewardItem> listLevelUpRewardItem = CsGameData.Instance.LevelUpRewardEntryList[nIndex].LevelUpRewardItemList;

        for (int i = 0; i < listLevelUpRewardItem.Count; i++)
        {
            Transform trItemSlot = trLevelUpRewardItem.Find("ItemSlotReward" + i);

            CsItemReward csItemReward = listLevelUpRewardItem[i].ItemReward;
            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OnClickItemSlot(csItemReward));
            buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            trItemSlot.gameObject.SetActive(true);
        }

        Text textReceiveComplete = trLevelUpRewardItem.Find("TextReceiptComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReceiveComplete);
        textReceiveComplete.text = CsConfiguration.Instance.GetString("A37_TXT_00003");

        Button buttonReceive = trLevelUpRewardItem.Find("ButtonReceipt").GetComponent<Button>();
        buttonReceive.onClick.RemoveAllListeners();
        buttonReceive.onClick.AddListener(() => OnClickReceiveLevelUpReward(nIndex));
        buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonRecive = buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonRecive);

        UpdateLevelUpRewardItem(nIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLevelUpRewardItem(int nIndex)
    {
        List<CsLevelUpRewardEntry> listLevelUpRewardEntry = CsGameData.Instance.LevelUpRewardEntryList;
        Transform trLevelUpRewardItem = m_trLevelUpRewardList.Find("LevelUpRewardItem" + nIndex);

        Text textReceiveComplete = trLevelUpRewardItem.Find("TextReceiptComplete").GetComponent<Text>();

        Button buttonReceive = trLevelUpRewardItem.Find("ButtonReceipt").GetComponent<Button>();
        Text textButtonRecive = buttonReceive.transform.Find("Text").GetComponent<Text>();

        if (IsReceiveLevelUpReward(listLevelUpRewardEntry[nIndex].EntryId))
        {
            // 아이템 슬롯 딤 처리
            List<CsLevelUpRewardItem> listLevelUpRewardItem = listLevelUpRewardEntry[nIndex].LevelUpRewardItemList;

            for (int i = 0; i < listLevelUpRewardItem.Count; i++)
            {
                Transform trItemSlot = trLevelUpRewardItem.Find("ItemSlotReward" + i);

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
            // 보상을 받을 레벨이 아직 되지 않았다면
            if (CsGameData.Instance.MyHeroInfo.Level < listLevelUpRewardEntry[nIndex].Level)
            {
                // 수령 버튼 비활성화
                CsUIData.Instance.DisplayButtonInteractable(buttonReceive, false);

                // 수령 버튼 텍스트
                textButtonRecive.text = string.Format(CsConfiguration.Instance.GetString("A37_BTN_01001"), listLevelUpRewardEntry[nIndex].Level);
                textButtonRecive.color = CsUIData.Instance.ColorRed;
            }
            else
            {
                // 수령 버튼 텍스트
                textButtonRecive.text = CsConfiguration.Instance.GetString("A37_BTN_00001");
                textButtonRecive.color = CsUIData.Instance.ColorWhite;
            }

            buttonReceive.gameObject.SetActive(true);

            textReceiveComplete.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool IsReceiveLevelUpReward(int nEntryId)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList[i] == nEntryId)
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
    bool CheckNoticeLevelUpReward()
    {
        List<CsLevelUpRewardEntry> list = CsGameData.Instance.LevelUpRewardEntryList.FindAll(a => a.Level <= CsGameData.Instance.MyHeroInfo.Level);

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList.Count; i++)
        {
            list.RemoveAll(a => a.EntryId == CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList[i]);
        }

        if (list.Count > 0)
        {
            return true;
        }

        return false;
    }
}