using ClientCommon;
using SimpleDebugLog;
using System;
using UnityEngine;


public enum EnDimensionRaidState { None = 0, Accepted = 1, Completed = 2 }
public class CsDimensionRaidQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bInteraction = false;
	bool m_bInteractionButton = false;
	int m_nStep;

	int m_nDailyDimensionRaidQuestStartCount;
	DateTime m_dtDateDimensionRaidQuestStartCount;
	EnDimensionRaidState m_enDimensionRaidState = EnDimensionRaidState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsDimensionRaidQuestManager Instance
	{
		get { return CsSingleton<CsDimensionRaidQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui
	public event Delegate EventUpdateState;
	public event Delegate EventNpctDialog;
	public event Delegate EventMissionDialog;
	public event Delegate EventNationTransmission;
	public event Delegate<bool> EventInteractionArea;
	public event Delegate EventMyHeroDimensionRaidInteractionStart;

	public event Delegate EventMyHeroDimensionRaidInteractionCancel;

	public event Delegate EventDimensionRaidQuestAccept;
	public event Delegate<bool, long, int> EventDimensionRaidQuestComplete;
	public event Delegate EventDimensionRaidInteractionStart;

	public event Delegate EventDimensionRaidInteractionCompleted;
	public event Delegate EventDimensionRaidInteractionCanceled;

	public event Delegate<Guid> EventHeroDimensionRaidInteractionStarted;
	public event Delegate<Guid> EventHeroDimensionRaidInteractionCompleted;
	public event Delegate<Guid> EventHeroDimensionRaidInteractionCanceled;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool Interaction
	{
		get { return m_bInteraction; }
	}

	public int Step
	{
		get { return m_nStep; }
	}

	public bool InteractionButton
	{
		get { return m_bInteractionButton; }
		set { m_bInteractionButton = value; }
	}

	public int DailyDimensionRaidQuestStartCount
	{
		get { return m_nDailyDimensionRaidQuestStartCount; }
		set { m_nDailyDimensionRaidQuestStartCount = value; }
	}

	public DateTime DimensionRaidQuestStartCountDate
	{
		get { return m_dtDateDimensionRaidQuestStartCount; }
		set { m_dtDateDimensionRaidQuestStartCount = value; }
	}

	public CsDimensionRaidQuest DimensionRaidQuest
	{
		get { return CsGameData.Instance.DimensionRaidQuest; }
		set { CsGameData.Instance.DimensionRaidQuest = value; }
	}

	public EnDimensionRaidState DimensionRaidState
	{
		get { return m_enDimensionRaidState; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroDimensionRaidQuest heroDimensionRaidQuest, int nDailyDimensionRaidQuestStartCount, DateTime dtDate)
	{
		UnInit();

		SetQuestInfo(heroDimensionRaidQuest, nDailyDimensionRaidQuestStartCount, dtDate);

		// Command 
		CsRplzSession.Instance.EventResDimensionRaidQuestAccept += OnEventResDimensionRaidQuestAccept;
		CsRplzSession.Instance.EventResDimensionRaidQuestComplete += OnEventResDimensionRaidQuestComplete;
		CsRplzSession.Instance.EventResDimensionRaidInteractionStart += OnEventResDimensionRaidInteractionStart;

		// Event
		CsRplzSession.Instance.EventEvtDimensionRaidInteractionCompleted += OnEventEvtDimensionRaidInteractionCompleted;
		CsRplzSession.Instance.EventEvtDimensionRaidInteractionCanceled += OnEventEvtDimensionRaidInteractionCanceled;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionStarted += OnEventEvtHeroDimensionRaidInteractionStarted;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionCompleted += OnEventEvtHeroDimensionRaidInteractionCompleted;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionCanceled += OnEventEvtHeroDimensionRaidInteractionCanceled;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command 
		CsRplzSession.Instance.EventResDimensionRaidQuestAccept -= OnEventResDimensionRaidQuestAccept;
		CsRplzSession.Instance.EventResDimensionRaidQuestComplete -= OnEventResDimensionRaidQuestComplete;
		CsRplzSession.Instance.EventResDimensionRaidInteractionStart -= OnEventResDimensionRaidInteractionStart;

		// Event
		CsRplzSession.Instance.EventEvtDimensionRaidInteractionCompleted -= OnEventEvtDimensionRaidInteractionCompleted;
		CsRplzSession.Instance.EventEvtDimensionRaidInteractionCanceled -= OnEventEvtDimensionRaidInteractionCanceled;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionStarted -= OnEventEvtHeroDimensionRaidInteractionStarted;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionCompleted -= OnEventEvtHeroDimensionRaidInteractionCompleted;
		CsRplzSession.Instance.EventEvtHeroDimensionRaidInteractionCanceled -= OnEventEvtHeroDimensionRaidInteractionCanceled;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_bInteraction = false;
		m_bInteractionButton = false;
		m_nStep = 0;

		m_nDailyDimensionRaidQuestStartCount = 0;
		m_dtDateDimensionRaidQuestStartCount = DateTime.Now;
		m_enDimensionRaidState = EnDimensionRaidState.None;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetQuestInfo(PDHeroDimensionRaidQuest heroDimensionRaidQuest, int nDailyDimensionRaidQuestStartCount, DateTime dtDate)
	{
		if (heroDimensionRaidQuest == null)
		{
			UpdateDimensionRaidState(true);
		}
		else
		{
			m_nStep = heroDimensionRaidQuest.step;
			UpdateDimensionRaidState();
		}

		m_nDailyDimensionRaidQuestStartCount = nDailyDimensionRaidQuestStartCount;
		m_dtDateDimensionRaidQuestStartCount = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDimensionRaidState(bool bReset = false)
	{

		if (bReset)
		{
			m_nStep = -1;
			m_enDimensionRaidState = EnDimensionRaidState.None;
		}
		else
		{
			if (m_nStep == -1) // 수락전.
			{
				m_enDimensionRaidState = EnDimensionRaidState.None;
			}
			else // 수락후
			{
				if (m_nStep == 0) // 완료
				{
					m_enDimensionRaidState = EnDimensionRaidState.Completed;
				}
				else
				{
					m_enDimensionRaidState = EnDimensionRaidState.Accepted;
				}
			}
		}

		dd.d("UpdateDimensionRaidState           DimensionRaidState = ",  m_enDimensionRaidState);

		if (EventUpdateState != null)
		{
			EventUpdateState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;
		if (CsGameData.Instance.DimensionRaidQuest == null) return; // 더이상 진행할 퀘스트가 없을때.

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
		dd.d("CsBountyHunterQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
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
	public bool NpctDialog()
	{
		Debug.Log("1. NpctDialog");
		if (m_bWaitResponse) return false;
		if (m_enDimensionRaidState == EnDimensionRaidState.None && m_nDailyDimensionRaidQuestStartCount >= DimensionRaidQuest.LimitCount) return false;

		if (EventNpctDialog != null)
		{
			Debug.Log("2. NpctDialog");
			EventNpctDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionReadyOK()
	{
		if (EventMissionDialog != null)
		{
			EventMissionDialog();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NationTransmissionReadyOK()
	{
		if (m_bWaitResponse) return;

		if (EventNationTransmission != null)
		{
			EventNationTransmission();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckInteractionNpc(int nNpcId)
	{
		if (m_nStep <= 0) return false;
		if (DimensionRaidQuest.GetDimensionRaidQuestStep(m_nStep).TargetNpcInfo.NpcId == nNpcId)
		{
			return true;
		}
		return false;
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
	public void StartDimensionRaidInteractionStart()
	{
		if (EventMyHeroDimensionRaidInteractionStart != null)
		{
			EventMyHeroDimensionRaidInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionStart()
	{
		SendDimensionRaidInteractionStart();
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsMysteryBoxQuestNpc(int nNpcId)
	{
		if (DimensionRaidQuest == null) return false;

		if (m_enDimensionRaidState == EnDimensionRaidState.None || m_enDimensionRaidState == EnDimensionRaidState.Completed)
		{
			return (DimensionRaidQuest.QuestNpcInfo != null && DimensionRaidQuest.QuestNpcInfo.NpcId == nNpcId);
		}

		return false;
	}

	#region public.Event

	//---------------------------------------------------------------------------------------------------
	public void SendDimensionRaidInteractionCancel()
	{
		Debug.Log("SendDimensionRaidInteractionCancel()");
		m_bInteraction = false;
		CEBDimensionRaidInteractionCancelEventBody csEvt = new CEBDimensionRaidInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.DimensionRaidInteractionCancel, csEvt);
		if (EventMyHeroDimensionRaidInteractionCancel != null)
		{
			EventMyHeroDimensionRaidInteractionCancel();
		}
	}

	#endregion public.Event

	#region Protocol.Command
	//---------------------------------------------------------------------------------------------------
	public void SendDimensionRaidQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DimensionRaidQuestAcceptCommandBody cmdBody = new DimensionRaidQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DimensionRaidQuestAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResDimensionRaidQuestAccept(int nReturnCode, DimensionRaidQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nStep = resBody.quest.step;
			m_nDailyDimensionRaidQuestStartCount = resBody.dailyStartCount;

			UpdateDimensionRaidState();
			
			if (EventDimensionRaidQuestAccept != null)
			{
				EventDimensionRaidQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A52_ERROR_00101"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendDimensionRaidQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DimensionRaidQuestCompleteCommandBody cmdBody = new DimensionRaidQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DimensionRaidQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResDimensionRaidQuestComplete(int nReturnCode, DimensionRaidQuestCompleteResponseBody resBody)
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
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			UpdateDimensionRaidState(true);

			if (EventDimensionRaidQuestComplete != null)
			{
				bool bLevelUp = (nOldLevel == resBody.level) ? false : true;
				EventDimensionRaidQuestComplete(bLevelUp, resBody.acquiredExp, resBody.acquiredExploitPoint);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SendDimensionRaidInteractionStart()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DimensionRaidInteractionStartCommandBody cmdBody = new DimensionRaidInteractionStartCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DimensionRaidInteractionStart, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResDimensionRaidInteractionStart(int nReturnCode, DimensionRaidInteractionStartResponseBody resBody) // 차원습격상호작용시작
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bInteraction = true;
			if (EventDimensionRaidInteractionStart != null)
			{
				EventDimensionRaidInteractionStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A52_ERROR_00301"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 탈것 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A52_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 다른 행동중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A52_ERROR_00304"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDimensionRaidInteractionCompleted(SEBDimensionRaidInteractionCompletedEventBody eventBody) // 차원습격상호작용완료
	{
		Debug.Log("OnEventEvtDimensionRaidInteractionCompleted     eventBody.nextStep = " + eventBody.nextStep);
		m_bInteraction = false;
		m_nStep = eventBody.nextStep;
	
		UpdateDimensionRaidState();

		if (EventDimensionRaidInteractionCompleted != null)
		{
			EventDimensionRaidInteractionCompleted();
		}									 
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDimensionRaidInteractionCanceled(SEBDimensionRaidInteractionCanceledEventBody eventBody) // 차원습격상호작용취소
	{
		m_bInteraction = false;

		UpdateDimensionRaidState();

		if (EventDimensionRaidInteractionCanceled != null)
		{
			EventDimensionRaidInteractionCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDimensionRaidInteractionStarted(SEBHeroDimensionRaidInteractionStartedEventBody eventBody) // 당사자외 차원습격상호작용시작
	{
		if (EventHeroDimensionRaidInteractionStarted != null)
		{
			EventHeroDimensionRaidInteractionStarted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDimensionRaidInteractionCompleted(SEBHeroDimensionRaidInteractionCompletedEventBody eventBody) // 당사자외 차원습격상호작용완료
	{
		if (EventHeroDimensionRaidInteractionCompleted != null)
		{
			EventHeroDimensionRaidInteractionCompleted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroDimensionRaidInteractionCanceled(SEBHeroDimensionRaidInteractionCanceledEventBody eventBody) // 당사자외 차원습격상호작용취소
	{
		if (EventHeroDimensionRaidInteractionCanceled != null)
		{
			EventHeroDimensionRaidInteractionCanceled(eventBody.heroId);
		}
	}

	#endregion Protocol.Event
}
