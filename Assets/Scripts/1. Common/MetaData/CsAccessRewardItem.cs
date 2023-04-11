using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsAccessRewardItem
{
	int m_nEntryId;
	int m_nRewardNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryId
	{
		get { return m_nEntryId; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccessRewardItem(WPDAccessRewardItem accessRewardItem)
	{
		m_nEntryId = accessRewardItem.entryId;
		m_nRewardNo = accessRewardItem.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(accessRewardItem.itemRewardId);
	}
}
