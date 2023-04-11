using UnityEngine;

public class CsPlayThemeDungeonFieldOfHonor : CsPlayThemeDungeon
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
		
        m_csDungeonManager.EventFieldOfHonorStart += OnEventFieldOfHonorStart;
        m_csDungeonManager.EventFieldOfHonorBanished += OnEventFieldOfHonorBanished;
		m_csDungeonManager.EventFieldOfHonorAbandon += OnEventFieldOfHonorAbandon;
		m_csDungeonManager.EventFieldOfHonorExit += OnEventFieldOfHonorExit;
	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventFieldOfHonorStart -= OnEventFieldOfHonorStart;
        m_csDungeonManager.EventFieldOfHonorBanished -= OnEventFieldOfHonorBanished;
        m_csDungeonManager.EventFieldOfHonorAbandon -= OnEventFieldOfHonorAbandon;
		m_csDungeonManager.EventFieldOfHonorExit -= OnEventFieldOfHonorExit;
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
				if (m_csDungeonManager.FieldOfHonor == null) return;
				DugeonBattle(m_csDungeonManager.FieldOfHonor.StartPosition, 100f);
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
            Debug.Log("CsPlayThemeDungeonFieldOfHonor.StopAutoPlay() bNotify = " + bNotify);
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
        if (enAutoDungeonPlay != EnDungeonPlay.FieldOfHonor) return;
        if (Player.Dead)
        {
            CsDungeonManager.Instance.StopAutoPlay(this);
            return;
        }

        Debug.Log("CsPlayThemeDungeonFieldOfHonor.OnEventStartAutoPlay()");
        Player.SetAutoPlay(this, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.FieldOfHonor) return;
        if (!IsThisAutoPlaying()) return;
        if (objCaller == this) return;

        Debug.Log("CsPlayThemeDungeonFieldOfHonor.OnEventStopAutoPlay()");
        Player.SetAutoPlay(null, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorStart()
    {
		m_bDungeonStart = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorBanished(int nContinent)
    {
		ExitDungeon();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorAbandon(int nContinent)
    {
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFieldOfHonorExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}