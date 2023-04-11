using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsAttainmentEntry
{
	int m_nEntryNo;
	string m_strName;
	string m_strDescription;
	int m_nType;                    // 1:레벨도달, 2:메인퀘스트도달
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;

	List<CsAttainmentEntryReward> m_listCsAttainmentEntryReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryNo
	{
		get { return m_nEntryNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public List<CsAttainmentEntryReward> AttainmentEntryRewardList
	{
		get { return m_listCsAttainmentEntryReward; }
	}
	//---------------------------------------------------------------------------------------------------
	public CsAttainmentEntry(WPDAttainmentEntry attainmentEntry)
	{
		m_nEntryNo = attainmentEntry.entryNo;
		m_strName = CsConfiguration.Instance.GetString(attainmentEntry.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(attainmentEntry.descriptionKey);
		m_nType = attainmentEntry.type;
		m_nRequiredHeroLevel = attainmentEntry.requiredHeroLevel;
		m_nRequiredMainQuestNo = attainmentEntry.requiredMainQuestNo;

		m_listCsAttainmentEntryReward = new List<CsAttainmentEntryReward>();
	}
}
