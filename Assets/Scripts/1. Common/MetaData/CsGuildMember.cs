using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-09)
//---------------------------------------------------------------------------------------------------

public class CsGuildMember
{
	Guid m_guidId;
	string m_strName;
	int m_nLevel;
	int m_nVipLevel;
	int m_nTotalContributionPoint;
	CsGuildMemberGrade m_csGuildMemberGrade;
	bool m_bIsLoggedIn;
	float m_flLogoutElapsedTime;
	CsJob m_csJob;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int VipLevel
	{
		get { return m_nVipLevel; }
	}

	public int TotalContributionPoint
	{
		get { return m_nTotalContributionPoint; }
	}

	public CsGuildMemberGrade GuildMemberGrade
	{
		get { return m_csGuildMemberGrade; }
	}

	public bool IsLoggedIn
	{
		get { return m_bIsLoggedIn; }
	}

	public float LogoutElapsedTime
	{
		get { return m_flLogoutElapsedTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildMember(PDGuildMember guildMember)
	{
		m_guidId = guildMember.id;
		m_strName = guildMember.name;
		m_nLevel = guildMember.level;
		m_nVipLevel = guildMember.vipLevel;
		m_nTotalContributionPoint = guildMember.totalContributionPoint;
		m_csGuildMemberGrade = CsGameData.Instance.GetGuildMemberGrade(guildMember.memberGrade);
		m_bIsLoggedIn = guildMember.isLoggedIn;
		m_flLogoutElapsedTime = guildMember.logoutElapsedTime;
		m_csJob = CsGameData.Instance.GetJob(guildMember.jobId);
	}
}
