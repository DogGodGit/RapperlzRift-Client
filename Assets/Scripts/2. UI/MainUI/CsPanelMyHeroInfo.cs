using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-21)
//---------------------------------------------------------------------------------------------------

public class CsPanelMyHeroInfo : MonoBehaviour
{
    Text m_textLevel;
    Text m_textBattlePower;
    Text m_textSliderValue;
    Text m_textExp;
    Text m_textTime;
    Slider m_sliderHp;
    Slider m_sliderExp;
	Slider m_sliderBattery;
	Transform m_trNetwork;
	GameObject m_goNetwork3G;
	GameObject m_goNetworkLTE;
	GameObject m_goNetworkWIFI1;
	GameObject m_goNetworkWIFI2;
	GameObject m_goNetworkWIFI3;
	GameObject m_goBatteryFull;
	GameObject m_goCharge;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventMainGearEquip += OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip += OnEventMainGearUnequip;
        CsGameEventUIToUI.Instance.EventMainGearEnchant += OnEventMainGearEnchant;
        CsGameEventUIToUI.Instance.EventMainGearTransit += OnEventMainGearTransit;
        CsGameEventUIToUI.Instance.EventMainGearRefinementApply += OnEventMainGearRefinementApply;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate += OnEventMainGearEnchantLevelSetActivate;

        CsGameEventUIToUI.Instance.EventSubGearEquip += OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip += OnEventSubGearUnequip;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount += OnEventSoulstoneSocketMount;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketUnmount += OnEventSoulstoneSocketUnmount;
        CsGameEventUIToUI.Instance.EventRuneSocketMount += OnEventRuneSocketMount;
        CsGameEventUIToUI.Instance.EventRuneSocketUnmount += OnEventRuneSocketUnmount;
        CsGameEventUIToUI.Instance.EventSubGearLevelUp += OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally += OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp += OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp += OnEventSubGearQualityUp;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose += OnEventMountedSoulstoneCompose;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate += OnEventSubGearSoulstoneLevelSetActivate;

        CsGameEventUIToUI.Instance.EventSkillLevelUp += OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally += OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventRestRewardReceiveFree += OnEventRestRewardReceiveFree;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold += OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia += OnEventRestRewardReceiveDia;

        CsGameEventUIToUI.Instance.EventHpPotionUse += OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventExpPotionUse += OnEventExpPotionUse;

        CsGameEventUIToUI.Instance.EventImmediateRevive += OnEventImmediateRevive;

        CsGameEventUIToUI.Instance.EventExpAcquisition += OnEventExpAcquisition;

        CsGameEventUIToUI.Instance.EventMountEquip += OnEventMountEquip;
        CsGameEventUIToUI.Instance.EventMountLevelUp += OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountGearRefine += OnEventMountGearRefine;
        CsGameEventUIToUI.Instance.EventMountGearEquip += OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip += OnEventMountGearUnequip;

        CsGameEventUIToUI.Instance.EventWingEquip += OnEventWingEquip;
        CsGameEventUIToUI.Instance.EventWingEnchant += OnEventWingEnchant;
        CsGameEventUIToUI.Instance.EventWingEnchantTotally += OnEventWingEnchantTotally;

        CsGameEventUIToUI.Instance.EventRankAcquire += OnEventRankAcquire;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed += OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismissed += OnEventNationNoblesseDismissed;

		CsGameEventUIToUI.Instance.EventNetworkStatus += OnEventNetworkStatus;
		CsGameEventUIToUI.Instance.EventBatteryStatus += OnEventBatteryStatus;

		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp += OnEventRankPassiveSkillLevelUp;

		CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall += OnEventWingMemoryPieceInstall;

		CsGameEventUIToUI.Instance.EventWingItemUse += OnEventWingItemUse;
		CsGameEventUIToUI.Instance.EventHeroAttrPotionUse += OnEventHeroAttrPotionUse;
		CsGameEventUIToUI.Instance.EventHeroAttrPotionUseAll += OnEventHeroAttrPotionUseAll;

		CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp += OnEventMountAwakeningLevelUp;
		CsGameEventUIToUI.Instance.EventMountAttrPotionUse += OnEventMountAttrPotionUse;
		CsGameEventUIToUI.Instance.EventMountItemUse += OnEventMountItemUse;

		///////
		CsMainQuestManager.Instance.EventAccepted += OnEventMainQuestAccepted;
		CsMainQuestManager.Instance.EventCompleted += OnEventMainQuestCompleted;

        ///////
        CsGameEventToUI.Instance.EventMyHeroInfoUpdate += OnEventMyHeroInfoUpdate;

        //사운드 용
        CsGameEventUIToUI.Instance.EventGoldItemUse += OnEventGoldItemUse;                                  // 골드아이템 사용
        CsDungeonManager.Instance.EventGoldDungeonStepCompleted += OnEventGoldDungeonStepCompleted;         // 골드 던전 스탭 클리어
        CsDungeonManager.Instance.EventGoldDungeonClear += OnEventGoldDungeonClear;                         // 골드 던전 클리어
        CsGameEventUIToUI.Instance.EventRankRewardReceive += OnEventRankRewardReceive;                      // 계급 보상
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepRewardReceive += OnEventIllustratedBookExplorationStepRewardReceive; //도감 보상

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive += OnEventMainQuestDungeonSaftyRevive;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted += OnEventMainQuestDungeonStepCompleted;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;

        //던전
        CsDungeonManager.Instance.EventAncientRelicBanished += OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicExit += OnEventAncientRelicExit;

        CsDungeonManager.Instance.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit += OnEventArtifactRoomExit;

        CsDungeonManager.Instance.EventExpDungeonBanished += OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventExpDungeonExit += OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonClear += OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonSweep += OnEventExpDungeonSweep;

        CsDungeonManager.Instance.EventGoldDungeonBanished += OnEventGoldDungeonBanished;
        CsDungeonManager.Instance.EventGoldDungeonExit += OnEventGoldDungeonExit;

        CsDungeonManager.Instance.EventStoryDungeonBanished += OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonExit += OnEventStoryDungeonExit;

        CsDungeonManager.Instance.EventFieldOfHonorClear += OnEventFieldOfHonorClear;
        CsDungeonManager.Instance.EventFieldOfHonorFail += OnEventFieldOfHonorFail;
        CsDungeonManager.Instance.EventFieldOfHonorExit += OnEventFieldOfHonorExit;
        CsDungeonManager.Instance.EventFieldOfHonorBanished += OnEventFieldOfHonorBanished;

