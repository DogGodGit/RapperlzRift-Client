using ClientCommon;
using System;
using UnityEngine;

public class CsPlayThemeDungeonRuinsReclaim : CsPlayThemeDungeon
{
	enum EnDugeonType { Move = 1, Interaction, Wave }

	bool m_bPortalEnter = false;
	CsRuinsReclaimObject m_csRuinsReclaimObject;
	CsRuinsReclaimPortal m_csRuinsReclaimPortal;

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
		m_csDungeonManager.EventRuinsReclaimRevive += OnEventRuinsReclaimRevive;

		m_csDungeonManager.EventDugeonObjectInteractionStart += OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel += OnEventDungeonInteractionStartCancel;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel += OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished += OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;
		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionCancel += OnEventRuinsReclaimRewardObjectInteractionCancel;
		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionFinished += OnEventRuinsReclaimRewardObjectInteractionFinished;

		m_csDungeonManager.EventRuinsReclaimDebuffEffectStart += OnEventRuinsReclaimDebuffEffectStart;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStop += OnEventRuinsReclaimDebuffEffectStop;

		m_csDungeonManager.EventRuinsReclaimPortalEnter += OnEventRuinsReclaimPortalEnter;
		m_csDungeonManager.EventRuinsReclaimStepStart += OnEventRuinsReclaimStepStart;

		m_csDungeonManager.EventRuinsReclaimAbandon += OnEventRuinsReclaimAbandon;
		m_csDungeonManager.EventRuinsReclaimBanished += OnEventRuinsReclaimBanished;
		m_csDungeonManager.EventRuinsReclaimExit += OnEventRuinsReclaimExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;
		m_csDungeonManager.EventRuinsReclaimRevive -= OnEventRuinsReclaimRevive;

		m_csDungeonManager.EventDugeonObjectInteractionStart -= OnEventDugeonObjectInteractionStart;
		m_csDungeonManager.EventDungeonInteractionStartCancel -= OnEventDungeonInteractionStartCancel;

		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel -= OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel;
		m_csDungeonManager.EventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished -= OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished;

		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionCancel -= OnEventRuinsReclaimRewardObjectInteractionCancel;
		m_csDungeonManager.EventRuinsReclaimRewardObjectInteractionFinished -= OnEventRuinsReclaimRewardObjectInteractionFinished;

		m_csDungeonManager.EventRuinsReclaimDebuffEffectStart -= OnEventRuinsReclaimDebuffEffectStart;
		m_csDungeonManager.EventRuinsReclaimDebuffEffectStop -= OnEventRuinsReclaimDebuffEffectStop;

		m_csDungeonManager.EventRuinsReclaimPortalEnter -= OnEventRuinsReclaimPortalEnter;
		m_csDungeonManager.EventRuinsReclaimStepStart -= OnEventRuinsReclaimStepStart;

