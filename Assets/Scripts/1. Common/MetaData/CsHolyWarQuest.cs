using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-14)
//---------------------------------------------------------------------------------------------------

public class CsHolyWarQuest
{
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strDescription;
	int m_nRequiredHeroLevel;
	CsNpcInfo m_csNpcInfoQuest;
	int m_nLimitTime;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	string m_strCompletionText;

	List<CsHolyWarQuestSchedule> m_listCsHolyWarQuestSchedule;
	List<CsHolyWarQuestGloryLevel> m_listCsHolyWarQuestGloryLevel;
	List<CsHolyWarQuestReward> m_listCsHolyWarQuestReward;

	//---------------------------------------------------------------------------------------------------
	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public CsNpcInfo QuestNpcInfo
	{
		get { return m_csNpcInfoQuest; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public string CompletionText
	{
		get { return m_strCompletionText; }
	}

	public List<CsHolyWarQuestSchedule> HolyWarQuestScheduleList
	{
		get { return m_listCsHolyWarQuestSchedule; }
	}

	public List<CsHolyWarQuestGloryLevel> HolyWarQuestGloryLevelList
	{
		get { return m_listCsHolyWarQuestGloryLevel; }
	}

	public List<CsHolyWarQuestReward> HolyWarQuestRewardList
	{
		get { return m_listCsHolyWarQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHolyWarQuest(WPDHolyWarQuest holyWarQuest)
	{
		m_strTargetTitle = CsConfiguration.Instance.GetString(holyWarQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(holyWarQuest.targetContentKey);
		m_strDescription = CsConfiguration.Instance.GetString(holyWarQuest.descriptionKey);
		m_nRequiredHeroLevel = holyWarQuest.requiredHeroLevel;
		m_csNpcInfoQuest = CsGameData.Instance.GetNpcInfo(holyWarQuest.questNpcId);
		m_nLimitTime = holyWarQuest.limitTime;
		m_strStartDialogue = CsConfiguration.Instance.GetString(holyWarQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(holyWarQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(holyWarQuest.completionTextKey);

		m_listCsHolyWarQuestSchedule = new List<CsHolyWarQuestSchedule>();
		m_listCsHolyWarQuestGloryLevel = new List<CsHolyWarQuestGloryLevel>();
		m_listCsHolyWarQuestReward = new List<CsHolyWarQuestReward>();
	}
}
