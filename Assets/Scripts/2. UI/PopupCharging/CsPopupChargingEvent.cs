using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-01)
//---------------------------------------------------------------------------------------------------

enum EnChargingEventMenu
{
	None,
	FirstCharging,
	ReCharging,
	ChargingEvent,
	DailyChargingEvent,
	ConsumptionEvent,
	DailyConsumptionEvent
}

public class CsPopupChargingEvent : MonoBehaviour 
{
	[SerializeField]
	GameObject m_goItemSlot;

	Transform m_trImageBackground;
	GameObject m_goDisplayChargingReward;
	GameObject m_goDisplayChargingEventReward;

	EnChargingEventMenu m_enChargingEventMenu = EnChargingEventMenu.None;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;

		// 첫충전
		CsCashManager.Instance.EventFirstChargeEventObjectiveCompleted += OnEventFirstChargeEventObjectiveCompleted;
		CsCashManager.Instance.EventFirstChargeEventRewardReceive += OnEventFirstChargeEventRewardReceive;

		// 재충전
		CsCashManager.Instance.EventRechargeEventProgress += OnEventRechargeEventProgress;
		CsCashManager.Instance.EventRechargeEventRewardReceive += OnEventRechargeEventRewardReceive;

		// 누적충전
		CsCashManager.Instance.EventChargeEventProgress += OnEventChargeEventProgress;
		CsCashManager.Instance.EventChargeEventMissionRewardReceive += OnEventChargeEventMissionRewardReceive;

		// 매일충전
		CsCashManager.Instance.EventDailyChargeEventProgress += OnEventDailyChargeEventProgress;
		CsCashManager.Instance.EventDailyChargeEventMissionRewardReceive += OnEventDailyChargeEventMissionRewardReceive;

		// 누적소비
		CsCashManager.Instance.EventConsumeEventProgress += OnEventConsumeEventProgress;
		CsCashManager.Instance.EventConsumeEventMissionRewardReceive += OnEventConsumeEventMissionRewardReceive;

		// 매일소비
		CsCashManager.Instance.EventDailyConsumeEventProgress += OnEventDailyConsumeEventProgress;
		CsCashManager.Instance.EventDailyConsumeEventMissionRewardReceive += OnEventDailyConsumeEventMissionRewardReceive;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;

		// 첫충전
		CsCashManager.Instance.EventFirstChargeEventObjectiveCompleted -= OnEventFirstChargeEventObjectiveCompleted;
		CsCashManager.Instance.EventFirstChargeEventRewardReceive -= OnEventFirstChargeEventRewardReceive;

		// 재충전
		CsCashManager.Instance.EventRechargeEventProgress -= OnEventRechargeEventProgress;
		CsCashManager.Instance.EventRechargeEventRewardReceive -= OnEventRechargeEventRewardReceive;

		// 누적충전
		CsCashManager.Instance.EventChargeEventProgress -= OnEventChargeEventProgress;
		CsCashManager.Instance.EventChargeEventMissionRewardReceive -= OnEventChargeEventMissionRewardReceive;

		// 매일충전
		CsCashManager.Instance.EventDailyChargeEventProgress -= OnEventDailyChargeEventProgress;
		CsCashManager.Instance.EventDailyChargeEventMissionRewardReceive -= OnEventDailyChargeEventMissionRewardReceive;

		// 누적소비
		CsCashManager.Instance.EventConsumeEventProgress -= OnEventConsumeEventProgress;
		CsCashManager.Instance.EventConsumeEventMissionRewardReceive -= OnEventConsumeEventMissionRewardReceive;

		// 매일소비
		CsCashManager.Instance.EventDailyConsumeEventProgress -= OnEventDailyConsumeEventProgress;
		CsCashManager.Instance.EventDailyConsumeEventMissionRewardReceive -= OnEventDailyConsumeEventMissionRewardReceive;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_goDisplayChargingReward = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/DisplayChargingReward");
		m_goDisplayChargingEventReward = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/DisplayChargingEventReward");

		m_trImageBackground = transform.Find("ImageBackground");

		CsUIData.Instance.SetText(m_trImageBackground.Find("TextTitle"), "A125_NAME_00001", true);
		CsUIData.Instance.SetButton(m_trImageBackground.Find("ButtonClose"), PopupClose);

		GameObject goToggleChargingEvent = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/ToggleChargingEvent");

		Transform trFrameToggles = m_trImageBackground.Find("FrameToggles");
		ToggleGroup toggleGroup = trFrameToggles.GetComponent<ToggleGroup>();

		foreach(EnChargingEventMenu enumItem in System.Enum.GetValues(typeof(EnChargingEventMenu)))
		{
			if (enumItem == EnChargingEventMenu.None)
				continue;

			Transform trToggle = Instantiate(goToggleChargingEvent, trFrameToggles).transform;
			trToggle.name = enumItem.ToString();

			Toggle toggle = trToggle.GetComponent<Toggle>();
			toggle.group = toggleGroup;
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggleChargingEvent(isOn, enumItem));

