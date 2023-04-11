using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-12)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyQuestRoundReward
{
	int m_nRoundNo;
	int m_nLevel;
	CsExpReward m_csExpReward;
	CsGoldReward m_csGoldReward;

	//---------------------------------------------------------------------------------------------------
	public int RoundNo
	{
		get { return m_nRoundNo; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyQuestRoundReward(WPDWeeklyQuestRoundReward weeklyQuestRoundReward)
	{
		m_nRoundNo = weeklyQuestRoundReward.roundNo;
		m_nLevel = weeklyQuestRoundReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(weeklyQuestRoundReward.expRewardId);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(weeklyQuestRoundReward.goldRewardId);
	}
}
