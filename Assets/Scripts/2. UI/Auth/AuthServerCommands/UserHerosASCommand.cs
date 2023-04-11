using System.Collections;
using System.Collections.Generic;

using LitJson;

public class UserHerosASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	string m_sUserAccessToken = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors
	public UserHerosASCommand()
		: base("UserHeros")
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
		return new UserHerosASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["userAccessToken"] = m_sUserAccessToken;

		return joReq;
	}
}

public class UserHerosASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	List<CsUserHero> m_listCsUserHero;

	public UserHerosASResponse()
	{
		m_listCsUserHero = new List<CsUserHero>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public List<CsUserHero> UserHeroList
	{
		get { return m_listCsUserHero; }
	}

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			JsonData jsonDataUserHeros = LitJsonUtil.GetArrayProperty(m_joContent, "userHeros");

			for (int i = 0; i < jsonDataUserHeros.Count; i++)
			{
				m_listCsUserHero.Add(new CsUserHero(LitJsonUtil.GetIntProperty(jsonDataUserHeros[i], "virtualGameServerId"),
													LitJsonUtil.GetStringProperty(jsonDataUserHeros[i], "heroId"),
													LitJsonUtil.GetStringProperty(jsonDataUserHeros[i], "name"),
													LitJsonUtil.GetIntProperty(jsonDataUserHeros[i], "serverId")));
			}
		}
	}
}

