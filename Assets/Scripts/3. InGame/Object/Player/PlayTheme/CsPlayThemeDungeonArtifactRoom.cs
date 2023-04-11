using ClientCommon;
using System;
using UnityEngine;

public class CsPlayThemeDungeonArtifactRoom : CsPlayThemeDungeon
{
    #region IAutoPlay

    //---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

    #endregion IAutoPlay

    #region Override

    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csDungeonManager = CsDungeonManager.Instance;
        m_csDungeonManager.EventDungeonStartAutoPlay += OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csDungeonManager.EventArtifactRoomStart += OnEventArtifactRoomStart;
		m_csDungeonManager.EventArtifactRoomBanished += OnEventArtifactRoomBanished;
		m_csDungeonManager.EventArtifactRoomAbandon += OnEventArtifactRoomAbandon;
		m_csDungeonManager.EventArtifactRoomExit += OnEventArtifactRoomExit;
	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventArtifactRoomStart -= OnEventArtifactRoomStart;
		m_csDungeonManager.EventArtifactRoomBanished -= OnEventArtifactRoomBanished;
		m_csDungeonManager.EventArtifactRoomAbandon -= OnEventArtifactRoomAbandon;
		m_csDungeonManager.EventArtifactRoomExit -= OnEventArtifactRoomExit;
		m_csDungeonManager = null;
        base.Uninit();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void StartAutoPlay()
    {
        base.StartAutoPlay();
        m_timer.Init(0.2f);
    }

    //---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csDungeonManager.ArtifactRoom == null) return;

			if (m_bDungeonStart)
			{
				DugeonBattle(m_csDungeonManager.ArtifactRoom.StartPosition, 100f);
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
            Debug.Log("CsPlayThemeQuestMain.StopAutoPlay() bNotify = " + bNotify);
            if (bNotify)
            {
                m_csDungeonManager.StopAutoPlay(this);
            }
        }
    }

    #endregion Override

    #region Event.DungeonManager

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlay(EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.ArtifactRoom) return;
		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}
		Debug.Log("CsPlayThemeDugeonArtifactRoom.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.ArtifactRoom) return;
        if (!IsThisAutoPlaying()) return;
        if (objCaller == this) return;

        Debug.Log("CsPlayThemeDugeonArtifactRoom.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
    }
	
	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactRoomStart(PDArtifactRoomMonsterInstance[] a)
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactRoomBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactRoomAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactRoomExit(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	#endregion Event.DungeonManager
}
