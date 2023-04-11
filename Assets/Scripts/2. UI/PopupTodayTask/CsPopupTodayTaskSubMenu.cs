using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CsPopupTodayTaskSubMenu : CsPopupSub
{
    [SerializeField] GameObject m_goToggleTodayTaskSubMenuItem;
    [SerializeField] GameObject m_goItemSlot;
    GameObject m_goPopupItemInfo;

    Transform m_trTodayTaskSubMenuItemList;
    Transform m_trPopupTodayTask;
    Transform m_trCanvas1;
    Transform m_trCanvas2;
    Transform m_trPopupTodayTaskInfo;
    Transform m_trPopupItemInfo;

    Toggle m_toggleDisplayRecommend;

    List<CsTodayTask> m_listCsTodayTaskOpenContent = new List<CsTodayTask>();
    List<CsTodayTask> m_listCsTodayTaskCloseContent = new List<CsTodayTask>();

    bool m_bFirst = true;
    float m_flTime = 0.0f;

    enum EnTodayTaskCategories
    {
        TodayTaskExp = 1,
        TodayTaskItem = 2,
        TodayTaskLimit = 3,
    }

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        // 레벨업 상황
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        m_trTodayTaskSubMenuItemList = transform.Find("Scroll View/Viewport/Content");
        m_trCanvas1 = GameObject.Find("Canvas1").transform;
        m_trCanvas2 = GameObject.Find("Canvas2").transform;

        m_toggleDisplayRecommend = transform.Find("ToggleDisplayRecommend").GetComponent<Toggle>();
        m_toggleDisplayRecommend.onValueChanged.RemoveAllListeners();
        m_toggleDisplayRecommend.isOn = false;
        m_toggleDisplayRecommend.onValueChanged.AddListener((ison) => OnValueChangedDisplayRecommend(ison));
        m_toggleDisplayRecommend.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        m_trPopupTodayTask = m_trCanvas1.Find("PopupTodayTask");
        m_trPopupTodayTaskInfo = m_trPopupTodayTask.Find("PopupTodayTaskInfo");

        Text textRecommend = m_toggleDisplayRecommend.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRecommend);
        textRecommend.text = CsConfiguration.Instance.GetString("A50_TXT_00003");

        // max 카운트 초기화
        for (int i = 0; i < CsGameData.Instance.TodayTaskList.Count; i++)
        {
            CsGameData.Instance.TodayTaskList[i].Init();
        }

        CreateToggleTodayTaskSubMenuItem();
        UpdateTodayTaskItemDisplay(m_toggleDisplayRecommend.isOn);
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        // 레벨업 상황
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;
        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.TodayTaskLimit)
            {
                CreateToggleTodayTaskSubMenuItem();
                UpdateTodayTaskItemDisplay(m_toggleDisplayRecommend.isOn);


            }
            else
            {
                return;
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        UpdatePopupTodayTaskSubMenu();
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        UpdatePopupTodayTaskSubMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        UpdatePopupTodayTaskSubMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupTodayTaskSubMenu()
    {
        CreateToggleTodayTaskSubMenuItem();
        m_trTodayTaskSubMenuItemList.localPosition = Vector3.zero;
        UpdateTodayTaskItemDisplay(m_toggleDisplayRecommend.isOn);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTodayTaskItem(int nTaskId)
    {
        InitializePopupTodayTaskInfo(CsGameData.Instance.GetTodayTask(nTaskId));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupTodayTaskInfoClose()
    {
        m_trPopupTodayTaskInfo.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAttendButton(int nTaskId)
    {
        switch ((EnTodayTaskType)nTaskId)
        {
            case EnTodayTaskType.ExpDungeon:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

			// 공포의 제단
			case EnTodayTaskType.FearAltar:
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
					StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
				}
				else
				{
					PopupClose();
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
				}
				break;

            //차원의습격퀘스트
            case EnTodayTaskType.DimensionRaidQuest:
                PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.DimensionRaid);
                break;

            //의문의상자퀘스트
            case EnTodayTaskType.MysteryBoxQuest:
                PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MysteryBox);
                break;

            //밀서퀘스트
            case EnTodayTaskType.SecretLetterQuest:
                PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SecretLetter);
                break;

            //낚시퀘스트
            case EnTodayTaskType.FishingQuest:
                PopupClose();

                if (CsFishingQuestManager.Instance.BaitItemId != 0 || UseBaitItemCheck() != 0)
                {
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Fishing);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A46_TXT_02003"));
                }
                break;

            //현상금사냥퀘스트
            case EnTodayTaskType.BountyHunterQuest:
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
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02003"));
                        }
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A45_TXT_02002"));
                    }
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.Hunter);
                }
                break;

            //지하미로던전
            case EnTodayTaskType.UndergroundMaze:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            // 용맹의 증명
            case EnTodayTaskType.ProofOfValor:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            //스토리던전
            case EnTodayTaskType.StoryDungeon:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            case EnTodayTaskType.AncientRelic:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            // 검투 대회
            case EnTodayTaskType.FieldOfHonor:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            //농장의위협퀘스트
            case EnTodayTaskType.ThreatOfFarmQuest:
                PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.ThreatOfFarm);
                break;

            case EnTodayTaskType.ArtifactRoom:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            // 영혼을 탐하는 자
            case EnTodayTaskType.SoulCoveter:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;
            ///

            case EnTodayTaskType.OsirisRoom:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            //위대한성전
            case EnTodayTaskType.HolyWarQuest:
                PopupClose();

                if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.None && !CsHolyWarQuestManager.Instance.CheckAvailability())
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A53_TXT_00001"));
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.HolyWar);
                }
                break;
            //보급지원퀘스트
            case EnTodayTaskType.SupplySupportQuest:
                PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.SupplySupport);
                break;
            //길드 미션
            case EnTodayTaskType.GuildMissionQuest:
                if (CsGuildManager.Instance.Guild == null)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    //길드 미션 자동
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildMission);
                }
                break;

            // 길드 기부
            case EnTodayTaskType.GuildAltar:
                // 길드가 없음
                if (CsGuildManager.Instance.Guild == null)
                {
                    // 길드 가입 신청
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
                    {
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildAlter);
                    }
                    else
                    {
                        // 길드 영지 이동
                        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    }

                    PopupClose();
                }
                break;

            case EnTodayTaskType.GuildSupply:
                if (CsGuildManager.Instance.Guild == null)
                {
                    // 길드 가입 신청
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    if (CsGuildManager.Instance.MyGuildMemberGrade.GuildSupplySupportQuestEnabled)
                    {
                        PopupClose();
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildSupplySupport);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("권한 x"));
                    }
                }

                break;

            // 길드 농장
            case EnTodayTaskType.GuildFarmQuest:
                // 길드가 없음
                if (CsGuildManager.Instance.Guild == null)
                {
                    // 길드 가입 신청
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
                    {
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFarm);
                    }
                    else
                    {
                        // 길드 영지 이동
                        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    }

                    PopupClose();
                }

                break;

            // 길드 군량
            case EnTodayTaskType.GuildFoodWarehouse:
                // 길드가 없음
                if (CsGuildManager.Instance.Guild == null)
                {
                    // 길드 가입 신청
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
                    {
                        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildFoodWareHouse);
                    }
                    else
                    {
                        // 길드 영지 이동
                        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
                    }

                    PopupClose();
                }

                break;

            case EnTodayTaskType.GuildHunting:
                // 길드가 없음
                if (CsGuildManager.Instance.Guild == null)
                {
                    // 길드 가입 신청
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.GuildHunting);
                    PopupClose();
                }

                break;

            case EnTodayTaskType.DailyQuest:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DailyQuest, EnSubMenu.DailyQuest);
                PopupClose();

                break;

            case EnTodayTaskType.WeeklyQuest:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.WeeklyQuest, EnSubMenu.WeeklyQuest);
                PopupClose();

				break;

			case EnTodayTaskType.WisdomTemple:
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
					StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
				}
				else
				{
					PopupClose();
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
				}

                break;

			case EnTodayTaskType.RuinsReclaim:
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
					StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
				}
				else
				{
					PopupClose();
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
				}

				break;

			// 진정한 영웅 퀘스트
            case EnTodayTaskType.TrueHero:
				PopupClose();
                CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.TrueHero);

                break;

			// 필드 보스
			case EnTodayTaskType.FieldBoss:
				PopupClose();
				// 필드 보스 팝업 열기
				CsGameEventUIToUI.Instance.OnEventOpenPopupFieldBoss();
				break;

            case EnTodayTaskType.InfiniteWar:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

			case EnTodayTaskType.CreatureFarm:
				PopupClose();
				CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.CreatureFarm);

				break;

            case EnTodayTaskType.WarMemory:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

            case EnTodayTaskType.DragonNest:
                if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
                    StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
                }
                else
                {
                    PopupClose();
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                }
                break;

			case EnTodayTaskType.TradeShip:
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
					StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
				}
				else
				{
					PopupClose();
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
				}
				break;

			case EnTodayTaskType.AnkouTomb:
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
					StartCoroutine(LoadDungeonSubMenu((EnTodayTaskType)nTaskId));
				}
				else
				{
					PopupClose();
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
				}
				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDungeonSubMenu(EnTodayTaskType enTodayTaskType)
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory") != null);

        Transform trDungeonSubMenu = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupDungeonCategory");
        CsDungeonCartegory csDungeonCartegory = trDungeonSubMenu.GetComponent<CsDungeonCartegory>();

        switch (enTodayTaskType)
        {
            case EnTodayTaskType.ExpDungeon:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.Exp);
                break;

            case EnTodayTaskType.UndergroundMaze:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.UndergroundMaze);
                break;

            case EnTodayTaskType.StoryDungeon:
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
            case EnTodayTaskType.AncientRelic:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.AncientRelic);
                break;

            case EnTodayTaskType.FieldOfHonor:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.FieldOfHonor);
                break;

            case EnTodayTaskType.ProofOfValor:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.ProofOfValor);
                break;

            case EnTodayTaskType.ArtifactRoom:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.ArtifactRoom);
                break;

            case EnTodayTaskType.OsirisRoom:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.OsirisRoom);
                break;

            case EnTodayTaskType.SoulCoveter:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.SoulCoveter);
                break;

			case EnTodayTaskType.WisdomTemple:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnIndividualDungeonType.WisdomTemple);
				break;

			case EnTodayTaskType.RuinsReclaim:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.RuinsReclaim);
				break;

            case EnTodayTaskType.InfiniteWar:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.InfiniteWar);
                break;

			case EnTodayTaskType.FearAltar:
				csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.FearAltar);
				break;

            case EnTodayTaskType.WarMemory:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.WarMemory);
                break;

            case EnTodayTaskType.DragonNest:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnPartyDungeonType.DragonNest);
                break;

            case EnTodayTaskType.TradeShip:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.TradeShip);
                break;

            case EnTodayTaskType.AnkouTomb:
                csDungeonCartegory.ShortCutDungeonInfo((int)EnTimeLimitDungeonType.AnkouTomb);
                break;
        }

        PopupClose();
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
    void OnValueChangedDisplayRecommend(bool bIsOn)
    {
        m_trTodayTaskSubMenuItemList.localPosition = Vector3.zero;
        UpdateTodayTaskItemDisplay(bIsOn);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void CreateToggleTodayTaskSubMenuItem()
    {
        int nCategoryId = 0;

        // 카테고리 설정
        switch (m_iPopupMain.GetCurrentSubMenu().EnSubMenu)
        {
            case EnSubMenu.TodayTaskExp:
                nCategoryId = (int)EnTodayTaskCategories.TodayTaskExp;
                break;
            case EnSubMenu.TodayTaskItem:
                nCategoryId = (int)EnTodayTaskCategories.TodayTaskItem;
                break;
            case EnSubMenu.TodayTaskLimit:
                nCategoryId = (int)EnTodayTaskCategories.TodayTaskLimit;
                break;
        }

        for (int i = 0; i < m_trTodayTaskSubMenuItemList.childCount; i++)
        {
            m_trTodayTaskSubMenuItemList.GetChild(i).gameObject.SetActive(false);
        }

        m_listCsTodayTaskOpenContent.Clear();
        m_listCsTodayTaskCloseContent.Clear();

        for (int i = 0; i < CsGameData.Instance.TodayTaskList.Count; i++)
        {
            // 카테고리가 같은 오늘의 할 일
            if (CsGameData.Instance.TodayTaskList[i].TodayTaskCategory.CategoryId == nCategoryId)
            {
                Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + CsGameData.Instance.TodayTaskList[i].TaskId);

                if (trTodayTaskItem == null)
                {
                    trTodayTaskItem = Instantiate(m_goToggleTodayTaskSubMenuItem, m_trTodayTaskSubMenuItemList).transform;
                    trTodayTaskItem.name = "TodayTaskItem" + CsGameData.Instance.TodayTaskList[i].TaskId;
                }

                UpdateTodayTaskSubMenuItem(trTodayTaskItem, CsGameData.Instance.TodayTaskList[i]);

                int nTaskId = CsGameData.Instance.TodayTaskList[i].TaskId;

                Button buttonTodayTaskItem = trTodayTaskItem.Find("ButtonTodayTask").GetComponent<Button>();
                buttonTodayTaskItem.onClick.RemoveAllListeners();
                buttonTodayTaskItem.onClick.AddListener(() => OnClickTodayTaskItem(nTaskId));
                buttonTodayTaskItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
        }

        // 순서 정렬
        m_listCsTodayTaskOpenContent.Sort(CompareTo);
        m_listCsTodayTaskCloseContent.Sort(CompareTo);

        for (int i = 0; i < m_listCsTodayTaskOpenContent.Count; i++)
        {
            Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskOpenContent[i].TaskId);
            trTodayTaskItem.SetSiblingIndex(i);
        }

        for (int i = m_listCsTodayTaskOpenContent.Count; i < m_listCsTodayTaskCloseContent.Count + m_listCsTodayTaskOpenContent.Count; i++)
        {
            Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskCloseContent[i - m_listCsTodayTaskOpenContent.Count].TaskId);
            trTodayTaskItem.SetSiblingIndex(i);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayTaskSubMenuItem(Transform trTodayTaskSubMenuItem, CsTodayTask csTodayTask)
    {
        Image imageIcon = trTodayTaskSubMenuItem.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + csTodayTask.TaskId);

        Transform trSubMenuItemInfo = trTodayTaskSubMenuItem.Find("SubMenuItemInfo");

        Text textName = trSubMenuItemInfo.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csTodayTask.Name;

        Text textRewardInfo = trSubMenuItemInfo.Find("TextRewardInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRewardInfo);

        Transform trLevelList = trSubMenuItemInfo.Find("LevelList");

        // 진행도
        Transform trProgressInfo = trSubMenuItemInfo.Find("ProgressInfo");

        Text textDailyProgressCount = trProgressInfo.Find("TextDailyProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDailyProgressCount);

        Text textAchievementPoint = trProgressInfo.Find("TextAchievementPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAchievementPoint);
        textAchievementPoint.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01002"), csTodayTask.AchievementPoint);

        Text textDescriptionInfo = trSubMenuItemInfo.Find("TextDescriptionInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescriptionInfo);
        textDescriptionInfo.text = CsConfiguration.Instance.GetString(csTodayTask.LockTextKey);

        Text textComplete = trTodayTaskSubMenuItem.Find("TextComplete").GetComponent<Text>();
        CsUIData.Instance.SetFont(textComplete);
        textComplete.text = CsConfiguration.Instance.GetString("A50_TXT_00001");

        Button buttonAttend = trTodayTaskSubMenuItem.Find("ButtonAttend").GetComponent<Button>();
        buttonAttend.onClick.RemoveAllListeners();
        buttonAttend.onClick.AddListener(() => OnClickAttendButton(csTodayTask.TaskId));
        buttonAttend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonAttend = buttonAttend.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAttend);
      

        if (csTodayTask.TodayTaskType == EnTodayTaskType.GuildAltar ||
            csTodayTask.TodayTaskType == EnTodayTaskType.GuildFarmQuest ||
            csTodayTask.TodayTaskType == EnTodayTaskType.GuildFoodWarehouse ||
            csTodayTask.TodayTaskType == EnTodayTaskType.GuildMissionQuest ||
            csTodayTask.TodayTaskType == EnTodayTaskType.GuildHunting)
        {
            if (CsGuildManager.Instance.Guild == null)
            {
                // 길드 가입
                textButtonAttend.text = CsConfiguration.Instance.GetString("A50_BTN_00002");
            }
            else
            {
                // 참여
                textButtonAttend.text = CsConfiguration.Instance.GetString("A50_BTN_00001");
            }
        }
        else if (csTodayTask.TodayTaskType == EnTodayTaskType.GuildSupply)
        {
            if (CsGuildManager.Instance.Guild == null)
            {
                // 길드 가입
                textButtonAttend.text = CsConfiguration.Instance.GetString("A50_BTN_00002");
                CsUIData.Instance.DisplayButtonInteractable(buttonAttend, true);
            }
            else
            {
                textButtonAttend.text = CsConfiguration.Instance.GetString("A50_BTN_00001");
                CsUIData.Instance.DisplayButtonInteractable(buttonAttend, CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).GuildSupplySupportQuestEnabled);
            }
        }
        else
        {
            textButtonAttend.text = CsConfiguration.Instance.GetString("A50_BTN_00001");
        }

        Transform trImageLock = trTodayTaskSubMenuItem.Find("ImageLock");

        Text textLock = trImageLock.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLock);

        Transform trAttendTimeInfo = trTodayTaskSubMenuItem.Find("AttendTimeInfo");

        Text textAttendTime = trAttendTimeInfo.Find("TextAttendTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttendTime);

        Text textAttendDay = trAttendTimeInfo.Find("TextAttendDay").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttendDay);

        Text textAttendPlace = trAttendTimeInfo.Find("TextAttendPlace").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttendPlace);

        switch (m_iPopupMain.GetCurrentSubMenu().EnSubMenu)
        {
            case EnSubMenu.TodayTaskExp:
                textRewardInfo.gameObject.SetActive(false);

                // 랭크 표시
                for (int i = 0; i < trLevelList.childCount; i++)
                {
                    Transform trImageStar = trLevelList.GetChild(i);

                    if (i < csTodayTask.Rank)
                    {
                        trImageStar.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageStar.gameObject.SetActive(false);
                    }
                }

                trLevelList.gameObject.SetActive(true);
                break;

            case EnSubMenu.TodayTaskItem:
                textRewardInfo.text = csTodayTask.RewardText;
                textRewardInfo.gameObject.SetActive(true);
                trLevelList.gameObject.SetActive(false);
                break;

            case EnSubMenu.TodayTaskLimit:
                textRewardInfo.text = csTodayTask.RewardText;
                textRewardInfo.gameObject.SetActive(true);
                trLevelList.gameObject.SetActive(false);
                break;
        }

        int nRequiredLevel = 0;

        int nCurrentSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;
        System.TimeSpan tsStartTime;
        System.TimeSpan tsEndTime;

        bool bAttend;
        bool bOpenDungeon = false;

        switch ((EnTodayTaskType)csTodayTask.TaskId)
        {
            case EnTodayTaskType.ExpDungeon:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                // 한계
                break;

            case EnTodayTaskType.FearAltar:
				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FearAltar.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.FearAltar.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
				}
                break;

            case EnTodayTaskType.DimensionRaidQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.DimensionRaidQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.DimensionRaidQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.MysteryBoxQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.MysteryBoxQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.MysteryBoxQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.SecretLetterQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SecretLetterQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.SecretLetterQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.FishingQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FishingQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), nRequiredLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.BountyHunterQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.BountyHunterQuestRequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), nRequiredLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.UndergroundMaze:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.UndergroundMaze.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.UndergroundMaze.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    System.TimeSpan tsRemainingTime;

                    if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.UndergroundMaze) == null)
                    {
                        tsRemainingTime = System.TimeSpan.FromSeconds(CsGameData.Instance.UndergroundMaze.LimitTime - CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime);
                    }
                    else
                    {
                        tsRemainingTime = System.TimeSpan.FromSeconds(0.0f);
                    }

                    if (tsRemainingTime.TotalSeconds > 0.0f)
                    {
                        textComplete.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        m_listCsTodayTaskOpenContent.Add(csTodayTask);
                    }
                    else
                    {
                        tsRemainingTime = System.TimeSpan.FromSeconds(0.0f);

                        buttonAttend.gameObject.SetActive(false);
                        textComplete.gameObject.SetActive(true);

                        m_listCsTodayTaskCloseContent.Add(csTodayTask);
                    }

                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01008"), tsRemainingTime.Hours.ToString("0#"), tsRemainingTime.Minutes.ToString("0#"), tsRemainingTime.Seconds.ToString("0#"));

					UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.ProofOfValor:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ProofOfValor.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.ProofOfValor.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.StoryDungeon:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.StoryDungeonList[0].RequiredHeroMinLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.StoryDungeonList[0].RequiredHeroMinLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.AncientRelic:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.AncientRelic.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.AncientRelic.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.FieldOfHonor:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FieldOfHonor.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.FieldOfHonor.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }
                // 제한없음
                break;

            case EnTodayTaskType.ThreatOfFarmQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.ArtifactRoom:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ArtifactRoom.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.ArtifactRoom.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }
                break;

            case EnTodayTaskType.SoulCoveter:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SoulCoveter.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.SoulCoveter.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }

                break;

            case EnTodayTaskType.OsirisRoom:
                if (CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList.Count > 0)
                {
                    if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList[0].RequiredHeroLevel)
                    {
                        UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                        textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList[0].RequiredHeroLevel);
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);
                    }
                    else
                    {
                        UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                    }
                }
                break;

            case EnTodayTaskType.DimensionInfiltrationEvent:
                textComplete.gameObject.SetActive(false);
                buttonAttend.gameObject.SetActive(false);

                textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");

                tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.DimensionInfiltrationEvent.StartTime);
                tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.DimensionInfiltrationEvent.EndTime);

                textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));
                trAttendTimeInfo.gameObject.SetActive(true);

                if (tsStartTime.TotalSeconds <= nCurrentSecond &&
                    nCurrentSecond < tsEndTime.TotalSeconds)
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                }
                else
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                }

                if (tsEndTime.TotalSeconds < nCurrentSecond)
                {
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }

                break;

            case EnTodayTaskType.HolyWarQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.HolyWarQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);

                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.HolyWarQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    bAttend = false;

                    for (int i = 0; i < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count; i++)
                    {
                        // 참가가 가능한지 여부
                        if (CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].StartTime <= nCurrentSecond &&
                            nCurrentSecond < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].EndTime)
                        {
                            bAttend = true;
                            break;
                        }
                    }

                    if (bAttend)
                    {
                        trAttendTimeInfo.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                    }
                    else
                    {
                        CsHolyWarQuestSchedule csHolyWarQuestSchedule = null;

                        for (int i = 0; i < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count; i++)
                        {
                            if (nCurrentSecond < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].EndTime)
                            {
                                csHolyWarQuestSchedule = CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i];
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (csHolyWarQuestSchedule == null)
                        {
                            csHolyWarQuestSchedule = CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0];
                        }

                        tsStartTime = System.TimeSpan.FromSeconds(csHolyWarQuestSchedule.StartTime);
                        tsEndTime = System.TimeSpan.FromSeconds(csHolyWarQuestSchedule.EndTime);

                        textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                        buttonAttend.gameObject.SetActive(false);
                        trAttendTimeInfo.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                    }

                    if (CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count - 1].EndTime < nCurrentSecond)
                    {
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);
                    }
                    else
                    {
                        m_listCsTodayTaskOpenContent.Add(csTodayTask);
                    }

                    textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");
                }
                break;

            case EnTodayTaskType.SupplySupportQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SupplySupportQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.GuildMissionQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameConfig.Instance.GuildRequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }

                break;

            // 길드 기부
            case EnTodayTaskType.GuildAltar:
                UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                break;

            case EnTodayTaskType.GuildSupply:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameConfig.Instance.GuildRequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            // 길드 농장
            case EnTodayTaskType.GuildFarmQuest:
                if (CsGameData.Instance.GuildFarmQuest.StartTime <= nCurrentSecond &&
                    nCurrentSecond <= CsGameData.Instance.GuildFarmQuest.EndTime)
                {
                    bAttend = true;
                }
                else
                {
                    bAttend = false;
                }

                if (bAttend)
                {
                    trAttendTimeInfo.gameObject.SetActive(false);
                    buttonAttend.gameObject.SetActive(true);

                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                }
                else
                {
                    textAttendTime.text = csTodayTask.EventTimeText;

                    buttonAttend.gameObject.SetActive(false);
                    trAttendTimeInfo.gameObject.SetActive(true);

                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                }

                if (CsGameData.Instance.GuildFarmQuest.EndTime < nCurrentSecond)
                {
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }

                CsHeroTodayTask csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(csTodayTask.TaskId);

                if (csHeroTodayTask == null)
                {
                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), 0, CsGameData.Instance.GuildFarmQuest.LimitCount);
                }
                else
                {
                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), csHeroTodayTask.ProgressCount, CsGameData.Instance.GuildFarmQuest.LimitCount);
                }

                break;

            // 길드 군량
            case EnTodayTaskType.GuildFoodWarehouse:
                UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                break;

            case EnTodayTaskType.GuildHunting:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.GuildRequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameConfig.Instance.GuildRequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.DailyQuest:
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.DailyQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.DailyQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }
                break;

            case EnTodayTaskType.WeeklyQuest:

                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.WeeklyQuest.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.WeeklyQuest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
                }

                break;

			case EnTodayTaskType.WisdomTemple:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.WisdomTemple.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.WisdomTemple.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
				}

				break;

			case EnTodayTaskType.RuinsReclaim:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.RuinsReclaim.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.RuinsReclaim.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					bAttend = false;

					TimeSpan timeSpanCurrent = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date);
					int nMaxScheduleId = CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList.Max(schedule => schedule.ScheduleId);
					CsRuinsReclaimOpenSchedule csRuinsReclaimOpenSchedule = null;

					foreach (var schedule in CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList)
					{
						if (timeSpanCurrent.TotalSeconds < schedule.StartTime)
						{
							csRuinsReclaimOpenSchedule = schedule;
							break;
						}
						else if (schedule.StartTime <= timeSpanCurrent.TotalSeconds && timeSpanCurrent.TotalSeconds < schedule.EndTime)
						{
							bAttend = true;
							break;
						}
						else
						{
							if (schedule.ScheduleId >= nMaxScheduleId)
							{
								csRuinsReclaimOpenSchedule = CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList[0];
							}
						}
					}

					if (bAttend)
					{
						m_listCsTodayTaskOpenContent.Add(csTodayTask);

						trAttendTimeInfo.gameObject.SetActive(false);
						buttonAttend.gameObject.SetActive(true);

						UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
					}
					else
					{
						m_listCsTodayTaskCloseContent.Add(csTodayTask);

						tsStartTime = System.TimeSpan.FromSeconds(csRuinsReclaimOpenSchedule.StartTime);
						tsEndTime = System.TimeSpan.FromSeconds(csRuinsReclaimOpenSchedule.EndTime);

						textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

						trAttendTimeInfo.gameObject.SetActive(true);
						buttonAttend.gameObject.SetActive(false);

						UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
					}

					textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), CsDungeonManager.Instance.RuinsReclaim.FreePlayCount, 1);
				}

				break;

			case EnTodayTaskType.TrueHero:
				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.TrueHeroQuest.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.TrueHeroQuest.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
				}

				break;

			case EnTodayTaskType.FieldBoss:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FieldBossEvent.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.FieldBossEvent.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					textComplete.gameObject.SetActive(true);
					buttonAttend.gameObject.SetActive(true);

					textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");

					trAttendTimeInfo.gameObject.SetActive(false);

					UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
					m_listCsTodayTaskOpenContent.Add(csTodayTask);
				}

				break;

            case EnTodayTaskType.InfiniteWar:

                if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.InfiniteWar.RequiredHeroLevel)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.InfiniteWar.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    bAttend = false;

                    for (int i = 0; i < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                    {
                        if (CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].StartTime <= nCurrentSecond && nCurrentSecond < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                        {
                            bAttend = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (bAttend)
                    {
                        trAttendTimeInfo.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                    }
                    else
                    {
                        CsInfiniteWarOpenSchedule csInfiniteWarOpenSchedule = null;

                        for (int i = 0; i < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                        {
                            if (nCurrentSecond < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                            {
                                csInfiniteWarOpenSchedule = CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i];
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (csInfiniteWarOpenSchedule == null)
                        {
                            csInfiniteWarOpenSchedule = CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[0];
                        }

                        tsStartTime = System.TimeSpan.FromSeconds(csInfiniteWarOpenSchedule.StartTime);
                        tsEndTime = System.TimeSpan.FromSeconds(csInfiniteWarOpenSchedule.EndTime);

                        textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                        buttonAttend.gameObject.SetActive(false);
                        trAttendTimeInfo.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                    }

                    if (CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count - 1].EndTime < nCurrentSecond)
                    {
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);
                    }
                    else
                    {
                        m_listCsTodayTaskOpenContent.Add(csTodayTask);
                    }

                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), CsDungeonManager.Instance.InfiniteWar.DailyPlayCount, CsGameData.Instance.InfiniteWar.EnterCount);
                }

                break;

            case EnTodayTaskType.BattleFieldSupportEvent:

                textComplete.gameObject.SetActive(false);
                buttonAttend.gameObject.SetActive(false);

                textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");

                tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.BattlefieldSupportEvent.StartTime);
                tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.BattlefieldSupportEvent.EndTime);

                textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));
                trAttendTimeInfo.gameObject.SetActive(true);

                if (tsStartTime.TotalSeconds <= nCurrentSecond &&
                    nCurrentSecond < tsEndTime.TotalSeconds)
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                }
                else
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                }

                if (tsEndTime.TotalSeconds < nCurrentSecond)
                {
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }

                break;

            case EnTodayTaskType.NationWar:

                break;

			case EnTodayTaskType.CreatureFarm:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.CreatureFarmQuest.RequiredHeroLevel)
				{
					UpdateTodayTaskLock(trTodayTaskSubMenuItem);
					textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.CreatureFarmQuest.RequiredHeroLevel);
					m_listCsTodayTaskCloseContent.Add(csTodayTask);
				}
				else
				{
					UpdateTodayTaskCompleteCheck(trTodayTaskSubMenuItem, csTodayTask);
				}

				break;

            case EnTodayTaskType.SafeTimeEvent:
                textComplete.gameObject.SetActive(false);
                buttonAttend.gameObject.SetActive(false);

                textDailyProgressCount.text = CsConfiguration.Instance.GetString("A50_TXT_01007");

                tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.SafeTimeEvent.StartTime);
                tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.SafeTimeEvent.EndTime);

                textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));
                trAttendTimeInfo.gameObject.SetActive(true);

                if (tsStartTime.TotalSeconds <= nCurrentSecond &&
                    nCurrentSecond < tsEndTime.TotalSeconds)
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                }
                else
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                }

                if (tsEndTime.TotalSeconds < nCurrentSecond)
                {
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    m_listCsTodayTaskOpenContent.Add(csTodayTask);
                }
                break;

            case EnTodayTaskType.WarMemory:

                bOpenDungeon = CsGameData.Instance.WarMemory.RequiredConditionType == 1 ? CsGameData.Instance.MyHeroInfo.Level <= CsGameData.Instance.WarMemory.RequiredHeroLevel : CsMainQuestManager.Instance.MainQuest != null && CsGameData.Instance.WarMemory.RequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo;

                if (bOpenDungeon)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.WarMemory.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    bAttend = false;

                    TimeSpan timeSpanCurrent = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date);
                    int nMaxScheduleId = CsGameData.Instance.WarMemory.WarMemoryScheduleList.Max(schedule => schedule.ScheduleId);
                    CsWarMemorySchedule csWarMemorySchedule = null;

                    foreach (var schedule in CsGameData.Instance.WarMemory.WarMemoryScheduleList)
                    {
                        if (timeSpanCurrent.TotalSeconds < schedule.StartTime)
                        {
                            csWarMemorySchedule = schedule;
                            break;
                        }
                        else if (schedule.StartTime <= timeSpanCurrent.TotalSeconds && timeSpanCurrent.TotalSeconds < schedule.EndTime)
                        {
                            bAttend = true;
                            break;
                        }
                        else
                        {
                            if (schedule.ScheduleId >= nMaxScheduleId)
                            {
                                csWarMemorySchedule = CsGameData.Instance.WarMemory.WarMemoryScheduleList[0];
                            }
                        }
                    }

                    if (bAttend)
                    {
                        m_listCsTodayTaskOpenContent.Add(csTodayTask);

                        trAttendTimeInfo.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                    }
                    else
                    {
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);

                        tsStartTime = System.TimeSpan.FromSeconds(csWarMemorySchedule.StartTime);
                        tsEndTime = System.TimeSpan.FromSeconds(csWarMemorySchedule.EndTime);

                        textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                        trAttendTimeInfo.gameObject.SetActive(true);
                        buttonAttend.gameObject.SetActive(false);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                    }

                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), CsDungeonManager.Instance.WarMemory.FreePlayCount, CsDungeonManager.Instance.WarMemory.FreeEnterCount);
                }

                break;

            case EnTodayTaskType.DragonNest:

                bOpenDungeon = CsGameData.Instance.DragonNest.RequiredConditionType == 1 ? CsGameData.Instance.MyHeroInfo.Level <= CsGameData.Instance.DragonNest.RequiredHeroLevel : CsMainQuestManager.Instance.MainQuest != null && CsGameData.Instance.DragonNest.RequiredMainQuestNo <= CsMainQuestManager.Instance.MainQuest.MainQuestNo;

                if (bOpenDungeon)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.DragonNest.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                }

                break;

            case EnTodayTaskType.AnkouTomb:

                bOpenDungeon = CsGameData.Instance.AnkouTomb.RequiredConditionType == 1 ? CsGameData.Instance.MyHeroInfo.Level <= CsGameData.Instance.AnkouTomb.RequiredHeroLevel : CsMainQuestManager.Instance.MainQuest != null && CsGameData.Instance.AnkouTomb.RequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo;

                if (bOpenDungeon)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.AnkouTomb.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    bAttend = false;

                    TimeSpan timeSpanCurrent = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date);
                    int nMaxScheduleId = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList.Max(schedule => schedule.ScheduleId);
                    CsAnkouTombSchedule csAnkouTombSchedule = null;

                    foreach (var schedule in CsGameData.Instance.AnkouTomb.AnkouTombScheduleList)
                    {
                        if (timeSpanCurrent.TotalSeconds < schedule.StartTime)
                        {
                            csAnkouTombSchedule = schedule;
                            break;
                        }
                        else if (schedule.StartTime <= timeSpanCurrent.TotalSeconds && timeSpanCurrent.TotalSeconds < schedule.EndTime)
                        {
                            bAttend = true;
                            break;
                        }
                        else
                        {
                            if (schedule.ScheduleId >= nMaxScheduleId)
                            {
                                csAnkouTombSchedule = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[0];
                            }
                        }
                    }

                    if (bAttend)
                    {
                        m_listCsTodayTaskOpenContent.Add(csTodayTask);

                        trAttendTimeInfo.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                    }
                    else
                    {
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);

                        tsStartTime = System.TimeSpan.FromSeconds(csAnkouTombSchedule.StartTime);
                        tsEndTime = System.TimeSpan.FromSeconds(csAnkouTombSchedule.EndTime);

                        textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                        trAttendTimeInfo.gameObject.SetActive(true);
                        buttonAttend.gameObject.SetActive(false);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                    }

                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), CsDungeonManager.Instance.WarMemory.FreePlayCount, CsDungeonManager.Instance.WarMemory.FreeEnterCount);
                }

                break;
                
            case EnTodayTaskType.TradeShip:

                bOpenDungeon = CsGameData.Instance.TradeShip.RequiredConditionType == 1 ? CsGameData.Instance.MyHeroInfo.Level <= CsGameData.Instance.TradeShip.RequiredHeroLevel : CsMainQuestManager.Instance.MainQuest != null && CsGameData.Instance.TradeShip.RequiredMainQuestNo < CsMainQuestManager.Instance.MainQuest.MainQuestNo;

                if (bOpenDungeon)
                {
                    UpdateTodayTaskLock(trTodayTaskSubMenuItem);
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01005"), CsGameData.Instance.TradeShip.RequiredHeroLevel);
                    m_listCsTodayTaskCloseContent.Add(csTodayTask);
                }
                else
                {
                    bAttend = false;

                    TimeSpan timeSpanCurrent = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date);
                    int nMaxScheduleId = CsGameData.Instance.TradeShip.TradeShipScheduleList.Max(schedule => schedule.ScheduleId);
                    CsTradeShipSchedule csTradeShipSchedule = null;

                    foreach (var schedule in CsGameData.Instance.TradeShip.TradeShipScheduleList)
                    {
                        if (timeSpanCurrent.TotalSeconds < schedule.StartTime)
                        {
                            csTradeShipSchedule = schedule;
                            break;
                        }
                        else if (schedule.StartTime <= timeSpanCurrent.TotalSeconds && timeSpanCurrent.TotalSeconds < schedule.EndTime)
                        {
                            bAttend = true;
                            break;
                        }
                        else
                        {
                            if (schedule.ScheduleId >= nMaxScheduleId)
                            {
                                csTradeShipSchedule = CsGameData.Instance.TradeShip.TradeShipScheduleList[0];
                            }
                        }
                    }

                    if (bAttend)
                    {
                        m_listCsTodayTaskOpenContent.Add(csTodayTask);

                        trAttendTimeInfo.gameObject.SetActive(false);
                        buttonAttend.gameObject.SetActive(true);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, true);
                    }
                    else
                    {
                        m_listCsTodayTaskCloseContent.Add(csTodayTask);

                        tsStartTime = System.TimeSpan.FromSeconds(csTradeShipSchedule.StartTime);
                        tsEndTime = System.TimeSpan.FromSeconds(csTradeShipSchedule.EndTime);

                        textAttendTime.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                        trAttendTimeInfo.gameObject.SetActive(true);
                        buttonAttend.gameObject.SetActive(false);

                        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
                    }

                    textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), CsDungeonManager.Instance.WarMemory.FreePlayCount, CsDungeonManager.Instance.WarMemory.FreeEnterCount);
                }

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayTaskLock(Transform trTodayTaskSubMenuItem)
    {
        Transform trSubMenuItemInfo = trTodayTaskSubMenuItem.Find("SubMenuItemInfo");

        for (int i = 0; i < trSubMenuItemInfo.childCount; i++)
        {
            trSubMenuItemInfo.GetChild(i).gameObject.SetActive(false);
        }

        Transform trTextName = trSubMenuItemInfo.Find("TextName");
        trTextName.gameObject.SetActive(true);

        Transform trTextDescriptionInfo = trSubMenuItemInfo.Find("TextDescriptionInfo");
        trTextDescriptionInfo.gameObject.SetActive(true);

        Transform trButtonAttend = trTodayTaskSubMenuItem.Find("ButtonAttend");
        trButtonAttend.gameObject.SetActive(false);

        Transform trImageLock = trTodayTaskSubMenuItem.Find("ImageLock");
        trImageLock.gameObject.SetActive(true);

        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayTaskCompleteCheck(Transform trTodayTaskSubMenuItem, CsTodayTask csTodayTask)
    {
        CsHeroTodayTask csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(csTodayTask.TaskId);
        int nProgressCount = 0;

        if (csHeroTodayTask == null)
        {
            nProgressCount = 0;

            if (CsGuildManager.Instance.Guild != null)
            {
                switch (csTodayTask.TodayTaskType)
                {
                    case EnTodayTaskType.GuildSupply:
                        if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.None && CsGuildManager.Instance.Guild.DailyGuildSupplySupportQuestStartCount == 1)
                        {
                            nProgressCount = 1;
                        }
                        else
                        {
                            nProgressCount = 0;
                        }

                        break;

                    case EnTodayTaskType.GuildHunting:
                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.GuildHunting) == null)
                        {
                            nProgressCount = CsGuildManager.Instance.DailyGuildHuntingQuestStartCount;
                        }
                        else
                        {
                            nProgressCount = csTodayTask.MaxCount;
                        }
                        break;

                    case EnTodayTaskType.GuildMissionQuest:
                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.GuildMission) == null)
                        {
                            nProgressCount = 0;
                        }
                        else
                        {
                            nProgressCount = csTodayTask.MaxCount;
                        }
                        break;

                    case EnTodayTaskType.WeeklyQuest:
                        if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest == null)
                        {

                        }
                        else
                        {
                            nProgressCount = CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo - 1;
                        }
                        break;
                }
            }
        }
        else
        {
            if (csTodayTask.TodayTaskType == EnTodayTaskType.DailyQuest)
            {
                nProgressCount = csHeroTodayTask.ProgressCount;

                if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
                    {
                        if (CsDailyQuestManager.Instance.HeroDailyQuestList[i].IsAccepted && !CsDailyQuestManager.Instance.HeroDailyQuestList[i].Completed)
                        {
                            nProgressCount--;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                nProgressCount = csHeroTodayTask.ProgressCount;
            }
        }

        Text textDailyProgressCount = trTodayTaskSubMenuItem.Find("SubMenuItemInfo/ProgressInfo/TextDailyProgressCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDailyProgressCount);

        if (csTodayTask.TodayTaskType == EnTodayTaskType.WeeklyQuest)
        {
            textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01009"), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo - 1, CsGameData.Instance.WeeklyQuest.RoundCount);
        }
        else
        {
            textDailyProgressCount.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01001"), nProgressCount, csTodayTask.MaxCount);
        }

        Transform trButtonAttend = trTodayTaskSubMenuItem.Find("ButtonAttend");
        Text textComplete = trTodayTaskSubMenuItem.Find("TextComplete").GetComponent<Text>();

        if (nProgressCount < csTodayTask.MaxCount)
        {
            textComplete.gameObject.SetActive(false);
            trButtonAttend.gameObject.SetActive(true);
            m_listCsTodayTaskOpenContent.Add(csTodayTask);
        }
        else
        {
            if (csHeroTodayTask != null)
            {
                textComplete.text = CsConfiguration.Instance.GetString("A50_TXT_00001");
                textComplete.color = CsUIData.Instance.ColorGreen;
            }
            else
            {
                switch (csTodayTask.TodayTaskType)
                {
                    case EnTodayTaskType.GuildSupply:
                        textComplete.text = CsConfiguration.Instance.GetString("A50_TXT_00006");
                        textComplete.color = CsUIData.Instance.ColorRed;
                        break;

                    case EnTodayTaskType.GuildHunting:
                        textComplete.text = CsConfiguration.Instance.GetString("A50_TXT_00001");
                        textComplete.color = CsUIData.Instance.ColorGreen;
                        break;
                }
            }

            trButtonAttend.gameObject.SetActive(false);
            textComplete.gameObject.SetActive(true);
            m_listCsTodayTaskCloseContent.Add(csTodayTask);
        }
		Debug.Log("taskid : " + csTodayTask.TaskId + ", progressCount : " + nProgressCount + ", maxCount : " + csTodayTask.MaxCount);
        UpdateTodayTaskAttendCheck(trTodayTaskSubMenuItem, nProgressCount < csTodayTask.MaxCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayTaskAttendCheck(Transform trTodayTaskSubMenuItem, bool bIsAttend)
    {
        Image imageIcon = trTodayTaskSubMenuItem.Find("ImageIcon").GetComponent<Image>();

        Transform trSubMenuItemInfo = trTodayTaskSubMenuItem.Find("SubMenuItemInfo");

        Text textName = trSubMenuItemInfo.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        if (bIsAttend)
        {
            imageIcon.color = new Color(1, 1, 1, 1);
            textName.color = new Color32(255, 214, 80, 255);
        }
        else
        {
            imageIcon.color = new Color(1, 1, 1, 0.7f);
            textName.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupTodayTaskInfo(CsTodayTask csTodayTask)
    {
        Transform trImageBackground = m_trPopupTodayTaskInfo.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = csTodayTask.Name;

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickPopupTodayTaskInfoClose());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageTodayTaskIcon = trImageBackground.Find("ImageTodayTaskIcon").GetComponent<Image>();
        imageTodayTaskIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/todaytask_" + csTodayTask.TaskId);

        Text textDescription = trImageBackground.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString(csTodayTask.Description);

		Text textAchievement = trImageBackground.Find("TextAchievement").GetComponent<Text>();
		CsUIData.Instance.SetFont(textAchievement);
		textAchievement.text = CsConfiguration.Instance.GetString("A50_TXT_00002");

		Text textAchievementPoint = trImageBackground.Find("TextAchievementCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(textAchievementPoint);
		textAchievementPoint.text = csTodayTask.AchievementPoint.ToString("#,##0");

		Text textEventTime = trImageBackground.Find("TextEventTime").GetComponent<Text>();
		CsUIData.Instance.SetFont(textEventTime);
		textEventTime.text = CsConfiguration.Instance.GetString("A50_TXT_00004");

		Text textEventTimeInfo = trImageBackground.Find("TextEventTimeInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEventTimeInfo);

        System.TimeSpan tsElapsedTime = CsGameData.Instance.MyHeroInfo.CurrentDateTime - CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;
        System.TimeSpan tsStartTime;
        System.TimeSpan tsEndTime;

        switch (csTodayTask.TodayTaskType)
        {
            case EnTodayTaskType.DimensionInfiltrationEvent:
                tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.DimensionInfiltrationEvent.StartTime);
                tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.DimensionInfiltrationEvent.EndTime);

                textEventTimeInfo.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                break;
            case EnTodayTaskType.HolyWarQuest:
                if (CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count - 1].EndTime < tsElapsedTime.TotalSeconds)
                {
                    tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0].StartTime);
                    tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0].EndTime);
                }
                else if (tsElapsedTime.TotalSeconds < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0].StartTime)
                {
                    tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0].StartTime);
                    tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[0].EndTime);
                }
                else
                {
                    int nIndex = 0;

                    for (int i = 0; i < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count; i++)
                    {
                        if (CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].StartTime < tsElapsedTime.TotalSeconds &&
                            tsElapsedTime.TotalSeconds < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].EndTime)
                        {
                            nIndex = i;
                            break;
                        }

                        if (i == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i - 1].EndTime < tsElapsedTime.TotalSeconds &&
                                tsElapsedTime.TotalSeconds < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].StartTime)
                            {
                                nIndex = i;
                                break;
                            }
                        }
                    }

                    tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[nIndex].StartTime);
                    tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[nIndex].EndTime);
                }

                textEventTimeInfo.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));

                break;

			case EnTodayTaskType.FieldBoss:
			{
				int nIndex = 0;

				for (int i = 0; i < CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList.Count; i++)
				{
					if (tsElapsedTime.TotalSeconds < CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[i].EndTime)
					{
						nIndex = i;
						break;
					}
				}

				tsStartTime = System.TimeSpan.FromSeconds(CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[nIndex].StartTime);
				tsEndTime = System.TimeSpan.FromSeconds(CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[nIndex].EndTime);

				textEventTimeInfo.text = string.Format(CsConfiguration.Instance.GetString("A50_TXT_01003"), tsStartTime.Hours.ToString("#0"), tsStartTime.Minutes.ToString("0#"), tsEndTime.Hours.ToString("0#"), tsEndTime.Minutes.ToString("0#"));
			}
				break;

            default:
                textEventTimeInfo.text = CsConfiguration.Instance.GetString(csTodayTask.EventTimeText);
                break;
        }

		Text textEventReward = trImageBackground.Find("TextEventReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEventReward);
        textEventReward.text = CsConfiguration.Instance.GetString("A50_TXT_00005");

        Transform trTodayTaskAvailableRewardContent = trImageBackground.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < trTodayTaskAvailableRewardContent.childCount; i++)
        {
            trTodayTaskAvailableRewardContent.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < csTodayTask.TodayTaskAvailableRewardList.Count; i++)
        {
            CsItem csItem = csTodayTask.TodayTaskAvailableRewardList[i].Item;
            Transform trItemSlot = trTodayTaskAvailableRewardContent.Find("ItemSlot" + i);

            if (trItemSlot == null)
            {
                trItemSlot = Instantiate(m_goItemSlot, trTodayTaskAvailableRewardContent).transform;
                trItemSlot.name = "ItemSlot" + i;
            }

            CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, false, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
            trItemSlot.gameObject.SetActive(true);

            Button buttonItemSlot = trItemSlot.GetComponent<Button>();
            buttonItemSlot.onClick.RemoveAllListeners();
            buttonItemSlot.onClick.AddListener(() => OpenPopupItemInfo(csItem));
        }

        m_trPopupTodayTaskInfo.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTodayTaskItemDisplay(bool bIson)
    {
        if (bIson)
        {
            for (int i = 0; i < m_listCsTodayTaskOpenContent.Count; i++)
            {
                Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskOpenContent[i].TaskId);

                if (trTodayTaskItem == null)
                {
                    continue;
                }
                else
                {
                    if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.TodayTaskLimit)
                    {
                        trTodayTaskItem.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (m_listCsTodayTaskOpenContent[i].IsRecommend)
                        {
                            trTodayTaskItem.gameObject.SetActive(true);
                        }
                        else
                        {
                            trTodayTaskItem.gameObject.SetActive(false);
                        }
                    }
                }
            }

            for (int i = 0; i < m_listCsTodayTaskCloseContent.Count; i++)
            {
                Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskCloseContent[i].TaskId);

                if (trTodayTaskItem == null)
                {
                    continue;
                }
                else
                {
                    trTodayTaskItem.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_listCsTodayTaskOpenContent.Count; i++)
            {
                Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskOpenContent[i].TaskId);

                if (trTodayTaskItem == null)
                {
                    continue;
                }
                else
                {
                    trTodayTaskItem.gameObject.SetActive(true);
                }
            }

            for (int i = 0; i < m_listCsTodayTaskCloseContent.Count; i++)
            {
                Transform trTodayTaskItem = m_trTodayTaskSubMenuItemList.Find("TodayTaskItem" + m_listCsTodayTaskCloseContent[i].TaskId);

                if (trTodayTaskItem == null)
                {
                    continue;
                }
                else
                {
                    trTodayTaskItem.gameObject.SetActive(true);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(CsTodayTask x, CsTodayTask y)
    {
        if (x.SortNo > y.SortNo) return 1;
        else if (x.SortNo < y.SortNo) return -1;
        return 0;
    }

    #region ItemCount

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

    #endregion ItemCount

    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(m_trPopupTodayTask.gameObject);
    }

    #region PopupItemInfo

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem)
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(csItem));
        }
        else
        {
            UpdatePopupItemInfo(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItem csItem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        UpdatePopupItemInfo(csItem);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupItemInfo(CsItem csItem)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupTodayTaskInfo);
        m_trPopupItemInfo = goPopupItemInfo.transform;

        CsPopupItemInfo csPopupItemInfo = m_trPopupItemInfo.GetComponent<CsPopupItemInfo>();
        csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, 0, false, -1, false);
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