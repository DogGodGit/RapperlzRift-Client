using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsAnkouTombDifficulty
{
	int m_nDifficulty;
	long m_nRecommendBattlePower;
	int m_nMinHeroLevel;
	int m_nMaxHeroLevel;
	CsGoldReward m_csGoldReward;
	CsExpReward m_csExpReward;
	CsGoldReward m_csGoldRewardPoint;
	CsExpReward m_csExpRewardPoint;
	long m_lMaxAdditionalExp;

	List<CsAnkouTombAvailableReward> m_listCsAnkouTombAvailableReward;

	//---------------------------------------------------------------------------------------------------
	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public long RecommendBattlePower
	{
		get { return m_nRecommendBattlePower; }
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

	public List<CsAnkouTombAvailableReward> AnkouTombAvailableRewardList
	{
		get { return m_listCsAnkouTombAvailableReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAnkouTombDifficulty(WPDAnkouTombDifficulty ankouTombDifficulty)
	{
		m_nDifficulty = ankouTombDifficulty.difficulty;
		m_nRecommendBattlePower = ankouTombDifficulty.recommendBattlePower;
		m_nMinHeroLevel = ankouTombDifficulty.minHeroLevel;
		m_nMaxHeroLevel = ankouTombDifficulty.maxHeroLevel;
		m_csGoldReward = CsGameData.Instance.GetGoldReward(ankouTombDifficulty.goldRewardId);
		m_csExpReward = CsGameData.Instance.GetExpReward(ankouTombDifficulty.expRewardId);
		m_csGoldRewardPoint = CsGameData.Instance.GetGoldReward(ankouTombDifficulty.pointGoldRewardId);
		m_csExpRewardPoint = CsGameData.Instance.GetExpReward(ankouTombDifficulty.pointExpRewardId);
		m_lMaxAdditionalExp = ankouTombDifficulty.maxAdditionalExp;

		m_listCsAnkouTombAvailableReward = new List<CsAnkouTombAvailableReward>();
	}
}
