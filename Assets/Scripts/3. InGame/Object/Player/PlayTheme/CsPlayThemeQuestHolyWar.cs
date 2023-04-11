using System.Collections;
using UnityEngine;


public class CsPlayThemeQuestHolyWar : CsPlayThemeQuest
{
	CsHolyWarQuestManager m_csHolyWarQuestManager;
	CsNpcInfo m_csNpcInfo;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			CheckNpcDialog();
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csHolyWarQuestManager = CsHolyWarQuestManager.Instance;
		m_csHolyWarQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csHolyWarQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csHolyWarQuestManager.EventUpdateState += OnEventUpdateState;
		m_csHolyWarQuestManager.EventHolyWarQuestComplete += OnEventHolyWarQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		RestartAutoPlay();
		
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csHolyWarQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csHolyWarQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csHolyWarQuestManager.EventUpdateState -= OnEventUpdateState;
		m_csHolyWarQuestManager.EventHolyWarQuestComplete -= OnEventHolyWarQuestComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

		m_csHolyWarQuestManager = null;
		m_csNpcInfo = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		m_timer.Init(0.1f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		// 1. 수락 전에는 수락 Npc 이동.
		// 2. 수락 후 >>  (적국인경우 AutoBattle, 본국인경우 국가이동 Npc 이동)
		// 3. 타임아웃 후 완료Npc로이동.					if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)

		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csHolyWarQuestManager.HolyWarQuestState == EnHolyWarQuestState.Accepted)          // 1. 미션중.
			{
				if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam)     // 2. 적국일때.
				{

					BattlePlayer(Player.transform.position, 50f);                                   // 3. 자동 전투.
					return;
				}
			}

			if (m_csNpcInfo == null)
			{
				m_csNpcInfo = GetNpcInfo();
				return;
			}

			if (MovePlayer(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId, m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange, true))
			{
				CheckNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			if (bNotify)
			{
				m_csHolyWarQuestManager.StopAutoPlay(this);
			}
		}
	}

	#endregion Override

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			if (Player.Target != null && Player.Target.CompareTag("Npc"))
			{
				CheckNpcDialog();
			}
		}
	}

	#endregion Event.Player

	#region Event.HolyWarQuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csHolyWarQuestManager.StopAutoPlay(this);
			return;
		}
		
		SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (IsThisAutoPlaying())
		{
			Player.ChangeState(CsHero.EnState.Idle);
			SetDisplayPath();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHolyWarQuestComplete(bool bLevelUp, long lAcquiredExp, int nAcquiredExploitPoint)
	{
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.HolyWarQuestManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;

		if (m_csHolyWarQuestManager.HolyWarQuestState == EnHolyWarQuestState.Accepted)			// 1. 미션중.
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam)     // 2. 적국일때.
			{
				Player.DisplayPath.SetPath(Player.transform.position);
				Player.SetWayPoint(Player.transform.position, EnWayPointType.Battle, 30);

				return;
			}
		}

		m_csNpcInfo = GetNpcInfo();

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
		{
			Player.DisplayPath.SetPath(m_csNpcInfo.Position);
			Player.SetWayPoint(m_csNpcInfo.Position, EnWayPointType.Npc, m_csNpcInfo.InteractionMaxRange);
		}
		else
		{			
			Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId);
			Player.DisplayPath.SetPath(vtPos);
			Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto && IsThisAutoPlaying())
		{
			m_csNpcInfo = GetNpcInfo();
			if (m_csNpcInfo.NpcType == EnNpcType.NationTransmission && Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange)) // 국가이동 Npc앞 도착.
			{
				NpcDialog(m_csNpcInfo);
				m_csHolyWarQuestManager.NationTransmissionReadyOK();
				return;
			}
		}

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			if (m_csHolyWarQuestManager.HolyWarQuestState != EnHolyWarQuestState.Accepted) // 퀘스트 수락 or 완료 
			{
				m_csNpcInfo = m_csHolyWarQuestManager.HolyWarQuest.QuestNpcInfo;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (m_csHolyWarQuestManager.CheckAvailability())
					{
						Debug.Log("CsPlayThemeQuestHolyWar.CheckNpcDialog()   >>>   NpctDialog");
						NpcDialog(m_csNpcInfo);
						m_csHolyWarQuestManager.NpctDialog();
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo()
	{
		if (m_csHolyWarQuestManager.HolyWarQuestState == EnHolyWarQuestState.Accepted)  // <미션중>. (적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
		}
		else  // <수락전 or 완료>.  (퀘스트 목적 국가로 이동)
		{
			if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) // 적국일때 >> 국가이동 Npc에게 이동.
			{
				return CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
			}
			else
			{
				return m_csHolyWarQuestManager.HolyWarQuest.QuestNpcInfo;
			}
		}
		return null; // 이동할 Npc 없음 >> AutoBattle 진행.
	}

	//---------------------------------------------------------------------------------------------------
	void RestartAutoPlay()
	{
		Player.StartCoroutine(DelayStart());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csHolyWarQuestManager.Auto)
		{
			if (Player.Dead || m_csHolyWarQuestManager.HolyWarQuest == null || !CsIngameData.Instance.IngameManagement.IsContinent())
			{
				m_csHolyWarQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestHolyWar.RestartAutoPlay()");
				SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
