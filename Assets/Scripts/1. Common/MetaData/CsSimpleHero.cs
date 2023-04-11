using ClientCommon;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-07)
//---------------------------------------------------------------------------------------------------

public class CsSimpleHero : CsHeroBase
{
	long m_lBattlePower;
	int m_nVipLevel;
	int m_nNoblesseId;

	//---------------------------------------------------------------------------------------------------
	public long BattlePower
	{
		get { return m_lBattlePower; }
	}

	public int VipLevel
	{
		get { return m_nVipLevel; }
	}

	public int NoblesseId
	{
		get { return m_nNoblesseId; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleHero(PDSimpleHero simpleHero)
		: base(simpleHero.id, simpleHero.name, simpleHero.nationId, simpleHero.jobId, simpleHero.level)
	{
		m_lBattlePower = simpleHero.battlePower;
		m_nVipLevel = simpleHero.vipLevel;
		m_nNoblesseId = simpleHero.noblesseId;
	}
}
