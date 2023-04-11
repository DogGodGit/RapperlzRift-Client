using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-14)
//---------------------------------------------------------------------------------------------------

public class CsGuildFundReward
{
	long m_lGuildFundRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long GuildFundRewardId
	{
		get { return m_lGuildFundRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildFundReward(WPDGuildFundReward guildFundReward)
	{
		m_lGuildFundRewardId = guildFundReward.guildFundRewardId;
		m_nValue = guildFundReward.value;
	}
}
