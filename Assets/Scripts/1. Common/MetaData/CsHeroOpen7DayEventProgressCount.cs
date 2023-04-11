using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroOpen7DayEventProgressCount
{
	int m_nType;
	int m_nAccProgressCount;

	//---------------------------------------------------------------------------------------------------
	public int Type
	{
		get { return m_nType; }
	}

	public int AccProgressCount
	{
		get { return m_nAccProgressCount; }
		set { m_nAccProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroOpen7DayEventProgressCount(PDHeroOpen7DayEventProgressCount heroOpen7DayEventProgressCount)
	{
		m_nType = heroOpen7DayEventProgressCount.type;
		m_nAccProgressCount = heroOpen7DayEventProgressCount.accProgressCount;
	}
}
