using System;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsCommandEventManager
{
    bool m_bProcessing = false;

    // 메인장비
    Guid m_guildHeroGearId;
    Guid m_guildMaterialHeroGearId;
    int m_nMainGearSetNo;

    // 보조장비
    int m_nSubGearId;
    int m_nSocketIndex;
    int m_nItemId;
    int m_nSubGearSetNo;

    // 메일
    Guid m_guidMailId;

    // 스킬
    int m_nSkillId;

    // 간이상점
    int[] m_anSlotIndices;

    // 아이템
    int m_nInventorySlotIndex;

    // 파티
    Guid m_guidMemberId;
    long m_lApplicationNo;

    // 지원
    int m_nEntryId;

    // 탈것
    int m_nMountId;

    // 탈것장비
    Guid m_guidHeroMountGearId;
    int m_nOptionAttrIndex;

    // 날개
    int m_nWingId;
    int m_nWingPartId;

    // 스토리던전
    //int m_nDungeonNo;
    //int m_nDifficulty;

    // 영웅정보
    Guid m_guidHeroId;

    // 대륙전송
    int m_nNpcId;
    int m_nExitNo;

    // 국가전송
    int m_nNationId;

    // 미션
    int m_nMissionId;
    int m_nStep;

    // 오늘의할일 今天的待办事项
    int m_nRewardNo;

    // Vip레벨보상받기 赚取Vip级别的奖励
    int m_nVipLevel;

    // 계급 级别
    int m_nTargetRankNo;

    // 도달보상 达成奖励
    int m_nEntryNo;

    // 설정 设置
    int m_nLootingItemMinGrade;

	int m_nSlotIndex;

    // NPC상점 NPC商店
    int m_nProductId;

	int m_nTargetSkillId;

	int m_nDay;

    // 회수 召回
    int m_nRetrievalId;

    // 할일위탁 委托任务
    Guid m_guidInstanceId;

    // 한정선물 有限的礼物
    int m_nScheduleId;

    // 주말보상 周末奖励
    int m_nSelectionNo;

    // 공포의 제단 恐惧的祭坛
    int m_nFearAltarHalidomCollectionRewardNo;
	int m_nFearAltarElementalId;

	//---------------------------------------------------------------------------------------------------
	public static CsCommandEventManager Instance
    {
        get { return CsSingleton<CsCommandEventManager>.GetInstance(); }
    }

    //---------------------------------------------------------------------------------------------------
    public void Initialize()
    {
        UnRegister();

        // 날짜변경
        CsRplzSession.Instance.EventEvtDateChanged += OnEventEvtDateChanged;

        // 메인장비
        CsRplzSession.Instance.EventResMainGearEquip += OnEventResMainGearEquip;
        CsRplzSession.Instance.EventResMainGearUnequip += OnEventResMainGearUnequip;
        CsRplzSession.Instance.EventResMainGearEnchant += OnEventResMainGearEnchant;
        CsRplzSession.Instance.EventResMainGearTransit += OnEventResMainGearTransit;
        CsRplzSession.Instance.EventResMainGearRefine += OnEventResMainGearRefine;
        CsRplzSession.Instance.EventResMainGearRefinementApply += OnEventResMainGearRefinementApply;
        CsRplzSession.Instance.EventResMainGearDisassemble += OnEventResMainGearDisassemble;
        CsRplzSession.Instance.EventResMainGearEnchantLevelSetActivate += OnEventResMainGearEnchantLevelSetActivate;

        // 보조장비
        CsRplzSession.Instance.EventResSubGearEquip += OnEventResSubGearEquip;
        CsRplzSession.Instance.EventResSubGearUnequip += OnEventResSubGearUnequip;
        CsRplzSession.Instance.EventResSoulstoneSocketMount += OnEventResSoulstoneSocketMount;
        CsRplzSession.Instance.EventResSoulstoneSocketUnmount += OnEventResSoulstoneSocketUnmount;
        CsRplzSession.Instance.EventResRuneSocketMount += OnEventResRuneSocketMount;
        CsRplzSession.Instance.EventResRuneSocketUnmount += OnEventResRuneSocketUnmount;
        CsRplzSession.Instance.EventResSubGearLevelUp += OnEventResSubGearLevelUp;
        CsRplzSession.Instance.EventResSubGearLevelUpTotally += OnEventResSubGearLevelUpTotally;
        CsRplzSession.Instance.EventResSubGearGradeUp += OnEventResSubGearGradeUp;
        CsRplzSession.Instance.EventResSubGearQualityUp += OnEventResSubGearQualityUp;
        CsRplzSession.Instance.EventResMountedSoulstoneCompose += OnEventResMountedSoulstoneCompose;
        CsRplzSession.Instance.EventResSubGearSoulstoneLevelSetActivate += OnEventResSubGearSoulstoneLevelSetActivate;

        // 메일
        CsRplzSession.Instance.EventResMailReceive += OnEventResMailReceive;
        CsRplzSession.Instance.EventResMailReceiveAll += OnEventResMailReceiveAll;
		CsRplzSession.Instance.EventResMailDelete += OnEventResMailDelete;
		CsRplzSession.Instance.EventResMailDeleteAll += OnEventResMailDeleteAll;
		CsRplzSession.Instance.EventEvtNewMail += OnEventEvtNewMail;                    // 신규메일

        // 라크획득
        CsRplzSession.Instance.EventEvtLakAcquisition += OnEventEvtLakAcquisition;      // 라크획득
                                                                                        // 경험치획득
        CsRplzSession.Instance.EventEvtExpAcquisition += OnEventEvtExpAcquisition;

        // 스킬
        CsRplzSession.Instance.EventResSkillLevelUp += OnEventResSkillLevelUp;
        CsRplzSession.Instance.EventResSkillLevelUpTotally += OnEventResSkillLevelUpTotally;

        // 간이상점
        CsRplzSession.Instance.EventResSimpleShopBuy += OnEventResSimpleShopBuy;
        CsRplzSession.Instance.EventResSimpleShopSell += OnEventResSimpleShopSell;

        // 인벤토리
        CsRplzSession.Instance.EventResInventorySlotExtend += OnEventResInventorySlotExtend;

        // 아이템
        CsRplzSession.Instance.EventResItemCompose += OnEventResItemCompose;
        CsRplzSession.Instance.EventResItemComposeTotally += OnEventResItemComposeTotally;
        CsRplzSession.Instance.EventResHpPotionUse += OnEventResHpPotionUse;
        CsRplzSession.Instance.EventResReturnScrollUse += OnEventResReturnScrollUse;
        CsRplzSession.Instance.EventEvtReturnScrollUseFinished += OnEventEvtReturnScrollUseFinished;
        CsRplzSession.Instance.EventEvtReturnScrollUseCancel += OnEventEvtReturnScrollUseCancel;
        CsRplzSession.Instance.EventResPickBoxUse += OnEventResPickBoxUse;
        CsRplzSession.Instance.EventResMainGearBoxUse += OnEventResMainGearBoxUse;
        CsRplzSession.Instance.EventResExpPotionUse += OnEventResExpPotionUse;
        CsRplzSession.Instance.EventResExpScrollUse += OnEventResExpScrollUse;
        CsRplzSession.Instance.EventResBountyHunterQuestScrollUse += OnEventResBountyHunterQuestScrollUse;
        CsRplzSession.Instance.EventResFishingBaitUse += OnEventResFishingBaitUse;
        CsRplzSession.Instance.EventResDistortionScrollUse += OnEventResDistortionScrollUse;
        CsRplzSession.Instance.EventResDistortionCancel += OnEventResDistortionCancel;
        CsRplzSession.Instance.EventEvtDistortionCanceled += OnEventEvtDistortionCanceled;
        CsRplzSession.Instance.EventResGoldItemUse += OnEventResGoldItemUse;
        CsRplzSession.Instance.EventResOwnDiaItemUse += OnEventResOwnDiaItemUse;
        CsRplzSession.Instance.EventResHonorPointItemUse += OnEventResHonorPointItemUse;
        CsRplzSession.Instance.EventResExploitPointItemUse += OnEventResExploitPointItemUse;
		CsRplzSession.Instance.EventResWingItemUse += OnEventResWingItemUse;

		// 휴식
		CsRplzSession.Instance.EventResRestRewardReceiveFree += OnEventResRestRewardReceiveFree;
        CsRplzSession.Instance.EventResRestRewardReceiveGold += OnEventResRestRewardReceiveGold;
        CsRplzSession.Instance.EventResRestRewardReceiveDia += OnEventResRestRewardReceiveDia;

        // 부활
        CsRplzSession.Instance.EventResImmediateRevive += OnEventResImmediateRevive;
        CsRplzSession.Instance.EventResContinentSaftyRevive += OnEventResContinentSaftyRevive;

        // 아이템 루팅
        CsRplzSession.Instance.EventEvtDropObjectLooted += OnEventEvtDropObjectLooted;

        // 파티
        CsRplzSession.Instance.EventResPartySurroundingHeroList += OnEventResPartySurroundingHeroList;
        CsRplzSession.Instance.EventResPartySurroundingPartyList += OnEventResPartySurroundingPartyList;
        CsRplzSession.Instance.EventResPartyCreate += OnEventResPartyCreate;
        CsRplzSession.Instance.EventResPartyExit += OnEventResPartyExit;
        CsRplzSession.Instance.EventResPartyMemberBanish += OnEventResPartyMemberBanish;
        CsRplzSession.Instance.EventResPartyCall += OnEventResPartyCall;
        CsRplzSession.Instance.EventResPartyDisband += OnEventResPartyDisband;
        CsRplzSession.Instance.EventResPartyApply += OnEventResPartyApply;
        CsRplzSession.Instance.EventResPartyApplicationAccept += OnEventResPartyApplicationAccept;
        CsRplzSession.Instance.EventResPartyApplicationRefuse += OnEventResPartyApplicationRefuse;
        CsRplzSession.Instance.EventResPartyInvite += OnEventResPartyInvite;
        CsRplzSession.Instance.EventResPartyInvitationAccept += OnEventResPartyInvitationAccept;
        CsRplzSession.Instance.EventResPartyInvitationRefuse += OnEventResPartyInvitationRefuse;
        CsRplzSession.Instance.EventResPartyMasterChange += OnEventResPartyMasterChange;

        CsRplzSession.Instance.EventEvtPartyApplicationArrived += OnEventEvtPartyApplicationArrived;
        CsRplzSession.Instance.EventEvtPartyApplicationCanceled += OnEventEvtPartyApplicationCanceled;
        CsRplzSession.Instance.EventEvtPartyApplicationAccepted += OnEventEvtPartyApplicationAccepted;
        CsRplzSession.Instance.EventEvtPartyApplicationRefused += OnEventEvtPartyApplicationRefused;
        CsRplzSession.Instance.EventEvtPartyApplicationLifetimeEnded += OnEventEvtPartyApplicationLifetimeEnded;

        CsRplzSession.Instance.EventEvtPartyInvitationArrived += OnEventEvtPartyInvitationArrived;
        CsRplzSession.Instance.EventEvtPartyInvitationCanceled += OnEventEvtPartyInvitationCanceled;
        CsRplzSession.Instance.EventEvtPartyInvitationAccepted += OnEventEvtPartyInvitationAccepted;
        CsRplzSession.Instance.EventEvtPartyInvitationRefused += OnEventEvtPartyInvitationRefused;
        CsRplzSession.Instance.EventEvtPartyInvitationLifetimeEnded += OnEventEvtPartyInvitationLifetimeEnded;

        CsRplzSession.Instance.EventEvtPartyMemberEnter += OnEventEvtPartyMemberEnter;
        CsRplzSession.Instance.EventEvtPartyMemberExit += OnEventEvtPartyMemberExit;
        CsRplzSession.Instance.EventEvtPartyBanished += OnEventEvtPartyBanished;
        CsRplzSession.Instance.EventEvtPartyMasterChanged += OnEventEvtPartyMasterChanged;
        CsRplzSession.Instance.EventEvtPartyCall += OnEventEvtPartyCall;
        CsRplzSession.Instance.EventEvtPartyDisbanded += OnEventEvtPartyDisbanded;
        CsRplzSession.Instance.EventEvtPartyMembersUpdated += OnEventEvtPartyMembersUpdated;

        // 채팅
        CsRplzSession.Instance.EventResChattingMessageSend += OnEventResChattingMessageSend;
        CsRplzSession.Instance.EventEvtChattingMessageReceived += OnEventEvtChattingMessageReceived;

        // 지원
        CsRplzSession.Instance.EventResLevelUpRewardReceive += OnEventResLevelUpRewardReceive;
        CsRplzSession.Instance.EventResDailyAccessTimeRewardReceive += OnEventResDailyAccessTimeRewardReceive;
        CsRplzSession.Instance.EventResAttendRewardReceive += OnEventResAttendRewardReceive;
        CsRplzSession.Instance.EventResSeriesMissionRewardReceive += OnEventResSeriesMissionRewardReceive;
        CsRplzSession.Instance.EventResTodayMissionRewardReceive += OnEventResTodayMissionRewardReceive;
        CsRplzSession.Instance.EventEvtSeriesMissionUpdated += OnEventEvtSeriesMissionUpdated;
        CsRplzSession.Instance.EventEvtTodayMissionListChanged += OnEventEvtTodayMissionListChanged;
        CsRplzSession.Instance.EventEvtTodayMissionUpdated += OnEventEvtTodayMissionUpdated;
		CsRplzSession.Instance.EventResRookieGiftReceive += OnEventResRookieGiftReceive;
		CsRplzSession.Instance.EventResOpenGiftReceive += OnEventResOpenGiftReceive;

		// 탈것
		CsRplzSession.Instance.EventResMountEquip += OnEventResMountEquip;
        CsRplzSession.Instance.EventResMountLevelUp += OnEventResMountLevelUp;
        CsRplzSession.Instance.EventResMountGetOn += OnEventResMountGetOn;
		CsRplzSession.Instance.EventResMountAwakeningLevelUp += OnEventResMountAwakeningLevelUp;
		CsRplzSession.Instance.EventResMountAttrPotionUse += OnEventResMountAttrPotionUse;
		CsRplzSession.Instance.EventResMountItemUse += OnEventResMountItemUse;

		// 탈것장비
		CsRplzSession.Instance.EventResMountGearEquip += OnEventResMountGearEquip;
        CsRplzSession.Instance.EventResMountGearUnequip += OnEventResMountGearUnequip;
        CsRplzSession.Instance.EventResMountGearRefine += OnEventResMountGearRefine;
        CsRplzSession.Instance.EventResMountGearPickBoxMake += OnEventResMountGearPickBoxMake;
        CsRplzSession.Instance.EventResMountGearPickBoxMakeTotally += OnEventResMountGearPickBoxMakeTotally;

        // 영웅
        CsRplzSession.Instance.EventResHeroInfo += OnEventResHeroInfo;
		CsRplzSession.Instance.EventResHeroPosition += OnEventResHeroPosition;

        // 날개
        CsRplzSession.Instance.EventResWingEquip += OnEventResWingEquip;
        CsRplzSession.Instance.EventResWingEnchant += OnEventResWingEnchant;
        CsRplzSession.Instance.EventResWingEnchantTotally += OnEventResWingEnchantTotally;
        CsRplzSession.Instance.EventEvtWingAcquisition += OnEventEvtWingAcquisition;
		CsRplzSession.Instance.EventResWingMemoryPieceInstall += OnEventResWingMemoryPieceInstall;

		// 스토리던전
		//CsRplzSession.Instance.EventResContinentExitForStoryDungeonEnter += OnEventResContinentExitForStoryDungeonEnter;
		//CsRplzSession.Instance.EventResStoryDungeonAbandon += OnEventResStoryDungeonAbandon;
		//CsRplzSession.Instance.EventResStoryDungeonExit += OnEventResStoryDungeonExit;
		//CsRplzSession.Instance.EventResStoryDungeonRevive += OnEventResStoryDungeonRevive;
		//CsRplzSession.Instance.EventResStoryDungeonSweep += OnEventResStoryDungeonSweep;

		// 스테미나
		CsRplzSession.Instance.EventEvtStaminaAutoRecovery += OnEventEvtStaminaAutoRecovery;
        CsRplzSession.Instance.EventResStaminaBuy += OnEventResStaminaBuy;
        CsRplzSession.Instance.EventEvtStaminaScheduleRecovery += OnEventEvtStaminaScheduleRecovery;

        // 대륙전송
        CsRplzSession.Instance.EventResContinentTransmission += OnEventResContinentTransmission;

        // 국가
        CsRplzSession.Instance.EventResNationTransmission += OnEventResNationTransmission;
        CsRplzSession.Instance.EventResHeroSearch += OnEventResHeroSearch;
        CsRplzSession.Instance.EventResNationDonate += OnEventResNationDonate;
        CsRplzSession.Instance.EventResNationNoblesseAppoint += OnEventResNationNoblesseAppoint;
        CsRplzSession.Instance.EventResNationNoblesseDismiss += OnEventResNationNoblesseDismiss;
        CsRplzSession.Instance.EventEvtNationNoblesseAppointment += OnEventEvtNationNoblesseAppointment;
        CsRplzSession.Instance.EventEvtNationNoblesseDismissal += OnEventEvtNationNoblesseDismissal;
        CsRplzSession.Instance.EventEvtNationFundChanged += OnEventEvtNationFundChanged;
        CsRplzSession.Instance.EventResNationCall += OnEventResNationCall;
        CsRplzSession.Instance.EventResNationCallTransmission += OnEventResNationCallTransmission;
        CsRplzSession.Instance.EventEvtNationCall += OnEventEvtNationCall;

		// 오늘의 할일
		CsRplzSession.Instance.EventResAchievementRewardReceive += OnEventResAchievementRewardReceive;
        CsRplzSession.Instance.EventEvtTodayTaskUpdated += OnEventEvtTodayTaskUpdated;

        // VIP
        CsRplzSession.Instance.EventResVipLevelRewardReceive += OnEventResVipLevelRewardReceive;

        // 계급
        CsRplzSession.Instance.EventResRankAcquire += OnEventResRankAcquire;
        CsRplzSession.Instance.EventResRankRewardReceive += OnEventResRankRewardReceive;
		CsRplzSession.Instance.EventResRankActiveSkillLevelUp += OnEventResRankActiveSkillLevelUp;
		CsRplzSession.Instance.EventResRankActiveSkillSelect += OnEventResRankActiveSkillSelect;
		CsRplzSession.Instance.EventResRankPassiveSkillLevelUp += OnEventResRankPassiveSkillLevelUp;

		// 명예상점
		CsRplzSession.Instance.EventResHonorShopProductBuy += OnEventResHonorShopProductBuy;

        // 랭킹
        CsRplzSession.Instance.EventResServerBattlePowerRanking += OnEventResServerBattlePowerRanking;
        CsRplzSession.Instance.EventResServerJobBattlePowerRanking += OnEventResServerJobBattlePowerRanking;
        CsRplzSession.Instance.EventResServerLevelRanking += OnEventResServerLevelRanking;
        CsRplzSession.Instance.EventResServerLevelRankingRewardReceive += OnEventResServerLevelRankingRewardReceive;
        CsRplzSession.Instance.EventResNationBattlePowerRanking += OnEventResNationBattlePowerRanking;
        CsRplzSession.Instance.EventResNationExploitPointRanking += OnEventResNationExploitPointRanking;
        CsRplzSession.Instance.EventEvtDailyServerLevelRankingUpdated += OnEventEvtDailyServerLevelRankingUpdated;
        CsRplzSession.Instance.EventResServerCreatureCardRanking += OnEventResServerCreatureCardRanking;
        CsRplzSession.Instance.EventResServerIllustratedBookRanking += OnEventResServerIllustratedBookRanking;
		CsRplzSession.Instance.EventEvtDailyServerNationPowerRankingUpdated += OnEventEvtDailyServerNationPowerRankingUpdated;

		// 도달
		CsRplzSession.Instance.EventResAttainmentRewardReceive += OnEventResAttainmentRewardReceive;

        // 대륙
        CsRplzSession.Instance.EventEvtContinentBanished += OnEventEvtContinentBanished;

        // 세팅
        CsRplzSession.Instance.EventResBattleSettingSet += OnEventResBattleSettingSet;

        // 공지
        CsRplzSession.Instance.EventEvtNotice += OnEventEvtNotice;

		// 오늘의미션튜토리얼시작
        CsRplzSession.Instance.EventResTodayMissionTutorialStart += OnEventResTodayMissionTutorialStart;

		// 서버최고레벨갱신
		CsRplzSession.Instance.EventEvtServerMaxLevelUpdated += OnEventEvtServerMaxLevelUpdated;

        CsRplzSession.Instance.EventResGroggyMonsterItemStealStart += OnEventResGroggyMonsterItemStealStart;
        CsRplzSession.Instance.EventEvtGroggyMonsterItemStealCancel += OnEventEvtGroggyMonsterItemStealCancel;
        CsRplzSession.Instance.EventEvtGroggyMonsterItemStealFinished += OnEventEvtGroggyMonsterItemStealFinished;

		// 영웅NPC상점상품
		CsRplzSession.Instance.EventResNpcShopProductBuy += OnEventResNpcShopProductBuy;

		// 오픈7일이벤트
		CsRplzSession.Instance.EventResOpen7DayEventMissionRewardReceive += OnEventResOpen7DayEventMissionRewardReceive;
		CsRplzSession.Instance.EventResOpen7DayEventProductBuy += OnEventResOpen7DayEventProductBuy;
		CsRplzSession.Instance.EventResOpen7DayEventRewardReceive += OnEventResOpen7DayEventRewardReceive;
		CsRplzSession.Instance.EventEvtOpen7DayEventProgressCountUpdated += OnEventEvtOpen7DayEventProgressCountUpdated;

		// 회수
		CsRplzSession.Instance.EventResRetrieveGold += OnEventResRetrieveGold;
		CsRplzSession.Instance.EventResRetrieveGoldAll += OnEventResRetrieveGoldAll;
		CsRplzSession.Instance.EventResRetrieveDia += OnEventResRetrieveDia;
		CsRplzSession.Instance.EventResRetrieveDiaAll += OnEventResRetrieveDiaAll;
		CsRplzSession.Instance.EventEvtRetrievalProgressCountUpdated += OnEventEvtRetrievalProgressCountUpdated;

		// 할일위탁
		CsRplzSession.Instance.EventResTaskConsignmentStart += OnEventResTaskConsignmentStart;
		CsRplzSession.Instance.EventResTaskConsignmentComplete += OnEventResTaskConsignmentComplete;
		CsRplzSession.Instance.EventResTaskConsignmentImmediatelyComplete += OnEventResTaskConsignmentImmediatelyComplete;

		// 한정선물
		CsRplzSession.Instance.EventResLimitationGiftRewardReceive += OnEventResLimitationGiftRewardReceive;

		// 주말보상
		CsRplzSession.Instance.EventResWeekendRewardSelect += OnEventResWeekendRewardSelect;
		CsRplzSession.Instance.EventResWeekendRewardReceive += OnEventResWeekendRewardReceive;

		// 창고
		CsRplzSession.Instance.EventResWarehouseDeposit += OnEventResWarehouseDeposit;
		CsRplzSession.Instance.EventResWarehouseWithdraw += OnEventResWarehouseWithdraw;
		CsRplzSession.Instance.EventResWarehouseSlotExtend += OnEventResWarehouseSlotExtend;

		//다이아상점
		CsRplzSession.Instance.EventResDiaShopProductBuy += OnEventResDiaShopProductBuy;

		// 성물 수집
		CsRplzSession.Instance.EventResFearAltarHalidomElementalRewardReceive += OnEventResFearAltarHalidomElementalRewardReceive;
		CsRplzSession.Instance.EventResFearAltarHalidomCollectionRewardReceive += OnEventResFearAltarHalidomCollectionRewardReceive;
		CsRplzSession.Instance.EventEvtFearAltarHalidomAcquisition += OnEventEvtFearAltarHalidomAcquisition;

        // 포탈
        CsRplzSession.Instance.EventResPortalEnter += OnEventResPortalEnter;

        // 다른 디바이스에서 중복 접속
        CsRplzSession.Instance.EventEvtAccountLoginDuplicated += OnEventEvtAccountLoginDuplicated;

		// 물약속성
		CsRplzSession.Instance.EventResHeroAttrPotionUse += OnEventResHeroAttrPotionUse;
		CsRplzSession.Instance.EventResHeroAttrPotionUseAll += OnEventResHeroAttrPotionUseAll;

		// System Message
		CsRplzSession.Instance.EventEvtSystemMessage += OnEventEvtSystemMessage;
	}

	//---------------------------------------------------------------------------------------------------
	void UnRegister()
    {
        // 날짜변경
        CsRplzSession.Instance.EventEvtDateChanged -= OnEventEvtDateChanged;

        // 메인장비
        CsRplzSession.Instance.EventResMainGearEquip -= OnEventResMainGearEquip;
        CsRplzSession.Instance.EventResMainGearUnequip -= OnEventResMainGearUnequip;
        CsRplzSession.Instance.EventResMainGearEnchant -= OnEventResMainGearEnchant;
        CsRplzSession.Instance.EventResMainGearTransit -= OnEventResMainGearTransit;
        CsRplzSession.Instance.EventResMainGearRefine -= OnEventResMainGearRefine;
        CsRplzSession.Instance.EventResMainGearRefinementApply -= OnEventResMainGearRefinementApply;
        CsRplzSession.Instance.EventResMainGearDisassemble -= OnEventResMainGearDisassemble;
        CsRplzSession.Instance.EventResMainGearEnchantLevelSetActivate -= OnEventResMainGearEnchantLevelSetActivate;

        // 보조장비
        CsRplzSession.Instance.EventResSubGearEquip -= OnEventResSubGearEquip;
        CsRplzSession.Instance.EventResSubGearUnequip -= OnEventResSubGearUnequip;
        CsRplzSession.Instance.EventResSoulstoneSocketMount -= OnEventResSoulstoneSocketMount;
        CsRplzSession.Instance.EventResSoulstoneSocketUnmount -= OnEventResSoulstoneSocketUnmount;
        CsRplzSession.Instance.EventResRuneSocketMount -= OnEventResRuneSocketMount;
        CsRplzSession.Instance.EventResRuneSocketUnmount -= OnEventResRuneSocketUnmount;
        CsRplzSession.Instance.EventResSubGearLevelUp -= OnEventResSubGearLevelUp;
        CsRplzSession.Instance.EventResSubGearLevelUpTotally -= OnEventResSubGearLevelUpTotally;
        CsRplzSession.Instance.EventResSubGearGradeUp -= OnEventResSubGearGradeUp;
        CsRplzSession.Instance.EventResSubGearQualityUp -= OnEventResSubGearQualityUp;
        CsRplzSession.Instance.EventResMountedSoulstoneCompose -= OnEventResMountedSoulstoneCompose;
        CsRplzSession.Instance.EventResSubGearSoulstoneLevelSetActivate -= OnEventResSubGearSoulstoneLevelSetActivate;

        // 메일
        CsRplzSession.Instance.EventResMailReceive -= OnEventResMailReceive;
        CsRplzSession.Instance.EventResMailReceiveAll -= OnEventResMailReceiveAll;
		CsRplzSession.Instance.EventResMailDelete -= OnEventResMailDelete;
		CsRplzSession.Instance.EventResMailDeleteAll -= OnEventResMailDeleteAll;
		CsRplzSession.Instance.EventEvtNewMail -= OnEventEvtNewMail;

        // 라크획득
        CsRplzSession.Instance.EventEvtLakAcquisition -= OnEventEvtLakAcquisition;
        // 경험치획득
        CsRplzSession.Instance.EventEvtExpAcquisition -= OnEventEvtExpAcquisition;

        // 스킬
        CsRplzSession.Instance.EventResSkillLevelUp -= OnEventResSkillLevelUp;
        CsRplzSession.Instance.EventResSkillLevelUpTotally -= OnEventResSkillLevelUpTotally;

        // 간이상점
        CsRplzSession.Instance.EventResSimpleShopBuy -= OnEventResSimpleShopBuy;
        CsRplzSession.Instance.EventResSimpleShopSell -= OnEventResSimpleShopSell;

        // 인벤토리
        CsRplzSession.Instance.EventResInventorySlotExtend -= OnEventResInventorySlotExtend;

        // 아이템
        CsRplzSession.Instance.EventResItemCompose -= OnEventResItemCompose;
        CsRplzSession.Instance.EventResItemComposeTotally -= OnEventResItemComposeTotally;
        CsRplzSession.Instance.EventResHpPotionUse -= OnEventResHpPotionUse;
        CsRplzSession.Instance.EventResReturnScrollUse -= OnEventResReturnScrollUse;
        CsRplzSession.Instance.EventEvtReturnScrollUseFinished -= OnEventEvtReturnScrollUseFinished;
        CsRplzSession.Instance.EventEvtReturnScrollUseCancel -= OnEventEvtReturnScrollUseCancel;
        CsRplzSession.Instance.EventResPickBoxUse -= OnEventResPickBoxUse;
        CsRplzSession.Instance.EventResMainGearBoxUse -= OnEventResMainGearBoxUse;
        CsRplzSession.Instance.EventResExpPotionUse -= OnEventResExpPotionUse;
        CsRplzSession.Instance.EventResExpScrollUse -= OnEventResExpScrollUse;
        CsRplzSession.Instance.EventResBountyHunterQuestScrollUse -= OnEventResBountyHunterQuestScrollUse;
        CsRplzSession.Instance.EventResFishingBaitUse -= OnEventResFishingBaitUse;
        CsRplzSession.Instance.EventResDistortionScrollUse -= OnEventResDistortionScrollUse;
        CsRplzSession.Instance.EventResDistortionCancel -= OnEventResDistortionCancel;
        CsRplzSession.Instance.EventEvtDistortionCanceled -= OnEventEvtDistortionCanceled;
        CsRplzSession.Instance.EventResGoldItemUse -= OnEventResGoldItemUse;
        CsRplzSession.Instance.EventResOwnDiaItemUse -= OnEventResOwnDiaItemUse;
        CsRplzSession.Instance.EventResHonorPointItemUse -= OnEventResHonorPointItemUse;
        CsRplzSession.Instance.EventResExploitPointItemUse -= OnEventResExploitPointItemUse;
		CsRplzSession.Instance.EventResWingItemUse -= OnEventResWingItemUse;


		// 휴식
		CsRplzSession.Instance.EventResRestRewardReceiveFree -= OnEventResRestRewardReceiveFree;
        CsRplzSession.Instance.EventResRestRewardReceiveGold -= OnEventResRestRewardReceiveGold;
        CsRplzSession.Instance.EventResRestRewardReceiveDia -= OnEventResRestRewardReceiveDia;

        // 부활
        CsRplzSession.Instance.EventResImmediateRevive -= OnEventResImmediateRevive;
        CsRplzSession.Instance.EventResContinentSaftyRevive -= OnEventResContinentSaftyRevive;

        // 아이템 루팅
        CsRplzSession.Instance.EventEvtDropObjectLooted -= OnEventEvtDropObjectLooted;

        // 파티
        CsRplzSession.Instance.EventResPartySurroundingHeroList -= OnEventResPartySurroundingHeroList;
        CsRplzSession.Instance.EventResPartySurroundingPartyList -= OnEventResPartySurroundingPartyList;
        CsRplzSession.Instance.EventResPartyCreate -= OnEventResPartyCreate;
        CsRplzSession.Instance.EventResPartyExit -= OnEventResPartyExit;
        CsRplzSession.Instance.EventResPartyMemberBanish -= OnEventResPartyMemberBanish;
        CsRplzSession.Instance.EventResPartyCall -= OnEventResPartyCall;
        CsRplzSession.Instance.EventResPartyDisband -= OnEventResPartyDisband;
        CsRplzSession.Instance.EventResPartyApply -= OnEventResPartyApply;
        CsRplzSession.Instance.EventResPartyApplicationAccept -= OnEventResPartyApplicationAccept;
        CsRplzSession.Instance.EventResPartyApplicationRefuse -= OnEventResPartyApplicationRefuse;
        CsRplzSession.Instance.EventResPartyInvite -= OnEventResPartyInvite;
        CsRplzSession.Instance.EventResPartyInvitationAccept -= OnEventResPartyInvitationAccept;
        CsRplzSession.Instance.EventResPartyInvitationRefuse -= OnEventResPartyInvitationRefuse;
        CsRplzSession.Instance.EventResPartyMasterChange -= OnEventResPartyMasterChange;

        CsRplzSession.Instance.EventEvtPartyApplicationArrived -= OnEventEvtPartyApplicationArrived;
        CsRplzSession.Instance.EventEvtPartyApplicationCanceled -= OnEventEvtPartyApplicationCanceled;
        CsRplzSession.Instance.EventEvtPartyApplicationAccepted -= OnEventEvtPartyApplicationAccepted;
        CsRplzSession.Instance.EventEvtPartyApplicationRefused -= OnEventEvtPartyApplicationRefused;
        CsRplzSession.Instance.EventEvtPartyApplicationLifetimeEnded -= OnEventEvtPartyApplicationLifetimeEnded;

        CsRplzSession.Instance.EventEvtPartyInvitationArrived -= OnEventEvtPartyInvitationArrived;
        CsRplzSession.Instance.EventEvtPartyInvitationCanceled -= OnEventEvtPartyInvitationCanceled;
        CsRplzSession.Instance.EventEvtPartyInvitationAccepted -= OnEventEvtPartyInvitationAccepted;
        CsRplzSession.Instance.EventEvtPartyInvitationRefused -= OnEventEvtPartyInvitationRefused;
        CsRplzSession.Instance.EventEvtPartyInvitationLifetimeEnded -= OnEventEvtPartyInvitationLifetimeEnded;

        CsRplzSession.Instance.EventEvtPartyMemberEnter -= OnEventEvtPartyMemberEnter;
        CsRplzSession.Instance.EventEvtPartyMemberExit -= OnEventEvtPartyMemberExit;
        CsRplzSession.Instance.EventEvtPartyBanished -= OnEventEvtPartyBanished;
        CsRplzSession.Instance.EventEvtPartyMasterChanged -= OnEventEvtPartyMasterChanged;
        CsRplzSession.Instance.EventEvtPartyCall -= OnEventEvtPartyCall;
        CsRplzSession.Instance.EventEvtPartyDisbanded -= OnEventEvtPartyDisbanded;
        CsRplzSession.Instance.EventEvtPartyMembersUpdated -= OnEventEvtPartyMembersUpdated;

        // 채팅
        CsRplzSession.Instance.EventResChattingMessageSend -= OnEventResChattingMessageSend;
        CsRplzSession.Instance.EventEvtChattingMessageReceived -= OnEventEvtChattingMessageReceived;

        // 지원
        CsRplzSession.Instance.EventResLevelUpRewardReceive -= OnEventResLevelUpRewardReceive;
        CsRplzSession.Instance.EventResDailyAccessTimeRewardReceive -= OnEventResDailyAccessTimeRewardReceive;
        CsRplzSession.Instance.EventResAttendRewardReceive -= OnEventResAttendRewardReceive;
        CsRplzSession.Instance.EventResSeriesMissionRewardReceive -= OnEventResSeriesMissionRewardReceive;
        CsRplzSession.Instance.EventResTodayMissionRewardReceive -= OnEventResTodayMissionRewardReceive;
        CsRplzSession.Instance.EventEvtSeriesMissionUpdated -= OnEventEvtSeriesMissionUpdated;
        CsRplzSession.Instance.EventEvtTodayMissionListChanged -= OnEventEvtTodayMissionListChanged;
        CsRplzSession.Instance.EventEvtTodayMissionUpdated -= OnEventEvtTodayMissionUpdated;
		CsRplzSession.Instance.EventResRookieGiftReceive -= OnEventResRookieGiftReceive;
		CsRplzSession.Instance.EventResOpenGiftReceive -= OnEventResOpenGiftReceive;

		// 탈것
		CsRplzSession.Instance.EventResMountEquip -= OnEventResMountEquip;
        CsRplzSession.Instance.EventResMountLevelUp -= OnEventResMountLevelUp;
        CsRplzSession.Instance.EventResMountGetOn -= OnEventResMountGetOn;
		CsRplzSession.Instance.EventResMountAwakeningLevelUp -= OnEventResMountAwakeningLevelUp;
		CsRplzSession.Instance.EventResMountAttrPotionUse -= OnEventResMountAttrPotionUse;
		CsRplzSession.Instance.EventResMountItemUse -= OnEventResMountItemUse;

		// 탈것장비
		CsRplzSession.Instance.EventResMountGearEquip -= OnEventResMountGearEquip;
        CsRplzSession.Instance.EventResMountGearUnequip -= OnEventResMountGearUnequip;
        CsRplzSession.Instance.EventResMountGearRefine -= OnEventResMountGearRefine;
        CsRplzSession.Instance.EventResMountGearPickBoxMake -= OnEventResMountGearPickBoxMake;
        CsRplzSession.Instance.EventResMountGearPickBoxMakeTotally -= OnEventResMountGearPickBoxMakeTotally;

        // 영웅
        CsRplzSession.Instance.EventResHeroInfo -= OnEventResHeroInfo;
		CsRplzSession.Instance.EventResHeroPosition -= OnEventResHeroPosition;

        // 날개
        CsRplzSession.Instance.EventResWingEquip -= OnEventResWingEquip;
        CsRplzSession.Instance.EventResWingEnchant -= OnEventResWingEnchant;
        CsRplzSession.Instance.EventResWingEnchantTotally -= OnEventResWingEnchantTotally;
        CsRplzSession.Instance.EventEvtWingAcquisition -= OnEventEvtWingAcquisition;
		CsRplzSession.Instance.EventResWingMemoryPieceInstall -= OnEventResWingMemoryPieceInstall;

		// 스토리던전
		//CsRplzSession.Instance.EventResContinentExitForStoryDungeonEnter -= OnEventResContinentExitForStoryDungeonEnter;
		//CsRplzSession.Instance.EventResStoryDungeonAbandon -= OnEventResStoryDungeonAbandon;
		//CsRplzSession.Instance.EventResStoryDungeonExit -= OnEventResStoryDungeonExit;
		//CsRplzSession.Instance.EventResStoryDungeonRevive -= OnEventResStoryDungeonRevive;
		//CsRplzSession.Instance.EventResStoryDungeonSweep -= OnEventResStoryDungeonSweep;

		// 스테미나
		CsRplzSession.Instance.EventEvtStaminaAutoRecovery -= OnEventEvtStaminaAutoRecovery;
        CsRplzSession.Instance.EventResStaminaBuy -= OnEventResStaminaBuy;
        CsRplzSession.Instance.EventEvtStaminaScheduleRecovery -= OnEventEvtStaminaScheduleRecovery;

        // 대륙전송
        CsRplzSession.Instance.EventResContinentTransmission -= OnEventResContinentTransmission;

        // 국가
        CsRplzSession.Instance.EventResNationTransmission -= OnEventResNationTransmission;
        CsRplzSession.Instance.EventResHeroSearch -= OnEventResHeroSearch;
        CsRplzSession.Instance.EventResNationDonate -= OnEventResNationDonate;
        CsRplzSession.Instance.EventResNationNoblesseAppoint -= OnEventResNationNoblesseAppoint;
        CsRplzSession.Instance.EventResNationNoblesseDismiss -= OnEventResNationNoblesseDismiss;
        CsRplzSession.Instance.EventEvtNationNoblesseAppointment -= OnEventEvtNationNoblesseAppointment;
        CsRplzSession.Instance.EventEvtNationNoblesseDismissal -= OnEventEvtNationNoblesseDismissal;
        CsRplzSession.Instance.EventEvtNationFundChanged -= OnEventEvtNationFundChanged;
        CsRplzSession.Instance.EventResNationCall -= OnEventResNationCall;
        CsRplzSession.Instance.EventResNationCallTransmission -= OnEventResNationCallTransmission;
        CsRplzSession.Instance.EventEvtNationCall -= OnEventEvtNationCall;

		// 오늘의 할일
		CsRplzSession.Instance.EventResAchievementRewardReceive -= OnEventResAchievementRewardReceive;
        CsRplzSession.Instance.EventEvtTodayTaskUpdated -= OnEventEvtTodayTaskUpdated;

        // VIP
        CsRplzSession.Instance.EventResVipLevelRewardReceive -= OnEventResVipLevelRewardReceive;

        // 계급
        CsRplzSession.Instance.EventResRankAcquire -= OnEventResRankAcquire;
        CsRplzSession.Instance.EventResRankRewardReceive -= OnEventResRankRewardReceive;
		CsRplzSession.Instance.EventResRankActiveSkillLevelUp -= OnEventResRankActiveSkillLevelUp;
		CsRplzSession.Instance.EventResRankActiveSkillSelect -= OnEventResRankActiveSkillSelect;
		CsRplzSession.Instance.EventResRankPassiveSkillLevelUp -= OnEventResRankPassiveSkillLevelUp;

		// 명예상점
		CsRplzSession.Instance.EventResHonorShopProductBuy -= OnEventResHonorShopProductBuy;

        // 랭킹
        CsRplzSession.Instance.EventResServerBattlePowerRanking -= OnEventResServerBattlePowerRanking;
        CsRplzSession.Instance.EventResServerJobBattlePowerRanking -= OnEventResServerJobBattlePowerRanking;
        CsRplzSession.Instance.EventResServerLevelRanking -= OnEventResServerLevelRanking;
        CsRplzSession.Instance.EventResServerLevelRankingRewardReceive -= OnEventResServerLevelRankingRewardReceive;
        CsRplzSession.Instance.EventResNationBattlePowerRanking -= OnEventResNationBattlePowerRanking;
        CsRplzSession.Instance.EventResNationExploitPointRanking -= OnEventResNationExploitPointRanking;
        CsRplzSession.Instance.EventEvtDailyServerLevelRankingUpdated -= OnEventEvtDailyServerLevelRankingUpdated;
        CsRplzSession.Instance.EventResServerCreatureCardRanking -= OnEventResServerCreatureCardRanking;
        CsRplzSession.Instance.EventResServerIllustratedBookRanking -= OnEventResServerIllustratedBookRanking;
		CsRplzSession.Instance.EventEvtDailyServerNationPowerRankingUpdated -= OnEventEvtDailyServerNationPowerRankingUpdated;

		// 도달
		CsRplzSession.Instance.EventResAttainmentRewardReceive -= OnEventResAttainmentRewardReceive;

        // 대륙
        CsRplzSession.Instance.EventEvtContinentBanished -= OnEventEvtContinentBanished;

        // 세팅
        CsRplzSession.Instance.EventResBattleSettingSet -= OnEventResBattleSettingSet;

        // 공지
        CsRplzSession.Instance.EventEvtNotice -= OnEventEvtNotice;

		// 오늘의미션튜토리얼시작
        CsRplzSession.Instance.EventResTodayMissionTutorialStart -= OnEventResTodayMissionTutorialStart;

		// 서버최고레벨갱신
		CsRplzSession.Instance.EventEvtServerMaxLevelUpdated -= OnEventEvtServerMaxLevelUpdated;

        CsRplzSession.Instance.EventResGroggyMonsterItemStealStart -= OnEventResGroggyMonsterItemStealStart;
        CsRplzSession.Instance.EventEvtGroggyMonsterItemStealCancel -= OnEventEvtGroggyMonsterItemStealCancel;
        CsRplzSession.Instance.EventEvtGroggyMonsterItemStealFinished -= OnEventEvtGroggyMonsterItemStealFinished;

		// 영웅NPC상점상품
		CsRplzSession.Instance.EventResNpcShopProductBuy -= OnEventResNpcShopProductBuy;

		// 오픈7일이벤트
		CsRplzSession.Instance.EventResOpen7DayEventMissionRewardReceive -= OnEventResOpen7DayEventMissionRewardReceive;
		CsRplzSession.Instance.EventResOpen7DayEventProductBuy -= OnEventResOpen7DayEventProductBuy;
		CsRplzSession.Instance.EventResOpen7DayEventRewardReceive -= OnEventResOpen7DayEventRewardReceive;
		CsRplzSession.Instance.EventEvtOpen7DayEventProgressCountUpdated -= OnEventEvtOpen7DayEventProgressCountUpdated;

		// 회수
		CsRplzSession.Instance.EventResRetrieveGold -= OnEventResRetrieveGold;
		CsRplzSession.Instance.EventResRetrieveGoldAll -= OnEventResRetrieveGoldAll;
		CsRplzSession.Instance.EventResRetrieveDia -= OnEventResRetrieveDia;
		CsRplzSession.Instance.EventResRetrieveDiaAll -= OnEventResRetrieveDiaAll;
		CsRplzSession.Instance.EventEvtRetrievalProgressCountUpdated -= OnEventEvtRetrievalProgressCountUpdated;

		// 할일위탁
		CsRplzSession.Instance.EventResTaskConsignmentStart -= OnEventResTaskConsignmentStart;
		CsRplzSession.Instance.EventResTaskConsignmentComplete -= OnEventResTaskConsignmentComplete;
		CsRplzSession.Instance.EventResTaskConsignmentImmediatelyComplete -= OnEventResTaskConsignmentImmediatelyComplete;

		// 한정선물
		CsRplzSession.Instance.EventResLimitationGiftRewardReceive -= OnEventResLimitationGiftRewardReceive;

		// 주말보상
		CsRplzSession.Instance.EventResWeekendRewardSelect -= OnEventResWeekendRewardSelect;
		CsRplzSession.Instance.EventResWeekendRewardReceive -= OnEventResWeekendRewardReceive;

		// 창고
		CsRplzSession.Instance.EventResWarehouseDeposit -= OnEventResWarehouseDeposit;
		CsRplzSession.Instance.EventResWarehouseWithdraw -= OnEventResWarehouseWithdraw;
		CsRplzSession.Instance.EventResWarehouseSlotExtend -= OnEventResWarehouseSlotExtend;

		//다이아상점
		CsRplzSession.Instance.EventResDiaShopProductBuy -= OnEventResDiaShopProductBuy;

		// 성물 수집
		CsRplzSession.Instance.EventResFearAltarHalidomElementalRewardReceive -= OnEventResFearAltarHalidomElementalRewardReceive;
		CsRplzSession.Instance.EventResFearAltarHalidomCollectionRewardReceive -= OnEventResFearAltarHalidomCollectionRewardReceive;
		CsRplzSession.Instance.EventEvtFearAltarHalidomAcquisition -= OnEventEvtFearAltarHalidomAcquisition;

        // 포탈
        CsRplzSession.Instance.EventResPortalEnter -= OnEventResPortalEnter;

        // 계정중복로그인
        CsRplzSession.Instance.EventEvtAccountLoginDuplicated += OnEventEvtAccountLoginDuplicated;

		// 물약속성
		CsRplzSession.Instance.EventResHeroAttrPotionUse -= OnEventResHeroAttrPotionUse;
		CsRplzSession.Instance.EventResHeroAttrPotionUseAll -= OnEventResHeroAttrPotionUseAll;

		// System Message
		CsRplzSession.Instance.EventEvtSystemMessage -= OnEventEvtSystemMessage;
	}

	#region DateChanged

	void OnEventEvtDateChanged(SEBDateChangedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.TimeOffset = eventBody.time;
        CsGameEventUIToUI.Instance.OnEventDateChanged();
    }

    #endregion DateChanged

    #region Main Gear

    //---------------------------------------------------------------------------------------------------
    // 메인장비장착
    public void SendMainGearEquip(Guid guidHeroMainGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearEquipCommandBody cmdBody = new MainGearEquipCommandBody();
            cmdBody.heroMainGearId = m_guildHeroGearId = guidHeroMainGearId;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearEquip, cmdBody);
        }
    }

    void OnEventResMainGearEquip(int nReturnCode, MainGearEquipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 인벤토리슬롯정보
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByHeroMainGearId(m_guildHeroGearId);

            // 기존장착 정보를 저장하고 새로운 장착정보로 변경.
            int nIndex = csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearType.EquippedIndex;
            CsHeroMainGear csHeroMainGearEquipped = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[nIndex];
            csInventorySlot.InventoryObjectMainGear.HeroMainGear.Owned = responseBody.mainGearOwned;
            CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[nIndex] = csInventorySlot.InventoryObjectMainGear.HeroMainGear;

            // 최대장착메인장비강화레벨
            CsAccomplishmentManager.Instance.MaxEquippedMainGearEnchantLevel = responseBody.maxEquippedMainGearEnchantLevel;

            // 인벤토리 변경 - 기존 인벤토리 삭제.
            CsGameData.Instance.MyHeroInfo.RemoveInventorySlot(csInventorySlot, false);

            // 기존장착 메인장비가 있을 경우 인벤토리 슬롯에 추가
            if (csHeroMainGearEquipped != null && responseBody.changedInventorySlotIndex > -1)
            {
                CsInventoryObjectMainGear csInventoryObjectMainGear = new CsInventoryObjectMainGear((int)EnInventoryObjectType.MainGear, csHeroMainGearEquipped.Id);
                CsInventorySlot csInventorySlotNew = new CsInventorySlot(responseBody.changedInventorySlotIndex, csInventoryObjectMainGear);
                CsGameData.Instance.MyHeroInfo.AddInventorySlot(csInventorySlotNew);
            }

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            // Ingame에 변경이벤트 보냄.
            CsGameEventToIngame.Instance.OnEventMainGearChanged(CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[nIndex].MainGear.MainGearType.MainGearCategory.EnMainGearCategory, CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[nIndex]);

            CsGameEventUIToUI.Instance.OnEventMainGearEquip(m_guildHeroGearId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비장착해체
    public void SendMainGearUnequip(Guid guidHeroMainGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearUnequipCommandBody cmdBody = new MainGearUnequipCommandBody();
            cmdBody.heroMainGearId = m_guildHeroGearId = guidHeroMainGearId;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearUnequip, cmdBody);
        }
    }

    void OnEventResMainGearUnequip(int nReturnCode, MainGearUnequipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 기존장착장비
            CsHeroMainGear csHeroMainGearEquipped = CsGameData.Instance.MyHeroInfo.GetHeroMainGearEquipped(m_guildHeroGearId);

            // 장착정보 리셋
            int nIndex = csHeroMainGearEquipped.MainGear.MainGearType.EquippedIndex;
            CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[nIndex] = null;

            // 인벤토리 추가
            CsInventoryObjectMainGear csInventoryObjectMainGear = new CsInventoryObjectMainGear((int)EnInventoryObjectType.MainGear, csHeroMainGearEquipped.Id);
            CsInventorySlot csInventorySlotNew = new CsInventorySlot(responseBody.changedInventorySlotIndex, csInventoryObjectMainGear);
            CsGameData.Instance.MyHeroInfo.AddInventorySlot(csInventorySlotNew);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            // Ingame에 변경이벤트 보냄.
            CsGameEventToIngame.Instance.OnEventMainGearChanged(csHeroMainGearEquipped.MainGear.MainGearType.MainGearCategory.EnMainGearCategory, null);

            CsGameEventUIToUI.Instance.OnEventMainGearUnequip(m_guildHeroGearId);
        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00101"));
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비강화
    public void SendMainGearEnchant(Guid guidHeroMainGearId, bool bUsePenaltyPreventItem)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearEnchantCommandBody cmdBody = new MainGearEnchantCommandBody();
            cmdBody.heroMainGearId = m_guildHeroGearId = guidHeroMainGearId;
            cmdBody.usePenaltyPreventItem = bUsePenaltyPreventItem;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearEnchant, cmdBody);
        }
    }

    void OnEventResMainGearEnchant(int nReturnCode, MainGearEnchantResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.MainGearEnchantDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.MainGearEnchantDailyCount = responseBody.enchantDailyCount;

            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guildHeroGearId);
            csHeroMainGear.EnchantLevel = responseBody.enchantLevel;

            // 최대장착메인장비강화레벨
            CsAccomplishmentManager.Instance.MaxEquippedMainGearEnchantLevel = responseBody.maxEquippedMainGearEnchantLevel;

            // 인벤토리변경 
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMainGearEnchant(responseBody.isSuccess, m_guildHeroGearId);
        }
        else if (nReturnCode == 101)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00701"));
        }
        else if (nReturnCode == 102)
        {
            // 패널티방지아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00702"));
        }
        else if (nReturnCode == 103)
        {
            // 일일최대강화횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00703"));
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비전이
    public void SendMainGearTransit(Guid guidTargetHeroMainGearId, Guid guidMaterialHeroMainGearId, bool bIsEnchantLevelTransit, bool bIsOptionAttrTransit)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearTransitCommandBody cmdBody = new MainGearTransitCommandBody();
            cmdBody.targetHeroMainGearId = m_guildHeroGearId = guidTargetHeroMainGearId;
            cmdBody.materialHeroMainGearId = m_guildMaterialHeroGearId = guidMaterialHeroMainGearId;
            cmdBody.isEnchantLevelTransit = bIsEnchantLevelTransit;
            cmdBody.isOptionAttrTransit = bIsOptionAttrTransit;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearTransit, cmdBody);
        }
    }

    void OnEventResMainGearTransit(int nReturnCode, MainGearTransitResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 타켓장비
            CsHeroMainGear csHeroMainGearTarget = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guildHeroGearId);
            csHeroMainGearTarget.Owned = responseBody.targetOwned;
            csHeroMainGearTarget.EnchantLevel = responseBody.targetEnchantLevel;
            csHeroMainGearTarget.AddOptionAttributes(responseBody.targetOptionAttrs);

            // 재료장비
            CsHeroMainGear csHeroMainGearMaterial = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guildMaterialHeroGearId);
            csHeroMainGearMaterial.Owned = responseBody.materialOwned;
            csHeroMainGearMaterial.EnchantLevel = responseBody.materialEnchantLevel;
            csHeroMainGearMaterial.AddOptionAttributes(responseBody.materialOptionAttrs);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMainGearTransit(m_guildHeroGearId, m_guildMaterialHeroGearId);
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비세련
    public void SendMainGearRefine(Guid guidHeroMainGearId, bool bSingleRefine, int[] anProtectedIndices)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearRefineCommandBody cmdBody = new MainGearRefineCommandBody();
            cmdBody.heroMainGearId = m_guildHeroGearId = guidHeroMainGearId;
            cmdBody.isSingleRefinement = bSingleRefine;
            cmdBody.protectedIndices = anProtectedIndices;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearRefine, cmdBody);
        }
    }

    void OnEventResMainGearRefine(int nReturnCode, MainGearRefineResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guildHeroGearId);
            csHeroMainGear.AddHeroMainGearRefinements(responseBody.refinements);

            // 세련 날짜 및 횟수 변경
            CsGameData.Instance.MyHeroInfo.MainGearRefineDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.MainGearRefineDailyCount = responseBody.refinementDailyCount;

            // 인벤토리변경 
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventMainGearRefine(m_guildHeroGearId);

        }
        else if (nReturnCode == 101)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_001901"));
        }
        else if (nReturnCode == 102)
        {
            // 보호아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_001902"));
        }
        else if (nReturnCode == 103)
        {
            // 일일최대세련횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_001903"));
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비세련적용
    public void SendMainGearRefinementApply(Guid guidHeroMainGearId, int nTurn)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearRefinementApplyCommandBody cmdBody = new MainGearRefinementApplyCommandBody();
            cmdBody.heroMainGearId = m_guildHeroGearId = guidHeroMainGearId;
            cmdBody.turn = nTurn;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearRefinementApply, cmdBody);
        }
    }

    void OnEventResMainGearRefinementApply(int nReturnCode, MainGearRefinementApplyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(m_guildHeroGearId);
            csHeroMainGear.AddOptionAttributes(responseBody.optionAttrs);
            csHeroMainGear.ClearHeroMainGearRefinements();

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMainGearRefinementApply(m_guildHeroGearId);
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메인장비분해
    public void SendMainGearDisassemble(Guid[] aguids)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearDisassembleCommandBody cmdBody = new MainGearDisassembleCommandBody();
            cmdBody.targetHeroMainGearIds = aguids;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearDisassemble, cmdBody);
        }
    }

    void OnEventResMainGearDisassemble(int nReturnCode, MainGearDisassembleResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventMainGearDisassemble();
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    //메인장비강화레벨세트활성

    public void SendMainGearEnchantLevelSetActivate(int nSetNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearEnchantLevelSetActivateCommandBody cmdBody = new MainGearEnchantLevelSetActivateCommandBody();
            cmdBody.setNo = m_nMainGearSetNo = nSetNo;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearEnchantLevelSetActivate, cmdBody);
        }
    }

    void OnEventResMainGearEnchantLevelSetActivate(int nReturnCode, MainGearEnchantLevelSetActivateResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.MainGearEnchantLevelSetNo = m_nMainGearSetNo;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMainGearEnchantLevelSetActivate();
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Main Gear 

    #region Sub Gear

    //---------------------------------------------------------------------------------------------------
    // 보조장비장착
    public void SendSubGearEquip(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearEquipCommandBody cmdBody = new SubGearEquipCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearEquip, cmdBody);
        }
    }

    void OnEventResSubGearEquip(int nReturnCode, SubGearEquipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 인벤토리슬롯정보
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotBySubGearId(m_nSubGearId);

            // 인벤토리 삭제.
            CsGameData.Instance.MyHeroInfo.RemoveInventorySlot(csInventorySlot, false);

            // 보조장비 장착정보변경
            CsGameData.Instance.MyHeroInfo.UpdateHeroSubGearEquipped(m_nSubGearId, true);

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearEquip(m_nSubGearId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 보조장비장착해체
    public void SendSubGearUnequip(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearUnequipCommandBody cmdBody = new SubGearUnequipCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearUnequip, cmdBody);
        }
    }

    void OnEventResSubGearUnequip(int nReturnCode, SubGearUnequipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 장착정보 변경
            CsGameData.Instance.MyHeroInfo.UpdateHeroSubGearEquipped(m_nSubGearId, false);

            // 인벤토리 추가
            CsInventoryObjectSubGear csInventoryObjectSubGear = new CsInventoryObjectSubGear((int)EnInventoryObjectType.SubGear, m_nSubGearId);
            CsInventorySlot csInventorySlotNew = new CsInventorySlot(responseBody.changedInventorySlotIndex, csInventoryObjectSubGear);
            CsGameData.Instance.MyHeroInfo.AddInventorySlot(csInventorySlotNew);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearUnequip(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00201"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 소울스톤소켓장착
    public void SendSoulstoneSocketMount(int nSubGearId, int nSocketIndex, int nItemId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SoulstoneSocketMountCommandBody cmdBody = new SoulstoneSocketMountCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            cmdBody.socketIndex = m_nSocketIndex = nSocketIndex;
            cmdBody.itemId = m_nItemId = nItemId;
            CsRplzSession.Instance.Send(ClientCommandName.SoulstoneSocketMount, cmdBody);
        }
    }

    void OnEventResSoulstoneSocketMount(int nReturnCode, SoulstoneSocketMountResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 소울스톤소켓마운트
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.AddSoulstoneSocket(m_nSocketIndex, m_nItemId);

            // 인벤토리 변경
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSoulstoneSocketMount(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 아직 개방되지않은 소켓입니다. 
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00301"));
        }
        else if (nReturnCode == 102)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00302"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 소울스톤소켓장착해제
    public void SendSoulstoneSocketUnmount(int nSubGearId, int nSocketIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SoulstoneSocketUnmountCommandBody cmdBody = new SoulstoneSocketUnmountCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            cmdBody.socketIndex = m_nSocketIndex = nSocketIndex;
            CsRplzSession.Instance.Send(ClientCommandName.SoulstoneSocketUnmount, cmdBody);
        }
    }

    void OnEventResSoulstoneSocketUnmount(int nReturnCode, SoulstoneSocketUnmountResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 소울스톤소켓언마운트
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.RemoveSoulstoneSocket(m_nSocketIndex);

            // 인벤토리 변경
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSoulstoneSocketUnmount(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00401"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 룬소켓장착
    public void SendRuneSocketMount(int nSubGearId, int nSocketIndex, int nItemId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RuneSocketMountCommandBody cmdBody = new RuneSocketMountCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            cmdBody.socketIndex = m_nSocketIndex = nSocketIndex;
            cmdBody.itemId = m_nItemId = nItemId;
            CsRplzSession.Instance.Send(ClientCommandName.RuneSocketMount, cmdBody);
        }
    }

    void OnEventResRuneSocketMount(int nReturnCode, RuneSocketMountResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.AddRuneSocket(m_nSocketIndex, m_nItemId);

            // 변경인벤토리 슬롯.
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventRuneSocketMount(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 아직 개방되지않은 소켓입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00501"));
        }
        else if (nReturnCode == 102)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00502"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 룬소켓장착해제
    public void SendRuneSocketUnmount(int nSubGearId, int nSocketIndex, int nItemId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RuneSocketUnmountCommandBody cmdBody = new RuneSocketUnmountCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            cmdBody.socketIndex = m_nSocketIndex = nSocketIndex;
            CsRplzSession.Instance.Send(ClientCommandName.RuneSocketUnmount, cmdBody);
        }
    }

    void OnEventResRuneSocketUnmount(int nReturnCode, RuneSocketUnmountResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.RemoveRuneSocket(m_nSocketIndex);

            // 변경인벤토리 슬롯.
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventRuneSocketUnmount(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00601"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 보조장비레벨업
    public void SendSubGearLevelUp(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearLevelUpCommandBody cmdBody = new SubGearLevelUpCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearLevelUp, cmdBody);
        }
    }

    void OnEventResSubGearLevelUp(int nReturnCode, SubGearLevelUpResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.Level = responseBody.subGearLevel;
            csHeroSubGear.Quality = responseBody.subGearQuality;

            // 골드
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearLevelUp(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 최대레벨입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 품질업이 필요합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 등급업이 필요합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            //  골드가 부족합니다. 
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00104"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 보조장비전체레벨업
    public void SendSubGearLevelUpTotally(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearLevelUpTotallyCommandBody cmdBody = new SubGearLevelUpTotallyCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearLevelUpTotally, cmdBody);
        }
    }

    void OnEventResSubGearLevelUpTotally(int nReturnCode, SubGearLevelUpTotallyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.Level = responseBody.subGearLevel;
            csHeroSubGear.Quality = responseBody.subGearQuality;

            // 골드
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearLevelUpTotally(m_nSubGearId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 보조장비등급업
    public void SendSubGearGradeUp(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearGradeUpCommandBody cmdBody = new SubGearGradeUpCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearGradeUp, cmdBody);
        }
    }

    void OnEventResSubGearGradeUp(int nReturnCode, SubGearGradeUpResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.Level = responseBody.subGearLevel;
            csHeroSubGear.Quality = responseBody.subGearQuality;

            // 인베톤리 변경.
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearGradeUp(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 최대레벨입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00301"));
        }
        else if (nReturnCode == 102)
        {
            // 품질업이 필요합니다. 
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00302"));
        }
        else if (nReturnCode == 103)
        {
            // 현재 등급의 최대레벨이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00303"));
        }
        else if (nReturnCode == 104)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00304"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 보조장비품질업
    public void SendSubGearQualityUp(int nSubGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearQualityUpCommandBody cmdBody = new SubGearQualityUpCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearQualityUp, cmdBody);
        }
    }

    void OnEventResSubGearQualityUp(int nReturnCode, SubGearQualityUpResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            csHeroSubGear.Quality = responseBody.subGearQuality;

            // 인베톤리 변경.
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearQualityUp(m_nSubGearId);
        }
        else if (nReturnCode == 101)
        {
            // 이미 최대품질입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00401"));
        }
        else if (nReturnCode == 102)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A09_ERROR_00402"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 장착된소울스톤합성
    public void SendMountedSoulstoneCompose(int nSubGearId, int nSocketIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountedSoulstoneComposeCommandBody cmdBody = new MountedSoulstoneComposeCommandBody();
            cmdBody.subGearId = m_nSubGearId = nSubGearId;
            cmdBody.slotIndex = m_nSocketIndex = nSocketIndex;
            CsRplzSession.Instance.Send(ClientCommandName.MountedSoulstoneCompose, cmdBody);
        }
    }

    void OnEventResMountedSoulstoneCompose(int nReturnCode, MountedSoulstoneComposeResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(m_nSubGearId);
            CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = csHeroSubGear.GetHeroSubGearSoulstoneSocket(m_nSocketIndex);

            CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(csHeroSubGearSoulstoneSocket.Item.ItemId);
            csHeroSubGearSoulstoneSocket.Item = CsGameData.Instance.GetItem(csItemCompositionRecipe.Item.ItemId);

            // 인베톤리 변경.
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            // 나의 영웅정보(MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMountedSoulstoneCompose(m_nSubGearId, m_nSocketIndex);
        }
        else if (nReturnCode == 101)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A10_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A10_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    //보조장비소울스톤레벨세트활성
    public void SendSubGearSoulstoneLevelSetActivate(int nSetNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SubGearSoulstoneLevelSetActivateCommandBody cmdBody = new SubGearSoulstoneLevelSetActivateCommandBody();
            cmdBody.setNo = m_nSubGearSetNo = nSetNo;
            CsRplzSession.Instance.Send(ClientCommandName.SubGearSoulstoneLevelSetActivate, cmdBody);
        }
    }

    void OnEventResSubGearSoulstoneLevelSetActivate(int nReturnCode, SubGearSoulstoneLevelSetActivateResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.SubGearSoulstoneLevelSetNo = m_nSubGearSetNo;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSubGearSoulstoneLevelSetActivate();
        }
        else
        {
            // 에러...
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Sub Gear 

    #region Mail

    //---------------------------------------------------------------------------------------------------
    // 메일받기
    public void SendMailReceive(Guid guid)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MailReceiveCommandBody cmdBody = new MailReceiveCommandBody();
            cmdBody.mailId = m_guidMailId = guid;
            CsRplzSession.Instance.Send(ClientCommandName.MailReceive, cmdBody);
        }
    }

    void OnEventResMailReceive(int nReturnCode, MailReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
			CsMail csMail = CsGameData.Instance.MyHeroInfo.GetMail(m_guidMailId);
			csMail.Received = true;

			// 인벤토리 목록 추가
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventMailReceive(m_guidMailId);
        }
        else if (nReturnCode == 101)
        {
            // 메일이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00102"));
        }
		else if (nReturnCode == 103)
		{
			// 메일첨부가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 이미 첨부를 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00104"));
		}
		else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 메일전체받기
    public void SendMailReceiveAll()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MailReceiveAllCommandBody cmdBody = new MailReceiveAllCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.MailReceiveAll, cmdBody);
        }
    }

    void OnEventResMailReceiveAll(int nReturnCode, MailReceiveAllResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            for (int i = 0; i < responseBody.receivedMails.Length; i++)
			{
				CsMail csMail = CsGameData.Instance.MyHeroInfo.GetMail(responseBody.receivedMails[i]);
				csMail.Received = true;
			}

            // 인벤토리 목록 추가
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventMailReceiveAll(responseBody.receivedMails);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 메일삭제
	public void SendMailDelete(Guid guid)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			MailDeleteCommandBody cmdBody = new MailDeleteCommandBody();
			cmdBody.mailId = m_guidMailId = guid;
			CsRplzSession.Instance.Send(ClientCommandName.MailDelete, cmdBody);
		}
	}

	void OnEventResMailDelete(int nReturnCode, MailDeleteResposneBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			// 받은 메일 삭제
			CsGameData.Instance.MyHeroInfo.RemoveMail(m_guidMailId);

			CsGameEventUIToUI.Instance.OnEventMailDelete(m_guidMailId);
		}
		else if (nReturnCode == 101)
		{
			// 메일이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			//  메일첨부를 받지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A03_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 메일전체삭제
	public void SendMailDeleteAll()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			MailDeleteAllCommandBody cmdBody = new MailDeleteAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MailDeleteAll, cmdBody);
		}
	}

	void OnEventResMailDeleteAll(int nReturnCode, MailDeleteAllResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			// 받은 메일 삭제
			CsGameData.Instance.MyHeroInfo.MailList.Clear();

			CsGameEventUIToUI.Instance.OnEventMailDeleteAll();
		}
		else if (nReturnCode == 101)
		{
			// 첨부를 받지않은 메일이 존재합니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A03_TXT_02003"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 신규메일
	void OnEventEvtNewMail(SEBNewMailEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.AddMail(eventBody.mail);

        CsGameEventUIToUI.Instance.OnEventNewMail(eventBody.mail.id);
    }

    #endregion Mail

    #region Lak, Exp
    //---------------------------------------------------------------------------------------------------
    // 라크획득
    void OnEventEvtLakAcquisition(SEBLakAcquisitionEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.Lak = eventBody.lak;
    }

    //---------------------------------------------------------------------------------------------------
    // 경험치획득
    void OnEventEvtExpAcquisition(SEBExpAcquisitionEventBody eventBody)
    {
        int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
        CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
        CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

        CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

        bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

        CsGameEventUIToUI.Instance.OnEventExpAcquisition(eventBody.acquiredExp, bLevelUp);
    }

    #endregion Lak, Exp

    #region Skill

    //---------------------------------------------------------------------------------------------------
    // 스킬레벨업
    public void SendSkillLevelUp(int nSkillId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SkillLevelUpCommandBody cmdBody = new SkillLevelUpCommandBody();
            cmdBody.skillId = m_nSkillId = nSkillId;
            CsRplzSession.Instance.Send(ClientCommandName.SkillLevelUp, cmdBody);
        }
    }

    void OnEventResSkillLevelUp(int nReturnCode, SkillLevelUpResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 스킬레벨변경
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(m_nSkillId);
            csHeroSkill.SkillLevel = responseBody.skillLevel;

            // 인벤토리변경.
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 골드
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            // 전투력변경
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSkillLevelUp(m_nSkillId);
        }
        else if (nReturnCode == 101)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A14_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A14_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 스킬전체레벨업
    public void SendSkillLevelUpTotally(int nSkillId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SkillLevelUpTotallyCommandBody cmdBody = new SkillLevelUpTotallyCommandBody();
            cmdBody.skillId = m_nSkillId = nSkillId;
            CsRplzSession.Instance.Send(ClientCommandName.SkillLevelUpTotally, cmdBody);
        }
    }

    void OnEventResSkillLevelUpTotally(int nReturnCode, SkillLevelUpTotallyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 스킬레벨변경
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(m_nSkillId);
            csHeroSkill.SkillLevel = responseBody.skillLevel;

            // 인벤토리변경.
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 골드
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            // 전투력변경
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventSkillLevelUpTotally(m_nSkillId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Skill

    #region SimpleShop

    //---------------------------------------------------------------------------------------------------
    // 간이상점구입
    public void SendSimpleShopBuy(int nProductId, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SimpleShopBuyCommandBody cmdBody = new SimpleShopBuyCommandBody();
            cmdBody.productId = nProductId;
            cmdBody.count = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.SimpleShopBuy, cmdBody);
        }
    }

    void OnEventResSimpleShopBuy(int nReturnCode, SimpleShopBuyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventSimpleShopBuy();

        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00801"));
        }
        else if (nReturnCode == 102)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00802"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }


    //---------------------------------------------------------------------------------------------------
    // 간이상점판매
    public void SendSimpleShopSell(int[] anSlotIndices)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SimpleShopSellCommandBody cmdBody = new SimpleShopSellCommandBody();
            cmdBody.slotIndices = m_anSlotIndices = anSlotIndices;
            CsRplzSession.Instance.Send(ClientCommandName.SimpleShopSell, cmdBody);
        }
    }

    void OnEventResSimpleShopSell(int nReturnCode, SimpleShopSellResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            for (int i = 0; i < m_anSlotIndices.Length; i++)
            {
                CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot(m_anSlotIndices[i]);
                CsGameData.Instance.MyHeroInfo.RemoveInventorySlot(csInventorySlot);
            }

            // 최대 골드
            CsAccomplishmentManager.Instance.MaxGold = responseBody.maxGold;

            CsGameEventUIToUI.Instance.OnEventSimpleShopSell();
        }
        else if (nReturnCode == 101)
        {
            // 최대판매슬롯수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_00901"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion SimpleShop

    #region Inventory

    //---------------------------------------------------------------------------------------------------
    // 인벤토리슬롯확장

    public void SendInventorySlotExtend(int nExtendSlotCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            InventorySlotExtendCommandBody cmdBody = new InventorySlotExtendCommandBody();
            cmdBody.extendSlotCount = nExtendSlotCount;
            CsRplzSession.Instance.Send(ClientCommandName.InventorySlotExtend, cmdBody);
        }
    }

    void OnEventResInventorySlotExtend(int nReturnCode, InventorySlotExtendResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.PaidInventorySlotCount = responseBody.paidInventorySlotCount;

            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
            CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

            CsGameEventUIToUI.Instance.OnEventInventorySlotExtend();
        }
        else if (nReturnCode == 101)
        {
            // 유효확장할 수 있는 최대슬롯수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01001"));
        }
        else if (nReturnCode == 102)
        {
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01002"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Inventory

    #region Item

    public void SendItemUse(int nInventorySlotIndex, int nCount = 1)
    {
        CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot(nInventorySlotIndex);

        switch (csInventorySlot.InventoryObjectItem.Item.ItemType.EnItemType)
        {
            case EnItemType.HpPotion:

                if (CsUIData.Instance.DungeonInNow != EnDungeon.FieldOfHonor)
                {
                    SendHpPotionUse(nInventorySlotIndex);
                }
                break;

            case EnItemType.ReturnScroll:
                if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                {
                    SendReturnScrollUse(nInventorySlotIndex);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                }
                break;

            case EnItemType.MainGearBox:
                SendMainGearBoxUse(nInventorySlotIndex, nCount);
                break;

            case EnItemType.PickBox:
			    SendPickBoxUse(nInventorySlotIndex, nCount);
                break;

            case EnItemType.Speaker:
                break;

            case EnItemType.ExpPotion:
                SendExpPotionUse(nInventorySlotIndex, nCount);
                break;
            case EnItemType.ExpScroll:
                SendExpScrollUse(nInventorySlotIndex);
                break;
            case EnItemType.FishingBait:
                if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                {
                    SendFishingBaitUse(nInventorySlotIndex);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                }
                break;
            case EnItemType.BountyHunter:
                SendBountyHunterQuestScrollUse(nInventorySlotIndex);
                break;
            case EnItemType.DistortionScroll:
                if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                {
                    SendDistortionScrollUse(nInventorySlotIndex);
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_ITEMNODUN"));
                }
                break;

            case EnItemType.GuildCall:
                CsGuildManager.Instance.SendGuildCall(nInventorySlotIndex);
                break;

            case EnItemType.NationCall:
                SendNationCall(nInventorySlotIndex);
                break;

            case EnItemType.Title:
                CsTitleManager.Instance.SendTitleItemUse(nInventorySlotIndex);
                break;

            case EnItemType.IllustratedBook:
                CsIllustratedBookManager.Instance.SendIllustratedBookUse(nInventorySlotIndex);
                break;

            case EnItemType.Gold:
                SendGoldItemUse(nInventorySlotIndex, nCount);
                break;

            case EnItemType.OwnDia:
                SendOwnDiaItemUse(nInventorySlotIndex, nCount);
                break;

            case EnItemType.HonorPoint:
                SendHonorPointItemUse(nInventorySlotIndex, nCount);
                break;

			case EnItemType.BiographyItem:
				CsBiography csBiography = CsGameData.Instance.BiographyList.Find(biography => biography.RequiredItem.ItemId == csInventorySlot.InventoryObjectItem.Item.ItemId);

				if (csBiography != null)
				{
					if (CsBiographyManager.Instance.GetHeroBiography(csBiography.BiographyId) == null)
					{
						CsBiographyManager.Instance.SendBiographyStart(csBiography.BiographyId);
					}
					else
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A122_TXT_03001"));
					}
				}

				break;

            case EnItemType.ExploitPoint:
                SendExploitPointItemUse(nInventorySlotIndex, nCount);
                break;

			case EnItemType.Costume:
				CsCostumeManager.Instance.SendCostumeItemUse(nInventorySlotIndex);
				break;

            case EnItemType.CostumeEffect:

                break;

			case EnItemType.CreatureEgg:
				if (CsCreatureManager.Instance.HeroCreatureList.Count < CsGameConfig.Instance.CreatureMaxCount)
				{
					CsCreatureManager.Instance.SendCreatureEggUse(nInventorySlotIndex);
				}
				else
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00046"));
				}

				break;

			case EnItemType.Wing:
				SendWingItemUse(nInventorySlotIndex);
				break;

			case EnItemType.SpiritStone:
				SendSpiritStoneItemUse(nInventorySlotIndex, nCount);
				break;

			case EnItemType.StarEssense:
				CsItem csItemStarEssense = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.StarEssense);

				if (csItemStarEssense != null && CsConstellationManager.Instance.DailyStarEssenseItemUseCount < csItemStarEssense.Value2)
				{
					CsConstellationManager.Instance.SendStarEssenseItemUse(nInventorySlotIndex, nCount);
				}
				else
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_ERROR_01602"));
				}
				
				break;

			case EnItemType.PremiumStarEssense:
				CsConstellationManager.Instance.SendPremiumStarEssenseItemUse(nInventorySlotIndex, nCount);
				break;

			case EnItemType.Mount:
				SendMountItemUse(nInventorySlotIndex);
				break;

			case EnItemType.AccomplishmentPoint:
				CsAccomplishmentManager.Instance.SendAccomplishmentPointItemUse(nInventorySlotIndex, nCount);
				break;

		}
    }

    //---------------------------------------------------------------------------------------------------
    // 아이템합성
    public void SendItemCompose(int nMaterialItemId, bool bOwned)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ItemComposeCommandBody cmdBody = new ItemComposeCommandBody();
            cmdBody.materialItemId = nMaterialItemId;
            cmdBody.owned = bOwned;
            CsRplzSession.Instance.Send(ClientCommandName.ItemCompose, cmdBody);
        }
    }

    void OnEventResItemCompose(int nReturnCode, ItemComposeResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameEventUIToUI.Instance.OnEventItemCompose();
        }
        else if (nReturnCode == 101)
        {
            // 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01101"));
        }
        else if (nReturnCode == 102)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 아이템전체합성
    public void SendItemComposeTotally(int nMaterialItemId, bool bOwned)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ItemComposeTotallyCommandBody cmdBody = new ItemComposeTotallyCommandBody();
            cmdBody.materialItemId = nMaterialItemId;
            cmdBody.owned = bOwned;
            CsRplzSession.Instance.Send(ClientCommandName.ItemComposeTotally, cmdBody);
        }
    }

    void OnEventResItemComposeTotally(int nReturnCode, ItemComposeTotallyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameEventUIToUI.Instance.OnEventItemComposeTotally();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 물약사용
    public void SendHpPotionUse(int nInventorySlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            HpPotionUseCommandBody cmdBody = new HpPotionUseCommandBody();
            cmdBody.inventorySlotIndex = m_nInventorySlotIndex = nInventorySlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.HpPotionUse, cmdBody);
        }
    }

    void OnEventResHpPotionUse(int nReturnCode, HpPotionUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 인벤토리 변경
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            int nOldHp = CsGameData.Instance.MyHeroInfo.Hp;

            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.HpPotionId);
            CsUIData.Instance.HpPotionRemainingCoolTime = csItem.Value1 + Time.realtimeSinceStartup;

            CsGameEventUIToUI.Instance.OnEventHpPotionUse(CsGameData.Instance.MyHeroInfo.Hp - nOldHp);
            CsGameEventToIngame.Instance.OnEventHpPotionUse();
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01201"));
        }
        else if (nReturnCode == 102)
        {
            // 이미 최대HP입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01202"));
        }
        else if (nReturnCode == 103)
        {
            // 쿨타임이 경과하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01203"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 귀환주문서사용
    public void SendReturnScrollUse(int nInventorySlotIndex)
    {
        if (!m_bProcessing)
        {
			// InGame 시전시작.
            if (CsGameEventToIngame.Instance.OnEventReturnScrollUseStart())
            {
                m_bProcessing = true;

                ReturnScrollUseCommandBody cmdBody = new ReturnScrollUseCommandBody();
                cmdBody.inventorySlotIndex = m_nInventorySlotIndex = nInventorySlotIndex;
                CsRplzSession.Instance.Send(ClientCommandName.ReturnScrollUse, cmdBody);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A55_TXT_02003"));
            }
        }
    }

    void OnEventResReturnScrollUse(int nReturnCode, ReturnScrollUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.ReturnScrollId);
            CsUIData.Instance.ReturnScrollRemainingCastTime = csItem.Value2 + Time.realtimeSinceStartup;
            CsGameEventUIToUI.Instance.OnEventReturnScrollUseStart();

        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01301"));
        }
        else if (nReturnCode == 102)
        {
            // 현재 전투중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01302"));
        }
        else if (nReturnCode == 103)
        {
            // 쿨타임이 경과하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01303"));
        }
        else if (nReturnCode == 104)
        {
            // 이미 사용중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01304"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtReturnScrollUseFinished(SEBReturnScrollUseFinishedEventBody eventBody)
    {
        // 쿨타임시작
        CsItem csItem = CsGameData.Instance.GetItem(CsUIData.Instance.ReturnScrollId);
        CsUIData.Instance.ReturnScrollRemainingCoolTime = csItem.Value1 + Time.realtimeSinceStartup;
        CsUIData.Instance.ReturnScrollRemainingCastTime = 0;

        // 인벤토리 변경
        PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { eventBody.changedInventorySlot };
        CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

        CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.targetNationId;

        CsGameEventUIToUI.Instance.OnEventReturnScrollUseFinished(eventBody.targetContinentId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtReturnScrollUseCancel(SEBReturnScrollUseCancelEventBody eventBody)
    {
        // 취소
        CsUIData.Instance.ReturnScrollRemainingCastTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    // 귀환주문서사용취소 클라이언트 이벤트
    public void SendReturnScrollUseCancel()
    {
        CEBReturnScrollUseCancelEventBody csEvt = new CEBReturnScrollUseCancelEventBody();
        CsRplzSession.Instance.Send(ClientEventName.ReturnScrollUseCancel, csEvt);

        // 취소
        CsUIData.Instance.ReturnScrollRemainingCastTime = 0;
    }

    //---------------------------------------------------------------------------------------------------
    // 뽑기상자사용
    public void SendPickBoxUse(int nSlotIndex, int nUseCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

			CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot(nSlotIndex);
			m_nItemId = csInventorySlot.InventoryObjectItem.Item.ItemId;

			PickBoxUseCommandBody cmdBody = new PickBoxUseCommandBody();
            cmdBody.slotIndex = m_nSlotIndex = nSlotIndex;
            cmdBody.useCount = nUseCount;
            CsRplzSession.Instance.Send(ClientCommandName.PickBoxUse, cmdBody);
        }
    }

    void OnEventResPickBoxUse(int nReturnCode, PickBoxUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
			List<CsDropObject> listLooted = new List<CsDropObject>();
			List<CsDropObject> listNotLooted = new List<CsDropObject>();

			CsGameData.Instance.MyHeroInfo.AddHeroMainGears(responseBody.addedHeroMainGears);
			
			// 탈것장비
			CsGameData.Instance.MyHeroInfo.AddHeroMountGears(responseBody.addedHeroMountGears);
			for (int i = 0; i < responseBody.addedHeroMountGears.Length; i++)
			{
				listLooted.Add(new CsDropObjectMountGear((int)EnDropObjectType.MountGear, responseBody.addedHeroMountGears[i]));
			}
			
			// 장비 또는 아이템
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			for (int i = 0; i < responseBody.changedInventorySlots.Length; i++)
			{
				if (responseBody.changedInventorySlots[i].inventoryObject != null)
				{
					CsInventorySlot csInventorySlot = new CsInventorySlot(responseBody.changedInventorySlots[i]);

					if (responseBody.changedInventorySlots[i].inventoryObject.type == (int)EnInventoryObjectType.MainGear)
					{
						listLooted.Add(new CsDropObjectMainGear((int)EnDropObjectType.MainGear, csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearId, csInventorySlot.InventoryObjectMainGear.HeroMainGear.Owned, csInventorySlot.InventoryObjectMainGear.HeroMainGear.EnchantLevel));
					}
					else if (responseBody.changedInventorySlots[i].inventoryObject.type == (int)EnInventoryObjectType.Item)
					{
						if (responseBody.changedInventorySlots[i].index == m_nSlotIndex && ((PDItemInventoryObject)(responseBody.changedInventorySlots[i].inventoryObject)).itemId == m_nItemId)
						{
							continue;
						}

						listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, csInventorySlot.InventoryObjectItem.Item.ItemId, csInventorySlot.InventoryObjectItem.Owned, csInventorySlot.InventoryObjectItem.Count));
					}
				}
			}

			// 획득표시
			CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
			
			// 변경된 크리쳐카드 목록
			CsCreatureCardManager.Instance.AddHeroCreatureCards(responseBody.changedHeroCreatureCards);

			List<CsHeroCreatureCard> list = new List<CsHeroCreatureCard>();

			for (int i = 0; i < responseBody.changedHeroCreatureCards.Length; i++)
			{
				list.Add(new CsHeroCreatureCard(responseBody.changedHeroCreatureCards[i]));
			}

			// 획득표시
			CsGameEventUIToUI.Instance.OnEventGetHeroCreatureCard(list);

			bool bFull = false;

            // 인벤토리가 가득찼을 경우, 변경슬롯이 빈값으로 온다.
            if (responseBody.changedInventorySlots.Length == 0)
                bFull = true;

            // 최대획득메인장비등급
            CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade = responseBody.maxAcquisitionMainGearGrade;

            CsGameEventUIToUI.Instance.OnEventPickBoxUse(bFull);
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01401"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }


    //---------------------------------------------------------------------------------------------------
    // 메인장비상자사용
    public void SendMainGearBoxUse(int nSlotIndex, int nUseCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MainGearBoxUseCommandBody cmdBody = new MainGearBoxUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nUseCount;
            CsRplzSession.Instance.Send(ClientCommandName.MainGearBoxUse, cmdBody);
        }
    }

    void OnEventResMainGearBoxUse(int nReturnCode, MainGearBoxUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            bool bFull = false;

            // 인벤토리가 가득찼을 경우, 변경슬롯이 빈값으로 온다.
            if (responseBody.changedInventorySlots.Length == 0)
                bFull = true;

            CsGameEventUIToUI.Instance.OnEventMainGearBoxUse(bFull);
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01501"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 경험치물약사용
    public void SendExpPotionUse(int nSlotIndex, int nUseCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ExpPotionUseCommandBody cmdBody = new ExpPotionUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nUseCount;
            CsRplzSession.Instance.Send(ClientCommandName.ExpPotionUse, cmdBody);
        }
    }

    void OnEventResExpPotionUse(int nReturnCode, ExpPotionUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.ExpPotionUseCountDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount = responseBody.expPotinDailyUseCount;

            int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

            CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
            CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            PDInventorySlot[] inventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

            CsGameEventUIToUI.Instance.OnEventExpPotionUse(bLevelUp, responseBody.acquiredExp);
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01501"));
        }
        else if (nReturnCode == 102)
        {
            // 일일사용횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01602"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 경험치물약사용
    public void SendExpScrollUse(int nSlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ExpScrollUseCommandBody cmdBody = new ExpScrollUseCommandBody();
            cmdBody.slotIndex = m_nInventorySlotIndex = nSlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.ExpScrollUse, cmdBody);
        }
    }

    void OnEventResExpScrollUse(int nReturnCode, ExpScrollUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlot(m_nInventorySlotIndex);

            CsGameData.Instance.MyHeroInfo.ExpScrollUseCountDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.ExpScrollDailyUseCount = responseBody.expScrollDailyUseCount;
            CsGameData.Instance.MyHeroInfo.ExpScrollRemainingTime = responseBody.expScrollRemainingTime;
            CsGameData.Instance.MyHeroInfo.ExpScrollItemId = csInventorySlot.InventoryObjectItem.Item.ItemId;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventExpScrollUse();
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01801"));
        }
        else if (nReturnCode == 102)
        {
            // 일일사용횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01802"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 현상금사냥꾼퀘스트주문서사용
    void SendBountyHunterQuestScrollUse(int nSlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            BountyHunterQuestScrollUseCommandBody cmdBody = new BountyHunterQuestScrollUseCommandBody();
            cmdBody.slotIndex = m_nInventorySlotIndex = nSlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.BountyHunterQuestScrollUse, cmdBody);
        }
    }

    void OnEventResBountyHunterQuestScrollUse(int nReturnCode, BountyHunterQuestScrollUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsBountyHunterQuestManager.Instance.SetQuestInfo(responseBody.quest, responseBody.bountyHunterQuestDailyStartCount, responseBody.date);

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventBountyHunterQuestScrollUse();
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A45_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 일일사용횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A45_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }


    //---------------------------------------------------------------------------------------------------
    // 미끼 사용
    void SendFishingBaitUse(int nSlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            FishingBaitUseCommandBody cmdBody = new FishingBaitUseCommandBody();
            cmdBody.slotIndex = m_nInventorySlotIndex = nSlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.FishingBaitUse, cmdBody);
        }
    }

    void OnEventResFishingBaitUse(int nReturnCode, FishingBaitUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsFishingQuestManager.Instance.SetQuestInfo(responseBody.quest, responseBody.fishingQuestDailyStartCount, responseBody.date);

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            //누적에픽미끼사용횟수
            CsAccomplishmentManager.Instance.AccEpicBaitItemUseCount = responseBody.accEpicBaitItemUseCount;

            //누적전설미끼사용횟수
            CsAccomplishmentManager.Instance.AccLegendBaitItemUseCount = responseBody.accLegendBaitItemUseCount;

            CsGameEventUIToUI.Instance.OnEventFishingBaitUse();
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A46_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 일일사용횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A46_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 왜곡주문서사용
    public void SendDistortionScrollUse(int nSlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            DistortionScrollUseCommandBody cmdBody = new DistortionScrollUseCommandBody();
            cmdBody.slotIndex = m_nInventorySlotIndex = nSlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.DistortionScrollUse, cmdBody);
        }
    }

    void OnEventResDistortionScrollUse(int nReturnCode, DistortionScrollUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.DistortionScrollUseCountDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.DistortionScrollDailyUseCount = responseBody.distortionScrollDailyUseCount;
            CsGameData.Instance.MyHeroInfo.RemainingDistortionTime = responseBody.distortionRemainingTime;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventDistortionScrollUse();
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 전투중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A56_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A56_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 일일사용횟수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A56_ERROR_00103"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 왜곡취소.
    public void SendDistortionCancel()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            DistortionCancelCommandBody cmdBody = new DistortionCancelCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.DistortionCancel, cmdBody);
        }
    }

    void OnEventResDistortionCancel(int nReturnCode, DistortionCancelResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.RemainingDistortionTime = 0;

            CsGameEventUIToUI.Instance.OnEventDistortionCanceled();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 왜곡취소이벤트
    void OnEventEvtDistortionCanceled(SEBDistortionCanceledEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.RemainingDistortionTime = 0;

        CsGameEventUIToUI.Instance.OnEventDistortionCanceled();
    }


    //---------------------------------------------------------------------------------------------------
    // 골드아이템사용
    public void SendGoldItemUse(int nSlotIndex, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            GoldItemUseCommandBody cmdBody = new GoldItemUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.GoldItemUse, cmdBody);
        }
    }

    void OnEventResGoldItemUse(int nReturnCode, GoldItemUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
            CsAccomplishmentManager.Instance.MaxGold = responseBody.maxGold;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventGoldItemUse();
        }
        else if (nReturnCode == 101)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_00901"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 귀속다이아아이템사용
    public void SendOwnDiaItemUse(int nSlotIndex, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            OwnDiaItemUseCommandBody cmdBody = new OwnDiaItemUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.OwnDiaItemUse, cmdBody);
        }
    }

    void OnEventResOwnDiaItemUse(int nReturnCode, OwnDiaItemUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventOwnDiaItemUse();
        }
        else if (nReturnCode == 101)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001001"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 명예포인트아이템사용
    public void SendHonorPointItemUse(int nSlotIndex, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            HonorPointItemUseCommandBody cmdBody = new HonorPointItemUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.HonorPointItemUse, cmdBody);
        }
    }

    void OnEventResHonorPointItemUse(int nReturnCode, HonorPointItemUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.HonorPoint = responseBody.honorPoint;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventHonorPointItemUse();
        }
        else if (nReturnCode == 101)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 공적포인트아이템사용
    public void SendExploitPointItemUse(int nSlotIndex, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ExploitPointItemUseCommandBody cmdBody = new ExploitPointItemUseCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            cmdBody.useCount = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.ExploitPointItemUse, cmdBody);
        }
    }

    void OnEventResExploitPointItemUse(int nReturnCode, ExploitPointItemUseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.ExploitPoint = responseBody.exploitPoint;
            CsGameData.Instance.MyHeroInfo.DailyExploitPoint = responseBody.dailyExploitPoint;
            CsGameData.Instance.MyHeroInfo.ExploitPointDate = responseBody.date;

            PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

            CsGameEventUIToUI.Instance.OnEventExploitPointItemUse();
        }
        else if (nReturnCode == 101)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001201"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 날개아이템사용
	public void SendWingItemUse(int nSlotIndex)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WingItemUseCommandBody cmdBody = new WingItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.WingItemUse, cmdBody);
		}
	}

	void OnEventResWingItemUse(int nReturnCode, WingItemUseResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.AddHeroWing(responseBody.addedWing);
			PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventWingItemUse();
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영혼석사용
	public void SendSpiritStoneItemUse(int nSlotIndex, int nUseCount)
	{

	}

	void OnEventResSpiritStoneItemUse(int nReturnCode, SpiritStoneItemUseResponseBody responseBody)
	{
	}


	#endregion Item

	#region Rest
	//---------------------------------------------------------------------------------------------------
	// 무료휴식보상받기
	public void SendRestRewardReceiveFree()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RestRewardReceiveFreeCommandBody cmdBody = new RestRewardReceiveFreeCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.RestRewardReceiveFree, cmdBody);
        }
    }

    void OnEventResRestRewardReceiveFree(int nReturnCode, RestRewardReceiveFreeResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

            CsGameData.Instance.MyHeroInfo.RestTime = 0;
            CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
            CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

            CsGameEventUIToUI.Instance.OnEventRestRewardReceiveFree(bLevelUp, responseBody.acquiredExp);
        }
        else if (nReturnCode == 101)
        {
            // 영웅의 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A33_ERROR_00301"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 골드휴식보상받기
    public void SendRestRewardReceiveGold()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RestRewardReceiveGoldCommandBody cmdBody = new RestRewardReceiveGoldCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.RestRewardReceiveGold, cmdBody);
        }
    }

    void OnEventResRestRewardReceiveGold(int nReturnCode, RestRewardReceiveGoldResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

            CsGameData.Instance.MyHeroInfo.RestTime = 0;
            CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
            CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

            CsGameEventUIToUI.Instance.OnEventRestRewardReceiveGold(bLevelUp, responseBody.acquiredExp);
        }
        else if (nReturnCode == 101)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A33_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅의 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A33_ERROR_00402"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 다이아휴식보상받기
    public void SendRestRewardReceiveDia()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RestRewardReceiveDiaCommandBody cmdBody = new RestRewardReceiveDiaCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.RestRewardReceiveDia, cmdBody);
        }
    }

    void OnEventResRestRewardReceiveDia(int nReturnCode, RestRewardReceiveDiaResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

            CsGameData.Instance.MyHeroInfo.RestTime = 0;
            CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
            CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
            CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

            CsGameEventUIToUI.Instance.OnEventRestRewardReceiveDia(bLevelUp, responseBody.acquiredExp);
        }
        else if (nReturnCode == 101)
        {
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A33_ERROR_00201"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅의 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A33_ERROR_00502"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Rest

    #region Revive

    //---------------------------------------------------------------------------------------------------
    // 즉시부활
    public void SendImmediateRevive()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ImmediateReviveCommandBody cmdBody = new ImmediateReviveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ImmediateRevive, cmdBody);
        }
    }

    void OnEventResImmediateRevive(int nReturnCode, ImmediateReviveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

            // 일일무료즉시부활횟수
            CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.FreeImmediateRevivalDailyCount = responseBody.freeImmediateRevivalDailyCount;

            // 일일유료즉시부활횟수
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.PaidImmediateRevivalDailyCount = responseBody.paidImmediateRevivalDailyCount;

            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
            CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

            // 인게임
            CsGameEventToIngame.Instance.OnEventImmediateRevived();
            CsGameEventUIToUI.Instance.OnEventImmediateRevive();
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태가 아닙니다.
            CsGameEventToIngame.Instance.OnEventImmediateRevived();
            CsGameEventUIToUI.Instance.OnEventImmediateRevive();
            //CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("RESURRE_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("RESURRE_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 대륙안전부활
    public void SendContinentSaftyRevive()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ContinentSaftyReviveCommandBody cmdBody = new ContinentSaftyReviveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ContinentSaftyRevive, cmdBody);
        }
    }

    void OnEventResContinentSaftyRevive(int nReturnCode, ContinentSaftyReviveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.revivalTargetNationId;
            CsGameEventUIToUI.Instance.OnEventContinentSaftyRevive(responseBody.revivalTargetContinentId);
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태가 아닙니다.
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.revivalTargetNationId;
            CsGameEventUIToUI.Instance.OnEventContinentSaftyRevive(responseBody.revivalTargetContinentId);
            //CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("RESURRE_ERROR_00201"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Revive

    #region DropObject

    //---------------------------------------------------------------------------------------------------
    // 아이템 루팅
    void OnEventEvtDropObjectLooted(SEBDropObjectLootedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.AddHeroMainGears(eventBody.heroMainGears);
        CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

        List<CsDropObject> listLooted = new List<CsDropObject>();
        List<CsDropObject> listNotLooted = new List<CsDropObject>();

        for (int i = 0; i < eventBody.lootedDropObjects.Length; i++)
        {
            switch ((EnDropObjectType)eventBody.lootedDropObjects[i].type)
            {
                case EnDropObjectType.MainGear:
                    listLooted.Add(new CsDropObjectMainGear((PDMainGearDropObject)eventBody.lootedDropObjects[i]));
                    break;

                case EnDropObjectType.Item:
                    listLooted.Add(new CsDropObjectItem((PDItemDropObject)eventBody.lootedDropObjects[i]));
                    break;
            }
        }

        for (int i = 0; i < eventBody.notLootedDropObjects.Length; i++)
        {
            switch ((EnDropObjectType)eventBody.notLootedDropObjects[i].type)
            {
                case EnDropObjectType.MainGear:
                    listNotLooted.Add(new CsDropObjectMainGear((PDMainGearDropObject)eventBody.notLootedDropObjects[i]));
                    break;

                case EnDropObjectType.Item:
                    listNotLooted.Add(new CsDropObjectItem((PDItemDropObject)eventBody.notLootedDropObjects[i]));
                    break;
            }
        }

        // 최대획득메인장비등급 
        CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade = eventBody.maxAcquisitionMainGearGrade;

        CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
    }

    #endregion DropObject

    #region Party

    //---------------------------------------------------------------------------------------------------
    // 주변영웅목록
    public void SendPartySurroundingHeroList()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartySurroundingHeroListCommandBody cmdBody = new PartySurroundingHeroListCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartySurroundingHeroList, cmdBody);
        }
    }

    void OnEventResPartySurroundingHeroList(int nReturnCode, PartySurroundingHeroListResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsSimpleHero[] simpleHeroes = new CsSimpleHero[responseBody.heroes.Length];

            for (int i = 0; i < responseBody.heroes.Length; i++)
            {
                simpleHeroes[i] = new CsSimpleHero(responseBody.heroes[i]);
            }

            CsGameEventUIToUI.Instance.OnEventPartySurroundingHeroList(simpleHeroes);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 주변파티목록
    public void SendPartySurroundingPartyList()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartySurroundingPartyListCommandBody cmdBody = new PartySurroundingPartyListCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartySurroundingPartyList, cmdBody);
        }
    }

    void OnEventResPartySurroundingPartyList(int nReturnCode, PartySurroundingPartyListResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsSimpleParty[] simpleParties = new CsSimpleParty[responseBody.parties.Length];

            for (int i = 0; i < responseBody.parties.Length; i++)
            {
                simpleParties[i] = new CsSimpleParty(responseBody.parties[i]);
            }

            CsGameEventUIToUI.Instance.OnEventPartySurroundingPartyList(simpleParties);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티생성
    public void SendPartyCreate()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyCreateCommandBody cmdBody = new PartyCreateCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartyCreate, cmdBody);
        }
    }

    void OnEventResPartyCreate(int nReturnCode, PartyCreateResponseBody responseBody)
    {
        m_bProcessing = false;


        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party = new CsParty(responseBody.party);
            CsGameEventUIToUI.Instance.OnEventPartyCreate();
        }
        else if (nReturnCode == 101)
        {
            // 이미 파티에 가입되어 있습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }


    }

    //---------------------------------------------------------------------------------------------------
    // 파티탈퇴
    public void SendPartyExit()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyExitCommandBody cmdBody = new PartyExitCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartyExit, cmdBody);
        }
    }

    void OnEventResPartyExit(int nReturnCode, PartyExitResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party = null;

            CsGameEventUIToUI.Instance.OnEventPartyExit();
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00201"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티멤버강퇴
    public void SendPartyMemberBanish(Guid guidMemberId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyMemberBanishCommandBody cmdBody = new PartyMemberBanishCommandBody();
            cmdBody.targetMemberId = m_guidMemberId = guidMemberId;
            CsRplzSession.Instance.Send(ClientCommandName.PartyMemberBanish, cmdBody);
        }
    }

    void OnEventResPartyMemberBanish(int nReturnCode, PartyMemberBanishResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party.RemoveMember(m_guidMemberId);

            CsGameEventUIToUI.Instance.OnEventPartyMemberBanish(m_guidMemberId);
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00301"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00302"));
        }
        else if (nReturnCode == 103)
        {
            // 대상이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00303"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티소집
    public void SendPartyCall()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyCallCommandBody cmdBody = new PartyCallCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartyCall, cmdBody);
        }
    }

    void OnEventResPartyCall(int nReturnCode, PartyCallResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party.CallRemainingCoolTime = CsGameConfig.Instance.PartyCallCoolTime;

            CsGameEventUIToUI.Instance.OnEventPartyCall();
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00401"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00402"));
        }
        else if (nReturnCode == 103)
        {
            // 쿨타임이 경과되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00403"));
        }
        else if (nReturnCode == 104)
        {
            // 현재 장소에서 사용할 수 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00404"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티해산
    public void SendPartyDisband()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyDisbandCommandBody cmdBody = new PartyDisbandCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.PartyDisband, cmdBody);
        }
    }

    void OnEventResPartyDisband(int nReturnCode, PartyDisbandResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party = null;

            CsGameEventUIToUI.Instance.OnEventPartyDisband();
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00501"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00502"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청
    public void SendPartyApply(Guid guidPartyId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyApplyCommandBody cmdBody = new PartyApplyCommandBody();
            cmdBody.partyId = guidPartyId;
            CsRplzSession.Instance.Send(ClientCommandName.PartyApply, cmdBody);
        }
    }

    void OnEventResPartyApply(int nReturnCode, PartyApplyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsPartyApplication csPartyApplication = new CsPartyApplication(responseBody.app);

            // 파티신청리스트에 등록한다.
            CsGameData.Instance.MyHeroInfo.AddPartyApplication(csPartyApplication);

            CsGameEventUIToUI.Instance.OnEventPartyApply(csPartyApplication);
        }
        else if (nReturnCode == 101)
        {
            // 이미 파티에 가입되어 있습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04022"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingPartyListRequest();
        }
        else if (nReturnCode == 102)
        {
            // 대상 파티에 대한 신청이 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00602"));
        }
        else if (nReturnCode == 103)
        {
            // 파티가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04021"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingPartyListRequest();
        }
        else if (nReturnCode == 104)
        {
            // 다른 국가의 파티입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00604"));
        }
        else if (nReturnCode == 105)
        {
            // 파티의 멤버가 모두 찼습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04006"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingPartyListRequest();
        }
        else if (nReturnCode == 106)
        {
            // 파티장이 로그인중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04020"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingPartyListRequest();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청수락
    public void SendPartyApplicationAccept(long lApplicationNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyApplicationAcceptCommandBody cmdBody = new PartyApplicationAcceptCommandBody();
            cmdBody.applicationNo = m_lApplicationNo = lApplicationNo;
            CsRplzSession.Instance.Send(ClientCommandName.PartyApplicationAccept, cmdBody);
        }
    }

    void OnEventResPartyApplicationAccept(int nReturnCode, PartyApplicationAcceptResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsPartyMember csPartyMember = new CsPartyMember(responseBody.acceptedMember);
            CsGameData.Instance.MyHeroInfo.Party.AddMember(csPartyMember);

            // 신청받은 파티리스트에서 삭제한다.
            CsGameData.Instance.MyHeroInfo.Party.RemovePartyApplication(m_lApplicationNo);

            CsGameEventUIToUI.Instance.OnEventPartyApplicationAccept(csPartyMember);
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00701"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00702"));
        }
        else if (nReturnCode == 103)
        {
            // 신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00703"));
        }
        else if (nReturnCode == 104)
        {
            // 파티의 멤버가 모두 찼습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04006"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청거절
    public void SendPartyApplicationRefuse(long lApplicationNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyApplicationRefuseCommandBody cmdBody = new PartyApplicationRefuseCommandBody();
            cmdBody.applicationNo = m_lApplicationNo = lApplicationNo;
            CsRplzSession.Instance.Send(ClientCommandName.PartyApplicationRefuse, cmdBody);
        }
    }

    void OnEventResPartyApplicationRefuse(int nReturnCode, PartyApplicationRefuseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 신청받은 파티리스트에서 삭제한다.
            CsGameData.Instance.MyHeroInfo.Party.RemovePartyApplication(m_lApplicationNo);

            CsGameEventUIToUI.Instance.OnEventPartyApplicationRefuse();
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00801"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00802"));
        }
        else if (nReturnCode == 103)
        {
            // 신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00803"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티마스터변경
    public void SendPartyMasterChange(Guid guidTargetId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyMasterChangeCommandBody cmdBody = new PartyMasterChangeCommandBody();
            cmdBody.targetMemberId = m_guidMemberId = guidTargetId;
            CsRplzSession.Instance.Send(ClientCommandName.PartyMasterChange, cmdBody);
        }
    }

    void OnEventResPartyMasterChange(int nReturnCode, PartyMasterChangeResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsPartyMember csPartyMember = CsGameData.Instance.MyHeroInfo.Party.GetMember(m_guidMemberId);
            CsGameData.Instance.MyHeroInfo.Party.Master = csPartyMember;

            CsGameEventUIToUI.Instance.OnEventPartyMasterChange();
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01201"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01202"));
        }
        else if (nReturnCode == 103)
        {
            // 대상이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01203"));
        }
        else if (nReturnCode == 104)
        {
            //  대상이 로그인중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01204"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대
    public void SendPartyInvite(Guid guidHeroId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyInviteCommandBody cmdBody = new PartyInviteCommandBody();
            cmdBody.targetHeroId = m_guidMemberId = guidHeroId;
            CsRplzSession.Instance.Send(ClientCommandName.PartyInvite, cmdBody);
        }
    }

    void OnEventResPartyInvite(int nReturnCode, PartyInviteResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsPartyInvitation csPartyInvitation = new CsPartyInvitation(responseBody.invitation);
            CsGameData.Instance.MyHeroInfo.Party.AddPartyInvitation(csPartyInvitation);

            CsGameEventUIToUI.Instance.OnEventPartyInvite(csPartyInvitation);
        }
        else if (nReturnCode == 101)
        {
            // 파티에 가입되어있지 않습니다.

            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00901"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04023"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingHeroListRequest();
        }
        else if (nReturnCode == 103)
        {
            // 대상영웅이 존재하지 않거나 다른 국가 영웅입니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A15_TXT_02001"));
        }
        else if (nReturnCode == 104)
        {
            // 대상영웅은 이미 파티멤버입니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04005"));
            CsGameEventUIToUI.Instance.OnEventPartySurroundingHeroListRequest();
        }
        else if (nReturnCode == 105)
        {
            // 대상영웅은 이미 초대되었습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_00905"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대수락
    public void SendPartyInvitationAccept(long lInvitationNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyInvitationAcceptCommandBody cmdBody = new PartyInvitationAcceptCommandBody();
            cmdBody.invitationNo = m_lApplicationNo = lInvitationNo;
            CsRplzSession.Instance.Send(ClientCommandName.PartyInvitationAccept, cmdBody);
        }
    }

    void OnEventResPartyInvitationAccept(int nReturnCode, PartyInvitationAcceptResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.Party = new CsParty(responseBody.party);
            // 초대목록삭제
            CsGameData.Instance.MyHeroInfo.RemovePartyInvitation(m_lApplicationNo);

            CsGameEventUIToUI.Instance.OnEventPartyInvitationAccept();
        }
        else if (nReturnCode == 101)
        {
            // 이미 파티에 가입되어 있습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04022"));
        }
        else if (nReturnCode == 102)
        {
            // 초대가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01002"));
        }
        else if (nReturnCode == 103)
        {
            // 파티의 멤버가 모두 찼습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A36_TXT_04006"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대거절
    public void SendPartyInvitationRefuse(long lInvitationNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            PartyInvitationRefuseCommandBody cmdBody = new PartyInvitationRefuseCommandBody();
            cmdBody.invitationNo = m_lApplicationNo = lInvitationNo;
            CsRplzSession.Instance.Send(ClientCommandName.PartyInvitationRefuse, cmdBody);
        }
    }

    void OnEventResPartyInvitationRefuse(int nReturnCode, PartyInvitationRefuseResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 초대목록삭제
            CsGameData.Instance.MyHeroInfo.RemovePartyInvitation(m_lApplicationNo);

            CsGameEventUIToUI.Instance.OnEventPartyInvitationRefuse();
        }
        else if (nReturnCode == 101)
        {
            //  초대가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A36_ERROR_01101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청도착 
    void OnEventEvtPartyApplicationArrived(SEBPartyApplicationArrivedEventBody eventBody)
    {
        CsPartyApplication csPartyApplication = new CsPartyApplication(eventBody.app);

        // 파티신청받은리스트에 등록
        CsGameData.Instance.MyHeroInfo.Party.AddPartyApplication(csPartyApplication);

        CsGameEventUIToUI.Instance.OnEventPartyApplicationArrived(csPartyApplication);
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청취소
    void OnEventEvtPartyApplicationCanceled(SEBPartyApplicationCanceledEventBody eventBody)
    {
        // 파티신청받은리스트에서 삭제
        CsGameData.Instance.MyHeroInfo.Party.RemovePartyApplication(eventBody.applicationNo);

        CsGameEventUIToUI.Instance.OnEventPartyApplicationCanceled();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청수락
    void OnEventEvtPartyApplicationAccepted(SEBPartyApplicationAcceptedEventBody eventBody)
    {
        // 파티신청 리스트 삭제
        CsGameData.Instance.MyHeroInfo.RemovePartyApplication(eventBody.applicationNo);

        CsGameData.Instance.MyHeroInfo.Party = new CsParty(eventBody.party);

        CsGameEventUIToUI.Instance.OnEventPartyApplicationAccepted();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청거절
    void OnEventEvtPartyApplicationRefused(SEBPartyApplicationRefusedEventBody eventBody)
    {
        // 파티신청 리스트 삭제
        CsGameData.Instance.MyHeroInfo.RemovePartyApplication(eventBody.applicationNo);

        CsGameEventUIToUI.Instance.OnEventPartyApplicationRefused();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티신청수명종료
    void OnEventEvtPartyApplicationLifetimeEnded(SEBPartyApplicationLifetimeEndedEventBody eventBody)
    {
        // 파티신청 리스트 삭제
        CsGameData.Instance.MyHeroInfo.RemovePartyApplication(eventBody.applicationNo);
        // 파티신청 받은 리스트 삭제
        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsGameData.Instance.MyHeroInfo.Party.RemovePartyApplication(eventBody.applicationNo);
        }

        CsGameEventUIToUI.Instance.OnEventPartyApplicationLifetimeEnded();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대도착
    void OnEventEvtPartyInvitationArrived(SEBPartyInvitationArrivedEventBody eventBody)
    {
        CsPartyInvitation csPartyInvitation = new CsPartyInvitation(eventBody.invitation);

        // 파티초대받은 리스트 등록
        CsGameData.Instance.MyHeroInfo.AddPartyInvitation(csPartyInvitation);

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyInvitationAutoAccept);

            if (nAutoAccept == 1)
            {
                SendPartyInvitationAccept(csPartyInvitation.No);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventPartyInvitationArrived(csPartyInvitation);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventPartyInvitationArrived(csPartyInvitation);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대취소
    void OnEventEvtPartyInvitationCanceled(SEBPartyInvitationCanceledEventBody eventBody)
    {
        // 파티초대받은 리스트 삭제
        CsGameData.Instance.MyHeroInfo.RemovePartyInvitation(eventBody.invitationNo);

        CsGameEventUIToUI.Instance.OnEventPartyInvitationCanceled();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대수락
    void OnEventEvtPartyInvitationAccepted(SEBPartyInvitationAcceptedEventBody eventBody)
    {
        // 파티초대 리스트 삭제
        CsGameData.Instance.MyHeroInfo.Party.RemovePartyInvitation(eventBody.invitationNo);

        CsGameEventUIToUI.Instance.OnEventPartyInvitationAccepted();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대거절
    void OnEventEvtPartyInvitationRefused(SEBPartyInvitationRefusedEventBody eventBody)
    {
        // 파티초대 리스트 삭제
        CsGameData.Instance.MyHeroInfo.Party.RemovePartyInvitation(eventBody.invitationNo);

        CsGameEventUIToUI.Instance.OnEventPartyInvitationRefused();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티초대수명종료
    void OnEventEvtPartyInvitationLifetimeEnded(SEBPartyInvitationLifetimeEndedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.RemovePartyInvitation(eventBody.invitationNo);

        if (CsGameData.Instance.MyHeroInfo.Party != null)
        {
            CsGameData.Instance.MyHeroInfo.Party.RemovePartyInvitation(eventBody.invitationNo);
        }

        CsGameEventUIToUI.Instance.OnEventPartyInvitationLifetimeEnded();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티멤버입장
    void OnEventEvtPartyMemberEnter(SEBPartyMemberEnterEventBody eventBody)
    {
        CsPartyMember csPartyMember = new CsPartyMember(eventBody.member);
        CsGameData.Instance.MyHeroInfo.Party.AddMember(csPartyMember);

        CsGameEventUIToUI.Instance.OnEventPartyMemberEnter(csPartyMember);
    }

    //---------------------------------------------------------------------------------------------------
    // 파티멤버퇴장
    void OnEventEvtPartyMemberExit(SEBPartyMemberExitEventBody eventBody)
    {
        CsPartyMember csPartyMember = CsGameData.Instance.MyHeroInfo.Party.GetMember(eventBody.memberId);
        CsGameData.Instance.MyHeroInfo.Party.RemoveMember(csPartyMember.Id);

        CsGameEventUIToUI.Instance.OnEventPartyMemberExit(csPartyMember, eventBody.banished);
    }

    //---------------------------------------------------------------------------------------------------
    // 파티강퇴
    void OnEventEvtPartyBanished(SEBPartyBanishedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.Party = null;

        CsGameEventUIToUI.Instance.OnEventPartyBanished();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티장변경
    void OnEventEvtPartyMasterChanged(SEBPartyMasterChangedEventBody eventBody)
    {
        CsPartyMember csPartyMember = CsGameData.Instance.MyHeroInfo.Party.GetMember(eventBody.masterId);

        CsGameData.Instance.MyHeroInfo.Party.Master = csPartyMember;
        CsGameData.Instance.MyHeroInfo.Party.CallRemainingCoolTime = eventBody.callRemainingCoolTime;

        CsGameEventUIToUI.Instance.OnEventPartyMasterChanged();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티소집
    void OnEventEvtPartyCall(SEBPartyCallEventBody eventBody)
    {
        Vector3 v3Postion = new Vector3(eventBody.position.x, eventBody.position.y, eventBody.position.z);

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyCallAutoAccept);

            if (nAutoAccept == 1)
            {
                CsGameEventToIngame.Instance.OnEventPartyCalled(eventBody.continentId, eventBody.nationId, v3Postion);
            }
            else
            {
                CsGameEventUIToUI.Instance.OnEventPartyCalled(eventBody.continentId, eventBody.nationId, v3Postion);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventPartyCalled(eventBody.continentId, eventBody.nationId, v3Postion);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 파티해산
    void OnEventEvtPartyDisbanded(SEBPartyDisbandedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.Party = null;

        CsGameEventUIToUI.Instance.OnEventPartyDisbanded();
    }

    //---------------------------------------------------------------------------------------------------
    // 파티멤버갱신
    void OnEventEvtPartyMembersUpdated(SEBPartyMembersUpdatedEventBody eventBody)
    {
        for (int i = 0; i < eventBody.members.Length; i++)
        {
            CsGameData.Instance.MyHeroInfo.Party.UpdateMember(eventBody.members[i]);
        }

        CsGameEventUIToUI.Instance.OnEventPartyMembersUpdated();
    }

    #endregion Party

    #region Chatting

    //---------------------------------------------------------------------------------------------------
    // 채팅메시지발송
    public void SendChattingMessageSend(int nType, string[] asMessage, PDChattingLink pDChattingLink, Guid guidTargetHeroId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ChattingMessageSendCommandBody cmdBody = new ChattingMessageSendCommandBody();
            cmdBody.type = nType;
            cmdBody.messages = asMessage;
            cmdBody.link = pDChattingLink;
            cmdBody.targetHeroId = guidTargetHeroId;
            CsRplzSession.Instance.Send(ClientCommandName.ChattingMessageSend, cmdBody);
        }
    }

    void OnEventResChattingMessageSend(int nReturnCode, ChattingMessageSendResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            if (responseBody.changedInventorySlot != null)
            {
                PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
                CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);
            }

            CsGameEventUIToUI.Instance.OnEventChattingMessageSend();
        }
        else if (nReturnCode == 101)
        {
            // 메시지 길이가 최대 길이를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A34_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            //  최소 채팅간격이 경과되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A34_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 파티에 가입되어 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A34_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 자신에게 메시지를 보낼 수 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A34_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 대상영웅이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A15_TXT_02001"));
        }
        else if (nReturnCode == 106)
        {
            // 세계채팅 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A34_ERROR_00106"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 채팅메시지수신
    void OnEventEvtChattingMessageReceived(SEBChattingMessageReceivedEventBody eventBody)
    {
		// 블랙리스트 체크
		CsBlacklistEntry csBlacklistEntry = CsFriendManager.Instance.GetBlacklistEntry(eventBody.sender.id);

		if (csBlacklistEntry != null)
			return;

		CsChattingMessage csChattingMessage = new CsChattingMessage(eventBody.type, eventBody.messages, eventBody.link, eventBody.sender, eventBody.target);
        CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

        List<CsChattingMessage> list = null;

        if (csChattingMessage.ChattingType == EnChattingType.OneToOne)
        {
            list = CsUIData.Instance.ChattingMessageList.FindAll(a => a.ChattingType == csChattingMessage.ChattingType
                                                                    && (a.Sender.HeroId == csChattingMessage.Sender.HeroId && a.Target.HeroId == csChattingMessage.Target.HeroId
                                                                        || a.Sender.HeroId == csChattingMessage.Target.HeroId && a.Target.HeroId == csChattingMessage.Sender.HeroId));

            CsChattingMessage csChattingMessageOneToOneNew = null;

            if (csChattingMessage.Sender.HeroId == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                csChattingMessageOneToOneNew = new CsChattingMessage((int)EnChattingType.OneToOne, eventBody.messages, eventBody.link, eventBody.target, null);
            }
            else
            {
                csChattingMessageOneToOneNew = new CsChattingMessage((int)EnChattingType.OneToOne, eventBody.messages, eventBody.link, eventBody.sender, null);
            }

            CsChattingMessage csChattingMessageOneToOne = CsUIData.Instance.OneToOneList.Find(a => a.Sender.HeroId == csChattingMessageOneToOneNew.Sender.HeroId);

            if (csChattingMessageOneToOne == null)
            {
                CsUIData.Instance.OneToOneList.Add(csChattingMessageOneToOneNew);
            }
            else
            {
                csChattingMessageOneToOne.Update(eventBody.messages, eventBody.link);
            }

            CsUIData.Instance.OneToOneList.Sort();
        }
        else
        {
            list = CsUIData.Instance.ChattingMessageList.FindAll(a => a.ChattingType == csChattingMessage.ChattingType);
        }

        if (list.Count > CsGameConfig.Instance.ChattingDisplayMaxCount)
        {
            CsUIData.Instance.ChattingMessageList.Remove(list[0]);
        }

        CsGameEventUIToUI.Instance.OnEventChattingMessageReceived(csChattingMessage);
    }

    #endregion Chatting

    #region Support

    //---------------------------------------------------------------------------------------------------
    // 레벨업보상받기
    public void SendLevelUpRewardReceive(int nEntryId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            LevelUpRewardReceiveCommandBody cmdBody = new LevelUpRewardReceiveCommandBody();
            cmdBody.entryId = m_nEntryId = nEntryId;
            CsRplzSession.Instance.Send(ClientCommandName.LevelUpRewardReceive, cmdBody);
        }
    }

    void OnEventResLevelUpRewardReceive(int nReturnCode, LevelUpRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.ReceivedLevelUpRewardList.Add(m_nEntryId);

            CsGameEventUIToUI.Instance.OnEventLevelUpRewardReceive();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일접속시간보상받기
    public void SendDailyAccessTimeRewardReceive(int nEntryId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            DailyAccessTimeRewardReceiveCommandBody cmdBody = new DailyAccessTimeRewardReceiveCommandBody();
            cmdBody.entryId = m_nEntryId = nEntryId;
            CsRplzSession.Instance.Send(ClientCommandName.DailyAccessTimeRewardReceive, cmdBody);
        }
    }

    void OnEventResDailyAccessTimeRewardReceive(int nReturnCode, DailyAccessTimeRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardList.Add(m_nEntryId);
            CsGameData.Instance.MyHeroInfo.ReceivedDailyAccessRewardDate = new DateTime(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Year, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Month, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Day);

            CsGameEventUIToUI.Instance.OnEventDailyAccessTimeRewardReceive();
        }
        else if (nReturnCode == 101)
        {
            // 접속시간이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A37_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 출석보상받기
    public void SendAttendRewardReceive(int nAttendDay)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            AttendRewardReceiveCommandBody cmdBody = new AttendRewardReceiveCommandBody();
            cmdBody.attendDay = m_nEntryId = nAttendDay;
            CsRplzSession.Instance.Send(ClientCommandName.AttendRewardReceive, cmdBody);
        }
    }

    void OnEventResAttendRewardReceive(int nReturnCode, AttendRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDay = m_nEntryId;
            CsGameData.Instance.MyHeroInfo.ReceivedAttendRewardDate = new DateTime(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Year, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Month, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Day);

            CsGameEventUIToUI.Instance.OnEventAttendRewardReceive();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 연속미션보상받기
    public void SendSeriesMissionRewardReceive(int nMissionId, int nStep)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            SeriesMissionRewardReceiveCommandBody cmdBody = new SeriesMissionRewardReceiveCommandBody();
            cmdBody.missionId = m_nMissionId = nMissionId;
            cmdBody.step = m_nStep = nStep;
            CsRplzSession.Instance.Send(ClientCommandName.SeriesMissionRewardReceive, cmdBody);
        }
    }

    void OnEventResSeriesMissionRewardReceive(int nReturnCode, SeriesMissionRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsHeroSeriesMission csHeroSeriesMission = CsGameData.Instance.MyHeroInfo.GetHeroSeriesMission(m_nMissionId);

            if (csHeroSeriesMission.SeriesMission.GetSeriesMissionStep(m_nStep + 1) == null)
            {
                CsGameData.Instance.MyHeroInfo.RemoveHeroSeriesMission(csHeroSeriesMission);
                CsGameEventUIToUI.Instance.OnEventSeriesMissionRewardReceive(null);
            }
            else
            {
                csHeroSeriesMission.CurrentStep++;
                CsGameEventUIToUI.Instance.OnEventSeriesMissionRewardReceive(csHeroSeriesMission);
            }
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 오늘의미션보상받기
    public void SendTodayMissionRewardReceive(DateTime dtDate, int nMissionId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            TodayMissionRewardReceiveCommandBody cmdBody = new TodayMissionRewardReceiveCommandBody();
            cmdBody.date = dtDate;
            cmdBody.missionId = m_nMissionId = nMissionId;
            CsRplzSession.Instance.Send(ClientCommandName.TodayMissionRewardReceive, cmdBody);
        }
    }

    void OnEventResTodayMissionRewardReceive(int nReturnCode, TodayMissionRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsHeroTodayMission csHeroTodayMission = CsGameData.Instance.MyHeroInfo.GetHeroTodayMission(m_nMissionId);
            csHeroTodayMission.RewardReceived = true;

            CsGameEventUIToUI.Instance.OnEventTodayMissionRewardReceive(csHeroTodayMission);
        }
        else if (nReturnCode == 101)
        {
            // 날짜가 현재미션날짜와 다릅니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A37_ERROR_00501"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 연속미션갱신
    void OnEventEvtSeriesMissionUpdated(SEBSeriesMissionUpdatedEventBody eventBody)
    {
        CsHeroSeriesMission csHeroSeriesMission = CsGameData.Instance.MyHeroInfo.GetHeroSeriesMission(eventBody.missionId);

        if (csHeroSeriesMission != null)
        {
            csHeroSeriesMission.ProgressCount = eventBody.progressCount;
            CsGameEventUIToUI.Instance.OnEventSeriesMissionUpdated(csHeroSeriesMission);
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 오늘의미션목록변경
    void OnEventEvtTodayMissionListChanged(SEBTodayMissionListChangedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.AddHeroTodayMissions(eventBody.missions, eventBody.date);

        CsGameEventUIToUI.Instance.OnEventTodayMissionListChanged();
    }

    //---------------------------------------------------------------------------------------------------
    // 오늘의미션갱신
    void OnEventEvtTodayMissionUpdated(SEBTodayMissionUpdatedEventBody eventBody)
    {
        CsHeroTodayMission csHeroTodayMission = CsGameData.Instance.MyHeroInfo.GetHeroTodayMission(eventBody.missionId);
        if (csHeroTodayMission != null)
        {
            csHeroTodayMission.ProgressCount = eventBody.progressCount;
            CsGameEventUIToUI.Instance.OnEventTodayMissionUpdated(csHeroTodayMission);
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 신병선물받기
	public void SendRookieGiftReceive()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RookieGiftReceiveCommandBody cmdBody = new RookieGiftReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RookieGiftReceive, cmdBody);
		}
	}

	void OnEventResRookieGiftReceive(int nReturnCode, RookieGiftReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsRookieGift csRookieGift = CsGameData.Instance.GetRookieGift(CsGameData.Instance.MyHeroInfo.RookieGiftNo);

			if (csRookieGift != null)
			{
				foreach (var reward in csRookieGift.RookieGiftRewardList)
				{
					List<CsDropObject> listLooted = new List<CsDropObject>();
					List<CsDropObject> listNotLooted = new List<CsDropObject>();

					listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, reward.ItemReward.Item.ItemId, reward.ItemReward.ItemOwned, reward.ItemReward.ItemCount));

					CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
				}
			}

			CsGameData.Instance.MyHeroInfo.RookieGiftNo = responseBody.rookieGiftNo;
			CsGameData.Instance.MyHeroInfo.RookieGiftRemainingTime = responseBody.rookieGiftRemainingTime;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventRookieGiftReceive();
		}
		else if (nReturnCode == 101)
		{
			// 대기시간이 경과되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A99_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A99_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 오픈선물받기
	public void SendOpenGiftReceive(int nDay)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			OpenGiftReceiveCommandBody cmdBody = new OpenGiftReceiveCommandBody();
			cmdBody.day = m_nDay = nDay;
			CsRplzSession.Instance.Send(ClientCommandName.OpenGiftReceive, cmdBody);
		}
	}

	void OnEventResOpenGiftReceive(int nReturnCode, OpenGiftReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.ReceivedOpenGiftRewardList.Add(m_nDay);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventOpenGiftReceive(m_nDay);

			CsOpenGiftReward csOpenGiftReward = CsGameData.Instance.GetOpenGiftReward(m_nDay);

			if (csOpenGiftReward != null)
			{
				List<CsDropObject> listLooted = new List<CsDropObject>();
				List<CsDropObject> listNotLooted = new List<CsDropObject>();

				listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, csOpenGiftReward.ItemReward.Item.ItemId, csOpenGiftReward.ItemReward.ItemOwned, csOpenGiftReward.ItemReward.ItemCount));

				CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A99_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 일차가 경과되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A99_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A99_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


    #endregion Support

    #region Mount
    //---------------------------------------------------------------------------------------------------
    // 탈것장착
    public void SendMountEquip(int nMountId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountEquipCommandBody cmdBody = new MountEquipCommandBody();
            cmdBody.mountId = m_nMountId = nMountId;
            CsRplzSession.Instance.Send(ClientCommandName.MountEquip, cmdBody);
        }
    }

    void OnEventResMountEquip(int nReturnCode, MountEquipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroMount csHeroMountOld = CsGameData.Instance.MyHeroInfo.GetHeroMount(CsGameData.Instance.MyHeroInfo.EquippedMountId);
            CsHeroMount csHeroMountNew = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
            CsGameData.Instance.MyHeroInfo.EquippedMountId = m_nMountId;

            // 전투력 
            if (csHeroMountOld != null)
            {
                csHeroMountOld.UpdateBattlePower();
            }

            csHeroMountNew.UpdateBattlePower();

            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.IsRiding = responseBody.isRiding;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            // InGame
            CsGameEventToIngame.Instance.OnEventHeroMountEquipped(csHeroMountNew);

            CsGameEventUIToUI.Instance.OnEventMountEquip();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 탈것레벨업
    public void SendMountLevelUp(int nMountId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountLevelUpCommandBody cmdBody = new MountLevelUpCommandBody();
            cmdBody.mountId = m_nMountId = nMountId;
            CsRplzSession.Instance.Send(ClientCommandName.MountLevelUp, cmdBody);
        }
    }

    void OnEventResMountLevelUp(int nReturnCode, MountLevelUpResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            PDInventorySlot[] pDInventorySlot = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(pDInventorySlot);

            CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
            int nOldLevel = csHeroMount.Level;
            csHeroMount.Level = responseBody.mountLevel;
            csHeroMount.Satiety = responseBody.mountSatiety;

            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMountLevelUp((nOldLevel == csHeroMount.Level) ? false : true);
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A19_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 탈것타기
    public void SendMountGetOn()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGetOnCommandBody cmdBody = new MountGetOnCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.MountGetOn, cmdBody);
        }
    }

    void OnEventResMountGetOn(int nReturnCode, MountGetOnResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            //CsGameData.Instance.MyHeroInfo.MoveSpeed = responseBody.moveSpeed;
            CsGameEventToIngame.Instance.OnEventMountGetOn();
            CsGameEventUIToUI.Instance.OnEventDisplayMountToggleIsOn(true);
            // 
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A04_ERROR_00101"));
            CsGameEventUIToUI.Instance.OnEventDisplayMountToggleIsOn(false);
        }
        else if (nReturnCode == 102)
        {
            // 영웅 전투상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A04_ERROR_00102"));
            CsGameEventUIToUI.Instance.OnEventDisplayMountToggleIsOn(false);
        }
        else if (nReturnCode == 103)
        {
            // 이미 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A04_ERROR_00103"));
            CsGameEventUIToUI.Instance.OnEventDisplayMountToggleIsOn(false);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
            CsGameEventUIToUI.Instance.OnEventDisplayMountToggleIsOn(false);
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 탈것각성레벨업
	public void SendMountAwakeningLevelUp(int nMountId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			MountAwakeningLevelUpCommandBody cmdBody = new MountAwakeningLevelUpCommandBody();
			cmdBody.mountId = m_nMountId = nMountId;
			CsRplzSession.Instance.Send(ClientCommandName.MountAwakeningLevelUp, cmdBody);
		}
	}

	void OnEventResMountAwakeningLevelUp(int nReturnCode, MountAwakeningLevelUpResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
			csHeroMount.AwakeningLevel = responseBody.awakningLevel;
			csHeroMount.AwakeningExp = responseBody.awakningExp;

			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInvetorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventMountAwakeningLevelUp();
		}
		else if (nReturnCode == 101)
		{
			// 마지막 각성레벨입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A158_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A158_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A158_ERROR_00103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것속성물약사용
	public void SendMountAttrPotionUse(int nMountId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			MountAttrPotionUseCommandBody cmdBody = new MountAttrPotionUseCommandBody();
			cmdBody.mountId = m_nMountId = nMountId;
			CsRplzSession.Instance.Send(ClientCommandName.MountAttrPotionUse, cmdBody);
		}
	}

	void OnEventResMountAttrPotionUse(int nReturnCode, MountAttrPotionUseResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
			csHeroMount.PotionAttrCount = responseBody.potionAttrCount;

			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventMountAttrPotionUse();
		}
		else if (nReturnCode == 101)
		{
			// 사용횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A158_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A158_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 탈것속성물약사용
	public void SendMountItemUse(int nSlotIndex)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			MountItemUseCommandBody cmdBody = new MountItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.MountItemUse, cmdBody);
		}
	}

	void OnEventResMountItemUse(int nReturnCode, MountItemUseResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			PDHeroMount[] mounts = new PDHeroMount[] { responseBody.addedMount };
			CsGameData.Instance.MyHeroInfo.AddHeroMounts(mounts);

			PDInventorySlot[] slots = new PDInventorySlot[] { responseBody .changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventMountItemUse();
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 보유하고 있는 탈것입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00802"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Mount

	#region MountGear

	//---------------------------------------------------------------------------------------------------
	// 탈것장비장착
	public void SendMountGearEquip(Guid guidHeroMountGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGearEquipCommandBody cmdBody = new MountGearEquipCommandBody();
            cmdBody.heroMountGearId = m_guidHeroMountGearId = guidHeroMountGearId;
            CsRplzSession.Instance.Send(ClientCommandName.MountGearEquip, cmdBody);
        }
    }

    void OnEventResMountGearEquip(int nReturnCode, MountGearEquipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 인벤토리슬롯정보
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.GetInventorySlotByHeroMountGearId(m_guidHeroMountGearId);

            // 기존장착 정보
            CsHeroMountGear csHeroMountGearOld = CsGameData.Instance.MyHeroInfo.GetEquippedMountGearBySlotIndex(csInventorySlot.InventoryObjectMountGear.HeroMountGear.MountGear.MountGearType.SlotIndex);

            // 인벤토리 변경 - 기존 인벤토리 삭제.
            CsGameData.Instance.MyHeroInfo.RemoveInventorySlot(csInventorySlot, false);

            if (csHeroMountGearOld != null && responseBody.changedInventorySlotIndex > -1)
            {
                CsGameData.Instance.MyHeroInfo.RemoveEquippedMountGear(csHeroMountGearOld.Id);

                CsInventoryObjectMountGear csInventoryObjectMountGear = new CsInventoryObjectMountGear((int)EnInventoryObjectType.MountGear, csHeroMountGearOld);
                CsInventorySlot csInventorySlotNew = new CsInventorySlot(responseBody.changedInventorySlotIndex, csInventoryObjectMountGear);
                CsGameData.Instance.MyHeroInfo.AddInventorySlot(csInventorySlotNew);
            }

            CsGameData.Instance.MyHeroInfo.AddEquippedMountGear(csInventorySlot.InventoryObjectMountGear.HeroMountGear.Id);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMountGearEquip(m_guidHeroMountGearId);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 탈것장비장착해제
    public void SendMountGearUnequip(Guid guidHeroMountGearId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGearUnequipCommandBody cmdBody = new MountGearUnequipCommandBody();
            cmdBody.heroMountGearId = m_guidHeroMountGearId = guidHeroMountGearId;
            CsRplzSession.Instance.Send(ClientCommandName.MountGearUnequip, cmdBody);
        }
    }

    void OnEventResMountGearUnequip(int nReturnCode, MountGearUnequipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 기존장착장비
            CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(m_guidHeroMountGearId);

            // 장착정보 리셋
            CsGameData.Instance.MyHeroInfo.RemoveEquippedMountGear(m_guidHeroMountGearId);

            // 인벤토리 추가
            CsInventoryObjectMountGear csInventoryObjectMountGear = new CsInventoryObjectMountGear((int)EnInventoryObjectType.MountGear, csHeroMountGear);
            CsInventorySlot csInventorySlotNew = new CsInventorySlot(responseBody.changedInventorySlotIndex, csInventoryObjectMountGear);
            CsGameData.Instance.MyHeroInfo.AddInventorySlot(csInventorySlotNew);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMountGearUnequip(m_guidHeroMountGearId);

        }
        else if (nReturnCode == 101)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A02_ERROR_01701"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 탈것장비재강화
    public void SendMountGearRefine(Guid guidHeroMountGearId, int nOptionAttrIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGearRefineCommandBody cmdBody = new MountGearRefineCommandBody();
            cmdBody.heroMountGearId = m_guidHeroMountGearId = guidHeroMountGearId;
            cmdBody.optionAttrIndex = m_nOptionAttrIndex = nOptionAttrIndex;
            CsRplzSession.Instance.Send(ClientCommandName.MountGearRefine, cmdBody);
        }
    }

    void OnEventResMountGearRefine(int nReturnCode, MountGearRefineResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(m_guidHeroMountGearId);

            CsGameData.Instance.MyHeroInfo.MountGearRefinementDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.MountGearRefinementDailyCount = responseBody.mountGearRefinementDailyCount;

            PDInventorySlot[] pDInventorySlot = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(pDInventorySlot);

            // 속성변경
            csHeroMountGear.UpdateOptionAttr(m_nOptionAttrIndex, responseBody.changedHeroMountGearOptionAttr);

            // 나의 영웅정보(Hp, MaxHp) 변경
            CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            // 전투력 갱신
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventMountGearRefine(m_guidHeroMountGearId);
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A20_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

    }

    //---------------------------------------------------------------------------------------------------
    // 탈것장비뽑기상자제작
    public void SendMountGearPickBoxMake(int nMountGearPickBoxItemId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGearPickBoxMakeCommandBody cmdBody = new MountGearPickBoxMakeCommandBody();
            cmdBody.mountGearPickBoxItemId = nMountGearPickBoxItemId;
            CsRplzSession.Instance.Send(ClientCommandName.MountGearPickBoxMake, cmdBody);
        }
    }

    void OnEventResMountGearPickBoxMake(int nReturnCode, MountGearPickBoxMakeResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameEventUIToUI.Instance.OnEventMountGearPickBoxMake();
        }
        else if (nReturnCode == 101)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 재료아이템1이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            //  재료아이템2가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 재료아이템3이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 재료아이템4가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00105"));
        }
        else if (nReturnCode == 106)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A21_ERROR_00106"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 탈것장비뽑기상자모두제작
    public void SendMountGearPickBoxMakeTotally(int nMountGearPickBoxItemId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            MountGearPickBoxMakeTotallyCommandBody cmdBody = new MountGearPickBoxMakeTotallyCommandBody();
            cmdBody.mountGearPickBoxItemId = nMountGearPickBoxItemId;
            CsRplzSession.Instance.Send(ClientCommandName.MountGearPickBoxMakeTotally, cmdBody);
        }
    }

    void OnEventResMountGearPickBoxMakeTotally(int nReturnCode, MountGearPickBoxMakeTotallyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameEventUIToUI.Instance.OnEventMountGearPickBoxMakeTotally();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion MountGear

    #region Hero
    public void SendHeroInfo(Guid guidHeroId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            HeroInfoCommandBody cmdBody = new HeroInfoCommandBody();
            cmdBody.heroId = m_guidHeroId = guidHeroId;
            CsRplzSession.Instance.Send(ClientCommandName.HeroInfo, cmdBody);
        }
    }

    void OnEventResHeroInfo(int nReturnCode, HeroInfoResponseBody responseBody)
    {
        m_bProcessing = false;
        if (nReturnCode == 0)
        {
            CsHeroInfo csHeroInfo = new CsHeroInfo(m_guidHeroId,
                                                   responseBody.name,
                                                   responseBody.nationId,
                                                   responseBody.jobId,
                                                   responseBody.level,
                                                   responseBody.isLoggedIn,
                                                   responseBody.battlePower,
                                                   responseBody.equippedWingId,
                                                   responseBody.mainGearEnchantLevelSetNo,
                                                   responseBody.subGearSoulstoneLevelSetNo,
                                                   responseBody.equippedWeapon,
                                                   responseBody.equippedArmor,
                                                   responseBody.equippedHeroSubGears,
                                                   responseBody.heroWingParts,
                                                   responseBody.realAttrValues,
                                                   responseBody.wingStep,
                                                   responseBody.wingLevel,
                                                   responseBody.wings,
                                                   responseBody.guildId,
                                                   responseBody.guildName,
                                                   responseBody.guildMemberGrade,
                                                   responseBody.customPresetHair,
                                                   responseBody.customFaceJawHeight,
                                                   responseBody.customFaceJawWidth,
                                                   responseBody.customFaceJawEndHeight,
                                                   responseBody.customFaceWidth,
                                                   responseBody.customFaceEyebrowHeight,
                                                   responseBody.customFaceEyebrowRotation,
                                                   responseBody.customFaceEyesWidth,
                                                   responseBody.customFaceNoseHeight,
                                                   responseBody.customFaceNoseWidth,
                                                   responseBody.customFaceMouthHeight,
                                                   responseBody.customFaceMouthWidth,
                                                   responseBody.customBodyHeadSize,
                                                   responseBody.customBodyArmsLength,
                                                   responseBody.customBodyArmsWidth,
                                                   responseBody.customBodyChestSize,
                                                   responseBody.customBodyWaistWidth,
                                                   responseBody.customBodyHipsSize,
                                                   responseBody.customBodyPelvisWidth,
                                                   responseBody.customBodyLegsLength,
                                                   responseBody.customBodyLegsWidth,
                                                   responseBody.customColorSkin,
                                                   responseBody.customColorEyes,
                                                   responseBody.customColorBeardAndEyebrow,
                                                   responseBody.customColorHair,
												   responseBody.equippedCostumeId,
												   responseBody.appliedCostumeEffectId,
												   responseBody.displayTitleId										   
												   );

            CsGameEventUIToUI.Instance.OnEventHeroInfo(csHeroInfo);

        }
        else if (nReturnCode == 101)
        {
            // 해당 영웅이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A15_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 영웅위치
	public void SendHeroPosition(Guid guidHeroId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			HeroPositionCommandBody cmdBody = new HeroPositionCommandBody();
			cmdBody.heroId = m_guidHeroId = guidHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.HeroPosition, cmdBody);
		}
	}

	void OnEventResHeroPosition(int nReturnCode, HeroPositionResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			if (responseBody.isLoggedIn)
			{
				CsGameEventToIngame.Instance.OnEventPartyCalled(responseBody.locationId, responseBody.locationParam, new Vector3(responseBody.position.x, responseBody.position.y, responseBody.position.z));
                CsGameEventUIToUI.Instance.OnEventHeroPosition();
			}
			else
			{
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A36_TXT_04020"));
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


    #endregion Hero

    #region Wing

    //---------------------------------------------------------------------------------------------------
    // 날개장착
    public void SendWingEquip(int nWingId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            WingEquipCommandBody cmdBody = new WingEquipCommandBody();
            cmdBody.wingId = m_nWingId = nWingId;
            CsRplzSession.Instance.Send(ClientCommandName.WingEquip, cmdBody);
        }
    }

    void OnEventResWingEquip(int nReturnCode, WingEquipResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.EquippedWingId = m_nWingId;

            CsGameEventUIToUI.Instance.OnEventWingEquip();
            CsGameEventToIngame.Instance.OnEventWingEquip();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 날개강화
    public void SendWingEnchant(int nWingPartId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            WingEnchantCommandBody cmdBody = new WingEnchantCommandBody();
            cmdBody.wingPartId = m_nWingPartId = nWingPartId;
            CsRplzSession.Instance.Send(ClientCommandName.WingEnchant, cmdBody);
        }
    }

    void OnEventResWingEnchant(int nReturnCode, WingEnchantResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroWingPart csHeroWingPart = CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nWingPartId);
            csHeroWingPart.UpdateHeroWingEnchant(responseBody.changedEnchant);
            
            CsGameData.Instance.MyHeroInfo.WingStep = responseBody.wingStep;
            CsGameData.Instance.MyHeroInfo.WingLevel = responseBody.wingLevel;
            CsGameData.Instance.MyHeroInfo.WingExp = responseBody.wingExp;

            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            if (responseBody.addedWing != null)
            {
                CsGameData.Instance.MyHeroInfo.AddHeroWing(responseBody.addedWing);
            }

            CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventWingEnchant();
        }
        else if (nReturnCode == 101)
        {
            //  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A22_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 날개전체강화
    public void SendWingEnchantTotally(int nWingPartId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            WingEnchantTotallyCommandBody cmdBody = new WingEnchantTotallyCommandBody();
            cmdBody.wingPartId = m_nWingPartId = nWingPartId;
            CsRplzSession.Instance.Send(ClientCommandName.WingEnchantTotally, cmdBody);
        }
    }

    void OnEventResWingEnchantTotally(int nReturnCode, WingEnchantTotallyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsHeroWingPart csHeroWingPart = CsGameData.Instance.MyHeroInfo.GetHeroWingPart(m_nWingPartId);
            csHeroWingPart.UpdateHeroWingEnchant(responseBody.changedEnchant);

            CsGameData.Instance.MyHeroInfo.WingStep = responseBody.wingStep;
            CsGameData.Instance.MyHeroInfo.WingLevel = responseBody.wingLevel;
            CsGameData.Instance.MyHeroInfo.WingExp = responseBody.wingExp;

            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (responseBody.addedWing != null)
			{
				CsGameData.Instance.MyHeroInfo.AddHeroWing(responseBody.addedWing);
			}

			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHp;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

            CsGameEventUIToUI.Instance.OnEventWingEnchantTotally();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 날개획득
    void OnEventEvtWingAcquisition(SEBWingAcquisitionEventBody eventBody)
    {
		CsGameData.Instance.MyHeroInfo.AddHeroWing(eventBody.wing);

		CsGameEventUIToUI.Instance.OnEventWingAcquisition();

        if (CsGameData.Instance.MyHeroInfo.EquippedWingId == 0)
        {
            SendWingEquip(eventBody.wing.wingId);
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 날개기억조각장착
	public void SendWingMemoryPieceInstall(int nWingId, int nWingMemoryPieceType)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WingMemoryPieceInstallCommandBody cmdBody = new WingMemoryPieceInstallCommandBody();
			cmdBody.wingId = m_nWingId = nWingId;
			cmdBody.wingMemoryPieceType = nWingMemoryPieceType;
			CsRplzSession.Instance.Send(ClientCommandName.WingMemoryPieceInstall, cmdBody);
		}
	}

	void OnEventResWingMemoryPieceInstall(int nReturnCode, WingMemoryPieceInstallResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.UpdateHeroWing(m_nWingId, responseBody.memoryPieceStep, responseBody.changedWingMemoryPieceSlots);

			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventWingMemoryPieceInstall();
		}
		else if (nReturnCode == 101)
		{
			// 기억조각을 전부 장착한 날개입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A118_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			//  아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A118_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Wing

	#region StoryDungeon

	////---------------------------------------------------------------------------------------------------
	//// 스토리던전입장을위한대륙퇴장
	//public void SendContinentExitForStoryDungeonEnter(int nDungeonNo, int nDifficulty)
	//{
	//	if (!m_bProcessing)
	//	{
	//		m_bProcessing = true;

	//		ContinentExitForStoryDungeonEnterCommandBody cmdBody = new ContinentExitForStoryDungeonEnterCommandBody();
	//		cmdBody.dungeonNo = m_nDungeonNo = nDungeonNo;
	//		cmdBody.difficulty = m_nDifficulty = nDifficulty;
	//		CsRplzSession.Instance.Send(ClientCommandName.ContinentExitForStoryDungeonEnter, cmdBody);
	//	}
	//}

	//void OnEventResContinentExitForStoryDungeonEnter(int nReturnCode, ContinentExitForStoryDungeonEnterResponseBody responseBody)
	//{
	//	if (nReturnCode == 0)
	//	{
	//		CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(m_nDungeonNo);

	//		CsGameEventUIToUI.Instance.OnEventContinentExitForStoryDungeonEnter(csStoryDungeon.SceneName);
	//	}
	//	else if (nReturnCode == 101)
	//	{
	//		// 영웅의 레벨이 낮아 해당 던전에 입장할 수 없습니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 102)
	//	{
	//		// 영웅의 레벨이 높아 해당 던전에 입장할 수 없습니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 103)
	//	{
	//		// 영웅이 죽은상태입니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 104)
	//	{
	//		// 영웅이 전투상태입니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 105)
	//	{
	//		// 스태미나가 부족합니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 106)
	//	{
	//		// 입장횟수가 초과되었습니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else
	//	{
	//		CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
	//	}

	//	m_bProcessing = false;
	//}

	////---------------------------------------------------------------------------------------------------
	//// 스토리던전포기
	//void SendStoryDungeonAbandon()
	//{
	//	if (!m_bProcessing)
	//	{
	//		m_bProcessing = true;

	//		StoryDungeonAbandonCommandBody cmdBody = new StoryDungeonAbandonCommandBody();
	//		CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonAbandon, cmdBody);
	//	}
	//}

	//void OnEventResStoryDungeonAbandon(int nReturnCode, StoryDungeonAbandonResponseBody responseBody)
	//{
	//	if (nReturnCode == 0)
	//	{
	//		CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.previousNationId;


	//	}
	//	else if (nReturnCode == 101)
	//	{
	//		// 던전상태가 유효하지 않습니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else if (nReturnCode == 102)
	//	{
	//		// 죽은 상태입니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else
	//	{
	//		CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
	//	}

	//	m_bProcessing = false;
	//}

	////---------------------------------------------------------------------------------------------------
	//// 스토리던전퇴장
	//void SendStoryDungeonExit()
	//{
	//	if (!m_bProcessing)
	//	{
	//		m_bProcessing = true;

	//		StoryDungeonExitCommandBody cmdBody = new StoryDungeonExitCommandBody();
	//		CsRplzSession.Instance.Send(ClientCommandName.StoryDungeonExit, cmdBody);
	//	}
	//}

	//void OnEventResStoryDungeonExit(int nReturnCode, StoryDungeonExitResponseBody responseBody)
	//{
	//	if (nReturnCode == 0)
	//	{

	//	}
	//	else if (nReturnCode == 101)
	//	{
	//		// 던전상태가 유효하지 않습니다.
	//		//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
	//	}
	//	else
	//	{
	//		CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
	//	}

	//	m_bProcessing = false;
	//}

	////---------------------------------------------------------------------------------------------------
	//// 스토리던전부활
	//void OnEventResStoryDungeonRevive(int nReturnCode, StoryDungeonReviveResponseBody responseBody)
	//{

	//}

	////---------------------------------------------------------------------------------------------------
	//// 스토리던전소탕
	//void OnEventResStoryDungeonSweep(int nReturnCode, StoryDungeonSweepResponseBody responseBody)
	//{

	//}

	#endregion StoryDungeon

	#region Stamina

	void OnEventEvtStaminaAutoRecovery(SEBStaminaAutoRecoveryEventBody eventBody)
    {
		CsGameData.Instance.MyHeroInfo.SetStamina(true, eventBody.stamina);

        CsGameEventUIToUI.Instance.OnEventStaminaAutoRecovery();
    }

    //---------------------------------------------------------------------------------------------------
    // 체력구매
    public void SendStaminaBuy()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            StaminaBuyCommandBody cmdBody = new StaminaBuyCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.StaminaBuy, cmdBody);
        }
    }

    void OnEventResStaminaBuy(int nReturnCode, StaminaBuyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.StaminaBuyCountDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.DailyStaminaBuyCount = responseBody.dailyBuyCount;

			CsGameData.Instance.MyHeroInfo.SetStamina(false, responseBody.stamina);

            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
            CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

            CsGameEventUIToUI.Instance.OnEventStaminaBuy();
        }
        else if (nReturnCode == 101)
        {
            // 금일 구매횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A90_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A90_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    void OnEventEvtStaminaScheduleRecovery(SEBStaminaScheduleRecoveryEventBody eventBody)
    {
		CsGameData.Instance.MyHeroInfo.SetStamina(false, eventBody.stamina);
        CsGameEventUIToUI.Instance.OnEventStaminaScheduleRecovery();
    }

    #endregion Stamina

    #region ContinentTransmission

    //---------------------------------------------------------------------------------------------------
    // 대륙전송
    public void SendContinentTransmission(int nNpcId, int nExitNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ContinentTransmissionCommandBody cmdBody = new ContinentTransmissionCommandBody();
            cmdBody.npcId = m_nNpcId = nNpcId;
            cmdBody.exitNo = m_nExitNo = nExitNo;
            CsRplzSession.Instance.Send(ClientCommandName.ContinentTransmission, cmdBody);
        }
    }

    void OnEventResContinentTransmission(int nReturnCode, ContinentTransmissionResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(m_nNpcId);
            CsContinentTransmissionExit csContinentTransmissionExit = csNpcInfo.GetContinentTransmissionExit(m_nExitNo);

            CsGameEventUIToUI.Instance.OnEventContinentTransmission(csContinentTransmissionExit.Continent.SceneName);
        }
        else if (nReturnCode == 101)
        {
            // NPC와의 거리가 너무 멉니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A42_ERROR_00501"));
        }
        else if (nReturnCode == 102)
        {
            // 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A42_ERROR_00502"));
        }
        else if (nReturnCode == 103)
        {
            // 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A42_ERROR_00503"));
        }
        else if (nReturnCode == 104)
        {
            // 전투중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A42_ERROR_00504"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion ContinentTransmission

    #region Nation

    //---------------------------------------------------------------------------------------------------
    // 국가전송
    public void SendNationTransmission(int nNpcId, int nNationId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationTransmissionCommandBody cmdBody = new NationTransmissionCommandBody();
            cmdBody.npcId = m_nNpcId = nNpcId;
            cmdBody.nationId = m_nNationId = nNationId;
            CsRplzSession.Instance.Send(ClientCommandName.NationTransmission, cmdBody);
        }
    }
	  
    void OnEventResNationTransmission(int nReturnCode, NationTransmissionResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            //CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(m_nNpcId);
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = m_nNationId;
            CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId);
            CsGameEventUIToUI.Instance.OnEventNationTransmission(csContinent.SceneName);
        }
        else if (nReturnCode == 101)
        {
            // NPC와의 거리가 너무 멉니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A44_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A44_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 죽은 상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A44_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 104 : 대상 장소는 국가전중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A44_ERROR_00104"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 영웅검색
    public void SendHeroSearch(string strHeroName)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            HeroSearchCommandBody cmdBody = new HeroSearchCommandBody();
            cmdBody.searchName = strHeroName;
            CsRplzSession.Instance.Send(ClientCommandName.HeroSearch, cmdBody);
        }
    }

    void OnEventResHeroSearch(int nReturnCode, HeroSearchResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            List<CsSearchHero> list = new List<CsSearchHero>();

            for (int i = 0; i < responseBody.heroes.Length; i++)
            {
                list.Add(new CsSearchHero(responseBody.heroes[i]));
            }

            CsGameEventUIToUI.Instance.OnEventHeroSearch(list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가기부
    public void SendNationDonate(int nEntryId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationDonateCommandBody cmdBody = new NationDonateCommandBody();
            cmdBody.entryId = nEntryId;
            CsRplzSession.Instance.Send(ClientCommandName.NationDonate, cmdBody);
        }
    }

    void OnEventResNationDonate(int nReturnCode, NationDonateResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.NationDonationCountDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.DailyNationDonationCount = responseBody.dailyNationDonationCount;

            CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
            CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

            CsGameData.Instance.MyHeroInfo.ExploitPoint = responseBody.exploitPoint;
            CsGameData.Instance.MyHeroInfo.DailyExploitPoint = responseBody.dailyExploitPoint;

            CsGameData.Instance.MyHeroInfo.NationFund = responseBody.nationFund;

            CsGameEventUIToUI.Instance.OnEventNationDonate(responseBody.acquiredExploitPoint);
        }
        else if (nReturnCode == 101)
        {
            // 일일기부횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00401"));
        }
        else if (nReturnCode == 102)
        {
            // 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00402"));
        }
        else if (nReturnCode == 103)
        {
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00403"));
        }
		else if (nReturnCode == 104)
		{
			// 필요한 메인퀘스트를 완료하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00404"));
		}
		else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가관직임명
    public void SendNationNoblesseAppoint(int nTargetNoblesseId, Guid guidTargetHeroId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationNoblesseAppointCommandBody cmdBody = new NationNoblesseAppointCommandBody();
            cmdBody.targetNoblesseId = nTargetNoblesseId;
            cmdBody.targetHeroId = guidTargetHeroId;
            CsRplzSession.Instance.Send(ClientCommandName.NationNoblesseAppoint, cmdBody);
        }
    }

    void OnEventResNationNoblesseAppoint(int nReturnCode, NationNoblesseAppointResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameEventUIToUI.Instance.OnEventNationNoblesseAppoint();
        }
        else if (nReturnCode == 101)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00501"));
        }
        else if (nReturnCode == 102)
        {
            // 대상 관직에는 이미 임명자가 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00502"));
        }
        else if (nReturnCode == 103)
        {
            // 대상 영웅은 이미 관직을 가지고 있습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00503"));
        }
        else if (nReturnCode == 104)
        {
            // 금일 이미 대상관직에 대해 임명했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00504"));
        }
        else if (nReturnCode == 105)
        {
            // 대상 영웅이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00505"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가관직해임
    public void SendNationNoblesseDismiss(int nTargetNoblesseId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationNoblesseDismissCommandBody cmdBody = new NationNoblesseDismissCommandBody();
            cmdBody.targetNoblesseId = nTargetNoblesseId;
            CsRplzSession.Instance.Send(ClientCommandName.NationNoblesseDismiss, cmdBody);
        }
    }

    void OnEventResNationNoblesseDismiss(int nReturnCode, NationNoblesseDismissResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameEventUIToUI.Instance.OnEventNationNoblesseDismiss();
        }
        else if (nReturnCode == 101)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00601"));
        }
        else if (nReturnCode == 102)
        {
            // 대상 관직에는 임명자가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00602"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가관직임명 이벤트
    void OnEventEvtNationNoblesseAppointment(SEBNationNoblesseAppointmentEventBody eventBody)
    {
		CsGameData.Instance.MyHeroInfo.AddNationNoblesse(eventBody.nationId, eventBody.noblesseId, eventBody.heroId, eventBody.heroName, eventBody.heroJobId, eventBody.appointmentDate);

        if (eventBody.heroId == CsGameData.Instance.MyHeroInfo.HeroId)
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

        CsGameEventUIToUI.Instance.OnEventNationNoblesseAppointed(eventBody.noblesseId, eventBody.heroName);
		//CsGameEventUIToUI.Instance.OnEventHeroNationNoblesseChanged(eventBody.heroId, eventBody.noblesseId);
	}

    //---------------------------------------------------------------------------------------------------
    // 국가관직해임 이벤트
    void OnEventEvtNationNoblesseDismissal(SEBNationNoblesseDismissalEventBody eventBody)
    {
		CsNationInstance csNationInstance = CsGameData.Instance.MyHeroInfo.GetNationInstance(eventBody.nationId);

		CsNationNoblesseInstance csNationNoblesseInstance = csNationInstance.GetNationNoblesseInstance(eventBody.noblesseId);
		Guid guidHeroId = csNationNoblesseInstance.HeroId;

		CsGameData.Instance.MyHeroInfo.RemoveNationNoblesse(eventBody.nationId, eventBody.noblesseId);

        if (guidHeroId == CsGameData.Instance.MyHeroInfo.HeroId)
            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

        CsGameEventUIToUI.Instance.OnEventNationNoblesseDismissed();
		//CsGameEventUIToUI.Instance.OnEventHeroNationNoblesseChanged(guidHeroId, 0);
	}

    //---------------------------------------------------------------------------------------------------
    // 국가자금변경 이벤트
    void OnEventEvtNationFundChanged(SEBNationFundChangedEventBody eventBody)
    {
		CsNationInstance csNationInstance = CsGameData.Instance.MyHeroInfo.GetNationInstance(eventBody.nationId);
		csNationInstance.Fund = eventBody.fund;

        CsGameEventUIToUI.Instance.OnEventNationFundChanged();
    }

    //---------------------------------------------------------------------------------------------------
    // 국가소집
    public void SendNationCall(int nSlotIndex)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationCallCommandBody cmdBody = new NationCallCommandBody();
            cmdBody.slotIndex = nSlotIndex;
            CsRplzSession.Instance.Send(ClientCommandName.NationCall, cmdBody);
        }
    }

    void OnEventResNationCall(int nReturnCode, NationCallResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            // 인벤토리 변경
            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventNationCall();
        }
        else if (nReturnCode == 101)
        {
            // 관직이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00701"));
        }
        else if (nReturnCode == 102)
        {
            // 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00702"));
        }
        else if (nReturnCode == 103)
        {
            // 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00703"));
        }
        else if (nReturnCode == 104)
        {
            // 국가전중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00704"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가소집전송
    public void SendNationCallTransmission(long lCallId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationCallTransmissionCommandBody cmdBody = new NationCallTransmissionCommandBody();
            cmdBody.callId = lCallId;
            CsRplzSession.Instance.Send(ClientCommandName.NationCallTransmission, cmdBody);
        }
    }

    void OnEventResNationCallTransmission(int nReturnCode, NationCallTransmissionResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = responseBody.targetNationId;
            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationCallTransmission;
            Debug.Log(">>>>OnEventResNationCallTransmission<<<< : " + CsGameData.Instance.MyHeroInfo.MyHeroEnterType);

            CsGameEventUIToUI.Instance.OnEventNationCallTransmission(responseBody.targetContinentId);
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00801"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00802"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00803"));
        }
        else if (nReturnCode == 104)
        {
            // 국가소집이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00804"));
        }
        else if (nReturnCode == 105)
        {
            // 국가전중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_00805"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 국가소집 이벤트
    void OnEventEvtNationCall(SEBNationCallEventBody eventBody)
    {
        CsNationCall csNationCall = new CsNationCall(eventBody.call);
        CsGameEventUIToUI.Instance.OnEventNationCalled(csNationCall);
    }

    #endregion Nation

    #region TodayTasks

    //---------------------------------------------------------------------------------------------------
    // 달성보상받기
    public void SendAchievementRewardReceive(DateTime dtDate, int nRewardNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            AchievementRewardReceiveCommandBody cmdBody = new AchievementRewardReceiveCommandBody();
            cmdBody.date = dtDate;
            cmdBody.rewardNo = m_nRewardNo = nRewardNo;
            CsRplzSession.Instance.Send(ClientCommandName.AchievementRewardReceive, cmdBody);
        }
    }
	 
    void OnEventResAchievementRewardReceive(int nReturnCode, AchievementRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
			List<CsDropObject> listLooted = new List<CsDropObject>();
			List<CsDropObject> listNotLooted = new List<CsDropObject>();

			CsGameData.Instance.MyHeroInfo.ReceivedAchievementRewardNo = m_nRewardNo;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			// 장비 또는 아이템
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			for (int i = 0; i < responseBody.changedInventorySlots.Length; i++)
			{
				CsInventorySlot csInventorySlot = new CsInventorySlot(responseBody.changedInventorySlots[i]);

				if (responseBody.changedInventorySlots[i].inventoryObject.type == (int)EnInventoryObjectType.MainGear)
				{
					listLooted.Add(new CsDropObjectMainGear((int)EnDropObjectType.MainGear, csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearId, csInventorySlot.InventoryObjectMainGear.HeroMainGear.Owned, csInventorySlot.InventoryObjectMainGear.HeroMainGear.EnchantLevel));
				}
				else if (responseBody.changedInventorySlots[i].inventoryObject.type == (int)EnInventoryObjectType.Item)
				{
					listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, csInventorySlot.InventoryObjectItem.Item.ItemId, csInventorySlot.InventoryObjectItem.Owned, csInventorySlot.InventoryObjectItem.Count));
				}
			}

			// 획득표시
			CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);

			CsGameEventUIToUI.Instance.OnEventAchievementRewardReceive();
        }
        else if (nReturnCode == 101)
        {
            // 날짜가 현재 오늘의할일날자와 다릅니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A50_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 달성점수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A50_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 오늘의할일갱신
    void OnEventEvtTodayTaskUpdated(SEBTodayTaskUpdatedEventBody eventBody)
    {
        CsHeroTodayTask csHeroTodayTask = CsGameData.Instance.MyHeroInfo.GetHeroTodayTask(eventBody.taskId);

        if (csHeroTodayTask == null)
        {
            CsGameData.Instance.MyHeroInfo.AddHeroTodayTask(eventBody.taskId, eventBody.progressCount, eventBody.date);
        }
        else
        {
            csHeroTodayTask.TodayTaskDate = eventBody.date;
            csHeroTodayTask.ProgressCount = eventBody.progressCount;
        }

        CsGameData.Instance.MyHeroInfo.AchievementDailyPoint = eventBody.achievementDailyPoint;

        CsGameEventUIToUI.Instance.OnEventTodayTaskUpdated();
    }

    #endregion TodayTasks

    #region VIP

    //---------------------------------------------------------------------------------------------------
    // Vip레벨보상받기
    public void SendVipLevelRewardReceive(int nVipLevel)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            VipLevelRewardReceiveCommandBody cmdBody = new VipLevelRewardReceiveCommandBody();
            cmdBody.vipLevel = m_nVipLevel = nVipLevel;
            CsRplzSession.Instance.Send(ClientCommandName.VipLevelRewardReceive, cmdBody);
        }
    }

    void OnEventResVipLevelRewardReceive(int nReturnCode, VipLevelRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
            CsGameData.Instance.MyHeroInfo.ReceivedVipLevelRewardList.Add(m_nVipLevel);

            CsGameEventUIToUI.Instance.OnEventVipLevelRewardReceive();
        }
        else if (nReturnCode == 101)
        {
            //  vip레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A54_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 이미 받은 보상입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A54_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion VIP

    #region Rank

    //---------------------------------------------------------------------------------------------------
    // 계급획득
    public void SendRankAcquire(int nTargetRankNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RankAcquireCommandBody cmdBody = new RankAcquireCommandBody();
            cmdBody.targetRankNo = m_nTargetRankNo = nTargetRankNo;
            CsRplzSession.Instance.Send(ClientCommandName.RankAcquire, cmdBody);
        }
    }

    void OnEventResRankAcquire(int nReturnCode, RankAcquireResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.RankNo = m_nTargetRankNo;
			CsGameData.Instance.MyHeroInfo.UpdateRankSkill(m_nTargetRankNo);
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventRankAcquire();
        }
        else if (nReturnCode == 101)
        {
            // 공적포인트가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A27_ERROR_00101"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 계급보상받기
    public void SendRankRewardReceive()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            RankRewardReceiveCommandBody cmdBody = new RankRewardReceiveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.RankRewardReceive, cmdBody);
        }
    }

    void OnEventResRankRewardReceive(int nReturnCode, RankRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.RankRewardReceivedDate = responseBody.date;
            CsGameData.Instance.MyHeroInfo.RankRewardReceivedRankNo = CsGameData.Instance.MyHeroInfo.RankNo;
            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 최대 골드
            CsAccomplishmentManager.Instance.MaxGold = responseBody.maxGold;

            CsGameEventUIToUI.Instance.OnEventRankRewardReceive();
        }
        else if (nReturnCode == 101)
        {
            // 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A27_ERROR_00201"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 계급액티브스킬레벨업
	public void SendRankActiveSkillLevelUp(int nTargetSkillId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RankActiveSkillLevelUpCommandBody cmdBody = new RankActiveSkillLevelUpCommandBody();
			cmdBody.targetSkillId = m_nTargetSkillId = nTargetSkillId;
			CsRplzSession.Instance.Send(ClientCommandName.RankActiveSkillLevelUp, cmdBody);
		}
	}

	public void OnEventResRankActiveSkillLevelUp(int nReturnCode, RankActiveSkillLevelUpResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsHeroRankActiveSkill csHeroRankActiveSkill = CsGameData.Instance.MyHeroInfo.GetHeroRankActiveSkill(m_nTargetSkillId);
			csHeroRankActiveSkill.Level++;
		
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventRankActiveSkillLevelUp();
		}
		else if (nReturnCode == 101)
		{
			// 이미 최대레벨입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 골드가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 계급액티브스킬선택
	public void SendRankActiveSkillSelect(int nTargetSkillId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RankActiveSkillSelectCommandBody cmdBody = new RankActiveSkillSelectCommandBody();
			cmdBody.targetSkillId = m_nTargetSkillId = nTargetSkillId;
			CsRplzSession.Instance.Send(ClientCommandName.RankActiveSkillSelect, cmdBody);
		}
	}

	public void OnEventResRankActiveSkillSelect(int nReturnCode, RankActiveSkillSelectResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.SelectedRankActiveSkillId = m_nTargetSkillId;

			CsGameEventUIToUI.Instance.OnEventRankActiveSkillSelect();
		}
		else if (nReturnCode == 101)
		{
			// 스킬쿨타임이 만료되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 계급패시브스킬레벨업
	public void SendRankPassiveSkillLevelUp(int nTargetSkillId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RankPassiveSkillLevelUpCommandBody cmdBody = new RankPassiveSkillLevelUpCommandBody();
			cmdBody.targetSkillId = m_nTargetSkillId = nTargetSkillId;
			CsRplzSession.Instance.Send(ClientCommandName.RankPassiveSkillLevelUp, cmdBody);
		}
	}

	public void OnEventResRankPassiveSkillLevelUp(int nReturnCode, RankPassiveSkillLevelUpResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsHeroRankPassiveSkill csHeroRankPassiveSkill = CsGameData.Instance.MyHeroInfo.GetHeroRankPassiveSkill(m_nTargetSkillId);
			csHeroRankPassiveSkill.Level++;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;
			CsGameData.Instance.MyHeroInfo.SpiritStone = responseBody.spiritStone;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

 			CsGameEventUIToUI.Instance.OnEventRankPassiveSkillLevelUp();
		}
		else if (nReturnCode == 101)
		{
			// 이미 최대레벨입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 정령석이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A28_ERROR_00303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Rank

	#region HonorShop

	//---------------------------------------------------------------------------------------------------
	// 명예상점상품구매
	public void SendHonorShopProductBuy(int nProductId, int nCount)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            HonorShopProductBuyCommandBody cmdBody = new HonorShopProductBuyCommandBody();
            cmdBody.productId = nProductId;
            cmdBody.count = nCount;
            CsRplzSession.Instance.Send(ClientCommandName.HonorShopProductBuy, cmdBody);
        }
    }

    void OnEventResHonorShopProductBuy(int nReturnCode, HonorShopProductBuyResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.HonorPoint = responseBody.honorPoint;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventHonorShopProductBuy();
        }
        else if (nReturnCode == 101)
        {
            // 명예포인트가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A55_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A55_ERROR_00102"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion HonorShop

    #region Ranking

    //---------------------------------------------------------------------------------------------------
    // 서버전투력랭킹
    public void SendServerBattlePowerRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerBattlePowerRankingCommandBody cmdBody = new ServerBattlePowerRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ServerBattlePowerRanking, cmdBody);
        }
    }

    void OnEventResServerBattlePowerRanking(int nReturnCode, ServerBattlePowerRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsRanking(responseBody.myRanking);

            List<CsRanking> list = new List<CsRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventServerBattlePowerRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버직업전투력랭킹
    public void SendServerJobBattlePowerRanking(int nJobId)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerJobBattlePowerRankingCommandBody cmdBody = new ServerJobBattlePowerRankingCommandBody();
            cmdBody.jobId = nJobId;
            CsRplzSession.Instance.Send(ClientCommandName.ServerJobBattlePowerRanking, cmdBody);
        }
    }

    void OnEventResServerJobBattlePowerRanking(int nReturnCode, ServerJobBattlePowerRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsRanking(responseBody.myRanking);

            List<CsRanking> list = new List<CsRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventServerJobBattlePowerRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버레벨랭킹
    public void SendServerLevelRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerLevelRankingCommandBody cmdBody = new ServerLevelRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ServerLevelRanking, cmdBody);
        }
    }

    void OnEventResServerLevelRanking(int nReturnCode, ServerLevelRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsRanking(responseBody.myRanking);

            List<CsRanking> list = new List<CsRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventServerLevelRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버레벨랭킹보상받기
    public void SendServerLevelRankingRewardReceive()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerLevelRankingRewardReceiveCommandBody cmdBody = new ServerLevelRankingRewardReceiveCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ServerLevelRankingRewardReceive, cmdBody);
        }
    }

    void OnEventResServerLevelRankingRewardReceive(int nReturnCode, ServerLevelRankingRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.RewardedDailyServerLevelRankingNo = responseBody.rewardedRankingNo;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            CsGameEventUIToUI.Instance.OnEventServerLevelRankingRewardReceive();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버레벨랭킹보상받기
    public void SendNationBattlePowerRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationBattlePowerRankingCommandBody cmdBody = new NationBattlePowerRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.NationBattlePowerRanking, cmdBody);
        }
    }

    void OnEventResNationBattlePowerRanking(int nReturnCode, NationBattlePowerRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsRanking(responseBody.myRanking);

            List<CsRanking> list = new List<CsRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventNationBattlePowerRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버레벨랭킹보상받기
    public void SendNationExploitPointRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            NationExploitPointRankingCommandBody cmdBody = new NationExploitPointRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.NationExploitPointRanking, cmdBody);
        }
    }

    void OnEventResNationExploitPointRanking(int nReturnCode, NationExploitPointRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsRanking(responseBody.myRanking);

            List<CsRanking> list = new List<CsRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventNationExploitPointRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 일일서버레벨랭킹갱신
    void OnEventEvtDailyServerLevelRankingUpdated(SEBDailyServerLevelRankingUpdatedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.DailyServerLevelRakingNo = eventBody.rankingNo;
        CsGameData.Instance.MyHeroInfo.DailyServerLevelRanking = eventBody.ranking;

        CsGameEventUIToUI.Instance.OnEventDailyServerLevelRankingUpdated();
    }

    //---------------------------------------------------------------------------------------------------
    // 서버크리쳐카드랭킹
    public void SendServerCreatureCardRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerCreatureCardRankingCommandBody cmdBody = new ServerCreatureCardRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ServerCreatureCardRanking, cmdBody);
        }
    }

    void OnEventResServerCreatureCardRanking(int nReturnCode, ServerCreatureCardRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsCreatureCardRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsCreatureCardRanking(responseBody.myRanking);

            List<CsCreatureCardRanking> list = new List<CsCreatureCardRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsCreatureCardRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventServerCreatureCardRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    // 서버도감랭킹
    public void SendServerIllustratedBookRanking()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            ServerIllustratedBookRankingCommandBody cmdBody = new ServerIllustratedBookRankingCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.ServerIllustratedBookRanking, cmdBody);
        }
    }

    void OnEventResServerIllustratedBookRanking(int nReturnCode, ServerIllustratedBookRankingResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsIllustratedBookRanking csRanking = null;

            if (responseBody.myRanking != null)
                csRanking = new CsIllustratedBookRanking(responseBody.myRanking);

            List<CsIllustratedBookRanking> list = new List<CsIllustratedBookRanking>();

            for (int i = 0; i < responseBody.rankings.Length; i++)
            {
                list.Add(new CsIllustratedBookRanking(responseBody.rankings[i]));
            }

            CsGameEventUIToUI.Instance.OnEventServerIllustratedBookRanking(csRanking, list);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

	//---------------------------------------------------------------------------------------------------
	// 일일서버국력랭킹갱신
	void OnEventEvtDailyServerNationPowerRankingUpdated(SEBDailyServerNationPowerRankingUpdatedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Clear();

		for (int i = 0; i < eventBody.rankings.Length; i++)
		{
			CsGameData.Instance.MyHeroInfo.NationPowerRankingList.Add(new CsNationPowerRanking(eventBody.rankings[i]));
		}

		CsGameEventUIToUI.Instance.OnEventDailyServerNationPowerRankingUpdated();
	}

	#endregion Ranking

	#region Attainment

	//---------------------------------------------------------------------------------------------------
	// 도달보상받기
	public void SendAttainmentRewardReceive(int nEntryNo)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            AttainmentRewardReceiveCommandBody cmdBody = new AttainmentRewardReceiveCommandBody();
            cmdBody.entryNo = m_nEntryNo = nEntryNo;
            CsRplzSession.Instance.Send(ClientCommandName.AttainmentRewardReceive, cmdBody);
        }
    }

    void OnEventResAttainmentRewardReceive(int nReturnCode, AttainmentRewardReceiveResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo = m_nEntryNo;
            CsGameData.Instance.MyHeroInfo.AddHeroMainGears(responseBody.addedMainGears);
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

            // 최대획득메인장비등급
            CsAccomplishmentManager.Instance.MaxAcquisitionMainGearGrade = responseBody.maxAcquisitionMainGearGrade;

            CsGameEventUIToUI.Instance.OnEventAttainmentRewardReceive();
        }
        else if (nReturnCode == 101)
        {
            // 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A30_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A30_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A30_ERROR_00103"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Attainment

    #region Continent

    void OnEventEvtContinentBanished(SEBContinentBanishedEventBody eventBody)
    {
        CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam = eventBody.targetNationId;

        CsGameEventUIToUI.Instance.OnEventContinentBanished(eventBody.targetContinentId);
    }

    #endregion Continent

    #region Settings

    //---------------------------------------------------------------------------------------------------
    // 전투설정
    public void SendBattleSettingSet(int nLootingItemMinGrade)
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;

            BattleSettingSetCommandBody cmdBody = new BattleSettingSetCommandBody();
            cmdBody.lootingItemMinGrade = m_nLootingItemMinGrade = nLootingItemMinGrade;
            CsRplzSession.Instance.Send(ClientCommandName.BattleSettingSet, cmdBody);
        }
    }

    void OnEventResBattleSettingSet(int nReturnCode, BattleSettingSetResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo.LootingItemMinGrade = m_nLootingItemMinGrade;
            CsGameEventUIToUI.Instance.OnEventBattleSettingSet();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    #endregion Settings

    #region Notice

    public void OnEventEvtNotice(SEBNoticeEventBody eventBody)
    {
        CsGameEventUIToUI.Instance.OnEventNotice(eventBody.content);
    }

	#endregion Notice

	#region TodayMissionTutorial

    //---------------------------------------------------------------------------------------------------
    // 오늘의미션튜토리얼시작
    public void SendTodayMissionTutorialStart()
    {
        if (!m_bProcessing)
        {
            m_bProcessing = true;
            
            TodayMissionTutorialStartCommandBody cmdBody = new TodayMissionTutorialStartCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.TodayMissionTutorialStart, cmdBody);
        }
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventResTodayMissionTutorialStart(int nReturnCode, TodayMissionTutorialStartResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.TodayMissionTutorialStarted = true;
			CsGameEventUIToUI.Instance.OnEventTodayMissionTutorialStart();
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion TodayMissionTutorial

	//---------------------------------------------------------------------------------------------------
	// 서버최고레벨갱신
	void OnEventEvtServerMaxLevelUpdated(SEBServerMaxLevelUpdatedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.ServerMaxLevel = eventBody.serverMaxLevel;
	}

    #region GroggyMonsterItemSteal
    //---------------------------------------------------------------------------------------------------
    void OnEventResGroggyMonsterItemStealStart(int nReturnCode, GroggyMonsterItemStealStartResponseBody responseBody)
    {
        if (nReturnCode == 0)
        {
            CsGameEventUIToUI.Instance.OnEventGroggyMonsterItemStealStart();
        }
        else if (nReturnCode == 101)
        {
            // 영웅이 죽은상태입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A95_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 영웅이 카트에 탑승중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A95_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 훔치기를 할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A95_ERROR_00103"));
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SendGroggyMonsterItemStealCancel()
    {
        CEBGroggyMonsterItemStealCancelEventBody csEvt = new CEBGroggyMonsterItemStealCancelEventBody();
        CsRplzSession.Instance.Send(ClientEventName.GroggyMonsterItemStealCancel, csEvt);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtGroggyMonsterItemStealCancel(SEBGroggyMonsterItemStealCancelEventBody eventBody)
    {
        CsGameEventUIToUI.Instance.OnEventGroggyMonsterItemStealCancel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtGroggyMonsterItemStealFinished(SEBGroggyMonsterItemStealFinishedEventBody eventBody)
    {
        if (eventBody.booty == null)
        {
            // 몬스터 테이밍 실패
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A17_TXT_04005"));
        }
        else
        {
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

            CsItem csItem = CsGameData.Instance.GetItem(eventBody.booty.id);
            CsGameEventUIToUI.Instance.OnEventGroggyMonsterItemStealFinished(csItem, eventBody.booty.owned, eventBody.booty.count);
        }
    }
    #endregion GroggyMonsterItemSteal

    #region Reconnect

    //---------------------------------------------------------------------------------------------------
	public void SendLogin()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;
			CsRplzSession.Instance.EventResLogin += OnEventResLogin;

			LoginCommandBody cmdBody = new LoginCommandBody();
			cmdBody.virtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId;
			cmdBody.accessToken = CsConfiguration.Instance.User.AccessToken;
			CsRplzSession.Instance.Send(ClientCommandName.Login, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResLogin(int nReturnCode, LoginResponseBody responseBody)
	{
		Debug.Log("OnEventResLogin : " + nReturnCode);
		CsRplzSession.Instance.EventResLogin -= OnEventResLogin;

		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			SendHeroLogin();
		}
		else if (nReturnCode == 101)
		{
			Debug.Log("ERROR (OnEventResLogin) : 101 - 이미 로그인중이거나 로그인되어 있습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 102)
		{
			Debug.Log("ERROR (OnEventResLogin) : 102 - 가상게임서버가 존재하지 않습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 103)
		{
			Debug.Log("ERROR (OnEventResLogin) : 103 - 게임서버가 다릅니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 104)
		{
			Debug.Log("ERROR (OnEventResLogin) : 104 -  사용자가 존재하지 않습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 105)
		{
			Debug.Log("ERROR (OnEventResLogin) : 105 - 엑세스시크릿이 다릅니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 106)
		{
			Debug.Log("ERROR (OnEventResLogin) : 106 - 현재 사용자가 너무 많습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else
		{
			Debug.Log("ERROR (OnEventResLogin) : " + nReturnCode);
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


	//---------------------------------------------------------------------------------------------------
	void SendHeroLogin()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;
			CsRplzSession.Instance.EventResHeroLogin += OnEventResHeroLogin;

			HeroLoginCommandBody cmdBody = new HeroLoginCommandBody();
			cmdBody.id = CsGameData.Instance.MyHeroInfo.HeroId;
			CsRplzSession.Instance.Send(ClientCommandName.HeroLogin, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResHeroLogin(int nReturnCode, HeroLoginResponseBody responseBody)
	{
		Debug.Log("OnEventResHeroLogin : " + nReturnCode);
		CsRplzSession.Instance.EventResHeroLogin -= OnEventResHeroLogin;

        m_bProcessing = false;

        if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo = new CsMyHeroInfo(responseBody.id,
															  responseBody.name,
															  responseBody.nationId,
															  responseBody.jobId,
															  responseBody.level,
															  responseBody.exp,
															  responseBody.maxHP,
															  responseBody.hp,
															  responseBody.date,
															  responseBody.spiritStone,
															  responseBody.paidInventorySlotCount,
															  responseBody.initEntranceLocationId,
															  responseBody.initEntranceLocationParam,
															  responseBody.lak,
															  responseBody.mainGearEnchantDailyCount,
															  responseBody.mainGearRefinementDailyCount,
															  responseBody.ownDia,
															  responseBody.unOwnDia,
															  responseBody.gold,
															  responseBody.freeImmediateRevivalDailyCount,
															  responseBody.paidImmediateRevivalDailyCount,
															  responseBody.restTime,
															  responseBody.party,
															  responseBody.vipPoint,
															  responseBody.expPotionDailyUseCount,
															  responseBody.receivedLevelUpRewards,
															  responseBody.receivedDailyAcessRewards,
															  responseBody.dailyAccessTime,
															  responseBody.receivedAttendRewardDate,
															  responseBody.receivedAttendRewardDay,
															  responseBody.equippedMountId,
															  responseBody.mountGearRefinementDailyCount,
															  responseBody.wings,
															  responseBody.equippedWingId,
															  responseBody.wingStep,
															  responseBody.wingLevel,
															  responseBody.wingExp,
															  responseBody.wingParts,
															  responseBody.freeSweepDailyCount,
															  responseBody.storyDungeonPlays,
															  responseBody.storyDungeonClears,
															  responseBody.stamina,
															  responseBody.staminaAutoRecoveryRemainingTime,
															  responseBody.bIsRiding,
															  responseBody.expDungeonDailyPlayCount,
															  responseBody.expDungeonClearedDifficulties,
															  responseBody.mainGearEnchantLevelSetNo,
															  responseBody.subGearSoulstoneLevelSetNo,
															  responseBody.expScrollDailyUseCount,
															  responseBody.expScrollItemId,
															  responseBody.expScrollRemainingTime,
															  responseBody.goldDungeonDailyPlayCount,
															  responseBody.goldDungeonClearedDifficulties,
															  responseBody.undergroundMazeDailyPlayTime,
															  responseBody.artifactRoomBestFloor,
															  responseBody.artifactRoomCurrentFloor,
															  responseBody.artifactRoomDailyInitCount,
															  responseBody.artifactRoomSweepProgressFloor,
															  responseBody.artifactRoomSweepRemainingTime,
															  responseBody.time,
															  responseBody.exploitPoint,
															  responseBody.dailyExploitPoint,
															  responseBody.seriesMissions,
															  responseBody.todayMissions,
															  responseBody.todayTasks,
															  responseBody.achievementDailyPoint,
															  responseBody.receivedAchievementRewardNo,
															  responseBody.honorPoint,
															  responseBody.receivedVipLevelRewards,
															  responseBody.rankNo,
															  responseBody.rankRewardReceivedDate,
															  responseBody.rankRewardReceivedRankNo,
															  responseBody.rewardedAttainmentEntryNo,
															  responseBody.distortionScrollDailyUseCount,
															  responseBody.remainingDistortionTime,
															  responseBody.ridingCartInst,
															  responseBody.dailyServerLevelRakingNo,
															  responseBody.dailyServerLevelRanking,
															  responseBody.rewardedDailyServerLevelRankingNo,
															  responseBody.previousContinentId,
															  responseBody.previousNationId,
															  responseBody.nationInsts,
															  responseBody.dailyNationDonationCount,
															  responseBody.serverOpenDate,
															  responseBody.explorationPoint,
															  responseBody.soulPowder,
															  responseBody.dailyStaminaBuyCount,
															  responseBody.lootingItemMinGrade,
															  responseBody.customPresetHair,
															  responseBody.customFaceJawHeight,
															  responseBody.customFaceJawWidth,
															  responseBody.customFaceJawEndHeight,
															  responseBody.customFaceWidth,
															  responseBody.customFaceEyebrowHeight,
															  responseBody.customFaceEyebrowRotation,
															  responseBody.customFaceEyesWidth,
															  responseBody.customFaceNoseHeight,
															  responseBody.customFaceNoseWidth,
															  responseBody.customFaceMouthHeight,
															  responseBody.customFaceMouthWidth,
															  responseBody.customBodyHeadSize,
															  responseBody.customBodyArmsLength,
															  responseBody.customBodyArmsWidth,
															  responseBody.customBodyChestSize,
															  responseBody.customBodyWaistWidth,
															  responseBody.customBodyHipsSize,
															  responseBody.customBodyPelvisWidth,
															  responseBody.customBodyLegsLength,
															  responseBody.customBodyLegsWidth,
															  responseBody.customColorSkin,
															  responseBody.customColorEyes,
															  responseBody.customColorBeardAndEyebrow,
															  responseBody.customColorHair,
															  responseBody.todayMissionTutorialStarted,
															  responseBody.serverMaxLevel,
															  responseBody.heroNpcShopProducts,
															  responseBody.rankActiveSkills,
															  responseBody.rankPassiveSkills,
															  responseBody.selectedRankActiveSkillId,
															  responseBody.rankActiveSkillRemainingCoolTime,
															  responseBody.rookieGiftNo,
															  responseBody.rookieGiftRemainingTime,
															  responseBody.receivedOpenGiftRewards,
															  responseBody.regDate,
															  responseBody.rewardedOpen7DayEventMissions,
															  responseBody.purchasedOpen7DayEventProducts,
															  responseBody.open7DayEventProgressCounts,
															  responseBody.retrievalProgressCounts,
															  responseBody.retrievals,
															  responseBody.taskConsignments,
															  responseBody.taskConsignmentStartCounts,
															  responseBody.rewardedLimitationGiftScheduleIds,
															  responseBody.weekendReward,
															  responseBody.paidWarehouseSlotCount,
															  responseBody.dailyDiaShopProductBuyCounts,
															  responseBody.totalDiaShopProductBuyCounts,
															  responseBody.weeklyFearAltarHalidomCollectionRewardNo,
															  responseBody.weeklyFearAltarHalidoms,
															  responseBody.weeklyRewardReceivedFearAltarHalidomElementals,
															  responseBody.open7DayEventRewarded,
															  responseBody.nationPowerRankings,
															  responseBody.potionAttrs);

			// 공유 이벤트
			CsSharingEventManager.Instance.RequestFirebaseDynamicLinkReward();
			CsSharingEventManager.Instance.RequestFirebaseDynamicLinkReceive();

			// 메인장비
			CsGameData.Instance.MyHeroInfo.AddHeroMainGears(responseBody.mainGears, true, false);
			CsGameData.Instance.MyHeroInfo.SetHeroMainGearEquipped(responseBody.equippedWeaponId);
			CsGameData.Instance.MyHeroInfo.SetHeroMainGearEquipped(responseBody.equippedArmorId);

			// 서브장비
			CsGameData.Instance.MyHeroInfo.AddHeroSubGears(responseBody.subGears, true, false);

			// 영웅스킬.
			CsGameData.Instance.MyHeroInfo.AddHeroSkills(responseBody.skills);

			// 메일
			CsGameData.Instance.MyHeroInfo.AddMails(responseBody.mails);

			// 보유한 탈것 목록
			CsGameData.Instance.MyHeroInfo.AddHeroMounts(responseBody.mounts, true);

			// 보유한 탈것장비 목록
			CsGameData.Instance.MyHeroInfo.AddHeroMountGears(responseBody.mountGears, true);

			// 장착한 탈것장비ID 목록
			CsGameData.Instance.MyHeroInfo.AddEquippedMountGears(responseBody.equippedMountGears, true);

			// 인벤토리
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.placedInventorySlots, true, false);

			// 창고
			CsGameData.Instance.MyHeroInfo.AddWarehouseSlots(responseBody.placedWarehouseSlots, true);

			// 메인퀘스트
			CsMainQuestManager.Instance.Init(responseBody.currentMainQuest);

			// 농장의 위협 퀘스트
			CsThreatOfFarmQuestManager.Instance.Init(responseBody.treatOfFarmQuest);

			// 현상금사냥꾼퀘스트
			CsBountyHunterQuestManager.Instance.Init(responseBody.bountyHunterQuest, responseBody.bountyHunterDailyStartCount, responseBody.date);

			// 낚시퀘스트
			CsFishingQuestManager.Instance.Init(responseBody.fishingQuest, responseBody.fishingQuestDailyStartCount, responseBody.date);

			// 의문의상자퀘스트
			CsMysteryBoxQuestManager.Instance.Init(responseBody.mysteryBoxQuest, responseBody.dailyMysteryBoxQuestStartCount, responseBody.date);

			// 밀서퀘스트
			CsSecretLetterQuestManager.Instance.Init(responseBody.secretLetterQuest, responseBody.dailySecretLetterQuestStartCount, responseBody.date);
			CsSecretLetterQuestManager.Instance.SecretLetterQuestTargetNationId = responseBody.secretLetterQuestTargetNationId;

			// 차원습격퀘스트
			CsDimensionRaidQuestManager.Instance.Init(responseBody.dimensionRaidQuest, responseBody.dailyDimensionRaidQuestStartCount, responseBody.date);

			// 위대한성전퀘스트
			CsHolyWarQuestManager.Instance.Init(responseBody.holyWarQuest, responseBody.dailyHolyWarQuestStartSchedules, responseBody.date);

			// 고대인의 유적
			CsGameData.Instance.AncientRelic.AncientRelicDailyPlayCount = responseBody.ancientRelicDailyPlayCount;
			CsGameData.Instance.AncientRelic.AncientRelicPlayCountDate = responseBody.date;

			// 검투대회
			CsGameData.Instance.FieldOfHonor.RewardedDailyFieldOfHonorRankingNo = responseBody.rewardedDailyFieldOfHonorRankingNo;
			CsGameData.Instance.FieldOfHonor.DailyFieldOfHonorRankingNo = responseBody.dailyFieldOfHonorRankingNo;
			CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking = responseBody.dailyfieldOfHonorRanking;
			CsGameData.Instance.FieldOfHonor.FieldOfHonorDailyPlayCount = responseBody.fieldOfHonorDailyPlayCount;

			// 탑승카트
			CsCartManager.Instance.Init();

			// 대륙상호작용오브젝트
			CsContinentObjectManager.Instance.Init();

			// 길드
			CsGuildManager.Instance.Init(responseBody.guild,
										 responseBody.guildSkills,
										 responseBody.guildMemberGrade,
										 responseBody.totalGuildContributionPoint,
										 responseBody.guildContributionPoint,
										 responseBody.guildPoint,
										 responseBody.guildRejoinRemainingTime,
										 responseBody.guildApplications,
										 responseBody.dailyGuildApplicationCount,
										 responseBody.date,
										 responseBody.dailyGuildDonationCount,
										 responseBody.dailyGuildFarmQuestStartCount,
										 responseBody.guildFarmQuest,
										 responseBody.dailyGuildFoodWarehouseStockCount,
										 responseBody.receivedGuildFoodWarehouseCollectionId,
										 responseBody.guildMoralPoint,
										 responseBody.guildAltarDefenseMissionRemainingCoolTime,
										 responseBody.guildAltarRewardReceivedDate,
										 responseBody.guildMissionQuest,
										 responseBody.guildDailyRewardReceivedDate,
										 responseBody.guildSupplySupportQuestPlay,
										 responseBody.guildHuntingQuest,
										 responseBody.dailyGuildHuntingQuestStartCount,
										 responseBody.guildHuntingDonationDate,
										 responseBody.guildHuntingDonationRewardReceivedDate,
										 responseBody.guildDailyObjectiveRewardReceivedNo,
										 responseBody.guildWeeklyObjectiveRewardReceivedDate,
										 responseBody.guildDailyObjectiveNoticeRemainingCoolTime);

			// 국가전
			CsNationWarManager.Instance.Init(responseBody.nationWarDeclarations,
											 responseBody.weeklyNationWarDeclarationCount,
											 responseBody.nationWarJoined,
											 responseBody.nationWarKillCount,
											 responseBody.nationWarAssistCount,
											 responseBody.nationWarDeadCount,
											 responseBody.nationWarImmediateRevivalCount,
											 responseBody.dailyNationWarFreeTransmissionCount,
											 responseBody.dailyNationWarPaidTransmissionCount,
											 responseBody.dailyNationWarCallCount,
											 responseBody.nationWarCallRemainingCoolTime,
											 responseBody.dailyNationWarConvergingAttackCount,
											 responseBody.nationWarConvergingAttackRemainingCoolTime,
											 responseBody.nationWarConvergingAttackTargetArrangeId,
											 responseBody.nationWarMonsterInsts);

			// 보급지원
			CsSupplySupportQuestManager.Instance.Init(responseBody.supplySupportQuest);
			CsSupplySupportQuestManager.Instance.DailySupplySupportQuestCount = responseBody.dailySupplySupportQuestStartCount;

			// 도감
			CsIllustratedBookManager.Instance.Init(responseBody.illustratedBookExplorationStepNo,
												   responseBody.illustratedBookExplorationStepRewardReceivedDate,
												   responseBody.illustratedBookExplorationStepRewardReceivedStepNo,
												   responseBody.activationIllustratedBookIds,
												   responseBody.completedSceneryQuests);

			// 업적


			// 칭호
			CsTitleManager.Instance.Init(responseBody.titles,
										 responseBody.displayTitleId,
										 responseBody.activationTitleId);

			// 크리처카드
			CsCreatureCardManager.Instance.Init(responseBody.creatureCards,
												responseBody.activatedCreatureCardCollections,
												responseBody.creatureCardCollectionFamePoint,
												responseBody.purchasedCreatureCardShopFixedProducts,
												responseBody.creatureCardShopRandomProducts,
												responseBody.dailyCreatureCardShopPaidRefreshCount,
												responseBody.date);

			// 정예
			CsEliteManager.Instance.Init(responseBody.heroEliteMonsterKills,
										 responseBody.spawnedEliteMonsters,
										 responseBody.dailyEliteDungeonPlayCount,
										 responseBody.date);

			//용맹의 증명
			CsGameData.Instance.ProofOfValor.DailyPlayCount = responseBody.dailyProofOfValorPlayCount;
			CsGameData.Instance.ProofOfValor.MyDailyFreeRefreshCount = responseBody.dailyProofOfValorFreeRefreshCount;
			CsGameData.Instance.ProofOfValor.MyDailyPaidRefreshCount = responseBody.dailyProofOfValorPaidRefreshCount;
			CsGameData.Instance.ProofOfValor.PaidRefreshCount = responseBody.poofOfValorPaidRefreshCount;
			CsGameData.Instance.ProofOfValor.BossMonsterArrangeId = responseBody.heroProofOfValorInstance.bossMonsterArrangeId;
			CsGameData.Instance.ProofOfValor.CreatureCardId = responseBody.heroProofOfValorInstance.creatureCardId;
			CsGameData.Instance.ProofOfValor.DailyPlayCountDate = responseBody.date;
			CsGameData.Instance.ProofOfValor.ProofOfValorCleared = responseBody.proofOfValorCleared;

			//영혼을 탐하는자
			CsGameData.Instance.SoulCoveter.SoulCoveterWeeklyPlayCount = responseBody.weeklySoulCoveterPlayCount;

			// 지혜의 신전
			CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount = responseBody.dailyWisdomTemplePlayCount;
			CsGameData.Instance.WisdomTemple.WisdomTempleCleared = responseBody.wisdomTempleCleared;

			// 유적 탈환
			CsGameData.Instance.RuinsReclaim.FreePlayCount = responseBody.dailyRuinsReclaimFreePlayCount;

			// 무한 대전
			CsGameData.Instance.InfiniteWar.DailyPlayCount = responseBody.dailyInfiniteWarPlayCount;

			// 공포의 제단
			CsGameData.Instance.FearAltar.DailyFearAltarPlayCount = responseBody.dailyFearAltarPlayCount;

            // 전쟁의 기억
            CsGameData.Instance.WarMemory.FreePlayCount = responseBody.dailyWarMemoryFreePlayCount;

            // 오시리스 방
            CsGameData.Instance.OsirisRoom.DailyPlayCount = responseBody.dailyOsirisRoomPlayCount;

            // 무역선 탈환
            CsGameData.Instance.TradeShip.PlayCount = responseBody.dailyTradeShipPlayCount;

            for (int i = 0; i < responseBody.myTradeShipBestRecords.Length; i++)
            {
                CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(responseBody.myTradeShipBestRecords[i]));
            }

            for (int i = 0; i < responseBody.serverTradeShipBestRecords.Length; i++)
            {
                CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(responseBody.serverTradeShipBestRecords[i]));
            }

            // 앙쿠의 무덤
            CsGameData.Instance.AnkouTomb.PlayCount = responseBody.dailyAnkouTombPlayCount;

            for (int i = 0; i < responseBody.myAnkouTombBestRecords.Length; i++)
            {
                CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(responseBody.myAnkouTombBestRecords[i]));
            }

            for (int i = 0; i < responseBody.serverAnkouTombBestRecords.Length; i++)
            {
                CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(responseBody.serverAnkouTombBestRecords[i]));
            }

			// 던전
			CsDungeonManager.Instance.Init();

			// 버프 디버프
			CsBuffDebuffManager.Instance.Init();

			// 데일리 퀘스트
			CsDailyQuestManager.Instance.Init(responseBody.dailyQuestAcceptionCount, responseBody.dailyQuestFreeRefreshCount, responseBody.dailyQuests, responseBody.date);

			// 위클리 퀘스트
			CsWeeklyQuestManager.Instance.Init(responseBody.weeklyQuest);

			// 진정한영웅 퀘스트
			CsTrueHeroQuestManager.Instance.Init(responseBody.trueHeroQuest);

			// 서브퀘스트
			CsSubQuestManager.Instance.Init(responseBody.subQuests);

			// 시련퀘스트
			CsOrdealQuestManager.Instance.Init(responseBody.ordealQuest);

			// 전기
			CsBiographyManager.Instance.Init(responseBody.biographies);

			// 친구 블랙리스트...
			CsFriendManager.Instance.Init(responseBody.friends, responseBody.tempFriends, responseBody.blacklistEntries, responseBody.deadRecords);

			// 행운상점
			CsLuckyShopManager.Instance.Init(responseBody.date, responseBody.itemLuckyShopFreePickRemainingTime, responseBody.itemLuckyShopFreePickCount, responseBody.itemLuckyShopPick1TimeCount, responseBody.itemLuckyShopPick5TimeCount,
											 responseBody.creatureCardLuckyShopFreePickRemainingTime, responseBody.creatureCardLuckyShopFreePickCount, responseBody.creatureCardLuckyShopPick1TimeCount, responseBody.creatureCardLuckyShopPick5TimeCount);

			// 축복 퀘스트 / 유망자 퀘스트
			CsBlessingQuestManager.Instance.Init(responseBody.ownerProspectQuests, responseBody.targetProspectQuests);

			// 크리쳐
			CsCreatureManager.Instance.Init(responseBody.creatures, responseBody.participatedCreatureId, responseBody.dailyCreatureVariationCount, responseBody.date);

			// 코스튬
			CsCostumeManager.Instance.Init(responseBody.costumes, 
										   responseBody.equippedCostumeId,
										   responseBody.costumeCollectionId,
										   responseBody.costumeCollectionActivated);

			// 선물
			CsPresentManager.Instance.Init(responseBody.weeklyPresentPopularityPoint, responseBody.weeklyPresentContributionPoint, responseBody.nationWeeklyPresentPopularityPointRankingNo, responseBody.nationWeeklyPresentPopularityPointRanking, responseBody.rewardedNationWeeklyPresentPopularityPointRankingNo,
										   responseBody.nationWeeklyPresentContributionPointRankingNo, responseBody.nationWeeklyPresentContributionPointRanking, responseBody.rewardedNationWeeklyPresentContributionPointRankingNo, responseBody.date);

			// 크리처농장퀘스트
			CsCreatureFarmQuestManager.Instance.Init(responseBody.dailyCreatureFarmQuestAcceptionCount, responseBody.creatureFarmQuest, responseBody.date);

			// 국가동맹
			CsNationAllianceManager.Instance.Init(responseBody.nationAlliances, responseBody.nationAllianceApplications);

			CsOpenToastManager.Instance.Init();

			// 전직퀘스트
			CsJobChangeManager.Instance.Init(responseBody.jobChangeQuest);

			// 캐쉬
			CsCashManager.Instance.Init(responseBody.cashProductPurchaseCounts,
										responseBody.firstChargeEventObjectiveCompleted,
										responseBody.firstChargeEventRewarded,
										responseBody.rechargeEventAccUnOwnDia,
										responseBody.rechargeEventRewarded,
										responseBody.chargeEvent,
										responseBody.dailyChargeEventAccUnOwnDia,
										responseBody.rewardedDailyChargeEventMissions,
										responseBody.consumeEvent,
										responseBody.dailyConsumeEventAccDia,
										responseBody.rewardedDailyConsumeEventMissions,
										responseBody.date);

			// 별자리
			CsConstellationManager.Instance.Init(responseBody.constellations,
												 responseBody.starEssense,
												 responseBody.dailyStarEssenseItemUseCount,
												 responseBody.date);

			// 아티팩트
			CsArtifactManager.Instance.Init(responseBody.artifactNo,
											responseBody.artifactLevel,
											responseBody.artifactExp,
											responseBody.equippedArtifactNo);

			// 전투력 업데이트.
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower(true);

			CsGameData.Instance.MyHeroInfo.IsCreateEnter = false;

			CsGameEventUIToUI.Instance.OnEventHeroLogin();
		}
		else if (nReturnCode == 101)
		{
			Debug.Log("ERROR (OnEventResHeroLogin) : 101 - 해당 영웅이 존재하지 않습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else if (nReturnCode == 102)
		{
			Debug.Log("ERROR (OnEventResHeroLogin) : 102 - 영웅 생성이 완료되지 않았습니다.");
			//CsGameEventUIToUI.Instance.OnEventAlert("");
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Reconnect

	#region ErrorLogging

	public void SendErrorLogging(string strMessage)
	{
		if (CsConfiguration.Instance.SystemSetting.LoggingEnabled)
		{ 
			if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
			{
				LoggingNACommand cmd = new LoggingNACommand();
				cmd.Content = strMessage;
				cmd.Finished += ResponseErrorLogging;
				cmd.Run();
			}
		}
	}

	void ResponseErrorLogging(object sender, EventArgs e)
	{
		LoggingNACommand cmd = (LoggingNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		LoggingNAResponse res = (LoggingNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseErrorLogging OK");
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	#endregion ErrorLoggin

	#region FirebaseLogEvent

	public void SendLogEvent(string strName, string strValue)
	{
		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			LogEventNACommand cmd = new LogEventNACommand();
			cmd.Name = strName;
			cmd.Value = strValue;
			cmd.Finished += ResponseLogEvent;
			cmd.Run();
		}
	}

	void ResponseLogEvent(object sender, EventArgs e)
	{
		LogEventNACommand cmd = (LogEventNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		LogEventNAResponse res = (LogEventNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseLogEvent OK");
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	#endregion FirebaseLogEvent 

	#region NpcShop

	//---------------------------------------------------------------------------------------------------
	// NPC상점상품구입
	public void SendNpcShopProductBuy(int nProductId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			NpcShopProductBuyCommandBody cmdBody = new NpcShopProductBuyCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			CsRplzSession.Instance.Send(ClientCommandName.NpcShopProductBuy, cmdBody);
		}
	}

	void OnEventResNpcShopProductBuy(int nReturnCode, NpcShopProductBuyResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddNpcShopProductBuyCount(m_nProductId);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			
			CsGameEventUIToUI.Instance.OnEventNpcShopProductBuy();
		}
		else if (nReturnCode == 101)
		{
			// 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A96_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A96_TXT_00004"));
		}
		else if (nReturnCode == 103)
		{
			// 구매수량이 제한수량을 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A96_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion NpcShop

	#region Open7DayEvent

	//---------------------------------------------------------------------------------------------------
	// 오픈7일이벤트미션보상받기
	public void SendOpen7DayEventMissionRewardReceive(int nMissionId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			Open7DayEventMissionRewardReceiveCommandBody cmdBody = new Open7DayEventMissionRewardReceiveCommandBody();
			cmdBody.missionId = m_nMissionId = nMissionId;
			CsRplzSession.Instance.Send(ClientCommandName.Open7DayEventMissionRewardReceive, cmdBody);
		}
	}

	void OnEventResOpen7DayEventMissionRewardReceive(int nReturnCode, Open7DayEventMissionRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.RewardedOpen7DayEventMissionList.Add(m_nMissionId);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventOpen7DayEventMissionRewardReceive();

			CsOpen7DayEventMission csOpen7DayEventMission = CsGameData.Instance.GetOpen7DayEventMission(m_nMissionId);

			if (csOpen7DayEventMission != null)
			{
				List<CsDropObject> listLooted = new List<CsDropObject>();
				List<CsDropObject> listNotLooted = new List<CsDropObject>();

				foreach (CsOpen7DayEventMissionReward csOpen7DayEventMissionReward in csOpen7DayEventMission.Open7DayEventMissionRewardList)
				{
					listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, csOpen7DayEventMissionReward.ItemReward.Item.ItemId, csOpen7DayEventMissionReward.ItemReward.ItemOwned, csOpen7DayEventMissionReward.ItemReward.ItemCount));
				}

				CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
			}
		}
		else if (nReturnCode == 101)
		{
			//  이미 보상받은 미션입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			//  아직 오픈되지 않은 이벤트입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 받을 수 있는 일차가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 미션이 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00105"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 오픈7일이벤트상품구매
	public void SendOpen7DayEventProductBuy(int nProductId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			Open7DayEventProductBuyCommandBody cmdBody = new Open7DayEventProductBuyCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			CsRplzSession.Instance.Send(ClientCommandName.Open7DayEventProductBuy, cmdBody);
		}
	}

	void OnEventResOpen7DayEventProductBuy(int nReturnCode, Open7DayEventProductBuyResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PurchasedOpen7DayEventProductList.Add(m_nProductId);
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventOpen7DayEventProductBuy();
		}
		else if (nReturnCode == 101)
		{
            // 이미 구매한 상품입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
            // 아직 오픈되지 않은 이벤트입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
            // 이벤트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
            // 구매할 수 있는 일차가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00204"));
		}
		else if (nReturnCode == 105)
		{
            // 비귀속다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00205"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 오픈7일이벤트보상받기
	public void SendOpen7DayEventRewardReceive()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			Open7DayEventRewardReceiveCommandBody cmdBody = new Open7DayEventRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.Open7DayEventRewardReceive, cmdBody);
		}
	}

	void OnEventResOpen7DayEventRewardReceive(int nReturnCode, Open7DayEventRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);
			CsGameData.Instance.MyHeroInfo.Open7DayEventRewarded = true;

			CsGameEventUIToUI.Instance.OnEventOpen7DayEventRewardReceive();
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A97_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 오픈7일이벤트진행카운트갱신
	void OnEventEvtOpen7DayEventProgressCountUpdated(SEBOpen7DayEventProgressCountUpdatedEventBody eventBody)
	{
		CsHeroOpen7DayEventProgressCount csHeroOpen7DayEventProgressCount = CsGameData.Instance.MyHeroInfo.GetHeroOpen7DayEventProgressCount(eventBody.progressCount.type);

		if (csHeroOpen7DayEventProgressCount == null)
		{
			CsGameData.Instance.MyHeroInfo.HeroOpen7DayEventProgressCountList.Add(new CsHeroOpen7DayEventProgressCount(eventBody.progressCount));
		}
		else
		{
			csHeroOpen7DayEventProgressCount.AccProgressCount = eventBody.progressCount.accProgressCount;
		}

		CsGameEventUIToUI.Instance.OnEventOpen7DayEventProgressCountUpdated();
	}

	#endregion Open7DayEvent

	#region Retrieval

	//---------------------------------------------------------------------------------------------------
	// 골드회수
	public void SendRetrieveGold(int nRetrievalId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RetrieveGoldCommandBody cmdBody = new RetrieveGoldCommandBody();
			cmdBody.retrievalId = m_nRetrievalId = nRetrievalId;
			CsRplzSession.Instance.Send(ClientCommandName.RetrieveGold, cmdBody);
		}
	}

	void OnEventResRetrieveGold(int nReturnCode, RetrieveGoldResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.UpdateHeroRetrieval(m_nRetrievalId, responseBody.retrievalCount);

			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventRetrieveGold(bLevelUp, responseBody.acquiredExp);
		}
		else if (nReturnCode == 101)
		{
			// 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 개방되지 않은 회수입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 회수 횟수를 모두 사용했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 골드모두회수
	public void SendRetrieveGoldAll()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RetrieveGoldAllCommandBody cmdBody = new RetrieveGoldAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RetrieveGoldAll, cmdBody);
		}
	}

	void OnEventResRetrieveGoldAll(int nReturnCode, RetrieveGoldAllResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			for (int i = 0; i < responseBody.retrievals.Length; i++)
			{ 
				CsGameData.Instance.MyHeroInfo.UpdateHeroRetrieval(responseBody.retrievals[i].retrievalId, responseBody.retrievals[i].count);
			}

			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventRetrieveGoldAll(bLevelUp, responseBody.acquiredExp);
		}
		else if (nReturnCode == 101)
		{
			// 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 다이아회수
	public void SendRetrieveDia(int nRetrievalId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RetrieveDiaCommandBody cmdBody = new RetrieveDiaCommandBody();
			cmdBody.retrievalId = m_nRetrievalId = nRetrievalId;
			CsRplzSession.Instance.Send(ClientCommandName.RetrieveDia, cmdBody);
		}
	}

	void OnEventResRetrieveDia(int nReturnCode, RetrieveDiaResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.UpdateHeroRetrieval(m_nRetrievalId, responseBody.retrievalCount);

			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventRetrieveDia(bLevelUp, responseBody.acquiredExp);
		}
		else if (nReturnCode == 101)
		{
			//  다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 개방되지 않은 회수입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 회수 횟수를 모두 사용했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 다이아모두회수
	public void SendRetrieveDiaAll()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			RetrieveDiaAllCommandBody cmdBody = new RetrieveDiaAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RetrieveDiaAll, cmdBody);
		}
	}

	void OnEventResRetrieveDiaAll(int nReturnCode, RetrieveDiaAllResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			for (int i = 0; i < responseBody.retrievals.Length; i++)
			{
				CsGameData.Instance.MyHeroInfo.UpdateHeroRetrieval(responseBody.retrievals[i].retrievalId, responseBody.retrievals[i].count);
			}

			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventRetrieveDiaAll(bLevelUp, responseBody.acquiredExp);
		}
		else if (nReturnCode == 101)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A107_ERROR_00401"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 회수진행카운트갱신
	void OnEventEvtRetrievalProgressCountUpdated(SEBRetrievalProgressCountUpdatedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.UpdateHeroRetrievalProgressCount(eventBody.progressCount);

		CsGameEventUIToUI.Instance.OnEventRetrievalProgressCountUpdated();
	}

	#endregion Retrieval

	#region TaskConsignment
	//---------------------------------------------------------------------------------------------------
	// 골드회수
	public void SendTaskConsignmentStart(int nConsignmentId, int nUseExpItemId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			TaskConsignmentStartCommandBody cmdBody = new TaskConsignmentStartCommandBody();
			cmdBody.consignmentId = nConsignmentId;
			cmdBody.useExpItemId = nUseExpItemId;
			CsRplzSession.Instance.Send(ClientCommandName.TaskConsignmentStart, cmdBody);
		}
	}

	void OnEventResTaskConsignmentStart(int nReturnCode, TaskConsignmentStartResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AchievementDailyPoint = responseBody.achievementDailyPoint;
			CsGameData.Instance.MyHeroInfo.UpdateHeroTaskConsignment(responseBody.taskConsignment);
			CsGameData.Instance.MyHeroInfo.UpdateHeroTaskConsignmentStartCount(responseBody.startCount);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventTaskConsignmentStart();
		}
		else if (nReturnCode == 101)
		{
			// 길드멤버가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// VIP레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 시작한 위탁입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 이미 시작한 할일입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 시작횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 경험치아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 재료아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A109_ERROR_00107"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 할일위탁완료
	public void SendTaskConsignmentComplete(Guid guidInstanceId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			TaskConsignmentCompleteCommandBody cmdBody = new TaskConsignmentCompleteCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.TaskConsignmentComplete, cmdBody);
		}
	}

	void OnEventResTaskConsignmentComplete(int nReturnCode, TaskConsignmentCompleteResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.RemoveHeroTaskConsignment(m_guidInstanceId);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (CsGuildManager.Instance.Guild != null)
			{
				CsGuildManager.Instance.GuildContributionPoint = responseBody.guildContributionPoint;
				CsGuildManager.Instance.TotalGuildContributionPoint = responseBody.totalGuildContributionPoint;
				CsGuildManager.Instance.Guild.Fund = responseBody.giFund;
				CsGuildManager.Instance.Guild.BuildingPoint = responseBody.giBuildingPoint;
			}

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventTaskConsignmentComplete(bLevelUp, responseBody.acquiredExp);
		}
		else if (nReturnCode == 101)
		{
			// 완료시간이 경과되지 않았습니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else if (nReturnCode == 102)
		{
			// 인벤토리가 부족합니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 할일위탁완료
	public void SendTaskConsignmentImmediatelyComplete(Guid guidInstanceId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			TaskConsignmentImmediatelyCompleteCommandBody cmdBody = new TaskConsignmentImmediatelyCompleteCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.TaskConsignmentImmediatelyComplete, cmdBody);
		}
	}

	void OnEventResTaskConsignmentImmediatelyComplete(int nReturnCode, TaskConsignmentImmediatelyCompleteResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;

			CsGameData.Instance.MyHeroInfo.Level = responseBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = responseBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = responseBody.hp;

			CsGameData.Instance.MyHeroInfo.RemoveHeroTaskConsignment(m_guidInstanceId);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (CsGuildManager.Instance.Guild != null)
			{
				CsGuildManager.Instance.GuildContributionPoint = responseBody.guildContributionPoint;
				CsGuildManager.Instance.TotalGuildContributionPoint = responseBody.totalGuildContributionPoint;
				CsGuildManager.Instance.Guild.Fund = responseBody.giFund;
				CsGuildManager.Instance.Guild.BuildingPoint = responseBody.giBuildingPoint;
			}

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			CsGameEventUIToUI.Instance.OnEventTaskConsignmentImmediatelyComplete(bLevelUp, responseBody.acquiredExp);
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion TaskConsignment

	#region LimitationGift

	//----------------------------------------------------------------------------------------------------
	// 한정선물보상받기
	public void SendLimitationGiftRewardReceive(int nScheduleId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			LimitationGiftRewardReceiveCommandBody cmdBody = new LimitationGiftRewardReceiveCommandBody();
			cmdBody.scheduleId = m_nScheduleId = nScheduleId;
			CsRplzSession.Instance.Send(ClientCommandName.LimitationGiftRewardReceive, cmdBody);
		}
	}

	void OnEventResLimitationGiftRewardReceive(int nReturnCode, LimitationGiftRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.RewardedLimitationGiftScheduleIdList.Add(m_nScheduleId);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventLimitationGiftRewardReceive();
		}
		else if (nReturnCode == 101)
		{
			// 보상을 받을 수 없는 요일입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 받을 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A102_TXT_00012"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion LimitationGift

	#region WeekendReward

	//----------------------------------------------------------------------------------------------------
	// 주말보상선택
	public void SendWeekendRewardSelect(int nSelectionNo)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WeekendRewardSelectCommandBody cmdBody = new WeekendRewardSelectCommandBody();
			cmdBody.selectionNo = m_nSelectionNo = nSelectionNo;
			CsRplzSession.Instance.Send(ClientCommandName.WeekendRewardSelect, cmdBody);
		}
	}

	void OnEventResWeekendRewardSelect(int nReturnCode, WeekendRewardSelectResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			switch (m_nSelectionNo)
			{
				case 1:
					CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 = responseBody.selectedNumber;
					break;

				case 2:
					CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 = responseBody.selectedNumber;
					break;

				case 3:
					CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 = responseBody.selectedNumber;
					break;
			}
			
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventWeekendRewardSelect();
		}
		else if (nReturnCode == 101)
		{
			// 영웅 레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 선택할 수 없는 선택번호입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 선택했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 주말보상받기
	public void SendWeekendRewardReceive()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WeekendRewardReceiveCommandBody cmdBody = new WeekendRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.WeekendRewardReceive, cmdBody);
		}
	}

	void OnEventResWeekendRewardReceive(int nReturnCode, WeekendRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Rewarded = true;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventWeekendRewardReceive();
		}
		else if (nReturnCode == 101)
		{
			// 보상을 받을 수 없는 요일입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A102_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion WeekendReward

	#region Warehouse

	//----------------------------------------------------------------------------------------------------
	// 창고입고
	public void SendWarehouseDeposit(int[] anInventorySlotIndices)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WarehouseDepositCommandBody cmdBody = new WarehouseDepositCommandBody();
			cmdBody.inventorySlotIndices = anInventorySlotIndices;
			CsRplzSession.Instance.Send(ClientCommandName.WarehouseDeposit, cmdBody);
		}
	}

	void OnEventResWarehouseDeposit(int nReturnCode, WarehouseDepositResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots, false, true, false);
			CsGameData.Instance.MyHeroInfo.AddWarehouseSlots(responseBody.changedWarehouseSlots);

			CsGameEventUIToUI.Instance.OnEventWarehouseDeposit();
		}
		else if (nReturnCode == 101)
		{
			// VIP레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A113_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 창고출고
	public void SendWarehouseWithdraw(int nWarehouseSlotIndex)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WarehouseWithdrawCommandBody cmdBody = new WarehouseWithdrawCommandBody();
			cmdBody.warehouseSlotIndex = nWarehouseSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.WarehouseWithdraw, cmdBody);
		}
	}

	void OnEventResWarehouseWithdraw(int nReturnCode, WarehouseWithdrawResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.cangedInvetorySlots, false, true, false);
			PDWarehouseSlot[] chagnedWarehouseSlots = new PDWarehouseSlot[] { responseBody.changedWarehouseSlot };
			CsGameData.Instance.MyHeroInfo.AddWarehouseSlots(chagnedWarehouseSlots);

			CsGameEventUIToUI.Instance.OnEventWarehouseWithdraw();
		}
		else if (nReturnCode == 101)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A113_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//----------------------------------------------------------------------------------------------------
	// 창고슬롯확장
	public void SendWarehouseSlotExtend(int nExtendSlotCount)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			WarehouseSlotExtendCommandBody cmdBody = new WarehouseSlotExtendCommandBody();
			cmdBody.extendSlotCount = nExtendSlotCount;
			CsRplzSession.Instance.Send(ClientCommandName.WarehouseSlotExtend, cmdBody);
		}
	}

	void OnEventResWarehouseSlotExtend(int nReturnCode, WarehouseSlotExtendResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.PaidWarehouseSlotCount = responseBody.paidWarehouseSlotCount;
			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;

			CsGameEventUIToUI.Instance.OnEventWarehouseSlotExtend();
		}
		else if (nReturnCode == 101)
		{
			// 유효확장할 수 있는 최대슬롯수를 초과합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A113_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A113_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Warehouse

	#region DiaShop

	//----------------------------------------------------------------------------------------------------
	// 다이아상점상품구매
	public void SendDiaShopProductBuy(int nProductId, int nCount)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			DiaShopProductBuyCommandBody cmdBody = new DiaShopProductBuyCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			cmdBody.count = nCount;
			CsRplzSession.Instance.Send(ClientCommandName.DiaShopProductBuy, cmdBody);
		}
	}

	void OnEventResDiaShopProductBuy(int nReturnCode, DiaShopProductBuyResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.UpdateHeroDiaShopProductBuyCount(m_nProductId, responseBody.dailyBuyCount);
			CsGameData.Instance.MyHeroInfo.UpdateTotalHeroDiaShopProductBuyCount(m_nProductId, responseBody.totalBuyCount);

			CsGameData.Instance.MyHeroInfo.OwnDia = responseBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = responseBody.unOwnDia;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameEventUIToUI.Instance.OnEventDiaShopProductBuy();
		}
		else if (nReturnCode == 101)
		{
			// VIP레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 구매가 불가능한 상품입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 시간이 유효하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 요일이 유효하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 금일 구매횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00107"));
		}
		else if (nReturnCode == 108)
		{
			// 비귀속다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00108"));
		}
		else if (nReturnCode == 109)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A117_ERROR_00109"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion DiaShop

	#region FearAltarHalidomCollection

	//---------------------------------------------------------------------------------------------------
	// 공포의제단 원소보상
	public void SendFearAltarHalidomElementalRewardReceive(int nElementalId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			m_nFearAltarElementalId = nElementalId;

			FearAltarHalidomElementalRewardReceiveCommandBody cmdBody = new FearAltarHalidomElementalRewardReceiveCommandBody();
			cmdBody.elementalId = nElementalId;
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarHalidomElementalRewardReceive, cmdBody);
		}
	}

	void OnEventResFearAltarHalidomElementalRewardReceive(int nReturnCode, FearAltarHalidomElementalRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date <= responseBody.weekStartDate.Date.AddDays(6))
			{
				//responseBody.booty;
				CsGameData.Instance.MyHeroInfo.WeeklyRewardReceivedFearAltarHalidomElementalList.Add(m_nFearAltarElementalId);
				CsGameEventUIToUI.Instance.OnEventFearAltarHalidomElementalRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 해당 성물레벨을 모두 획득하지 않았습니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 102 : 인벤토리슬롯이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00702"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 공포의제단 수집보상
	public void SendFearAltarHalidomCollectionRewardReceive(int nRewardNo)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			m_nFearAltarHalidomCollectionRewardNo = nRewardNo;

			FearAltarHalidomCollectionRewardReceiveCommandBody cmdBody = new FearAltarHalidomCollectionRewardReceiveCommandBody();
			cmdBody.rewardNo = nRewardNo;
			CsRplzSession.Instance.Send(ClientCommandName.FearAltarHalidomCollectionRewardReceive, cmdBody);
		}
	}

	void OnEventResFearAltarHalidomCollectionRewardReceive(int nReturnCode, FearAltarHalidomCollectionRewardReceiveResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date <= responseBody.weekStartDate.Date.AddDays(6))
			{
				CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomCollectionRewardNo = m_nFearAltarHalidomCollectionRewardNo;

				//responseBody.booty;
				CsGameEventUIToUI.Instance.OnEventFearAltarHalidomCollectionRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 101 : 성물갯수가 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00801"));
		}
		else if (nReturnCode == 102)
		{
			// 102 : 인벤토리슬롯이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A116_ERROR_00802"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 공포의제단 성물획득
	void OnEventEvtFearAltarHalidomAcquisition(SEBFearAltarHalidomAcquisitionEventBody eventBody)
	{
		if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date <= eventBody.weekDateTime.Date.AddDays(6))
		{
			CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomList.Add(eventBody.halidomId);
			CsGameEventUIToUI.Instance.OnEventFearAltarHalidomAcquisition(eventBody.halidomId);
		}
	}

    #endregion FearAltarHalidomCollection

    #region PortalEnter
    //---------------------------------------------------------------------------------------------------
    public void SendPortalEnter(int nPortalId)
    {
        if (!m_bProcessing)
        {
			m_bProcessing = true;

			PortalEnterCommandBody cmdBody = new PortalEnterCommandBody();
			cmdBody.portalId = nPortalId;
			CsRplzSession.Instance.Send(ClientCommandName.PortalEnter, cmdBody);
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResPortalEnter(int nReturnCode, PortalEnterResponseBody responseBody)
    {
        m_bProcessing = false;

        if (nReturnCode == 0)
        {
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.Portal;
			CsGameEventUIToUI.Instance.OnEventPortalEnter();
        }
        else if (nReturnCode == 101)
        {

        }
        else if (nReturnCode == 102)
        {

        }
        else if (nReturnCode == 103)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_NOPORTAL"));
        }
    }
    #endregion PortalEnter

    //---------------------------------------------------------------------------------------------------
    public void SendHeroLogOut()
    {
        if (!m_bProcessing)
        {
            HeroLogoutCommandBody csEvt = new HeroLogoutCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.HeroLogout, csEvt);
        }
    }

    // 계정중복로그인
    //---------------------------------------------------------------------------------------------------
    public void OnEventEvtAccountLoginDuplicated(SEBAccountLoginDuplicatedEventBody eventBody)
    {
        CsGameEventUIToUI.Instance.OnEventAccountLoginDuplicated();
    }

    #region SafeMode

    //---------------------------------------------------------------------------------------------------
    public void SendAutoHuntStart()
    {
        CEBAutoHuntStartEventBody csEvt = new CEBAutoHuntStartEventBody();
        CsRplzSession.Instance.Send(ClientEventName.AutoHuntStart, csEvt);
    }

    //---------------------------------------------------------------------------------------------------
    public void SendAutoHuntEnd()
    {
        CEBAutoHuntEndEventBody csEvt = new CEBAutoHuntEndEventBody();
        CsRplzSession.Instance.Send(ClientEventName.AutoHuntEnd, csEvt);
    }

	#endregion SafeMode

	#region PotionAttr

	int m_nPotionAttrId;

	//---------------------------------------------------------------------------------------------------
	// 영웅속성물약사용
	public void SendHeroAttrPotionUse(int nPotionAttrId)
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			HeroAttrPotionUseCommandBody cmdBody = new HeroAttrPotionUseCommandBody();
			cmdBody.potionAttrId = m_nPotionAttrId = nPotionAttrId;
			CsRplzSession.Instance.Send(ClientCommandName.HeroAttrPotionUse, cmdBody);
		}
	}

	void OnEventResHeroAttrPotionUse(int nReturnCode, HeroAttrPotionUseResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			CsGameData.Instance.MyHeroInfo.AddHeroPotionAttr(m_nPotionAttrId, responseBody.potionAttrCount);
			PDInventorySlot[] slots = new PDInventorySlot[] { responseBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventHeroAttrPotionUse();
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A155_ERROR_00101"));
		}
		else if (nReturnCode == 101)
		{
			// 사용횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A155_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅속성물약전체사용
	public void SendHeroAttrPotionUseAll()
	{
		if (!m_bProcessing)
		{
			m_bProcessing = true;

			HeroAttrPotionUseAllCommandBody cmdBody = new HeroAttrPotionUseAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.HeroAttrPotionUseAll, cmdBody);
		}
	}

	void OnEventResHeroAttrPotionUseAll(int nReturnCode, HeroAttrPotionUseAllResponseBody responseBody)
	{
		m_bProcessing = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = responseBody.maxHP;
			for (int i = 0; i < responseBody.changedPotionAttrs.Length; i++)
			{
				CsGameData.Instance.MyHeroInfo.AddHeroPotionAttr(responseBody.changedPotionAttrs[i].potionAttrId, responseBody.changedPotionAttrs[i].count);
			}
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			CsGameEventUIToUI.Instance.OnEventHeroAttrPotionUseAll();
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion PotionAttr

	#region Sytem Message

	void OnEventEvtSystemMessage(SEBSystemMessageEventBody eventBody)
	{
		CsSystemMessage csSystemMessage = null;

		switch ((EnSystemMessage)eventBody.systemMessage.id)
		{
			case EnSystemMessage.MainGearAcquirement:
				csSystemMessage = new CsSystemMessageMainGearAcquirement((PDMainGearAcquirementSystemMessage)eventBody.systemMessage);
				break;
			case EnSystemMessage.MainGearEnchantment:
				csSystemMessage = new CsSystemMessageMainGearEnchantment((PDMainGearEnchantmentSystemMessage)eventBody.systemMessage);
				break;
			case EnSystemMessage.CreatureCardAcquirement:
				csSystemMessage = new CsSystemMessageCreatureCardAcquirement((PDCreatureCardAcquirementSystemMessage)eventBody.systemMessage);
				break;
			case EnSystemMessage.CreatureAcquirement:
				csSystemMessage = new CsSystemMessageCreatureAcquirement((PDCreatureAcquirementSystemMessage)eventBody.systemMessage);
				break;
			case EnSystemMessage.CreatureInjection:
				csSystemMessage = new CsSystemMessageCreatureInjection((PDCreatureInjectionSystemMessage)eventBody.systemMessage);
				break;
			case EnSystemMessage.CostumeEnchantment:
				csSystemMessage = new CsSystemMessageCostumeEnchantment((PDCostumeEnchantmentSystemMessage)eventBody.systemMessage);
				break;
		}

		CsGameEventUIToUI.Instance.OnEventSystemMessage(csSystemMessage);
	}

	#endregion System Message
}