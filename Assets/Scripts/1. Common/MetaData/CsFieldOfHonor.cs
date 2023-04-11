using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-19)
//---------------------------------------------------------------------------------------------------

public class CsFieldOfHonor
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strSceneName;
	int m_nRequiredConditionType;
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
    Vector3 m_vtStartPosition;
	float m_flStartYRotation;
    Vector3 m_vtTargetPosition;
	float m_flTargetYRotation;
	CsHonorPointReward m_csHonorPointRewardWinner;
	CsHonorPointReward m_csHonorPointRewardLoser;
	int m_lLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

    DateTime m_dtDateFieldOfHonorPlayCount;
    int m_nFieldOfHonorDailyPlayCount = 0;
    int m_nMyRanking = 0;
    int m_nSuccessiveCount = 0;
    int m_nRewardedDailyFieldOfHonorRankingNo = 0;
    int m_nDailyFieldOfHonorRankingNo = 0;
    int m_nDailyfieldOfHonorRanking = 0;

	List<CsFieldOfHonorRankingReward> m_listCsFieldOfHonorRankingReward;

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

    public Vector3 StartPosition
    {
        get { return m_vtStartPosition; }
    }

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

    public Vector3 TargetPosition
    {
        get { return m_vtTargetPosition; }
    }

	public float TargetYRotation
	{
		get { return m_flTargetYRotation; }
	}

	public CsHonorPointReward WinnerHonorPointReward
	{
		get { return m_csHonorPointRewardWinner; }
	}

	public CsHonorPointReward LoserHonorPointReward
	{
		get { return m_csHonorPointRewardLoser; }
	}

	public int LocationId
	{
		get { return m_lLocationId; }
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

    public DateTime FieldOfHonorPlayCountDate
	{
		get { return m_dtDateFieldOfHonorPlayCount; }
        set { m_dtDateFieldOfHonorPlayCount = value; }
	}

    public int FieldOfHonorDailyPlayCount
	{
		get { return m_nFieldOfHonorDailyPlayCount; }
        set { m_nFieldOfHonorDailyPlayCount = value; }
	}

    public int MyRanking
	{
        get { return m_nMyRanking; }
        set { m_nMyRanking = value; }
	}

    public int SuccessiveCount
    {
        get { return m_nSuccessiveCount; }
        set { m_nSuccessiveCount = value; }
    }

    public int RewardedDailyFieldOfHonorRankingNo
    {
        get { return m_nRewardedDailyFieldOfHonorRankingNo; }
        set { m_nRewardedDailyFieldOfHonorRankingNo = value; }
    }

    public int DailyFieldOfHonorRankingNo
    {
        get { return m_nDailyFieldOfHonorRankingNo; }
        set { m_nDailyFieldOfHonorRankingNo = value; }
    }

    public int DailyfieldOfHonorRanking
    {
        get { return m_nDailyfieldOfHonorRanking; }
        set { m_nDailyfieldOfHonorRanking = value; }
    }    

	public List<CsFieldOfHonorRankingReward> FieldOfHonorRankingRewardList
	{
		get { return m_listCsFieldOfHonorRankingReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsFieldOfHonor(WPDFieldOfHonor fieldOfHonor)
	{
		m_strName = CsConfiguration.Instance.GetString(fieldOfHonor.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(fieldOfHonor.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(fieldOfHonor.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(fieldOfHonor.targetContentKey);
		m_strSceneName = fieldOfHonor.sceneName;
		m_nRequiredConditionType = fieldOfHonor.requiredConditionType;
		m_nRequiredMainQuestNo = fieldOfHonor.requiredMainQuestNo;
		m_nRequiredHeroLevel = fieldOfHonor.requiredHeroLevel;
		m_nStartDelayTime = fieldOfHonor.startDelayTime;
		m_nLimitTime = fieldOfHonor.limitTime;
		m_nExitDelayTime = fieldOfHonor.exitDelayTime;
        m_vtStartPosition = new Vector3(fieldOfHonor.startXPosition, fieldOfHonor.startYPosition, fieldOfHonor.startZPosition);
		m_flStartYRotation = fieldOfHonor.startYRotation;
        m_vtTargetPosition = new Vector3(fieldOfHonor.targetXPosition, fieldOfHonor.targetYPosition, fieldOfHonor.targetZPosition);
		m_flTargetYRotation = fieldOfHonor.targetYRotation;
		m_csHonorPointRewardWinner = CsGameData.Instance.GetHonorPointReward(fieldOfHonor.winnerHonorPointRewardId);
		m_csHonorPointRewardLoser = CsGameData.Instance.GetHonorPointReward(fieldOfHonor.loserHonorPointRewardId);
		m_lLocationId = fieldOfHonor.locationId;
		m_flX = fieldOfHonor.x;
		m_flZ = fieldOfHonor.z;
		m_flXSize = fieldOfHonor.xSize;
		m_flZSize = fieldOfHonor.zSize;

		m_listCsFieldOfHonorRankingReward = new List<CsFieldOfHonorRankingReward>();
	}
}
