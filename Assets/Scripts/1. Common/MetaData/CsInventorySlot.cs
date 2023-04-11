using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public enum EnSortOrder
{
	Ascending = 1,
	Descending = -1
}

public class CsInventorySlot
{
	int m_nIndex;
	CsInventoryObject m_csInventoryObject;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public EnInventoryObjectType EnType
	{
		get { return m_csInventoryObject.EnType; }
	}

	public CsInventoryObjectMainGear InventoryObjectMainGear
	{
		get { return (CsInventoryObjectMainGear)m_csInventoryObject; }
	}

	public CsInventoryObjectSubGear InventoryObjectSubGear
	{
		get { return (CsInventoryObjectSubGear)m_csInventoryObject; }
	}

	public CsInventoryObjectItem InventoryObjectItem
	{
		get { return (CsInventoryObjectItem)m_csInventoryObject; }
	}

	public CsInventoryObjectMountGear InventoryObjectMountGear
	{
		get { return (CsInventoryObjectMountGear)m_csInventoryObject; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot(PDInventorySlot inventorySlot)
	{
		m_nIndex = inventorySlot.index;

		switch ((EnInventoryObjectType)inventorySlot.inventoryObject.type)
		{
			case EnInventoryObjectType.MainGear:
				m_csInventoryObject = new CsInventoryObjectMainGear((PDMainGearInventoryObject)inventorySlot.inventoryObject);
				break;

			case EnInventoryObjectType.SubGear:
				m_csInventoryObject = new CsInventoryObjectSubGear((PDSubGearInventoryObject)inventorySlot.inventoryObject);
				break;

			case EnInventoryObjectType.Item:
				m_csInventoryObject = new CsInventoryObjectItem((PDItemInventoryObject)inventorySlot.inventoryObject);
				break;

			case EnInventoryObjectType.MountGear:
				m_csInventoryObject = new CsInventoryObjectMountGear((PDMountGearInventoryObject)inventorySlot.inventoryObject);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot(int nIndex, PDInventoryObject inventoryObject)
	{
		m_nIndex = nIndex;

		switch ((EnInventoryObjectType)inventoryObject.type)
		{
			case EnInventoryObjectType.MainGear:
				m_csInventoryObject = new CsInventoryObjectMainGear((PDMainGearInventoryObject)inventoryObject);
				break;

			case EnInventoryObjectType.SubGear:
				m_csInventoryObject = new CsInventoryObjectSubGear((PDSubGearInventoryObject)inventoryObject);
				break;

			case EnInventoryObjectType.Item:
				m_csInventoryObject = new CsInventoryObjectItem((PDItemInventoryObject)inventoryObject);
				break;

			case EnInventoryObjectType.MountGear:
				m_csInventoryObject = new CsInventoryObjectMountGear((PDMountGearInventoryObject)inventoryObject);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventorySlot(int nIndex, CsInventoryObject csInventoryObject)
	{
		m_nIndex = nIndex;
		m_csInventoryObject = csInventoryObject;
	}
}
