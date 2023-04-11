using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonTeamBattlefield : CsPlayThemeDungeon
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

		m_csDungeonManager.EventTeamBattlefieldStart += OnEventTeamBattlefieldStart;
		m_csDungeonManager.EventTeamBattlefieldAbandon += EventTeamBattlefieldAbandon;
		m_csDungeonManager.EventTeamBattlefieldBanished += OnTeamBattlefieldBanished;
		m_csDungeonManager.EventTeamBattlefieldExit += OnEventTeamBattlefieldExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventTeamBattlefieldStart -= OnEventTeamBattlefieldStart;
		m_csDungeonManager.EventTeamBattlefieldAbandon -= EventTeamBattlefieldAbandon;
		m_csDungeonManager.EventTeamBattlefieldBanished -= OnTeamBattlefieldBanished;
		m_csDungeonManager.EventTeamBattlefieldExit -= OnEventTeamBattlefieldExit;
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
		Debug.Log("CsPlayThemeDungeonTeamBattlefield.OnEventDungeonStartAutoPlay()                    enAutoDungeonPlay = " + enAutoDungeonPlay);
		if (enAutoDungeonPlay != EnDungeonPlay.TeamBattlefield) return;

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
		Debug.Log("CsPlayThemeDungeonTeamBattlefield.OnEventDungeonStopAutoPlay()                  enAutoDungeonPlay = " + enAutoDungeonPlay);
		if (enAutoDungeonPlay != EnDungeonPlay.TeamBattlefield) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldStart()
	{
		Debug.Log("CsPlayThemeDungeonTeamBattlefield.OnEventTeamBattlefieldStart()      m_csDungeonManager.Auto  => " + m_csDungeonManager.Auto);
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void EventTeamBattlefieldAbandon(bool bLevelUp, long lAcquiredExp, int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnTeamBattlefieldBanished(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTeamBattlefieldExit(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