        CsDungeonManager.Instance.EventUndergroundMazeBanished += OnEventUndergroundMazeBanished;

        CsDungeonManager.Instance.EventSoulCoveterExit += OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterAbandon += OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished += OnEventSoulCoveterBanished;

        CsDungeonManager.Instance.EventEliteDungeonExit += OnEventEliteDungeonExit;
        CsDungeonManager.Instance.EventEliteDungeonAbandon += OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished += OnEventEliteDungeonBanished;

        CsDungeonManager.Instance.EventStoryDungeonRevive += OnEventStoryDungeonRevive;
        CsDungeonManager.Instance.EventGoldDungeonRevive += OnEventGoldDungeonRevive;
        CsDungeonManager.Instance.EventExpDungeonRevive += OnEventExpDungeonRevive;
        CsDungeonManager.Instance.EventAncientRelicRevive += OnEventAncientRelicRevive;
        CsDungeonManager.Instance.EventSoulCoveterRevive += OnEventSoulCoveterRevive;
        CsDungeonManager.Instance.EventEliteDungeonRevive += OnEventEliteDungeonRevive;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForUndergroundMazeRevive += OnEventUndergroundMazeEnterForUndergroundMazeRevive;
        
        CsDungeonManager.Instance.EventProofOfValorSweep += OnEventProofOfValorSweep;
        CsDungeonManager.Instance.EventProofOfValorBanished += OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorClear += OnEventProofOfValorClear;
        CsDungeonManager.Instance.EventProofOfValorFail += OnEventProofOfValorFail;
        CsDungeonManager.Instance.EventProofOfValorAbandon += OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBuffBoxAcquire += OnEventProofOfValorBuffBoxAcquire;

		CsDungeonManager.Instance.EventWisdomTempleSweep += OnEventWisdomTempleSweep;
		CsDungeonManager.Instance.EventWisdomTempleExit += OnEventWisdomTempleExit;
		CsDungeonManager.Instance.EventWisdomTempleStepCompleted += OnEventWisdomTempleStepCompleted;
		CsDungeonManager.Instance.EventWisdomTemplePuzzleCompleted += OnEventWisdomTemplePuzzleCompleted;

		CsDungeonManager.Instance.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimRevive += OnEventRuinsReclaimRevive;

        CsDungeonManager.Instance.EventInfiniteWarExit += OnEventInfiniteWarExit;
        CsDungeonManager.Instance.EventInfiniteWarAbandon += OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarRevive += OnEventInfiniteWarRevive;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxAcquire += OnEventInfiniteWarBuffBoxAcquire;

		CsDungeonManager.Instance.EventFearAltarExit += OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon += OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;
		CsDungeonManager.Instance.EventFearAltarRevive += OnEventFearAltarRevive;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryExit += OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished += OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryRevive += OnEventWarMemoryRevive;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestExit += OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestAbandon += OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished += OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestRevive += OnEventDragonNestRevive;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipExit += OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon += OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished += OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipRevive += OnEventTradeShipRevive;

        // 앙쿠의 무덤
		CsDungeonManager.Instance.EventAnkouTombExit += OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished += OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombRevive += OnEventAnkouTombRevive;

        // 현상금사냥꾼 퀘스트
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete += OnEventBountyHunterQuestComplete;

        // 의문의상자퀘스트완료
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete += OnEventMysteryBoxQuestComplete;

        // 밀서퀘스트완료
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete += OnEventSecretLetterQuestComplete;

        // 차원습격퀘스트완료
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete += OnEventDimensionRaidQuestComplete;

        // 낚시 캐스팅 완료
        CsFishingQuestManager.Instance.EventFishingCastingCompleted += OnEventFishingCastingCompleted;

        //농장의 위협
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete += OnEventMissionComplete;

        // 위대한성전퀘스트완료
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete += OnEventHolyWarQuestComplete;

        // 세리우 보급지원
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail += OnEventSupplySupportQuestFail;

        // 길드미션완료
        CsGuildManager.Instance.EventGuildMissionComplete += OnEventGuildMissionComplete;

        //길드농장
        CsGuildManager.Instance.EventGuildFarmQuestComplete += OnEventGuildFarmQuestComplete;

        // 길드 제단 완료
        CsGuildManager.Instance.EventGuildAltarCompleted += OnEventGuildAltarCompleted;

        // 길드매니저
        CsGuildManager.Instance.EventGuildCreate += OnEventGuildCreate;
        CsGuildManager.Instance.EventGuildExit += OnEventGuildExit;
        CsGuildManager.Instance.EventGuildInvitationAccept += OnEventGuildInvitationAccept;
        CsGuildManager.Instance.EventGuildApplicationAccepted += OnEventGuildApplicationAccepted;
        CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildSkillLevelUp += OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventGuildFoodWarehouseStock += OnEventGuildFoodWarehouseStock;
        CsGuildManager.Instance.EventGuildBuildingLevelUp += OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent += OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestCompleted += OnEventGuildSupplySupportQuestCompleted;

        // 도감
        CsIllustratedBookManager.Instance.EventIllustratedBookUse += OnEventIllustratedBookUse;
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepAcquire += OnEventIllustratedBookExplorationStepAcquire;

        // 칭호
        CsTitleManager.Instance.EventTitleLifetimeEnded += OnEventTitleLifetimeEnded;
        CsTitleManager.Instance.EventTitleItemUse += OnEventTitleItemUse;
        CsTitleManager.Instance.EventActivationTitleSet += OnEventActivationTitleSet;

        // 크리쳐카드
        CsCreatureCardManager.Instance.EventCreatureCardCollectionActivate += OnEventCreatureCardCollectionActivate;

		// 데일리퀘스트 완료
		CsDailyQuestManager.Instance.EventDailyQuestComplete += OnEventDailyQuestComplete;

		// 위클리 퀘스트
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted += OnEventWeeklyQuestRoundCompleted;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete += OnEventWeeklyQuestRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete += OnEventWeeklyQuestTenRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete += OnEventWeeklyQuestRoundMoveMissionComplete;

		// 회수
		CsGameEventUIToUI.Instance.EventRetrieveGold += OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll += OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia += OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll += OnEventRetrieveDiaAll;

		// 할일위탁
		CsGameEventUIToUI.Instance.EventTaskConsignmentComplete += OnEventTaskConsignmentComplete;
		CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete += OnEventTaskConsignmentImmediatelyComplete;

		// 진정한 영웅
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete += OnEventTrueHeroQuestComplete;

