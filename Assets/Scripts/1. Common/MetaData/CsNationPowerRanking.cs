using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-17)
//---------------------------------------------------------------------------------------------------

public class CsNationPowerRanking
{
	int m_nRanking;
	int m_nNationId;
	int m_nNationPower;

	//---------------------------------------------------------------------------------------------------
	public int Ranking
	{
		get { return m_nRanking; }
	}
	
	public int NationId
	{
		get { return m_nNationId; }
	}

	public int NationPower
	{
		get { return m_nNationPower; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationPowerRanking(PDNationPowerRanking nationPowerRanking)
	{
		m_nRanking = nationPowerRanking.ranking;
		m_nNationId = nationPowerRanking.nationId;
		m_nNationPower = nationPowerRanking.nationPower;
	}
}
