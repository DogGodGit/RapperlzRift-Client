using SimpleDebugLog;
using UnityEngine;

public partial class CsPlayThemeQuestMain
{
	//---------------------------------------------------------------------------------------------------
	class CsState
	{
		protected IPlayThemeQuestMain m_itrTheme;
		protected CsMainQuestManager m_csMainQuestManager;
		protected CsMyPlayer m_csPlayer;
		protected CsMyHeroInfo m_csMyHeroInfo;
		protected CsMyPlayer Player { get { return m_csPlayer; } }

		public virtual void Init(IPlayThemeQuestMain itrTheme, CsMyPlayer csPlayer)
		{
			m_itrTheme = itrTheme;
			m_csMyHeroInfo = CsGameData.Instance.MyHeroInfo;
			m_csMainQuestManager = CsMainQuestManager.Instance;
			m_csPlayer = csPlayer;
		}

		public virtual void Uninit() // 테스트중.
		{
			m_itrTheme = null;
			m_csMyHeroInfo = null;
			m_csMainQuestManager = null;			
			m_csPlayer = null;
		}

		public virtual void Play(){}
		public virtual void Update(){}
		public virtual void SetDisplayPath(){}
		public virtual void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget){}
		public virtual void StateChangedToIdle() {}
		public virtual void OnEventChangeCartRiding(){}

		//---------------------------------------------------------------------------------------------------
		protected void NpcDialog(CsNpcInfo csNpcInfo)
		{
			if (csNpcInfo == null) return;
			if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
			{
				Player.SelectTarget(Player.FindNpc(csNpcInfo.Position), true);
				Player.transform.LookAt(csNpcInfo.Position);
				Player.IsQuestDialog = true;
				CsIngameData.Instance.IngameManagement.NpcDialog(csNpcInfo.NpcId);
			}
		}

		protected bool MovePlayer(CsNpcInfo csNpcInfo)
		{
			if (csNpcInfo != null)
			{
				if (MovePlayer(csNpcInfo.ContinentId, csNpcInfo.Position, csNpcInfo.InteractionMaxRange, true))
				{
					return true;
				}
				return false;
			}
			return true;
		}


