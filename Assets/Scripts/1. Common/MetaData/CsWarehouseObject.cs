using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public enum EnWarehouseObjectType
{
	MainGear = 1,
	SubGear = 2,
	Item = 3,
	MountGear = 4
}

public class CsWarehouseObject
{
	int m_nType;

	//---------------------------------------------------------------------------------------------------
	public EnWarehouseObjectType EnType
	{
		get { return (EnWarehouseObjectType)m_nType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObject(PDWarehouseObject inventoryObject)
	{
		m_nType = inventoryObject.type;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObject(int nType)
	{
		m_nType = nType;
	}
}
