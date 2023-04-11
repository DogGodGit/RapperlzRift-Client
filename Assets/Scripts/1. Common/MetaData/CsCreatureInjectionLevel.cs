using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsCreatureInjectionLevel
{
	int m_nInjectionLevel;
	int m_nNextLevelUpRequiredExp;
	int m_nRequiredItemCount;
	long m_lRequiredGold;

	//---------------------------------------------------------------------------------------------------
	public int InjectionLevel
	{
		get { return m_nInjectionLevel; }
	}

	public int NextLevelUpRequiredExp
	{
		get { return m_nNextLevelUpRequiredExp; }
	}

	public int RequiredItemCount
	{
		get { return m_nRequiredItemCount; }
	}

	public long RequiredGold
	{
		get { return m_lRequiredGold; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureInjectionLevel(WPDCreatureInjectionLevel creatureInjectionLevel)
	{
		m_nInjectionLevel = creatureInjectionLevel.injectionLevel;
		m_nNextLevelUpRequiredExp = creatureInjectionLevel.nextLevelUpRequiredExp;
		m_nRequiredItemCount = creatureInjectionLevel.requiredItemCount;
		m_lRequiredGold = creatureInjectionLevel.requiredGold;
	}
}
