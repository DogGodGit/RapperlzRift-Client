using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsRankingBase
{
	int m_nRanking;
	Guid m_guildHeroId;
	CsNation m_csNation;
	CsJob m_csJob;
	string m_strName;
	int m_nLevel;

	//---------------------------------------------------------------------------------------------------
	public int Ranking
	{
		get { return m_nRanking; }
	}

	public Guid HeroId
	{
		get { return m_guildHeroId; }
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRankingBase(int nRanking, Guid guidHeroId, int nNationId, int nJobId, string strName, int nLevel)
	{
		m_nRanking = nRanking;
		m_guildHeroId = guidHeroId;
		m_csNation = CsGameData.Instance.GetNation(nNationId);
		m_csJob = CsGameData.Instance.GetJob(nJobId);
		m_strName = strName;
		m_nLevel = nLevel;
	}
}
