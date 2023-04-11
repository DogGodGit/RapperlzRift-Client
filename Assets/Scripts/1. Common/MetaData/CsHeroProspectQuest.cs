using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-03)
//---------------------------------------------------------------------------------------------------

public class CsHeroProspectQuest
{
	Guid m_guidInstanceId;

	Guid m_guidOwnerId;
	string m_strOwnerName;
	CsJob m_csJobOwner;
	int m_nOwnerLevel;

	Guid m_guidTargetId;
	string m_strTargetName;
	CsJob m_csJobTarget;
	int m_nTargetLevel;

	CsBlessingTargetLevel m_csBlessingTargetLevel;

	float m_flRemainingTime;
	bool m_bIsCompleted;

	//---------------------------------------------------------------------------------------------------
	public Guid InstanceId
	{
		get { return m_guidInstanceId; }
	}
	
	public Guid OwnerId
	{
		get { return m_guidOwnerId; }
	}

	public string OwnerName
	{
		get { return m_strOwnerName; }
	}

	public CsJob OwnerJob
	{
		get { return m_csJobOwner; }
	}

	public int OwnerLevel
	{
		get { return m_nOwnerLevel; }
	}

	public Guid TargetId
	{
		get { return m_guidTargetId; }
	}

	public string TargetName
	{
		get { return m_strTargetName; }
	}

	public CsJob TargetJob
	{
		get { return m_csJobTarget; }
	}

	public int TargetLevel
	{
		get { return m_nTargetLevel; }
		set { m_nTargetLevel = value; }
	}

	public CsBlessingTargetLevel BlessingTargetLevel
	{
		get { return m_csBlessingTargetLevel; }
	}

	public float RemainingTime
	{
		get { return m_flRemainingTime - Time.realtimeSinceStartup; }
		set { m_flRemainingTime = value + Time.realtimeSinceStartup; }
	}

	public bool IsCompleted
	{
		get { return m_bIsCompleted; }
		set { m_bIsCompleted = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroProspectQuest(PDHeroProspectQuest heroProspectQuest)
	{
		m_guidInstanceId = heroProspectQuest.instanceId;

		m_guidOwnerId = heroProspectQuest.ownerId;
		m_strOwnerName = heroProspectQuest.ownerName;
		m_csJobOwner = CsGameData.Instance.GetJob(heroProspectQuest.ownerJobId);
		m_nOwnerLevel = heroProspectQuest.ownerLevel;

		m_guidTargetId = heroProspectQuest.targetId;
		m_strTargetName = heroProspectQuest.targetName;
		m_csJobTarget = CsGameData.Instance.GetJob(heroProspectQuest.targetJobId);
		m_nTargetLevel = heroProspectQuest.targetLevel;

		m_csBlessingTargetLevel = CsGameData.Instance.GetBlessingTargetLevel(heroProspectQuest.blessingTargetLevelId);

		m_flRemainingTime = heroProspectQuest.remainingTime + Time.realtimeSinceStartup;
		m_bIsCompleted = heroProspectQuest.isCompleted;
	}
}
