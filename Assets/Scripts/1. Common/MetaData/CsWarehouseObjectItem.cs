using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsWarehouseObjectItem : CsWarehouseObject
{
	CsItem m_csItem;
	bool m_bOwned;
	int m_nCount;

	//---------------------------------------------------------------------------------------------------
	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool Owned
	{
		get { return m_bOwned; }
	}

	public int Count
	{
		get { return m_nCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectItem(PDItemWarehouseObject itemWarehouseObject)
		:base(itemWarehouseObject.type)
	{
		m_csItem = CsGameData.Instance.GetItem(itemWarehouseObject.itemId);
		m_bOwned = itemWarehouseObject.owned;
		m_nCount = itemWarehouseObject.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarehouseObjectItem(int nType, int nItemId, bool bOwned, int nCount)
		: base(nType)
	{
		m_csItem = CsGameData.Instance.GetItem(nItemId);
		m_bOwned = bOwned;
		m_nCount = nCount;
	}
}
