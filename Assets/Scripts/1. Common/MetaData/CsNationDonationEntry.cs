using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsNationDonationEntry
{
	int m_nEntryId;
	string m_strName;
	int m_nMoneyType;       // 1:골드, 2:다이아(귀속+비귀속)
	long m_lMoneyAmount;
	CsExploitPointReward m_csExploitPointReward;
	CsNationFundReward m_csNationFundReward;

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

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	public CsNationFundReward NationFundReward
	{
		get { return m_csNationFundReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsNationDonationEntry(WPDNationDonationEntry nationDonationEntry)
	{
		m_nEntryId = nationDonationEntry.entryId;
		m_strName = CsConfiguration.Instance.GetString(nationDonationEntry.nameKey);
		m_nMoneyType = nationDonationEntry.moneyType;
		m_lMoneyAmount = nationDonationEntry.moneyAmount;
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(nationDonationEntry.exploitPointRewardId);
		m_csNationFundReward = CsGameData.Instance.GetNationFundReward(nationDonationEntry.nationFundRewardId);
	}
}
