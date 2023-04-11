using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun06_Trainee : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForExpDungeonEnter += OnEventContinentExitForExpDungeonEnter;
		m_csDungeonManager.EventExpDungeonEnter += OnEventExpDungeonEnter;
		m_csDungeonManager.EventExpDungeonWaveStart += OnEventExpDungeonWaveStart;
		m_csDungeonManager.EventExpDungeonRevive += OnEventExpDungeonRevive;
		m_csDungeonManager.EventExpDungeonWaveTimeout += OnEventExpDungeonTimeout;

		m_csDungeonManager.SendExpDungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonEnter();
		m_csDungeonManager.EventContinentExitForExpDungeonEnter -= OnEventContinentExitForExpDungeonEnter;
		m_csDungeonManager.EventExpDungeonEnter -= OnEventExpDungeonEnter;
		m_csDungeonManager.EventExpDungeonWaveStart -= OnEventExpDungeonWaveStart;
		m_csDungeonManager.EventExpDungeonRevive -= OnEventExpDungeonRevive;
		m_csDungeonManager.EventExpDungeonWaveTimeout -= OnEventExpDungeonTimeout;
		m_csDungeonManager.ResetDungeon();

		CsIngameData.Instance.MyHeroDead = false;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForExpDungeonEnter() // 던전 입장. 탈것 초기화.
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExpDungeonEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId)
	{
		Debug.Log("CsSceneDun06_Trainee.OnEventExpDungeonEnter()");
		SetMyHeroLocation(m_csDungeonManager.ExpDungeon.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonWaveStart(PDExpDungeonMonsterInstance[] apDExpDungeonMonsterInstance, PDExpDungeonLakChargeMonsterInstance apDExpDungeonLakChargeMonsterInstance)
	{
		Debug.Log("CsSceneDun06_Trainee.OnEventExpDungeonWaveStart()");
		for (int i = 0; i < apDExpDungeonMonsterInstance.Length; i++) // Monster Create
		{
			StartCoroutine(AsyncCreateMonster(apDExpDungeonMonsterInstance[i]));
		}

		if (apDExpDungeonLakChargeMonsterInstance != null)
		{
			StartCoroutine(AsyncCreateMonster(apDExpDungeonLakChargeMonsterInstance, false, true));
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonRevive()
    {
		Debug.Log("CsSceneDun06_Trainee.OnEventExpDungeonRevive()");
		m_csPlayer.NetEventRevive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpDungeonTimeout()
    {
		Debug.Log("CsSceneDun06_Trainee.OnEventExpDungeonTimeout()");
		ClearMonster();
    }

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonExp());
	}
}