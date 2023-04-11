using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsNationInstance
{
	int m_nNationId;
	long m_lFund;
	List<CsNationNoblesseInstance> m_listCsNationNoblesseInstance;

	//---------------------------------------------------------------------------------------------------
	public int NationId
	{
		get { return m_nNationId; }
	}

	public long Fund
	{
		get { return m_lFund; }
		set { m_lFund = value; }
	}

	public List<CsNationNoblesseInstance> NationNoblesseInstanceList
	{
		get { return m_listCsNationNoblesseInstance; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationInstance(PDNationInstance nationInstance)
	{
		m_nNationId = nationInstance.nationId;
		m_lFund = nationInstance.fund;

		m_listCsNationNoblesseInstance = new List<CsNationNoblesseInstance>();

		for (int i = 0; i < nationInstance.noblesseInsts.Length; i++)
		{
			m_listCsNationNoblesseInstance.Add(new CsNationNoblesseInstance(nationInstance.noblesseInsts[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesseInstance GetNationNoblesseInstanceByHeroId(Guid guidHeroId)
	{
		for (int i = 0; i < m_listCsNationNoblesseInstance.Count; i++)
		{
			if (m_listCsNationNoblesseInstance[i].HeroId == guidHeroId)
				return m_listCsNationNoblesseInstance[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationNoblesseInstance GetNationNoblesseInstance(int nNoblessId)
	{
		for (int i = 0; i < m_listCsNationNoblesseInstance.Count; i++)
		{
			if (m_listCsNationNoblesseInstance[i].NationNoblesse.NoblesseId == nNoblessId)
				return m_listCsNationNoblesseInstance[i];
		}

		return null;
	}
}
