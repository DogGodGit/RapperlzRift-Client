using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsCostume
{
	int m_nCostumeId;
	string m_strName;
	string m_strDescription;
	string m_strPrefabName;
	int m_nRequiredHeroLevel;
	int m_nPeriodLimitDay;

	List<CsCostumeDisplay> m_listCsCostumeDisplay;
	List<CsCostumeAttr> m_listCsCostumeAttr;

	//---------------------------------------------------------------------------------------------------
	public int CostumeId
	{
		get { return m_nCostumeId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int PeriodLimitDay
	{
		get { return m_nPeriodLimitDay; }
	}

	public List<CsCostumeDisplay> CostumeDisplayList
	{
		get { return m_listCsCostumeDisplay; }
	}

	public List<CsCostumeAttr> CostumeAttrList
	{
		get { return m_listCsCostumeAttr; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostume(WPDCostume costume)
	{
		m_nCostumeId = costume.costumeId;
		m_strName = CsConfiguration.Instance.GetString(costume.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(costume.descriptionKey);
		m_strPrefabName = costume.prefabName;
		m_nRequiredHeroLevel = costume.requiredHeroLevel;
		m_nPeriodLimitDay = costume.periodLimitDay;

		m_listCsCostumeDisplay = new List<CsCostumeDisplay>();
		m_listCsCostumeAttr = new List<CsCostumeAttr>();
	}
}
