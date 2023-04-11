using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public class CsHeroMainGear : CsHeroObject
{ 
	Guid m_guid;                // 영웅메인장비ID
	CsMainGear m_csMainGear;    // 메인장비ID	
	int m_nEnchantLevel;        // 강화 레벨
	bool m_bOwned;              // 귀속 여부

	// 전투력
	//int m_nBattlePower = 0;
	int m_nOptionAttributesBattlePower = 0;
	int m_nBaseAttributesBattlePower = 0;

	CsMainGearEnchantLevel m_csMainGearEnchantLevel;

	List<CsAttrValue> m_listCsAttrValueBase = new List<CsAttrValue>();
	List<CsHeroMainGearOptionAttr> m_listCsHeroMainGearOptionAttr = new List<CsHeroMainGearOptionAttr>();
	List<CsHeroMainGearRefinement> m_listCsHeroMainGearRefinement = new List<CsHeroMainGearRefinement>();

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	public CsMainGearEnchantLevel MainGearEnchantLevel
	{
		get { return m_csMainGearEnchantLevel; }
	}


	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
		set
		{
			m_nEnchantLevel = value;

			m_csMainGearEnchantLevel = CsGameData.Instance.GetMainGearEnchantLevel(m_nEnchantLevel);

			m_listCsAttrValueBase.Clear();
			m_nBaseAttributesBattlePower = 0;

			for (int i = 0; i < m_csMainGear.MainGearBaseAttrList.Count; i++)
			{
				CsMainGearBaseAttrEnchantLevel csMainGearBaseAttrEnchantLevel = m_csMainGear.MainGearBaseAttrList[i].GetMainGearBaseAttrEnchantLevel(m_nEnchantLevel);

				int nValue = 0;

				if (csMainGearBaseAttrEnchantLevel != null)
				{
					nValue = csMainGearBaseAttrEnchantLevel.Value;
				}

				CsAttrValue csAttrValue = new CsAttrValue(m_csMainGear.MainGearBaseAttrList[i].Attr, nValue);
				m_listCsAttrValueBase.Add(csAttrValue);

				m_nBaseAttributesBattlePower += csAttrValue.BattlePowerValue;
			}
		}
	}

	public bool Owned
	{
		get { return m_bOwned; }
		set { m_bOwned = value; }
	}

	public int BattlePower
	{
		get { return m_nOptionAttributesBattlePower + m_nBaseAttributesBattlePower; }
	}

	public List<CsAttrValue> BaseAttrValueList
	{
		get { return m_listCsAttrValueBase; }
	}

	public List<CsHeroMainGearOptionAttr> OptionAttrList
	{
		get { return m_listCsHeroMainGearOptionAttr; }
	}

	public List<CsHeroMainGearRefinement> HeroMainGearRefinementList
	{
		get { return m_listCsHeroMainGearRefinement; }
	}

	public int BaseAttributesBattlePower
	{
		get { return m_nBaseAttributesBattlePower; }
	}

	public int OptionAttributesBattlePower
	{
		get { return m_nOptionAttributesBattlePower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear(PDFullHeroMainGear heroMainGear)
		: base(EnHeroObjectType.MainGear)
	{
		m_guid = heroMainGear.id;
		m_csMainGear = CsGameData.Instance.GetMainGear(heroMainGear.mainGearId);
		EnchantLevel = heroMainGear.enchantLevel;
		m_bOwned = heroMainGear.owned;
		AddOptionAttributes(heroMainGear.optionAttrs);
		AddHeroMainGearRefinements(heroMainGear.refinements);
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear(Guid guid, int nMainGearId, int nEnchantLevel, bool bOwned, List<CsHeroMainGearOptionAttr> list, List<CsHeroMainGearRefinement> refinements)
		: base(EnHeroObjectType.MainGear)
	{
		m_guid = guid;
		m_csMainGear = CsGameData.Instance.GetMainGear(nMainGearId);
		EnchantLevel = nEnchantLevel;
		m_bOwned = bOwned;
		AddOptionAttributes(list);

		if (refinements != null)
			m_listCsHeroMainGearRefinement = refinements;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear(Guid guid, int nMainGearId, int nEnchantLevel, bool bOwned, PDHeroMainGearOptionAttr[] list, List<CsHeroMainGearRefinement> refinements = null)
		: base(EnHeroObjectType.MainGear)
	{
		m_guid = guid;
		m_csMainGear = CsGameData.Instance.GetMainGear(nMainGearId);
		EnchantLevel = nEnchantLevel;
		m_bOwned = bOwned;
		AddOptionAttributes(list);

		if (refinements != null)
			m_listCsHeroMainGearRefinement = refinements;
	}


	//---------------------------------------------------------------------------------------------------
	public void AddOptionAttributes(PDHeroMainGearOptionAttr[] heroMainGearOptionAttrs)
	{
		m_listCsHeroMainGearOptionAttr.Clear();
		m_nOptionAttributesBattlePower = 0;

		for (int i = 0; i < heroMainGearOptionAttrs.Length; i++)
		{
			CsHeroMainGearOptionAttr csHeroMainGearOptionAttr = new CsHeroMainGearOptionAttr(heroMainGearOptionAttrs[i]);
			m_listCsHeroMainGearOptionAttr.Add(csHeroMainGearOptionAttr);

			m_nOptionAttributesBattlePower += csHeroMainGearOptionAttr.BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddOptionAttributes(List<CsHeroMainGearOptionAttr> listCsHeroMainGearOptionAttr)
	{
		m_listCsHeroMainGearOptionAttr.Clear();
		m_nOptionAttributesBattlePower = 0;

		for (int i = 0; i < listCsHeroMainGearOptionAttr.Count; i++)
		{
			m_listCsHeroMainGearOptionAttr.Add(listCsHeroMainGearOptionAttr[i]);

			m_nOptionAttributesBattlePower += listCsHeroMainGearOptionAttr[i].BattlePowerValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroMainGearRefinements(PDHeroMainGearRefinement[] heroMainGearRefinements)
	{
		ClearHeroMainGearRefinements();

		for (int i = 0; i < heroMainGearRefinements.Length; i++)
		{
			m_listCsHeroMainGearRefinement.Add(new CsHeroMainGearRefinement(heroMainGearRefinements[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ClearHeroMainGearRefinements()
	{
		m_listCsHeroMainGearRefinement.Clear();
	}
}
