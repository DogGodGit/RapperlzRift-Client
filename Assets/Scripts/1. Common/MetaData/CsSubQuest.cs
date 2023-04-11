using System.Collections.Generic;
using UnityEngine;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsSubQuest
{
	int m_nQuestId;
	int m_nRequiredConditionType;   // 1:메인퀘스트,2:레벨
	int m_nRequiredConditionValue;
	string m_strTitle;
	int m_nType;
	/*
	1 : 상호작용
	- 목표대륙오브젝트ID
	- 목표수량

	2 : 몬스터처치
	- 목표몬스터ID
	- 목표수량

	3 : 수집
	- 목표몬스터ID
	- 목표획득확률
	- 목표수량

	4 : 컨텐츠플레이
	- 목표컨텐츠ID (컨텐츠 미정)
	*/
	CsNpcInfo m_csNpcStart;
	string m_strStartDialogue;
	string m_strTargetText;
	CsContinent m_csContinentTarget;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	CsContinentObject m_csContinentObjectTarget;
	CsMonsterInfo m_csMonsterTarget;
	int m_nTargetAcquisitionRate;
	int m_nTargetContentId;
	/*
	1 : 스토리 던전 
	2 : 오시리스의 방 
	3 : 수련 동굴 
	4 : 농장의 위협  
	5 : 지하 미로 
	6 : 현상금 사냥 
	7 : 낚시 미끼 
	8 : 밀서 유출 
	9 : 의문의 상자 
	10 : 고대 유물의 방 
	11 : 고대인의 유적
	12 : 차원 습격 
	13 : 검투 대회(결투장) 
	14 : 위대한 성전 
	15 : 영혼을 탐하는 자 
	16 : 용맹의 증명 
	17 : 지혜의 신전 
	18 : 유적 탈환 
	19 : 진정한 영웅 
	20 : 공포의 제단 
	21 : 전쟁의 기억 
	22 : 용의 둥지 
	23 : 크리처 농장 
	24 : 일일 퀘스트 
	25 : 주간 퀘스트 
	26 : 친구추가
	27 : 길드가입
	*/
	int m_nTargetCount;
	CsNpcInfo m_csNpcCompletion;
	string m_strCompletion;
	string m_strCompletionDialogue;
	bool m_bAbandonmentEnabled;
	bool m_bReacceptanceEnabled;
	CsExpReward m_csExpReward;
	CsGoldReward m_csGoldReward;

	List<CsSubQuestReward> m_listCsSubQuestReward;

	//---------------------------------------------------------------------------------------------------
	public int QuestId
	{
		get { return m_nQuestId; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredConditionValue
	{
		get { return m_nRequiredConditionValue; }
	}

	public string Title
	{
		get { return m_strTitle; }
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

	public string TargetText
	{
		get { return m_strTargetText; }
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

	public CsContinentObject TargetContinentObject
	{
		get { return m_csContinentObjectTarget; }
	}

	public CsMonsterInfo TargetMonster
	{
		get { return m_csMonsterTarget; }
	}

	public int TargetAcquisitionRate
	{
		get { return m_nTargetAcquisitionRate; }
	}

	public int TargetContentId
	{
		get { return m_nTargetContentId; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcCompletion; }
	}

	public string Completion
	{
		get { return m_strCompletion; }
	}

	public string CompletionDialogue
	{
		get { return m_strCompletionDialogue; }
	}

	public bool AbandonmentEnabled
	{
		get { return m_bAbandonmentEnabled; }
	}

	public bool ReacceptanceEnabled
	{
		get { return m_bReacceptanceEnabled; }
	}

	public CsExpReward ExpReward
	{
		get { return m_csExpReward; }
	}

	public CsGoldReward GoldReward
	{
		get { return m_csGoldReward; }
	}

	public List<CsSubQuestReward> SubQuestRewardList
	{
		get { return m_listCsSubQuestReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubQuest(WPDSubQuest subQuest)
	{
		m_nQuestId = subQuest.questId;
		m_nRequiredConditionType = subQuest.requiredConditionType;
		m_nRequiredConditionValue = subQuest.requiredConditionValue;
		m_strTitle = CsConfiguration.Instance.GetString(subQuest.titleKey);
		m_nType = subQuest.type;
		m_csNpcStart = CsGameData.Instance.GetNpcInfo(subQuest.startNpcId);
		m_strStartDialogue = CsConfiguration.Instance.GetString(subQuest.startDialogueKey);
		m_strTargetText = CsConfiguration.Instance.GetString(subQuest.targetTextKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(subQuest.targetContinentId);
		m_vtTargetPosition = new Vector3((float)subQuest.targetXPosition, (float)subQuest.targetYPosition, (float)subQuest.targetZPosition);
		m_flTargetRadius = subQuest.targetRadius;
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(subQuest.targetContinentObjectId);
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(subQuest.targetMonsterId);
		m_nTargetAcquisitionRate = subQuest.targetAcquisitionRate;
		m_nTargetContentId = subQuest.targetContentId;
		m_nTargetCount = subQuest.targetCount;
		m_csNpcCompletion = CsGameData.Instance.GetNpcInfo(subQuest.completionNpcId);
		m_strCompletion = CsConfiguration.Instance.GetString(subQuest.completionKey);
		m_strCompletionDialogue = CsConfiguration.Instance.GetString(subQuest.completionDialogueKey);
		m_bAbandonmentEnabled = subQuest.abandonmentEnabled;
		m_bReacceptanceEnabled = subQuest.reacceptanceEnabled;
		m_csExpReward = CsGameData.Instance.GetExpReward(subQuest.expRewardId);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(subQuest.goldRewardId);

		m_listCsSubQuestReward = new List<CsSubQuestReward>();
	}
}
