using System.Collections.Generic;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsHeroGuildSkill
{
	CsGuildSkill m_csGuildSkill;
	int m_nLevel;

	//---------------------------------------------------------------------------------------------------
	public int Id
	{
		get { return m_csGuildSkill.GuildSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set { m_nLevel = value; }
	}

	public CsGuildSkill GuildSkill
	{
		get { return m_csGuildSkill; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildSkill(PDHeroGuildSkill heroGuildSkill)
	{
		m_csGuildSkill = CsGameData.Instance.GetGuildSkill(heroGuildSkill.id);
		m_nLevel = heroGuildSkill.level;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildSkill(int nId, int nLevel)
	{
		m_csGuildSkill = CsGameData.Instance.GetGuildSkill(nId);
		m_nLevel = nLevel;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsGuildSkillLevelAttrValue> GetGuildSkillLevelAttrValue(int nLevel)
	{
		return m_csGuildSkill.GetGuildSkillLevelAttrValue(nLevel);
	}

	public long GetBattlePower(int nLevel)
	{
		long lBattlePower = 0;
		List<CsGuildSkillLevelAttrValue> list = GetGuildSkillLevelAttrValue(nLevel);

		for (int i = 0; i < list.Count; i++)
		{
			lBattlePower += list[i].BattlePower;
		}

		return lBattlePower;
	}


}
