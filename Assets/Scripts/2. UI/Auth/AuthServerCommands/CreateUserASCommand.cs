using UnityEngine;
using System.Collections;

public abstract class CreateUserASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public CreateUserASCommand(string sCommand) : base(sCommand)
	{
	}
}

public abstract class CreateUserASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string	m_sUserId = null;
	private string	m_sUserSecret = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public virtual string userId
	{
		get { return m_sUserId; }
	}

	public virtual string userSecret
	{
		get { return m_sUserSecret; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		m_sUserId = LitJsonUtil.GetStringProperty(m_joContent, "userId");
		m_sUserSecret = LitJsonUtil.GetStringProperty(m_joContent, "userSecret");
	}
}
