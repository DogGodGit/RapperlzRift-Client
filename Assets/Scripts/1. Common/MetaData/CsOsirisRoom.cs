using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-22)
//---------------------------------------------------------------------------------------------------

public class CsOsirisRoom
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredStamina;
	string m_strSceneName;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartYRotation;
	int m_nWaveInterval;
	int m_nMonsterSpawnInterval;
	float m_flMonsterSpawnXPosition;
	float m_flMonsterSpawnYPosition;
	float m_flMonsterSpawnZPosition;
	float m_flMonsterSpawnYRotation;
	float m_flTargetXPosition;
	float m_flTargetYPosition;
	float m_flTargetZPosition;
	float m_flTargetRadius;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

    DateTime m_dtPlayCountDate;
    int m_nDailyPlayCount = 0;

	List<CsOsirisRoomAvailableReward> m_listCsOsirisRoomAvailableReward;
	List<CsOsirisRoomDifficulty> m_listCsOsirisRoomDifficulty;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
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

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
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

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int WaveInterval
	{
		get { return m_nWaveInterval; }
	}

	public int MonsterSpawnInterval
	{
		get { return m_nMonsterSpawnInterval; }
	}

	public float MonsterSpawnXPosition
	{
		get { return m_flMonsterSpawnXPosition; }
	}

	public float MonsterSpawnYPosition
	{
		get { return m_flMonsterSpawnYPosition; }
	}

	public float MonsterSpawnZPosition
	{
		get { return m_flMonsterSpawnZPosition; }
	}

	public float MonsterSpawnYRotation
	{
		get { return m_flMonsterSpawnYRotation; }
	}

	public float TargetXPosition
	{
		get { return m_flTargetXPosition; }
	}

	public float TargetYPosition
	{
		get { return m_flTargetYPosition; }
	}

	public float TargetZPosition
	{
		get { return m_flTargetZPosition; }
	}

	public float TargetRadius
	{
		get { return m_flTargetRadius; }
	}

	public CsLocation Location
	{
		get { return m_csLocation; }
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

	public List<CsOsirisRoomAvailableReward> OsirisRoomAvailableRewardList
	{
		get { return m_listCsOsirisRoomAvailableReward; }
	}

	public List<CsOsirisRoomDifficulty> OsirisRoomDifficultyList
	{
		get { return m_listCsOsirisRoomDifficulty; }
	}

    public DateTime PlayCountDate
    {
        get { return m_dtPlayCountDate; }
        set { m_dtPlayCountDate = value; }
    }

    public int DailyPlayCount
    {
        get { return m_nDailyPlayCount; }
        set { m_nDailyPlayCount = value; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsOsirisRoom(WPDOsirisRoom osirisRoom)
	{
		m_strName = CsConfiguration.Instance.GetString(osirisRoom.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(osirisRoom.descriptionKey);
		m_nRequiredStamina = osirisRoom.requiredStamina;
		m_strSceneName = osirisRoom.sceneName;
		m_nStartDelayTime = osirisRoom.startDelayTime;
		m_nLimitTime = osirisRoom.limitTime;
		m_nExitDelayTime = osirisRoom.exitDelayTime;
		m_flStartXPosition = osirisRoom.startXPosition;
		m_flStartYPosition = osirisRoom.startYPosition;
		m_flStartZPosition = osirisRoom.startZPosition;
		m_flStartYRotation = osirisRoom.startYRotation;
		m_nWaveInterval = osirisRoom.waveInterval;
		m_nMonsterSpawnInterval = osirisRoom.monsterSpawnInterval;
		m_flMonsterSpawnXPosition = osirisRoom.monsterSpawnXPosition;
		m_flMonsterSpawnYPosition = osirisRoom.monsterSpawnYPosition;
		m_flMonsterSpawnZPosition = osirisRoom.monsterSpawnZPosition;
		m_flMonsterSpawnYRotation = osirisRoom.monsterSpawnYRotation;
		m_flTargetXPosition = osirisRoom.targetXPosition;
		m_flTargetYPosition = osirisRoom.targetYPosition;
		m_flTargetZPosition = osirisRoom.targetZPosition;
		m_flTargetRadius = osirisRoom.targetRadius;
		m_csLocation = CsGameData.Instance.GetLocation(osirisRoom.locationId);
		m_flX = osirisRoom.x;
		m_flZ = osirisRoom.z;
		m_flXSize = osirisRoom.xSize;
		m_flZSize = osirisRoom.zSize;

		m_listCsOsirisRoomAvailableReward = new List<CsOsirisRoomAvailableReward>();
		m_listCsOsirisRoomDifficulty = new List<CsOsirisRoomDifficulty>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsOsirisRoomDifficulty GetOsirisRoomDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsOsirisRoomDifficulty.Count; i++)
		{
			if (m_listCsOsirisRoomDifficulty[i].Difficulty == nDifficulty)
				return m_listCsOsirisRoomDifficulty[i];
		}

		return null;
	}
}
