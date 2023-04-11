using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsCartGrade
{
	int m_nGrade;
	string m_strName;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCartGrade(WPDCartGrade cartGrade)
	{
		m_nGrade = cartGrade.grade;
		m_strName = CsConfiguration.Instance.GetString(cartGrade.nameKey);
		m_strColorCode = cartGrade.colorCode;
	}
}
