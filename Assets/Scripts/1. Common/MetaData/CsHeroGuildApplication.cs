using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-09)
//---------------------------------------------------------------------------------------------------

public class CsHeroGuildApplication
{
	Guid m_guidId;

	Guid m_guidGuildId;
	string m_strGuildName;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public Guid GuildId
	{
		get { return m_guidGuildId; }
	}

	public string GuildName
	{
		get { return m_strGuildName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroGuildApplication(PDHeroGuildApplication heroGuildApplication)
	{
		m_guidId = heroGuildApplication.id;
		m_guidGuildId = heroGuildApplication.guildId;
		m_strGuildName = heroGuildApplication.guildName;
	}
}
