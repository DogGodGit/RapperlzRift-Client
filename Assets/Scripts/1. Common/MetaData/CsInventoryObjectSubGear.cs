using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public class CsInventoryObjectSubGear : CsInventoryObject
{
	CsHeroSubGear m_csHeroSubGear;

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubGear HeroSubGear
	{
		get { return m_csHeroSubGear; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectSubGear(PDSubGearInventoryObject subGearInventoryObject)
		: base(subGearInventoryObject.type)
	{
		m_csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(subGearInventoryObject.subGearId);
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectSubGear(int nType, int nSubGearId)
		: base(nType)
	{
		m_csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
	}
}
