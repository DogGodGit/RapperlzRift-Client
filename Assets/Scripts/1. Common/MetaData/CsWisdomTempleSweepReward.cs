using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWisdomTempleSweepReward
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
	public CsWisdomTempleSweepReward(WPDWisdomTempleSweepReward wisdomTempleSweepReward)
	{
		m_nLevel = wisdomTempleSweepReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(wisdomTempleSweepReward.expRewardId);
	}
}
