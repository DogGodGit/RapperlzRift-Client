using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsAttainmentEntryReward
{
	int m_nEntryNo;
	int m_nRewardNo;
	int m_nType;                // 1:메인장비,2:아이템
	CsMainGear m_csMainGear;
	bool m_bMainGearOwned;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int EntryNo
	{
		get { return m_nEntryNo; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	public bool MainGearOwned
	{
		get { return m_bMainGearOwned; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAttainmentEntryReward(WPDAttainmentEntryReward attainmentEntryReward)
	{
		m_nEntryNo = attainmentEntryReward.entryNo;
		m_nRewardNo = attainmentEntryReward.rewardNo;
		m_nType = attainmentEntryReward.type;
		m_csMainGear = CsGameData.Instance.GetMainGear(attainmentEntryReward.mainGearId);
		m_bMainGearOwned = attainmentEntryReward.mainGearOwned;
		m_csItemReward = CsGameData.Instance.GetItemReward(attainmentEntryReward.itemRewardId);
	}
}
