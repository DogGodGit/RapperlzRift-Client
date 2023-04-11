using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

public enum EnMainQuestRewardType
{
	MainGear = 1,
	SubGear = 2,
	Item = 3,
	Mount = 4,
	CreatureCard = 5,
}

public class CsMainQuestReward
{
	int m_nMainQuestNo;					// 퀘스트번호
	int m_nRewardNo;					// 보상번호
	int m_nType;                        // 타입(1:메인장비,2:보조장비,3:아이템,4탈것,5:크리처카드)
	int m_nJobId;                       // 직업ID
	CsMainGear m_csMainGear;            // 메인장비
	bool m_bMainGearOwned;              // 메인장비귀속여부
	CsSubGear m_csSubGear;              // 보조장비
	CsItemReward m_csItemReward;        // 보상아이템
	CsMount m_csMount;                  // 탈것
	CsCreatureCard m_csCreatureCard;    // 크리처카드ID

	//---------------------------------------------------------------------------------------------------
	public int MainQuestNo
	{
		get { return m_nMainQuestNo; }
	}

	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int JobId
	{
		get { return m_nJobId; }
	}

	public CsMainGear MainGear
	{
		get { return m_csMainGear; }
	}

	public bool MainGearOwned
	{
		get { return m_bMainGearOwned; }
	}

	public CsSubGear SubGear
	{
		get { return m_csSubGear; }
	}

	public CsItem Item
	{
		get { return m_csItemReward.Item; }
	}

	public int ItemCount
	{
		get { return m_csItemReward.ItemCount; }
	}

	public bool ItemOwned
	{
		get { return m_csItemReward.ItemOwned; }
	}

	public CsMount Mount
	{
		get { return m_csMount; }
	}

	public CsCreatureCard CreatureCard
	{
		get { return m_csCreatureCard; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestReward(WPDMainQuestReward mainQuestRewardItem)
	{
		m_nMainQuestNo = mainQuestRewardItem.mainQuestNo;
		m_nRewardNo = mainQuestRewardItem.rewardNo;
		m_nType = mainQuestRewardItem.type;
		m_nJobId = mainQuestRewardItem.jobId;
		m_csMainGear = CsGameData.Instance.GetMainGear(mainQuestRewardItem.mainGearId);
		m_bMainGearOwned = mainQuestRewardItem.mainGearOwned;
		m_csSubGear = CsGameData.Instance.GetSubGear(mainQuestRewardItem.subGearId);
		m_csItemReward = CsGameData.Instance.GetItemReward(mainQuestRewardItem.itemRewardId);
		m_csMount = CsGameData.Instance.GetMount(mainQuestRewardItem.mountId);
		m_csCreatureCard = CsGameData.Instance.GetCreatureCard(mainQuestRewardItem.creatureCardId);
	}
}
