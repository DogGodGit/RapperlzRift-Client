using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsAuthServerInfoNACommand : NativeApiCommand 
{
	public CsAuthServerInfoNACommand() : base("AuthServerInfo")
	{
	}

	//---------------------------------------------------------------------------------------------------
	
	protected override NativeApiResponse CreateResponse()
	{
		return new CsAuthServerInfoNAResponse();
	}
}

public class CsAuthServerInfoNAResponse : NativeApiResponse
{
	int m_nPlatformId;
	int m_nBuildNo;
	string m_strGateServerApiUrl;
	string m_strClientVersion;
	
	//---------------------------------------------------------------------------------------------------
	
	public int PlatformId
	{
		get { return m_nPlatformId; }
	}

	public int BuildNo
	{
		get { return m_nBuildNo; }
	}

	public string GateServerApiUrl
	{
		get { return m_strGateServerApiUrl; }
	}
	
	public string ClientVersion
	{
		get { return m_strClientVersion; }
	}

	//---------------------------------------------------------------------------------------------------
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		m_nPlatformId = LitJsonUtil.GetIntProperty(m_joContent, "platformId");
		m_nBuildNo = LitJsonUtil.GetIntProperty(m_joContent, "buildNo");
		m_strGateServerApiUrl = LitJsonUtil.GetStringProperty(m_joContent, "gateServerApiUrl");
		m_strClientVersion = LitJsonUtil.GetStringProperty(m_joContent, "versionName");
	}
}
