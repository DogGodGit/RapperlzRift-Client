using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-01-02)
//---------------------------------------------------------------------------------------------------

public class CsPartyApplication
{
	long m_lNo;
	Guid m_guidPartyId;
	Guid m_guidApplicantId;
	string m_strApplicantName;
    float m_flRemainingTime;

    //---------------------------------------------------------------------------------------------------
    public long No
	{
		get { return m_lNo; }
	}

	public Guid PartyId
	{
		get { return m_guidPartyId; }
	}

	public Guid ApplicantId
	{
		get { return m_guidApplicantId; }
	}

	public string ApplicantName
	{
		get { return m_strApplicantName; }
	}

    public float RemainingTime
    {
        get { return m_flRemainingTime; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsPartyApplication(PDPartyApplication partyApplication)
	{
		m_lNo = partyApplication.no;
		m_guidPartyId = partyApplication.partyId;
		m_guidApplicantId = partyApplication.applicantId;
		m_strApplicantName = partyApplication.applicantName;
        m_flRemainingTime = Time.realtimeSinceStartup + CsGameConfig.Instance.PartyApplicationLifetime;
    }

}
