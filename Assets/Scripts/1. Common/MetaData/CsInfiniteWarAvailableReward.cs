using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsInfiniteWarAvailableReward
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
	public CsInfiniteWarAvailableReward(WPDInfiniteWarAvailableReward infiniteWarAvailableReward)
	{
		m_nRewardNo = infiniteWarAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(infiniteWarAvailableReward.itemId);
	}
}
