using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsPresentPopularityPointRanking
{
	int m_nRanking;
	Guid m_guidHeroId;
	CsNation m_csNation;
	CsJob m_csJob;
	string m_strName;
	int m_nLevel;
	int m_nPopularityPoint;

	//---------------------------------------------------------------------------------------------------
	public int Ranking
	{
		get { return m_nRanking; }
	}

	public Guid HeroId
	{
		get { return m_guidHeroId; }
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

	public int PopularityPoint
	{
		get { return m_nPopularityPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPresentPopularityPointRanking(PDPresentPopularityPointRanking presentPopularityPointRanking)
	{
		m_nRanking = presentPopularityPointRanking.ranking;
		m_guidHeroId = presentPopularityPointRanking.heroId;
		m_csNation = CsGameData.Instance.GetNation(presentPopularityPointRanking.nationId);
		m_csJob = CsGameData.Instance.GetJob(presentPopularityPointRanking.jobId);
		m_strName = presentPopularityPointRanking.name;
		m_nLevel = presentPopularityPointRanking.level;
		m_nPopularityPoint = presentPopularityPointRanking.popularityPoint;
	}
}
