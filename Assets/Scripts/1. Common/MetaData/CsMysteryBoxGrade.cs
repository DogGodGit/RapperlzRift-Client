using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-08)
//---------------------------------------------------------------------------------------------------

public class CsMysteryBoxGrade
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
	public CsMysteryBoxGrade(WPDMysteryBoxGrade mysteryBoxGrade)
	{
		m_nGrade = mysteryBoxGrade.grade;
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(mysteryBoxGrade.exploitPointRewardId);
	}
}
