using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAchievementReward
{
	int m_nRewardNo;
	int m_nRequiredAchievementPoint;

	List<CsAchievementRewardEntry> m_listCsAchievementRewardEntry;

	//---------------------------------------------------------------------------------------------------
	public int RewardNo
	{
		get { return m_nRewardNo; }
	}

	public int RequiredAchievementPoint
	{
		get { return m_nRequiredAchievementPoint; }
	}

	public List<CsAchievementRewardEntry> AchievementRewardEntryList
	{
		get { return m_listCsAchievementRewardEntry; }
	}


	//---------------------------------------------------------------------------------------------------
	public CsAchievementReward(WPDAchievementReward achievementReward)
	{
		m_nRewardNo = achievementReward.rewardNo;
		m_nRequiredAchievementPoint = achievementReward.requiredAchievementPoint;

		m_listCsAchievementRewardEntry = new List<CsAchievementRewardEntry>();
	}
}
