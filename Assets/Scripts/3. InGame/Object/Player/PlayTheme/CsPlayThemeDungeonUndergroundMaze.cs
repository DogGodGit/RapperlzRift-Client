using SimpleDebugLog;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 안광열 (2018-02-05)
//---------------------------------------------------------------------------------------------------

public class CsPlayThemeDungeonUndergroundMaze : CsPlayThemeDungeon
{
    #region IAutoPlay

    //---------------------------------------------------------------------------------------------------
	public override bool IsRequiredLoakAtTarget() { return true; }

	//---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		dd.d("1. CsPlayThemeDugeonUndergroundMaze.ArrivalMoveToPos");
		//Player.SetWayPoint(Vector3.zero, EnWayPointType.None);
	}

    #endregion IAutoPlay

    #region Override

    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csDungeonManager = CsDungeonManager.Instance;
        m_csDungeonManager.EventDungeonStartAutoPlay += OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csDungeonManager.EventUndergroundMazePortalEnter += OnEventUndergroundMazePortalEnter;
		m_csDungeonManager.EventUndergroundMazeTransmission += OnEventUndergroundMazeTransmission;
		m_csDungeonManager.EventUndergroundMazeBanished += OnEventUndergroundMazeBanished;
        m_csDungeonManager.EventUndergroundMazeExit += OnEventUndergroundMazeExit;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
    }

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csDungeonManager.EventDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csDungeonManager.EventDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csDungeonManager.EventUndergroundMazePortalEnter -= OnEventUndergroundMazePortalEnter;
		m_csDungeonManager.EventUndergroundMazeTransmission -= OnEventUndergroundMazeTransmission;
		m_csDungeonManager.EventUndergroundMazeBanished -= OnEventUndergroundMazeBanished;
        m_csDungeonManager.EventUndergroundMazeExit -= OnEventUndergroundMazeExit;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;
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
			DugeonBattle(Player.transform.position, 100f);
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
        {
			Debug.Log("CsPlayThemeDugeonUndergroundMaze.StopAutoPlay() bNotify = " + bNotify);
            if (bNotify)
            {
                m_csDungeonManager.StopAutoPlay(this);
            }
        }
    }

    #endregion Override

	#region Event.Player

	//---------------------------------------------------------------------------------------------------
	void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
	{
		if (bMoveByTouchTarget)
		{
			CsUndergroundMazeNpc csUndergroundMazeNpc = GetUndergroundMazeNpc(CsIngameData.Instance.IngameManagement.GetNpcId(m_csPlayer.Target));
			if (csUndergroundMazeNpc == null) return;

			if (Player.IsTargetInDistance(csUndergroundMazeNpc.Position, csUndergroundMazeNpc.InteractionMaxRange))
			{
				Debug.Log("CsPlayThemeDugeonUndergroundMaze.OnEventArrivalMoveByTouch()     >>     m_csDungeonManager.UndergroundMazeTransmissionReadyOK();");
				Player.IsQuestDialog = true;
				m_csDungeonManager.UndergroundMazeTransmissionReadyOK(csUndergroundMazeNpc.NpcId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsUndergroundMazeNpc GetUndergroundMazeNpc(int nNpcId)
	{
		if (m_csDungeonManager == null || m_csDungeonManager.UndergroundMazeFloor == null) return null;
		return m_csDungeonManager.UndergroundMazeFloor.UndergroundMazeNpcList.Find(a => a.NpcId == nNpcId);
	}

	#endregion Event.Player

    #region Event.DungeonManager

    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlay(EnDungeonPlay enAutoDungeonPlay)
    {
        if (enAutoDungeonPlay != EnDungeonPlay.UndergroundMaze) return;
		Debug.Log("CsPlayThemeDugeonUndergroundMaze.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			m_csDungeonManager.StopAutoPlay(this);
			return;
		}
		Player.SetAutoPlay(this, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnDungeonPlay enAutoDungeonPlay)
    {
		if (enAutoDungeonPlay != EnDungeonPlay.UndergroundMaze) return;
        if (!IsThisAutoPlaying()) return;
        if (objCaller == this) return;
		Debug.Log("CsPlayThemeDugeonUndergroundMaze.OnEventStopAutoPlay()");
		Player.SetAutoPlay(null, false);
    }

    //---------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazePortalEnter()
    {
		Debug.Log("CsPlayThemeDugeonUndergroundMaze.OnEventUndergroundMazePortalEnter()");
		if (m_csDungeonManager.Auto)
		{
			m_csPlayer.SetAutoPlay(null, true);
		}
    }

    //---------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeTransmission()
    {
		Debug.Log("CsPlayThemeDugeonUndergroundMaze.OnEventUndergroundMazeTransmission()");
		if (m_csDungeonManager.Auto)
		{
			m_csPlayer.SetAutoPlay(null, true);
		}
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventUndergroundMazeBanished(int nContinent)
	{
		ExitDungeon();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventUndergroundMazeExit(int nContinent)
    {
		ExitDungeon();
	}

    #endregion Event.DungeonManager
}
