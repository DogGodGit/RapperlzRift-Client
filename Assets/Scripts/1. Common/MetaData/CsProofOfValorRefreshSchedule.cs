using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorRefreshSchedule
{
	int m_nScheduleId;
	int m_nRefreshTime;

	//---------------------------------------------------------------------------------------------------
	public int ScheduleId
	{
		get { return m_nScheduleId; }
	}

	public int RefreshTime
	{
		get { return m_nRefreshTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorRefreshSchedule(WPDProofOfValorRefreshSchedule proofOfValorRefreshSchedule)
	{
		m_nScheduleId = proofOfValorRefreshSchedule.scheduleId;
		m_nRefreshTime = proofOfValorRefreshSchedule.refreshTime;
	}
}
