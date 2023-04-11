using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementMainCategory : IComparable
{
	int m_nMainCategoryId;
	string m_strName;
	int m_nSortNo;

	List<CsImprovementMainCategoryContent> m_listCsImprovementMainCategoryContent;
	List<CsImprovementSubCategory> m_listCsImprovementSubCategory;

	//---------------------------------------------------------------------------------------------------
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

	public List<CsImprovementMainCategoryContent> ImprovementMainCategoryContentList
	{
		get { return m_listCsImprovementMainCategoryContent; }
	}

	public List<CsImprovementSubCategory> ImprovementSubCategoryList
	{
		get { return m_listCsImprovementSubCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementMainCategory(WPDImprovementMainCategory improvementMainCategory)
	{
		m_nMainCategoryId = improvementMainCategory.mainCategoryId;
		m_strName = CsConfiguration.Instance.GetString(improvementMainCategory.nameKey);
		m_nSortNo = improvementMainCategory.sortNo;

		m_listCsImprovementMainCategoryContent = new List<CsImprovementMainCategoryContent>();
		m_listCsImprovementSubCategory = new List<CsImprovementSubCategory>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementSubCategory GetImprovementSubCategory(int nSubCategoryId)
	{
		for (int i = 0; i < m_listCsImprovementSubCategory.Count; i++)
		{
			if (m_listCsImprovementSubCategory[i].SubCategoryId == nSubCategoryId)
				return m_listCsImprovementSubCategory[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementMainCategoryContent GetImprovementMainCategoryContent(int nMainCategoryId)
	{
		for (int i = 0; i < m_listCsImprovementMainCategoryContent.Count; i++)
		{
			if (m_listCsImprovementMainCategoryContent[i].MainCategoryId == nMainCategoryId)
				return m_listCsImprovementMainCategoryContent[i];
		}

		return null;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsImprovementMainCategory)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
