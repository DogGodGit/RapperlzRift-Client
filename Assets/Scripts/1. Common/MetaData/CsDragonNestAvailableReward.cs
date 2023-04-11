using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNestAvailableReward
{
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDragonNestAvailableReward(WPDDragonNestAvailableReward dragonNestAvailableReward)
	{
		m_nRewardNo = dragonNestAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(dragonNestAvailableReward.itemId);
	}
}
