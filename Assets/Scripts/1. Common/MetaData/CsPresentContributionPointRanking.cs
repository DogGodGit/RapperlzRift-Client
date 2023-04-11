using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsPresentContributionPointRanking
{
	int m_nRanking;
	Guid m_guidHeroId;
	CsNation m_csNation;
	CsJob m_csJob;
	string m_strName;
	int m_nLevel;
	int m_nContributionPoint;

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

	public int ContributionPoint
	{
		get { return m_nContributionPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPresentContributionPointRanking(PDPresentContributionPointRanking presentContributionPointRanking)
	{
		m_nRanking = presentContributionPointRanking.ranking;
		m_guidHeroId = presentContributionPointRanking.heroId;
		m_csNation = CsGameData.Instance.GetNation(presentContributionPointRanking.nationId);
		m_csJob = CsGameData.Instance.GetJob(presentContributionPointRanking.jobId);
		m_strName = presentContributionPointRanking.name;
		m_nLevel = presentContributionPointRanking.level;
		m_nContributionPoint = presentContributionPointRanking.contributionPoint;
	}
}
