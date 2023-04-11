using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsItemLuckyShop
{
	string m_strName;
	int m_nFreePickCount;
	int m_nFreePickWaitingTime;
	int m_nPick1TimeDia;
	int m_nPick5TimeDia;
	CsGoldReward m_csGoldRewardPick1Time;
	CsGoldReward m_csGoldRewardPick5Time;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public int FreePickCount
	{
		get { return m_nFreePickCount; }
	}

	public int FreePickWaitingTime
	{
		get { return m_nFreePickWaitingTime; }
	}

	public int Pick1TimeDia
	{
		get { return m_nPick1TimeDia; }
	}

	public int Pick5TimeDia
	{
		get { return m_nPick5TimeDia; }
	}

	public CsGoldReward Pick1TimeGoldReward
	{
		get { return m_csGoldRewardPick1Time; }
	}

	public CsGoldReward Pick5TimeGoldReward
	{
		get { return m_csGoldRewardPick5Time; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsItemLuckyShop(WPDItemLuckyShop itemLuckyShop)
	{
		m_strName = CsConfiguration.Instance.GetString(itemLuckyShop.nameKey);
		m_nFreePickCount = itemLuckyShop.freePickCount;
		m_nFreePickWaitingTime = itemLuckyShop.freePickWaitingTime;
		m_nPick1TimeDia = itemLuckyShop.pick1TimeDia;
		m_nPick5TimeDia = itemLuckyShop.pick5TimeDia;
		m_csGoldRewardPick1Time = CsGameData.Instance.GetGoldReward(itemLuckyShop.pick1TimeGoldRewardId);
		m_csGoldRewardPick5Time = CsGameData.Instance.GetGoldReward(itemLuckyShop.pick5TimeGoldRewardId);
	}
}
