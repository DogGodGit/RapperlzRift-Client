using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupAttainment : CsPopupSub
{
    [SerializeField]
    GameObject m_goAccomplishmentItem;

    Transform m_trTopPanel;
    Transform m_trLeftPanel;
    Transform m_TrRightPanel;

    Text m_textClear;

    int m_nAccomplishmentCategoryId;
    int m_nRewardListCount;
    bool m_bDisplay;

    List<CsAccomplishment> m_listComplete = new List<CsAccomplishment>();
    List<CsAccomplishment> m_listAccomplishment = new List<CsAccomplishment>();
    List<CsAccomplishment> m_listReceive = new List<CsAccomplishment>();

    enum EnAccomplishmentCategories
    {
        Growth = 1,
        Adventure = 2,
        War = 3,
        Collection = 4,
    }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsAccomplishmentManager.Instance.EventAccomplishmentRewardReceive += OnEventAccomplishmentRewardReceive;
        CsAccomplishmentManager.Instance.EventAccomplishmentRewardReceiveAll += OnEventAccomplishmentRewardReceiveAll;
        CsAccomplishmentManager.Instance.EventAccMonsterKillCountUpdated += OnEventAccMonsterKillCountUpdated;
        CsAccomplishmentManager.Instance.EventAccNationWarCommanderKillCountUpdated += OnEventAccNationWarCommanderKillCountUpdated;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsAccomplishmentManager.Instance.EventAccomplishmentRewardReceive -= OnEventAccomplishmentRewardReceive;
        CsAccomplishmentManager.Instance.EventAccomplishmentRewardReceiveAll -= OnEventAccomplishmentRewardReceiveAll;
        CsAccomplishmentManager.Instance.EventAccMonsterKillCountUpdated -= OnEventAccMonsterKillCountUpdated;
        CsAccomplishmentManager.Instance.EventAccNationWarCommanderKillCountUpdated -= OnEventAccNationWarCommanderKillCountUpdated;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccomplishmentRewardReceive()
    {
        UpdateTop();
        UpdateAccomplishmentItem();

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A82_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccomplishmentRewardReceiveAll()
    {
        UpdateTop();
        UpdateAccomplishmentItem();

        if (m_nRewardListCount == CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A82_TXT_02003"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A82_TXT_02002"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccMonsterKillCountUpdated()
    {
        UpdateAccomplishmentItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccNationWarCommanderKillCountUpdated()
    {
        UpdateAccomplishmentItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonReceive(int nAccomplishmentId)
    {
        CsAccomplishmentManager.Instance.SendAccomplishmentRewardReceive(nAccomplishmentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonReceiveAll()
    {
        m_nRewardListCount = CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count;
        CsAccomplishmentManager.Instance.SendAccomplishmentRewardReceiveAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedToggleContent(Toggle toggleContent, int nAccomplishmentCategoryId)
    {
        m_nAccomplishmentCategoryId = nAccomplishmentCategoryId;

        if (toggleContent.isOn)
        {
            Transform trContentAccomplishment = m_TrRightPanel.Find("ScrollViewAccomplishment/Viewport/Content");
            trContentAccomplishment.localPosition = Vector3.zero;

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            UpdateAccomplishmentItem();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedShowReceiveAccomplishment(bool bIson)
    {
        m_bDisplay = bIson;
        UpdateAccomplishmentItem();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trTopPanel = transform.Find("TopPanel");
        m_trLeftPanel = transform.Find("LeftPanel");
        m_TrRightPanel = transform.Find("RightPanel");

        // TopPanel
        Slider sliderAccomplishmentProgress = m_trTopPanel.Find("SliderAccomplishment").GetComponent<Slider>();
        sliderAccomplishmentProgress.maxValue = CsGameData.Instance.AccomplishmentList.Count;
        sliderAccomplishmentProgress.value = CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count;

        Button buttonReceiveAll = m_trTopPanel.Find("ButtonReceiveAll").GetComponent<Button>();
        buttonReceiveAll.onClick.RemoveAllListeners();
        buttonReceiveAll.onClick.AddListener(() => OnClickButtonReceiveAll());
        buttonReceiveAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAccomplishmentProgress = m_trTopPanel.Find("TextAccomplishmentProgress").GetComponent<Text>();
        Text textAccomplishmentScore = m_trTopPanel.Find("TextAccomplishmentScore").GetComponent<Text>();
        Text textButtonReceiveAll = buttonReceiveAll.transform.Find("Text").GetComponent<Text>();

        CsUIData.Instance.SetFont(textAccomplishmentProgress);
        CsUIData.Instance.SetFont(textAccomplishmentScore);
        CsUIData.Instance.SetFont(textButtonReceiveAll);

        textAccomplishmentProgress.text = CsConfiguration.Instance.GetString("A82_TXT_00001");
        textAccomplishmentScore.text = CsConfiguration.Instance.GetString("A82_TXT_00002");
        textButtonReceiveAll.text = CsConfiguration.Instance.GetString("A82_BTN_00001");

        UpdateTop();

        // LeftPanel
        Transform trContent = m_trLeftPanel.Find("Content");

        for (int i = 0; i < CsGameData.Instance.AccomplishmentCategoryList.Count; i++)
        {
            Toggle toggleContent = trContent.Find("ToggleContent" + i).GetComponent<Toggle>();
            toggleContent.onValueChanged.RemoveAllListeners();
            int nCategoryId = CsGameData.Instance.AccomplishmentCategoryList[i].CategoryId;
            toggleContent.onValueChanged.AddListener((bison) => OnValueChangedToggleContent(toggleContent, nCategoryId));

            Text textContent = toggleContent.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textContent);
            textContent.text = CsGameData.Instance.AccomplishmentCategoryList[i].Name;

            Transform trImageNotice = toggleContent.transform.Find("ImageNotice");
            trImageNotice.gameObject.SetActive(false);
        }

        Toggle toggleShowReceiveAccomplishment = m_trLeftPanel.Find("ToggleShowReceiveAccomplishment").GetComponent<Toggle>();
        toggleShowReceiveAccomplishment.onValueChanged.RemoveAllListeners();
        toggleShowReceiveAccomplishment.onValueChanged.AddListener((bIson) => OnValueChangedShowReceiveAccomplishment(bIson));
        toggleShowReceiveAccomplishment.onValueChanged.AddListener((bIson) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textToggleShowReceiveAccomplishment = toggleShowReceiveAccomplishment.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textToggleShowReceiveAccomplishment);
        textToggleShowReceiveAccomplishment.text = CsConfiguration.Instance.GetString("A82_TXT_00003");

        // RightPanel
        m_nAccomplishmentCategoryId = (int)EnAccomplishmentCategories.Growth;
        m_bDisplay = false;

        m_textClear = m_TrRightPanel.Find("TextClear").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textClear);
        m_textClear.text = CsConfiguration.Instance.GetString("A82_TXT_00005");

        UpdateAccomplishmentItem();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTop()
    {
        Slider sliderAccomplishmentProgress = m_trTopPanel.Find("SliderAccomplishment").GetComponent<Slider>();
        sliderAccomplishmentProgress.maxValue = CsGameData.Instance.AccomplishmentList.Count;
        sliderAccomplishmentProgress.value = CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count;

        Text textTotalScore = m_trTopPanel.Find("TextTotalScore").GetComponent<Text>();
        Text textPercent = sliderAccomplishmentProgress.transform.Find("TextPercent").GetComponent<Text>();

        CsUIData.Instance.SetFont(textTotalScore);
        CsUIData.Instance.SetFont(textPercent);

        int nScore = 0;
        for (int i = 0; i < CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count; i++)
        {
            CsAccomplishment csAccomplishment = CsGameData.Instance.AccomplishmentList.Find(a => a.AccomplishmentId == CsAccomplishmentManager.Instance.RewardedAccomplishmentList[i]);

            if (csAccomplishment != null)
            {
                //nScore += csAccomplishment.Point;
            }
        }

        textTotalScore.text = nScore.ToString("#,##0");

        int nPercent = (int)((float)CsAccomplishmentManager.Instance.RewardedAccomplishmentList.Count / CsGameData.Instance.AccomplishmentList.Count * 100);
        textPercent.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_PER"), nPercent);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAccomplishmentItem()
    {
        CheckAccomplishmentCategoryNotice();

        m_listAccomplishment.Clear();
        m_listComplete.Clear();
        m_listReceive.Clear();

        Transform trContentAccomplishment = m_TrRightPanel.Find("ScrollViewAccomplishment/Viewport/Content");
        Transform trContent = m_trLeftPanel.Find("Content");

        // 초기화
        for (int i = 0; i < trContentAccomplishment.childCount; i++)
        {
            trContentAccomplishment.GetChild(i).gameObject.SetActive(false);
        }

        // CategoryId
        List<CsAccomplishment> listCsAccomplishment = CsGameData.Instance.AccomplishmentList.FindAll(a => a.AccomplishmentCategory.CategoryId == m_nAccomplishmentCategoryId);
        Transform trAccomplishmentItem = null;

        long lProgressCount = 0;
        int nMaxActivateCreatureCard = 0;

        for (int i = 0; i < listCsAccomplishment.Count; i++)
        {
            switch ((EnAccomplishment)listCsAccomplishment[i].Type)
            {
                case EnAccomplishment.MaxLevel:
                    lProgressCount = CsGameData.Instance.MyHeroInfo.Level;

                    break;
                case EnAccomplishment.MaxBattlePower:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxBattlePower;

                    break;
                case EnAccomplishment.MaxGold:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxGold;

                    break;
                case EnAccomplishment.MaxMainQuest:
                    if (CsMainQuestManager.Instance.MainQuest != null)
                    {
                        lProgressCount = CsMainQuestManager.Instance.MainQuest.MainQuestNo - 1;
                    }
                    else
                    {
                        lProgressCount = CsGameData.Instance.MainQuestList.Count;
                    }

                    break;
                case EnAccomplishment.AccMonsterKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccMonsterKillCount;

                    break;
                case EnAccomplishment.AccSoulCoveterPlay:
                    lProgressCount = CsAccomplishmentManager.Instance.AccSoulCoveterPlayCount;

                    break;
                case EnAccomplishment.AccEpicBaitItemUse:
                    lProgressCount = CsAccomplishmentManager.Instance.AccEpicBaitItemUseCount;

                    break;
                case EnAccomplishment.AccLegendBaitItemUse:
                    lProgressCount = CsAccomplishmentManager.Instance.AccLegendBaitItemUseCount;

                    break;
                case EnAccomplishment.AccNationWarWin:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarWinCount;

                    break;
                case EnAccomplishment.AccNationWarKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarKillCount;

                    break;
                case EnAccomplishment.AccNationWarCommanderKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarCommanderKillCount;

                    break;
                case EnAccomplishment.AccNationWarImmediateRevival:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarImmediateRevivalCount;

                    break;
                case EnAccomplishment.MaxAcquisitionMainGearGrade:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade;

                    break;
                case EnAccomplishment.MaxEquippedMainGearEnchantLevel:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxEquippedMainGearEnchantLevel;

                    break;

                case EnAccomplishment.ActivateCreatureCardCollection:
                    lProgressCount = 0;

                    if (CsGameData.Instance.GetCreatureCardCategory((int)listCsAccomplishment[i].ObjectiveValue) != null)
                    {
                        int nCategoryId = CsGameData.Instance.GetCreatureCardCategory((int)listCsAccomplishment[i].ObjectiveValue).CategoryId;

                        List<CsCreatureCardCollection> listCsCreatureCardCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == nCategoryId);
                        nMaxActivateCreatureCard = listCsCreatureCardCollection.Count;

                        for (int j = 0; j < listCsCreatureCardCollection.Count; j++)
                        {
                            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCsCreatureCardCollection[j].CollectionId))
                            {
                                lProgressCount++;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }


                    break;
            }

            trAccomplishmentItem = trContentAccomplishment.Find("AccomplishmentItem" + listCsAccomplishment[i].AccomplishmentId);

            if (trAccomplishmentItem == null)
            {
                trAccomplishmentItem = Instantiate(m_goAccomplishmentItem, trContentAccomplishment).transform;
                trAccomplishmentItem.name = "AccomplishmentItem" + listCsAccomplishment[i].AccomplishmentId;
            }

            // 포인트
            Image imageWreath = trAccomplishmentItem.Find("ImageWreath").GetComponent<Image>();

            Text textImageWreath = imageWreath.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textImageWreath);
            //textImageWreath.text = listCsAccomplishment[i].Point.ToString();

            Transform trAccomplishmentInfo = trAccomplishmentItem.Find("AccomplishmentInfo");

            Text textAccomplishmentName = trAccomplishmentInfo.Find("TextName").GetComponent<Text>();
            Text textAccomplishmentObjective = trAccomplishmentInfo.Find("TextObjective").GetComponent<Text>();

            CsUIData.Instance.SetFont(textAccomplishmentName);
            CsUIData.Instance.SetFont(textAccomplishmentObjective);

            textAccomplishmentName.text = listCsAccomplishment[i].Name;

            if ((EnAccomplishment)listCsAccomplishment[i].Type == EnAccomplishment.MaxAcquisitionMainGearGrade ||
                (EnAccomplishment)listCsAccomplishment[i].Type == EnAccomplishment.MaxMainQuest ||
                (EnAccomplishment)listCsAccomplishment[i].Type == EnAccomplishment.ActivateCreatureCardCollection)
            {
                switch ((EnAccomplishment)listCsAccomplishment[i].Type)
                {
                    case EnAccomplishment.MaxMainQuest:
                        CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest((int)listCsAccomplishment[i].ObjectiveValue);

                        if (csMainQuest == null)
                        {
                            textAccomplishmentObjective.text = "";
                        }
                        else
                        {
                            textAccomplishmentObjective.text = string.Format(listCsAccomplishment[i].ObjectiveText, csMainQuest.Title);
                        }
                        break;

                    case EnAccomplishment.MaxAcquisitionMainGearGrade:
                        CsMainGearGrade csMainGearGrade = CsGameData.Instance.GetMainGearGrade((int)listCsAccomplishment[i].ObjectiveValue);

                        if (csMainGearGrade == null)
                        {
                            textAccomplishmentObjective.text = "";
                        }
                        else
                        {
                            textAccomplishmentObjective.text = string.Format(listCsAccomplishment[i].ObjectiveText, csMainGearGrade.Name);
                        }
                        break;

                    case EnAccomplishment.ActivateCreatureCardCollection:
                        CsCreatureCardCategory csCreatureCardCategory = CsGameData.Instance.GetCreatureCardCategory((int)listCsAccomplishment[i].ObjectiveValue);

                        if (csCreatureCardCategory == null)
                        {
                            textAccomplishmentObjective.text = "";
                        }
                        else
                        {
                            textAccomplishmentObjective.text = string.Format(listCsAccomplishment[i].ObjectiveText, csCreatureCardCategory.Name);
                        }
                        break;
                }
            }
            else
            {
                textAccomplishmentObjective.text = string.Format(listCsAccomplishment[i].ObjectiveText, listCsAccomplishment[i].ObjectiveValue);
            }

            Slider sliderObjective = trAccomplishmentInfo.Find("Slider").GetComponent<Slider>();

            if (listCsAccomplishment[i].Type == (int)EnAccomplishment.ActivateCreatureCardCollection)
            {
                sliderObjective.maxValue = nMaxActivateCreatureCard;
            }
            else
            {
                sliderObjective.maxValue = listCsAccomplishment[i].ObjectiveValue;
            }

            sliderObjective.value = lProgressCount;

            Text textPercent = sliderObjective.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPercent);

            if (sliderObjective.value > sliderObjective.maxValue)
            {
                sliderObjective.value = sliderObjective.maxValue;
            }

            int nPercent = (int)((float)sliderObjective.value / sliderObjective.maxValue * 100);
            textPercent.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TXT_PER"), nPercent);

            Button buttonReceive = trAccomplishmentItem.Find("ButtonReceive").GetComponent<Button>();
            buttonReceive.onClick.RemoveAllListeners();
            int nAccomplishmentId = listCsAccomplishment[i].AccomplishmentId;
            buttonReceive.onClick.AddListener(() => OnClickButtonReceive(nAccomplishmentId));
            buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            CsUIData.Instance.DisplayButtonInteractable(buttonReceive, lProgressCount >= sliderObjective.maxValue);

            Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonReceive);
            textButtonReceive.text = CsConfiguration.Instance.GetString("A82_BTN_00002");

            Text textReceive = trAccomplishmentItem.Find("TextReceive").GetComponent<Text>();
            CsUIData.Instance.SetFont(textReceive);
            textReceive.text = CsConfiguration.Instance.GetString("A82_TXT_00004");

            Transform trItemSlot = trAccomplishmentItem.Find("ItemSlot");

            Text textRewardTitle = trAccomplishmentItem.Find("TextRewardTitle").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRewardTitle);

            Text textProgress = trAccomplishmentItem.Find("TextProgress").GetComponent<Text>();
            CsUIData.Instance.SetFont(textProgress);
            textProgress.text = CsConfiguration.Instance.GetString("A82_TXT_00006");

            if (listCsAccomplishment[i].RewardType == 1)
            {
                // 칭호
                trItemSlot.gameObject.SetActive(false);

                CsTitle csTitle = CsGameData.Instance.GetTitle(listCsAccomplishment[i].RewardTitleId);
                textRewardTitle.text = string.Format("<color={0}>{1}</color>", csTitle.TitleGrade.ColorCode, csTitle.Name);
                textRewardTitle.gameObject.SetActive(true);

                buttonReceive.gameObject.SetActive(false);
                textProgress.gameObject.SetActive(true);
            }
            else if (listCsAccomplishment[i].RewardType == 2)
            {
                // 아이템
                textRewardTitle.gameObject.SetActive(false);

                CsItemReward csItemReward = listCsAccomplishment[i].ItemReward;
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Small, false);
                trItemSlot.gameObject.SetActive(true);

                textProgress.gameObject.SetActive(false);
                buttonReceive.gameObject.SetActive(true);
            }

            if (lProgressCount >= sliderObjective.maxValue)
            {
                // 수령 가능
                if (CsAccomplishmentManager.Instance.RewardedAccomplishmentList.FindIndex(a => a == listCsAccomplishment[i].AccomplishmentId) == -1)
                {
                    if (listCsAccomplishment[i].RewardType == 1)
                    {
                        // 칭호
                        textProgress.gameObject.SetActive(true);
                    }
                    else
                    {
                        // 아이템
                        buttonReceive.gameObject.SetActive(true);
                    }

                    // 아직 수령 안한 상태
                    textReceive.gameObject.SetActive(false);
                    trAccomplishmentItem.gameObject.SetActive(true);

                    m_listComplete.Add(listCsAccomplishment[i]);
                }
                else
                {
                    if (listCsAccomplishment[i].RewardType == 1)
                    {
                        // 칭호
                        textProgress.gameObject.SetActive(false);
                    }
                    else
                    {
                        // 아이템
                        buttonReceive.gameObject.SetActive(false);
                    }

                    // 수령한 상태
                    textReceive.gameObject.SetActive(true);
                    trAccomplishmentItem.gameObject.SetActive(m_bDisplay);

                    m_listReceive.Add(listCsAccomplishment[i]);
                }
            }
            else
            {
                // 수령 불가 도전 상태
                if (listCsAccomplishment[i].RewardType == 1)
                {
                    // 칭호
                    textProgress.gameObject.SetActive(true);
                }
                else
                {
                    // 아이템
                    buttonReceive.gameObject.SetActive(true);
                }

                textReceive.gameObject.SetActive(false);
                trAccomplishmentItem.gameObject.SetActive(true);

                m_listAccomplishment.Add(listCsAccomplishment[i]);
            }
        }

        bool bClear = true;

        for (int i = 0; i < trContentAccomplishment.childCount; i++)
        {
            if (trContentAccomplishment.GetChild(i).gameObject.activeSelf)
            {
                bClear = false;
                break;
            }
        }

        m_textClear.gameObject.SetActive(bClear);

        int nStart = 0;

        for (int i = 0; i < m_listComplete.Count; i++)
        {
            trAccomplishmentItem = trContentAccomplishment.Find("AccomplishmentItem" + m_listComplete[i].AccomplishmentId);
            trAccomplishmentItem.SetSiblingIndex(i);
        }

        nStart = m_listComplete.Count;

        for (int i = 0; i < m_listAccomplishment.Count; i++)
        {
            trAccomplishmentItem = trContentAccomplishment.Find("AccomplishmentItem" + m_listAccomplishment[i].AccomplishmentId);
            trAccomplishmentItem.SetSiblingIndex(i + nStart);
        }

        nStart = m_listComplete.Count + m_listAccomplishment.Count;

        for (int i = 0; i < m_listReceive.Count; i++)
        {
            trAccomplishmentItem = trContentAccomplishment.Find("AccomplishmentItem" + m_listReceive[i].AccomplishmentId);
            trAccomplishmentItem.SetSiblingIndex(i + nStart);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckAccomplishmentCategoryNotice()
    {
        Transform trContent = m_trLeftPanel.Find("Content");

        for (int i = 0; i < CsGameData.Instance.AccomplishmentCategoryList.Count; i++)
        {
            Transform trImageNotice = trContent.Find("ToggleContent" + i + "/ImageNotice");

            if (trImageNotice.gameObject.activeSelf)
            {
                trImageNotice.gameObject.SetActive(false);
            }
            else
            {
                continue;
            }
        }

        List<CsAccomplishment> listCsAccomplishment = CsGameData.Instance.AccomplishmentList;

        long lProgressCount = 0;
        int nMaxActivateCreatureCard = 0;

        for (int i = 0; i < listCsAccomplishment.Count; i++)
        {
            switch ((EnAccomplishment)listCsAccomplishment[i].Type)
            {
                case EnAccomplishment.MaxLevel:
                    lProgressCount = CsGameData.Instance.MyHeroInfo.Level;

                    break;

                case EnAccomplishment.MaxBattlePower:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxBattlePower;

                    break;

                case EnAccomplishment.MaxGold:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxGold;

                    break;

                case EnAccomplishment.MaxMainQuest:
                    if (CsMainQuestManager.Instance.MainQuest != null)
                    {
                        lProgressCount = CsMainQuestManager.Instance.MainQuest.MainQuestNo - 1;
                    }
                    else
                    {
                        lProgressCount = CsGameData.Instance.MainQuestList.Count;
                    }

                    break;

                case EnAccomplishment.AccMonsterKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccMonsterKillCount;

                    break;

                case EnAccomplishment.AccSoulCoveterPlay:
                    lProgressCount = CsAccomplishmentManager.Instance.AccSoulCoveterPlayCount;

                    break;

                case EnAccomplishment.AccEpicBaitItemUse:
                    lProgressCount = CsAccomplishmentManager.Instance.AccEpicBaitItemUseCount;

                    break;

                case EnAccomplishment.AccLegendBaitItemUse:
                    lProgressCount = CsAccomplishmentManager.Instance.AccLegendBaitItemUseCount;

                    break;

                case EnAccomplishment.AccNationWarWin:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarWinCount;

                    break;

                case EnAccomplishment.AccNationWarKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarKillCount;

                    break;

                case EnAccomplishment.AccNationWarCommanderKill:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarCommanderKillCount;

                    break;

                case EnAccomplishment.AccNationWarImmediateRevival:
                    lProgressCount = CsAccomplishmentManager.Instance.AccNationWarImmediateRevivalCount;

                    break;

                case EnAccomplishment.MaxAcquisitionMainGearGrade:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade;

                    break;

                case EnAccomplishment.MaxEquippedMainGearEnchantLevel:
                    lProgressCount = CsAccomplishmentManager.Instance.MaxEquippedMainGearEnchantLevel;

                    break;

                case EnAccomplishment.ActivateCreatureCardCollection:
                    lProgressCount = 0;

                    if (CsGameData.Instance.GetCreatureCardCategory((int)listCsAccomplishment[i].ObjectiveValue) != null)
                    {
                        int nCategoryId = CsGameData.Instance.GetCreatureCardCategory((int)listCsAccomplishment[i].ObjectiveValue).CategoryId;

                        List<CsCreatureCardCollection> listCsCreatureCardCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == nCategoryId);
                        nMaxActivateCreatureCard = listCsCreatureCardCollection.Count;

                        for (int j = 0; j < listCsCreatureCardCollection.Count; j++)
                        {
                            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCsCreatureCardCollection[j].CollectionId))
                            {
                                lProgressCount++;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    break;
            }

            long lMaxCount = 0;

            if ((EnAccomplishment)listCsAccomplishment[i].Type == EnAccomplishment.ActivateCreatureCardCollection)
            {
                lMaxCount = nMaxActivateCreatureCard;
            }
            else
            {
                lMaxCount = listCsAccomplishment[i].ObjectiveValue;
            }

            if (lProgressCount >= lMaxCount)
            {
                // 수령 가능
                if (CsAccomplishmentManager.Instance.RewardedAccomplishmentList.FindIndex(a => a == listCsAccomplishment[i].AccomplishmentId) == -1)
                {
                    Transform trImageNotice = trContent.Find("ToggleContent" + (listCsAccomplishment[i].AccomplishmentCategory.CategoryId - 1) + "/ImageNotice");

                    if (!trImageNotice.gameObject.activeSelf)
                    {
                        trImageNotice.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(CsAccomplishment x, CsAccomplishment y)
    {
        if (x.AccomplishmentId > y.AccomplishmentId) return 1;
        else if (x.AccomplishmentId < y.AccomplishmentId) return -1;
        return 0;
    }
}