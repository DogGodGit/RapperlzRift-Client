using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookAttrGrade
{
	int m_nGrade;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookAttrGrade(WPDIllustratedBookAttrGrade illustratedBookAttrGrade)
	{
		m_nGrade = illustratedBookAttrGrade.grade;
		m_strColorCode = illustratedBookAttrGrade.colorCode;
	}
}
