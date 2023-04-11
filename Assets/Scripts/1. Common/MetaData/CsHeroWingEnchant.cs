using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-23)
//---------------------------------------------------------------------------------------------------

public class CsHeroWingEnchant
{
	int m_nStep;
	int m_nLevel;
	int m_nEnchantCount;

	//---------------------------------------------------------------------------------------------------
	public int Step
	{
		get { return m_nStep; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int EnchantCount
	{
		get { return m_nEnchantCount; }
		set { m_nEnchantCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWingEnchant(PDHeroWingEnchant heroWingEnchant)
	{
		m_nStep = heroWingEnchant.step;
		m_nLevel = heroWingEnchant.level;
		m_nEnchantCount = heroWingEnchant.enchantCount;
	}
}
