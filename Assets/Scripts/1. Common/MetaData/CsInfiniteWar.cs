using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-26)
//---------------------------------------------------------------------------------------------------

public class CsInfiniteWar
{
	string m_strName;
	string m_strDescription;
	int m_nEnterCount;
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
	string m_strGuideImageName;
	string m_strStartGuideTitle;
	string m_strStartGuideContent;
	int m_nMonsterSpawnDelayTime;
	string m_strMonsterSpawnGuideTitle;
	string m_strMonsterSpawnGuideContent;
	int m_nHeroKillPoint;
	int m_nBuffBoxCreationInterval;
	int m_nBuffBoxCreationCount;
	float m_flBuffBoxXPosition;
	float m_flBuffBoxYPosition;
	float m_flBuffBoxZPosition;
	float m_flBuffBoxRadius;
	int m_nBuffBoxLifetime;
	float m_flBuffBoxAcquisitionRange;
	int m_nBuffDuration;
	string m_strBuffCreationGuideTitle;
	string m_strBuffCreationGuideContent;
	int m_nSafeRevivalWaitingTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	DateTime m_dtPlayDate;
	int m_nDailyPlayCount = 0;

	List<CsInfiniteWarBuffBox> m_listCsInfiniteWarBuffBox;
	List<CsInfiniteWarMonsterAttrFactor> m_listCsInfiniteWarMonsterAttrFactor;
	List<CsInfiniteWarOpenSchedule> m_listCsInfiniteWarOpenSchedule;
	List<CsInfiniteWarAvailableReward> m_listCsInfiniteWarAvailableReward;

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public DateTime PlayDate
	{
		get { return m_dtPlayDate; }
		set { m_dtPlayDate = value; }
	}

