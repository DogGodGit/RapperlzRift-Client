using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-23)
//---------------------------------------------------------------------------------------------------

public class CsRuinsReclaim
{
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nFreeEnterCount;
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
	int m_nDebuffAreaActivationStepNo;
	int m_nDebuffAreaDeactivationStepNo;
	float m_flDebuffAreaXPosition;
	float m_flDebuffAreaYPosition;
	float m_flDebuffAreaZPosition;
	float m_flDebuffAreaYRotation;
	int m_nDebuffAreaWidth;
	int m_nDebuffAreaHeight;
	float m_flDebuffAreaOffenseFactor;
	int m_nSummonMonsterHpRecoveryInterval;
	string m_strSummonMonsterHpRecoveryGuideImageName;
	string m_strSummonMonsterHpRecoveryGuideTitle;
	string m_strSummonMonsterHpRecoveryGuideContent;
	int m_nSafeRevivalWaitingTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	DateTime m_dtPlayDate;
	int m_nFreePlayCount = 0;

	List<CsRuinsReclaimMonsterAttrFactor> m_listCsRuinsReclaimMonsterAttrFactor;
	List<CsRuinsReclaimAvailableReward> m_listCsRuinsReclaimAvailableReward;
	List<CsRuinsReclaimRevivalPoint> m_listCsRuinsReclaimRevivalPoint;
	List<CsRuinsReclaimObstacle> m_listCsRuinsReclaimObstacle;
	List<CsRuinsReclaimTrap> m_listCsRuinsReclaimTrap;
	List<CsRuinsReclaimPortal> m_listCsRuinsReclaimPortal;
	List<CsRuinsReclaimOpenSchedule> m_listCsRuinsReclaimOpenSchedule;
	List<CsRuinsReclaimStep> m_listCsRuinsReclaimStep;
	List<CsRuinsReclaimRandomRewardPoolEntry> m_listCsRuinsReclaimRandomRewardPoolEntry;

	//---------------------------------------------------------------------------------------------------
	public DateTime PlayDate
	{
		get { return m_dtPlayDate; }
		set { m_dtPlayDate = value; }
	}

