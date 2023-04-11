using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun21_FearAltar : CsSceneIngameDungeon
{
	CsMonster m_csHalidomMonster = null;

	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventClickFearAltarTargetHalidomMonsterTargetButton += OnEventClickFearAltarTargetHalidomMonsterTargetButton;

		m_csDungeonManager.EventFearAltarEnter += OnEventFearAltarEnter;
		m_csDungeonManager.EventFearAltarWaveStart += OnEventFearAltarWaveStart;
		m_csDungeonManager.EventFearAltarRevive += OnEventFearAltarRevive;

		m_csDungeonManager.FearAltarEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager.EventClickFearAltarTargetHalidomMonsterTargetButton -= OnEventClickFearAltarTargetHalidomMonsterTargetButton;

		m_csDungeonManager.EventFearAltarEnter -= OnEventFearAltarEnter;
		m_csDungeonManager.EventFearAltarWaveStart -= OnEventFearAltarWaveStart;
		m_csDungeonManager.EventFearAltarRevive -= OnEventFearAltarRevive;

		m_csHalidomMonster = null;
		ClearBuffObject();		
		m_listBuffArea = null;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClickFearAltarTargetHalidomMonsterTargetButton()
	{
		Debug.Log("CsSceneDun21_FearAltar.OnEventClickFearAltarTargetHalidomMonsterTargetButton() m_csHalidomMonster = "+ m_csHalidomMonster);
		if (m_csHalidomMonster != null)
		{
			m_csPlayer.NetEventSelectMonster(m_csHalidomMonster.transform);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] apDHeros, PDMonsterInstance[] apDMonsterInstance)
	{
		Debug.Log("CsSceneDun21_FearAltar.OnEventFearAltarEnter()   m_csDungeonManager.FearAltarStage.Location.LocationId = "+ m_csDungeonManager.FearAltarStage.Location.LocationId);
		SetMyHeroLocation(m_csDungeonManager.FearAltarStage.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < apDHeros.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(apDHeros[i], false, false));
		}

		for (int i = 0; i < apDMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apDMonsterInstance[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarWaveStart(PDFearAltarMonsterInstance[] pDMonsters, PDFearAltarHalidomMonsterInstance pDHalidomMonster)
	{
		Debug.Log("CsSceneDun21_FearAltar.OnEventFearAltarWaveStart()  pDMonsters.Length = "+ pDMonsters.Length);
		for (int i = 0; i < pDMonsters.Length; i++)
		{
			bool bBoss = (pDMonsters[i].monsterType == 2) ? true : false;
			StartCoroutine(AsyncCreateMonster(pDMonsters[i], bBoss, false));
		}

		m_csHalidomMonster = CreateMonster(pDHalidomMonster);
		if (m_csHalidomMonster != null)
		{
			m_csHalidomMonster.NetEventSetFearAltarHalidomMonster(pDHalidomMonster.halidomId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarRevive()
	{
		Debug.Log("CsSceneDun21_FearAltar.OnEventFearAltarRevive()");
		m_csPlayer.NetEventRevive();
	}

	//----------------------------------------------------------------------------------------------------
	void RemoveHalidomMonster()
	{
		if (m_csHalidomMonster != null)
		{
			Destroy(m_csHalidomMonster.gameObject);
			m_csHalidomMonster = null;
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonFearAltar());
	}
}
