using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun20_WarMemory : CsSceneIngameDungeon
{
	GameObject m_goWarMemoryTransformation = null;
	Dictionary<long, CsWarMemoryObject> m_dicWarMemoryObject = new Dictionary<long, CsWarMemoryObject>();

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventWarMemoryEnter += OnEventWarMemoryEnter;
		m_csDungeonManager.EventWarMemoryRevive += OnEventWarMemoryRevive;
		m_csDungeonManager.EventWarMemoryWaveStart += OnEventWarMemoryWaveStart;
		m_csDungeonManager.EventWarMemoryWaveCompleted += OnEventWarMemoryWaveCompleted;
		m_csDungeonManager.EventWarMemoryMonsterSummon += OnEventWarMemoryMonsterSummon;

		//My
		m_csDungeonManager.EventWarMemoryTransformationObjectLifetimeEnded += OnEventWarMemoryTransformationObjectLifetimeEnded;
		m_csDungeonManager.EventWarMemoryTransformationObjectInteractionFinished += OnEventWarMemoryTransformationObjectInteractionFinished;
		m_csDungeonManager.EventWarMemoryMonsterTransformationCancel += OnEventWarMemoryMonsterTransformationCancel;
		m_csDungeonManager.EventWarMemoryMonsterTransformationFinished += OnEventWarMemoryMonsterTransformationFinished;

		// Other
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionStart += OnEventHeroWarMemoryTransformationObjectInteractionStart;
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionCancel += OnEventHeroWarMemoryTransformationObjectInteractionCancel;
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionFinished += OnEventHeroWarMemoryTransformationObjectInteractionFinished;
		m_csDungeonManager.EventHeroWarMemoryMonsterTransformationCancel += OnEventHeroWarMemoryMonsterTransformationCancel;
		m_csDungeonManager.EventHeroWarMemoryMonsterTransformationFinished += OnEventHeroWarMemoryMonsterTransformationFinished;
		m_csDungeonManager.EventHeroWarMemoryTransformationMonsterSkillCast += OnEventHeroWarMemoryTransformationMonsterSkillCast;


		m_csDungeonManager.WarMemoryEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.EventWarMemoryEnter -= OnEventWarMemoryEnter;
		m_csDungeonManager.EventWarMemoryRevive -= OnEventWarMemoryRevive;
		m_csDungeonManager.EventWarMemoryWaveStart -= OnEventWarMemoryWaveStart;
		m_csDungeonManager.EventWarMemoryWaveCompleted -= OnEventWarMemoryWaveCompleted;
		m_csDungeonManager.EventWarMemoryMonsterSummon -= OnEventWarMemoryMonsterSummon;

		//My
		m_csDungeonManager.EventWarMemoryTransformationObjectLifetimeEnded -= OnEventWarMemoryTransformationObjectLifetimeEnded;
		m_csDungeonManager.EventWarMemoryTransformationObjectInteractionFinished -= OnEventWarMemoryTransformationObjectInteractionFinished;
		m_csDungeonManager.EventWarMemoryMonsterTransformationCancel -= OnEventWarMemoryMonsterTransformationCancel;
		m_csDungeonManager.EventWarMemoryMonsterTransformationFinished -= OnEventWarMemoryMonsterTransformationFinished;

		// Other
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionStart -= OnEventHeroWarMemoryTransformationObjectInteractionStart;
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionCancel -= OnEventHeroWarMemoryTransformationObjectInteractionCancel;
		m_csDungeonManager.EventHeroWarMemoryTransformationObjectInteractionFinished -= OnEventHeroWarMemoryTransformationObjectInteractionFinished;
		m_csDungeonManager.EventHeroWarMemoryMonsterTransformationCancel -= OnEventHeroWarMemoryMonsterTransformationCancel;
		m_csDungeonManager.EventHeroWarMemoryMonsterTransformationFinished -= OnEventHeroWarMemoryMonsterTransformationFinished;
		m_csDungeonManager.EventHeroWarMemoryTransformationMonsterSkillCast -= OnEventHeroWarMemoryTransformationMonsterSkillCast;

		m_csDungeonManager.ResetDungeon();

		CsIngameData.Instance.MyHeroDead = false;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHero, PDMonsterInstance[] aMonsterInstance, PDWarMemoryTransformationObjectInstance[] aWarMemoryTransformationObjectInstance)
	{
		SetMyHeroLocation(m_csDungeonManager.WarMemory.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < aHero.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(aHero[i], false, false));
		}

		if (m_csDungeonManager.WarMemoryWave != null)
		{
			Debug.Log("OnEventWarMemoryEnter      >>>>>     Create = "+ aMonsterInstance.Length+" , "+ aWarMemoryTransformationObjectInstance.Length);
			for (int i = 0; i < aMonsterInstance.Length; i++)
			{
				StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
			}

			for (int i = 0; i < aWarMemoryTransformationObjectInstance.Length; i++)
			{
				StartCoroutine(AsyncCreateWarMemoryObject(aWarMemoryTransformationObjectInstance[i]));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryRevive(PDVector3 pDVector3, float flRotationY)
	{
		Debug.Log("OnEventWarMemoryRevive");
		m_csPlayer.NetEventRevive(CsRplzSession.Translate(pDVector3), flRotationY);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryWaveStart(PDWarMemoryMonsterInstance[] aMonsterInstance, PDWarMemoryTransformationObjectInstance[] aWarMemoryTransformationObjectInstance)
	{
		Debug.Log("OnEventWarMemoryWaveStart");

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			bool bBoss = aMonsterInstance[i].monsterType == 2 ? true : false; // 1. 일반몬스터  2. 보스몬스터
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		if (m_csDungeonManager.WarMemoryWave != null)
		{
			for (int i = 0; i < aWarMemoryTransformationObjectInstance.Length; i++)
			{
				StartCoroutine(AsyncCreateWarMemoryObject(aWarMemoryTransformationObjectInstance[i]));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryWaveCompleted()
	{
		Debug.Log("OnEventWarMemoryWaveCompleted");
		ClearWarMemoryObject();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryMonsterSummon(PDWarMemorySummonMonsterInstance[] aMonsterInstance)
	{
		Debug.Log("OnEventWarMemoryMonsterSummon");

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}
	}

	// My
	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryTransformationObjectLifetimeEnded(long lInstanceId)
	{
		Debug.Log("OnEventWarMemoryTransformationObjectLifetimeEnded      lInstanceId = "+ lInstanceId);
		RemoveWarMemoryObject(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryTransformationObjectInteractionFinished(int nObjectId, long lInstanceId, long[] alRemovedAbnormalStateEffects)	// 상호작용 종료 변신시작.
	{
		Debug.Log("OnEventWarMemoryTransformationObjectInteractionFinished         lInstanceId = " + lInstanceId);

		if (m_dicWarMemoryObject.ContainsKey(lInstanceId))
		{
			m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.Monster, false, m_dicWarMemoryObject[lInstanceId].WarMemoryTransformationObject.TransformationMonster);
		}

		m_csPlayer.RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
		ClearWarMemoryObject();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryMonsterTransformationCancel(long[] alRemovedAbnormalStateEffects)	// 변신취소
	{
		Debug.Log("OnEventWarMemoryMonsterTransformationCancel");
		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
		m_csPlayer.RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryMonsterTransformationFinished(long[] alRemovedAbnormalStateEffects)	// 변신종료
	{
		Debug.Log("OnEventWarMemoryMonsterTransformationFinished");
		m_csPlayer.ChangeTransformationState(CsHero.EnTransformationState.None);
		m_csPlayer.RemoveAbnormalEffect(alRemovedAbnormalStateEffects);
	}

	// Other
	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryTransformationObjectInteractionStart(Guid guidHeroId, long lInstnaceId)
	{
		Debug.Log("OnEventHeroWarMemoryTransformationObjectInteractionStart");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			if (m_dicWarMemoryObject.ContainsKey(lInstnaceId))
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart(m_dicWarMemoryObject[lInstnaceId].transform.position, m_dicWarMemoryObject[lInstnaceId].InteractionMaxRange);
			}
			else
			{
				m_dicHeros[guidHeroId].NetEventInteractionStart();
			}
		}
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryTransformationObjectInteractionCancel(Guid guidHeroId, long lInstnaceId)
	{
		Debug.Log("OnEventHeroWarMemoryTransformationObjectInteractionCancel");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryTransformationObjectInteractionFinished(Guid guidHeroId, long lInstnaceId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("OnEventHeroWarMemoryTransformationObjectInteractionFinished");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			if (m_dicWarMemoryObject.ContainsKey(lInstnaceId))
			{
				m_dicHeros[guidHeroId].NetWarMemoryTransformationObjectInteractionFinished(m_dicWarMemoryObject[lInstnaceId].WarMemoryTransformationObject.TransformationMonster);
			}
		}
		ClearWarMemoryObject();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryMonsterTransformationCancel(Guid guidHeroId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("OnEventHeroWarMemoryMonsterTransformationCancel");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventWarMemoryMonsterTransformationFinished(nMaxHp, nHp, alRemovedAbnormalStateEffects);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryMonsterTransformationFinished(Guid guidHeroId, int nMaxHp, int nHp, long[] alRemovedAbnormalStateEffects)
	{
		Debug.Log("OnEventHeroWarMemoryMonsterTransformationFinished");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventWarMemoryMonsterTransformationFinished(nMaxHp, nHp, alRemovedAbnormalStateEffects);
		}		
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroWarMemoryTransformationMonsterSkillCast(Guid guidHeroId, int nSkillId, PDVector3 vtTargetPos)
	{
		Debug.Log("OnEventHeroWarMemoryTransformationMonsterSkillCast");
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventTransformationMonsterSkillCast(nSkillId, CsRplzSession.Translate(vtTargetPos));
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator AsyncCreateWarMemoryObject(PDWarMemoryTransformationObjectInstance pDWarMemoryTransformationObjectInstance)
	{
		if (pDWarMemoryTransformationObjectInstance != null)
		{
			CsWarMemoryTransformationObject csWarMemoryTransformationObject = m_csDungeonManager.WarMemoryWave.GetTransformationObject(pDWarMemoryTransformationObjectInstance.objectId);
			Debug.Log("CreateWarMemoryObject             csWarMemoryTransformationObject.ObjectPrefabName = " + csWarMemoryTransformationObject.ObjectPrefabName);
			if (pDWarMemoryTransformationObjectInstance != null)
			{
				if (m_dicWarMemoryObject.ContainsKey(pDWarMemoryTransformationObjectInstance.instanceId) == false)
				{
					GameObject goObject = null;
					if (m_goWarMemoryTransformation == null)
					{
						ResourceRequest req = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/InteractionObject/" + csWarMemoryTransformationObject.ObjectPrefabName);
						yield return req;

						m_goWarMemoryTransformation = Instantiate(req.asset, transform.Find("Object")) as GameObject;
						m_goWarMemoryTransformation.name = csWarMemoryTransformationObject.ObjectPrefabName;
						m_goWarMemoryTransformation.SetActive(false);

						yield return m_goWarMemoryTransformation;
					}

					goObject = Instantiate(m_goWarMemoryTransformation, transform.Find("Object")) as GameObject;

					if (goObject.GetComponent<CsInteractionObject>() != null)
					{
						Destroy(goObject.GetComponent<CsInteractionObject>());
					}

					goObject.AddComponent<CsWarMemoryObject>();
					CsWarMemoryObject csWarMemoryObject = goObject.GetComponent<CsWarMemoryObject>();
					if (csWarMemoryObject != null)
					{
						m_dicWarMemoryObject.Add(pDWarMemoryTransformationObjectInstance.instanceId, csWarMemoryObject);
						csWarMemoryObject.Init(pDWarMemoryTransformationObjectInstance.instanceId, CsRplzSession.Translate(pDWarMemoryTransformationObjectInstance.position), csWarMemoryTransformationObject);
						goObject.SetActive(true);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveWarMemoryObject(long lInstanceId)
	{
		if (m_dicWarMemoryObject.ContainsKey(lInstanceId))
		{
			Destroy(m_dicWarMemoryObject[lInstanceId].gameObject);
			m_dicWarMemoryObject.Remove(lInstanceId);
			RemoveObjectCheck();
		}
	}

	//----------------------------------------------------------------------------------------------------
	void ClearWarMemoryObject()
	{
		foreach (var dicWarMemoryObject in m_dicWarMemoryObject)
		{
			GameObject.Destroy(dicWarMemoryObject.Value.gameObject);
		}
		m_dicWarMemoryObject.Clear();
	}

	//---------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonWarMemory());
	}
}
