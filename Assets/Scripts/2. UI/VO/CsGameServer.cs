using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-23)
//---------------------------------------------------------------------------------------------------

public class CsGameServer
{
    int m_nServerId;
    string m_strName;
    int m_nGroupId;
    string m_strApiUrl;
    string m_strProxyServer;
    int m_nProxyServerPort;
	int m_nCurrentUserCount;
    int m_nStatus;
    bool m_bIsNew;
    bool m_bIsMaintenance;
    bool m_bIsRecommend;
	bool m_bRecommendable;
	DateTime? m_dtOpenTime;
    int m_nVirtualGameServerId;

    //---------------------------------------------------------------------------------------------------
    public int VirtualGameServerId
    {
        get { return m_nVirtualGameServerId; }
    }

    public int ServerId
    {
        get { return m_nServerId; }
    }

    public string Name
    {
        get { return m_strName; }
    }

	public int GroupId
	{
		get { return m_nGroupId; }
	}

	public string ApiUrl
    {
        get { return m_strApiUrl; }
    }

    public string ProxyServer
    {
        get { return m_strProxyServer; }
    }

    public int ProxyServerPort
    {
        get { return m_nProxyServerPort; }
    }

	public int CurrentUserCount
	{
		get { return m_nCurrentUserCount; }
	}

    public int Status
    {
        get { return m_nStatus; }
    }

    public bool IsNew
    {
        get { return m_bIsNew; }
    }

    public bool IsMaintenance
    {
        get { return m_bIsMaintenance; }
    }

    public string ServerAddress
    {
        get { return m_strProxyServer + ":" + m_nProxyServerPort; }
    }

    public bool Recommend
    {
        get { return m_bIsRecommend; }
    }

	public bool Recommendable
	{
		get { return m_bRecommendable; }
	}

	public DateTime? OpenTime
	{
		// Null 체크 후 사용
		get { return m_dtOpenTime; }
	}

    //---------------------------------------------------------------------------------------------------
    public CsGameServer(int nVirtualGameServerId, int nServerId, string strName, int nGroupId, string strApiUrl,
						string strProxyServer, int nProxyServerPort, int nCurrentUserCount, int nStatus, bool bIsNew,
						bool bIsMaintenance, bool bIsRecommend, bool bRecommendable, string strOpenTime)
    {
        m_nVirtualGameServerId = nVirtualGameServerId;
        m_nServerId = nServerId;
        m_strName = strName;
        m_nGroupId = nGroupId;
        m_strApiUrl = strApiUrl;
        m_strProxyServer = strProxyServer;
        m_nProxyServerPort = nProxyServerPort;
		m_nCurrentUserCount = nCurrentUserCount;
        m_nStatus = nStatus;
		m_bIsNew = bIsNew;
        m_bIsMaintenance = bIsMaintenance;
        m_bIsRecommend = bIsRecommend;
		m_bRecommendable = bRecommendable;

		DateTimeOffset dateTimeOffset;

		if (DateTimeOffset.TryParse(strOpenTime, out dateTimeOffset))
		{
			m_dtOpenTime = dateTimeOffset.DateTime;
		}
		else
		{
			m_dtOpenTime = null;
		}
    }




}
