using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-13)
//---------------------------------------------------------------------------------------------------

public class CsSupplySupportQuestReward
{
	int m_nCartId;
	int m_nLevel;
	CsExpReward m_csExpReward;
	CsGoldReward m_csGoldReward;
	CsExploitPointReward m_csExploitPointReward;

	//---------------------------------------------------------------------------------------------------
	public int CartId
	{
		get { return m_nCartId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public CsExploitPointReward ExploitPointReward
	{
		get { return m_csExploitPointReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSupplySupportQuestReward(WPDSupplySupportQuestReward supplySupportQuestReward)
	{
		m_nCartId = supplySupportQuestReward.cartId;
		m_nLevel = supplySupportQuestReward.level;
		m_csExpReward = CsGameData.Instance.GetExpReward(supplySupportQuestReward.expRewardId);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(supplySupportQuestReward.goldRewardId);
		m_csExploitPointReward = CsGameData.Instance.GetExploitPointReward(supplySupportQuestReward.exploitPointRewardId);
	}

}
