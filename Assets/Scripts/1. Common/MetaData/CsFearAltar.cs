using System.Collections.Generic;
using WebCommon;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-07)
//---------------------------------------------------------------------------------------------------

public class CsFearAltar
{
	string m_strName;
	string m_strDescription;
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
	int m_nSafeRevivalWaitingTime;
	int m_nHalidomMonsterLifetime;
	string m_strHalidomMonsterSpawnText;
	int m_nHalidomDisplayDuration;
	int m_nHalidomAcquisitionRate;
	int m_nDailyFearAltarPlayCount;
	DateTime m_dtPlayDate;
	
	List<CsFearAltarReward> m_listCsFearAltarReward;
	List<CsFearAltarHalidomCollectionReward> m_listCsFearAltarHalidomCollectionReward;
	List<CsFearAltarHalidom> m_listCsFearAltarHalidom;
	List<CsFearAltarStage> m_listCsFearAltarStage;
	
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

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public int HalidomMonsterLifetime
	{
		get { return m_nHalidomMonsterLifetime; }
	}

	public string HalidomMonsterSpawnText
	{
		get { return m_strHalidomMonsterSpawnText; }
	}

	public int HalidomDisplayDuration
	{
		get { return m_nHalidomDisplayDuration; }
	}

	public int HalidomAcquisitionRate
	{
		get { return m_nHalidomAcquisitionRate; }
	}

	public List<CsFearAltarReward> FearAltarRewardList
	{
		get { return m_listCsFearAltarReward; }
	}

	public List<CsFearAltarHalidomCollectionReward> FearAltarHalidomCollectionRewardList
	{
		get { return m_listCsFearAltarHalidomCollectionReward; }
	}

	public List<CsFearAltarHalidom> FearAltarHalidomList
	{
		get { return m_listCsFearAltarHalidom; }
	}

	public List<CsFearAltarStage> FearAltarStageList
	{
		get { return m_listCsFearAltarStage; }
	}

	public int DailyFearAltarPlayCount
	{
		get { return m_nDailyFearAltarPlayCount; }
		set { m_nDailyFearAltarPlayCount = value; }
	}

	public DateTime PlayDate
	{
		get { return m_dtPlayDate; }
		set { m_dtPlayDate = value; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public CsFearAltar(WPDFearAltar fearAltar)
	{
		m_strName = CsConfiguration.Instance.GetString(fearAltar.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(fearAltar.descriptionKey);
		m_nRequiredConditionType = fearAltar.requiredConditionType;
		m_nRequiredMainQuestNo = fearAltar.requiredMainQuestNo;
		m_nRequiredHeroLevel = fearAltar.requiredHeroLevel;
		m_nRequiredStamina = fearAltar.requiredStamina;
		m_nEnterMinMemberCount = fearAltar.enterMinMemberCount;
		m_nEnterMaxMemberCount = fearAltar.enterMaxMemberCount;
		m_nMatchingWaitingTime = fearAltar.matchingWaitingTime;
		m_nEnterWaitingTime = fearAltar.enterWaitingTime;
		m_nStartDelayTime = fearAltar.startDelayTime;
		m_nLimitTime = fearAltar.limitTime;
		m_nExitDelayTime = fearAltar.exitDelayTime;
		m_nSafeRevivalWaitingTime = fearAltar.safeRevivalWaitingTime;
		m_nHalidomMonsterLifetime = fearAltar.halidomMonsterLifetime;
		m_strHalidomMonsterSpawnText = CsConfiguration.Instance.GetString(fearAltar.halidomMonsterSpawnTextKey);
		m_nHalidomDisplayDuration = fearAltar.halidomDisplayDuration;
		m_nHalidomAcquisitionRate = fearAltar.halidomAcquisitionRate;

		m_listCsFearAltarReward = new List<CsFearAltarReward>();
		m_listCsFearAltarHalidomCollectionReward = new List<CsFearAltarHalidomCollectionReward>();
		m_listCsFearAltarHalidom = new List<CsFearAltarHalidom>();
		m_listCsFearAltarStage = new List<CsFearAltarStage>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarStage GetFearAltarStage(int nStageId)
	{
		for (int i = 0; i < m_listCsFearAltarStage.Count; i++)
		{
			if (m_listCsFearAltarStage[i].StageId == nStageId)
				return m_listCsFearAltarStage[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarReward GetFearAltarReward()
	{
		int nLevel = CsGameData.Instance.MyHeroInfo.Level;

		for (int i = 0; i < m_listCsFearAltarReward.Count; i++)
		{
			if (m_listCsFearAltarReward[i].Level == nLevel)
				return m_listCsFearAltarReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsFearAltarHalidom GetFearAltarHalidom(int nHalidomId)
	{
		for (int i = 0; i < m_listCsFearAltarHalidom.Count; i++)
		{
			if (m_listCsFearAltarHalidom[i].HalidomId == nHalidomId)
			{
				return m_listCsFearAltarHalidom[i];
			}
		}

		return null;
	}
}
