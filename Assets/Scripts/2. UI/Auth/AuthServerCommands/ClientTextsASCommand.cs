using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using WebCommon;

public class ClientTextsASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	int m_nLanguageId;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public ClientTextsASCommand()
		: base("ClientTextMetaDatas")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties
	public int languageId
	{
		get { return m_nLanguageId; }
		set { m_nLanguageId = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new ClientTextsASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		joReq["languageId"] = m_nLanguageId;
		return joReq;
	}
}

public class ClientTextsASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	public ClientTextsASResponse()
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			string sBase64String = Util.UnZipFromBase64(LitJsonUtil.GetStringProperty(m_joContent, "clientTexts"));

			WPDClientTexts clientTexts = new WPDClientTexts();
			clientTexts.DeserializeFromBase64String(sBase64String);

			CsConfiguration.Instance.Dic.Clear();

			foreach (WPDClientText clientText in clientTexts.clientTexts)
			{
				CsConfiguration.Instance.Dic.Add(clientText.name.Trim(), clientText.value.Trim());
			}

			// ClientTexts 저장.
			using (FileStream fsClientText = File.Create(CsConfiguration.Instance.ClientTextSavePath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fsClientText, sBase64String);

				fsClientText.Close();
				fsClientText.Dispose();
			}
		}
	}
}

