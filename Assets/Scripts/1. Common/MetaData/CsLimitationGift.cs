using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsLimitationGift
{
	string m_strName;
	string m_strScheduleText;
	int m_nRequiredHeroLevel;

	List<CsLimitationGiftRewardDayOfWeek> m_listCsLimitationGiftRewardDayOfWeek;
	List<CsLimitationGiftRewardSchedule> m_listCsLimitationGiftRewardSchedule;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string ScheduleText
	{
		get { return m_strScheduleText; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public List<CsLimitationGiftRewardDayOfWeek> LimitationGiftRewardDayOfWeekList
	{
		get { return m_listCsLimitationGiftRewardDayOfWeek; }
	}

	public List<CsLimitationGiftRewardSchedule> LimitationGiftRewardScheduleList
	{
		get { return m_listCsLimitationGiftRewardSchedule; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsLimitationGift(WPDLimitationGift limitationGift)
	{
		m_strName = CsConfiguration.Instance.GetString(limitationGift.nameKey);
		m_strScheduleText = CsConfiguration.Instance.GetString(limitationGift.scheduleTextKey);
		m_nRequiredHeroLevel = limitationGift.requiredHeroLevel;

		m_listCsLimitationGiftRewardDayOfWeek = new List<CsLimitationGiftRewardDayOfWeek>();
		m_listCsLimitationGiftRewardSchedule = new List<CsLimitationGiftRewardSchedule>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsLimitationGiftRewardSchedule GetLimitationGiftRewardSchedule(int nScheduleId)
	{
		for (int i = 0; i < m_listCsLimitationGiftRewardSchedule.Count; i++)
		{
			if (m_listCsLimitationGiftRewardSchedule[i].ScheduleId == nScheduleId)
				return m_listCsLimitationGiftRewardSchedule[i];
		}

		return null;
	}
}
