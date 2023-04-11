using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsCashProduct : IComparable
{
	int m_nProductId;
	string m_strName;
	CsInAppProduct m_csInAppProduct;
	string m_strImageName;
	int m_nType;            // 1:다이아, 2:아이템
	int m_nUnOwnDia;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nItemCount;
	int m_nVipPoint;
	int m_nFirstPurchaseBonusUnOwnDia;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsInAppProduct InAppProduct
	{
		get { return m_csInAppProduct; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int UnOwnDia
	{
		get { return m_nUnOwnDia; }
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

	public int VipPoint
	{
		get { return m_nVipPoint; }
	}

	public int FirstPurchaseBonusUnOwnDia
	{
		get { return m_nFirstPurchaseBonusUnOwnDia; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCashProduct(WPDCashProduct cashProduct)
	{
		m_nProductId = cashProduct.productId;
		m_strName = CsConfiguration.Instance.GetString(cashProduct.nameKey);
		m_csInAppProduct = CsGameData.Instance.GetInAppProduct(cashProduct.inAppProductKey);
		m_strImageName = cashProduct.imageName;
		m_nType = cashProduct.type;  
		m_nUnOwnDia = cashProduct.unOwnDia;
		m_csItem = CsGameData.Instance.GetItem(cashProduct.itemId);
		m_bItemOwned = cashProduct.itemOwned;
		m_nItemCount = cashProduct.itemCount;
		m_nVipPoint = cashProduct.vipPoint;
		m_nFirstPurchaseBonusUnOwnDia = cashProduct.firstPurchaseBonusUnOwnDia;
		m_nSortNo = cashProduct.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsCashProduct)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
