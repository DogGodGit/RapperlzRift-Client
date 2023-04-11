using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsQuickMenu : IComparable
{
	int m_nMenuId;
	string m_strImageName;
	int m_nItemType;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int MenuId
	{
		get { return m_nMenuId; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int ItemType
	{
		get { return m_nItemType; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsQuickMenu(WPDQuickMenu quickMenu)
	{
		m_nMenuId = quickMenu.menuId;
		m_strImageName = quickMenu.imageName;
		m_nItemType = quickMenu.itemType;
		m_nSortNo = quickMenu.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsQuickMenu)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
