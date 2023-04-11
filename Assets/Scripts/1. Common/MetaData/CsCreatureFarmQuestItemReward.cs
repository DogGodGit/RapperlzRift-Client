using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsCreatureFarmQuestItemReward
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
	public CsCreatureFarmQuestItemReward(WPDCreatureFarmQuestItemReward creatureFarmQuestItemReward)
	{
		m_nRewardNo = creatureFarmQuestItemReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(creatureFarmQuestItemReward.itemRewardId);
	}
}
