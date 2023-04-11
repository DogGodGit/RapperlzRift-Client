using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-24)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardShopRefreshSchedule
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
	public CsCreatureCardShopRefreshSchedule(WPDCreatureCardShopRefreshSchedule creatureCardShopRefreshSchedule)
	{
		m_nScheduleId = creatureCardShopRefreshSchedule.scheduleId;
		m_nRefreshTime = creatureCardShopRefreshSchedule.refreshTime;
	}
}
