using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-10)
//---------------------------------------------------------------------------------------------------

public class CsWeekendAttendRewardAvailableDayOfWeek
{
	int m_nDayOfWeek;

	//---------------------------------------------------------------------------------------------------
	public int DayOfWeek
	{
		get { return m_nDayOfWeek; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeekendAttendRewardAvailableDayOfWeek(WPDWeekendAttendRewardAvailableDayOfWeek weekendAttendRewardAvailableDayOfWeek)
	{
		m_nDayOfWeek = weekendAttendRewardAvailableDayOfWeek.dayOfWeek;
	}
}
