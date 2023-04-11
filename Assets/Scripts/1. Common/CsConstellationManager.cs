using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using System.Linq;
using UnityEngine;

public class CsConstellationManager
{
	bool m_bWaitResponse = false;

	List<CsHeroConstellation> m_listCsHeroConstellation;
	int m_nStarEssense;
	int m_nDailyStarEssenseItemUseCount;
	DateTime m_dtStarEssenseItemUseCountDate;

	int m_nConstellationId;
	int m_nStep;
	int m_nCycle;
	int m_nEntryNo;

	//---------------------------------------------------------------------------------------------------
	public static CsConstellationManager Instance
	{
		get { return CsSingleton<CsConstellationManager>.GetInstance(); }
	}

	public List<CsHeroConstellation> HeroConstellationList
	{
		get { return m_listCsHeroConstellation; }
	}

	public int StarEssense
	{
		get { return m_nStarEssense; }
	}

	public int DailyStarEssenseItemUseCount
	{
		get { return m_nDailyStarEssenseItemUseCount; }
		set { m_nDailyStarEssenseItemUseCount = value; }
	}

	public DateTime StarEssenseItemUseCountDate
	{
		get { return m_dtStarEssenseItemUseCountDate; }
		set { m_dtStarEssenseItemUseCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<int> EventStarEssenseItemUse;
	public event Delegate<int> EventPremiumStarEssenseItemUse;
	public event Delegate<bool> EventConstellationEntryActivate;
	public event Delegate<int> EventConstellationStepOpen;
	public event Delegate EventConstellationOpened;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroConstellation[] constellations, int starEssense, int dailyStarEssenseItemUseCount, DateTime dtDate)
	{
		UnInit();

		m_listCsHeroConstellation = new List<CsHeroConstellation>();

		for (int i = 0; i < constellations.Length; i++)
		{
			m_listCsHeroConstellation.Add(new CsHeroConstellation(constellations[i]));
		}

		m_nStarEssense = starEssense;
		m_nDailyStarEssenseItemUseCount = dailyStarEssenseItemUseCount;
		m_dtStarEssenseItemUseCountDate = dtDate;

		// Command
		CsRplzSession.Instance.EventResStarEssenseItemUse += OnEventResStarEssenseItemUse;
		CsRplzSession.Instance.EventResPremiumStarEssenseItemUse += OnEventResPremiumStarEssenseItemUse;
		CsRplzSession.Instance.EventResConstellationEntryActivate += OnEventResConstellationEntryActivate;
		CsRplzSession.Instance.EventResConstellationStepOpen += OnEventResConstellationStepOpen;

		// Event
		CsRplzSession.Instance.EventEvtConstellationOpened += OnEventEvtConstellationOpened;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResStarEssenseItemUse -= OnEventResStarEssenseItemUse;
		CsRplzSession.Instance.EventResPremiumStarEssenseItemUse -= OnEventResPremiumStarEssenseItemUse;
		CsRplzSession.Instance.EventResConstellationEntryActivate -= OnEventResConstellationEntryActivate;
		CsRplzSession.Instance.EventResConstellationStepOpen -= OnEventResConstellationStepOpen;

		// Event
		CsRplzSession.Instance.EventEvtConstellationOpened -= OnEventEvtConstellationOpened;

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellation GetHeroConstellation(int nConstellationId)
	{
		for (int i = 0; i < m_listCsHeroConstellation.Count; i++)
		{
			if (m_listCsHeroConstellation[i].Id == nConstellationId)
				return m_listCsHeroConstellation[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroConstellationStep GetHeroConstellationStep(int nConstellationId, int nStep)
	{
		CsHeroConstellation csHeroConstellation = GetHeroConstellation(nConstellationId);

		if (csHeroConstellation != null)
		{
			return csHeroConstellation.GetHeroConstellationStep(nStep);
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	public void GetCycleBuff(ref Dictionary<CsAttr, int> dicConstellationAttr, int nConstellationId, int nStepNo)
	{
		CsHeroConstellationStep csHeroConstellationStep = GetHeroConstellationStep(nConstellationId, nStepNo);

		CsConstellationCycle csConstellationCycle = null;

		if (csHeroConstellationStep.Activated)
		{
			// 현재 사이클
			csConstellationCycle = csHeroConstellationStep.ConstellationCycle;
		}
		else if (1 < csHeroConstellationStep.Cycle)
		{
			// 이전 사이클
			csConstellationCycle = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(csHeroConstellationStep.Cycle - 1);
		}

		if (csConstellationCycle != null)
		{
			foreach (CsConstellationCycleBuff csConstellationCycleBuff in csConstellationCycle.ConstellationCycleBuffList)
			{
				if (dicConstellationAttr.ContainsKey(csConstellationCycleBuff.Attr))
				{
					dicConstellationAttr[csConstellationCycleBuff.Attr] += csConstellationCycleBuff.AttrValue.Value;
				}
				else
				{
					dicConstellationAttr.Add(csConstellationCycleBuff.Attr, csConstellationCycleBuff.AttrValue.Value);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void GetEntryBuff(ref Dictionary<CsAttr, int> dicConstellationAttr, int nConstellationId, int nStepNo)
	{
		CsHeroConstellationStep csHeroConstellationStep = GetHeroConstellationStep(nConstellationId, nStepNo);

		CsConstellationEntry csConstellationEntry = null;

		if (csHeroConstellationStep.Activated)
		{
			// 현재 엔트리
			csConstellationEntry = csHeroConstellationStep.ConstellationEntry;
		}
		else if (1 < csHeroConstellationStep.EntryNo)
		{
			// 이전 엔트리
			csConstellationEntry = csHeroConstellationStep.ConstellationCycle.GetConstellationEntry(csHeroConstellationStep.EntryNo - 1);
		}
		else
		{
			if (1 < csHeroConstellationStep.Cycle)
			{
				// 이전 사이클의 마지막 엔트리
				CsConstellationCycle prevCycle = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(csHeroConstellationStep.Cycle - 1);

				csConstellationEntry = prevCycle.GetConstellationEntry(prevCycle.ConstellationEntryList.Max(entry => entry.EntryNo));
			}
		}

		if (csConstellationEntry != null)
		{
			foreach (CsConstellationEntryBuff csConstellationEntryBuff in csConstellationEntry.ConstellationEntryBuffList)
			{
				if (dicConstellationAttr.ContainsKey(csConstellationEntryBuff.Attr))
				{
					dicConstellationAttr[csConstellationEntryBuff.Attr] += csConstellationEntryBuff.AttrValue.Value;
				}
				else
				{
					dicConstellationAttr.Add(csConstellationEntryBuff.Attr, csConstellationEntryBuff.AttrValue.Value);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void GetNextEntryCycleBuff(ref Dictionary<CsAttr, int> dicConstellationAttr, int nConstellationId, int nStepNo)
	{
		CsHeroConstellationStep csHeroConstellationStep = GetHeroConstellationStep(nConstellationId, nStepNo);

		if (csHeroConstellationStep.Activated)
		{
			return;
		}

		foreach (CsConstellationEntryBuff csConstellationEntryBuff in csHeroConstellationStep.ConstellationEntry.ConstellationEntryBuffList)
		{
			if (dicConstellationAttr.ContainsKey(csConstellationEntryBuff.Attr))
			{
				dicConstellationAttr[csConstellationEntryBuff.Attr] += csConstellationEntryBuff.AttrValue.Value;
			}
			else
			{
				dicConstellationAttr.Add(csConstellationEntryBuff.Attr, csConstellationEntryBuff.AttrValue.Value);
			}
		}

		CsConstellationCycle csConstellationCycle = null;

		if (1 < csHeroConstellationStep.Cycle && csHeroConstellationStep.EntryNo < 12)
		{
			csConstellationCycle = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(csHeroConstellationStep.Cycle - 1);
		}
		else if (csHeroConstellationStep.EntryNo == 12)
		{
			csConstellationCycle = csHeroConstellationStep.ConstellationCycle;
		}

		if (csConstellationCycle != null)
		{
			foreach (CsConstellationCycleBuff csConstellationCycleBuff in csConstellationCycle.ConstellationCycleBuffList)
			{
				if (dicConstellationAttr.ContainsKey(csConstellationCycleBuff.Attr))
				{
					dicConstellationAttr[csConstellationCycleBuff.Attr] += csConstellationCycleBuff.AttrValue.Value;
				}
				else
				{
					dicConstellationAttr.Add(csConstellationCycleBuff.Attr, csConstellationCycleBuff.AttrValue.Value);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 별의정수 사용
	public void SendStarEssenseItemUse(int nSlotIndex, int nCount)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			StarEssenseItemUseCommandBody cmdBody = new StarEssenseItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			cmdBody.useCount = nCount;
			CsRplzSession.Instance.Send(ClientCommandName.StarEssenseItemUse, cmdBody);
		}
	}

	void OnEventResStarEssenseItemUse(int nReturnCode, StarEssenseItemUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			int nDifference = resBody.starEssesne - m_nStarEssense;

			m_nDailyStarEssenseItemUseCount = resBody.dailyStarEssenseItemUseCount;
			m_nStarEssense = resBody.starEssesne;
			
			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			if (EventStarEssenseItemUse != null)
			{
				EventStarEssenseItemUse(nDifference);
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001401"));
		}
		else if (nReturnCode == 102)
		{
			// 사용횟수가 최대횟수를 초과합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001402"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 고급 별의정수 사용
	public void SendPremiumStarEssenseItemUse(int nSlotIndex, int nCount)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			PremiumStarEssenseItemUseCommandBody cmdBody = new PremiumStarEssenseItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			cmdBody.useCount = nCount;
			CsRplzSession.Instance.Send(ClientCommandName.PremiumStarEssenseItemUse, cmdBody);
		}
	}

	void OnEventResPremiumStarEssenseItemUse(int nReturnCode, PremiumStarEssenseItemUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			int nDifference = resBody.starEssesne - m_nStarEssense;

			m_nStarEssense = resBody.starEssesne;
			
			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			if (EventPremiumStarEssenseItemUse != null)
			{
				EventPremiumStarEssenseItemUse(nDifference);
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A91_ERROR_001501"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 별자리개방 
	public void SendConstellationEntryActivate(int nConstellationId, int nStep, int nCycle, int nEntryNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ConstellationEntryActivateCommandBody cmdBody = new ConstellationEntryActivateCommandBody();
			cmdBody.constellationId = m_nConstellationId = nConstellationId;
			cmdBody.step = m_nStep = nStep;
			cmdBody.cycle = m_nCycle = nCycle;
			cmdBody.entryNo = m_nEntryNo = nEntryNo;
			CsRplzSession.Instance.Send(ClientCommandName.ConstellationEntryActivate, cmdBody);
		}
	}

	void OnEventResConstellationEntryActivate(int nReturnCode, ConstellationEntryActivateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;

			m_nStarEssense = resBody.starEssense;

			CsHeroConstellationStep csHeroConstellationStep = GetHeroConstellationStep(m_nConstellationId, m_nStep);

			if (resBody.success)
			{
				csHeroConstellationStep.FailPoint = 0;

				CsConstellationEntry csConstellationEntry = csHeroConstellationStep.ConstellationCycle.GetConstellationEntry(m_nEntryNo + 1);

				if (csConstellationEntry != null)	// 다음 엔트리가 있을 경우.
				{
					csHeroConstellationStep.EntryNo = m_nEntryNo + 1;
				}
				else                                // 다음 엔트리가 없을 경우. 
				{
					// 다음사이클이 있는지 조회
					CsConstellationCycle csConstellationCycle = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(m_nCycle + 1);

					if (csConstellationCycle != null)	// 다음 사이클이 있을 경우.
					{
						csHeroConstellationStep.Cycle = m_nCycle + 1;
						csHeroConstellationStep.EntryNo = 1;
					}
					else                                // 다음 사이클이 없으면 마지막.                    
					{
						csHeroConstellationStep.Activated = true;
					}
				}
			}
			else
			{
				csHeroConstellationStep.FailPoint = resBody.failPoint;
			}

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventConstellationEntryActivate != null)
			{
				EventConstellationEntryActivate(resBody.success);
			}
		}
		else if (nReturnCode == 101)
		{
			// 별자리가 개방되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A163_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 별자리단계가 개방되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A163_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 별의정수가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A163_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 골드가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A163_ERROR_00104"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}


	//---------------------------------------------------------------------------------------------------
	// 별자리개방 
	public void SendConstellationStepOpen(int nConstellationId, int nStep)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ConstellationStepOpenCommandBody cmdBody = new ConstellationStepOpenCommandBody();
			cmdBody.constellationId = m_nConstellationId = nConstellationId;
			cmdBody.step = m_nStep = nStep;
			CsRplzSession.Instance.Send(ClientCommandName.ConstellationStepOpen, cmdBody);
		}
	}

	void OnEventResConstellationStepOpen(int nReturnCode, ConstellationStepOpenResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroConstellation csHeroConstellation = GetHeroConstellation(m_nConstellationId);
			csHeroConstellation.HeroConstellationStepList.Add(new CsHeroConstellationStep(m_nConstellationId, m_nStep));

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			if (EventConstellationStepOpen != null)
			{
				EventConstellationStepOpen(m_nStep);
			}
		}
		else if (nReturnCode == 101)
		{
			// 다이아가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A163_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 별자리개방 
	void OnEventEvtConstellationOpened(SEBConstellationOpenedEventBody eventBody)
	{
		for (int i = 0; i < eventBody.constellations.Length; i++)
		{
			m_listCsHeroConstellation.Add(new CsHeroConstellation(eventBody.constellations[i]));
		}

		if (EventConstellationOpened != null)
		{
			EventConstellationOpened();
		}
	}

	#endregion Protocol.Event

}
