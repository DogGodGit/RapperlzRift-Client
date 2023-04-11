using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-14)
//---------------------------------------------------------------------------------------------------

public class CsHolyWarQuestSchedule
{
	int m_nScheduleId;
	int m_nStartTime;
	int m_nEndTime;

	//---------------------------------------------------------------------------------------------------
	public int ScheduleId
	{
		get { return m_nScheduleId; }
	}

	public int StartTime
	{
		get { return m_nStartTime; }
	}

	public int EndTime
	{
		get { return m_nEndTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHolyWarQuestSchedule(WPDHolyWarQuestSchedule holyWarQuestSchedule)
	{
		m_nScheduleId = holyWarQuestSchedule.scheduleId;
		m_nStartTime = holyWarQuestSchedule.startTime;
		m_nEndTime = holyWarQuestSchedule.endTime;
	}
}
