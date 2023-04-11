using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValorReward
{
	int m_nHeroLevel;
	CsExpReward m_csExpRewardSuccess;
	CsExpReward m_csExpRewardFailure;

	//---------------------------------------------------------------------------------------------------
	public int HeroLevel
	{
		get { return m_nHeroLevel; }
	}

	public CsExpReward SuccessExpReward
	{
		get { return m_csExpRewardSuccess; }
	}

	public CsExpReward FailureExpReward
	{
		get { return m_csExpRewardFailure; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorReward(WPDProofOfValorReward proofOfValorReward)
	{
		m_nHeroLevel = proofOfValorReward.heroLevel;
		m_csExpRewardSuccess = CsGameData.Instance.GetExpReward(proofOfValorReward.successExpRewardId);
		m_csExpRewardFailure = CsGameData.Instance.GetExpReward(proofOfValorReward.failureExpRewardId);

	}
}
