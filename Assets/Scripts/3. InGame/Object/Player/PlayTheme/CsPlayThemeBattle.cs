using SimpleDebugLog;
using UnityEngine;

public class CsPlayThemeBattle : CsPlayTheme
{
	Vector3 m_vtAutoStartPos; // 오토 시작 위치(저장용).

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override float GetTargetRange() // 자동 사냥 탐색 범위
	{
		if (CsIngameData.Instance.CombatRange == 0) // 제자리
		{
			return CsGameConfig.Instance.ShortDistanceBattleRange;
		}
		else if (CsIngameData.Instance.CombatRange == 1) // 현재화면
		{
			return CsGameConfig.Instance.StandingBattleRange;
		}
		else
		{
			return 100;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		Player.ChangeState(CsHero.EnState.Idle);
	}

	#endregion IAutoPlay


	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		CsGameEventToIngame.Instance.EventAutoBattleStart += OnEventAutoBattleChangeState;
		m_bUpdateOnMove = true;
		csPlayer.SetAutoBattlePlay(this);
		Debug.Log("CsPlayThemeBattle.Init()");
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		CsGameEventToIngame.Instance.EventAutoBattleStart -= OnEventAutoBattleChangeState;
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
		if (Player.Dead) return;

		if (CsIngameData.Instance.AutoBattleMode == EnBattleMode.None) return;

		if (Player.State == CsHero.EnState.MoveByJoystic) // || Player.State == CsHero.EnState.MoveByTouch)	// 터치이동 임시 제거.
		{
			m_vtAutoStartPos = Player.transform.position;
			return;
		}

		if (Player.SkillStatus.IsStatusPlayAnim() == false || Player.SkillStatus.IsCurrentChainable()) // 공격중이 아니거나 연계스킬 가능 상태일때.
		{
			if (Player.IsTargetEnemy == false)
			{
				if (Player.IsStateIdle) // find center pos, near center pos, return auto-start-pos
				{
					if (CsIngameData.Instance.CombatRange == 0) // AutoStartPos를 기준으로 가까운 타겟 찾기.
					{
						Player.SelectTargetEnemy(Player.FindBestTarget(m_vtAutoStartPos, m_vtAutoStartPos, GetTargetRange()), true);
					}
					else if (CsIngameData.Instance.CombatRange == 1) // AutoStartPos 를 기준 20m 이내 대상중 본인기준 가장 가까운 타겟 찾기.
					{
						Player.SelectTargetEnemy(NearAutoTypeFindTarget(), true);
					}
					else
					{
						m_vtAutoStartPos = Player.transform.position;
						Player.SelectTargetEnemy(Player.FindBestTarget(Player.transform.position, Player.transform.position, GetTargetRange()), true);
					}

					if (Player.ChangeTargetAutoToEnemy()) return;

					if (Player.IsTransformationStateTame()) return; // 테임중에는 제자리 복귀 처리 안함.

					if (Player.TargetEnemy == null)
					{
						if (CsIngameData.Instance.CombatRange != 2) //AutoType이 FarAround 아닐때. 오토 시작지점 복귀.
						{
							if (!Player.IsTargetInDistance(m_vtAutoStartPos, CsMyPlayer.c_flStopRange))
							{
								if (m_timer.CheckSetTimer())
								{
									Player.MoveToPosNear(m_vtAutoStartPos, CsMyPlayer.c_flStopRange);
								}
							}
						}
					}		
				}
			}
			else
			{
				if (Player.NavMeshCornerCount(Player.TargetEnemy.position) < 20)
				{
					if (Player.IsTransformationStateTame())
					{
						if (CsIngameData.Instance.TameMonster.IsAttack)
						{
							if (Player.IsStateIdle == false)
							{
								Player.ChangeState(CsHero.EnState.Idle);
							}
							return;
						}

						if (Player.IsTargetInDistance(Player.TargetEnemy.position, CsIngameData.Instance.TameMonster.GetCastRange()))
						{
							Player.LookAtPosition(Player.TargetEnemy.position);
							CsIngameData.Instance.TameMonster.PlayBattle(Player.TargetEnemy);
						}
						else
						{
							Player.MoveToPosNear(Player.TargetEnemy.position, CsIngameData.Instance.TameMonster.GetCastRange());
						}
					}
					else
					{
						Player.PlayBattle();
					}
				}
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
				Player.ResetBattleMode();				
			}
		}
	}

	#endregion Override

	#region Event
	//---------------------------------------------------------------------------------------------------
	void OnEventAutoBattleChangeState(EnBattleMode enBattleMode)
	{
		Debug.Log("OnEventAutoBattleChangeState     enBattleMode = " + enBattleMode);

		if (Player == null) return;
		if (Player.Dead)
		{
			Player.ResetBattleMode();
			return;
		}

		if (enBattleMode != EnBattleMode.None && Player.AutoPlay.GetType() == EnAutoMode.Move)	// 자동전투시작할때.
		{
			Debug.Log("OnEventAutoBattleChangeState   Start AutoBattle  >>   Stop AutoMove");
			CsIngameData.Instance.AutoPlayKey = 0;
			CsIngameData.Instance.AutoPlaySub = 0;
			Player.SetAutoPlay(null, true);
		}

		Player.ResetTarget();
		CsIngameData.Instance.AutoBattleMode = enBattleMode;

		if (CsIngameData.Instance.Directing) return; // 연출 중인경우.

		m_vtAutoStartPos = Player.transform.position;
	}

	#endregion Event

	//---------------------------------------------------------------------------------------------------
	Transform NearAutoTypeFindTarget()
	{
		return CsMyPlayer.FindTarget(m_vtAutoStartPos,
									 Player.transform.position,
		                             CsMonster.c_strLayer,
		                             CsMonster.c_strTag,
									 CsGameConfig.Instance.StandingBattleRange);
	}
}