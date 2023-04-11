using System.Collections;
using System.Collections.Generic;

using LitJson;

public class SystemSettingASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public SystemSettingASCommand()
		: base("SystemSetting")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new SystemSettingASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		//joReq["storeType"] = AuthSettings.StoreType;
		return joReq;
	}
}

public class SystemSettingASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	CsSystemSetting m_csSystemSetting;
	
	public SystemSettingASResponse()
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public CsSystemSetting SystemSetting
	{
		get { return m_csSystemSetting; }
	}

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			JsonData jsonDataSystemSetting = LitJsonUtil.GetObjectProperty(m_joContent, "systemSetting");

			m_csSystemSetting = new CsSystemSetting(LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "assetBundleUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "termsOfServiceUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "privacyPolicyUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "clientVersion"),
													LitJsonUtil.GetIntProperty(jsonDataSystemSetting, "clientTextVersion"),
													LitJsonUtil.GetIntProperty(jsonDataSystemSetting, "metaDataVersion"),
													LitJsonUtil.GetBooleanProperty(jsonDataSystemSetting, "isMaintenance"),
													LitJsonUtil.GetIntProperty(jsonDataSystemSetting, "recommendGameServerId"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "maintenanceInfoUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "loggingUrl"),
													LitJsonUtil.GetBooleanProperty(jsonDataSystemSetting, "loggingEnabled"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "statusLoggingUrl"),
													LitJsonUtil.GetIntProperty(jsonDataSystemSetting, "statusLoggingInterval"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "googlePublicKey"),
													LitJsonUtil.GetBooleanProperty(jsonDataSystemSetting, "helpshiftSdkEnabled"),
													LitJsonUtil.GetBooleanProperty(jsonDataSystemSetting, "helpshiftWebViewEnabled"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "helpshiftUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "appStoreId"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "authUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "csUrl"),
													LitJsonUtil.GetStringProperty(jsonDataSystemSetting, "homepageUrl"));
		}
	}
}