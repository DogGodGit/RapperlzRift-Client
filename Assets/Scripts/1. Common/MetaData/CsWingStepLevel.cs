using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsWingStepLevel
{
	int m_nStep;
	int m_nLevel;
	int m_nNextLevelUpRequiredExp;
	int m_nAccEnchantLimitCount;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpRequiredExp
	{
		get { return m_nNextLevelUpRequiredExp; }
	}

	public int AccEnchantLimitCount
	{
		get { return m_nAccEnchantLimitCount; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWingStepLevel(WPDWingStepLevel wingStepLevel)
	{
		m_nStep = wingStepLevel.step;
		m_nLevel = wingStepLevel.level;
		m_nNextLevelUpRequiredExp = wingStepLevel.nextLevelUpRequiredExp;
		m_nAccEnchantLimitCount = wingStepLevel.accEnchantLimitCount;
	}
}
