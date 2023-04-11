using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-19)
//---------------------------------------------------------------------------------------------------

public class CsRetrievalReward
{
	int m_nRetrievalId;
	int m_nLevel;
	CsExpReward m_csExpRewardGold;
	CsItemReward m_csItemRewardGold;
	CsExpReward m_csExpRewardDia;
	CsItemReward m_csItemRewardDia;

	//---------------------------------------------------------------------------------------------------
	public int RetrievalId
	{
		get { return m_nRetrievalId; }
	}
	
	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward GoldExpReward
	{
		get { return m_csExpRewardGold; }
	}

	public CsItemReward GoldItemReward
	{
		get { return m_csItemRewardGold; }
	}

	public CsExpReward DiaExpReward
	{
		get { return m_csExpRewardDia; }
	}

	public CsItemReward DiaItemReward
	{
		get { return m_csItemRewardDia; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRetrievalReward(WPDRetrievalReward retrievalReward)
	{
		m_nRetrievalId = retrievalReward.retrievalId;
		m_nLevel = retrievalReward.level;
		m_csExpRewardGold = CsGameData.Instance.GetExpReward(retrievalReward.goldExpRewardId);
		m_csItemRewardGold = CsGameData.Instance.GetItemReward(retrievalReward.goldItemRewardId);
		m_csExpRewardDia = CsGameData.Instance.GetExpReward(retrievalReward.diaExpRewardId);
		m_csItemRewardDia = CsGameData.Instance.GetItemReward(retrievalReward.diaItemRewardId);
	}
}
