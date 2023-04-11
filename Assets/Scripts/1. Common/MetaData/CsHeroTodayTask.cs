using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsHeroTodayTask
{
	CsTodayTask m_csTodayTask;
	DateTime m_dtDate;
	int m_nProgressCount;

	//---------------------------------------------------------------------------------------------------
	public CsTodayTask TodayTask
	{
		get { return m_csTodayTask; }
	}

	public DateTime TodayTaskDate
	{
		get { return m_dtDate; }
		set { m_dtDate = value; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTodayTask(PDHeroTodayTask heroTodayTask, DateTime dtDate)
	{
		m_csTodayTask = CsGameData.Instance.GetTodayTask(heroTodayTask.taskId);
		m_nProgressCount = heroTodayTask.progressCount;
		m_dtDate = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTodayTask(int nTaskId, int nProgressCount, DateTime dtDate)
	{
		m_csTodayTask = CsGameData.Instance.GetTodayTask(nTaskId);
		m_nProgressCount = nProgressCount;
		m_dtDate = dtDate;
	}
}
