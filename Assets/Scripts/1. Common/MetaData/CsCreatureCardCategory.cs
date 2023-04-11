using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCategory
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
	public CsCreatureCardCategory(WPDCreatureCardCategory creatureCardCategory)
	{
		m_nCategoryId = creatureCardCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(creatureCardCategory.nameKey);
	}
}
