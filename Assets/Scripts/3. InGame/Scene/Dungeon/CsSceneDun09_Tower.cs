using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun09_Tower : CsSceneIngameDungeon
{
    //---------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForArtifactRoomEnter += OnEventContinentExitForArtifactRoomEnter;
		m_csDungeonManager.EventArtifactRoomEnter += OnEventArtifactRoomEnter;
		m_csDungeonManager.EventArtifactRoomStart += OnEventArtifactRoomStart;
		m_csDungeonManager.EventArtifactRoomFail += OnEventArtifactRoomFail;
		m_csDungeonManager.EventArtifactRoomClear += OnEventArtifactRoomClear;

		m_csDungeonManager.SendArtifactRoomEnter();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();
		m_csDungeonManager.EventContinentExitForArtifactRoomEnter -= OnEventContinentExitForArtifactRoomEnter;
		m_csDungeonManager.EventArtifactRoomEnter -= OnEventArtifactRoomEnter;
		m_csDungeonManager.EventArtifactRoomStart -= OnEventArtifactRoomStart;
		m_csDungeonManager.EventArtifactRoomFail -= OnEventArtifactRoomFail;
		m_csDungeonManager.EventArtifactRoomClear -= OnEventArtifactRoomClear;
		m_csDungeonManager.ResetDungeon();

		m_csDungeonManager = null;
        base.OnDestroy();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForArtifactRoomEnter()
    {
        DungeonEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY)
    {
		SetMyHeroLocation(m_csDungeonManager.ArtifactRoom.LocationId);
        SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomStart(PDArtifactRoomMonsterInstance[] apArtifactRoomInstance)
    {
        for (int i = 0; i < apArtifactRoomInstance.Length; i++)
        {
			StartCoroutine(AsyncCreateMonster(apArtifactRoomInstance[i]));
        }    
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomFail()
    {
        //ClearMonster();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomClear(PDItemBooty pDItemBooty)
    {
		//테스트 던전 클리어 연출.
        //StartCoroutine(DungeonClearDirection());
    }

    //---------------------------------------------------------------------------------------------------
    public override void InitPlayThemes()
    {
        base.InitPlayThemes();
        AddPlayTheme(new CsPlayThemeDungeonArtifactRoom());
    }
}

