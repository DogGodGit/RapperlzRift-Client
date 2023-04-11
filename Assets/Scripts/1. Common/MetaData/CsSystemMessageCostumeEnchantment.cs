using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageCostumeEnchantment : CsSystemMessage
{
	CsCostume m_csCostume;
	int m_nEnchantLevel;

	//---------------------------------------------------------------------------------------------------
	public CsCostume Costume
	{
		get { return m_csCostume; }
	}

	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageCostumeEnchantment(PDCostumeEnchantmentSystemMessage costumeEnchantmentSystemMessage)
		: base (costumeEnchantmentSystemMessage)
	{
		m_csCostume = CsGameData.Instance.GetCostume(costumeEnchantmentSystemMessage.costumeId);
		m_nEnchantLevel = costumeEnchantmentSystemMessage.enchantLevel;
	}
}
