using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyPresentContributionPointRankingRewardGroup
{
	int m_nGroupNo;
	int m_nHighRanking;
	int m_nLowRanking;

	List<CsWeeklyPresentContributionPointRankingReward> m_listCsWeeklyPresentContributionPointRankingReward;

	//---------------------------------------------------------------------------------------------------
	public int GroupNo
	{
		get { return m_nGroupNo; }
	}

	public int HighRanking
	{
		get { return m_nHighRanking; }
	}

	public int LowRanking
	{
		get { return m_nLowRanking; }
	}

	public List<CsWeeklyPresentContributionPointRankingReward> WeeklyPresentContributionPointRankingRewardList
	{
		get { return m_listCsWeeklyPresentContributionPointRankingReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyPresentContributionPointRankingRewardGroup(WPDWeeklyPresentContributionPointRankingRewardGroup weeklyPresentContributionPointRankingRewardGroup)
	{
		m_nGroupNo = weeklyPresentContributionPointRankingRewardGroup.groupNo;
		m_nHighRanking = weeklyPresentContributionPointRankingRewardGroup.highRanking;
		m_nLowRanking = weeklyPresentContributionPointRankingRewardGroup.lowRanking;

		m_listCsWeeklyPresentContributionPointRankingReward = new List<CsWeeklyPresentContributionPointRankingReward>();
	}
}
