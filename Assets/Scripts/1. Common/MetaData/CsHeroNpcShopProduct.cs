using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-02)
//---------------------------------------------------------------------------------------------------

public class CsHeroNpcShopProduct
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
	public CsHeroNpcShopProduct(PDHeroNpcShopProduct heroNpcShopProduct)
	{
		m_nProductId = heroNpcShopProduct.productId;
		m_nBuyCount = heroNpcShopProduct.buyCount;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroNpcShopProduct(int nProductId, int nBuyCount)
	{
		m_nProductId = nProductId;
		m_nBuyCount = nBuyCount;
	}
}
