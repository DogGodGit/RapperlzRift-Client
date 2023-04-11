using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildSkill
{
	int m_nGuildSkillId;
	string m_strName;

	List<CsGuildSkillAttr> m_listCsGuildSkillAttr;
	List<CsGuildSkillLevel> m_listCsGuildSkillLevel;

	//---------------------------------------------------------------------------------------------------
	public int GuildSkillId
	{
		get { return m_nGuildSkillId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsGuildSkillAttr> GuildSkillAttrList
	{
		get { return m_listCsGuildSkillAttr; }
	}

	public List<CsGuildSkillLevel> GuildSkillLevelList
	{
		get { return m_listCsGuildSkillLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkill(WPDGuildSkill guildSkill)
	{
		m_nGuildSkillId = guildSkill.guildSkillId;
		m_strName = CsConfiguration.Instance.GetString(guildSkill.nameKey);

		m_listCsGuildSkillAttr = new List<CsGuildSkillAttr>();
		m_listCsGuildSkillLevel = new List<CsGuildSkillLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkillAttr GetGuildSkillAttr(int nAttrId)
	{
		for (int i = 0; i < m_listCsGuildSkillAttr.Count; i++)
		{
			if (m_listCsGuildSkillAttr[i].Attr.AttrId == nAttrId)
				return m_listCsGuildSkillAttr[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsGuildSkillLevelAttrValue> GetGuildSkillLevelAttrValue(int nLevel)
	{
		List<CsGuildSkillLevelAttrValue> list = new List<CsGuildSkillLevelAttrValue>();

		for (int i = 0; i < m_listCsGuildSkillAttr.Count; i++)
		{
			CsGuildSkillLevelAttrValue csGuildSkillLevelAttrValue = m_listCsGuildSkillAttr[i].GetGuildSkillLevelAttrValueByLevel(nLevel);
			list.Add(csGuildSkillLevelAttrValue);
		}

		return list;
	}




}
