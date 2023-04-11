using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-10)
//---------------------------------------------------------------------------------------------------

public class CsSharingEventSenderReward
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
	public CsSharingEventSenderReward(WPDSharingEventSenderReward sharingEventSenderReward)
	{
		m_nEventId = sharingEventSenderReward.eventId;
		m_nRewardNo = sharingEventSenderReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(sharingEventSenderReward.itemId);
		m_bItemOwned = sharingEventSenderReward.itemOwned;
		m_nItemCount = sharingEventSenderReward.itemCount;
	}
}
