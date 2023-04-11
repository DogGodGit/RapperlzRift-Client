using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;

public enum EnTrueHeroQuestState 
{
	None = 0,		// 대기
	Accepted,		// 수락
	Executed,		// 진행 완료
	Completed		// 보상 완료
};

public class CsTrueHeroQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bInteracted = false;
	int m_nQuestAcceptVipLevel;			// 퀘스트 시작 시점의 VIP레벨
	float m_flRemainingWaitingTime = 0;	// 버티기 남은 시간

	int m_nLastStepNo = 0;				// Executed 상태에서 사용

	DateTime m_dtAcceptedDate;
	Guid m_guidInstanceId;

	EnTrueHeroQuestState m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
	EnInteractionState m_enInteractionState = EnInteractionState.None;
	CsTrueHeroQuest m_csTrueHeroQuest;
	CsTrueHeroQuestStep m_csTrueHeroQuestStep;

	//---------------------------------------------------------------------------------------------------
	public static CsTrueHeroQuestManager Instance
	{
		get { return CsSingleton<CsTrueHeroQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object> EventStopAutoPlay;
	public event Delegate EventNpcDialog;
	public event Delegate<int, int, Vector3> EventTrueHeroQuestAutoMove;                    // 즉시 이동 클릭 시

	public event Delegate EventMyHeroTrueHeroQuestInteractionStart;
	public event Delegate EventTrueHeroObjectInteractionCancel;
	public event Delegate<EnInteractionState> EventChangeInteractionState;

	public event Delegate EventTrueHeroQuestAccept;
	public event Delegate<bool, long, int> EventTrueHeroQuestComplete;
	public event Delegate EventTrueHeroQuestStepInteractionStart;

	public event Delegate EventTrueHeroQuestStepInteractionCancel;

	public event Delegate<Guid> EventHeroTrueHeroQuestStepInteractionStarted;				// 당사자 이외
	public event Delegate<Guid> EventHeroTrueHeroQuestStepInteractionCanceled;				// 당사자 이외
	public event Delegate<Guid> EventHeroTrueHeroQuestStepInteractionFinished;				// 당사자 이외
	public event Delegate<CsChattingMessage> EventTrueHeroQuestTaunted;						// 관련 국민
	public event Delegate EventTrueHeroQuestStepInteractionCanceled;						// 당사자
	public event Delegate EventTrueHeroQuestStepInteractionFinished;						// 당사자
	public event Delegate EventTrueHeroQuestStepWaitingCanceled;							// 당사자
	public event Delegate EventTrueHeroQuestStepCompleted;									// 당사자

	

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool Interacted
	{
		get { return m_bInteracted; }
	}
	
	public int QuestAcceptVipLevel
	{
		get { return m_nQuestAcceptVipLevel; }
	}

	public EnTrueHeroQuestState TrueHeroQuestState
	{
		get { return m_enTrueHeroQuestState; }
	}

	public EnInteractionState InteractionState
	{
		get { return m_enInteractionState; }
	}

	public CsTrueHeroQuest TrueHeroQuest
	{
		get { return m_csTrueHeroQuest; }
	}

	public CsTrueHeroQuestStep TrueHeroQuestStep
	{
		get { return m_csTrueHeroQuestStep; }
	}

	public bool IsNone
	{
		get { return m_enTrueHeroQuestState == EnTrueHeroQuestState.None; }
	}

	public bool IsAccepted
	{
		get { return m_enTrueHeroQuestState == EnTrueHeroQuestState.Accepted; }
	}

	public bool IsExecuted
	{
		get { return m_enTrueHeroQuestState == EnTrueHeroQuestState.Executed; }
	}

	public bool IsCompleted
	{
		get { return m_enTrueHeroQuestState == EnTrueHeroQuestState.Completed; }
	}

	public bool IsStateinteracting
	{
		get { return m_enInteractionState == EnInteractionState.interacting; }
	}

	public float RemainingWaitingTime
	{
		get { return m_flRemainingWaitingTime - Time.realtimeSinceStartup; }
	}

	public DateTime AcceptedDate
	{
		get { return m_dtAcceptedDate; }
	}

	public int LastStepNo
	{
		get { return m_nLastStepNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroTrueHeroQuest pdHeroTrueHeroQuest)
	{
		UnInit();
		m_csTrueHeroQuest = CsGameData.Instance.TrueHeroQuest;

		SetQuestInfo(pdHeroTrueHeroQuest);
		
		CsRplzSession.Instance.EventResTrueHeroQuestAccept += OnEventResTrueHeroQuestAccept;
		CsRplzSession.Instance.EventResTrueHeroQuestComplete += OnEventResTrueHeroQuestComplete;
		CsRplzSession.Instance.EventResTrueHeroQuestStepInteractionStart += OnEventResTrueHeroQuestStepInteractionStart;

		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionStarted += OnEventHeroTrueHeroQuestStepInteractionStarted;
		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionCanceled += OnEventHeroTrueHeroQuestStepInteractionCanceled;
		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionFinished += OnEventHeroTrueHeroQuestStepInteractionFinished;

		CsRplzSession.Instance.EventEvtTrueHeroQuestTaunted += OnEventTrueHeroQuestTaunted;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepInteractionCanceled += OnEventTrueHeroQuestStepInteractionCanceled;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepInteractionFinished += OnEventTrueHeroQuestStepInteractionFinished;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepWaitingCanceled += OnEventTrueHeroQuestStepWaitingCanceled;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepCompleted += OnEventTrueHeroQuestStepCompleted;

		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		CsRplzSession.Instance.EventResTrueHeroQuestAccept -= OnEventResTrueHeroQuestAccept;
		CsRplzSession.Instance.EventResTrueHeroQuestComplete -= OnEventResTrueHeroQuestComplete;
		CsRplzSession.Instance.EventResTrueHeroQuestStepInteractionStart -= OnEventResTrueHeroQuestStepInteractionStart;

		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionStarted -= OnEventHeroTrueHeroQuestStepInteractionStarted;
		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionCanceled -= OnEventHeroTrueHeroQuestStepInteractionCanceled;
		CsRplzSession.Instance.EventEvtHeroTrueHeroQuestStepInteractionFinished -= OnEventHeroTrueHeroQuestStepInteractionFinished;

		CsRplzSession.Instance.EventEvtTrueHeroQuestTaunted -= OnEventTrueHeroQuestTaunted;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepInteractionCanceled -= OnEventTrueHeroQuestStepInteractionCanceled;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepInteractionFinished -= OnEventTrueHeroQuestStepInteractionFinished;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepWaitingCanceled -= OnEventTrueHeroQuestStepWaitingCanceled;
		CsRplzSession.Instance.EventEvtTrueHeroQuestStepCompleted -= OnEventTrueHeroQuestStepCompleted;

		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

		m_bAuto = false;
		m_bWaitResponse = false;
		
		m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
		m_enInteractionState = EnInteractionState.None;
		m_csTrueHeroQuest = null; ;
		m_csTrueHeroQuestStep = null;		
	}

	//---------------------------------------------------------------------------------------------------
	void SetQuestInfo(PDHeroTrueHeroQuest pdHeroTrueHeroQuest)
	{
		if (pdHeroTrueHeroQuest == null)
		{
			m_bInteracted = false;
			m_nQuestAcceptVipLevel = 0;
			m_dtAcceptedDate = new DateTime();
			m_guidInstanceId = new Guid();
			m_csTrueHeroQuestStep = null;
			m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
        }
		else
		{
			m_bInteracted = false;
			m_nQuestAcceptVipLevel = pdHeroTrueHeroQuest.vipLevel;
			m_dtAcceptedDate = pdHeroTrueHeroQuest.acceptedDate;
			m_guidInstanceId = pdHeroTrueHeroQuest.instanceId;
			m_csTrueHeroQuestStep = m_csTrueHeroQuest.GetTrueHeroStep(pdHeroTrueHeroQuest.stepNo, m_nQuestAcceptVipLevel);
			
			if (pdHeroTrueHeroQuest.completed)
			{
				if (m_dtAcceptedDate.Date < CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
				{
					m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
				}
				else
				{
					m_enTrueHeroQuestState = EnTrueHeroQuestState.Completed;
				}
			}
			else
			{
				if (m_csTrueHeroQuestStep == null)
				{
					m_enTrueHeroQuestState = EnTrueHeroQuestState.Executed;
					m_nLastStepNo = pdHeroTrueHeroQuest.stepNo - 1;
				}
				else
				{
					m_enTrueHeroQuestState = EnTrueHeroQuestState.Accepted;
				}
			}
		}
    }

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;
		if (m_csTrueHeroQuest == null) return;

		dd.d("CsTrueHeroQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto);

		if (!m_bAuto)
		{
			m_bAuto = true;
			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		dd.d("CsTrueHeroQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);

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
	//public bool IsTrueHeroQuestNpc(int nNpcId)
	//{
	//	if (m_csTrueHeroQuest == null) return false;

	//	if (m_enTrueHeroQuestState != EnTrueHeroQuestState.Accepted)
	//	{
	//		return (m_csTrueHeroQuest.QuestNpc != null && m_csTrueHeroQuest.QuestNpc.NpcId == nNpcId);
	//	}
	//	return false;
	//}

	//---------------------------------------------------------------------------------------------------
	public bool NpcDialogReadyOK()
	{
		dd.d("NpcDialogReadyOK", m_enTrueHeroQuestState, m_bWaitResponse);
		if (m_enTrueHeroQuestState != EnTrueHeroQuestState.Accepted && !m_bWaitResponse)
		{
			if (m_csTrueHeroQuest.QuestNpc != null)
			{
				if (EventNpcDialog != null)
				{
					dd.d("NpcDialogReadyOK");
					EventNpcDialog();
				}
				return true;
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void Accept()
	{
		dd.d("Accept()", IsAccepted, m_bWaitResponse);
		if (!IsAccepted)
		{
			SendTrueHeroQuestAccept();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Complete()
	{
		if (IsExecuted)
		{
			SendTrueHeroQuestComplete();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void TryTrueHeroObjectInteractionCancel()
	{
		Debug.Log("TryContinentObjectInteractionCancel()");
		ChangeTrueHeroInteractionState(EnInteractionState.None);

		SendTrueHeroQuestStepInteractionCancel();

		if (EventTrueHeroObjectInteractionCancel != null)
		{
			EventTrueHeroObjectInteractionCancel(); // ui
		}
	}
	
	//---------------------------------------------------------------------------------------------------
	public void ChangeTrueHeroInteractionState(EnInteractionState enNewInteractionState)
	{
		dd.d("ChangeInteractionState", enNewInteractionState);
		if (m_enInteractionState != enNewInteractionState)
		{
			m_enInteractionState = enNewInteractionState;

			if (EventChangeInteractionState != null)
			{
				EventChangeInteractionState(m_enInteractionState);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void MyHeroTrueHeroQuestInteractionStart()
	{
		dd.d("ChangeInteractionState", m_enInteractionState);
		if (EventMyHeroTrueHeroQuestInteractionStart != null)
		{
			EventMyHeroTrueHeroQuestInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionStart()
	{
		dd.d("ChangeInteractionState", m_bWaitResponse, m_enInteractionState);
		SendTrueHeroQuestStepInteractionStart();
	}

	#region public.Event
	//---------------------------------------------------------------------------------------------------
	void SendTrueHeroQuestStepInteractionCancel()
	{
		CEBTrueHeroQuestStepInteractionCancelEventBody csEvt = new CEBTrueHeroQuestStepInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.TrueHeroQuestStepInteractionCancel, csEvt);

		if (EventTrueHeroQuestStepInteractionCancel != null)
		{
			EventTrueHeroQuestStepInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		// 날짜가 바뀐 경우
		if (m_enTrueHeroQuestState == EnTrueHeroQuestState.Completed)
		{
			m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventTrueHeroQuestAutoMove(int nContinentId, int nNationId, Vector3 vtPosition)
	{
		if (EventTrueHeroQuestAutoMove != null)
		{
			EventTrueHeroQuestAutoMove(nContinentId, nNationId, vtPosition);
		}
	}

	#endregion public.Event

	#region Protocol.Command
	//---------------------------------------------------------------------------------------------------
	void SendTrueHeroQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TrueHeroQuestAcceptCommandBody cmdBody = new TrueHeroQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TrueHeroQuestAccept, cmdBody);
		}
	}
	void OnEventResTrueHeroQuestAccept(int nReturnCode, TrueHeroQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResTrueHeroQuestAccept");

		if (nReturnCode == 0)
		{
			SetQuestInfo(resBody.trueHeroQuest);

			if (EventTrueHeroQuestAccept != null)
			{
				EventTrueHeroQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트NPC와 상호작용을 할 수 없는 거리입니다. [A111_ERROR_00101]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅레벨이 부족합니다. [A111_ERROR_00102]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// VIP레벨이 부족합니다. [A111_ERROR_00103]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 수락한 퀘스트가 존재합니다. [A111_ERROR_00104]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 이미 퀘스트를 완료했습니다. [A111_ERROR_00105]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00105"));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendTrueHeroQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TrueHeroQuestCompleteCommandBody cmdBody = new TrueHeroQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TrueHeroQuestComplete, cmdBody);
		}
	}
	void OnEventResTrueHeroQuestComplete(int nReturnCode, TrueHeroQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResTrueHeroQuestComplete");

		if (nReturnCode == 0)
		{
			bool bLevelUp = CsGameData.Instance.MyHeroInfo.Level < resBody.level;

			if (m_dtAcceptedDate.Date < CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
			{
				// 전날 수락한 퀘스트를 완료한 경우 대기 상태로 변경
				m_enTrueHeroQuestState = EnTrueHeroQuestState.None;
			}
			else
			{
				m_enTrueHeroQuestState = EnTrueHeroQuestState.Completed;
			}

			CsGameData.Instance.MyHeroInfo.DailyExploitPoint = resBody.dailyExploitPoint;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.ExploitPoint = resBody.exploitPoint;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;

			// resBody.date;

			if (EventTrueHeroQuestComplete != null)
			{
				EventTrueHeroQuestComplete(bLevelUp, resBody.acquiredExp, resBody.acquiredExploitPoint);
			}
		}
		else if (nReturnCode == 101)
		{
			//101 : 퀘스트NPC와 상호작용할 수 없는 거리입니다. [A111_ERROR_00201]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			//102 : 이미 퀘스트를 완료했습니다. [A111_ERROR_00202]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			//103 : 목표가 완료되지 않았습니다. [A111_ERROR_00203]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00203"));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendTrueHeroQuestStepInteractionStart()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TrueHeroQuestStepInteractionStartCommandBody cmdBody = new TrueHeroQuestStepInteractionStartCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TrueHeroQuestStepInteractionStart, cmdBody);
		}
	}
	void OnEventResTrueHeroQuestStepInteractionStart(int nReturnCode, TrueHeroQuestStepInteractionStartResponseBody resBody)
	{
		Debug.Log("OnEventResTrueHeroQuestInteractionStart  nReturnCode = "+ nReturnCode);
		m_bWaitResponse = false;
		
		if (nReturnCode == 0)
		{
			ChangeTrueHeroInteractionState(EnInteractionState.interacting);

			if (EventTrueHeroQuestStepInteractionStart != null)
			{
				EventTrueHeroQuestStepInteractionStart();
			}
		}
		else if (nReturnCode == 101)
		{
			//101 : 영웅이 죽은 상태입니다. [A111_ERROR_00301]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			//102 : 영웅이 탈것을 탑승중입니다. [A111_ERROR_00302]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			//103 : 영웅이 다른 행동중입니다. [A111_ERROR_00303]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			//104 : 이미 대기단계입니다. [A111_ERROR_00304]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00304"));
		}
		else if (nReturnCode == 105)
		{
			//105 : 동맹 국가입니다. [A111_ERROR_00305]
			CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A111_ERROR_00305"));
		}
	}
	
	#endregion Protocol.Command

	#region Protocol.Event
	//---------------------------------------------------------------------------------------------------
	// 당사자 이외
	public void OnEventHeroTrueHeroQuestStepInteractionStarted(SEBHeroTrueHeroQuestStepInteractionStartedEventBody eventBody)
	{
		if (EventHeroTrueHeroQuestStepInteractionStarted != null)
		{
			EventHeroTrueHeroQuestStepInteractionStarted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자 이외
	public void OnEventHeroTrueHeroQuestStepInteractionCanceled(SEBHeroTrueHeroQuestStepInteractionCanceledEventBody eventBody)
	{
		if (EventHeroTrueHeroQuestStepInteractionCanceled != null)
		{
			EventHeroTrueHeroQuestStepInteractionCanceled(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자 이외
	public void OnEventHeroTrueHeroQuestStepInteractionFinished(SEBHeroTrueHeroQuestStepInteractionFinishedEventBody eventBody)
	{
		if (EventHeroTrueHeroQuestStepInteractionFinished != null)
		{
			EventHeroTrueHeroQuestStepInteractionFinished(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 관련 국민
	public void OnEventTrueHeroQuestTaunted(SEBTrueHeroQuestTauntedEventBody eventBody)
	{
		CsChattingMessage csChattingMessage = new CsChattingMessage(eventBody.heroId, eventBody.heroName, eventBody.nationId, eventBody.continentId, CsRplzSession.Translate(eventBody.position));

		CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

		if (EventTrueHeroQuestTaunted != null)
		{
			EventTrueHeroQuestTaunted(csChattingMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자
	public void OnEventTrueHeroQuestStepInteractionCanceled(SEBTrueHeroQuestStepInteractionCanceledEventBody eventBody)
	{
		ChangeTrueHeroInteractionState(EnInteractionState.None);

		if (EventTrueHeroQuestStepInteractionCanceled != null)
		{
			EventTrueHeroQuestStepInteractionCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자
	public void OnEventTrueHeroQuestStepInteractionFinished(SEBTrueHeroQuestStepInteractionFinishedEventBody eventBody)
	{
		m_bInteracted = true;

		m_flRemainingWaitingTime = m_csTrueHeroQuestStep.ObjectiveWaitingTime + Time.realtimeSinceStartup;

		ChangeTrueHeroInteractionState(EnInteractionState.None);

		if (EventTrueHeroQuestStepInteractionFinished != null)
		{
			EventTrueHeroQuestStepInteractionFinished();
		}

		CsChattingMessage csChattingMessage = new CsChattingMessage(CsGameData.Instance.MyHeroInfo.HeroId, CsGameData.Instance.MyHeroInfo.Name, CsGameData.Instance.MyHeroInfo.Nation.NationId, 0, Vector3.zero);

		CsUIData.Instance.ChattingMessageList.Add(csChattingMessage);

		if (EventTrueHeroQuestTaunted != null)
		{
			EventTrueHeroQuestTaunted(csChattingMessage);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자
	public void OnEventTrueHeroQuestStepWaitingCanceled(SEBTrueHeroQuestStepWaitingCanceledEventBody eventBody)
	{
		m_bInteracted = false;

		m_flRemainingWaitingTime = 0;

		if (EventTrueHeroQuestStepWaitingCanceled != null)
		{
			EventTrueHeroQuestStepWaitingCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 당사자
	public void OnEventTrueHeroQuestStepCompleted(SEBTrueHeroQuestStepCompletedEventBody eventBody)
	{
		Debug.Log("OnEventTrueHeroQuestStepCompleted    nextStepNo = " + eventBody.nextStepNo);
		
		CsTrueHeroQuestStep prevStep = m_csTrueHeroQuestStep;

		m_csTrueHeroQuestStep = m_csTrueHeroQuest.GetTrueHeroStep(eventBody.nextStepNo, m_nQuestAcceptVipLevel);
		m_bInteracted = false;

		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		if (m_csTrueHeroQuestStep == null) // 더이상 진행할 스텝이 없어 완료 Npc 이동.
		{
			m_enTrueHeroQuestState = EnTrueHeroQuestState.Executed;
			m_nLastStepNo = eventBody.nextStepNo - 1;
		}

		if (EventTrueHeroQuestStepCompleted != null)
		{
			EventTrueHeroQuestStepCompleted();
		}

		List<CsDropObject> listLooted = new List<CsDropObject>();
		List<CsDropObject> listNotLooted = new List<CsDropObject>();

		listLooted.Add(new CsDropObjectItem((int)EnDropObjectType.Item, prevStep.ItemReward.Item.ItemId, prevStep.ItemReward.ItemOwned, prevStep.ItemReward.ItemCount));

		CsGameEventUIToUI.Instance.OnEventDropObjectLooted(listLooted, listNotLooted);
	}

	#endregion Protocol.Event
}
