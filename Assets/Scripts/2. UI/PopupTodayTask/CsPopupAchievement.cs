using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupAchievement : CsPopupSub
{
    Transform m_trAchievementFrame;
    Transform m_trButtonPopupClose;

    Text m_textAchievementPoint;

    Slider m_sliderAchievement;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventAchievementRewardReceive += OnEventAchievementRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        m_trAchievementFrame = transform.Find("AchievementFrame");

        Text textAchievement = m_trAchievementFrame.Find("TextAchievement").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAchievement);
        textAchievement.text = CsConfiguration.Instance.GetString("A50_TXT_00002");

        m_textAchievementPoint = m_trAchievementFrame.Find("TextAchievementPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAchievementPoint);
        m_textAchievementPoint.text = CsGameData.Instance.MyHeroInfo.AchievementDailyPoint.ToString("#,##0");

        m_sliderAchievement = m_trAchievementFrame.Find("SliderAchievement").GetComponent<Slider>();
        m_sliderAchievement.maxValue = 100;

        m_trButtonPopupClose = transform.Find("ButtonPopupClose");

        Button buttonPopupClose = m_trButtonPopupClose.GetComponent<Button>();
        buttonPopupClose.onClick.RemoveAllListeners();
        buttonPopupClose.onClick.AddListener(() => OnClickPopupClose());
        buttonPopupClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdateSliderAchievement();
        UpdateButtonRewardItem();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventAchievementRewardReceive -= OnEventAchievementRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAchievementRewardReceive()
    {
        UpdateSliderAchievement();
        UpdateButtonRewardItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupOpen(int nRewardNo)
    {
        UpdateRewardItemPopup(nRewardNo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        m_trButtonPopupClose.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAchievementRewardReceive(System.DateTime dtTime, int nRewardNo)
    {
        CsCommandEventManager.Instance.SendAchievementRewardReceive(dtTime, nRewardNo);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSliderAchievement()
    {
        if (CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo == CsGameData.Instance.AchievementRewardList.Count)
        {
            // 최대레벨
            m_sliderAchievement.value = m_sliderAchievement.maxValue;
        }
        else
        {
            int nValue = CsGameData.Instance.MyHeroInfo.AchievementDailyPoint - (CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo / 4) * 100;
            m_sliderAchievement.value = nValue;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonRewardItem()
    {
        Transform trButtonRewardItemList = m_trAchievementFrame.Find("ButtonRewardList");

        for (int i = 0; i < 4; i++)
        {
            int nRewardNo = (CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo / 4) * 4 + i + 1;

            Button buttonRewardItem = trButtonRewardItemList.Find("ButtonRewardItem" + i).GetComponent<Button>();
            buttonRewardItem.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift01");
            buttonRewardItem.onClick.RemoveAllListeners();
            buttonRewardItem.onClick.AddListener(() => OnClickPopupOpen(nRewardNo));
            buttonRewardItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            buttonRewardItem.transition = Selectable.Transition.ColorTint;
            buttonRewardItem.interactable = true;

            if (CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo == CsGameData.Instance.AchievementRewardList.Count)
            {
                buttonRewardItem.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
                buttonRewardItem.onClick.RemoveAllListeners();
                buttonRewardItem.transition = Selectable.Transition.None;
                buttonRewardItem.interactable = false;

                CsAchievementReward csAchievementReward = CsGameData.Instance.GetAchievementReward(nRewardNo - 4);
                Text textButtonRewardItem = buttonRewardItem.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButtonRewardItem);
                textButtonRewardItem.text = csAchievementReward.RequiredAchievementPoint.ToString("#,##0");
            }
            else
            {
                CsAchievementReward csAchievementReward = CsGameData.Instance.GetAchievementReward(nRewardNo);
                Text textButtonRewardItem = buttonRewardItem.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButtonRewardItem);
                textButtonRewardItem.text = csAchievementReward.RequiredAchievementPoint.ToString("#,##0");

                csAchievementReward = CsGameData.Instance.AchievementRewardList[CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo];

                if (csAchievementReward.RewardNo == nRewardNo &&
                    csAchievementReward.RequiredAchievementPoint <= CsGameData.Instance.MyHeroInfo.AchievementDailyPoint)
                {
                    buttonRewardItem.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift02");
                    buttonRewardItem.onClick.RemoveAllListeners();
                    buttonRewardItem.onClick.AddListener(() => OnClickAchievementRewardReceive(CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardDate, nRewardNo));
                    buttonRewardItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                }

                if (i < (CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo % 4))
                {
                    buttonRewardItem.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_gift03");
                    buttonRewardItem.onClick.RemoveAllListeners();
                    buttonRewardItem.transition = Selectable.Transition.None;
                    buttonRewardItem.interactable = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateRewardItemPopup(int nRewardNo)
    {
        m_trButtonPopupClose.gameObject.SetActive(true);
        Transform trImageBackground = m_trButtonPopupClose.Find("ImageBackground");

        CsAchievementReward csAchievementReward = CsGameData.Instance.GetAchievementReward(nRewardNo);

        Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01004"), csAchievementReward.RequiredAchievementPoint);

        int nNum = (nRewardNo - 1) % 4;

        if (nNum == 0)
        {
            trImageBackground.localPosition = new Vector3(-55.5f, 70, 0);
        }
        else if (nNum == 1)
        {
            trImageBackground.localPosition = new Vector3(87.5f, 70, 0);
        }
        else if (nNum == 2)
        {
            trImageBackground.localPosition = new Vector3(230.5f, 70, 0);
        }
        else if (nNum == 3)
        {
            trImageBackground.localPosition = new Vector3(421.5f, 70, 0);
        }

        for (int i = 0; i < 2; i++)
        {
            trImageBackground.Find("RewardItemGrid" + i).gameObject.SetActive(false);
        }

        for (int i = 0; i < csAchievementReward.AchievementRewardEntryList.Count; i++)
        {
            Transform trRewardItemGrid = trImageBackground.Find("RewardItemGrid" + i);
            Transform trItemSlot = trRewardItemGrid.Find("ItemSlot");
            CsItemReward csItemReward = csAchievementReward.AchievementRewardEntryList[i].ItemReward;

            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

            Text textItemName = trRewardItemGrid.Find("TextItemName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textItemName);
            textItemName.text = csItemReward.Item.Name;

            trRewardItemGrid.gameObject.SetActive(true);
        }
    }
}