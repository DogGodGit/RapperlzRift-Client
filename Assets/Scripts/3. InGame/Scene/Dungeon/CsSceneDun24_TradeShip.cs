using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun24_TradeShip : CsSceneIngameDungeon
{
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForTradeShipEnter += OnEventContinentExitForTradeShipEnter;
		m_csDungeonManager.EventTradeShipEnter += OnEventTradeShipEnter;
		m_csDungeonManager.EventTradeShipStepStart += OnEventTradeShipStepStart;
		m_csDungeonManager.EventTradeShipRevive += OnEventTradeShipRevive;
		m_csDungeonManager.EventTradeShipClear += OnEventTradeShipClear;
		m_csDungeonManager.EventTradeShipFail += OnEventTradeShipFail;

		m_csDungeonManager.TradeShipEnter();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventContinentExitForTradeShipEnter -= OnEventContinentExitForTradeShipEnter;
		m_csDungeonManager.EventTradeShipEnter -= OnEventTradeShipEnter;
		m_csDungeonManager.EventTradeShipStepStart -= OnEventTradeShipStepStart;
		m_csDungeonManager.EventTradeShipRevive -= OnEventTradeShipRevive;
		m_csDungeonManager.EventTradeShipClear -= OnEventTradeShipClear;
		m_csDungeonManager.EventTradeShipFail -= OnEventTradeShipFail;

		m_csDungeonManager.ResetDungeon();
		m_csDungeonManager = null;

		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroHit(SEBHeroHitEventBody csEvt)
	{
		base.OnEventEvtHeroHit(csEvt);
		if (csEvt.heroId != m_csPlayer.HeroId) // 타유저 피격시 정보 갱신.
		{
			Debug.Log("CsSceneDun24_TradeShip.OnEventEvtHeroHit               파티원 HP 정보 변경");
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroLevelUp(SEBHeroLevelUpEventBody csEvt) // 당사자 외 레벨업. 
	{
		base.OnEventEvtHeroLevelUp(csEvt);
		if (m_dicHeros.ContainsKey(csEvt.heroId))
		{
			Debug.Log("CsSceneDun24_TradeShip.OnEventEvtHeroLevelUp          파티원 레벨업.");
			m_csDungeonManager.UpdateDungeonMember();
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroExit(SEBHeroExitEventBody csEvt)
	{
		base.OnEventEvtHeroExit(csEvt);
		m_csDungeonManager.OnEventPartyDungeonHeroExit(csEvt.heroId);
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventContinentExitForTradeShipEnter()
	{
		DungeonEnter();
	}

	//----------------------------------------------------------------------------------------------------
    void OnEventTradeShipEnter(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero[] pdHero, PDMonsterInstance[] aMonsterInstance, int nDifficulty)
	{
		Debug.Log(" CsSceneDun24_TradeShip.OnEventTradeShipEnter() ");
		SetMyHeroLocation(m_csDungeonManager.TradeShip.LocationId);
		SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);

		StartCoroutine(DirectingByDungeonEnter());

		for (int i = 0; i < pdHero.Length; i++)
		{
			StartCoroutine(AsyncCreateHero(pdHero[i], false, false));
		}

		for (int i = 0; i < aMonsterInstance.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(aMonsterInstance[i]));
		}

		for (int i = 0; i < m_csDungeonManager.TradeShip.TradeShipObstacleList.Count; i++)
		{
			CsTradeShipObstacle csTradeShipObstacle = m_csDungeonManager.TradeShip.TradeShipObstacleList[i];
			if (m_csDungeonManager.TradeShipStep != null)
			{
				if (m_csDungeonManager.TradeShipStep.StepNo >= csTradeShipObstacle.RemoveStepNo) // 이전 스텝의 장애물 제거.
				{
					continue;
				}
			}
			CreateObstacle(csTradeShipObstacle.ObstacleId, new Vector3(csTradeShipObstacle.XPosition, csTradeShipObstacle.YPosition, csTradeShipObstacle.ZPosition), csTradeShipObstacle.YRotation, csTradeShipObstacle.Scale);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventTradeShipStepStart(PDTradeShipMonsterInstance[] apMonInst, PDTradeShipAdditionalMonsterInstance[] apAddMonInst, PDTradeShipObjectInstance[] apObjInst)
	{
		if (m_csDungeonManager.TradeShipStep != null)
		{
			for (int i = 0; i < m_csDungeonManager.TradeShip.TradeShipObstacleList.Count; i++)
			{
				CsTradeShipObstacle csTradeShipObstacle = m_csDungeonManager.TradeShip.TradeShipObstacleList[i];

				if (csTradeShipObstacle.RemoveStepNo == m_csDungeonManager.TradeShipStep.StepNo)
				{
					RemoveObstacle(csTradeShipObstacle.ObstacleId);
				}
			}
		}

		for (int i = 0; i < apObjInst.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apObjInst[i]));
		}

		for (int i = 0; i < apMonInst.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apMonInst[i]));
		}

		for (int i = 0; i < apAddMonInst.Length; i++)
		{
			StartCoroutine(AsyncCreateMonster(apAddMonInst[i]));
		}
	}

	//----------------------------------------------------------------------------------------------------
	void OnEventTradeShipRevive()
	{
		Debug.Log(" CsSceneDun24_TradeShip.OnEventTradeShipRevive() ");
		m_csPlayer.NetEventRevive();
	}

	//---------------------------------------------------------------------------------------------------
    void OnEventTradeShipClear(bool bLevelUp, long lAcquiredExp, PDItemBooty aPDItemBooty)
	{
		Debug.Log(" OnEventTradeShipClear");
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
		StartCoroutine(DungeonClearDirection(new Vector3(0.86f, 4.06f, 119.33f), 180));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventTradeShipFail(bool bLevelUp, long lAcquiredExp)
	{
		Debug.Log(" OnEventTradeShipFail() ");
		foreach (var dicMonsters in m_dicMonsters)
		{
			dicMonsters.Value.NetEventDungeonClear();
		}
	}

	//----------------------------------------------------------------------------------------------------
	//public override void StartClearDiretion()
	//{
	//	//if (m_bClearDiretion) return;

	//	//Debug.Log("##############  StartClearDiretion()  override ################ ");
	//	//m_bClearDiretion = true;
	//	//ClearHero();
	//	//StartCoroutine(DungeonClearDirection(new Vector3(0.86f, 4.06f, 119.33f), 180));
	//}

	//----------------------------------------------------------------------------------------------------
	public override void InitPlayThemes()
	{
		base.InitPlayThemes();
		AddPlayTheme(new CsPlayThemeDungeonTradeShip());
	}
}
