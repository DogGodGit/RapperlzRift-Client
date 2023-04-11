using ClientCommon;
using UnityEngine;


public class CsPlayThemeDungeonMainQuest : CsPlayThemeDungeon
{
    CsMainQuestDungeonManager m_csMainQuestDungeonManager;

    #region IAutoPlay
    #endregion IAutoPlay

    #region Override

    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csMainQuestDungeonManager = CsMainQuestDungeonManager.Instance;
        m_csMainQuestDungeonManager.EventMainQuestDungeonStartAutoPlay += OnEventStartAutoPlay;
        m_csMainQuestDungeonManager.EventMainQuestDungeonStopAutoPlay += OnEventStopAutoPlay;

		m_csMainQuestDungeonManager.EventMainQuestDungeonStepStart += OnEventMainQuestDungeonStepStart;
		m_csMainQuestDungeonManager.EventMainQuestDungeonBanished += OnEventMainQuestDungeonBanished;
        m_csMainQuestDungeonManager.EventMainQuestDungeonAbandon += OnEventMainQuestDungeonAbandon;
		m_csMainQuestDungeonManager.EventMainQuestDungeonExit += OnEventMainQuestDungeonExit;
		Debug.Log("CsPlayThemeDungeonMainQuest.Init()");
	}

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csMainQuestDungeonManager.EventMainQuestDungeonStartAutoPlay -= OnEventStartAutoPlay;
        m_csMainQuestDungeonManager.EventMainQuestDungeonStopAutoPlay -= OnEventStopAutoPlay;

		m_csMainQuestDungeonManager.EventMainQuestDungeonStepStart -= OnEventMainQuestDungeonStepStart;
		m_csMainQuestDungeonManager.EventMainQuestDungeonBanished -= OnEventMainQuestDungeonBanished;
        m_csMainQuestDungeonManager.EventMainQuestDungeonAbandon -= OnEventMainQuestDungeonAbandon;
		m_csMainQuestDungeonManager.EventMainQuestDungeonExit -= OnEventMainQuestDungeonExit;
		m_csMainQuestDungeonManager = null;
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
			PlayAutoMainQuestDungeon();
		}
	}

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.IsAutoPlaying)
        {
            Debug.Log("CsPlayThemeQuestMain.StopAutoPlay() bNotify = " + bNotify);
            if (bNotify)
            {
                m_csMainQuestDungeonManager.StopAutoPlay(this);
            }
        }
    }

    #endregion Override

	#region Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay()
	{
		Debug.Log("CsPlayThemeDugeonMainQuest.OnEventStartAutoPlay()");
		if (Player.Dead)
		{
			m_csMainQuestDungeonManager.StopAutoPlay(this);
			return;
		}

		SetDisplayPath();
		Player.SetAutoPlay(this, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller)
	{
		Debug.Log("CsPlayThemeDugeonMainQuest.OnEventStopAutoPlay()");
		if (!IsThisAutoPlaying()) return;
		if (objCaller == this) return;
		Player.SetAutoPlay(null, false);
	}
	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonStepStart(PDMainQuestDungeonMonsterInstance[] pDMainQuestDungeonMonsterInstance)
	{
		m_bDungeonStart = true;
		if (IsThisAutoPlaying())
		{
			SetDisplayPath();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonAbandon(int nContinent, bool bChangeScene)
    {
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonBanished(int nContinent, bool bChangeScene)
	{
		ExitDungeon();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestDungeonExit(int nContinent, bool bChangeScene)
	{
		ExitDungeon();
	}

	#endregion Event.DungeonManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		if (m_csMainQuestDungeonManager != null && m_csMainQuestDungeonManager.MainQuestDungeonStep != null)
		{
			EnWayPointType enWayPointType = m_csMainQuestDungeonManager.MainQuestDungeonStep.Type == 1 ? EnWayPointType.Move : EnWayPointType.Battle;
			Player.SetWayPoint(m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetPosition, enWayPointType, m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetRadius);
			Player.DisplayPath.SetPath(m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetPosition);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayAutoMainQuestDungeon()
	{
		if (m_csMainQuestDungeonManager == null) return;
		if (m_csMainQuestDungeonManager.MainQuestDungeon == null) return;
		if (m_csMainQuestDungeonManager.MainQuestDungeonStep == null) return;

		switch (m_csMainQuestDungeonManager.MainQuestDungeonStep.Type) // 1:이동, 2:몬스터처치
		{
			case 1: // 이동
				if (MovePlayer(m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetPosition, m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetRadius))
				{
					if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
					{
						m_csPlayer.ChangeState(CsHero.EnState.Idle);
					}
				}
				break;
			case 2: // 전투
				if (MovePlayer(m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetPosition, m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetRadius))
				{
					if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
					{
						DugeonBattle(m_csMainQuestDungeonManager.MainQuestDungeonStep.TargetPosition, 100f);
					}
				}
				break;
			case 3: // 연출
				if (m_csPlayer.IsStateIdle == false)
				{
					m_csPlayer.ChangeState(CsHero.EnState.Idle);
				}
				break;
		}
	}
}
