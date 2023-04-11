using System.Collections;
using System.Collections.Generic;

public class BatteryStatusNACommand : NativeApiCommand
{
	public BatteryStatusNACommand() : base("BatteryStatus")
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override NativeApiResponse CreateResponse()
	{
		return new BatteryStatusNAResponse();
	}
}

public class BatteryStatusNAResponse : NativeApiResponse
{
	string m_strBatteryStatus;  //  ( 0 ~ 100 )
	string m_strChargeType;     // ( unPlugged, AC, USB, WIRELESS )

	//---------------------------------------------------------------------------------------------------
	public string BatteryStatus
	{
		get { return m_strBatteryStatus; }
	}

	public string ChargeType
	{
		get { return m_strChargeType; }
	}

	//---------------------------------------------------------------------------------------------------
	protected override void HandleBody()
	{
		base.HandleBody();

		m_strBatteryStatus = LitJsonUtil.GetStringProperty(m_joContent, "batteryStatus");
		m_strChargeType = LitJsonUtil.GetStringProperty(m_joContent, "chargeType");
	}
}
