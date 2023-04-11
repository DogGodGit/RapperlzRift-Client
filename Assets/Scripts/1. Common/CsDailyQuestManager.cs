using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;

public class CsDailyQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	Guid m_guidAutoQuestId;

	int m_nDailyQuestAcceptionCount;
	DateTime m_dtQuestAcceptionCount;
	int m_nDailyQuestFreeRefreshCount;
	DateTime m_dtQuestFreeRefreshCount;

	List<CsHeroDailyQuest> m_listCsHeroDailyQuest = new List<CsHeroDailyQuest>();

	Guid m_guidQuestId;

	//---------------------------------------------------------------------------------------------------
	public static CsDailyQuestManager Instance
	{
		get { return CsSingleton<CsDailyQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<Guid> EventStartAutoPlay;
	public event Delegate<object, Guid> EventStopAutoPlay;

	// Command
	public event Delegate EventDailyQuestAccept;
	public event Delegate<bool, long, int> EventDailyQuestComplete;
	public event Delegate EventDailyQuestRefresh;
	public event Delegate EventDailyQuestMissionImmediatlyComplete;
	public event Delegate<int> EventDailyQuestAbandon;

	// Event
	public event Delegate EventHeroDailyQuestProgressCountUpdated;
	public event Delegate EventHeroDailyQuestCreated;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public Guid QuestId
	{
		get { return m_guidQuestId; }
	}

	public Guid AutoQuestId
	{
		get { return m_guidAutoQuestId; }
	}

	public int DailyQuestAcceptionCount
	{
		get { return m_nDailyQuestAcceptionCount; }
		set { m_nDailyQuestAcceptionCount = value; }
	}

	public DateTime QuestAcceptionCountDate
	{
		get { return m_dtQuestAcceptionCount; }
		set { m_dtQuestAcceptionCount = value; }
	}

	public int DailyQuestFreeRefreshCount
	{
		get { return m_nDailyQuestFreeRefreshCount; }
		set { m_nDailyQuestFreeRefreshCount = value; }
	}

	public DateTime QuestFreeRefreshCountDate
	{
		get { return m_dtQuestFreeRefreshCount; }
		set { m_dtQuestFreeRefreshCount = value; }
	}

	public List<CsHeroDailyQuest> HeroDailyQuestList
	{
		get { return m_listCsHeroDailyQuest; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(int nDailyQuestAcceptionCount, int nDailyQuestFreeRefreshCount, PDHeroDailyQuest[] dailyQuests, DateTime dtDate)
	{
		UnInit();

		m_nDailyQuestAcceptionCount = nDailyQuestAcceptionCount;
		m_nDailyQuestFreeRefreshCount = nDailyQuestFreeRefreshCount;
		m_dtQuestFreeRefreshCount = m_dtQuestAcceptionCount = dtDate;

		m_listCsHeroDailyQuest = new List<CsHeroDailyQuest>(CsGameData.Instance.DailyQuest.SlotCount);

        for (int i = 0; i < CsGameData.Instance.DailyQuest.SlotCount; i++)
        {
            CsHeroDailyQuest csHeroDailyQuest = new CsHeroDailyQuest();
            m_listCsHeroDailyQuest.Add(csHeroDailyQuest);
        }
        
		for (int i = 0; i < dailyQuests.Length; i++)
		{
            AddHeroDailyQuest(dailyQuests[i]);
		}

		// Command
		CsRplzSession.Instance.EventResDailyQuestAccept += OnEventResDailyQuestAccept;
		CsRplzSession.Instance.EventResDailyQuestComplete += OnEventResDailyQuestComplete;
		CsRplzSession.Instance.EventResDailyQuestRefresh += OnEventResDailyQuestRefresh;
		CsRplzSession.Instance.EventResDailyQuestMissionImmediatlyComplete += OnEventResDailyQuestMissionImmediatlyComplete;
		CsRplzSession.Instance.EventResDailyQuestAbandon += OnEventResDailyQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtHeroDailyQuestProgressCountUpdated += OnEventEvtHeroDailyQuestProgressCountUpdated;
		CsRplzSession.Instance.EventEvtHeroDailyQuestCreated += OnEventEvtHeroDailyQuestCreated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResDailyQuestAccept -= OnEventResDailyQuestAccept;
		CsRplzSession.Instance.EventResDailyQuestComplete -= OnEventResDailyQuestComplete;
		CsRplzSession.Instance.EventResDailyQuestRefresh -= OnEventResDailyQuestRefresh;
		CsRplzSession.Instance.EventResDailyQuestMissionImmediatlyComplete -= OnEventResDailyQuestMissionImmediatlyComplete;
		CsRplzSession.Instance.EventResDailyQuestAbandon -= OnEventResDailyQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtHeroDailyQuestProgressCountUpdated -= OnEventEvtHeroDailyQuestProgressCountUpdated;
		CsRplzSession.Instance.EventEvtHeroDailyQuestCreated -= OnEventEvtHeroDailyQuestCreated;

		m_bWaitResponse = false;
		m_bAuto = false;
		m_guidAutoQuestId = Guid.Empty;
		m_guidQuestId = Guid.Empty;
				
		m_nDailyQuestAcceptionCount = 0;
		m_nDailyQuestFreeRefreshCount = 0;
		m_listCsHeroDailyQuest.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	void AddHeroDailyQuest(PDHeroDailyQuest heroDailyQuest)
	{
        CsHeroDailyQuest csHeroDailyQuest = new CsHeroDailyQuest(heroDailyQuest);
		m_listCsHeroDailyQuest[csHeroDailyQuest.SlotIndex] = csHeroDailyQuest;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay(Guid guidAutoQuestId)
	{
		dd.d("CsDailyQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto, guidAutoQuestId);
		if (m_bWaitResponse) return;
		if (m_listCsHeroDailyQuest == null) return;

		CsHeroDailyQuest csHeroDailyQuest = GetHeroDailyQuest(guidAutoQuestId);
		if (csHeroDailyQuest == null) return;

		if (m_bAuto == false || m_guidAutoQuestId != guidAutoQuestId)
		{
			m_bAuto = true;
			m_guidAutoQuestId = guidAutoQuestId;

			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay(m_guidAutoQuestId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller, Guid guidAutoQuestId)
	{
		dd.d("CsDailyQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
		if (m_bAuto)
		{
			m_bAuto = false;

			if (m_guidAutoQuestId == guidAutoQuestId)
			{
				m_guidAutoQuestId = Guid.Empty;
			}

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller, guidAutoQuestId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroDailyQuest GetHeroDailyQuest(Guid guidId)
	{
		for (int i = 0; i < m_listCsHeroDailyQuest.Count; i++)
		{
			if (m_listCsHeroDailyQuest[i].Id == guidId)
				return m_listCsHeroDailyQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroDailyQuest GetHeroDailyQuestTypeInteraction(int nObjectId)
	{
		for (int i = 0; i < m_listCsHeroDailyQuest.Count; i++)
		{
			if (m_listCsHeroDailyQuest[i].DailyQuestMission != null)
			{
				if (m_listCsHeroDailyQuest[i].DailyQuestMission.TargetContinentObject != null)
				{
					if (m_listCsHeroDailyQuest[i].DailyQuestMission.TargetContinentObject.ObjectId == nObjectId)
					{
						return m_listCsHeroDailyQuest[i];
					}
				}
			}			
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsDailyQuestObject(int nObjectId)
	{
		if (m_listCsHeroDailyQuest == null) return false;

		for (int i = 0; i < m_listCsHeroDailyQuest.Count; i++)
		{
			CsHeroDailyQuest csHeroDailyQuest = m_listCsHeroDailyQuest[i];
			if (csHeroDailyQuest.DailyQuestMission != null)
			{
				if (csHeroDailyQuest.DailyQuestMission.TargetContinentObject != null)
				{					
					if (csHeroDailyQuest.DailyQuestMission.TargetContinentObject.ObjectId == nObjectId)
					{
						if (csHeroDailyQuest.IsAccepted && !csHeroDailyQuest.Completed && !csHeroDailyQuest.MissionImmediateCompleted && csHeroDailyQuest.ProgressCount < csHeroDailyQuest.DailyQuestMission.TargetCount)
						{
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트수락
	public void SendDailyQuestAccept(Guid guidQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DailyQuestAcceptCommandBody cmdBody = new DailyQuestAcceptCommandBody();
			cmdBody.questId = m_guidQuestId = guidQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.DailyQuestAccept, cmdBody);
		}
	}

	void OnEventResDailyQuestAccept(int nReturnCode, DailyQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroDailyQuest csHeroDailyQuest = GetHeroDailyQuest(m_guidQuestId);
			csHeroDailyQuest.IsAccepted = true;
			csHeroDailyQuest.AutoCompletionRemainingTime = csHeroDailyQuest.DailyQuestMission.DailyQuestGrade.AutoCompletionRequiredTime * 60;

			m_nDailyQuestAcceptionCount = resBody.dailyQuestAcceptionCount;
			m_dtQuestAcceptionCount = resBody.date;

			if (EventDailyQuestAccept != null)
			{
				EventDailyQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			//  일일퀘스트수락횟수가 제한횟수를 넘어갑니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트완료
	public void SendDailyQuestComplete(Guid guidQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DailyQuestCompleteCommandBody cmdBody = new DailyQuestCompleteCommandBody();
			cmdBody.questId = m_guidQuestId = guidQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.DailyQuestComplete, cmdBody);
		}
	}

	void OnEventResDailyQuestComplete(int nReturnCode, DailyQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.VipPoint = resBody.vipPoint;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlot);

			AddHeroDailyQuest(resBody.addedDailyQuest);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventDailyQuestComplete != null)
			{
				EventDailyQuestComplete(bLevelUp, resBody.acquiredExp, resBody.addedDailyQuest.slotIndex);
			}
		}
		else if (nReturnCode == 101)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 미션을 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트갱신
	public void SendDailyQuestRefresh()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DailyQuestRefreshCommandBody cmdBody = new DailyQuestRefreshCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.DailyQuestRefresh, cmdBody);
		}
	}

	void OnEventResDailyQuestRefresh(int nReturnCode, DailyQuestRefreshResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nDailyQuestFreeRefreshCount = resBody.dailyQuestFreeRefreshCount;
			m_dtQuestFreeRefreshCount = resBody.date;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;

			for (int i = 0; i < resBody.addedDailyQuests.Length; i++)
			{
				AddHeroDailyQuest(resBody.addedDailyQuests[i]);
			}

			if (EventDailyQuestRefresh != null)
			{
				EventDailyQuestRefresh();
			}
		}
		else if (nReturnCode == 101)
		{
			// 골드가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트미션즉시완료
	public void SendDailyQuestMissionImmediatlyComplete(Guid guidQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DailyQuestMissionImmediatlyCompleteCommandBody cmdBody = new DailyQuestMissionImmediatlyCompleteCommandBody();
			cmdBody.questId = m_guidQuestId = guidQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.DailyQuestMissionImmediatlyComplete, cmdBody);
		}
	}

	void OnEventResDailyQuestMissionImmediatlyComplete(int nReturnCode, DailyQuestMissionImmediatlyCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroDailyQuest csHeroDailyQuest = GetHeroDailyQuest(m_guidQuestId);
			csHeroDailyQuest.MissionImmediateCompleted = true;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;

			if (EventDailyQuestMissionImmediatlyComplete != null)
			{
				EventDailyQuestMissionImmediatlyComplete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 미션을 완료했습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 골드가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00402"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트포기
	public void SendDailyQuestAbandon(Guid guidQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DailyQuestAbandonCommandBody cmdBody = new DailyQuestAbandonCommandBody();
			cmdBody.questId = m_guidQuestId = guidQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.DailyQuestAbandon, cmdBody);
		}
	}

	void OnEventResDailyQuestAbandon(int nReturnCode, DailyQuestAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			AddHeroDailyQuest(resBody.addedDailyQuest);

			if (EventDailyQuestAbandon != null)
			{
				EventDailyQuestAbandon(resBody.addedDailyQuest.slotIndex);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 미션을 완료했습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A101_ERROR_00501"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command


	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트진행카운트갱신
	void OnEventEvtHeroDailyQuestProgressCountUpdated(SEBHeroDailyQuestProgressCountUpdatedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.progressCounts.Length; i++)
		{
			CsHeroDailyQuest csHeroDailyQuest = GetHeroDailyQuest(eventBody.progressCounts[i].id);
			csHeroDailyQuest.ProgressCount = eventBody.progressCounts[i].progressCount;
		}

		if (EventHeroDailyQuestProgressCountUpdated != null)
		{
			EventHeroDailyQuestProgressCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 일일퀘스트생성
	void OnEventEvtHeroDailyQuestCreated(SEBHeroDailyQuestCreatedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.quests.Length; i++)
		{
			AddHeroDailyQuest(eventBody.quests[i]);
		}

		if (EventHeroDailyQuestCreated != null)
		{
			EventHeroDailyQuestCreated();
		}
	}

	#endregion Protocol.Event
}
