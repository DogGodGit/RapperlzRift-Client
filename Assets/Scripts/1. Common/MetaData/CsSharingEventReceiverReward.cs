using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-10)
//---------------------------------------------------------------------------------------------------


public class CsSharingEventReceiverReward
{
	int m_nEventId;
	int m_nRewardNo;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nItemCount;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int ItemCount
	{
		get { return m_nItemCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSharingEventReceiverReward(WPDSharingEventReceiverReward sharingEventReceiverReward)
	{
		m_nEventId = sharingEventReceiverReward.eventId;
		m_nRewardNo = sharingEventReceiverReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(sharingEventReceiverReward.itemId);
		m_bItemOwned = sharingEventReceiverReward.itemOwned;
		m_nItemCount = sharingEventReceiverReward.itemCount;
	}
}
