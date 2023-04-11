using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelicAvailableReward
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
	public CsAncientRelicAvailableReward(WPDAncientRelicAvailableReward ancientRelicAvailableReward)
	{
		m_nRewardNo = ancientRelicAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(ancientRelicAvailableReward.itemId);
	}
}
