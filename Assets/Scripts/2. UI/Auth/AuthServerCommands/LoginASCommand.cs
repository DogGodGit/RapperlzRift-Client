using System.Collections;
using System.Collections.Generic;

using LitJson;

public class LoginASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string m_sUserId = null;
	private string m_sUserSecret = null;
	int m_nLanguageId;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public LoginASCommand()
		: base("Login")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string userId
	{
		get { return m_sUserId; }
		set { m_sUserId = value; }
	}

	public string userSecret
	{
		get { return m_sUserSecret; }
		set { m_sUserSecret = value; }                                                                                  
	}

	public int languageId
	{
		get { return m_nLanguageId; }
		set { m_nLanguageId = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new LoginASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["userId"] = m_sUserId;
		joReq["userSecret"] = m_sUserSecret;
		joReq["languageId"] = m_nLanguageId;
		return joReq;
	}
}

public class LoginASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	User m_user;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public User User
	{
		get { return m_user; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			m_user = new User(LitJsonUtil.GetIntProperty(m_joContent, "userType"),
							  LitJsonUtil.GetStringProperty(m_joContent, "accessToken"),
							  LitJsonUtil.GetIntProperty(m_joContent, "lastVirtualGameServerId1"),
							  LitJsonUtil.GetIntProperty(m_joContent, "lastVirtualGameServerId2"));

			JsonData joSharingEventReceive = LitJsonUtil.GetObjectProperty(m_joContent, "sharingEventReceive");

			if (joSharingEventReceive == null)
			{
				m_user.EventId = 0;
				m_user.Rewarded = false;
			}
			else
			{
				m_user.EventId = LitJsonUtil.GetIntProperty(joSharingEventReceive, "eventId");
				m_user.Rewarded = LitJsonUtil.GetBooleanProperty(joSharingEventReceive, "rewarded");
			}
		}
	}
}

