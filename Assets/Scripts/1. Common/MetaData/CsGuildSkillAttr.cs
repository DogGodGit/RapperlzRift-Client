using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildSkillAttr
{
	int m_nGuildSkillId;
	CsAttr m_csAttr;

	List<CsGuildSkillLevelAttrValue> m_listCsGuildSkillLevelAttrValue;

	//---------------------------------------------------------------------------------------------------
	public int GuildSkillId
	{
		get { return m_nGuildSkillId; }
	}

	public CsAttr Attr
	{
		get { return m_csAttr; }
	}

	public List<CsGuildSkillLevelAttrValue> GuildSkillLevelAttrValueList
	{
		get { return m_listCsGuildSkillLevelAttrValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkillAttr(WPDGuildSkillAttr guildSkillAttr)
	{
		m_nGuildSkillId = guildSkillAttr.guildSkillId;
		m_csAttr = CsGameData.Instance.GetAttr(guildSkillAttr.attrId);

		m_listCsGuildSkillLevelAttrValue = new List<CsGuildSkillLevelAttrValue>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkillLevelAttrValue GetGuildSkillLevelAttrValueByLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsGuildSkillLevelAttrValue.Count; i++)
		{
			if (m_listCsGuildSkillLevelAttrValue[i].Level == nLevel)
				return m_listCsGuildSkillLevelAttrValue[i];
		}

		return null;
	}
}
