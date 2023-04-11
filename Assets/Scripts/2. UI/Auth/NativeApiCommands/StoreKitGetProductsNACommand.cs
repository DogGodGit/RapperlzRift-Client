using UnityEngine;
using System.Collections;
using System.Text;
using System.Linq;

using LitJson;

public class StoreKitGetProductsNACommand : NativeApiCommand
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Constructors

	public StoreKitGetProductsNACommand()
		: base("StoreKitGetProducts")
	{
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member variables

	string[] m_arrProducts;

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Properties

	public string[] Products
	{
		get { return m_arrProducts; }
		set { m_arrProducts = value; }
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override NativeApiResponse CreateResponse()
	{
		return new StoreKitGetProductsNAResponse();
	}

	protected override JsonData MakeRequestContent()
	{
		JsonData joReq = base.MakeRequestContent();

		StringBuilder sb = new StringBuilder();
		sb.Length = 0;
		
		for (int i = 0; i < m_arrProducts.Length; i++)
		{
			sb.Append(m_arrProducts[i]);

			if (i < m_arrProducts.Length - 1)
			{
				sb.Append("/");
			}
		}

		joReq["products"] = sb.ToString();
		
		return joReq;
	}
}

public class StoreKitGetProductsNAResponse : NativeApiResponse
{
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Constants

    public const int kResult_Cancled = 101;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Member variables
	
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Properties

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// Member functions

	protected override void HandleBody()
	{
		base.HandleBody();

		JsonData data = LitJsonUtil.GetArrayProperty(m_joContent, "products");
		foreach (JsonData skuDetails in data.Cast<JsonData>())
		{
			string sInAppProductKey = LitJsonUtil.GetStringProperty(skuDetails, "productId");
			string sPrice = LitJsonUtil.GetStringProperty(skuDetails, "price");

			CsCashProduct csCashProduct = CsGameData.Instance.CashProductList.Find(id => id.InAppProduct.InAppProductKey == sInAppProductKey);
			
			if (csCashProduct != null)
			{
				// SkuDetails

				csCashProduct.InAppProduct.GetInAppProductPrice().DisplayPrice = sPrice;
			}
		}
	}
}
