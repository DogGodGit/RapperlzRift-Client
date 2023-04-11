using System.Collections;
using System.Collections.Generic;

using LitJson;

public class CreateFacebookUserASCommand : CreateUserASCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string m_sFacebookAppId = null;
	private string m_sFacebookUserId = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public CreateFacebookUserASCommand()
		: base("CreateFacebookUser")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string facebookAppId
	{
		get { return m_sFacebookAppId; }
		set { m_sFacebookAppId = value; }
	}

	public string facebookUserId
	{
		get { return m_sFacebookUserId; }
		set { m_sFacebookUserId = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new CreateFacebookUserASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["facebookAppId"] = m_sFacebookAppId;
		joReq["facebookUserId"] = m_sFacebookUserId;

		return joReq;
	}
}

public class CreateFacebookUserASResponse : CreateUserASResponse
{
}
