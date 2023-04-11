using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CsPopupOrdealQuest : CsUpdateableMonoBehaviour
{
	Transform m_trContent;

	float m_flTime = 0.0f;
	
	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsOrdealQuestManager.Instance.EventOrdealQuestComplete += OnEventOrdealQuestComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotComplete += OnEventOrdealQuestSlotComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotProgressCountsUpdated += OnEventOrdealQuestSlotProgressCountsUpdated;
		
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsOrdealQuestManager.Instance.EventOrdealQuestComplete -= OnEventOrdealQuestComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotComplete -= OnEventOrdealQuestSlotComplete;
		CsOrdealQuestManager.Instance.EventOrdealQuestSlotProgressCountsUpdated -= OnEventOrdealQuestSlotProgressCountsUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PopupClose();
		}

		if (m_flTime + flTime < Time.time)
		{
			foreach (var csHeroOrdealQuestSlot in CsOrdealQuestManager.Instance.CsHeroOrdealQuest.HeroOrdealQuestSlotList)
			{
				UpdateRemainingTime(csHeroOrdealQuestSlot);
				UpdateButtonSlotReceive(csHeroOrdealQuestSlot);
			}

			m_flTime = Time.time;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPopupClose()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonReceive()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
		CsOrdealQuestManager.Instance.SendOrdealQuestComplete();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonMissionReceive(int nIndex)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
		CsOrdealQuestManager.Instance.SendOrdealQuestSlotComplete(nIndex);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonMissionStart(CsOrdealQuestMission csOrdealQuestMission)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsGameEventUIToUI.Instance.OnEventStartOrdealQuestMission(csOrdealQuestMission);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOrdealQuestComplete(bool bLevelUp, long lAcquiredExp)
	{
		if (CsOrdealQuestManager.Instance.CsHeroOrdealQuest == null ||
			CsOrdealQuestManager.Instance.CsHeroOrdealQuest.Completed)
		{
			PopupClose();
			return;
		}

		UpdateOrdealQuestContent();
		UpdateButtonReceive();
		UpdateMissionSlotAll();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOrdealQuestSlotComplete(bool bLevelUp, long lAcquiredExp, int nIndex)
	{
		UpdateMissionSlot(CsOrdealQuestManager.Instance.CsHeroOrdealQuest.GetHeroOrdealQuestSlot(nIndex));

		UpdateButtonReceive();
	}
		
	//---------------------------------------------------------------------------------------------------
	void OnEventOrdealQuestSlotProgressCountsUpdated(int nIndex)
	{
		CsHeroOrdealQuestSlot csHeroOrdealQuestSlot = CsOrdealQuestManager.Instance.CsHeroOrdealQuest.GetHeroOrdealQuestSlot(nIndex);

		if (csHeroOrdealQuestSlot != null)
		{
			UpdateProgressBar(csHeroOrdealQuestSlot);
			UpdateButtonSlotReceive(csHeroOrdealQuestSlot);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Transform trImageBackground = transform.Find("ImageBackground");
		m_trContent = trImageBackground.Find("Content");

		Text textTitle = trImageBackground.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A120_TITLE_00001");

		Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClickPopupClose);

		UpdateOrdealQuestContent();

		Button buttonReceive = trImageBackground.Find("ButtonReceive").GetComponent<Button>();
		buttonReceive.onClick.RemoveAllListeners();
		buttonReceive.onClick.AddListener(OnClickButtonReceive);

		Text textReceive = trImageBackground.Find("ButtonReceive/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textReceive);
		textReceive.text = CsConfiguration.Instance.GetString("A120_BTN_00002");

		UpdateButtonReceive();

		UpdateMissionSlotAll();
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateOrdealQuestContent()
	{
		Transform trImageBackground = transform.Find("ImageBackground");

		Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = string.Format(CsOrdealQuestManager.Instance.CsHeroOrdealQuest.OrdealQuest.Name, CsOrdealQuestManager.Instance.CsHeroOrdealQuest.OrdealQuest.RequiredHeroLevel);

		Text textDescription = trImageBackground.Find("TextDescription").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDescription);
		textDescription.text = CsOrdealQuestManager.Instance.CsHeroOrdealQuest.OrdealQuest.Description;

		Transform trItemSlot = trImageBackground.Find("ItemSlot");
		CsUIData.Instance.DisplayItemSlot(trItemSlot, CsOrdealQuestManager.Instance.CsHeroOrdealQuest.OrdealQuest.AvailableRewardItem, false, 0, false, EnItemSlotSize.Medium, false);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButtonReceive()
	{
		Button button = transform.Find("ImageBackground/ButtonReceive").GetComponent<Button>();
		button.interactable = CsOrdealQuestManager.Instance.RewardReceivable;

		Text text = button.transform.Find("Text").GetComponent<Text>();

		if (CsOrdealQuestManager.Instance.RewardReceivable)
		{
			text.color = new Color32(255, 255, 255, 255);
		}
		else
		{
			text.color = new Color32(108, 113, 117, 255);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateMissionSlotAll()
	{
		foreach (var csHeroOrdealQuestSlot in CsOrdealQuestManager.Instance.CsHeroOrdealQuest.HeroOrdealQuestSlotList)
		{
			UpdateMissionSlot(csHeroOrdealQuestSlot);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateMissionSlot(CsHeroOrdealQuestSlot csHeroOrdealQuestSlot)
	{
		Transform trSlot = transform.Find("ImageBackground/Content/Slot" + csHeroOrdealQuestSlot.Index.ToString());
		Transform trFrameNonCompleted = trSlot.Find("FrameNonCompleted");
		Transform trFrameCompleted = trSlot.Find("FrameCompleted");

		if (trSlot != null)
		{
			trFrameNonCompleted.gameObject.SetActive(!csHeroOrdealQuestSlot.IsCompleted);
			trFrameCompleted.gameObject.SetActive(csHeroOrdealQuestSlot.IsCompleted);
			
			if (csHeroOrdealQuestSlot.IsCompleted)
			{
				Text textReceived = trFrameCompleted.Find("TextReceived").GetComponent<Text>();
				CsUIData.Instance.SetFont(textReceived);
				textReceived.text = CsConfiguration.Instance.GetString("A120_TXT_00002");
			}
			else
			{
				Transform trItemSlot = trFrameNonCompleted.Find("ItemSlot64");
				CsUIData.Instance.DisplaySmallItemSlot(trItemSlot, csHeroOrdealQuestSlot.OrdealQuestMission.AvailableRewardItem, false, 0);

				Transform trContent = trFrameNonCompleted.Find("Content");
				Transform trTextTitle = trContent.Find("TextTitle");
				
				Text textTitle = trTextTitle.GetComponent<Text>();
				CsUIData.Instance.SetFont(textTitle);

				if (csHeroOrdealQuestSlot.OrdealQuestMission.Type == 18)
				{
					string strRankName = null;
					
					CsRank csRank = CsGameData.Instance.GetRank(csHeroOrdealQuestSlot.OrdealQuestMission.TargetCount);

					if (csRank != null)
						strRankName = csRank.Name;

					textTitle.text = string.Format(csHeroOrdealQuestSlot.OrdealQuestMission.Name, strRankName);
				}
				else
				{
					textTitle.text = string.Format(csHeroOrdealQuestSlot.OrdealQuestMission.Name, csHeroOrdealQuestSlot.OrdealQuestMission.TargetCount);
				}

				Text textButtonReceive = trFrameNonCompleted.Find("ButtonReceive/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textButtonReceive);
				textButtonReceive.text = CsConfiguration.Instance.GetString("A120_BTN_00002");

				Text textButtonStart = trFrameNonCompleted.Find("ButtonStart/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textButtonStart);
				textButtonStart.text = CsConfiguration.Instance.GetString("A120_BTN_00003");

				UpdateRemainingTime(csHeroOrdealQuestSlot);
				UpdateProgressBar(csHeroOrdealQuestSlot);
				UpdateButtonSlotReceive(csHeroOrdealQuestSlot);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateProgressBar(CsHeroOrdealQuestSlot csHeroOrdealQuestSlot)
	{
		if (m_trContent != null &&
			!csHeroOrdealQuestSlot.IsCompleted)
		{
			Transform trSlot = m_trContent.Find("Slot" + csHeroOrdealQuestSlot.Index.ToString());

			Slider slider = trSlot.Find("FrameNonCompleted/Content/Progress/ProgressBar").GetComponent<Slider>();
			slider.minValue = 0;
			slider.maxValue = csHeroOrdealQuestSlot.OrdealQuestMission.TargetCount;
			slider.value = csHeroOrdealQuestSlot.ProgressCount;

			Text text = trSlot.Find("FrameNonCompleted/Content/Progress/Text").GetComponent<Text>();
			CsUIData.Instance.SetFont(text);
			text.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csHeroOrdealQuestSlot.ProgressCount, csHeroOrdealQuestSlot.OrdealQuestMission.TargetCount);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateRemainingTime(CsHeroOrdealQuestSlot csHeroOrdealQuestSlot)
	{
		if (m_trContent != null &&
			!csHeroOrdealQuestSlot.IsCompleted)
		{
			Transform trSlot = m_trContent.Find("Slot" + csHeroOrdealQuestSlot.Index.ToString());

			Transform trContent = trSlot.Find("FrameNonCompleted/Content");

			Transform trRemainingTime = trContent.Find("RemainingTime");
			trRemainingTime.gameObject.SetActive(csHeroOrdealQuestSlot.OrdealQuestMission.AutoCompletable);

			if (csHeroOrdealQuestSlot.OrdealQuestMission.AutoCompletable)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(csHeroOrdealQuestSlot.RemainingTime);

				Text text = trRemainingTime.Find("Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(text);
				text.text = string.Format(CsConfiguration.Instance.GetString("A120_TXT_00001"),
					timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));

				if (csHeroOrdealQuestSlot.RemainingTime <= 0)
				{
					CsOrdealQuestManager.Instance.SendOrdealQuestSlotComplete(csHeroOrdealQuestSlot.Index);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButtonSlotReceive(CsHeroOrdealQuestSlot csHeroOrdealQuestSlot)
	{
		if (m_trContent != null &&
			!csHeroOrdealQuestSlot.IsCompleted)
		{
			Transform trSlot = m_trContent.Find("Slot" + csHeroOrdealQuestSlot.Index.ToString());
			Transform trFrameNonCompleted = trSlot.Find("FrameNonCompleted");

			// 보상 수령 가능한 경우 표시
			Transform trImageReceivable = trFrameNonCompleted.Find("ImageReceivable");
			trImageReceivable.gameObject.SetActive(csHeroOrdealQuestSlot.Receivable);

			Transform trButtonReceive = trFrameNonCompleted.Find("ButtonReceive");
			trButtonReceive.gameObject.SetActive(csHeroOrdealQuestSlot.Receivable);

			Button buttonReceive = trButtonReceive.GetComponent<Button>();
			buttonReceive.onClick.RemoveAllListeners();
			buttonReceive.onClick.AddListener(() => OnClickButtonMissionReceive(csHeroOrdealQuestSlot.Index));

			// 보상 수령 불가능한 경우 표시
			Transform trButtonStart = trFrameNonCompleted.Find("ButtonStart");
			trButtonStart.gameObject.SetActive(!csHeroOrdealQuestSlot.Receivable);

			Button buttonStart = trButtonStart.GetComponent<Button>();
			buttonStart.onClick.RemoveAllListeners();
			buttonStart.onClick.AddListener(() => OnClickButtonMissionStart(csHeroOrdealQuestSlot.OrdealQuestMission));
		}
	}
}
