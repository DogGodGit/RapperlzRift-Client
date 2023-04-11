using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-14)
//---------------------------------------------------------------------------------------------------

public class CsHolyWarQuestGloryLevel
{
	int m_nGloryLevel;
	int m_nRequiredKillCount;
	CsExploitPointReward m_csExploitPointReward;

	//---------------------------------------------------------------------------------------------------
	public int GloryLevel
	{
		get { return m_nGloryLevel; }
	}

	public int RequiredKillCount
	{
		get { return m_nRequiredKillCount; }
	}

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHolyWarQuestGloryLevel(WPDHolyWarQuestGloryLevel holyWarQuestGloryLevel)
	{
		m_nGloryLevel = holyWarQuestGloryLevel.gloryLevel;
		m_nRequiredKillCount = holyWarQuestGloryLevel.requiredKillCount;
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(holyWarQuestGloryLevel.exploitPointRewardId);
	}
}
