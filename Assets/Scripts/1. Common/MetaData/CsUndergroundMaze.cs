using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-02)
//---------------------------------------------------------------------------------------------------

public class CsUndergroundMaze
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nLimitTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	int m_nStartYRotationType;
	float m_flStartYRotation;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsUndergroundMazeEntrance> m_listCsUndergroundMazeEntrance;
	List<CsUndergroundMazeFloor> m_listCsUndergroundMazeFloor;

	float m_flUndergroundMazeDailyPlayTime;
	DateTime m_dtDateUndergroundMazePlayTime;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
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

	public int StartYRotationType
	{
		get { return m_nStartYRotationType; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
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

	public List<CsUndergroundMazeEntrance> UndergroundMazeEntranceList
	{
		get { return m_listCsUndergroundMazeEntrance; }
	}

	public List<CsUndergroundMazeFloor> UndergroundMazeFloorList
	{
		get { return m_listCsUndergroundMazeFloor; }
	}

	public float UndergroundMazeDailyPlayTime
	{
		get { return m_flUndergroundMazeDailyPlayTime; }
		set { m_flUndergroundMazeDailyPlayTime = value; }
	}

	public DateTime UndergroundMazePlayTimeDate
	{
		get { return m_dtDateUndergroundMazePlayTime; }
		set { m_dtDateUndergroundMazePlayTime = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMaze(WPDUndergroundMaze undergroundMaze)
	{
		m_strName = CsConfiguration.Instance.GetString(undergroundMaze.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(undergroundMaze.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(undergroundMaze.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(undergroundMaze.targetContentKey);
		m_strSceneName = undergroundMaze.sceneName;
		m_nRequiredConditionType = undergroundMaze.requiredConditionType;
		m_nRequiredMainQuestNo = undergroundMaze.requiredMainQuestNo;
		m_nRequiredHeroLevel = undergroundMaze.requiredHeroLevel;
		m_nLimitTime = undergroundMaze.limitTime;
		m_flStartXPosition = undergroundMaze.startXPosition;
		m_flStartYPosition = undergroundMaze.startYPosition;
		m_flStartZPosition = undergroundMaze.startZPosition;
		m_flStartRadius = undergroundMaze.startRadius;
		m_nStartYRotationType = undergroundMaze.startYRotationType;
		m_flStartYRotation = undergroundMaze.startYRotation;
		m_nLocationId = undergroundMaze.locationId;
		m_flX = undergroundMaze.x;
		m_flZ = undergroundMaze.z;
		m_flXSize = undergroundMaze.xSize;
		m_flZSize = undergroundMaze.zSize;

		m_listCsUndergroundMazeEntrance = new List<CsUndergroundMazeEntrance>();
		m_listCsUndergroundMazeFloor = new List<CsUndergroundMazeFloor>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeFloor GetUndergroundMazeFloor(int nFloor)
	{
		for (int i = 0; i < m_listCsUndergroundMazeFloor.Count; i++)
		{
			if (m_listCsUndergroundMazeFloor[i].Floor == nFloor)
				return m_listCsUndergroundMazeFloor[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsUndergroundMazeEntrance GetUndergroundMazeEntrance(int nFloor)
	{
		for (int i = 0; i < m_listCsUndergroundMazeEntrance.Count; i++)
		{
			if (m_listCsUndergroundMazeEntrance[i].Floor == nFloor)
				return m_listCsUndergroundMazeEntrance[i];
		}

		return null;
	}
}
