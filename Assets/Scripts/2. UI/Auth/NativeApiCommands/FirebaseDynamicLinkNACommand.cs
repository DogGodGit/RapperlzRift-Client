using System.Collections;
using UnityEngine;
using LitJson;

public class FirebaseDynamicLinkNACommand : NativeApiCommand {

    public FirebaseDynamicLinkNACommand()
        : base("FirebaseDynamicLink")
    {

    }

    private int m_nEventId;
    private int m_nVirtualGameServerId;
    private string m_strHeroId;
    private string m_sContent;
    private string m_sAppStoreId;
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

    public string Content
    {
        get { return m_sContent; }
        set { m_sContent = value; }
    }

    public string AppStoreId
    {
        get { return m_sAppStoreId; }
        set { m_sAppStoreId = value; }
    }

    public string AuthUrl
    {
        get { return m_sAuthUrl; }
        set { m_sAuthUrl = value; }
    }

    protected override NativeApiResponse CreateResponse()
    {
        return new FirebaseDynamicLinkNAResponse();
    }

    protected override JsonData MakeRequestContent()
    {
        JsonData joReq = base.MakeRequestContent();
        joReq["eventId"] = m_nEventId;
		joReq["virtualGameServerId"] = m_nVirtualGameServerId;
		joReq["heroId"] = m_strHeroId;
        joReq["content"] = m_sContent;
        joReq["appStoreId"] = m_sAppStoreId;
        joReq["authUrl"] = m_sAuthUrl;

        return joReq;
    }
}

public class FirebaseDynamicLinkNAResponse : NativeApiResponse
{
    protected override void HandleBody()
    {
        base.HandleBody();
    }
}
