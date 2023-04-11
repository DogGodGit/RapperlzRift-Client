using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;

public enum EnSecretLetterState { None = 0, Accepted = 1, Executed = 2, Completed = 3 }
public class CsSecretLetterQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bSecretLetterPick = false;
	bool m_bLowPickComplete = false;
	bool m_bInteractionButton = false;

	int m_nTargetNationId;
	int m_nPickCount;
	int m_nPickedLetterGrade;

	int m_nDailySecretLetterQuestStartCount;
	DateTime m_dtDateSecretLetterQuestStartCount;

	int m_nSecretLetterQuestTargetNationId;

	EnSecretLetterState m_enSecretLetterState = EnSecretLetterState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsSecretLetterQuestManager Instance
	{
		get { return CsSingleton<CsSecretLetterQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui
	public event Delegate EventUpdateState;
	public event Delegate EventAcceptDialog;
	public event Delegate EventMissionDialog;
	public event Delegate EventNationTransmission;
	public event Delegate<bool> EventInteractionArea;
	public event Delegate EventMyHeroSecretLetterPickStart;

	public event Delegate EventMyHeroSecretLetterPickCanceled;

	public event Delegate EventSecretLetterQuestAccept;
	public event Delegate<bool, long, int> EventSecretLetterQuestComplete;
	public event Delegate EventSecretLetterPickStart;
	public event Delegate EventSecretLetterPickCompleted;
	public event Delegate EventSecretLetterPickCanceled;

	public event Delegate<Guid> EventHeroSecretLetterPickStarted;
	public event Delegate<Guid, int> EventHeroSecretLetterPickCompleted;
	public event Delegate<Guid> EventHeroSecretLetterPickCanceled;
	public event Delegate<Guid> EventHeroSecretLetterQuestCompleted;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool SecretLetterPick
	{
		get { return m_bSecretLetterPick; }
	}

	public bool LowPickComplete
	{
		get { return m_bLowPickComplete; }
		set { m_bLowPickComplete = value; }
	}

	public bool InteractionButton
	{
		get { return m_bInteractionButton; }
		set { m_bInteractionButton = value; }
	}

	public int TargetNationId
	{
		get { return m_nTargetNationId; }
	}

	public int PickCount
	{
		get { return m_nPickCount; }
	}

	public int PickedLetterGrade
	{
		get { return m_nPickedLetterGrade; }
	}

	public int DailySecretLetterQuestStartCount
	{
		get { return m_nDailySecretLetterQuestStartCount; }
		set { m_nDailySecretLetterQuestStartCount = value; }
	}

	public DateTime SecretLetterQuestStartCountDate
	{
		get { return m_dtDateSecretLetterQuestStartCount; }
		set { m_dtDateSecretLetterQuestStartCount = value; }
	}

	public int SecretLetterQuestTargetNationId
	{
		get { return m_nSecretLetterQuestTargetNationId; }
		set { m_nSecretLetterQuestTargetNationId = value; }
	}

	public CsSecretLetterQuest SecretLetterQuest
	{
		get { return CsGameData.Instance.SecretLetterQuest; }
		set { CsGameData.Instance.SecretLetterQuest = value; }
	}

	public EnSecretLetterState SecretLetterState
	{
		get { return m_enSecretLetterState; }
	}

	public bool IsAccepted
	{
		get { return m_enSecretLetterState == EnSecretLetterState.Accepted; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroSecretLetterQuest heroSecretLetterQuest, int nDailyStartCount, DateTime dtDate)
	{
		UnInit();

		SetQuestInfo(heroSecretLetterQuest, nDailyStartCount, dtDate);

		// Command
		CsRplzSession.Instance.EventResSecretLetterQuestAccept += OnEventResSecretLetterQuestAccept;
		CsRplzSession.Instance.EventResSecretLetterQuestComplete += OnEventResSecretLetterQuestComplete;
		CsRplzSession.Instance.EventResSecretLetterPickStart += OnEventResSecretLetterPickStart;

		// Event
		CsRplzSession.Instance.EventEvtSecretLetterPickCompleted += OnEventEvtSecretLetterPickCompleted;
		CsRplzSession.Instance.EventEvtSecretLetterPickCanceled += OnEventEvtSecretLetterPickCanceled;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickStarted += OnEventEvtHeroSecretLetterPickStarted;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickCompleted += OnEventEvtHeroSecretLetterPickCompleted;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickCanceled += OnEventEvtHeroSecretLetterPickCanceled;
		CsRplzSession.Instance.EventEvtHeroSecretLetterQuestCompleted += OnEventEvtHeroSecretLetterQuestCompleted;
		CsRplzSession.Instance.EventEvtSecretLetterQuestTargetNationChanged += OnEventEvtSecretLetterQuestTargetNationChanged;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResSecretLetterQuestAccept -= OnEventResSecretLetterQuestAccept;
		CsRplzSession.Instance.EventResSecretLetterQuestComplete -= OnEventResSecretLetterQuestComplete;
		CsRplzSession.Instance.EventResSecretLetterPickStart -= OnEventResSecretLetterPickStart;

		// Event
		CsRplzSession.Instance.EventEvtSecretLetterPickCompleted -= OnEventEvtSecretLetterPickCompleted;
		CsRplzSession.Instance.EventEvtSecretLetterPickCanceled -= OnEventEvtSecretLetterPickCanceled;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickStarted -= OnEventEvtHeroSecretLetterPickStarted;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickCompleted -= OnEventEvtHeroSecretLetterPickCompleted;
		CsRplzSession.Instance.EventEvtHeroSecretLetterPickCanceled -= OnEventEvtHeroSecretLetterPickCanceled;
		CsRplzSession.Instance.EventEvtHeroSecretLetterQuestCompleted -= OnEventEvtHeroSecretLetterQuestCompleted;
		CsRplzSession.Instance.EventEvtSecretLetterQuestTargetNationChanged -= OnEventEvtSecretLetterQuestTargetNationChanged;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_bSecretLetterPick = false;
		m_bLowPickComplete = false;
		m_bInteractionButton = false;
		m_enSecretLetterState = EnSecretLetterState.None;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetQuestInfo(PDHeroSecretLetterQuest heroSecretLetterQuest, int nDailyStartCount, DateTime dtDate)
	{
		if (heroSecretLetterQuest == null)
		{
			UpdateSecretLetterState(true);
		}
		else
		{
			m_nTargetNationId = heroSecretLetterQuest.targetNationId;
			m_nPickCount = heroSecretLetterQuest.pickCount;
			m_nPickedLetterGrade = heroSecretLetterQuest.pickedLetterGrade;
			UpdateSecretLetterState();
		}

		m_nDailySecretLetterQuestStartCount = nDailyStartCount;
		m_dtDateSecretLetterQuestStartCount = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateSecretLetterState(bool bReset = false)
	{
		if (bReset)
		{
			m_bLowPickComplete = false;
			m_nTargetNationId = 0;
			m_nPickCount = 0;
			m_nPickedLetterGrade = 0;
			m_enSecretLetterState = EnSecretLetterState.None;
		}
		else
		{
			if (m_nPickCount == 0)
			{
				m_enSecretLetterState = EnSecretLetterState.Accepted;
			}
			else
			{
				if (m_nPickedLetterGrade == 5)
				{
					m_enSecretLetterState = EnSecretLetterState.Completed;
				}
				else
				{
					m_enSecretLetterState = EnSecretLetterState.Executed;
				}
			}
		}

		if (EventUpdateState != null)
		{
			EventUpdateState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;
		if (CsGameData.Instance.SecretLetterQuest == null) return;
		dd.d("CsSecretLetterQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto);
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
		dd.d("CsSecretLetterQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
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
		Debug.Log("1. AcceptReadyOK()");
		if (m_bWaitResponse) return false;
		if (m_enSecretLetterState == EnSecretLetterState.None && m_nDailySecretLetterQuestStartCount >= SecretLetterQuest.LimitCount) return false;

		if (EventAcceptDialog != null)
		{
			Debug.Log("2. AcceptReadyOK()");
			EventAcceptDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void MissionReadyOK()
	{
		Debug.Log("1. MissionReadyOK()");
		if (m_bWaitResponse) return;
		if (m_enSecretLetterState == EnSecretLetterState.Accepted || m_enSecretLetterState == EnSecretLetterState.Executed)
		{
			if (EventMissionDialog != null)
			{
				Debug.Log("2. MissionReadyOK()");
				EventMissionDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NationTransmissionReadyOK()
	{
		if (m_bWaitResponse) return;

		if (EventNationTransmission != null)
		{
			dd.d("NationTransmissionReadyOK");
			EventNationTransmission();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionArea(bool bEnter)
	{
		dd.d("InteractionArea  bEnter = ", bEnter);
		m_bInteractionButton = bEnter;
		if (EventInteractionArea != null)
		{
			EventInteractionArea(bEnter);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckInteractionNpc(int nNpcId)
	{
		if (SecretLetterQuest.TargetNpcInfo.NpcId == nNpcId)
		{
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartSecretLetterPickStart()
	{
		if (EventMyHeroSecretLetterPickStart != null)
		{
			EventMyHeroSecretLetterPickStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsSecretLetterQuestNpc(int nNpcId)
	{
		if (SecretLetterQuest == null) return false;

		if (m_enSecretLetterState == EnSecretLetterState.None || m_enSecretLetterState == EnSecretLetterState.Executed || m_enSecretLetterState == EnSecretLetterState.Completed)
		{
			return (SecretLetterQuest.QuestNpcInfo != null && SecretLetterQuest.QuestNpcInfo.NpcId == nNpcId);
		}

		return false;
	}
	#region public.Event

	//---------------------------------------------------------------------------------------------------
	public void SendSecretLetterPickCancel()
	{
		Debug.Log("SendSecretLetterPickCancel()");
		m_bSecretLetterPick = false;
		CEBSecretLetterPickCancelEventBody csEvt = new CEBSecretLetterPickCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.SecretLetterPickCancel, csEvt);

		if (EventMyHeroSecretLetterPickCanceled != null)
		{
			EventMyHeroSecretLetterPickCanceled();
		}
	}

	#endregion public.Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendSecretLetterQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			SecretLetterQuestAcceptCommandBody cmdBody = new SecretLetterQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SecretLetterQuestAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResSecretLetterQuestAccept(int nReturnCode, SecretLetterQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			SetQuestInfo(resBody.quest, resBody.dailyStartCount, resBody.date);

			if (EventSecretLetterQuestAccept != null)
			{
				EventSecretLetterQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A51_ERROR_00101"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSecretLetterQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSecretLetterQuestComplete()");
			m_bWaitResponse = true;
			SecretLetterQuestCompleteCommandBody cmdBody = new SecretLetterQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SecretLetterQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResSecretLetterQuestComplete(int nReturnCode, SecretLetterQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{			
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.ExploitPoint = resBody.exploitPoint;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.DailyExploitPoint = resBody.dailyExploitPoint;
			CsGameData.Instance.MyHeroInfo.ExploitPointDate = resBody.date;
			UpdateSecretLetterState(true); // 초기화.

			if (EventSecretLetterQuestComplete != null)
			{
				bool bLevelUp = (nOldLevel == resBody.level) ? false : true;
				EventSecretLetterQuestComplete(bLevelUp, resBody.acquiredExp, resBody.acquiredExploitPoint);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendSecretLetterPickStart()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendSecretLetterPickStart()");
			m_bWaitResponse = true;
			SecretLetterPickStartCommandBody cmdBody = new SecretLetterPickStartCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.SecretLetterPickStart, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResSecretLetterPickStart(int nReturnCode, SecretLetterPickStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bSecretLetterPick = true;
			
			if (EventSecretLetterPickStart != null)
			{
				EventSecretLetterPickStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A51_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 전투상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A51_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 탈것 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A51_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 다른 행동중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A51_ERROR_00304"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event
	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSecretLetterPickCompleted(SEBSecretLetterPickCompletedEventBody eventBody)
	{
		Debug.Log("OnEventEvtSecretLetterPickCompleted()");
		m_bSecretLetterPick = false;
		m_nPickCount = eventBody.pickCount;
		m_nPickedLetterGrade = eventBody.pickedLetterGrade;
		UpdateSecretLetterState();

		if (EventSecretLetterPickCompleted != null)
		{
			EventSecretLetterPickCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSecretLetterPickCanceled(SEBSecretLetterPickCanceledEventBody eventBody)
	{
		Debug.Log("OnEventEvtSecretLetterPickCanceled()");
		m_bSecretLetterPick = false;
		UpdateSecretLetterState();

		if (EventSecretLetterPickCanceled != null)
		{
			EventSecretLetterPickCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSecretLetterPickStarted(SEBHeroSecretLetterPickStartedEventBody eventBody) // 타영웅 뽑기 시작.
	{
		Debug.Log("OnEventEvtHeroSecretLetterPickStarted()");
		if (EventHeroSecretLetterPickStarted != null)
		{
			EventHeroSecretLetterPickStarted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSecretLetterPickCompleted(SEBHeroSecretLetterPickCompletedEventBody eventBody) // 타영웅 뽑기 완료.
	{
		Debug.Log("OnEventEvtHeroSecretLetterPickCompleted()");
		if (EventHeroSecretLetterPickCompleted != null)
		{
			EventHeroSecretLetterPickCompleted(eventBody.heroId, eventBody.pickedLetterGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSecretLetterPickCanceled(SEBHeroSecretLetterPickCanceledEventBody eventBody) // 타영웅 뽑기 취소.
	{
		Debug.Log("OnEventEvtHeroSecretLetterPickCanceled()");
		if (EventHeroSecretLetterPickCanceled != null)
		{
			EventHeroSecretLetterPickCanceled(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroSecretLetterQuestCompleted(SEBHeroSecretLetterQuestCompletedEventBody eventBody) // 타영웅 퀘스트 완료.
	{
		Debug.Log("OnEventEvtHeroSecretLetterQuestCompleted()");
		if (EventHeroSecretLetterQuestCompleted != null)
		{
			EventHeroSecretLetterQuestCompleted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtSecretLetterQuestTargetNationChanged(SEBSecretLetterQuestTargetNationChangedEventBody eventBody) // 자정 지나 대상 국가 변경.
	{
		Debug.Log("OnEventEvtSecretLetterQuestTargetNationChanged()");
		m_nSecretLetterQuestTargetNationId = eventBody.targetNationId;
	}

	#endregion Protocol.Event
}
