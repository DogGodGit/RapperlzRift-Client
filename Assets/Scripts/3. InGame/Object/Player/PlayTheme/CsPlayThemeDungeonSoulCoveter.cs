using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonSoulCoveter : CsPlayThemeDungeon 
{
	#region IAutoPlay
	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csDungeonManager.EventSoulCoveterWaveStart += OnEventSoulCoveterWaveStart;
		m_csDungeonManager.EventSoulCoveterBanished += OnEventSoulCoveterBanished;
		m_csDungeonManager.EventSoulCoveterAbandon += OnEventSoulCoveterAbandon;
		m_csDungeonManager.EventSoulCoveterExit += OnEventSoulCoveterExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventSoulCoveterWaveStart -= OnEventSoulCoveterWaveStart;
		m_csDungeonManager.EventSoulCoveterBanished -= OnEventSoulCoveterBanished;
		m_csDungeonManager.EventSoulCoveterAbandon -= OnEventSoulCoveterAbandon;
		m_csDungeonManager.EventSoulCoveterExit -= OnEventSoulCoveterExit;
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
				DugeonBattle(m_csDungeonManager.SoulCoveter.StartPosition, 100f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeDugeonSoulCoveter.StopAutoPlay() bNotify = " + bNotify);
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
		if (enAutoDungeonPlay != EnDungeonPlay.SoulCoveter) return;
		Debug.Log("CsPlayThemeDugeonSoulCoveter.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.SoulCoveter) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDugeonSoulCoveter.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterWaveStart(PDSoulCoveterMonsterInstance[] amon)
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSoulCoveterExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager

}
