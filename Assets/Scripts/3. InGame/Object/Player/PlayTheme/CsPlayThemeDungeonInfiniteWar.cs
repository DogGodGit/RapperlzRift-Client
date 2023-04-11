using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonInfiniteWar : CsPlayThemeDungeon
{
	#region IAutoPlay
	#endregion IAutoPlay	

	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventInfiniteWarStart += OnEventInfiniteWarStart;		
		m_csDungeonManager.EventInfiniteWarAbandon += OnEventInfiniteWarAbandon;
		m_csDungeonManager.EventInfiniteWarBanished += OnEventInfiniteWarBanished;
		m_csDungeonManager.EventInfiniteWarExit += OnEventInfiniteWarExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventInfiniteWarStart -= OnEventInfiniteWarStart;
		m_csDungeonManager.EventInfiniteWarAbandon -= OnEventInfiniteWarAbandon;
		m_csDungeonManager.EventInfiniteWarBanished -= OnEventInfiniteWarBanished;
		m_csDungeonManager.EventInfiniteWarExit -= OnEventInfiniteWarExit;
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
			Debug.Log("CsPlayThemeDungeonInfiniteWar.StopAutoPlay() bNotify = " + bNotify);
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
		Debug.Log("CsPlayThemeDungeonInfiniteWar.OnEventDungeonStartAutoPlay()                    enAutoDungeonPlay = " + enAutoDungeonPlay);
		if (enAutoDungeonPlay != EnDungeonPlay.InfiniteWar) return;
		
		if (Player.Dead)
		{
			m_csDungeonManager.StopAutoPlay(this);
			return;
		}
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		Debug.Log("CsPlayThemeDungeonInfiniteWar.OnEventDungeonStopAutoPlay()                  enAutoDungeonPlay = "+ enAutoDungeonPlay);
		if (enAutoDungeonPlay != EnDungeonPlay.InfiniteWar) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarStart()
	{
		Debug.Log("CsPlayThemeDungeonInfiniteWar.OnEventInfiniteWarStart()      m_csDungeonManager.Auto  => " + m_csDungeonManager.Auto);
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarAbandon(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarBanished(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventInfiniteWarExit(int nPreviousContinentId)
	{
		ExitDungeon();
	}
	#endregion Event.DungeonManager
}
