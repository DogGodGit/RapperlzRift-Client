using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsCreatureCardLuckyShop
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
	public CsCreatureCardLuckyShop(WPDCreatureCardLuckyShop creatureCardLuckyShop)
	{
		m_strName = CsConfiguration.Instance.GetString(creatureCardLuckyShop.nameKey);
		m_nFreePickCount = creatureCardLuckyShop.freePickCount;
		m_nFreePickWaitingTime = creatureCardLuckyShop.freePickWaitingTime;
		m_nPick1TimeDia = creatureCardLuckyShop.pick1TimeDia;
		m_nPick5TimeDia = creatureCardLuckyShop.pick5TimeDia;
		m_csGoldRewardPick1Time = CsGameData.Instance.GetGoldReward(creatureCardLuckyShop.pick1TimeGoldRewardId);
		m_csGoldRewardPick5Time = CsGameData.Instance.GetGoldReward(creatureCardLuckyShop.pick5TimeGoldRewardId);
	}
}
