using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;
using System.Linq;

public class CsSubQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	
	int m_nQuestId;
	CsHeroSubQuest m_csAutoHeroSubQuest;

	List<CsHeroSubQuest> m_listCsHeroSubQuest;

	//---------------------------------------------------------------------------------------------------
	public static CsSubQuestManager Instance
	{
		get { return CsSingleton<CsSubQuestManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object, int> EventStopAutoPlay;
	public event Delegate<int> EventNpcDialog;

	public event Delegate<int> EventSubQuestAccept;
	public event Delegate<bool, long, int> EventSubQuestComplete;
	public event Delegate<int> EventSubQuestAbandon;
	public event Delegate<int> EventSubQuestsAccepted;
	public event Delegate<int> EventSubQuestProgressCountsUpdated;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public CsHeroSubQuest AutoHeroSubQuest
	{
		get { return m_csAutoHeroSubQuest; }
	}

	public List<CsHeroSubQuest> HeroSubQuestList
	{
		get { return m_listCsHeroSubQuest; }
	}
	
	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroSubQuest[] subQuests)
	{
		UnInit();

		m_listCsHeroSubQuest = new List<CsHeroSubQuest>();

		for (int i = 0; i < subQuests.Length; i++)
		{
			m_listCsHeroSubQuest.Add(new CsHeroSubQuest(subQuests[i]));
		}

		// Command
		CsRplzSession.Instance.EventResSubQuestAccept += OnEventResSubQuestAccept;
		CsRplzSession.Instance.EventResSubQuestComplete += OnEventResSubQuestComplete;
		CsRplzSession.Instance.EventResSubQuestAbandon += OnEventResSubQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtSubQuestsAccepted += OnEventEvtSubQuestsAccepted;
		CsRplzSession.Instance.EventEvtSubQuestProgressCountsUpdated += OnEventEvtSubQuestProgressCountsUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResSubQuestAccept -= OnEventResSubQuestAccept;
		CsRplzSession.Instance.EventResSubQuestComplete -= OnEventResSubQuestComplete;
		CsRplzSession.Instance.EventResSubQuestAbandon -= OnEventResSubQuestAbandon;

		// Event
		CsRplzSession.Instance.EventEvtSubQuestsAccepted -= OnEventEvtSubQuestsAccepted;
		CsRplzSession.Instance.EventEvtSubQuestProgressCountsUpdated -= OnEventEvtSubQuestProgressCountsUpdated;

		m_bAuto = false;
		m_bWaitResponse = false;
		m_csAutoHeroSubQuest = null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroSubQuest GetHeroSubQuest(int nQuestId)
	{
		for (int i = 0; i < m_listCsHeroSubQuest.Count; i++)
		{
			if (m_listCsHeroSubQuest[i].SubQuest.QuestId == nQuestId)
				return m_listCsHeroSubQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay(int nQuestId)
	{
		dd.d("CsWeeklyQuestManager.StartAutoPlay", m_bWaitResponse, m_bAuto);

		if (m_bWaitResponse) return;

		m_csAutoHeroSubQuest = GetHeroSubQuest(nQuestId);
		if (m_csAutoHeroSubQuest == null) return;

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

		int nSubQuestId = 0;
		if (m_csAutoHeroSubQuest != null)
		{
			nSubQuestId = m_csAutoHeroSubQuest.SubQuest.QuestId;
		}

		m_csAutoHeroSubQuest = null;
		if (m_bAuto)
		{
			m_bAuto = false;

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller, nSubQuestId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsSubQuestNpc(int nNpcId)
	{
		dd.d("IsSubQuestNpc", nNpcId, m_bWaitResponse);
		if (!m_bWaitResponse)
		{
			for (int i = 0; i < m_listCsHeroSubQuest.Count; i++)
			{
				CsHeroSubQuest csHeroSubQuest = m_listCsHeroSubQuest[i];

				if (csHeroSubQuest == null || csHeroSubQuest.SubQuest == null) continue;

				if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
				{
					if (csHeroSubQuest.SubQuest.CompletionNpc != null && csHeroSubQuest.SubQuest.CompletionNpc.NpcId == nNpcId)
					{
						if (EventNpcDialog != null)
						{
							dd.d("1. IsSubQuestNpc");
							EventNpcDialog(nNpcId);
						}
						return true;
					}
				}
				else if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon)
				{
					if (csHeroSubQuest.SubQuest.ReacceptanceEnabled)
					{
						if (csHeroSubQuest.SubQuest.StartNpc != null && csHeroSubQuest.SubQuest.StartNpc.NpcId == nNpcId)
						{
							if (EventNpcDialog != null)
							{
								dd.d("2. IsSubQuestNpc");
								EventNpcDialog(nNpcId);
							}
							return true;
						}
					}
				}
			}

			for (int i = 0; i < CsGameData.Instance.SubQuestList.Count; i++)
			{
				CsSubQuest csSubQuest = CsGameData.Instance.SubQuestList[i];
				if (csSubQuest.RequiredConditionType == 1)
				{
					if (CsMainQuestManager.Instance.MainQuest == null || csSubQuest.RequiredConditionValue < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
					{
						if (csSubQuest.StartNpc != null && csSubQuest.StartNpc.NpcId == nNpcId)
						{
							if (EventNpcDialog != null)
							{
								dd.d("3. IsSubQuestNpc");
								EventNpcDialog(nNpcId);
							}
							return true;
						}
					}
				}
				else
				{
					if (csSubQuest.RequiredConditionValue <= CsGameData.Instance.MyHeroInfo.Level)
					{
						if (csSubQuest.StartNpc != null && csSubQuest.StartNpc.NpcId == nNpcId)
						{
							if (EventNpcDialog != null)
							{
								dd.d("4. IsSubQuestNpc");
								EventNpcDialog(nNpcId);
							}
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public CsSubQuest GetSubQuestFromNpc(int nNpcId)
	{
		if (!m_bWaitResponse)
		{
			for (int i = 0; i < CsGameData.Instance.SubQuestList.Count; i++)
			{
				CsSubQuest csSubQuest = CsGameData.Instance.SubQuestList[i];
				CsHeroSubQuest csHeroSubQuest = GetHeroSubQuest(csSubQuest.QuestId);

				if (csHeroSubQuest == null ||
					(csHeroSubQuest.EnStatus == EnSubQuestStatus.Abandon && csSubQuest.ReacceptanceEnabled))
				{
					if (csSubQuest.StartNpc != null &&
						csSubQuest.StartNpc.NpcId == nNpcId)
					{
						if (csSubQuest.RequiredConditionType == 1)
						{
							if (csSubQuest.RequiredConditionValue < CsMainQuestManager.Instance.MainQuest.MainQuestNo)
							{
								return csSubQuest;
							}
						}
						else
						{
							if (csSubQuest.RequiredConditionValue <= CsGameData.Instance.MyHeroInfo.Level)
							{
								return csSubQuest;
							}
						}
					}
				}
				else if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
				{
					if (csSubQuest.Completion != null &&
						csSubQuest.CompletionNpc.NpcId == nNpcId)
					{
						return csSubQuest;
					}
				}
			}
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsSubQuestObject(int nObjectId)
	{
		if (m_listCsHeroSubQuest == null) return false;

		for (int i = 0; i < m_listCsHeroSubQuest.Count; i++)
		{
			CsHeroSubQuest csHeroSubQuest = m_listCsHeroSubQuest[i];
			if (csHeroSubQuest == null) return false;

			if (csHeroSubQuest.SubQuest != null)
			{
				if (csHeroSubQuest.SubQuest.TargetContinentObject != null)
				{
					if (csHeroSubQuest.SubQuest.TargetContinentObject.ObjectId == nObjectId)
					{
						if (csHeroSubQuest.EnStatus == EnSubQuestStatus.Acception)
						{
							return true;
						}
					}
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public CsNpcInfo GetSubQuestNpc(int nNpcId)
	{
		for (int i = 0; i < CsGameData.Instance.SubQuestList.Count; i++)
		{
			CsSubQuest csSubQuest = CsGameData.Instance.SubQuestList[i];

			if (csSubQuest != null)
			{
				if (csSubQuest.StartNpc != null && csSubQuest.StartNpc.NpcId == nNpcId)
				{
					return csSubQuest.StartNpc;
				}
				else if (csSubQuest.CompletionNpc != null && csSubQuest.CompletionNpc.NpcId == nNpcId)
				{
					return csSubQuest.CompletionNpc;
				}
			}
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	// 진행 중인 서브 퀘스트 리스트
	public List<CsHeroSubQuest> GetAcceptionSubQuestList()
	{
		return m_listCsHeroSubQuest.FindAll(heroSubQuest => heroSubQuest.EnStatus == EnSubQuestStatus.Acception || heroSubQuest.EnStatus == EnSubQuestStatus.Excuted);
	}

	//---------------------------------------------------------------------------------------------------
	// 수락 가능한 모든 서브 퀘스트 리스트
	public IEnumerable<CsSubQuest> GetAcceptableSubQuestList()
	{
		var res = from subQuest in CsGameData.Instance.SubQuestList
				  where subQuest.StartNpc != null &&
						(subQuest.RequiredConditionType == 1 ? subQuest.RequiredConditionValue < CsMainQuestManager.Instance.MainQuest.MainQuestNo :
							subQuest.RequiredConditionValue <= CsGameData.Instance.MyHeroInfo.Level) &&
						(m_listCsHeroSubQuest.Find(heroSubQuest => heroSubQuest.SubQuest.QuestId == subQuest.QuestId) == null ||
						 (m_listCsHeroSubQuest.Find(heroSubQuest => heroSubQuest.SubQuest.QuestId == subQuest.QuestId).EnStatus == EnSubQuestStatus.Abandon &&
						 subQuest.ReacceptanceEnabled))
				  select subQuest;

		return res;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 서브퀘스트수락
	public void SendSubQuestAccept(int nQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			SubQuestAcceptCommandBody cmdBody = new SubQuestAcceptCommandBody();
			cmdBody.questId = m_nQuestId = nQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.SubQuestAccept, cmdBody);
		}
	}
	void OnEventResSubQuestAccept(int nReturnCode, SubQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroSubQuest csHeroSubQuest = GetHeroSubQuest(m_nQuestId);

			if (csHeroSubQuest != null)
			{
				csHeroSubQuest.ProgressCount = 0;
				csHeroSubQuest.Status = (int)EnSubQuestStatus.Acception;
			}
			else
			{
				m_listCsHeroSubQuest.Add(new CsHeroSubQuest(m_nQuestId));
			}

			if (EventSubQuestAccept != null)
			{
				EventSubQuestAccept(m_nQuestId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 목표NPC와 상호작용을 할 수 없는 거리입니다. 
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 메인퀘스트를 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 시작NPC가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 이미 수락한 서브퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 이미 완료한 서브퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 재수락을 할 수 없는 서브퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00107"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서브퀘스트완료
	public void SendSubQuestComplete(int nQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			SubQuestCompleteCommandBody cmdBody = new SubQuestCompleteCommandBody();
			cmdBody.questId = m_nQuestId = nQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.SubQuestComplete, cmdBody);
		}
	}
	void OnEventResSubQuestComplete(int nReturnCode, SubQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroSubQuest csHeroSubQuest = GetHeroSubQuest(m_nQuestId);
			csHeroSubQuest.Status = (int)EnSubQuestStatus.Completion;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			// 최대 골드
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventSubQuestComplete != null)
			{
				EventSubQuestComplete(bLevelUp, resBody.acquiredExp, m_nQuestId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 수락하지 않은 서브퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 퀘스트목표를 아직 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 목표NPC와 상호작용을 할 수 없는 거리입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00204"));
		}
		else if (nReturnCode == 105)
		{
			// 길드에 가입하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00205"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서브퀘스트포기
	public void SendSubQuestAbandon(int nQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			SubQuestAbandonCommandBody cmdBody = new SubQuestAbandonCommandBody();
			cmdBody.questId = m_nQuestId = nQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.SubQuestAbandon, cmdBody);
		}
	}
	void OnEventResSubQuestAbandon(int nReturnCode, SubQuestAbandonResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroSubQuest csHeroSubQuest = GetHeroSubQuest(m_nQuestId);
			csHeroSubQuest.Status = (int)EnSubQuestStatus.Abandon;

			if (EventSubQuestAbandon != null)
			{
				EventSubQuestAbandon(m_nQuestId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 수락하지 않은 서브퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A119_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 서브퀘스트수락
	void OnEventEvtSubQuestsAccepted(SEBSubQuestsAcceptedEventBody eventBody)
	{
		Debug.Log("OnEventEvtSubQuestsAccepted  eventBody.subQuests.Length = "+ eventBody.subQuests.Length);
		for (int i = 0; i < eventBody.subQuests.Length; i++)
		{
			m_listCsHeroSubQuest.Add(new CsHeroSubQuest(eventBody.subQuests[i]));

			if (EventSubQuestsAccepted != null)
			{
				EventSubQuestsAccepted(eventBody.subQuests[i].questId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서브퀘스트진행카운트갱신
	void OnEventEvtSubQuestProgressCountsUpdated(SEBSubQuestProgressCountsUpdatedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.progressCounts.Length; i++)
		{
			CsHeroSubQuest csHeroSubQuest = GetHeroSubQuest(eventBody.progressCounts[i].questId);
			csHeroSubQuest.ProgressCount = eventBody.progressCounts[i].progressCount;

			if (csHeroSubQuest.SubQuest.RequiredConditionType == 2 &&
				csHeroSubQuest.SubQuest.CompletionNpc == null &&
				csHeroSubQuest.EnStatus == EnSubQuestStatus.Excuted)
			{
				CsSubQuestManager.Instance.SendSubQuestComplete(csHeroSubQuest.SubQuest.QuestId);
				continue;
			}

			if (EventSubQuestProgressCountsUpdated != null)
			{
				EventSubQuestProgressCountsUpdated(eventBody.progressCounts[i].questId);
			}
		}
	}

	#endregion Protocol.Event
	
}
