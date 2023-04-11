using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LitJson;
using WebCommon;

public class GameDatasASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public GameDatasASCommand()
		: base("GameMetaDatas")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties	

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new GameDatasASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();

		return joReq;
	}
}

public class GameDatasASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables
	WPDGameDatas m_gameData;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public WPDGameDatas GameData
	{
		get { return m_gameData; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			string sBase64String = Util.UnZipFromBase64(LitJsonUtil.GetStringProperty(m_joContent, "gameDatas"));

			m_gameData = new WPDGameDatas();
			m_gameData.DeserializeFromBase64String(sBase64String);

			// GameMetaData 저장.
			using (FileStream fsGameMetaData = File.Create(CsConfiguration.Instance.GameMetaDataSavePath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fsGameMetaData, sBase64String);

				fsGameMetaData.Close();
				fsGameMetaData.Dispose();
			}
		}
	}
}
