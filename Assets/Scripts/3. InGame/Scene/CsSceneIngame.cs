using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IIngameManagement
{
	void Test(int nNum);
	void Interaction(long lInstanceId, EnInteractionQuestType enInteractionQuestType, CsContinentObject csContinentObject);
	void StateEndOfInteraction();
	void TartgetReset();
	void CreateGold(Transform trMonster);
	void MonsterDead(long lInstanceId);
	void LookAtTarget(Vector3 vtPos);
	void DirectingEnd(bool bFade); // 연출 끝.
	void PortalAreaEnter(int nPortalId);
	void RemoveHeroCart(Guid guidHeroId, long lInstanceId);
	void TrasfomationAttackStart();
	void TrasfomationAttackEnd();

	bool MyHeroRequestRidingCart();
	void AttackByHit(Transform trTarget, PDHitResult pDHitResult, float flHitTime, PDHitResult pDAddHitResult, bool bMyHero, bool bKnockback = false);

	void NpcDialog(int nNpcId);
	CsCartObject GetCsCartObject(long lInstaceId);

	Transform GetAttacker(PDAttacker pDAttacker);
	void SelectHero(Transform trHero, bool bSelect);
	Guid GetHeroId(Transform trHero);
	CsHero GetHero(Transform trHero);
	CsHero GetHero(Guid guidHeroId);
	long GetInstanceId(Transform trTarget);
	int GetNpcId(Transform trNpc);

	CsMoveUnit GetCsMoveUnit(Transform trTarget);
	float GetHeroHeight(Guid guidHeroId);
	string GetName(Transform trTarget);
	CsJob GetHeroJob(Transform trTarget);
	CsNpcInfo GetNpcInfo(Transform trTarget);
	AudioClip GetNpcVoice(string strPrefabName);
	AudioClip GetMonsterSound(string strName);

	bool IsViewHero(Transform trHero);
	bool IsContinent();				// 플레이어가 대륙에 위치해있는지 아닌지 확인.
	bool IsDungeonStart();			// 플레이어가 던전인 경우 던전이 시작했는지를 확인.
	bool IsHeroStateIdle();
	bool IsHeroStateAttack();
	bool IsHeroStateNoJoyOfMove();

	void InitPlayThemes();
	void UninitPlayThemes();
}

public class CsSceneIngame : CsScene, IIngameManagement
{
	protected Dictionary<Guid, CsOtherPlayer> m_dicHeros = new Dictionary<Guid, CsOtherPlayer>();
	protected Dictionary<Guid, CsHeroClone> m_dicClone = new Dictionary<Guid, CsHeroClone>();
	protected Dictionary<int, CsNpc> m_dicNpcs = new Dictionary<int, CsNpc>();
	protected Dictionary<long, CsCartObject> m_dicCart = new Dictionary<long, CsCartObject>();
	protected Dictionary<long, CsInteractionObject> m_dicInteractionObject = new Dictionary<long, CsInteractionObject>();
	protected Dictionary<int, GameObject> m_dicObstacle = new Dictionary<int, GameObject>();
	protected Dictionary<int, CsPortalArea> m_dicPortal = new Dictionary<int, CsPortalArea>();

	protected Dictionary<long, CsMonster> m_dicMonsters = new Dictionary<long, CsMonster>();
	protected Dictionary<long, SEBMonsterOwnershipChangeEventBody> m_dicMissingOwnership = new Dictionary<long, SEBMonsterOwnershipChangeEventBody>();
	protected Dictionary<string, GameObject> m_dicMonsterResources = new Dictionary<string, GameObject>();

	protected CsMyPlayer m_csPlayer;
	protected AudioSource m_audioSource;

	protected bool m_bChaegeScene = true;
	protected bool m_bApplicationQuit = false;

	int m_nRemoveObjectCount = 0;
	int m_nUserViewLimit;

	List<CsOtherPlayer> m_listOtherPlayer = new List<CsOtherPlayer>();

