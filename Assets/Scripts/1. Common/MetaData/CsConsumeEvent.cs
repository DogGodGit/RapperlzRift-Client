using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsConsumeEvent
{
	int m_nEventId;
	string m_strName;
	string m_strDescription;
	DateTime m_dtStartTime;
	DateTime m_dtEndTime;

	List<CsConsumeEventMission> m_listCsConsumeEventMission;

	//---------------------------------------------------------------------------------------------------
	public int EventId
	{
		get { return m_nEventId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public DateTime StartTime
	{
		get { return m_dtStartTime; }
	}

	public DateTime EndTime
	{
		get { return m_dtEndTime; }
	}

	public List<CsConsumeEventMission> ConsumeEventMissionList
	{
		get { return m_listCsConsumeEventMission; }
	}

	public CsConsumeEvent(WPDConsumeEvent consumeEvent)
	{
		m_nEventId = consumeEvent.eventId;
		m_strName = CsConfiguration.Instance.GetString(consumeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(consumeEvent.descriptionKey);
		m_dtStartTime = consumeEvent.startTime;
		m_dtEndTime = consumeEvent.endTime;

		m_listCsConsumeEventMission = new List<CsConsumeEventMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsConsumeEventMission GetConsumeEventMission(int nMissionNo)
	{
		for (int i = 0; i < m_listCsConsumeEventMission.Count; i++)
		{
			if (m_listCsConsumeEventMission[i].MissionNo == nMissionNo)
				return m_listCsConsumeEventMission[i];
		}

		return null;
	}
}
