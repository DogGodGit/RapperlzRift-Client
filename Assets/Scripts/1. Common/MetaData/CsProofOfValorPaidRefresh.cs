using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorPaidRefresh
{
	int m_nRefreshCount;
	int m_nRequiredDia;

	//---------------------------------------------------------------------------------------------------
	public int RefreshCount
	{
		get { return m_nRefreshCount; }
	}

	public int RequiredDia
	{
		get { return m_nRequiredDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorPaidRefresh(WPDProofOfValorPaidRefresh proofOfValorPaidRefresh)
	{
		m_nRefreshCount = proofOfValorPaidRefresh.refreshCount;
		m_nRequiredDia = proofOfValorPaidRefresh.requiredDia;
	}
}
