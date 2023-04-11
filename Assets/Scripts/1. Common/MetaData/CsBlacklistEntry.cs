using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsBlacklistEntry
{
	Guid m_guidHeroId;
	string m_strName;
	CsNation m_csNation;
	CsJob m_csJob;
	int m_nLevel;
	long m_lBattlePower;

	//---------------------------------------------------------------------------------------------------
	public Guid HeroId
	{
		get { return m_guidHeroId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set { m_nLevel = value; }
	}

	public long BattlePower
	{
		get { return m_lBattlePower; }
		set { m_lBattlePower = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlacklistEntry(PDBlacklistEntry blacklistEntry)
	{
		m_guidHeroId = blacklistEntry.heroId;
		m_strName = blacklistEntry.name;
		m_csNation = CsGameData.Instance.GetNation(blacklistEntry.nationId);
		m_csJob = CsGameData.Instance.GetJob(blacklistEntry.jobId);
		m_nLevel = blacklistEntry.level;
		m_lBattlePower = blacklistEntry.battlePower;
	}
}
