using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun25_TeamBattlefield : CsSceneIngameDungeon
{

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForTeamBattlefieldEnter += OnEventContinentExitForTeamBattlefieldEnter;
		m_csDungeonManager.EventTeamBattlefieldEnter += OnEventTeamBattlefieldEnter;
		m_csDungeonManager.EventTeamBattlefieldRevive += OnEventTeamBattlefieldRevive;

		m_csDungeonManager.EventTeamBattlefieldPlayWaitStart += OnEventTeamBattlefieldPlayWaitStart;

		m_csDungeonManager.TeamBattlefieldEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventContinentExitForTeamBattlefieldEnter -= OnEventContinentExitForTeamBattlefieldEnter;
		m_csDungeonManager.EventTeamBattlefieldEnter -= OnEventTeamBattlefieldEnter;
		m_csDungeonManager.EventTeamBattlefieldRevive -= OnEventTeamBattlefieldRevive;

		m_csDungeonManager.EventTeamBattlefieldPlayWaitStart -= OnEventTeamBattlefieldPlayWaitStart;

		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager = null;

		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForTeamBattlefieldEnter()
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHeros)
	{
		Debug.Log("OnEventTeamBattlefieldEnter            apDHeros.Length = " + aHeros.Length);
		SetMyHeroLocation(m_csDungeonManager.TeamBattlefield.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < aHeros.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(aHeros[i], false, false));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldPlayWaitStart(PDTeamBattlefieldMember[] pDTeamBattlefieldMember)
	{
		int nMyTeamNo = 0;
		int nMyIndex = 0;
		for (int i = 0; i < pDTeamBattlefieldMember.Length; i++)
		{

			
		}

		for (int i = 0; i < pDTeamBattlefieldMember.Length; i++)
		{
			if (nMyIndex == i)
			{
				continue;
			}
			else
			{

			}
		}

		// 팀 결정으로 인한 플레이어의 태그 및 HUD 변경
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldRevive(float nRevivalInvincibilityRemainingTime, PDVector3 pdVtPos, float flRot)
	{
		// 특정 지역으로 이동하여 부활
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonTeamBattlefield());
	}
}
