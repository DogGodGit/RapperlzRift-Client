using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsItemMainCategory
{
	int m_nMainCategoryId;      // 메인카테고리ID
	string m_strName;           // 이름

	List<CsItemSubCategory> m_listCsItemSubCategory;

	//---------------------------------------------------------------------------------------------------
	public int MainCategoryId
	{
		get { return m_nMainCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsItemSubCategory> ItemSubCategoryList
	{
		get { return m_listCsItemSubCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemMainCategory(WPDItemMainCategory itemMainCategory)
	{
		m_nMainCategoryId = itemMainCategory.mainCategoryId;
		m_strName = CsConfiguration.Instance.GetString(itemMainCategory.nameKey);

		m_listCsItemSubCategory = new List<CsItemSubCategory>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemSubCategory GetItemSubCategory(int nSubCategoryId)
	{
		for (int i = 0; i < m_listCsItemSubCategory.Count; i++)
		{
			if (m_listCsItemSubCategory[i].SubCategoryId == nSubCategoryId)
				return m_listCsItemSubCategory[i];
		}

		return null;
	}

}
