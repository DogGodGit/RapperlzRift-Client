using ClientCommon;
using SimpleDebugLog;
using System;

public class CsWeeklyQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	Guid m_guidRoundId;

	CsHeroWeeklyQuest m_csHeroWeeklyQuest;

	//---------------------------------------------------------------------------------------------------
	public static CsWeeklyQuestManager Instance
	{
		get { return CsSingleton<CsWeeklyQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object> EventStopAutoPlay;

	// Command
	public event Delegate EventWeeklyQuestRoundAccept;
	public event Delegate EventWeeklyQuestRoundRefresh;
	public event Delegate<bool, long> EventWeeklyQuestRoundImmediatlyComplete;
	public event Delegate<bool, long> EventWeeklyQuestTenRoundImmediatlyComplete;
	public event Delegate<bool, long> EventWeeklyQuestRoundMoveMissionComplete;

	// Event
	public event Delegate EventWeeklyQuestCreated;
	public event Delegate EventWeeklyQuestRoundProgressCountUpdated;
	public event Delegate<bool, long> EventWeeklyQuestRoundCompleted;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public CsHeroWeeklyQuest HeroWeeklyQuest
	{
		get { return m_csHeroWeeklyQuest; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroWeeklyQuest weeklyQuest)
	{
		UnInit();

		if (weeklyQuest != null)
		{
			m_csHeroWeeklyQuest = new CsHeroWeeklyQuest(weeklyQuest);
		}

		// Command
		CsRplzSession.Instance.EventResWeeklyQuestRoundAccept += OnEventResWeeklyQuestRoundAccept;
		CsRplzSession.Instance.EventResWeeklyQuestRoundRefresh += OnEventResWeeklyQuestRoundRefresh;
		CsRplzSession.Instance.EventResWeeklyQuestRoundImmediatlyComplete += OnEventResWeeklyQuestRoundImmediatlyComplete;
		CsRplzSession.Instance.EventResWeeklyQuestTenRoundImmediatlyComplete += OnEventResWeeklyQuestTenRoundImmediatlyComplete;
		CsRplzSession.Instance.EventResWeeklyQuestRoundMoveMissionComplete += OnEventResWeeklyQuestRoundMoveMissionComplete;

		// Event
		CsRplzSession.Instance.EventEvtWeeklyQuestCreated += OnEventEvtWeeklyQuestCreated;
		CsRplzSession.Instance.EventEvtWeeklyQuestRoundProgressCountUpdated += OnEventEvtWeeklyQuestRoundProgressCountUpdated;
		CsRplzSession.Instance.EventEvtWeeklyQuestRoundCompleted += OnEventEvtWeeklyQuestRoundCompleted;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResWeeklyQuestRoundAccept -= OnEventResWeeklyQuestRoundAccept;
		CsRplzSession.Instance.EventResWeeklyQuestRoundRefresh -= OnEventResWeeklyQuestRoundRefresh;
		CsRplzSession.Instance.EventResWeeklyQuestRoundImmediatlyComplete -= OnEventResWeeklyQuestRoundImmediatlyComplete;
		CsRplzSession.Instance.EventResWeeklyQuestTenRoundImmediatlyComplete -= OnEventResWeeklyQuestTenRoundImmediatlyComplete;
		CsRplzSession.Instance.EventResWeeklyQuestRoundMoveMissionComplete -= OnEventResWeeklyQuestRoundMoveMissionComplete;

		// Event
		CsRplzSession.Instance.EventEvtWeeklyQuestCreated -= OnEventEvtWeeklyQuestCreated;
		CsRplzSession.Instance.EventEvtWeeklyQuestRoundProgressCountUpdated -= OnEventEvtWeeklyQuestRoundProgressCountUpdated;
		CsRplzSession.Instance.EventEvtWeeklyQuestRoundCompleted -= OnEventEvtWeeklyQuestRoundCompleted;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		dd.d("CsWeeklyQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto);
		if (m_bWaitResponse) return;
		if (m_csHeroWeeklyQuest == null) return;

		if (m_bAuto == false)
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
		dd.d("CsWeeklyQuestManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
		if (m_bAuto)
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
		if (m_csHeroWeeklyQuest == null) return false;

		if (m_csHeroWeeklyQuest.WeeklyQuestMission != null)
		{
			if (m_csHeroWeeklyQuest.WeeklyQuestMission.TargetContinentObject != null)
			{
				if (m_csHeroWeeklyQuest.WeeklyQuestMission.TargetContinentObject.ObjectId == nObjectId)
				{
					if (m_csHeroWeeklyQuest.IsRoundAccepted)
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void WeeklyQuestRoundMoveMissionComplete()
	{
		SendWeeklyQuestRoundMoveMissionComplete();
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드수락
	public void SendWeeklyQuestRoundAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			WeeklyQuestRoundAcceptCommandBody cmdBody = new WeeklyQuestRoundAcceptCommandBody();
			cmdBody.roundId = HeroWeeklyQuest.RoundId;
			CsRplzSession.Instance.Send(ClientCommandName.WeeklyQuestRoundAccept, cmdBody);
		}
	}

	void OnEventResWeeklyQuestRoundAccept(int nReturnCode, WeeklyQuestRoundAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csHeroWeeklyQuest.IsRoundAccepted = true;

			if (EventWeeklyQuestRoundAccept != null)
			{
				EventWeeklyQuestRoundAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 주간퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 주간퀘스트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 진행중인 라운드ID가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 생성상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드갱신
	public void SendWeeklyQuestRoundRefresh()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			WeeklyQuestRoundRefreshCommandBody cmdBody = new WeeklyQuestRoundRefreshCommandBody();
			cmdBody.roundId = HeroWeeklyQuest.RoundId;
			CsRplzSession.Instance.Send(ClientCommandName.WeeklyQuestRoundRefresh, cmdBody);
		}
	}

	void OnEventResWeeklyQuestRoundRefresh(int nReturnCode, WeeklyQuestRoundRefreshResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			m_csHeroWeeklyQuest.RoundId = resBody.newRoundId;
			m_csHeroWeeklyQuest.RoundMissionId = resBody.newRoundMissionId;

			if (EventWeeklyQuestRoundRefresh != null)
			{
				EventWeeklyQuestRoundRefresh();
			}
		}
		else if (nReturnCode == 101)
		{
			// 주간퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 주간퀘스트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 진행중인 라운드ID가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 생성상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00204"));
		}
		else if (nReturnCode == 105)
		{
			// 골드가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00205"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드즉시완료
	public void SendWeeklyQuestRoundImmediatlyComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			WeeklyQuestRoundImmediatlyCompleteCommandBody cmdBody = new WeeklyQuestRoundImmediatlyCompleteCommandBody();
			cmdBody.roundId = HeroWeeklyQuest.RoundId;
			CsRplzSession.Instance.Send(ClientCommandName.WeeklyQuestRoundImmediatlyComplete, cmdBody);
		}
	}

	void OnEventResWeeklyQuestRoundImmediatlyComplete(int nReturnCode, WeeklyQuestRoundImmediatlyCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			m_csHeroWeeklyQuest.Reset(resBody.nextRoundNo, resBody.nextRoundId, resBody.nextRoundMissionId);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventWeeklyQuestRoundImmediatlyComplete != null)
			{
				EventWeeklyQuestRoundImmediatlyComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 주간퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 주간퀘스트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 진행중인 라운드ID가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 진행상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00304"));
		}
		else if (nReturnCode == 105)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00305"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트10라운드즉시완료
	public void SendWeeklyQuestTenRoundImmediatlyComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			WeeklyQuestTenRoundImmediatlyCompleteCommandBody cmdBody = new WeeklyQuestTenRoundImmediatlyCompleteCommandBody();
			cmdBody.roundId = HeroWeeklyQuest.RoundId;
			CsRplzSession.Instance.Send(ClientCommandName.WeeklyQuestTenRoundImmediatlyComplete, cmdBody);
		}
	}

	void OnEventResWeeklyQuestTenRoundImmediatlyComplete(int nReturnCode, WeeklyQuestTenRoundImmediatlyCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			m_csHeroWeeklyQuest.Reset(resBody.nextRoundNo, resBody.nextRoundId, resBody.nextRoundMissionId);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventWeeklyQuestTenRoundImmediatlyComplete != null)
			{
				EventWeeklyQuestTenRoundImmediatlyComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 주간퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 주간퀘스트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 진행중인 라운드ID가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00403"));
		}
		else if (nReturnCode == 104)
		{
			// VIP레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00404"));
		}
		else if (nReturnCode == 105)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00405"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드이동미션완료
	void SendWeeklyQuestRoundMoveMissionComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			WeeklyQuestRoundMoveMissionCompleteCommandBody cmdBody = new WeeklyQuestRoundMoveMissionCompleteCommandBody();
			cmdBody.roundId = HeroWeeklyQuest.RoundId;
			CsRplzSession.Instance.Send(ClientCommandName.WeeklyQuestRoundMoveMissionComplete, cmdBody);
		}
	}

	void OnEventResWeeklyQuestRoundMoveMissionComplete(int nReturnCode, WeeklyQuestRoundMoveMissionCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHp;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			m_csHeroWeeklyQuest.Reset(resBody.nextRoundNo, resBody.nextRoundId, resBody.nextRoundMissionId);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventWeeklyQuestRoundMoveMissionComplete != null)
			{
				EventWeeklyQuestRoundMoveMissionComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 주간퀘스트가 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 주간퀘스트를 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00502"));
		}
		else if (nReturnCode == 103)
		{
			// 진행중인 라운드ID가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00503"));
		}
		else if (nReturnCode == 104)
		{
			// 진행상태가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A104_ERROR_00504"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트생성
	void OnEventEvtWeeklyQuestCreated(SEBWeeklyQuestCreatedEventBody eventBody)
	{
		m_csHeroWeeklyQuest = new CsHeroWeeklyQuest(eventBody.quest);

		if (EventWeeklyQuestCreated != null)
		{
			EventWeeklyQuestCreated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드진행카운트갱신
	void OnEventEvtWeeklyQuestRoundProgressCountUpdated(SEBWeeklyQuestRoundProgressCountUpdatedEventBody eventBody)
	{
		if (m_csHeroWeeklyQuest.RoundId == eventBody.roundId)
			m_csHeroWeeklyQuest.RoundProgressCount = eventBody.roundProgressCount;

		if (EventWeeklyQuestRoundProgressCountUpdated != null)
		{
			EventWeeklyQuestRoundProgressCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 주간퀘스트라운드완료 
	void OnEventEvtWeeklyQuestRoundCompleted(SEBWeeklyQuestRoundCompletedEventBody eventBody)
	{
		CsGameData.Instance.MyHeroInfo.Gold = eventBody.gold;
		CsAccomplishmentManager.Instance.MaxGold = eventBody.maxGold;

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHp;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);

		m_csHeroWeeklyQuest.Reset(eventBody.nextRoundNo, eventBody.nextRoundId, eventBody.nextRoundMissionId);

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		if (EventWeeklyQuestRoundCompleted != null)
		{
			EventWeeklyQuestRoundCompleted(bLevelUp, eventBody.acquiredExp);
		}
	}

	#endregion Protocol.Event
}
