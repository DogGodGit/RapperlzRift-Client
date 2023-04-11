using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsFieldBossEventSchedule
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
	public CsFieldBossEventSchedule(WPDFieldBossEventSchedule fieldBossEventSchedule)
	{
		m_nScheduleId = fieldBossEventSchedule.scheduleId;
		m_nStartTime = fieldBossEventSchedule.startTime;
		m_nEndTime = fieldBossEventSchedule.endTime;
	}
}
