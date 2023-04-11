using ClientCommon;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CsSceneIngameContinent : CsSceneIngameMainQuestDungeon
{
	CsQuestArea m_csGuildMissionArea = null;
	CsQuestArea m_csWeeklyQuestArea = null;
	CsQuestArea m_csBiographyQuestArea = null;
	CsQuestArea m_csTrueHeroQuestArea = null;
	CsQuestArea m_csFarmQuestArea = null;

	protected CsContinent m_csContinent;

	//----------------------------------------------------------------------------------------------------
	protected override void Awake()
	{
		base.Awake();

		if (CsGameData.Instance.MyHeroInfo != null)
		{	
			StartCoroutine(StartMonsterReourcesAsync());			// 해당 대륙 몬스터 비동기 로드.
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		if (CsGameData.Instance.MyHeroInfo == null) return;

		CsRplzSession.Instance.EventResPortalExit += OnEventResPortalExit;
		CsRplzSession.Instance.EventResPrevContinentEnter += OnEventResPrevContinentEnter;													// 이전대륙입장
		CsRplzSession.Instance.EventResContinentEnterForNationTransmission += OnEventResContinentEnterForNationTransmission;				// 국가전송
		CsRplzSession.Instance.EventResContinentEnterForContinentTransmission += OnEventResContinentEnterForContinentTransmission;			// 대륙전송
		CsRplzSession.Instance.EventResContinentSaftyAreaEnter += OnEventResContinentSaftyAreaEnter;
		CsRplzSession.Instance.EventResContinentEnterForNationCallTransmission += OnEventResContinentEnterForNationCallTransmission;

		CsRplzSession.Instance.EventResContinentEnterForReturnScrollUse += OnEventResContinentEnterForReturnScrollUse;						// 귀환주문서 사용
		CsRplzSession.Instance.EventResContinentEnterForSaftyRevival += OnEventResContinentEnterForSaftyRevival;                            // 안전부활.

		// Distortion
		CsRplzSession.Instance.EventResDistortionScrollUse += OnEventResDistortionScrollUse;												// 왜곡주문서사용
		CsRplzSession.Instance.EventResDistortionCancel += OnEventResDistortionCancel;														// 왜곡취소
		CsRplzSession.Instance.EventEvtDistortionCanceled += OnEventEvtDistortionCanceled;													// 왜곡취소
		CsRplzSession.Instance.EventEvtHeroDistortionStarted += OnEventEvtHeroDistortionStarted;									 		// 영웅왜곡시작
		CsRplzSession.Instance.EventEvtHeroDistortionFinished += OnEventEvtHeroDistortionFinished;											// 영웅왜곡종료
		CsRplzSession.Instance.EventEvtHeroDistortionCanceled += OnEventEvtHeroDistortionCanceled;											// 영웅왜곡취소 

		// ContinentObject
		CsContinentObjectManager.Instance.EventContinentObjectInteractionStart += OnEventContinentObjectInteractionStart;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted += OnEventMyHeroContinentObjectInteractionStarted;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel += OnEventMyHeroContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished += OnEventMyHeroContinentObjectInteractionFinished;
		CsContinentObjectManager.Instance.EventCreateInteractionObject += OnEventCreateInteractionObject;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionStart += OnEventHeroContinentObjectInteractionStart;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionCancel += OnEventHeroContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionFinished += OnEventHeroContinentObjectInteractionFinished;		
		
		// Cart
		CsCartManager.Instance.EventRemoveMyCart += OnEventRemoveMyCart;
		CsCartManager.Instance.EventMyHeroCartGetOn += OnEventMyHeroCartGetOn;
		CsCartManager.Instance.EventMyHeroCartGetOff += OnEventMyHeroCartGetOff;
		CsCartManager.Instance.EventCartAccelerate += OnEventCartAccelerate; // 속도.
		CsCartManager.Instance.EventCartPortalExit += OnEventCartPortalExit;

		CsCartManager.Instance.EventCartEnter += OnEventCartEnter;
		CsCartManager.Instance.EventCartExit += OnEventCartExit;
		CsCartManager.Instance.EventCartGetOn += OnEventCartGetOn;
		CsCartManager.Instance.EventCartGetOff += OnEventCartGetOff;
		CsCartManager.Instance.EventCartHighSpeedStart += OnEventCartHighSpeedStart;
		CsCartManager.Instance.EventCartHighSpeedEnd += OnEventCartHighSpeedEnd;
		CsCartManager.Instance.EventMyCartHighSpeedEnd += OnEventMyCartHighSpeedEnd;
		CsCartManager.Instance.EventCartInterestAreaEnter += OnEventCartInterestAreaEnter;
		CsCartManager.Instance.EventCartInterestAreaExit += OnEventCartInterestAreaExit;
		CsCartManager.Instance.EventCartMove += OnEventCartMove;
		CsCartManager.Instance.EventCartChanged += OnEventCartChanged;
		CsCartManager.Instance.EventCartHit += OnEventCartHit;
		CsCartManager.Instance.EventCartAbnormalStateEffectStart += OnEventCartAbnormalStateEffectStart;
		CsCartManager.Instance.EventCartAbnormalStateEffectHit += OnEventCartAbnormalStateEffectHit;
		CsCartManager.Instance.EventCartAbnormalStateEffectFinished += OnEventCartAbnormalStateEffectFinished;

		//MainQuest
		CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;
		CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
		CsMainQuestManager.Instance.EventMainQuestMonsterTransformationCanceled += OnEventMainQuestMonsterTransformationCanceled;
		CsMainQuestManager.Instance.EventMainQuestMonsterTransformationFinished += OnEventMainQuestMonsterTransformationFinished;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationStarted += OnEventHeroMainQuestMonsterTransformationStarted;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationCanceled += OnEventHeroMainQuestMonsterTransformationCanceled;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationFinished += OnEventHeroMainQuestMonsterTransformationFinished;
		CsMainQuestManager.Instance.EventHeroMainQuestTransformationMonsterSkillCast += OnEventHeroMainQuestTransformationMonsterSkillCast;

		// SecretLetter
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickStarted += OnEventHeroMysteryBoxPickStarted;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCompleted += OnEventHeroMysteryBoxPickCompleted;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCanceled += OnEventHeroMysteryBoxPickCanceled;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted += OnEventHeroMysteryBoxQuestCompleted;

		// SecretLetter
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickStarted += OnEventHeroSecretLetterPickStarted;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCompleted += OnEventHeroSecretLetterPickCompleted;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCanceled += OnEventHeroSecretLetterPickCanceled;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted += OnEventHeroSecretLetterQuestCompleted;

		// DimensionRaid
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionStarted += OnEventHeroDimensionRaidInteractionStarted;
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionCanceled += OnEventHeroDimensionRaidInteractionCanceled;
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionCompleted += OnEventHeroDimensionRaidInteractionCompleted;

		// Supplysupport
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept += OnEventSupplySupportQuestAccept;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange += OnEventSupplySupportQuestCartChange;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;

		// GuildMission
		CsGuildManager.Instance.EventUpdateMissionState += OnEventUpdateMissionState;
        
		// GuildSupplysupport
		CsGuildManager.Instance.EventGuildSupplySupportQuestAccept += OnEventGuildSupplySupportQuestAccept;
		CsGuildManager.Instance.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;
		CsGuildManager.Instance.EventGuildSupplySupportQuestFail += OnEventGuildSupplySupportQuestFail;

		// NationWar
        CsNationWarManager.Instance.EventNationWarJoin += OnEventNationWarJoin;
        CsNationWarManager.Instance.EventContinentEnterForNationWarJoin += OnEventContinentEnterForNationWarJoin;
        CsNationWarManager.Instance.EventNationWarTransmission += OnEventNationWarTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarTransmission += OnEventContinentEnterForNationWarTransmission;
        CsNationWarManager.Instance.EventNationWarNpcTransmission += OnEventNationWarNpcTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarNpcTransmission += OnEventContinentEnterForNationWarNpcTransmission;
        CsNationWarManager.Instance.EventNationWarRevive += OnEventNationWarRevive;
        CsNationWarManager.Instance.EventContinentEnterForNationWarRevive += OnEventContinentEnterForNationWarRevive;
        CsNationWarManager.Instance.EventNationWarCallTransmission += OnEventNationWarCallTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarCallTransmission += OnEventContinentEnterForNationWarCallTransmission;

        CsNationWarManager.Instance.EventNationWarStart += OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished += OnEventNationWarFinished;
		CsNationWarManager.Instance.EventNationWarMonsterDead += OnEventNationWarMonsterDead;
		CsNationWarManager.Instance.EventNationWarMonsterSpawn += OnEventNationWarMonsterSpawn;

		// WeeklyQuest
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept += OnEventWeeklyQuestRoundAccept;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;

		// TrueHero
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept += OnEventTrueHeroQuestAccept;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepCompleted;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepInteractionFinished += OnEventTrueHeroQuestStepInteractionFinished;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepWaitingCanceled += OnEventTrueHeroQuestStepWaitingCanceled;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionStarted += OnEventHeroTrueHeroQuestStepInteractionStarted;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionCanceled += OnEventHeroTrueHeroQuestStepInteractionCanceled;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionFinished += OnEventHeroTrueHeroQuestStepInteractionFinished;

		// Biography
		CsBiographyManager.Instance.EventBiographyQuestAccept += OnEventBiographyQuestAccept;
		CsBiographyManager.Instance.EventBiographyQuestComplete += OnEventBiographyQuestComplete;
		CsBiographyManager.Instance.EventBiographyQuestMoveObjectiveComplete += OnEventBiographyQuestMoveObjectiveComplete;

		// CreatureFarmQuest
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept += OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete += OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete += OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted += OnEventCreatureFarmQuestMissionCompleted;

		// JobChange
		CsJobChangeManager.Instance.EventJobChangeQuestMonsterSpawned += OnEventJobChangeQuestMonsterSpawned;
		CsJobChangeManager.Instance.EventJobChangeQuestFailed += OnEventJobChangeQuestFailed;
		CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;
		CsJobChangeManager.Instance.EventHeroJobChanged += OnEventHeroJobChanged;


		if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.Portal)
		{
			if (CsCartManager.Instance.IsMyHeroRidingCart) // 카트 탑승중.
			{
				CsCartManager.Instance.SendCartPortalExit();
			}
			else
			{
				SendPortalExit();
			}
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.PrevContinent)
		{
			SendPrevContinentEnterCommand();// 이전대륙입장 (던전 >> 대륙)
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.ReturnScroll)
		{
			SendContinentEnterForReturnScrollUseCommand();
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.Revival)
		{
			SendContinentEnterForSaftyRevival();
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.ContinentTransmission)
		{
			SendContinentEnterForContinentTransmission();
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationTransmission)
		{
			SendContinentEnterForNationTransmission();
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.SaftyAreaEnter) // ContinentSaftyAreaEnter
		{
			SendContinentSaftyAreaEnter();
		}
        else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationWarJoin)
        {
            CsNationWarManager.Instance.SendContinentEnterForNationWarJoin();
        }
        else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationWarTransmission)
		{
            CsNationWarManager.Instance.SendContinentEnterForNationWarTransmission();
        }
        else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationWarNpcTransmission)
		{
            CsNationWarManager.Instance.SendContinentEnterForNationWarNpcTransmission();
        }
        else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationWarRevive)
		{
            CsNationWarManager.Instance.SendContinentEnterForNationWarRevive();
        }
        else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationWarCallTransmission)
		{		
            CsNationWarManager.Instance.SendContinentEnterForNationWarCallTransmission();
        }
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.GuildCallTransmission)
		{
			CsGuildManager.Instance.SendContinentEnterForGuildCallTransmission();
		}
		else if (CsGameData.Instance.MyHeroInfo.MyHeroEnterType == EnMyHeroEnterType.NationCallTransmission) // 14
		{
			SendContinentEnterForNationCallTransmission();
		}
        else
        {
			if (CsDungeonManager.Instance.DungeonPlay == EnDungeonPlay.MainQuest)
			{
				CsMainQuestDungeonManager.Instance.SendMainQuestDungeonEnter();
			}
			else
			{
				SendHeroInitEnterCommand();
			}
        }

		CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.InitEnter; // 초기화.

		StartCoroutine(CreateContinentPotal(m_csContinent.ContinentId));
		CreateSceneryQuestArea();
		CreateWeeklyQuestArea();
		CreateTrueHeroQuestArea();
		CreateBiographyQuestArea();
		CreateCreatureFarmQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		base.OnDestroy();
		CsRplzSession.Instance.EventResPortalExit -= OnEventResPortalExit;
		CsRplzSession.Instance.EventResPrevContinentEnter -= OnEventResPrevContinentEnter;													// 이전대륙입장
		CsRplzSession.Instance.EventResContinentEnterForNationTransmission -= OnEventResContinentEnterForNationTransmission;				// 국가전송
		CsRplzSession.Instance.EventResContinentEnterForContinentTransmission -= OnEventResContinentEnterForContinentTransmission;			// 대륙전송
		CsRplzSession.Instance.EventResContinentSaftyAreaEnter -= OnEventResContinentSaftyAreaEnter;
		CsRplzSession.Instance.EventResContinentEnterForNationCallTransmission -= OnEventResContinentEnterForNationCallTransmission;
		CsRplzSession.Instance.EventResContinentEnterForReturnScrollUse -= OnEventResContinentEnterForReturnScrollUse;					    // 귀환주문서 사용
		CsRplzSession.Instance.EventResContinentEnterForSaftyRevival -= OnEventResContinentEnterForSaftyRevival;							// 안전부활.

		//Distortion
		CsRplzSession.Instance.EventResDistortionScrollUse -= OnEventResDistortionScrollUse;                                                // 왜곡주문서사용
		CsRplzSession.Instance.EventResDistortionCancel -= OnEventResDistortionCancel;                                                      // 왜곡취소
		CsRplzSession.Instance.EventEvtDistortionCanceled -= OnEventEvtDistortionCanceled;                                                  //왜곡취소
		CsRplzSession.Instance.EventEvtHeroDistortionStarted -= OnEventEvtHeroDistortionStarted;                                            // 영웅왜곡시작
		CsRplzSession.Instance.EventEvtHeroDistortionFinished -= OnEventEvtHeroDistortionFinished;                                          // 영웅왜곡종료
		CsRplzSession.Instance.EventEvtHeroDistortionCanceled -= OnEventEvtHeroDistortionCanceled;                                          // 영웅왜곡취소 

		// ContinentObject
		CsContinentObjectManager.Instance.EventContinentObjectInteractionStart -= OnEventContinentObjectInteractionStart;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionStarted -= OnEventMyHeroContinentObjectInteractionStarted;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionCancel -= OnEventMyHeroContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished -= OnEventMyHeroContinentObjectInteractionFinished;
		CsContinentObjectManager.Instance.EventCreateInteractionObject -= OnEventCreateInteractionObject;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionStart -= OnEventHeroContinentObjectInteractionStart;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionCancel -= OnEventHeroContinentObjectInteractionCancel;
		CsContinentObjectManager.Instance.EventHeroContinentObjectInteractionFinished -= OnEventHeroContinentObjectInteractionFinished;

		// Cart
		CsCartManager.Instance.EventRemoveMyCart -= OnEventRemoveMyCart;
		CsCartManager.Instance.EventMyHeroCartGetOn -= OnEventMyHeroCartGetOn;
		CsCartManager.Instance.EventMyHeroCartGetOff -= OnEventMyHeroCartGetOff;
		CsCartManager.Instance.EventCartAccelerate -= OnEventCartAccelerate; 
		CsCartManager.Instance.EventCartPortalExit -= OnEventCartPortalExit;
		CsCartManager.Instance.EventCartEnter -= OnEventCartEnter;
		CsCartManager.Instance.EventCartExit -= OnEventCartExit;
		CsCartManager.Instance.EventCartGetOn -= OnEventCartGetOn;
		CsCartManager.Instance.EventCartGetOff -= OnEventCartGetOff;
		CsCartManager.Instance.EventCartHighSpeedStart -= OnEventCartHighSpeedStart;
		CsCartManager.Instance.EventCartHighSpeedEnd -= OnEventCartHighSpeedEnd;
		CsCartManager.Instance.EventMyCartHighSpeedEnd -= OnEventMyCartHighSpeedEnd;
		CsCartManager.Instance.EventCartInterestAreaEnter -= OnEventCartInterestAreaEnter;
		CsCartManager.Instance.EventCartInterestAreaExit -= OnEventCartInterestAreaExit;
		CsCartManager.Instance.EventCartMove -= OnEventCartMove;
		CsCartManager.Instance.EventCartChanged -= OnEventCartChanged;
		CsCartManager.Instance.EventCartHit -= OnEventCartHit;
		CsCartManager.Instance.EventCartAbnormalStateEffectStart -= OnEventCartAbnormalStateEffectStart;
		CsCartManager.Instance.EventCartAbnormalStateEffectHit -= OnEventCartAbnormalStateEffectHit;
		CsCartManager.Instance.EventCartAbnormalStateEffectFinished -= OnEventCartAbnormalStateEffectFinished;

		//MainQuest
		CsMainQuestManager.Instance.EventAccepted -= OnEventAccepted;
		CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
		CsMainQuestManager.Instance.EventMainQuestMonsterTransformationCanceled -= OnEventMainQuestMonsterTransformationCanceled;
		CsMainQuestManager.Instance.EventMainQuestMonsterTransformationFinished -= OnEventMainQuestMonsterTransformationFinished;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationStarted -= OnEventHeroMainQuestMonsterTransformationStarted;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationCanceled -= OnEventHeroMainQuestMonsterTransformationCanceled;
		CsMainQuestManager.Instance.EventHeroMainQuestMonsterTransformationFinished -= OnEventHeroMainQuestMonsterTransformationFinished;
		CsMainQuestManager.Instance.EventHeroMainQuestTransformationMonsterSkillCast -= OnEventHeroMainQuestTransformationMonsterSkillCast;


		// MysteryBoxPick
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickStarted -= OnEventHeroMysteryBoxPickStarted;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCompleted -= OnEventHeroMysteryBoxPickCompleted;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxPickCanceled -= OnEventHeroMysteryBoxPickCanceled;
		CsMysteryBoxQuestManager.Instance.EventHeroMysteryBoxQuestCompleted -= OnEventHeroMysteryBoxQuestCompleted;

		// SecretLetter
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickStarted -= OnEventHeroSecretLetterPickStarted;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCompleted -= OnEventHeroSecretLetterPickCompleted;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterPickCanceled -= OnEventHeroSecretLetterPickCanceled;
		CsSecretLetterQuestManager.Instance.EventHeroSecretLetterQuestCompleted -= OnEventHeroSecretLetterQuestCompleted;

		// DimensionRaid
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionStarted -= OnEventHeroDimensionRaidInteractionStarted;
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionCanceled -= OnEventHeroDimensionRaidInteractionCanceled;
		CsDimensionRaidQuestManager.Instance.EventHeroDimensionRaidInteractionCompleted -= OnEventHeroDimensionRaidInteractionCompleted;

		// Supplysupport
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestAccept -= OnEventSupplySupportQuestAccept;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestCartChange -= OnEventSupplySupportQuestCartChange;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;
		CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;

		// GuildMission
		CsGuildManager.Instance.EventUpdateMissionState -= OnEventUpdateMissionState;

		// GuildSupplysupport
		CsGuildManager.Instance.EventGuildSupplySupportQuestAccept -= OnEventGuildSupplySupportQuestAccept;
		CsGuildManager.Instance.EventGuildSupplySupportQuestComplete -= OnEventGuildSupplySupportQuestComplete;
		CsGuildManager.Instance.EventGuildSupplySupportQuestFail -= OnEventGuildSupplySupportQuestFail;

        // NationWar
        CsNationWarManager.Instance.EventNationWarJoin -= OnEventNationWarJoin;
        CsNationWarManager.Instance.EventContinentEnterForNationWarJoin -= OnEventContinentEnterForNationWarJoin;
        CsNationWarManager.Instance.EventNationWarTransmission -= OnEventNationWarTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarTransmission -= OnEventContinentEnterForNationWarTransmission;
        CsNationWarManager.Instance.EventNationWarNpcTransmission -= OnEventNationWarNpcTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarNpcTransmission -= OnEventContinentEnterForNationWarNpcTransmission;
        CsNationWarManager.Instance.EventNationWarRevive -= OnEventNationWarRevive;
        CsNationWarManager.Instance.EventContinentEnterForNationWarRevive -= OnEventContinentEnterForNationWarRevive;
        CsNationWarManager.Instance.EventNationWarCallTransmission -= OnEventNationWarCallTransmission;
        CsNationWarManager.Instance.EventContinentEnterForNationWarCallTransmission -= OnEventContinentEnterForNationWarCallTransmission;
        CsNationWarManager.Instance.EventNationWarStart -= OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished -= OnEventNationWarFinished;
		CsNationWarManager.Instance.EventNationWarMonsterDead -= OnEventNationWarMonsterDead;
		CsNationWarManager.Instance.EventNationWarMonsterSpawn -= OnEventNationWarMonsterSpawn;

		// WeeklyQuest
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundAccept -= OnEventWeeklyQuestRoundAccept;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete -= OnEventWeeklyQuestRoundMoveMissionComplete;

		// TrueHero
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAccept -= OnEventTrueHeroQuestAccept;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepCompleted; // 오브젝트 생성.
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestStepInteractionFinished -= OnEventTrueHeroQuestStepInteractionFinished;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionStarted -= OnEventHeroTrueHeroQuestStepInteractionStarted;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionCanceled -= OnEventHeroTrueHeroQuestStepInteractionCanceled;
		CsTrueHeroQuestManager.Instance.EventHeroTrueHeroQuestStepInteractionFinished -= OnEventHeroTrueHeroQuestStepInteractionFinished;

		// Biography
		CsBiographyManager.Instance.EventBiographyQuestAccept -= OnEventBiographyQuestAccept;
		CsBiographyManager.Instance.EventBiographyQuestComplete -= OnEventBiographyQuestComplete;
		CsBiographyManager.Instance.EventBiographyQuestMoveObjectiveComplete -= OnEventBiographyQuestMoveObjectiveComplete;

		// CreatureFarmQuest
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestAccept -= OnEventCreatureFarmQuestAccept;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete -= OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete -= OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted -= OnEventCreatureFarmQuestMissionCompleted;

		// JobChange
		CsJobChangeManager.Instance.EventJobChangeQuestMonsterSpawned -= OnEventJobChangeQuestMonsterSpawned;
		CsJobChangeManager.Instance.EventJobChangeQuestFailed -= OnEventJobChangeQuestFailed;
		CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;
		CsJobChangeManager.Instance.EventHeroJobChanged -= OnEventHeroJobChanged;

		m_csGuildMissionArea = null;
		m_csWeeklyQuestArea = null;
		m_csBiographyQuestArea = null;
		m_csTrueHeroQuestArea = null;
		m_csFarmQuestArea = null;
	}

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();

		if (IsContinent())
		{
			AddPlayTheme(new CsPlayThemeQuestMain());               // 메인
			AddPlayTheme(new CsPlayThemeQuestSub());				// 서브
			AddPlayTheme(new CsPlayThemeQuestDaily());              // 일일
			AddPlayTheme(new CsPlayThemeQuestWeekly());             // 주간
			AddPlayTheme(new CsPlayThemeQuestJobChange());			// 전직

			AddPlayTheme(new CsPlayThemeQuestBountyHunter());		// 현상금사냥꾼
			AddPlayTheme(new CsPlayThemeQuestThreatOfFarm());		// 농장의위협
			AddPlayTheme(new CsPlayThemeQuestMysteryBox());			// 의문의상자
			AddPlayTheme(new CsPlayThemeQuestSecretLetter());		// 밀서
			AddPlayTheme(new CsPlayThemeQuestDimensionRaid());		// 차원습격
			AddPlayTheme(new CsPlayThemeQuestHolyWar());			// 위대한성전
			AddPlayTheme(new CsPlayThemeQuestSupplySupport());      // 보급지원
			AddPlayTheme(new CsPlayThemeQuestBiography());          // 전기
			AddPlayTheme(new CsPlayThemeQuestCreatureFarm());       // 크리쳐퀘스트
			
			AddPlayTheme(new CsPlayThemeQuestGuildMission());		// 길드미션
			AddPlayTheme(new CsPlayThemeQuestGuildSupplySupport()); // 길드 물자지원
			AddPlayTheme(new CsPlayThemeQuestGuildHunting());       // 길드 헌팅
			AddPlayTheme(new CsPlayThemeQuestTrueHero());			// 길드 헌팅

			CreateGuildMissionArea();
		}
		else
		{
			AddPlayTheme(new CsPlayThemeDungeonMainQuest());
		}
	}

	# region SendToServer
	//---------------------------------------------------------------------------------------------------
	protected void SendContinentEnterForReturnScrollUseCommand()
	{
		Debug.Log("SendContinentEnterForReturnScrollUseCommand   m_bWaitResponse = "+ m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentEnterForReturnScrollUseCommandBody csEvt = new ContinentEnterForReturnScrollUseCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForReturnScrollUse, csEvt);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendContinentEnterForSaftyRevival() // 안전부활에의한대륙입장
	{
		Debug.Log("SendContinentEnterForSaftyRevival    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentEnterForSaftyRevivalCommandBody csEvt = new ContinentEnterForSaftyRevivalCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForSaftyRevival, csEvt);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendPortalExit()
	{
		Debug.Log("SendPortalExit()    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			PortalExitCommandBody cmdBody = new PortalExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.PortalExit, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendContinentEnterForContinentTransmission()
	{
		Debug.Log("SendContinentEnterForContinentTransmission()    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentEnterForContinentTransmissionCommandBody cmdBoby = new ContinentEnterForContinentTransmissionCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForContinentTransmission, cmdBoby);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendContinentEnterForNationTransmission()
	{
		Debug.Log("SendContinentEnterForNationTransmission()    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationTransmissionCommandBody cmdBoby = new ContinentEnterForNationTransmissionCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationTransmission, cmdBoby);
		}
	}

	////----------------------------------------------------------------------------------------------------
	protected void SendContinentSaftyAreaEnter()
	{
		Debug.Log("SendContinentSaftyAreaEnter()    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentSaftyAreaEnterCommandBody cmdBoby = new ContinentSaftyAreaEnterCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentSaftyAreaEnter, cmdBoby);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void SendContinentEnterForNationCallTransmission()
	{
		Debug.Log("SendContinentEnterForNationCallTransmission()    m_bWaitResponse = " + m_bWaitResponse);
		if (m_bWaitResponse == false)
		{
			m_bWaitResponse = true;
			ContinentEnterForNationCallTransmissionCommandBody cmdBoby = new ContinentEnterForNationCallTransmissionCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ContinentEnterForNationCallTransmission, cmdBoby);
		}
	}

	# endregion SendToServer

	# region ClientCommon
	//---------------------------------------------------------------------------------------------------
	void OnEventResPortalExit(int nReturnCode, PortalExitResponseBody csRes)
	{
		Debug.Log("OnEventResPortalExit          ReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			if (nReturnCode == 101)
			{
				Debug.Log("OnEventResPortalExit        nReturnCode == 101 : 대상 장소는 국가전중입니다. ");
				CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
				CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
			}
			else
			{
				CsDungeonManager.Instance.ContinentEnterFail();
			}
		}

		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	protected virtual void OnEventResPrevContinentEnter(int nReturnCode, PrevContinentEnterResponseBody csRes) // 이전 대륙 입장. ( 던전 >> 대륙 )
	{
		Debug.Log("OnEventResPrevContinentEnter         ReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsDungeonManager.Instance.ResetDungeon();

			if (CsMainQuestManager.Instance.WaitMainQuestComplete)
			{
				CsMainQuestManager.Instance.Complete();
			}

			if (m_bChaegeScene == false)
			{
				StartCoroutine(CreateContinentPotal(m_csContinent.ContinentId));
			}

			SetEnter(csRes.entranceInfo);

			CsGameEventToUI.Instance.OnEventPrevContinentEnter(); //  >> ui로 던전에서 대륙으로 이동 여부 전달.
		}
		else
		{
			if (nReturnCode == 101)
			{
				Debug.Log("OnEventResPrevContinentEnter        nReturnCode == 102 : 대상 장소는 국가전중입니다. ");
				CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
				CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
			}
		}
		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationTransmission(int nReturnCode, ContinentEnterForNationTransmissionResponseBody csRes)
	{
		Debug.Log("OnEventResContinentEnterForNationTransmission        nReturnCode : " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			if (nReturnCode == 101)
			{
				Debug.Log("OnEventResContinentEnterForNationTransmission        nReturnCode == 102 : 대상 장소는 국가전중입니다. ");
				CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
				CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
			}
			else
			{
				CsDungeonManager.Instance.ContinentEnterFail();
			}
		}
		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForContinentTransmission(int nReturnCode, ContinentEnterForContinentTransmissionResponseBody csRes)
	{
		Debug.Log("OnEventResContinentEnterForContinentTransmission        nReturnCode : " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			CsDungeonManager.Instance.ContinentEnterFail();
		}
		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResContinentSaftyAreaEnter(int nReturnCode, ContinentSaftyAreaEnterResponseBody csRes) // 대륙안전지역입장
	{
		Debug.Log("OnEventResContinentSaftyAreaEnter                nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			Debug.Log("안전지역입장 실패");
		}
		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForReturnScrollUse(int nReturnCode, ContinentEnterForReturnScrollUseResponseBody csRes) // 귀환 주문서 사용 입장.
	{
		Debug.Log("OnEventResContinentEnterForReturnScrollUse                nReturnCode = " + nReturnCode);
		CsIngameData.Instance.ReturnScroll = false;
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			Debug.Log("OnEventResContinentEnterForReturnScrollUse");
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
			CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
		}
		m_bWaitResponse = false;
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForSaftyRevival(int nReturnCode, ContinentEnterForSaftyRevivalResponseBody csRes) // 대륙 부활 입장.
	{
		Debug.Log("OnEventResContinentEnterForSaftyRevival         nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Hp = csRes.hp;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = csRes.date;
			CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = csRes.paidImmediateRevivalDailyCount;
			CsIngameData.Instance.MyHeroDead = false;

			SetEnter(csRes.entranceInfo);

			CsGameEventToUI.Instance.OnEventMyHeroInfoUpdate(); // 정보갱신 전달.

			Debug.Log(" 일일유료즉시부활횟수            RevivalDailyCount =  " + csRes.paidImmediateRevivalDailyCount);
		}
		else
		{
			Debug.Log("OnEventResContinentEnterForSaftyRevival");
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.SaftyAreaEnter;
			CsGameEventToUI.Instance.OnEventContinentSaftySceneLoad();
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResContinentEnterForNationCallTransmission(int nReturnCode, ContinentEnterForNationCallTransmissionResponseBody csRes)
	{
		Debug.Log("OnEventResContinentEnterForNationCallTransmission                nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			SetEnter(csRes.entranceInfo);
		}
		else
		{
			CsDungeonManager.Instance.ContinentEnterFail();
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResDistortionScrollUse(int nReturnCode, DistortionScrollUseResponseBody csRes)	// 왜곡주문서사용
	{
		Debug.Log("OnEventResDistortionScrollUse                nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_csPlayer.NetEventDistortionStart();
		}
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResDistortionCancel(int nReturnCode, DistortionCancelResponseBody csRes)    // 왜곡취소
	{
		Debug.Log("OnEventResDistortionCancel                nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_csPlayer.NetEventDistortionFinish();
		}
		m_bWaitResponse = false;
	}

	#endregion ClientCommon

	#region Distortion

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtDistortionCanceled(SEBDistortionCanceledEventBody csEvt) // 당사자 왜곡취소
	{
		Debug.Log("OnEventEvtDistortionCanceled");
		m_csPlayer.NetEventDistortionFinish();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDistortionStarted(SEBHeroDistortionStartedEventBody csEvt)   // 영웅왜곡시작
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroDistortionStarted");
			m_dicHeros[csEvt.heroId].NetEventDistortionStart();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDistortionFinished(SEBHeroDistortionFinishedEventBody csEvt) // 영웅왜곡종료
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroDistortionFinished");
			m_dicHeros[csEvt.heroId].NetEventDistortionFinish();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDistortionCanceled(SEBHeroDistortionCanceledEventBody csEvt) // 영웅왜곡취소 
	{
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("OnEventEvtHeroDistortionCanceled");
			m_dicHeros[csEvt.heroId].NetEventDistortionFinish();
		}
	}

	#endregion Distortion

	#region ContinentObject

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentObjectInteractionStart()
	{
		m_csPlayer.ContinentObjectInteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroContinentObjectInteractionStarted(long lInstanceId, EnInteractionQuestType enInteractionQuestType, CsContinentObject csContinentObject)
	{
		if (m_csPlayer.State != CsHero.EnState.Interaction)
		{
			Debug.Log(" #####     OnEventMyHeroContinentObjectInteractionStarted       >>>     StateEndOfInteraction()      #####");
			m_csPlayer.ChangeState(CsHero.EnState.Idle);
			StateEndOfInteraction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroContinentObjectInteractionCancel()
	{
		Debug.Log(" #####     OnEventMyHeroContinentObjectInteractionCancel     #####");
		m_csPlayer.ContinentObjectInteractionCancel();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroContinentObjectInteractionFinished(long lInstanceId) // 내가 완료한 상호작용 오브젝트.
	{
		Debug.Log(" #####     OnEventMyHeroContinentObjectInteractionFinished     #####    lInstanceId = "+ lInstanceId);
		m_csPlayer.ContinentObjectInteractionFinished();

		if (m_dicInteractionObject.ContainsKey(lInstanceId))
		{
			m_dicInteractionObject[lInstanceId].InteractionFinish();

			if (m_dicInteractionObject[lInstanceId].ContinentObject.IsPublic) // 공용 상호작용 오브젝트인경우 삭제 처리.  (리스폰 발생)
			{
				StartCoroutine(DelayRemoveInteractionObject(lInstanceId));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayRemoveInteractionObject(long lInstanceId)
	{
		yield return new WaitForSeconds(0.5f);
		RemoveInteractionObject(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreateInteractionObject(int nArrangeNo, long lInstanceId)
	{
		CreateInteractionObject(nArrangeNo, lInstanceId, Guid.Empty);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroContinentObjectInteractionStart(Guid guidHeroId, long lContinentObjectInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId)) // 플레이어가 존재하고.
		{
			if (m_dicInteractionObject.ContainsKey(lContinentObjectInstanceId)) // 상호작용할 Object가 존재할때.
			{
				float flMaxRange = m_dicInteractionObject[lContinentObjectInstanceId].ContinentObject.InteractionMaxRange - 0.5f;
				m_dicHeros[guidHeroId].NetEventInteractionStart(m_dicInteractionObject[lContinentObjectInstanceId].transform.position, flMaxRange);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroContinentObjectInteractionCancel(Guid guidHeroId) // OtherHero.
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroContinentObjectInteractionFinished(Guid guidHeroId, long lInstanceId) // 타유저 완료 한 상호작용.
	{
		if (m_dicInteractionObject.ContainsKey(lInstanceId))
		{	
			if (m_dicInteractionObject[lInstanceId].ContinentObject.IsPublic) // 공용 상호작용 오브젝트인경우 삭제 처리.  (리스폰 발생)
			{
				RemoveInteractionObject(lInstanceId);
			}
		}

		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionFinished();
		}
	}

	#endregion ContinentObject

	#region Cart

	//----------------------------------------------------------------------------------------------------
	void OnEventMyHeroCartGetOn(PDCartInstance pDCartInstance) // 당사자 카트 탑승.
	{
		CsGameData.Instance.MyHeroInfo.CartInstance = pDCartInstance;
		StartCoroutine(CreateCart(pDCartInstance));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventMyHeroCartGetOff() // 당사자 카트 하차.
	{
		CsGameData.Instance.MyHeroInfo.CartInstance = null; // 초기화.
		m_csPlayer.NetEventCartGetOff();
		if (m_csPlayer.IsAutoPlaying)
		{
			m_csPlayer.SetAutoPlay(null, true);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartAccelerate(bool bSuccess, float flRemainingAccelCoolTime) // 당사자 가속.
	{
		m_csPlayer.NetEventCartAccelerate(bSuccess);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartPortalExit(PDContinentEntranceInfo pDContinentEntranceInfo) // 당사자 포탈 퇴장.
	{
		SetEnter(pDContinentEntranceInfo);
	}
	
	//----------------------------------------------------------------------------------------------------
	void OnEventCartEnter(PDCartInstance pDCartInstance) // 카트 입장. (OtherHero 최초 접속 및 필드로 재 입장시)
	{
		StartCoroutine(CreateCart(pDCartInstance));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartExit(long lCartInstaceId) // 카트 퇴장.
	{
		CsCartObject csCartObject = GetCsCartObject(lCartInstaceId);

		if (csCartObject != null)
		{
			if (m_dicHeros.ContainsKey(csCartObject.OwnerId)) // 
			{
				m_dicHeros[csCartObject.OwnerId].NetEventCartGetOff(); // 카트 탑승 해제.
			}
		}
		RemoveCart(lCartInstaceId); // 카트 삭제.
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartGetOn(Guid guidHeroId, PDCartInstance pDCartInstance) // OtherHero 카트 탑승. (Other가 존재하는 상태에서 탑승상태로 변경)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			StartCoroutine(CreateCart(pDCartInstance));
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartGetOff(PDHero pDHero, long lCartInstaceId) // Other 카트내리기. (카트를 타고있는 Other가 존재함.)
	{
		if (m_dicCart.ContainsKey(lCartInstaceId))
		{
			if (m_dicHeros.ContainsKey(m_dicCart[lCartInstaceId].OwnerId)) // Other가 존재 할때.
			{
				m_dicHeros[m_dicCart[lCartInstaceId].OwnerId].NetEventCartGetOff();
			}
			else
			{
				Debug.Log("#####      OnEventCartHighSpeedEnd          카트 정보만 있고 OtherPlayer 정보는 없음.      ");
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartHighSpeedStart(long lCartInstaceId)
	{
		if (m_dicCart.ContainsKey(lCartInstaceId))
		{
			if (m_dicHeros.ContainsKey(m_dicCart[lCartInstaceId].OwnerId)) // Other에 이동 전달.
			{
				m_dicHeros[m_dicCart[lCartInstaceId].OwnerId].NetEventCartAccelerate(true);
			}
			else
			{
				Debug.Log("#####      OnEventCartHighSpeedEnd          카트 정보만 있고 OtherPlayer 정보는 없음.      ");
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartHighSpeedEnd(long lCartInstaceId)
	{
		if (m_dicCart.ContainsKey(lCartInstaceId))
		{
			if (m_dicHeros.ContainsKey(m_dicCart[lCartInstaceId].OwnerId)) // Other에 이동 전달.
			{
				m_dicHeros[m_dicCart[lCartInstaceId].OwnerId].NetEventCartHighSpeedEnd();
			}
			else
			{
				Debug.Log("#####      OnEventCartHighSpeedEnd          카트 정보만 있고 OtherPlayer 정보는 없음.      ");
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventMyCartHighSpeedEnd()
	{
		m_csPlayer.NetEventCartHighSpeedEnd();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartInterestAreaEnter(PDCartInstance pDCartInstance) // 관심지역 카트 입장.
	{
		StartCoroutine(CreateCart(pDCartInstance));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartInterestAreaExit(long lCartInstaceId)  // 관심지역 카트 퇴장.
	{
		if (m_dicCart.ContainsKey(lCartInstaceId))
		{
			if (m_dicCart[lCartInstaceId].RidingCart)
			{
				if (m_dicHeros.ContainsKey(m_dicCart[lCartInstaceId].OwnerId))
				{
					RemoveHero(m_dicCart[lCartInstaceId].OwnerId);
				}
			}
		}
		RemoveCart(lCartInstaceId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartMove(long lCartInstaceId, PDVector3 pDVector3, float flRotationY) // OtherCart 이동. < Other가 생성 안되고 카트 정보로만 생성 된 경우 문제 발생 될 수 있음).
	{
		if (m_dicCart.ContainsKey(lCartInstaceId))
		{
			if (m_dicHeros.ContainsKey(m_dicCart[lCartInstaceId].OwnerId)) // Other에 이동 전달.
			{
				m_dicHeros[m_dicCart[lCartInstaceId].OwnerId].NetEventCartMove(CsRplzSession.Translate(pDVector3), flRotationY);
			}
			else
			{
				Debug.Log("#####      OnEventCartMove          카트 정보만 있고 OtherPlayer 정보는 없음.      ");
			}
		}
	}

	// 소유자 이외 관련 유저
	//----------------------------------------------------------------------------------------------------
	void OnEventCartChanged(long lInstanceId, int nCartId) 
	{
		Debug.Log("OnEventCartChanged()");
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			m_dicCart[lInstanceId].CartInstance.cartId = nCartId;
			ChangeCart(m_dicCart[lInstanceId].CartInstance);
		}
	}

	// 소유자 포함 관련 유저
	//----------------------------------------------------------------------------------------------------
	void OnEventCartHit(long lInstanceId, PDHitResult pDHitResult) // 소유자 포함 관련 유저
	{
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			m_dicCart[lInstanceId].NetEventCartHit(pDHitResult);
		}
	}

	// 소유자 포함 관련 유저
	//----------------------------------------------------------------------------------------------------
	void OnEventCartAbnormalStateEffectStart(long lInstanceId, long lAbnormalStateEffectId, int nAbnormalStateId, int nSourceJobId, int nAbnormalLevel, int nDamageAbsorbShieldRemain , float flRemainTime, long[] alRemovedAbnormalEffects)
	{
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			m_dicCart[lInstanceId].NetEventCartAbnormalStateEffectStart(lAbnormalStateEffectId, nAbnormalStateId, nSourceJobId, nAbnormalLevel, nDamageAbsorbShieldRemain, flRemainTime, alRemovedAbnormalEffects);
		}
	}

	// 소유자 포함 관련 유저
	//----------------------------------------------------------------------------------------------------
	void OnEventCartAbnormalStateEffectHit(long lInstanceId, int nHp, long[] alRemovedAbnormalStateEffects, long lAbnormalStateEffectId, int nDamage, int nHpDamage, Transform trAttacker)
	{
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			m_dicCart[lInstanceId].NetEventCartAbnormalStateEffectHit(nHp, alRemovedAbnormalStateEffects, lAbnormalStateEffectId, nDamage, nHpDamage, trAttacker);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventCartAbnormalStateEffectFinished(long lInstanceId, long lRemovedAbnormalStateEffect) // 소유자 포함 관련 유저
	{
		if (m_dicCart.ContainsKey(lInstanceId))
		{
			m_dicCart[lInstanceId].NetEventCartAbnormalStateEffectFinished(lRemovedAbnormalStateEffect);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRemoveMyCart()
	{
		if (m_csPlayer.IsTransformationStateCart())
		{
			m_csPlayer.NetEventCartGetOff();
		}
		RemoveCart(m_csPlayer.CartObject.InstanceId);
		CsGameData.Instance.MyHeroInfo.CartInstance = null;
	}

	#endregion Cart

	#region MainQuest
	//---------------------------------------------------------------------------------------------------
	void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("OnEventAccepted       "+ CsMainQuestManager.Instance.MainQuest.MainQuestType + " , "+ nTransformationMonsterId);

		if (CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Dungeon)
		{
			CsMainQuestDungeonManager.Instance.ContinentExit(); // 퀘스트 던전 시작을 위한 이전대륙 퇴장.
		}
		else if (CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Cart)
		{
			StartCoroutine(CreateCart(CsGameData.Instance.MyHeroInfo.CartInstance));
		}

		if (nTransformationMonsterId != 0)
		{
			m_csPlayer.NetEventTransformationMonster(nTransformationMonsterId, m_csPlayer.MaxHp, m_csPlayer.Hp, alRemovedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCompleted(CsMainQuest csMainQuest, bool b, long lAcquiredExp)           // 메인 퀘스트 완료 후	
	{
		Debug.Log("OnEventCompleted");
		if (csMainQuest.TransformationRestored)	// 변신해제.
		{
			if (m_csPlayer.IsTransformationStateMonster())
			{
				m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
			}
		}

		if (CsConfiguration.Instance.ServerType == CsConfiguration.EnServerType.BalancesStage)
		{
			Debug.Log("OnEventCompleted : " + CsConfiguration.Instance.ServerType + " , " + csMainQuest.MainQuestNo);
			if (csMainQuest.MainQuestNo == 1)
			{
				StartCoroutine(FlightDirect(new Vector3(148.66f, 55f, 226.20f), 90f, new Vector3(164.9683f, 33.83142f, 270.8357f), true));
			}
			else if (csMainQuest.MainQuestNo == 14)
			{
				StartCoroutine(FlightDirect(m_csPlayer.transform.position, m_csPlayer.transform.eulerAngles.y, new Vector3(353f, 47f, 205f)));
			}
			else if (csMainQuest.MainQuestNo == 15)
			{
				StartCoroutine(MainQuestClearCameraMoveDirect());
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void Test(int nNum)
	{
		if (nNum == 1)
		{
			StartCoroutine(FlightDirect(new Vector3(148.66f, 55f, 226.20f), 90f, new Vector3(164.9683f, 33.83142f, 270.8357f), true));
		}
		else if (nNum == 2)
		{
			StartCoroutine(FlightDirect(m_csPlayer.transform.position, m_csPlayer.transform.eulerAngles.y, new Vector3(344f, 46.8f, 203f)));
		}
		else if (nNum == 3)
		{
			StartCoroutine(MainQuestClearCameraMoveDirect());
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestMonsterTransformationCanceled(long[] alRemovedAbnormalStateEffects)
	{
		m_csPlayer.NetEventTransformationMonsterFinish(m_csPlayer.MaxHp, m_csPlayer.Hp, alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestMonsterTransformationFinished(long[] alRemovedAbnormalStateEffects)
	{
		m_csPlayer.NetEventTransformationMonsterFinish(m_csPlayer.MaxHp, m_csPlayer.Hp, alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMainQuestMonsterTransformationStarted(Guid guidHeroId, int nTransformationMonsterId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonster(nTransformationMonsterId, nMaxHp, nHp, alRemovedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMainQuestMonsterTransformationCanceled(Guid guidHeroId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonsterFinish(nMaxHp, nHp, alRemovedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMainQuestMonsterTransformationFinished(Guid guidHeroId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonsterFinish(nMaxHp, nHp, alRemovedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMainQuestTransformationMonsterSkillCast(Guid guidHeroId, int nSkillId, Vector3 vtTargetPos)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonsterSkillCast(nSkillId, vtTargetPos);
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected IEnumerator FlightDirect(Vector3 vtStartPos, float flRotionY, Vector3 vtEndPos, bool bInteraction = false)
	{
		Debug.Log(" DungeonClearDirect()   Directing = " + CsIngameData.Instance.Directing);
		CsIngameData.Instance.Directing = true;
		GameObject goLiveNav = null;

		if (bInteraction)
		{
			goLiveNav = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/LiveNavmesh"), transform);
		}

		CsGameEventToUI.Instance.OnEventJoystickReset();
		m_csPlayer.SkillStatus.Reset();
		m_csPlayer.MyHeroNavMeshAgent.enabled = false;
		m_csPlayer.transform.position = vtStartPos;
		m_csPlayer.ChangeEulerAngles(flRotionY);
		CsGameEventToUI.Instance.OnEventFade(false);
		CsGameEventToUI.Instance.OnEventHideMainUI(true);
		CsIngameData.Instance.InGameCamera.SetFlightCamera(EnCameraMode.CameraAuto);
		yield return new WaitForEndOfFrame();

		m_csPlayer.MyHeroNavMeshAgent.enabled = true;
		yield return new WaitForSeconds(0.1f);

		if (bInteraction)
		{
			m_csPlayer.ChangeState(CsHero.EnState.Interaction);
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.None, m_csPlayer.transform, m_csPlayer.transform.position, "Flight_Direction", 1.5f);
			yield return new WaitForSeconds(1.5f);
		}

		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.Flight);
		yield return new WaitUntil(() => m_csPlayer.IsTransformationStateFlight());
		yield return new WaitForSeconds(0.2f);

		m_csPlayer.MoveToPos(vtEndPos, 0.5f, false);
		yield return new WaitForSeconds(1.5f);

		m_csPlayer.FlightAccelerate = true;
		if (bInteraction)
		{
			yield return new WaitForSeconds(2f);
			yield return new WaitUntil(() => m_csPlayer.MyHeroNavMeshAgent.baseOffset < 9f);
			m_csPlayer.FlightAccelerate = false;
		}

		yield return new WaitUntil(() => m_csPlayer.IsTransformationStateNone());
		m_csPlayer.FlightAccelerate = false;
		m_csPlayer.DirectingEnd();

		if (goLiveNav != null)
		{
			m_csPlayer.MyHeroNavMeshAgent.enabled = false;
			Destroy(goLiveNav);
			yield return new WaitForEndOfFrame();
			m_csPlayer.MyHeroNavMeshAgent.enabled = true;
		}

		yield return new WaitForSeconds(1.5f);

		CsIngameData.Instance.IngameManagement.DirectingEnd(true);
		CsIngameData.Instance.InGameCamera.ResetCamera();
		yield return new WaitForSeconds(0.5f);
		CsGameEventToUI.Instance.OnEventFade(false);
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator MainQuestClearCameraMoveDirect()
	{
		CsIngameData.Instance.Directing = true;

		if (m_csPlayer != null)
		{
			m_csPlayer.MyHeroView(false);
		}

		CsGameEventToUI.Instance.OnEventFade(false);
		CsGameEventToUI.Instance.OnEventHideMainUI(true);
		CsIngameData.Instance.InGameCamera.CameraCullDistance(300);
		GameObject goCameraMoveDirect = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Cinema/CineScene_MainQuest_Salvador"), transform) as GameObject;
		PlayableDirector playableCameraMoveDirect = goCameraMoveDirect.GetComponent<PlayableDirector>();
		playableCameraMoveDirect.Play();
		yield return new WaitForSeconds(1f);
		yield return new WaitUntil(() => playableCameraMoveDirect.state != PlayState.Playing);  // 연출이 끝나면

		CsIngameData.Instance.InGameCamera.ResetFarClipPlane();
		CsIngameData.Instance.IngameManagement.DirectingEnd(true);
		CsIngameData.Instance.InGameCamera.CameraCullDistance(30);
		GameObject.Destroy(goCameraMoveDirect);
		CsGameEventToUI.Instance.OnEventFade(false);
	}

	#endregion mainQeust

	#region MysteryBox

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMysteryBoxPickStarted(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventMysteryBoxPickStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMysteryBoxPickCompleted(Guid guidHeroId, int nGrade) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventMysteryBoxPick(nGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMysteryBoxPickCanceled(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventMysteryBoxPickCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroMysteryBoxQuestCompleted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventMysteryBoxPick(0);
		}
	}

	#endregion MysteryBox

	#region SecretLetter

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroSecretLetterPickStarted(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventSecretLetterPickStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroSecretLetterPickCompleted(Guid guidHeroId, int nGrade) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventSecretLetterPick(nGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroSecretLetterPickCanceled(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventSecretLetterPickCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroSecretLetterQuestCompleted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventSecretLetterPick(0);
		}
	}

	#endregion SecretLetter

	#region DimensionRaid

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDimensionRaidInteractionStarted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventDimensionRaidInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDimensionRaidInteractionCanceled(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventDimensionRaidInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDimensionRaidInteractionCompleted(Guid guidHeroId) // 당사자외
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventDimensionRaidInteractionCancel();
		}
	}

	#endregion DimensionRaid

	#region SupplySupportQuest
	//----------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestAccept(PDSupplySupportQuestCartInstance pDCartInstance)
	{
		Debug.Log("OnEventSupplySupportQuestAccept");
		CsGameData.Instance.MyHeroInfo.CartInstance = pDCartInstance;
		StartCoroutine(CreateCart(pDCartInstance));
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestCartChange(int nOldCartId, int nNewCartId)
	{
		Debug.Log("OnEventSupplySupportQuestCartChange()");
		CsGameData.Instance.MyHeroInfo.CartInstance.cartId = nNewCartId;
		ChangeCart(CsGameData.Instance.MyHeroInfo.CartInstance);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestFail()
	{
		Debug.Log("OnEventSupplySupportQuestFail()");
		m_csPlayer.SetAutoPlay(null, true);
		CsCartManager.Instance.RemoveMyCart();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp, long lGold, int nAcquiredExploitPoint)
	{
		Debug.Log("OnEventSupplySupportQuestComplete()");
		m_csPlayer.SetAutoPlay(null, true);
		CsCartManager.Instance.RemoveMyCart();
	}

	#endregion SupplySupportQuest

	#region GuildMission
	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateMissionState()
	{
		CreateGuildMissionArea();
	}

	#endregion GuildMission

	#region GuildSupplySupport
	
	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestAccept(PDGuildSupplySupportQuestCartInstance pDGuildSupplySupportQuestCartInstance)
	{
		Debug.Log(" OnEventGuildSupplySupportQuestAccept ()");
		CsGameData.Instance.MyHeroInfo.CartInstance = pDGuildSupplySupportQuestCartInstance;
		StartCoroutine(CreateCart(pDGuildSupplySupportQuestCartInstance));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		Debug.Log(" OnEventGuildSupplySupportQuestComplete ()");
		m_csPlayer.SetAutoPlay(null, true);
		CsCartManager.Instance.RemoveMyCart();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestFail()
	{
		Debug.Log(" OnEventGuildSupplySupportQuestFail ()");
		m_csPlayer.SetAutoPlay(null, true);
		CsCartManager.Instance.RemoveMyCart();
	}

	#endregion GuildSupplySupport

	#region NationWar
	
    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarJoin(int nTargetContinentId)
    {
        if (m_csPlayer != null)
        {
            m_csPlayer.SetAutoPlay(null, true);            
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentEnterForNationWarJoin(PDContinentEntranceInfo pDContinentEntranceInfo)
	{
		SetEnter(pDContinentEntranceInfo, false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventNationWarTransmission(int nTargetContinentId)
    {
        if (m_csPlayer != null)
        {
            m_csPlayer.SetAutoPlay(null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentEnterForNationWarTransmission(PDContinentEntranceInfo pDContinentEntranceInfo)
    {
		SetEnter(pDContinentEntranceInfo, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarNpcTransmission(int nTargetContinentId)
    {
        if (m_csPlayer != null)
        {
            m_csPlayer.SetAutoPlay(null, true);            
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentEnterForNationWarNpcTransmission(PDContinentEntranceInfo pDContinentEntranceInfo)
    {
		SetEnter(pDContinentEntranceInfo, false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventNationWarRevive(int nTargetContinentId)
    {
        if (m_csPlayer != null)
        {
            m_csPlayer.SetAutoPlay(null, true);            
			m_csPlayer.NetEventRevive();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentEnterForNationWarRevive(PDContinentEntranceInfo pDContinentEntranceInfo)
    {
		SetEnter(pDContinentEntranceInfo, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarCallTransmission(int nTargetContinentId)
    {
        if (m_csPlayer != null)
        {
            m_csPlayer.SetAutoPlay(null, true);            
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentEnterForNationWarCallTransmission(PDContinentEntranceInfo pDContinentEntranceInfo)
    {
		SetEnter(pDContinentEntranceInfo, false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventNationWarStart(Guid guidDeclarationId)
    {
		CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.MyNationWarDeclaration;
		if (csNationWarDeclaration != null && csNationWarDeclaration.DeclarationId == guidDeclarationId)	// 당사자 국가전일때.
		{
			m_csPlayer.NetEventNationWarChangeState();
		}

		foreach (var dicHero in m_dicHeros)
		{
			dicHero.Value.NetEventNationWarChangeState();
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarFinished(Guid guidDeclarationId, int nWinNationId)
    {
		CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.MyNationWarDeclaration;
		if (csNationWarDeclaration != null && csNationWarDeclaration.DeclarationId == guidDeclarationId)	// 당사자 국가전 종료일때.
		{
			m_csPlayer.NetEventNationWarChangeState();
		}

		foreach (var dicHero in m_dicHeros)
		{
			dicHero.Value.NetEventNationWarChangeState();
		}

		for (int i = 0; i < CsGameData.Instance.NationWar.NationWarNpcList.Count; i++)
        {
            RemoveNpc(CsGameData.Instance.NationWar.NationWarNpcList[i].NpcId);
        }
        
        for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
		{
			RemoveMonster(CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].MonsterArrangeId);
		}
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventNationWarMonsterDead(int nArrangeId)
	{
		for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
		{
			if (nArrangeId == CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].ArrangeId)
			{
				if (CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].Type == 5)  // 1:총사령관,2:위저드,3:엔젤,4:드래곤,5:암석
				{
					CsNationWarManager.Instance.AppearNpc = true;
					StartCoroutine(AsyncCreateNationWarNPC(CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].NationWarNpc));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventNationWarMonsterSpawn(int nArrangeId, int nNationId)
	{
		if (CsNationWarManager.Instance.AppearNpc && nArrangeId == 7)
		{
			CsNationWarManager.Instance.AppearNpc = false;
			RemoveNpc(CsGameData.Instance.NationWar.GetNationWarMonsterArrange(nArrangeId).NationWarNpc.NpcId);
		}
	}

	#endregion NationWar

	#region WeeklyQuest

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundAccept()
	{
		CreateWeeklyQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lExp)
	{
		CreateWeeklyQuestArea();
	}

	#endregion WeeklyQuest

	#region TrueHeroQuest

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestAccept()
	{
		CreateTrueHeroQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepCompleted()
	{
		CreateTrueHeroQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		CreateTrueHeroQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepInteractionFinished()
	{
		CreateTrueHeroQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestStepWaitingCanceled()
	{
		CreateTrueHeroQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroTrueHeroQuestStepInteractionStarted(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroTrueHeroQuestStepInteractionCanceled(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroTrueHeroQuestStepInteractionFinished(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionFinished();
		}
	}

	#endregion TrueHeroQuest

	#region BiographyQuest
	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestAccept(int nBiographyId)
	{
		CreateBiographyQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestComplete(bool bLevelUp, long lAcquiredExp, int nBiographyId)
	{
		CreateBiographyQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestMoveObjectiveComplete(int nBiographyId)
	{
		CreateBiographyQuestArea();
	}

	#endregion BiographyQuest

	#region CreatureFarmQuest

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestAccept()
	{
		CreateCreatureFarmQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestComplete(bool bLevelUp, long lExp)
	{
		CreateCreatureFarmQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionMoveObjectiveComplete(bool bLevelUp, long lExp)
	{
		CreateCreatureFarmQuestArea();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionCompleted(bool bLevelUp, long lExp)
	{
		CreateCreatureFarmQuestArea();
	}

	#endregion CreatureFarmQuest

	#region JobChangeQuest

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestMonsterSpawned()
	{
		if (CsJobChangeManager.Instance.HeroJobChangeQuest != null)
		{
			//CsJobChangeManager.Instance.HeroJobChangeQuest.MonsterInstanceId
			//CsJobChangeManager.Instance.HeroJobChangeQuest.MonsterPosition
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestFailed()
	{
		if (CsJobChangeManager.Instance.HeroJobChangeQuest != null)
		{
			RemoveMonster(CsJobChangeManager.Instance.HeroJobChangeQuest.MonsterInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroJobChange()
	{
		Debug.Log("OnEventHeroJobChange");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroJobChanged(Guid guidHeroId, int nJobId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			//m_dicHeros[guidHeroId]. 직업 변경 처리.
		}
	}

	#endregion JobChangeQuest

	#region ObjectManagement

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator CreateContinentPotal(int nContinentId)
	{
		Transform trPortal = transform.Find("Portal");
		ResourceRequest resourceRequest = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Portal");
		yield return resourceRequest;

		for (int i = 0; i < CsGameData.Instance.PortalList.Count; i++)
		{
			if (CsGameData.Instance.PortalList[i].ContinentId == nContinentId)
			{
				GameObject goPortal = Instantiate(resourceRequest.asset, trPortal) as GameObject;
				CsPortalArea csPortalArea = goPortal.GetComponent<CsPortalArea>();
				csPortalArea.Init(CsGameData.Instance.PortalList[i]);
				m_dicPortal.Add(CsGameData.Instance.PortalList[i].PortalId, csPortalArea);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateGuildMissionArea()	// 길드미션 퀘스트
	{
		RemoveGuildMissionArea();

		CsGuildManager csGuildManager = CsGuildManager.Instance;

		if (csGuildManager.GuildMission == null) return;
		if (csGuildManager.GuildMission.TargetContinent == null) return;

		if (csGuildManager.GuildMission.TargetContinent.ContinentId == m_csContinent.ContinentId)
		{
			if ((EnMissionType)csGuildManager.GuildMission.Type == EnMissionType.GuildSpirit)
			{
				if (csGuildManager.GuildMissionState == EnGuildMissionState.Accepted)
				{
					Debug.Log("#####################                    CreateGuildMissionArea()              ############################# ");
					GameObject goGuildMissionArea = new GameObject();
					goGuildMissionArea.transform.parent = transform.Find("Area");
					goGuildMissionArea.name = "GuildMissionArea";
					goGuildMissionArea.AddComponent<SphereCollider>();
					goGuildMissionArea.AddComponent<CsQuestArea>();

					m_csGuildMissionArea = goGuildMissionArea.AddComponent<CsQuestArea>();
					m_csGuildMissionArea.Init(csGuildManager.GuildMission);

					foreach (var dicInteractionObject in m_dicInteractionObject)
					{
						if (dicInteractionObject.Value.ObjectId == csGuildManager.GuildMission.ContinentObjectTarget.ObjectId)
						{
							dicInteractionObject.Value.InteractionRegeneration();
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateSceneryQuestArea()   // 풍광 퀘스트
	{
		if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam || CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == 0)
		{
			for (int i = 0; i < CsGameData.Instance.SceneryQuestList.Count; i++)
			{
				CsSceneryQuest csSceneryQuest = CsGameData.Instance.SceneryQuestList[i];
				if (csSceneryQuest.Continent.ContinentId == m_csContinent.ContinentId) // 같은 대륙일때.
				{
					bool bComplete = false;
					for (int j = 0; j < CsIllustratedBookManager.Instance.CompletedSceneryQuestList.Count; j++)
					{
						if (CsIllustratedBookManager.Instance.CompletedSceneryQuestList[j] == csSceneryQuest.QuestId)
						{
							bComplete = true;
							break;
						}
					}

					if (bComplete) continue;

					GameObject goSceneryArea = new GameObject();
					goSceneryArea.transform.parent = transform.Find("Area");
					goSceneryArea.name = "SceneryArea";
					goSceneryArea.AddComponent<SphereCollider>();
					goSceneryArea.AddComponent<CsQuestArea>();
					goSceneryArea.GetComponent<CsQuestArea>().Init(csSceneryQuest);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateBiographyQuestArea()  // 전기 퀘스트
	{
		RemoveBiographyQuestArea();

		for (int i = 0; i < CsBiographyManager.Instance.HeroBiographyList.Count; i++)
		{
			if (CsBiographyManager.Instance.HeroBiographyList[i].HeroBiograhyQuest == null) continue;   // 진행중 전기 퀘스트 없음.

			CsBiographyQuest csBiographyQuest = CsBiographyManager.Instance.HeroBiographyList[i].HeroBiograhyQuest.BiographyQuest;

			if (csBiographyQuest.Type == 1)                                                     // 이동 타입일때.
			{
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)                                     // 같은 국가일때.
				{
					if (csBiographyQuest.TargetContinent.ContinentId == m_csContinent.ContinentId)  // 같은 대륙일때.
					{
						GameObject goBiographyQuestArea = new GameObject();
						goBiographyQuestArea.transform.parent = transform.Find("Area");
						goBiographyQuestArea.name = "BiographyQuestArea";
						goBiographyQuestArea.AddComponent<SphereCollider>();
						goBiographyQuestArea.AddComponent<CsQuestArea>();

						m_csBiographyQuestArea = goBiographyQuestArea.GetComponent<CsQuestArea>();
						m_csBiographyQuestArea.Init(csBiographyQuest);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateTrueHeroQuestArea()   // 진정한영웅 퀘스트
	{
		RemoveTrueHeroQuestArea();

		CsTrueHeroQuest csTrueHeroQuest = CsTrueHeroQuestManager.Instance.TrueHeroQuest;
		CsTrueHeroQuestStep csTrueHeroQuestStep = CsTrueHeroQuestManager.Instance.TrueHeroQuestStep;

		if (csTrueHeroQuest == null || csTrueHeroQuestStep == null) return;
		if (CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
		{
			if (csTrueHeroQuestStep.TargetContinent.ContinentId == m_csContinent.ContinentId)
			{
				GameObject goTrueHeroQuestArea = new GameObject();
				goTrueHeroQuestArea.transform.parent = transform.Find("Area");
				goTrueHeroQuestArea.name = "TrueHeroQuestArea";
				goTrueHeroQuestArea.AddComponent<SphereCollider>();
				goTrueHeroQuestArea.AddComponent<CsQuestArea>();

				m_csTrueHeroQuestArea = goTrueHeroQuestArea.GetComponent<CsQuestArea>();
				m_csTrueHeroQuestArea.Init(csTrueHeroQuestStep.TargetObjectPosition, csTrueHeroQuest.TargetObjectInteractionMaxRange);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateWeeklyQuestArea()    // 주간 퀘스트
	{
		RemoveWeeklyQuestArea();

		if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest == null) return;
		if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission == null) return;
		if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.Type == 1)         // 이동 퀘스트인경우.
		{
			if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
			{
				if (CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission.TargetContinent.LocationId == m_csContinent.ContinentId)
				{
					GameObject goWeeklyQuestArea = new GameObject();
					goWeeklyQuestArea.transform.parent = transform.Find("Area");
					goWeeklyQuestArea.name = "WeeklyQuestArea";
					goWeeklyQuestArea.AddComponent<SphereCollider>();
					goWeeklyQuestArea.AddComponent<CsQuestArea>();

					m_csWeeklyQuestArea = goWeeklyQuestArea.GetComponent<CsQuestArea>();
					m_csWeeklyQuestArea.Init(CsWeeklyQuestManager.Instance.HeroWeeklyQuest.WeeklyQuestMission);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateCreatureFarmQuestArea()  // 크리처농장퀘스트
	{
		RemoveCreatureFarmQuestArea();

		if (CsCreatureFarmQuestManager.Instance.CreatureFarmQuestState == EnCreatureFarmQuestState.Accepted)
		{
			if (CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest == null) return;
			if (CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission == null) return;

			if (CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 1)				// 1.  이동 퀘스트인경우.
			{
				if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)	// 2. 본인 국가인 경우.
				{
					CsCreatureFarmQuestMission csCreatureFarmQuestMission = CsCreatureFarmQuestManager.Instance.HeroCreatureFarmQuest.CreatureFarmQuestMission;

					if (csCreatureFarmQuestMission.ContinentTarget.ContinentId == m_csContinent.ContinentId)
					{
						GameObject goCreatureFarmQuestArea = new GameObject();
						goCreatureFarmQuestArea.transform.parent = transform.Find("Area");
						goCreatureFarmQuestArea.name = "CreatureFarmQuestArea";
						goCreatureFarmQuestArea.AddComponent<SphereCollider>();
						goCreatureFarmQuestArea.AddComponent<CsQuestArea>();

						m_csFarmQuestArea = goCreatureFarmQuestArea.GetComponent<CsQuestArea>();
						m_csFarmQuestArea.Init(csCreatureFarmQuestMission);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveGuildMissionArea()
	{
		if (m_csGuildMissionArea != null)
		{
			Debug.Log("#####################                    RemoveGuildMissionArea()             ############################# ");
			GameObject.Destroy(m_csGuildMissionArea.gameObject);
			m_csGuildMissionArea = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveBiographyQuestArea()
	{
		if (m_csBiographyQuestArea != null)
		{
			Debug.Log(" RemoveBiographyQuestArea()");
			Destroy(m_csBiographyQuestArea.gameObject);
			m_csBiographyQuestArea = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveTrueHeroQuestArea()
	{
		if (m_csTrueHeroQuestArea != null)
		{
			Destroy(m_csTrueHeroQuestArea.gameObject);
			m_csTrueHeroQuestArea = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveWeeklyQuestArea()
	{
		if (m_csWeeklyQuestArea != null)
		{
			Destroy(m_csWeeklyQuestArea.gameObject);
			m_csWeeklyQuestArea = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveCreatureFarmQuestArea()
	{
		if (m_csFarmQuestArea != null)
		{
			Destroy(m_csFarmQuestArea.gameObject);
			m_csFarmQuestArea = null;
		}
	}

	#endregion ObjectManagement
}