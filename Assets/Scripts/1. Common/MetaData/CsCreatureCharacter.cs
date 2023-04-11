using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCharacter
{
	int m_nCreatureCharacterId;
	string m_strName;
	string m_strDescription;
	int m_nRequiredHeroLevel;
	string m_strPrefabName;
	string m_strImageName;

	//---------------------------------------------------------------------------------------------------
	public int CreatureCharacterId
	{
		get { return m_nCreatureCharacterId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCharacter(WPDCreatureCharacter creatureCharacter)
	{
		m_nCreatureCharacterId = creatureCharacter.creatureCharacterId;
		m_strName = CsConfiguration.Instance.GetString(creatureCharacter.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(creatureCharacter.descriptionKey);
		m_nRequiredHeroLevel = creatureCharacter.requiredHeroLevel;
		m_strPrefabName = creatureCharacter.prefabName;
		m_strImageName = creatureCharacter.imageName;
	}
}
