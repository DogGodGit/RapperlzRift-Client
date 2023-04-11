using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildSkillLevel
{
	int m_nGuildSkillId;
	int m_nLevel;
	int m_nRequiredGuildContributionPoint;
	int m_nRequiredLaboratoryLevel;

	//---------------------------------------------------------------------------------------------------
	public int GuildSkillId
	{
		get { return m_nGuildSkillId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int RequiredGuildContributionPoint
	{
		get { return m_nRequiredGuildContributionPoint; }
	}

	public int RequiredLaboratoryLevel
	{
		get { return m_nRequiredLaboratoryLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildSkillLevel(WPDGuildSkillLevel guildSkillLevel)
	{
		m_nGuildSkillId = guildSkillLevel.guildSkillId;
		m_nLevel = guildSkillLevel.level;
		m_nRequiredGuildContributionPoint = guildSkillLevel.requiredGuildContributionPoint;
		m_nRequiredLaboratoryLevel = guildSkillLevel.requiredLaboratoryLevel;
	}
}
