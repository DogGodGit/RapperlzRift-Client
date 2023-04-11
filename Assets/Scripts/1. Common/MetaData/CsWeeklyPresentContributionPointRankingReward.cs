using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyPresentContributionPointRankingReward
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
	public CsWeeklyPresentContributionPointRankingReward(WPDWeeklyPresentContributionPointRankingReward weeklyPresentContributionPointRankingReward)
	{
		m_nGroupNo = weeklyPresentContributionPointRankingReward.groupNo;
		m_nRewardNo = weeklyPresentContributionPointRankingReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(weeklyPresentContributionPointRankingReward.itemRewardId);
	}
}
