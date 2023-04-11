using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using SimpleDebugLog;
using System;


public class CsCartManager
{
	bool m_bWaitResponse = false;
	bool m_bMyHeroRidingCart = false;
	bool m_bMyHeroAccelerate = false;

	//---------------------------------------------------------------------------------------------------
	public static CsCartManager Instance
	{
		get { return CsSingleton<CsCartManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventAutoMoveToCart;
	public event Delegate EventRemoveMyCart;
	public event Delegate<PDCartInstance> EventMyHeroCartGetOn;
	public event Delegate EventMyHeroCartGetOff;
	public event Delegate<bool, float> EventCartAccelerate;
	public event Delegate<int> EventCartPortalEnter;
	public event Delegate<PDContinentEntranceInfo> EventCartPortalExit;

	public event Delegate<PDCartInstance> EventCartEnter;
	public event Delegate<long> EventCartExit;
	public event Delegate<Guid, PDCartInstance> EventCartGetOn;
	public event Delegate<PDHero, long> EventCartGetOff;
	public event Delegate<long> EventCartHighSpeedStart;
	public event Delegate<long> EventCartHighSpeedEnd;
	public event Delegate EventMyCartHighSpeedEnd;

	public event Delegate<PDCartInstance> EventCartInterestAreaEnter;
	public event Delegate<long> EventCartInterestAreaExit;
	public event Delegate<long, PDVector3, float> EventCartMove;

	public event Delegate<long, int> EventCartChanged;
	public event Delegate<long, PDHitResult> EventCartHit;
	public event Delegate<long, long, int, int,int, int, float, long[]> EventCartAbnormalStateEffectStart;
	public event Delegate<long,int,long[],long,int,int,Transform> EventCartAbnormalStateEffectHit;
	public event Delegate<long, long> EventCartAbnormalStateEffectFinished;

	//---------------------------------------------------------------------------------------------------
	public bool IsMyHeroRidingCart
    {
        get { return m_bMyHeroRidingCart; }
        set
        {
            m_bMyHeroRidingCart = value;

            ////카트 튜토리얼 체크
            //if (m_bMyHeroRidingCart && CsConfiguration.Instance.GetTutorialKey(EnTutorialType.Cart))
            //{
            //    CsGameEventUIToUI.Instance.OnEventReferenceTutorial(EnTutorialType.Cart);
            //}
        }
    }

	public bool MyHeroAccelerate
	{
		get { return m_bMyHeroAccelerate; }
		set { m_bMyHeroAccelerate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		UnInit();

		// Command 
		CsRplzSession.Instance.EventResCartGetOn += OnEventResCartGetOn;
		CsRplzSession.Instance.EventResCartGetOff += OnEventResCartGetOff;
		CsRplzSession.Instance.EventResCartAccelerate += OnEventResCartAccelerate;
		CsRplzSession.Instance.EventResCartPortalEnter += OnEventResCartPortalEnter;
		CsRplzSession.Instance.EventResCartPortalExit += OnEventResCartPortalExit;

		// Event
		CsRplzSession.Instance.EventEvtCartEnter += OnEventEvtCartEnter;
		CsRplzSession.Instance.EventEvtCartExit += OnEventEvtCartExit;
		CsRplzSession.Instance.EventEvtCartGetOn += OnEventEvtCartGetOn;
		CsRplzSession.Instance.EventEvtCartGetOff += OnEventEvtCartGetOff;
		CsRplzSession.Instance.EventEvtCartHighSpeedStart += OnEventEvtCartHighSpeedStart;
		CsRplzSession.Instance.EventEvtCartHighSpeedEnd += OnEventEvtCartHighSpeedEnd;
		CsRplzSession.Instance.EventEvtMyCartHighSpeedEnd += OnEventEvtMyCartHighSpeedEnd;
		CsRplzSession.Instance.EventEvtCartInterestAreaEnter += OnEventEvtCartInterestAreaEnter;
		CsRplzSession.Instance.EventEvtCartInterestAreaExit += OnEventEvtCartInterestAreaExit;
		CsRplzSession.Instance.EventEvtCartMove += OnEventEvtCartMove;

		CsRplzSession.Instance.EventEvtCartChanged += OnEventEvtCartChanged;
		CsRplzSession.Instance.EventEvtCartHit += OnEventEvtCartHit;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectStart += OnEventEvtCartAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectHit += OnEventEvtCartAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectFinished += OnEventEvtCartAbnormalStateEffectFinished;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command 
		CsRplzSession.Instance.EventResCartGetOn -= OnEventResCartGetOn;
		CsRplzSession.Instance.EventResCartGetOff -= OnEventResCartGetOff;
		CsRplzSession.Instance.EventResCartAccelerate -= OnEventResCartAccelerate;
		CsRplzSession.Instance.EventResCartPortalEnter -= OnEventResCartPortalEnter;
		CsRplzSession.Instance.EventResCartPortalExit -= OnEventResCartPortalExit;

		// Event
		CsRplzSession.Instance.EventEvtCartEnter -= OnEventEvtCartEnter;
		CsRplzSession.Instance.EventEvtCartExit -= OnEventEvtCartExit;
		CsRplzSession.Instance.EventEvtCartGetOn -= OnEventEvtCartGetOn;
		CsRplzSession.Instance.EventEvtCartGetOff -= OnEventEvtCartGetOff;

		CsRplzSession.Instance.EventEvtCartHighSpeedStart -= OnEventEvtCartHighSpeedStart;
		CsRplzSession.Instance.EventEvtCartHighSpeedEnd -= OnEventEvtCartHighSpeedEnd;
		CsRplzSession.Instance.EventEvtMyCartHighSpeedEnd -= OnEventEvtMyCartHighSpeedEnd;
		CsRplzSession.Instance.EventEvtCartInterestAreaEnter -= OnEventEvtCartInterestAreaEnter;
		CsRplzSession.Instance.EventEvtCartInterestAreaExit -= OnEventEvtCartInterestAreaExit;
		CsRplzSession.Instance.EventEvtCartMove -= OnEventEvtCartMove;

		CsRplzSession.Instance.EventEvtCartChanged -= OnEventEvtCartChanged;
		CsRplzSession.Instance.EventEvtCartHit -= OnEventEvtCartHit;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectStart -= OnEventEvtCartAbnormalStateEffectStart;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectHit -= OnEventEvtCartAbnormalStateEffectHit;
		CsRplzSession.Instance.EventEvtCartAbnormalStateEffectFinished -= OnEventEvtCartAbnormalStateEffectFinished;


		m_bWaitResponse = false;
		m_bMyHeroRidingCart = false;
		m_bMyHeroAccelerate = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void CartPortalEnter(int nPortalId)
	{
		SendCartPortalEnter(nPortalId);
	}

	#region Public Event

	//---------------------------------------------------------------------------------------------------
	public void SendCartMoveStartEvent(Guid guidPlaceIntanceId)
	{
		//dd.d("!!!!!!!!!!!!!!!!!!!!!!!  SendCartMoveStartEvent  @@@@@@@@@@@@@@@@@@@@@@@");
		CEBCartMoveStartEventBody csEvet = new CEBCartMoveStartEventBody();
		csEvet.placeInstanceId = guidPlaceIntanceId;
		CsRplzSession.Instance.Send(ClientEventName.CartMoveStart, csEvet);
	}

	//---------------------------------------------------------------------------------------------------
	public void SendCartMoveEvent(Guid guidPlaceIntanceId, Vector3 vtPos, float flRotationY)
	{
		CEBCartMoveEventBody csEvet = new CEBCartMoveEventBody();
		csEvet.placeInstanceId = guidPlaceIntanceId;
		csEvet.position = CsRplzSession.Translate(vtPos);
		csEvet.rotationY = flRotationY;
		CsRplzSession.Instance.Send(ClientEventName.CartMove, csEvet);
	}

	//---------------------------------------------------------------------------------------------------
	public void SendCartMoveEndEvent(Guid guidPlaceIntanceId)
	{
		//dd.d("!!!!!!!!!!!!!!!!!!!!!!!  SendCartMoveEndEvent  @@@@@@@@@@@@@@@@@@@@@@@");
		CEBCartMoveEndEventBody csEvet = new CEBCartMoveEndEventBody();
		csEvet.placeInstanceId = guidPlaceIntanceId;
		CsRplzSession.Instance.Send(ClientEventName.CartMoveEnd, csEvet);
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveMyCart()
	{
		if (EventRemoveMyCart != null)
		{
			EventRemoveMyCart();
		}
	}

	#endregion Public Event

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	public void SendCartGetOn(long lInstanceId)
	{
		Debug.Log("SendCartGetOn()     m_bWaitResponse = " + m_bWaitResponse);
		if (!m_bWaitResponse)
		{
			if (CsIngameData.Instance.IngameManagement.MyHeroRequestRidingCart())
			{
				Debug.Log("1. SendCartGetOn()");
				m_bWaitResponse = true;
				CartGetOnCommandBody cmdBody = new CartGetOnCommandBody();
				cmdBody.instanceId = lInstanceId;
				CsRplzSession.Instance.Send(ClientCommandName.CartGetOn, cmdBody);
			}
			else
			{
				Debug.Log("2. SendCartGetOn() ");
				if (EventAutoMoveToCart != null) // 카트로 자동 이동.
				{
					EventAutoMoveToCart();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResCartGetOn(int nReturnCode, CartGetOnResponseBody resBody) // 카트타기
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bMyHeroRidingCart = true;
			if (EventMyHeroCartGetOn != null)
			{
				EventMyHeroCartGetOn(resBody.cartInst);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅이 죽은 상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 영웅이 전투상태입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅이 탈것 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 영웅이 카트를 탑승중입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 카트가 존재하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00105"));
		}
		else if (nReturnCode == 106)
		{
			// 카트소유주가 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00106"));
		}
		else if (nReturnCode == 107)
		{
			// 카트를 탑승할 수 있는 거리가 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00107"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendCartGetOff()
	{
		Debug.Log("SendCartGetOff");
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CartGetOffCommandBody cmdBody = new CartGetOffCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CartGetOff, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResCartGetOff(int nReturnCode, CartGetOffResponseBody resBody) // 카트내리기
	{
		m_bWaitResponse = false;
		m_bMyHeroAccelerate = false;

		if (nReturnCode == 0)
		{
			m_bMyHeroRidingCart = false;

			if (EventMyHeroCartGetOff != null)
			{
				EventMyHeroCartGetOff();
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SendCartAccelerate()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CartAccelerateCommandBody cmdBody = new CartAccelerateCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CartAccelerate, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResCartAccelerate(int nReturnCode, CartAccelerateResponseBody resBody) // 카트가속
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bMyHeroAccelerate = resBody.isSuccess;

			if (EventCartAccelerate != null)
			{
				EventCartAccelerate(resBody.isSuccess, resBody.remainingAccelCoolTime);
			}
		}
		else if (nReturnCode == 101)
		{
			// 카트를 타고있지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 쿨타임이 경과되지 않았습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00302"));
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	int m_nPortalId = 0;
	//---------------------------------------------------------------------------------------------------
	void SendCartPortalEnter(int nPortalId)
	{
		if (!m_bWaitResponse)
		{
			Debug.Log("SendCartPortalEnter");
			m_bWaitResponse = true;
			CartPortalEnterCommandBody cmdBody = new CartPortalEnterCommandBody();
			cmdBody.portalId = m_nPortalId = nPortalId;
			CsRplzSession.Instance.Send(ClientCommandName.CartPortalEnter, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResCartPortalEnter(int nReturnCode, CartPortalEnterResponseBody resBody)
	{
		Debug.Log("OnEventResCartPortalEnter  nReturnCode = " + nReturnCode);
		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.Portal;
			if (EventCartPortalEnter != null)
			{
				EventCartPortalEnter(m_nPortalId);
			}
		}
		else
		{
			if (nReturnCode == 101)
			{
				// 카트를 타고있지 않습니다.
				CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A63_ERROR_00401"));
			}
			else
			{
				CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
			}
		}

		m_nPortalId = 0;
		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	public void SendCartPortalExit()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CartPortalExitCommandBody cmdBody = new CartPortalExitCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CartPortalExit, cmdBody);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResCartPortalExit(int nReturnCode, CartPortalExitResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (EventCartPortalExit != null)
			{
				EventCartPortalExit(resBody.entranceInfo);
			}
		}
		else
		{
			CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartEnter(SEBCartEnterEventBody eventBody) // 카트입장(소유자 이외 관련 유저)
	{
		if (EventCartEnter !=null)
		{
			EventCartEnter(eventBody.cartInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartExit(SEBCartExitEventBody eventBody) // 카트퇴장(소유자 이외 관련 유저)
	{
		if (EventCartExit != null)
		{
			EventCartExit(eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartGetOn(SEBCartGetOnEventBody eventBody) // 카트타기(소유자 이외 관련 유저)
	{
		if (EventCartGetOn != null)
		{
			EventCartGetOn(eventBody.heroId, eventBody.cartInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartGetOff(SEBCartGetOffEventBody eventBody) // 카트내리기(소유자 이외 관련 유저)
	{
		if (EventCartGetOff != null)
		{
			EventCartGetOff(eventBody.hero, eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartHighSpeedStart(SEBCartHighSpeedStartEventBody eventBody) // 카트고속주행시작(소유자 이외 관련 유저)
	{
		if (EventCartHighSpeedStart != null)
		{
			EventCartHighSpeedStart(eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartHighSpeedEnd(SEBCartHighSpeedEndEventBody eventBody) // 카트고속주행종료(소유자 이외 관련 유저)
	{
		if (EventCartHighSpeedEnd != null)
		{
			EventCartHighSpeedEnd(eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtMyCartHighSpeedEnd(SEBMyCartHighSpeedEndEventBody eventBody) // 내카트고속주행종료(소유자)
	{
		m_bMyHeroAccelerate = false;
		if (EventMyCartHighSpeedEnd != null)
		{
			EventMyCartHighSpeedEnd();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartInterestAreaEnter(SEBCartInterestAreaEnterEventBody eventBody) // 카트관심영역입장(소유자 이외 관련 유저)
	{
		if (EventCartInterestAreaEnter != null)
		{
			EventCartInterestAreaEnter(eventBody.cartInst);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartInterestAreaExit(SEBCartInterestAreaExitEventBody eventBody) // 카트관심영역퇴장(소유자 이외 관련 유저)
	{
		if (EventCartInterestAreaExit != null)
		{
			EventCartInterestAreaExit(eventBody.instanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartMove(SEBCartMoveEventBody eventBody) // 카트이동(소유자 이외 관련 유저)
	{
		if (EventCartMove != null)
		{
			EventCartMove(eventBody.instanceId, eventBody.position, eventBody.rotationY);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartChanged(SEBCartChangedEventBody eventBody)
	{
		if (EventCartChanged != null)
		{
			EventCartChanged(eventBody.instanceId, eventBody.cartId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartHit(SEBCartHitEventBody eventBody)
	{
		if (EventCartHit != null)
		{
			EventCartHit(eventBody.cartInstanceId, eventBody.hitResult);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartAbnormalStateEffectStart(SEBCartAbnormalStateEffectStartEventBody eventBody)
	{
		//if (EventCartAbnormalStateEffectStart != null)
		//{
		//    EventCartAbnormalStateEffectStart(eventBody.cartInstanceId, 
		//                                      eventBody.abnormalStateEffectInstanceId,
		//                                      eventBody.abnormalStateId,
		//                                      eventBody.sourceJobId,
		//                                      eventBody.level,
		//                                      eventBody.damageAbsorbShieldRemainingAbsorbAmount, 
		//                                      eventBody.remainingTime, 
		//                                      eventBody.removedAbnormalStateEffects);
		//}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartAbnormalStateEffectHit(SEBCartAbnormalStateEffectHitEventBody eventBody)
	{
		if (EventCartAbnormalStateEffectHit != null)
		{
			EventCartAbnormalStateEffectHit(eventBody.cartInstanceId, 
											eventBody.hp, 
											eventBody.removedAbnormalStateEffects, 
											eventBody.abnormalStateEffectInstanceId, 
											eventBody.damage, 
											eventBody.hpDamage, 
											CsIngameData.Instance.IngameManagement.GetAttacker(eventBody.attacker));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtCartAbnormalStateEffectFinished(SEBCartAbnormalStateEffectFinishedEventBody eventBody)
	{
		if (EventCartAbnormalStateEffectFinished != null)
		{
			EventCartAbnormalStateEffectFinished(eventBody.cartInstanceId, eventBody.abnormalStateEffectInstanceId);
		}
	}

	#endregion Protocol.Event
}
