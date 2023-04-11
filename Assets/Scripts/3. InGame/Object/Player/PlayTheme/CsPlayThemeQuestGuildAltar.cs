using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildAltar : CsPlayThemeQuest
{
    CsGuildManager m_csGuildManager;
    CsGuildTerritoryNpc m_csGuildTerritoryNpc;
	bool m_bArrival = false;

    #region IAutoPlay

    //---------------------------------------------------------------------------------------------------
    public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("1. CsPlayThemeQuestGuildAltar.ArrivalMoveToPos   m_enGuildPlayAutoState = " + m_enGuildPlayAutoState);
			if (m_enGuildPlayAutoState == EnGuildPlayState.Altar)
			{
				Player.SetAutoPlay(null, true);
			}
			m_bArrival = true;
			CheckAltarDialog();
		}
    }

    #endregion IAutoPlay

	#region Override
    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csGuildManager = CsGuildManager.Instance;
        m_csGuildTerritoryNpc = CsGameData.Instance.GuildAltar.GuildTerritoryNpc;
		
        m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;

        m_csGuildManager.EventGuildAltarSpellInjectionMissionStart += OnEventGuildAltarSpellInjectionMissionStart;
        m_csGuildManager.EventGuildAltarSpellInjectionMissionCompleted += OnEventGuildAltarSpellInjectionMissionCompleted;
        m_csGuildManager.EventGuildAltarSpellInjectionMissionCanceled += OnEventGuildAltarSpellInjectionMissionCanceled;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

        Debug.Log("Player  " + Player);
    }

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;
        
        m_csGuildManager.EventGuildAltarSpellInjectionMissionStart -= OnEventGuildAltarSpellInjectionMissionStart;
        m_csGuildManager.EventGuildAltarSpellInjectionMissionCompleted -= OnEventGuildAltarSpellInjectionMissionCompleted;
        m_csGuildManager.EventGuildAltarSpellInjectionMissionCanceled -= OnEventGuildAltarSpellInjectionMissionCanceled;

        Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

        m_csGuildManager = null;
		m_csGuildTerritoryNpc = null;
        base.Uninit();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void StartAutoPlay()
    {
       // m_timer.Init(0.2f);
    }

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (IsThisAutoPlaying())
			{
				Debug.Log("StateChangedToIdle");
				Player.StartCoroutine(CorutineStateChangedToIdle());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CorutineStateChangedToIdle()
	{
		yield return new WaitForSeconds(0.2f);
		if (Player.IsStateIdle)
		{
			if (IsThisAutoPlaying())
			{
				if (m_enGuildPlayAutoState == EnGuildPlayState.Altar) // 제단까지 자동이동.
				{
					if (m_bArrival == false)
					{
						PlayAltar();
					}
				}
			}
		}
		m_bArrival = false;
	}

    //---------------------------------------------------------------------------------------------------
 //   protected override void UpdateAutoPlay()
	//{
	//	if (m_enGuildPlayAutoState == EnGuildPlayState.Defense) // 제단 수비 퀘스트 Auto.
	//	{
	//		PlayDefense(m_csGuildManager.DefenseMonsterInstanceId, m_csGuildManager.DefenseMonsterPos);
	//	}
	//	else if (m_enGuildPlayAutoState == EnGuildPlayState.Altar)
	//	{
	//		PlayAltar();
	//	}
 //   }

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (IsThisAutoPlaying())
        {
			Player.IsQuestDialog = false;
			if (bNotify)
			{
				Debug.Log("StopAutoPlay   >>>>>     UI      m_enGuildPlayAutoState = " + m_enGuildPlayAutoState);
				m_csGuildManager.StopAutoPlay(this, m_enGuildPlayAutoState);
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
            if (m_csPlayer.Target.CompareTag("Npc"))
			{
                CheckAltarDialog();
            }
        }
    }

    #endregion Event.Player

    #region Command.GuildManager

    //----------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionStart()
    {
		Debug.Log("1. OnEventGuildAltarSpellInjectionMissionStart");
		Player.ResetBattleMode();
		if (Player.SkillStatus.IsStatusPlayAnim()) return;

		Debug.Log("2. OnEventGuildAltarSpellInjectionMissionStart");
		Player.ChangeState(CsHero.EnState.Interaction);
    }

    #endregion Command.GuildManager

    #region Event.GuildManager

    //----------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCompleted()
    {
		Debug.Log("OnEventGuildAltarSpellInjectionMissionCompleted");
		Player.ChangeState(CsHero.EnState.Idle);
    }

    //----------------------------------------------------------------------------------------------------
    void OnEventGuildAltarSpellInjectionMissionCanceled()
    {
		Debug.Log("OnEventGuildAltarSpellInjectionMissionCanceled");
        Player.ChangeState(CsHero.EnState.Idle);
    }

    #endregion Event.GuildManager

    #region Event.Auto
	EnGuildPlayState m_enGuildPlayAutoState = EnGuildPlayState.None;
	EnGuildPlayState m_enOldGuildPlayAutoState = EnGuildPlayState.None;
    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
    {
		Debug.Log("OnEventStartAutoPlay       enGuildPlayAutoState = " + enGuildPlayAutoState + " // IsThisAutoPlaying() = " + IsThisAutoPlaying());
		if (enGuildPlayAutoState == EnGuildPlayState.Defense || enGuildPlayAutoState == EnGuildPlayState.Altar)
		{
			m_enGuildPlayAutoState = enGuildPlayAutoState;
			if (IsThisAutoPlaying() == false)
			{				
				Player.SetAutoPlay(this, true);
			}

			SetDisplayPath();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
    {
		Debug.Log("1. CsPlayThemeQuestGuildAltar.OnEventStopAutoPlay() + " + enGuildPlayAutoState);
		if (IsThisAutoPlaying())
		{
			if (m_enGuildPlayAutoState == enGuildPlayAutoState)
			{
				Debug.Log("2. CsPlayThemeQuestGuildAltar.OnEventStopAutoPlay() + " + enGuildPlayAutoState);
				Player.SetAutoPlay(null, false);
			}
		}
    }

    #endregion Event.Auto

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		Debug.Log("SetDisplayPath()     m_enGuildPlayAutoState = " + m_enGuildPlayAutoState);
		if (m_enGuildPlayAutoState == EnGuildPlayState.Defense) // 제단 수비 퀘스트 Auto.
		{
			Player.DisplayPath.SetPath(m_csGuildManager.DefenseMonsterPos);
			Player.SetWayPoint(m_csGuildManager.DefenseMonsterPos, EnWayPointType.Battle, 30);
		}
		else if (m_enGuildPlayAutoState == EnGuildPlayState.Altar)
		{
			Player.DisplayPath.SetPath(CsGameData.Instance.GuildAltar.GuildTerritoryNpc.Position);
			Player.SetWayPoint(CsGameData.Instance.GuildAltar.GuildTerritoryNpc.Position, EnWayPointType.Npc, CsGameData.Instance.GuildAltar.GuildTerritoryNpc.InteractionMaxRange);
		}
	}

    //---------------------------------------------------------------------------------------------------
	void PlayDefense(long lMonsterInstanceId, Vector3 vtMonsterPos)
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csMyHeroInfo.LocationId, vtMonsterPos, 30, false))
			{
				Player.PlayBattle(lMonsterInstanceId, Player.transform.position, 30f);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PlayAltar()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csMyHeroInfo.LocationId, m_csGuildTerritoryNpc.Position, m_csGuildTerritoryNpc.InteractionMaxRange, true))
			{
				CheckAltarDialog();
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void CheckAltarDialog()
    {
		if (Player.IsTargetInDistance(m_csGuildTerritoryNpc.Position, m_csGuildTerritoryNpc.InteractionMaxRange))
		{
			Debug.Log("CsPlayThemeQuestGuildAltar.CheckAltarDialog()");
			Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
			Player.IsQuestDialog = true;
			Player.LookAtPosition(m_csGuildTerritoryNpc.Position);
			m_csGuildManager.GuildAltarNPCDialog();
			if (IsThisAutoPlaying() && m_csGuildManager.GuildPlayAutoState == EnGuildPlayState.Altar)
			{
				Player.SetAutoPlay(null, true);
			}
		}
    }
}
