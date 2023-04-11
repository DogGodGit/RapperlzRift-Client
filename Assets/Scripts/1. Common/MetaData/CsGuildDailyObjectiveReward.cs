using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildDailyObjectiveReward
{
	int m_nRewardNo;
	int m_nCompletionMemberCount;
	CsItemReward m_csItemReward1;
	CsItemReward m_csItemReward2;
	CsItemReward m_csItemReward3;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int CompletionMemberCount
	{
		get { return m_nCompletionMemberCount; }
	}

	public CsItemReward ItemReward1
	{
		get { return m_csItemReward1; }
	}

	public CsItemReward ItemReward2
	{
		get { return m_csItemReward2; }
	}

	public CsItemReward ItemReward3
	{
		get { return m_csItemReward3; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildDailyObjectiveReward(WPDGuildDailyObjectiveReward guildDailyObjectiveReward)
	{
		m_nRewardNo = guildDailyObjectiveReward.rewardNo;
		m_nCompletionMemberCount = guildDailyObjectiveReward.completionMemberCount;
		m_csItemReward1 = CsGameData.Instance.GetItemReward(guildDailyObjectiveReward.itemReward1Id);
		m_csItemReward2 = CsGameData.Instance.GetItemReward(guildDailyObjectiveReward.itemReward2Id);
		m_csItemReward3 = CsGameData.Instance.GetItemReward(guildDailyObjectiveReward.itemReward3Id);
	}
}
