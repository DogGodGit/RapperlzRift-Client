using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

public class CsSecretLetterGrade
{
	int m_nGrade;
	CsExploitPointReward m_csExploitPointReward;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSecretLetterGrade(WPDSecretLetterGrade secretLetterGrade)
	{
		m_nGrade = secretLetterGrade.grade;
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(secretLetterGrade.exploitPointRewardId);
	}
}
