using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public class CsPresentManager
{
	bool m_bWaitResponse = false;

	DateTime m_dtWeeklyPresentPopularityPointDate;
	int m_nWeeklyPresentPopularityPoint;                            // 주간선물인기점수
	DateTime m_dtWeeklyPresentContributionPointDate;
	int m_nWeeklyPresentContributionPoint;                          // 주간선물공헌점수

	int m_nNationWeeklyPresentPopularityPointRankingNo;             // 국가주간선물인기점수랭킹번호
	int m_nNationWeeklyPresentPopularityPointRanking;               // 나의 국가주간선물인기점수랭킹. 랭킹에 없을 경우 0
	int m_nRewardedNationWeeklyPresentPopularityPointRankingNo;     // 보상받은 국가주간선물인기점수랭킹번호.

	int m_nNationWeeklyPresentContributionPointRankingNo;           // 국가주간선물공헌점수랭킹번호
	int m_nNationWeeklyPresentContributionPointRanking;             // 나의 국가주간선물공헌점수랭킹. 랭킹에 없을 경우 0
	int m_nRewardedNationWeeklyPresentContributionPointRankingNo;   // 보상받은 국가주간선물공헌점수랭킹번호.

	List<CsHeroReceivedPresent> m_listCsHeroReceivedPresent;

    Guid m_guidSenderId = Guid.Empty;
    int m_nPresendId;

	//---------------------------------------------------------------------------------------------------
	public static CsPresentManager Instance
	{
		get { return CsSingleton<CsPresentManager>.GetInstance(); }
	}

	public DateTime WeeklyPresentPopularityPointDate
	{
		get { return m_dtWeeklyPresentPopularityPointDate; }
		set { m_dtWeeklyPresentPopularityPointDate = value; }
	}

	public int WeeklyPresentPopularityPoint
	{
		get { return m_nWeeklyPresentPopularityPoint; }
		set { m_nWeeklyPresentPopularityPoint = value; }
	}

	public DateTime WeeklyPresentContributionPointDate
	{
		get { return m_dtWeeklyPresentContributionPointDate; }
		set { m_dtWeeklyPresentContributionPointDate = value; }
	}
	
	public int WeeklyPresentContributionPoint
	{
		get { return m_nWeeklyPresentContributionPoint; }
		set { m_nWeeklyPresentContributionPoint = value; }
	}

	public int NationWeeklyPresentPopularityPointRankingNo
	{
		get { return m_nNationWeeklyPresentPopularityPointRankingNo; }
	}

	public int NationWeeklyPresentPopularityPointRanking
	{
		get { return m_nNationWeeklyPresentPopularityPointRanking; }
	}

	public int RewardedNationWeeklyPresentPopularityPointRankingNo
	{
		get { return m_nRewardedNationWeeklyPresentPopularityPointRankingNo; }
	}

	public int NationWeeklyPresentContributionPointRankingNo
	{
		get { return m_nNationWeeklyPresentContributionPointRankingNo; }
	}

	public int NationWeeklyPresentContributionPointRanking
	{
		get { return m_nNationWeeklyPresentContributionPointRanking; }
	}

	public int RewardedNationWeeklyPresentContributionPointRankingNo
	{
		get { return m_nRewardedNationWeeklyPresentContributionPointRankingNo; }
	}

    public List<CsHeroReceivedPresent> CsHeroReceivedPresentList
    {
        get { return m_listCsHeroReceivedPresent; }
    }

	//---------------------------------------------------------------------------------------------------
	public event Delegate<int> EventPresentSend;
	public event Delegate EventPresentReply;
	public event Delegate<CsPresentPopularityPointRanking, List<CsPresentPopularityPointRanking>> EventServerPresentPopularityPointRanking;
	public event Delegate<CsPresentPopularityPointRanking, List<CsPresentPopularityPointRanking>> EventNationWeeklyPresentPopularityPointRanking;
	public event Delegate EventNationWeeklyPresentPopularityPointRankingRewardReceive;
	public event Delegate<CsPresentContributionPointRanking, List<CsPresentContributionPointRanking>> EventServerPresentContributionPointRanking;
	public event Delegate<CsPresentContributionPointRanking, List<CsPresentContributionPointRanking>> EventNationWeeklyPresentContributionPointRanking;
	public event Delegate EventNationWeeklyPresentContributionPointRankingRewardReceive;

	public event Delegate EventPresentReceived;
	public event Delegate<Guid, string, int> EventPresentReplyReceived;
	public event Delegate<Guid, string, int, Guid, string, int, CsPresent> EventHeroPresent;
	public event Delegate EventNationWeeklyPresentPopularityPointRankingUpdated;
	public event Delegate EventNationWeeklyPresentContributionPointRankingUpdated;

	//---------------------------------------------------------------------------------------------------
	public void Init(int weeklyPresentPopularityPoint, int weeklyPresentContributionPoint, int nationWeeklyPresentPopularityPointRankingNo, int nationWeeklyPresentPopularityPointRanking, int rewardedNationWeeklyPresentPopularityPointRankingNo,
					 int nationWeeklyPresentContributionPointRankingNo, int nationWeeklyPresentContributionPointRanking, int rewardedNationWeeklyPresentContributionPointRankingNo, DateTime dtDate)
	{
		UnInit();

		DateTime dtStartWeek;

		if (dtDate.DayOfWeek == DayOfWeek.Sunday)
		{
			dtStartWeek = dtDate.AddDays(-6);
		}
		else if (dtDate.DayOfWeek == DayOfWeek.Monday)
		{
			dtStartWeek = dtDate;
		}
		else
		{
			dtStartWeek = dtDate.AddDays(1 - (int)dtDate.DayOfWeek);
		}

		m_dtWeeklyPresentPopularityPointDate = m_dtWeeklyPresentContributionPointDate = dtStartWeek;

		m_nWeeklyPresentPopularityPoint = weeklyPresentPopularityPoint; 
		m_nWeeklyPresentContributionPoint = weeklyPresentContributionPoint;

		m_nNationWeeklyPresentPopularityPointRankingNo = nationWeeklyPresentPopularityPointRankingNo;
		m_nNationWeeklyPresentPopularityPointRanking = nationWeeklyPresentPopularityPointRanking;
		m_nRewardedNationWeeklyPresentPopularityPointRankingNo = rewardedNationWeeklyPresentPopularityPointRankingNo;

		m_nNationWeeklyPresentContributionPointRankingNo = nationWeeklyPresentContributionPointRankingNo;
		m_nNationWeeklyPresentContributionPointRanking = nationWeeklyPresentContributionPointRanking;
		m_nRewardedNationWeeklyPresentContributionPointRankingNo = rewardedNationWeeklyPresentContributionPointRankingNo;

		m_listCsHeroReceivedPresent = new List<CsHeroReceivedPresent>();

		// Command
		CsRplzSession.Instance.EventResPresentSend += OnEventResPresentSend;
		CsRplzSession.Instance.EventResPresentReply += OnEventResPresentReply;
		CsRplzSession.Instance.EventResServerPresentPopularityPointRanking += OnEventResServerPresentPopularityPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentPopularityPointRanking += OnEventResNationWeeklyPresentPopularityPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentPopularityPointRankingRewardReceive += OnEventResNationWeeklyPresentPopularityPointRankingRewardReceive;
		CsRplzSession.Instance.EventResServerPresentContributionPointRanking += OnEventResServerPresentContributionPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentContributionPointRanking += OnEventResNationWeeklyPresentContributionPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentContributionPointRankingRewardReceive += OnEventResNationWeeklyPresentContributionPointRankingRewardReceive;

		// Event
		CsRplzSession.Instance.EventEvtPresentReceived += OnEventEvtPresentReceived;
		CsRplzSession.Instance.EventEvtPresentReplyReceived += OnEventEvtPresentReplyReceived;
		CsRplzSession.Instance.EventEvtHeroPresent += OnEventEvtHeroPresent;
		CsRplzSession.Instance.EventEvtNationWeeklyPresentPopularityPointRankingUpdated += OnEventEvtNationWeeklyPresentPopularityPointRankingUpdated;
		CsRplzSession.Instance.EventEvtNationWeeklyPresentContributionPointRankingUpdated += OnEventEvtNationWeeklyPresentContributionPointRankingUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResPresentSend -= OnEventResPresentSend;
		CsRplzSession.Instance.EventResPresentReply -= OnEventResPresentReply;
		CsRplzSession.Instance.EventResServerPresentPopularityPointRanking -= OnEventResServerPresentPopularityPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentPopularityPointRanking -= OnEventResNationWeeklyPresentPopularityPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentPopularityPointRankingRewardReceive -= OnEventResNationWeeklyPresentPopularityPointRankingRewardReceive;
		CsRplzSession.Instance.EventResServerPresentContributionPointRanking -= OnEventResServerPresentContributionPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentContributionPointRanking -= OnEventResNationWeeklyPresentContributionPointRanking;
		CsRplzSession.Instance.EventResNationWeeklyPresentContributionPointRankingRewardReceive -= OnEventResNationWeeklyPresentContributionPointRankingRewardReceive;

		// Event
		CsRplzSession.Instance.EventEvtPresentReceived -= OnEventEvtPresentReceived;
		CsRplzSession.Instance.EventEvtPresentReplyReceived -= OnEventEvtPresentReplyReceived;
		CsRplzSession.Instance.EventEvtHeroPresent -= OnEventEvtHeroPresent;
		CsRplzSession.Instance.EventEvtNationWeeklyPresentPopularityPointRankingUpdated -= OnEventEvtNationWeeklyPresentPopularityPointRankingUpdated;
		CsRplzSession.Instance.EventEvtNationWeeklyPresentContributionPointRankingUpdated -= OnEventEvtNationWeeklyPresentContributionPointRankingUpdated;

		m_bWaitResponse = false;

		if (m_listCsHeroReceivedPresent != null)
		{
			m_listCsHeroReceivedPresent.Clear();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetPoint(DateTime dtDate)
	{
		m_dtWeeklyPresentPopularityPointDate = dtDate;
		m_nWeeklyPresentPopularityPoint = 0;  
		m_dtWeeklyPresentContributionPointDate = dtDate;
		m_nWeeklyPresentContributionPoint = 0;
	}

    //---------------------------------------------------------------------------------------------------
    public void RemoveHeroReceivedPresentList(CsHeroReceivedPresent csHeroReceivedPresent)
    {
        if (csHeroReceivedPresent == null)
        {
            return;
        }

        m_listCsHeroReceivedPresent.Remove(csHeroReceivedPresent);
    }

    //---------------------------------------------------------------------------------------------------
    public void DeleteAllHeroReceivedPresentList()
    {
        m_listCsHeroReceivedPresent.Clear();
    }

    //---------------------------------------------------------------------------------------------------
    public bool CheckPresent()
    {
        if ((DateTime.Compare(m_dtWeeklyPresentPopularityPointDate, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) > 0 &&
            m_nNationWeeklyPresentPopularityPointRankingNo == m_nRewardedNationWeeklyPresentPopularityPointRankingNo) || 
            (DateTime.Compare(m_dtWeeklyPresentContributionPointDate, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) > 0 && 
            m_nNationWeeklyPresentContributionPointRankingNo == m_nRewardedNationWeeklyPresentContributionPointRankingNo))
        {
            return true;
        }

        return false;
    }

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 선물발송
	public void SendPresentSend(Guid guidTargetHeroId, int nPresentId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			PresentSendCommandBody cmdBody = new PresentSendCommandBody();
			m_guidSenderId = cmdBody.targetHeroid = guidTargetHeroId;
			m_nPresendId = cmdBody.presentId = nPresentId;
			CsRplzSession.Instance.Send(ClientCommandName.PresentSend, cmdBody);
		}
	}

	void OnEventResPresentSend(int nReturnCode, PresentSendResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtWeeklyPresentContributionPointDate = resBody.weekStartDate;
			m_nWeeklyPresentContributionPoint = resBody.weeklyPresentContributionPoint;

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			if (EventPresentSend != null)
			{
				EventPresentSend(m_nPresendId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상영웅이 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A147_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 대상영웅은 생성중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A147_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// Vip레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A147_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A147_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 선물답장
	public void SendPresentReply(Guid guidTargetHeroId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			PresentReplyCommandBody cmdBody = new PresentReplyCommandBody();
			m_guidSenderId = cmdBody.targetHeroid = guidTargetHeroId;
			CsRplzSession.Instance.Send(ClientCommandName.PresentReply, cmdBody);
		}
	}

	void OnEventResPresentReply(int nReturnCode, PresentReplyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{

			if (EventPresentReply != null)
			{
				EventPresentReply();
			}
		}
		else if (nReturnCode == 101)
		{
			// 대상영웅이 로그인중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A147_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서버선물인기점수랭킹
	public void SendServerPresentPopularityPointRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ServerPresentPopularityPointRankingCommandBody cmdBody = new ServerPresentPopularityPointRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ServerPresentPopularityPointRanking, cmdBody);
		}
	}

	void OnEventResServerPresentPopularityPointRanking(int nReturnCode, ServerPresentPopularityPointRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            CsPresentPopularityPointRanking myRanking = null;

            if (resBody.myRanking != null)
            {
                myRanking = new CsPresentPopularityPointRanking(resBody.myRanking);
            } 

			List<CsPresentPopularityPointRanking> list = new List<CsPresentPopularityPointRanking>();

			for (int i = 0; i < resBody.rankings.Length; i++)
			{
				list.Add(new CsPresentPopularityPointRanking(resBody.rankings[i]));
			}

			if (EventServerPresentPopularityPointRanking != null)
			{
				EventServerPresentPopularityPointRanking(myRanking, list);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물인기점수랭킹
	public void SendNationWeeklyPresentPopularityPointRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationWeeklyPresentPopularityPointRankingCommandBody cmdBody = new NationWeeklyPresentPopularityPointRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.NationWeeklyPresentPopularityPointRanking, cmdBody);
		}
	}

	void OnEventResNationWeeklyPresentPopularityPointRanking(int nReturnCode, NationWeeklyPresentPopularityPointRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            CsPresentPopularityPointRanking myRanking = null;

            if (resBody.myRanking != null)
            {
                myRanking = new CsPresentPopularityPointRanking(resBody.myRanking);
            }

			List<CsPresentPopularityPointRanking> list = new List<CsPresentPopularityPointRanking>();

			for (int i = 0; i < resBody.rankings.Length; i++)
			{
				list.Add(new CsPresentPopularityPointRanking(resBody.rankings[i]));
			}

			if (EventNationWeeklyPresentPopularityPointRanking != null)
			{
				EventNationWeeklyPresentPopularityPointRanking(myRanking, list);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물인기점수랭킹보상받기
	public void SendNationWeeklyPresentPopularityPointRankingRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationWeeklyPresentPopularityPointRankingRewardReceiveCommandBody cmdBody = new NationWeeklyPresentPopularityPointRankingRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.NationWeeklyPresentPopularityPointRankingRewardReceive, cmdBody);
		}
	}

	void OnEventResNationWeeklyPresentPopularityPointRankingRewardReceive(int nReturnCode, NationWeeklyPresentPopularityPointRankingRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nRewardedNationWeeklyPresentPopularityPointRankingNo = resBody.rewardedRankingNo;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventNationWeeklyPresentPopularityPointRankingRewardReceive != null)
			{
				EventNationWeeklyPresentPopularityPointRankingRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A148_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 랭킹에 진입하지 못했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A148_ERROR_00302"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서버선물공헌점수랭킹
	public void SendServerPresentContributionPointRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ServerPresentContributionPointRankingCommandBody cmdBody = new ServerPresentContributionPointRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ServerPresentContributionPointRanking, cmdBody);
		}
	}

	void OnEventResServerPresentContributionPointRanking(int nReturnCode, ServerPresentContributionPointRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            CsPresentContributionPointRanking myRanking = null;

            if (resBody.myRanking != null)
            {
                myRanking = new CsPresentContributionPointRanking(resBody.myRanking);
            }

			List<CsPresentContributionPointRanking> list = new List<CsPresentContributionPointRanking>();

            for (int i = 0; i < resBody.rankings.Length; i++)
            {
                list.Add(new CsPresentContributionPointRanking(resBody.rankings[i]));
            }

			if (EventServerPresentContributionPointRanking != null)
			{
				EventServerPresentContributionPointRanking(myRanking, list);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물공헌점수랭킹
	public void SendNationWeeklyPresentContributionPointRanking()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationWeeklyPresentContributionPointRankingCommandBody cmdBody = new NationWeeklyPresentContributionPointRankingCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.NationWeeklyPresentContributionPointRanking, cmdBody);
		}
	}

	void OnEventResNationWeeklyPresentContributionPointRanking(int nReturnCode, NationWeeklyPresentContributionPointRankingResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            CsPresentContributionPointRanking myRanking = null;

            if (resBody.myRanking != null)
            {
                myRanking = new CsPresentContributionPointRanking(resBody.myRanking);
            }

			List<CsPresentContributionPointRanking> list = new List<CsPresentContributionPointRanking>();

            for (int i = 0; i < resBody.rankings.Length; i++)
            {
                list.Add(new CsPresentContributionPointRanking(resBody.rankings[i]));
            }

			if (EventNationWeeklyPresentContributionPointRanking != null)
			{
				EventNationWeeklyPresentContributionPointRanking(myRanking, list);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물공헌점수랭킹보상받기
	public void SendNationWeeklyPresentContributionPointRankingRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			NationWeeklyPresentContributionPointRankingRewardReceiveCommandBody cmdBody = new NationWeeklyPresentContributionPointRankingRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.NationWeeklyPresentContributionPointRankingRewardReceive, cmdBody);
		}
	}

	void OnEventResNationWeeklyPresentContributionPointRankingRewardReceive(int nReturnCode, NationWeeklyPresentContributionPointRankingRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nRewardedNationWeeklyPresentContributionPointRankingNo = resBody.rewardedRankingNo;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventNationWeeklyPresentContributionPointRankingRewardReceive != null)
			{
				EventNationWeeklyPresentContributionPointRankingRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A148_ERROR_00601"));
		}
		else if (nReturnCode == 102)
		{
			// 랭킹에 진입하지 못했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A148_ERROR_00602"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 선물수신
	void OnEventEvtPresentReceived(SEBPresentReceivedEventBody eventBody)
	{
		CsHeroReceivedPresent csHeroReceivedPresent = new CsHeroReceivedPresent(eventBody.senderId, eventBody.senderName, eventBody.senderNationId, eventBody.presentId);
		m_listCsHeroReceivedPresent.Add(csHeroReceivedPresent);

		m_dtWeeklyPresentPopularityPointDate = eventBody.weekStartDate;
		m_nWeeklyPresentPopularityPoint = eventBody.weeklyPresentPopularityPoint;

		if (EventPresentReceived != null)
		{
			EventPresentReceived();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 선물답장수신
	void OnEventEvtPresentReplyReceived(SEBPresentReplyReceivedEventBody eventBody)
	{
		if (EventPresentReplyReceived != null)
		{
			EventPresentReplyReceived(eventBody.senderId, eventBody.senderName, eventBody.senderNationId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅선물
	void OnEventEvtHeroPresent(SEBHeroPresentEventBody eventBody)
	{
		if (EventHeroPresent != null)
		{
			CsPresent csPresent = CsGameData.Instance.GetPresent(eventBody.presentId);
			EventHeroPresent(eventBody.senderId, eventBody.senderName, eventBody.senderNationId, eventBody.targetId, eventBody.targetName, eventBody.targetNationId, csPresent);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물인기점수랭킹갱신
	void OnEventEvtNationWeeklyPresentPopularityPointRankingUpdated(SEBNationWeeklyPresentPopularityPointRankingUpdatedEventBody eventBody)
	{
		m_nNationWeeklyPresentPopularityPointRankingNo = eventBody.rankingNo;
		m_nNationWeeklyPresentPopularityPointRanking = eventBody.ranking;  

		if (EventNationWeeklyPresentPopularityPointRankingUpdated != null)
		{
			EventNationWeeklyPresentPopularityPointRankingUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 국가주간선물공헌점수랭킹갱신
	void OnEventEvtNationWeeklyPresentContributionPointRankingUpdated(SEBNationWeeklyPresentContributionPointRankingUpdatedEventBody eventBody)
	{
		m_nNationWeeklyPresentContributionPointRankingNo = eventBody.rankingNo;
		m_nNationWeeklyPresentContributionPointRanking = eventBody.ranking;

		if (EventNationWeeklyPresentContributionPointRankingUpdated != null)
		{
			EventNationWeeklyPresentContributionPointRankingUpdated();
		}
	}

	#endregion Protocol.Event
}
