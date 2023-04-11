using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsTodayTaskCategory
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
	public CsTodayTaskCategory(WPDTodayTaskCategory todayTaskCategory)
	{
		m_nCategoryId = todayTaskCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(todayTaskCategory.nameKey);
	}
}
