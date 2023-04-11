using System;
using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-04-05)
//---------------------------------------------------------------------------------------------------

public class CsNationCall
{
	long m_lId;
	Guid m_guidCallerId;
	string m_strCallerName;
    CsNationNoblesse m_csNationNoblesseCaller;
    float m_flRemainingTime;

	//---------------------------------------------------------------------------------------------------
	public long Id
	{
		get { return m_lId; }
	}

	public Guid CallerId
	{
		get { return m_guidCallerId; }
	}

	public string CallerName
	{
		get { return m_strCallerName; }
	}

	public CsNationNoblesse CallerNationNoblesse
	{
		get { return m_csNationNoblesseCaller; }
	}

    public float RemainingTime
    {
        get { return m_flRemainingTime; }
    }

	//---------------------------------------------------------------------------------------------------
	public CsNationCall(PDNationCall nationCall)
	{
		m_lId = nationCall.id;
		m_guidCallerId = nationCall.callerId;
		m_strCallerName = nationCall.callerName;
        m_csNationNoblesseCaller = CsGameData.Instance.GetNationNoblesse(nationCall.callerNoblesseId);
        m_flRemainingTime = Time.realtimeSinceStartup + CsGameConfig.Instance.NationCallLifetime;
	}
}
