using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-05-03)
//---------------------------------------------------------------------------------------------------

public class CsProofOfValor
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredConditionType;   // 1:영웅레벨,2:메인퀘스트번호
	int m_nRequiredMainQuestNo;
	int m_nRequiredHeroLevel;
	int m_nRequiredStamina;
	int m_nDailyFreeRefreshCount;
	int m_nDailyPaidRefreshCount;
	string m_strSceneName;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	Vector3 m_vtStartPosition;
	string m_strTargetTitle;
	string m_strTargetContent;
	string m_strGuideImageName;
	string m_strStartGuideTitle;
    string m_strStartGuideContent;
    string m_strBuffBoxCreationGuideTitle;
    string m_strBuffBoxCreationGuideContent;
    int m_nBuffBoxCreationTime;
    int m_nBuffBoxCreationInterval;
    int m_nBuffBoxLifetime;
    int m_nBuffDuration;
    int m_nFailureRewardSoulPowder;
    CsLocation m_csLocation;
    float m_flX;
    float m_flZ;
    float m_flXSize;
    float m_flZSize;

    List<CsProofOfValorBuffBox> m_listCsProofOfValorBuffBox;
    List<CsProofOfValorBossMonsterArrange> m_listCsProofOfValorBossMonsterArrange;
    List<CsProofOfValorPaidRefresh> m_listCsProofOfValorPaidRefresh;
    List<CsProofOfValorRefreshSchedule> m_listCsProofOfValorRefreshSchedule;
    List<CsProofOfValorReward> m_listCsProofOfValorReward;
	List<CsProofOfValorClearGrade> m_listCsProofOfValorClearGrade;

	bool m_bProofOfValorCleared;
	int m_nMyDailyFreeRefreshCount;
	int m_nMyDailyPaidRefreshCount;
	int m_nClearGrade;
	int m_nDailyPlayCount;
	int m_nPaidRefreshCount;
	int m_nBossMonsterArrangeId;
	int m_nCreatureCardId;
	DateTime m_dtDailyPlayCountDate;

    //---------------------------------------------------------------------------------------------------
    public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
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

	public int DailyFreeRefreshCount
	{
		get { return m_nDailyFreeRefreshCount; }
	}

	public int DailyPaidRefreshCount
	{
		get { return m_nDailyPaidRefreshCount; }
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

	public Vector3 StartPosition
	{
		get { return m_vtStartPosition; }
	}

	public string TargetTitle
	{
		get { return m_strTargetTitle; }
	}

	public string TargetContent
	{
		get { return m_strTargetContent; }
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

	public string BuffBoxCreationGuideTitle
	{
		get { return m_strBuffBoxCreationGuideTitle; }
	}

	public string BuffBoxCreationGuideContent
	{
		get { return m_strBuffBoxCreationGuideContent; }
	}

	public int BuffBoxCreationTime
	{
		get { return m_nBuffBoxCreationTime; }
	}

	public int BuffBoxCreationInterval
	{
		get { return m_nBuffBoxCreationInterval; }
	}

	public int BuffBoxLifetime
	{
		get { return m_nBuffBoxLifetime; }
	}

	public int BuffDuration
	{
		get { return m_nBuffDuration; }
	}

	public int FailureRewardSoulPowder
	{
		get { return m_nFailureRewardSoulPowder; }
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


    public List<CsProofOfValorBuffBox> ProofOfValorBuffBoxList
	{
		get { return m_listCsProofOfValorBuffBox; }
	}

	public List<CsProofOfValorBossMonsterArrange> ProofOfValorBossMonsterArrangeList
	{
		get { return m_listCsProofOfValorBossMonsterArrange; }
	}

	public List<CsProofOfValorPaidRefresh> ProofOfValorPaidRefreshList
	{
		get { return m_listCsProofOfValorPaidRefresh; }
	}

	public List<CsProofOfValorRefreshSchedule> ProofOfValorRefreshScheduleList
	{
		get { return m_listCsProofOfValorRefreshSchedule; }
	}

	public List<CsProofOfValorReward> ProofOfValorRewardList
	{
		get { return m_listCsProofOfValorReward; }
	}

	public List<CsProofOfValorClearGrade> ProofOfValorClearGradeList
	{
		get { return m_listCsProofOfValorClearGrade; }
	}

	public int MyDailyFreeRefreshCount
	{
		get { return m_nMyDailyFreeRefreshCount; }
		set { m_nMyDailyFreeRefreshCount = value; }
	}

	public int MyDailyPaidRefreshCount
	{
		get { return m_nMyDailyPaidRefreshCount; }
		set { m_nMyDailyPaidRefreshCount = value; }
	}

	public int ClearGrade
	{ 
		get { return m_nClearGrade; }
		set { m_nClearGrade = value; } 
	}

	public int DailyPlayCount
	{
		get { return m_nDailyPlayCount; } 
		set { m_nDailyPlayCount = value; } 
	}

	public DateTime DailyPlayCountDate 
	{
		get { return m_dtDailyPlayCountDate; } 
		set { m_dtDailyPlayCountDate = value; } 
	}

	public int PaidRefreshCount 
	{
		get { return m_nPaidRefreshCount; } 
		set { m_nPaidRefreshCount = value; } 
	}

	public int BossMonsterArrangeId 
	{ 
		get { return m_nBossMonsterArrangeId; }
		set { m_nBossMonsterArrangeId = value; } 
	}

	public int CreatureCardId 
	{
		get { return m_nCreatureCardId; } 
		set { m_nCreatureCardId = value; } 
	}

	public bool ProofOfValorCleared
	{
		get { return m_bProofOfValorCleared; }
		set { m_bProofOfValorCleared = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValor(WPDProofOfValor proofOfValor)
	{
		m_strName = CsConfiguration.Instance.GetString(proofOfValor.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(proofOfValor.descriptionKey);
		m_nRequiredConditionType = proofOfValor.requiredConditionType;
		m_nRequiredMainQuestNo = proofOfValor.requiredMainQuestNo;
		m_nRequiredHeroLevel = proofOfValor.requiredHeroLevel;
		m_nRequiredStamina = proofOfValor.requiredStamina;
		m_nDailyFreeRefreshCount = proofOfValor.dailyFreeRefreshCount;
		m_nDailyPaidRefreshCount = proofOfValor.dailyPaidRefreshCount;
		m_strSceneName = proofOfValor.sceneName;
		m_nStartDelayTime = proofOfValor.startDelayTime;
		m_nLimitTime = proofOfValor.limitTime;
		m_nExitDelayTime = proofOfValor.exitDelayTime;
		m_vtStartPosition = new Vector3(proofOfValor.startXPosition, proofOfValor.startYPosition, proofOfValor.startZPosition);
		m_strTargetTitle = CsConfiguration.Instance.GetString(proofOfValor.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(proofOfValor.targetContentKey);
		m_strGuideImageName = proofOfValor.guideImageName;
		m_strStartGuideTitle = CsConfiguration.Instance.GetString(proofOfValor.startGuideTitleKey);
		m_strStartGuideContent = CsConfiguration.Instance.GetString(proofOfValor.startGuideContentKey);
		m_strBuffBoxCreationGuideTitle = CsConfiguration.Instance.GetString(proofOfValor.buffBoxCreationGuideTitleKey);
		m_strBuffBoxCreationGuideContent = CsConfiguration.Instance.GetString(proofOfValor.buffBoxCreationGuideContentKey);
		m_nBuffBoxCreationTime = proofOfValor.buffBoxCreationTime;
		m_nBuffBoxCreationInterval = proofOfValor.buffBoxCreationInterval;
		m_nBuffBoxLifetime = proofOfValor.buffBoxLifetime;
		m_nBuffDuration = proofOfValor.buffDuration;
		m_nFailureRewardSoulPowder = proofOfValor.failureRewardSoulPowder;
		m_csLocation = CsGameData.Instance.GetLocation(proofOfValor.locationId);
		m_flX = proofOfValor.x;
		m_flZ = proofOfValor.z;
		m_flXSize = proofOfValor.xSize;
		m_flZSize = proofOfValor.zSize;

        m_listCsProofOfValorBuffBox = new List<CsProofOfValorBuffBox>();
		m_listCsProofOfValorBossMonsterArrange = new List<CsProofOfValorBossMonsterArrange>();
		m_listCsProofOfValorPaidRefresh = new List<CsProofOfValorPaidRefresh>();
		m_listCsProofOfValorRefreshSchedule = new List<CsProofOfValorRefreshSchedule>();
		m_listCsProofOfValorReward = new List<CsProofOfValorReward>();
		m_listCsProofOfValorClearGrade = new List<CsProofOfValorClearGrade>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsProofOfValorBuffBox GetProofOfValorBuffBox(int nBuffBoxId)
	{
		for (int i = 0; i < m_listCsProofOfValorBuffBox.Count; i++)
		{
			if (m_listCsProofOfValorBuffBox[i].BuffBoxId == nBuffBoxId)
				return m_listCsProofOfValorBuffBox[i];
		}

		return null;
	}

    //---------------------------------------------------------------------------------------------------
    public CsProofOfValorPaidRefresh GetProofOfValorPaidRefresh(int nRefreshCount)
    {
        for (int i = 0; i < m_listCsProofOfValorPaidRefresh.Count; i++)
        {
            if (m_listCsProofOfValorPaidRefresh[i].RefreshCount == nRefreshCount)
                return m_listCsProofOfValorPaidRefresh[i];
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public CsProofOfValorBossMonsterArrange GetProofOfValorBossMonsterArrange(int ProofOfValorBossMonsterArrangeId)
    {
        for (int i = 0; i < m_listCsProofOfValorBossMonsterArrange.Count; i++)
        {
            if (m_listCsProofOfValorBossMonsterArrange[i].ProofOfValorBossMonsterArrangeId == ProofOfValorBossMonsterArrangeId)
                return m_listCsProofOfValorBossMonsterArrange[i];
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public CsProofOfValorReward GetProofOfValorReward(int nLevel)
    {
        for (int i = 0; i < m_listCsProofOfValorReward.Count; i++)
        {
            if (m_listCsProofOfValorReward[i].HeroLevel == nLevel)
                return m_listCsProofOfValorReward[i];
        }

        return null;
    }
}
