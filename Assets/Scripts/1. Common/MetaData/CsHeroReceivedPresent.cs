using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-06)
//---------------------------------------------------------------------------------------------------

public class CsHeroReceivedPresent
{
	Guid m_guidSenderId;
	string m_strSenderName;
	CsNation m_csNationSender;
	CsPresent m_csPresent;

	//---------------------------------------------------------------------------------------------------
	public Guid SenderId
	{
		get { return m_guidSenderId; }
	}

	public string SenderName
	{
		get { return m_strSenderName; }
	}

	public CsNation SenderNation
	{
		get { return m_csNationSender; }
	}

	public CsPresent Present
	{
		get { return m_csPresent; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroReceivedPresent(Guid guidSenderId, string strSenderName, int nSenderNationId, int nPresentId)
	{
		m_guidSenderId = guidSenderId;
		m_strSenderName = strSenderName;
		m_csNationSender = CsGameData.Instance.GetNation(nSenderNationId);
		m_csPresent = CsGameData.Instance.GetPresent(nPresentId);
	}
}
