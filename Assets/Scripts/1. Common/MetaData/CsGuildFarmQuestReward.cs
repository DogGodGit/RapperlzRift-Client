using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildFarmQuestReward
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
	public CsGuildFarmQuestReward(WPDGuildFarmQuestReward guildFarmQuestReward)
	{
		m_nLevel = guildFarmQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(guildFarmQuestReward.expRewardId);
	}
}
