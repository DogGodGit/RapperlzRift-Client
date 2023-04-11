using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsLimitationGiftRewardDayOfWeek
{
	int m_nDayOfWeek;

	//---------------------------------------------------------------------------------------------------
	public int DayOfWeek
	{
		get { return m_nDayOfWeek; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLimitationGiftRewardDayOfWeek(WPDLimitationGiftRewardDayOfWeek limitationGiftRewardDayOfWeek)
	{
		m_nDayOfWeek = limitationGiftRewardDayOfWeek.dayOfWeek;
	}
}
