using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public class CsMainQuestDungeonStep
{
	int m_nDungeonId;                       // 던전ID
	int m_nStep;                            // 단계
	int m_nType;                            // 타입 1:이동, 2:몬스터처치, 3연출
	string m_strTargetTitle;                // 목표제목
	string m_strTargetContent;              // 목표내용
	Vector3 m_flTargetPosition;             // 목표좌표
	float m_flTargetRadius;                 // 목표반지름
	int m_nTargetMonsterArrangeNo;          // 목표몬스터배치번호	0 : 모든몬스터처치시 클리어,  > 0 : 해당 몬스처배치 처치시 클리어
	int m_nDirectingDuration;               // 연출시간
	float m_flDirectingStartYRotation;      // 연출시작영웅방향
	int m_nRemoveObstacleId;                // 제거장애물ID
	CsExpReward m_csExpReward;              // 경험치보상
	CsGoldReward m_csGoldReward;            // 골드보상

	List<CsMainQuestDungeonGuide> m_listCsMainQuestDungeonGuide;

	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public Vector3 TargetPosition
	{
		get { return m_flTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int TargetMonsterArrangeNo
	{
		get { return m_nTargetMonsterArrangeNo; }
	}

	public int DirectingDuration
	{
		get { return m_nDirectingDuration; }
	}

	public float DirectingStartYRotation
	{
		get { return m_flDirectingStartYRotation; }
	}

	public int RemoveObstacleId
	{
		get { return m_nRemoveObstacleId; }
	}

	public long ExpReward
	{
		get { return m_csExpReward.Value; }
	}

	public long GoldReward
	{
		get { return m_csGoldReward.Value; }
	}

	public List<CsMainQuestDungeonGuide> MainQuestDungeonGuideList
	{
		get { return m_listCsMainQuestDungeonGuide; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeonStep(WPDMainQuestDungeonStep mainQuestDungeonStep)
	{
		m_nDungeonId = mainQuestDungeonStep.dungeonId;
		m_nStep = mainQuestDungeonStep.step;
		m_nType = mainQuestDungeonStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(mainQuestDungeonStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(mainQuestDungeonStep.targetContentKey);
		m_flTargetPosition = new Vector3(mainQuestDungeonStep.targetXPosition, mainQuestDungeonStep.targetYPosition, mainQuestDungeonStep.targetZPosition);
		m_flTargetRadius = mainQuestDungeonStep.targetRadius ;
		m_nTargetMonsterArrangeNo = mainQuestDungeonStep.targetMonsterArrangeNo;
		m_nDirectingDuration = mainQuestDungeonStep.directingDuration;
		m_flDirectingStartYRotation = mainQuestDungeonStep.directingStartYRotation;
		m_nRemoveObstacleId = mainQuestDungeonStep.removeObstacleId;
		m_csExpReward = CsGameData.Instance.GetExpReward(mainQuestDungeonStep.expRewardId);
		m_csGoldReward = CsGameData.Instance.GetGoldReward(mainQuestDungeonStep.goldRewardId);

		m_listCsMainQuestDungeonGuide = new List<CsMainQuestDungeonGuide>();
	}
}
