using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonAnkouTomb : CsPlayThemeDungeon
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

		m_csDungeonManager.EventAnkouTombWaveStart += OnEventAnkouTombWaveStart;
		m_csDungeonManager.EventAnkouTombAbandon += OnEventAnkouTombAbandon;
		m_csDungeonManager.EventAnkouTombBanished += OnEventAnkouTombBanished;
		m_csDungeonManager.EventAnkouTombExit += OnEventAnkouTombExit;
	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventAnkouTombWaveStart -= OnEventAnkouTombWaveStart;
		m_csDungeonManager.EventAnkouTombAbandon -= OnEventAnkouTombAbandon;
		m_csDungeonManager.EventAnkouTombBanished -= OnEventAnkouTombBanished;
		m_csDungeonManager.EventAnkouTombExit -= OnEventAnkouTombExit;
		m_csDungeonManager = null;
        base.Uninit();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void StartAutoPlay()
    {
        base.StartAutoPlay();
        //m_timer.Init(0.2f);
    }

    //---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csDungeonManager.ExpDungeon == null) return;

			if (m_bDungeonStart)
			{
				DugeonBattle(m_csDungeonManager.ExpDungeon.StartPosition, 100f);    // 원형던전 이동 필요 없음.
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
			Debug.Log("CsPlayThemeDungeonAnkouTomb.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.AnkouTomb)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeDungeonAnkouTomb.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.AnkouTomb) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDungeonAnkouTomb.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombWaveStart(PDAnkouTombMonsterInstance[] aExp, int nWaveNo)
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombAbandon(int nContinent)
    {
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventAnkouTombExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
