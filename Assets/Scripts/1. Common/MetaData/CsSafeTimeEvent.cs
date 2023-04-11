using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-07)
//---------------------------------------------------------------------------------------------------

public class CsSafeTimeEvent
{
	int m_nRequiredAutoDuration;
	int m_nStartTime;
	int m_nEndTime;

	//---------------------------------------------------------------------------------------------------
	public int RequiredAutoDuration
	{
		get { return m_nRequiredAutoDuration; }
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
	public CsSafeTimeEvent(WPDSafeTimeEvent safeTimeEvent)
	{
		m_nRequiredAutoDuration = safeTimeEvent.requiredAutoDuration;
		m_nStartTime = safeTimeEvent.startTime;
		m_nEndTime = safeTimeEvent.endTime;
	}
}
