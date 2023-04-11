using System.Collections.Generic;
using WebCommon;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-27)
//---------------------------------------------------------------------------------------------------

public class CsAnkouTomb
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strSceneName;
	int m_nRequiredConditionType;
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;
	int m_nRequiredStamina;
	int m_nEnterCount;
	int m_nEnterMinMemberCount;
	int m_nEnterMaxMemberCount;
	int m_nWaveCount;
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
	float m_flMonsterSpawnXPosition;
	float m_flMonsterSpawnYPosition;
	float m_flMonsterSpawnZPosition;
	float m_flMonsterSpawnRadius;
	int m_nMonsterPoint;
	int m_nBossMonsterPoint;
	int m_nClearPoint;
	int m_nExp2xRewardRequiredUnOwnDia;
	int m_nExp3xRewardRequiredUnOwnDia;
	int m_nSafeRevivalWaitingTime;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

    int m_nPlayCount;
    DateTime m_dtPlayDate;

	List<CsAnkouTombSchedule> m_listCsAnkouTombSchedule;
	List<CsAnkouTombDifficulty> m_listCsAnkouTombDifficulty;

    List<CsHeroAnkouTombBestRecord> m_listMyCsHeroAnkouTombBestRecord;
    List<CsHeroAnkouTombBestRecord> m_listServerCsHeroAnkouTombBestRecord;

	//---------------------------------------------------------------------------------------------------
    public int PlayCount
    {
        get { return m_nPlayCount; }
        set { m_nPlayCount = value; }
    }
    
    public DateTime PlayDate
    {
        get { return m_dtPlayDate; }
        set { m_dtPlayDate = value; }
    }

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

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
	}

	public int EnterCount
	{
		get { return m_nEnterCount; }
	}

	public int EnterMinMemberCount
	{
		get { return m_nEnterMinMemberCount; }
	}

	public int EnterMaxMemberCount
	{
		get { return m_nEnterMaxMemberCount; }
	}

	public int WaveCount
	{
		get { return m_nWaveCount; }
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

	public float MonsterSpawnRadius
	{
		get { return m_flMonsterSpawnRadius; }
	}

	public int MonsterPoint
	{
		get { return m_nMonsterPoint; }
	}

	public int BossMonsterPoint
	{
		get { return m_nBossMonsterPoint; }
	}

	public int ClearPoint
	{
		get { return m_nClearPoint; }
	}

	public int Exp2xRewardRequiredUnOwnDia
	{
		get { return m_nExp2xRewardRequiredUnOwnDia; }
	}

	public int Exp3xRewardRequiredUnOwnDia
	{
		get { return m_nExp3xRewardRequiredUnOwnDia; }
	}

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
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

	public List<CsAnkouTombSchedule> AnkouTombScheduleList
	{
		get { return m_listCsAnkouTombSchedule; }
	}

	public List<CsAnkouTombDifficulty> AnkouTombDifficultyList
	{
		get { return m_listCsAnkouTombDifficulty; }
	}

    public List<CsHeroAnkouTombBestRecord> MyHeroAnkouTombBestRecordList
    {
        get { return m_listMyCsHeroAnkouTombBestRecord; }
    }

    public List<CsHeroAnkouTombBestRecord> ServerHeroAnkouTombBestRecordList
    {
        get { return m_listServerCsHeroAnkouTombBestRecord; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsAnkouTomb(WPDAnkouTomb ankouTomb)
	{
		m_strName = CsConfiguration.Instance.GetString(ankouTomb.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(ankouTomb.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(ankouTomb.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(ankouTomb.targetContentKey);
		m_strSceneName = ankouTomb.sceneName;
		m_nRequiredConditionType = ankouTomb.requiredConditionType;
		m_nRequiredHeroLevel = ankouTomb.requiredHeroLevel;
		m_nRequiredMainQuestNo = ankouTomb.requiredMainQuestNo;
		m_nRequiredStamina = ankouTomb.requiredStamina;
		m_nEnterCount = ankouTomb.enterCount;
		m_nEnterMinMemberCount = ankouTomb.enterMinMemberCount;
		m_nEnterMaxMemberCount = ankouTomb.enterMaxMemberCount;
		m_nWaveCount = ankouTomb.waveCount;
		m_nMatchingWaitingTime = ankouTomb.matchingWaitingTime;
		m_nEnterWaitingTime = ankouTomb.enterWaitingTime;
		m_nStartDelayTime = ankouTomb.startDelayTime;
		m_nLimitTime = ankouTomb.limitTime;
		m_nExitDelayTime = ankouTomb.exitDelayTime;
		m_flStartXPosition = ankouTomb.startXPosition;
		m_flStartYPosition = ankouTomb.startYPosition;
		m_flStartZPosition = ankouTomb.startZPosition;
		m_flStartRadius = ankouTomb.startRadius;
		m_nStartYRotationType = ankouTomb.startYRotationType;
		m_flStartYRotation = ankouTomb.startYRotation;
		m_flMonsterSpawnXPosition = ankouTomb.monsterSpawnXPosition;
		m_flMonsterSpawnYPosition = ankouTomb.monsterSpawnYPosition;
		m_flMonsterSpawnZPosition = ankouTomb.monsterSpawnZPosition;
		m_flMonsterSpawnRadius = ankouTomb.monsterSpawnRadius;
		m_nMonsterPoint = ankouTomb.monsterPoint;
		m_nBossMonsterPoint = ankouTomb.bossMonsterPoint;
		m_nClearPoint = ankouTomb.clearPoint;
		m_nExp2xRewardRequiredUnOwnDia = ankouTomb.exp2xRewardRequiredUnOwnDia;
		m_nExp3xRewardRequiredUnOwnDia = ankouTomb.exp3xRewardRequiredUnOwnDia;
		m_nSafeRevivalWaitingTime = ankouTomb.safeRevivalWaitingTime;
		m_nLocationId = ankouTomb.locationId;
		m_flX = ankouTomb.x;
		m_flZ = ankouTomb.z;
		m_flXSize = ankouTomb.xSize;
		m_flZSize = ankouTomb.zSize;

		m_listCsAnkouTombSchedule = new List<CsAnkouTombSchedule>();
		m_listCsAnkouTombDifficulty = new List<CsAnkouTombDifficulty>();

        m_listMyCsHeroAnkouTombBestRecord = new List<CsHeroAnkouTombBestRecord>();
        m_listServerCsHeroAnkouTombBestRecord = new List<CsHeroAnkouTombBestRecord>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsAnkouTombDifficulty GetAnkouTombDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsAnkouTombDifficulty.Count; i++)
		{
			if (m_listCsAnkouTombDifficulty[i].Difficulty == nDifficulty)
				return m_listCsAnkouTombDifficulty[i];
		}

		return null;
	}

    //---------------------------------------------------------------------------------------------------
    public CsHeroAnkouTombBestRecord GetMyHeroAnkouTombBestRecord(int nDifficulty)
    {
        for (int i = 0; i < m_listMyCsHeroAnkouTombBestRecord.Count; i++)
        {
            if (m_listMyCsHeroAnkouTombBestRecord[i].Difficulty == nDifficulty)
            {
                return m_listMyCsHeroAnkouTombBestRecord[i];
            }
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroAnkouTombBestRecord GetServerHeroAnkouTombBestRecord(int nDifficulty)
    {
        for (int i = 0; i < m_listServerCsHeroAnkouTombBestRecord.Count; i++)
        {
            if (m_listServerCsHeroAnkouTombBestRecord[i].Difficulty == nDifficulty)
            {
                return m_listServerCsHeroAnkouTombBestRecord[i];
            }
        }

        return null;
    }
}
