using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-27)
//---------------------------------------------------------------------------------------------------

public class CsGuildMissionQuestReward
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
	public CsGuildMissionQuestReward(WPDGuildMissionQuestReward guildMissionQuestReward)
	{
		m_nLevel = guildMissionQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(guildMissionQuestReward.expRewardId);
	}
}
