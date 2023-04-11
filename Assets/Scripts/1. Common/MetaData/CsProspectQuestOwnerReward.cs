using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsProspectQuestOwnerReward
{
	int m_nTargetLevelId;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int TargetLevelId
	{
		get { return m_nTargetLevelId; }
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
	public CsProspectQuestOwnerReward(WPDProspectQuestOwnerReward prospectQuestOwnerReward)
	{
		m_nTargetLevelId = prospectQuestOwnerReward.targetLevelId;
		m_nRewardNo = prospectQuestOwnerReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(prospectQuestOwnerReward.itemRewardId);
	}
}
