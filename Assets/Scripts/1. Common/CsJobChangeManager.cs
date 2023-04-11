using ClientCommon;
using System;
using UnityEngine;

public class CsJobChangeManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;

	CsHeroJobChangeQuest m_csHeroJobChangeQuest;

	int m_nTargetJobId;

	//---------------------------------------------------------------------------------------------------
	public static CsJobChangeManager Instance
	{
		get { return CsSingleton<CsJobChangeManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object> EventStopAutoPlay;
	public event Delegate<int> EventNpcStartDialog;
	public event Delegate EventNpcEndDialog;

	public event Delegate EventJobChangeQuestAccept;
	public event Delegate EventJobChangeQuestComplete;
	public event Delegate EventHeroJobChange;

	public event Delegate EventJobChangeQuestProgressCountUpdated;
	public event Delegate EventJobChangeQuestMonsterSpawned;
	public event Delegate EventJobChangeQuestFailed;
	public event Delegate<Guid, int> EventHeroJobChanged;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public CsHeroJobChangeQuest HeroJobChangeQuest
	{
		get { return m_csHeroJobChangeQuest; }
	}


	public EnJobChangeQuestStaus JobChangeState
	{
		get { return m_csHeroJobChangeQuest == null ? EnJobChangeQuestStaus.None : (EnJobChangeQuestStaus)m_csHeroJobChangeQuest.Status; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroJobChangeQuest heroJobChangeQuest)
	{
		UnInit();
		Debug.Log("CsJobChangeManager.Init() : "+ heroJobChangeQuest);
		if (heroJobChangeQuest != null)
		{
			m_csHeroJobChangeQuest = new CsHeroJobChangeQuest(heroJobChangeQuest);
			Debug.Log("CsJobChangeManager.Init() : " + (EnJobChangeQuestStaus)m_csHeroJobChangeQuest.Status);
		}

		// Command
		CsRplzSession.Instance.EventResJobChangeQuestAccept += OnEventResJobChangeQuestAccept;
		CsRplzSession.Instance.EventResJobChangeQuestComplete += OnEventResJobChangeQuestComplete;
		CsRplzSession.Instance.EventResHeroJobChange += OnEventResHeroJobChange;

		// Event
		CsRplzSession.Instance.EventEvtJobChangeQuestProgressCountUpdated += OnEventEvtJobChangeQuestProgressCountUpdated;
		CsRplzSession.Instance.EventEvtJobChangeQuestMonsterSpawned += OnEventEvtJobChangeQuestMonsterSpawned;
		CsRplzSession.Instance.EventEvtJobChangeQuestFailed += OnEventEvtJobChangeQuestFailed;
		CsRplzSession.Instance.EventEvtHeroJobChanged += OnEventEvtHeroJobChanged;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResJobChangeQuestAccept -= OnEventResJobChangeQuestAccept;
		CsRplzSession.Instance.EventResJobChangeQuestComplete -= OnEventResJobChangeQuestComplete;
		CsRplzSession.Instance.EventResHeroJobChange -= OnEventResHeroJobChange;

		// Event
		CsRplzSession.Instance.EventEvtJobChangeQuestProgressCountUpdated -= OnEventEvtJobChangeQuestProgressCountUpdated;
		CsRplzSession.Instance.EventEvtJobChangeQuestMonsterSpawned -= OnEventEvtJobChangeQuestMonsterSpawned;
		CsRplzSession.Instance.EventEvtJobChangeQuestFailed -= OnEventEvtJobChangeQuestFailed;
		CsRplzSession.Instance.EventEvtHeroJobChanged -= OnEventEvtHeroJobChanged;

		m_bWaitResponse = false;
		m_csHeroJobChangeQuest = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;

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
	public bool IsInteractionQuest(int nObjectId)
	{
		if (JobChangeState == EnJobChangeQuestStaus.Accepted)
		{
			CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(m_csHeroJobChangeQuest.QuestNo);
			if (csJobChangeQuest != null)
			{
				if (csJobChangeQuest.Type == 2)
				{
					if (m_csHeroJobChangeQuest.ProgressCount < csJobChangeQuest.TargetCount)
					{
						if (csJobChangeQuest.TargetContinentObject != null && csJobChangeQuest.TargetContinentObject.ObjectId == nObjectId)
						{
							if (csJobChangeQuest.IsTargetOwnNation)
							{
								if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
								{
									return true;
								}
							}
							else
							{
								if (CsGameData.Instance.MyHeroInfo.Nation.NationId != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
								{
									return true;
								}
							}
						}
					}
				}
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool NpcDialogReadyOK(int nNpcId)
	{
        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.JobChangeRequiredHeroLevel)
        {
            return false;
        }

		if (JobChangeState == EnJobChangeQuestStaus.None || JobChangeState == EnJobChangeQuestStaus.Completed) // 퀘스트수락전 or 완료후 재수락
		{
			int nQuestNo = m_csHeroJobChangeQuest == null ? 1 : m_csHeroJobChangeQuest.QuestNo + 1;
			CsJobChangeQuest JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(nQuestNo);
			if (JobChangeQuest != null )
			{
				if (JobChangeQuest.QuestNpc != null && JobChangeQuest.QuestNpc.NpcId == nNpcId)
				{
					if (EventNpcStartDialog != null)
					{
						EventNpcStartDialog(nQuestNo);
					}
					return true;
				}
			}
		}
		else
		{
			CsJobChangeQuest JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(m_csHeroJobChangeQuest.QuestNo);
			if (JobChangeQuest != null && JobChangeQuest.QuestNpc.NpcId == nNpcId)
			{
				if (JobChangeState == EnJobChangeQuestStaus.Failed)
				{
					if (EventNpcStartDialog != null)
					{
						EventNpcStartDialog(m_csHeroJobChangeQuest.QuestNo);
					}
					return true;
				}
				else if (JobChangeState == EnJobChangeQuestStaus.Accepted)
				{
					if (m_csHeroJobChangeQuest.ProgressCount >= CsGameData.Instance.GetJobChangeQuest(m_csHeroJobChangeQuest.QuestNo).TargetCount)  // 미션 완료 가능상태.
					{
						if (EventNpcEndDialog != null)
						{
							EventNpcEndDialog();
						}
						return true;
					}
				}
			}
		}

		return false;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 전직퀘스트수락
	public void SendJobChangeQuestAccept(int nQuestNo, int nDifficulty)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			JobChangeQuestAcceptCommandBody cmdBody = new JobChangeQuestAcceptCommandBody();
			cmdBody.questNo = nQuestNo;
			cmdBody.difficulty = nDifficulty;
			CsRplzSession.Instance.Send(ClientCommandName.JobChangeQuestAccept, cmdBody);
		}
	}

	void OnEventResJobChangeQuestAccept(int nReturnCode, JobChangeQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResJobChangeQuestAccept     nReturnCode : " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_csHeroJobChangeQuest = new CsHeroJobChangeQuest(resBody.quest);			

			if (EventJobChangeQuestAccept != null)
			{
				EventJobChangeQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 퀘스트를 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 퀘스트NPC와 상호작용할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 길드에 가입하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전직퀘스트완료
	public void SendJobChangeQuestComplete(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			JobChangeQuestCompleteCommandBody cmdBody = new JobChangeQuestCompleteCommandBody();
			cmdBody.instanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.JobChangeQuestComplete, cmdBody);
		}
	}

	void OnEventResJobChangeQuestComplete(int nReturnCode, JobChangeQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResJobChangeQuestComplete     nReturnCode : " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);
			m_csHeroJobChangeQuest.Status = (int)EnJobChangeQuestStaus.Completed;

			if (EventJobChangeQuestComplete != null)
			{
				EventJobChangeQuestComplete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 진행중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 퀘스트NPC와 상호작용할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A153_ERROR_00204"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅전직
	public void SendHeroJobChange(int nTargetJobId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			HeroJobChangeCommandBody cmdBody = new HeroJobChangeCommandBody();
			cmdBody.targetJobId = m_nTargetJobId = nTargetJobId;
			CsRplzSession.Instance.Send(ClientCommandName.HeroJobChange, cmdBody);
		}
	}

	void OnEventResHeroJobChange(int nReturnCode, HeroJobChangeResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);
			CsGameData.Instance.MyHeroInfo.JobId = m_nTargetJobId;

			if (EventHeroJobChange != null)
			{
				EventHeroJobChange();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A154_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A154_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 전직퀘스트진행카운트갱신
	void OnEventEvtJobChangeQuestProgressCountUpdated(SEBJobChangeQuestProgressCountUpdatedEventBody eventBody)
	{
		m_csHeroJobChangeQuest.InstanceId = eventBody.instanceId;
		m_csHeroJobChangeQuest.ProgressCount = eventBody.progressCount;
		Debug.Log("OnEventEvtJobChangeQuestProgressCountUpdated : " + m_csHeroJobChangeQuest.ProgressCount);
		if (EventJobChangeQuestProgressCountUpdated != null)
		{
			EventJobChangeQuestProgressCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전직퀘스트몬스터스폰
	void OnEventEvtJobChangeQuestMonsterSpawned(SEBJobChangeQuestMonsterSpawnedEventBody eventBody)
	{
		m_csHeroJobChangeQuest.MonsterInstanceId = eventBody.instanceId;
		m_csHeroJobChangeQuest.MonsterPosition = new Vector3(eventBody.position.x, eventBody.position.y, eventBody.position.z);
		m_csHeroJobChangeQuest.RemainingTime = eventBody.remainingLifetime;

		if (EventJobChangeQuestMonsterSpawned != null)
		{
			EventJobChangeQuestMonsterSpawned();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전직퀘스트실패
	void OnEventEvtJobChangeQuestFailed(SEBJobChangeQuestFailedEventBody eventBody)
	{
		if (m_csHeroJobChangeQuest.InstanceId == eventBody.instanceId)
		{
			m_csHeroJobChangeQuest.Status = (int)EnJobChangeQuestStaus.Failed;
		}

		if (EventJobChangeQuestFailed != null)
		{
			EventJobChangeQuestFailed();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅전직
	void OnEventEvtHeroJobChanged(SEBHeroJobChangedEventBody eventBody)
	{
		if (EventHeroJobChanged != null)
		{
			EventHeroJobChanged(eventBody.heroId, eventBody.jobId);
		}
	}

	#endregion Protocol.Event
}