	public int FreePlayCount
	{
		get { return m_nFreePlayCount; }
		set { m_nFreePlayCount = value; }
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

	public int FreeEnterCount
	{
		get { return m_nFreeEnterCount; }
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

	public int DebuffAreaActivationStepNo
	{
		get { return m_nDebuffAreaActivationStepNo; }
	}

	public int DebuffAreaDeactivationStepNo
	{
		get { return m_nDebuffAreaDeactivationStepNo; }
	}

	public float DebuffAreaXPosition
	{
		get { return m_flDebuffAreaXPosition; }
	}

	public float DebuffAreaYPosition
	{
		get { return m_flDebuffAreaYPosition; }
	}

	public float DebuffAreaZPosition
	{
		get { return m_flDebuffAreaZPosition; }
	}

	public float DebuffAreaYRotation
	{
		get { return m_flDebuffAreaYRotation; }
	}

	public int DebuffAreaWidth
	{
		get { return m_nDebuffAreaWidth; }
	}

	public int DebuffAreaHeight
	{
		get { return m_nDebuffAreaHeight; }
	}

	public float DebuffAreaOffenseFactor
	{
		get { return m_flDebuffAreaOffenseFactor; }
	}

	public int SummonMonsterHpRecoveryInterval
	{
		get { return m_nSummonMonsterHpRecoveryInterval; }
	}

	public string SummonMonsterHpRecoveryGuideImageName
	{
		get { return m_strSummonMonsterHpRecoveryGuideImageName; }
	}

	public string SummonMonsterHpRecoveryGuideTitle
	{
		get { return m_strSummonMonsterHpRecoveryGuideTitle; }
	}

	public string SummonMonsterHpRecoveryGuideContent
	{
		get { return m_strSummonMonsterHpRecoveryGuideContent; }
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

	public List<CsRuinsReclaimMonsterAttrFactor> RuinsReclaimMonsterAttrFactorList
	{
		get { return m_listCsRuinsReclaimMonsterAttrFactor; }
	}

	public List<CsRuinsReclaimAvailableReward> RuinsReclaimAvailableRewardList
	{
		get { return m_listCsRuinsReclaimAvailableReward; }
	}

	public List<CsRuinsReclaimRevivalPoint> RuinsReclaimRevivalPointList
	{
		get { return m_listCsRuinsReclaimRevivalPoint; }
	}

	public List<CsRuinsReclaimObstacle> RuinsReclaimObstacleList
	{
		get { return m_listCsRuinsReclaimObstacle; }
	}

	public List<CsRuinsReclaimTrap> RuinsReclaimTrapList
	{
		get { return m_listCsRuinsReclaimTrap; }
	}

	public List<CsRuinsReclaimPortal> RuinsReclaimPortalList
	{
		get { return m_listCsRuinsReclaimPortal; }
	}

	public List<CsRuinsReclaimOpenSchedule> RuinsReclaimOpenScheduleList
	{
		get { return m_listCsRuinsReclaimOpenSchedule; }
	}

	public List<CsRuinsReclaimStep> RuinsReclaimStepList
	{
		get { return m_listCsRuinsReclaimStep; }
	}

	public List<CsRuinsReclaimRandomRewardPoolEntry> RuinsReclaimRandomRewardPoolEntryList
	{
		get { return m_listCsRuinsReclaimRandomRewardPoolEntry; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaim(WPDRuinsReclaim ruinsReclaim)
	{
		m_strName = CsConfiguration.Instance.GetString(ruinsReclaim.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(ruinsReclaim.descriptionKey);
		m_strSceneName = ruinsReclaim.sceneName;
		m_nRequiredConditionType = ruinsReclaim.requiredConditionType;
		m_nRequiredMainQuestNo = ruinsReclaim.requiredMainQuestNo;
		m_nRequiredHeroLevel = ruinsReclaim.requiredHeroLevel;
		m_nFreeEnterCount = ruinsReclaim.freeEnterCount;
		m_csItemEnterRequired = CsGameData.Instance.GetItem(ruinsReclaim.enterRequiredItemId);
		m_nEnterMinMemberCount = ruinsReclaim.enterMinMemberCount;
		m_nEnterMaxMemberCount = ruinsReclaim.enterMaxMemberCount;
		m_nMatchingWaitingTime = ruinsReclaim.matchingWaitingTime;
		m_nEnterWaitingTime = ruinsReclaim.enterWaitingTime;
		m_nStartDelayTime = ruinsReclaim.startDelayTime;
		m_nLimitTime = ruinsReclaim.limitTime;
		m_nExitDelayTime = ruinsReclaim.exitDelayTime;
		m_flStartXPosition = ruinsReclaim.startXPosition;
		m_flStartYPosition = ruinsReclaim.startYPosition;
		m_flStartZPosition = ruinsReclaim.startZPosition;
		m_flStartRadius = ruinsReclaim.startRadius;
		m_nStartYRotationType = ruinsReclaim.startYRotationType;
		m_flStartYRotation = ruinsReclaim.startYRotation;
		m_nDebuffAreaActivationStepNo = ruinsReclaim.debuffAreaActivationStepNo;
		m_nDebuffAreaDeactivationStepNo = ruinsReclaim.debuffAreaDeactivationStepNo;
		m_flDebuffAreaXPosition = ruinsReclaim.debuffAreaXPosition;
		m_flDebuffAreaYPosition = ruinsReclaim.debuffAreaYPosition;
		m_flDebuffAreaZPosition = ruinsReclaim.debuffAreaZPosition;
		m_flDebuffAreaYRotation = ruinsReclaim.debuffAreaYRotation;
		m_nDebuffAreaWidth = ruinsReclaim.debuffAreaWidth;
		m_nDebuffAreaHeight = ruinsReclaim.debuffAreaHeight;
		m_flDebuffAreaOffenseFactor = ruinsReclaim.debuffAreaOffenseFactor;
		m_nSummonMonsterHpRecoveryInterval = ruinsReclaim.summonMonsterHpRecoveryInterval;
		m_strSummonMonsterHpRecoveryGuideImageName = ruinsReclaim.summonMonsterHpRecoveryGuideImageName;
		m_strSummonMonsterHpRecoveryGuideTitle = CsConfiguration.Instance.GetString(ruinsReclaim.summonMonsterHpRecoveryGuideTitleKey);
		m_strSummonMonsterHpRecoveryGuideContent = CsConfiguration.Instance.GetString(ruinsReclaim.summonMonsterHpRecoveryGuideContentKey);
		m_nSafeRevivalWaitingTime = ruinsReclaim.safeRevivalWaitingTime;
		m_csLocation = CsGameData.Instance.GetLocation(ruinsReclaim.locationId);
		m_flX = ruinsReclaim.x;
		m_flZ = ruinsReclaim.z;
		m_flXSize = ruinsReclaim.xSize;
		m_flZSize = ruinsReclaim.zSize;

		m_listCsRuinsReclaimMonsterAttrFactor = new List<CsRuinsReclaimMonsterAttrFactor>();
		m_listCsRuinsReclaimAvailableReward = new List<CsRuinsReclaimAvailableReward>();
		m_listCsRuinsReclaimRevivalPoint = new List<CsRuinsReclaimRevivalPoint>();
		m_listCsRuinsReclaimObstacle = new List<CsRuinsReclaimObstacle>();
		m_listCsRuinsReclaimTrap = new List<CsRuinsReclaimTrap>();
		m_listCsRuinsReclaimPortal = new List<CsRuinsReclaimPortal>();
		m_listCsRuinsReclaimOpenSchedule = new List<CsRuinsReclaimOpenSchedule>();
		m_listCsRuinsReclaimStep = new List<CsRuinsReclaimStep>();
		m_listCsRuinsReclaimRandomRewardPoolEntry = new List<CsRuinsReclaimRandomRewardPoolEntry>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimStep GetRuinsReclaimStep(int nStepNo)
	{
		for (int i = 0; i < m_listCsRuinsReclaimStep.Count; i++)
		{
			if (m_listCsRuinsReclaimStep[i].StepNo == nStepNo)
			{
				return m_listCsRuinsReclaimStep[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsRuinsReclaimPortal GetRuinsReclaimPortal(int nPortalId)
	{
		for (int i = 0; i < m_listCsRuinsReclaimPortal.Count; i++)
		{
			if (m_listCsRuinsReclaimPortal[i].PortalId == nPortalId)
			{
				return m_listCsRuinsReclaimPortal[i];
			}
		}

		return null;
	}
}
