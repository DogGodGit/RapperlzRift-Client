using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun11_Betrayal : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForAncientRelicEnter += OnEventContinentExitForAncientRelicEnter;
		m_csDungeonManager.EventAncientRelicEnter += OnEventAncientRelicEnter;
		m_csDungeonManager.EventAncientRelicStepStart += OnEventAncientRelicStepStart;
		m_csDungeonManager.EventAncientRelicWaveStart += OnEventAncientRelicWaveStart;
		m_csDungeonManager.EventAncientRelicStepCompleted += OnEventAncientRelicStepCompleted;
		m_csDungeonManager.EventAncientRelicRevive += OnEventAncientRelicRevive;
		m_csDungeonManager.EventAncientRelicFail += OnEventAncientRelicFail;

		m_csDungeonManager.EventAncientRelicTrapActivated += OnEventAncientRelicTrapActivated;
		m_csDungeonManager.EventAncientRelicTrapHit += OnEventAncientRelicTrapHit;
		m_csDungeonManager.EventAncientRelicTrapEffectFinished += OnEventAncientRelicTrapEffectFinished;

		m_csDungeonManager.SendAncientRelicEnter();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.EventContinentExitForAncientRelicEnter -= OnEventContinentExitForAncientRelicEnter;
		m_csDungeonManager.EventAncientRelicEnter -= OnEventAncientRelicEnter;
		m_csDungeonManager.EventAncientRelicStepStart -= OnEventAncientRelicStepStart;
		m_csDungeonManager.EventAncientRelicWaveStart -= OnEventAncientRelicWaveStart;
		m_csDungeonManager.EventAncientRelicStepCompleted -= OnEventAncientRelicStepCompleted;
		m_csDungeonManager.EventAncientRelicRevive -= OnEventAncientRelicRevive;
		m_csDungeonManager.EventAncientRelicFail -= OnEventAncientRelicFail;

		m_csDungeonManager.EventAncientRelicTrapActivated -= OnEventAncientRelicTrapActivated;
		m_csDungeonManager.EventAncientRelicTrapHit -= OnEventAncientRelicTrapHit;
		m_csDungeonManager.EventAncientRelicTrapEffectFinished -= OnEventAncientRelicTrapEffectFinished;
		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager = null;
		
		base.OnDestroy();
    }

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroHit(SEBHeroHitEventBody csEvt)
	{
		base.OnEventEvtHeroHit(csEvt);
		if (csEvt.heroId != m_csPlayer.HeroId) // 타유저 피격시 정보 갱신.
		{
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroLevelUp(SEBHeroLevelUpEventBody csEvt) // 당사자 외 레벨업. 
	{
		base.OnEventEvtHeroLevelUp(csEvt);
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroExit(SEBHeroExitEventBody csEvt)
	{
		base.OnEventEvtHeroExit(csEvt);
		m_csDungeonManager.OnEventPartyDungeonHeroExit(csEvt.heroId);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForAncientRelicEnter()
    {
        DungeonEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, Guid[] guidTrapEffectHeroes,int[] aRemoveObstacle)
    {
        Debug.Log(" OnEventAncientRelicEnter () ");

		SetMyHeroLocation(m_csDungeonManager.AncientRelic.LocationId);
        SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
        for (int i = 0; i < pdHero.Length; i++)
        {
			bool bTrap = false;
			for (int j = 0; j < guidTrapEffectHeroes.Length; j++)
			{
				if (pdHero[i].id == guidTrapEffectHeroes[j])
				{
					bTrap = true;
					break;
				}
			}
			
			StartCoroutine(AsyncCreateHero(pdHero[i], false, false, 0, bTrap));
        }

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		for (int i = 0; i < m_csDungeonManager.AncientRelic.AncientRelicObstacleList.Count; i++)
		{
			CsAncientRelicObstacle Obstacle = m_csDungeonManager.AncientRelic.AncientRelicObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}
		
		for (int i = 0; i < aRemoveObstacle.Length; i++)	// 삭제할 장애물.
		{
			RemoveObstacle(aRemoveObstacle[i]);
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicStepStart(int nRemoveObstacleId, Vector3 vtTargetPosition, float flTargetRadius)
    {
        Debug.Log(" OnEventAncientRelicStepStart () ");
		Debug.Log("SceneDun  nRemoveObstacleId   =   " + nRemoveObstacleId);
		RemoveObstacle(nRemoveObstacleId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicWaveStart(PDAncientRelicMonsterInstance[] apAncientRelicMonsterInstance)
    {
        Debug.Log(" OnEventAncientRelicWaveStart() ");
        for (int i = 0; i < apAncientRelicMonsterInstance.Length; i++)
        {
            StartCoroutine(AsyncCreateMonster(apAncientRelicMonsterInstance[i]));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicStepCompleted(PDItemBooty[] pdItemBooty)
    {
        Debug.Log(" OnEventAncientRelicStepCompleted() ");
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicRevive()
    {
        Debug.Log(" OnEventAncientRelicRevive() ");
        m_csPlayer.NetEventRevive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicFail()
    {
        Debug.Log(" OnEventAncientRelicFail() ");
        ClearMonster();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicTrapActivated(int nTrapid)
    {
        //Debug.Log(" OnEventAncientRelicTrapActivated()  nTrapid = "+ nTrapid);
        //CreateTrap(nTrapid);
    }
    
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicTrapHit(Guid guidHeroId, int nHp, int nDamage, int nHpDamage, long[] lRemoveAbnormalStateEffect, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		Debug.Log(" OnEventAncientRelicTrapHit() ");
		if (CsGameData.Instance.MyHeroInfo.HeroId == guidHeroId)
		{
			m_csPlayer.NetTrapHit(nHp,nDamage,nHpDamage,lRemoveAbnormalStateEffect, true, m_csDungeonManager.AncientRelic.TrapPenaltyMoveSpeed, apDAbnormalStateEffectDamageAbsorbShield);
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetTrapHit(nHp, nDamage, nHpDamage, lRemoveAbnormalStateEffect, true, m_csDungeonManager.AncientRelic.TrapPenaltyMoveSpeed, apDAbnormalStateEffectDamageAbsorbShield);
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicTrapEffectFinished(Guid guidHeroId)
    {
        Debug.Log(" OnEventAncientRelicTrapEffectFinished() ");
		if (CsGameData.Instance.MyHeroInfo.HeroId == guidHeroId)
		{
			m_csPlayer.NetTrapEffectFinished();
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetTrapEffectFinished();
		}
    }

  //  //---------------------------------------------------------------------------------------------------
  //  void CreateTrap(int nTrapId)
  //  {
  //      StartCoroutine(ActiveTrap(nTrapId));
  //  }

  //  //---------------------------------------------------------------------------------------------------
  //  IEnumerator ActiveTrap(int nTrapId)
  //  {
  //      List <CsAncientRelicTrap> listCsAncientRelicTrap = new List<CsAncientRelicTrap>();
		//for (int i = 0; i < m_csDungeonManager.AncientRelic.AncientRelicTrapList.Count; i++)
  //      {
		//	if (m_csDungeonManager.AncientRelic.AncientRelicTrapList[i].TrapId == nTrapId)
  //          {
		//		listCsAncientRelicTrap.Add(m_csDungeonManager.AncientRelic.AncientRelicTrapList[i]);
		//		// 오브젝트 생성필요
  //          }
  //      }
  //      yield return listCsAncientRelicTrap[0].Duration;
  //      // 오브젝트 destroy필요
  //      listCsAncientRelicTrap.Clear();
  //  }

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonAncientRelic());
	}
}
