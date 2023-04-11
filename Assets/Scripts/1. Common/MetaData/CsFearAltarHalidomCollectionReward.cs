using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltarHalidomCollectionReward
{
	int m_nRewardNo;
	int m_nCollectionCount;
	CsItemReward m_csItemReward;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int CollectionCount
	{
		get { return m_nCollectionCount; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidomCollectionReward(WPDFearAltarHalidomCollectionReward fearAltarHalidomCollectionReward)
	{
		m_nRewardNo = fearAltarHalidomCollectionReward.rewardNo;
		m_nCollectionCount = fearAltarHalidomCollectionReward.collectionCount;
		m_csItemReward = CsGameData.Instance.GetItemReward(fearAltarHalidomCollectionReward.itemRewardId);
	}
}
