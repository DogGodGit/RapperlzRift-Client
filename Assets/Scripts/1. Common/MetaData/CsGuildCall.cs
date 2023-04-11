using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-02)
//---------------------------------------------------------------------------------------------------

public class CsGuildCall
{
	long m_lId;
	Guid m_guidCallerId;
	string m_strCallerName;
	int m_nCallerMemberGrade;
	float m_flRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public long Id
	{
		get { return m_lId; }
	}

	public Guid CallerId
	{
		get { return m_guidCallerId; }
	}

	public string CallerName
	{
		get { return m_strCallerName; }
	}

	public int CallerMemberGrade
	{
		get { return m_nCallerMemberGrade; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildCall(PDGuildCall guildCall)
	{
		m_lId = guildCall.id;
		m_guidCallerId = guildCall.callerId;
		m_strCallerName = guildCall.callerName;
		m_nCallerMemberGrade = guildCall.callerMemberGrade;
        m_flRemainingTime = Time.realtimeSinceStartup + CsGameConfig.Instance.GuildCallLifetime;
	}
}
