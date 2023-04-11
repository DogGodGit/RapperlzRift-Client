using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStaminaBuyCount
{
	int m_nBuyCount;
	int m_nStamina;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int BuyCount
	{
		get { return m_nBuyCount; }
	}

	public int Stamina
	{
		get { return m_nStamina; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStaminaBuyCount(WPDStaminaBuyCount staminaBuyCount)
	{
		m_nBuyCount = staminaBuyCount.buyCount;
		m_nStamina = staminaBuyCount.stamina;
		m_nRequiredDia = staminaBuyCount.requiredDia;
	}
}
