using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-20)
//---------------------------------------------------------------------------------------------------

public class CsMainGearEnchantStep
{
	int m_nStep;								// 단계
	CsItem csItemNextEnchantMaterial;           // 다음강화재료아이템
	CsItem csItemNextEnchantPenaltyPrevent;     // 다음강화패널티방지아이템

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public CsItem NextEnchantMaterialItem
	{
		get { return csItemNextEnchantMaterial; }
	}

	public CsItem NextEnchantPenaltyPreventItem
	{
		get { return csItemNextEnchantPenaltyPrevent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainGearEnchantStep(WPDMainGearEnchantStep mainGearEnchantStep)
	{
		m_nStep = mainGearEnchantStep.step;
		csItemNextEnchantMaterial = CsGameData.Instance.GetItem(mainGearEnchantStep.nextEnchantMaterialItemId);
		csItemNextEnchantPenaltyPrevent = CsGameData.Instance.GetItem(mainGearEnchantStep.nextEnchantPenaltyPreventItemId);
	}
}
