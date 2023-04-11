using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemorySchedule
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
	public CsWarMemorySchedule(WPDWarMemorySchedule warMemorySchedule)
	{
		m_nScheduleId = warMemorySchedule.scheduleId;
		m_nStartTime = warMemorySchedule.startTime;
		m_nEndTime = warMemorySchedule.endTime;
	}
}
