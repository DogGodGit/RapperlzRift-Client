using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-13)
//---------------------------------------------------------------------------------------------------

public class CsGuildDailyObjectiveCompletionMember
{
	Guid m_guidId;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildDailyObjectiveCompletionMember(PDGuildDailyObjectiveCompletionMember guildDailyObjectiveCompletionMember)
	{
		m_guidId = guildDailyObjectiveCompletionMember.id;
		m_strName = guildDailyObjectiveCompletionMember.name;
	}
}
