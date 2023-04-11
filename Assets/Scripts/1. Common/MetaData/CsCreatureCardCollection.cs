using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardCollection
{
	int m_nCollectionId;
	string m_strName;
	CsCreatureCardCollectionCategory m_csCreatureCardCollectionCategory;
	CsCreatureCardCollectionGrade m_csCreatureCardCollectionGrade;

	List<CsCreatureCardCollectionAttr> m_listCsCreatureCardCollectionAttr;

	//---------------------------------------------------------------------------------------------------
	public int CollectionId
	{
		get { return m_nCollectionId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsCreatureCardCollectionCategory CreatureCardCollectionCategory
	{
		get { return m_csCreatureCardCollectionCategory; }
	}

	public CsCreatureCardCollectionGrade CreatureCardCollectionGrade
	{
		get { return m_csCreatureCardCollectionGrade; }
	}

	public List<CsCreatureCardCollectionAttr> CreatureCardCollectionAttrList
	{
		get { return m_listCsCreatureCardCollectionAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardCollection(WPDCreatureCardCollection creatureCardCollection)
	{
		m_nCollectionId = creatureCardCollection.collectionId;
		m_strName = CsConfiguration.Instance.GetString(creatureCardCollection.nameKey);
		m_csCreatureCardCollectionCategory = CsGameData.Instance.GetCreatureCardCollectionCategory(creatureCardCollection.categoryId);
		m_csCreatureCardCollectionGrade = CsGameData.Instance.GetCreatureCardCollectionGrade(creatureCardCollection.grade);

		m_listCsCreatureCardCollectionAttr = new List<CsCreatureCardCollectionAttr>();
	}
}
