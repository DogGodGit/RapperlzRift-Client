using SimpleDebugLog;
using UnityEngine;

public enum EnAutoQuestPlay { None, Main, Weekly, Dungeon, Farm, Hunter, MysteryBox, SecretLetter, HolyWar }
public class CsPlayThemeQuest : CsPlayTheme
{
	protected Vector3 m_vtQuestPos;
	protected float m_flQuestStopRange = 0f;

	#region IAutoPlay

	//---------------------------------------------------------------------------------------------------
	public override EnAutoMode GetType() { return EnAutoMode.Quest; }
	public override float GetTargetRange() { return 50.0f; }

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		Player.IsQuestDialog = false;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnEventContinentObjectInteractionStarted(long l)   // 오브젝트 상호작용 사작 후
	{
		if (Player.State == CsHero.EnState.Interaction)
		{
			dd.d("CsPlayThemeQuest.OnEventContinentObjectInteractionStarted");
			Player.ChangeState(CsHero.EnState.Interaction);
		}
	}

	#endregion Override

	//---------------------------------------------------------------------------------------------------
	public EnAutoQuestPlay GetQuestType()
	{
		return (EnAutoQuestPlay)GetTypeSub();
	}

	//---------------------------------------------------------------------------------------------------
	public void NpcDialog(CsNpcInfo csNpcInfo)
	{
		if (csNpcInfo == null) return;
		if (Player.IsTargetInDistance(csNpcInfo.Position, csNpcInfo.InteractionMaxRange))
		{
			Debug.Log("CsPlayThemeQuest.NpcDialog      NpcId = " + csNpcInfo.NpcId);
			Player.SelectTarget(Player.FindNpc(csNpcInfo.Position), true);
			Player.transform.LookAt(csNpcInfo.Position);
			Player.IsQuestDialog = true;
			CsIngameData.Instance.IngameManagement.NpcDialog(csNpcInfo.NpcId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected bool MovePlayer(int nNationId, int nContinentId, Vector3 vtPosition, float flRange, bool bTargetNpc)
	{
		if (Player.SkillStatus.IsStatusPlayAnim()) return false;

		if (nNationId == m_csMyHeroInfo.InitEntranceLocationParam && nContinentId == m_csMyHeroInfo.LocationId) // 대륙 및 국가 확인.
		{			
			if (Player.IsTargetInDistance(vtPosition, flRange))
			{
				return true;
			}

			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				if (Player.IsStateIdle)
				{
					dd.d("MovePlayer2", vtPosition, flRange);
					Player.MoveToPos(vtPosition, flRange, bTargetNpc);
				}
			}
		}
		else
		{
			if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
			{
				vtPosition = Player.ChaseContinent(m_csMyHeroInfo.Nation.NationId, nContinentId);
				if (Player.IsStateIdle)
				{
					if (Player.IsTargetInDistance(vtPosition, 2f) == false)
					{
						Player.MoveToPos(vtPosition, 2f, false);
					}
				}
			}
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void BattlePlayer(Vector3 vtPos, float flRadius)
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
			Player.PlayBattle();
		}
	}
}

