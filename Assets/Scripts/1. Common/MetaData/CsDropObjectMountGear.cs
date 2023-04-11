using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-25)
//---------------------------------------------------------------------------------------------------

public class CsDropObjectMountGear : CsDropObject
{
	CsMountGear m_csMountGear;
	bool m_bOwned;

	//---------------------------------------------------------------------------------------------------
	public CsMountGear MountGear
	{
		get { return m_csMountGear; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDropObjectMountGear(int nType, PDHeroMountGear heroMountGear)
		: base(nType)
	{
		m_csMountGear = CsGameData.Instance.GetMountGear(heroMountGear.mountGearId);
		m_bOwned = heroMountGear.owned;
	}
}
