using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;

public enum EnMysteryBoxState { None = 0, Accepted = 1, Executed = 2, Completed = 3 }
public class CsMysteryBoxQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bMysteryBoxPick = false;
    bool m_bLowPickComplete = false;
    bool m_bInteractionButton = false;

	int m_nPickCount;
	int m_nPickedBoxGrade;

	int m_nDailyMysteryBoxQuestStartCount;
	DateTime m_dtDateMysteryBoxQuestStartCount;

	EnMysteryBoxState m_enMysteryBoxState = EnMysteryBoxState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsMysteryBoxQuestManager Instance
	{
		get { return CsSingleton<CsMysteryBoxQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui
	public event Delegate EventUpdateState;
	public event Delegate EventAcceptDialog;
	public event Delegate EventMissionDialog;
	public event Delegate EventNationTransmission;
	public event Delegate<bool> EventInteractionArea;
	public event Delegate EventMyHeroMysteryBoxPickStart;

	public event Delegate EventMyHeroMysteryBoxPickCancel;

	public event Delegate EventMysteryBoxQuestAccept;
	public event Delegate<bool, long, int> EventMysteryBoxQuestComplete;
	public event Delegate EventMysteryBoxPickStart;

	public event Delegate EventMysteryBoxPickCompleted;
	public event Delegate EventMysteryBoxPickCanceled;

	public event Delegate<Guid> EventHeroMysteryBoxPickStarted;
	public event Delegate<Guid, int> EventHeroMysteryBoxPickCompleted;
	public event Delegate<Guid> EventHeroMysteryBoxPickCanceled;
	public event Delegate<Guid> EventHeroMysteryBoxQuestCompleted;

    //---------------------------------------------------------------------------------------------------
    public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool MysteryBoxPick
	{
		get { return m_bMysteryBoxPick; }
	}

    public bool LowPickComplete
    {
        get { return m_bLowPickComplete; }
        set { m_bLowPickComplete = value; }
    }

    public bool InteractionButton
	{
		get { return m_bInteractionButton; }
	}

	public int PickCount
	{
		get { return m_nPickCount; }
	}

	public int PickedBoxGrade
	{
		get { return m_nPickedBoxGrade; }
	}

	public int DailyMysteryBoxQuestStartCount
	{
		get { return m_nDailyMysteryBoxQuestStartCount; }
		set { m_nDailyMysteryBoxQuestStartCount = value; }
	}

	public DateTime MysteryBoxQuestStartCountDate
	{
		get { return m_dtDateMysteryBoxQuestStartCount; }
		set { m_dtDateMysteryBoxQuestStartCount = value; }
	}

	public CsMysteryBoxQuest MysteryBoxQuest
	{
		get { return CsGameData.Instance.MysteryBoxQuest; }
		set { CsGameData.Instance.MysteryBoxQuest = value; }
	}

	public EnMysteryBoxState MysteryBoxState
	{
		get { return m_enMysteryBoxState; }
	} 

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroMysteryBoxQuest heroMysteryBoxQuest, int nDailyMysteryBoxQuestStartCount, DateTime dtDate)
	{
		UnInit();

		SetQuestInfo(heroMysteryBoxQuest, nDailyMysteryBoxQuestStartCount, dtDate);

		// Command
		CsRplzSession.Instance.EventResMysteryBoxQuestAccept += OnEventResMysteryBoxQuestAccept;
		CsRplzSession.Instance.EventResMysteryBoxQuestComplete += OnEventResMysteryBoxQuestComplete;
		CsRplzSession.Instance.EventResMysteryBoxPickStart += OnEventResMysteryBoxPickStart;

		// Event
		CsRplzSession.Instance.EventEvtMysteryBoxPickCompleted += OnEventEvtMysteryBoxPickCompleted;
		CsRplzSession.Instance.EventEvtMysteryBoxPickCanceled += OnEventEvtMysteryBoxPickCanceled;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickStarted += OnEventEvtHeroMysteryBoxPickStarted;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickCompleted += OnEventEvtHeroMysteryBoxPickCompleted;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickCanceled += OnEventEvtHeroMysteryBoxPickCanceled;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxQuestCompleted += OnEventEvtHeroMysteryBoxQuestCompleted;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResMysteryBoxQuestAccept -= OnEventResMysteryBoxQuestAccept;
		CsRplzSession.Instance.EventResMysteryBoxQuestComplete -= OnEventResMysteryBoxQuestComplete;
		CsRplzSession.Instance.EventResMysteryBoxPickStart -= OnEventResMysteryBoxPickStart;

		// Event
		CsRplzSession.Instance.EventEvtMysteryBoxPickCompleted -= OnEventEvtMysteryBoxPickCompleted;
		CsRplzSession.Instance.EventEvtMysteryBoxPickCanceled -= OnEventEvtMysteryBoxPickCanceled;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickStarted -= OnEventEvtHeroMysteryBoxPickStarted;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickCompleted -= OnEventEvtHeroMysteryBoxPickCompleted;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxPickCanceled -= OnEventEvtHeroMysteryBoxPickCanceled;
		CsRplzSession.Instance.EventEvtHeroMysteryBoxQuestCompleted -= OnEventEvtHeroMysteryBoxQuestCompleted;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_bMysteryBoxPick = false;
		m_bLowPickComplete = false;
		m_bInteractionButton = false;
		m_enMysteryBoxState = EnMysteryBoxState.None;
	}

	//---------------------------------------------------------------------------------------------------
	void SetQuestInfo(PDHeroMysteryBoxQuest heroMysteryBoxQuest, int nDailyMysteryBoxQuestStartCount, DateTime dtDate)
	{
		if (heroMysteryBoxQuest == null)
		{
			m_nPickCount = 0;
			m_nPickedBoxGrade = 0;
			UpdateMysteryBoxState(true);
		}
		else
		{
			m_nPickCount = heroMysteryBoxQuest.pickCount;
			m_nPickedBoxGrade = heroMysteryBoxQuest.pickedBoxGrade;
			UpdateMysteryBoxState();
		}
		
		m_nDailyMysteryBoxQuestStartCount = nDailyMysteryBoxQuestStartCount;
		m_dtDateMysteryBoxQuestStartCount = dtDate;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateMysteryBoxState(bool bReset = false)
	{
		if (bReset)
		{
            m_bLowPickComplete = false;
            m_nPickCount = 0;
            m_nPickedBoxGrade = 0;
            m_enMysteryBoxState = EnMysteryBoxState.None;
		}
		else
		{
			if (m_nPickCount == 0)
			{
				m_enMysteryBoxState = EnMysteryBoxState.Accepted;
			}
			else
			{
				if (m_nPickedBoxGrade == 5)
				{
					m_enMysteryBoxState = EnMysteryBoxState.Completed;
				}
				else
				{
					m_enMysteryBoxState = EnMysteryBoxState.Executed;
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
		if (CsGameData.Instance.MysteryBoxQuest == null) return;
		dd.d("CsMysteryBoxQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto);
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
		dd.d("CsMysteryBoxQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
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
		if (m_bWaitResponse) return false;
		if (m_enMysteryBoxState == EnMysteryBoxState.None && m_nDailyMysteryBoxQuestStartCount >= MysteryBoxQuest.LimitCount) return false;

		if (EventAcceptDialog != null)
		{
			dd.d("AcceptReadyOK");
			EventAcceptDialog();
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public bool MissionReadyOK()
	{
		if (m_bWaitResponse) return false;
		if (m_enMysteryBoxState == EnMysteryBoxState.Accepted || m_enMysteryBoxState == EnMysteryBoxState.Executed)
		{
			if (EventMissionDialog != null)
			{
				dd.d("CompleteReadyOK");
				EventMissionDialog();
			}
			return true;
		}
		return false;
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
		if (MysteryBoxQuest.TargetNpcInfo.NpcId == nNpcId)
		{
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartMysteryBoxPickStart()
	{
		if (EventMyHeroMysteryBoxPickStart != null)
		{
			EventMyHeroMysteryBoxPickStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsMysteryBoxQuestNpc(int nNpcId)
	{
		if (MysteryBoxQuest == null) return false;

		if (m_enMysteryBoxState == EnMysteryBoxState.None || m_enMysteryBoxState == EnMysteryBoxState.Executed || m_enMysteryBoxState == EnMysteryBoxState.Completed)
		{
			return (MysteryBoxQuest.QuestNpcInfo != null && MysteryBoxQuest.QuestNpcInfo.NpcId == nNpcId);
		}

		return false;
	}

	#region public.Event

	//---------------------------------------------------------------------------------------------------
	public void SendMysteryBoxPickCancel()
	{
		Debug.Log("SendMysteryBoxPickCancel()");
		m_bMysteryBoxPick = false;
		CEBMysteryBoxPickCancelEventBody csEvt = new CEBMysteryBoxPickCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.MysteryBoxPickCancel, csEvt);

        if (EventMyHeroMysteryBoxPickCancel != null)
		{
            EventMyHeroMysteryBoxPickCancel();
		}
	}

	#endregion public.Event

	#region Protocol.Command
	
	//---------------------------------------------------------------------------------------------------
	public void SendMysteryBoxQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendMysteryBoxQuestAccept()");
			m_bWaitResponse = true;
			MysteryBoxQuestAcceptCommandBody cmdBody = new MysteryBoxQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MysteryBoxQuestAccept, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMysteryBoxQuestAccept(int nReturnCode, MysteryBoxQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			SetQuestInfo(resBody.quest, resBody.dailyStartCount, resBody.date);

			if (EventMysteryBoxQuestAccept != null)
			{
				EventMysteryBoxQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A49_ERROR_00101"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendMysteryBoxQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendMysteryBoxQuestComplete()");
			m_bWaitResponse = true;
			MysteryBoxQuestCompleteCommandBody cmdBody = new MysteryBoxQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MysteryBoxQuestComplete, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMysteryBoxQuestComplete(int nReturnCode, MysteryBoxQuestCompleteResponseBody resBody)
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
			UpdateMysteryBoxState(true); // 초기화.

			if (EventMysteryBoxQuestComplete != null)
			{
				bool bLevelUp = (nOldLevel == resBody.level) ? false : true;
				EventMysteryBoxQuestComplete(bLevelUp, resBody.acquiredExp, resBody.acquiredExploitPoint);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendMysteryBoxPickStart()
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendMysteryBoxPickStart()");
			m_bWaitResponse = true;
			MysteryBoxPickStartCommandBody cmdBody = new MysteryBoxPickStartCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.MysteryBoxPickStart, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResMysteryBoxPickStart(int nReturnCode, MysteryBoxPickStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bMysteryBoxPick = true;

			if (EventMysteryBoxPickStart != null)
			{
				EventMysteryBoxPickStart();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A49_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 전투상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A49_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 탈것 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A49_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 다른 행동중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A49_ERROR_00304"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMysteryBoxPickCompleted(SEBMysteryBoxPickCompletedEventBody eventBody) // 의문의상자뽑기완료
	{
		Debug.Log("OnEventEvtMysteryBoxPickCompleted()");
		m_bMysteryBoxPick = false;
		m_nPickCount = eventBody.pickCount;
		m_nPickedBoxGrade = eventBody.pickedBoxGrade;
		UpdateMysteryBoxState();

		if (EventMysteryBoxPickCompleted != null)
		{
			EventMysteryBoxPickCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMysteryBoxPickCanceled(SEBMysteryBoxPickCanceledEventBody eventBody) // 의문의상자뽑기취소
	{
		Debug.Log("OnEventEvtMysteryBoxPickCanceled()");
		m_bMysteryBoxPick = false;
		UpdateMysteryBoxState();

		if (EventMysteryBoxPickCanceled != null)
		{
			EventMysteryBoxPickCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMysteryBoxPickStarted(SEBHeroMysteryBoxPickStartedEventBody eventBody) // 타영웅 의문의상자뽑기시작
	{
		Debug.Log("OnEventEvtHeroMysteryBoxPickStarted()");
		if (EventHeroMysteryBoxPickStarted != null)
		{
			EventHeroMysteryBoxPickStarted(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMysteryBoxPickCompleted(SEBHeroMysteryBoxPickCompletedEventBody eventBody) // 타영웅 의문의상자뽑기완료
	{
		Debug.Log("OnEventEvtHeroMysteryBoxPickCompleted()");
		if (EventHeroMysteryBoxPickCompleted != null)
		{
			EventHeroMysteryBoxPickCompleted(eventBody.heroId, eventBody.pickedBoxGrade);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMysteryBoxPickCanceled(SEBHeroMysteryBoxPickCanceledEventBody eventBody) // 타영웅 의문의상자뽑기취소
	{
		Debug.Log("OnEventEvtHeroMysteryBoxPickCanceled()");
		if (EventHeroMysteryBoxPickCanceled != null)
		{
			EventHeroMysteryBoxPickCanceled(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtHeroMysteryBoxQuestCompleted(SEBHeroMysteryBoxQuestCompletedEventBody eventBody)  // 타영웅 영웅의문의상자퀘스트완료
	{
		Debug.Log("OnEventEvtHeroMysteryBoxQuestCompleted()");
		if (EventHeroMysteryBoxQuestCompleted != null)
		{
			EventHeroMysteryBoxQuestCompleted(eventBody.heroId);
		}
	}

	#endregion Protocol.Event
}
