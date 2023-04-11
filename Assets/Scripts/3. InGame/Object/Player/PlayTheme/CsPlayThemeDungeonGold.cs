using UnityEngine;

public class CsPlayThemeDungeonGold : CsPlayThemeDungeon
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

		m_csDungeonManager.EventGoldDungeonAbandon += OnEventGoldDungeonAbandon;
		m_csDungeonManager.EventGoldDungeonBanished += OnEventGoldDungeonBanished;
        m_csDungeonManager.EventGoldDungeonExit += OnEventGoldDungeonExit;

    }

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

        m_csDungeonManager.EventGoldDungeonAbandon -= OnEventGoldDungeonAbandon;
		m_csDungeonManager.EventGoldDungeonBanished -= OnEventGoldDungeonBanished;
		m_csDungeonManager.EventGoldDungeonExit -= OnEventGoldDungeonExit;
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
				DugeonBattle(Player.transform.position, 100f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
            Debug.Log("CsPlayThemeDugeonGold.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.Gold)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}
		Debug.Log("CsPlayThemeDugeonGold.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.Gold) return;
        if (!IsThisAutoPlaying()) return;
        if (objCaller == this) return;

        Debug.Log("CsPlayThemeDugeonGold.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventGoldDungeonAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGoldDungeonBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGoldDungeonExit(int nContinent)
	{
		ExitDungeon();
	}
	#endregion Event.DungeonManager
}
