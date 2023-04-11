using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-14)
//---------------------------------------------------------------------------------------------------

public class CsGuildContributionPointReward
{
	long m_lGuildContributionPointRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long GuildContributionPointRewardId
	{
		get { return m_lGuildContributionPointRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildContributionPointReward(WPDGuildContributionPointReward guildContributionPointReward)
	{
		m_lGuildContributionPointRewardId = guildContributionPointReward.guildContributionPointRewardId;
		m_nValue = guildContributionPointReward.value;
	}
}
