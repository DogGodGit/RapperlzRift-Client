using ClientCommon;
using SimpleDebugLog;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun15_RuinsReclaim : CsSceneIngameDungeon
{
	GameObject m_goRewardObject = null;
	GameObject m_goCancelObject = null;
	Dictionary<long, CsRuinsReclaimObject> m_dicRuinsReclaimObject = new Dictionary<long, CsRuinsReclaimObject>();
	Dictionary<int, GameObject> m_dicRuinsReclaimTrap = new Dictionary<int, GameObject>();

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;

		m_csDungeonManager.EventRuinsReclaimEnter += OnEventRuinsReclaimEnter;
		m_csDungeonManager.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
		m_csDungeonManager.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		m_csDungeonManager.EventRuinsReclaimPortalEnter += OnEventRuinsReclaimPortalEnter;
		m_csDungeonManager.EventRuinsReclaimRevive += OnEventRuinsReclaimRevive;

		m_csDungeonManager.EventRuinsReclaimStepStart += OnEventRuinsReclaimStepStart;
		m_csDungeonManager.EventRuinsReclaimWaveStart += OnEventRuinsReclaimWaveStart;
		m_csDungeonManager.EventRuinsReclaimWaveCompleted += OnEventRuinsReclaimWaveCompleted;
		m_csDungeonManager.EventRuinsReclaimStepCompleted += OnEventRuinsReclaimStepCompleted;

		m_csDungeonManager.EventRuinsReclaimStepWaveSkillCast += OnEventRuinsReclaimStepWaveSkillCast;
		m_csDungeonManager.EventRuinsReclaimMonsterSummon += OnEventRuinsReclaimMonsterSummon;
		m_csDungeonManager.EventRuinsReclaimTrapHit += OnEventRuinsReclaimTrapHit;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStart += OnEventRuinsReclaimDebuffEffectStart;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStop += OnEventRuinsReclaimDebuffEffectStop;

		m_csDungeonManager.EventRuinsReclaimClear += OnEventRuinsReclaimClear;
		m_csDungeonManager.EventRuinsReclaimFail += OnEventRuinsReclaimFail;

		// My
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationStart += OnEventRuinsReclaimMonsterTransformationStart;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationFinished += OnEventRuinsReclaimMonsterTransformationFinished;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded += OnEventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionFinished += OnEventRuinsReclaimRewardObjectInteractionFinished;

		// Other
		m_csDungeonManager.EventHeroRuinsReclaimPortalEnter += OnEventHeroRuinsReclaimPortalEnter;

		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationStart += OnEventHeroRuinsReclaimMonsterTransformationStart;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationFinished += OnEventHeroRuinsReclaimMonsterTransformationFinished;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart += OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel += OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;

		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionStart += OnEventHeroRuinsReclaimRewardObjectInteractionStart;
		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionCancel += OnEventHeroRuinsReclaimRewardObjectInteractionCancel;
		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionFinished += OnEventHeroRuinsReclaimRewardObjectInteractionFinished;

		m_csDungeonManager.RuinsReclaimEnter();
		FindDebuffArea();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventRuinsReclaimEnter -= OnEventRuinsReclaimEnter;
		m_csDungeonManager.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;
		m_csDungeonManager.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
		m_csDungeonManager.EventRuinsReclaimPortalEnter -= OnEventRuinsReclaimPortalEnter;
		m_csDungeonManager.EventRuinsReclaimRevive -= OnEventRuinsReclaimRevive;

		m_csDungeonManager.EventRuinsReclaimStepStart -= OnEventRuinsReclaimStepStart;
		m_csDungeonManager.EventRuinsReclaimWaveStart -= OnEventRuinsReclaimWaveStart;
		m_csDungeonManager.EventRuinsReclaimWaveCompleted -= OnEventRuinsReclaimWaveCompleted;
		m_csDungeonManager.EventRuinsReclaimStepCompleted -= OnEventRuinsReclaimStepCompleted;

		m_csDungeonManager.EventRuinsReclaimStepWaveSkillCast -= OnEventRuinsReclaimStepWaveSkillCast;
		m_csDungeonManager.EventRuinsReclaimMonsterSummon -= OnEventRuinsReclaimMonsterSummon;
		m_csDungeonManager.EventRuinsReclaimTrapHit -= OnEventRuinsReclaimTrapHit;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStart -= OnEventRuinsReclaimDebuffEffectStart;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStop -= OnEventRuinsReclaimDebuffEffectStop;


		m_csDungeonManager.EventRuinsReclaimClear -= OnEventRuinsReclaimClear;
		m_csDungeonManager.EventRuinsReclaimFail -= OnEventRuinsReclaimFail;

		// My
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationStart -= OnEventRuinsReclaimMonsterTransformationStart;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationFinished -= OnEventRuinsReclaimMonsterTransformationFinished;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded -= OnEventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished -= OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionFinished -= OnEventRuinsReclaimRewardObjectInteractionFinished;

		// Other		
		m_csDungeonManager.EventHeroRuinsReclaimPortalEnter -= OnEventHeroRuinsReclaimPortalEnter;

		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationStart -= OnEventHeroRuinsReclaimMonsterTransformationStart;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationFinished -= OnEventHeroRuinsReclaimMonsterTransformationFinished;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart -= OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel -= OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		m_csDungeonManager.EventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished -= OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;

		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionStart -= OnEventHeroRuinsReclaimRewardObjectInteractionStart;
		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionCancel -= OnEventHeroRuinsReclaimRewardObjectInteractionCancel;
		m_csDungeonManager.EventHeroRuinsReclaimRewardObjectInteractionFinished -= OnEventHeroRuinsReclaimRewardObjectInteractionFinished;

		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager = null;
		ClearRuinsReclaimObject();
		base.OnDestroy();
	}

	Transform m_trDebuffArea;
	//---------------------------------------------------------------------------------------------------
	void FindDebuffArea()
	{
		m_trDebuffArea = transform.Find("DebuffArea");
		m_trDebuffArea.gameObject.SetActive(false);
		m_trDebuffArea.position = new Vector3(m_csDungeonManager.RuinsReclaim.DebuffAreaXPosition, m_csDungeonManager.RuinsReclaim.DebuffAreaYPosition, m_csDungeonManager.RuinsReclaim.DebuffAreaZPosition);
		m_trDebuffArea.eulerAngles = new Vector3(0, m_csDungeonManager.RuinsReclaim.DebuffAreaYRotation, 0);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance,
								  PDRuinsReclaimRewardObjectInstance[] aRewardObjectInstance, PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aCancelObjectInstance, Guid[] aTransformationHeroes)
	{
		SetMyHeroLocation(m_csDungeonManager.RuinsReclaim.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < aHero.Length; i++)													// OtherHero Create
		{
			StartCoroutine(AsyncCreateHero(aHero[i], false, false));
		}

		for (int i = 0; i < m_csDungeonManager.RuinsReclaim.RuinsReclaimObstacleList.Count; i++)	// 장애물 생성.
		{
			CsRuinsReclaimObstacle Obstacle = m_csDungeonManager.RuinsReclaim.RuinsReclaimObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		if (m_csDungeonManager.RuinsReclaimStep != null)
		{
			RemoveObstacle(m_csDungeonManager.RuinsReclaimStep.RemoveObstacleId);                                                                       // 장애물 제거.
			if (m_csDungeonManager.RuinsReclaimStep.StepNo > 1)
			{
				CsRuinsReclaimStep csRuinsReclaimStep = m_csDungeonManager.RuinsReclaim.GetRuinsReclaimStep(m_csDungeonManager.RuinsReclaimStep.StepNo - 1);
				if (csRuinsReclaimStep != null)
				{
					RemoveObstacle(csRuinsReclaimStep.RemoveObstacleId);
					if (csRuinsReclaimStep.StepNo > 1)
					{
						csRuinsReclaimStep = m_csDungeonManager.RuinsReclaim.GetRuinsReclaimStep(m_csDungeonManager.RuinsReclaimStep.StepNo - 1);
						if (csRuinsReclaimStep != null)
						{
							RemoveObstacle(csRuinsReclaimStep.RemoveObstacleId);
						}
					}
				}
			}

			StartCoroutine(CreatePotal(m_csDungeonManager.RuinsReclaim.GetRuinsReclaimPortal(m_csDungeonManager.RuinsReclaimStep.ActivationPortalId))); // 포탈 생성.

			for (int i = 0; i < aRewardObjectInstance.Length; i++)
			{
				CsRuinsReclaimObjectArrange csRuinsReclaimObjectArrange = m_csDungeonManager.RuinsReclaimStep.GetRuinsReclaimObjectArrange(aRewardObjectInstance[i].arrangeNo);
				StartCoroutine(CreateRewardObject(csRuinsReclaimObjectArrange, aRewardObjectInstance[i].instanceId, CsRplzSession.Translate(aRewardObjectInstance[i].position)));
			}

			for (int i = 0; i < aCancelObjectInstance.Length; i++)
			{
				for (int j = 0; j < m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList.Count; j++)
				{
					CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill = m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList[i];
					StartCoroutine(CreateCancelObject(csRuinsReclaimStepWaveSkill, aCancelObjectInstance[i].instanceId, CsRplzSession.Translate(aCancelObjectInstance[i].position)));
				}
			}

			for (int i = 0; i < aTransformationHeroes.Length; i++)
			{
				if (m_dicHeros.ContainsKey(aTransformationHeroes[i]))
				{
					for (int j = 0; j < m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList.Count; j++)
					{
						CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill = m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList[i];
						CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(csRuinsReclaimStepWaveSkill.TransformationMonsterId);
						m_dicHeros[aTransformationHeroes[i]].ChangeTransformationState(CsHero.EnTransformationState.Monster, false, csMonsterInfo);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nContinentId)
	{
		DungeonExit();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nContinentId)
	{
		DungeonExit();
	}
	
	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimPortalEnter(PDVector3 pDVector3, float flRotationY)
	{
		m_csPlayer.NetEventPortalEnter(CsRplzSession.Translate(pDVector3), flRotationY);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRevive(PDVector3 pDVector3, float flRotationY)
	{
		m_csPlayer.NetEventRevive(CsRplzSession.Translate(pDVector3), flRotationY);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepStart(PDRuinsReclaimRewardObjectInstance[] apDRuinsReclaimRewardObjectInstance)
	{
		if (m_csDungeonManager.RuinsReclaimStep != null)
		{			
			ClearRuinsReclaimObject();                                                                                                                  // 모든 상호작용 오브젝트 제거.
			RemoveObstacle(m_csDungeonManager.RuinsReclaimStep.RemoveObstacleId);                                                                       // 장애물 제거.
			RemovePortal(m_csDungeonManager.RuinsReclaimStep.DeactivationPortalId);                                                                     // 포탈 제거.
			StartCoroutine(CreatePotal(m_csDungeonManager.RuinsReclaim.GetRuinsReclaimPortal(m_csDungeonManager.RuinsReclaimStep.ActivationPortalId))); // 포탈 생성.

			if (m_csDungeonManager.RuinsReclaimStep.Type == 2) // 상호작용.
			{
				for (int i = 0; i < apDRuinsReclaimRewardObjectInstance.Length; i++)
				{
					CsRuinsReclaimObjectArrange csRuinsReclaimObjectArrange = m_csDungeonManager.RuinsReclaimStep.GetRuinsReclaimObjectArrange(apDRuinsReclaimRewardObjectInstance[i].arrangeNo);
					StartCoroutine(CreateRewardObject(csRuinsReclaimObjectArrange, apDRuinsReclaimRewardObjectInstance[i].instanceId, CsRplzSession.Translate(apDRuinsReclaimRewardObjectInstance[i].position)));
				}
			}

			if (m_trDebuffArea == null)
			{
				FindDebuffArea();
			}

			if (m_csDungeonManager.RuinsReclaimStep.StepNo == m_csDungeonManager.RuinsReclaim.DebuffAreaActivationStepNo)
			{
				if (m_trDebuffArea != null)
				{
					m_trDebuffArea.gameObject.SetActive(true);
				}
			}
			else if (m_csDungeonManager.RuinsReclaimStep.StepNo == m_csDungeonManager.RuinsReclaim.DebuffAreaDeactivationStepNo)
			{
				if (m_trDebuffArea != null)
				{
					m_trDebuffArea.gameObject.SetActive(false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimWaveStart(PDRuinsReclaimMonsterInstance[] apDRuinsReclaimMonsterInstance)
	{
		for (int i = 0; i < apDRuinsReclaimMonsterInstance.Length; i++)
		{
			bool bBoss = apDRuinsReclaimMonsterInstance[i].monsterType == 2 ? true : false;	// 1. 일반몬스터  2. 보스몬스터
			StartCoroutine(AsyncCreateMonster(apDRuinsReclaimMonsterInstance[i], bBoss));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimWaveCompleted()
	{
		ClearRuinsReclaimObject();  // 상호작용 관련 오브젝트 삭제처리.

		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.ChangeTransformationState(CsHero.EnTransformationState.None);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepCompleted(PDItemBooty[] apDItemBooty)
	{
		ClearRuinsReclaimObject();  // 상호작용 관련 오브젝트 삭제처리.

		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
		foreach (var dicHeros in m_dicHeros)
		{
			dicHeros.Value.ChangeTransformationState(CsHero.EnTransformationState.None);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepWaveSkillCast(PDRuinsReclaimMonsterTransformationCancelObjectInstance[] aCancelObjectInstance, PDVector3 pDVector3)
	{
		if (m_csDungeonManager.RuinsReclaimStep != null)
		{
			if (m_csDungeonManager.RuinsReclaimStep.Type == 3) // 웨이브 처치.
			{
				for (int i = 0; i < aCancelObjectInstance.Length; i++)
				{					
					for (int j = 0; j < m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList.Count; j++)
					{
						CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill = m_csDungeonManager.RuinsReclaimStep.RuinsReclaimStepWaveSkillList[j];
						StartCoroutine(CreateCancelObject(csRuinsReclaimStepWaveSkill, aCancelObjectInstance[i].instanceId, CsRplzSession.Translate(aCancelObjectInstance[i].position)));
					}
				}
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterSummon(PDRuinsReclaimSummonMonsterInstance[] aSummonMonsterInstance)
	{
		Debug.Log("OnEventRuinsReclaimMonsterSummon");
		if (aSummonMonsterInstance != null)
		{
			for (int i = 0; i < aSummonMonsterInstance.Length; i++)
			{
				StartCoroutine(AsyncCreateMonster(aSummonMonsterInstance[i]));
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimTrapHit(Guid guidHeroId, int nHP, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		Debug.Log("OnEventRuinsReclaimTrapHit");
		if (m_csPlayer.HeroId == guidHeroId)
		{
			m_csPlayer.NetTrapHit(nHP, nDamage, nHpDamage, alRemovedAbnormalStateEffects, false, 0, apDAbnormalStateEffectDamageAbsorbShield);
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetTrapHit(nHP, nDamage, nHpDamage, alRemovedAbnormalStateEffects, false, 0, apDAbnormalStateEffectDamageAbsorbShield);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimDebuffEffectStart()	// 기획에서 공격력감소 디버프 관련 효과 이팩트 결정하면 적용. 없으면 필요 없음.
	{
		Debug.Log("OnEventRuinsReclaimDebuffEffectStart");
		//csPlayer
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimDebuffEffectStop()    // 기획에서 공격력감소 디버프 관련 효과 이팩트 결정하면 적용. 없으면 필요 없음.
	{
		Debug.Log("OnOnEventRuinsReclaimDebuffEffectStop");
		//csPlayer
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimClear(PDItemBooty[] aPDItemBooty, PDItemBooty randomBooty,
		Guid monsterTerminatorHeroId, string monsterTerminatorHeroName, PDItemBooty monsterTerminatorBooty,
		Guid ultimateAttackKingHeroId, string ultimateAttackKingHeroName, PDItemBooty ultimateAttackKingBooty,
		Guid partyVolunteerHeroId, string partyVolunteerHeroName, PDItemBooty partyVolunteerBooty)
	{
		StartCoroutine(DelayRemove());
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimFail()
	{
		StartCoroutine(DelayRemove());
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationStart(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		m_csPlayer.NetEventTransformationMonster(nTransformationMonsterId, m_csPlayer.MaxHp, m_csPlayer.Hp, alRemovedAbnormalStateEffects);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationFinished()
	{
		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectLifetimeEnded(long lInstanceId)
	{
		RemoveRuinsReclaimObject(lInstanceId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(long lInstanceId)
	{
		RemoveRuinsReclaimObject(lInstanceId);
		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRewardObjectInteractionFinished(PDItemBooty booty, long lInstanceId)
	{
		RemoveRuinsReclaimObject(lInstanceId);
	}

	#region Other

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimPortalEnter(Guid guidHeroId, PDVector3 pDVector3, float flRotationY)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventPortalEnter(CsRplzSession.Translate(pDVector3), flRotationY);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimMonsterTransformationStart(Guid guidHeroId, int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonster(nTransformationMonsterId, m_dicHeros[guidHeroId].MaxHp, m_dicHeros[guidHeroId].Hp, alRemovedAbnormalStateEffects);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimMonsterTransformationFinished(Guid guidHeroId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].ChangeTransformationState(CsHero.EnTransformationState.None);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionStart(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			if (m_dicRuinsReclaimObject.ContainsKey(lInstanceId))
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart(m_dicRuinsReclaimObject[lInstanceId].transform.position, m_dicRuinsReclaimObject[lInstanceId].InteractionMaxRange);
			}
			else
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart(); ;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionCancel(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventRuinsReclaimInteractionFinished();
		}

		RemoveRuinsReclaimObject(lInstanceId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimRewardObjectInteractionStart(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			if (m_dicRuinsReclaimObject.ContainsKey(lInstanceId))
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart(m_dicRuinsReclaimObject[lInstanceId].transform.position, m_dicRuinsReclaimObject[lInstanceId].InteractionMaxRange);
			}
			else
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart(); ;
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimRewardObjectInteractionCancel(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventHeroRuinsReclaimRewardObjectInteractionFinished	(Guid guidHeroId, long lInstanceId)
	{
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionFinished();
		}
		RemoveRuinsReclaimObject(lInstanceId);
	}

	#endregion Other

	//----------------------------------------------------------------------------------------------------
	IEnumerator DelayRemove()
	{
		yield return new WaitForSeconds(2f);

		ClearMonster();
	}

	//---------------------------------------------------------------------------------------------------
	protected IEnumerator CreatePotal(CsRuinsReclaimPortal csRuinsReclaimPortal)
	{
		Debug.Log("CreatePotal   csRuinsReclaimPortal = "+ csRuinsReclaimPortal);
		if (csRuinsReclaimPortal != null)
		{
			Debug.Log("CreatePotal  csRuinsReclaimPortal = " + csRuinsReclaimPortal.PortalId);
			Transform trPortal = transform.Find("Portal");
			ResourceRequest resourceRequest = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Portal");
			yield return resourceRequest;
			yield return new WaitForSeconds(1f);

			GameObject goPortal = Instantiate(resourceRequest.asset, trPortal) as GameObject;
			CsPortalArea csPortalArea = goPortal.GetComponent<CsPortalArea>();
			csPortalArea.Init(csRuinsReclaimPortal);
			m_dicPortal.Add(csRuinsReclaimPortal.PortalId, csPortalArea);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CreateRewardObject(CsRuinsReclaimObjectArrange csRuinsReclaimObjectArrange, long lInstanceId, Vector3 vtPos)
	{
		Debug.Log("CreateRewardObject     lInstanceId = "+ lInstanceId);
		if (m_dicRuinsReclaimObject.ContainsKey(lInstanceId) == false)
		{
			GameObject goObject = null;
			if (m_goRewardObject == null)
			{
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/InteractionObject/" + csRuinsReclaimObjectArrange.PrefabName);

				yield return req;

				m_goRewardObject = Instantiate(req.asset, transform.Find("Object")) as GameObject;
				m_goRewardObject.name = csRuinsReclaimObjectArrange.PrefabName;
				m_goRewardObject.SetActive(false);

				yield return m_goRewardObject;

				goObject = Instantiate(m_goRewardObject, transform.Find("Object")) as GameObject;
			}
			else
			{
				goObject = Instantiate(m_goRewardObject, transform.Find("Object")) as GameObject;
			}

			if (goObject.GetComponent<CsInteractionObject>() != null)
			{
				Destroy(goObject.GetComponent<CsInteractionObject>());
			}

			goObject.name = lInstanceId.ToString();
			goObject.AddComponent<CsRuinsReclaimObject>();
			CsRuinsReclaimObject csRuinsReclaimObject = goObject.GetComponent<CsRuinsReclaimObject>();

			m_dicRuinsReclaimObject.Add(lInstanceId, csRuinsReclaimObject);
			csRuinsReclaimObject.RewardObjectInit(lInstanceId, vtPos, csRuinsReclaimObjectArrange);
			goObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CreateCancelObject(CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill, long lInstanceId, Vector3 vtPos)
	{
		Debug.Log("CreateCancelObject     lInstanceId = " + lInstanceId);
		if (m_dicRuinsReclaimObject.ContainsKey(lInstanceId) == false)
		{
			GameObject goObject = null;
			if (m_goCancelObject == null)
			{
				ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/InteractionObject/" + csRuinsReclaimStepWaveSkill.ObjectPrefabName);
				yield return req;

				m_goCancelObject = Instantiate(req.asset, transform.Find("Object")) as GameObject;
				m_goCancelObject.name = csRuinsReclaimStepWaveSkill.ObjectPrefabName;
				m_goCancelObject.SetActive(false);

				yield return m_goCancelObject;

				goObject = Instantiate(m_goCancelObject, transform.Find("Object")) as GameObject;
			}
			else
			{
				goObject = Instantiate(m_goCancelObject, transform.Find("Object")) as GameObject;
			}
			
			if (goObject.GetComponent<CsInteractionObject>() != null)
			{
				Destroy(goObject.GetComponent<CsInteractionObject>());
			}

			goObject.AddComponent<CsRuinsReclaimObject>();
			CsRuinsReclaimObject csRuinsReclaimObject = goObject.GetComponent<CsRuinsReclaimObject>();

			m_dicRuinsReclaimObject.Add(lInstanceId, csRuinsReclaimObject);
			csRuinsReclaimObject.CancelObjectInit(lInstanceId, vtPos, csRuinsReclaimStepWaveSkill);
			goObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveRuinsReclaimObject(long lInstanceId)
	{
		if (m_dicRuinsReclaimObject.ContainsKey(lInstanceId))
		{
			Destroy(m_dicRuinsReclaimObject[lInstanceId].gameObject);
			m_dicRuinsReclaimObject.Remove(lInstanceId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void ClearRuinsReclaimObject()
	{
		foreach (var dicRuinsReclaimObject in m_dicRuinsReclaimObject)
		{
			GameObject.Destroy(dicRuinsReclaimObject.Value.gameObject);
		}
		m_dicRuinsReclaimObject.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonRuinsReclaim());
	}
}
