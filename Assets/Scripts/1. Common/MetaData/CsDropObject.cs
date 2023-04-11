using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public enum EnDropObjectType
{
	MainGear = 1,
	Item = 2,
	MountGear = 3,
}

public class CsDropObject
{
	int m_nType;

	//---------------------------------------------------------------------------------------------------
	public EnDropObjectType DropObjectTypeType
	{
		get { return (EnDropObjectType)m_nType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDropObject(int nType)
	{
		m_nType = nType;
	}
}
