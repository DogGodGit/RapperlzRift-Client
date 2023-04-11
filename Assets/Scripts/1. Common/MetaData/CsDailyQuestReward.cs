using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsDailyQuestReward
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
	public CsDailyQuestReward(WPDDailyQuestReward dailyQuestReward)
	{
		m_nLevel = dailyQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(dailyQuestReward.expRewardId);
	}
}
