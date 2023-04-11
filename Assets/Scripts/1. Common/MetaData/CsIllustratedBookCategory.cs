using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookCategory
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
	public CsIllustratedBookCategory(WPDIllustratedBookCategory illustratedBookCategory)
	{
		m_nCategoryId = illustratedBookCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(illustratedBookCategory.nameKey);
	}
}
