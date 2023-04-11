using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCollectionCategory
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
	public CsCreatureCardCollectionCategory(WPDCreatureCardCollectionCategory creatureCardCollectionCategory)
	{
		m_nCategoryId = creatureCardCollectionCategory.categoryId;
		m_strName = CsConfiguration.Instance.GetString(creatureCardCollectionCategory.nameKey);
	}
}
