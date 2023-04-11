using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsFirstChargeEventReward
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
	public CsFirstChargeEventReward(WPDFirstChargeEventReward firstChargeEventReward)
	{
		m_nRewardNo = firstChargeEventReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(firstChargeEventReward.itemRewardId);
	}
}
