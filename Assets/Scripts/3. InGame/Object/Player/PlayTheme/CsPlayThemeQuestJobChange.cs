using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsPlayThemeQuestJobChange : CsPlayThemeQuest
{
	bool m_bArrival = false;

	CsJobChangeManager m_csJobChangeManager;
	Coroutine m_corReStartCoroutine;
	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			JobChangeQuestPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csJobChangeManager = CsJobChangeManager.Instance;
		m_csJobChangeManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csJobChangeManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csJobChangeManager.EventJobChangeQuestAccept += OnEventJobChangeQuestAccept;
		m_csJobChangeManager.EventJobChangeQuestProgressCountUpdated += OnEventJobChangeQuestProgressCountUpdated;
		m_csJobChangeManager.EventJobChangeQuestComplete += OnEventJobChangeQuestComplete;
		m_csJobChangeManager.EventJobChangeQuestFailed += OnEventJobChangeQuestFailed;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csJobChangeManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csJobChangeManager.EventStopAutoPlay -= OnEventStopAutoPlay;

		m_csJobChangeManager.EventJobChangeQuestAccept -= OnEventJobChangeQuestAccept;
		m_csJobChangeManager.EventJobChangeQuestProgressCountUpdated -= OnEventJobChangeQuestProgressCountUpdated;
		m_csJobChangeManager.EventJobChangeQuestComplete -= OnEventJobChangeQuestComplete;
		m_csJobChangeManager.EventJobChangeQuestFailed -= OnEventJobChangeQuestFailed;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		m_csJobChangeManager = null;
		m_corReStartCoroutine = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		//m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Player.IsQuestDialog = false;

			if (bNotify)
			{
				m_csJobChangeManager.StopAutoPlay(this);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Player.StartCoroutine(CorutineStateChangedToIdle());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (m_bArrival == false)
		{
			if (Player.IsStateIdle)
			{
				if (IsThisAutoPlaying())
				{
					JobChangeQuestPlay();
				}
			}
		}

		m_bArrival = false;
	}

	#endregion Override


	#region Event.DailyQuestManager
	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (IsThisAutoPlaying() == false)
		{
			Player.SetAutoPlay(this, true);
		}

		JobChangeQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (objCaller == this) return;
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestAccept()
	{
		if (IsThisAutoPlaying())
		{
			JobChangeQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestProgressCountUpdated()
	{
		if (IsThisAutoPlaying())
		{
			JobChangeQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestComplete()
	{
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventJobChangeQuestFailed()
	{
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.DailyQuestManager

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget && Player.Target != null)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (Player.Target.CompareTag("Npc"))
				{
					CsNpcInfo csNpcInfo = CsIngameData.Instance.IngameManagement.GetNpcInfo(Player.Target);
					if (csNpcInfo != null)
					{
						if (m_csJobChangeManager.NpcDialogReadyOK(csNpcInfo.NpcId))
						{							
							NpcDialog(csNpcInfo);
						}
					}
				}
			}
		}
	}

	#endregion Event.Player

	//---------------------------------------------------------------------------------------------------
	void ReStartAutoPlay()
	{
		if (m_csJobChangeManager.Auto)
		{
			m_corReStartCoroutine = Player.StartCoroutine(DelayReStart());
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayReStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		BiographyQuestReStart();
	}

	//---------------------------------------------------------------------------------------------------
	void BiographyQuestReStart()
	{
		m_corReStartCoroutine = null;

		if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
		{
			m_csJobChangeManager.StopAutoPlay(this);
			return;
		}

		if (IsThisAutoPlaying()) return;

		Player.SetAutoPlay(this, true);
		JobChangeQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath(int nContinent, Vector3 vtPos, float flRadius, EnWayPointType enWayPointType, bool bTargetOwnNation, bool GuildTerritory = false)
	{
		if (GuildTerritory)
		{
			Player.SetWayPoint(vtPos, enWayPointType, flRadius);
		}
		else
		{
			if (bTargetOwnNation) // 본인국가에서 퀘스트 진행.
			{
				if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
				{
					vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinent);
					Player.SetWayPoint(vtPos, EnWayPointType.Npc, 4f);
				}
				else
				{
					if (nContinent == m_csMyHeroInfo.LocationId)
					{
						Player.SetWayPoint(vtPos, enWayPointType, flRadius);
					}
					else
					{
						vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, nContinent);
						Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
					}
				}
			}
			else                  // 적국가에서 퀘스트 진행.
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인국가일때. >> 국가이동 Npc에게 이동.
				{
					int nNationId = m_csMyHeroInfo.Nation.NationId > 2 ? 1 : m_csMyHeroInfo.Nation.NationId + 1;	// 다른국가 ID 설정.

					vtPos = Player.ChaseContinent(nNationId, nContinent);
					Player.SetWayPoint(vtPos, EnWayPointType.Npc, 4f);
				}
				else
				{
					if (nContinent == m_csMyHeroInfo.LocationId)
					{
						Player.SetWayPoint(vtPos, enWayPointType, flRadius);
					}
					else
					{
						vtPos = Player.ChaseContinent(m_csMyHeroInfo.InitEntranceLocationParam, nContinent);
						Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
					}
				}
			}
		}
		

		Player.DisplayPath.SetPath(vtPos);
	}

	//---------------------------------------------------------------------------------------------------
	void JobChangeQuestPlay()
	{
		CsJobChangeQuest JobChangeQuest;
		Debug.Log("JobChangeQuestPlay : "+ m_csJobChangeManager.JobChangeState);
		
		switch (m_csJobChangeManager.JobChangeState)
		{
			case EnJobChangeQuestStaus.None:
				JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(1);
				SetDisplayPath(JobChangeQuest.QuestNpc.ContinentId, JobChangeQuest.QuestNpc.Position, JobChangeQuest.QuestNpc.InteractionMaxRange, EnWayPointType.Npc, true);
				break;
			case EnJobChangeQuestStaus.Failed:
				JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(m_csJobChangeManager.HeroJobChangeQuest.QuestNo);
				if (JobChangeQuest != null)
				{
					SetDisplayPath(JobChangeQuest.QuestNpc.ContinentId, JobChangeQuest.QuestNpc.Position, JobChangeQuest.QuestNpc.InteractionMaxRange, EnWayPointType.Npc, true);
				}
				break;
			case EnJobChangeQuestStaus.Completed:
				JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(m_csJobChangeManager.HeroJobChangeQuest.QuestNo + 1);
				if (JobChangeQuest != null)
				{
					SetDisplayPath(JobChangeQuest.QuestNpc.ContinentId, JobChangeQuest.QuestNpc.Position, JobChangeQuest.QuestNpc.InteractionMaxRange, EnWayPointType.Npc, true);
				}
				break;
			case EnJobChangeQuestStaus.Accepted:
				JobChangeQuest = CsGameData.Instance.GetJobChangeQuest(m_csJobChangeManager.HeroJobChangeQuest.QuestNo);
				if (JobChangeQuest != null)
				{
					Debug.Log("JobChangeQuest : "+ m_csJobChangeManager.HeroJobChangeQuest.Difficulty + " , " + JobChangeQuest.Type + " , " + JobChangeQuest.TargetCount + " , " + m_csJobChangeManager.HeroJobChangeQuest.ProgressCount);

					if (m_csJobChangeManager.HeroJobChangeQuest.ProgressCount < JobChangeQuest.TargetCount)
					{
						if (m_csJobChangeManager.HeroJobChangeQuest.Difficulty == 1)    // 쉬움 (길드영지)
						{
							if (CsGameData.Instance.MyHeroInfo.LocationId == CsGuildManager.Instance.GuildTerritory.LocationId)
							{
								switch (JobChangeQuest.Type)	// 쉬움은 전용 몬스터 처치만 있음.
								{
									case 3: // 전용 몬스터 처치
										SetDisplayPath(CsGameData.Instance.MyHeroInfo.LocationId, JobChangeQuest.TargetGuildTerritoryPosition, JobChangeQuest.TargetGuildTerritoryRadius, EnWayPointType.Battle, JobChangeQuest.IsTargetOwnNation, true);
										break;
								}
							}
							else
							{
								// 길드 영지로 이동.
							}
						}
						else                                                            // 어려움(공용필드)
						{
							switch (JobChangeQuest.Type)
							{
								case 1: // 몬스터 처치
									SetDisplayPath(JobChangeQuest.TargetContinent.ContinentId, JobChangeQuest.TargetPosition, JobChangeQuest.TargetRadius, EnWayPointType.Battle, JobChangeQuest.IsTargetOwnNation);
									break;
								case 2: // 상호작용
									SetDisplayPath(JobChangeQuest.TargetContinent.ContinentId, JobChangeQuest.TargetPosition, JobChangeQuest.TargetContinentObject.InteractionMaxRange, EnWayPointType.Interaction, JobChangeQuest.IsTargetOwnNation);
									break;
								case 3: // 전용 몬스터 처치
									SetDisplayPath(JobChangeQuest.TargetContinent.ContinentId, JobChangeQuest.TargetPosition, JobChangeQuest.TargetRadius, EnWayPointType.Battle, JobChangeQuest.IsTargetOwnNation);
									break;
							}
						}
					}
					else    // 목표미션 완료 완료받으로 가야함.
					{
						if (CsIngameData.Instance.IngameManagement.IsContinent())
						{
							SetDisplayPath(JobChangeQuest.QuestNpc.ContinentId, JobChangeQuest.QuestNpc.Position, JobChangeQuest.QuestNpc.InteractionMaxRange, EnWayPointType.Npc, true);
						}
						else
						{
							// 대륙으로 이동.
						}
					}
				}
				break;
		}		
	}
}
