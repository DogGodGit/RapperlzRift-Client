using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsLevelRankingReward
{
	int m_nHighRanking;
	int m_nLowRanking;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int HighRanking
	{
		get { return m_nHighRanking; }
	}

	public int LowRanking
	{
		get { return m_nLowRanking; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLevelRankingReward(WPDLevelRankingReward levelRankingReward)
	{
		m_nHighRanking = levelRankingReward.highRanking;
		m_nLowRanking = levelRankingReward.lowRanking;
		m_csItemReward = CsGameData.Instance.GetItemReward(levelRankingReward.itemRewardId);
	}
}
