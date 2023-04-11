using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsHeroGuildInvitation
{
	Guid m_guidId;
	Guid m_guidGuildId;
	string m_strGuildName;
	Guid m_guidInviterId;
	string m_strInviterName;
    float m_flRemainingTime;

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

	public Guid InviterId
	{
		get { return m_guidInviterId; }
	}

	public string InviterName
	{
		get { return m_strInviterName; }
	}

    public float RemainingTime
    {
        get { return m_flRemainingTime; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroGuildInvitation(PDHeroGuildInvitation heroGuildInvitation)
	{
		m_guidId = heroGuildInvitation.id;
		m_guidGuildId = heroGuildInvitation.guildId;
		m_strGuildName = heroGuildInvitation.guildName;
		m_guidInviterId = heroGuildInvitation.inviterId;
		m_strInviterName = heroGuildInvitation.inviterName;
        m_flRemainingTime = Time.realtimeSinceStartup + CsGameConfig.Instance.GuildInvitationLifetime;
    }
}
