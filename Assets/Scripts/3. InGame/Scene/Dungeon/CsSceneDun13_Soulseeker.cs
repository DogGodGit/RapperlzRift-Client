using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun13_Soulseeker : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForSoulCoveterEnter += OnEventContinentExitForSoulCoveterEnter;
		m_csDungeonManager.EventSoulCoveterEnter += OnEventSoulCoveterEnter;
		m_csDungeonManager.EventSoulCoveterWaveStart += OnEventSoulCoveterWaveStart;
		m_csDungeonManager.EventSoulCoveterRevive += OnEventSoulCoveterRevive;
		m_csDungeonManager.EventSoulCoveterFail += OnEventSoulCoveterFail;
	
		m_csDungeonManager.SendSoulCoveterEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventContinentExitForSoulCoveterEnter -= OnEventContinentExitForSoulCoveterEnter;
		m_csDungeonManager.EventSoulCoveterEnter -= OnEventSoulCoveterEnter;
		m_csDungeonManager.EventSoulCoveterWaveStart -= OnEventSoulCoveterWaveStart;
		m_csDungeonManager.EventSoulCoveterRevive -= OnEventSoulCoveterRevive;
		m_csDungeonManager.EventSoulCoveterFail -= OnEventSoulCoveterFail;
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
			Debug.Log("CsSceneDun13_Soulseeker.OnEventEvtHeroHit               파티원 HP 정보 변경");
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroLevelUp(SEBHeroLevelUpEventBody csEvt) // 당사자 외 레벨업. 
	{
		base.OnEventEvtHeroLevelUp(csEvt);
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("CsSceneDun13_Soulseeker.OnEventEvtHeroLevelUp          파티원 레벨업.");
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
	void OnEventContinentExitForSoulCoveterEnter()
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance)
	{
		Debug.Log(" CsSceneDun13_Soulseeker.OnEventSoulCoveterEnter() ");
		SetMyHeroLocation(m_csDungeonManager.SoulCoveter.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < pdHero.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(pdHero[i], false, false));
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		for (int i = 0; i < m_csDungeonManager.SoulCoveter.SoulCoveterObstacleList.Count; i++)
		{
			CsSoulCoveterObstacle Obstacle = m_csDungeonManager.SoulCoveter.SoulCoveterObstacleList[i];
			CreateObstacle(Obstacle.ObstacleId, new Vector3(Obstacle.XPosition, Obstacle.YPosition, Obstacle.ZPosition), Obstacle.YRotation, Obstacle.Scale);
		}

		if (m_csDungeonManager.SoulCoveterDifficultyWave != null)
		{
			for (int i = 0; i < CsGameData.Instance.SoulCoveter.SoulCoveterObstacleList.Count; i++)
			{
				RemoveObstacle(CsGameData.Instance.SoulCoveter.SoulCoveterObstacleList[i].ObstacleId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterWaveStart(PDSoulCoveterMonsterInstance[] apSoulCoveterMonsterInstance)
	{
		Debug.Log(" CsSceneDun13_Soulseeker.OnEventSoulCoveterWaveStart() ");
		for (int i = 0; i < apSoulCoveterMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apSoulCoveterMonsterInstance[i]));
		}

		if (m_csDungeonManager.SoulCoveterDifficultyWave.WaveNo == 1)
		{
			for (int i = 0; i < CsGameData.Instance.SoulCoveter.SoulCoveterObstacleList.Count; i++)
			{
				RemoveObstacle(CsGameData.Instance.SoulCoveter.SoulCoveterObstacleList[i].ObstacleId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterRevive()
	{
		Debug.Log(" CsSceneDun13_Soulseeker.OnEventSoulCoveterRevive() ");
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterFail()
	{
		Debug.Log(" OnEventSoulCoveterFail() ");
		ClearMonster();
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonSoulCoveter());
	}
}
