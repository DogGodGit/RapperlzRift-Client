using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-16)
//---------------------------------------------------------------------------------------------------

public class CsInventoryObjectItem : CsInventoryObject
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
	public CsInventoryObjectItem(PDItemInventoryObject itemInventoryObject)
		:base(itemInventoryObject.type)
	{
		m_csItem = CsGameData.Instance.GetItem(itemInventoryObject.itemId);
		m_bOwned = itemInventoryObject.owned;
		m_nCount = itemInventoryObject.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsInventoryObjectItem(int nType, int nItemId, bool bOwned, int nCount)
		: base(nType)
	{
		m_csItem = CsGameData.Instance.GetItem(nItemId);
		m_bOwned = bOwned;
		m_nCount = nCount;
	}
}
