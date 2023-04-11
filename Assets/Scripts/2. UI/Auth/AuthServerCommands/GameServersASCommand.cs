using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class GameServersASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public GameServersASCommand()
		: base("GameServers")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new GameServersASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		return joReq;
	}
}

public class GameServersASResponse : AuthServerResponse
{
	List<CsGameServerRegion> m_csGameServerRegionList;

	public List<CsGameServerRegion> GameServerRegionList
	{
		get { return m_csGameServerRegionList; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	public GameServersASResponse()
	{
		m_csGameServerRegionList = new List<CsGameServerRegion>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			List<CsGameServerGroup> listGameServerGroup = new List<CsGameServerGroup>();

			JsonData jsonDataServerRegions = LitJsonUtil.GetArrayProperty(m_joContent, "gameServerRegions");

			for (int i = 0; i < jsonDataServerRegions.Count; i++)
			{
				m_csGameServerRegionList.Add(new CsGameServerRegion(LitJsonUtil.GetIntProperty(jsonDataServerRegions[i], "regionId"),
																	LitJsonUtil.GetStringProperty(jsonDataServerRegions[i], "nameKey")));

			}
			
            JsonData jsonDataServerGroups = LitJsonUtil.GetArrayProperty(m_joContent, "gameServerGroups");

			for (int i = 0; i < jsonDataServerGroups.Count; i++)
			{
				int nRegionId = LitJsonUtil.GetIntProperty(jsonDataServerGroups[i], "regionId");

				CsGameServerRegion csGameServerRegion = m_csGameServerRegionList.Find(gameServerRegion => gameServerRegion.RegionId == nRegionId);

				if (csGameServerRegion != null)
				{
					CsGameServerGroup csGameServerGroup = new CsGameServerGroup(LitJsonUtil.GetIntProperty(jsonDataServerGroups[i], "groupId"),
																				LitJsonUtil.GetStringProperty(jsonDataServerGroups[i], "nameKey"),
																				LitJsonUtil.GetIntProperty(jsonDataServerGroups[i], "regionId"),
																				LitJsonUtil.GetBooleanProperty(jsonDataServerGroups[i], "recommendServerAuto"),
																				LitJsonUtil.GetIntProperty(jsonDataServerGroups[i], "recommendServerConditionType"),
																				LitJsonUtil.GetBooleanProperty(jsonDataServerGroups[i], "isAccessRestriction"),
																				LitJsonUtil.GetBooleanProperty(jsonDataServerGroups[i], "accessAllowed"));

					csGameServerRegion.GameServerGroupList.Add(csGameServerGroup);
					listGameServerGroup.Add(csGameServerGroup);
				}
			}

			JsonData jsonDataGameServers = LitJsonUtil.GetArrayProperty(m_joContent, "gameServers");

			for (int i = 0; i < jsonDataGameServers.Count; i++)
			{
				int nGroupId = LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "groupId");

				CsGameServerGroup csGameServerGroup = listGameServerGroup.Find(a => a.GroupId == nGroupId);
				if (csGameServerGroup != null)
				{
					csGameServerGroup.GameServerList.Add(new CsGameServer(LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "virtualGameServerId"),
																		  LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "serverId"),
																		  LitJsonUtil.GetStringProperty(jsonDataGameServers[i], "name"),
																		  LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "groupId"),
																		  LitJsonUtil.GetStringProperty(jsonDataGameServers[i], "apiUrl"),
																		  LitJsonUtil.GetStringProperty(jsonDataGameServers[i], "proxyServer"),
																		  LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "proxyServerPort"),
																		  LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "currentUserCount"),
																		  LitJsonUtil.GetIntProperty(jsonDataGameServers[i], "status"),
																		  LitJsonUtil.GetBooleanProperty(jsonDataGameServers[i], "isNew"),
																		  LitJsonUtil.GetBooleanProperty(jsonDataGameServers[i], "isMaintenance"),
                                                                          LitJsonUtil.GetBooleanProperty(jsonDataGameServers[i], "isRecommend"),
																		  LitJsonUtil.GetBooleanProperty(jsonDataGameServers[i], "recommendable"),
																		  LitJsonUtil.GetStringProperty(jsonDataGameServers[i], "openTime")));
				}
			}
		}
	}
}



