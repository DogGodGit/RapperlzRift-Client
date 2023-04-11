using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiographyQuestDungeon
{
	int m_nDungeonId;
	string m_strName;
	string m_strDescription;
	string m_strSceneName;
	int m_nStartDelayTime;
	int m_nLimitTime;
	int m_nExitDelayTime;
	float m_flStartXPosition;
	float m_flStartYPosition;
	float m_flStartZPosition;
	float m_flStartRadius;
	float m_flStartYRotation;
	int m_nSafeRevivalWaitingTime;
	CsLocation m_csLocation;
	float m_flX;
	float m_flZ;
	float m_flXSize;
	float m_flZSize;

	List<CsBiographyQuestDungeonWave> m_listCsBiographyQuestDungeonWave;

	//---------------------------------------------------------------------------------------------------
	public int DungeonId
	{
		get { return m_nDungeonId; }
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

	public float StartYRotation
	{
		get { return m_flStartYRotation; }
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

	public List<CsBiographyQuestDungeonWave> BiographyQuestDungeonWaveList
	{
		get { return m_listCsBiographyQuestDungeonWave; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuestDungeon(WPDBiographyQuestDungeon biographyQuestDungeon)
	{
		m_nDungeonId = biographyQuestDungeon.dungeonId;
		m_strName = CsConfiguration.Instance.GetString(biographyQuestDungeon.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(biographyQuestDungeon.descriptionKey);
		m_strSceneName = biographyQuestDungeon.sceneName;
		m_nStartDelayTime = biographyQuestDungeon.startDelayTime;
		m_nLimitTime = biographyQuestDungeon.limitTime;
		m_nExitDelayTime = biographyQuestDungeon.exitDelayTime;
		m_flStartXPosition = biographyQuestDungeon.startXPosition;
		m_flStartYPosition = biographyQuestDungeon.startYPosition;
		m_flStartZPosition = biographyQuestDungeon.startZPosition;
		m_flStartRadius = biographyQuestDungeon.startRadius;
		m_flStartYRotation = biographyQuestDungeon.startYRotation;
		m_nSafeRevivalWaitingTime = biographyQuestDungeon.safeRevivalWaitingTime;
		m_csLocation = CsGameData.Instance.GetLocation(biographyQuestDungeon.locationId);
		m_flX = biographyQuestDungeon.x;
		m_flZ = biographyQuestDungeon.z;
		m_flXSize = biographyQuestDungeon.xSize;
		m_flZSize = biographyQuestDungeon.zSize;

		m_listCsBiographyQuestDungeonWave = new List<CsBiographyQuestDungeonWave>();
	}

	public CsBiographyQuestDungeonWave GetBiographyQuestDungeonWave(int nWaveNo)
	{
		for (int i = 0; i < m_listCsBiographyQuestDungeonWave.Count; i++)
		{
			if (m_listCsBiographyQuestDungeonWave[i].WaveNo == nWaveNo)
			{
				return m_listCsBiographyQuestDungeonWave[i];
			}
		}
		return null;
	}
}
