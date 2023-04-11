using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public class CsItemSubCategory
{
	int m_nMainCategoryId;      // 메인카테고리ID
	int m_nSubCategoryId;		// 서브카테고리ID
	string m_strName;           // 이름

	//---------------------------------------------------------------------------------------------------
	public int MainCategoryId
	{
		get { return m_nMainCategoryId; }
	}

	public int SubCategoryId
	{
		get { return m_nSubCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemSubCategory(WPDItemSubCategory itemSubCategory)
	{
		m_nMainCategoryId = itemSubCategory.mainCategoryId;
		m_nSubCategoryId = itemSubCategory.subCategoryId;
		m_strName = CsConfiguration.Instance.GetString(itemSubCategory.nameKey);
	}
}
