using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonAvailableReward
{
	int m_nDungeonNo;
	int m_nDifficulty;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
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
	public CsStoryDungeonAvailableReward(WPDStoryDungeonAvailableReward storyDungeonAvailableReward)
	{
		m_nDungeonNo = storyDungeonAvailableReward.dungeonNo;
		m_nDifficulty = storyDungeonAvailableReward.difficulty;
		m_nRewardNo = storyDungeonAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(storyDungeonAvailableReward.itemId);
	}
}
