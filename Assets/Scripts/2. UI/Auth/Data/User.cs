using UnityEngine;
using System.Collections;

public enum EnUserType
{
	Guest = 1,
	Facebook = 101,
	Google = 102,
	Entermate = 201,
}

public class User
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	int		m_nUserType = 0;
	string	m_sUserId = null;
	string	m_sAccessToken = null;
    int     m_nLastVirtualGameServer1 = 0;
	int		m_nLastVirtualGameServer2 = 0;
	int m_nEventId;
	bool m_bRewarded;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public int UserType
	{
		get { return m_nUserType; }
		//set { m_nUserType = value; }
	}

	public string UserId
	{
		get { return m_sUserId; }
		set { m_sUserId = value; }
	}

	public string AccessToken
	{
		get { return m_sAccessToken; }
		//set { m_sAccessToken = value; }
	}

    public int LastVirtualGameServer1
    {
        get { return m_nLastVirtualGameServer1; }
        //set { m_nLastGameServer1 = value; }
    }

	public int LastVirtualGameServer2
	{
		get { return m_nLastVirtualGameServer2; }
		//set { m_nLastGameServer2 = value; }
	}

	public int EventId
	{
		get { return m_nEventId; }
		set { m_nEventId = value; }
	}

	public bool Rewarded
	{
		get { return m_bRewarded; }
		set { m_bRewarded = value; }
	}


	public User(int nUserType, string sAccessToken, int nLastVirtualGameServer1, int nLastVirtualGameServer2)
	{
		m_nUserType = nUserType;
		m_sAccessToken = sAccessToken;
		m_nLastVirtualGameServer1 = nLastVirtualGameServer1;
		m_nLastVirtualGameServer2 = nLastVirtualGameServer2;
	}
}
