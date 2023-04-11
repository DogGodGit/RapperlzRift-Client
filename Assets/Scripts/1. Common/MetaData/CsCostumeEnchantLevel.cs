using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsCostumeEnchantLevel
{
	int m_nEnchantLevel;
	int m_nStep;
	int m_nNextLevelUpSuccessRate;
	int m_nNextLevelRequiredItemCount;
	int m_nNextLevelMaxLuckyValue;

	//---------------------------------------------------------------------------------------------------
	public int EnchantLevel
	{
		get { return m_nEnchantLevel; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int NextLevelUpSuccessRate
	{
		get { return m_nNextLevelUpSuccessRate; }
	}

	public int NextLevelRequiredItemCount
	{
		get { return m_nNextLevelRequiredItemCount; }
	}

	public int NextLevelMaxLuckyValue
	{
		get { return m_nNextLevelMaxLuckyValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCostumeEnchantLevel(WPDCostumeEnchantLevel costumeEnchantLevel)
	{
		m_nEnchantLevel = costumeEnchantLevel.enchantLevel;
		m_nStep = costumeEnchantLevel.step;
		m_nNextLevelUpSuccessRate = costumeEnchantLevel.nextLevelUpSuccessRate;
		m_nNextLevelRequiredItemCount = costumeEnchantLevel.nextLevelUpRequiredItemCount;
		m_nNextLevelMaxLuckyValue = costumeEnchantLevel.nextLevelUpMaxLuckyValue;
	}

}
