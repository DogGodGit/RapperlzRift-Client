using UnityEngine;
using System.Collections;

using LitJson;

public class CreateEntermateUserASCommand : CreateUserASCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string	m_sEntermateUserId = null;
	private string 	m_sEntermatePrivateKey = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public CreateEntermateUserASCommand() 
		: base("CreateEntermateUser")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string entermateUserId
	{
		get { return m_sEntermateUserId; }
		set { m_sEntermateUserId = value; }
	}

	public string entermatePrivateKey
	{
		get { return m_sEntermatePrivateKey; }
		set { m_sEntermatePrivateKey = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new CreateEntermateUserASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["entermateUserId"] = m_sEntermateUserId;
		joReq["entermatePrivateKey"] = m_sEntermatePrivateKey;

		return joReq;
	}
}

public class CreateEntermateUserASResponse : CreateUserASResponse
{
}
