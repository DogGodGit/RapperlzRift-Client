using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

/*
1 : 왕 
2 : 대공 
3 : 공작 
4 : 후작 
5 : 백작 
6 : 자작 
7 : 남작 
8 : 준남작  
*/

public class CsNationNoblesse
{
	int m_nNoblesseId;
	string m_strName;
	bool m_bNationWarDeclarationEnabled;
	bool m_bNationCallEnabled;
	bool m_bNationWarCallEnabled;
	bool m_bNationWarConvergingAttackEnabled;
	bool m_bNationAllianceEnabled;

	List<CsNationNoblesseAttr> m_listCsNationNoblesseAttr;
	List<CsNationNoblesseAppointmentAuthority> m_listCsNationNoblesseAppointmentAuthority;

	//---------------------------------------------------------------------------------------------------
	public int NoblesseId
	{
		get { return m_nNoblesseId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public bool NationWarDeclarationEnabled
	{
		get { return m_bNationWarDeclarationEnabled; }
	}

	public bool NationCallEnabled
	{
		get { return m_bNationCallEnabled; }
	}

	public bool NationWarCallEnabled
	{
		get { return m_bNationWarCallEnabled; }
	}

	public bool NationWarConvergingAttackEnabled
	{
		get { return m_bNationWarConvergingAttackEnabled; }
	}

	public List<CsNationNoblesseAttr> NationNoblesseAttrList
	{
		get { return m_listCsNationNoblesseAttr; }
	}

	public List<CsNationNoblesseAppointmentAuthority> NationNoblesseAppointmentAuthorityList
	{
		get { return m_listCsNationNoblesseAppointmentAuthority; }
	}

	public bool NationAllianceEnabled
	{
		get { return m_bNationAllianceEnabled; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesse(WPDNationNoblesse nationNoblesse)
	{
		m_nNoblesseId = nationNoblesse.noblesseId;
		m_strName = CsConfiguration.Instance.GetString(nationNoblesse.nameKey);
		m_bNationWarDeclarationEnabled = nationNoblesse.nationWarDeclarationEnabled;
		m_bNationCallEnabled = nationNoblesse.nationCallEnabled;
		m_bNationWarCallEnabled = nationNoblesse.nationWarCallEnabled;
		m_bNationWarConvergingAttackEnabled = nationNoblesse.nationWarConvergingAttackEnabled;
		m_bNationAllianceEnabled = nationNoblesse.nationAllianceEnabled;

		m_listCsNationNoblesseAttr = new List<CsNationNoblesseAttr>();
		m_listCsNationNoblesseAppointmentAuthority = new List<CsNationNoblesseAppointmentAuthority>();
	}
}
