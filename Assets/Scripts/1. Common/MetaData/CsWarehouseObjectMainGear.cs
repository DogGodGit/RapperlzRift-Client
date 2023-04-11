using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseObjectMainGear : CsWarehouseObject
{
	CsHeroMainGear m_csHeroMainGear;

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear HeroMainGear
	{
		get { return m_csHeroMainGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectMainGear(PDMainGearWarehouseObject mainGearWarehouseObject)
		: base (mainGearWarehouseObject.type)
	{
		m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(mainGearWarehouseObject.heroMainGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectMainGear(int nType, Guid guidMainGear)
		:  base(nType)
	{
		m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidMainGear);
	}
}
