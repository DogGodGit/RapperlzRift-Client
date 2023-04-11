using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class CsGameData
{
	public static CsGameData Instance
	{
		get { return CsSingleton<CsGameData>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	Transform m_trMyHero;
	List<IHeroObjectInfo> m_listHeroObjectInfo = new List<IHeroObjectInfo>();
	List<IMonsterObjectInfo> m_listMonsterObjectInfo = new List<IMonsterObjectInfo>();
	List<INpcObjectInfo> m_listNpcObjectInfo = new List<INpcObjectInfo>();
	List<ICartObjectInfo> m_listCartObjectInfo = new List<ICartObjectInfo>();

	Vector3[] m_avtPathCornerByAutoMove;
	int m_nAutoMoveLocationId;
	int m_nAutoMoveNationId;
	Vector3 m_vtAutoMoveObjectPos;
	bool m_bJoystickDragging = false;
	bool m_bJoystickDown = false;
	bool m_bJoysticWalk = false;
	float m_flJoystickAngle;

	CsMyHeroInfo m_csMyHeroInfo;

	// Game Meta Data Variables
	List<CsElemental> m_csElemental = new List<CsElemental>();
	List<CsJob> m_listCsJob = new List<CsJob>();
	List<CsNation> m_listCsNation = new List<CsNation>();
	List<CsAttrCategory> m_listCsAttrCategory = new List<CsAttrCategory>();
	List<CsAttr> m_listCsAttr = new List<CsAttr>();
	Dictionary<long, CsAttrValueInfo> m_dicCsAttrValueInfo = new Dictionary<long, CsAttrValueInfo>();
	//List<CsAttrValueInfo> m_listCsAttrValueInfo = new List<CsAttrValueInfo>();
	List<CsItemMainCategory> m_listCsItemMainCategory = new List<CsItemMainCategory>();
	List<CsItemType> m_listCsItemType = new List<CsItemType>();
	List<CsItemGrade> m_listCsItemGrade = new List<CsItemGrade>();
	List<CsItem> m_listCsItem = new List<CsItem>();

	//List<CsExpReward> m_listCsExpReward = new List<CsExpReward>();
	//List<CsGoldReward> m_listCsGoldReward = new List<CsGoldReward>();
	//List<CsItemReward> m_listCsItemReward = new List<CsItemReward>();

	Dictionary<long, CsExpReward> m_dicCsExpReward = new Dictionary<long, CsExpReward>();
	Dictionary<long, CsGoldReward> m_dicCsGoldReward = new Dictionary<long, CsGoldReward>();
	Dictionary<long, CsItemReward> m_dicCsItemReward = new Dictionary<long, CsItemReward>();
	Dictionary<long, CsExploitPointReward> m_dicCsExploitPointReward = new Dictionary<long, CsExploitPointReward>();
	Dictionary<long, CsHonorPointReward> m_dicCsHonorPointReward = new Dictionary<long, CsHonorPointReward>();

	List<CsMainGearCategory> m_listCsMainGearCategory = new List<CsMainGearCategory>();
	List<CsMainGearType> m_listCsMainGearType = new List<CsMainGearType>();
	List<CsMainGearTier> m_listCsMainGearTier = new List<CsMainGearTier>();
	List<CsMainGearGrade> m_listCsMainGearGrade = new List<CsMainGearGrade>();
	List<CsMainGearQuality> m_listCsMainGearQuality = new List<CsMainGearQuality>();
	List<CsMainGear> m_listCsMainGear = new List<CsMainGear>();
	List<CsMainGearEnchantStep> m_listCsMainGearEnchantStep = new List<CsMainGearEnchantStep>();
	List<CsMainGearEnchantLevel> m_listCsMainGearEnchantLevel = new List<CsMainGearEnchantLevel>();
	List<CsMainGearOptionAttrGrade> m_listCsMainGearOptionAttrGrade = new List<CsMainGearOptionAttrGrade>();
	List<CsMainGearRefinementRecipe> m_listCsMainGearRefinementRecipe = new List<CsMainGearRefinementRecipe>();
	List<CsSubGearGrade> m_listCsSubGearGrade = new List<CsSubGearGrade>();
	List<CsSubGear> m_listCsSubGear = new List<CsSubGear>();
	List<CsLocation> m_listCsLocation = new List<CsLocation>();
	List<CsMonsterCharacter> m_listCsMonsterCharacter = new List<CsMonsterCharacter>();
	List<CsMonsterInfo> m_listCsMonsterInfo = new List<CsMonsterInfo>();
	List<CsContinent> m_listCsContinent = new List<CsContinent>();
	List<CsMainMenu> m_listCsMainMenu = new List<CsMainMenu>();
	List<CsJobSkillMaster> m_listCsJobSkillMaster = new List<CsJobSkillMaster>();
	List<CsJobSkill> m_listCsJobSkill = new List<CsJobSkill>();
	List<CsPortal> m_listCsPortal = new List<CsPortal>();
	List<CsNpcInfo> m_listCsNpcInfo = new List<CsNpcInfo>();
	List<CsContinentObject> m_listCsContinentObject = new List<CsContinentObject>();
	List<CsMainQuest> m_listCsMainQuest = new List<CsMainQuest>();
	List<CsPaidImmediateRevival> m_listCsPaidImmediateRevival = new List<CsPaidImmediateRevival>();
	List<CsMonsterSkill> m_listCsMonsterSkill = new List<CsMonsterSkill>();
	List<CsAbnormalState> m_listCsAbnormalState = new List<CsAbnormalState>();
	List<CsJobLevelMaster> m_listCsJobLevelMaster = new List<CsJobLevelMaster>();
	Dictionary<long, CsMonsterArrange> m_dicCsMonsterArrange = new Dictionary<long, CsMonsterArrange>();
	List<CsSimpleShopProduct> m_listCsSimpleShopProduct = new List<CsSimpleShopProduct>();
	List<CsVipLevel> m_listCsVipLevel = new List<CsVipLevel>();
	List<CsInventorySlotExtendRecipe> m_listCsInventorySlotExtendRecipe = new List<CsInventorySlotExtendRecipe>();
	List<CsMainGearDisassembleAvailableResultEntry> m_listCsMainGearDisassembleAvailableResultEntry = new List<CsMainGearDisassembleAvailableResultEntry>();
	List<CsItemCompositionRecipe> m_listCsItemCompositionRecipe = new List<CsItemCompositionRecipe>();
	List<CsRestRewardTime> m_listCsRestRewardTime = new List<CsRestRewardTime>();
	List<CsChattingType> m_listCsChattingType = new List<CsChattingType>();
	List<CsMainQuestDungeon> m_listCsMainQuestDungeon = new List<CsMainQuestDungeon>();
	List<CsLevelUpRewardEntry> m_listCsLevelUpRewardEntry = new List<CsLevelUpRewardEntry>();
	List<CsDailyAttendRewardEntry> m_listCsDailyAttendRewardEntry = new List<CsDailyAttendRewardEntry>();
	List<CsWeekendAttendRewardAvailableDayOfWeek> m_listCsWeekendAttendRewardAvailableDayOfWeek = new List<CsWeekendAttendRewardAvailableDayOfWeek>();
	List<CsAccessRewardEntry> m_listCsAccessRewardEntry = new List<CsAccessRewardEntry>();
	List<CsMountQualityMaster> m_listCsMountQualityMaster = new List<CsMountQualityMaster>();
	List<CsMountLevelMaster> m_listCsMountLevelMaster = new List<CsMountLevelMaster>();
	List<CsMount> m_listMount = new List<CsMount>();
	List<CsMountGearType> m_listCsMountGearType = new List<CsMountGearType>();
	List<CsMountGearGrade> m_listCsMountGearGrade = new List<CsMountGearGrade>();
	List<CsMountGearQuality> m_listCsMountGearQuality = new List<CsMountGearQuality>();
	List<CsMountGear> m_listCsMountGear = new List<CsMountGear>();
	List<CsMountGearOptionAttrGrade> m_listCsMountGearOptionAttrGrade = new List<CsMountGearOptionAttrGrade>();
	List<CsMountGearPickBoxRecipe> m_listCsMountGearPickBoxRecipe = new List<CsMountGearPickBoxRecipe>();
	List<CsStoryDungeon> m_listCsStoryDungeon = new List<CsStoryDungeon>();
	List<CsWingStep> m_listCsWingStep = new List<CsWingStep>();
	List<CsWingPart> m_listCsWingPart = new List<CsWingPart>();
	List<CsWing> m_listCsWing = new List<CsWing>();
	List<CsStaminaBuyCount> m_listCsStaminaBuyCount = new List<CsStaminaBuyCount>();
	CsExpDungeon m_csExpDungeon;
	List<CsMainGearEnchantLevelSet> m_listCsMainGearEnchantLevelSet = new List<CsMainGearEnchantLevelSet>();
	List<CsMainGearSet> m_listCsMainGearSet = new List<CsMainGearSet>();
	List<CsSubGearSoulstoneLevelSet> m_listCsSubGearSoulstoneLevelSet = new List<CsSubGearSoulstoneLevelSet>();
	List<CsCartGrade> m_listCsCartGrade = new List<CsCartGrade>();
	List<CsCart> m_listCsCart = new List<CsCart>();
	CsGoldDungeon m_csGoldDungeon;
	List<CsWorldLevelExpFactor> m_listCsWorldLevelExpFactor = new List<CsWorldLevelExpFactor>();
	CsThreatOfFarmQuest m_csThreatOfFarmQuest;
	CsUndergroundMaze m_csUndergroundMaze;
	List<CsBountyHunterQuest> m_listCsBountyHunterQuest = new List<CsBountyHunterQuest>();
	List<CsBountyHunterQuestReward> m_listCsBountyHunterQuestReward = new List<CsBountyHunterQuestReward>();
	CsFishingQuest m_csFishingQuest;
	CsArtifactRoom m_csArtifactRoom;
	List<CsMysteryBoxGrade> m_listCsMysteryBoxGrade = new List<CsMysteryBoxGrade>();
	CsMysteryBoxQuest m_csMysteryBoxQuest;
	List<CsSecretLetterGrade> m_listCsSecretLetterGrade = new List<CsSecretLetterGrade>();
	CsSecretLetterQuest m_csSecretLetterQuest;
	List<CsTodayMission> m_listCsTodayMission = new List<CsTodayMission>();
	List<CsSeriesMission> m_listCsSeriesMission = new List<CsSeriesMission>();
	CsDimensionInfiltrationEvent m_csDimensionInfiltrationEvent;
	CsDimensionRaidQuest m_csDimensionRaidQuest;
	CsAncientRelic m_csAncientRelic;
	List<CsTodayTaskCategory> m_listCsTodayTaskCategory = new List<CsTodayTaskCategory>();
	List<CsTodayTask> m_listCsTodayTask = new List<CsTodayTask>();
	List<CsAchievementReward> m_listCsAchievementReward = new List<CsAchievementReward>();
	CsHolyWarQuest m_csHolyWarQuest;
	CsFieldOfHonor m_csFieldOfHonor;
	List<CsHonorShopProduct> m_listCsHonorShopProduct = new List<CsHonorShopProduct>();
	List<CsRank> m_listCsRank = new List<CsRank>();
	CsBattlefieldSupportEvent m_csBattlefieldSupportEvent;
	List<CsLevelRankingReward> m_listCsLevelRankingReward = new List<CsLevelRankingReward>();
	List<CsAttainmentEntry> m_listCsAttainmentEntry = new List<CsAttainmentEntry>();
	List<CsMenu> m_listCsMenu = new List<CsMenu>();
	List<CsMenuContent> m_listCsMenuContent = new List<CsMenuContent>();
	List<CsGuildLevel> m_listCsGuildLevel = new List<CsGuildLevel>();
	List<CsGuildMemberGrade> m_listCsGuildMemberGrade = new List<CsGuildMemberGrade>();
	CsSupplySupportQuest m_csSupplySupportQuest;

	Dictionary<long, CsGuildContributionPointReward> m_dicCsGuildContributionPointReward = new Dictionary<long, CsGuildContributionPointReward>();
	Dictionary<long, CsGuildBuildingPointReward> m_dicCsGuildBuildingPointReward = new Dictionary<long, CsGuildBuildingPointReward>();
	Dictionary<long, CsGuildFundReward> m_dicCsGuildFundReward = new Dictionary<long, CsGuildFundReward>();
	Dictionary<long, CsGuildPointReward> m_dicCsGuildPointReward = new Dictionary<long, CsGuildPointReward>();
	List<CsGuildDonationEntry> m_listCsGuildDonationEntry = new List<CsGuildDonationEntry>();

	Dictionary<long, CsNationFundReward> m_dicCsNationFundReward = new Dictionary<long, CsNationFundReward>();
	List<CsNationDonationEntry> m_listCsNationDonationEntry = new List<CsNationDonationEntry>();
	List<CsNationNoblesse> m_listCsNationNoblesse = new List<CsNationNoblesse>();

	List<CsGuildBuilding> m_listCsGuildBuilding = new List<CsGuildBuilding>();
	List<CsGuildSkill> m_listCsGuildSkill = new List<CsGuildSkill>();
	CsGuildTerritory m_csGuildTerritory;
	CsGuildFarmQuest m_csGuildFarmQuest;
	CsGuildFoodWarehouse m_csGuildFoodWarehouse;
	CsGuildAltar m_csGuildAltar;
	CsGuildMissionQuest m_csGuildMissionQuest;

	CsNationWar m_csNationWar;
	List<CsNationWarRevivalPoint> m_listCsNationWarRevivalPoint = new List<CsNationWarRevivalPoint>();

	CsGuildSupplySupportQuest m_csGuildSupplySupportQuest;

	CsSoulCoveter m_csSoulCoveter;
	List<CsClientTutorialStep> m_listCsClientTutorialStep = new List<CsClientTutorialStep>();
	Dictionary<long, CsOwnDiaReward> m_dicCsOwnDiaReward = new Dictionary<long, CsOwnDiaReward>();

	List<CsGuildContent> m_listCsGuildContent = new List<CsGuildContent>();
	List<CsGuildDailyObjectiveReward> m_listCsGuildDailyObjectiveReward = new List<CsGuildDailyObjectiveReward>();
	List<CsGuildWeeklyObjective> m_listCsGuildWeeklyObjective = new List<CsGuildWeeklyObjective>();
	CsGuildHuntingQuest m_csGuildHuntingQuest;

	List<CsIllustratedBookCategory> m_listCsIllustratedBookCategory = new List<CsIllustratedBookCategory>();
	List<CsIllustratedBookType> m_listCsIllustratedBookType = new List<CsIllustratedBookType>();
	List<CsIllustratedBookAttrGrade> m_listCsIllustratedBookAttrGrade = new List<CsIllustratedBookAttrGrade>();
	List<CsIllustratedBook> m_listCsIllustratedBook = new List<CsIllustratedBook>();
	List<CsIllustratedBookExplorationStep> m_listCsIllustratedBookExplorationStep = new List<CsIllustratedBookExplorationStep>();
	List<CsSceneryQuest> m_listCsSceneryQuest = new List<CsSceneryQuest>();

	List<CsAccomplishmentCategory> m_listCsAccomplishmentCategory = new List<CsAccomplishmentCategory>();
	List<CsAccomplishment> m_listCsAccomplishment = new List<CsAccomplishment>();
	List<CsTitleCategory> m_listCsTitleCategory = new List<CsTitleCategory>();
	List<CsTitleType> m_listCsTitleType = new List<CsTitleType>();
	List<CsTitleGrade> m_listCsTitleGrade = new List<CsTitleGrade>();
	List<CsTitle> m_listCsTitle = new List<CsTitle>();

	List<CsEliteMonsterCategory> m_listCsEliteMonsterCategory = new List<CsEliteMonsterCategory>();
	List<CsEliteMonsterMaster> m_listCsEliteMonsterMaster = new List<CsEliteMonsterMaster>();
	List<CsEliteMonster> m_listCsEliteMonster = new List<CsEliteMonster>();
	CsEliteDungeon m_csEliteDungeon;
	List<CsCreatureCardCategory> m_listCsCreatureCardCategory = new List<CsCreatureCardCategory>();
	List<CsCreatureCardGrade> m_listCsCreatureCardGrade = new List<CsCreatureCardGrade>();
	List<CsCreatureCard> m_listCsCreatureCard = new List<CsCreatureCard>();
	List<CsCreatureCardCollectionCategory> m_listCsCreatureCardCollectionCategory = new List<CsCreatureCardCollectionCategory>();
	List<CsCreatureCardCollectionGrade> m_listCsCreatureCardCollectionGrade = new List<CsCreatureCardCollectionGrade>();
	List<CsCreatureCardCollection> m_listCsCreatureCardCollection = new List<CsCreatureCardCollection>();
	List<CsCreatureCardShopRefreshSchedule> m_listCsCreatureCardShopRefreshSchedule = new List<CsCreatureCardShopRefreshSchedule>();
	List<CsCreatureCardShopFixedProduct> m_listCsCreatureCardShopFixedProduct = new List<CsCreatureCardShopFixedProduct>();
	List<CsCreatureCardShopRandomProduct> m_listCsCreatureCardShopRandomProduct = new List<CsCreatureCardShopRandomProduct>();
	List<CsCreatureCardCollectionEntry> m_listCsCreatureCardCollectionEntry = new List<CsCreatureCardCollectionEntry>();

	List<CsStaminaRecoverySchedule> m_listCsStaminaRecoverySchedule = new List<CsStaminaRecoverySchedule>();

	CsProofOfValor m_csProofOfValor;

	List<CsBanWord> m_listCsBanWord = new List<CsBanWord>();

	List<CsMenuContentOpenPreview> m_listCsMenuContentOpenPreview = new List<CsMenuContentOpenPreview>();

	List<CsJobCommonSkill> m_listCsJobCommonSkill = new List<CsJobCommonSkill>();

	List<CsNpcShop> m_listCsNpcShop = new List<CsNpcShop>();

	List<CsRankActiveSkill> m_listCsRankActiveSkill = new List<CsRankActiveSkill>();
	List<CsRankPassiveSkill> m_listCsRankPassiveSkill = new List<CsRankPassiveSkill>();

	List<CsRookieGift> m_listCsRookieGift = new List<CsRookieGift>();
	List<CsOpenGiftReward> m_listCsOpenGiftReward = new List<CsOpenGiftReward>();
	List<CsQuickMenu> m_listCsQuickMenu = new List<CsQuickMenu>();
	CsDailyQuest m_csDailyQuest;
	List<CsDailyQuestGrade> m_listCsDailyQuestGrade = new List<CsDailyQuestGrade>();

	CsWisdomTemple m_csWisdomTemple;

	CsWeeklyQuest m_csWeeklyQuest;

	List<CsOpen7DayEventDay> m_listCsOpen7DayEventDay = new List<CsOpen7DayEventDay>();

	List<CsRetrieval> m_listCsRetrieval = new List<CsRetrieval>();

	List<CsTaskConsignment> m_listCsTaskConsignment = new List<CsTaskConsignment>();

	CsRuinsReclaim m_csRuinsReclaim;
	CsInfiniteWar m_csInfiniteWar;
	CsTrueHeroQuest m_csTrueHeroQuest;
	CsLimitationGift m_csLimitationGift;
	CsWeekendReward m_csWeekendReward;
	CsFieldBossEvent m_csFieldBossEvent;

	List<CsWarehouseSlotExtendRecipe> m_listCsWarehouseSlotExtendRecipe = new List<CsWarehouseSlotExtendRecipe>();

	CsFearAltar m_csFearAltar;
	List<CsFearAltarHalidomElemental> m_listCsFearAltarHalidomElemental = new List<CsFearAltarHalidomElemental>();
	List<CsFearAltarHalidomLevel> m_listCsFearAltarHalidomLevel = new List<CsFearAltarHalidomLevel>();

	List<CsDiaShopCategory> m_listCsDiaShopCategory = new List<CsDiaShopCategory>();
	List<CsDiaShopProduct> m_listCsDiaShopProduct = new List<CsDiaShopProduct>();

	List<CsWingMemoryPieceType> m_listCsWingMemoryPieceType = new List<CsWingMemoryPieceType>();

	List<CsSubQuest> m_listCsSubQuest = new List<CsSubQuest>();

	CsWarMemory m_csWarMemory;

	List<CsOrdealQuest> m_listCsOrdealQuest = new List<CsOrdealQuest>();

	CsOsirisRoom m_csOsirisRoom;
	List<CsMoneyBuff> m_listCsMoneyBuff = new List<CsMoneyBuff>();

	List<CsBiography> m_listCsBiography = new List<CsBiography>();
	List<CsBiographyQuestDungeon> m_listCsBiographyQuestDungeon = new List<CsBiographyQuestDungeon>();

	CsItemLuckyShop m_csItemLuckyShop;
	CsCreatureCardLuckyShop m_csCreatureCardLuckyShop;

	List<CsBlessing> m_listCsBlessing = new List<CsBlessing>();
	List<CsBlessingTargetLevel> m_listCsBlessingTargetLevel = new List<CsBlessingTargetLevel>();

	CsDragonNest m_csDragonNest;

	List<CsCreatureGrade> m_listCsCreatureGrade = new List<CsCreatureGrade>();
	List<CsCreatureCharacter> m_listCsCreatureCharacter = new List<CsCreatureCharacter>();
	List<CsCreatureSkillSlotOpenRecipe> m_listCsCreatureSkillSlotOpenRecipe = new List<CsCreatureSkillSlotOpenRecipe>();
	List<CsCreatureSkillSlotProtection> m_listCsCreatureSkillSlotProtection = new List<CsCreatureSkillSlotProtection>();
	List<CsCreature> m_listCsCreature = new List<CsCreature>();
	List<CsCreatureSkillGrade> m_listCsCreatureSkillGrade = new List<CsCreatureSkillGrade>();
	List<CsCreatureSkill> m_listCsCreatureSkill = new List<CsCreatureSkill>();
	List<CsCreatureBaseAttr> m_listCsCreatureBaseAttr = new List<CsCreatureBaseAttr>();
	List<CsCreatureLevel> m_listCsCreatureLevel = new List<CsCreatureLevel>();
	List<CsCreatureAdditionalAttr> m_listCsCreatureAdditionalAttr = new List<CsCreatureAdditionalAttr>();
	List<CsCreatureInjectionLevel> m_listCsCreatureInjectionLevel = new List<CsCreatureInjectionLevel>();

	List<CsPresent> m_listCsPresent = new List<CsPresent>();
	List<CsWeeklyPresentContributionPointRankingRewardGroup> m_listCsWeeklyPresentContributionPointRankingRewardGroup = new List<CsWeeklyPresentContributionPointRankingRewardGroup>();
	List<CsWeeklyPresentPopularityPointRankingRewardGroup> m_listCsWeeklyPresentPopularityPointRankingRewardGroup = new List<CsWeeklyPresentPopularityPointRankingRewardGroup>();
	List<CsCostume> m_listCsCostume = new List<CsCostume>();
	List<CsCostumeEffect> m_listCsCostumeEffect = new List<CsCostumeEffect>();

	CsCreatureFarmQuest m_csCreatureFarmQuest;

	CsSafeTimeEvent m_csSafeTimeEvent;

	List<CsGuildBlessingBuff> m_listCsGuildBlessingBuff = new List<CsGuildBlessingBuff>();

	List<CsJobChangeQuest> m_listCsJobChangeQuest = new List<CsJobChangeQuest>();
	List<CsRecommendBattlePowerLevel> m_listCsRecommendBattlePowerLevel = new List<CsRecommendBattlePowerLevel>();
	List<CsImprovementContent> m_listCsImprovementContent = new List<CsImprovementContent>();
	List<CsImprovementContentAchievement> m_listCsImprovementContentAchievement = new List<CsImprovementContentAchievement>();
	List<CsImprovementMainCategory> m_listCsImprovementMainCategory = new List<CsImprovementMainCategory>();

	List<CsPotionAttr> m_listCsPotionAttr = new List<CsPotionAttr>();

	List<CsInAppProduct> m_listCsInAppProduct = new List<CsInAppProduct>();
	List<CsCashProduct> m_listCsCashProduct = new List<CsCashProduct>();
	CsFirstChargeEvent m_csFirstChargeEvent;
	CsRechargeEvent m_csRechargeEvent;
	List<CsChargeEvent> m_listCsChargeEvent = new List<CsChargeEvent>();
	CsDailyChargeEvent m_csDailyChargeEvent;
	List<CsConsumeEvent> m_listCsConsumeEvent = new List<CsConsumeEvent>();
	CsDailyConsumeEvent m_csDailyConsumeEvent;

	CsAnkouTomb m_csAnkouTomb;
	List<CsConstellation> m_listCsConstellation = new List<CsConstellation>();
	List<CsArtifact> m_listCsArtifact = new List<CsArtifact>();
	List<CsArtifactLevelUpMaterial> m_listCsArtifactLevelUpMaterial = new List<CsArtifactLevelUpMaterial>();
	List<CsMountAwakeningLevelMaster> m_listCsMountAwakeningLevelMaster = new List<CsMountAwakeningLevelMaster>();
	List<CsMountPotionAttrCount> m_listCsMountPotionAttrCount = new List<CsMountPotionAttrCount>();

	CsTradeShip m_csTradeShip;
	List<CsCostumeCollection> m_listCsCostumeCollection = new List<CsCostumeCollection>();
	List<CsCostumeEnchantLevel> m_listCsCostumeEnchantLevel = new List<CsCostumeEnchantLevel>();
	List<CsCostumeEnchantLevelAttr> m_listCsCostumeEnchantLevelAttr = new List<CsCostumeEnchantLevelAttr>();

	List<CsScheduleNotice> m_listCsScheduleNotice = new List<CsScheduleNotice>();
	List<CsSharingEvent> m_listCsSharingEvent = new List<CsSharingEvent>();
	List<CsSystemMessageInfo> m_listCsSystemMessageInfo = new List<CsSystemMessageInfo>();

	List<CsTradition> m_listCsTradition = new List<CsTradition>();
	CsTeamBattlefield m_csTeamBattlefield;
	List<CsAccomplishmentLevel> m_listCsAccomplishmentLevel = new List<CsAccomplishmentLevel>();
	Dictionary<long, CsAccomplishmentPointReward> m_dicCsAccomplishmentPointReward = new Dictionary<long, CsAccomplishmentPointReward>();

	//---------------------------------------------------------------------------------------------------
	public Transform MyHeroTransform { get { return m_trMyHero; } set { m_trMyHero = value; } }
	public List<IHeroObjectInfo> ListHeroObjectInfo { get { return m_listHeroObjectInfo; } }
	public List<IMonsterObjectInfo> ListMonsterObjectInfo { get { return m_listMonsterObjectInfo; } }
	public List<INpcObjectInfo> ListNpcObjectInfo { get { return m_listNpcObjectInfo; } }
	public List<ICartObjectInfo> ListCartObjectInfo { get { return m_listCartObjectInfo; } }

	public Vector3[] PathCornerByAutoMove { get { return m_avtPathCornerByAutoMove; } set { m_avtPathCornerByAutoMove = value; } }
	public int AutoMoveLocationId { get { return m_nAutoMoveLocationId; } set { m_nAutoMoveLocationId = value; } }
	public int AutoMoveNationId { get { return m_nAutoMoveNationId; } set { m_nAutoMoveNationId = value; } }
	public Vector3 AutoMoveObjectPos { get { return m_vtAutoMoveObjectPos; } set { m_vtAutoMoveObjectPos = value; } }

	public bool JoystickDragging { get { return m_bJoystickDragging; } set { m_bJoystickDragging = value; } }
	public bool JoystickDown { get { return m_bJoystickDown; } set { m_bJoystickDown = value; } }
	public bool JoysticWalk { get { return m_bJoysticWalk; } set { m_bJoysticWalk = value; } }
	public float JoystickAngle { get { return m_flJoystickAngle; } set { m_flJoystickAngle = value; } }

	public CsMyHeroInfo MyHeroInfo { get { return m_csMyHeroInfo; } set { m_csMyHeroInfo = value; } }

	// Game Meta Data Attributes
	public List<CsElemental> ElementalList { get { return m_csElemental; } }
	public List<CsJob> JobList { get { return m_listCsJob; } }
	public List<CsNation> NationList { get { return m_listCsNation; } }
	public List<CsAttrCategory> AttrCategoryList { get { return m_listCsAttrCategory; } }
	public List<CsAttr> AttrList { get { return m_listCsAttr; } }
	//public List<CsAttrValueInfo> AttrValueInfoList { get { return m_listCsAttrValueInfo; } }
	public Dictionary<long, CsAttrValueInfo> AttrValueInfoDictionary { get { return m_dicCsAttrValueInfo; } }

	public List<CsItemMainCategory> ItemMainCategoryList { get { return m_listCsItemMainCategory; } }
	public List<CsItemType> ItemTypeList { get { return m_listCsItemType; } }
	public List<CsItemGrade> ItemGradeList { get { return m_listCsItemGrade; } }
	public List<CsItem> ItemList { get { return m_listCsItem; } }

	//public List<CsExpReward> ExpRewardList { get { return m_listCsExpReward; } }
	//public List<CsGoldReward> GoldRewardList { get { return m_listCsGoldReward; } }
	//public List<CsItemReward> ItemRewardList { get { return m_listCsItemReward; } }

	public Dictionary<long, CsExpReward> ExpRewardDictionary { get { return m_dicCsExpReward; } }
	public Dictionary<long, CsGoldReward> GoldRewardDictionary { get { return m_dicCsGoldReward; } }
	public Dictionary<long, CsItemReward> ItemRewardDictionary { get { return m_dicCsItemReward; } }
	public Dictionary<long, CsExploitPointReward> ExploitPointRewardDictionary { get { return m_dicCsExploitPointReward; } }
	public Dictionary<long, CsHonorPointReward> HonorPointRewardDictionary { get { return m_dicCsHonorPointReward; } }

	public List<CsMainGearCategory> MainGearCategoryList { get { return m_listCsMainGearCategory; } }
	public List<CsMainGearType> MainGearTypeList { get { return m_listCsMainGearType; } }
	public List<CsMainGearTier> MainGearTierList { get { return m_listCsMainGearTier; } }
	public List<CsMainGearGrade> MainGearGradeList { get { return m_listCsMainGearGrade; } }
	public List<CsMainGearQuality> MainGearQualityList { get { return m_listCsMainGearQuality; } }
	public List<CsMainGear> MainGearList { get { return m_listCsMainGear; } }
	public List<CsMainGearEnchantStep> MainGearEnchantStepList { get { return m_listCsMainGearEnchantStep; } }
	public List<CsMainGearEnchantLevel> MainGearEnchantLevelList { get { return m_listCsMainGearEnchantLevel; } }
	public List<CsMainGearOptionAttrGrade> MainGearOptionAttrGradeList { get { return m_listCsMainGearOptionAttrGrade; } }
	public List<CsMainGearRefinementRecipe> MainGearRefinementRecipeList { get { return m_listCsMainGearRefinementRecipe; } }
	public List<CsSubGearGrade> SubGearGradeList { get { return m_listCsSubGearGrade; } }
	public List<CsSubGear> SubGearList { get { return m_listCsSubGear; } }
	public List<CsLocation> LocationList { get { return m_listCsLocation; } }
	public List<CsMonsterCharacter> MonsterCharacterList { get { return m_listCsMonsterCharacter; } }
	public List<CsMonsterInfo> MonsterInfoList { get { return m_listCsMonsterInfo; } }
	public List<CsContinent> ContinentList { get { return m_listCsContinent; } }
	public List<CsMainMenu> MainMenuList { get { return m_listCsMainMenu; } }
	public List<CsJobSkillMaster> JobSkillMasterList { get { return m_listCsJobSkillMaster; } }
	public List<CsJobSkill> JobSkillList { get { return m_listCsJobSkill; } }
	public List<CsPortal> PortalList { get { return m_listCsPortal; } }
	public List<CsNpcInfo> NpcInfoList { get { return m_listCsNpcInfo; } }
	public List<CsContinentObject> ContinentObjectList { get { return m_listCsContinentObject; } }
	public List<CsMainQuest> MainQuestList { get { return m_listCsMainQuest; } }
	public List<CsPaidImmediateRevival> PaidImmediateRevivalList { get { return m_listCsPaidImmediateRevival; } }
	public List<CsMonsterSkill> MonsterSkillList { get { return m_listCsMonsterSkill; } }
	public List<CsAbnormalState> AbnormalStateList { get { return m_listCsAbnormalState; } }
	public List<CsJobLevelMaster> JobLevelMasterList { get { return m_listCsJobLevelMaster; } }
	public Dictionary<long, CsMonsterArrange> MonsterArrangeDictionary { get { return m_dicCsMonsterArrange; } }
	public List<CsSimpleShopProduct> SimpleShopProductList { get { return m_listCsSimpleShopProduct; } }
	public List<CsVipLevel> VipLevelList { get { return m_listCsVipLevel; } }
	public List<CsInventorySlotExtendRecipe> InventorySlotExtendRecipeList { get { return m_listCsInventorySlotExtendRecipe; } }
	public List<CsMainGearDisassembleAvailableResultEntry> MainGearDisassembleAvailableResultEntryList { get { return m_listCsMainGearDisassembleAvailableResultEntry; } }
	public List<CsItemCompositionRecipe> ItemCompositionRecipeList { get { return m_listCsItemCompositionRecipe; } }
	public List<CsRestRewardTime> RestRewardTimeList { get { return m_listCsRestRewardTime; } }
	public List<CsChattingType> ChattingTypeList { get { return m_listCsChattingType; } }
	public List<CsMainQuestDungeon> MainQuestDungeonList { get { return m_listCsMainQuestDungeon; } }
	public List<CsLevelUpRewardEntry> LevelUpRewardEntryList { get { return m_listCsLevelUpRewardEntry; } }
	public List<CsDailyAttendRewardEntry> DailyAttendRewardEntryList { get { return m_listCsDailyAttendRewardEntry; } }
	public List<CsWeekendAttendRewardAvailableDayOfWeek> WeekendAttendRewardAvailableDayOfWeekList { get { return m_listCsWeekendAttendRewardAvailableDayOfWeek; } }
	public List<CsAccessRewardEntry> AccessRewardEntryList { get { return m_listCsAccessRewardEntry; } }
	public List<CsMountQualityMaster> MountQualityMasterList { get { return m_listCsMountQualityMaster; } }
	public List<CsMountLevelMaster> MountLevelMasterList { get { return m_listCsMountLevelMaster; } }
	public List<CsMount> MountList { get { return m_listMount; } }
	public List<CsMountGearType> MountGearTypeList { get { return m_listCsMountGearType; } }
	public List<CsMountGearGrade> MountGearGradeList { get { return m_listCsMountGearGrade; } }
	public List<CsMountGearQuality> MountGearQualityList { get { return m_listCsMountGearQuality; } }
	public List<CsMountGear> MountGearList { get { return m_listCsMountGear; } }
	public List<CsMountGearOptionAttrGrade> MountGearOptionAttrGradeList { get { return m_listCsMountGearOptionAttrGrade; } }
	public List<CsMountGearPickBoxRecipe> MountGearPickBoxRecipeList { get { return m_listCsMountGearPickBoxRecipe; } }
	public List<CsStoryDungeon> StoryDungeonList { get { return m_listCsStoryDungeon; } }
	public List<CsWingStep> WingStepList { get { return m_listCsWingStep; } }
	public List<CsWingPart> WingPartList { get { return m_listCsWingPart; } }
	public List<CsWing> WingList { get { return m_listCsWing; } }
	public List<CsStaminaBuyCount> StaminaBuyCountList { get { return m_listCsStaminaBuyCount; } }
	public CsExpDungeon ExpDungeon { get { return m_csExpDungeon; } set { m_csExpDungeon = value; } }
	public List<CsMainGearEnchantLevelSet> MainGearEnchantLevelSetList { get { return m_listCsMainGearEnchantLevelSet; } }
	public List<CsMainGearSet> MainGearSetList { get { return m_listCsMainGearSet; } }
	public List<CsSubGearSoulstoneLevelSet> SubGearSoulstoneLevelSetList { get { return m_listCsSubGearSoulstoneLevelSet; } }
	public List<CsCartGrade> CartGradeList { get { return m_listCsCartGrade; } }
	public List<CsCart> CartList { get { return m_listCsCart; } }
	public CsGoldDungeon GoldDungeon { get { return m_csGoldDungeon; } set { m_csGoldDungeon = value; } }
	public List<CsWorldLevelExpFactor> WorldLevelExpFactorList { get { return m_listCsWorldLevelExpFactor; } }
	public CsThreatOfFarmQuest ThreatOfFarmQuest { get { return m_csThreatOfFarmQuest; } set { m_csThreatOfFarmQuest = value; } }
	public CsUndergroundMaze UndergroundMaze { get { return m_csUndergroundMaze; } set { m_csUndergroundMaze = value; } }
	public List<CsBountyHunterQuest> BountyHunterQuestList { get { return m_listCsBountyHunterQuest; } }
	public List<CsBountyHunterQuestReward> BountyHunterQuestRewardList { get { return m_listCsBountyHunterQuestReward; } }
	public CsFishingQuest FishingQuest { get { return m_csFishingQuest; } set { m_csFishingQuest = value; } }
	public CsArtifactRoom ArtifactRoom { get { return m_csArtifactRoom; } set { m_csArtifactRoom = value; } }
	public List<CsMysteryBoxGrade> MysteryBoxGradeList { get { return m_listCsMysteryBoxGrade; } }
	public CsMysteryBoxQuest MysteryBoxQuest { get { return m_csMysteryBoxQuest; } set { m_csMysteryBoxQuest = value; } }
	public List<CsSecretLetterGrade> SecretLetterGradeList { get { return m_listCsSecretLetterGrade; } }
	public CsSecretLetterQuest SecretLetterQuest { get { return m_csSecretLetterQuest; } set { m_csSecretLetterQuest = value; } }
	public List<CsTodayMission> TodayMissionList { get { return m_listCsTodayMission; } }
	public List<CsSeriesMission> SeriesMissionList { get { return m_listCsSeriesMission; } }
	public CsDimensionInfiltrationEvent DimensionInfiltrationEvent { get { return m_csDimensionInfiltrationEvent; } set { m_csDimensionInfiltrationEvent = value; } }
	public CsDimensionRaidQuest DimensionRaidQuest { get { return m_csDimensionRaidQuest; } set { m_csDimensionRaidQuest = value; } }
	public CsAncientRelic AncientRelic { get { return m_csAncientRelic; } set { m_csAncientRelic = value; } }
	public List<CsTodayTaskCategory> TodayTaskCategoryList { get { return m_listCsTodayTaskCategory; } }
	public List<CsTodayTask> TodayTaskList { get { return m_listCsTodayTask; } }
	public List<CsAchievementReward> AchievementRewardList { get { return m_listCsAchievementReward; } }
	public CsHolyWarQuest HolyWarQuest { get { return m_csHolyWarQuest; } set { m_csHolyWarQuest = value; } }
	public CsFieldOfHonor FieldOfHonor { get { return m_csFieldOfHonor; } set { m_csFieldOfHonor = value; } }
	public List<CsHonorShopProduct> HonorShopProductList { get { return m_listCsHonorShopProduct; } }
	public List<CsRank> RankList { get { return m_listCsRank; } }
	public CsBattlefieldSupportEvent BattlefieldSupportEvent { get { return m_csBattlefieldSupportEvent; } set { m_csBattlefieldSupportEvent = value; } }
	public List<CsLevelRankingReward> LevelRankingRewardList { get { return m_listCsLevelRankingReward; } }
	public List<CsAttainmentEntry> AttainmentEntryList { get { return m_listCsAttainmentEntry; } }
	public List<CsMenu> MenuList { get { return m_listCsMenu; } }
	public List<CsMenuContent> MenuContentList { get { return m_listCsMenuContent; } }
	public List<CsGuildLevel> GuildLevelList { get { return m_listCsGuildLevel; } }
	public List<CsGuildMemberGrade> GuildMemberGradeList { get { return m_listCsGuildMemberGrade; } }
	public CsSupplySupportQuest SupplySupportQuest { get { return m_csSupplySupportQuest; } set { m_csSupplySupportQuest = value; } }

	public Dictionary<long, CsGuildContributionPointReward> GuildContributionPointRewardDictionary { get { return m_dicCsGuildContributionPointReward; } }
	public Dictionary<long, CsGuildBuildingPointReward> GuildBuildingPointRewardDictionary { get { return m_dicCsGuildBuildingPointReward; } }
	public Dictionary<long, CsGuildFundReward> GuildFundRewardDictionary { get { return m_dicCsGuildFundReward; } }
	public Dictionary<long, CsGuildPointReward> GuildPointRewardDictionary { get { return m_dicCsGuildPointReward; } }
	public List<CsGuildDonationEntry> GuildDonationEntryList { get { return m_listCsGuildDonationEntry; } }

	public Dictionary<long, CsNationFundReward> NationFundRewardDictionary { get { return m_dicCsNationFundReward; } }
	public List<CsNationDonationEntry> NationDonationEntryList { get { return m_listCsNationDonationEntry; } }
	public List<CsNationNoblesse> NationNoblesseList { get { return m_listCsNationNoblesse; } }

	public List<CsGuildBuilding> GuildBuildingList { get { return m_listCsGuildBuilding; } }
	public List<CsGuildSkill> GuildSkillList { get { return m_listCsGuildSkill; } }
	public CsGuildTerritory GuildTerritory { get { return m_csGuildTerritory; } set { m_csGuildTerritory = value; } }
	public CsGuildFarmQuest GuildFarmQuest { get { return m_csGuildFarmQuest; } set { m_csGuildFarmQuest = value; } }
	public CsGuildFoodWarehouse GuildFoodWarehouse { get { return m_csGuildFoodWarehouse; } set { m_csGuildFoodWarehouse = value; } }
	public CsGuildAltar GuildAltar { get { return m_csGuildAltar; } set { m_csGuildAltar = value; } }
	public CsGuildMissionQuest GuildMissionQuest { get { return m_csGuildMissionQuest; } set { m_csGuildMissionQuest = value; } }

	public CsNationWar NationWar { get { return m_csNationWar; } set { m_csNationWar = value; } }
	public List<CsNationWarRevivalPoint> NationWarRevivalPointList { get { return m_listCsNationWarRevivalPoint; } }

	public CsGuildSupplySupportQuest GuildSupplySupportQuest { get { return m_csGuildSupplySupportQuest; } set { m_csGuildSupplySupportQuest = value; } }

	public CsSoulCoveter SoulCoveter { get { return m_csSoulCoveter; } set { m_csSoulCoveter = value; } }
	public List<CsClientTutorialStep> ClientTutorialStepList { get { return m_listCsClientTutorialStep; } } 

	public Dictionary<long, CsOwnDiaReward> OwnDiaRewardDictionary { get { return m_dicCsOwnDiaReward; } }

	public List<CsGuildContent> GuildContentList { get { return m_listCsGuildContent; } }
	public List<CsGuildDailyObjectiveReward> GuildDailyObjectiveRewardList { get { return m_listCsGuildDailyObjectiveReward; } }
	public List<CsGuildWeeklyObjective> GuildWeeklyObjectiveList { get { return m_listCsGuildWeeklyObjective; } }
	public CsGuildHuntingQuest GuildHuntingQuest { get { return m_csGuildHuntingQuest; } set { m_csGuildHuntingQuest = value; } }

	public List<CsIllustratedBookCategory> IllustratedBookCategoryList { get { return m_listCsIllustratedBookCategory; } }
	public List<CsIllustratedBookType> IllustratedBookTypeList { get { return m_listCsIllustratedBookType; } }
	public List<CsIllustratedBookAttrGrade> IllustratedBookAttrGradeList { get { return m_listCsIllustratedBookAttrGrade; } }
	public List<CsIllustratedBook> IllustratedBookList { get { return m_listCsIllustratedBook; } }
	public List<CsIllustratedBookExplorationStep> IllustratedBookExplorationStepList { get { return m_listCsIllustratedBookExplorationStep; } }
	public List<CsSceneryQuest> SceneryQuestList { get { return m_listCsSceneryQuest; } }

	public List<CsAccomplishmentCategory> AccomplishmentCategoryList { get { return m_listCsAccomplishmentCategory; } }
	public List<CsAccomplishment> AccomplishmentList { get { return m_listCsAccomplishment; } }
	public List<CsTitleCategory> TitleCategoryList { get { return m_listCsTitleCategory; } }
	public List<CsTitleType> TitleTypeList { get { return m_listCsTitleType; } }
	public List<CsTitleGrade> TitleGradeList { get { return m_listCsTitleGrade; } }
	public List<CsTitle> TitleList { get { return m_listCsTitle; } }

	public List<CsEliteMonsterCategory> EliteMonsterCategoryList { get { return m_listCsEliteMonsterCategory; } }
	public List<CsEliteMonsterMaster> EliteMonsterMasterList { get { return m_listCsEliteMonsterMaster; } }
	public List<CsEliteMonster> EliteMonsterList { get { return m_listCsEliteMonster; } }
	public CsEliteDungeon EliteDungeon { get { return m_csEliteDungeon; } set { m_csEliteDungeon = value; } }
	public List<CsCreatureCardCategory> CreatureCardCategoryList { get { return m_listCsCreatureCardCategory; } }
	public List<CsCreatureCardGrade> CreatureCardGradeList { get { return m_listCsCreatureCardGrade; } }
	public List<CsCreatureCard> CreatureCardList { get { return m_listCsCreatureCard; } }
	public List<CsCreatureCardCollectionCategory> CreatureCardCollectionCategoryList { get { return m_listCsCreatureCardCollectionCategory; } }
	public List<CsCreatureCardCollectionGrade> CreatureCardCollectionGradeList { get { return m_listCsCreatureCardCollectionGrade; } }
	public List<CsCreatureCardCollection> CreatureCardCollectionList { get { return m_listCsCreatureCardCollection; } }
	public List<CsCreatureCardShopRefreshSchedule> CreatureCardShopRefreshScheduleList { get { return m_listCsCreatureCardShopRefreshSchedule; } }
	public List<CsCreatureCardShopFixedProduct> CreatureCardShopFixedProductList { get { return m_listCsCreatureCardShopFixedProduct; } }
	public List<CsCreatureCardShopRandomProduct> CreatureCardShopRandomProductList { get { return m_listCsCreatureCardShopRandomProduct; } }
	public List<CsCreatureCardCollectionEntry> CreatureCardCollectionEntryList { get { return m_listCsCreatureCardCollectionEntry; } }

	public List<CsStaminaRecoverySchedule> StaminaRecoveryScheduleList { get { return m_listCsStaminaRecoverySchedule; } }

	public CsProofOfValor ProofOfValor { get { return m_csProofOfValor; } set { m_csProofOfValor = value; } }

	public List<CsBanWord> BanWordList { get { return m_listCsBanWord; } }

	public List<CsMenuContentOpenPreview> MenuContentOpenPreviewList { get { return m_listCsMenuContentOpenPreview; } }

	public List<CsJobCommonSkill> JobCommonSkillList { get { return m_listCsJobCommonSkill; } }

	public List<CsNpcShop> NpcShopList { get { return m_listCsNpcShop; } }

	public List<CsRankActiveSkill> RankActiveSkillList { get { return m_listCsRankActiveSkill; } }
	public List<CsRankPassiveSkill> RankPassiveSkillList { get { return m_listCsRankPassiveSkill; } }

	public List<CsRookieGift> RookieGiftList { get { return m_listCsRookieGift; } }
	public List<CsOpenGiftReward> OpenGiftRewardList { get { return m_listCsOpenGiftReward; } }
	public List<CsQuickMenu> QuickMenuList { get { return m_listCsQuickMenu; } }
	public CsDailyQuest DailyQuest { get { return m_csDailyQuest; } set { m_csDailyQuest = value; } }
	public List<CsDailyQuestGrade> DailyQuestGradeList { get { return m_listCsDailyQuestGrade; } }

	public CsWisdomTemple WisdomTemple { get { return m_csWisdomTemple; } set { m_csWisdomTemple = value; } }

	public CsWeeklyQuest WeeklyQuest { get { return m_csWeeklyQuest; } set { m_csWeeklyQuest = value; } }

	public List<CsOpen7DayEventDay> Open7DayEventDayList { get { return m_listCsOpen7DayEventDay; } }

	public List<CsRetrieval> RetrievalList { get { return m_listCsRetrieval; } }

	public List<CsTaskConsignment> TaskConsignmentList { get { return m_listCsTaskConsignment; } }

	public CsRuinsReclaim RuinsReclaim { get { return m_csRuinsReclaim; } set { m_csRuinsReclaim = value; } }
	public CsInfiniteWar InfiniteWar { get { return m_csInfiniteWar; } set { m_csInfiniteWar = value; } }
	public CsTrueHeroQuest TrueHeroQuest { get { return m_csTrueHeroQuest; } set { m_csTrueHeroQuest = value; } }
	public CsLimitationGift LimitationGift { get { return m_csLimitationGift; } set { m_csLimitationGift = value; } }
	public CsWeekendReward WeekendReward { get { return m_csWeekendReward; } set { m_csWeekendReward = value; } }
	public CsFieldBossEvent FieldBossEvent { get { return m_csFieldBossEvent; } set { m_csFieldBossEvent = value; } }

	public List<CsWarehouseSlotExtendRecipe> WarehouseSlotExtendRecipeList { get { return m_listCsWarehouseSlotExtendRecipe; } }

	public CsFearAltar FearAltar { get { return m_csFearAltar; } set { m_csFearAltar = value; } }
	public List<CsFearAltarHalidomElemental> FearAltarHalidomElementalList { get { return m_listCsFearAltarHalidomElemental; } }
	public List<CsFearAltarHalidomLevel> FearAltarHalidomLevelList { get { return m_listCsFearAltarHalidomLevel; } }

	public List<CsDiaShopCategory> DiaShopCategoryList { get { return m_listCsDiaShopCategory; } }
	public List<CsDiaShopProduct> DiaShopProductList { get { return m_listCsDiaShopProduct; } }

	public List<CsWingMemoryPieceType> WingMemoryPieceTypeList { get { return m_listCsWingMemoryPieceType; } }

	public List<CsSubQuest> SubQuestList { get { return m_listCsSubQuest; } }

	public CsWarMemory WarMemory { get { return m_csWarMemory; } set { m_csWarMemory = value; } }

	public List<CsOrdealQuest> OrdealQuestList { get { return m_listCsOrdealQuest; } }

	public CsOsirisRoom OsirisRoom { get { return m_csOsirisRoom; } set { m_csOsirisRoom = value; } }
	public List<CsMoneyBuff> MoneyBuffList { get { return m_listCsMoneyBuff; } }

	public List<CsBiography> BiographyList { get { return m_listCsBiography; } }
	public List<CsBiographyQuestDungeon> BiographyQuestDungeonList { get { return m_listCsBiographyQuestDungeon; } }

	public CsItemLuckyShop ItemLuckyShop { get { return m_csItemLuckyShop; } set { m_csItemLuckyShop = value; } }
	public CsCreatureCardLuckyShop CreatureCardLuckyShop { get { return m_csCreatureCardLuckyShop; } set { m_csCreatureCardLuckyShop = value; } }

	public List<CsBlessing> BlessingList { get { return m_listCsBlessing; } }
	public List<CsBlessingTargetLevel> BlessingTargetLevelList { get { return m_listCsBlessingTargetLevel; } }

	public CsDragonNest DragonNest { get { return m_csDragonNest; } set { m_csDragonNest = value; } }

	public List<CsCreatureGrade> CreatureGradeList { get { return m_listCsCreatureGrade; } }
	public List<CsCreatureCharacter> CreatureCharacterList { get { return m_listCsCreatureCharacter; } }
	public List<CsCreatureSkillSlotOpenRecipe> CreatureSkillSlotOpenRecipeList { get { return m_listCsCreatureSkillSlotOpenRecipe; } }
	public List<CsCreatureSkillSlotProtection> CreatureSkillSlotProtectionList { get { return m_listCsCreatureSkillSlotProtection; } }
	public List<CsCreature> CreatureList { get { return m_listCsCreature; } }
	public List<CsCreatureSkillGrade> CreatureSkillGradeList { get { return m_listCsCreatureSkillGrade; } }
	public List<CsCreatureSkill> CreatureSkillList { get { return m_listCsCreatureSkill; } }
	public List<CsCreatureBaseAttr> CreatureBaseAttrList { get { return m_listCsCreatureBaseAttr; } }
	public List<CsCreatureLevel> CreatureLevelList { get { return m_listCsCreatureLevel; } }
	public List<CsCreatureAdditionalAttr> CreatureAdditionalAttrList { get { return m_listCsCreatureAdditionalAttr; } }
	public List<CsCreatureInjectionLevel> CreatureInjectionLevelList { get { return m_listCsCreatureInjectionLevel; } }

	public List<CsPresent> PresentList { get { return m_listCsPresent; } }
	public List<CsWeeklyPresentContributionPointRankingRewardGroup> WeeklyPresentContributionPointRankingRewardGroupList { get { return m_listCsWeeklyPresentContributionPointRankingRewardGroup; } }
	public List<CsWeeklyPresentPopularityPointRankingRewardGroup> WeeklyPresentPopularityPointRankingRewardGroupList { get { return m_listCsWeeklyPresentPopularityPointRankingRewardGroup; } }
	public List<CsCostume> CostumeList { get { return m_listCsCostume; } }
	public List<CsCostumeEffect> CostumeEffectList { get { return m_listCsCostumeEffect; } }

	public CsCreatureFarmQuest CreatureFarmQuest { get { return m_csCreatureFarmQuest; } set { m_csCreatureFarmQuest = value; } }

	public CsSafeTimeEvent SafeTimeEvent { get { return m_csSafeTimeEvent; } set { m_csSafeTimeEvent = value; } }

	public List<CsGuildBlessingBuff> GuildBlessingBuffList { get { return m_listCsGuildBlessingBuff; } }

	public List<CsJobChangeQuest> JobChangeQuestList { get { return m_listCsJobChangeQuest; } }
	public List<CsRecommendBattlePowerLevel> RecommendBattlePowerLevelList { get { return m_listCsRecommendBattlePowerLevel; } }
	public List<CsImprovementContent> ImprovementContentList { get { return m_listCsImprovementContent; } }
	public List<CsImprovementContentAchievement> ImprovementContentAchievementList { get { return m_listCsImprovementContentAchievement; } }
	public List<CsImprovementMainCategory> ImprovementMainCategoryList { get { return m_listCsImprovementMainCategory; } }

	public List<CsPotionAttr> PotionAttrList { get { return m_listCsPotionAttr; } }

	public List<CsInAppProduct> InAppProductList { get { return m_listCsInAppProduct; } }
	public List<CsCashProduct> CashProductList { get { return m_listCsCashProduct; } }
	public CsFirstChargeEvent FirstChargeEvent { get { return m_csFirstChargeEvent; } set { m_csFirstChargeEvent = value; } }
	public CsRechargeEvent RechargeEvent { get { return m_csRechargeEvent; } set { m_csRechargeEvent = value; } }
	public List<CsChargeEvent> ChargeEventList { get { return m_listCsChargeEvent; } }
	public CsDailyChargeEvent DailyChargeEvent { get { return m_csDailyChargeEvent; } set { m_csDailyChargeEvent = value; } }
	public List<CsConsumeEvent> ConsumeEventList { get { return m_listCsConsumeEvent; } }
	public CsDailyConsumeEvent DailyConsumeEvent { get { return m_csDailyConsumeEvent; } set { m_csDailyConsumeEvent = value; } }

	public CsAnkouTomb AnkouTomb { get { return m_csAnkouTomb; } set { m_csAnkouTomb = value; } }
	public List<CsConstellation> ConstellationList { get { return m_listCsConstellation; } }
	public List<CsArtifact> ArtifactList { get { return m_listCsArtifact; } }
	public List<CsArtifactLevelUpMaterial> ArtifactLevelUpMaterialList { get { return m_listCsArtifactLevelUpMaterial; } }
	public List<CsMountAwakeningLevelMaster> MountAwakeningLevelMasterList { get { return m_listCsMountAwakeningLevelMaster; } }
	public List<CsMountPotionAttrCount> MountPotionAttrCountList { get { return m_listCsMountPotionAttrCount; } }

	public CsTradeShip TradeShip { get { return m_csTradeShip; } set { m_csTradeShip = value; } }
	public List<CsCostumeCollection> CostumeCollectionList { get { return m_listCsCostumeCollection; } }
	public List<CsCostumeEnchantLevel> CostumeEnchantLevelList { get { return m_listCsCostumeEnchantLevel; } }
	public List<CsCostumeEnchantLevelAttr> CostumeEnchantLevelAttrList { get { return m_listCsCostumeEnchantLevelAttr; } }

	public List<CsScheduleNotice> ScheduleNoticeList { get { return m_listCsScheduleNotice; } }
	public List<CsSharingEvent> SharingEventList { get { return m_listCsSharingEvent; } }
	public List<CsSystemMessageInfo> SystemMessageInfoList { get { return m_listCsSystemMessageInfo; } }

	public List<CsTradition> TraditionList { get { return m_listCsTradition; } }
	public CsTeamBattlefield TeamBattlefield { get { return m_csTeamBattlefield; } set { m_csTeamBattlefield = value; } }
	public List<CsAccomplishmentLevel> AccomplishmentLevelList { get { return m_listCsAccomplishmentLevel; } }
	public Dictionary<long, CsAccomplishmentPointReward> AccomplishmentPointRewardDictionary { get { return m_dicCsAccomplishmentPointReward; } }

	//---------------------------------------------------------------------------------------------------
	public CsElemental GetElemental(int nElementalId)
	{
		for (int i = 0; i < m_csElemental.Count; i++)
		{
			if (m_csElemental[i].ElementalId == nElementalId)
				return m_csElemental[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJob GetJob(int nJobId)
	{
		for (int i = 0; i < m_listCsJob.Count; i++)
		{
			if (m_listCsJob[i].JobId == nJobId)
				return m_listCsJob[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNation GetNation(int nNationId)
	{
		for (int i = 0; i < m_listCsNation.Count; i++)
		{
			if (m_listCsNation[i].NationId == nNationId)
				return m_listCsNation[i];
		}

		return null;
	}

	public CsAttrCategory GetAttrCategory(int nAttrCategoryId)
	{
		for (int i = 0; i < m_listCsAttrCategory.Count; i++)
		{
			if (m_listCsAttrCategory[i].AttrCategoryId == nAttrCategoryId)
				return m_listCsAttrCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttr GetAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsAttr.Count; i++)
		{
			if (m_listCsAttr[i].AttrId == nAttrId)
				return m_listCsAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttr GetAttr(EnAttr enAttr)
	{
		for (int i = 0; i < m_listCsAttr.Count; i++)
		{
			if (m_listCsAttr[i].EnAttr == enAttr)
				return m_listCsAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttrValueInfo GetAttrValueInfo(long lAttrValueId)
	{
		//for (int i = 0; i < m_listCsAttrValueInfo.Count; i++)
		//{
		//	if (m_listCsAttrValueInfo[i].AttrValueId == lAttrValueId)
		//		return m_listCsAttrValueInfo[i];
		//}

		//return null;
		if (m_dicCsAttrValueInfo.ContainsKey(lAttrValueId))
			return m_dicCsAttrValueInfo[lAttrValueId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemType GetItemType(int nItemType)
	{
		for (int i = 0; i < m_listCsItemType.Count; i++)
		{
			if (m_listCsItemType[i].ItemType == nItemType)
				return m_listCsItemType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsItem GetItem(int nItemId)
	{
		for (int i = 0; i < m_listCsItem.Count; i++)
		{
			if (m_listCsItem[i].ItemId == nItemId)
				return m_listCsItem[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpReward GetExpReward(long lExpRewardId)
	{
		//for (int i = 0; i < m_listCsExpReward.Count; i++)
		//{
		//	if (m_listCsExpReward[i].ExpRewardId == lExpRewardId)
		//		return m_listCsExpReward[i];
		//}

		//return null;

		if (m_dicCsExpReward.ContainsKey(lExpRewardId))
			return m_dicCsExpReward[lExpRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldReward GetGoldReward(long lGoldRewardId)
	{
		//for (int i = 0; i < m_listCsGoldReward.Count; i++)
		//{
		//	if (m_listCsGoldReward[i].GoldRewardId == lGoldRewardId)
		//		return m_listCsGoldReward[i];
		//}

		//return null;

		if (m_dicCsGoldReward.ContainsKey(lGoldRewardId))
			return m_dicCsGoldReward[lGoldRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemReward GetItemReward(long lItemRewardId)
	{
		//for (int i = 0; i < m_listCsItemReward.Count; i++)
		//{
		//	if (m_listCsItemReward[i].ItemRewardId == lItemRewardId)
		//		return m_listCsItemReward[i];
		//}

		//return null;

		if (m_dicCsItemReward.ContainsKey(lItemRewardId))
			return m_dicCsItemReward[lItemRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearType GetMainGearType(int nMainGearType)
	{
		for (int i = 0; i < m_listCsMainGearType.Count; i++)
		{
			if (m_listCsMainGearType[i].MainGearType == nMainGearType)
				return m_listCsMainGearType[i];
		}

		return null;
	}

	public CsMainGearCategory GetMainGearCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsMainGearCategory.Count; i++)
		{
			if (m_listCsMainGearCategory[i].CategoryId == nCategoryId)
				return m_listCsMainGearCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearType GetMainGearType(EnMainGearType enMainGearType)
	{
		for (int i = 0; i < m_listCsMainGearType.Count; i++)
		{
			if (m_listCsMainGearType[i].EnMainGearType == enMainGearType)
				return m_listCsMainGearType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearTier GetMainGearTier(int nTier)
	{
		for (int i = 0; i < m_listCsMainGearTier.Count; i++)
		{
			if (m_listCsMainGearTier[i].Tier == nTier)
				return m_listCsMainGearTier[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearGrade GetMainGearGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsMainGearGrade.Count; i++)
		{
			if (m_listCsMainGearGrade[i].Grade == nGrade)
				return m_listCsMainGearGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearGrade GetMainGearGrade(EnMainGearGrade enMainGearGrade)
	{
		for (int i = 0; i < m_listCsMainGearGrade.Count; i++)
		{
			if (m_listCsMainGearGrade[i].EnGrade == enMainGearGrade)
				return m_listCsMainGearGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearQuality GetMainGearQuality(int nQuality)
	{
		for (int i = 0; i < m_listCsMainGearQuality.Count; i++)
		{
			if (m_listCsMainGearQuality[i].Quality == nQuality)
				return m_listCsMainGearQuality[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearQuality GetMainGearQuality(EnMainGearQuality enMainGearQuality)
	{
		for (int i = 0; i < m_listCsMainGearQuality.Count; i++)
		{
			if (m_listCsMainGearQuality[i].EnQuality == enMainGearQuality)
				return m_listCsMainGearQuality[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGear GetMainGear(int nGearId)
	{
		for (int i = 0; i < m_listCsMainGear.Count; i++)
		{
			if (m_listCsMainGear[i].MainGearId == nGearId)
				return m_listCsMainGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearRefinementRecipe GetMainGearRefinementRecipe(int nProtectionCount)
	{
		for (int i = 0; i < m_listCsMainGearRefinementRecipe.Count; i++)
		{
			if (m_listCsMainGearRefinementRecipe[i].ProtectionCount == nProtectionCount)
				return m_listCsMainGearRefinementRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearGrade GetSubGearGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsSubGearGrade.Count; i++)
		{
			if (m_listCsSubGearGrade[i].Grade == nGrade)
				return m_listCsSubGearGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearGrade GetSubGearGrade(EnSubGearGrade enSubGearGrade)
	{
		for (int i = 0; i < m_listCsSubGearGrade.Count; i++)
		{
			if (m_listCsSubGearGrade[i].EnGrade == enSubGearGrade)
				return m_listCsSubGearGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGear GetSubGear(int nSubGearId)
	{
		for (int i = 0; i < m_listCsSubGear.Count; i++)
		{
			if (m_listCsSubGear[i].SubGearId == nSubGearId)
				return m_listCsSubGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsLocation GetLocation(int nLocationId)
	{
		for (int i = 0; i < m_listCsLocation.Count; i++)
		{
			if (m_listCsLocation[i].LocationId == nLocationId)
				return m_listCsLocation[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterCharacter GetMonsterCharacter(int nMonsterCharacterId)
	{
		for (int i = 0; i < m_listCsMonsterCharacter.Count; i++)
		{
			if (m_listCsMonsterCharacter[i].MonsterCharacterId == nMonsterCharacterId)
				return m_listCsMonsterCharacter[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterInfo GetMonsterInfo(int nMonsterId)
	{
		for (int i = 0; i < m_listCsMonsterInfo.Count; i++)
		{
			if (m_listCsMonsterInfo[i].MonsterId == nMonsterId)
				return m_listCsMonsterInfo[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinent GetContinent(int nContinentId)
	{
		for (int i = 0; i < m_listCsContinent.Count; i++)
		{
			if (m_listCsContinent[i].ContinentId == nContinentId)
				return m_listCsContinent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinent GetContinentByLocationId(int nLocationId)
	{
		for (int i = 0; i < m_listCsContinent.Count; i++)
		{
			if (m_listCsContinent[i].LocationId == nLocationId)
				return m_listCsContinent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantStep GetMainGearEnchantStep(int nStep)
	{
		for (int i = 0; i < m_listCsMainGearEnchantStep.Count; i++)
		{
			if (m_listCsMainGearEnchantStep[i].Step == nStep)
				return m_listCsMainGearEnchantStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantLevel GetMainGearEnchantLevel(int nEnchantLevel)
	{
		for (int i = 0; i < m_listCsMainGearEnchantLevel.Count; i++)
		{
			if (m_listCsMainGearEnchantLevel[i].EnchantLevel == nEnchantLevel)
				return m_listCsMainGearEnchantLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearOptionAttrGrade GetMainGearOptionAttrGrade(int nAttrGrade)
	{
		for (int i = 0; i < m_listCsMainGearOptionAttrGrade.Count; i++)
		{
			if (m_listCsMainGearOptionAttrGrade[i].AttrGrade == nAttrGrade)
				return m_listCsMainGearOptionAttrGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainMenu GetMainMenu(int nMainMenuId)
	{
		for (int i = 0; i < m_listCsMainMenu.Count; i++)
		{
			if (m_listCsMainMenu[i].MenuId == nMainMenuId)
				return m_listCsMainMenu[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkillMaster GetJobSkillMaster(int m_SkillId)
	{
		for (int i = 0; i < m_listCsJobSkillMaster.Count; i++)
		{
			if (m_listCsJobSkillMaster[i].SkillId == m_SkillId)
				return m_listCsJobSkillMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobSkill GetJobSkill(int nJobId, int nSkillId)
	{
		for (int i = 0; i < m_listCsJobSkill.Count; i++)
		{
			if (m_listCsJobSkill[i].JobId == nJobId && m_listCsJobSkill[i].SkillId == nSkillId)
			{
				return m_listCsJobSkill[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemMainCategory GetItemMainCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsItemMainCategory.Count; i++)
		{
			if (m_listCsItemMainCategory[i].MainCategoryId == nCategoryId)
				return m_listCsItemMainCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsPortal GetPortal(int nPortalId)
	{
		for (int i = 0; i < m_listCsPortal.Count; i++)
		{
			if (m_listCsPortal[i].PortalId == nPortalId)
				return m_listCsPortal[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsPortal> GetPortalList(int nContinentId)
	{
		return m_listCsPortal.FindAll(a => a.ContinentId == nContinentId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcInfo GetNpcInfo(int nNpcId)
	{
		for (int i = 0; i < m_listCsNpcInfo.Count; i++)
		{
			if (m_listCsNpcInfo[i].NpcId == nNpcId)
				return m_listCsNpcInfo[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentObject GetContinentObject(int nObjectId)
	{
		for (int i = 0; i < m_listCsContinentObject.Count; i++)
		{
			if (m_listCsContinentObject[i].ObjectId == nObjectId)
				return m_listCsContinentObject[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuest GetMainQuest(int nMainQuestNo)
	{
		for (int i = 0; i < m_listCsMainQuest.Count; i++)
		{
			if (m_listCsMainQuest[i].GetMainQuestNo() == nMainQuestNo)
				return m_listCsMainQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsPaidImmediateRevival GetPaidImmdiateRevival(int nRevivalCount)
	{
		for (int i = 0; i < m_listCsPaidImmediateRevival.Count; i++)
		{
			if (m_listCsPaidImmediateRevival[i].RevivalCount == nRevivalCount)
				return m_listCsPaidImmediateRevival[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterSkill GetMonsterSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsMonsterSkill.Count; i++)
		{
			if (m_listCsMonsterSkill[i].SkillId == nSkillId)
				return m_listCsMonsterSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAbnormalState GetAbnormalState(int nAbnormalStateId)
	{
		for (int i = 0; i < m_listCsAbnormalState.Count; i++)
		{
			if (m_listCsAbnormalState[i].AbnormalStateId == nAbnormalStateId)
				return m_listCsAbnormalState[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobLevelMaster GetJobLevelMaster(int nLevel)
	{
		for (int i = 0; i < m_listCsJobLevelMaster.Count; i++)
		{
			if (m_listCsJobLevelMaster[i].Level == nLevel)
				return m_listCsJobLevelMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterArrange GetMonsterArrange(long lMonsterArrangeId)
	{
		if (m_dicCsMonsterArrange.ContainsKey(lMonsterArrangeId))
			return m_dicCsMonsterArrange[lMonsterArrangeId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleShopProduct GetSimpleShopProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsSimpleShopProduct.Count; i++)
		{
			if (m_listCsSimpleShopProduct[i].ProductId == nProductId)
				return m_listCsSimpleShopProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsSimpleShopProduct> GetSimpleShopProductList(int nItemId)
	{
		return m_listCsSimpleShopProduct.FindAll(a => a.Item.ItemId == nItemId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsVipLevel GetVipLevel(int nVipLevel)
	{
		for (int i = 0; i < m_listCsVipLevel.Count; i++)
		{
			if (m_listCsVipLevel[i].VipLevel == nVipLevel)
				return m_listCsVipLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsVipLevel GetVipLevelByAccVipPoint(int nAccVipPoint)
	{
		for (int i = 0; i < m_listCsVipLevel.Count; i++)
		{
			if (m_listCsVipLevel[i].RequiredAccVipPoint > nAccVipPoint)
			{
				return m_listCsVipLevel[i - 1];
			}
		}

		return m_listCsVipLevel[m_listCsVipLevel.Count - 1];
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlotExtendRecipe GetInventorySlotExtendRecipe(int nSlotCount)
	{
		for (int i = 0; i < m_listCsInventorySlotExtendRecipe.Count; i++)
		{
			if (m_listCsInventorySlotExtendRecipe[i].SlotCount == nSlotCount)
				return m_listCsInventorySlotExtendRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsMainGearDisassembleAvailableResultEntry> GetMainGearDisassembleAvailableResultEntryList(int nTier, int nGrade)
	{
		return m_listCsMainGearDisassembleAvailableResultEntry.FindAll(a => a.Tier == nTier && a.Grade == nGrade);
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemCompositionRecipe GetItemCompositionRecipe(int nMaterialItemId)
	{
		for (int i = 0; i < m_listCsItemCompositionRecipe.Count; i++)
		{
			if (m_listCsItemCompositionRecipe[i].MaterialItemId == nMaterialItemId)
				return m_listCsItemCompositionRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRestRewardTime GetRestRewardTime(int nRestTime)
	{
		if (nRestTime > m_listCsRestRewardTime[m_listCsRestRewardTime.Count - 1].RestTime)
		{
			return m_listCsRestRewardTime[m_listCsRestRewardTime.Count - 1];
		}

		for (int i = 0; i < m_listCsRestRewardTime.Count; i++)
		{
			if (m_listCsRestRewardTime[i].RestTime == nRestTime)
				return m_listCsRestRewardTime[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsChattingType GetChattingType(EnChattingType enChattingType)
	{
		for (int i = 0; i < m_listCsChattingType.Count; i++)
		{
			if (m_listCsChattingType[i].ChattingType == enChattingType)
				return m_listCsChattingType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeon GetMainQuestDungeon(int nDungeonId)
	{
		for (int i = 0; i < m_listCsMainQuestDungeon.Count; i++)
		{
			if (m_listCsMainQuestDungeon[i].DungeonId == nDungeonId)
				return m_listCsMainQuestDungeon[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsLevelUpRewardEntry GetLevelUpRewardEntry(int m_nEntryId)
	{
		for (int i = 0; i < m_listCsLevelUpRewardEntry.Count; i++)
		{
			if (m_listCsLevelUpRewardEntry[i].EntryId == m_nEntryId)
				return m_listCsLevelUpRewardEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyAttendRewardEntry GetDailyAttendRewardEntry(int nDay)
	{
		for (int i = 0; i < m_listCsDailyAttendRewardEntry.Count; i++)
		{
			if (m_listCsDailyAttendRewardEntry[i].Day == nDay)
				return m_listCsDailyAttendRewardEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeekendAttendRewardAvailableDayOfWeek GetWeekendAttendRewardAvailableDayOfWeek(int nDayOfWeek)
	{
		for (int i = 0; i < m_listCsWeekendAttendRewardAvailableDayOfWeek.Count; i++)
		{
			if (m_listCsWeekendAttendRewardAvailableDayOfWeek[i].DayOfWeek == nDayOfWeek)
				return m_listCsWeekendAttendRewardAvailableDayOfWeek[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccessRewardEntry GetAccessRewardEntry(int nEntryId)
	{
		for (int i = 0; i < m_listCsAccessRewardEntry.Count; i++)
		{
			if (m_listCsAccessRewardEntry[i].EntryId == nEntryId)
				return m_listCsAccessRewardEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountQualityMaster GetMountQualityMaster(int nQuality)
	{
		for (int i = 0; i < m_listCsMountQualityMaster.Count; i++)
		{
			if (m_listCsMountQualityMaster[i].Quality == nQuality)
				return m_listCsMountQualityMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountLevelMaster GetMountLevelMaster(int nLevel)
	{
		for (int i = 0; i < m_listCsMountLevelMaster.Count; i++)
		{
			if (m_listCsMountLevelMaster[i].Level == nLevel)
				return m_listCsMountLevelMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMount GetMount(int nMountId)
	{
		for (int i = 0; i < m_listMount.Count; i++)
		{
			if (m_listMount[i].MountId == nMountId)
				return m_listMount[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearType GetMountGearType(int nType)
	{
		for (int i = 0; i < m_listCsMountGearType.Count; i++)
		{
			if (m_listCsMountGearType[i].Type == nType)
				return m_listCsMountGearType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearType GetMountGearTypeBySlotIndex(int nSlotIndex)
	{
		for (int i = 0; i < m_listCsMountGearType.Count; i++)
		{
			if (m_listCsMountGearType[i].SlotIndex == nSlotIndex)
				return m_listCsMountGearType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearGrade GetMountGearGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsMountGearGrade.Count; i++)
		{
			if (m_listCsMountGearGrade[i].Grade == nGrade)
				return m_listCsMountGearGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearQuality GetMountGearQuality(int nQuality)
	{
		for (int i = 0; i < m_listCsMountGearQuality.Count; i++)
		{
			if (m_listCsMountGearQuality[i].Quality == nQuality)
				return m_listCsMountGearQuality[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGear GetMountGear(int nMountGearId)
	{
		for (int i = 0; i < m_listCsMountGear.Count; i++)
		{
			if (m_listCsMountGear[i].MountGearId == nMountGearId)
				return m_listCsMountGear[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearOptionAttrGrade GetMountGearOptionAttrGrade(int nAttrGrade)
	{
		for (int i = 0; i < m_listCsMountGearOptionAttrGrade.Count; i++)
		{
			if (m_listCsMountGearOptionAttrGrade[i].AttrGrade == nAttrGrade)
				return m_listCsMountGearOptionAttrGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearPickBoxRecipe GetMountGearPickBoxRecipe(int nItemId)
	{
		for (int i = 0; i < m_listCsMountGearPickBoxRecipe.Count; i++)
		{
			if (m_listCsMountGearPickBoxRecipe[i].Item.ItemId == nItemId)
				return m_listCsMountGearPickBoxRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeon GetStoryDungeon(int nDungeonNo)
	{
		for (int i = 0; i < m_listCsStoryDungeon.Count; i++)
		{
			if (m_listCsStoryDungeon[i].DungeonNo == nDungeonNo)
				return m_listCsStoryDungeon[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingStep GetWingStep(int nStep)
	{
		for (int i = 0; i < m_listCsWingStep.Count; i++)
		{
			if (m_listCsWingStep[i].Step == nStep)
				return m_listCsWingStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingPart GetWingPart(int nPartId)
	{
		for (int i = 0; i < m_listCsWingPart.Count; i++)
		{
			if (m_listCsWingPart[i].PartId == nPartId)
				return m_listCsWingPart[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWing GetWing(int nWingId)
	{
		for (int i = 0; i < m_listCsWing.Count; i++)
		{
			if (m_listCsWing[i].WingId == nWingId)
				return m_listCsWing[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsStaminaBuyCount GetStaminaBuyCount(int nBuyCount)
	{
		for (int i = 0; i < m_listCsStaminaBuyCount.Count; i++)
		{
			if (m_listCsStaminaBuyCount[i].BuyCount == nBuyCount)
				return m_listCsStaminaBuyCount[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemGrade GetItemGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsItemGrade.Count; i++)
		{
			if (m_listCsItemGrade[i].Grade == nGrade)
				return m_listCsItemGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantLevelSet GetMainGearEnchantLevelSet(int nSetNo)
	{
		for (int i = 0; i < m_listCsMainGearEnchantLevelSet.Count; i++)
		{
			if (m_listCsMainGearEnchantLevelSet[i].SetNo == nSetNo)
				return m_listCsMainGearEnchantLevelSet[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantLevelSet GetMainGearEnchantLevelSetByEnchantLevel(int nEnchantLevel)
	{
		if (m_listCsMainGearEnchantLevelSet[0].RequiredTotalEnchantLevel > nEnchantLevel)
			return null;

		for (int i = 1; i < m_listCsMainGearEnchantLevelSet.Count; i++)
		{
			if (m_listCsMainGearEnchantLevelSet[i].RequiredTotalEnchantLevel > nEnchantLevel)
				return m_listCsMainGearEnchantLevelSet[i - 1];
		}

		return m_listCsMainGearEnchantLevelSet[m_listCsMainGearEnchantLevelSet.Count - 1];
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearSet GetMainGearSet(int nTier, int nGrade, int nQuality)
	{
		for (int i = 0; i < m_listCsMainGearSet.Count; i++)
		{
			if (m_listCsMainGearSet[i].Tier == nTier && m_listCsMainGearSet[i].Grade == nGrade && m_listCsMainGearSet[i].Quality == nQuality)
				return m_listCsMainGearSet[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneLevelSet GetSubGearSoulstoneLevelSet(int nSetNo)
	{
		for (int i = 0; i < m_listCsSubGearSoulstoneLevelSet.Count; i++)
		{
			if (m_listCsSubGearSoulstoneLevelSet[i].SetNo == nSetNo)
				return m_listCsSubGearSoulstoneLevelSet[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneLevelSet GetSubGearSoulstoneLevelSetByLevel(int nTotalLevel)
	{
		if (m_listCsSubGearSoulstoneLevelSet[0].RequiredTotalLevel > nTotalLevel)
			return null;

		for (int i = 1; i < m_listCsSubGearSoulstoneLevelSet.Count; i++)
		{
			if (m_listCsSubGearSoulstoneLevelSet[i].RequiredTotalLevel > nTotalLevel)
				return m_listCsSubGearSoulstoneLevelSet[i - 1];
		}

		return m_listCsSubGearSoulstoneLevelSet[m_listCsSubGearSoulstoneLevelSet.Count - 1];
	}

	//---------------------------------------------------------------------------------------------------
	public CsCartGrade GetCartGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsCartGrade.Count; i++)
		{
			if (m_listCsCartGrade[i].Grade == nGrade)
				return m_listCsCartGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCart GetCart(int nCartId)
	{
		for (int i = 0; i < m_listCsCart.Count; i++)
		{
			if (m_listCsCart[i].CartId == nCartId)
				return m_listCsCart[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWorldLevelExpFactor GetWorldLevelExpFactor(int nLevelGap)
	{
		for (int i = 0; i < m_listCsWorldLevelExpFactor.Count; i++)
		{
			if (m_listCsWorldLevelExpFactor[i].LevelGap == nLevelGap)
				return m_listCsWorldLevelExpFactor[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBountyHunterQuest GetBountyHunterQuest(int nQuestId)
	{
		for (int i = 0; i < m_listCsBountyHunterQuest.Count; i++)
		{
			if (m_listCsBountyHunterQuest[i].QuestId == nQuestId)
				return m_listCsBountyHunterQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBountyHunterQuestReward GetBountyHunterQuestReward(int nQuestItemGrade, int nLevel)
	{
		for (int i = 0; i < m_listCsBountyHunterQuestReward.Count; i++)
		{
			if (m_listCsBountyHunterQuestReward[i].QuestItemGrade == nQuestItemGrade && m_listCsBountyHunterQuestReward[i].Level == nLevel)
				return m_listCsBountyHunterQuestReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMysteryBoxGrade GetMysteryBoxGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsMysteryBoxGrade.Count; i++)
		{
			if (m_listCsMysteryBoxGrade[i].Grade == nGrade)
				return m_listCsMysteryBoxGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSecretLetterGrade GetSecretLetterGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsSecretLetterGrade.Count; i++)
		{
			if (m_listCsSecretLetterGrade[i].Grade == nGrade)
				return m_listCsSecretLetterGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsExploitPointReward GetExploitPointReward(long lExploitPointRewardId)
	{
		if (m_dicCsExploitPointReward.ContainsKey(lExploitPointRewardId))
			return m_dicCsExploitPointReward[lExploitPointRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayMission GetTodayMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsTodayMission.Count; i++)
		{
			if (m_listCsTodayMission[i].MissionId == nMissionId)
				return m_listCsTodayMission[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSeriesMission GetSeriesMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsSeriesMission.Count; i++)
		{
			if (m_listCsSeriesMission[i].MissionId == nMissionId)
				return m_listCsSeriesMission[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayTaskCategory GetTodayTaskCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsTodayTaskCategory.Count; i++)
		{
			if (m_listCsTodayTaskCategory[i].CategoryId == nCategoryId)
				return m_listCsTodayTaskCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayTask GetTodayTask(int nTaskId)
	{
		for (int i = 0; i < m_listCsTodayTask.Count; i++)
		{
			if (m_listCsTodayTask[i].TaskId == nTaskId)
				return m_listCsTodayTask[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAchievementReward GetAchievementReward(int nRewardNo)
	{
		for (int i = 0; i < m_listCsAchievementReward.Count; i++)
		{
			if (m_listCsAchievementReward[i].RewardNo == nRewardNo)
				return m_listCsAchievementReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHonorPointReward GetHonorPointReward(long lHonorPointRewardId)
	{
		if (m_dicCsHonorPointReward.ContainsKey(lHonorPointRewardId))
			return m_dicCsHonorPointReward[lHonorPointRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHonorShopProduct GetHonorShopProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsHonorShopProduct.Count; i++)
		{
			if (m_listCsHonorShopProduct[i].ProductId == nProductId)
				return m_listCsHonorShopProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRank GetRank(int nRankNo)
	{
		for (int i = 0; i < m_listCsRank.Count; i++)
		{
			if (m_listCsRank[i].RankNo == nRankNo)
				return m_listCsRank[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttainmentEntry GetAttainmentEntry(int nEntryNo)
	{
		for (int i = 0; i < m_listCsAttainmentEntry.Count; i++)
		{
			if (m_listCsAttainmentEntry[i].EntryNo == nEntryNo)
				return m_listCsAttainmentEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenu GetMenu(int nMenuId)
	{
		for (int i = 0; i < m_listCsMenu.Count; i++)
		{
			if (m_listCsMenu[i].MenuId == nMenuId)
				return m_listCsMenu[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsMenu> GetMenuListByMenuGroup(int nMenuGroup)
	{
		return m_listCsMenu.FindAll(a => a.MenuGroup == nMenuGroup);
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenuContent GetMenuContent(int nContentId)
	{
		for (int i = 0; i < m_listCsMenuContent.Count; i++)
		{
			if (m_listCsMenuContent[i].ContentId == nContentId)
				return m_listCsMenuContent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsMenuContent> GetMenuContentListByMenu(int nMenuId)
	{
		return m_listCsMenuContent.FindAll(a => a.Menu.MenuId == nMenuId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildLevel GetGuildLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsGuildLevel.Count; i++)
		{
			if (m_listCsGuildLevel[i].Level == nLevel)
				return m_listCsGuildLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMemberGrade GetGuildMemberGrade(int nMemberGrade)
	{
		for (int i = 0; i < m_listCsGuildMemberGrade.Count; i++)
		{
			if (m_listCsGuildMemberGrade[i].MemberGrade == nMemberGrade)
				return m_listCsGuildMemberGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildContributionPointReward GetGuildContributionPointReward(long lGuildContributionPointRewardId)
	{
		if (m_dicCsGuildContributionPointReward.ContainsKey(lGuildContributionPointRewardId))
			return m_dicCsGuildContributionPointReward[lGuildContributionPointRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuildingPointReward GetGuildBuildingPointReward(long lGuildBuildingPointRewardId)
	{
		if (m_dicCsGuildBuildingPointReward.ContainsKey(lGuildBuildingPointRewardId))
			return m_dicCsGuildBuildingPointReward[lGuildBuildingPointRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildFundReward GetGuildFundReward(long lGuildFundRewardId)
	{
		if (m_dicCsGuildFundReward.ContainsKey(lGuildFundRewardId))
			return m_dicCsGuildFundReward[lGuildFundRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildPointReward GetGuildPointReward(long lGuildPointReward)
	{
		if (m_dicCsGuildPointReward.ContainsKey(lGuildPointReward))
			return m_dicCsGuildPointReward[lGuildPointReward];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildDonationEntry GetGuildDonationEntry(int nEntryId)
	{
		for (int i = 0; i < m_listCsGuildDonationEntry.Count; i++)
		{
			if (m_listCsGuildDonationEntry[i].EntryId == nEntryId)
				return m_listCsGuildDonationEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationFundReward GetNationFundReward(long lNationFundRewardId)
	{
		if (m_dicCsNationFundReward.ContainsKey(lNationFundRewardId))
			return m_dicCsNationFundReward[lNationFundRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationDonationEntry GetNationDonationEntry(int nEntryId)
	{
		for (int i = 0; i < m_listCsNationDonationEntry.Count; i++)
		{
			if (m_listCsNationDonationEntry[i].EntryId == nEntryId)
				return m_listCsNationDonationEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesse GetNationNoblesse(int nNoblesseId)
	{
		for (int i = 0; i < m_listCsNationNoblesse.Count; i++)
		{
			if (m_listCsNationNoblesse[i].NoblesseId == nNoblesseId)
				return m_listCsNationNoblesse[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuilding GetGuildBuilding(int nBuildingId)
	{
		for (int i = 0; i < m_listCsGuildBuilding.Count; i++)
		{
			if (m_listCsGuildBuilding[i].BuildingId == nBuildingId)
				return m_listCsGuildBuilding[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkill GetGuildSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsGuildSkill.Count; i++)
		{
			if (m_listCsGuildSkill[i].GuildSkillId == nSkillId)
				return m_listCsGuildSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarRevivalPoint GetNationWarRevivalPoint(int nRevivalPointId)
	{
		for (int i = 0; i < m_listCsNationWarRevivalPoint.Count; i++)
		{
			if (m_listCsNationWarRevivalPoint[i].RevivalPointId == nRevivalPointId)
				return m_listCsNationWarRevivalPoint[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsClientTutorialStep> GetClientTutorialStepList(int nTutorialId)
	{
		return m_listCsClientTutorialStep.FindAll(a => a.TutorialId == nTutorialId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsOwnDiaReward GetOwnDiaReward(long lOwnDiaRewardId)
	{
		if (m_dicCsOwnDiaReward.ContainsKey(lOwnDiaRewardId))
			return m_dicCsOwnDiaReward[lOwnDiaRewardId];
		else
			return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildContent GetGuildContent(int nGuildContentId)
	{
		for (int i = 0; i < m_listCsGuildContent.Count; i++)
		{
			if (m_listCsGuildContent[i].GuildContentId == nGuildContentId)
				return m_listCsGuildContent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildDailyObjectiveReward GetGuildDailyObjectiveReward(int nRewardNo)
	{
		for (int i = 0; i < m_listCsGuildDailyObjectiveReward.Count; i++)
		{
			if (m_listCsGuildDailyObjectiveReward[i].RewardNo == nRewardNo)
				return m_listCsGuildDailyObjectiveReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildWeeklyObjective GetGuildWeeklyObjective(int nObjectiveId)
	{
		for (int i = 0; i < m_listCsGuildWeeklyObjective.Count; i++)
		{
			if (m_listCsGuildWeeklyObjective[i].ObjectiveId == nObjectiveId)
				return m_listCsGuildWeeklyObjective[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookCategory GetIllustratedBookCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsIllustratedBookCategory.Count; i++)
		{
			if (m_listCsIllustratedBookCategory[i].CategoryId == nCategoryId)
				return m_listCsIllustratedBookCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookType GetIllustratedBookType(int nType)
	{
		for (int i = 0; i < m_listCsIllustratedBookType.Count; i++)
		{
			if (m_listCsIllustratedBookType[i].Type == nType)
				return m_listCsIllustratedBookType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookAttrGrade GetIllustratedBookAttrGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsIllustratedBookAttrGrade.Count; i++)
		{
			if (m_listCsIllustratedBookAttrGrade[i].Grade == nGrade)
				return m_listCsIllustratedBookAttrGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBook GetIllustratedBook(int nIllustratedBookId)
	{
		for (int i = 0; i < m_listCsIllustratedBook.Count; i++)
		{
			if (m_listCsIllustratedBook[i].IllustratedBookId == nIllustratedBookId)
				return m_listCsIllustratedBook[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookExplorationStep GetIllustratedBookExplorationStep(int nStepNo)
	{
		for (int i = 0; i < m_listCsIllustratedBookExplorationStep.Count; i++)
		{
			if (m_listCsIllustratedBookExplorationStep[i].StepNo == nStepNo)
				return m_listCsIllustratedBookExplorationStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSceneryQuest GetSceneryQuest(int nQuestId)
	{
		for (int i = 0; i < m_listCsSceneryQuest.Count; i++)
		{
			if (m_listCsSceneryQuest[i].QuestId == nQuestId)
				return m_listCsSceneryQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishmentCategory GetAccomplishmentCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsAccomplishmentCategory.Count; i++)
		{
			if (m_listCsAccomplishmentCategory[i].CategoryId == nCategoryId)
				return m_listCsAccomplishmentCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishment GetAccomplishment(int nAccomplishmentId)
	{
		for (int i = 0; i < m_listCsAccomplishment.Count; i++)
		{
			if (m_listCsAccomplishment[i].AccomplishmentId == nAccomplishmentId)
				return m_listCsAccomplishment[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitleCategory GetTitleCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsTitleCategory.Count; i++)
		{
			if (m_listCsTitleCategory[i].CategoryId == nCategoryId)
				return m_listCsTitleCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitleType GetTitleType(int nType)
	{
		for (int i = 0; i < m_listCsTitleType.Count; i++)
		{
			if (m_listCsTitleType[i].Type == nType)
				return m_listCsTitleType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitleGrade GetTitleGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsTitleGrade.Count; i++)
		{
			if (m_listCsTitleGrade[i].Grade == nGrade)
				return m_listCsTitleGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitle GetTitle(int nTitleId)
	{
		for (int i = 0; i < m_listCsTitle.Count; i++)
		{
			if (m_listCsTitle[i].TitleId == nTitleId)
				return m_listCsTitle[i];
		}

		return null;

	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterCategory GetEliteMonsterCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsEliteMonsterCategory.Count; i++)
		{
			if (m_listCsEliteMonsterCategory[i].CategoryId == nCategoryId)
				return m_listCsEliteMonsterCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonsterMaster GetEliteMonsterMaster(int nEliteMonsterMasterId)
	{
		for (int i = 0; i < m_listCsEliteMonsterMaster.Count; i++)
		{
			if (m_listCsEliteMonsterMaster[i].EliteMonsterMasterId == nEliteMonsterMasterId)
				return m_listCsEliteMonsterMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsEliteMonster GetEliteMonster(int nEliteMonsterId)
	{
		for (int i = 0; i < m_listCsEliteMonster.Count; i++)
		{
			if (m_listCsEliteMonster[i].EliteMonsterId == nEliteMonsterId)
				return m_listCsEliteMonster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCategory GetCreatureCardCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsCreatureCardCategory.Count; i++)
		{
			if (m_listCsCreatureCardCategory[i].CategoryId == nCategoryId)
				return m_listCsCreatureCardCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardGrade GetCreatureCardGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsCreatureCardGrade.Count; i++)
		{
			if (m_listCsCreatureCardGrade[i].Grade == nGrade)
				return m_listCsCreatureCardGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCard GetCreatureCard(int nCreatureCardId)
	{
		for (int i = 0; i < m_listCsCreatureCard.Count; i++)
		{
			if (m_listCsCreatureCard[i].CreatureCardId == nCreatureCardId)
				return m_listCsCreatureCard[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollectionCategory GetCreatureCardCollectionCategory(int nCatergoryId)
	{
		for (int i = 0; i < m_listCsCreatureCardCollectionCategory.Count; i++)
		{
			if (m_listCsCreatureCardCollectionCategory[i].CategoryId == nCatergoryId)
				return m_listCsCreatureCardCollectionCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollectionGrade GetCreatureCardCollectionGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsCreatureCardCollectionGrade.Count; i++)
		{
			if (m_listCsCreatureCardCollectionGrade[i].Grade == nGrade)
				return m_listCsCreatureCardCollectionGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollection GetCreatureCardCollection(int nCollectionId)
	{
		for (int i = 0; i < m_listCsCreatureCardCollection.Count; i++)
		{
			if (m_listCsCreatureCardCollection[i].CollectionId == nCollectionId)
				return m_listCsCreatureCardCollection[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopRefreshSchedule GetCreatureCardShopRefreshSchedule(int nScheduleId)
	{
		for (int i = 0; i < m_listCsCreatureCardShopRefreshSchedule.Count; i++)
		{
			if (m_listCsCreatureCardShopRefreshSchedule[i].ScheduleId == nScheduleId)
				return m_listCsCreatureCardShopRefreshSchedule[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopFixedProduct GetCreatureCardShopFixedProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsCreatureCardShopFixedProduct.Count; i++)
		{
			if (m_listCsCreatureCardShopFixedProduct[i].ProductId == nProductId)
				return m_listCsCreatureCardShopFixedProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopRandomProduct GetCreatureCardShopRandomProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsCreatureCardShopRandomProduct.Count; i++)
		{
			if (m_listCsCreatureCardShopRandomProduct[i].ProductId == nProductId)
				return m_listCsCreatureCardShopRandomProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsStaminaRecoverySchedule GetStaminaRecoverySchedule(int nScheduleId)
	{
		for (int i = 0; i < m_listCsStaminaRecoverySchedule.Count; i++)
		{
			if (m_listCsStaminaRecoverySchedule[i].ScheduleId == nScheduleId)
				return m_listCsStaminaRecoverySchedule[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsCreatureCardCollectionEntry> GetCreatureCardCollectionEntryListByCollection(int nCollectionId)
	{
		return m_listCsCreatureCardCollectionEntry.FindAll(a => a.CreatureCardCollection.CollectionId == nCollectionId);
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsCreatureCardCollectionEntry> GetCreatureCardCollectionEntryListByCreatureCard(int nCreatureCardId)
	{
		return m_listCsCreatureCardCollectionEntry.FindAll(a => a.CreatureCard.CreatureCardId == nCreatureCardId);
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsBanWord> GetBanWordList(int nType)
	{
		return m_listCsBanWord.FindAll(a => a.Type == nType);
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenuContentOpenPreview GetMenuContentOpenPreview(int nPreviewNo)
	{
		for (int i = 0; i < m_listCsMenuContentOpenPreview.Count; i++)
		{
			if (m_listCsMenuContentOpenPreview[i].PreviewNo == nPreviewNo)
				return m_listCsMenuContentOpenPreview[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobCommonSkill GetJobCommonSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsJobCommonSkill.Count; i++)
		{
			if (m_listCsJobCommonSkill[i].SkillId == nSkillId)
				return m_listCsJobCommonSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShop GetNpcShop(int nShopId)
	{
		for (int i = 0; i < m_listCsNpcShop.Count; i++)
		{
			if (m_listCsNpcShop[i].ShopId == nShopId)
				return m_listCsNpcShop[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShop GetNpcShopByNpcId(int nNpcId)
	{
		for (int i = 0; i < m_listCsNpcShop.Count; i++)
		{
			if (m_listCsNpcShop[i].NpcId == nNpcId)
				return m_listCsNpcShop[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankActiveSkill GetRankActiveSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsRankActiveSkill.Count; i++)
		{
			if (m_listCsRankActiveSkill[i].SkillId == nSkillId)
				return m_listCsRankActiveSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankPassiveSkill GetRankPassiveSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsRankPassiveSkill.Count; i++)
		{
			if (m_listCsRankPassiveSkill[i].SkillId == nSkillId)
				return m_listCsRankPassiveSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRookieGift GetRookieGift(int nGiftNo)
	{
		for (int i = 0; i < m_listCsRookieGift.Count; i++)
		{
			if (m_listCsRookieGift[i].GiftNo == nGiftNo)
				return m_listCsRookieGift[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpenGiftReward GetOpenGiftReward(int nDay)
	{
		for (int i = 0; i < m_listCsOpenGiftReward.Count; i++)
		{
			if (m_listCsOpenGiftReward[i].Day == nDay)
				return m_listCsOpenGiftReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsQuickMenu GetQuickMenu(int nMenuId)
	{
		for (int i = 0; i < m_listCsQuickMenu.Count; i++)
		{
			if (m_listCsQuickMenu[i].MenuId == nMenuId)
				return m_listCsQuickMenu[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyQuestGrade GetDailyQuestGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsDailyQuestGrade.Count; i++)
		{
			if (m_listCsDailyQuestGrade[i].Grade == nGrade)
				return m_listCsDailyQuestGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventDay GetOpen7DayEventDay(int nDay)
	{
		for (int i = 0; i < m_listCsOpen7DayEventDay.Count; i++)
		{
			if (m_listCsOpen7DayEventDay[i].Day == nDay)
				return m_listCsOpen7DayEventDay[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventMission GetOpen7DayEventMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsOpen7DayEventDay.Count; i++)
		{
			for (int j = 0; j < m_listCsOpen7DayEventDay[i].Open7DayEventMissionList.Count; j++)
			{
				if (m_listCsOpen7DayEventDay[i].Open7DayEventMissionList[j].MissionId == nMissionId)
					return m_listCsOpen7DayEventDay[i].Open7DayEventMissionList[j];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRetrieval GetRetrieval(int nRetrievalId)
	{
		for (int i = 0; i < m_listCsRetrieval.Count; i++)
		{
			if (m_listCsRetrieval[i].RetrievalId == nRetrievalId)
				return m_listCsRetrieval[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTaskConsignment GetTaskConsignment(int nConsignmentId)
	{
		for (int i = 0; i < m_listCsTaskConsignment.Count; i++)
		{
			if (m_listCsTaskConsignment[i].ConsignmentId == nConsignmentId)
				return m_listCsTaskConsignment[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseSlotExtendRecipe GetWarehouseSlotExtendRecipe(int nSlotCount)
	{
		for (int i = 0; i < m_listCsWarehouseSlotExtendRecipe.Count; i++)
		{
			if (m_listCsWarehouseSlotExtendRecipe[i].SlotCount == nSlotCount)
				return m_listCsWarehouseSlotExtendRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidomElemental GetFearAltarHalidomElemental(int nHalidomElementalId)
	{
		for (int i = 0; i < m_listCsFearAltarHalidomElemental.Count; i++)
		{
			if (m_listCsFearAltarHalidomElemental[i].HalidomElementalId == nHalidomElementalId)
				return m_listCsFearAltarHalidomElemental[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidomLevel GetFearAltarHalidomLevel(int nHalidomLevel)
	{
		for (int i = 0; i < m_listCsFearAltarHalidomLevel.Count; i++)
		{
			if (m_listCsFearAltarHalidomLevel[i].HalidomLevel == nHalidomLevel)
				return m_listCsFearAltarHalidomLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDiaShopCategory GetDiaShopCategory(int nCategoryId)
	{
		for (int i = 0; i < m_listCsDiaShopCategory.Count; i++)
		{
			if (m_listCsDiaShopCategory[i].CategoryId == nCategoryId)
				return m_listCsDiaShopCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDiaShopProduct GetDiaShopProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsDiaShopProduct.Count; i++)
		{
			if (m_listCsDiaShopProduct[i].ProductId == nProductId)
				return m_listCsDiaShopProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsDiaShopProduct> GetDiaShopProductList(int nCategoryId)
	{
		List<CsDiaShopProduct> list = m_listCsDiaShopProduct.FindAll(a => a.DiaShopCategory.CategoryId == nCategoryId);

		list.Sort((CsDiaShopProduct product1, CsDiaShopProduct product2) =>
		{
			return product1.CategorySortNo.CompareTo(product2.CategorySortNo);
		});

		return list;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsDiaShopProduct> GetDiaShopProductListByLimitEdition()
	{
		List<CsDiaShopProduct> list = m_listCsDiaShopProduct.FindAll(a => a.IsLimitEdition == true);

		list.Sort((CsDiaShopProduct product1, CsDiaShopProduct product2) =>
		{
			return product1.LimitEditionSortNo.CompareTo(product2.LimitEditionSortNo);
		});

		return list;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingMemoryPieceType GetWingMemoryPieceType(int nType)
	{
		for (int i = 0; i < m_listCsWingMemoryPieceType.Count; i++)
		{
			if (m_listCsWingMemoryPieceType[i].Type == nType)
				return m_listCsWingMemoryPieceType[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubQuest GetSubQuest(int nQuestId)
	{
		for (int i = 0; i < m_listCsSubQuest.Count; i++)
		{
			if (m_listCsSubQuest[i].QuestId == nQuestId)
				return m_listCsSubQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsOrdealQuest GetOrdealQuest(int nQuestNo)
	{
		for (int i = 0; i < m_listCsOrdealQuest.Count; i++)
		{
			if (m_listCsOrdealQuest[i].QuestNo == nQuestNo)
				return m_listCsOrdealQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMoneyBuff GetMoneyBuff(int nBuffId)
	{
		for (int i = 0; i < m_listCsMoneyBuff.Count; i++)
		{
			if (m_listCsMoneyBuff[i].BuffId == nBuffId)
				return m_listCsMoneyBuff[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiography GetBiography(int nBiographyId)
	{
		for (int i = 0; i < m_listCsBiography.Count; i++)
		{
			if (m_listCsBiography[i].BiographyId == nBiographyId)
				return m_listCsBiography[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuestDungeon GetBiographyQuestDungeon(int nDungeonId)
	{
		for (int i = 0; i < m_listCsBiographyQuestDungeon.Count; i++)
		{
			if (m_listCsBiographyQuestDungeon[i].DungeonId == nDungeonId)
				return m_listCsBiographyQuestDungeon[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlessing GetBlessing(int nBlessingId)
	{
		for (int i = 0; i < m_listCsBlessing.Count; i++)
		{
			if (m_listCsBlessing[i].BlessingId == nBlessingId)
				return m_listCsBlessing[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlessingTargetLevel GetBlessingTargetLevel(int nTargetLevelId)
	{
		for (int i = 0; i < m_listCsBlessingTargetLevel.Count; i++)
		{
			if (m_listCsBlessingTargetLevel[i].TargetLevelId == nTargetLevelId)
				return m_listCsBlessingTargetLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureGrade GetCreatureGrade(int nGrade)
	{
		for (int i = 0; i < m_listCsCreatureGrade.Count; i++)
		{
			if (m_listCsCreatureGrade[i].Grade == nGrade)
				return m_listCsCreatureGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCharacter GetCreatureCharacter(int nCreatureCharacterId)
	{
		for (int i = 0; i < m_listCsCreatureCharacter.Count; i++)
		{
			if (m_listCsCreatureCharacter[i].CreatureCharacterId == nCreatureCharacterId)
				return m_listCsCreatureCharacter[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillSlotOpenRecipe GetCreatureSkillSlotOpenRecipe(int nSlotCount)
	{
		for (int i = 0; i < m_listCsCreatureSkillSlotOpenRecipe.Count; i++)
		{
			if (m_listCsCreatureSkillSlotOpenRecipe[i].SlotCount == nSlotCount)
				return m_listCsCreatureSkillSlotOpenRecipe[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillSlotProtection GetCreatureSkillSlotProtection(int nProtectionCount)
	{
		for (int i = 0; i < m_listCsCreatureSkillSlotProtection.Count; i++)
		{
			if (m_listCsCreatureSkillSlotProtection[i].ProtectionCount == nProtectionCount)
				return m_listCsCreatureSkillSlotProtection[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreature GetCreature(int nCreatureId)
	{
		for (int i = 0; i < m_listCsCreature.Count; i++)
		{
			if (m_listCsCreature[i].CreatureId == nCreatureId)
				return m_listCsCreature[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillGrade GetCreatureSkillGrade(int nSkillGrade)
	{
		for (int i = 0; i < m_listCsCreatureSkillGrade.Count; i++)
		{
			if (m_listCsCreatureSkillGrade[i].SkillGrade == nSkillGrade)
				return m_listCsCreatureSkillGrade[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkill GetCreatureSkill(int nSkillId)
	{
		for (int i = 0; i < m_listCsCreatureSkill.Count; i++)
		{
			if (m_listCsCreatureSkill[i].SkillId == nSkillId)
				return m_listCsCreatureSkill[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureBaseAttr GetCreatureBaseAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsCreatureBaseAttr.Count; i++)
		{
			if (m_listCsCreatureBaseAttr[i].Attr.AttrId == nAttrId)
				return m_listCsCreatureBaseAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureLevel GetCreatureLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsCreatureLevel.Count; i++)
		{
			if (m_listCsCreatureLevel[i].Level == nLevel)
				return m_listCsCreatureLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureAdditionalAttr GetCreatureAdditionalAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsCreatureAdditionalAttr.Count; i++)
		{
			if (m_listCsCreatureAdditionalAttr[i].Attr.AttrId == nAttrId)
				return m_listCsCreatureAdditionalAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureInjectionLevel GetCreatureInjectionLevel(int nInjectionLevel)
	{
		for (int i = 0; i < m_listCsCreatureInjectionLevel.Count; i++)
		{
			if (m_listCsCreatureInjectionLevel[i].InjectionLevel == nInjectionLevel)
				return m_listCsCreatureInjectionLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsPresent GetPresent(int nPresendId)
	{
		for (int i = 0; i < m_listCsPresent.Count; i++)
		{
			if (m_listCsPresent[i].PresentId == nPresendId)
				return m_listCsPresent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyPresentContributionPointRankingRewardGroup GetWeeklyPresentContributionPointRankingRewardGroup(int nGroupNo)
	{
		for (int i = 0; i < m_listCsWeeklyPresentContributionPointRankingRewardGroup.Count; i++)
		{
			if (m_listCsWeeklyPresentContributionPointRankingRewardGroup[i].GroupNo == nGroupNo)
				return m_listCsWeeklyPresentContributionPointRankingRewardGroup[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyPresentPopularityPointRankingRewardGroup GetWeeklyPresentPopularityPointRankingRewardGroup(int nGroupNo)
	{
		for (int i = 0; i < m_listCsWeeklyPresentPopularityPointRankingRewardGroup.Count; i++)
		{
			if (m_listCsWeeklyPresentPopularityPointRankingRewardGroup[i].GroupNo == nGroupNo)
				return m_listCsWeeklyPresentPopularityPointRankingRewardGroup[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostume GetCostume(int nCostumeId)
	{
		for (int i = 0; i < m_listCsCostume.Count; i++)
		{
			if (m_listCsCostume[i].CostumeId == nCostumeId)
				return m_listCsCostume[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeEffect GetCostumeEffect(int nCostumeEffectId)
	{
		for (int i = 0; i < m_listCsCostumeEffect.Count; i++)
		{
			if (m_listCsCostumeEffect[i].CostumeEffectId == nCostumeEffectId)
				return m_listCsCostumeEffect[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBlessingBuff GetGuildBlessingBuff(int nBuffId)
	{
		for (int i = 0; i < m_listCsGuildBlessingBuff.Count; i++)
		{
			if (m_listCsGuildBlessingBuff[i].BuffId == nBuffId)
				return m_listCsGuildBlessingBuff[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobChangeQuest GetJobChangeQuest(int nQuestNo)
	{
		for (int i = 0; i < m_listCsJobChangeQuest.Count; i++)
		{
			if (m_listCsJobChangeQuest[i].QuestNo == nQuestNo)
				return m_listCsJobChangeQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRecommendBattlePowerLevel GetRecommendBattlePowerLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsRecommendBattlePowerLevel.Count; i++)
		{
			if (m_listCsRecommendBattlePowerLevel[i].Level == nLevel)
				return m_listCsRecommendBattlePowerLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContent GetImprovementContent(int nContentId)
	{
		for (int i = 0; i < m_listCsImprovementContent.Count; i++)
		{
			if (m_listCsImprovementContent[i].ContentId == nContentId)
				return m_listCsImprovementContent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContentAchievement GetImprovementContentAchievement(int nAchievement)
	{
		for (int i = 0; i < m_listCsImprovementContentAchievement.Count; i++)
		{
			if (m_listCsImprovementContentAchievement[i].Achievement == nAchievement)
				return m_listCsImprovementContentAchievement[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementMainCategory GetImprovementMainCategory(int nMainCategoryId)
	{
		for (int i = 0; i < m_listCsImprovementMainCategory.Count; i++)
		{
			if (m_listCsImprovementMainCategory[i].MainCategoryId == nMainCategoryId)
				return m_listCsImprovementMainCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsPotionAttr GetPotionAttr(int nPotionAttrId)
	{
		for (int i = 0; i < m_listCsPotionAttr.Count; i++)
		{
			if (m_listCsPotionAttr[i].PotionAttrId == nPotionAttrId)
				return m_listCsPotionAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInAppProduct GetInAppProduct(string strInAppProductKey)
	{
		for (int i = 0; i < m_listCsInAppProduct.Count; i++)
		{
			if (m_listCsInAppProduct[i].InAppProductKey == strInAppProductKey)
				return m_listCsInAppProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCashProduct GetCashProduct(int nProductId)
	{
		for (int i = 0; i < m_listCsCashProduct.Count; i++)
		{
			if (m_listCsCashProduct[i].ProductId == nProductId)
				return m_listCsCashProduct[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsChargeEvent GetChargeEvent(int nEventId)
	{
		for (int i = 0; i < m_listCsChargeEvent.Count; i++)
		{
			if (m_listCsChargeEvent[i].EventId == nEventId)
				return m_listCsChargeEvent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsChargeEvent GetCurrentChargeEvent()
	{
		return m_listCsChargeEvent.Find(chargeEvent => chargeEvent.StartTime <= m_csMyHeroInfo.CurrentDateTime &&
														m_csMyHeroInfo.CurrentDateTime < chargeEvent.EndTime);
	}

	//---------------------------------------------------------------------------------------------------
	public CsConsumeEvent GetConsumeEvent(int nEventId)
	{
		for (int i = 0; i < m_listCsConsumeEvent.Count; i++)
		{
			if (m_listCsConsumeEvent[i].EventId == nEventId)
				return m_listCsConsumeEvent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsConsumeEvent GetCurrentConsumeEvent()
	{
		return m_listCsConsumeEvent.Find(consumeEvent => consumeEvent.StartTime <= m_csMyHeroInfo.CurrentDateTime &&
														m_csMyHeroInfo.CurrentDateTime < consumeEvent.EndTime);
	}

	//---------------------------------------------------------------------------------------------------
	public CsConstellation GetConstellation(int nConstellationId)
	{
		for (int i = 0; i < m_listCsConstellation.Count; i++)
		{
			if (m_listCsConstellation[i].ConstellationId == nConstellationId)
				return m_listCsConstellation[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifact GetArtifact(int nArtifactNo)
	{
		for (int i = 0; i < m_listCsArtifact.Count; i++)
		{
			if (m_listCsArtifact[i].ArtifactNo == nArtifactNo)
				return m_listCsArtifact[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactLevelUpMaterial GetArtifactLevelUpMaterial(int nTier, int nGrade)
	{
		for (int i = 0; i < m_listCsArtifactLevelUpMaterial.Count; i++)
		{
			if (m_listCsArtifactLevelUpMaterial[i].Tier == nTier && m_listCsArtifactLevelUpMaterial[i].Grade == nGrade)
				return m_listCsArtifactLevelUpMaterial[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountAwakeningLevelMaster GetMountAwakeningLevelMaster(int nAwakeningLevel)
	{
		for (int i = 0; i < m_listCsMountAwakeningLevelMaster.Count; i++)
		{
			if (m_listCsMountAwakeningLevelMaster[i].AwakeningLevel == nAwakeningLevel)
				return m_listCsMountAwakeningLevelMaster[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountPotionAttrCount GetMountPotionAttrCount(int nCount, int nAttrId)
	{
		for (int i = 0; i < m_listCsMountPotionAttrCount.Count; i++)
		{
			if (m_listCsMountPotionAttrCount[i].Count == nCount && m_listCsMountPotionAttrCount[i].Attr.AttrId == nAttrId)
				return m_listCsMountPotionAttrCount[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsMountPotionAttrCount> GetMountPotionAttrCountList(int nCount)
	{
		return m_listCsMountPotionAttrCount.FindAll(a => a.Count == nCount);
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeCollection GetCostumeCollection(int nCostumeCollectionId)
	{
		for (int i = 0; i < m_listCsCostumeCollection.Count; i++)
		{
			if (m_listCsCostumeCollection[i].CostumeCollectionId == nCostumeCollectionId)
				return m_listCsCostumeCollection[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeEnchantLevel GetCostumeEnchantLevel(int nEnchantLevel)
	{
		for (int i = 0; i < m_listCsCostumeEnchantLevel.Count; i++)
		{
			if (m_listCsCostumeEnchantLevel[i].EnchantLevel == nEnchantLevel)
				return m_listCsCostumeEnchantLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsCostumeEnchantLevelAttr> GetCostumeEnchantLevelAttrList(int nCostumeId, int nEnchantLevel)
	{
		return m_listCsCostumeEnchantLevelAttr.FindAll(a => a.CostumeId == nCostumeId && a.EnchantLevel == nEnchantLevel);
	}

	//---------------------------------------------------------------------------------------------------
	public CsScheduleNotice GetScheduleNotice(int nNoticeId)
	{
		for (int i = 0; i < m_listCsScheduleNotice.Count; i++)
		{
			if (m_listCsScheduleNotice[i].NoticeId == nNoticeId)
				return m_listCsScheduleNotice[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSharingEvent GetSharingEvent(int nEventId)
	{
		for (int i = 0; i < m_listCsSharingEvent.Count; i++)
		{
			if (m_listCsSharingEvent[i].EventId == nEventId)
				return m_listCsSharingEvent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSharingEvent GetCurrentSharingEvent()
	{
		for (int i = 0; i < m_listCsSharingEvent.Count; i++)
		{
			if (m_listCsSharingEvent[i].TargetLevel <= CsGameData.Instance.MyHeroInfo.Level &&
				m_listCsSharingEvent[i].StartTime.DateTime <= CsGameData.Instance.MyHeroInfo.CurrentDateTime &&
				CsGameData.Instance.MyHeroInfo.CurrentDateTime < m_listCsSharingEvent[i].EndTime.DateTime)
				return m_listCsSharingEvent[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageInfo GetSystemMessageInfo(int nMessageId)
	{
		for (int i = 0; i < m_listCsSystemMessageInfo.Count; i++)
		{
			if (m_listCsSystemMessageInfo[i].MessageId == nMessageId)
				return m_listCsSystemMessageInfo[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradition GetTradition(int nTradition)
	{
		for (int i = 0; i < m_listCsTradition.Count; i++)
		{
			if (m_listCsTradition[i].Tradition == nTradition)
				return m_listCsTradition[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishmentLevel GetAccomplishmentLevel(int nAccomplishmentLevel)
	{
		for (int i = 0; i < m_listCsAccomplishmentLevel.Count; i++)
		{
			if (m_listCsAccomplishmentLevel[i].AccomplishmentLevel == nAccomplishmentLevel)
				return m_listCsAccomplishmentLevel[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishmentPointReward GetAccomplishmentPointReward(long lAccomplishmentPointRewardId)
	{
		if (m_dicCsAccomplishmentPointReward.ContainsKey(lAccomplishmentPointRewardId))
			return m_dicCsAccomplishmentPointReward[lAccomplishmentPointRewardId];
		else
			return null;
	}



}

