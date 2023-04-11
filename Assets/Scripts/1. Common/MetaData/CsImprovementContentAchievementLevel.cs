using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementContentAchievementLevel
{
	int m_nContentId;
	int m_nLevel;
	int m_nAchievementValue;

	//---------------------------------------------------------------------------------------------------
	public int ContentId
	{
		get { return m_nContentId; }
	}

	public int Level
	{
		get { return m_nLevel; }
	}

	public int AchievementValue
	{
		get { return m_nAchievementValue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContentAchievementLevel(WPDImprovementContentAchievementLevel improvementContentAchievementLevel)
	{
		m_nContentId = improvementContentAchievementLevel.contentId;
		m_nLevel = improvementContentAchievementLevel.level;
		m_nAchievementValue = improvementContentAchievementLevel.achievementValue;
	}

}
