using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-12)
//---------------------------------------------------------------------------------------------------

public class CsMonsterCharacter
{
	int m_nMonsterCharacterId;      // 몬스터캐릭터ID
	string m_strName;               // 이름
	string m_strPrefabName;         // 프리팹이름

	//---------------------------------------------------------------------------------------------------
	public int MonsterCharacterId
	{
		get { return m_nMonsterCharacterId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMonsterCharacter(WPDMonsterCharacter monsterCharacter)
	{
		m_nMonsterCharacterId = monsterCharacter.monsterCharacterId;
		m_strName = CsConfiguration.Instance.GetString(monsterCharacter.nameKey);
		m_strPrefabName = monsterCharacter.prefabName;
	}

	// 2018.05.09
	//---------------------------------------------------------------------------------------------------
	public CsMonsterCharacter(int nMonsterCharacterId, string strName, string strPrefabName)
	{
		m_nMonsterCharacterId = nMonsterCharacterId;
		m_strName = strName;
		m_strPrefabName = strPrefabName;
	}

}
