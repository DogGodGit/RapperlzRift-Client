using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureLevel
{
	int m_nLevel;
	int m_nNextLevelUpRequiredExp;
	int m_nMaxInjectionLevel;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpRequiredExp
	{
		get { return m_nNextLevelUpRequiredExp; }
	}

	public int MaxInjectionLevel
	{
		get { return m_nMaxInjectionLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureLevel(WPDCreatureLevel creatureLevel)
	{
		m_nLevel = creatureLevel.level;
		m_nNextLevelUpRequiredExp = creatureLevel.nextLevelUpRequiredExp;
		m_nMaxInjectionLevel = creatureLevel.maxInjectionLevel;

	}
}
