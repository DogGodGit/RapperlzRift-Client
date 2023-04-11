using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsItemGrade
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
	public CsItemGrade(WPDItemGrade itemGrade)
	{
		m_nGrade = itemGrade.grade;
		m_strName = CsConfiguration.Instance.GetString(itemGrade.nameKey);
		m_strColorCode = itemGrade.colorCode;
	}
}
