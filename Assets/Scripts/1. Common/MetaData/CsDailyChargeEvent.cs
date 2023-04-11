using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsDailyChargeEvent
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredHeroLevel;

	List<CsDailyChargeEventMission> m_listCsDailyChargeEventMission;

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

	public List<CsDailyChargeEventMission> DailyChargeEventMissionList
	{
		get { return m_listCsDailyChargeEventMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyChargeEvent(WPDDailyChargeEvent dailyChargeEvent)
	{
		m_strName = CsConfiguration.Instance.GetString(dailyChargeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(dailyChargeEvent.descriptionKey);
		m_nRequiredHeroLevel = dailyChargeEvent.requiredHeroLevel;

		m_listCsDailyChargeEventMission = new List<CsDailyChargeEventMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyChargeEventMission GetDailyChargeEventMission(int nMissionNo)
	{
		for (int i = 0; i < m_listCsDailyChargeEventMission.Count; i++)
		{
			if (m_listCsDailyChargeEventMission[i].MissionNo == nMissionNo)
				return m_listCsDailyChargeEventMission[i];
		}

		return null;
	}
}
