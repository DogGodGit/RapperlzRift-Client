using ClientCommon;
using SimpleDebugLog;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnDungeonPlay { None = 0, MainQuest, Story, Exp, Gold, UndergroundMaze, ArtifactRoom, AncientRelic, FieldOfHonor, SoulCoveter, Elite, ProofOfValor, WisdomTemple, RuinsReclaim, InfiniteWar, FearAltar, WarMemory, OsirisRoom , Biography , DragonNest , AnkouTomb, TradeShip, TeamBattlefield }
public enum EnDungeonEnterType { InitEnter = 0, Revival, Portal, Transmission }
public enum EnWisdomTempleType { None, ColorMatching, FindTreasureBox, PuzzleReward, Quiz }
public enum EnDungeonMatchingState { None = 0, Matching, MatchReady, MatchComplete }
public enum EnMatchingRoomBanishedType { Dead = 1, CartRide = 2, OpenTime = 3, Item = 4, Location = 5, DungeonEnter = 6 }
public enum EnBattlefieldState { None = 0, Waiting, Playing, End }

public class CsDungeonManager
{
	// StoryDungeon
	int m_nStoryDungeonNo;
	CsStoryDungeon m_csStoryDungeon;
	CsStoryDungeonDifficulty m_csStoryDungeonDifficulty;
	CsStoryDungeonStep m_csStoryDungeonStep;

	// ExpDungeon
	CsExpDungeonDifficulty m_csExpDungeonDifficulty;
	CsExpDungeonDifficultyWave m_csExpDungeonDifficultyWave;

	// GoldDungeon
	CsGoldDungeonDifficulty m_csGoldDungeonDifficulty;
	CsGoldDungeonStep m_csGoldDungeonStep;
	CsGoldDungeonStepWave m_csGoldDungeonStepWave;

	// UndergroundMaze
	int m_nUndergroundMazeFloor;
	CsUndergroundMazeFloor m_csUndergroundMazeFloor;

	// ArtifactRoom
	CsArtifactRoomFloor m_csArtifactRoomFloor;

	// AncientRelic
	float m_flAncientRelicCurrentPoint = 0;
	float m_flAncientRelicMatchingRemainingTime = 0f;
	CsAncientRelicStep m_csAncientRelicStep;
	CsAncientRelicStepWave m_csAncientRelicStepWave;
	EnDungeonMatchingState m_enAncientRelicState = EnDungeonMatchingState.None;

	// FieldOfHonor
	PDHero m_pDHeroFieldOfHonor = null;

	// SoulCoveter
	float m_flSoulCoveterMatchingRemainingTime = 0f;
	CsSoulCoveterDifficulty m_csSoulCoveterDifficulty;
	CsSoulCoveterDifficultyWave m_csSoulCoveterDifficultyWave;
	EnDungeonMatchingState m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;

	// Elite
	CsEliteMonster m_csEliteMonster;

	// WisdomTemple
	CsWisdomTempleStep m_csWisdomTempleStep;
	CsWisdomTemplePuzzle m_csWisdomTemplePuzzle;
	EnWisdomTempleType m_enWisdomTempleType = EnWisdomTempleType.None;

	// RuinsReclaim
	float m_flRuinsReclaimMatchingRemainingTime = 0f;
	CsRuinsReclaimStep m_csRuinsReclaimStep;
	CsRuinsReclaimStepWave m_csRuinsReclaimStepWave;
	EnDungeonMatchingState m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;

	//ProofOfValor
	CsProofOfValorBuffBox m_csProofOfValorBuffBox;

	//InfiniteWar
	EnDungeonMatchingState m_enInfiniteWarMatchingState = EnDungeonMatchingState.None;
	CsInfiniteWarBuffBox m_csInfiniteWarBuffBox;

	// FearAltar
	float m_flFearAltarMatchingRemainingTime = 0f;
	CsFearAltarStage m_csFearAltarStage;
	CsFearAltarStageWave m_csFearAltarStageWave;
	EnDungeonMatchingState m_enFearAltarMatchingState = EnDungeonMatchingState.None;

	// WarMemory
	CsWarMemoryWave m_csWarMemoryWave;
	EnDungeonMatchingState m_enWarMemoryMatchingState = EnDungeonMatchingState.None;

	// OsirisRoom
	CsOsirisRoomDifficulty m_csOsirisRoomDifficulty;
	CsOsirisRoomDifficultyWave m_csOsirisRoomDifficultyWave;

	// Biography
	CsBiographyQuestDungeon m_csBiographyQuestDungeon;
	CsBiographyQuestDungeonWave m_csBiographyQuestDungeonWave;

	// DragonNest
	float m_flDragonNestMatchingRemainingTime = 0f;
	CsDragonNestStep m_csDragonNestStep;
	EnDungeonMatchingState m_enDragonNestMatchingState = EnDungeonMatchingState.None;

	//AnkouTomb
	float m_flAnkouTombMatchingRemainingTime = 0f;
    int m_nAnkouTombPoint = 0;
	EnDungeonMatchingState m_enAnkouTombMatchingState = EnDungeonMatchingState.None;

	// TradeShip
	float m_flTradeShipMatchingRemainingTime = 0f;
    int m_nTradeShipPoint = 0;
	CsTradeShipStep m_csTradeShipStep;
	EnDungeonMatchingState m_enTradeShipMatchingState = EnDungeonMatchingState.None;

	// TeamBattlefield
	float m_flTeamBattlefieldRemainingEnterWaitingTime = 0f;
	int m_nTeamBattlefieldKillcount = 0;
	int m_nTeamBattlefieldPoint = 0;
	EnBattlefieldState m_enBattlefieldState = EnBattlefieldState.None;

	// 공용.
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bDungeonStart = false;
	bool m_bDungeonClear = false;

	int m_nWaveNo;
	int m_nDifficulty;
	int m_nArrangeNo;
	int m_nObjectId;
	int m_nPortalId;
	long m_lInstanceId;
	float m_flInteractionDuration;
	long m_lBuffBoxInstanceId;
    float m_flMultiDungeonRemainingStartTime;
    float m_flMultiDungeonRemainingLimitTime;

	EnDungeonPlay m_enDungeonPlay = EnDungeonPlay.None;
	EnDungeonEnterType m_enDungeonEnterType = EnDungeonEnterType.InitEnter;
	EnInteractionState m_enInteractionState = EnInteractionState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsDungeonManager Instance
	{
		get { return CsSingleton<CsDungeonManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	#region Delegate.Event

	public event Delegate<EnDungeonPlay> EventDungeonStartAutoPlay;
	public event Delegate<object, EnDungeonPlay> EventDungeonStopAutoPlay;
	public event DelegateR<RectTransform, int> EventCreateUndergroundMazeNpcHUD;								// UndergroundMazeNpcHUD 생성.
	public event Delegate EventLakMonsterDead;																	// LakMonster 사망.
	public event Delegate EventMyHeroDungeonEnterMoveStop;
	public event Delegate EventUpdateAncientRelicMember;
	public event Delegate EventUpdateDungeonMember;
	public event Delegate EventDungeonClear;
	public event Delegate<bool> EventDungeonResult;
	public event Delegate<Guid> EventPartyDungeonHeroExit;
	public event Delegate EventDugeonObjectInteractionStart;													// 던전 상호작용버튼을 통한 시작
	public event Delegate EventDugeonObjectInteractionCancel;													// 던전 취소
	public event Delegate<EnInteractionState> EventChangeDungeonInteractionState;								// 던전 상호작용 오브젝트 버튼 On / Off.
	public event Delegate EventDungeonInteractionStartCancel;													// 상호작용 요청 후 에러코드 취소 처리.
	public event Delegate<int> EventDungeonEnterFail;															// 던전 입장 실패.
	public event DelegateR<RectTransform, EnDungeonPlay, long, CsMonsterInfo, bool, int> EventCreateMonsterHUD;	// HUD 생성.
	public event Delegate<long> EventDeleteMonsterHUD;															// HUD 삭제.

	// StoryDungeon
	public event Delegate EventContinentExitForStoryDungeonEnter;
	public event Delegate<PDVector3, float, Guid> EventStoryDungeonEnter;
	public event Delegate<int> EventStoryDungeonAbandon;
	public event Delegate<int> EventStoryDungeonExit;
	public event Delegate EventStoryDungeonRevive;
	public event Delegate EventStoryDungeonSweep;
	public event Delegate EventStoryDungeonMonsterTame;
	public event Delegate EventStoryDungeonRemoveTaming;
	public event Delegate EventStoryDungeonHeroStartTame;
	public event Delegate<bool> EventStoryDungeonTameable;
	public event Delegate<int> EventStoryDungeonTrapCast;
	public event Delegate<int, int, int, long[], PDAbnormalStateEffectDamageAbsorbShield[]> EventStoryDungeonTrapHit;

	public event Delegate<PDStoryDungeonMonsterInstance[]> EventStoryDungeonStepStart;
	public event Delegate<PDItemBooty[]> EventStoryDungeonClear;
	public event Delegate EventStoryDungeonFail;
	public event Delegate<int> EventStoryDungeonBanished;
	public event Delegate EventStoryDungeonAccelerationStarted;

	// ExpDungeon
	public event Delegate EventContinentExitForExpDungeonEnter;
	public event Delegate<PDVector3, float, Guid> EventExpDungeonEnter;
	public event Delegate<int> EventExpDungeonAbandon;
	public event Delegate<int> EventExpDungeonExit;
	public event Delegate EventExpDungeonRevive;
	public event Delegate<bool, long> EventExpDungeonSweep;

	public event Delegate<PDExpDungeonMonsterInstance[], PDExpDungeonLakChargeMonsterInstance> EventExpDungeonWaveStart;
	public event Delegate EventExpDungeonWaveCompleted;
	public event Delegate EventExpDungeonWaveTimeout;
	public event Delegate<bool, long> EventExpDungeonClear;
	public event Delegate<int> EventExpDungeonBanished;

	// GoldDungeon
	public event Delegate EventContinentExitForGoldDungeonEnter;
	public event Delegate<PDVector3, float, Guid> EventGoldDungeonEnter;
	public event Delegate<int> EventGoldDungeonAbandon;
	public event Delegate<int> EventGoldDungeonExit;
	public event Delegate EventGoldDungeonRevive;
	public event Delegate<long> EventGoldDungeonSweep;

	public event Delegate<PDGoldDungeonMonsterInstance[]> EventGoldDungeonStepStart;
	public event Delegate<long> EventGoldDungeonStepCompleted;
	public event Delegate<int> EventGoldDungeonWaveStart;
	public event Delegate EventGoldDungeonWaveCompleted;
	public event Delegate EventGoldDungeonWaveTimeout;
	public event Delegate<long> EventGoldDungeonClear;
	public event Delegate EventGoldDungeonFail;
	public event Delegate<int> EventGoldDungeonBanished;

	// UndergroundMaze
	public event Delegate EventMyHeroUndergroundMazePortalEnter;
	public event Delegate EventContinentExitForUndergroundMazeEnter;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDUndergroundMazeMonsterInstance[]> EventUndergroundMazeEnter;
	public event Delegate<int> EventUndergroundMazeExit;
	public event Delegate EventUndergroundMazeRevive;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDUndergroundMazeMonsterInstance[]> EventUndergroundMazeEnterForUndergroundMazeRevive;
	public event Delegate EventUndergroundMazePortalEnter;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDUndergroundMazeMonsterInstance[]> EventUndergroundMazePortalExit;
	public event Delegate<int> EventUndergroundMazeTransmissionNpcDialog;
	public event Delegate EventUndergroundMazeTransmission;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDUndergroundMazeMonsterInstance[]> EventUndergroundMazeEnterForTransmission;
	public event Delegate<int> EventUndergroundMazeBanished;

	//ArtifactRoom
	public event Delegate EventContinentExitForArtifactRoomEnter;
	public event Delegate<Guid, PDVector3, float> EventArtifactRoomEnter;
	public event Delegate<int> EventArtifactRoomExit;
	public event Delegate<int> EventArtifactRoomAbandon;
	public event Delegate EventArtifactRoomNextFloorChallenge;
	public event Delegate EventArtifactRoomInit;
	public event Delegate EventArtifactRoomSweep;
	public event Delegate<PDItemBooty[]> EventArtifactRoomSweepAccelerate;
	public event Delegate<PDItemBooty[]> EventArtifactRoomSweepComplete;
	public event Delegate<PDItemBooty[]> EventArtifactRoomSweepStop;

	public event Delegate<PDArtifactRoomMonsterInstance[]> EventArtifactRoomStart;
	public event Delegate<PDItemBooty> EventArtifactRoomClear;
	public event Delegate EventArtifactRoomFail;
	public event Delegate<int> EventArtifactRoomBanished;
	public event Delegate EventArtifactRoomBanishedForNextFloorChallenge;
	public event Delegate EventArtifactRoomSweepNextFloorStart;
	public event Delegate EventArtifactRoomSweepCompleted;

	// AncientRelic
	public event Delegate EventAncientRelicMatchingStart;
	public event Delegate EventAncientRelicMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], Guid[], int[]> EventAncientRelicEnter;
	public event Delegate<int> EventAncientRelicExit;
	public event Delegate<int> EventAncientRelicAbandon;
	public event Delegate EventAncientRelicRevive;

	public event Delegate EventAncientRelicMatchingStatusChanged;
	public event Delegate EventAncientRelicMatchingRoomBanished;
	public event Delegate EventContinentExitForAncientRelicEnter;
	public event Delegate<int, Vector3, float> EventAncientRelicStepStart;
	public event Delegate<PDItemBooty[]> EventAncientRelicStepCompleted;
	public event Delegate<PDAncientRelicMonsterInstance[]> EventAncientRelicWaveStart;
	public event Delegate EventAncientRelicClear;
	public event Delegate EventAncientRelicFail;
	public event Delegate<int> EventAncientRelicBanished;
	public event Delegate EventAncientRelicPointUpdated;
	public event Delegate<int> EventAncientRelicTrapActivated;
	public event Delegate<Guid, int, int, int, long[], PDAbnormalStateEffectDamageAbsorbShield[]> EventAncientRelicTrapHit;
	public event Delegate<Guid> EventAncientRelicTrapEffectFinished;
	public event Delegate EventAncientRelicMatchingRoomPartyEnter;

	//FieldOfHonor
	public event Delegate<PDFieldOfHonorHistory[], PDFieldOfHonorHero[]> EventFieldOfHonorInfo;
	public event Delegate<PDFieldOfHonorRanking[]> EventFieldOfHonorTopRankingList;
	public event Delegate EventContinentExitForFieldOfHonorChallenge;
	public event Delegate<Guid, PDVector3, float, PDHero> EventFieldOfHonorChallenge;
	public event Delegate<int> EventFieldOfHonorExit;
	public event Delegate<int> EventFieldOfHonorAbandon;
	public event Delegate<PDItemBooty[]> EventFieldOfHonorRankingRewardReceive;
	public event Delegate<PDFieldOfHonorHero> EventFieldOfHonorRankerInfo;

	public event Delegate EventFieldOfHonorStart;
	public event Delegate<bool, long, int> EventFieldOfHonorClear;
	public event Delegate<bool, long, int> EventFieldOfHonorFail;
	public event Delegate<int> EventFieldOfHonorBanished;
	public event Delegate EventFieldOfHonorDailyRankingUpdated;

	//SoulCoveter
	public event Delegate EventSoulCoveterMatchingStart;
	public event Delegate EventSoulCoveterMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[]> EventSoulCoveterEnter;
	public event Delegate<int> EventSoulCoveterExit;
	public event Delegate<int> EventSoulCoveterAbandon;
	public event Delegate EventSoulCoveterRevive;

	public event Delegate EventSoulCoveterMatchingStatusChanged;
	public event Delegate EventSoulCoveterMatchingRoomBanished;
	public event Delegate EventSoulCoveterMatchingRoomPartyEnter;
	public event Delegate EventContinentExitForSoulCoveterEnter;
	public event Delegate<PDSoulCoveterMonsterInstance[]> EventSoulCoveterWaveStart;
	public event Delegate<PDItemBooty[]> EventSoulCoveterClear;
	public event Delegate EventSoulCoveterFail;
	public event Delegate<int> EventSoulCoveterBanished;

	// Elite
	public event Delegate EventContinentExitForEliteDungeonEnter;
	public event Delegate<Guid, PDVector3, float, PDEliteDungeonMonsterInstance[]> EventEliteDungeonEnter;
	public event Delegate<int> EventEliteDungeonExit;
	public event Delegate<int> EventEliteDungeonAbandon;
	public event Delegate EventEliteDungeonRevive;

	public event Delegate EventEliteDungeonStart;
	public event Delegate EventEliteDungeonClear;
	public event Delegate EventEliteDungeonFail;
	public event Delegate<int> EventEliteDungeonBanished;

	// ProofOfValor
	public event Delegate EventContinentExitForProofOfValorEnter;
	public event Delegate<Guid, PDVector3, float, PDMonsterInstance[]> EventProofOfValorEnter;
	public event Delegate<int> EventProofOfValorExit;
	public event Delegate<int, bool, long> EventProofOfValorAbandon;
	public event Delegate<bool, long> EventProofOfValorSweep;
	public event Delegate EventProofOfValorRefresh;
	public event Delegate<int> EventProofOfValorBuffBoxAcquire;

	public event Delegate EventProofOfValorStart;
	public event Delegate<bool, long> EventProofOfValorClear;
	public event Delegate<bool, long> EventProofOfValorFail;
	public event Delegate<int> EventProofOfValorBanished;
	public event Delegate<PDProofOfValorBuffBoxInstance[]> EventProofOfValorBuffBoxCreated;
	public event Delegate EventProofOfValorBuffBoxLifetimeEnded;
	public event Delegate EventProofOfValorBuffFinished;
	public event Delegate EventProofOfValorRefreshed;
	public event Delegate<CsHeroCreatureCard> EventProofOfValorGetCreatureCard;

	// WisdomTemple
	public event Delegate EventContinentExitForWisdomTempleEnter;
	public event Delegate<Guid, PDVector3, float> EventWisdomTempleEnter;
	public event Delegate<int> EventWisdomTempleExit;
	public event Delegate<int> EventWisdomTempleAbandon;
	public event Delegate<int> EventWisdomTempleColorMatchingObjectInteractionStart;
	public event Delegate<PDWisdomTempleColorMatchingObjectInstance[], int> EventWisdomTempleColorMatchingObjectCheck;
	public event Delegate EventWisdomTemplePuzzleRewardObjectInteractionStart;
	public event Delegate<bool, long, PDItemBooty> EventWisdomTempleSweep;

	public event Delegate<PDWisdomTempleMonsterInstance[], PDWisdomTempleColorMatchingObjectInstance[], int> EventWisdomTempleStepStart;
	public event Delegate<bool, long, PDItemBooty> EventWisdomTempleStepCompleted;
	public event Delegate<PDWisdomTempleColorMatchingObjectInstance> EventWisdomTempleColorMatchingObjectInteractionFinished;
	public event Delegate EventWisdomTempleColorMatchingObjectInteractionCancel;
	public event Delegate<PDWisdomTempleColorMatchingMonsterInstance> EventWisdomTempleColorMatchingMonsterCreated;
	public event Delegate<PDWisdomTempleColorMatchingObjectInstance[], int> EventWisdomTempleColorMatchingMonsterKill;
	public event Delegate<int, int, bool> EventWisdomTempleFakeTreasureBoxKill;
	public event Delegate<bool, long, PDWisdomTemplePuzzleRewardObjectInstance[]> EventWisdomTemplePuzzleCompleted;
	public event Delegate<PDItemBooty, long> EventWisdomTemplePuzzleRewardObjectInteractionFinished;
	public event Delegate EventWisdomTemplePuzzleRewardObjectInteractionCancel;
	public event Delegate EventWisdomTempleQuizFail;
	public event Delegate<PDWisdomTempleBossMonsterInstance> EventWisdomTempleBossMonsterCreated;
	public event Delegate<PDItemBooty> EventWisdomTempleBossMonsterKill;
	public event Delegate EventWisdomTempleClear;
	public event Delegate EventWisdomTempleFail;
	public event Delegate<int> EventWisdomTempleBanished;

	// RuinsReclaim
	public event Delegate EventRuinsReclaimMatchingStart;
	public event Delegate EventRuinsReclaimMatchingCancel;
	public event Delegate EventRuinsReclaimMatchingRoomPartyEnter;
	public event Delegate EventRuinsReclaimMatchingStatusChanged;
	public event Delegate EventRuinsReclaimMatchingRoomBanished;
	public event Delegate EventContinentExitForRuinsReclaimEnter;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], PDRuinsReclaimRewardObjectInstance[], PDRuinsReclaimMonsterTransformationCancelObjectInstance[], Guid[]> EventRuinsReclaimEnter;
	public event Delegate<int> EventRuinsReclaimExit;
	public event Delegate<int> EventRuinsReclaimAbandon;
	public event Delegate<PDVector3, float> EventRuinsReclaimPortalEnter;
	public event Delegate<PDVector3, float> EventRuinsReclaimRevive;
	public event Delegate<PDRuinsReclaimRewardObjectInstance[]> EventRuinsReclaimStepStart;
	public event Delegate<PDItemBooty[]> EventRuinsReclaimStepCompleted;
	public event Delegate<PDRuinsReclaimMonsterInstance[]> EventRuinsReclaimWaveStart;
	public event Delegate EventRuinsReclaimWaveCompleted;
	public event Delegate<PDRuinsReclaimMonsterTransformationCancelObjectInstance[], PDVector3> EventRuinsReclaimStepWaveSkillCast;
	public event Delegate<PDRuinsReclaimSummonMonsterInstance[]> EventRuinsReclaimMonsterSummon;
	public event Delegate<Guid, int, int, int, long[], PDAbnormalStateEffectDamageAbsorbShield[]> EventRuinsReclaimTrapHit;
	public event Delegate EventRuinsReclaimDebuffEffectStart;
	public event Delegate EventRuinsReclaimDebuffEffectStop;
	public event Delegate<PDItemBooty[], PDItemBooty, Guid, string, PDItemBooty, Guid, string, PDItemBooty, Guid, string, PDItemBooty> EventRuinsReclaimClear;
	public event Delegate EventRuinsReclaimFail;
	public event Delegate<int> EventRuinsReclaimBanished;
	public event Delegate<int, long[]> EventRuinsReclaimMonsterTransformationStart;
	public event Delegate EventRuinsReclaimMonsterTransformationFinished;
	public event Delegate<long> EventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded;
	public event Delegate EventRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
	public event Delegate EventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
	public event Delegate<long> EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
	public event Delegate EventRuinsReclaimRewardObjectInteractionStart;
	public event Delegate EventRuinsReclaimRewardObjectInteractionCancel;
	public event Delegate<PDItemBooty, long> EventRuinsReclaimRewardObjectInteractionFinished;
	public event Delegate<Guid, PDVector3, float> EventHeroRuinsReclaimPortalEnter;
	public event Delegate<Guid, int, long[]> EventHeroRuinsReclaimMonsterTransformationStart;
	public event Delegate<Guid> EventHeroRuinsReclaimMonsterTransformationFinished;
	public event Delegate<Guid, long> EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
	public event Delegate<Guid, long> EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
	public event Delegate<Guid, long> EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
	public event Delegate<Guid, long> EventHeroRuinsReclaimRewardObjectInteractionStart;
	public event Delegate<Guid, long> EventHeroRuinsReclaimRewardObjectInteractionCancel;
	public event Delegate<Guid, long> EventHeroRuinsReclaimRewardObjectInteractionFinished;

