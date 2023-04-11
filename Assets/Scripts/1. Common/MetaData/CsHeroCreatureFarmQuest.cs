using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsHeroCreatureFarmQuest
{
	Guid m_guidInstanceId;
	int m_nMissionNo;
	CsCreatureFarmQuestMission m_csCreatureFarmQuestMission;
	int m_nProgressCount;

	long m_lMonsterInstanceId;
	Vector3 m_v3MonsterPosition;
	float m_flRemainingMonsterLifetime;

	//---------------------------------------------------------------------------------------------------
	public Guid InstanceId
	{
		get { return m_guidInstanceId; }
	}

	public CsCreatureFarmQuestMission CreatureFarmQuestMission
	{
		get { return m_csCreatureFarmQuestMission; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}
	
	public long MonsterInstanceId
	{
		get { return m_lMonsterInstanceId; }
		set { m_lMonsterInstanceId = value; }
	}

	public Vector3 MonsterPosition
	{
		get { return m_v3MonsterPosition; }
		set { m_v3MonsterPosition = value; }
	}

	public float RemainingMonsterLifetime
	{
		get { return m_flRemainingMonsterLifetime - Time.realtimeSinceStartup; }
		set { m_flRemainingMonsterLifetime = value + Time.realtimeSinceStartup; }
	}

	public int MissionNo
	{
		get { return m_nMissionNo; }
		set
		{
			m_nMissionNo = value;
			m_csCreatureFarmQuestMission = CsGameData.Instance.CreatureFarmQuest.GetCreatureFarmQuestMission(m_nMissionNo);
			m_nProgressCount = 0;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreatureFarmQuest(PDHeroCreatureFarmQuest heroCreatureFarmQuest)
	{
		m_guidInstanceId = heroCreatureFarmQuest.instanceId;
		m_nMissionNo = heroCreatureFarmQuest.missionNo;
		m_csCreatureFarmQuestMission = CsGameData.Instance.CreatureFarmQuest.GetCreatureFarmQuestMission(heroCreatureFarmQuest.missionNo);
		m_nProgressCount = heroCreatureFarmQuest.progressCount;

		m_lMonsterInstanceId = heroCreatureFarmQuest.monsterInstanceId;
		m_v3MonsterPosition = new Vector3(heroCreatureFarmQuest.monsterPosition.x, heroCreatureFarmQuest.monsterPosition.y, heroCreatureFarmQuest.monsterPosition.z);
		m_flRemainingMonsterLifetime = heroCreatureFarmQuest.remainingMonsterLifetime + Time.realtimeSinceStartup;
	}
}
