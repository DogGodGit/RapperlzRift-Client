using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsSimpleParty
{
	Guid m_guid;
	CsSimpleHero m_csSimpleHeroMaster;
	int m_nMemberCount;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public CsSimpleHero Master
	{
		get { return m_csSimpleHeroMaster; }
	}

	public int MemberCount
	{
		get { return m_nMemberCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleParty(PDSimpleParty simpleParty)
	{
		m_guid = simpleParty.id;
		m_csSimpleHeroMaster = new CsSimpleHero(simpleParty.master);
		m_nMemberCount = simpleParty.memberCount;
	}
}
