using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-02)
//---------------------------------------------------------------------------------------------------

public class CsGuildRanking
{
	int m_nRanking;
	CsNation m_csNation;
	Guid m_guidGuildId;
	string m_strGuildName;
	Guid m_guidGuildMasterId;
	string m_strGuildMasterName;
	long m_lMight;

	//---------------------------------------------------------------------------------------------------
	public int Ranking
	{
		get { return m_nRanking; }
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public Guid GuildId
	{
		get { return m_guidGuildId; }
	}

	public string GuildName
	{
		get { return m_strGuildName; }
	}

	public Guid GuildMasterId
	{
		get { return m_guidGuildMasterId; }
	}

	public string GuildMasterName
	{
		get { return m_strGuildMasterName; }
	}

	public long Might
	{
		get { return m_lMight; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildRanking(PDGuildRanking guildRanking)
	{
		m_nRanking = guildRanking.ranking;
		m_csNation = CsGameData.Instance.GetNation(guildRanking.nationId);
		m_guidGuildId = guildRanking.guildId;
		m_strGuildName = guildRanking.guildName;
		m_guidGuildMasterId = guildRanking.guildMasterId;
		m_strGuildMasterName = guildRanking.guildMasterName;
		m_lMight = guildRanking.might;

	}
}
