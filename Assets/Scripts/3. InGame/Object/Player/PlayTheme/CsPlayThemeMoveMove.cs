using System.Collections;
using UnityEngine;

public enum EnAutoMovePlay { None = 0, Move, Npc, Fishing, MainCart, SupplyCart, NationWar, GuildSupplyCart, EliteMonster, GuildFarmNpc, TrueHero }

public class CsPlayThemeMoveMove : CsPlayThemeMove
{
	EnAutoMovePlay m_enAutoMovePlay = EnAutoMovePlay.None;
	bool m_bAutoStart = false;
	bool m_bSavable = true;
	bool m_bTartgetNpc = false;

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------
	public override int GetTypeSub() { return (int)m_enAutoMovePlay; }
	public override bool IsSavable() { return m_bSavable; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (IsThisAutoPlaying())
			{
				if (m_enAutoMovePlay == EnAutoMovePlay.NationWar)
				{
					if (m_csMyHeroInfo.InitEntranceLocationParam != CsGameData.Instance.AutoMoveNationId)
					{
						CsNpcInfo csNpcInfo = CsGameData.Instance.NpcInfoList.Find(a => a.NpcType == EnNpcType.NationTransmission);
						if (csNpcInfo.NpcType == EnNpcType.NationTransmission && Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
						{
							Player.IsQuestDialog = true;
							Player.LookAtPosition(csNpcInfo.Position);
							CsNationWarManager.Instance.NationTransmissionReadyOK();
							Debug.Log("CsPlayThemeMoveMove     >>     ArrivalMoveToPos     CsNationWarManager.NationTransmissionReadyOK();");
						}
					}
					else
					{
						m_enAutoMovePlay = EnAutoMovePlay.None;
						Player.SetAutoPlay(null, true);
					}
				}
				else
				{
					if (Player.MovePos == CsGameData.Instance.AutoMoveObjectPos)
					{
						Debug.Log("CsPlayThemeMoveMove.ArrivalMoveToPos()    m_enAutoMovePlay = " + m_enAutoMovePlay);
						Player.ChangeState(CsHero.EnState.Idle);

						if (m_enAutoMovePlay == EnAutoMovePlay.Npc)
						{
							CsGameEventToUI.Instance.OnEventArrivalNpcByAuto(CsIngameData.Instance.AutoPlayNpcId, CsGameData.Instance.AutoMoveNationId);
						}
						else if (m_enAutoMovePlay == EnAutoMovePlay.Fishing)
						{
							CsFishingQuestManager.Instance.ArrivalNpcDialog();
							CsIngameData.Instance.IngameManagement.NpcDialog(CsIngameData.Instance.AutoPlayNpcId);
						}
						else if (m_enAutoMovePlay == EnAutoMovePlay.MainCart)
						{
							if (Player.BattleMode == false)
							{
								CsCartManager.Instance.SendCartGetOn(CsMainQuestManager.Instance.CartInstanceId);
							}
						}
						else if (m_enAutoMovePlay == EnAutoMovePlay.SupplyCart)
						{
							if (Player.BattleMode == false)
							{
								CsCartManager.Instance.SendCartGetOn(CsSupplySupportQuestManager.Instance.CartInstanceId);
							}
						}
						else if (m_enAutoMovePlay == EnAutoMovePlay.GuildSupplyCart)
						{
							if (Player.BattleMode == false)
							{
								CsCartManager.Instance.SendCartGetOn(CsGuildManager.Instance.GuildSupplySupportQuestPlay.CartInstanceId);
							}
						}
						else if (m_enAutoMovePlay == EnAutoMovePlay.GuildFarmNpc)
						{
							CsGuildManager.Instance.GuildFarmQuestNPCDialog();
						}

						m_enAutoMovePlay = EnAutoMovePlay.None;
						Player.SetAutoPlay(null, true);
					}
				}
			}
		}
		else
		{
			if (Player.MovePos == CsGameData.Instance.AutoMoveObjectPos)
			{
				m_enAutoMovePlay = EnAutoMovePlay.None;
				Player.SetAutoPlay(null, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterPortal(int nPortalId) 
	{
		Debug.Log("1. ##############                      EnterPortal     nPortalId = " + nPortalId);
		if (m_enAutoMovePlay == EnAutoMovePlay.Move)
		{
			Debug.Log("2. ##############                      EnterPortal     CsIngameData.Instance.AutoPortalId = " + CsIngameData.Instance.AutoPortalId);
			if (nPortalId == CsIngameData.Instance.AutoPortalId)
			{
				Debug.Log("3. ##############                      EnterPortal");
				CsIngameData.Instance.AutoPlaySub = 0;
				Player.SetAutoPlay(null, true);
			}
		}
 
	}
	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		CsGameEventToIngame.Instance.EventPartyCalled += OnEventPartyCalled;
		CsGameEventToIngame.Instance.EventMapMove += OnEventMapMove;
		CsGameEventToIngame.Instance.EventNpcAutoMove += OnEventNpcAutoMove;

		CsDungeonManager.Instance.EventMyHeroDungeonEnterMoveStop += OnEventMyHeroDungeonEnterMoveStop;
		CsFishingQuestManager.Instance.EventFishingNpcAutoMove += OnEventFishingNpcAutoMove;
		CsNationWarManager.Instance.EventMoveToNationWarMonster += OnEventMoveToNationWarMonster;
		CsCartManager.Instance.EventAutoMoveToCart += OnEventAutoMoveToCart;
		CsEliteManager.Instance.EventAutoMoveToEliteMonster += OnEventAutoMoveToEliteMonster;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAutoMove += OnEventTrueHeroQuestAutoMove;

		m_bUpdateOnMove = true;
		RestartAutoPlay();
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		CsGameEventToIngame.Instance.EventPartyCalled -= OnEventPartyCalled;
		CsGameEventToIngame.Instance.EventMapMove -= OnEventMapMove;
		CsGameEventToIngame.Instance.EventNpcAutoMove -= OnEventNpcAutoMove;

		CsDungeonManager.Instance.EventMyHeroDungeonEnterMoveStop -= OnEventMyHeroDungeonEnterMoveStop;
		CsFishingQuestManager.Instance.EventFishingNpcAutoMove -= OnEventFishingNpcAutoMove;
		CsNationWarManager.Instance.EventMoveToNationWarMonster -= OnEventMoveToNationWarMonster;
		CsCartManager.Instance.EventAutoMoveToCart -= OnEventAutoMoveToCart;
		CsEliteManager.Instance.EventAutoMoveToEliteMonster -= OnEventAutoMoveToEliteMonster;
		CsTrueHeroQuestManager.Instance.EventTrueHeroQuestAutoMove -= OnEventTrueHeroQuestAutoMove;

		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		if (m_csPlayer.IsStateIdle == false && m_bAutoStart == false) return; // 대기 상태가 아닐때는 새로운 자동 이동 입력시에만 호출.
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto || m_enAutoMovePlay == EnAutoMovePlay.Move)
		{
			if (m_csMyHeroInfo.LocationId == CsGameData.Instance.AutoMoveLocationId && m_csMyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.AutoMoveNationId) // 대륙 및 국가 확인.
			{
				MovePlayer(CsGameData.Instance.AutoMoveObjectPos, Player.AutoMoveStopRange, m_bTartgetNpc);
			}
			else
			{
				MovePlayer(Player.ChaseContinent(CsGameData.Instance.AutoMoveNationId, CsGameData.Instance.AutoMoveLocationId), Player.AutoMoveStopRange, m_bTartgetNpc);
			}
		}

		m_bAutoStart = false;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeMoveMove.StopAutoPlay()         bNotify = " + bNotify);
			CsGameData.Instance.AutoMoveObjectPos = Vector3.zero;
			CsGameData.Instance.AutoMoveLocationId = 0;
			CsGameData.Instance.AutoMoveNationId = 0;
			CsIngameData.Instance.AutoPlayNpcId = 0;
			m_enAutoMovePlay = EnAutoMovePlay.None;

			if (bNotify)
			{
				CsGameEventToUI.Instance.OnEventAutoStop(EnAutoStateType.Move);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroDungeonEnterMoveStop()
	{
		Debug.Log("OnEventMyHeroDungeonEnterMoveStop()");
		Player.SetAutoPlay(null, true);
	}

	#endregion Override

	#region Event

	//---------------------------------------------------------------------------------------------------
	void OnEventPartyCalled(int nContinentId, int nNationId, Vector3 vtPosition) // 파티원 소집.
	{
		Debug.Log("OnEventPartyCalled() nContinentId = " + nContinentId + " // nNationId = " + nNationId + " // v3Position = " + vtPosition);
		Player.AutoMoveStopRange = 2f;
		SetMove(EnAutoMovePlay.Move, nContinentId, nNationId, vtPosition);
	}

	//---------------------------------------------------------------------------------------------------
	bool OnEventMapMove(int nContinentId, int nNationId, Vector3 vtPosition, int nPortalId) // 지점 이동.
	{
		Debug.Log(" OnEventMapMove        nContinentId = " + nContinentId + "  //  vtPosition = " + vtPosition);
		if (nContinentId == m_csMyHeroInfo.LocationId)
		{
			if (vtPosition.y == 100) // 미니맵 2D좌표를 3D 네비 좌표로 변환 처리.
			{
				vtPosition = CheckMovePosition(vtPosition);
				if (vtPosition == Vector3.zero)
				{
					Debug.Log("OnEventMapMove   >>   Don't Move Position,  Becauese Don't have Navmesh");
					Player.SetAutoPlay(null, true);
					return false;
				}
			}
		}

		Player.AutoMoveStopRange = 2f;
		SetMove(EnAutoMovePlay.Move, nContinentId, nNationId, vtPosition, 0, nPortalId);
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventNpcAutoMove(int nNationId, CsNpcInfo csNpcInfo)
	{
		Debug.Log("CsPlayThemeMoveMove.OnEventTransmissionNpcMove() csNpcInfo = " + csNpcInfo);
		if (csNpcInfo != null)
		{
			Player.AutoMoveStopRange = csNpcInfo.InteractionMaxRange;
			SetMove(EnAutoMovePlay.Npc, csNpcInfo.ContinentId, nNationId, csNpcInfo.Position, csNpcInfo.NpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFishingNpcAutoMove(CsNpcInfo csNpcInfo)
	{
		Debug.Log("CsPlayThemeMoveMove.OnEventFishingNpcAutoMove()  csNpcInfo = " + csNpcInfo);
		if (csNpcInfo != null)
		{
			Player.AutoMoveStopRange = csNpcInfo.InteractionMaxRange;
			SetMove(EnAutoMovePlay.Fishing, csNpcInfo.ContinentId, m_csMyHeroInfo.Nation.NationId, csNpcInfo.Position, csNpcInfo.NpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMoveToNationWarMonster(int nNationId, int nContinentId, Vector3 vtPosition)
	{
		Debug.Log("CsPlayThemeMoveMove.OnEventMoveToNationWarMonster()    NationId = " + nNationId + " // nContinentId = " + nContinentId);
		Player.AutoMoveStopRange = 4f;
		SetMove(EnAutoMovePlay.NationWar, nContinentId, nNationId, vtPosition);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAutoMoveToCart()
	{
		Debug.Log("1. CsPlayThemeMoveMove.OnEventAutoMoveToCart()");
		Player.AutoMoveStopRange = 1.5f;
		if (CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Cart && CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.None) // 메인퀘스트 중일때.
		{
			Debug.Log("2. CsPlayThemeMoveMove.OnEventAutoMoveToCart()        EnAutoMovePlay.MainCart");
			SetMove(EnAutoMovePlay.MainCart,
					CsMainQuestManager.Instance.CartContinentId, 
					m_csMyHeroInfo.Nation.NationId, 
					CsMainQuestManager.Instance.CartPosition);
		}
		else if (CsGuildManager.Instance.GuildSupplySupportQuestPlay != null) // 길드 보급지원.
		{
			Debug.Log("2. CsPlayThemeMoveMove.OnEventAutoMoveToCart()        EnAutoMovePlay.GuildSupplyCart");
			SetMove(EnAutoMovePlay.GuildSupplyCart, 
					CsGuildManager.Instance.GuildSupplySupportQuestPlay.CartContinentId, 
					m_csMyHeroInfo.Nation.NationId, 
					CsGuildManager.Instance.GuildSupplySupportQuestPlay.CartPosition);
		}
		else // 보급지원 퀘스트.
		{
			Debug.Log("2. CsPlayThemeMoveMove.OnEventAutoMoveToCart()        EnAutoMovePlay.SupplyCart");
			SetMove(EnAutoMovePlay.SupplyCart,
					CsSupplySupportQuestManager.Instance.CartContinentId, 
					m_csMyHeroInfo.Nation.NationId,
					CsSupplySupportQuestManager.Instance.CartPosition);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAutoMoveToEliteMonster(int nContinentId, Vector3 vtPosition)
	{
		Debug.Log("CsPlayThemeMoveMove.OnEventAutoMoveToEliteMonster()        nContinentId = " + nContinentId + " // vtPosition = " + vtPosition);
		Player.AutoMoveStopRange = 4f;
		SetMove(EnAutoMovePlay.EliteMonster, nContinentId, m_csMyHeroInfo.Nation.NationId, vtPosition);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTrueHeroQuestAutoMove(int nContinentId, int nNationId, Vector3 vtPosition)
	{
		Debug.Log("CsPlayThemeMoveMove.OnEventTrueHeroQuestAutoMove()    NationId = " + nNationId + " // nContinentId = " + nContinentId);
		Player.AutoMoveStopRange = 2f;
		SetMove(EnAutoMovePlay.TrueHero, nContinentId, nNationId, vtPosition);
	}

	#endregion Event

	//---------------------------------------------------------------------------------------------------
	void RestartAutoPlay()
	{
		Player.StartCoroutine(DelayStart());
	}
			
	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayStart()
	{
		yield return new WaitUntil(() => CsIngameData.Instance.ActiveScene);

		m_enAutoMovePlay = (EnAutoMovePlay)CsIngameData.Instance.AutoPlaySub;
		//Debug.Log("CsPlayThemeMoveMove.RestartAutoPlay()      GetTypeKey() = " + GetTypeKey() + " // AutoPlayKey = " + CsIngameData.Instance.AutoPlayKey);
		if (GetTypeKey() == CsIngameData.Instance.AutoPlayKey)
		{
			if (CsIngameData.Instance.IngameManagement.IsContinent()) // 대륙일때.
			{
				if (m_enAutoMovePlay == EnAutoMovePlay.Npc)
				{
					CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(CsIngameData.Instance.AutoPlayNpcId);
					if (csNpcInfo != null)
					{
						OnEventNpcAutoMove(CsGameData.Instance.AutoMoveNationId, csNpcInfo);
					}
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.Move)
				{
					OnEventMapMove(CsGameData.Instance.AutoMoveLocationId,
								   CsGameData.Instance.AutoMoveNationId,
								   CsGameData.Instance.AutoMoveObjectPos,
								   CsIngameData.Instance.AutoPortalId);
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.Fishing)
				{
					CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(CsIngameData.Instance.AutoPlayNpcId);
					if (csNpcInfo != null)
					{
						OnEventFishingNpcAutoMove(csNpcInfo);
					}
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.MainCart)
				{
					OnEventAutoMoveToCart();
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.NationWar)
				{
					OnEventMoveToNationWarMonster(CsGameData.Instance.AutoMoveNationId,
												  CsGameData.Instance.AutoMoveLocationId,
												  CsGameData.Instance.AutoMoveObjectPos);
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.EliteMonster)
				{
					OnEventAutoMoveToEliteMonster(CsGameData.Instance.AutoMoveLocationId, CsGameData.Instance.AutoMoveObjectPos);
				}
				else if (m_enAutoMovePlay == EnAutoMovePlay.TrueHero)
				{
					OnEventTrueHeroQuestAutoMove(CsGameData.Instance.AutoMoveLocationId, CsGameData.Instance.AutoMoveNationId, CsGameData.Instance.AutoMoveObjectPos);
				}

			}
			else // 대륙이 아닐때.
			{
				Player.SetAutoPlay(null, true);
			}
		}

		CsIngameData.Instance.AutoPlayKey = 0;
		CsIngameData.Instance.AutoPlaySub = 0;
	}

	//---------------------------------------------------------------------------------------------------
	Vector3 CheckMovePosition(Vector3 vtPos)
	{
		int nLayerMask = 1 << LayerMask.NameToLayer("Terrain");
		Vector3 vtDir = new Vector3(0f, -1f, 0f);
		Ray ray = new Ray(vtPos, vtDir);
		RaycastHit hitInfo;

		if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, 100, nLayerMask))
		{
			if (hitInfo.collider.gameObject.CompareTag("NavMesh"))
			{
				return hitInfo.point; // 이동할 InGame 좌표.
			}
		}
		return Vector3.zero;
	}

	//---------------------------------------------------------------------------------------------------
	void SetMove(EnAutoMovePlay enAutoMovePlay, int nContinentId, int nNationId, Vector3 vtPosition, int nNpcId = 0, int nPortalId = 0)
	{
		if (CsIngameData.Instance.IngameManagement.IsContinent() == false || (CsGameData.Instance.MyHeroInfo.LocationId == nContinentId && CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == nNationId))
		{
			m_bSavable = false;
		}
		else
		{
			m_bSavable = true;
		}

		if (enAutoMovePlay == EnAutoMovePlay.Move)
		{
			Player.ResetBattleMode();
		}

		m_bTartgetNpc = nNpcId == 0 ? false : true;

		Player.SetAutoPlay(null, true);
		CsGameData.Instance.AutoMoveObjectPos = vtPosition;
		CsGameData.Instance.AutoMoveLocationId = nContinentId;
		CsGameData.Instance.AutoMoveNationId = nNationId;
		CsIngameData.Instance.AutoPlayNpcId = nNpcId;
		CsIngameData.Instance.AutoPortalId = nPortalId;

		if (m_csMyHeroInfo.LocationId == CsGameData.Instance.AutoMoveLocationId && m_csMyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.AutoMoveNationId)
		{
			EnWayPointType enWayPojntType = m_bTartgetNpc ? EnWayPointType.Npc : EnWayPointType.Move;
			Player.SetWayPoint(vtPosition, enWayPojntType, 4);
			Player.DisplayPath.SetPath(vtPosition);
		}
		else
		{
			Player.SetWayPoint(Player.ChaseContinent(nNationId, nContinentId), EnWayPointType.Move, 4);
			Player.DisplayPath.SetPath(Player.ChaseContinent(nNationId, nContinentId));
		}

		m_enAutoMovePlay = enAutoMovePlay;
		Player.SetAutoPlay(this, false);
		m_bAutoStart = true;
	}
}