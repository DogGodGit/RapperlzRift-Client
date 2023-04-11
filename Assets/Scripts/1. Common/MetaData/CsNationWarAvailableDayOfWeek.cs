using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-30)
//---------------------------------------------------------------------------------------------------

public class CsNationWarAvailableDayOfWeek
{
	int m_nDayOfWeek;

	//---------------------------------------------------------------------------------------------------
	public int DayOfWeek
	{
		get { return m_nDayOfWeek; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationWarAvailableDayOfWeek(WPDNationWarAvailableDayOfWeek nationWarAvailableDayOfWeek)
	{
		m_nDayOfWeek = nationWarAvailableDayOfWeek.dayOfWeek;
	}
}
