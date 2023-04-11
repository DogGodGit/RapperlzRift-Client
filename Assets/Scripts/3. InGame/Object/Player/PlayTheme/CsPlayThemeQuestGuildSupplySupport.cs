using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildSupplySupport : CsPlayThemeQuest
{
	CsGuildManager m_csGuildManager;
	CsNpcInfo m_csNpcInfo;
	bool m_bArrival = false;

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("CsPlayThemeQuestGuildSupplySupport.ArrivalMoveToPos()");
			GuildSupplySupportPlay();
			m_bArrival = true;
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csGuildManager = CsGuildManager.Instance;
		m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csGuildManager.EventUpdateSupplySupportState += OnEventUpdateSupplySupportState;
		m_csGuildManager.EventGuildSupplySupportQuestComplete += OnEventGuildSupplySupportQuestComplete;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventChangeCartRiding += OnEventChangeCartRiding;

		ReStartAutoPlay();
		//Debug.Log("CsPlayThemeQuestGuildSupplySupport.Init() m_csGuildManager = " + m_csGuildManager);
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csGuildManager.EventUpdateSupplySupportState -= OnEventUpdateSupplySupportState;
		m_csGuildManager.EventGuildSupplySupportQuestComplete -= OnEventGuildSupplySupportQuestComplete;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventChangeCartRiding -= OnEventChangeCartRiding;
		m_csGuildManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
			Debug.Log("CsPlayThemeQuestGuildSupplySupport.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.SupplySupport);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (IsThisAutoPlaying())
			{
				Player.StartCoroutine(CorutineStateChangedToIdle());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (Player.IsStateIdle)
		{
			if (IsThisAutoPlaying())
			{
				if (m_bArrival == false)
				{
					Debug.Log("CsPlayThemeQuestGuildSupplySupport.CorutineStateChangedToIdle");
					GuildSupplySupportPlay();
				}
			}
		}
		m_bArrival = false;
	}

	#endregion Override

	#region Event.Player
	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			if (m_csPlayer.Target.CompareTag("Cart"))
			{
				if (m_csGuildManager.GuildSupplySupportQuestPlay != null)
				{
					if (Player.GetDistanceFormTarget(m_csGuildManager.GuildSupplySupportQuestPlay.CartPosition) <= 1.5f)
					{
						if (Player.BattleMode == false)
						{
							CsCartManager.Instance.SendCartGetOn(m_csGuildManager.GuildSupplySupportQuestPlay.CartInstanceId);
						}
					}
				}
			}
			else if (m_csPlayer.Target.CompareTag("Npc"))
			{
				CheckNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventChangeCartRiding()
	{
		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
		}
	}

	#endregion Event.Player

	#region Event.GuildManager
	
	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.SupplySupport)
		{
			if (Player.Dead)
			{
				m_csGuildManager.StopAutoPlay(this, enGuildPlayAutoState);
				return;
			}
			Debug.Log("m_GuildManager.OnEventStartAutoPlay()   enGuildPlayAutoState  = " + enGuildPlayAutoState);
			SetDisplayPath();
			Player.SetAutoPlay(this, true);
			GuildSupplySupportPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.SupplySupport)
		{
			if (!IsThisAutoPlaying()) return;
			if (objCaller == this) return;

			Debug.Log("m_GuildManager.OnEventStopAutoPlay()   enGuildPlayAutoState  = " + enGuildPlayAutoState);
			Player.SetAutoPlay(null, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateSupplySupportState()
	{
		if (m_csGuildManager == null) return;

		Debug.Log("1. OnEventUpdateSupplySupportState() GuildSupplySupportState = " + m_csGuildManager.GuildSupplySupportState);
		if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.None)
		{
			Debug.Log("2. OnEventUpdateSupplySupportState()");
			if (Player.IsTransformationStateCart())
			{
				Debug.Log("3. OnEventUpdateSupplySupportState()");
				CsCartManager.Instance.RemoveMyCart();

				if (IsThisAutoPlaying())
				{
					Player.SetAutoPlay(null, true);
				}
				return;
			}
		}

		if (IsThisAutoPlaying())
		{
			Debug.Log("4. OnEventUpdateSupplySupportState()");
			Player.ChangeState(CsHero.EnState.Idle);
			SetDisplayPath();
			GuildSupplySupportPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildSupplySupportQuestComplete(bool b, long l)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log(" OnEventGuildSupplySupportQuestComplete()");
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.GuildManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;

		if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.Accepted) // 1. 퀘스트 수행중
		{
			if (Player.IsTransformationStateCart() == false)								// 2. 카트 탑승중이 아닐때.
			{
				if (m_csGuildManager.GuildSupplySupportQuestPlay != null)
				{
					Debug.Log("3. CsPlayThemeQuestGuildSupplySupport.GuildSupplySupportPlay");

					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csGuildManager.GuildSupplySupportQuestPlay.CartContinentId == m_csMyHeroInfo.LocationId)
					{
						Player.DisplayPath.SetPath(m_csGuildManager.GuildSupplySupportQuestPlay.CartPosition);
						Player.SetWayPoint(m_csGuildManager.GuildSupplySupportQuestPlay.CartPosition, EnWayPointType.Interaction, 2f);
					}
					else
					{
						Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csGuildManager.GuildSupplySupportQuestPlay.CartContinentId);
						Player.DisplayPath.SetPath(vtPos);
						Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
					}
					return;
				}
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
	void GuildSupplySupportPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.Accepted) // 1. 퀘스트 수행중
			{
				if (Player.IsTransformationStateCart() == false)                                // 2. 카트 탑승중이 아닐때.
				{
					if (m_csGuildManager.GuildSupplySupportQuestPlay == null) return;           // 3. 카트 관련 정보가 없으면 리턴.

					if (MovePlayer(m_csMyHeroInfo.Nation.NationId, m_csGuildManager.GuildSupplySupportQuestPlay.CartContinentId, m_csGuildManager.GuildSupplySupportQuestPlay.CartPosition, 1.5f, false))
					{
						if (Player.BattleMode == false)
						{
							CsCartManager.Instance.SendCartGetOn(m_csGuildManager.GuildSupplySupportQuestPlay.CartInstanceId);
						}
					}
					return;
				}
			}

			m_csNpcInfo = GetNpcInfo();

			if (MovePlayer(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId, m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange, true))
			{
				CheckNpcDialog();
			}
		}


	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			m_csNpcInfo = GetNpcInfo();
			if (m_csGuildManager.Guild == null) return;                                                     // 길드 가입되어있지 않은경우.
			if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.None)
			{
				if (m_csGuildManager.MyGuildMemberGrade.GuildSupplySupportQuestEnabled == false) return;    // 퀘스트 수행 자격이 없는경우.
				if (m_csGuildManager.DailyGuildSupplySupportQuestStartCount != 0) return;                   // 이미 퀘스트 수행한 경우.

				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					if (m_csGuildManager.MyGuildMemberGrade.GuildSupplySupportQuestEnabled)
					{
						if (m_csGuildManager.StartSupplySupportNpctDialog())
						{
							Debug.Log("1. CsPlayThemeQuestGuildSupplySupport.CheckNpcDialog  >>  StartSupplySupportNpctDialog");
							NpcDialog(m_csNpcInfo);
						}
					}
				}
			}
			else // EnGuildSupplySupportState.Accepted
			{
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					Debug.Log("1. CsPlayThemeQuestGuildSupplySupport.CheckNpcDialog  >>  EndSupplySupportNpcDialog");
					Player.IsQuestDialog = true;
					Player.LookAtPosition(m_csNpcInfo.Position);
					m_csGuildManager.EndSupplySupportNpcDialog();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo()
	{
		if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.Accepted)
		{
			return CsGameData.Instance.GuildSupplySupportQuest.CompletionNpc;
		}
		else if (m_csGuildManager.GuildSupplySupportState == EnGuildSupplySupportState.None)
		{
			return CsGameData.Instance.GuildSupplySupportQuest.StartNpc;
		}
		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void ReStartAutoPlay()
	{
		Player.StartCoroutine(DelayStart());	
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);
		if (m_csGuildManager.Auto)
		{
			if (m_csGuildManager.GuildPlayAutoState == EnGuildPlayState.SupplySupport)
			{
				if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
				{
					m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.SupplySupport);
				}
				else
				{
					Debug.Log(" CsPlayThemeQuestGuildSupplySupport  RestartAutoPlay");
					SetDisplayPath();
					Player.SetAutoPlay(this, true);
					GuildSupplySupportPlay();
				}
			}
		}
	}
}
