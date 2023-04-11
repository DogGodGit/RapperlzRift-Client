using System.Collections.Generic;
using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiographyQuest
{
	int m_nBiographyId;
	int m_nQuestNo;
	int m_nType;
	/*
		1 : 이동
		2 : 몬스터처치
		3 : 상호작용
		4 : NPC대화
		5 : 전기퀘스트던전클리어
	*/
	CsNpcInfo m_csNpcStart;
	string m_strStartDialogue;
	string m_strTargetTitle;
	string m_strTargetContent;
	CsNpcInfo m_csNpcCompletion;
	string m_strCompletionDialogue;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	CsMonsterInfo m_csMonsterTarget;
	CsNpcInfo m_csNpcTarget;
	CsContinentObject m_csContinentObjectTarget;
	int m_nTargetDungeonId;
	int m_nTargetCount;
	CsExpReward m_csExpReward;

	List<CsBiographyQuestStartDialogue> m_listCsBiographyQuestStartDialogue;

	//---------------------------------------------------------------------------------------------------
	public int BiographyId
	{
		get { return m_nBiographyId; }
	}

	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcStart; }
	}

	public string StartDialogue
	{
		get { return m_strStartDialogue; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcCompletion; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
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

	public CsNpcInfo TargetNpc
	{
		get { return m_csNpcTarget; }
	}

	public CsContinentObject TargetContinentObject
	{
		get { return m_csContinentObjectTarget; }
	}

	public int TargetDungeonId
	{
		get { return m_nTargetDungeonId; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public List<CsBiographyQuestStartDialogue> BiographyQuestStartDialogueList
	{
		get { return m_listCsBiographyQuestStartDialogue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuest(WPDBiographyQuest biographyQuest)
	{
		m_nBiographyId = biographyQuest.biographyId;
		m_nQuestNo = biographyQuest.questNo;
		m_nType = biographyQuest.type;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(biographyQuest.startNpcId);
		m_strStartDialogue = CsConfiguration.Instance.GetString(biographyQuest.startDialogueKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(biographyQuest.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(biographyQuest.targetContentKey);
		m_csNpcCompletion = CsGameData.Instance.GetNpcInfo(biographyQuest.completionNpcId);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(biographyQuest.completionDialogueKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(biographyQuest.targetContinentId);
		m_vtTargetPosition = new Vector3(biographyQuest.targetXPosition, biographyQuest.targetYPosition, biographyQuest.targetZPosition);
		m_flTargetRadius = biographyQuest.targetRadius;
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(biographyQuest.targetMonsterId);
		m_csNpcTarget = CsGameData.Instance.GetNpcInfo(biographyQuest.targetNpcId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(biographyQuest.targetContinentObjectId);
		m_nTargetDungeonId = biographyQuest.targetDungeonId;
		m_nTargetCount = biographyQuest.targetCount;
		m_csExpReward = CsGameData.Instance.GetExpReward(biographyQuest.expRewardId);

		m_listCsBiographyQuestStartDialogue = new List<CsBiographyQuestStartDialogue>();
	}
}
