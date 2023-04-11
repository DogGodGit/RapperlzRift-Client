using System;
using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsFriend
{
	Guid m_guidId;
	string m_strName;
	CsNation m_csNation;
	CsJob m_csJob;
	int m_nLevel;
	long m_lBattlePower;
    bool m_bIsLoggedIn;

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
	}

	public long BattlePower
	{
		get { return m_lBattlePower; }
	}

    public bool IsLoggedIn
    {
        get { return m_bIsLoggedIn; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsFriend(PDFriend friend)
	{
		m_guidId = friend.id;
		m_strName = friend.name;
		m_csNation = CsGameData.Instance.GetNation(friend.nationId);
		m_csJob = CsGameData.Instance.GetJob(friend.jobId);
		m_nLevel = friend.level;
		m_lBattlePower = friend.battlePower;
        m_bIsLoggedIn = friend.isLoggedIn;
	}

	//---------------------------------------------------------------------------------------------------
	public void Update(PDFriend friend)
	{
		m_nLevel = friend.level;
		m_lBattlePower = friend.battlePower;
	}
}
