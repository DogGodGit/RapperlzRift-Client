using System.Collections.Generic; 
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsAccessRewardEntry
{
	int m_nEntryId;
	int m_nAccessTime;

	List<CsAccessRewardItem> m_listCsAccessRewardItem;
	//---------------------------------------------------------------------------------------------------
	public int EntryId
	{
		get { return m_nEntryId; }
	}

	public int AccessTime
	{
		get { return m_nAccessTime; }
	}

	public List<CsAccessRewardItem> AccessRewardItemList
	{
		get { return m_listCsAccessRewardItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccessRewardEntry(WPDAccessRewardEntry accessRewardEntry)
	{
		m_nEntryId = accessRewardEntry.entryId;
		m_nAccessTime = accessRewardEntry.accessTime;

		m_listCsAccessRewardItem = new List<CsAccessRewardItem>();
	}
}
