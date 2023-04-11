using UnityEngine;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-05)
//---------------------------------------------------------------------------------------------------

/*
gala:
* 공통
- 목표대륙ID
- 목표x좌표
- 목표y좌표
- 목표z좌표
- 목표반지름

* 타입
0 : 목표없음

1 : 이동
- 목표수량

2 : 몬스터처치
- 목표몬스터ID
- 목표수량

3 : 수집
- 목표몬스터ID
- 목표획득확률
- 목표수량

4 : 상호작용
- 목표대륙오브젝트ID
- 목표수량

5 : 메인퀘스트던전 클리어
- 목표던전ID

6 : 운송
- 수레ID

7 : 컨텐츠플레이
- 목표컨텐츠ID
*/

public enum EnMainQuestType
{
	None = 0,
	Move = 1,
	Kill = 2,
	Collect = 3,
	Interaction = 4,
	Dungeon = 5,
	Cart = 6,
}

public class CsMainQuest
{
	int m_nMainQuestNo;								// 메인퀘스트번호
	int m_nRequiredHeroLevel;						// 필요영웅레벨
	string m_strTitle;								// 제목
	int m_nType;									// 타입
	CsNpcInfo m_csNpcInfoStart;						// 시작NPC
	string m_strStartText;							// 시작내용
	string m_strTargetText;							// 목표텍스트
	CsContinent m_csContinentTarget;				// 목표대륙
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;							// 목표반지름
	CsNpcInfo m_csNpcInfoTarget;					// 목표NPC
	CsContinentObject m_csContinentObjectTarget;    // 목표대륙오브젝트
	CsMainQuestDungeon m_csMainQuestDungeonTarget;  // 목표던전
	CsMonsterInfo m_csMonsterTarget;				// 목표몬스터
	int m_nTargetAcquisitionRate;                   // 목표획득확률
	int m_nTargetContentId;
	int m_nTargetCount;                             // 목표수량
	int m_nTransformationMonsterId;
	int m_nTransformationLifetime;
	bool m_bTransformationRestored;
	CsNpcInfo m_csNpcInfoCompletion;				// 완료NPC
	string m_strCompletionText;						// 완료내용
	CsExpReward m_csExpReward;						// 보상경험치
	CsGoldReward m_csGoldReward;                    // 보상골드 
	int m_nCardId;                                  // 카트ID

	List<CsMainQuestReward> m_listMainQuestRewardItem;  // 메인퀘스트보상아이템리스트
	List<CsMainQuestStartDialogue> m_listCsMainQuestStartDialogue;
	List<CsMainQuestCompletionDialogue> m_listCsMainQuestCompletionDialogue;

	//---------------------------------------------------------------------------------------------------
	public int MainQuestNo
	{
		get 
		{
			// 메인 퀘스트를 모두 완료했을 경우
			if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Completed)
			{
				return m_nMainQuestNo + 1;
			}
			else
			{
				return m_nMainQuestNo;
			}
		}
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public string Title
	{
		get { return m_strTitle; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public EnMainQuestType MainQuestType
	{
		get { return (EnMainQuestType)m_nType; }
	}

	public CsNpcInfo StartNpc
	{
		get { return m_csNpcInfoStart; }
	}

	public string StartText
	{
		get { return m_strStartText; }
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

	public CsNpcInfo TargetNpc
	{
		get { return m_csNpcInfoTarget; }
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

	public int CardId
	{
		get { return m_nCardId; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public int TransformationMonsterId
	{
		get { return m_nTransformationMonsterId; }
	}

	public int TransformationLifetime
	{
		get { return m_nTransformationLifetime; }
	}

	public bool TransformationRestored
	{
		get { return m_bTransformationRestored; }
	}

	public CsNpcInfo CompletionNpc
	{
		get { return m_csNpcInfoCompletion; }
	}

	public string CompletionText
	{
		get { return m_strCompletionText; }
	}

	public long RewardExp
	{
		get { return m_csExpReward.Value; }
	}

	public long RewardGold
	{
		get { return m_csGoldReward.Value; }
	}

	public int TargetContentId
	{
		get { return m_nTargetContentId; }
	}

	public List<CsMainQuestReward> MainQuestRewardItemList
	{
		get { return m_listMainQuestRewardItem; }
	}

	public CsMainQuestDungeon MainQuestDungeonTarget
	{
		get { return m_csMainQuestDungeonTarget; }
	}

	public List<CsMainQuestStartDialogue> MainQuestStartDialogueList
	{
		get { return m_listCsMainQuestStartDialogue; }
	}

	public List<CsMainQuestCompletionDialogue> MainQuestCompletionDialogueList
	{
		get { return m_listCsMainQuestCompletionDialogue; }
	}

	//---------------------------------------------------------------------------------------------------
	public int GetMainQuestNo()
	{
		return m_nMainQuestNo;
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuest(WPDMainQuest mainQuest)
	{
		m_nMainQuestNo = mainQuest.mainQuestNo;
		m_nRequiredHeroLevel = mainQuest.requiredHeroLevel;
		m_strTitle = CsConfiguration.Instance.GetString(mainQuest.titleKey);
		m_nType = mainQuest.type;
		m_csNpcInfoStart = CsGameData.Instance.GetNpcInfo(mainQuest.startNpcId);
		m_strStartText = CsConfiguration.Instance.GetString(mainQuest.startTextKey);
		m_strTargetText = CsConfiguration.Instance.GetString(mainQuest.targetTextKey);
		m_csContinentTarget = CsGameData.Instance.GetContinent(mainQuest.targetContinentId);
		m_vtTargetPosition = new Vector3(mainQuest.targetXPosition, mainQuest.targetYPosition, mainQuest.targetZPosition);
		m_flTargetRadius = mainQuest.targetRadius;
		m_csNpcInfoTarget = CsGameData.Instance.GetNpcInfo(mainQuest.targetNpcId);
		m_csContinentObjectTarget = CsGameData.Instance.GetContinentObject(mainQuest.targetContinentObjectId);
		m_csMainQuestDungeonTarget = CsGameData.Instance.GetMainQuestDungeon(mainQuest.targetDungeonId);
		m_csMonsterTarget = CsGameData.Instance.GetMonsterInfo(mainQuest.targetMonsterId);
		m_nTargetAcquisitionRate = mainQuest.targetAcquisitionRate;
		m_nTargetContentId = mainQuest.targetContentId;
		m_nTargetCount = mainQuest.targetCount;
		m_nTransformationMonsterId = mainQuest.transformationMonsterId;
		m_nTransformationLifetime = mainQuest.transformationLifetime;
		m_bTransformationRestored = mainQuest.transformationRestored;
		m_csNpcInfoCompletion = CsGameData.Instance.GetNpcInfo(mainQuest.completionNpcId);
		m_strCompletionText = CsConfiguration.Instance.GetString(mainQuest.completionTextKey);
		m_csExpReward = CsGameData.Instance.GetExpReward(mainQuest.expRewardId);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(mainQuest.goldRewardId);
		m_nCardId = mainQuest.cartId;

		m_listMainQuestRewardItem = new List<CsMainQuestReward>();
		m_listCsMainQuestStartDialogue = new List<CsMainQuestStartDialogue>();
		m_listCsMainQuestCompletionDialogue = new List<CsMainQuestCompletionDialogue>();
	}
}
