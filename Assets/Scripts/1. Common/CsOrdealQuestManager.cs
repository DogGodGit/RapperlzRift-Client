using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

enum EnOrdealQuestMissionType
{
	SoulStoneLevel		= 1,	// 장착 소울 스톤 총 레벨
	BuyStamina			= 2,	// 스태미나 구입
	MountLevel			= 3,	// 탈것 레벨
	CreatureLevel		= 4,	// 크리쳐 레벨
	WingLevel			= 5,	// 날개 레벨
	BountyHunter		= 6,	// 현상금 사냥
	Monster				= 7,	// 몬스터
	LegendBountyHunter	= 8,	// 전설등급 현상금 사냥
	LegendFishingBait	= 9,	// 전설등급 낚시 미끼
	LegendSupplySupport	= 10,	// 전설등급 보급지령서
	RuinsReclaim		= 11,	// 유적탈환
	WarMemory			= 12,	// 전쟁의 기억
	DemensionRaid		= 13,	// 차원습격
	HolyWar				= 14,	// 위대한 성전
	NationWar			= 15,	// 차원전쟁
	LegendSecretLetter	= 16,	// 전설등급 밀서유출
	ExploitPoint		= 17,	// 공적포인트
	Rank				= 18,	// 계급
	AchievePoint		= 19,	// 업적포인트
}

public class CsOrdealQuestManager
{
	bool m_bWaitResponse = false;

	CsHeroOrdealQuest m_csHeroOrdealQuest;

	int m_nIndex;

	//---------------------------------------------------------------------------------------------------
	public static CsOrdealQuestManager Instance
	{
		get { return CsSingleton<CsOrdealQuestManager>.GetInstance(); }
	}

	public CsHeroOrdealQuest CsHeroOrdealQuest
	{
		get { return m_csHeroOrdealQuest; }
	}

	public bool RewardReceivable
	{
		get
		{
			if (m_csHeroOrdealQuest == null)
				return false;

			if (m_csHeroOrdealQuest.Completed)
				return false;

			if (m_csHeroOrdealQuest.HeroOrdealQuestSlotList == null)
				return false;

			foreach (var csHeroOrdealQuestSlot in m_csHeroOrdealQuest.HeroOrdealQuestSlotList)
			{
				if (!csHeroOrdealQuestSlot.IsCompleted)
				{
					return false;
				}
			}

			return true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<bool, long, int> EventOrdealQuestSlotComplete;
	public event Delegate<bool, long> EventOrdealQuestComplete;
	public event Delegate EventOrdealQuestAccepted;
	public event Delegate<int> EventOrdealQuestSlotProgressCountsUpdated;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroOrdealQuest ordealQuest)
	{
		UnInit();

		if (ordealQuest != null)
		{
			m_csHeroOrdealQuest = new CsHeroOrdealQuest(ordealQuest);
		}

		// Command
		CsRplzSession.Instance.EventResOrdealQuestSlotComplete += OnEventResOrdealQuestSlotComplete;
		CsRplzSession.Instance.EventResOrdealQuestComplete += OnEventResOrdealQuestComplete;


		// Event
		CsRplzSession.Instance.EventEvtOrdealQuestAccepted += OnEventEvtOrdealQuestAccepted;
		CsRplzSession.Instance.EventEvtOrdealQuestSlotProgressCountsUpdated += OnEventEvtOrdealQuestSlotProgressCountsUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResOrdealQuestSlotComplete -= OnEventResOrdealQuestSlotComplete;
		CsRplzSession.Instance.EventResOrdealQuestComplete -= OnEventResOrdealQuestComplete;

		// Event
		CsRplzSession.Instance.EventEvtOrdealQuestAccepted -= OnEventEvtOrdealQuestAccepted;
		CsRplzSession.Instance.EventEvtOrdealQuestSlotProgressCountsUpdated -= OnEventEvtOrdealQuestSlotProgressCountsUpdated;

		m_bWaitResponse = false;
		m_csHeroOrdealQuest = null;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 시련퀘스트슬롯완료
	public void SendOrdealQuestSlotComplete(int nIndex)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			OrdealQuestSlotCompleteCommandBody cmdBody = new OrdealQuestSlotCompleteCommandBody();
			cmdBody.index = m_nIndex = nIndex;
			CsRplzSession.Instance.Send(ClientCommandName.OrdealQuestSlotComplete, cmdBody);
		}
	}

	void OnEventResOrdealQuestSlotComplete(int nReturnCode, OrdealQuestSlotCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroOrdealQuestSlot csHeroOrdealQuestSlot = m_csHeroOrdealQuest.GetHeroOrdealQuestSlot(m_nIndex);
			CsOrdealQuestMission csOrdealQuestMission = m_csHeroOrdealQuest.OrdealQuest.GetOrdealQuestMission(m_nIndex, resBody.nextMissionNo);

			csHeroOrdealQuestSlot.OrdealQuestMission = csOrdealQuestMission;
			csHeroOrdealQuestSlot.ProgressCount = 0;

			if (csOrdealQuestMission != null)
			{
				csHeroOrdealQuestSlot.RemainingTime = csOrdealQuestMission.AutoCompletionRequiredTime;
			}
			else
			{
				csHeroOrdealQuestSlot.RemainingTime = 0;
			}


			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventOrdealQuestSlotComplete != null)
			{
				EventOrdealQuestSlotComplete(bLevelUp, resBody.acquiredExp, m_nIndex);
			}
		}
		else if (nReturnCode == 101)
		{
			// 모든 미션을 완료했습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A120_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 미션 목표를 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A120_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


	//---------------------------------------------------------------------------------------------------
	// 시련퀘스트완료
	public void SendOrdealQuestComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			OrdealQuestCompleteCommandBody cmdBody = new OrdealQuestCompleteCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.OrdealQuestComplete, cmdBody);
		}
	}

	void OnEventResOrdealQuestComplete(int nReturnCode, OrdealQuestCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (resBody.nextQuest == null)
			{ 
				m_csHeroOrdealQuest.Completed = true;
			}
			else
			{
				m_csHeroOrdealQuest = new CsHeroOrdealQuest(resBody.nextQuest);
			}

			int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;

			CsGameData.Instance.MyHeroInfo.Level = resBody.level;
			CsGameData.Instance.MyHeroInfo.Exp = resBody.exp;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

			if (EventOrdealQuestComplete != null)
			{
				EventOrdealQuestComplete(bLevelUp, resBody.acquiredExp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 완료한 퀘스트입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A120_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 퀘스트목표를 완료하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A120_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 시련퀘스트수락
	void OnEventEvtOrdealQuestAccepted(SEBOrdealQuestAcceptedEventBody eventBody)
	{
		m_csHeroOrdealQuest = new CsHeroOrdealQuest(eventBody.quest);

		if (EventOrdealQuestAccepted != null)
		{
			EventOrdealQuestAccepted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 시련퀘스트진행카운트갱신
	void OnEventEvtOrdealQuestSlotProgressCountsUpdated(SEBOrdealQuestSlotProgressCountsUpdatedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.progressCounts.Length; i++)
		{ 
			CsHeroOrdealQuestSlot csHeroOrdealQuestSlot = m_csHeroOrdealQuest.GetHeroOrdealQuestSlot(eventBody.progressCounts[i].index);
			csHeroOrdealQuestSlot.ProgressCount = eventBody.progressCounts[i].progressCount;

			if (EventOrdealQuestSlotProgressCountsUpdated != null)
			{
				EventOrdealQuestSlotProgressCountsUpdated(eventBody.progressCounts[i].index);
			}
		}
	}

	#endregion Protocol.Event
}
