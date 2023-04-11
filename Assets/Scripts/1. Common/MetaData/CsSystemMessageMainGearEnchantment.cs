using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageMainGearEnchantment : CsSystemMessage
{
	CsMainGear m_csMainGear;
	int m_nEnchantLevel;

	//---------------------------------------------------------------------------------------------------
	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageMainGearEnchantment(PDMainGearEnchantmentSystemMessage mainGearEnchantmentSystemMessage) 
		: base (mainGearEnchantmentSystemMessage)
	{
		m_csMainGear = CsGameData.Instance.GetMainGear(mainGearEnchantmentSystemMessage.mainGearId);
		m_nEnchantLevel = mainGearEnchantmentSystemMessage.enchantLevel;
	}
}
