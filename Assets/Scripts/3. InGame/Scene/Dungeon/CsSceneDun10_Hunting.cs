using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun10_Hunting : CsSceneIngameDungeon 
{
    //---------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForUndergroundMazeEnter += OnEventContinentExitForUndergroundMazeEnter; // 지하미로입장을위한대륙퇴장
		m_csDungeonManager.EventUndergroundMazeEnter += OnEventUndergroundMazeEnter; // 지하미로입장
		m_csDungeonManager.EventUndergroundMazeRevive += OnEventUndergroundMazeRevive; // 지하미로부활
		m_csDungeonManager.EventUndergroundMazeEnterForUndergroundMazeRevive += OnEventUndergroundMazeEnterForUndergroundMazeRevive; // 부활에대한지하미로입장
		m_csDungeonManager.EventMyHeroUndergroundMazePortalEnter += OnEventMyHeroUndergroundMazePortalEnter; // 지하미로포탈입장.
		m_csDungeonManager.EventUndergroundMazePortalExit += OnEventUndergroundMazePortalExit; // 지하미로포탈퇴장
		m_csDungeonManager.EventUndergroundMazeEnterForTransmission += OnEventUndergroundMazeEnterForTransmission; // 전송에대한지하미로입장

		m_csDungeonManager.DungeonPlay = EnDungeonPlay.UndergroundMaze;

		switch (m_csDungeonManager.DungeonEnterType) // 구분 필요.
		{
			case EnDungeonEnterType.InitEnter:
				if (CsGameData.Instance.MyHeroInfo.LocationId == m_csDungeonManager.UndergroundMaze.LocationId)  // 최초 던전 입장 Enter
				{
					m_csDungeonManager.InitializeUndergroundMaze(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam); // 지하미로 층 값 세팅.
					SendHeroInitEnterCommand();
				}
				else
				{
					m_csDungeonManager.SendUndergroundMazeEnter(); // 대륙에서 던전 입장 Enter
				}
				break;
			case EnDungeonEnterType.Portal:
				m_csDungeonManager.SendUndergroundMazePortalExit(); // 포탈 입장 Enter
				break;
			case EnDungeonEnterType.Transmission:
				m_csDungeonManager.SendUndergroundMazeEnterForTransmission(); // 전송 입장 Enter
				break;
			case EnDungeonEnterType.Revival:
				m_csDungeonManager.SendUndergroundMazeEnterForUndergroundMazeRevive(); // 부활 입장 Enter
				break;
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		m_csDungeonManager.EventContinentExitForUndergroundMazeEnter -= OnEventContinentExitForUndergroundMazeEnter;
		m_csDungeonManager.EventUndergroundMazeEnter -= OnEventUndergroundMazeEnter;
		m_csDungeonManager.EventUndergroundMazeRevive -= OnEventUndergroundMazeRevive;
		m_csDungeonManager.EventUndergroundMazeEnterForUndergroundMazeRevive -= OnEventUndergroundMazeEnterForUndergroundMazeRevive;
		m_csDungeonManager.EventMyHeroUndergroundMazePortalEnter -= OnEventMyHeroUndergroundMazePortalEnter;
		m_csDungeonManager.EventUndergroundMazePortalExit -= OnEventUndergroundMazePortalExit;
		m_csDungeonManager.EventUndergroundMazeEnterForTransmission -= OnEventUndergroundMazeEnterForTransmission;
		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager = null;
        base.OnDestroy();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventContinentExitForUndergroundMazeEnter() // 던전 입장. 탈것 초기화.
	{
		DungeonEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventResHeroInitEnter(int nReturnCode, HeroInitEnterResponseBody csRes) // 기본 입장.
	{
		if (nReturnCode == 0)
		{
			SetMyHeroLocation(m_csDungeonManager.UndergroundMaze.LocationId);
			SetUndergroundMazeEnterEnter(csRes.position, csRes.rotationY, csRes.placeInstanceId, csRes.heroes, csRes.monsterInsts);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pDHeros, PDUndergroundMazeMonsterInstance[] pDMonsterInsts)
	{
		SetMyHeroLocation(m_csDungeonManager.UndergroundMaze.LocationId);
		SetUndergroundMazeEnterEnter(pDVector3, flRotationY, guidPlaceInstanceId, pDHeros, pDMonsterInsts);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroUndergroundMazePortalEnter()
	{
		m_csPlayer.NetPortalEnter(0);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazePortalExit(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pDHeros, PDUndergroundMazeMonsterInstance[] pDMonsterInsts)
	{
		SetUndergroundMazeEnterEnter(pDVector3, flRotationY, guidPlaceInstanceId, pDHeros, pDMonsterInsts);
	}

    //---------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeRevive()
    {
        m_csPlayer.NetEventRevive();
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeEnterForUndergroundMazeRevive(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pDHeros, PDUndergroundMazeMonsterInstance[] pDMonsterInsts)
	{
		SetUndergroundMazeEnterEnter(pDVector3, flRotationY, guidPlaceInstanceId, pDHeros, pDMonsterInsts);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeEnterForTransmission(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pDHeros, PDUndergroundMazeMonsterInstance[] pDMonsterInsts)
	{
		SetUndergroundMazeEnterEnter(pDVector3, flRotationY, guidPlaceInstanceId, pDHeros, pDMonsterInsts);
	}

	//----------------------------------------------------------------------------------------------------
	void SetUndergroundMazeEnterEnter(PDVector3 pDVector3, float flRotationY, Guid guidPlaceInstanceId, PDHero[] pDHero, PDMonsterInstance[] pDMonsterInstance)
	{
		Debug.Log("SetEnter()    Name = " + CsGameData.Instance.MyHeroInfo.Name);
		SetMyHeroLocation(m_csDungeonManager.UndergroundMaze.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false); // MyHero Create

		for (int i = 0; i < pDHero.Length; i++) // OtherHero Create
		{
			if (pDHero[i].id == CsGameData.Instance.MyHeroInfo.HeroId)
			{
 				// 부활입장 때 중복 생성 방지
				continue;
			}
			StartCoroutine(AsyncCreateHero(pDHero[i], false, false));
		}

		for (int i = 0; i < pDMonsterInstance.Length; i++) // Monster Create
		{
			StartCoroutine(AsyncCreateMonster(pDMonsterInstance[i]));
		}

		for (int i = 0; i < m_csDungeonManager.UndergroundMazeFloor.UndergroundMazeNpcList.Count; i++) // NPC Create
		{
			CreateUndergroundMazeNpc(m_csDungeonManager.UndergroundMazeFloor.UndergroundMazeNpcList[i]);
		}

		for (int i = 0; i < m_csDungeonManager.UndergroundMazeFloor.UndergroundMazePortalList.Count; i++)
		{
			CreateUndergroundMazePotal(m_csDungeonManager.UndergroundMazeFloor.UndergroundMazePortalList[i]);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void CreateUndergroundMazeNpc(CsUndergroundMazeNpc csUndergroundMazeNpc)
	{
		if (csUndergroundMazeNpc == null) return;
		if (m_dicNpcs.ContainsKey(csUndergroundMazeNpc.NpcId)) return;

		if (csUndergroundMazeNpc.Floor == m_csDungeonManager.UndergroundMazeFloor.Floor)
		{
			GameObject goNpc = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/NpcObject/{0:D2}", csUndergroundMazeNpc.PrefabName));

			if (goNpc == null)
			{
				Debug.Log("CreateNpc         프리펩 네임 확인 필요         PrefabName = " + csUndergroundMazeNpc.PrefabName);
				return;
			}

			goNpc = Instantiate(goNpc, csUndergroundMazeNpc.Position, Quaternion.identity, transform.Find("Npc")) as GameObject;
			goNpc.AddComponent<UnityEngine.AI.NavMeshAgent>();
			goNpc.AddComponent<CapsuleCollider>();
			goNpc.AddComponent<CsNpc>();

			CsNpc csNpc = goNpc.GetComponent<CsNpc>();
			csNpc.InitDungeonNpc(csUndergroundMazeNpc);
			m_dicNpcs.Add(csUndergroundMazeNpc.NpcId, csNpc);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CreateUndergroundMazePotal(CsUndergroundMazePortal csUndergroundMazePortal)
	{
		if (csUndergroundMazePortal == null) return;
		if (csUndergroundMazePortal.Floor == m_csDungeonManager.UndergroundMazeFloor.Floor)
		{
			Debug.Log("1. CreatePotal() ");
			Transform trPortal = transform.Find("Portal");
			GameObject goPortal = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Portal"), trPortal);
			goPortal.GetComponent<CsPortalArea>().Init(csUndergroundMazePortal);
		}
	}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonUndergroundMaze());
	}
}
