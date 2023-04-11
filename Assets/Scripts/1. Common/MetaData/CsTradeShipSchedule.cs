using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShipSchedule
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
	public CsTradeShipSchedule(WPDTradeShipSchedule tradeShipSchedule)
	{
		m_nScheduleId = tradeShipSchedule.scheduleId;
		m_nStartTime = tradeShipSchedule.startTime;
		m_nEndTime = tradeShipSchedule.endTime;
	}
}