		protected bool MovePlayer(int nContinentId, Vector3 vtPosition, float flRange, bool bTargetNpc)
		{
			if (nContinentId == m_csMyHeroInfo.LocationId && m_csMyHeroInfo.InitEntranceLocationParam == m_csMyHeroInfo.Nation.NationId) // 대륙 및 국가 확인.)
			{
				if (Player.IsTargetInDistance(vtPosition, flRange))
				{
					return true;
				}

				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (Player.IsStateIdle)
					{
						Player.MoveToPos(vtPosition, flRange, bTargetNpc);
					}
				}
			}
			else
			{
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (Player.IsStateIdle)
					{
						vtPosition = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);
						if (Player.IsTargetInDistance(vtPosition, 2f) == false)
						{
							Player.MoveToPos(vtPosition, 2f, false);
						}
					}
				}
			}
			return false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateAccept : CsState
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMainQuestManager.MainQuest.StartNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						NpcDialog(m_csMainQuestManager.MainQuest.StartNpc);
						m_csMainQuestManager.AcceptReadyOK();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMainQuestManager.MainQuest.StartNpc == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.StartNpc.Position);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.StartNpc.Position, EnWayPointType.Npc, m_csMainQuestManager.MainQuest.StartNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.StartNpc.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (!bMoveByTouchTarget || Player.Target == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (Player.Target.CompareTag("Npc"))
				{
					CsNpcInfo csNpcInfo = m_csMainQuestManager.MainQuest.StartNpc;
					if (csNpcInfo == null)
					{
						csNpcInfo = m_csMainQuestManager.MainQuest.CompletionNpc;
						if (csNpcInfo != null)
						{
							if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == csNpcInfo.NpcId)
							{
								if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
								{
									Debug.Log("1. CsPlayThemeQuestMain.CsStateExecuteDungeon.ArrivalMoveByTouchByManual  >>  CompletionNpc");
									NpcDialog(csNpcInfo);
									m_csMainQuestManager.Accept();
								}
							}
						}
					}
					else
					{
						if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == csNpcInfo.NpcId)
						{
							if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
							{
								Debug.Log("2. CsPlayThemeQuestMain.CsStateExecuteDungeon.ArrivalMoveByTouchByManual  >>  StartNpc");
								NpcDialog(csNpcInfo);
								m_csMainQuestManager.AcceptReadyOK();
							}
						}
					}
				}
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecute : CsState
	{
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteMove : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				CsMainQuest csQuest = m_csMainQuestManager.MainQuest;
				if (MovePlayer(csQuest.TargetContinent.ContinentId, csQuest.TargetPosition, csQuest.TargetRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.TargetPosition);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.TargetPosition, EnWayPointType.Move, m_csMainQuestManager.MainQuest.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void StateChangedToIdle()
		{			
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteCart : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				CsMainQuest csQuest = m_csMainQuestManager.MainQuest;

				if (Player.IsTransformationStateCart()) // 카트 탑승중
				{
					if (MovePlayer(csQuest.TargetContinent.ContinentId, csQuest.TargetPosition, csQuest.TargetRadius, false))
					{
						Player.ChangeState(CsHero.EnState.Idle);
					}
				}
				else // 카트에 탑승중이지 않을때.
				{
					if (MovePlayer(m_csMainQuestManager.CartContinentId, m_csMainQuestManager.CartPosition, 1.5f, false)) // 도착 거리 임시. csCartObject.Cart.RidableRange
					{
						Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
						if (CsIngameData.Instance.IngameManagement.GetCsCartObject(m_csMainQuestManager.CartInstanceId) != null)
						{
							if (Player.BattleMode == false)
							{
								CsCartManager.Instance.SendCartGetOn(m_csMainQuestManager.CartInstanceId);
							}
						}
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (CsCartManager.Instance.IsMyHeroRidingCart) // 카트 탑승중
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.TargetPosition);
					Player.SetWayPoint(m_csMainQuestManager.MainQuest.TargetPosition, EnWayPointType.Npc, m_csMainQuestManager.MainQuest.TargetRadius);
				}
				else
				{
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.TargetContinent.ContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
			else // 카트에 탑승중이지 않을때.
			{
				if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.CartContinentId == m_csMyHeroInfo.LocationId)
				{
					Player.DisplayPath.SetPath(m_csMainQuestManager.CartPosition);
					Player.SetWayPoint(m_csMainQuestManager.CartPosition, EnWayPointType.Interaction, 2f);
				}
				else
				{					
					Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.CartContinentId);
					Player.DisplayPath.SetPath(vtPos);
					Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
				}
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csMainQuestManager.MainQuest == null) return;

			if (!bMoveByTouchTarget || Player.Target == null) return;

			if (Player.IsTransformationStateCart() == false)
			{
				if (Player.Target.CompareTag("Cart"))
				{
					CsCartObject csCartObject = CsIngameData.Instance.IngameManagement.GetCsCartObject(m_csMainQuestManager.CartInstanceId);
					if (Player.Target == csCartObject.transform)
					{
						Debug.Log("CsPlayThemeQuestMain.CsStateExecuteCart.ArrivalMoveByTouchByManual");
						if (Player.IsTargetInDistance(csCartObject.transform.position, csCartObject.Cart.RidableRange))
						{
							if (Player.IsTransformationStateMount())
							{
								Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
							}

							Player.LookAtPosition(csCartObject.transform.position);
							if (Player.BattleMode == false)
							{
								CsCartManager.Instance.SendCartGetOn(csCartObject.InstanceId);
							}
						}
					}
				}
			}
		}

		public override void OnEventChangeCartRiding()
		{
			Debug.Log("@@@@@@@@@@@@        CsPlayThemeQuestMain.CsStateExecuteCart   OnEventChangeCartRiding           @@@@@@@@@@@@@");
			SetDisplayPath();
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteDungeon : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMainQuestManager.MainQuest.StartNpc))
				{
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						dd.d("PlayExecute()", m_csMainQuestManager.MainQuest.MainQuestType, m_csMainQuestManager.MainQuest.TargetCount, m_csMainQuestManager.MainQuest.MainQuestNo);
						NpcDialog(m_csMainQuestManager.MainQuest.StartNpc);
						CsMainQuestDungeonManager.Instance.OnEventContinentExitForMainQuestDungeonReEnter();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMainQuestManager.MainQuest.StartNpc == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.StartNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.StartNpc.Position);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.StartNpc.Position, EnWayPointType.Npc, m_csMainQuestManager.MainQuest.StartNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.StartNpc.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (!bMoveByTouchTarget || Player.Target == null) return;
			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (Player.Target.CompareTag("Npc"))
				{
					CsNpcInfo csNpcInfo = m_csMainQuestManager.MainQuest.StartNpc;
					if (csNpcInfo == null) return;
					if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == csNpcInfo.NpcId)
					{
						if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
						{
							Debug.Log("CsPlayThemeQuestMain.CsStateExecuteDungeon.ArrivalMoveByTouchByManual");
							NpcDialog(csNpcInfo);
							CsMainQuestDungeonManager.Instance.OnEventContinentExitForMainQuestDungeonReEnter();
						}
					}
				}
			}
		}

		public override void StateChangedToIdle()
		{			
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteBattle : CsStateExecute
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				CsMainQuest csMainQuest = m_csMainQuestManager.MainQuest;
				if (MovePlayer(csMainQuest.TargetContinent.ContinentId, csMainQuest.TargetPosition, csMainQuest.TargetMonster.QuestAreaRadius, false))
				{
					Player.ChangeState(CsHero.EnState.Idle);
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.TargetPosition);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.TargetPosition, EnWayPointType.Battle, m_csMainQuestManager.MainQuest.TargetRadius);
			}
			else
			{				
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void Update()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (m_csMainQuestManager.MainQuestState == EnMainQuestState.Accepted && Player.State != CsHero.EnState.MoveToPos) // 자동전투 미션 완료가 아닐때.
				{
					CsMainQuest csMainQuest = m_csMainQuestManager.MainQuest;

					if (MovePlayer(csMainQuest.TargetContinent.ContinentId, csMainQuest.TargetPosition, csMainQuest.TargetMonster.QuestAreaRadius, false))
					{
						Player.PlayBattle(csMainQuest.TargetMonster.MonsterId, csMainQuest.TargetPosition, csMainQuest.TargetMonster.QuestAreaRadius);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateExecuteInteraction : CsStateExecute
	{
		public override void Init(IPlayThemeQuestMain csTheme, CsMyPlayer csPlayer)
		{
			base.Init(csTheme, csPlayer);
		}

		public override void Uninit()
		{	
			base.Uninit();
		}

		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				CsMainQuest csQuest = m_csMainQuestManager.MainQuest;

				if (MovePlayer(csQuest.TargetContinent.ContinentId, csQuest.TargetPosition, csQuest.TargetRadius, false))
				{
					dd.d("PlayExecute().CsStateExecuteInteraction", csQuest.MainQuestType, csQuest.TargetCount, csQuest.MainQuestNo, csQuest.TargetContinentObject.ObjectId);
					Player.ContinentObjectInteractionStart();
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.TargetContinent.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.TargetPosition);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.TargetPosition, EnWayPointType.Interaction, m_csMainQuestManager.MainQuest.TargetRadius);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.TargetContinent.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (!bMoveByTouchTarget || Player.Target == null) return;
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	class CsStateComplete : CsState
	{
		public override void Play()
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (MovePlayer(m_csMainQuestManager.MainQuest.CompletionNpc))
				{
					Debug.Log("CsStateComplete.Play()");
					if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
					{
						NpcDialog(m_csMainQuestManager.MainQuest.CompletionNpc);
						m_csMainQuestManager.CompleteReadyOK();
					}
				}
			}
		}

		public override void SetDisplayPath()
		{
			if (Player == null || Player.DisplayPath == null) return;

			if (m_csMainQuestManager.MainQuest.CompletionNpc == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam && m_csMainQuestManager.MainQuest.CompletionNpc.ContinentId == m_csMyHeroInfo.LocationId)
			{
				Player.DisplayPath.SetPath(m_csMainQuestManager.MainQuest.CompletionNpc.Position);
				Player.SetWayPoint(m_csMainQuestManager.MainQuest.CompletionNpc.Position, EnWayPointType.Npc, m_csMainQuestManager.MainQuest.CompletionNpc.InteractionMaxRange);
			}
			else
			{
				Vector3 vtPos = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, m_csMainQuestManager.MainQuest.CompletionNpc.ContinentId);
				Player.DisplayPath.SetPath(vtPos);
				Player.SetWayPoint(vtPos, EnWayPointType.Move, 4f);
			}
		}

		public override void ArrivalMoveByTouchByManual(bool bMoveByTouchTarget)
		{
			if (m_csMainQuestManager.MainQuest == null) return;
			if (!bMoveByTouchTarget || Player.Target == null) return;

			if (m_csMyHeroInfo.Nation.NationId == m_csMyHeroInfo.InitEntranceLocationParam) // 본인 국가일때.
			{
				if (Player.Target.CompareTag("Npc"))
				{
					CsNpcInfo csNpcInfo = m_csMainQuestManager.MainQuest.CompletionNpc;
					if (csNpcInfo == null) return;

					if (CsIngameData.Instance.IngameManagement.GetNpcId(Player.Target) == csNpcInfo.NpcId) // 퀘스트 Npc와 현재 선택된 Npc가 같을때.
					{
						if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
						{
							Debug.Log("CsPlayThemeQuestMain.CsStateComplete.OnEventArrivalMoveByTouch");
							NpcDialog(csNpcInfo);
							m_csMainQuestManager.CompleteReadyOK();
						}
					}
				}
			}
		}

		public override void StateChangedToIdle()
		{
			Play();
		}
	}
}