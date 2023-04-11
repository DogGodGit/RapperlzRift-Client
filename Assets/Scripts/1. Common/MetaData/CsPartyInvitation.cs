using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsPartyInvitation
{
	long m_lNo;
	Guid m_guidPartyId;
	Guid m_guidTargetId;
	string m_strTargetName;
	Guid m_guidInviterId;
	string m_strInviterName;
    float m_flRemainingTime;

    //---------------------------------------------------------------------------------------------------
    public long No
	{
		get { return m_lNo; }
	}

	public Guid PartyId
	{
		get { return m_guidPartyId; }
	}

	public Guid TargetId
	{
		get { return m_guidTargetId; }
	}

	public string TargetName
	{
		get { return m_strTargetName; }
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
    public CsPartyInvitation(PDPartyInvitation partyInvitation)
	{
		m_lNo = partyInvitation.no;
		m_guidPartyId = partyInvitation.partyId;
		m_guidTargetId = partyInvitation.targetId;
		m_strTargetName = partyInvitation.targetName;
		m_guidInviterId = partyInvitation.inviterId;
		m_strInviterName = partyInvitation.inviterName;
        m_flRemainingTime = Time.realtimeSinceStartup + CsGameConfig.Instance.PartyInvitationLifetime;
    }
}
