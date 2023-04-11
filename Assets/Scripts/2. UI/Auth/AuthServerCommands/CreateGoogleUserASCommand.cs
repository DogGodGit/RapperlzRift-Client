using UnityEngine;
using System.Collections;

using LitJson;

public class CreateGoogleUserASCommand : CreateUserASCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string	m_sGoogleUserId = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public CreateGoogleUserASCommand() 
		: base("CreateGoogleUser")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string googleUserId
	{
		get { return m_sGoogleUserId; }
		set { m_sGoogleUserId = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new CreateGoogleUserASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["googleUserId"] = m_sGoogleUserId;

		return joReq;
	}
}

public class CreateGoogleUserASResponse : CreateUserASResponse
{
}
