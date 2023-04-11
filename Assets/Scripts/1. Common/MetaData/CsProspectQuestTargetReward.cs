using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsProspectQuestTargetReward
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
	public CsProspectQuestTargetReward(WPDProspectQuestTargetReward prospectQuestTargetReward)
	{
		m_nTargetLevelId = prospectQuestTargetReward.targetLevelId;
		m_nRewardNo = prospectQuestTargetReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(prospectQuestTargetReward.itemRewardId);
	}
}
