using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemoryAvailableReward
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
	public CsWarMemoryAvailableReward(WPDWarMemoryAvailableReward warMemoryAvailableReward)
	{
		m_nRewardNo = warMemoryAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(warMemoryAvailableReward.itemId);
	}
}
