using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsTitleCategory
{
	int m_nCategoryId;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public int CategoryId
	{
		get { return m_nCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitleCategory(WPDTitleCategory titleCategory)
	{
		m_nCategoryId = titleCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(titleCategory.nameKey);
	}
}
