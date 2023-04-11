using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsTempFriend
{
	Guid m_guidId;
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
	public CsTempFriend(PDTempFriend tempFriend)
	{
		m_guidId = tempFriend.id;
		m_strName = tempFriend.name;
		m_csNation = CsGameData.Instance.GetNation(tempFriend.nationId);
		m_csJob = CsGameData.Instance.GetJob(tempFriend.jobId);
		m_nLevel = tempFriend.level;
		m_lBattlePower = tempFriend.battlePower;
		m_flRegElapsedTime = tempFriend.regElapsedTime + Time.realtimeSinceStartup;
	}
}
