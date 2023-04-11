using System.Collections;
using System.Collections.Generic;

using LitJson;

public class UserGameServersASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	string m_sUserAccessToken = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public UserGameServersASCommand()
		: base("UserGameServers")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public string UserAccessToken
	{
		get { return m_sUserAccessToken; }
		set { m_sUserAccessToken = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new UserGameServersASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["userAccessToken"] = m_sUserAccessToken;

		return joReq;
	}
}

public class UserGameServersASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	List<CsUserGameServer> m_csUserGameServerList;
	
	public UserGameServersASResponse()
	{
		m_csUserGameServerList = new List<CsUserGameServer>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public List<CsUserGameServer> UserGameServerList
	{
		get { return m_csUserGameServerList; }
	}

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			JsonData jsonDataUserGameServers = LitJsonUtil.GetArrayProperty(m_joContent, "userGameServers");

			for (int i = 0; i < jsonDataUserGameServers.Count; i++)
			{
				m_csUserGameServerList.Add(new CsUserGameServer(LitJsonUtil.GetIntProperty(jsonDataUserGameServers[i], "serverId"),
															    LitJsonUtil.GetIntProperty(jsonDataUserGameServers[i], "heroCount"),
                                                                LitJsonUtil.GetIntProperty(jsonDataUserGameServers[i], "virtualGameServerId")));
			}
		}
	}
}
