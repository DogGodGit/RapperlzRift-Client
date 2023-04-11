using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsLevelUpRewardEntry
{
	int m_nEntryId;
	int m_nLevel;

	List<CsLevelUpRewardItem> m_listCsLevelUpRewardItem;

	//---------------------------------------------------------------------------------------------------
	public int EntryId
	{
		get { return m_nEntryId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public List<CsLevelUpRewardItem> LevelUpRewardItemList
	{
		get { return m_listCsLevelUpRewardItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLevelUpRewardEntry(WPDLevelUpRewardEntry levelUpRewardEntry)
	{
		m_nEntryId = levelUpRewardEntry.entryId;
		m_nLevel = levelUpRewardEntry.level;

		m_listCsLevelUpRewardItem = new List<CsLevelUpRewardItem>();
	}
}
