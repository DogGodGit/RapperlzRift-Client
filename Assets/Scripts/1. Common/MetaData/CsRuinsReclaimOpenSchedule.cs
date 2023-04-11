using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaimOpenSchedule
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
	public CsRuinsReclaimOpenSchedule(WPDRuinsReclaimOpenSchedule ruinsReclaimOpenSchedule)
	{
		m_nScheduleId = ruinsReclaimOpenSchedule.scheduleId;
		m_nStartTime = ruinsReclaimOpenSchedule.startTime;
		m_nEndTime = ruinsReclaimOpenSchedule.endTime;
	}
}
