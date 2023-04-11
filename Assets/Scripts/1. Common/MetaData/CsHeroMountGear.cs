using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroMountGear : CsHeroObject
{
	Guid m_guid;
	CsMountGear m_csMountGear;
	bool m_bOwned;
	List<CsHeroMountGearOptionAttr> m_listCsHeroMountGearOptionAttr;
	int m_nBattlePower;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public CsMountGear MountGear
	{
		get { return m_csMountGear; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	public List<CsHeroMountGearOptionAttr> HeroMountGearOptionAttrList
	{
		get { return m_listCsHeroMountGearOptionAttr; }
	}

	public int BattlePower
	{
		get { return m_nBattlePower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMountGear(PDHeroMountGear heroMountGear)
		: base(EnHeroObjectType.MountGear)
	{
		m_guid = heroMountGear.id;
		m_csMountGear = CsGameData.Instance.GetMountGear(heroMountGear.mountGearId);
		m_bOwned = heroMountGear.owned;

		m_listCsHeroMountGearOptionAttr = new List<CsHeroMountGearOptionAttr>();

		for (int i = 0; i < heroMountGear.optionAttrs.Length; i++)
		{
			CsHeroMountGearOptionAttr csHeroMountGearOptionAttr = new CsHeroMountGearOptionAttr(heroMountGear.optionAttrs[i]);
			m_listCsHeroMountGearOptionAttr.Add(csHeroMountGearOptionAttr);
		}

		UpdateBattlePower();
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateBattlePower()
	{
		m_nBattlePower = m_csMountGear.BattlePowerValue;

		for (int i = 0; i < m_listCsHeroMountGearOptionAttr.Count; i++)
		{
			m_nBattlePower += m_listCsHeroMountGearOptionAttr[i].BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroMountGearOptionAttr GetHeroMountGearOptionAttr(int nIndex)
	{
		for (int i = 0; i < m_listCsHeroMountGearOptionAttr.Count; i++)
		{
			if (m_listCsHeroMountGearOptionAttr[i].Index == nIndex)
				return m_listCsHeroMountGearOptionAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateOptionAttr(int nIndex, PDHeroMountGearOptionAttr optionAttr)
	{
		CsHeroMountGearOptionAttr csHeroMountGearOptionAttr = GetHeroMountGearOptionAttr(nIndex);
		csHeroMountGearOptionAttr.Update(optionAttr);

		UpdateBattlePower();
	}
}
