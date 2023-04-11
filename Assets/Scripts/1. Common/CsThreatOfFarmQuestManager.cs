using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientCommon;
using SimpleDebugLog;
using System;
using UnityEngine.SceneManagement;


public enum EnQuestState { None, Accepted, Executed, Complete };

public class CsTopqMonster
{
	long m_lInstanceId;
	float m_time;
	Vector3 m_vtPos;

	public CsTopqMonster(long lInstanceId, Vector3 vtPos, float flRemainingLifeTime)
	{
		m_lInstanceId = lInstanceId;
		m_vtPos = vtPos;
		m_time = Time.time + flRemainingLifeTime;
	}

	public float RemainingLifeTime
	{
		get { return Mathf.Max(m_time - Time.time, 0); }
	}

	public long InstanceId
	{
		get { return m_lInstanceId;}
	}

	public Vector3 Pos
	{
		get { return m_vtPos;}
	}
}

public class CsThreatOfFarmQuestManager
{
	DateTime m_dtQuestStart;
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	int m_nProgressCount = 0;		// 진행 카운트

	CsThreatOfFarmQuest m_csQuest;
	CsThreatOfFarmQuestMission m_csMission;

	EnQuestState m_enQuestState = EnQuestState.None;
	CsTopqMonster m_csMonster;