		// 서브퀘스트
		CsSubQuestManager.Instance.EventSubQuestComplete += OnEventSubQuestComplete;

		// 시련퀘스트
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotComplete += OnEventOrdealQuestSlotComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestComplete += OnEventOrdealQuestComplete;

		// 전기
		CsBiographyManager.Instance.EventBiographyQuestComplete += OnEventBiographyQuestComplete;

		// 크리처
		CsCreatureManager.Instance.EventCreatureParticipate += OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel += OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer += OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel += OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear += OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureInject += OnEventCreatureInject;
		CsCreatureManager.Instance.EventCreatureInjectionRetrieval += OnEventCreatureInjectionRetrieval;
		CsCreatureManager.Instance.EventCreatureVariation += OnEventCreatureVary;
		CsCreatureManager.Instance.EventCreatureAdditionalAttrSwitch += OnEventCreatureAdditionalAttrSwitch;
		CsCreatureManager.Instance.EventCreatureSkillSlotOpen += OnEventCreatureSkillSlotOpen;
		CsCreatureManager.Instance.EventCreatureCompose += OnEventCreatureCompose;

		// 크리처농장퀘스트
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete += OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete += OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted += OnEventCreatureFarmQuestMissionCompleted;

		// 별자리
		CsConstellationManager.Instance.EventConstellationEntryActivate += OnEventConstellationEntryActivate;

		// 아티팩트
		CsArtifactManager.Instance.EventArtifactOpened += OnEventArtifactOpened;
		CsArtifactManager.Instance.EventArtifactLevelUp += OnEventArtifactLevelUp;

		// 코스튬
		CsCostumeManager.Instance.EventCostumeEnchant += OnEventCostumeEnchant;
		CsCostumeManager.Instance.EventCostumeCollectionShuffle += OnEventCostumeCollectionShuffle;
		CsCostumeManager.Instance.EventCostumeCollectionActivate += OnEventCostumeCollectionActivate;

        // 전직
        CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;

