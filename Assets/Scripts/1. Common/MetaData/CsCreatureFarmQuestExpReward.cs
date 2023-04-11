using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsCreatureFarmQuestExpReward
{
	int m_nLevel;
	CsExpReward m_csExpReward;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsCreatureFarmQuestExpReward(WPDCreatureFarmQuestExpReward creatureFarmQuestExpReward)
	{
		m_nLevel = creatureFarmQuestExpReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(creatureFarmQuestExpReward.expRewardId);
	}
}
