using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-24)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardShopRandomProduct
{
	int m_nProductId;
	CsCreatureCard m_csCreatureCard;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public CsCreatureCard CreatureCard
	{
		get { return m_csCreatureCard; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopRandomProduct(WPDCreatureCardShopRandomProduct creatureCardShopRandomProduct)
	{
		m_nProductId = creatureCardShopRandomProduct.productId;
		m_csCreatureCard = CsGameData.Instance.GetCreatureCard(creatureCardShopRandomProduct.creatureCardId);
	}
}
