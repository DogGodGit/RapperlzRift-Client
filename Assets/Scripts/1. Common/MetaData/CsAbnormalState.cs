using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsAbnormalState
{
	int m_nAbnormalStateId;
	string m_strName;
	string m_strDescription;
	bool m_bIsOverlap;
	int m_nType;
	int m_nSourceType;

	List<CsAbnormalStateLevel> m_listCsAbnormalStateLevel;
	List<CsAbnormalStateRankSkillLevel> m_listCsAbnormalStateRankSkillLevel;

	//---------------------------------------------------------------------------------------------------
	public int AbnormalStateId
	{
		get { return m_nAbnormalStateId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public bool IsOverlap
	{
		get { return m_bIsOverlap; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int SourceType
	{
		get { return m_nSourceType; }
	}

	public List<CsAbnormalStateLevel> AbnormalStateLevelList
	{
		get { return m_listCsAbnormalStateLevel; }
	}

	public List<CsAbnormalStateRankSkillLevel> AbnormalStateRankSkillLevelList
	{
		get { return m_listCsAbnormalStateRankSkillLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAbnormalState(WPDAbnormalState abnormalState)
	{
		m_nAbnormalStateId = abnormalState.abnormalStateId;
		m_strName = CsConfiguration.Instance.GetString(abnormalState.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(abnormalState.descriptionKey);
		m_bIsOverlap = abnormalState.isOverlap;
		m_nType = abnormalState.type;
		m_nSourceType = abnormalState.sourceType;

		m_listCsAbnormalStateLevel = new List<CsAbnormalStateLevel>();
		m_listCsAbnormalStateRankSkillLevel = new List<CsAbnormalStateRankSkillLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsAbnormalStateLevel GetAbnormalStateLevel(int nJobId, int nLevel)
	{
		for (int i = 0; i < m_listCsAbnormalStateLevel.Count; i++)
		{
			if (m_listCsAbnormalStateLevel[i].JobId == nJobId &&
				m_listCsAbnormalStateLevel[i].Level == nLevel)
			{
				return m_listCsAbnormalStateLevel[i];
			}
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAbnormalStateRankSkillLevel GetAbnormalStateRankSkillLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsAbnormalStateRankSkillLevel.Count; i++)
		{
			if (m_listCsAbnormalStateRankSkillLevel[i].SkillLevel == nLevel)
			{
				return m_listCsAbnormalStateRankSkillLevel[i];
			}
		}
		return null;
	}
}
