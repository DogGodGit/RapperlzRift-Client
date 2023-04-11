using UnityEngine;
using System.Collections;
using LitJson;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class StoreKitPurchaseNACommand : NativeApiCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public StoreKitPurchaseNACommand()
		: base("StoreKitPurchase")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	string m_sAuthServerUrl = "";
	string m_sUserAccessToken = "";
	int m_nVirtualGameServerId = 0;
	string m_sHeroId = "";
	string m_sProductId = "";
	string m_sLogId = "";
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string AuthServerUrl 
	{
		get { return m_sAuthServerUrl; }
		set { m_sAuthServerUrl = value; } 
	}
	public string UserAccessToken
	{
		get { return m_sUserAccessToken; }
		set { m_sUserAccessToken = value; }
	}
	public int VirtualGameServerId
	{
		get { return m_nVirtualGameServerId; }
		set { m_nVirtualGameServerId = value; }
	}
	public string HeroId
	{
		get { return m_sHeroId; }
		set { m_sHeroId = value; }
	}
	public string ProductId
	{
		get { return m_sProductId; }
		set { m_sProductId = value; }
	}
	public string LogId
	{
		get { return m_sLogId; }
		set { m_sLogId = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override NativeApiResponse CreateResponse()
	{
		return new StoreKitPurchaseNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();

		joReq["url"] = m_sAuthServerUrl;
		joReq["userAccessToken"] = m_sUserAccessToken;
		joReq["virtualGameServerId"] = m_nVirtualGameServerId;
		joReq["heroId"] = m_sHeroId;
		joReq["productId"] = m_sProductId;
		joReq["developerPayload"] = m_sLogId;
		
		return joReq;
	}
}

public class StoreKitPurchaseNAResponse : NativeApiResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Constants

    public const int kResult_Canceled = 101;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Member variables

    private string m_sLogId = null;
	//string m_sSkuDetails = null;
	//string m_sPurchaseData = null;
	//string m_sDataSignature = null;

	//string m_sTransactionId = null;
	
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Properties

	public string logId { get { return m_sLogId; }}
	//public string SkuDetails { get { return m_sSkuDetails; }}
	//public string PurchaseData { get { return m_sPurchaseData; }}
	//public string DataSignature { get { return m_sDataSignature; }}

	//public string TransactionId { get { return m_sTransactionId; }}
	
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		m_sLogId = LitJsonUtil.GetStringProperty(m_joContent, "logId");

//        if (Config.instance.UseTapjoy() && Config.instance.ServiceMode != EnServiceMode.Entermate)
//        {
//#if UNITY_ANDROID
//            m_sSkuDetails = LitJsonUtil.GetStringProperty(m_joContent, "skuDetails");

//            // 결제취소 발생 시 -> native쪽 프로퍼티와 일치하는지 확인
//            //m_sPurchaseData = LitJsonUtil.GetStringProperty(m_joContent, "purchaseData");
//            //m_sDataSignature = LitJsonUtil.GetStringProperty(m_joContent, "signature");
//#elif UNITY_IOS
//            m_sTransactionId = LitJsonUtil.GetStringProperty(m_joContent, "transactionId");
//#endif
//        }
		
		//m_sCampaignId = LitJsonUtil.GetStringProperty(m_joContent, "campaignId");
    }
}
