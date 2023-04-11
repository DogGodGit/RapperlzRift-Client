using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-16)
//---------------------------------------------------------------------------------------------------

public class CsInventoryObjectMountGear : CsInventoryObject
{
	CsHeroMountGear m_csHeroMountGear;

	//---------------------------------------------------------------------------------------------------

	public CsHeroMountGear HeroMountGear
	{
		get { return m_csHeroMountGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectMountGear(PDMountGearInventoryObject mountGearInventoryObject)
		: base (mountGearInventoryObject.type)
	{
		m_csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(mountGearInventoryObject.heroMountGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectMountGear(int nType, CsHeroMountGear csHeroMountGear)
		: base(nType)
	{
		m_csHeroMountGear = csHeroMountGear;
	}


}
