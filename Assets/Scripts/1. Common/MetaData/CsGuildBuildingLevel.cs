using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildBuildingLevel
{
	int m_nBuildingId;
	int m_nLevel;
	int m_nNextLevelUpGuildBuildingPoint;
	int m_nNextLevelUpGuildFund;

	//---------------------------------------------------------------------------------------------------
	public int BuildingId
	{
		get { return m_nBuildingId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int NextLevelUpGuildBuildingPoint
	{
		get { return m_nNextLevelUpGuildBuildingPoint; }
	}

	public int NextLevelUpGuildFund
	{
		get { return m_nNextLevelUpGuildFund; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuildingLevel(WPDGuildBuildingLevel guildBuildingLevel)
	{
		m_nBuildingId = guildBuildingLevel.buildingId;
		m_nLevel = guildBuildingLevel.level;
		m_nNextLevelUpGuildBuildingPoint = guildBuildingLevel.nextLevelUpGuildBuildingPoint;
		m_nNextLevelUpGuildFund = guildBuildingLevel.nextLevelUpGuildFund;
	}
}
