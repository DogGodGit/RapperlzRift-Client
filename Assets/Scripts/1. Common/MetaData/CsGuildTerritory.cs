using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildTerritory
{
	string m_strSceneName;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	int m_nStartYRotationType;
	float m_flStartYRotation;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsGuildTerritoryNpc> m_listCsGuildTerritoryNpc;

	//---------------------------------------------------------------------------------------------------
	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public float StartXPosition
	{
		get { return m_flStartXPosition; }
	}

	public float StartYPosition
	{
		get { return m_flStartYPosition; }
	}

	public float StartZPosition
	{
		get { return m_flStartZPosition; }
	}

	public float StartRadius
	{
		get { return m_flStartRadius; }
	}

	public int StartYRotationType
	{
		get { return m_nStartYRotationType; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public float X
	{
		get { return m_flX; }
	}

	public float Z
	{
		get { return m_flZ; }
	}

	public float XSize
	{
		get { return m_flXSize; }
	}

	public float ZSize
	{
		get { return m_flZSize; }
	}

	public List<CsGuildTerritoryNpc> GuildTerritoryNpcList
	{
		get { return m_listCsGuildTerritoryNpc; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildTerritory(WPDGuildTerritory guildTerritory)
	{
		m_strSceneName = guildTerritory.sceneName;
		m_flStartXPosition = guildTerritory.startXPosition;
		m_flStartYPosition = guildTerritory.startYPosition;
		m_flStartZPosition = guildTerritory.startZPosition;
		m_flStartRadius = guildTerritory.startRadius;
		m_nStartYRotationType = guildTerritory.startYRotationType;
		m_flStartYRotation = guildTerritory.startYRotation;
		m_nLocationId = guildTerritory.locationId;
		m_flX = guildTerritory.x;
		m_flZ = guildTerritory.z;
		m_flXSize = guildTerritory.xSize;
		m_flZSize = guildTerritory.zSize;

		m_listCsGuildTerritoryNpc = new List<CsGuildTerritoryNpc>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildTerritoryNpc GetGuildTerritoryNpc(int nNpcId)
	{
		for (int i = 0; i < m_listCsGuildTerritoryNpc.Count; i++)
		{
			if (m_listCsGuildTerritoryNpc[i].NpcId == nNpcId)
				return m_listCsGuildTerritoryNpc[i];
		}

		return null;
	}
}
