using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public enum EnPurchaseState
{
	None,
	Complete,
	Cancel,
	Fail,
	InventoryFullError,
}


public class CsCashManager
{
	bool m_bWaitResponse = false;
	bool m_bStoreKitInitialized = false;

	List<CsCashProductPurchaseCount> m_listCsCashProductPurchaseCount;  // 캐쉬상품구매카운트 목록

	bool m_bFirstChargeEventObjectiveCompleted;         // 첫충전이벤트목표완료여부
	bool m_bFirstChargeEventRewarded;                   // 첫충전이벤트보상여부

	int m_nRechargeEventAccUnOwnDia;                    // 재충전이벤트누적비귀속다이아
	bool m_bRechargeEventRewarded;                      // 재충전이벤트보상여부

	CsAccountChargeEvent m_csAccountChargeEvent;        // 충전이벤트. 없을 경우 null

	DateTime m_dtDailyChargeEventDate;
	int m_nDailyChargeEventAccUnOwnDia;                 // 매일충전이벤트누적비귀속다이아
	List<int> m_listRewardedDailyChargeEventMission;    // 매일충전이벤트보상받은미션목록. 배열항목 : 미션번호

	CsAccountConsumeEvent m_csAccountConsumeEvent;      // 소비이벤트, 없을 경우 null

	DateTime m_dtDailyConsumeEventDate;
	int m_nDailyConsumeEventAccDia;                     // 매일소비이벤트누적다이아
	List<int> m_listRewardedDailyConsumeEventMission;   // 매일소비이벤트보상받은미션목록. 배열항목 : 미션번호


	Guid m_guidPurchaseId;
	int m_nProductId;
	int m_nMissionNo;

	EnPurchaseState m_enPurchaseState = EnPurchaseState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsCashManager Instance
	{
		get { return CsSingleton<CsCashManager>.GetInstance(); }
	}

	public List<CsCashProductPurchaseCount> CashProductPurchaseCountList
	{
		get { return m_listCsCashProductPurchaseCount; }
	}

	public bool StoreKitInitialized
	{
		get { return m_bStoreKitInitialized; }
	}

	public bool FirstChargeEventObjectiveCompleted
	{
		get { return m_bFirstChargeEventObjectiveCompleted; }
	}

	public bool FirstChargeEventRewarded
	{
		get { return m_bFirstChargeEventRewarded; }
	}

	public int RechargeEventAccUnOwnDia
	{
		get { return m_nRechargeEventAccUnOwnDia; }
	}

	public bool RechargeEventRewarded
	{
		get { return m_bRechargeEventRewarded; }
	}

	public CsAccountChargeEvent AccountChargeEvent
	{
		get { return m_csAccountChargeEvent; }
	}

	public DateTime DailyChargeEventDate
	{
		get { return m_dtDailyChargeEventDate; }
		set { m_dtDailyChargeEventDate = value; }
	}

	public int DailyChargeEventAccUnOwnDia
	{
		get { return m_nDailyChargeEventAccUnOwnDia; }
		set { m_nDailyChargeEventAccUnOwnDia = value; }
	}

	public List<int> RewardedDailyChargeEventMissionList
	{
		get { return m_listRewardedDailyChargeEventMission; }
	}

	public CsAccountConsumeEvent AccountConsumeEvent
	{
		get { return m_csAccountConsumeEvent; }
	}

	public DateTime DailyConsumeEventDate
	{
		get { return m_dtDailyConsumeEventDate; }
		set { m_dtDailyConsumeEventDate = value; }
	}

	public int DailyConsumeEventAccDia
	{
		get { return m_nDailyConsumeEventAccDia; }
		set { m_nDailyConsumeEventAccDia = value; }
	}

	public List<int> RewardedDailyConsumeEventMissionList
	{
		get { return m_listRewardedDailyConsumeEventMission; }
	}

	public EnPurchaseState PurchaseState
	{
		get { return m_enPurchaseState; }
	}

