using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-22)
//---------------------------------------------------------------------------------------------------

public class CsExpDungeon
{
	string m_strName;
	string m_strDescription;
	int m_nRequiredStamina;
	int m_nLimitTime;
	string m_strSceneName;
	Vector3 vtm_flStartPosition;
	float m_flStartYRotation;
	int m_nStartDelayTime;
	int m_nExitDelayTime;
	int m_nWaveIntervalTime;
	int m_nSafeRevivalWaitingTime;
	float m_flSweepExpRewardFactor;
	string m_strGuideImageName;
	string m_strGuideTitle;
	string m_strStartGuideContent;
	string m_strLakChargeMonsterAppearContent;
	string m_strLakChargeMonsterKillContent;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsExpDungeonDifficulty> m_listCsExpDungeonDifficulty;

	DateTime m_dtDateExpDungeonPlayCount;
	int m_nExpDungeonDailyPlayCount = 0;
	List<int> m_listExpDungeonClearedDifficulty = new List<int>();

	//---------------------------------------------------------------------------------------------------
	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
	}

	public int LimitTime
	{
		get { return m_nLimitTime; }
	}

	public string SceneName
	{
		get { return m_strSceneName; }
	}

	public Vector3 StartPosition
	{
		get { return vtm_flStartPosition; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public int StartDelayTime
	{
		get { return m_nStartDelayTime; }
	}

	public int ExitDelayTime
	{
		get { return m_nExitDelayTime; }
	}

	public int WaveIntervalTime
	{
		get { return m_nWaveIntervalTime; }
	}

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public float SweepExpRewardFactor
	{
		get { return m_flSweepExpRewardFactor; }
	}

	public string GuideImageName
	{
		get { return m_strGuideImageName; }
	}

	public string GuideTitle
	{
		get { return m_strGuideTitle; }
	}

	public string StartGuideContent
	{
		get { return m_strStartGuideContent; }
	}

	public string LakChargeMonsterAppearContent
	{
		get { return m_strLakChargeMonsterAppearContent; }
	}

	public string LakChargeMonsterKillContent
	{
		get { return m_strLakChargeMonsterKillContent; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public List<CsExpDungeonDifficulty> ExpDungeonDifficultyList
	{
		get { return m_listCsExpDungeonDifficulty; }
	}

	public DateTime ExpDungeonPlayCountDate
	{
		get { return m_dtDateExpDungeonPlayCount; }
		set { m_dtDateExpDungeonPlayCount = value; }
	}

	public int ExpDungeonDailyPlayCount
	{
		get { return m_nExpDungeonDailyPlayCount; }
		set { m_nExpDungeonDailyPlayCount = value; }
	}

	public List<int> ExpDungeonClearedDifficultyList
	{
		get { return m_listExpDungeonClearedDifficulty; }
		set { m_listExpDungeonClearedDifficulty = value; }
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

	//---------------------------------------------------------------------------------------------------
	public CsExpDungeon(WPDExpDungeon expDungeon)
	{
		m_strName = CsConfiguration.Instance.GetString(expDungeon.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(expDungeon.descriptionKey);
		m_nRequiredStamina= expDungeon.requiredStamina;
		m_nLimitTime = expDungeon.limitTime;
		m_strSceneName = expDungeon.sceneName;
		vtm_flStartPosition = new Vector3((float)expDungeon.startXPosition, (float)expDungeon.startXPosition, (float)expDungeon.startXPosition);
		m_flStartYRotation = expDungeon.startYRotation;
		m_nStartDelayTime = expDungeon.startDelayTime;
		m_nExitDelayTime = expDungeon.exitDelayTime;
		m_nWaveIntervalTime = expDungeon.waveIntervalTime;
		m_nSafeRevivalWaitingTime = expDungeon.safeRevivalWaitingTime;
		m_flSweepExpRewardFactor = expDungeon.sweepExpRewardFactor;
		m_strGuideImageName = expDungeon.guideImageName;
		m_strGuideTitle = CsConfiguration.Instance.GetString(expDungeon.guideTitleKey);
		m_strStartGuideContent = CsConfiguration.Instance.GetString(expDungeon.startGuideContentKey);
		m_strLakChargeMonsterAppearContent = CsConfiguration.Instance.GetString(expDungeon.lakChargeMonsterAppearContentKey);
		m_strLakChargeMonsterKillContent = CsConfiguration.Instance.GetString(expDungeon.lakChargeMonsterKillContentKey);
		m_nLocationId = expDungeon.locationId;
		m_flX = expDungeon.x;
		m_flZ = expDungeon.z;
		m_flXSize = expDungeon.xSize;
		m_flZSize = expDungeon.zSize;

		m_listCsExpDungeonDifficulty = new List<CsExpDungeonDifficulty>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsExpDungeonDifficulty GetExpDungeonDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsExpDungeonDifficulty.Count; i++)
		{
			if (m_listCsExpDungeonDifficulty[i].Difficulty == nDifficulty)
				return m_listCsExpDungeonDifficulty[i];
		}

		return null;
	}
}
