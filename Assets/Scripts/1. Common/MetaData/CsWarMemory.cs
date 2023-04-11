using System.Collections.Generic;
using WebCommon;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-14)
//---------------------------------------------------------------------------------------------------

public class CsWarMemory
{
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nFreeEnterCount;
	int m_nEnterRequiredItemId;
	int m_nEnterMinMemberCount;
	int m_nEnterMaxMemberCount;
	int m_nMatchingWaitingTime;
	int m_nEnterWaitingTime;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	string m_strTransformationGuideImageName;
	string m_strTransformationGuideTitle;
	string m_strTransformationGuideContent;
	string m_strMonsterSummonGuideTitle;
	string m_strMonsterSummonGuideContent;
	int m_nSafeRevivalWaitingTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

    DateTime m_dtPlayDate;
    int m_nFreePlayCount = 0;

	List<CsWarMemoryMonsterAttrFactor> m_listCsWarMemoryMonsterAttrFactor;
	List<CsWarMemoryStartPosition> m_listCsWarMemoryStartPosition;
	List<CsWarMemorySchedule> m_listCsWarMemorySchedule;
	List<CsWarMemoryAvailableReward> m_listCsWarMemoryAvailableReward;
	List<CsWarMemoryReward> m_listCsWarMemoryReward;
	List<CsWarMemoryRankingReward> m_listCsWarMemoryRankingReward;
	List<CsWarMemoryWave> m_listCsWarMemoryWave;

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

	public int EnterRequiredItemId
	{
		get { return m_nEnterRequiredItemId; }
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

	public string TransformationGuideImageName
	{
		get { return m_strTransformationGuideImageName; }
	}

	public string TransformationGuideTitle
	{
		get { return m_strTransformationGuideTitle; }
	}

	public string TransformationGuideContent
	{
		get { return m_strTransformationGuideContent; }
	}

	public string MonsterSummonGuideTitle
	{
		get { return m_strMonsterSummonGuideTitle; }
	}

	public string MonsterSummonGuideContent
	{
		get { return m_strMonsterSummonGuideContent; }
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

	public List<CsWarMemoryMonsterAttrFactor> WarMemoryMonsterAttrFactorList
	{
		get { return m_listCsWarMemoryMonsterAttrFactor; }
	}

	public List<CsWarMemoryStartPosition> WarMemoryStartPositionList
	{
		get { return m_listCsWarMemoryStartPosition; }
	}

	public List<CsWarMemorySchedule> WarMemoryScheduleList
	{
		get { return m_listCsWarMemorySchedule; }
	}

	public List<CsWarMemoryAvailableReward> WarMemoryAvailableRewardList
	{
		get { return m_listCsWarMemoryAvailableReward; }
	}

	public List<CsWarMemoryReward> WarMemoryRewardList
	{
		get { return m_listCsWarMemoryReward; }
	}

	public List<CsWarMemoryRankingReward> WarMemoryRankingRewardList
	{
		get { return m_listCsWarMemoryRankingReward; }
	}

	public List<CsWarMemoryWave> WarMemoryWaveList
	{
		get { return m_listCsWarMemoryWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemory(WPDWarMemory warMemory)
	{
		m_strName = CsConfiguration.Instance.GetString(warMemory.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(warMemory.descriptionKey);
		m_strSceneName = warMemory.sceneName;
		m_nRequiredConditionType = warMemory.requiredConditionType;
		m_nRequiredMainQuestNo = warMemory.requiredMainQuestNo;
		m_nRequiredHeroLevel = warMemory.requiredHeroLevel;
		m_nFreeEnterCount = warMemory.freeEnterCount;
		m_nEnterRequiredItemId = warMemory.enterRequiredItemId;
		m_nEnterMinMemberCount = warMemory.enterMinMemberCount;
		m_nEnterMaxMemberCount = warMemory.enterMaxMemberCount;
		m_nMatchingWaitingTime = warMemory.matchingWaitingTime;
		m_nEnterWaitingTime = warMemory.enterWaitingTime;
		m_nStartDelayTime = warMemory.startDelayTime;
		m_nLimitTime = warMemory.limitTime;
		m_nExitDelayTime = warMemory.exitDelayTime;
		m_strTransformationGuideImageName = warMemory.transformationGuideImageName;
		m_strTransformationGuideTitle = CsConfiguration.Instance.GetString(warMemory.transformationGuideTitleKey);
		m_strTransformationGuideContent = CsConfiguration.Instance.GetString(warMemory.transformationGuideContentKey);
		m_strMonsterSummonGuideTitle = CsConfiguration.Instance.GetString(warMemory.monsterSummonGuideTitleKey);
		m_strMonsterSummonGuideContent = CsConfiguration.Instance.GetString(warMemory.monsterSummonGuideContentKey);
		m_nSafeRevivalWaitingTime = warMemory.safeRevivalWaitingTime;
		m_csLocation = CsGameData.Instance.GetLocation(warMemory.locationId);
		m_flX = warMemory.x;
		m_flZ = warMemory.z;
		m_flXSize = warMemory.xSize;
		m_flZSize = warMemory.zSize;

		m_listCsWarMemoryMonsterAttrFactor = new List<CsWarMemoryMonsterAttrFactor>();
		m_listCsWarMemoryStartPosition = new List<CsWarMemoryStartPosition>();
		m_listCsWarMemorySchedule = new List<CsWarMemorySchedule>();
		m_listCsWarMemoryAvailableReward = new List<CsWarMemoryAvailableReward>();
		m_listCsWarMemoryReward = new List<CsWarMemoryReward>();
		m_listCsWarMemoryRankingReward = new List<CsWarMemoryRankingReward>();
		m_listCsWarMemoryWave = new List<CsWarMemoryWave>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsWarMemoryWave GetWarMemoryWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsWarMemoryWave.Count; i++)
		{
			if (m_listCsWarMemoryWave[i].WaveNo == nWaveNo)
				return m_listCsWarMemoryWave[i];
		}

		return null;
	}
}
