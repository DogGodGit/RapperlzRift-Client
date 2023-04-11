using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleStepReward
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
	public CsWisdomTempleStepReward(WPDWisdomTempleStepReward wisdomTempleStepReward)
	{
		m_nLevel = wisdomTempleStepReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(wisdomTempleStepReward.expRewardId);
	}
}
