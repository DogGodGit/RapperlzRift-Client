using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNestStepReward
{
	int m_nStepNo;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int StepNo
	{
		get { return m_nStepNo; }
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
	public CsDragonNestStepReward(WPDDragonNestStepReward dragonNestStepReward)
	{
		m_nStepNo = dragonNestStepReward.stepNo;
		m_nRewardNo = dragonNestStepReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(dragonNestStepReward.itemRewardId);
	}
}
