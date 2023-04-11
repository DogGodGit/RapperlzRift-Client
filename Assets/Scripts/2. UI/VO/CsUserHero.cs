using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-10)
//---------------------------------------------------------------------------------------------------

public class CsUserHero
{
    int m_nVirtualGameServerId;
    string m_strHeroId;
    string m_strName;
    int m_nServerId;

    CsGameServer m_scGameServer;

    //---------------------------------------------------------------------------------------------------
    public int VirtualGameServerId
    {
        get { return m_nVirtualGameServerId; }
    }

    public string HeroId
    {
        get { return m_strHeroId; }
    }

    public string Name
    {
        get { return m_strName; }
    }

    public int ServerId
    {
        get { return m_nServerId; }
    }

    public CsGameServer GameServer
    {
        get { return m_scGameServer; }
        set { m_scGameServer = value; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsUserHero(int nVirtualGameServerId, string strHeroId, string strName, int nServerId)
    {
        m_nVirtualGameServerId = nVirtualGameServerId;
        m_strHeroId = strHeroId;
        m_strName = strName;
        m_nServerId = nServerId;
    }

}