	//----------------------------------------------------------------------------------------------------
	protected virtual void Awake()
	{
		Debug.Log("CsSceneIngame.Awake");
		CsIngameData.Instance.Scene = this;
		CsIngameData.Instance.IngameManagement = this;
		m_audioSource = GetComponent<AudioSource>();

		if (CsGameData.Instance.MyHeroInfo != null)
		{
			SetOptionSetting();
			ChangeBGM();
			StartCoroutine(StartGearReourcesAsync());               // Hero Low폴리곤 갑옷, 무기, 얼굴, 헤어 비동기 로드	
			StartCoroutine(ObjectSoundAsyncLoad());					// Sound 비동기 로드.
			GuiStyleSetting();                                      // 임시 프레임 표시 및 프레임 세팅.
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void Start()
	{
		Debug.Log("CsSceneIngame.Start");
		CsRplzSession.Instance.EventResHeroInitEnter += OnEventResHeroInitEnter;

		// Hero 이동
		CsRplzSession.Instance.EventEvtHeroEnter += OnEventEvtHeroEnter;
		CsRplzSession.Instance.EventEvtHeroExit += OnEventEvtHeroExit;
		CsRplzSession.Instance.EventEvtHeroMove += OnEventEvtHeroMove;
		CsRplzSession.Instance.EventEvtHeroMoveModeChanged += OnEventEvtHeroMoveModeChanged;

		// Hero 가속 Acceleration
		CsRplzSession.Instance.EventEvtAccelerationStarted += OnEventEvtAccelerationStarted;						// 당사자 영웅 가속시작.
		CsRplzSession.Instance.EventEvtHeroAccelerationStarted += OnEventEvtHeroAccelerationStarted;				// 타영웅 가속시작.
		CsRplzSession.Instance.EventEvtHeroAccelerationEnded += OnEventEvtHeroAccelerationEnded;					// 타영웅 가속종료.

		// Hero 전투 상태
		CsRplzSession.Instance.EventEvtBattleModeStart += OnEventEvtBattleModeStart;
		CsRplzSession.Instance.EventEvtBattleModeEnd += OnEventEvtBattleModeEnd;

		CsRplzSession.Instance.EventEvtHeroSkillCast += OnEventEvtHeroSkillCast;
		CsRplzSession.Instance.EventEvtHeroJobCommonSkillCast += OnEventEvtHeroJobCommonSkillCast;
		CsRplzSession.Instance.EventEvtHeroRankActiveSkillCast += OnEventEvtHeroRankActiveSkillCast;
		CsRplzSession.Instance.EventEvtHeroHit += OnEventEvtHeroHit;

		CsRplzSession.Instance.EventEvtHeroLevelUp += OnEventEvtHeroLevelUp;
		//CsRplzSession.Instance.EventEvtRevivalInvincibilityCanceled += OnEventEvtRevivalInvincibilityCanceled;			// 당사자 부활취소
		CsRplzSession.Instance.EventEvtHeroRevived += OnEventEvtHeroRevived;											// 타영웅 부활
		//CsRplzSession.Instance.EventEvtHeroRevivalInvincibilityCanceled += OnEventEvtHeroRevivalInvincibilityCanceled;	// 당사자 부활취소
		CsRplzSession.Instance.EventEvtMaxHpChanged += OnEventEvtMaxHpChanged; // 본인 최대HP변경

		// Hero 상태변경.
		CsRplzSession.Instance.EventEvtHeroMaxHpChanged += OnEventEvtHeroMaxHpChanged;
		CsRplzSession.Instance.EventEvtHeroHpRestored += OnEventEvtHeroHpRestored;
		CsRplzSession.Instance.EventEvtHeroBattleModeStart += OnEventEvtHeroBattleModeStart;
		CsRplzSession.Instance.EventEvtHeroBattleModeEnd += OnEventEvtHeroBattleModeEnd;

		// Hero 상태이상.
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectStart += OnEventHeroAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectHit += OnEventHeroAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectFinished += EventHeroAbnormalStateEffectFinished;

		// Hero 몬스터 변신
		CsRplzSession.Instance.EventEvtMainQuestMonsterTransformationCanceled += OnEventEvtMainQuestMonsterTransformationCanceled;
		CsRplzSession.Instance.EventEvtMainQuestMonsterTransformationFinished += OnEventEvtMainQuestMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationStarted += OnEventEvtHeroMainQuestMonsterTransformationStarted;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationCanceled += OnEventEvtHeroMainQuestMonsterTransformationCanceled;
		CsRplzSession.Instance.EventEvtHeroMainQuestMonsterTransformationFinished += OnEventEvtHeroMainQuestMonsterTransformationFinished;
		CsRplzSession.Instance.EventEvtHeroMainQuestTransformationMonsterSkillCast += OnEventEvtHeroMainQuestTransformationMonsterSkillCast;

		// Hero 탈것.
		CsRplzSession.Instance.EventEvtHeroMountGetOn += OnEventEvtHeroMountGetOn;
		CsRplzSession.Instance.EventEvtHeroMountGetOff += OnEventEvtHeroMountGetOff;
		CsRplzSession.Instance.EventEvtHeroMountLevelUp += OnEventEvtHeroMountLevelUp;

		// Hero 귀환주문서.
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseStart += OnEventEvtHeroReturnScrollUseStart;
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseCancel += OnEventEvtHeroReturnScrollUseCancel;
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseFinished += OnEventEvtHeroReturnScrollUseFinished;

		// Hero 장비
		CsRplzSession.Instance.EventEvtHeroMainGearEquip += OnEventEvtHeroMainGearEquip;
		CsRplzSession.Instance.EventEvtHeroMainGearUnequip += OnEventEvtHeroMainGearUnequip;

		// Hero Costume
		CsCostumeManager.Instance.EventCostumeEquip += OnEventCostumeEquip;
		CsCostumeManager.Instance.EventCostumeUnequip += OnEventCostumeUnequip;
		CsCostumeManager.Instance.EventCostumeEffectApply += OnEventCostumeEffectApply;
		CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;

		CsCostumeManager.Instance.EventHeroCostumeEquipped += OnEventHeroCostumeEquipped;
		CsCostumeManager.Instance.EventHeroCostumeUnequipped += OnEventHeroCostumeUnequipped;
		CsCostumeManager.Instance.EventHeroCostumeEffectApplied += OnEventHeroCostumeEffectApplied;

		// HeroArtifact
		CsArtifactManager.Instance.EventArtifactEquip += OnEventArtifactEquip;
		CsArtifactManager.Instance.EventHeroEquippedArtifactChanged += OnEventHeroEquippedArtifactChanged;

		// Hero 안전모드
		CsRplzSession.Instance.EventEvtSafeModeStarted += OnEventEvtSafeModeStarted;
		CsRplzSession.Instance.EventEvtSafeModeEnded += OnEventEvtSafeModeEnded;
		CsRplzSession.Instance.EventEvtHeroSafeModeStarted += OnEventEvtHeroSafeModeStarted;
		CsRplzSession.Instance.EventEvtHeroSafeModeEnded += OnEventEvtHeroSafeModeEnded;

		// GroggyMonster 상호작용.
		CsRplzSession.Instance.EventResGroggyMonsterItemStealStart += OnEventResGroggyMonsterItemStealStart;
		CsRplzSession.Instance.EventEvtGroggyMonsterItemStealFinished += OnEventEvtGroggyMonsterItemStealFinished;
		CsRplzSession.Instance.EventEvtGroggyMonsterItemStealCancel += OnEventEvtGroggyMonsterItemStealCancel;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealStart += OnEventEvtHeroGroggyMonsterItemStealStart;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealFinished += OnEventEvtHeroGroggyMonsterItemStealFinished;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealCancel += OnEventEvtHeroGroggyMonsterItemStealCancel;

		// 관심지역
		CsRplzSession.Instance.EventEvtHeroInterestAreaEnter += OnEventEvtHeroInterestAreaEnter;
		CsRplzSession.Instance.EventEvtHeroInterestAreaExit += OnEventEvtHeroInterestAreaExit;
		CsRplzSession.Instance.EventEvtHeroInterestTargetChange += OnEventEvtInterestTargetChange;

		// 몬스터
		CsRplzSession.Instance.EventEvtMonsterRemoved += OnEventEvtMonsterRemoved; // 몬스터 삭제.
		CsRplzSession.Instance.EventEvtMonsterInterestAreaEnter += OnEventEvtMonsterInterestAreaEnter;
		CsRplzSession.Instance.EventEvtMonsterInterestAreaExit += OnEventEvtMonsterInterestAreaExit;
		CsRplzSession.Instance.EventEvtMonsterOwnershipChange += OnEventEvtMonsterOwnershipChange;
		CsRplzSession.Instance.EventEvtMonsterMove += OnEventEvtMonsterMove;
		CsRplzSession.Instance.EventEvtMonsterSpawn += OnEventEvtMonsterSpawn;
		CsRplzSession.Instance.EventEvtMonsterSkillCast += OnEventEvtMonsterSkillCast;
		CsRplzSession.Instance.EventEvtMonsterHit += OnEventEvtMonsterHit;
		CsRplzSession.Instance.EventEvtMonsterMentalHit += OnEventEvtMonsterMentalHit;
		CsRplzSession.Instance.EventEvtMonsterReturnModeChanged += OnEventEvtMonsterReturnModeChanged;

		// 몬스터 상태이상
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectStart += OnEventEvtMonsterAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectHit += OnEventEvtMonsterAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectFinished += OnEventEvtMonsterAbnormalStateEffectFinished;

		// Guild
		CsGuildManager.Instance.EventGuildCreate += OnEventGuildCreate;
		CsGuildManager.Instance.EventGuildInvitationAccept += OnEventGuildInvitationAccept;
		CsGuildManager.Instance.EventGuildExit += OnEventGuildExit;
		CsGuildManager.Instance.EventGuildMasterTransfer += OnEventGuildMasterTransfer;

		CsGuildManager.Instance.EventGuildApplicationAccepted += OnEventGuildApplicationAccepted;
		CsGuildManager.Instance.EventHeroGuildInfoUpdated += OnEventHeroGuildInfoUpdated;
		CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
		CsGuildManager.Instance.EventGuildAppointed += OnEventGuildAppointed;
		CsGuildManager.Instance.EventGuildMasterTransferred += OnEventGuildMasterTransferred;

		// Title
		CsTitleManager.Instance.EventDisplayTitleSet += OnEventDisplayTitleSet;
		CsTitleManager.Instance.EventTitleLifetimeEnded += OnEventTitleLifetimeEnded;
		CsTitleManager.Instance.EventHeroDisplayTitleChanged += OnEventHeroDisplayTitleChanged;

		// Creature
		CsCreatureManager.Instance.EventCreatureParticipate += OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel += OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventHeroCreatureParticipated += OnEventHeroCreatureParticipated;
		CsCreatureManager.Instance.EventHeroCreatureParticipationCanceled += OnEventHeroCreatureParticipationCanceled;

		// FishingQuest
		CsFishingQuestManager.Instance.EventFishingZone += OnEventFishingZone;
		CsFishingQuestManager.Instance.EventMyHeroFishingStart += OnEventMyHeroFishingStart;
		CsFishingQuestManager.Instance.EventFishingCanceled += OnEventFishingCanceled;
		CsFishingQuestManager.Instance.EventFishingCastingCompleted += OnEventFishingCastingCompleted;
		CsFishingQuestManager.Instance.EventHeroFishingStarted += OnEventHeroFishingStarted;
		CsFishingQuestManager.Instance.EventHeroFishingCanceled += OnEventHeroFishingCanceled;
		CsFishingQuestManager.Instance.EventHeroFishingCompleted += OnEventHeroFishingCompleted;

		// NationAlliance
		CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept += OnEventNationAllianceApplicationAccept;
		CsNationAllianceManager.Instance.EventNationAllianceBreak += OnEventNationAllianceBreak;
		CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;		
		CsNationAllianceManager.Instance.EventNationAllianceBroken += OnEventNationAllianceBroken;

		// HeroJobChanged
		CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;

		CsGameEventToIngame.Instance.EventPlayerPrefsKeySet += OnEventPlayerPrefsKeySet;
		CsGameEventToIngame.Instance.EventLoadingUIComplete += OnEventLoadingUIComplete;
		CsGameEventToIngame.Instance.EventOtherHeroView += OnEventOtherHeroView;

		CsRplzSession.Instance.EventEvtHeroRankAcquired += OnEventHeroRankAcquired;

		StartCoroutine(HeroViewSetting());
	}

	//----------------------------------------------------------------------------------------------------
	void OnApplicationQuit()
	{
		m_bApplicationQuit = true;
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		Debug.Log("CsSceneIngame.OnDestroy()");

		CsRplzSession.Instance.EventResHeroInitEnter -= OnEventResHeroInitEnter;

		// Hero 이동
		CsRplzSession.Instance.EventEvtHeroEnter -= OnEventEvtHeroEnter;
		CsRplzSession.Instance.EventEvtHeroExit -= OnEventEvtHeroExit;
		CsRplzSession.Instance.EventEvtHeroMove -= OnEventEvtHeroMove;
		CsRplzSession.Instance.EventEvtHeroMoveModeChanged -= OnEventEvtHeroMoveModeChanged;

		// Hero 가속 Acceleration
		CsRplzSession.Instance.EventEvtAccelerationStarted -= OnEventEvtAccelerationStarted;                        // 당사자 영웅 가속시작.
		CsRplzSession.Instance.EventEvtHeroAccelerationStarted -= OnEventEvtHeroAccelerationStarted;                // 타영웅 가속시작.
		CsRplzSession.Instance.EventEvtHeroAccelerationEnded -= OnEventEvtHeroAccelerationEnded;                    // 타영웅 가속종료.

		// Hero 전투 상태
		CsRplzSession.Instance.EventEvtBattleModeStart -= OnEventEvtBattleModeStart;
		CsRplzSession.Instance.EventEvtBattleModeEnd -= OnEventEvtBattleModeEnd;

		CsRplzSession.Instance.EventEvtHeroSkillCast -= OnEventEvtHeroSkillCast;
		CsRplzSession.Instance.EventEvtHeroJobCommonSkillCast -= OnEventEvtHeroJobCommonSkillCast;
		CsRplzSession.Instance.EventEvtHeroRankActiveSkillCast -= OnEventEvtHeroRankActiveSkillCast;
		CsRplzSession.Instance.EventEvtHeroHit -= OnEventEvtHeroHit;

		CsRplzSession.Instance.EventEvtHeroLevelUp -= OnEventEvtHeroLevelUp;
		CsRplzSession.Instance.EventEvtHeroRevived -= OnEventEvtHeroRevived;
		CsRplzSession.Instance.EventEvtMaxHpChanged -= OnEventEvtMaxHpChanged; // 본인 최대HP변경

		// Other 상태변경.
		CsRplzSession.Instance.EventEvtHeroMaxHpChanged -= OnEventEvtHeroMaxHpChanged;
		CsRplzSession.Instance.EventEvtHeroHpRestored -= OnEventEvtHeroHpRestored;
		CsRplzSession.Instance.EventEvtHeroBattleModeStart -= OnEventEvtHeroBattleModeStart;
		CsRplzSession.Instance.EventEvtHeroBattleModeEnd -= OnEventEvtHeroBattleModeEnd;

		// Hero 상태이상.
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectStart -= OnEventHeroAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectHit -= OnEventHeroAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtHeroAbnormalStateEffectFinished -= EventHeroAbnormalStateEffectFinished;

		// Hero 탈것.
		CsRplzSession.Instance.EventEvtHeroMountGetOn -= OnEventEvtHeroMountGetOn;
		CsRplzSession.Instance.EventEvtHeroMountGetOff -= OnEventEvtHeroMountGetOff;
		CsRplzSession.Instance.EventEvtHeroMountLevelUp -= OnEventEvtHeroMountLevelUp;

		// Hero 귀환주문서.
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseStart -= OnEventEvtHeroReturnScrollUseStart;
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseCancel -= OnEventEvtHeroReturnScrollUseCancel;
		CsRplzSession.Instance.EventEvtHeroReturnScrollUseFinished -= OnEventEvtHeroReturnScrollUseFinished;

		// Hero 장비
		CsRplzSession.Instance.EventEvtHeroMainGearEquip -= OnEventEvtHeroMainGearEquip;
		CsRplzSession.Instance.EventEvtHeroMainGearUnequip -= OnEventEvtHeroMainGearUnequip;

		// Hero Costume
		CsCostumeManager.Instance.EventCostumeEquip -= OnEventCostumeEquip;
		CsCostumeManager.Instance.EventCostumeUnequip -= OnEventCostumeUnequip;
		CsCostumeManager.Instance.EventCostumeEffectApply -= OnEventCostumeEffectApply;
		CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;

		CsCostumeManager.Instance.EventHeroCostumeEquipped -= OnEventHeroCostumeEquipped;
		CsCostumeManager.Instance.EventHeroCostumeUnequipped -= OnEventHeroCostumeUnequipped;
		CsCostumeManager.Instance.EventHeroCostumeEffectApplied -= OnEventHeroCostumeEffectApplied;

		// HeroArtifact
		CsArtifactManager.Instance.EventArtifactEquip -= OnEventArtifactEquip;
		CsArtifactManager.Instance.EventHeroEquippedArtifactChanged -= OnEventHeroEquippedArtifactChanged;

		// Hero 안전모드
		CsRplzSession.Instance.EventEvtSafeModeStarted -= OnEventEvtSafeModeStarted;
		CsRplzSession.Instance.EventEvtSafeModeEnded -= OnEventEvtSafeModeEnded;
		CsRplzSession.Instance.EventEvtHeroSafeModeStarted -= OnEventEvtHeroSafeModeStarted;
		CsRplzSession.Instance.EventEvtHeroSafeModeEnded -= OnEventEvtHeroSafeModeEnded;

		// GroggyMonster 상호작용.
		CsRplzSession.Instance.EventResGroggyMonsterItemStealStart -= OnEventResGroggyMonsterItemStealStart;
		CsRplzSession.Instance.EventEvtGroggyMonsterItemStealFinished -= OnEventEvtGroggyMonsterItemStealFinished;
		CsRplzSession.Instance.EventEvtGroggyMonsterItemStealCancel -= OnEventEvtGroggyMonsterItemStealCancel;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealStart -= OnEventEvtHeroGroggyMonsterItemStealStart;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealFinished -= OnEventEvtHeroGroggyMonsterItemStealFinished;
		CsRplzSession.Instance.EventEvtHeroGroggyMonsterItemStealCancel -= OnEventEvtHeroGroggyMonsterItemStealCancel;

		// 관심지역
		CsRplzSession.Instance.EventEvtHeroInterestAreaEnter -= OnEventEvtHeroInterestAreaEnter;
		CsRplzSession.Instance.EventEvtHeroInterestAreaExit -= OnEventEvtHeroInterestAreaExit;
		CsRplzSession.Instance.EventEvtHeroInterestTargetChange -= OnEventEvtInterestTargetChange;

		// 몬스터
		CsRplzSession.Instance.EventEvtMonsterRemoved -= OnEventEvtMonsterRemoved; // 몬스터 삭제.
		CsRplzSession.Instance.EventEvtMonsterInterestAreaEnter -= OnEventEvtMonsterInterestAreaEnter;
		CsRplzSession.Instance.EventEvtMonsterInterestAreaExit -= OnEventEvtMonsterInterestAreaExit;
		CsRplzSession.Instance.EventEvtMonsterOwnershipChange -= OnEventEvtMonsterOwnershipChange;
		CsRplzSession.Instance.EventEvtMonsterMove -= OnEventEvtMonsterMove;
		CsRplzSession.Instance.EventEvtMonsterSpawn -= OnEventEvtMonsterSpawn;
		CsRplzSession.Instance.EventEvtMonsterSkillCast -= OnEventEvtMonsterSkillCast;
		CsRplzSession.Instance.EventEvtMonsterHit -= OnEventEvtMonsterHit;
		CsRplzSession.Instance.EventEvtMonsterReturnModeChanged -= OnEventEvtMonsterReturnModeChanged;

		// 몬스터 상태이상
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectStart -= OnEventEvtMonsterAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectHit -= OnEventEvtMonsterAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtMonsterAbnormalStateEffectFinished -= OnEventEvtMonsterAbnormalStateEffectFinished;

		// Guild
		CsGuildManager.Instance.EventGuildCreate -= OnEventGuildCreate;
		CsGuildManager.Instance.EventGuildInvitationAccept -= OnEventGuildInvitationAccept;
		CsGuildManager.Instance.EventGuildExit -= OnEventGuildExit;
		CsGuildManager.Instance.EventGuildMasterTransfer -= OnEventGuildMasterTransfer;

		CsGuildManager.Instance.EventGuildApplicationAccepted -= OnEventGuildApplicationAccepted;
		CsGuildManager.Instance.EventHeroGuildInfoUpdated -= OnEventHeroGuildInfoUpdated;
		CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
		CsGuildManager.Instance.EventGuildAppointed -= OnEventGuildAppointed;
		CsGuildManager.Instance.EventGuildMasterTransferred -= OnEventGuildMasterTransferred;

		// Title
		CsTitleManager.Instance.EventDisplayTitleSet -= OnEventDisplayTitleSet;
		CsTitleManager.Instance.EventTitleLifetimeEnded -= OnEventTitleLifetimeEnded;
		CsTitleManager.Instance.EventHeroDisplayTitleChanged -= OnEventHeroDisplayTitleChanged;

		// Creature
		CsCreatureManager.Instance.EventCreatureParticipate -= OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel -= OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventHeroCreatureParticipated -= OnEventHeroCreatureParticipated;
		CsCreatureManager.Instance.EventHeroCreatureParticipationCanceled -= OnEventHeroCreatureParticipationCanceled;

		// FishingQuest
		CsFishingQuestManager.Instance.EventFishingZone -= OnEventFishingZone;
		CsFishingQuestManager.Instance.EventMyHeroFishingStart -= OnEventMyHeroFishingStart;
		CsFishingQuestManager.Instance.EventFishingCanceled -= OnEventFishingCanceled;
		CsFishingQuestManager.Instance.EventFishingCastingCompleted -= OnEventFishingCastingCompleted;
		CsFishingQuestManager.Instance.EventHeroFishingStarted -= OnEventHeroFishingStarted;
		CsFishingQuestManager.Instance.EventHeroFishingCanceled -= OnEventHeroFishingCanceled;
		CsFishingQuestManager.Instance.EventHeroFishingCompleted -= OnEventHeroFishingCompleted;

		// NationAlliance
		CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept -= OnEventNationAllianceApplicationAccept;
		CsNationAllianceManager.Instance.EventNationAllianceBreak -= OnEventNationAllianceBreak;
		CsNationAllianceManager.Instance.EventNationAllianceConcluded -= OnEventNationAllianceConcluded;
		CsNationAllianceManager.Instance.EventNationAllianceBroken -= OnEventNationAllianceBroken;

		// HeroJobChanged
		CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;

		CsGameEventToIngame.Instance.EventPlayerPrefsKeySet -= OnEventPlayerPrefsKeySet;
		CsGameEventToIngame.Instance.EventLoadingUIComplete -= OnEventLoadingUIComplete;
		CsGameEventToIngame.Instance.EventOtherHeroView -= OnEventOtherHeroView;

		CsRplzSession.Instance.EventEvtHeroRankAcquired -= OnEventHeroRankAcquired;

		m_dicHeros.Clear();
		m_dicNpcs.Clear();
		m_dicInteractionObject.Clear();
		m_dicObstacle.Clear();

		m_dicMonsters.Clear();
		m_dicMonsterResources.Clear();
		m_dicMissingOwnership.Clear();

		m_csPlayer = null;
		m_audioSource = null;
		StopAllCoroutines();
	}

	#region 임시 프레임 표시

	float m_flTimer = 0.0f;
	float m_flDeltaTime = 0.0f;
	GUIStyle m_guistyle;
	Rect m_rect;

	//---------------------------------------------------------------------------------------------------
	// Gui 스타일 생성
	//---------------------------------------------------------------------------------------------------
	void GuiStyleSetting()
	{
		m_rect = new Rect(0, 0, Screen.width, (Screen.height * 190) / 100);
		m_guistyle = new GUIStyle();
		m_guistyle.alignment = TextAnchor.MiddleCenter;
		m_guistyle.fontSize = (int)((Screen.height * 2.5f) / 100);
		m_guistyle.normal.textColor = Color.white;
		m_guistyle.stretchHeight = true;
		m_flTimer = Time.time;
		Debug.Log("GuiStyleSetting     PlayerPrefsKeyFrame : " + CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Frame) + " Application.targetFrameRate : " + Application.targetFrameRate);
	}

	//---------------------------------------------------------------------------------------------------
	// 현재 프레임을 체크 / 소스로 GUI 표시. 
	//---------------------------------------------------------------------------------------------------

	List<float> m_listFps = new List<float>();
	float m_flFps = 0;

	void OnGUI()
	{
		//if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			//if (CsConfiguration.Instance.ServerType == CsConfiguration.EnServerType.Dev)
			{
				if (m_csPlayer == null) return;
				if (CsIngameData.Instance.InGameCamera == null) return;

				m_flDeltaTime += (Time.deltaTime - m_flDeltaTime) * 0.1f;       // 임시 프레임 표시
				float flFps = 1.0f / m_flDeltaTime;  //초당 프레임 - 1초에

				m_listFps.Add(flFps);

				if (Time.time > m_flTimer + 1f)
				{
					float flValue = 0;
					for (int i = 0; i < m_listFps.Count; i++)
					{
						flValue += m_listFps[i];
					}

					m_flFps = flValue / m_listFps.Count;
					m_listFps.Clear();
					m_flTimer = Time.time;
				}

				string m_strText = " [ " + m_flFps.ToString("F1") + " / " + Application.targetFrameRate.ToString("F1") + " , " + Screen.currentResolution + " , " + CsConfiguration.Instance.ServerType + "  ]   ";
				GUI.Label(m_rect, m_strText, m_guistyle);
			}
		}
	}

	#endregion 임시 프레임 표시

	#region SendToServer

