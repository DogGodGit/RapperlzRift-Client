using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBookType
{
	int m_nType;
	string m_strName;
	CsIllustratedBookCategory m_csIllustratedBookCategory;

	//---------------------------------------------------------------------------------------------------
	public int Type
	{
		get { return m_nType; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsIllustratedBookCategory IllustratedBookCategory
	{
		get { return m_csIllustratedBookCategory; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBookType(WPDIllustratedBookType illustratedBookType)
	{
		m_nType = illustratedBookType.type;
		m_strName = CsConfiguration.Instance.GetString(illustratedBookType.nameKey);
		m_csIllustratedBookCategory = CsGameData.Instance.GetIllustratedBookCategory(illustratedBookType.categoryId);
	}
}
