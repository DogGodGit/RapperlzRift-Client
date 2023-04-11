using System.Collections;
using System.Collections.Generic;

using LitJson;

public class SupportedLanguagesASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public SupportedLanguagesASCommand()
		: base("SupportedLanguages")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new SupportedLanguagesASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		return joReq;
	}
}


public class SupportedLanguagesASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	int m_nDefaultLanague;
	Dictionary<int, string> m_dicSupportedLanguageList;
	string m_strMaintenanceInfoUrl;

	public SupportedLanguagesASResponse()
	{
		m_dicSupportedLanguageList = new Dictionary<int, string>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public int DefaultLanague
	{
		get { return m_nDefaultLanague; }
	}

	public Dictionary<int, string> SupportedLanguageList
	{
		get { return m_dicSupportedLanguageList; }
	}

	public string MaintenanceInfoUrl
	{
		get { return m_strMaintenanceInfoUrl; }
	}

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			m_nDefaultLanague = LitJsonUtil.GetIntProperty(m_joContent, "defaultLanguageId");
			
			JsonData jsonDataSupportedLanguage = LitJsonUtil.GetArrayProperty(m_joContent, "supportedLanguages");

			for (int i = 0; i < jsonDataSupportedLanguage.Count; i++)
			{
				m_dicSupportedLanguageList.Add(LitJsonUtil.GetIntProperty(jsonDataSupportedLanguage[i], "languageId"), LitJsonUtil.GetStringProperty(jsonDataSupportedLanguage[i], "maintenanceInfoUrl"));
			}
		}
	}
}
