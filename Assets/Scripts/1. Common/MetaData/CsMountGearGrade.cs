using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearGrade
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
	public CsMountGearGrade(WPDMountGearGrade mountGearGrade)
	{
		m_nGrade = mountGearGrade.grade;
		m_strName = CsConfiguration.Instance.GetString(mountGearGrade.nameKey);
		m_strColorCode = mountGearGrade.colorCode;
	}
}
