using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsLevelUpRewardItem
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
	public CsLevelUpRewardItem(WPDLevelUpRewardItem levelUpRewardItem)
	{
		m_nEntryId = levelUpRewardItem.entryId;
		m_nRewardNo = levelUpRewardItem.rewardNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(levelUpRewardItem.itemRewardId);
	}
}
