using UnityEngine;

public class CsPlayThemeDungeonAncientRelic : CsPlayThemeDungeon
{
	Vector3 m_vtTargetPosition;
	float m_flTargetRadius;

    #region IAutoPlay
    //---------------------------------------------------------------------------------------------------

    #endregion IAutoPlay
    
    #region Override

    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csDungeonManager = CsDungeonManager.Instance;
        m_csDungeonManager.EventDungeonStartAutoPlay += OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csDungeonManager.EventAncientRelicStepStart += OnEventAncientRelicStepStart;
        m_csDungeonManager.EventAncientRelicBanished += OnEventAncientRelicBanished;
        m_csDungeonManager.EventAncientRelicAbandon += OnEventAncientRelicAbandon;
		m_csDungeonManager.EventAncientRelicExit += OnEventAncientRelicExit;

	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventAncientRelicStepStart -= OnEventAncientRelicStepStart;
        m_csDungeonManager.EventAncientRelicBanished -= OnEventAncientRelicBanished;
        m_csDungeonManager.EventAncientRelicAbandon -= OnEventAncientRelicAbandon;
		m_csDungeonManager.EventAncientRelicExit -= OnEventAncientRelicExit;
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
			if (m_bDungeonStart)
			{
				if (MovePlayer(m_vtTargetPosition, m_flTargetRadius))
				{
					DugeonBattle(m_vtTargetPosition, 100f);
				}
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
            Debug.Log("CsPlayThemeDugeonAncientRelic.StopAutoPlay() bNotify = " + bNotify);
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
        if (enAutoDungeonPlay != EnDungeonPlay.AncientRelic) return;

        if (Player.Dead)
        {
            CsDungeonManager.Instance.StopAutoPlay(this);
            return;
        }

		Debug.Log("CsPlayThemeDugeonAncientRelic.OnEventStartAutoPlay()");
		Player.DisplayPath.SetPath(m_vtTargetPosition);
        Player.SetAutoPlay(this, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.AncientRelic) return;
        if (!IsThisAutoPlaying()) return;
        if (objCaller == this) return;

        Debug.Log("CsPlayThemeDugeonAncientRelic.OnEventStopAutoPlay()");
        Player.SetAutoPlay(null, false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventAncientRelicStepStart(int nRemoveObstacleId, Vector3 vtTargetPosition, float flTargetRadius)
	{
		m_bDungeonStart = true;
		m_vtTargetPosition = vtTargetPosition;
		m_flTargetRadius = flTargetRadius;

		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeDugeonAncientRelic.OnEventAncientRelicStepStart()");
			Player.DisplayPath.SetPath(m_vtTargetPosition);
		}
	}
	
    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicBanished(int nContinent)
    {
		ExitDungeon();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventAncientRelicAbandon(int nContinent)
	{
		ExitDungeon();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventAncientRelicExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
