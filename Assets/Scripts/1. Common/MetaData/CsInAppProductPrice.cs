using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsInAppProductPrice
{
	string m_strInAppProductKey;
	int m_nStoreType;				// 1:구글,2:애플
	string m_strCurrencyCode;
	string m_strDisplayPrice;

	//---------------------------------------------------------------------------------------------------
	public string InAppProductKey
	{
		get { return m_strInAppProductKey; }
	}

	public int StoreType
	{
		get { return m_nStoreType; }
	}

	public string CurrencyCode
	{
		get { return m_strCurrencyCode; }
	}

	public string DisplayPrice
	{
		get 
		{
			int nPrice;

			if (int.TryParse(m_strDisplayPrice, out nPrice))
			{
				return nPrice.ToString("#,##0");
			}
			else
			{
				return m_strDisplayPrice;
			}
		}
		set
		{
		    m_strDisplayPrice = value;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsInAppProductPrice(WPDInAppProductPrice inAppProductPrice)
	{
		m_strInAppProductKey = inAppProductPrice.inAppProductKey;
		m_nStoreType = inAppProductPrice.storeType;
		m_strCurrencyCode = inAppProductPrice.currencyCode;
		m_strDisplayPrice = inAppProductPrice.displayPrice;
	}
}
