using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsMountGearOptionAttrGrade
{
	int m_nAttrGrade;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int AttrGrade
	{
		get { return m_nAttrGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMountGearOptionAttrGrade(WPDMountGearOptionAttrGrade mountGearOptionAttrGrade)
	{
		m_nAttrGrade = mountGearOptionAttrGrade.attrGrade;
		m_strColorCode = mountGearOptionAttrGrade.colorCode;
	}
}
