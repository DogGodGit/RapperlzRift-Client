using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAchievementRewardEntry
{
	int m_nRewardNo;
	int m_nRewardEntryNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int RewardEntryNo
	{
		get { return m_nRewardEntryNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAchievementRewardEntry(WPDAchievementRewardEntry achievementRewardEntry)
	{
		m_nRewardNo = achievementRewardEntry.rewardNo;
		m_nRewardEntryNo = achievementRewardEntry.rewardEntryNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(achievementRewardEntry.itemRewardId);
	}
}
