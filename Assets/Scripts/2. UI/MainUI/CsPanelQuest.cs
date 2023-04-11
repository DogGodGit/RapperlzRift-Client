using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using ClientCommon;
using UnityEngine.EventSystems;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-26)
//---------------------------------------------------------------------------------------------------

public enum EnLeftBoardTap
{
    Quest = 1,
    Party = 2,
}

public class CsPanelQuest : MonoBehaviour
{
    Transform m_trPopupNpcDialog;
    Transform m_trPopupMainQuest;
    Transform m_trPopupList;
    Transform m_trPopup;
    Transform m_trItemInfo;
    Transform m_trQuestPartyPanel;
    Transform m_trMainQuestToggle;
    Transform m_trPartyMemberList;
    Transform m_trToggleDungeonQuest;
    Transform m_trSubMenu1;
    Transform m_trPanelInfiniteWar;
    Transform m_trPanelDungeon;
    Transform m_trPanelOsirisRoom;
    Transform m_trPopupGuildBlessing;

    GameObject m_goPopupItemInfo;
    GameObject m_goPopupNationTransmission;

    Text m_textQuestName;
    Text m_textNpcName;
    Text m_textNpcScript;
    Text m_textExp;
    Text m_textGold;
    Text m_textExploit;
    Text m_textAccept;
    Text m_textPartyCall;
    Text m_textDialogNpcName;
    Text m_textDialogNpcScript;
    Text m_textGuildMoralPoint;
    Text m_textGuildBuildingPoint;
    Text m_textGuildFund;
	Text m_textTimeTrueHeroQuest;
	Text m_textTimeCreatureFarmQuest;
    Text m_textTimeJobChangeQuest;

    Button m_buttonAccept;
    Button m_buttonCancel;
    Button m_buttonPartyCall;
    Button m_buttonAutoCancel;

    bool m_bIsFirstQuestPopup = true;
    bool m_bIsFirstNpcDialogPopup = true;
    bool m_bIsFirstInDungeon = true;
    bool m_bIsAlter = false;
    bool m_bPanelHide = false;
    bool m_bDisplayArrow = false;
    bool m_bIsMainQuestAcceptDialog = false;
	bool m_bSelectedMainQuestPanel = false;
    bool m_bStartJobChangeQuest = false;

	int m_nSelectedSubQuest = 0;
	int m_nSelectedSubQuestId = 0; // m_nSelectedSubQuest 변수가 0보다 클 경우에만 사용
	int m_nNpcId = 0;
    int m_nStoryDungeonNo = 0;
	
    float m_flTime = 0;
    float m_flExpWaveRemainingTime = 0;
    float m_flThreatOfFarmMissionRemainingTime;
    float m_flGuildMissionRemainingTime = 0.0f;

    long m_lExpDungeonPoint = 0;

	// 지혜의신전
	int m_nPuzzleCount = 0; // 색맞추기 점수, 상자찾기 남은횟수
	int m_nQuizNo = 0;

    EnLeftBoardTap m_enLeftBoardTapNow = EnLeftBoardTap.Quest;

    CsPopupItemInfo m_csPopupItemInfo;

    IEnumerator m_IEnumeratorDungeonGuide;
    IEnumerator m_iEnumeratorDungeonReadyTime;
	
    GameObject m_goQuestPanel;
    List<Transform> m_listQuestPanel = new List<Transform>();

    bool m_bIsLoadQuestPanel = false;
    bool m_bIsInDungeon = false;
    bool m_bIsInGuildIsland = false;

    int m_nAutoDailyQuestSlotIndex = -1;
    int m_nAnkouTombWaveNo = 0;

    CsUiAnimation m_csMainQuestAni;
	
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trSubMenu1 = trCanvas2.Find("MainPopupSubMenu/SubMenu1");
        m_trPopup = GameObject.Find("Canvas/Popup").transform;
		m_trQuestPartyPanel = transform.Find("QuestPartyPanel");

        InitializePanelMainQuest();

        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventQuestCompltedError += OnEventErrorClear;

		CsGameEventUIToUI.Instance.EventReturnScrollUseStart += OnEventReturnScrollUseStart;

        // 메인 퀘스트
		CsMainQuestManager.Instance.EventAcceptDialog += OnEventAcceptDialog;
        CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;
        CsMainQuestManager.Instance.EventExecuteDataUpdated += OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventCompleteDialog += OnEventCompleteDialog;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        CsMainQuestManager.Instance.EventStopAutoPlay += OnEventStopAutoPlay;
        CsMainQuestManager.Instance.EventStartAutoPlay += OnEventStartAutoPlay;

		CsGameEventUIToUI.Instance.EventContinueNextQuest += OnEventContinueNextQuest;
		CsGameEventUIToUI.Instance.EventOnClickPanelDialogAccept += OnEventOnClickPanelDialogAccept;
		CsGameEventUIToUI.Instance.EventOnClickPanelDialogCancel += OnEventOnClickPanelDialogCancel;

        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        //CsGameEventToUI.Instance.EventAutoStop += OnEventAutoStop;

        CsGameEventToUI.Instance.EventPrevContinentEnter += OnEventPrevContinentEnter;
        CsGameEventUIToUI.Instance.EventStartQuestAutoPlay += OnEventStartQuestAutoPlay;

        CsGameEventUIToUI.Instance.EventExpAcquisition += OnEventExpAcquisition;

        //퀘스트 패널
        CsGameEventUIToUI.Instance.EventDisplayQuestPanel += OnEventDisplayQuestPanel;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter += OnEventContinentExitForMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonReEnter += OnEventContinentExitForMainQuestDungeonReEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter += OnEventMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart += OnEventMainQuestDungeonStepStart;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon += OnEventMainQuestDungeonAbandon;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStopAutoPlay += OnEventMainQuestDungeonStopAutoPlay;


        CsDungeonManager.Instance.EventDungeonStopAutoPlay += OnEventDungeonStopAutoPlay;
		CsDungeonManager.Instance.EventUpdateDungeonMember += OnEventUpdateDungeonMember;

		//스토리던전
		CsDungeonManager.Instance.EventContinentExitForStoryDungeonEnter += OnEventContinentExitForStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonEnter += OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonStepStart += OnEventStoryDungeonStepStart;
        CsDungeonManager.Instance.EventStoryDungeonAbandon += OnEventStoryDungeonAbandon;
        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;

        //경험치던전
        CsDungeonManager.Instance.EventContinentExitForExpDungeonEnter += OnEventContinentExitForExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonEnter += OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonWaveStart += OnEventExpDungeonWaveStart;
        CsDungeonManager.Instance.EventExpDungeonAbandon += OnEventExpDungeonAbandon;
        CsDungeonManager.Instance.EventExpDungeonExit += OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonBanished += OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventLakMonsterDead += OnEventLakMonsterDead;

        //골드던전
        CsDungeonManager.Instance.EventContinentExitForGoldDungeonEnter += OnEventContinentExitForGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonEnter += OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonWaveStart += OnEventGoldDungeonWaveStart;
        CsDungeonManager.Instance.EventGoldDungeonAbandon += OnEventGoldDungeonAbandon;
        CsDungeonManager.Instance.EventGoldDungeonExit += OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonBanished += OnEventGoldDungeonBanished;

        //지하미로
        CsDungeonManager.Instance.EventUndergroundMazeEnter += OnEventUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForUndergroundMazeRevive += OnEventUndergroundMazeEnterForUndergroundMazeRevive;
        CsDungeonManager.Instance.EventUndergroundMazePortalExit += OnEventUndergroundMazePortalExit;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForTransmission += OnEventUndergroundMazeEnterForTransmission;
        CsDungeonManager.Instance.EventUndergroundMazeTransmissionNpcDialog += OnEventUndergroundMazeTransmissionNpcDialog;
        CsDungeonManager.Instance.EventUndergroundMazeBanished += OnEventUndergroundMazeBanished;
        CsDungeonManager.Instance.EventUndergroundMazeExit += OnEventUndergroundMazeExit;

        //고대유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter += OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomEnter += OnEventArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomAbandon += OnEventArtifactRoomAbandon;
        CsDungeonManager.Instance.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit += OnEventArtifactRoomExit;
        CsDungeonManager.Instance.EventArtifactRoomStart += OnEventArtifactRoomStart;

        //고대인의 유적
        CsDungeonManager.Instance.EventContinentExitForAncientRelicEnter += OnEventContinentExitForAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicEnter += OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicExit += OnEventAncientRelicExit;
        CsDungeonManager.Instance.EventAncientRelicAbandon += OnEventAncientRelicAbandon;
        CsDungeonManager.Instance.EventAncientRelicBanished += OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicStepStart += OnEventAncientRelicStepStart;
        CsDungeonManager.Instance.EventAncientRelicPointUpdated += OnEventAncientRelicPointUpdated;
        

        //영혼을 탐하는 자
        CsDungeonManager.Instance.EventContinentExitForSoulCoveterEnter += OnEventContinentExitForSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterEnter += OnEventSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterExit += OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterAbandon += OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished += OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventSoulCoveterWaveStart += OnEventSoulCoveterWaveStart;

        //검투 대회
        CsDungeonManager.Instance.EventContinentExitForFieldOfHonorChallenge += OnEventContinentExitForFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorChallenge += OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorExit += OnEventFieldOfHonorExit;
        CsDungeonManager.Instance.EventFieldOfHonorBanished += OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorAbandon += OnEventFieldOfHonorAbandon;
        CsDungeonManager.Instance.EventFieldOfHonorStart += OnEventFieldOfHonorStart;

        //용맹의 증명
        CsDungeonManager.Instance.EventContinentExitForProofOfValorEnter += OnEventContinentExitForProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorEnter += OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorExit += OnEventProofOfValorExit;
        CsDungeonManager.Instance.EventProofOfValorAbandon += OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBanished += OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorStart += OnEventProofOfValorStart;

		// 지혜의 신전
		CsDungeonManager.Instance.EventContinentExitForWisdomTempleEnter += OnEventContinentExitForWisdomTempleEnter;
		CsDungeonManager.Instance.EventWisdomTempleEnter += OnEventWisdomTempleEnter;
		CsDungeonManager.Instance.EventWisdomTempleExit += OnEventWisdomTempleExit;
		CsDungeonManager.Instance.EventWisdomTempleAbandon += OnEventWisdomTempleAbandon;
		CsDungeonManager.Instance.EventWisdomTempleStepStart += OnEventWisdomTempleStepStart;
		CsDungeonManager.Instance.EventWisdomTempleStepCompleted += OnEventWisdomTempleStepCompleted;
		CsDungeonManager.Instance.EventWisdomTempleColorMatchingObjectCheck += OnEventWisdomTempleColorMatchingObjectCheck;
		CsDungeonManager.Instance.EventWisdomTempleColorMatchingMonsterCreated += OnEventWisdomTempleColorMatchingMonsterCreated;
		CsDungeonManager.Instance.EventWisdomTempleColorMatchingMonsterKill += OnEventWisdomTempleColorMatchingMonsterKill;
		CsDungeonManager.Instance.EventWisdomTempleFakeTreasureBoxKill += OnEventWisdomTempleFakeTreasureBoxKill;
		CsDungeonManager.Instance.EventWisdomTemplePuzzleCompleted += OnEventWisdomTemplePuzzleCompleted;
		CsDungeonManager.Instance.EventWisdomTempleQuizFail += OnEventWisdomTempleQuizFail;
		CsDungeonManager.Instance.EventWisdomTempleBossMonsterCreated += OnEventWisdomTempleBossMonsterCreated;

		// 유적 탈환
		CsDungeonManager.Instance.EventContinentExitForRuinsReclaimEnter += OnEventContinentExitForRuinsReclaimEnter;
		CsDungeonManager.Instance.EventRuinsReclaimEnter += OnEventRuinsReclaimEnter;
		CsDungeonManager.Instance.EventRuinsReclaimStepStart += OnEventRuinsReclaimStepStart;
		CsDungeonManager.Instance.EventRuinsReclaimStepWaveSkillCast += OnEventRuinsReclaimStepWaveSkillCast;
		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;

        // 무한 대전
        CsDungeonManager.Instance.EventContinentExitForInfiniteWarEnter += OnEventContinentExitForInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart += OnEventInfiniteWarStart;
        CsDungeonManager.Instance.EventInfiniteWarExit += OnEventInfiniteWarExit;
        CsDungeonManager.Instance.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarAbandon += OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarMonsterSpawn += OnEventInfiniteWarMonsterSpawn;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxCreated += OnEventInfiniteWarBuffBoxCreated;

        CsDungeonManager.Instance.EventInfiniteWarPointAcquisition += OnEventInfiniteWarPointAcquisition;
        CsDungeonManager.Instance.EventHeroInfiniteWarPointAcquisition += OnEventHeroInfiniteWarPointAcquisition;

		CsRplzSession.Instance.EventEvtHeroEnter += OnEventEvtHeroEnter;
        CsRplzSession.Instance.EventEvtHeroExit += OnEventEvtHeroExit;

		// 공포의 제단
		CsDungeonManager.Instance.EventContinentExitForFearAltarEnter += OnEventContinentExitForFearAltarEnter;
		CsDungeonManager.Instance.EventFearAltarEnter += OnEventFearAltarEnter;
		CsDungeonManager.Instance.EventFearAltarWaveStart += OnEventFearAltarWaveStart;
		CsDungeonManager.Instance.EventFearAltarExit += OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon += OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventContinentExitForWarMemoryEnter += OnEventContinentExitForWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryEnter += OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryWaveStart += OnEventWarMemoryWaveStart;
        CsDungeonManager.Instance.EventWarMemoryWaveCompleted += OnEventWarMemoryWaveCompleted;
        CsDungeonManager.Instance.EventWarMemoryExit += OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryMonsterSummon += OnEventWarMemoryMonsterSummon;
        CsDungeonManager.Instance.EventWarMemoryTransformationObjectInteractionFinished += OnEventWarMemoryTransformationObjectInteractionFinished;
        CsDungeonManager.Instance.EventHeroWarMemoryTransformationObjectInteractionFinished += OnEventHeroWarMemoryTransformationObjectInteractionFinished;

        CsDungeonManager.Instance.EventWarMemoryPointAcquisition += OnEventWarMemoryPointAcquisition;
        CsDungeonManager.Instance.EventHeroWarMemoryPointAcquisition += OnEventHeroWarMemoryPointAcquisition;
        
        // 오시리스 룸
        CsDungeonManager.Instance.EventContinentExitForOsirisRoomEnter += OnEventContinentExitForOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomEnter += OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart += OnEventOsirisRoomWaveStart;
        CsDungeonManager.Instance.EventOsirisRoomExit += OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon += OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished += OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomRewardGoldAcquisition += OnEventOsirisRoomRewardGoldAcquisition;
        CsDungeonManager.Instance.EventOsirisRoomMonsterSpawn += OnEventOsirisRoomMonsterSpawn;
        CsDungeonManager.Instance.EventOsirisRoomMonsterKillFail += OnEventOsirisRoomMonsterKillFail;

		// 전기퀘스트 던전
		CsDungeonManager.Instance.EventContinentExitForBiographyQuestDungeonEnter += OnEventContinentExitForBiographyQuestDungeonEnter;
		CsDungeonManager.Instance.EventBiographyQuestDungeonEnter += OnEventBiographyQuestDungeonEnter;
		CsDungeonManager.Instance.EventBiographyQuestDungeonExit += OnEventBiographyQuestDungeonExit;
		CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon += OnEventBiographyQuestDungeonAbandon;
		CsDungeonManager.Instance.EventBiographyQuestDungeonBanished += OnEventBiographyQuestDungeonBanished;
		CsDungeonManager.Instance.EventBiographyQuestDungeonWaveStart += OnEventBiographyQuestDungeonWaveStart;

        // 용의 둥지
        CsDungeonManager.Instance.EventContinentExitForDragonNestEnter += OnEventContinentExitForDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestEnter += OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestStepStart += OnEventDragonNestStepStart;
        CsDungeonManager.Instance.EventDragonNestExit += OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestAbandon += OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished += OnEventDragonNestBanished;

        // 무역선 탈환
        CsDungeonManager.Instance.EventContinentExitForTradeShipEnter += OnEventContinentExitForTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipEnter += OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipExit += OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon += OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipStepStart += OnEventTradeShipStepStart;
        CsDungeonManager.Instance.EventTradeShipPointAcquisition += OnEventTradeShipPointAcquisition;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventContinentExitForAnkouTombEnter += OnEventContinentExitForAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombEnter += OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombExit += OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombWaveStart += OnEventAnkouTombWaveStart;
        CsDungeonManager.Instance.EventAnkouTombPointAcquisition += OnEventAnkouTombPointAcquisition;

		// 팀 전장
		CsDungeonManager.Instance.EventContinentExitForTeamBattlefieldEnter += OnEventContinentExitForTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldEnter += OnEventTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit += OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon += OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished += OnEventTeamBattlefieldBanished;
		CsDungeonManager.Instance.EventTeamBattlefieldPlayWaitStart += OnEventTeamBattlefieldPlayWaitStart;
		CsDungeonManager.Instance.EventTeamBattlefieldPointAcquisition += OnEventTeamBattlefieldPointAcquisition;
		CsDungeonManager.Instance.EventHeroTeamBattlefieldPointAcquisition += OnEventHeroTeamBattlefieldPointAcquisition;

        // 정예 던전
        CsDungeonManager.Instance.EventContinentExitForEliteDungeonEnter += OnEventContinentExitForEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonEnter += OnEventEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonStart += OnEventEliteDungeonStart;

        //파티
        CsGameEventUIToUI.Instance.EventPartyCreate += OnEventPartyCreate;                                  //생성
        CsGameEventUIToUI.Instance.EventPartyExit += OnEventPartyExit;                                      //탈퇴
        CsGameEventUIToUI.Instance.EventPartyMemberBanish += OnEventPartyMemberBanish;                      //맴버강퇴
        //CsGameEventUIToUI.Instance.EventPartyCall += OnEventPartyCall;                                    //소집
        CsGameEventUIToUI.Instance.EventPartyDisband += OnEventPartyDisband;                                //해산
        CsGameEventUIToUI.Instance.EventPartyApplicationAccept += OnEventPartyApplicationAccept;            //신청수락
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept += OnEventPartyInvitationAccept;              //초대수락
        CsGameEventUIToUI.Instance.EventHeroPosition += OnEventHeroPosition;                                //파티집합

        //파티 서버이벤트
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted += OnEventPartyApplicationAccepted;        //신청수락 - 신청자에게
        CsGameEventUIToUI.Instance.EventPartyInvitationAccepted += OnEventPartyInvitationAccepted;          //초대수락 - 파티장에게
        CsGameEventUIToUI.Instance.EventPartyMemberEnter += OnEventPartyMemberEnter;                        //맴버입장
        CsGameEventUIToUI.Instance.EventPartyMemberExit += OnEventPartyMemberExit;                          //맴버퇴장
        CsGameEventUIToUI.Instance.EventPartyBanished += OnEventPartyBanished;                              //맴버강퇴
        CsGameEventUIToUI.Instance.EventPartyMasterChanged += OnEventPartyMasterChanged;                    //파티장 변경
        CsGameEventUIToUI.Instance.EventPartyCalled += OnEventPartyCalled;                                  //소집
        CsGameEventUIToUI.Instance.EventPartyDisbanded += OnEventPartyDisbanded;                            //해산
        CsGameEventUIToUI.Instance.EventPartyMembersUpdated += OnEventPartyMembersUpdated;                  //맴버업데이트

        CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;
        CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;

        // NPC 상호작용
        CsGameEventToUI.Instance.EventArrivalNpcByTouch += OnEventArrivalNpcByTouch;
        CsGameEventToUI.Instance.EventArrivalNpcByAuto += OnEventArrivalNpcByAuto;
        CsFishingQuestManager.Instance.EventArrivalNpcDialog += OnEventArrivalFishingNpcDialog;
        CsMainQuestManager.Instance.EventNationTransmission += OnEventMainQuestNationTransmission;

        //농장의 위협
        CsThreatOfFarmQuestManager.Instance.EventQuestAccepted += OnEventQuestAccepted;
        CsThreatOfFarmQuestManager.Instance.EventMissionAccepted += OnEventMissionAccepted;
        CsThreatOfFarmQuestManager.Instance.EventQuestAcceptDialog += OnEventQuestAcceptDialog;
        CsThreatOfFarmQuestManager.Instance.EventQuestComplete += OnEventQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventQuestCompleteDialog += OnEventQuestCompleteDialog;
        CsThreatOfFarmQuestManager.Instance.EventMissionMonsterSpawned += OnEventMissionMonsterSpawned;
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete += OnEventMissionComplete;
        CsThreatOfFarmQuestManager.Instance.EventMissionFail += OnEventMissionFail;
        CsThreatOfFarmQuestManager.Instance.EventQuestReset += OnEventQuestReset;
        CsThreatOfFarmQuestManager.Instance.EventStopAutoPlay += OnEventStopAutoPlayThreatOfFarm;
        CsThreatOfFarmQuestManager.Instance.EventStartAutoPlay += OnEventStartAutoPlayThreatOfFarm;
        CsThreatOfFarmQuestManager.Instance.EventMissionAbandoned += OnEventMissionAbandoned;

        //바운티 헌터
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse += OnEventBountyHunterQuestScrollUse;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete += OnEventBountyHunterQuestComplete;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestAbandon += OnEventBountyHunterQuestAbandon;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestUpdated += OnEventBountyHunterQuestUpdated;
        CsBountyHunterQuestManager.Instance.EventStopAutoPlay += OnEventStopAutoPlayBountyHunter;
        CsBountyHunterQuestManager.Instance.EventStartAutoPlay += OnEventStartAutoPlayBountyHunter;

        //밀서
        CsSecretLetterQuestManager.Instance.EventAcceptDialog += OnEventSecretLetterQuestAcceptDialog;
        CsSecretLetterQuestManager.Instance.EventMissionDialog += OnEventSecretLetterQuestMissionDialog;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestAccept += OnEventSecretLetterQuestAccept;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted += OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventNationTransmission += OnEventSecretLetterQuestNationTransmission;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete += OnEventSecretLetterQuestComplete;
        CsSecretLetterQuestManager.Instance.EventStopAutoPlay += OnEventSecretLetterQuestStopAutoPlay;
        CsSecretLetterQuestManager.Instance.EventStartAutoPlay += OnEventSecretLetterQuestStartAutoPlay;

        //의문의 박스
        CsMysteryBoxQuestManager.Instance.EventAcceptDialog += OnEventMysteryBoxAcceptDialog;
        CsMysteryBoxQuestManager.Instance.EventMissionDialog += OnEventMysteryBoxMissionDialog;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestAccept += OnEventMysteryBoxAccept;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted += OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventNationTransmission += OnEventMysteryBoxNationTransmission;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete += OnEventMysteryBoxComplete;
        CsMysteryBoxQuestManager.Instance.EventStopAutoPlay += OnEventMysteryBoxStopAutoPlay;
        CsMysteryBoxQuestManager.Instance.EventStartAutoPlay += OnEventMysteryBoxStartAutoPlay;

        //차원의 습격
        CsDimensionRaidQuestManager.Instance.EventNpctDialog += OnEventDimensionRaidQuestNpctDialog;
        CsDimensionRaidQuestManager.Instance.EventMissionDialog += OnEventDimensionRaidQuestMissionDialog;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestAccept += OnEventDimensionRaidQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventNationTransmission += OnEventDimensionRaidQuestNationTransmission;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted += OnEventDimensionRaidInteractionCompleted;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete += OnEventDimensionRaidQuestComplete;
        CsDimensionRaidQuestManager.Instance.EventStopAutoPlay += OnEventDimensionRaidQuestStopAutoPlay;
        CsDimensionRaidQuestManager.Instance.EventStartAutoPlay += OnEventDimensionRaidQuestStartAutoPlay;

        //위대한 성전
        CsHolyWarQuestManager.Instance.EventNpctDialog += OnEventHolyWarQuestNpcDialog;
        CsHolyWarQuestManager.Instance.EventNationTransmission += OnEventHolyWarQuestNationTransmission;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestAccept += OnEventHolyWarQuestAccept;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete += OnEventHolyWarQuestComplete;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestUpdated += OnEventHolyWarQuestUpdated;
        CsHolyWarQuestManager.Instance.EventStopAutoPlay += OnEventHolyWarStopAutoPlay;
        CsHolyWarQuestManager.Instance.EventStartAutoPlay += OnEventHolyWarStartAutoPlay;

        //자동이동취소버튼
        CsGameEventToUI.Instance.EventAutoStop += OnEventAutoStop;
        CsGameEventUIToUI.Instance.EventAutoCancelButtonOpen += OnEventAutoCancelButtonOpen;
        CsGameEventUIToUI.Instance.EventAutoQuestStart += OnEventAutoQuestStart;

        // 세리우 보급지원
        CsSupplySupportQuestManager.Instance.EventCartChangeNpcDialog += OnEventCartChangeNpcDialog;
        CsSupplySupportQuestManager.Instance.EventStartNpctDialog += OnEventStartNpctDialog;
        CsSupplySupportQuestManager.Instance.EventEndNpctDialog += OnEventEndNpctDialog;
        CsSupplySupportQuestManager.Instance.EventUpdateState += OnEventUpdateState;
        CsSupplySupportQuestManager.Instance.EventStopAutoPlay += OnEventStopAutoPlaySupplySupport;
        CsSupplySupportQuestManager.Instance.EventStartAutoPlay += OnEventStartAutoPlaySupplySupport;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept += OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange += OnEventSupplySupportQuestCartChange;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;

        //길드 농장퀘스트
        CsGuildManager.Instance.EventGuildFarmQuestNPCDialog += OnEventGuildFarmQuestNPCDialog;
        CsGuildManager.Instance.EventGuildFarmQuestAccept += OnEventGuildFarmQuestAccept;
        CsGuildManager.Instance.EventGuildFarmQuestComplete += OnEventGuildFarmQuestComplete;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCompleted += OnEventGuildFarmQuestInteractionCompleted;

        //길드 영지 입장
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter += OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit += OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
        CsGuildManager.Instance.EventStopAutoPlay += OnEventGuildStopAutoPlay;
        CsGuildManager.Instance.EventStartAutoPlay += OnEventGuildStartAutoPlay;

        //길드 제단
        CsGuildManager.Instance.EventGuildAltarNPCDialog += OnEventGuildAltarNPCDialog;
        CsGuildManager.Instance.EventGuildAltarCompleted += OnEventGuildAltarCompleted;

        //길드 제단 기부
        CsGuildManager.Instance.EventGuildAltarDonate += OnEventGuildAltarDonate;

        //길드 제단 마력주입완료
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionStart += OnEventGuildAltarSpellInjectionMissionStart;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCompleted += OnEventGuildAltarSpellInjectionMissionCompleted;

        //길드 제단 수비
        CsGuildManager.Instance.EventGuildAltarDefenseMissionCompleted += OnEventGuildAltarDefenseMissionCompleted;
        CsGuildManager.Instance.EventGuildAltarDefenseMissionStart += OnEventGuildAltarDefenseMissionStart;
        CsGuildManager.Instance.EventGuildAltarDefenseMissionFailed += OnEventGuildAltarDefenseMissionFailed;

        //길드 미션
        CsGuildManager.Instance.EventMissionMissionDialog += OnEventMissionMissionDialog;
        CsGuildManager.Instance.EventMissionAcceptDialog += OnEventMissionAcceptDialog;
        CsGuildManager.Instance.EventGuildMissionAccept += OnEventGuildMissionAccept;
        CsGuildManager.Instance.EventGuildMissionQuestAccept += OnEventGuildMissionQuestAccept;
        CsGuildManager.Instance.EventGuildMissionComplete += OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon += OnEventGuildMissionAbandon;
        CsGuildManager.Instance.EventGuildMissionFailed += OnEventGuildMissionFailed;
        CsGuildManager.Instance.EventUpdateMissionState += OnEventUpdateMissionState;

        //길드 군량
        CsGuildManager.Instance.EventGuildFoodWareHouseDialog += OnEventGuildFoodWareHouseDialog;

        //길드 물자 지원
        CsGuildManager.Instance.EventGuildSupplySupportQuestAccept += OnEventGuildSupplySupportQuestAccept;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail += OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventStartSupplySupportNpctDialog += OnEventStartSupplySupportNpctDialog;
        CsGuildManager.Instance.EventEndSupplySupportNpctDialog += OnEventEndSupplySupportNpctDialog;

        //길드 현상금
        CsGuildManager.Instance.EventStartGuildHuntingDialog += OnEventStartGuildHuntingDialog;
        CsGuildManager.Instance.EventGuildHuntingQuestAccept += OnEventGuildHuntingQuestAccept;
        CsGuildManager.Instance.EventGuildHuntingQuestComplete += OnEventGuildHuntingQuestComplete;
        CsGuildManager.Instance.EventGuildHuntingQuestUpdated += OnEventGuildHuntingQuestUpdated;
        CsGuildManager.Instance.EventGuildHuntingQuestAbandon += OnEventGuildHuntingQuestAbandon;
        CsGuildManager.Instance.EventGuildHuntingDonate += OnEventGuildHuntingDonate;
        CsGuildManager.Instance.EventGuildHuntingDonationCountUpdated += OnEventGuildHuntingDonationCountUpdated;

        // 길드 낚시
        CsGuildManager.Instance.EventFishingDialog += OnEventFishingDialog;

        // 국가전
        CsNationWarManager.Instance.EventNationWarNpcDialog += OnEventNationWarNpcDialog;

        //도감 - 풍광퀘스트
        CsIllustratedBookManager.Instance.EventSceneryQuestStart += OnEventSceneryQuestStart;
        CsIllustratedBookManager.Instance.EventSceneryQuestCanceled += OnEventSceneryQuestCanceled;
        CsIllustratedBookManager.Instance.EventSceneryQuestCompleted += OnEventSceneryQuestCompleted;

        // 일일 퀘스트
        CsDailyQuestManager.Instance.EventDailyQuestAbandon += OnEventDailyQuestAbandon;
        CsDailyQuestManager.Instance.EventDailyQuestAccept += OnEventDailyQuestAccept;
        CsDailyQuestManager.Instance.EventDailyQuestComplete += OnEventDailyQuestComplete;
        CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete += OnEventDailyQuestMissionImmediatlyComplete;
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated += OnEventHeroDailyQuestProgressCountUpdated;
        CsDailyQuestManager.Instance.EventStopAutoPlay += OnEventDailyQuestStopAutoPlay;
        CsDailyQuestManager.Instance.EventStartAutoPlay += OnEventDailyQuestStartAutoPlay;

        // 주간 퀘스트
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept += OnEventWeeklyQuestRoundAccept;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted += OnEventWeeklyQuestRoundCompleted;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete += OnEventWeeklyQuestRoundImmediatlyComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated += OnEventWeeklyQuestRoundProgressCountUpdated;
        CsWeeklyQuestManager.Instance.EventStopAutoPlay += OnEventWeeklyQuestStopAutoPlay;
        CsWeeklyQuestManager.Instance.EventStartAutoPlay += OnEventWeeklyQuestStartAutoPlay;

		// 진정한 영웅 퀘스트
		CsTrueHeroQuestManager.Instance.EventNpcDialog += OnEventTrueHeroQuestDialog;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept += OnEventTrueHeroQuestAccept;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;

		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepInteractionFinished += OnEventTrueHeroQuestStepInteractionFinished;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepWaitingCanceled += OnEventTrueHeroQuestStepWaitingCanceled;

		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepComplete;
		CsTrueHeroQuestManager.Instance.EventStopAutoPlay += OnEventTrueHeroQuestStopAutoPlay;
		CsTrueHeroQuestManager.Instance.EventStartAutoPlay += OnEventTrueHeroQuestStartAutoPlay;

		// 서브 퀘스트
		CsSubQuestManager.Instance.EventSubQuestAccept += OnEventSubQuestAccept;
		CsSubQuestManager.Instance.EventSubQuestAbandon += OnEventSubQuestAbandon;
		CsSubQuestManager.Instance.EventSubQuestComplete += OnEventSubQuestComplete;
		CsSubQuestManager.Instance.EventSubQuestProgressCountsUpdated += OnEventSubQuestProgressCountsUpdated;
		CsSubQuestManager.Instance.EventSubQuestsAccepted += OnEventSubQuestsAccepted;
		CsSubQuestManager.Instance.EventNpcDialog += OnEventSubQuestNpcDialog;

		CsSubQuestManager.Instance.EventStartAutoPlay += OnEventSubQuestStartAutoPlay;
		CsSubQuestManager.Instance.EventStopAutoPlay += OnEventSubQuestStopAutoPlay;

		// 전기
		CsGameEventUIToUI.Instance.EventBiographyNpcDialog += OnEventBiographyNpcDialog;		// 수락

		CsBiographyManager.Instance.EventStartAutoPlay += OnEventBiographyQuestStartAutoPlay;
		CsBiographyManager.Instance.EventStopAutoPlay += OnEventBiographyQuestStopAutoPlay;
		CsBiographyManager.Instance.EventNpcDialog += OnEventBiographyNpcDialog;				// 완료, 재입장
		
		CsBiographyManager.Instance.EventBiographyQuestAccept += OnEventBiographyQuestAccept;
		CsBiographyManager.Instance.EventBiographyQuestComplete += OnEventBiographyQuestComplete;
		CsBiographyManager.Instance.EventBiographyQuestMoveObjectiveComplete += OnEventBiographyQuestMoveObjectiveComplete;
		CsBiographyManager.Instance.EventBiographyQuestNpcConversationComplete += OnEventBiographyQuestNpcConversationComplete;
		CsBiographyManager.Instance.EventBiographyQuestProgressCountsUpdated += OnEventBiographyQuestProgressCountsUpdated;

		// 크리쳐 농장 퀘스트
		CsCreatureFarmQuestManager.Instance.EventAcceptDialog += OnEventCreatureFarmQuestAcceptDialog;
		CsCreatureFarmQuestManager.Instance.EventCompleteDialog += OnEventCreatureFarmQuestCompleteDialog;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept += OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete += OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted += OnEventCreatureFarmQuestMissionCompleted;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete += OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMonsterSpawned += OnEventCreatureFarmQuestMissionMonsterSpawned;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionProgressCountUpdated += OnEventCreatureFarmQuestMissionProgressCountUpdated;
		CsCreatureFarmQuestManager.Instance.EventStartAutoPlay += OnEventCreatureFarmQuestStartAutoPlay;
		CsCreatureFarmQuestManager.Instance.EventStopAutoPlay += OnEventCreatureFarmQuestStopAutoPlay;
		
		// 전직 퀘스트
		CsJobChangeManager.Instance.EventJobChangeQuestAccept += OnEventJobChangeQuestAccept;
		CsJobChangeManager.Instance.EventJobChangeQuestComplete += OnEventJobChangeQuestComplete;
		CsJobChangeManager.Instance.EventJobChangeQuestFailed += OnEventJobChangeQuestFailed;
		CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated += OnEventJobChangeQuestProgressCountUpdated;
        CsJobChangeManager.Instance.EventStartAutoPlay += OnEventJobChangeQuestStartAutoPlay;
        CsJobChangeManager.Instance.EventStopAutoPlay += OnEventJobChangeQuestStopAutoPlay;
        CsJobChangeManager.Instance.EventNpcEndDialog += OnEventJobChangeQuestNpcEndDialog;
        CsJobChangeManager.Instance.EventNpcStartDialog += OnEventJobChangeQuestNpcStartDialog;

        CsGameEventToUI.Instance.EventChangeAutoBattleMode += OnEventChangeAutoBattleMode;
        CsGameEventUIToUI.Instance.EventChangeAutoBattleMode += OnEventChangeAutoBattleMode;

        // 재접속시
        CsGameEventUIToUI.Instance.EventHeroLogin += OnEventHeroLogin;
    }

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        StartCoroutine(LoadQuestPanel());
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_trPopupSupplySupportAccept != null)
            {
                Destroy(m_trPopupSupplySupportAccept.gameObject);
            }

            if (m_trPopupGuildHungting != null)
            {
                Destroy(m_trPopupGuildHungting.gameObject);
            }
        }

        if (m_flTime + 1.0f < Time.time)
        {
            //Debug.Log(m_textQuestName);

            if (CsGameData.Instance.MyHeroInfo.Party != null)
            {
                if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count > 1)
                {
                    if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
                    {
                        if (m_textPartyCall != null)
                        {
                            if (CsGameData.Instance.MyHeroInfo.Party.CallRemainingCoolTime - Time.realtimeSinceStartup > 0)
                            {
                                m_textPartyCall.text = (CsGameData.Instance.MyHeroInfo.Party.CallRemainingCoolTime - Time.realtimeSinceStartup).ToString("#0");
                                m_textPartyCall.color = CsUIData.Instance.ColorRed;

                                if (m_buttonPartyCall.interactable == true)
                                {
                                    m_buttonPartyCall.interactable = false;
                                }
                            }
                            else
                            {
                                m_textPartyCall.text = CsConfiguration.Instance.GetString("A36_TXT_00003");
                                m_textPartyCall.color = CsUIData.Instance.ColorWhite;

                                if (m_buttonPartyCall.interactable == false)
                                {
                                    m_buttonPartyCall.interactable = true;
                                }
                            }
                        }
                    }
                }
            }

            if (m_flExpWaveRemainingTime - Time.realtimeSinceStartup > 0)
            {
                UpdateExpDungeonPanel();
            }

            if (m_bIsLoadQuestPanel)
            {
                if (m_textTimeThreatOfFarm != null && m_textTimeThreatOfFarm.gameObject.activeSelf && m_flThreatOfFarmMissionRemainingTime - Time.realtimeSinceStartup > 0)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(m_flThreatOfFarmMissionRemainingTime - Time.realtimeSinceStartup);
                    m_textTimeThreatOfFarm.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
                }

                if (CsHolyWarQuestManager.Instance.HolyWarQuest != null && CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Accepted)
                {
                    UpdateToggleHolyWarQuestTimeText();
                }

                if (CsSupplySupportQuestManager.Instance.RemainingTime - Time.realtimeSinceStartup > 0)
                {
                    UpdateSupplySupportTime();
                }

                if (CsGuildManager.Instance.GuildMissionMonster != null && CsGuildManager.Instance.GuildMissionMonster.RemainingLifeTime > 0)
                {
                    UpdateGuildMissionTimeText();
                }

                if (CsGuildManager.Instance.GuildSupplySupportQuestRemainingTime - Time.realtimeSinceStartup > 0)
                {
                    UpdateGuildSupplySupportTime();
                }
            }

            if (m_trPopupNpcDialog != null && m_trPopupNpcDialog.gameObject.activeSelf == true)
            {
                if (CsGuildManager.Instance.IsGuildDefense == false && m_bIsAlter == true)
                {
                    UpdateAlterDefenceButton();
                }
            }

            int nQuestType = (int)EnQuestType.DailyQuest01;

            for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
            {
                if (GetQuestPanel((EnQuestType)nQuestType) == null)
                {
                    continue;
                }
                else
                {
                    UpdateQuestPanel((EnQuestType)nQuestType);
                }

                nQuestType++;
            }

			if (CsTrueHeroQuestManager.Instance.Interacted)
			{
				UpdateTrueHeroQuestWaitingTime();
			}

			if (CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest != null &&
				CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission != null &&
				CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 3)
			{
				UpdateCreatureFarmQuestWaitingTime();
			}

            if (CsJobChangeManager.Instance.HeroJobChangeQuest != null &&
                CsJobChangeManager.Instance.HeroJobChangeQuest.RemainingTime >= 0f)
            {
                UpdateJobChangeQuestWaitingTime();
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventQuestCompltedError -= OnEventErrorClear;

        CsGameEventUIToUI.Instance.EventReturnScrollUseStart -= OnEventReturnScrollUseStart;

        // 메인 퀘스트
		CsMainQuestManager.Instance.EventAcceptDialog -= OnEventAcceptDialog;
        CsMainQuestManager.Instance.EventAccepted -= OnEventAccepted;
        CsMainQuestManager.Instance.EventExecuteDataUpdated -= OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventCompleteDialog -= OnEventCompleteDialog;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsMainQuestManager.Instance.EventStopAutoPlay -= OnEventStopAutoPlay;
        CsMainQuestManager.Instance.EventStartAutoPlay -= OnEventStartAutoPlay;

		CsGameEventUIToUI.Instance.EventContinueNextQuest -= OnEventContinueNextQuest;
		CsGameEventUIToUI.Instance.EventOnClickPanelDialogAccept -= OnEventOnClickPanelDialogAccept;
		CsGameEventUIToUI.Instance.EventOnClickPanelDialogCancel -= OnEventOnClickPanelDialogCancel;

        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;
        //CsGameEventToUI.Instance.EventAutoStop -= OnEventAutoStop;

        CsGameEventToUI.Instance.EventPrevContinentEnter -= OnEventPrevContinentEnter;
        CsGameEventUIToUI.Instance.EventStartQuestAutoPlay -= OnEventStartQuestAutoPlay;

        CsGameEventUIToUI.Instance.EventExpAcquisition -= OnEventExpAcquisition;

        //퀘스트 패널
        CsGameEventUIToUI.Instance.EventDisplayQuestPanel -= OnEventDisplayQuestPanel;

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonEnter -= OnEventContinentExitForMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventContinentExitForMainQuestDungeonReEnter -= OnEventContinentExitForMainQuestDungeonReEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonEnter -= OnEventMainQuestDungeonEnter;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepStart -= OnEventMainQuestDungeonStepStart;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonAbandon -= OnEventMainQuestDungeonAbandon;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStopAutoPlay -= OnEventMainQuestDungeonStopAutoPlay;


        CsDungeonManager.Instance.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;
		CsDungeonManager.Instance.EventUpdateDungeonMember -= OnEventUpdateDungeonMember;

		//스토리던전
		CsDungeonManager.Instance.EventContinentExitForStoryDungeonEnter -= OnEventContinentExitForStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonEnter -= OnEventStoryDungeonEnter;
        CsDungeonManager.Instance.EventStoryDungeonStepStart -= OnEventStoryDungeonStepStart;
        CsDungeonManager.Instance.EventStoryDungeonAbandon -= OnEventStoryDungeonAbandon;
        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;
        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;

        //경험치던전
        CsDungeonManager.Instance.EventContinentExitForExpDungeonEnter -= OnEventContinentExitForExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonEnter -= OnEventExpDungeonEnter;
        CsDungeonManager.Instance.EventExpDungeonWaveStart -= OnEventExpDungeonWaveStart;
        CsDungeonManager.Instance.EventExpDungeonAbandon -= OnEventExpDungeonAbandon;
        CsDungeonManager.Instance.EventExpDungeonExit -= OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonBanished -= OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventLakMonsterDead -= OnEventLakMonsterDead;

        //골드던전
        CsDungeonManager.Instance.EventContinentExitForGoldDungeonEnter -= OnEventContinentExitForGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonEnter -= OnEventGoldDungeonEnter;
        CsDungeonManager.Instance.EventGoldDungeonWaveStart -= OnEventGoldDungeonWaveStart;
        CsDungeonManager.Instance.EventGoldDungeonAbandon -= OnEventGoldDungeonAbandon;
        CsDungeonManager.Instance.EventGoldDungeonExit -= OnEventGoldDungeonExit;
        CsDungeonManager.Instance.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;

        //지하미로
        CsDungeonManager.Instance.EventUndergroundMazeEnter -= OnEventUndergroundMazeEnter;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForUndergroundMazeRevive -= OnEventUndergroundMazeEnterForUndergroundMazeRevive;
        CsDungeonManager.Instance.EventUndergroundMazePortalExit -= OnEventUndergroundMazePortalExit;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForTransmission -= OnEventUndergroundMazeEnterForTransmission;
        CsDungeonManager.Instance.EventUndergroundMazeTransmissionNpcDialog -= OnEventUndergroundMazeTransmissionNpcDialog;
        CsDungeonManager.Instance.EventUndergroundMazeBanished -= OnEventUndergroundMazeBanished;
        CsDungeonManager.Instance.EventUndergroundMazeExit -= OnEventUndergroundMazeExit;

        //고대유물의 방
        CsDungeonManager.Instance.EventContinentExitForArtifactRoomEnter -= OnEventContinentExitForArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomEnter -= OnEventArtifactRoomEnter;
        CsDungeonManager.Instance.EventArtifactRoomAbandon -= OnEventArtifactRoomAbandon;
        CsDungeonManager.Instance.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit -= OnEventArtifactRoomExit;
        CsDungeonManager.Instance.EventArtifactRoomStart -= OnEventArtifactRoomStart;

        //고대인의 유적
        CsDungeonManager.Instance.EventContinentExitForAncientRelicEnter -= OnEventContinentExitForAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicEnter -= OnEventAncientRelicEnter;
        CsDungeonManager.Instance.EventAncientRelicExit -= OnEventAncientRelicExit;
        CsDungeonManager.Instance.EventAncientRelicAbandon -= OnEventAncientRelicAbandon;
        CsDungeonManager.Instance.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicStepStart -= OnEventAncientRelicStepStart;
        CsDungeonManager.Instance.EventAncientRelicPointUpdated -= OnEventAncientRelicPointUpdated;        

        //영혼을 탐하는 자
        CsDungeonManager.Instance.EventContinentExitForSoulCoveterEnter -= OnEventContinentExitForSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterEnter -= OnEventSoulCoveterEnter;
        CsDungeonManager.Instance.EventSoulCoveterExit -= OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterAbandon -= OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;
        CsDungeonManager.Instance.EventSoulCoveterWaveStart -= OnEventSoulCoveterWaveStart;

        //검투 대회
        CsDungeonManager.Instance.EventContinentExitForFieldOfHonorChallenge -= OnEventContinentExitForFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorChallenge -= OnEventFieldOfHonorChallenge;
        CsDungeonManager.Instance.EventFieldOfHonorExit -= OnEventFieldOfHonorExit;
        CsDungeonManager.Instance.EventFieldOfHonorBanished -= OnEventFieldOfHonorBanished;
        CsDungeonManager.Instance.EventFieldOfHonorAbandon -= OnEventFieldOfHonorAbandon;
        CsDungeonManager.Instance.EventFieldOfHonorStart -= OnEventFieldOfHonorStart;

        //용맹의 증명
        CsDungeonManager.Instance.EventContinentExitForProofOfValorEnter -= OnEventContinentExitForProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorEnter -= OnEventProofOfValorEnter;
        CsDungeonManager.Instance.EventProofOfValorExit -= OnEventProofOfValorExit;
        CsDungeonManager.Instance.EventProofOfValorAbandon -= OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBanished -= OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorStart -= OnEventProofOfValorStart;

        // 지혜의 신전
        CsDungeonManager.Instance.EventContinentExitForWisdomTempleEnter -= OnEventContinentExitForWisdomTempleEnter;
        CsDungeonManager.Instance.EventWisdomTempleEnter -= OnEventWisdomTempleEnter;
        CsDungeonManager.Instance.EventWisdomTempleExit -= OnEventWisdomTempleExit;
        CsDungeonManager.Instance.EventWisdomTempleAbandon -= OnEventWisdomTempleAbandon;
        CsDungeonManager.Instance.EventWisdomTempleStepStart -= OnEventWisdomTempleStepStart;
        CsDungeonManager.Instance.EventWisdomTempleStepCompleted -= OnEventWisdomTempleStepCompleted;
        CsDungeonManager.Instance.EventWisdomTempleColorMatchingObjectCheck -= OnEventWisdomTempleColorMatchingObjectCheck;
        CsDungeonManager.Instance.EventWisdomTempleColorMatchingMonsterCreated -= OnEventWisdomTempleColorMatchingMonsterCreated;
        CsDungeonManager.Instance.EventWisdomTempleColorMatchingMonsterKill -= OnEventWisdomTempleColorMatchingMonsterKill;
        CsDungeonManager.Instance.EventWisdomTempleFakeTreasureBoxKill -= OnEventWisdomTempleFakeTreasureBoxKill;
        CsDungeonManager.Instance.EventWisdomTemplePuzzleCompleted -= OnEventWisdomTemplePuzzleCompleted;
        CsDungeonManager.Instance.EventWisdomTempleQuizFail -= OnEventWisdomTempleQuizFail;
        CsDungeonManager.Instance.EventWisdomTempleBossMonsterCreated -= OnEventWisdomTempleBossMonsterCreated;

        // 유적 탈환
        CsDungeonManager.Instance.EventContinentExitForRuinsReclaimEnter -= OnEventContinentExitForRuinsReclaimEnter;
        CsDungeonManager.Instance.EventRuinsReclaimEnter -= OnEventRuinsReclaimEnter;
        CsDungeonManager.Instance.EventRuinsReclaimStepStart -= OnEventRuinsReclaimStepStart;
        CsDungeonManager.Instance.EventRuinsReclaimStepWaveSkillCast -= OnEventRuinsReclaimStepWaveSkillCast;
        CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;
        CsDungeonManager.Instance.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;

        // 무한 대전
        CsDungeonManager.Instance.EventContinentExitForInfiniteWarEnter -= OnEventContinentExitForInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
        CsDungeonManager.Instance.EventInfiniteWarStart -= OnEventInfiniteWarStart;
        CsDungeonManager.Instance.EventInfiniteWarExit -= OnEventInfiniteWarExit;
        CsDungeonManager.Instance.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarAbandon -= OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarMonsterSpawn -= OnEventInfiniteWarMonsterSpawn;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxCreated -= OnEventInfiniteWarBuffBoxCreated;

        CsDungeonManager.Instance.EventInfiniteWarPointAcquisition -= OnEventInfiniteWarPointAcquisition;
        CsDungeonManager.Instance.EventHeroInfiniteWarPointAcquisition -= OnEventHeroInfiniteWarPointAcquisition;

        CsRplzSession.Instance.EventEvtHeroEnter -= OnEventEvtHeroEnter;
        CsRplzSession.Instance.EventEvtHeroExit -= OnEventEvtHeroExit;

        // 공포의 제단
        CsDungeonManager.Instance.EventContinentExitForFearAltarEnter -= OnEventContinentExitForFearAltarEnter;
        CsDungeonManager.Instance.EventFearAltarEnter -= OnEventFearAltarEnter;
        CsDungeonManager.Instance.EventFearAltarWaveStart -= OnEventFearAltarWaveStart;
        CsDungeonManager.Instance.EventFearAltarExit -= OnEventFearAltarExit;
        CsDungeonManager.Instance.EventFearAltarAbandon -= OnEventFearAltarAbandon;
        CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventContinentExitForWarMemoryEnter -= OnEventContinentExitForWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryEnter -= OnEventWarMemoryEnter;
        CsDungeonManager.Instance.EventWarMemoryWaveStart -= OnEventWarMemoryWaveStart;
        CsDungeonManager.Instance.EventWarMemoryWaveCompleted -= OnEventWarMemoryWaveCompleted;
        CsDungeonManager.Instance.EventWarMemoryExit -= OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryMonsterSummon -= OnEventWarMemoryMonsterSummon;
        CsDungeonManager.Instance.EventWarMemoryTransformationObjectInteractionFinished -= OnEventWarMemoryTransformationObjectInteractionFinished;
        CsDungeonManager.Instance.EventHeroWarMemoryTransformationObjectInteractionFinished -= OnEventHeroWarMemoryTransformationObjectInteractionFinished;

        CsDungeonManager.Instance.EventWarMemoryPointAcquisition -= OnEventWarMemoryPointAcquisition;
        CsDungeonManager.Instance.EventHeroWarMemoryPointAcquisition -= OnEventHeroWarMemoryPointAcquisition;

        // 오시리스 룸
        CsDungeonManager.Instance.EventContinentExitForOsirisRoomEnter -= OnEventContinentExitForOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomEnter -= OnEventOsirisRoomEnter;
        CsDungeonManager.Instance.EventOsirisRoomWaveStart -= OnEventOsirisRoomWaveStart;
        CsDungeonManager.Instance.EventOsirisRoomExit -= OnEventOsirisRoomExit;
        CsDungeonManager.Instance.EventOsirisRoomAbandon -= OnEventOsirisRoomAbandon;
        CsDungeonManager.Instance.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;
        CsDungeonManager.Instance.EventOsirisRoomRewardGoldAcquisition -= OnEventOsirisRoomRewardGoldAcquisition;
        CsDungeonManager.Instance.EventOsirisRoomMonsterSpawn -= OnEventOsirisRoomMonsterSpawn;
        CsDungeonManager.Instance.EventOsirisRoomMonsterKillFail -= OnEventOsirisRoomMonsterKillFail;

        // 전기퀘스트 던전
        CsDungeonManager.Instance.EventContinentExitForBiographyQuestDungeonEnter -= OnEventContinentExitForBiographyQuestDungeonEnter;
        CsDungeonManager.Instance.EventBiographyQuestDungeonEnter -= OnEventBiographyQuestDungeonEnter;
        CsDungeonManager.Instance.EventBiographyQuestDungeonExit -= OnEventBiographyQuestDungeonExit;
        CsDungeonManager.Instance.EventBiographyQuestDungeonAbandon -= OnEventBiographyQuestDungeonAbandon;
        CsDungeonManager.Instance.EventBiographyQuestDungeonBanished -= OnEventBiographyQuestDungeonBanished;
        CsDungeonManager.Instance.EventBiographyQuestDungeonWaveStart -= OnEventBiographyQuestDungeonWaveStart;

        // 용의 둥지
        CsDungeonManager.Instance.EventContinentExitForDragonNestEnter -= OnEventContinentExitForDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestEnter -= OnEventDragonNestEnter;
        CsDungeonManager.Instance.EventDragonNestStepStart -= OnEventDragonNestStepStart;
        CsDungeonManager.Instance.EventDragonNestExit -= OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestAbandon -= OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished -= OnEventDragonNestBanished;

        // 무역선 탈환
        CsDungeonManager.Instance.EventContinentExitForTradeShipEnter -= OnEventContinentExitForTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipEnter -= OnEventTradeShipEnter;
        CsDungeonManager.Instance.EventTradeShipExit -= OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon -= OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipStepStart -= OnEventTradeShipStepStart;
        CsDungeonManager.Instance.EventTradeShipPointAcquisition -= OnEventTradeShipPointAcquisition;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventContinentExitForAnkouTombEnter -= OnEventContinentExitForAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombEnter -= OnEventAnkouTombEnter;
        CsDungeonManager.Instance.EventAnkouTombExit -= OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombWaveStart -= OnEventAnkouTombWaveStart;
        CsDungeonManager.Instance.EventAnkouTombPointAcquisition -= OnEventAnkouTombPointAcquisition;

		// 팀 전장
		CsDungeonManager.Instance.EventContinentExitForTeamBattlefieldEnter -= OnEventContinentExitForTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldEnter -= OnEventTeamBattlefieldEnter;
		CsDungeonManager.Instance.EventTeamBattlefieldExit -= OnEventTeamBattlefieldExit;
		CsDungeonManager.Instance.EventTeamBattlefieldAbandon -= OnEventTeamBattlefieldAbandon;
		CsDungeonManager.Instance.EventTeamBattlefieldBanished -= OnEventTeamBattlefieldBanished;
		CsDungeonManager.Instance.EventTeamBattlefieldPlayWaitStart -= OnEventTeamBattlefieldPlayWaitStart;
		CsDungeonManager.Instance.EventTeamBattlefieldPointAcquisition -= OnEventTeamBattlefieldPointAcquisition;
		CsDungeonManager.Instance.EventHeroTeamBattlefieldPointAcquisition -= OnEventHeroTeamBattlefieldPointAcquisition;

        // 정예 던전
        CsDungeonManager.Instance.EventContinentExitForEliteDungeonEnter -= OnEventContinentExitForEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonEnter -= OnEventEliteDungeonEnter;
        CsDungeonManager.Instance.EventEliteDungeonStart -= OnEventEliteDungeonStart;

        //파티
        CsGameEventUIToUI.Instance.EventPartyCreate -= OnEventPartyCreate;                                  //생성
        CsGameEventUIToUI.Instance.EventPartyExit -= OnEventPartyExit;                                      //탈퇴
        CsGameEventUIToUI.Instance.EventPartyMemberBanish -= OnEventPartyMemberBanish;                      //맴버강퇴
        //CsGameEventUIToUI.Instance.EventPartyCall -= OnEventPartyCall;                                    //소집
        CsGameEventUIToUI.Instance.EventPartyDisband -= OnEventPartyDisband;                                //해산
        CsGameEventUIToUI.Instance.EventPartyApplicationAccept -= OnEventPartyApplicationAccept;            //신청수락
        CsGameEventUIToUI.Instance.EventPartyInvitationAccept -= OnEventPartyInvitationAccept;              //초대수락
        CsGameEventUIToUI.Instance.EventHeroPosition -= OnEventHeroPosition;                                //파티집합

        //파티 서버이벤트
        CsGameEventUIToUI.Instance.EventPartyApplicationAccepted -= OnEventPartyApplicationAccepted;        //신청수락 - 신청자에게
        CsGameEventUIToUI.Instance.EventPartyInvitationAccepted -= OnEventPartyInvitationAccepted;          //초대수락 - 파티장에게
        CsGameEventUIToUI.Instance.EventPartyMemberEnter -= OnEventPartyMemberEnter;                        //맴버입장
        CsGameEventUIToUI.Instance.EventPartyMemberExit -= OnEventPartyMemberExit;                          //맴버퇴장
        CsGameEventUIToUI.Instance.EventPartyBanished -= OnEventPartyBanished;                              //맴버강퇴
        CsGameEventUIToUI.Instance.EventPartyMasterChanged -= OnEventPartyMasterChanged;                    //파티장 변경
        CsGameEventUIToUI.Instance.EventPartyCalled -= OnEventPartyCalled;                                  //소집
        CsGameEventUIToUI.Instance.EventPartyDisbanded -= OnEventPartyDisbanded;                            //해산
        CsGameEventUIToUI.Instance.EventPartyMembersUpdated -= OnEventPartyMembersUpdated;                  //맴버업데이트

        CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;
        CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;

        // NPC 상호작용
        CsGameEventToUI.Instance.EventArrivalNpcByTouch -= OnEventArrivalNpcByTouch;
        CsGameEventToUI.Instance.EventArrivalNpcByAuto -= OnEventArrivalNpcByAuto;
        CsFishingQuestManager.Instance.EventArrivalNpcDialog -= OnEventArrivalFishingNpcDialog;
        CsMainQuestManager.Instance.EventNationTransmission -= OnEventMainQuestNationTransmission;

        //농장의 위협
        CsThreatOfFarmQuestManager.Instance.EventQuestAccepted -= OnEventQuestAccepted;
        CsThreatOfFarmQuestManager.Instance.EventMissionAccepted -= OnEventMissionAccepted;
        CsThreatOfFarmQuestManager.Instance.EventQuestAcceptDialog -= OnEventQuestAcceptDialog;
        CsThreatOfFarmQuestManager.Instance.EventQuestComplete -= OnEventQuestComplete;
        CsThreatOfFarmQuestManager.Instance.EventQuestCompleteDialog -= OnEventQuestCompleteDialog;
        CsThreatOfFarmQuestManager.Instance.EventMissionMonsterSpawned -= OnEventMissionMonsterSpawned;
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete -= OnEventMissionComplete;
        CsThreatOfFarmQuestManager.Instance.EventMissionFail -= OnEventMissionFail;
        CsThreatOfFarmQuestManager.Instance.EventQuestReset -= OnEventQuestReset;
        CsThreatOfFarmQuestManager.Instance.EventStopAutoPlay -= OnEventStopAutoPlayThreatOfFarm;
        CsThreatOfFarmQuestManager.Instance.EventStartAutoPlay -= OnEventStartAutoPlayThreatOfFarm;
        CsThreatOfFarmQuestManager.Instance.EventMissionAbandoned -= OnEventMissionAbandoned;

        //바운티 헌터
        CsGameEventUIToUI.Instance.EventBountyHunterQuestScrollUse -= OnEventBountyHunterQuestScrollUse;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete -= OnEventBountyHunterQuestComplete;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestAbandon -= OnEventBountyHunterQuestAbandon;
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestUpdated -= OnEventBountyHunterQuestUpdated;
        CsBountyHunterQuestManager.Instance.EventStopAutoPlay -= OnEventStopAutoPlayBountyHunter;
        CsBountyHunterQuestManager.Instance.EventStartAutoPlay -= OnEventStartAutoPlayBountyHunter;

        //밀서
        CsSecretLetterQuestManager.Instance.EventAcceptDialog -= OnEventSecretLetterQuestAcceptDialog;
        CsSecretLetterQuestManager.Instance.EventMissionDialog -= OnEventSecretLetterQuestMissionDialog;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestAccept -= OnEventSecretLetterQuestAccept;
        CsSecretLetterQuestManager.Instance.EventSecretLetterPickCompleted -= OnEventSecretLetterPickCompleted;
        CsSecretLetterQuestManager.Instance.EventNationTransmission -= OnEventSecretLetterQuestNationTransmission;
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete -= OnEventSecretLetterQuestComplete;
        CsSecretLetterQuestManager.Instance.EventStopAutoPlay -= OnEventSecretLetterQuestStopAutoPlay;
        CsSecretLetterQuestManager.Instance.EventStartAutoPlay -= OnEventSecretLetterQuestStartAutoPlay;

        //의문의 박스
        CsMysteryBoxQuestManager.Instance.EventAcceptDialog -= OnEventMysteryBoxAcceptDialog;
        CsMysteryBoxQuestManager.Instance.EventMissionDialog -= OnEventMysteryBoxMissionDialog;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestAccept -= OnEventMysteryBoxAccept;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxPickCompleted -= OnEventMysteryBoxPickCompleted;
        CsMysteryBoxQuestManager.Instance.EventNationTransmission -= OnEventMysteryBoxNationTransmission;
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete -= OnEventMysteryBoxComplete;
        CsMysteryBoxQuestManager.Instance.EventStopAutoPlay -= OnEventMysteryBoxStopAutoPlay;
        CsMysteryBoxQuestManager.Instance.EventStartAutoPlay -= OnEventMysteryBoxStartAutoPlay;

        //차원의 습격
        CsDimensionRaidQuestManager.Instance.EventNpctDialog -= OnEventDimensionRaidQuestNpctDialog;
        CsDimensionRaidQuestManager.Instance.EventMissionDialog -= OnEventDimensionRaidQuestMissionDialog;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestAccept -= OnEventDimensionRaidQuestAccept;
        CsDimensionRaidQuestManager.Instance.EventNationTransmission -= OnEventDimensionRaidQuestNationTransmission;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted -= OnEventDimensionRaidInteractionCompleted;
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete -= OnEventDimensionRaidQuestComplete;
        CsDimensionRaidQuestManager.Instance.EventStopAutoPlay -= OnEventDimensionRaidQuestStopAutoPlay;
        CsDimensionRaidQuestManager.Instance.EventStartAutoPlay -= OnEventDimensionRaidQuestStartAutoPlay;

        //위대한 성전
        CsHolyWarQuestManager.Instance.EventNpctDialog -= OnEventHolyWarQuestNpcDialog;
        CsHolyWarQuestManager.Instance.EventNationTransmission -= OnEventHolyWarQuestNationTransmission;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestAccept -= OnEventHolyWarQuestAccept;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete -= OnEventHolyWarQuestComplete;
        CsHolyWarQuestManager.Instance.EventHolyWarQuestUpdated -= OnEventHolyWarQuestUpdated;
        CsHolyWarQuestManager.Instance.EventStopAutoPlay -= OnEventHolyWarStopAutoPlay;
        CsHolyWarQuestManager.Instance.EventStartAutoPlay -= OnEventHolyWarStartAutoPlay;

        //자동이동취소버튼
        CsGameEventToUI.Instance.EventAutoStop -= OnEventAutoStop;
        CsGameEventUIToUI.Instance.EventAutoCancelButtonOpen -= OnEventAutoCancelButtonOpen;
        CsGameEventUIToUI.Instance.EventAutoQuestStart -= OnEventAutoQuestStart;

        // 세리우 보급지원
        CsSupplySupportQuestManager.Instance.EventCartChangeNpcDialog -= OnEventCartChangeNpcDialog;
        CsSupplySupportQuestManager.Instance.EventStartNpctDialog -= OnEventStartNpctDialog;
        CsSupplySupportQuestManager.Instance.EventEndNpctDialog -= OnEventEndNpctDialog;
        CsSupplySupportQuestManager.Instance.EventUpdateState -= OnEventUpdateState;
        CsSupplySupportQuestManager.Instance.EventStopAutoPlay -= OnEventStopAutoPlaySupplySupport;
        CsSupplySupportQuestManager.Instance.EventStartAutoPlay -= OnEventStartAutoPlaySupplySupport;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept -= OnEventSupplySupportQuestAccept;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange -= OnEventSupplySupportQuestCartChange;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;

        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;

        //길드 농장퀘스트
        CsGuildManager.Instance.EventGuildFarmQuestNPCDialog -= OnEventGuildFarmQuestNPCDialog;
        CsGuildManager.Instance.EventGuildFarmQuestAccept -= OnEventGuildFarmQuestAccept;
        CsGuildManager.Instance.EventGuildFarmQuestComplete -= OnEventGuildFarmQuestComplete;
        CsGuildManager.Instance.EventGuildFarmQuestInteractionCompleted -= OnEventGuildFarmQuestInteractionCompleted;

        //길드 영지 입장
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter -= OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit -= OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
        CsGuildManager.Instance.EventStopAutoPlay -= OnEventGuildStopAutoPlay;
        CsGuildManager.Instance.EventStartAutoPlay -= OnEventGuildStartAutoPlay;

        //길드 제단
        CsGuildManager.Instance.EventGuildAltarNPCDialog -= OnEventGuildAltarNPCDialog;
        CsGuildManager.Instance.EventGuildAltarCompleted -= OnEventGuildAltarCompleted;

        //길드 제단 기부
        CsGuildManager.Instance.EventGuildAltarDonate -= OnEventGuildAltarDonate;

        //길드 제단 마력주입완료
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionStart -= OnEventGuildAltarSpellInjectionMissionStart;
        CsGuildManager.Instance.EventGuildAltarSpellInjectionMissionCompleted -= OnEventGuildAltarSpellInjectionMissionCompleted;

        //길드 제단 수비
        CsGuildManager.Instance.EventGuildAltarDefenseMissionCompleted -= OnEventGuildAltarDefenseMissionCompleted;
        CsGuildManager.Instance.EventGuildAltarDefenseMissionStart -= OnEventGuildAltarDefenseMissionStart;
        CsGuildManager.Instance.EventGuildAltarDefenseMissionFailed -= OnEventGuildAltarDefenseMissionFailed;

        //길드 미션
        CsGuildManager.Instance.EventMissionMissionDialog -= OnEventMissionMissionDialog;
        CsGuildManager.Instance.EventMissionAcceptDialog -= OnEventMissionAcceptDialog;
        CsGuildManager.Instance.EventGuildMissionAccept -= OnEventGuildMissionAccept;
        CsGuildManager.Instance.EventGuildMissionQuestAccept -= OnEventGuildMissionQuestAccept;
        CsGuildManager.Instance.EventGuildMissionComplete -= OnEventGuildMissionComplete;
        CsGuildManager.Instance.EventGuildMissionAbandon -= OnEventGuildMissionAbandon;
        CsGuildManager.Instance.EventGuildMissionFailed -= OnEventGuildMissionFailed;
        CsGuildManager.Instance.EventUpdateMissionState -= OnEventUpdateMissionState;

        //길드 군량
        CsGuildManager.Instance.EventGuildFoodWareHouseDialog -= OnEventGuildFoodWareHouseDialog;

        //길드 물자 지원
        CsGuildManager.Instance.EventGuildSupplySupportQuestAccept -= OnEventGuildSupplySupportQuestAccept;
        CsGuildManager.Instance.EventGuildSupplySupportQuestFail -= OnEventGuildSupplySupportQuestFail;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete -= OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventStartSupplySupportNpctDialog -= OnEventStartSupplySupportNpctDialog;
        CsGuildManager.Instance.EventEndSupplySupportNpctDialog -= OnEventEndSupplySupportNpctDialog;

        //길드 현상금
        CsGuildManager.Instance.EventStartGuildHuntingDialog -= OnEventStartGuildHuntingDialog;
        CsGuildManager.Instance.EventGuildHuntingQuestAccept -= OnEventGuildHuntingQuestAccept;
        CsGuildManager.Instance.EventGuildHuntingQuestComplete -= OnEventGuildHuntingQuestComplete;
        CsGuildManager.Instance.EventGuildHuntingQuestUpdated -= OnEventGuildHuntingQuestUpdated;
        CsGuildManager.Instance.EventGuildHuntingQuestAbandon -= OnEventGuildHuntingQuestAbandon;
        CsGuildManager.Instance.EventGuildHuntingDonate -= OnEventGuildHuntingDonate;
        CsGuildManager.Instance.EventGuildHuntingDonationCountUpdated -= OnEventGuildHuntingDonationCountUpdated;

        // 길드 낚시
        CsGuildManager.Instance.EventFishingDialog -= OnEventFishingDialog;

        // 국가전
        CsNationWarManager.Instance.EventNationWarNpcDialog -= OnEventNationWarNpcDialog;

        //도감 - 풍광퀘스트
        CsIllustratedBookManager.Instance.EventSceneryQuestStart -= OnEventSceneryQuestStart;
        CsIllustratedBookManager.Instance.EventSceneryQuestCanceled -= OnEventSceneryQuestCanceled;
        CsIllustratedBookManager.Instance.EventSceneryQuestCompleted -= OnEventSceneryQuestCompleted;

        // 일일 퀘스트
        CsDailyQuestManager.Instance.EventDailyQuestAbandon -= OnEventDailyQuestAbandon;
        CsDailyQuestManager.Instance.EventDailyQuestAccept -= OnEventDailyQuestAccept;
        CsDailyQuestManager.Instance.EventDailyQuestComplete -= OnEventDailyQuestComplete;
        CsDailyQuestManager.Instance.EventDailyQuestMissionImmediatlyComplete -= OnEventDailyQuestMissionImmediatlyComplete;
        CsDailyQuestManager.Instance.EventHeroDailyQuestProgressCountUpdated -= OnEventHeroDailyQuestProgressCountUpdated;
        CsDailyQuestManager.Instance.EventStopAutoPlay -= OnEventDailyQuestStopAutoPlay;
        CsDailyQuestManager.Instance.EventStartAutoPlay -= OnEventDailyQuestStartAutoPlay;

        // 주간 퀘스트
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept -= OnEventWeeklyQuestRoundAccept;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted -= OnEventWeeklyQuestRoundCompleted;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete -= OnEventWeeklyQuestRoundImmediatlyComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete -= OnEventWeeklyQuestRoundMoveMissionComplete;
        CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundProgressCountUpdated -= OnEventWeeklyQuestRoundProgressCountUpdated;
        CsWeeklyQuestManager.Instance.EventStopAutoPlay -= OnEventWeeklyQuestStopAutoPlay;
        CsWeeklyQuestManager.Instance.EventStartAutoPlay -= OnEventWeeklyQuestStartAutoPlay;

        // 진정한 영웅 퀘스트
        CsTrueHeroQuestManager.Instance.EventNpcDialog -= OnEventTrueHeroQuestDialog;
        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept -= OnEventTrueHeroQuestAccept;
        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;

        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepInteractionFinished -= OnEventTrueHeroQuestStepInteractionFinished;
        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepWaitingCanceled -= OnEventTrueHeroQuestStepWaitingCanceled;

        CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepComplete;
        CsTrueHeroQuestManager.Instance.EventStopAutoPlay -= OnEventTrueHeroQuestStopAutoPlay;
        CsTrueHeroQuestManager.Instance.EventStartAutoPlay -= OnEventTrueHeroQuestStartAutoPlay;

        // 서브 퀘스트
        CsSubQuestManager.Instance.EventSubQuestAccept -= OnEventSubQuestAccept;
        CsSubQuestManager.Instance.EventSubQuestAbandon -= OnEventSubQuestAbandon;
        CsSubQuestManager.Instance.EventSubQuestComplete -= OnEventSubQuestComplete;
        CsSubQuestManager.Instance.EventSubQuestProgressCountsUpdated -= OnEventSubQuestProgressCountsUpdated;
        CsSubQuestManager.Instance.EventSubQuestsAccepted -= OnEventSubQuestsAccepted;
        CsSubQuestManager.Instance.EventNpcDialog -= OnEventSubQuestNpcDialog;

        CsSubQuestManager.Instance.EventStartAutoPlay -= OnEventSubQuestStartAutoPlay;
        CsSubQuestManager.Instance.EventStopAutoPlay -= OnEventSubQuestStopAutoPlay;

        // 전기
        CsGameEventUIToUI.Instance.EventBiographyNpcDialog -= OnEventBiographyNpcDialog;		// 수락

        CsBiographyManager.Instance.EventStartAutoPlay -= OnEventBiographyQuestStartAutoPlay;
        CsBiographyManager.Instance.EventStopAutoPlay -= OnEventBiographyQuestStopAutoPlay;
        CsBiographyManager.Instance.EventNpcDialog -= OnEventBiographyNpcDialog;				// 완료, 재입장

        CsBiographyManager.Instance.EventBiographyQuestAccept -= OnEventBiographyQuestAccept;
        CsBiographyManager.Instance.EventBiographyQuestComplete -= OnEventBiographyQuestComplete;
        CsBiographyManager.Instance.EventBiographyQuestMoveObjectiveComplete -= OnEventBiographyQuestMoveObjectiveComplete;
        CsBiographyManager.Instance.EventBiographyQuestNpcConversationComplete -= OnEventBiographyQuestNpcConversationComplete;
        CsBiographyManager.Instance.EventBiographyQuestProgressCountsUpdated -= OnEventBiographyQuestProgressCountsUpdated;

		// 크리쳐 농장 퀘스트
		CsCreatureFarmQuestManager.Instance.EventAcceptDialog -= OnEventCreatureFarmQuestAcceptDialog;
		CsCreatureFarmQuestManager.Instance.EventCompleteDialog -= OnEventCreatureFarmQuestCompleteDialog;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept -= OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete -= OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted -= OnEventCreatureFarmQuestMissionCompleted;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete -= OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMonsterSpawned -= OnEventCreatureFarmQuestMissionMonsterSpawned;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionProgressCountUpdated -= OnEventCreatureFarmQuestMissionProgressCountUpdated;
		CsCreatureFarmQuestManager.Instance.EventStartAutoPlay -= OnEventCreatureFarmQuestStartAutoPlay;
		CsCreatureFarmQuestManager.Instance.EventStopAutoPlay -= OnEventCreatureFarmQuestStopAutoPlay;

        // 전직 퀘스트
        CsJobChangeManager.Instance.EventJobChangeQuestAccept -= OnEventJobChangeQuestAccept;
        CsJobChangeManager.Instance.EventJobChangeQuestComplete -= OnEventJobChangeQuestComplete;
        CsJobChangeManager.Instance.EventJobChangeQuestFailed -= OnEventJobChangeQuestFailed;
        CsJobChangeManager.Instance.EventJobChangeQuestProgressCountUpdated -= OnEventJobChangeQuestProgressCountUpdated;
        CsJobChangeManager.Instance.EventStartAutoPlay -= OnEventJobChangeQuestStartAutoPlay;
        CsJobChangeManager.Instance.EventStopAutoPlay -= OnEventJobChangeQuestStopAutoPlay;
        CsJobChangeManager.Instance.EventNpcEndDialog -= OnEventJobChangeQuestNpcEndDialog;
        CsJobChangeManager.Instance.EventNpcStartDialog -= OnEventJobChangeQuestNpcStartDialog;

        CsGameEventToUI.Instance.EventChangeAutoBattleMode -= OnEventChangeAutoBattleMode;
        CsGameEventUIToUI.Instance.EventChangeAutoBattleMode -= OnEventChangeAutoBattleMode;

        // 재접속시
        CsGameEventUIToUI.Instance.EventHeroLogin -= OnEventHeroLogin;
	}

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccepted()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationAccepted()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberEnter(CsPartyMember csPartyMember)
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberExit(CsPartyMember csPartyMember, bool bBanished)
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberBanish(Guid guidMemberId)
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyBanished()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMasterChanged()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCalled(int nContinentId, int nNationId, Vector3 v3Position)
    {
        //팝업생성
        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A36_TXT_03004"), CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), (() => PartyCalled(nContinentId, nNationId, v3Position)),
                                                  CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisbanded()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMembersUpdated()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChange()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAutoCancel(EnAutoStateType enAutoStateType)
    {
        Transform trQuestPanel = null;
        CsUIData.Instance.AutoStateType = EnAutoStateType.None;

        m_buttonAutoCancel.gameObject.SetActive(false);

        switch (enAutoStateType)
        {
            case EnAutoStateType.None:
                break;

            case EnAutoStateType.Battle:

                CsGameEventUIToUI.Instance.OnEventChangeAutoBattleMode(EnBattleMode.None);
                CsGameEventToIngame.Instance.OnEventAutoBattleStart(EnBattleMode.None);

                if (m_enAutoStateType != EnAutoStateType.None)
                {
                    AutoCancelButtonOpen(m_enAutoStateType);
                }

                break;

            case EnAutoStateType.MainQuest:
                trQuestPanel = GetQuestPanel(EnQuestType.MainQuest);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsMainQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.Move:
			case EnAutoStateType.FindingPath:
                CsGameEventToIngame.Instance.OnEventAutoStop(EnAutoStateType.Move);
                break;

            case EnAutoStateType.Fishing:
			case EnAutoStateType.TrueHeroTaunted:
                CsGameEventToIngame.Instance.OnEventAutoStop(EnAutoStateType.Move);

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.SecretLetter:
                trQuestPanel = GetQuestPanel(EnQuestType.SecretLetter);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsSecretLetterQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.MysteryBox:
                trQuestPanel = GetQuestPanel(EnQuestType.MysteryBox);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsMysteryBoxQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.DimensionRaid:
                trQuestPanel = GetQuestPanel(EnQuestType.DimensionRaid);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsDimensionRaidQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.ThreatOfFarm:
                trQuestPanel = GetQuestPanel(EnQuestType.ThreatOfFarm);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.HolyWar:
                trQuestPanel = GetQuestPanel(EnQuestType.HolyWar);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsHolyWarQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.SupplySupport:
                trQuestPanel = GetQuestPanel(EnQuestType.SupplySupport);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsSupplySupportQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildMission:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildMission);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Mission);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildAlter:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildAlter);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Altar);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildAlterDefence:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildAlterDefence);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Defense);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildFarm:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildFarm);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.FarmQuest);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildFoodWareHouse:
                CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.FoodWareHouse);

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildSupplySupport:
                trQuestPanel = GetQuestPanel(EnQuestType.SupplySupport);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.SupplySupport);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildHunting:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildHunting);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Hunting);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.GuildFishing:
                CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Fishing);

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.NationWar:
                CsGameEventToIngame.Instance.OnEventAutoStop(EnAutoStateType.Move);

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.DailyQuest01:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest01);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsDailyQuestManager.Instance.StopAutoPlay(this, CsDailyQuestManager.Instance.HeroDailyQuestList[0].Id);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.DailyQuest02:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest02);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsDailyQuestManager.Instance.StopAutoPlay(this, CsDailyQuestManager.Instance.HeroDailyQuestList[1].Id);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.DailyQuest03:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest03);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsDailyQuestManager.Instance.StopAutoPlay(this, CsDailyQuestManager.Instance.HeroDailyQuestList[2].Id);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

            case EnAutoStateType.WeeklyQuest:
                trQuestPanel = GetQuestPanel(EnQuestType.WeeklyQuest);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsWeeklyQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

                break;

			case EnAutoStateType.TrueHero:
				trQuestPanel = GetQuestPanel(EnQuestType.TrueHero);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsTrueHeroQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

				break;

			case EnAutoStateType.SubQuest:
				foreach (var csHeroSubQuest in CsSubQuestManager.Instance.HeroSubQuestList)
				{
					trQuestPanel = GetQuestPanel(EnQuestType.SubQuest, csHeroSubQuest.SubQuest.QuestId);

					if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
					{
					    trQuestPanel.GetComponent<Toggle>().isOn = false;
						break;
					}
					else
					{
					    CsSubQuestManager.Instance.StopAutoPlay(this);
					}
				}

                m_enAutoStateType = EnAutoStateType.None;

				break;

			case EnAutoStateType.Biography:
				foreach (var csHeroBiography in CsBiographyManager.Instance.HeroBiographyList)
				{
					trQuestPanel = GetQuestPanel(EnQuestType.Biography, csHeroBiography.BiographyId);

					if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
					{
						trQuestPanel.GetComponent<Toggle>().isOn = false;
						break;
					}
					else
					{
						CsBiographyManager.Instance.StopAutoPlay(this);
					}
				}

                m_enAutoStateType = EnAutoStateType.None;

				break;

			case EnAutoStateType.CreatureFarm:
				trQuestPanel = GetQuestPanel(EnQuestType.CreatureFarm);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsCreatureFarmQuestManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

				break;

			case EnAutoStateType.JobChange:
				trQuestPanel = GetQuestPanel(EnQuestType.JobChange);

                if (trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }
                else
                {
                    CsJobChangeManager.Instance.StopAutoPlay(this);
                }

                m_enAutoStateType = EnAutoStateType.None;

				break;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickRewardItem(int nItemId, int nItemCount, bool bOwned)
	{
		CsItem csItem = CsGameData.Instance.GetItem(nItemId);

		if (csItem == null)
			return;

		if (m_goPopupItemInfo == null)
		{
			StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csItem, nItemCount, bOwned)));
		}
		else
		{
			OpenPopupItemInfo(csItem, nItemCount, bOwned);
		}
	}

    #endregion Event

    #region EventHandler

    #region 도감 - 풍광퀘스트
    //---------------------------------------------------------------------------------------------------
    void OnEventSceneryQuestStart(int nQuestId)
    {
        OpenPopupSceneryQuest(nQuestId);

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsConfiguration.Instance.GetString("A84_TXT_00011"), CsGameData.Instance.GetSceneryQuest(nQuestId).Description, "frm_guide_npc01");
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneryQuestCanceled()
    {
        ClosePopupSceneryQuest();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsConfiguration.Instance.GetString("A84_TXT_00011"), CsConfiguration.Instance.GetString("A84_TXT_00014"), "frm_guide_npc01");
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneryQuestCompleted(PDItemBooty pDItemBooty, int nQuestId)
    {
        ClosePopupSceneryQuest();

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsConfiguration.Instance.GetString("A84_TXT_00011"), CsConfiguration.Instance.GetString("A84_TXT_00013"), "frm_guide_npc01");
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    #endregion 도감 - 풍광퀘스트

    #region 재접속
    //---------------------------------------------------------------------------------------------------
    void OnEventHeroLogin()
    {
        InitializeQuestPanel();
    }
    #endregion 재접속

    #region 길드영지
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGuildTerritoryEnter(string strSceneName)
    {
        GuildTerritoryEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryExit(int nContinentId)
    {
        m_bIsInGuildIsland = false;
        UpdateQuestPanel();

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");
        trQuestContent.localPosition = Vector3.zero;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBanished(int nContinentId)
    {
        m_bIsInGuildIsland = false;
        UpdateQuestPanel();

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");
        trQuestContent.localPosition = Vector3.zero;
    }

    //---------------------------------------------------------------------------------------------------
    void GuildTerritoryEnter()
    {
        AllPanelQuestOff();

        m_bIsInGuildIsland = true;

        if (CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint || CsGuildManager.Instance.Guild.MoralPoint < CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint)
        {
            UpdateQuestPanel(EnQuestType.GuildAlter);
        }

        if (CsGuildManager.Instance.IsGuildDefense)
        {
            UpdateQuestPanel(EnQuestType.GuildAlterDefence);
        }

        if (CsGuildManager.Instance.GuildFarmQuestState != EnGuildFarmQuestState.None)
        {
            UpdateQuestPanel(EnQuestType.GuildFarm);
        }

        CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;
        
        if (csHeroJobChangeQuest != null && csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
        {
            
        }
        else
        {
            UpdateQuestPanel(EnQuestType.JobChange);
        }

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");
        trQuestContent.localPosition = Vector3.zero;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarNPCDialog()
    {
        CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Altar);
        OpenAlterNpcDialogPopup();
        DisplayOffAutoCancel();
    }

    void OnEventGuildAltarCompleted(bool bLevelUp, long lAcquiredExp)
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    //길드
    void OnEventGuildStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
    {
        CsPanelQuest csPanelQuest = objCaller as CsPanelQuest;

        switch (enGuildPlayAutoState)
        {
            case EnGuildPlayState.FarmQuest:
                if (CsGuildManager.Instance.GuildFarmQuestState != EnGuildFarmQuestState.None)
                {
                    ChangeQuestToggleIsOnDisplay(EnQuestType.GuildFarm, false);
                }
                break;

            case EnGuildPlayState.Altar:
                ChangeQuestToggleIsOnDisplay(EnQuestType.GuildAlter, false);
                break;

            case EnGuildPlayState.Defense:
                if (CsGuildManager.Instance.IsGuildDefense)
                {
                    ChangeQuestToggleIsOnDisplay(EnQuestType.GuildAlterDefence, false);
                }
                break;

            case EnGuildPlayState.FoodWareHouse:
                ChangeQuestToggleIsOnDisplay(EnQuestType.GuildAlterDefence, false);
                break;

            case EnGuildPlayState.Mission:
                ChangeQuestToggleIsOnDisplay(EnQuestType.GuildMission, false);
                break;

            case EnGuildPlayState.SupplySupport:
                ChangeQuestToggleIsOnDisplay(EnQuestType.GuildSupplySupport, false);
                break;

            case EnGuildPlayState.Hunting:
                ChangeQuestToggleIsOnDisplay(EnQuestType.GuildHunting, false);
                break;
        }

        DisplayOffAutoCancel();
    }

    void OnEventGuildStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
    {
        switch (enGuildPlayAutoState)
        {
            case EnGuildPlayState.FarmQuest:
                AutoCancelButtonOpen(EnAutoStateType.GuildFarm);
                break;

            case EnGuildPlayState.FoodWareHouse:
                AutoCancelButtonOpen(EnAutoStateType.GuildFoodWareHouse);
                break;

            case EnGuildPlayState.Altar:
                AutoCancelButtonOpen(EnAutoStateType.GuildAlter);
                break;

            case EnGuildPlayState.Defense:
                AutoCancelButtonOpen(EnAutoStateType.GuildAlterDefence);
                break;

            case EnGuildPlayState.Mission:
                AutoCancelButtonOpen(EnAutoStateType.GuildMission);
                break;

            case EnGuildPlayState.SupplySupport:
                AutoCancelButtonOpen(EnAutoStateType.SupplySupport);
                break;

            case EnGuildPlayState.Hunting:
                AutoCancelButtonOpen(EnAutoStateType.GuildHunting);
                break;
        }
    }

    #endregion

    #region 길드제단 기부

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDonate()
    {
        UpdateQuestPanel(EnQuestType.GuildAlter);
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A68_TXT_03001"), CsGameData.Instance.GuildAltar.DonationRewardMoralPoint, CsGuildManager.Instance.GuildMoralPoint, CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint));
        CloseNpcDialogPopup();

        UpdateQuestPanel(EnQuestType.GuildAlter);
    }

    #endregion 길드제단 기부

    #region 길드제단 마력주입
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCompleted()
    {
        UpdateQuestPanel(EnQuestType.GuildAlter);
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A68_TXT_03001"), CsGameData.Instance.GuildAltar.SpellInjectionRewardMoralPoint, CsGuildManager.Instance.GuildMoralPoint, CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint));

        UpdateQuestPanel(EnQuestType.GuildAlter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionStart()
    {
        CloseNpcDialogPopup();
    }

    #endregion

    #region 길드제단 수비
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDefenseMissionCompleted()
    {
        UpdateQuestPanel(EnQuestType.GuildAlter);
        UpdateQuestPanel(EnQuestType.GuildAlterDefence);

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A68_TXT_03001"), CsGameData.Instance.GuildAltar.DefenseRewardMoralPoint, CsGuildManager.Instance.GuildMoralPoint, CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint));

        UpdateQuestPanel(EnQuestType.GuildAlter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDefenseMissionStart()
    {
        CloseNpcDialogPopup();
        UpdateQuestPanel(EnQuestType.GuildAlterDefence);
        ChangeQuestToggleIsOn(EnQuestType.GuildAlterDefence, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildAltarDefenseMissionFailed()
    {
        UpdateQuestPanel(EnQuestType.GuildAlterDefence);
    }

    #endregion

    #region 길드미션



    #endregion 길드미션

    #region 정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForEliteDungeonEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonEnter(Guid guid, PDVector3 pdVector3, float flRotationY, PDEliteDungeonMonsterInstance[] pDEliteDungeonMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.EliteDungeon.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonStart()
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        UpdateEliteDungeonPanel();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;
    }

    #endregion 정예 던전

    #region 영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForSoulCoveterEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterEnter(Guid guid, PDVector3 pDVector3, float flRotate, PDHero[] pDHero, PDMonsterInstance[] aPDMonsterInstance)
    {
        UpdateMatchingDIsplay();

        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime > 0)
		{
			m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
			StartCoroutine(m_iEnumeratorDungeonReadyTime);
		}
		else
		{
			Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
			toggleDungeon.interactable = true;

			UpdateRuinsReclaimPanelStepStart();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterExit(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.SoulCoveter));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterAbandon(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.SoulCoveter));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterBanished(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.SoulCoveter));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterWaveStart(PDSoulCoveterMonsterInstance[] pDSoulCoveterMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateSoulConveterDungeonPanel();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.SoulCoveterDifficultyWave.GuideTitle, CsDungeonManager.Instance.SoulCoveterDifficultyWave.GuideContent, CsDungeonManager.Instance.SoulCoveterDifficultyWave.GuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateDungeonMember()
    {
        UpdateMatchingDIsplay();
    }

    #endregion 영혼을 탐하는 자

    #region 고대인의 유적

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForAncientRelicEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicEnter(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDMonsterInstance[] aPDMonsterInstance, Guid[] arrGuidTrapEffectHeroe, int[] arrRemovedObstacleId)
    {
        UpdateMatchingDIsplay();

        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

		// 시작 후 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime > 0)
		{
			m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
			StartCoroutine(m_iEnumeratorDungeonReadyTime);
		}
		else
		{
			Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
			toggleDungeon.interactable = true;

			UpdateRuinsReclaimPanelStepStart();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicExit(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AncientRelic));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicAbandon(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AncientRelic));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinentId)
    {
        DisplayNpcGuideDialog(false);
        UpdateParty();

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AncientRelic));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicStepStart(int nRemoveObstacleId, Vector3 v3Position, float flTargetRadius)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateAncientRelicDungeonPanel();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.AncientRelicStep.GuideTitle, CsDungeonManager.Instance.AncientRelicStep.GuideContent, CsDungeonManager.Instance.AncientRelicStep.GuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicPointUpdated()
    {
        UpdateAncientRelicDungeonPanel();
    }

    #endregion 고대인의 유적

    #region 검투대회
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForFieldOfHonorChallenge()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorChallenge(Guid guid, PDVector3 pdVector3, float flRotationY, PDHero pdHeroTarget)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.FieldOfHonor.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorExit(int nContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.FieldOfHonor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorBanished(int nContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.FieldOfHonor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorAbandon(int nContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.FieldOfHonor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorStart()
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;
        UpdateFieldOfHonorDungeonPanel();
    }

    #endregion 검투대회

    #region 용맹의 증명

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForProofOfValorEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorEnter(Guid guid, PDVector3 pdVector3, float flRotationY, PDMonsterInstance[] pDMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.ProofOfValor.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorExit(int nContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ProofOfValor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorAbandon(int nContinentId, bool bLevelUp, long lAcquiredExp)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ProofOfValor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBanished(int nContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ProofOfValor));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorStart()
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateProofOfValorDungeonPanel();
    }

    #endregion 용맹의 증명

	#region 지혜의 신전

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForWisdomTempleEnter()
	{
		DungeonEnter();

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = false;
		ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleEnter(Guid Guid, PDVector3 pDVector3, float flLotationY)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.WisdomTemple.StartDelayTime);
		StartCoroutine(m_iEnumeratorDungeonReadyTime);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleExit(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.WisdomTemple));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleAbandon(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.WisdomTemple));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepStart(PDWisdomTempleMonsterInstance[] aPDWisdomTempleMonsterInstance, PDWisdomTempleColorMatchingObjectInstance[] aPDWisdomTempleColorMatchingObjectInstance, int nQuizNo)
	{
		m_nPuzzleCount = 0;
		m_nQuizNo = nQuizNo;

		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = true;

		UpdateWisdomTemplePanel();

		OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTempleStep.GuideTitle, CsDungeonManager.Instance.WisdomTempleStep.GuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepCompleted(bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		if (CsDungeonManager.Instance.WisdomTempleStep.Type == 2)
		{
			// 퀴즈 정답 가이드
			OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.QuizRightAnswerGuideTitle, CsDungeonManager.Instance.WisdomTemple.QuizRightAnswerGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingObjectCheck(PDWisdomTempleColorMatchingObjectInstance[] aPDWisdomTempleColorMatchingObjectInstance, int nColorMatchingPoint)
	{
		if (m_nPuzzleCount != nColorMatchingPoint)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A105_TXT_03001"), nColorMatchingPoint - m_nPuzzleCount));
			m_nPuzzleCount = nColorMatchingPoint;

			// 점수 업데이트
			UpdateWisdomTemplePanel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingMonsterCreated(PDWisdomTempleColorMatchingMonsterInstance pdWisdomTempleColorMatchingMonsterInstance)
	{
		// 제사장 몬스터 생성 토스트
		OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.ColorMatchingMonsterSpawnGuideTitle, CsDungeonManager.Instance.WisdomTemple.ColorMatchingMonsterSpawnGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleColorMatchingMonsterKill(PDWisdomTempleColorMatchingObjectInstance[] aPDWisdomTempleColorMatchingObjectInstance, int nColorMatchingPoint)
	{

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A105_TXT_03001"), nColorMatchingPoint - m_nPuzzleCount));
		m_nPuzzleCount = nColorMatchingPoint;

		// 점수 업데이트
		UpdateWisdomTemplePanel();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleFakeTreasureBoxKill(int nRow, int nCol, bool bExistAroundRealTreasureBox)
	{
		if (nRow == 1 && nCol == 1)
		{
			if (bExistAroundRealTreasureBox)
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A105_TXT_03003"));
			}
			else
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A105_TXT_03002"));
			}
		}

		m_nPuzzleCount++;

		// 점수 업데이트
		UpdateWisdomTemplePanel();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleCompleted(bool bLevelUp, long lAcquiredExp, PDWisdomTemplePuzzleRewardObjectInstance[] aPDWisdomTemplePuzzleRewardObjectInstance)
	{
		if (CsDungeonManager.Instance.WisdomTemplePuzzle.PuzzleId == 1)
		{
			OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.PuzzleRewardGuideTitle, CsDungeonManager.Instance.WisdomTemple.PuzzleRewardGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
		}
		else
		{
			OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.FindTreasureBoxSuccessGuideTitle, CsDungeonManager.Instance.WisdomTemple.FindTreasureBoxSuccessGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
		}

		UpdateWisdomTemplePanelPuzzleComplete();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleQuizFail()
	{
		// 퀴즈 오답 가이드
		OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.QuizWrongAnswerGuideTitle, CsDungeonManager.Instance.WisdomTemple.QuizWrongAnswerGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBossMonsterCreated(PDWisdomTempleBossMonsterInstance pdWisdomTempleBossMonsterInstance)
	{
		OpenNpcDungeonGuide(CsDungeonManager.Instance.WisdomTemple.BossMonsterSpawnGuideTitle, CsDungeonManager.Instance.WisdomTemple.BossMonsterSpawnGuideContent, CsDungeonManager.Instance.WisdomTemple.GuideImageName);
		UpdateWisdomTemplePanelBossMonsterCreated();
	}

	#endregion 지혜의 신전

	#region 유적 탈환

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForRuinsReclaimEnter()
	{
		DungeonEnter();

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = false;
		ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventRuinsReclaimEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotate, ClientCommon.PDHero[] aPDHero, ClientCommon.PDMonsterInstance[] aPDMonsterInstance, ClientCommon.PDRuinsReclaimRewardObjectInstance[] aPDRuinReclaimRewardObjectInstance, ClientCommon.PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aPDRuinsReclaimMonsterTransformationCancelObjectInstance, Guid[] aGuidMonsterTransformationHero)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		// 시작 전 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime > 0)
		{
			m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
			StartCoroutine(m_iEnumeratorDungeonReadyTime);
		}
		else
		{
			Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
			toggleDungeon.interactable = true;

			UpdateRuinsReclaimPanelStepStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepStart(PDRuinsReclaimRewardObjectInstance[] aPDRuinsReclaimRewardObjectInstance)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = true;

		UpdateRuinsReclaimPanelStepStart();
	}

	
	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepWaveSkillCast(PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aPDRuinsReclaimMonsterTransformationCancelObjectInstance, PDVector3 pdVector3)
	{
		CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill = CsDungeonManager.Instance.RuinsReclaimStep.GetRuinsReclaimStepWaveSkill(CsDungeonManager.Instance.RuinsReclaimStepWave.WaveNo);

		if (csRuinsReclaimStepWaveSkill != null)
		{
			OpenNpcDungeonGuide(csRuinsReclaimStepWaveSkill.GuideTitle, csRuinsReclaimStepWaveSkill.GuideContent, csRuinsReclaimStepWaveSkill.GuideImageName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.RuinsReclaim));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.RuinsReclaim));
	}

	#endregion 유적 탈환

    #region 무한 대전

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForInfiniteWarEnter()
    {
        DungeonEnter();
        AllPanelQuestOff();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarEnter(Guid guid, ClientCommon.PDVector3 pDVector3, float flRotationY, ClientCommon.PDHero[] pDHeroes, ClientCommon.PDMonsterInstance[] pDMonsterInstance, ClientCommon.PDInfiniteWarBuffBoxInstance[] pDInfiniteWarBuffBoxInstance)
    {
        InitializePanelInfiniteWar(pDHeroes);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarStart()
    {
        CsInfiniteWar csInfiniteWar = CsGameData.Instance.InfiniteWar;
        OpenNpcDungeonGuide(csInfiniteWar.StartGuideTitle, csInfiniteWar.StartGuideContent, csInfiniteWar.GuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.InfiniteWar));

        m_trPanelInfiniteWar.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.InfiniteWar));

        m_trPanelInfiniteWar.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.InfiniteWar));

        m_trPanelInfiniteWar.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarMonsterSpawn(PDInfiniteWarMonsterInstance[] pDInfiniteWarMonsterInstance)
    {
        CsInfiniteWar csInfiniteWar = CsGameData.Instance.InfiniteWar;
        OpenNpcDungeonGuide(csInfiniteWar.MonsterSpawnGuideTitle, csInfiniteWar.MonsterSpawnGuideContent, csInfiniteWar.GuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBuffBoxCreated(PDInfiniteWarBuffBoxInstance[] pDInfiniteWarBuffBoxInstance)
    {
        CsInfiniteWar csInfiniteWar = CsGameData.Instance.InfiniteWar;
        OpenNpcDungeonGuide(csInfiniteWar.BuffCreationGuideTitle, csInfiniteWar.BuffCreationGuideContent, csInfiniteWar.GuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarPointAcquisition()
    {
        UpdatePanelInfiniteWar();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroInfiniteWarPointAcquisition()
    {
        UpdatePanelInfiniteWar();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroEnter(SEBHeroEnterEventBody eventBody)
    {
        Transform trRankList = m_trPanelInfiniteWar.Find("ImageBackground/RankList");

        if (CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar && trRankList != null)
        {
            Transform trImageRank = null;

            for (int i = 0; i < trRankList.childCount; i++)
            {
                trImageRank = trRankList.GetChild(i);

                if (trImageRank.gameObject.activeSelf)
                {
                    continue;
                }
                else
                {
                    Text textRank = trImageRank.Find("TextRank").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textRank);
                    textRank.text = (i + 1).ToString();

                    Text textName = trImageRank.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textName);
                    textName.text = eventBody.hero.name;

                    Text textPoint = trImageRank.Find("TextPoint").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textPoint);
                    textPoint.text = "0";

                    trImageRank.gameObject.SetActive(true);

                    break;
                }
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroExit(SEBHeroExitEventBody eventBody)
    {
        Transform trRankList = m_trPanelInfiniteWar.Find("ImageBackground/RankList");

        if (CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar && trRankList != null)
        {
            if (CsDungeonManager.Instance.DicInfiniteWarHeroPoint.ContainsKey(eventBody.heroId))
            {
                CsDungeonManager.Instance.RemoveInfiniteWarHero(eventBody.heroId);
                UpdatePanelInfiniteWar();
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    #endregion 무한 대전

	#region 공포의 제단

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForFearAltarEnter()
	{
		DungeonEnter();

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = false;
		ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarEnter(Guid guidPlaceInstanceId, PDVector3 position, float flRotationY, PDHero[] aPDHero, PDMonsterInstance[] aPDMonsterInstance)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		// 시작 전 입장했을 경우
		if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime > 0)
		{
			m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
			StartCoroutine(m_iEnumeratorDungeonReadyTime);
		}
		else
		{
			Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
			toggleDungeon.interactable = true;

			// 목표
			UpdateFearAltarPanel();

			// 가이드
			OpenNpcDungeonGuide(CsDungeonManager.Instance.FearAltarStageWave.GuideTitle, CsDungeonManager.Instance.FearAltarStageWave.GuideContent, CsDungeonManager.Instance.FearAltarStageWave.GuideImageName);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarWaveStart(PDFearAltarMonsterInstance[] aPDFearAltarMonsterInstance, PDFearAltarHalidomMonsterInstance pdFearAltarHalidomMonsterInstance)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = true;

		// 목표
		if (CsDungeonManager.Instance.FearAltarStageWave.WaveNo == 1)
		{
			UpdateFearAltarPanel();
		}

		// 가이드
		OpenNpcDungeonGuide(CsDungeonManager.Instance.FearAltarStageWave.GuideTitle, CsDungeonManager.Instance.FearAltarStageWave.GuideContent, CsDungeonManager.Instance.FearAltarStageWave.GuideImageName);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.FearAltar));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.FearAltar));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.FearAltar));
	}

	#endregion 공포의 제단

    #region 전쟁의 기억

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForWarMemoryEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryEnter(Guid guidPlaceInstanceId, PDVector3 position, float flRotationY, PDHero[] aPDHero, PDMonsterInstance[] aPDMonsterInstance, PDWarMemoryTransformationObjectInstance[] aPDWarMemoryTransformationObjectInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        // 시작 전 입장했을 경우
        if (CsDungeonManager.Instance.MultiDungeonRemainingStartTime > 0)
        {
            m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
            StartCoroutine(m_iEnumeratorDungeonReadyTime);
        }
        else
        {
            Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
            toggleDungeon.interactable = true;

            // 목표
            UpdateWarMemoryPanel();

            // 가이드
            if (CsDungeonManager.Instance.WarMemoryWave != null)
            {
                OpenNpcDungeonGuide(CsDungeonManager.Instance.WarMemoryWave.GuideTitle, CsDungeonManager.Instance.WarMemoryWave.GuideContent, CsDungeonManager.Instance.WarMemoryWave.GuideImageName);
            }

            m_trPanelDungeon.gameObject.SetActive(true);
            UpdateWarMemoryPanelDungeon();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryWaveStart(PDWarMemoryMonsterInstance[] aPDWarMemoryMonsterInstance, PDWarMemoryTransformationObjectInstance[] aPDWarMemoryTransformationObjectInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        // 목표
        UpdateWarMemoryPanel();

        // 가이드
        if (CsDungeonManager.Instance.WarMemoryWave != null)
        {
            OpenNpcDungeonGuide(CsDungeonManager.Instance.WarMemoryWave.GuideTitle, CsDungeonManager.Instance.WarMemoryWave.GuideContent, CsDungeonManager.Instance.WarMemoryWave.GuideImageName);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryWaveCompleted()
    {
        UpdateWarMemoryPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.WarMemory));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.WarMemory));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.WarMemory));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryMonsterSummon(PDWarMemorySummonMonsterInstance[] arrPDWarMemorySummonMonsterInstance)
    {
        OpenNpcDungeonGuide(CsDungeonManager.Instance.WarMemory.MonsterSummonGuideTitle, CsDungeonManager.Instance.WarMemory.MonsterSummonGuideContent, CsDungeonManager.Instance.WarMemory.TransformationGuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryTransformationObjectInteractionFinished(int nObjectId, long lInstanceId, long[] arrRemovedAbnormalStateEffects)
    {
        OpenNpcDungeonGuide(CsDungeonManager.Instance.WarMemory.TransformationGuideTitle, CsDungeonManager.Instance.WarMemory.TransformationGuideContent, CsDungeonManager.Instance.WarMemory.TransformationGuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroWarMemoryTransformationObjectInteractionFinished(Guid guidHeroId, long lObjectInstanceId, int nMaxHp, int nHp, long[] arrRemovedAbnormalStateEffects)
    {
        OpenNpcDungeonGuide(CsDungeonManager.Instance.WarMemory.TransformationGuideTitle, CsDungeonManager.Instance.WarMemory.TransformationGuideContent, CsDungeonManager.Instance.WarMemory.TransformationGuideImageName);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryPointAcquisition()
    {
        UpdateWarMemoryPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroWarMemoryPointAcquisition()
    {
        UpdateWarMemoryPanelDungeon();
    }

    #endregion 전쟁의 기억

    #region 오시리스 룸

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForOsirisRoomEnter()
    {
        DungeonEnter();
        AllPanelQuestOff();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomEnter(Guid placeInstanceId, PDVector3 position, float rotationY)
    {
        m_nOsirisRoomRemainingMonsterCount = 0;
        
        UpdatePanelOsirisRoom();
        m_trPanelOsirisRoom.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomWaveStart()
    {
        UpdatePanelOsirisRoom();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.OsirisRoom));

        m_trPanelOsirisRoom.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.OsirisRoom));

        m_trPanelOsirisRoom.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.OsirisRoom));

        m_trPanelOsirisRoom.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomRewardGoldAcquisition()
    {
        m_nOsirisRoomRemainingMonsterCount--;
        UpdatePanelOsirisRoom();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomMonsterSpawn(PDOsirisRoomMonsterInstance pDOsirisRoomMonsterInstance)
    {
        m_nOsirisRoomRemainingMonsterCount++;
        UpdatePanelOsirisRoom();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOsirisRoomMonsterKillFail()
    {
        m_nOsirisRoomRemainingMonsterCount--;
        UpdatePanelOsirisRoom();
    }

    #endregion 오시리스 룸

	#region 전기퀘스트던전

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForBiographyQuestDungeonEnter()
	{
		DungeonEnter();

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = false;
		ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonEnter(Guid guidPlaceInstanceId, PDVector3 vtPosition, float flRotationY)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.BiographyQuestDungeon.StartDelayTime);
		StartCoroutine(m_iEnumeratorDungeonReadyTime);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonExit(int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonAbandon(int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonBanished(int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonWaveStart(PDBiographyQuestDungeonMonsterInstance[] aPDBiographyQuestDungeonMonsterInstance)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		// 패널 업데이트
		string strMonsterName = null;

		if (CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetType == 2)
		{
			for (int i = 0; i < aPDBiographyQuestDungeonMonsterInstance.Length; i++)
			{
				if (aPDBiographyQuestDungeonMonsterInstance[i].arrangeKey == CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetArrangeKey)
				{
					CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(aPDBiographyQuestDungeonMonsterInstance[i].monsterId);

					if (csMonsterInfo != null)
					{
						strMonsterName = csMonsterInfo.Name;
					}
				}
			}
		}

		UpdateBiographyQuestDungeonPanel(strMonsterName);
	}

	//---------------------------------------------------------------------------------------------------
	#endregion 전기퀘스트던전

    #region 용의 둥지

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForDragonNestEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance, Guid[] aTrapHeros)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.MultiDungeonRemainingStartTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestStepStart(PDDragonNestMonsterInstance[] aMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateDragonNestDungeonPanel();

        if (CsDungeonManager.Instance.DragonNestStep.GuideContent == "")
        {

        }
        else
        {
            OpenNpcDungeonGuide(CsDungeonManager.Instance.DragonNestStep.GuideTitle, CsDungeonManager.Instance.DragonNestStep.GuideContent, CsDungeonManager.Instance.DragonNestStep.GuideImageName);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestExit(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.DragonNest));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestAbandon(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.DragonNest));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestBanished(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.DragonNest));
    }

    #endregion 용의 둥지

    #region 무역선 탈환

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForTradeShipEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        m_trPanelDungeon.gameObject.SetActive(true);
        UpdateTradeShipDungeonPanel();
        UpdateTradeShipPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipExit(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.TradeShip));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipAbandon(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.TradeShip));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipBanished(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.TradeShip));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipStepStart(PDTradeShipMonsterInstance[] apMonInst, PDTradeShipAdditionalMonsterInstance[] apAddMonInst, PDTradeShipObjectInstance[] apObjInst)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateTradeShipDungeonPanel();
        UpdateTradeShipPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTradeShipPointAcquisition()
    {
        UpdateTradeShipPanelDungeon();
    }

    #endregion 무역선 탈환

    #region 앙쿠의 무덤

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForAnkouTombEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        m_nAnkouTombWaveNo = 0;

        m_trPanelDungeon.gameObject.SetActive(true);

        UpdateAnkouTombDungeonPanel();
        UpdateAnkouTombPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombExit(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AnkouTomb));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombAbandon(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AnkouTomb));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombBanished(int nPrevContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.AnkouTomb));

        if (m_trPanelDungeon == null)
        {
            return;
        }
        else
        {
            m_trPanelDungeon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombWaveStart(PDAnkouTombMonsterInstance[] arrAnkouTombMonsterInstance, int nWaveNo)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        m_nAnkouTombWaveNo = nWaveNo;

        UpdateAnkouTombDungeonPanel();
        UpdateAnkouTombPanelDungeon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombPointAcquisition()
    {
        UpdateAnkouTombPanelDungeon();
    }

    #endregion 앙쿠의 무덤

	#region 팀 전장
	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForTeamBattlefieldEnter()
	{
		DungeonEnter();

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = false;
		ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldEnter(Guid guidPlaceInstanceId, PDVector3 position, float flRotationY, PDHero[] aPDHeroes)
	{
		//if (m_iEnumeratorDungeonReadyTime != null)
		//{
		//    StopCoroutine(m_iEnumeratorDungeonReadyTime);
		//    m_iEnumeratorDungeonReadyTime = null;
		//}

		//m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsGameData.Instance.TeamBattlefield.EnterWaitingTime);
		//StartCoroutine(m_iEnumeratorDungeonReadyTime);

		Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
		toggleDungeon.interactable = true;

		m_trPanelDungeon.gameObject.SetActive(true);

		// 패널 업데이트
		//UpdateAnkouTombDungeonPanel();
		//UpdateAnkouTombPanelDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldExit(int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.TeamBattlefield));

		if (m_trPanelDungeon == null)
		{
			return;
		}
		else
		{
			m_trPanelDungeon.gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldAbandon(bool bLevelUp, long lAcquiredExp, int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.TeamBattlefield));

		if (m_trPanelDungeon == null)
		{
			return;
		}
		else
		{
			m_trPanelDungeon.gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldBanished(int nPrevContinentId)
	{
		DisplayNpcGuideDialog(false);

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
		StartCoroutine(DungeonShortCut(EnDungeon.TeamBattlefield));

		if (m_trPanelDungeon == null)
		{
			return;
		}
		else
		{
			m_trPanelDungeon.gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldPlayWaitStart(PDTeamBattlefieldMember[] aPDTeamBattlefieldMember)
	{
		if (m_iEnumeratorDungeonReadyTime != null)
		{
			StopCoroutine(m_iEnumeratorDungeonReadyTime);
			m_iEnumeratorDungeonReadyTime = null;
		}

		m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsGameData.Instance.TeamBattlefield.StartDelayTime);
		StartCoroutine(m_iEnumeratorDungeonReadyTime);


	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldPointAcquisition()
	{
		// 패널 업데이트
		//UpdateAnkouTombDungeonPanel();
		//UpdateAnkouTombPanelDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroTeamBattlefieldPointAcquisition()
	{
		// 패널 업데이트
		//UpdateAnkouTombDungeonPanel();
		//UpdateAnkouTombPanelDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	#endregion 팀 전장

	#region 고대유물의 방

	//---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomStart(PDArtifactRoomMonsterInstance[] pDArtifactRoomMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateArtifactRoomDungeonPanel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForArtifactRoomEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomEnter(Guid guid, PDVector3 pDVector3, float flRotationY)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.ArtifactRoom.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ArtifactRoom));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ArtifactRoom));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ArtifactRoom));
    }

    #endregion 고대유물의 방

    #region 지하미로

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnter(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        DungeonEnter();
        UpdateUndergroundMazeDungeonPanel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnterForUndergroundMazeRevive(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        UpdateUndergroundMazeDungeonPanel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazePortalExit(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        UpdateUndergroundMazeDungeonPanel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnterForTransmission(Guid guid, PDVector3 pDVector3, float flRotationY, PDHero[] pDHero, PDUndergroundMazeMonsterInstance[] pDUndergroundMazeMonsterInstance)
    {
        UpdateUndergroundMazeDungeonPanel();
    }

    #endregion 지하미로

    #region 경험치던전
    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ExpDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ExpDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.ExpDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventLakMonsterDead()
    {
        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.ExpDungeon.GuideTitle, CsDungeonManager.Instance.ExpDungeon.LakChargeMonsterKillContent, CsDungeonManager.Instance.ExpDungeon.GuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonWaveStart(PDExpDungeonMonsterInstance[] pDExpDungeonMonsterInstance, PDExpDungeonLakChargeMonsterInstance pDExpDungeonLakChargeMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        m_flExpWaveRemainingTime = CsDungeonManager.Instance.ExpDungeonDifficultyWave.WaveLimitTime + Time.realtimeSinceStartup;
        UpdateExpDungeonPanel();

        if (CsDungeonManager.Instance.ExpDungeonDifficultyWave.WaveNo == 1)
        {
            if (m_IEnumeratorDungeonGuide != null)
            {
                StopCoroutine(m_IEnumeratorDungeonGuide);
                m_IEnumeratorDungeonGuide = null;
            }

            m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.ExpDungeon.GuideTitle, CsDungeonManager.Instance.ExpDungeon.StartGuideContent, CsDungeonManager.Instance.ExpDungeon.GuideImageName);
            StartCoroutine(m_IEnumeratorDungeonGuide);
        }

        if (pDExpDungeonLakChargeMonsterInstance != null)
        {
            if (m_IEnumeratorDungeonGuide != null)
            {
                StopCoroutine(m_IEnumeratorDungeonGuide);
                m_IEnumeratorDungeonGuide = null;
            }

            m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.ExpDungeon.GuideTitle, CsDungeonManager.Instance.ExpDungeon.LakChargeMonsterAppearContent, CsDungeonManager.Instance.ExpDungeon.GuideImageName);
            StartCoroutine(m_IEnumeratorDungeonGuide);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonEnter(PDVector3 pDVector3, float flLotationY, Guid Guid)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.ExpDungeon.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForExpDungeonEnter()
    {
        m_lExpDungeonPoint = 0;

        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    #endregion 경험치 던전

    #region 골드던전

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.GoldDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.GoldDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.IndividualDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.GoldDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonWaveStart(int nWaveNo)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateGoldDungeonPanel();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsDungeonManager.Instance.GoldDungeonStepWave.GuideTitle, CsDungeonManager.Instance.GoldDungeonStepWave.GuideContent, CsDungeonManager.Instance.GoldDungeonStepWave.GuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonEnter(PDVector3 pDVector3, float flLotationY, Guid Guid)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.GoldDungeon.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGoldDungeonEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    #endregion 골드던전

    #region 스토리던전

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.StoryDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.StoryDungeon));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonAbandon(int nPreviousContinentId)
    {
        DisplayNpcGuideDialog(false);

        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.StoryDungeon));

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonStepStart(PDStoryDungeonMonsterInstance[] apDStoryDungeonMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateStoryDungeonPanel();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcStoryDungeonGuideCoroutine();
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }


    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonEnter(PDVector3 pDVector3, float flLotationY, Guid Guid)
    {
        m_nStoryDungeonNo = CsDungeonManager.Instance.StoryDungeonNo;

        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsDungeonManager.Instance.StoryDungeon.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForStoryDungeonEnter()
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    #endregion 스토리던전

    #region 메인퀘스트 던전
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDungeon(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (CsUIData.Instance.DungeonInNow == EnDungeon.MainQuestDungeon)
            {
                CsMainQuestDungeonManager.Instance.StartAutoPlay();
            }
            else
            {
                CsDungeonManager.Instance.StartAutoPlay();
            }
        }
        else
        {
            if (CsUIData.Instance.DungeonInNow == EnDungeon.MainQuestDungeon)
            {
                CsMainQuestDungeonManager.Instance.StopAutoPlay(this);
            }
            else
            {
                CsDungeonManager.Instance.StopAutoPlay(this);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStopAutoPlay(object objectCalled)
    {
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonAbandon(int nContinentId, bool bIsSceneLoad)
    {
        DisplayNpcGuideDialog(false);
        CsUIData.Instance.DungeonInNow = EnDungeon.None;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nContinentId, bool bIsSceneLoad)
    {
        DisplayNpcGuideDialog(false);
        CsUIData.Instance.DungeonInNow = EnDungeon.None;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonBanished(int nContinentId, bool bIsSceneLoad)
    {
        DisplayNpcGuideDialog(false);
        CsUIData.Instance.DungeonInNow = EnDungeon.None;
    }

    //---------------------------------------------------------------------------------------------------
    //퀘스트 팝업에서 자동실행.
    void OnEventStartQuestAutoPlay(EnQuestCategoryType enQuestCartegoryType, int nIndex)
    {
        switch (enQuestCartegoryType)
        {
            case EnQuestCategoryType.MainQuest:
                m_trMainQuestToggle.GetComponent<Toggle>().isOn = true;
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForMainQuestDungeonEnter(bool bChangeScene)
    {
        CsUIData.Instance.DungeonInNow = EnDungeon.MainQuestDungeon;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForMainQuestDungeonReEnter()
    {
        CsUIData.Instance.DungeonInNow = EnDungeon.MainQuestDungeon;
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;
        UpdatePopupNpcDialog(csMainQuest.StartNpc.Name, csMainQuest.StartText, CancelReEnter, MainQuestDungeonReEnter, CsConfiguration.Instance.GetString("A13_BTN_00005"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
    {
        DungeonEnter();

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = false;
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);

        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        m_iEnumeratorDungeonReadyTime = DungeonReadyTime(CsMainQuestDungeonManager.Instance.MainQuestDungeon.StartDelayTime);
        StartCoroutine(m_iEnumeratorDungeonReadyTime);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStepStart(PDMainQuestDungeonMonsterInstance[] apDMainQuestDungeonMonsterInstance)
    {
        if (m_iEnumeratorDungeonReadyTime != null)
        {
            StopCoroutine(m_iEnumeratorDungeonReadyTime);
            m_iEnumeratorDungeonReadyTime = null;
        }

        Toggle toggleDungeon = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeon.interactable = true;

        UpdateMainQuestDungeonPanel();

        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcMainDungeonGuideCoroutine();
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }

    #endregion 메인퀘스트 던전

    #region 파티
    //---------------------------------------------------------------------------------------------------
    void OnClickOpenParty()
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.AncientRelic)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.PartyMatching);
        }
        else if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.SurroundingHero);
        }
        else if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count == 1)
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.SurroundingHero);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.MyParty);
        }
    }

    ////---------------------------------------------------------------------------------------------------
    //void OnClickOpenMatching()
    //{
    //    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Party, EnSubMenu.PartyMatching);
    //}

    //---------------------------------------------------------------------------------------------------
    void OnClickPartyCall()
    {
        if (CsGameData.Instance.MyHeroInfo.Party.Master.Id == CsGameData.Instance.MyHeroInfo.HeroId)
        {
            CsCommandEventManager.Instance.SendPartyCall();
        }
        else
        {
            CsCommandEventManager.Instance.SendHeroPosition(CsGameData.Instance.MyHeroInfo.Party.Master.Id);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyCreate()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyExit()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyMemberBanish()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyDisband()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyApplicationAccept(CsPartyMember csPartyMember)
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPartyInvitationAccept()
    {
        UpdateParty();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroPosition()
    {
        AutoCancelButtonOpen(EnAutoStateType.Move);
    }

    #endregion 파티

    #region 메인퀘스트

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlay()
    {
        Debug.Log("#@#@ OnEventStartAutoPlay #@#@");
        AutoCancelButtonOpen(EnAutoStateType.MainQuest);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventContinueNextQuest()
	{
		CheckNextQuest(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOnClickPanelDialogAccept()
	{
		OnClickAccept();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOnClickPanelDialogCancel()
	{
		OnClickMainQuestCancel();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller)
    {
        //MainQuest Auto UI Close
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.MainQuest, false);

        m_bDisplayArrow = true;
        DisplayMainQuestArrow();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        // 전직 퀘스트
        UpdateQuestPanel(EnQuestType.JobChange);
        
        // 메인 퀘스트
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;

        if (csMainQuest == null)
        {
            return;
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level < csMainQuest.RequiredHeroLevel)
            {
                return;
            }
            else
            {
                UpdateQuestPanel(EnQuestType.MainQuest);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventAcceptDialog(int nNpcId)
    {
		OpenMainQuestPopup(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
		CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.MainQuest);

		if (CsMainQuestManager.Instance.MainQuest == null ||
			(CsMainQuestManager.Instance.MainQuest.StartNpc == null &&
			(CsMainQuestManager.Instance.PrevMainQuest == null ||
			CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc == null ||
			CsMainQuestManager.Instance.MainQuest.CompletionNpc == null ||
			CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc.NpcId != CsMainQuestManager.Instance.MainQuest.CompletionNpc.NpcId)))
		{
			m_nNpcId = 0;
		}

		//CheckNextQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExecuteDataUpdated(int nProgressCount)
    {
        UpdateQuestPanel(EnQuestType.MainQuest);
    }

    //---------------------------------------------------------------------------------------------------
	void OnEventCompleteDialog(int nNpcId)
    {
		OpenMainQuestPopup(nNpcId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        CloseQuestPopup();
		
        UpdateQuestPanel(EnQuestType.MainQuest);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
        //SwitchMainQuestToggle(false);

        if (CsGameData.Instance.MyHeroInfo.Level < CsMainQuestManager.Instance.MainQuest.RequiredHeroLevel)
        {
			CsMainQuestManager.Instance.StopAutoPlay(this);
            ChangeQuestToggleIsOnDisplay(EnQuestType.MainQuest, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRewardItem(int nRewardIndex)
    {
        CsMainQuestReward csMainQuestReward = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[nRewardIndex];

        if (csMainQuestReward != null && csMainQuestReward.Type != (int)EnMainQuestRewardType.Mount)
        {
            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csMainQuestReward)));
            }
            else
            {
                OpenPopupItemInfo(csMainQuestReward);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMainQuest(Toggle toggleMainQuest)
    {
        DisplayMainQuestArrow();

		m_bSelectedMainQuestPanel = toggleMainQuest.isOn;

        if (toggleMainQuest.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (CsMainQuestManager.Instance.MainQuest == null || 
				CsMainQuestManager.Instance.MainQuest.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level || 
				CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Completed)
            {
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.TodayTask, EnSubMenu.TodayTaskExp);
                ChangeQuestToggleIsOnDisplay(EnQuestType.MainQuest, false);
            }
            else
            {
                CsMainQuestManager.Instance.StartAutoPlay();
                m_bDisplayArrow = false;
            }
        }
        else
        {
            CsMainQuestManager.Instance.StopAutoPlay(this);
            m_bDisplayArrow = true;

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.MainQuest, true);

				if (CsGameEventToIngame.Instance.OnEventCheckQuestAreaInHero())
				{
					if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None ||
						CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed)
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_MQGUIDE_0"));
					}
					else
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_MQGUIDE_" + CsMainQuestManager.Instance.MainQuest.Type.ToString()));
					}
				}
				else
				{
					// 경로를 따라 목표 지점으로 이동하세요
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_MQMOVE"));
				}
				
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMainQuestCancel()
    {
		CloseQuestPopup();
        ChangeQuestToggleIsOn(EnQuestType.MainQuest, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAccept()
    {
        if (CsMainQuestManager.Instance.IsAccepted)
        {
            //퀘스트완료 진행 - 인벤토리 여유자리 있는지 체크
            bool bComplete = true;

            for (int i = 0; i < CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList.Count; i++)
            {
                CsMainQuestReward csMainQuestReward = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i];

                switch ((EnMainQuestRewardType)csMainQuestReward.Type)
                {
                    case EnMainQuestRewardType.MainGear:
                    case EnMainQuestRewardType.SubGear:

                        if (CsGameData.Instance.MyHeroInfo.InventorySlotCount <= CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
                        {
                            //빈공간 부족
                            bComplete = false;
                            break;
                        }

                        break;

                    case EnMainQuestRewardType.Item:

                        if (CsGameData.Instance.MyHeroInfo.InventorySlotCount <= CsGameData.Instance.MyHeroInfo.InventorySlotList.Count)
                        {
                            if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(csMainQuestReward.Item.ItemId, csMainQuestReward.ItemOwned) < csMainQuestReward.ItemCount)
                            {
                                bComplete = false;
                                break;
                            }
                        }

                        break;
                }
            }

            if (bComplete)
            {
                //퀘스트 완료 진행.
				m_nNpcId = CsMainQuestManager.Instance.MainQuest.CompletionNpc.NpcId;
                CsMainQuestManager.Instance.Complete();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A12_TXT_02008"));
                CloseQuestPopup();
            }
        }
        else
        {
            //퀘스트 수락 진행.
			m_nNpcId = CsMainQuestManager.Instance.MainQuest.StartNpc.NpcId;
            CsMainQuestManager.Instance.Accept();
        }
    }

    #endregion 메인퀘스트

    #region 농장의 위협
    //---------------------------------------------------------------------------------------------------
    void OnClickAcceptThreatOfFarm()
    {
        if (CsThreatOfFarmQuestManager.Instance.IsExecuted)
        {
            CsThreatOfFarmQuestManager.Instance.CompleteQuest();
        }
        else
        {
            CsThreatOfFarmQuestManager.Instance.AcceptQuest();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionAbandoned()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        ResetQuestPanelDisplay(EnQuestType.ThreatOfFarm);
        m_flThreatOfFarmMissionRemainingTime = 0;
        CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlayThreatOfFarm(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.ThreatOfFarm, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlayThreatOfFarm()
    {
        AutoCancelButtonOpen(EnAutoStateType.ThreatOfFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestReset()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        m_flThreatOfFarmMissionRemainingTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionFail()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        ResetQuestPanelDisplay(EnQuestType.ThreatOfFarm);
        m_flThreatOfFarmMissionRemainingTime = 0;
        CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionComplete(bool bLevelUp, long lExp)
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        m_flThreatOfFarmMissionRemainingTime = 0;

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionMonsterSpawned(long lInstanceId, Vector3 vector3, float flRemainingLifetime)
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestAccepted()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);

        ChangeQuestToggleIsOnDisplay(EnQuestType.ThreatOfFarm, CsThreatOfFarmQuestManager.Instance.Auto);
        CloseQuestPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionAccepted()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        ChangeQuestToggleIsOnDisplay(EnQuestType.ThreatOfFarm, CsThreatOfFarmQuestManager.Instance.Auto);
        CloseQuestPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestAcceptDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel && CsThreatOfFarmQuestManager.Instance.ProgressCount < CsGameData.Instance.ThreatOfFarmQuest.LimitCount)
            {
                UpdatePopupThreatOfFarm();
            }
            else
            {
                if (CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc == null)
                {
                    return;
                }
                else
                {
                    UpdateNpcDialog(CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestComplete()
    {
        UpdateQuestPanel(EnQuestType.ThreatOfFarm);
        CloseQuestPopup();
        ResetQuestPanelDisplay(EnQuestType.ThreatOfFarm);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventQuestCompleteDialog()
    {
        UpdatePopupThreatOfFarm();
    }

    #endregion 농장의 위협

    #region 현상금사냥

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestScrollUse()
    {
        if (CsBountyHunterQuestManager.Instance.BountyHunterQuest != null)
        {
            if (m_IEnumeratorDungeonGuide != null)
            {
                StopCoroutine(m_IEnumeratorDungeonGuide);
                m_IEnumeratorDungeonGuide = null;
            }

            m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsBountyHunterQuestManager.Instance.BountyHunterQuest.GuideTitle, CsBountyHunterQuestManager.Instance.BountyHunterQuest.StartGuideContent, CsBountyHunterQuestManager.Instance.BountyHunterQuest.GuideImageName);
            StartCoroutine(m_IEnumeratorDungeonGuide);

            if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
            {
                UpdateQuestPanel(EnQuestType.BountyHunter);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        ChangeQuestToggleIsOnDisplay(EnQuestType.BountyHunter, false);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        // NPC 대화 토스트 띄워주기
        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }
        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsBountyHunterQuestManager.Instance.BountyHunterQuest.GuideTitle, CsBountyHunterQuestManager.Instance.BountyHunterQuest.CompletionGuideContent, CsBountyHunterQuestManager.Instance.BountyHunterQuest.GuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);

        CsBountyHunterQuestManager.Instance.ResetBountyHunter();
        GetQuestPanel(EnQuestType.BountyHunter).gameObject.SetActive(false);
        ResetQuestPanelDisplay(EnQuestType.BountyHunter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestAbandon()
    {
        // 퀘스트 포기
        GetQuestPanel(EnQuestType.BountyHunter).gameObject.SetActive(false);
        ResetQuestPanelDisplay(EnQuestType.BountyHunter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestUpdated()
    {
        UpdateToggleBountyHunterProgressCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlayBountyHunter(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.BountyHunter, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlayBountyHunter()
    {
        AutoCancelButtonOpen(EnAutoStateType.Hunter);
    }

    #endregion 현상금사냥

    #region 위대한 성전

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestNationTransmission()
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

        if (csNpcInfo == null)
            return;

        if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.None ||
            CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
            {
                // 국가 이동 실패
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
            }
            else
            {
                CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsGameData.Instance.MyHeroInfo.Nation.NationId);
            }
        }
        else
        {
            OpenPopupNationTransmission(csNpcInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestNpcDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsHolyWarQuestManager.Instance.HolyWarQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                OpenHolyWarQuestPopup();
            }
            else
            {
                if (CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo == null)
                {
                    return;
                }
                else
                {
                    UpdateNpcDialog(CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.HolyWar);

        CloseQuestPopup();
        ChangeQuestToggleIsOnDisplay(EnQuestType.HolyWar, CsHolyWarQuestManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.HolyWar);
        ResetQuestPanelDisplay(EnQuestType.HolyWar);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    void OnEventHolyWarQuestUpdated()
    {
        UpdateToggleHolyWarQuestKillCount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.HolyWar, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.HolyWar);
    }

    #endregion 위대한 성전

    #region 자동이동버튼

    //---------------------------------------------------------------------------------------------------
    void OnEventAutoStop(EnAutoStateType enAutoStateType)
    {
        if (CsUIData.Instance.AutoStateType == enAutoStateType)
        {
            DisplayOffAutoCancel();
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAutoCancelButtonOpen(EnAutoStateType enAutoStateType)
    {
        AutoCancelButtonOpen(enAutoStateType);
    }

    // 경로 표시 저장
    EnAutoStateType m_enAutoStateType = EnAutoStateType.None;

    //---------------------------------------------------------------------------------------------------
    void AutoCancelButtonOpen(EnAutoStateType enAutoStateType)
    {
        CsUIData.Instance.AutoStateType = enAutoStateType;

        if(enAutoStateType == EnAutoStateType.Move)
        {
            m_buttonAutoCancel.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("PUBLIC_AUTO_1");

            ButtonAutoCancel(enAutoStateType);
        }
        else if (enAutoStateType == EnAutoStateType.Battle)
        {
            m_buttonAutoCancel.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("PUBLIC_AUTO_BATTLE");

            ButtonAutoCancel(enAutoStateType);
        }
        else
        {
            m_enAutoStateType = enAutoStateType;

            if (CsIngameData.Instance.AutoBattleMode == EnBattleMode.None)
            {
                m_buttonAutoCancel.transform.Find("Text").GetComponent<Text>().text = CsConfiguration.Instance.GetString("A04_TXT_00002");
                CsGameEventUIToUI.Instance.OnEventDisplayJoystickEffect(true);

                ButtonAutoCancel(enAutoStateType);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ButtonAutoCancel(EnAutoStateType enAutoStateType)
    {
        m_buttonAutoCancel.onClick.RemoveAllListeners();
        m_buttonAutoCancel.onClick.AddListener(() => OnClickAutoCancel(enAutoStateType));
        m_buttonAutoCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonAutoCancel.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
    void OnEventAutoQuestStart(EnAutoStateType enAutoStateType, int nParam)
    {
        CsUIData.Instance.AutoStateType = enAutoStateType;
        Transform trQuestPanel = null;

        switch (enAutoStateType)
        {
            case EnAutoStateType.None:
                break;

            case EnAutoStateType.MainQuest:
                CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.MainQuest);
                ChangeQuestToggleIsOn(EnQuestType.MainQuest, true);
                break;

            case EnAutoStateType.Move:
                break;

            case EnAutoStateType.Battle:
                break;

            case EnAutoStateType.Hunter:
                trQuestPanel = GetQuestPanel(EnQuestType.BountyHunter);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.BountyHunter, true);
                }
                else
                {
                    CsBountyHunterQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.SecretLetter:
                trQuestPanel = GetQuestPanel(EnQuestType.SecretLetter);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.SecretLetter, true);
                }
                else
                {
                    CsSecretLetterQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.MysteryBox:
                trQuestPanel = GetQuestPanel(EnQuestType.MysteryBox);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.MysteryBox, true);
                }
                else
                {
                    CsMysteryBoxQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.DimensionRaid:
                trQuestPanel = GetQuestPanel(EnQuestType.DimensionRaid);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.DimensionRaid, true);
                }
                else
                {
                    CsDimensionRaidQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.Fishing:
                CsFishingQuestManager.Instance.FishingNpcAutoMove();
                AutoCancelButtonOpen(enAutoStateType);
                break;

            case EnAutoStateType.ThreatOfFarm:
                trQuestPanel = GetQuestPanel(EnQuestType.ThreatOfFarm);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.ThreatOfFarm, true);
                }
                else
                {
                    CsThreatOfFarmQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.HolyWar:
                trQuestPanel = GetQuestPanel(EnQuestType.HolyWar);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.HolyWar, true);
                }
                else
                {
                    CsHolyWarQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.SupplySupport:
                trQuestPanel = GetQuestPanel(EnQuestType.SupplySupport);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.SupplySupport, true);
                }
                else
                {
                    CsSupplySupportQuestManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.GuildMission:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildMission);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.GuildMission, true);
                }
                else
                {
                    CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Mission);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.GuildAlter:
                if (CsGuildManager.Instance.Guild != null)
                {
                    trQuestPanel = GetQuestPanel(EnQuestType.GuildAlter);

                    if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                    {
                        ChangeQuestToggleIsOn(EnQuestType.GuildAlter, true);
                    }
                    else
                    {
                        CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Altar);
                        AutoCancelButtonOpen(enAutoStateType);
                    }
                }
                break;

            case EnAutoStateType.GuildFarm:
                if (CsGuildManager.Instance.Guild != null)
                {
                    trQuestPanel = GetQuestPanel(EnQuestType.GuildFarm);

                    if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                    {
                        ChangeQuestToggleIsOn(EnQuestType.GuildFarm, true);
                    }
                    else if(CsGuildManager.Instance.GuildPlayAutoState != EnGuildPlayState.FarmQuest)
                    {
                        CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.FarmQuest);
                        AutoCancelButtonOpen(enAutoStateType);
                    }
                }
                break;

            case EnAutoStateType.GuildFoodWareHouse:
                if (CsGuildManager.Instance.Guild != null)
                {
                    if (CsGuildManager.Instance.GuildPlayAutoState != EnGuildPlayState.FoodWareHouse)
                    {
                        CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.FoodWareHouse);
                        AutoCancelButtonOpen(enAutoStateType);
                    }
                }
                break;

            case EnAutoStateType.GuildSupplySupport:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildSupplySupport);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.GuildSupplySupport, true);
                }
                else
                {
                    CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.SupplySupport);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.GuildHunting:
                trQuestPanel = GetQuestPanel(EnQuestType.GuildHunting);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.GuildHunting, true);
                }
                else
                {
                    CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Hunting);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.GuildFishing:
                
                if (CsGuildManager.Instance.Guild != null)
                {
                    if (CsGuildManager.Instance.GuildPlayAutoState != EnGuildPlayState.Fishing)
                    {
                        CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Fishing);
                        AutoCancelButtonOpen(enAutoStateType);
                    }
                }
                
                break;

            case EnAutoStateType.NationWar:
                AutoCancelButtonOpen(enAutoStateType);
                break;

            case EnAutoStateType.DailyQuest01:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest01);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.DailyQuest01, true);
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(CsDailyQuestManager.Instance.HeroDailyQuestList[0].Id);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.DailyQuest02:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest02);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.DailyQuest01, true);
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(CsDailyQuestManager.Instance.HeroDailyQuestList[1].Id);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.DailyQuest03:
                trQuestPanel = GetQuestPanel(EnQuestType.DailyQuest03);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.DailyQuest01, true);
                }
                else
                {
                    CsDailyQuestManager.Instance.StartAutoPlay(CsDailyQuestManager.Instance.HeroDailyQuestList[2].Id);
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

            case EnAutoStateType.WeeklyQuest:
                trQuestPanel = GetQuestPanel(EnQuestType.WeeklyQuest);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.WeeklyQuest, true);
                }
                else
                {
                    AutoCancelButtonOpen(enAutoStateType);
                }
                break;

			case EnAutoStateType.TrueHero:
				trQuestPanel = GetQuestPanel(EnQuestType.TrueHero);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
					ChangeQuestToggleIsOn(EnQuestType.TrueHero, true);
                }
                else
                {
					CsTrueHeroQuestManager.Instance.StartAutoPlay();
					AutoCancelButtonOpen(enAutoStateType);
                }
                
				break;

			case EnAutoStateType.SubQuest:

				trQuestPanel = GetQuestPanel(EnQuestType.SubQuest, nParam);

				if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
				{
					ChangeQuestToggleIsOn(EnQuestType.SubQuest, true, nParam);
				}
				else
				{
					CsSubQuestManager.Instance.StartAutoPlay(nParam);
					AutoCancelButtonOpen(enAutoStateType);
				}

				break;

			case EnAutoStateType.Biography:

				trQuestPanel = GetQuestPanel(EnQuestType.Biography, nParam);

				if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
				{
					ChangeQuestToggleIsOn(EnQuestType.Biography, true, nParam);
				}
				else
				{
					CsBiographyManager.Instance.StartAutoPlay(nParam);
					AutoCancelButtonOpen(enAutoStateType);
				}

				break;

			case EnAutoStateType.CreatureFarm:

				trQuestPanel = GetQuestPanel(EnQuestType.CreatureFarm, nParam);

				if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
				{
					ChangeQuestToggleIsOn(EnQuestType.CreatureFarm, true, nParam);
				}
				else
				{
					CsCreatureFarmQuestManager.Instance.StartAutoPlay();
					AutoCancelButtonOpen(enAutoStateType);
				}

				break;

            case EnAutoStateType.JobChange:

                trQuestPanel = GetQuestPanel(EnQuestType.JobChange, nParam);

                if (trQuestPanel != null && trQuestPanel.gameObject.activeSelf)
                {
                    ChangeQuestToggleIsOn(EnQuestType.JobChange, true, nParam);
                }
                else
                {
                    CsJobChangeManager.Instance.StartAutoPlay();
                    AutoCancelButtonOpen(enAutoStateType);
                }

                break;
        }
    }

    #endregion 자동이동버튼

    #region 세리우 보급지원

    //---------------------------------------------------------------------------------------------------
    void OnEventCartChangeNpcDialog(int nWayPoint)
    {
        CsSupplySupportQuestManager.Instance.SendSupplySupportQuestCartChange(nWayPoint);
        DisplayOffAutoCancel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestCartChange(int nOldCartGrade, int nNewCartGrade)
    {
        bool bOldAuto = CsSupplySupportQuestManager.Instance.Auto;

        // 카트가 최대 레벨일 때
        if (nOldCartGrade == CsGameData.Instance.SupplySupportQuest.SupplySupportQuestCartList[CsGameData.Instance.SupplySupportQuest.SupplySupportQuestCartList.Count - 1].CartId)
        {
            // 토스트 호출
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A41_TXT_02001"));
        }
        else
        {
            Debug.Log("###>> OnEventSupplySupportQuestCartChange <<### : nOldCartGrade : " + nOldCartGrade + ", nNewCartGrade : " + nNewCartGrade + ", bOldAuto : " + bOldAuto);
            // 카트체인지 이벤트 호출
            CsSupplySupportQuestManager.Instance.StopAutoPlay(this);
            OpenPopupSupplySupportChange(nOldCartGrade, nNewCartGrade, bOldAuto);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartNpctDialog(bool bQuestAble)
    {
        if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.SupplySupportQuest.RequiredHeroLevel && CsSupplySupportQuestManager.Instance.DailySupplySupportQuestCount < CsGameData.Instance.SupplySupportQuest.LimitCount)
        {
            OpenPopupSupplySupportAccept();
        }
        else
        {
            if (CsSupplySupportQuestManager.Instance.SupplySupportQuest.StartNpc == null)
            {
                return;
            }
            else
            {
                UpdateNpcDialog(CsSupplySupportQuestManager.Instance.SupplySupportQuest.StartNpc);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEndNpctDialog()
    {
        CsSupplySupportQuestReward csSupplySupportQuestReward = CsSupplySupportQuestManager.Instance.SupplySupportQuestCart.SupplySupportQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level);
        OpenPopupSupplySupportClear(csSupplySupportQuestReward.ExpReward.Value, csSupplySupportQuestReward.GoldReward.Value, csSupplySupportQuestReward.ExploitPointReward.Value);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateState()
    {
        UpdateQuestPanel(EnQuestType.SupplySupport);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlaySupplySupport(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.SupplySupport, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlaySupplySupport()
    {
        AutoCancelButtonOpen(EnAutoStateType.SupplySupport);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestAccept(PDSupplySupportQuestCartInstance pDSupplySupportQuestCartInstance)
    {
        UpdateQuestPanel(EnQuestType.SupplySupport);

        OnClickPopupSupplySupportAcceptClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestComplete(bool bLevelUp, long lExp, long lGold, int nExploitPoint)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.SupplySupport);
        ResetQuestPanelDisplay(EnQuestType.SupplySupport);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestFail()
    {
        UpdateQuestPanel(EnQuestType.SupplySupport);
        ResetQuestPanelDisplay(EnQuestType.SupplySupport);

        DisplayOffAutoCancel();
        CsSupplySupportQuestManager.Instance.StopAutoPlay(this);

        //실패시 Npc가이드 토스트
        if (m_IEnumeratorDungeonGuide != null)
        {
            StopCoroutine(m_IEnumeratorDungeonGuide);
            m_IEnumeratorDungeonGuide = null;
        }

        m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(CsSupplySupportQuestManager.Instance.SupplySupportQuest.FailGuideTitle, CsSupplySupportQuestManager.Instance.SupplySupportQuest.FailGuideContent, CsSupplySupportQuestManager.Instance.SupplySupportQuest.FailGuideImageName);
        StartCoroutine(m_IEnumeratorDungeonGuide);
    }


    #endregion 세리우 보급지원

    #region 길드농장 퀘스트

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestNPCDialog()
    {
        CsGuildTerritoryNpc csGuildTerritoryNpc = null;

        switch (CsGuildManager.Instance.GuildFarmQuestState)
        {
            case EnGuildFarmQuestState.None:
                csGuildTerritoryNpc = CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc;
                int nCurrentTime = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

                if (CsGameData.Instance.GuildFarmQuest.StartTime <= nCurrentTime && nCurrentTime <= CsGameData.Instance.GuildFarmQuest.EndTime)
                {
                    if (CsGuildManager.Instance.DailyGuildFarmQuestStartCount < CsGameData.Instance.GuildFarmQuest.LimitCount)
                    {
                        UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue, OnClickCloseGuildAltarNpcDialogPopup, UpdateGuildFarmQuest, string.Format(CsConfiguration.Instance.GetString("A66_BTN_00001"), CsGuildManager.Instance.DailyGuildFarmQuestStartCount, CsGameData.Instance.GuildFarmQuest.LimitCount));
                    }
                    else
                    {
                        UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue);
                    }
                }
                else
                {
                    UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue);
                }
                break;

            case EnGuildFarmQuestState.Executed:
                UpdateGuildFarmQuest();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestAccept(PDHeroGuildFarmQuest pDHeroGuildFarmQuest)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.GuildFarm);
        ChangeQuestToggleIsOn(EnQuestType.GuildFarm, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestComplete(bool bLevelUp, long lExp)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.GuildFarm);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCompleted()
    {
        UpdateQuestPanel(EnQuestType.GuildFarm);
    }

    #endregion 길드농장 퀘스트

	#region 진정한 영웅 퀘스트
	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestDialog()
	{
		if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsTrueHeroQuestManager.Instance.TrueHeroQuest.RequiredVipLevel)
		{
			UpdateNpcDialog(CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc);
		}
		else
		{
			OpenPopupTrueHeroQuest();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickAcceptTrueHeroQuest()
	{
		if (CsTrueHeroQuestManager.Instance.IsExecuted)
		{
			CsTrueHeroQuestManager.Instance.Complete();
		}
		else
		{
			CsTrueHeroQuestManager.Instance.Accept();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickCancelTrueHeroQuest()
	{
		CloseQuestPopup();

		if (CsTrueHeroQuestManager.Instance.Auto)
		{
			CsTrueHeroQuestManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedTrueHeroQuest(bool bIsOn)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
			CsTrueHeroQuestManager.Instance.StartAutoPlay();
		}
		else
		{
			CsTrueHeroQuestManager.Instance.StopAutoPlay(this);

			if (CheckQuestToggleAllOff())
			{
				ChangeQuestToggleIsOn(EnQuestType.TrueHero, true);
			}
			else
			{
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepInteractionFinished()
	{
		UpdateQuestPanel(EnQuestType.TrueHero);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepWaitingCanceled()
	{
		UpdateQuestPanel(EnQuestType.TrueHero);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepComplete()
	{
		UpdateQuestPanel(EnQuestType.TrueHero);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestAccept()
	{
		UpdateQuestPanel(EnQuestType.TrueHero);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.TrueHero, CsTrueHeroQuestManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		CloseQuestPopup();
		UpdateQuestPanel(EnQuestType.TrueHero);
		ResetQuestPanelDisplay(EnQuestType.TrueHero);
		CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

		if (CsTrueHeroQuestManager.Instance.Auto)
		{
			CsWeeklyQuestManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStopAutoPlay(object objectCaller)
	{
		DisplayOffAutoCancel();
		ChangeQuestToggleIsOnDisplay(EnQuestType.TrueHero, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStartAutoPlay()
	{
		AutoCancelButtonOpen(EnAutoStateType.TrueHero);
	}

	#endregion 진정한 영웅 퀘스트

	#region 서브퀘스트
	//---------------------------------------------------------------------------------------------------
	void OnClickCancelSubQuest()
	{
		CloseQuestPopup();

		if (CsSubQuestManager.Instance.Auto)
		{
			CsSubQuestManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickAcceptSubQuest(CsSubQuest csSubQuest)
	{
		int nSubQuestId = csSubQuest.QuestId;

		CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(nSubQuestId);

		if (csHeroSubQuest == null)
		{
			m_nNpcId = csSubQuest.StartNpc.NpcId;
			CsSubQuestManager.Instance.SendSubQuestAccept(nSubQuestId);
		}
		else
		{
			switch (csHeroSubQuest.EnStatus)
			{
				case EnSubQuestStatus.Abandon:
					m_nNpcId = csSubQuest.StartNpc.NpcId;
					CsSubQuestManager.Instance.SendSubQuestAccept(nSubQuestId);
					break;
				case EnSubQuestStatus.Excuted:
					m_nNpcId = csSubQuest.CompletionNpc.NpcId;
					CsSubQuestManager.Instance.SendSubQuestComplete(nSubQuestId);
					break;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestAccept(int nSubQuestId)
	{
		UpdateQuestPanel(EnQuestType.SubQuest, nSubQuestId);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.SubQuest, CsSubQuestManager.Instance.Auto, nSubQuestId);

		CsSubQuest csSubQuest = CsGameData.Instance.GetSubQuest(nSubQuestId);

		if (csSubQuest == null ||
			csSubQuest.StartNpc == null)
		{
			m_nNpcId = 0;
		}

		CheckNextQuest();
	}
	
	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestAbandon(int nSubQuestId)
	{
		UpdateQuestPanel(EnQuestType.SubQuest, nSubQuestId);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.SubQuest, CsSubQuestManager.Instance.Auto, nSubQuestId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestComplete(bool bLevelUp, long lAcquireExp, int nSubQuestId)
	{
		UpdateQuestPanel(EnQuestType.SubQuest, nSubQuestId);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.SubQuest, CsSubQuestManager.Instance.Auto, nSubQuestId);

		CsSubQuest csSubQuest = CsGameData.Instance.GetSubQuest(nSubQuestId);

		if (csSubQuest == null ||
			csSubQuest.CompletionNpc == null)
		{
			m_nNpcId = 0;
		}

		CheckNextQuest();

		if (CsSubQuestManager.Instance.Auto)
		{
			CsSubQuestManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestProgressCountsUpdated(int nSubQuestId)
	{
		UpdateQuestPanel(EnQuestType.SubQuest, nSubQuestId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestsAccepted(int nSubQuestId)
	{
		UpdateQuestPanel(EnQuestType.SubQuest, nSubQuestId);

		ChangeQuestToggleIsOnDisplay(EnQuestType.SubQuest, CsSubQuestManager.Instance.Auto, nSubQuestId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestNpcDialog(int nNpcId)
	{
		CsSubQuest csSubQuest = CsSubQuestManager.Instance.GetSubQuestFromNpc(nNpcId);

		if (csSubQuest != null)
		{
			OpenPopupSubQuest(csSubQuest);
		}
		else
		{
			UpdateNpcDialog(CsGameData.Instance.GetNpcInfo(nNpcId));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestStopAutoPlay(object objectCaller, int nSubQuestId)
	{
		DisplayOffAutoCancel();
		ChangeQuestToggleIsOnDisplay(EnQuestType.SubQuest, false, nSubQuestId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestStartAutoPlay()
	{
		AutoCancelButtonOpen(EnAutoStateType.SubQuest);
	}

	#endregion 서브퀘스트

	#region 전기

	//---------------------------------------------------------------------------------------------------
	void OnClickCancelBiographyQuest()
	{
		CloseQuestPopup();

		if (CsBiographyManager.Instance.Auto)
		{
			CsBiographyManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickAcceptBiographyQuest(CsBiographyQuest csBiographyQuest)
	{
		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(csBiographyQuest.BiographyId);

		if (csHeroBiographyQuest == null ||
			csHeroBiographyQuest.QuestNo < csBiographyQuest.QuestNo)
		{
			CsBiographyManager.Instance.SendBiographyQuestAccept(csBiographyQuest.BiographyId, csBiographyQuest.QuestNo);
		}
		else
		{
			CsBiographyManager.Instance.SendBiographyQuestComplete(csBiographyQuest.BiographyId, csBiographyQuest.QuestNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestStopAutoPlay(object objectCaller, int nBiographyId)
	{
		DisplayOffAutoCancel();
		ChangeQuestToggleIsOnDisplay(EnQuestType.Biography, false, nBiographyId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestStartAutoPlay()
	{
		AutoCancelButtonOpen(EnAutoStateType.Biography);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyNpcDialog(CsBiographyQuest csBiographyQuest)
	{
		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(csBiographyQuest.BiographyId);
		
		if (csHeroBiographyQuest == null ||
			csHeroBiographyQuest.Excuted)
		{
			OpenPopupBiographyQuest(csBiographyQuest);
		}
		else
		{
			if (csHeroBiographyQuest.Completed)
			{
				CsBiographyQuest nextQuest = CsBiographyManager.Instance.GetAcceptableQuest(csBiographyQuest.BiographyId);
				if (nextQuest != null)
				{
					OpenPopupBiographyQuest(nextQuest);
				}
				else
				{
					UpdateNpcDialog(csBiographyQuest.CompletionNpc);
				}
			}
			else
			{
				if (csHeroBiographyQuest.BiographyQuest.Type == 5)
				{
					// 전기퀘스트 던전 재입장
					UpdatePopupNpcDialog(csBiographyQuest.StartNpc.Name, csBiographyQuest.StartDialogue,
						CloseNpcDialogPopup,
						() => BiographyQuestDungeonReEnter(csBiographyQuest.TargetDungeonId),
						CsConfiguration.Instance.GetString("A13_BTN_00005"));
				}
				else
				{
					UpdateNpcDialog(csBiographyQuest.CompletionNpc);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyMissionNpcDialog(CsBiographyQuest csBiographyQuest)
	{
		UpdateNpcDialog(csBiographyQuest.TargetNpc);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestAccept(int nBiographyId)
	{
		UpdateQuestPanel(EnQuestType.Biography, nBiographyId);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.Biography, CsBiographyManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestComplete(bool bLevelUp, long lAcquiredExp, int nBiographyId)
	{
		UpdateQuestPanel(EnQuestType.Biography, nBiographyId);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.Biography, false);

		// 다음 퀘스트가 존재하고, 시작 NPC가 없는 경우 자동 수락
		CsBiographyQuest nextQuest = CsBiographyManager.Instance.GetAcceptableQuest(nBiographyId);

		if (nextQuest != null && nextQuest.StartNpc == null)
		{
			CsBiographyManager.Instance.SendBiographyQuestAccept(nBiographyId, nextQuest.QuestNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestMoveObjectiveComplete(int nBiographyId)
	{
		UpdateQuestPanel(EnQuestType.Biography, nBiographyId);

		// 완료 NPC가 없는 경우 자동 완료
		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(nBiographyId);

		if (csHeroBiographyQuest != null && csHeroBiographyQuest.Excuted && csHeroBiographyQuest.BiographyQuest.CompletionNpc == null)
		{
			CsBiographyManager.Instance.SendBiographyQuestComplete(nBiographyId, csHeroBiographyQuest.BiographyQuest.QuestNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestNpcConversationComplete(int nBiographyId)
	{
		// 완료 NPC가 없는 경우 자동 완료
		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(nBiographyId);

		if (csHeroBiographyQuest != null && csHeroBiographyQuest.Excuted && csHeroBiographyQuest.BiographyQuest.CompletionNpc == null)
		{
			CsBiographyManager.Instance.SendBiographyQuestComplete(nBiographyId, csHeroBiographyQuest.BiographyQuest.QuestNo);
		}
		else
		{
			UpdateQuestPanel(EnQuestType.Biography, nBiographyId);
			OpenPopupBiographyQuest(csHeroBiographyQuest.BiographyQuest);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestProgressCountsUpdated(int nBiographyId)
	{
		UpdateQuestPanel(EnQuestType.Biography, nBiographyId);

		// 완료 NPC가 없는 경우 자동 완료
		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(nBiographyId);

		if (csHeroBiographyQuest != null && csHeroBiographyQuest.Excuted && csHeroBiographyQuest.BiographyQuest.CompletionNpc == null)
		{
			CsBiographyManager.Instance.SendBiographyQuestComplete(nBiographyId, csHeroBiographyQuest.BiographyQuest.QuestNo);
		}
	}

	#endregion 전기

	#region CreatureFarm Quest

	//---------------------------------------------------------------------------------------------------
	void OnClickCancelCreatureFarmQuest()
	{
		CloseQuestPopup();

		if (CsCreatureFarmQuestManager.Instance.Auto)
		{
			CsCreatureFarmQuestManager.Instance.StopAutoPlay(this);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickAcceptCreatureFarmQuest()
	{
		if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.None)
		{
			CsCreatureFarmQuestManager.Instance.Accept();
		}
		else if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
		{
			CsCreatureFarmQuestManager.Instance.Complete();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestAcceptDialog(int nNpcId)
	{
		OpenPopupCreatureFarmQuest();
	}

	//---------------------------------------------------------------------------------------------------

	void OnEventCreatureFarmQuestCompleteDialog(int nNpcId)
	{
		OpenPopupCreatureFarmQuest();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestAccept()
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, CsCreatureFarmQuestManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);

		CloseQuestPopup();

		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionCompleted(bool bLevelUp, long lAcquiredExp)
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);

		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, CsCreatureFarmQuestManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionMoveObjectiveComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);

		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, CsCreatureFarmQuestManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionMonsterSpawned()
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);

		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, CsCreatureFarmQuestManager.Instance.Auto);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionProgressCountUpdated()
	{
		UpdateQuestPanel(EnQuestType.CreatureFarm);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestStartAutoPlay()
	{
		AutoCancelButtonOpen(EnAutoStateType.CreatureFarm);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestStopAutoPlay(object objectCaller)
	{
		DisplayOffAutoCancel();
		ChangeQuestToggleIsOnDisplay(EnQuestType.CreatureFarm, false);
	}	
	
	//---------------------------------------------------------------------------------------------------

	#endregion CreatureFarm Quest

    #region JobChange

    //---------------------------------------------------------------------------------------------------
    void OnClickCancelJobChangeQuest()
    {
        CloseQuestPopup();

        if (CsJobChangeManager.Instance.Auto)
        {
            CsJobChangeManager.Instance.StopAutoPlay(this);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJobChangeQuest(int nDifficulty = 0)
    {
        CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;
        CsJobChangeQuest csJobChangeQuest = null;

        if (csHeroJobChangeQuest == null)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
            {
                return;
            }
            else
            {
                csJobChangeQuest = CsGameData.Instance.JobChangeQuestList.First();
                
                if (csJobChangeQuest == null)
                {
                    return;
                }
                else
                {
                    CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuest.QuestNo, 0);
                }
            }
        }
        else
        {
            csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);

            if (csJobChangeQuest == null)
            {
                return;
            }
            else
            {
                if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Accepted && csJobChangeQuest.TargetCount <= csHeroJobChangeQuest.ProgressCount)
                {
                    CsJobChangeManager.Instance.SendJobChangeQuestComplete(csHeroJobChangeQuest.InstanceId);
                }
                else if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                {
                    if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo)
                    {
                        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuest.QuestNo + 1, nDifficulty);
                    }
                    else
                    {
                        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuest.QuestNo + 1, 0);
                    }
                }
                else if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Failed)
                {
                    if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo)
                    {
                        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuest.QuestNo, nDifficulty);
                    }
                    else
                    {
                        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuest.QuestNo, 0);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.JobChange);

        CloseQuestPopup();

        ChangeQuestToggleIsOnDisplay(EnQuestType.JobChange, CsJobChangeManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestComplete()
    {
        UpdateQuestPanel(EnQuestType.JobChange);

        CloseQuestPopup();

        ChangeQuestToggleIsOnDisplay(EnQuestType.JobChange, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestFailed()
    {
        UpdateQuestPanel(EnQuestType.JobChange);

        ChangeQuestToggleIsOnDisplay(EnQuestType.JobChange, CsJobChangeManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestProgressCountUpdated()
    {
        UpdateQuestPanel(EnQuestType.JobChange);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.JobChange);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.JobChange, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestNpcEndDialog()
    {
        OpenPopupJobChangeQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJobChangeQuestNpcStartDialog(int nQuestNo)
    {
        OpenPopupJobChangeQuest();
    }

    #endregion JobChange

    #region 자동 전투

    //---------------------------------------------------------------------------------------------------
    void OnEventChangeAutoBattleMode(EnBattleMode enBattleMode)
    {
        switch (enBattleMode)
        {
            case EnBattleMode.None:
                if (CsUIData.Instance.AutoStateType == EnAutoStateType.Move)
                {
                    return;
                }
                else
                {
                    DisplayOffAutoCancel();
                }
                break;

            case EnBattleMode.Auto:
                AutoCancelButtonOpen(EnAutoStateType.Battle);
                break;

            case EnBattleMode.Manual:
                AutoCancelButtonOpen(EnAutoStateType.Battle);
                break;
        }
    }

    #endregion 자동 전투
    
    //---------------------------------------------------------------------------------------------------    
	void OnEventCloseAllPopup()
    {
		CloseQuestPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventErrorClear(string strErrorMessage)
    {
        CsGameEventToIngame.Instance.OnEventQuestCompltedError();
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, strErrorMessage);
        CloseQuestPopup();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventReturnScrollUseStart()
	{
		DisplayOffAutoCancel();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventDungeonStopAutoPlay(object objectCaller, EnDungeonPlay enAutoDungeonPlay)
    {
        ChangeQuestToggleIsOnDisplay(EnQuestType.Dungeon, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPrevContinentEnter()
    {
        DungeonExit();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcquisition(long lExpAcq, bool bLevelUp)
    {
        if (CsUIData.Instance.DungeonInNow == EnDungeon.ExpDungeon)
        {
            m_lExpDungeonPoint += lExpAcq;
            UpdateExpDungeonPanel();
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void OnEventDisplayQuestPanel(EnQuestType enQuestType, bool bIsOn, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

        if (bIsOn)
        {
			PlayerPrefs.SetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName, 1);
			PlayerPrefs.Save();
			UpdateQuestPanel(enQuestType, nParam);
        }
        else
        {
			PlayerPrefs.SetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName, 0);
			PlayerPrefs.Save();
			
			Transform trQuestPanel = GetQuestPanel(enQuestType, nParam);

            if (trQuestPanel == null)
            {
                return;
            }
            else
            {
                if (trQuestPanel.GetComponent<Toggle>().isOn)
                {
                    trQuestPanel.GetComponent<Toggle>().isOn = false;
                }

                trQuestPanel.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAttainmentMainQuestAutoStart()
    {
        if (m_trMainQuestToggle.gameObject.activeSelf)
        {
            m_trMainQuestToggle.GetComponent<Toggle>().isOn = true;
        }
    }


    //---------------------------------------------------------------------------------------------------
    void OnValueChangedPanelQuest(Toggle toggleQuest)
    {
        Transform trSelect = toggleQuest.transform.Find("ImageSelect");
        Transform trQuest = m_trQuestPartyPanel.Find("Quest");

        if (toggleQuest.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (m_enLeftBoardTapNow == EnLeftBoardTap.Quest)
            {
                CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId);
                
                if (csContinent != null)
                {
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.QuestList, EnSubMenu.Receipt);
                }
                else
                {
                    Debug.Log("던전에선 사용할 수 없습니다");
                }
            }
            else
            {
                m_enLeftBoardTapNow = EnLeftBoardTap.Quest;
                trSelect.gameObject.SetActive(true);
                trQuest.gameObject.SetActive(true);

                if (CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar)
                {
                    m_trPanelInfiniteWar.gameObject.SetActive(true);
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            trSelect.gameObject.SetActive(false);
            trQuest.gameObject.SetActive(false);

            if (CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar)
            {
                m_trPanelInfiniteWar.gameObject.SetActive(false);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedPanelParty(Toggle toggleParty)
    {
        Transform trSelect = toggleParty.transform.Find("ImageSelect");
        Transform trParty = m_trQuestPartyPanel.Find("Party");
        Transform trMatching = m_trQuestPartyPanel.Find("Matching");

        if (toggleParty.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (m_enLeftBoardTapNow == EnLeftBoardTap.Party)
            {
                OnClickOpenParty();
            }
            else
            {
                m_enLeftBoardTapNow = EnLeftBoardTap.Party;
            }

            if (CsUIData.Instance.DungeonInNow == EnDungeon.AncientRelic)
            {
                UpdateMatchingDIsplay();

                trSelect.gameObject.SetActive(true);
                trMatching.gameObject.SetActive(true);
            }
            else
            {
                UpdateParty();

                trSelect.gameObject.SetActive(true);
                trParty.gameObject.SetActive(true);
            }
        }
        else
        {
            trSelect.gameObject.SetActive(false);
            trParty.gameObject.SetActive(false);
            trMatching.gameObject.SetActive(false);
        }
    }

    #region NPC 상호작용

    //---------------------------------------------------------------------------------------------------
    void OnEventArrivalNpcByAuto(int nNpcId, int nNationId)
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(nNpcId);

        if (csNpcInfo.NpcType == EnNpcType.NationTransmission)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
            {
                // 국가 이동 실패
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
            }
            else
            {
                CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
                CsCommandEventManager.Instance.SendNationTransmission(nNpcId, nNationId);
            }
        }
        else if (csNpcInfo.NpcId == CsGuildManager.Instance.GuildHuntingQuest.QuestNpcId)
        {
            OpenPopupGuildHunting();
            DisplayOffAutoCancel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArrivalFishingNpcDialog()
    {
        DisplayOffAutoCancel();
        CsNpcInfo csNpcInfo = CsGameData.Instance.FishingQuest.NpcInfo;
        UpdatePopupNpcDialog(csNpcInfo.Nick, csNpcInfo.Dialogue);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestNationTransmission()
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

        if (csNpcInfo == null)
            return;

        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
        {
            // 국가 이동 실패
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
        }
        else
        {
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
            CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsGameData.Instance.MyHeroInfo.Nation.NationId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArrivalNpcByTouch(int nNpcId)
    {
        if (CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId) != null)
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(nNpcId);

            if (csNpcInfo == null)
            {
                return;
            }
            else
            {
                CsNpcShop csNpcShop = CsGameData.Instance.NpcShopList.Find(a => a.NpcId == csNpcInfo.NpcId);

                if (csNpcShop == null)
                {
                    UpdateNpcDialog(csNpcInfo);
                }
                else
                {
                    StartCoroutine(LoadPopupNpcShop(csNpcInfo.NpcId));
                }
            }
        }
        // 길드 영지에 있을 경우
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            CsGuildTerritoryNpc csGuildTerritoryNpc = CsGuildManager.Instance.GuildTerritory.GetGuildTerritoryNpc(nNpcId);

            if (csGuildTerritoryNpc == null)
            {
                return;
            }
            else
            {
                if (csGuildTerritoryNpc.DialogueEnabled)
                {
                    UpdateGuildTerritoryNpcDialog(csGuildTerritoryNpc);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNpcDialog(CsNpcInfo csNpcInfo)
    {
        switch (csNpcInfo.NpcType)
        {
            case EnNpcType.Normal:
                UpdatePopupNpcDialog(csNpcInfo.Name, csNpcInfo.Dialogue);
                break;
            case EnNpcType.ContinentTransmission:
                // 현재 캐릭터가 위치한 국가와 캐릭터의 소속 국가가 같을 때에만 대륙 이동 팝업 창을 띄워줌
                if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                {
                    switch (csNpcInfo.ContinentTransmissionExitList.Count)
                    {
                        case 1:
                            UpdatePopupNpcDialog(csNpcInfo.Nick, csNpcInfo.Dialogue, null, () => OnClickContinentTransmission(0, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[0].Name));
                            break;
                        case 2:
                            UpdatePopupNpcDialog(csNpcInfo.Nick, csNpcInfo.Dialogue, null, () => OnClickContinentTransmission(1, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[1].Name),
                                                                                           () => OnClickContinentTransmission(0, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[0].Name));
                            break;
                        case 3:
                            UpdatePopupNpcDialog(csNpcInfo.Nick, csNpcInfo.Dialogue, null, () => OnClickContinentTransmission(2, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[2].Name),
                                                                                           () => OnClickContinentTransmission(1, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[1].Name),
                                                                                           () => OnClickContinentTransmission(0, csNpcInfo), CsConfiguration.Instance.GetString(csNpcInfo.ContinentTransmissionExitList[0].Name));
                            break;
                    }
                }
                break;
            case EnNpcType.NationTransmission:
                OpenPopupNationTransmission(csNpcInfo);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildTerritoryNpcDialog(CsGuildTerritoryNpc csGuildTerritoryNpc)
    {
        UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeTransmissionNpcDialog(int nNpcId)
    {
        CsUndergroundMazeNpcTransmissionEntry csUndergroundMazeNpcTransmissionEntry2;
        CsUndergroundMazeNpcTransmissionEntry csUndergroundMazeNpcTransmissionEntry1;
        CsUndergroundMazeNpcTransmissionEntry csUndergroundMazeNpcTransmissionEntry0;

        CsUndergroundMazeNpc csUndergroundMazeNpc = CsDungeonManager.Instance.UndergroundMazeFloor.GetUndergroundMazeNpc(nNpcId);

        if (csUndergroundMazeNpc != null)
        {
            switch (csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList.Count)
            {
                case 2:
                    csUndergroundMazeNpcTransmissionEntry1 = csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList[1];
                    csUndergroundMazeNpcTransmissionEntry0 = csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList[0];
                    UpdatePopupNpcDialog(csUndergroundMazeNpc.Name, csUndergroundMazeNpc.Dialogue, null, () => OnClickUndergroundMazeTransmission(csUndergroundMazeNpcTransmissionEntry1.NpcId, csUndergroundMazeNpcTransmissionEntry1.Floor), string.Format(CsConfiguration.Instance.GetString("A43_BTN_00007"), csUndergroundMazeNpcTransmissionEntry1.Floor),
                                                                                   () => OnClickUndergroundMazeTransmission(csUndergroundMazeNpcTransmissionEntry0.NpcId, csUndergroundMazeNpcTransmissionEntry0.Floor), string.Format(CsConfiguration.Instance.GetString("A43_BTN_00007"), csUndergroundMazeNpcTransmissionEntry0.Floor));
                    break;
                case 3:
                    csUndergroundMazeNpcTransmissionEntry2 = csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList[2];
                    csUndergroundMazeNpcTransmissionEntry1 = csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList[1];
                    csUndergroundMazeNpcTransmissionEntry0 = csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList[0];
                    UpdatePopupNpcDialog(csUndergroundMazeNpc.Name, csUndergroundMazeNpc.Dialogue, null, () => OnClickUndergroundMazeTransmission(csUndergroundMazeNpcTransmissionEntry2.NpcId, csUndergroundMazeNpcTransmissionEntry2.Floor), string.Format(CsConfiguration.Instance.GetString("A43_BTN_00007"), csUndergroundMazeNpcTransmissionEntry2.Floor),
                                                                                   () => OnClickUndergroundMazeTransmission(csUndergroundMazeNpcTransmissionEntry1.NpcId, csUndergroundMazeNpcTransmissionEntry1.Floor), string.Format(CsConfiguration.Instance.GetString("A43_BTN_00007"), csUndergroundMazeNpcTransmissionEntry1.Floor),
                                                                                   () => OnClickUndergroundMazeTransmission(csUndergroundMazeNpcTransmissionEntry0.NpcId, csUndergroundMazeNpcTransmissionEntry0.Floor), string.Format(CsConfiguration.Instance.GetString("A43_BTN_00007"), csUndergroundMazeNpcTransmissionEntry0.Floor));
                    break;
            }
        }
    }

    #endregion NPC 상호작용

    #region 지하미로

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeBanished(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.UndergroundMaze));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeExit(int nContinentId)
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.TimeLimitDungeon);
        StartCoroutine(DungeonShortCut(EnDungeon.UndergroundMaze));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickUndergroundMazeTransmission(int nNpcId, int nFloor)
    {
        CsDungeonManager.Instance.SendUndergroundMazeTransmission(nNpcId, nFloor);
        CloseNpcDialogPopup();
    }

    #endregion 지하미로

    //---------------------------------------------------------------------------------------------------
    void OnClickContinentTransmission(int nIndex, CsNpcInfo csNpcInfo)
    {
        CsContinentTransmissionExit csContinentTransmissionExit = csNpcInfo.ContinentTransmissionExitList[nIndex];

        // 현재 영웅의 레벨이 이동 지역 요구 레벨 보다 작다면
        if (CsGameData.Instance.MyHeroInfo.Level < csContinentTransmissionExit.Continent.RequiredHeroLevel)
        {
            // 토스트 메세지
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A42_TXT_00001"), csContinentTransmissionExit.Continent.RequiredHeroLevel));
        }
        else
        {
            CloseNpcDialogPopup();
            // 대륙 이동
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.ContinentTransmission;
            CsCommandEventManager.Instance.SendContinentTransmission(csContinentTransmissionExit.NpcId, csContinentTransmissionExit.ExitNo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPanelHide(Image imageIcon)
    {
        m_bPanelHide = !m_bPanelHide;

        CsUiAnimation csUiAnimation = m_trQuestPartyPanel.GetComponent<CsUiAnimation>();

        if (m_bPanelHide)
        {
            csUiAnimation.UIType = EnUIAnimationType.MoveOff;
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUi/ico_arrow08_Right");
        }
        else
        {
            csUiAnimation.UIType = EnUIAnimationType.MoveOn;
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUi/ico_arrow08_Left");
        }

        csUiAnimation.StartAinmation();

        DisplayMainQuestArrow();
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializePanelMainQuest()
    {
        Transform trToggleList = m_trQuestPartyPanel.Find("ToggleList");

        Toggle toggleQuest = trToggleList.Find("ToggleQuest").GetComponent<Toggle>();
        toggleQuest.onValueChanged.RemoveAllListeners();
        toggleQuest.isOn = true;
        toggleQuest.onValueChanged.AddListener((ison) => OnValueChangedPanelQuest(toggleQuest));

        Toggle toggleParty = trToggleList.Find("ToggleParty").GetComponent<Toggle>();
        toggleParty.onValueChanged.RemoveAllListeners();
        toggleParty.onValueChanged.AddListener((ison) => OnValueChangedPanelParty(toggleParty));

        m_buttonAutoCancel = transform.Find("ButtonAutoCancel").GetComponent<Button>();
        m_buttonAutoCancel.onClick.RemoveAllListeners();
        m_buttonAutoCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAutoCancel = m_buttonAutoCancel.transform.Find("Text").GetComponent<Text>();
        textAutoCancel.text = CsConfiguration.Instance.GetString("A04_TXT_00002");
        CsUIData.Instance.SetFont(textAutoCancel);

        Transform m_panelHide = m_trQuestPartyPanel.Find("ButtonPanelHide");
        if (m_panelHide != null)//UNDONE
        {
            var m_buttonPanelHide = m_panelHide.GetComponent<Button>();
            Image ImageIcon = m_buttonPanelHide.transform.Find("Image").GetComponent<Image>();
            m_buttonPanelHide.onClick.RemoveAllListeners();
            m_buttonPanelHide.onClick.AddListener(() => OnClickPanelHide(ImageIcon));
            m_buttonPanelHide.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));
        }

        m_trPanelInfiniteWar = m_trQuestPartyPanel.Find("PanelInfiniteWar");

        m_trPanelDungeon = m_trQuestPartyPanel.Find("PanelDungeon");
        if (m_trPanelDungeon != null)//UNDONE
        {
            Transform trWarMemoryBackground = m_trPanelDungeon.Find("ImageBackground");

            Text textWarMemoryPoint = trWarMemoryBackground.Find("ImageAcquirePoint/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textWarMemoryPoint);
            textWarMemoryPoint.text = CsConfiguration.Instance.GetString("A121_TXT_00001");
        }
        m_trPanelOsirisRoom = m_trQuestPartyPanel.Find("PanelOsirisRoom");

        DisplayMainQuestArrow();
        InitializeParty();
        InitializeMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainQuestArrow()
    {
        Transform trArrow = m_trQuestPartyPanel.Find("Quest/ImageArrow");

        if (m_bPanelHide)
        {
            trArrow.gameObject.SetActive(false);
        }
		else if (CsMainQuestManager.Instance.MainQuest != null &&
					CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.Completed && 
					CsMainQuestManager.Instance.MainQuest.TargetContinent.ContinentId == 1)
        {
            if (trArrow != null)
            {
                trArrow.gameObject.SetActive(m_bDisplayArrow);
            }
            else
            {
                return;
            }
        }
        else
        {
            trArrow.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenMainQuestPopup(int nNpcId)
    {
		Debug.Log("npcId : " + nNpcId);
		CsSubQuest csSubQuest = CsSubQuestManager.Instance.GetSubQuestFromNpc(nNpcId);
		Debug.Log("quest : " + csSubQuest);
		if (csSubQuest != null)
		{
			OpenPopupDialogSelectingQuest(csSubQuest, nNpcId);
		}
		else
		{
			OpenDialogMainQuestNpc();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void CloseQuestPopup()
    {
        if (m_trPopupMainQuest != null)
        {
            m_trPopupMainQuest.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupQuest()
    {
        m_trPopupMainQuest = transform.Find("PopupMainQuest");
        m_csMainQuestAni = m_trPopupMainQuest.GetComponent<CsUiAnimation>();
        m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;
        Transform trBack = m_trPopupMainQuest.Find("ImageBackGround");

        m_textQuestName = trBack.Find("ImageQuestName/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textQuestName);

        m_textNpcName = trBack.Find("TextNpcName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNpcName);

        m_textNpcScript = trBack.Find("Scroll View/Viewport/Content/TextNpcScript").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNpcScript);

        Text textClearReward = trBack.Find("TextClearReward").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClearReward);
        textClearReward.text = CsConfiguration.Instance.GetString("A12_TXT_03001");

        Transform trRewardList = textClearReward.transform.Find("RewardList");

        m_textExp = trRewardList.Find("RewardExp/TextExp").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textExp);

        m_textGold = trRewardList.Find("RewardGold/TextGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGold);

        m_textExploit = trRewardList.Find("RewardExploitPoint/TextExploit").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textExploit);

        m_textGuildMoralPoint = trRewardList.Find("RewardGuildMoralPoint/TextGuildMoralPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuildMoralPoint);

        m_textGuildBuildingPoint = trRewardList.Find("RewardGuildBuildingPoint/TextGuildBuildingPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuildBuildingPoint);

        m_textGuildFund = trRewardList.Find("RewardGuildFund/TextGuildFund").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuildFund);

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        Transform trButtonList = trBack.Find("ButtonList");

        m_buttonAccept = trButtonList.Find("ButtonAccept").GetComponent<Button>();

        m_textAccept = m_buttonAccept.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAccept);

        m_buttonCancel = trButtonList.Find("ButtonCancel").GetComponent<Button>();
        m_buttonCancel.onClick.RemoveAllListeners();

        Text textCancel = m_buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCancel);
        textCancel.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");

        m_bIsFirstQuestPopup = false;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMainQuest()
    {
		if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Accepted)
			return;

		CsGameEventUIToUI.Instance.OnEventMainQuestNpcDialog();
		return;

		//if (m_bIsFirstQuestPopup)
		//{
		//    InitializePopupQuest();
		//}

		//Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

		//for (int i = 0; i < trRewardList.childCount; i++)
		//{
		//    trRewardList.GetChild(i).gameObject.SetActive(false);
		//}

		//trRewardList.Find("RewardExp").gameObject.SetActive(true);
		//trRewardList.Find("RewardGold").gameObject.SetActive(true);

		//if (CsMainQuestManager.Instance.IsAccepted)
		//{
		//    //완료
		//    m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
			
		//    if (CsMainQuestManager.Instance.MainQuest.CompletionNpc != null)
		//    {
		//        m_textNpcName.text = CsMainQuestManager.Instance.MainQuest.CompletionNpc.Name;
		//    }

		//    m_textNpcScript.text = CsMainQuestManager.Instance.MainQuest.CompletionText;

		//    m_buttonCancel.gameObject.SetActive(false);
		//    m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
		//    m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
		//}
		//else
		//{
		//    //수락
		//    m_textQuestName.text = CsMainQuestManager.Instance.MainQuest.Title;

		//    if (CsMainQuestManager.Instance.MainQuest.StartNpc != null)
		//    {
		//        m_textNpcName.text = CsMainQuestManager.Instance.MainQuest.StartNpc.Name;
		//    }

		//    m_textNpcScript.text = CsMainQuestManager.Instance.MainQuest.StartText;

		//    m_buttonCancel.onClick.RemoveAllListeners();
		//    m_buttonCancel.onClick.AddListener(OnClickMainQuestCancel);
		//    m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		//    m_buttonCancel.gameObject.SetActive(true);
		//    m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
		//    m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
		//}

		//m_textExp.text = CsMainQuestManager.Instance.MainQuest.RewardExp.ToString("#,##0");
		//m_textGold.text = CsMainQuestManager.Instance.MainQuest.RewardGold.ToString("#,##0");

		//int nRewardCount = 0;

		//for (int i = 0; i < CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList.Count; i++)
		//{
		//    int nRewardIndex = i;
		//    Transform trReward = trRewardList.Find("ButtonReward" + nRewardCount);
		//    Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();

		//    Text textName = trReward.Find("TextName").GetComponent<Text>();
		//    CsUIData.Instance.SetFont(textName);

		//    Text textCount = trReward.Find("TextCount").GetComponent<Text>();
		//    CsUIData.Instance.SetFont(textCount);

		//    if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.MainGear)
		//    {
		//        if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.JobId == CsGameData.Instance.MyHeroInfo.Job.JobId || CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.JobId == 0)
		//        {
		//            imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.Image);

		//            textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.Name;

		//            textCount.text = "";

		//            nRewardCount++;
		//        }
		//    }
		//    else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.SubGear)
		//    {
		//        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + string.Format("sub_{0}_{1}", CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].SubGear.SubGearId, 1));

		//        textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].SubGear.Name;
		//        textCount.text = "";

		//        nRewardCount++;
		//    }
		//    else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.Item)
		//    {
		//        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Item.Image);

		//        textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Item.Name;

		//        textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].ItemCount);

		//        nRewardCount++;
		//    }
		//    else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.Mount)
		//    {
		//        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/mount_" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Mount.MountId + "_1");

		//        textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Mount.Name;

		//        textCount.text = "";

		//        nRewardCount++;
		//    }
		//    else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.CreatureCard)
		//    {
		//        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_card_" + CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].CreatureCard.CreatureCardGrade.Grade);

		//        textName.text = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].CreatureCard.Name;

		//        textCount.text = "";

		//        nRewardCount++;
		//    }

		//    Button buttonReward = trReward.GetComponent<Button>();
		//    buttonReward.onClick.RemoveAllListeners();
		//    buttonReward.onClick.AddListener(() => OnClickRewardItem(nRewardIndex));
		//    buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		//    trReward.gameObject.SetActive(true);
		//}

		//for (int i = 0; i < 2 - nRewardCount; i++)
		//{
		//    Transform trReward = trRewardList.Find("ButtonReward" + (i + nRewardCount));
		//    trReward.gameObject.SetActive(false);
		//}

		//m_buttonAccept.onClick.RemoveAllListeners();
		//m_buttonAccept.onClick.AddListener(OnClickAccept);
		//m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		//m_trPopupMainQuest.gameObject.SetActive(true);
		//m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsMainQuestReward csMainQuestReward)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        switch ((EnMainQuestRewardType)csMainQuestReward.Type)
        {
            case EnMainQuestRewardType.MainGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.MainGear, false);
                break;
            case EnMainQuestRewardType.SubGear:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.SubGear, false);
                break;
            case EnMainQuestRewardType.Item:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.Item, 0, csMainQuestReward.ItemOwned, -1, false);
                break;
            case EnMainQuestRewardType.CreatureCard:
                m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.CreatureCard);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItem csItem, int nCount, bool bOwned)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItem, nCount, bOwned, -1, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

    #region 농장의 위협

    Text m_textTimeThreatOfFarm;

    //농장의위협 수락 / 완료
    //---------------------------------------------------------------------------------------------------
    void UpdatePopupThreatOfFarm()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");
        
        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        if (CsThreatOfFarmQuestManager.Instance.IsAccepted)
        {
            if (CsThreatOfFarmQuestManager.Instance.IsExecuted)
            {
                m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
                m_textNpcName.text = CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc.Name;
                m_textNpcScript.text = CsThreatOfFarmQuestManager.Instance.Quest.CompletionDialogue;

                m_buttonCancel.gameObject.SetActive(false);
                m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
                m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
            }
            else
            {
                m_textQuestName.text = CsThreatOfFarmQuestManager.Instance.Quest.Title;
                m_textNpcName.text = CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc.Name;
                m_textNpcScript.text = CsThreatOfFarmQuestManager.Instance.Quest.StartDialogue;
                m_buttonCancel.gameObject.SetActive(true);
                m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
                m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
            }
        }
        else
        {
            m_textQuestName.text = CsThreatOfFarmQuestManager.Instance.Quest.Title;
            m_textNpcName.text = CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc.Name;
            m_textNpcScript.text = CsThreatOfFarmQuestManager.Instance.Quest.StartDialogue;

            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
        }

        CsThreatOfFarmQuestReward csThreatOfFarmQuestReward = CsThreatOfFarmQuestManager.Instance.Quest.GetThreatOfFarmQuestReward(CsGameData.Instance.MyHeroInfo.Level);

        Transform trReward = trRewardList.Find("ButtonReward0");

        Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();
        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csThreatOfFarmQuestReward.QuestCompletionItemReward.Item.Image);

        Text textName = trReward.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csThreatOfFarmQuestReward.QuestCompletionItemReward.Item.Name;

        Text textCount = trReward.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), csThreatOfFarmQuestReward.QuestCompletionItemReward.ItemCount);

        Button buttonReward = trReward.GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
		buttonReward.onClick.AddListener(() => OnClickRewardItem(csThreatOfFarmQuestReward.QuestCompletionItemReward.Item.ItemId, csThreatOfFarmQuestReward.QuestCompletionItemReward.ItemCount, csThreatOfFarmQuestReward.QuestCompletionItemReward.ItemOwned));
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        trReward.gameObject.SetActive(true);

        Transform trReward1 = trRewardList.Find("ButtonReward1");
        trReward1.gameObject.SetActive(false);

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickAcceptThreatOfFarm);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonCancel.onClick.RemoveAllListeners();
        m_buttonCancel.onClick.AddListener(OnClickThreatOfFarmCancel);
        m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickThreatOfFarmCancel()
    {
        CloseQuestPopup();

        if (CsThreatOfFarmQuestManager.Instance.Auto)
        {
            CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedThreatOfFarm(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsThreatOfFarmQuestManager.Instance.StartAutoPlay();
        }
        else
        {
            CsThreatOfFarmQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.ThreatOfFarm, true);
            }
            else
            {
                return;
            }
        }
    }

    #endregion 농장의 위협

    #region 현상금 사냥

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleBountyHunterProgressCount()
    {
        CsBountyHunterQuest csBountyHunterQuest = CsBountyHunterQuestManager.Instance.BountyHunterQuest;

        if (csBountyHunterQuest == null)
        {
            return;
        }
        else
        {
			Transform trQuestPanel = GetQuestPanel(EnQuestType.BountyHunter);

			if (trQuestPanel != null)
			{
				Text textTarget = trQuestPanel.Find("TextTarget").GetComponent<Text>();
				textTarget.text = string.Format(CsConfiguration.Instance.GetString(csBountyHunterQuest.TargetContent), CsBountyHunterQuestManager.Instance.ProgressCount, csBountyHunterQuest.TargetCount);
			}

            // 퀘스트 클리어
            if (CsBountyHunterQuestManager.Instance.ProgressCount >= csBountyHunterQuest.TargetCount)
            {
                CsBountyHunterQuestManager.Instance.SendBountyHunterQuestComplete();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedBountyHunter(bool bIson)
    {
        if (bIson)
        {
            // 자동 이동
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsBountyHunterQuestManager.Instance.StartAutoPlay();
        }
        else
        {
            CsBountyHunterQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.BountyHunter, true);
            }
            else
            {
                return;
            }
        }
    }

    #endregion 현상금 사냥

    #region Npc Guide Dialog

	//---------------------------------------------------------------------------------------------------
	void OpenNpcDungeonGuide(string strTitle, string strContent, string strImageName)
	{
		if (m_IEnumeratorDungeonGuide != null)
		{
			StopCoroutine(m_IEnumeratorDungeonGuide);
			m_IEnumeratorDungeonGuide = null;
		}

		m_IEnumeratorDungeonGuide = NpcDungeonGuideCoroutine(strTitle, strContent, strImageName);
		StartCoroutine(m_IEnumeratorDungeonGuide);
	}

    //---------------------------------------------------------------------------------------------------
    IEnumerator NpcDungeonGuideCoroutine(string strNpcName, string strGuide, string strImageName)
    {
        UpdateNpcGuideDialog(strNpcName, strGuide, strImageName);
        DisplayNpcGuideDialog(true);

        yield return new WaitForSeconds(CsGameConfig.Instance.GuideToastDisplayDuration);

        DisplayNpcGuideDialog(false);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator NpcStoryDungeonGuideCoroutine()
    {
        int nNo = 1;

        CsStoryDungeonGuide csStoryDungeonGuide = CsDungeonManager.Instance.StoryDungeonStep.StoryDungeonGuideList.Find(a => a.Step == CsDungeonManager.Instance.StoryDungeonStep.Step && a.No == nNo);

        while (csStoryDungeonGuide != null)
        {
            UpdateNpcGuideDialog(csStoryDungeonGuide.Title, csStoryDungeonGuide.Content, csStoryDungeonGuide.ImageName);
            DisplayNpcGuideDialog(true);

            yield return new WaitForSeconds(CsGameConfig.Instance.GuideToastDisplayDuration);

            DisplayNpcGuideDialog(false);

            yield return new WaitForSeconds(CsDungeonManager.Instance.StoryDungeon.GuideDisplayInterval);

            nNo++;
            csStoryDungeonGuide = CsDungeonManager.Instance.StoryDungeonStep.StoryDungeonGuideList.Find(a => a.Step == CsDungeonManager.Instance.StoryDungeonStep.Step && a.No == nNo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator NpcMainDungeonGuideCoroutine()
    {
        int nNo = 1;

        CsMainQuestDungeonGuide csMainQuestDungeonGuide = CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.MainQuestDungeonGuideList.Find(a => a.Step == CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Step && a.No == nNo);

        while (csMainQuestDungeonGuide != null)
        {
            UpdateNpcGuideDialog(csMainQuestDungeonGuide.Title, csMainQuestDungeonGuide.Content, csMainQuestDungeonGuide.ImageName);
            DisplayNpcGuideDialog(true);

            yield return new WaitForSeconds(CsGameConfig.Instance.GuideToastDisplayDuration);

            DisplayNpcGuideDialog(false);

            yield return new WaitForSeconds(CsMainQuestDungeonManager.Instance.MainQuestDungeon.GuideDisplayInterval);

            nNo++;
            csMainQuestDungeonGuide = CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.MainQuestDungeonGuideList.Find(a => a.Step == CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.Step && a.No == nNo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayNpcGuideDialog(bool bIsOpen)
    {
        Transform trQuestPartyPanel = transform.Find("QuestPartyPanel");
        Transform trDialog = transform.Find("ImageDungeonNpcDialog");

        if (bIsOpen)
        {
            //trQuestPartyPanel.gameObject.SetActive(false);
            trDialog.gameObject.SetActive(true);
            //UpdateDungeonDialog();
        }
        else
        {
            //trQuestPartyPanel.gameObject.SetActive(true);
            trDialog.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNpcGuideDialog(string str1, string str2, string strImageName)
    {
        Transform trDialog = transform.Find("ImageDungeonNpcDialog");

        Image imageNpc = trDialog.Find("ImageNpc").GetComponent<Image>();
        imageNpc.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/" + strImageName);

        //Text textName = trDialog.Find("TextNpcName").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textName);
        //textName.text = str1;

        Text textDialog = trDialog.Find("TextDialog").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDialog);
        textDialog.text = str2;
    }

    #endregion Npc Guide Dialog

    #region NpcDialog Popup

    //---------------------------------------------------------------------------------------------------
    void CloseNpcDialogPopup()
    {
        if (m_trPopupNpcDialog != null)
        {
            m_trPopupNpcDialog.gameObject.SetActive(false);

            Transform trButtonList = m_trPopupNpcDialog.Find("ImageBackGround/ButtonList");

            for (int i = 0; i < trButtonList.childCount; i++)
            {
                Button buttonDialog = trButtonList.GetChild(i).GetComponent<Button>();

                if (buttonDialog != null && !buttonDialog.interactable)
                {
                    buttonDialog.interactable = true;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupNpcDialog()
    {
        m_trPopupNpcDialog = transform.Find("PopupNpcDialog");
        m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;
        Transform trBack = m_trPopupNpcDialog.Find("ImageBackGround");

        m_textDialogNpcName = trBack.Find("TextNpcName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDialogNpcName);

        m_textDialogNpcScript = trBack.Find("Scroll View/Viewport/Content/TextNpcScript").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDialogNpcScript);

        m_bIsFirstNpcDialogPopup = false;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupNpcDialog(string strNpcName, string strScript, UnityAction unityAction0 = null, UnityAction unityAction1 = null, string strButton1 = "", UnityAction unityAction2 = null, string strButton2 = "", UnityAction unityAction3 = null, string strButton3 = "")
    {
        m_bIsAlter = false;
        if (m_bIsFirstNpcDialogPopup)
        {
            InitializePopupNpcDialog();
            m_bIsFirstNpcDialogPopup = false;
        }

        m_trPopupNpcDialog.gameObject.SetActive(true);
        m_trPopupNpcDialog.GetComponent<CsUiAnimation>().StartAinmation();

        m_textDialogNpcName.text = strNpcName;
        m_textDialogNpcScript.text = strScript;

        Transform trBack = m_trPopupNpcDialog.Find("ImageBackGround");
        Transform trButtonList = trBack.Find("ButtonList");

		for (int i = 0; i < trButtonList.childCount; i++)
		{
			trButtonList.GetChild(i).gameObject.SetActive(false);
		}

        //닫기버튼
		trButtonList.Find("ButtonCancel").gameObject.SetActive(true);

        Button buttonCancel = trButtonList.Find("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCancel = buttonCancel.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCancel);
        textCancel.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");

        if (unityAction0 == null)
        {
            buttonCancel.onClick.AddListener(CloseNpcDialogPopup);
        }
        else
        {
            buttonCancel.onClick.AddListener(unityAction0);
        }

        //0번버튼
        Button button0 = trButtonList.Find("Button0").GetComponent<Button>();
        button0.onClick.RemoveAllListeners();
        button0.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton0 = button0.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton0);

        if (unityAction1 == null)
        {
            button0.gameObject.SetActive(false);
        }
        else
        {
            button0.onClick.AddListener(unityAction1);
            button0.gameObject.SetActive(true);

            textButton0.text = strButton1;
        }

        //1번버튼
        Button button1 = trButtonList.Find("Button1").GetComponent<Button>();
        button1.onClick.RemoveAllListeners();
        button1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton1 = button1.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton1);

        if (unityAction2 == null)
        {
            button1.gameObject.SetActive(false);
        }
        else
        {
            button1.gameObject.SetActive(true);
            button1.onClick.AddListener(unityAction2);
            textButton1.text = strButton2;
        }

        //2번버튼
        Button button2 = trButtonList.Find("Button2").GetComponent<Button>();
        button2.onClick.RemoveAllListeners();
        button2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButton2 = button2.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButton2);

        if (unityAction3 == null)
        {
            button2.gameObject.SetActive(false);
        }
        else
        {
            button2.gameObject.SetActive(true);
            button2.onClick.AddListener(unityAction3);
            textButton2.text = strButton3;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OpenPopupDialogSelectingQuest(CsSubQuest csSubQuest, int nNpcId)
	{
		if (m_bSelectedMainQuestPanel)
		{
			UpdatePopupMainQuest();
			return;
		}
		else if (m_nSelectedSubQuest > 0)
		{
			if (csSubQuest != null)
			{
				OpenPopupSubQuest(csSubQuest);
				return;
			}
		}

		m_bIsAlter = false;
		if (m_bIsFirstNpcDialogPopup)
		{
			InitializePopupNpcDialog();
			m_bIsFirstNpcDialogPopup = false;
		}

		m_trPopupNpcDialog.gameObject.SetActive(true);
		m_trPopupNpcDialog.GetComponent<CsUiAnimation>().StartAinmation();

		CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(nNpcId);

		m_textDialogNpcName.text = csNpcInfo.Name;
		m_textDialogNpcScript.text = csNpcInfo.Dialogue;

		Transform trBack = m_trPopupNpcDialog.Find("ImageBackGround");
		Transform trButtonList = trBack.Find("ButtonList");

		for (int i = 0; i < trButtonList.childCount; i++)
		{
			trButtonList.GetChild(i).gameObject.SetActive(false);
		}

		//닫기버튼
		trButtonList.Find("ButtonCancel").gameObject.SetActive(true);

		Button buttonCancel = trButtonList.Find("ButtonCancel").GetComponent<Button>();
		buttonCancel.onClick.RemoveAllListeners();
		buttonCancel.onClick.AddListener(CloseNpcDialogPopup);
		buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textCancel = buttonCancel.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textCancel);
		textCancel.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");

		// 메인퀘스트 세팅
		Transform trButtonMainQuest = trButtonList.Find("Button1");

		trButtonMainQuest.gameObject.SetActive(true);

		Text textMainQuest = trButtonMainQuest.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textMainQuest);
		textMainQuest.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_MAIN"), CsMainQuestManager.Instance.MainQuest.Title);

		Button buttonMainQuest = trButtonMainQuest.GetComponent<Button>();
		buttonMainQuest.onClick.RemoveAllListeners();
		buttonMainQuest.onClick.AddListener(CloseNpcDialogPopup);
		buttonMainQuest.onClick.AddListener(UpdatePopupMainQuest);
		buttonMainQuest.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		// 서브퀘스트 세팅
		Transform trButtonSubQuest = trButtonList.Find("Button0");

		trButtonSubQuest.gameObject.SetActive(true);

		Text text = trButtonSubQuest.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(text);
		text.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csSubQuest.Title);

		Button buttonSubQuest = trButtonSubQuest.GetComponent<Button>();
		buttonSubQuest.onClick.RemoveAllListeners();
		buttonSubQuest.onClick.AddListener(CloseNpcDialogPopup);
		buttonSubQuest.onClick.AddListener(() => OpenPopupSubQuest(csSubQuest));
		buttonSubQuest.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
	}

    //---------------------------------------------------------------------------------------------------
    void MainQuestDungeonReEnter()
    {
        CsMainQuestDungeonManager.Instance.ContinentExit();
        CloseNpcDialogPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void CancelReEnter()
    {
        m_trMainQuestToggle.GetComponent<Toggle>().isOn = false;
        CloseNpcDialogPopup();
    }

    #endregion NpcDialog Popup

    #region NationTransmission Popup

    //---------------------------------------------------------------------------------------------------
    void OpenPopupNationTransmission(CsNpcInfo csNpcInfo)
    {
        Transform trPopupList = GameObject.Find("Canvas2/PopupList").transform;
        Transform trPopupNationTransmission = trPopupList.Find("PopupNationTransmission");

        if (trPopupNationTransmission == null)
        {
            StartCoroutine(LoadPopupNationTransmission(csNpcInfo, trPopupList));
        }
        else
        {
            trPopupNationTransmission.gameObject.SetActive(true);

            CsPopupNationTransmission csPopupNationTransmission = trPopupNationTransmission.GetComponent<CsPopupNationTransmission>();
            csPopupNationTransmission.UpdatePopupNationTransmission(csNpcInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupNationTransmission(CsNpcInfo csNpcInfo, Transform trPopupList)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNationTransmission/PopupNationTransmission");
        yield return resourceRequest;
        m_goPopupNationTransmission = (GameObject)resourceRequest.asset;

        Transform trPopupNationTransmission = Instantiate(m_goPopupNationTransmission, trPopupList).transform;
        trPopupNationTransmission.name = "PopupNationTransmission";

        CsPopupNationTransmission csPopupNationTransmission = trPopupNationTransmission.GetComponent<CsPopupNationTransmission>();
        csPopupNationTransmission.UpdatePopupNationTransmission(csNpcInfo);
    }

    #endregion NationTransmission Popup

    #region Dungeon

    //---------------------------------------------------------------------------------------------------
    void UpdateMainQuestDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsMainQuestDungeonManager.Instance.MainQuestDungeonStep.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateStoryDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.StoryDungeonStep.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.StoryDungeonStep.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExpDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        if (CsDungeonManager.Instance.ExpDungeonDifficultyWave == null)
        {

        }
        else
        {
            Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
            textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.ExpDungeonDifficultyWave.TargetTitle);

            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            textPanelDungeonTarget.text = string.Format(CsDungeonManager.Instance.ExpDungeonDifficultyWave.TargetContent, m_lExpDungeonPoint, (m_flExpWaveRemainingTime - Time.realtimeSinceStartup).ToString("#0"));

            Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
            textPanelDungeonTime.text = "";

            Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
            trDungeonTimeIcon.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGoldDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.GoldDungeonStepWave.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.GoldDungeonStepWave.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateUndergroundMazeDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsGameData.Instance.UndergroundMaze.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsGameData.Instance.UndergroundMaze.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateArtifactRoomDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsGameData.Instance.ArtifactRoom.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsGameData.Instance.ArtifactRoom.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAncientRelicDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.AncientRelicStep.TargetTitle);

        if (CsDungeonManager.Instance.AncientRelicStep.Step == 1)
        {
            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            textPanelDungeonTarget.text = string.Format(CsDungeonManager.Instance.AncientRelicStep.TargetContent, CsDungeonManager.Instance.AncientRelicCurrentPoint, CsDungeonManager.Instance.AncientRelicStep.TargetPoint);
        }
        else
        {
            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            textPanelDungeonTarget.text = CsDungeonManager.Instance.AncientRelicStep.TargetContent;
        }

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSoulConveterDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.SoulCoveterDifficultyWave.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.SoulCoveterDifficultyWave.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateFieldOfHonorDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.FieldOfHonor.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.FieldOfHonor.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateEliteDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.EliteDungeon.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.EliteDungeon.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateProofOfValorDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsGameData.Instance.ProofOfValor.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsGameData.Instance.ProofOfValor.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTemplePanel()
	{
		if (m_bIsFirstInDungeon)
		{
			InitializeDungeonPanel();
			m_bIsFirstInDungeon = false;
		}

		Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
		textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.WisdomTempleStep.TargetContent);

		Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();

		if (CsDungeonManager.Instance.WisdomTempleStep.Type == 1)
		{
			// 퍼즐
			if (CsDungeonManager.Instance.WisdomTemplePuzzle.PuzzleId == 1)
			{
				// 색 맞추기
				textPanelDungeonTarget.text = CsDungeonManager.Instance.WisdomTemplePuzzle.TargetTitle + "\n" + string.Format(CsDungeonManager.Instance.WisdomTemplePuzzle.TargetContent, m_nPuzzleCount, CsDungeonManager.Instance.WisdomTemple.ColorMatchingObjectivePoint);
			}
			else
			{
				// 상자 찾기
				textPanelDungeonTarget.text = CsDungeonManager.Instance.WisdomTemplePuzzle.TargetTitle + "\n" + string.Format(CsDungeonManager.Instance.WisdomTemplePuzzle.TargetContent, m_nPuzzleCount);
			}

		}
		else
		{
			// 퀴즈
			textPanelDungeonTarget.text = CsDungeonManager.Instance.WisdomTempleStep.GetWisdomTempleQuizPoolEntry(m_nQuizNo).Question;
		}

		Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
		textPanelDungeonTime.text = "";

		Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
		trDungeonTimeIcon.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTemplePanelPuzzleComplete()
	{
		Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
		textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.WisdomTemple.PuzzleRewardTargetTitle);

		Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
		textPanelDungeonTarget.text = CsDungeonManager.Instance.WisdomTemple.PuzzleRewardTargetContent;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTemplePanelBossMonsterCreated()
	{
		Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
		textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.WisdomTemple.BossMonsterTargetTitle);

		Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
		textPanelDungeonTarget.text = CsDungeonManager.Instance.WisdomTemple.BossMonsterTargetContent;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateRuinsReclaimPanelStepStart()
	{
		if (m_bIsFirstInDungeon)
		{
			InitializeDungeonPanel();
			m_bIsFirstInDungeon = false;
		}

		Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
		textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.RuinsReclaimStep.TargetTitle);

		Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
		textPanelDungeonTarget.text = CsDungeonManager.Instance.RuinsReclaimStep.TargetContent;

		Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
		textPanelDungeonTime.text = "";

		Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
		trDungeonTimeIcon.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateFearAltarPanel()
	{
		if (m_bIsFirstInDungeon)
		{
			InitializeDungeonPanel();
			m_bIsFirstInDungeon = false;
		}

		Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
		textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.FearAltarStage.TargetTitle);

		Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
		textPanelDungeonTarget.text = CsDungeonManager.Instance.FearAltarStage.TargetContent;

		Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
		textPanelDungeonTime.text = "";

		Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
		trDungeonTimeIcon.gameObject.SetActive(false);
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateWarMemoryPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();

        if (CsDungeonManager.Instance.WarMemoryWave != null)
        {
            string strDungeonTitle = string.Format(CsConfiguration.Instance.GetString("A121_TXT_00007"), CsDungeonManager.Instance.WarMemoryWave.WaveNo);
            textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), strDungeonTitle);

            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            Debug.Log("#@#@ CsDungeonManager.Instance.WarMemoryWave.TargetType : " + CsDungeonManager.Instance.WarMemoryWave.TargetType);
            if (CsDungeonManager.Instance.WarMemoryWave.TargetType == 1)
            {
                textPanelDungeonTarget.text = CsConfiguration.Instance.GetString("A121_TXT_00008");
            }
            else if (CsDungeonManager.Instance.WarMemoryWave.TargetType == 2)
            {
                if (CsDungeonManager.Instance.WarMemoryWave.WaveNo == CsGameData.Instance.WarMemory.WarMemoryWaveList.Count)
                {
                    textPanelDungeonTarget.text = CsConfiguration.Instance.GetString("A121_TXT_00010");
                }
                else
                {
                    textPanelDungeonTarget.text = CsConfiguration.Instance.GetString("A121_TXT_00009");
                }   
            }
        }

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateBiographyQuestDungeonPanel(string strTargetName = null)
	{
		if (m_bIsFirstInDungeon)
		{
			InitializeDungeonPanel();
			m_bIsFirstInDungeon = false;
		}

		if (CsDungeonManager.Instance.BiographyQuestDungeon != null)
		{
			if (CsDungeonManager.Instance.BiographyQuestDungeonWave != null)
			{
				Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
				textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetTitle);

				Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();

				if (CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetType == 2)
				{
					textPanelDungeonTarget.text = string.Format(CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetContent, strTargetName);
				}
				else
				{
					textPanelDungeonTarget.text = CsDungeonManager.Instance.BiographyQuestDungeonWave.TargetContent;
				}
			}
		}

		Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
		textPanelDungeonTime.text = "";

		Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
		trDungeonTimeIcon.gameObject.SetActive(false);
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateDragonNestDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        if (CsDungeonManager.Instance.DragonNestStep != null)
        {
            Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
            textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.DragonNestStep.TargetTitle);

            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            textPanelDungeonTarget.text = CsDungeonManager.Instance.DragonNestStep.TargetContent;
        }

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTradeShipDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        if (CsDungeonManager.Instance.TradeShipStep != null)
        {
            Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
            textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.TradeShipStep.TargetTitle);

            Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
            textPanelDungeonTarget.text = CsDungeonManager.Instance.TradeShipStep.TargetContent;
        }

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAnkouTombDungeonPanel()
    {
        if (m_bIsFirstInDungeon)
        {
            InitializeDungeonPanel();
            m_bIsFirstInDungeon = false;
        }

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00002"), CsDungeonManager.Instance.AnkouTomb.TargetTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = CsDungeonManager.Instance.AnkouTomb.TargetContent;

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        textPanelDungeonTime.text = "";

        Transform trDungeonTimeIcon = m_trToggleDungeonQuest.Find("ImageTime");
        trDungeonTimeIcon.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator DungeonReadyTime(float flDelayTime)
    {
        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        textPanelDungeonTitle.text = CsConfiguration.Instance.GetString("A13_TXT_00004");

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        textPanelDungeonTarget.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00003"), flDelayTime);

        float flRemaningDelayTime = flDelayTime + Time.realtimeSinceStartup;

        while (flRemaningDelayTime - Time.realtimeSinceStartup > 0)
        {
            textPanelDungeonTarget.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00003"), (flRemaningDelayTime - Time.realtimeSinceStartup).ToString("#0"));
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeDungeonPanel()
    {
        Toggle toggleDungeonQuest = m_trToggleDungeonQuest.GetComponent<Toggle>();
        toggleDungeonQuest.onValueChanged.RemoveAllListeners();
        toggleDungeonQuest.onValueChanged.AddListener((ison) => OnValueChangedDungeon(toggleDungeonQuest));

        Text textPanelDungeonTitle = m_trToggleDungeonQuest.Find("TextTitle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPanelDungeonTitle);

        Text textPanelDungeonTarget = m_trToggleDungeonQuest.Find("TextTarget").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPanelDungeonTarget);

        Text textPanelDungeonTime = m_trToggleDungeonQuest.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPanelDungeonTime);
    }

    //---------------------------------------------------------------------------------------------------
    void DungeonEnter()
    {
        AllPanelQuestOff();

        m_bIsInDungeon = true;

        m_bDisplayArrow = false;
        DisplayMainQuestArrow();
        DisplayOffAutoCancel();

        CsGameEventToIngame.Instance.OnEventDungeonEnter();

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");
        trQuestContent.localPosition = Vector3.zero;

        if (CsUIData.Instance.DungeonInNow == EnDungeon.InfiniteWar)
        {
            return;
        }
        else
        {
            UpdateQuestPanel(EnQuestType.Dungeon);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DungeonExit()
    {
        m_bIsInDungeon = false;

        UpdateQuestPanel();
        m_trToggleDungeonQuest.gameObject.SetActive(false);

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");
        trQuestContent.localPosition = Vector3.zero;
    }

    #endregion Dungeon

    #region 파티

    //---------------------------------------------------------------------------------------------------
    void InitializeParty()
    {
        Transform trOpenPartyButton = m_trQuestPartyPanel.Find("Party/ButtonPartyList");

        Button buttonOpenPopupParty = trOpenPartyButton.GetComponent<Button>();
        buttonOpenPopupParty.onClick.RemoveAllListeners();
        buttonOpenPopupParty.onClick.AddListener(OnClickOpenParty);
        buttonOpenPopupParty.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trManagement = trOpenPartyButton.Find("PartyManagement");

        Text textManagement = trManagement.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textManagement);

        m_trPartyMemberList = trOpenPartyButton.Find("PartyList");

        for (int i = 0; i < m_trPartyMemberList.childCount; i++)
        {
            Transform trMember = m_trPartyMemberList.Find("PartyMember" + i);

            Text textLevelName = trMember.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
        }

        m_buttonPartyCall = m_trQuestPartyPanel.Find("Party/ButtonPartyCall").GetComponent<Button>();
        m_buttonPartyCall.onClick.RemoveAllListeners();
        m_buttonPartyCall.onClick.AddListener(OnClickPartyCall);
        m_buttonPartyCall.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_textPartyCall = m_buttonPartyCall.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPartyCall);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateParty()
    {
        if (m_enLeftBoardTapNow != EnLeftBoardTap.Party)
        {
            return;
        }

        m_trQuestPartyPanel.Find("Party").gameObject.SetActive(true);
        m_trQuestPartyPanel.Find("Matching").gameObject.SetActive(false);

        Transform trOpenPartyButton = m_trQuestPartyPanel.Find("Party/ButtonPartyList");

        Transform trManagement = trOpenPartyButton.Find("PartyManagement");
        Text textManagement = trManagement.Find("Text").GetComponent<Text>();

        if (CsGameData.Instance.MyHeroInfo.Party == null)
        {
            trManagement.gameObject.SetActive(true);
            m_trPartyMemberList.gameObject.SetActive(false);
            m_buttonPartyCall.gameObject.SetActive(false);
            textManagement.text = CsConfiguration.Instance.GetString("A36_TXT_00005");
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count == 1)
            {
                trManagement.gameObject.SetActive(true);
                m_trPartyMemberList.gameObject.SetActive(false);
                m_buttonPartyCall.gameObject.SetActive(false);
                textManagement.text = CsConfiguration.Instance.GetString("A36_TXT_00006");
            }
            else
            {
                trManagement.gameObject.SetActive(false);
                m_trPartyMemberList.gameObject.SetActive(true);

                List<CsPartyMember> listcsPartyMemberView = new List<CsPartyMember>();
                listcsPartyMemberView.Clear();

                //내가 파티장인지 검사.
                if (CsGameData.Instance.MyHeroInfo.Party.Master.Id != CsGameData.Instance.MyHeroInfo.HeroId)
                {
                    listcsPartyMemberView.Add(CsGameData.Instance.MyHeroInfo.Party.Master);
                    m_textPartyCall.text = CsConfiguration.Instance.GetString("A36_TXT_00004");

                    m_buttonPartyCall.gameObject.SetActive(CsGameData.Instance.MyHeroInfo.Party.Master.IsLoggedIn);
                }
                else
                {
                    //내가 파티장
                    m_buttonPartyCall.gameObject.SetActive(true);

                    if (CsGameData.Instance.MyHeroInfo.Party.CallRemainingCoolTime - Time.realtimeSinceStartup <= 0)
                    {
                        m_textPartyCall.text = CsConfiguration.Instance.GetString("A36_TXT_00003");
                    }
                }

                //남은 파티원 리스트에 추가
                for (int i = 0; i < CsGameData.Instance.MyHeroInfo.Party.PartyMemberList.Count; i++)
                {
                    if (CsGameData.Instance.MyHeroInfo.Party.PartyMemberList[i].Id != CsGameData.Instance.MyHeroInfo.Party.Master.Id && CsGameData.Instance.MyHeroInfo.Party.PartyMemberList[i].Id != CsGameData.Instance.MyHeroInfo.HeroId)
                    {
                        listcsPartyMemberView.Add(CsGameData.Instance.MyHeroInfo.Party.PartyMemberList[i]);
                    }
                }

                //리스트 디스플레이
                for (int i = 0; i < listcsPartyMemberView.Count; i++)
                {
                    CsPartyMember csPartyMember = listcsPartyMemberView[i];

                    Transform trMember = m_trPartyMemberList.Find("PartyMember" + i);
                    trMember.gameObject.SetActive(true);

                    Image imageJob = trMember.Find("ImageJob").GetComponent<Image>();

                    if (imageJob.sprite.name != "ico_small_emblem_off_" + csPartyMember.Job.JobId)
                    {
                        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csPartyMember.Job.JobId);
                    }

                    Text textLevelName = trMember.Find("TextLevelName").GetComponent<Text>();
                    textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), csPartyMember.Level, csPartyMember.Name);

                    Slider slider = trMember.Find("Slider").GetComponent<Slider>();

                    Text textHp = slider.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textHp);

                    Transform trImageMaster = trMember.Find("ImageMaster");

                    if (csPartyMember.Id == CsGameData.Instance.MyHeroInfo.Party.Master.Id)
                    {
                        trImageMaster.gameObject.SetActive(true);
                    }
                    else
                    {
                        trImageMaster.gameObject.SetActive(false);
                    }

                    Transform trImageOffline = trMember.Find("ImageOffline");

                    if (csPartyMember.IsLoggedIn)
                    {
                        textLevelName.color = CsUIData.Instance.ColorButtonOn;

                        slider.maxValue = csPartyMember.MaxHp;
                        slider.value = csPartyMember.Hp;
                        textHp.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csPartyMember.Hp, csPartyMember.MaxHp);
                        slider.gameObject.SetActive(true);
                        trImageOffline.gameObject.SetActive(false);
                    }
                    else
                    {
                        textLevelName.color = CsUIData.Instance.ColorButtonOff;
                        slider.gameObject.SetActive(false);
                        trImageOffline.gameObject.SetActive(true);
                    }
                }

                //파티원이 없는 슬롯 끄기
                for (int i = 0; i < m_trPartyMemberList.childCount - listcsPartyMemberView.Count; i++)
                {
                    Transform trMember = m_trPartyMemberList.Find("PartyMember" + (i + listcsPartyMemberView.Count));
                    trMember.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeMatching()
    {
        Transform trOpenPartyButton = m_trQuestPartyPanel.Find("Matching/ButtonMatchingList");

        for (int i = 0; i < trOpenPartyButton.childCount; i++)
        {
            Transform trMember = trOpenPartyButton.Find("PartyMember" + i);

            Text textLevelName = trMember.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMatchingDIsplay()
    {
        if (m_enLeftBoardTapNow != EnLeftBoardTap.Party)
        {
            return;
        }

        m_trQuestPartyPanel.Find("Party").gameObject.SetActive(false);
        m_trQuestPartyPanel.Find("Matching").gameObject.SetActive(true);

        Transform trOpenPartyButton = m_trQuestPartyPanel.Find("Matching/ButtonMatchingList");

        for (int i = 0; i < CsGameData.Instance.ListHeroObjectInfo.Count; i++)
        {
            Transform trMember = trOpenPartyButton.Find("PartyMember" + i);
            trMember.gameObject.SetActive(true);

            CsHeroBase csHeroBase = CsGameData.Instance.ListHeroObjectInfo[i].GetHeroBase();

            Image imageJob = trMember.Find("ImageJob").GetComponent<Image>();
            imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroBase.Job.JobId);

            Text textLevelName = trMember.Find("TextLevelName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevelName);
            textLevelName.color = CsUIData.Instance.ColorButtonOn;
            textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), csHeroBase.Level, csHeroBase.Name);

            Slider slider = trMember.Find("Slider").GetComponent<Slider>();
            Text textHp = slider.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textHp);

            slider.maxValue = csHeroBase.MaxHp;
            slider.value = csHeroBase.Hp;
            textHp.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csHeroBase.Hp, csHeroBase.MaxHp);
            slider.gameObject.SetActive(true);
        }

        //파티원이 없는 슬롯 끄기
        for (int i = 0; i < trOpenPartyButton.childCount - CsGameData.Instance.ListHeroObjectInfo.Count; i++)
        {
            Transform trMember = trOpenPartyButton.Find("PartyMember" + (i + CsGameData.Instance.ListHeroObjectInfo.Count));
            trMember.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void PartyCalled(int nContinentId, int nNationId, Vector3 v3Position)
    {
        AutoCancelButtonOpen(EnAutoStateType.Move);
        CsGameEventToIngame.Instance.OnEventPartyCalled(nContinentId, nNationId, v3Position);
    }

    #endregion 파티

    #region SecretLetterQuest

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupSecretLetterQuest()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);
        trRewardList.Find("RewardExp").gameObject.SetActive(true);

        if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.None)
        {
            //수락
            m_textQuestName.text = CsGameData.Instance.SecretLetterQuest.TargetTitle;
            m_textNpcName.text = CsGameData.Instance.SecretLetterQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = string.Format(CsGameData.Instance.SecretLetterQuest.StartDialogue, CsGameData.Instance.GetNation(CsSecretLetterQuestManager.Instance.SecretLetterQuestTargetNationId).Name);

            m_buttonCancel.onClick.RemoveAllListeners();
            m_buttonCancel.onClick.AddListener(OnClickSecretLetterQuestCancel);
            m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_textExp.text = CsGameData.Instance.SecretLetterQuest.SecretLetterQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.SecretLetterGrade.Grade == 1).ExpReward.Value.ToString("#,##0");
            m_textExploit.text = CsGameData.Instance.GetSecretLetterGrade(1).ExploitPointReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;

        }
        else if (CsSecretLetterQuestManager.Instance.SecretLetterState != EnSecretLetterState.Accepted)
        {
            //완료
            m_textQuestName.text = CsGameData.Instance.SecretLetterQuest.TargetTitle;
            m_textNpcName.text = CsGameData.Instance.SecretLetterQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = CsGameData.Instance.SecretLetterQuest.CompletionDialogue;
            m_buttonCancel.gameObject.SetActive(false);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
            m_textExp.text = CsGameData.Instance.SecretLetterQuest.SecretLetterQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.SecretLetterGrade.Grade == CsSecretLetterQuestManager.Instance.PickedLetterGrade).ExpReward.Value.ToString("#,##0");
            m_textExploit.text = CsGameData.Instance.GetSecretLetterGrade(CsSecretLetterQuestManager.Instance.PickedLetterGrade).ExploitPointReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        }

        for (int i = 0; i < 2; i++)
        {
            Transform trReward = trRewardList.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(false);
        }

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickSecretLetterQuest);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenSecretLetterQuestPopup()
    {
        UpdatePopupSecretLetterQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSecretLetterQuest(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Executed)
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A49_TXT_00001"),
                CsConfiguration.Instance.GetString("A49_BTN_00002"), OnClickSecretLetterQuestRetry,
                CsConfiguration.Instance.GetString("A49_BTN_00001"), OnClickSecretLetterQuestRetryCancel, true);

            }
            else
            {
                CsSecretLetterQuestManager.Instance.StartAutoPlay();
            }
        }
        else
        {
            CsSecretLetterQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.SecretLetter, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestAcceptDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.SecretLetterQuest.RequiredHeroLevel)
            {
                if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Executed || CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed)
                {
                    OpenSecretLetterQuestPopup();
                }
                else
                {
                    if (CsSecretLetterQuestManager.Instance.DailySecretLetterQuestStartCount < CsGameData.Instance.SecretLetterQuest.LimitCount)
                    {
                        OpenSecretLetterQuestPopup();
                    }
                    else
                    {
                        if (CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo == null)
                        {
                            return;
                        }
                        else
                        {
                            UpdateNpcDialog(CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo);
                        }
                    }
                }
            }
            else
            {
                if (CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo == null)
                {
                    return;
                }
                else
                {
                    UpdateNpcDialog(CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestMissionDialog()
    {
        //CsSecretLetterQuestManager.Instance.SendSecretLetterPickStart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.SecretLetter);

        CloseQuestPopup();

        ChangeQuestToggleIsOnDisplay(EnQuestType.SecretLetter, CsSecretLetterQuestManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterPickCompleted()
    {
        if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A49_TXT_00003"));
        }

        UpdateQuestPanel(EnQuestType.SecretLetter);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.SecretLetter);
        ResetQuestPanelDisplay(EnQuestType.SecretLetter);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestNationTransmission()
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

        if (csNpcInfo == null)
            return;

        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
        {
            // 국가 이동 실패
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
        }
        else
        {
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;

            if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.None ||
                CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed ||
                CsSecretLetterQuestManager.Instance.LowPickComplete)
            {
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsGameData.Instance.MyHeroInfo.Nation.NationId);
            }
            else
            {
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsSecretLetterQuestManager.Instance.TargetNationId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSecretLetterQuest()
    {
        switch (CsSecretLetterQuestManager.Instance.SecretLetterState)
        {
            case EnSecretLetterState.None:
                CsSecretLetterQuestManager.Instance.SendSecretLetterQuestAccept();
                break;
            case EnSecretLetterState.Executed:
                CsSecretLetterQuestManager.Instance.SendSecretLetterQuestComplete();
                break;
            case EnSecretLetterState.Completed:
                CsSecretLetterQuestManager.Instance.SendSecretLetterQuestComplete();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSecretLetterQuestRetry()
    {
        CsSecretLetterQuestManager.Instance.LowPickComplete = false;
        AutoCancelButtonOpen(EnAutoStateType.SecretLetter);
        CsSecretLetterQuestManager.Instance.StartAutoPlay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSecretLetterQuestRetryCancel()
    {
        CsSecretLetterQuestManager.Instance.LowPickComplete = true;
        AutoCancelButtonOpen(EnAutoStateType.SecretLetter);
        CsSecretLetterQuestManager.Instance.StartAutoPlay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSecretLetterQuestCancel()
    {
        CloseQuestPopup();

        if (CsSecretLetterQuestManager.Instance.Auto)
        {
            CsSecretLetterQuestManager.Instance.StopAutoPlay(this);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.SecretLetter, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.SecretLetter);
    }

    #endregion SecretLetterQuest

    #region MysteryBoxQuest

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupMysteryBox()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");
        
        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);
        trRewardList.Find("RewardExp").gameObject.SetActive(true);

        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.None)
        {
            //수락
            m_textQuestName.text = CsGameData.Instance.MysteryBoxQuest.TargetTitle;
            m_textNpcName.text = CsGameData.Instance.MysteryBoxQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = CsGameData.Instance.MysteryBoxQuest.StartDialogue;

            m_buttonCancel.onClick.RemoveAllListeners();
            m_buttonCancel.onClick.AddListener(OnClickMysteryBoxCancel);
            m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_textExp.text = CsGameData.Instance.MysteryBoxQuest.MysteryBoxQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.MysteryBoxGrade.Grade == 1).ExpReward.Value.ToString("#,##0");
            m_textExploit.text = CsGameData.Instance.GetMysteryBoxGrade(1).ExploitPointReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
        }
        else if (CsMysteryBoxQuestManager.Instance.MysteryBoxState != EnMysteryBoxState.Accepted)
        {
            //완료
            m_textQuestName.text = CsGameData.Instance.MysteryBoxQuest.TargetTitle;
            m_textNpcName.text = CsGameData.Instance.MysteryBoxQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = CsGameData.Instance.MysteryBoxQuest.CompletionDialogue;
            m_buttonCancel.gameObject.SetActive(false);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
            m_textExp.text = CsGameData.Instance.MysteryBoxQuest.MysteryBoxQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level && a.MysteryBoxGrade.Grade == CsMysteryBoxQuestManager.Instance.PickedBoxGrade).ExpReward.Value.ToString("#,##0");
            m_textExploit.text = CsGameData.Instance.GetMysteryBoxGrade(CsMysteryBoxQuestManager.Instance.PickedBoxGrade).ExploitPointReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        }

        for (int i = 0; i < 2; i++)
        {
            Transform trReward = trRewardList.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(false);
        }

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickMysteryBoxQuest);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenMysteryBoxPopup()
    {
        UpdatePopupMysteryBox();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMysteryBox(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed)
            {
                CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A51_TXT_00001"),
                CsConfiguration.Instance.GetString("A49_BTN_00002"), OnClickMysteryBoxRetry,
                CsConfiguration.Instance.GetString("A49_BTN_00001"), OnClickMysteryBoxRetryCancel, true);

            }
            else
            {
                CsMysteryBoxQuestManager.Instance.StartAutoPlay();
            }
        }
        else
        {
            CsMysteryBoxQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.MysteryBox, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxAcceptDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.MysteryBoxQuest.RequiredHeroLevel)
            {
                if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Executed || CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Completed)
                {
                    OpenMysteryBoxPopup();
                }
                else
                {
                    if (CsMysteryBoxQuestManager.Instance.DailyMysteryBoxQuestStartCount < CsGameData.Instance.MysteryBoxQuest.LimitCount)
                    {
                        OpenMysteryBoxPopup();
                    }
                    else
                    {
                        if (CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo == null)
                        {
                            return;
                        }
                        else
                        {
                            UpdateNpcDialog(CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo);
                        }
                    }
                }
            }
            else
            {
                if (CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo == null)
                {
                    return;
                }
                else
                {
                    UpdateNpcDialog(CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxMissionDialog()
    {
        //CsMysteryBoxQuestManager.Instance.SendMysteryBoxPickStart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxAccept()
    {
        UpdateQuestPanel(EnQuestType.MysteryBox);

        CloseQuestPopup();

        ChangeQuestToggleIsOnDisplay(EnQuestType.MysteryBox, CsMysteryBoxQuestManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxPickCompleted()
    {
        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Completed)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A51_TXT_00003"));
        }

        UpdateQuestPanel(EnQuestType.MysteryBox);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.MysteryBox);
        ResetQuestPanelDisplay(EnQuestType.MysteryBox);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxNationTransmission()
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

        if (csNpcInfo == null)
            return;

        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.None ||
            CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Completed ||
            CsMysteryBoxQuestManager.Instance.LowPickComplete)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
            {
                // 국가 이동 실패
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
            }
            else
            {
                CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsGameData.Instance.MyHeroInfo.Nation.NationId);
            }
        }
        else
        {
            OpenPopupNationTransmission(csNpcInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMysteryBoxQuest()
    {
        switch (CsMysteryBoxQuestManager.Instance.MysteryBoxState)
        {
            case EnMysteryBoxState.None:
                CsMysteryBoxQuestManager.Instance.SendMysteryBoxQuestAccept();
                break;
            case EnMysteryBoxState.Accepted:
                break;
            case EnMysteryBoxState.Executed:
                CsMysteryBoxQuestManager.Instance.SendMysteryBoxQuestComplete();
                break;
            case EnMysteryBoxState.Completed:
                CsMysteryBoxQuestManager.Instance.SendMysteryBoxQuestComplete();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMysteryBoxRetry()
    {
        CsMysteryBoxQuestManager.Instance.LowPickComplete = false;
        AutoCancelButtonOpen(EnAutoStateType.MysteryBox);
        CsMysteryBoxQuestManager.Instance.StartAutoPlay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMysteryBoxRetryCancel()
    {
        CsMysteryBoxQuestManager.Instance.LowPickComplete = true;
        AutoCancelButtonOpen(EnAutoStateType.MysteryBox);
        CsMysteryBoxQuestManager.Instance.StartAutoPlay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMysteryBoxCancel()
    {
        CloseQuestPopup();

        if (CsMysteryBoxQuestManager.Instance.Auto)
        {
            CsMysteryBoxQuestManager.Instance.StopAutoPlay(this);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.MysteryBox, false);
    }

    void OnEventMysteryBoxStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.MysteryBox);
    }

    #endregion MysteryBoxQuest

    #region DimensionRaidQuest

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupDimensionRaidQuest()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);
        trRewardList.Find("RewardExp").gameObject.SetActive(true);

        CsDimensionRaidQuest csDimensionRaidQuest = CsGameData.Instance.DimensionRaidQuest;
        CsDimensionRaidQuestReward csDimensionRaidQuestReward = csDimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level);

        if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.None)
        {
            //수락
            m_textQuestName.text = csDimensionRaidQuest.GetDimensionRaidQuestStep(1).TargetTitle;
            m_textNpcName.text = csDimensionRaidQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = csDimensionRaidQuest.StartDialogue;

            m_buttonCancel.onClick.RemoveAllListeners();
            m_buttonCancel.onClick.AddListener(OnClickDimensionRaidQuestCancel);
            m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
        }
        else if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
        {
            //완료
            m_textQuestName.text = csDimensionRaidQuest.GetDimensionRaidQuestStep(1).TargetTitle;
            m_textNpcName.text = csDimensionRaidQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = csDimensionRaidQuest.CompletionDialogue;
            m_buttonCancel.gameObject.SetActive(false);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
            m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        }

        m_textExp.text = csDimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
        m_textExploit.text = csDimensionRaidQuest.GetDimensionRaidQuestReward(CsGameData.Instance.MyHeroInfo.Level).ExploitPointReward.Value.ToString("#,##0");
        int nItemId = csDimensionRaidQuestReward.ItemRewardId.Item.ItemId;
        Transform trReward = trRewardList.Find("ButtonReward0");
        trReward.gameObject.SetActive(true);

        Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();
        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + nItemId);

        Text textName = trReward.Find("TextName").GetComponent<Text>();
        textName.text = csDimensionRaidQuestReward.ItemRewardId.Item.Name;
        CsUIData.Instance.SetFont(textName);

        Text textCount = trReward.Find("TextCount").GetComponent<Text>();
        textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), csDimensionRaidQuestReward.ItemRewardId.ItemCount);
        CsUIData.Instance.SetFont(textCount);

        Button buttonReward = trReward.GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
        buttonReward.onClick.AddListener(() => OnClickRewardItem(nItemId, csDimensionRaidQuestReward.ItemRewardId.ItemCount, csDimensionRaidQuestReward.ItemRewardId.ItemOwned));
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        trReward = trRewardList.Find("ButtonReward1");
        trReward.gameObject.SetActive(false);

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickDimensionRaidQuest);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

	//---------------------------------------------------------------------------------------------------
    void OpenDimensionRaidQuestPopup()
    {
        UpdatePopupDimensionRaidQuest();
    }

	//---------------------------------------------------------------------------------------------------
    void OnValueChangedDimensionRaidQuest(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsDimensionRaidQuestManager.Instance.StartAutoPlay();
        }
        else
        {
            CsDimensionRaidQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.DimensionRaid, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestAcceptDialog()
    {
        OpenDimensionRaidQuestPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.DimensionRaid);

        CloseQuestPopup();
        ChangeQuestToggleIsOnDisplay(EnQuestType.DimensionRaid, CsDimensionRaidQuestManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.DimensionRaid);
        ResetQuestPanelDisplay(EnQuestType.DimensionRaid);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestNpctDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level >= CsGameData.Instance.DimensionRaidQuest.RequiredHeroLevel)
            {
                if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
                {
                    OpenDimensionRaidQuestPopup();
                }
                else
                {
                    if (CsDimensionRaidQuestManager.Instance.DailyDimensionRaidQuestStartCount < CsGameData.Instance.DimensionRaidQuest.LimitCount)
                    {
                        OpenDimensionRaidQuestPopup();
                    }
                    else
                    {
                        if (CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo == null)
                        {
                            return;
                        }
                        else
                        {
                            UpdateNpcDialog(CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo);
                        }
                    }
                }
            }
            else
            {
                if (CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo == null)
                {
                    return;
                }
                else
                {
                    UpdateNpcDialog(CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestMissionDialog()
    {
        //CsDimensionRaidQuestManager.Instance.SendDimensionRaidInteractionStart();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidInteractionCompleted()
    {
        UpdateQuestPanel(EnQuestType.DimensionRaid);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestNationTransmission()
    {
        CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);

        if (csNpcInfo == null)
            return;

        if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.None || CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
            {
                // 국가 이동 실패
                CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
            }
            else
            {
                CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
                CsCommandEventManager.Instance.SendNationTransmission(csNpcInfo.NpcId, CsGameData.Instance.MyHeroInfo.Nation.NationId);
            }
        }
        else
        {
            OpenPopupNationTransmission(csNpcInfo);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDimensionRaidQuest()
    {
        switch (CsDimensionRaidQuestManager.Instance.DimensionRaidState)
        {
            case EnDimensionRaidState.None:
                CsDimensionRaidQuestManager.Instance.SendDimensionRaidQuestAccept();
                break;
            case EnDimensionRaidState.Completed:
                CsDimensionRaidQuestManager.Instance.SendDimensionRaidQuestComplete();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDimensionRaidQuestCancel()
    {
        CloseQuestPopup();

        if (CsDimensionRaidQuestManager.Instance.Auto)
        {
            CsDimensionRaidQuestManager.Instance.StopAutoPlay(this);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.DimensionRaid, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.DimensionRaid);
    }

    #endregion DimensionRaidQuest

    #region HolyWarQuest

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleHolyWarQuestTimeText()
    {
        Text textTime = GetQuestPanel(EnQuestType.HolyWar).Find("TextTime").GetComponent<Text>();
        float flRemainingTime = CsHolyWarQuestManager.Instance.RemainingTime;

        if (flRemainingTime <= 0.0f)
        {
            CsHolyWarQuestManager.Instance.RemainingTime = 0.0f;
            CsHolyWarQuestManager.Instance.UpdateHolyWarQuestState();
            UpdateQuestPanel(EnQuestType.HolyWar);
        }
        else
        {
            System.TimeSpan tsRemainingTime = System.TimeSpan.FromSeconds(flRemainingTime);
            textTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsRemainingTime.Minutes.ToString("0#"), tsRemainingTime.Seconds.ToString("0#"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleHolyWarQuestKillCount()
    {
        Text textTarget = GetQuestPanel(EnQuestType.HolyWar).Find("TextTarget").GetComponent<Text>();

        int nGloryLevel = 0;

        for (int i = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList.Count - 1; i >= 0; i--)
        {
            if (CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList[i].RequiredKillCount <= CsHolyWarQuestManager.Instance.KillCount)
            {
                nGloryLevel = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList[i].GloryLevel;
                break;
            }
            else
            {
                continue;
            }
        }

        textTarget.text = string.Format(CsConfiguration.Instance.GetString(CsHolyWarQuestManager.Instance.HolyWarQuest.TargetContent), CsHolyWarQuestManager.Instance.KillCount, nGloryLevel);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupHolyWarQuest()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExp").gameObject.SetActive(true);
        trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);

        CsHolyWarQuest csHolyWarQuest = CsHolyWarQuestManager.Instance.HolyWarQuest;
        
        if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.None)
        {
            //수락
            m_textQuestName.text = csHolyWarQuest.TargetTitle;
            m_textNpcName.text = csHolyWarQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = string.Format(CsConfiguration.Instance.GetString(csHolyWarQuest.StartDialogue), CsGameData.Instance.MyHeroInfo.Nation.Name);
            m_textExploit.text = CsConfiguration.Instance.GetString("PUBLIC_TXT_QUE");

            m_buttonCancel.onClick.RemoveAllListeners();
            m_buttonCancel.onClick.AddListener(OnClickHolyWarQuestCancel);
            m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
        }
        else if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
        {
			CsHolyWarQuestGloryLevel csHolyWarQuestGloryLevel = null;

			for (int i = csHolyWarQuest.HolyWarQuestGloryLevelList.Count - 1; i >= 0; i--)
			{
			    if (csHolyWarQuest.HolyWarQuestGloryLevelList[i].RequiredKillCount <= CsHolyWarQuestManager.Instance.KillCount)
			    {
					csHolyWarQuestGloryLevel = CsHolyWarQuestManager.Instance.HolyWarQuest.HolyWarQuestGloryLevelList[i];
			        break;
			    }
			}

			//완료
            m_textQuestName.text = csHolyWarQuest.TargetTitle;
            m_textNpcName.text = csHolyWarQuest.QuestNpcInfo.Name;
            m_textNpcScript.text = csHolyWarQuest.CompletionDialogue;

			if (csHolyWarQuestGloryLevel == null)
			{
				m_textExploit.text = "0";
			}
			else
			{
				m_textExploit.text = csHolyWarQuestGloryLevel.ExploitPointReward.Value.ToString("#,##0");
			}

            m_buttonCancel.gameObject.SetActive(false);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
            m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        }

        long lExpReward = 0;

        for (int i = 0; i < csHolyWarQuest.HolyWarQuestRewardList.Count; i++)
        {
            if (csHolyWarQuest.HolyWarQuestRewardList[i].Level == CsGameData.Instance.MyHeroInfo.Level)
            {
                lExpReward = csHolyWarQuest.HolyWarQuestRewardList[i].ExpReward.Value;
                break;
            }
        }

        m_textExp.text = lExpReward.ToString("#,##0");

        Transform trReward;

        trReward = trRewardList.Find("ButtonReward0");
        trReward.gameObject.SetActive(false);

        trReward = trRewardList.Find("ButtonReward1");
        trReward.gameObject.SetActive(false);

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickHolyWarQuest);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenHolyWarQuestPopup()
    {
        UpdatePopupHolyWarQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedHolyWarQuest(bool bIsOn)
    {
        if (bIsOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsHolyWarQuestManager.Instance.StartAutoPlay();
        }
        else
        {
            CsHolyWarQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.HolyWar, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHolyWarQuest()
    {
        if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.None)
        {
            CsHolyWarQuestManager.Instance.SendHolyWarQuestAccept();
        }
        else if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
        {
            CsHolyWarQuestManager.Instance.SendHolyWarQuestComplete();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHolyWarQuestCancel()
    {
        CloseQuestPopup();

        if (CsHolyWarQuestManager.Instance.Auto)
        {
            CsHolyWarQuestManager.Instance.StopAutoPlay(this);
        }
    }

    #endregion HolyWarQuest

    #region SupplySupport

    Transform m_trPopupSupplySupportAccept;
    Text m_textTImeSupplySupport;
    GameObject m_goSupplySupportAcceptPopup;
    int m_nSupplySupportSelectItemIndex = 0;
    GameObject m_goPopupChangeCart;

    //---------------------------------------------------------------------------------------------------
    void UpdateSupplySupportTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(CsSupplySupportQuestManager.Instance.RemainingTime - Time.realtimeSinceStartup);
        m_textTImeSupplySupport.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSupplySupportAccept()
    {
        if (m_goSupplySupportAcceptPopup == null)
        {
            StartCoroutine(LoadPopupSupplySupportAcceptCoroutine(() => UpdatePopupSupplySupportAccept()));
        }
        else
        {
            UpdatePopupSupplySupportAccept();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSupplySupportAcceptCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupCart");
        yield return resourceRequest;
        m_goSupplySupportAcceptPopup = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupSupplySupportAccept()
    {
        Transform trPopup = GameObject.Find("Canvas/Popup").transform;
        m_trPopupSupplySupportAccept = trPopup.Find("PopupSupplySupportAccept");

        if (m_trPopupSupplySupportAccept == null)
        {
            GameObject goPopup = Instantiate(m_goSupplySupportAcceptPopup, trPopup);
            goPopup.name = "PopupSupplySupportAccept";
            m_trPopupSupplySupportAccept = goPopup.transform;
        }
        else
        {
            m_trPopupSupplySupportAccept.gameObject.SetActive(true);
        }

        InitializePopupSupplySupportAccept();

        Transform trBack = m_trPopupSupplySupportAccept.Find("ImageBackground");
        Transform trFlagList = trBack.Find("FlagList");

        for (int i = 0; i < CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList.Count; i++)
        {
            int nIndex = i;
            Transform trFlag = trFlagList.Find("ToggleFlag" + i);

            Toggle toggleFlag = trFlag.GetComponent<Toggle>();
            toggleFlag.onValueChanged.RemoveAllListeners();
            toggleFlag.isOn = false;
            m_nSupplySupportSelectItemIndex = 0;
            toggleFlag.onValueChanged.AddListener((ison) => OnValueChangedSupplySupportFlag(toggleFlag, nIndex));

            Image imageItemIcon = trFlag.Find("ImageItem").GetComponent<Image>();
            imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList[i].OrderItem.Image);

            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList[i].OrderItem.ItemId);

            Text textFlagCount = trFlag.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textFlagCount);
            textFlagCount.text = nItemCount.ToString("#,##0");

            Transform trLock = trFlag.Find("ImageLock");

            if (nItemCount > 0)
            {
                toggleFlag.interactable = true;
                trLock.gameObject.SetActive(false);
            }
            else
            {
                toggleFlag.interactable = false;
                trLock.gameObject.SetActive(true);
            }
        }

        Button buttonAccept = trBack.Find("ButtonAccept").GetComponent<Button>();
        CsUIData.Instance.DisplayButtonInteractable(buttonAccept, false);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupSupplySupportAccept()
    {
        Transform trBack = m_trPopupSupplySupportAccept.Find("ImageBackground");

        Text textPopupName = trBack.Find("TextTitle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A41_TXT_01001");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPopupSupplySupportAcceptClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textScript = trBack.Find("TextScript").GetComponent<Text>();
        CsUIData.Instance.SetFont(textScript);
        textScript.text = CsConfiguration.Instance.GetString("A41_TXT_01002");

        Text textFlag = trBack.Find("TextFlag").GetComponent<Text>();
        CsUIData.Instance.SetFont(textFlag);
        textFlag.text = CsConfiguration.Instance.GetString("A41_TXT_01003");

        Button buttonAccept = trBack.Find("ButtonAccept").GetComponent<Button>();
        buttonAccept.onClick.RemoveAllListeners();
        buttonAccept.onClick.AddListener(OnClickSupplySupportAccept);
        buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAccept = buttonAccept.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAccept);
        textAccept.text = CsConfiguration.Instance.GetString("A41_BTN_00002");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupSupplySupportAcceptClose()
    {
        if (m_trPopupSupplySupportAccept != null)
        {
            Destroy(m_trPopupSupplySupportAccept.gameObject);
            m_trPopupSupplySupportAccept = null;
        }

        if (CsSupplySupportQuestManager.Instance.Auto)
        {
            ChangeQuestToggleIsOnDisplay(EnQuestType.SupplySupport, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSupplySupportAccept()
    {
        //저장된 OrderId로 퀘스트 수락하기
        if (m_nSupplySupportSelectItemIndex > 0)
        {
            CsSupplySupportQuestManager.Instance.SendSupplySupportQuestAccept(m_nSupplySupportSelectItemIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSupplySupportFlag(Toggle toggle, int nIndex)
    {
        Transform trBack = m_trPopupSupplySupportAccept.Find("ImageBackground");
        Button buttonAccept = trBack.Find("ButtonAccept").GetComponent<Button>();

        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_nSupplySupportSelectItemIndex = CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList[nIndex].OrderId;
            CsUIData.Instance.DisplayButtonInteractable(buttonAccept, true);

            //아이템이있음
            if (CsGameData.Instance.MyHeroInfo.Gold >= CsGameData.Instance.SupplySupportQuest.GuaranteeGold)
            {
                if (CsGameData.Instance.SupplySupportQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonAccept, true);
                }
            }
        }
        else
        {
            m_nSupplySupportSelectItemIndex = 0;
            CsUIData.Instance.DisplayButtonInteractable(buttonAccept, false);
        }
    }

    Transform m_trPopupCartChange;

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSupplySupportChange(int nOldGrade, int nNewGrade, bool bAuto)
    {
        if (m_goPopupChangeCart == null)
        {
            m_goPopupChangeCart = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/PopupCartUpgrade");
        }

        Transform trPopup = GameObject.Find("Canvas/Popup").transform;
        m_trPopupCartChange = trPopup.Find("PopupCartChange");

        if (m_trPopupCartChange == null)
        {
            GameObject goPopup = Instantiate(m_goPopupChangeCart, trPopup);
            goPopup.name = "PopupCartChange";
            m_trPopupCartChange = goPopup.transform;
        }
        else
        {
            m_trPopupCartChange.gameObject.SetActive(true);
        }

        Transform trBack = m_trPopupCartChange.Find("ImageBackground");

        Text textPopupName = trBack.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A41_TXT_01004");

        StartCoroutine(CartChangeCoroutine(nOldGrade, nNewGrade, bAuto));
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator CartChangeCoroutine(int nOldGrade, int nNewGrade, bool bAuto)
    {
        Transform trCartList = m_trPopupCartChange.Find("ImageBackground/CartList");
        Toggle toggle;
        int SelectGrade = nOldGrade;

        for (int i = 1; i <= 5; i++)
        {
            Transform trLock = trCartList.Find("ToggleCart" + i + "/ImageLock");

            if (i < nOldGrade)
            {
                trLock.gameObject.SetActive(true);
            }
            else
            {
                trLock.gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < 20; i++)
        {
            toggle = trCartList.Find("ToggleCart" + SelectGrade).GetComponent<Toggle>();
            toggle.isOn = true;

            if (i > 15 && SelectGrade == nNewGrade)
            {
            }
            else
            {
                SelectGrade++;

                if (SelectGrade > 5)
                {
                    SelectGrade = nOldGrade;
                }
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        Destroy(m_trPopupCartChange.gameObject);
        m_trPopupCartChange = null;

        if (bAuto)
        {
            AutoCancelButtonOpen(EnAutoStateType.SupplySupport);
            CsSupplySupportQuestManager.Instance.StartAutoPlay();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSupplySupportClear(long lExp, long lGold, int nExploitPoint)
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExp").gameObject.SetActive(true);
        trRewardList.Find("RewardGold").gameObject.SetActive(true);
        trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);

        //완료
        m_textQuestName.text = CsGameData.Instance.SupplySupportQuest.TargetTitle;
        m_textNpcName.text = CsGameData.Instance.SupplySupportQuest.CompletionNpc.Name;
        m_textNpcScript.text = CsGameData.Instance.SupplySupportQuest.CompletionDialogue;
        m_buttonCancel.gameObject.SetActive(false);
        m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
        m_textGold.text = lGold.ToString("#,##0");
        m_textExp.text = lExp.ToString("#,##0");
        m_textExploit.text = nExploitPoint.ToString("#,##0");
        m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;

        for (int i = 0; i < 2; i++)
        {
            Transform trReward = trRewardList.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(false);
        }

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickSupplySupportClear);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSupplySupportClear()
    {
        CsSupplySupportQuestManager.Instance.SendSupplySupportQuestComplete();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueSupplySupport(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsSupplySupportQuestManager.Instance.StartAutoPlay();
        }
        else
        {
            CsSupplySupportQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.SupplySupport, true);
            }
            else
            {
                return;
            }
        }
    }

    #endregion SupplySupport

    #region GuildFarmQuest
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildFarmQuest(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.FarmQuest);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.FarmQuest);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildFarm, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildFarmQuest()
    {
        CloseNpcDialogPopup();

        if (CsGuildManager.Instance.GuildFarmQuestState != EnGuildFarmQuestState.Executed)
        {
            int nCurrentTime = (int)CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date).TotalSeconds;

            if (CsGameData.Instance.GuildFarmQuest.EndTime < nCurrentTime)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_ERROR_TIME"));
                return;
            }
        }

        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        //여기에 수락인지 완료인지 상태로 나눔

        switch (CsGuildManager.Instance.GuildFarmQuestState)
        {
            case EnGuildFarmQuestState.None:
                //수락
                m_textQuestName.text = CsGameData.Instance.GuildFarmQuest.Name;
                m_textNpcName.text = CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc.Name;
                m_textNpcScript.text = CsGameData.Instance.GuildFarmQuest.QuestStartDialogue;

                m_buttonCancel.gameObject.SetActive(true);
                m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
                m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
                break;


            case EnGuildFarmQuestState.Executed:
                //완료
                m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
                m_textNpcName.text = CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc.Name;
                m_textNpcScript.text = CsGameData.Instance.GuildFarmQuest.QuestCompletionDialogue;

                m_buttonCancel.gameObject.SetActive(false);
                m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
                m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
                break;
        }

        //보상세팅
        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardExp").gameObject.SetActive(true);

        CsGuildFarmQuestReward csGuildFarmQuestReward = CsGameData.Instance.GuildFarmQuest.GuildFarmQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level);

        if (csGuildFarmQuestReward != null)
        {
            m_textExp.text = csGuildFarmQuestReward.ExpReward.Value.ToString("#,##0");
        }

        trRewardList.Find("RewardGuildMoralPoint").gameObject.SetActive(true);
        m_textGuildMoralPoint.text = CsGameData.Instance.GuildFarmQuest.CompletionGuildContributionPointReward.Value.ToString("#,##0");

        trRewardList.Find("RewardGuildBuildingPoint").gameObject.SetActive(true);
        m_textGuildBuildingPoint.text = CsGameData.Instance.GuildFarmQuest.CompletionGuildBuildingPointReward.Value.ToString("#,##0");

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickAcceptGuildFarmQuest);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonCancel.onClick.RemoveAllListeners();
        m_buttonCancel.onClick.AddListener(CloseQuestPopup);
        m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAcceptGuildFarmQuest()
    {
        switch (CsGuildManager.Instance.GuildFarmQuestState)
        {
            case EnGuildFarmQuestState.None:
                int nCurrentTime = (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour * 3600) + (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute * 60) + CsGameData.Instance.MyHeroInfo.CurrentDateTime.Second;

                if (CsGameData.Instance.GuildFarmQuest.EndTime < nCurrentTime)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_ERROR_TIME"));
                    return;
                }

                CsGuildManager.Instance.SendGuildFarmQuestAccept();
                break;

            case EnGuildFarmQuestState.Executed:
                CsGuildManager.Instance.SendGuildFarmQuestComplete();
                break;
        }
    }

    #endregion GuildFarmQuest

    #region GuildMission

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildMissionTimeText()
    {
        Transform trQuestPanel = GetQuestPanel(EnQuestType.GuildMission);
        TimeSpan tsRemainigTime = TimeSpan.FromSeconds(m_flGuildMissionRemainingTime - Time.realtimeSinceStartup);

        Text textTime = trQuestPanel.Find("TextTime").GetComponent<Text>();
        textTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsRemainigTime.Minutes.ToString("00"), tsRemainigTime.Seconds.ToString("00"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildMission(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Mission);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Mission);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildMission, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionMissionDialog()
    {
        CsGuildMission csGuildMission = CsGuildManager.Instance.GuildMission;
        if (csGuildMission != null && csGuildMission.Type == 1)
        {
            CsGuildManager.Instance.SendGuildMissionTargetNpcInteract();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionAcceptDialog()
    {
        CsGuildMissionQuest csGuildMissionQuest = CsGameData.Instance.GuildMissionQuest;

        if (!CsGuildManager.Instance.MissionCompleted && CsGameData.Instance.MyHeroInfo.Level >= CsGameConfig.Instance.GuildRequiredHeroLevel && CsGuildManager.Instance.Guild != null && CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.GuildMission) == null)
        {
            UpdatePopupNpcDialog(csGuildMissionQuest.StartNpc.Name, csGuildMissionQuest.StartNpc.Dialogue, OnClickGuildMissionCancel, OnClickGuildMissionAccept, csGuildMissionQuest.Name);
        }
        else
        {
            UpdateNpcDialog(csGuildMissionQuest.StartNpc);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildMissionCancel()
    {
        CloseNpcDialogPopup();

        if (CsGuildManager.Instance.Auto)
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Mission);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionAccept()
    {
        UpdateQuestPanel(EnQuestType.GuildMission);
        CloseNpcDialogPopup();
        ChangeQuestToggleIsOnDisplay(EnQuestType.GuildMission, CsGuildManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.GuildMission);
        CloseNpcDialogPopup();
        ChangeQuestToggleIsOnDisplay(EnQuestType.GuildMission, CsGuildManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        UpdateQuestPanel(EnQuestType.GuildMission);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionAbandon()
    {
        UpdateQuestPanel(EnQuestType.GuildMission);
        ResetQuestPanelDisplay(EnQuestType.GuildMission);
        m_flGuildMissionRemainingTime = 0.0f;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionFailed()
    {
        UpdateQuestPanel(EnQuestType.GuildMission);
        ResetQuestPanelDisplay(EnQuestType.GuildMission);
        m_flGuildMissionRemainingTime = 0.0f;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateMissionState()
    {
        UpdateQuestPanel(EnQuestType.GuildMission);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildMissionAccept()
    {
        if (CsGuildManager.Instance.GuildMissionState == EnGuildMissionState.Accepted)
        {
            CsGuildManager.Instance.SendGuildMissionAccept();
        }
        else
        {
            if (CsGuildManager.Instance.MissionQuest)
            {
                CsGuildManager.Instance.SendGuildMissionAccept();
            }
            else
            {
                CsGuildManager.Instance.SendGuildMissionQuestAccept();
            }
        }
    }

    #endregion GuildMission

    #region GuildFoodWarehouse

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWareHouseDialog()
    {
        CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc;
        UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue, null, OnClickOpenPopupGuildFoodWarehouse, CsGameData.Instance.GuildFoodWarehouse.Name);

        DisplayOffAutoCancel();
        CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.FoodWareHouse);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupGuildFoodWarehouse()
    {
        Transform trPopupGuildFoodWarehouse = m_trPopupList.Find("PopupGuildFoodWarehouse");

        if (trPopupGuildFoodWarehouse == null)
        {
            StartCoroutine(LoadPopupGuildFoodWarehouse());
        }

        CloseNpcDialogPopup();
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupGuildFoodWarehouse()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupGuild/PopupGuildFoodWarehouse");
        yield return resourceRequest;

        Instantiate((GameObject)resourceRequest.asset, m_trPopupList);
    }

    #endregion GuildFoodWarehouse

    #region GuildSupplySupport

    Text m_textTImeGuildSupplySupport;

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildSupplySupportTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(CsGuildManager.Instance.GuildSupplySupportQuestRemainingTime - Time.realtimeSinceStartup);
        m_textTImeGuildSupplySupport.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupGuildSupplySupport()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        trRewardList.Find("RewardGuildBuildingPoint").gameObject.SetActive(true);
        trRewardList.Find("RewardGuildFund").gameObject.SetActive(true);
        trRewardList.Find("RewardExp").gameObject.SetActive(true);

        if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.None)
        {
            //수락
            m_textQuestName.text = CsGameData.Instance.GuildSupplySupportQuest.Name;
            m_textNpcName.text = CsGameData.Instance.GuildSupplySupportQuest.StartNpc.Name;
            m_textNpcScript.text = CsGameData.Instance.GuildSupplySupportQuest.StartDialogue;

            m_buttonCancel.onClick.RemoveAllListeners();
            m_buttonCancel.onClick.AddListener(OnClickGuildSupplySupportCancel);
            m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            m_buttonCancel.gameObject.SetActive(true);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
            m_textExp.text = CsGameData.Instance.GuildSupplySupportQuest.GuildSupplySupportQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
            m_textGuildBuildingPoint.text = CsGameData.Instance.GuildSupplySupportQuest.GuildBuildingPointReward.Value.ToString("#,##0");
            m_textGuildFund.text = CsGameData.Instance.GuildSupplySupportQuest.GuildFundReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.Quest;

        }
        else if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.Accepted)
        {
            //완료
            m_textQuestName.text = CsGameData.Instance.GuildSupplySupportQuest.Name;
            m_textNpcName.text = CsGameData.Instance.GuildSupplySupportQuest.CompletionNpc.Name;
            m_textNpcScript.text = CsGameData.Instance.GuildSupplySupportQuest.CompletionDialogue;
            m_buttonCancel.gameObject.SetActive(false);
            m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
            m_textExp.text = CsGameData.Instance.GuildSupplySupportQuest.GuildSupplySupportQuestRewardList.Find(a => a.Level == CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
            m_textGuildBuildingPoint.text = CsGameData.Instance.GuildSupplySupportQuest.GuildBuildingPointReward.Value.ToString("#,##0");
            m_textGuildFund.text = CsGameData.Instance.GuildSupplySupportQuest.GuildFundReward.Value.ToString("#,##0");
            m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        }

        for (int i = 0; i < 2; i++)
        {
            Transform trReward = trRewardList.Find("ButtonReward" + i);
            trReward.gameObject.SetActive(false);
        }

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickGuildSupplySupportAccept);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildSupplySupport(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.SupplySupport);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.SupplySupport);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildSupplySupport, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildSupplySupportAccept()
    {
        switch (CsGuildManager.Instance.GuildSupplySupportState)
        {
            case EnGuildSupplySupportState.None:
                CsGuildManager.Instance.SendGuildSupplySupportQuestAccept();
                break;
            case EnGuildSupplySupportState.Accepted:
                CsGuildManager.Instance.SendGuildSupplySupportQuestComplete();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildSupplySupportCancel()
    {
        CloseQuestPopup();

        if (CsGuildManager.Instance.Auto)
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.SupplySupport);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestAccept(PDGuildSupplySupportQuestCartInstance pDGuildSupplySupportQuestCartInstance)
    {
        UpdateQuestPanel(EnQuestType.GuildSupplySupport);

        CloseQuestPopup();
        ChangeQuestToggleIsOnDisplay(EnQuestType.GuildSupplySupport, CsGuildManager.Instance.Auto);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestFail()
    {
        UpdateQuestPanel(EnQuestType.GuildSupplySupport);
        ResetQuestPanelDisplay(EnQuestType.GuildSupplySupport);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestComplete(bool bLevelUp, long lAcquredExp)
    {
        CloseQuestPopup();
        UpdateQuestPanel(EnQuestType.GuildSupplySupport);
        ResetQuestPanelDisplay(EnQuestType.GuildSupplySupport);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartSupplySupportNpctDialog()
    {
        if (m_trPopupMainQuest != null && m_trPopupMainQuest.gameObject.activeSelf)
        {
            return;
        }
        else
        {
            if (CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.DailyGuildSupplySupportQuestStartCount < 1 && CsGameData.Instance.GetGuildMemberGrade(CsGuildManager.Instance.MyGuildMemberGrade.MemberGrade).GuildSupplySupportQuestEnabled)
            {
                UpdatePopupGuildSupplySupport();
                DisplayOffAutoCancel();
            }
            else
            {
                UpdateNpcDialog(CsGuildManager.Instance.GuildSupplySupportQuest.StartNpc);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEndSupplySupportNpctDialog()
    {
        UpdatePopupGuildSupplySupport();
    }

    #endregion GuildSupplySupport

    #region GuildHunting

    GameObject m_goPopupGuildHunting;
    Transform m_trPopupGuildHungting;

    //---------------------------------------------------------------------------------------------------
    void OpenPopupGuildHunting()
    {
        if (CsGuildManager.Instance.GuildHuntingState != EnGuildHuntingState.Executed)
        {
            if (m_goPopupGuildHunting == null)
            {
                StartCoroutine(LoadPopupGuildHuntingCoroutine(() => UpdatePopupGuildHunting()));
            }
            else
            {
                UpdatePopupGuildHunting();
            }
        }
        else
        {
            UpdateGuildHuntingComplete();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupGuildHuntingCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGuildHunting");
        yield return resourceRequest;
        m_goPopupGuildHunting = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupGuildHuntingClose()
    {
        if (m_trPopupGuildHungting != null)
        {
            Destroy(m_trPopupGuildHungting.gameObject);
            m_trPopupGuildHungting = null;
        }
        if (CsGuildManager.Instance.Auto)
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Hunting);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildHuntingAccept()
    {
        if (CsGuildManager.Instance.Guild != null)
        {
            switch (CsGuildManager.Instance.GuildHuntingState)
            {
                case EnGuildHuntingState.None:
                    CsGuildManager.Instance.SendGuildHuntingQuestAccept();
                    break;
                case EnGuildHuntingState.Accepted:
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A75_TXT_03003"));
                    break;
                case EnGuildHuntingState.Executed:
                    CsGuildManager.Instance.SendGuildHuntingQuestComplete();
                    break;
                case EnGuildHuntingState.Competed:
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A75_TXT_03003"));
                    break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupGuildHuntingGuage()
    {
        if (m_trPopupGuildHungting != null)
        {
            int nMyCount = 0;
            int nMyDonateMaxCount = 1;
            int nGuildCount = CsGuildManager.Instance.Guild.DailyHuntingDonationCount;
            int nGuildMaxCount = CsGameConfig.Instance.GuildHuntingDonationMaxCount;


            if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Day.CompareTo(CsGuildManager.Instance.GuildHuntingDonationDate.Day) == 0)
            {
                nMyCount = 1;
            }

            Transform trGuilageCircleBack = m_trPopupGuildHungting.Find("ImageBackground/ImageGuageCircleBack");

            Image imageValue = trGuilageCircleBack.Find("ImageValue").GetComponent<Image>();
            imageValue.fillAmount = (float)nGuildCount / nGuildMaxCount;

            Text textValue = trGuilageCircleBack.Find("Text").GetComponent<Text>();
            textValue.text = string.Format(CsConfiguration.Instance.GetString("A75_TXT_00002"), nGuildCount, nGuildMaxCount);
            CsUIData.Instance.SetFont(textValue);

            Button buttonDonate = m_trPopupGuildHungting.Find("ImageBackground/ButtonDonate").GetComponent<Button>();
            Image imageIcon = buttonDonate.transform.Find("Image").GetComponent<Image>();

            if (nMyCount < nMyDonateMaxCount)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonDonate, true);
                imageIcon.color = new Color(1, 1, 1, 1f);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonDonate, false);
                imageIcon.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupGuildHunting()
    {
        Transform trBack = m_trPopupGuildHungting.Find("ImageBackground");

        Text textPopupName = trBack.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A75_TXT_00001");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPopupGuildHuntingClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textCount = trBack.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);

        Button buttonGuildHunting = trBack.Find("ButtonGuildHunting").GetComponent<Button>();
        buttonGuildHunting.onClick.RemoveAllListeners();
        buttonGuildHunting.onClick.AddListener(OnClickGuildHuntingAccept);
        buttonGuildHunting.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.GuildHunting) == null)
        {
            textCount.text = string.Format(CsConfiguration.Instance.GetString("A75_TXT_00003"), CsGuildManager.Instance.DailyGuildHuntingQuestStartCount, CsGuildManager.Instance.GuildHuntingQuest.LimitCount);
            CsUIData.Instance.DisplayButtonInteractable(buttonGuildHunting, CsGuildManager.Instance.DailyGuildHuntingQuestStartCount < CsGuildManager.Instance.GuildHuntingQuest.LimitCount);
        }
        else
        {
            textCount.text = string.Format(CsConfiguration.Instance.GetString("A75_TXT_00003"), CsGuildManager.Instance.GuildHuntingQuest.LimitCount, CsGuildManager.Instance.GuildHuntingQuest.LimitCount);
            CsUIData.Instance.DisplayButtonInteractable(buttonGuildHunting, false);
        }

        Text textGuildHunting = buttonGuildHunting.transform.Find("Text").GetComponent<Text>();
        textGuildHunting.text = CsConfiguration.Instance.GetString("A75_BTN_00001");
        CsUIData.Instance.SetFont(textGuildHunting);

        Button buttonDonate = trBack.Find("ButtonDonate").GetComponent<Button>();
        buttonDonate.onClick.RemoveAllListeners();
        buttonDonate.onClick.AddListener(OnClickGuildHuntingDonate);
        buttonDonate.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDonate = buttonDonate.transform.Find("Text").GetComponent<Text>();
        textDonate.text = CsConfiguration.Instance.GetString("A75_BTN_00002");
        CsUIData.Instance.SetFont(textDonate);

        Image imageIcon = buttonDonate.transform.Find("Image").GetComponent<Image>();
        imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + CsGameConfig.Instance.GuildHuntingDonationItemId);

        UpdatePopupGuildHuntingGuage();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupGuildHunting()
    {
        Transform trPopup = GameObject.Find("Canvas/Popup").transform;
        m_trPopupGuildHungting = trPopup.Find("PopupGuildHunting");

        if (m_trPopupGuildHungting == null)
        {
            m_trPopupGuildHungting = Instantiate(m_goPopupGuildHunting, trPopup).transform;
            m_trPopupGuildHungting.name = "PopupGuildHunting";
        }
        else
        {
            m_trPopupGuildHungting.gameObject.SetActive(true);
        }

        InitializePopupGuildHunting();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildHuntingComplete()
    {
        if (m_bIsFirstQuestPopup)
        {
            InitializePopupQuest();
        }

        m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
        m_textQuestName.text = CsGuildManager.Instance.GuildHuntingQuestObjective.TargetTitle;
        m_textNpcName.text = CsGameData.Instance.GetNpcInfo(CsGuildManager.Instance.GuildHuntingQuest.QuestNpcId).Name;
        m_textNpcScript.text = CsGameData.Instance.GuildHuntingQuest.CompletionDialogue;
        m_buttonCancel.gameObject.SetActive(false);
        m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");

        CsItemReward csItemReward = CsGameData.Instance.GuildHuntingQuest.ItemReward;

        Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

        for (int i = 0; i < trRewardList.childCount; i++)
        {
            trRewardList.GetChild(i).gameObject.SetActive(false);
        }

        Transform trReward = trRewardList.Find("ButtonReward0");
        trRewardList.Find("ButtonReward1").gameObject.SetActive(false);

        Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();
        imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);

        Text textName = trReward.Find("TextName").GetComponent<Text>();
        textName.text = csItemReward.Item.Name;
        CsUIData.Instance.SetFont(textName);

        Text textCount = trReward.Find("TextCount").GetComponent<Text>();
        textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), csItemReward.ItemCount);
        CsUIData.Instance.SetFont(textCount);

        Button buttonReward = trReward.GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
		buttonReward.onClick.AddListener(() => OnClickRewardItem(csItemReward.Item.ItemId, csItemReward.ItemCount, csItemReward.ItemOwned));
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        trReward.gameObject.SetActive(true);

        m_buttonAccept.onClick.RemoveAllListeners();
        m_buttonAccept.onClick.AddListener(OnClickGuildHuntingAccept);
        m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trPopupMainQuest.gameObject.SetActive(true);
        m_csMainQuestAni.StartAinmation();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildHunting(bool bIson)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Hunting);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Hunting);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildHunting, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildHuntingDonate()
    {
        if (CsGuildManager.Instance.Guild.DailyHuntingDonationCount < CsGameConfig.Instance.GuildHuntingDonationMaxCount)
        {
            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.GuildHuntingDonationItemId) > 0)
            {
                CsGuildManager.Instance.SendGuildHuntingDonate();
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A75_TXT_03001"));
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A75_TXT_03002"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStartGuildHuntingDialog()
    {
        if (CsGuildManager.Instance.Guild != null)
        {
            OpenPopupGuildHunting();
        }
        else
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(CsGuildManager.Instance.GuildHuntingQuest.QuestNpcId);

            if (csNpcInfo != null)
            {
                UpdateNpcDialog(csNpcInfo);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingQuestAccept()
    {
        UpdateQuestPanel(EnQuestType.GuildHunting);
        ChangeQuestToggleIsOnDisplay(EnQuestType.GuildHunting, CsGuildManager.Instance.Auto);

        if (m_trPopupGuildHungting != null)
        {
            Destroy(m_trPopupGuildHungting.gameObject);
            m_trPopupGuildHungting = null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingQuestComplete()
    {
        UpdateQuestPanel(EnQuestType.GuildHunting);
        CloseQuestPopup();
        OpenPopupGuildHunting();

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingQuestUpdated()
    {
        UpdateQuestPanel(EnQuestType.GuildHunting);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingQuestAbandon()
    {
        UpdateQuestPanel(EnQuestType.GuildHunting);
        CloseQuestPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingDonate()
    {
        UpdatePopupGuildHuntingGuage();
    }
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildHuntingDonationCountUpdated()
    {
        UpdatePopupGuildHuntingGuage();
    }

    #endregion GuildHunting

    void OnEventFishingDialog(int nNpcId)
    {
        CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildTerritory.GetGuildTerritoryNpc(nNpcId);

        if (csGuildTerritoryNpc == null)
        {
            return;
        }
        else
        {
            if (csGuildTerritoryNpc.NpcId == CsGameConfig.Instance.GuildBlessingGuildTerritoryNpcId && CsGuildManager.Instance.Guild != null && CsGuildManager.Instance.MyGuildMemberGrade.GuildBlessingBuffEnabled)
            {
                UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue, null, () => StartCoroutine(LoadPopupGuildBlessingBuff()), CsConfiguration.Instance.GetString("A131_TXT_00004"));
            }
            else
            {
                UpdateGuildTerritoryNpcDialog(csGuildTerritoryNpc);
            }
        }
    }

    #region NationWar

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarNpcDialog()
    {
        CsNationWarNpc csNationWarNpc = CsGameData.Instance.NationWar.NationWarNpcList.First();

        switch (csNationWarNpc.NationWarTransmissionExitList.Count)
        {
            case 0:
                UpdatePopupNpcDialog(csNationWarNpc.Name, csNationWarNpc.Dialogue);

                break;
            case 1:
                UpdatePopupNpcDialog(csNationWarNpc.Name, csNationWarNpc.Dialogue, null, () => OnClickNationWarTransmission(0), csNationWarNpc.NationWarTransmissionExitList[0].Name);

                break;
            case 2:
                UpdatePopupNpcDialog(csNationWarNpc.Name, csNationWarNpc.Dialogue, null, () => OnClickNationWarTransmission(0), csNationWarNpc.NationWarTransmissionExitList[0].Name,
                                                                                         () => OnClickNationWarTransmission(1), csNationWarNpc.NationWarTransmissionExitList[1].Name);

                break;
            case 3:
                UpdatePopupNpcDialog(csNationWarNpc.Name, csNationWarNpc.Dialogue, null, () => OnClickNationWarTransmission(0), csNationWarNpc.NationWarTransmissionExitList[0].Name,
                                                                                         () => OnClickNationWarTransmission(1), csNationWarNpc.NationWarTransmissionExitList[1].Name,
                                                                                         () => OnClickNationWarTransmission(2), csNationWarNpc.NationWarTransmissionExitList[2].Name);

                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationWarTransmission(int nIndex)
    {
        if (CsNationWarManager.Instance.AppearNpc)
        {
            CloseNpcDialogPopup();

            CsNationWarNpc csNationWarNpc = CsGameData.Instance.NationWar.NationWarNpcList.First();
            CsNationWarManager.Instance.SendNationWarNpcTransmission(csNationWarNpc.NpcId, csNationWarNpc.NationWarTransmissionExitList[nIndex].ExitNo);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A70_TXT_04023"));
        }
    }

    #endregion NationWar

    #region 길드 제단

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildAlter(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Altar);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Altar);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildAlter, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGuildAlterDefence(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsGuildManager.Instance.StartAutoPlay(EnGuildPlayState.Defense);
        }
        else
        {
            CsGuildManager.Instance.StopAutoPlay(this, EnGuildPlayState.Defense);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.GuildAlterDefence, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenAlterNpcDialogPopup()
    {
        CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildAltar.GuildTerritoryNpc;
        if (CsGuildManager.Instance.GuildMoralPoint >= CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue);
        }
        else
        {
            //제단수비 상태 검사
            if (CsGuildManager.Instance.IsGuildDefense)
            {
                //제단수비 진행중
                UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue, null);
            }
            else
            {
                //제단수비 쿨타임 검사
                UpdatePopupNpcDialog(csGuildTerritoryNpc.Name, csGuildTerritoryNpc.Dialogue, null, OnClickGuildAlterDefence, CsConfiguration.Instance.GetString("A68_TXT_00003"), OnClickGuildAlterSpellInjection, CsConfiguration.Instance.GetString("A68_BTN_00002"), OnClickGuildAltarDonate, CsConfiguration.Instance.GetString("A68_BTN_00001"));
                m_bIsAlter = true;
                UpdateAlterDefenceButton();
            }

            UpdateAlterDonateButton();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAlterDefenceButton()
    {
        Transform trButton = m_trPopupNpcDialog.Find("ImageBackGround/ButtonList/Button0");

        if (trButton == null)
        {
            Debug.Log("trButton NULL");
        }

        if (trButton != null)
        {
            Button button0 = trButton.GetComponent<Button>();
            Text textButton0 = button0.transform.Find("Text").GetComponent<Text>();

            if (CsGuildManager.Instance.GuildAltarDefenseMissionRemainingCoolTime - Time.realtimeSinceStartup > 0)
            {
                button0.interactable = false;
                textButton0.text = string.Format(CsConfiguration.Instance.GetString("A68_TXT_00005"), (CsGuildManager.Instance.GuildAltarDefenseMissionRemainingCoolTime - Time.realtimeSinceStartup).ToString("00"));
            }
            else
            {
                if (button0.interactable != true)
                {
                    button0.interactable = true;
                }

                textButton0.text = CsConfiguration.Instance.GetString("A68_TXT_00003");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAlterDonateButton()
    {
        Button button3 = m_trPopupNpcDialog.Find("ImageBackGround/ButtonList/Button2").GetComponent<Button>();
        Text textButton3 = button3.transform.Find("Text").GetComponent<Text>();

        bool bGoldCheck = CsGameData.Instance.MyHeroInfo.Gold >= CsGameData.Instance.GuildAltar.DonationGold;
        bool bPointCheck = CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint;

        textButton3.color = bGoldCheck ? CsUIData.Instance.ColorWhite : CsUIData.Instance.ColorRed;

        if (bGoldCheck && bPointCheck)
        {
            if (button3.interactable != true)
            {
                button3.interactable = true;
            }
        }
        else
        {
            button3.interactable = false;
        }
    }


    //---------------------------------------------------------------------------------------------------
    void OnClickGuildAltarDonate()          //길드제단기부
    {
        bool bGoldCheck = CsGameData.Instance.MyHeroInfo.Gold >= CsGameData.Instance.GuildAltar.DonationGold;
        bool bPointCheck = CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint;

        if (bGoldCheck && bPointCheck)
        {
            CsGuildManager.Instance.SendGuildAltarDonate();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildAlterSpellInjection()
    {
        Debug.Log("1 OnClickGuildAlterSpellInjection  GuildMoralPoint = " + CsGuildManager.Instance.GuildMoralPoint + " // DailyHeroMaxMoralPoint = " + CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint);
        if (CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            Debug.Log("2 OnClickGuildAlterSpellInjection");
            CsGuildManager.Instance.SendGuildAltarSpellInjectionMissionStart();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildAlterDefence()
    {
        Debug.Log("1 OnClickGuildAlterDefence");
        if (CsGuildManager.Instance.GuildMoralPoint < CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint)
        {
            if (!CsGuildManager.Instance.IsGuildDefense && CsGuildManager.Instance.GuildAltarDefenseMissionRemainingCoolTime - Time.realtimeSinceStartup <= 0)
            {
                Debug.Log("2 OnClickGuildAlterDefence");
                CsGuildManager.Instance.SendGuildAltarDefenseMissionStart();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseGuildAltarNpcDialogPopup()
    {
        CloseNpcDialogPopup();
    }

    #endregion 길드 제단

    #region DailyQuest
    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDailyQuest(Toggle toggle, CsHeroDailyQuest csHeroDailyQuest)
    {
        if (toggle.isOn)
        {
            Debug.Log("csHeroDailyQuest name : " + csHeroDailyQuest.DailyQuestMission.Title);
            Debug.Log("csHeroDailyQuest : " + csHeroDailyQuest.MissionImmediateCompleted);
            if (csHeroDailyQuest.Completed)
            {
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DailyQuest, EnSubMenu.DailyQuest);
                ChangeQuestToggleIsOnDisplay((EnQuestType)(csHeroDailyQuest.SlotIndex + (int)EnQuestType.DailyQuest01), false);
            }
            else
            {
                CsDailyQuestManager.Instance.StartAutoPlay(csHeroDailyQuest.Id);
            }

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            CsDailyQuestManager.Instance.StopAutoPlay(this, csHeroDailyQuest.Id);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn((EnQuestType)((int)EnQuestType.DailyQuest01 + csHeroDailyQuest.SlotIndex), true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일 퀘스트 포기
    void OnEventDailyQuestAbandon(int nSlotIndex)
    {
        EnQuestType enQuestType = (EnQuestType)((int)EnQuestType.DailyQuest01 + nSlotIndex);
        UpdateQuestPanel(enQuestType);

        if (nSlotIndex == m_nAutoDailyQuestSlotIndex)
        {
            DisplayOffAutoCancel();
            ChangeQuestToggleIsOnDisplay((EnQuestType)(nSlotIndex + (int)EnQuestType.DailyQuest01), false);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일 퀘스트 수락
    void OnEventDailyQuestAccept()
    {
        int nQuestType = (int)EnQuestType.DailyQuest01;

        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            UpdateQuestPanel((EnQuestType)nQuestType);
            nQuestType++;

            if (GetQuestPanel((EnQuestType)nQuestType) != null && GetQuestPanel((EnQuestType)nQuestType).gameObject.activeSelf)
            {
                continue;
            }
            else
            {
                ChangeQuestToggleIsOnDisplay((EnQuestType)nQuestType, false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일 퀘스트 보상 수령
    void OnEventDailyQuestComplete(bool bLevelUp, long lAcquiredExp, int nSlotIndex)
    {
        EnQuestType enQuestType = (EnQuestType)((int)EnQuestType.DailyQuest01 + nSlotIndex);
        UpdateQuestPanel(enQuestType);

        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        if (nSlotIndex == m_nAutoDailyQuestSlotIndex)
        {
            DisplayOffAutoCancel();
            ChangeQuestToggleIsOnDisplay((EnQuestType)(nSlotIndex + (int)EnQuestType.DailyQuest01), false);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일 퀘스트 즉시 완료
    void OnEventDailyQuestMissionImmediatlyComplete()
    {
        int nQuestType = (int)EnQuestType.DailyQuest01;

        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            UpdateQuestPanel((EnQuestType)nQuestType);
            nQuestType++;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버 이벤트 : 카운트 업데이트
    void OnEventHeroDailyQuestProgressCountUpdated()
    {
        int nQuestType = (int)EnQuestType.DailyQuest01;

        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            UpdateQuestPanel((EnQuestType)nQuestType);

            Transform trQuestPanel = GetQuestPanel((EnQuestType)nQuestType);

            if (CsDailyQuestManager.Instance.HeroDailyQuestList[i].DailyQuestMission.TargetCount <= CsDailyQuestManager.Instance.HeroDailyQuestList[i].ProgressCount && 
                trQuestPanel != null && trQuestPanel.GetComponent<Toggle>().isOn == true)
            {
                CsDailyQuestManager.Instance.StopAutoPlay(this, CsDailyQuestManager.Instance.HeroDailyQuestList[i].Id);
            }

            nQuestType++;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 자동 취소
    void OnEventDailyQuestStopAutoPlay(object objCaller, Guid guidAutoQuestId)
    {
        DisplayOffAutoCancel();

        CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.GetHeroDailyQuest(guidAutoQuestId);
        ChangeQuestToggleIsOnDisplay((EnQuestType)(csHeroDailyQuest.SlotIndex + (int)EnQuestType.DailyQuest01), false);

        m_nAutoDailyQuestSlotIndex = -1;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDailyQuestStartAutoPlay(Guid guidAutoQuestId)
    {
        CsHeroDailyQuest csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList.Find(a => a.Id == guidAutoQuestId);

        if (csHeroDailyQuest == null)
        {
            m_nAutoDailyQuestSlotIndex = -1;
        }
        else
        {
            m_nAutoDailyQuestSlotIndex = csHeroDailyQuest.SlotIndex;
            AutoCancelButtonOpen((EnAutoStateType)((int)EnAutoStateType.DailyQuest01 + m_nAutoDailyQuestSlotIndex));
        }
    }

    #endregion DailyQuest

    #region WeeklyQuest

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWeeklyQuest(Toggle toggle)
    {
        if (toggle.isOn)
        {
            CsWeeklyQuestManager.Instance.StartAutoPlay();
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            CsWeeklyQuestManager.Instance.StopAutoPlay(this);

            if (CheckQuestToggleAllOff())
            {
                ChangeQuestToggleIsOn(EnQuestType.WeeklyQuest, true);
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundAccept()
    {
        UpdateQuestPanel(EnQuestType.WeeklyQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundCompleted(bool bLevelUp, long lAcquiredExp)
    {
        UpdateQuestPanel(EnQuestType.WeeklyQuest);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        if (CsWeeklyQuestManager.Instance.Auto)
        {
            CsWeeklyQuestManager.Instance.StopAutoPlay(this);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateQuestPanel(EnQuestType.WeeklyQuest);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        if (CsWeeklyQuestManager.Instance.Auto)
        {
            CsWeeklyQuestManager.Instance.StopAutoPlay(this);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateQuestPanel(EnQuestType.WeeklyQuest);
        CsUIData.Instance.PlayUISound(EnUISoundType.QuestComplete);

        if (CsWeeklyQuestManager.Instance.Auto)
        {
            CsWeeklyQuestManager.Instance.StopAutoPlay(this);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestRoundProgressCountUpdated()
    {
        UpdateQuestPanel(EnQuestType.WeeklyQuest);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestStopAutoPlay(object objCaller)
    {
        DisplayOffAutoCancel();
        ChangeQuestToggleIsOnDisplay(EnQuestType.WeeklyQuest, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeeklyQuestStartAutoPlay()
    {
        AutoCancelButtonOpen(EnAutoStateType.WeeklyQuest);
    }

    #endregion WeeklyQuest

	#region TrueHeroQuest
	//---------------------------------------------------------------------------------------------------
	void OpenPopupTrueHeroQuest()
	{
		if (m_bIsFirstQuestPopup)
		{
			InitializePopupQuest();
		}

		Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

		for (int i = 0; i < trRewardList.childCount; i++)
		{
			trRewardList.GetChild(i).gameObject.SetActive(false);
		}

		trRewardList.Find("RewardExp").gameObject.SetActive(true);
		trRewardList.Find("RewardExploitPoint").gameObject.SetActive(true);

		if (CsTrueHeroQuestManager.Instance.IsExecuted)
		{
			//완료
			m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
			m_textNpcName.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc.Name;
			m_textNpcScript.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.CompletionDialogue;

			m_buttonCancel.gameObject.SetActive(false);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
			m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
		}
		else
		{
			//수락
			m_textQuestName.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.Name;
			m_textNpcName.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc.Name;
			m_textNpcScript.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.StartDialogue;

			m_buttonCancel.onClick.RemoveAllListeners();
			m_buttonCancel.onClick.AddListener(OnClickCancelTrueHeroQuest);
			m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			m_buttonCancel.gameObject.SetActive(true);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
			m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
		}

		m_textExp.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.GetTrueHeroReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
		m_textExploit.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.GetTrueHeroReward(CsGameData.Instance.MyHeroInfo.Level).ExploitPointReward.Value.ToString("#,##0");

		m_buttonAccept.onClick.RemoveAllListeners();
		m_buttonAccept.onClick.AddListener(OnClickAcceptTrueHeroQuest);
		m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_trPopupMainQuest.gameObject.SetActive(true);
		m_csMainQuestAni.StartAinmation();
	}

	//---------------------------------------------------------------------------------------------------
	#endregion TrueHeroQuest

	#region SubQuest
	//---------------------------------------------------------------------------------------------------
	void OpenPopupSubQuest(CsSubQuest csSubQuest)
	{
		// 시작 NPC가 존재하는 경우에만 표시
		CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(csSubQuest.QuestId);

		if (m_bIsFirstQuestPopup)
		{
			InitializePopupQuest();
		}

		Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

		for (int i = 0; i < trRewardList.childCount; i++)
		{
			trRewardList.GetChild(i).gameObject.SetActive(false);
		}

		trRewardList.Find("RewardExp").gameObject.SetActive(true);
		trRewardList.Find("RewardGold").gameObject.SetActive(true);

		int nCount = 0;
		foreach (var reward in csSubQuest.SubQuestRewardList)
		{
			Transform trButtonReward = trRewardList.Find("ButtonReward" + nCount.ToString());

			if (trButtonReward == null)
			{
				trButtonReward = Instantiate(trRewardList.Find("ButtonReward0").gameObject, trRewardList).transform;
				trButtonReward.name = "ButtonReward" + nCount.ToString();
			}

			Image image = trButtonReward.Find("ImageItem").GetComponent<Image>();
			image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + reward.ItemReward.Item.Image);

			Text textName = trButtonReward.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = reward.ItemReward.Item.Name;

			Text textCount = trButtonReward.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCount);
			textCount.text = reward.ItemReward.ItemCount.ToString();

			Button button = trButtonReward.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => OnClickRewardItem(reward.RewardNo));
			button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}

		if (csHeroSubQuest == null ||
			csHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon)
		{
			//수락, 재수락
			m_textQuestName.text = csSubQuest.Title;
			m_textNpcName.text = csSubQuest.StartNpc.Name;
			m_textNpcScript.text = csSubQuest.StartDialogue;

			m_buttonCancel.onClick.RemoveAllListeners();
			m_buttonCancel.onClick.AddListener(OnClickCancelSubQuest);
			m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			m_buttonCancel.gameObject.SetActive(true);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
			m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
		}
		else
		{
			//완료 대기
			m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
			m_textNpcName.text = csSubQuest.CompletionNpc.Name;
			m_textNpcScript.text = csSubQuest.CompletionDialogue;

			m_buttonCancel.gameObject.SetActive(false);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
			m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
		}

		m_textExp.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.GetTrueHeroReward(CsGameData.Instance.MyHeroInfo.Level).ExpReward.Value.ToString("#,##0");
		m_textGold.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.GetTrueHeroReward(CsGameData.Instance.MyHeroInfo.Level).ExploitPointReward.Value.ToString("#,##0");

		m_buttonAccept.onClick.RemoveAllListeners();
		m_buttonAccept.onClick.AddListener(() => OnClickAcceptSubQuest(csSubQuest));
		m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_trPopupMainQuest.gameObject.SetActive(true);
		m_csMainQuestAni.StartAinmation();
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedSubQuest(bool bIsOn, int nSubQuestId)
	{
		CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(nSubQuestId);

		if (csHeroSubQuest == null)
			return;

		if (bIsOn)
		{
			m_nSelectedSubQuest++;
			m_nSelectedSubQuestId = nSubQuestId;
			CsSubQuestManager.Instance.StartAutoPlay(nSubQuestId);
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
		}
		else
		{
			m_nSelectedSubQuest--;
			CsSubQuestManager.Instance.StopAutoPlay(this);

			if (CheckQuestToggleAllOff())
			{
				ChangeQuestToggleIsOn(EnQuestType.SubQuest, true, nSubQuestId);
			}
			else
			{
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	#endregion SubQuest

	#region BiographyQuest
	//---------------------------------------------------------------------------------------------------
	void OpenPopupBiographyQuest(CsBiographyQuest csBiographyQuest)
	{
		if (m_bIsFirstQuestPopup)
		{
			InitializePopupQuest();
		}

		Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

		for (int i = 0; i < trRewardList.childCount; i++)
		{
			trRewardList.GetChild(i).gameObject.SetActive(false);
		}

		trRewardList.Find("RewardExp").gameObject.SetActive(true);
		m_textExp.text = csBiographyQuest.ExpReward.Value.ToString("#,##0");
		

		CsHeroBiographyQuest csHeroBiographyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(csBiographyQuest.BiographyId);

		if (csHeroBiographyQuest == null ||
			csHeroBiographyQuest.QuestNo < csBiographyQuest.QuestNo)
		{
			// 퀘스트가 없는 상태이거나 다음 퀘스트인 경우 수락
			CsBiography csBiography = CsGameData.Instance.GetBiography(csBiographyQuest.BiographyId);
			m_textQuestName.text = string.Format(csBiography.TargetTitle, csBiography.Name, csBiographyQuest.QuestNo);
			m_textNpcName.text = csBiographyQuest.StartNpc.Name;
			m_textNpcScript.text = csBiographyQuest.StartDialogue;

			m_buttonCancel.onClick.RemoveAllListeners();
			m_buttonCancel.onClick.AddListener(OnClickCancelBiographyQuest);
			m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			m_buttonCancel.gameObject.SetActive(true);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
			m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
		}
		else if (csHeroBiographyQuest.Excuted)
		{
			//완료 대기
			m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
			m_textNpcName.text = csBiographyQuest.CompletionNpc.Name;
			m_textNpcScript.text = csBiographyQuest.CompletionDialogue;

			m_buttonCancel.gameObject.SetActive(false);
			m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
			m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
		}

		m_buttonAccept.onClick.RemoveAllListeners();
		m_buttonAccept.onClick.AddListener(() => OnClickAcceptBiographyQuest(csBiographyQuest));
		m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		m_trPopupMainQuest.gameObject.SetActive(true);
		m_csMainQuestAni.StartAinmation();
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedBiographyQuest(bool bIsOn, int nBiographyId)
	{
		CsHeroBiographyQuest csHeroBiograhyQuest = CsBiographyManager.Instance.GetHeroBiographyQuest(nBiographyId);
		
		if (csHeroBiograhyQuest == null)
			return;

		if (bIsOn)
		{
			CsBiographyManager.Instance.StartAutoPlay(nBiographyId);
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
		}
		else
		{
			CsBiographyManager.Instance.StopAutoPlay(this);

			if (CheckQuestToggleAllOff())
			{
				ChangeQuestToggleIsOn(EnQuestType.Biography, true, nBiographyId);
			}
			else
			{
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void BiographyQuestDungeonReEnter(int nDungeonId)
	{
		CsDungeonManager.Instance.ContinentExitForBiographyQuest(nDungeonId);
		CloseNpcDialogPopup();
	}

	//---------------------------------------------------------------------------------------------------
	#endregion BiographyQuest

	#region CreatureFarmQuest

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedCreatureFarmQuest(bool bIsOn)
	{
		if (bIsOn)
		{
			CsCreatureFarmQuestManager.Instance.StartAutoPlay();
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
		}
		else
		{
			CsCreatureFarmQuestManager.Instance.StopAutoPlay(this);

			if (CheckQuestToggleAllOff())
			{
				ChangeQuestToggleIsOn(EnQuestType.CreatureFarm, true);
			}
			else
			{
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupCreatureFarmQuest()
	{
		if (m_bIsFirstQuestPopup)
		{
			InitializePopupQuest();
		}

		Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

		for (int i = 0; i < trRewardList.childCount; i++)
		{
			trRewardList.GetChild(i).gameObject.SetActive(false);
		}

		CsCreatureFarmQuest csCreatureFarmQuest = CsCreatureFarmQuestManager.Instance.CreatureFarmQuest;

		if (csCreatureFarmQuest == null)
		{
			return;
		}
		else
		{
			if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.None &&
				CsCreatureFarmQuestManager.Instance.CreatureFarmQuestAcceptionCountDate < CsGameData.Instance.MyHeroInfo.CurrentDateTime &&
				CsCreatureFarmQuestManager.Instance.DailyCreatureFarmQuestAcceptionCount <= 0)
			{
				// 수락
				m_textQuestName.text = csCreatureFarmQuest.Name;
				m_textNpcName.text = csCreatureFarmQuest.StartNpc.Name;
				m_textNpcScript.text = csCreatureFarmQuest.StartDialogue;

				m_buttonCancel.onClick.RemoveAllListeners();
				m_buttonCancel.onClick.AddListener(OnClickCancelCreatureFarmQuest);
				m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

				m_buttonCancel.gameObject.SetActive(true);
				m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
				m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
			}
			else if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
			{
				// 완료
				m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
				m_textNpcName.text = csCreatureFarmQuest.CompletionNpc.Name;
				m_textNpcScript.text = csCreatureFarmQuest.CompletionDialogue;

				m_buttonCancel.gameObject.SetActive(false);
				m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
				m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
			}
		}

		m_buttonAccept.onClick.RemoveAllListeners();
		m_buttonAccept.onClick.AddListener(() => OnClickAcceptCreatureFarmQuest());
		m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		trRewardList.Find("RewardExp").gameObject.SetActive(true);

		CsCreatureFarmQuestExpReward csCreatureFarmQuestExpReward = csCreatureFarmQuest.GetCreatureFarmQuestExpReward(CsGameData.Instance.MyHeroInfo.Level);
		
		if (csCreatureFarmQuestExpReward != null)
		{
			m_textExp.text = csCreatureFarmQuestExpReward.ExpReward.Value.ToString("#,##0");
		}

		int nItemCount = 0;
		foreach (CsCreatureFarmQuestItemReward csCreatureFarmQuestItemReward in csCreatureFarmQuest.CreatureFarmQuestItemRewardList)
		{
			if (nItemCount < 2)
			{
				Transform trReward = trRewardList.Find("ButtonReward" + nItemCount.ToString());
				trReward.gameObject.SetActive(true);

				Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();
				imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csCreatureFarmQuestItemReward.ItemReward.Item.Image);

				Text textName = trReward.Find("TextName").GetComponent<Text>();
				textName.text = csCreatureFarmQuestItemReward.ItemReward.Item.Name;
				CsUIData.Instance.SetFont(textName);

				Text textCount = trReward.Find("TextCount").GetComponent<Text>();
				textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), csCreatureFarmQuestItemReward.ItemReward.ItemCount);
				CsUIData.Instance.SetFont(textCount);

				Button buttonReward = trReward.GetComponent<Button>();
				buttonReward.onClick.RemoveAllListeners();
				buttonReward.onClick.AddListener(() => OnClickRewardItem(csCreatureFarmQuestItemReward.ItemReward.Item.ItemId, csCreatureFarmQuestItemReward.ItemReward.ItemCount, csCreatureFarmQuestItemReward.ItemReward.ItemOwned));
				buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
			}

			nItemCount++;
		}

		m_trPopupMainQuest.gameObject.SetActive(true);
		m_csMainQuestAni.StartAinmation();
	}

	//---------------------------------------------------------------------------------------------------

	#endregion CreatureFarmQuest

	#region JobChangeQuest

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedJobChangeQuest(bool bIsOn)
	{
		if (bIsOn)
		{
			CsJobChangeManager.Instance.StartAutoPlay();
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            if (m_bStartJobChangeQuest)
            {
                return;
            }
            else
            {
                m_bStartJobChangeQuest = true;
                UpdateQuestPanel(EnQuestType.JobChange);
            }
		}
		else
		{
			CsJobChangeManager.Instance.StopAutoPlay(this);

			if (CheckQuestToggleAllOff())
			{
				ChangeQuestToggleIsOn(EnQuestType.JobChange, true);
			}
			else
			{
				return;
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopupJobChangeQuest()
    {
        Debug.Log("OpenPopupJobChangeQuest");
        CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

        CsJobChangeQuest csJobChangeQuest = null;

        if (csHeroJobChangeQuest != null && 
            ((CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo + 1) != null && CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo + 1).JobChangeQuestDifficultyList != null && 
            CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo + 1).JobChangeQuestDifficultyList.Count > 0 && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed) || 
            (CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo) != null && CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo).JobChangeQuestDifficultyList != null && 
            CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo).JobChangeQuestDifficultyList.Count > 0 && csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Failed)))
        {
            if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Failed)
            {
                csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);
            }
            else if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
            {
                csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo + 1);
            }

            if (csJobChangeQuest == null)
            {
                return;
            }
            else
            {
                CsNpcInfo csNpcInfo = csJobChangeQuest.QuestNpc;

                if (csNpcInfo == null)
                {
                    return;
                }
                else
                {
                    UpdatePopupNpcDialog(csNpcInfo.Name, csJobChangeQuest.StartDialogue, null,
                                         () => OnClickJobChangeQuestDifficult(csJobChangeQuest.JobChangeQuestDifficultyList[1]), csJobChangeQuest.JobChangeQuestDifficultyList[1].Name,
                                         () => OnClickJobChangeQuestEasy(csJobChangeQuest.JobChangeQuestDifficultyList[0]), csJobChangeQuest.JobChangeQuestDifficultyList[0].Name);
                }
            }
        }
        else
        {
            if (m_bIsFirstQuestPopup)
            {
                InitializePopupQuest();
            }

            Transform trRewardList = m_trPopupMainQuest.Find("ImageBackGround/TextClearReward/RewardList");

            for (int i = 0; i < trRewardList.childCount; i++)
            {
                trRewardList.GetChild(i).gameObject.SetActive(false);
            }

            if (csHeroJobChangeQuest == null)
            {
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
                {
                    return;
                }
                else
                {
                    csJobChangeQuest = CsGameData.Instance.JobChangeQuestList.First();
                    // 퀘스트 수락
                    m_textQuestName.text = csJobChangeQuest.Title;
                    m_textNpcName.text = csJobChangeQuest.QuestNpc.Name;
                    m_textNpcScript.text = csJobChangeQuest.StartDialogue;

                    m_buttonCancel.onClick.RemoveAllListeners();
                    m_buttonCancel.onClick.AddListener(OnClickCancelCreatureFarmQuest);
                    m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                    m_buttonCancel.gameObject.SetActive(true);
                    m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
                    m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
                }
            }
            else
            {
                csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);

                if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)
                {
                    // 퀘스트 수락
                    m_textQuestName.text = csJobChangeQuest.Title;
                    m_textNpcName.text = csJobChangeQuest.QuestNpc.Name;
                    m_textNpcScript.text = csJobChangeQuest.StartDialogue;

                    m_buttonCancel.onClick.RemoveAllListeners();
                    m_buttonCancel.onClick.AddListener(() => OnClickJobChangeQuest(0));
                    m_buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                    m_buttonCancel.gameObject.SetActive(true);
                    m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
                    m_csMainQuestAni.UIType = EnUIAnimationType.Quest;
                }
                else if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Accepted && csJobChangeQuest.TargetCount <= CsJobChangeManager.Instance.HeroJobChangeQuest.ProgressCount)
                {
                    // 완료
                    m_textQuestName.text = CsConfiguration.Instance.GetString("A12_TXT_03002");
                    m_textNpcName.text = csJobChangeQuest.QuestNpc.Name;
                    m_textNpcScript.text = csJobChangeQuest.CompletionDialogue;

                    m_buttonCancel.gameObject.SetActive(false);
                    m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
                    m_csMainQuestAni.UIType = EnUIAnimationType.QuestClear;
                }
            }

            m_buttonAccept.onClick.RemoveAllListeners();
            m_buttonAccept.onClick.AddListener(() => OnClickJobChangeQuest());
            m_buttonAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            if (csHeroJobChangeQuest != null)
            {
                CsItemReward csItemReward = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo).CompletionItemReward;

                if (csItemReward == null)
                {

                    Transform trReward = trRewardList.Find("ButtonReward" + 0);
                    trReward.gameObject.SetActive(false);
                }
                else
                {
                    Transform trReward = trRewardList.Find("ButtonReward" + 0);
                    trReward.gameObject.SetActive(true);

                    Image imageItem = trReward.Find("ImageItem").GetComponent<Image>();
                    imageItem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItemReward.Item.Image);

                    Text textName = trReward.Find("TextName").GetComponent<Text>();
                    textName.text = csItemReward.Item.Name;
                    CsUIData.Instance.SetFont(textName);

                    Text textCount = trReward.Find("TextCount").GetComponent<Text>();
                    textCount.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01004"), csItemReward.ItemCount);
                    CsUIData.Instance.SetFont(textCount);

                    Button buttonReward = trReward.GetComponent<Button>();
                    buttonReward.onClick.RemoveAllListeners();
                    buttonReward.onClick.AddListener(() => OnClickRewardItem(csItemReward.Item.ItemId, csItemReward.ItemCount, csItemReward.ItemOwned));
                    buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                }
            }

            m_trPopupMainQuest.gameObject.SetActive(true);
            m_csMainQuestAni.StartAinmation();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJobChangeQuestEasy(CsJobChangeQuestDifficulty csJobChangeQuestDifficulty)
    {
        if (csJobChangeQuestDifficulty == null)
        {
            return;
        }
        else
        {
            if (CsGuildManager.Instance.Guild == null)
            {
                // 길드가 없음
                CsGameEventUIToUI.Instance.OnEventConfirm("A153_TXT_00001",
                                                          CsConfiguration.Instance.GetString("A153_TXT_00002"), () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember),
                                                          CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), null, true);
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
                {
                    // 레벨 부족
                }
                else
                {
                    CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

                    if (csHeroJobChangeQuest == null)
                    {
                        AcceptJobChangeQuestEasy(csJobChangeQuestDifficulty);
                    }
                    else
                    {
                        if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Accepted)
                        {
                            // 진행중
                        }
                        else
                        {
                            AcceptJobChangeQuestEasy(csJobChangeQuestDifficulty);
                        }
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AcceptJobChangeQuestEasy(CsJobChangeQuestDifficulty csJobChangeQuestDifficulty)
    {
        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuestDifficulty.QuestNo, csJobChangeQuestDifficulty.Difficulty);

        // 길드 영지 이동
        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A67_TXT_00002"),
                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CsGuildManager.Instance.SendContinentExitForGuildTerritoryEnter,
                                      CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);

        CloseNpcDialogPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJobChangeQuestDifficult(CsJobChangeQuestDifficulty csJobChangeQuestDifficulty)
    {
        if (csJobChangeQuestDifficulty == null)
        {

        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
            {

            }
            else
            {
                CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

                if (csHeroJobChangeQuest == null)
                {
                    CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuestDifficulty.QuestNo, csJobChangeQuestDifficulty.Difficulty);
                }
                else
                {
                    if (csHeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Accepted)
                    {
                        // 진행 중
                    }
                    else
                    {
                        CsJobChangeManager.Instance.SendJobChangeQuestAccept(csJobChangeQuestDifficulty.QuestNo, csJobChangeQuestDifficulty.Difficulty);
                    }
                }
            }
        }

        CloseNpcDialogPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateJobChangeQuestWaitingTime()
    {
        CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;

        if (csHeroJobChangeQuest == null)
        {
            return;
        }
        else
        {
            if (csHeroJobChangeQuest.RemainingTime < 0f)
            {
                return;
            }
            else
            {
                if (m_textTimeJobChangeQuest == null)
                {
                    return;
                }
                else
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(csHeroJobChangeQuest.RemainingTime - Time.realtimeSinceStartup);
                    m_textTimeJobChangeQuest.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
                }
            }
        }
    }

	//---------------------------------------------------------------------------------------------------

	#endregion JobChangeQuest

	#region PanelQuest

	//---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	Transform InitializeQuestPanel(EnQuestType enQuestType, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}
    
        Transform trQuest = m_trQuestPartyPanel.Find("Quest");

        Transform trArrow = trQuest.Find("ImageArrow");
        trArrow.gameObject.SetActive(false);

		Transform trQuestContent = trQuest.Find("Scroll View/Viewport/Content");
		Transform trQuestPanel = trQuestContent.Find(strName);

        if (trQuestPanel == null)
        {
            trQuestPanel = Instantiate(m_goQuestPanel, trQuestContent).transform;
			trQuestPanel.name = strName;
        }

        Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();
        toggleQuestPanel.onValueChanged.RemoveAllListeners();
        toggleQuestPanel.group = trQuestContent.GetComponent<ToggleGroup>();

        Text textTitie = trQuestPanel.Find("TextTitle").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTitie);

        Text textTarget = trQuestPanel.Find("TextTarget").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTarget);

        Text textTime = trQuestPanel.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTime);
        textTime.gameObject.SetActive(false);

        Image imageTime = trQuestPanel.Find("ImageTime").GetComponent<Image>();
        imageTime.gameObject.SetActive(false);

        Image imageComplete = trQuestPanel.Find("ImageComplete").GetComponent<Image>();
        imageComplete.gameObject.SetActive(false);

        int nHeroDailyQuestIndex = (int)enQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (enQuestType)
        {
            case EnQuestType.MainQuest:
                textTitie.color = CsUIData.Instance.ColorSkyblue;
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedMainQuest(toggleQuestPanel));

                m_trMainQuestToggle = trQuestPanel;
                break;

            case EnQuestType.ThreatOfFarm:
                textTitie.color = new Color32(175, 213, 122, 255);

                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock01");

                textTime.color = new Color32(225, 150, 150, 255);
                m_textTimeThreatOfFarm = textTime;
                CsUIData.Instance.SetFont(m_textTimeThreatOfFarm);

                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedThreatOfFarm(toggleQuestPanel));
                break;

            case EnQuestType.DailyQuest01:
                if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    textTitie.color = new Color32(40, 183, 197, 255);
                    toggleQuestPanel.isOn = false;

                    csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                else
                {

                }
                
                break;

            case EnQuestType.DailyQuest02:
                if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    textTitie.color = new Color32(40, 183, 197, 255);
                    toggleQuestPanel.isOn = false;

                    csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                else
                {

                }
                break;

            case EnQuestType.DailyQuest03:
                if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    textTitie.color = new Color32(40, 183, 197, 255);
                    toggleQuestPanel.isOn = false;

                    csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestIndex];
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                else
                {
                }
                break;

            case EnQuestType.WeeklyQuest:
                textTitie.color = new Color32(206, 170, 139, 255);
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedWeeklyQuest(toggleQuestPanel));
                break;

            case EnQuestType.BountyHunter:
                textTitie.color = new Color32(175, 213, 122, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedBountyHunter);
                break;

            case EnQuestType.GuildAlter:
                textTitie.color = new Color32(255, 102, 204, 255);
                toggleQuestPanel.isOn = false;
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildAlter(toggleQuestPanel));
                break;

            case EnQuestType.GuildAlterDefence:
                textTitie.color = new Color32(255, 102, 204, 255);
                toggleQuestPanel.isOn = false;
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildAlterDefence(toggleQuestPanel));
                break;

            case EnQuestType.GuildFarm:
                textTitie.color = new Color32(255, 102, 204, 255);
                toggleQuestPanel.isOn = false;
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildFarmQuest(toggleQuestPanel));
                break;

            case EnQuestType.GuildMission:
                textTitie.color = new Color32(255, 102, 204, 255);
                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock01");
                textTime.color = new Color32(255, 150, 150, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildMission);
                break;

            case EnQuestType.GuildSupplySupport:
                textTitie.color = new Color32(255, 102, 204, 255);
                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock01");
                m_textTImeGuildSupplySupport = textTime;
                textTime.color = new Color32(255, 150, 150, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildSupplySupport);
                break;

            case EnQuestType.GuildHunting:
                textTitie.color = new Color32(255, 102, 204, 255);
                toggleQuestPanel.isOn = false;
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildHunting);
                break;

            case EnQuestType.SecretLetter:
                textTitie.color = new Color32(254, 148, 0, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedSecretLetterQuest);
                break;

            case EnQuestType.MysteryBox:
                textTitie.color = new Color32(254, 148, 0, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedMysteryBox);
                break;

            case EnQuestType.DimensionRaid:
                textTitie.color = new Color32(254, 148, 0, 255);
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedDimensionRaidQuest);
                break;

            case EnQuestType.HolyWar:
                textTitie.color = new Color32(254, 148, 0, 255);

                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock02");
                imageTime.gameObject.SetActive(true);

                textTime.color = new Color32(161, 245, 141, 255);
                textTime.gameObject.SetActive(true);

                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedHolyWarQuest);
                break;

            case EnQuestType.SupplySupport:
                textTitie.color = new Color32(254, 148, 0, 255);

                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueSupplySupport(toggleQuestPanel));

                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock01");

                textTime.color = new Color32(225, 150, 150, 255);
                m_textTImeSupplySupport = textTime;
                break;

            case EnQuestType.Dungeon:
                textTitie.color = CsUIData.Instance.ColorSkyblue;

                imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock02");
                textTime.color = new Color32(161, 245, 141, 255);

                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDungeon(toggleQuestPanel));

                m_trToggleDungeonQuest = trQuestPanel;
                trQuestPanel.gameObject.SetActive(false);
                break;

			case EnQuestType.TrueHero:
				m_textTimeTrueHeroQuest = textTime;

				textTitie.color = new Color32(254, 148, 0, 255);
				
				//imageTime.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_clock02");
				//imageTime.gameObject.SetActive(true);

				//textTime.color = new Color32(161, 245, 141, 255);
				//textTime.gameObject.SetActive(true);

                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedTrueHeroQuest);

				break;

			case EnQuestType.SubQuest:
				textTitie.color = new Color32(255, 214, 80, 255);
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedSubQuest(isOn, nParam));
				break;

			case EnQuestType.Biography:
				textTitie.color = new Color32(254, 148, 0, 255);
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedBiographyQuest(isOn, nParam));
				break;

			case EnQuestType.CreatureFarm:
				m_textTimeCreatureFarmQuest = textTime;
				textTitie.color = new Color32(175, 213, 122, 255);
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedCreatureFarmQuest(isOn));
				break;

			case EnQuestType.JobChange:
                m_textTimeJobChangeQuest = textTime;
				textTitie.color = new Color32(171, 130, 255, 255);
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedJobChangeQuest(isOn));
				break;
        }

        m_listQuestPanel.Add(trQuestPanel);

        return trQuestPanel;
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void ChangeQuestToggleIsOn(EnQuestType enQuestType, bool bIsOn, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");

		Transform trQuestPanel = trQuestContent.Find(strName);

		if (!UpdateQuestPanelDisplay(enQuestType, nParam))
        {
			ResetQuestPanelDisplay(enQuestType, nParam);
			UpdateQuestPanel(enQuestType, nParam);
        }

        if (trQuestPanel != null)
        {
			Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();
            toggleQuestPanel.isOn = bIsOn;
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	void ChangeQuestToggleIsOnDisplay(EnQuestType enQuestType, bool bIsOn, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

        Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");

		Transform trQuestPanel = trQuestContent.Find(strName);

        if (trQuestPanel == null)
        {
            return;
        }

        Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();
        toggleQuestPanel.onValueChanged.RemoveAllListeners();
        toggleQuestPanel.isOn = bIsOn;

        int nHeroDailyQuestSlotIndex = (int)enQuestType - (int)EnQuestType.DailyQuest01;
        CsHeroDailyQuest csHeroDailyQuest = null;

        switch (enQuestType)
        {
            case EnQuestType.MainQuest:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedMainQuest(toggleQuestPanel));
                break;

            case EnQuestType.ThreatOfFarm:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedThreatOfFarm(toggleQuestPanel));
                break;

            case EnQuestType.BountyHunter:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedBountyHunter);
                break;

            case EnQuestType.DailyQuest01:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestSlotIndex];
                
                if (csHeroDailyQuest == null)
                {
                    break;
                }
                else
                {
                    toggleQuestPanel.onValueChanged.RemoveAllListeners();
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                break;

            case EnQuestType.DailyQuest02:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestSlotIndex];
                
                if (csHeroDailyQuest == null)
                {
                    break;
                }
                else
                {
                    toggleQuestPanel.onValueChanged.RemoveAllListeners();
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                break;

            case EnQuestType.DailyQuest03:
                csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nHeroDailyQuestSlotIndex];
                
                if (csHeroDailyQuest == null)
                {
                    break;
                }
                else
                {
                    toggleQuestPanel.onValueChanged.RemoveAllListeners();
                    toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                }
                break;

            case EnQuestType.WeeklyQuest:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedWeeklyQuest(toggleQuestPanel));
                break;

            case EnQuestType.GuildAlter:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildAlter(toggleQuestPanel));
                break;

            case EnQuestType.GuildAlterDefence:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildAlterDefence(toggleQuestPanel));
                break;

            case EnQuestType.GuildFarm:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedGuildFarmQuest(toggleQuestPanel));
                break;

            case EnQuestType.GuildMission:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildMission);
                break;

            case EnQuestType.GuildSupplySupport:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildSupplySupport);
                break;

            case EnQuestType.GuildHunting:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedGuildHunting);
                break;

            case EnQuestType.SecretLetter:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedSecretLetterQuest);
                break;

            case EnQuestType.MysteryBox:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedMysteryBox);
                break;

            case EnQuestType.DimensionRaid:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedDimensionRaidQuest);
                break;

            case EnQuestType.HolyWar:
                toggleQuestPanel.onValueChanged.AddListener(OnValueChangedHolyWarQuest);
                break;

            case EnQuestType.SupplySupport:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueSupplySupport(toggleQuestPanel));
                break;

            case EnQuestType.Dungeon:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDungeon(toggleQuestPanel));
                break;

			case EnQuestType.TrueHero:
				toggleQuestPanel.onValueChanged.AddListener(OnValueChangedTrueHeroQuest);
				break;

			case EnQuestType.SubQuest:
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedSubQuest(isOn, nParam));
				break;

			case EnQuestType.Biography:
				toggleQuestPanel.onValueChanged.AddListener((isOn) => OnValueChangedBiographyQuest(isOn, nParam));
				break;

			case EnQuestType.CreatureFarm:
				toggleQuestPanel.onValueChanged.AddListener(OnValueChangedCreatureFarmQuest);
				break;

            case EnQuestType.JobChange:
                toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedJobChangeQuest(ison));
                break;
        }
    }


    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	Transform GetQuestPanel(EnQuestType enQuestType, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

		if (m_trQuestPartyPanel != null)
		{
			Transform trQuestContent = m_trQuestPartyPanel.Find("Quest/Scroll View/Viewport/Content");

			for (int i = 0; i < trQuestContent.childCount; i++)
			{
				if (trQuestContent.GetChild(i).name == strName)
				{
					return trQuestContent.GetChild(i);
				}
			}
		}

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    void AllPanelQuestOff()
    {
        Transform trQuest = m_trQuestPartyPanel.Find("Quest");
        Transform trQuestContent = trQuest.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < trQuestContent.childCount; i++)
        {
            trQuestContent.GetChild(i).gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateQuestPanel()
    {   
        foreach (EnQuestType enumItem in Enum.GetValues(typeof(EnQuestType)))
        {
			if (enumItem == EnQuestType.SubQuest)
			{
				foreach (var subQuest in CsSubQuestManager.Instance.GetAcceptionSubQuestList())
				{
					UpdateQuestPanel(enumItem, subQuest.SubQuest.QuestId);
				}
			}
			else if (enumItem == EnQuestType.Biography)
			{
				foreach (var subQuest in CsBiographyManager.Instance.GetAcceptionQuests())
				{
					UpdateQuestPanel(enumItem, subQuest.BiographyQuest.BiographyId);
				}
			}
			else
			{
				UpdateQuestPanel(enumItem);
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadQuestPanel()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/QuestPanel");
        yield return resourceRequest;
        m_goQuestPanel = (GameObject)resourceRequest.asset;

        m_bIsLoadQuestPanel = true;
        InitializeQuestPanel();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeQuestPanel()
    {
        if (CsGameData.Instance.MyHeroInfo.LocationId == CsGameData.Instance.UndergroundMaze.LocationId)
        {
            DungeonEnter();
            UpdateUndergroundMazeDungeonPanel();
        }
        else if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
        {
            GuildTerritoryEnter();
        }
        else
        {
            CsUIData.Instance.DungeonInNow = EnDungeon.None;
            DungeonExit();
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
    void UpdateQuestPanel(EnQuestType enQuestType, int nParam = 0)
    {
		if (!UpdateQuestPanelDisplay(enQuestType, nParam))
        {
            return;
        }
        else
        {
			Transform trQuestPanel = GetQuestPanel(enQuestType, nParam);

            if (trQuestPanel == null)
            {
				trQuestPanel = InitializeQuestPanel(enQuestType, nParam);
            }

            Text textTitle = trQuestPanel.Find("TextTitle").GetComponent<Text>();
            Text textTarget = trQuestPanel.Find("TextTarget").GetComponent<Text>();
            Text textTime = trQuestPanel.Find("TextTime").GetComponent<Text>();

            Transform trImageTime = trQuestPanel.Find("ImageTime");
            Transform trImageComplete = trQuestPanel.Find("ImageComplete");

            int nIndexHeroDailyQuest = (int)enQuestType - (int)EnQuestType.DailyQuest01;
            CsHeroDailyQuest csHeroDailyQuest = null;

            switch (enQuestType)
            {
                case EnQuestType.MainQuest:
                    textTarget.text = CsMainQuestManager.Instance.ObjectiveMessage;
                    var trRenewal = trQuestPanel.Find("QuestRenewal");//UNDONE
                    if (CsMainQuestManager.Instance.MainQuest != null &&
						CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.Completed)
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01001"), CsMainQuestManager.Instance.MainQuest.Title);

                        if (CsMainQuestManager.Instance.IsAccepted)
                        {
                            //수락했을때
                            if (CsMainQuestManager.Instance.IsExecuted)
                            {
                                //미완료
                                trImageComplete.gameObject.SetActive(true);

                                // 수락 이펙트 추가
                                if (trRenewal != null) trRenewal.gameObject.SetActive(true);
                            }
                            else
                            {
                                if (trRenewal != null) trRenewal.gameObject.SetActive(false);

                                trImageComplete.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            trImageComplete.gameObject.SetActive(false);
                            if (trRenewal != null) trRenewal.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        textTitle.text = CsConfiguration.Instance.GetString("A12_TXT_00002");
                        trImageComplete.gameObject.SetActive(true);
                        if (trRenewal != null) trRenewal.gameObject.SetActive(false);
                    }

                    if (m_bIsInDungeon)
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(true);
                    }
                    break;

                case EnQuestType.ThreatOfFarm:
                    textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), CsThreatOfFarmQuestManager.Instance.Quest.Title);

                    if (CsThreatOfFarmQuestManager.Instance.Quest != null)
                    {
                        if (CsThreatOfFarmQuestManager.Instance.IsAccepted)
                        {
                            if (CsThreatOfFarmQuestManager.Instance.IsExecuted)
                            {
                                if (CsThreatOfFarmQuestManager.Instance.IsComplete)
                                {
                                    //퀘스트 완료
                                    trQuestPanel.gameObject.SetActive(false);
                                }
                                else
                                {
                                    //미션완료                        
                                    trQuestPanel.gameObject.SetActive(true);
                                    trImageTime.gameObject.SetActive(false);
                                    m_textTimeThreatOfFarm.gameObject.SetActive(false);
                                    textTarget.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.CompletionText, CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc.Name);
                                }
                            }
                            else
                            {
                                //미션중
                                if (CsThreatOfFarmQuestManager.Instance.Mission == null)
                                {
                                    //시간초과 , 포기 등으로 퀘스트 중단상태.
                                    trQuestPanel.gameObject.SetActive(false);
                                }
                                else
                                {
                                    if (CsThreatOfFarmQuestManager.Instance.Monster == null)
                                    {
                                        textTarget.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.TargetMovingText, CsThreatOfFarmQuestManager.Instance.Mission.TargetPositionName);
                                        trImageTime.gameObject.SetActive(false);
                                        m_textTimeThreatOfFarm.gameObject.SetActive(false);

                                        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
                                        {
                                            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                                            m_IEnumeratorThreatOfFameMonsterInfo = null;
                                        }
                                    }
                                    else
                                    {
                                        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
                                        {
                                            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                                            m_IEnumeratorThreatOfFameMonsterInfo = null;
                                        }

                                        m_IEnumeratorThreatOfFameMonsterInfo = UpdateThreatOfFameMonsterInfo(textTarget);
                                        StartCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
                                        
                                        //시간체크 시작
                                        trImageTime.gameObject.SetActive(true);
                                        m_textTimeThreatOfFarm.gameObject.SetActive(true);
                                        m_flThreatOfFarmMissionRemainingTime = CsThreatOfFarmQuestManager.Instance.Monster.RemainingLifeTime + Time.realtimeSinceStartup;
                                    }

                                    trQuestPanel.gameObject.SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            //수락상태가 아님
                            trQuestPanel.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    break;

                case EnQuestType.BountyHunter:
                    CsBountyHunterQuest csBountyHunterQuest = CsBountyHunterQuestManager.Instance.BountyHunterQuest;

                    if (csBountyHunterQuest == null)
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    else
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), CsConfiguration.Instance.GetString(csBountyHunterQuest.TargetTitle));
                        textTarget.text = string.Format(CsConfiguration.Instance.GetString(csBountyHunterQuest.TargetContent), CsBountyHunterQuestManager.Instance.ProgressCount, csBountyHunterQuest.TargetCount);
                        trQuestPanel.gameObject.SetActive(true);
                    }
                    break;

                case EnQuestType.DailyQuest01:
                    if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nIndexHeroDailyQuest];

                            if (csHeroDailyQuest == null)
                            {
                                trQuestPanel.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (csHeroDailyQuest.IsAccepted)
                                {
                                    textTitle.text = string.Format(CsConfiguration.Instance.GetString(CsGameData.Instance.DailyQuest.Title), csHeroDailyQuest.DailyQuestMission.Title);
                                    
                                    if (csHeroDailyQuest.Completed)
                                    {
                                        textTarget.text = CsConfiguration.Instance.GetString("A101_TXT_00001");
                                        trImageComplete.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        switch (csHeroDailyQuest.DailyQuestMission.Type)
                                        {
                                            case 1:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;

                                            case 2:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;
                                        }

                                        trImageComplete.gameObject.SetActive(false);
                                    }

                                    trQuestPanel.gameObject.SetActive(true);

                                    Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();

                                    if (toggleQuestPanel == null)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        toggleQuestPanel.onValueChanged.RemoveAllListeners();
                                        toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                                    }
                                }
                                else
                                {
                                    trQuestPanel.gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            trQuestPanel.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    
                    break;

                case EnQuestType.DailyQuest02:
                    if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nIndexHeroDailyQuest];

                            if (csHeroDailyQuest == null)
                            {
                                trQuestPanel.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (csHeroDailyQuest.IsAccepted)
                                {
                                    textTitle.text = string.Format(CsConfiguration.Instance.GetString(CsGameData.Instance.DailyQuest.Title), csHeroDailyQuest.DailyQuestMission.Title);

                                    if (csHeroDailyQuest.Completed)
                                    {
                                        textTarget.text = CsConfiguration.Instance.GetString("A101_TXT_00001");
                                        trImageComplete.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        switch (csHeroDailyQuest.DailyQuestMission.Type)
                                        {
                                            case 1:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;

                                            case 2:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;
                                        }

                                        trImageComplete.gameObject.SetActive(false);
                                    }

                                    trQuestPanel.gameObject.SetActive(true);

                                    Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();

                                    if (toggleQuestPanel == null)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        toggleQuestPanel.onValueChanged.RemoveAllListeners();
                                        toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                                    }
                                }
                                else
                                {
                                    trQuestPanel.gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            trQuestPanel.gameObject.SetActive(false);
                        }
                        
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    
                    break;

                case EnQuestType.DailyQuest03:
                    if (CsGameData.Instance.DailyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                    {
                        if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                        {
                            csHeroDailyQuest = CsDailyQuestManager.Instance.HeroDailyQuestList[nIndexHeroDailyQuest];

                            if (csHeroDailyQuest == null)
                            {
                                trQuestPanel.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (csHeroDailyQuest.IsAccepted)
                                {
                                    textTitle.text = string.Format(CsConfiguration.Instance.GetString(CsGameData.Instance.DailyQuest.Title), csHeroDailyQuest.DailyQuestMission.Title);

                                    if (csHeroDailyQuest.Completed)
                                    {
                                        textTarget.text = CsConfiguration.Instance.GetString("A101_TXT_00001");
                                        trImageComplete.gameObject.SetActive(true);
                                    }
                                    else
                                    {
                                        switch (csHeroDailyQuest.DailyQuestMission.Type)
                                        {
                                            case 1:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetMonster.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;

                                            case 2:
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(csHeroDailyQuest.DailyQuestMission.TargetContent), csHeroDailyQuest.DailyQuestMission.TargetContinentObject.Name, csHeroDailyQuest.ProgressCount, csHeroDailyQuest.DailyQuestMission.TargetCount);
                                                break;
                                        }

                                        trImageComplete.gameObject.SetActive(false);
                                    }

                                    trQuestPanel.gameObject.SetActive(true);

                                    Toggle toggleQuestPanel = trQuestPanel.GetComponent<Toggle>();

                                    if (toggleQuestPanel == null)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        toggleQuestPanel.onValueChanged.RemoveAllListeners();
                                        toggleQuestPanel.onValueChanged.AddListener((ison) => OnValueChangedDailyQuest(toggleQuestPanel, csHeroDailyQuest));
                                    }
                                }
                                else
                                {
                                    trQuestPanel.gameObject.SetActive(false);
                                }
                            }
                        }
                        else
                        {
                            trQuestPanel.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    
                    break;

                case EnQuestType.WeeklyQuest:
                    if (CsGameData.Instance.WeeklyQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsWeeklyQuestManager.Instance.HeroWeeklyQuest.IsRoundAccepted)
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetTitle), (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundNo - 1), CsGameData.Instance.WeeklyQuest.RoundCount);
                        
                        switch ((EnWeeklyQuestType)CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.Type)
                        {
                            case EnWeeklyQuestType.Move:
                                textTarget.text = CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent;
                                break;

                            case EnWeeklyQuestType.Monster:
                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetMonster.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetCount);
                                break;

                            case EnWeeklyQuestType.Collect:
                                textTarget.text = string.Format(CsConfiguration.Instance.GetString(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContent), CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContinentObject.Name, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.RoundProgressCount, CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetCount);
                                break;
                        }

                        trQuestPanel.gameObject.SetActive(true);
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }

                    break;

                case EnQuestType.GuildAlter:
                    if (m_bIsInGuildIsland)
                    {
                        if (CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint <= CsGuildManager.Instance.GuildMoralPoint || CsGameData.Instance.GuildAltar.DailyGuildMaxMoralPoint <= CsGuildManager.Instance.Guild.MoralPoint)
                        {
                            trQuestPanel.gameObject.SetActive(false);
                        }
                        else
                        {
                            trQuestPanel.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsConfiguration.Instance.GetString("A68_TXT_00001"));
                            textTarget.text = string.Format(CsConfiguration.Instance.GetString("A68_TXT_00002"), CsGuildManager.Instance.GuildMoralPoint, CsGameData.Instance.GuildAltar.DailyHeroMaxMoralPoint);
                        }
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    break;

                case EnQuestType.GuildAlterDefence:

                    if (m_bIsInGuildIsland)
                    {
                        if (CsGuildManager.Instance.IsGuildDefense)
                        {
                            trQuestPanel.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsConfiguration.Instance.GetString("A68_TXT_00003"));
                            textTarget.text = CsConfiguration.Instance.GetString("A68_TXT_00004");
                        }
                        else
                        {
                            ChangeQuestToggleIsOnDisplay(enQuestType, false);
                            trQuestPanel.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        trQuestPanel.GetComponent<Toggle>().isOn = false;
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    break;

                case EnQuestType.GuildFarm:
                    if (m_bIsInGuildIsland)
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsGameData.Instance.GuildFarmQuest.TargetTitle);

                        switch (CsGuildManager.Instance.GuildFarmQuestState)
                        {
                            case EnGuildFarmQuestState.None:
                                ChangeQuestToggleIsOnDisplay(enQuestType, false);
                                trQuestPanel.gameObject.SetActive(false);
                                break;

                            case EnGuildFarmQuestState.Accepted:
                                trQuestPanel.gameObject.SetActive(true);
                                textTarget.text = string.Format(CsGameData.Instance.GuildFarmQuest.TargetContent, CsGameData.Instance.GuildFarmQuest.TargetGuildTerritoryNpc.Name);
                                break;

                            case EnGuildFarmQuestState.Executed:
                                trQuestPanel.gameObject.SetActive(true);
                                textTarget.text = string.Format(CsGameData.Instance.GuildFarmQuest.TargetCompletion, CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc.Name);
                                break;

                            case EnGuildFarmQuestState.Competed:
                                ChangeQuestToggleIsOnDisplay(enQuestType, false);
                                trQuestPanel.gameObject.SetActive(false);
                                break;
                        }
                    }
                    else
                    {
                        trQuestPanel.GetComponent<Toggle>().isOn = false;
                        trQuestPanel.gameObject.SetActive(false);
                    }

                    break;

                case EnQuestType.GuildMission:
                    CsGuildMission csGuildMission = CsGuildManager.Instance.GuildMission;

                    if (csGuildMission == null)
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    else
                    {
                        textTitle.text = string.Format(csGuildMission.TargetTitle, CsGuildManager.Instance.MissionCompletedCount, CsGameData.Instance.GuildMissionQuest.LimitCount);

                        switch (CsGuildManager.Instance.GuildMissionState)
                        {
                            case EnGuildMissionState.None:
                                trQuestPanel.gameObject.SetActive(false);
                                break;
                            case EnGuildMissionState.Accepted:
                                switch (csGuildMission.Type)
                                {
                                    case 1:
                                        textTarget.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetNpc.Name);

                                        trImageTime.gameObject.SetActive(false);
                                        textTime.gameObject.SetActive(false);
                                        break;
                                    case 2:
                                        textTarget.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetMonster.Name, CsGuildManager.Instance.MissionProgressCount, csGuildMission.TargetCount);

                                        trImageTime.gameObject.SetActive(false);
                                        textTime.gameObject.SetActive(false);
                                        break;
                                    case 3:
                                        textTarget.text = csGuildMission.TargetContent;

                                        Debug.Log("====== m_flGuildMissionRemainingTime : ======" + m_flGuildMissionRemainingTime);
                                        m_flGuildMissionRemainingTime = CsGuildManager.Instance.GuildMissionMonster.RemainingLifeTime + Time.realtimeSinceStartup;

                                        trImageTime.gameObject.SetActive(true);
                                        textTime.gameObject.SetActive(true);
                                        break;
                                    case 4:
                                        textTarget.text = string.Format(csGuildMission.TargetContent, csGuildMission.TargetContinent.Name);

                                        trImageTime.gameObject.SetActive(false);
                                        textTime.gameObject.SetActive(false);
                                        break;
                                }
                                trQuestPanel.gameObject.SetActive(true);
                                break;
                            case EnGuildMissionState.Competed:
                                trQuestPanel.gameObject.SetActive(false);
                                break;
                        }
                    }
                    break;

                case EnQuestType.GuildSupplySupport:

                    if (CsGuildManager.Instance.GuildSupplySupportQuestRemainingTime - Time.realtimeSinceStartup > 0)
                    {
                        UpdateGuildSupplySupportTime();
                    }
                    else
                    {
                        m_textTImeGuildSupplySupport.text = "";
                    }

                    switch (CsGuildManager.Instance.GuildSupplySupportState)
                    {
                        case EnGuildSupplySupportState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnGuildSupplySupportState.Accepted:
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsGuildManager.Instance.GuildSupplySupportQuest.Name);
                            //나중에 수정해야됨
                            textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), CsGuildManager.Instance.GuildSupplySupportQuest.CompletionNpc.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            trImageTime.gameObject.SetActive(true);
                            textTime.gameObject.SetActive(true);
                            break;
                    }
                    break;

                case EnQuestType.GuildHunting:

                    switch (CsGuildManager.Instance.GuildHuntingState)
                    {
                        case EnGuildHuntingState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnGuildHuntingState.Accepted:
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsGuildManager.Instance.GuildHuntingQuestObjective.TargetTitle);
                            CsGuildHuntingQuestObjective csGuildHuntingQuestObjective = CsGuildManager.Instance.GuildHuntingQuestObjective;

                            if (csGuildHuntingQuestObjective.Type == 1)
                            {
                                textTarget.text = string.Format(csGuildHuntingQuestObjective.TargetContent, csGuildHuntingQuestObjective.TargetMonster.Name, CsGuildManager.Instance.HeroGuildHuntingQuest.ProgressCount, CsGuildManager.Instance.GuildHuntingQuestObjective.TargetCount);
                            }
                            else if (CsGuildManager.Instance.GuildHuntingQuestObjective.Type == 2)
                            {
                                textTarget.text = string.Format(CsGuildManager.Instance.GuildHuntingQuestObjective.TargetContent, csGuildHuntingQuestObjective.TargetContinentObject.Name, CsGuildManager.Instance.HeroGuildHuntingQuest.ProgressCount, CsGuildManager.Instance.GuildHuntingQuestObjective.TargetCount);
                            }

                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnGuildHuntingState.Executed://A12_TXT_01002
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01007"), CsGuildManager.Instance.GuildHuntingQuestObjective.TargetTitle);
                            textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), CsGameData.Instance.GetNpcInfo(CsGameData.Instance.GuildHuntingQuest.QuestNpcId).Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnGuildHuntingState.Competed:
                            trQuestPanel.gameObject.SetActive(false);
                            break;
                    }

                    break;

                case EnQuestType.SecretLetter:
                    switch (CsSecretLetterQuestManager.Instance.SecretLetterState)
                    {
                        case EnSecretLetterState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnSecretLetterState.Accepted:
                            trImageComplete.gameObject.SetActive(false);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.SecretLetterQuest.TargetTitle);
                            textTarget.text = string.Format(CsGameData.Instance.SecretLetterQuest.TargetContent, CsGameData.Instance.GetNation(CsSecretLetterQuestManager.Instance.TargetNationId).Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnSecretLetterState.Executed:
                            trImageComplete.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.SecretLetterQuest.TargetTitle);
                            textTarget.text = string.Format(CsSecretLetterQuestManager.Instance.SecretLetterQuest.CompletionText, CsGameData.Instance.SecretLetterQuest.QuestNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnSecretLetterState.Completed:
                            trImageComplete.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.SecretLetterQuest.TargetTitle);
                            textTarget.text = string.Format(CsSecretLetterQuestManager.Instance.SecretLetterQuest.CompletionText, CsGameData.Instance.SecretLetterQuest.QuestNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;
                    }
                    break;

                case EnQuestType.MysteryBox:
                    switch (CsMysteryBoxQuestManager.Instance.MysteryBoxState)
                    {
                        case EnMysteryBoxState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnMysteryBoxState.Accepted:
                            trImageComplete.gameObject.SetActive(false);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.MysteryBoxQuest.TargetTitle);
                            textTarget.text = CsGameData.Instance.MysteryBoxQuest.TargetContent;
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnMysteryBoxState.Executed:
                            trImageComplete.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.MysteryBoxQuest.TargetTitle);
                            textTarget.text = string.Format(CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.CompletionText, CsGameData.Instance.MysteryBoxQuest.QuestNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnMysteryBoxState.Completed:
                            trImageComplete.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.MysteryBoxQuest.TargetTitle);
                            textTarget.text = string.Format(CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.CompletionText, CsGameData.Instance.MysteryBoxQuest.QuestNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;
                    }
                    break;

                case EnQuestType.DimensionRaid:
                    switch (CsDimensionRaidQuestManager.Instance.DimensionRaidState)
                    {
                        case EnDimensionRaidState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnDimensionRaidState.Accepted:
                            trImageComplete.gameObject.SetActive(false);
                            CsDimensionRaidQuestStep csDimensionRaidQuestStep = CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(CsDimensionRaidQuestManager.Instance.Step);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), csDimensionRaidQuestStep.TargetTitle);
                            textTarget.text = string.Format(csDimensionRaidQuestStep.TargetContent, csDimensionRaidQuestStep.TargetNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnDimensionRaidState.Completed:
                            trImageComplete.gameObject.SetActive(true);
                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsGameData.Instance.DimensionRaidQuest.GetDimensionRaidQuestStep(1).TargetTitle);
                            textTarget.text = string.Format(CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.CompletionText, CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo.Name);
                            trQuestPanel.gameObject.SetActive(true);
                            break;
                    }
                    break;

                case EnQuestType.HolyWar:
                    CsHolyWarQuest csHolyWarQuest = CsHolyWarQuestManager.Instance.HolyWarQuest;
                    EnHolyWarQuestState enHolyWarQuestState = CsHolyWarQuestManager.Instance.HolyWarQuestState;

                    if (enHolyWarQuestState == EnHolyWarQuestState.None)
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    else if (enHolyWarQuestState == EnHolyWarQuestState.Accepted)
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), csHolyWarQuest.TargetTitle);

                        UpdateToggleHolyWarQuestKillCount();

                        trImageTime.gameObject.SetActive(true);

                        UpdateToggleHolyWarQuestTimeText();
                        textTime.gameObject.SetActive(true);

                        trImageComplete.gameObject.SetActive(false);
                        trQuestPanel.gameObject.SetActive(true);
                    }
                    else if (enHolyWarQuestState == EnHolyWarQuestState.Completed)
                    {
                        textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), csHolyWarQuest.TargetTitle);
                        textTarget.text = string.Format(csHolyWarQuest.CompletionText, csHolyWarQuest.QuestNpcInfo.Name);

                        trImageTime.gameObject.SetActive(false);
                        textTime.gameObject.SetActive(false);

                        trImageComplete.gameObject.SetActive(true);
                        trQuestPanel.gameObject.SetActive(true);
                    }
                    break;

                case EnQuestType.SupplySupport:
                    textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01008"), CsSupplySupportQuestManager.Instance.SupplySupportQuest.TargetTitle);

                    if (CsSupplySupportQuestManager.Instance.RemainingTime - Time.realtimeSinceStartup > 0)
                    {
                        UpdateSupplySupportTime();
                    }
                    else
                    {
                        m_textTImeSupplySupport.text = "";
                    }

                    switch (CsSupplySupportQuestManager.Instance.QuestState)
                    {
                        case EnSupplySupportState.None:
                            trQuestPanel.gameObject.SetActive(false);
                            break;

                        case EnSupplySupportState.Accepted:
                            textTarget.text = CsGameData.Instance.SupplySupportQuest.TargetContent;
                            trImageComplete.gameObject.SetActive(false);
                            textTime.gameObject.SetActive(true);
                            trImageTime.gameObject.SetActive(true);
                            trQuestPanel.gameObject.SetActive(true);
                            break;

                        case EnSupplySupportState.Executed:
                            textTarget.text = string.Format(CsGameData.Instance.SupplySupportQuest.CompletionText, CsGameData.Instance.SupplySupportQuest.CompletionNpc.Name);
                            trImageComplete.gameObject.SetActive(false);
                            textTime.gameObject.SetActive(true);
                            trImageTime.gameObject.SetActive(true);
                            trQuestPanel.gameObject.SetActive(true);
                            break;
                    }
                    break;

                case EnQuestType.Dungeon:
                    if (m_bIsInDungeon)
                    {
                        trQuestPanel.gameObject.SetActive(true);
                    }
                    else
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    break;

				case EnQuestType.TrueHero:

					// 모래시계 끄기
					trImageTime.gameObject.SetActive(false);
					textTime.gameObject.SetActive(false);

					switch (CsTrueHeroQuestManager.Instance.TrueHeroQuestState)
                    {
						case EnTrueHeroQuestState.None:
                            trQuestPanel.gameObject.SetActive(false);
							break;

						case EnTrueHeroQuestState.Accepted:
                            trImageComplete.gameObject.SetActive(false);

							// 버티기
							if (CsTrueHeroQuestManager.Instance.Interacted)
							{
								textTitle.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.StepNo);
								textTarget.text = CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetContent;

								// 모래시계 켜기
								trImageTime.gameObject.SetActive(true);
								textTime.gameObject.SetActive(true);

								TimeSpan timeSpan = TimeSpan.FromSeconds(CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.ObjectiveWaitingTime);
								textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
							}
							// 상호작용
							else
							{
								textTitle.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.StepNo);
								textTarget.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.TargetContent, CsTrueHeroQuestManager.Instance.TrueHeroQuestStep.TargetContinent.Name);
							}

                            trQuestPanel.gameObject.SetActive(true);
                            break;

						case EnTrueHeroQuestState.Executed:
                            trImageComplete.gameObject.SetActive(true);
							textTitle.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.TargetTitle, CsTrueHeroQuestManager.Instance.LastStepNo);
                            textTarget.text = string.Format(CsTrueHeroQuestManager.Instance.TrueHeroQuest.CompletionText, CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc.Name);
							trQuestPanel.gameObject.SetActive(true);
                            break;

						case EnTrueHeroQuestState.Completed:
                            trQuestPanel.gameObject.SetActive(false);
                            break;
                    }
                    break;

				case EnQuestType.SubQuest:
					trImageTime.gameObject.SetActive(false);
					textTime.gameObject.SetActive(false);

					CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.GetHeroSubQuest(nParam);
					
					switch (csHeroSubQuest.EnStatus)
					{
						case EnSubQuestStatus.Abandon:
							trQuestPanel.gameObject.SetActive(false);
							break;
						case EnSubQuestStatus.Acception:
							trQuestPanel.gameObject.SetActive(true);
							trImageComplete.gameObject.SetActive(false);
							textTitle.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csHeroSubQuest.SubQuest.Title);

							switch (csHeroSubQuest.SubQuest.Type)
							{
								case 1:
									textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.SubQuest.TargetContinentObject.Name, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
									break;
								case 2:
									textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.SubQuest.TargetMonster.Name, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
									break;
								case 3:
									textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
									break;
								default:
									break;
							}
                            
							break;
						case EnSubQuestStatus.Completion:
							trQuestPanel.gameObject.SetActive(false);
							break;
						case EnSubQuestStatus.Excuted:

							trQuestPanel.gameObject.SetActive(true);
							trImageComplete.gameObject.SetActive(true);
							textTitle.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_TITLE_SUB"), csHeroSubQuest.SubQuest.Title);

							if (csHeroSubQuest.SubQuest.RequiredConditionType == 2 &&
								csHeroSubQuest.SubQuest.CompletionNpc == null)
							{
								switch (csHeroSubQuest.SubQuest.Type)
								{
									case 1:
										textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.SubQuest.TargetContinentObject.Name, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
										break;
									case 2:
										textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.SubQuest.TargetMonster.Name, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
										break;
									case 3:
										textTarget.text = string.Format(csHeroSubQuest.SubQuest.TargetText, csHeroSubQuest.ProgressCount, csHeroSubQuest.SubQuest.TargetCount);
										break;
									default:
										break;
								}
							}
							else
							{
								textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csHeroSubQuest.SubQuest.CompletionNpc.Name);
							}
							break;
					}

					break;

				case EnQuestType.Biography:
					trImageTime.gameObject.SetActive(false);
					textTime.gameObject.SetActive(false);
					
					CsHeroBiography csHeroBiography = CsBiographyManager.Instance.GetHeroBiography(nParam);

					if (csHeroBiography == null)
					{
						trQuestPanel.gameObject.SetActive(false);
					}
					else
					{
						CsHeroBiographyQuest csHeroBiographyQuest = csHeroBiography.HeroBiograhyQuest;

						if (csHeroBiographyQuest == null)
						{
							trQuestPanel.gameObject.SetActive(false);
						}
						else
						{
							if (csHeroBiographyQuest.Completed)
							{
								CsBiographyQuest nextQuest = csHeroBiography.Biography.GetBiographyQuest(csHeroBiographyQuest.QuestNo + 1);

								if (nextQuest == null)
								{
									trQuestPanel.gameObject.SetActive(false);
								}
								else
								{
									trQuestPanel.gameObject.SetActive(true);
									trImageComplete.gameObject.SetActive(false);

									textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01003"), string.Format(nextQuest.TargetTitle, csHeroBiography.Biography.Name, nextQuest.QuestNo));

									if (nextQuest.StartNpc != null)
									{
										textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), nextQuest.StartNpc.Name);
									}
								}
							}
							else
							{
								trQuestPanel.gameObject.SetActive(true);
								trImageComplete.gameObject.SetActive(csHeroBiographyQuest.Excuted);

								textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01003"), string.Format(csHeroBiographyQuest.BiographyQuest.TargetTitle, csHeroBiography.Biography.Name, csHeroBiographyQuest.QuestNo));

								if (csHeroBiographyQuest.Excuted)
								{
									if (csHeroBiographyQuest.BiographyQuest.CompletionNpc != null)
									{
										textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csHeroBiographyQuest.BiographyQuest.CompletionNpc.Name);
									}
								}
								else
								{
									switch (csHeroBiographyQuest.BiographyQuest.Type)
									{
										case 1:
											textTarget.text = csHeroBiographyQuest.BiographyQuest.TargetContent;
											break;
										case 2:
											textTarget.text = string.Format(csHeroBiographyQuest.BiographyQuest.TargetContent, csHeroBiographyQuest.BiographyQuest.TargetMonster.Name, csHeroBiographyQuest.ProgressCount, csHeroBiographyQuest.BiographyQuest.TargetCount);
											break;
										case 3:
											textTarget.text = string.Format(csHeroBiographyQuest.BiographyQuest.TargetContent, csHeroBiographyQuest.BiographyQuest.TargetContinentObject.Name, csHeroBiographyQuest.ProgressCount, csHeroBiographyQuest.BiographyQuest.TargetCount);
											break;
										case 4:
											textTarget.text = string.Format(csHeroBiographyQuest.BiographyQuest.TargetContent, csHeroBiographyQuest.BiographyQuest.TargetNpc.Name, csHeroBiographyQuest.ProgressCount, csHeroBiographyQuest.BiographyQuest.TargetCount);
											break;
										case 5:
											textTarget.text = string.Format(csHeroBiographyQuest.BiographyQuest.TargetContent, CsGameData.Instance.GetBiographyQuestDungeon(csHeroBiographyQuest.BiographyQuest.TargetDungeonId).Name);
											break;
									}
								}
							}
						}
					}

					break;

				case EnQuestType.CreatureFarm:
					
					int nMissionCount = CsGameData.Instance.CreatureFarmQuest.CreatureFarmQuestMissionList.Count;

					switch (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState)
					{
						case EnCreatureFarmQuestState.None:
						case EnCreatureFarmQuestState.Completed:

							trImageTime.gameObject.SetActive(false);
							textTime.gameObject.SetActive(false);

							trQuestPanel.gameObject.SetActive(false);
							trImageComplete.gameObject.SetActive(false);

							break;

						case EnCreatureFarmQuestState.Accepted:

							trQuestPanel.gameObject.SetActive(true);
							trImageComplete.gameObject.SetActive(false);

							CsHeroCreatureFarmQuest csHeroCreatureFarmQuest = CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest;

							if (csHeroCreatureFarmQuest != null)
							{
								textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), string.Format(CsConfiguration.Instance.GetString("A149_TXT_00001"), CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.MissionNo, nMissionCount));
								
								switch (csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType)
								{
									case 1:
										// 이동
										trImageTime.gameObject.SetActive(false);
										textTime.gameObject.SetActive(false);

										textTarget.text = csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent;

										break;

									case 2:
										// 상호작용
										trImageTime.gameObject.SetActive(false);
										textTime.gameObject.SetActive(false);

										textTarget.text = string.Format(csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent, csHeroCreatureFarmQuest.CreatureFarmQuestMission.ContinentObjectTarget.Name);

										break;

									case 3:
										// 몬스터 처치
										trImageTime.gameObject.SetActive(csHeroCreatureFarmQuest.RemainingMonsterLifetime > 0);
										textTime.gameObject.SetActive(csHeroCreatureFarmQuest.RemainingMonsterLifetime > 0);
										
										textTarget.text = csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetContent;

										if (csHeroCreatureFarmQuest.RemainingMonsterLifetime > 0)
										{
											TimeSpan timeSpan = TimeSpan.FromSeconds(csHeroCreatureFarmQuest.RemainingMonsterLifetime);
											textTime.text = string.Format(CsConfiguration.Instance.GetString("A13_TXT_00006"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
										}

										break;
								}
							}

							break;

						case EnCreatureFarmQuestState.Executed:

							trImageTime.gameObject.SetActive(false);
							textTime.gameObject.SetActive(false);

							trQuestPanel.gameObject.SetActive(true);
							trImageComplete.gameObject.SetActive(true);

							//CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetTitle

							textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01004"), string.Format(CsConfiguration.Instance.GetString("A149_TXT_00001"), nMissionCount, nMissionCount));

							textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), CsCreatureFarmQuestManager.Instance.CreatureFarmQuest.CompletionNpc.Name);

							break;
					}

					break;

				case EnQuestType.JobChange:
					trImageTime.gameObject.SetActive(false);
					textTime.gameObject.SetActive(false);

					if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
                    {
                        trQuestPanel.gameObject.SetActive(false);
                    }
                    else
                    {
                        CsHeroJobChangeQuest csHeroJobChangeQuest = CsJobChangeManager.Instance.HeroJobChangeQuest;
                        
                        if (csHeroJobChangeQuest == null)
                        {
                            textTitle.text = CsConfiguration.Instance.GetString("A153_TXT_00003");

                            // 수락 받기 전
                            if (m_bStartJobChangeQuest)
                            {
                                // 퀘스트 패널을 한번이라도 누름
                                CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.JobChangeQuestList.First();

                                if (csJobChangeQuest == null)
                                {
                                    textTarget.text = "";
                                }
                                else
                                {
                                    textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);
                                }
                            }
                            else
                            {
                                // 이곳을 눌러 퀘스트를 받으세요
                                textTarget.text = CsConfiguration.Instance.GetString("A12_TXT_00001");
                            }

                            trQuestPanel.gameObject.SetActive(true);
                        }
                        else
                        {
                            CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(csHeroJobChangeQuest.QuestNo);

                            if (csJobChangeQuest == null)
                            {
                                trQuestPanel.gameObject.SetActive(false);
                            }
                            else
                            {
                                if (csHeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo)
                                {
                                    // 모두 완료
                                    switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                                    {
                                        case EnJobChangeQuestStaus.Accepted:

                                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01001"), csJobChangeQuest.Title);
                                            textTarget.text = csJobChangeQuest.TargetContent;

                                            if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                            {
                                                trImageComplete.gameObject.SetActive(false);

                                                trImageTime.gameObject.SetActive(true);

                                                TimeSpan tsJobChangeQuestRemainingTime = TimeSpan.FromSeconds(csHeroJobChangeQuest.RemainingTime - Time.realtimeSinceStartup);
                                                textTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), tsJobChangeQuestRemainingTime.Minutes.ToString("00"), tsJobChangeQuestRemainingTime.Seconds.ToString("00"));
                                                textTime.gameObject.SetActive(true);
                                            }
                                            else
                                            {
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);

                                                trImageTime.gameObject.SetActive(false);
                                                textTime.gameObject.SetActive(false);

                                                trImageComplete.gameObject.SetActive(true);
                                            }

                                            trQuestPanel.gameObject.SetActive(true);

                                            break;

                                        case EnJobChangeQuestStaus.Completed:
                                            trQuestPanel.gameObject.SetActive(false);
                                            break;

                                        case EnJobChangeQuestStaus.Failed:

                                            trImageTime.gameObject.SetActive(false);
                                            textTime.gameObject.SetActive(false);

                                            textTitle.text = CsConfiguration.Instance.GetString("A38_BTN_00002");
                                            textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);
                                            trQuestPanel.gameObject.SetActive(true);

                                            break;
                                    }
                                }
                                else
                                {
                                    trImageTime.gameObject.SetActive(false);
                                    textTime.gameObject.SetActive(false);
                                    
                                    switch ((EnJobChangeQuestStaus)csHeroJobChangeQuest.Status)
                                    {
                                        case EnJobChangeQuestStaus.Accepted:

                                            textTitle.text = string.Format(CsConfiguration.Instance.GetString("A38_TXT_01001"), csJobChangeQuest.Title);
                                            
                                            switch (csJobChangeQuest.Type)
                                            {
                                                case 1:
                                                    textTarget.text = string.Format((csJobChangeQuest.TargetContent), csJobChangeQuest.TargetMonster.Name, csHeroJobChangeQuest.ProgressCount, csJobChangeQuest.TargetCount);
                                                    break;

                                                case 2:
                                                    textTarget.text = string.Format((csJobChangeQuest.TargetContent), csJobChangeQuest.TargetContinentObject.Name, csHeroJobChangeQuest.ProgressCount, csJobChangeQuest.TargetCount);
                                                    break;

                                                default:
                                                    break;
	                                        }

                                            if (csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
                                            {
                                                trImageComplete.gameObject.SetActive(false);
                                            }
                                            else
                                            {
                                                textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);

                                                trImageComplete.gameObject.SetActive(true);
                                            }

                                            trQuestPanel.gameObject.SetActive(true);

                                            break;

                                        case EnJobChangeQuestStaus.Completed:

                                            // 다음 퀘스트 이동
                                            trImageComplete.gameObject.SetActive(false);

                                            textTitle.text = CsConfiguration.Instance.GetString("A38_BTN_00002");
                                            textTarget.text = string.Format(CsConfiguration.Instance.GetString("A12_TXT_01002"), csJobChangeQuest.QuestNpc.Name);
                                            trQuestPanel.gameObject.SetActive(true);

                                            break;

                                        case EnJobChangeQuestStaus.Failed:
                                            Debug.Log("failed 서버 버그");
                                            break;
                                    }
                                }
                            }
                        }
                    }

					break;
            }

            UpdateQuestPanelDisplay();
        }
    }

    IEnumerator m_IEnumeratorThreatOfFameMonsterInfo;

    //---------------------------------------------------------------------------------------------------
    IEnumerator UpdateThreatOfFameMonsterInfo(Text textTarget)
    {
        IMonsterObjectInfo iMonsterObjectInfo = null;
        
        while (iMonsterObjectInfo == null)
        {
            iMonsterObjectInfo = CsGameData.Instance.ListMonsterObjectInfo.Find(a => a.GetInstanceId() == CsThreatOfFarmQuestManager.Instance.Monster.InstanceId);
            yield return null;
        }

        yield return new WaitUntil(() => iMonsterObjectInfo != null);

        CsMonsterInfo csMonsterInfo = iMonsterObjectInfo.GetMonsterInfo();
        textTarget.text = string.Format(CsThreatOfFarmQuestManager.Instance.Quest.TargetKillText, csMonsterInfo.Name);

        if (m_IEnumeratorThreatOfFameMonsterInfo != null)
        {
            StopCoroutine(m_IEnumeratorThreatOfFameMonsterInfo);
            m_IEnumeratorThreatOfFameMonsterInfo = null;
        }
        else
        {
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateQuestPanelDisplay()
    {
        List<EnQuestType> listCompleteQuestPanel = new List<EnQuestType>();
        List<EnQuestType> listQuestPanel = new List<EnQuestType>();

        // 메인 퀘스트
        listCompleteQuestPanel.Add(EnQuestType.MainQuest);

        // 농장의 위협
        if (CsThreatOfFarmQuestManager.Instance.QuestState == EnQuestState.Complete)
        {
            listCompleteQuestPanel.Add(EnQuestType.ThreatOfFarm);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.ThreatOfFarm);
        }

        // 현상금 사냥
        listQuestPanel.Add(EnQuestType.BountyHunter);

        // 일일 퀘스트
        int nQuestType = (int)EnQuestType.DailyQuest01;
        for (int i = 0; i < CsDailyQuestManager.Instance.HeroDailyQuestList.Count; i++)
        {
            if (CsDailyQuestManager.Instance.HeroDailyQuestList[i].IsAccepted)
            {
                if (CsDailyQuestManager.Instance.HeroDailyQuestList[i].Completed)
                {
                    listCompleteQuestPanel.Add((EnQuestType)nQuestType);
                }
                else
                {
                    listQuestPanel.Add((EnQuestType)nQuestType);
                }
            }
            else
            {
                
            }

            nQuestType++;
        }

        // 주간 퀘스트
        listQuestPanel.Add(EnQuestType.WeeklyQuest);

        // 밀서
        if (CsSecretLetterQuestManager.Instance.SecretLetterState == EnSecretLetterState.Completed)
        {
            listCompleteQuestPanel.Add(EnQuestType.SecretLetter);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.SecretLetter);
        }

        // 의문의 상자
        if (CsMysteryBoxQuestManager.Instance.MysteryBoxState == EnMysteryBoxState.Completed)
        {
            listCompleteQuestPanel.Add(EnQuestType.MysteryBox);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.MysteryBox);
        }

        // 차원 잠입
        if (CsDimensionRaidQuestManager.Instance.DimensionRaidState == EnDimensionRaidState.Completed)
        {
            listCompleteQuestPanel.Add(EnQuestType.DimensionRaid);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.DimensionRaid);
        }

        // 위대한 성전
        if (CsHolyWarQuestManager.Instance.HolyWarQuestState == EnHolyWarQuestState.Completed)
        {
            listCompleteQuestPanel.Add(EnQuestType.HolyWar);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.HolyWar);
        }

        // 보급 지원
        listQuestPanel.Add(EnQuestType.SupplySupport);

        //길드 미션
        listQuestPanel.Add(EnQuestType.GuildMission);

        //길드 카트
        listQuestPanel.Add(EnQuestType.GuildSupplySupport);

        if (CsGuildManager.Instance.GuildHuntingState == EnGuildHuntingState.Competed)
        {
            listCompleteQuestPanel.Add(EnQuestType.GuildHunting);
        }
        else
        {
            listQuestPanel.Add(EnQuestType.GuildHunting);
        }

		// 진정한 영웅
		if (CsTrueHeroQuestManager.Instance.TrueHeroQuestState == EnTrueHeroQuestState.Completed)
		{
			listCompleteQuestPanel.Add(EnQuestType.TrueHero);
		}
		else
		{
			listQuestPanel.Add(EnQuestType.TrueHero);
		}

		// 서브퀘스트
		for (int i = 0; i < CsSubQuestManager.Instance.HeroSubQuestList.Count; i++)
		{
			CsHeroSubQuest csHeroSubQuest = CsSubQuestManager.Instance.HeroSubQuestList[i];

			if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon)
				continue;

			if (!listCompleteQuestPanel.Contains(EnQuestType.SubQuest) &&
				csHeroSubQuest.EnStatus == EnSubQuestStatus.Completion)
			{
				listCompleteQuestPanel.Add(EnQuestType.SubQuest);
			}

			if (!listQuestPanel.Contains(EnQuestType.SubQuest) &&
				csHeroSubQuest.EnStatus != EnSubQuestStatus.Completion)
			{
				listQuestPanel.Add(EnQuestType.SubQuest);
			}
		}

		if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Completed)
		{
			listCompleteQuestPanel.Add(EnQuestType.CreatureFarm);
		}
		else
		{
			listQuestPanel.Add(EnQuestType.CreatureFarm);
		}

        listCompleteQuestPanel.Sort(CompareTo);
        listQuestPanel.Sort(CompareTo);

        listCompleteQuestPanel.Insert(0, EnQuestType.JobChange);

		for (int i = 0; i < listCompleteQuestPanel.Count; i++)
        {
			Transform trQuestPanel = null;
			
			if (listCompleteQuestPanel[i] == EnQuestType.SubQuest)
			{
				foreach (var heroSubQuest in CsSubQuestManager.Instance.HeroSubQuestList)
				{
					if (heroSubQuest.EnStatus == EnSubQuestStatus.Completion)
					{
						trQuestPanel = GetQuestPanel(listCompleteQuestPanel[i], heroSubQuest.SubQuest.QuestId);
					}
				}
			}
			else
			{
				trQuestPanel = GetQuestPanel(listCompleteQuestPanel[i]);
			}

            if (trQuestPanel == null)
            {
                continue;
            }
            else
            {
                trQuestPanel.SetSiblingIndex(i);
            }
        }

        int nStartCount = listCompleteQuestPanel.Count;

        for (int i = nStartCount; i < listQuestPanel.Count + nStartCount; i++)
        {
            Transform trQuestPanel = null;
			
			if (listQuestPanel[i - nStartCount] == EnQuestType.SubQuest)
			{
				foreach (var heroSubQuest in CsSubQuestManager.Instance.HeroSubQuestList)
				{
					if (heroSubQuest.EnStatus == EnSubQuestStatus.Abandon ||
						heroSubQuest.EnStatus == EnSubQuestStatus.Completion)
						continue;

					trQuestPanel = GetQuestPanel(listQuestPanel[i - nStartCount], heroSubQuest.SubQuest.QuestId);
				}
			}
			else
			{
				trQuestPanel = GetQuestPanel(listQuestPanel[i - nStartCount]);
			}

            if (trQuestPanel == null)
            {
                continue;
            }
            else
            {
                trQuestPanel.SetSiblingIndex(i);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(EnQuestType value1, EnQuestType value2)
    {
        if ((int)value1 > (int)value2) return 1;
        else if ((int)value1 < (int)value2) return -1;
        return 0;
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
	bool UpdateQuestPanelDisplay(EnQuestType enQuestType, int nParam = 0)
	{
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}

		if (PlayerPrefs.HasKey(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName))
        {
			return PlayerPrefs.GetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName) == 1;
        }
        else
        {
            return true;
        }
    }

    //---------------------------------------------------------------------------------------------------
	// 퀘스트 타입 하나에 여러 퀘스트가 동시에 진행될 수 있는 경우 nParam 사용
    void ResetQuestPanelDisplay(EnQuestType enQuestType, int nParam = 0)
    {
		string strName = null;

		switch (enQuestType)
		{
			case EnQuestType.SubQuest:
			case EnQuestType.Biography:
				strName = enQuestType.ToString() + nParam.ToString();
				break;
			default:
				strName = enQuestType.ToString();
				break;
		}
		
		if (PlayerPrefs.HasKey(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName))
        {
			PlayerPrefs.SetInt(CsGameData.Instance.MyHeroInfo.HeroId.ToString() + strName, 1);
			PlayerPrefs.Save();
        }
        else
        {
            return;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateTrueHeroQuestWaitingTime()
	{
		if (CsTrueHeroQuestManager.Instance.RemainingWaitingTime < 0)
		{
			return;
		}

		if (m_textTimeTrueHeroQuest != null)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(CsTrueHeroQuestManager.Instance.RemainingWaitingTime);
			m_textTimeTrueHeroQuest.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureFarmQuestWaitingTime()
	{
		if (CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.RemainingMonsterLifetime < 0)
		{
			return;
		}

		if (m_textTimeCreatureFarmQuest != null)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.RemainingMonsterLifetime);
			m_textTimeCreatureFarmQuest.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenDialogMainQuestNpc()
	{
		if (CsMainQuestManager.Instance.MainQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
		{
			UpdatePopupMainQuest();
		}
		else
		{
			CsNpcInfo csNpcInfo = GetCurrentMainQuestNpcInfo();
			Debug.Log("csNpcInfo : " + csNpcInfo);
			if (csNpcInfo == null)
			{
				return;
			}
			else
			{
				EnQuestState enThreatOfFarmState = CsThreatOfFarmQuestManager.Instance.QuestState;                                  // 서브 퀘스트: 농장의 위협
				EnMysteryBoxState enMysteryBoxState = CsMysteryBoxQuestManager.Instance.MysteryBoxState;                            // 서브 퀘스트: 의문의 상자
				EnSecretLetterState enSecretLetterState = CsSecretLetterQuestManager.Instance.SecretLetterState;                    // 서브 퀘스트: 밀서 유출
				EnDimensionRaidState enDimensionRaidState = CsDimensionRaidQuestManager.Instance.DimensionRaidState;                // 서브 퀘스트: 차원 습격
				EnHolyWarQuestState enHolywarQuestState = CsHolyWarQuestManager.Instance.HolyWarQuestState;                         // 서브 퀘스트: 위대한 성전
				EnSupplySupportState enSupplySupportState = CsSupplySupportQuestManager.Instance.QuestState;                        // 서브 퀘스트: 보급 지원
				EnGuildMissionState enGuildMissionState = CsGuildManager.Instance.GuildMissionState;
				EnGuildSupplySupportState enGuildSupplySupportState = CsGuildManager.Instance.GuildSupplySupportState;              // 서브 퀘스트: 길드 물자 지원
				EnGuildHuntingState enGuildHuntingState = CsGuildManager.Instance.GuildHuntingState;                                // 서브 퀘스트: 길드 헌팅
				EnTrueHeroQuestState enTrueHeroQuestState = CsTrueHeroQuestManager.Instance.TrueHeroQuestState;						// 진정한 영웅

				// 농장의 위협
				if (csNpcInfo == CsThreatOfFarmQuestManager.Instance.Quest.QuestNpc && (enThreatOfFarmState == EnQuestState.None || (enThreatOfFarmState == EnQuestState.Accepted && CsThreatOfFarmQuestManager.Instance.Mission == null)))
				{
					return;
				}
				// 의문의 상자
				else if (csNpcInfo == CsMysteryBoxQuestManager.Instance.MysteryBoxQuest.QuestNpcInfo && enMysteryBoxState != EnMysteryBoxState.Accepted)
				{
					return;
				}
				// 밀서
				else if (csNpcInfo == CsSecretLetterQuestManager.Instance.SecretLetterQuest.QuestNpcInfo && enSecretLetterState != EnSecretLetterState.Accepted)
				{
					return;
				}
				// 차원의 습격
				else if (csNpcInfo == CsDimensionRaidQuestManager.Instance.DimensionRaidQuest.QuestNpcInfo && enDimensionRaidState != EnDimensionRaidState.Accepted)
				{
					return;
				}
				// 위대한 성전
				else if (csNpcInfo == CsHolyWarQuestManager.Instance.HolyWarQuest.QuestNpcInfo && enHolywarQuestState != EnHolyWarQuestState.Accepted && CsHolyWarQuestManager.Instance.CheckAvailability())
				{
					return;
				}
				// 세리우 보급 지원
				else if ((csNpcInfo == CsSupplySupportQuestManager.Instance.SupplySupportQuest.StartNpc && enSupplySupportState == EnSupplySupportState.None) || (csNpcInfo == CsSupplySupportQuestManager.Instance.SupplySupportQuest.CompletionNpc && enSupplySupportState == EnSupplySupportState.Executed))
				{
					return;
				}
				// 길드 미션
				else if (csNpcInfo == CsGuildManager.Instance.GuildMissionQuest.StartNpc && enGuildMissionState == EnGuildMissionState.None)
				{
					return;
				}
				// 길드 보급 지원
				else if ((csNpcInfo == CsGuildManager.Instance.GuildSupplySupportQuest.StartNpc && enGuildSupplySupportState == EnGuildSupplySupportState.None) || (csNpcInfo == CsGuildManager.Instance.GuildSupplySupportQuest.CompletionNpc && enGuildSupplySupportState == EnGuildSupplySupportState.Accepted))
				{
					return;
				}
				// 길드 헌팅
				else if (csNpcInfo.NpcId == CsGuildManager.Instance.GuildHuntingQuest.QuestNpcId && enGuildHuntingState != EnGuildHuntingState.Accepted)
				{
					return;
				}
				// 진정한 영웅
				else if (csNpcInfo.NpcId == CsTrueHeroQuestManager.Instance.TrueHeroQuest.QuestNpc.NpcId &&
					(enTrueHeroQuestState != EnTrueHeroQuestState.None || enTrueHeroQuestState != EnTrueHeroQuestState.Executed))
				{
					return;
				}
                else if ((CsJobChangeManager.Instance.HeroJobChangeQuest == null && CsGameConfig.Instance.JobChangeRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level) || 
                    (CsJobChangeManager.Instance.HeroJobChangeQuest != null && !(CsJobChangeManager.Instance.HeroJobChangeQuest.QuestNo == CsGameData.Instance.JobChangeQuestList.Last().QuestNo && CsJobChangeManager.Instance.HeroJobChangeQuest.Status == (int)EnJobChangeQuestStaus.Completed)) &&
                    csNpcInfo.NpcId == CsGameData.Instance.JobChangeQuestList.First().QuestNpc.NpcId)
                {
                    return;
                }
				else
				{
					UpdatePopupNpcDialog(csNpcInfo.Name, csNpcInfo.Dialogue);
				}
			}
		}
	}

    #endregion PanelQuest

    #region 풍광 퀘스트
    Transform m_trPopupSceneryQuest;
    GameObject m_goPopupSceneryQuest;
    IEnumerator m_iEnumeratorSceneryQuestTime;

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSceneryQuest(int nQuestId)
    {
        if (m_goPopupSceneryQuest == null)
        {
            m_goPopupSceneryQuest = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/PopupSceneryQuest");
        }

        Debug.Log(m_goPopupSceneryQuest + " /// " + m_trPopup);

        GameObject goPopup = Instantiate(m_goPopupSceneryQuest, m_trPopup);
        m_trPopupSceneryQuest = goPopup.transform;

        CsSceneryQuest csSceneryQuest = CsGameData.Instance.GetSceneryQuest(nQuestId);

        Text textName = m_trPopupSceneryQuest.Find("TextBookName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csSceneryQuest.Name;

        Text textTIme = m_trPopupSceneryQuest.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTIme);
        TimeSpan timeSpan = TimeSpan.FromSeconds(csSceneryQuest.WaitingTime);
        textTIme.text = string.Format("{0}:{1}", timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));

        if (m_iEnumeratorSceneryQuestTime != null)
        {
            StopCoroutine(m_iEnumeratorSceneryQuestTime);
            m_iEnumeratorSceneryQuestTime = null;
        }

        m_iEnumeratorSceneryQuestTime = SceneryQuestTimeCoroutine(textTIme);
        StartCoroutine(m_iEnumeratorSceneryQuestTime);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator SceneryQuestTimeCoroutine(Text textTIme)
    {
        Debug.Log(CsIllustratedBookManager.Instance.SceneryQuestRemainingTime);
        while (CsIllustratedBookManager.Instance.SceneryQuestRemainingTime - Time.realtimeSinceStartup > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(CsIllustratedBookManager.Instance.SceneryQuestRemainingTime - Time.realtimeSinceStartup);
            textTIme.text = string.Format("{0}:{1}", timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));

            yield return new WaitForSeconds(1f);
        }

        ClosePopupSceneryQuest();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupSceneryQuest()
    {
        if (m_iEnumeratorSceneryQuestTime != null)
        {
            StopCoroutine(m_iEnumeratorSceneryQuestTime);
            m_iEnumeratorSceneryQuestTime = null;
        }

        if (m_trPopupSceneryQuest != null)
        {
            Destroy(m_trPopupSceneryQuest.gameObject);
            m_trPopupSceneryQuest = null;
        }
    }

    #endregion 풍광 퀘스트

	//---------------------------------------------------------------------------------------------------
    IEnumerator DungeonShortCut(EnDungeon enDungeon)
    {
        yield return new WaitUntil(() => m_trSubMenu1.Find("PopupDungeonCategory") != null);

        Transform trCategory = m_trSubMenu1.Find("PopupDungeonCategory");
        int nDungeonIndex = 0;

        switch (enDungeon)
        {
            case EnDungeon.StoryDungeon:
                nDungeonIndex = m_nStoryDungeonNo - 1;
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

                /*
            case EnDungeon.GoldDungeon:
                nDungeonIndex = (int)EnIndividualDungeonType.Gold;
                break;
                */

            case EnDungeon.OsirisRoom:
                nDungeonIndex = (int)EnIndividualDungeonType.OsirisRoom;
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

            case EnDungeon.DragonNest:
                nDungeonIndex = (int)EnPartyDungeonType.DragonNest;
                break;

            case EnDungeon.TradeShip:
                nDungeonIndex = (int)EnTimeLimitDungeonType.TradeShip;
                break;

            case EnDungeon.AnkouTomb:
                nDungeonIndex = (int)EnTimeLimitDungeonType.AnkouTomb;
                break;

			case EnDungeon.TeamBattlefield:
				nDungeonIndex = (int)EnEventDungeonType.TeamBattlefield;
				break;
        }

        if (trCategory != null)
        {
            CsDungeonCartegory csDungeonCartegory = trCategory.GetComponent<CsDungeonCartegory>();
            csDungeonCartegory.ShortCutDungeonInfo(nDungeonIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayOffAutoCancel()
    {
		if (m_buttonAutoCancel.gameObject.activeSelf)
		{
            Debug.Log("##@@ DisplayOffAutoCancel ##@@");
			m_buttonAutoCancel.gameObject.SetActive(false);
			CsGameEventUIToUI.Instance.OnEventDisplayJoystickEffect(false);
		}
    }

    //---------------------------------------------------------------------------------------------------
    CsNpcInfo GetCurrentMainQuestNpcInfo()
    {
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;

        if (csMainQuest != null)
        {
            CsNpcInfo csNpcInfo = null;

            switch (CsMainQuestManager.Instance.MainQuestState)
            {
                case EnMainQuestState.None:
                    csNpcInfo = CsMainQuestManager.Instance.MainQuest.StartNpc;
                    return csNpcInfo;

                case EnMainQuestState.Executed:
                    csNpcInfo = CsMainQuestManager.Instance.MainQuest.CompletionNpc;
                    return csNpcInfo;

                default:
                    return null;
            }
        }
        else
        {
            return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    // Npc 상점창 열기
    IEnumerator LoadPopupNpcShop(int nNpcId)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupNpcShop/PopupNpcShop");
        yield return resourceRequest;
        GameObject goPopupNpcShop = (GameObject)resourceRequest.asset;

		Transform trCanvas2PopupList = GameObject.Find("Canvas2/PopupList").transform;

		Transform trPopupNpcShop = trCanvas2PopupList.Find("PopupNpcShop");

        if (trPopupNpcShop == null)
        {
			trPopupNpcShop = Instantiate(goPopupNpcShop, trCanvas2PopupList).transform;
            trPopupNpcShop.name = "PopupNpcShop";
        }

        CsPopupNpcShop csPopupNpcShop = trPopupNpcShop.GetComponent<CsPopupNpcShop>();

        if (csPopupNpcShop == null)
        {
            Destroy(trPopupNpcShop.gameObject);
        }
        else
        {
            csPopupNpcShop.DisplayPopupNpcShop(nNpcId);
        }
    }

    #region InfiniteWar

    //---------------------------------------------------------------------------------------------------
    void InitializePanelInfiniteWar(PDHero[] pDHeroes)
    {
        Transform trImageBackground = m_trPanelInfiniteWar.Find("ImageBackground");

        Text textPoint = trImageBackground.Find("TextPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPoint);
        textPoint.text = CsConfiguration.Instance.GetString("A112_TXT_00001");

        Text textMyPointValue = trImageBackground.Find("TextMyPointValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMyPointValue);
        textMyPointValue.text = "0";

        const int nRankingNo = 3;
        Transform trRankList = trImageBackground.Find("RankList");
        Transform trImageRank = null;

        for (int i = 0; i < trRankList.childCount; i++)
        {
            trRankList.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < nRankingNo; i++)
        {
            trImageRank = trRankList.Find("ImageRank" + i);

            if (trImageRank == null)
            {
                continue;
            }
            else
            {
                Text textRank = trImageRank.Find("TextRank").GetComponent<Text>();
                CsUIData.Instance.SetFont(textRank);

                Text textName = trImageRank.Find("TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);

                Text textPointValue = trImageRank.Find("TextPoint").GetComponent<Text>();
                CsUIData.Instance.SetFont(textPointValue);

                if (i == 0)
                {
                    textName.text = CsGameData.Instance.MyHeroInfo.Name;
                    textRank.text = (i + 1).ToString();
                    textPointValue.text = "0";
                }
                else
                {
                    if (i - 1 < pDHeroes.Length)
                    {
                        textName.text = pDHeroes[i - 1].name;
                        textRank.text = (i + 1).ToString();
                        textPointValue.text = "0";
                    }
                    else
                    {
                        continue;
                    }
                }

                trImageRank.gameObject.SetActive(true);
            }
        }

        m_trPanelInfiniteWar.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePanelInfiniteWar()
    {
        if (m_trPanelInfiniteWar == null)
        {
            return;
        }
        else
        {
            Transform trImageBackground = m_trPanelInfiniteWar.Find("ImageBackground");

            Text textMyPointValue = trImageBackground.Find("TextMyPointValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textMyPointValue);
            textMyPointValue.text = CsDungeonManager.Instance.DicInfiniteWarHeroPoint[CsGameData.Instance.MyHeroInfo.HeroId].Point.ToString("#,##0");

            Transform trRankList = trImageBackground.Find("RankList");
            Transform trImageRank = null;

            int nIndex = 0;
            const int nRankingNo = 3;

            foreach (var item in CsDungeonManager.Instance.DicInfiniteWarHeroPoint.OrderByDescending(infiniteWarPoint => infiniteWarPoint.Value.Point).ThenBy(infiniteWarPointUpdatedTimeTicks => infiniteWarPointUpdatedTimeTicks.Value.PointUpdatedTimeTicks))
            {
                if (nIndex < nRankingNo)
                {
                    trImageRank = trRankList.Find("ImageRank" + nIndex);

                    if (trImageRank == null)
                    {
                        continue;
                    }
                    else
                    {
                        Text textName = trImageRank.Find("TextName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textName);

                        Text textPointValue = trImageRank.Find("TextPoint").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textPointValue);

                        textName.text = item.Value.HeroName;
                        textPointValue.text = item.Value.Point.ToString("#,##0");

                        nIndex++;
                    }
                }
                else
                {
                    return;
                }
            }

            if (nIndex < nRankingNo)
            {
                for (int i = nIndex; i < nRankingNo; i++)
                {
                    trImageRank = trRankList.Find("ImageRank" + nIndex);

                    if (trImageRank == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (trImageRank.gameObject.activeSelf)
                        {
                            trImageRank.gameObject.SetActive(false);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
    }

    #endregion InfiniteWar

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void UpdateWarMemoryPanelDungeon()
    {
        Transform trImageBackground = m_trPanelDungeon.Find("ImageBackground");

        Text textAcquirePointValue = trImageBackground.Find("ImageAcquirePoint/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAcquirePointValue);
        textAcquirePointValue.text = CsDungeonManager.Instance.DicWarMemoryHeroPoint[CsGameData.Instance.MyHeroInfo.HeroId].Point.ToString("#,##0");

        Text textRanking = trImageBackground.Find("ImageDungeonValue2/Text").GetComponent<Text>();
        Text textRankingValue = trImageBackground.Find("ImageDungeonValue2/TextValue").GetComponent<Text>();

        CsUIData.Instance.SetFont(textRanking);
        CsUIData.Instance.SetFont(textRankingValue);

        textRanking.text = CsConfiguration.Instance.GetString("A121_TXT_00002");

        int nMyRanking = 0;

        foreach (var item in CsDungeonManager.Instance.DicWarMemoryHeroPoint.OrderByDescending(warMemoryPoint => warMemoryPoint.Value.Point).ThenBy(warMemoryPointUpdatedTimeTicks => warMemoryPointUpdatedTimeTicks.Value.PointUpdatedTimeTicks))
        {
            nMyRanking++;

            if (item.Key == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                textRankingValue.text = nMyRanking.ToString("#,##0");
                break;
            }
            else
            {
                continue;
            }
        }
    }

    #endregion WarMemory

    #region TradeShip

    //---------------------------------------------------------------------------------------------------
    void UpdateTradeShipPanelDungeon()
    {
        Transform trImageBackground = m_trPanelDungeon.Find("ImageBackground");

        Text textAcquirePointValue = trImageBackground.Find("ImageAcquirePoint/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAcquirePointValue);
        textAcquirePointValue.text = CsDungeonManager.Instance.TradeShipPoint.ToString("#,##0");

        Text textCurrentStep = trImageBackground.Find("ImageDungeonValue2/Text").GetComponent<Text>();
        Text textCurrentStepValue = trImageBackground.Find("ImageDungeonValue2/TextValue").GetComponent<Text>();

        CsUIData.Instance.SetFont(textCurrentStep);
        CsUIData.Instance.SetFont(textCurrentStepValue);

        textCurrentStep.text = CsConfiguration.Instance.GetString("A124_TXT_00001");

        int nStepNo = 0;

        if (CsDungeonManager.Instance.TradeShipStep == null)
        {
            nStepNo = 0;
        }
        else
        {
            nStepNo = CsDungeonManager.Instance.TradeShipStep.StepNo;
        }

        textCurrentStepValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nStepNo, CsGameData.Instance.TradeShip.TradeShipStepList.Count);
    }

    #endregion TradeShip

    #region AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void UpdateAnkouTombPanelDungeon()
    {
        Transform trImageBackground = m_trPanelDungeon.Find("ImageBackground");

        Text textAcquirePointValue = trImageBackground.Find("ImageAcquirePoint/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAcquirePointValue);
        textAcquirePointValue.text = CsDungeonManager.Instance.AnkouTombPoint.ToString("#,##0");

        Text textCurrentWave = trImageBackground.Find("ImageDungeonValue2/Text").GetComponent<Text>();
        Text textCurrentWaveValue = trImageBackground.Find("ImageDungeonValue2/TextValue").GetComponent<Text>();

        CsUIData.Instance.SetFont(textCurrentWave);
        CsUIData.Instance.SetFont(textCurrentWaveValue);

        textCurrentWave.text = CsConfiguration.Instance.GetString("A124_TXT_00001");
        textCurrentWaveValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), m_nAnkouTombWaveNo, CsGameData.Instance.AnkouTomb.WaveCount); 
    }

    #endregion AnkouTomb

    #region OsirisRoom

    int m_nOsirisRoomRemainingMonsterCount = 0;

    void UpdatePanelOsirisRoom()
    {
        Text textCurrentWave = m_trPanelOsirisRoom.Find("ImageCurrentWave/Text").GetComponent<Text>();
        Text textCurrentWaveValue = m_trPanelOsirisRoom.Find("ImageCurrentWave/TextWave").GetComponent<Text>();
        Text textCurrentMonster = m_trPanelOsirisRoom.Find("ImageCurrentMonster/Text").GetComponent<Text>();
        Text textCurrentMonsterValue = m_trPanelOsirisRoom.Find("ImageCurrentMonster/TextMonster").GetComponent<Text>();
        Text textGoldValue = m_trPanelOsirisRoom.Find("ImageCurrentGold/TextGold").GetComponent<Text>();

        CsUIData.Instance.SetFont(textCurrentWave);
        CsUIData.Instance.SetFont(textCurrentWaveValue);
        CsUIData.Instance.SetFont(textCurrentMonster);
        CsUIData.Instance.SetFont(textCurrentMonsterValue);
        CsUIData.Instance.SetFont(textGoldValue);

        textCurrentWave.text = CsConfiguration.Instance.GetString("A124_TXT_00001");
        textCurrentMonster.text = CsConfiguration.Instance.GetString("A124_TXT_00002");

        if (CsDungeonManager.Instance.OsirisRoomDifficulty != null && CsDungeonManager.Instance.OsirisRoomDifficultyWave != null)
        {
            textCurrentWaveValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsDungeonManager.Instance.OsirisRoomDifficultyWave.WaveNo, CsDungeonManager.Instance.OsirisRoomDifficulty.OsirisRoomDifficultyWaveList.Count);
            textCurrentMonsterValue.text = m_nOsirisRoomRemainingMonsterCount.ToString("#,##0");
        }
        else
        {
            if (CsDungeonManager.Instance.OsirisRoomDifficulty == null)
            {
                textCurrentWaveValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 0, 0);
            }
            else
            {
                textCurrentWaveValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 0, CsDungeonManager.Instance.OsirisRoomDifficulty.OsirisRoomDifficultyWaveList.Count);
            }

            textCurrentMonsterValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), 0, 0);
        }

        textGoldValue.text = CsDungeonManager.Instance.OsirisRoomTotalGoldAcquisitionValue.ToString("#,##0");
    }

    #endregion OsirisRoom

    //---------------------------------------------------------------------------------------------------
    bool CheckQuestToggleAllOff()
    {
        bool bReAutoQuest = true;

        Transform trQuest = m_trQuestPartyPanel.Find("Quest");
        Transform trQuestContent = trQuest.Find("Scroll View/Viewport/Content");
        
        for (int i = 0; i < trQuestContent.childCount; i++)
        {
            if (trQuestContent.GetChild(i).gameObject.activeSelf && trQuestContent.GetChild(i).GetComponent<Toggle>().isOn)
            {
                bReAutoQuest = false;
                break;
            }
            else
            {
                continue;
            }
        }

        if (bReAutoQuest && CsUIData.Instance.AutoStateType != EnAutoStateType.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 메인 퀘스트와 서브 퀘스트가 같은 NPC에 속한 경우
	// 한 가지를 수락/완료하면 나머지 퀘스트를 이어서 진행
	void CheckNextQuest(bool bCompleteMainQuest = false)
	{
		if (m_nNpcId <= 0)
			return;

		if (bCompleteMainQuest)
		{
			bool bMainQuest = false;
			
			if (CsGameData.Instance.MyHeroInfo.Level >= CsMainQuestManager.Instance.MainQuest.RequiredHeroLevel &&
				CsMainQuestManager.Instance.MainQuest != null &&
				CsMainQuestManager.Instance.PrevMainQuest != null)
			{
				if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None &&
					CsMainQuestManager.Instance.MainQuest.StartNpc != null &&
					CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc != null &&
					CsMainQuestManager.Instance.MainQuest.StartNpc.NpcId == CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc.NpcId)
				{
					bMainQuest = true;
				}
				// 메인퀘스트 자동수락 후 이전 퀘스트의 완료 NPC와 현재 퀘스트의 완료 NPC가 동일한 경우
				else if (CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc != null &&
						CsMainQuestManager.Instance.MainQuest.StartNpc == null &&
						CsMainQuestManager.Instance.MainQuest.CompletionNpc != null &&
						CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc.NpcId == CsMainQuestManager.Instance.MainQuest.CompletionNpc.NpcId)
				{
					bMainQuest = true;
				}
			}

			if (bMainQuest)
			{
				UpdatePopupMainQuest();
			}
			else
			{
				CsSubQuest csSubQuest = CsSubQuestManager.Instance.GetSubQuestFromNpc(m_nNpcId);

				if (csSubQuest != null)
				{
					OpenPopupSubQuest(csSubQuest);
				}
			}
		}
		else
		{
			ContinueNextQuest();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ContinueNextQuest()
	{
		if (m_bSelectedMainQuestPanel)
		{
			CsSubQuest csSubQuest = CsSubQuestManager.Instance.GetSubQuestFromNpc(m_nNpcId);
			
			if (csSubQuest != null)
			{
				OpenPopupSubQuest(csSubQuest);
			}
		}
		else if (m_nSelectedSubQuest > 0)
		{
			if (CsMainQuestManager.Instance.MainQuest != null)
			{
				if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None &&
					CsMainQuestManager.Instance.MainQuest.StartNpc != null &&
					CsMainQuestManager.Instance.MainQuest.StartNpc.NpcId == m_nNpcId)
				{
					UpdatePopupMainQuest();
				}
				else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed &&
					CsMainQuestManager.Instance.MainQuest.CompletionNpc != null &&
					CsMainQuestManager.Instance.MainQuest.CompletionNpc.NpcId == m_nNpcId)
				{
					UpdatePopupMainQuest();
				}
			}
		}
	}

    #region GuildBlessingBuff

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupGuildBlessingBuff()
    {
        CloseNpcDialogPopup();

        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupGuildBlessing");
        yield return resourceRequest;

        m_trPopupGuildBlessing = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        m_trPopupGuildBlessing.name = "PopupGuildBlessing";

        InitializePopupGuildBlessingBuff();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupGuildBlessingBuff()
    {
        if (m_trPopupGuildBlessing == null)
        {
            return;
        }
        else
        {
            m_trPopupGuildBlessing.gameObject.SetActive(false);

            Transform trImageBackground = m_trPopupGuildBlessing.Find("ImageBackground");

            Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = CsConfiguration.Instance.GetString("A131_TXT_00004");

            Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonClose.onClick.AddListener(OnClickClosePopupGuildBlessingBuff);

            Text textDiaValue = trImageBackground.Find("ImageGoods/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDiaValue);
            textDiaValue.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");

            Transform trGuildBlessingList = trImageBackground.Find("GuildBlessingList");
            Transform trGuildBlessing = null;

            for (int i = 0; i < trGuildBlessingList.childCount; i++)
            {
                if (i < CsGameData.Instance.GuildBlessingBuffList.Count)
                {
                    trGuildBlessing = trGuildBlessingList.Find("GuildBlessing" + i);

                    if (trGuildBlessing == null)
                    {
                        continue;
                    }
                    else
                    {
                        CsGuildBlessingBuff csGuildBlessingBuff = CsGameData.Instance.GuildBlessingBuffList[i];

                        Text textBlessingName = trGuildBlessing.Find("TextBlessingName").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textBlessingName);
                        textBlessingName.text = csGuildBlessingBuff.Name;

                        Text textAdditionalExp = trGuildBlessing.Find("TextAdditionalExp").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textAdditionalExp);
                        textAdditionalExp.text = string.Format(CsConfiguration.Instance.GetString(csGuildBlessingBuff.Description), (1 - csGuildBlessingBuff.ExpRewardFactor) * 100);

                        Button buttonBlessing = trGuildBlessing.Find("ButtonBlessing").GetComponent<Button>();

                        if (DateTime.Compare(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date, CsGuildManager.Instance.Guild.LastBlessingBuffStartDate) == 0 || CsGuildManager.Instance.Guild.IsBlessingBuffRunning)
                        {
                            buttonBlessing.interactable = false;
                        }
                        else
                        {
                            buttonBlessing.onClick.RemoveAllListeners();
                            buttonBlessing.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                            buttonBlessing.onClick.AddListener(() => OnClickGuildBlessingBuff(csGuildBlessingBuff));
                            buttonBlessing.interactable = true;
                        }

                        Text textButtonBlessingDiaValue = buttonBlessing.transform.Find("TextValue").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textButtonBlessingDiaValue);
                        textButtonBlessingDiaValue.text = csGuildBlessingBuff.Dia.ToString("#,##0");
                    }
                }
                else
                {
                    continue;
                }
            }

            m_trPopupGuildBlessing.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupGuildBlessingBuff()
    {
        ClosePopupGuildBlessingBuff();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildBlessingBuff(CsGuildBlessingBuff csGuildBlessingBuff)
    {
        if (CsGuildManager.Instance.Guild == null)
        {
            return;
        }
        else
        {
            if (CsGuildManager.Instance.MyGuildMemberGrade.GuildBlessingBuffEnabled && DateTime.Compare(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date, CsGuildManager.Instance.Guild.LastBlessingBuffStartDate) != 0 && 
                !CsGuildManager.Instance.Guild.IsBlessingBuffRunning)
            {
                if (CsGameData.Instance.MyHeroInfo.Dia < csGuildBlessingBuff.Dia)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
                }
                else
                {
                    CsGuildManager.Instance.SendGuildBlessingBuffStart(csGuildBlessingBuff.BuffId);
                    ClosePopupGuildBlessingBuff();
                }
            }
            else
            {
                // 사용 불가
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupGuildBlessingBuff()
    {
        if (m_trPopupGuildBlessing == null)
        {
            return;
        }
        else
        {
            Destroy(m_trPopupGuildBlessing.gameObject);
        }
    }

    #endregion GuildBlessingBuff

}