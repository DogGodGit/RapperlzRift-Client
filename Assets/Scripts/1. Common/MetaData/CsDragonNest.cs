using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsDragonNest
{
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	CsItem m_csItemEnterRequired;
	int m_nEnterMinMemberCount;
	int m_nEnterMaxMemberCount;
	int m_nMatchingWaitingTime;
	int m_nEnterWaitingTime;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	int m_nStartYRotationType;
	float m_flStartYRotation;
	int m_nSafeRevivalWaitingTime;
	int m_nBaseMaxStep;
	string m_strAreaEffectPrefabName;
	float m_flAreaEffectScale;
	int m_nTrapPenaltyMoveSpeed;
	int m_nTrapPenaltyDuration;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsDragonNestAvailableReward> m_listCsDragonNestAvailableReward;
	List<CsDragonNestObstacle> m_listCsDragonNestObstacle;
	List<CsDragonNestTrap> m_listCsDragonNestTrap;
	List<CsDragonNestStep> m_listCsDragonNestStep;

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

	public CsItem EnterRequiredItem
	{
		get { return m_csItemEnterRequired; }
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

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public int BaseMaxStep
	{
		get { return m_nBaseMaxStep; }
	}

	public string AreaEffectPrefabName
	{
		get { return m_strAreaEffectPrefabName; }
	}

	public float AreaEffectScale
	{
		get { return m_flAreaEffectScale; }
	}

	public int TrapPenaltyMoveSpeed
	{
		get { return m_nTrapPenaltyMoveSpeed; }
	}

	public int TrapPenaltyDuration
	{
		get { return m_nTrapPenaltyDuration; }
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

	public List<CsDragonNestAvailableReward> DragonNestAvailableRewardList
	{
		get { return m_listCsDragonNestAvailableReward; }
	}

	public List<CsDragonNestObstacle> DragonNestObstacleList
	{
		get { return m_listCsDragonNestObstacle; }
	}

	public List<CsDragonNestTrap> DragonNestTrapList
	{
		get { return m_listCsDragonNestTrap; }
	}

	public List<CsDragonNestStep> DragonNestStepList
	{
		get { return m_listCsDragonNestStep; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public CsDragonNest(WPDDragonNest dragonNest)
	{
		m_strName = CsConfiguration.Instance.GetString(dragonNest.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(dragonNest.descriptionKey);
		m_strSceneName = dragonNest.sceneName;
		m_nRequiredConditionType = dragonNest.requiredConditionType;
		m_nRequiredMainQuestNo = dragonNest.requiredMainQuestNo;
		m_nRequiredHeroLevel = dragonNest.requiredHeroLevel;
		m_csItemEnterRequired = CsGameData.Instance.GetItem(dragonNest.enterRequiredItemId);
		m_nEnterMinMemberCount = dragonNest.enterMinMemberCount;
		m_nEnterMaxMemberCount = dragonNest.enterMaxMemberCount;
		m_nMatchingWaitingTime = dragonNest.matchingWaitingTime;
		m_nEnterWaitingTime = dragonNest.enterWaitingTime;
		m_nStartDelayTime = dragonNest.startDelayTime;
		m_nLimitTime = dragonNest.limitTime;
		m_nExitDelayTime = dragonNest.exitDelayTime;
		m_flStartXPosition = dragonNest.startXPosition;
		m_flStartYPosition = dragonNest.startYPosition;
		m_flStartZPosition = dragonNest.startZPosition;
		m_flStartRadius = dragonNest.startRadius;
		m_nStartYRotationType = dragonNest.startYRotationType;
		m_flStartYRotation = dragonNest.startYRotation;
		m_nSafeRevivalWaitingTime = dragonNest.safeRevivalWaitingTime;
		m_nBaseMaxStep = dragonNest.baseMaxStep;
		m_strAreaEffectPrefabName = dragonNest.areaEffectPrefabName;
		m_flAreaEffectScale = dragonNest.areaEffectScale;
		m_nTrapPenaltyMoveSpeed = dragonNest.trapPenaltyMoveSpeed;
		m_nTrapPenaltyDuration = dragonNest.trapPenaltyDuration;
		m_csLocation = CsGameData.Instance.GetLocation(dragonNest.locationId);
		m_flX = dragonNest.x;
		m_flZ = dragonNest.z;
		m_flXSize = dragonNest.xSize;
		m_flZSize = dragonNest.zSize;

		m_listCsDragonNestAvailableReward = new List<CsDragonNestAvailableReward>();
		m_listCsDragonNestObstacle = new List<CsDragonNestObstacle>();
		m_listCsDragonNestTrap = new List<CsDragonNestTrap>();
		m_listCsDragonNestStep = new List<CsDragonNestStep>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsDragonNestStep GetDragonNestStep(int nStepNo)
	{
		for (int i = 0; i < m_listCsDragonNestStep.Count; i++)
		{
			if (m_listCsDragonNestStep[i].StepNo == nStepNo)
				return m_listCsDragonNestStep[i];
		}

		return null;
	}
}
