using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-25)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookRanking : CsRankingBase
{
	int m_nExplorationPoint;

	//---------------------------------------------------------------------------------------------------
	public int ExplorationPoint
	{
		get { return m_nExplorationPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookRanking(PDIllustratedBookRanking illustratedBookRanking)
		: base (illustratedBookRanking.ranking, illustratedBookRanking.heroId, illustratedBookRanking.nationId, illustratedBookRanking.jobId, illustratedBookRanking.name, illustratedBookRanking.level)
	{
		m_nExplorationPoint = illustratedBookRanking.explorationPoint;
	}
}
