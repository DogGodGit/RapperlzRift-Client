using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsDailyConsumeEvent
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredHeroLevel;

	List<CsDailyConsumeEventMission> m_listCsDailyConsumeEventMission;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public List<CsDailyConsumeEventMission> DailyConsumeEventMissionList
	{
		get { return m_listCsDailyConsumeEventMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyConsumeEvent(WPDDailyConsumeEvent dailyConsumeEvent)
	{
		m_strName = CsConfiguration.Instance.GetString(dailyConsumeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(dailyConsumeEvent.descriptionKey);
		m_nRequiredHeroLevel = dailyConsumeEvent.requiredHeroLevel;

		m_listCsDailyConsumeEventMission = new List<CsDailyConsumeEventMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyConsumeEventMission GetDailyConsumeEventMission(int nMissionNo)
	{
		for (int i = 0; i < m_listCsDailyConsumeEventMission.Count; i++)
		{
			if (m_listCsDailyConsumeEventMission[i].MissionNo == nMissionNo)
				return m_listCsDailyConsumeEventMission[i];
		}

		return null;
	}
}
