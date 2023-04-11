using System;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsParty
{
	Guid m_guid;
	CsPartyMember m_csPartyMemberMaster;
	float m_flCallRemainingCoolTime;
	List<CsPartyMember> m_listCsPartyMember;

	List<CsPartyApplication> m_listCsPartyApplication;  // 신청받은리스트
	List<CsPartyInvitation> m_listCsPartyInvitation;    // 초대리스트

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public CsPartyMember Master
	{
		get { return m_csPartyMemberMaster; }
		set { m_csPartyMemberMaster = value; }
	}

	public float CallRemainingCoolTime
	{
		get { return m_flCallRemainingCoolTime; }
		set { m_flCallRemainingCoolTime = value + Time.realtimeSinceStartup; }
	}

	public List<CsPartyMember> PartyMemberList
	{
		get { return m_listCsPartyMember; }
	}

	public List<CsPartyApplication> PartyApplicationList
	{
		get { return m_listCsPartyApplication; }
	}

	public List<CsPartyInvitation> PartyInvitationList
	{
		get { return m_listCsPartyInvitation; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsParty(PDParty party)
	{
		m_guid = party.id;
		m_flCallRemainingCoolTime = party.callRemainingCoolTime + Time.realtimeSinceStartup;

		m_listCsPartyMember = new List<CsPartyMember>();

		AddMembers(party.members);

		m_csPartyMemberMaster = GetMember(party.masterId);

		m_listCsPartyApplication = new List<CsPartyApplication>();
		m_listCsPartyInvitation = new List<CsPartyInvitation>();
	}

	//---------------------------------------------------------------------------------------------------
	public void AddMembers(PDPartyMember[] partyMembers)
	{
		m_listCsPartyMember.Clear();

		for (int i = 0; i < partyMembers.Length; i++)
		{
			CsPartyMember csPartyMember = new CsPartyMember(partyMembers[i]);
			m_listCsPartyMember.Add(csPartyMember);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveMember(Guid guidMemberId)
	{
		for (int i = 0; i < m_listCsPartyMember.Count; i++)
		{
			if (m_listCsPartyMember[i].Id == guidMemberId)
			{
				m_listCsPartyMember.RemoveAt(i);
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddMember(CsPartyMember csPartyMember)
	{
		m_listCsPartyMember.Add(csPartyMember);
	}

	//---------------------------------------------------------------------------------------------------
	public void UpdateMember(PDPartyMember partyMember)
	{
		CsPartyMember csPartyMember = GetMember(partyMember.id);

		if (csPartyMember != null)
		{
			csPartyMember.UpdatePartyMember(partyMember);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyMember GetMember(Guid guidMemberId)
	{
		for (int i = 0; i < m_listCsPartyMember.Count; i++)
		{
			if (m_listCsPartyMember[i].Id == guidMemberId)
				return m_listCsPartyMember[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddPartyApplication(CsPartyApplication csPartyApplication)
	{
		m_listCsPartyApplication.Add(csPartyApplication);
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyApplication GetPartyApplication(long lNo)
	{
		for (int i = 0; i < m_listCsPartyApplication.Count; i++)
		{
			if (m_listCsPartyApplication[i].No == lNo)
				return m_listCsPartyApplication[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemovePartyApplication(long lNo)
	{
		for (int i = 0; i < m_listCsPartyApplication.Count; i++)
		{
			if (m_listCsPartyApplication[i].No == lNo)
			{
				m_listCsPartyApplication.RemoveAt(i);
				return;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddPartyInvitation(CsPartyInvitation csPartyInvitation)
	{
		m_listCsPartyInvitation.Add(csPartyInvitation);
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyInvitation GetPartyInvitation(long lNo)
	{
		for (int i = 0; i < m_listCsPartyInvitation.Count; i++)
		{
			if (m_listCsPartyInvitation[i].No == lNo)
				return m_listCsPartyInvitation[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemovePartyInvitation(long lNo)
	{
		for (int i = 0; i < m_listCsPartyInvitation.Count; i++)
		{
			if (m_listCsPartyInvitation[i].No == lNo)
			{
				m_listCsPartyInvitation.RemoveAt(i);
				return;
			}
		}
	}
}
