using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using System;


public enum EnSupplySupportState { None, Accepted, Executed };

public class CsSupplySupportQuestManager
{
	bool m_bWaitResponse = false;
	bool m_bAuto = false;
	bool m_bMissionCompleted = false;

	int m_nCartId = 1;
	int m_nCartContinentId;			                                        // 카트 생성 대륙
	long m_lCartInstanceId;			                                        // 카트 인스턴스ID
	Vector3 m_vtCartPosition;		                                        // 카트 위치
	float m_flCartRotationY;		                                         // 카트 방향
    CsSupplySupportQuestCart m_csSupplySupportQuestCart;

    CsSupplySupportQuestOrder m_csSupplySupportQuestOrder;    
    int m_nOrderId;
    int m_nWaypoint;
	float m_flRemainingTime;                                                // 남은 제한시간
	List<CsSupplySupportQuestWayPoint> m_listCsSupplySupportQuestWayPoint = new List<CsSupplySupportQuestWayPoint>();  // 방문하지 않은 WayPoint 리스트

    int m_nDailySupplySupportQuestCount;                                    // 퀘스트 횟수
    DateTime m_dtSupplySupportQuestCountDate;                               // 퀘스트 일자

	EnSupplySupportState m_enQuestState = EnSupplySupportState.None;

	//---------------------------------------------------------------------------------------------------
	public static CsSupplySupportQuestManager Instance
	{
		get { return CsSingleton<CsSupplySupportQuestManager>.GetInstance(); }
	}

    //---------------------------------------------------------------------------------------------------
	public event Delegate EventStartAutoPlay; // ig
	public event Delegate<object> EventStopAutoPlay; // ig, ui
	public event Delegate EventUpdateState;
	public event Delegate<bool> EventStartNpctDialog;
	public event Delegate<int> EventCartChangeNpcDialog;
	public event Delegate EventEndNpctDialog;

	public event Delegate<PDSupplySupportQuestCartInstance> EventSupplySupportQuestAccept;
    public event Delegate<int, int> EventSupplySupportQuestCartChange;
    public event Delegate<bool, long, long, int> EventSupplySupportQuestComplete;

    public event Delegate EventSupplySupportQuestFail;
    public event Delegate <PDItemBooty> EventSupplySupportQuestCartDestructionReward;


    //---------------------------------------------------------------------------------------------------    
	public bool Auto
	{
		get { return m_bAuto; }
	}

	public bool MissionCompleted
	{
		get { return m_bMissionCompleted; }
		set { m_bMissionCompleted = value; }
	}
	
	public int CartContinentId
	{
		get { return m_nCartContinentId; }
		set { m_nCartContinentId = value; }
	}

	public long CartInstanceId
	{
		get { return m_lCartInstanceId; }
		set { m_lCartInstanceId = value; }
	}

	public Vector3 CartPosition
	{
		get { return m_vtCartPosition; }
		set { m_vtCartPosition = value; }
	}

	public float CartRotationY
	{
		get { return m_flCartRotationY; }
		set { m_flCartRotationY = value; }
	}

    public int DailySupplySupportQuestCount
    {
        get { return m_nDailySupplySupportQuestCount; }
        set { m_nDailySupplySupportQuestCount = value; }
    }

    public DateTime SupplySupportQuestCountDate
    {
        get { return m_dtSupplySupportQuestCountDate; }
        set { m_dtSupplySupportQuestCountDate = value; }
    }

	public CsSupplySupportQuest SupplySupportQuest
	{
		get { return CsGameData.Instance.SupplySupportQuest; }
		set { CsGameData.Instance.SupplySupportQuest = value; }
	}

    public CsSupplySupportQuestOrder SupplySupportQuestOrder
    {
        get { return m_csSupplySupportQuestOrder; }
    }

	public float RemainingTime
	{
		get { return m_flRemainingTime; }
		set { m_flRemainingTime = value; }
	}

    public EnSupplySupportState QuestState
    {
        get { return m_enQuestState; }
    }

    public CsSupplySupportQuestCart SupplySupportQuestCart
    {
        get { return m_csSupplySupportQuestCart; }
    }

    public List<CsSupplySupportQuestWayPoint> CsSupplySupportQuestWayPointList
    {
        get { return m_listCsSupplySupportQuestWayPoint; }                          //아직 가지않은 waypoint의 목록
    }


