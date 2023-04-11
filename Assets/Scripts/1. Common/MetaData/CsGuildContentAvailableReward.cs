using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-23)
//---------------------------------------------------------------------------------------------------

public class CsGuildContentAvailableReward
{
	int m_nGuildContentId;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int GuildContentId
	{
		get { return m_nGuildContentId; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public CsItem Item
	{
		get { return m_csItem; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildContentAvailableReward(WPDGuildContentAvailableReward guildContentAvailableReward)
	{
		m_nGuildContentId = guildContentAvailableReward.guildContentId;
		m_nRewardNo = guildContentAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(guildContentAvailableReward.itemId);
	}
}
