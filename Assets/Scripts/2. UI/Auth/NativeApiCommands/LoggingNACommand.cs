using System.Collections;
using System.Collections.Generic;
using LitJson;

public class LoggingNACommand : NativeApiCommand
{
	string m_strContent;

	public LoggingNACommand() : base("Logging")
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override NativeApiResponse CreateResponse()
	{
		return new LoggingNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["content"] = m_strContent;
		joReq["url"] = CsConfiguration.Instance.SystemSetting.LoggingUrl;

		return joReq;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public string Content
	{
		get { return m_strContent; }
		set { m_strContent = value; }
	}
}

public class LoggingNAResponse : NativeApiResponse
{
	//---------------------------------------------------------------------------------------------------
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();
	}
}
