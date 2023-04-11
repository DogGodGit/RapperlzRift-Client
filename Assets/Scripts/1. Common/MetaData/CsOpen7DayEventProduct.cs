using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-16)
//---------------------------------------------------------------------------------------------------

public class CsOpen7DayEventProduct
{
	int m_nProductId;
	int m_nDay;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nItemCount;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public int Day
	{
		get { return m_nDay; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int ItemCount
	{
		get { return m_nItemCount; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventProduct(WPDOpen7DayEventProduct open7DayEventProduct)
	{
		m_nProductId = open7DayEventProduct.productId;
		m_nDay = open7DayEventProduct.day;
		m_csItem = CsGameData.Instance.GetItem(open7DayEventProduct.itemId);
		m_bItemOwned = open7DayEventProduct.itemOwned;
		m_nItemCount = open7DayEventProduct.itemCount;
		m_nRequiredDia = open7DayEventProduct.requiredDia;
	}

}
