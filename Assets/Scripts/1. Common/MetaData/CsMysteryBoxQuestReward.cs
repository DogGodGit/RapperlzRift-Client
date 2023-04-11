using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-08)
//---------------------------------------------------------------------------------------------------

public class CsMysteryBoxQuestReward
{
	CsMysteryBoxGrade m_csMysteryBoxGrade;
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public CsMysteryBoxGrade MysteryBoxGrade
	{
		get { return m_csMysteryBoxGrade; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMysteryBoxQuestReward(WPDMysteryBoxQuestReward mysteryBoxQuestReward)
	{
		m_csMysteryBoxGrade = CsGameData.Instance.GetMysteryBoxGrade(mysteryBoxQuestReward.grade);
		m_nLevel = mysteryBoxQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(mysteryBoxQuestReward.expRewardId);
	}
}
