using System.Collections;
using System.Collections.Generic;

public class NetworkStatusNACommand : NativeApiCommand
{
	public NetworkStatusNACommand() : base("NetworkStatus")
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override NativeApiResponse CreateResponse()
	{
		return new NetworkStatusNAResponse();
	}
}

public class NetworkStatusNAResponse : NativeApiResponse
{
	string m_strNetworkType;        // WIFI, LTE, 3G
	string m_strSignalStrength;

	//---------------------------------------------------------------------------------------------------
	public string NetworkType
	{
		get { return m_strNetworkType; }
	}

	public string SignalStrength
	{
		get { return m_strSignalStrength; }
	}

	//---------------------------------------------------------------------------------------------------

	protected override void HandleBody()
	{
		base.HandleBody();

		m_strNetworkType = LitJsonUtil.GetStringProperty(m_joContent, "networkType");
		m_strSignalStrength = LitJsonUtil.GetStringProperty(m_joContent, "signalStrength");
	}
}
