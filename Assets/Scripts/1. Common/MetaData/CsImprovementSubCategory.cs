using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementSubCategory : IComparable
{
	int m_nSubCategoryId;
	int m_nMainCategoryId;
	string m_strName;
	int m_nSortNo;

	List<CsImprovementSubCategoryContent> m_listCsImprovementSubCategoryContent;

	//---------------------------------------------------------------------------------------------------
	public int SubCategoryId
	{
		get { return m_nSubCategoryId; }
	}

	public int MainCategoryId
	{
		get { return m_nMainCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsImprovementSubCategoryContent> ImprovementSubCategoryContentList
	{
		get { return m_listCsImprovementSubCategoryContent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementSubCategory(WPDImprovementSubCategory improvementSubCategory)
	{
		m_nSubCategoryId = improvementSubCategory.subCategoryId;
		m_nMainCategoryId = improvementSubCategory.mainCategoryId;
		m_strName = CsConfiguration.Instance.GetString(improvementSubCategory.nameKey);
		m_nSortNo = improvementSubCategory.sortNo;

		m_listCsImprovementSubCategoryContent = new List<CsImprovementSubCategoryContent>();
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsImprovementSubCategory)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
