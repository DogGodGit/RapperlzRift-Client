using SimpleDebugLog;
using System.Collections;
using UnityEngine;


public class CsPlayThemeQuestBountyHunter : CsPlayThemeQuest
{
	CsBountyHunterQuestManager m_csBountyHunterQuestManager;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			dd.d("1. CsPlayThemeQuestSecretLetter.ArrivalMoveToPos");
			if (m_csMyHeroInfo.InitEntranceLocationParam != m_csMyHeroInfo.Nation.NationId) // 타국가 일때 본인 국가로 자동 이동.
			{
				Player.SelectTarget(Player.FindNpc(m_csPlayer.MovePos), true); // Npc 검색.

				if (m_csPlayer.Target != null && m_csPlayer.Target.CompareTag("Npc")) // 타겟이 Npc일때.
				{
					CsGameEventToUI.Instance.OnEventArrivalNpcByAuto(CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target), m_csMyHeroInfo.Nation.NationId);
				}
			}
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csBountyHunterQuestManager = CsBountyHunterQuestManager.Instance;
		m_csBountyHunterQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csBountyHunterQuestManager.EventStopAutoPlay +=OnEventStopAutoPlay;
		m_csBountyHunterQuestManager.EventBountyHunterQuestComplete += OnEventBountyHunterQuestComplete;

		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csBountyHunterQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csBountyHunterQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csBountyHunterQuestManager.EventBountyHunterQuestComplete -= OnEventBountyHunterQuestComplete;
		m_csBountyHunterQuestManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		//m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	//protected override void UpdateAutoPlay()
	//{
	//	if (m_csBountyHunterQuestManager.BountyHunterQuest != null)
	//	{
	//		BountyHunterPlay();
	//	}
	//}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeQuestBountyHunter.StopAutoPlay()     bNotify = " + bNotify);
			if (bNotify)
			{
				m_csBountyHunterQuestManager.StopAutoPlay(this);
			}
		}
	}

	#endregion Override

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csBountyHunterQuestManager.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeQuestBountyHunter.OnEventStartAutoPlay()");
		SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeQuestBountyHunter.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBountyHunterQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeQuestBountyHunter.OnEventBountyHunterQuestComplete()");
			Player.SetAutoPlay(null, true);
		}
	}

	#endregion Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;
		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csBountyHunterQuestManager.BountyHunterQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId) 
		{
			Player.DisplayPath.SetPath(m_csBountyHunterQuestManager.BountyHunterQuest.TargetPosition);
			Player.SetWayPoint(m_csBountyHunterQuestManager.BountyHunterQuest.TargetPosition, EnWayPointType.Battle, m_csBountyHunterQuestManager.BountyHunterQuest.TargetRadius);
		}
		else
		{
			Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csBountyHunterQuestManager.BountyHunterQuest.TargetContinent.ContinentId);
			Player.DisplayPath.SetPath(vtPos);
			Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void BountyHunterPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (MovePlayer(m_csMyHeroInfo.Nation.NationId,
						   m_csBountyHunterQuestManager.BountyHunterQuest.TargetContinent.ContinentId,
						   m_csBountyHunterQuestManager.BountyHunterQuest.TargetPosition,
						   m_csBountyHunterQuestManager.BountyHunterQuest.TargetRadius,
						   false))
			{
				BattlePlayer(m_csBountyHunterQuestManager.BountyHunterQuest.TargetPosition, m_csBountyHunterQuestManager.BountyHunterQuest.TargetRadius);
			}
		}
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
		if (m_csBountyHunterQuestManager.Auto)
		{
			if (Player.Dead || m_csBountyHunterQuestManager.BountyHunterQuest == null || !CsIngameData.Instance.IngameManagement.IsContinent())
			{
				m_csBountyHunterQuestManager.StopAutoPlay(this);
			}
			else
			{
				Debug.Log("CsPlayThemeQuestBountyHunter.RestartAutoPlay()");
				SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
