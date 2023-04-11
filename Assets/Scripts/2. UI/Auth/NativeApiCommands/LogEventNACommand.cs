using System.Collections;
using System.Collections.Generic;
using LitJson;

public class LogEventNACommand : NativeApiCommand
{
	string m_strName;
	string m_strValue;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
		set { m_strName = value; }
	}

	public string Value
	{
		get { return m_strValue; }
		set { m_strValue = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public LogEventNACommand() : base("LogEvent")
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override NativeApiResponse CreateResponse()
	{
		return new LogEventNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["name"] = m_strName;
		joReq["value"] = m_strValue;

		return joReq;
	}

}

public class LogEventNAResponse : NativeApiResponse
{
	//---------------------------------------------------------------------------------------------------
	protected override void HandleBody()
	{
		base.HandleBody();
	}
}
