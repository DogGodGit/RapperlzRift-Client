using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-31)
//---------------------------------------------------------------------------------------------------

public class CsWeekendReward
{
	string m_strName;
	string m_strScheduleText;
	string m_strDescription;
	int m_nRequiredHeroLevel;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string ScheduleText
	{
		get { return m_strScheduleText; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWeekendReward(WPDWeekendReward weekendReward)
	{
		m_strName = CsConfiguration.Instance.GetString(weekendReward.nameKey);
		m_strScheduleText = CsConfiguration.Instance.GetString(weekendReward.scheduleTextKey);
		m_strDescription = CsConfiguration.Instance.GetString(weekendReward.descriptionKey);
		m_nRequiredHeroLevel = weekendReward.requiredHeroLevel;
	}
}
