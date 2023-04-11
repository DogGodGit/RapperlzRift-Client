using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsTradeShip
{
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nRequiredConditionType;
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;
	int m_nRequiredStamina;
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
	int m_nMonsterRegenTime;
	int m_nClearPointPerRemainTime;
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

	List<CsTradeShipSchedule> m_listCsTradeShipSchedule;
	List<CsTradeShipObstacle> m_listCsTradeShipObstacle;
	List<CsTradeShipStep> m_listCsTradeShipStep;
	List<CsTradeShipDifficulty> m_listCsTradeShipDifficulty;

    List<CsHeroTradeShipBestRecord> m_listMyCsHeroTradeShipBestRecord;
    List<CsHeroTradeShipBestRecord> m_listServerCsHeroTradeShipBestRecord;

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

	public int MonsterRegenTime
	{
		get { return m_nMonsterRegenTime; }
	}

	public int ClearPointPerRemainTime
	{
		get { return m_nClearPointPerRemainTime; }
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

	public List<CsTradeShipSchedule> TradeShipScheduleList
	{
		get { return m_listCsTradeShipSchedule; }
	}

	public List<CsTradeShipObstacle> TradeShipObstacleList
	{
		get { return m_listCsTradeShipObstacle; }
	}

	public List<CsTradeShipStep> TradeShipStepList
	{
		get { return m_listCsTradeShipStep; }
	}

	public List<CsTradeShipDifficulty> TradeShipDifficultyList
	{
		get { return m_listCsTradeShipDifficulty; }
	}

    public List<CsHeroTradeShipBestRecord> MyHeroTradeShipBestRecordList
    {
        get { return m_listMyCsHeroTradeShipBestRecord; }
    }

    public List<CsHeroTradeShipBestRecord> ServerHeroTradeShipBestRecordList
    {
        get { return m_listServerCsHeroTradeShipBestRecord; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsTradeShip(WPDTradeShip tradeShip)
	{
		m_strName = CsConfiguration.Instance.GetString(tradeShip.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(tradeShip.descriptionKey);
		m_strSceneName = tradeShip.sceneName;
		m_nRequiredConditionType = tradeShip.requiredConditionType;
		m_nRequiredHeroLevel = tradeShip.requiredHeroLevel;
		m_nRequiredMainQuestNo = tradeShip.requiredMainQuestNo;
		m_nRequiredStamina = tradeShip.requiredStamina;
		m_nEnterMinMemberCount = tradeShip.enterMinMemberCount;
		m_nEnterMaxMemberCount = tradeShip.enterMaxMemberCount;
		m_nMatchingWaitingTime = tradeShip.matchingWaitingTime;
		m_nEnterWaitingTime = tradeShip.enterWaitingTime;
		m_nStartDelayTime = tradeShip.startDelayTime;
		m_nLimitTime = tradeShip.limitTime;
		m_nExitDelayTime = tradeShip.exitDelayTime;
		m_flStartXPosition = tradeShip.startXPosition;
		m_flStartYPosition = tradeShip.startYPosition;
		m_flStartZPosition = tradeShip.startZPosition;
		m_flStartRadius = tradeShip.startRadius;
		m_nStartYRotationType = tradeShip.startYRotationType;
		m_flStartYRotation = tradeShip.startYRotation;
		m_nMonsterRegenTime = tradeShip.monsterRegenTime;
		m_nClearPointPerRemainTime = tradeShip.clearPointPerRemainTime;
		m_nExp2xRewardRequiredUnOwnDia = tradeShip.exp2xRewardRequiredUnOwnDia;
		m_nExp3xRewardRequiredUnOwnDia = tradeShip.exp3xRewardRequiredUnOwnDia;
		m_nSafeRevivalWaitingTime = tradeShip.safeRevivalWaitingTime;
		m_nLocationId = tradeShip.locationId;
		m_flX = tradeShip.x;
		m_flZ = tradeShip.z;
		m_flXSize = tradeShip.xSize;
		m_flZSize = tradeShip.zSize;

		m_listCsTradeShipSchedule = new List<CsTradeShipSchedule>();
		m_listCsTradeShipObstacle = new List<CsTradeShipObstacle>();
		m_listCsTradeShipStep = new List<CsTradeShipStep>();
		m_listCsTradeShipDifficulty = new List<CsTradeShipDifficulty>();

        m_listMyCsHeroTradeShipBestRecord = new List<CsHeroTradeShipBestRecord>();
        m_listServerCsHeroTradeShipBestRecord = new List<CsHeroTradeShipBestRecord>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipDifficulty GetTradeShipDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsTradeShipDifficulty.Count; i++)
		{
			if (m_listCsTradeShipDifficulty[i].Difficulty == nDifficulty)
				return m_listCsTradeShipDifficulty[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsTradeShipStep GetTradeShipStep(int nStep)
	{
		for (int i = 0; i < m_listCsTradeShipStep.Count; i++)
		{
			if (m_listCsTradeShipStep[i].StepNo == nStep)
				return m_listCsTradeShipStep[i];
		}

		return null;
	}

    //---------------------------------------------------------------------------------------------------
    public CsHeroTradeShipBestRecord GetMyHeroTradeShipBestRecord(int nDifficulty)
    {
        for (int i = 0; i < m_listMyCsHeroTradeShipBestRecord.Count; i++)
        {
            if (m_listMyCsHeroTradeShipBestRecord[i].Difficulty == nDifficulty)
            {
                return m_listMyCsHeroTradeShipBestRecord[i];
            }
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroTradeShipBestRecord GetServerHeroTradeShipBestRecord(int nDifficulty)
    {
        for (int i = 0; i < m_listServerCsHeroTradeShipBestRecord.Count; i++)
        {
            if (m_listServerCsHeroTradeShipBestRecord[i].Difficulty == nDifficulty)
            {
                return m_listServerCsHeroTradeShipBestRecord[i];
            }
        }

        return null;
    }
}
