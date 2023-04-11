using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureGrade
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
	public CsCreatureGrade(WPDCreatureGrade creatureGrade)
	{
		m_nGrade = creatureGrade.grade;
		m_strName = CsConfiguration.Instance.GetString(creatureGrade.nameKey);
		m_strColorCode = creatureGrade.colorCode;
	}
}
