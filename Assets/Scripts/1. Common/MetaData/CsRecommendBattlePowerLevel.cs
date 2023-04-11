using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsRecommendBattlePowerLevel
{
	int m_nLevel;
	long m_lSRankBattlePower;
	long m_lARankBattlePower;
	long m_lBRankBattlePower;
	long m_lCRankBattlePower;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public long SRankBattlePower
	{
		get { return m_lSRankBattlePower; }
	}

	public long ARankBattlePower
	{
		get { return m_lARankBattlePower; }
	}

	public long BRankBattlePower
	{
		get { return m_lBRankBattlePower; }
	}

	public long CRankBattlePower
	{
		get { return m_lCRankBattlePower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRecommendBattlePowerLevel(WPDRecommendBattlePowerLevel recommendBattlePowerLevel)
	{
		m_nLevel = recommendBattlePowerLevel.level;
		m_lSRankBattlePower = recommendBattlePowerLevel.sRankBattlePower;
		m_lARankBattlePower = recommendBattlePowerLevel.aRankBattlePower;
		m_lBRankBattlePower = recommendBattlePowerLevel.bRankBattlePower;
		m_lCRankBattlePower = recommendBattlePowerLevel.cRankBattlePower;
	}
}
