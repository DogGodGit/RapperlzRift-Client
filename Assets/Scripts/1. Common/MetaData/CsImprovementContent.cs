using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsImprovementContent
{
	int m_nContentId;
	string m_strName;
	string m_strDescription;
	bool m_bIsAchievementDisplay;
	int m_nRequiredConditionType;
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;

	List<CsImprovementContentAchievementLevel> m_listCsImprovementContentAchievementLevel;

	//---------------------------------------------------------------------------------------------------
	public int ContentId
	{
		get { return m_nContentId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public bool IsAchievementDisplay
	{
		get { return m_bIsAchievementDisplay; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public List<CsImprovementContentAchievementLevel> ImprovementContentAchievementLevelList
	{
		get { return m_listCsImprovementContentAchievementLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContent(WPDImprovementContent improvementContent)
	{
		m_nContentId = improvementContent.contentId;
		m_strName = CsConfiguration.Instance.GetString(improvementContent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(improvementContent.descriptionKey);
		m_bIsAchievementDisplay = improvementContent.isAchievementDisplay;
		m_nRequiredConditionType = improvementContent.requiredConditionType;
		m_nRequiredHeroLevel = improvementContent.requiredHeroLevel;
		m_nRequiredMainQuestNo = improvementContent.requiredMainQuestNo;

		m_listCsImprovementContentAchievementLevel = new List<CsImprovementContentAchievementLevel>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsImprovementContentAchievementLevel GetImprovementContentAchievementLevel(int nLevel)
	{
		for (int i = 0; i < m_listCsImprovementContentAchievementLevel.Count; i++)
		{
			if (m_listCsImprovementContentAchievementLevel[i].Level == nLevel)
				return m_listCsImprovementContentAchievementLevel[i];
		}

		return null;
	}
}
