using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-24)
//---------------------------------------------------------------------------------------------------

public class CsAccomplishmentLevel
{
	int m_nAccomplishmentLevel;
	int m_nRequiredAccomplishmentPoint;
	string m_strDescription;

	List<CsAccomplishmentLevelAttr> m_listCsAccomplishmentLevelAttr;

	//---------------------------------------------------------------------------------------------------
	public int AccomplishmentLevel
	{
		get { return m_nAccomplishmentLevel; }
	}

	public int RequiredAccomplishmentPoint
	{
		get { return m_nRequiredAccomplishmentPoint; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public List<CsAccomplishmentLevelAttr> AccomplishmentLevelAttrList
	{
		get { return m_listCsAccomplishmentLevelAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAccomplishmentLevel(WPDAccomplishmentLevel accomplishmentLevel)
	{


		m_listCsAccomplishmentLevelAttr = new List<CsAccomplishmentLevelAttr>();
	}
}
