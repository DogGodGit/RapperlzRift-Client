//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-05)
//---------------------------------------------------------------------------------------------------

public class CsHeroItem : CsHeroObject
{
	CsItem m_csItem;
	bool m_bOwned;
	int m_nCount;
	int m_nInventorySlotIndex;

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

	public int InventorySlotIndex
	{
		get { return m_nInventorySlotIndex; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroItem(CsItem csItem, bool bOwned, int nCount, int nInventorySlotIndex)
		: base(EnHeroObjectType.Item)
	{
		m_csItem = csItem;
		m_bOwned = bOwned;
		m_nCount = nCount;
		m_nInventorySlotIndex = nInventorySlotIndex;
	}
}
