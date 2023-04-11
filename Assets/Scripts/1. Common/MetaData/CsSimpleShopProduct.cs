using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsSimpleShopProduct : IComparable
{
	int m_nProductId;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nSaleGold;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int SaleGold
	{
		get { return m_nSaleGold; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSimpleShopProduct(WPDSimpleShopProduct simpleShopProduct)
	{
		m_nProductId = simpleShopProduct.productId;
		m_csItem = CsGameData.Instance.GetItem(simpleShopProduct.itemId);
		m_bItemOwned = simpleShopProduct.itemOwned;
		m_nSaleGold = simpleShopProduct.saleGold;
		m_nSortNo = simpleShopProduct.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsSimpleShopProduct)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
