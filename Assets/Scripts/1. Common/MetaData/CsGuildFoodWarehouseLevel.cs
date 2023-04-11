using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildFoodWarehouseLevel
{
	int m_nLevel;
	int m_nNextLevelUpRequiredExp;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpRequiredExp
	{
		get { return m_nNextLevelUpRequiredExp; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildFoodWarehouseLevel(WPDGuildFoodWarehouseLevel guildFoodWarehouseLevel)
	{
		m_nLevel = guildFoodWarehouseLevel.level;
		m_nNextLevelUpRequiredExp = guildFoodWarehouseLevel.nextLevelUpRequiredExp;
	}
}
