using UnityEngine;

public class CsPlayThemeDungeonProofOfValor : CsPlayThemeDungeon
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

		m_csDungeonManager.EventProofOfValorStart += OnEventProofOfValorStart;
		m_csDungeonManager.EventProofOfValorAbandon += OnEventProofOfValorAbandon;
		m_csDungeonManager.EventProofOfValorBanished += OnEventProofOfValorBanished;
		m_csDungeonManager.EventProofOfValorExit += OnEventProofOfValorExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventProofOfValorStart -= OnEventProofOfValorStart;
		m_csDungeonManager.EventProofOfValorAbandon -= OnEventProofOfValorAbandon;
		m_csDungeonManager.EventProofOfValorBanished -= OnEventProofOfValorBanished;
		m_csDungeonManager.EventProofOfValorExit -= OnEventProofOfValorExit;
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
			Debug.Log("CsPlayThemeDungeonProofOfValor.StopAutoPlay() bNotify = " + bNotify);
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
		Debug.Log(" enAutoDungeonPlay == " + enAutoDungeonPlay);
		if (enAutoDungeonPlay != EnDungeonPlay.ProofOfValor) return;
		Debug.Log("CsPlayThemeDungeonProofOfValor.OnEventDungeonStartAutoPlay()");
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
		if (enAutoDungeonPlay != EnDungeonPlay.ProofOfValor) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDungeonProofOfValor.OnEventDungeonStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorStart()
	{
		m_bDungeonStart = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorExit(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorBanished(int nPreviousContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventProofOfValorAbandon(int nPreviousContinentId, bool bLevelUp, long lAcquiredExp)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
}
