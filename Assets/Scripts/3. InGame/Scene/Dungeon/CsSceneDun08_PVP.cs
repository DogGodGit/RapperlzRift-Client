using ClientCommon;
using System;
using UnityEngine;

public class CsSceneDun08_PVP : CsSceneIngameDungeon 
{
	CsHeroClone m_csHeroClone;

    //---------------------------------------------------------------------------------------------------
    protected override void Start()
    {
        base.Start();
		m_csDungeonManager = CsDungeonManager.Instance;
		m_csDungeonManager.EventContinentExitForFieldOfHonorChallenge += OnEventContinentExitForFieldOfHonorChallenge;
		m_csDungeonManager.EventFieldOfHonorChallenge += OnEventFieldOfHonorChallenge;
		m_csDungeonManager.EventFieldOfHonorStart += OnEventFieldOfHonorStart;
		m_csDungeonManager.EventFieldOfHonorClear += OnEventFieldOfHonorClear;

		m_csDungeonManager.SendFieldOfHonorChallenge(); // 입장 요청.
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnDestroy()
    {
		if (m_bApplicationQuit) return;
		DungeonExit();

		m_csDungeonManager.EventContinentExitForFieldOfHonorChallenge -= OnEventContinentExitForFieldOfHonorChallenge;
		m_csDungeonManager.EventFieldOfHonorChallenge -= OnEventFieldOfHonorChallenge;
		m_csDungeonManager.EventFieldOfHonorStart -= OnEventFieldOfHonorStart;
		m_csDungeonManager.EventFieldOfHonorClear -= OnEventFieldOfHonorClear;
		m_csDungeonManager.ResetDungeon();

		m_csDungeonManager = null;
		m_csHeroClone = null;
		base.OnDestroy();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventContinentExitForFieldOfHonorChallenge()
    {
        Debug.Log("   OnEventContinentExitForFieldOfHonorChallenge()   ");
        DungeonEnter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorChallenge(Guid guidPlaceInstanceId, PDVector3 pDVector3, float flRotationY, PDHero pDTargetHero)
    {
        Debug.Log("   OnEventFieldOfHonorChallenge    ");
        SetMyHeroLocation(m_csDungeonManager.FieldOfHonor.LocationId);
        SetMyHero(pDVector3, flRotationY, guidPlaceInstanceId, false);
		CreateHeroClone(pDTargetHero, guidPlaceInstanceId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorStart()
    {
        Debug.Log("OnEventFieldOfHonorStart");
		m_csHeroClone.StartBattle();
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventFieldOfHonorClear(bool bLevelUp, long lAcquiredExp, int nHonorPoint)
    {
		Debug.Log("CsSceneDun08_PVP   OnEventFieldOfHonorClear  $$$");
        m_csHeroClone.ChangeState(CsHero.EnState.Dead);
    }

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventEvtHeroHit(SEBHeroHitEventBody csEvt) // 당사자 포함 관련유저. 유저 피격시.
	{
		//Debug.Log("@@@@@@@@@@@@@@@         CsSceneDun08_PVP.OnEventEvtHeroHit           @@@@@@@@@@@@@@@@@@@@@@@@@");
		Transform trTarget = m_csPlayer.transform;

		if (m_csHeroClone != null && m_csHeroClone.HeroId == csEvt.heroId) // 대상이 클론일때.
		{
			trTarget = m_csHeroClone.transform;
		}

		if (csEvt.hitResult.hp == 0)
		{
			if (trTarget == m_csHeroClone.transform)
			{
				m_csHeroClone.NetEventHeroDead(GetAttacker(csEvt.hitResult.attacker), csEvt.hitResult);
			}
			else
			{
				m_csPlayer.NetEventHeroDead(GetAttacker(csEvt.hitResult.attacker), csEvt.hitResult);
			}
		}

		Guid guidHeroId = GetHeroId(csEvt.hitResult.attacker);

		if (m_csHeroClone != null && m_csHeroClone.HeroId == guidHeroId) // 클론일 경우.
		{
			m_csHeroClone.NetEventHitApprove(csEvt.hitResult, trTarget);
		}
		else if (m_csPlayer.HeroId == guidHeroId)
		{
			m_csPlayer.NetEventHitApprove(csEvt.hitResult, trTarget);
		}
	}

	//----------------------------------------------------------------------------------------------------
	void CreateHeroClone(PDHero pDHero, Guid guidPlaceInstanceId)
	{
		//Debug.Log("CreateHeroClone()");
		if (pDHero == null)
		{
			pDHero = CsHeroClone.TempMakePDHero(m_csPlayer.transform.position + Vector3.left);
		}
		Transform trOtherPlayer = transform.Find("OtherPlayer");
		string str = "Prefab/Player/Clone/" + (EnJob)pDHero.jobId; // (EnJob)CsGameData.Instance.MyHeroInfo.Job.JobId;
		GameObject go = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>(str), trOtherPlayer);
		m_csHeroClone = go.transform.GetComponent<CsHeroClone>();

		m_csHeroClone.Init(pDHero, guidPlaceInstanceId);
	}


	//----------------------------------------------------------------------------------------------------
	protected override void OnEventHeroAbnormalStateEffectStart(SEBHeroAbnormalStateEffectStartEventBody csEvt) // 당사자 포함 관련 유저
	{
		// 패킷데이터 변경으로 인한 주석처리
		//if (m_csHeroClone != null && m_csHeroClone.HeroId == csEvt.heroId)
		//{
		//    m_csHeroClone.NetEventHeroAbnormalStateEffectStart(csEvt.abnormalStateEffectInstanceId,
		//                                                       csEvt.abnormalStateId,
		//                                                       csEvt.sourceJobId,
		//                                                       csEvt.level,
		//                                                       csEvt.damageAbsorbShieldRemainingAbsorbAmount,
		//                                                       csEvt.remainingTime,
		//                                                       csEvt.damageAbsorbShieldRemainingAbsorbAmount,
		//                                                       csEvt.removedAbnormalStateEffects);
		//}
		//else if (m_csPlayer.HeroId == csEvt.heroId)
		//{
		//    m_csPlayer.NetEventHeroAbnormalStateEffectStart(csEvt.abnormalStateEffectInstanceId,
		//                                                    csEvt.abnormalStateId,
		//                                                    csEvt.sourceJobId,
		//                                                    csEvt.level,
		//                                                    csEvt.damageAbsorbShieldRemainingAbsorbAmount,
		//                                                    csEvt.remainingTime,
		//                                                    csEvt.damageAbsorbShieldRemainingAbsorbAmount,
		//                                                    csEvt.removedAbnormalStateEffects);
		//}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void OnEventHeroAbnormalStateEffectHit(SEBHeroAbnormalStateEffectHitEventBody csEvt) // 당사자 포함 관련 유저
	{
		if (m_csHeroClone != null && m_csHeroClone.HeroId == csEvt.heroId)
		{
			m_csHeroClone.NetEventHeroAbnormalStateEffectHit(csEvt.hp,
															 csEvt.removedAbnormalStateEffects,
															 csEvt.abnormalStateEffectInstanceId,
															 csEvt.damage, csEvt.hpDamage,
															 GetAttacker(csEvt.attacker));

		}
		else if (m_csPlayer.HeroId == csEvt.heroId)
		{
			m_csPlayer.NetEventHeroAbnormalStateEffectHit(csEvt.hp,
														  csEvt.removedAbnormalStateEffects,
														  csEvt.abnormalStateEffectInstanceId,
														  csEvt.damage, csEvt.hpDamage,
														  GetAttacker(csEvt.attacker));
		}
	}

	//----------------------------------------------------------------------------------------------------
	protected override void EventHeroAbnormalStateEffectFinished(SEBHeroAbnormalStateEffectFinishedEventBody csEvt) // 당사자 포함 관련 유저
	{
		if (m_csHeroClone != null && m_csHeroClone.HeroId == csEvt.heroId)
		{
			m_csHeroClone.NetEventHeroAbnormalStateEffectFinished(csEvt.abnormalStateEffectInstanceId);
		}
		else if (m_csPlayer.HeroId == csEvt.heroId)
		{
			m_csPlayer.NetEventHeroAbnormalStateEffectFinished(csEvt.abnormalStateEffectInstanceId);
		}
	}

    //---------------------------------------------------------------------------------------------------
    public override void InitPlayThemes()
    {
        base.InitPlayThemes();
        AddPlayTheme(new CsPlayThemeDungeonFieldOfHonor());
    }
}