using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsMainGearSet
{
	int m_nTier;
	int m_nGrade;
	int m_nQuality;
	string m_strName;

	List<CsMainGearSetAttr> m_listCsMainGearSetAttr;

	//---------------------------------------------------------------------------------------------------
	public int Tier
	{
		get { return m_nTier; }
	}

	public int Grade
	{
		get { return m_nGrade; }
	}

	public int Quality
	{
		get { return m_nQuality; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsMainGearSetAttr> MainGearSetAttrList
	{
		get { return m_listCsMainGearSetAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearSet(WPDMainGearSet mainGearSet)
	{
		m_nTier = mainGearSet.tier;
		m_nGrade = mainGearSet.grade;
		m_nQuality = mainGearSet.quality;
		m_strName = CsConfiguration.Instance.GetString(mainGearSet.nameKey);

		m_listCsMainGearSetAttr = new List<CsMainGearSetAttr>();
	}
}
