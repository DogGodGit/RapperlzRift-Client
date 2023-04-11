using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun18_DragonNest : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDragonNestEnter += OnEventDragonNestEnter;
		m_csDungeonManager.EventDragonNestRevive += OnEventDragonNestRevive;
		m_csDungeonManager.EventDragonNestStepStart += OnEventDragonNestStepStart;
		m_csDungeonManager.EventHeroDragonNestTrapHit += OnEventHeroDragonNestTrapHit;
		m_csDungeonManager.EventDragonNestTrapEffectFinished += OnEventDragonNestTrapEffectFinished;
		m_csDungeonManager.EventHeroDragonNestTrapEffectFinished += OnEventHeroDragonNestTrapEffectFinished;

		m_csDungeonManager.DragonNestEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventDragonNestEnter -= OnEventDragonNestEnter;
		m_csDungeonManager.EventDragonNestRevive -= OnEventDragonNestRevive;
		m_csDungeonManager.EventDragonNestStepStart -= OnEventDragonNestStepStart;
		m_csDungeonManager.EventHeroDragonNestTrapHit -= OnEventHeroDragonNestTrapHit;
		m_csDungeonManager.EventDragonNestTrapEffectFinished -= OnEventDragonNestTrapEffectFinished;
		m_csDungeonManager.EventHeroDragonNestTrapEffectFinished -= OnEventHeroDragonNestTrapEffectFinished;

		m_csDungeonManager.ResetDungeon();

		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance, Guid[] aTrapHeros)
	{
		Debug.Log("CsSceneDun18_DragonNest.OnEventDragonNestEnter");
		SetMyHeroLocation(m_csDungeonManager.DragonNest.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < aHero.Length; i++)
		{
			bool bTrap = false;
			for (int j = 0; j < aTrapHeros.Length; j++)
			{
				if (aHero[i].id == aTrapHeros[j])
				{
					bTrap = true;
					break;
				}
			}
			StartCoroutine(AsyncCreateHero(aHero[i], false, false,0, bTrap));
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		for (int i = 0; i < m_csDungeonManager.DragonNest.DragonNestObstacleList.Count; i++)
		{
			CsDragonNestObstacle Obstacle = m_csDungeonManager.DragonNest.DragonNestObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}

		CsDragonNestStep csDragonNestStep = m_csDungeonManager.DragonNestStep;
		if (csDragonNestStep != null)	// 스텝시작되어 있는 경우 장애물 제거.
		{
			RemoveObstacle(csDragonNestStep.RemoveObstacleId);

			if (csDragonNestStep.StepNo > 1)
			{
				csDragonNestStep = m_csDungeonManager.DragonNest.GetDragonNestStep(csDragonNestStep.StepNo - 1);
				if (csDragonNestStep != null)
				{
					RemoveObstacle(csDragonNestStep.RemoveObstacleId);

					if (csDragonNestStep.StepNo > 1)
					{
						csDragonNestStep = m_csDungeonManager.DragonNest.GetDragonNestStep(csDragonNestStep.StepNo - 1);
						if (csDragonNestStep != null)
						{
							RemoveObstacle(csDragonNestStep.RemoveObstacleId);
						}
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestRevive()
	{
		Debug.Log("CsSceneDun18_DragonNest.OnEventDragonNestRevive");
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestStepStart(PDDragonNestMonsterInstance[] aMonsterInstance)
	{
		Debug.Log("CsSceneDun18_DragonNest.OnEventDragonNestStepStart");

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			bool bBoss = aMonsterInstance[i].monsterType == 2 ? true : false; // 1. 일반몬스터  2. 보스몬스터
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i], bBoss));
		}

		if (m_csDungeonManager.DragonNestStep != null)
		{
			RemoveObstacle(m_csDungeonManager.DragonNestStep.RemoveObstacleId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDragonNestTrapHit(Guid guidHeroId, int nHp, int nDamage, int nHpDamage, long[] aRemovedAbnormalStateEffects, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		if (m_csPlayer.HeroId == guidHeroId)
		{
			m_csPlayer.NetTrapHit(nHp, nDamage, nHpDamage, aRemovedAbnormalStateEffects, true, m_csDungeonManager.DragonNest.TrapPenaltyMoveSpeed, apDAbnormalStateEffectDamageAbsorbShield);
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetTrapHit(nHp, nDamage, nHpDamage, aRemovedAbnormalStateEffects, true, m_csDungeonManager.DragonNest.TrapPenaltyMoveSpeed, apDAbnormalStateEffectDamageAbsorbShield);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestTrapEffectFinished()
	{
		m_csPlayer.NetTrapEffectFinished();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDragonNestTrapEffectFinished(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetTrapEffectFinished();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonDragonNest());
	}
}
