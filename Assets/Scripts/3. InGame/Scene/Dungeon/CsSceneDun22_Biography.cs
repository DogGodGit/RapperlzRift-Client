using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun22_Biography : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;

		m_csDungeonManager.EventBiographyQuestDungeonEnter += OnEventBiographyQuestDungeonEnter;
		m_csDungeonManager.EventBiographyQuestDungeonWaveStart += OnEventBiographyQuestDungeonWaveStart;
		m_csDungeonManager.EventBiographyQuestDungeonRevive += OnEventBiographyQuestDungeonRevive;

		m_csDungeonManager.BiographyQuestDungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.ResetDungeon();

		m_csDungeonManager.EventBiographyQuestDungeonEnter -= OnEventBiographyQuestDungeonEnter;
		m_csDungeonManager.EventBiographyQuestDungeonWaveStart -= OnEventBiographyQuestDungeonWaveStart;
		m_csDungeonManager.EventBiographyQuestDungeonRevive -= OnEventBiographyQuestDungeonRevive;

		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY)
	{
		Debug.Log("CsSceneDun22_Biography.OnEventBiographyQuestDungeonEnter()   BiographyQuestDungeon.LocationId = " + m_csDungeonManager.BiographyQuestDungeon.Location.LocationId);
		SetMyHeroLocation(m_csDungeonManager.BiographyQuestDungeon.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonWaveStart(PDBiographyQuestDungeonMonsterInstance[] pDMonsters)
	{
		Debug.Log("CsSceneDun22_Biography.OnEventBiographyQuestDungeonWaveStart()  pDMonsters.Length = " + pDMonsters.Length);
		for (int i = 0; i < pDMonsters.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(pDMonsters[i], false, false));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyQuestDungeonRevive()
	{
		Debug.Log("CsSceneDun22_Biography.OnEventBiographyQuestDungeonRevive()");
		m_csPlayer.NetEventRevive();
	}
}
