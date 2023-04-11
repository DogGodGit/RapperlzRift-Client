using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun17_InfiniteWar : CsSceneIngameDungeon
{	
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;

		m_csDungeonManager.EventInfiniteWarEnter += OnEventInfiniteWarEnter;
		m_csDungeonManager.EventInfiniteWarRevive += OnEventInfiniteWarRevive;
		m_csDungeonManager.EventInfiniteWarBuffBoxAcquire += OnEventInfiniteWarBuffBoxAcquire;

		m_csDungeonManager.EventInfiniteWarStart += OnEventInfiniteWarStart;
		m_csDungeonManager.EventInfiniteWarMonsterSpawn += OnEventInfiniteWarMonsterSpawn;

		m_csDungeonManager.EventInfiniteWarBuffBoxCreated += OnEventInfiniteWarBuffBoxCreated;
		m_csDungeonManager.EventHeroInfiniteWarBuffBoxAcquisition += OnEventHeroInfiniteWarBuffBoxAcquisition;
		m_csDungeonManager.EventInfiniteWarBuffBoxLifetimeEnded += OnEventInfiniteWarBuffBoxLifetimeEnded;
		m_csDungeonManager.EventInfiniteWarBuffFinished += OnEventInfiniteWarBuffFinished;
		m_csDungeonManager.InfiniteWarEnter();

		string[] astr = { "BuffBox01", "BuffBox02", "BuffBox03" };
		StartCoroutine(StartLoadEffect(astr));
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.EventInfiniteWarEnter -= OnEventInfiniteWarEnter;
		m_csDungeonManager.EventInfiniteWarRevive -= OnEventInfiniteWarRevive;
		m_csDungeonManager.EventInfiniteWarBuffBoxAcquire -= OnEventInfiniteWarBuffBoxAcquire;

		m_csDungeonManager.EventInfiniteWarStart -= OnEventInfiniteWarStart;
		m_csDungeonManager.EventInfiniteWarMonsterSpawn -= OnEventInfiniteWarMonsterSpawn;

		m_csDungeonManager.EventInfiniteWarBuffBoxCreated -= OnEventInfiniteWarBuffBoxCreated;
		m_csDungeonManager.EventHeroInfiniteWarBuffBoxAcquisition -= OnEventHeroInfiniteWarBuffBoxAcquisition;
		m_csDungeonManager.EventInfiniteWarBuffBoxLifetimeEnded -= OnEventInfiniteWarBuffBoxLifetimeEnded;
		m_csDungeonManager.EventInfiniteWarBuffFinished -= OnEventInfiniteWarBuffFinished;

		m_csDungeonManager.ResetDungeon();
		CsIngameData.Instance.MyHeroDead = false;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] aHeros, PDMonsterInstance[] aMonsterInstance, PDInfiniteWarBuffBoxInstance[] aBuffBoxInstance)
	{
		Debug.Log("OnEventInfiniteWarEnter            apDHeros.Length = " + aHeros.Length);
		SetMyHeroLocation(m_csDungeonManager.InfiniteWar.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < aHeros.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(aHeros[i], false, false));
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));			
		}

		for (int i = 0; i < aBuffBoxInstance.Length; i++)
		{
			CreateBuffObject(aBuffBoxInstance[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarRevive()
	{
		Debug.Log("OnEventInfiniteWarRevive");
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarBuffBoxAcquire(int nRecoveryHp)
	{
		Debug.Log("OnEventProofOfValorBuffBoxLifetimeEnded");
		CsBuffBoxArea csBuffBoxArea = m_listBuffArea.Find(a => a.InstanceId == m_csDungeonManager.BuffBoxInstanceId);
		
		if (csBuffBoxArea != null)
		{
			string strName = "BuffBox0" + csBuffBoxArea.BuffBoxId.ToString();
			StartCoroutine(NormalEffect(strName, m_csPlayer.transform.parent, m_csPlayer.transform.position, m_csPlayer.transform.rotation, 5f));
			m_csPlayer.NetBuffBoxAcquire(csBuffBoxArea.BuffBoxId);
		}

		RemoveBuffObject(m_csDungeonManager.BuffBoxInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarStart()
	{
		Debug.Log("OnEventInfiniteWarStart");

	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarMonsterSpawn(PDInfiniteWarMonsterInstance[] apDInfiniteWarMonsterInstance)
	{
		Debug.Log("OnEventInfiniteWarMonsterSpawn  apDInfiniteWarMonsterInstance = "+ apDInfiniteWarMonsterInstance.Length);
		for (int i = 0; i < apDInfiniteWarMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apDInfiniteWarMonsterInstance[i]));			
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarBuffBoxCreated(PDInfiniteWarBuffBoxInstance[] apDInfiniteWarBuffBoxInstance)
	{
		Debug.Log("OnEventProofOfValorBuffBoxCreated");

		ClearBuffObject();

		for (int i = 0; i < apDInfiniteWarBuffBoxInstance.Length; i++)
		{
			CreateBuffObject(apDInfiniteWarBuffBoxInstance[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroInfiniteWarBuffBoxAcquisition(Guid guidHeroId, int nHp, long lInstanceId)
	{
		Debug.Log("OnEventInfiniteWarBuffBoxLifetimeEnded  lInstanceId = " + lInstanceId);
		RemoveBuffObject(lInstanceId);

		if (m_csPlayer.HeroId == guidHeroId)
		{
			m_csPlayer.NetEventChangeHp(nHp);
		}
		else if (m_dicHeros.ContainsKey(guidHeroId))
		{
			m_dicHeros[guidHeroId].NetEventChangeHp(nHp);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarBuffBoxLifetimeEnded(long lInstanceId)
	{
		Debug.Log("OnEventInfiniteWarBuffBoxLifetimeEnded  lInstanceId = "+ lInstanceId);
		RemoveBuffObject(lInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarBuffFinished()
	{
		Debug.Log("OnEventInfiniteWarBuffFinished");
		m_csPlayer.NetBuffFinish();
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroEnter(SEBHeroEnterEventBody csEvt)
	{
		base.OnEventEvtHeroEnter(csEvt);
		m_csDungeonManager.SetInfiniteWarPoint(csEvt.hero.id, csEvt.hero.name, 0, 0);
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroExit(SEBHeroExitEventBody csEvt)
	{
		base.OnEventEvtHeroExit(csEvt);
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonInfiniteWar());
	}
}
