using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorClearGrade
{
	int m_nClearGrade;
	int m_nMinRemainTime;

	//---------------------------------------------------------------------------------------------------
	public int ClearGrade
	{
		get { return m_nClearGrade; }
	}

	public int MinRemainTime
	{
		get { return m_nMinRemainTime; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorClearGrade(WPDProofOfValorClearGrade proofOfValorClearGrade)
	{
		m_nClearGrade = proofOfValorClearGrade.clearGrade;
		m_nMinRemainTime = proofOfValorClearGrade.minRemainTime;
	}
}
