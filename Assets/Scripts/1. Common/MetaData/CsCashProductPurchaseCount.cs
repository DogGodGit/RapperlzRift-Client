using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsCashProductPurchaseCount
{
	int m_nProductId;
	int m_nCount;

	//---------------------------------------------------------------------------------------------------
	public int ProductId
	{
		get { return m_nProductId; }
	}

	public int Count
	{
		get { return m_nCount; }
		set { m_nCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCashProductPurchaseCount(PDCashProductPurchaseCount cashProductPurchaseCount)
	{
		m_nProductId = cashProductPurchaseCount.productId;
		m_nCount = cashProductPurchaseCount.count;
	}

	//---------------------------------------------------------------------------------------------------
	public CsCashProductPurchaseCount(int nProductId, int nCount)
	{
		m_nProductId = nProductId;
		m_nCount = nCount;
	}
}
