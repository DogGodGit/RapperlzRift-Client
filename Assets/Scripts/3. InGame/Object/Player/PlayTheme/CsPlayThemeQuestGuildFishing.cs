using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildFishing : CsPlayThemeQuest
{
    CsGuildManager m_csGuildManager;
	CsGuildTerritoryNpc m_csFishingNpc;
	bool m_bArrival = false;

    #region IAutoPlay

    //---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{			
			CheckFoodWareHouseDialog();
			m_bArrival = true;
		}
	}

    #endregion IAutoPlay

    #region Override
    //----------------------------------------------------------------------------------------------------
    public override void Init(CsMyPlayer csPlayer)
    {
        base.Init(csPlayer);
        
        m_csGuildManager = CsGuildManager.Instance;
        m_csGuildManager.EventStartAutoPlay += OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay += OnEventStopAutoPlay;

        Player.EventArrivalMoveByTouch += OnEventArrivalMoveByTouch;

		m_csFishingNpc = m_csGuildManager.GuildTerritory.GetGuildTerritoryNpc(CsGameConfig.Instance.GuildBlessingGuildTerritoryNpcId);
	}

    //----------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;

        Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

        m_csGuildManager = null;
		m_csFishingNpc = null;

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
                m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.Fishing);
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
					PlayFoodWareHouse();
				}
			}
		}
		m_bArrival = false;
	}

    #endregion Override

    //----------------------------------------------------------------------------------------------------
    void OnEventArrivalMoveByTouch(bool bMoveByTouchTarget)
    {
        if (bMoveByTouchTarget)
        {
			CheckFoodWareHouseDialog();
        }
    }

    #region Event.Auto
    //---------------------------------------------------------------------------------------------------
    void OnEventStartAutoPlay(EnGuildPlayState enGuildPlayAutoState)
    {
        if (enGuildPlayAutoState == EnGuildPlayState.Fishing)
        {
			Debug.Log("OnEventStartAutoPlay      enGuildPlayAutoState = " + enGuildPlayAutoState);
			SetDisplayPath();
            Player.SetAutoPlay(this, true);
			PlayFoodWareHouse();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventStopAutoPlay(object objCaller, EnGuildPlayState enGuildPlayAutoState)
    {
		if (IsThisAutoPlaying())
		{
			Debug.Log("m_csGuildManager.OnEventStopAutoPlay()");
			Player.SetAutoPlay(null, false);
		}
    }

    #endregion Event.Auto

	//---------------------------------------------------------------------------------------------------
	void SetDisplayPath()
	{		
		Player.DisplayPath.SetPath(m_csFishingNpc.Position);
		Player.SetWayPoint(m_csFishingNpc.Position, EnWayPointType.Npc, m_csFishingNpc.InteractionMaxRange);
	}

	//----------------------------------------------------------------------------------------------------
	void PlayFoodWareHouse()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csMyHeroInfo.LocationId, m_csFishingNpc.Position, m_csFishingNpc.InteractionMaxRange, true))
			{
				CheckFoodWareHouseDialog();
			}
		}
	}

    //----------------------------------------------------------------------------------------------------
    void CheckFoodWareHouseDialog()
	{		
		if (Player.IsTargetInDistance(m_csFishingNpc.Position, m_csFishingNpc.InteractionMaxRange))
        {
			Player.IsQuestDialog = true;
			Player.LookAtPosition(m_csFishingNpc.Position);

			if (m_csGuildManager.MyGuildMemberGrade.GuildBlessingBuffEnabled)
			{
				m_csGuildManager.FishingDialog(m_csFishingNpc.NpcId);
			}

			CsIngameData.Instance.IngameManagement.NpcDialog(m_csFishingNpc.NpcId);
        }
    }
}
