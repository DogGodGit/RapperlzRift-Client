using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-07)
//---------------------------------------------------------------------------------------------------

public class CsHeroBase
{
	Guid m_guidHeroId;
	string m_strName;
	int m_nJobId;
	CsJob m_csJob;
	CsNation m_csNation;
	int m_nLevel;
	int m_nMaxHp;  
	int m_nHp;
	int m_nRankNo;  

	protected CsJobLevel m_csJobLevel;

	//---------------------------------------------------------------------------------------------------
	public Guid HeroId
	{
		get { return m_guidHeroId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public int JobId
	{
		get { return m_nJobId; }
		set
		{
			m_nJobId = value;
			m_csJob = CsGameData.Instance.GetJob(m_nJobId);
			m_csJobLevel = m_csJob.GetJobLevel(Level);
		}
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public int Level
	{
		get { return m_csJobLevel.Level; }
		set { m_csJobLevel = m_csJob.GetJobLevel(value); }
	}

	public CsJobLevel JobLevel
	{
		get { return m_csJobLevel; }
	}

	public int Hp
	{
		get { return m_nHp; }
		set { m_nHp = value; }
	}

	public int MaxHp
	{
		get { return m_nMaxHp; }
		set { m_nMaxHp = value; }
	}

	public int RankNo      
	{
		get { return m_nRankNo; }
		set	{ m_nRankNo = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBase(Guid guidHeroId, string strName, int nNationId, int nJobId, int nLevel, int nHp = 0, int nMaxHp = 0, int nRank = 0)
	{
		m_guidHeroId = guidHeroId;
		m_strName = strName;
		m_csNation = CsGameData.Instance.GetNation(nNationId);
		m_nJobId = nJobId;
		m_csJob = CsGameData.Instance.GetJob(nJobId);
		m_csJobLevel = m_csJob.GetJobLevel(nLevel);

		m_nMaxHp = nMaxHp;
		m_nHp = nHp;
		m_nRankNo = nRank;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBase(PDHero pdHero)
	{
		m_guidHeroId = pdHero.id;
		m_strName = pdHero.name;
		m_csNation = CsGameData.Instance.GetNation(pdHero.nationId);
		m_nJobId = pdHero.jobId;
		m_csJob = CsGameData.Instance.GetJob(pdHero.jobId);
		m_csJobLevel = m_csJob.GetJobLevel(pdHero.level);

		m_nMaxHp = pdHero.maxHP;
		m_nHp = pdHero.hp;
		m_nRankNo = pdHero.rankNo;
	}
}
