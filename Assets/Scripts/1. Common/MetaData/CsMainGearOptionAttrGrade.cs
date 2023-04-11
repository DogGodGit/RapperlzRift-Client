using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsMainGearOptionAttrGrade
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
	public CsMainGearOptionAttrGrade(WPDMainGearOptionAttrGrade mainGearOptionAttrGrade)
	{
		m_nAttrGrade = mainGearOptionAttrGrade.attrGrade;
		m_strColorCode = mainGearOptionAttrGrade.colorCode;
	}
}