	// InfiniteWar
	public event Delegate EventInfiniteWarMatchingStart;
	public event Delegate EventInfiniteWarMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], PDInfiniteWarBuffBoxInstance[]> EventInfiniteWarEnter;
	public event Delegate<int> EventInfiniteWarExit;
	public event Delegate<int> EventInfiniteWarAbandon;
	public event Delegate EventInfiniteWarRevive;
	public event Delegate<int> EventInfiniteWarBuffBoxAcquire;

	public event Delegate EventContinentExitForInfiniteWarEnter;
	public event Delegate EventInfiniteWarMatchingStatusChanged;
	public event Delegate EventInfiniteWarMatchingRoomBanished;
	public event Delegate EventInfiniteWarStart;
	public event Delegate<PDInfiniteWarMonsterInstance[]> EventInfiniteWarMonsterSpawn;
	public event Delegate<PDInfiniteWarBuffBoxInstance[]> EventInfiniteWarBuffBoxCreated;
	public event Delegate<long> EventInfiniteWarBuffBoxLifetimeEnded;
	public event Delegate<Guid, int, long> EventHeroInfiniteWarBuffBoxAcquisition;
	public event Delegate EventInfiniteWarBuffFinished;
	public event Delegate EventInfiniteWarPointAcquisition;
	public event Delegate EventHeroInfiniteWarPointAcquisition;
	public event Delegate<PDInfiniteWarRanking[], PDItemBooty[], PDItemBooty[]> EventInfiniteWarClear;
	public event Delegate<int> EventInfiniteWarBanished;

	// FearAltar
	public event Delegate EventFearAltarHalidomMonsterKillFail;
	public event Delegate EventFearAltarHalidomMonsterKill;
	public event Delegate EventClickFearAltarTargetHalidomMonsterTargetButton;

	public event Delegate EventFearAltarMatchingStart;
	public event Delegate EventFearAltarMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[]> EventFearAltarEnter;
	public event Delegate<int> EventFearAltarExit;
	public event Delegate<int> EventFearAltarAbandon;

	public event Delegate EventFearAltarRevive;
	
	public event Delegate EventContinentExitForFearAltarEnter;
	public event Delegate EventFearAltarMatchingRoomPartyEnter;
	public event Delegate EventFearAltarMatchingRoomBanished;
	public event Delegate EventFearAltarMatchingStatusChanged;
	public event Delegate<PDFearAltarMonsterInstance[], PDFearAltarHalidomMonsterInstance> EventFearAltarWaveStart;
	public event Delegate<long, PDFearAltarHero[], bool> EventFearAltarClear;
	public event Delegate EventFearAltarFail;
	public event Delegate<int> EventFearAltarBanished;

    // WarMemory
    public event Delegate EventWarMemoryMatchingStart;
    public event Delegate EventWarMemoryMatchingCancel;
    public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], PDWarMemoryTransformationObjectInstance[]> EventWarMemoryEnter;
    public event Delegate<int> EventWarMemoryExit;
    public event Delegate<int> EventWarMemoryAbandon;
    public event Delegate<PDVector3, float> EventWarMemoryRevive;
    public event Delegate EventWarMemoryTransformationObjectInteractionStart;

    public event Delegate EventContinentExitForWarMemoryEnter;
    public event Delegate EventWarMemoryMatchingRoomPartyEnter;
    public event Delegate EventWarMemoryMatchingRoomBanished;
    public event Delegate EventWarMemoryMatchingStatusChanged;
    public event Delegate<PDWarMemoryMonsterInstance[], PDWarMemoryTransformationObjectInstance[]> EventWarMemoryWaveStart;
    public event Delegate EventWarMemoryWaveCompleted;
    public event Delegate<long> EventWarMemoryTransformationObjectLifetimeEnded;
    public event Delegate EventWarMemoryTransformationObjectInteractionCancel;
    public event Delegate<int, long, long[]> EventWarMemoryTransformationObjectInteractionFinished;
    public event Delegate<Guid, long> EventHeroWarMemoryTransformationObjectInteractionStart;
    public event Delegate<Guid, long> EventHeroWarMemoryTransformationObjectInteractionCancel;
    public event Delegate<Guid, long, int, int, long[]> EventHeroWarMemoryTransformationObjectInteractionFinished;
    public event Delegate<long[]> EventWarMemoryMonsterTransformationCancel;
    public event Delegate<long[]> EventWarMemoryMonsterTransformationFinished;
    public event Delegate<Guid, int, int, long[]> EventHeroWarMemoryMonsterTransformationCancel;
    public event Delegate<Guid, int, int, long[]> EventHeroWarMemoryMonsterTransformationFinished;
    public event Delegate<PDWarMemorySummonMonsterInstance[]> EventWarMemoryMonsterSummon;
    public event Delegate EventWarMemoryPointAcquisition;
    public event Delegate EventHeroWarMemoryPointAcquisition;
    public event Delegate<Guid, int, PDVector3> EventHeroWarMemoryTransformationMonsterSkillCast;
    public event Delegate<PDWarMemoryRanking[], PDItemBooty[], long, bool> EventWarMemoryClear;
    public event Delegate EventWarMemoryFail;
    public event Delegate<int> EventWarMemoryBanished;

	// OsirisRoom
	public event Delegate EventOsirisRoomMonsterKillFail;

	public event Delegate EventContinentExitForOsirisRoomEnter;
	public event Delegate<Guid, PDVector3, float> EventOsirisRoomEnter;
	public event Delegate<int> EventOsirisRoomExit;
	public event Delegate<int> EventOsirisRoomAbandon;
	public event Delegate EventOsirisRoomMoneyBuffActivate;

	public event Delegate EventOsirisRoomWaveStart;
	public event Delegate<PDOsirisRoomMonsterInstance> EventOsirisRoomMonsterSpawn;
	public event Delegate EventOsirisRoomRewardGoldAcquisition;
	public event Delegate EventOsirisRoomMoneyBuffFinished;
	public event Delegate EventOsirisRoomMoneyBuffCancel;
	public event Delegate EventOsirisRoomClear;
	public event Delegate EventOsirisRoomFail;
	public event Delegate<int> EventOsirisRoomBanished;

	// Biography
	public event Delegate EventContinentExitForBiographyQuestDungeonEnter;
	public event Delegate<Guid, PDVector3, float> EventBiographyQuestDungeonEnter;
	public event Delegate<int> EventBiographyQuestDungeonExit;
	public event Delegate<int> EventBiographyQuestDungeonAbandon;
	public event Delegate EventBiographyQuestDungeonRevive;

	public event Delegate<PDBiographyQuestDungeonMonsterInstance[]> EventBiographyQuestDungeonWaveStart;
	public event Delegate EventBiographyQuestDungeonWaveCompleted;
	public event Delegate EventBiographyQuestDungeonFail;
	public event Delegate EventBiographyQuestDungeonClear;
	public event Delegate<int> EventBiographyQuestDungeonBanished;

	// DragonNest
	public event Delegate EventDragonNestMatchingStart;
	public event Delegate EventDragonNestMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], Guid[]> EventDragonNestEnter;
	public event Delegate<int> EventDragonNestExit;
	public event Delegate<int> EventDragonNestAbandon;
	public event Delegate EventDragonNestRevive;

	public event Delegate EventContinentExitForDragonNestEnter;
	public event Delegate EventDragonNestMatchingRoomPartyEnter;
	public event Delegate EventDragonNestMatchingRoomBanished;
	public event Delegate EventDragonNestMatchingStatusChanged;
	public event Delegate<PDDragonNestMonsterInstance[]> EventDragonNestStepStart;
	public event Delegate<PDItemBooty[]> EventDragonNestStepCompleted;
	public event Delegate<Guid, int, int, int, long[], PDAbnormalStateEffectDamageAbsorbShield[]> EventHeroDragonNestTrapHit;
	public event Delegate EventDragonNestTrapEffectFinished;
	public event Delegate<Guid> EventHeroDragonNestTrapEffectFinished;
	public event Delegate<PDSimpleHero[]> EventDragonNestClear;
	public event Delegate EventDragonNestFail;
	public event Delegate<int> EventDragonNestBanished;

	// AnkouTomb
	public event Delegate EventAnkouTombMatchingStart;
	public event Delegate EventAnkouTombMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], int> EventAnkouTombEnter;
	public event Delegate<int> EventAnkouTombExit;
	public event Delegate<int> EventAnkouTombAbandon;
	public event Delegate EventAnkouTombRevive;
	public event Delegate EventAnkouTombMoneyBuffActivate;
	public event Delegate<bool, long> EventAnkouTombAdditionalRewardExpReceive;

	public event Delegate EventContinentExitForAnkouTombEnter;
	public event Delegate<int> EventAnkouTombMatchingRoomPartyEnter;
	public event Delegate EventAnkouTombMatchingRoomBanished;
	public event Delegate EventAnkouTombMatchingStatusChanged;
	public event Delegate<PDAnkouTombMonsterInstance[], int> EventAnkouTombWaveStart;
	public event Delegate EventAnkouTombPointAcquisition;
	public event Delegate EventAnkouTombMoneyBuffFinished;
	public event Delegate EventAnkouTombMoneyBuffCancel;
	public event Delegate<bool, long, PDItemBooty> EventAnkouTombClear;
	public event Delegate<bool, long> EventAnkouTombFail;
	public event Delegate<int> EventAnkouTombBanished;
	public event Delegate EventAnkouTombServerBestRecordUpdated;

	// TradeShip
	public event Delegate EventTradeShipMatchingStart;
	public event Delegate EventTradeShipMatchingCancel;
	public event Delegate<Guid, PDVector3, float, PDHero[], PDMonsterInstance[], int> EventTradeShipEnter;
	public event Delegate<int> EventTradeShipExit;
	public event Delegate<int> EventTradeShipAbandon;
	public event Delegate EventTradeShipRevive;
	public event Delegate EventTradeShipMoneyBuffActivate;
	public event Delegate<bool, long> EventTradeShipAdditionalRewardExpReceive;

	public event Delegate EventContinentExitForTradeShipEnter;
	public event Delegate EventTradeShipMatchingRoomPartyEnter;
	public event Delegate EventTradeShipMatchingRoomBanished;
	public event Delegate EventTradeShipMatchingStatusChanged;
	public event Delegate<PDTradeShipMonsterInstance[], PDTradeShipAdditionalMonsterInstance[], PDTradeShipObjectInstance[]>EventTradeShipStepStart;
	public event Delegate EventTradeShipPointAcquisition;
	public event Delegate<PDItemBooty> EventTradeShipObjectDestructionReward;
	public event Delegate EventTradeShipMoneyBuffFinished;
	public event Delegate EventTradeShipMoneyBuffCancel;
	public event Delegate<bool, long, PDItemBooty> EventTradeShipClear;
	public event Delegate<bool, long> EventTradeShipFail;
	public event Delegate<int> EventTradeShipBanished;
	public event Delegate EventTradeShipServerBestRecordUpdated;

	// TeamBattlefield
	public event Delegate<bool,bool,bool,int> EventTeamBattlefieldInfo;
	public event Delegate EventTeamBattlefieldInfoFail;
	public event Delegate EventContinentExitForTeamBattlefieldEnter;
	public event Delegate<Guid,PDVector3,float,PDHero[]> EventTeamBattlefieldEnter;
	public event Delegate<int> EventTeamBattlefieldExit;
	public event Delegate<bool, long, int> EventTeamBattlefieldAbandon;
	public event Delegate<float, PDVector3, float> EventTeamBattlefieldRevive;

	public event Delegate<PDTeamBattlefieldMember[]> EventTeamBattlefieldPlayWaitStart;
	public event Delegate EventTeamBattlefieldStart;
	public event Delegate EventTeamBattlefieldPointAcquisition;
	public event Delegate EventHeroTeamBattlefieldPointAcquisition;
	public event Delegate<int, int, bool, long> EventTeamBattlefieldClear;
	public event Delegate<int> EventTeamBattlefieldBanished;
	public event Delegate EventTeamBattlefieldServerBestRecordUpdated;

	#endregion Delegate.Event

	//---------------------------------------------------------------------------------------------------
	public bool Auto { get { return m_bAuto; } }
	public EnDungeonPlay DungeonPlay { get { return m_enDungeonPlay; } set { m_enDungeonPlay = value; } }
	public EnDungeonEnterType DungeonEnterType { get { return m_enDungeonEnterType; } }

	public int ArrangeNo { get { return m_nArrangeNo; } }
	public int InteractionObjectId { get { return m_nObjectId; } }
	public long InteractionInstanceId { get { return m_lInstanceId; } }
	public float InteractionDuration { get { return m_flInteractionDuration; } }
	public long BuffBoxInstanceId { get { return m_lBuffBoxInstanceId; } }

	public bool IsStateNone { get { return m_enInteractionState == EnInteractionState.None; } }
	public bool IsStateViewButton { get { return m_enInteractionState == EnInteractionState.ViewButton; } }
	public bool IsStateinteracting { get { return m_enInteractionState == EnInteractionState.interacting; } }

	public float MultiDungeonRemainingStartTime { get { return m_flMultiDungeonRemainingStartTime; } }
	public float MultiDungeonRemainingLimitTime { get { return m_flMultiDungeonRemainingLimitTime; } }

	// StoryDungeon
	public int StoryDungeonNo { get { return m_nStoryDungeonNo; } }
	public CsStoryDungeon StoryDungeon { get { return m_csStoryDungeon; } }
	public CsStoryDungeonStep StoryDungeonStep { get { return m_csStoryDungeonStep; } }

	// ExpDungeon
	public CsExpDungeon ExpDungeon { get { return CsGameData.Instance.ExpDungeon; } set { CsGameData.Instance.ExpDungeon = value; } }
	public CsExpDungeonDifficultyWave ExpDungeonDifficultyWave { get { return m_csExpDungeonDifficultyWave; } }

	// GoldDungeon
	public CsGoldDungeon GoldDungeon { get { return CsGameData.Instance.GoldDungeon; } set { CsGameData.Instance.GoldDungeon = value; } }
	public CsGoldDungeonDifficulty GoldDungeonDifficulty { get { return m_csGoldDungeonDifficulty; } }
	public CsGoldDungeonStep GoldDungeonStep { get { return m_csGoldDungeonStep; } }
	public CsGoldDungeonStepWave GoldDungeonStepWave { get { return m_csGoldDungeonStepWave; } }

	// UndergroundMaze
	public CsUndergroundMaze UndergroundMaze { get { return CsGameData.Instance.UndergroundMaze; } set { CsGameData.Instance.UndergroundMaze = value; } }
	public CsUndergroundMazeFloor UndergroundMazeFloor { get { return m_csUndergroundMazeFloor; } }

	//ArtifactRoom
	public CsArtifactRoom ArtifactRoom { get { return CsGameData.Instance.ArtifactRoom; } set { CsGameData.Instance.ArtifactRoom = value; } }
	public CsArtifactRoomFloor ArtifactRoomFloor { get { return m_csArtifactRoomFloor; } }

	// AncientRelic
	public float AncientRelicCurrentPoint { get { return m_flAncientRelicCurrentPoint; } }
	public float AncientRelicMatchingRemainingTime { get { return m_flAncientRelicMatchingRemainingTime; } }
	public CsAncientRelic AncientRelic { get { return CsGameData.Instance.AncientRelic; } set { CsGameData.Instance.AncientRelic = value; } }
	public CsAncientRelicStep AncientRelicStep { get { return m_csAncientRelicStep; } }
	public CsAncientRelicStepWave AncientRelicStepWave { get { return m_csAncientRelicStepWave; } }
	public EnDungeonMatchingState AncientRelicState { get { return m_enAncientRelicState; } }

	// FieldOfHonor
	public CsFieldOfHonor FieldOfHonor { get { return CsGameData.Instance.FieldOfHonor; } set { CsGameData.Instance.FieldOfHonor = value; } }
	public PDHero FieldOfHonorTartgetHero { get { return m_pDHeroFieldOfHonor; } set { m_pDHeroFieldOfHonor = value; } }

	//SoulCoveter
	public float SoulCoveterMatchingRemainingTime { get { return m_flSoulCoveterMatchingRemainingTime; } }
	public CsSoulCoveter SoulCoveter { get { return CsGameData.Instance.SoulCoveter; } set { CsGameData.Instance.SoulCoveter = value; } }
	public CsSoulCoveterDifficulty SoulCoveterDifficulty { get { return m_csSoulCoveterDifficulty; } }
	public CsSoulCoveterDifficultyWave SoulCoveterDifficultyWave { get { return m_csSoulCoveterDifficultyWave; } }
	public EnDungeonMatchingState SoulCoveterMatchingState { get { return m_enSoulCoveterMatchingState; } }

	// Elite
	public CsEliteDungeon EliteDungeon { get { return CsGameData.Instance.EliteDungeon; } set { CsGameData.Instance.EliteDungeon = value; } }
	public CsEliteMonster EliteMonster { get { return m_csEliteMonster; } }

	// ProofOfValor
	public CsProofOfValor ProofOfValor { get { return CsGameData.Instance.ProofOfValor; } set { CsGameData.Instance.ProofOfValor = value; } }

	// WisdomTemple
	public CsWisdomTemple WisdomTemple { get { return CsGameData.Instance.WisdomTemple; } set { CsGameData.Instance.WisdomTemple = value; } }
	public CsWisdomTempleStep WisdomTempleStep { get { return m_csWisdomTempleStep; } }
	public CsWisdomTemplePuzzle WisdomTemplePuzzle { get { return m_csWisdomTemplePuzzle; } }
	public EnWisdomTempleType WisdomTempleType { get { return m_enWisdomTempleType; } }

	// RuinsReclaim
	public float RuinsReclaimMatchingRemainingTime { get { return m_flRuinsReclaimMatchingRemainingTime; } }
	public CsRuinsReclaim RuinsReclaim { get { return CsGameData.Instance.RuinsReclaim; } set { CsGameData.Instance.RuinsReclaim = value; } }
	public CsRuinsReclaimStep RuinsReclaimStep { get { return m_csRuinsReclaimStep; } }
	public CsRuinsReclaimStepWave RuinsReclaimStepWave { get { return m_csRuinsReclaimStepWave; } }
	public EnDungeonMatchingState RuinsReclaimMatchingState { get { return m_enRuinsReclaimMatchingState; } }

	// InfiniteWar
	public CsInfiniteWar InfiniteWar { get { return CsGameData.Instance.InfiniteWar; } set { CsGameData.Instance.InfiniteWar = value; } }
	public EnDungeonMatchingState InfiniteWarMatchingState { get { return m_enInfiniteWarMatchingState; } }

	// FearAltar
	public float FearAltarMatchingRemainingTime { get { return m_flFearAltarMatchingRemainingTime; } }
	public CsFearAltar FearAltar { get { return CsGameData.Instance.FearAltar; } set { CsGameData.Instance.FearAltar = value; } }
	public CsFearAltarStage FearAltarStage { get { return m_csFearAltarStage; } }
	public CsFearAltarStageWave FearAltarStageWave { get { return m_csFearAltarStageWave; } }
	public EnDungeonMatchingState FearAltarMatchingState { get { return m_enFearAltarMatchingState; } }

	// WarMemory
	public CsWarMemory WarMemory { get { return CsGameData.Instance.WarMemory; } set { CsGameData.Instance.WarMemory = value; } }
	public CsWarMemoryWave WarMemoryWave { get { return m_csWarMemoryWave; } }
	public EnDungeonMatchingState WarMemoryMatchingState { get { return m_enWarMemoryMatchingState; } }

	// OsirisRoom
	public CsOsirisRoom OsirisRoom { get { return CsGameData.Instance.OsirisRoom; } set { CsGameData.Instance.OsirisRoom = value; } }
	public CsOsirisRoomDifficulty OsirisRoomDifficulty { get { return m_csOsirisRoomDifficulty; } }
	public CsOsirisRoomDifficultyWave OsirisRoomDifficultyWave { get { return m_csOsirisRoomDifficultyWave; } }

	// Biography
	public CsBiographyQuestDungeon BiographyQuestDungeon { get { return m_csBiographyQuestDungeon; } }
	public CsBiographyQuestDungeonWave BiographyQuestDungeonWave { get { return m_csBiographyQuestDungeonWave; } }

	// DragonNest
	public CsDragonNest DragonNest { get { return CsGameData.Instance.DragonNest; } set { CsGameData.Instance.DragonNest = value; } }
	public CsDragonNestStep DragonNestStep { get { return m_csDragonNestStep; } }
    public EnDungeonMatchingState DragonNestMatchingState { get { return m_enDragonNestMatchingState; } }
    public float DragonNestMatchingRemainingTime { get { return m_flDragonNestMatchingRemainingTime; } }

	// AnkouTomb
	public EnDungeonMatchingState AnkouTombMatchingState { get { return m_enAnkouTombMatchingState; } }
	public CsAnkouTomb AnkouTomb { get { return CsGameData.Instance.AnkouTomb; } set { CsGameData.Instance.AnkouTomb = value; } }
    public float AnkouTombMatchingRemainingTime { get { return m_flAnkouTombMatchingRemainingTime; } }
    public int AnkouTombPoint { get { return m_nAnkouTombPoint; } }

	// TradeShip
	public CsTradeShip TradeShip { get { return CsGameData.Instance.TradeShip; } set { CsGameData.Instance.TradeShip = value; } }
	public CsTradeShipStep TradeShipStep { get { return m_csTradeShipStep; } }
	public EnDungeonMatchingState TradeShipMatchingState { get { return m_enTradeShipMatchingState; } }
	public float TradeShipMatchingRemainingTime { get { return m_flTradeShipMatchingRemainingTime; } }
    public int TradeShipPoint { get { return m_nTradeShipPoint; } }

	// TeamBattlefield
	public CsTeamBattlefield TeamBattlefield { get { return CsGameData.Instance.TeamBattlefield; } set { CsGameData.Instance.TeamBattlefield = value; } }
	public float TeamBattlefieldRemainingEnterWaitingTime { get { return m_flTeamBattlefieldRemainingEnterWaitingTime; } }
	public int TeamBattlefieldKillcount { get { return m_nTeamBattlefieldKillcount; } }
	public int TeamBattlefieldPoint { get { return m_nTeamBattlefieldPoint; } }
	public EnBattlefieldState BattlefieldState { get { return m_enBattlefieldState; } }
	//---------------------------------------------------------------------------------------------------
	public CsDungeonManager()
	{
		// StoryDungeon
		CsRplzSession.Instance.EventResContinentExitForStoryDungeonEnter += OnEventResContinentExitForStoryDungeonEnter;
		CsRplzSession.Instance.EventResStoryDungeonEnter += OnEventResStoryDungeonEnter;
		CsRplzSession.Instance.EventResStoryDungeonAbandon += OnEventResStoryDungeonAbandon;
		CsRplzSession.Instance.EventResStoryDungeonExit += OnEventResStoryDungeonExit;
		CsRplzSession.Instance.EventResStoryDungeonRevive += OnEventResStoryDungeonRevive;
		CsRplzSession.Instance.EventResStoryDungeonSweep += OnEventResStoryDungeonSweep;
		CsRplzSession.Instance.EventResStoryDungeonMonsterTame += OnEventResStoryDungeonMonsterTame;

		CsRplzSession.Instance.EventEvtStoryDungeonStepStart += OnEventEvtStoryDungeonStepStart;
		CsRplzSession.Instance.EventEvtStoryDungeonClear += OnEventEvtStoryDungeonClear;
		CsRplzSession.Instance.EventEvtStoryDungeonFail += OnEventEvtStoryDungeonFail;
		CsRplzSession.Instance.EventEvtStoryDungeonBanished += OnEventEvtStoryDungeonBanished;
		CsRplzSession.Instance.EventEvtStoryDungeonTrapCast += OnEventEvtStoryDungeonTrapCast;
		CsRplzSession.Instance.EventEvtStoryDungeonTrapHit += OnEventStoryDungeonTrapHit;

		// ExpDungeon
		CsRplzSession.Instance.EventResContinentExitForExpDungeonEnter += OnEventResContinentExitForExpDungeonEnter;
		CsRplzSession.Instance.EventResExpDungeonEnter += OnEventResExpDungeonEnter;
		CsRplzSession.Instance.EventResExpDungeonAbandon += OnEventResExpDungeonAbandon;
		CsRplzSession.Instance.EventResExpDungeonExit += OnEventResExpDungeonExit;
		CsRplzSession.Instance.EventResExpDungeonRevive += OnEventResExpDungeonRevive;
		CsRplzSession.Instance.EventResExpDungeonSweep += OnEventResExpDungeonSweep;

		CsRplzSession.Instance.EventEvtExpDungeonWaveStart += OnEventEvtExpDungeonWaveStart;
		CsRplzSession.Instance.EventEvtExpDungeonWaveCompleted += OnEventEvtExpDungeonWaveCompleted;
		CsRplzSession.Instance.EventEvtExpDungeonWaveTimeout += OnEventEvtExpDungeonWaveTimeout;
		CsRplzSession.Instance.EventEvtExpDungeonClear += OnEventEvtExpDungeonClear;
		CsRplzSession.Instance.EventEvtExpDungeonBanished += OnEventEvtExpDungeonBanished;

		// GoldDungeon
		CsRplzSession.Instance.EventResContinentExitForGoldDungeonEnter += OnEventResContinentExitForGoldDungeonEnter;
		CsRplzSession.Instance.EventResGoldDungeonEnter += OnEventResGoldDungeonEnter;
		CsRplzSession.Instance.EventResGoldDungeonAbandon += OnEventResGoldDungeonAbandon;
		CsRplzSession.Instance.EventResGoldDungeonExit += OnEventResGoldDungeonExit;
		CsRplzSession.Instance.EventResGoldDungeonRevive += OnEventResGoldDungeonRevive;
		CsRplzSession.Instance.EventResGoldDungeonSweep += OnEventResGoldDungeonSweep;

		CsRplzSession.Instance.EventEvtGoldDungeonStepStart += OnEventEvtGoldDungeonStepStart;
		CsRplzSession.Instance.EventEvtGoldDungeonStepCompleted += OnEventEvtGoldDungeonStepCompleted;
		CsRplzSession.Instance.EventEvtGoldDungeonWaveStart += OnEventEvtGoldDungeonWaveStart;
		CsRplzSession.Instance.EventEvtGoldDungeonWaveCompleted += OnEventEvtGoldDungeonWaveCompleted;
		CsRplzSession.Instance.EventEvtGoldDungeonWaveTimeout += OnEventEvtGoldDungeonWaveTimeout;
		CsRplzSession.Instance.EventEvtGoldDungeonClear += OnEventEvtGoldDungeonClear;
		CsRplzSession.Instance.EventEvtGoldDungeonFail += OnEventEvtGoldDungeonFail;
		CsRplzSession.Instance.EventEvtGoldDungeonBanished += OnEventEvtGoldDungeonBanished;

		// UndergroundMaze
		CsRplzSession.Instance.EventResContinentExitForUndergroundMazeEnter += OnEventResContinentExitForUndergroundMazeEnter;
		CsRplzSession.Instance.EventResUndergroundMazeEnter += OnEventResUndergroundMazeEnter;
		CsRplzSession.Instance.EventResUndergroundMazeExit += OnEventResUndergroundMazeExit;
		CsRplzSession.Instance.EventResUndergroundMazeRevive += OnEventResUndergroundMazeRevive;
		CsRplzSession.Instance.EventResUndergroundMazeEnterForUndergroundMazeRevive += OnEventResUndergroundMazeEnterForUndergroundMazeRevive;
		CsRplzSession.Instance.EventResUndergroundMazePortalEnter += OnEventResUndergroundMazePortalEnter;
		CsRplzSession.Instance.EventResUndergroundMazePortalExit += OnEventResUndergroundMazePortalExit;
		CsRplzSession.Instance.EventResUndergroundMazeTransmission += OnEventResUndergroundMazeTransmission;
		CsRplzSession.Instance.EventResUndergroundMazeEnterForTransmission += OnEventResUndergroundMazeEnterForTransmission;
		CsRplzSession.Instance.EventEvtUndergroundMazeBanished += OnEventEvtUndergroundMazeBanished;

		// ArtifactRoom
		CsRplzSession.Instance.EventResContinentExitForArtifactRoomEnter += OnEventResContinentExitForArtifactRoomEnter;
		CsRplzSession.Instance.EventResArtifactRoomEnter += OnEventResArtifactRoomEnter;
		CsRplzSession.Instance.EventResArtifactRoomExit += OnEventResArtifactRoomExit;
		CsRplzSession.Instance.EventResArtifactRoomAbandon += OnEventResArtifactRoomAbandon;
		CsRplzSession.Instance.EventResArtifactRoomNextFloorChallenge += OnEventResArtifactRoomNextFloorChallenge;
		CsRplzSession.Instance.EventResArtifactRoomInit += OnEventResArtifactRoomInit;
		CsRplzSession.Instance.EventResArtifactRoomSweep += OnEventResArtifactRoomSweep;
		CsRplzSession.Instance.EventResArtifactRoomSweepAccelerate += OnEventResArtifactRoomSweepAccelerate;
		CsRplzSession.Instance.EventResArtifactRoomSweepComplete += OnEventResArtifactRoomSweepComplete;
		CsRplzSession.Instance.EventResArtifactRoomSweepStop += OnEventResArtifactRoomSweepStop;

		CsRplzSession.Instance.EventEvtArtifactRoomStart += OnEventEvtArtifactRoomStart;
		CsRplzSession.Instance.EventEvtArtifactRoomClear += OnEventEvtArtifactRoomClear;
		CsRplzSession.Instance.EventEvtArtifactRoomFail += OnEventEvtArtifactRoomFail;
		CsRplzSession.Instance.EventEvtArtifactRoomBanished += OnEventEvtArtifactRoomBanished;
		CsRplzSession.Instance.EventEvtArtifactRoomBanishedForNextFloorChallenge += OnEventEvtArtifactRoomBanishedForNextFloorChallenge;
		CsRplzSession.Instance.EventEvtArtifactRoomSweepNextFloorStart += OnEventEvtArtifactRoomSweepNextFloorStart;
		CsRplzSession.Instance.EventEvtArtifactRoomSweepCompleted += OnEventEvtArtifactRoomSweepCompleted;

		// AncientRelic
		CsRplzSession.Instance.EventResAncientRelicMatchingStart += OnEventResAncientRelicMatchingStart;
		CsRplzSession.Instance.EventResAncientRelicMatchingCancel += OnEventResAncientRelicMatchingCancel;
		CsRplzSession.Instance.EventResAncientRelicEnter += OnEventResAncientRelicEnter;
		CsRplzSession.Instance.EventResAncientRelicExit += OnEventResAncientRelicExit;
		CsRplzSession.Instance.EventResAncientRelicAbandon += OnEventResAncientRelicAbandon;
		CsRplzSession.Instance.EventResAncientRelicRevive += OnEventResAncientRelicRevive;

		CsRplzSession.Instance.EventEvtAncientRelicMatchingStatusChanged += OnEventEvtAncientRelicMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtAncientRelicMatchingRoomBanished += OnEventEvtAncientRelicMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtContinentExitForAncientRelicEnter += OnEventEvtContinentExitForAncientRelicEnter;
		CsRplzSession.Instance.EventEvtAncientRelicStepStart += OnEventEvtAncientRelicStepStart;
		CsRplzSession.Instance.EventEvtAncientRelicStepCompleted += OnEventEvtAncientRelicStepCompleted;
		CsRplzSession.Instance.EventEvtAncientRelicWaveStart += OnEventEvtAncientRelicWaveStart;
		CsRplzSession.Instance.EventEvtAncientRelicClear += OnEventEvtAncientRelicClear;
		CsRplzSession.Instance.EventEvtAncientRelicFail += OnEventEvtAncientRelicFail;
		CsRplzSession.Instance.EventEvtAncientRelicBanished += OnEventEvtAncientRelicBanished;
		CsRplzSession.Instance.EventEvtAncientRelicPointUpdated += OnEventEvtAncientRelicPointUpdated;
		CsRplzSession.Instance.EventEvtAncientRelicTrapActivated += OnEventEvtAncientRelicTrapActivated;
		CsRplzSession.Instance.EventEvtAncientRelicTrapHit += OnEventEvtAncientRelicTrapHit;
		CsRplzSession.Instance.EventEvtAncientRelicTrapEffectFinished += OnEventEvtAncientRelicTrapEffectFinished;
		CsRplzSession.Instance.EventEvtAncientRelicMatchingRoomPartyEnter += OnEventEvtAncientRelicMatchingRoomPartyEnter;

		// FieldOfHonor
		CsRplzSession.Instance.EventResFieldOfHonorInfo += OnEventResFieldOfHonorInfo;
		CsRplzSession.Instance.EventResFieldOfHonorTopRankingList += OnEventResFieldOfHonorTopRankingList;
		CsRplzSession.Instance.EventResContinentExitForFieldOfHonorChallenge += OnEventResContinentExitForFieldOfHonorChallenge;
		CsRplzSession.Instance.EventResFieldOfHonorChallenge += OnEventResFieldOfHonorChallenge;
		CsRplzSession.Instance.EventResFieldOfHonorExit += OnEventResFieldOfHonorExit;
		CsRplzSession.Instance.EventResFieldOfHonorAbandon += OnEventResFieldOfHonorAbandon;
		CsRplzSession.Instance.EventResFieldOfHonorRankingRewardReceive += OnEventResFieldOfHonorRankingRewardReceive;
		CsRplzSession.Instance.EventResFieldOfHonorRankerInfo += OnEventResFieldOfHonorRankerInfo;

		CsRplzSession.Instance.EventEvtFieldOfHonorStart += OnEventEvtFieldOfHonorStart;
		CsRplzSession.Instance.EventEvtFieldOfHonorClear += OnEventEvtFieldOfHonorClear;
		CsRplzSession.Instance.EventEvtFieldOfHonorFail += OnEventEvtFieldOfHonorFail;
		CsRplzSession.Instance.EventEvtFieldOfHonorBanished += OnEventEvtFieldOfHonorBanished;
		CsRplzSession.Instance.EventEvtFieldOfHonorDailyRankingUpdated += OnEventEvtFieldOfHonorDailyRankingUpdated;

		//SoulCoveter
		CsRplzSession.Instance.EventResSoulCoveterMatchingStart += OnEventResSoulCoveterMatchingStart;
		CsRplzSession.Instance.EventResSoulCoveterMatchingCancel += OnEventResSoulCoveterMatchingCancel;
		CsRplzSession.Instance.EventResSoulCoveterEnter += OnEventResSoulCoveterEnter;
		CsRplzSession.Instance.EventResSoulCoveterExit += OnEventResSoulCoveterExit;
		CsRplzSession.Instance.EventResSoulCoveterAbandon += OnEventResSoulCoveterAbandon;
		CsRplzSession.Instance.EventResSoulCoveterRevive += OnEventResSoulCoveterRevive;

		CsRplzSession.Instance.EventEvtSoulCoveterMatchingStatusChanged += OnEventEvtSoulCoveterMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtSoulCoveterMatchingRoomBanished += OnEventEvtSoulCoveterMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtSoulCoveterMatchingRoomPartyEnter += OnEventEvtSoulCoveterMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtContinentExitForSoulCoveterEnter += OnEventEvtContinentExitForSoulCoveterEnter;
		CsRplzSession.Instance.EventEvtSoulCoveterWaveStart += OnEventEvtSoulCoveterWaveStart;
		CsRplzSession.Instance.EventEvtSoulCoveterClear += OnEventEvtSoulCoveterClear;
		CsRplzSession.Instance.EventEvtSoulCoveterFail += OnEventEvtSoulCoveterFail;
		CsRplzSession.Instance.EventEvtSoulCoveterBanished += OnEventEvtSoulCoveterBanished;

		// Elite
		CsRplzSession.Instance.EventResContinentExitForEliteDungeonEnter += OnEventResContinentExitForEliteDungeonEnter;
		CsRplzSession.Instance.EventResEliteDungeonEnter += OnEventResEliteDungeonEnter;
		CsRplzSession.Instance.EventResEliteDungeonExit += OnEventResEliteDungeonExit;
		CsRplzSession.Instance.EventResEliteDungeonAbandon += OnEventResEliteDungeonAbandon;
		CsRplzSession.Instance.EventResEliteDungeonRevive += OnEventResEliteDungeonRevive;

		CsRplzSession.Instance.EventEvtEliteDungeonStart += OnEventEvtEliteDungeonStart;
		CsRplzSession.Instance.EventEvtEliteDungeonClear += OnEventEvtEliteDungeonClear;
		CsRplzSession.Instance.EventEvtEliteDungeonFail += OnEventEvtEliteDungeonFail;
		CsRplzSession.Instance.EventEvtEliteDungeonBanished += OnEventEvtEliteDungeonBanished;

		// ProofOfValor
		CsRplzSession.Instance.EventResContinentExitForProofOfValorEnter += OnEventResContinentExitForProofOfValorEnter;
		CsRplzSession.Instance.EventResProofOfValorEnter += OnEventResProofOfValorEnter;
		CsRplzSession.Instance.EventResProofOfValorExit += OnEventResProofOfValorExit;
		CsRplzSession.Instance.EventResProofOfValorAbandon += OnEventResProofOfValorAbandon;
		CsRplzSession.Instance.EventResProofOfValorSweep += OnEventResProofOfValorSweep;
		CsRplzSession.Instance.EventResProofOfValorRefresh += OnEventResProofOfValorRefresh;
		CsRplzSession.Instance.EventResProofOfValorBuffBoxAcquire += OnEventResProofOfValorBuffBoxAcquire;

		CsRplzSession.Instance.EventEvtProofOfValorStart += OnEventEvtProofOfValorStart;
		CsRplzSession.Instance.EventEvtProofOfValorClear += OnEventEvtProofOfValorClear;
		CsRplzSession.Instance.EventEvtProofOfValorFail += OnEventEvtProofOfValorFail;
		CsRplzSession.Instance.EventEvtProofOfValorBanished += OnEventEvtProofOfValorBanished;
		CsRplzSession.Instance.EventEvtProofOfValorBuffBoxCreated += OnEventEvtProofOfValorBuffBoxCreated;
		CsRplzSession.Instance.EventEvtProofOfValorBuffBoxLifetimeEnded += OnEventEvtProofOfValorBuffBoxLifetimeEnded;
		CsRplzSession.Instance.EventEvtProofOfValorBuffFinished += OnEventEvtProofOfValorBuffFinished;
		CsRplzSession.Instance.EventEvtProofOfValorRefreshed += OnEventEvtProofOfValorRefreshed;

		// WisdomTemple
		CsRplzSession.Instance.EventResContinentExitForWisdomTempleEnter += OnEventResContinentExitForWisdomTempleEnter;
		CsRplzSession.Instance.EventResWisdomTempleEnter += OnEventResWisdomTempleEnter;
		CsRplzSession.Instance.EventResWisdomTempleExit += OnEventResWisdomTempleExit;
		CsRplzSession.Instance.EventResWisdomTempleAbandon += OnEventResWisdomTempleAbandon;
		CsRplzSession.Instance.EventResWisdomTempleColorMatchingObjectInteractionStart += OnEventResWisdomTempleColorMatchingObjectInteractionStart;
		CsRplzSession.Instance.EventResWisdomTempleColorMatchingObjectCheck += OnEventResWisdomTempleColorMatchingObjectCheck;
		CsRplzSession.Instance.EventResWisdomTemplePuzzleRewardObjectInteractionStart += OnEventResWisdomTemplePuzzleRewardObjectInteractionStart;
		CsRplzSession.Instance.EventResWisdomTempleSweep += OnEventResWisdomTempleSweep;

		CsRplzSession.Instance.EventEvtWisdomTempleStepStart += OnEventEvtWisdomTempleStepStart;
		CsRplzSession.Instance.EventEvtWisdomTempleStepCompleted += OnEventEvtWisdomTempleStepCompleted;
		CsRplzSession.Instance.EventEvtWisdomTempleColorMatchingObjectInteractionFinished += OnEventEvtWisdomTempleColorMatchingObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtWisdomTempleColorMatchingObjectInteractionCancel += OnEventEvtWisdomTempleColorMatchingObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtWisdomTempleColorMatchingMonsterCreated += OnEventEvtWisdomTempleColorMatchingMonsterCreated;
		CsRplzSession.Instance.EventEvtWisdomTempleColorMatchingMonsterKill += OnEventEvtWisdomTempleColorMatchingMonsterKill;
		CsRplzSession.Instance.EventEvtWisdomTempleFakeTreasureBoxKill += OnEventEvtWisdomTempleFakeTreasureBoxKill;
		CsRplzSession.Instance.EventEvtWisdomTemplePuzzleCompleted += OnEventEvtWisdomTemplePuzzleCompleted;
		CsRplzSession.Instance.EventEvtWisdomTemplePuzzleRewardObjectInteractionFinished += OnEventEvtWisdomTemplePuzzleRewardObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtWisdomTemplePuzzleRewardObjectInteractionCancel += OnEventEvtWisdomTemplePuzzleRewardObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtWisdomTempleQuizFail += OnEventEvtWisdomTempleQuizFail;
		CsRplzSession.Instance.EventEvtWisdomTempleBossMonsterCreated += OnEventEvtWisdomTempleBossMonsterCreated;
		CsRplzSession.Instance.EventEvtWisdomTempleBossMonsterKill += OnEventEvtWisdomTempleBossMonsterKill;
		CsRplzSession.Instance.EventEvtWisdomTempleClear += OnEventEvtWisdomTempleClear;
		CsRplzSession.Instance.EventEvtWisdomTempleFail += OnEventEvtWisdomTempleFail;
		CsRplzSession.Instance.EventEvtWisdomTempleBanished += OnEventEvtWisdomTempleBanished;

		// RuinsReclaim
		CsRplzSession.Instance.EventResRuinsReclaimMatchingStart += OnEventResRuinsReclaimMatchingStart;
		CsRplzSession.Instance.EventResRuinsReclaimMatchingCancel += OnEventResRuinsReclaimMatchingCancel;
		CsRplzSession.Instance.EventResRuinsReclaimEnter += OnEventResRuinsReclaimEnter;
		CsRplzSession.Instance.EventResRuinsReclaimExit += OnEventResRuinsReclaimExit;
		CsRplzSession.Instance.EventResRuinsReclaimAbandon += OnEventResRuinsReclaimAbandon;
		CsRplzSession.Instance.EventResRuinsReclaimPortalEnter += OnEventResRuinsReclaimPortalEnter;
		CsRplzSession.Instance.EventResRuinsReclaimRevive += OnEventResRuinsReclaimRevive;
		CsRplzSession.Instance.EventResRuinsReclaimMonsterTransformationCancelObjectInteractionStart += OnEventResRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
		CsRplzSession.Instance.EventResRuinsReclaimRewardObjectInteractionStart += OnEventResRuinsReclaimRewardObjectInteractionStart;

		CsRplzSession.Instance.EventEvtContinentExitForRuinsReclaimEnter += OnEventEvtContinentExitForRuinsReclaimEnter;
		CsRplzSession.Instance.EventEvtRuinsReclaimMatchingRoomPartyEnter += OnEventEvtRuinsReclaimMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtRuinsReclaimMatchingStatusChanged += OnEventEvtRuinsReclaimMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtRuinsReclaimMatchingRoomBanished += OnEventEvtRuinsReclaimMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtRuinsReclaimStepStart += OnEventEvtRuinsReclaimStepStart;
		CsRplzSession.Instance.EventEvtRuinsReclaimStepCompleted += OnEventEvtRuinsReclaimStepCompleted;
		CsRplzSession.Instance.EventEvtRuinsReclaimRewardObjectInteractionCancel += OnEventEvtRuinsReclaimRewardObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtRuinsReclaimRewardObjectInteractionFinished += OnEventEvtRuinsReclaimRewardObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimRewardObjectInteractionStart += OnEventEvtHeroRuinsReclaimRewardObjectInteractionStart;
		CsRplzSession.Instance.EventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel += OnEventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimRewardObjectInteractionFinished += OnEventEvtHeroRuinsReclaimRewardObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtRuinsReclaimWaveStart += OnEventEvtRuinsReclaimWaveStart;
		CsRplzSession.Instance.EventEvtRuinsReclaimWaveCompleted += OnEventEvtRuinsReclaimWaveCompleted;
		CsRplzSession.Instance.EventEvtRuinsReclaimStepWaveSkillCast += OnEventEvtRuinsReclaimStepWaveSkillCast;
	
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterTransformationStart += OnEventEvtRuinsReclaimMonsterTransformationStart;
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterTransformationFinished += OnEventEvtRuinsReclaimMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded += OnEventEvtRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimMonsterTransformationStart += OnEventEvtHeroRuinsReclaimMonsterTransformationStart;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimMonsterTransformationFinished += OnEventEvtHeroRuinsReclaimMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionCancel += OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart += OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel += OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		CsRplzSession.Instance.EventEvtRuinsReclaimMonsterSummon += OnEventEvtRuinsReclaimMonsterSummon;
		CsRplzSession.Instance.EventEvtRuinsReclaimTrapHit += OnEventEvtRuinsReclaimTrapHit;
		CsRplzSession.Instance.EventEvtRuinsReclaimDebuffEffectStart += OnEventEvtRuinsReclaimDebuffEffectStart;
		CsRplzSession.Instance.EventEvtRuinsReclaimDebuffEffectStop += OnEventEvtRuinsReclaimDebuffEffectStop;
		CsRplzSession.Instance.EventEvtHeroRuinsReclaimPortalEnter += OnEventEvtHeroRuinsReclaimPortalEnter;
		CsRplzSession.Instance.EventEvtRuinsReclaimClear += OnEventEvtRuinsReclaimClear;
		CsRplzSession.Instance.EventEvtRuinsReclaimFail += OnEventEvtRuinsReclaimFail;
		CsRplzSession.Instance.EventEvtRuinsReclaimBanished += OnEventEvtRuinsReclaimBanished;

		//InfiniteWar
		CsRplzSession.Instance.EventResInfiniteWarMatchingStart += OnEventResInfiniteWarMatchingStart;
		CsRplzSession.Instance.EventResInfiniteWarMatchingCancel += OnEventResInfiniteWarMatchingCancel;
		CsRplzSession.Instance.EventResInfiniteWarEnter += OnEventResInfiniteWarEnter;
		CsRplzSession.Instance.EventResInfiniteWarExit += OnEventResInfiniteWarExit;
		CsRplzSession.Instance.EventResInfiniteWarAbandon += OnEventResInfiniteWarAbandon;
		CsRplzSession.Instance.EventResInfiniteWarRevive += OnEventResInfiniteWarRevive;
		CsRplzSession.Instance.EventResInfiniteWarBuffBoxAcquire += OnEventResInfiniteWarBuffBoxAcquire;

		CsRplzSession.Instance.EventEvtContinentExitForInfiniteWarEnter += OnEventEvtContinentExitForInfiniteWarEnter;
		CsRplzSession.Instance.EventEvtInfiniteWarMatchingStatusChanged += OnEventEvtInfiniteWarMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtInfiniteWarMatchingRoomBanished += OnEventEvtInfiniteWarMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtInfiniteWarStart += OnEventEvtInfiniteWarStart;
		CsRplzSession.Instance.EventEvtInfiniteWarMonsterSpawn += OnEventEvtInfiniteWarMonsterSpawn;
		CsRplzSession.Instance.EventEvtInfiniteWarBuffBoxCreated += OnEventEvtInfiniteWarBuffBoxCreated;
		CsRplzSession.Instance.EventEvtInfiniteWarBuffBoxLifetimeEnded += OnEventEvtInfiniteWarBuffBoxLifetimeEnded;
		CsRplzSession.Instance.EventEvtHeroInfiniteWarBuffBoxAcquisition += OnEventEvtHeroInfiniteWarBuffBoxAcquisition;
		CsRplzSession.Instance.EventEvtInfiniteWarBuffFinished += OnEventEvtInfiniteWarBuffFinished;
		CsRplzSession.Instance.EventEvtInfiniteWarPointAcquisition += OnEventEvtInfiniteWarPointAcquisition;
		CsRplzSession.Instance.EventEvtHeroInfiniteWarPointAcquisition += OnEventEvtHeroInfiniteWarPointAcquisition;
		CsRplzSession.Instance.EventEvtInfiniteWarClear += OnEventEvtInfiniteWarClear;
		CsRplzSession.Instance.EventEvtInfiniteWarBanished += OnEventEvtInfiniteWarBanished;

		// FearAltar
		CsRplzSession.Instance.EventResFearAltarMatchingStart += OnEventResFearAltarMatchingStart;
		CsRplzSession.Instance.EventResFearAltarMatchingCancel += OnEventResFearAltarMatchingCancel;
		CsRplzSession.Instance.EventResFearAltarEnter += OnEventResFearAltarEnter;
		CsRplzSession.Instance.EventResFearAltarExit += OnEventResFearAltarExit;
		CsRplzSession.Instance.EventResFearAltarAbandon += OnEventResFearAltarAbandon;
		CsRplzSession.Instance.EventResFearAltarRevive += OnEventResFearAltarRevive;

		CsRplzSession.Instance.EventEvtContinentExitForFearAltarEnter += OnEventEvtContinentExitForFearAltarEnter;
		CsRplzSession.Instance.EventEvtFearAltarMatchingRoomPartyEnter += OnEventEvtFearAltarMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtFearAltarMatchingRoomBanished += OnEventEvtFearAltarMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtFearAltarMatchingStatusChanged += OnEventEvtFearAltarMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtFearAltarWaveStart += OnEventEvtFearAltarWaveStart;
		CsRplzSession.Instance.EventEvtFearAltarClear += OnEventEvtFearAltarClear;
		CsRplzSession.Instance.EventEvtFearAltarFail += OnEventEvtFearAltarFail;
		CsRplzSession.Instance.EventEvtFearAltarBanished += OnEventEvtFearAltarBanished;

		// WarMemory
		CsRplzSession.Instance.EventResWarMemoryMatchingStart += OnEventResWarMemoryMatchingStart;
        CsRplzSession.Instance.EventResWarMemoryMatchingCancel += OnEventResWarMemoryMatchingCancel;
        CsRplzSession.Instance.EventResWarMemoryEnter += OnEventResWarMemoryEnter;
        CsRplzSession.Instance.EventResWarMemoryExit += OnEventResWarMemoryExit;
        CsRplzSession.Instance.EventResWarMemoryAbandon += OnEventResWarMemoryAbandon;
        CsRplzSession.Instance.EventResWarMemoryRevive += OnEventResWarMemoryRevive;
        CsRplzSession.Instance.EventResWarMemoryTransformationObjectInteractionStart += OnEventResWarMemoryTransformationObjectInteractionStart;

        CsRplzSession.Instance.EventEvtContinentExitForWarMemoryEnter += OnEventEvtContinentExitForWarMemoryEnter;
        CsRplzSession.Instance.EventEvtWarMemoryMatchingRoomPartyEnter += OnEventEvtWarMemoryMatchingRoomPartyEnter;
        CsRplzSession.Instance.EventEvtWarMemoryMatchingRoomBanished += OnEventEvtWarMemoryMatchingRoomBanished;
        CsRplzSession.Instance.EventEvtWarMemoryMatchingStatusChanged += OnEventEvtWarMemoryMatchingStatusChanged;
        CsRplzSession.Instance.EventEvtWarMemoryWaveStart += OnEventEvtWarMemoryWaveStart;
        CsRplzSession.Instance.EventEvtWarMemoryWaveCompleted += OnEventEvtWarMemoryWaveCompleted;
        CsRplzSession.Instance.EventEvtWarMemoryTransformationObjectLifetimeEnded += OnEventEvtWarMemoryTransformationObjectLifetimeEnded;
        CsRplzSession.Instance.EventEvtWarMemoryTransformationObjectInteractionCancel += OnEventEvtWarMemoryTransformationObjectInteractionCancel;
        CsRplzSession.Instance.EventEvtWarMemoryTransformationObjectInteractionFinished += OnEventEvtWarMemoryTransformationObjectInteractionFinished;
        CsRplzSession.Instance.EventEvtHeroWarMemoryTransformationObjectInteractionStart += OnEventEvtHeroWarMemoryTransformationObjectInteractionStart;
        CsRplzSession.Instance.EventEvtHeroWarMemoryTransformationObjectInteractionCancel += OnEventEvtHeroWarMemoryTransformationObjectInteractionCancel;
        CsRplzSession.Instance.EventEvtHeroWarMemoryTransformationObjectInteractionFinished += OnEventEvtHeroWarMemoryTransformationObjectInteractionFinished;
        CsRplzSession.Instance.EventEvtWarMemoryMonsterTransformationCancel += OnEventEvtWarMemoryMonsterTransformationCancel;
        CsRplzSession.Instance.EventEvtWarMemoryMonsterTransformationFinished += OnEventEvtWarMemoryMonsterTransformationFinished;
        CsRplzSession.Instance.EventEvtHeroWarMemoryMonsterTransformationCancel += OnEventEvtHeroWarMemoryMonsterTransformationCancel;
        CsRplzSession.Instance.EventEvtHeroWarMemoryMonsterTransformationFinished += OnEventEvtHeroWarMemoryMonsterTransformationFinished;
        CsRplzSession.Instance.EventEvtWarMemoryMonsterSummon += OnEventEvtWarMemoryMonsterSummon;
        CsRplzSession.Instance.EventEvtWarMemoryPointAcquisition += OnEventEvtWarMemoryPointAcquisition;
        CsRplzSession.Instance.EventEvtHeroWarMemoryPointAcquisition += OnEventEvtHeroWarMemoryPointAcquisition;
        CsRplzSession.Instance.EventEvtHeroWarMemoryTransformationMonsterSkillCast += OnEventEvtHeroWarMemoryTransformationMonsterSkillCast;
        CsRplzSession.Instance.EventEvtWarMemoryClear += OnEventEvtWarMemoryClear;
        CsRplzSession.Instance.EventEvtWarMemoryFail += OnEventEvtWarMemoryFail;
        CsRplzSession.Instance.EventEvtWarMemoryBanished += OnEventEvtWarMemoryBanished;

		// OsirisRoom
		CsRplzSession.Instance.EventResContinentExitForOsirisRoomEnter += OnEventResContinentExitForOsirisRoomEnter;
		CsRplzSession.Instance.EventResOsirisRoomEnter += OnEventResOsirisRoomEnter;
		CsRplzSession.Instance.EventResOsirisRoomExit += OnEventResOsirisRoomExit;
		CsRplzSession.Instance.EventResOsirisRoomAbandon += OnEventResOsirisRoomAbandon;
		CsRplzSession.Instance.EventResOsirisRoomMoneyBuffActivate += OnEventResOsirisRoomMoneyBuffActivate;

		CsRplzSession.Instance.EventEvtOsirisRoomWaveStart += OnEventEvtOsirisRoomWaveStart;
		CsRplzSession.Instance.EventEvtOsirisRoomMonsterSpawn += OnEventEvtOsirisRoomMonsterSpawn;
		CsRplzSession.Instance.EventEvtOsirisRoomRewardGoldAcquisition += OnEventEvtOsirisRoomRewardGoldAcquisition;
		CsRplzSession.Instance.EventEvtOsirisRoomMoneyBuffFinished += OnEventEvtOsirisRoomMoneyBuffFinished;
		CsRplzSession.Instance.EventEvtOsirisRoomMoneyBuffCancel += OnEventEvtOsirisRoomMoneyBuffCancel;
		CsRplzSession.Instance.EventEvtOsirisRoomClear += OnEventEvtOsirisRoomClear;
		CsRplzSession.Instance.EventEvtOsirisRoomFail += OnEventEvtOsirisRoomFail;
		CsRplzSession.Instance.EventEvtOsirisRoomBanished += OnEventEvtOsirisRoomBanished;

		// Biography
		CsRplzSession.Instance.EventResContinentExitForBiographyQuestDungeonEnter += OnEventResContinentExitForBiographyQuestDungeonEnter;
		CsRplzSession.Instance.EventResBiographyQuestDungeonEnter += OnEventResBiographyQuestDungeonEnter;
		CsRplzSession.Instance.EventResBiographyQuestDungeonExit += OnEventResBiographyQuestDungeonExit;
		CsRplzSession.Instance.EventResBiographyQuestDungeonAbandon += OnEventResBiographyQuestDungeonAbandon;
		CsRplzSession.Instance.EventResBiographyQuestDungeonRevive += OnEventResBiographyQuestDungeonRevive;

		CsRplzSession.Instance.EventEvtBiographyQuestDungeonWaveStart += OnEventEvtBiographyQuestDungeonWaveStart;
		CsRplzSession.Instance.EventEvtBiographyQuestDungeonWaveCompleted += OnEventEvtBiographyQuestDungeonWaveCompleted;
		CsRplzSession.Instance.EventEvtBiographyQuestDungeonFail += OnEventEvtBiographyQuestDungeonFail;
		CsRplzSession.Instance.EventEvtBiographyQuestDungeonClear += OnEventEvtBiographyQuestDungeonClear;
		CsRplzSession.Instance.EventEvtBiographyQuestDungeonBanished += OnEventEvtBiographyQuestDungeonBanished;

		// DragonNest
		CsRplzSession.Instance.EventResDragonNestMatchingStart += OnEventResDragonNestMatchingStart;
		CsRplzSession.Instance.EventResDragonNestMatchingCancel += OnEventResDragonNestMatchingCancel;
		CsRplzSession.Instance.EventResDragonNestEnter += OnEventResDragonNestEnter;
		CsRplzSession.Instance.EventResDragonNestExit += OnEventResDragonNestExit;
		CsRplzSession.Instance.EventResDragonNestAbandon += OnEventResDragonNestAbandon;
		CsRplzSession.Instance.EventResDragonNestRevive += OnEventResDragonNestRevive;

		CsRplzSession.Instance.EventEvtContinentExitForDragonNestEnter +=OnEventEvtContinentExitForDragonNestEnter;
		CsRplzSession.Instance.EventEvtDragonNestMatchingRoomPartyEnter+=OnEventEvtDragonNestMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtDragonNestMatchingRoomBanished+=OnEventEvtDragonNestMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtDragonNestMatchingStatusChanged+=OnEventEvtDragonNestMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtDragonNestStepStart+=OnEventEvtDragonNestStepStart;
		CsRplzSession.Instance.EventEvtDragonNestStepCompleted += OnEventEvtDragonNestStepCompleted;
		CsRplzSession.Instance.EventEvtHeroDragonNestTrapHit+=OnEventEvtHeroDragonNestTrapHit;
		CsRplzSession.Instance.EventEvtDragonNestTrapEffectFinished+=OnEventEvtDragonNestTrapEffectFinished;
		CsRplzSession.Instance.EventEvtHeroDragonNestTrapEffectFinished+=OnEventEvtHeroDragonNestTrapEffectFinished;
		CsRplzSession.Instance.EventEvtDragonNestClear+=OnEventEvtDragonNestClear;
		CsRplzSession.Instance.EventEvtDragonNestFail+=OnEventEvtDragonNestFail;
		CsRplzSession.Instance.EventEvtDragonNestBanished+=OnEventEvtDragonNestBanished;

		// AnkouTomb
		CsRplzSession.Instance.EventResAnkouTombMatchingStart += OnEventResAnkouTombMatchingStart;
		CsRplzSession.Instance.EventResAnkouTombMatchingCancel += OnEventResAnkouTombMatchingCancel;
		CsRplzSession.Instance.EventResAnkouTombEnter += OnEventResAnkouTombEnter;
		CsRplzSession.Instance.EventResAnkouTombExit += OnEventResAnkouTombExit;
		CsRplzSession.Instance.EventResAnkouTombAbandon += OnEventResAnkouTombAbandon;
		CsRplzSession.Instance.EventResAnkouTombRevive += OnEventResAnkouTombRevive;
		CsRplzSession.Instance.EventResAnkouTombMoneyBuffActivate += OnEventResAnkouTombMoneyBuffActivate;
		CsRplzSession.Instance.EventResAnkouTombAdditionalRewardExpReceive += OnEventResAnkouTombAdditionalRewardExpReceive;

		CsRplzSession.Instance.EventEvtContinentExitForAnkouTombEnter += OnEventEvtContinentExitForAnkouTombEnter;
		CsRplzSession.Instance.EventEvtAnkouTombMatchingRoomPartyEnter += OnEventEvtAnkouTombMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtAnkouTombMatchingRoomBanished += OnEventEvtAnkouTombMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtAnkouTombMatchingStatusChanged += OnEventEvtAnkouTombMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtAnkouTombWaveStart += OnEventEvtAnkouTombWaveStart;
		CsRplzSession.Instance.EventEvtAnkouTombPointAcquisition += OnEventEvtAnkouTombPointAcquisition;
		CsRplzSession.Instance.EventEvtAnkouTombMoneyBuffFinished += OnEventEvtAnkouTombMoneyBuffFinished;
		CsRplzSession.Instance.EventEvtAnkouTombMoneyBuffCancel += OnEventEvtAnkouTombMoneyBuffCancel;
		CsRplzSession.Instance.EventEvtAnkouTombClear += OnEventEvtAnkouTombClear;
		CsRplzSession.Instance.EventEvtAnkouTombFail += OnEventEvtAnkouTombFail;
		CsRplzSession.Instance.EventEvtAnkouTombBanished += OnEventEvtAnkouTombBanished;
		CsRplzSession.Instance.EventEvtAnkouTombServerBestRecordUpdated += OnEventEvtAnkouTombServerBestRecordUpdated;

		// TradeShip
		CsRplzSession.Instance.EventResTradeShipMatchingStart += OnEventResTradeShipMatchingStart;
		CsRplzSession.Instance.EventResTradeShipMatchingCancel += OnEventResTradeShipMatchingCancel;
		CsRplzSession.Instance.EventResTradeShipEnter += OnEventResTradeShipEnter;
		CsRplzSession.Instance.EventResTradeShipExit += OnEventResTradeShipExit;
		CsRplzSession.Instance.EventResTradeShipAbandon += OnEventResTradeShipAbandon;
		CsRplzSession.Instance.EventResTradeShipRevive += OnEventResTradeShipRevive;
		CsRplzSession.Instance.EventResTradeShipMoneyBuffActivate += OnEventResTradeShipMoneyBuffActivate;
		CsRplzSession.Instance.EventResTradeShipAdditionalRewardExpReceive += OnEventResTradeShipAdditionalRewardExpReceive;

		CsRplzSession.Instance.EventEvtContinentExitForTradeShipEnter += OnEventEvtContinentExitForTradeShipEnter;
		CsRplzSession.Instance.EventEvtTradeShipMatchingRoomPartyEnter += OnEventEvtTradeShipMatchingRoomPartyEnter;
		CsRplzSession.Instance.EventEvtTradeShipMatchingRoomBanished += OnEventEvtTradeShipMatchingRoomBanished;
		CsRplzSession.Instance.EventEvtTradeShipMatchingStatusChanged += OnEventEvtTradeShipMatchingStatusChanged;
		CsRplzSession.Instance.EventEvtTradeShipStepStart += OnEventEvtTradeShipStepStart;
		CsRplzSession.Instance.EventEvtTradeShipPointAcquisition += OnEventEvtTradeShipPointAcquisition;
		CsRplzSession.Instance.EventEvtTradeShipObjectDestructionReward += OnEventEvtTradeShipObjectDestructionReward;
		CsRplzSession.Instance.EventEvtTradeShipMoneyBuffFinished += OnEventEvtTradeShipMoneyBuffFinished;
		CsRplzSession.Instance.EventEvtTradeShipMoneyBuffCancel += OnEventEvtTradeShipMoneyBuffCancel;
		CsRplzSession.Instance.EventEvtTradeShipClear += OnEventEvtTradeShipClear;
		CsRplzSession.Instance.EventEvtTradeShipFail += OnEventEvtTradeShipFail;
		CsRplzSession.Instance.EventEvtTradeShipBanished += OnEventEvtTradeShipBanished;
		CsRplzSession.Instance.EventEvtTradeShipServerBestRecordUpdated += OnEventEvtTradeShipServerBestRecordUpdated;

		//TeamBattlefield



	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		UnInit();
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_bWaitResponse = false;
		m_bAuto = false;
		m_bDungeonStart = false;
		m_bDungeonClear = false;

		m_nStoryDungeonNo = 0;
		m_nWaveNo = 0;
		m_nDifficulty = 0;
		m_lBuffBoxInstanceId = 0;
		m_nUndergroundMazeFloor = 0;

		m_flAncientRelicCurrentPoint = 0f;
		m_flAncientRelicMatchingRemainingTime = 0f;
		m_flSoulCoveterMatchingRemainingTime = 0f;
		m_flRuinsReclaimMatchingRemainingTime = 0f;
        m_flDragonNestMatchingRemainingTime = 0f;
		m_flAnkouTombMatchingRemainingTime = 0f;
		m_flTeamBattlefieldRemainingEnterWaitingTime = 0f;

        m_nAnkouTombPoint = 0;
        m_nTradeShipPoint = 0;
		m_nTeamBattlefieldKillcount = 0;
		m_nTeamBattlefieldPoint = 0;
		
		m_csStoryDungeon = null;
		m_csStoryDungeonDifficulty = null;
		m_csStoryDungeonStep = null;
		m_csExpDungeonDifficulty = null;
		m_csExpDungeonDifficultyWave = null;
		m_csGoldDungeonDifficulty = null;
		m_csGoldDungeonStep = null;
		m_csGoldDungeonStepWave = null;
		m_csUndergroundMazeFloor = null;
		m_csArtifactRoomFloor = null;
		m_csAncientRelicStep = null;
		m_csAncientRelicStepWave = null;
		m_pDHeroFieldOfHonor = null;
		m_csSoulCoveterDifficulty = null;
		m_csSoulCoveterDifficultyWave = null;
		m_csEliteMonster = null;
		SetInfiniteWarPoint(Guid.Empty, "", 0, 0,true);
        SetWarMemoryPoint(Guid.Empty, 0, 0, true);
		m_csWarMemoryWave = null;
		m_csOsirisRoomDifficulty = null;
		m_csOsirisRoomDifficultyWave = null;
		m_csDragonNestStep = null;
		m_csTradeShipStep = null;

		m_enDungeonPlay = EnDungeonPlay.None;
		m_enDungeonEnterType = EnDungeonEnterType.InitEnter;
		m_enWisdomTempleType = EnWisdomTempleType.None;

		m_enAncientRelicState = EnDungeonMatchingState.None;
		m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;
		m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;
		m_enInfiniteWarMatchingState = EnDungeonMatchingState.None;
		m_enFearAltarMatchingState = EnDungeonMatchingState.None;
		m_enWarMemoryMatchingState = EnDungeonMatchingState.None;
        m_enDragonNestMatchingState = EnDungeonMatchingState.None;
		m_enAnkouTombMatchingState = EnDungeonMatchingState.None;
		m_enTradeShipMatchingState = EnDungeonMatchingState.None;
}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse)
		{
			return;
		}

		if (m_bAuto == false)
		{
			m_bAuto = true;

			if (EventDungeonStartAutoPlay != null)
			{
				EventDungeonStartAutoPlay(m_enDungeonPlay);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		if (m_bAuto == true)
		{
			m_bAuto = false;
			if (EventDungeonStopAutoPlay != null)
			{
				EventDungeonStopAutoPlay(objCaller, m_enDungeonPlay);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public RectTransform OnEventCreateMonsterHUD(long lInstanceId, CsMonsterInfo csMonsterInfo, bool bIsBossMonster = false, int nHalidomId = 0)
	{
		if (EventCreateMonsterHUD != null)
		{
			return EventCreateMonsterHUD(m_enDungeonPlay, lInstanceId, csMonsterInfo, bIsBossMonster, nHalidomId);
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDeleteMonsterHUD(long lInstanceId)
	{
		if (EventDeleteMonsterHUD != null)
		{
			EventDeleteMonsterHUD(lInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool DungeonStart()
	{
		if (m_enDungeonPlay != EnDungeonPlay.None && m_enDungeonPlay != EnDungeonPlay.MainQuest) 
		{
			return m_bDungeonStart;
		}

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void DungeonClear()
	{
		Debug.Log("DungeonClear   "+ m_bDungeonClear);
		if (m_bDungeonClear) return;

		m_bDungeonClear = true;
		if (EventDungeonClear != null)
		{
			EventDungeonClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeInteractionState(EnInteractionState enNewInteractionState)
	{
		dd.d("ChangeInteractionState", m_nObjectId, m_lInstanceId, enNewInteractionState);

		if (enNewInteractionState == EnInteractionState.None)
		{
			m_nArrangeNo = 0;
			m_nObjectId = 0;
			m_lInstanceId = 0;
			m_flInteractionDuration = 0;
		}

		m_enInteractionState = enNewInteractionState;

		if (EventChangeDungeonInteractionState != null)
		{
			EventChangeDungeonInteractionState(m_enInteractionState);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void DungeonObjectInteractionAble(bool bAble, int nObjectId, long lInstanceId)
	{
		if (m_enInteractionState == EnInteractionState.interacting) return;

		//dd.d("######    DungeonObjectInteractionAble()      ######", bAble, nObjectId, lInstanceId);

		if (bAble)
		{
			m_nObjectId = nObjectId;
			m_lInstanceId = lInstanceId;

			if (m_enInteractionState == EnInteractionState.None)
			{
				ChangeInteractionState(EnInteractionState.ViewButton);
			}
		}
		else
		{
			if (m_enInteractionState == EnInteractionState.ViewButton)
			{
				if (m_lInstanceId == lInstanceId)
				{
					ChangeInteractionState(EnInteractionState.None);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void DugeonObjectInteractionStart()
	{
		Debug.Log("######    DugeonObjectInteractionStart()      ######");
		if (EventDugeonObjectInteractionStart != null)
		{
			EventDugeonObjectInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void TryDungeonObjectInteractionCancel()
	{
		dd.d("TryDungeonObjectInteractionCancel    m_enDungeonPlay = " + m_enDungeonPlay);
		ChangeInteractionState(EnInteractionState.None);

		switch (m_enDungeonPlay)
		{
			case EnDungeonPlay.WisdomTemple:
				SendWisdomTempleObjectInteractionCancel();
				break;
			case EnDungeonPlay.RuinsReclaim:
				SendRuinsReclaimObjectInteractionCancel();
				break;
			case EnDungeonPlay.WarMemory:
				WarMemoryObjectInteractionCancel();
				break;
		}

		if (EventDugeonObjectInteractionCancel != null)
		{
			EventDugeonObjectInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void LakMonsterDead()
	{
		if (EventLakMonsterDead != null)
		{
			EventLakMonsterDead();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void DungeonResult(bool bSuccess)
	{
		if (EventDungeonResult != null)
		{
			EventDungeonResult(bSuccess);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventPartyDungeonHeroExit(Guid guiHeroId)
	{
		if (EventPartyDungeonHeroExit != null)
		{
			EventPartyDungeonHeroExit(guiHeroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateDungeonMember()
	{
		if (EventUpdateDungeonMember != null)
		{
			EventUpdateDungeonMember();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetDungeon()
	{
		Debug.Log("ResetDungeon "+ m_enDungeonPlay);
		switch (m_enDungeonPlay)
		{
			case EnDungeonPlay.AncientRelic:
				m_csAncientRelicStep = null;
				m_csAncientRelicStepWave = null;
				m_enAncientRelicState = EnDungeonMatchingState.None;
				break;
			case EnDungeonPlay.ArtifactRoom:
				m_csArtifactRoomFloor = null;
				break;
			case EnDungeonPlay.Elite:
				m_csEliteMonster = null;
				break;
			case EnDungeonPlay.Exp:
				m_csExpDungeonDifficulty = null;
				m_csExpDungeonDifficultyWave = null;
				break;
			case EnDungeonPlay.FieldOfHonor:
				m_pDHeroFieldOfHonor = null;
				break;
			case EnDungeonPlay.Gold:
				m_csGoldDungeonDifficulty = null;
				m_csGoldDungeonStep = null;
				m_csGoldDungeonStepWave = null;
				break;
			case EnDungeonPlay.ProofOfValor:
				break;
			case EnDungeonPlay.SoulCoveter:
				m_csSoulCoveterDifficulty = null;
				m_csSoulCoveterDifficultyWave = null;
				m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;
				break;
			case EnDungeonPlay.Story:
				m_nStoryDungeonNo = 0;
				m_csStoryDungeon = null;
				m_csStoryDungeonDifficulty = null;
				m_csStoryDungeonStep = null;
				break;
			case EnDungeonPlay.WisdomTemple:
				m_csWisdomTempleStep = null;
				m_csWisdomTemplePuzzle = null;
				break;
			case EnDungeonPlay.RuinsReclaim:
				m_csRuinsReclaimStep = null;
				m_csRuinsReclaimStepWave = null;
				m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;
				break;
			case EnDungeonPlay.InfiniteWar:
				SetInfiniteWarPoint(Guid.Empty, "", 0, 0, true);
				m_enInfiniteWarMatchingState = EnDungeonMatchingState.None;
				ClearInfiniteWarHero();
				break;
			case EnDungeonPlay.WarMemory:
				SetWarMemoryPoint(Guid.Empty, 0, 0, true);
				m_csWarMemoryWave = null;
				m_enWarMemoryMatchingState = EnDungeonMatchingState.None;
				ClearWarMemoryHero();
				break;
			case EnDungeonPlay.OsirisRoom:
				m_csOsirisRoomDifficulty = null;
				m_csOsirisRoomDifficultyWave = null;
                m_lOsirisRoomTotalGoldAcquisitionValue = 0;
				break;
			case EnDungeonPlay.Biography:
				m_csBiographyQuestDungeon = null;
				m_csBiographyQuestDungeonWave = null;
				break;
			case EnDungeonPlay.DragonNest:
                m_enDragonNestMatchingState = EnDungeonMatchingState.None;
                m_flDragonNestMatchingRemainingTime = 0f;
				m_csDragonNestStep = null;
				break;
			case EnDungeonPlay.AnkouTomb:
				m_enAnkouTombMatchingState = EnDungeonMatchingState.None;
				m_flAnkouTombMatchingRemainingTime = 0f;
                m_nAnkouTombPoint = 0;
				break;
			case EnDungeonPlay.TradeShip:
				m_enTradeShipMatchingState = EnDungeonMatchingState.None;
				m_flTradeShipMatchingRemainingTime = 0f;
                m_nTradeShipPoint = 0;
				m_csTradeShipStep = null;
				break;
			case EnDungeonPlay.TeamBattlefield:
				m_flTeamBattlefieldRemainingEnterWaitingTime = 0f;
				m_nTeamBattlefieldKillcount = 0;
				m_nTeamBattlefieldPoint = 0;
				m_enBattlefieldState = EnBattlefieldState.None;
				break;		}

		m_bDungeonStart = false;
		m_bDungeonClear = false;

		m_nWaveNo = 0;
		m_nDifficulty = 0;
		m_nPortalId = 0;
		m_nObjectId = 0;
		m_lInstanceId = 0;
		m_lBuffBoxInstanceId = 0;

		ChangeInteractionState(EnInteractionState.None);
		m_enDungeonPlay = EnDungeonPlay.None;
	}

	//---------------------------------------------------------------------------------------------------
	void DungeonEnterFail()
	{
		Debug.Log("DungeonEnterFail()                               m_enDungeonPlay = " + m_enDungeonPlay);
		ResetDungeon();
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
		if (EventDungeonEnterFail != null)
		{
			EventDungeonEnterFail(CsGameData.Instance.MyHeroInfo.LocationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ContinentEnterFail()
	{
		Debug.Log("ContinentEnterFail()");
		ResetDungeon();

		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
		if (EventDungeonEnterFail != null)
		{
			EventDungeonEnterFail(CsGameData.Instance.MyHeroInfo.LocationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region StoryDungeon

	//---------------------------------------------------------------------------------------------------
	public void StoryDungeonTameable(bool bTameable)
	{
		if (EventStoryDungeonTameable != null)
		{
			EventStoryDungeonTameable(bTameable);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StoryDungeonHeroStartTame()
	{
		if (EventStoryDungeonHeroStartTame != null)
		{
			EventStoryDungeonHeroStartTame();
		}
	}

	#region StoryDungeon.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForStoryDungeonEnter(int nDungeonNo, int nDifficulty) // 스토리던전입장을위한대륙퇴장
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForStoryDungeonEnterCommandBody cmdBody = new ContinentExitForStoryDungeonEnterCommandBody();
			cmdBody.dungeonNo = m_nStoryDungeonNo = nDungeonNo;     //스토리던전번호
			cmdBody.difficulty = m_nDifficulty = nDifficulty;   // 난이도

			m_csStoryDungeon = CsGameData.Instance.GetStoryDungeon(m_nStoryDungeonNo);
			m_csStoryDungeonDifficulty = m_csStoryDungeon.GetStoryDungeonDifficulty(m_nDifficulty);
			m_csStoryDungeonStep = m_csStoryDungeonDifficulty.GetStoryDungeonStep(1);
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForStoryDungeonEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForStoryDungeonEnter(int nReturnCode, ContinentExitForStoryDungeonEnterResponseBody responeseBody)
	{
		Debug.Log("##########          OnEventResContinentExitForStoryDungeonEnter()     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.Story;
			if (EventContinentExitForStoryDungeonEnter != null)
			{
				EventContinentExitForStoryDungeonEnter();
			}
		}
		else if (nReturnCode == 101)
        {
            // 영웅의 레벨이 낮아 해당 던전에 입장할 수 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00101"));
		}
		else if (nReturnCode == 102)
        {
            // 영웅의 레벨이 높아 해당 던전에 입장할 수 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00102"));
		}
		else if (nReturnCode == 103)
        {
            // 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00103"));
		}
		else if (nReturnCode == 104)
        {
            // 영웅이 전투상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00104"));
		}
		else if (nReturnCode == 105)
        {
            // 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00105"));
		}
		else if (nReturnCode == 106)
        {
            // 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00106"));
		}
        else if (nReturnCode == 107)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00107"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoryDungeonEnter() // 스토리던전입장
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StoryDungeonEnterCommandBody cmdBody = new StoryDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonEnter, cmdBody);
		}
	}

	void OnEventResStoryDungeonEnter(int nReturnCode, StoryDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = m_csStoryDungeon.LocationId; // 위치 정보 갱신.
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_csStoryDungeon.PlayCount = responseBody.playCount;
			m_csStoryDungeon.PlayDate = responseBody.date;
			m_bDungeonStart = true;

			if (EventStoryDungeonEnter != null)
			{
				EventStoryDungeonEnter(responseBody.position, responseBody.rotationY, responseBody.placeInstanceId);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoryDungeonAbandon() //스토리던전포기
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StoryDungeonAbandonCommandBody cmdBody = new StoryDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonAbandon, cmdBody);
		}
	}

	void OnEventResStoryDungeonAbandon(int nReturnCode, StoryDungeonAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventStoryDungeonAbandon != null)
			{
				EventStoryDungeonAbandon(responseBody.previousContinentId);
			}

		}
		else if (nReturnCode == 101)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00202"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoryDungeonExit() // 스토리던전퇴장
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			StoryDungeonExitCommandBody cmdBody = new StoryDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonExit, cmdBody);
		}
	}

	void OnEventResStoryDungeonExit(int nReturnCode, StoryDungeonExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			Debug.Log("OnEventResStoryDungeonExit========> confirm");
			if (EventStoryDungeonExit != null)
			{
				EventStoryDungeonExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00301"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoryDungeonRevive() // 스토리던전부활
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StoryDungeonReviveCommandBody cmdBody = new StoryDungeonReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonRevive, cmdBody);
		}
	}
	void OnEventResStoryDungeonRevive(int nReturnCode, StoryDungeonReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventStoryDungeonRevive != null)
			{
				EventStoryDungeonRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00403"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	public void SendStoryDungeonSweep(int nDungeonNo, int nDifficulty) // 스토리던전소탕
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StoryDungeonSweepCommandBody cmdBody = new StoryDungeonSweepCommandBody();
			cmdBody.dungeonNo = m_nStoryDungeonNo = nDungeonNo;     //스토리던전번호
			cmdBody.difficulty = m_nDifficulty = nDifficulty;   // 난이도

			m_csStoryDungeon = CsGameData.Instance.GetStoryDungeon(m_nStoryDungeonNo);
			m_csStoryDungeonDifficulty = m_csStoryDungeon.GetStoryDungeonDifficulty(m_nDifficulty);

			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonSweep, cmdBody);
		}
	}
	void OnEventResStoryDungeonSweep(int nReturnCode, StoryDungeonSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_csStoryDungeon.PlayDate = responseBody.date;
			m_csStoryDungeon.PlayCount = responseBody.playCount;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = responseBody.freeSweepDailyCount;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (EventStoryDungeonSweep != null)
			{
				EventStoryDungeonSweep();
			}

		}
		else if (nReturnCode == 101)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00503"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoryDungeonMonsterTame(long lMonsterInstanceId) // 스토리던전몬스터테이밍
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StoryDungeonMonsterTameCommandBody cmdBody = new StoryDungeonMonsterTameCommandBody();
			cmdBody.monsterInstanceId = lMonsterInstanceId;     // 테이밍몬스터 ID

			m_csStoryDungeon = CsGameData.Instance.GetStoryDungeon(m_nStoryDungeonNo);
			m_csStoryDungeonDifficulty = m_csStoryDungeon.GetStoryDungeonDifficulty(m_nDifficulty);

			CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonMonsterTame, cmdBody);
		}
	}

	void OnEventResStoryDungeonMonsterTame(int nReturnCode, StoryDungeonMonsterTameResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsIngameData.Instance.TameMonster.SetTame(CsRplzSession.Translate(responseBody.position), responseBody.rotationY);
			if (EventStoryDungeonMonsterTame != null)
			{
				EventStoryDungeonMonsterTame();
			}
		}
		else if (nReturnCode == 101)
		{
			// 테이밍을 할 수 없는 위치입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 테이밍중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00702"));
		}
		else if (nReturnCode == 103)
		{
			//영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A17_ERROR_00703"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}


	#endregion StoryDungeon.Protocol.Command

	#region StoryDungeon.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtStoryDungeonStepStart(SEBStoryDungeonStepStartEventBody eventBody) // 스토리던전단계시작
	{
		if (m_csStoryDungeonStep != null)
		{
			if (m_csStoryDungeonStep.IsCompletionRemoveTaming)
			{
				if (EventStoryDungeonRemoveTaming != null)
				{
					EventStoryDungeonRemoveTaming();
				}
			}
		}

		m_csStoryDungeonStep = m_csStoryDungeonDifficulty.GetStoryDungeonStep(eventBody.stepNo);   // 세팅 필요 현재 공용에 가져올 데이터 설정 안되어있음.

		if (EventStoryDungeonStepStart != null)
		{
			EventStoryDungeonStepStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtStoryDungeonClear(SEBStoryDungeonClearEventBody eventBody) // 스토리던전클리어
	{
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		if (m_csStoryDungeon.ClearMaxDifficulty < m_nDifficulty)
		{
			m_csStoryDungeon.ClearMaxDifficulty = m_nDifficulty;
		}

		DungeonResult(true);
		if (EventStoryDungeonClear != null)
		{
			EventStoryDungeonClear(eventBody.booties);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtStoryDungeonFail(SEBStoryDungeonFailEventBody eventBody) // 스토리던전실패
	{
		DungeonResult(false);
		if (EventStoryDungeonFail != null)
		{
			EventStoryDungeonFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtStoryDungeonBanished(SEBStoryDungeonBanishedEventBody eventBody) // 스토리던전강퇴
	{
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		Debug.Log("OnEventEvtStoryDungeonBanished  " + eventBody.hp);

		if (EventStoryDungeonBanished != null)
		{
			EventStoryDungeonBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	//void OnEventEvtStoryDungeonAccelerationStarted(SEBStoryDungeonAccelerationStartedEventBody eventBody) // 가속
	//{
	//	if (EventStoryDungeonAccelerationStarted != null)
	//	{
	//		EventStoryDungeonAccelerationStarted();
	//	}
	//}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtStoryDungeonTrapCast(SEBStoryDungeonTrapCastEventBody eventBody)
	{
		if (EventStoryDungeonTrapCast != null)
		{
			EventStoryDungeonTrapCast(eventBody.trapId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonTrapHit(SEBStoryDungeonTrapHitEventBody eventBody)
	{
		if (EventStoryDungeonTrapHit != null)
		{
			//eventBody.isImmortal // 죽지 않았다라는 표시용.(별도 처리 없음)
			EventStoryDungeonTrapHit(eventBody.hp, eventBody.damage, eventBody.hpDamage, eventBody.removedAbnormalStateEffects, eventBody.changedAbnormalStateEffectDamageAbsorbShields);
		}
	}

	#endregion StoryDungeon.Protocol.Event
	#endregion StoryDungeon

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region ExpDungeon
	#region ExpDungeon.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForExpDungeonEnter(int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			if (EventMyHeroDungeonEnterMoveStop != null)
			{
				EventMyHeroDungeonEnterMoveStop();
			}

			ContinentExitForExpDungeonEnterCommandBody cmdBody = new ContinentExitForExpDungeonEnterCommandBody();
			cmdBody.difficulty = m_nDifficulty = nDifficulty;

			m_csExpDungeonDifficulty = ExpDungeon.GetExpDungeonDifficulty(m_nDifficulty);
			m_csExpDungeonDifficultyWave = m_csExpDungeonDifficulty.GetExpDungeonDifficultyWave(1);

			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForExpDungeonEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForExpDungeonEnter(int nReturnCode, ContinentExitForExpDungeonEnterResponseBody responeseBody)
	{
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.Exp;
			if (EventContinentExitForExpDungeonEnter != null)
			{
				EventContinentExitForExpDungeonEnter();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 낮아 해당 던전에 입장할 수 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 전투상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00105"));
		}
        else if (nReturnCode == 106)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00106"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendExpDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendExpDungeonEnter()");
			m_bWaitResponse = true;
			ExpDungeonEnterCommandBody cmdBody = new ExpDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ExpDungeonEnter, cmdBody);
		}
	}

	void OnEventResExpDungeonEnter(int nReturnCode, ExpDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.LocationId = ExpDungeon.LocationId; // 위치 정보 갱신.
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			ExpDungeon.ExpDungeonDailyPlayCount = responseBody.playCount;
			ExpDungeon.ExpDungeonPlayCountDate = responseBody.date;
			m_bDungeonStart = true;

			if (EventExpDungeonEnter != null)
			{
				EventExpDungeonEnter(responseBody.position, responseBody.rotationY, responseBody.placeInstanceId);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendExpDungeonAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendExpDungeonAbandon()");
			m_bWaitResponse = true;
			ExpDungeonAbandonCommandBody cmdBody = new ExpDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ExpDungeonAbandon, cmdBody);
		}
	}

	void OnEventResExpDungeonAbandon(int nReturnCode, ExpDungeonAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventExpDungeonAbandon != null)
			{
				EventExpDungeonAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00301"));

		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00302"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendExpDungeonExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ExpDungeonExitCommandBody cmdBody = new ExpDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ExpDungeonExit, cmdBody);
		}
	}

	void OnEventResExpDungeonExit(int nReturnCode, ExpDungeonExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventStoryDungeonExit != null)
			{
				EventExpDungeonExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00401"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendExpDungeonRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ExpDungeonReviveCommandBody cmdBody = new ExpDungeonReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ExpDungeonRevive, cmdBody);
		}
	}

	void OnEventResExpDungeonRevive(int nReturnCode, ExpDungeonReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventStoryDungeonRevive != null)
			{
				EventExpDungeonRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 부활대기시간이 경과하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00503"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendExpDungeonSweep(int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ExpDungeonSweepCommandBody cmdBody = new ExpDungeonSweepCommandBody();
			cmdBody.difficulty = m_nDifficulty = nDifficulty;
			CsRplzSession.Instance.Send(ClientCommandName.ExpDungeonSweep, cmdBody);
		}
	}

	void OnEventResExpDungeonSweep(int nReturnCode, ExpDungeonSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ExpDungeon.ExpDungeonPlayCountDate = responseBody.date;
			ExpDungeon.ExpDungeonDailyPlayCount = responseBody.playCount;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = responseBody.freeSweepDailyCount;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (responseBody.changedInventorySlot != null)
			{
				PDInventorySlot[] arrInventorySlot = { responseBody.changedInventorySlot };
				CsGameData.Instance.MyHeroInfo.AddInventorySlots(arrInventorySlot);
			}

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventExpDungeonSweep != null)
			{
				EventExpDungeonSweep(bLevelUp, responseBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 전투상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
			// 소탕아이템이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00603"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미너가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00604"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A32_ERROR_00605"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion ExpDungeon.Protocol.Command

	#region ExpDungeon.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtExpDungeonWaveStart(SEBExpDungeonWaveStartEventBody eventBody)
	{
		m_csExpDungeonDifficultyWave = m_csExpDungeonDifficulty.GetExpDungeonDifficultyWave(eventBody.waveNo);

		if (EventExpDungeonWaveStart != null)
		{
			EventExpDungeonWaveStart(eventBody.monsterInsts, eventBody.lakChargeMonsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtExpDungeonWaveCompleted(SEBExpDungeonWaveCompletedEventBody eventBody)
	{
		if (EventExpDungeonWaveCompleted != null)
		{
			EventExpDungeonWaveCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtExpDungeonWaveTimeout(SEBExpDungeonWaveTimeoutEventBody eventBody)
	{
		if (EventExpDungeonWaveTimeout != null)
		{
			EventExpDungeonWaveTimeout(); // ingame 몬스터 삭제 처리 필요.
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtExpDungeonClear(SEBExpDungeonClearEventBody eventBody)
	{
		ExpDungeon.ExpDungeonClearedDifficultyList.Add(m_nDifficulty);
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(true);
		if (EventExpDungeonClear != null)
		{
			EventExpDungeonClear(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtExpDungeonBanished(SEBExpDungeonBanishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventExpDungeonBanished != null)
		{
			EventExpDungeonBanished(eventBody.previousContinentId);
		}
	}

	#endregion ExpDungeon.Protocol.Event
	#endregion ExpDungeon

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region GoldDungeon
	#region GoldDungeon.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForGoldDungeonEnter(int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			if (EventMyHeroDungeonEnterMoveStop != null)
			{
				EventMyHeroDungeonEnterMoveStop();
			}

			ContinentExitForGoldDungeonEnterCommandBody cmdBody = new ContinentExitForGoldDungeonEnterCommandBody();
			cmdBody.difficulty = m_nDifficulty = nDifficulty;

			GoldDungeon = CsGameData.Instance.GoldDungeon;
			m_csGoldDungeonDifficulty = GoldDungeon.GetGoldDungeonDifficulty(nDifficulty);
			m_csGoldDungeonStep = m_csGoldDungeonDifficulty.GetGoldDungeonStep(1);

			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForGoldDungeonEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForGoldDungeonEnter(int nReturnCode, ContinentExitForGoldDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.Gold;
			if (EventContinentExitForGoldDungeonEnter != null)
			{
				EventContinentExitForGoldDungeonEnter();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 낮아 해당 던전에 입장할 수 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 전투상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00105"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGoldDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			GoldDungeonEnterCommandBody cmdBody = new GoldDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GoldDungeonEnter, cmdBody);
		}
	}

	void OnEventResGoldDungeonEnter(int nReturnCode, GoldDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			GoldDungeon.GoldDungeonDailyPlayCount = responseBody.playCount;
			GoldDungeon.GoldDungeonPlayCountDate = responseBody.date;

			CsGameData.Instance.MyHeroInfo.LocationId = GoldDungeon.LocationId; // 위치 정보 갱신.
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_bDungeonStart = true;

			if (EventGoldDungeonEnter != null)
			{
				EventGoldDungeonEnter(responseBody.position, responseBody.rotationY, responseBody.placeInstanceId);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGoldDungeonAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			GoldDungeonAbandonCommandBody cmdBody = new GoldDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GoldDungeonAbandon, cmdBody);
		}
	}

	void OnEventResGoldDungeonAbandon(int nReturnCode, GoldDungeonAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;


			if (EventGoldDungeonAbandon != null)
			{
				EventGoldDungeonAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00202"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGoldDungeonExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			GoldDungeonExitCommandBody cmdBody = new GoldDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GoldDungeonExit, cmdBody);
		}
	}

	void OnEventResGoldDungeonExit(int nReturnCode, GoldDungeonExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventGoldDungeonExit != null)
			{
				EventGoldDungeonExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00301"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGoldDungeonRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			GoldDungeonReviveCommandBody cmdBody = new GoldDungeonReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.GoldDungeonRevive, cmdBody);
		}
	}

	void OnEventResGoldDungeonRevive(int nReturnCode, GoldDungeonReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventGoldDungeonRevive != null)
			{
				EventGoldDungeonRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 부활대기시간이 경과하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00403"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendGoldDungeonSweep(int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			GoldDungeonSweepCommandBody cmdBody = new GoldDungeonSweepCommandBody();
			cmdBody.difficulty = m_nDifficulty = nDifficulty;

			GoldDungeon = CsGameData.Instance.GoldDungeon;
			m_csGoldDungeonDifficulty = GoldDungeon.GetGoldDungeonDifficulty(nDifficulty);

			CsRplzSession.Instance.Send(ClientCommandName.GoldDungeonSweep, cmdBody);
		}
	}

	void OnEventResGoldDungeonSweep(int nReturnCode, GoldDungeonSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			GoldDungeon.GoldDungeonPlayCountDate = responseBody.date;
			GoldDungeon.GoldDungeonDailyPlayCount = responseBody.playCount;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = responseBody.freeSweepDailyCount;

			if (responseBody.changedInventorySlot != null)
			{
				PDInventorySlot[] arrInventorySlot = { responseBody.changedInventorySlot };
				CsGameData.Instance.MyHeroInfo.AddInventorySlots(arrInventorySlot);
			}
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

			// 최대골드
			CsAccomplishmentManager.Instance.MaxGold = responseBody.maxGold;

			if (EventGoldDungeonSweep != null)
			{
				EventGoldDungeonSweep(responseBody.rewardGold); // 보상 골드 전달.
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 전투상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			// 소탕아이템이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00503"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미너가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00504"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A39_ERROR_00505"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion GoldDungeon.Protocol.Command

	#region GoldDungeon.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonStepStart(SEBGoldDungeonStepStartEventBody eventBody)  // 추가적으로 연출 몬스터 생성 및 처리 필요.
	{
		m_csGoldDungeonStep = m_csGoldDungeonDifficulty.GetGoldDungeonStep(eventBody.stepNo);

		if (EventGoldDungeonStepStart != null)
		{
			EventGoldDungeonStepStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonStepCompleted(SEBGoldDungeonStepCompletedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;

		// 최대골드
		CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

		if (EventGoldDungeonStepCompleted != null)
		{
			EventGoldDungeonStepCompleted(eventBody.rewardGold);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonWaveStart(SEBGoldDungeonWaveStartEventBody eventBody) // 해당 웨이브No 에 해당하는 몬스터 공격 상태로 전환 필요.
	{
		m_csGoldDungeonStepWave = m_csGoldDungeonStep.GetGoldDungeonStepWave(eventBody.waveNo);

		if (EventGoldDungeonWaveStart != null)
		{
			EventGoldDungeonWaveStart(eventBody.waveNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonWaveCompleted(SEBGoldDungeonWaveCompletedEventBody eventBody)
	{
		if (EventGoldDungeonWaveCompleted != null)
		{
			EventGoldDungeonWaveCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonWaveTimeout(SEBGoldDungeonWaveTimeoutEventBody eventBody) // 몬스터 삭제 처리 필요.
	{
		if (EventGoldDungeonWaveTimeout != null)
		{
			EventGoldDungeonWaveTimeout();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonClear(SEBGoldDungeonClearEventBody eventBody)
	{
		GoldDungeon.GoldDungeonClearedDifficultyList.Add(m_nDifficulty);
		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold; // 총 골드

		// 최대골드
		CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

		DungeonResult(true);
		if (EventGoldDungeonClear != null)
		{
			EventGoldDungeonClear(eventBody.rewardGold); // 보상 골드
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonFail(SEBGoldDungeonFailEventBody eventBody) // 몬스터 삭제 처리 필요.
	{
		DungeonResult(false);
		if (EventGoldDungeonFail != null)
		{
			EventGoldDungeonFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtGoldDungeonBanished(SEBGoldDungeonBanishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventExpDungeonBanished != null)
		{
			EventGoldDungeonBanished(eventBody.previousContinentId);
		}
	}

	#endregion GoldDungeon.Protocol.Event

	#endregion GoldDungeon

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region UndergroundMaze

	//---------------------------------------------------------------------------------------------------
	public void InitializeUndergroundMaze(int nFloor)
	{
		m_nUndergroundMazeFloor = nFloor; // 층
		m_enDungeonPlay = EnDungeonPlay.UndergroundMaze;
		m_enDungeonEnterType = EnDungeonEnterType.InitEnter;
		m_csUndergroundMazeFloor = UndergroundMaze.GetUndergroundMazeFloor(nFloor);
	}

	//---------------------------------------------------------------------------------------------------
	public RectTransform CreateUndergroundMazeNpcHUD(int nNpcId)
	{
		if (EventCreateUndergroundMazeNpcHUD != null)
		{
			return EventCreateUndergroundMazeNpcHUD(nNpcId);
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UndergroundMazeTransmissionReadyOK(int nNpcId)
	{
		if (EventUndergroundMazeTransmissionNpcDialog != null)
		{
			EventUndergroundMazeTransmissionNpcDialog(nNpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UndergroundMazePortalEnter(int nPortalId)
	{
		SendUndergroundMazePortalEnter(nPortalId);
	}

	#region UndergroundMaze.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForUndergroundMazeEnter(int nFloor) // ui
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendContinentExitForUndergroundMazeEnter()");
			m_bWaitResponse = true;
			InitializeUndergroundMaze(nFloor);
			if (EventMyHeroDungeonEnterMoveStop != null)
			{
				EventMyHeroDungeonEnterMoveStop();
			}

			ContinentExitForUndergroundMazeEnterCommandBody cmdBody = new ContinentExitForUndergroundMazeEnterCommandBody();
			cmdBody.floor = m_nUndergroundMazeFloor = nFloor;
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForUndergroundMazeEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForUndergroundMazeEnter(int nReturnCode, ContinentExitForUndergroundMazeEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("SendContinentExitForUndergroundMazeEnter()");
			if (EventContinentExitForUndergroundMazeEnter != null)
			{
				EventContinentExitForUndergroundMazeEnter();
			}
		}
		else if (nReturnCode == 103)
		{
			// 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 영웅이 전투중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 플레이시간이 제한시간을 초과했습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 이미 위탁을 한 할일입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00107"));
		}
        else if (nReturnCode == 108)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00108"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeEnter() // ingame
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendUndergroundMazeEnter()");
			m_bWaitResponse = true;

			UndergroundMazeEnterCommandBody cmdBody = new UndergroundMazeEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeEnter, cmdBody);
		}
	}

	void OnEventResUndergroundMazeEnter(int nReturnCode, UndergroundMazeEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			UndergroundMaze.UndergroundMazePlayTimeDate = responseBody.date;
			UndergroundMaze.UndergroundMazeDailyPlayTime = responseBody.playtime;
			m_bDungeonStart = true;

			if (EventUndergroundMazeEnter != null)
			{
				EventUndergroundMazeEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			UndergroundMazeExitCommandBody cmdBody = new UndergroundMazeExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeExit, cmdBody);
		}
	}

	void OnEventResUndergroundMazeExit(int nReturnCode, UndergroundMazeExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			UndergroundMaze.UndergroundMazeDailyPlayTime = responseBody.playTime;
			UndergroundMaze.UndergroundMazePlayTimeDate = responseBody.date;

			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventUndergroundMazeExit != null)
			{
				EventUndergroundMazeExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00301"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			m_enDungeonEnterType = EnDungeonEnterType.Revival;
			UndergroundMazeReviveCommandBody cmdBody = new UndergroundMazeReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeRevive, cmdBody);
		}
	}

	void OnEventResUndergroundMazeRevive(int nReturnCode, UndergroundMazeReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_bDungeonStart = true;
			if (EventUndergroundMazeRevive != null)
			{
				EventUndergroundMazeRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00401"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeEnterForUndergroundMazeRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			UndergroundMazeEnterForUndergroundMazeReviveCommandBody cmdBody = new UndergroundMazeEnterForUndergroundMazeReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeEnterForUndergroundMazeRevive, cmdBody);
		}
	}

	void OnEventResUndergroundMazeEnterForUndergroundMazeRevive(int nReturnCode, UndergroundMazeEnterForUndergroundMazeReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			m_csUndergroundMazeFloor = UndergroundMaze.GetUndergroundMazeFloor(m_nUndergroundMazeFloor);
			UndergroundMaze.UndergroundMazePlayTimeDate = responseBody.date;
			m_bDungeonStart = true;

			if (EventUndergroundMazeEnterForUndergroundMazeRevive != null)
			{
				EventUndergroundMazeEnterForUndergroundMazeRevive(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendUndergroundMazePortalEnter(int nPortalId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			m_enDungeonEnterType = EnDungeonEnterType.Portal;

			if (EventMyHeroUndergroundMazePortalEnter != null)
			{
				EventMyHeroUndergroundMazePortalEnter();
			}

			UndergroundMazePortalEnterCommandBody cmdBody = new UndergroundMazePortalEnterCommandBody();
			cmdBody.portalId = m_nPortalId = nPortalId;
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazePortalEnter, cmdBody);
		}
	}

	void OnEventResUndergroundMazePortalEnter(int nReturnCode, UndergroundMazePortalEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			int nLinkedPortalId = m_csUndergroundMazeFloor.UndergroundMazePortalList.Find(a => a.PortalId == m_nPortalId).LinkedPortalId;
			for (int i = 0; i < UndergroundMaze.UndergroundMazeFloorList.Count; i++)
			{
				CsUndergroundMazePortal csUndergroundMazePortal = UndergroundMaze.UndergroundMazeFloorList[i].UndergroundMazePortalList.Find(a => a.PortalId == nLinkedPortalId);
				if (csUndergroundMazePortal != null)
				{
					m_nUndergroundMazeFloor = csUndergroundMazePortal.Floor;
					m_csUndergroundMazeFloor = UndergroundMaze.GetUndergroundMazeFloor(m_nUndergroundMazeFloor);
					break;
				}
			}
			m_bDungeonStart = true;

			if (EventUndergroundMazePortalEnter != null)
			{
				EventUndergroundMazePortalEnter();
			}
		}
		else if (nReturnCode == 101)
		{
			// 포탈 입장범위안에 있지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00602"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_nPortalId = 0;
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazePortalExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			UndergroundMazePortalExitCommandBody cmdBody = new UndergroundMazePortalExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazePortalExit, cmdBody);
		}
	}

	void OnEventResUndergroundMazePortalExit(int nReturnCode, UndergroundMazePortalExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_csUndergroundMazeFloor = UndergroundMaze.GetUndergroundMazeFloor(m_nUndergroundMazeFloor);

			if (EventUndergroundMazePortalExit != null)
			{
				EventUndergroundMazePortalExit(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeTransmission(int nNpcId, int nFloor)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			m_enDungeonEnterType = EnDungeonEnterType.Transmission;
			UndergroundMazeTransmissionCommandBody cmdBody = new UndergroundMazeTransmissionCommandBody();
			cmdBody.npcId = nNpcId;
			cmdBody.floor = m_nUndergroundMazeFloor = nFloor;
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeTransmission, cmdBody);
		}
	}

	void OnEventResUndergroundMazeTransmission(int nReturnCode, UndergroundMazeTransmissionResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_bDungeonStart = true;
			if (EventUndergroundMazeTransmission != null)
			{
				EventUndergroundMazeTransmission();
			}
		}
		else if (nReturnCode == 101)
		{
			// 전송범위안에 있지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A43_ERROR_00802"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendUndergroundMazeEnterForTransmission()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			UndergroundMazeEnterForTransmissionCommandBody cmdBody = new UndergroundMazeEnterForTransmissionCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.UndergroundMazeEnterForTransmission, cmdBody);
		}
	}

	void OnEventResUndergroundMazeEnterForTransmission(int nReturnCode, UndergroundMazeEnterForTransmissionResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_csUndergroundMazeFloor = UndergroundMaze.GetUndergroundMazeFloor(m_nUndergroundMazeFloor);
			if (EventUndergroundMazeEnterForTransmission != null)
			{
				EventUndergroundMazeEnterForTransmission(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion UndergroundMaze.Protocol.Command

	#region UndergroundMaze.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtUndergroundMazeBanished(SEBUndergroundMazeBanishedEventBody eventBody)
	{
		UndergroundMaze.UndergroundMazeDailyPlayTime = CsGameData.Instance.UndergroundMaze.LimitTime;
		//UndergroundMaze.UndergroundMazePlayTimeDate = responseBody.date;

		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventUndergroundMazeBanished != null)
		{
			EventUndergroundMazeBanished(eventBody.previousContinentId);
		}
	}

	#endregion UndergroundMaze.Protocol.Event

	#endregion UndergroundMaze

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region ArtifactRoom

	#region ArtifactRoom.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForArtifactRoomEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			if (EventMyHeroDungeonEnterMoveStop != null)
			{
				EventMyHeroDungeonEnterMoveStop();
			}
			ContinentExitForArtifactRoomEnterCommandBody cmdBody = new ContinentExitForArtifactRoomEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForArtifactRoomEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForArtifactRoomEnter(int nReturnCode, ContinentExitForArtifactRoomEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.ArtifactRoom;
			m_csArtifactRoomFloor = ArtifactRoom.GetArtifactRoomFloor(ArtifactRoom.ArtifactRoomCurrentFloor);

			if (EventContinentExitForArtifactRoomEnter != null)
			{
				EventContinentExitForArtifactRoomEnter();
			}
		}
		else if (nReturnCode == 101)
		{
            // 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 모든층을 클리어했습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 소탕중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00105"));
		}
        else if (nReturnCode == 106)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00109"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomEnterCommandBody cmdBody = new ArtifactRoomEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomEnter, cmdBody);
		}
	}

	void OnEventResArtifactRoomEnter(int nReturnCode, ArtifactRoomEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.ArtifactRoom;
			m_csArtifactRoomFloor = ArtifactRoom.GetArtifactRoomFloor(ArtifactRoom.ArtifactRoomCurrentFloor);

			ArtifactRoom.ArtifactRoomInitCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_bDungeonStart = true;

			if (EventArtifactRoomEnter != null)
			{
				EventArtifactRoomEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomExitCommandBody cmdBody = new ArtifactRoomExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomExit, cmdBody);
		}
	}

	void OnEventResArtifactRoomExit(int nReturnCode, ArtifactRoomExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventArtifactRoomExit != null)
			{
				EventArtifactRoomExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00301"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomAbandon() // 고대유물의방포기
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomAbandonCommandBody cmdBody = new ArtifactRoomAbandonCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomAbandon, cmdBody);
		}
	}

	void OnEventResArtifactRoomAbandon(int nReturnCode, ArtifactRoomAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventArtifactRoomAbandon != null)
			{
				EventArtifactRoomAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00401"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomNextFloorChallenge()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomNextFloorChallengeCommandBody cmdBody = new ArtifactRoomNextFloorChallengeCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomNextFloorChallenge, cmdBody);
		}
	}

	void OnEventResArtifactRoomNextFloorChallenge(int nReturnCode, ArtifactRoomNextFloorChallengeResponseBody responseBody) // 층 변경.
	{
		if (nReturnCode == 0)
		{
			m_csArtifactRoomFloor = ArtifactRoom.GetArtifactRoomFloor(ArtifactRoom.ArtifactRoomCurrentFloor);

			if (EventArtifactRoomNextFloorChallenge != null)
			{
				EventArtifactRoomNextFloorChallenge();
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전이 클리어되지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00502"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomInit() // 고대유물의방초기화
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomInitCommandBody cmdBody = new ArtifactRoomInitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomInit, cmdBody);
		}
	}

	void OnEventResArtifactRoomInit(int nReturnCode, ArtifactRoomInitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ArtifactRoom.ArtifactRoomInitCountDate = responseBody.date;
			ArtifactRoom.ArtifactRoomDailyInitCount = responseBody.dailyInitCount;
			ArtifactRoom.ArtifactRoomCurrentFloor = responseBody.currentFloor;
			m_csArtifactRoomFloor = ArtifactRoom.GetArtifactRoomFloor(ArtifactRoom.ArtifactRoomCurrentFloor);

			if (EventArtifactRoomInit != null)
			{
				EventArtifactRoomInit();
			}
		}
		else if (nReturnCode == 101)
		{
			// 소탕중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 초기화횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00602"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomSweep()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomSweepCommandBody cmdBody = new ArtifactRoomSweepCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomSweep, cmdBody);
		}
	}

	void OnEventResArtifactRoomSweep(int nReturnCode, ArtifactRoomSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ArtifactRoom.ArtifactRoomSweepProgressFloor = ArtifactRoom.ArtifactRoomCurrentFloor;
			ArtifactRoom.ArtifactRoomSweepRemainingTime = responseBody.remainingTime;

			if (EventArtifactRoomSweep != null)
			{
				EventArtifactRoomSweep();
			}
		}
		else if (nReturnCode == 101)
		{
			// 소탕할 층이 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 소탕중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00702"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomSweepAccelerate() // 고대유물의방소탕가속
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomSweepAccelerateCommandBody cmdBody = new ArtifactRoomSweepAccelerateCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomSweepAccelerate, cmdBody);
		}
	}

	void OnEventResArtifactRoomSweepAccelerate(int nReturnCode, ArtifactRoomSweepAccelerateResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ArtifactRoom.ArtifactRoomSweepProgressFloor = 0;
			ArtifactRoom.ArtifactRoomCurrentFloor = responseBody.currentFloor;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			if (EventArtifactRoomSweepAccelerate != null)
			{
				EventArtifactRoomSweepAccelerate(responseBody.booties);
			}
		}
		else if (nReturnCode == 101)
		{
			// 소탕중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 다이아가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00802"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomSweepComplete() // 고대유물의방소탕완료
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomSweepCompleteCommandBody cmdBody = new ArtifactRoomSweepCompleteCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomSweepComplete, cmdBody);
		}
	}

	void OnEventResArtifactRoomSweepComplete(int nReturnCode, ArtifactRoomSweepCompleteResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ArtifactRoom.ArtifactRoomSweepProgressFloor = 0;
			ArtifactRoom.ArtifactRoomCurrentFloor = responseBody.currentFloor;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			if (EventArtifactRoomSweepComplete != null)
			{
				EventArtifactRoomSweepComplete(responseBody.booties);
			}
		}
		else if (nReturnCode == 101)
		{
			// 소탕중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00901"));
		}
		else if (nReturnCode == 102)
		{
			// 소탕시간이 완료되지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_00902"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendArtifactRoomSweepStop() // 고대유물의방소탕중지
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ArtifactRoomSweepStopCommandBody cmdBody = new ArtifactRoomSweepStopCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.ArtifactRoomSweepStop, cmdBody);
		}
	}

	void OnEventResArtifactRoomSweepStop(int nReturnCode, ArtifactRoomSweepStopResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			ArtifactRoom.ArtifactRoomSweepProgressFloor = 0;
			ArtifactRoom.ArtifactRoomCurrentFloor = responseBody.currentFloor;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			if (EventArtifactRoomSweepStop != null)
			{
				EventArtifactRoomSweepStop(responseBody.booties);
			}
		}
		else if (nReturnCode == 101)
		{
			// 소탕중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A47_ERROR_01001"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion ArtifactRoom.Protocol.Command

	#region ArtifactRoom.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomStart(SEBArtifactRoomStartEventBody eventBody)
	{
		if (EventArtifactRoomStart != null)
		{
			EventArtifactRoomStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomClear(SEBArtifactRoomClearEventBody eventBody) // 고대유물의방완료
	{
		int nNextFloor = CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor + 1;
		if (CsGameData.Instance.ArtifactRoom.GetArtifactRoomFloor(nNextFloor) != null)
		{
			CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor = nNextFloor;
			if (CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor > CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor)
			{
				CsGameData.Instance.ArtifactRoom.ArtifactRoomBestFloor = CsGameData.Instance.ArtifactRoom.ArtifactRoomCurrentFloor;
			}
		}
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		DungeonResult(true);
		if (EventArtifactRoomClear != null)
		{
			EventArtifactRoomClear(eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomFail(SEBArtifactRoomFailEventBody eventBody)
	{
		DungeonResult(false);
		if (EventArtifactRoomFail != null)
		{
			EventArtifactRoomFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomBanished(SEBArtifactRoomBanishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventArtifactRoomBanished != null)
		{
			EventArtifactRoomBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomBanishedForNextFloorChallenge(SEBArtifactRoomBanishedForNextFloorChallengeEventBody eventBody)
	{
		m_csArtifactRoomFloor = ArtifactRoom.GetArtifactRoomFloor(ArtifactRoom.ArtifactRoomCurrentFloor);
		if (EventArtifactRoomBanishedForNextFloorChallenge != null)
		{
			EventArtifactRoomBanishedForNextFloorChallenge();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomSweepNextFloorStart(SEBArtifactRoomSweepNextFloorStartEventBody eventBody)
	{
		ArtifactRoom.ArtifactRoomSweepProgressFloor = eventBody.progressFloor;
		if (EventArtifactRoomSweepNextFloorStart != null)
		{
			EventArtifactRoomSweepNextFloorStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtArtifactRoomSweepCompleted(SEBArtifactRoomSweepCompletedEventBody eventBody)
	{
		if (EventArtifactRoomSweepCompleted != null)
		{
			EventArtifactRoomSweepCompleted();
		}
	}

	#endregion ArtifactRoom.Protocol.Event

	#endregion ArtifactRoom

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region AncientRelic


	#region AncientRelic.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicMatchingStart(bool bIsPartyEntrance) // 고대인의유적매칭시작
	{
		Debug.Log("    SendAncientRelicMatchingStart()    bIsPartyEntrance = " + bIsPartyEntrance);
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicMatchingStartCommandBody cmdBody = new AncientRelicMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bIsPartyEntrance;
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicMatchingStart, cmdBody);
		}
	}

	void OnEventResAncientRelicMatchingStart(int nReturnCode, AncientRelicMatchingStartResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicMatchingStart()     ");
		if (nReturnCode == 0)
		{
			m_enAncientRelicState = (EnDungeonMatchingState)responseBody.matchingStatus;
			Debug.Log("OnEventResAncientRelicMatchingStart    >>   m_enAncientRelicState  ===> " + m_enAncientRelicState);
			m_flAncientRelicMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;

			if (EventAncientRelicMatchingStart != null)
			{
				EventAncientRelicMatchingStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 매칭중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 스태미너가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 조과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00106"));
		}

		else if (nReturnCode == 107)
		{
			// 파티중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00107"));
		}
		else if (nReturnCode == 108)
		{
			// 파티장이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00108"));
		}
        else if (nReturnCode == 109)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00109"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicMatchingCancel() // 고대인의유적매칭취소
	{
		Debug.Log("    SendAncientRelicMatchingCancel()     ");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicMatchingCancelCommandBody cmdBody = new AncientRelicMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicMatchingCancel, cmdBody);
		}
	}

	void OnEventResAncientRelicMatchingCancel(int nReturnCode, AncientRelicMatchingCancelResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicMatchingCancel()     ");
		if (nReturnCode == 0)
		{
			m_enAncientRelicState = EnDungeonMatchingState.None;
			m_flAncientRelicMatchingRemainingTime = 0f;

			if (EventAncientRelicMatchingCancel != null)
			{
				EventAncientRelicMatchingCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 매칭중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00201"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicEnter() // 고대인의유적입장
	{
		Debug.Log("    SendAncientRelicEnter()     ");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicEnterCommandBody cmdBody = new AncientRelicEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicEnter, cmdBody);
		}
	}

	List<PDHero> m_listHero = new List<PDHero>();
	void OnEventResAncientRelicEnter(int nReturnCode, AncientRelicEnterResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicEnter()    : "+ nReturnCode);
		if (nReturnCode == 0)
		{
			m_enAncientRelicState = EnDungeonMatchingState.None;
			m_flAncientRelicMatchingRemainingTime = 0f;

			AncientRelic.AncientRelicPlayCountDate = responseBody.date;
			AncientRelic.AncientRelicDailyPlayCount = responseBody.playCount;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			m_csAncientRelicStep = AncientRelic.GetAncientRelicStep(responseBody.stepNo);
			m_bDungeonStart = true;

			if (m_csAncientRelicStep != null)
			{
				m_csAncientRelicStepWave = m_csAncientRelicStep.GetAncientRelicStepWave(responseBody.waveNo);
			}

            m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

			if (responseBody.heroes != null)
			{
				for (int i = 0; i < responseBody.heroes.Length; i++)
				{
					m_listHero.Add(responseBody.heroes[i]);
				}
			}

			if (EventAncientRelicEnter != null)
			{
				EventAncientRelicEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts, responseBody.trapEffectHeroes, responseBody.removedObstacleIds);
			}
		}
		else
		{
			DungeonEnterFail();
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicExit() // 고대인의유적퇴장
	{
		Debug.Log("    SendAncientRelicExit()     ");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicExitCommandBody cmdBody = new AncientRelicExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicExit, cmdBody);
		}
	}

	void OnEventResAncientRelicExit(int nReturnCode, AncientRelicExitResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicExit()     ");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventAncientRelicExit != null)
			{
				EventAncientRelicExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00401"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicAbandon() // 고대인의유적포기
	{
		Debug.Log("    SendAncientRelicAbandon()     ");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicAbandonCommandBody cmdBody = new AncientRelicAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicAbandon, cmdBody);
		}
	}

	void OnEventResAncientRelicAbandon(int nReturnCode, AncientRelicAbandonResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicAbandon()     ");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventAncientRelicAbandon != null)
			{
				EventAncientRelicAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00502"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAncientRelicRevive() // 고대인의유적부활
	{
		Debug.Log("    SendAncientRelicRevive()     ");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AncientRelicReviveCommandBody cmdBody = new AncientRelicReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AncientRelicRevive, cmdBody);
		}
	}

	void OnEventResAncientRelicRevive(int nReturnCode, AncientRelicReviveResponseBody responseBody)
	{
		Debug.Log("    OnEventResAncientRelicRevive()     ");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventAncientRelicRevive != null)
			{
				EventAncientRelicRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 부활대기시간이 경과하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A40_ERROR_00603"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion AncientRelic.Protocol.Command

	#region AncientRelic.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicMatchingStatusChanged(SEBAncientRelicMatchingStatusChangedEventBody eventBody) // 고대인유적매칭상태변경
	{
		Debug.Log("    OnEventEvtAncientRelicMatchingStatusChanged()     ");
		Debug.Log("eventBody.matchingStatus => " + eventBody.matchingStatus);
		m_enAncientRelicState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flAncientRelicMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventAncientRelicMatchingStatusChanged != null)
		{
			EventAncientRelicMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicMatchingRoomBanished(SEBAncientRelicMatchingRoomBanishedEventBody eventBody) // 고대인유적매칭방강퇴
	{
		Debug.Log("    OnEventEvtAncientRelicMatchingRoomBanished()     ");
		m_csAncientRelicStep = null;
		m_csAncientRelicStepWave = null;
		m_enAncientRelicState = EnDungeonMatchingState.None;

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

		if (EventAncientRelicMatchingRoomBanished != null)
		{
			EventAncientRelicMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForAncientRelicEnter(SEBContinentExitForAncientRelicEnterEventBody eventBody) // 고대인유적입장에대한대륙퇴장
	{
		Debug.Log("    OnEventEvtContinentExitForAncientRelicEnter()     ");
		m_flAncientRelicCurrentPoint = 0;
		m_enDungeonPlay = EnDungeonPlay.AncientRelic;
		if (EventContinentExitForAncientRelicEnter != null)
		{
			EventContinentExitForAncientRelicEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicStepStart(SEBAncientRelicStepStartEventBody eventBody) // 고대인유적단계시작
	{
		Debug.Log("    OnEventEvtAncientRelicStepStart()     ");
		m_flAncientRelicCurrentPoint = 0;
		m_csAncientRelicStep = AncientRelic.GetAncientRelicStep(eventBody.stepNo);
		Vector3 vtTargetPos = CsRplzSession.Translate(eventBody.targetPosition);
		
		if (EventAncientRelicStepStart != null)
		{
			EventAncientRelicStepStart(eventBody.removeObstacleId, vtTargetPos, eventBody.targetRadius);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicStepCompleted(SEBAncientRelicStepCompletedEventBody eventBody) // 고대인유적단계완료
	{
		Debug.Log("    OnEventEvtAncientRelicStepCompleted()     ");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		if (EventAncientRelicStepCompleted != null)
		{
			EventAncientRelicStepCompleted(eventBody.booties); // 보상
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicWaveStart(SEBAncientRelicWaveStartEventBody eventBody) // 고대인유적웨이브시작
	{
		Debug.Log("    OnEventEvtAncientRelicWaveStart()     ");
		m_csAncientRelicStepWave = m_csAncientRelicStep.GetAncientRelicStepWave(eventBody.waveNo); // WaveNo 갱신.
		if (EventAncientRelicWaveStart != null)
		{
			EventAncientRelicWaveStart(eventBody.monsterInsts); // 몬스터 생성.
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicClear(SEBAncientRelicClearEventBody eventBody) // 고대인유적완료
	{
		Debug.Log("    OnEventEvtAncientRelicClear()     ");
		m_flAncientRelicCurrentPoint = 0;

		DungeonResult(true);
		if (EventAncientRelicClear != null)
		{
			EventAncientRelicClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicFail(SEBAncientRelicFailEventBody eventBody) // 고대인유적실패
	{
		Debug.Log("    OnEventEvtAncientRelicFail()     ");
		m_flAncientRelicCurrentPoint = 0;

		DungeonResult(false);
		if (EventAncientRelicFail != null)
		{
			EventAncientRelicFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicBanished(SEBAncientRelicBanishedEventBody eventBody) // 고대인유적강퇴
	{
		Debug.Log("    OnEventEvtAncientRelicBanished()     ");

		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		m_flAncientRelicCurrentPoint = 0;

		if (EventAncientRelicBanished != null)
		{
			EventAncientRelicBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicPointUpdated(SEBAncientRelicPointUpdatedEventBody eventBody) // 고대인유적포인트갱신
	{
		Debug.Log("    OnEventEvtAncientRelicPointUpdated()     ");
		m_flAncientRelicCurrentPoint = eventBody.point;

		if (EventAncientRelicPointUpdated != null)
		{
			EventAncientRelicPointUpdated(); // 포인트 획득.
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicTrapActivated(SEBAncientRelicTrapActivatedEventBody eventBody) // 고대인의유적함정활성
	{
		if (EventAncientRelicTrapActivated != null)
		{
			EventAncientRelicTrapActivated(eventBody.id);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicTrapHit(SEBAncientRelicTrapHitEventBody eventBody) // 고대인유적함정적중
	{
		if (EventAncientRelicTrapHit != null)
		{
			EventAncientRelicTrapHit(eventBody.heroId, eventBody.hp, eventBody.damage, eventBody.hpDamage, eventBody.removedAbnormalStateEffects, eventBody.changedAbnormalStateEffectDamageAbsorbShields);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicTrapEffectFinished(SEBAncientRelicTrapEffectFinishedEventBody eventBody) // 고대인유적함정효과종료
	{
		Debug.Log("    OnEventEvtAncientRelicTrapEffectFinished()     ");
		if (EventAncientRelicTrapEffectFinished != null)
		{
			EventAncientRelicTrapEffectFinished(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAncientRelicMatchingRoomPartyEnter(SEBAncientRelicMatchingRoomPartyEnterEventBody eventBody)
	{
		Debug.Log("    OnEventEvtAncientRelicMatchingRoomPartyEnter()     ");
		m_enAncientRelicState = (EnDungeonMatchingState)eventBody.matchingStatus;
		Debug.Log("eventBody.matchingStatus => " + eventBody.matchingStatus);
		m_flAncientRelicMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventAncientRelicMatchingRoomPartyEnter != null)
		{
			EventAncientRelicMatchingRoomPartyEnter();
		}
	}

	#endregion AncientRelic.Protocol.Event

	#endregion AncientRelic

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region FieldOfHonor
	#region FieldOfHonor.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendFieldOfHonorInfo()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FieldOfHonorInfoCommandBody cmdBody = new FieldOfHonorInfoCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorInfo, cmdBody);
		}
	}

	void OnEventResFieldOfHonorInfo(int nReturnCode, FieldOfHonorInfoResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			FieldOfHonor.FieldOfHonorPlayCountDate = responseBody.date;
			FieldOfHonor.MyRanking = responseBody.myRanking;
			FieldOfHonor.SuccessiveCount = responseBody.successiveCount;
			FieldOfHonor.MyRanking = responseBody.myRanking;
			FieldOfHonor.FieldOfHonorDailyPlayCount = responseBody.playCount;

			if (EventFieldOfHonorInfo != null)
			{
				EventFieldOfHonorInfo(responseBody.histories, responseBody.matchedRankings);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00101"));
		}
        else if (nReturnCode == 102)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00102"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendOnEventResFieldOfHonorTopRankingList()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FieldOfHonorTopRankingListCommandBody cmdBody = new FieldOfHonorTopRankingListCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorTopRankingList, cmdBody);
		}
	}

	void OnEventResFieldOfHonorTopRankingList(int nReturnCode, FieldOfHonorTopRankingListResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			FieldOfHonor.MyRanking = responseBody.myRanking;
			if (EventFieldOfHonorTopRankingList != null)
			{
				EventFieldOfHonorTopRankingList(responseBody.rankings);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForFieldOfHonorChallenge(int nTargetRanking)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForFieldOfHonorChallengeCommandBody cmdBody = new ContinentExitForFieldOfHonorChallengeCommandBody();
			cmdBody.targetRanking = nTargetRanking;
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForFieldOfHonorChallenge, cmdBody);
		}
	}

	void OnEventResContinentExitForFieldOfHonorChallenge(int nReturnCode, ContinentExitForFieldOfHonorChallengeResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_enDungeonPlay = EnDungeonPlay.FieldOfHonor;
			if (EventContinentExitForFieldOfHonorChallenge != null)
			{
				EventContinentExitForFieldOfHonorChallenge();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상 랭킹이 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 자기자신에 도전할 수 없습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00403"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00404"));
		}
		else if (nReturnCode == 105)
		{
			// 영우이 전투 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00405"));
		}
		else if (nReturnCode == 106)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00406"));
		}
		else if (nReturnCode == 107)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00407"));
		}
		else if (nReturnCode == 108)
		{
			// 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00408"));
		}
        else if (nReturnCode == 109)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00409"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFieldOfHonorChallenge()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FieldOfHonorChallengeCommandBody cmdBody = new FieldOfHonorChallengeCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorChallenge, cmdBody);
		}
	}

	void OnEventResFieldOfHonorChallenge(int nReturnCode, FieldOfHonorChallengeResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			FieldOfHonor.FieldOfHonorDailyPlayCount = responseBody.playCount;
			FieldOfHonor.FieldOfHonorPlayCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_pDHeroFieldOfHonor = responseBody.targetHero;
			m_bDungeonStart = true;

			if (EventFieldOfHonorChallenge != null)
			{
				EventFieldOfHonorChallenge(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.targetHero);
			}
		}
		else
		{
			DungeonEnterFail();
			if (nReturnCode == 101)
			{

			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFieldOfHonorExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FieldOfHonorExitCommandBody cmdBody = new FieldOfHonorExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorExit, cmdBody);
		}
	}

	void OnEventResFieldOfHonorExit(int nReturnCode, FieldOfHonorExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			if (EventFieldOfHonorExit != null)
			{
				EventFieldOfHonorExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
        {
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00601"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFieldOfHonorAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FieldOfHonorAbandonCommandBody cmdBody = new FieldOfHonorAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorAbandon, cmdBody);
		}
	}

	void OnEventResFieldOfHonorAbandon(int nReturnCode, FieldOfHonorAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.HonorPoint = responseBody.honorPoint;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventFieldOfHonorAbandon != null)
			{
				EventFieldOfHonorAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00701"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFieldOfHonorRankingRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FieldOfHonorRankingRewardReceiveCommandBody cmdBody = new FieldOfHonorRankingRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorRankingRewardReceive, cmdBody);
		}
	}

	void OnEventResFieldOfHonorRankingRewardReceive(int nReturnCode, FieldOfHonorRankingRewardReceiveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.FieldOfHonor.RewardedDailyFieldOfHonorRankingNo = responseBody.rewardedRankingNo;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (EventFieldOfHonorRankingRewardReceive != null)
			{
				EventFieldOfHonorRankingRewardReceive(responseBody.booties);
			}
		}
		else if (nReturnCode == 101)
		{
			// 랭킹이 순위밖입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 보상을 받았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A31_ERROR_00802"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendEventResFieldOfHonorRankerInfo(Guid giHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FieldOfHonorRankerInfoCommandBody cmdBody = new FieldOfHonorRankerInfoCommandBody();
			cmdBody.heroId = giHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.FieldOfHonorRankerInfo, cmdBody);
		}
	}

	void OnEventResFieldOfHonorRankerInfo(int nReturnCode, FieldOfHonorRankerInfoResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			if (EventFieldOfHonorRankerInfo != null)
			{
				EventFieldOfHonorRankerInfo(responseBody.ranker);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion FieldOfHonor.Protocol.Command

	#region FieldOfHonor.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFieldOfHonorStart(SEBFieldOfHonorStartEventBody eventBody)
	{
		if (EventFieldOfHonorStart != null)
		{
			EventFieldOfHonorStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFieldOfHonorClear(SEBFieldOfHonorClearEventBody eventBody)
	{
		int nOldHonorPoint = CsGameData.Instance.MyHeroInfo.HonorPoint;

		FieldOfHonor.MyRanking = eventBody.myRanking;
		CsGameData.Instance.MyHeroInfo.HonorPoint = eventBody.honorPoint;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(true);
		if (EventFieldOfHonorClear != null)
		{
			EventFieldOfHonorClear(bLevelUp, eventBody.acquiredExp, eventBody.honorPoint - nOldHonorPoint);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFieldOfHonorFail(SEBFieldOfHonorFailEventBody eventBody)
	{
		int nOldHonorPoint = CsGameData.Instance.MyHeroInfo.HonorPoint;

		FieldOfHonor.SuccessiveCount = eventBody.successiveCount;
		CsGameData.Instance.MyHeroInfo.HonorPoint = eventBody.honorPoint;

		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(false);
		if (EventFieldOfHonorFail != null)
		{
			EventFieldOfHonorFail(bLevelUp, eventBody.acquiredExp, eventBody.honorPoint - nOldHonorPoint);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFieldOfHonorBanished(SEBFieldOfHonorBanishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventFieldOfHonorBanished != null)
		{
			EventFieldOfHonorBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFieldOfHonorDailyRankingUpdated(SEBFieldOfHonorDailyRankingUpdatedEventBody eventBody)
	{
		CsGameData.Instance.FieldOfHonor.DailyFieldOfHonorRankingNo = eventBody.rankingNo;
		CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking = eventBody.ranking;

		if (EventFieldOfHonorDailyRankingUpdated != null)
		{
			EventFieldOfHonorDailyRankingUpdated();
		}
	}

	#endregion FieldOfHonor.Protocol.Event

	#endregion FieldOfHonor

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region SoulCoveter

	#region SoulCoveter.Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterMatchingStart(bool bIsPartyEntrance, int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterMatchingStart  ############");
			m_bWaitResponse = true;

			SoulCoveterMatchingStartCommandBody cmdBody = new SoulCoveterMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bIsPartyEntrance;
			cmdBody.difficulty = nDifficulty;
			m_csSoulCoveterDifficulty = CsGameData.Instance.SoulCoveter.GetSoulCoveterDifficulty(nDifficulty);

			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterMatchingStart, cmdBody);
		}
	}

	void OnEventResSoulCoveterMatchingStart(int nReturnCode, SoulCoveterMatchingStartResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterMatchingStart  ############");
		if (nReturnCode == 0)
		{
			m_enSoulCoveterMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;
			Debug.Log("m_enSoulCoveterMatchingState  = " + m_enSoulCoveterMatchingState);
			m_flSoulCoveterMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;

			if (EventSoulCoveterMatchingStart != null)
			{
				EventSoulCoveterMatchingStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 파티중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 파티장이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 현재 매칭중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 영웅이 스태미너가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 입장횟수가 초과되었습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00107"));
		}
        else if (nReturnCode == 108)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00108"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterMatchingCancel  ############");
			m_bWaitResponse = true;

			SoulCoveterMatchingCancelCommandBody cmdBody = new SoulCoveterMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterMatchingCancel, cmdBody);
		}
	}

	void OnEventResSoulCoveterMatchingCancel(int nReturnCode, SoulCoveterMatchingCancelResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterMatchingCancel  ############");
		if (nReturnCode == 0)
		{
			m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;
			Debug.Log("m_enSoulCoveterMatchingState  = " + m_enSoulCoveterMatchingState);
			m_flSoulCoveterMatchingRemainingTime = 0f;
			if (EventSoulCoveterMatchingCancel != null)
			{
				EventSoulCoveterMatchingCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 매칭중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00201"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterEnter  ############");
			m_bWaitResponse = true;
			SoulCoveterEnterCommandBody cmdBody = new SoulCoveterEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterEnter, cmdBody);
		}
	}

	void OnEventResSoulCoveterEnter(int nReturnCode, SoulCoveterEnterResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterEnter  ############");
		if (nReturnCode == 0)
		{
			m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;
			m_flSoulCoveterMatchingRemainingTime = 0f;

			SoulCoveter.SoulCoveterPlayCountDate = responseBody.date;
			SoulCoveter.SoulCoveterWeeklyPlayCount = responseBody.playCount;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			// 누적플레이횟수
			CsAccomplishmentManager.Instance.AccSoulCoveterPlayCount = responseBody.accPlayCount;

			m_csSoulCoveterDifficultyWave = m_csSoulCoveterDifficulty.GetSoulCoveterDifficultyWave(responseBody.waveNo);
			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;
			m_bDungeonStart = true;

			if (responseBody.heroes != null)
			{
				for (int i = 0; i < responseBody.heroes.Length; i++)
				{
					m_listHero.Add(responseBody.heroes[i]);
				}
			}

			if (EventSoulCoveterEnter != null)
			{
				EventSoulCoveterEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else
		{
			DungeonEnterFail();
			if (nReturnCode == 101)
			{
				// 던전 상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterExit()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterExit  ############");
			m_bWaitResponse = true;

			SoulCoveterExitCommandBody cmdBody = new SoulCoveterExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterExit, cmdBody);
		}
	}
	void OnEventResSoulCoveterExit(int nReturnCode, SoulCoveterExitResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterExit  ############");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventSoulCoveterExit != null)
			{
				EventSoulCoveterExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전 상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00401"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterAbandon  ############");
			m_bWaitResponse = true;

			SoulCoveterAbandonCommandBody cmdBody = new SoulCoveterAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterAbandon, cmdBody);
		}
	}
	void OnEventResSoulCoveterAbandon(int nReturnCode, SoulCoveterAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterAbandon  ############");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventSoulCoveterAbandon != null)
			{
				EventSoulCoveterAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전 상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00702"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSoulCoveterRevive()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSoulCoveterRevive  ############");
			m_bWaitResponse = true;
			SoulCoveterReviveCommandBody cmdBody = new SoulCoveterReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SoulCoveterRevive, cmdBody);
		}
	}
	void OnEventResSoulCoveterRevive(int nReturnCode, SoulCoveterReviveResponseBody responseBody)
	{
		Debug.Log("OnEventResSoulCoveterRevive  ############");
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventSoulCoveterRevive != null)
			{
				EventSoulCoveterRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 죽은 상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 부활대기 시간이 경과하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00802"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A74_ERROR_00803"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion SoulCoveter.Protocol.Command

	#region SoulCoveter.Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterMatchingStatusChanged(SEBSoulCoveterMatchingStatusChangedEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterMatchingStatusChanged()     ");
		Debug.Log("eventBody.matchingStatus => " + eventBody.matchingStatus);
		m_enSoulCoveterMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flSoulCoveterMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventSoulCoveterMatchingStatusChanged != null)
		{
			EventSoulCoveterMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterMatchingRoomBanished(SEBSoulCoveterMatchingRoomBanishedEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterMatchingRoomBanished()     ");
		m_csSoulCoveterDifficulty = null;
		m_csSoulCoveterDifficultyWave = null;
		m_enSoulCoveterMatchingState = EnDungeonMatchingState.None;

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

		if (EventSoulCoveterMatchingRoomBanished != null)
		{
			EventSoulCoveterMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterMatchingRoomPartyEnter(SEBSoulCoveterMatchingRoomPartyEnterEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterMatchingRoomPartyEnter()     ");
		m_enSoulCoveterMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flSoulCoveterMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_csSoulCoveterDifficulty = CsGameData.Instance.SoulCoveter.GetSoulCoveterDifficulty(eventBody.difficulty);
		if (EventSoulCoveterMatchingRoomPartyEnter != null)
		{
			EventSoulCoveterMatchingRoomPartyEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForSoulCoveterEnter(SEBContinentExitForSoulCoveterEnterEventBody eventBody)
	{
		Debug.Log("    OnEventEvtContinentExitForSoulCoveterEnter()     ");
		m_enDungeonPlay = EnDungeonPlay.SoulCoveter;
		if (EventContinentExitForSoulCoveterEnter != null)
		{
			EventContinentExitForSoulCoveterEnter();
		}
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterWaveStart(SEBSoulCoveterWaveStartEventBody eventBody)
	{
		Debug.Log("m_csSoulCoveterDifficultyWave == " + eventBody.waveNo);
		m_csSoulCoveterDifficultyWave = m_csSoulCoveterDifficulty.GetSoulCoveterDifficultyWave(eventBody.waveNo);

		if (EventSoulCoveterWaveStart != null)
		{
			EventSoulCoveterWaveStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterClear(SEBSoulCoveterClearEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterClear()     ");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		DungeonResult(true);
		if (EventSoulCoveterClear != null)
		{
			EventSoulCoveterClear(eventBody.booties);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterFail(SEBSoulCoveterFailEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterFail()     ");
		DungeonResult(false);
		if (EventSoulCoveterFail != null)
		{
			EventSoulCoveterFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSoulCoveterBanished(SEBSoulCoveterBanishedEventBody eventBody)
	{
		Debug.Log("    OnEventEvtSoulCoveterBanished()     ");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
		if (EventSoulCoveterBanished != null)
		{
			EventSoulCoveterBanished(eventBody.previousContinentId);
		}
	}

	#endregion SoulCoveter.Protocol.Event

	#endregion SoulCoveter

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region EliteDungeon
	#region EliteDungeon.Command
	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForEliteDungeonEnter(int nEliteMonsterMasterId)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendContinentExitForEliteDungeonEnter  ############");
			m_bWaitResponse = true;
			ContinentExitForEliteDungeonEnterCommandBody cmdBody = new ContinentExitForEliteDungeonEnterCommandBody();
			cmdBody.eliteMonsterMasterId = nEliteMonsterMasterId;
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForEliteDungeonEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForEliteDungeonEnter(int nReturnCode, ContinentExitForEliteDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResContinentExitForEliteDungeonEnter()  ############");
			m_enDungeonPlay = EnDungeonPlay.Elite;
			if (EventContinentExitForEliteDungeonEnter != null)
			{
				EventContinentExitForEliteDungeonEnter();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 입장횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00104"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	public void SendEliteDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendEliteDungeonEnter  ############");
			m_bWaitResponse = true;
			EliteDungeonEnterCommandBody cmdBody = new EliteDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.EliteDungeonEnter, cmdBody);
		}
	}

	void OnEventResEliteDungeonEnter(int nReturnCode, EliteDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResEliteDungeonEnter()  ############");

			CsEliteManager.Instance.EliteDungeonPlayCountDate = responseBody.date;
			CsEliteManager.Instance.DailyEliteDungeonPlayCount = responseBody.playCount;
			CsGameData.Instance.MyHeroInfo.LocationId = EliteDungeon.Location.LocationId;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_bDungeonStart = true;

			for (int i = 0; i < responseBody.monsters.Length; i++)
			{
				if (responseBody.monsters[i] != null)
				{
					int nEliteMonsterId = responseBody.monsters[i].eliteMonsterId;
					m_csEliteMonster = CsGameData.Instance.GetEliteMonster(nEliteMonsterId);
					break;
				}
			}

			if (EventEliteDungeonEnter != null)
			{
				EventEliteDungeonEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.monsters);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendEliteDungeonExit()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendEliteDungeonExit  ############");
			m_bWaitResponse = true;

			EliteDungeonExitCommandBody cmdBody = new EliteDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.EliteDungeonExit, cmdBody);
		}
	}

	void OnEventResEliteDungeonExit(int nReturnCode, EliteDungeonExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResEliteDungeonExit()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventEliteDungeonExit != null)
			{
				EventEliteDungeonExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00301"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendEliteDungeonAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendEliteDungeonAbandon  ############");
			m_bWaitResponse = true;

			EliteDungeonAbandonCommandBody cmdBody = new EliteDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.EliteDungeonAbandon, cmdBody);
		}
	}

	void OnEventResEliteDungeonAbandon(int nReturnCode, EliteDungeonAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResEliteDungeonAbandon()  ############");
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (EventEliteDungeonAbandon != null)
			{
				EventEliteDungeonAbandon(responseBody.previousContinentId);
			}

		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00402"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendEliteDungeonRevive()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendEliteDungeonRevive  ############");
			m_bWaitResponse = true;

			EliteDungeonReviveCommandBody cmdBody = new EliteDungeonReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.EliteDungeonRevive, cmdBody);
		}
	}

	void OnEventResEliteDungeonRevive(int nReturnCode, EliteDungeonReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResEliteDungeonRevive()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventEliteDungeonRevive != null)
			{
				EventEliteDungeonRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A87_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			// 던전상태가 유효하지 않습니다.
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	#endregion EliteDungeon.Command

	#region EliteDungeon.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteDungeonStart(SEBEliteDungeonStartEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtEliteDungeonStart ####");
		if (EventEliteDungeonStart != null)
		{
			EventEliteDungeonStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteDungeonClear(SEBEliteDungeonClearEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtEliteDungeonClear ####");
		DungeonResult(true);
		if (EventEliteDungeonClear != null)
		{
			EventEliteDungeonClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteDungeonFail(SEBEliteDungeonFailEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtEliteDungeonFail ####");
		DungeonResult(false);
		if (EventEliteDungeonFail != null)
		{
			EventEliteDungeonFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtEliteDungeonBanished(SEBEliteDungeonBanishedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtEliteDungeonBanished ####");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;
		if (EventEliteDungeonBanished != null)
		{
			EventEliteDungeonBanished(eventBody.previousContinentId);
		}
	}

	#endregion EliteDungeon.Event

	#endregion EliteDungeon

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region ProofOfValor

	public void ProofOfValorBuffBoxAcquire(long lBuffBoxInstanceId, CsProofOfValorBuffBox csProofOfValorBuffBox)
	{
		m_csProofOfValorBuffBox = csProofOfValorBuffBox;

		SendProofOfValorBuffBoxAcquire(lBuffBoxInstanceId);
	}

	#region ProofOfValor.Command
	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForProofOfValorEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendContinentExitForProofOfValorEnter  ############");
			m_bWaitResponse = true;
			ContinentExitForProofOfValorEnterCommandBody cmdBody = new ContinentExitForProofOfValorEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForProofOfValorEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForProofOfValorEnter(int nReturnCode, ContinentExitForProofOfValorEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResContinentExitForProofOfValorEnter()  ############");
			m_enDungeonPlay = EnDungeonPlay.ProofOfValor;
			if (EventContinentExitForProofOfValorEnter != null)
			{
				EventContinentExitForProofOfValorEnter();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00105"));
		}
        else if (nReturnCode == 106)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00106"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendProofOfValorEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorEnter  ############");
			m_bWaitResponse = true;
			Debug.Log("Current LocationId == " + CsGameData.Instance.MyHeroInfo.LocationId);
			ProofOfValorEnterCommandBody cmdBody = new ProofOfValorEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorEnter, cmdBody);
		}
	}

	void OnEventResProofOfValorEnter(int nReturnCode, ProofOfValorEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResProofOfValorEnter()  ############");

			ProofOfValor.DailyPlayCount = responseBody.playCount;
			ProofOfValor.DailyPlayCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.LocationId = ProofOfValor.Location.LocationId;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_bDungeonStart = true;

			if (EventProofOfValorEnter != null)
			{
				EventProofOfValorEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.monsters);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendProofOfValorExit()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorExit  ############");
			m_bWaitResponse = true;

			ProofOfValorExitCommandBody cmdBody = new ProofOfValorExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorExit, cmdBody);
		}
	}

	void OnEventResProofOfValorExit(int nReturnCode, ProofOfValorExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResProofOfValorExit()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventProofOfValorExit != null)
			{
				EventProofOfValorExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00301"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendProofOfValorAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorAbandon  ############");
			m_bWaitResponse = true;

			ProofOfValorAbandonCommandBody cmdBody = new ProofOfValorAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorAbandon, cmdBody);
		}
	}

	void OnEventResProofOfValorAbandon(int nReturnCode, ProofOfValorAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResProofOfValorAbandon()  ############");

			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			ProofOfValor.BossMonsterArrangeId = responseBody.heroProofOfValorInst.bossMonsterArrangeId;
			ProofOfValor.CreatureCardId = responseBody.heroProofOfValorInst.creatureCardId;
			ProofOfValor.PaidRefreshCount = responseBody.proofOfValorPaidRefreshCount;
			CsGameData.Instance.MyHeroInfo.SoulPowder = responseBody.soulPowder;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventProofOfValorAbandon != null)
			{
				EventProofOfValorAbandon(responseBody.previousContinentId, bLevelUp, responseBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00402"));
		}
		else
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendProofOfValorSweep()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorSweep  ############");
			m_bWaitResponse = true;

			ProofOfValorSweepCommandBody cmdBody = new ProofOfValorSweepCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorSweep, cmdBody);
		}
	}

	void OnEventResProofOfValorSweep(int nReturnCode, ProofOfValorSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = responseBody.date;

			CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = responseBody.freeSweepDailyCount;
			if (responseBody.changedInventorySlot != null)
			{
				PDInventorySlot[] arrInventorySlot = { responseBody.changedInventorySlot };
				CsGameData.Instance.MyHeroInfo.AddInventorySlots(arrInventorySlot);
			}

			ProofOfValor.DailyPlayCountDate = responseBody.date;
			ProofOfValor.DailyPlayCount = responseBody.playCount;
			ProofOfValor.BossMonsterArrangeId = responseBody.heroProofOfValorInst.bossMonsterArrangeId;
			ProofOfValor.CreatureCardId = responseBody.heroProofOfValorInst.creatureCardId;
			ProofOfValor.PaidRefreshCount = responseBody.proofOfValorPaidRefreshCount;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;
			CsGameData.Instance.MyHeroInfo.SoulPowder = responseBody.soulPowder;

			CsCreatureCardManager.Instance.AddHeroCreatureCard(responseBody.changedCreatureCard);

			if (EventProofOfValorGetCreatureCard != null)
			{
				EventProofOfValorGetCreatureCard(CsCreatureCardManager.Instance.GetHeroCreatureCard(responseBody.changedCreatureCard.creatureCardId));
			}
			
			if (EventProofOfValorSweep != null)
			{
				EventProofOfValorSweep(bLevelUp, responseBody.acquiredExp);
			}

		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			// 소탕아이템이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00503"));
		}
		else if (nReturnCode == 104)
		{
			// 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00504"));
		}
		else if (nReturnCode == 105)
		{
			// 입장횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00505"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendProofOfValorRefresh()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorRefresh  ############");
			m_bWaitResponse = true;

			ProofOfValorRefreshCommandBody cmdBody = new ProofOfValorRefreshCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorRefresh, cmdBody);
		}
	}

	void OnEventResProofOfValorRefresh(int nReturnCode, ProofOfValorRefreshResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			ProofOfValor.DailyPlayCountDate = responseBody.date;
			ProofOfValor.MyDailyFreeRefreshCount = responseBody.dailyProofOfValorFreeRefreshCount;
			ProofOfValor.MyDailyPaidRefreshCount = responseBody.dailyProofOfValorPaidRefreshCount;
			ProofOfValor.PaidRefreshCount = responseBody.proofOfValorPaidRefreshCount;
			ProofOfValor.BossMonsterArrangeId = responseBody.heroProofOfValorInst.bossMonsterArrangeId;
			ProofOfValor.CreatureCardId = responseBody.heroProofOfValorInst.creatureCardId;

			if (EventProofOfValorRefresh != null)
			{
				EventProofOfValorRefresh();
			}
		}
		else if (nReturnCode == 101)
		{
			// 다이아가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 금일 유료갱신 횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00602"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	// 용맹의증명버프상자획득
	//---------------------------------------------------------------------------------------------------
	void SendProofOfValorBuffBoxAcquire(long lBuffBoxInstanceId)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendProofOfValorBuffBoxAcquire  ############ lBuffBoxInstanceId = " + lBuffBoxInstanceId);
			m_bWaitResponse = true;

			ProofOfValorBuffBoxAcquireCommandBody cmdBody = new ProofOfValorBuffBoxAcquireCommandBody();
			cmdBody.buffBoxInstanceId = m_lBuffBoxInstanceId = lBuffBoxInstanceId;

			CsRplzSession.Instance.Send(ClientCommandName.ProofOfValorBuffBoxAcquire, cmdBody);
		}
	}

	void OnEventResProofOfValorBuffBoxAcquire(int nReturnCode, ProofOfValorBuffBoxAcquireResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
            int nRecoveryHp = responseBody.hp - CsGameData.Instance.MyHeroInfo.Hp;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (m_csProofOfValorBuffBox != null)
			{
				CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

				if (m_csProofOfValorBuffBox.OffenseFactor > 0)
				{
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense), m_csProofOfValorBuffBox.OffenseFactor);
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.MagicalOffense), m_csProofOfValorBuffBox.OffenseFactor);
				}

				if (m_csProofOfValorBuffBox.PhysicalDefenseFactor > 0)
				{
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense), m_csProofOfValorBuffBox.OffenseFactor);
				}
			}
			
			if (EventProofOfValorBuffBoxAcquire != null)
			{
                EventProofOfValorBuffBoxAcquire(nRecoveryHp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 버프상자가 존재하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00702"));
		}
		else if (nReturnCode == 103)
		{
			// 버프상자를 획득할 수 없는 위치입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A89_ERROR_00703"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	#endregion ProofOfValor.Command

	#region ProofOfValor.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorStart(SEBProofOfValorStartEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtProofOfValorStart ####");
		if (EventProofOfValorStart != null)
		{
			EventProofOfValorStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorClear(SEBProofOfValorClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorClear()  ############");

		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.SoulPowder = eventBody.soulPowder;
		CsCreatureCardManager.Instance.AddHeroCreatureCard(eventBody.changedCreatureCard);

		ProofOfValor.ClearGrade = ProofOfValor.ProofOfValorClearGradeList.Find(a => a.ClearGrade == eventBody.clearGrade).ClearGrade;
		ProofOfValor.PaidRefreshCount = eventBody.proofOfValorPaidRefreshCount;
		ProofOfValor.BossMonsterArrangeId = eventBody.heroProofOfValorInst.bossMonsterArrangeId;
		ProofOfValor.CreatureCardId = eventBody.heroProofOfValorInst.creatureCardId;

        if (!ProofOfValor.ProofOfValorCleared)
        {
            ProofOfValor.ProofOfValorCleared = true;
        }

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(true);

		if (EventProofOfValorClear != null)
		{
			EventProofOfValorClear(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorFail(SEBProofOfValorFailEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorFail()  ############");
		CsGameData.Instance.MyHeroInfo.SoulPowder = eventBody.soulPowder;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

		ProofOfValor.PaidRefreshCount = eventBody.proofOfValorPaidRefreshCount;
		ProofOfValor.BossMonsterArrangeId = eventBody.heroProofOfValorInst.bossMonsterArrangeId;
		ProofOfValor.CreatureCardId = eventBody.heroProofOfValorInst.creatureCardId;

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(false);
		if (EventProofOfValorFail != null)
		{
			EventProofOfValorFail(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorBanished(SEBProofOfValorBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorBanished()  ############");

		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventProofOfValorBanished != null)
		{
			EventProofOfValorBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorBuffBoxCreated(SEBProofOfValorBuffBoxCreatedEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorBuffBoxCreated()  ############");

		if (EventProofOfValorBuffBoxCreated != null)
		{
			EventProofOfValorBuffBoxCreated(eventBody.buffBoxInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorBuffBoxLifetimeEnded(SEBProofOfValorBuffBoxLifetimeEndedEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorBuffBoxLifetimeEnded()  ############");

		if (EventProofOfValorBuffBoxLifetimeEnded != null)
		{
			EventProofOfValorBuffBoxLifetimeEnded();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorBuffFinished(SEBProofOfValorBuffFinishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorBuffFinished()  ############");

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventProofOfValorBuffFinished != null)
		{
			EventProofOfValorBuffFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtProofOfValorRefreshed(SEBProofOfValorRefreshedEventBody eventBody)
	{
		Debug.Log("OnEventEvtProofOfValorRefreshed()  ############");
		ProofOfValor.PaidRefreshCount = eventBody.proofOfValorPaidRefreshCount;
		ProofOfValor.BossMonsterArrangeId = eventBody.heroProofOfValorInst.bossMonsterArrangeId;
		ProofOfValor.CreatureCardId = eventBody.heroProofOfValorInst.creatureCardId;

		if (EventProofOfValorRefreshed != null)
		{
			EventProofOfValorRefreshed();
		}
	}

	#endregion ProofOfValor.Event

	#endregion ProofOfValor

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region WisdomTemple

	//---------------------------------------------------------------------------------------------------
	void UpdateWisdomTempleStepState(bool bPuzzleReward = false, bool bReset = false)
	{
		//m_bInteraction = false;
		ChangeInteractionState(EnInteractionState.None);

		if (bReset)
		{
			m_csWisdomTempleStep = null;
			m_csWisdomTemplePuzzle = null;
		}

		if (m_csWisdomTempleStep != null)
		{
			if (m_csWisdomTempleStep.Type == 1 && m_csWisdomTemplePuzzle != null)   // 1. 퍼즐
			{
				if (bPuzzleReward)
				{
					m_enWisdomTempleType = EnWisdomTempleType.PuzzleReward; ;
				}
				else
				{
					if (m_csWisdomTemplePuzzle.PuzzleId == 1)    // 1. 색맞추기
					{
						m_enWisdomTempleType = EnWisdomTempleType.ColorMatching;
					}
					else                                                        // 2. 보물상자찾기
					{
						m_enWisdomTempleType = EnWisdomTempleType.FindTreasureBox;
					}
				}
			}
			else                                                // 2. 퀴즈
			{
				m_enWisdomTempleType = EnWisdomTempleType.Quiz;
			}
		}
		else
		{
			m_enWisdomTempleType = EnWisdomTempleType.None;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ColorMatchingObjectInteraction(long lInstanceId, int nObjectId, float flInteractionDuration)
	{
		dd.d("ColorMatchingObjectInteraction ", lInstanceId, nObjectId, flInteractionDuration);
		m_lInstanceId = lInstanceId;
		m_nObjectId = nObjectId;
		m_flInteractionDuration = flInteractionDuration;

		SendWisdomTempleColorMatchingObjectInteractionStart(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	public void PuzzleRewardObjectInteraction(long lInstanceId, float flInteractionDuration)
	{
		dd.d("PuzzleRewardObjectInteraction ", lInstanceId, flInteractionDuration);
		m_lInstanceId = lInstanceId;
		m_nObjectId = 0;
		m_flInteractionDuration = flInteractionDuration;

		SendWisdomTemplePuzzleRewardObjectInteractionStart(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	public void WisdomTempleColorMatchingObjectCheck()
	{
		dd.d("WisdomTempleColorMatchingObjectCheck");
		if (m_csWisdomTempleStep != null && m_enWisdomTempleType == EnWisdomTempleType.ColorMatching)
		{
			SendWisdomTempleColorMatchingObjectCheck(m_csWisdomTempleStep.StepNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendWisdomTempleObjectInteractionCancel()
	{
		Debug.Log("SendWisdomTempleObjectInteractionCancel  ############");		
		CEBWisdomTempleObjectInteractionCancelEvnetBody csEvt = new CEBWisdomTempleObjectInteractionCancelEvnetBody();
		CsRplzSession.Instance.Send(ClientEventName.WisdomTempleObjectInteractionCancel, csEvt);
	}

	#region WisdomTemple.Command

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전입장을위한대륙퇴장
	public void SendContinentExitForWisdomTempleEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendContinentExitForWisdomTempleEnter  ############");
			m_bWaitResponse = true;
			ContinentExitForWisdomTempleEnterCommandBody cmdBody = new ContinentExitForWisdomTempleEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForWisdomTempleEnter, cmdBody);
		}
	}

	void OnEventResContinentExitForWisdomTempleEnter(int nReturnCode, ContinentExitForWisdomTempleEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResContinentExitForWisdomTempleEnter()  ############");
			m_enDungeonPlay = EnDungeonPlay.WisdomTemple;
			if (EventContinentExitForWisdomTempleEnter != null)
			{
				EventContinentExitForWisdomTempleEnter();
			}
		}
        else if (nReturnCode == 101)
        {
            // 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 입장횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00105"));
        }
        else if (nReturnCode == 106)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A105_ERROR_00106"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전입장
	public void SendWisdomTempleEnter()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleEnter  ############");
			m_bWaitResponse = true;
			WisdomTempleEnterCommandBody cmdBody = new WisdomTempleEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleEnter, cmdBody);
		}
	}

	void OnEventResWisdomTempleEnter(int nReturnCode, WisdomTempleEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleEnter()  ############");

			CsGameData.Instance.WisdomTemple.PlayDate = responseBody.date;
			CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount = responseBody.playCount;

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_bDungeonStart = true;

			if (EventWisdomTempleEnter != null)
			{
				EventWisdomTempleEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퇴장
	public void SendWisdomTempleExit()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleExit  ############");
			m_bWaitResponse = true;
			WisdomTempleExitCommandBody cmdBody = new WisdomTempleExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleExit, cmdBody);
		}
	}

	void OnEventResWisdomTempleExit(int nReturnCode, WisdomTempleExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleExit()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventWisdomTempleExit != null)
			{
				EventWisdomTempleExit(responseBody.previousContinentId);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전포기
	public void SendWisdomTempleAbandon()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleAbandon  ############");
			m_bWaitResponse = true;
			WisdomTempleAbandonCommandBody cmdBody = new WisdomTempleAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleAbandon, cmdBody);
		}
	}

	void OnEventResWisdomTempleAbandon(int nReturnCode, WisdomTempleAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleAbandon()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventWisdomTempleAbandon != null)
			{
				EventWisdomTempleAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기오브젝트상호작용시작
	void SendWisdomTempleColorMatchingObjectInteractionStart(long lInstanceId)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleColorMatchingObjectInteractionStart  ############     lInstanceId = " + lInstanceId);
			m_bWaitResponse = true;
			WisdomTempleColorMatchingObjectInteractionStartCommandBody cmdBody = new WisdomTempleColorMatchingObjectInteractionStartCommandBody();
			cmdBody.instanceId = lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleColorMatchingObjectInteractionStart, cmdBody);
		}
	}

	void OnEventResWisdomTempleColorMatchingObjectInteractionStart(int nReturnCode, WisdomTempleColorMatchingObjectInteractionStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleColorMatchingObjectInteractionStart()  ############");
			ChangeInteractionState(EnInteractionState.interacting);

			if (EventWisdomTempleColorMatchingObjectInteractionStart != null)
			{
				EventWisdomTempleColorMatchingObjectInteractionStart(m_nObjectId);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));

			ChangeInteractionState(EnInteractionState.None);
			if (EventDungeonInteractionStartCancel != null)
			{
				EventDungeonInteractionStartCancel();
			}
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기오브젝트검사
	void SendWisdomTempleColorMatchingObjectCheck(int nStepNo)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleColorMatchingObjectCheck  ############    nStepNo = " + nStepNo);
			m_bWaitResponse = true;
			WisdomTempleColorMatchingObjectCheckCommandBody cmdBody = new WisdomTempleColorMatchingObjectCheckCommandBody();
			cmdBody.stepNo = nStepNo;
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleColorMatchingObjectCheck, cmdBody);
		}
	}

	void OnEventResWisdomTempleColorMatchingObjectCheck(int nReturnCode, WisdomTempleColorMatchingObjectCheckResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleColorMatchingObjectCheck()  ############  colorMatchingPoint = " + responseBody.colorMatchingPoint);

			if (EventWisdomTempleColorMatchingObjectCheck != null)
			{
				EventWisdomTempleColorMatchingObjectCheck(responseBody.createdColorMatchingObjectInsts, responseBody.colorMatchingPoint);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퍼즐보상오브젝트상호작용시작
	void SendWisdomTemplePuzzleRewardObjectInteractionStart(long lInstanceId)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTemplePuzzleRewardObjectInteractionStart  ############");
			m_bWaitResponse = true;
			WisdomTemplePuzzleRewardObjectInteractionStartCommandBody cmdBody = new WisdomTemplePuzzleRewardObjectInteractionStartCommandBody();
			cmdBody.instanceId = lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTemplePuzzleRewardObjectInteractionStart, cmdBody);
		}
	}

	void OnEventResWisdomTemplePuzzleRewardObjectInteractionStart(int nReturnCode, WisdomTemplePuzzleRewardObjectInteractionStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTemplePuzzleRewardObjectInteractionStart()  ############");
			ChangeInteractionState(EnInteractionState.interacting);

			if (EventWisdomTemplePuzzleRewardObjectInteractionStart != null)
			{
				EventWisdomTemplePuzzleRewardObjectInteractionStart();
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));

			if (EventDungeonInteractionStartCancel != null)
			{
				EventDungeonInteractionStartCancel();
			}
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전소탕
	public void SendWisdomTempleSweep()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendWisdomTempleSweep  ############");
			m_bWaitResponse = true;
			WisdomTempleSweepCommandBody cmdBody = new WisdomTempleSweepCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.WisdomTempleSweep, cmdBody);
		}
	}

	void OnEventResWisdomTempleSweep(int nReturnCode, WisdomTempleSweepResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWisdomTempleSweep()  ############");

			CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount = responseBody.playCount;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.FreeSweepCountDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.FreeSweepDailyCount = responseBody.freeSweepDailyCount;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventWisdomTempleSweep != null)
			{
				EventWisdomTempleSweep(bLevelUp, responseBody.acquiredExp, responseBody.booty);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
		m_bWaitResponse = false;
	}

	#endregion WisdomTemple.Command

	#region WisdomTemple.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtWisdomTempleStepStart(SEBWisdomTempleStepStartEventBody eventBody)   // 지혜의신전단계시작
	{
		Debug.Log(" #### OnEventEvtWisdomTempleStepStart ####");
		m_csWisdomTempleStep = WisdomTemple.GetWisdomTempleStep(eventBody.stepNo);
		m_csWisdomTemplePuzzle = WisdomTemple.GetWisdomTemplePuzzle(eventBody.puzzleId);
		UpdateWisdomTempleStepState();

		if (EventWisdomTempleStepStart != null)
		{
			EventWisdomTempleStepStart(eventBody.monsterInsts, eventBody.colorMatchingObjectInsts, eventBody.quizNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전단계완료
	void OnEventEvtWisdomTempleStepCompleted(SEBWisdomTempleStepCompletedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleStepCompleted ####");

		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		UpdateWisdomTempleStepState();
		if (EventWisdomTempleStepCompleted != null)
		{
			EventWisdomTempleStepCompleted(bLevelUp, eventBody.acquiredExp, eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기오브젝트상호작용종료
	void OnEventEvtWisdomTempleColorMatchingObjectInteractionFinished(SEBWisdomTempleColorMatchingObjectInteractionFinishedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleColorMatchingObjectInteractionFinished ####");
		ChangeInteractionState(EnInteractionState.None);

		if (EventWisdomTempleColorMatchingObjectInteractionFinished != null)
		{
			EventWisdomTempleColorMatchingObjectInteractionFinished(eventBody.colorMatchingObjectInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기오브젝트상호작용취소
	void OnEventEvtWisdomTempleColorMatchingObjectInteractionCancel(SEBWisdomTempleColorMatchingObjectInteractionCancelEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleColorMatchingObjectInteractionFinished ####");
		ChangeInteractionState(EnInteractionState.None);

		if (EventWisdomTempleColorMatchingObjectInteractionCancel != null)
		{
			EventWisdomTempleColorMatchingObjectInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기몬스터생성
	void OnEventEvtWisdomTempleColorMatchingMonsterCreated(SEBWisdomTempleColorMatchingMonsterCreatedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleColorMatchingMonsterCreated ####");
		if (EventWisdomTempleColorMatchingMonsterCreated != null)
		{
			EventWisdomTempleColorMatchingMonsterCreated(eventBody.monsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전색맞추기몬스터킬
	void OnEventEvtWisdomTempleColorMatchingMonsterKill(SEBWisdomTempleColorMatchingMonsterKillEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleColorMatchingMonsterKill ####");
		if (EventWisdomTempleColorMatchingMonsterKill != null)
		{
			EventWisdomTempleColorMatchingMonsterKill(eventBody.createdColorMatchingObjectInsts, eventBody.colorMatchingPoint);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전가짜보물상자킬
	void OnEventEvtWisdomTempleFakeTreasureBoxKill(SEBWisdomTempleFakeTreasureBoxKillEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleFakeTreasureBoxKill ####   " + eventBody.existAroundRealTreasureBox);
		if (EventWisdomTempleFakeTreasureBoxKill != null)
		{
			EventWisdomTempleFakeTreasureBoxKill(eventBody.row, eventBody.col, eventBody.existAroundRealTreasureBox);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퍼즐완료
	void OnEventEvtWisdomTemplePuzzleCompleted(SEBWisdomTemplePuzzleCompletedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTemplePuzzleCompleted ####");
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		UpdateWisdomTempleStepState(true, false);
		if (EventWisdomTemplePuzzleCompleted != null)
		{
			EventWisdomTemplePuzzleCompleted(bLevelUp, eventBody.acquiredExp, eventBody.puzzleRewardObjectInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퍼즐보상오브젝트상호작용종료
	void OnEventEvtWisdomTemplePuzzleRewardObjectInteractionFinished(SEBWisdomTemplePuzzleRewardObjectInteractionFinishedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTemplePuzzleRewardObjectInteractionFinished ####");

		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		long lInstanceId = m_lInstanceId;
		ChangeInteractionState(EnInteractionState.None);
		if (EventWisdomTemplePuzzleRewardObjectInteractionFinished != null)
		{
			EventWisdomTemplePuzzleRewardObjectInteractionFinished(eventBody.booty, lInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퍼즐보상오브젝트상호작용취소
	void OnEventEvtWisdomTemplePuzzleRewardObjectInteractionCancel(SEBWisdomTemplePuzzleRewardObjectInteractionCancelEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTemplePuzzleRewardObjectInteractionCancel ####");
		//m_bInteraction = false;
		ChangeInteractionState(EnInteractionState.None);

		if (EventWisdomTemplePuzzleRewardObjectInteractionCancel != null)
		{
			EventWisdomTemplePuzzleRewardObjectInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전퀴즈실패
	void OnEventEvtWisdomTempleQuizFail(SEBWisdomTempleQuizFailEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleQuizFail ####");
		UpdateWisdomTempleStepState(false, true);
		if (EventWisdomTempleQuizFail != null)
		{
			EventWisdomTempleQuizFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전보스몬스터생성
	void OnEventEvtWisdomTempleBossMonsterCreated(SEBWisdomTempleBossMonsterCreatedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleBossMonsterCreated ####");
		if (EventWisdomTempleBossMonsterCreated != null)
		{
			EventWisdomTempleBossMonsterCreated(eventBody.monsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전보스몬스터킬
	void OnEventEvtWisdomTempleBossMonsterKill(SEBWisdomTempleBossMonsterKillEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleBossMonsterKill ####");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (EventWisdomTempleBossMonsterKill != null)
		{
			EventWisdomTempleBossMonsterKill(eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전클리어
	void OnEventEvtWisdomTempleClear(SEBWisdomTempleClearEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleClear ####");

		DungeonResult(true);
		UpdateWisdomTempleStepState(false, true);
		if (EventWisdomTempleClear != null)
		{
			EventWisdomTempleClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전실패
	void OnEventEvtWisdomTempleFail(SEBWisdomTempleFailEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleFail ####");

		DungeonResult(false);
		UpdateWisdomTempleStepState(false, true);
		if (EventWisdomTempleFail != null)
		{
			EventWisdomTempleFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 지혜의신전강퇴
	void OnEventEvtWisdomTempleBanished(SEBWisdomTempleBanishedEventBody eventBody)
	{
		Debug.Log(" #### OnEventEvtWisdomTempleBanished ####");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		UpdateWisdomTempleStepState(false, true);
		if (EventWisdomTempleBanished != null)
		{
			EventWisdomTempleBanished(eventBody.previousContinentId);
		}
	}

	#endregion WisdomTemple.Event

	#endregion WisdomTemple

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region RuinsReclaim

	//---------------------------------------------------------------------------------------------------
	// 던전입장
	public void RuinsReclaimEnter()
	{
		dd.d("RuinsReclaimEnter ", m_bWaitResponse);
		SendRuinsReclaimEnter();
	}

	//---------------------------------------------------------------------------------------------------
	// 포탈입장
	public void RuinsReclaimPortalEnter(int nPortalId)
	{
		dd.d("RuinsReclaimPortalEnter ", m_bWaitResponse, nPortalId);
		SendRuinsReclaimPortalEnter(nPortalId);
	}

	//---------------------------------------------------------------------------------------------------
	// 상호작용시작
	public void RuinsReclaimObjectInteractionStart(CsRuinsReclaimObject csRuinsReclaimObject)
	{
		Debug.Log("RuinsReclaimStartObjectInteraction " + m_bWaitResponse + " , " + csRuinsReclaimObject);

		m_nObjectId = csRuinsReclaimObject.ObjectId;
		m_lInstanceId = csRuinsReclaimObject.InstanceId;

		if (m_csRuinsReclaimStep.Type == 2) // 상호작용.
		{
			m_nArrangeNo = csRuinsReclaimObject.RuinsReclaimObjectArrange.ArrangeNo;
			m_flInteractionDuration = csRuinsReclaimObject.RuinsReclaimObjectArrange.ObjectInteractionDuration;
			SendRuinsReclaimRewardObjectInteractionStart();
		}
		else
		{
			m_flInteractionDuration = csRuinsReclaimObject.RuinsReclaimStepWaveSkill.ObjectInteractionDuration;
			SendRuinsReclaimMonsterTransformationCancelObjectInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 상호작용취소
	void SendRuinsReclaimObjectInteractionCancel()
	{
		Debug.Log("SendRuinsReclaimObjectInteractionCancel  ############");
		CEBRuinsReclaimObjectInteractionCancelEventBody csEvt = new CEBRuinsReclaimObjectInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.RuinsReclaimObjectInteractionCancel, csEvt);
	}

	#region RuinsReclaim.Command

	//---------------------------------------------------------------------------------------------------
	// 유적탈환매칭시작
	public void SendRuinsReclaimMatchingStart(bool bPartyEntrance)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimMatchingStartCommandBody cmdBody = new RuinsReclaimMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bPartyEntrance;
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimMatchingStart, cmdBody);
		}
	}
	void OnEventResRuinsReclaimMatchingStart(int nReturnCode, RuinsReclaimMatchingStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimMatchingStart()  ############  matchingStatus = " + (EnDungeonMatchingState)responseBody.matchingStatus);
			m_enRuinsReclaimMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;
			m_flRuinsReclaimMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;

			if (EventRuinsReclaimMatchingStart != null)
			{
				EventRuinsReclaimMatchingStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 입장가능 시간이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 매칭중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 입장아이템이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			//  카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 파티중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 파티장이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00107"));
		}
        else if (nReturnCode == 108)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00108"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환매칭취소
	public void SendRuinsReclaimMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimMatchingCancelCommandBody cmdBody = new RuinsReclaimMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimMatchingCancel, cmdBody);
		}
	}
	void OnEventResRuinsReclaimMatchingCancel(int nReturnCode, RuinsReclaimMatchingCancelResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimMatchingCancel()  ############");
			m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;
			m_flRuinsReclaimMatchingRemainingTime = 0f;

			if (EventRuinsReclaimMatchingCancel != null)
			{
				EventRuinsReclaimMatchingCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 유적탈환매칭중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00201"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환입장
	void SendRuinsReclaimEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimEnterCommandBody cmdBody = new RuinsReclaimEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimEnter, cmdBody);
		}
	}
	void OnEventResRuinsReclaimEnter(int nReturnCode, RuinsReclaimEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimEnter()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);
			
			RuinsReclaim.FreePlayCount = responseBody.freePlayCount;
			RuinsReclaim.PlayDate = responseBody.date;
			m_flRuinsReclaimMatchingRemainingTime = 0f;

			m_csRuinsReclaimStep = RuinsReclaim.GetRuinsReclaimStep(responseBody.stepNo);
			if (m_csRuinsReclaimStep != null)
			{
				m_csRuinsReclaimStepWave = m_csRuinsReclaimStep.GetRuinsReclaimStepWave(responseBody.waveNo);
			}

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;
			m_bDungeonStart = true;

			if (EventRuinsReclaimEnter != null)
			{
				EventRuinsReclaimEnter(responseBody.placeInstanceId, 
									   responseBody.position,
									   responseBody.rotationY, 
									   responseBody.heroes,
									   responseBody.monsterInsts,
									   responseBody.rewardObjectInsts,
									   responseBody.monsterTransformationCancelObjectInsts,
									   responseBody.monsterTransformationHeroes);
			}
		}
		else
		{
			DungeonEnterFail();
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환퇴장
	public void SendRuinsReclaimExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimExitCommandBody cmdBody = new RuinsReclaimExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimExit, cmdBody);
		}
	}
	void OnEventResRuinsReclaimExit(int nReturnCode, RuinsReclaimExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimExit()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventRuinsReclaimExit != null)
			{
				EventRuinsReclaimExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00401"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환포기
	public void SendRuinsReclaimAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimAbandonCommandBody cmdBody = new RuinsReclaimAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimAbandon, cmdBody);
		}
	}
	void OnEventResRuinsReclaimAbandon(int nReturnCode, RuinsReclaimAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimAbandon()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventRuinsReclaimAbandon != null)
			{
				EventRuinsReclaimAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00501"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환포탈입장
	void SendRuinsReclaimPortalEnter(int nPortalId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimPortalEnterCommandBody cmdBody = new RuinsReclaimPortalEnterCommandBody();
			cmdBody.portalId = m_nPortalId = nPortalId;
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimPortalEnter, cmdBody);
		}
	}
	void OnEventResRuinsReclaimPortalEnter(int nReturnCode, RuinsReclaimPortalEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimPortalEnter()  ############");
			if (EventRuinsReclaimPortalEnter != null)
			{
				EventRuinsReclaimPortalEnter(responseBody.position, responseBody.rotationY);
			}
		}
		else if (nReturnCode == 101)
		{
			// 포탈이 존재하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 입장할수 없는 위치입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 죽은상태입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00603"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00604"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_nPortalId = 0;
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환부활
	public void SendRuinsReclaimRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimReviveCommandBody cmdBody = new RuinsReclaimReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimRevive, cmdBody);
		}
	}
	void OnEventResRuinsReclaimRevive(int nReturnCode, RuinsReclaimReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimRevive()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsIngameData.Instance.MyHeroDead = false;

			if (EventRuinsReclaimRevive != null)
			{
				EventRuinsReclaimRevive(responseBody.position, responseBody.rotationY);
			}
		}
		else if (nReturnCode == 101)
		{
			// 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 죽은상태가 아닙니다
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00702"));
		}
        else if (nReturnCode == 103)
        {
            // 부활대기시간이 경과하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00703"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신취소오브젝트상호작용시작
	void SendRuinsReclaimMonsterTransformationCancelObjectInteractionStart()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimMonsterTransformationCancelObjectInteractionStartCommandBody cmdBody = new RuinsReclaimMonsterTransformationCancelObjectInteractionStartCommandBody();
			cmdBody.instanceId = m_lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimMonsterTransformationCancelObjectInteractionStart, cmdBody);
		}
	}
	void OnEventResRuinsReclaimMonsterTransformationCancelObjectInteractionStart(int nReturnCode, RuinsReclaimMonsterTransformationCancelObjectInteractionStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimMonsterTransformationCancelObjectInteractionStart()  ############");
			ChangeInteractionState(EnInteractionState.interacting);

			if (EventRuinsReclaimMonsterTransformationCancelObjectInteractionStart != null)
			{
				EventRuinsReclaimMonsterTransformationCancelObjectInteractionStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00801"));
			}
			else if (nReturnCode == 102)
			{
				// 던전단계가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00802"));
			}
			else if (nReturnCode == 103)
			{
				// 오브젝트가 이미 상호작용중입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00803"));
			}
			else if (nReturnCode == 104)
			{
				// 영웅이 죽은상태입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00804"));
			}
			else if (nReturnCode == 105)
			{
				//  상호작용할수 없는 위치입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00805"));
			}
			else if (nReturnCode == 106)
			{
				//  영웅이 몬스터변신중이 아닙니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00806"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			if (EventDungeonInteractionStartCancel != null)
			{
				EventDungeonInteractionStartCancel();
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환보상오브젝트상호작용시작
	void SendRuinsReclaimRewardObjectInteractionStart()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			RuinsReclaimRewardObjectInteractionStartCommandBody cmdBody = new RuinsReclaimRewardObjectInteractionStartCommandBody();
			cmdBody.instanceId = m_lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.RuinsReclaimRewardObjectInteractionStart, cmdBody);
		}
	}
	void OnEventResRuinsReclaimRewardObjectInteractionStart(int nReturnCode, RuinsReclaimRewardObjectInteractionStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResRuinsReclaimRewardObjectInteractionStart()  ############");
			ChangeInteractionState(EnInteractionState.interacting);

			if (EventRuinsReclaimRewardObjectInteractionStart != null)
			{
				EventRuinsReclaimRewardObjectInteractionStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00901"));
			}
			else if (nReturnCode == 102)
			{
				// 던전단계가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00902"));
			}
			else if (nReturnCode == 103)
			{
				// 오브젝트가 이미 상호작용중입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00903"));
			}
			else if (nReturnCode == 104)
			{
				// 영웅이 죽은상태입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00904"));
			}
			else if (nReturnCode == 105)
			{
				//  상호작용할수 없는 위치입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A110_ERROR_00905"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			if (EventDungeonInteractionStartCancel != null)
			{
				EventDungeonInteractionStartCancel();
			}
		}

		m_bWaitResponse = false;
	}

	#endregion RuinsReclaim.Command

	#region RuinsReclaim.Event

	//---------------------------------------------------------------------------------------------------
	// 유적탈환입장에대한대륙퇴장
	void OnEventEvtContinentExitForRuinsReclaimEnter(SEBContinentExitForRuinsReclaimEnterEventBody eventBody)
	{
		dd.d("OnEventEvtContinentExitForRuinsReclaimEnter");
		m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;
		m_enDungeonPlay = EnDungeonPlay.RuinsReclaim;

		if (EventContinentExitForRuinsReclaimEnter != null)
		{
			EventContinentExitForRuinsReclaimEnter();
		}

		if (EventMyHeroDungeonEnterMoveStop != null)
		{
			EventMyHeroDungeonEnterMoveStop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환매칭방파티입장
	void OnEventEvtRuinsReclaimMatchingRoomPartyEnter(SEBRuinsReclaimMatchingRoomPartyEnterEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMatchingRoomPartyEnter");
		m_enRuinsReclaimMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flRuinsReclaimMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventRuinsReclaimMatchingRoomPartyEnter != null)
		{
			EventRuinsReclaimMatchingRoomPartyEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환매칭상태변경
	void OnEventEvtRuinsReclaimMatchingStatusChanged(SEBRuinsReclaimMatchingStatusChangedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMatchingStatusChanged         matchingStatus = " + (EnDungeonMatchingState)eventBody.matchingStatus);
		m_enRuinsReclaimMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flRuinsReclaimMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventRuinsReclaimMatchingStatusChanged != null)
		{
			EventRuinsReclaimMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환매칭방강퇴
	void OnEventEvtRuinsReclaimMatchingRoomBanished(SEBRuinsReclaimMatchingRoomBanishedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMatchingRoomBanished");
		m_enRuinsReclaimMatchingState = EnDungeonMatchingState.None;
		m_flRuinsReclaimMatchingRemainingTime = 0f;

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

		if (EventRuinsReclaimMatchingRoomBanished != null)
		{
			EventRuinsReclaimMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환단계시작
	void OnEventEvtRuinsReclaimStepStart(SEBRuinsReclaimStepStartEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimStepStart");
		m_csRuinsReclaimStep = RuinsReclaim.GetRuinsReclaimStep(eventBody.stepNo);

		if (EventRuinsReclaimStepStart != null)
		{
			EventRuinsReclaimStepStart(eventBody.objectInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환단계완료
	void OnEventEvtRuinsReclaimStepCompleted(SEBRuinsReclaimStepCompletedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimStepCompleted");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (EventRuinsReclaimStepCompleted != null)
		{
			EventRuinsReclaimStepCompleted(eventBody.booties);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환보상오브젝트상호작용취소
	void OnEventEvtRuinsReclaimRewardObjectInteractionCancel(SEBRuinsReclaimRewardObjectInteractionCancelEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimRewardObjectInteractionCancel");
		ChangeInteractionState(EnInteractionState.None);

		if (EventRuinsReclaimRewardObjectInteractionCancel != null)
		{
			EventRuinsReclaimRewardObjectInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환보상오브젝트상호작용종료
	void OnEventEvtRuinsReclaimRewardObjectInteractionFinished(SEBRuinsReclaimRewardObjectInteractionFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimRewardObjectInteractionFinished");
		long lObjectInstanceId = m_lInstanceId;
		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

		ChangeInteractionState(EnInteractionState.None);

		if (EventRuinsReclaimRewardObjectInteractionFinished != null)
		{
			EventRuinsReclaimRewardObjectInteractionFinished(eventBody.booty, lObjectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환보상오브젝트상호작용시작 // Other
	void OnEventEvtHeroRuinsReclaimRewardObjectInteractionStart(SEBHeroRuinsReclaimRewardObjectInteractionStartEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimRewardObjectInteractionStart");
		if (EventHeroRuinsReclaimRewardObjectInteractionStart != null)
		{
			EventHeroRuinsReclaimRewardObjectInteractionStart(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환보상오브젝트상호작용취소  // Other
	void OnEventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel(SEBHeroRuinsReclaimRewardObjectInteractionCancelEventBody eventBody)
	{
		dd.d("OnEventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel");
		if (EventHeroRuinsReclaimRewardObjectInteractionCancel != null)
		{
			EventHeroRuinsReclaimRewardObjectInteractionCancel(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환보상오브젝트상호작용종료  // Other
	void OnEventEvtHeroRuinsReclaimRewardObjectInteractionFinished(SEBHeroRuinsReclaimRewardObjectInteractionFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtHeroHeroRuinsReclaimRewardObjectInteractionCancel");
		if (EventHeroRuinsReclaimRewardObjectInteractionFinished != null)
		{
			EventHeroRuinsReclaimRewardObjectInteractionFinished(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환웨이브시작
	void OnEventEvtRuinsReclaimWaveStart(SEBRuinsReclaimWaveStartEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimWaveStart", eventBody.waveNo);
		m_csRuinsReclaimStepWave = m_csRuinsReclaimStep.GetRuinsReclaimStepWave(eventBody.waveNo);

		if (EventRuinsReclaimWaveStart != null)
		{
			EventRuinsReclaimWaveStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환웨이브완료
	void OnEventEvtRuinsReclaimWaveCompleted(SEBRuinsReclaimWaveCompletedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimWaveCompleted");

		if (EventRuinsReclaimWaveCompleted != null)
		{
			EventRuinsReclaimWaveCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환단계웨이브스킬시전
	void OnEventEvtRuinsReclaimStepWaveSkillCast(SEBRuinsReclaimStepWaveSkillCastEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimStepWaveSkillCast");

		if (EventRuinsReclaimStepWaveSkillCast != null)
		{
			EventRuinsReclaimStepWaveSkillCast(eventBody.createdObjectInsts, eventBody.targetPosition);     // 몬스터 변신 취소 오브젝트 생성.
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신시작
	void OnEventEvtRuinsReclaimMonsterTransformationStart(SEBRuinsReclaimMonsterTransformationStartEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterTransformationStart");

		if (EventRuinsReclaimMonsterTransformationStart != null)
		{
			EventRuinsReclaimMonsterTransformationStart(eventBody.transformationMonsterId, eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신종료
	void OnEventEvtRuinsReclaimMonsterTransformationFinished(SEBRuinsReclaimMonsterTransformationFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterTransformationFinished");
		if (EventRuinsReclaimMonsterTransformationFinished != null)
		{
			EventRuinsReclaimMonsterTransformationFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신취소오브젝트수명종료
	void OnEventEvtRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded(SEBRuinsReclaimMonsterTransformationCancelObjectLifetimeEndedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterTransformationFinished          유적탈환몬스터변신취소오브젝트수명종료");
		if (EventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded != null)
		{
			EventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded(eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환몬스터변신시작	// Other
	void OnEventEvtHeroRuinsReclaimMonsterTransformationStart(SEBHeroRuinsReclaimMonsterTransformationStartEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimMonsterTransformationStart");

		if (EventHeroRuinsReclaimMonsterTransformationStart != null)
		{
			EventHeroRuinsReclaimMonsterTransformationStart(eventBody.heroId, eventBody.transformationMonsterId, eventBody.removedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환몬스터변신종료
	void OnEventEvtHeroRuinsReclaimMonsterTransformationFinished(SEBHeroRuinsReclaimMonsterTransformationFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimMonsterTransformationFinished");

		if (EventHeroRuinsReclaimMonsterTransformationFinished != null)
		{
			EventHeroRuinsReclaimMonsterTransformationFinished(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신취소오브젝트상호작용취소
	void OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionCancel(SEBRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionCancel");
		ChangeInteractionState(EnInteractionState.None);

		if (EventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel != null)
		{
			EventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터변신취소오브젝트상호작용종료
	void OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(SEBRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterTransformationCancelObjectInteractionFinished");
		long lObjectInstanceId = m_lInstanceId;
		ChangeInteractionState(EnInteractionState.None);

		if (EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished != null)
		{
			EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(lObjectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환몬스터변신취소오브젝트상호작용시작
	void OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart(SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStartEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart");

		if (EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart != null)
		{
			EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환몬스터변신취소오브젝트상호작용취소
	void OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel(SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancelEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel");

		if (EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel != null)
		{
			EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환몬스터변신취소오브젝트상호작용종료
	void OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(SEBHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished");

		if (EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished != null)
		{
			EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(eventBody.heroId, eventBody.objectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환몬스터소환
	void OnEventEvtRuinsReclaimMonsterSummon(SEBRuinsReclaimMonsterSummonEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimMonsterSummon");

		if (EventRuinsReclaimMonsterSummon != null)
		{
			EventRuinsReclaimMonsterSummon(eventBody.monsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환함정적중
	void OnEventEvtRuinsReclaimTrapHit(SEBRuinsReclaimTrapHitEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimTrapHit");

		if (EventRuinsReclaimTrapHit != null)
		{
			EventRuinsReclaimTrapHit(eventBody.heroId, eventBody.hp, eventBody.damage, eventBody.hpDamage, eventBody.removedAbnormalStateEffects, eventBody.changedAbnormalStateEffectDamageAbsorbShields);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환디버프효과시작
	void OnEventEvtRuinsReclaimDebuffEffectStart(SEBRuinsReclaimDebuffEffectStartEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimDebuffEffectStart");
		
		CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense), RuinsReclaim.DebuffAreaOffenseFactor);
		CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.MagicalOffense), RuinsReclaim.DebuffAreaOffenseFactor);

		if (EventRuinsReclaimDebuffEffectStart != null)
		{
			EventRuinsReclaimDebuffEffectStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환디버프효과중지
	void OnEventEvtRuinsReclaimDebuffEffectStop(SEBRuinsReclaimDebuffEffectStopEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimDebuffEffectStop");
		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventRuinsReclaimDebuffEffectStop != null)
		{
			EventRuinsReclaimDebuffEffectStop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅유적탈환포탈입장	// Other
	void OnEventEvtHeroRuinsReclaimPortalEnter(SEBHeroRuinsReclaimPortalEnterEventBody eventBody)
	{
		dd.d("OnEventEvtHeroRuinsReclaimPortalEnter");

		if (EventHeroRuinsReclaimPortalEnter != null)
		{
			EventHeroRuinsReclaimPortalEnter(eventBody.heroId, eventBody.position, eventBody.rotationY);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환클리어
	void OnEventEvtRuinsReclaimClear(SEBRuinsReclaimClearEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimClear");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		
		if (EventRuinsReclaimClear != null)
		{
			EventRuinsReclaimClear(eventBody.booties, eventBody.randomBooty,
				eventBody.monsterTerminatorHeroId, eventBody.monsterTerminatorHeroName, eventBody.monsterTerminatorBooty,
				eventBody.ultimateAttackKingHeroId, eventBody.ultimateAttackKingHeroName, eventBody.ultimateAttackKingBooty,
				eventBody.partyVolunteerHeroId, eventBody.partyVolunteerHeroName, eventBody.partyVolunteerBooty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환실패
	void OnEventEvtRuinsReclaimFail(SEBRuinsReclaimFailEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimFail");
		
		if (EventRuinsReclaimFail != null)
		{
			EventRuinsReclaimFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 유적탈환강퇴
	void OnEventEvtRuinsReclaimBanished(SEBRuinsReclaimBanishedEventBody eventBody)
	{
		dd.d("OnEventEvtRuinsReclaimBanished");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventRuinsReclaimBanished != null)
		{
			EventRuinsReclaimBanished(eventBody.previousContinentId);
		}
	}

	#endregion RuinsReclaim.Event

	#endregion RuinsReclaim

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region InfiniteWar
	//---------------------------------------------------------------------------------------------------
	public class CsInfiniteWarPointValue
	{
		Guid m_guidHeroId;
        string m_strHeroName;
		int m_nPoint;
		long m_lPointUpdatedTimeTicks;

		public Guid HeroId { get { return m_guidHeroId; } }
        public string HeroName { get { return m_strHeroName; } }
		public int Point { get { return m_nPoint; } set { m_nPoint = value; } }
		public long PointUpdatedTimeTicks { get { return m_lPointUpdatedTimeTicks; } set { m_lPointUpdatedTimeTicks = value; } }

		public CsInfiniteWarPointValue(Guid guidHeroId, string strHeroName, int nPoint, long lPointUpdatedTimeTicks)
		{
			m_guidHeroId = guidHeroId;
            m_strHeroName = strHeroName;
			m_nPoint = nPoint;
			m_lPointUpdatedTimeTicks = lPointUpdatedTimeTicks;
		}
	}

	Dictionary<Guid, CsInfiniteWarPointValue> m_dicInfiniteWarHeroPoint = new Dictionary<Guid, CsInfiniteWarPointValue>();
    public Dictionary<Guid, CsInfiniteWarPointValue> DicInfiniteWarHeroPoint { get { return m_dicInfiniteWarHeroPoint; } }
    
	//---------------------------------------------------------------------------------------------------
	// 던전입장
	public void InfiniteWarEnter()
	{
		dd.d("IniteWarEnter ", m_bWaitResponse);
		SendInfiniteWarEnter();
	}

	//---------------------------------------------------------------------------------------------------
	public void InfiniteWarBuffBoxAcquire(long lInstnaceId, CsInfiniteWarBuffBox csInfiniteWarBuffBox)
	{
		m_csInfiniteWarBuffBox = csInfiniteWarBuffBox;

		dd.d("InfiniteWarBuffBoxAcquire ", m_bWaitResponse, lInstnaceId);
		SendInfiniteWarBuffBoxAcquire(lInstnaceId);

	}

	//---------------------------------------------------------------------------------------------------
	public void SetInfiniteWarPoint(Guid guidHeroId, string strHeroName, int nPoint, long lPointUpdatedTimeTicks, bool bReset = false)
	{
		if (bReset)
		{
			m_dicInfiniteWarHeroPoint.Clear();			
		}
		else
		{
			if (m_dicInfiniteWarHeroPoint.ContainsKey(guidHeroId))
			{
				m_dicInfiniteWarHeroPoint[guidHeroId].Point = nPoint;
				m_dicInfiniteWarHeroPoint[guidHeroId].PointUpdatedTimeTicks = lPointUpdatedTimeTicks;
			}
			else
			{
                m_dicInfiniteWarHeroPoint.Add(guidHeroId, new CsInfiniteWarPointValue(guidHeroId, strHeroName, nPoint, lPointUpdatedTimeTicks));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveInfiniteWarHero(Guid guidHeroId)
	{
		if (m_dicInfiniteWarHeroPoint.ContainsKey(guidHeroId))
		{
			m_dicInfiniteWarHeroPoint.Remove(guidHeroId);			
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ClearInfiniteWarHero()
	{
		m_dicInfiniteWarHeroPoint.Clear();
	}

	#region InfiniteWar.Command

	//---------------------------------------------------------------------------------------------------
	public void SendInfiniteWarMatchingStart()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarMatchingStartCommandBody cmdBody = new InfiniteWarMatchingStartCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarMatchingStart, cmdBody);
        }
    }
    void OnEventResInfiniteWarMatchingStart(int nReturnCode, InfiniteWarMatchingStartResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResInfiniteWarMatchingStart()  ############");
            m_enInfiniteWarMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;

            if (EventInfiniteWarMatchingStart != null)
            {
                EventInfiniteWarMatchingStart();
            }
        }
        else if (nReturnCode == 103)
        {
            // 입장가능 시간이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 매칭중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00105"));
        }
        else if (nReturnCode == 106)
        {
            // 영웅이 카트에 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00106"));
        }
        else if (nReturnCode == 107)
        {
            //  스태미나가 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00107"));
        }
        else if (nReturnCode == 108)
        {
            // 입장횟수가 최대입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00108"));
        }
        else if (nReturnCode == 109)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00109"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendInfiniteWarMatchingCancel()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarMatchingCancelCommandBody cmdBody = new InfiniteWarMatchingCancelCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarMatchingCancel, cmdBody);
        }
    }
    void OnEventResInfiniteWarMatchingCancel(int nReturnCode, InfiniteWarMatchingCancelResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResInfiniteWarMatchingCancel()  ############");
            m_enInfiniteWarMatchingState = EnDungeonMatchingState.None;

            if (EventRuinsReclaimMatchingCancel != null)
            {
                EventRuinsReclaimMatchingCancel();
            }
        }
        else if (nReturnCode == 101)
        {
            // 무한대전매칭중이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00201"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendInfiniteWarEnter()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarEnterCommandBody cmdBody = new InfiniteWarEnterCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarEnter, cmdBody);
        }
    }
	void OnEventResInfiniteWarEnter(int nReturnCode, InfiniteWarEnterResponseBody responseBody)
    {
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResInfiniteWarEnter()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);

			InfiniteWar.PlayDate = responseBody.date;
			InfiniteWar.DailyPlayCount = responseBody.playCount;

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

            Debug.Log("responseBody.remainingStartTime : " + responseBody.remainingStartTime);
            Debug.Log("responseBody.remainingLimitTime : " + responseBody.remainingLimitTime);

			SetInfiniteWarPoint(CsGameData.Instance.MyHeroInfo.HeroId, CsGameData.Instance.MyHeroInfo.Name, 0, 0);

			for (int i = 0; i < responseBody.heroes.Length; i++)
			{
				SetInfiniteWarPoint(responseBody.heroes[i].id, responseBody.heroes[i].name, 0, 0);
			}

			for (int i = 0; i < responseBody.points.Length; i++)
			{
				SetInfiniteWarPoint(responseBody.points[i].heroId, responseBody.points[i].name, responseBody.points[i].point, responseBody.points[i].pointUpdatedTimeTicks);
			}

			if (m_flMultiDungeonRemainingStartTime == 0)	// 이미 시작 한 경우.
			{
				m_bDungeonStart = true;
			}

			if (EventInfiniteWarEnter != null)
			{
				EventInfiniteWarEnter(responseBody.placeInstanceId, 
									  responseBody.position, 
									  responseBody.rotationY, 
									  responseBody.heroes,
									  responseBody.monsterInsts,
									  responseBody.buffBoxInsts);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			DungeonEnterFail();
		}

		m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendInfiniteWarExit()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarExitCommandBody cmdBody = new InfiniteWarExitCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarExit, cmdBody);
        }
    }
    void OnEventResInfiniteWarExit(int nReturnCode, InfiniteWarExitResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResInfiniteWarExit()  ############");

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

            if (EventInfiniteWarExit != null)
            {
                EventInfiniteWarExit(responseBody.previousContinentId);
            }
        }
        else if (nReturnCode == 101)
        {
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00301"));
            }
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendInfiniteWarAbandon()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarAbandonCommandBody cmdBody = new InfiniteWarAbandonCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarAbandon, cmdBody);
        }
    }
    void OnEventResInfiniteWarAbandon(int nReturnCode, InfiniteWarAbandonResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResRuinsReclaimAbandon()  ############");

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

            if (EventInfiniteWarAbandon != null)
            {
                EventInfiniteWarAbandon(responseBody.previousContinentId);
            }
        }
        else if (nReturnCode == 101)
        {
            // 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00401"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendInfiniteWarRevive()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarReviveCommandBody cmdBody = new InfiniteWarReviveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarRevive, cmdBody);
        }
    }
    void OnEventResInfiniteWarRevive(int nReturnCode, InfiniteWarReviveResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResInfiniteWarRevive()  ############");

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

            if (EventInfiniteWarRevive != null)
            {
                EventInfiniteWarRevive();
            }
        }
        else if (nReturnCode == 101)
        {
            // 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00601"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 죽은상태가 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00602"));
        }
        else if (nReturnCode == 103)
        {
            // 부활대기시간이 경화하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00603"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

	//---------------------------------------------------------------------------------------------------
	void SendInfiniteWarBuffBoxAcquire(long lInstanceId)
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            InfiniteWarBuffBoxAcquireCommandBody cmdBody = new InfiniteWarBuffBoxAcquireCommandBody();
            cmdBody.instanceId = m_lBuffBoxInstanceId = lInstanceId;
            CsRplzSession.Instance.Send(ClientCommandName.InfiniteWarBuffBoxAcquire, cmdBody);
        }
    }
    void OnEventResInfiniteWarBuffBoxAcquire(int nReturnCode, InfiniteWarBuffBoxAcquireResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResInfiniteWarBuffBoxAcquire()  ############");

            int nRecoveryHp = responseBody.hp - CsGameData.Instance.MyHeroInfo.Hp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			if (m_csInfiniteWarBuffBox != null)
			{
				CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

				if (m_csInfiniteWarBuffBox.OffenseFactor > 0)
				{
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.PhysicalOffense), m_csInfiniteWarBuffBox.OffenseFactor);
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.MagicalOffense), m_csInfiniteWarBuffBox.OffenseFactor);
				}

				if (m_csInfiniteWarBuffBox.DefenseFactor > 0)
				{
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.PhysicalDefense), m_csInfiniteWarBuffBox.OffenseFactor);
					CsBuffDebuffManager.Instance.AddDungeonBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(EnAttr.MagicalDefense), m_csInfiniteWarBuffBox.OffenseFactor);
				}
			}

            if (EventInfiniteWarBuffBoxAcquire != null)
            {
                EventInfiniteWarBuffBoxAcquire(nRecoveryHp);
            }
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00701"));
        }
        else if (nReturnCode == 102)
        {
            // 버프상자가 존재하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00702"));
        }
        else if (nReturnCode == 103)
        {
            // 버프상자를 획득할 수 없는 위치입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A112_ERROR_00703"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    #endregion InfiniteWar.Command

    #region InfiniteWar.Event

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtContinentExitForInfiniteWarEnter(SEBContinentExitForInfiniteWarEnterEventBody eventBody)
    {
        dd.d("OnEventEvtContinentExitForInfiniteWarEnter ####");

		m_enDungeonPlay = EnDungeonPlay.InfiniteWar;
		m_enInfiniteWarMatchingState = EnDungeonMatchingState.None;
		
        if (EventContinentExitForInfiniteWarEnter != null)
        {
            EventContinentExitForInfiniteWarEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarMatchingStatusChanged(SEBInfiniteWarMatchingStatusChangedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarMatchingRoomPartyEnter ####");
        m_enInfiniteWarMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

        if (EventInfiniteWarMatchingStatusChanged != null)
        {
            EventInfiniteWarMatchingStatusChanged();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarMatchingRoomBanished(SEBInfiniteWarMatchingRoomBanishedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarMatchingRoomBanished ####");

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

		if (EventInfiniteWarMatchingRoomBanished != null)
        {
            EventInfiniteWarMatchingRoomBanished();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarStart(SEBInfiniteWarStartEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarStart ####");
		m_bDungeonStart = true;

		if (EventInfiniteWarStart != null)
        {
            EventInfiniteWarStart();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarMonsterSpawn(SEBInfiniteWarMonsterSpawnEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarMonsterSpawn ####");

        if (EventInfiniteWarMonsterSpawn != null)
        {
            EventInfiniteWarMonsterSpawn(eventBody.monsterInsts);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarBuffBoxCreated(SEBInfiniteWarBuffBoxCreatedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarBuffBoxCreated ####");

        if (EventInfiniteWarBuffBoxCreated != null)
        {
            EventInfiniteWarBuffBoxCreated(eventBody.buffBoxInsts);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarBuffBoxLifetimeEnded(SEBInfiniteWarBuffBoxLifetimeEndedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarBuffBoxLifetimeEnded ####   eventBody.instanceId = " + eventBody.instanceId);

        if (EventInfiniteWarBuffBoxLifetimeEnded != null)
        {
            EventInfiniteWarBuffBoxLifetimeEnded(eventBody.instanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroInfiniteWarBuffBoxAcquisition(SEBHeroInfiniteWarBuffBoxAcquisitionEventBody eventBody)
    {
        dd.d("OnEventEvtHeroInfiniteWarBuffBoxAcquisition ####  eventBody.instanceId = " + eventBody.instanceId);

        if (EventHeroInfiniteWarBuffBoxAcquisition != null)
        {
            EventHeroInfiniteWarBuffBoxAcquisition(eventBody.heroId, eventBody.hp, eventBody.instanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarBuffFinished(SEBInfiniteWarBuffFinishedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarBuffFinished ####");

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

        if (EventInfiniteWarBuffFinished != null)
        {
            EventInfiniteWarBuffFinished();
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtInfiniteWarPointAcquisition(SEBInfiniteWarPointAcquisitionEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarPointAcquisition #### ", eventBody.point, eventBody.pointUpdatedTimeTicks);
		SetInfiniteWarPoint(CsGameData.Instance.MyHeroInfo.HeroId, CsGameData.Instance.MyHeroInfo.Name, eventBody.point, eventBody.pointUpdatedTimeTicks);

		if (EventInfiniteWarPointAcquisition != null)
        {
            EventInfiniteWarPointAcquisition();
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroInfiniteWarPointAcquisition(SEBHeroInfiniteWarPointAcquisitionEventBody eventBody)
    {
        dd.d("OnEventEvtHeroInfiniteWarPointAcquisition #### ", eventBody.point, eventBody.pointUpdatedTimeTicks);
		SetInfiniteWarPoint(eventBody.heroId, "", eventBody.point, eventBody.pointUpdatedTimeTicks);

		if (EventHeroInfiniteWarPointAcquisition != null)
        {
            EventHeroInfiniteWarPointAcquisition();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarClear(SEBInfiniteWarClearEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarClear ####");

        CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

        if (EventInfiniteWarClear != null)
        {
            EventInfiniteWarClear(eventBody.rankings, eventBody.booties, eventBody.rankingBooties);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtInfiniteWarBanished(SEBInfiniteWarBanishedEventBody eventBody)
    {
        dd.d("OnEventEvtInfiniteWarBanished ####");

		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventInfiniteWarBanished != null)
        {
            EventInfiniteWarBanished(eventBody.previousContinentId);
        }
    }

	#endregion InfiniteWar.Event

	#endregion InfiniteWar

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region FearAltar

	//---------------------------------------------------------------------------------------------------
	public void FearAltarEnter()
	{
		dd.d("FearAltarEnter");
		SendFearAltarEnter();
	}

	//---------------------------------------------------------------------------------------------------
	public void FearAltarRevive()
	{
		dd.d("FearAltarRevive");
		SendFearAltarRevive();
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventFearAltarHalidomMonsterKillFail()
	{
		dd.d("OnEventFearAltarHalidomMonsterKillFail");
		if (EventFearAltarHalidomMonsterKillFail != null)
		{
			EventFearAltarHalidomMonsterKillFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventFearAltarHalidomMonsterKill()
	{
		dd.d("OnEventFearAltarHalidomMonsterKill");
		if (EventFearAltarHalidomMonsterKill != null)
		{
			EventFearAltarHalidomMonsterKill();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventClickFearAltarTargetHalidomMonsterTargetButton()
	{
		dd.d("OnEventClickFearAltarTargetHalidomMonsterTargetButton");

		if (EventClickFearAltarTargetHalidomMonsterTargetButton != null)
		{
			EventClickFearAltarTargetHalidomMonsterTargetButton();
		}
	}

	#region FearAltar.Command
	//---------------------------------------------------------------------------------------------------
	public void SendFearAltarMatchingStart(bool bPartyEntrance)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarMatchingStartCommandBody cmdBody = new FearAltarMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bPartyEntrance;
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarMatchingStart, cmdBody);
		}
	}
	void OnEventResFearAltarMatchingStart(int nReturnCode, FearAltarMatchingStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarMatchingStart()  ############");

			m_enFearAltarMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;
			m_flFearAltarMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;

			if (EventFearAltarMatchingStart != null)
			{
				EventFearAltarMatchingStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 파티중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 102 : 파티장이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 103 : 매칭중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 104 : 영웅레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 105 : 영웅이 카트에 탑승중입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 106 : 스태미나가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 107 : 입장횟수가 최대입니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00107"));
		}
        else if (nReturnCode == 108)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00108"));
        }
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	public void SendFearAltarMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarMatchingCancelCommandBody cmdBody = new FearAltarMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarMatchingCancel, cmdBody);
		}
	}
	void OnEventResFearAltarMatchingCancel(int nReturnCode, FearAltarMatchingCancelResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarMatchingCancel()  ############");
			m_enFearAltarMatchingState = EnDungeonMatchingState.None;
			m_flFearAltarMatchingRemainingTime = 0f;

			if (EventFearAltarMatchingCancel != null)
			{
				EventFearAltarMatchingCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 공포의제단매칭중이 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00201"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	void SendFearAltarEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarEnterCommandBody cmdBody = new FearAltarEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarEnter, cmdBody);
		}
	}
	void OnEventResFearAltarEnter(int nReturnCode, FearAltarEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarEnter()  ############");

			m_enFearAltarMatchingState = EnDungeonMatchingState.None;
			m_flFearAltarMatchingRemainingTime = 0f;

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

			CsGameData.Instance.FearAltar.PlayDate = responseBody.date;
			CsGameData.Instance.FearAltar.DailyFearAltarPlayCount = responseBody.playCount;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_csFearAltarStageWave = m_csFearAltarStage.GetFearAltarStageWave(responseBody.waveNo);
			m_bDungeonStart = true;

			if (EventFearAltarEnter != null)
			{
				EventFearAltarEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts);
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00301"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFearAltarExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarExitCommandBody cmdBody = new FearAltarExitCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.FearAltarExit, cmdBody);
		}
	}
	void OnEventResFearAltarExit(int nReturnCode, FearAltarExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarExit()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventFearAltarExit != null)
			{
				EventFearAltarExit(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 101 : 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00401"));
            }
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendFearAltarAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarAbandonCommandBody cmdBody = new FearAltarAbandonCommandBody();

			CsRplzSession.Instance.Send(ClientCommandName.FearAltarAbandon, cmdBody);
		}
	}
	void OnEventResFearAltarAbandon(int nReturnCode, FearAltarAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarAbandon()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventFearAltarAbandon != null)
			{
				EventFearAltarAbandon(responseBody.previousContinentId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00501"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendFearAltarRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			FearAltarReviveCommandBody cmdBody = new FearAltarReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarRevive, cmdBody);
		}
	}	
	void OnEventResFearAltarRevive(int nReturnCode, FearAltarReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResFearAltarRevive()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventFearAltarRevive != null)
			{
				EventFearAltarRevive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 던전상태가 유효하지 않습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 102 : 영웅이 죽은상태가 아닙니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00602"));
		}
		else if (nReturnCode == 103)
		{
			// 103 : 부활대기시간이 경과하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00603"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	#endregion FearAltar.Command

	#region FearAltar.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForFearAltarEnter(SEBContinentExitForFearAltarEnterEventBody eventBody)
	{
		m_enDungeonPlay = EnDungeonPlay.FearAltar;
		m_csFearAltarStage = CsGameData.Instance.FearAltar.GetFearAltarStage(eventBody.stageId);
		
		if (EventContinentExitForFearAltarEnter != null)
		{
			EventContinentExitForFearAltarEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarMatchingRoomPartyEnter(SEBFearAltarMatchingRoomPartyEnterEventBody eventBody)
	{
		m_enFearAltarMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flFearAltarMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;

		if (EventFearAltarMatchingRoomPartyEnter != null)
		{
			EventFearAltarMatchingRoomPartyEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarMatchingRoomBanished(SEBFearAltarMatchingRoomBanishedEventBody eventBody)
	{
		m_enFearAltarMatchingState = EnDungeonMatchingState.None;
		m_flFearAltarMatchingRemainingTime = 0f;

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

		if (EventFearAltarMatchingRoomBanished != null)
		{
			EventFearAltarMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarMatchingStatusChanged(SEBFearAltarMatchingStatusChangedEventBody eventBody)
	{
		m_enFearAltarMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		m_flFearAltarMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		
		if (EventFearAltarMatchingStatusChanged != null)
		{
			EventFearAltarMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarWaveStart(SEBFearAltarWaveStartEventBody eventBody)
	{
		m_csFearAltarStageWave = m_csFearAltarStage.GetFearAltarStageWave(eventBody.waveNo);		

		if (EventFearAltarWaveStart != null)
		{
			Debug.Log("OnEventEvtFearAltarWaveStart");
			Debug.Log(m_csFearAltarStageWave.StageId + " :: " + m_csFearAltarStageWave.WaveNo + " :: " + m_csFearAltarStageWave.Type);
			EventFearAltarWaveStart(eventBody.monsterInsts, eventBody.halidomMonsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarClear(SEBFearAltarClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtFearAltarClear");

		bool bLevelUp = CsGameData.Instance.MyHeroInfo.Level < eventBody.level;
		
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;

		DungeonResult(true);
		if (EventFearAltarClear != null)
		{
			EventFearAltarClear(eventBody.acquiredExp, eventBody.clearedHeroes, bLevelUp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarFail(SEBFearAltarFailEventBody eventBody)
	{
		DungeonResult(false);
		if (EventFearAltarFail != null)
		{
			EventFearAltarFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtFearAltarBanished(SEBFearAltarBanishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventFearAltarBanished != null)
		{
			EventFearAltarBanished(eventBody.previousContinentId);
		}
	}

	#endregion FearAltar.Event

	#endregion FearAltar

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region WarMemory

	//---------------------------------------------------------------------------------------------------
	public class CsWarMemoryPointValue
    {
        Guid m_guidHeroId;
        int m_nPoint;
        long m_lPointUpdatedTimeTicks;

        public Guid HeroId { get { return m_guidHeroId; } }
        public int Point { get { return m_nPoint; } set { m_nPoint = value; } }
        public long PointUpdatedTimeTicks { get { return m_lPointUpdatedTimeTicks; } set { m_lPointUpdatedTimeTicks = value; } }

        public CsWarMemoryPointValue(Guid guidHeroId, int nPoint, long lPointUpdatedTimeTicks)
        {
            m_guidHeroId = guidHeroId;
            m_nPoint = nPoint;
            m_lPointUpdatedTimeTicks = lPointUpdatedTimeTicks;
        }
    }

	Dictionary<Guid, CsWarMemoryPointValue> m_dicWarMemoryHeroPoint = new Dictionary<Guid, CsWarMemoryPointValue>();
    public Dictionary<Guid, CsWarMemoryPointValue> DicWarMemoryHeroPoint { get { return m_dicWarMemoryHeroPoint; } }

    //---------------------------------------------------------------------------------------------------
    public void SetWarMemoryPoint(Guid guidHeroId, int nPoint, long lPointUpdatedTimeTicks, bool bReset = false)
    {
        if (bReset)
        {
			m_dicWarMemoryHeroPoint.Clear();
        }
        else
        {
            if (m_dicWarMemoryHeroPoint.ContainsKey(guidHeroId))
            {
                m_dicWarMemoryHeroPoint[guidHeroId].Point = nPoint;
                m_dicWarMemoryHeroPoint[guidHeroId].PointUpdatedTimeTicks = lPointUpdatedTimeTicks;
            }
            else
            {
                m_dicWarMemoryHeroPoint.Add(guidHeroId, new CsWarMemoryPointValue(guidHeroId, nPoint, lPointUpdatedTimeTicks));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void RemoveWarMemoryHero(Guid guidHeroId)
    {
        if (m_dicWarMemoryHeroPoint.ContainsKey(guidHeroId))
        {
            m_dicWarMemoryHeroPoint.Remove(guidHeroId);
        }
        else
        {
            return;
        }
    }

	//---------------------------------------------------------------------------------------------------
	void ClearWarMemoryHero()
	{
		m_dicWarMemoryHeroPoint.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	public void WarMemoryEnter()
	{
		SendWarMemoryEnter();
	}

	//---------------------------------------------------------------------------------------------------
	public void WarMemoryTransformationObjectInteraction(CsWarMemoryObject csWarMemoryObject)
	{
		Debug.Log("WarMemoryTransformationObjectInteraction " + m_bWaitResponse + " , " + csWarMemoryObject + " , " + csWarMemoryObject.ObjectId);
		m_nObjectId = csWarMemoryObject.ObjectId;
		m_lInstanceId = csWarMemoryObject.InstanceId;
		m_flInteractionDuration = csWarMemoryObject.InteractionMaxRange;
		SendWarMemoryTransformationObjectInteractionStart(m_lInstanceId);		
	}

	//---------------------------------------------------------------------------------------------------
	public void WarMemoryObjectInteractionCancel()
	{
		Debug.Log("SendWarMemoryObjectInteractionCancel  ############");
		CEBWarMemoryObjectInteractionCancelEventBody csEvt = new CEBWarMemoryObjectInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.WarMemoryObjectInteractionCancel, csEvt);
	}

	#region WarMemory.Command

	//---------------------------------------------------------------------------------------------------
	public void SendWarMemoryMatchingStart(bool bisPartyEntrance)
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryMatchingStartCommandBody cmdBody = new WarMemoryMatchingStartCommandBody();
            cmdBody.isPartyEntrance = bisPartyEntrance;
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryMatchingStart, cmdBody);
        }
    }
    void OnEventResWarMemoryMatchingStart(int nReturnCode, WarMemoryMatchingStartResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResWarMemoryMatchingStart()  ############");
            m_enWarMemoryMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;

            if (EventWarMemoryMatchingStart != null)
            {
                EventWarMemoryMatchingStart();
            }
        }
        else if (nReturnCode == 101)
        {
            // 파티중이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 파티장이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 입장가능 시간이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 매칭중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00105"));
        }
        else if (nReturnCode == 106)
        {
            // 입장아이템이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00106"));
        }
        else if (nReturnCode == 107)
        {
            // 영웅이 카트에 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00107"));
        }
        else if (nReturnCode == 108)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00108"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendWarMemoryMatchingCancel()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryMatchingCancelCommandBody cmdBody = new WarMemoryMatchingCancelCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryMatchingCancel, cmdBody);
        }
    }
    void OnEventResWarMemoryMatchingCancel(int nReturnCode, WarMemoryMatchingCancelResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResWarMemoryMatchingCancel()  ############");
            m_enWarMemoryMatchingState = EnDungeonMatchingState.None;

            if (EventWarMemoryMatchingCancel != null)
            {
                EventWarMemoryMatchingCancel();
            }
        }
        else if (nReturnCode == 101)
        {
            // 전쟁의기억매칭중이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00201"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendWarMemoryEnter()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryEnterCommandBody cmdBody = new WarMemoryEnterCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryEnter, cmdBody);
        }
    }
    void OnEventResWarMemoryEnter(int nReturnCode, WarMemoryEnterResponseBody responseBody)
    {
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResWarMemoryEnter()  ############   m_flMultiDungeonRemainingStartTime = " + m_flMultiDungeonRemainingStartTime);

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (responseBody.changedInventorySlot != null)
			{
				List<PDInventorySlot> listPDInventorySlot = new List<PDInventorySlot>();
				listPDInventorySlot.Add(responseBody.changedInventorySlot);
				CsGameData.Instance.MyHeroInfo.AddInventorySlots(listPDInventorySlot.ToArray());
			}

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

			WarMemory.PlayDate = responseBody.date;
			WarMemory.FreePlayCount = responseBody.freePlayCount;

			m_csWarMemoryWave = WarMemory.GetWarMemoryWave(responseBody.waveNo);

			SetWarMemoryPoint(CsGameData.Instance.MyHeroInfo.HeroId, 0, 0);

			for (int i = 0; i < responseBody.heroes.Length; i++)
			{
				SetWarMemoryPoint(responseBody.heroes[i].id, 0, 0);
			}

			for (int i = 0; i < responseBody.points.Length; i++)
			{
				SetWarMemoryPoint(responseBody.points[i].heroId, responseBody.points[i].point, responseBody.points[i].pointUpdatedTimeTicks);
			}

			m_bDungeonStart = true;

			if (EventWarMemoryEnter != null)
			{
				EventWarMemoryEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts, responseBody.objectInsts);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00301"));
			}
			else if (nReturnCode == 102)
			{
				// 입장아이템이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00302"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendWarMemoryExit()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryExitCommandBody cmdBody = new WarMemoryExitCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryExit, cmdBody);
        }
    }
    void OnEventResWarMemoryExit(int nReturnCode, WarMemoryExitResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResWarMemoryExit()  ############");

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			DungeonResult(false);

			if (EventWarMemoryExit != null)
            {
                EventWarMemoryExit(responseBody.previousContinentId);
            }
        }
        else if (nReturnCode == 101)
        {
            if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
            {
                // 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00401"));
            }
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendWarMemoryAbandon()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryAbandonCommandBody cmdBody = new WarMemoryAbandonCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryAbandon, cmdBody);
        }
    }
    void OnEventResWarMemoryAbandon(int nReturnCode, WarMemoryAbandonResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResWarMemoryAbandon()  ############");

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

            if (EventWarMemoryAbandon != null)
            {
                EventWarMemoryAbandon(responseBody.previousContinentId);
            }
        }
        else if (nReturnCode == 101)
        {
            // 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00502"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendWarMemoryRevive()
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryReviveCommandBody cmdBody = new WarMemoryReviveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.WarMemoryRevive, cmdBody);
        }
    }
    void OnEventResWarMemoryRevive(int nReturnCode, WarMemoryReviveResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            Debug.Log("OnEventResWarMemoryRevive()  ############");
	
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
            
            if (EventWarMemoryRevive != null)
            {
                EventWarMemoryRevive(responseBody.position, responseBody.rotationY);
            }
        }
        else if (nReturnCode == 101)
        {
            // 던전상태가 유효하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00601"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 죽은상태가 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00602"));
        }
        else if (nReturnCode == 103)
        {
            // 부활대기시간이 경화하지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00603"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendWarMemoryTransformationObjectInteractionStart(long lInstanceId)
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            WarMemoryTransformationObjectInteractionStartCommandBody cmdBody = new WarMemoryTransformationObjectInteractionStartCommandBody();
			cmdBody.instanceId = m_lInstanceId = lInstanceId;

			CsRplzSession.Instance.Send(ClientCommandName.WarMemoryTransformationObjectInteractionStart, cmdBody);
        }
    }
    void OnEventResWarMemoryTransformationObjectInteractionStart(int nReturnCode, WarMemoryTransformationObjectInteractionStartResponseBody responseBody)
    {
		if (nReturnCode == 0)
		{
            Debug.Log("OnEventResWarMemoryTransformationObjectInteractionStart()  ############");
            ChangeInteractionState(EnInteractionState.interacting);

			if (EventWarMemoryTransformationObjectInteractionStart != null)
			{
				EventWarMemoryTransformationObjectInteractionStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00701"));
			}
			else if (nReturnCode == 102)
			{
				// 오브젝트가 존재하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00702"));
			}
			else if (nReturnCode == 103)
			{
				// 오브젝트가 상호작용중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00703"));
			}
			else if (nReturnCode == 104)
			{
				// 영웅이 죽은상태입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00704"));
			}
			else if (nReturnCode == 105)
			{
				// 상호작용 할 수 없는 위치입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A121_ERROR_00705"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			if (EventDungeonInteractionStartCancel != null)
			{
				EventDungeonInteractionStartCancel();
			}
		}

		m_bWaitResponse = false;
    }

    #endregion WarMemory.Command

    #region WarMemory.Event

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtContinentExitForWarMemoryEnter(SEBContinentExitForWarMemoryEnterEventBody eventBody)
    {
        dd.d("OnEventEvtContinentExitForWarMemoryEnter ####");
        m_enWarMemoryMatchingState = EnDungeonMatchingState.None;
        m_enDungeonPlay = EnDungeonPlay.WarMemory;

        if (EventContinentExitForWarMemoryEnter != null)
        {
            EventContinentExitForWarMemoryEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMatchingRoomPartyEnter(SEBWarMemoryMatchingRoomPartyEnterEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMatchingRoomPartyEnter ####");
        m_enWarMemoryMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

        if (EventWarMemoryMatchingRoomPartyEnter != null)
        {
            EventWarMemoryMatchingRoomPartyEnter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMatchingRoomBanished(SEBWarMemoryMatchingRoomBanishedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMatchingRoomBanished ####");

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }

        if (EventWarMemoryMatchingRoomBanished != null)
        {
            EventWarMemoryMatchingRoomBanished();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMatchingStatusChanged(SEBWarMemoryMatchingStatusChangedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMatchingStatusChanged ####");
        m_enWarMemoryMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

        if (EventWarMemoryMatchingStatusChanged != null)
        {
            EventWarMemoryMatchingStatusChanged();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryWaveStart(SEBWarMemoryWaveStartEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryWaveStart ####    WaveNo = "+ eventBody.waveNo);
		m_csWarMemoryWave = WarMemory.GetWarMemoryWave(eventBody.waveNo);

		if (EventWarMemoryWaveStart != null)
        {
            EventWarMemoryWaveStart(eventBody.monsterInsts, eventBody.objectInsts);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryWaveCompleted(SEBWarMemoryWaveCompletedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryWaveCompleted ####");

        for (int i = 0; i < eventBody.points.Length; i++)
        {
            SetWarMemoryPoint(eventBody.points[i].heroId, eventBody.points[i].point, eventBody.points[i].pointUpdatedTimeTicks);
        }

        if (EventWarMemoryWaveCompleted != null)
        {
            EventWarMemoryWaveCompleted();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryTransformationObjectLifetimeEnded(SEBWarMemoryTransformationObjectLifetimeEndedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryWaveStart ####");
        if (EventWarMemoryTransformationObjectLifetimeEnded != null)
        {
            EventWarMemoryTransformationObjectLifetimeEnded(eventBody.instanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryTransformationObjectInteractionCancel(SEBWarMemoryTransformationObjectInteractionCancelEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryTransformationObjectInteractionCancel ####");
        ChangeInteractionState(EnInteractionState.None);

        if (EventWarMemoryTransformationObjectInteractionCancel != null)
        {
            EventWarMemoryTransformationObjectInteractionCancel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryTransformationObjectInteractionFinished(SEBWarMemoryTransformationObjectInteractionFinishedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryTransformationObjectInteractionFinished ####");

		SetWarMemoryPoint(CsGameData.Instance.MyHeroInfo.HeroId, eventBody.point, eventBody.pointUpdatedTimeTicks);
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		long lOldInstanceId = m_lInstanceId;
        int nOldObjectId = m_nObjectId;

		ChangeInteractionState(EnInteractionState.None);

		if (EventWarMemoryTransformationObjectInteractionFinished != null)
        {
            EventWarMemoryTransformationObjectInteractionFinished(nOldObjectId, lOldInstanceId, eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryTransformationObjectInteractionStart(SEBHeroWarMemoryTransformationObjectInteractionStartEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryTransformationObjectInteractionStart ####");

        if (EventHeroWarMemoryTransformationObjectInteractionStart != null)
        {
            EventHeroWarMemoryTransformationObjectInteractionStart(eventBody.heroId, eventBody.objectInstanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryTransformationObjectInteractionCancel(SEBHeroWarMemoryTransformationObjectInteractionCancelEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryTransformationObjectInteractionCancel ####");

        if (EventHeroWarMemoryTransformationObjectInteractionCancel != null)
        {
            EventHeroWarMemoryTransformationObjectInteractionCancel(eventBody.heroId, eventBody.objectInstanceId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryTransformationObjectInteractionFinished(SEBHeroWarMemoryTransformationObjectInteractionFinishedEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryTransformationObjectInteractionFinished ####");

        if (EventHeroWarMemoryTransformationObjectInteractionFinished != null)
        {
            EventHeroWarMemoryTransformationObjectInteractionFinished(eventBody.heroId, eventBody.objectInstanceId, eventBody.maxHp, eventBody.hp, eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMonsterTransformationCancel(SEBWarMemoryMonsterTransformationCancelEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMonsterTransformationCancel ####");

        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

        if (EventWarMemoryMonsterTransformationCancel != null)
        {
            EventWarMemoryMonsterTransformationCancel(eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMonsterTransformationFinished(SEBWarMemoryMonsterTransformationFinishedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMonsterTransformationFinished ####");

        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

        if (EventWarMemoryMonsterTransformationFinished != null)
        {
            EventWarMemoryMonsterTransformationFinished(eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryMonsterTransformationCancel(SEBHeroWarMemoryMonsterTransformationCancelEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryMonsterTransformationCancel ####");

        if (EventHeroWarMemoryMonsterTransformationCancel != null)
        {
            EventHeroWarMemoryMonsterTransformationCancel(eventBody.heroId, eventBody.maxHp, eventBody.hp, eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryMonsterTransformationFinished(SEBHeroWarMemoryMonsterTransformationFinishedEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryMonsterTransformationFinished ####");

        if (EventHeroWarMemoryMonsterTransformationFinished != null)
        {
            EventHeroWarMemoryMonsterTransformationFinished(eventBody.heroId, eventBody.maxHp, eventBody.hp, eventBody.removedAbnormalStateEffects);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryMonsterSummon(SEBWarMemoryMonsterSummonEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryMonsterSummon ####");

        if (EventWarMemoryMonsterSummon != null)
        {
            EventWarMemoryMonsterSummon(eventBody.monsterInsts);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryPointAcquisition(SEBWarMemoryPointAcquisitionEventBody eventBody)
    {
        SetWarMemoryPoint(CsGameData.Instance.MyHeroInfo.HeroId, eventBody.point, eventBody.pointUpdatedTimeTicks);

        if (EventWarMemoryPointAcquisition != null)
        {
            EventWarMemoryPointAcquisition();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryPointAcquisition(SEBHeroWarMemoryPointAcquisitionEventBody eventBody)
    {
        SetWarMemoryPoint(eventBody.heroId, eventBody.point, eventBody.pointUpdatedTimeTicks);

        if (EventHeroWarMemoryPointAcquisition != null)
        {
            EventHeroWarMemoryPointAcquisition();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtHeroWarMemoryTransformationMonsterSkillCast(SEBHeroWarMemoryTransformationMonsterSkillCastEventBody eventBody)
    {
        dd.d("OnEventEvtHeroWarMemoryTransformationMonsterSkillCast ####");

        if (EventHeroWarMemoryTransformationMonsterSkillCast != null)
        {
            EventHeroWarMemoryTransformationMonsterSkillCast(eventBody.heroId, eventBody.skillId, eventBody.skillTargetPosition);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryClear(SEBWarMemoryClearEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryClear ####");

        bool bLevelUp = CsGameData.Instance.MyHeroInfo.Level < eventBody.level;

        CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
        CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;

        CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		DungeonResult(true);
		if (EventWarMemoryClear != null)
        {
            EventWarMemoryClear(eventBody.rankings, eventBody.rankingBooties, eventBody.acquiredExp, bLevelUp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryFail(SEBWarMemoryFailEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryFail ####");
		DungeonResult(false);
        if (EventWarMemoryFail != null)
        {
            EventWarMemoryFail();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtWarMemoryBanished(SEBWarMemoryBanishedEventBody eventBody)
    {
        dd.d("OnEventEvtWarMemoryBanished ####");

        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

        CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
        CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventWarMemoryBanished != null)
        {
            EventWarMemoryBanished(eventBody.previousContinentId);
        }
    }

	#endregion WarMemory.Event

	#endregion WarMemory

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region OsirisRoom

    long m_lOsirisRoomTotalGoldAcquisitionValue = 0;

    public long OsirisRoomTotalGoldAcquisitionValue { get { return m_lOsirisRoomTotalGoldAcquisitionValue; } }

	//---------------------------------------------------------------------------------------------------
	public void OsirisRoomEnter()
	{
		SendOsirisRoomEnter();
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventOsirisRoomMonsterKillFail()
	{
		if (EventOsirisRoomMonsterKillFail != null)
		{
			EventOsirisRoomMonsterKillFail();
		}
	}

	#region OsirisRoom.Command
	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForOsirisRoomEnter(int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForOsirisRoomEnterCommandBody cmdBody = new ContinentExitForOsirisRoomEnterCommandBody();
			cmdBody.difficulty = m_nDifficulty = nDifficulty;
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForOsirisRoomEnter, cmdBody);
		}
	}
	void OnEventResContinentExitForOsirisRoomEnter(int nReturnCode, ContinentExitForOsirisRoomEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResContinentExitForOsirisRoomEnter()  ############");			
			m_enDungeonPlay = EnDungeonPlay.OsirisRoom;
			m_csOsirisRoomDifficulty = OsirisRoom.GetOsirisRoomDifficulty(m_nDifficulty);
            m_lOsirisRoomTotalGoldAcquisitionValue = 0;

			if (EventContinentExitForOsirisRoomEnter != null)
			{
				EventContinentExitForOsirisRoomEnter();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 영웅레벨이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00101"));
			}
			else if (nReturnCode == 102)
			{
				// 영웅이 죽은상태입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00102"));
			}
			else if (nReturnCode == 103)
			{
				// 스태미나가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00103"));
			}
			else if (nReturnCode == 104)
			{
				// 입장횟수가 초과되었습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00104"));
			}
            else if (nReturnCode == 105)
            {
                // 필요한 메인퀘스트를 완료하지 않았습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00105"));
            }
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendOsirisRoomEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			OsirisRoomEnterCommandBody cmdBody = new OsirisRoomEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.OsirisRoomEnter, cmdBody);
		}
	}
	void OnEventResOsirisRoomEnter(int nReturnCode, OsirisRoomEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResOsirisRoomEnter()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
            OsirisRoom.DailyPlayCount = responseBody.playCount;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_bDungeonStart = true;

			if (EventOsirisRoomEnter != null)
			{
				EventOsirisRoomEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendOsirisRoomExit()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendOsirisRoomExit");
			m_bWaitResponse = true;
			OsirisRoomExitCommandBody cmdBody = new OsirisRoomExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.OsirisRoomExit, cmdBody);
		}
	}
	void OnEventResOsirisRoomExit(int nReturnCode, OsirisRoomExitResponseBody responseBody)
	{
		Debug.Log("OnEventResOsirisRoomExit()  ############     nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventOsirisRoomExit != null)
			{
				EventOsirisRoomExit(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
                if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
                {
                    // 던전상태가 유효하지 않습니다.
                    CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00301"));
                }
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendOsirisRoomAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			OsirisRoomAbandonCommandBody cmdBody = new OsirisRoomAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.OsirisRoomAbandon, cmdBody);
		}
	}
	void OnEventResOsirisRoomAbandon(int nReturnCode, OsirisRoomAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResOsirisRoomAbandon()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventOsirisRoomAbandon != null)
			{
				EventOsirisRoomAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00401"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	int m_nBuffId = 0;

	//---------------------------------------------------------------------------------------------------
	public void SendOsirisRoomMoneyBuffActivate(int nBuffId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			OsirisRoomMoneyBuffActivateCommandBody cmdBody = new OsirisRoomMoneyBuffActivateCommandBody();
			cmdBody.buffId = m_nBuffId = nBuffId;
			CsRplzSession.Instance.Send(ClientCommandName.OsirisRoomMoneyBuffActivate, cmdBody);
		}
	}
	void OnEventResOsirisRoomMoneyBuffActivate(int nReturnCode, OsirisRoomMoneyBuffActivateResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResOsirisRoomMoneyBuffActivate()  ############");
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsBuffDebuffManager.Instance.SetMoneyBuff(m_nBuffId);

			if (EventOsirisRoomMoneyBuffActivate != null)
			{
				EventOsirisRoomMoneyBuffActivate();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00501"));
			}
			else if (nReturnCode == 102)
			{
				// 골드가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00502"));
			}
			else if (nReturnCode == 103)
			{
				// 다이아가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A124_ERROR_00503"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	#endregion OsirisRoom.Command

	#region OsirisRoom.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomWaveStart(SEBOsirisRoomWaveStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtOsirisRoomWaveStart    "+ eventBody.waveNo);
		if (m_csOsirisRoomDifficulty == null)
		{
			m_csOsirisRoomDifficulty = OsirisRoom.GetOsirisRoomDifficulty(m_nDifficulty);
		}

		m_csOsirisRoomDifficultyWave = m_csOsirisRoomDifficulty.GetOsirisRoomDifficultyWave(eventBody.waveNo);

		if (EventOsirisRoomWaveStart != null)
		{
			EventOsirisRoomWaveStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomMonsterSpawn(SEBOsirisRoomMonsterSpawnEventBody eventBody)
	{
		if (EventOsirisRoomMonsterSpawn != null)
		{
			EventOsirisRoomMonsterSpawn(eventBody.monsterInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomRewardGoldAcquisition(SEBOsirisRoomRewardGoldAcquisitionEventBody eventBody)
	{
        long lGoldAcquisition = eventBody.gold - CsGameData.Instance.MyHeroInfo.Gold;
        m_lOsirisRoomTotalGoldAcquisitionValue += lGoldAcquisition;

		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;

		if (EventOsirisRoomRewardGoldAcquisition != null)
		{
			EventOsirisRoomRewardGoldAcquisition();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomMoneyBuffFinished(SEBOsirisRoomMoneyBuffFinishedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventOsirisRoomMoneyBuffFinished != null)
		{
			EventOsirisRoomMoneyBuffFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomMoneyBuffCancel(SEBOsirisRoomMoneyBuffCancelEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventOsirisRoomMoneyBuffCancel != null)
		{
			EventOsirisRoomMoneyBuffCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomClear(SEBOsirisRoomClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtOsirisRoomClear");
		DungeonResult(true);
		if (EventOsirisRoomClear != null)
		{
			EventOsirisRoomClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomFail(SEBOsirisRoomFailEventBody eventBody)	// Fail이지만 실패 처리 없이 모두 클리어처리.
	{
		Debug.Log("OnEventEvtOsirisRoomFail");
		DungeonResult(true);
		if (EventOsirisRoomFail != null)
		{
			EventOsirisRoomFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtOsirisRoomBanished(SEBOsirisRoomBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtOsirisRoomBanished");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventOsirisRoomBanished != null)
		{
			EventOsirisRoomBanished(eventBody.previousContinentId);
		}
	}

	#endregion OsirisRoom.Event

	#endregion OsirisRoom

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region Biography
	//---------------------------------------------------------------------------------------------------
	public void ContinentExitForBiographyQuest(int nDungeonId)
	{
		SendContinentExitForBiographyQuestDungeonEnter(nDungeonId);
	}

	//---------------------------------------------------------------------------------------------------
	public void BiographyQuestDungeonEnter()
	{
		SendBiographyQuestDungeonEnter();
	}

	#region Biography.Command

	//---------------------------------------------------------------------------------------------------
	void SendContinentExitForBiographyQuestDungeonEnter(int nDungeonId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentExitForBiographyQuestDungeonEnterCommandBody cmdBody = new ContinentExitForBiographyQuestDungeonEnterCommandBody();
			cmdBody.dungeonId = nDungeonId;
			m_csBiographyQuestDungeon = CsGameData.Instance.GetBiographyQuestDungeon(nDungeonId);
			CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForBiographyQuestDungeonEnter, cmdBody);
		}
	}
	void OnEventResContinentExitForBiographyQuestDungeonEnter(int nReturnCode, ContinentExitForBiographyQuestDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResContinentExitForBiographyQuestDungeonEnter()  ############");
			m_enDungeonPlay = EnDungeonPlay.Biography;
			
			if (EventContinentExitForBiographyQuestDungeonEnter != null)
			{
				EventContinentExitForBiographyQuestDungeonEnter();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 입장할 수 있는 위치가 아닙니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00101"));
			}
			else if (nReturnCode == 102)
			{
				// 영웅이 죽은 상태입니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00102"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			ResetDungeon();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendBiographyQuestDungeonEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BiographyQuestDungeonEnterCommandBody cmdBody = new BiographyQuestDungeonEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestDungeonEnter, cmdBody);
		}
	}
	void OnEventResBiographyQuestDungeonEnter(int nReturnCode, BiographyQuestDungeonEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResBiographyQuestDungeonEnter()  ############");
			m_enDungeonPlay = EnDungeonPlay.Biography;

			//responseBody.date
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_bDungeonStart = true;

			if (EventBiographyQuestDungeonEnter != null)
			{
				EventBiographyQuestDungeonEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendBiographyQuestDungeonExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BiographyQuestDungeonExitCommandBody cmdBody = new BiographyQuestDungeonExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestDungeonExit, cmdBody);
		}
	}
	void OnEventResBiographyQuestDungeonExit(int nReturnCode, BiographyQuestDungeonExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResBiographyQuestDungeonExit()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventBiographyQuestDungeonExit != null)
			{
				EventBiographyQuestDungeonExit(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
                if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
                {
                    // 던전상태가 유효하지 않습니다.
                    CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00301"));
                }
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendBiographyQuestDungeonAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BiographyQuestDungeonAbandonCommandBody cmdBody = new BiographyQuestDungeonAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestDungeonAbandon, cmdBody);
		}
	}
	void OnEventResBiographyQuestDungeonAbandon(int nReturnCode, BiographyQuestDungeonAbandonResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResBiographyQuestDungeonAbandon()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventBiographyQuestDungeonAbandon != null)
			{
				EventBiographyQuestDungeonAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00401"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendBiographyQuestDungeonRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			BiographyQuestDungeonReviveCommandBody cmdBody = new BiographyQuestDungeonReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestDungeonRevive, cmdBody);
		}
	}
	void OnEventResBiographyQuestDungeonRevive(int nReturnCode, BiographyQuestDungeonReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResBiographyQuestDungeonRevive()  ############");

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventBiographyQuestDungeonRevive != null)
			{
				EventBiographyQuestDungeonRevive();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 영웅이 죽은 상태가 아닙니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00501"));
			}
			else if (nReturnCode == 102)
			{
				// 부활대기시간이 경과하지 않았습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00502"));
			}
			else if (nReturnCode == 103)
			{
				// 던전상태가 유효하지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A123_ERROR_00503"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	#endregion Biography.Command

	#region Biography.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBiographyQuestDungeonWaveStart(SEBBiographyQuestDungeonWaveStartEventBody eventBody)
	{
		dd.d("OnEventEvtBiographyQuestDungeonWaveStart ####      waveNo = " + eventBody.waveNo);
		//eventBody.waveNo;
		m_csBiographyQuestDungeonWave = m_csBiographyQuestDungeon.GetBiographyQuestDungeonWave(eventBody.waveNo);
		if (EventBiographyQuestDungeonWaveStart != null)
		{
			EventBiographyQuestDungeonWaveStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBiographyQuestDungeonWaveCompleted(SEBBiographyQuestDungeonWaveCompletedEventBody eventBody)
	{
		dd.d("OnEventEvtBiographyQuestDungeonWaveCompleted ####      waveNo = "+ eventBody.waveNo);
		//eventBody.waveNo;
		if (EventBiographyQuestDungeonWaveCompleted != null)
		{
			EventBiographyQuestDungeonWaveCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBiographyQuestDungeonFail(SEBBiographyQuestDungeonFailEventBody eventBody)
	{
		dd.d("OnEventEvtBiographyQuestDungeonFail ####      ");
		DungeonResult(false);
		if (EventBiographyQuestDungeonFail != null)
		{
			EventBiographyQuestDungeonFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBiographyQuestDungeonClear(SEBBiographyQuestDungeonClearEventBody eventBody)
	{
		dd.d("OnEventEvtBiographyQuestDungeonClear ####      ");
		DungeonResult(true);
		if (EventBiographyQuestDungeonClear != null)
		{
			EventBiographyQuestDungeonClear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtBiographyQuestDungeonBanished(SEBBiographyQuestDungeonBanishedEventBody eventBody)
	{
		dd.d("OnEventEvtBiographyQuestDungeonBanished ####      ");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventBiographyQuestDungeonBanished != null)
		{
			EventBiographyQuestDungeonBanished(eventBody.previousContinentId);
		}
	}

	#endregion Biography.Event
	#endregion Biography

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region DragonNest

	//---------------------------------------------------------------------------------------------------
	public void DragonNestEnter()
	{
		SendDragonNestEnter();
	}

	#region DragonNest.Command

	//---------------------------------------------------------------------------------------------------
	public void SendDragonNestMatchingStart(bool bPartyEntrance)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DragonNestMatchingStartCommandBody cmdBody = new DragonNestMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bPartyEntrance;
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestMatchingStart, cmdBody);
		}
	}
	void OnEventResDragonNestMatchingStart(int nReturnCode, DragonNestMatchingStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResDragonNestMatchingStart()  ############");

			m_flDragonNestMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;
			m_enDragonNestMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;

			if (EventDragonNestMatchingStart != null)
			{
				EventDragonNestMatchingStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 파티중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00101"));
			}
			else if (nReturnCode == 102)
			{
				// 파티장이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00102"));
			}
			else if (nReturnCode == 103)
			{
				// 매칭중입니다
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00103"));
			}
			else if (nReturnCode == 104)
			{
				// 영웅레벨이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00104"));
			}
			else if (nReturnCode == 105)
			{
				// 영웅이 카트에 탑승중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00105"));
			}
			else if (nReturnCode == 106)
			{
				// 입장아이템이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00106"));
			}
            else if (nReturnCode == 107)
            {
                // 필요한 메인퀘스트를 완료하지 않았습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00107"));
            }
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

        m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendDragonNestMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
            DragonNestMatchingCancelCommandBody cmdBody = new DragonNestMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestMatchingCancel, cmdBody);
		}
	}
	void OnEventResDragonNestMatchingCancel(int nReturnCode, DragonNestMatchingCancelResponseBody responseBody)
    {
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResDragonNestMatchingCancel()  ############");
            m_enDragonNestMatchingState = EnDungeonMatchingState.None;
            m_flDragonNestMatchingRemainingTime = 0f;

			if (EventDragonNestMatchingCancel != null)
			{
				EventDragonNestMatchingCancel();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 용의둥지매칭중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00201"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

        m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendDragonNestEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DragonNestEnterCommandBody cmdBody = new DragonNestEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestEnter, cmdBody);
		}
	}
	void OnEventResDragonNestEnter(int nReturnCode, DragonNestEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResDragonNestEnter()  ############");
            m_enDragonNestMatchingState = EnDungeonMatchingState.None;
            m_flDragonNestMatchingRemainingTime = 0f;

			//responseBody.date;
            m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;
            m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;

			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			CsGameData.Instance.MyHeroInfo.AddInventorySlot(responseBody.changedInventorySlot);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			m_bDungeonStart = true;

			if (EventDragonNestEnter != null)
			{
				EventDragonNestEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts, responseBody.trapEffectHeroes);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

        m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendDragonNestExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DragonNestExitCommandBody cmdBody = new DragonNestExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestExit, cmdBody);
		}
	}
	void OnEventResDragonNestExit(int nReturnCode, DragonNestExitResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResDragonNestExit()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			if (EventDragonNestExit != null)
			{
				EventDragonNestExit(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
                if (CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.None)
                {
                    // 던전상태가 유효하지 않습니다.
                    CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00401"));
                }
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

        m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendDragonNestAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DragonNestAbandonCommandBody cmdBody = new DragonNestAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestAbandon, cmdBody);
		}
	}
	void OnEventResDragonNestAbandon(int nReturnCode, DragonNestAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResDragonNestAbandon()  ############  nReturnCode = "+ nReturnCode);
		if (nReturnCode == 0)
		{
	
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;


			if (EventDragonNestAbandon != null)
			{
				EventDragonNestAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00501"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

        m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendDragonNestRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DragonNestReviveCommandBody cmdBody = new DragonNestReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DragonNestRevive, cmdBody);
		}
	}
	void OnEventResDragonNestRevive(int nReturnCode, DragonNestReviveResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResDragonNestRevive()  ############");
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventDragonNestRevive != null)
			{
				EventDragonNestRevive();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00601"));
			}
			if (nReturnCode == 101)
			{
				// 영웅이 죽은상태가 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00602"));
			}
			if (nReturnCode == 101)
			{
				// 부활대기시간이 경과하지 않았습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A144_ERROR_00603"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

        m_bWaitResponse = false;
	}

	#endregion DragonNest.Command

	#region DragonNest.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForDragonNestEnter(SEBContinentExitForDragonNestEnterEventBody eventBody)
	{
		Debug.Log("OnEventEvtContinentExitForDragonNestEnter()  ############");
		m_enDungeonPlay = EnDungeonPlay.DragonNest;

		if (EventContinentExitForDragonNestEnter != null)
		{
			EventContinentExitForDragonNestEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestMatchingRoomPartyEnter(SEBDragonNestMatchingRoomPartyEnterEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestMatchingRoomPartyEnter()  ############");

		m_flDragonNestMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_enDragonNestMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

		if (EventDragonNestMatchingRoomPartyEnter != null)
		{
			EventDragonNestMatchingRoomPartyEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestMatchingRoomBanished(SEBDragonNestMatchingRoomBanishedEventBody eventBody)
	{
        Debug.Log("OnEventEvtDragonNestMatchingRoomBanished()  ############  " + eventBody.banishType);
        m_enDragonNestMatchingState = EnDungeonMatchingState.None;
        m_flDragonNestMatchingRemainingTime = 0f;

        switch ((EnMatchingRoomBanishedType)eventBody.banishType)
        {
            case EnMatchingRoomBanishedType.Dead:
                break;

            case EnMatchingRoomBanishedType.CartRide:
                break;

            case EnMatchingRoomBanishedType.OpenTime:
                break;

            case EnMatchingRoomBanishedType.Item:
                break;

            case EnMatchingRoomBanishedType.Location:
                break;

            case EnMatchingRoomBanishedType.DungeonEnter:
                break;
        }
		
		//eventBody.banishType
		if (EventDragonNestMatchingRoomBanished != null)
		{
			EventDragonNestMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestMatchingStatusChanged(SEBDragonNestMatchingStatusChangedEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestMatchingStatusChanged()  ############");

		m_flDragonNestMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_enDragonNestMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

		if (EventDragonNestMatchingStatusChanged != null)
		{
			EventDragonNestMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestStepStart(SEBDragonNestStepStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestStepStart()  ############");
		m_csDragonNestStep = DragonNest.GetDragonNestStep(eventBody.stepNo);

		if (EventDragonNestStepStart != null)
		{
			EventDragonNestStepStart(eventBody.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestStepCompleted(SEBDragonNestStepCompletedEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestStepCompleted()  ############");
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		if (EventDragonNestStepCompleted != null)
		{
			EventDragonNestStepCompleted(eventBody.booties);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDragonNestTrapHit(SEBHeroDragonNestTrapHitEventBody eventBody)
	{
		if (EventHeroDragonNestTrapHit != null)
		{
			EventHeroDragonNestTrapHit(eventBody.heroId, eventBody.hp, eventBody.damage, eventBody.hpDamage, eventBody.removedAbnormalStateEffects, eventBody.changedAbnormalStateEffectDamageAbsorbShields);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestTrapEffectFinished(SEBDragonNestTrapEffectFinishedEventBody eventBody)
	{
		if (EventDragonNestTrapEffectFinished != null)
		{
			EventDragonNestTrapEffectFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDragonNestTrapEffectFinished(SEBHeroDragonNestTrapEffectFinishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtHeroDragonNestTrapEffectFinished()  ############");
		
		if (EventHeroDragonNestTrapEffectFinished != null)
		{
			EventHeroDragonNestTrapEffectFinished(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestClear(SEBDragonNestClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestClear()  ############");
		DungeonResult(true);

		if (EventDragonNestClear != null)
		{
            EventDragonNestClear(eventBody.clearedHeroes);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestFail(SEBDragonNestFailEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestFail()  ############");
		DungeonResult(false);
		if (EventDragonNestFail != null)
		{
			EventDragonNestFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDragonNestBanished(SEBDragonNestBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtDragonNestBanished()  ############");
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		if (EventDragonNestBanished != null)
		{
			EventDragonNestBanished(eventBody.previousContinentId);
		}
	}

	#endregion DragonNest.Event

	#endregion DragonNest

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region AnkouTomb

	//---------------------------------------------------------------------------------------------------
	public void AnkouTombEnter()
	{
		Debug.Log("AnkouTombEnter");
		SendAnkouTombEnter();
	}

	#region AnkouTomb.Command
	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombMatchingStart(bool bPartyEntrance, int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombMatchingStartCommandBody cmdBody = new AnkouTombMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bPartyEntrance;
			cmdBody.difficulty = m_nDifficulty = nDifficulty;
			
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombMatchingStart, cmdBody);
		}
	}
	void OnEventResAnkouTombMatchingStart(int nReturnCode, AnkouTombMatchingStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResAnkouTombMatchingStart()  ############");
			m_flAnkouTombMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;
			m_enAnkouTombMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;

			if (EventAnkouTombMatchingStart != null)
			{
				EventAnkouTombMatchingStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 파티중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00101"));
			}
			else if (nReturnCode == 102)
			{
				// 파티장이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00102"));
			}
			else if (nReturnCode == 103)
			{
				// 입장할 가능 시간이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00103"));
			}
			else if (nReturnCode == 104)
			{
				// 매칭중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00104"));
			}
			else if (nReturnCode == 105)
			{
				// 영웅레벨이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00105"));
			}
			else if (nReturnCode == 106)
			{
				// 영웅레벨이 낮아 해당 난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00106"));
			}
			else if (nReturnCode == 107)
			{
				// 영웅레벨이 높아 해당난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00107"));
			}
			else if (nReturnCode == 108)
			{
				// 영웅레벨이 높아 해당난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00108"));
			}
			else if (nReturnCode == 109)
			{
				// 영영위 카트에 탑승중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00109"));
			}
			else if (nReturnCode == 110)
			{
				// 스테미너가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00110"));
			}
			else if (nReturnCode == 111)
			{
				// 입장횟수가 초과되었습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00111"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombMatchingCancelCommandBody cmdBody = new AnkouTombMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombMatchingCancel, cmdBody);
		}
	}
	void OnEventResAnkouTombMatchingCancel(int nReturnCode, AnkouTombMatchingCancelResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResAnkouTombMatchingCancel()  ############");
            m_enAnkouTombMatchingState = EnDungeonMatchingState.None;
            m_flAnkouTombMatchingRemainingTime = 0f;

			if (EventAnkouTombMatchingCancel != null)
			{
				EventAnkouTombMatchingCancel();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 안쿠의무덤매칭중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00201"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombEnterCommandBody cmdBody = new AnkouTombEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombEnter, cmdBody);
		}
	}
	void OnEventResAnkouTombEnter(int nReturnCode, AnkouTombEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
            Debug.Log("OnEventResAnkouTombEnter()  ############");
            m_enAnkouTombMatchingState = EnDungeonMatchingState.None;
            m_flAnkouTombMatchingRemainingTime = 0f;

            AnkouTomb.PlayDate = responseBody.date;
            AnkouTomb.PlayCount = responseBody.playCount;

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_bDungeonStart = true;

			if (EventAnkouTombEnter != null)
			{
				EventAnkouTombEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts, m_nDifficulty);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombExitCommandBody cmdBody = new AnkouTombExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombExit, cmdBody);
		}
	}
	void OnEventResAnkouTombExit(int nReturnCode, AnkouTombExitResponseBody responseBody)
	{
		Debug.Log("OnEventResAnkouTombExit()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventAnkouTombExit != null)
			{
				EventAnkouTombExit(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00401"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombAbandonCommandBody cmdBody = new AnkouTombAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombAbandon, cmdBody);
		}
	}
	void OnEventResAnkouTombAbandon(int nReturnCode, AnkouTombAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResAnkouTombAbandon()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventAnkouTombAbandon != null)
			{
				EventAnkouTombAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00501"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombReviveCommandBody cmdBody = new AnkouTombReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombRevive, cmdBody);
		}
	}
	void OnEventResAnkouTombRevive(int nReturnCode, AnkouTombReviveResponseBody responseBody)
	{
		Debug.Log("OnEventResAnkouTombRevive()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventAnkouTombRevive != null)
			{
				EventAnkouTombRevive();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00601"));
			}
			if (nReturnCode == 102)
			{
				// 영웅이 죽은상태가 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00602"));
			}
			if (nReturnCode == 103)
			{
				// 부활대기시간이 경과하지 않았습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00603"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendAnkouTombMoneyBuffActivate(int nBuffId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombMoneyBuffActivateCommandBody cmdBody = new AnkouTombMoneyBuffActivateCommandBody();
            cmdBody.buffId = m_nBuffId = nBuffId;
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombMoneyBuffActivate, cmdBody);
		}
	}
	void OnEventResAnkouTombMoneyBuffActivate(int nReturnCode, AnkouTombMoneyBuffActivateResponseBody responseBody)
	{
		Debug.Log("OnEventResAnkouTombRevive()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsBuffDebuffManager.Instance.SetMoneyBuff(m_nBuffId);

			if (EventAnkouTombMoneyBuffActivate != null)
			{
				EventAnkouTombMoneyBuffActivate();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00701"));
			}
			if (nReturnCode == 102)
			{
				// 골드가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00702"));
			}
			if (nReturnCode == 103)
			{
				// 다이아가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00703"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendAnkouTombAdditionalRewardExpReceive(int nRewardExpType)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AnkouTombAdditionalRewardExpReceiveCommandBody cmdBody = new AnkouTombAdditionalRewardExpReceiveCommandBody();
            cmdBody.rewardExpType = nRewardExpType;
			CsRplzSession.Instance.Send(ClientCommandName.AnkouTombAdditionalRewardExpReceive, cmdBody);
		}
	}
	void OnEventResAnkouTombAdditionalRewardExpReceive(int nReturnCode, AnkouTombAdditionalRewardExpReceiveResponseBody responseBody)
	{
		Debug.Log("OnEventResAnkouTombRevive()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;


			if (EventAnkouTombAdditionalRewardExpReceive != null)
			{
				EventAnkouTombAdditionalRewardExpReceive(bLevelUp, responseBody.acquiredExp);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00801"));
			}
			if (nReturnCode == 102)
			{
				// 비귀속다이아가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A162_ERROR_00802"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}
	#endregion AnkouTomb.Command

	#region AnkouTomb.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForAnkouTombEnter(SEBContinentExitForAnkouTombEnterEventBody eventBody)
	{
		Debug.Log("OnEventEvtContinentExitForAnkouTombEnter()  ############");
		m_enDungeonPlay = EnDungeonPlay.AnkouTomb;
		if (EventContinentExitForAnkouTombEnter != null)
		{
			EventContinentExitForAnkouTombEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombMatchingRoomPartyEnter(SEBAnkouTombMatchingRoomPartyEnterEventBody eventBody)   // 관련유저 안쿠의무덤매칭방파티입장
	{
		Debug.Log("OnEventEvtAnkouTombMatchingRoomPartyEnter()  ############  " + eventBody.difficulty + " , " + (EnDungeonMatchingState)eventBody.matchingStatus);

		m_nDifficulty = eventBody.difficulty;
		m_flAnkouTombMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_enAnkouTombMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

		if (EventAnkouTombMatchingRoomPartyEnter != null)
		{
			EventAnkouTombMatchingRoomPartyEnter(eventBody.difficulty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombMatchingRoomBanished(SEBAnkouTombMatchingRoomBanishedEventBody eventBody)
	{
        Debug.Log("OnEventEvtAnkouTombMatchingRoomBanished()  ############   " + (EnMatchingRoomBanishedType)eventBody.banishType);
        m_enAnkouTombMatchingState = EnDungeonMatchingState.None;
        m_flAnkouTombMatchingRemainingTime = 0f;
		// 강퇴타입
		// 1.죽은상태
		// 2.카트탑승상태
		// 3.오픈시간경과
		// 4.입장아이템부족
		// 5.위치정보없음
		// 6.던전입장
		switch ((EnMatchingRoomBanishedType)eventBody.banishType)
		{
			case EnMatchingRoomBanishedType.Dead:
				break;

			case EnMatchingRoomBanishedType.CartRide:
				break;

			case EnMatchingRoomBanishedType.OpenTime:
				break;

			case EnMatchingRoomBanishedType.Item:
				break;

			case EnMatchingRoomBanishedType.Location:
				break;

			case EnMatchingRoomBanishedType.DungeonEnter:
				break;
		}

		//eventBody.banishType
		if (EventAnkouTombMatchingRoomBanished != null)
		{
			EventAnkouTombMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombMatchingStatusChanged(SEBAnkouTombMatchingStatusChangedEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombMatchingStatusChanged()  ############   "+ (EnDungeonMatchingState)eventBody.matchingStatus);
		m_flAnkouTombMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;		
		m_enAnkouTombMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		if (EventAnkouTombMatchingStatusChanged != null)
		{
			EventAnkouTombMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombWaveStart(SEBAnkouTombWaveStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombWaveStart()  ############   "+ eventBody.waveNo);

		m_nWaveNo = eventBody.waveNo;
		if (EventAnkouTombWaveStart != null)
		{
			EventAnkouTombWaveStart(eventBody.monsterInsts, eventBody.waveNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombPointAcquisition(SEBAnkouTombPointAcquisitionEventBody eventBody)
	{
        m_nAnkouTombPoint = eventBody.point;
		
		if (EventAnkouTombPointAcquisition != null)
		{
			EventAnkouTombPointAcquisition();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombMoneyBuffFinished(SEBAnkouTombMoneyBuffFinishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombMoneyBuffFinished()  ############");
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventAnkouTombMoneyBuffFinished != null)
		{
			EventAnkouTombMoneyBuffFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombMoneyBuffCancel(SEBAnkouTombMoneyBuffCancelEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombMoneyBuffCancel()  ############");
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventAnkouTombMoneyBuffCancel != null)
		{
			EventAnkouTombMoneyBuffCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombClear(SEBAnkouTombClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombClear()  ############");
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;
        CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

        m_nAnkouTombPoint = eventBody.point;

        if (CsGameData.Instance.AnkouTomb.GetMyHeroAnkouTombBestRecord(m_nDifficulty) == null)
        {
            CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(CsGameData.Instance.MyHeroInfo.HeroId, CsGameData.Instance.MyHeroInfo.Name, CsGameData.Instance.MyHeroInfo.Job.JobId, CsGameData.Instance.MyHeroInfo.Nation.NationId, m_nDifficulty, eventBody.point));
        }
        else
        {
            for (int i = 0; i < CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList.Count; i++)
            {
                if (CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList[i].Difficulty == m_nDifficulty)
                {
                    if (CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList[i].Point < eventBody.point)
                    {
                        CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList[i].Point = eventBody.point;
                    }

                    break;
                }
            }
        }
		
		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(true);
		if (EventAnkouTombClear != null)
		{
			EventAnkouTombClear(bLevelUp, eventBody.acquiredExp, eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombFail(SEBAnkouTombFailEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombFail()  ############");
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(false);
		if (EventAnkouTombFail != null)
		{
			EventAnkouTombFail(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombBanished(SEBAnkouTombBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombBanished()  ############   " + eventBody.previousContinentId);
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventAnkouTombBanished != null)
		{
			EventAnkouTombBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtAnkouTombServerBestRecordUpdated(SEBAnkouTombServerBestRecordUpdatedEventBody eventBody)
	{
		Debug.Log("OnEventEvtAnkouTombServerBestRecordUpdated()  ############");

        if (CsGameData.Instance.AnkouTomb.GetServerHeroAnkouTombBestRecord(m_nDifficulty) == null)
        {
            CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(eventBody.record));
        }
        else
        {
            for (int i = 0; i < CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList.Count; i++)
            {
                if (CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].Difficulty == eventBody.record.difficulty)
                {
                    CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].HeroId = eventBody.record.heroId;
                    CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].HeroName = eventBody.record.heroName;
                    CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].JobId = eventBody.record.heroJobId;
                    CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].HeroNationId = eventBody.record.heroNationId;
                    CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList[i].Point = eventBody.record.point;
                    break;
                }
            }
        }

		if (EventAnkouTombServerBestRecordUpdated != null)
		{
			EventAnkouTombServerBestRecordUpdated();
		}
	}

	#endregion AnkouTomb.Event

	#endregion AnkouTomb

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region TradeShip

	//---------------------------------------------------------------------------------------------------
	public void TradeShipEnter()
	{
		Debug.Log("TradeShipEnter()");
		SendTradeShipEnter();
	}

	#region TradeShip.Command
	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipMatchingStart(bool bPartyEntrance, int nDifficulty)
	{
		Debug.Log("SendTradeShipMatchingStart() : "+ bPartyEntrance+" , "+ nDifficulty);
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipMatchingStartCommandBody cmdBody = new TradeShipMatchingStartCommandBody();
			cmdBody.isPartyEntrance = bPartyEntrance;
			cmdBody.difficulty = m_nDifficulty = nDifficulty;

			CsRplzSession.Instance.Send(ClientCommandName.TradeShipMatchingStart, cmdBody);
		}
	}

	void OnEventResTradeShipMatchingStart(int nReturnCode, TradeShipMatchingStartResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResTradeShipMatchingStart()  ############");
			m_flTradeShipMatchingRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;
			m_enTradeShipMatchingState = (EnDungeonMatchingState)responseBody.matchingStatus;

			if (EventTradeShipMatchingStart != null)
			{
				EventTradeShipMatchingStart();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 파티중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00101"));
			}
			else if (nReturnCode == 102)
			{
				// 파티장이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00102"));
			}
			else if (nReturnCode == 103)
			{
				// 입장할 가능 시간이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00103"));
			}
			else if (nReturnCode == 104)
			{
				// 매칭중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00104"));
			}
			else if (nReturnCode == 105)
			{
				// 영웅레벨이 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00105"));
			}
			else if (nReturnCode == 106)
			{
				// 영웅레벨이 낮아 해당 난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00106"));
			}
			else if (nReturnCode == 107)
			{
				// 영웅레벨이 높아 해당난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00107"));
			}
			else if (nReturnCode == 108)
			{
				// 영웅레벨이 높아 해당난이도에 입장할 수 없습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00108"));
			}
			else if (nReturnCode == 109)
			{
				// 영영위 카트에 탑승중입니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00109"));
			}
			else if (nReturnCode == 110)
			{
				// 스테미너가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00110"));
			}
			else if (nReturnCode == 111)
			{
				// 입장횟수가 초과되었습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00111"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipMatchingCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipMatchingCancelCommandBody cmdBody = new TradeShipMatchingCancelCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipMatchingCancel, cmdBody);
		}
	}

	void OnEventResTradeShipMatchingCancel(int nReturnCode, TradeShipMatchingCancelResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResTradeShipMatchingCancel()  ############");
            m_enTradeShipMatchingState = EnDungeonMatchingState.None;
            m_flTradeShipMatchingRemainingTime = 0f;

			if (EventTradeShipMatchingCancel != null)
			{
				EventTradeShipMatchingCancel();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 무역선탄환매칭중이 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00201"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void SendTradeShipEnter()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipEnterCommandBody cmdBody = new TradeShipEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipEnter, cmdBody);
		}
	}

	void OnEventResTradeShipEnter(int nReturnCode, TradeShipEnterResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			Debug.Log("OnEventResTradeShipEnter()  ############");
            m_enTradeShipMatchingState = EnDungeonMatchingState.None;
            m_flTradeShipMatchingRemainingTime = 0f;

            CsGameData.Instance.TradeShip.PlayCount = responseBody.playCount;
            CsGameData.Instance.TradeShip.PlayDate = responseBody.date;

			m_csTradeShipStep = TradeShip.GetTradeShipStep(responseBody.stepNo);

			m_flMultiDungeonRemainingStartTime = responseBody.remainingStartTime;
			m_flMultiDungeonRemainingLimitTime = responseBody.remainingLimitTime;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;
			m_bDungeonStart = true;

			if (EventTradeShipEnter != null)
			{
				EventTradeShipEnter(responseBody.placeInstanceId, responseBody.position, responseBody.rotationY, responseBody.heroes, responseBody.monsterInsts, m_nDifficulty);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00301"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipExitCommandBody cmdBody = new TradeShipExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipExit, cmdBody);
		}
	}

	void OnEventResTradeShipExit(int nReturnCode, TradeShipExitResponseBody responseBody)
	{
		Debug.Log("OnEventResTradeShipExit()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventTradeShipExit != null)
			{
				EventTradeShipExit(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00401"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipAbandonCommandBody cmdBody = new TradeShipAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipAbandon, cmdBody);
		}
	}

	void OnEventResTradeShipAbandon(int nReturnCode, TradeShipAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResTradeShipAbandon()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

			CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

			if (EventTradeShipAbandon != null)
			{
				EventTradeShipAbandon(responseBody.previousContinentId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00501"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
			DungeonEnterFail();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipRevive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipReviveCommandBody cmdBody = new TradeShipReviveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipRevive, cmdBody);
		}
	}

	void OnEventResTradeShipRevive(int nReturnCode, TradeShipReviveResponseBody responseBody)
	{
		Debug.Log("OnEventResTradeShipRevive()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

			if (EventTradeShipRevive != null)
			{
				EventTradeShipRevive();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00601"));
			}
			if (nReturnCode == 102)
			{
				// 영웅이 죽은상태가 아닙니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00602"));
			}
			if (nReturnCode == 103)
			{
				// 부활대기시간이 경과하지 않았습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00603"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTradeShipMoneyBuffActivate(int nBuffId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipMoneyBuffActivateCommandBody cmdBody = new TradeShipMoneyBuffActivateCommandBody();
            cmdBody.buffId = m_nBuffId = nBuffId;
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipMoneyBuffActivate, cmdBody);
		}
	}

	void OnEventResTradeShipMoneyBuffActivate(int nReturnCode, TradeShipMoneyBuffActivateResponseBody responseBody)
	{
		Debug.Log("OnEventResTradeShipMoneyBuffActivate()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsBuffDebuffManager.Instance.SetMoneyBuff(m_nBuffId);

			if (EventTradeShipMoneyBuffActivate != null)
			{
				EventTradeShipMoneyBuffActivate();
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00701"));
			}
			if (nReturnCode == 102)
			{
				// 골드가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00702"));
			}
			if (nReturnCode == 103)
			{
				// 다이아가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00703"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
    public void SendTradeShipAdditionalRewardExpReceive(int nRewardExpType)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TradeShipAdditionalRewardExpReceiveCommandBody cmdBody = new TradeShipAdditionalRewardExpReceiveCommandBody();
            cmdBody.rewardExpType = nRewardExpType;
			CsRplzSession.Instance.Send(ClientCommandName.TradeShipAdditionalRewardExpReceive, cmdBody);
		}
	}

	void OnEventResTradeShipAdditionalRewardExpReceive(int nReturnCode, TradeShipAdditionalRewardExpReceiveResponseBody responseBody)
	{
		Debug.Log("OnEventResTradeShipAdditionalRewardExpReceive()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;


			if (EventTradeShipAdditionalRewardExpReceive != null)
			{
				EventTradeShipAdditionalRewardExpReceive(bLevelUp, responseBody.acquiredExp);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00801"));
			}
			if (nReturnCode == 102)
			{
				// 비귀속다이아가 부족합니다.
                CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A164_ERROR_00802"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_bWaitResponse = false;
	}

	#endregion TradeShip.Command

	#region TradeShip.Event
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtContinentExitForTradeShipEnter(SEBContinentExitForTradeShipEnterEventBody eventBody)
	{
		Debug.Log("OnEventEvtContinentExitForTradeShipEnter()  ############");
		m_enDungeonPlay = EnDungeonPlay.TradeShip;
		if (EventContinentExitForTradeShipEnter != null)
		{
			EventContinentExitForTradeShipEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipMatchingRoomPartyEnter(SEBTradeShipMatchingRoomPartyEnterEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipMatchingRoomPartyEnter()  ############  " + eventBody.difficulty + " , " + (EnDungeonMatchingState)eventBody.matchingStatus);

		m_nDifficulty = eventBody.difficulty;
		m_flTradeShipMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_enTradeShipMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;

		if (EventTradeShipMatchingRoomPartyEnter != null)
		{
			EventTradeShipMatchingRoomPartyEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipMatchingRoomBanished(SEBTradeShipMatchingRoomBanishedEventBody eventBody)
	{
        Debug.Log("OnEventEvtTradeShipMatchingRoomBanished()  ############   " + (EnMatchingRoomBanishedType)eventBody.banishType);
        m_enTradeShipMatchingState = EnDungeonMatchingState.None;
        m_flTradeShipMatchingRemainingTime = 0f;
		
		switch ((EnMatchingRoomBanishedType)eventBody.banishType)
		{
			case EnMatchingRoomBanishedType.Dead:
				break;

			case EnMatchingRoomBanishedType.CartRide:
				break;

			case EnMatchingRoomBanishedType.OpenTime:
				break;

			case EnMatchingRoomBanishedType.Item:
				break;

			case EnMatchingRoomBanishedType.Location:
				break;

			case EnMatchingRoomBanishedType.DungeonEnter:
				break;
		}

		//eventBody.banishType
		if (EventTradeShipMatchingRoomBanished != null)
		{
			EventTradeShipMatchingRoomBanished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipMatchingStatusChanged(SEBTradeShipMatchingStatusChangedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipMatchingStatusChanged()  ############   " + (EnDungeonMatchingState)eventBody.matchingStatus);
		m_flTradeShipMatchingRemainingTime = eventBody.remainingTime + Time.realtimeSinceStartup;
		m_enTradeShipMatchingState = (EnDungeonMatchingState)eventBody.matchingStatus;
		if (EventTradeShipMatchingStatusChanged != null)
		{
			EventTradeShipMatchingStatusChanged();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipStepStart(SEBTradeShipStepStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipStepStart()  ############");
		m_csTradeShipStep = TradeShip.GetTradeShipStep(eventBody.stepNo);

		if (EventTradeShipStepStart != null)
		{
			EventTradeShipStepStart(eventBody.monsterInsts, eventBody.additionalMonsterInsts, eventBody.objectInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipPointAcquisition(SEBTradeShipPointAcquisitionEventBody eventBody)
	{
		//Debug.Log("OnEventEvtTradeShipPointAcquisition()  ############");

        m_nTradeShipPoint = eventBody.point;

		if (EventTradeShipPointAcquisition != null)
		{
			EventTradeShipPointAcquisition();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipObjectDestructionReward(SEBTradeShipObjectDestructionRewardEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipObjectDestructionReward()  ############");
        m_nTradeShipPoint += eventBody.point;

		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		if (EventTradeShipObjectDestructionReward != null)
		{
			EventTradeShipObjectDestructionReward(eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipMoneyBuffFinished(SEBTradeShipMoneyBuffFinishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipMoneyBuffFinished()  ############");
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventTradeShipMoneyBuffFinished != null)
		{
			EventTradeShipMoneyBuffFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipMoneyBuffCancel(SEBTradeShipMoneyBuffCancelEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipMoneyBuffCancel()  ############");
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventTradeShipMoneyBuffCancel != null)
		{
			EventTradeShipMoneyBuffCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipClear(SEBTradeShipClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipClear()  ############");
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;
        CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

        m_nTradeShipPoint = eventBody.point;

        if (CsGameData.Instance.TradeShip.GetMyHeroTradeShipBestRecord(m_nDifficulty) == null)
        {
            CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(CsGameData.Instance.MyHeroInfo.HeroId, CsGameData.Instance.MyHeroInfo.Name, CsGameData.Instance.MyHeroInfo.JobId, CsGameData.Instance.MyHeroInfo.Nation.NationId, m_nDifficulty, eventBody.point));
        }
        else
        {
            for (int i = 0; i < CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList.Count; i++)
            {
                if (CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList[i].Difficulty == m_nDifficulty)
                {
                    if (CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList[i].Point < eventBody.point)
                    {
                        CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList[i].Point = eventBody.point;
                    }

                    break;
                }
            }
        }

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(true);
		if (EventTradeShipClear != null)
		{
			EventTradeShipClear(bLevelUp, eventBody.acquiredExp, eventBody.booty);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipFail(SEBTradeShipFailEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipFail()  ############");
		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		DungeonResult(false);
		if (EventTradeShipFail != null)
		{
			EventTradeShipFail(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipBanished(SEBTradeShipBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipBanished()  ############   " + eventBody.previousContinentId);
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.previousNationId;
		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.PrevContinent;

		CsBuffDebuffManager.Instance.ResetDungeonBuffDebuffAttrFactor();

		if (EventTradeShipBanished != null)
		{
			EventTradeShipBanished(eventBody.previousContinentId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTradeShipServerBestRecordUpdated(SEBTradeShipServerBestRecordUpdatedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTradeShipServerBestRecordUpdated()  ############");

        if (CsGameData.Instance.TradeShip.GetServerHeroTradeShipBestRecord(m_nDifficulty) == null)
        {
            CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(eventBody.record));
        }
        else
        {
            for (int i = 0; i < CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList.Count; i++)
            {
                if (CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].Difficulty == m_nDifficulty)
                {
                    CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].HeroId = eventBody.record.heroId;
                    CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].HeroName = eventBody.record.heroName;
                    CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].JobId = eventBody.record.heroJobId;
                    CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].HeroNationId = eventBody.record.heroNationId;
                    CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList[i].Point = eventBody.record.point;
                    break;
                }
            }
        }

		if (EventTradeShipServerBestRecordUpdated != null)
		{
			EventTradeShipServerBestRecordUpdated();
		}
	}

	#endregion TradeShip.Event

	#endregion TradeShip

	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	#region TeamBattlefield

	//---------------------------------------------------------------------------------------------------
	public class CsTeamBattlefieldPointValue
	{
		Guid m_guidHeroId;
        string m_strHeroName;
		int m_nPoint;
		long m_lPointUpdatedTimeTicks;

		public Guid HeroId { get { return m_guidHeroId; } }
        public string HeroName { get { return m_strHeroName; } }
		public int Point { get { return m_nPoint; } set { m_nPoint = value; } }
		public long PointUpdatedTimeTicks { get { return m_lPointUpdatedTimeTicks; } set { m_lPointUpdatedTimeTicks = value; } }

		public CsTeamBattlefieldPointValue(Guid guidHeroId, string strHeroName, int nPoint, long lPointUpdatedTimeTicks)
		{
			m_guidHeroId = guidHeroId;
            m_strHeroName = strHeroName;
			m_nPoint = nPoint;
			m_lPointUpdatedTimeTicks = lPointUpdatedTimeTicks;
		}
	}

	Dictionary<Guid, CsTeamBattlefieldPointValue> m_dicTeamBattlefieldHeroPoint = new Dictionary<Guid, CsTeamBattlefieldPointValue>();
	public Dictionary<Guid, CsTeamBattlefieldPointValue> DicTeamBattlefieldHeroPoint { get { return m_dicTeamBattlefieldHeroPoint; } }
	//---------------------------------------------------------------------------------------------------
	public void SetTeamBattlefieldPoint(Guid guidHeroId, string strHeroName, int nPoint, long lPointUpdatedTimeTicks, bool bReset = false)
	{
		if (bReset)
		{
			m_dicTeamBattlefieldHeroPoint.Clear();
		}
		else
		{
			if (m_dicTeamBattlefieldHeroPoint.ContainsKey(guidHeroId))
			{
				m_dicTeamBattlefieldHeroPoint[guidHeroId].Point = nPoint;
				m_dicTeamBattlefieldHeroPoint[guidHeroId].PointUpdatedTimeTicks = lPointUpdatedTimeTicks;
			}
			else
			{
				m_dicTeamBattlefieldHeroPoint.Add(guidHeroId, new CsTeamBattlefieldPointValue(guidHeroId, strHeroName, nPoint, lPointUpdatedTimeTicks));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void TeamBattlefieldEnter()
	{
		Debug.Log("TeamBattlefieldEnter()");
		SendTeamBattlefieldEnter();
	}

	#region TeamBattlefield.Command

	//---------------------------------------------------------------------------------------------------
	public void SendTeamBattlefieldInfo()
	{
		Debug.Log("SendTeamBattlefieldInfo()");
		if (!m_bWaitResponse)
		{
		
		}
	}

	void OnEventResTeamBattlefieldInfo(int nReturnCode, TeamBattlefieldInfoResponseBody responseBody)
	{
		Debug.Log("OnEventResTeamBattlefieldInfo()  ############        nReturnCode = " + nReturnCode);

		if (nReturnCode == 0)
		{
			if (EventTeamBattlefieldInfo != null)
			{
				
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));

			if (EventTeamBattlefieldInfoFail != null)
			{
				EventTeamBattlefieldInfoFail();
			}
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendContinentExitForTeamBattlefieldEnter()
	{
		if (!m_bWaitResponse)
		{
			
		}                                                                                                                 
	}

	void OnEventResContinentExitForTeamBattlefieldEnter(int nReturnCode, ContinentExitForTeamBattlefieldEnterResponseBody responseBody)
	{
		Debug.Log("OnEventResContinentExitForTeamBattlefieldEnter()  ############        nReturnCode = " + nReturnCode);

		m_enBattlefieldState = EnBattlefieldState.None;

		if (nReturnCode == 0)
		{
			if (EventContinentExitForTeamBattlefieldEnter != null)
			{
				EventContinentExitForTeamBattlefieldEnter();
			}

			SendTeamBattlefieldEnter();
		}
		else
		{
			if(nReturnCode == 101)
			{
				// 입장가능한 요일이 아닙니다.
			}
			else if (nReturnCode == 102)
			{
				// 던전이 존재하지 않습니다.
			}
			else if (nReturnCode == 103)
			{
				// 던전이 입장할 수 없는 상태입니다.
			}
			else if (nReturnCode == 104)
			{
				// 정원이 초과되었습니다.
			}
			else if (nReturnCode == 105)
			{
				// 영웅레벨이 부족합니다.
			}
			else if (nReturnCode == 106)
			{
				// 입장에 필요한 메인 퀘스트를 완료하지 않았습니다.
			}
			else if (nReturnCode == 107)
			{
				// 입장에 필요한 메인 퀘스트를 수락하지 않았습니다.
			}
			else if (nReturnCode == 108)
			{
				// 영웅이 죽은 상태입니다.
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			ResetDungeon();
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTeamBattlefieldEnter()
	{
		if (!m_bWaitResponse)
		{

		}
	}

	void OnEventResTeamBattlefieldEnter(int nReturnCode, TeamBattlefieldEnterResponseBody responseBody)
	{
		Debug.Log("OnEventResTeamBattlefieldEnter()  ############        nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
		
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 입장가능한 요일이 아닙니다.
			}
			else if (nReturnCode == 102)
			{
				// 던전이 존재하지 않습니다.
			}
			else if (nReturnCode == 103)
			{
				//던전이 입장할 수 없는 상태입니다.
			}
			else if (nReturnCode == 104)
			{
				// 정원이 초과되었습니다.
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTeamBattlefieldExit()
	{
		if (!m_bWaitResponse)
		{

		}    
	}

	void OnEventResTeamBattlefieldExit(int nReturnCode, TeamBattlefieldExitResponseBody responseBody)
	{
		Debug.Log("OnEventResTeamBattlefieldExit()  ############        nReturnCode = " + nReturnCode);

		m_enBattlefieldState = EnBattlefieldState.None;

		if (nReturnCode == 0)
		{

		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendTeamBattlefieldAbandon()
	{
		if (!m_bWaitResponse)
		{

		}    
	}

	void OnEventResTeamBattlefieldAbandon(int nReturnCode, TeamBattlefieldAbandonResponseBody responseBody)
	{
		Debug.Log("OnEventResTeamBattlefieldAbandon()  ############        nReturnCode = " + nReturnCode);

		m_enBattlefieldState = EnBattlefieldState.None;

		if (nReturnCode == 0)
		{

		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전상태가 유효하지 않습니다.
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}
		m_bWaitResponse = false;
	}
	//---------------------------------------------------------------------------------------------------
	public void SendTeamBattlefieldRevive()
	{
		if (!m_bWaitResponse)
		{
		
		} 
	}

	void OnEventResTeamBattlefieldRevive(int nReturnCode, TeamBattlefieldReviveResponseBody responseBody)
	{
		Debug.Log("OnEventResTeamBattlefieldRevive()  ############        nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 던전 상태가 유효화지 않습니다.
			}
			else if (nReturnCode == 102)
			{
				// 영웅이 죽은 상태가 아닙니다.
			}
			else if (nReturnCode == 103)
			{
				// 부활대기시간이 경과하지 않았습니다.
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}
		m_bWaitResponse = false;
	}
	#endregion TeamBattlefield.Command

	#region TeamBattlefield.Event
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldPlayWaitStart(SEBTeamBattlefieldPlayWaitStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtTeamBattlefieldPlayWaitStart()  ############   ");



	}
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldStart(SEBTeamBattlefieldStartEventBody eventBody)
	{
		Debug.Log("OnEventEvtTeamBattlefieldStart()  ############   ");

		m_enBattlefieldState = EnBattlefieldState.Playing;

		if (EventTeamBattlefieldStart != null)
		{
			EventTeamBattlefieldStart();
		}
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldAcquisition(SEBTeamBattlefieldPointAcquisitionEventBody eventBody)
	{

	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroTeamBattlefieldPointAcquisition(SEBHeroTeamBattlefieldPointAcquisitionEventBody eventBody)
	{

	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldClear(SEBTeamBattlefieldClearEventBody eventBody)
	{
		Debug.Log("OnEventEvtTeamBattlefieldClear()  ############   ");


	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldBanished(SEBTeamBattlefieldBanishedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTeamBattlefieldBanished()  ############   ");



	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtTeamBattlefieldServerBestRecordUpdated(SEBTeamBattlefieldServerBestRecordUpdatedEventBody eventBody)
	{
		Debug.Log("OnEventEvtTeamBattlefieldServerBestRecordUpdated()  ############");

		// 기록저장 처리

		if (EventTeamBattlefieldServerBestRecordUpdated != null)
		{
			EventTeamBattlefieldServerBestRecordUpdated();
		}
	}

	#endregion TeamBattlefield.Event

	#endregion TeamBattlefield
}