			switch (enumItem)
			{
				case EnChargingEventMenu.FirstCharging:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00001", true);
					break;

				case EnChargingEventMenu.ReCharging:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00003", true);
					break;

				case EnChargingEventMenu.ChargingEvent:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00004", true);
					break;

				case EnChargingEventMenu.DailyChargingEvent:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00007", true);
					break;

				case EnChargingEventMenu.ConsumptionEvent:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00008", true);
					break;

				case EnChargingEventMenu.DailyConsumptionEvent:
					CsUIData.Instance.SetText(trToggle.Find("Text"), "A125_BTN_00010", true);
					break;
			}
		}

		UpdateDisplayToggles();
		SelectFirstToggle();
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDisplayToggles()
	{
		foreach (EnChargingEventMenu enChargingEventMenu in Enum.GetValues(typeof(EnChargingEventMenu)))
		{
			UpdateDisplayToggle(enChargingEventMenu);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDisplayToggle(EnChargingEventMenu enChargingEventMenu)
	{
		if (enChargingEventMenu == EnChargingEventMenu.None)
			return;

		Transform trToggle = m_trImageBackground.Find("FrameToggles/" + enChargingEventMenu.ToString());
		Transform trImageNotice = trToggle.Find("ImageNotice");

		bool bDisplay = false;

		switch (enChargingEventMenu)
		{
			case EnChargingEventMenu.FirstCharging:
				if (!CsCashManager.Instance.FirstChargeEventRewarded)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckFirstChargingEvent());
				}
				break;
			case EnChargingEventMenu.ReCharging:
				if (CsCashManager.Instance.FirstChargeEventRewarded && !CsCashManager.Instance.RechargeEventRewarded)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckReChargingEvent());
				}
				break;
			case EnChargingEventMenu.ChargingEvent:
				if (CsGameConfig.Instance.ChargeEventRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
					CsGameData.Instance.GetCurrentChargeEvent() != null)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckChargingEvent());
				}
				break;
			case EnChargingEventMenu.DailyChargingEvent:
				if (CsGameData.Instance.DailyChargeEvent.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckDailyChargingEvent());
				}
				break;
			case EnChargingEventMenu.ConsumptionEvent:
				if (CsGameConfig.Instance.ConsumeEventRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level &&
					CsGameData.Instance.GetCurrentConsumeEvent() != null)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckConsumptionEvent());
				}
				break;
			case EnChargingEventMenu.DailyConsumptionEvent:
				if (CsGameData.Instance.DailyConsumeEvent.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
				{
					bDisplay = true;

					trImageNotice.gameObject.SetActive(CsCashManager.Instance.CheckDailyConsumptionEvent());
				}
				break;
		}

		trToggle.gameObject.SetActive(bDisplay);
	}

	//---------------------------------------------------------------------------------------------------
	void SelectFirstToggle()
	{
		Transform trFrameToggles = m_trImageBackground.Find("FrameToggles");

		for (int i = 0; i < trFrameToggles.childCount; i++)
		{
			if (trFrameToggles.GetChild(i).gameObject.activeSelf)
			{
				trFrameToggles.GetChild(i).GetComponent<Toggle>().isOn = true;
				break;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateChargingDisplay()
	{
		Transform trDisplayCharging = m_trImageBackground.Find("DisplayChargingReward");

		Transform trFrameFirstCharging = trDisplayCharging.Find("FrameFirstCharging");
		Transform trFrameReCharging = trDisplayCharging.Find("FrameReCharging");

		trFrameFirstCharging.gameObject.SetActive(m_enChargingEventMenu == EnChargingEventMenu.FirstCharging);
		trFrameReCharging.gameObject.SetActive(m_enChargingEventMenu == EnChargingEventMenu.ReCharging);

		Transform trButtonCharging = trDisplayCharging.Find("ButtonCharging");
		Transform trButtonReceipt = trDisplayCharging.Find("ButtonReceipt");

		Transform trFrameReward = null;

		if (m_enChargingEventMenu == EnChargingEventMenu.FirstCharging)
		{
			trButtonCharging.gameObject.SetActive(!CsCashManager.Instance.FirstChargeEventObjectiveCompleted);
			trButtonReceipt.gameObject.SetActive(CsCashManager.Instance.FirstChargeEventObjectiveCompleted && !CsCashManager.Instance.FirstChargeEventRewarded);

			CsUIData.Instance.SetText(trFrameFirstCharging.Find("ImageTitle/TextTitle"), "A125_TXT_00001", true);
			CsUIData.Instance.SetText(trFrameFirstCharging.Find("TextDescription"), "A125_TXT_00002", true);

			trFrameReward = trFrameFirstCharging.Find("FrameReward");
		}
		else if (m_enChargingEventMenu == EnChargingEventMenu.ReCharging)
		{
			trButtonCharging.gameObject.SetActive(CsCashManager.Instance.RechargeEventAccUnOwnDia < CsGameData.Instance.RechargeEvent.RequiredUnOwnDia);
			trButtonReceipt.gameObject.SetActive(CsCashManager.Instance.RechargeEventAccUnOwnDia >= CsGameData.Instance.RechargeEvent.RequiredUnOwnDia && !CsCashManager.Instance.RechargeEventRewarded);

			CsUIData.Instance.SetText(trFrameReCharging.Find("ImageTitle/TextTitle"), "A125_TXT_00003", true);
			CsUIData.Instance.SetText(trFrameReCharging.Find("ImageFrameDescription0/TextDescription"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01001"), CsGameData.Instance.RechargeEvent.RequiredUnOwnDia - CsCashManager.Instance.RechargeEventAccUnOwnDia), false);
			// CsUIData.Instance.SetText(trFrameReCharging.Find("ImageFrameDescription1/TextDescription"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01002"), "신상패키지 가격"), false);

			trFrameReward = trFrameReCharging.Find("FrameReward");
		}

		if (trFrameReward != null)
		{
			for (int i = 0; i < trFrameReward.childCount; i++)
			{
				trFrameReward.GetChild(i).gameObject.SetActive(false);
			}

			var rewardList = (m_enChargingEventMenu == EnChargingEventMenu.FirstCharging) ? CsGameData.Instance.FirstChargeEvent.FirstChargeEventRewardList.Select(reward => reward.ItemReward) :
																							CsGameData.Instance.RechargeEvent.RechargeEventRewardList.Select(reward => reward.ItemReward);

			GameObject goChargingRewardWing = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/ChargingRewardWing");
			GameObject goChargingRewardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/popupCharging/ChargingRewardItem");

			int nWingCount = 0;
			int nItemCount = 0;

			foreach (var reward in rewardList)
			{
				Transform trChargingReward = null;

				if (reward.Item.ItemType.EnItemType == EnItemType.Wing)
				{
					trChargingReward = trFrameReward.Find("ChargingRewardWing" + nWingCount.ToString());

					if (trChargingReward == null)
					{
						trChargingReward = Instantiate(goChargingRewardWing, trFrameReward).transform;
						trChargingReward.name = "ChargingRewardWing" + nWingCount.ToString();
					}
					else
					{
						trChargingReward.gameObject.SetActive(true);
					}

					nWingCount++;
				}
				else
				{
					trChargingReward = trFrameReward.Find("ChargingRewardItem" + nItemCount.ToString());

					if (trChargingReward == null)
					{
						trChargingReward = Instantiate(goChargingRewardItem, trFrameReward).transform;
						trChargingReward.name = "ChargingRewardItem" + nItemCount.ToString();
					}
					else
					{
						trChargingReward.gameObject.SetActive(true);
					}

					nItemCount++;
				}

				CsUIData.Instance.DisplayItemSlot(trChargingReward.Find("ItemSlot"), reward.Item, reward.ItemOwned, reward.ItemCount, false, EnItemSlotSize.Medium, false);
				CsUIData.Instance.SetButton(trChargingReward.Find("ItemSlot"), () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(reward.Item));
				CsUIData.Instance.SetText(trChargingReward.Find("TextValueName"), reward.Item.Name, false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateEventDisplay()
	{
		Transform trDisplay = m_trImageBackground.Find("DisplayChargingEventReward");

		Transform trTextDescription = trDisplay.Find("TextDescription");
		Transform trTextAccumulatedDia = trDisplay.Find("ImageFrameInfo/TextAccumulatedDia");
		Transform trTextValueEventTime = trDisplay.Find("ImageFrameInfo/TextValueEventTime");

		Transform trContent = trDisplay.Find("Scroll View/Viewport/Content");

		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
			trContent.GetChild(i).name = "";
		}

		GameObject goChargingEventMission = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCharging/ChargingEventMission");

		switch (m_enChargingEventMenu)
		{
			case EnChargingEventMenu.ChargingEvent:
			{
				CsUIData.Instance.SetText(trTextDescription, "A125_TXT_00017", true);

				if (CsCashManager.Instance.AccountChargeEvent != null)
				{
					CsUIData.Instance.SetText(trTextAccumulatedDia, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01003"), CsCashManager.Instance.AccountChargeEvent.AccUnOwnDia), false);
				}

				CsChargeEvent csChargeEvent = CsGameData.Instance.GetCurrentChargeEvent();

				if (csChargeEvent != null)
				{
					DateTime startTime = csChargeEvent.StartTime;
					DateTime endTime = csChargeEvent.EndTime;

					CsUIData.Instance.SetText(trTextValueEventTime, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01004"),
																			startTime.Year.ToString(), startTime.Month.ToString(), startTime.Day.ToString(),
																			startTime.Hour.ToString("00"), startTime.Minute.ToString("00"),
																			endTime.Year.ToString(), endTime.Month.ToString(), endTime.Day.ToString(),
																			endTime.Hour.ToString("00"), endTime.Minute.ToString("00")), false);

					// 미션
					int nChildCount = 0;

					foreach (var mission in csChargeEvent.ChargeEventMissionList)
					{
						Transform trMission = null;

						if (nChildCount < trContent.childCount)
						{
							trMission = trContent.GetChild(nChildCount);
							trMission.gameObject.SetActive(true);
						}
						else
						{
							trMission = Instantiate(goChargingEventMission, trContent).transform;
						}

						trMission.name = mission.MissionNo.ToString();

						CsUIData.Instance.SetText(trMission.Find("TextContent"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01005"), mission.RequiredUnOwnDia), false);

						// 보상
						Transform trFrameReward = trMission.Find("FrameReward");

						for (int i = 0; i < trFrameReward.childCount; i++)
						{
							trFrameReward.GetChild(i).gameObject.SetActive(false);
							trFrameReward.GetChild(i).name = "";
						}

						int nRewardCount = 0;
						foreach (var missionReward in mission.ChargeEventMissionRewardList)
						{
							Transform trSlot = null;

							if (nRewardCount < trFrameReward.childCount)
							{
								trSlot = trFrameReward.GetChild(nRewardCount);
								trSlot.gameObject.SetActive(true);
							}
							else
							{
								trSlot = Instantiate(m_goItemSlot, trFrameReward).transform;
							}

							trSlot.name = missionReward.RewardNo.ToString();

							CsUIData.Instance.DisplayItemSlot(trSlot, missionReward.ItemReward.Item, missionReward.ItemReward.ItemOwned, missionReward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);
							CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(missionReward.ItemReward.Item));

							nRewardCount++;
						}

						UpdateEventMissionButton(mission.MissionNo, csChargeEvent, null);

						nChildCount++;
					}
				}
			}
				break;

			case EnChargingEventMenu.DailyChargingEvent:
			{
				CsUIData.Instance.SetText(trTextDescription, "A125_TXT_00004", true);
				CsUIData.Instance.SetText(trTextAccumulatedDia, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01006"), CsCashManager.Instance.DailyChargeEventAccUnOwnDia), false);

				DateTime today = CsGameData.Instance.MyHeroInfo.CurrentDateTime;

				CsUIData.Instance.SetText(trTextValueEventTime, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01004"),
																			today.Year.ToString(), today.Month.ToString(), today.Day.ToString(), "00", "00",
																			today.Year.ToString(), today.Month.ToString(), today.Day.ToString(), "23", "59"), false);

				// 미션
				int nChildCount = 0;

				foreach (var mission in CsGameData.Instance.DailyChargeEvent.DailyChargeEventMissionList)
				{
					Transform trMission = null;

					if (nChildCount < trContent.childCount)
					{
						trMission = trContent.GetChild(nChildCount);
						trMission.gameObject.SetActive(true);
					}
					else
					{
						trMission = Instantiate(goChargingEventMission, trContent).transform;
					}

					trMission.name = mission.MissionNo.ToString();

					CsUIData.Instance.SetText(trMission.Find("TextContent"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01005"), mission.RequiredUnOwnDia), false);

					// 보상
					Transform trFrameReward = trMission.Find("FrameReward");

					for (int i = 0; i < trFrameReward.childCount; i++)
					{
						trFrameReward.GetChild(i).gameObject.SetActive(false);
						trFrameReward.GetChild(i).name = "";
					}

					int nRewardCount = 0;
					foreach (var missionReward in mission.DailyChargeEventMissionRewardList)
					{
						Transform trSlot = null;

						if (nRewardCount < trFrameReward.childCount)
						{
							trSlot = trFrameReward.GetChild(nRewardCount);
							trSlot.gameObject.SetActive(true);
						}
						else
						{
							trSlot = Instantiate(m_goItemSlot, trFrameReward).transform;
						}

						trSlot.name = missionReward.RewardNo.ToString();

						CsUIData.Instance.DisplayItemSlot(trSlot, missionReward.ItemReward.Item, missionReward.ItemReward.ItemOwned, missionReward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);
						CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(missionReward.ItemReward.Item));

						nRewardCount++;
					}

					UpdateEventMissionButton(mission.MissionNo);

					nChildCount++;
				}
			}
				break;

			case EnChargingEventMenu.ConsumptionEvent:
			{
				CsUIData.Instance.SetText(trTextDescription, "A125_TXT_00018", true);

				if (CsCashManager.Instance.AccountConsumeEvent != null)
				{
					CsUIData.Instance.SetText(trTextAccumulatedDia, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01007"), CsCashManager.Instance.AccountConsumeEvent.AccDia), false);
				}

				CsConsumeEvent csConsumeEvent = CsGameData.Instance.GetCurrentConsumeEvent();

				if (csConsumeEvent != null)
				{
					DateTime startTime = csConsumeEvent.StartTime;
					DateTime endTime = csConsumeEvent.EndTime;

					CsUIData.Instance.SetText(trTextValueEventTime, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01004"),
																			startTime.Year.ToString(), startTime.Month.ToString(), startTime.Day.ToString(),
																			startTime.Hour.ToString("00"), startTime.Minute.ToString("00"),
																			endTime.Year.ToString(), endTime.Month.ToString(), endTime.Day.ToString(),
																			endTime.Hour.ToString("00"), endTime.Minute.ToString("00")), false);

					// 미션
					int nChildCount = 0;

					foreach (var mission in csConsumeEvent.ConsumeEventMissionList)
					{
						Transform trMission = null;

						if (nChildCount < trContent.childCount)
						{
							trMission = trContent.GetChild(nChildCount);
							trMission.gameObject.SetActive(true);
						}
						else
						{
							trMission = Instantiate(goChargingEventMission, trContent).transform;
						}

						trMission.name = mission.MissionNo.ToString();

						CsUIData.Instance.SetText(trMission.Find("TextContent"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01008"), mission.RequiredDia), false);

						// 보상
						Transform trFrameReward = trMission.Find("FrameReward");

						for (int i = 0; i < trFrameReward.childCount; i++)
						{
							trFrameReward.GetChild(i).gameObject.SetActive(false);
							trFrameReward.GetChild(i).name = "";
						}

						int nRewardCount = 0;
						foreach (var missionReward in mission.ConsumeEventMissionRewardList)
						{
							Transform trSlot = null;

							if (nRewardCount < trFrameReward.childCount)
							{
								trSlot = trFrameReward.GetChild(nRewardCount);
								trSlot.gameObject.SetActive(true);
							}
							else
							{
								trSlot = Instantiate(m_goItemSlot, trFrameReward).transform;
							}

							trSlot.name = missionReward.RewardNo.ToString();

							CsUIData.Instance.DisplayItemSlot(trSlot, missionReward.ItemReward.Item, missionReward.ItemReward.ItemOwned, missionReward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);
							CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(missionReward.ItemReward.Item));

							nRewardCount++;
						}

						UpdateEventMissionButton(mission.MissionNo, null, csConsumeEvent);

						nChildCount++;
					}
				}
			}
				break;

			case EnChargingEventMenu.DailyConsumptionEvent:
			{
				CsUIData.Instance.SetText(trTextDescription, "A125_TXT_00006", true);
				CsUIData.Instance.SetText(trTextAccumulatedDia, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01009"), CsCashManager.Instance.DailyConsumeEventAccDia), false);

				DateTime today = CsGameData.Instance.MyHeroInfo.CurrentDateTime;

				CsUIData.Instance.SetText(trTextValueEventTime, string.Format(CsConfiguration.Instance.GetString("A125_TXT_01004"),
																			today.Year.ToString(), today.Month.ToString(), today.Day.ToString(), "00", "00",
																			today.Year.ToString(), today.Month.ToString(), today.Day.ToString(), "23", "59"), false);

				// 미션
				int nChildCount = 0;

				foreach (var mission in CsGameData.Instance.DailyConsumeEvent.DailyConsumeEventMissionList)
				{
					Transform trMission = null;

					if (nChildCount < trContent.childCount)
					{
						trMission = trContent.GetChild(nChildCount);
						trMission.gameObject.SetActive(true);
					}
					else
					{
						trMission = Instantiate(goChargingEventMission, trContent).transform;
					}

					trMission.name = mission.MissionNo.ToString();

					CsUIData.Instance.SetText(trMission.Find("TextContent"), string.Format(CsConfiguration.Instance.GetString("A125_TXT_01008"), mission.RequiredDia), false);

					// 보상
					Transform trFrameReward = trMission.Find("FrameReward");

					for (int i = 0; i < trFrameReward.childCount; i++)
					{
						trFrameReward.GetChild(i).gameObject.SetActive(false);
						trFrameReward.GetChild(i).name = "";
					}

					int nRewardCount = 0;
					foreach (var missionReward in mission.DailyConsumeEventMissionRewardList)
					{
						Transform trSlot = null;

						if (nRewardCount < trFrameReward.childCount)
						{
							trSlot = trFrameReward.GetChild(nRewardCount);
							trSlot.gameObject.SetActive(true);
						}
						else
						{
							trSlot = Instantiate(m_goItemSlot, trFrameReward).transform;
						}

						trSlot.name = missionReward.RewardNo.ToString();

						CsUIData.Instance.DisplayItemSlot(trSlot, missionReward.ItemReward.Item, missionReward.ItemReward.ItemOwned, missionReward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);
						CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(missionReward.ItemReward.Item));

						nRewardCount++;
					}

					UpdateEventMissionButton(mission.MissionNo);

					nChildCount++;
				}
			}
				break;

			default:
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateEventMissionButton(int nMissionNo, CsChargeEvent csChargeEvent = null, CsConsumeEvent csConsumeEvent = null)
	{
		Transform trDisplay = m_trImageBackground.Find("DisplayChargingEventReward");
		Transform trContent = trDisplay.Find("Scroll View/Viewport/Content");

		Transform trMission = trContent.Find(nMissionNo.ToString());
		Transform trFrameReceived = trMission.Find("FrameReceived");
		Transform trButtonReceipt = trMission.Find("ButtonReceipt");

		bool bIsRewardedMission = false;
		trFrameReceived.gameObject.SetActive(false);
		trButtonReceipt.gameObject.SetActive(false);

		switch (m_enChargingEventMenu)
		{
			case EnChargingEventMenu.ChargingEvent:
				if (CsCashManager.Instance.AccountChargeEvent != null)
				{
					if (csChargeEvent == null)
					{
						csChargeEvent = CsGameData.Instance.GetCurrentChargeEvent();
					}

					if (csChargeEvent != null)
					{
						CsChargeEventMission csChargeEventMission = csChargeEvent.GetChargeEventMission(nMissionNo);

						if (csChargeEventMission != null)
						{
							bIsRewardedMission = CsCashManager.Instance.AccountChargeEvent.IsRewardedMission(csChargeEventMission.MissionNo);

							trFrameReceived.gameObject.SetActive(bIsRewardedMission);
							trButtonReceipt.gameObject.SetActive(!bIsRewardedMission);

							if (bIsRewardedMission)
							{
								CsUIData.Instance.SetText(trFrameReceived.Find("TextReceived"), "A125_TXT_00005", true);
							}
							else
							{
								CsUIData.Instance.SetButton(trButtonReceipt, () => OnClickButtonEventReceipt(csChargeEventMission.MissionNo, csChargeEvent, null));
								CsUIData.Instance.SetText(trButtonReceipt.Find("TextReceipt"), "A125_BTN_00006", true);

								Button button = trButtonReceipt.GetComponent<Button>();
								button.interactable = csChargeEventMission.RequiredUnOwnDia <= CsCashManager.Instance.AccountChargeEvent.AccUnOwnDia;
							}
						}
					}
				}
				break;

			case EnChargingEventMenu.DailyChargingEvent:
				if (CsCashManager.Instance.DailyChargeEventDate.Date == CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
				{
					CsDailyChargeEventMission csDailyChargeEventMission = CsGameData.Instance.DailyChargeEvent.GetDailyChargeEventMission(nMissionNo);

					if (csDailyChargeEventMission != null)
					{
						bIsRewardedMission = CsCashManager.Instance.RewardedDailyChargeEventMissionList.Contains(csDailyChargeEventMission.MissionNo);

						trFrameReceived.gameObject.SetActive(bIsRewardedMission);
						trButtonReceipt.gameObject.SetActive(!bIsRewardedMission);

						if (bIsRewardedMission)
						{
							CsUIData.Instance.SetText(trFrameReceived.Find("TextReceived"), "A125_TXT_00005", true);
						}
						else
						{
							CsUIData.Instance.SetButton(trButtonReceipt, () => OnClickButtonEventReceipt(csDailyChargeEventMission.MissionNo));
							CsUIData.Instance.SetText(trButtonReceipt.Find("TextReceipt"), "A125_BTN_00006", true);

							Button button = trButtonReceipt.GetComponent<Button>();
							button.interactable = csDailyChargeEventMission.RequiredUnOwnDia <= CsCashManager.Instance.DailyChargeEventAccUnOwnDia;
						}
					}
				}
	
				break;

			case EnChargingEventMenu.ConsumptionEvent:
				if (CsCashManager.Instance.AccountConsumeEvent != null)
				{
					if (csConsumeEvent == null)
					{
						csConsumeEvent = CsGameData.Instance.GetCurrentConsumeEvent();
					}

					if (csConsumeEvent != null)
					{
						CsConsumeEventMission csConsumeEventMission = csConsumeEvent.GetConsumeEventMission(nMissionNo);

						if (csConsumeEventMission != null)
						{
							bIsRewardedMission = CsCashManager.Instance.AccountConsumeEvent.IsRewardedMission(csConsumeEventMission.MissionNo);

							trFrameReceived.gameObject.SetActive(bIsRewardedMission);
							trButtonReceipt.gameObject.SetActive(!bIsRewardedMission);

							if (bIsRewardedMission)
							{
								CsUIData.Instance.SetText(trFrameReceived.Find("TextReceived"), "A125_TXT_00005", true);
							}
							else
							{
								CsUIData.Instance.SetButton(trButtonReceipt, () => OnClickButtonEventReceipt(csConsumeEventMission.MissionNo, null, csConsumeEvent));
								CsUIData.Instance.SetText(trButtonReceipt.Find("TextReceipt"), "A125_BTN_00006", true);

								Button button = trButtonReceipt.GetComponent<Button>();
								button.interactable = csConsumeEventMission.RequiredDia <= CsCashManager.Instance.AccountConsumeEvent.AccDia;
							}
						}
					}
				}

				break;

			case EnChargingEventMenu.DailyConsumptionEvent:
				if (CsCashManager.Instance.DailyConsumeEventDate.Date == CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
				{
					CsDailyConsumeEventMission csDailyConsumeEventMission = CsGameData.Instance.DailyConsumeEvent.GetDailyConsumeEventMission(nMissionNo);

					if (csDailyConsumeEventMission != null)
					{
						bIsRewardedMission = CsCashManager.Instance.RewardedDailyConsumeEventMissionList.Contains(csDailyConsumeEventMission.MissionNo);

						trFrameReceived.gameObject.SetActive(bIsRewardedMission);
						trButtonReceipt.gameObject.SetActive(!bIsRewardedMission);

						if (bIsRewardedMission)
						{
							CsUIData.Instance.SetText(trFrameReceived.Find("TextReceived"), "A125_TXT_00005", true);
						}
						else
						{
							CsUIData.Instance.SetButton(trButtonReceipt, () => OnClickButtonEventReceipt(csDailyConsumeEventMission.MissionNo));
							CsUIData.Instance.SetText(trButtonReceipt.Find("TextReceipt"), "A125_BTN_00006", true);

							Button button = trButtonReceipt.GetComponent<Button>();
							button.interactable = csDailyConsumeEventMission.RequiredDia <= CsCashManager.Instance.DailyConsumeEventAccDia;
						}
					}
				}

				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DisplayContents()
	{
		Transform trDisplay = null;

		switch (m_enChargingEventMenu)
		{
			case EnChargingEventMenu.FirstCharging:
			case EnChargingEventMenu.ReCharging:
				if (m_trImageBackground.Find("DisplayChargingEventReward") != null)
				{
					m_trImageBackground.Find("DisplayChargingEventReward").gameObject.SetActive(false);
				}

				trDisplay = m_trImageBackground.Find("DisplayChargingReward");

				if (trDisplay == null)
				{
					trDisplay = Instantiate(m_goDisplayChargingReward, m_trImageBackground).transform;
					trDisplay.name = "DisplayChargingReward";

					CsUIData.Instance.SetButton(trDisplay.Find("ButtonCharging"), OnClickButtonCharging);
					CsUIData.Instance.SetButton(trDisplay.Find("ButtonReceipt"), OnClickButtonReceipt);
					CsUIData.Instance.SetText(trDisplay.Find("ButtonCharging/TextCharging"), "A125_BTN_00002", true);
					CsUIData.Instance.SetText(trDisplay.Find("ButtonReceipt/TextReceipt"), "A125_BTN_00006", true);
				}
				else
				{
					trDisplay.gameObject.SetActive(true);
				}

				UpdateChargingDisplay();

				break;

			case EnChargingEventMenu.ChargingEvent:
			case EnChargingEventMenu.DailyChargingEvent:
			case EnChargingEventMenu.ConsumptionEvent:
			case EnChargingEventMenu.DailyConsumptionEvent:
				if (m_trImageBackground.Find("DisplayChargingReward") != null)
				{
					m_trImageBackground.Find("DisplayChargingReward").gameObject.SetActive(false);
				}

				trDisplay = m_trImageBackground.Find("DisplayChargingEventReward");

				if (trDisplay == null)
				{
					trDisplay = Instantiate(m_goDisplayChargingEventReward, m_trImageBackground).transform;
					trDisplay.name = "DisplayChargingEventReward";

					CsUIData.Instance.SetButton(trDisplay.Find("ImageFrameInfo/ButtonShop"), OnClickButtonShop);
					CsUIData.Instance.SetButton(trDisplay.Find("ImageFrameInfo/ButtonCharging"), OnClickButtonCharging);
					CsUIData.Instance.SetText(trDisplay.Find("ImageFrameInfo/ButtonShop/TextShop"), "A125_BTN_00009", true);
					CsUIData.Instance.SetText(trDisplay.Find("ImageFrameInfo/ButtonCharging/TextCharging"), "A125_BTN_00005", true);
				}
				else
				{
					trDisplay.gameObject.SetActive(true);
				}

				trDisplay.Find("ImageFrameInfo/ButtonShop").gameObject.SetActive(m_enChargingEventMenu == EnChargingEventMenu.ConsumptionEvent || m_enChargingEventMenu == EnChargingEventMenu.DailyConsumptionEvent);

				UpdateEventDisplay();

				trDisplay.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

				break;

			default:
				break;
		}
	}

	#region Event

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleChargingEvent(bool bIsOn, EnChargingEventMenu enChargingEventMenu)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			m_enChargingEventMenu = enChargingEventMenu;

			DisplayContents();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 첫 충전, 재충전 이벤트 보상
	void OnClickButtonReceipt()
	{
		if (m_enChargingEventMenu == EnChargingEventMenu.FirstCharging)
		{
			// 인벤토리 체크
			var rewards = CsGameData.Instance.FirstChargeEvent.FirstChargeEventRewardList.Select(reward => reward.ItemReward);

			if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
			{
				CsCashManager.Instance.SendFirstChargeEventRewardReceive();
			}
			else
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
			}
		}
		else if (m_enChargingEventMenu == EnChargingEventMenu.ReCharging)
		{
			// 인벤토리 체크
			var rewards = CsGameData.Instance.RechargeEvent.RechargeEventRewardList.Select(reward => reward.ItemReward);

			if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
			{
				CsCashManager.Instance.SendRechargeEventRewardReceive();
			}
			else
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCharging()
	{
		CsGameEventUIToUI.Instance.OnEventOpenPopupCharging();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonShop()
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
	}

	//---------------------------------------------------------------------------------------------------
	// 구매, 소비 이벤트 보상
	void OnClickButtonEventReceipt(int nMissionNo, CsChargeEvent csChargeEvent = null, CsConsumeEvent csConsumeEvent = null)
	{
		switch (m_enChargingEventMenu)
		{
			case EnChargingEventMenu.ChargingEvent:

				if (csChargeEvent != null)
				{
					CsChargeEventMission csChargeEventMission = csChargeEvent.GetChargeEventMission(nMissionNo);

					if (csChargeEventMission != null)
					{
						// 인벤토리 체크
						var rewards = csChargeEventMission.ChargeEventMissionRewardList.Select(reward => reward.ItemReward);

						if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
						{
							CsCashManager.Instance.SendChargeEventMissionRewardReceive(csChargeEvent.EventId, nMissionNo);
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
						}
					}
				}

				break;

			case EnChargingEventMenu.DailyChargingEvent:

				CsDailyChargeEventMission csDailyChargeEventMission = CsGameData.Instance.DailyChargeEvent.GetDailyChargeEventMission(nMissionNo);

				if (csDailyChargeEventMission != null)
				{
					// 인벤토리 체크
					var rewards = csDailyChargeEventMission.DailyChargeEventMissionRewardList.Select(reward => reward.ItemReward);

					if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
					{
						CsCashManager.Instance.SendDailyChargeEventMissionRewardReceive(nMissionNo);
					}
					else
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
					}
				}

				break;

			case EnChargingEventMenu.ConsumptionEvent:
				
				if (csConsumeEvent != null)
				{
					CsConsumeEventMission csChargeEventMission = csConsumeEvent.GetConsumeEventMission(nMissionNo);

					if (csChargeEventMission != null)
					{
						// 인벤토리 체크
						var rewards = csChargeEventMission.ConsumeEventMissionRewardList.Select(reward => reward.ItemReward);

						if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
						{
							CsCashManager.Instance.SendConsumeEventMissionRewardReceive(csConsumeEvent.EventId, nMissionNo);
						}
						else
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
						}
					}
				}

				break;

			case EnChargingEventMenu.DailyConsumptionEvent:

				CsDailyConsumeEventMission csDailyConsumeEventMission = CsGameData.Instance.DailyConsumeEvent.GetDailyConsumeEventMission(nMissionNo);

				if (csDailyConsumeEventMission != null)
				{
					// 인벤토리 체크
					var rewards = csDailyConsumeEventMission.DailyConsumeEventMissionRewardList.Select(reward => reward.ItemReward);

					if (CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(rewards))
					{
						CsCashManager.Instance.SendDailyConsumeEventMissionRewardReceive(nMissionNo);
					}
					else
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A125_TXT_02001"));
					}
				}
				
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		UpdateDisplayToggles();
		UpdateEventDisplay();

		Transform trSelectedToggle = m_trImageBackground.Find("FrameToggles/" + m_enChargingEventMenu.ToString());

		if (trSelectedToggle != null && !trSelectedToggle.gameObject.activeSelf)
		{
			SelectFirstToggle();
		}
	}

	// 첫충전
	//---------------------------------------------------------------------------------------------------
	void OnEventFirstChargeEventObjectiveCompleted()
	{
		UpdateDisplayToggles();
		UpdateChargingDisplay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFirstChargeEventRewardReceive()
	{
		UpdateDisplayToggles();
		SelectFirstToggle();
	}

	// 재충전
	//---------------------------------------------------------------------------------------------------
	void OnEventRechargeEventProgress()
	{
		UpdateDisplayToggles();
		UpdateChargingDisplay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRechargeEventRewardReceive()
	{
		UpdateDisplayToggles();
		SelectFirstToggle();
	}
	
	// 누적충전
	//---------------------------------------------------------------------------------------------------
	void OnEventChargeEventProgress()
	{
		UpdateDisplayToggles();
		UpdateEventDisplay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventChargeEventMissionRewardReceive(int nMissionNo)
	{
		UpdateDisplayToggles();
		UpdateEventMissionButton(nMissionNo);
	}
	
	// 일일충전
	//---------------------------------------------------------------------------------------------------
	void OnEventDailyChargeEventProgress()
	{
		UpdateDisplayToggles();
		UpdateEventDisplay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDailyChargeEventMissionRewardReceive(int nMissionNo)
	{
		UpdateDisplayToggles();
		UpdateEventMissionButton(nMissionNo);
	}
	
	// 누적소비
	//---------------------------------------------------------------------------------------------------
	void OnEventConsumeEventProgress()
	{
		UpdateDisplayToggles();
		UpdateEventDisplay();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConsumeEventMissionRewardReceive(int nMissionNo)
	{
		UpdateDisplayToggles();
		UpdateEventMissionButton(nMissionNo);
	}
	
	// 일일소비
	//---------------------------------------------------------------------------------------------------
	void OnEventDailyConsumeEventProgress()
	{
		UpdateDisplayToggles();
		UpdateEventDisplay();
	}
	
	//---------------------------------------------------------------------------------------------------
	void OnEventDailyConsumeEventMissionRewardReceive(int nMissionNo)
	{
		UpdateDisplayToggles();
		UpdateEventMissionButton(nMissionNo);
	}
	
	//---------------------------------------------------------------------------------------------------

	#endregion Event
}
