using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsMainGearEnchantLevel
{
	int m_nEnchantLevel;								// 강화레벨
	CsMainGearEnchantStep m_csMainGearEnchantStep;		// 단계
	int m_nNextSuccessRate;								// 다음강화성공확률
	bool m_bPenaltyPreventEnabled;						// 패널티방지사용가능여부

	//---------------------------------------------------------------------------------------------------
	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	public CsMainGearEnchantStep MainGearEnchantStep
	{
		get { return m_csMainGearEnchantStep; }
	}

	public int NextSuccessRatePercentage
	{
		get { return m_nNextSuccessRate / 100; }
	}

	public bool PenaltyPreventEnabled
	{
		get { return m_bPenaltyPreventEnabled; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantLevel(WPDMainGearEnchantLevel mainGearEnchantLevel)
	{
		m_nEnchantLevel = mainGearEnchantLevel.enchantLevel;
		m_csMainGearEnchantStep = CsGameData.Instance.GetMainGearEnchantStep(mainGearEnchantLevel.step);
		m_nNextSuccessRate = mainGearEnchantLevel.nextSuccessRate;
		m_bPenaltyPreventEnabled = mainGearEnchantLevel.penaltyPreventEnabled;
	}
}
