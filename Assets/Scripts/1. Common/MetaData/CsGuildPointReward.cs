using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-14)
//---------------------------------------------------------------------------------------------------

public class CsGuildPointReward
{
	long m_lGuildPointRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long GuildPointRewardId
	{
		get { return m_lGuildPointRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildPointReward(WPDGuildPointReward guildPointReward)
	{
		m_lGuildPointRewardId = guildPointReward.guildPointRewardId;
		m_nValue = guildPointReward.value;
	}
}
