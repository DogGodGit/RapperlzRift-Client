using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipDifficulty
{
	int m_nDifficulty;
	long m_lRecommendBattlePower;
	int m_nMinHeroLevel;
	int m_nMaxHeroLevel;
	CsGoldReward m_csGoldReward;
	CsExpReward m_csExpReward;
	CsGoldReward m_csGoldRewardPoint;
	CsExpReward m_csExpRewardPoint;
	long m_lMaxAdditionalExp;

	List<CsTradeShipAvailableReward> m_listCsTradeShipAvailableReward;
	List<CsTradeShipObject> m_listCsTradeShipObject;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public long RecommendBattlePower
	{
		get { return m_lRecommendBattlePower; }
	}

	public int MinHeroLevel
	{
		get { return m_nMinHeroLevel; }
	}

	public int MaxHeroLevel
	{
		get { return m_nMaxHeroLevel; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public CsGoldReward PointGoldReward
	{
		get { return m_csGoldRewardPoint; }
	}

	public CsExpReward PointExpReward
	{
		get { return m_csExpRewardPoint; }
	}

	public long MaxAdditionalExp
	{
		get { return m_lMaxAdditionalExp; }
	}

	public List<CsTradeShipAvailableReward> TradeShipAvailableRewardList
	{
		get { return m_listCsTradeShipAvailableReward; }
	}

	public List<CsTradeShipObject> TradeShipObjectList
	{
		get { return m_listCsTradeShipObject; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipDifficulty(WPDTradeShipDifficulty tradeShipDifficulty)
	{
		m_nDifficulty = tradeShipDifficulty.difficulty;
		m_lRecommendBattlePower = tradeShipDifficulty.recommendBattlePower;
		m_nMinHeroLevel = tradeShipDifficulty.minHeroLevel;
		m_nMaxHeroLevel = tradeShipDifficulty.maxHeroLevel;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(tradeShipDifficulty.goldRewardId);
		m_csExpReward = CsGameData.Instance.GetExpReward(tradeShipDifficulty.expRewardId);
		m_csGoldRewardPoint = CsGameData.Instance.GetGoldReward(tradeShipDifficulty.pointGoldRewardId);
		m_csExpRewardPoint = CsGameData.Instance.GetExpReward(tradeShipDifficulty.pointExpRewardId);
		m_lMaxAdditionalExp = tradeShipDifficulty.maxAdditionalExp;

		m_listCsTradeShipAvailableReward = new List<CsTradeShipAvailableReward>();
		m_listCsTradeShipObject = new List<CsTradeShipObject>(); ;
	}
}
