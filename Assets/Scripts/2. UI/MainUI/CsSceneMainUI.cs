using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ClientCommon;
using UnityEngine.EventSystems;
using System.Linq;

public class CsSceneMainUI : CsUpdateableMonoBehaviour
{
    Canvas m_canvas;
    Transform m_trCanvas;
    Transform m_trCanvas1;
    Transform m_trCanvas2;
	Transform m_trCanvas3;
	Transform m_trSubMenu1;
	
    Transform m_trPopupQuickUse;
    Transform m_trPopupMain;
    Transform m_trPopupSupport;
    Transform m_trPopupTodayTask;
    Transform m_trPopupSetting;
    Transform m_trUserReference;
    Transform m_trLoadingImage;
	Transform m_trItemInfo;

    CsPopupMain m_csPopupMain;
	CsPopupItemInfo m_csPopupItemInfo;

	GameObject m_goPopupItemInfo;
    GameObject m_goPopupMain;
    GameObject m_goPopupRevive;
    GameObject m_goPopupSupport;
    GameObject m_goPopupTodayTask;
    GameObject m_goPopupSetting;
    GameObject m_goPopupQuickUse;
    GameObject m_goPopupDropObject;
    GameObject m_goPopupGetCard;
    GameObject m_goPopupUserReference;
    GameObject m_goPopupGuildApply;
    GameObject m_goPopupNationWarAttend;
    GameObject m_goPopupNationWar;
    GameObject m_goPopupTamingMonster;
	GameObject m_goPopupOpenGift;
	GameObject m_goPopupRookieGift;
    GameObject m_goPopupOpen7Day;
	GameObject m_goPopupRetrieval;
	GameObject m_goPopupChargingEvent;
	GameObject m_goPopupFieldBoss;
	GameObject m_goPopupHelp;
	GameObject m_goPopupCharging;
	GameObject m_goPopupSmallAmountCharging;
    GameObject m_goPopupOrdeal;
    GameObject m_goPopupMountPotionAttr = null;

    Transform m_trPopupRestReward;
    Transform m_trPopupRevive;
    Transform m_trPopupGuildApply;
    Transform m_trPopupNationWarAttend;
    Transform m_trPopupNationWar;
    Transform m_trPanelPopup;
    Transform m_trPopup;
    Transform m_trPopupList;
    Transform m_trPopupOrdeal;
    Transform m_trPopupMountPotionAttr;

    Text m_textLoadingPercent;
    Slider m_sliderLoading;

    CsPanelModal m_csPanelModal;
    CsPanelTimerModal m_csPanelTimerModal;

    CsUIJoyStick m_csUIJoyStick;
    CsPopupUserReference m_csPopupUserReference;
    CsPopupGuildApply m_csPopupGuildApply;

    float m_flTime = 0;
    bool m_bIsProcess = false;
    int m_nCurMask;
    int m_nPortalId;
    float m_flNationWarCallRemainingTime = 0.0f;

    //DateTime m_dtOld;   // 날짜비교

    List<CsHeroObject> m_listHeroObjectDisplay = new List<CsHeroObject>();
    List<CsDropObject> m_listcsDropObject = new List<CsDropObject>();
	List<CsHeroCreatureCard> m_listCsHeroCreatureCardList = new List<CsHeroCreatureCard>();

	bool m_bOpenHeroObjectGot = false;
    IEnumerator m_iEnumeratorQuickUse;
    IEnumerator m_iEnumeratorOpenPopupRevive;

    List<CsGuildCall> m_listCsGuildCall = new List<CsGuildCall>();
    List<CsNationCall> m_listCsNationCall = new List<CsNationCall>();
    Guid m_guidNationWarCall = Guid.Empty;

	bool m_bImmPickBoxUse = false;   // 사용레벨즉시사용

	float m_flDeltaTime = 0.0f;

    bool m_bSleepMode = false;
    float m_flSleepModeTimer = 0f;

