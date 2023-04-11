using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimRandomRewardPoolEntry
{
	int m_nEntryNo;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryNo
	{
		get { return m_nEntryNo; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimRandomRewardPoolEntry(WPDRuinsReclaimRandomRewardPoolEntry ruinsReclaimRandomRewardPoolEntry)
	{
		m_nEntryNo = ruinsReclaimRandomRewardPoolEntry.entryNo;
		m_csItemReward = CsGameData.Instance.GetItemReward(ruinsReclaimRandomRewardPoolEntry.itemRewardId);
	}
}
