using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsTitleGrade
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
	public CsTitleGrade(WPDTitleGrade titleGrade)
	{
		m_nGrade = titleGrade.grade;
		m_strColorCode = titleGrade.colorCode;
	}
}
