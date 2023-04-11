using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonWarMemory : CsPlayThemeDungeon
{
	CsWarMemoryObject m_csWarMemoryObject = null;
	#region Override

	//---------------------------------------------------------------------------------------------------
	public override void Init(CsMyPlayer csPlayer)
	{
		base.Init(csPlayer);
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventDungeonStartAutoPlay += OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay += OnEventDungeonStopAutoPlay;
		m_csDungeonManager.EventWarMemoryTransformationObjectLifetimeEnded += OnEventWarMemoryTransformationObjectLifetimeEnded;
		m_csDungeonManager.EventDugeonObjectInteractionStart += OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel += OnEventDungeonInteractionStartCancel;

		m_csDungeonManager.EventWarMemoryTransformationObjectInteractionCancel += OnEventWarMemoryTransformationObjectInteractionCancel;

		m_csDungeonManager.EventWarMemoryWaveStart += OnEventWarMemoryWaveStart;
		m_csDungeonManager.EventWarMemoryBanished += OnEventWarMemoryBanished;
		m_csDungeonManager.EventWarMemoryAbandon += OnEventWarMemoryAbandon;
		m_csDungeonManager.EventWarMemoryExit += OnEventWarMemoryExit;

		if (m_csDungeonManager.WarMemoryWave != null)
		{
			m_bDungeonStart = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;
		m_csDungeonManager.EventWarMemoryTransformationObjectLifetimeEnded -= OnEventWarMemoryTransformationObjectLifetimeEnded;
		m_csDungeonManager.EventDugeonObjectInteractionStart -= OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel -= OnEventDungeonInteractionStartCancel;

		m_csDungeonManager.EventWarMemoryTransformationObjectInteractionCancel -= OnEventWarMemoryTransformationObjectInteractionCancel;

		m_csDungeonManager.EventWarMemoryWaveStart -= OnEventWarMemoryWaveStart;
		m_csDungeonManager.EventWarMemoryBanished -= OnEventWarMemoryBanished;
		m_csDungeonManager.EventWarMemoryAbandon -= OnEventWarMemoryAbandon;
		m_csDungeonManager.EventWarMemoryExit -= OnEventWarMemoryExit;
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
				PlayAutoWarMemory();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeDungeonWarMemory.StopAutoPlay() bNotify = " + bNotify);
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
		if (enAutoDungeonPlay != EnDungeonPlay.WarMemory) return;
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Player.SetAutoPlay(this, true);

		SetDisplayPath();
		PlayAutoWarMemory();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.WarMemory) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryTransformationObjectLifetimeEnded(long lInstanceId)
	{
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventWarMemoryTransformationObjectLifetimeEnded()   = "+ lInstanceId);
		if (m_csDungeonManager.InteractionInstanceId == lInstanceId)
		{
			if (m_csPlayer.State == CsHero.EnState.Interaction)
			{
				m_csPlayer.ChangeState(CsHero.EnState.Idle);
				ResetInteractionData();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDugeonObjectInteractionStart()
	{
		if (m_csDungeonManager.WarMemoryWave == null) return;

		Debug.Log("OnEventInteractionCheck    Player.IsStateIdle = " + Player.IsStateIdle);
		if (Player.IsStateIdle)
		{
			if (FindInteractionObject())
			{
				Player.SelectTarget(m_csWarMemoryObject.transform, true);
				Player.LookAtPosition(m_csWarMemoryObject.transform.position);
				Player.ChangeState(CsHero.EnState.Idle);
				Player.ChangeState(CsHero.EnState.Interaction);
				Player.ForceSyncMoveDataWithServer();
				m_csDungeonManager.WarMemoryTransformationObjectInteraction(m_csWarMemoryObject);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonInteractionStartCancel()
	{
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventDungeonInteractionStartCancel()");
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryTransformationObjectInteractionCancel()
	{
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventWarMemoryTransformationObjectInteractionCancel()");
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------

	void OnEventWarMemoryWaveStart(PDMonsterInstance[] aMonsterInstance, PDWarMemoryTransformationObjectInstance[] aWarMemoryTransformationObjectInstance)
	{
		Debug.Log("CsPlayThemeDungeonWarMemory.OnEventWarMemoryWaveStart()");
		m_bDungeonStart = true;
		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventWarMemoryExit(int nContinent)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager
	//---------------------------------------------------------------------------------------------------
	bool FindInteractionObject()
	{
		Transform trTarget = CsMyPlayer.FindTarget(Player.transform.position, Player.transform.position, "Object", "InteractionObject", 5);
		if (trTarget != null)
		{
			m_csWarMemoryObject = trTarget.GetComponent<CsWarMemoryObject>();
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetInteractionData()
	{
		Debug.Log("ResetInteractionData     m_csWarMemoryObject = " + m_csWarMemoryObject);
		if (m_csWarMemoryObject != null)
		{
			if (m_csWarMemoryObject.transform == Player.Target)
			{
				Player.ResetTarget();
			}
			m_csWarMemoryObject = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()	// 전쟁의 기억 따로 표시하지 않음.
	{
		if (Player == null || Player.DisplayPath == null || m_csDungeonManager.WarMemoryWave == null) return;

		switch (m_csDungeonManager.WarMemoryWave.TargetType)
		{
			case 1: // 모든 몬스터 처치
				break;
			case 2: // 대상 몬스터 처치
				//m_csDungeonManager.WarMemoryWave.TargetArrangeKey
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayAutoWarMemory()
	{
		if (m_csDungeonManager.WarMemoryWave == null) return;
		DugeonBattle(Player.transform.position, 100f);
	}
}
