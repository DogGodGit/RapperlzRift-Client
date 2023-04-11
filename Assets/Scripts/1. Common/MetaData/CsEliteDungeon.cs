using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-23)
//---------------------------------------------------------------------------------------------------

public class CsEliteDungeon
{
	string m_strName;
	string m_strDescription;
	string m_strTargetTitle;
	string m_strTargetContent;
	int m_nBaseEnterCount;
	int m_nEnterCountAddInterval;
	int m_nRequiredStamina;
	string m_strSceneName;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	int m_nSafeRevivalWaitingTime;
	Vector3 m_vtStartPosition;
	float m_flStartYRotation;
	Vector3 m_vtMonsterPosition;
	float m_flMonsterYRotation;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

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

	public int BaseEnterCount
	{
		get { return m_nBaseEnterCount; }
	}

	public int EnterCountAddInterval
	{
		get { return m_nEnterCountAddInterval; }
	}

	public int RequiredStamina
	{
		get { return m_nRequiredStamina; }
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

	public int SafeRevivalWaitingTime
	{
		get { return m_nSafeRevivalWaitingTime; }
	}

	public Vector3 StartPosition
	{
		get { return m_vtStartPosition; }
	}

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
	}

	public Vector3 MonsterPosition
	{
		get { return m_vtMonsterPosition; }
	}

	public float MonsterYRotation
	{
		get { return m_flMonsterYRotation; }
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

	//---------------------------------------------------------------------------------------------------
	public CsEliteDungeon(WPDEliteDungeon eliteDungeon)
	{
		m_strName = CsConfiguration.Instance.GetString(eliteDungeon.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(eliteDungeon.descriptionKey);
		m_strTargetTitle = CsConfiguration.Instance.GetString(eliteDungeon.targetTitleKey);
		m_strTargetContent = CsConfiguration.Instance.GetString(eliteDungeon.targetContentKey);
		m_nBaseEnterCount = eliteDungeon.baseEnterCount;
		m_nEnterCountAddInterval = eliteDungeon.enterCountAddInterval;
		m_nRequiredStamina = eliteDungeon.requiredStamina;
		m_strSceneName = eliteDungeon.sceneName;
		m_nStartDelayTime = eliteDungeon.startDelayTime;
		m_nLimitTime = eliteDungeon.limitTime;
		m_nExitDelayTime = eliteDungeon.exitDelayTime;
		m_nSafeRevivalWaitingTime = eliteDungeon.safeRevivalWaitingTime;
		m_vtStartPosition = new Vector3(eliteDungeon.startXPosition, eliteDungeon.startYPosition, eliteDungeon.startZPosition);
		m_flStartYRotation = eliteDungeon.startYRotation;
		m_vtMonsterPosition = new Vector3(eliteDungeon.monsterXPosition, eliteDungeon.monsterYPosition, eliteDungeon.monsterZPosition);
		m_flMonsterYRotation = eliteDungeon.monsterYRotation;
		m_csLocation = CsGameData.Instance.GetLocation(eliteDungeon.locationId);
		m_flX = eliteDungeon.x;
		m_flZ = eliteDungeon.z;
		m_flXSize = eliteDungeon.xSize;
		m_flZSize = eliteDungeon.zSize;
	}
}
