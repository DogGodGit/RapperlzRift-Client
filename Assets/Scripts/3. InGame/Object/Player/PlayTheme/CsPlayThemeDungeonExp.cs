using ClientCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 안광열 (2018-01-17)
//---------------------------------------------------------------------------------------------------

public class CsPlayThemeDungeonExp : CsPlayThemeDungeon
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

		m_csDungeonManager.EventExpDungeonWaveStart += OnEventExpDungeonWaveStart;
		m_csDungeonManager.EventExpDungeonAbandon += OnEventExpDungeonAbandon;
		m_csDungeonManager.EventExpDungeonBanished += OnEventExpDungeonBanished;
		m_csDungeonManager.EventExpDungeonExit += OnEventExpDungeonExit;
	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventExpDungeonWaveStart -= OnEventExpDungeonWaveStart;
		m_csDungeonManager.EventExpDungeonAbandon -= OnEventExpDungeonAbandon;
		m_csDungeonManager.EventExpDungeonBanished -= OnEventExpDungeonBanished;
		m_csDungeonManager.EventExpDungeonExit -= OnEventExpDungeonExit;
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
			Debug.Log("CsPlayThemeDugeonExp.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.Exp)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeDugeonExp.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.Exp) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDugeonExp.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExpDungeonWaveStart(PDExpDungeonMonsterInstance[] aExp, PDExpDungeonLakChargeMonsterInstance mon)
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExpDungeonAbandon(int nContinent)
    {
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExpDungeonBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventExpDungeonExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
