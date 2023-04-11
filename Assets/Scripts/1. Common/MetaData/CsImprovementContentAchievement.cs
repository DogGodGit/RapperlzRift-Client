using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementContentAchievement
{
	int m_nAchievement;
	int m_nAchievementRate;
	string m_strName;
	string m_strColorCode;

	//---------------------------------------------------------------------------------------------------
	public int Achievement
	{
		get { return m_nAchievement; }
	}

	public int AchievementRate
	{
		get { return m_nAchievementRate; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ColorCode
	{
		get { return m_strColorCode; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContentAchievement(WPDImprovementContentAchievement improvementContentAchievement)
	{
		m_nAchievement = improvementContentAchievement.achievement;
		m_nAchievementRate = improvementContentAchievement.achievementRate;
		m_strName = CsConfiguration.Instance.GetString(improvementContentAchievement.nameKey);
		m_strColorCode = improvementContentAchievement.colorCode;
	}
}
