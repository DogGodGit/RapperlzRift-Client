using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildBuilding
{
	int m_nBuildingId;
	string m_strName;

	List<CsGuildBuildingLevel> m_listCsGuildBuildingLevel;

	//---------------------------------------------------------------------------------------------------
	public int BuildingId
	{
		get { return m_nBuildingId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public List<CsGuildBuildingLevel> GuildBuildingLevelList
	{
		get { return m_listCsGuildBuildingLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuilding(WPDGuildBuilding guildBuilding)
	{
		m_nBuildingId = guildBuilding.buildingId;
		m_strName = CsConfiguration.Instance.GetString(guildBuilding.nameKey);

		m_listCsGuildBuildingLevel = new List<CsGuildBuildingLevel>();
	}
}
