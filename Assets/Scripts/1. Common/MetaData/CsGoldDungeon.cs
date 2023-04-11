using System;
using System.Collections.Generic;
using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsGoldDungeon
{
	string m_strName;
	string m_strDescription;
	int m_strRequiredStamina;
	int m_nLimitTime;
	string m_strSceneName;
	Vector3 m_vtStartPosition;
	float m_flStartYRotation;
	int m_nStartDelayTime;
	int m_nExitDelayTime;
	int m_nSafeRevivalWaitingTime;
	Vector3 m_vtMonsterEscapePosition;
	float m_flMonsterEscapeRadius;
	int m_nLocationId;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsGoldDungeonDifficulty> m_listCsGoldDungeonDifficulty;

	DateTime m_dtDateGoldDungeonPlayCount;
	int m_nGoldDungeonDailyPlayCount = 0;
	List<int> m_listGoldDungeonClearedDifficulty = new List<int>();

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
		get { return m_strRequiredStamina; }
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
		get { return m_vtStartPosition; }
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

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public Vector3 MonsterEscapePosition
	{
		get { return m_vtMonsterEscapePosition; }
	}

	public float MonsterEscapeRadius
	{
		get { return m_flMonsterEscapeRadius; }
	}

	public int LocationId
	{
		get { return m_nLocationId; }
	}

	public List<CsGoldDungeonDifficulty> GoldDungeonDifficultyList
	{
		get { return m_listCsGoldDungeonDifficulty; }
	}

	public DateTime GoldDungeonPlayCountDate
	{
		get { return m_dtDateGoldDungeonPlayCount; }
		set { m_dtDateGoldDungeonPlayCount = value; }
	}

	public int GoldDungeonDailyPlayCount
	{
		get { return m_nGoldDungeonDailyPlayCount; }
		set { m_nGoldDungeonDailyPlayCount = value; }
	}

	public List<int> GoldDungeonClearedDifficultyList
	{
		get { return m_listGoldDungeonClearedDifficulty; }
		set { m_listGoldDungeonClearedDifficulty = value; }
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
	public CsGoldDungeon(WPDGoldDungeon goldDungeon)
	{
		m_strName = CsConfiguration.Instance.GetString(goldDungeon.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(goldDungeon.descriptionKey);
		m_strRequiredStamina = goldDungeon.requiredStamina;
		m_nLimitTime = goldDungeon.limitTime;
		m_strSceneName = goldDungeon.sceneName;
		m_vtStartPosition = new Vector3(goldDungeon.startXPosition, goldDungeon.startYPosition, goldDungeon.startZPosition);
		m_flStartYRotation = goldDungeon.startYRotation;
		m_nStartDelayTime = goldDungeon.startDelayTime;
		m_nExitDelayTime = goldDungeon.exitDelayTime;
		m_nSafeRevivalWaitingTime = goldDungeon.safeRevivalWaitingTime;
		m_vtMonsterEscapePosition = new Vector3(goldDungeon.monsterEscapeXPosition, goldDungeon.monsterEscapeYPosition, goldDungeon.monsterEscapeZPosition);
		m_flMonsterEscapeRadius = goldDungeon.monsterEscapeRadius;
		m_nLocationId = goldDungeon.locationId;
		m_flX = goldDungeon.x;
		m_flZ = goldDungeon.z;
		m_flXSize = goldDungeon.xSize;
		m_flZSize = goldDungeon.zSize;

		m_listCsGoldDungeonDifficulty = new List<CsGoldDungeonDifficulty>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsGoldDungeonDifficulty GetGoldDungeonDifficulty(int nDifficulty)
	{
		for (int i = 0; i < m_listCsGoldDungeonDifficulty.Count; i++)
		{
			if (m_listCsGoldDungeonDifficulty[i].Difficulty == nDifficulty)
				return m_listCsGoldDungeonDifficulty[i];
		}

		return null;
	}
}
