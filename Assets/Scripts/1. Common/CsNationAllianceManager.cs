using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;
using System.Linq;

public class CsNationAllianceManager
{
	bool m_bWaitResponse = false;

	List<CsNationAlliance> m_listCsNationAlliance;
	List<CsNationAllianceApplication> m_listCsNationAllianceApplication;

	Guid m_guidApplicationId;
	Guid m_guidNationAllianceId;

	//---------------------------------------------------------------------------------------------------
	public static CsNationAllianceManager Instance
	{
		get { return CsSingleton<CsNationAllianceManager>.GetInstance(); }
	}

	public List<CsNationAlliance> NationAllianceList
	{
		get { return m_listCsNationAlliance; }
	}

	public List<CsNationAllianceApplication> NationAllianceApplicationList
	{
		get { return m_listCsNationAllianceApplication; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventNationAllianceApply;
	public event Delegate<CsNationAlliance> EventNationAllianceApplicationAccept;
	public event Delegate EventNationAllianceApplicationCancel;
	public event Delegate EventNationAllianceApplicationReject;
	public event Delegate EventNationAllianceBreak;

	public event Delegate EventNationAllianceApplied;
	public event Delegate<CsNationAlliance> EventNationAllianceConcluded;
	public event Delegate EventNationAllianceApplicationAccepted;
	public event Delegate EventNationAllianceApplicationCanceled;
	public event Delegate EventNationAllianceApplicationRejected;
	public event Delegate EventNationAllianceBroken;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDNationAlliance[] nationAlliances, PDNationAllianceApplication[] nationAllianceApplications)
	{
		UnInit();

		m_listCsNationAlliance = new List<CsNationAlliance>();

		for (int i = 0; i < nationAlliances.Length; i++)
		{
			m_listCsNationAlliance.Add(new CsNationAlliance(nationAlliances[i]));
		}

		m_listCsNationAllianceApplication = new List<CsNationAllianceApplication>();

		for (int i = 0; i < nationAllianceApplications.Length; i++)
		{
			m_listCsNationAllianceApplication.Add(new CsNationAllianceApplication(nationAllianceApplications[i]));
		}

		// Command
		CsRplzSession.Instance.EventResNationAllianceApply += OnEventResNationAllianceApply;
		CsRplzSession.Instance.EventResNationAllianceApplicationAccept += OnEventResNationAllianceApplicationAccept;
		CsRplzSession.Instance.EventResNationAllianceApplicationCancel += OnEventResNationAllianceApplicationCancel;
		CsRplzSession.Instance.EventResNationAllianceApplicationReject += OnEventResNationAllianceApplicationReject;
		CsRplzSession.Instance.EventResNationAllianceBreak += OnEventResNationAllianceBreak;

		// Event
		CsRplzSession.Instance.EventEvtNationAllianceApplied += OnEventEvtNationAllianceApplied;
		CsRplzSession.Instance.EventEvtNationAllianceConcluded += OnEventEvtNationAllianceConcluded;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationAccepted += OnEventEvtNationAllianceApplicationAccepted;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationCanceled += OnEventEvtNationAllianceApplicationCanceled;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationRejected += OnEventEvtNationAllianceApplicationRejected;
		CsRplzSession.Instance.EventEvtNationAllianceBroken += OnEventEvtNationAllianceBroken;

	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResNationAllianceApply -= OnEventResNationAllianceApply;
		CsRplzSession.Instance.EventResNationAllianceApplicationAccept -= OnEventResNationAllianceApplicationAccept;
		CsRplzSession.Instance.EventResNationAllianceApplicationCancel -= OnEventResNationAllianceApplicationCancel;
		CsRplzSession.Instance.EventResNationAllianceApplicationReject -= OnEventResNationAllianceApplicationReject;
		CsRplzSession.Instance.EventResNationAllianceBreak -= OnEventResNationAllianceBreak;

		// Event
		CsRplzSession.Instance.EventEvtNationAllianceApplied -= OnEventEvtNationAllianceApplied;
		CsRplzSession.Instance.EventEvtNationAllianceConcluded -= OnEventEvtNationAllianceConcluded;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationAccepted -= OnEventEvtNationAllianceApplicationAccepted;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationCanceled -= OnEventEvtNationAllianceApplicationCanceled;
		CsRplzSession.Instance.EventEvtNationAllianceApplicationRejected -= OnEventEvtNationAllianceApplicationRejected;
		CsRplzSession.Instance.EventEvtNationAllianceBroken -= OnEventEvtNationAllianceBroken;

		m_bWaitResponse = false;

		if (m_listCsNationAlliance != null)
		{
			m_listCsNationAlliance.Clear();
			m_listCsNationAlliance = null;
		}

		if (m_listCsNationAllianceApplication != null)
		{
			m_listCsNationAllianceApplication.Clear();
			m_listCsNationAllianceApplication = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNationAllianceApplication GetNationAllianceApplication(Guid guidId)
	{
		for (int i = 0; i < m_listCsNationAllianceApplication.Count; i++)
		{
			if (m_listCsNationAllianceApplication[i].Id == guidId)
				return m_listCsNationAllianceApplication[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveNationAllianceApplication(Guid guidId)
	{
		CsNationAllianceApplication csNationAllianceApplication = GetNationAllianceApplication(guidId);

		if (csNationAllianceApplication != null)
		{
			m_listCsNationAllianceApplication.Remove(csNationAllianceApplication);
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNationAlliance GetNationAlliance(Guid guidId)
	{
		for (int i = 0; i < m_listCsNationAlliance.Count; i++)
		{
			if (m_listCsNationAlliance[i].Id == guidId)
				return m_listCsNationAlliance[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveNationAlliance(Guid guidId)
	{
		CsNationAlliance csNationAlliance = GetNationAlliance(guidId);

		if (csNationAlliance != null)
		{
			m_listCsNationAlliance.Remove(csNationAlliance);
		}
	}

    //---------------------------------------------------------------------------------------------------
    public CsNationAlliance GetNationAlliance(int nNationId)
    {
        CsNationAlliance csNationAlliance = null;

        for (int i = 0; i < m_listCsNationAlliance.Count; i++)
        {
            csNationAlliance = m_listCsNationAlliance[i];

            for (int j = 0; j < csNationAlliance.Nations.Length; j++)
            {
                if (csNationAlliance.Nations[j] == nNationId)
                {
                    return csNationAlliance;
                }
                else
                {
                    continue;
                }
            }
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public int GetNationAllianceId(int nNationId)
    {
        bool bMyNationAlliance = false;
        int nNationAllianceId = 0;
        CsNationAlliance csNationAlliance = null;

        for (int i = 0; i < m_listCsNationAlliance.Count; i++)
        {
            csNationAlliance = m_listCsNationAlliance[i];

            for (int j = 0; j < csNationAlliance.Nations.Length; j++)
            {
                if (csNationAlliance.Nations[j] == nNationId)
                {
                    bMyNationAlliance = true;
                }
                else
                {
                    nNationAllianceId = csNationAlliance.Nations[j];
                }
            }

            if (bMyNationAlliance)
            {
                return nNationAllianceId;
            }
            else
            {
                continue;
            }
        }

        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    public List<int> GetNationIdAllianceApplicationList(int nNationId)
    {
        List<int> listNationAllianceApplicationId = new List<int>();

        for (int i = 0; i < m_listCsNationAllianceApplication.Count; i++)
        {
            if (m_listCsNationAllianceApplication[i].NationId == nNationId)
            {
                listNationAllianceApplicationId.Add(m_listCsNationAllianceApplication[i].TargetNationId);
            }
            else
            {
                continue;
            }
        }

        return listNationAllianceApplicationId;
    }

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청
	public void SendNationAllianceApply(int nTargetNationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationAllianceApplyCommandBody cmdBody = new NationAllianceApplyCommandBody();
			cmdBody.targetNationId = nTargetNationId;
			CsRplzSession.Instance.Send(ClientCommandName.NationAllianceApply, cmdBody);
		}
	}

	void OnEventResNationAllianceApply(int nReturnCode, NationAllianceApplyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.NationFund = resBody.fund;
			m_listCsNationAllianceApplication.Add(new CsNationAllianceApplication(resBody.application));

			if (EventNationAllianceApply != null)
			{
				EventNationAllianceApply();
			}
		}
		else if (nReturnCode == 101)
		{
			// 국가동맹관련 명령을 수행할 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001001"));
		}
		else if (nReturnCode == 102)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001002"));
		}
		else if (nReturnCode == 103)
		{
			// 자신의 국가가 동맹중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001003"));
		}
		else if (nReturnCode == 104)
		{
			// 대상국가가 동맹중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001004"));
		}
		else if (nReturnCode == 105)
		{
			// 자신의국가의 국력랭킹이 높습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001005"));
		}
		else if (nReturnCode == 106)
		{
			// 대상국가의 국력랭킹이 높습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001006"));
		}
		else if (nReturnCode == 107)
		{
			// 대상국가에 국가동맹신청을 보냈습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001007"));
		}
		else if (nReturnCode == 108)
		{
			// 대상국가의 국가동맹신청을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001008"));
		}
		else if (nReturnCode == 109)
		{
			// 국고자금이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001009"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청수락
	public void SendNationAllianceApplicationAccept(Guid guidApplicationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationAllianceApplicationAcceptCommandBody cmdBody = new NationAllianceApplicationAcceptCommandBody();
			cmdBody.applicationId = m_guidApplicationId = guidApplicationId;
			CsRplzSession.Instance.Send(ClientCommandName.NationAllianceApplicationAccept, cmdBody);
		}
	}

	void OnEventResNationAllianceApplicationAccept(int nReturnCode, NationAllianceApplicationAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.NationFund = resBody.fund;
			RemoveNationAllianceApplication(m_guidApplicationId);
			m_listCsNationAlliance.Add(new CsNationAlliance(resBody.nationAlliance));

			if (EventNationAllianceApplicationAccept != null)
			{
				EventNationAllianceApplicationAccept(m_listCsNationAlliance.Last());
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 국가동맹신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001101"));
		}
		else if (nReturnCode == 102)
		{
			// 국가동맹관련 명령을 수행할 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001102"));
		}
		else if (nReturnCode == 103)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001103"));
		}
		else if (nReturnCode == 104)
		{
			// 국고자금이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청취소
	public void SendNationAllianceApplicationCancel(Guid guidApplicationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationAllianceApplicationCancelCommandBody cmdBody = new NationAllianceApplicationCancelCommandBody();
			cmdBody.applicationId = m_guidApplicationId = guidApplicationId;
			CsRplzSession.Instance.Send(ClientCommandName.NationAllianceApplicationCancel, cmdBody);
		}
	}

	void OnEventResNationAllianceApplicationCancel(int nReturnCode, NationAllianceApplicationCancelResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.NationFund = resBody.fund;
			RemoveNationAllianceApplication(m_guidApplicationId);

			if (EventNationAllianceApplicationCancel != null)
			{
				EventNationAllianceApplicationCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 국가동맹신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001201"));
		}
		else if (nReturnCode == 102)
		{
			// 국가동맹관련 명령을 수행할 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001202"));
		}
		else if (nReturnCode == 103)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청거절
	public void SendNationAllianceApplicationReject(Guid guidApplicationId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationAllianceApplicationRejectCommandBody cmdBody = new NationAllianceApplicationRejectCommandBody();
			cmdBody.applicationId = m_guidApplicationId = guidApplicationId;
			CsRplzSession.Instance.Send(ClientCommandName.NationAllianceApplicationReject, cmdBody);
		}
	}

	void OnEventResNationAllianceApplicationReject(int nReturnCode, NationAllianceApplicationRejectResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveNationAllianceApplication(m_guidApplicationId);

			if (EventNationAllianceApplicationReject != null)
			{
				EventNationAllianceApplicationReject();
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 국가동맹신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001301"));
		}
		else if (nReturnCode == 102)
		{
			// 국가동맹관련 명령을 수행할 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001302"));
		}
		else if (nReturnCode == 103)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹파기
	public void SendNationAllianceBreak(Guid guidNationAllianceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationAllianceBreakCommandBody cmdBody = new NationAllianceBreakCommandBody();
			cmdBody.nationAllianceId = m_guidNationAllianceId = guidNationAllianceId;
			CsRplzSession.Instance.Send(ClientCommandName.NationAllianceBreak, cmdBody);
		}
	}

	void OnEventResNationAllianceBreak(int nReturnCode, NationAllianceBreakResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveNationAlliance(m_guidNationAllianceId);

			if (EventNationAllianceBreak != null)
			{
				EventNationAllianceBreak();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상동맹이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001401"));
		}
		else if (nReturnCode == 102)
		{
			// 자신의 국가가 동맹중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001402"));
		}
		else if (nReturnCode == 103)
		{
			// 대상동맹이 자신국가동맹이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001403"));
		}
		else if (nReturnCode == 104)
		{
			// 국가동맹파기불가기간이 만료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001404"));
		}
		else if (nReturnCode == 105)
		{
			// 국가동맹관련 명령을 수행할 수 없는 시간입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001405"));
		}
		else if (nReturnCode == 106)
		{
			// 권한이 없습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A61_ERROR_001406"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청
	void OnEventEvtNationAllianceApplied(SEBNationAllianceAppliedEventBody eventBody)
	{
		m_listCsNationAllianceApplication.Add(new CsNationAllianceApplication(eventBody.application));

		if (EventNationAllianceApplied != null)
		{
			EventNationAllianceApplied();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹체결
	void OnEventEvtNationAllianceConcluded(SEBNationAllianceConcludedEventBody eventBody)
	{
		m_listCsNationAlliance.Add(new CsNationAlliance(eventBody.nationAlliance));

		if (EventNationAllianceConcluded != null)
		{
			EventNationAllianceConcluded(m_listCsNationAlliance.Last());
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청수락
	void OnEventEvtNationAllianceApplicationAccepted(SEBNationAllianceApplicationAcceptedEventBody eventBody)
	{
		RemoveNationAllianceApplication(eventBody.applicationId);

		if (EventNationAllianceApplicationAccepted != null)
		{
			EventNationAllianceApplicationAccepted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청취소
	void OnEventEvtNationAllianceApplicationCanceled(SEBNationAllianceApplicationCanceledEventBody eventBody)
	{
		RemoveNationAllianceApplication(eventBody.applicationId);

		if (EventNationAllianceApplicationCanceled != null)
		{
			EventNationAllianceApplicationCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹신청거절
	void OnEventEvtNationAllianceApplicationRejected(SEBNationAllianceApplicationRejectedEventBody eventBody)
	{
		RemoveNationAllianceApplication(eventBody.applicationId);

		if (EventNationAllianceApplicationRejected != null)
		{
			EventNationAllianceApplicationRejected();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가동맹파기
	void OnEventEvtNationAllianceBroken(SEBNationAllianceBrokenEventBody eventBody)
	{
		RemoveNationAlliance(eventBody.nationAllianceId);

		if (EventNationAllianceBroken != null)
		{
			EventNationAllianceBroken();
		}
	}


	#endregion Protocol.Event
}
