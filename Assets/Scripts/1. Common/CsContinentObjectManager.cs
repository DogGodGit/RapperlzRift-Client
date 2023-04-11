using ClientCommon;
using SimpleDebugLog;
using System;
using UnityEngine;

public enum EnInteractionState { None, ViewButton, interacting }
public enum EnInteractionQuestType { None = 0, Main, Daily, Weekly, GuildMission, GuildHunting, Sub , Biography , CreatureFarm }

public class CsContinentObjectManager
{
	//---------------------------------------------------------------------------------------------------
	public static CsContinentObjectManager Instance
	{
		get { return CsSingleton<CsContinentObjectManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	bool m_bWaitResponse = false;
	int m_nObjectId;
	long m_lInstanceId;
	CsContinentObject m_csContinentObject;
	Guid m_guidMyPlayer = Guid.Empty;
	EnInteractionState m_enInteractionState = EnInteractionState.None;
	EnInteractionQuestType m_enInteractionQuestType = EnInteractionQuestType.None;

	public event Delegate EventContinentObjectInteractionStart;
	public event Delegate<EnInteractionState> EventChangeInteractionState;
	public event Delegate<int, long> EventCreateInteractionObject;

	//My
	public event Delegate<long, EnInteractionQuestType, CsContinentObject> EventMyHeroContinentObjectInteractionStarted;             // 오브젝트 상호작용 사작 후
	public event Delegate EventMyHeroContinentObjectInteractionCancel;
	public event Delegate<long> EventMyHeroContinentObjectInteractionFinished;
	public event Delegate EventSendContinentObjectInteractionCancel;

	//Other
	public event Delegate<Guid, long> EventHeroContinentObjectInteractionStart;
	public event Delegate<Guid, long> EventHeroContinentObjectInteractionFinished;
	public event Delegate<Guid> EventHeroContinentObjectInteractionCancel;

	//---------------------------------------------------------------------------------------------------
	public Guid MyPlayerGuid
	{
		get { return m_guidMyPlayer; }
		set { m_guidMyPlayer = value; }
	}
	public int InteractionObjectId
	{
		get { return m_nObjectId; }
	}
	
	public long InteractionInstanceId
	{
		get { return m_lInstanceId; }
	}

	public bool IsInteractionStateNone
	{
		get { return m_enInteractionState == EnInteractionState.None; }
	}

	public bool IsInteractionStateViewButton
	{
		get { return m_enInteractionState == EnInteractionState.ViewButton; }
	}

	public bool Isinteracting
	{
		get { return m_enInteractionState == EnInteractionState.interacting; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsContinentObjectManager()
	{
		// Command
		CsRplzSession.Instance.EventResContinentObjectInteractionStart += OnEventResContinentObjectInteractionStart;

		// Event
		CsRplzSession.Instance.EventEvtContinentObjectCreated += OnEventEvtContinentObjectCreated;
		CsRplzSession.Instance.EventEvtHeroContinentObjectInteractionStart += OnEventEvtHeroContinentObjectInteractionStart;
		CsRplzSession.Instance.EventEvtHeroContinentObjectInteractionCancel += OnEventEvtHeroContinentObjectInteractionCancel;
		CsRplzSession.Instance.EventEvtHeroContinentObjectInteractionFinished += OnEventEvtHeroContinentObjectInteractionFinished;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		UnInit();
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_bWaitResponse = false;
		m_lInstanceId = 0;
		m_enInteractionState = EnInteractionState.None;
		m_guidMyPlayer = Guid.Empty;
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeInteractionState(EnInteractionState enNewInteractionState)
	{
		dd.d("ChangeInteractionState", m_nObjectId, m_lInstanceId, enNewInteractionState);
		if (enNewInteractionState == EnInteractionState.None)
		{
			m_nObjectId = 0;
			m_lInstanceId = 0;
			m_enInteractionQuestType = EnInteractionQuestType.None;
		}
		m_enInteractionState = enNewInteractionState;

		if (EventChangeInteractionState != null)
		{
			EventChangeInteractionState(m_enInteractionState);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ContinentObjectInteractionAble(bool bAble, int nObjectId, long lInstanceId)
	{
		if (m_enInteractionState == EnInteractionState.interacting) return;

		dd.d("ContinentObjectInteractionAble", bAble, nObjectId, lInstanceId, m_enInteractionState);
		if (bAble)
		{
			m_nObjectId = nObjectId;
			m_lInstanceId = lInstanceId;

			if (m_enInteractionState == EnInteractionState.None)
			{
				ChangeInteractionState(EnInteractionState.ViewButton);
			}
		}
		else
		{
			if (m_enInteractionState == EnInteractionState.ViewButton)
			{
				if (m_lInstanceId == lInstanceId)
				{
					ChangeInteractionState(EnInteractionState.None);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ContinentObjectInteractionStart()
	{
		Debug.Log("ContinentObjectInteractionStart");
		if (EventContinentObjectInteractionStart != null)
		{
			EventContinentObjectInteractionStart();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Interaction(long lInstanceId, EnInteractionQuestType enInteractionQuestType, CsContinentObject csContinentObject)
	{
		dd.d("Interaction", m_bWaitResponse, csContinentObject.ObjectId, lInstanceId, enInteractionQuestType);
		m_nObjectId = csContinentObject.ObjectId;
		m_lInstanceId = lInstanceId;
		m_csContinentObject = csContinentObject;
		m_enInteractionQuestType = enInteractionQuestType;
		SendContinentObjectInteractionStart(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	public void TryContinentObjectInteractionCancel()
	{
		Debug.Log("TryContinentObjectInteractionCancel()");
		ChangeInteractionState(EnInteractionState.None);

		SendContinentObjectInteractionCancel();

		if (EventSendContinentObjectInteractionCancel != null)
		{
			EventSendContinentObjectInteractionCancel(); // ui
		}
	}

	#region Public Event

	//---------------------------------------------------------------------------------------------------
	void SendContinentObjectInteractionCancel() // 상호작용 취소.
	{
		CEBContinentObjectInteractionCancelEventBody csEvt = new CEBContinentObjectInteractionCancelEventBody();
		CsRplzSession.Instance.Send(ClientEventName.ContinentObjectInteractionCancel, csEvt);
	}

	#endregion Public Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 대륙오브젝트상호작용시작
	void SendContinentObjectInteractionStart(long lInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ContinentObjectInteractionStartCommandBody cmdBody = new ContinentObjectInteractionStartCommandBody();
			cmdBody.instanceId = lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.ContinentObjectInteractionStart, cmdBody);
		}
	}
	
	void OnEventResContinentObjectInteractionStart(int nReturnCode, ContinentObjectInteractionStartResponseBody responseBody)
	{
		dd.d("OnEventResContinentObjectInteractionStart", nReturnCode);
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{			
			ChangeInteractionState(EnInteractionState.interacting);

			if (EventMyHeroContinentObjectInteractionStarted != null)
			{
				EventMyHeroContinentObjectInteractionStarted(m_lInstanceId, m_enInteractionQuestType, m_csContinentObject);
			}
		}
		else
		{
			if (nReturnCode == 101)	// 영웅이 죽은 상태입니다.
			{
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("INTER_ERROR_00101"));
			}
			else if (nReturnCode == 102) //  오브젝트가 존재하지 않습니다.
			{
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("INTER_ERROR_00102"));
			}
			else if (nReturnCode == 103) // 상호작용할 수 있는 위치가 아닙니다.
			{
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("INTER_ERROR_00103"));
			}
			else if (nReturnCode == 104) // 상호작용할 수 없습니다.
			{
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("INTER_ERROR_00104"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}

			if (EventMyHeroContinentObjectInteractionCancel != null)
			{
				EventMyHeroContinentObjectInteractionCancel();
			}
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 대륙오브젝트생성
	void OnEventEvtContinentObjectCreated(SEBContinentObjectCreatedEventBody eventBody)
	{
		dd.d("OnEventEvtContinentObjectCreated ", eventBody.arrangeNo, eventBody.continentObjectInstanceId);
		if (EventCreateInteractionObject != null)
		{
			EventCreateInteractionObject(eventBody.arrangeNo, eventBody.continentObjectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅대륙오브젝트상호작용시작 (other)
	void OnEventEvtHeroContinentObjectInteractionStart(SEBHeroContinentObjectInteractionStartEventBody eventBody)
	{
		dd.d("OnEventEvtHeroContinentObjectInteractionStart ", eventBody.continentObjectInstanceId);		
		if (EventHeroContinentObjectInteractionStart != null)
		{
			EventHeroContinentObjectInteractionStart(eventBody.heroId, eventBody.continentObjectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅대륙오브젝트상호작용취소 (my, other)
	void OnEventEvtHeroContinentObjectInteractionCancel(SEBHeroContinentObjectInteractionCancelEventBody eventBody)
	{
		dd.d("OnEventEvtHeroContinentObjectInteractionCancel");

		if (m_guidMyPlayer == eventBody.heroId) // My
		{
			ChangeInteractionState(EnInteractionState.None);

			if (EventMyHeroContinentObjectInteractionCancel != null)
			{
				EventMyHeroContinentObjectInteractionCancel();  // MyHero 수신 처리 필요.
			}
		}
		else // Other
		{
			if (EventHeroContinentObjectInteractionCancel != null)
			{
				EventHeroContinentObjectInteractionCancel(eventBody.heroId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅대륙오브젝트상호작용완료 (my, other)
	void OnEventEvtHeroContinentObjectInteractionFinished(SEBHeroContinentObjectInteractionFinishedEventBody eventBody)
	{
		dd.d("OnEventEvtHeroContinentObjectInteractionFinished ", eventBody.continentObjectInstanceId);		
		if (m_guidMyPlayer == eventBody.heroId)
		{
			ChangeInteractionState(EnInteractionState.None);
			if (EventMyHeroContinentObjectInteractionFinished != null)
			{
				EventMyHeroContinentObjectInteractionFinished(eventBody.continentObjectInstanceId);
			}
		}
		else
		{
			if (EventHeroContinentObjectInteractionFinished != null)
			{
				EventHeroContinentObjectInteractionFinished(eventBody.heroId, eventBody.continentObjectInstanceId);
			}
		}
	}

	#endregion Protocol.Event

}
