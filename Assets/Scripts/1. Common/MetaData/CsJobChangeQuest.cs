using System.Collections.Generic;
using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------

public class CsJobChangeQuest
{
	int m_nQuestNo;
	string m_strTitle;
	string m_strTargetTitle;
	string m_strTargetContent;
	CsNpcInfo m_csNpcQuest;
	string m_strStartDialogue;
	string m_strCompletionDialogue;
	int m_nType;
	bool m_bIsTargetOwnNation;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	CsMonsterInfo m_csMonsterTarget;
	CsContinentObject m_csContinentObjectTarget;
	int m_nTargetCount;
	int m_nLimitTime;
	CsMonsterArrange m_csMonsterArrangeTarget;
	Vector3 m_vtTargetGuildTerritoryPosition;
	float m_flTargetGuildTerritoryRadius;
	CsMonsterArrange m_csMonsterArrangeTargetGuild;
	CsItemReward m_csItemRewardCompletion;

	List<CsJobChangeQuestDifficulty> m_listCsJobChangeQuestDifficulty;

	//---------------------------------------------------------------------------------------------------
	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsNpcInfo QuestNpc
	{
		get { return m_csNpcQuest; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public bool IsTargetOwnNation
	{
		get { return m_bIsTargetOwnNation; }
	}

	public CsContinent TargetContinent
	{
		get { return m_csContinentTarget; }
	}

	public Vector3 TargetPosition
	{
		get { return m_vtTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public CsMonsterInfo TargetMonster
	{
		get { return m_csMonsterTarget; }
	}

	public CsContinentObject TargetContinentObject
	{
		get { return m_csContinentObjectTarget; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public CsMonsterArrange TargetMonsterArrange
	{
		get { return m_csMonsterArrangeTarget; }
	}

	public Vector3 TargetGuildTerritoryPosition
	{
		get { return m_vtTargetGuildTerritoryPosition; }
	}

	public float TargetGuildTerritoryRadius
	{
		get { return m_flTargetGuildTerritoryRadius; }
	}

	public CsMonsterArrange TargetGuildMonsterArrange
	{
		get { return m_csMonsterArrangeTargetGuild; }
	}

	public CsItemReward CompletionItemReward
	{
		get { return m_csItemRewardCompletion; }
	}

	public List<CsJobChangeQuestDifficulty> JobChangeQuestDifficultyList
	{
		get { return m_listCsJobChangeQuestDifficulty; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsJobChangeQuest(WPDJobChangeQuest jobChangeQuest)
	{
		m_nQuestNo = jobChangeQuest.questNo;
		m_strTitle = CsConfiguration.Instance.GetString(jobChangeQuest.titleKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(jobChangeQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(jobChangeQuest.targetContentKey);
		m_csNpcQuest = CsGameData.Instance.GetNpcInfo(jobChangeQuest.questNpcId);
		m_strStartDialogue = CsConfiguration.Instance.GetString(jobChangeQuest.startDialogueKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(jobChangeQuest.completionDialogueKey);
		m_nType = jobChangeQuest.type;
		m_bIsTargetOwnNation = jobChangeQuest.isTargetOwnNation;
		m_csContinentTarget = CsGameData.Instance.GetContinent(jobChangeQuest.targetContinentId);
		m_vtTargetPosition = new Vector3(jobChangeQuest.targetXPosition, jobChangeQuest.targetYPosition, jobChangeQuest.targetZPosition);
		m_flTargetRadius = jobChangeQuest.targetRadius;
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(jobChangeQuest.targetMonsterId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(jobChangeQuest.targetContinentObjectId);
		m_nTargetCount = jobChangeQuest.targetCount;
		m_nLimitTime = jobChangeQuest.limitTime;
		m_csMonsterArrangeTarget = CsGameData.Instance.GetMonsterArrange(jobChangeQuest.targetMonsterArrangeId);
		m_vtTargetGuildTerritoryPosition = new Vector3(jobChangeQuest.targetGuildTerritoryXPosition, jobChangeQuest.targetGuildTerritoryYPosition, jobChangeQuest.targetGuildTerritoryZPosition);
		m_flTargetGuildTerritoryRadius = jobChangeQuest.targetGuildTerritoryRadius;
		m_csMonsterArrangeTargetGuild = CsGameData.Instance.GetMonsterArrange(jobChangeQuest.targetGuildMonsterArrangeId);
		m_csItemRewardCompletion = CsGameData.Instance.GetItemReward(jobChangeQuest.completionItemRewardId);

		m_listCsJobChangeQuestDifficulty = new List<CsJobChangeQuestDifficulty>();
	}
}
