using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsDropObjectItem : CsDropObject
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
	public CsDropObjectItem(PDItemDropObject itemDropObject)
		: base(itemDropObject.type)
	{
		m_csItem = CsGameData.Instance.GetItem(itemDropObject.id);
		m_bOwned = itemDropObject.owned;
		m_nCount = itemDropObject.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsDropObjectItem(int nType, int nItemId, bool bOwned, int nCount)
		: base(nType)
	{
		m_csItem = CsGameData.Instance.GetItem(nItemId);
		m_bOwned = bOwned;
		m_nCount = nCount;
	}
}