	//---------------------------------------------------------------------------------------------------
	// 캐쉬상점
	public event Delegate EventCashProductPurchaseCancel;
	public event Delegate EventCashProductPurchaseFail;
	public event Delegate EventCashProductPurchaseComplete;
	public event Delegate EventDisableDimImage;

	// 충전이벤트
	public event Delegate EventFirstChargeEventRewardReceive;
	public event Delegate EventRechargeEventRewardReceive;

	public event Delegate<int> EventChargeEventMissionRewardReceive;
	public event Delegate<int> EventDailyChargeEventMissionRewardReceive;

	// 소비이벤트
	public event Delegate<int> EventConsumeEventMissionRewardReceive;
	public event Delegate<int> EventDailyConsumeEventMissionRewardReceive;

	// 충전이벤트
	public event Delegate EventFirstChargeEventObjectiveCompleted;
	public event Delegate EventRechargeEventProgress;

	public event Delegate EventChargeEventProgress;
	public event Delegate EventDailyChargeEventProgress;
	// 소비이벤트
	public event Delegate EventConsumeEventProgress;
	public event Delegate EventDailyConsumeEventProgress;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDCashProductPurchaseCount[] cashProductPurchaseCounts, bool firstChargeEventObjectiveCompleted, bool firstChargeEventRewarded, int rechargeEventAccUnOwnDia, bool rechargeEventRewarded,
					 PDAccountChargeEvent chargeEvent, int dailyChargeEventAccUnOwnDia, int[] rewardedDailyChargeEventMissions, PDAccountConsumeEvent consumeEvent, int dailyConsumeEventAccDia,
					 int[] rewardedDailyConsumeEventMissions, DateTime dtDate)
	{
		UnInit();

		m_listCsCashProductPurchaseCount = new List<CsCashProductPurchaseCount>();

		for (int i = 0; i < cashProductPurchaseCounts.Length; i++)
		{
			m_listCsCashProductPurchaseCount.Add(new CsCashProductPurchaseCount(cashProductPurchaseCounts[i]));
		}

		m_bFirstChargeEventObjectiveCompleted = firstChargeEventObjectiveCompleted;
		m_bFirstChargeEventRewarded = firstChargeEventRewarded;

		m_nRechargeEventAccUnOwnDia = rechargeEventAccUnOwnDia;
		m_bRechargeEventRewarded = rechargeEventRewarded;

		if (chargeEvent != null)
		{
			m_csAccountChargeEvent = new CsAccountChargeEvent(chargeEvent);
		}

		m_dtDailyChargeEventDate = dtDate;
		m_nDailyChargeEventAccUnOwnDia = dailyChargeEventAccUnOwnDia;
		m_listRewardedDailyChargeEventMission = new List<int>(rewardedDailyChargeEventMissions);

		if (consumeEvent != null)
		{
			m_csAccountConsumeEvent = new CsAccountConsumeEvent(consumeEvent);
		}

		m_dtDailyConsumeEventDate = dtDate;
		m_nDailyConsumeEventAccDia = dailyConsumeEventAccDia;
		m_listRewardedDailyConsumeEventMission = new List<int>(rewardedDailyConsumeEventMissions);

		// Command
		// 캐쉬상점
		CsRplzSession.Instance.EventResCashProductPurchaseStart += OnEventResCashProductPurchaseStart;
		CsRplzSession.Instance.EventResCashProductPurchaseCancel += OnEventResCashProductPurchaseCancel;
		CsRplzSession.Instance.EventResCashProductPurchaseFail += OnEventResCashProductPurchaseFail;
		CsRplzSession.Instance.EventResCashProductPurchaseComplete += OnEventResCashProductPurchaseComplete;
		// 충전이벤트
		CsRplzSession.Instance.EventResFirstChargeEventRewardReceive += OnEventResFirstChargeEventRewardReceive;
		CsRplzSession.Instance.EventResRechargeEventRewardReceive += OnEventResRechargeEventRewardReceive;
		CsRplzSession.Instance.EventResChargeEventMissionRewardReceive += OnEventResChargeEventMissionRewardReceive;
		CsRplzSession.Instance.EventResDailyChargeEventMissionRewardReceive += OnEventResDailyChargeEventMissionRewardReceive;
		// 소비이벤트
		CsRplzSession.Instance.EventResConsumeEventMissionRewardReceive += OnEventResConsumeEventMissionRewardReceive;
		CsRplzSession.Instance.EventResDailyConsumeEventMissionRewardReceive += OnEventResDailyConsumeEventMissionRewardReceive;

		// Event
		// 충전이벤트
		CsRplzSession.Instance.EventEvtFirstChargeEventObjectiveCompleted += OnEventEvtFirstChargeEventObjectiveCompleted;
		CsRplzSession.Instance.EventEvtRechargeEventProgress += OnEventEvtRechargeEventProgress;
		CsRplzSession.Instance.EventEvtChargeEventProgress += OnEventEvtChargeEventProgress;
		CsRplzSession.Instance.EventEvtDailyChargeEventProgress += OnEventEvtDailyChargeEventProgress;
		// 소비이벤트
		CsRplzSession.Instance.EventEvtConsumeEventProgress += OnEventEvtConsumeEventProgress;
		CsRplzSession.Instance.EventEvtDailyConsumeEventProgress += OnEventEvtDailyConsumeEventProgress;

		// 결제 중 접속이 종료된 경우 재접속 시 아이템은 자동 지급 & 알림 출력
		switch (m_enPurchaseState)
		{
			case EnPurchaseState.Complete:
				CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00019"));
				break;
			case EnPurchaseState.Cancel:
				CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00020"));
				break;
			case EnPurchaseState.Fail:
				CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A125_TXT_00021"));
				break;
			default:
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		// 캐쉬상점
		CsRplzSession.Instance.EventResCashProductPurchaseStart -= OnEventResCashProductPurchaseStart;
		CsRplzSession.Instance.EventResCashProductPurchaseCancel -= OnEventResCashProductPurchaseCancel;
		CsRplzSession.Instance.EventResCashProductPurchaseFail -= OnEventResCashProductPurchaseFail;
		CsRplzSession.Instance.EventResCashProductPurchaseComplete -= OnEventResCashProductPurchaseComplete;
		// 충전이벤트
		CsRplzSession.Instance.EventResFirstChargeEventRewardReceive -= OnEventResFirstChargeEventRewardReceive;
		CsRplzSession.Instance.EventResRechargeEventRewardReceive -= OnEventResRechargeEventRewardReceive;
		CsRplzSession.Instance.EventResChargeEventMissionRewardReceive -= OnEventResChargeEventMissionRewardReceive;
		CsRplzSession.Instance.EventResDailyChargeEventMissionRewardReceive -= OnEventResDailyChargeEventMissionRewardReceive;
		// 소비이벤트
		CsRplzSession.Instance.EventResConsumeEventMissionRewardReceive -= OnEventResConsumeEventMissionRewardReceive;
		CsRplzSession.Instance.EventResDailyConsumeEventMissionRewardReceive -= OnEventResDailyConsumeEventMissionRewardReceive;

		// Event
		// 충전이벤트
		CsRplzSession.Instance.EventEvtFirstChargeEventObjectiveCompleted -= OnEventEvtFirstChargeEventObjectiveCompleted;
		CsRplzSession.Instance.EventEvtRechargeEventProgress -= OnEventEvtRechargeEventProgress;
		CsRplzSession.Instance.EventEvtChargeEventProgress -= OnEventEvtChargeEventProgress;
		CsRplzSession.Instance.EventEvtDailyChargeEventProgress -= OnEventEvtDailyChargeEventProgress;
		// 소비이벤트
		CsRplzSession.Instance.EventEvtConsumeEventProgress -= OnEventEvtConsumeEventProgress;
		CsRplzSession.Instance.EventEvtDailyConsumeEventProgress -= OnEventEvtDailyConsumeEventProgress;

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	CsCashProductPurchaseCount GetCashProductPurchaseCount(int nProductId)
	{
		for (int i = 0; i < m_listCsCashProductPurchaseCount.Count; i++)
		{
			if (m_listCsCashProductPurchaseCount[i].ProductId == nProductId)
				return m_listCsCashProductPurchaseCount[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void AddCashProductPurchaseCount(int nProductId, int nCount = 1)
	{
		CsCashProductPurchaseCount csCashProductPurchaseCount = GetCashProductPurchaseCount(nProductId);

		if (csCashProductPurchaseCount == null)
		{
			m_listCsCashProductPurchaseCount.Add(new CsCashProductPurchaseCount(nProductId, nCount));
		}
		else
		{
			csCashProductPurchaseCount.Count += nCount;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InitializeStoreKit()
	{
		if (!m_bStoreKitInitialized)
		{
			if (CsConfiguration.Instance.PlatformId == CsConfiguration.EnPlatformID.Android)
			{
				SendStoreKitInitNACommand(CsConfiguration.Instance.SystemSetting.GooglePublicKey);
			}
			else if (CsConfiguration.Instance.PlatformId == CsConfiguration.EnPlatformID.iOS)
			{
				SendStoreKitInitNACommand();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StoreKitGetProducts()
	{
		SendStoreKitGetProducts();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDisableDimImage()
	{
		if (EventDisableDimImage != null)
		{
			EventDisableDimImage();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckFirstChargingEvent()
	{
		return m_bFirstChargeEventObjectiveCompleted && !m_bFirstChargeEventRewarded;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckReChargingEvent()
	{
		return m_bFirstChargeEventRewarded && !CsCashManager.Instance.RechargeEventRewarded &&
			CsGameData.Instance.RechargeEvent.RequiredUnOwnDia <= m_nRechargeEventAccUnOwnDia;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckChargingEvent()
	{
		CsChargeEvent csChargeEvent = CsGameData.Instance.GetCurrentChargeEvent();

		if (CsGameConfig.Instance.ChargeEventRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
			csChargeEvent != null && m_csAccountChargeEvent != null)
		{
			foreach (CsChargeEventMission csChargeEventMission in csChargeEvent.ChargeEventMissionList)
			{
				if (csChargeEventMission.RequiredUnOwnDia <= CsCashManager.Instance.AccountChargeEvent.AccUnOwnDia &&
					!m_csAccountChargeEvent.RewardedMissionList.Contains(csChargeEventMission.MissionNo))
				{
					return true;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckDailyChargingEvent()
	{
		if (CsGameData.Instance.DailyChargeEvent.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
		{
			foreach (CsDailyChargeEventMission csDailyChargeEventMission in CsGameData.Instance.DailyChargeEvent.DailyChargeEventMissionList)
			{
				if (csDailyChargeEventMission.RequiredUnOwnDia <= m_nDailyChargeEventAccUnOwnDia &&
					!m_listRewardedDailyChargeEventMission.Contains(csDailyChargeEventMission.MissionNo))
				{
					return true;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckConsumptionEvent()
	{
		CsConsumeEvent csConsumeEvent = CsGameData.Instance.GetCurrentConsumeEvent();

		if (CsGameConfig.Instance.ConsumeEventRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
			csConsumeEvent != null && m_csAccountConsumeEvent != null)
		{
			foreach (CsConsumeEventMission csConsumeEventMission in csConsumeEvent.ConsumeEventMissionList)
			{
				if (csConsumeEventMission.RequiredDia <= m_csAccountConsumeEvent.AccDia &&
					!m_csAccountConsumeEvent.RewardedMissionList.Contains(csConsumeEventMission.MissionNo))
				{
					return true;
				}
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckDailyConsumptionEvent()
	{
		if (CsGameData.Instance.DailyConsumeEvent.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
		{
			foreach (CsDailyConsumeEventMission csDailyConsumeEventMission in CsGameData.Instance.DailyConsumeEvent.DailyConsumeEventMissionList)
			{
				if (csDailyConsumeEventMission.RequiredDia <= m_nDailyConsumeEventAccDia &&
					!m_listRewardedDailyConsumeEventMission.Contains(csDailyConsumeEventMission.MissionNo))
				{
					return true;
				}
			}
		}

		return false;
	}

	#region Protocol.Command

	#region 캐쉬상점

	//---------------------------------------------------------------------------------------------------
	// 캐쉬상품구매시작
	public void SendCashProductPurchaseStart(int nProductId)
	{
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("유니티 에디터에서는 사용할 수 없습니다.");
			OnEventDisableDimImage();
			return;
		}

		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CashProductPurchaseStartCommandBody cmdBody = new CashProductPurchaseStartCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			cmdBody.storeType = (int)CsConfiguration.Instance.PlatformId;
			CsRplzSession.Instance.Send(ClientCommandName.CashProductPurchaseStart, cmdBody);
		}
	}

	void OnEventResCashProductPurchaseStart(int nReturnCode, CashProductPurchaseStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_guidPurchaseId = resBody.purchaseId;

			SendStoreKitPurchaseNACommand();
		}
		else
		{
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 캐쉬상품구매취소
	public void SendCashProductPurchaseCancel()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CashProductPurchaseCancelCommandBody cmdBody = new CashProductPurchaseCancelCommandBody();
			cmdBody.purchaseId = m_guidPurchaseId;
			CsRplzSession.Instance.Send(ClientCommandName.CashProductPurchaseCancel, cmdBody);
		}
	}

	void OnEventResCashProductPurchaseCancel(int nReturnCode, CashProductPurchaseCancelResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_enPurchaseState = EnPurchaseState.None;
			m_guidPurchaseId = Guid.Empty;

			if (EventCashProductPurchaseCancel != null)
			{
				EventCashProductPurchaseCancel();
			}
		}
		else if (nReturnCode == 101)
		{
			// 구매내역이 존재하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 구매상태가 유효하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00202"));
		}
		else
		{
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 캐쉬상품구매실패
	public void SendCashProductPurchaseFail(string strFailReason)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CashProductPurchaseFailCommandBody cmdBody = new CashProductPurchaseFailCommandBody();
			cmdBody.purchaseId = m_guidPurchaseId;
			cmdBody.failReason = strFailReason;
			CsRplzSession.Instance.Send(ClientCommandName.CashProductPurchaseFail, cmdBody);
		}
	}

	void OnEventResCashProductPurchaseFail(int nReturnCode, CashProductPurchaseFailResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_enPurchaseState = EnPurchaseState.None;
			m_guidPurchaseId = Guid.Empty;

			if (EventCashProductPurchaseFail != null)
			{
				EventCashProductPurchaseFail();
			}
		}
		else if (nReturnCode == 101)
		{
			// 구매내역이 존재하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 구매상태가 유효하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00302"));
		}
		else
		{
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 캐쉬상품구매완료
	public void SendCashProductPurchaseComplete()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CashProductPurchaseCompleteCommandBody cmdBody = new CashProductPurchaseCompleteCommandBody();
			cmdBody.purchaseId = m_guidPurchaseId;
			CsRplzSession.Instance.Send(ClientCommandName.CashProductPurchaseComplete, cmdBody);
		}
	}

	void OnEventResCashProductPurchaseComplete(int nReturnCode, CashProductPurchaseCompleteResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_enPurchaseState = EnPurchaseState.None;
			m_guidPurchaseId = Guid.Empty;
			AddCashProductPurchaseCount(m_nProductId);

			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;
			CsGameData.Instance.MyHeroInfo.VipPoint = resBody.vipPoint;

			if (EventCashProductPurchaseComplete != null)
			{
				EventCashProductPurchaseComplete();
			}
		}
		else if (nReturnCode == 101)
		{
			// 구매내역이 존재하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 구매상태가 유효하지 않습니다.
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A159_ERROR_00402"));
		}
		else
		{
			OnEventDisableDimImage();
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion 캐쉬상점

	#region 충전이벤트

	//---------------------------------------------------------------------------------------------------
	// 첫충전이벤트보상받기
	public void SendFirstChargeEventRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			FirstChargeEventRewardReceiveCommandBody cmdBody = new FirstChargeEventRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.FirstChargeEventRewardReceive, cmdBody);
		}
	}

	void OnEventResFirstChargeEventRewardReceive(int nReturnCode, FirstChargeEventRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bFirstChargeEventRewarded = true;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventFirstChargeEventRewardReceive != null)
			{
				EventFirstChargeEventRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 첫충전을 하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 재충전이벤트보상받기
	public void SendRechargeEventRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			RechargeEventRewardReceiveCommandBody cmdBody = new RechargeEventRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.RechargeEventRewardReceive, cmdBody);
		}
	}

	void OnEventResRechargeEventRewardReceive(int nReturnCode, RechargeEventRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bRechargeEventRewarded = true;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventRechargeEventRewardReceive != null)
			{
				EventRechargeEventRewardReceive();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 목표액을 충전하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00204"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 충전이벤트미션보상받기
	public void SendChargeEventMissionRewardReceive(int nEventId, int nMissionNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ChargeEventMissionRewardReceiveCommandBody cmdBody = new ChargeEventMissionRewardReceiveCommandBody();
			cmdBody.eventId = nEventId;
			cmdBody.missionNo = m_nMissionNo = nMissionNo;
			CsRplzSession.Instance.Send(ClientCommandName.ChargeEventMissionRewardReceive, cmdBody);
		}
	}

	void OnEventResChargeEventMissionRewardReceive(int nReturnCode, ChargeEventMissionRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csAccountChargeEvent.RewardedMissionList.Add(m_nMissionNo);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventChargeEventMissionRewardReceive != null)
			{
				EventChargeEventMissionRewardReceive(m_nMissionNo);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 이벤트시간이 아닙니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 목표액을 충전하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00303"));
		}
		else if (nReturnCode == 104)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00304"));
		}
		else if (nReturnCode == 105)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00305"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 매일충전이벤트미션보상받기
	public void SendDailyChargeEventMissionRewardReceive(int nMissionNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			DailyChargeEventMissionRewardReceiveCommandBody cmdBody = new DailyChargeEventMissionRewardReceiveCommandBody();
			cmdBody.date = m_dtDailyChargeEventDate;
			cmdBody.missionNo = m_nMissionNo = nMissionNo;
			CsRplzSession.Instance.Send(ClientCommandName.DailyChargeEventMissionRewardReceive, cmdBody);
		}
	}

	void OnEventResDailyChargeEventMissionRewardReceive(int nReturnCode, DailyChargeEventMissionRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);
			m_listRewardedDailyChargeEventMission.Add(m_nMissionNo);

			if (EventDailyChargeEventMissionRewardReceive != null)
			{
				EventDailyChargeEventMissionRewardReceive(m_nMissionNo);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00401"));
		}
		else if (nReturnCode == 102)
		{
			// 해당 날짜는 보상받을 수 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00402"));
		}
		else if (nReturnCode == 103)
		{
			// 영우레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00403"));
		}
		else if (nReturnCode == 104)
		{
			// 목표액을 충전하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00404"));
		}
		else if (nReturnCode == 105)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00405"));
		}
		else if (nReturnCode == 106)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A160_ERROR_00406"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion 충전이벤트

	#region 소비이벤트

	//---------------------------------------------------------------------------------------------------
	// 소비이벤트미션보상받기
	public void SendConsumeEventMissionRewardReceive(int nEventId, int nMissionNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ConsumeEventMissionRewardReceiveCommandBody cmdBody = new ConsumeEventMissionRewardReceiveCommandBody();
			cmdBody.eventId = nEventId;
			cmdBody.missionNo = m_nMissionNo = nMissionNo;
			CsRplzSession.Instance.Send(ClientCommandName.ConsumeEventMissionRewardReceive, cmdBody);
		}
	}

	void OnEventResConsumeEventMissionRewardReceive(int nReturnCode, ConsumeEventMissionRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_csAccountConsumeEvent.RewardedMissionList.Add(m_nMissionNo);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventConsumeEventMissionRewardReceive != null)
			{
				EventConsumeEventMissionRewardReceive(m_nMissionNo);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 이벤트시간이 아닙니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 목표액을 소비하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00105"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 매일소비이벤트미션보상받기
	public void SendDailyConsumeEventMissionRewardReceive(int nMissionNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			DailyConsumeEventMissionRewardReceiveCommandBody cmdBody = new DailyConsumeEventMissionRewardReceiveCommandBody();
			cmdBody.date = m_dtDailyConsumeEventDate;
			cmdBody.missionNo = m_nMissionNo = nMissionNo;
			CsRplzSession.Instance.Send(ClientCommandName.DailyConsumeEventMissionRewardReceive, cmdBody);
		}
	}

	void OnEventResDailyConsumeEventMissionRewardReceive(int nReturnCode, DailyConsumeEventMissionRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listRewardedDailyConsumeEventMission.Add(m_nMissionNo);
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventDailyConsumeEventMissionRewardReceive != null)
			{
				EventDailyConsumeEventMissionRewardReceive(m_nMissionNo);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이벤트가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 해당 날짜는 보상받을 수 없습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 영웅레벨이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00203"));
		}
		else if (nReturnCode == 104)
		{
			// 목표액을 충전하지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00204"));
		}
		else if (nReturnCode == 105)
		{
			// 이미 보상을 받았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00205"));
		}
		else if (nReturnCode == 106)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A161_ERROR_00206"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion 소비이벤트

	#endregion Protocol.Command

	#region Protocol.Event

	#region 충전이벤트

	//---------------------------------------------------------------------------------------------------
	// 첫충전이벤트목표완료
	void OnEventEvtFirstChargeEventObjectiveCompleted(SEBFirstChargeEventObjectiveCompletedEventBody eventBody)
	{
		m_bFirstChargeEventObjectiveCompleted = true;

		if (EventFirstChargeEventObjectiveCompleted != null)
		{
			EventFirstChargeEventObjectiveCompleted();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 재충전이벤트진행
	void OnEventEvtRechargeEventProgress(SEBRechargeEventProgressEventBody eventBody)
	{
		m_nRechargeEventAccUnOwnDia = eventBody.accUnOwnDia;

		if (EventRechargeEventProgress != null)
		{
			EventRechargeEventProgress();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 충전이벤트진행
	void OnEventEvtChargeEventProgress(SEBChargeEventProgressEventBody eventBody)
	{
		m_csAccountChargeEvent.AccUnOwnDia = eventBody.accUnOwnDia;

		if (EventChargeEventProgress != null)
		{
			EventChargeEventProgress();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDailyChargeEventProgress(SEBDailyChargeEventProgressEventBody eventBody)
	{
		m_nDailyChargeEventAccUnOwnDia = eventBody.accUnOwnDia;

		if (EventDailyChargeEventProgress != null)
		{
			EventDailyChargeEventProgress();
		}
	}

	#endregion 충전이벤트

	#region 소비이벤트

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtConsumeEventProgress(SEBConsumeEventProgressEventBody eventBody)
	{
		m_csAccountConsumeEvent.AccDia = eventBody.accDia;

		if (EventConsumeEventProgress != null)
		{
			EventConsumeEventProgress();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEvtDailyConsumeEventProgress(SEBDailyConsumeEventProgressEventBody eventBody)
	{
		m_nDailyConsumeEventAccDia = eventBody.accDia;

		if (EventDailyConsumeEventProgress != null)
		{
			EventDailyConsumeEventProgress();
		}
	}

	#endregion 소비이벤트

	#endregion Protocol.Event

	#region Protocol.Native.Command

	//---------------------------------------------------------------------------------------------------
	void SendStoreKitInitNACommand(string sPublicKey = "")
	{
		StoreKitInitNACommand cmd = new StoreKitInitNACommand();
		cmd.Finished += OnEventResStoreKitInit;

		if (sPublicKey.Length > 0)
		{
			cmd.PublicKey = sPublicKey;
		}

		cmd.Run();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResStoreKitInit(object sender, EventArgs e)
	{
		StoreKitInitNACommand cmd = (StoreKitInitNACommand)sender;

		if (!cmd.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("StoreKitInitNACommand Error :" + cmd.error);
			return;
		}

		StoreKitInitNAResponse res = (StoreKitInitNAResponse)cmd.response;

		if (!res.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("StoreKitInitNAResponse Error :" + res.errorMessage);
			return;
		}

		m_bStoreKitInitialized = true;

		StoreKitGetProducts();
	}

	//---------------------------------------------------------------------------------------------------
	void SendStoreKitGetProducts()
	{
		//StoreKitGetProductsNACommand cmd = new StoreKitGetProductsNACommand();
		//cmd.Products = new string[CsGameData.Instance.CashProductList.Count];
		//
		//int remainCount = (Config.instance.InAppProductList.Count - (m_nStoreKitGetProductCount * 20)) / 20 > 0 ? 20 : Config.instance.InAppProductList.Count - (m_nStoreKitGetProductCount * 20);
		//
		//for (int j = 0; j < remainCount; j++)
		//{
		//    cmd.Products[j] = Config.instance.InAppProductList[(m_nStoreKitGetProductCount * 20) + j].InAppProductKey;
		//}
		//
		//cmd.Run();

		CsGameEventUIToUI.Instance.OnEventOpenPopupCharging(true);
	}

	void OnEventResStoreKitGetProducts(object sender, EventArgs e)
	{
		StoreKitGetProductsNACommand cmd = (StoreKitGetProductsNACommand)sender;

		if (!cmd.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("StoreKitGetProductsNACommand Error :" + cmd.error);
			return;
		}

		StoreKitGetProductsNAResponse res = (StoreKitGetProductsNAResponse)cmd.response;

		if (!res.isOK)
		{
			CsGameEventUIToUI.Instance.OnEventAlert("StoreKitInitNAResponse Error :" + res.errorMessage);
			return;
		}

		CsGameEventUIToUI.Instance.OnEventOpenPopupCharging(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void SendStoreKitPurchaseNACommand()
	{
		CsCashProduct csCashProduct = CsGameData.Instance.GetCashProduct(m_nProductId);

		if (csCashProduct != null)
		{
			StoreKitPurchaseNACommand cmd = new StoreKitPurchaseNACommand();
			cmd.Finished += OnEventResStoreKitPurchase;

			cmd.AuthServerUrl = CsConfiguration.Instance.AuthServerApiUrl;
			cmd.UserAccessToken = CsConfiguration.Instance.User.AccessToken;
			cmd.VirtualGameServerId = CsConfiguration.Instance.GameServerSelected.VirtualGameServerId;
			cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
			cmd.ProductId = csCashProduct.InAppProduct.InAppProductKey;
			cmd.LogId = m_guidPurchaseId.ToString();

			cmd.Run();
		}
	}

	void OnEventResStoreKitPurchase(object sender, EventArgs e)
	{
		StoreKitPurchaseNACommand cmd = (StoreKitPurchaseNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log("StoreKitPurchaseNACommand Error :" + cmd.error);
			m_enPurchaseState = EnPurchaseState.Cancel;
			SendCashProductPurchaseCancel();
			return;
		}

		StoreKitPurchaseNAResponse res = (StoreKitPurchaseNAResponse)cmd.response;

		if (res.isOK)
		{
			m_enPurchaseState = EnPurchaseState.Complete;
			SendCashProductPurchaseComplete();
		}
		else
		{
			Debug.Log("StoreKitPurchaseNAResponse Error :" + res.errorMessage);

			if (res.result == StoreKitPurchaseNAResponse.kResult_Canceled)
			{
				m_enPurchaseState = EnPurchaseState.Cancel;
				SendCashProductPurchaseCancel();
			}
			else
			{
				m_enPurchaseState = EnPurchaseState.Fail;
				SendCashProductPurchaseFail(res.errorMessage);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	
	#endregion Protocol.Native.Command
}
