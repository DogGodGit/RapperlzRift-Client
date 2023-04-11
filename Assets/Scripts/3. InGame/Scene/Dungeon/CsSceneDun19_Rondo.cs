using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun19_Rondo : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;

		if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.Elite)
		{
			Debug.Log("EliteDungeon Enter!!!!!");
			m_csDungeonManager.EventContinentExitForEliteDungeonEnter += OnEventContinentExitForEliteDungeonEnter;
			m_csDungeonManager.EventEliteDungeonEnter += OnEventEliteDungeonEnter;
			m_csDungeonManager.EventEliteDungeonStart += OnEventEliteDungeonStart;
			m_csDungeonManager.EventEliteDungeonRevive += OnEventEliteDungeonRevive;
			m_csDungeonManager.EventEliteDungeonClear += OnEventEliteDungeonClear;
			m_csDungeonManager.SendEliteDungeonEnter();
		}
		else if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.ProofOfValor)
		{
			Debug.Log("ProofOfValors Enter!!!!!");			
			m_csDungeonManager.EventContinentExitForProofOfValorEnter += OnEventContinentExitForProofOfValorEnter;
			m_csDungeonManager.EventProofOfValorEnter += OnEventProofOfValorEnter;
			m_csDungeonManager.EventProofOfValorStart += OnEventProofOfValorStart;

			m_csDungeonManager.EventProofOfValorBuffBoxCreated += OnEventProofOfValorBuffBoxCreated;
			m_csDungeonManager.EventProofOfValorBuffBoxAcquire += OnEventProofOfValorBuffBoxAcquire;
			m_csDungeonManager.EventProofOfValorBuffBoxLifetimeEnded += OnEventProofOfValorBuffBoxLifetimeEnded;
			m_csDungeonManager.EventProofOfValorBuffFinished += OnEventProofOfValorBuffFinished;
			m_csDungeonManager.SendProofOfValorEnter();
		}

		string[] astr = { "BuffBox01", "BuffBox02", "BuffBox03" };
		StartCoroutine(StartLoadEffect(astr));
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.Elite)
		{
			m_csDungeonManager.EventContinentExitForEliteDungeonEnter -= OnEventContinentExitForEliteDungeonEnter;
			m_csDungeonManager.EventEliteDungeonEnter -= OnEventEliteDungeonEnter;
			m_csDungeonManager.EventEliteDungeonStart -= OnEventEliteDungeonStart;
			m_csDungeonManager.EventEliteDungeonRevive -= OnEventEliteDungeonRevive;
			m_csDungeonManager.EventEliteDungeonClear -= OnEventEliteDungeonClear;
		}
		else if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.ProofOfValor)
		{
			m_csDungeonManager.EventContinentExitForProofOfValorEnter -= OnEventContinentExitForProofOfValorEnter;
			m_csDungeonManager.EventProofOfValorEnter -= OnEventProofOfValorEnter;
			m_csDungeonManager.EventProofOfValorStart -= OnEventProofOfValorStart;

			m_csDungeonManager.EventProofOfValorBuffBoxCreated -= OnEventProofOfValorBuffBoxCreated;
			m_csDungeonManager.EventProofOfValorBuffBoxAcquire -= OnEventProofOfValorBuffBoxAcquire;
			m_csDungeonManager.EventProofOfValorBuffBoxLifetimeEnded -= OnEventProofOfValorBuffBoxLifetimeEnded;
			m_csDungeonManager.EventProofOfValorBuffFinished -= OnEventProofOfValorBuffFinished;
		}

		m_csDungeonManager.ResetDungeon();

		ClearBuffObject();
		m_listBuffArea = null;
		m_csDungeonManager = null;
		base.OnDestroy();
	}

	#region EliteDungeon

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForEliteDungeonEnter()
	{
		DungeonEnter();
	}
	
	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDEliteDungeonMonsterInstance[] apDEliteDungeonMonsterInstance)
	{
		Debug.Log("OnEventEliteDungeonEnter()  ");

		SetMyHeroLocation(m_csDungeonManager.EliteDungeon.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < apDEliteDungeonMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apDEliteDungeonMonsterInstance[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonStart()
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventStartEliteDungeon();
		}	
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonRevive()
	{
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonClear()
	{
		// 연출
	}

	#endregion EliteDungeon

	#region ProofOfValor

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForProofOfValorEnter()
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDMonsterInstance[] apDMonsterInstance)
	{
		SetMyHeroLocation(m_csDungeonManager.ProofOfValor.Location.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < apDMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apDMonsterInstance[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorStart()
	{
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventStartEliteDungeon();
		}
	}

	// 용맹의증명버프상자획득
	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorBuffBoxAcquire(int nRecoveryHp)
	{
		CsBuffBoxArea csBuffBoxArea = m_listBuffArea.Find(a => a.InstanceId == m_csDungeonManager.BuffBoxInstanceId);

		if (csBuffBoxArea != null)
		{
			string strName = "BuffBox0" + csBuffBoxArea.BuffBoxId.ToString();
			StartCoroutine(NormalEffect(strName, m_csPlayer.transform.parent, m_csPlayer.transform.position, m_csPlayer.transform.rotation, 5f));
			m_csPlayer.NetBuffBoxAcquire(csBuffBoxArea.BuffBoxId);
		}
		
		ClearBuffObject();
	}

	// 용맹의증명버프상자생성
	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorBuffBoxCreated(PDProofOfValorBuffBoxInstance[] apDProofOfValorBuffBoxInstance)
	{
		Transform trArea = transform.Find("Area");
		m_listBuffArea.Clear();
		for (int i = 0; i < trArea.childCount; i++)	// 초기화.
		{
			Destroy(trArea.GetChild(i).gameObject);
		}

		for (int i = 0; i < apDProofOfValorBuffBoxInstance.Length; i++)
		{
			CreateBuffObject(apDProofOfValorBuffBoxInstance[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorBuffBoxLifetimeEnded()
	{
		Debug.Log("OnEventProofOfValorBuffBoxLifetimeEnded");
		ClearBuffObject();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorBuffFinished()
	{
		Debug.Log("OnEventProofOfValorBuffFinished");
		m_csPlayer.NetBuffFinish();
	}

	#endregion ProofOfValor

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.Elite)
		{
			AddPlayTheme(new CsPlayThemeDungeonElite());
		}
		else if (m_csDungeonManager.DungeonPlay == EnDungeonPlay.ProofOfValor)
		{
			AddPlayTheme(new CsPlayThemeDungeonProofOfValor());
		}
	}
}
