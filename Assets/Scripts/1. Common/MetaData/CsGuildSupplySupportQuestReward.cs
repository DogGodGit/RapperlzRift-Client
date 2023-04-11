using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-05)
//---------------------------------------------------------------------------------------------------

public class CsGuildSupplySupportQuestReward
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
	public CsGuildSupplySupportQuestReward(WPDGuildSupplySupportQuestReward guildSupplySupportQuestReward)
	{
		m_nLevel = guildSupplySupportQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(guildSupplySupportQuestReward.expRewardId);
	}
}
