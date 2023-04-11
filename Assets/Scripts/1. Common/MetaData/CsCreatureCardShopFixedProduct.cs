using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-24)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardShopFixedProduct
{
	int m_nProductId;
	CsItem m_csItem;
	bool m_bItemOwned;
	int m_nSaleSoulPowder;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	public bool ItemOwned
	{
		get { return m_bItemOwned; }
	}

	public int SaleSoulPowder
	{
		get { return m_nSaleSoulPowder; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCardShopFixedProduct(WPDCreatureCardShopFixedProduct creatureCardShopFixedProduct)
	{
		m_nProductId = creatureCardShopFixedProduct.productId;
		m_csItem = CsGameData.Instance.GetItem(creatureCardShopFixedProduct.itemId);
		m_bItemOwned = creatureCardShopFixedProduct.itemOwned;
		m_nSaleSoulPowder = creatureCardShopFixedProduct.saleSoulPowder;
	}
}
