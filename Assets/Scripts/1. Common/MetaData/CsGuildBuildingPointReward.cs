using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-14)
//---------------------------------------------------------------------------------------------------

public class CsGuildBuildingPointReward
{
	long m_lGuildBuildingPointRewardId;
	int m_nValue;

	//---------------------------------------------------------------------------------------------------
	public long GuildBuildingPointRewardId
	{
		get { return m_lGuildBuildingPointRewardId; }
	}

	public int Value
	{
		get { return m_nValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildBuildingPointReward(WPDGuildBuildingPointReward guildBuildingPointReward)
	{
		m_lGuildBuildingPointRewardId = guildBuildingPointReward.guildBuildingPointRewardId;
		m_nValue = guildBuildingPointReward.value;
	}
}
