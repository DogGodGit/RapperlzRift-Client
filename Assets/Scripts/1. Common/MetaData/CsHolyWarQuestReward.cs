using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-09)
//---------------------------------------------------------------------------------------------------

public class CsHolyWarQuestReward
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
	public CsHolyWarQuestReward(WPDHolyWarQuestReward holyWarQuestReward)
	{
		m_nLevel = holyWarQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(holyWarQuestReward.expRewardId);
	}
}
