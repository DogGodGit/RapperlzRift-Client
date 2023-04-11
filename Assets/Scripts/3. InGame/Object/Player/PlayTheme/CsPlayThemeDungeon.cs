using UnityEngine;

public class CsPlayThemeDungeon : CsPlayTheme
{
	protected CsDungeonManager m_csDungeonManager;
	protected bool m_bDungeonStart = false;

	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------

	#endregion IAutoPlay
	//---------------------------------------------------------------------------------------------------
	protected void DugeonBattle(Vector3 vtPos, float flRadius)
	{
		if (Player.TargetEnemy == null)
		{
			if (!Player.ChangeTargetAutoToEnemy())
			{
				Player.SelectTargetEnemy(Player.FindBestTarget(vtPos, Player.transform.position, flRadius), true);
			}
		}
		else
		{
			if (Player.IsTransformationStateTame())	// 테이밍중.
			{
				if (CsIngameData.Instance.TameMonster.IsAttack)
				{
					if (Player.IsStateIdle == false)
					{
						Player.ChangeState(CsHero.EnState.Idle);
					}
					return;
				}

				Player.LookAtPosition(Player.TargetEnemy.position);

				if (Player.IsTargetInDistance(Player.TargetEnemy.position, CsIngameData.Instance.TameMonster.GetCastRange()))
				{
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

	//---------------------------------------------------------------------------------------------------
	protected bool MovePlayer(Vector3 vtPosition, float flRange, bool bTargetNpc = false)
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
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void ExitDungeon()
	{
		Player.ResetBattleMode();
		if (IsThisAutoPlaying())
		{
			Debug.Log("ExitDungeon()");
			Player.SetAutoPlay(null, true);
		}
	}
}
