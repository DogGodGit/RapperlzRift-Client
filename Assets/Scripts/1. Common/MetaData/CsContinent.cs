using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsContinent
{
	int m_nContinentId;             // 대륙ID
	string m_strName;				// 이름
	string m_strDescription;		// 설명
	bool m_bIsNationTerritory;      // 국가영토여부 0이면 공용
	int m_nRequiredHeroLevel;       // 필요영웅레벨(입장가능영웅레벨)	
	bool m_bIsNationWarTarget;      // 국가전대상여부
	string m_strSceneName;          // 씬이름
	int m_nLocationId;              // 위치ID
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsContinentObjectArrange> m_listCsContinentObjectArrange;
	List<CsContinentMapMonster> m_listCsContinentMapMonster;

	//---------------------------------------------------------------------------------------------------
	public int ContinentId
	{
		get { return m_nContinentId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public bool IsNationTerritory
	{
		get { return m_bIsNationTerritory; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public bool IsNationWarTarget
	{
		get { return m_bIsNationWarTarget; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public float XSize
	{
		get { return m_flXSize; }
	}

	public float ZSize
	{
		get { return m_flZSize; }
	}

	public float X
	{
		get { return m_flX; }
	}

	public float Z
	{
		get { return m_flZ; }
	}

	public List<CsContinentObjectArrange> ContinentObjectArrangeList
	{
		get { return m_listCsContinentObjectArrange; }
	}

	public List<CsContinentMapMonster> ContinentMapMonsterList
	{
		get { return m_listCsContinentMapMonster; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinent(WPDContinent continent)
	{
		m_nContinentId = continent.continentId;
		m_strName = CsConfiguration.Instance.GetString(continent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(continent.descriptionKey);
		m_bIsNationTerritory = continent.isNationTerritory;
		m_nRequiredHeroLevel = continent.requiredHeroLevel;
		m_bIsNationWarTarget = continent.isNationWarTarget;
		m_strSceneName = continent.sceneName;
		m_nLocationId = continent.locationId;
		m_flX = continent.x;
		m_flZ = continent.z;
		m_flXSize = continent.xSize;
		m_flZSize = continent.zSize;

		m_listCsContinentObjectArrange = new List<CsContinentObjectArrange>();
		m_listCsContinentMapMonster = new List<CsContinentMapMonster>();
	}
}
