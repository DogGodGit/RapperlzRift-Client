using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-09)
//---------------------------------------------------------------------------------------------------

public class CsSimpleGuild
{
	Guid m_guidId;
	string m_strName;
	int m_nLevel;
	string m_strNotice;
	Guid m_guidMasterId;
	string m_strMasterName;
	int m_nMemberCount;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public string Notice
	{
		get { return m_strNotice; }
	}

	public Guid MasterId
	{
		get { return m_guidMasterId; }
	}

	public string MasterName
	{
		get { return m_strMasterName; }
	}

	public int MemberCount
	{
		get { return m_nMemberCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleGuild(PDSimpleGuild simpleGuild)
	{
		m_guidId = simpleGuild.id;
		m_strName = simpleGuild.name;
		m_nLevel = simpleGuild.level;
		m_strNotice = simpleGuild.notice;

		m_guidMasterId = simpleGuild.masterId;
		m_strMasterName = simpleGuild.masterName;

		m_nMemberCount = simpleGuild.memberCount;
	}
}
