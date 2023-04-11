using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsPartyMember
{
	Guid m_guid;
	string m_strName;
	CsJob m_csJob;
	int m_nLevel;
	long m_lBattlePower;
	int m_nMaxHp;
	int m_nHp;
	bool m_bIsLoggedIn;

	//---------------------------------------------------------------------------------------------------
	public Guid Id
	{
		get { return m_guid; }
	}

	public string Name
	{
		get { return m_strName; }
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

	public int MaxHp
	{
		get { return m_nMaxHp; }
		set { m_nMaxHp = value; }
	}

	public int Hp
	{
		get { return m_nHp; }
		set { m_nHp = value; }
	}

	public bool IsLoggedIn
	{
		get { return m_bIsLoggedIn; }
		set { m_bIsLoggedIn = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsPartyMember(PDPartyMember partyMember)
	{
		m_guid = partyMember.id;
		m_strName = partyMember.name;
		m_csJob = CsGameData.Instance.GetJob(partyMember.jobId);
		m_nLevel = partyMember.level;
		m_lBattlePower = partyMember.battlePower;
		m_nMaxHp = partyMember.maxHP;
		m_nHp = partyMember.hp;
		m_bIsLoggedIn = partyMember.isLoggedIn;
	}

	public void UpdatePartyMember(PDPartyMember partyMember)
	{
		m_csJob = CsGameData.Instance.GetJob(partyMember.jobId);
		m_nLevel = partyMember.level;
		m_lBattlePower = partyMember.battlePower;
		m_nMaxHp = partyMember.maxHP;
		m_nHp = partyMember.hp;
		m_bIsLoggedIn = partyMember.isLoggedIn;
	}
}
