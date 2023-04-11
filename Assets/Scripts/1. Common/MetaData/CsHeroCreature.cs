using System;
using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreature
{
	Guid m_guidInstanceId;
	CsCreature m_csCreature;
	int m_nCreatureLevel;
	CsCreatureLevel m_csCreatureLevel;
	int m_nAdditionalOpenSkillSlotCount;
	int m_nExp;
	int m_nInjectionLevel;
	CsCreatureInjectionLevel m_csCreatureInjectionLevel;
	int m_nInjectionExp;
	int m_nInjectionItemCount;
	int m_nQuality;
	bool m_bCheered;

	List<CsHeroCreatureBaseAttr> m_listCsHeroCreatureBaseAttr;
	List<int> m_listAdditionalAttrId;
	List<CsHeroCreatureSkill> m_listCsHeroCreatureSkill;

	//---------------------------------------------------------------------------------------------------
	public Guid InstanceId
	{
		get { return m_guidInstanceId; }
	}

	public CsCreature Creature
	{
		get { return m_csCreature; }
	}

	public int Level
	{
		get { return m_nCreatureLevel; }
		set
		{
			m_nCreatureLevel = value;
			m_csCreatureLevel = CsGameData.Instance.GetCreatureLevel(m_nCreatureLevel);
		}
	}

	public CsCreatureLevel CreatureLevel
	{
		get { return m_csCreatureLevel; }
	}

	public int AdditionalOpenSkillSlotCount
	{
		get { return m_nAdditionalOpenSkillSlotCount; }
		set { m_nAdditionalOpenSkillSlotCount = value; }
	}

	public int Exp
	{
		get { return m_nExp; }
		set { m_nExp = value; }
	}

	public int InjectionLevel
	{
		get { return m_nInjectionLevel; }
		set
		{
			m_nInjectionLevel = value;
			m_csCreatureInjectionLevel = CsGameData.Instance.GetCreatureInjectionLevel(m_nInjectionLevel);
		}
	}

	public CsCreatureInjectionLevel CreatureInjectionLevel
	{
		get { return m_csCreatureInjectionLevel; }
	}

	public int InjectionExp
	{
		get { return m_nInjectionExp; }
		set { m_nInjectionExp = value; }
	}

	public int InjectionItemCount
	{
		get { return m_nInjectionItemCount; }
		set { m_nInjectionItemCount = value; }
	}

	public int Quality
	{
		get { return m_nQuality; }
		set { m_nQuality = value; }
	}

	public bool Cheered
	{
		get { return m_bCheered; }
		set { m_bCheered = value; }
	}

	public List<CsHeroCreatureBaseAttr> HeroCreatureBaseAttrList
	{
		get { return m_listCsHeroCreatureBaseAttr; }
	}

	public List<int> AdditionalAttrIdList
	{
		get { return m_listAdditionalAttrId; }
	}

	public List<CsHeroCreatureSkill> HeroCreatureSkillList
	{
		get { return m_listCsHeroCreatureSkill; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreature(PDHeroCreature heroCreature)
	{
		m_guidInstanceId = heroCreature.instanceId;
		m_csCreature = CsGameData.Instance.GetCreature(heroCreature.creatureId);
		m_nCreatureLevel = heroCreature.level;
		m_csCreatureLevel = CsGameData.Instance.GetCreatureLevel(heroCreature.level);
		m_nAdditionalOpenSkillSlotCount = heroCreature.additionalOpenSkillSlotCount;
		m_nExp = heroCreature.exp;
		m_nInjectionLevel = heroCreature.injectionLevel;
		m_csCreatureInjectionLevel = CsGameData.Instance.GetCreatureInjectionLevel(heroCreature.injectionLevel);
		m_nInjectionExp = heroCreature.injectionExp;
		m_nInjectionItemCount = heroCreature.injectionItemCount;
		m_nQuality = heroCreature.quality;
		m_bCheered = heroCreature.cheered;

		m_listCsHeroCreatureBaseAttr = new List<CsHeroCreatureBaseAttr>();

		for (int i = 0; i < heroCreature.baseAttrs.Length; i++)
		{
			m_listCsHeroCreatureBaseAttr.Add(new CsHeroCreatureBaseAttr(heroCreature.baseAttrs[i]));
		}

		m_listAdditionalAttrId = new List<int>(heroCreature.additionalAttrIds);

		m_listCsHeroCreatureSkill = new List<CsHeroCreatureSkill>();

		for (int i = 0; i < heroCreature.skills.Length; i++)
		{
			m_listCsHeroCreatureSkill.Add(new CsHeroCreatureSkill(heroCreature.skills[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroCreatureBaseAttr GetHeroCreatureBaseAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsHeroCreatureBaseAttr.Count; i++)
		{
			if (m_listCsHeroCreatureBaseAttr[i].CreatureBaseAttr.Attr.AttrId == nAttrId)
				return m_listCsHeroCreatureBaseAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateCreatureBaseAttrs(PDHeroCreatureBaseAttr[] baseAttrs)
	{
		for (int i = 0; i < baseAttrs.Length; i++)
		{
			CsHeroCreatureBaseAttr csHeroCreatureBaseAttr = GetHeroCreatureBaseAttr(baseAttrs[i].attrId);
			csHeroCreatureBaseAttr.BaseValue = baseAttrs[i].baseValue;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateCreatureAdditionalAttrs(int[] additionalAttrIds)
	{
		m_listAdditionalAttrId.Clear();
		m_listAdditionalAttrId = new List<int>(additionalAttrIds);
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateCreatureSkills(PDHeroCreatureSkill[] mainHeroCreatureSkills)
	{
		m_listCsHeroCreatureSkill.Clear();

		for (int i = 0; i < mainHeroCreatureSkills.Length; i++)
		{
			m_listCsHeroCreatureSkill.Add(new CsHeroCreatureSkill(mainHeroCreatureSkills[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처 평점
	public int GetCreatureGrade()
	{
		int nTotalAttr = 0;

		if (m_listCsHeroCreatureBaseAttr.Count > 0)
		{
			foreach (CsHeroCreatureBaseAttr csHeroCreatureBaseAttr in m_listCsHeroCreatureBaseAttr)
			{
				nTotalAttr += csHeroCreatureBaseAttr.BaseValue;
			}

			return nTotalAttr / m_listCsHeroCreatureBaseAttr.Count;
		}
		else
		{
			return 0;
		}
	}
}
