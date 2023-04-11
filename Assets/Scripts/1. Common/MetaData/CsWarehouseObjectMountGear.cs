using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseObjectMountGear : CsWarehouseObject
{
	CsHeroMountGear m_csHeroMountGear;

	//---------------------------------------------------------------------------------------------------

	public CsHeroMountGear HeroMountGear
	{
		get { return m_csHeroMountGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectMountGear(PDMountGearWarehouseObject mountGearWarehouseObject)
		: base (mountGearWarehouseObject.type)
	{
		m_csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(mountGearWarehouseObject.heroMountGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectMountGear(int nType, CsHeroMountGear csHeroMountGear)
		: base(nType)
	{
		m_csHeroMountGear = csHeroMountGear;
	}
}
