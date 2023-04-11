using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-07)
//---------------------------------------------------------------------------------------------------

public class CsArtifactRoom
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
	int m_nContinuationChallengeWaitingTime;
	int m_nStartDelayTime;
	int m_nExitDelayTime;
    Vector3 m_vtStartPosition;
	float m_flStartYRotation;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsArtifactRoomFloor> m_listCsArtifactRoomFloor;

	int m_nArtifactRoomBestFloor;
	int m_nArtifactRoomCurrentFloor;
	int m_nArtifactRoomDailyInitCount;
	DateTime m_dtDateArtifactRoomInitCount;

	int m_nArtifactRoomSweepProgressFloor;
	float m_flArtifactRoomSweepRemainingTime;

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

	public int ContinuationChallengeWaitingTime
	{
		get { return m_nContinuationChallengeWaitingTime; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
	}

    public Vector3 StartPosition
    {
        get { return m_vtStartPosition; }
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

	public List<CsArtifactRoomFloor> ArtifactRoomFloorList
	{
		get { return m_listCsArtifactRoomFloor; }
	}

	public int ArtifactRoomBestFloor
	{
		get { return m_nArtifactRoomBestFloor; }
		set { m_nArtifactRoomBestFloor = value; }
	}

	public int ArtifactRoomCurrentFloor
	{
		get { return m_nArtifactRoomCurrentFloor; }
		set { m_nArtifactRoomCurrentFloor = value; }
	}

	public int ArtifactRoomDailyInitCount
	{
		get { return m_nArtifactRoomDailyInitCount; }
		set { m_nArtifactRoomDailyInitCount = value; }
	}

	public DateTime ArtifactRoomInitCountDate
	{
		get { return m_dtDateArtifactRoomInitCount; }
		set { m_dtDateArtifactRoomInitCount = value; }
	}

	public int ArtifactRoomSweepProgressFloor
	{
		get { return m_nArtifactRoomSweepProgressFloor; }
		set { m_nArtifactRoomSweepProgressFloor = value; }
	}

	public float ArtifactRoomSweepRemainingTime
	{
		get { return m_flArtifactRoomSweepRemainingTime; }
		set { m_flArtifactRoomSweepRemainingTime = value + Time.realtimeSinceStartup; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactRoom(WPDArtifactRoom artifactRoom)
	{
		m_strName = CsConfiguration.Instance.GetString(artifactRoom.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(artifactRoom.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(artifactRoom.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(artifactRoom.targetContentKey);
		m_strSceneName = artifactRoom.sceneName;
		m_nRequiredConditionType = artifactRoom.requiredConditionType;
		m_nRequiredMainQuestNo = artifactRoom.requiredMainQuestNo;
		m_nRequiredHeroLevel = artifactRoom.requiredHeroLevel;
		m_nLimitTime = artifactRoom.limitTime;
		m_nContinuationChallengeWaitingTime = artifactRoom.continuationChallengeWaitingTime;
		m_nStartDelayTime = artifactRoom.startDelayTime;
		m_nExitDelayTime = artifactRoom.exitDelayTime;
        m_vtStartPosition = new Vector3(artifactRoom.startXPosition, artifactRoom.startYPosition, artifactRoom.startZPosition);
		m_flStartYRotation = artifactRoom.startYRotation;
		m_nLocationId = artifactRoom.locationId;
		m_flX = artifactRoom.x;
		m_flZ = artifactRoom.z;
		m_flXSize = artifactRoom.xSize;
		m_flZSize = artifactRoom.zSize;

		m_listCsArtifactRoomFloor = new List<CsArtifactRoomFloor>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsArtifactRoomFloor GetArtifactRoomFloor(int nFloor)
	{
		for (int i = 0; i < m_listCsArtifactRoomFloor.Count; i++)
		{
			if (m_listCsArtifactRoomFloor[i].Floor == nFloor)
				return m_listCsArtifactRoomFloor[i];
		}

		return null;
	}
}
