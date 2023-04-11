using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureSkillGrade
{
	int m_nSkillGrade;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int SkillGrade
	{
		get { return m_nSkillGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillGrade(WPDCreatureSkillGrade creatureSkillGrade)
	{
		m_nSkillGrade = creatureSkillGrade.skillGrade;
		m_strColorCode = creatureSkillGrade.colorCode;
	}
}
