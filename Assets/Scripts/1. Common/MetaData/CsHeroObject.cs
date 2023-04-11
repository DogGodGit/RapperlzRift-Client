//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-05)
//---------------------------------------------------------------------------------------------------

public enum EnHeroObjectType
{
	MainGear = 1,
	SubGear = 2,
	Item = 3,
	MountGear = 4
}

public class CsHeroObject
{
	EnHeroObjectType m_enHeroObjectType;

	//---------------------------------------------------------------------------------------------------
	public EnHeroObjectType HeroObjectType
	{
		get { return m_enHeroObjectType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroObject(EnHeroObjectType enHeroObjectType)
	{
		m_enHeroObjectType = enHeroObjectType;
	}
}
