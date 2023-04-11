using System;
using ClientCommon; 

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsRanking : CsRankingBase
{
	long m_lBattlePower;
	long m_lExp;
	int m_nExploitPoint;

	//---------------------------------------------------------------------------------------------------
	public long BattlePower
	{
		get { return m_lBattlePower; }
	}

	public long Exp
	{
		get { return m_lExp; }
	}

	public int ExploitPoint
	{
		get { return m_nExploitPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRanking(PDRanking ranking) 
		: base (ranking.ranking, ranking.heroId, ranking.nationId, ranking.jobId, ranking.name, ranking.level)
	{
		m_lBattlePower = ranking.battlePower;
		m_lExp = ranking.exp;
		m_nExploitPoint = ranking.exploitPoint;
	}
}
