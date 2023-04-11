using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public class CsAttrCategory
{
	int m_nAttrCategoryId;              // 속성카테고리ID
	string m_strName;                   // 이름

	//---------------------------------------------------------------------------------------------------
	public int AttrCategoryId
	{
		get { return m_nAttrCategoryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public CsAttrCategory(WPDAttrCategory attrCategory)
	{
		m_nAttrCategoryId = attrCategory.attrCategoryId;
		m_strName = CsConfiguration.Instance.GetString(attrCategory.nameKey);
	}

}
