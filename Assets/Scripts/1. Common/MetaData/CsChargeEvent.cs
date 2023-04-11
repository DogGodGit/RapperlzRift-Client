using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsChargeEvent
{
	int m_nEventId;
	string m_strName;
	string m_strDescription;
	DateTime m_dtStartTime;
	DateTime m_dtEndTime;

	List<CsChargeEventMission> m_listCsChargeEventMission;

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

	public List<CsChargeEventMission> ChargeEventMissionList
	{
		get { return m_listCsChargeEventMission; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsChargeEvent(WPDChargeEvent chargeEvent)
	{
		m_nEventId = chargeEvent.eventId;
		m_strName = CsConfiguration.Instance.GetString(chargeEvent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(chargeEvent.descriptionKey);
		m_dtStartTime = chargeEvent.startTime;
		m_dtEndTime = chargeEvent.endTime;

		m_listCsChargeEventMission = new List<CsChargeEventMission>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsChargeEventMission GetChargeEventMission(int nMissionNo)
	{
		for (int i = 0; i < m_listCsChargeEventMission.Count; i++)
		{
			if (m_listCsChargeEventMission[i].MissionNo == nMissionNo)
				return m_listCsChargeEventMission[i];
		}

		return null;
	}
}
