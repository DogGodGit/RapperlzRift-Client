using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public class CsInventoryObjectMainGear : CsInventoryObject
{
	CsHeroMainGear m_csHeroMainGear;

	//---------------------------------------------------------------------------------------------------
	public CsHeroMainGear HeroMainGear
	{
		get { return m_csHeroMainGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectMainGear(PDMainGearInventoryObject mainGearInventoryObject)
		: base (mainGearInventoryObject.type)
	{
		m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(mainGearInventoryObject.heroMainGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectMainGear(int nType, Guid guidMainGear)
		:  base(nType)
	{
		m_csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidMainGear);
	}
}
