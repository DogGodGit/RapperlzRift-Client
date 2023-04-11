using ClientCommon;
using UnityEngine;

public class CsPlayThemeDungeonTradeShip : CsPlayThemeDungeon
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

		m_csDungeonManager.EventTradeShipStepStart += OnEventTradeShipStepStart;
		m_csDungeonManager.EventTradeShipAbandon += OnEventTradeShipAbandon;
		m_csDungeonManager.EventTradeShipBanished += OnEventTradeShipBanished;
		m_csDungeonManager.EventTradeShipExit += OnEventTradeShipExit;
	}

	//---------------------------------------------------------------------------------------------------
	public override void Uninit()
	{
		m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
		m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventTradeShipStepStart -= OnEventTradeShipStepStart;
		m_csDungeonManager.EventTradeShipAbandon -= OnEventTradeShipAbandon;
		m_csDungeonManager.EventTradeShipBanished -= OnEventTradeShipBanished;
		m_csDungeonManager.EventTradeShipExit -= OnEventTradeShipExit;
		m_csDungeonManager = null;
		base.Uninit();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StartAutoPlay()
	{
		base.StartAutoPlay();
		//m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void UpdateAutoPlay()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csDungeonManager.ExpDungeon == null) return;

			if (m_bDungeonStart)
			{
				DugeonBattle(m_csDungeonManager.ExpDungeon.StartPosition, 100f);    // 원형던전 이동 필요 없음.
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void StopAutoPlay(bool bNotify)
	{
		if (Player.AutoPlay == this)
		{
			Debug.Log("CsPlayThemeDungeonAnkouTomb.StopAutoPlay() bNotify = " + bNotify);
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
		if (Player.Dead || enAutoDungeonPlay != EnDungeonPlay.TradeShip)
		{
			CsDungeonManager.Instance.StopAutoPlay(this);
			return;
		}

		Debug.Log("CsPlayThemeDungeonAnkouTomb.OnEventStartAutoPlay()");
		Player.SetAutoPlay(this, true);
		//TradeShipPlay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
	{
		if (enAutoDungeonPlay != EnDungeonPlay.TradeShip) return;
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;

		Debug.Log("CsPlayThemeDungeonAnkouTomb.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTradeShipStepStart(PDTradeShipMonsterInstance[] aMonInst, PDTradeShipAdditionalMonsterInstance[] aAddMonInst, PDTradeShipObjectInstance[] aObjInst)
	{
		m_bDungeonStart = true;

		//if (IsThisAutoPlaying())
		//{
		//	TradeShipPlay();
		//}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTradeShipAbandon(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTradeShipBanished(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTradeShipExit(int nContinent)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void TradeShipPlay()
	{
		if (m_csDungeonManager.TradeShipStep == null) return;

		//Vector3 vtTargetPos = new Vector3(m_csDungeonManager.TradeShipStep, m_csDungeonManager.TradeShipStep.TargetYPosition, m_csDungeonManager.DragonNestStep.TargetZPosition);
		//EnWayPointType enWayPointType = m_csDungeonManager.DragonNestStep.Type == 1 ? EnWayPointType.Move : EnWayPointType.Battle;

		//m_csPlayer.SetWayPoint(vtTargetPos, enWayPointType, m_csDungeonManager.DragonNestStep.TargetRadius);
		//m_csPlayer.DisplayPath.SetPath(vtTargetPos);
	}

	#endregion Event.DungeonManager
}
