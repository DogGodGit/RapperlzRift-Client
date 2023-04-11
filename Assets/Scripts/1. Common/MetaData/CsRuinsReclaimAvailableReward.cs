using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimAvailableReward
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
	public CsRuinsReclaimAvailableReward(WPDRuinsReclaimAvailableReward ruinsReclaimAvailableReward)
	{
		m_nRewardNo = ruinsReclaimAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(ruinsReclaimAvailableReward.itemId);
	}
}
