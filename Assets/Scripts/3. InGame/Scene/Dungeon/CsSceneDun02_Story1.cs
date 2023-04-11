using ClientCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsSceneDun02_Story1 : CsSceneIngameDungeon
{
	CsMonster m_csMonster;
	CsStoryDungeonDifficulty m_csStoryDungeonDifficulty;

	Dictionary<int, CsTrapObject> m_dicTrap = new Dictionary<int, CsTrapObject>();
																								 
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForStoryDungeonEnter += OnEventContinentExitForStoryDungeonEnter;
		m_csDungeonManager.EventStoryDungeonEnter += OnEventStoryDungeonEnter;
		m_csDungeonManager.EventStoryDungeonStepStart += OnEventStoryDungeonStepStart;
		m_csDungeonManager.EventStoryDungeonRevive += OnEventStoryDungeonRevive;
		m_csDungeonManager.EventStoryDungeonMonsterTame += OnEventStoryDungeonMonsterTame;
		m_csDungeonManager.EventStoryDungeonRemoveTaming += OnEventStoryDungeonRemoveTaming;
		m_csDungeonManager.EventStoryDungeonTrapCast +=OnEventStoryDungeonTrapCast;
		m_csDungeonManager.EventStoryDungeonTrapHit += OnEventStoryDungeonTrapHit;
		m_csDungeonManager.EventStoryDungeonClear += OnEventStoryDungeonClear;


		CsGameEventToIngame.Instance.EventBossAppearSkip += OnEventBossAppearSkip;

		m_csDungeonManager.SendStoryDungeonEnter();


		for (int i = 0; i < m_csDungeonManager.StoryDungeon.StoryDungeonObstacleList.Count; i++)
		{
			CsStoryDungeonObstacle Obstacle = m_csDungeonManager.StoryDungeon.StoryDungeonObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}

		m_csStoryDungeonDifficulty = m_csDungeonManager.StoryDungeon.GetStoryDungeonDifficulty( m_csDungeonManager.StoryDungeonStep.Difficulty);
		for (int j = 0; j < m_csStoryDungeonDifficulty.StoryDungeonTrapList.Count; j++)
		{
			CreateTrap(m_csStoryDungeonDifficulty.StoryDungeonTrapList[j]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		CsIngameData.Instance.InGameCamera.ResetCamera();

		m_csDungeonManager.EventContinentExitForStoryDungeonEnter -= OnEventContinentExitForStoryDungeonEnter;
		m_csDungeonManager.EventStoryDungeonEnter -= OnEventStoryDungeonEnter;
		m_csDungeonManager.EventStoryDungeonStepStart -= OnEventStoryDungeonStepStart;
		m_csDungeonManager.EventStoryDungeonRevive -= OnEventStoryDungeonRevive;
		m_csDungeonManager.EventStoryDungeonMonsterTame -= OnEventStoryDungeonMonsterTame;
		m_csDungeonManager.EventStoryDungeonRemoveTaming -= OnEventStoryDungeonRemoveTaming;
		m_csDungeonManager.EventStoryDungeonTrapCast -= OnEventStoryDungeonTrapCast;
		m_csDungeonManager.EventStoryDungeonTrapHit -= OnEventStoryDungeonTrapHit;
		m_csDungeonManager.EventStoryDungeonClear -= OnEventStoryDungeonClear;

		m_csDungeonManager.ResetDungeon();

		CsGameEventToIngame.Instance.EventBossAppearSkip -= OnEventBossAppearSkip;
		m_csDungeonManager = null;
		m_csStoryDungeonDifficulty = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForStoryDungeonEnter() // 던전 입장. 탈것 초기화.
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
	{
		SetMyHeroLocation(m_csDungeonManager.StoryDungeon.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
		StartCoroutine(DirectingByDungeonEnter());
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonStepStart(PDStoryDungeonMonsterInstance[] apDStoryDungeonMonsterInstance)
	{
		int nLastStepNo = m_csDungeonManager.StoryDungeon.GetStoryDungeonDifficulty(m_csDungeonManager.StoryDungeonStep.Difficulty).StoryDungeonStepList.Count + 1;
		Debug.Log("OnEventStoryDungeonStepStart           >>>>>>>>>         CreateObjects       nLastStepNo = " + nLastStepNo + " // Step = " + m_csDungeonManager.StoryDungeonStep.Step);

		for (int i = 0; i < apDStoryDungeonMonsterInstance.Length; i++) // Monster Create
		{
			m_csMonster = CreateMonster(apDStoryDungeonMonsterInstance[i]);

			if (apDStoryDungeonMonsterInstance[i].monsterType == 2)		// 보스 몬스터인경우.
			{
				if (m_csDungeonManager.StoryDungeonStep.Difficulty == 3)
				{
					StartCoroutine(BossAppearanceDirection(m_csMonster)); 	//보스 연출 
				}
			}

			m_csMonster.NetEventSetStoryDungeonMoneter(apDStoryDungeonMonsterInstance[i].monsterType); // 몬스터 타입 설정.
		}

		if (m_csDungeonManager.StoryDungeonStep != null)
        {
			RemoveObstacle(m_csDungeonManager.StoryDungeonStep.RemoveObstacleId);
        }
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventStoryDungeonRevive()
    {
		m_csPlayer.NetEventRevive();
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonMonsterTame()
	{
		m_csPlayer.NetEventTameGetOn();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonRemoveTaming()
	{
		m_csPlayer.NetEventTameGetOff();
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonTrapCast(int nTrapId)
	{
		if (m_dicTrap.ContainsKey(nTrapId))
		{
			StartCoroutine(FireTrap(m_dicTrap[nTrapId]));
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator FireTrap(CsTrapObject csTrapObject)
	{
		csTrapObject.ReadyTrap();
		yield return new WaitForSeconds(csTrapObject.StoryDungeonTrap.CastingStartDelay);
		csTrapObject.StartTrap(true);
		yield return new WaitForSeconds(csTrapObject.StoryDungeonTrap.CastingDuration);
		csTrapObject.StartTrap(false);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonTrapHit(int nHp, int nDamage, int nHpDamage, long[] lremovedAbnormalStateEffects, PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{		
		m_csPlayer.NetTrapHit(nHp, nDamage, nHpDamage, lremovedAbnormalStateEffects, false, 0, apDAbnormalStateEffectDamageAbsorbShield);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventStoryDungeonClear(PDItemBooty[] apDItemBooty)
	{
		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
		StartCoroutine(DungeonClearDirection(new Vector3(353.3f, 1f, -298.1f), -90f, 2.0f));
	}

	//----------------------------------------------------------------------------------------------------
	void CreateTrap(CsStoryDungeonTrap csStoryDungeonTrap)
	{
		if (m_dicTrap.ContainsKey(csStoryDungeonTrap.TrapId) == false)
		{
			GameObject goTrap = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/TrapObject/" + csStoryDungeonTrap.PrefabName), transform.Find("Object"));			
			CsTrapObject csTrapObject = goTrap.GetComponent<CsTrapObject>();
			csTrapObject.Init(csStoryDungeonTrap);

			m_dicTrap.Add(csStoryDungeonTrap.TrapId, csTrapObject);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventBossAppearSkip()
	{
		StopCoroutine(BossAppearanceDirection(m_csMonster));
		CsIngameData.Instance.Directing = false;
		m_csMonster.BossAppearSkip();

		CsIngameData.Instance.InGameDungeonCamera.ChangeDungeonCameraState(EnCameraPlay.Normal);
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonStory());
	}
}