		// 업적
		CsAccomplishmentManager.Instance.EventAccomplishmentLevelUp += OnEventAccomplishmentLevelUp;
	}

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventMainGearEquip -= OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip -= OnEventMainGearUnequip;
        CsGameEventUIToUI.Instance.EventMainGearEnchant -= OnEventMainGearEnchant;
        CsGameEventUIToUI.Instance.EventMainGearTransit -= OnEventMainGearTransit;
        CsGameEventUIToUI.Instance.EventMainGearRefinementApply -= OnEventMainGearRefinementApply;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate -= OnEventMainGearEnchantLevelSetActivate;

        CsGameEventUIToUI.Instance.EventSubGearEquip -= OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip -= OnEventSubGearUnequip;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount -= OnEventSoulstoneSocketMount;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketUnmount -= OnEventSoulstoneSocketUnmount;
        CsGameEventUIToUI.Instance.EventRuneSocketMount -= OnEventRuneSocketMount;
        CsGameEventUIToUI.Instance.EventRuneSocketUnmount -= OnEventRuneSocketUnmount;
        CsGameEventUIToUI.Instance.EventSubGearLevelUp -= OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally -= OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp -= OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp -= OnEventSubGearQualityUp;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose -= OnEventMountedSoulstoneCompose;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate -= OnEventSubGearSoulstoneLevelSetActivate;

        CsGameEventUIToUI.Instance.EventSkillLevelUp -= OnEventSkillLevelUp;
        CsGameEventUIToUI.Instance.EventSkillLevelUpTotally -= OnEventSkillLevelUpTotally;

        CsGameEventUIToUI.Instance.EventRestRewardReceiveFree -= OnEventRestRewardReceiveFree;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold -= OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia -= OnEventRestRewardReceiveDia;

        CsGameEventUIToUI.Instance.EventHpPotionUse -= OnEventHpPotionUse;
        CsGameEventUIToUI.Instance.EventExpPotionUse -= OnEventExpPotionUse;

        CsGameEventUIToUI.Instance.EventImmediateRevive -= OnEventImmediateRevive;

        CsGameEventUIToUI.Instance.EventExpAcquisition -= OnEventExpAcquisition;

        CsGameEventUIToUI.Instance.EventMountEquip -= OnEventMountEquip;
        CsGameEventUIToUI.Instance.EventMountLevelUp -= OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountGearRefine -= OnEventMountGearRefine;
        CsGameEventUIToUI.Instance.EventMountGearEquip -= OnEventMountGearEquip;
        CsGameEventUIToUI.Instance.EventMountGearUnequip -= OnEventMountGearUnequip;

        CsGameEventUIToUI.Instance.EventWingEquip -= OnEventWingEquip;
        CsGameEventUIToUI.Instance.EventWingEnchant -= OnEventWingEnchant;
        CsGameEventUIToUI.Instance.EventWingEnchantTotally -= OnEventWingEnchantTotally;

        CsGameEventUIToUI.Instance.EventRankAcquire -= OnEventRankAcquire;

        CsGameEventUIToUI.Instance.EventNationNoblesseAppointed -= OnEventNationNoblesseAppointed;
        CsGameEventUIToUI.Instance.EventNationNoblesseDismissed -= OnEventNationNoblesseDismissed;

        CsGameEventUIToUI.Instance.EventNetworkStatus -= OnEventNetworkStatus;
        CsGameEventUIToUI.Instance.EventBatteryStatus -= OnEventBatteryStatus;

		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp -= OnEventRankPassiveSkillLevelUp;

		CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall -= OnEventWingMemoryPieceInstall;

		CsGameEventUIToUI.Instance.EventWingItemUse -= OnEventWingItemUse;
		CsGameEventUIToUI.Instance.EventHeroAttrPotionUse -= OnEventHeroAttrPotionUse;
		CsGameEventUIToUI.Instance.EventHeroAttrPotionUseAll -= OnEventHeroAttrPotionUseAll;

		CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp -= OnEventMountAwakeningLevelUp;
		CsGameEventUIToUI.Instance.EventMountAttrPotionUse -= OnEventMountAttrPotionUse;
		CsGameEventUIToUI.Instance.EventMountItemUse -= OnEventMountItemUse;

		///////
		CsMainQuestManager.Instance.EventAccepted -= OnEventMainQuestAccepted;
		CsMainQuestManager.Instance.EventCompleted -= OnEventMainQuestCompleted;

        ///////
        CsGameEventToUI.Instance.EventMyHeroInfoUpdate -= OnEventMyHeroInfoUpdate;

        //사운드 용
        CsGameEventUIToUI.Instance.EventGoldItemUse -= OnEventGoldItemUse;                                  // 골드아이템 사용
        CsDungeonManager.Instance.EventGoldDungeonStepCompleted -= OnEventGoldDungeonStepCompleted;         // 골드 던전 스탭 클리어
        CsDungeonManager.Instance.EventGoldDungeonClear -= OnEventGoldDungeonClear;                         // 골드 던전 클리어
        CsGameEventUIToUI.Instance.EventRankRewardReceive -= OnEventRankRewardReceive;                      // 계급 보상
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepRewardReceive -= OnEventIllustratedBookExplorationStepRewardReceive; //도감 보상

        //메인퀘스트 던전
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonSaftyRevive -= OnEventMainQuestDungeonSaftyRevive;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonStepCompleted -= OnEventMainQuestDungeonStepCompleted;
        CsMainQuestDungeonManager.Instance.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;

        //던전
        CsDungeonManager.Instance.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        CsDungeonManager.Instance.EventAncientRelicExit -= OnEventAncientRelicExit;

        CsDungeonManager.Instance.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
        CsDungeonManager.Instance.EventArtifactRoomExit -= OnEventArtifactRoomExit;

        CsDungeonManager.Instance.EventExpDungeonBanished -= OnEventExpDungeonBanished;
        CsDungeonManager.Instance.EventExpDungeonExit -= OnEventExpDungeonExit;
        CsDungeonManager.Instance.EventExpDungeonClear -= OnEventExpDungeonClear;
        CsDungeonManager.Instance.EventExpDungeonSweep -= OnEventExpDungeonSweep;

        CsDungeonManager.Instance.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;
        CsDungeonManager.Instance.EventGoldDungeonExit -= OnEventGoldDungeonExit;

        CsDungeonManager.Instance.EventStoryDungeonBanished -= OnEventStoryDungeonBanished;
        CsDungeonManager.Instance.EventStoryDungeonExit -= OnEventStoryDungeonExit;

        CsDungeonManager.Instance.EventFieldOfHonorClear -= OnEventFieldOfHonorClear;
        CsDungeonManager.Instance.EventFieldOfHonorFail -= OnEventFieldOfHonorFail;
        CsDungeonManager.Instance.EventFieldOfHonorExit -= OnEventFieldOfHonorExit;
        CsDungeonManager.Instance.EventFieldOfHonorBanished -= OnEventFieldOfHonorBanished;

        CsDungeonManager.Instance.EventUndergroundMazeBanished -= OnEventUndergroundMazeBanished;

        CsDungeonManager.Instance.EventSoulCoveterExit -= OnEventSoulCoveterExit;
        CsDungeonManager.Instance.EventSoulCoveterAbandon -= OnEventSoulCoveterAbandon;
        CsDungeonManager.Instance.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;

        CsDungeonManager.Instance.EventEliteDungeonExit -= OnEventEliteDungeonExit;
        CsDungeonManager.Instance.EventEliteDungeonAbandon -= OnEventEliteDungeonAbandon;
        CsDungeonManager.Instance.EventEliteDungeonBanished -= OnEventEliteDungeonBanished;

        CsDungeonManager.Instance.EventStoryDungeonRevive -= OnEventStoryDungeonRevive;
        CsDungeonManager.Instance.EventGoldDungeonRevive -= OnEventGoldDungeonRevive;
        CsDungeonManager.Instance.EventExpDungeonRevive -= OnEventExpDungeonRevive;
        CsDungeonManager.Instance.EventAncientRelicRevive -= OnEventAncientRelicRevive;
        CsDungeonManager.Instance.EventSoulCoveterRevive -= OnEventSoulCoveterRevive;
        CsDungeonManager.Instance.EventEliteDungeonRevive -= OnEventEliteDungeonRevive;
        CsDungeonManager.Instance.EventUndergroundMazeEnterForUndergroundMazeRevive -= OnEventUndergroundMazeEnterForUndergroundMazeRevive;

        CsDungeonManager.Instance.EventProofOfValorSweep -= OnEventProofOfValorSweep;
        CsDungeonManager.Instance.EventProofOfValorBanished -= OnEventProofOfValorBanished;
        CsDungeonManager.Instance.EventProofOfValorClear -= OnEventProofOfValorClear;
        CsDungeonManager.Instance.EventProofOfValorFail -= OnEventProofOfValorFail;
        CsDungeonManager.Instance.EventProofOfValorAbandon -= OnEventProofOfValorAbandon;
        CsDungeonManager.Instance.EventProofOfValorBuffBoxAcquire -= OnEventProofOfValorBuffBoxAcquire;

		CsDungeonManager.Instance.EventWisdomTempleSweep -= OnEventWisdomTempleSweep;
		CsDungeonManager.Instance.EventWisdomTempleExit -= OnEventWisdomTempleExit;
		CsDungeonManager.Instance.EventWisdomTempleStepCompleted -= OnEventWisdomTempleStepCompleted;
		CsDungeonManager.Instance.EventWisdomTemplePuzzleCompleted -= OnEventWisdomTemplePuzzleCompleted;

		CsDungeonManager.Instance.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;
		CsDungeonManager.Instance.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
		CsDungeonManager.Instance.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
		CsDungeonManager.Instance.EventRuinsReclaimRevive -= OnEventRuinsReclaimRevive;

        CsDungeonManager.Instance.EventInfiniteWarExit -= OnEventInfiniteWarExit;
        CsDungeonManager.Instance.EventInfiniteWarAbandon -= OnEventInfiniteWarAbandon;
        CsDungeonManager.Instance.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
        CsDungeonManager.Instance.EventInfiniteWarRevive -= OnEventInfiniteWarRevive;
        CsDungeonManager.Instance.EventInfiniteWarBuffBoxAcquire -= OnEventInfiniteWarBuffBoxAcquire;

		CsDungeonManager.Instance.EventFearAltarExit -= OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon -= OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;
		CsDungeonManager.Instance.EventFearAltarRevive -= OnEventFearAltarRevive;

        // 전쟁의 기억
        CsDungeonManager.Instance.EventWarMemoryExit -= OnEventWarMemoryExit;
        CsDungeonManager.Instance.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
        CsDungeonManager.Instance.EventWarMemoryBanished -= OnEventWarMemoryBanished;
        CsDungeonManager.Instance.EventWarMemoryRevive -= OnEventWarMemoryRevive;

        // 용의 둥지
        CsDungeonManager.Instance.EventDragonNestExit -= OnEventDragonNestExit;
        CsDungeonManager.Instance.EventDragonNestAbandon -= OnEventDragonNestAbandon;
        CsDungeonManager.Instance.EventDragonNestBanished -= OnEventDragonNestBanished;
        CsDungeonManager.Instance.EventDragonNestRevive -= OnEventDragonNestRevive;

        // 무역선 탈환
        CsDungeonManager.Instance.EventTradeShipExit -= OnEventTradeShipExit;
        CsDungeonManager.Instance.EventTradeShipAbandon -= OnEventTradeShipAbandon;
        CsDungeonManager.Instance.EventTradeShipBanished -= OnEventTradeShipBanished;
        CsDungeonManager.Instance.EventTradeShipRevive -= OnEventTradeShipRevive;

        // 앙쿠의 무덤
        CsDungeonManager.Instance.EventAnkouTombExit -= OnEventAnkouTombExit;
        CsDungeonManager.Instance.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
        CsDungeonManager.Instance.EventAnkouTombBanished -= OnEventAnkouTombBanished;
        CsDungeonManager.Instance.EventAnkouTombRevive -= OnEventAnkouTombRevive;

        // 현상금사냥꾼 퀘스트
        CsBountyHunterQuestManager.Instance.EventBountyHunterQuestComplete -= OnEventBountyHunterQuestComplete;

        // 의문의상자퀘스트완료
        CsMysteryBoxQuestManager.Instance.EventMysteryBoxQuestComplete -= OnEventMysteryBoxQuestComplete;

        // 밀서퀘스트완료
        CsSecretLetterQuestManager.Instance.EventSecretLetterQuestComplete -= OnEventSecretLetterQuestComplete;

        // 차원습격퀘스트완료
        CsDimensionRaidQuestManager.Instance.EventDimensionRaidQuestComplete -= OnEventDimensionRaidQuestComplete;

        // 낚시 캐스팅 완료
        CsFishingQuestManager.Instance.EventFishingCastingCompleted -= OnEventFishingCastingCompleted;

        //농장의 위협
        CsThreatOfFarmQuestManager.Instance.EventMissionComplete -= OnEventMissionComplete;

        // 위대한성전퀘스트완료
        CsHolyWarQuestManager.Instance.EventHolyWarQuestComplete -= OnEventHolyWarQuestComplete;

        // 세리우 보급지원
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;
        CsSupplySupportQuestManager.Instance.EventSupplySupportQuestFail -= OnEventSupplySupportQuestFail;

        // 길드미션완료
        CsGuildManager.Instance.EventGuildMissionComplete -= OnEventGuildMissionComplete;

        //길드농장
        CsGuildManager.Instance.EventGuildFarmQuestComplete -= OnEventGuildFarmQuestComplete;

        // 길드 제단 완료
        CsGuildManager.Instance.EventGuildAltarCompleted -= OnEventGuildAltarCompleted;

        // 길드매니저
        CsGuildManager.Instance.EventGuildCreate -= OnEventGuildCreate;
        CsGuildManager.Instance.EventGuildExit -= OnEventGuildExit;
        CsGuildManager.Instance.EventGuildInvitationAccept -= OnEventGuildInvitationAccept;
        CsGuildManager.Instance.EventGuildApplicationAccepted -= OnEventGuildApplicationAccepted;
        CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
        CsGuildManager.Instance.EventGuildSkillLevelUp -= OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventGuildFoodWarehouseStock -= OnEventGuildFoodWarehouseStock;
        CsGuildManager.Instance.EventGuildBuildingLevelUp -= OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent -= OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildSupplySupportQuestComplete -= OnEventGuildSupplySupportQuestComplete;
        CsGuildManager.Instance.EventGuildSupplySupportQuestCompleted -= OnEventGuildSupplySupportQuestCompleted;

        // 도감
        CsIllustratedBookManager.Instance.EventIllustratedBookUse -= OnEventIllustratedBookUse;
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepAcquire -= OnEventIllustratedBookExplorationStepAcquire;

        // 칭호
        CsTitleManager.Instance.EventTitleLifetimeEnded -= OnEventTitleLifetimeEnded;
        CsTitleManager.Instance.EventTitleItemUse -= OnEventTitleItemUse;
        CsTitleManager.Instance.EventActivationTitleSet -= OnEventActivationTitleSet;

        // 크리쳐카드
        CsCreatureCardManager.Instance.EventCreatureCardCollectionActivate -= OnEventCreatureCardCollectionActivate;

		// 데일리퀘스트 완료
		CsDailyQuestManager.Instance.EventDailyQuestComplete -= OnEventDailyQuestComplete;

		// 위클리 퀘스트
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundCompleted -= OnEventWeeklyQuestRoundCompleted;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundImmediatlyComplete -= OnEventWeeklyQuestRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestTenRoundImmediatlyComplete -= OnEventWeeklyQuestTenRoundImmediatlyComplete;
		CsWeeklyQuestManager.Instance.EventWeeklyQuestRoundMoveMissionComplete -= OnEventWeeklyQuestRoundMoveMissionComplete;

		// 회수
		CsGameEventUIToUI.Instance.EventRetrieveGold -= OnEventRetrieveGold;
		CsGameEventUIToUI.Instance.EventRetrieveGoldAll -= OnEventRetrieveGoldAll;
		CsGameEventUIToUI.Instance.EventRetrieveDia -= OnEventRetrieveDia;
		CsGameEventUIToUI.Instance.EventRetrieveDiaAll -= OnEventRetrieveDiaAll;

		// 할일위탁
		CsGameEventUIToUI.Instance.EventTaskConsignmentComplete -= OnEventTaskConsignmentComplete;
		CsGameEventUIToUI.Instance.EventTaskConsignmentImmediatelyComplete -= OnEventTaskConsignmentImmediatelyComplete;

		// 진정한 영웅
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestComplete -= OnEventTrueHeroQuestComplete;

		// 서브퀘스트
		CsSubQuestManager.Instance.EventSubQuestComplete -= OnEventSubQuestComplete;

		// 시련퀘스트
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotComplete -= OnEventOrdealQuestSlotComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestComplete -= OnEventOrdealQuestComplete;

		// 전기
		CsBiographyManager.Instance.EventBiographyQuestComplete -= OnEventBiographyQuestComplete;

		// 크리처
		CsCreatureManager.Instance.EventCreatureParticipate -= OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel -= OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer -= OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel -= OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear -= OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureInject -= OnEventCreatureInject;
		CsCreatureManager.Instance.EventCreatureInjectionRetrieval -= OnEventCreatureInjectionRetrieval;
		CsCreatureManager.Instance.EventCreatureVariation -= OnEventCreatureVary;
		CsCreatureManager.Instance.EventCreatureAdditionalAttrSwitch -= OnEventCreatureAdditionalAttrSwitch;
		CsCreatureManager.Instance.EventCreatureSkillSlotOpen -= OnEventCreatureSkillSlotOpen;
		CsCreatureManager.Instance.EventCreatureCompose -= OnEventCreatureCompose;

		// 크리처농장퀘스트
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestComplete -= OnEventCreatureFarmQuestComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionMoveObjectiveComplete -= OnEventCreatureFarmQuestMissionMoveObjectiveComplete;
		CsCreatureFarmQuestManager.Instance.EventCreatureFarmQuestMissionCompleted -= OnEventCreatureFarmQuestMissionCompleted;

		// 별자리
		CsConstellationManager.Instance.EventConstellationEntryActivate -= OnEventConstellationEntryActivate;

		// 아티팩트
		CsArtifactManager.Instance.EventArtifactOpened -= OnEventArtifactOpened;
		CsArtifactManager.Instance.EventArtifactLevelUp -= OnEventArtifactLevelUp;

		// 코스튬
		CsCostumeManager.Instance.EventCostumeEnchant -= OnEventCostumeEnchant;
		CsCostumeManager.Instance.EventCostumeCollectionShuffle -= OnEventCostumeCollectionShuffle;
		CsCostumeManager.Instance.EventCostumeCollectionActivate -= OnEventCostumeCollectionActivate;

        // 전직
        CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;

		// 업적
		CsAccomplishmentManager.Instance.EventAccomplishmentLevelUp -= OnEventAccomplishmentLevelUp;
	}

	#region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnEventNetworkStatus(string strNetworkType, string strSignalStrength)
	{
		UpdateNetworkState(strNetworkType, strSignalStrength);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBatteryStatus(string strBatteryStatus, string strChargeType)
	{
		UpdateBatteryState(strBatteryStatus, strChargeType);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRankPassiveSkillLevelUp()
	{
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWingMemoryPieceInstall()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWingItemUse()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroAttrPotionUse()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroAttrPotionUseAll()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMountAwakeningLevelUp()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMountAttrPotionUse()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMountItemUse()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildAltarCompleted(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        if (lAcquiredExp > 0)
        {
            CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        if (lAcquiredExp > 0)
        {
            CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp, long lGold, int nAcquiredExploitPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        //경험치랑 골드 획득 토스트
        if (lGold > 0)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), lGold));
        }

        if (lAcquiredExp > 0)
        {
			CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSupplySupportQuestFail()
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonStepCompleted(bool bLevelUp, long lGold, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        //경험치랑 골드 획득 토스트
        if (lGold > 0)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_GOLD"), lGold));
        }

        if (lAcquiredExp > 0)
        {
            CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
        }
    }

    //정예 던전
    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonExit(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonAbandon(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonBanished(int nContinentId)
    {
        UpdateHp();
    }

    // 영혼을 탐하는 자
    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterExit(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterAbandon(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterBanished(int nContinentId)
    {
        UpdateHp();
    }

    //검투 대회
    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorClear(bool bLevelUp, long lAcquiredExp, int nHonorPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorFail(bool bLevelUp, long lAcquiredExp, int nHonorPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorExit(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorBanished(int nContinentId)
    {
        UpdateHp();
    }

    //고대인의 유적
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicExit(int nContinentId)
    {
        UpdateHp();
    }

    //고대 유물의 방
    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomExit(int nContinentId)
    {
        UpdateHp();
    }

    //경험치던전
    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonExit(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonSweep(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonClear(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //골드던전
    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonExit(int nContinentId)
    {
        UpdateHp();
    }

    //스토리던전
    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonExit(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonBanished(int nContinentId)
    {
        UpdateHp();
    }

    //용맹의 증명
    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorSweep(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorClear(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorFail(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorAbandon(int nPreviousContinentId, bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventProofOfValorBuffBoxAcquire(int nRecoveryHp)
    {
        UpdateHp();
    }

	// 지혜의 신전
	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleSweep(bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleBanished()
	{
		
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleClear(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleFail(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleAbandon(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleExit(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTempleStepCompleted(bool bLevelUp, long lAcquiredExp, PDItemBooty pdItemBooty)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWisdomTemplePuzzleCompleted(bool bLevelUp, long lAcquiredExp, PDWisdomTemplePuzzleRewardObjectInstance[] aPDWisdomTemplePuzzleRewardObjectInstance)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRevive(PDVector3 position, float flRotationY)
	{
		UpdateHp();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarExit(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarAbandon(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBanished(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventInfiniteWarBuffBoxAcquire(int nRecoveryHp)
    {
        UpdateHp();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarRevive()
	{
		UpdateHp();
	}

    #region WarMemory

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryExit(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryAbandon(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryBanished(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWarMemoryRevive(PDVector3 pDVector3, float flRotationY)
    {
        UpdateHp();
    }

    #endregion WarMemory

    #region DragonNest

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestExit(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestAbandon(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestBanished(int nPreviousContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDragonNestRevive()
    {
        UpdateHp();
    }

    #endregion DragonNest

    #region TradeShip

    void OnEventTradeShipExit(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventTradeShipAbandon(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventTradeShipBanished(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventTradeShipRevive()
    {
        UpdateHp();
    }

    #endregion TradeShip

    #region AnkouTomb

    void OnEventAnkouTombExit(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventAnkouTombAbandon(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventAnkouTombBanished(int nPreviousContinentId)
    {
        UpdateHp();
    }

    void OnEventAnkouTombRevive()
    {
        UpdateHp();
    }

    #endregion AnkouTomb

    //---------------------------------------------------------------------------------------------------
    void OnEventMissionComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEquip(Guid guidHeroGearId)
    {
        CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroGearId);
        if(csHeroMainGear != null)
        {
            if (csHeroMainGear.MainGear.MainGearType.EnMainGearType == EnMainGearType.Armor)
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.Armor);
            }
            else
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.Weapon);
            }
        }

        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearUnequip(Guid guidHeroGearId)
    {
        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchant(bool bSuccess, Guid guidHeroGearId)
    {
        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearTransit(Guid guidTargetHeroGearId, Guid guidMaterialHeroGearId)
    {
        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearRefinementApply(Guid guidHeroGearId)
    {
        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchantLevelSetActivate()
    {
        UpdateMainGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearEquip(int nSubGearId)
    {
        switch ((EnSubGearType)nSubGearId)
        {
            case EnSubGearType.Helmet:
                CsUIData.Instance.PlayUISound(EnUISoundType.Armor);
                break;

            case EnSubGearType.Belt:
                CsUIData.Instance.PlayUISound(EnUISoundType.Armor);
                break;

            case EnSubGearType.Gloves:
                CsUIData.Instance.PlayUISound(EnUISoundType.Armor);
                break;

            case EnSubGearType.Shoes:
                CsUIData.Instance.PlayUISound(EnUISoundType.Armor);
                break;

            case EnSubGearType.Necklace:
                CsUIData.Instance.PlayUISound(EnUISoundType.Ring);
                break;

            case EnSubGearType.Ring:
                CsUIData.Instance.PlayUISound(EnUISoundType.Ring);
                break;
        }
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearUnequip(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulstoneSocketMount(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulstoneSocketUnmount(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuneSocketMount(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuneSocketUnmount(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUp(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUpTotally(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearGradeUp(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearQualityUp(int nSubGearId)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountedSoulstoneCompose(int nSubGearId, int nSocketIndex)
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSoulstoneLevelSetActivate()
    {
        UpdateSubGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUp(int nSkillId)
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSkillLevelUpTotally(int nSkillId)
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
        if (nTransformationMonsterId == 0)
        {
            return;
        }
        else
        {
            UpdateBattlePower();
            UpdateHp();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsUIData.Instance.PlayUISound(EnUISoundType.Gold);

		if (CsMainQuestManager.Instance.PrevMainQuest != null &&
			CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc != null &&
			CsMainQuestManager.Instance.MainQuest != null &&
			((CsMainQuestManager.Instance.MainQuest.StartNpc != null &&
			CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc.NpcId == CsMainQuestManager.Instance.MainQuest.StartNpc.NpcId) ||
			(CsMainQuestManager.Instance.MainQuest.CompletionNpc != null &&
			CsMainQuestManager.Instance.PrevMainQuest.CompletionNpc.NpcId == CsMainQuestManager.Instance.MainQuest.CompletionNpc.NpcId)))
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveFree(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveGold(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveDia(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHpPotionUse(int nRecoveryHp)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcquisition(long lAcquiredExp, bool bLevelUp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpPotionUse(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventImmediateRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroInfoUpdate()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldItemUse()
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonStepCompleted(long lRewardGold)
    {
        if (lRewardGold > 0)
            CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonClear(long lRewardGold)
    {
        if(lRewardGold > 0)
            CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRankRewardReceive()
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookExplorationStepRewardReceive(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Gold);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountEquip()
    {
        UpdateMountGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountLevelUp(bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateMonutLevelUp();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearRefine(Guid guid)
    {
        UpdateMountGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearEquip(Guid guid)
    {
        UpdateMountGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearUnequip(Guid guid)
    {
        UpdateMountGearChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEquip()
    {
        UpdateWingChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEnchant()
    {
        UpdateWingChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEnchantTotally()
    {
        UpdateWingChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRankAcquire()
    {
        UpdateBattlePower();
    }

    void OnEventNationNoblesseAppointed(int nNoblessId, string m_strHeroName)
    {
        UpdateBattlePower();
    }

    void OnEventNationNoblesseDismissed()
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------


    //---------------------------------------------------------------------------------------------------
    void OnEventBountyHunterQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMysteryBoxQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSecretLetterQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDimensionRaidQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFishingCastingCompleted(bool bLevelUp, long lAcquiredExp, bool bBaitEnable)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHolyWarQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonBanished(int nPreviousContinentId, bool bChangeScene)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonSaftyRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGoldDungeonRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulCoveterRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEliteDungeonRevive()
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeEnterForUndergroundMazeRevive(Guid guidPlaceInstanceId, ClientCommon.PDVector3 position, float flRotationY, ClientCommon.PDHero[] heroes, ClientCommon.PDUndergroundMazeMonsterInstance[] monsterInsts)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeBanished(int nContinentId)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainQuestDungeonExit(int nPreviousContinentId, bool bChangeScene)
    {
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildCreate()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildExit(int nContinentId)
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildInvitationAccept()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildApplicationAccepted()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBanished(int nContinentId)
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSkillLevelUp()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUp()
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUpEvent()
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWarehouseStock(bool bLevelUp, long lAcquredExp, int nAddedFoodWarehouseExp, int nFoodWarehouseLevel, int nFoodWarehouseExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSupplySupportQuestCompleted(bool bLevelUp, long lAcquiredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }

        CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildMissionComplete(bool bLevelUp, long lAcquredExp)
    {
        UpdateLevelUpInfo();

        if (bLevelUp)
        {
            LevelUpEvent();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookUse()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookExplorationStepAcquire()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTitleLifetimeEnded(int nTitleId)
    {
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventTitleItemUse()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventActivationTitleSet()
    {
        UpdateHp();
        UpdateBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardCollectionActivate(int nCollectionId)
    {
        UpdateHp();
        UpdateBattlePower();
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventDailyQuestComplete(bool bLevelUp, long lAcquiredExp, int nSlotIndex)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundCompleted(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestTenRoundImmediatlyComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWeeklyQuestRoundMoveMissionComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGold(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveGoldAll(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDia(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRetrieveDiaAll(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTaskConsignmentComplete(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTaskConsignmentImmediatelyComplete(bool bLevelUp, long lExpAcq)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		if (lExpAcq > 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lExpAcq));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSubQuestComplete(bool bLevelUp, long lAcquiredExp, int nSubQuestId)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOrdealQuestSlotComplete(bool bLevelUp, long lAcquiredExp, int nIndex)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOrdealQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("PUBLIC_GET_EXP"), lAcquiredExp));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestComplete(bool bLevelUp, long lAcquiredExp, int nBiographyId)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipate()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipationCancel()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheer()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheerCancel()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureRear()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureInject(bool bCritical, bool bLevelUp)
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureInjectionRetrieval()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureVary()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureAdditionalAttrSwitch()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureSkillSlotOpen()
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCompose(Guid guidCreatureInstanceId)
	{
		UpdateBattlePower();
		UpdateHp();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionMoveObjectiveComplete(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureFarmQuestMissionCompleted(bool bLevelUp, long lAcquiredExp)
	{
		UpdateLevelUpInfo();

		if (bLevelUp)
		{
			LevelUpEvent();
		}

		CsGameEventUIToUI.Instance.OnEventExpAcuisitionText(lAcquiredExp);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationEntryActivate(bool bSuccess)
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactOpened()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactLevelUp()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCostumeEnchant(bool bEnchant)
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCostumeCollectionShuffle()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCostumeCollectionActivate()
	{
		UpdateHp();
		UpdateBattlePower();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChange()
    {
        UpdateBattlePower();
        UpdateHp();

        Image imageJob = transform.Find("MyheroInfo/ImageEmblem/ImageIcon").GetComponent<Image>();
        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + CsGameData.Instance.MyHeroInfo.Job.JobId);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventAccomplishmentLevelUp()
	{
		UpdateHp();
		UpdateBattlePower();
	}

	#endregion EventHandler

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
    {
        // 나의영웅정보
        Transform trMyHeroInfo = transform.Find("MyheroInfo");

        Image imageJob = trMyHeroInfo.Find("ImageEmblem/ImageIcon").GetComponent<Image>();
        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + CsGameData.Instance.MyHeroInfo.Job.JobId);

        m_textLevel = trMyHeroInfo.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevel);

        //Text textName = trMyHeroInfo.Find("TextName").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textName);
        //textName.text = CsGameData.Instance.MyHeroInfo.Name;

        m_textBattlePower = trMyHeroInfo.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBattlePower);

        m_textSliderValue = trMyHeroInfo.Find("TextSliderValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSliderValue);

        m_sliderHp = trMyHeroInfo.Find("SliderHp").GetComponent<Slider>();

        // 경험치
        Transform trExp = transform.Find("Exp");

        m_textExp = trExp.Find("TextExp").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textExp);

        m_textTime = trExp.Find("TextTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTime);

        m_sliderExp = trExp.Find("Slider").GetComponent<Slider>();

		// 네트웍
		m_trNetwork = trExp.Find("Network");
		m_goNetwork3G = m_trNetwork.Find("Image3G").gameObject;
		m_goNetworkLTE = m_trNetwork.Find("ImageLTE").gameObject;
		m_goNetworkWIFI1 = m_trNetwork.Find("ImageWIFI1").gameObject;
		m_goNetworkWIFI2 = m_trNetwork.Find("ImageWIFI2").gameObject;
		m_goNetworkWIFI3 = m_trNetwork.Find("ImageWIFI3").gameObject;

		// 배터리
		Transform trBattery = trExp.Find("Battery");
		m_sliderBattery = trBattery.Find("Slider").GetComponent<Slider>();
		m_goBatteryFull = trBattery.Find("ImageFull").gameObject;
		m_goCharge = trBattery.Find("ImageCharge").gameObject;

		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			m_trNetwork.gameObject.SetActive(true);
			trBattery.gameObject.SetActive(true);
		}
		
        UpdateLevel();
        UpdateBattlePower();
        UpdateHp();
        UpdateExperience();

        InvokeRepeating("UpdateCurrentTime", 0, 60.0f);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLevel()
    {
        m_textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), CsGameData.Instance.MyHeroInfo.Level);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBattlePower()
    {
        m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), CsGameData.Instance.MyHeroInfo.BattlePower.ToString("#,###"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHp()
    {
        m_sliderHp.maxValue = CsGameData.Instance.MyHeroInfo.MaxHp;
        m_sliderHp.value = CsGameData.Instance.MyHeroInfo.Hp;
        //m_textSliderValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.Hp.ToString("#,###"), CsGameData.Instance.MyHeroInfo.MaxHp.ToString("#,###"));
        m_textSliderValue.text = CsGameData.Instance.MyHeroInfo.Hp.ToString("#,##0");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExperience()
    {
        m_sliderExp.maxValue = CsGameData.Instance.MyHeroInfo.RequiredExp;
        m_sliderExp.value = CsGameData.Instance.MyHeroInfo.Exp;
        m_textExp.text = string.Format(CsConfiguration.Instance.GetString("INPUT_EXP"), CsGameData.Instance.MyHeroInfo.ExpPercent.ToString("0.##"));
        CsGameEventUIToUI.Instance.OnEventMyHeroExpUp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCurrentTime()
    {
        m_textTime.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), CsGameData.Instance.MyHeroInfo.CurrentDateTime.Hour.ToString("00"), CsGameData.Instance.MyHeroInfo.CurrentDateTime.Minute.ToString("00"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearChanged()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMountGearChanged()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearChanged()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingChanged()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMonutLevelUp()
    {
        UpdateBattlePower();
        UpdateHp();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLevelUpInfo()
    {
        UpdateLevel();
        UpdateBattlePower();
        UpdateHp();
        UpdateExperience();
    }

    //---------------------------------------------------------------------------------------------------
    void LevelUpEvent()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        CsGameEventUIToUI.Instance.OnEventMyHeroLevelUp();
        CsGameEventToIngame.Instance.OnEventMyHeroLevelUp();

		CsConfiguration.Instance.SendFirebaseLogEvent("level_up", CsGameData.Instance.MyHeroInfo.Level.ToString());
		CsSharingEventManager.Instance.RequestFirebaseDynamicLinkReward();
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateNetworkState(string strNetworkType, string strSignalStrength)
	{
		if (m_trNetwork != null)
		{
			for (int i = 0; i < m_trNetwork.childCount; i++)
			{
				m_trNetwork.GetChild(i).gameObject.SetActive(false);
			}
		}

		switch (strNetworkType.ToUpper())
		{
			case "3G":
				if (m_goNetwork3G != null)
				{
					m_goNetwork3G.SetActive(true);
				}
				break;
			case "LTE":
				if (m_goNetworkLTE != null)
				{
					m_goNetworkLTE.SetActive(true);
				}
				break;
			case "WIFI":
				int nSignalStrength = int.Parse(strSignalStrength);

				if (nSignalStrength == 1)
				{
					if (m_goNetworkWIFI1 != null)
					{
						m_goNetworkWIFI1.SetActive(true);
					}
				}
				else if (nSignalStrength == 2)
				{
					if (m_goNetworkWIFI2 != null)
					{
						m_goNetworkWIFI2.SetActive(true);
					}
				}
				else if (nSignalStrength >= 3)
				{
					if (m_goNetworkWIFI3 != null)
					{
						m_goNetworkWIFI3.SetActive(true);
					}
				}
				
				break;
			default:
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateBatteryState(string strBatteryState, string strChargeType)
	{
		float flBatteryState = float.Parse(strBatteryState);

		if (m_sliderBattery != null)
		{
			m_sliderBattery.value = flBatteryState * 0.01f;
		}

		if (m_goBatteryFull != null)
		{
			m_goBatteryFull.SetActive(flBatteryState >= 100.0f);
		}

		if (m_goCharge != null)
		{
			m_goCharge.SetActive(strChargeType.ToLower().CompareTo("unplugged") != 0);
		}
	}

	
}
