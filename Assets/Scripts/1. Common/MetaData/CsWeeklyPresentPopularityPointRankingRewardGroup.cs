using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-05)
//---------------------------------------------------------------------------------------------------

public class CsWeeklyPresentPopularityPointRankingRewardGroup
{
	int m_nGroupNo;
	int m_nHighRanking;
	int m_nLowRanking;

	List<CsWeeklyPresentPopularityPointRankingReward> m_listCsWeeklyPresentPopularityPointRankingReward;

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

	public List<CsWeeklyPresentPopularityPointRankingReward> WeeklyPresentPopularityPointRankingRewardList
	{
		get { return m_listCsWeeklyPresentPopularityPointRankingReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeeklyPresentPopularityPointRankingRewardGroup(WPDWeeklyPresentPopularityPointRankingRewardGroup weeklyPresentPopularityPointRankingRewardGroup)
	{
		m_nGroupNo = weeklyPresentPopularityPointRankingRewardGroup.groupNo;
		m_nHighRanking = weeklyPresentPopularityPointRankingRewardGroup.highRanking;
		m_nLowRanking = weeklyPresentPopularityPointRankingRewardGroup.lowRanking;

		m_listCsWeeklyPresentPopularityPointRankingReward = new List<CsWeeklyPresentPopularityPointRankingReward>();
	}
}
