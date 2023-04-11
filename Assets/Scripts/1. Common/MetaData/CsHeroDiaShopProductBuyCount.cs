using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsHeroDiaShopProductBuyCount
{
	int m_nProductId;
	int m_nBuyCount;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public int BuyCount
	{
		get { return m_nBuyCount; }
		set { m_nBuyCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroDiaShopProductBuyCount(PDHeroDiaShopProductBuyCount heroDiaShopProductBuyCount)
	{
		m_nProductId = heroDiaShopProductBuyCount.productId;
		m_nBuyCount = heroDiaShopProductBuyCount.buyCount;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroDiaShopProductBuyCount(int nProductId, int nBuyCount)
	{
		m_nProductId = nProductId;
		m_nBuyCount = nBuyCount;
	}
}
