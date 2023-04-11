//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public class CsUserGameServer
{
    int m_nServerId;
    int m_nHeroCount;
    int m_nVirtualGameServerId;
    CsGameServer m_scGameServer;

    //---------------------------------------------------------------------------------------------------
    public int ServerId
    {
        get { return m_nServerId; }
    }

    public int VirtualGameServerId
    {
        get { return m_nVirtualGameServerId; }
    }

    public int HeroCount
    {
        get { return m_nHeroCount; }
    }

    public CsGameServer GameServer
    {
        get { return m_scGameServer; }
        set { m_scGameServer = value; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsUserGameServer(int nServerId, int nHeroCount, int nVirtualGameServerId)
    {
        m_nServerId = nServerId;
        m_nHeroCount = nHeroCount;
        m_nVirtualGameServerId = nVirtualGameServerId;

    }
}
