using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseObjectSubGear : CsWarehouseObject
{
	CsHeroSubGear m_csHeroSubGear;

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear HeroSubGear
	{
		get { return m_csHeroSubGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectSubGear(PDSubGearWarehouseObject subGearWarehouseObject)
		: base(subGearWarehouseObject.type)
	{
		m_csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(subGearWarehouseObject.subGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectSubGear(int nType, int nSubGearId)
		: base(nType)
	{
		m_csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
	}
}
