using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-15)
//---------------------------------------------------------------------------------------------------

public class CsMainGearDisassembleAvailableResultEntry
{
	int m_nTier;		
	int m_nGrade;
	int m_nEntryNo;
	CsItem m_csItem;
	int m_nItemCount;
	bool m_bItemOwned;

	//---------------------------------------------------------------------------------------------------
	public int Tier
	{
		get { return m_nTier; }
	}

	public int Grade
	{
		get { return m_nGrade; }
	}

	public int EntryNo
	{
		get { return m_nEntryNo; }
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
	public CsMainGearDisassembleAvailableResultEntry(WPDMainGearDisassembleAvailableResultEntry mainGearDisassembleAvailableResultEntry)
	{
		m_nTier = mainGearDisassembleAvailableResultEntry.tier;
		m_nGrade = mainGearDisassembleAvailableResultEntry.grade;
		m_nEntryNo = mainGearDisassembleAvailableResultEntry.entryNo;
		m_csItem = CsGameData.Instance.GetItem(mainGearDisassembleAvailableResultEntry.itemId);
		m_nItemCount = mainGearDisassembleAvailableResultEntry.itemCount;
		m_bItemOwned = mainGearDisassembleAvailableResultEntry.itemOwned;
	}
}
