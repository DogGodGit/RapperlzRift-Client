using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public class CsSubGearSoulstoneLevelSet
{
	int m_nSetNo;
	string m_strName;
	int m_nRequiredTotalLevel;

	List<CsSubGearSoulstoneLevelSetAttr> m_listCsSubGearSoulstoneLevelSetAttr;

	//---------------------------------------------------------------------------------------------------
	public int SetNo
	{
		get { return m_nSetNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredTotalLevel
	{
		get { return m_nRequiredTotalLevel; }
	}

	public List<CsSubGearSoulstoneLevelSetAttr> SubGearSoulstoneLevelSetAttrList
	{
		get { return m_listCsSubGearSoulstoneLevelSetAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubGearSoulstoneLevelSet(WPDSubGearSoulstoneLevelSet subGearSoulstoneLevelSet)
	{
		m_nSetNo = subGearSoulstoneLevelSet.setNo;
		m_strName = CsConfiguration.Instance.GetString(subGearSoulstoneLevelSet.nameKey);
		m_nRequiredTotalLevel = subGearSoulstoneLevelSet.requiredTotalLevel;

		m_listCsSubGearSoulstoneLevelSetAttr = new List<CsSubGearSoulstoneLevelSetAttr>();
	}
}
