using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsDimensionInfiltrationEvent
{
	int m_nStartTime;
	int m_nEndTime;

	//---------------------------------------------------------------------------------------------------
	public int StartTime
	{
		get { return m_nStartTime; }
	}

	public int EndTime
	{
		get { return m_nEndTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDimensionInfiltrationEvent(WPDDimensionInfiltrationEvent dimensionInfiltrationEvent)
	{
		m_nStartTime = dimensionInfiltrationEvent.startTime;
		m_nEndTime = dimensionInfiltrationEvent.endTime;
	}
}
