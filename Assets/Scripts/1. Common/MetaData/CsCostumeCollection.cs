using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeCollection
{
	int m_nCostumeCollectionId;
	string m_strName;
	string m_strDescription;
	int m_nActivationItemCount;

	List<CsCostumeCollectionAttr> m_listCsCostumeCollectionAttr;
	List<CsCostumeCollectionEntry> m_listCsCostumeCollectionEntry;

	//---------------------------------------------------------------------------------------------------
	public int CostumeCollectionId
	{
		get { return m_nCostumeCollectionId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int ActivationItemCount
	{
		get { return m_nActivationItemCount; }
	}

	public List<CsCostumeCollectionAttr> CostumeCollectionAttrList
	{
		get { return m_listCsCostumeCollectionAttr; }
	}

	public List<CsCostumeCollectionEntry> CostumeCollectionEntryList
	{
		get { return m_listCsCostumeCollectionEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeCollection(WPDCostumeCollection costumeCollection)
	{
		m_nCostumeCollectionId = costumeCollection.costumeCollectionId;
		m_strName = CsConfiguration.Instance.GetString(costumeCollection.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(costumeCollection.descriptionKey);
		m_nActivationItemCount = costumeCollection.activationItemCount;

		m_listCsCostumeCollectionAttr = new List<CsCostumeCollectionAttr>();
		m_listCsCostumeCollectionEntry = new List<CsCostumeCollectionEntry>();
	}
}
