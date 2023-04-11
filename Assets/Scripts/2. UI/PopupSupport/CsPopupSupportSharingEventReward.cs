using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-11)
//---------------------------------------------------------------------------------------------------

public class CsPopupSupportSharingEventReward : CsPopupSub
{
	[SerializeField]
	GameObject m_goItemSlot;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		UpdateEvent();
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Transform trImageBackground = transform.Find("ImageBackground");

		Transform trImageFrameSharingReward = trImageBackground.Find("FrameReward/ImageFrameSharingReward");

		CsUIData.Instance.SetText(trImageFrameSharingReward.Find("TextTitle"), "A166_TXT_00001", true);
		
		CsUIData.Instance.SetText(transform.Find("TextDescriptionBottom"), "A166_TXT_00005", true);
		CsUIData.Instance.SetButton(transform.Find("ButtonSharing"), OnClickButtonSharing);
		CsUIData.Instance.SetText(transform.Find("ButtonSharing/TextSharing"), "A166_BTN_00002", true);

		Transform trImageFrameAcceptanceReward = trImageBackground.Find("FrameReward/ImageFrameAcceptanceReward");

		CsUIData.Instance.SetText(trImageFrameAcceptanceReward.Find("TextTitle"), "A166_TXT_00003", true);
		CsUIData.Instance.SetText(trImageFrameAcceptanceReward.Find("TextDescription"), "A166_TXT_00004", true);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateEvent()
	{
		Transform trImageBackground = transform.Find("ImageBackground");

		CsSharingEvent csSharingEvent = CsGameData.Instance.GetCurrentSharingEvent();

		if (csSharingEvent != null)
		{
			Transform trImageFrameSharingReward = trImageBackground.Find("FrameReward/ImageFrameSharingReward");

			trImageFrameSharingReward.gameObject.SetActive(csSharingEvent.RewardRange == 1 || csSharingEvent.RewardRange == 2);

			if (csSharingEvent.RewardRange == 1 ||
				csSharingEvent.RewardRange == 2)
			{
				CsUIData.Instance.SetText(trImageBackground.Find("TextContent"), csSharingEvent.Description1, false);
				CsUIData.Instance.SetText(trImageBackground.Find("FrameDescription/TextDescription"), csSharingEvent.Description2, false);

				CsUIData.Instance.SetText(trImageFrameSharingReward.Find("TextDescription"), string.Format(CsConfiguration.Instance.GetString("A166_TXT_00002"), 0), false);

				Transform trSharingRewardFrameLayout = trImageFrameSharingReward.Find("FrameLayout");

				for (int i = 0; i < trSharingRewardFrameLayout.childCount; i++)
				{
					trSharingRewardFrameLayout.GetChild(i).gameObject.SetActive(false);
				}

				int nChlidIndex = 0;

				foreach (CsSharingEventSenderReward csSharingEventSenderReward in csSharingEvent.SharingEventSenderRewardList)
				{
					Transform trSlot = null;
					
					if (nChlidIndex < trSharingRewardFrameLayout.childCount)
					{
						trSlot = trSharingRewardFrameLayout.GetChild(nChlidIndex);
						trSlot.gameObject.SetActive(true);
					}
					else
					{
						trSlot = Instantiate(m_goItemSlot, trSharingRewardFrameLayout).transform;
					}

					CsUIData.Instance.DisplayItemSlot(trSlot, csSharingEventSenderReward.Item, csSharingEventSenderReward.ItemOwned, csSharingEventSenderReward.ItemCount, false, EnItemSlotSize.Medium, false);
					CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(csSharingEventSenderReward.Item));

					nChlidIndex++;
				}
			}

			Transform trImageFrameAcceptanceReward = trImageBackground.Find("FrameReward/ImageFrameAcceptanceReward");

			trImageFrameAcceptanceReward.gameObject.SetActive(csSharingEvent.RewardRange == 1 || csSharingEvent.RewardRange == 3);

			if (csSharingEvent.RewardRange == 1 ||
				csSharingEvent.RewardRange == 3)
			{
				Transform trAcceptanceRewardFrameLayout = trImageFrameAcceptanceReward.Find("FrameLayout");

				for (int i = 0; i < trAcceptanceRewardFrameLayout.childCount; i++)
				{
					trAcceptanceRewardFrameLayout.GetChild(i).gameObject.SetActive(false);
				}

				int nChlidIndex = 0;

				foreach (CsSharingEventReceiverReward csSharingEventReceiverReward in csSharingEvent.SharingEventReceiverRewardList)
				{
					Transform trSlot = null;

					if (nChlidIndex < trAcceptanceRewardFrameLayout.childCount)
					{
						trSlot = trAcceptanceRewardFrameLayout.GetChild(nChlidIndex);
						trSlot.gameObject.SetActive(true);
					}
					else
					{
						trSlot = Instantiate(m_goItemSlot, trAcceptanceRewardFrameLayout).transform;
					}

					CsUIData.Instance.DisplayItemSlot(trSlot, csSharingEventReceiverReward.Item, csSharingEventReceiverReward.ItemOwned, csSharingEventReceiverReward.ItemCount, false, EnItemSlotSize.Medium, false);
					CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(csSharingEventReceiverReward.Item));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSharing()
	{
		CsSharingEventManager.Instance.RequestFirebaseDynamicLink();
	}

	//---------------------------------------------------------------------------------------------------
}
