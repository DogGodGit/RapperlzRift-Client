using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public class CsBiographyManager
{
	public enum EnBiographyQuestState
	{
		None = 0,       // 대기
		Accepted,       // 수락
		Executed,       // 진행 완료
	};

	bool m_bWaitResponse = false;
	bool m_bAuto = false;

	int m_nOpenedBiographyId = 0;
	int m_nBiographyId;
	int m_nAutoBiographyId;
	int m_nQuestNo;

	EnInteractionState m_enInteractionState = EnInteractionState.None;

	List<CsBiographyQuest> m_listAcceptableBiographyQuest = new List<CsBiographyQuest>();
	List<CsHeroBiography> m_listCsHeroBiography = new List<CsHeroBiography>();

	//---------------------------------------------------------------------------------------------------
	public static CsBiographyManager Instance
	{
		get { return CsSingleton<CsBiographyManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay;
	public event Delegate<object, int> EventStopAutoPlay;
	public event Delegate<CsBiographyQuest> EventNpcDialog;
	public event Delegate<CsBiographyQuest> EventNpcStartDialog;
	
	public event Delegate<CsBiography> EventBiographyStart;
	public event Delegate<int> EventBiographyComplete;
	public event Delegate<int> EventBiographyQuestAccept;
	public event Delegate<bool, long, int> EventBiographyQuestComplete;
	public event Delegate<int> EventBiographyQuestMoveObjectiveComplete;
	public event Delegate<int> EventBiographyQuestNpcConversationComplete;

	public event Delegate<int> EventBiographyQuestProgressCountsUpdated;

	//---------------------------------------------------------------------------------------------------
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public EnInteractionState InteractionState
	{
		get { return m_enInteractionState; }
	}

	public int AutoBiographyId
	{
		get { return m_nAutoBiographyId; }
	}

	public int OpenedBiographyId
	{
		get { return m_nOpenedBiographyId; }
	}

	public List<CsHeroBiography> HeroBiographyList
	{
		get { return m_listCsHeroBiography; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroBiography[] biographies)
	{
		UnInit();

		m_listCsHeroBiography = new List<CsHeroBiography>();
		m_listAcceptableBiographyQuest = new List<CsBiographyQuest>();

		for (int i = 0; i < biographies.Length; i++)
		{
			CsHeroBiography csHeroBiography = new CsHeroBiography(biographies[i]);
			m_listCsHeroBiography.Add(csHeroBiography);

			if (biographies[i].completed == false) // 완료전
			{
				if (biographies[i].quest == null)// 수락된 전기퀘스트 없거나
				{
					m_listAcceptableBiographyQuest.Add(csHeroBiography.Biography.GetBiographyQuest(1));    // 수락 가능 퀘스트 세팅.
				}
				else 
				{
					if (biographies[i].quest.completed) // 현재 퀘스트를 완료했고
					{
						CsBiographyQuest nextQuest = GetNextBiographyQuest(csHeroBiography.BiographyId);
						
						if (nextQuest != null) // 다음 퀘스트가 있을 때
						{
							m_listAcceptableBiographyQuest.Add(nextQuest);    // 수락 가능 퀘스트 세팅.
						}
					}
				}
			}
		}

		// Command
		CsRplzSession.Instance.EventResBiographyStart += OnEventResBiographyStart;
		CsRplzSession.Instance.EventResBiographyComplete += OnEventResBiographyComplete;
		CsRplzSession.Instance.EventResBiographyQuestAccept += OnEventResBiographyQuestAccept;
		CsRplzSession.Instance.EventResBiographyQuestComplete += OnEventResBiographyQuestComplete;
		CsRplzSession.Instance.EventResBiographyQuestMoveObjectiveComplete += OnEventResBiographyQuestMoveObjectiveComplete;
		CsRplzSession.Instance.EventResBiographyQuestNpcConversationComplete += OnEventResBiographyQuestNpcConversationComplete;

		// Event
		CsRplzSession.Instance.EventEvtBiographyQuestProgressCountsUpdated += OnEventEvtBiographyQuestProgressCountsUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResBiographyStart -= OnEventResBiographyStart;
		CsRplzSession.Instance.EventResBiographyComplete -= OnEventResBiographyComplete;
		CsRplzSession.Instance.EventResBiographyQuestAccept -= OnEventResBiographyQuestAccept;
		CsRplzSession.Instance.EventResBiographyQuestComplete -= OnEventResBiographyQuestComplete;
		CsRplzSession.Instance.EventResBiographyQuestMoveObjectiveComplete -= OnEventResBiographyQuestMoveObjectiveComplete;
		CsRplzSession.Instance.EventResBiographyQuestNpcConversationComplete -= OnEventResBiographyQuestNpcConversationComplete;

		// Event
		CsRplzSession.Instance.EventEvtBiographyQuestProgressCountsUpdated -= OnEventEvtBiographyQuestProgressCountsUpdated;

		m_bWaitResponse = false;
		m_listCsHeroBiography.Clear();
		m_listCsHeroBiography = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay(int nBiographyQuestId)
	{
		if (m_bWaitResponse) return;
		if (m_listCsHeroBiography == null) return;

		CsHeroBiography csHeroBiography = m_listCsHeroBiography.Find(a => a.BiographyId == nBiographyQuestId);
		if (csHeroBiography != null)
		{
			dd.d("CsBiographyManager.StartAutoPlay", m_bWaitResponse, m_bAuto);
			m_nAutoBiographyId = nBiographyQuestId;

			if (!m_bAuto)
			{
				m_bAuto = true;
				if (EventStartAutoPlay != null)
				{
					EventStartAutoPlay();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		dd.d("CsBiographyManager.StopAutoPlay", m_bWaitResponse, m_bAuto);
		
		if (m_bAuto == true)
		{
			m_bAuto = false;

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller, m_nAutoBiographyId);
			}

			m_nAutoBiographyId = 0;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool NpcDialogReadyOK(int nNpcId)
	{
		if (!m_bWaitResponse)
		{
			CsNpcInfo csNpcInfo = null;

			for (int i = 0; i < m_listAcceptableBiographyQuest.Count; i++)  // 전기퀘스트 수락전.
			{
				csNpcInfo = m_listAcceptableBiographyQuest[i].StartNpc;
				if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
				{
					if (EventNpcStartDialog != null)
					{
						dd.d("NpcDialogReadyOK   >>>   StartNpc");
						EventNpcStartDialog(m_listAcceptableBiographyQuest[i]);
					}
					return true;
				}
			}

			for (int i = 0; i < m_listCsHeroBiography.Count; i++)
			{
				CsHeroBiography csHeroBiography = m_listCsHeroBiography[i];

				if (csHeroBiography.Completed) continue;

				if (csHeroBiography.HeroBiograhyQuest != null)                              // 2. 진행중 전기퀘스트 있을때.
				{
					if (csHeroBiography.HeroBiograhyQuest.Completed) continue;				// 3. 전기퀘스트 완료

					if (csHeroBiography.HeroBiograhyQuest.Excuted)                          // 4. 전기퀘스트 완료 가능.
					{
						csNpcInfo = csHeroBiography.HeroBiograhyQuest.BiographyQuest.CompletionNpc;
						if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
						{
							if (EventNpcDialog != null)
							{
								dd.d("NpcDialogReadyOK   >>>   CompletionNpc");
								EventNpcDialog(csHeroBiography.HeroBiograhyQuest.BiographyQuest);
							}
							return true;
						}
					}
					else																	// 5. 전기퀘스트 미션중.
					{
						if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.Type == 4)		// Npc대화
						{
							csNpcInfo = csHeroBiography.HeroBiograhyQuest.BiographyQuest.TargetNpc;

							if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
							{
								SendBiographyQuestNpcConversationComplete();
								return true;
							}
						}
						else if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.Type == 5)  // 전기퀘스트던전
						{
							csNpcInfo = csHeroBiography.HeroBiograhyQuest.BiographyQuest.StartNpc;
							if (csNpcInfo != null && csNpcInfo.NpcId == nNpcId)
							{
								if (EventNpcDialog != null)
								{
									dd.d("NpcDialogReadyOK   >>>   TargetNpc");
									EventNpcDialog(csHeroBiography.HeroBiograhyQuest.BiographyQuest);
								}
								return true;
							}
						}
					}
				}
			}
		}
		return false;
	}
	
	//---------------------------------------------------------------------------------------------------
	public bool IsInteractionQuest(int nObjectId)
	{
		if (m_listCsHeroBiography == null) return false;

		for (int i = 0; i < m_listCsHeroBiography.Count; i++)
		{
			CsHeroBiography csHeroBiography = m_listCsHeroBiography[i];

			if (csHeroBiography.Completed == false)                                                                                 // 1. 전기 완료전.
			{
				if (csHeroBiography.HeroBiograhyQuest != null)                                                                      // 2. 진행중 전기퀘스트.
				{
					if (csHeroBiography.HeroBiograhyQuest.Completed || csHeroBiography.HeroBiograhyQuest.Excuted) continue;

					if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.Type == 3) // 상호작용퀘스트
					{
						if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.TargetContinentObject.ObjectId == nObjectId)
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
	public CsHeroBiography GetHeroBiography(int nBiographyId)
	{
		for (int i = 0; i < m_listCsHeroBiography.Count; i++)
		{
			if (m_listCsHeroBiography[i].BiographyId == nBiographyId)
			{
				return m_listCsHeroBiography[i];
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroBiographyQuest GetHeroBiographyQuest(int nBiographyId)
	{
		for (int i = 0; i < m_listCsHeroBiography.Count; i++)
		{
			if (m_listCsHeroBiography[i].BiographyId == nBiographyId)
			{
				return m_listCsHeroBiography[i].HeroBiograhyQuest;
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	CsBiographyQuest GetNextBiographyQuest(int nBiographyId)
	{
		CsBiography csBiography = CsGameData.Instance.GetBiography(nBiographyId);
		CsHeroBiographyQuest csHeroBiographyQuest = GetHeroBiographyQuest(nBiographyId);

		CsBiographyQuest nextBiographyQuest = csBiography.GetBiographyQuest(csHeroBiographyQuest.QuestNo + 1);

		return nextBiographyQuest;
	}

	//---------------------------------------------------------------------------------------------------
	public List<CsHeroBiographyQuest> GetAcceptionQuests()
	{
		List<CsHeroBiographyQuest> listCsHeroBiographyQuest = new List<CsHeroBiographyQuest>();

		foreach (var csHeroBiography in m_listCsHeroBiography)
		{
			if (csHeroBiography.HeroBiograhyQuest != null)
			{
				listCsHeroBiographyQuest.Add(csHeroBiography.HeroBiograhyQuest);
			}
		}

		return listCsHeroBiographyQuest;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuest GetAcceptableQuest(int nBiographyId)
	{
		return m_listAcceptableBiographyQuest.Find(quest => quest.BiographyId == nBiographyId);
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckBiographyNotice(CsHeroBiography csHeroBiography)
	{
		if (csHeroBiography.Completed)
			return false;

		if (csHeroBiography.HeroBiograhyQuest != null &&
			csHeroBiography.HeroBiograhyQuest.Completed &&
			csHeroBiography.Biography.GetBiographyQuest(csHeroBiography.HeroBiograhyQuest.QuestNo + 1) != null)
		{
			return true;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckBiographyNotices()
	{
		if (!CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.Biography))
		{
			return false;
		}

		foreach (var csHeroBiography in m_listCsHeroBiography)
		{
			if (CheckBiographyNotice(csHeroBiography))
				return true;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public void BiographyQuestMoveObjectiveComplete()
	{
		if(true)
		{
			SendBiographyQuestMoveObjectiveComplete();
		}
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 전기시작
	public void SendBiographyStart(int nBiographyId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyStartCommandBody cmdBody = new BiographyStartCommandBody();
			cmdBody.biographyId = m_nBiographyId = nBiographyId;
			CsRplzSession.Instance.Send(ClientCommandName.BiographyStart, cmdBody);
		}
	}
	void OnEventResBiographyStart(int nReturnCode, BiographyStartResponseBody resBody)
	{
		m_bWaitResponse = false;
		Debug.Log("OnEventResBiographyStart    nReturnCode = "+ nReturnCode);
		if (nReturnCode == 0)
		{
			m_nOpenedBiographyId = m_nBiographyId;

			m_nQuestNo = 1;
			m_listCsHeroBiography.Add(new CsHeroBiography(m_nBiographyId));
			CsHeroBiography csHeroBiography = GetHeroBiography(m_nBiographyId);
			m_listAcceptableBiographyQuest.Add(csHeroBiography.Biography.GetBiographyQuest(m_nQuestNo));	// 수락 가능 퀘스트 세팅.

			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			if (EventBiographyStart != null)
			{
				EventBiographyStart(csHeroBiography.Biography);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 시작한 전기입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전기완료
	public void SendBiographyComplete(int nBiographyId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyCompleteCommandBody cmdBody = new BiographyCompleteCommandBody();
			cmdBody.biographyId = m_nBiographyId = nBiographyId;
			CsRplzSession.Instance.Send(ClientCommandName.BiographyComplete, cmdBody);
		}
	}
	void OnEventResBiographyComplete(int nReturnCode, BiographyCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroBiography csHeroBiography = GetHeroBiography(m_nBiographyId);
			csHeroBiography.Completed = true;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventBiographyComplete != null)
			{
				EventBiographyComplete(m_nBiographyId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 완료된 전기입니다. 
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 퀘스트를 모두 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전기퀘스트수락
	public void SendBiographyQuestAccept(int nBiographyId, int nQuestNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyQuestAcceptCommandBody cmdBody = new BiographyQuestAcceptCommandBody();
			cmdBody.biographyId = m_nBiographyId = nBiographyId;
			cmdBody.questNo = m_nQuestNo = nQuestNo;
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestAccept, cmdBody);
		}
	}
	void OnEventResBiographyQuestAccept(int nReturnCode, BiographyQuestAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsBiographyQuest csBiographyQuest = m_listAcceptableBiographyQuest.Find(a => a.BiographyId == m_nBiographyId);
			if (csBiographyQuest != null)
			{
				m_listAcceptableBiographyQuest.Remove(csBiographyQuest);
			}

			CsHeroBiography csHeroBiography = GetHeroBiography(m_nBiographyId);
			csHeroBiography.HeroBiograhyQuest = new CsHeroBiographyQuest(csHeroBiography.Biography, m_nQuestNo);

			if (csHeroBiography.HeroBiograhyQuest.BiographyQuest.Type == 5) // 퀘스트 타입이 던전 클리어
			{
				CsDungeonManager.Instance.ContinentExitForBiographyQuest(csHeroBiography.HeroBiograhyQuest.BiographyQuest.TargetDungeonId);
			}

			if (EventBiographyQuestAccept != null)
			{
				EventBiographyQuestAccept(m_nBiographyId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 완료되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 마지막 퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 시작NPC와 상호작용할 수 없는 거리입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전기퀘스트완료
	public void SendBiographyQuestComplete(int nBiographyId, int nQuestNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyQuestCompleteCommandBody cmdBody = new BiographyQuestCompleteCommandBody();
			cmdBody.biographyId = m_nBiographyId = nBiographyId;
			cmdBody.questNo = m_nQuestNo = nQuestNo;
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestComplete, cmdBody);
		}
	}
	void OnEventResBiographyQuestComplete(int nReturnCode, BiographyQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroBiography csHeroBiography = GetHeroBiography(m_nBiographyId);
			csHeroBiography.HeroBiograhyQuest.Completed = true;												// 전기 퀘스트 완료 처리
			m_nQuestNo++;

			CsBiographyQuest csBiographyQuest = csHeroBiography.Biography.GetBiographyQuest(m_nQuestNo);
			
			if (csBiographyQuest != null)
			{
				m_listAcceptableBiographyQuest.Add(csBiographyQuest);    // 다음 전기 퀘스트 수락 가능 퀘스트에 세팅.
			}
			else
			{
				CsGameEventUIToUI.Instance.OnEventOpenPanelBiographyComplete(csHeroBiography.BiographyId);
			}
			
			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventBiographyQuestComplete != null)
			{
				EventBiographyQuestComplete(bLevelUp, resBody.acquiredExp, m_nBiographyId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 완료한 전기입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 퀘스트 목표를 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 완료한 퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A122_ERROR_00403"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

	}

	//---------------------------------------------------------------------------------------------------
	// 전기퀘스트이동목표완료
	void SendBiographyQuestMoveObjectiveComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyQuestMoveObjectiveCompleteCommandBody cmdBody = new BiographyQuestMoveObjectiveCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestMoveObjectiveComplete, cmdBody);
		}
	}
	void OnEventResBiographyQuestMoveObjectiveComplete(int nReturnCode, BiographyQuestMoveObjectiveCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		Debug.Log("OnEventResBiographyQuestMoveObjectiveComplete   nReturnCode = "+ nReturnCode);
		if (nReturnCode == 0)
		{
			for (int i = 0; i < resBody.progressCounts.Length; i++)
			{
				CsHeroBiography csHeroBiography = GetHeroBiography(resBody.progressCounts[i].biographyId);
				csHeroBiography.HeroBiograhyQuest.ProgressCount = resBody.progressCounts[i].progressCount;

				if (EventBiographyQuestMoveObjectiveComplete != null)
				{
					EventBiographyQuestMoveObjectiveComplete(resBody.progressCounts[i].biographyId);
				}
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 전기퀘스트NPC대화완료
	void SendBiographyQuestNpcConversationComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BiographyQuestNpcConversationCompleteCommandBody cmdBody = new BiographyQuestNpcConversationCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BiographyQuestNpcConversationComplete, cmdBody);
		}
	}
	void OnEventResBiographyQuestNpcConversationComplete(int nReturnCode, BiographyQuestNpcConversationCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			for (int i = 0; i < resBody.progressCounts.Length; i++)
			{
				CsHeroBiography csHeroBiography = GetHeroBiography(resBody.progressCounts[i].biographyId);
				csHeroBiography.HeroBiograhyQuest.ProgressCount = resBody.progressCounts[i].progressCount;

				if (EventBiographyQuestNpcConversationComplete != null)
				{
					EventBiographyQuestNpcConversationComplete(resBody.progressCounts[i].biographyId);
				}
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 전기퀘스트진행카운트갱신
	void OnEventEvtBiographyQuestProgressCountsUpdated(SEBBiographyQuestProgressCountsUpdatedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.progressCounts.Length; i++)
		{
			CsHeroBiography csHeroBiography = GetHeroBiography(eventBody.progressCounts[i].biographyId);
			csHeroBiography.HeroBiograhyQuest.ProgressCount = eventBody.progressCounts[i].progressCount;

			if (EventBiographyQuestProgressCountsUpdated != null)
			{
				EventBiographyQuestProgressCountsUpdated(eventBody.progressCounts[i].biographyId);
			}
		}
	}

	#endregion Protocol.Event
}
