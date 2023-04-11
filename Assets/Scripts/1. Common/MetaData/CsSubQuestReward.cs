using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsSubQuestReward
{
	int m_nQuestId;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int QuestId
	{
		get { return m_nQuestId; }
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
	public CsSubQuestReward(WPDSubQuestReward subQuestReward)
	{
		m_nQuestId = subQuestReward.questId;
		m_nRewardNo = subQuestReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(subQuestReward.itemRewardId);
	}
}
