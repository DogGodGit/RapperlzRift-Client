using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-14)
//---------------------------------------------------------------------------------------------------

public class CsGuildDonationEntry
{
	int m_nEntryId;
	string m_strName;
	int m_nMoneyType;   // 1:골드, 2:다이아(귀속+비귀속)
	long m_lMoneyAmount;
	CsGuildContributionPointReward m_csGuildContributionPointReward;
	CsGuildFundReward m_csGuildFundReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryId
	{
		get { return m_nEntryId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int MoneyType
	{
		get { return m_nMoneyType; }
	}

	public long MoneyAmount
	{
		get { return m_lMoneyAmount; }
	}

	public CsGuildContributionPointReward GuildContributionPointReward
	{
		get { return m_csGuildContributionPointReward; }
	}

	public CsGuildFundReward GuildFundReward
	{
		get { return m_csGuildFundReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildDonationEntry(WPDGuildDonationEntry guildDonationEntry)
	{
		m_nEntryId = guildDonationEntry.entryId;
		m_strName = CsConfiguration.Instance.GetString(guildDonationEntry.nameKey);
		m_nMoneyType = guildDonationEntry.moneyType;   
		m_lMoneyAmount = guildDonationEntry.moneyAmount;
		m_csGuildContributionPointReward = CsGameData.Instance.GetGuildContributionPointReward(guildDonationEntry.guildContributionPointRewardId);
		m_csGuildFundReward = CsGameData.Instance.GetGuildFundReward(guildDonationEntry.guildFundRewardId);
	}

}
