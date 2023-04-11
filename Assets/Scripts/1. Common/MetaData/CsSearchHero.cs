using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-02)
//---------------------------------------------------------------------------------------------------

public class CsSearchHero
{
	Guid m_guidHeroId;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public Guid HeroId
	{
		get { return m_guidHeroId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSearchHero(PDSearchHero searchHero)
	{
		m_guidHeroId = searchHero.heroId;
		m_strName = searchHero.name;
	}
}
