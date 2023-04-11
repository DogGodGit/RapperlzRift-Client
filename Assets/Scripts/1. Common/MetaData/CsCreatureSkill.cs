using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureSkill
{
	int m_nSkillId;
	CsAttr m_csAttr;
	string m_strName;
	string m_strImageName;
	string m_strEffectText;

	List<CsCreatureSkillAttr> m_listCsCreatureSkillAttr; 

	//---------------------------------------------------------------------------------------------------
	public int SkillId
	{
		get { return m_nSkillId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public string EffectText
	{
		get { return m_strEffectText; }
	}

	public List<CsCreatureSkillAttr> CreatureSkillAttrList
	{
		get { return m_listCsCreatureSkillAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkill(WPDCreatureSkill creatureSkill)
	{
		m_nSkillId = creatureSkill.skillId;
		m_csAttr = CsGameData.Instance.GetAttr(creatureSkill.attrId);
		m_strName = CsConfiguration.Instance.GetString(creatureSkill.nameKey);
		m_strImageName = creatureSkill.imageName;
		m_strEffectText = CsConfiguration.Instance.GetString(creatureSkill.effectTextKey);

		m_listCsCreatureSkillAttr = new List<CsCreatureSkillAttr>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureSkillAttr GetCreatureSkillAttr(int nSkillGrade)
	{
		for (int i = 0; i < m_listCsCreatureSkillAttr.Count; i++)
		{
			if (m_listCsCreatureSkillAttr[i].CreatureSkillGrade.SkillGrade == nSkillGrade)
				return m_listCsCreatureSkillAttr[i];
		}

		return null;
	}
}