		m_csDungeonManager.EventRuinsReclaimAbandon -= OnEventRuinsReclaimAbandon;
		m_csDungeonManager.EventRuinsReclaimBanished -= OnEventRuinsReclaimBanished;
		m_csDungeonManager.EventRuinsReclaimExit -= OnEventRuinsReclaimExit;

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
				PlayAutoRuinsReclaim();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeDungeonRuinsReclaim.StopAutoPlay() bNotify = " + bNotify);
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
		if (enAutoDungeonPlay != EnDungeonPlay.RuinsReclaim) return;
		Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Player.SetAutoPlay(this, true);
		PlayAutoRuinsReclaim();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.RuinsReclaim) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRevive(PDVector3 pDVector3, float flRotationY)
	{
		Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventRuinsReclaimRevive()");
		m_bPortalEnter = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDugeonObjectInteractionStart()
	{
		if (m_csDungeonManager.RuinsReclaimStep == null) return;

		if (Player.IsStateIdle)
		{
			if (FindInteractionObject())
			{
				Player.SelectTarget(m_csRuinsReclaimObject.transform, true);

				Player.LookAtPosition(m_csRuinsReclaimObject.transform.position);
				Player.ChangeState(CsHero.EnState.Idle);
				Player.ChangeState(CsHero.EnState.Interaction);
				Player.ForceSyncMoveDataWithServer();
				m_csDungeonManager.RuinsReclaimObjectInteractionStart(m_csRuinsReclaimObject);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonInteractionStartCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimMonsterTransformationCancelObjectInteractionFinished(long lInstanceId)
	{
		ResetInteractionData();
		Player.ChangeTransformationState(CsHero.EnTransformationState.None);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRewardObjectInteractionCancel()
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimRewardObjectInteractionFinished(PDItemBooty pDItemBooty, long lInstanceId)
	{
		ResetInteractionData();
		Player.ChangeState(CsHero.EnState.Idle);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimDebuffEffectStart()
	{
		Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventRuinsReclaimDebuffEffectStart()  ");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimDebuffEffectStop()
	{
		Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventRuinsReclaimDebuffEffectStop()  ");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimPortalEnter(PDVector3 pdVetor3, float flRotationY)
	{
		m_bPortalEnter = true;
		if (IsThisAutoPlaying())
		{
			Debug.Log("CsPlayThemeDungeonRuinsReclaim.OnEventRuinsReclaimPortalEnter()");
			PlayAutoRuinsReclaim();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimStepStart(PDRuinsReclaimRewardObjectInstance[] apDRuinsReclaimRewardObjectInstance)
	{
		m_bDungeonStart = true;

		CsRuinsReclaimPortal csRuinsReclaimPortal = m_csDungeonManager.RuinsReclaim.GetRuinsReclaimPortal(m_csDungeonManager.RuinsReclaimStep.ActivationPortalId);
		if (csRuinsReclaimPortal != null)
		{
			m_csRuinsReclaimPortal = csRuinsReclaimPortal;
		}
		
		if (IsThisAutoPlaying())
		{
			PlayAutoRuinsReclaim();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRuinsReclaimExit(int nContinent)
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
			m_csRuinsReclaimObject = trTarget.GetComponent<CsRuinsReclaimObject>();
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetInteractionData()
	{
		Debug.Log("ResetInteractionData     m_csRuinsReclaimObject = " + m_csRuinsReclaimObject);
		if (m_csRuinsReclaimObject != null)
		{
			if (m_csRuinsReclaimObject.transform == Player.Target)
			{
				Player.ResetTarget();
			}
			m_csRuinsReclaimObject = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (Player == null || Player.DisplayPath == null || m_csDungeonManager.RuinsReclaimStep == null) return;

		Debug.Log("CsPlayThemeDungeonRuinsReclaim.SetDisplayPath     Type = " + (EnDugeonType)m_csDungeonManager.RuinsReclaimStep.Type + "  // PortalEnter = " + m_bPortalEnter + " // m_csRuinsReclaimPortal = " + m_csRuinsReclaimPortal);
		EnWayPointType enWayPointType = EnWayPointType.Move;

		switch ((EnDugeonType)m_csDungeonManager.RuinsReclaimStep.Type)
		{
		case EnDugeonType.Move:
		case EnDugeonType.Interaction:
			enWayPointType = EnWayPointType.Interaction;
			break;
		case EnDugeonType.Wave:
			enWayPointType = EnWayPointType.Battle;
			break;
		}

		if (m_csRuinsReclaimPortal != null && m_bPortalEnter == false)
		{
			Vector3 vtPortalPos = new Vector3(m_csRuinsReclaimPortal.XPosition, m_csRuinsReclaimPortal.YPosition, m_csRuinsReclaimPortal.ZPosition);
			Player.SetWayPoint(vtPortalPos, EnWayPointType.Move, 4);
			Player.DisplayPath.SetPath(vtPortalPos);
		}
		else
		{
			Player.SetWayPoint(m_csDungeonManager.RuinsReclaimStep.TargetPosition, enWayPointType, m_csDungeonManager.RuinsReclaimStep.TargetRadius);
			Player.DisplayPath.SetPath(m_csDungeonManager.RuinsReclaimStep.TargetPosition);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayAutoRuinsReclaim()
	{
		if (m_csDungeonManager.RuinsReclaim == null) return;
		if (m_csDungeonManager.RuinsReclaimStep == null) return;

		SetDisplayPath();

		switch ((EnDugeonType)m_csDungeonManager.RuinsReclaimStep.Type)
		{
			case EnDugeonType.Move:
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (MovePlayer(CsIngameData.Instance.TameMonster.transform.position, CsIngameData.Instance.TameMonster.TameRadius)) // 테임 가능 범위 안으로 이동만.
					{
						m_csPlayer.ChangeState(CsHero.EnState.Idle);
					}
				}
				break;
			case EnDugeonType.Interaction:
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (MovePlayer(CsIngameData.Instance.TameMonster.transform.position, CsIngameData.Instance.TameMonster.TameRadius)) // 테임 가능 범위 안으로 이동만.
					{
						m_csPlayer.ChangeState(CsHero.EnState.Idle);
					}
				}
				break;
			case EnDugeonType.Wave:
				if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
				{
					if (MovePlayer(CsIngameData.Instance.TameMonster.transform.position, CsIngameData.Instance.TameMonster.TameRadius)) // 테임 가능 범위 안으로 이동만.
					{
						DugeonBattle(Player.transform.position, 100f);
					}
				}
				break;
		}
	}
}
