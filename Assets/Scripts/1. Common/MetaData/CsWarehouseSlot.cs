using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseSlot
{
	int m_nIndex;
	CsWarehouseObject m_csWarehouseObject;

	//---------------------------------------------------------------------------------------------------
	public int Index
	{
		get { return m_nIndex; }
	}

	public EnWarehouseObjectType EnType
	{
		get { return m_csWarehouseObject.EnType; }
	}

	public CsWarehouseObjectMainGear WarehouseObjectMainGear
	{
		get { return (CsWarehouseObjectMainGear)m_csWarehouseObject; }
	}

	public CsWarehouseObjectSubGear WarehouseObjectSubGear
	{
		get { return (CsWarehouseObjectSubGear)m_csWarehouseObject; }
	}

	public CsWarehouseObjectItem WarehouseObjectItem
	{
		get { return (CsWarehouseObjectItem)m_csWarehouseObject; }
	}

	public CsWarehouseObjectMountGear WarehouseObjectMountGear
	{
		get { return (CsWarehouseObjectMountGear)m_csWarehouseObject; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseSlot(PDWarehouseSlot warehouseSlot)
	{
		m_nIndex = warehouseSlot.index;

		switch ((EnWarehouseObjectType)warehouseSlot.warehouseObject.type)
		{
			case EnWarehouseObjectType.MainGear:
				m_csWarehouseObject = new CsWarehouseObjectMainGear((PDMainGearWarehouseObject)warehouseSlot.warehouseObject);
				break;

			case EnWarehouseObjectType.SubGear:
				m_csWarehouseObject = new CsWarehouseObjectSubGear((PDSubGearWarehouseObject)warehouseSlot.warehouseObject);
				break;

			case EnWarehouseObjectType.Item:
				m_csWarehouseObject = new CsWarehouseObjectItem((PDItemWarehouseObject)warehouseSlot.warehouseObject);
				break;

			case EnWarehouseObjectType.MountGear:
				m_csWarehouseObject = new CsWarehouseObjectMountGear((PDMountGearWarehouseObject)warehouseSlot.warehouseObject);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseSlot(int nIndex, PDWarehouseObject warehouseObject)
	{
		m_nIndex = nIndex;

		switch ((EnWarehouseObjectType)warehouseObject.type)
		{
			case EnWarehouseObjectType.MainGear:
				m_csWarehouseObject = new CsWarehouseObjectMainGear((PDMainGearWarehouseObject)warehouseObject);
				break;

			case EnWarehouseObjectType.SubGear:
				m_csWarehouseObject = new CsWarehouseObjectSubGear((PDSubGearWarehouseObject)warehouseObject);
				break;

			case EnWarehouseObjectType.Item:
				m_csWarehouseObject = new CsWarehouseObjectItem((PDItemWarehouseObject)warehouseObject);
				break;

			case EnWarehouseObjectType.MountGear:
				m_csWarehouseObject = new CsWarehouseObjectMountGear((PDMountGearWarehouseObject)warehouseObject);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseSlot(int nIndex, CsWarehouseObject csWarehouseObject)
	{
		m_nIndex = nIndex;
		m_csWarehouseObject = csWarehouseObject;
	}
}
