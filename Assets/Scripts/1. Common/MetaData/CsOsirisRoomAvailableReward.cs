using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsOsirisRoomAvailableReward
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
	public CsOsirisRoomAvailableReward(WPDOsirisRoomAvailableReward osirisRoomAvailableReward)
	{
		m_nRewardNo = osirisRoomAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(osirisRoomAvailableReward.itemId);
	}
}
