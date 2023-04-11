using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

/*
1 : 로우
2 : 노멀
3 : 레어
4 : 에픽
5 : 전설
6 : 정교
7 : 완벽
*/
public enum EnSubGearGrade
{
	Low = 1,
	Normal = 2,
	Rare = 3,
	Epic = 4,
	Legend = 5,
	Elaborate = 6,
	Perfect = 7,
}

public class CsSubGearGrade
{
	int m_nGrade;                   // 보조장비등급
	string m_strName;               // 이름
	string m_strColorCode;          // 색상코드

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public EnSubGearGrade EnGrade
	{
		get { return (EnSubGearGrade)m_nGrade; }
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
	public CsSubGearGrade(WPDSubGearGrade subGearGrade)
	{
		m_nGrade = subGearGrade.grade;
		m_strName = CsConfiguration.Instance.GetString(subGearGrade.nameKey);
		m_strColorCode = subGearGrade.colorCode;
	}
}
