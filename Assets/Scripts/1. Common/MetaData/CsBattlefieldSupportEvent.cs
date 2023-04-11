using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsBattlefieldSupportEvent
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
	public CsBattlefieldSupportEvent(WPDBattlefieldSupportEvent battlefieldSupportEvent)
	{
		m_nStartTime = battlefieldSupportEvent.startTime;
		m_nEndTime = battlefieldSupportEvent.endTime;
	}
}
