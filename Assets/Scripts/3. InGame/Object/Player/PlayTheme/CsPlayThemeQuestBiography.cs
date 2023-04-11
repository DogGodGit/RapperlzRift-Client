using SimpleDebugLog;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestBiography : CsPlayThemeQuest
{
	bool m_bArrival = false;

	int m_nAutoHeroBiograhyid = 0;
	CsBiographyManager m_csBiographyManager;
	Coroutine m_corReStartCoroutine;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			BiographyQuestPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csBiographyManager = CsBiographyManager.Instance;
		m_csBiographyManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csBiographyManager.EventStopAutoPlay += OnEventStopAutoPlay;

		m_csBiographyManager.EventBiographyQuestAccept += OnEventBiographyQuestAccept;
		m_csBiographyManager.EventBiographyQuestComplete += OnEventBiographyQuestComplete;
		m_csBiographyManager.EventBiographyComplete += OnEventBiographyComplete;

		m_csBiographyManager.EventBiographyQuestMoveObjectiveComplete += OnEventBiographyQuestMoveObjectiveComplete;
		m_csBiographyManager.EventBiographyQuestNpcConversationComplete += OnEventBiographyQuestNpcConversationComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		ReStartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csBiographyManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csBiographyManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csBiographyManager.EventBiographyQuestAccept -= OnEventBiographyQuestAccept;
		m_csBiographyManager.EventBiographyQuestComplete -= OnEventBiographyQuestComplete;
		m_csBiographyManager.EventBiographyComplete -= OnEventBiographyComplete;

		m_csBiographyManager.EventBiographyQuestMoveObjectiveComplete -= OnEventBiographyQuestMoveObjectiveComplete;
		m_csBiographyManager.EventBiographyQuestNpcConversationComplete -= OnEventBiographyQuestNpcConversationComplete;
		m_csBiographyManager = null;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
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
				m_csBiographyManager.StopAutoPlay(this);
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
					BiographyQuestPlay();
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

		m_nAutoHeroBiograhyid = m_csBiographyManager.AutoBiographyId;
		BiographyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, int nBiographyId)
	{
		if (objCaller == this) return;
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestAccept(int nBiographyId)
	{
		if (IsThisAutoPlaying())
		{
			m_nAutoHeroBiograhyid = nBiographyId;
			BiographyQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestComplete(bool bLevelUp, long lAcquiredExp, int nBiographyId)
	{
		if (IsThisAutoPlaying())
		{
			m_nAutoHeroBiograhyid = nBiographyId;
			BiographyQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyComplete(int nBiographyId)
	{
		if (IsThisAutoPlaying())
		{
			if (m_nAutoHeroBiograhyid == nBiographyId)
			{
				Player.SetAutoPlay(null, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestMoveObjectiveComplete(int nBiographyId)
	{
		if (IsThisAutoPlaying())
		{
			BiographyQuestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestNpcConversationComplete(int nBiographyId)
	{
		if (IsThisAutoPlaying())
		{
			BiographyQuestPlay();
		}
	}

	#endregion Event.DailyQuestManager

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (Player.Target.CompareTag("Npc"))
				{
					CsNpcInfo csNpcInfo = CsIngameData.Instance.IngameManagement.GetNpcInfo(Player.Target);
					if (csNpcInfo != null)
					{
						if (m_csBiographyManager.NpcDialogReadyOK(csNpcInfo.NpcId))
						{
							Debug.Log("CsPlayThemeQuestBiography.OnEventArrivalMoveByTouch()     m_csDailyQuestManager.Auto = " + m_csBiographyManager.Auto);
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
		if (m_csBiographyManager.Auto)
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
			m_csBiographyManager.StopAutoPlay(this);
			return;
		}

		if (IsThisAutoPlaying()) return;

		m_nAutoHeroBiograhyid = m_csBiographyManager.AutoBiographyId;
		Player.SetAutoPlay(this, true);
		BiographyQuestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath(int nContinent, Vector3 vtPos, float flRadius, EnWayPointType enWayPointType)
	{
		if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
		{
			vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinent);
			Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
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

		Player.DisplayPath.SetPath(vtPos);
	}

	//---------------------------------------------------------------------------------------------------
	void BiographyQuestPlay()
	{
		CsHeroBiography csHeroBiography = m_csBiographyManager.GetHeroBiography(m_nAutoHeroBiograhyid);
		if (csHeroBiography != null)
		{
			CsHeroBiographyQuest csHeroBiographyQuest = csHeroBiography.HeroBiograhyQuest;

			if (csHeroBiographyQuest.BiographyQuest != null)
			{
				if (csHeroBiographyQuest.Excuted)    // 미션완료
				{
					CsNpcInfo csNpcInfo = csHeroBiographyQuest.BiographyQuest.CompletionNpc;
					SetDisplayPath(csNpcInfo.ContinentId, csNpcInfo.Position , csNpcInfo.InteractionMaxRange, EnWayPointType.Npc);
				}
				else                                // 미션중
				{
					switch (csHeroBiographyQuest.BiographyQuest.Type)
					{
						case 1: // 이동		
							SetDisplayPath(csHeroBiographyQuest.BiographyQuest.TargetContinent.ContinentId,
										   csHeroBiographyQuest.BiographyQuest.TargetPosition,
										   csHeroBiographyQuest.BiographyQuest.TargetRadius,
										   EnWayPointType.Move);
							break;
						case 2: // 몬스터처치
							SetDisplayPath(csHeroBiographyQuest.BiographyQuest.TargetContinent.ContinentId,
										   csHeroBiographyQuest.BiographyQuest.TargetPosition,
										   csHeroBiographyQuest.BiographyQuest.TargetRadius,
										   EnWayPointType.Battle);
							break;
						case 3: // 상호작용
							SetDisplayPath(csHeroBiographyQuest.BiographyQuest.TargetContinent.ContinentId,
										   csHeroBiographyQuest.BiographyQuest.TargetPosition,
										   csHeroBiographyQuest.BiographyQuest.TargetRadius,
										   EnWayPointType.Interaction);
							break;
						case 4: // NPC대화
							SetDisplayPath(csHeroBiographyQuest.BiographyQuest.TargetNpc.ContinentId, 
										   csHeroBiographyQuest.BiographyQuest.TargetNpc.Position,
										   csHeroBiographyQuest.BiographyQuest.TargetNpc.
										   InteractionMaxRange, 
										   EnWayPointType.Npc);
							break;
						case 5: // 전기퀘스트던전클리어
							SetDisplayPath(csHeroBiographyQuest.BiographyQuest.TargetContinent.ContinentId,
										   csHeroBiographyQuest.BiographyQuest.TargetPosition,
										   csHeroBiographyQuest.BiographyQuest.TargetRadius,
										   EnWayPointType.None);
							break;
					}
				}
			}
		}
	}
}
