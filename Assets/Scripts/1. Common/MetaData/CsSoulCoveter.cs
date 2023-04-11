using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-10)
//---------------------------------------------------------------------------------------------------

public class CsSoulCoveter
{
	string m_strName;
	string m_strDescription;
    string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nRequiredStamina;
	int m_nEnterMinMemberCount;
	int m_nEnterMaxMemberCount;
	int m_nMatchingWaitingTime;
	int m_nEnterWaitingTime;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	int m_nWaveIntervalTime;
	Vector3 m_vtStartPosition;
	float m_flStartRadius;
	int m_nStartYRotationType;
	float m_flStartYRotation;
	int m_nSafeRevivalWaitingTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsSoulCoveterAvailableReward> m_listCsSoulCoveterAvailableReward;
	List<CsSoulCoveterObstacle> m_listCsSoulCoveterObstacle;
	List<CsSoulCoveterDifficulty> m_listCsSoulCoveterDifficulty;

	int m_nSoulCoveterWeeklyPlayCount;
	DateTime m_dtSoulCoveterPlayCountDate;

	//---------------------------------------------------------------------------------------------------
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

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
	}

	public int EnterMinMemberCount
	{
		get { return m_nEnterMinMemberCount; }
	}

	public int EnterMaxMemberCount
	{
		get { return m_nEnterMaxMemberCount; }
	}

	public int MatchingWaitingTime
	{
		get { return m_nMatchingWaitingTime; }
	}

	public int EnterWaitingTime
	{
		get { return m_nEnterWaitingTime; }
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

	public int WaveIntervalTime
	{
		get { return m_nWaveIntervalTime; }
	}

	public Vector3 StartPosition
	{
		get { return m_vtStartPosition; }
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

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
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

	public List<CsSoulCoveterAvailableReward> SoulCoveterAvailableRewardList
	{
		get { return m_listCsSoulCoveterAvailableReward; }
	}

	public List<CsSoulCoveterObstacle> SoulCoveterObstacleList
	{
		get { return m_listCsSoulCoveterObstacle; }
	}

	public List<CsSoulCoveterDifficulty> SoulCoveterDifficultyList
	{
		get { return m_listCsSoulCoveterDifficulty; }
	}

	public int SoulCoveterWeeklyPlayCount
	{
		get { return m_nSoulCoveterWeeklyPlayCount; }
		set { m_nSoulCoveterWeeklyPlayCount = value; }
	}

	public DateTime SoulCoveterPlayCountDate
	{
		get { return m_dtSoulCoveterPlayCountDate; }
		set { m_dtSoulCoveterPlayCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSoulCoveter(WPDSoulCoveter soulCoveter)
	{
		m_strName = CsConfiguration.Instance.GetString(soulCoveter.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(soulCoveter.descriptionKey);
        m_strSceneName = CsConfiguration.Instance.GetString(soulCoveter.sceneName);
		m_nRequiredConditionType = soulCoveter.requiredConditionType;
		m_nRequiredMainQuestNo = soulCoveter.requiredMainQuestNo;
		m_nRequiredHeroLevel = soulCoveter.requiredHeroLevel;
		m_nRequiredStamina = soulCoveter.requiredStamina;
		m_nEnterMinMemberCount = soulCoveter.enterMinMemberCount;
		m_nEnterMaxMemberCount = soulCoveter.enterMaxMemberCount;
		m_nMatchingWaitingTime = soulCoveter.matchingWaitingTime;
		m_nEnterWaitingTime = soulCoveter.enterWaitingTime;
		m_nStartDelayTime = soulCoveter.startDelayTime;
		m_nLimitTime = soulCoveter.limitTime;
		m_nExitDelayTime = soulCoveter.exitDelayTime;
		m_nWaveIntervalTime = soulCoveter.waveIntervalTime;
		m_vtStartPosition = new Vector3(soulCoveter.startXPosition, soulCoveter.startYPosition, soulCoveter.startZPosition);
		m_flStartRadius = soulCoveter.startRadius;
		m_nStartYRotationType = soulCoveter.startYRotationType;
		m_flStartYRotation = soulCoveter.startYRotation;
		m_nSafeRevivalWaitingTime = soulCoveter.safeRevivalWaitingTime;
		m_csLocation = CsGameData.Instance.GetLocation(soulCoveter.locationId);
		m_flX = soulCoveter.x;
		m_flZ = soulCoveter.z;
		m_flXSize = soulCoveter.xSize;
		m_flZSize = soulCoveter.zSize;

		m_listCsSoulCoveterAvailableReward = new List<CsSoulCoveterAvailableReward>();
		m_listCsSoulCoveterObstacle = new List<CsSoulCoveterObstacle>();
		m_listCsSoulCoveterDifficulty = new List<CsSoulCoveterDifficulty>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsSoulCoveterDifficulty GetSoulCoveterDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsSoulCoveterDifficulty.Count; i++)
		{
			if (m_listCsSoulCoveterDifficulty[i].Difficulty == nDifficulty)
				return m_listCsSoulCoveterDifficulty[i];
		}

		return null;
	}
}
