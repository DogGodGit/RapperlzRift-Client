using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public enum EnOpen7DayMissionType
{
    Level = 1,
    StoryDungeon = 2,
    SubGearLevel = 3,
    ExpDungeon = 4,
    BountyHunter = 5,
    SoulStoneLevel = 6,
    BattlePower = 7,
    ProofOfValor = 8,
    GuildMission = 9,
    MainGearEnchantLevel = 10,
    Fishing = 11,
    Rank = 12,
    SecretLetter = 13,
    MysteryBox = 14,
    DimensionRaid = 15,
    HolyWar = 16,
}

public class CsPopupOpen7Day : CsUpdateableMonoBehaviour
{
    GameObject m_goOpen7DayMissionItem;
    GameObject m_goPopupCalculator;

    Transform m_trCanvas2;
    Transform m_trPopupList;
    Transform m_trCalculator;

    Transform m_trTopFrame;
    Transform m_trToggleList;
    Transform m_trContent;
    Transform m_trOpen7DayProduct;

    Button m_buttonReceive;

    CsPopupCalculator m_csPopupCalculator;
    CsOpen7DayEventProduct m_csOpen7DayEventProduct;

    int m_nSelectDay = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventOpen7DayEventMissionRewardReceive += OnEventOpen7DayEventMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventOpen7DayEventProductBuy += OnEventOpen7DayEventProductBuy;
        CsGameEventUIToUI.Instance.EventOpen7DayEventProgressCountUpdated += OnEventOpen7DayEventProgressCountUpdated;
        CsGameEventUIToUI.Instance.EventOpen7DayEventRewardReceive += OnEventOpen7DayEventRewardReceive;

        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventOpen7DayEventMissionRewardReceive -= OnEventOpen7DayEventMissionRewardReceive;
        CsGameEventUIToUI.Instance.EventOpen7DayEventProductBuy -= OnEventOpen7DayEventProductBuy;
        CsGameEventUIToUI.Instance.EventOpen7DayEventProgressCountUpdated -= OnEventOpen7DayEventProgressCountUpdated;
        CsGameEventUIToUI.Instance.EventOpen7DayEventRewardReceive -= OnEventOpen7DayEventRewardReceive;

        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopupClose();
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventOpen7DayEventMissionRewardReceive()
    {
        LoadOpen7DayMissions(m_nSelectDay);
        UpdateTopFrame();
        CheckOpen7Day();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpen7DayEventProductBuy()
    {
        UpdateOpen7DayProduct(m_nSelectDay);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpen7DayEventProgressCountUpdated()
    {
        LoadOpen7DayMissions(m_nSelectDay);
        CheckOpen7Day();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpen7DayEventRewardReceive()
    {
        UpdateTopFrame();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        UpdateToggleList(false);
        CheckOpen7Day();
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDay(Toggle toggle, int nDay)
    {
        Text textToggle = toggle.transform.Find("Text").GetComponent<Text>();

        if (toggle.isOn)
        {
            m_nSelectDay = nDay;

            m_trContent.localPosition = new Vector3(0, 0, 0);
            LoadOpen7DayMissions(nDay);
            UpdateOpen7DayProduct(nDay);

            textToggle.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textToggle.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMissionRewardReceive(int nMissionId, bool bReceive)
    {
        if (bReceive)
        {
            if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(CsGameData.Instance.GetOpen7DayEventMission(nMissionId).Open7DayEventMissionRewardList.Select(reward => reward.ItemReward)))
            {
                CsCommandEventManager.Instance.SendOpen7DayEventMissionRewardReceive(nMissionId);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A97_TXT_03001"));
            }
        }
        else
        {
            CsOpen7DayEventMission csOpen7DayEventMission = CsGameData.Instance.GetOpen7DayEventMission(nMissionId);

            if (csOpen7DayEventMission == null)
            {
                return;
            }
            else
            {
                switch ((EnOpen7DayMissionType)csOpen7DayEventMission.Type)
                {
                    case EnOpen7DayMissionType.Level:
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.StoryDungeon:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.StoryDungeonList[0].RequiredHeroMinLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                            PopupClose();
                        }
                        else
                        {
                            if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                            {
                                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
                                StartCoroutine(LoadDungeonSubMenu((EnOpen7DayMissionType)csOpen7DayEventMission.Type));
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                                PopupClose();
                            }
                        }
                        break;

                    case EnOpen7DayMissionType.SubGearLevel:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearLevelUp))
                        {
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearLevelUp);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.ExpDungeon:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                            PopupClose();
                        }
                        else
                        {
                            if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                            {
                                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                                StartCoroutine(LoadDungeonSubMenu((EnOpen7DayMissionType)csOpen7DayEventMission.Type));
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                                PopupClose();
                            }
                        }
                        break;

                    case EnOpen7DayMissionType.BountyHunter:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.BountyHunterQuestRequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            if (CsBountyHunterQuestManager.Instance.BountyHunterQuest == null)
                            {
                                if (CsBountyHunterQuestManager.Instance.BountyHunterQuestDailyStartCount < CsGameConfig.Instance.BountyHunterQuestMaxCount)
                                {
                                    if (UseBountyHunterItemCheck() != 0)
                                    {
                                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Inventory);
                                        CsItem csItem = CsGameData.Instance.GetItem(UseBountyHunterItemCheck());
                                        StartCoroutine(LoadInventorySubMenu(csItem));
                                    }
                                    else
                                    {
                                        PopupClose();
                                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02003"));
                                    }
                                }
                                else
                                {
                                    PopupClose();
                                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02002"));
                                }
                            }
                            else
                            {
                                PopupClose();
                                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Hunter);
                            }
                        }
                        break;

                    case EnOpen7DayMissionType.SoulStoneLevel:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SubGearSoulstone))
                        {
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearSoulstone);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.BattlePower:
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.ProofOfValor:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ProofOfValor.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                            PopupClose();
                        }
                        else
                        {
                            if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                            {
                                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                                StartCoroutine(LoadDungeonSubMenu((EnOpen7DayMissionType)csOpen7DayEventMission.Type));
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                                PopupClose();
                            }
                        }
                        break;

                    case EnOpen7DayMissionType.GuildMission:
                        if (CsGuildManager.Instance.Guild == null)
                        {
                            // 길드 가입 신청
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.MainGearEnchantLevel:
                        if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearEnchant))
                        {
                            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.MainGear, EnSubMenu.MainGearEnchant);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.Fishing:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FishingQuest.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            if (CsFishingQuestManager.Instance.BaitItemId != 0 || UseBaitItemCheck() != 0)
                            {
                                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Fishing);
                            }
                            else
                            {
                                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A46_TXT_02003"));
                            }
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.Rank:
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Class, EnSubMenu.Class);
                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.SecretLetter:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SecretLetterQuest.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SecretLetter);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.MysteryBox:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.MysteryBoxQuest.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MysteryBox);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.DimensionRaid:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.DimensionRaidQuest.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.DimensionRaid);
                        }

                        PopupClose();
                        break;

                    case EnOpen7DayMissionType.HolyWar:
                        if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.HolyWarQuest.RequiredHeroLevel)
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.HolyWar);
                        }

                        PopupClose();
                        break;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpen7DayProductBuy(CsOpen7DayEventProduct csOpen7DayEventProduct)
    {
        m_csOpen7DayEventProduct = csOpen7DayEventProduct;

        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupBuytItem(csOpen7DayEventProduct));
        }
        else
        {
            OpenPopupBuyItem(csOpen7DayEventProduct);
        }

        /*
        if (CsGameData.Instance.MyHeroInfo.UnOwnDia < csOpen7DayEventProduct.RequiredDia)
        {
            
        }
        else
        {
            CsCommandEventManager.Instance.SendOpen7DayEventProductBuy(csOpen7DayEventProduct.ProductId);
        }
        */
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeReward()
    {
        Debug.Log("#@#@ OnClickCostumeReward");
        if (CsGameData.Instance.MyHeroInfo.Open7DayEventRewarded)
        {
            // 이미 선물을 받음
            Debug.Log("#@#@ OnClickCostumeReward 1");
        }
        else
        {
            if (CsGameConfig.Instance.Open7DayEventCostumeRewardRequiredItemCount <= CsGameData.Instance.MyHeroInfo.GetItemCount((int)CsGameConfig.Instance.Open7DayEventCostumeRewardRequiredItemId))
            {
                // 아이템 개수
                Debug.Log("#@#@ OnClickCostumeReward 2");
            }
            else
            {
                CsItemReward csItemReward = CsGameData.Instance.GetItemReward(CsGameConfig.Instance.Open7DayEventCostumeItemRewardId);
                List<CsItemReward> listCsItemReward = new List<CsItemReward>();

                if (csItemReward == null)
                {
                    Debug.Log("#@#@ OnClickCostumeReward 3");
                    return;
                }
                else
                {
                    listCsItemReward.Add(csItemReward);

                    if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(listCsItemReward))
                    {
                        // 받을 수 있음
                        CsCommandEventManager.Instance.SendOpen7DayEventRewardReceive();
                    }
                    else
                    {
                        // 인벤토리 꽉참
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_NOINVEN"));
                    }

                    listCsItemReward.Clear();
                    listCsItemReward = null;
                }
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = m_trCanvas2.Find("PopupList");

        Transform trImageBackground = transform.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A97_NAME_00001");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPopupClose);

        m_trTopFrame = trImageBackground.Find("TopFrame");

        Text textDescription = m_trTopFrame.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString("A97_TXT_00001");

        m_buttonReceive = m_trTopFrame.Find("ButtonReceive").GetComponent<Button>();
        m_buttonReceive.onClick.RemoveAllListeners();
        m_buttonReceive.onClick.AddListener(OnClickCostumeReward);
        m_buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceive = m_buttonReceive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceive);
        textButtonReceive.text = CsConfiguration.Instance.GetString("A97_BTN_00001");

        UpdateTopFrame();

        m_trToggleList = trImageBackground.Find("ToggleList");
        UpdateToggleList(true);

        m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");
        LoadOpen7DayMissions(m_nSelectDay);

        m_trOpen7DayProduct = trImageBackground.Find("Open7DayProduct");
        UpdateOpen7DayProduct(m_nSelectDay);

        CheckOpen7Day();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTopFrame()
    {
        int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount((int)CsGameConfig.Instance.Open7DayEventCostumeRewardRequiredItemId);

        Slider sliderItemCount = m_trTopFrame.Find("Slider").GetComponent<Slider>();
        sliderItemCount.maxValue = CsGameConfig.Instance.Open7DayEventCostumeRewardRequiredItemCount;
        sliderItemCount.value = nItemCount;

        if (sliderItemCount.maxValue == sliderItemCount.value && !CsGameData.Instance.MyHeroInfo.Open7DayEventRewarded)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonReceive, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonReceive, false);
        }

        Text textItemCount = m_trTopFrame.Find("ImageItemIcon/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemCount);
        textItemCount.text = string.Format(CsConfiguration.Instance.GetString("A97_TXT_01001"), nItemCount, CsGameConfig.Instance.Open7DayEventCostumeRewardRequiredItemCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleList(bool bInitialize)
    {
        // 현재 날짜 - 영웅 생성 날짜
        System.TimeSpan tsRegTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date - CsGameData.Instance.MyHeroInfo.RegDate.Date;

        for (int i = 0; i < CsGameData.Instance.Open7DayEventDayList.Count; i++)
        {
            Toggle toggleDay = m_trToggleList.Find("Toggle" + CsGameData.Instance.Open7DayEventDayList[i].Day).GetComponent<Toggle>();

            if (toggleDay == null)
            {
                continue;
            }
            else
            {
                Text textDay = toggleDay.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDay);
                textDay.text = string.Format(CsConfiguration.Instance.GetString("A97_BTN_01001"), CsGameData.Instance.Open7DayEventDayList[i].Day);

                Transform trImageDim = toggleDay.transform.Find("ImageDim");

                if (CsGameData.Instance.Open7DayEventDayList[i].Day <= (tsRegTime.TotalDays + 1))
                {
                    toggleDay.interactable = true;
                    trImageDim.gameObject.SetActive(false);
                }
                else
                {
                    toggleDay.interactable = false;
                    trImageDim.gameObject.SetActive(true);
                }

                if (bInitialize)
                {
                    if (i == 0)
                    {
                        toggleDay.isOn = true;
                        m_nSelectDay = CsGameData.Instance.Open7DayEventDayList[i].Day;

                        textDay.color = CsUIData.Instance.ColorWhite;
                    }
                    else
                    {
                        toggleDay.isOn = false;

                        textDay.color = CsUIData.Instance.ColorGray;
                    }

                    int nDay = CsGameData.Instance.Open7DayEventDayList[i].Day;

                    toggleDay.onValueChanged.RemoveAllListeners();
                    toggleDay.onValueChanged.AddListener((ison) => OnValueChangedDay(toggleDay, nDay));
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void LoadOpen7DayMissions(int nDay)
    {
        if (m_goOpen7DayMissionItem == null)
        {
            StartCoroutine(LoadOpen7DayMission(nDay));
        }
        else
        {
            UpdateOpen7DayMissions(nDay);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadOpen7DayMission(int nDay)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupOpen7Day/Open7DayMissionItem");
        yield return resourceRequest;

        m_goOpen7DayMissionItem = (GameObject)resourceRequest.asset;
        UpdateOpen7DayMissions(nDay);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOpen7DayMissions(int nDay)
    {
        CsOpen7DayEventDay csOpen7DayEventDay = CsGameData.Instance.Open7DayEventDayList.Find(a => a.Day == nDay);

        if (csOpen7DayEventDay == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < m_trContent.childCount; i++)
            {
                m_trContent.GetChild(i).gameObject.SetActive(false);
            }

            Transform trOpen7DayMissionItem = null;

            Dictionary<int, CsOpen7DayEventMission> dicComplete = new Dictionary<int, CsOpen7DayEventMission>();
            Dictionary<int, CsOpen7DayEventMission> dicMissions = new Dictionary<int, CsOpen7DayEventMission>();
            Dictionary<int, CsOpen7DayEventMission> dicRewarded = new Dictionary<int, CsOpen7DayEventMission>();
            
            for (int i = 0; i < csOpen7DayEventDay.Open7DayEventMissionList.Count; i++)
            {
                CsOpen7DayEventMission csOpen7DayEventMission = csOpen7DayEventDay.Open7DayEventMissionList[i];

                trOpen7DayMissionItem = m_trContent.Find("Open7DayMissionItem" + i);

                if (trOpen7DayMissionItem == null)
                {
                    trOpen7DayMissionItem = Instantiate(m_goOpen7DayMissionItem, m_trContent).transform;
                    trOpen7DayMissionItem.name = "Open7DayMissionItem" + i;
                }

                Text textMissionName = trOpen7DayMissionItem.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textMissionName);
                if (10 <= csOpen7DayEventMission.Type)
                {
                    textMissionName.text = string.Format(CsConfiguration.Instance.GetString("A106_TXT_010" + csOpen7DayEventMission.Type), csOpen7DayEventMission.TargetValue);
                }
                else
                {
                    textMissionName.text = string.Format(CsConfiguration.Instance.GetString("A106_TXT_0100" + csOpen7DayEventMission.Type), csOpen7DayEventMission.TargetValue);
                }

                Slider sliderProgress = trOpen7DayMissionItem.Find("Slider").GetComponent<Slider>();
                sliderProgress.maxValue = csOpen7DayEventMission.TargetValue;

                Transform trItemSlot = null;

                for (int j = 0; j < csOpen7DayEventMission.Open7DayEventMissionRewardList.Count; j++)
                {
                    trItemSlot = trOpen7DayMissionItem.Find("RewardItemSlot" + j);

                    if (trItemSlot == null)
                    {
                        continue;
                    }
                    else
                    {
                        CsItemReward csItemReward = csOpen7DayEventMission.Open7DayEventMissionRewardList[j].ItemReward;
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Small, false);
                    }
                }

                Text textComplete = trOpen7DayMissionItem.Find("TextComplete").GetComponent<Text>();
                CsUIData.Instance.SetFont(textComplete);
                textComplete.text = CsConfiguration.Instance.GetString("A97_TXT_00002");

                Button buttonReceive = trOpen7DayMissionItem.Find("ButtonReceive").GetComponent<Button>();

                // 아직 수령 받기 전
                if (CsGameData.Instance.MyHeroInfo.RewardedOpen7DayEventMissionList.FindIndex(a => a == csOpen7DayEventMission.MissionId) == -1)
                {
                    sliderProgress.value = GetOpen7DayProgressCount(csOpen7DayEventMission);

                    bool bReceive = sliderProgress.maxValue == sliderProgress.value;
                    int nMissionId = csOpen7DayEventMission.MissionId;

                    buttonReceive.onClick.RemoveAllListeners();
                    buttonReceive.onClick.AddListener(() => OnClickMissionRewardReceive(nMissionId, bReceive));

                    Text textButtonReceive = buttonReceive.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textButtonReceive);

                    Image buttonImage = buttonReceive.GetComponent<Image>();

                    if (bReceive)
                    {
                        textButtonReceive.text = CsConfiguration.Instance.GetString("A97_BTN_00002");
                        buttonImage.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_01");

                        dicComplete.Add(i, csOpen7DayEventMission);
                    }
                    else
                    {
                        textButtonReceive.text = CsConfiguration.Instance.GetString("A97_BTN_00003");
                        buttonImage.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_03");

                        dicMissions.Add(i, csOpen7DayEventMission);
                    }

                    buttonReceive.gameObject.SetActive(true);
					textComplete.gameObject.SetActive(false);

					buttonReceive.interactable = !bReceive || CsMainQuestManager.Instance.MainQuest != null && CsGameConfig.Instance.Open7DayEventRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo;
                }
                // 수령 받은 후
                else
                {
                    sliderProgress.value = sliderProgress.maxValue;
                    buttonReceive.gameObject.SetActive(false);
					textComplete.gameObject.SetActive(true);

                    dicRewarded.Add(i, csOpen7DayEventMission);
                }

                Text textProgressCount = sliderProgress.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textProgressCount);
                textProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A97_TXT_01003"), sliderProgress.value, sliderProgress.maxValue);

                trOpen7DayMissionItem.gameObject.SetActive(true);
            }

            int nIndex = 0;
            trOpen7DayMissionItem = null;

            foreach (var item in dicComplete.OrderBy(a => a.Value.MissionId))
            {
                trOpen7DayMissionItem = m_trContent.Find("Open7DayMissionItem" + item.Key);

                if (trOpen7DayMissionItem == null)
                {
                    continue;
                }
                else
                {
                    trOpen7DayMissionItem.SetSiblingIndex(nIndex);
                    nIndex++;
                }
            }

            foreach (var item in dicMissions.OrderBy(a => a.Value.MissionId))
            {
                trOpen7DayMissionItem = m_trContent.Find("Open7DayMissionItem" + item.Key);

                if (trOpen7DayMissionItem == null)
                {
                    continue;
                }
                else
                {
                    trOpen7DayMissionItem.SetSiblingIndex(nIndex);
                    nIndex++;
                }
            }

            foreach (var item in dicRewarded.OrderBy(a => a.Value.MissionId))
            {
                trOpen7DayMissionItem = m_trContent.Find("Open7DayMissionItem" + item.Key);

                if (trOpen7DayMissionItem == null)
                {
                    continue;
                }
                else
                {
                    trOpen7DayMissionItem.SetSiblingIndex(nIndex);
                    nIndex++;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckOpen7Day()
    {
        Transform trImageNew = null;

        // 현재 날짜 - 영웅 생성 날짜
        System.TimeSpan tsRegTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date - CsGameData.Instance.MyHeroInfo.RegDate.Date;

        for (int i = 0; i < CsGameData.Instance.Open7DayEventDayList.Count; i++)
        {
            if (CsGameData.Instance.Open7DayEventDayList[i].Day <= (tsRegTime.TotalDays + 1))
            {
                CsOpen7DayEventDay csOpen7DayEventDay = CsGameData.Instance.Open7DayEventDayList[i];
                trImageNew = m_trToggleList.Find("Toggle" + csOpen7DayEventDay.Day + "/ImageNew");

                if (csOpen7DayEventDay == null)
                {
                    continue;
                }
                else
                {
                    bool bReceive = false;

					if (CsGameConfig.Instance.Open7DayEventRequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
					{
						for (int j = 0; j < csOpen7DayEventDay.Open7DayEventMissionList.Count; j++)
						{
							CsOpen7DayEventMission csOpen7DayEventMission = csOpen7DayEventDay.Open7DayEventMissionList[j];

							if (CsGameData.Instance.MyHeroInfo.RewardedOpen7DayEventMissionList.FindIndex(a => a == csOpen7DayEventMission.MissionId) == -1 &&
								csOpen7DayEventMission.TargetValue <= GetOpen7DayProgressCount(csOpen7DayEventMission))
							{
								bReceive = true;
								break;
							}
							else
							{
								continue;
							}
						}
					}

                    trImageNew.gameObject.SetActive(bReceive);
                }
            }
            else
            {
                continue;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateOpen7DayProduct(int nDay)
    {
        CsOpen7DayEventDay csOpen7DayEventDay = CsGameData.Instance.Open7DayEventDayList.Find(a => a.Day == nDay);

        if (csOpen7DayEventDay == null)
        {
            return;
        }
        else
        {
            Transform trProduct = null;

            for (int i = 0; i < csOpen7DayEventDay.Open7DayEventProductList.Count; i++)
            {
                trProduct = m_trOpen7DayProduct.Find("ImageProduct" + i);

                if (trProduct == null)
                {
                    continue;
                }
                else
                {
                    CsOpen7DayEventProduct csOpen7DayEventProduct = csOpen7DayEventDay.Open7DayEventProductList[i];

                    Image imageItemIcon = trProduct.Find("ImageItemIcon").GetComponent<Image>();
                    imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csOpen7DayEventDay.Open7DayEventProductList[i].Item.Image);

                    Text textProductName = trProduct.Find("TextProductName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textProductName);
                    textProductName.text = csOpen7DayEventDay.Open7DayEventProductList[i].Item.Name;

                    Button buttonBuyProduct = trProduct.Find("ButtonBuyProduct").GetComponent<Button>();
                    buttonBuyProduct.onClick.RemoveAllListeners();
                    buttonBuyProduct.onClick.AddListener(() => OnClickOpen7DayProductBuy(csOpen7DayEventProduct));

                    Text textProductValue = buttonBuyProduct.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textProductValue);
                    textProductValue.text = csOpen7DayEventDay.Open7DayEventProductList[i].RequiredDia.ToString("#,##0");

                    Text textBuyCount = trProduct.Find("TextBuyCount").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textBuyCount);

                    Transform trImageLock = trProduct.Find("ImageLock");

                    if (CsGameData.Instance.MyHeroInfo.PurchasedOpen7DayEventProductList.FindIndex(a => a == csOpen7DayEventDay.Open7DayEventProductList[i].ProductId) < 0)
                    {
                        textBuyCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 1, 1);
                        trImageLock.gameObject.SetActive(false);
                    }
                    else
                    {
                        textBuyCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 0, 1);
                        trImageLock.gameObject.SetActive(true);

                        Text textLock = trImageLock.Find("Text").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textLock);
                        textLock.text = CsConfiguration.Instance.GetString("A106_TXT_00001");
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    long GetOpen7DayProgressCount(CsOpen7DayEventMission csOpen7DayEventMission)
    {
        long lProgressCount = 0;
        EnOpen7DayMissionType enOpen7DayMissionType = (EnOpen7DayMissionType)csOpen7DayEventMission.Type;
        CsHeroOpen7DayEventProgressCount csHeroOpen7DayEventProgressCount = CsGameData.Instance.MyHeroInfo.GetHeroOpen7DayEventProgressCount(csOpen7DayEventMission.Type);

        if (enOpen7DayMissionType == EnOpen7DayMissionType.Level)
        {
            lProgressCount = CsGameData.Instance.MyHeroInfo.Level;
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.SubGearLevel)
        {
            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSubGearList.Count; i++)
            {
                if (CsGameData.Instance.MyHeroInfo.HeroSubGearList[i] == null)
                {
                    continue;
                }
                else
                {
                    if (lProgressCount < CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].Level)
                    {
                        lProgressCount = CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].Level;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.SoulStoneLevel)
        {
            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSubGearList.Count; i++)
            {
                if (CsGameData.Instance.MyHeroInfo.HeroSubGearList[i] == null)
                {
                    continue;
                }
                else
                {
                    for (int j = 0; j < CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].SoulstoneSocketList.Count; j++)
                    {
                        if (CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].SoulstoneSocketList[j].Item == null && CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].Equipped)
                        {
                            continue;
                        }
                        else
                        {
                            lProgressCount += CsGameData.Instance.MyHeroInfo.HeroSubGearList[i].SoulstoneSocketList[j].Item.Level;
                        }
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.BattlePower)
        {
            lProgressCount = CsGameData.Instance.MyHeroInfo.BattlePower;
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.MainGearEnchantLevel)
        {
            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
            {
                if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i] == null)
                {
                    continue;
                }
                else
                {
                    if (lProgressCount < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].EnchantLevel)
                    {
                        lProgressCount = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].EnchantLevel;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
        else if (enOpen7DayMissionType == EnOpen7DayMissionType.Rank)
        {
            lProgressCount = CsGameData.Instance.MyHeroInfo.RankNo;
        }
        else
        {
            if (csHeroOpen7DayEventProgressCount == null)
            {
                lProgressCount = 0;
            }
            else
            {
                lProgressCount = csHeroOpen7DayEventProgressCount.AccProgressCount;
            }
        }

        return lProgressCount;
    }

    const int m_nBaitItemIdDefault = 1401;
    //---------------------------------------------------------------------------------------------------
    //사용가능한 아이템있는지 체크 
    int UseBaitItemCheck()
    {
        for (int i = 4; i >= 0; --i)
        {
            if (CsGameData.Instance.MyHeroInfo.GetItemCount((m_nBaitItemIdDefault + i)) != 0)
            {
                return (m_nBaitItemIdDefault + i);
            }
        }

        return 0;
    }

    const int m_nBountyHunterDefault = 1345;
    //---------------------------------------------------------------------------------------------------
    int UseBountyHunterItemCheck()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                CsItem csItem = CsGameData.Instance.GetItem(m_nBountyHunterDefault - i * 10 - j);

                if (CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId) != 0)
                {
                    if (CsGameData.Instance.MyHeroInfo.Level <= csItem.RequiredMaxHeroLevel &&
                        csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        return (m_nBountyHunterDefault - i * 10 - j);
                    }
                }
            }
        }

        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadInventorySubMenu(CsItem csItem)
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory") != null);
        Transform trInventorySubMenu = m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory");

        CsInventory csInventory = trInventorySubMenu.GetComponent<CsInventory>();

        if (csInventory != null)
        {
            int nIndex = -1;

            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
            {
                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.Item &&
                    CsGameData.Instance.MyHeroInfo.InventorySlotList[i].InventoryObjectItem.Item.ItemId == csItem.ItemId)
                {
                    nIndex = i;
                    break;
                }
            }

            if (nIndex != -1)
            {
                csInventory.QuickOpenPopupItemInfo(nIndex);
            }
        }

        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDungeonSubMenu(EnOpen7DayMissionType enOpen7DayMissionType)
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory") != null);

        Transform trDungeonSubMenu = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory");
        CsDungeonCartegory csDungeonCartegory = trDungeonSubMenu.GetComponent<CsDungeonCartegory>();

        switch (enOpen7DayMissionType)
        {
            case EnOpen7DayMissionType.StoryDungeon:
                for (int i = 0; i < CsGameData.Instance.StoryDungeonList.Count; i++)
                {
                    if (CsGameData.Instance.StoryDungeonList[i].RequiredHeroMinLevel < CsGameData.Instance.MyHeroInfo.Level &&
                        CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.StoryDungeonList[i].RequiredHeroMaxLevel)
                    {
                        csDungeonCartegory.ShortCutDungeonInfo(i);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                break;

            case EnOpen7DayMissionType.ExpDungeon:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.Exp);
                break;

            case EnOpen7DayMissionType.ProofOfValor:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.ProofOfValor);
                break;
        }

        PopupClose();
    }

    #region PopupCalculator

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupBuytItem(CsOpen7DayEventProduct csOpen7DayEventProduct)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        OpenPopupBuyItem(csOpen7DayEventProduct);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem(CsOpen7DayEventProduct csOpen7DayEventProduct)
    {
        m_trCalculator = m_trPopupList.Find("PopupCalculator");

        if (m_trCalculator == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            m_trCalculator = goPopupBuyCount.transform;
        }
        else
        {
            m_trCalculator.gameObject.SetActive(false);
        }

        m_csPopupCalculator = m_trCalculator.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;
        m_csPopupCalculator.DisplayItem(csOpen7DayEventProduct.Item, csOpen7DayEventProduct.ItemOwned, csOpen7DayEventProduct.RequiredDia, EnResourceType.UnOwnDia);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseCalculator()
    {
        m_csPopupCalculator.EventBuyItem -= OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator -= OnEventCloseCalculator;
        m_csPopupCalculator = null;

        Transform trPopupCalculator = m_trPopupList.Find("PopupCalculator");

        if (trPopupCalculator != null)
        {
            Destroy(trPopupCalculator.gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        if (nCount == 0)
        {
            return;
        }

        if (nCount == 1)
        {
            if (CsGameData.Instance.MyHeroInfo.UnOwnDia < m_csOpen7DayEventProduct.RequiredDia)
            {

            }
            else
            {
                CsCommandEventManager.Instance.SendOpen7DayEventProductBuy(m_csOpen7DayEventProduct.ProductId);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A106_TXT_00002"));
        }
    }

    #endregion PopupCalculator
}