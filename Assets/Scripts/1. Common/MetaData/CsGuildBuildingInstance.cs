using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildBuildingInstance
{
	int m_nBuildingId;
	int m_nLevel;

	//---------------------------------------------------------------------------------------------------
	public int BuildingId
	{
		get { return m_nBuildingId; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set { m_nLevel = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuildingInstance(PDGuildBuildingInstance guildBuildingInstance)
	{
		m_nBuildingId = guildBuildingInstance.buildingId;
		m_nLevel = guildBuildingInstance.level;
	}
}