	public int DailyPlayCount
	{
		get { return m_nDailyPlayCount; }
		set { m_nDailyPlayCount = value; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int EnterCount
	{
		get { return m_nEnterCount; }
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

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string StartGuideTitle
	{
		get { return m_strStartGuideTitle; }
	}

	public string StartGuideContent
	{
		get { return m_strStartGuideContent; }
	}

	public int MonsterSpawnDelayTime
	{
		get { return m_nMonsterSpawnDelayTime; }
	}

	public string MonsterSpawnGuideTitle
	{
		get { return m_strMonsterSpawnGuideTitle; }
	}

	public string MonsterSpawnGuideContent
	{
		get { return m_strMonsterSpawnGuideContent; }
	}

	public int HeroKillPoint
	{
		get { return m_nHeroKillPoint; }
	}

	public int BuffBoxCreationInterval
	{
		get { return m_nBuffBoxCreationInterval; }
	}

	public int BuffBoxCreationCount
	{
		get { return m_nBuffBoxCreationCount; }
	}

	public float BuffBoxXPosition
	{
		get { return m_flBuffBoxXPosition; }
	}

	public float BuffBoxYPosition
	{
		get { return m_flBuffBoxYPosition; }
	}

	public float BuffBoxZPosition
	{
		get { return m_flBuffBoxZPosition; }
	}

	public float BuffBoxRadius
	{
		get { return m_flBuffBoxRadius; }
	}

	public int BuffBoxLifetime
	{
		get { return m_nBuffBoxLifetime; }
	}

	public float BuffBoxAcquisitionRange
	{
		get { return m_flBuffBoxAcquisitionRange; }
	}

	public int BuffDuration
	{
		get { return m_nBuffDuration; }
	}

	public string BuffCreationGuideTitle
	{
		get { return m_strBuffCreationGuideTitle; }
	}

	public string BuffCreationGuideContent
	{
		get { return m_strBuffCreationGuideContent; }
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

	public List<CsInfiniteWarBuffBox> InfiniteWarBuffBoxList
	{
		get { return m_listCsInfiniteWarBuffBox; }
	}

	public List<CsInfiniteWarMonsterAttrFactor> InfiniteWarMonsterAttrFactorList
	{
		get { return m_listCsInfiniteWarMonsterAttrFactor; }
	}

	public List<CsInfiniteWarOpenSchedule> InfiniteWarOpenScheduleList
	{
		get { return m_listCsInfiniteWarOpenSchedule; }
	}

	public List<CsInfiniteWarAvailableReward> InfiniteWarAvailableRewardList
	{
		get { return m_listCsInfiniteWarAvailableReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsInfiniteWar(WPDInfiniteWar infiniteWar)
	{
		m_strName = CsConfiguration.Instance.GetString(infiniteWar.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(infiniteWar.descriptionKey);
		m_nEnterCount = infiniteWar.enterCount;
		m_strSceneName = infiniteWar.sceneName;
		m_nRequiredConditionType = infiniteWar.requiredConditionType;
		m_nRequiredMainQuestNo = infiniteWar.requiredMainQuestNo;
		m_nRequiredHeroLevel = infiniteWar.requiredHeroLevel;
		m_nRequiredStamina = infiniteWar.requiredStamina;
		m_nEnterMinMemberCount = infiniteWar.enterMinMemberCount;
		m_nEnterMaxMemberCount = infiniteWar.enterMaxMemberCount;
		m_nMatchingWaitingTime = infiniteWar.matchingWaitingTime;
		m_nEnterWaitingTime = infiniteWar.enterWaitingTime;
		m_nStartDelayTime = infiniteWar.startDelayTime;
		m_nLimitTime = infiniteWar.limitTime;
		m_nExitDelayTime = infiniteWar.exitDelayTime;
		m_strGuideImageName = infiniteWar.guideImageName;
		m_strStartGuideTitle = CsConfiguration.Instance.GetString(infiniteWar.startGuideTitleKey);
		m_strStartGuideContent = CsConfiguration.Instance.GetString(infiniteWar.startGuideContentKey);
		m_nMonsterSpawnDelayTime = infiniteWar.monsterSpawnDelayTime;
		m_strMonsterSpawnGuideTitle = CsConfiguration.Instance.GetString(infiniteWar.monsterSpawnGuideTitleKey);
		m_strMonsterSpawnGuideContent = CsConfiguration.Instance.GetString(infiniteWar.monsterSpawnGuideContentKey);
		m_nHeroKillPoint = infiniteWar.heroKillPoint;
		m_nBuffBoxCreationInterval = infiniteWar.buffBoxCreationInterval;
		m_nBuffBoxCreationCount = infiniteWar.buffBoxCreationCount;
		m_flBuffBoxXPosition = infiniteWar.buffBoxXPosition;
		m_flBuffBoxYPosition = infiniteWar.buffBoxYPosition;
		m_flBuffBoxZPosition = infiniteWar.buffBoxZPosition;
		m_flBuffBoxRadius = infiniteWar.buffBoxRadius;
		m_nBuffBoxLifetime = infiniteWar.buffBoxLifetime;
		m_flBuffBoxAcquisitionRange = infiniteWar.buffBoxAcquisitionRange;
		m_nBuffDuration = infiniteWar.buffDuration;
		m_strBuffCreationGuideTitle = CsConfiguration.Instance.GetString(infiniteWar.buffCreationGuideTitleKey);
		m_strBuffCreationGuideContent = CsConfiguration.Instance.GetString(infiniteWar.buffCreationGuideContentKey);
		m_nSafeRevivalWaitingTime = infiniteWar.safeRevivalWaitingTime;
		m_csLocation = CsGameData.Instance.GetLocation(infiniteWar.locationId);
		m_flX = infiniteWar.x;
		m_flZ = infiniteWar.z;
		m_flXSize = infiniteWar.xSize;
		m_flZSize = infiniteWar.zSize;

		m_listCsInfiniteWarBuffBox = new List<CsInfiniteWarBuffBox>();
		m_listCsInfiniteWarMonsterAttrFactor = new List<CsInfiniteWarMonsterAttrFactor>();
		m_listCsInfiniteWarOpenSchedule = new List<CsInfiniteWarOpenSchedule>();
		m_listCsInfiniteWarAvailableReward = new List<CsInfiniteWarAvailableReward>();
	}

	public CsInfiniteWarBuffBox GetInfiniteWarBuffBox(int nBuffBoxId)
	{
		for (int i = 0; i < m_listCsInfiniteWarBuffBox.Count; i++)
		{
			if (m_listCsInfiniteWarBuffBox[i].BuffBoxId == nBuffBoxId)
			{
				return m_listCsInfiniteWarBuffBox[i];
			}
		}
		return null;
	}
}
