using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun23_AnkouTomb : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForAnkouTombEnter += OnEventContinentExitForAnkouTombEnter;
		m_csDungeonManager.EventAnkouTombEnter += OnEventAnkouTombEnter;
		m_csDungeonManager.EventAnkouTombWaveStart += OnEventAnkouTombWaveStart;
		m_csDungeonManager.EventAnkouTombRevive += OnEventAnkouTombRevive;
		m_csDungeonManager.EventAnkouTombClear += OnEventAnkouTombClear;
		m_csDungeonManager.EventAnkouTombFail += OnEventAnkouTombFail;

		m_csDungeonManager.AnkouTombEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventContinentExitForAnkouTombEnter -= OnEventContinentExitForAnkouTombEnter;
		m_csDungeonManager.EventAnkouTombEnter -= OnEventAnkouTombEnter;
		m_csDungeonManager.EventAnkouTombWaveStart -= OnEventAnkouTombWaveStart;
		m_csDungeonManager.EventAnkouTombRevive -= OnEventAnkouTombRevive;
		m_csDungeonManager.EventAnkouTombClear -= OnEventAnkouTombClear;
		m_csDungeonManager.EventAnkouTombFail -= OnEventAnkouTombFail;

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
			Debug.Log("CsSceneDun23_AnkouTomb.OnEventEvtHeroHit               파티원 HP 정보 변경");
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroLevelUp(SEBHeroLevelUpEventBody csEvt) // 당사자 외 레벨업. 
	{
		base.OnEventEvtHeroLevelUp(csEvt);
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("CsSceneDun23_AnkouTomb.OnEventEvtHeroLevelUp          파티원 레벨업.");
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
	void OnEventContinentExitForAnkouTombEnter()
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventAnkouTombEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
	{
		Debug.Log(" CsSceneDun23_AnkouTomb.OnEventAnkouTombEnter() ");
		SetMyHeroLocation(m_csDungeonManager.AnkouTomb.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		StartCoroutine(DirectingByDungeonEnter());

		for (int i = 0; i < pdHero.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(pdHero[i], false, false));
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombWaveStart(PDAnkouTombMonsterInstance[] apAnkouTombMonsterInstance, int nWaveNo)
	{		
		for (int i = 0; i < apAnkouTombMonsterInstance.Length; i++)
		{
			bool bBoss = apAnkouTombMonsterInstance[i].monsterType == 2 ? true : false;		//  1. 일반몬스터   2. 보스몬스터
			StartCoroutine(AsyncCreateMonster(apAnkouTombMonsterInstance[i], bBoss));

			if (bBoss)
			{
				Debug.Log(" CsSceneDun23_AnkouTomb.OnEventAnkouTombWaveStart()          CreateBossMonster");
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombRevive()
	{
		Debug.Log(" CsSceneDun23_AnkouTomb.OnEventAnkouTombRevive() ");
		m_csPlayer.NetEventRevive();
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
	{
		Debug.Log(" OnEventAnkouTombClear");
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
		StartCoroutine(DungeonClearDirection(new Vector3(-0.219f, 1.56f, 16.13f), 180));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombFail(bool bLevelUp, long lAcquiredExp)
	{
		Debug.Log(" OnEventAnkouTombFail() ");
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
	}

	//----------------------------------------------------------------------------------------------------
	//public override void StartClearDiretion()
	//{
	//	//if (m_bClearDiretion) return;

	//	//Debug.Log("##############  StartClearDiretion()  override ################ ");
	//	//m_bClearDiretion = true;
	//	//ClearHero();
	//	//StartCoroutine(DungeonClearDirection(new Vector3(-0.219f, 1.56f, 16.13f), 180));
	//}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonAnkouTomb());
	}
}