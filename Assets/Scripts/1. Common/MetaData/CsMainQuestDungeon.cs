using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-04)
//---------------------------------------------------------------------------------------------------

public class CsMainQuestDungeon
{
	int m_nDungeonId;
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	float m_flStartYRotation;
	int m_nSafeRevivalWaitingTime;
	int m_nGuideDisplayInterval;
	bool m_bCompletionExitPositionEnabled;
	float m_flCompletionExitXPosition;
	float m_flCcompletionExitYPosition;
	float m_flCcompletionExitZPosition;
	float m_flCcompletionExitYRotation;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsMainQuestDungeonObstacle> m_listCsMainQuestDungeonObstacle;
	List<CsMainQuestDungeonStep> m_listCsMainQuestDungeonStep;
	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public float StartXPosition
	{
		get { return m_flStartXPosition; }
	}

	public float StartYPosition
	{
		get { return m_flStartYPosition; }
	}

	public float StartZPosition
	{
		get { return m_flStartZPosition; }
	}

	public float StartRadius
	{
		get { return m_flStartRadius; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public int GuideDisplayInterval
	{
		get { return m_nGuideDisplayInterval; }
	}

	public bool CompletionExitPositionEnabled
	{
		get { return m_bCompletionExitPositionEnabled; }
	}

	public float CompletionExitXPosition
	{
		get { return m_flCompletionExitXPosition; }
	}

	public float CcompletionExitYPosition
	{
		get { return m_flCcompletionExitYPosition; }
	}

	public float CcompletionExitZPosition
	{
		get { return m_flCcompletionExitZPosition; }
	}

	public float CcompletionExitYRotation
	{
		get { return m_flCcompletionExitYRotation; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public List<CsMainQuestDungeonObstacle> MainQuestDungeonObstacleList
	{
		get { return m_listCsMainQuestDungeonObstacle; }
	}

	public List<CsMainQuestDungeonStep> MainQuestDungeonStepList
	{
		get { return m_listCsMainQuestDungeonStep; }
	}

	public float X
	{
		get { return m_flX; }
	}

	public float Z
	{
		get { return m_flZ; }
	}

	public float XSize
	{
		get { return m_flXSize; }
	}

	public float ZSize
	{
		get { return m_flZSize; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeon(WPDMainQuestDungeon mainQuestDungeon)
	{
		m_nDungeonId = mainQuestDungeon.dungeonId;
		m_strName = CsConfiguration.Instance.GetString(mainQuestDungeon.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(mainQuestDungeon.descriptionKey);
		m_strSceneName = mainQuestDungeon.sceneName;
		m_nStartDelayTime = mainQuestDungeon.startDelayTime;
		m_nLimitTime = mainQuestDungeon.limitTime;
		m_nExitDelayTime = mainQuestDungeon.exitDelayTime;
		m_flStartXPosition = mainQuestDungeon.startXPosition;
		m_flStartYPosition = mainQuestDungeon.startYPosition;
		m_flStartZPosition = mainQuestDungeon.startZPosition;
		m_flStartRadius = mainQuestDungeon.startRadius;
		m_flStartYRotation = mainQuestDungeon.startYRotation;
		m_nSafeRevivalWaitingTime = mainQuestDungeon.safeRevivalWaitingTime;
		m_nGuideDisplayInterval = mainQuestDungeon.guideDisplayInterval;
		m_bCompletionExitPositionEnabled = mainQuestDungeon.completionExitPositionEnabled;
		m_flCompletionExitXPosition = mainQuestDungeon.completionExitXPosition;
		m_flCcompletionExitYPosition = mainQuestDungeon.completionExitYPosition;
		m_flCcompletionExitZPosition = mainQuestDungeon.completionExitZPosition;
		m_flCcompletionExitYRotation = mainQuestDungeon.completionExitYRotation;
		m_nLocationId = mainQuestDungeon.locationId;
		m_flX = mainQuestDungeon.x;
		m_flZ = mainQuestDungeon.z;
		m_flXSize = mainQuestDungeon.xSize;
		m_flZSize = mainQuestDungeon.zSize;

		m_listCsMainQuestDungeonObstacle = new List<CsMainQuestDungeonObstacle>();
		m_listCsMainQuestDungeonStep = new List<CsMainQuestDungeonStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestDungeonStep GetMainQuestDungeonStep(int nStep)
	{
		for (int i = 0; i < m_listCsMainQuestDungeonStep.Count; i++)
		{
			if (m_listCsMainQuestDungeonStep[i].Step == nStep)
				return m_listCsMainQuestDungeonStep[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
}
