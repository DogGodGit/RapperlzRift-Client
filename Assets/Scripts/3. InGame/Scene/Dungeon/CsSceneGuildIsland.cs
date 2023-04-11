using ClientCommon;
using System;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 강훈 (2018-03-22)
//---------------------------------------------------------------------------------------------------
public class CsSceneGuildIsland : CsSceneIngameDungeon
{
    protected override void Start()
    {
        base.Start();
        CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter += OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryEnter += OnEventGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit += OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventGuildExit += OnEventGuildExit;
        CsGuildManager.Instance.EventGuildBanished += OnEventGuildBanished;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionStarted += OnEventHeroGuildFarmQuestInteractionStarted;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionCompleted += OnEventHeroGuildFarmQuestInteractionCompleted;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionCanceled += OnEventHeroGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionStarted += OnEventHeroGuildAltarSpellInjectionMissionStarted;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionCompleted += OnEventHeroGuildAltarSpellInjectionMissionCompleted;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionCanceled += OnEventHeroGuildAltarSpellInjectionMissionCanceled;
		CsGuildManager.Instance.EventGuildTerritoryEnterForGuildTerritoryRevival += OnEventGuildTerritoryEnterForGuildTerritoryRevival;


		if (CsIngameData.Instance.MyHeroDead)	// 죽은 상태인경우.
		{
			CsGuildManager.Instance.SendGuildTerritoryEnterForGuildTerritoryRevival();
		}
		else
		{
			if (CsGameData.Instance.MyHeroInfo.LocationId == CsGuildManager.Instance.GuildTerritory.LocationId)  // 최초 던전 입장 Enter
			{
				SendHeroInitEnterCommand();
			}
			else
			{
				CsGuildManager.Instance.SendGuildTerritoryEnter(); // 대륙에서 던전 입장 Enter
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		CsGuildManager.Instance.EventContinentExitForGuildTerritoryEnter -= OnEventContinentExitForGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryEnter -= OnEventGuildTerritoryEnter;
        CsGuildManager.Instance.EventGuildTerritoryExit -= OnEventGuildTerritoryExit;
        CsGuildManager.Instance.EventGuildExit -= OnEventGuildExit;
        CsGuildManager.Instance.EventGuildBanished -= OnEventGuildBanished;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionStarted -= OnEventHeroGuildFarmQuestInteractionStarted;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionCompleted -= OnEventHeroGuildFarmQuestInteractionCompleted;
        CsGuildManager.Instance.EventHeroGuildFarmQuestInteractionCanceled -= OnEventHeroGuildFarmQuestInteractionCanceled;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionStarted -= OnEventHeroGuildAltarSpellInjectionMissionStarted;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionCompleted -= OnEventHeroGuildAltarSpellInjectionMissionCompleted;
        CsGuildManager.Instance.EventHeroGuildAltarSpellInjectionMissionCanceled -= OnEventHeroGuildAltarSpellInjectionMissionCanceled;
		CsGuildManager.Instance.EventGuildTerritoryEnterForGuildTerritoryRevival -= OnEventGuildTerritoryEnterForGuildTerritoryRevival;
		base.OnDestroy();
    }

    #region event
    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForGuildTerritoryEnter(string strSceneName)
    {
        DungeonEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryEnter(Guid guidPlaceInstanceId, PDHero[] pDHeroes, PDMonsterInstance[] pDMonsters, PDVector3 pDPosition, float flRotationY)
    {
		Debug.Log(" OnEventGuildTerritoryEnter ");
		CsIngameData.Instance.MyHeroDead = false;
		SetMyHeroLocation(CsGameData.Instance.GuildTerritory.LocationId);
        SetMyHero(pDPosition, flRotationY, guidPlaceInstanceId, false);
        for (int i = 0; i < pDHeroes.Length; i++)
        {
			StartCoroutine(AsyncCreateHero(pDHeroes[i], false, false)); // 길드원 생성
        }

        for (int i = 0; i < pDMonsters.Length; i++)
        {
			StartCoroutine(AsyncCreateMonster(pDMonsters[i])); // 몬스터 생성
        }
		
        for (int i = 0; i < CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList.Count; i++)
        {
            CreateGuildTerritoryNpc(CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList[i]); // NPC 생성
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildTerritoryExit(int nPreviousContinentId)
    {
        DungeonExit(); // 추가 내용 확인
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildExit(int nPreviousContinentId)
    {
        DungeonExit();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBanished(int nPreviousContinentId)
    {
        DungeonExit();
    }

    //----------------------------------------------------------------------------------------------------
    protected override void OnEventResHeroInitEnter(int nReturnCode, HeroInitEnterResponseBody csRes) // 기본 입장.
    {
        Debug.Log("OnEventHeroInitEnter         nReturnCode = " + nReturnCode);
        if (nReturnCode == 0)
        {
            SetMyHeroLocation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationId);
            SetEnter(csRes.placeInstanceId, csRes.heroes, csRes.monsterInsts, csRes.position, csRes.rotationY);
        }
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildFarmQuestInteractionStarted(Guid guidHeroId)
    {
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			Debug.Log("OnEventHeroGuildFarmQuestInteractionStarted");
			m_dicHeros[guidHeroId].NetEventInteractionStart();
		}
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildFarmQuestInteractionCompleted(Guid guidHeroId)
    {
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			Debug.Log("OnEventHeroGuildFarmQuestInteractionCompleted");
			m_dicHeros[guidHeroId].NetEventInteractionFinished();
		}
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildFarmQuestInteractionCanceled(Guid guidHeroId)
    {
		if (m_dicHeros.ContainsKey(guidHeroId))
		{
			Debug.Log("OnEventHeroGuildFarmQuestInteractionCanceled");
			m_dicHeros[guidHeroId].NetEventInteractionCancel();
		}
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildAltarSpellInjectionMissionStarted(Guid guidHeroId)
    {
        for (int i = 0; i < CsGuildManager.Instance.GuildMemberList.Count; i++)
        {
            if (CsGuildManager.Instance.GuildMemberList[i].Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                continue;
            }
            else
            {
                if (CsGuildManager.Instance.GuildMemberList[i].Id == guidHeroId)
                {
                    //동작
                }
            }
        }
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildAltarSpellInjectionMissionCompleted(Guid guidHeroId)
    {
        for (int i = 0; i < CsGuildManager.Instance.GuildMemberList.Count; i++)
        {
            if (CsGuildManager.Instance.GuildMemberList[i].Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                continue;
            }
            else
            {
                if (CsGuildManager.Instance.GuildMemberList[i].Id == guidHeroId)
                {
                    //동작
                }
            }
        }
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventHeroGuildAltarSpellInjectionMissionCanceled(Guid guidHeroId)
    {
        for (int i = 0; i < CsGuildManager.Instance.GuildMemberList.Count; i++)
        {
            if (CsGuildManager.Instance.GuildMemberList[i].Id == CsGameData.Instance.MyHeroInfo.HeroId)
            {
                continue;
            }
            else
            {
                if (CsGuildManager.Instance.GuildMemberList[i].Id == guidHeroId)
                {
                    //동작
                }
            }
        }
    }

	//----------------------------------------------------------------------------------------------------
	void OnEventGuildTerritoryEnterForGuildTerritoryRevival(Guid guidPlaceInstanceId, PDHero[] pDHeroes, PDMonsterInstance[] pDMonsters, PDVector3 pDPosition, float flRotationY)
	{
		Debug.Log("OnEventGuildTerritoryEnterForGuildTerritoryRevival");

		CsIngameData.Instance.MyHeroDead = false;
		SetMyHeroLocation(CsGameData.Instance.GuildTerritory.LocationId);
		SetMyHero(pDPosition, flRotationY, guidPlaceInstanceId, false);

		for (int i = 0; i < pDHeroes.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(pDHeroes[i], false, false)); // 길드원 생성
		}

		for (int i = 0; i < pDMonsters.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(pDMonsters[i])); // 몬스터 생성
		}
		
		for (int i = 0; i < CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList.Count; i++)
		{
			CreateGuildTerritoryNpc(CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList[i]); // NPC 생성
		}
	}

	#endregion event

	//----------------------------------------------------------------------------------------------------
	void CreateGuildTerritoryNpc(CsGuildTerritoryNpc csGuildTerritoryNpc)
    {
        if (csGuildTerritoryNpc == null) return;
        if (m_dicNpcs.ContainsKey(csGuildTerritoryNpc.NpcId)) return;

        GameObject goNpc = CsIngameData.Instance.LoadAsset<GameObject>(string.Format("Prefab/NpcObject/{0:D2}", csGuildTerritoryNpc.PrefabName)) as GameObject;

        if (goNpc == null)
        {
            Debug.Log("CreateNpc         프리펩 네임 확인 필요         PrefabName = " + csGuildTerritoryNpc.PrefabName);
            return;
        }
        
		goNpc = Instantiate(goNpc, csGuildTerritoryNpc.Position, Quaternion.identity, transform.Find("Npc")) as GameObject;
		goNpc.AddComponent<UnityEngine.AI.NavMeshAgent>();
		goNpc.AddComponent<CapsuleCollider>();
		goNpc.AddComponent<CsNpc>();

		CsNpc csNpc = goNpc.GetComponent<CsNpc>();
        csNpc.InitGuildTerritoryNpc(csGuildTerritoryNpc);
        m_dicNpcs.Add(csGuildTerritoryNpc.NpcId, csNpc);
    }

    //----------------------------------------------------------------------------------------------------
    void SetEnter(Guid guidPlaceInstanceId, PDHero[] pDHeroes, PDMonsterInstance[] pDMonsters, PDVector3 pDPosition, float flRotationY)
    {
        SetMyHero(pDPosition, flRotationY, guidPlaceInstanceId, false);
        for (int i = 0; i < pDHeroes.Length; i++)
        {
			StartCoroutine(AsyncCreateHero(pDHeroes[i], false, false)); // 길드원 생성
        }

        for (int i = 0; i < pDMonsters.Length; i++)
        {
			StartCoroutine(AsyncCreateMonster(pDMonsters[i])); // 몬스터 생성
        }

        for (int i = 0; i < CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList.Count; i++)
        {
            CreateGuildTerritoryNpc(CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList[i]); // NPC 생성
        }
    }

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeQuestJobChange());			//	전직.
		AddPlayTheme(new CsPlayThemeQuestGuildAltar());
		AddPlayTheme(new CsPlayThemeQuestGuildFarm());
		AddPlayTheme(new CsPlayThemeQuestGuildFoodWareHouse());
		AddPlayTheme(new CsPlayThemeQuestGuildFishing());
	}	
}
