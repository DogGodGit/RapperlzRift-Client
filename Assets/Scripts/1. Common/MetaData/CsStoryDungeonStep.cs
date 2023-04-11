using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeonStep
{
	int m_nDungeonNo;
	int m_nDifficulty;
	int m_nStep;
	int m_nType;
	string m_strTargetTitle;
	string m_strTargetContent;
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;
	int m_nRemoveObstacleId;
	bool m_bIsCompletionRemoveTaming;

	List<CsStoryDungeonGuide> m_listCsStoryDungeonGuide;

	//---------------------------------------------------------------------------------------------------
	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
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
		get { return m_vtTargetPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public int RemoveObstacleId
	{
		get { return m_nRemoveObstacleId; }
	}

	public bool IsCompletionRemoveTaming
	{
		get { return m_bIsCompletionRemoveTaming; }
	}

	public List<CsStoryDungeonGuide> StoryDungeonGuideList
	{
		get { return m_listCsStoryDungeonGuide; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonStep(WPDStoryDungeonStep storyDungeonStep)
	{
		m_nDungeonNo = storyDungeonStep.dungeonNo;
		m_nDifficulty = storyDungeonStep.difficulty;
		m_nStep = storyDungeonStep.step;
		m_nType = storyDungeonStep.type;
		m_strTargetTitle = CsConfiguration.Instance.GetString(storyDungeonStep.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(storyDungeonStep.targetContentKey);
		m_vtTargetPosition = new Vector3((float)storyDungeonStep.targetXPosition, (float)storyDungeonStep.targetYPosition, (float)storyDungeonStep.targetZPosition);
		m_flTargetRadius = storyDungeonStep.targetRadius;
		m_nRemoveObstacleId = storyDungeonStep.removeObstacleId;
		m_bIsCompletionRemoveTaming = storyDungeonStep.isCompletionRemoveTaming;

		m_listCsStoryDungeonGuide = new List<CsStoryDungeonGuide>();
	}
}
