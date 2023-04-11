using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-18)
//---------------------------------------------------------------------------------------------------
public enum EnJobChangeQuestStaus
{
	None = -1,
	Accepted = 0,
	Completed = 1,
	Failed = 2,
}

public class CsHeroJobChangeQuest
{
	Guid m_guidInstanceId;
	int m_nQuestNo;
	int m_nProgressCount;
	int m_nDifficulty;
	float m_flRemainingTime;
	int m_nStatus;
	long m_lMonsterInstanceId;
	Vector3 m_v3MonsterPosition;

	//---------------------------------------------------------------------------------------------------
	public Guid InstanceId
	{
		get { return m_guidInstanceId; }
		set { m_guidInstanceId = value; }
	}

	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
		set { m_nProgressCount = value; }
	}

	public int Difficulty
	{
		get { return m_nDifficulty; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
		set { m_flRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public int Status
	{
		get { return m_nStatus; }
		set { m_nStatus = value; }
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

	//---------------------------------------------------------------------------------------------------
	public CsHeroJobChangeQuest(PDHeroJobChangeQuest heroJobChangeQuest)
	{
		m_guidInstanceId = heroJobChangeQuest.instanceId;
		m_nQuestNo = heroJobChangeQuest.questNo;
		m_nProgressCount = heroJobChangeQuest.progressCount;
		m_nDifficulty = heroJobChangeQuest.difficulty;
		m_flRemainingTime = heroJobChangeQuest.remainingTime + Time.realtimeSinceStartup;
		m_nStatus = heroJobChangeQuest.status;
		m_lMonsterInstanceId = heroJobChangeQuest.monsterInstanceId;
		m_v3MonsterPosition = new Vector3(heroJobChangeQuest.monsterPosition.x, heroJobChangeQuest.monsterPosition.y, heroJobChangeQuest.monsterPosition.z);
	}
}
