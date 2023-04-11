using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-01)
//---------------------------------------------------------------------------------------------------

public class CsThreatOfFarmQuest
{
	int m_nRequiredHeroLevel;				// 필요영웅레벨
	string m_strTitle;						// 제목	 
	string m_strTargetMovingText;			// 목표이동텍스트
	string m_strTargetMovingDescription;    // 목표이동설명키
	string m_strTargetKillText;             // 목표처치텍스트
	string m_strTargetKillDescription;      // 목표처치설명키
	int m_nLimitCount;						// 일일제한횟수
	CsNpcInfo m_csNpcQuest;					// 퀘스트NPC	
	int m_nMonsterKillLimitTime;            // 몬스터처치제한시간
	string m_strStartDialogue;              // 시작대화키
	string m_strCompletionDialogue;         // 완료대화키
	string m_strCompletionText;

	List<CsThreatOfFarmQuestMission> m_listCsThreatOfFarmQuestMission;
	List<CsThreatOfFarmQuestReward> m_listCsThreatOfFarmQuestReward;

	//---------------------------------------------------------------------------------------------------
	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string TargetMovingText
	{
		get { return m_strTargetMovingText; }
	}

	public string TargetMovingDescription
	{
		get { return m_strTargetMovingDescription; }
	}

	public string TargetKillText
	{
		get { return m_strTargetKillText; }
	}

	public string TargetKillDescription
	{
		get { return m_strTargetKillDescription; }
	}

	public int LimitCount
	{
		get { return m_nLimitCount; }
	}

	public CsNpcInfo QuestNpc
	{
		get { return m_csNpcQuest; }
	}

	public int MonsterKillLimitTime
	{
		get { return m_nMonsterKillLimitTime; }
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

	public List<CsThreatOfFarmQuestMission> ThreatOfFarmQuestMissionList
	{
		get { return m_listCsThreatOfFarmQuestMission; }
	}

	public List<CsThreatOfFarmQuestReward> ThreatOfFarmQuestRewardList
	{
		get { return m_listCsThreatOfFarmQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuest(WPDTreatOfFarmQuest threatOfFarmQuest)
	{
		m_nRequiredHeroLevel = threatOfFarmQuest.requiredHeroLevel;
		m_strTitle = CsConfiguration.Instance.GetString(threatOfFarmQuest.titleKey);
		m_strTargetMovingText = CsConfiguration.Instance.GetString(threatOfFarmQuest.targetMovingTextKey);
		m_strTargetMovingDescription = CsConfiguration.Instance.GetString(threatOfFarmQuest.targetMovingDescriptionKey);
		m_strTargetKillText = CsConfiguration.Instance.GetString(threatOfFarmQuest.targetKillTextKey);
		m_strTargetKillDescription = CsConfiguration.Instance.GetString(threatOfFarmQuest.targetKillDescriptionKey);
		m_nLimitCount = threatOfFarmQuest.limitCount;
		m_csNpcQuest = CsGameData.Instance.GetNpcInfo(threatOfFarmQuest.questNpcId);
		m_nMonsterKillLimitTime = threatOfFarmQuest.monsterKillLimitTime;
		m_strStartDialogue = CsConfiguration.Instance.GetString(threatOfFarmQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(threatOfFarmQuest.completionDialogueKey);
		m_strCompletionText = CsConfiguration.Instance.GetString(threatOfFarmQuest.completionTextKey);

		m_listCsThreatOfFarmQuestMission = new List<CsThreatOfFarmQuestMission>();
		m_listCsThreatOfFarmQuestReward = new List<CsThreatOfFarmQuestReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuestMission GetThreatOfFarmQuestMission(int nMissionId)
	{
		for (int i = 0; i < m_listCsThreatOfFarmQuestMission.Count; i++)
		{
			SimpleDebugLog.dd.d(i, m_listCsThreatOfFarmQuestMission[i].MissionId, m_listCsThreatOfFarmQuestMission[i].TargetPosition);
		}

		for (int i = 0; i < m_listCsThreatOfFarmQuestMission.Count; i++)
		{
			if (m_listCsThreatOfFarmQuestMission[i].MissionId == nMissionId)
				return m_listCsThreatOfFarmQuestMission[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuestReward GetThreatOfFarmQuestReward(int nLevel)
	{
		for (int i = 0; i < m_listCsThreatOfFarmQuestReward.Count; i++)
		{
			if (m_listCsThreatOfFarmQuestReward[i].Level == nLevel)
				return m_listCsThreatOfFarmQuestReward[i];
		}

		return null;
	}


}
