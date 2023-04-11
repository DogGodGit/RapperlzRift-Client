using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonDragonNest : CsPlayThemeDungeon
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

		m_csDungeonManager.EventDragonNestStepStart += OnEventDragonNestStepStart;
		m_csDungeonManager.EventDragonNestBanished += OnEventDragonNestBanished;
		m_csDungeonManager.EventDragonNestAbandon += OnEventDragonNestAbandon;
		m_csDungeonManager.EventDragonNestExit += OnEventDragonNestExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventDungeonStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventDungeonStopAutoPlay;

		m_csDungeonManager.EventDragonNestStepStart -= OnEventDragonNestStepStart;
		m_csDungeonManager.EventDragonNestBanished -= OnEventDragonNestBanished;
		m_csDungeonManager.EventDragonNestAbandon -= OnEventDragonNestAbandon;
		m_csDungeonManager.EventDragonNestExit -= OnEventDragonNestExit;
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
			Debug.Log("CsPlayThemeDungeonDragonNest.StopAutoPlay() bNotify = " + bNotify);
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
		if (enAutoDungeonPlay != EnDungeonPlay.DragonNest) return;

		if (Player.Dead)
		{
			m_csDungeonManager.StopAutoPlay(this);
			return;
		}

		Player.SetAutoPlay(this, true);
		DragonNestPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDungeonStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.DragonNest) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestStepStart(PDDragonNestMonsterInstance[] aMonsterInstance)
	{
		m_bDungeonStart = true;
		if (IsThisAutoPlaying())
		{
			DragonNestPlay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestBanished(int nContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestAbandon(int nContinentId)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDragonNestExit(int nContinentId)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void DragonNestPlay()
	{
		if (m_csDungeonManager.DragonNestStep == null) return;

		Vector3 vtTargetPos = new Vector3(m_csDungeonManager.DragonNestStep.TargetXPosition, m_csDungeonManager.DragonNestStep.TargetYPosition, m_csDungeonManager.DragonNestStep.TargetZPosition);
		EnWayPointType enWayPointType = m_csDungeonManager.DragonNestStep.Type == 1 ? EnWayPointType.Move : EnWayPointType.Battle;

		m_csPlayer.SetWayPoint(vtTargetPos, enWayPointType, m_csDungeonManager.DragonNestStep.TargetRadius);
		m_csPlayer.DisplayPath.SetPath(vtTargetPos);
	}
}
