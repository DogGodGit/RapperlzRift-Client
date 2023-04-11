using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardRanking : CsRankingBase
{
	int m_nCollectionFamePoint;

	//---------------------------------------------------------------------------------------------------
	public int CollectionFamePoint
	{
		get { return m_nCollectionFamePoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardRanking(PDCreatureCardRanking creatureCardRanking)
		: base (creatureCardRanking.ranking, creatureCardRanking.heroId, creatureCardRanking.nationId, creatureCardRanking.jobId, creatureCardRanking.name, creatureCardRanking.level)
	{
		m_nCollectionFamePoint = creatureCardRanking.collectionFamePoint;
	}

}
