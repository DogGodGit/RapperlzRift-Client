using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimStepReward
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
	public CsRuinsReclaimStepReward(WPDRuinsReclaimStepReward ruinsReclaimStepReward)
	{
		m_nStepNo = ruinsReclaimStepReward.stepNo;
		m_nRewardNo = ruinsReclaimStepReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(ruinsReclaimStepReward.itemRewardId);
	}
}
