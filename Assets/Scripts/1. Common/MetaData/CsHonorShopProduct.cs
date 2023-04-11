using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsHonorShopProduct : IComparable
{
	int m_nProductId;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nRequiredHonorPoint;
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

	public int RequiredHonorPoint
	{
		get { return m_nRequiredHonorPoint; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHonorShopProduct(WPDHonorShopProduct honorShopProduct)
	{
		m_nProductId = honorShopProduct.productId;
		m_csItem = CsGameData.Instance.GetItem(honorShopProduct.itemId);
		m_bItemOwned = honorShopProduct.itemOwned;
		m_nRequiredHonorPoint = honorShopProduct.requiredHonorPoint;
		m_nSortNo = honorShopProduct.sortNo;
	}


	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsHonorShopProduct)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
