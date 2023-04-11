using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsAccomplishmentCategory
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
	public CsAccomplishmentCategory(WPDAccomplishmentCategory accomplishmentCategory)
	{
		m_nCategoryId = accomplishmentCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(accomplishmentCategory.nameKey);
	}
}
