using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsDiaShopProduct
{
	int m_nProductId;
	CsDiaShopCategory m_csDiaShopCategory;
	int m_nCostumeProductType;       // 1:코스튬,2:효과,3:탈것
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nTagType;					// 0:일반,1:신품,2:한정,3:인기,4:특가,5:구매횟수한정
	int m_nMoneyType;				// 1:다이아(귀속/비귀속),2:비귀속다이아,3:아이템
	CsItem m_csItemMoney;
	int m_nOriginalPrice;
	int m_nPrice;
	int m_nBuyLimitType;            // 1:일일, 2:누적
	int m_nBuyLimitCount;           // 0:무제한	
	int m_nPeriodType;				// 0:무제한,1:기간,2:요일
	DateTime m_dtPeriodStartTime;
	DateTime m_dtPeriodEndTime;
	int m_nPeriodDayOfWeek;
	bool m_bRecommended;
	bool m_bIsLimitEdition;
	int m_nCategorySortNo;
	int m_nLimitEditionSortNo;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public CsDiaShopCategory DiaShopCategory
	{
		get { return m_csDiaShopCategory; }
	}

	public int CostumeProductType
	{
		get { return m_nCostumeProductType; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int TagType
	{
		get { return m_nTagType; }
	}

	public int MoneyType
	{
		get { return m_nMoneyType; }
	}

	public CsItem MoneyItem
	{
		get { return m_csItemMoney; }
	}

	public int OriginalPrice
	{
		get { return m_nOriginalPrice; }
	}

	public int Price
	{
		get { return m_nPrice; }
	}

	public int BuyLimitType
	{
		get { return m_nBuyLimitType; }
	}

	public int BuyLimitCount
	{
		get { return m_nBuyLimitCount; }
	}

	public int PeriodType
	{
		get { return m_nPeriodType; }
	}

	public DateTime PeriodStartTime
	{
		get { return m_dtPeriodStartTime; }
	}

	public DateTime PeriodEndTime
	{
		get { return m_dtPeriodEndTime; }
	}

	public int PeriodDayOfWeek
	{
		get { return m_nPeriodDayOfWeek; }
	}

	public bool Recommended
	{
		get { return m_bRecommended; }
	}

	public bool IsLimitEdition
	{
		get { return m_bIsLimitEdition; }
	}

	public int CategorySortNo
	{
		get { return m_nCategorySortNo; }
	}

	public int LimitEditionSortNo
	{
		get { return m_nLimitEditionSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDiaShopProduct(WPDDiaShopProduct diaShopProduct)
	{
		m_nProductId = diaShopProduct.productId;
		m_csDiaShopCategory = CsGameData.Instance.GetDiaShopCategory(diaShopProduct.categoryId);
		m_nCostumeProductType = diaShopProduct.costumeProductType;
		m_csItem = CsGameData.Instance.GetItem(diaShopProduct.itemId);
		m_bItemOwned = diaShopProduct.itemOwned;
		m_nTagType = diaShopProduct.tagType;
		m_nMoneyType = diaShopProduct.moneyType;
		m_csItemMoney = CsGameData.Instance.GetItem(diaShopProduct.moneyItemId);
		m_nOriginalPrice = diaShopProduct.originalPrice;
		m_nPrice = diaShopProduct.price;
		m_nBuyLimitType = diaShopProduct.buyLimitType;
		m_nBuyLimitCount = diaShopProduct.buyLimitCount;
		m_nPeriodType = diaShopProduct.periodType;
		m_dtPeriodStartTime = diaShopProduct.periodStartTime;
		m_dtPeriodEndTime = diaShopProduct.periodEndTime;
		m_nPeriodDayOfWeek = diaShopProduct.periodDayOfWeek;
		m_bRecommended = diaShopProduct.recommended;
		m_bIsLimitEdition = diaShopProduct.isLimitEdition;
		m_nCategorySortNo = diaShopProduct.categorySortNo;
		m_nLimitEditionSortNo = diaShopProduct.limitEditionSortNo;
	}
}
