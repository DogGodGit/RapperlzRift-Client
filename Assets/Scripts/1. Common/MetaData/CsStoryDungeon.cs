using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsStoryDungeon
{
	int m_nDungeonNo;
	string m_strName;
	string m_strSubName;
	int m_nEnterCount;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredHeroMinLevel;
	int m_nRequiredHeroMaxLevel;
	int m_nRequiredMainQuestNo;
	int m_nRequiredStamina;
	int m_nLimitTime;
	string m_strSceneName;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	float m_flStartYRotation;
	int m_nStartDelayTime;
	int m_nExitDelayTime;
	float m_flTamingXPosition;
	float m_flTamingYPosition;
	float m_flTamingZPosition;
	float m_flTamingYRotation;
	float m_flClearXPosition;
	float m_flClearYPosition;
	float m_flClearZPosition;
	float m_flClearYRotation;
	int m_nSafeRevivalWaitingTime;
	int m_nGuideDisplayInterval;
	int m_nComboDuration;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsStoryDungeonDifficulty> m_listCsStoryDungeonDifficulty;
	List<CsStoryDungeonObstacle> m_listCsStoryDungeonObstacle;

	DateTime m_dtDatePlay;
	int m_nPlayCount = 0;
	int m_nClearMaxDifficulty = 0;

	//---------------------------------------------------------------------------------------------------
	public DateTime PlayDate
	{
		get { return m_dtDatePlay; }
		set { m_dtDatePlay = value; }
	}

	public int PlayCount
	{
		get { return m_nPlayCount; }
		set { m_nPlayCount = value; }
	}

	public int DungeonNo
	{
		get { return m_nDungeonNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string SubName
	{
		get { return m_strSubName; }
	}

	public int EnterCount
	{
		get { return m_nEnterCount; }
	}

	public int RequiredHeroMinLevel
	{
		get { return m_nRequiredHeroMinLevel; }
	}

	public int RequiredHeroMaxLevel
	{
		get { return m_nRequiredHeroMaxLevel; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
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

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
	}

	public float TamingXPosition
	{
		get { return m_flTamingXPosition; }
	}

	public float TamingYPosition
	{
		get { return m_flTamingYPosition; }
	}

	public float TamingZPosition
	{
		get { return m_flTamingZPosition; }
	}

	public float TamingYRotation
	{
		get { return m_flTamingYRotation; }
	}

	public float ClearXPosition
	{
		get { return m_flClearXPosition; }
	}

	public float ClearYPosition
	{
		get { return m_flClearYPosition; }
	}

	public float ClearZPosition
	{
		get { return m_flClearZPosition; }
	}

	public float ClearYRotation
	{
		get { return m_flClearYRotation; }
	}

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public int GuideDisplayInterval
	{
		get { return m_nGuideDisplayInterval; }
	}

	public int ComboDuration
	{
		get { return m_nComboDuration; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public int ClearMaxDifficulty
	{
		get { return m_nClearMaxDifficulty; }
		set { m_nClearMaxDifficulty = value; }
	}

	public List<CsStoryDungeonDifficulty> StoryDungeonDifficultyList
	{
		get { return m_listCsStoryDungeonDifficulty; }
	}

	public List<CsStoryDungeonObstacle> StoryDungeonObstacleList
	{
		get { return m_listCsStoryDungeonObstacle; }
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

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeon(WPDStoryDungeon storyDungeon)
	{
		m_nDungeonNo = storyDungeon.dungeonNo;
		m_strName = CsConfiguration.Instance.GetString(storyDungeon.nameKey);
		m_strSubName = CsConfiguration.Instance.GetString(storyDungeon.subNameKey);
		m_nEnterCount = storyDungeon.enterCount;
		m_nRequiredConditionType = storyDungeon.requiredConditionType;
		m_nRequiredHeroMinLevel = storyDungeon.requiredHeroMinLevel;
		m_nRequiredHeroMaxLevel = storyDungeon.requiredHeroMaxLevel;
		m_nRequiredMainQuestNo = storyDungeon.requiredMainQuestNo;
		m_nRequiredStamina = storyDungeon.requiredStamina;
		m_nLimitTime = storyDungeon.limitTime;
		m_strSceneName = storyDungeon.sceneName;
		m_flStartXPosition = storyDungeon.startXPosition;
		m_flStartYPosition = storyDungeon.startYPosition;
		m_flStartZPosition = storyDungeon.startZPosition;
		m_flStartRadius = storyDungeon.startRadius;
		m_flStartYRotation = storyDungeon.startYRotation;
		m_nStartDelayTime = storyDungeon.startDelayTime;
		m_nExitDelayTime = storyDungeon.exitDelayTime;
		m_flTamingXPosition = storyDungeon.tamingXPosition;
		m_flTamingYPosition = storyDungeon.tamingYPosition;
		m_flTamingZPosition = storyDungeon.tamingZPosition;
		m_flTamingYRotation = storyDungeon.tamingYRotation;
		m_flClearXPosition = storyDungeon.clearXPosition;
		m_flClearYPosition = storyDungeon.clearYPosition;
		m_flClearZPosition = storyDungeon.clearZPosition;
		m_flClearYRotation = storyDungeon.clearYRotation;
		m_nSafeRevivalWaitingTime = storyDungeon.safeRevivalWaitingTime;
		m_nGuideDisplayInterval = storyDungeon.guideDisplayInterval;
		m_nComboDuration = storyDungeon.comboDuration;
		m_nLocationId = storyDungeon.locationId;
		m_flX = storyDungeon.x;
		m_flZ = storyDungeon.z;
		m_flXSize = storyDungeon.xSize;
		m_flZSize = storyDungeon.zSize;

		m_listCsStoryDungeonDifficulty = new List<CsStoryDungeonDifficulty>();
		m_listCsStoryDungeonObstacle = new List<CsStoryDungeonObstacle>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsStoryDungeonDifficulty GetStoryDungeonDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsStoryDungeonDifficulty.Count; i++)
		{
			if (m_listCsStoryDungeonDifficulty[i].Difficulty == nDifficulty)
				return m_listCsStoryDungeonDifficulty[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset(DateTime dt)
	{
		m_dtDatePlay = dt;
		m_nPlayCount = 0;
	}


}
