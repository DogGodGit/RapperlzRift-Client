using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsInAppProduct
{
	string m_strInAppProductKey;

	List<CsInAppProductPrice> m_listCsInAppProductPrice;

	//---------------------------------------------------------------------------------------------------
	public string InAppProductKey
	{
		get { return m_strInAppProductKey; }
	}

	public List<CsInAppProductPrice> InAppProductPriceList
	{
		get { return m_listCsInAppProductPrice; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInAppProduct(WPDInAppProduct inAppProduct)
	{
		m_strInAppProductKey = inAppProduct.inAppProductKey;

		m_listCsInAppProductPrice = new List<CsInAppProductPrice>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsInAppProductPrice GetInAppProductPrice()
	{
		for (int i = 0; i < m_listCsInAppProductPrice.Count; i++)
		{
			if (m_listCsInAppProductPrice[i].StoreType == (int)CsConfiguration.Instance.PlatformId)
			{
				return m_listCsInAppProductPrice[i];
			}
		}

		return null;
	}
}
