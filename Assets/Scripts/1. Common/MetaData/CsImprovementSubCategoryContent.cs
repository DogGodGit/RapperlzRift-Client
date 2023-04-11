using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementSubCategoryContent : IComparable
{
	int m_nSubCategoryId;
	CsImprovementContent m_csImprovementContent;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int SubCategoryId
	{
		get { return m_nSubCategoryId; }
	}

	public CsImprovementContent ImprovementContent
	{
		get { return m_csImprovementContent; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementSubCategoryContent(WPDImprovementSubCategoryContent improvementSubCategoryContent)
	{
		m_nSubCategoryId = improvementSubCategoryContent.subCategoryId;
		m_csImprovementContent = CsGameData.Instance.GetImprovementContent(improvementSubCategoryContent.contentId);
		m_nSortNo = improvementSubCategoryContent.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsImprovementSubCategoryContent)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
