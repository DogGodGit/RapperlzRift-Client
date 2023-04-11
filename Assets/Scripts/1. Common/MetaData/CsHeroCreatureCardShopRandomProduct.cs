using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-24)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreatureCardShopRandomProduct
{
	CsCreatureCardShopRandomProduct m_csCreatureCardShopRandomProduct;
	bool m_bPurchased;

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopRandomProduct CreatureCardShopRandomProduct
	{
		get { return m_csCreatureCardShopRandomProduct; }
	}

	public bool Purchased
	{
		get { return m_bPurchased; }
		set { m_bPurchased = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreatureCardShopRandomProduct(PDHeroCreatureCardShopRandomProduct heroCreatureCardShopRandomProduct)
	{
		m_csCreatureCardShopRandomProduct = CsGameData.Instance.GetCreatureCardShopRandomProduct(heroCreatureCardShopRandomProduct.productId);
		m_bPurchased = heroCreatureCardShopRandomProduct.purchased;
	}
}
