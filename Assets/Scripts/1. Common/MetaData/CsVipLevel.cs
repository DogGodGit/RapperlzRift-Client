using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsVipLevel
{
	int m_nVipLevel;                        // VIP레벨
	string m_strDescription;                // 설명키
	int m_nRequiredAccVipPoint;				// 필요누적VIP포인트
	int m_nMainGearEnchantMaxCount;         // 메인장비강화횟수
	int m_nMainGearRefinementMaxCount;      // 메인장비세련횟수	
	int m_nMountGearRefinementMaxCount;     // 탈것장비재강화횟수
	int m_nExpPotionUseMaxCount;            // 경험치물약사용횟수 
	int m_nStaminaBuyMaxCount;              // 체력구입횟수
	int m_nExpDungeonEnterCount;            // 경험치던전입장횟수	
	int m_nGoldDungeonEnterCount;           // 골드던전입장횟수
	int m_nOsirisRoomEnterCount;            // 오시리스의방입장횟수
	int m_nExpScrollUseMaxCount;            // 경험치스크롤사용횟수	
	int m_nDailyMaxExploitPoint;            // 일일공적최대치
	int m_nArtifactRoomInitMaxCount;        // 고대유물의방초기화최대횟수
	int m_nAncientRelicEnterCount;          // 고대인의유적입장횟수
	int m_nFieldOfHonorEnterCount;          // 결투장입장횟수
	int m_nDistortionScrollUseMaxCount;		// 
	int m_nGuildDonationMaxCount;           // 길드기부최대횟수
	int m_nNationDonationMaxCount;          // 국가기부최대횟수 
	int m_nSoulCoveterWeeklyEnterCount;     // 영혼을탐하는자주간입장횟수
	bool m_bCreatureCardCompositionEnabled;
	int m_nCreatureCardShopPaidRefreshMaxCount;
	int m_nProofOfValorEnterCount;
	int m_nTrueHeroQuestStepNo;
	int m_nFearAltarEnterCount;
	float m_flExpDungeonAdditionalExpRewardFactor;
	int m_nLuckyShopPickMaxCount;
	int m_nCreatureVariationMaxCount;
	int m_nTradeShipEnterCount;

	List<CsVipLevelReward> m_listCsVipLevelReward;

	//---------------------------------------------------------------------------------------------------
	public int VipLevel
	{
		get { return m_nVipLevel; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredAccVipPoint
	{
		get { return m_nRequiredAccVipPoint; }
	}

	public int MainGearEnchantMaxCount
	{
		get { return m_nMainGearEnchantMaxCount; }
	}

	public int MainGearRefinementMaxCount
	{
		get { return m_nMainGearRefinementMaxCount; }
	}

	public int MountGearRefinementMaxCount
	{
		get { return m_nMountGearRefinementMaxCount; }
	}

	public int ExpPotionUseMaxCount
	{
		get { return m_nExpPotionUseMaxCount; }
	}

	public int StaminaBuyMaxCount
	{
		get { return m_nStaminaBuyMaxCount; }
	}

	public int ExpDungeonEnterCount
	{
		get { return m_nExpDungeonEnterCount; }
	}

	public int GoldDungeonEnterCount
	{
		get { return m_nGoldDungeonEnterCount; }
	}

	public int OsirisRoomEnterCount
	{
		get { return m_nOsirisRoomEnterCount; }
	}

	public int ExpScrollUseMaxCount
	{
		get { return m_nExpScrollUseMaxCount; }
	}

	public int DailyMaxExploitPoint
	{
		get { return m_nDailyMaxExploitPoint; }
	}

	public List<CsVipLevelReward> VipLevelRewardList
	{
		get { return m_listCsVipLevelReward; }
	}

    public int AncientRelicEnterCount
    {
        get { return m_nAncientRelicEnterCount; }
    }

    public int ArtifactRoomInitMaxCount
    {
        get { return m_nArtifactRoomInitMaxCount; }
    }

	public int FieldOfHonorEnterCount
	{
		get { return m_nFieldOfHonorEnterCount; }
	}

	public int DistortionScrollUseMaxCount
	{
		get { return m_nDistortionScrollUseMaxCount; }
	}

	public int GuildDonationMaxCount
	{
		get { return m_nGuildDonationMaxCount; }
	}

	public int NationDonationMaxCount
	{
		get { return m_nNationDonationMaxCount; }
	}

	public int SoulCoveterWeeklyEnterCount
	{
		get { return m_nSoulCoveterWeeklyEnterCount; }
	}

	public bool CreatureCardCompositionEnabled
	{
		get { return m_bCreatureCardCompositionEnabled; }
	}

	public int CreatureCardShopPaidRefreshMaxCount
	{
		get { return m_nCreatureCardShopPaidRefreshMaxCount; }
	}

	public int ProofOfValorEnterCount
	{
		get { return m_nProofOfValorEnterCount; }
	}

	public int TrueHeroQuestStepNo
	{
		get { return m_nTrueHeroQuestStepNo; }
	}

	public int FearAltarEnterCount
	{
		get { return m_nFearAltarEnterCount; }
	}

	public float ExpDungeonAdditionalExpRewardFactor
	{
		get { return m_flExpDungeonAdditionalExpRewardFactor; }
	}

	public int LuckyShopPickMaxCount
	{
		get { return m_nLuckyShopPickMaxCount; }
	}

	public int CreatureVariationMaxCount
	{
		get { return m_nCreatureVariationMaxCount; }
	}

	public int TradeShipEnterCount
	{
		get { return m_nTradeShipEnterCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsVipLevel(WPDVipLevel vipLevel)
	{
		m_nVipLevel = vipLevel.vipLevel;
		m_strDescription = CsConfiguration.Instance.GetString(vipLevel.descriptionKey);
		m_nRequiredAccVipPoint = vipLevel.requiredAccVipPoint;
		m_nMainGearEnchantMaxCount = vipLevel.mainGearEnchantMaxCount;
		m_nMainGearRefinementMaxCount = vipLevel.mainGearRefinementMaxCount;
		m_nMountGearRefinementMaxCount = vipLevel.mountGearRefinementMaxCount;
		m_nExpPotionUseMaxCount = vipLevel.expPotionUseMaxCount;
		m_nStaminaBuyMaxCount = vipLevel.staminaBuyMaxCount;
		m_nExpDungeonEnterCount = vipLevel.expDungeonEnterCount;
		m_nGoldDungeonEnterCount = vipLevel.goldDungeonEnterCount;
		m_nOsirisRoomEnterCount = vipLevel.osirisRoomEnterCount;
		m_nExpScrollUseMaxCount = vipLevel.expScrollUseMaxCount;
		m_nDailyMaxExploitPoint = vipLevel.dailyMaxExploitPoint;
        m_nAncientRelicEnterCount = vipLevel.ancientRelicEnterCount;
        m_nArtifactRoomInitMaxCount = vipLevel.artifactRoomInitMaxCount;
		m_nFieldOfHonorEnterCount = vipLevel.fieldOfHonorEnterCount;
		m_nDistortionScrollUseMaxCount = vipLevel.distortionScrollUseMaxCount;
		m_nGuildDonationMaxCount = vipLevel.guildDonationMaxCount;
		m_nNationDonationMaxCount = vipLevel.nationDonationmaxCount;
		m_nSoulCoveterWeeklyEnterCount = vipLevel.soulCoveterWeeklyEnterCount;
		m_bCreatureCardCompositionEnabled = vipLevel.creatureCardCompositionEnabled;
		m_nCreatureCardShopPaidRefreshMaxCount = vipLevel.creatureCardShopPaidRefreshMaxCount;
		m_nProofOfValorEnterCount = vipLevel.proofOfValorEnterCount;
		m_nTrueHeroQuestStepNo = vipLevel.trueHeroQuestStepNo;
		m_nFearAltarEnterCount = vipLevel.fearAltarEnterCount;
		m_flExpDungeonAdditionalExpRewardFactor = vipLevel.expDungeonAdditionalExpRewardFactor;
		m_nLuckyShopPickMaxCount = vipLevel.luckyShopPickMaxCount;
		m_nCreatureVariationMaxCount = vipLevel.creatureVariationMaxCount;
		m_nTradeShipEnterCount = vipLevel.tradeShipEnterCount;

		m_listCsVipLevelReward = new List<CsVipLevelReward>();
	}
}
