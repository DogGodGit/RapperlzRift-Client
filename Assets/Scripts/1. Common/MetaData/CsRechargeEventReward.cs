using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsRechargeEventReward
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
	public CsRechargeEventReward(WPDRechargeEventReward rechargeEventReward)
	{
		m_nRewardNo = rechargeEventReward.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(rechargeEventReward.itemRewardId);
	}
}
