using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryRankingReward
{
	int m_lHighRanking;
	int m_lLowRanking;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int HighRanking
	{
		get { return m_lHighRanking; }
	}

	public int LowRanking
	{
		get { return m_lLowRanking; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryRankingReward(WPDWarMemoryRankingReward warMemoryRankingReward)
	{
		m_lHighRanking = warMemoryRankingReward.highRanking;
		m_lLowRanking = warMemoryRankingReward.lowRanking;
		m_nRewardNo = warMemoryRankingReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(warMemoryRankingReward.itemRewardId);
	}
}
