using ClientCommon;
using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildFarm : CsPlayThemeQuest
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
			Debug.Log("1. CsPlayThemeQuestGuildFarm.ArrivalMoveToPos");
			PlayGuildFarmQuest();
			m_bArrival = true;
		}
	}

    #endregion IAutoPlay

    #region Override
    //---------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        m_csGuildManager = CsGuildManager.Instance;
        m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;

        m_csGuildManager.EventUpdateFarmQuestState += OnEventUpdateState;
		m_csGuildManager.EventGuildFarmQuestAccept += OnEventGuildFarmQuestAccept;
        m_csGuildManager.EventGuildFarmQuestComplete += OnEventGuildFarmQuestComplete;
        m_csGuildManager.EventGuildFarmQuestAbandon += OnEventGuildFarmQuestAbandon;

		m_csGuildManager.EventGuildFarmQuestInteractionStart += OnEventGuildFarmQuestInteractionStart;
        m_csGuildManager.EventGuildFarmQuestInteractionCompleted += OnEventGuildFarmQuestInteractionCompleted;
        m_csGuildManager.EventGuildFarmQuestInteractionCanceled += OnEventGuildFarmQuestInteractionCanceled;

		Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;
    }

    //---------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;

        m_csGuildManager.EventUpdateFarmQuestState -= OnEventUpdateState;
		m_csGuildManager.EventGuildFarmQuestAccept -= OnEventGuildFarmQuestAccept;
		m_csGuildManager.EventGuildFarmQuestComplete -= OnEventGuildFarmQuestComplete;
        m_csGuildManager.EventGuildFarmQuestAbandon -= OnEventGuildFarmQuestAbandon;

		m_csGuildManager.EventGuildFarmQuestInteractionStart -= OnEventGuildFarmQuestInteractionStart;
        m_csGuildManager.EventGuildFarmQuestInteractionCompleted -= OnEventGuildFarmQuestInteractionCompleted;
        m_csGuildManager.EventGuildFarmQuestInteractionCanceled -= OnEventGuildFarmQuestInteractionCanceled;

		Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

        m_csGuildManager = null;
		m_csGuildTerritoryNpc = null;		
        base.Uninit();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void StopAutoPlay(bool bNotify)
    {
        if (Player.AutoPlay == this)
		{
			Player.IsQuestDialog = false;
            if (bNotify)
            {
                m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.FarmQuest);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	public override void StateChangedToIdle()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (IsThisAutoPlaying())
			{
				Debug.Log("m_csGuildManager.StateChangedToIdle()");
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
				if (m_bArrival == false)
				{
					PlayGuildFarmQuest();
				}
			}
		}
		m_bArrival = false;
	}

    #endregion Override

    #region Event.Player

    //---------------------------------------------------------------------------------------------------
    void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
    {
        if (bMoveByTouchTarget)
		{
			CheckNpcDialog();
        }
    }

    #endregion Event.Player

	#region Event.GuildManager
	//---------------------------------------------------------------------------------------------------
	void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
	{
		if (enGuildPlayAutoState == EnGuildPlayState.FarmQuest)
		{
			Debug.Log("OnEventStartAutoPlay      enGuildPlayAutoState  = " + enGuildPlayAutoState+"  GuildFarmQuestState = "+m_csGuildManager.GuildFarmQuestState);
			Player.SetAutoPlay(this, true);
			SetDisplayPath();
			PlayGuildFarmQuest();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
	{
		if (IsThisAutoPlaying())
		{
			if (enGuildPlayAutoState == EnGuildPlayState.FarmQuest)
			{
				Debug.Log("m_csGuildManager.OnEventStopAutoPlay()");
				Player.SetAutoPlay(null, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("m_csGuildManager.OnEventUpdateState()");
			SetDisplayPath();
			PlayGuildFarmQuest();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildFarmQuestAccept(PDHeroGuildFarmQuest pDHeroGuildFarmQuest)
	{
		if (IsThisAutoPlaying())
		{
			Debug.Log("m_csGuildManager.OnEventUpdateState()");
			SetDisplayPath();
			PlayGuildFarmQuest();
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestComplete(bool bLevelUp, long lAcquiredExp)
    {
		if (IsThisAutoPlaying())
        {
            Debug.Log("OnEventGuildFarmQuestComplete()");
            Player.SetAutoPlay(null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestAbandon()
    {
		if (IsThisAutoPlaying())
        {
            Debug.Log("OnEventGuildFarmQuestAbandon()");
			Player.SetAutoPlay(null, true);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventGuildFarmQuestInteractionStart()
	{
		Debug.Log(" OnEventGuildFarmQuestInteractionStart() ");
		Player.ResetBattleMode();
		if (Player.SkillStatus.IsStatusPlayAnim())
		{
			if (CsGuildManager.Instance.Interaction)
			{
				Debug.Log("OnEventGuildFarmQuestInteractionStart    CsGuildManager.Instance.GuildInteractionCancel()");
				CsGuildManager.Instance.GuildInteractionCancel();
			}
			return;
		}

		Player.ChangeState(CsHero.EnState.Interaction);
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCompleted()
    {
        Debug.Log(" OnEventGuildFarmQuestInteractionCompleted() ");
		m_csPlayer.ChangeState(CsHero.EnState.Idle);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFarmQuestInteractionCanceled()
    {
        Debug.Log(" OnEventGuildFarmQuestInteractionCanceled() ");
        m_csPlayer.ChangeState(CsHero.EnState.Idle);
    }

    #endregion Event.GuildManager

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{
		m_csGuildTerritoryNpc = GetGuildTerritoryNpcInfo();
		Player.DisplayPath.SetPath(m_csGuildTerritoryNpc.Position);
		Player.SetWayPoint(m_csGuildTerritoryNpc.Position, EnWayPointType.Npc, m_csGuildTerritoryNpc.InteractionMaxRange);
	}

    //---------------------------------------------------------------------------------------------------
    void PlayGuildFarmQuest()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (m_csGuildManager.Interaction) return; // 상호작용중 or 대화중

			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csMyHeroInfo.LocationId, m_csGuildTerritoryNpc.Position, m_csGuildTerritoryNpc.InteractionMaxRange, true))
			{
				CheckNpcDialog();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckNpcDialog()
	{
		m_csGuildTerritoryNpc = GetGuildTerritoryNpcInfo();

		if (m_csGuildManager.GuildFarmQuestState == EnGuildFarmQuestState.Accepted) // 퀘스트 수락 or 완료
		{
			if (Player.IsTargetInDistance(m_csGuildTerritoryNpc.Position, m_csGuildTerritoryNpc.InteractionMaxRange))
			{
				Player.LookAtPosition(m_csGuildTerritoryNpc.Position);
				CsIngameData.Instance.IngameManagement.NpcDialog(m_csGuildTerritoryNpc.NpcId);
				Player.ChangeTransformationState(CsHero.EnTransformationState.None, true);
			}
		}
		else
		{
			if (Player.IsTargetInDistance(m_csGuildTerritoryNpc.Position, m_csGuildTerritoryNpc.InteractionMaxRange))
			{
				Debug.Log("CheckNpcDialog   >>   GuildFarmQuestNPCDialog()   Player.IsQuestDialog = "+ Player.IsQuestDialog);
				
				if (m_csGuildManager.GuildFarmQuestState != EnGuildFarmQuestState.Competed)
				{
					Player.LookAtPosition(m_csGuildTerritoryNpc.Position);
					CsIngameData.Instance.IngameManagement.NpcDialog(m_csGuildTerritoryNpc.NpcId);
					Player.IsQuestDialog = true;
					m_csGuildManager.GuildFarmQuestNPCDialog();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsGuildTerritoryNpc GetGuildTerritoryNpcInfo()
	{
		if (m_csGuildManager.GuildFarmQuestState == EnGuildFarmQuestState.Accepted) // 미션수행 진행.
		{
			return CsGameData.Instance.GuildFarmQuest.TargetGuildTerritoryNpc;
		}
		else // 수락전 상태.
		{
			return CsGameData.Instance.GuildFarmQuest.QuestGuildTerritoryNpc;
		}
	}

}
