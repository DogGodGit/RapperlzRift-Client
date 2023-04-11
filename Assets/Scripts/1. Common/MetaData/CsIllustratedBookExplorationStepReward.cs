using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookExplorationStepReward
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
	public CsIllustratedBookExplorationStepReward(WPDIllustratedBookExplorationStepReward illustratedBookExplorationStepReward)
	{
		m_nStepNo = illustratedBookExplorationStepReward.stepNo;
		m_nRewardNo = illustratedBookExplorationStepReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(illustratedBookExplorationStepReward.itemRewardId);
	}
}
