using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsAnkouTombAvailableReward
{
	int m_nDifficulty;
	int m_nRewardNo;
	CsItem m_csItem;

	//---------------------------------------------------------------------------------------------------
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
	public CsAnkouTombAvailableReward(WPDAnkouTombAvailableReward ankouTombAvailableReward)
	{
		m_nDifficulty = ankouTombAvailableReward.difficulty;
		m_nRewardNo = ankouTombAvailableReward.rewardNo;
		m_csItem = CsGameData.Instance.GetItem(ankouTombAvailableReward.itemId);
	}
}