    //---------------------------------------------------------------------------------------------------
    public CsSupplySupportQuestManager()
    {
        CsRplzSession.Instance.EventResSupplySupportQuestAccept += OnEventResSupplySupportQuestAccept;
        CsRplzSession.Instance.EventResSupplySupportQuestCartChange += OnEventResSupplySupportQuestCartChange;
        CsRplzSession.Instance.EventResSupplySupportQuestComplete += OnEventResSupplySupportQuestComplete;

        CsRplzSession.Instance.EventEvtSupplySupportQuestCartDestructionReward += OnEventEvtSupplySupportQuestCartDestructionReward;
        CsRplzSession.Instance.EventEvtSupplySupportQuestFail += OnEventEvtSupplySupportQuestFail;
    }

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroSupplySupportQuest pDHeroSupplySupportQuest)
	{
		UnInit();
		if (pDHeroSupplySupportQuest != null)
		{
            m_enQuestState = EnSupplySupportState.None;

            m_nCartId = pDHeroSupplySupportQuest.cartId;
            if (CsGameData.Instance.MyHeroInfo.CartInstance != null)
            {
                m_csSupplySupportQuestCart = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestCart(CsGameData.Instance.MyHeroInfo.CartInstance.cartId);
            }

			m_nCartContinentId = pDHeroSupplySupportQuest.cartContinentId;
			m_lCartInstanceId = pDHeroSupplySupportQuest.cartInstanceId;
			m_vtCartPosition = CsRplzSession.Translate(pDHeroSupplySupportQuest.cartPosition);
			m_flCartRotationY = pDHeroSupplySupportQuest.cartRotationY;
            m_csSupplySupportQuestOrder = SupplySupportQuest.GetSupplySupportQuestOrder(pDHeroSupplySupportQuest.orderId);
            m_flRemainingTime = pDHeroSupplySupportQuest.remainingTime + Time.realtimeSinceStartup;			

            m_listCsSupplySupportQuestWayPoint.Clear();

			if (m_csSupplySupportQuestCart.CartId != 5) // 최대 등급이 아닌경우.
			{
				m_listCsSupplySupportQuestWayPoint.AddRange(SupplySupportQuest.SupplySupportQuestWayPointList);

				for (int i = 0; i < m_listCsSupplySupportQuestWayPoint.Count; i++)
				{
					for (int j = 0; j < pDHeroSupplySupportQuest.visitedWayPoints.Length; j++)
					{
						if (m_listCsSupplySupportQuestWayPoint[i].WayPoint == pDHeroSupplySupportQuest.visitedWayPoints[j])
						{
							m_listCsSupplySupportQuestWayPoint.RemoveAt(i);
						}
					}
				}
			}

			UpdateSupplySupportState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_bWaitResponse = false;
		m_bAuto = false;
		m_bMissionCompleted = false;
		m_csSupplySupportQuestCart = null;
		m_csSupplySupportQuestOrder = null;
		m_listCsSupplySupportQuestWayPoint.Clear();
		m_enQuestState = EnSupplySupportState.None;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateSupplySupportState(bool bReset = false)
	{		
		if (bReset)
		{
			m_enQuestState = EnSupplySupportState.None;
			m_csSupplySupportQuestOrder = null;
			m_listCsSupplySupportQuestWayPoint.Clear();
			m_flRemainingTime = 0;
		}
		else
		{
			if (m_flRemainingTime == 0) // 수락전.
			{
				m_enQuestState = EnSupplySupportState.None;
			}
			else // 수락후
			{
				if (m_listCsSupplySupportQuestWayPoint.Count == 0) // 미션완료
				{
					m_enQuestState = EnSupplySupportState.Executed;
				}
				else
				{
					m_enQuestState = EnSupplySupportState.Accepted;
				}
			}
		}

		if (EventUpdateState != null)
		{
			EventUpdateState();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StartAutoPlay()
	{
		if (m_bWaitResponse) return;
		if (CsGameData.Instance.DimensionRaidQuest == null) return; // 더이상 진행할 퀘스트가 없을때.

		if (!m_bAuto)
		{
			m_bAuto = true;

			if (EventStartAutoPlay != null)
			{
				EventStartAutoPlay();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StopAutoPlay(object objCaller)
	{
		if (m_bAuto == true)
		{
			m_bAuto = false;

			if (EventStopAutoPlay != null)
			{
				EventStopAutoPlay(objCaller);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool StartNpctDialog(bool bQuestAble)
	{
		if (m_enQuestState == EnSupplySupportState.None && m_nDailySupplySupportQuestCount >= SupplySupportQuest.LimitCount) return false;

		if (EventStartNpctDialog != null)
		{
			EventStartNpctDialog(bQuestAble);
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void CartChangeNpcDialog(int nWayPoint)
	{
		for (int i = 0; i < m_listCsSupplySupportQuestWayPoint.Count; i++)
		{
			if (m_listCsSupplySupportQuestWayPoint[i].WayPoint == nWayPoint)
			{
				m_listCsSupplySupportQuestWayPoint.RemoveAt(i);
			}
		}

		if (EventCartChangeNpcDialog != null)
		{
            EventCartChangeNpcDialog(nWayPoint);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void EndNpctDialog()
	{
		if (EventEndNpctDialog != null)
		{
			if (m_csSupplySupportQuestCart == null && CsGameData.Instance.MyHeroInfo.CartInstance != null)
			{
				m_csSupplySupportQuestCart = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestCart(CsGameData.Instance.MyHeroInfo.CartInstance.cartId);
			}
			EventEndNpctDialog();
		}
	}

    #region SupplySupportQuest.Protocol.Command

    //---------------------------------------------------------------------------------------------------
    public void SendSupplySupportQuestAccept(int nOrderId) // 보급지원 퀘스트 수락
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            SupplySupportQuestAcceptCommandBody cmdBody = new SupplySupportQuestAcceptCommandBody();
            cmdBody.orderId = m_nOrderId = nOrderId;

            CsRplzSession.Instance.Send(ClientCommandName.SupplySupportQuestAccept, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResSupplySupportQuestAccept(int nReturnCode, SupplySupportQuestAcceptResponseBody responseBody) 
    {
        if (nReturnCode == 0)
        {
            m_listCsSupplySupportQuestWayPoint.Clear();
            m_listCsSupplySupportQuestWayPoint.AddRange(SupplySupportQuest.SupplySupportQuestWayPointList);

			m_nCartId = responseBody.cartInst.cartId;
			m_csSupplySupportQuestCart = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestCart(m_nCartId);
            m_enQuestState = EnSupplySupportState.Accepted;
            m_csSupplySupportQuestOrder = SupplySupportQuest.GetSupplySupportQuestOrder(m_nOrderId);

            CsGameData.Instance.MyHeroInfo.Gold = responseBody.gold;            
            m_flRemainingTime = responseBody.remainingTime + Time.realtimeSinceStartup;
            m_nDailySupplySupportQuestCount = responseBody.supplySupportQuestDailyStartCount;
            m_dtSupplySupportQuestCountDate = responseBody.date;

            PDInventorySlot[] chagnedInventorySlots = new PDInventorySlot[] { responseBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(chagnedInventorySlots); // 슬롯변경 여부

			if (m_csSupplySupportQuestCart.CartId == 5) // 최대 등급인 경우.
			{
				m_listCsSupplySupportQuestWayPoint.Clear();
			}

			if (EventSupplySupportQuestAccept != null)
			{
				EventSupplySupportQuestAccept(responseBody.cartInst);
			}

			UpdateSupplySupportState();
        }
        else if (nReturnCode == 101)
        {
            // 영웅의 레벨이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00101"));
        }
        else if (nReturnCode == 102)
        {
            // 지령서아이템이 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00102"));
        }
        else if (nReturnCode == 103)
        {
            // 골드가 부족합니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00103"));
        }
        else if (nReturnCode == 104)
        {
            // 일일퀘스트시작횟수가 최대입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00104"));
        }
        else if (nReturnCode == 105)
        {
            // 시작NPC와 상호작용할 수 없는 위치입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00105"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

         m_bWaitResponse = false;
    }

	//---------------------------------------------------------------------------------------------------
	public void SendSupplySupportQuestCartChange(int nWaypoint) // 보급지원 퀘스트 카트 변경
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            SupplySupportQuestCartChangeCommandBody cmdBody = new SupplySupportQuestCartChangeCommandBody();
            cmdBody.wayPoint = m_nWaypoint = nWaypoint;
            CsRplzSession.Instance.Send(ClientCommandName.SupplySupportQuestCartChange, cmdBody);
        }
    }
    
    //---------------------------------------------------------------------------------------------------
    void OnEventResSupplySupportQuestCartChange(int nReturnCode, SupplySupportQuestCartChangeResponseBody responeseBody)
    {
        if (nReturnCode == 0)
        {
			int nOldCartId = m_nCartId;
			m_nCartId = responeseBody.cartId;
			m_csSupplySupportQuestCart = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestCart(m_nCartId);

			if (m_csSupplySupportQuestCart.CartId == 5) // 최대 등급인 경우.
			{
				m_listCsSupplySupportQuestWayPoint.Clear();
			}
			else
			{
				CsSupplySupportQuestWayPoint csSupplySupportQuestWayPoint = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestWayPoint(m_nWaypoint);

				if (m_listCsSupplySupportQuestWayPoint.Contains(csSupplySupportQuestWayPoint))
				{
					m_listCsSupplySupportQuestWayPoint.Remove(csSupplySupportQuestWayPoint);
				}
			}

			if (EventSupplySupportQuestCartChange != null)
            {
                EventSupplySupportQuestCartChange(nOldCartId, responeseBody.cartId);	
            }

			UpdateSupplySupportState();
        }
        else if (nReturnCode == 101)
        {
            // 현재 퀘스트를 수행중이 아닙니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00201"));
        }
        else if (nReturnCode == 102)
        {
            // 이미 방문한 웨이포인트입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00202"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅이 웨이포인트NPC와 상호작용할 수 없는 위치입니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00203"));
        }
        else if (nReturnCode == 104)
        {
            // 카트가 웨이포인트 목표영역에 존재하지 않습니다.
            CsErrorMessageManager.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A64_ERROR_00204"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void SendSupplySupportQuestComplete() // 보급지원 퀘스트 완료
    {
        if (!m_bWaitResponse)
        {
            m_bWaitResponse = true;
            SupplySupportQuestCompleteCommandBody cmdBody = new SupplySupportQuestCompleteCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.SupplySupportQuestComplete, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResSupplySupportQuestComplete(int nReturnCode, SupplySupportQuestCompleteResponseBody responeseBody) 
    {
        if (nReturnCode == 0)
        {
			m_nCartId = 0;
            int nOldLevel = CsGameData.Instance.MyHeroInfo.Level;
            long nOldGold = CsGameData.Instance.MyHeroInfo.Gold;
            m_flRemainingTime = 0;

            CsGameData.Instance.MyHeroInfo.DailyExploitPoint = responeseBody.dailyExploitPoint;
            CsGameData.Instance.MyHeroInfo.ExploitPoint = responeseBody.exploitPoint;
            CsGameData.Instance.MyHeroInfo.ExploitPointDate = responeseBody.date;
            CsGameData.Instance.MyHeroInfo.Gold = responeseBody.gold;
            CsGameData.Instance.MyHeroInfo.Hp = responeseBody.hp;
            CsGameData.Instance.MyHeroInfo.MaxHp = responeseBody.maxHp;
            CsGameData.Instance.MyHeroInfo.Level = responeseBody.level;
            CsGameData.Instance.MyHeroInfo.Exp = responeseBody.exp;

			// 최대골드
			CsAccomplishmentManager.Instance.MaxGold = responeseBody.maxGold;

            CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			bool bLevelUp = (nOldLevel == CsGameData.Instance.MyHeroInfo.Level) ? false : true;

            if (EventSupplySupportQuestComplete != null)
            {
                EventSupplySupportQuestComplete(bLevelUp, responeseBody.acquiredExp, responeseBody.gold - nOldGold, responeseBody.acquiredExploitPoint);
            }
			UpdateSupplySupportState(true);
        }
        else if (nReturnCode == 101)
        {
            // 현재 퀘스트를 수행중이 아닙니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A64_ERROR_00301"));
        }
        else if (nReturnCode == 102)
        {
            // 이미 방문한 웨이포인트입니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A64_ERROR_00302"));
        }
        else if (nReturnCode == 103)
        {
            // 영웅이 웨이포인트NPC와 상호작용할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A64_ERROR_00303"));
        }
        else if (nReturnCode == 104)
        {
            // 카트가 웨이포인트 목표영역에 존재하지 않습니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A64_ERROR_00304"));
        }
        else if (nReturnCode == 105)
        {
            // 이미 최대등급입니다.
            CsGameEventUIToUI.Instance.OnEventQuestCompltedError(CsConfiguration.Instance.GetString("A64_ERROR_00305"));
        }
        else
        {
            CsErrorMessageManager.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
        }

        m_bWaitResponse = false;
    }

    #endregion SupplySupportQuest.Protocol.Command


    #region SupplySupportQuest.Protocol.Event

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtSupplySupportQuestCartDestructionReward(SEBSupplySupportQuestCartDestructionRewardEventBody EventBody) // 보급지원 카트파괴 보상
    {
        CsGameData.Instance.MyHeroInfo.AddInventorySlots(EventBody.changedInventorySlots);

        if (EventSupplySupportQuestCartDestructionReward != null)
        {
            EventSupplySupportQuestCartDestructionReward(EventBody.booty);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventEvtSupplySupportQuestFail(SEBSupplySupportQuestFailEventBody EventBody) // 보급지원 실패
    {
        CsGameData.Instance.MyHeroInfo.Gold = EventBody.gold;

		// 최대골드
		CsAccomplishmentManager.Instance.MaxGold = EventBody.maxGold;

		m_flRemainingTime = 0;
		m_nCartId = 0;
        if (EventSupplySupportQuestFail != null)
        {
            EventSupplySupportQuestFail();
        }
		UpdateSupplySupportState(true);
    }

    #endregion SupplySupportQuest.Protocol.Event
}
