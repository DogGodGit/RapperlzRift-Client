using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsDeadRecord
{
	Guid m_guidId;
	Guid m_guidKillerId;
	string m_strName;
	CsNation m_csNation;
	CsJob m_csJob;
	int m_nLevel;
	long m_lBattlePower;
	float m_flRegElapsedTime;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guidId; }
	}

	public Guid KillerId
	{
		get { return m_guidKillerId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public CsNation Nation
	{
		get { return m_csNation; }
	}

	public CsJob Job
	{
		get { return m_csJob; }
	}

	public int Level
	{
		get { return m_nLevel; }
		set { m_nLevel = value; }
	}

	public long BattlePower
	{
		get { return m_lBattlePower; }
		set { m_lBattlePower = value; }
	}

	public float RegElapsedTime
	{
		get { return m_flRegElapsedTime; }
		set { m_flRegElapsedTime = value + Time.realtimeSinceStartup; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsDeadRecord(PDDeadRecord deadRecord)
	{
		m_guidId = deadRecord.id;
		m_guidKillerId = deadRecord.killerId;
		m_strName = deadRecord.name;
		m_csNation = CsGameData.Instance.GetNation(deadRecord.nationId);
		m_csJob = CsGameData.Instance.GetJob(deadRecord.jobId);
		m_nLevel = deadRecord.level;
		m_lBattlePower = deadRecord.battlePower;
		m_flRegElapsedTime = deadRecord.regElapsedTime + Time.realtimeSinceStartup;
	}
}
