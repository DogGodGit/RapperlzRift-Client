using UnityEngine;

public class CsPlayThemeDungeonElite : CsPlayThemeDungeon
{
	#region IAutoPlay
	//---------------------------------------------------------------------------------------------------

	#endregion IAutoPlay

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventEliteDungeonStart += OnEventEliteDungeonStart;
		m_csDungeonManager.EventEliteDungeonAbandon += OnEventEliteDungeonAbandon;
		m_csDungeonManager.EventEliteDungeonBanished -= OnEventEliteDungeonBanished;
		m_csDungeonManager.EventEliteDungeonExit += OnEventEliteDungeonExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventEliteDungeonStart -= OnEventEliteDungeonStart;
		m_csDungeonManager.EventEliteDungeonAbandon -= OnEventEliteDungeonAbandon;
		m_csDungeonManager.EventEliteDungeonBanished -= OnEventEliteDungeonBanished;
		m_csDungeonManager.EventEliteDungeonExit -= OnEventEliteDungeonExit;
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
				if (m_csDungeonManager.EliteDungeon == null) return;

				DugeonBattle(m_csDungeonManager.EliteDungeon.MonsterPosition, 100f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeDungeonElite.StopAutoPlay() bNotify = " + bNotify);
			if (bNotify)
			{
				m_csDungeonManager.StopAutoPlay(this);
			}
		}
	}

	#endregion Override

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStartAutoPlay(EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.Elite) return;

		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeDungeonElite.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.Elite) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDungeonElite.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonStart()
	{
		Debug.Log("CsPlayThemeDungeonElite.OnEventEliteDungeonStart()");
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEliteDungeonExit(int nContinent)
	{
		ExitDungeon();
	}
	#endregion Event.DungeonManager
}
