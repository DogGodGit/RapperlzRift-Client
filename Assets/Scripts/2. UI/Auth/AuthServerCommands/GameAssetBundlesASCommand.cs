using System.Collections;
using System.Collections.Generic;

using LitJson;

public class GameAssetBundlesASCommand : AuthServerCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables


	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public GameAssetBundlesASCommand()
		: base("GameAssetBundles")
	{

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override AuthServerResponse CreateResponse()
	{
		return new GameAssetBundlesASResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();
		return joReq;
	}
}

public class GameAssetBundlesASResponse : AuthServerResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	private List<CsGameAssetBundle> m_csGameSetBundleList;
	
	
	public GameAssetBundlesASResponse()
	{
		m_csGameSetBundleList = new List<CsGameAssetBundle>();
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public List<CsGameAssetBundle> GameSetBundleList
	{
		get { return m_csGameSetBundleList; }
	}

	protected override void HandleBody()
	{
		base.HandleBody();

		int nResult = LitJsonUtil.GetIntProperty(m_joContent, "result");

		if (nResult == 0)
		{
			JsonData jsonDataGameAssetBundles = LitJsonUtil.GetArrayProperty(m_joContent, "gameAssetBundles");

			for (int i = 0; i < jsonDataGameAssetBundles.Count; i++)
			{
				m_csGameSetBundleList.Add(new CsGameAssetBundle(LitJsonUtil.GetIntProperty(jsonDataGameAssetBundles[i], "bundleNo"),
																LitJsonUtil.GetStringProperty(jsonDataGameAssetBundles[i], "fileName"),
																LitJsonUtil.GetIntProperty(jsonDataGameAssetBundles[i], "androidVer"),
																LitJsonUtil.GetIntProperty(jsonDataGameAssetBundles[i], "iosVer")));
			}
		}
	}
}


