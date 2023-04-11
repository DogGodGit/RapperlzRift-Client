using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-09)
//---------------------------------------------------------------------------------------------------

public class CsGuildApplication
{
	Guid m_guidId;

	Guid m_guidHeroId;
	string m_strHeroName;
	CsJob m_csJobIdHero;
	int m_nHeroLevel;
	int m_nHeroVipLevel;
	long m_lHeroBattlePower;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public Guid HeroId
	{
		get { return m_guidHeroId; }
	}

	public string HeroName
	{
		get { return m_strHeroName; }
	}

	public CsJob HeroJob
	{
		get { return m_csJobIdHero; }
	}

	public int HeroLevel
	{
		get { return m_nHeroLevel; }
	}

	public int HeroVipLevel
	{
		get { return m_nHeroVipLevel; }
	}

	public long HeroBattlePower
	{
		get { return m_lHeroBattlePower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildApplication(PDGuildApplication guildApplication)
	{
		m_guidId = guildApplication.id;

		m_guidHeroId = guildApplication.heroId;
		m_strHeroName = guildApplication.heroName;
		m_csJobIdHero = CsGameData.Instance.GetJob(guildApplication.heroJobId);
			
		m_nHeroLevel = guildApplication.heroLevel;
		m_nHeroVipLevel = guildApplication.heroVipLevel;
		m_lHeroBattlePower = guildApplication.heroBattlePower;
	}
}
