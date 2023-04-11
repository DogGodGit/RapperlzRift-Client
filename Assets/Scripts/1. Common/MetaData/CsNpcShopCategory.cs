using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-02)
//---------------------------------------------------------------------------------------------------

public class CsNpcShopCategory : IComparable
{
	int m_nShopId;
	int m_nCategoryId;
	string m_strName;
	CsItem m_csItemRequired;
	int m_nSortNo;

	List<CsNpcShopProduct> m_listCsNpcShopProduct;

	//---------------------------------------------------------------------------------------------------
	public int ShopId
	{
		get { return m_nShopId; }
	}

	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsItem RequiredItem
	{
		get { return m_csItemRequired; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsNpcShopProduct> NpcShopProductList
	{
		get { return m_listCsNpcShopProduct; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcShopCategory(WPDNpcShopCategory npcShopCategory)
	{
		m_nShopId = npcShopCategory.shopId;
		m_nCategoryId = npcShopCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(npcShopCategory.nameKey);
		m_csItemRequired = CsGameData.Instance.GetItem(npcShopCategory.requiredItemId);
		m_nSortNo = npcShopCategory.sortNo;

		m_listCsNpcShopProduct = new List<CsNpcShopProduct>();
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsNpcShopCategory)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
