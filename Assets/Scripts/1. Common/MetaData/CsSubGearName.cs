using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-13)
//---------------------------------------------------------------------------------------------------

public class CsSubGearName
{
	int m_nSubGearId;						// 보조장비ID
	CsSubGearGrade m_csSubGearGrade;        // 보조장비등급
	string m_strName;						// 이름

	//---------------------------------------------------------------------------------------------------
	public int SubGearId
	{
		get { return m_nSubGearId; }
	}

	public CsSubGearGrade SubGearGrade
	{
		get { return m_csSubGearGrade; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearName(WPDSubGearName subGearName)
	{
		m_nSubGearId = subGearName.subGearId;
		m_csSubGearGrade = CsGameData.Instance.GetSubGearGrade(subGearName.grade);
		m_strName = CsConfiguration.Instance.GetString(subGearName.nameKey);
	}

}
