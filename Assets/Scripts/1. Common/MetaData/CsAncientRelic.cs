using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

public class CsAncientRelic
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
	int m_nTrapActivateStartStep;
	int m_nTrapPenaltyMoveSpeed;
	int m_nTrapPenaltyDuration;
	int m_nTrapDamage;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsAncientRelicObstacle> m_listCsAncientRelicObstacle;
	List<CsAncientRelicAvailableReward> m_listCsAncientRelicAvailableReward;
	List<CsAncientRelicTrap> m_listCsAncientRelicTrap;
	List<CsAncientRelicStep> m_listCsAncientRelicStep;

	int m_nAncientRelicDailyPlayCount;
	DateTime m_dtDateAncientRelicPlayCount;

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

	public int TrapActivateStartStep
	{
		get { return m_nTrapActivateStartStep; }
	}

	public int TrapPenaltyMoveSpeed
	{
		get { return m_nTrapPenaltyMoveSpeed; }
	}

	public int TrapPenaltyDuration
	{
		get { return m_nTrapPenaltyDuration; }
	}

	public int TrapDamage
	{
		get { return m_nTrapDamage; }
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

	public List<CsAncientRelicObstacle> AncientRelicObstacleList
	{
		get { return m_listCsAncientRelicObstacle; }
	}

	public List<CsAncientRelicAvailableReward> AncientRelicAvailableRewardList
	{
		get { return m_listCsAncientRelicAvailableReward; }
	}

	public List<CsAncientRelicTrap> AncientRelicTrapList
	{
		get { return m_listCsAncientRelicTrap; }
	}

	public List<CsAncientRelicStep> AncientRelicStepList
	{
		get { return m_listCsAncientRelicStep; }
	}

	public int AncientRelicDailyPlayCount
	{
		get { return m_nAncientRelicDailyPlayCount; }
		set { m_nAncientRelicDailyPlayCount = value; }
	}

	public DateTime AncientRelicPlayCountDate
	{
		get { return m_dtDateAncientRelicPlayCount; }
		set { m_dtDateAncientRelicPlayCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelic(WPDAncientRelic ancientRelic)
	{
		m_strName = CsConfiguration.Instance.GetString(ancientRelic.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(ancientRelic.descriptionKey);
		m_strSceneName = ancientRelic.sceneName;
		m_nRequiredConditionType = ancientRelic.requiredConditionType;
		m_nRequiredMainQuestNo = ancientRelic.requiredMainQuestNo;
		m_nRequiredHeroLevel = ancientRelic.requiredHeroLevel;
		m_nRequiredStamina = ancientRelic.requiredStamina;
		m_nEnterMinMemberCount = ancientRelic.enterMinMemberCount;
		m_nEnterMaxMemberCount = ancientRelic.enterMaxMemberCount;
		m_nMatchingWaitingTime = ancientRelic.matchingWaitingTime;
		m_nEnterWaitingTime = ancientRelic.enterWaitingTime;
		m_nStartDelayTime = ancientRelic.startDelayTime;
		m_nLimitTime = ancientRelic.limitTime;
		m_nExitDelayTime = ancientRelic.exitDelayTime;
		m_nWaveIntervalTime = ancientRelic.waveIntervalTime;
        m_vtStartPosition = new Vector3(ancientRelic.startXPosition, ancientRelic.startYPosition, ancientRelic.startZPosition);
		m_flStartRadius = ancientRelic.startRadius;
		m_nStartYRotationType = ancientRelic.startYRotationType;
		m_flStartYRotation = ancientRelic.startYRotation;
		m_nSafeRevivalWaitingTime = ancientRelic.safeRevivalWaitingTime;
		m_nTrapActivateStartStep = ancientRelic.trapActivateStartStep;
		m_nTrapPenaltyMoveSpeed = ancientRelic.trapPenaltyMoveSpeed;
		m_nTrapPenaltyDuration = ancientRelic.trapPenaltyDuration;
		m_nTrapDamage = ancientRelic.trapDamage;
		m_nLocationId = ancientRelic.locationId;
		m_flX = ancientRelic.x;
		m_flZ = ancientRelic.z;
		m_flXSize = ancientRelic.xSize;
		m_flZSize = ancientRelic.zSize;

		m_listCsAncientRelicObstacle = new List<CsAncientRelicObstacle>();
		m_listCsAncientRelicAvailableReward = new List<CsAncientRelicAvailableReward>();
		m_listCsAncientRelicTrap = new List<CsAncientRelicTrap>();
		m_listCsAncientRelicStep = new List<CsAncientRelicStep>();

		m_nAncientRelicDailyPlayCount = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public CsAncientRelicStep GetAncientRelicStep(int nStep)
	{
		for (int i = 0; i < m_listCsAncientRelicStep.Count; i++)
		{
			if (m_listCsAncientRelicStep[i].Step == nStep)
				return m_listCsAncientRelicStep[i];
		}

		return null;
	}
}
