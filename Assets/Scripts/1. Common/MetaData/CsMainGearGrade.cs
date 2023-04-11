using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

/*
1 : 일반
2 : 고급
3 : 마법
4 : 희귀
5 : 전설
*/
public enum EnMainGearGrade
{
	General = 1,
	High = 2,
	Magic = 3,
	Rare = 4,
	Legend = 5,
}

public class CsMainGearGrade
{
	int m_nGrade;               // 메인장비등급
	string m_strName;			// 이름
	string m_strColorCode;		// 색상코드

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public EnMainGearGrade EnGrade
	{
		get { return (EnMainGearGrade)m_nGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public string Name
	{
		get { return string.Format("<color={0}>{1}</color>", m_strColorCode, m_strName); }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearGrade(WPDMainGearGrade mainGearGrade)
	{
		m_nGrade = mainGearGrade.grade;
		m_strColorCode = mainGearGrade.colorCode;
		m_strName = CsConfiguration.Instance.GetString(mainGearGrade.nameKey);
	}
}