	protected bool m_bWaitResponse = false;
	//---------------------------------------------------------------------------------------------------
	protected void SendHeroInitEnterCommand()
	{
		Debug.Log("SendHeroInitEnterCommand   m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			HeroInitEnterCommandBody csEvt = new HeroInitEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.HeroInitEnter, csEvt);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendPrevContinentEnterCommand()
	{
		Debug.Log("SendPrevContinentEnterCommand   m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			PrevContinentEnterCommandBody csEvt = new PrevContinentEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.PrevContinentEnter, csEvt);
		}
	}

	#endregion SendToServer

	#region ClientCommon
	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventResHeroInitEnter(int nReturnCode, HeroInitEnterResponseBody csRes) // 기본 입장.
	{
		Debug.Log("OnEventHeroInitEnter         nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetMyHeroLocation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationId);
			SetEnter(csRes.position, csRes.rotationY, csRes.placeInstanceId, csRes.heroes, csRes.monsterInsts, csRes.continentObjectInsts, csRes.cartInsts, true);
		}
		else
		{
			if (nReturnCode == 101)
			{
				Debug.Log("OnEventResHeroInitEnter        nReturnCode == 102 : 대상 장소는 국가전중입니다. ");
				CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
				CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
			}
		}
	}

	#endregion ClientCommon

	#region Hero

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventEvtHeroEnter(SEBHeroEnterEventBody csEvt)
	{
		StartCoroutine(AsyncCreateHero(csEvt.hero, !csEvt.isRevivalEnter, csEvt.isRevivalEnter));
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventEvtHeroExit(SEBHeroExitEventBody csEvt)
	{
		RemoveHero(csEvt.heroId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMove(SEBHeroMoveEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId) == true)
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMove(CsRplzSession.Translate(csEvt.position), csEvt.rotationY);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMoveModeChanged(SEBHeroMoveModeChangedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMoveModeChanged(csEvt.isWalking);
		}
	}

	#region HeroAcceleration

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtAccelerationStarted(SEBAccelerationStartedEventBody csEvt)
	{
		m_csPlayer.NetEventAccelerationStarted();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroAccelerationStarted(SEBHeroAccelerationStartedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroAccelerationStarted();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroAccelerationEnded(SEBHeroAccelerationEndedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroAccelerationEnded();
		}
	}

	#endregion HeroAcceleration

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSkillCast(SEBHeroSkillCastEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId) == true)
		{
			m_dicHeros[csEvt.heroId].NetEventHeroSkillCast(csEvt.skillId,
														   csEvt.chainSkillId,
														   CsRplzSession.Translate(csEvt.heroTargetPosition),
														   CsRplzSession.Translate(csEvt.skillTargetPosition));
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtBattleModeStart(SEBBattleModeStartEventBody csEvt) // 당사자.
	{
		m_csPlayer.NetEventBattleModeStart();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtBattleModeEnd(SEBBattleModeEndEventBody csEvt) // 당사자.
	{
		if (m_csPlayer != null)
		{
			m_csPlayer.NetEventBattleModeEnd();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventEvtHeroHit(SEBHeroHitEventBody csEvt) // 당사자 포함 관련유저. 유저 피격시.
	{
		Transform trTarget = m_csPlayer.transform;

		if (m_dicHeros.ContainsKey(csEvt.heroId)) // 대상이 Other일때.
		{
			trTarget = m_dicHeros[csEvt.heroId].transform;
		}

		if (csEvt.hitResult.hp == 0)
		{
			foreach (var dicMonsters in m_dicMonsters)
			{
				dicMonsters.Value.NetEventTargetHeroDead(trTarget);
			}

			foreach (var dicHeros in m_dicHeros)
			{
				dicHeros.Value.NetEventTargetHeroDead(trTarget);
			}

			HeroHit(csEvt.heroId, GetAttacker(csEvt.hitResult.attacker), csEvt.hitResult); // 피격자에게 정보 전달.
		}

		if (csEvt.hitResult.attacker.type == 1)  // type  1 Hero
		{
			Guid guidHeroId = GetHeroId(csEvt.hitResult.attacker);

			if (m_dicHeros.ContainsKey(guidHeroId)) // other >> my, other
			{
				m_dicHeros[guidHeroId].NetEventHitApprove(csEvt.hitResult, trTarget);
			}
			else if (m_csPlayer.HeroId == guidHeroId)
			{
				m_csPlayer.NetEventHitApprove(csEvt.hitResult, trTarget);
			}
		}
		else if (csEvt.hitResult.attacker.type == 2) // type  2 Monster
		{
			long longMonsterAttackerId = GetMonsterId(csEvt.hitResult.attacker); // 현재 서버에서 받아온 몬스터 아이디값을 받아오는 변수.

			if (m_dicMonsters.ContainsKey(longMonsterAttackerId)) // moster >> my, other
			{
				m_dicMonsters[longMonsterAttackerId].NetEventHitApprove(csEvt.hitResult, trTarget); // 몬스터가 히어로 힛 정보까지 가져감.
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void OnEventEvtHeroJobCommonSkillCast(SEBHeroJobCommonSkillCastEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroJobCommonSkillCast(csEvt.skillId, CsRplzSession.Translate(csEvt.skillTargetPosition));
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void OnEventEvtHeroRankActiveSkillCast(SEBHeroRankActiveSkillCastEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroRankActiveSkillCast(csEvt.skillId, CsRplzSession.Translate(csEvt.skillTargetPosition));
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventEvtHeroLevelUp(SEBHeroLevelUpEventBody csEvt) // 당사자 외 레벨업. 
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroLevelUp(csEvt.level, csEvt.maxHp, csEvt.hp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventHeroAbnormalStateEffectStart(SEBHeroAbnormalStateEffectStartEventBody csEvt) // 당사자 포함 관련 유저
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroAbnormalStateEffectStart(csEvt.abnormalStateEffectInstanceId,
																		  csEvt.abnormalStateId,
																		  csEvt.sourceJobId,
																		  csEvt.level,
																		  csEvt.damageAbsorbShieldRemainingAbsorbAmount,
																		  csEvt.remainingTime,
																		  csEvt.damageAbsorbShieldRemainingAbsorbAmount,
																		  csEvt.removedAbnormalStateEffects);
		}
		else if (m_csPlayer.HeroId == csEvt.heroId)
		{
			m_csPlayer.NetEventHeroAbnormalStateEffectStart(csEvt.abnormalStateEffectInstanceId,
															csEvt.abnormalStateId,
															csEvt.sourceJobId,
															csEvt.level,
															csEvt.damageAbsorbShieldRemainingAbsorbAmount,
															csEvt.remainingTime,
															csEvt.damageAbsorbShieldRemainingAbsorbAmount,
															csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventHeroAbnormalStateEffectHit(SEBHeroAbnormalStateEffectHitEventBody csEvt) // 당사자 포함 관련 유저
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId) == true)
		{
			m_dicHeros[csEvt.heroId].NetEventHeroAbnormalStateEffectHit(csEvt.hp,
																		csEvt.removedAbnormalStateEffects,
																		csEvt.abnormalStateEffectInstanceId,
																		csEvt.damage, csEvt.hpDamage,
																		GetAttacker(csEvt.attacker));
		}
		else if (m_csPlayer.HeroId == csEvt.heroId)
		{
			m_csPlayer.NetEventHeroAbnormalStateEffectHit(csEvt.hp,
														  csEvt.removedAbnormalStateEffects,
														  csEvt.abnormalStateEffectInstanceId,
														  csEvt.damage, csEvt.hpDamage,
														  GetAttacker(csEvt.attacker));
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void EventHeroAbnormalStateEffectFinished(SEBHeroAbnormalStateEffectFinishedEventBody csEvt) // 당사자 포함 관련 유저
	{
		if (m_csPlayer == null) return;
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroAbnormalStateEffectFinished(csEvt.abnormalStateEffectInstanceId);
		}
		else if (m_csPlayer.HeroId == csEvt.heroId)
		{
			m_csPlayer.NetEventHeroAbnormalStateEffectFinished(csEvt.abnormalStateEffectInstanceId);
		}
	}

	// Hero 몬스터 변신
	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMainQuestMonsterTransformationCanceled(SEBMainQuestMonsterTransformationCanceledEventBody csEvt)	// 메인퀘스트몬스터변신취소
	{
		Debug.Log("OnEventEvtMainQuestMonsterTransformationCanceled");
		if (m_csPlayer != null)
		{
			m_csPlayer.NetEventTransformationMonsterFinish(csEvt.maxHP, csEvt.hp, csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMainQuestMonsterTransformationFinished(SEBMainQuestMonsterTransformationFinishedEventBody csEvt) // 메인퀘스트몬스터변신종료
	{
		Debug.Log("OnEventEvtMainQuestMonsterTransformationFinished");
		if (m_csPlayer != null)
		{
			m_csPlayer.NetEventTransformationMonsterFinish(csEvt.maxHP, csEvt.hp, csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationStarted(SEBHeroMainQuestMonsterTransformationStartedEventBody csEvt) // 영웅메인퀘스트몬스터변신시작
	{
		Debug.Log("OnEventEvtHeroMainQuestMonsterTransformationStarted");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventTransformationMonster(csEvt.transformationMonsterId, csEvt.maxHP, csEvt.hp, csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationCanceled(SEBHeroMainQuestMonsterTransformationCanceledEventBody csEvt) // 영웅메인퀘스트몬스터변신취소
	{
		Debug.Log("OnEventEvtHeroMainQuestMonsterTransformationCanceled");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventTransformationMonsterFinish(csEvt.maxHP, csEvt.hp, csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestMonsterTransformationFinished(SEBHeroMainQuestMonsterTransformationFinishedEventBody csEvt) // 영웅메인퀘스트몬스터변신취소
	{
		Debug.Log("OnEventEvtHeroMainQuestMonsterTransformationFinished");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventTransformationMonsterFinish(csEvt.maxHP, csEvt.hp, csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainQuestTransformationMonsterSkillCast(SEBHeroMainQuestTransformationMonsterSkillCastEventBody csEvt) // 영웅메인퀘스트변신몬스터스킬시전
	{
		Debug.Log("OnEventEvtHeroMainQuestTransformationMonsterSkillCast");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventTransformationMonsterSkillCast(csEvt.skillId, CsRplzSession.Translate(csEvt.skillTargetPosition));
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMountGetOn(SEBHeroMountGetOnEventBody csEvt) // 영웅탈것타기   당사자 외.
	{
		Debug.Log("OnEventEvtHeroMountGetOn");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMountGetOn(csEvt.mountId, csEvt.level);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMountGetOff(SEBHeroMountGetOffEventBody csEvt) // 영웅탈것내리기   당사자 포함.
	{
		Debug.Log("OnEventEvtHeroMountGetOff");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMountGetOff();
		}
		else if (CsGameData.Instance.MyHeroInfo.HeroId == csEvt.heroId)
		{
			if (m_csPlayer == null)
			{
				CsGameData.Instance.MyHeroInfo.IsRiding = false;
			}
			else
			{
				m_csPlayer.NetEventHeroMountGetOff();
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMountLevelUp(SEBHeroMountLevelUpEventBody csEvt) // 영웅탈것레벨업   당사자 외.
	{
		Debug.Log("OnEventEvtHeroMountLevelUp");
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMountLevelUp(csEvt.mountId, csEvt.mountLevel);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroRevived(SEBHeroRevivedEventBody csEvt)
	{

	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMaxHpChanged(SEBMaxHpChangedEventBody csEvt) // 본인 최대 Hp 변경
	{
		m_csPlayer.NetEventChangedMaxHp(csEvt.maxHp, csEvt.hp);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMaxHpChanged(SEBHeroMaxHpChangedEventBody csEvt) // 당사자 외 영웅최대HP변경
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventChangedMaxHp(csEvt.maxHp, csEvt.hp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroHpRestored(SEBHeroHpRestoredEventBody csEvt) // 당사자 외 영웅HP회복
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventEvtHeroHpRestored(csEvt.hp);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroBattleModeStart(SEBHeroBattleModeStartEventBody csEvt) // 당사자 외 영웅전투모드시작
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventEvtHeroBattleMode(true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroBattleModeEnd(SEBHeroBattleModeEndEventBody csEvt) // 당사자 외 영웅전투모드종료
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventEvtHeroBattleMode(false);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void OnEventEvtHeroInterestAreaEnter(SEBHeroInterestAreaEnterEventBody csEvt)
	{
		Debug.Log("OnEventEvtHeroInterestAreaEnter");
		StartCoroutine(AsyncCreateHero(csEvt.hero, false, false));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroInterestAreaExit(SEBHeroInterestAreaExitEventBody csEvt)
	{
		RemoveHero(csEvt.heroId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtInterestTargetChange(SEBInterestTargetChangeEventBody csEvt) // 관심지역 변경.
	{
		for (int i = 0; i < csEvt.addedHeroes.Length; i++) // 추가된 영웅
		{
			Debug.Log("OnEventEvtInterestTargetChange");
			StartCoroutine(AsyncCreateHero(csEvt.addedHeroes[i], false, false));
		}

		for (int i = 0; i < csEvt.removedHeroes.Length; i++)    // 삭제된 영웅
		{
			Debug.Log("OnEventEvtInterestTargetChange Remove");
			RemoveHero(csEvt.removedHeroes[i]);
		}

		for (int i = 0; i < csEvt.addedMonsterInsts.Length; i++) // 추가된 몬스터
		{
			StartCoroutine(AsyncCreateMonster(csEvt.addedMonsterInsts[i]));
		}

		for (int i = 0; i < csEvt.removedMonsterInsts.Length; i++) // 삭제된 몬스터
		{
			RemoveMonster(csEvt.removedMonsterInsts[i]);
		}

		for (int i = 0; i < csEvt.addedContinentObjectInsts.Length; i++) // 추가된 상호작용오브젝트
		{
			CreateInteractionObject(csEvt.addedContinentObjectInsts[i].arrangeNo, csEvt.addedContinentObjectInsts[i].instanceId, csEvt.addedContinentObjectInsts[i].interactionHeroId);
		}

		for (int i = 0; i < csEvt.removedContinentObjectInsts.Length; i++) // 삭제된 상호작용오브젝트
		{
			RemoveInteractionObject(csEvt.removedContinentObjectInsts[i]);
		}

		for (int i = 0; i < csEvt.addedCartInsts.Length; i++)
		{
			StartCoroutine(CreateCart(csEvt.addedCartInsts[i]));
		}

		for (int i = 0; i < csEvt.removedCartInsts.Length; i++)
		{
			RemoveCart(csEvt.removedCartInsts[i]);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainGearEquip(SEBHeroMainGearEquipEventBody csEvt) // 당사자 이외 무기교체.
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMainGearEquip(csEvt.heroMainGear.id, csEvt.heroMainGear.mainGearId, csEvt.heroMainGear.enchantLevel);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMainGearUnequip(SEBHeroMainGearUnequipEventBody csEvt) // 당사자 이외 무기해제.
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroMainGearUnequip(csEvt.heroMainGearId);
		}
	}

	// Hero Costume
	//----------------------------------------------------------------------------------------------------
	void OnEventCostumeEquip(int nCostumeId, int nCostumeEffectId)
	{
		Debug.Log("OnEventCostumeEquip");
		m_csPlayer.NetEventCostumeEquip(nCostumeId, nCostumeEffectId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCostumeEffectApply(int nCostumeEffectId)
	{
		Debug.Log("OnEventCostumeEffectApply");
		m_csPlayer.NetEventCostumeEffectApply(nCostumeEffectId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCostumeUnequip()
	{
		Debug.Log("OnEventCostumeUnequip");
		m_csPlayer.NetEventCostumeUnequip();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCostumePeriodExpired()
	{
		Debug.Log("OnEventCostumePeriodExpired");
		m_csPlayer.NetEventCostumeUnequip();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroCostumeEquipped(Guid guidHeroId, int nCostumeId, int nCostumeEffectId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventCostumeEquip(nCostumeId, nCostumeEffectId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroCostumeEffectApplied(Guid guidHeroId, int nCostumeEffectId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventCostumeEffectApply(nCostumeEffectId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroCostumeUnequipped(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventCostumeUnequip();
		}
	}

	// HeroArtifact
	//----------------------------------------------------------------------------------------------------
	void OnEventArtifactEquip()
	{
		m_csPlayer.NetEventArtifactEquip();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroEquippedArtifactChanged(Guid guidHeroId, int nEquippedArtifactNo)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventHeroEquippedArtifactChanged(nEquippedArtifactNo);
		}
	}

	// Hero 안전모드
	//----------------------------------------------------------------------------------------------------
	void OnEventEvtSafeModeStarted(SEBSafeModeStartedEventBody csEvt)
	{
		m_csPlayer.NetEventSafeModeStarted();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtSafeModeEnded(SEBSafeModeEndedEventBody csEvt)
	{
		m_csPlayer.NetEventSafeModeEnded();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSafeModeStarted(SEBHeroSafeModeStartedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventSafeModeStarted();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSafeModeEnded(SEBHeroSafeModeEndedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventSafeModeEnded();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResGroggyMonsterItemStealStart(int nReturnCode, GroggyMonsterItemStealStartResponseBody csRes)
	{
		Debug.Log("OnEventResGroggyMonsterItemStealStart     nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_csPlayer.NetGroggyMonsterItemStealStart(true);
		}
		else
		{
			m_csPlayer.NetGroggyMonsterItemStealStart(false);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtGroggyMonsterItemStealFinished(SEBGroggyMonsterItemStealFinishedEventBody csEvt)
	{
		Debug.Log("OnEventEvtGroggyMonsterItemStealFinished");
		m_csPlayer.NetGroggyMonsterItemStealFinished();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtGroggyMonsterItemStealCancel(SEBGroggyMonsterItemStealCancelEventBody csEvt)
	{
		Debug.Log("OnEventEvtGroggyMonsterItemStealCancel");
		m_csPlayer.NetGroggyMonsterItemStealFinished();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroGroggyMonsterItemStealStart(SEBHeroGroggyMonsterItemStealStartEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroGroggyMonsterItemStealStart");
			m_dicHeros[csEvt.heroId].NetGroggyMonsterItemStealStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroGroggyMonsterItemStealFinished(SEBHeroGroggyMonsterItemStealFinishedEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroGroggyMonsterItemStealFinished");
			m_dicHeros[csEvt.heroId].NetGroggyMonsterItemStealFinished();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroGroggyMonsterItemStealCancel(SEBHeroGroggyMonsterItemStealCancelEventBody csEvt)
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroGroggyMonsterItemStealCancel");
			m_dicHeros[csEvt.heroId].NetGroggyMonsterItemStealFinished();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroReturnScrollUseStart(SEBHeroReturnScrollUseStartEventBody csEvt) // 당사자 외.
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroReturnScrollCastStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroReturnScrollUseCancel(SEBHeroReturnScrollUseCancelEventBody csEvt) // 당사자 외.
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroReturnScrollCastEnd();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroReturnScrollUseFinished(SEBHeroReturnScrollUseFinishedEventBody csEvt) // 당사자 외.
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_dicHeros[csEvt.heroId].NetEventHeroReturnScrollCastEnd();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRankAcquired(SEBHeroRankAcquiredEventBody eventBody) // UI로부터 받아온 랭크 변경 이벤트
	{
		if (m_dicHeros.ContainsKey(eventBody.heroId))
		{
			m_dicHeros[eventBody.heroId].NetEventHeroDisplayRank(eventBody.rankNo);
		}
	}

	#endregion Hero

	#region Monster

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterRemoved(SEBMonsterRemovedEventBody csEvt) // 몬스터 제거.
	{
		RemoveMonster(csEvt.monsterInstanceId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterInterestAreaEnter(SEBMonsterInterestAreaEnterEventBody csEvt)
	{
		StartCoroutine(AsyncCreateMonster(csEvt.monsterInst));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterInterestAreaExit(SEBMonsterInterestAreaExitEventBody csEvt)
	{
		RemoveMonster(csEvt.instanceId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterOwnershipChange(SEBMonsterOwnershipChangeEventBody csEvt)
	{
		if (m_dicMissingOwnership.ContainsKey(csEvt.instanceId)) // 이전 누락 Ownership 처리 정리.
		{
			m_dicMissingOwnership.Remove(csEvt.instanceId);
		}

		if (m_dicMonsters.ContainsKey(csEvt.instanceId)) // 정상적으로 몬스터 생성 후 Ownership 변경이 되는 경우.
		{
			m_dicMonsters[csEvt.instanceId].NetEventChangeOwnership(csEvt.ownerId, (CsMonster.EnOwnership)csEvt.ownerType);
		}
		else
		{
			//Debug.Log("OnEventEvtMonsterOwnershipChange   Monster 생성 전    m_listMissingOwnership 저장 몬스터 생성시 입력.  instanceId = " + csEvt.instanceId);
			m_dicMissingOwnership.Add(csEvt.instanceId, csEvt);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterMove(SEBMonsterMoveEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.instanceId))
		{
			m_dicMonsters[csEvt.instanceId].NetEventMonsterMove(CsRplzSession.Translate(csEvt.position), csEvt.rotationY);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterSpawn(SEBMonsterSpawnEventBody csEvt)
	{
		StartCoroutine(AsyncCreateMonster(csEvt.monster));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterSkillCast(SEBMonsterSkillCastEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId))
		{
			m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterSkillCast(CsRplzSession.Translate(csEvt.skillTargetPosition), csEvt.skillId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterReturnModeChanged(SEBMonsterReturnModeChangedEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.instanceId))
		{
			m_dicMonsters[csEvt.instanceId].NetEventMonsterReturnToSpawnedPosition(csEvt.isReturnMode);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterAbnormalStateEffectStart(SEBMonsterAbnormalStateEffectStartEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId))
		{
			m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterAbnormalStateEffectStart(csEvt.abnormalStateEffectInstanceId,
																						   csEvt.abnormalStateId,
																						   csEvt.sourceJobId,
																						   csEvt.level,
																						   csEvt.damageAbsorbShieldRemainingAbsorbAmount,
																						   csEvt.remainingTime,
																						   csEvt.removedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterAbnormalStateEffectHit(SEBMonsterAbnormalStateEffectHitEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId) == true)
		{
			m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterAbnormalStateEffectHit(csEvt.hp,
																						 csEvt.removedAbnormalStateEffects,
																						 csEvt.abnormalStateEffectInstanceId,
																						 csEvt.damage,
																						 csEvt.hpDamage,
																						 GetHeroId(csEvt.attacker));
		}
		else
		{
			if (csEvt.hp == 0)
			{
				StartCoroutine(MissingMonsterDead(csEvt.monsterInstanceId));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterAbnormalStateEffectFinished(SEBMonsterAbnormalStateEffectFinishedEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId))
		{
			m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterAbnormalStateEffectFinished(csEvt.abnormalStateEffectInstanceId);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventEvtMonsterHit(SEBMonsterHitEventBody csEvt) // 몬스터가 피격당했을때.
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId))
		{
			Guid guidHeroAttackerId = GetHeroId(csEvt.hitResult.attacker);
			Transform trTarget = m_dicMonsters[csEvt.monsterInstanceId].transform;

			if (csEvt.hitResult.hp == 0)
			{
				m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterDead();
			}

			if (m_dicHeros.ContainsKey(guidHeroAttackerId)) // other >> monster
			{
				if (m_dicHeros[guidHeroAttackerId].IsTransformationStateTame() || m_dicHeros[guidHeroAttackerId].IsTransformationStateMonster())
				{
					AttackByHit(m_dicMonsters[csEvt.monsterInstanceId].transform, csEvt.hitResult, Time.time, null, false, false);
				}
				else
				{
					m_dicHeros[guidHeroAttackerId].NetEventHitApprove(csEvt.hitResult, trTarget);
				}
			}
			else if (m_csPlayer.HeroId == guidHeroAttackerId) // my >> monter
			{
				if (m_csPlayer.IsTransformationStateTame())
				{
					AttackByHit(m_dicMonsters[csEvt.monsterInstanceId].transform, csEvt.hitResult, Time.time, null, true, true);
				}
				else if (m_csPlayer.IsTransformationStateMonster())
				{
					AttackByHit(m_dicMonsters[csEvt.monsterInstanceId].transform, csEvt.hitResult, Time.time, null, true, false);
				}
				else
				{
					m_csPlayer.NetEventHitApprove(csEvt.hitResult, trTarget);
				}
			}
		}
		else
		{
			if (csEvt.hitResult.hp == 0)
			{
				StartCoroutine(MissingMonsterDead(csEvt.monsterInstanceId));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator MissingMonsterDead(long lMonsterInstanceId)
	{
		Debug.Log("MissingMonsterDead   lMonsterInstanceId = " + lMonsterInstanceId);
		yield return new WaitUntil(() => m_dicMonsters.ContainsKey(lMonsterInstanceId));
		m_dicMonsters[lMonsterInstanceId].NetEventMonsterDead();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtMonsterMentalHit(SEBMonsterMentalHitEventBody csEvt)
	{
		if (m_dicMonsters.ContainsKey(csEvt.monsterInstanceId))
		{
			Debug.Log("OnEventEvtMonsterMentalHit    monsterInstanceId = " + csEvt.monsterInstanceId);
			//Transform trTarget = m_dicMonsters[csEvt.monsterInstanceId].transform;
			m_dicMonsters[csEvt.monsterInstanceId].NetEventMonsterMentalHit(csEvt.tamerId, csEvt.mentalStrength, csEvt.mentalStrengthDamage, csEvt.removedAbnormalStateEffects);
		}
	}

	#endregion Monster

	#region Guild

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildCreate() // 당사자 길드생성
	{
		Debug.Log("OnEventGuildCreate() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroGuildInfoUpdated(Guid guidHeroId, Guid guidGuildId, string strGuildName, int nGuildMemberGrade) // 길드 정보 갱신.
	{
		Debug.Log("1. OnEventHeroGuildInfoUpdated");
		if (m_csPlayer.HeroId == guidHeroId)
		{
			Debug.Log("2. OnEventHeroGuildInfoUpdated                당사자 길드 정보 갱신           ");
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetHeroGuildInfoUpdated(guidGuildId, strGuildName, nGuildMemberGrade);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildExit(int nContinentId) // 당사자 탈퇴.
	{
		Debug.Log("OnEventGuildExit() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildBanished(int nContinentId) // 당사자 길드추방당함.
	{
		Debug.Log("OnEventGuildBanished() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildInvitationAccept() // 당사자 길드 초대 가입 신청 수락됨.
	{
		Debug.Log("OnEventGuildInvitationAccept() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildApplicationAccepted() // 당사자 길드 신청 가입 신청 수락 
	{
		Debug.Log("OnEventGuildApplicationAccepted() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildAppointed(Guid guidAppointerId, string strAppointerName, int nAppointerGrade, Guid guidAppointeeId, string strAppointeeName, int nAppointeeGrade)
	{
		Debug.Log("1. OnEventGuildAppointed() ");
		if (m_csPlayer.HeroId == guidAppointeeId)
		{
			Debug.Log("2. OnEventGuildAppointed() ");
			m_csPlayer.NetMyGuildHUDUpdate();
		}
		else
		{
			Debug.Log("3. OnEventGuildAppointed() ");
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildMasterTransferred(Guid guidTransfererId, string strTransfererName, Guid guidtransfereeId, string strTransfereeName)
	{
		Debug.Log("1. OnEventGuildMasterTransferred() ");
		if (m_csPlayer.HeroId == guidtransfereeId)
		{
			Debug.Log("2. OnEventGuildMasterTransferred() ");
			m_csPlayer.NetMyGuildHUDUpdate();
		}
		else
		{
			Debug.Log("3. OnEventGuildMasterTransferred() ");
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildMasterTransfer()
	{
		Debug.Log(" OnEventGuildMasterTransfer() ");
		m_csPlayer.NetMyGuildHUDUpdate();
	}

	#endregion Guild

	#region Title
	//----------------------------------------------------------------------------------------------------
	void OnEventDisplayTitleSet()
	{
		Debug.Log("1. OnEventDisplayTitleSet");
		m_csPlayer.NetEventDisplayTitleChange();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventTitleLifetimeEnded(int nTitleId)
	{
		Debug.Log("1. OnEventTitleLifetimeEnded");
		m_csPlayer.NetEventDisplayTitleChange();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroDisplayTitleChanged(Guid guidHeroId, int nTitleId)
	{
		Debug.Log("1. OnEventHeroDisplayTitleChanged");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventHeroDisplayTitleChange(nTitleId);
		}
	}

	#endregion Title

	#region Creature
	//----------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipate()
	{
		if (m_csPlayer != null)
		{
			CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsCreatureManager.Instance.ParticipatedCreatureId);
			if (csHeroCreature != null)
			{
				m_csPlayer.NetEventCreatureParticipated(csHeroCreature.Creature);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipationCancel()
	{
		if (m_csPlayer != null)
		{
			m_csPlayer.NetEventCreatureParticipationCanceled();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroCreatureParticipated(Guid guidHeroId, int nCreatureId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventCreatureParticipated(CsGameData.Instance.GetCreature(nCreatureId));
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroCreatureParticipationCanceled(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventCreatureParticipationCanceled();
		}
	}

	#endregion Creature

	#region FishingQuest

	//---------------------------------------------------------------------------------------------------
	void OnEventFishingZone(bool bEnter, int nSpotId)
	{
		if (bEnter)
		{
			m_csPlayer.NetEventFishingZoneEnter();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroFishingStart(Transform trFishingArea)
	{
		m_csPlayer.NetEventFishingStart(trFishingArea);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFishingCanceled()
	{
		m_csPlayer.NetEventFishingStop();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFishingCastingCompleted(bool bLevelup, long lExp, bool bBaitEnable)
	{
		if (bBaitEnable) return;
		m_csPlayer.NetEventFishingStop();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroFishingStarted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventFishingStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroFishingCanceled(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventFishingStop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroFishingCompleted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventFishingStop();
		}
	}

	#endregion FishingQuest

	#region NationAlliance

	//---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccept(CsNationAlliance csNationAlliance)
	{
		Debug.Log("OnEventNationAllianceApplicationAccept");
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.NetEventNationAllianceConcluded();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventNationAllianceBreak()
	{
		Debug.Log("OnEventNationAllianceBreak");
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.NetEventNationAllianceBroken();
		}
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceConcluded(CsNationAlliance csNationAlliance)
	{
		Debug.Log("OnEventNationAllianceConcluded");
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.NetEventNationAllianceConcluded();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventNationAllianceBroken()
	{
		Debug.Log("OnEventNationAllianceBroken");
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.NetEventNationAllianceBroken();
		}
	}

	#endregion NationAlliance

	#region HeroJobChanged

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
	{		
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventHeroJobChanged(nJobId);
		}
	}

	#endregion HeroJobChanged

	#region CinemachineScene

	//----------------------------------------------------------------------------------------------------
	void CinemachineSceneFirst() // 대륙 최초 입장 연출 시작.
	{
		//Debug.Log("################                        CinemachineSceneFirst()     CsIngameData.Instance.Directing = " + CsIngameData.Instance.Directing);
		//CsIngameData.Instance.Directing = true;

		//if (m_csPlayer != null)
		//{
		//	m_csPlayer.MyHeroView(false);
		//}

		//CsGameEventToUI.Instance.OnEventFade(false);
		//CsGameEventToUI.Instance.OnEventHideMainUI(true);
		//GameObject goFirtst = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Cinema/CineScene_FirstEnter"), transform) as GameObject;
		//goFirtst.GetComponent<CsCinemachineSceneFirstEnter>().Init();
	}

	#endregion CinemachineScene

	#region EquipSetting

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator StartGearReourcesAsync()  // 촤초 한번만 실행.
	{
		if (GameObject.Find("GearPool") == null)
		{
			GameObject goGearPool = new GameObject();
			goGearPool.name = "GearPool";
			//goGearPool.transform.SetParent(transform);
			DontDestroyOnLoad(goGearPool);

			for (int i = 1; i < 5; i++)
			{
				string strJob = ((EnJob)i).ToString();
				if (CsIngameData.Instance.OtherPlayer.ContainsKey(strJob) == false)
				{
					ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Player/" + strJob);
					yield return req;

					GameObject goHero = Instantiate((GameObject)req.asset, Vector3.zero, Quaternion.identity, goGearPool.transform) as GameObject;
					goHero.transform.name = strJob;
					goHero.SetActive(false);
					CsIngameData.Instance.OtherPlayer.Add(strJob, goHero);
				}
			}

			for (int i = 0; i < CsGameData.Instance.MainGearList.Count; i++)
			{
				CsMainGear csMainGear = CsGameData.Instance.MainGearList[i];

				if (csMainGear.MainGearType.MainGearCategory == null) continue;

				if (csMainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Weapon)
				{
					if (csMainGear.PrefabName == null) continue;

					if (CsIngameData.Instance.Weapon.ContainsKey(csMainGear.PrefabName) == false)
					{
						ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroWeapon/" + csMainGear.PrefabName);
						yield return req;
						if (req.asset == null)
						{
							Debug.Log("########        StartLoadSetEquip()         ##########   req.asset == nul     HeroWeapon = " + csMainGear.PrefabName);
						}
						else
						{
							GameObject goWeapon = GameObject.Instantiate((GameObject)req.asset, Vector3.zero, Quaternion.identity, goGearPool.transform) as GameObject;
							goWeapon.transform.name = goWeapon.transform.name.Replace("(Clone)", "");
							goWeapon.SetActive(false);
							CsIngameData.Instance.Weapon.Add(csMainGear.PrefabName, goWeapon);
						}
					}
				}
				else if (csMainGear.MainGearType.MainGearCategory.EnMainGearCategory == EnMainGearCategory.Armor)
				{
					if (csMainGear.PrefabName == null) continue;

					for (int j = 1; j < 5; j++)
					{
						string strKey = j.ToString() + csMainGear.PrefabName;

						if (CsIngameData.Instance.Armor.ContainsKey(strKey) == false)
						{
							ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroArmor/" + j.ToString() + "/" + csMainGear.PrefabName);
							yield return req;

							if (req.asset == null)
							{
								Debug.Log("########        StartLoadSetEquip()         ##########   req.asset == nul     HeroArmor = " + csMainGear.PrefabName);
							}
							else
							{
								GameObject goArmor = GameObject.Instantiate((GameObject)req.asset, Vector3.zero, Quaternion.identity, goGearPool.transform) as GameObject;
								goArmor.transform.name = strKey;
								goArmor.SetActive(false);
								CsIngameData.Instance.Armor.Add(strKey, goArmor);
							}
						}
					}


				}
			}

			for (int i = 1; i < 4; i++)  // 1 ~ 3
			{
				string strPrefabName = i.ToString();
				for (int j = 1; j < 5; j++)
				{
					string strKey = j.ToString() + strPrefabName;

					if (!CsIngameData.Instance.Face.ContainsKey(strKey))
					{
						ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroFace/" + j.ToString() + "/" + strPrefabName);
						yield return req;

						if (req.asset == null)
						{
							Debug.Log("########        StartLoadSetEquip()         ##########    req.asset == nul     HeroFace = " + strPrefabName);
						}
						else
						{
							GameObject goFace = GameObject.Instantiate((GameObject)req.asset, Vector3.zero, Quaternion.identity, goGearPool.transform) as GameObject;
							goFace.transform.name = strKey;
							goFace.SetActive(false);
							CsIngameData.Instance.Face.Add(strKey, goFace);
						}
					}

					if (!CsIngameData.Instance.Hair.ContainsKey(strKey))
					{
						ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/HeroHair/" + j.ToString() + "/" + strPrefabName);
						yield return req;

						if (req.asset == null)
						{
							Debug.Log("########        StartLoadSetEquip()         ##########    req.asset == nul       HeroHair = " + strPrefabName);
						}
						else
						{
							GameObject goHair = GameObject.Instantiate((GameObject)req.asset, Vector3.zero, Quaternion.identity, goGearPool.transform) as GameObject;
							goHair.transform.name = strKey;
							goHair.SetActive(false);
							CsIngameData.Instance.Hair.Add(strKey, goHair);
						}
					}
				}
			}
		}
	}

	#endregion EquipSetting

	#region OptionSetting

	//---------------------------------------------------------------------------------------------------
	void SetOptionSetting()
	{
		CsIngameData.Instance.Graphic = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Graphic);                                               // 그래픽  0.하, 1.중, 2.상
		CsIngameData.Instance.Effect = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Effect);                                                 // 이팩트  0.안보기, 1.내것만, 2.모두

		CsIngameData.Instance.BGM = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.BGM) == 1 ? true : false;                                   // 배경음   0.안듣기 , 1.듣기
		CsIngameData.Instance.EffectSound = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.EffectSound) == 1 ? true : false;                   // 효과음   0.안듣기 , 1.듣기

		CsIngameData.Instance.UserViewFilter = (EnUserViewFilter)CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.UserViewFilter);               // 0.아군만보기, 1.적군만보기, 2.모두보기

		switch (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.UserViewLimit))                                                                 // 0.최하, 1.하, 2.중, 3.상
		{
		case 0:
			m_nUserViewLimit = 0;
			break;
		case 1:
			m_nUserViewLimit = 5;
			break;
		case 2:
			m_nUserViewLimit = 10;
			break;
		case 3:
			m_nUserViewLimit = 15;
			break;
		}

		CsIngameData.Instance.CombatRange = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.CombatRange);
		CsIngameData.Instance.AutoSkill2 = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoSkill2) == 1 ? true : false;                     // 2번스킬자동  0.비사용, 1.사용
		CsIngameData.Instance.AutoSkill3 = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoSkill3) == 1 ? true : false;                     // 3번스킬자동  0.비사용, 1.사용
		CsIngameData.Instance.AutoSkill4 = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoSkill4) == 1 ? true : false;                     // 4번스킬자동  0.비사용, 1.사용
		CsIngameData.Instance.AutoSkill5 = CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.AutoSkill5) == 1 ? true : false;                     // 5번라크스킬자동  0.비사용, 1.사용
		Debug.Log("#####     SetOptionSetting()           ");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPlayerPrefsKeySet(EnPlayerPrefsKey enPlayerPrefsKey, int nValue)
	{
		switch (enPlayerPrefsKey)
		{
			case EnPlayerPrefsKey.Frame:
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
				GuiStyleSetting();  // 임시 프레임 표시 및 프레임 세팅.

				break;
			case EnPlayerPrefsKey.Graphic:
				CsIngameData.Instance.Graphic = nValue;
				switch (CsIngameData.Instance.Graphic)
				{
					case 0:
						QualitySettings.SetQualityLevel(0);
						StartCoroutine(SetResolution(960, 540, true));
						break;
					case 1:
						QualitySettings.SetQualityLevel(2);
						StartCoroutine(SetResolution(1280, 720, true));
						break;
					case 2:
						QualitySettings.SetQualityLevel(4);
						StartCoroutine(SetResolution(1280, 720, true));
						break;
				}
				break;
			case EnPlayerPrefsKey.BGM:
				CsIngameData.Instance.BGM = nValue == 1 ? true : false;
				ChangeBGM();
				break;
			case EnPlayerPrefsKey.Effect:
				CsIngameData.Instance.Effect = nValue;
				break;
			case EnPlayerPrefsKey.EffectSound:
				CsIngameData.Instance.EffectSound = nValue == 1 ? true : false;
				break;
			case EnPlayerPrefsKey.UserViewFilter:
				CsIngameData.Instance.UserViewFilter = (EnUserViewFilter)nValue;
				foreach (KeyValuePair<Guid, CsOtherPlayer> dicHero in m_dicHeros)
				{
					dicHero.Value.OnEventHideOptionChange();
				}
				break;
			case EnPlayerPrefsKey.UserViewLimit:
				switch (nValue)
				{
				case 0:
					m_nUserViewLimit = 0;
					break;
				case 1:
					m_nUserViewLimit = 5;
					break;
				case 2:
					m_nUserViewLimit = 10;
					break;
				case 3:
					m_nUserViewLimit = 15;
					break;
				}				
				break;
			case EnPlayerPrefsKey.CombatRange:
				CsIngameData.Instance.CombatRange = nValue;
				break;
			case EnPlayerPrefsKey.AutoSkill2:
				CsIngameData.Instance.AutoSkill2 = nValue == 1 ? true : false;
				break;
			case EnPlayerPrefsKey.AutoSkill3:
				CsIngameData.Instance.AutoSkill3 = nValue == 1 ? true : false;
				break;
			case EnPlayerPrefsKey.AutoSkill4:
				CsIngameData.Instance.AutoSkill4 = nValue == 1 ? true : false;
				break;
			case EnPlayerPrefsKey.AutoSkill5:
				CsIngameData.Instance.AutoSkill5 = nValue == 1 ? true : false;
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public IEnumerator SetResolution(int width, int height, bool fullscreen)
	{
		CsGameEventToUI.Instance.OnEventFade(true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		Camera[] aCamera = Camera.allCameras;
		List<Camera> listCamera = new List<Camera>();
		foreach (var camera in aCamera)
		{
			if (camera.enabled)
			{
				listCamera.Add(camera);
				camera.enabled = false;
			}
		}
		Screen.SetResolution(width, height, fullscreen);
		yield return new WaitForEndOfFrame();

		GC.Collect();
		Resources.UnloadUnusedAssets();
		yield return new WaitForEndOfFrame();

		CsGameEventToUI.Instance.OnEventFade(false);
		foreach (var camera in listCamera)
		{
			camera.enabled = true;
		}
		yield return new WaitForEndOfFrame();

		GuiStyleSetting();                  // 임시 프레임 표시 및 프레임 세팅.
		CsGameEventToUI.Instance.OnEventChangeResolution();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventLoadingUIComplete()
	{
		if (CsGameData.Instance.MyHeroInfo.IsCreateEnter)
		{
			Debug.Log("#####################                             OnEventLoadingUIComplete     >>>     Start Timeline");
			CinemachineSceneFirst();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOtherHeroView(bool bView)
	{
		Debug.Log("###########    OnEventOtherHeroView     #############  bView = "+ bView);
		CsIngameData.Instance.OtherHeroView = bView;
	}

	//----------------------------------------------------------------------------------------------------
	void ChangeBGM() // m_nAreaNo = 1 == Default 
	{
		Debug.Log("ChangeBGM()    CsIngameData.Instance.BGM = " + CsIngameData.Instance.BGM);
		if (CsIngameData.Instance.BGM) // BGM설정이 켜져 있을때.
		{
			if (m_audioSource.clip != null)
			{
				m_audioSource.Play();
			}
		}
		else // BGM 설정 꺼져있는 경우.
		{
			if (m_audioSource.clip != null)
			{
				m_audioSource.Stop();
			}
		}
	}

	#endregion OptionSetting

	#region ObjectManagement
	//----------------------------------------------------------------------------------------------------
	protected void SetEnter(PDContinentEntranceInfo pDContinentEntranceInfo, bool bFirst = false)
	{
		Debug.Log("SetEnter()    " + CsGameData.Instance.MyHeroInfo.Name + " , " + pDContinentEntranceInfo.nationId + " , " + pDContinentEntranceInfo.continentId);

		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = pDContinentEntranceInfo.nationId;
		SetMyHeroLocation(pDContinentEntranceInfo.continentId);

		SetMyHero(pDContinentEntranceInfo.position, pDContinentEntranceInfo.rotationY, pDContinentEntranceInfo.placeInstanceId, bFirst); // MyHero Create

		CreatObject(pDContinentEntranceInfo.position,
					pDContinentEntranceInfo.heroes,
					pDContinentEntranceInfo.monsters,
					pDContinentEntranceInfo.objectInsts,
					pDContinentEntranceInfo.cartInsts);
	}

	//----------------------------------------------------------------------------------------------------
	protected void SetEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId, PDHero[] pDHero, PDMonsterInstance[] pDMonsterInstance,
							PDContinentObjectInstance[] pDContinentObjectInstance, PDCartInstance[] pDCartInstance, bool bFirst)
	{
		Debug.Log("SetEnter()     " + CsGameData.Instance.MyHeroInfo.Name + " , " + pDVector3 + " , " + pDCartInstance.Length + " , " + bFirst);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, bFirst); // MyHero Create
		CreatObject(pDVector3, pDHero, pDMonsterInstance, pDContinentObjectInstance, pDCartInstance);
	}

	//----------------------------------------------------------------------------------------------------
	void CreatObject(PDVector3 pDVector3, PDHero[] pDHero, PDMonsterInstance[] pDMonsterInstance, PDContinentObjectInstance[] pDContinentObjectInstance, PDCartInstance[] pDCartInstance)
	{
		for (int i = 0; i < pDHero.Length; i++) // OtherHero Create
		{
			StartCoroutine(AsyncCreateHero(pDHero[i], false, false));
		}

		for (int i = 0; i < pDMonsterInstance.Length; i++) // Monster Create
		{
			StartCoroutine(AsyncCreateMonster(pDMonsterInstance[i]));
		}

		for (int i = 0; i < pDContinentObjectInstance.Length; i++) // Monster Create
		{
			CreateInteractionObject(pDContinentObjectInstance[i].arrangeNo, pDContinentObjectInstance[i].instanceId, pDContinentObjectInstance[i].interactionHeroId);
		}

		if (pDCartInstance != null)
		{
			for (int i = 0; i < pDCartInstance.Length; i++)
			{
				StartCoroutine(CreateCart(pDCartInstance[i]));
			}
		}

		for (int i = 0; i < CsGameData.Instance.NpcInfoList.Count; i++) // NPC Create
		{
			StartCoroutine(AsyncCreateContinentNpc(CsGameData.Instance.NpcInfoList[i]));
		}

		if (CsNationWarManager.Instance.AppearNpc == true)
		{
			for (int i = 0; i < CsGameData.Instance.NationWar.NationWarNpcList.Count; i++)
			{
				StartCoroutine(AsyncCreateNationWarNPC(CsGameData.Instance.NationWar.NationWarNpcList[i]));
			}
		}
	}

	#region HeroManagement

	//----------------------------------------------------------------------------------------------------
	protected void SetMyHero(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId, bool bFirst)
	{
		Resources.UnloadUnusedAssets();
		System.GC.Collect(); // 가비지 초기화.

		Transform trPlayer = transform.Find("Player");

		for (int i = 0; i < trPlayer.childCount; i++)
		{
			Destroy(trPlayer.GetChild(i).gameObject);
		}

		// 전직으로 인한 변경
		GameObject goPlayer;

		int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;
		goPlayer = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Player/My/" + (EnJob)nJobId), CsRplzSession.Translate(pDVector3), Quaternion.Euler(new Vector3(0f, flRotationY, 0)), trPlayer) as GameObject;
		
		CsGameData.Instance.MyHeroTransform = goPlayer.transform;
		m_csPlayer = CsGameData.Instance.MyHeroTransform.GetComponent<CsMyPlayer>();

		if (CsGameData.Instance.MyHeroInfo.IsCreateEnter) // 캐릭터 생성후 최초 입장.
		{
			bFirst = false;
		}
		else
		{
			if (bFirst == false)
			{
				CsIngameData.Instance.InGameCamera.RightAndLeft = flRotationY * Mathf.Deg2Rad;
			}
		}

		if (CsGameData.Instance.MyHeroInfo.CartInstance != null)
		{
			CsGameData.Instance.MyHeroInfo.CartInstance.position = pDVector3;
			CsGameData.Instance.MyHeroInfo.CartInstance.rotationY = flRotationY;
			StartCoroutine(CreateCart(CsGameData.Instance.MyHeroInfo.CartInstance));
		}

		m_csPlayer.Init(CsRplzSession.Translate(pDVector3), flRotationY, guidPlaceInstanceId, bFirst);
		StartCoroutine(SceneLoadComplete(m_bChaegeScene)); // UI에 게임 진입 완료를 전달.
		m_bChaegeScene = true;
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearMyHero()
	{
		if (m_csPlayer != null)
		{
			GameObject.Destroy(m_csPlayer.gameObject);
		}
		m_csPlayer = null;
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator AsyncCreateHero(PDHero pDHero, bool bFirst, bool bRevivalEnter, float flRevivalInvincibilityRemainingTime = 0, bool bTrap = false)
	{
		Transform trOtherPlayer = transform.Find("OtherPlayer");

		yield return new WaitUntil(() => CsIngameData.Instance.Directing == false); // 연출이 끝나면 생성.

		CsJob csJob = CsGameData.Instance.GetJob(pDHero.jobId);
		int nJob = csJob.ParentJobId == 0 ? csJob.JobId : csJob.ParentJobId;

		if (m_dicHeros.ContainsKey(pDHero.id) == false)
		{
			Debug.Log("  CreateHero  ###  create");

			ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Player/Other/" + (EnJob)nJob);
			yield return req;

			GameObject goHero = Instantiate((GameObject)req.asset, trOtherPlayer) as GameObject;
			CsOtherPlayer csOtherPlayer = goHero.GetComponent<CsOtherPlayer>();
			if (csOtherPlayer != null)
			{
				csOtherPlayer.Init(pDHero, bFirst, bRevivalEnter, flRevivalInvincibilityRemainingTime);
				m_dicHeros.Add(pDHero.id, csOtherPlayer);
				yield return new WaitForSeconds(1f);
				m_listOtherPlayer.Add(csOtherPlayer);
			}
		}
		else if (m_dicHeros.ContainsKey(pDHero.id) && m_dicHeros[pDHero.id].CartObject != null && m_dicHeros[pDHero.id].CartObject.CartQuestType == EnCartQuestType.GuildSupplySupport)
		{
			Debug.Log("  CreateHero  ###  create have Cart");
			ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Player/Other/" + (EnJob)nJob);
			yield return req;

			GameObject goHero = Instantiate((GameObject)req.asset, trOtherPlayer) as GameObject;
			CsOtherPlayer csOtherPlayer = goHero.GetComponent<CsOtherPlayer>();
			m_listOtherPlayer.Add(csOtherPlayer);

			if (csOtherPlayer != null)
			{
				csOtherPlayer.Init(pDHero, bFirst, bRevivalEnter, flRevivalInvincibilityRemainingTime);
				csOtherPlayer.NetEventCartGetOn(m_dicHeros[pDHero.id].CartObject);
			}
		}

		if (bTrap)
		{
			if (m_dicHeros.ContainsKey(pDHero.id))
			{
				m_dicHeros[pDHero.id].NetStartTrap();
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveHero(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId) == true)
		{
			m_listOtherPlayer.Remove(m_dicHeros[guidHeroId]);
			GameObject.Destroy(m_dicHeros[guidHeroId].gameObject);
			m_dicHeros.Remove(guidHeroId);

			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearHero()
	{
		foreach (var dicHeros in m_dicHeros)
		{
			GameObject.Destroy(dicHeros.Value.gameObject);
		}
		m_dicHeros.Clear();
	}

	#endregion HeroManagement

	#region MonsterManagement

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator StartMonsterReourcesAsync()
	{
		CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
		if (csContinent != null)    // 대륙인 경우.
		{
			for (int i = 0; i < csContinent.ContinentMapMonsterList.Count; i++)
			{
				CsMonsterInfo csMonsterInfo = csContinent.ContinentMapMonsterList[i].MonsterInfo;
				string strName = csMonsterInfo.MonsterCharacter.PrefabName;

				if (csMonsterInfo == null || csMonsterInfo.MonsterCharacter == null) continue;

				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>(string.Format("Prefab/MonsterObject/{0:D2}", strName));
				yield return req;

				if (req.asset == null)
				{
					Debug.Log("########        StartMonsterReourcesAsync()         ##########    req.asset == nul       PrefabName = " + strName);
				}
				else
				{
					if (m_dicMonsterResources.ContainsKey(strName) == false)
					{
						Transform trParent = transform.Find("Monster");
						GameObject goMonster = Instantiate((GameObject)req.asset, trParent);
						goMonster.name = strName;
						goMonster.SetActive(false);
						m_dicMonsterResources.Add(strName, goMonster);
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator AsyncCreateMonster(PDMonsterInstance pdMon, bool bBoss = false, bool bLak = false)
	{
		if (pdMon != null)
		{
			if (m_dicMonsters.ContainsKey(pdMon.instanceId) == false)
			{
				yield return new WaitUntil(() => CsIngameData.Instance.Directing == false);	// 연출이 끝나면 생성.

				CsMonsterInfo csMonsterInfo = CsGameData.Instance.MonsterInfoList.Find(a => a.MonsterId == pdMon.monsterId);

				if (csMonsterInfo != null)
				{
					GameObject goMon = null;
					Transform trParent = transform.Find("Monster");
					string strPrefabName = csMonsterInfo.MonsterCharacter.PrefabName;

					if (m_dicMonsterResources.ContainsKey(strPrefabName))
					{
						goMon = Instantiate(m_dicMonsterResources[strPrefabName], CsRplzSession.Translate(pdMon.position), Quaternion.identity, trParent) as GameObject;
					}
					else
					{
						ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>(string.Format("Prefab/MonsterObject/{0:D2}", strPrefabName));
						yield return req;

						if (req.asset == null)
						{
							Debug.Log("########        StartMonsterReourcesAsync()         ##########    req.asset == nul       PrefabName = " + strPrefabName);
						}
						else
						{
							if (m_dicMonsterResources.ContainsKey(strPrefabName) == false)
							{
								GameObject goMonster = Instantiate((GameObject)req.asset, trParent);
								goMonster.name = strPrefabName;
								goMonster.SetActive(false);
								m_dicMonsterResources.Add(strPrefabName, (GameObject)req.asset);
							}

							goMon = Instantiate(m_dicMonsterResources[strPrefabName], CsRplzSession.Translate(pdMon.position), Quaternion.identity, trParent) as GameObject;
						}
					}

					goMon.SetActive(true);
					goMon.AddComponent<UnityEngine.AI.NavMeshAgent>();
					goMon.AddComponent<CapsuleCollider>();
					goMon.AddComponent<AudioSource>();

					if (goMon.GetComponent<CsMonsterBoss>() == null && goMon.GetComponent<CsMonsterBossOsiris>() == null)
					{
						if (pdMon.type == MonsterInstanceType.OsirisMonster)
						{
							goMon.AddComponent<CsMonsterDungeonOsiris>();
						}
						else
						{
							goMon.AddComponent<CsMonster>();
						}
					}

					CsMonster csMonster = goMon.GetComponent<CsMonster>();

					csMonster.Init(csMonsterInfo,
								   pdMon.instanceId,
								   pdMon.type,
								   pdMon.maxHP,
								   pdMon.hp,
								   pdMon.maxMentalStrength,
								   pdMon.mentalStrength,
								   CsRplzSession.Translate(pdMon.spawnedPosition),
								   pdMon.spawnedRotationY,
								   CsRplzSession.Translate(pdMon.position),
								   pdMon.rotationY,
								   pdMon.ownerId,
								   (CsMonster.EnOwnership)pdMon.ownerType,
								   pdMon.isExclusive,
								   pdMon.exclusiveHeroId,
								   pdMon.exclusiveHeroName,
								   pdMon.nationId,
								   bBoss);

					m_dicMonsters.Add(pdMon.instanceId, csMonster);

					if (m_dicMissingOwnership.ContainsKey(pdMon.instanceId))
					{
						m_dicMonsters[pdMon.instanceId].NetEventChangeOwnership(m_dicMissingOwnership[pdMon.instanceId].ownerId, (CsMonster.EnOwnership)m_dicMissingOwnership[pdMon.instanceId].ownerType);
						m_dicMissingOwnership.Remove(pdMon.instanceId);
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected CsMonster CreateMonster(PDMonsterInstance pdMon)
	{
		if (pdMon == null) return null;

		if (m_dicMonsters.ContainsKey(pdMon.instanceId) == false)
		{
			CsMonsterInfo csMonsterInfo = CsGameData.Instance.MonsterInfoList.Find(a => a.MonsterId == pdMon.monsterId);
			GameObject goMon = null;

			if (csMonsterInfo == null) return null;
			if (m_dicMonsterResources.ContainsKey(csMonsterInfo.MonsterCharacter.PrefabName))
			{
				goMon = Instantiate(m_dicMonsterResources[csMonsterInfo.MonsterCharacter.PrefabName], CsRplzSession.Translate(pdMon.position), Quaternion.identity, transform.Find("Monster")) as GameObject;
			}
			else
			{
				GameObject goMonResources = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/MonsterObject/{0:D2}", csMonsterInfo.MonsterCharacter.PrefabName));
				if (goMonResources == null)
				{
					Debug.Log("CreateMonster         프리펩 네임 확인 필요         PrefabName = " + csMonsterInfo.MonsterCharacter.PrefabName);
					return null;
				}
				goMon = Instantiate(goMonResources, CsRplzSession.Translate(pdMon.position), Quaternion.identity, transform.Find("Monster")) as GameObject;
				m_dicMonsterResources.Add(csMonsterInfo.MonsterCharacter.PrefabName, goMonResources);
			}

			goMon.SetActive(true);
			goMon.AddComponent<UnityEngine.AI.NavMeshAgent>();
			goMon.AddComponent<CapsuleCollider>();
			goMon.AddComponent<AudioSource>();

			if (goMon.GetComponent<CsMonsterBoss>() == null && goMon.GetComponent<CsMonsterBossOsiris>() == null)
			{
				goMon.AddComponent<CsMonster>();
			}

			CsMonster csMon = goMon.GetComponent<CsMonster>();

			csMon.Init(csMonsterInfo,
					   pdMon.instanceId,
					   pdMon.type,
					   pdMon.maxHP,
					   pdMon.hp,
					   pdMon.maxMentalStrength,
					   pdMon.mentalStrength,
					   CsRplzSession.Translate(pdMon.spawnedPosition),
					   pdMon.spawnedRotationY,
					   CsRplzSession.Translate(pdMon.position),
					   pdMon.rotationY,
					   pdMon.ownerId,
					   (CsMonster.EnOwnership)pdMon.ownerType,
					   pdMon.isExclusive,
					   pdMon.exclusiveHeroId,
					   pdMon.exclusiveHeroName,
					   pdMon.nationId,
					   false);

			m_dicMonsters.Add(pdMon.instanceId, csMon);

			if (m_dicMissingOwnership.ContainsKey(pdMon.instanceId))
			{
				Debug.Log("AddMonster  누락 OwnershipChange 처리");
				m_dicMonsters[pdMon.instanceId].NetEventChangeOwnership(m_dicMissingOwnership[pdMon.instanceId].ownerId, (CsMonster.EnOwnership)m_dicMissingOwnership[pdMon.instanceId].ownerType);
				m_dicMissingOwnership.Remove(pdMon.instanceId);
			}
			return csMon;
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveMonster(long lMonInstanceId)
	{
		if (m_dicMonsters.ContainsKey(lMonInstanceId))
		{
			if (m_dicMonsters[lMonInstanceId].gameObject)
			{
				DestroyImmediate(m_dicMonsters[lMonInstanceId].gameObject);
			}
			m_dicMonsters.Remove(lMonInstanceId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearMonster()
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			Destroy(dicMonsters.Value.gameObject);
		}
		m_dicMonsters.Clear();
	}

	#endregion MonsterManagement

	#region NpcManagement

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator AsyncCreateContinentNpc(CsNpcInfo csNpcInfo)
	{
		if (csNpcInfo != null)
		{
			if (csNpcInfo.ContinentId == CsGameData.Instance.MyHeroInfo.LocationId)
			{
				if (m_dicNpcs.ContainsKey(csNpcInfo.NpcId) == false)
				{
					ResourceRequest resourceRequest = CsIngameData.Instance.LoadAssetAsync<GameObject>(string.Format("Prefab/NpcObject/{0:D2}", csNpcInfo.PrefabName));
					yield return resourceRequest;
					if (resourceRequest.asset == null)
					{
						Debug.Log("######################          AddNpcAssetAsync     resourceRequest.asset == Null    csNpcInfo.PrefabName = " + csNpcInfo.PrefabName);
					}
					else
					{
						GameObject goNpc = Instantiate(resourceRequest.asset, csNpcInfo.Position, Quaternion.identity, transform.Find("Npc")) as GameObject;
						goNpc.AddComponent<UnityEngine.AI.NavMeshAgent>();
						goNpc.AddComponent<CapsuleCollider>();
						goNpc.AddComponent<CsNpc>();

						CsNpc csNpc = goNpc.GetComponent<CsNpc>();
						csNpc.InitNpc(csNpcInfo);

						m_dicNpcs.Add(csNpcInfo.NpcId, csNpc);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator AsyncCreateNationWarNPC(CsNationWarNpc csNationWarNpc)
	{
		if (csNationWarNpc != null)
		{
			if (m_dicNpcs.ContainsKey(csNationWarNpc.NpcId) == false)
			{
				if (csNationWarNpc.Continent.ContinentId == CsGameData.Instance.MyHeroInfo.LocationId)
				{
					Debug.Log("############## CreateNationWarNPC() ####################      csNationWarNpc.NpcId = " + csNationWarNpc.NpcId);
					ResourceRequest resourceRequest = CsIngameData.Instance.LoadAssetAsync<GameObject>(string.Format("Prefab/NpcObject/{0:D2}", csNationWarNpc.PrefabName));
					yield return resourceRequest;

					if (resourceRequest.asset == null)
					{
						Debug.Log("######################          CreateNationWarNPC     resourceRequest.asset == Null    csNationWarNpc.PrefabName = " + csNationWarNpc.PrefabName);
					}
					else
					{
						yield return new WaitForSeconds(2f);
						GameObject goNpc = Instantiate(resourceRequest.asset, csNationWarNpc.Position, Quaternion.identity, transform.Find("Npc")) as GameObject;
						goNpc.AddComponent<UnityEngine.AI.NavMeshAgent>();
						goNpc.AddComponent<CapsuleCollider>();
						goNpc.AddComponent<CsNpc>();

						CsNpc csNpc = goNpc.GetComponent<CsNpc>();
						csNpc.InitNationWarNpc(csNationWarNpc);
						m_dicNpcs.Add(csNationWarNpc.NpcId, csNpc);
					}
				}
			}

		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveNpc(int nNpcId)
	{
		if (m_dicNpcs.ContainsKey(nNpcId) == true)
		{
			GameObject.Destroy(m_dicNpcs[nNpcId].gameObject);
			m_dicNpcs.Remove(nNpcId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearNpc()
	{
		foreach (var dicNpcs in m_dicNpcs)
		{
			GameObject.Destroy(dicNpcs.Value.gameObject);
		}
		m_dicNpcs.Clear();
	}

	#endregion NpcManagement

	#region CartManagement

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator CreateCart(PDCartInstance pDCartInstance)
	{
		if (pDCartInstance != null)
		{
			CsCartObject csCartObject = null;

			if (m_dicCart.ContainsKey(pDCartInstance.instanceId) == false) // 최초 생성.
			{
				CsCart csCart = CsGameData.Instance.GetCart(pDCartInstance.cartId);
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>(string.Format("Prefab/CartObject/{0:D2}", csCart.PrefabName));
				yield return req;
				if (req.asset != null)
				{
					GameObject goCart = Instantiate(req.asset, transform.Find("Player")) as GameObject;
					csCartObject = goCart.GetComponent<CsCartObject>();
					csCartObject.Init(pDCartInstance);
					m_dicCart.Add(pDCartInstance.instanceId, csCartObject);
				}

				Debug.Log("1. CreateCart     instanceId = " + pDCartInstance.instanceId + "  //  PrefabName = " + csCart.PrefabName + " csCartObject = " + csCartObject);
			}
			else
			{
				csCartObject = m_dicCart[pDCartInstance.instanceId];
			}

			if (csCartObject != null)
			{
				Debug.Log("2. CreateCart     pDCartInstance.rider = "+ pDCartInstance.rider);

				if (pDCartInstance.rider != null) // 카트탑승 운송상태.
				{
					csCartObject.ChangeRiding(true);

					if (m_csPlayer.HeroId == pDCartInstance.ownerId) // MyHero.
					{
						Debug.Log("3. CreateCart     MyHero ");
						CsGameData.Instance.MyHeroInfo.CartInstance = pDCartInstance; // 탑승시 카트정보 MyHeroInfo에 저장.
						m_csPlayer.NetEventCartGetOn(csCartObject);
					}
					else
					{
						Debug.Log("4. CreateCart     OtherHero ");
						if (m_dicHeros.ContainsKey(pDCartInstance.ownerId) == false) // OtherHero 없으면 생성.
						{
							StartCoroutine(AsyncCreateHero(pDCartInstance.rider, false, false));
						}

						yield return new WaitUntil(() => m_dicHeros.ContainsKey(pDCartInstance.ownerId));
						yield return new WaitForSeconds(1f);

						if (m_dicHeros.ContainsKey(pDCartInstance.ownerId)) // OtherHero가 있으면 카트 탑승처리.
						{
							m_dicHeros[pDCartInstance.ownerId].NetEventCartGetOn(csCartObject);
						}
					}
				}
				else // 카트만 생성.
				{
					Debug.Log("5. CreateCart     NoneRide ");
					if (m_csPlayer.HeroId == pDCartInstance.ownerId) // MyHero.
					{
						CsGameData.Instance.MyHeroInfo.CartInstance = pDCartInstance; // 탑승중이 아닌경우 카트정보 MyHeroInfo에 저장.
					}

					csCartObject.ChangeRiding(false);
				}
			}

			Debug.Log("CreateCart   csCartObject = " + csCartObject);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ChangeCart(PDCartInstance pDCartInstance)
	{
		if (pDCartInstance == null) return;

		if (m_dicCart.ContainsKey(pDCartInstance.instanceId)) // 교체할 카트 확인.
		{
			if (m_csPlayer.HeroId == pDCartInstance.ownerId) // MyHero.
			{
				Debug.Log("1. CartChange     m_csPlayer.HeroId == pDCartInstance.ownerId");
				m_csPlayer.NetEventCartGetOff();
			}
			else if (m_dicHeros.ContainsKey(pDCartInstance.ownerId)) // OtherHero.
			{
				Debug.Log("2. CreateCart     pDCartInstance.rider = " + pDCartInstance.rider.name);
				m_dicHeros[pDCartInstance.ownerId].NetEventCartGetOff();
			}

			RemoveCart(pDCartInstance.instanceId);
			GameObject goCart = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/CartObject/{0:D2}", CsGameData.Instance.GetCart(pDCartInstance.cartId).PrefabName)) as GameObject;
			goCart = Instantiate(goCart, transform.Find("Player")) as GameObject;

			CsCartObject csCartObject = goCart.GetComponent<CsCartObject>();
			csCartObject.Init(pDCartInstance);
			m_dicCart.Add(pDCartInstance.instanceId, csCartObject);

			if (m_csPlayer.HeroId == pDCartInstance.ownerId) // MyHero.
			{
				Debug.Log("3. CartChange     m_csPlayer.HeroId == pDCartInstance.ownerId");
				m_csPlayer.NetEventCartGetOn(csCartObject);
				CsGameData.Instance.MyHeroInfo.CartInstance = pDCartInstance; // 탑승시 카트정보 MyHeroInfo에 저장.
			}
			else
			{
				if (m_dicHeros.ContainsKey(pDCartInstance.ownerId)) // OtherHero가 있으면 카트 탑승처리.
				{
					Debug.Log("4. CreateCart     pDCartInstance.rider = " + pDCartInstance.rider.name);
					m_dicHeros[pDCartInstance.ownerId].NetEventCartGetOn(csCartObject);
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveCart(long lInstanceId)
	{
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			if (m_dicCart[lInstanceId] != null)
			{
				GameObject.Destroy(m_dicCart[lInstanceId].gameObject);
			}
			m_dicCart.Remove(lInstanceId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearCartObject()
	{
		foreach (var dicCart in m_dicCart)
		{
			GameObject.Destroy(dicCart.Value.gameObject);
		}
		m_dicCart.Clear();
	}

	#endregion CartManagement

	#region InteractionManagement

	//----------------------------------------------------------------------------------------------------
	protected void CreateInteractionObject(int nArrangeNo, long lInstanceId, Guid guidInteractionHeroId)
	{
		if (m_dicInteractionObject.ContainsKey(lInstanceId) == false)
		{
			CsContinent csContinent = CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId);
			if (csContinent == null) return;

			CsContinentObjectArrange csContinentObjectArrange = csContinent.ContinentObjectArrangeList.Find(a => a.ArrangeNo == nArrangeNo);
			if (csContinentObjectArrange == null) return;

			CsContinentObject csContinentObject = CsGameData.Instance.GetContinentObject(csContinentObjectArrange.ObjectId);
			if (csContinentObject == null) return;

			GameObject goInteractionObject = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/InteractionObject/{0:D2}", csContinentObject.PrefabName)) as GameObject;

			if (goInteractionObject == null)
			{
				Debug.Log("CreateInteractionObject         프리펩 네임 확인 필요         PrefabName = " + csContinentObject.PrefabName);
				return;
			}

			if (m_dicHeros.ContainsKey(guidInteractionHeroId))
			{
				Debug.Log(" OtherHero가 " + csContinentObject.PrefabName + "에게 상호작용중입니다.");
			}

			goInteractionObject = Instantiate(goInteractionObject, transform.Find("Object")) as GameObject;

			goInteractionObject.AddComponent<CapsuleCollider>();
			goInteractionObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
			goInteractionObject.AddComponent<CsInteractionObject>();

			CsInteractionObject csInteractionObject = goInteractionObject.GetComponent<CsInteractionObject>();
			m_dicInteractionObject.Add(lInstanceId, csInteractionObject);
			csInteractionObject.Init(lInstanceId, csContinentObjectArrange, csContinentObject.Name);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveInteractionObject(long lInstanceId)
	{
		if (m_dicInteractionObject.ContainsKey(lInstanceId))
		{
			Destroy(m_dicInteractionObject[lInstanceId].gameObject);
			m_dicInteractionObject.Remove(lInstanceId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void ClearInteractionObject()
	{
		foreach (var dicInteractionObject in m_dicInteractionObject)
		{
			GameObject.Destroy(dicInteractionObject.Value.gameObject);
		}
		m_dicInteractionObject.Clear();
	}

	#endregion InteractionManagement

	//----------------------------------------------------------------------------------------------------
	protected void CreateObstacle(int nObstacleId, Vector3 vtPos, float flYRotation, float flScale) // 장애물 생성.
	{
		if (m_dicObstacle.ContainsKey(nObstacleId) == false)
		{
			GameObject goObstacle = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Dungeon/Curtain"), transform.Find("Object"));
			goObstacle.SetActive(false);
			goObstacle.transform.position = vtPos;
			goObstacle.transform.localScale = new Vector3(flScale, flScale, flScale);
			goObstacle.transform.eulerAngles = new Vector3(0f, flYRotation, 0f);
			goObstacle.transform.name = nObstacleId.ToString();
			goObstacle.SetActive(true);
			m_dicObstacle.Add(nObstacleId, goObstacle);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveObstacle(int nRemoveObstacleId) // 장애물 제거.
	{
		if (m_dicObstacle.ContainsKey(nRemoveObstacleId))
		{
			Destroy(m_dicObstacle[nRemoveObstacleId]);
			m_dicObstacle.Remove(nRemoveObstacleId);
			RemoveObjectCheck();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void RemovePortal(int nPortalId)
	{
		if (m_dicPortal.ContainsKey(nPortalId))
		{
			Destroy(m_dicPortal[nPortalId].gameObject);
		}
		m_dicPortal.Remove(nPortalId);
	}

	//---------------------------------------------------------------------------------------------------
	protected void ClearPortal()
	{
		Transform trPortal = transform.Find("Portal");

		for (int i = 0; i < trPortal.childCount; i++)
		{
			Destroy(trPortal.GetChild(i).gameObject);
		}
		m_dicPortal.Clear();
	}

	#endregion ObjectManagement

	#region Setting

	//---------------------------------------------------------------------------------------------------
	IEnumerator HeroViewSetting()
	{
		yield return new WaitUntil(() => m_csPlayer != null);
		yield return new WaitForSeconds(1f);

		while (true)
		{
			if (m_csPlayer != null)
			{
				if (m_listOtherPlayer.Count > 0)
				{
					if (m_nUserViewLimit == 0 || CsIngameData.Instance.OtherHeroView == false)
					{
						for (int i = 0; i < m_listOtherPlayer.Count; i++)
						{
							if (CsIngameData.Instance.TargetTransform == m_listOtherPlayer[i].transform)
							{
								m_listOtherPlayer[i].ChangeViewHero(true);
							}
							else
							{
								m_listOtherPlayer[i].ChangeViewHero(false);
							}
						}
					}
					else
					{
						m_listOtherPlayer.Sort((x, y) => Vector3.Distance(m_csPlayer.transform.position, x.transform.position).CompareTo(Vector3.Distance(m_csPlayer.transform.position, y.transform.position)));

						for (int i = 0; i < m_listOtherPlayer.Count; i++)
						{
							if (i < m_nUserViewLimit)           // 활성화
							{
								m_listOtherPlayer[i].ChangeViewHero(true);
							}
							else                                // 비활성화
							{
								if (CsIngameData.Instance.TargetTransform == m_listOtherPlayer[i].transform)
								{
									m_listOtherPlayer[i].ChangeViewHero(true);
								}
								else
								{
									m_listOtherPlayer[i].ChangeViewHero(false);
								}
							}
						}
						RemoveObjectCheck();
					}
				}
			}
			yield return new WaitForSeconds(1f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ObjectSoundAsyncLoad()
	{
		string[] astr = {	"SFX_Mon_Hit_Public",
							"SFX_NPCVoice_7000_1", "SFX_NPCVoice_7000_2", "SFX_NPCVoice_7000_3",
							"SFX_NPCVoice_7005_1", "SFX_NPCVoice_7005_2", "SFX_NPCVoice_7005_3",
							"SFX_NPCVoice_7008_1", "SFX_NPCVoice_7008_2", "SFX_NPCVoice_7008_3",
							"SFX_NPCVoice_7012_1", "SFX_NPCVoice_7012_2", "SFX_NPCVoice_7012_3",
							"SFX_NPCVoice_7013_1", "SFX_NPCVoice_7013_2", "SFX_NPCVoice_7013_3", "SFX_NPCVoice_7013_4", "SFX_NPCVoice_7013_5",
							"SFX_NPCVoice_7014_1", "SFX_NPCVoice_7014_2", "SFX_NPCVoice_7014_3",
							"SFX_NPCVoice_7015_1", "SFX_NPCVoice_7015_2", "SFX_NPCVoice_7015_3",
							"SFX_NPCVoice_7016_1", "SFX_NPCVoice_7016_2", "SFX_NPCVoice_7016_3",
						};

		for (int i = 0; i < astr.Length; i++)
		{
			if (CsIngameData.Instance.ObjectSound.ContainsKey(astr[i]) == false)
			{
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<AudioClip>("Sound/Object/" + astr[i]);
				yield return req;
				if (req.asset != null)
				{
					CsIngameData.Instance.ObjectSound.Add(astr[i], (AudioClip)req.asset);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator SceneLoadComplete(bool bChaegeScene)
	{
		Scene scene = SceneManager.GetSceneByName(SceneManager.GetActiveScene().name);
		yield return scene.isLoaded;
		Debug.Log("SceneLoadComplete()        IsCreateEnter = " + CsGameData.Instance.MyHeroInfo.IsCreateEnter + " // bChaegeScene = " + bChaegeScene);
		
		yield return new WaitForSeconds(1f);
		CsGameEventToUI.Instance.OnEventSceneLoadComplete(bChaegeScene);
		yield return new WaitForSeconds(1f);
		CsIngameData.Instance.ActiveScene = true;
	}

	//----------------------------------------------------------------------------------------------------
	protected void RemoveObjectCheck()
	{
		m_nRemoveObjectCount++;
		if (m_nRemoveObjectCount > 200)
		{
			Debug.Log("RemoveObjectCheck           m_nRemoveObjectCount > 200             Resources.UnloadUnusedAssets()");
			GL.Flush();							//Test
			GL.Clear(true, true, Color.white);	//Test

			m_nRemoveObjectCount = 0;
			Resources.UnloadUnusedAssets();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void SetMyHeroLocation(int nLocationId = 0)
	{
		if (nLocationId == 0)
		{
			CsContinent csContinent = CsGameData.Instance.ContinentList.Find(a => a.SceneName == gameObject.scene.name);
			nLocationId = (csContinent == null) ? CsGameData.Instance.MyHeroInfo.LocationId : csContinent.LocationId;
		}

		CsGameData.Instance.MyHeroInfo.LocationId = nLocationId;
	}

	//---------------------------------------------------------------------------------------------------
	protected void DungeonEnter()
	{
		if (m_csPlayer != null)
		{
			m_csPlayer.SetAutoPlay(null, true);
			m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None, true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected void HeroHit(Guid guidHeroId, Transform trKiller, PDHitResult pDHitResult)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventHeroDead(pDHitResult);
		}
		else
		{
			m_csPlayer.NetEventHeroDead(trKiller, pDHitResult);
		}
	}

	//----------------------------------------------------------------------------------------------------
	Transform Attacker(PDAttacker pDAttacker)
	{
		if (pDAttacker.type == 1)
		{
			Guid guidHeroId = GetHeroId(pDAttacker);
			if (m_dicHeros.ContainsKey(guidHeroId))
			{
				return m_dicHeros[guidHeroId].transform;
			}
		}
		else
		{
			long lMonsterId = GetMonsterId(pDAttacker);
			if (m_dicMonsters.ContainsKey(lMonsterId))
			{
				return m_dicMonsters[lMonsterId].transform;
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	protected Guid GetHeroId(PDAttacker pDAttacker)
	{
		if (pDAttacker != null)
		{
			if (pDAttacker.type == 1)
			{
				PDHeroAttacker pdHero = (PDHeroAttacker)pDAttacker;
				return pdHero.heroId;
			}
		}
		return Guid.Empty;
	}

	//---------------------------------------------------------------------------------------------------
	protected long GetMonsterId(PDAttacker pDAttacker)
	{
		if (pDAttacker != null)
		{
			if (pDAttacker.type == 2)
			{
				PDMonsterAttacker pdMonster = (PDMonsterAttacker)pDAttacker;
				return pdMonster.monsterInstanceId;
			}
		}
		return 0;
	}

	protected virtual void CreateGoldItem(Transform trMonster)
	{

	}

	//----------------------------------------------------------------------------------------------------
	protected void DungeonExit()
	{
		Debug.Log("DungeonExit()");
		if (m_csPlayer == null)
		{
			CsIngameData.Instance.MyHeroDead = false;
		}
		else
		{
			m_csPlayer.DeadSetting(false, false);
		}
	}

	#endregion Setting

	#region IIngameManagement
	//----------------------------------------------------------------------------------------------------
	public virtual void Test(int nNum)
	{

	}

	//----------------------------------------------------------------------------------------------------
	public bool IsViewHero(Transform trHero)
	{
		if (m_listOtherPlayer.Count > m_nUserViewLimit)
		{
			for (int i = 0; i < m_nUserViewLimit; i++)
			{
				if (m_listOtherPlayer[i].transform == trHero)
				{
					return true;
				}
			}
		}
		else
		{
			return true;
		}
		return false;
	}

	//----------------------------------------------------------------------------------------------------
	public void LookAtTarget(Vector3 vtPos)
	{
		StartCoroutine(DelayLookAtTarget(vtPos));
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator DelayLookAtTarget(Vector3 vtPos)
	{
		yield return new WaitUntil(() => (m_csPlayer.SkillStatus.IsStatusPlayAnim() == false));

		m_csPlayer.transform.LookAt(vtPos);
		CsIngameData.Instance.InGameCamera.ChangeCamera(false, true, false, 0, CsGameData.Instance.MyHeroTransform.rotation.eulerAngles.y * Mathf.Deg2Rad, 0, 1f);
	}

	//----------------------------------------------------------------------------------------------------
	public void NpcDialog(int nNpcId)
	{
		m_csPlayer.ResetBattleMode();                       // 자동전투 종료.
		CsGameEventToUI.Instance.OnEventJoystickReset();    // 조이스틱 초기화.

		if (m_dicNpcs.ContainsKey(nNpcId))
		{
			m_dicNpcs[nNpcId].NetDialog();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void CreateGold(Transform trMonster)
	{
		CreateGoldItem(trMonster);
	}

	//----------------------------------------------------------------------------------------------------
	public void MonsterDead(long lInstanceId) // 몬스처 사망치 삭제처리.
	{
		RemoveMonster(lInstanceId);
	}

	//----------------------------------------------------------------------------------------------------
	public void Interaction(long lInstanceId, EnInteractionQuestType enInteractionQuestType, CsContinentObject csContinentObject)
	{
		CsContinentObjectManager.Instance.Interaction(lInstanceId, enInteractionQuestType, csContinentObject);
	}

	//----------------------------------------------------------------------------------------------------
	public void StateEndOfInteraction()
	{
		if (CsContinentObjectManager.Instance.Isinteracting)
		{
			CsContinentObjectManager.Instance.TryContinentObjectInteractionCancel();
		}
		else if (CsDimensionRaidQuestManager.Instance.Interaction)		
		{
			CsDimensionRaidQuestManager.Instance.SendDimensionRaidInteractionCancel();
		}
		else if (CsGuildManager.Instance.Interaction)
		{
			CsGuildManager.Instance.GuildInteractionCancel();
		}
		else if (CsSecretLetterQuestManager.Instance.SecretLetterPick)
		{
			CsSecretLetterQuestManager.Instance.SendSecretLetterPickCancel();
		}
		else if (CsMysteryBoxQuestManager.Instance.MysteryBoxPick)
		{
			CsMysteryBoxQuestManager.Instance.SendMysteryBoxPickCancel();
		}
		else if (CsDungeonManager.Instance.IsStateinteracting)
		{
			CsDungeonManager.Instance.TryDungeonObjectInteractionCancel();
		}
		else if (CsTrueHeroQuestManager.Instance.IsStateinteracting)
		{
			CsTrueHeroQuestManager.Instance.TryTrueHeroObjectInteractionCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void TartgetReset()
	{
		if (m_csPlayer != null)
		{
			m_csPlayer.ResetTarget();
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void PortalAreaEnter(int nPortalId) // 포탈 입장.
	{
		m_csPlayer.NetPortalEnter(nPortalId);
		AudioClip EnterSound = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Etc/SFX_potal_enter");
		if (CsIngameData.Instance.EffectSound)
		{
			m_audioSource.PlayOneShot(EnterSound);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public void DirectingEnd(bool bFade) // 연출 종료
	{
		StartCoroutine(CinemaEnd(bFade));
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator CinemaEnd(bool bFade)
	{
		if (m_csPlayer != null)
		{
			m_csPlayer.MyHeroView(true);
		}

		if (bFade)
		{
			CsGameEventToUI.Instance.OnEventFade(true);
			yield return new WaitForSeconds(0.5f);
		}

		CsIngameData.Instance.Directing = false;
		CsGameEventToUI.Instance.OnEventHideMainUI(false);
	}

	//----------------------------------------------------------------------------------------------------
	public void RemoveHeroCart(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			if (m_dicHeros[guidHeroId].IsTransformationStateCart())
			{
				m_dicHeros[guidHeroId].ChangeTransformationState(CsHero.EnTransformationState.None);
			}
		}

		RemoveCart(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	public void TrasfomationAttackStart()
	{
		m_csPlayer.TrasfomationAttackStart();
	}

	//---------------------------------------------------------------------------------------------------
	public void TrasfomationAttackEnd()
	{
		m_csPlayer.TrasfomationAttackEnd();
	}

	//---------------------------------------------------------------------------------------------------
	public bool MyHeroRequestRidingCart()
	{
		if (CsGameData.Instance.MyHeroInfo.LocationId == CsMainQuestManager.Instance.CartContinentId && m_csPlayer.GetDistanceFormTarget(CsMainQuestManager.Instance.CartPosition) < 2f)
		{
			if (m_csPlayer.IsTransformationStateNone() == false)
			{
				m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None, true);
			}
			return true;
		}
		else if (CsGameData.Instance.MyHeroInfo.LocationId == CsSupplySupportQuestManager.Instance.CartContinentId && m_csPlayer.GetDistanceFormTarget(CsSupplySupportQuestManager.Instance.CartPosition) < 2f)
		{
			if (m_csPlayer.IsTransformationStateNone() == false)
			{
				m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None, true);
			}
			return true;
		}
		else
		{
			CsGuildSupplySupportQuestPlay csGuildSupplySupportQuestPlay = CsGuildManager.Instance.GuildSupplySupportQuestPlay;
			if (csGuildSupplySupportQuestPlay == null) return false;

			if (csGuildSupplySupportQuestPlay.CartContinentId == CsGameData.Instance.MyHeroInfo.LocationId && m_csPlayer.GetDistanceFormTarget(csGuildSupplySupportQuestPlay.CartPosition) < 2f)
			{
				if (m_csPlayer.IsTransformationStateNone() == false)
				{
					m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None, true);
				}
				return true;
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void AttackByHit(Transform trTarget, PDHitResult pDHitResult, float flHitTime, PDHitResult pDAddHitResult, bool bMyHero, bool bKnockback = false)
	{
		if (trTarget != null && trTarget.GetComponent<CsMoveUnit>() != null)
		{
			trTarget.GetComponent<CsMoveUnit>().AttackByHit(pDHitResult, flHitTime, pDAddHitResult, bMyHero, bKnockback);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public long GetInstanceId(Transform trTarget)
	{
		if (trTarget != null)
		{
			CsMoveUnit csMoveUnit = trTarget.GetComponent<CsMoveUnit>();
			if (csMoveUnit != null)
			{
				return csMoveUnit.InstanceId;
			}
		}
		return 0;
	}

	//----------------------------------------------------------------------------------------------------
	public int GetNpcId(Transform trTarget)
	{
		if (trTarget != null)
		{
			CsNpc csNpc = trTarget.GetComponent<CsNpc>();
			if (csNpc != null)
			{
				return csNpc.NpcId;
			}
		}
		return 0;
	}

	//----------------------------------------------------------------------------------------------------
	public CsMoveUnit GetCsMoveUnit(Transform trTarget)
	{
		if (trTarget != null)
		{
			CsMoveUnit csMoveUnit = trTarget.GetComponent<CsMoveUnit>();
			if (csMoveUnit != null)
			{
				return csMoveUnit;
			}
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public float GetHeroHeight(Guid guidHeroId)
	{
		if (m_csPlayer.HeroId == guidHeroId)
		{
			return m_csPlayer.Height;
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			return m_dicHeros[guidHeroId].Height;
		}
		return 0;
	}

	//----------------------------------------------------------------------------------------------------
	public Transform GetAttacker(PDAttacker pDAttacker)
	{
		if (pDAttacker == null) return null;

		switch (pDAttacker.type)
		{
			case 1:
				PDHeroAttacker pdHero = (PDHeroAttacker)pDAttacker;
				if (m_dicHeros.ContainsKey(pdHero.heroId))
				{
					return m_dicHeros[pdHero.heroId].transform;
				}
				else if (m_csPlayer.HeroId == pdHero.heroId)
				{
					return m_csPlayer.transform;
				}
				break;
			case 2:
				PDMonsterAttacker pdMonster = (PDMonsterAttacker)pDAttacker;
				if (m_dicMonsters.ContainsKey(pdMonster.monsterInstanceId))
				{
					return m_dicMonsters[(int)pdMonster.monsterInstanceId].transform;
				}
				break;
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void SelectHero(Transform trHero, bool bSelect)
	{
		if (trHero.CompareTag("Hero"))
		{
			Guid guidHeroId = trHero.GetComponent<CsHero>().HeroId;
			if (m_dicHeros.ContainsKey(guidHeroId))
			{
				m_dicHeros[guidHeroId].SelectHero(bSelect);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public Guid GetHeroId(Transform trHero)
	{
		if (trHero != null)
		{
			CsHero csHero = trHero.GetComponent<CsHero>();
			if (csHero != null)
			{
				return trHero.GetComponent<CsHero>().HeroId;
			}
		}
		return Guid.Empty;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHero GetHero(Transform trHero)
	{
		if (trHero != null)
		{
			return trHero.GetComponent<CsHero>();
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public CsHero GetHero(Guid guidHeroId)
	{
		if (m_csPlayer.HeroId == guidHeroId)
		{
			return m_csPlayer;
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			return m_dicHeros[guidHeroId];
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public CsCartObject GetCsCartObject(long lInstaceId)
	{
		if (m_dicCart.ContainsKey(lInstaceId))
		{
			return m_dicCart[lInstaceId];
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public string GetName(Transform trTarget)
	{
		if (trTarget != null)
		{
			return trTarget.GetComponent<CsMoveUnit>().Name;
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public CsJob GetHeroJob(Transform trTarget)
	{
		if (trTarget != null)
		{
			CsHero csHero = trTarget.GetComponent<CsHero>();
			if (csHero != null)
			{
				return csHero.Job;
			}
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public CsNpcInfo GetNpcInfo(Transform trTarget)
	{
		if (trTarget != null)
		{
			CsNpc csNpc = trTarget.GetComponent<CsNpc>();
			if (csNpc != null)
			{
				return csNpc.NpcInfo;
			}
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public AudioClip GetNpcVoice(string strPrefabName)
	{
		if (strPrefabName == "7013")
		{
			string strKey = "SFX_NPCVoice_" + strPrefabName + "_" + UnityEngine.Random.Range(1, 6).ToString();
			if (CsIngameData.Instance.ObjectSound.ContainsKey(strKey))
			{
				return CsIngameData.Instance.ObjectSound[strKey];
			}
		}
		else
		{
			string strKey = "SFX_NPCVoice_" + strPrefabName + "_" + UnityEngine.Random.Range(1, 4).ToString();
			if (CsIngameData.Instance.ObjectSound.ContainsKey(strKey))
			{
				return CsIngameData.Instance.ObjectSound[strKey];
			}
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public AudioClip GetMonsterSound(string strName)
	{
		if (CsIngameData.Instance.ObjectSound.ContainsKey(strName))
		{
			return CsIngameData.Instance.ObjectSound[strName];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsContinent()
	{
		if (CsGameData.Instance.MyHeroInfo != null)
		{
			if (CsGameData.Instance.GetContinent(CsGameData.Instance.MyHeroInfo.LocationId) == null) return false;
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsDungeonStart()
	{
		return CsDungeonManager.Instance.DungeonStart();
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsHeroStateIdle()
	{
		return (m_csPlayer != null && m_csPlayer.State == CsHero.EnState.Idle);
	}

	//----------------------------------------------------------------------------------------------------
	public bool IsHeroStateAttack()
	{
		return (m_csPlayer != null && m_csPlayer.State == CsHero.EnState.Attack);
	}

	//----------------------------------------------------------------------------------------------------
	public bool IsHeroStateNoJoyOfMove()
	{
		return (m_csPlayer != null && m_csPlayer.IsStateNoJoyOfMove());
	}

	List<CsPlayTheme> m_listPlayTheme = new List<CsPlayTheme>();
	//----------------------------------------------------------------------------------------------------
	protected void AddPlayTheme(CsPlayTheme cs)
	{
		cs.Init(m_csPlayer);
		m_listPlayTheme.Add(cs);
	}

	//----------------------------------------------------------------------------------------------------
	public virtual void InitPlayThemes()
	{
		UninitPlayThemes();
		AddPlayTheme(new CsPlayThemeBattle());
		AddPlayTheme(new CsPlayThemeMoveMove());
	}

	//----------------------------------------------------------------------------------------------------
	public void UninitPlayThemes()
	{
		foreach (CsPlayTheme cs in m_listPlayTheme)
		{
			cs.Uninit();
		}
		m_listPlayTheme.Clear();
	}

	#endregion IIngameManagement
}

