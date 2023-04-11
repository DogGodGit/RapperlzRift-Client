using System.Collections.Generic;
using WebCommon;
//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-17)
//---------------------------------------------------------------------------------------------------

public class CsIllustratedBook
{
	int m_nIllustratedBookId;
	string m_strName;
	string m_strDescription;
	string m_strImageName;
	CsIllustratedBookType m_csIllustratedBookType;
	int m_nExplorationPoint;

	List<CsIllustratedBookAttr> m_listCsIllustratedBookAttr;

	//---------------------------------------------------------------------------------------------------
	public int IllustratedBookId
	{
		get { return m_nIllustratedBookId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public CsIllustratedBookType IllustratedBookType
	{
		get { return m_csIllustratedBookType; }
	}

	public int ExplorationPoint
	{
		get { return m_nExplorationPoint; }
	}

	public List<CsIllustratedBookAttr> IllustratedBookAttrList
	{
		get { return m_listCsIllustratedBookAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsIllustratedBook(WPDIllustratedBook illustratedBook)
	{
		m_nIllustratedBookId = illustratedBook.illustratedBookId;
		m_strName = CsConfiguration.Instance.GetString(illustratedBook.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(illustratedBook.descriptionKey);
		m_strImageName = illustratedBook.imageName;
		m_csIllustratedBookType = CsGameData.Instance.GetIllustratedBookType(illustratedBook.type);
		m_nExplorationPoint = illustratedBook.explorationPoint;

		m_listCsIllustratedBookAttr = new List<CsIllustratedBookAttr>();
	}
}
