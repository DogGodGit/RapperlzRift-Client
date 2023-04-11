using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementMainCategoryContent : IComparable
{
	int m_nMainCategoryId;
	CsImprovementContent m_csImprovementContent;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int MainCategoryId
	{
		get { return m_nMainCategoryId; }
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
	public CsImprovementMainCategoryContent(WPDImprovementMainCategoryContent improvementMainCategoryContent)
	{
		m_nMainCategoryId = improvementMainCategoryContent.mainCategoryId;
		m_csImprovementContent = CsGameData.Instance.GetImprovementContent(improvementMainCategoryContent.contentId);
		m_nSortNo = improvementMainCategoryContent.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsImprovementMainCategoryContent)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
