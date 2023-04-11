using System.Collections;
using System.Collections.Generic;
using LitJson;

public class StatusLoggingNACommand : NativeApiCommand
{
	string m_strPing;
	string m_strFrameRate;
	string m_strUserId;
	string m_strHeroId;

	public StatusLoggingNACommand() : base("StatusLogging")
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override NativeApiResponse CreateResponse()
	{
		return new StatusLoggingNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["userId"] = m_strUserId;
		joReq["heroId"] = m_strHeroId;
		joReq["ping"] = m_strPing;
		joReq["frameRate"] = m_strFrameRate;
		joReq["url"] = CsConfiguration.Instance.SystemSetting.StatusLoggingUrl;

		return joReq;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public string ping
	{
		get { return m_strPing; }
		set { m_strPing = value; }
	}

	public string frameRate
	{
		get { return m_strFrameRate; }
		set { m_strFrameRate = value; }
	}

	public string UserId
	{
		get { return m_strUserId; }
		set { m_strUserId = value; }
	}

	public string HeroId
	{
		get { return m_strHeroId; }
		set { m_strHeroId = value; }
	}
}

public class StatusLoggingNAResponse : NativeApiResponse
{
	protected override void HandleBody()
	{
		base.HandleBody();
	}
}

