using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonFearAltar : CsPlayThemeDungeon
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

		m_csDungeonManager.EventFearAltarWaveStart += OnEventFearAltarWaveStart;
		m_csDungeonManager.EventFearAltarAbandon += OnEventFearAltarAbandon;
		m_csDungeonManager.EventFearAltarBanished += OnEventFearAltarBanished;
		m_csDungeonManager.EventFearAltarExit += OnEventFearAltarExit;

	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventFearAltarWaveStart -= OnEventFearAltarWaveStart;
		m_csDungeonManager.EventFearAltarAbandon -= OnEventFearAltarAbandon;
		m_csDungeonManager.EventFearAltarBanished -= OnEventFearAltarBanished;
		m_csDungeonManager.EventFearAltarExit -= OnEventFearAltarExit;


		m_csDungeonManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();

		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			m_timer.Init(0.2f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			DugeonBattle(Player.transform.position, 100f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			//Debug.Log("CsPlayThemeDungeonFearAltar.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.FearAltar)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}
		Debug.Log("CsPlayThemeDungeonFearAltar.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.FearAltar) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDungeonFearAltar.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarWaveStart(PDFearAltarMonsterInstance[] pDMonsters, PDFearAltarHalidomMonsterInstance pFearAltarHalidomMonster)
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nContinentId)
	{
		ExitDungeon();
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nContinentId)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
