using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsHeroWeekendReward
{
	DateTime m_dtWeekStartDate;
	int m_nSelection1;
	int m_nSelection2;
	int m_nSelection3;
	bool m_bRewarded;

	//---------------------------------------------------------------------------------------------------
	public DateTime WeekStartDate
	{
		get { return m_dtWeekStartDate; }
	}

	public int Selection1
	{
		get { return m_nSelection1; }
		set { m_nSelection1 = value; }
	}

	public int Selection2
	{
		get { return m_nSelection2; }
		set { m_nSelection2 = value; }
	}

	public int Selection3
	{
		get { return m_nSelection3; }
		set { m_nSelection3 = value; }
	}

	public bool Rewarded
	{
		get { return m_bRewarded; }
		set { m_bRewarded = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroWeekendReward(PDHeroWeekendReward heroWeekendReward)
	{
		m_dtWeekStartDate = heroWeekendReward.weekStartDate;
		m_nSelection1 = heroWeekendReward.selection1;
		m_nSelection2 = heroWeekendReward.selection2;
		m_nSelection3 = heroWeekendReward.selection3;
		m_bRewarded = heroWeekendReward.rewarded;
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset(DateTime dtWeekStartDate)
	{
		m_dtWeekStartDate = dtWeekStartDate;
		m_nSelection1 = -1;
		m_nSelection2 = -1;
		m_nSelection3 = -1;
		m_bRewarded = false;
	}
}
