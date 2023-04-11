using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-07-06)
//---------------------------------------------------------------------------------------------------

public class CsPopupRookieGift : CsUpdateableMonoBehaviour
{
	GameObject m_goPopupItemInfo;

	Transform m_trPopupList;
	Transform m_trItemInfo;
	Transform m_trTextScratch;
	Transform m_trFrameItemSlot;
	
	Button m_buttonReceive;

	CsPopupItemInfo m_csPopupItemInfo;
	CsRookieGift m_csRookieGift;
	CsScratchImage m_csScratchImage;

	Text m_textRemainingTime;

	Coroutine m_coroutineScratch = null;

	float m_flTime = 0.0f;

	bool ReceivableRookieGift
	{
		get
		{
			return 0 < CsGameData.Instance.MyHeroInfo.RookieGiftNo &&
				CsGameData.Instance.MyHeroInfo.RookieGiftNo <= CsGameData.Instance.RookieGiftList.Max(gift => gift.GiftNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		if (!ReceivableRookieGift)
		{
			PopupClose();
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		CsGameEventUIToUI.Instance.EventRookieGiftReceive += OnEventRookieGiftReceive;

		CsScratchImage csCsratchImage = transform.Find("ImageBackground/ImageFrameBackground/ImageScratch").GetComponent<CsScratchImage>();
		csCsratchImage.EventScratchFinish += FinishScratch;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
		CsGameEventUIToUI.Instance.EventRookieGiftReceive -= OnEventRookieGiftReceive;

		CsScratchImage csCsratchImage = transform.Find("ImageBackground/ImageFrameBackground/ImageScratch").GetComponent<CsScratchImage>();
		csCsratchImage.EventScratchFinish -= FinishScratch;
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
			UpdateRemainingTime();

			m_flTime = Time.time;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;

		Transform trImageBackground = transform.Find("ImageBackground");

		Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = CsConfiguration.Instance.GetString("A100_NAME_00001");

		Transform trButtonClose = trImageBackground.Find("ButtonClose");
		Button buttonClose = trButtonClose.GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(OnClickPopupClose);

		Transform trImageFrameBackground = trImageBackground.Find("ImageFrameBackground");
		
		m_trTextScratch = trImageFrameBackground.Find("TextScratch");
		Text textScratch = m_trTextScratch.GetComponent<Text>();
		CsUIData.Instance.SetFont(textScratch);
		textScratch.text = CsConfiguration.Instance.GetString("A100_TXT_00001");

		m_textRemainingTime = trImageFrameBackground.Find("TextRemainingTime").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textRemainingTime);

		m_trFrameItemSlot = trImageFrameBackground.Find("FrameItemSlot");

		//Text textTip = trImageFrameBackground.Find("ImageScratch/TextTip").GetComponent<Text>();
		//CsUIData.Instance.SetFont(textTip);
		//textTip.text = CsConfiguration.Instance.GetString("A100_TXT_00001");

		m_buttonReceive = trImageFrameBackground.Find("ButtonReceive").GetComponent<Button>();
		m_buttonReceive.onClick.RemoveAllListeners();
		m_buttonReceive.onClick.AddListener(OnClickRookieGiftReceive);
		m_buttonReceive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textReceive = trImageFrameBackground.Find("ButtonReceive/TextReceive").GetComponent<Text>();
		CsUIData.Instance.SetFont(textReceive);
		textReceive.text = CsConfiguration.Instance.GetString("A100_BTN_00001");

		m_csScratchImage = trImageFrameBackground.Find("ImageScratch").GetComponent<CsScratchImage>();

		UpdateContent();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPopupClose()
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickRookieGiftReceive()
	{
		var csRookieGift = CsGameData.Instance.GetRookieGift(CsGameData.Instance.MyHeroInfo.RookieGiftNo);

		if (!CsGameData.Instance.MyHeroInfo.CheckAddItemAvailable(csRookieGift.RookieGiftRewardList.Select(reward => reward.ItemReward)))
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A100_TXT_03001"));
			return;
		}

		CsCommandEventManager.Instance.SendRookieGiftReceive();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickItemInfo(CsItem csItem)
	{
		StartCoroutine(LoadPopupItemInfo(csItem));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRookieGiftReceive()
	{
		if (!ReceivableRookieGift)
		{
			PopupClose();
			return;
		}

		UpdateContent();
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupItemInfo(CsItem csitem)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
		yield return resourceRequest;
		m_goPopupItemInfo = (GameObject)resourceRequest.asset;

		OpenPopupItemInfo(csitem);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo(CsItem csitem)
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

		m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, false, 0, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
	{
		m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
		Destroy(m_trItemInfo.gameObject);
		m_csPopupItemInfo = null;
		m_trItemInfo = null;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateContent()
	{
		m_coroutineScratch = null;
		m_csScratchImage.Scratchable = false;
		m_csRookieGift = CsGameData.Instance.GetRookieGift(CsGameData.Instance.MyHeroInfo.RookieGiftNo);

		Transform trImageFrameBackground = transform.Find("ImageBackground/ImageFrameBackground");
		Transform trFrameItemSlot = trImageFrameBackground.Find("FrameItemSlot");

		if (m_trFrameItemSlot != null)
		{
			m_trFrameItemSlot.gameObject.SetActive(false);
		}

		trImageFrameBackground.Find("ImageScratch").gameObject.SetActive(true);
		m_buttonReceive.gameObject.SetActive(false);
		m_buttonReceive.interactable = false;

		for (int i = 0; i < trFrameItemSlot.childCount; i++)
		{
			trFrameItemSlot.GetChild(i).gameObject.SetActive(false);
		}

		int nSlotCount = 0;
		foreach (var reward in m_csRookieGift.RookieGiftRewardList.OrderBy(reward => reward.RewardNo))
		{
			Transform trItemSlot = trFrameItemSlot.GetChild(nSlotCount);

			CsUIData.Instance.DisplayItemSlot(trItemSlot, reward.ItemReward.Item, reward.ItemReward.ItemOwned, reward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);

			Button buttonItemSlot = trItemSlot.GetComponent<Button>();
			buttonItemSlot.onClick.RemoveAllListeners();
			buttonItemSlot.onClick.AddListener(() => OnClickItemInfo(reward.ItemReward.Item));
			buttonItemSlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			trItemSlot.gameObject.SetActive(true);

			nSlotCount++;
		}

		UpdateRemainingTime();
	}

	//---------------------------------------------------------------------------------------------------
	void FinishScratch()
	{
		Transform trImageFrameBackground = transform.Find("ImageBackground/ImageFrameBackground");

		trImageFrameBackground.Find("ImageScratch").gameObject.SetActive(false);

		m_buttonReceive.interactable = true;
		m_buttonReceive.gameObject.SetActive(true);

		if (m_coroutineScratch != null)
		{
			StopCoroutine(m_coroutineScratch);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateRemainingTime()
	{
		int nRemainingTime = (int)CsGameData.Instance.MyHeroInfo.RookieGiftRemainingTime;
		bool bWaiting = nRemainingTime > 0;
		
		if (!bWaiting && m_coroutineScratch == null)
		{
			m_coroutineScratch = StartCoroutine(m_csScratchImage.AutoScratch(CsGameConfig.Instance.RookieGiftScratchOpenDuration));
		}
		
		if (m_textRemainingTime != null)
		{
			m_textRemainingTime.gameObject.SetActive(bWaiting);

			if (bWaiting)
			{
				TimeSpan timeSpan = TimeSpan.FromSeconds(nRemainingTime);
				m_textRemainingTime.text = string.Format(CsConfiguration.Instance.GetString("A100_TXT_01002"), timeSpan.Hours.ToString("00"), timeSpan.Minutes.ToString("00"), timeSpan.Seconds.ToString("00"));
			}
		}

		if (m_trTextScratch != null)
		{
			m_trTextScratch.gameObject.SetActive(!bWaiting);
		}

		if (m_trFrameItemSlot != null)
		{
			m_trFrameItemSlot.gameObject.SetActive(!bWaiting);
		}

		m_csScratchImage.Scratchable = !bWaiting;
	}
}
