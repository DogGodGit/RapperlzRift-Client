using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsAnkouTombSchedule
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
	public CsAnkouTombSchedule(WPDAnkouTombSchedule ankouTombSchedule)
	{
		m_nScheduleId = ankouTombSchedule.scheduleId;
		m_nStartTime = ankouTombSchedule.startTime;
		m_nEndTime = ankouTombSchedule.endTime;
	}
}
