using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

/*
1 : 최하급
2 : 하급
3 : 중급
4 : 상급
5 : 최상급
6 : 신급
*/
public enum EnMainGearQuality
{
	Lowest = 1,
	Low = 2,
	Intermediate = 3,
	High = 4,
	Heghest = 5,
	Divine = 6,
}

public class CsMainGearQuality
{
	int m_nQuality;     // 메인장비품질 
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public int Quality
	{
		get { return m_nQuality; }
	}

	public EnMainGearQuality EnQuality
	{
		get { return (EnMainGearQuality)m_nQuality; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearQuality(WPDMainGearQuality mainGearQuality)
	{
		m_nQuality = mainGearQuality.quality;
		m_strName = CsConfiguration.Instance.GetString(mainGearQuality.nameKey);
	}
}
