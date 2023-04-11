using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientCommon;
using System;

public class CsIllustratedBookManager
{
	bool m_bWaitResponse = false;

	int m_nIllustratedBookExplorationStepNo;                            // 도감탐험단계번호
	DateTime m_dtIllustratedBookExplorationStepRewardReceivedDate;      // 도감탐험단계보상받은날짜
	int m_nIllustratedBookExplorationStepRewardReceivedStepNo;          // 도감탐험단계보상받은단계번호	
	List<int> m_listActivationIllustratedBookIds;                       // 활성도감ID 목록
	List<int> m_listCompletedSceneryQuests;                             // 완료된 풍광퀘스트 목록. 배열항목 : 퀘스트ID
	float m_flSceneryQuestRemainingTime;                                // 풍광퀘스트 남은시간.

	int m_nSceneryQuestId;
	int m_nTargetStepNo;

	//---------------------------------------------------------------------------------------------------
	public static CsIllustratedBookManager Instance
	{
		get { return CsSingleton<CsIllustratedBookManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventIllustratedBookUse;
	public event Delegate EventIllustratedBookExplorationStepAcquire;
	public event Delegate<PDItemBooty[]> EventIllustratedBookExplorationStepRewardReceive;
	public event Delegate<int> EventSceneryQuestStart;

	public event Delegate EventSceneryQuestCanceled;
	public event Delegate<PDItemBooty, int> EventSceneryQuestCompleted;

	//---------------------------------------------------------------------------------------------------
	public int IllustratedBookExplorationStepNo
	{
		get { return m_nIllustratedBookExplorationStepNo; }
		set { m_nIllustratedBookExplorationStepNo = value; }
	}

	public DateTime IllustratedBookExplorationStepRewardReceivedDate
	{
		get { return m_dtIllustratedBookExplorationStepRewardReceivedDate; }
		set { m_dtIllustratedBookExplorationStepRewardReceivedDate = value; }
	}

	public int IllustratedBookExplorationStepRewardReceivedStepNo
	{
		get { return m_nIllustratedBookExplorationStepRewardReceivedStepNo; }
		set { m_nIllustratedBookExplorationStepRewardReceivedStepNo = value; }
	}

	public List<int> ActivationIllustratedBookIdList
	{
		get { return m_listActivationIllustratedBookIds; }
	}

	public List<int> CompletedSceneryQuestList
	{
		get { return m_listCompletedSceneryQuests; }
	}

	public float SceneryQuestRemainingTime
	{
		get { return m_flSceneryQuestRemainingTime; }
	}
	
	public int SceneryQuestId
	{
		get { return m_nSceneryQuestId; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(int nIllustratedBookExplorationStepNo, DateTime dtIllustratedBookExplorationStepRewardReceivedDate, int nIllustratedBookExplorationStepRewardReceivedStepNo, int[] activationIllustratedBookIds, int[] completedSceneryQuests)
	{
		UnInit();
		
		m_nIllustratedBookExplorationStepNo = nIllustratedBookExplorationStepNo;
		m_dtIllustratedBookExplorationStepRewardReceivedDate = dtIllustratedBookExplorationStepRewardReceivedDate;
		m_nIllustratedBookExplorationStepRewardReceivedStepNo = nIllustratedBookExplorationStepRewardReceivedStepNo;
		m_listActivationIllustratedBookIds = new List<int>(activationIllustratedBookIds);
		m_listCompletedSceneryQuests = new List<int>(completedSceneryQuests);

		// Command
		CsRplzSession.Instance.EventResIllustratedBookUse += OnEventResIllustratedBookUse;
		CsRplzSession.Instance.EventResIllustratedBookExplorationStepAcquire += OnEventResIllustratedBookExplorationStepAcquire;
		CsRplzSession.Instance.EventResIllustratedBookExplorationStepRewardReceive += OnEventResIllustratedBookExplorationStepRewardReceive;
		CsRplzSession.Instance.EventResSceneryQuestStart += OnEventResSceneryQuestStart;

		// Event
		CsRplzSession.Instance.EventEvtSceneryQuestCanceled += OnEventEvtSceneryQuestCanceled;
		CsRplzSession.Instance.EventEvtSceneryQuestCompleted += OnEventEvtSceneryQuestCompleted;

	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResIllustratedBookUse -= OnEventResIllustratedBookUse;
		CsRplzSession.Instance.EventResIllustratedBookExplorationStepAcquire -= OnEventResIllustratedBookExplorationStepAcquire;
		CsRplzSession.Instance.EventResIllustratedBookExplorationStepRewardReceive -= OnEventResIllustratedBookExplorationStepRewardReceive;
		CsRplzSession.Instance.EventResSceneryQuestStart -= OnEventResSceneryQuestStart;

		// Event
		CsRplzSession.Instance.EventEvtSceneryQuestCanceled -= OnEventEvtSceneryQuestCanceled;
		CsRplzSession.Instance.EventEvtSceneryQuestCompleted -= OnEventEvtSceneryQuestCompleted;

		m_bWaitResponse = false;
		m_listActivationIllustratedBookIds = null;
		m_listCompletedSceneryQuests = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void SceneryQuestStart(int nQuestId)
	{
		//Debug.Log("CsIllustratedBookManager.SceneryQuestStart   nQuestId = " + nQuestId + " m_flSceneryQuestRemainingTime = " + m_flSceneryQuestRemainingTime);
		if (m_flSceneryQuestRemainingTime == 0)
		{
			if(CsMainQuestManager.Instance.MainQuest != null && CsMainQuestManager.Instance.MainQuest.MainQuestNo > CsGameConfig.Instance.SceneryQuestRequiredMainQuestNo)
			{
				SendSceneryQuestStart(nQuestId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public Dictionary<int, long> GetCumulativeAttr()
	{
		Dictionary<int, long> dicAttrValue = new Dictionary<int, long>();		// 도감탐험 개방에 따른 누적 속성

		for (int i = 0; i < CsGameData.Instance.IllustratedBookExplorationStepList.Count; i++)
		{
			if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo <= m_nIllustratedBookExplorationStepNo)
			{
				CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.IllustratedBookExplorationStepList[i];

				for (int j = 0; j < csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList.Count; j++)
				{
					if (dicAttrValue.ContainsKey(csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].Attr.AttrId))
					{
						dicAttrValue[csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].Attr.AttrId] += csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].AttrValue.Value;
					}
					else
					{
						dicAttrValue.Add(csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].Attr.AttrId, csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[j].AttrValue.Value);
					}
				}
			}
		}

		return dicAttrValue;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 도감사용
	public void SendIllustratedBookUse(int nSlotIndex) 
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			IllustratedBookUseCommandBody cmdBody = new IllustratedBookUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.IllustratedBookUse, cmdBody);
		}
	}

	void OnEventResIllustratedBookUse(int nReturnCode, IllustratedBookUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			if (resBody.activationIllustratedBookId > 0)
				m_listActivationIllustratedBookIds.Add(resBody.activationIllustratedBookId);

			CsGameData.Instance.MyHeroInfo.ExplorationPoint = resBody.explorationPoint;

			PDInventorySlot[] inventorySlots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventIllustratedBookUse != null)
			{
				EventIllustratedBookUse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A79_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 도감탐험단계획득
	public void SendIllustratedBookExplorationStepAcquire(int nTargetStepNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			IllustratedBookExplorationStepAcquireCommandBody cmdBody = new IllustratedBookExplorationStepAcquireCommandBody();
			cmdBody.targetStepNo = m_nTargetStepNo = nTargetStepNo;
			CsRplzSession.Instance.Send(ClientCommandName.IllustratedBookExplorationStepAcquire, cmdBody);
		}
	}

	void OnEventResIllustratedBookExplorationStepAcquire(int nReturnCode, IllustratedBookExplorationStepAcquireResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nIllustratedBookExplorationStepNo = m_nTargetStepNo;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventIllustratedBookExplorationStepAcquire != null)
			{
				EventIllustratedBookExplorationStepAcquire();
			}
		}
		else if (nReturnCode == 101)
		{
			// 탐험점수가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A79_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 도감탐험단계보상받기
	public void SendIllustratedBookExplorationStepRewardReceive()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			IllustratedBookExplorationStepRewardReceiveCommandBody cmdBody = new IllustratedBookExplorationStepRewardReceiveCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.IllustratedBookExplorationStepRewardReceive, cmdBody);
		}
	}

	void OnEventResIllustratedBookExplorationStepRewardReceive(int nReturnCode, IllustratedBookExplorationStepRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtIllustratedBookExplorationStepRewardReceivedDate = resBody.date;
			m_nIllustratedBookExplorationStepRewardReceivedStepNo = m_nIllustratedBookExplorationStepNo;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			// 최대골드
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			if (EventIllustratedBookExplorationStepRewardReceive != null)
			{
				EventIllustratedBookExplorationStepRewardReceive(resBody.booties);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 보상을 받았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A79_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 풍광퀘스트시작
	void SendSceneryQuestStart(int nQuestId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			SceneryQuestStartCommandBody cmdBody = new SceneryQuestStartCommandBody();
            cmdBody.questId = m_nSceneryQuestId = nQuestId;
			CsRplzSession.Instance.Send(ClientCommandName.SceneryQuestStart, cmdBody);
		}
	}

	void OnEventResSceneryQuestStart(int nReturnCode, SceneryQuestStartResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_flSceneryQuestRemainingTime = resBody.remainingTime +Time.realtimeSinceStartup;

			if (EventSceneryQuestStart != null)
			{
				Debug.Log("OnEventResSceneryQuestStart     >>>>     EventSceneryQuestStart    m_nSceneryQuestId = " + m_nSceneryQuestId);
                EventSceneryQuestStart(m_nSceneryQuestId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 완료했습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A80_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 현재 진행중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A80_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 퀘스트를 시작할 수 없는 위치입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A80_ERROR_00103"));
		}
        else if (nReturnCode == 104)
        {
            // 필요한 메인퀘스트를 완료하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A80_ERROR_00104"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command


	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 풍광퀘스트취소
	void OnEventEvtSceneryQuestCanceled(SEBSceneryQuestCanceledEventBody eventBody)
	{
		Debug.Log("CsIllustratedBookManager.OnEventEvtSceneryQuestCanceled()");
		m_flSceneryQuestRemainingTime = 0; // 0? 으로 초기화?
		if (EventSceneryQuestCanceled != null)
		{
			EventSceneryQuestCanceled();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 풍광퀘스트완료
	void OnEventEvtSceneryQuestCompleted(SEBSceneryQuestCompletedEventBody eventBody)
	{
		Debug.Log("CsIllustratedBookManager.OnEventEvtSceneryQuestCompleted()    eventBody.questId = " + eventBody.questId);
		m_listCompletedSceneryQuests.Add(eventBody.questId);
		CsGameData.Instance.MyHeroInfo.AddInventorySlots(eventBody.changedInventorySlots);
		m_flSceneryQuestRemainingTime = 0; // 0? 으로 초기화?

		if (EventSceneryQuestCompleted != null)
		{
			EventSceneryQuestCompleted(eventBody.booty, eventBody.questId);
		}
	}

	#endregion Protocol.Event
}
