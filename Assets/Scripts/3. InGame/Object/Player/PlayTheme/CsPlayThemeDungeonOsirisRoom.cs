using UnityEngine;

public class CsPlayThemeDungeonOsirisRoom : CsPlayThemeDungeon
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

		m_csDungeonManager.EventOsirisRoomWaveStart += OnEventOsirisRoomWaveStart;
		m_csDungeonManager.EventOsirisRoomBanished += OnEventOsirisRoomBanished;
		m_csDungeonManager.EventOsirisRoomAbandon += OnEventOsirisRoomAbandon;
		m_csDungeonManager.EventOsirisRoomExit += OnEventOsirisRoomExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventOsirisRoomWaveStart -= OnEventOsirisRoomWaveStart;
		m_csDungeonManager.EventOsirisRoomBanished -= OnEventOsirisRoomBanished;
		m_csDungeonManager.EventOsirisRoomAbandon -= OnEventOsirisRoomAbandon;
		m_csDungeonManager.EventOsirisRoomExit -= OnEventOsirisRoomExit;
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
			DugeonBattle(Player.transform.position, 100f);    // 원형던전 이동 필요 없음.
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeDungeonOsirisRoom.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.OsirisRoom)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeDungeonOsirisRoom.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.OsirisRoom) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDungeonOsirisRoom.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomWaveStart()
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOsirisRoomExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
