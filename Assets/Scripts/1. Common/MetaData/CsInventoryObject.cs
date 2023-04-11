using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public enum EnInventoryObjectType
{
	MainGear = 1,
	SubGear = 2,
	Item = 3,
	MountGear = 4
}

public class CsInventoryObject
{
	int m_nType;

	//---------------------------------------------------------------------------------------------------
	public EnInventoryObjectType EnType
	{
		get { return (EnInventoryObjectType)m_nType; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObject(PDInventoryObject inventoryObject)
	{
		m_nType = inventoryObject.type;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObject(int nType)
	{
		m_nType = nType;
	}
}
