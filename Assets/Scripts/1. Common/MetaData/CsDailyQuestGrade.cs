using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsDailyQuestGrade
{
	int m_nGrade;
	string m_strColorCode;
	int m_nImmediateCompletionRequiredGold;
	int m_nAutoCompletionRequiredTime;
	int m_nRewardVipPoint;
	CsItemReward m_csItemReward;
	CsItemReward m_csItemRewardAvailable1;
	CsItemReward m_csItemRewardAvailable2;

	//---------------------------------------------------------------------------------------------------
	public int Grade
	{
		get { return m_nGrade; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	public int ImmediateCompletionRequiredGold
	{
		get { return m_nImmediateCompletionRequiredGold; }
	}

	public int AutoCompletionRequiredTime
	{
		get { return m_nAutoCompletionRequiredTime; }
	}

	public int RewardVipPoint
	{
		get { return m_nRewardVipPoint; }
	}

	public CsItemReward ItemReward
	{
		get { return m_csItemReward; }
	}

	public CsItemReward AvailableItemReward1
	{
		get { return m_csItemRewardAvailable1; }
	}

	public CsItemReward AvailableItemReward2
	{
		get { return m_csItemRewardAvailable2; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDailyQuestGrade(WPDDailyQuestGrade dailyQuestGrade)
	{
		m_nGrade = dailyQuestGrade.grade;
		m_strColorCode = dailyQuestGrade.colorCode;
		m_nImmediateCompletionRequiredGold = dailyQuestGrade.immediateCompletionRequiredGold;
		m_nAutoCompletionRequiredTime = dailyQuestGrade.autoCompletionRequiredTime;
		m_nRewardVipPoint = dailyQuestGrade.rewardVipPoint;
		m_csItemReward = CsGameData.Instance.GetItemReward(dailyQuestGrade.itemRewardId);
		m_csItemRewardAvailable1 = CsGameData.Instance.GetItemReward(dailyQuestGrade.availableItemRewardId1);
		m_csItemRewardAvailable2 = CsGameData.Instance.GetItemReward(dailyQuestGrade.availableItemRewardId2);
	}

}
