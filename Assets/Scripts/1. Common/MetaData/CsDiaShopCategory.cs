using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsDiaShopCategory : IComparable
{
	int m_nCategoryId;
	string m_strName;
	int m_nRequiredVipLevel;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RequiredVipLevel
	{
		get { return m_nRequiredVipLevel; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDiaShopCategory(WPDDiaShopCategory diaShopCategory)
	{
		m_nCategoryId = diaShopCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(diaShopCategory.nameKey);
		m_nRequiredVipLevel = diaShopCategory.requiredVipLevel;
		m_nSortNo = diaShopCategory.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsDiaShopCategory)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
