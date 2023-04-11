using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-12)
//---------------------------------------------------------------------------------------------------

public class CsGuildLevel
{
	int m_nLevel;
	int m_nMaxMemberCount;
	CsItemReward m_csItemRewardDaily;
	CsItemReward m_csItemRewardAltar;

	//---------------------------------------------------------------------------------------------------
	public int Level
	{
		get { return m_nLevel; }
	}

	public int MaxMemberCount
	{
		get { return m_nMaxMemberCount; }
	}

	public CsItemReward DailyItemReward
	{
		get { return m_csItemRewardDaily; }
	}

	public CsItemReward AltarItemReward
	{
		get { return m_csItemRewardAltar; }
	}
	//---------------------------------------------------------------------------------------------------
	public CsGuildLevel(WPDGuildLevel guildLevel)
	{
		m_nLevel = guildLevel.level;
		m_nMaxMemberCount = guildLevel.maxMemberCount;
		m_csItemRewardDaily = CsGameData.Instance.GetItemReward(guildLevel.dailyItemRewardId);
		m_csItemRewardAltar = CsGameData.Instance.GetItemReward(guildLevel.altarItemRewardId);
	}
}
