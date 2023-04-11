using System.Collections;
using UnityEngine;

public class CsPlayThemeQuestGuildFoodWareHouse : CsPlayThemeQuest
{
    CsGuildManager m_csGuildManager;
	bool m_bArrival = false;

    #region IAutoPlay

    //---------------------------------------------------------------------------------------------------
	public override void ArrivalMoveToPos()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			Debug.Log("1. CsPlayThemeQuestGuildFarm.ArrivalMoveToPos");
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
    }

    //----------------------------------------------------------------------------------------------------
    public override void Uninit()
    {
        m_csGuildManager.EventStartAutoPlay -= OnEventStartAutoPlay;
        m_csGuildManager.EventStopAutoPlay -= OnEventStopAutoPlay;

        Player.EventArrivalMoveByTouch -= OnEventArrivalMoveByTouch;

        m_csGuildManager = null;
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
                m_csGuildManager.StopAutoPlay(this, EnGuildPlayState.FoodWareHouse);
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
        if (enGuildPlayAutoState == EnGuildPlayState.FoodWareHouse)
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
		Player.DisplayPath.SetPath(CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc.Position);
		Player.SetWayPoint(CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc.Position, EnWayPointType.Npc, CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc.InteractionMaxRange);
	}

	//----------------------------------------------------------------------------------------------------
	void PlayFoodWareHouse()
	{
		if (CsIngameData.Instance.PlayerAutoMode == EnPlayerAutoMode.Auto)
		{
			CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc;

			if (MovePlayer(m_csMyHeroInfo.InitEntranceLocationParam, m_csMyHeroInfo.LocationId, csGuildTerritoryNpc.Position, csGuildTerritoryNpc.InteractionMaxRange, true))
			{
				CheckFoodWareHouseDialog();
			}
		}
	}

    //----------------------------------------------------------------------------------------------------
    void CheckFoodWareHouseDialog()
	{
		CsGuildTerritoryNpc csGuildTerritoryNpc = CsGameData.Instance.GuildFoodWarehouse.GuildTerritoryNpc;
		if (Player.IsTargetInDistance(csGuildTerritoryNpc.Position, csGuildTerritoryNpc.InteractionMaxRange))
        {
			Player.IsQuestDialog = true;
			Player.LookAtPosition(csGuildTerritoryNpc.Position);
			m_csGuildManager.GuildFoodWareHouseDialog();
			CsIngameData.Instance.IngameManagement.NpcDialog(csGuildTerritoryNpc.NpcId);
        }
    }
}
