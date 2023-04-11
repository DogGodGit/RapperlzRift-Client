using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsInfiniteWarOpenSchedule
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
	public CsInfiniteWarOpenSchedule(WPDInfiniteWarOpenSchedule infiniteWarOpenSchedule)
	{
		m_nScheduleId = infiniteWarOpenSchedule.scheduleId;
		m_nStartTime = infiniteWarOpenSchedule.startTime;
		m_nEndTime = infiniteWarOpenSchedule.endTime;
	}
}
