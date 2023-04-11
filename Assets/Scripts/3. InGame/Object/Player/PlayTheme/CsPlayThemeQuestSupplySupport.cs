using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestSupplySupport : CsPlayThemeQuest 
{
	CsSupplySupportQuestManager m_csSupplySupportQuestManager;
	CsNpcInfo m_csNpcInfo;
	bool m_bArrival = false;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (RequestRidingCart())
			{
				Debug.Log("1. CsPlayThemeQuestSupplySupport.ArrivalMoveToPos");
				CheckNpcDialog();
				m_bArrival = true;
			}
		}
	}

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		//Debug.Log("CsPlayThemeQuestSupplySupport.Init()");
		m_csSupplySupportQuestManager = CsSupplySupportQuestManager.Instance;
		m_csSupplySupportQuestManager.EventStartAutoPlay += OnEventStartAutoPlay;
		m_csSupplySupportQuestManager.EventStopAutoPlay += OnEventStopAutoPlay;
		m_csSupplySupportQuestManager.EventUpdateState += OnEventUpdateState;

		m_csSupplySupportQuestManager.EventSupplySupportQuestComplete += OnEventSupplySupportQuestComplete;
		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
		Player.EventChangeCartRiding += OnEventChangeCartRiding;

		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csSupplySupportQuestManager.EventStartAutoPlay -= OnEventStartAutoPlay;
		m_csSupplySupportQuestManager.EventStopAutoPlay -= OnEventStopAutoPlay;
		m_csSupplySupportQuestManager.EventUpdateState -= OnEventUpdateState;

		m_csSupplySupportQuestManager.EventSupplySupportQuestComplete -= OnEventSupplySupportQuestComplete;
		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
		Player.EventChangeCartRiding -= OnEventChangeCartRiding;
		m_csSupplySupportQuestManager = null;
		m_csNpcInfo = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSupplySupportQuestComplete(bool b , long l, long ll, int n)
	{
		if (IsThisAutoPlaying())
		{
			Player.SetAutoPlay(null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		//m_timer.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		SupplySupportPlay();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
			if (bNotify)
			{
				m_csSupplySupportQuestManager.StopAutoPlay(this);
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
			if (m_csPlayer.Target == null) return;

			if (m_csPlayer.Target.CompareTag("Cart"))
			{
				if (CsCartManager.Instance.IsMyHeroRidingCart == false) // 카트 탑승중이 아닐때.
				{
					if (m_csMyHeroInfo.Nation.NationId != m_csMyHeroInfo.InitEntranceLocationParam) return;
					if (m_csSupplySupportQuestManager.CartContinentId != m_csMyHeroInfo.LocationId) return;

					if (Player.IsTargetInDistance(m_csSupplySupportQuestManager.CartPosition, 1.5f))
					{
						if (Player.BattleMode == false)
						{
							CsCartManager.Instance.SendCartGetOn(m_csSupplySupportQuestManager.CartInstanceId);
						}
					}
				}
			}
			else if (m_csPlayer.Target.CompareTag("Npc"))
			{
				CheckNpcDialog(m_csPlayer.Target);
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

	//---------------------------------------------------------------------------------------------------
	void OnEventStateEndOfIdle()
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
					Debug.Log("CorutineStateChangedToIdle");
					SupplySupportPlay();
				}
			}
		}
		m_bArrival = false;
	}

	#endregion Event.Player

	#region Event.SupplySupportQuestManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		if (Player.Dead)
		{
			m_csSupplySupportQuestManager.StopAutoPlay(this);
			return;
		}

		Debug.Log("m_csSupplySupportQuestManager.OnEventStartAutoPlay()");
		SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("m_csSupplySupportQuestManager.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.None)
		{
			if (CsCartManager.Instance.IsMyHeroRidingCart)
			{
				CsCartManager.Instance.RemoveMyCart();
			}
		}

		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
			Player.ChangeState(CsHero.EnState.Idle);
		}
	}

	#endregion Event.SupplySupportQuestManager
	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null) return;
		
		if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.None) // 퀘스트 수락 or 완료 
		{
			m_csNpcInfo = m_csSupplySupportQuestManager.SupplySupportQuest.StartNpc;
			if (m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.InitEntranceLocationParam && m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
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
		else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Executed) // 미션 완료일때.
		{
			m_csNpcInfo = m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc;
			if (m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.InitEntranceLocationParam && m_csNpcInfo.ContinentId == m_csMyHeroInfo.LocationId)
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
		else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Accepted) // 미션 수행중. 카트 교체
		{
			if (Player.IsTransformationStateCart() == false) // 카트 탑승중이 아닐때.
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csSupplySupportQuestManager.CartContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csSupplySupportQuestManager.CartPosition);
					Player.SetWayPoint(m_csSupplySupportQuestManager.CartPosition, EnWayPointType.Interaction, 2f);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csSupplySupportQuestManager.CartContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
			else
			{
				m_csNpcInfo = GetNpcInfo();
				CsSupplySupportQuestWayPoint csWayPoint = m_csSupplySupportQuestManager.SupplySupportQuest.SupplySupportQuestWayPointList.Find(a => a.CartChangeNpc.NpcId == m_csNpcInfo.NpcId);
				if (csWayPoint != null)
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && csWayPoint.CartChangeNpc.ContinentId == m_csMyHeroInfo.LocationId)
					{
						Player.DisplayPath.SetPath(csWayPoint.CartChangeNpc.Position);
						Player.SetWayPoint(m_csSupplySupportQuestManager.CartPosition, EnWayPointType.Interaction, 2f);
					}
					else
					{
						Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, csWayPoint.CartChangeNpc.ContinentId);
						Player.DisplayPath.SetPath(vtPos);
						Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
					}
				}
				else
				{
					Debug.Log("############### 확인 필요함 목표 포인트가 없음.      #########################");
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SupplySupportPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csNpcInfo == null)
			{
				m_csNpcInfo = GetNpcInfo();
			}

			if (RequestRidingCart())
			{
				if (m_csNpcInfo == null) return;

				if (MovePlayer(m_csMyHeroInfo.Nation.NationId, m_csNpcInfo.ContinentId, m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange, true))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool RequestRidingCart()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (Player.IsTransformationStateCart() == false) // 카트 탑승중이 아닐때.
			{
				if (MovePlayer(m_csMyHeroInfo.Nation.NationId, m_csSupplySupportQuestManager.CartContinentId, m_csSupplySupportQuestManager.CartPosition, 1.5f, false))
				{
					if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Accepted)
					{
						if (Player.BattleMode == false)
						{
							CsCartManager.Instance.SendCartGetOn(m_csSupplySupportQuestManager.CartInstanceId);
						}
					}
				}
			}
			return false;
		}

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog(Transform trNpc = null)
	{
		m_csNpcInfo = GetNpcInfo(trNpc);

		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto && IsThisAutoPlaying())
		{
			if (m_csNpcInfo == null) return;
			if (m_csNpcInfo.NpcType == EnNpcType.NationTransmission && Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange)) // 국가이동 Npc앞 도착.
			{
				Debug.Log("CheckNpcDialog  >>  m_csSupplySupportQuestManager.      국가이동 할 수 없습니다.");
				return;
			}
		}

		if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
		{
			if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.None) // 퀘스트 수락 or 완료 
			{
				m_csNpcInfo = m_csSupplySupportQuestManager.SupplySupportQuest.StartNpc;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					bool bQuestAble = Player.Level > m_csSupplySupportQuestManager.SupplySupportQuest.RequiredHeroLevel ? true : false;

					if (m_csSupplySupportQuestManager.StartNpctDialog(bQuestAble))
					{
						Debug.Log("CheckNpcDialog  >>  m_csSupplySupportQuestManager.StartNpctDialog()       StartNpctDialog  = " + bQuestAble);
						NpcDialog(m_csNpcInfo);
					}
				}
			}
			else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Executed) // 미션 완료일때.
			{
				m_csNpcInfo = m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc;
				if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
				{
					NpcDialog(m_csNpcInfo);
					m_csSupplySupportQuestManager.EndNpctDialog();
				}
			}
			else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Accepted) // 미션 수행중. 카트 교체
			{
				if (Player.IsTransformationStateCart() && m_csNpcInfo != null) // 카트 탑승중일때.
				{
					if (m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc.NpcId == m_csNpcInfo.NpcId)
					{
						if (Player.IsTargetInDistance(m_csNpcInfo.Position, m_csNpcInfo.InteractionMaxRange))
						{
							NpcDialog(m_csNpcInfo);
							m_csSupplySupportQuestManager.EndNpctDialog();
						}
					}
					else
					{
						CsSupplySupportQuestWayPoint csWayPoint = m_csSupplySupportQuestManager.SupplySupportQuest.SupplySupportQuestWayPointList.Find(a => a.CartChangeNpc.NpcId == m_csNpcInfo.NpcId);
						if (Player.IsTargetInDistance(csWayPoint.CartChangeNpc.Position, csWayPoint.CartChangeNpc.InteractionMaxRange))
						{
							NpcDialog(csWayPoint.CartChangeNpc);
							m_csSupplySupportQuestManager.CartChangeNpcDialog(csWayPoint.WayPoint);
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo GetNpcInfo(Transform trNpc = null)
	{
		if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Accepted)  // <미션중>  웨이 포인트 검색.
		{
			if (trNpc == null)
			{
				CsSupplySupportQuestWayPoint csWayPoint = null;

				for (int i = 0; i < m_csSupplySupportQuestManager.CsSupplySupportQuestWayPointList.Count; i++)
				{
					if (csWayPoint == null || m_csSupplySupportQuestManager.CsSupplySupportQuestWayPointList[i].WayPoint < csWayPoint.WayPoint)
					{
						csWayPoint = m_csSupplySupportQuestManager.CsSupplySupportQuestWayPointList[i];
					}                    
				}

				if (csWayPoint != null)
				{
					return csWayPoint.CartChangeNpc;
				}
				else
				{
					return m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc; // 더이상 들릴 웨이포인트가 없으면 완료 NPC이동.
				}
			}
			else
			{
				int nNpcId = CsIngameData.Instance.IngameManagement.GetNpcId(trNpc);

				if (m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc.NpcId == nNpcId)
				{
					return m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc;
				}
				else
				{
					CsSupplySupportQuestWayPoint csWayPoint = m_csSupplySupportQuestManager.SupplySupportQuest.SupplySupportQuestWayPointList.Find(a => a.CartChangeNpc.NpcId == nNpcId);
					if (csWayPoint != null)
					{
						return csWayPoint.CartChangeNpc;
					}
				}
			}
		}
		else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.None)  // <미션중>  웨이 포인트 검색.
		{
			return m_csSupplySupportQuestManager.SupplySupportQuest.StartNpc;
		}
		else if (m_csSupplySupportQuestManager.QuestState == EnSupplySupportState.Executed)  // <미션중>  웨이 포인트 검색.
		{
			return m_csSupplySupportQuestManager.SupplySupportQuest.CompletionNpc;
		}
		return null;
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
		if (m_csSupplySupportQuestManager.Auto)
		{
			if (Player.Dead || CsIngameData.Instance.IngameManagement.IsContinent() == false)
			{
				Player.SetAutoPlay(null, true);
			}
			else
			{
				Debug.Log("m_csSupplySupportQuestManager.RestartAutoPlay()");
				SetDisplayPath();
				Player.SetAutoPlay(this, true);
			}
		}
	}
}
