using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsItemReward
{
	long m_lItemRewardId;
	CsItem m_csItem;
	int m_nItemCount;
	bool m_bItemOwned;

	//---------------------------------------------------------------------------------------------------
	public long ItemRewardId
	{
		get { return m_lItemRewardId; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public int ItemCount
	{
		get { return m_nItemCount; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemReward(WPDItemReward itemReward)
	{
		m_lItemRewardId = itemReward.itemRewardId;
		m_csItem = CsGameData.Instance.GetItem(itemReward.itemId);
		m_nItemCount = itemReward.itemCount;
		m_bItemOwned = itemReward.itemOwned;
	}


}
