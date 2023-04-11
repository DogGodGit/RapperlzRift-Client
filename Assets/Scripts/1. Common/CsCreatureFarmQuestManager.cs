using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public enum EnCreatureFarmQuestState { None, Accepted, Executed, Completed};

public class CsCreatureFarmQuestManager
{   
	//---------------------------------------------------------------------------------------------------
	public static CsCreatureFarmQuestManager Instance
	{
		get { return CsSingleton<CsCreatureFarmQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	bool m_bWaitResponse = false;
	bool m_bAuto = false;

	int m_nDailyCreatureFarmQuestAcceptionCount;
	DateTime m_dtCreatureFarmQuestAcceptionCountDate;

	CsHeroCreatureFarmQuest m_csHeroCreatureFarmQuest;
	EnCreatureFarmQuestState m_enCreatureFarmQuestState = EnCreatureFarmQuestState.None;


	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object> EventStopAutoPlay;
	public event Delegate<int> EventAcceptDialog;
	public event Delegate<int> EventCompleteDialog;

	public event Delegate EventCreatureFarmQuestAccept;
	public event Delegate<bool, long> EventCreatureFarmQuestComplete;
	public event Delegate<bool, long> EventCreatureFarmQuestMissionMoveObjectiveComplete;

	public event Delegate EventCreatureFarmQuestMissionProgressCountUpdated;
	public event Delegate<bool, long> EventCreatureFarmQuestMissionCompleted;
	public event Delegate EventCreatureFarmQuestMissionMonsterSpawned;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public int DailyCreatureFarmQuestAcceptionCount
	{
		get { return m_nDailyCreatureFarmQuestAcceptionCount; }
		set { m_nDailyCreatureFarmQuestAcceptionCount = value; }
	}

	public DateTime CreatureFarmQuestAcceptionCountDate
	{
		get { return m_dtCreatureFarmQuestAcceptionCountDate; }
		set { m_dtCreatureFarmQuestAcceptionCountDate = value; }
	}

	public CsHeroCreatureFarmQuest HeroCreatureFarmQuest
	{
		get { return m_csHeroCreatureFarmQuest; }
	}

	public CsCreatureFarmQuest CreatureFarmQuest
	{
		get { return CsGameData.Instance.CreatureFarmQuest; }
	}

	public EnCreatureFarmQuestState CreatureFarmQuestState
	{
		get { return m_enCreatureFarmQuestState; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(int dailyCreatureFarmQuestAcceptionCount, PDHeroCreatureFarmQuest creatureFarmQuest, DateTime dtDate)
	{
		UnInit();
		
		m_nDailyCreatureFarmQuestAcceptionCount = dailyCreatureFarmQuestAcceptionCount;
		m_dtCreatureFarmQuestAcceptionCountDate = dtDate;
		
		if (creatureFarmQuest != null)
		{
			m_csHeroCreatureFarmQuest = new CsHeroCreatureFarmQuest(creatureFarmQuest);
			if (m_csHeroCreatureFarmQuest.CreatureFarmQuestMission != null)
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Accepted;
			}
			else
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Executed;
			}
		}
		else
		{
			if (m_nDailyCreatureFarmQuestAcceptionCount == 1)
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Completed;
			}
			else
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.None;
			}
		}

		// Command
		CsRplzSession.Instance.EventResCreatureFarmQuestAccept += OnEventResCreatureFarmQuestAccept;
		CsRplzSession.Instance.EventResCreatureFarmQuestComplete += OnEventResCreatureFarmQuestComplete;
		CsRplzSession.Instance.EventResCreatureFarmQuestMissionMoveObjectiveComplete += OnEventResCreatureFarmQuestMissionMoveObjectiveComplete;

		// Event
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionProgressCountUpdated += OnEventEvtCreatureFarmQuestMissionProgressCountUpdated;
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionCompleted += OnEventEvtCreatureFarmQuestMissionCompleted;
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionMonsterSpawned += OnEventEvtCreatureFarmQuestMissionMonsterSpawned;
		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResCreatureFarmQuestAccept -= OnEventResCreatureFarmQuestAccept;
		CsRplzSession.Instance.EventResCreatureFarmQuestComplete -= OnEventResCreatureFarmQuestComplete;
		CsRplzSession.Instance.EventResCreatureFarmQuestMissionMoveObjectiveComplete -= OnEventResCreatureFarmQuestMissionMoveObjectiveComplete;

		// Event
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionProgressCountUpdated -= OnEventEvtCreatureFarmQuestMissionProgressCountUpdated;
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionCompleted -= OnEventEvtCreatureFarmQuestMissionCompleted;
		CsRplzSession.Instance.EventEvtCreatureFarmQuestMissionMonsterSpawned -= OnEventEvtCreatureFarmQuestMissionMonsterSpawned;
		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Completed)
		{
			m_enCreatureFarmQuestState = EnCreatureFarmQuestState.None;
		}
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
		Debug.Log("CsCreatureFarmQuestManager.StopAutoPlay");
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
		/*
			* 목표타입
			1 : 이동

			2 : 상호작용
			- 목표대륙오브젝트ID

			3 : 몬스터 처치
			- 몬스터배치
			- 목표자동완료시간 
		*/
		return (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Accepted &&
				m_csHeroCreatureFarmQuest.CreatureFarmQuestMission.TargetType == 2 &&
				m_csHeroCreatureFarmQuest.CreatureFarmQuestMission.ContinentObjectTarget != null &&
				m_csHeroCreatureFarmQuest.CreatureFarmQuestMission.ContinentObjectTarget.ObjectId == nObjectId);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsCreatureFarmQuestNpc(int nNpcId)
	{
		Debug.Log("IsMainQuestNpc  nNpcId = " + nNpcId);
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.None && m_nDailyCreatureFarmQuestAcceptionCount == 0)
		{
			return (CreatureFarmQuest.StartNpc != null && CreatureFarmQuest.StartNpc.NpcId == nNpcId);
		}
		else if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
		{
			return (CreatureFarmQuest.CompletionNpc != null && CreatureFarmQuest.CompletionNpc.NpcId == nNpcId);
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void AcceptReadyOK()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.None && m_nDailyCreatureFarmQuestAcceptionCount == 0)
		{
			if (CsGameData.Instance.CreatureFarmQuest.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
			{				
				if (CreatureFarmQuest.StartNpc != null)
				{
					if (EventAcceptDialog != null)
					{
						Debug.Log("AcceptReadyOK  nNpcId = " + CreatureFarmQuest.StartNpc.NpcId);
						EventAcceptDialog(CreatureFarmQuest.StartNpc.NpcId);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void CompleteReadyOK()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Executed)
		{
			if (CreatureFarmQuest.CompletionNpc != null)
			{
				if (EventCompleteDialog != null)
				{
					Debug.Log("AcceptReadyOK  nNpcId = " + CreatureFarmQuest.CompletionNpc.NpcId);
					EventCompleteDialog(CreatureFarmQuest.CompletionNpc.NpcId);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Accept()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.None && m_nDailyCreatureFarmQuestAcceptionCount == 0)
		{
			SendCreatureFarmQuestAccept();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void Complete()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Executed && m_csHeroCreatureFarmQuest != null)
		{
			SendCreatureFarmQuestComplete(m_csHeroCreatureFarmQuest.InstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void MissionMoveObjectiveComplete()
	{
		if (m_enCreatureFarmQuestState == EnCreatureFarmQuestState.Accepted && m_csHeroCreatureFarmQuest != null)
		{
			Debug.Log("MissionMoveObjectiveComplete    "+ m_csHeroCreatureFarmQuest.InstanceId +" , "+ m_csHeroCreatureFarmQuest.MissionNo);
			SendCreatureFarmQuestMissionMoveObjectiveComplete(m_csHeroCreatureFarmQuest.InstanceId, m_csHeroCreatureFarmQuest.MissionNo);
		}
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트수락
	void SendCreatureFarmQuestAccept()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureFarmQuestAcceptCommandBody cmdBody = new CreatureFarmQuestAcceptCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CreatureFarmQuestAccept, cmdBody);
		}
	}

	void OnEventResCreatureFarmQuestAccept(int nReturnCode, CreatureFarmQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResCreatureFarmQuestAccept       nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{			
		    m_csHeroCreatureFarmQuest = new CsHeroCreatureFarmQuest(resBody.heroCreatureFarmQuest);
			m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Accepted;
			m_nDailyCreatureFarmQuestAcceptionCount = 1;
			m_dtCreatureFarmQuestAcceptionCountDate = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date;

			if (EventCreatureFarmQuestAccept != null)
			{
				EventCreatureFarmQuestAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 위탁중인 할일입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 수락횟수가 최대횟수를 넘어갑니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 시작NPC와 상호작용할 수 없는 위치입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트완료
	void SendCreatureFarmQuestComplete(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureFarmQuestCompleteCommandBody cmdBody = new CreatureFarmQuestCompleteCommandBody();
			cmdBody.instanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureFarmQuestComplete, cmdBody);
		}
	}

	void OnEventResCreatureFarmQuestComplete(int nReturnCode, CreatureFarmQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResCreatureFarmQuestComplete       nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			m_csHeroCreatureFarmQuest = null;

			if (m_dtCreatureFarmQuestAcceptionCountDate < CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.None;
			}
			else
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Completed;
			}

			if (EventCreatureFarmQuestComplete != null)
			{
				EventCreatureFarmQuestComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 완료한 퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 퀘스트목표를 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			//  완료NPC와 상호작용할 수 없는 위치입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트미션이동목표완료
	void SendCreatureFarmQuestMissionMoveObjectiveComplete(Guid guidInstanceId, int nMissionNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureFarmQuestMissionMoveObjectiveCompleteCommandBody cmdBody = new CreatureFarmQuestMissionMoveObjectiveCompleteCommandBody();
			cmdBody.instanceId = guidInstanceId;
			cmdBody.missionNo = nMissionNo;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureFarmQuestMissionMoveObjectiveComplete, cmdBody);
		}
	}

	void OnEventResCreatureFarmQuestMissionMoveObjectiveComplete(int nReturnCode, CreatureFarmQuestMissionMoveObjectiveCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResCreatureFarmQuestMissionMoveObjectiveComplete       nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			m_csHeroCreatureFarmQuest.MissionNo = resBody.nextMissionNo;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (m_csHeroCreatureFarmQuest.CreatureFarmQuestMission == null)
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Executed;
			}
			else
			{
				m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Accepted;
			}

			if (EventCreatureFarmQuestMissionMoveObjectiveComplete != null)
			{
				EventCreatureFarmQuestMissionMoveObjectiveComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 목표위치와 상호작용할 수 없는 위치입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A149_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트미션진행카운트갱신
	void OnEventEvtCreatureFarmQuestMissionProgressCountUpdated(SEBCreatureFarmQuestMissionProgressCountUpdatedEventBody eventBody)
	{
		m_csHeroCreatureFarmQuest.ProgressCount = eventBody.progressCount;

		if (EventCreatureFarmQuestMissionProgressCountUpdated != null)
		{
			EventCreatureFarmQuestMissionProgressCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트미션진행카운트갱신
	void OnEventEvtCreatureFarmQuestMissionCompleted(SEBCreatureFarmQuestMissionCompletedEventBody eventBody)
	{
		m_csHeroCreatureFarmQuest.MissionNo = eventBody.nextMissionNo;

		int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

		CsGameData.Instance.MyHeroInfo.Level = eventBody.level;
		CsGameData.Instance.MyHeroInfo.Exp = eventBody.exp;
		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

		if (m_csHeroCreatureFarmQuest.CreatureFarmQuestMission == null)
		{
			m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Executed;
		}
		else
		{
			m_enCreatureFarmQuestState = EnCreatureFarmQuestState.Accepted;
		}
		
		if (EventCreatureFarmQuestMissionCompleted != null)
		{
			EventCreatureFarmQuestMissionCompleted(bLevelUp, eventBody.acquiredExp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처농장퀘스트미션몬스터스폰
	void OnEventEvtCreatureFarmQuestMissionMonsterSpawned(SEBCreatureFarmQuestMissionMonsterSpawnedEventBody eventBody)
	{
		m_csHeroCreatureFarmQuest.MonsterInstanceId = eventBody.instanceId;
		m_csHeroCreatureFarmQuest.MonsterPosition = CsRplzSession.Translate(eventBody.position);
		m_csHeroCreatureFarmQuest.RemainingMonsterLifetime = eventBody.remainingLifetime;

		Debug.Log("OnEventEvtCreatureFarmQuestMissionMonsterSpawned : "+ m_csHeroCreatureFarmQuest.MonsterPosition);
		if (EventCreatureFarmQuestMissionMonsterSpawned != null)
		{
			EventCreatureFarmQuestMissionMonsterSpawned();
		}
	}

	#endregion Protocol.Event
}
