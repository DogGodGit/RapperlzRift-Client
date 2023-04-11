using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;
using System.Linq;

public class CsBlessingQuestManager
{
	bool m_bWaitResponse = false;

	List<CsHeroBlessingQuest> m_listCsHeroBlessingQuest;

	List<CsHeroProspectQuest> m_listCsHeroProspectQuestOwner;
	List<CsHeroProspectQuest> m_listCsHeroProspectQuestTarget;

	List<CsHeroBlessing> m_listCsHeroBlessingReceived;

	long m_lQuestId;
	int m_nBlessingId;
	long m_lInstanceId;
	Guid m_guidInstanceId;

	//---------------------------------------------------------------------------------------------------
	public static CsBlessingQuestManager Instance
	{
		get { return CsSingleton<CsBlessingQuestManager>.GetInstance(); }
	}

	public List<CsHeroBlessingQuest> HeroBlessingQuestList
	{
		get { return m_listCsHeroBlessingQuest; }
	}

	public List<CsHeroBlessing> HeroBlessingReceivedList
	{
		get { return m_listCsHeroBlessingReceived; }
	}

	public List<CsHeroProspectQuest> HeroProspectQuestOwnerList
	{
		get { return m_listCsHeroProspectQuestOwner; }
	}

	public List<CsHeroProspectQuest> HeroProspectQuestTargetList
	{
		get { return m_listCsHeroProspectQuestTarget; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<long> EventBlessingQuestBlessingSend;
	public event Delegate EventBlessingQuestDeleteAll;
	public event Delegate<long> EventBlessingRewardReceive;
	public event Delegate EventBlessingDeleteAll;

	public event Delegate EventOwnerProspectQuestRewardReceive;
	public event Delegate EventOwnerProspectQuestRewardReceiveAll;
	public event Delegate EventTargetProspectQuestRewardReceive;
	public event Delegate EventTargetProspectQuestRewardReceiveAll;

	public event Delegate EventBlessingQuestStarted;
	public event Delegate EventBlessingReceived;

	public event Delegate<CsHeroProspectQuest> EventOwnerProspectQuestCompleted;
	public event Delegate<CsHeroProspectQuest> EventOwnerProspectQuestFailed;
	public event Delegate<CsHeroProspectQuest> EventOwnerProspectQuestTargetLevelUpdated;
	public event Delegate<CsHeroProspectQuest> EventTargetProspectQuestStarted;
	public event Delegate<CsHeroProspectQuest> EventTargetProspectQuestCompleted;
	public event Delegate<CsHeroProspectQuest> EventTargetProspectQuestFailed;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroProspectQuest[] ownerProspectQuests, PDHeroProspectQuest[] targetProspectQuests)
	{
		UnInit();

		m_listCsHeroProspectQuestOwner = new List<CsHeroProspectQuest>();

		for (int i = 0; i < ownerProspectQuests.Length; i++)
		{
			m_listCsHeroProspectQuestOwner.Add(new CsHeroProspectQuest(ownerProspectQuests[i]));
		}

		m_listCsHeroProspectQuestTarget = new List<CsHeroProspectQuest>();

		for (int i = 0; i < targetProspectQuests.Length; i++)
		{
			m_listCsHeroProspectQuestTarget.Add(new CsHeroProspectQuest(targetProspectQuests[i]));
		}

		m_listCsHeroBlessingQuest = new List<CsHeroBlessingQuest>();

		m_listCsHeroBlessingReceived = new List<CsHeroBlessing>();

		// Command
		CsRplzSession.Instance.EventResBlessingQuestBlessingSend += OnEventResBlessingQuestBlessingSend;
		CsRplzSession.Instance.EventResBlessingQuestDeleteAll += OnEventResBlessingQuestDeleteAll;
		CsRplzSession.Instance.EventResBlessingRewardReceive += OnEventResBlessingRewardReceive;
		CsRplzSession.Instance.EventResBlessingDeleteAll += OnEventResBlessingDeleteAll;

		CsRplzSession.Instance.EventResOwnerProspectQuestRewardReceive += OnEventResOwnerProspectQuestRewardReceive;
		CsRplzSession.Instance.EventResOwnerProspectQuestRewardReceiveAll += OnEventResOwnerProspectQuestRewardReceiveAll;
		CsRplzSession.Instance.EventResTargetProspectQuestRewardReceive += OnEventResTargetProspectQuestRewardReceive;
		CsRplzSession.Instance.EventResTargetProspectQuestRewardReceiveAll += OnEventResTargetProspectQuestRewardReceiveAll;

		// Event
		CsRplzSession.Instance.EventEvtBlessingQuestStarted += OnEventEvtBlessingQuestStarted;
		CsRplzSession.Instance.EventEvtBlessingReceived += OnEventEvtBlessingReceived;
		CsRplzSession.Instance.EventEvtBlessingThanksMessageReceived += OnEventEvtBlessingThanksMessageReceived;

		CsRplzSession.Instance.EventEvtOwnerProspectQuestCompleted += OnEventEvtOwnerProspectQuestCompleted;
		CsRplzSession.Instance.EventEvtOwnerProspectQuestFailed += OnEventEvtOwnerProspectQuestFailed;
		CsRplzSession.Instance.EventEvtOwnerProspectQuestTargetLevelUpdated += OnEventEvtOwnerProspectQuestTargetLevelUpdated;
		CsRplzSession.Instance.EventEvtTargetProspectQuestStarted += OnEventEvtTargetProspectQuestStarted;
		CsRplzSession.Instance.EventEvtTargetProspectQuestCompleted += OnEventEvtTargetProspectQuestCompleted;
		CsRplzSession.Instance.EventEvtTargetProspectQuestFailed += OnEventEvtTargetProspectQuestFailed;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResBlessingQuestBlessingSend -= OnEventResBlessingQuestBlessingSend;
		CsRplzSession.Instance.EventResBlessingQuestDeleteAll -= OnEventResBlessingQuestDeleteAll;
		CsRplzSession.Instance.EventResBlessingRewardReceive -= OnEventResBlessingRewardReceive;
		CsRplzSession.Instance.EventResBlessingDeleteAll -= OnEventResBlessingDeleteAll;

		CsRplzSession.Instance.EventResOwnerProspectQuestRewardReceive -= OnEventResOwnerProspectQuestRewardReceive;
		CsRplzSession.Instance.EventResOwnerProspectQuestRewardReceiveAll -= OnEventResOwnerProspectQuestRewardReceiveAll;
		CsRplzSession.Instance.EventResTargetProspectQuestRewardReceive -= OnEventResTargetProspectQuestRewardReceive;
		CsRplzSession.Instance.EventResTargetProspectQuestRewardReceiveAll -= OnEventResTargetProspectQuestRewardReceiveAll;

		// Event
		CsRplzSession.Instance.EventEvtBlessingQuestStarted -= OnEventEvtBlessingQuestStarted;
		CsRplzSession.Instance.EventEvtBlessingReceived -= OnEventEvtBlessingReceived;
		CsRplzSession.Instance.EventEvtBlessingThanksMessageReceived -= OnEventEvtBlessingThanksMessageReceived;

		CsRplzSession.Instance.EventEvtOwnerProspectQuestCompleted -= OnEventEvtOwnerProspectQuestCompleted;
		CsRplzSession.Instance.EventEvtOwnerProspectQuestFailed -= OnEventEvtOwnerProspectQuestFailed;
		CsRplzSession.Instance.EventEvtOwnerProspectQuestTargetLevelUpdated -= OnEventEvtOwnerProspectQuestTargetLevelUpdated;
		CsRplzSession.Instance.EventEvtTargetProspectQuestStarted -= OnEventEvtTargetProspectQuestStarted;
		CsRplzSession.Instance.EventEvtTargetProspectQuestCompleted -= OnEventEvtTargetProspectQuestCompleted;
		CsRplzSession.Instance.EventEvtTargetProspectQuestFailed -= OnEventEvtTargetProspectQuestFailed;

		m_bWaitResponse = false;

		if (m_listCsHeroBlessingQuest != null)
		{
			m_listCsHeroBlessingQuest.Clear();
			m_listCsHeroBlessingQuest = null;
		}

		if (m_listCsHeroProspectQuestOwner != null)
		{
			m_listCsHeroProspectQuestOwner.Clear();
			m_listCsHeroProspectQuestOwner = null;
		}

		if (m_listCsHeroProspectQuestTarget != null)
		{
			m_listCsHeroProspectQuestTarget.Clear();
			m_listCsHeroProspectQuestTarget = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroBlessingQuest GetHeroBlessingQuest(long lQuestId)
	{
		for (int i = 0; i < m_listCsHeroBlessingQuest.Count; i++)
		{
			if (m_listCsHeroBlessingQuest[i].Id == lQuestId)
				return m_listCsHeroBlessingQuest[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroBlessingQuest(long lQuestId)
	{
		CsHeroBlessingQuest csHeroBlessingQuest = GetHeroBlessingQuest(lQuestId);

		if (csHeroBlessingQuest != null)
			m_listCsHeroBlessingQuest.Remove(csHeroBlessingQuest);
	}


	//---------------------------------------------------------------------------------------------------
	CsHeroBlessing GetHeroBlessing(long lInstanceId)
	{
		for (int i = 0; i < m_listCsHeroBlessingReceived.Count; i++)
		{
			if (m_listCsHeroBlessingReceived[i].InstanceId == lInstanceId)
				return m_listCsHeroBlessingReceived[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroBlessing(long lInstanceId)
	{
		CsHeroBlessing csHeroBlessing = GetHeroBlessing(lInstanceId);

		if (csHeroBlessing != null)
			m_listCsHeroBlessingReceived.Remove(csHeroBlessing);
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroProspectQuest GetHeroProspectQuestOwner(Guid guidInstanceId)
	{
		for (int i = 0; i < m_listCsHeroProspectQuestOwner.Count; i++)
		{
			if (m_listCsHeroProspectQuestOwner[i].InstanceId == guidInstanceId)
				return m_listCsHeroProspectQuestOwner[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroProspectQuestOwner(Guid guidInstanceId)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestOwner(guidInstanceId);

		if (csHeroProspectQuest != null)
			m_listCsHeroProspectQuestOwner.Remove(csHeroProspectQuest);
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroProspectQuest GetHeroProspectQuestTarget(Guid guidInstanceId)
	{
		for (int i = 0; i < m_listCsHeroProspectQuestTarget.Count; i++)
		{
			if (m_listCsHeroProspectQuestTarget[i].InstanceId == guidInstanceId)
				return m_listCsHeroProspectQuestTarget[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroProspectQuestTarget(Guid guidInstanceId)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestTarget(guidInstanceId);

		if (csHeroProspectQuest != null)
			m_listCsHeroProspectQuestTarget.Remove(csHeroProspectQuest);
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckProspectQuest()
	{
		foreach (CsHeroProspectQuest csHeroProspectQuest in m_listCsHeroProspectQuestOwner)
		{
			if (csHeroProspectQuest.IsCompleted)
				return true;
		}

		foreach (CsHeroProspectQuest csHeroProspectQuest in m_listCsHeroProspectQuestTarget)
		{
			if (csHeroProspectQuest.IsCompleted)
				return true;
		}

		return false;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 축복퀘스트축복발송
	public void SendBlessingQuestBlessingSend(long lQuestId, int nBlessingId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlessingQuestBlessingSendCommandBody cmdBody = new BlessingQuestBlessingSendCommandBody();
			cmdBody.questId = m_lQuestId = lQuestId;
			cmdBody.blessingId = m_nBlessingId = nBlessingId;
			CsRplzSession.Instance.Send(ClientCommandName.BlessingQuestBlessingSend, cmdBody);
		}
	}

	void OnEventResBlessingQuestBlessingSend(int nReturnCode, BlessingQuestBlessingSendResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			RemoveHeroBlessingQuest(m_lQuestId);

			if (resBody.newProspectQuest != null)
			{
				m_listCsHeroProspectQuestOwner.Add(new CsHeroProspectQuest(resBody.newProspectQuest));
			}

			if (EventBlessingQuestBlessingSend != null)
			{
				EventBlessingQuestBlessingSend(m_lQuestId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 골드가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 다이아가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 대상영웅이 로그인중이 아닙니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 대상영웅의 받은 축복수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 나의 소유유망자퀘스트수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 대상영웅의 대상유망자퀘스트수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A141_ERROR_00107"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 축복퀘스트모두삭제
	public void SendBlessingQuestDeleteAll()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlessingQuestDeleteAllCommandBody cmdBody = new BlessingQuestDeleteAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BlessingQuestDeleteAll, cmdBody);
		}
	}

	void OnEventResBlessingQuestDeleteAll(int nReturnCode, BlessingQuestDeleteAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroBlessingQuest.Clear();

			if (EventBlessingQuestDeleteAll != null)
			{
				EventBlessingQuestDeleteAll();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 축복보상받기
	public void SendBlessingRewardReceive(long lInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlessingRewardReceiveCommandBody cmdBody = new BlessingRewardReceiveCommandBody();
			cmdBody.instanceId = m_lInstanceId = lInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.BlessingRewardReceive, cmdBody);
		}
	}

	void OnEventResBlessingRewardReceive(int nReturnCode, BlessingRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			RemoveHeroBlessing(m_lInstanceId);

			if (resBody.friendApplicationResult == 0)
			{
				CsFriendManager.Instance.FriendApplicationList.Add(new CsFriendApplication(resBody.friendAppication));
			}
			else
			{
				switch (resBody.friendApplicationResult)
				{
					case 1:     // 대상영웅에 대한 친구신청이 존재합니다.
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A142_ERROR_00103"));
						break;

					case 2:     // 대상영웅은 이미 친구입니다.
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A142_ERROR_00104"));
						break;

					case 3:     // 대상영웅은 나의 블랙리스트에 존재합니다.
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A142_ERROR_00105"));
						break;

					case 4:     // 대상영웅이 로그인중이 아닙니다.
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A142_ERROR_00106"));
						break;

					case 5:     // 내가 대상영웅의 블랙리스트에 존재합니다.
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A142_ERROR_00107"));
						break;
				}
			}

			if (EventBlessingRewardReceive != null)
			{
				EventBlessingRewardReceive(m_lInstanceId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 축복이 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A142_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 축복모두삭제 
	public void SendBlessingDeleteAll()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlessingDeleteAllCommandBody cmdBody = new BlessingDeleteAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.BlessingDeleteAll, cmdBody);
		}
	}

	void OnEventResBlessingDeleteAll(int nReturnCode, BlessingDeleteAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroBlessingReceived.Clear();
			
			if (EventBlessingDeleteAll != null)
			{
				EventBlessingDeleteAll();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트보상받기
	public void SendOwnerProspectQuestRewardReceive(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			OwnerProspectQuestRewardReceiveCommandBody cmdBody = new OwnerProspectQuestRewardReceiveCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.OwnerProspectQuestRewardReceive, cmdBody);
		}
	}

	void OnEventResOwnerProspectQuestRewardReceive(int nReturnCode, OwnerProspectQuestRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			RemoveHeroProspectQuestOwner(m_guidInstanceId);

			if (EventOwnerProspectQuestRewardReceive != null)
			{
				EventOwnerProspectQuestRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A143_ERROR_00101"));
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 완료되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A143_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트보상모두받기
	public void SendOwnerProspectQuestRewardReceiveAll()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			OwnerProspectQuestRewardReceiveAllCommandBody cmdBody = new OwnerProspectQuestRewardReceiveAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.OwnerProspectQuestRewardReceiveAll, cmdBody);
		}
	}

	void OnEventResOwnerProspectQuestRewardReceiveAll(int nReturnCode, OwnerProspectQuestRewardReceiveAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			for (int i = 0; i < resBody.receivedInstanceIds.Length; i++)
			{ 
				RemoveHeroProspectQuestOwner(resBody.receivedInstanceIds[i]);
			}

			if (EventOwnerProspectQuestRewardReceiveAll != null)
			{
				EventOwnerProspectQuestRewardReceiveAll();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 대상유망자퀘스트보상받기
	public void SendTargetProspectQuestRewardReceive(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			TargetProspectQuestRewardReceiveCommandBody cmdBody = new TargetProspectQuestRewardReceiveCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.TargetProspectQuestRewardReceive, cmdBody);
		}
	}

	void OnEventResTargetProspectQuestRewardReceive(int nReturnCode, TargetProspectQuestRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			RemoveHeroProspectQuestTarget(m_guidInstanceId);

			if (EventTargetProspectQuestRewardReceive != null)
			{
				EventTargetProspectQuestRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A143_ERROR_00301"));
		}
		else if (nReturnCode == 101)
		{
			// 퀘스트가 완료되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A143_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 대상유망자퀘스트보상모두받기
	public void SendTargetProspectQuestRewardReceiveAll()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			TargetProspectQuestRewardReceiveAllCommandBody cmdBody = new TargetProspectQuestRewardReceiveAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.TargetProspectQuestRewardReceiveAll, cmdBody);
		}
	}

	void OnEventResTargetProspectQuestRewardReceiveAll(int nReturnCode, TargetProspectQuestRewardReceiveAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			for (int i = 0; i < resBody.receivedInstanceIds.Length; i++)
			{
				RemoveHeroProspectQuestTarget(resBody.receivedInstanceIds[i]);
			}

			if (EventTargetProspectQuestRewardReceiveAll != null)
			{
				EventTargetProspectQuestRewardReceiveAll();
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
	// 축복퀘스트시작
	void OnEventEvtBlessingQuestStarted(SEBBlessingQuestStartedEventBody eventBody)
	{
		m_listCsHeroBlessingQuest.Add(new CsHeroBlessingQuest(eventBody.quest));

		if (EventBlessingQuestStarted != null)
		{
			EventBlessingQuestStarted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 축복받음
	void OnEventEvtBlessingReceived(SEBBlessingReceivedEventBody eventBody)
	{
		m_listCsHeroBlessingReceived.Add(new CsHeroBlessing(eventBody.blessing));

		if (EventBlessingReceived != null)
		{
			EventBlessingReceived();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 축복감사메시지 수신
	void OnEventEvtBlessingThanksMessageReceived(SEBBlessingThanksMessageReceivedEventBody eventBody)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_02014"), eventBody.senderName));
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트완료
	void OnEventEvtOwnerProspectQuestCompleted(SEBOwnerProspectQuestCompletedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestOwner(eventBody.instanceId);

		if (csHeroProspectQuest != null)
		{
			csHeroProspectQuest.IsCompleted = true;

			if (EventOwnerProspectQuestCompleted != null)
			{
				EventOwnerProspectQuestCompleted(csHeroProspectQuest);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트실패
	void OnEventEvtOwnerProspectQuestFailed(SEBOwnerProspectQuestFailedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestOwner(eventBody.instanceId);

		if (csHeroProspectQuest != null)
		{
			RemoveHeroProspectQuestOwner(eventBody.instanceId);

			if (EventOwnerProspectQuestFailed != null)
			{
				EventOwnerProspectQuestFailed(csHeroProspectQuest);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 소유유망자퀘스트대상레벨갱신
	void OnEventEvtOwnerProspectQuestTargetLevelUpdated(SEBOwnerProspectQuestTargetLevelUpdatedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestOwner(eventBody.instanceId);

		if (csHeroProspectQuest != null)
		{
			csHeroProspectQuest.TargetLevel = eventBody.targetLevel;

			if (EventOwnerProspectQuestTargetLevelUpdated != null)
			{
				EventOwnerProspectQuestTargetLevelUpdated(csHeroProspectQuest);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 대상유망자퀘스트시작
	void OnEventEvtTargetProspectQuestStarted(SEBTargetProspectQuestStartedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = new CsHeroProspectQuest(eventBody.quest);
		m_listCsHeroProspectQuestTarget.Add(csHeroProspectQuest);

		if (EventTargetProspectQuestStarted != null)
		{
			EventTargetProspectQuestStarted(csHeroProspectQuest);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 대상유망자퀘스트완료
	void OnEventEvtTargetProspectQuestCompleted(SEBTargetProspectQuestCompletedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestTarget(eventBody.instanceId);

		if (csHeroProspectQuest != null)
		{
			csHeroProspectQuest.IsCompleted = true;

			if (EventTargetProspectQuestCompleted != null)
			{
				EventTargetProspectQuestCompleted(csHeroProspectQuest);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 대상유망자퀘스트실패
	void OnEventEvtTargetProspectQuestFailed(SEBTargetProspectQuestFailedEventBody eventBody)
	{
		CsHeroProspectQuest csHeroProspectQuest = GetHeroProspectQuestTarget(eventBody.instanceId);

		if (csHeroProspectQuest != null)
		{
			RemoveHeroProspectQuestTarget(eventBody.instanceId);

			if (EventTargetProspectQuestFailed != null)
			{
				EventTargetProspectQuestFailed(csHeroProspectQuest);
			}
		}
	}

	#endregion Protocol.Event

}