	//---------------------------------------------------------------------------------------------------
	public static CsThreatOfFarmQuestManager Instance
	{
		get { return CsSingleton<CsThreatOfFarmQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ui, ig
	public event Delegate EventQuestAcceptDialog; // ui
	public event Delegate EventQuestAccepted;
	public event Delegate EventQuestCompleteDialog; // ui
	public event Delegate EventQuestComplete;
	public event Delegate EventMissionAccepted;
	public event Delegate<bool, long>  EventMissionComplete;
	public event Delegate EventMissionAbandoned;
	public event Delegate EventMissionFail;
	public event Delegate<long, Vector3, float> EventMissionMonsterSpawned;
	public event Delegate EventQuestReset;

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuestManager()
	{
		// Command
		CsRplzSession.Instance.EventResTreatOfFarmQuestAccept += OnEventResQuestAccept;
		CsRplzSession.Instance.EventResTreatOfFarmQuestMissionAccept += OnEventResMissionAccept;
		CsRplzSession.Instance.EventResTreatOfFarmQuestMissionAbandon += OnEventResMissionAbandon;
		CsRplzSession.Instance.EventResTreatOfFarmQuestComplete += OnEventResQuestComplete;

		// Event
		CsRplzSession.Instance.EventEvtTreatOfFarmQuestMissionComplete += OnEventEvtThreatOfFarmQuestMissionComplete;
		CsRplzSession.Instance.EventEvtTreatOfFarmQuestMissionFail += OnEventEvtThreatOfFarmQuestMissionFail;
		CsRplzSession.Instance.EventEvtTreatOfFarmQuestMissionMonsterSpawned += OnEventEvtThreatOfFarmQuestMissionMonsterSpawned;
	}

	//---------------------------------------------------------------------------------------------------
	public CsThreatOfFarmQuest Quest
	{
		get { return m_csQuest; }
	}

	public CsThreatOfFarmQuestMission Mission
	{
		get { return m_csMission; }
	}

	public bool Auto
	{
		get { return m_bAuto; }
	}

	public int ProgressCount
	{
		get { return m_nProgressCount; }
	}

	public bool IsAccepted
	{
		get { return m_enQuestState >= EnQuestState.Accepted; }
	}

	public bool IsExecuted
	{
		get { return m_enQuestState >= EnQuestState.Executed; }	
	}

	public bool IsComplete
	{
		get { return m_enQuestState >= EnQuestState.Complete; }
	}

	public EnQuestState QuestState
	{
		get { return m_enQuestState; }
	}

	public bool ResponseReady
	{
		get { return m_bWaitResponse; }
	}

	public CsTopqMonster Monster
	{
		get { return m_csMonster; }
	}

	public bool IsMissionExist
	{
		get { return m_csMission != null; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroTreatOfFarmQuest pDQuest)
	{
		UnInit();
		
		m_csQuest = CsGameData.Instance.ThreatOfFarmQuest;

		if (pDQuest == null)
		{
			m_enQuestState = EnQuestState.None;
		}
		else
		{
			if (pDQuest.completed)
			{				
				m_enQuestState = EnQuestState.Complete;
			}
			else
			{
				m_nProgressCount = pDQuest.completedMissionCount;
				if (m_nProgressCount < m_csQuest.LimitCount)
				{
					m_enQuestState = EnQuestState.Accepted;
					if (pDQuest.currentMission != null)
					{
						SetMission(pDQuest.currentMission);
					}
				}
				else
				{
					m_enQuestState = EnQuestState.Executed;
				}
			}
		}
		dd.d("CsThreatOfFarmQuestManager.Initialize    m_enQuestState ", m_enQuestState, m_nProgressCount);
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_nProgressCount = 0;
		m_bWaitResponse = false;
		m_bAuto = false;
		m_csQuest = null;
		m_csMission = null;
		m_csMonster = null;
		m_enQuestState = EnQuestState.None;
	}
		
	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		dd.d("CsThreatOfFarmQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto, m_enQuestState);

		if (m_csMission != null)
		{
			dd.d(m_csQuest.LimitCount, m_nProgressCount);
		}

		if (m_bWaitResponse || IsComplete)
		{
			return;
		}

		if (m_bAuto == false)
		{
			m_bAuto = true;
			dd.d(11);

			if (EventStartAutoPlay != null)
			{
				dd.d(22);
				EventStartAutoPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		dd.d("CsThreatOfFarmQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);

		if (m_bAuto == true)
		{
			m_bAuto = false;
			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool AcceptReadyOK()
	{
		dd.d("CsThreatOfFarmQuestManager.AcceptReadyOK", m_enQuestState, m_bWaitResponse, m_nProgressCount);
		if(m_bWaitResponse) return false;
		if(m_enQuestState == EnQuestState.None && m_nProgressCount >= m_csQuest.LimitCount) return false;

		if (EventQuestAcceptDialog != null)
		{
			dd.d("EventQuestAcceptDialog");
			EventQuestAcceptDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void AcceptQuest()
	{
		dd.d("AcceptQuest()", IsAccepted, m_bWaitResponse);

		if (m_bWaitResponse) return;

		if (m_enQuestState == EnQuestState.None)
		{
			SendQuestAccept();
		}
		else if (m_enQuestState == EnQuestState.Accepted)
		{	
			if (m_csMission == null)
			{
				SendMissionAccept();
			}
		}
	}

//	//---------------------------------------------------------------------------------------------------
//	public void AcceptMission()
//	{
//		dd.d("AcceptMission()", IsAccepted, m_bWaitResponse);
//		if (IsAccepted && !IsMissionExist && !m_bWaitResponse)
//		{	
//			SendMissionAccept();
//		}
//	}
//
	//---------------------------------------------------------------------------------------------------
	public void CompleteReadyOK()
	{
		dd.d("CsThreatOfFarmQuestManager.CompleteReadyOK", m_enQuestState, m_bWaitResponse);

		if (m_enQuestState == EnQuestState.Executed && !m_bWaitResponse)
		{
			if (EventQuestCompleteDialog != null)
			{
				dd.d("EventQuestCompleteDialog");
				EventQuestCompleteDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CompleteQuest()
	{
		dd.d("CompleteQuest()", IsComplete, m_bWaitResponse);
		if (m_enQuestState == EnQuestState.Executed && !m_bWaitResponse)
		{	
			SendQuestComplete();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetQuest()
	{
		StopAutoPlay(this);
		if (EventQuestReset != null)
		{
			EventQuestReset();
		}
		m_nProgressCount = 0;
		m_enQuestState = EnQuestState.None;
		m_csQuest = null;
		m_csMission = null;
		m_csMonster = null;

	}

	//---------------------------------------------------------------------------------------------------
	void SetMission(PDHeroTreatOfFarmQuestMission pDMission)
	{
		SetMission(pDMission.missionId, pDMission.monsterInstanceId, 
			CsRplzSession.Translate(pDMission.monsterPosition), pDMission.remainingMonsterLifetime);
	}

	//---------------------------------------------------------------------------------------------------
	void SetMission(int nMissionId, long lInstanceId, Vector3 vtPos, float flRemainingLifeTime)
	{
		dd.d("SetMission", nMissionId, lInstanceId, vtPos, flRemainingLifeTime);
		m_csMission = m_csQuest.GetThreatOfFarmQuestMission(nMissionId);
		dd.d("SetMission", m_csMission != null);
		dd.d(m_csMission.TargetPosition);


		if (lInstanceId > 0)
		{
			m_csMonster = new CsTopqMonster(lInstanceId, vtPos, flRemainingLifeTime);
		}
		else
		{
			m_csMonster = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AbandonMission()
	{
		SendMissionAbandon();
	}

	#region Public Event


	#endregion Public Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	void SendQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TreatOfFarmQuestAcceptCommandBody cmdBody = new TreatOfFarmQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TreatOfFarmQuestAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResQuestAccept(int nReturnCode, TreatOfFarmQuestAcceptResponseBody resBody)
	{
		dd.d("OnEventResQuestAccept nReturnCode", nReturnCode);
		m_bWaitResponse = false;
		if (nReturnCode == 0)
		{
			m_enQuestState = EnQuestState.Accepted;
			SetMission(resBody.heroTreatOfFarmQuest.currentMission);

			if (EventQuestAccepted != null)
			{
				EventQuestAccepted();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅의 레벨이 부족합니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A48_ERROR_0101"));

			//RemoteSettings();
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendMissionAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TreatOfFarmQuestMissionAcceptCommandBody cmdBody = new TreatOfFarmQuestMissionAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TreatOfFarmQuestMissionAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMissionAccept(int nReturnCode,  TreatOfFarmQuestMissionAcceptResponseBody resBody)
	{
		dd.d("OnEventResMissionAccept nReturnCode", nReturnCode);
		m_bWaitResponse = false;
		if (nReturnCode == 0)
		{
			SetMission(resBody.heroTreatOfFarmQuestMission);

			if (EventMissionAccepted != null)
			{
				EventMissionAccepted();
			}
		}
		else if (nReturnCode == 101)
		{
			// 현재 날짜가 퀘스트시작날짜와 다릅니다.
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A48_ERROR_0301"));
			ResetQuest();
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendMissionAbandon()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TreatOfFarmQuestMissionAbandonCommandBody cmdBody = new TreatOfFarmQuestMissionAbandonCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TreatOfFarmQuestMissionAbandon, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMissionAbandon(int nReturnCode,  TreatOfFarmQuestMissionAbandonResponseBody resBody)
	{
		dd.d("OnEventResMissionAbandon nReturnCode", nReturnCode);
		m_bWaitResponse = false;

		// check time

		DateTime dt = CsGameData.Instance.MyHeroInfo.TimeOffset.Date;

		if (nReturnCode == 0)
		{
			m_csMission = null;
			if (EventMissionAbandoned != null)
			{
				EventMissionAbandoned();
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TreatOfFarmQuestCompleteCommandBody cmdBody = new TreatOfFarmQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TreatOfFarmQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResQuestComplete(int nReturnCode,  TreatOfFarmQuestCompleteResponseBody resBody)
	{
		dd.d("OnEventResQuestComplete nReturnCode", nReturnCode);
		m_bWaitResponse = false;
		if (nReturnCode == 0)
		{
			m_enQuestState = EnQuestState.Complete;
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventQuestComplete != null)
			{
				EventQuestComplete();
			}

			if (m_bAuto)
			{
				StopAutoPlay(this);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 메인퀘스트던전단계시작
	void OnEventEvtThreatOfFarmQuestMissionComplete(SEBTreatOfFarmQuestMissionCompleteEventBody eventBody)
	{
		dd.d("OnEventEvtThreatOfFarmQuestMissionComplete()", m_csQuest.LimitCount, eventBody.completedMissionCount);

		m_nProgressCount = eventBody.completedMissionCount;

		if (m_nProgressCount < m_csQuest.LimitCount)
		{
			SetMission(eventBody.nextMission);
		}
		else
		{
			m_enQuestState = EnQuestState.Executed;
			m_csMission = null;
			m_csMonster = null;
		}

        int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

        CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

        CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
        CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;
        CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
        CsGameData.Instance.MyHeroInfo.Level = eventBody.level;

        CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

        bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		// check time


		if (EventMissionComplete != null)
		{
            EventMissionComplete(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtThreatOfFarmQuestMissionFail(SEBTreatOfFarmQuestMissionFailEventBody eventBody)
	{
		dd.d("OnEventEvtThreatOfFarmQuestMissionFail()");
		m_csMission = null;
		// check time
		if (EventMissionFail != null)
		{
			EventMissionFail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtThreatOfFarmQuestMissionMonsterSpawned(SEBTreatOfFarmQuestMissionMonsterSpawnedEventBody eventBody)
	{
		dd.d("OnEventEvtThreatOfFarmQuestMissionMonsterSpawned()");
		m_csMonster = new CsTopqMonster(eventBody.instanceId, CsRplzSession.Translate(eventBody.position), eventBody.remainingLifetime);
		if (EventMissionMonsterSpawned != null)
		{
			EventMissionMonsterSpawned(eventBody.instanceId, CsRplzSession.Translate(eventBody.position), eventBody.remainingLifetime);
		}
	}

	#endregion Protocol.Event

}
