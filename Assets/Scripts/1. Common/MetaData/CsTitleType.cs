using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-18)
//---------------------------------------------------------------------------------------------------

public class CsTitleType
{
	int m_nType;
	CsTitleCategory m_csTitleCategory;
	string m_strName;

	//---------------------------------------------------------------------------------------------------
	public int Type
	{
		get { return m_nType; }
	}

	public CsTitleCategory TitleCategory
	{
		get { return m_csTitleCategory; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTitleType(WPDTitleType titleType)
	{
		m_nType = titleType.type;
		m_csTitleCategory = CsGameData.Instance.GetTitleCategory(titleType.categoryId);
		m_strName = CsConfiguration.Instance.GetString(titleType.nameKey);
	}
}
