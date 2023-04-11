using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-16)
//---------------------------------------------------------------------------------------------------

public class CsOpen7DayEventDay
{
	int m_nDay;

	List<CsOpen7DayEventMission> m_listCsOpen7DayEventMission;
	List<CsOpen7DayEventProduct> m_listCsOpen7DayEventProduct;

	//---------------------------------------------------------------------------------------------------
	public int Day
	{
		get { return m_nDay; }
	}

	public List<CsOpen7DayEventMission> Open7DayEventMissionList
	{
		get { return m_listCsOpen7DayEventMission; }
	}

	public List<CsOpen7DayEventProduct> Open7DayEventProductList
	{
		get { return m_listCsOpen7DayEventProduct; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventDay(WPDOpen7DayEventDay open7DayEventDay)
	{
		m_nDay = open7DayEventDay.day;

		m_listCsOpen7DayEventMission = new List<CsOpen7DayEventMission>();
		m_listCsOpen7DayEventProduct = new List<CsOpen7DayEventProduct>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventMission GetOpen7DayEventMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsOpen7DayEventMission.Count; i++)
		{
			if (m_listCsOpen7DayEventMission[i].MissionId == nMissionId)
				return m_listCsOpen7DayEventMission[i];
		}

		return null;
	}
}
