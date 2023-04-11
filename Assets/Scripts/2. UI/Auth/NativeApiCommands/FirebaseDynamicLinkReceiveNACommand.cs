using System.Collections;
using UnityEngine;
using LitJson;

public class FirebaseDynamicLinkReceiveNACommand : NativeApiCommand
{
    public FirebaseDynamicLinkReceiveNACommand()
        : base("FirebaseDynamicLinkReceive")
    {

    }

    private int m_nEventId;
    private int m_nVirtualGameServerId;
	private string m_strHeroId;
    private string m_sAuthUrl;

    public int EventId
    {
        get { return m_nEventId; }
        set { m_nEventId = value; }
    }

    public int VirtualGameServerId
    {
        get { return m_nVirtualGameServerId; }
        set { m_nVirtualGameServerId = value; }
    }

	public string HeroId
    {
		get { return m_strHeroId; }
		set { m_strHeroId = value; }
    }

    public string AuthUrl
    {
        get { return m_sAuthUrl; }
        set { m_sAuthUrl = value; }
    }

    protected override NativeApiResponse CreateResponse()
    {
        return new FirebaseDynamicLinkReceiveNAResponse();
    }

    protected override JsonData MakeRequestContent()
    {
        JsonData joReq = base.MakeRequestContent();
        joReq["eventId"] = m_nEventId;
		joReq["virtualGameServerId"] = m_nVirtualGameServerId;
		joReq["heroId"] = m_strHeroId;
        joReq["authUrl"] = m_sAuthUrl;

        return joReq;
    }
}

public class FirebaseDynamicLinkReceiveNAResponse : NativeApiResponse
{
    protected override void HandleBody()
    {
        base.HandleBody();
    }
}
