using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public class CsFriendManager
{
	bool m_bWaitResponse = false;

	List<CsFriend> m_listCsFriend;
	List<CsTempFriend> m_listCsTempFriend;
	List<CsBlacklistEntry> m_listCsBlacklistEntry;
	List<CsDeadRecord> m_listCsDeadRecord;

	List<CsFriendApplication> m_listCsFriendApplication;
	List<CsFriendApplication> m_listCsFriendApplicationReceived;

	Guid[] m_aguidIds;
	long m_lApplicationNo;
	Guid m_guidTargetHeroId;

	//---------------------------------------------------------------------------------------------------
	public static CsFriendManager Instance
	{
		get { return CsSingleton<CsFriendManager>.GetInstance(); }
	}

	public List<CsFriend> FriendList
	{
		get { return m_listCsFriend; }
	}
	
	public List<CsTempFriend> TempFriendList
	{
		get { return m_listCsTempFriend; }
	}

	public List<CsBlacklistEntry> BlacklistEntryList
	{
		get { return m_listCsBlacklistEntry; }
	}

	public List<CsDeadRecord> DeadRecordList
	{
		get { return m_listCsDeadRecord; }
	}

	public List<CsFriendApplication> FriendApplicationList
	{
		get { return m_listCsFriendApplication; }
	}

	public List<CsFriendApplication> FriendApplicationReceivedList
	{
		get { return m_listCsFriendApplicationReceived; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventFriendList;
	public event Delegate EventFriendDelete;
	public event Delegate EventFriendApply;
	public event Delegate EventFriendApplicationAccept;
	public event Delegate EventFriendApplicationRefuse;
	public event Delegate<PDSearchHero[]> EventHeroSearchForFriendApplication;
	public event Delegate EventBlacklistEntryAdd;
	public event Delegate EventBlacklistEntryDelete;

	public event Delegate EventFriendApplicationReceived;
	public event Delegate EventFriendApplicationCanceled;
	public event Delegate EventFriendApplicationAccepted;
	public event Delegate EventFriendApplicationRefused;
	public event Delegate EventTempFriendAdded;
	public event Delegate EventDeadRecordAdded;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDFriend[] friends, PDTempFriend[] tempFriends, PDBlacklistEntry[] blacklistEntries, PDDeadRecord[] deadRecords)
	{
		UnInit();

		m_listCsFriend = new List<CsFriend>();

		for (int i = 0; i < friends.Length; i++)
		{
			m_listCsFriend.Add(new CsFriend(friends[i]));
		}

		m_listCsTempFriend = new List<CsTempFriend>();

		for (int i = 0; i < tempFriends.Length; i++)
		{
			m_listCsTempFriend.Add(new CsTempFriend(tempFriends[i]));
		}

		m_listCsBlacklistEntry = new List<CsBlacklistEntry>();

		for (int i = 0; i < blacklistEntries.Length; i++)
		{
			m_listCsBlacklistEntry.Add(new CsBlacklistEntry(blacklistEntries[i]));
		}

		m_listCsDeadRecord = new List<CsDeadRecord>();

		for (int i = 0; i < deadRecords.Length; i++)
		{
			m_listCsDeadRecord.Add(new CsDeadRecord(deadRecords[i]));
		}

		m_listCsFriendApplication = new List<CsFriendApplication>();
		m_listCsFriendApplicationReceived  = new List<CsFriendApplication>();


		// Command
		CsRplzSession.Instance.EventResFriendList += OnEventResFriendList;
		CsRplzSession.Instance.EventResFriendDelete += OnEventResFriendDelete;
		CsRplzSession.Instance.EventResFriendApply += OnEventResFriendApply;
		CsRplzSession.Instance.EventResFriendApplicationAccept += OnEventResFriendApplicationAccept;
		CsRplzSession.Instance.EventResFriendApplicationRefuse += OnEventResFriendApplicationRefuse;
		CsRplzSession.Instance.EventResHeroSearchForFriendApplication += OnEventResHeroSearchForFriendApplication;
		CsRplzSession.Instance.EventResBlacklistEntryAdd += OnEventResBlacklistEntryAdd;
		CsRplzSession.Instance.EventResBlacklistEntryDelete += OnEventResBlacklistEntryDelete;

		// Event
		CsRplzSession.Instance.EventEvtFriendApplicationReceived += OnEventEvtFriendApplicationReceived;
		CsRplzSession.Instance.EventEvtFriendApplicationCanceled += OnEventEvtFriendApplicationCanceled;
		CsRplzSession.Instance.EventEvtFriendApplicationAccepted += OnEventEvtFriendApplicationAccepted;
		CsRplzSession.Instance.EventEvtFriendApplicationRefused += OnEventEvtFriendApplicationRefused;
		CsRplzSession.Instance.EventEvtTempFriendAdded += OnEventEvtTempFriendAdded;
		CsRplzSession.Instance.EventEvtDeadRecordAdded += OnEventEvtDeadRecordAdded;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResFriendList -= OnEventResFriendList;
		CsRplzSession.Instance.EventResFriendDelete -= OnEventResFriendDelete;
		CsRplzSession.Instance.EventResFriendApply -= OnEventResFriendApply;
		CsRplzSession.Instance.EventResFriendApplicationAccept -= OnEventResFriendApplicationAccept;
		CsRplzSession.Instance.EventResFriendApplicationRefuse -= OnEventResFriendApplicationRefuse;
		CsRplzSession.Instance.EventResHeroSearchForFriendApplication -= OnEventResHeroSearchForFriendApplication;
		CsRplzSession.Instance.EventResBlacklistEntryAdd -= OnEventResBlacklistEntryAdd;
		CsRplzSession.Instance.EventResBlacklistEntryDelete -= OnEventResBlacklistEntryDelete;

		// Event
		CsRplzSession.Instance.EventEvtFriendApplicationReceived -= OnEventEvtFriendApplicationReceived;
		CsRplzSession.Instance.EventEvtFriendApplicationCanceled -= OnEventEvtFriendApplicationCanceled;
		CsRplzSession.Instance.EventEvtFriendApplicationAccepted -= OnEventEvtFriendApplicationAccepted;
		CsRplzSession.Instance.EventEvtFriendApplicationRefused -= OnEventEvtFriendApplicationRefused;
		CsRplzSession.Instance.EventEvtTempFriendAdded -= OnEventEvtTempFriendAdded;
		CsRplzSession.Instance.EventEvtDeadRecordAdded -= OnEventEvtDeadRecordAdded;

		m_bWaitResponse = false;

		if (m_listCsFriend != null)
		{ 
			m_listCsFriend.Clear();
			m_listCsFriend = null;
		}

		if (m_listCsTempFriend != null)
		{ 
			m_listCsTempFriend.Clear();
			m_listCsTempFriend = null;
		}

		if (m_listCsBlacklistEntry != null)
		{
			m_listCsBlacklistEntry.Clear();
			m_listCsBlacklistEntry = null;
		}

		if (m_listCsDeadRecord != null)
		{
			m_listCsDeadRecord.Clear();
			m_listCsDeadRecord = null;
		}

		if (m_listCsFriendApplication != null)
		{
			m_listCsFriendApplication.Clear();
			m_listCsFriendApplication = null;
		}

		if (m_listCsFriendApplicationReceived != null)
		{
			m_listCsFriendApplicationReceived.Clear();
			m_listCsFriendApplicationReceived = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsFriend GetFriend(Guid guidId)
	{
		for (int i = 0; i < m_listCsFriend.Count; i++)
		{
			if (m_listCsFriend[i].Id == guidId)
				return m_listCsFriend[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveFriend(Guid guidId)
	{
		CsFriend csFriend = GetFriend(guidId);

		if (csFriend != null)
		{
			m_listCsFriend.Remove(csFriend);
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsTempFriend GetTempFriend(Guid guidId)
	{
		for (int i = 0; i < m_listCsTempFriend.Count; i++)
		{
			if (m_listCsTempFriend[i].Id == guidId)
				return m_listCsTempFriend[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsBlacklistEntry GetBlacklistEntry(Guid guidHeroId)
	{
		for (int i = 0; i < m_listCsBlacklistEntry.Count; i++)
		{
			if (m_listCsBlacklistEntry[i].HeroId == guidHeroId)
				return m_listCsBlacklistEntry[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	CsDeadRecord GetDeadRecord(Guid guidId)
	{
		for (int i = 0; i < m_listCsDeadRecord.Count; i++)
		{
			if (m_listCsDeadRecord[i].Id == guidId)
				return m_listCsDeadRecord[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	CsFriendApplication GetFriendApplication(long lApplicationNo)
	{
		for (int i = 0; i < m_listCsFriendApplication.Count; i++)
		{
			if (m_listCsFriendApplication[i].No == lApplicationNo)
				return m_listCsFriendApplication[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	CsFriendApplication GetFriendApplicationReceived(long lApplicationNo)
	{
		for (int i = 0; i < m_listCsFriendApplicationReceived.Count; i++)
		{
			if (m_listCsFriendApplicationReceived[i].No == lApplicationNo)
				return m_listCsFriendApplicationReceived[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveFriendApplication(long lApplicationNo)
	{
		CsFriendApplication csFriendApplication = GetFriendApplication(lApplicationNo);

		if (csFriendApplication != null)
		{
			m_listCsFriendApplication.Remove(csFriendApplication);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveFriendApplicationReceived(long lApplicationNo)
	{
		CsFriendApplication csFriendApplication = GetFriendApplicationReceived(lApplicationNo);

		if (csFriendApplication != null)
		{
			m_listCsFriendApplicationReceived.Remove(csFriendApplication);
		}
	}

	//---------------------------------------------------------------------------------------------------

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 친구목록
	public void SendFriendList()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FriendListCommandBody cmdBody = new FriendListCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FriendList, cmdBody);
		}
	}

	void OnEventResFriendList(int nReturnCode, FriendListResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			for (int i = 0; i < resBody.friends.Length; i++)
			{
				CsFriend csFriend = GetFriend(resBody.friends[i].id);

				if (csFriend == null)
				{
					m_listCsFriend.Add(new CsFriend(resBody.friends[i]));
				}
				else
				{
					csFriend.Update(resBody.friends[i]);
				}
			}

			if (EventFriendList != null)
			{
				EventFriendList();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구삭제
	public void SendFriendDelete(Guid[] aguidIds)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FriendDeleteCommandBody cmdBody = new FriendDeleteCommandBody();
			cmdBody.friendIds = m_aguidIds = aguidIds;
			CsRplzSession.Instance.Send(ClientCommandName.FriendDelete, cmdBody);
		}
	}

	void OnEventResFriendDelete(int nReturnCode, FriendDeleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			for (int i = 0; i < m_aguidIds.Length; i++)
			{
				RemoveFriend(m_aguidIds[i]);
			}

			if (EventFriendDelete != null)
			{
				EventFriendDelete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 친구가 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 친구ID가 중복되었습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청
	public void SendFriendApply(Guid guidTargetHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FriendApplyCommandBody cmdBody = new FriendApplyCommandBody();
			cmdBody.targetHeroId = guidTargetHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.FriendApply, cmdBody);
		}
	}

	void OnEventResFriendApply(int nReturnCode, FriendApplyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsFriendApplication.Add(new CsFriendApplication(resBody.app));

			if (EventFriendApply != null)
			{
				EventFriendApply();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상영웅에 대한 친구신청이 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 대상은 이미 친구입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 대상영웅이 로그인중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 대상은 나의 블랙리스트에 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00304"));
		}
		else if (nReturnCode == 105)
		{
			// 나는 대상영웅의 블랙리스트에 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00305"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청수락
	public void SendFriendApplicationAccept(long lApplicationNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FriendApplicationAcceptCommandBody cmdBody = new FriendApplicationAcceptCommandBody();
			cmdBody.applicationNo = m_lApplicationNo = lApplicationNo;
			CsRplzSession.Instance.Send(ClientCommandName.FriendApplicationAccept, cmdBody);
		}
	}

	void OnEventResFriendApplicationAccept(int nReturnCode, FriendApplicationAcceptResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveFriendApplicationReceived(m_lApplicationNo);

            if (resBody.newFriend == null)
            {

            }
            else
            {
                CsFriend csFrend = GetFriend(resBody.newFriend.id);

                if (csFrend == null)
                {
                    m_listCsFriend.Add(new CsFriend(resBody.newFriend));
                }
            }

			if (EventFriendApplicationAccept != null)
			{
				EventFriendApplicationAccept();
			}
		}
		else if (nReturnCode == 101)
		{
			// 신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 나의 친구수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 신청자의 친구수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00403"));
		}
		else if (nReturnCode == 104)
		{
			// 신청자는 나의 블랙리스트에 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00404"));
		}
		else if (nReturnCode == 105)
		{
			// 나는 신청자의 블랙리스트에 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00405"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청거절
	public void SendFriendApplicationRefuse(long lApplicationNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FriendApplicationRefuseCommandBody cmdBody = new FriendApplicationRefuseCommandBody();
			cmdBody.applicationNo = m_lApplicationNo = lApplicationNo;
			CsRplzSession.Instance.Send(ClientCommandName.FriendApplicationRefuse, cmdBody);
		}
	}

	void OnEventResFriendApplicationRefuse(int nReturnCode, FriendApplicationRefuseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveFriendApplicationReceived(m_lApplicationNo);

			if (EventFriendApplicationRefuse != null)
			{
				EventFriendApplicationRefuse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 신청이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A137_ERROR_00501"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청을위한영웅검색
	public void SendHeroSearchForFriendApplication(string strText)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			HeroSearchForFriendApplicationCommandBody cmdBody = new HeroSearchForFriendApplicationCommandBody();
			cmdBody.text = strText;
			CsRplzSession.Instance.Send(ClientCommandName.HeroSearchForFriendApplication, cmdBody);
		}
	}

	void OnEventResHeroSearchForFriendApplication(int nReturnCode, HeroSearchForFriendApplicationResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventHeroSearchForFriendApplication != null)
			{
				EventHeroSearchForFriendApplication(resBody.results);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 블랙리스트항목추가
	public void SendBlacklistEntryAdd(Guid guidTargetHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlacklistEntryAddCommandBody cmdBody = new BlacklistEntryAddCommandBody();
			cmdBody.targetHeroId = m_guidTargetHeroId = guidTargetHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.BlacklistEntryAdd, cmdBody);
		}
	}

	void OnEventResBlacklistEntryAdd(int nReturnCode, BlacklistEntryAddResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsBlacklistEntry csBlacklistEntry = GetBlacklistEntry(resBody.newEntry.heroId);

			if (csBlacklistEntry == null)
			{
				m_listCsBlacklistEntry.Add(new CsBlacklistEntry(resBody.newEntry));
			}

			RemoveFriend(m_guidTargetHeroId);

			if (EventBlacklistEntryAdd != null)
			{
				EventBlacklistEntryAdd();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 존재합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A138_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 블랙리스트 항목수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A138_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 블랙리스트항목삭제 
	public void SendBlacklistEntryDelete(Guid guidTargetHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			BlacklistEntryDeleteCommandBody cmdBody = new BlacklistEntryDeleteCommandBody();
			cmdBody.targetHeroId = m_guidTargetHeroId = guidTargetHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.BlacklistEntryDelete, cmdBody);
		}
	}

	void OnEventResBlacklistEntryDelete(int nReturnCode, BlacklistEntryDeleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsBlacklistEntry csBlacklistEntry = GetBlacklistEntry(m_guidTargetHeroId);

			if (csBlacklistEntry != null)
			{
				m_listCsBlacklistEntry.Remove(csBlacklistEntry);
			}

			if (EventBlacklistEntryDelete != null)
			{
				EventBlacklistEntryDelete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상영웅은 블랙리스트에 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A138_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 친구신청받음
	void OnEventEvtFriendApplicationReceived(SEBFriendApplicationReceivedEventBody eventBody)
	{
		m_listCsFriendApplicationReceived.Add(new CsFriendApplication(eventBody.app));

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept))
        {
            int nAutoAccept = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyFriendApplicationAutoAccept);

            if (nAutoAccept == 1)
            {
                SendFriendApplicationAccept(eventBody.app.no);
            }
            else
            {
                if (EventFriendApplicationReceived != null)
                {
                    EventFriendApplicationReceived();
                }
            }
        }
        else
        {
            if (EventFriendApplicationReceived != null)
            {
                EventFriendApplicationReceived();
            }
        }
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청취소
	void OnEventEvtFriendApplicationCanceled(SEBFriendApplicationCanceledEventBody eventBody)
	{
		RemoveFriendApplicationReceived(eventBody.applicationNo);

		if (EventFriendApplicationCanceled != null)
		{
			EventFriendApplicationCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청수락
	void OnEventEvtFriendApplicationAccepted(SEBFriendApplicationAcceptedEventBody eventBody)
	{
		RemoveFriendApplication(eventBody.applicationNo);

		if (eventBody.newFriend != null)
		{
			CsFriend csFrend = GetFriend(eventBody.newFriend.id);

			if (csFrend == null)
			{
				m_listCsFriend.Add(new CsFriend(eventBody.newFriend));
			}
		}

		if (EventFriendApplicationAccepted != null)
		{
			EventFriendApplicationAccepted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 친구신청거절
	void OnEventEvtFriendApplicationRefused(SEBFriendApplicationRefusedEventBody eventBody)
	{
		RemoveFriendApplication(eventBody.applicationNo);

		if (EventFriendApplicationRefused != null)
		{
			EventFriendApplicationRefused();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 임시친구추가
	void OnEventEvtTempFriendAdded(SEBTempFriendAddedEventBody eventBody)
	{
		if (eventBody.tempFriend != null)
		{
			m_listCsTempFriend.Add(new CsTempFriend(eventBody.tempFriend));
		}

		if (eventBody.removedTempFriendId != Guid.Empty)
		{
			CsTempFriend csTempFriend = GetTempFriend(eventBody.removedTempFriendId);

			if (csTempFriend != null)
			{
				m_listCsTempFriend.Remove(csTempFriend);
			}
		}

		if (EventTempFriendAdded != null)
		{
			EventTempFriendAdded();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 사망기록추가
	void OnEventEvtDeadRecordAdded(SEBDeadRecordAddedEventBody eventBody)
	{
		if (eventBody.record != null)
		{
			m_listCsDeadRecord.Add(new CsDeadRecord(eventBody.record));
		}

		if (eventBody.removedRecordId != Guid.Empty)
		{
			CsDeadRecord csDeadRecord = GetDeadRecord(eventBody.removedRecordId);

			if (csDeadRecord != null)
			{
				m_listCsDeadRecord.Remove(csDeadRecord);
			}
		}

		if (EventDeadRecordAdded != null)
		{
			EventDeadRecordAdded();
		}
	}

	#endregion Protocol.Event

}
