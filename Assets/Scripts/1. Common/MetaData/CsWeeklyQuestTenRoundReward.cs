using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyQuestTenRoundReward
{
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestTenRoundReward(WPDWeeklyQuestTenRoundReward weeklyQuestTenRoundReward)
	{
		m_nRewardNo = weeklyQuestTenRoundReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(weeklyQuestTenRoundReward.itemRewardId);
	}
}
