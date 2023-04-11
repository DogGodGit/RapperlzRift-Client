using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-02)
//---------------------------------------------------------------------------------------------------

public class CsNpcShopProduct : IComparable
{
	int m_nProductId;
	int m_nShopId;
	int m_nCategoryId;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nRequiredItemCount;
	int m_nLimitCount;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public int ShopId
	{
		get { return m_nShopId; }
	}

	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int RequiredItemCount
	{
		get { return m_nRequiredItemCount; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShopProduct(WPDNpcShopProduct npcShopProduct)
	{
		m_nProductId = npcShopProduct.productId;
		m_nShopId = npcShopProduct.shopId;
		m_nCategoryId = npcShopProduct.categoryId;
		m_csItem = CsGameData.Instance.GetItem(npcShopProduct.itemId);
		m_bItemOwned = npcShopProduct.itemOwned;
		m_nRequiredItemCount = npcShopProduct.requiredItemCount;
		m_nLimitCount = npcShopProduct.limitCount;
		m_nSortNo = npcShopProduct.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsNpcShopProduct)obj).SortNo);
	}
	#endregion Interface(IComparable) implement

}
