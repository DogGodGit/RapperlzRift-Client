using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyPresentPopularityPointRankingReward
{
	int m_nGroupNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int GroupNo
	{
		get { return m_nGroupNo; }
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
	public CsWeeklyPresentPopularityPointRankingReward(WPDWeeklyPresentPopularityPointRankingReward weeklyPresentPopularityPointRankingReward)
	{
		m_nGroupNo = weeklyPresentPopularityPointRankingReward.groupNo;
		m_nRewardNo = weeklyPresentPopularityPointRankingReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(weeklyPresentPopularityPointRankingReward.itemRewardId);
	}
}
