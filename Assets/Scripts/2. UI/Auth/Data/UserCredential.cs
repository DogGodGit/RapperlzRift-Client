using UnityEngine;
using System.Collections;

public class UserCredential
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private string	m_sUserId = null;
	private string	m_sUserSecret = null;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public UserCredential()
	{
	}

	public UserCredential(string sUserId, string sUserSecret)
	{
		m_sUserId = sUserId;
		m_sUserSecret = sUserSecret;
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
}
