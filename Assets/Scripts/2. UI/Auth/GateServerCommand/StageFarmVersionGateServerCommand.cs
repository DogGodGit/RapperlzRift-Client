using UnityEngine;
using System.Collections;

using LitJson;


public class StageFarmVersionGateServerCommand : GateServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	int m_nPlatformId = 0; // 1 : Android, 2 : iOS
	string m_strVersionName = null;
	int m_nBuildNo = 0;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public StageFarmVersionGateServerCommand()
		: base("StageFarmVersion")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public int PlatformId
	{
		get { return m_nPlatformId; }
		set { m_nPlatformId = value; }
	}

	public string VersionName
	{
		get { return m_strVersionName; }
		set { m_strVersionName = value; }
	}

	public int BuildNo
	{
		get { return m_nBuildNo; }
		set { m_nBuildNo = value; }
	}


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override GateServerResponse CreateResponse()
	{
		return new StageFarmVersionGateServerResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["platformId"] = m_nPlatformId;
		joReq["versionName"] = m_strVersionName;
		joReq["buildNo"] = m_nBuildNo;
		return joReq;
	}
}

public class StageFarmVersionGateServerResponse : GateServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constants

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	int m_nFarmId = 0;
	string m_strName = null;
	string m_strServerUrl = null; // GateServer ApiUrl 
	string m_strDownloadUrl = null; // result가 101인경우. 앱다운로드 마켓 경로
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public int FarmId
	{
		get { return m_nFarmId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ServerUrl
	{
		get { return m_strServerUrl; }
	}

	public string DownloadUrl
	{
		get { return m_strDownloadUrl; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();
		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 101)
		{
			m_strDownloadUrl = LitJsonUtil.GetStringProperty(m_joContent, "downloadUrl");
		}
		else
		{
			JsonData jsonDataStageFarmVersion = LitJsonUtil.GetObjectProperty(m_joContent, "stageFarmVersion");

			m_nFarmId = LitJsonUtil.GetIntProperty(jsonDataStageFarmVersion, "farmId");
			m_strName = LitJsonUtil.GetStringProperty(jsonDataStageFarmVersion, "name");
			m_strServerUrl = LitJsonUtil.GetStringProperty(jsonDataStageFarmVersion, "serverUrl");
		}
	}
}