    EnBattleMode m_enBattleMode = EnBattleMode.None;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        // 그래픽 퀄리티 세팅.  0,2,4
        switch (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Graphic))
        {
            case 0:				
				QualitySettings.SetQualityLevel(0);
				Screen.SetResolution(960, 540, true);
				break;
            case 1:
				QualitySettings.SetQualityLevel(2);
				Screen.SetResolution(1280, 720, true);
				break;
            case 2:
				QualitySettings.SetQualityLevel(4);
				Screen.SetResolution(1280, 720, true);
				break;
			default:
                QualitySettings.SetQualityLevel(2);
                break;
        }
		CsGameEventToUI.Instance.OnEventChangeResolution();

		//프레임 셋팅
		switch (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Frame))
        {
            case 0:
                Application.targetFrameRate = 20;
                break;
            case 1:
                Application.targetFrameRate = 30;
                break;
            case 2:
                //Application.targetFrameRate = 45;
				Application.targetFrameRate = 200; // 임시.
				break;
            default:
                Application.targetFrameRate = 30;
                break;
        }

        CsUIData.Instance.SetAudio(false);  // 오디오 세팅
        m_flSleepModeTimer = Time.realtimeSinceStartup + CsGameConfig.Instance.OptimizationModeWaitingTime;

        GameObject goCanvas = GameObject.Find("Canvas");
        m_canvas = goCanvas.GetComponent<Canvas>();
        m_trCanvas = goCanvas.transform;
        m_trCanvas1 = GameObject.Find("Canvas1").transform;
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
		m_trCanvas3 = GameObject.Find("Canvas3").transform;
		m_trSubMenu1 = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1");

		m_csUIJoyStick = m_trCanvas.Find("PanelJoystick").GetComponent<CsUIJoyStick>();

        m_trLoadingImage = m_trCanvas3.Find("PanelLoading");
        m_trPanelPopup = m_trCanvas2.Find("PanelPopup");
        m_trPopup = m_trCanvas.Find("Popup");
        m_trPopupList = m_trCanvas2.Find("PopupList");

        CsAccomplishmentManager.Instance.CheckAccomplishmentComplete();
        CsCommandEventManager.Instance.Initialize();

		CsRplzSession.Instance.EventConnected += OnEventConnected;
		CsRplzSession.Instance.EventDisconnected += OnEventDisconnected;

		CsGameEventUIToUI.Instance.EventHeroLogin += OnEventHeroLogin;
		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
        CsGameEventUIToUI.Instance.EventAlert += OnEventAlert;
        CsGameEventUIToUI.Instance.EventConfirm += OnEventConfirm;
        CsGameEventUIToUI.Instance.EventCommonModalClose += OnEventCommonModalClose;
		CsGameEventUIToUI.Instance.EventOpenPopupItemInfo += OnEventOpenPopupItemInfo;
		CsGameEventUIToUI.Instance.EventSinglePopupOpen += OnEventSinglePopupOpen;
		CsGameEventUIToUI.Instance.EventOpenPopupFieldBoss += OnEventOpenPopupFieldBoss;
		CsGameEventUIToUI.Instance.EventOpenPopupHelp += OnEventOpenPopupHelp;
		CsGameEventUIToUI.Instance.EventOpenPopupCharging += OnEventOpenPopupCharging;
		CsGameEventUIToUI.Instance.EventOpenPopupSmallAmountCharging += OnEventOpenPopupSmallAmountCharging;
		CsGameEventUIToUI.Instance.EventOpenPopupOrdealQuest += OnEventOpenPopupOrdealQuest;
		CsGameEventUIToUI.Instance.EventPopupOpen += OnEventPopupOpen;
		CsGameEventUIToUI.Instance.EventPopupClose += OnEventPopupClose;
		CsGameEventUIToUI.Instance.EventTimerConfirm += OnEventTimerConfirm;
        CsGameEventUIToUI.Instance.EventTimerModalClose += OnEventTimerModalClose;
        CsGameEventUIToUI.Instance.EventPlayerPrefsKeySet += OnEventPlayerPrefsKeySet;

        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished += OnEventReturnScrollUseFinished;  // 귀환주문서 사용완료
        CsGameEventUIToUI.Instance.EventContinentSaftyRevive += OnEventContinentSaftyRevive;        // 안전부활
        CsGameEventUIToUI.Instance.EventImmediateRevive += OnEventImmediateRevive;                  // 즉시부활
        CsGameEventUIToUI.Instance.EventOpenUserReference += OnEventOpenUserReference;
        CsGameEventUIToUI.Instance.EventOpenFriendRefernce += OnEventOpenUserFriendRefernce;
        CsGameEventUIToUI.Instance.EventOpenGuildMemberReference += OnEventOpenGuildMemberReference;
        CsGameEventUIToUI.Instance.EventOpenNationNoblesseReference += OnEventOpenNationNoblesseReference;


        CsGameEventUIToUI.Instance.EventHeroObjectGot += OnEventHeroObjectGot;          // 아이템,장비 획득

        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;    // 인게임 로딩완료
        CsGameEventToUI.Instance.EventHeroDead += OnEventHeroDead;                      // 영웅사망
        CsGameEventToUI.Instance.EventPortalEnter += OnEventPortalEnter;                // 포탈입장
        CsGameEventUIToUI.Instance.EventPortalEnter += OnEventPortalEnter;
        CsCartManager.Instance.EventCartPortalEnter += OnEventCartPortalEnter;          // 카트포탈입장
        CsGameEventUIToUI.Instance.EventHeroInfo += OnEventHeroInfo;                    // 다른 유저 정보
        CsGuildManager.Instance.EventGuildMemberTabInfo += OnEventGuildMemberTabInfo;   // 길드 정보 갱신

        //파티
        CsGameEventUIToUI.Instance.EventPartyInvitationArrived += OnEventPartyInvitationArrived;                    //파티초대 알림도착
        CsGameEventUIToUI.Instance.EventPartyApplicationArrived += OnEventPartyApplicationArrived;                  //파티가입 알림도착
        CsGameEventUIToUI.Instance.EventPartyApplicationLifetimeEnded += OnEventPartyApplicationLifetimeEnded;      //파티가입알림시간초과
        CsGameEventUIToUI.Instance.EventPartyInvitationLifetimeEnded += OnEventPartyInvitationLifetimeEnded;        //파티초대알림시간초과
        CsGameEventUIToUI.Instance.EventPartyMemberEnter += OnEventPartyMemberEnter;                                //파티멤버추가
        CsGameEventUIToUI.Instance.EventPartyMemberExit += OnEventPartyMemberExit;                                  //파티멤버삭제
        CsGameEventUIToUI.Instance.EventPartyBanished += OnEventPartyBanished;                                      //파티강퇴당함(강퇴당한자)
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept += OnEventPartyInvitationAccept;                      //파티초대수락(초대받은자)
        CsGameEventUIToUI.Instance.EventPartyInvitationRefuse += OnEventPartyInvitationRefuse;                      //파티초대거절(초대받은자)
        CsGameEventUIToUI.Instance.EventPartyInvitationRefused += OnEventPartyInvitationRefused;                    //파티초대거절당함(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationAccept += OnEventPartyApplicationAccept;                    //파티가입수락(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationRefuse += OnEventPartyApplicationRefuse;                    //파티가입거절(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted += OnEventPartyApplicationAccepted;                //파티가입수락당함(가입신청한자)
        CsGameEventUIToUI.Instance.EventPartyApplicationRefused += OnEventPartyApplicationRefused;                  //파티가입거절당함(가입신청한자)
        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;                                          //파티생성
        CsGameEventUIToUI.Instance.EventPartyDisbanded += OnEventPartyDisbanded;                                    //파티해산
        CsGameEventUIToUI.Instance.EventPartyMasterChanged += OnEventPartyMasterChanged;                            //파티장변경

        //길드
        CsGuildManager.Instance.EventGuildInvitationArrived += OnEventGuildInvitationArrived;                   //길드초대도착
        CsGuildManager.Instance.EventGuildInvitationAccept += OnEventGuildInvitationAccept;                     //길드초대수락
        CsGuildManager.Instance.EventGuildInvitationRefuse += OnEventGuildInvitationRefuse;                     //초대거절
        CsGuildManager.Instance.EventGuildInvitationRefused += OnEventGuildInvitationRefused;                   //초대거절받음
        CsGuildManager.Instance.EventGuildInvitationLifetimeEnded += OnEventGuildInvitationLifetimeEnded;       //초대시간초과
        CsGuildManager.Instance.EventGuildApplicationAccepted += OnEventGuildApplicationAccepted;               //길드가입신청수락
        CsGuildManager.Instance.EventGuildApplicationRefused += OnEventGuildApplicationRefused;
        CsGuildManager.Instance.EventGuildAppointed += OnEventGuildAppointed;
        CsGuildManager.Instance.EventGuildMasterTransferred += OnEventGuildMasterTransferred;
        CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildExit += OnEventGuildExit;
        CsGuildManager.Instance.EventGuildTerritoryExit += OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter += OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildCallTransmission += OnEventGuildCallTransmission;
        CsGuildManager.Instance.EventGuildCalled += OnEventGuildCalled;
        CsGuildManager.Instance.EventGuildApply += OnEventGuildApply;
        CsGuildManager.Instance.EventGuildTerritoryRevive += OnEventGuildTerritoryRevive;

        CsGameEventUIToUI.Instance.EventDropObjectLooted += OnEventDropObjectLooted;

        CsGameEventUIToUI.Instance.EventContinentTransmission += OnEventContinentTransmission;
        CsGameEventUIToUI.Instance.EventNationTransmission += OnEventNationTransmission;

        //CsGameEventUIToUI.Instance.EventContinentExitForStoryDungeonEnter += OnEventContinentExitForStoryDungeonEnter;

        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter += OnEventContinentExitForMainQuestDungeonEnter;

        //m_dtOld = CsGameData.Instance.MyHeroInfo.CurrentDateTime;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon += OnEventMainQuestDungeonAbandon;

        //메인UI 숨기기
        CsGameEventToUI.Instance.EventHideMainUI += OnEventHideMainUI;
        CsGameEventUIToUI.Instance.EventVisibleMainUI += OnEventVisibleMainUI;

        //스토리던전
        CsDungeonManager.Instance.EventContinentExitForStoryDungeonEnter += OnEventContinentExitForStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonAbandon += OnEventStoryDungeonAbandon;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;

        //경험치 던전
        CsDungeonManager.Instance.EventContinentExitForExpDungeonEnter += OnEventContinentExitForExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonExit += OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonAbandon += OnEventExpDungeonAbandon;
        CsDungeonManager.Instance.EventExpDungeonBanished += OnEventExpDungeonBanished;

        //골드 던전
        CsDungeonManager.Instance.EventContinentExitForGoldDungeonEnter += OnEventContinentExitForGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonExit += OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonAbandon += OnEventGoldDungeonAbandon;
        CsDungeonManager.Instance.EventGoldDungeonBanished += OnEventGoldDungeonBanished;

        //지하 미로
        CsDungeonManager.Instance.EventContinentExitForUndergroundMazeEnter += OnEventContinentExitForUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazeExit += OnEventUndergroundMazeExit;
        CsDungeonManager.Instance.EventUndergroundMazeBanished += OnEventUndergroundMazeBanished;
        CsDungeonManager.Instance.EventUndergroundMazeRevive += OnEventUndergroundMazeRevive;
        CsDungeonManager.Instance.EventUndergroundMazePortalEnter += OnEventUndergroundMazePortalEnter;
        CsDungeonManager.Instance.EventUndergroundMazeTransmission += OnEventUndergroundMazeTransmission;

        //고대 유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter += OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge += OnEventArtifactRoomNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge += OnEventArtifactRoomBanishedForNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomAbandon += OnEventArtifactRoomAbandon;
        CsDungeonManager.Instance.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit += OnEventArtifactRoomExit;

        //고대인의 유적
        CsDungeonManager.Instance.EventContinentExitForAncientRelicEnter += OnEventContinentExitForAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicAbandon += OnEventAncientRelicAbandon;
        CsDungeonManager.Instance.EventAncientRelicBanished += OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicExit += OnEventAncientRelicExit;

        //검투대회
        CsDungeonManager.Instance.EventContinentExitForFieldOfHonorChallenge += OnEventContinentExitForFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorAbandon += OnEventFieldOfHonorAbandon;
        CsDungeonManager.Instance.EventFieldOfHonorBanished += OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorExit += OnEventFieldOfHonorExit;

        // 영혼을 탐하는 자
        CsDungeonManager.Instance.EventContinentExitForSoulCoveterEnter += OnEventContinentExitForSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterAbandon += OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished += OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventSoulCoveterExit += OnEventSoulCoveterExit;

        //용맹의 증명
        CsDungeonManager.Instance.EventContinentExitForProofOfValorEnter += OnEventContinentExitForProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorAbandon += OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBanished += OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorExit += OnEventProofOfValorExit;
		CsDungeonManager.Instance.EventProofOfValorGetCreatureCard += OnEventProofOfValorGetCreatureCard;

		// 지혜의 신전
		CsDungeonManager.Instance.EventContinentExitForWisdomTempleEnter += OnEventContinentExitForWisdomTempleEnter;
		CsDungeonManager.Instance.EventWisdomTempleExit += OnEventWisdomTempleExit;
		CsDungeonManager.Instance.EventWisdomTempleAbandon += OnEventWisdomTempleAbandon;
		CsDungeonManager.Instance.EventWisdomTempleBanished += OnEventWisdomTempleBanished;
		CsDungeonManager.Instance.EventWisdomTempleStepCompleted += OnEventWisdomTempleStepCompleted;
		CsDungeonManager.Instance.EventWisdomTemplePuzzleRewardObjectInteractionFinished += OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;
		CsDungeonManager.Instance.EventWisdomTempleBossMonsterKill += OnEventWisdomTempleBossMonsterKill;

        // 정예 던전
        CsDungeonManager.Instance.EventContinentExitForEliteDungeonEnter += OnEventContinentExitForEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonAbandon += OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished += OnEventEliteDungeonBanished;
        CsDungeonManager.Instance.EventEliteDungeonExit += OnEventEliteDungeonExit;

		// 유적 탈환
		CsDungeonManager.Instance.EventContinentExitForRuinsReclaimEnter += OnEventContinentExitForRuinsReclaimEnter;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
		CsDungeonManager.Instance.EventRuinsReclaimClear += OnEventRuinsReclaimClear;

        // 무한 대전
        CsDungeonManager.Instance.EventContinentExitForInfiniteWarEnter += OnEventContinentExitForInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart += OnEventInfiniteWarStart;
        CsDungeonManager.Instance.EventInfiniteWarAbandon += OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarExit += OnEventInfiniteWarExit;
		
		// 공포의 제단
		CsDungeonManager.Instance.EventContinentExitForFearAltarEnter += OnEventContinentExitForFearAltarEnter;
		CsDungeonManager.Instance.EventFearAltarAbandon += OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;
		CsDungeonManager.Instance.EventFearAltarExit += OnEventFearAltarExit;
		CsGameEventUIToUI.Instance.EventOpenPopupHalidomCollection += OnEventOpenPopupHalidomCollection;	// 성물 수집 팝업

        // 전쟁의 기억
        CsDungeonManager.Instance.EventContinentExitForWarMemoryEnter += OnEventContinentExitForWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryExit += OnEventWarMemoryExit;

        // 오시리스 방
        CsDungeonManager.Instance.EventContinentExitForOsirisRoomEnter += OnEventContinentExitForOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomAbandon += OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished += OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomExit += OnEventOsirisRoomExit;

		// 전기퀘스트 던전
		CsDungeonManager.Instance.EventContinentExitForBiographyQuestDungeonEnter += OnEventContinentExitForBiographyQuestDungeonEnter;
		CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon += OnEventBiographyQuestDungeonAbandon;
		CsDungeonManager.Instance.EventBiographyQuestDungeonBanished += OnEventBiographyQuestDungeonBanished;
		CsDungeonManager.Instance.EventBiographyQuestDungeonExit += OnEventBiographyQuestDungeonExit;

        // 용의 둥지
        CsDungeonManager.Instance.EventContinentExitForDragonNestEnter += OnEventContinentExitForDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestAbandon += OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished += OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestExit += OnEventDragonNestExit;

        // 무역선 탈환
        CsDungeonManager.Instance.EventContinentExitForTradeShipEnter += OnEventContinentExitForTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipAbandon += OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipExit += OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipObjectDestructionReward += OnEventTradeShipObjectDestructionReward;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventContinentExitForAnkouTombEnter += OnEventContinentExitForAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombExit += OnEventAnkouTombExit;

		// 팀 전장
		CsDungeonManager.Instance.EventContinentExitForTeamBattlefieldEnter += OnEventContinentExitForTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit += OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon += OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished += OnEventTeamBattlefieldBanished;

        CsErrorMessageManager.Instance.EventAlert += OnEventAlert;

        // 대륙
        CsGameEventUIToUI.Instance.EventContinentBanished += OnEventContinentBanished;

        // 국가 관직 임명
        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed += OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationCallTransmission += OnEventNationCallTransmission;
        CsGameEventUIToUI.Instance.EventNationCalled += OnEventNationCalled;

        // 국가전
        CsNationWarManager.Instance.EventNationWarStart += OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarJoin += OnEventNationWarJoin;
        CsNationWarManager.Instance.EventNationWarTransmission += OnEventNationWarTransmission;
        CsNationWarManager.Instance.EventNationWarCall += OnEventNationWarCall;
        CsNationWarManager.Instance.EventNationWarCallTransmission += OnEventNationWarCallTransmission;
        CsNationWarManager.Instance.EventNationWarRevive += OnEventNationWarRevive;
        CsNationWarManager.Instance.EventNationWarNpcTransmission += OnEventNationWarNpcTransmission;

        CsGameEventToUI.Instance.EventContinentSaftySceneLoad += OnEventContinentSaftySceneLoad;

		// 크리처카드획득
		CsGameEventUIToUI.Instance.EventGetHeroCreatureCard += OnEventGetHeroCreatureCard;

		// 레벨업
		CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;

        // 몬스터 테이밍
        CsGameEventToUI.Instance.EventCreateTameMonster += OnEventCreateTameMonster;
        CsGameEventToUI.Instance.EventDeleteTameMonster += OnEventDeleteTameMonster;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealStart += OnEventGroggyMonsterItemStealStart;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealCancel += OnEventGroggyMonsterItemStealCancel;
        CsGameEventToUI.Instance.EventGroggyMonsterItemStealCancel += OnEventGroggyMonsterItemStealCancelByIngame;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealFinished += OnEventGroggyMonsterItemStealFinished;

		CsGameEventUIToUI.Instance.EventStartOrdealQuestMission += OnEventStartOrdealQuestMission;

		// 던전입장실패
		CsDungeonManager.Instance.EventDungeonEnterFail += OnEventDungeonEnterFail;

        // 계정중복로그인
        CsGameEventUIToUI.Instance.EventAccountLoginDuplicated += OnEventAccountLoginDuplicated;

        // 친구
        CsFriendManager.Instance.EventFriendApplicationReceived += OnEventFriendApplicationReceived;
        CsFriendManager.Instance.EventFriendApplicationAccepted += OnEventFriendApplicationAccepted;
        CsFriendManager.Instance.EventFriendApplicationRefused += OnEventFriendApplicationRefused;
        CsFriendManager.Instance.EventFriendApplicationAccept += OnEventFriendApplicationAccept;
        CsFriendManager.Instance.EventFriendApplicationRefuse += OnEventFriendApplicationRefuse;

        CsFriendManager.Instance.EventBlacklistEntryAdd += OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventBlacklistEntryDelete += OnEventBlacklistEntryDelete;

        // 절전
        CsGameEventUIToUI.Instance.EventSleepModeReset += OnEventSleepModeReset;

		// 안전 모드
        CsGameEventToIngame.Instance.EventAutoBattleStart += OnEventAutoBattleStart;

		// 크리쳐카드 오픈 애니메이션 체크
		CsOpenToastManager.Instance.EventCheckCreatureCardAnimation += OnEventCheckCreatureCardAnimation;

        // 증정
        CsPresentManager.Instance.EventHeroPresent += OnEventHeroPresent;
        CsPresentManager.Instance.EventPresentReplyReceived += OnEventPresentReplyReceived;
        CsPresentManager.Instance.EventPresentSend += OnEventPresentSend;

		// 크리처
		CsGameEventUIToUI.Instance.EventGetHeroCreature += OnEventGetHeroCreature;

        // 길드 축복
        CsGameEventUIToUI.Instance.EventOpenPopupGuildBlessing += OnEventOpenPopupGuildBlessing;

        // 국가 동맹
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept += OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;

        CsGameEventUIToUI.Instance.EventOpenPopupMountAttrPotion += OnEventOpenPopupMountAttrPotion;
        CsGameEventUIToUI.Instance.EventOpenPopupCostumeEffect += OnEventOpenPopupCostumeEffect;
        CsGameEventUIToUI.Instance.EventOpenPopupHeroPotionAttr += OnEventOpenPopupHeroPotionAttr;

		// 웹뷰
		CsGameEventUIToUI.Instance.EventOpenWebView += OnEventOpenWebView;

		FishingPartyReset();
    }

    protected override void Initialize()
    {
		// 인게임씬로드.
		LoadInGameScene();

        m_csPanelModal = m_trCanvas3.Find("PanelModal").GetComponent<CsPanelModal>();
        m_csPanelTimerModal = m_trCanvas3.Find("PanelTimerModal").GetComponent<CsPanelTimerModal>();

        // 휴식보상
        StartCoroutine(OpenPopupRestRewardCoroutine());

        // 리소스 로드
        StartCoroutine(LoadResourceCoroutine());

        // 획득 메인장비, 보조장비, 아이템 장착 또는 사용.
        StartCoroutine(DisplayGetHeroObjectCoroutine());

        // 획득 드랍 오브젝트
        StartCoroutine(DisplayGetDropObjectCoroutine());

        // 획득 카드
        StartCoroutine(DisplayHeroCreatureCardCoroutine());
    }

	int m_nTick = 0;

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        CsRplzSession.Instance.Service();

        CsGameConfig.Instance.ScaleFactor = m_canvas.scaleFactor;

		m_flDeltaTime += (Time.deltaTime - m_flDeltaTime) * 0.1f;       // 임시 프레임 표시

		if (m_flTime + 1f < Time.time)
        {
            // 자동물약사용.
            if (CsGameData.Instance.MyHeroInfo.Hp > 0 && !CsIngameData.Instance.MyHeroDead && CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.HpPotion))
            {
                if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoPortion) == 1 && CsUIData.Instance.HpPotionRemainingCoolTime - Time.realtimeSinceStartup <= 0)
                {
                    float flPortionRate = CsGameConfig.Instance.HpPotionAutoUseHpRate / 10000.0f;
                    float flHpRate = (CsGameData.Instance.MyHeroInfo.Hp * 1.0f) / CsGameData.Instance.MyHeroInfo.MaxHp;

                    if (flPortionRate >= flHpRate)
                    {
                        if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsUIData.Instance.HpPotionId) > 0)
                        {
                            for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
                            {
                                if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.Item)
                                {
                                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].InventoryObjectItem.Item.ItemId == CsUIData.Instance.HpPotionId)
                                    {
                                        if (CsGameData.Instance.MyHeroTransform != null)
                                        {
                                            CsCommandEventManager.Instance.SendItemUse(CsGameData.Instance.MyHeroInfo.InventorySlotList[i].Index, 1);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 길드소집
            for (int i = 0; i < m_listCsGuildCall.Count; i++)
            {
                if (m_listCsGuildCall[i].RemainingTime <= Time.realtimeSinceStartup)
                {
                    GuildCallRemove(m_listCsGuildCall[i]);
                    break;
                }
            }

            if (m_guidNationWarCall != Guid.Empty)
            {
                if ((m_flNationWarCallRemainingTime - Time.realtimeSinceStartup) <= 0.0f)
                {
                    NationWarCallRemove();
                }
            }

            for (int i = 0; i < m_listCsNationCall.Count; i++)
            {
                if (m_listCsNationCall[i].RemainingTime <= Time.realtimeSinceStartup)
                {
                    NationCallRemove(m_listCsNationCall[i]);
                    break;
                }
            }

            // 버튼알립 표시
            UpdateMainButtons();

			// 사용레벨즉시사용
			if (m_bImmPickBoxUse)
			{
				List<CsHeroObject> list = new List<CsHeroObject>();

				for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
				{
					if (CsGameData.Instance.MyHeroInfo.InventorySlotList[i].EnType == EnInventoryObjectType.Item)
					{
						CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[i];

						if (csInventorySlot.InventoryObjectItem.Item.ItemType.ItemType == (int)EnItemType.PickBox)
						{
							list.Add(new CsHeroItem(csInventorySlot.InventoryObjectItem.Item, csInventorySlot.InventoryObjectItem.Owned, csInventorySlot.InventoryObjectItem.Count, csInventorySlot.Index));
						}
					}
				}

				OnEventHeroObjectGot(list);

				m_bImmPickBoxUse = false;
			}

            CsAccomplishmentManager.Instance.UpdateAccomplishmentComplete();

			if (++m_nTick % CsConfiguration.Instance.SystemSetting.StatusLoggingInterval == 0)
			{ 
				// ping, fps 전송
				if (CsRplzSession.Instance.PhotonPeer != null)
				{
					SendStatusLogging(CsRplzSession.Instance.PhotonPeer.RoundTripTime, (1.0f / m_flDeltaTime));
				}
			}

			// 네트웍상태
			SendNetworkStatus();

			// 배터리상태
			SendBatteryStatus();

			m_flTime = Time.time;
        }
        
        //절전모드 검사
        if (m_flSleepModeTimer - Time.realtimeSinceStartup <= 0)
        {
            if (!m_bSleepMode)
            {
                m_bSleepMode = true;
                SleepMode();
                CsGameEventToIngame.Instance.OnEventSleepMode(m_bSleepMode);
            }
        }

		// 안전모드 요청
		if (m_bAutoHuntSafeMode == false)
		{
			if (m_enBattleMode != EnBattleMode.None)  // 자동(반자동,자동) 사냥중일때.
			{
				if (m_flSafeModeTimer - Time.realtimeSinceStartup <= 0)
				{
					m_bAutoHuntSafeMode = true;
					CsCommandEventManager.Instance.SendAutoHuntStart();
				}
			}
		}


		if (Input.GetMouseButtonDown(0))
		{
			m_flSleepModeTimer = Time.realtimeSinceStartup + CsGameConfig.Instance.OptimizationModeWaitingTime;
			m_flSafeModeTimer = Time.realtimeSinceStartup + 60f;

			if (m_bAutoHuntSafeMode)
			{
				m_bAutoHuntSafeMode = false;
                CsCommandEventManager.Instance.SendAutoHuntEnd();
			}

			if (m_bSleepMode)
			{
				m_bSleepMode = false;
				SleepMode();
				CsGameEventToIngame.Instance.OnEventSleepMode(m_bSleepMode);
			}
		}

        foreach (EnScheduleNotice item in Enum.GetValues(typeof(EnScheduleNotice)))
        {
            CheckScheduleNotice(item);
        }
	}

	float m_flSafeModeTimer = 0;
	bool m_bAutoHuntSafeMode = false;

	//---------------------------------------------------------------------------------------------------
	//절전 모드
	void SleepMode()
    {
        if (m_bSleepMode)
        {
            Debug.Log("절전 모드 시작");
            Application.targetFrameRate = 15;
        }
        else
        {
            Debug.Log("절전 모드 종료");
            switch (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Frame))
            {
                case 0:
                    Application.targetFrameRate = 20;
                    break;
                case 1:
                    Application.targetFrameRate = 30;
                    break;
                case 2:
					//Application.targetFrameRate = 45;
					Application.targetFrameRate = 60; // 임시.
                    break;
                default:
                    Application.targetFrameRate = 30;
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
		Debug.Log("OnFinalizeOnFinalizeOnFinalizeOnFinalizeOnFinalize   Start");

        CsGameEventUIToUI.Instance.OnEventDeleteAllHUD();

        CsRplzSession.Instance.EventConnected -= OnEventConnected;
        CsRplzSession.Instance.EventDisconnected -= OnEventDisconnected;

        CsGameEventUIToUI.Instance.EventHeroLogin -= OnEventHeroLogin;
        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
        CsGameEventUIToUI.Instance.EventAlert -= OnEventAlert;
        CsGameEventUIToUI.Instance.EventConfirm -= OnEventConfirm;
        CsGameEventUIToUI.Instance.EventCommonModalClose -= OnEventCommonModalClose;
        CsGameEventUIToUI.Instance.EventOpenPopupItemInfo -= OnEventOpenPopupItemInfo;
        CsGameEventUIToUI.Instance.EventSinglePopupOpen -= OnEventSinglePopupOpen;
        CsGameEventUIToUI.Instance.EventOpenPopupFieldBoss -= OnEventOpenPopupFieldBoss;
		CsGameEventUIToUI.Instance.EventOpenPopupHelp -= OnEventOpenPopupHelp;
		CsGameEventUIToUI.Instance.EventOpenPopupCharging -= OnEventOpenPopupCharging;
		CsGameEventUIToUI.Instance.EventOpenPopupSmallAmountCharging -= OnEventOpenPopupSmallAmountCharging;
        CsGameEventUIToUI.Instance.EventOpenPopupOrdealQuest -= OnEventOpenPopupOrdealQuest;
        CsGameEventUIToUI.Instance.EventPopupOpen -= OnEventPopupOpen;
        CsGameEventUIToUI.Instance.EventPopupClose -= OnEventPopupClose;
        CsGameEventUIToUI.Instance.EventTimerConfirm -= OnEventTimerConfirm;
        CsGameEventUIToUI.Instance.EventTimerModalClose -= OnEventTimerModalClose;
        CsGameEventUIToUI.Instance.EventPlayerPrefsKeySet -= OnEventPlayerPrefsKeySet;

        CsGameEventUIToUI.Instance.EventReturnScrollUseFinished -= OnEventReturnScrollUseFinished;  // 귀환주문서 사용완료
        CsGameEventUIToUI.Instance.EventContinentSaftyRevive -= OnEventContinentSaftyRevive;        // 안전부활
        CsGameEventUIToUI.Instance.EventImmediateRevive -= OnEventImmediateRevive;                  // 즉시부활
        CsGameEventUIToUI.Instance.EventOpenUserReference -= OnEventOpenUserReference;
        CsGameEventUIToUI.Instance.EventOpenFriendRefernce -= OnEventOpenUserFriendRefernce;
        CsGameEventUIToUI.Instance.EventOpenGuildMemberReference -= OnEventOpenGuildMemberReference;
        CsGameEventUIToUI.Instance.EventOpenNationNoblesseReference -= OnEventOpenNationNoblesseReference;


        CsGameEventUIToUI.Instance.EventHeroObjectGot -= OnEventHeroObjectGot;          // 아이템,장비 획득

        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;    // 인게임 로딩완료
        CsGameEventToUI.Instance.EventHeroDead -= OnEventHeroDead;                      // 영웅사망
        CsGameEventToUI.Instance.EventPortalEnter -= OnEventPortalEnter;                // 포탈입장
        CsGameEventUIToUI.Instance.EventPortalEnter -= OnEventPortalEnter;
        CsCartManager.Instance.EventCartPortalEnter -= OnEventCartPortalEnter;          // 카트포탈입장
        CsGameEventUIToUI.Instance.EventHeroInfo -= OnEventHeroInfo;                    // 다른 유저 정보
        CsGuildManager.Instance.EventGuildMemberTabInfo -= OnEventGuildMemberTabInfo;   // 길드 정보 갱신

        //파티
        CsGameEventUIToUI.Instance.EventPartyInvitationArrived -= OnEventPartyInvitationArrived;                    //파티초대 알림도착
        CsGameEventUIToUI.Instance.EventPartyApplicationArrived -= OnEventPartyApplicationArrived;                  //파티가입 알림도착
        CsGameEventUIToUI.Instance.EventPartyApplicationLifetimeEnded -= OnEventPartyApplicationLifetimeEnded;      //파티가입알림시간초과
        CsGameEventUIToUI.Instance.EventPartyInvitationLifetimeEnded -= OnEventPartyInvitationLifetimeEnded;        //파티초대알림시간초과
        CsGameEventUIToUI.Instance.EventPartyMemberEnter -= OnEventPartyMemberEnter;                                //파티멤버추가
        CsGameEventUIToUI.Instance.EventPartyMemberExit -= OnEventPartyMemberExit;                                  //파티멤버삭제
        CsGameEventUIToUI.Instance.EventPartyBanished -= OnEventPartyBanished;                                      //파티강퇴당함(강퇴당한자)
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept -= OnEventPartyInvitationAccept;                      //파티초대수락(초대받은자)
        CsGameEventUIToUI.Instance.EventPartyInvitationRefuse -= OnEventPartyInvitationRefuse;                      //파티초대거절(초대받은자)
        CsGameEventUIToUI.Instance.EventPartyInvitationRefused -= OnEventPartyInvitationRefused;                    //파티초대거절당함(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationAccept -= OnEventPartyApplicationAccept;                    //파티가입수락(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationRefuse -= OnEventPartyApplicationRefuse;                    //파티가입거절(파티장)
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted -= OnEventPartyApplicationAccepted;                //파티가입수락당함(가입신청한자)
        CsGameEventUIToUI.Instance.EventPartyApplicationRefused -= OnEventPartyApplicationRefused;                  //파티가입거절당함(가입신청한자)
        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;                                          //파티생성
        CsGameEventUIToUI.Instance.EventPartyDisbanded -= OnEventPartyDisbanded;                                    //파티해산
        CsGameEventUIToUI.Instance.EventPartyMasterChanged -= OnEventPartyMasterChanged;                            //파티장변경

        //길드
        CsGuildManager.Instance.EventGuildInvitationArrived -= OnEventGuildInvitationArrived;                   //길드초대도착
        CsGuildManager.Instance.EventGuildInvitationAccept -= OnEventGuildInvitationAccept;                     //길드초대수락
        CsGuildManager.Instance.EventGuildInvitationRefuse -= OnEventGuildInvitationRefuse;                     //초대거절
        CsGuildManager.Instance.EventGuildInvitationRefused -= OnEventGuildInvitationRefused;                   //초대거절받음
        CsGuildManager.Instance.EventGuildInvitationLifetimeEnded -= OnEventGuildInvitationLifetimeEnded;       //초대시간초과
        CsGuildManager.Instance.EventGuildApplicationAccepted -= OnEventGuildApplicationAccepted;               //길드가입신청수락
        CsGuildManager.Instance.EventGuildApplicationRefused -= OnEventGuildApplicationRefused;
        CsGuildManager.Instance.EventGuildAppointed -= OnEventGuildAppointed;
        CsGuildManager.Instance.EventGuildMasterTransferred -= OnEventGuildMasterTransferred;
        CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildExit -= OnEventGuildExit;
        CsGuildManager.Instance.EventGuildTerritoryExit -= OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter -= OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildCallTransmission -= OnEventGuildCallTransmission;
        CsGuildManager.Instance.EventGuildCalled -= OnEventGuildCalled;
        CsGuildManager.Instance.EventGuildApply -= OnEventGuildApply;
        CsGuildManager.Instance.EventGuildTerritoryRevive -= OnEventGuildTerritoryRevive;

        CsGameEventUIToUI.Instance.EventDropObjectLooted -= OnEventDropObjectLooted;

        CsGameEventUIToUI.Instance.EventContinentTransmission -= OnEventContinentTransmission;
        CsGameEventUIToUI.Instance.EventNationTransmission -= OnEventNationTransmission;

        //CsGameEventUIToUI.Instance.EventContinentExitForStoryDungeonEnter -= OnEventContinentExitForStoryDungeonEnter;

        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter -= OnEventContinentExitForMainQuestDungeonEnter;

        //m_dtOld = CsGameData.Instance.MyHeroInfo.CurrentDateTime;

        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon -= OnEventMainQuestDungeonAbandon;

        //메인UI 숨기기
        CsGameEventToUI.Instance.EventHideMainUI -= OnEventHideMainUI;
        CsGameEventUIToUI.Instance.EventVisibleMainUI -= OnEventVisibleMainUI;

        //스토리던전
        CsDungeonManager.Instance.EventContinentExitForStoryDungeonEnter -= OnEventContinentExitForStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonAbandon -= OnEventStoryDungeonAbandon;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;

        //경험치 던전
        CsDungeonManager.Instance.EventContinentExitForExpDungeonEnter -= OnEventContinentExitForExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonExit -= OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonAbandon -= OnEventExpDungeonAbandon;
        CsDungeonManager.Instance.EventExpDungeonBanished -= OnEventExpDungeonBanished;

        //골드 던전
        CsDungeonManager.Instance.EventContinentExitForGoldDungeonEnter -= OnEventContinentExitForGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonExit -= OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonAbandon -= OnEventGoldDungeonAbandon;
        CsDungeonManager.Instance.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;

        //지하 미로
        CsDungeonManager.Instance.EventContinentExitForUndergroundMazeEnter -= OnEventContinentExitForUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazeExit -= OnEventUndergroundMazeExit;
        CsDungeonManager.Instance.EventUndergroundMazeBanished -= OnEventUndergroundMazeBanished;
        CsDungeonManager.Instance.EventUndergroundMazeRevive -= OnEventUndergroundMazeRevive;
        CsDungeonManager.Instance.EventUndergroundMazePortalEnter -= OnEventUndergroundMazePortalEnter;
        CsDungeonManager.Instance.EventUndergroundMazeTransmission -= OnEventUndergroundMazeTransmission;

        //고대 유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter -= OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge -= OnEventArtifactRoomNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge -= OnEventArtifactRoomBanishedForNextFloorChallenge;
        CsDungeonManager.Instance.EventArtifactRoomAbandon -= OnEventArtifactRoomAbandon;
        CsDungeonManager.Instance.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit -= OnEventArtifactRoomExit;

        //고대인의 유적
        CsDungeonManager.Instance.EventContinentExitForAncientRelicEnter -= OnEventContinentExitForAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicAbandon -= OnEventAncientRelicAbandon;
        CsDungeonManager.Instance.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicExit -= OnEventAncientRelicExit;

        //검투대회
        CsDungeonManager.Instance.EventContinentExitForFieldOfHonorChallenge -= OnEventContinentExitForFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorAbandon -= OnEventFieldOfHonorAbandon;
        CsDungeonManager.Instance.EventFieldOfHonorBanished -= OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorExit -= OnEventFieldOfHonorExit;

        // 영혼을 탐하는 자
        CsDungeonManager.Instance.EventContinentExitForSoulCoveterEnter -= OnEventContinentExitForSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterAbandon -= OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventSoulCoveterExit -= OnEventSoulCoveterExit;

        //용맹의 증명
        CsDungeonManager.Instance.EventContinentExitForProofOfValorEnter -= OnEventContinentExitForProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorAbandon -= OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBanished -= OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorExit -= OnEventProofOfValorExit;
		CsDungeonManager.Instance.EventProofOfValorGetCreatureCard -= OnEventProofOfValorGetCreatureCard;

        // 지혜의 신전
        CsDungeonManager.Instance.EventContinentExitForWisdomTempleEnter -= OnEventContinentExitForWisdomTempleEnter;
        CsDungeonManager.Instance.EventWisdomTempleExit -= OnEventWisdomTempleExit;
        CsDungeonManager.Instance.EventWisdomTempleAbandon -= OnEventWisdomTempleAbandon;
        CsDungeonManager.Instance.EventWisdomTempleBanished -= OnEventWisdomTempleBanished;
        CsDungeonManager.Instance.EventWisdomTempleStepCompleted -= OnEventWisdomTempleStepCompleted;
        CsDungeonManager.Instance.EventWisdomTemplePuzzleRewardObjectInteractionFinished -= OnEventWisdomTemplePuzzleRewardObjectInteractionFinished;
        CsDungeonManager.Instance.EventWisdomTempleBossMonsterKill -= OnEventWisdomTempleBossMonsterKill;

        // 정예 던전
        CsDungeonManager.Instance.EventContinentExitForEliteDungeonEnter -= OnEventContinentExitForEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonAbandon -= OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished -= OnEventEliteDungeonBanished;
        CsDungeonManager.Instance.EventEliteDungeonExit -= OnEventEliteDungeonExit;

        // 유적 탈환
        CsDungeonManager.Instance.EventContinentExitForRuinsReclaimEnter -= OnEventContinentExitForRuinsReclaimEnter;
        CsDungeonManager.Instance.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
        CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
        CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;
        CsDungeonManager.Instance.EventRuinsReclaimClear -= OnEventRuinsReclaimClear;

        // 무한 대전
        CsDungeonManager.Instance.EventContinentExitForInfiniteWarEnter -= OnEventContinentExitForInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart -= OnEventInfiniteWarStart;
        CsDungeonManager.Instance.EventInfiniteWarAbandon -= OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarExit -= OnEventInfiniteWarExit;

        // 공포의 제단
        CsDungeonManager.Instance.EventContinentExitForFearAltarEnter -= OnEventContinentExitForFearAltarEnter;
        CsDungeonManager.Instance.EventFearAltarAbandon -= OnEventFearAltarAbandon;
        CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;
        CsDungeonManager.Instance.EventFearAltarExit -= OnEventFearAltarExit;
        CsGameEventUIToUI.Instance.EventOpenPopupHalidomCollection -= OnEventOpenPopupHalidomCollection;	// 성물 수집 팝업

        // 전쟁의 기억
        CsDungeonManager.Instance.EventContinentExitForWarMemoryEnter -= OnEventContinentExitForWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryExit -= OnEventWarMemoryExit;

        // 오시리스 방
        CsDungeonManager.Instance.EventContinentExitForOsirisRoomEnter -= OnEventContinentExitForOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomAbandon -= OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomExit -= OnEventOsirisRoomExit;

        // 전기퀘스트 던전
        CsDungeonManager.Instance.EventContinentExitForBiographyQuestDungeonEnter -= OnEventContinentExitForBiographyQuestDungeonEnter;
        CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon -= OnEventBiographyQuestDungeonAbandon;
        CsDungeonManager.Instance.EventBiographyQuestDungeonBanished -= OnEventBiographyQuestDungeonBanished;
        CsDungeonManager.Instance.EventBiographyQuestDungeonExit -= OnEventBiographyQuestDungeonExit;

        // 용의 둥지
        CsDungeonManager.Instance.EventContinentExitForDragonNestEnter -= OnEventContinentExitForDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestAbandon -= OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished -= OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestExit -= OnEventDragonNestExit;

        // 무역선 탈환
        CsDungeonManager.Instance.EventContinentExitForTradeShipEnter -= OnEventContinentExitForTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipAbandon -= OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipExit -= OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipObjectDestructionReward -= OnEventTradeShipObjectDestructionReward;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventContinentExitForAnkouTombEnter -= OnEventContinentExitForAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombExit -= OnEventAnkouTombExit;

		// 팀 전장
		CsDungeonManager.Instance.EventContinentExitForTeamBattlefieldEnter -= OnEventContinentExitForTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit -= OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon -= OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished -= OnEventTeamBattlefieldBanished;

        CsErrorMessageManager.Instance.EventAlert -= OnEventAlert;

        // 대륙
        CsGameEventUIToUI.Instance.EventContinentBanished -= OnEventContinentBanished;

        // 국가 관직 임명
        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed -= OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationCallTransmission -= OnEventNationCallTransmission;
        CsGameEventUIToUI.Instance.EventNationCalled -= OnEventNationCalled;

        // 국가전
        CsNationWarManager.Instance.EventNationWarStart -= OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarJoin -= OnEventNationWarJoin;
        CsNationWarManager.Instance.EventNationWarTransmission -= OnEventNationWarTransmission;
        CsNationWarManager.Instance.EventNationWarCall -= OnEventNationWarCall;
        CsNationWarManager.Instance.EventNationWarCallTransmission -= OnEventNationWarCallTransmission;
        CsNationWarManager.Instance.EventNationWarRevive -= OnEventNationWarRevive;
        CsNationWarManager.Instance.EventNationWarNpcTransmission -= OnEventNationWarNpcTransmission;

        CsGameEventToUI.Instance.EventContinentSaftySceneLoad -= OnEventContinentSaftySceneLoad;

        // 크리처카드획득
        CsGameEventUIToUI.Instance.EventGetHeroCreatureCard -= OnEventGetHeroCreatureCard;

        // 레벨업
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

        // 몬스터 테이밍
        CsGameEventToUI.Instance.EventCreateTameMonster -= OnEventCreateTameMonster;
        CsGameEventToUI.Instance.EventDeleteTameMonster -= OnEventDeleteTameMonster;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealStart -= OnEventGroggyMonsterItemStealStart;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealCancel -= OnEventGroggyMonsterItemStealCancel;
        CsGameEventToUI.Instance.EventGroggyMonsterItemStealCancel -= OnEventGroggyMonsterItemStealCancelByIngame;
        CsGameEventUIToUI.Instance.EventGroggyMonsterItemStealFinished -= OnEventGroggyMonsterItemStealFinished;

        CsGameEventUIToUI.Instance.EventStartOrdealQuestMission -= OnEventStartOrdealQuestMission;

        // 던전입장실패
        CsDungeonManager.Instance.EventDungeonEnterFail -= OnEventDungeonEnterFail;

        // 계정중복로그인
        CsGameEventUIToUI.Instance.EventAccountLoginDuplicated -= OnEventAccountLoginDuplicated;

        // 친구
        CsFriendManager.Instance.EventFriendApplicationReceived -= OnEventFriendApplicationReceived;
        CsFriendManager.Instance.EventFriendApplicationAccepted -= OnEventFriendApplicationAccepted;
        CsFriendManager.Instance.EventFriendApplicationRefused -= OnEventFriendApplicationRefused;
        CsFriendManager.Instance.EventFriendApplicationAccept -= OnEventFriendApplicationAccept;
        CsFriendManager.Instance.EventFriendApplicationRefuse -= OnEventFriendApplicationRefuse;

        CsFriendManager.Instance.EventBlacklistEntryAdd -= OnEventBlacklistEntryAdd;
        CsFriendManager.Instance.EventBlacklistEntryDelete -= OnEventBlacklistEntryDelete;

        // 절전
        CsGameEventUIToUI.Instance.EventSleepModeReset -= OnEventSleepModeReset;

        // 안전 모드
        CsGameEventToIngame.Instance.EventAutoBattleStart -= OnEventAutoBattleStart;

        // 크리쳐카드 오픈 애니메이션 체크
		CsOpenToastManager.Instance.EventCheckCreatureCardAnimation -= OnEventCheckCreatureCardAnimation;

        // 증정
        CsPresentManager.Instance.EventHeroPresent -= OnEventHeroPresent;
        CsPresentManager.Instance.EventPresentReplyReceived -= OnEventPresentReplyReceived;
        CsPresentManager.Instance.EventPresentSend -= OnEventPresentSend;

		// 크리처
		CsGameEventUIToUI.Instance.EventGetHeroCreature -= OnEventGetHeroCreature;

        // 길드 축복
        CsGameEventUIToUI.Instance.EventOpenPopupGuildBlessing -= OnEventOpenPopupGuildBlessing;

        // 국가 동맹
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept -= OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded -= OnEventNationAllianceConcluded;

        CsGameEventUIToUI.Instance.EventOpenPopupMountAttrPotion -= OnEventOpenPopupMountAttrPotion;
        CsGameEventUIToUI.Instance.EventOpenPopupCostumeEffect -= OnEventOpenPopupCostumeEffect;
        CsGameEventUIToUI.Instance.EventOpenPopupHeroPotionAttr -= OnEventOpenPopupHeroPotionAttr;

		// 웹뷰
		CsGameEventUIToUI.Instance.EventOpenWebView -= OnEventOpenWebView;

		Debug.Log("2. OnFinalizeOnFinalizeOnFinalizeOnFinalizeOnFinalize   End");
	}

	#region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroLevelUp()
	{
		m_bImmPickBoxUse = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGetHeroCreatureCard(List<CsHeroCreatureCard> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			m_listCsHeroCreatureCardList.Add(list[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentSaftySceneLoad()
    {
        int nContinentId = CsGameConfig.Instance.SaftyRevivalContinentId;
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        InitializeNewDate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHideMainUI(bool bIsOn)
    {
        ToggleMainUI(!bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventVisibleMainUI(bool bIsOn)
    {
        ToggleMainUI(bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenUserReference(CsHeroBase csHeroBase)
    {
        OpenPopupUserReference(csHeroBase);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenUserFriendRefernce(CsFriend csFriend)
    {
        OpenPopupUserReference(csFriend);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenGuildMemberReference(CsGuildMember csGuildMember)
    {
        OpenPopupUserReference(csGuildMember);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenNationNoblesseReference(CsNationNoblesseInstance csNationNoblesseInstance)
    {
        OpenPopupUserReference(csNationNoblesseInstance);
    }

    // 정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForEliteDungeonEnter()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        LoadSceneByName(CsGameData.Instance.EliteDungeon.SceneName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonAbandon(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

	// 유적 탈환
	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForRuinsReclaimEnter()
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		LoadSceneByName(CsGameData.Instance.RuinsReclaim.SceneName);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nContinentId)
	{
		LoadContinentScene(nContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nContinentId)
	{
		LoadContinentScene(nContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nContinentId)
	{
		LoadContinentScene(nContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimClear(PDItemBooty[] aPDItemBooty, PDItemBooty randomBooty,
		Guid monsterTerminatorHeroId, string monsterTerminatorHeroName, PDItemBooty monsterTerminatorBooty,
		Guid ultimateAttackKingHeroId, string ultimateAttackKingHeroName, PDItemBooty ultimateAttackKingBooty,
		Guid partyVolunteerHeroId, string partyVolunteerHeroName, PDItemBooty partyVolunteerBooty)
	{
		List<CsDropObject> listLooted = new List<CsDropObject>();
		List<CsDropObject> listNotLooted = new List<CsDropObject>();

		if (monsterTerminatorHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
			listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, monsterTerminatorBooty.id, monsterTerminatorBooty.owned, monsterTerminatorBooty.count));
		}

		if (ultimateAttackKingHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
			listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, ultimateAttackKingBooty.id, ultimateAttackKingBooty.owned, ultimateAttackKingBooty.count));
		}

		if (partyVolunteerHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
		{
			listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, partyVolunteerBooty.id, partyVolunteerBooty.owned, partyVolunteerBooty.count));
		}
		
		CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
	}

    // 무한 대전
    #region InfiniteWar
    
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForInfiniteWarEnter()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        LoadSceneByName(CsGameData.Instance.InfiniteWar.SceneName);
        m_canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarEnter(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] aPDHero, PDMonsterInstance[] aPDMonsterInstance, PDInfiniteWarBuffBoxInstance[] aPDInfiniteWarBuffBoxInstance)
    {
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime <= 0.0f)
        {
            m_canvas.GetComponent<GraphicRaycaster>().enabled = true;
        }
        else
        {
            m_canvas.GetComponent<GraphicRaycaster>().enabled = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarStart()
    {
        m_canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarClear(PDInfiniteWarRanking[] pDInfiniteWarRanking, PDItemBooty[] pDItemBooties, PDItemBooty[] pDRankingItemBooties)
    {

    }

    #endregion InfiniteWar

	// 공포의 제단
	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForFearAltarEnter()
	{
		LoadSceneByName(CsDungeonManager.Instance.FearAltarStage.SceneName);
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenPopupHalidomCollection(bool bRewardReceivable)
	{
		GameObject goPopupHalidomCollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/PopupHalidomCollection");
		
		Transform trPopupHalidomCollection = Instantiate(goPopupHalidomCollection, m_trPopupList).transform;

		CsPopupHalidomCollection csPopupHalidomCollection = trPopupHalidomCollection.GetComponent<CsPopupHalidomCollection>();
		csPopupHalidomCollection.InitializeUI(bRewardReceivable);
	}

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForWarMemoryEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.WarMemory.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    #endregion WarMemory

    #region OsirisRoom

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForOsirisRoomEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.OsirisRoom.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    #endregion OsirisRoom

	#region BiographyQuestDungeon

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForBiographyQuestDungeonEnter()
	{
		if (CsDungeonManager.Instance.BiographyQuestDungeon != null)
		{
			LoadSceneByName(CsDungeonManager.Instance.BiographyQuestDungeon.SceneName);
			CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonAbandon(int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonBanished(int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonExit(int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	#endregion BiographyQuestDungeon

    #region DragonNest

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForDragonNestEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.DragonNest.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestAbandon(int nPrevContinentId)
    {
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestBanished(int nPrevContinentId)
    {
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestExit(int nPrevContinentId)
    {
        LoadContinentScene(nPrevContinentId);
    }

    #endregion DragonNest

    #region TradeShip

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForTradeShipEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.TradeShip.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipAbandon(int nPrevContinentId)
    {
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipBanished(int nPrevContinentId)
    {
        m_csPanelModal.CloseCommonModal();
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipExit(int nPrevContinentId)
    {
        m_csPanelModal.CloseCommonModal();
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipObjectDestructionReward(PDItemBooty pdItemBooty)
    {
        List<CsDropObject> listLooted = new List<CsDropObject>();
        List<CsDropObject> listNotLooted = new List<CsDropObject>();

        listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, pdItemBooty.id, pdItemBooty.owned, pdItemBooty.count));

        CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
    }

    #endregion TradeShip

    #region AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForAnkouTombEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.AnkouTomb.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombAbandon(int nPrevContinentId)
    {
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombBanished(int nPrevContinentId)
    {
        m_csPanelModal.CloseCommonModal();
        LoadContinentScene(nPrevContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombExit(int nPrevContinentId)
    {
        m_csPanelModal.CloseCommonModal();
        LoadContinentScene(nPrevContinentId);
    }

    #endregion AnkouTomb

	#region 팀 전장
	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForTeamBattlefieldEnter()
	{
		LoadSceneByName(CsGameData.Instance.TeamBattlefield.SceneName);
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldExit(int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldAbandon(bool bLevelUp, long lAcquiredExp, int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldBanished(int nPrevContinentId)
	{
		LoadContinentScene(nPrevContinentId);
	}

	#endregion 팀 전장

    // 영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForSoulCoveterEnter()
    {
        LoadSceneByName(CsGameData.Instance.SoulCoveter.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterAbandon(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //용맹의 증명

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForProofOfValorEnter()
    {
        LoadSceneByName(CsGameData.Instance.ProofOfValor.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorAbandon(int nContinentId, bool bLevelUp, long lAcquiredExp)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorGetCreatureCard(CsHeroCreatureCard csHeroCreatureCard)
	{
		OpenPopupHeroCreatureCard(csHeroCreatureCard, true);
	}

	// 지혜의 신전
	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForWisdomTempleEnter()
	{
		LoadSceneByName(CsGameData.Instance.WisdomTemple.SceneName);
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleExit(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleAbandon(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBanished(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepCompleted(bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		List<CsDropObject> listLooted = new List<CsDropObject>();
		List<CsDropObject> listNotLooted = new List<CsDropObject>();

		listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, pdItemBooty.id, pdItemBooty.owned, pdItemBooty.count));

		CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBossMonsterKill(PDItemBooty pdItemBooty)
	{
		List<CsDropObject> listLooted = new List<CsDropObject>();
		List<CsDropObject> listNotLooted = new List<CsDropObject>();

		listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, pdItemBooty.id, pdItemBooty.owned, pdItemBooty.count));

		CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleRewardObjectInteractionFinished(PDItemBooty pdItemBooty, long lInstanceId)
	{
		List<CsDropObject> listLooted = new List<CsDropObject>();
		List<CsDropObject> listNotLooted = new List<CsDropObject>();

		listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, pdItemBooty.id, pdItemBooty.owned, pdItemBooty.count));

		CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
	}
	
    //검투대회
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForFieldOfHonorChallenge()
    {
        LoadSceneByName(CsDungeonManager.Instance.FieldOfHonor.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorAbandon(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //고대인의 유적
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForAncientRelicEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.AncientRelic.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicAbandon(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //고대 유물의 방
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForArtifactRoomEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.ArtifactRoom.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomNextFloorChallenge()
    {
        LoadSceneByName(CsDungeonManager.Instance.ArtifactRoom.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanishedForNextFloorChallenge()
    {
        LoadSceneByName(CsDungeonManager.Instance.ArtifactRoom.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomAbandon(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }


    //지하미로
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForUndergroundMazeEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.UndergroundMaze.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeRevive()
    {
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        LoadSceneByName(CsDungeonManager.Instance.UndergroundMaze.SceneName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazePortalEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.UndergroundMaze.SceneName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeTransmission()
    {
        LoadSceneByName(CsDungeonManager.Instance.UndergroundMaze.SceneName);
    }

    //골드던전

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGoldDungeonEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.GoldDungeon.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //경험치던전

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForExpDungeonEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.ExpDungeon.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //스토리던전

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonAbandon(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nPreviousContinentId)
    {
        LoadContinentScene(nPreviousContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForStoryDungeonEnter()
    {
        LoadSceneByName(CsDungeonManager.Instance.StoryDungeon.SceneName);
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //메인퀘스트 던전

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonAbandon(int nContinentId, bool bChangeScene)
    {
        if (bChangeScene)
        {
            LoadContinentScene(nContinentId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonBanished(int nContinentId, bool bChangeScene)
    {
        if (bChangeScene)
        {
            LoadContinentScene(nContinentId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nContinentId, bool bChangeScene)
    {
        if (bChangeScene)
        {
            LoadContinentScene(nContinentId);
        }
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForMainQuestDungeonEnter(bool bSceneLoading)
    {
        if (bSceneLoading)
        {
            LoadMainQuestDungeonScene();
        }

        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAlert(string strErrorMessage)
    {
        m_csPanelModal.Choice(strErrorMessage, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventConfirm(string strMessage, string strButton1, UnityAction unityAction1, string strButton2, UnityAction unityAction2, bool bClose)
    {
        m_csPanelModal.Choice(strMessage, unityAction1, strButton1, unityAction2, strButton2, bClose);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCommonModalClose()
    {
        m_csPanelModal.CloseCommonModal();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenPopupItemInfo(CsItem csItem)
	{
		StartCoroutine(LoadPopupItemInfo(csItem));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSinglePopupOpen(EnMenuId enMenuId)
	{
		OpenSinglePopup(enMenuId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenPopupFieldBoss()
	{
		OpenPopupFieldBoss();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenPopupHelp(EnMainMenu enMainMenu)
	{
		OpenPopupHelp(enMainMenu);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOpenPopupCharging(bool bGetProductsFinished)
	{
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			OpenPopupCharging();
			return;
		}
		
		if (CsCashManager.Instance.StoreKitInitialized)
		{
			if (bGetProductsFinished)
			{
				OpenPopupCharging();
			}
			else
			{
				CsCashManager.Instance.StoreKitGetProducts();
			}
		}
		else
		{
			CsCashManager.Instance.InitializeStoreKit();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOpenPopupSmallAmountCharging()
	{
		OpenPopupSmallAmountCharging();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventOpenPopupOrdealQuest()
	{
		OpenPopupOrdealQuest();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventPopupOpen(EnMainMenu enMainMenu, EnSubMenu enSubMenu, CsHeroInfo csHeroInfo)
    {
        OpenPopup(enMainMenu, enSubMenu, csHeroInfo);
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventPopupClose()
    {
        ToggleMainCamera(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTimerConfirm(EnTimerModalType enTimerModalType, string strMessage, UnityAction unityAction1, UnityAction unityAction2, float flTime)
    {
        m_csPanelTimerModal.Choice(enTimerModalType, strMessage, unityAction1, unityAction2, flTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTimerModalClose(EnTimerModalType enTimerModalType)
    {
        m_csPanelTimerModal.CloseTimerModal(enTimerModalType);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPlayerPrefsKeySet(EnPlayerPrefsKey enPlayerPrefsKey, int nValue)
    {
        CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A18_TXT_02001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventReturnScrollUseFinished(int nContitnentId)
    {
        CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.ReturnScroll;
        LoadContinentScene(nContitnentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentSaftyRevive(int nContitnentId)
    {
        LoadContinentScene(nContitnentId);
    }

    void OnEventImmediateRevive()
    {
    }

    //---------------------------------------------------------------------------------------------------
	// 로딩완료후.
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
		m_bImmPickBoxUse = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroDead(string strName)
    {
        Debug.Log("## OnEventHeroDead ##");
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(false);

        if (CsUIData.Instance.DungeonInNow == EnDungeon.ArtifactRoom ||
            CsUIData.Instance.DungeonInNow == EnDungeon.FieldOfHonor || 
            CsUIData.Instance.DungeonInNow == EnDungeon.ProofOfValor ||
			CsUIData.Instance.DungeonInNow == EnDungeon.WisdomTemple)
        {
            return;
        }
        else
        {
            if (m_iEnumeratorOpenPopupRevive != null)
            {
                return;
            }
            else
            {
                m_iEnumeratorOpenPopupRevive = OpenPopupReviveCoroutine(strName);
                StartCoroutine(m_iEnumeratorOpenPopupRevive);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPortalEnter(int nPortalId)
    {
        m_nPortalId = nPortalId;
        CsCommandEventManager.Instance.SendPortalEnter(nPortalId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPortalEnter()
    {
        CsPortal csPortal = CsGameData.Instance.GetPortal(m_nPortalId);
        CsPortal csPortalLinked = CsGameData.Instance.GetPortal(csPortal.LinkedPortalId);
        CsContinent csContinent = CsGameData.Instance.GetContinent(csPortalLinked.ContinentId);
        StartCoroutine(LoadSceneMainUICoroutine(csContinent.SceneName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCartPortalEnter(int nPortalId)
    {
        CsPortal csPortal = CsGameData.Instance.GetPortal(nPortalId);
        CsPortal csPortalLinked = CsGameData.Instance.GetPortal(csPortal.LinkedPortalId);
        CsContinent csContinent = CsGameData.Instance.GetContinent(csPortalLinked.ContinentId);
        StartCoroutine(LoadSceneMainUICoroutine(csContinent.SceneName));
    }

    //---------------------------------------------------------------------------------------------------     OnEventHeroObjectGot(Lis
    void OnEventHeroInfo(CsHeroInfo csHeroInfo)
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.ViewOtherUsers, EnSubMenu.OtherUsers, csHeroInfo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMemberTabInfo()
    {
        if (m_csPopupMain == null)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroObjectGot(List<CsHeroObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].HeroObjectType == EnHeroObjectType.MainGear)
            {
                CsHeroMainGear csHeroMainGear = (CsHeroMainGear)list[i];

                // 직업조건
                int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

                if (csHeroMainGear.MainGear.JobId == (int)EnJob.Common || csHeroMainGear.MainGear.JobId == nJobId)
                {
                    // 착용레벨조건
                    if (csHeroMainGear.MainGear.MainGearTier.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[csHeroMainGear.MainGear.MainGearType.EquippedIndex] == null)
                        {
                            m_listHeroObjectDisplay.Add(list[i]);
                        }
                        else
                        {
                            if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[csHeroMainGear.MainGear.MainGearType.EquippedIndex].BattlePower < csHeroMainGear.BattlePower)
                            {
                                m_listHeroObjectDisplay.Add(list[i]);
                            }
                        }
                    }
                }
            }
            else if (list[i].HeroObjectType == EnHeroObjectType.Item)
            {
                CsHeroItem csHeroItem = (CsHeroItem)list[i];

                if ((csHeroItem.Item.RequiredMaxHeroLevel >= CsGameData.Instance.MyHeroInfo.Level
                    && csHeroItem.Item.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    && csHeroItem.Item.UsingRecommendationEnabled)
                {
                    m_listHeroObjectDisplay.Add(list[i]);
                }
            }
            else
            {
                m_listHeroObjectDisplay.Add(list[i]);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationArrived(CsPartyInvitation csPartyInvitation)
    {
        CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyInvitation.InviterName),
                (() => OnClickPartyInvitationAccept(csPartyInvitation.No)), (() => OnClickPartyInvitationRefuse(csPartyInvitation.No)), csPartyInvitation.RemainingTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationArrived(CsPartyApplication csPartyApplication)
    {
        CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyApplication.ApplicantName),
            (() => OnClickPartyApplicationAccept(csPartyApplication.No)), (() => OnClickPartyApplicationRefuse(csPartyApplication.No)), csPartyApplication.RemainingTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationRefuse()
    {
        List<CsPartyInvitation> listPartyInvitation = CsGameData.Instance.MyHeroInfo.PartyInvitationList;

        //남은 초대가 있는 경우
        if (listPartyInvitation.Count != 0)
        {
            CsPartyInvitation csPartyInvitation = listPartyInvitation[listPartyInvitation.Count - 1];

            CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyInvitation.InviterName),
                (() => OnClickPartyInvitationAccept(csPartyInvitation.No)), (() => OnClickPartyInvitationRefuse(csPartyInvitation.No)), csPartyInvitation.RemainingTime);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationRefuse()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            List<CsPartyApplication> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyApplicationList;

            //남은 신청이 있는 경우
            if (listPartyApplication.Count != 0)
            {
                CsPartyApplication csPartyApplication = listPartyApplication[listPartyApplication.Count - 1];

                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyApplication.ApplicantName),
                (() => OnClickPartyApplicationAccept(csPartyApplication.No)), (() => OnClickPartyApplicationRefuse(csPartyApplication.No)), csPartyApplication.RemainingTime);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationLifetimeEnded()
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            List<CsPartyApplication> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyApplicationList;

            //남은 신청이 있는 경우
            if (listPartyApplication.Count != 0)
            {
                CsPartyApplication csPartyApplication = listPartyApplication[listPartyApplication.Count - 1];

                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyApplication.ApplicantName),
                (() => OnClickPartyApplicationAccept(csPartyApplication.No)), (() => OnClickPartyApplicationRefuse(csPartyApplication.No)), csPartyApplication.RemainingTime);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationLifetimeEnded()
    {
        List<CsPartyInvitation> listPartyInvitation = CsGameData.Instance.MyHeroInfo.PartyInvitationList;

        //남은 신청이 있는 경우
        if (listPartyInvitation.Count != 0)
        {
            CsPartyInvitation csPartyInvitation = listPartyInvitation[listPartyInvitation.Count - 1];

            CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyInvitation.InviterName),
                (() => OnClickPartyInvitationAccept(csPartyInvitation.No)), (() => OnClickPartyInvitationRefuse(csPartyInvitation.No)), csPartyInvitation.RemainingTime);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberEnter(CsPartyMember csPartyMember)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_04002"), csPartyMember.Name));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberExit(CsPartyMember csPartyMember, bool bBanished)
    {
        if (bBanished)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_04012"), csPartyMember.Name));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_04016"), csPartyMember.Name));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyBanished()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04013"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationAccept()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04003"));
        CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationRefused()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04004"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationRefused()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04004"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisbanded()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04018"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccept(CsPartyMember csPartyMember)
    {
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            List<CsPartyApplication> listPartyApplication = CsGameData.Instance.MyHeroInfo.Party.PartyApplicationList;

            //남은 신청이 있는 경우
            if (listPartyApplication.Count != 0)
            {
                CsPartyApplication csPartyApplication = listPartyApplication[listPartyApplication.Count - 1];

                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.PartyModal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_03001"), csPartyApplication.ApplicantName),
                (() => OnClickPartyApplicationAccept(csPartyApplication.No)), (() => OnClickPartyApplicationRefuse(csPartyApplication.No)), csPartyApplication.RemainingTime);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.PartyModal);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccepted()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04003"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04001"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMasterChanged()
    {
        if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04008"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A36_TXT_04009"), CsGameData.Instance.MyHeroInfo.Party.Master.Name));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationArrived(CsHeroGuildInvitation csHeroGuildInvitation)
    {
        List<CsHeroGuildInvitation> listGuildInvitation = CsGuildManager.Instance.HeroGuildInvitationList;

        if (listGuildInvitation.Count <= 1)
        {
            CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.GuildModal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02016"), csHeroGuildInvitation.InviterName, csHeroGuildInvitation.GuildName),
                (() => OnClickGuildInvitationAccept(csHeroGuildInvitation)), (() => OnClickGuildInvitationRefuse(csHeroGuildInvitation)), csHeroGuildInvitation.RemainingTime);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationAccept()
    {
        CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationRefuse()
    {
        List<CsHeroGuildInvitation> listGuildInvitation = CsGuildManager.Instance.HeroGuildInvitationList;

        if (listGuildInvitation.Count != 0)
        {
            CsHeroGuildInvitation csHeroGuildInvitation = listGuildInvitation[listGuildInvitation.Count - 1];

            CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.GuildModal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02016"), csHeroGuildInvitation.InviterName, csHeroGuildInvitation.GuildName),
                  (() => OnClickGuildInvitationAccept(csHeroGuildInvitation)), (() => OnClickGuildInvitationRefuse(csHeroGuildInvitation)), csHeroGuildInvitation.RemainingTime);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationRefused(Guid guidTargetId, string strTargetName)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02008"), strTargetName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationLifetimeEnded(Guid guidInvitationId, Guid guidTargetId, string strTargetName)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId == guidInvitationId)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02008"), strTargetName));
        }
        else
        {
            List<CsHeroGuildInvitation> listGuildInvitation = CsGuildManager.Instance.HeroGuildInvitationList;

            if (listGuildInvitation.Count != 0)
            {
                CsHeroGuildInvitation csHeroGuildInvitation = listGuildInvitation[listGuildInvitation.Count - 1];

                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.GuildModal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_02016"), csHeroGuildInvitation.InviterName, csHeroGuildInvitation.GuildName),
                      (() => OnClickGuildInvitationAccept(csHeroGuildInvitation)), (() => OnClickGuildInvitationRefuse(csHeroGuildInvitation)), csHeroGuildInvitation.RemainingTime);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationAccepted()
    {
        CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A58_TXT_01008"), CsGuildManager.Instance.GuildName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationRefused(string strGuildName)
    {
        CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A58_TXT_01009"), strGuildName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAppointed(Guid guidAppointerId, string strAppointerName, int nAppointerGrade, Guid guidAppointeeId, string strAppointeeName, int nApoointeeGrade)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId == guidAppointeeId)
        {
            if (nApoointeeGrade == 4)
            {
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A58_TXT_01007"), strAppointerName, CsGameData.Instance.GetGuildMemberGrade(nAppointerGrade).Name));
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A58_TXT_01006"), strAppointerName, CsGameData.Instance.GetGuildMemberGrade(nAppointerGrade).Name, CsGameData.Instance.GetGuildMemberGrade(nApoointeeGrade).Name));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMasterTransferred(Guid guidTransfererId, string strTransFererName, Guid guidTransfereeId, string strTransfereeName)
    {
        if (CsGameData.Instance.MyHeroInfo.HeroId == guidTransfereeId)
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A58_TXT_01006"), strTransFererName, CsGameData.Instance.GetGuildMemberGrade(1).Name, CsGameData.Instance.GetGuildMemberGrade(1).Name));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBanished(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A58_TXT_01010"));

        if (m_listCsGuildCall.Count != 0)
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
        }

        if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            Debug.Log("### OnEventGuildBanished ###");
            LoadContinentScene(nContinentId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildExit(int nContinentId)
    {
        if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
            LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGuildTerritoryEnter(string sSceneName)
    {
        LoadSceneByName(sSceneName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryExit(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForStoryDungeonEnter(string strSceneName)
    {
        StartCoroutine(LoadSceneMainUICoroutine(strSceneName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildCallTransmission(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildCalled(CsGuildCall csGuildCall)
    {
        int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
        CsContinent csContinent = CsGameData.Instance.GetContinent(nLocationId);

        //길드 소집 조건
        //길드 소속 And Pvp 가능레벨
        if (CsGuildManager.Instance.Guild != null && CsGameData.Instance.MyHeroInfo.Level >= CsGameConfig.Instance.PvpMinHeroLevel)
        {
            //위치가 길드 영지 or 대륙 일때
            if (nLocationId == 201 || csContinent != null)
            {
                m_listCsGuildCall.Add(csGuildCall);

                if (m_listCsGuildCall.Count <= 1)
                {
                    CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.GuildModal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_01012"),
                        CsGameData.Instance.GetGuildMemberGrade(csGuildCall.CallerMemberGrade).Name, csGuildCall.CallerName),
                        (() => OnClickGuildCallAccept(csGuildCall)), (() => OnClickGuildCallRefuse(csGuildCall)), csGuildCall.RemainingTime);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApply()
    {
        if (m_trPopupGuildApply == null)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02004"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryRevive()
    {
        LoadSceneByName(CsGameData.Instance.GuildTerritory.SceneName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentTransmission(string strSceneName)
    {
        StartCoroutine(LoadSceneMainUICoroutine(strSceneName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationTransmission(string strSceneName)
    {
        StartCoroutine(LoadSceneMainUICoroutine(strSceneName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentBanished(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationNoblesseAppointed(int nNoblesseId, string strHeroName)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A61_TXT_00020"), strHeroName, CsGameData.Instance.GetNationNoblesse(nNoblesseId).Name));
    }

    #region NationWar

    //---------------------------------------------------------------------------------------------------
    void OnEventNationCallTransmission(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationCalled(CsNationCall csNationCall)
    {
        int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
        CsContinent csContinent = CsGameData.Instance.GetContinent(nLocationId);

        if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && (nLocationId == 201 || nLocationId == CsGameData.Instance.UndergroundMaze.LocationId || csContinent != null) && !CsIngameData.Instance.MyHeroDead)
        {
            m_listCsNationCall.Add(csNationCall);

            if (m_listCsNationCall.Count <= 1)
            {
                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.NationModal, string.Format(CsConfiguration.Instance.GetString("A70_TXT_03001"),
                        csNationCall.CallerNationNoblesse.Name, csNationCall.CallerName, "{0}"),
                        (() => OnClickNationCallAccept(csNationCall)), (() => OnClickNationCallRefuse(csNationCall)), csNationCall.RemainingTime);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationCallAccept(CsNationCall csNationCall)
    {
        int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;

        if (CsNationWarManager.Instance.MyNationWarDeclaration != null && CsNationWarManager.Instance.MyNationWarDeclaration.Status == EnNationWarDeclaration.Current)
        {
            m_listCsNationCall.Clear();
        }
        else if (nLocationId == 201 || nLocationId == CsGameData.Instance.UndergroundMaze.LocationId || CsGameData.Instance.GetContinent(nLocationId) != null)
        {
            if (!CsCartManager.Instance.IsMyHeroRidingCart)
            {
                CsCommandEventManager.Instance.SendNationCallTransmission(csNationCall.Id);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_ERROR_CART"));
            }

            NationCallRemove(csNationCall);
        }
        else
        {
            m_listCsNationCall.Clear();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationCallRefuse(CsNationCall csNationCall)
    {
        NationCallRemove(csNationCall);
    }

    //---------------------------------------------------------------------------------------------------
    void NationCallRemove(CsNationCall csNationCall)
    {
        m_listCsNationCall.Remove(csNationCall);

        if (m_listCsNationCall.Count != 0)
        {
            CsNationCall csNationCalled = m_listCsNationCall[0];

            int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
            CsContinent csContinent = CsGameData.Instance.GetContinent(nLocationId);

            if (CsNationWarManager.Instance.MyNationWarDeclaration != null && CsNationWarManager.Instance.MyNationWarDeclaration.Status == EnNationWarDeclaration.Current)
            {
                m_listCsNationCall.Clear();
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.NationModal);
            }
            else if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && (nLocationId == 201 || nLocationId == CsGameData.Instance.UndergroundMaze.LocationId || csContinent != null))
            {
                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.NationModal, string.Format(CsConfiguration.Instance.GetString("A70_TXT_03001"),
                        csNationCalled.CallerNationNoblesse.Name, csNationCalled.CallerName, "{0}"),
                        (() => OnClickNationCallAccept(csNationCalled)), (() => OnClickNationCallRefuse(csNationCalled)), csNationCalled.RemainingTime);
            }
            else
            {
                m_listCsNationCall.Clear();
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.NationModal);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.NationModal);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarStart(Guid guidNationWarDeclaration)
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.DeclarationId == guidNationWarDeclaration);

        if (csNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            int nMyHeroNationId = CsGameData.Instance.MyHeroInfo.Nation.NationId;

            if (csNationWarDeclaration.NationId == nMyHeroNationId || csNationWarDeclaration.TargetNationId == nMyHeroNationId)
            {
                int nMyHeroLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
                CsContinent csContinent = CsGameData.Instance.GetContinent(nMyHeroLocationId);

                // pvp 레벨 이상, 대륙에 있고, 카트를 타고 있지 않고, 아직 살아 있음
                if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
                    csContinent != null && !CsNationWarManager.Instance.NationWarJoined && !CsCartManager.Instance.IsMyHeroRidingCart && !CsIngameData.Instance.MyHeroDead)
                {
                    StartCoroutine(OpenPopupNationWarAttend());
                }
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarJoin(int nContinentId)
    {
        // Offense = 4
        // defense = 2
        Debug.Log("nContinentId : " + nContinentId);
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarTransmission(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarCall(Guid guidCallerId, string strCallerName, int nNoblesseId, DateTime dtCall)
    {
        int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
        CsContinent csContinent = CsGameData.Instance.GetContinent(nLocationId);

        // Pvp 가능레벨
        if (CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && csContinent != null)
        {
            m_guidNationWarCall = guidCallerId;

            if ((m_flNationWarCallRemainingTime - Time.realtimeSinceStartup) <= 0.0f)
            {
                m_flNationWarCallRemainingTime = CsGameConfig.Instance.NationCallLifetime + Time.realtimeSinceStartup;

                CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.NationModal, string.Format(CsConfiguration.Instance.GetString("A70_TXT_03001"), CsGameData.Instance.MyHeroInfo.GetNationNoblesseInstanceByHeroId(guidCallerId).NationNoblesse.Name, strCallerName, "{0}"),
                    () => OnClickNationWarCallAccept(),
                    () => OnClickNationWarCallRefuse(),
                    m_flNationWarCallRemainingTime);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarCallTransmission(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarRevive(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
        LoadContinentScene(nContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarNpcTransmission(int nContinentId)
    {
        LoadContinentScene(nContinentId);
    }

    #endregion NationWar

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonEnterFail(int nPreviousContinentId)
	{
		LoadContinentScene(nPreviousContinentId);
	}

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventDropObjectLooted(List<CsDropObject> listLooted, List<CsDropObject> listNotLooted)
    {
        for (int i = 0; i < listLooted.Count; i++)
        {
            m_listcsDropObject.Add(listLooted[i]);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //파티 초대 수락
    void OnClickPartyInvitationAccept(long lInvitationNo)
    {
        CsCommandEventManager.Instance.SendPartyInvitationAccept(lInvitationNo);
    }

    //---------------------------------------------------------------------------------------------------
    //파티 초대 거절
    void OnClickPartyInvitationRefuse(long lInvitationNo)
    {
        CsCommandEventManager.Instance.SendPartyInvitationRefuse(lInvitationNo);
    }

    //---------------------------------------------------------------------------------------------------
    //파티 가입 수락
    void OnClickPartyApplicationAccept(long lInvitationNo)
    {
        CsCommandEventManager.Instance.SendPartyApplicationAccept(lInvitationNo);
    }

    //---------------------------------------------------------------------------------------------------
    //파티 가입 거절
    void OnClickPartyApplicationRefuse(long lInvitationNo)
    {
        CsCommandEventManager.Instance.SendPartyApplicationRefuse(lInvitationNo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildInvitationAccept(CsHeroGuildInvitation csHeroGuildInvitation)
    {
        CsGuildManager.Instance.SendGuildInvitationAccept(csHeroGuildInvitation.Id);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildInvitationRefuse(CsHeroGuildInvitation csHeroGuildInvitation)
    {
        CsGuildManager.Instance.SendGuildInvitationRefuse(csHeroGuildInvitation.Id);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildCallAccept(CsGuildCall csGuildCall)
    {
        int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;

        if (CsGuildManager.Instance.Guild != null)
        {
            CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId || a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);
            if (csNationWarDeclaration != null && csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02020"));
                m_listCsGuildCall.Clear();
            }
            else if (nLocationId == 201 || CsGameData.Instance.GetContinent(nLocationId) != null)
            {
                CsGuildManager.Instance.SendGuildCallTransmission(csGuildCall.Id);
                GuildCallRemove(csGuildCall);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02019"));
                m_listCsGuildCall.Clear();
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_TXT_02019"));
            m_listCsGuildCall.Clear();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildCallRefuse(CsGuildCall csGuildCall)
    {
        GuildCallRemove(csGuildCall);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarCallAccept()
    {
        CsNationWarManager.Instance.SendNationWarCallTransmission();
        NationWarCallRemove();
    }

    void OnClickNationWarCallRefuse()
    {
        NationWarCallRemove();
    }

	#endregion Event

	//---------------------------------------------------------------------------------------------------
	void LoadInGameScene()
	{
		// 인게임씬로드.
		CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.InitEntranceLocationId);

		if (csContinent != null)
		{
			LoadSceneByName(csContinent.SceneName);
		}
		else if (CsGameData.Instance.UndergroundMaze.LocationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationId)
		{
			LoadSceneByName(CsGameData.Instance.UndergroundMaze.SceneName);
		}
		else
		{
			LoadSceneByName(CsGameData.Instance.GuildTerritory.SceneName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadResourceCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupMain/PopupMain");
        yield return resourceRequest;
        m_goPopupMain = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSupport/PopupSupport");
        yield return resourceRequest;
        m_goPopupSupport = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupTodayTask/PopupTodayTask");
        yield return resourceRequest;
        m_goPopupTodayTask = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSetting/PopupSetting");
        yield return resourceRequest;
        m_goPopupSetting = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNationWar/PopupNationWar");
        yield return resourceRequest;
        m_goPopupNationWar = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupOpenGift/PopupOpenGift");
        yield return resourceRequest;
        m_goPopupOpenGift = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupRookieGift/PopupRookieGift");
        yield return resourceRequest;
        m_goPopupRookieGift = (GameObject)resourceRequest.asset;

        resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupOpen7Day/PopupOpen7Day");
        yield return resourceRequest;
        m_goPopupOpen7Day = (GameObject)resourceRequest.asset;

		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupRetrieval/PopupRetrieval");
        yield return resourceRequest;
		m_goPopupRetrieval = (GameObject)resourceRequest.asset;

		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharging/PopupChargingEvent");
		yield return resourceRequest;
		m_goPopupChargingEvent = (GameObject)resourceRequest.asset;

		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharging/PopupSmallAmountCharging");
		yield return resourceRequest;
		m_goPopupSmallAmountCharging = (GameObject)resourceRequest.asset;
		
		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFieldBoss/PopupFieldBoss");
        yield return resourceRequest;
		m_goPopupFieldBoss = (GameObject)resourceRequest.asset;

		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupHelp/PopupHelp");
		yield return resourceRequest;
		m_goPopupHelp = (GameObject)resourceRequest.asset;

		resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharging/PopupCharging");
		yield return resourceRequest;
		m_goPopupCharging = (GameObject)resourceRequest.asset;
		
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSceneMainUICoroutine(string strSceneName)
    {
        SetLoadingImage(true);

        // MainUI 외의 다른 씬을 삭제한다.
        if (SceneManager.sceneCount > 1)
        {
            string strOldSceneName = SceneManager.GetActiveScene().name;
            AsyncOperation asyncOperationDelete = SceneManager.UnloadSceneAsync(strOldSceneName);
            yield return asyncOperationDelete;
            UnloadUnusedAssets();
        }

        // 조이스틱 리셋
        m_csUIJoyStick.Reset();

        // 새로운 씬을 로드한다.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        yield return asyncOperation;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(strSceneName));
    }

    //---------------------------------------------------------------------------------------------------
    void UnloadUnusedAssets(bool bForceUnloadInAndroid = true)
    {
#if UNITY_IOS || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
#else
        if (bForceUnloadInAndroid)
            Resources.UnloadUnusedAssets();
#endif
    }

	//---------------------------------------------------------------------------------------------------
	void OpenSinglePopup(EnMenuId enMenuId)
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			Transform trPopup = null;

			switch (enMenuId)
			{
				case EnMenuId.OpenGift:
					if (m_goPopupOpenGift != null)
					{
						trPopup = Instantiate<GameObject>(m_goPopupOpenGift, m_trPopupList).transform;
						trPopup.name = "PopupOpenGift";
					}
					break;

				case EnMenuId.RookieGift:
					if (m_goPopupRookieGift != null)
					{
						trPopup = Instantiate<GameObject>(m_goPopupRookieGift, m_trPopupList).transform;
						trPopup.name = "PopupRookieGift";
					}
					break;

				case EnMenuId.Open7Day:
					if (m_goPopupOpen7Day != null)
					{
						trPopup = Instantiate<GameObject>(m_goPopupOpen7Day, m_trPopupList).transform;
						trPopup.name = "PopupOpen7Day";
					}
					break;

				case EnMenuId.Retrieval:
					if (m_goPopupRetrieval != null)
					{
						trPopup = Instantiate<GameObject>(m_goPopupRetrieval, m_trPopupList).transform;
						trPopup.name = "PopupRetrieval";
					}
					break;

				case EnMenuId.ChargingEvent:
					if (m_goPopupChargingEvent != null)
					{
						trPopup = Instantiate<GameObject>(m_goPopupChargingEvent, m_trPopupList).transform;
						trPopup.name = "PopupChargingEvent";
					}
					break;
			}
			
			if (trPopup != null)
			{
				trPopup.SetAsLastSibling();
				trPopup.localPosition = new Vector3(0, 0, 0);
			}
		}

		m_bIsProcess = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupFieldBoss()
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			Transform trPopup = null;

			trPopup = Instantiate(m_goPopupFieldBoss, m_trPopup).transform;
			trPopup.name = "PopupFieldBoss";
			trPopup.SetAsLastSibling();
			trPopup.localPosition = new Vector3(0, 0, 0);
		}

		m_bIsProcess = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 팝업메인 도움말 팝업
	void OpenPopupHelp(EnMainMenu enMainMenu)
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			Transform trPopup = null;

			trPopup = Instantiate(m_goPopupHelp, m_trPopupList).transform;
			trPopup.name = "PopupHelp";
			trPopup.SetAsLastSibling();
			trPopup.localPosition = new Vector3(0, 0, 0);

			trPopup.GetComponent<CsPopupHelp>().SetHelp(enMainMenu);
		}

		m_bIsProcess = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 충전 팝업
	void OpenPopupCharging()
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			Transform trPopup = null;

			trPopup = Instantiate(m_goPopupCharging, m_trPopupList).transform;
			trPopup.name = "PopupCharging";
			trPopup.SetAsLastSibling();
			trPopup.localPosition = new Vector3(0, 0, 0);
		}

		m_bIsProcess = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 소액 충전 팝업
	void OpenPopupSmallAmountCharging()
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			Transform trPopup = null;

			trPopup = Instantiate(m_goPopupSmallAmountCharging, m_trPopupList).transform;
			trPopup.name = "PopupSmallAmountCharging";
			trPopup.SetAsLastSibling();
			trPopup.localPosition = new Vector3(0, 0, 0);
		}

		m_bIsProcess = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupOrdealQuest()
	{
		if (m_goPopupOrdeal == null)
		{
			StartCoroutine(LoadPopupOrdealQuest());
		}
		else
		{
			m_trPopupOrdeal = Instantiate(m_goPopupOrdeal, m_trPopupList).transform;
			m_trPopupOrdeal.name = "PopupOrdealQuest";
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupHalidomCollection()
	{
		if (!m_bIsProcess)
		{
			m_bIsProcess = true;

			GameObject goHalidomCollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/PopupHalidomCollection");

			Transform trPopup = null;

			trPopup = Instantiate(goHalidomCollection, m_trPopupList).transform;
			trPopup.name = "PopupHalidomCollection";
		}

		m_bIsProcess = false;
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopup(EnMainMenu enMainMenu, EnSubMenu enSubMenu, CsHeroInfo csHeroInfo)
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;
            if (enMainMenu == EnMainMenu.Support)
            {
                OpenPopupSupport(enMainMenu, enSubMenu);
            }
            else if (enMainMenu == EnMainMenu.TodayTask)
            {
                OpenPopupTodayTask(enMainMenu, enSubMenu);
            }
            else if (enMainMenu == EnMainMenu.ViewOtherUsers)
            {
                OpenPopupViewOtherUsers(enMainMenu, enSubMenu, csHeroInfo);
            }
            else if (enMainMenu == EnMainMenu.Setting)
            {
                OpenPopupSetting(enMainMenu, enSubMenu);
            }
            else if (enMainMenu == EnMainMenu.NationWar)
            {
                OpenPopupNationWar(enMainMenu, enSubMenu);
            }
            else if (enMainMenu == EnMainMenu.Guild)
            {
                if (CsGuildManager.Instance.GuildId == Guid.Empty)
                {
                    if (m_goPopupGuildApply == null)
                    {
                        StartCoroutine(OpenPopupGuildApplyCoroutine());

                    }
                    else
                    {
                        OpenPopupGuildApply();
                    }
                }
                else
                {
                    OpenPopupMain(enMainMenu, enSubMenu);
                }
            }
            else
            {
                OpenPopupMain(enMainMenu, enSubMenu);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupViewOtherUsers(EnMainMenu enMainMenu, EnSubMenu enSubMenue, CsHeroInfo csHeroInfo)
    {
        CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);

        if (m_trPopupMain == null)
        {
            m_trPopupMain = Instantiate<GameObject>(m_goPopupMain, m_trCanvas1).transform;
            m_trPopupMain.name = csMainMenu.PopupName;
            m_csPopupMain = m_trPopupMain.GetComponent<CsPopupMain>();
            m_csPopupMain.DisplayMenu(csMainMenu, enSubMenue, csHeroInfo);
        }
        else
        {
            m_trPopupMain.name = csMainMenu.PopupName;
            m_csPopupMain = m_trPopupMain.GetComponent<CsPopupMain>();
            m_csPopupMain.SubMenuClear();
            m_csPopupMain.DisplayMenu(csMainMenu, enSubMenue, csHeroInfo);
        }

        ToggleMainCamera(false);
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMain(EnMainMenu enMainMenu, EnSubMenu enSubMenue)
    {
		CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);
        if (m_trPopupMain == null)
        {
			m_trPopupMain = Instantiate<GameObject>(m_goPopupMain, m_trCanvas1).transform;
            m_trPopupMain.name = csMainMenu.PopupName;
            m_csPopupMain = m_trPopupMain.GetComponent<CsPopupMain>();
			m_csPopupMain.DisplayMenu(csMainMenu, enSubMenue);
        }
        else
        {
			m_trPopupMain.name = csMainMenu.PopupName;
            m_csPopupMain = m_trPopupMain.GetComponent<CsPopupMain>();
            m_csPopupMain.SubMenuClear();
			m_csPopupMain.DisplayMenu(csMainMenu, enSubMenue);
        }
		
		ToggleMainCamera(false);
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSupport(EnMainMenu enMainMenu, EnSubMenu enSubMenu)
    {
        if (m_trPopupSupport == null)
        {
            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);

            m_trPopupSupport = Instantiate<GameObject>(m_goPopupSupport, m_trCanvas2).transform;
            m_trPopupSupport.name = csMainMenu.PopupName;
            m_trPopupSupport.SetAsFirstSibling();
            m_trPopupSupport.localPosition = new Vector3(0, 0, 0);
            m_trPopupSupport.GetComponent<CsPopupSupport>().DisplayMenu(csMainMenu, enSubMenu);
        }

        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupTodayTask(EnMainMenu enMainMenu, EnSubMenu enSubMenu)
    {
        if (m_trPopupTodayTask == null)
        {
            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);

            m_trPopupTodayTask = Instantiate<GameObject>(m_goPopupTodayTask, m_trCanvas1).transform;
            m_trPopupTodayTask.name = csMainMenu.PopupName;
            m_trPopupTodayTask.SetAsFirstSibling();
            m_trPopupTodayTask.localPosition = new Vector3(0, 0, 0);
            CsPopupTodayTask csPopupTodayTask = m_trPopupTodayTask.GetComponent<CsPopupTodayTask>();
            csPopupTodayTask.DisplayMenu(csMainMenu, enSubMenu);
        }

        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSetting(EnMainMenu enMainMenu, EnSubMenu enSubMenu)
    {
        if (m_trPopupSetting == null)
        {
            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);

            m_trPopupSetting = Instantiate<GameObject>(m_goPopupSetting, m_trCanvas2).transform;
            m_trPopupSetting.name = csMainMenu.PopupName;
            m_trPopupSetting.SetAsFirstSibling();
            m_trPopupSetting.localPosition = new Vector3(0, 0, 0);
            CsPopupSetting csPopupSetting = m_trPopupSetting.GetComponent<CsPopupSetting>();
            csPopupSetting.DisplayMenu(csMainMenu, enSubMenu);
        }

        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupNationWar(EnMainMenu enMainMenu, EnSubMenu enSubMenu)
    {
        if (m_trPopupNationWar == null)
        {
            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu((int)enMainMenu);

            m_trPopupNationWar = Instantiate<GameObject>(m_goPopupNationWar, m_trCanvas2).transform;
            m_trPopupNationWar.name = csMainMenu.PopupName;
            m_trPopupNationWar.SetAsFirstSibling();
            m_trPopupNationWar.localPosition = new Vector3(0, 0, 0);

            CsPopupNationWar csPopupNationWar = m_trPopupNationWar.GetComponent<CsPopupNationWar>();

            if (csPopupNationWar != null)
            {
                csPopupNationWar.DisplayMenu(csMainMenu, enSubMenu);
            }
        }

        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void ToggleMainCamera(bool bVisible)
    {
        if (bVisible)
        {
            if (m_nCurMask == 0)
                return;

            Camera.main.cullingMask = m_nCurMask;
        }
        else
        {
            if (Camera.main != null && Camera.main.cullingMask != 0)
            {
                m_nCurMask = Camera.main.cullingMask;
                Camera.main.cullingMask = 0;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //임시로딩창
    public void SetLoadingImage(bool bIson = true)
    {
        //if (bIson)
        //	m_csPanelMinimap.IsStop = true;

        m_trLoadingImage.gameObject.SetActive(bIson);
    }

    //---------------------------------------------------------------------------------------------------
    // 휴식보상오픈
    IEnumerator OpenPopupRestRewardCoroutine()
    {
        if (CsGameConfig.Instance.RestRewardRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.RestTime >= 10)
        {
            // Open
            ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupRestReward");
            yield return resourceRequest;
            GameObject goPopupRestReward = (GameObject)resourceRequest.asset;

            if (!m_trPanelPopup.gameObject.activeSelf)
                m_trPanelPopup.gameObject.SetActive(true);

            m_trPopupRestReward = Instantiate<GameObject>(goPopupRestReward, m_trPanelPopup).transform;
            m_trPopupRestReward.name = goPopupRestReward.name;
            m_trPopupRestReward.localPosition = new Vector3(0, 0, 0);
        }

        yield return new WaitUntil(() => m_trPopupRevive != null);

        if (m_trPopupRevive != null && m_trPopupRestReward != null)
        {
            m_trPopupRestReward.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //부활
    IEnumerator OpenPopupReviveCoroutine(string strName)
    {
        // Open
        if (m_goPopupRevive == null)
        {
            ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupRevive");
            yield return resourceRequest;
            m_goPopupRevive = (GameObject)resourceRequest.asset;
        }

        yield return new WaitForSeconds(3f);

        OpenPopupRevive(strName);

        if (m_iEnumeratorOpenPopupRevive != null)
        {
            StopCoroutine(m_iEnumeratorOpenPopupRevive);
            m_iEnumeratorOpenPopupRevive = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupRevive(string strName)
    {
        if (!m_trPanelPopup.gameObject.activeSelf)
            m_trPanelPopup.gameObject.SetActive(true);

        m_trPopupRevive = Instantiate(m_goPopupRevive, m_trPanelPopup).transform;
        m_trPopupRevive.name = m_goPopupRevive.name;
        m_trPopupRevive.localPosition = new Vector3(0, 0, 0);

        CsPopupRevive csPopupRevive = m_trPopupRevive.GetComponent<CsPopupRevive>();
        CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId);

        // 대륙
        if (csContinent != null)
        {
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.Revival;
            csPopupRevive.DisplayRevive(EnReviveLocationType.Continent, strName);
        }
        // 길드 영지
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.Revival;
            csPopupRevive.DisplayRevive(EnReviveLocationType.GuildTerritory, strName);
        }
        // 던전
        else
        {
            switch (CsUIData.Instance.DungeonInNow)
            {
                case EnDungeon.MainQuestDungeon:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.MainQuestDungeon, strName);
                    break;

                case EnDungeon.StoryDungeon:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.StoryDungeon, strName);
                    break;

                case EnDungeon.ExpDungeon:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.ExpDungeon, strName);
                    break;

                case EnDungeon.GoldDungeon:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.GoldDungeon, strName);
                    break;

                case EnDungeon.UndergroundMaze:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.UndergroundMaze, strName);
                    break;

                case EnDungeon.AncientRelic:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.AncientRelic, strName);
                    break;

                case EnDungeon.SoulCoveter:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.SoulConveter, strName);
                    break;

                case EnDungeon.EliteDungeon:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.EliteDungeon, strName);
                    break;

				case EnDungeon.RuinsReclaim:
					csPopupRevive.DisplayRevive(EnReviveLocationType.RuinsReclaim, strName);
					break;

                case EnDungeon.InfiniteWar:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.InfiniteWar, strName);
                    break;

				case EnDungeon.FearAltar:
					csPopupRevive.DisplayRevive(EnReviveLocationType.FearAltar, strName);
					break;

                case EnDungeon.WarMemory:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.WarMemory, strName);
                    break;

				case EnDungeon.BiographyDungeon:
					csPopupRevive.DisplayRevive(EnReviveLocationType.BiographyQuestDungeon, strName);
					break;

                case EnDungeon.DragonNest:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.DragonNest, strName);
                    break;

                case EnDungeon.TradeShip:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.TradeShip, strName);
                    break;

                case EnDungeon.AnkouTomb:
                    csPopupRevive.DisplayRevive(EnReviveLocationType.AnkouTomb, strName);
                    break;
            }
        }
    }


    //---------------------------------------------------------------------------------------------------
    //길드 신청
    IEnumerator OpenPopupGuildApplyCoroutine()
    {
        // Open
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGuildApply");
        yield return resourceRequest;
        m_goPopupGuildApply = (GameObject)resourceRequest.asset;

        OpenPopupGuildApply();

    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupGuildApply()
    {
        if (!m_trPanelPopup.gameObject.activeSelf)
            m_trPanelPopup.gameObject.SetActive(true);

        m_trPopupGuildApply = Instantiate(m_goPopupGuildApply, m_trPanelPopup).transform;
        m_trPopupGuildApply.name = m_goPopupGuildApply.name;
        m_trPopupGuildApply.localPosition = new Vector3(0, 0, 0);

        m_csPopupGuildApply = m_trPopupGuildApply.GetComponent<CsPopupGuildApply>();
        m_csPopupGuildApply.EventCloseGuildApply += OnEventCloseGuildApply;

        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator OpenPopupNationWarAttend()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupNationWarAttend");
        yield return resourceRequest;
        m_goPopupNationWarAttend = (GameObject)resourceRequest.asset;

        m_trPopupNationWarAttend = Instantiate(m_goPopupNationWarAttend, m_trPanelPopup).transform;
        m_trPopupNationWarAttend.name = m_goPopupNationWarAttend.name;
        m_trPopupNationWarAttend.localPosition = new Vector3(0, 0, 0);

        CsPopupNationWarAttend csPopupNationWarAttend = m_trPopupNationWarAttend.GetComponent<CsPopupNationWarAttend>();
        csPopupNationWarAttend.EventCloseNationWarAttend += OnEventCloseNationWarAttend;

        if (!m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 카트오픈
    //IEnumerator OpenPopupCartCoroutine()
    //{
    //    if ((CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > 1) && CsGameData.Instance.MyHeroInfo.RestTime >= 10)
    //    {
    //        // Open
    //        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupRestReward");
    //        yield return resourceRequest;
    //        GameObject goPopupRestReward = (GameObject)resourceRequest.asset;

    //        if (!m_trPanelPopup.gameObject.activeSelf)
    //            m_trPanelPopup.gameObject.SetActive(true);

    //        m_trPopupRestReward = Instantiate<GameObject>(goPopupRestReward, m_trPanelPopup).transform;
    //        m_trPopupRestReward.name = goPopupRestReward.name;
    //        m_trPopupRestReward.localPosition = new Vector3(0, 0, 0);
    //    }

    //    yield return new WaitUntil(() => m_trPopupRevive != null);

    //    if (m_trPopupRevive != null && m_trPopupRestReward != null)
    //    {
    //        m_trPopupRestReward.gameObject.SetActive(false);
    //    }
    //}

    #region 씬 로드

    //---------------------------------------------------------------------------------------------------
    // 대륙씬로드
    void LoadContinentScene(int nContitnentId)
    {
        CsContinent csContinent = CsGameData.Instance.GetContinent(nContitnentId);

        if (csContinent != null)
        {
            StartCoroutine(LoadSceneMainUICoroutine(csContinent.SceneName));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인퀘스트 던전씬로드
    void LoadMainQuestDungeonScene()
    {
        if (CsMainQuestDungeonManager.Instance.MainQuestDungeon != null)
        {
            StartCoroutine(LoadSceneMainUICoroutine(CsMainQuestManager.Instance.MainQuest.MainQuestDungeonTarget.SceneName));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 던전씬로드
    void LoadSceneByName(string strSceneName)
    {
        StartCoroutine(LoadSceneMainUICoroutine(strSceneName));
    }

    #endregion 씬 로드

    //---------------------------------------------------------------------------------------------------
    //피싱 파티 초기화
    void FishingPartyReset()
    {
        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingPartyDate))
        {
            DateTime dtNew = CsGameData.Instance.MyHeroInfo.CurrentDateTime;
            DateTime dtOld = DateTime.Parse(PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyFishingPartyDate, CsGameData.Instance.MyHeroInfo.DateTimeBase.ToString()));
            //같은 날짜일 경우
            if (dtOld.DayOfWeek == dtNew.DayOfWeek)
            {
                if (dtOld.DayOfYear != dtNew.DayOfYear)
                {
                    if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingParty))
                    {
                        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty, 0);
                    }
                }
            }
            else if (dtOld.DayOfWeek != DayOfWeek.Monday && dtNew.DayOfWeek < dtOld.DayOfWeek)
            {
                if (dtOld.DayOfYear != dtNew.DayOfYear)

                {
                    if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingParty))
                    {
                        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty, 0);
                    }
                }
            }
            else if (dtOld.DayOfWeek == DayOfWeek.Sunday && dtNew.DayOfWeek > dtOld.DayOfWeek)
            {
                if (dtOld.DayOfYear != dtNew.DayOfYear)
                {
                    if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFishingParty))
                    {
                        PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFishingParty, 0);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //길드 소집 지우기
    void GuildCallRemove(CsGuildCall csGuildCall)
    {
        m_listCsGuildCall.Remove(csGuildCall);

        if (m_listCsGuildCall.Count != 0)
        {
            CsGuildCall csGuildCalled = m_listCsGuildCall[0];

            int nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
            CsContinent csContinent = CsGameData.Instance.GetContinent(nLocationId);
            CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId || a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);

            //길드 소집 조건
            //길드 소속 And Pvp 가능레벨
            if (CsGuildManager.Instance.Guild != null)
            {
                if (csNationWarDeclaration != null && csNationWarDeclaration.Status == EnNationWarDeclaration.Current)
                {
                    m_listCsGuildCall.Clear();
                    CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
                }
                else if (nLocationId == 201 || csContinent != null)
                {
                    CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.GuildModal, string.Format(CsConfiguration.Instance.GetString("A58_TXT_01012"),
                        CsGameData.Instance.GetGuildMemberGrade(csGuildCalled.CallerMemberGrade).Name, csGuildCalled.CallerName),
                        (() => OnClickGuildCallAccept(csGuildCalled)), (() => OnClickGuildCallRefuse(csGuildCalled)), csGuildCalled.RemainingTime);
                }
                else
                {
                    m_listCsGuildCall.Clear();
                    CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
                }
            }
            else
            {
                m_listCsGuildCall.Clear();
                CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.GuildModal);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void NationWarCallRemove()
    {
        m_guidNationWarCall = Guid.Empty;
        CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.NationModal);
    }

    //---------------------------------------------------------------------------------------------------
    // 날짜 바뀜 초기화
    void InitializeNewDate()
    {
        DateTime dtNew = CsGameData.Instance.MyHeroInfo.TimeOffset.Date;

        if (dtNew.DayOfWeek == DayOfWeek.Monday)
        {
            FishingPartyReset();
            CsNationWarManager.Instance.ResetWeeklyNationWarDeclarationCount();

            // SoulCoveter
            CsGameData.Instance.SoulCoveter.SoulCoveterWeeklyPlayCount = 0;
            CsGameData.Instance.SoulCoveter.SoulCoveterPlayCountDate = dtNew;

			CsGameData.Instance.MyHeroInfo.ResetWeeklyHalidomCollection();

			CsPresentManager.Instance.ResetPoint(dtNew);
        }
		else if (dtNew.DayOfWeek == DayOfWeek.Tuesday)
		{
			// 주말보상
			if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward != null)
			{ 
				CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Reset(dtNew);
			}
		}

        foreach (KeyValuePair<string, bool> item in m_dicScheduleNotice)
        {
            m_dicScheduleNotice[item.Key] = false;
        }

        // 메인장비인챈트
        CsGameData.Instance.MyHeroInfo.MainGearEnchantDate = dtNew;
        CsGameData.Instance.MyHeroInfo.MainGearEnchantDailyCount = 0;

        // 메인장비세련
        CsGameData.Instance.MyHeroInfo.MainGearRefineDate = dtNew;
        CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount = 0;

        // 금일무료즉시부활
        CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDate = dtNew;
        CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDailyCount = 0;

        // 금일유료즉시부활
        CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = dtNew;
        CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = 0;

        // 금일경험치물약사용
        CsGameData.Instance.MyHeroInfo.ExpPotionUseCountDate = dtNew;
        CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount = 0;

        // 받은 일일접속시간보상
        CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardDate = dtNew;
        CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList.Clear();

        // 탈것장비 일일재강화횟수
        CsGameData.Instance.MyHeroInfo.MountGearRefinementDate = dtNew;
        CsGameData.Instance.MyHeroInfo.MountGearRefinementDailyCount = 0;

        // 일일접속시간
        CsGameData.Instance.MyHeroInfo.DailyAccessTime = 0;

        // 스토리던전 reset
        for (int i = 0; i < CsGameData.Instance.StoryDungeonList.Count; i++)
        {
            CsGameData.Instance.StoryDungeonList[i].Reset(dtNew);
        }

        // 금일무료소탕횟수
        CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = 0;
        CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = dtNew;

        // 금일경험치주문서사용횟수
        CsGameData.Instance.MyHeroInfo.ExpScrollDailyUseCount = 0;
        CsGameData.Instance.MyHeroInfo.ExpPotionUseCountDate = dtNew;

        // 금일경험치던전플레이횟수
        CsGameData.Instance.ExpDungeon.ExpDungeonDailyPlayCount = 0;
        CsGameData.Instance.ExpDungeon.ExpDungeonPlayCountDate = dtNew;

        // 금일골드던전플레이횟수
        CsGameData.Instance.GoldDungeon.GoldDungeonDailyPlayCount = 0;
        CsGameData.Instance.GoldDungeon.GoldDungeonPlayCountDate = dtNew;

        // 금일지하미로플레이시간
        CsGameData.Instance.UndergroundMaze.UndergroundMazePlayTimeDate = dtNew;
        CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime = 0;

        // 고대유물의방
        CsGameData.Instance.ArtifactRoom.ArtifactRoomDailyInitCount = 0;
        CsGameData.Instance.ArtifactRoom.ArtifactRoomInitCountDate = dtNew;

        // 금일 현상금사냥꾼퀘스트시작횟수
        CsBountyHunterQuestManager.Instance.BountyHunterQuestDailyStartCount = 0;
        CsBountyHunterQuestManager.Instance.BountyHunterQuestStartCountDate = dtNew;

        // 금일 낚시퀘스트시작횟수
        CsFishingQuestManager.Instance.FishingQuestDailyStartCount = 0;
        CsFishingQuestManager.Instance.FishingQuestStartCountDate = dtNew;

        // 공적포인트
        CsGameData.Instance.MyHeroInfo.DailyExploitPoint = 0;
        CsGameData.Instance.MyHeroInfo.ExploitPointDate = dtNew;

        // 금일 의문의상자퀘스트 시작횟수
        CsMysteryBoxQuestManager.Instance.DailyMysteryBoxQuestStartCount = 0;
        CsMysteryBoxQuestManager.Instance.MysteryBoxQuestStartCountDate = dtNew;

        // 금일 밀서퀘스트 시작횟수
        CsSecretLetterQuestManager.Instance.DailySecretLetterQuestStartCount = 0;
        CsSecretLetterQuestManager.Instance.SecretLetterQuestStartCountDate = dtNew;

        // 금일 차원습격퀘스트 시작횟수
        CsDimensionRaidQuestManager.Instance.DailyDimensionRaidQuestStartCount = 0;
        CsDimensionRaidQuestManager.Instance.DimensionRaidQuestStartCountDate = dtNew;

        // 금일 고대인의유적 플레이횟수
        CsGameData.Instance.AncientRelic.AncientRelicDailyPlayCount = 0;
        CsGameData.Instance.AncientRelic.AncientRelicPlayCountDate = dtNew;

        // 금일 용맹의 증명 플레이 횟수
		CsGameData.Instance.ProofOfValor.DailyPlayCount = 0;
		CsGameData.Instance.ProofOfValor.DailyPlayCountDate = dtNew;
		CsGameData.Instance.ProofOfValor.MyDailyFreeRefreshCount = 0;
		CsGameData.Instance.ProofOfValor.MyDailyPaidRefreshCount = 0;

        // 금일 달성포인트
        CsGameData.Instance.MyHeroInfo.AchievementDailyPoint = 0;
        CsGameData.Instance.MyHeroInfo.AchievementPointDate = dtNew;

        // 금일 받은 달성보상번호
        CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo = 0;
        CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardDate = dtNew;

        // 오늘의 할일
        CsGameData.Instance.MyHeroInfo.HeroTodayTaskList.Clear();

        // 금일 왜곡주문서사용횟수
        CsGameData.Instance.MyHeroInfo.DistortionScrollDailyUseCount = 0;
        CsGameData.Instance.MyHeroInfo.DistortionScrollUseCountDate = dtNew;

        // 검투 대회
        CsGameData.Instance.FieldOfHonor.FieldOfHonorDailyPlayCount = 0;
        CsGameData.Instance.FieldOfHonor.FieldOfHonorPlayCountDate = dtNew;

        // 금일 일일길드신청수
        CsGuildManager.Instance.DailyGuildApplicationCount = 0;
        CsGuildManager.Instance.GuildApplicationCountDate = dtNew;

        // 금일 일일길드기부횟수
        CsGuildManager.Instance.DailyGuildDonationCount = 0;
        CsGuildManager.Instance.GuildDonationCountDate = dtNew;

        // 일일강퇴건수
        CsGuildManager.Instance.DailyBanishmentCount = 0;
        CsGuildManager.Instance.BanishmentCountDate = dtNew;

        // 세리우 보급지원
        CsSupplySupportQuestManager.Instance.DailySupplySupportQuestCount = 0;
        CsSupplySupportQuestManager.Instance.SupplySupportQuestCountDate = dtNew;

        // 금일 일일길드농장퀘스트시작횟수
        CsGuildManager.Instance.DailyGuildFarmQuestStartCount = 0;
        CsGuildManager.Instance.GuildFarmQuestStartCountDate = dtNew;

        // 금일 일일길드군량창고납부횟수
        CsGuildManager.Instance.DailyGuildFoodWarehouseStockCount = 0;
        CsGuildManager.Instance.GuildFoodWarehouseStockCountDate = dtNew;

        // 금일길드모럴포인트
        CsGuildManager.Instance.GuildMoralPoint = 0;
        CsGuildManager.Instance.GuildMoralPointDate = dtNew;

        if (CsGuildManager.Instance.Guild != null)
        {
            CsGuildManager.Instance.Guild.MoralPoint = 0;
            CsGuildManager.Instance.Guild.MoralPointDate = dtNew;
        }

        // 금일 일일국가기부횟수
        CsGameData.Instance.MyHeroInfo.DailyNationDonationCount = 0;
        CsGameData.Instance.MyHeroInfo.NationDonationCountDate = dtNew;

        // 금일 일일길드헌팅퀘스트시작횟수
        CsGuildManager.Instance.DailyGuildHuntingQuestStartCount = 0;
        CsGuildManager.Instance.GuildHuntingQuestStartCountDate = dtNew;

        // 길드일일목표보상받은번호
        CsGuildManager.Instance.GuildDailyObjectiveRewardReceivedNo = 0;
        CsGuildManager.Instance.GuildDailyObjectiveRewardReceivedDate = dtNew;

        // 금일크리쳐카드상점유료갱신횟수
        CsCreatureCardManager.Instance.DailyCreatureCardShopPaidRefreshCount = 0;
        CsCreatureCardManager.Instance.CreatureCardShopPaidRefreshCountDate = dtNew;

        // 금일 체력구매횟수
        CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount = 0;
        CsGameData.Instance.MyHeroInfo.StaminaBuyCountDate = dtNew;

        // 정예 던전 입장횟수
        CsEliteManager.Instance.DailyEliteDungeonPlayCount = 0;
        CsEliteManager.Instance.EliteDungeonPlayCountDate = dtNew;

		// 데일리퀘스트
		CsDailyQuestManager.Instance.DailyQuestAcceptionCount = 0;
		CsDailyQuestManager.Instance.QuestAcceptionCountDate = dtNew;
		CsDailyQuestManager.Instance.DailyQuestFreeRefreshCount = 0;
		CsDailyQuestManager.Instance.QuestFreeRefreshCountDate = dtNew;

		// 영웅회수진행카운트
		CsGameData.Instance.MyHeroInfo.HeroRetrievalList.Clear();

		// 할일위탁
		CsGameData.Instance.MyHeroInfo.HeroTaskConsignmentStartCountList.Clear();

		// 지혜의 신전
		CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount = 0;
		CsGameData.Instance.WisdomTemple.PlayDate = dtNew;

		// 유적탈환
		CsGameData.Instance.RuinsReclaim.FreePlayCount = 0;
		CsGameData.Instance.RuinsReclaim.PlayDate = dtNew;

		// 무한대전
		CsGameData.Instance.InfiniteWar.DailyPlayCount = 0;
		CsGameData.Instance.InfiniteWar.PlayDate = dtNew;

		// 공포의 제단
		CsGameData.Instance.FearAltar.DailyFearAltarPlayCount = 0;
		CsGameData.Instance.FearAltar.PlayDate = dtNew;

		// 한정선물
		CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.Clear();

		// 다이아상점
		CsGameData.Instance.MyHeroInfo.HeroDiaShopProductBuyCountList.Clear();

		// 행운상점
		CsLuckyShopManager.Instance.ItemLuckyShopPickDate = dtNew;
		CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount = 0;
		CsLuckyShopManager.Instance.ItemLuckyShopPick1TimeCount = 0;
		CsLuckyShopManager.Instance.ItemLuckyShopPick5TimeCount = 0;

		CsLuckyShopManager.Instance.CreatureCardLuckyShopPickDate = dtNew;
		CsLuckyShopManager.Instance.CreatureCardLuckyShopFreePickCount = 0;
		CsLuckyShopManager.Instance.CreatureCardLuckyShopPick1TimeCount = 0;
		CsLuckyShopManager.Instance.CreatureCardLuckyShopPick5TimeCount = 0;

		// 크리처
		CsCreatureManager.Instance.CreatureVariationCountDate = dtNew;
		CsCreatureManager.Instance.DailyCreatureVariationCount = 0;

		// 크리처농장퀘스트
		CsCreatureFarmQuestManager.Instance.CreatureFarmQuestAcceptionCountDate = dtNew;
		CsCreatureFarmQuestManager.Instance.DailyCreatureFarmQuestAcceptionCount = 0;

		// 매일충전이벤트
		CsCashManager.Instance.DailyChargeEventDate = dtNew;
		CsCashManager.Instance.DailyChargeEventAccUnOwnDia = 0;
		CsCashManager.Instance.RewardedDailyChargeEventMissionList.Clear();

		// 매일소비이벤트
		CsCashManager.Instance.DailyConsumeEventDate = dtNew;
		CsCashManager.Instance.DailyConsumeEventAccDia = 0;
		CsCashManager.Instance.RewardedDailyConsumeEventMissionList.Clear();

		// 금일별의정수사용횟수
		CsConstellationManager.Instance.DailyStarEssenseItemUseCount = 0;
		CsConstellationManager.Instance.StarEssenseItemUseCountDate = dtNew;
	}

	#region 빠른 사용 팝업

	//---------------------------------------------------------------------------------------------------
	// 장비장착 / 아이템사용 숏컷
	void OpenPopupQuickUse(CsHeroMainGear csHeroMainGear)
    {
        if (m_goPopupQuickUse == null)
        {
            StartCoroutine(LoadPopupQuickUseCoroutine(() => InitializePopupQuickUseMainGear(csHeroMainGear)));
        }
        else
        {
            InitializePopupQuickUseMainGear(csHeroMainGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupQuickUse(CsHeroSubGear csHeroSubGear)
    {
        if (m_goPopupQuickUse == null)
        {
            StartCoroutine(LoadPopupQuickUseCoroutine(() => InitializePopupQuickUseSubGear(csHeroSubGear)));
        }
        else
        {
            InitializePopupQuickUseSubGear(csHeroSubGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupQuickUse(CsHeroItem csHeroItem)
    {
        if (m_goPopupQuickUse == null)
        {
            StartCoroutine(LoadPopupQuickUseCoroutine(() => InitializePopupQuickUseItem(csHeroItem)));
        }
        else
        {
            InitializePopupQuickUseItem(csHeroItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupQuickUseMainGear(CsHeroMainGear csHeroMainGear)
    {
        CreatePopupQuickUse();

        Transform trBack = m_trPopupQuickUse.Find("ImageBackground");

        Transform trItemSlot = trBack.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGear, EnItemSlotSize.Small);

        Transform textList = trBack.Find("TextList");

        Text textItemName = textList.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = csHeroMainGear.MainGear.Name;

        Transform trBattlePower = textList.Find("BattlePower");
        trBattlePower.gameObject.SetActive(true);

        Text textValue = trBattlePower.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);

        CsHeroMainGear csHeroMainGearEquipped = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[csHeroMainGear.MainGear.MainGearType.EquippedIndex];

        if (csHeroMainGearEquipped != null)
        {
            textValue.text = (csHeroMainGear.BattlePower - csHeroMainGearEquipped.BattlePower).ToString("#,##0");
        }
        else
        {
            textValue.text = csHeroMainGear.BattlePower.ToString("#,##0");
        }

        Button buttonEquip = trBack.Find("ButtonUse").GetComponent<Button>();
        buttonEquip.onClick.RemoveAllListeners();
        buttonEquip.onClick.AddListener(() => OnClickQuickEquip(csHeroMainGear));
        buttonEquip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text TextEquip = trBack.Find("ButtonUse/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(TextEquip);
        TextEquip.text = CsConfiguration.Instance.GetString("A04_BTN_00001");

        m_iEnumeratorQuickUse = QuickUseTimeCoroutine(csHeroMainGear, TextEquip, CsConfiguration.Instance.GetString("A04_BTN_00001"));
        StartCoroutine(m_iEnumeratorQuickUse);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupQuickUseSubGear(CsHeroSubGear csHeroSubGear)
    {
        CreatePopupQuickUse();

        Transform trBack = m_trPopupQuickUse.Find("ImageBackground");

        Transform trItemSlot = trBack.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGear, EnItemSlotSize.Small);

        Transform textList = trBack.Find("TextList");

        Text textItemName = textList.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = csHeroSubGear.Name;

        Transform trBattlePower = textList.Find("BattlePower");
        trBattlePower.gameObject.SetActive(true);

        Text textValue = trBattlePower.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);
        textValue.text = csHeroSubGear.BattlePower.ToString("#,##0");

        Button buttonEquip = trBack.Find("ButtonUse").GetComponent<Button>();
        buttonEquip.onClick.RemoveAllListeners();
        buttonEquip.onClick.AddListener(() => OnClickQuickEquip(csHeroSubGear));
        buttonEquip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text TextEquip = trBack.Find("ButtonUse/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(TextEquip);
        TextEquip.text = CsConfiguration.Instance.GetString("A04_BTN_00001");

        m_iEnumeratorQuickUse = QuickUseTimeCoroutine(csHeroSubGear, TextEquip, CsConfiguration.Instance.GetString("A04_BTN_00001"));
        StartCoroutine(m_iEnumeratorQuickUse);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupQuickUseItem(CsHeroItem csHeroItem)
    {
        CreatePopupQuickUse();

        Transform trBack = m_trPopupQuickUse.Find("ImageBackground");

        Transform trItemSlot = trBack.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroItem.Item, csHeroItem.Owned, csHeroItem.Count, true, EnItemSlotSize.Small);

        Transform textList = trBack.Find("TextList");

        Text textItemName = textList.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = csHeroItem.Item.Name;

        Transform trBattlePower = textList.Find("BattlePower");
        trBattlePower.gameObject.SetActive(false);

        Button buttonEquip = trBack.Find("ButtonUse").GetComponent<Button>();
        buttonEquip.onClick.RemoveAllListeners();
        buttonEquip.onClick.AddListener(() => OnClickQuickUse(csHeroItem));
        buttonEquip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text TextEquip = trBack.Find("ButtonUse/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(TextEquip);
        TextEquip.text = CsConfiguration.Instance.GetString("A04_BTN_00002");

        m_iEnumeratorQuickUse = QuickUseTimeCoroutine(csHeroItem, TextEquip, CsConfiguration.Instance.GetString("A04_BTN_00002"));
        StartCoroutine(m_iEnumeratorQuickUse);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuickEquip(CsHeroMainGear csHeroMainGear)
    {
        CsCommandEventManager.Instance.SendMainGearEquip(csHeroMainGear.Id);
        ClosePopupQuickUse();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuickEquip(CsHeroSubGear csHeroSubGear)
    {
        CsCommandEventManager.Instance.SendSubGearEquip(csHeroSubGear.SubGear.SubGearId);
        ClosePopupQuickUse();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQuickUse(CsHeroItem csHeroItem)
    {
        int nUseCount = 1;

        if (csHeroItem.Item.UsingType == EnUsingType.Multiple)
            nUseCount = csHeroItem.Count;

        if (CsGameData.Instance.MyHeroInfo.GetItemCount(csHeroItem.Item.ItemId) < nUseCount)
        {
            ClosePopupQuickUse();
        }
        else
        {
            CsCommandEventManager.Instance.SendItemUse(csHeroItem.InventorySlotIndex, nUseCount);
            ClosePopupQuickUse();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupQuickUseCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupQuickUse");
        yield return resourceRequest;
        m_goPopupQuickUse = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupQuickUse()
    {
        GameObject goPopupQuickUse = Instantiate(m_goPopupQuickUse, m_trPopup);
        goPopupQuickUse.name = "PopupQuickUse";
        m_trPopupQuickUse = goPopupQuickUse.transform;
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupQuickUse()
    {
        m_bOpenHeroObjectGot = false;

        if (m_iEnumeratorQuickUse != null)
        {
            StopCoroutine(m_iEnumeratorQuickUse);
            m_iEnumeratorQuickUse = null;
        }

        if (m_trPopupQuickUse != null)
        {
            Destroy(m_trPopupQuickUse.gameObject);
        }

        m_listHeroObjectDisplay.RemoveAt(0);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator QuickUseTimeCoroutine(CsHeroObject csHeroObject, Text text1, string str1)
    {
        float flTIme = CsGameConfig.Instance.ItemToastDisplayDuration;


        while (flTIme > 0)
        {
            text1.text = string.Format(str1, flTIme.ToString("0"));
            yield return new WaitForSeconds(1f);
            flTIme--;
        }

        if (m_bOpenHeroObjectGot)
        {
            switch (csHeroObject.HeroObjectType)
            {
                case EnHeroObjectType.MainGear:
                    CsHeroMainGear csHeroMainGear = (CsHeroMainGear)csHeroObject;
                    CsHeroMainGear csHeroMainGearEquipped = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[csHeroMainGear.MainGear.MainGearType.EquippedIndex];

                    if (csHeroMainGearEquipped == null)
                    {
                        OnClickQuickEquip(csHeroMainGear);
                    }
                    else
                    {
                        ClosePopupQuickUse();
                    }
                    break;

                case EnHeroObjectType.SubGear:
                    OnClickQuickEquip((CsHeroSubGear)csHeroObject);
                    break;

                case EnHeroObjectType.Item:
                    ClosePopupQuickUse();
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 획득 메인장비, 보조장비, 아이템 장착 또는 사용.
    IEnumerator DisplayGetHeroObjectCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_listHeroObjectDisplay.Count > 0 && !m_bOpenHeroObjectGot);

            switch (m_listHeroObjectDisplay[0].HeroObjectType)
            {
                case EnHeroObjectType.MainGear:
                    CsHeroMainGear csHeroMainGear = (CsHeroMainGear)m_listHeroObjectDisplay[0];
                    CsHeroMainGear csHeroMainGearEquipped = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[csHeroMainGear.MainGear.MainGearType.EquippedIndex];

                    if (csHeroMainGearEquipped != null && csHeroMainGearEquipped.BattlePower > csHeroMainGear.BattlePower)
                    {
                        m_listHeroObjectDisplay.RemoveAt(0);
                    }
                    else
                    {
                        OpenPopupQuickUse(csHeroMainGear);
                        m_bOpenHeroObjectGot = true;
                    }
                    break;

                case EnHeroObjectType.SubGear:
                    CsHeroSubGear csHeroSubGear = (CsHeroSubGear)m_listHeroObjectDisplay[0];
                    OpenPopupQuickUse(csHeroSubGear);
                    m_bOpenHeroObjectGot = true;
                    break;

                case EnHeroObjectType.Item:
                    CsHeroItem csHeroItem = (CsHeroItem)m_listHeroObjectDisplay[0];
                    
                    OpenPopupQuickUse(csHeroItem);
                    m_bOpenHeroObjectGot = true;
                    break;
            }
        }
    }

    #endregion 빠른 사용 팝업

    #region 드랍 아이템 팝업

    //---------------------------------------------------------------------------------------------------
    IEnumerator DisplayGetDropObjectCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_listcsDropObject.Count > 0);

            OpenPopupDropObject(m_listcsDropObject[0]);

            yield return new WaitForSeconds(1.5f);
            m_listcsDropObject.RemoveAt(0);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDropObject(CsDropObject csDropObject)
    {
        if (m_goPopupDropObject == null)
        {
            StartCoroutine(LoadPopupDropObjectCoroutine(() => InitializePopupDropObject(csDropObject)));
        }
        else
        {
            InitializePopupDropObject(csDropObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupDropObject(CsDropObject csDropObject)
    {
        GameObject goPopupDropObject = Instantiate(m_goPopupDropObject, m_trPopup);
        goPopupDropObject.name = "PopupDropObject";
        Transform trPopupDropObject = goPopupDropObject.transform;

        Transform trBack = trPopupDropObject.Find("ImageBackground");
        Transform trItemSlot = trBack.Find("ItemSlot");

        Text textName = trBack.Find("TextObjectName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        switch (csDropObject.DropObjectTypeType)
        {
            case EnDropObjectType.MainGear:
                CsDropObjectMainGear csDropObjectMainGear = (CsDropObjectMainGear)csDropObject;
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csDropObjectMainGear.MainGear, csDropObjectMainGear.EnchantLevel, 0, csDropObjectMainGear.Owned, EnItemSlotSize.Small);
                textName.text = csDropObjectMainGear.MainGear.Name;
                break;

            case EnDropObjectType.Item:
                CsDropObjectItem csDropObjectItem = (CsDropObjectItem)csDropObject;
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csDropObjectItem.Item, csDropObjectItem.Owned, 0, true, EnItemSlotSize.Small);
                textName.text = csDropObjectItem.Item.Name;
                break;

            case EnDropObjectType.MountGear:
                CsDropObjectMountGear csDropObjectMountGear = (CsDropObjectMountGear)csDropObject;
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csDropObjectMountGear.MountGear, csDropObjectMountGear.Owned, EnItemSlotSize.Small);
                textName.text = csDropObjectMountGear.MountGear.Name;
                break;
        }

        Destroy(trPopupDropObject.gameObject, 1.5f);
        CsUIData.Instance.PlayUISound(EnUISoundType.Item);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupDropObjectCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupDropObject");
        yield return resourceRequest;
        m_goPopupDropObject = (GameObject)resourceRequest.asset;

        unityAction();
    }

    #endregion 드랍 아이템 팝업

    #region 유저 조회

    //---------------------------------------------------------------------------------------------------
    void OpenPopupUserReference(CsHeroBase csHeroBase)
    {
        if (m_goPopupUserReference == null)
        {
            StartCoroutine(LoadPopupUserReferenceCoroutine(() => CreatePopupUserReference(csHeroBase)));
        }
        else
        {
            CreatePopupUserReference(csHeroBase);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupUserReference(CsFriend csFriend)
    {
        if (m_goPopupUserReference == null)
        {
            StartCoroutine(LoadPopupUserReferenceCoroutine(() => CreatePopupUserReference(csFriend)));
        }
        else
        {
            CreatePopupUserReference(csFriend);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //길드 팝업에서만 사용
    void OpenPopupUserReference(CsGuildMember csGuildMember)
    {
        if (m_goPopupUserReference == null)
        {
            StartCoroutine(LoadPopupUserReferenceCoroutine(() => CreatePopupUserReference(csGuildMember)));
        }
        else
        {
            CreatePopupUserReference(csGuildMember);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupUserReference(CsNationNoblesseInstance csNationNoblesseInstance)
    {
        if (m_goPopupUserReference == null)
        {
            StartCoroutine(LoadPopupUserReferenceCoroutine(() => CreatePopupUserReference(csNationNoblesseInstance)));
        }
        else
        {
            CreatePopupUserReference(csNationNoblesseInstance);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupUserReferenceCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupUserReference");
        yield return resourceRequest;
        m_goPopupUserReference = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupUserReference(CsHeroBase csHeroBase)
    {
        GameObject goUserReference = Instantiate(m_goPopupUserReference, m_trPanelPopup);
        goUserReference.name = "PopupUserReference";
        m_trUserReference = goUserReference.transform;

        m_csPopupUserReference = m_trUserReference.GetComponent<CsPopupUserReference>();
        m_csPopupUserReference.EventCloseUserReference += OnEventCloseUserReference;
        m_csPopupUserReference.UpdateReference(csHeroBase);

        if (!m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupUserReference(CsFriend csFriend)
    {
        GameObject goUserReference = Instantiate(m_goPopupUserReference, m_trPanelPopup);
        goUserReference.name = "PopupUserReference";
        m_trUserReference = goUserReference.transform;

        m_csPopupUserReference = m_trUserReference.GetComponent<CsPopupUserReference>();
        m_csPopupUserReference.EventCloseUserReference += OnEventCloseUserReference;
        m_csPopupUserReference.UpdateReference(csFriend);

        if (!m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupUserReference(CsGuildMember csGuildMember)
    {
        GameObject goUserReference = Instantiate(m_goPopupUserReference, m_trPanelPopup);
        goUserReference.name = "PopupUserReference";
        m_trUserReference = goUserReference.transform;

        m_csPopupUserReference = m_trUserReference.GetComponent<CsPopupUserReference>();
        m_csPopupUserReference.EventCloseUserReference += OnEventCloseUserReference;
        m_csPopupUserReference.UpdateReference(csGuildMember);

        if (!m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupUserReference(CsNationNoblesseInstance csNationNoblesseInstance)
    {
        GameObject goUserReference = Instantiate(m_goPopupUserReference, m_trPanelPopup);
        goUserReference.name = "PopupUserReference";
        m_trUserReference = goUserReference.transform;

        m_csPopupUserReference = m_trUserReference.GetComponent<CsPopupUserReference>();
        m_csPopupUserReference.EventCloseUserReference += OnEventCloseUserReference;
        m_csPopupUserReference.UpdateReference(csNationNoblesseInstance);

        if (!m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseUserReference()
    {
        m_csPopupUserReference.EventCloseUserReference -= OnEventCloseUserReference;

        if (m_trUserReference != null)
        {
            Destroy(m_trUserReference.gameObject);
        }

        if (m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseGuildApply()
    {
        m_csPopupGuildApply.EventCloseGuildApply -= OnEventCloseGuildApply;

        if (m_trPopupGuildApply != null)
        {
            Destroy(m_trPopupGuildApply.gameObject);
            m_trPopupGuildApply = null;
        }

        if (m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseNationWarAttend()
    {
        if (m_trPopupNationWarAttend != null)
        {
            CsPopupNationWarAttend csPopupNationWarAttend = m_trPopupNationWarAttend.GetComponent<CsPopupNationWarAttend>();
            csPopupNationWarAttend.EventCloseNationWarAttend -= OnEventCloseNationWarAttend;

            Destroy(m_trPopupNationWarAttend.gameObject);
        }

        if (m_trPanelPopup.gameObject.activeSelf)
        {
            m_trPanelPopup.gameObject.SetActive(false);
        }
    }


    #endregion 유저 조회

    #region UI 전체 끄기 / 켜기

    //---------------------------------------------------------------------------------------------------
    void ToggleMainUI(bool bIsOn)
    {
        m_canvas.GetComponent<GraphicRaycaster>().enabled = bIsOn;

        Camera camera = GameObject.Find("UICamera").GetComponent<Camera>();
        camera.enabled = bIsOn;

        if (!bIsOn)
        {
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        }
    }

    #endregion

    #region 버튼업데이트

    //---------------------------------------------------------------------------------------------------
    void UpdateMainButtons()
    {
        #region MainMenu1

        // 도달보상
		bool bChecked = false;
			
		bChecked = CsGameData.Instance.MyHeroInfo.CheckAttainment();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu1, (int)EnMenuId.Attaniment, bChecked);

        // 오늘의할일
        bChecked = CsGameData.Instance.MyHeroInfo.CheckTodayTask();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu1, (int)EnMenuId.TodayTask, bChecked);

        // Mail
        bChecked = CsGameData.Instance.MyHeroInfo.CheckNoticeMail();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu1, (int)EnMenuId.Mail, bChecked);

        #endregion MainMenu1

        bool bMenuOpenButton = false;
		bool bLeftTopMenuOpenButton = false;

        #region MainMenu2

        // Character
        bChecked =  CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet() ||
					CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet() ||
					CsGameData.Instance.MyHeroInfo.CheckOrdealQuest() ||
					CsGameData.Instance.MyHeroInfo.CheckPotionAttr() || 
                    CsCostumeManager.Instance.CheckCostumeEnchant();

		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Character, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// Skill
        bChecked = CsGameData.Instance.MyHeroInfo.CheckNoticeSkill();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Skill, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// 업적
        bChecked = CsAccomplishmentManager.Instance.CheckAccomplishmentNotice();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Achievement, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// 계급
        bChecked = CsGameData.Instance.MyHeroInfo.CheckRankReward() ||
					CsGameData.Instance.MyHeroInfo.CheckRankSkillLevelUp();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Rank, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }
		
		// 장비
        bChecked = CsGameData.Instance.MyHeroInfo.CheckNoticeMainGearEnchant();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.MainGear, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// 보조장비
        bChecked = CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearLevelUp() ||
					CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearSoulstone() ||
					CsGameData.Instance.MyHeroInfo.CheckNoticeSubGearRune();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.SubGear, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }
  
		// 탈 것
        bChecked = CsGameData.Instance.MyHeroInfo.CheckMountLevelUp() ||
					CsGameData.Instance.MyHeroInfo.CheckMountAwakeningLevelUp() ||
					CsGameData.Instance.MyHeroInfo.CheckMountPotionAttr();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Mount, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

        // 날개
        bChecked = CsGameData.Instance.MyHeroInfo.CheckWingEnchant() || CsGameData.Instance.MyHeroInfo.CheckWingInstall();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Wing, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// 컬렉션
        bChecked = CsCreatureCardManager.Instance.CheckCreatureCardCollictionNotice() ||
					CsBiographyManager.Instance.CheckBiographyNotices();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Collection, bChecked);

		if (bChecked)
        {
            bMenuOpenButton = true;
        }
		
        // 크리쳐
		bChecked = CsCreatureManager.Instance.CheckCreatureRear() || CsCreatureManager.Instance.CheckCreatureVaritaion() ||
					CsCreatureManager.Instance.CheckCreatureSwitch() || CsCreatureManager.Instance.CheckCreatureInjection();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu2, (int)EnMenuId.Creature, bChecked);

		if (bChecked)
		{
			bMenuOpenButton = true;
		}

		#endregion MainMenu2

        #region MainMenu3

		// 정령(미구현)

		// %% 별자리(미구현)


        #endregion MainMenu3

        #region MainMenu4

        // 랭킹 
        bChecked = CsGameData.Instance.MyHeroInfo.CheckRanking();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu4, (int)EnMenuId.Ranking, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

        // 친구
		bChecked = CsBlessingQuestManager.Instance.CheckProspectQuest();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu4, (int)EnMenuId.Friend, bChecked);

		if (bChecked)
		{
			bMenuOpenButton = true;
		}

        // 길드
        bChecked = CsGuildManager.Instance.CheckGuild() || CsGuildManager.Instance.CheckWeeklyObjectiveSettingEnabled();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu4, (int)EnMenuId.Guild, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

        // 국가
        bChecked = CsGameData.Instance.MyHeroInfo.CheckNation();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu4, (int)EnMenuId.Nation, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

        #endregion MainMenu4

        #region MainMenu5

        // 상점(미구현)
        bChecked = CsLuckyShopManager.Instance.CheckNoticeLuckyShop();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu5, (int)EnMenuId.LuckyShop, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

        // 지원
        bChecked = CsGameData.Instance.MyHeroInfo.CheckSupport();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu5, (int)EnMenuId.Support, bChecked);

        if (bChecked)
        {
            bMenuOpenButton = true;
        }

		// 회수
		bChecked = CsGameData.Instance.MyHeroInfo.CheckRetrieval();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu5, (int)EnMenuId.Retrieval, bChecked);

		if (bChecked)
		{
			bMenuOpenButton = true;
		}

		#endregion MainMenu5

		#region MainMenu6

		bChecked = CsGameData.Instance.MyHeroInfo.CheckVipRewardsReceivable();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu6, (int)EnMenuId.Vip, bChecked);

		if (bChecked)
		{
			bLeftTopMenuOpenButton = true;
		}

		bChecked = CsGameData.Instance.MyHeroInfo.CheckOpenGift();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu6, (int)EnMenuId.OpenGift, bChecked);

		if (bChecked)
		{
			bLeftTopMenuOpenButton = true;
		}

		bChecked = CsGameData.Instance.MyHeroInfo.CheckRookieGift();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu6, (int)EnMenuId.RookieGift, bChecked);

		if (bChecked)
		{
			bLeftTopMenuOpenButton = true;
		}

        bChecked = CsGameData.Instance.MyHeroInfo.CheckOpen7DayEvent();
        CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu6, (int)EnMenuId.Open7Day, bChecked);

		if (bChecked)
		{
			bLeftTopMenuOpenButton = true;
		}

		bChecked = CsGameData.Instance.MyHeroInfo.CheckChargingEvent();
		CsGameEventUIToUI.Instance.OnEventMainButtonUpdate((int)EnMenuGroup.MainMenu6, (int)EnMenuId.ChargingEvent, bChecked);

		if (bChecked)
		{
			bLeftTopMenuOpenButton = true;
		}

		#endregion MainMenu6

		CsGameEventUIToUI.Instance.OnEventMenuOpenButtonNoticeUpdate(bMenuOpenButton);
		CsGameEventUIToUI.Instance.OnEventLeftTopMenuOpenButtonNoticeUpdate(bLeftTopMenuOpenButton);
	}

	#endregion 버튼업데이트

	#region 카드획득팝업

	IEnumerator DisplayHeroCreatureCardCoroutine()
	{
		while (true)
		{
			yield return new WaitUntil(() => m_listCsHeroCreatureCardList.Count > 0);

			OpenPopupHeroCreatureCard(m_listCsHeroCreatureCardList[0]);
			yield return new WaitForSeconds(2.1f);

			m_listCsHeroCreatureCardList.RemoveAt(0);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopupHeroCreatureCard(CsHeroCreatureCard csHeroCreatureCard, bool bIsDungeon = false)
	{
        if (m_goPopupGetCard == null)
        {
            StartCoroutine(LoadPopupGetCardCoroutine(() => InitializePopupGetCard(csHeroCreatureCard, bIsDungeon)));
        }
        else
        {
			InitializePopupGetCard(csHeroCreatureCard, bIsDungeon);
        }
    }


    //---------------------------------------------------------------------------------------------------
    void InitializePopupGetCard(CsHeroCreatureCard csHeroCreatureCard, bool bIsDungeon)
    {
		if (!bIsDungeon)
			CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

		Transform trPopupGetCard = Instantiate(m_goPopupGetCard, m_trPopupList).transform;
        trPopupGetCard.name = "PopupGetCard";

        Transform trBack = trPopupGetCard.Find("ImageBackground");

        Text textPopupName = trBack.Find("ImageName/Text").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("A86_TXT_00001");
        CsUIData.Instance.SetFont(textPopupName);

        Transform trCard = trBack.Find("CreatureCard");

        Image imageCard = trCard.Find("ImageCard").GetComponent<Image>();
        imageCard.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Card/card_" + csHeroCreatureCard.CreatureCard.CreatureCardId);

        Image imageFrm = trCard.Find("ImageFrm").GetComponent<Image>();
        imageFrm.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/frm_card_rank_" + csHeroCreatureCard.CreatureCard.CreatureCardGrade.Grade);

        Image imageIcon = trCard.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/ico_Card_creature_" + csHeroCreatureCard.CreatureCard.CreatureCardCategory.CategoryId);

        Text textHp = trCard.Find("TextHp").GetComponent<Text>();
        textHp.text = csHeroCreatureCard.CreatureCard.Life.ToString("#,##0");
        CsUIData.Instance.SetFont(textHp);

        Text textAttack = trCard.Find("TextAttack").GetComponent<Text>();
        textAttack.text = csHeroCreatureCard.CreatureCard.Attack.ToString("#,##0");
        CsUIData.Instance.SetFont(textAttack);

        Text textCardName = trCard.Find("TextName").GetComponent<Text>();
        textCardName.text = csHeroCreatureCard.CreatureCard.Name;
        CsUIData.Instance.SetFont(textCardName);

        Text textDescription = trCard.Find("TextDescription").GetComponent<Text>();
        textDescription.text = csHeroCreatureCard.CreatureCard.Description;
        CsUIData.Instance.SetFont(textDescription);

        Text textCount = trCard.Find("TextCount").GetComponent<Text>();
        textCount.text = csHeroCreatureCard.Count.ToString("#,##0");
        CsUIData.Instance.SetFont(textCount);

        Destroy(trPopupGetCard.gameObject, 2f);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupGetCardCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGetCard");
        yield return resourceRequest;
        m_goPopupGetCard = (GameObject)resourceRequest.asset;

        unityAction();
    }

    #endregion 카드획득팝업

    #region 재접속

    //---------------------------------------------------------------------------------------------------
    void OnEventDisconnected()
	{
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00002"), CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), ConnectGameServerServer, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), OnCancelConnect, true);
	}

	//---------------------------------------------------------------------------------------------------
	void ConnectGameServerServer()
	{
		CsRplzSession.Instance.Init(CsConfiguration.Instance.GameServerSelected.ServerAddress, "ProxyServer");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConnected()
	{
		// 로그인.
		CsCommandEventManager.Instance.SendLogin();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroLogin()
	{
		LoadInGameScene();
	}

	//---------------------------------------------------------------------------------------------------
	void OnCancelConnect()
	{
		CsUIData.Instance.IntroShortCutType = EnIntroShortCutType.LogOut;
		SceneManager.LoadScene(0);
	}

	#endregion 재접속

	#region 로깅

	//---------------------------------------------------------------------------------------------------
	void SendStatusLogging(int nPing, float fFps)
	{
		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			StatusLoggingNACommand cmd = new StatusLoggingNACommand();
			cmd.ping = nPing.ToString();
			cmd.frameRate = fFps.ToString();
			cmd.UserId = CsConfiguration.Instance.User.UserId;
			cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
			cmd.Finished += ResponseStatusLogging;
			cmd.Run();
		}
	}

	void ResponseStatusLogging(object sender, EventArgs e)
	{
		StatusLoggingNACommand cmd = (StatusLoggingNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		StatusLoggingNAResponse res = (StatusLoggingNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseStatusLogging OK");
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendNetworkStatus()
	{
		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			NetworkStatusNACommand cmd = new NetworkStatusNACommand();
			cmd.Finished += ResponseNetworkStatus;
			cmd.Run();
		}
	}

	void ResponseNetworkStatus(object sender, EventArgs e)
	{
		NetworkStatusNACommand cmd = (NetworkStatusNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		NetworkStatusNAResponse res = (NetworkStatusNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseNetworkStatus OK");
			CsGameEventUIToUI.Instance.OnEventNetworkStatus(res.NetworkType, res.SignalStrength);
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendBatteryStatus()
	{
		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			BatteryStatusNACommand cmd = new BatteryStatusNACommand();
			cmd.Finished += ResponseBatteryStatus;
			cmd.Run();
		}
	}

	void ResponseBatteryStatus(object sender, EventArgs e)
	{
		BatteryStatusNACommand cmd = (BatteryStatusNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		BatteryStatusNAResponse res = (BatteryStatusNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseBatteryStatus OK");
			CsGameEventUIToUI.Instance.OnEventBatteryStatus(res.BatteryStatus, res.ChargeType);
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}


	#endregion 로깅

	#region 몬스터 테이밍 버튼
	//---------------------------------------------------------------------------------------------------
	RectTransform OnEventCreateTameMonster()
    {
        return CreateMonsterTameHUD();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDeleteTameMonster()
    {
        DestroyMonsterTameHUD();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealStart()
    {
        DisplayMonsterTameHUD(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealCancel()
    {
        DisplayMonsterTameHUD(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealCancelByIngame()
    {
        DisplayMonsterTameHUD(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGroggyMonsterItemStealFinished(CsItem csItem, bool bOwned, int nCount)
    {
        DestroyMonsterTameHUD();
        OpenPopupTamingMonster(csItem);
    }

	//----------------------------------------------------------------------------------------------------
	public void OnEventStartOrdealQuestMission(CsOrdealQuestMission csOrdealQuestMission)
	{
		if ((EnOrdealQuestMissionType)csOrdealQuestMission.Type != EnOrdealQuestMissionType.BuyStamina)
		{
			CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		}

		switch ((EnOrdealQuestMissionType)csOrdealQuestMission.Type)
		{
			case EnOrdealQuestMissionType.SoulStoneLevel:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.SubGear))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.SubGear, EnSubMenu.SubGearSoulstone));
				}
				break;

			case EnOrdealQuestMissionType.BuyStamina:
				{
					OpenPopupBuyStamina();
					return;
				}

			case EnOrdealQuestMissionType.MountLevel:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Mount))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Mount, EnSubMenu.MountLevelUp));
				}
				break;

			case EnOrdealQuestMissionType.CreatureLevel:
				{
					CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Creature, EnSubMenu.CreatureTraining);
				}
				break;

			case EnOrdealQuestMissionType.WingLevel:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Wing))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Wing, EnSubMenu.WingEnchant));
				}
				break;

			case EnOrdealQuestMissionType.BountyHunter:
				{
					if (CsGameData.Instance.MyHeroInfo.GetItemCountByItemType(13) > 0)
					{
						var listCsItem = from item in CsGameData.Instance.ItemList
										 where item.ItemType.ItemType == 13 &&
											   item.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
											   CsGameData.Instance.MyHeroInfo.Level <= item.RequiredMaxHeroLevel
										 orderby item.Grade descending
										 select item;

						foreach (var csItem in listCsItem)
						{
							CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csItem.ItemId);

							if (csInventorySlot == null)
								continue;

							if (csInventorySlot.InventoryObjectItem.Count > 0)
							{
								StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Character, EnSubMenu.Inventory, csInventorySlot));
								break;
							}
						}
					}
					else
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A45_TXT_02003"));
						return;
					}
				}
				break;

			case EnOrdealQuestMissionType.Monster:
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A120_TXT_03001"));
				}
				break;

			case EnOrdealQuestMissionType.LegendBountyHunter:
				{
					CsItem csItem = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.BountyHunter &&
																				item.Grade == 5 &&
																				item.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
																				CsGameData.Instance.MyHeroInfo.Level <= item.RequiredMaxHeroLevel);

					if (csItem != null)
					{
						CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csItem.ItemId);

						if (csInventorySlot != null && csInventorySlot.InventoryObjectItem.Count > 0)
						{
							StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Character, EnSubMenu.Inventory, csInventorySlot));
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A120_TXT_03002"));
							return;
						}
					}
				}
				break;

			case EnOrdealQuestMissionType.LegendFishingBait:
				{
					CsItem csItem = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.FishingBait &&
																				item.Grade == 5 &&
																				item.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
																				CsGameData.Instance.MyHeroInfo.Level <= item.RequiredMaxHeroLevel);

					if (csItem != null)
					{
						CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csItem.ItemId);

						if (csInventorySlot != null && csInventorySlot.InventoryObjectItem.Count > 0)
						{
							StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Character, EnSubMenu.Inventory, csInventorySlot));
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A120_TXT_03003"));
							return;
						}
					}
				}
				break;

			case EnOrdealQuestMissionType.LegendSupplySupport:
				{
					CsItem csItem = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.SupplySupport &&
																				item.Grade == 5 &&
																				item.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
																				CsGameData.Instance.MyHeroInfo.Level <= item.RequiredMaxHeroLevel);

					if (csItem != null)
					{
						CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByItemId(csItem.ItemId);

						if (csInventorySlot != null && csInventorySlot.InventoryObjectItem.Count > 0)
						{
							StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Character, EnSubMenu.Inventory, csInventorySlot));
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A120_TXT_03004"));
							return;
						}
					}
				}
				break;

			case EnOrdealQuestMissionType.RuinsReclaim:
				{
					if (CsGameData.Instance.RuinsReclaim.RequiredHeroLevel < CsGameData.Instance.MyHeroInfo.Level)
					{
						StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon));
						StartCoroutine(DungeonShortCut(EnDungeon.RuinsReclaim));
					}
				}
				break;

			case EnOrdealQuestMissionType.WarMemory:
				{
					if (CsGameData.Instance.WarMemory.RequiredHeroLevel < CsGameData.Instance.MyHeroInfo.Level)
					{
						StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon));
						StartCoroutine(DungeonShortCut(EnDungeon.WarMemory));
					}
				}
				break;

			case EnOrdealQuestMissionType.DemensionRaid:
				{
					CsNpcInfo csNpcInfo = CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo;

					CsGameEventToIngame.Instance.OnEventMapMove(csNpcInfo.ContinentId, CsGameData.Instance.MyHeroInfo.Nation.NationId, csNpcInfo.Position);
				}
				break;

			case EnOrdealQuestMissionType.HolyWar:
				{
					CsNpcInfo csNpcInfo = CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo;

					CsGameEventToIngame.Instance.OnEventMapMove(csNpcInfo.ContinentId, CsGameData.Instance.MyHeroInfo.Nation.NationId, csNpcInfo.Position);
				}
				break;

			case EnOrdealQuestMissionType.NationWar:
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A120_TXT_03005"));
				}
				break;

			case EnOrdealQuestMissionType.LegendSecretLetter:
				{
					CsNpcInfo csNpcInfo = CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo;

					CsGameEventToIngame.Instance.OnEventMapMove(csNpcInfo.ContinentId, CsGameData.Instance.MyHeroInfo.Nation.NationId, csNpcInfo.Position);
				}
				break;

			case EnOrdealQuestMissionType.ExploitPoint:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Rank))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Class, EnSubMenu.Class));
				}
				break;

			case EnOrdealQuestMissionType.Rank:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Rank))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Class, EnSubMenu.Class));
				}
				break;

			case EnOrdealQuestMissionType.AchievePoint:
				{
					if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Achievement))
					{
						return;
					}

					StartCoroutine(OpenPopupAfterPopupClose(EnMainMenu.Achievement, EnSubMenu.Accomplishment));
				}
				break;
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopupTamingMonster(CsItem csItem)
    {
        if (m_goPopupTamingMonster == null)
        {
            StartCoroutine(LoadPopupTamingMonster(csItem));
        }
        else
        {
            CreatePopupTamingMonster(csItem);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupTamingMonster(CsItem csItem)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupTamingMonster");
        yield return resourceRequest;
        m_goPopupTamingMonster = (GameObject)resourceRequest.asset;

        CreatePopupTamingMonster(csItem);
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupTamingMonster(CsItem csItem)
    {
        Transform trPopupTamingMonster = m_trPopup.Find("PopupTamingMonster");

        if (trPopupTamingMonster == null)
        {
            trPopupTamingMonster = Instantiate(m_goPopupTamingMonster, m_trPopup).transform;
            trPopupTamingMonster.name = "PopupTamingMonster";
        }

        Transform trImageBackground = trPopupTamingMonster.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A17_TXT_04004");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopupTamingMonster);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trItemSlot = trImageBackground.Find("ItemSlot");

        Image imageFrameRank = trItemSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank0" + csItem.Grade);

        Image imageIcon = trItemSlot.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);

        Text textItemName = trImageBackground.Find("TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textItemName);
        textItemName.text = csItem.Name;

        Text textDescription = trImageBackground.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = csItem.Description;
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupTamingMonster()
    {
        Transform trPopupTamingMonster = m_trPopup.Find("PopupTamingMonster");

        if (trPopupTamingMonster == null)
        {
            return;
        }
        else
        {
            Destroy(trPopupTamingMonster.gameObject);
            trPopupTamingMonster = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    RectTransform CreateMonsterTameHUD()
    {
        Transform trMonsterTameHUD = m_trPopup.Find("MonsterTame");

        if (trMonsterTameHUD == null)
        {
            trMonsterTameHUD = Instantiate((GameObject)CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/MonsterTameHUD").asset, m_trPopup).transform;
            trMonsterTameHUD.name = "MonsterTame";
            //trMonsterTameHUD.position = new Vector3(0, 0, 0);
            trMonsterTameHUD.localScale = new Vector3(1, 1, 1);
            trMonsterTameHUD.localEulerAngles = new Vector3(0, 0, 0);
        }

        Button buttonTaming = trMonsterTameHUD.Find("ButtonTaming").GetComponent<Button>();
        buttonTaming.onClick.RemoveAllListeners();
        buttonTaming.onClick.AddListener(OnClickMonsterTaming);
        buttonTaming.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonMount = trMonsterTameHUD.Find("ButtonMount").GetComponent<Button>();
        buttonMount.onClick.RemoveAllListeners();
        buttonMount.onClick.AddListener(OnClickMonsterMount);
        buttonMount.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon)
        {
            buttonMount.gameObject.SetActive(true);
        }
        else
        {
            buttonMount.gameObject.SetActive(false);
        }

        Text textTaming = buttonTaming.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTaming);
        textTaming.text = CsConfiguration.Instance.GetString("A17_TXT_04002");

        Text textMount = buttonMount.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMount);
        textMount.text = CsConfiguration.Instance.GetString("A17_TXT_04001");

        return trMonsterTameHUD.GetComponent<RectTransform>();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMonsterTameHUD(bool bDisplay)
    {
        Transform trMonsterTameHUD = m_trPopup.Find("MonsterTame");

        if (trMonsterTameHUD == null)
        {
            return;
        }
        else
        {
            trMonsterTameHUD.gameObject.SetActive(bDisplay);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DestroyMonsterTameHUD()
    {
        Transform trMonsterTameHUD = m_trPopup.Find("MonsterTame");

        if (trMonsterTameHUD == null)
        {
            return;
        }
        else
        {
            Destroy(trMonsterTameHUD.gameObject);
            trMonsterTameHUD = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMonsterTaming()
    {
        CsGameEventToIngame.Instance.OnEventGroggyMonsterItemStealStart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMonsterMount()
    {
        CsDungeonManager.Instance.StoryDungeonHeroStartTame();
    }
    #endregion 몬스터 테이밍 버튼

	#region PopupOrdealQuest
	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupOrdealQuest()
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupOrdealQuest/PopupOrdealQuest");
		yield return resourceRequest;

		m_goPopupOrdeal = (GameObject)resourceRequest.asset;

		m_trPopupOrdeal = Instantiate(m_goPopupOrdeal, m_trPopupList).transform;
		m_trPopupOrdeal.name = "PopupOrdealQuest";
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupBuyStamina()
	{
		int nStaminBuyCount = CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount + 1;
		CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);

		if (csStaminaBuyCount == null)
		{
			nStaminBuyCount = CsGameData.Instance.StaminaBuyCountList.Count;
			csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminBuyCount);
		}

		//스테미너 충전 확인창
		string strDes = string.Format(CsConfiguration.Instance.GetString("A13_TXT_03001"), csStaminaBuyCount.RequiredDia, csStaminaBuyCount.Stamina);

		CsGameEventUIToUI.Instance.OnEventConfirm(strDes, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => OnClickChargeStaminaOK(nStaminBuyCount), CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickChargeStaminaOK(int nStaminaBuyCount)
	{
		CsStaminaBuyCount csStaminaBuyCount = CsGameData.Instance.GetStaminaBuyCount(nStaminaBuyCount);

		if (csStaminaBuyCount.RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
		{
			CsCommandEventManager.Instance.SendStaminaBuy();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 인벤토리 로딩 후 아이템 사용 팝업 표시
	IEnumerator UseItemAfterLoadingInventory(CsInventorySlot csInventorySlot)
	{
		yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory") != null);
		
		Transform trInventorySubMenu = m_trCanvas2.Find("MainPopupSubMenu/SubMenu2/Inventory");

		CsInventory csInventory = trInventorySubMenu.GetComponent<CsInventory>();

		if (csInventory != null)
		{
			int nListIndex = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindIndex(slot => slot.Index == csInventorySlot.Index);
			csInventory.QuickOpenPopupItemInfo(nListIndex);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DungeonShortCut(EnDungeon enDungeon)
	{
		yield return new WaitUntil(() => m_trSubMenu1.Find("PopupDungeonCategory") != null);

		Transform trCategory = m_trSubMenu1.Find("PopupDungeonCategory");
		int nDungeonIndex = 0;

		switch (enDungeon)
		{
			case EnDungeon.StoryDungeon:
				nDungeonIndex = (CsDungeonManager.Instance.StoryDungeonNo - 1);
				break;

			case EnDungeon.FearAltar:
				nDungeonIndex = (int)EnPartyDungeonType.FearAltar;
				break;

			case EnDungeon.AncientRelic:
				nDungeonIndex = (int)EnPartyDungeonType.AncientRelic;
				break;

			case EnDungeon.SoulCoveter:
				nDungeonIndex = (int)EnPartyDungeonType.SoulCoveter;
				break;

			case EnDungeon.ArtifactRoom:
				nDungeonIndex = (int)EnIndividualDungeonType.ArtifactRoom;
				break;

			case EnDungeon.ExpDungeon:
				nDungeonIndex = (int)EnIndividualDungeonType.Exp;
				break;

			case EnDungeon.GoldDungeon:
				nDungeonIndex = (int)EnIndividualDungeonType.Gold;
				break;

			case EnDungeon.FieldOfHonor:
				nDungeonIndex = (int)EnIndividualDungeonType.FieldOfHonor;
				break;

			case EnDungeon.ProofOfValor:
				nDungeonIndex = (int)EnIndividualDungeonType.ProofOfValor;
				break;

			case EnDungeon.WisdomTemple:
				nDungeonIndex = (int)EnIndividualDungeonType.WisdomTemple;
				break;

			case EnDungeon.UndergroundMaze:
				nDungeonIndex = (int)EnTimeLimitDungeonType.UndergroundMaze;
				break;

			case EnDungeon.RuinsReclaim:
				nDungeonIndex = (int)EnTimeLimitDungeonType.RuinsReclaim;
				break;

			case EnDungeon.InfiniteWar:
				nDungeonIndex = (int)EnTimeLimitDungeonType.InfiniteWar;
				break;

			case EnDungeon.WarMemory:
				nDungeonIndex = (int)EnTimeLimitDungeonType.WarMemory;
				break;
		}

		if (trCategory != null)
		{
			CsDungeonCartegory csDungeonCartegory = trCategory.GetComponent<CsDungeonCartegory>();
			csDungeonCartegory.ShortCutDungeonInfo(nDungeonIndex);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator OpenPopupAfterPopupClose(EnMainMenu enMainMenu, EnSubMenu enSubMenu, CsInventorySlot csInventorySlot = null)
	{
		yield return new WaitUntil(() => m_trCanvas1.Find("PopupCharacter") == null);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(enMainMenu, enSubMenu);

		if (csInventorySlot != null)
		{
			StartCoroutine(UseItemAfterLoadingInventory(csInventorySlot));
		}
	}
	#endregion PopupOrdealQuest

    #region 동시 접속
    
    //---------------------------------------------------------------------------------------------------
    void OnEventAccountLoginDuplicated()
    {
        m_csPanelModal.Choice(CsConfiguration.Instance.GetString("PUBLIC_DUALCONNECT"), OnCancelConnect, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
    }

    #endregion 동시 접속

    #region 친구

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationReceived()
    {
        Debug.Log("#@#@ OnEventFriendApplicationReceived #@#@");
        CsFriendApplication csFriendApplication = CsFriendManager.Instance.FriendApplicationReceivedList.Last();

        if (CsFriendManager.Instance.FriendList.Find(a => a.Id == csFriendApplication.ApplicationId) == null)
        {
            if (CsFriendManager.Instance.FriendApplicationReceivedList.Count <= 1)
            {
                OpenFriendApplication();
            }
            else
            {
                return;
            }
        }
        else
        {
            CsFriendManager.Instance.SendFriendApplicationAccept(csFriendApplication.No);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccepted()
    {
        // 상대가 친구 신청 수락
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02004"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationRefused()
    {
        // 상대가 친구 신청 거절
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02008"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationAccept()
    {
        // 친구 신청 수락
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02004"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFriendApplicationRefuse()
    {

    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBlacklistEntryAdd()
    {
        if (0 < CsFriendManager.Instance.BlacklistEntryList.Count)
        {
            CsBlacklistEntry csBlacklistEntry = CsFriendManager.Instance.BlacklistEntryList.Last();
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_02009"), csBlacklistEntry.Name));
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBlacklistEntryDelete()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02010"));
    }
    
    //---------------------------------------------------------------------------------------------------
    void OnClickFriendApplicationAccept(CsFriendApplication csFriendApplication)
    {
        FriendApplicationRemove();
        CsFriendManager.Instance.SendFriendApplicationAccept(csFriendApplication.No);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickFriendApplicationRefuse(CsFriendApplication csFriendApplication)
    {
        FriendApplicationRemove();
        CsFriendManager.Instance.SendFriendApplicationRefuse(csFriendApplication.No);
    }

    //---------------------------------------------------------------------------------------------------
    void FriendApplicationRemove()
    {
        CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.FriendModal);

        if (0 < CsFriendManager.Instance.FriendApplicationReceivedList.Count - 1)
        {
            OpenFriendApplication();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventTimerModalClose(EnTimerModalType.FriendModal);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenFriendApplication()
    {
        CsFriendApplication csFriendApplication = CsFriendManager.Instance.FriendApplicationReceivedList.Last();

        CsGameEventUIToUI.Instance.OnEventTimerConfirm(EnTimerModalType.FriendModal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_01023"), csFriendApplication.ApplicationName),
                () => OnClickFriendApplicationAccept(csFriendApplication),
                () => OnClickFriendApplicationRefuse(csFriendApplication));
    }

    #endregion 친구

	#region 아이템 정보창
	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupItemInfo(CsItem csitem)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
		yield return resourceRequest;
		m_goPopupItemInfo = (GameObject)resourceRequest.asset;

		OpenPopupItemInfo(csitem);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo(CsItem csitem)
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += ClosePopupItemInfo;
		CsGameEventUIToUI.Instance.EventCloseAllPopup += ClosePopupItemInfo;

		m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, false, 0, false);
	}

	//---------------------------------------------------------------------------------------------------
	void ClosePopupItemInfo()
	{
		m_csPopupItemInfo.EventClosePopupItemInfo -= ClosePopupItemInfo;
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= ClosePopupItemInfo;

		Destroy(m_trItemInfo.gameObject);
		m_csPopupItemInfo = null;
		m_trItemInfo = null;
	}

	//---------------------------------------------------------------------------------------------------
	void ClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
	{
		m_csPopupItemInfo.EventClosePopupItemInfo -= ClosePopupItemInfo;
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= ClosePopupItemInfo;

		Destroy(m_trItemInfo.gameObject);
		m_csPopupItemInfo = null;
		m_trItemInfo = null;
	}
	#endregion 아이템 정보창

    #region 절전 모드 리셋

    //---------------------------------------------------------------------------------------------------
    void OnEventSleepModeReset()
    {
        m_flSleepModeTimer = Time.realtimeSinceStartup + CsGameConfig.Instance.OptimizationModeWaitingTime;

        if (m_bSleepMode)
        {
            m_bSleepMode = false;
            SleepMode();
            CsGameEventToIngame.Instance.OnEventSleepMode(m_bSleepMode);
        }
        else
        {
            return;
        }
    }
    
    #endregion 절전 모드 리셋

    #region 안전모드 검사
    
    //---------------------------------------------------------------------------------------------------
    void OnEventAutoBattleStart(EnBattleMode enBattleMode)
    {
        m_enBattleMode = enBattleMode;
    }

    #endregion 안전모드 검사

	#region 크리쳐카드 오픈 애니메이션 체크
	//---------------------------------------------------------------------------------------------------
	void OnEventCheckCreatureCardAnimation()
	{
		StartCoroutine(CheckCreatureCardAnimationFinished());
	}

	IEnumerator CheckCreatureCardAnimationFinished()
	{
		yield return new WaitUntil(() => m_listCsHeroCreatureCardList.Count <= 0);

		CsOpenToastManager.Instance.CreatureCardAnimationFinished();
	}

	//---------------------------------------------------------------------------------------------------

	#endregion 크리쳐카드 오픈 애니메이션 체크

    #region 증정

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroPresent(Guid guidSenderId, string strSenderName, int nSenderNationId, Guid guidTargetId, string strTargetName, int nTargetNationId, CsPresent csPresent)
    {
        if (!csPresent.IsMessageSend)
        {
            return;
        }
        else
        {
            CsNation csNationSender = CsGameData.Instance.GetNation(nSenderNationId);
            CsNation csNationTarget = CsGameData.Instance.GetNation(nTargetNationId);

            if (csNationSender == null || csNationTarget == null)
            {
                return;
            }
            else
            {
                string strMessage = string.Format(CsConfiguration.Instance.GetString(csPresent.MessageText), csNationSender.Name, strSenderName, csNationTarget.Name, strTargetName);
                CsGameEventUIToUI.Instance.OnEventToastSystem(strMessage);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentReplyReceived(Guid guidSenderId, string strSenderName, int nSenderNationId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString(""), strSenderName));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentSend(int nPresentId)
    {
        CsPresent csPresent = CsGameData.Instance.GetPresent(nPresentId);

        if (csPresent == null)
        {
            return;
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_06001"), csPresent.RequiredDia));
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_06002"));
        }
    }

    #endregion 증정

	#region 크리처

	//---------------------------------------------------------------------------------------------------
	// 크리처 획득
	void OnEventGetHeroCreature(List<CsHeroCreature> heroCreatureList)
	{
		if (heroCreatureList.Count > 0)
		{
			StartCoroutine(LoadPopupGetCreature(heroCreatureList));
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupGetCreature(List<CsHeroCreature> heroCreatureList)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGetCreature");
		yield return resourceRequest;
		GameObject goPopupGetCreature = (GameObject)resourceRequest.asset;

		OpenPopupGetCreature(goPopupGetCreature, heroCreatureList);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupGetCreature(GameObject goPopupCreature, List<CsHeroCreature> heroCreatureList)
	{
		Transform trPopupGetCreature = Instantiate(goPopupCreature, m_trPopupList).transform;
		trPopupGetCreature.name = "PopupGetCreature";

		CsPopupGetCreature csPopupGetCreature = trPopupGetCreature.GetComponent<CsPopupGetCreature>();
		csPopupGetCreature.DisplayCreatures(heroCreatureList);
	}

	#endregion 크리처

    #region 길드 축복

    bool m_bLoadPopupGuildBlessing = false;
    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupGuildBlessing()
    {
        if (!m_bLoadPopupGuildBlessing)
        {
            m_bLoadPopupGuildBlessing = true;
            StartCoroutine(LoadPopupGuildBlessing());
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupGuildBlessing()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGuildBlessing");
        yield return resourceRequest;

        Transform trPopupGuildBlessing = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        trPopupGuildBlessing.name = "PopupGuildBlessing";
        m_bLoadPopupGuildBlessing = false;
    }

    #endregion 길드 축복

    #region NationAlliance

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccept(CsNationAlliance csNationAlliance)
    {
        int nNationAllianceId = 0;
        bool bMyNationAllianceAccept = false;

        for (int i = 0; i < csNationAlliance.Nations.Length; i++)
        {
            if (csNationAlliance.Nations[i] == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                bMyNationAllianceAccept = true;
            }
            else
            {
                nNationAllianceId = csNationAlliance.Nations[i];
            }
        }

        if (bMyNationAllianceAccept)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A156_TXT_00012"), CsGameData.Instance.GetNation(nNationAllianceId).Name));   
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceConcluded(CsNationAlliance csNationAlliance)
    {
        int nNationAllianceId = 0;
        bool bMyNationAllianceAccept = false;

        for (int i = 0; i < csNationAlliance.Nations.Length; i++)
        {
            if (csNationAlliance.Nations[i] == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                bMyNationAllianceAccept = true;
            }
            else
            {
                nNationAllianceId = csNationAlliance.Nations[i];
            }
        }

        if (bMyNationAllianceAccept)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A156_TXT_00012"), CsGameData.Instance.GetNation(nNationAllianceId).Name));   
        }
        else
        {
            return;
        }
    }

    #endregion NationAlliance


    #region PopupPotionAttr

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupMountAttrPotion(int nMountId)
    {
        if (m_goPopupMountPotionAttr == null)
        {
            StartCoroutine(LoadPopupMountPotionAttr(nMountId));
        }
        else
        {
            OpenPopupMountPotionAttr(nMountId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupMountPotionAttr(int nMountId)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupMount/PopupMountAttrPotion");
        yield return resourceRequest;
        m_goPopupMountPotionAttr = (GameObject)resourceRequest.asset;

        OpenPopupMountPotionAttr(nMountId);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupMountPotionAttr(int nMountId)
    {
        m_trPopupMountPotionAttr = Instantiate(m_goPopupMountPotionAttr, m_trPopupList).transform;
        m_trPopupMountPotionAttr.name = "PopupAttrPotion";

        CsPopupMountPotionAttr csPopupMountPotionAttr = m_trPopupMountPotionAttr.GetComponent<CsPopupMountPotionAttr>();
        csPopupMountPotionAttr.EventClosePopupMountAttrPotion += OnEventClosePopupMountAttrPotion;
        csPopupMountPotionAttr.DisplayPopupMountAttrPotion(nMountId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupMountAttrPotion()
    {
        CsPopupMountPotionAttr csPopupMountPotionAttr = m_trPopupMountPotionAttr.GetComponent<CsPopupMountPotionAttr>();
        csPopupMountPotionAttr.EventClosePopupMountAttrPotion -= OnEventClosePopupMountAttrPotion;

        Destroy(m_trPopupMountPotionAttr.gameObject);
        m_trPopupMountPotionAttr = null;
    }

    #endregion PopupPotionAttr

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupCostumeEffect()
    {
        StartCoroutine(LoadOpenPopupCostumeEffect());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadOpenPopupCostumeEffect()
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupCostume") != null);

        Transform trPopupCostume = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/PopupCostume");

        CsPopupCostume csPopupCostume = trPopupCostume.GetComponent<CsPopupCostume>();
        csPopupCostume.OpenPopupCostumeEffect();

        StopCoroutine(LoadOpenPopupCostumeEffect());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupHeroPotionAttr()
    {
        StartCoroutine(LoadOpenPopupHeroPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadOpenPopupHeroPotionAttr()
    {
        yield return new WaitUntil(() => m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/CharacterEquipment") != null);

        Transform trCharacterEquipment = m_trCanvas2.Find("MainPopupSubMenu/SubMenu1/CharacterEquipment");

        CsCharacterEquipment csCharacterEquipment = trCharacterEquipment.GetComponent<CsCharacterEquipment>();
        csCharacterEquipment.LoadPopupHeroAttrPotion();

        StopCoroutine(LoadOpenPopupHeroPotionAttr());
    }

    Dictionary<string, bool> m_dicScheduleNotice = new Dictionary<string, bool>();

    //---------------------------------------------------------------------------------------------------
    void CheckScheduleNotice(EnScheduleNotice enScheduleNotice)
    {

        CsScheduleNotice csScheduleNotice = CsGameData.Instance.GetScheduleNotice((int)enScheduleNotice);

        if (csScheduleNotice == null)
            return;

        int nCurrentSecond = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

        int nReadyTime = 0;
        int nStartTime = 0;
        int nEndTime = 0;

        switch (enScheduleNotice)
        {
            case EnScheduleNotice.DimensionInfiltration:

                nReadyTime = CsGameData.Instance.DimensionInfiltrationEvent.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = CsGameData.Instance.DimensionInfiltrationEvent.StartTime;
                nEndTime = CsGameData.Instance.DimensionInfiltrationEvent.EndTime;

                break;

            case EnScheduleNotice.BattleFieldSupport:

                nReadyTime = CsGameData.Instance.BattlefieldSupportEvent.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = CsGameData.Instance.BattlefieldSupportEvent.StartTime;
                nEndTime = CsGameData.Instance.BattlefieldSupportEvent.EndTime;

                break;

            case EnScheduleNotice.GuildFram:

                nReadyTime = CsGameData.Instance.GuildFarmQuest.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = CsGameData.Instance.GuildFarmQuest.StartTime;
                nEndTime = CsGameData.Instance.GuildFarmQuest.EndTime;

                break;

            case EnScheduleNotice.RuinsReclaim:

                CsRuinsReclaimOpenSchedule csRuinsReclaimOpenSchedule = null;

                for (int i = 0; i < CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList[i].EndTime)
                    {
                        csRuinsReclaimOpenSchedule = CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (csRuinsReclaimOpenSchedule == null)
                {
                    csRuinsReclaimOpenSchedule = CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList[0];
                }

                nReadyTime = csRuinsReclaimOpenSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csRuinsReclaimOpenSchedule.StartTime;
                nEndTime = csRuinsReclaimOpenSchedule.EndTime;

                break;

            case EnScheduleNotice.FieldBoss:

                CsFieldBossEventSchedule csFieldBossEventSchedule = null;

                for (int i = 0; i < CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[i].EndTime)
                    {
                        csFieldBossEventSchedule = CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (csFieldBossEventSchedule == null)
                {
                    csFieldBossEventSchedule = CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList[0];
                }

                nReadyTime = csFieldBossEventSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csFieldBossEventSchedule.StartTime;
                nEndTime = csFieldBossEventSchedule.EndTime;

                break;

            case EnScheduleNotice.NationWar:

                nReadyTime = CsGameData.Instance.NationWar.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = CsGameData.Instance.NationWar.StartTime;
                nEndTime = CsGameData.Instance.NationWar.EndTime;

                break;

            case EnScheduleNotice.WarMemory:

                CsWarMemorySchedule csWarMemorySchedule = null;

                for (int i = 0; i < CsGameData.Instance.WarMemory.WarMemoryScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.WarMemory.WarMemoryScheduleList[i].EndTime)
                    {
                        csWarMemorySchedule = CsGameData.Instance.WarMemory.WarMemoryScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (csWarMemorySchedule == null)
                {
                    csWarMemorySchedule = CsGameData.Instance.WarMemory.WarMemoryScheduleList[0];
                }

                nReadyTime = csWarMemorySchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csWarMemorySchedule.StartTime;
                nEndTime = csWarMemorySchedule.EndTime;

                break;

            case EnScheduleNotice.InfiniteWar:

                CsInfiniteWarOpenSchedule csInfiniteWarOpenSchedule = null;

                for (int i = 0; i < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                    {
                        csInfiniteWarOpenSchedule = CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i];
                        break;
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

                nReadyTime = csInfiniteWarOpenSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csInfiniteWarOpenSchedule.StartTime;
                nEndTime = csInfiniteWarOpenSchedule.EndTime;

                break;

            case EnScheduleNotice.AnkouTomb:

                CsAnkouTombSchedule csAnkouTombSchedule = null;

                for (int i = 0; i < CsGameData.Instance.AnkouTomb.AnkouTombScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i].EndTime)
                    {
                        csAnkouTombSchedule = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (csAnkouTombSchedule == null)
                {
                    csAnkouTombSchedule = CsGameData.Instance.AnkouTomb.AnkouTombScheduleList[0];
                }

                nReadyTime = csAnkouTombSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csAnkouTombSchedule.StartTime;
                nEndTime = csAnkouTombSchedule.EndTime;

                break;

            case EnScheduleNotice.TrandeShip:

                CsTradeShipSchedule csTradeShipSchedule = null;

                for (int i = 0; i < CsGameData.Instance.TradeShip.TradeShipScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.TradeShip.TradeShipScheduleList[i].EndTime)
                    {
                        csTradeShipSchedule = CsGameData.Instance.TradeShip.TradeShipScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (csTradeShipSchedule == null)
                {
                    csTradeShipSchedule = CsGameData.Instance.TradeShip.TradeShipScheduleList[0];
                }

                nReadyTime = csTradeShipSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csTradeShipSchedule.StartTime;
                nEndTime = csTradeShipSchedule.EndTime;

                break;

            case EnScheduleNotice.SafeTime:

                nReadyTime = CsGameData.Instance.SafeTimeEvent.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = CsGameData.Instance.SafeTimeEvent.StartTime;
                nEndTime = CsGameData.Instance.SafeTimeEvent.EndTime;

                break;

            case EnScheduleNotice.HolyWar:

                CsHolyWarQuestSchedule csHolyWarQuestSchedule = null;

                for (int i = 0; i < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Count; i++)
                {
                    if (nCurrentSecond < CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i].EndTime)
                    {
                        csHolyWarQuestSchedule = CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList[i];
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                nReadyTime = csHolyWarQuestSchedule.StartTime - csScheduleNotice.BeforeStartNoticeTime;
                nStartTime = csHolyWarQuestSchedule.StartTime;
                nEndTime = csHolyWarQuestSchedule.EndTime;

                break;
        }

        if (nReadyTime <= nCurrentSecond && nCurrentSecond < nReadyTime + 2f)
        {
            if (!m_dicScheduleNotice.ContainsKey(enScheduleNotice.ToString() + nReadyTime.ToString()))
            {
                // 아직 한번도 안들어옴
                CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.BeforeStartNotice);
                m_dicScheduleNotice.Add(enScheduleNotice.ToString() + nReadyTime.ToString(), true);
            }
            else
            {
                if (m_dicScheduleNotice[enScheduleNotice.ToString() + nReadyTime.ToString()])
                {
                    // 이미 알림

                }
                else
                {
                    // 초기화 되어 다시 알림
                    CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.BeforeStartNotice);
                }
            }
        }

        if (nStartTime <= nCurrentSecond && nCurrentSecond < nStartTime + 2f)
        {
            if (!m_dicScheduleNotice.ContainsKey(enScheduleNotice.ToString() + nStartTime.ToString()))
            {
                // 아직 한번도 안들어옴
                CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.StartNotice);
                m_dicScheduleNotice.Add(enScheduleNotice.ToString() + nStartTime.ToString(), true);
            }
            else
            {
                if (m_dicScheduleNotice[enScheduleNotice.ToString() + nStartTime.ToString()])
                {

                }
                else
                {
                    // 아직 한번도 안들어옴
                    CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.StartNotice);
                }
            }
        }

        if (nEndTime <= nCurrentSecond && nCurrentSecond < nEndTime + 2f)
        {
            if (!m_dicScheduleNotice.ContainsKey(enScheduleNotice.ToString() + nEndTime.ToString()))
            {
                CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.StartNotice);
                m_dicScheduleNotice.Add(enScheduleNotice.ToString() + nEndTime.ToString(), true);
            }
            else
            {
                if (m_dicScheduleNotice[enScheduleNotice.ToString() + nEndTime.ToString()])
                {

                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventScheduleNotice(csScheduleNotice.StartNotice);
                }
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenWebView(string strUrl)
	{
		GameObject goPopupFrameWebView = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupFrameWebView/PopupFrameWebView");

		Transform trWebView = Instantiate<GameObject>(goPopupFrameWebView, m_trCanvas3).transform;
		trWebView.name = "PopupFrameWebView";
		trWebView.SetAsLastSibling();

		CsPopupFrameWebView csPopupFrameWebView = trWebView.GetComponent<CsPopupFrameWebView>();
		csPopupFrameWebView.LoadUrl(strUrl);
	}
}