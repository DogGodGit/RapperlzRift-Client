//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-24)
//---------------------------------------------------------------------------------------------------

public class CsSystemSetting 
{
	string m_strAssetBundleUrl;
	string m_strTermsOfServiceUrl;
	string m_strPrivacyPolicyUrl;
	string m_strClientVersion;
	int m_nClientTextVersion;
	int m_nMetaDataVersion;
	bool m_bIsMaintenance;
	int m_nRecommendGameServerId;
	string m_strMaintenanceInfoUrl;

	string m_strLoggingUrl;
	bool m_bLoggingEnabled;
	string m_strStatusLoggingUrl;
	int m_nStatusLoggingInterval;
	string m_strGooglePublicKey;

	bool m_bHelpshiftSdkEnabled;
	bool m_bHelpshiftWebViewEnabled;
	string m_strHelpshiftUrl;

	string m_strAppStoreId;
	string m_strAuthUrl;	// 공유하기 용 url
	string m_strCsUrl;
	string m_strHomepageUrl;

	//---------------------------------------------------------------------------------------------------
	public string AssetBundleUrl
	{
		get { return m_strAssetBundleUrl; }
	}

	public string TermsOfServiceUrl
	{
		get { return m_strTermsOfServiceUrl; }
	}

	public string PrivacyPolicyUrl
	{
		get { return m_strPrivacyPolicyUrl; }
	}

	public string ClientVersion
	{
		get { return m_strClientVersion; }
	}

	public int ClientTextVersion
	{
		get { return m_nClientTextVersion; }
	}

	public int MetaDataVersion
	{
		get { return m_nMetaDataVersion; }
	}

	public bool IsMaintenance
	{
		get { return m_bIsMaintenance; }
	}

	public int RecommendGameServerId
	{
		get { return m_nRecommendGameServerId; }
	}

	public string MaintenanceInfoUrl
	{
		get { return m_strMaintenanceInfoUrl; }
	}

	public string LoggingUrl
	{
		get { return m_strLoggingUrl; }
	}

	public bool LoggingEnabled
	{
		get { return m_bLoggingEnabled; }
	}

	public string StatusLoggingUrl
	{
		get { return m_strStatusLoggingUrl; }
	}

	public int StatusLoggingInterval
	{
		get { return m_nStatusLoggingInterval; }
	}

	public string GooglePublicKey
	{
		get { return m_strGooglePublicKey; }
	}

	public bool HelpshiftSdkEnabled
	{
		get { return m_bHelpshiftSdkEnabled; }
	}

	public bool HelpshiftWebViewEnabled
	{
		get { return m_bHelpshiftWebViewEnabled; }
	}

	public string HelpshiftUrl
	{
		get { return m_strHelpshiftUrl; }
	}

	public string AppStoreId
	{
		get { return m_strAppStoreId; }
	}

	public string AuthUrl
	{
		get { return m_strAuthUrl; }
	}

	public string CsUrl
	{
		get { return m_strCsUrl; }
	}

	public string HomepageUrl
	{
		get { return m_strHomepageUrl; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemSetting(string strAssetBundleUrl,		string strTermsOfServiceUrl, string strPrivacyPolicyUrl, string strClientVersion,		int nClientTextVersion,
						   int nMetaDataVersion,			bool bIsMaintenance,		  int nRecommendGameServerId, string strMaintenanceInfoUrl, string strLoggingUrl,
						   bool bLoggingEnabled,			string strStatusLoggingUrl,  int nStatusLoggingInterval, string strGooglePublicKey,	bool bHelpshiftSdkEnabled,
						   bool bHelpshiftWebViewEnabled,	string strHelpshiftUrl,		 string strAppStoreId,		string strAuthUrl,			string strCsUrl,
						   string strHomepageUrl)
	{
		m_strAssetBundleUrl = strAssetBundleUrl;
		m_strTermsOfServiceUrl = strTermsOfServiceUrl;
		m_strPrivacyPolicyUrl = strPrivacyPolicyUrl;
		m_strClientVersion = strClientVersion;
		m_nClientTextVersion = nClientTextVersion;
		m_nMetaDataVersion = nMetaDataVersion;
		m_bIsMaintenance = bIsMaintenance;
		m_nRecommendGameServerId = nRecommendGameServerId;
		m_strMaintenanceInfoUrl = strMaintenanceInfoUrl;

		m_strLoggingUrl = strLoggingUrl;
		m_bLoggingEnabled = bLoggingEnabled;
		m_strStatusLoggingUrl = strStatusLoggingUrl;
		m_nStatusLoggingInterval = nStatusLoggingInterval;
		m_strGooglePublicKey = strGooglePublicKey;

		m_bHelpshiftSdkEnabled = bHelpshiftSdkEnabled;
		m_bHelpshiftWebViewEnabled = bHelpshiftWebViewEnabled;
		m_strHelpshiftUrl = strHelpshiftUrl;
		m_strAppStoreId = strAppStoreId;
		m_strAuthUrl = strAuthUrl;
		m_strCsUrl = strCsUrl;
		m_strHomepageUrl = strHomepageUrl;
	}


}
