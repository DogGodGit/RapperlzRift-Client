using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-08-08)
//---------------------------------------------------------------------------------------------------

public class CsPopupHalidomCollection : CsUpdateableMonoBehaviour
{
	Transform m_trImageBackground;
	Transform m_trPopupReward;
	Transform m_trImageFrameAchievement;

	Slider m_slider;
	Text m_textValueAchievement;

	int m_nMaxCollectionCount;
	int m_nFearAltarHalidomElementalId = 0;
	int m_nFearAltarHalidomCollectionNo = 0;

	float m_flSliderSizeX = 0f;
	float m_flSliderStartX = 0f;

	bool m_bRewardReceivable;
	
	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;

		CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomElementalRewardReceive += OnEventFearAltarHalidomElementalRewardReceive;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomCollectionRewardReceive += OnEventFearAltarHalidomCollectionRewardReceive;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomAcquisition += OnEventFearAltarHalidomAcquisition;

		CsDungeonManager.Instance.EventFearAltarExit += OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon += OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished += OnEventFearAltarBanished;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;

		CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomElementalRewardReceive -= OnEventFearAltarHalidomElementalRewardReceive;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomCollectionRewardReceive -= OnEventFearAltarHalidomCollectionRewardReceive;
		CsGameEventUIToUI.Instance.EventFearAltarHalidomAcquisition -= OnEventFearAltarHalidomAcquisition;

		CsDungeonManager.Instance.EventFearAltarExit -= OnEventFearAltarExit;
		CsDungeonManager.Instance.EventFearAltarAbandon -= OnEventFearAltarAbandon;
		CsDungeonManager.Instance.EventFearAltarBanished -= OnEventFearAltarBanished;
	}

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime)
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PopupClose();
		}

	}

	//---------------------------------------------------------------------------------------------------
	public void InitializeUI(bool bRewardReceivable)
	{
		// 던전 안에서 창을 열었을 경우 보상 수령 불가능
		m_bRewardReceivable = bRewardReceivable;

		m_trImageBackground = transform.Find("ImageBackground");

		Button buttonClose = m_trImageBackground.Find("ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(PopupClose);
		buttonClose.onClick.AddListener(PlayButtonSound);

		Text textTitle = m_trImageBackground.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A116_TXT_00003");

		Transform trFrameContent = m_trImageBackground.Find("FrameContent");

		GameObject goItemHalidomCollection = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/ItemHalidomElemental");
		GameObject goItemImageHalidom = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/ItemImageHalidom");
		GameObject goItemImageLine = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/ImageLine");

		int nElementalItemCount = 0;

		// 원소
		foreach (var csFearAltarHalidomElemental in CsGameData.Instance.FearAltarHalidomElementalList)
		{
			Transform trItem = Instantiate(goItemHalidomCollection, trFrameContent).transform;
			trItem.name = csFearAltarHalidomElemental.HalidomElementalId.ToString();

			Image imageIcon = trItem.Find("ImageIcon").GetComponent<Image>();
			imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupHalidomCollection/ico_skill_property" + csFearAltarHalidomElemental.HalidomElementalId.ToString("00"));

			Text textName = trItem.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = csFearAltarHalidomElemental.Name;

			Transform trHalidomContent = trItem.Find("FrameContent");

			// 아이콘
			var enumerable = from halidom in CsGameData.Instance.FearAltar.FearAltarHalidomList
							 where halidom.FearAltarHalidomElemental.HalidomElementalId == csFearAltarHalidomElemental.HalidomElementalId
							 orderby halidom.HalidomId
							 select halidom;

			foreach (var fearAltarHalidom in enumerable)
			{
				Transform trImageHalidom = Instantiate(goItemImageHalidom, trHalidomContent).transform;
				trImageHalidom.name = fearAltarHalidom.HalidomId.ToString();

				Image imageHalidom = trImageHalidom.GetComponent<Image>();
				imageHalidom.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupHalidomCollection/" + fearAltarHalidom.ImageName);
			}

			Button buttonReward = trItem.Find("ButtonReceive").GetComponent<Button>();
			buttonReward.onClick.RemoveAllListeners();
			buttonReward.onClick.AddListener(() => OnClickReceiveFearAltarHalidomElementalReward(csFearAltarHalidomElemental.HalidomElementalId));
			
			Image imageItemFrame = trItem.Find("ButtonReceive/ImageFrame").GetComponent<Image>();
			imageItemFrame.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csFearAltarHalidomElemental.CollectionItemReward.Item.Grade.ToString("00")); 
			
			Image imageItemIcon = trItem.Find("ButtonReceive/ImageIcon").GetComponent<Image>();
			imageItemFrame.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csFearAltarHalidomElemental.CollectionItemReward.Item.Image);

			UpdateHalidomElemantalItem(csFearAltarHalidomElemental);

			// 구분선 삽입
			if (++nElementalItemCount < CsGameData.Instance.FearAltarHalidomElementalList.Count)
			{
				Instantiate(goItemImageLine, trFrameContent);
			}
		}

		// 달성도
		m_trImageFrameAchievement = m_trImageBackground.Find("ImageFrameAchievement");
		
		Text textAchievement = m_trImageFrameAchievement.Find("TextAchievement").GetComponent<Text>();
		CsUIData.Instance.SetFont(textAchievement);
		textAchievement.text = CsConfiguration.Instance.GetString("A50_TXT_00002");

		m_textValueAchievement = m_trImageFrameAchievement.Find("TextValueAchievement").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textValueAchievement);

		m_nMaxCollectionCount = CsGameData.Instance.FearAltar.FearAltarHalidomCollectionRewardList.Max(reward => reward.CollectionCount);

		Transform trSlider = m_trImageFrameAchievement.Find("Slider");
		m_trPopupReward = m_trImageFrameAchievement.Find("PopupReward");

		RectTransform rtrSlider = trSlider.GetComponent<RectTransform>();
		m_flSliderSizeX = rtrSlider.sizeDelta.x;
		m_flSliderStartX = rtrSlider.anchoredPosition.x - m_flSliderSizeX * 0.5f;

		m_slider = trSlider.GetComponent<Slider>();
		m_slider.minValue = 0;
		m_slider.maxValue = m_nMaxCollectionCount;

		GameObject goItemHalidomCollectionReward = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupHalidomCollection/ItemHalidomCollectionReward");

		foreach (var csFearAltarHalidomCollectionReward in CsGameData.Instance.FearAltar.FearAltarHalidomCollectionRewardList)
		{
			Transform trItemReward = Instantiate(goItemHalidomCollectionReward, m_trImageFrameAchievement).transform;
			trItemReward.name = csFearAltarHalidomCollectionReward.RewardNo.ToString();

			RectTransform rtrItemReward = trItemReward.GetComponent<RectTransform>();
			rtrItemReward.anchoredPosition = new Vector2(m_flSliderStartX + m_flSliderSizeX * csFearAltarHalidomCollectionReward.CollectionCount / m_nMaxCollectionCount, 8f);

			Text textCount = trItemReward.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCount);
			textCount.text = csFearAltarHalidomCollectionReward.CollectionCount.ToString();
		}

		UpdateAchievement();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPopupClose()
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickReceiveFearAltarHalidomElementalReward(int nFearAltarHalidomElementalId)
	{
		// 던전 안에서 창을 열었을 경우 보상 수령 불가능
		if (!m_bRewardReceivable)
			return;

		m_nFearAltarHalidomElementalId = nFearAltarHalidomElementalId;

		CsCommandEventManager.Instance.SendFearAltarHalidomElementalRewardReceive(nFearAltarHalidomElementalId);
		PlayButtonSound();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPopupReward()
	{
		PlayButtonSound();
		m_trPopupReward.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonUnReceivable(CsFearAltarHalidomCollectionReward csFearAltarHalidomCollectionReward)
	{
		PlayButtonSound();

		if (m_nFearAltarHalidomCollectionNo == csFearAltarHalidomCollectionReward.RewardNo &&
			m_trPopupReward.gameObject.activeSelf)
		{
			m_trPopupReward.gameObject.SetActive(false);
			return;
		}

		m_nFearAltarHalidomCollectionNo = csFearAltarHalidomCollectionReward.RewardNo;

		Button buttonPopup = m_trPopupReward.GetComponent<Button>();
		buttonPopup.onClick.RemoveAllListeners();
		buttonPopup.onClick.AddListener(OnClickButtonPopupReward);

		RectTransform rtrPopupReward = m_trPopupReward.GetComponent<RectTransform>();
		rtrPopupReward.anchoredPosition = new Vector2(m_flSliderStartX + m_flSliderSizeX * csFearAltarHalidomCollectionReward.CollectionCount / m_nMaxCollectionCount, 140f);

		Text textTitle = m_trPopupReward.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = csFearAltarHalidomCollectionReward.ItemReward.Item.Name;

		Transform trItemSlot = m_trPopupReward.Find("ItemSlot");
		CsUIData.Instance.DisplayItemSlot(trItemSlot, csFearAltarHalidomCollectionReward.ItemReward.Item, csFearAltarHalidomCollectionReward.ItemReward.ItemOwned, csFearAltarHalidomCollectionReward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);

		Text textDescription = m_trPopupReward.Find("TextDescription").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDescription);
		textDescription.text = csFearAltarHalidomCollectionReward.ItemReward.Item.Description;

		m_trPopupReward.gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonReceiveCollectionReward(int nFearAltarHalidomCollectionNo)
	{
		// 던전 안에서 창을 열었을 경우 보상 수령 불가능
		if (!m_bRewardReceivable)
			return;

		CsCommandEventManager.Instance.SendFearAltarHalidomCollectionRewardReceive(nFearAltarHalidomCollectionNo);
		PlayButtonSound();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDateChanged()
	{
		foreach (var csFearAltarHalidomElemental in CsGameData.Instance.FearAltarHalidomElementalList)
		{
			UpdateHalidomElemantalItem(csFearAltarHalidomElemental);
		}

		UpdateAchievement();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomElementalRewardReceive()
	{
		var csFearAltarHalidomElemental = CsGameData.Instance.FearAltarHalidomElementalList.Find(elemental => elemental.HalidomElementalId == m_nFearAltarHalidomElementalId);
		
		if (csFearAltarHalidomElemental != null)
		{
			UpdateHalidomElemantalItem(csFearAltarHalidomElemental);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomCollectionRewardReceive()
	{
		UpdateAchievement();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarHalidomAcquisition(int nHalidomId)
	{
		var csFearAltarHalidom = CsGameData.Instance.FearAltar.FearAltarHalidomList.Find(halidom => halidom.HalidomId == nHalidomId);

		if (csFearAltarHalidom != null)
		{
			UpdateHalidomElemantalItem(csFearAltarHalidom.FearAltarHalidomElemental);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarAbandon(int nPreviousContinentId)
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarBanished(int nPreviousContinentId)
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventFearAltarExit(int nPreviousContinentId)
	{
		PopupClose();
	}

	//---------------------------------------------------------------------------------------------------
	void PlayButtonSound()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	// 성물 수집 보상
	void UpdateAchievementReward(CsFearAltarHalidomCollectionReward csFearAltarHalidomCollectionReward)
	{
		Transform trItemReward = m_trImageFrameAchievement.Find(csFearAltarHalidomCollectionReward.RewardNo.ToString());
		
		if (trItemReward != null)
		{
			if (CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomList.Count < csFearAltarHalidomCollectionReward.CollectionCount)
			{
				// 수령 불가능
				trItemReward.Find("ButtonUnreceivable").gameObject.SetActive(true);
				trItemReward.Find("ButtonReceive").gameObject.SetActive(false);
				trItemReward.Find("ImageReceived").gameObject.SetActive(false);

				Button buttonunreceivable = trItemReward.Find("ButtonUnreceivable").GetComponent<Button>();
				buttonunreceivable.onClick.RemoveAllListeners();
				buttonunreceivable.onClick.AddListener(() => OnClickButtonUnReceivable(csFearAltarHalidomCollectionReward));
			}
			else
			{
				trItemReward.Find("ButtonUnreceivable").gameObject.SetActive(false);

				if (CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomCollectionRewardNo < csFearAltarHalidomCollectionReward.CollectionCount)
				{
					// 수령 가능
					trItemReward.Find("ButtonReceive").gameObject.SetActive(true);
					trItemReward.Find("ImageReceived").gameObject.SetActive(false);

					Button buttonReceive = trItemReward.Find("ButtonReceive").GetComponent<Button>();
					buttonReceive.onClick.RemoveAllListeners();
					buttonReceive.onClick.AddListener(() => OnClickButtonReceiveCollectionReward(csFearAltarHalidomCollectionReward.RewardNo));
				}
				else
				{
					// 이미 수령 완료
					trItemReward.Find("ButtonReceive").gameObject.SetActive(false);
					trItemReward.Find("ImageReceived").gameObject.SetActive(true);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 달성도 업데이트
	void UpdateAchievement()
	{
		m_textValueAchievement.text = CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomList.Count.ToString();

		m_slider.value = Math.Min(CsGameData.Instance.MyHeroInfo.WeeklyFearAltarHalidomList.Count, m_nMaxCollectionCount);
		
		foreach (var csFearAltarHalidomCollectionReward in CsGameData.Instance.FearAltar.FearAltarHalidomCollectionRewardList)
		{
			UpdateAchievementReward(csFearAltarHalidomCollectionReward);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 원소 업데이트
	void UpdateHalidomElemantalItem(CsFearAltarHalidomElemental csFearAltarHalidomElemental)
	{
		Transform trHalidomElementalItem = m_trImageBackground.Find("FrameContent/" + csFearAltarHalidomElemental.HalidomElementalId.ToString());
		Transform trHalidomContent = trHalidomElementalItem.Find("FrameContent");

		if (trHalidomElementalItem != null)
		{
			int nReceiveCount = 0;
			int nMaxCount = 0;

			var enumerable = from halidom in CsGameData.Instance.FearAltar.FearAltarHalidomList
							 where halidom.FearAltarHalidomElemental.HalidomElementalId == csFearAltarHalidomElemental.HalidomElementalId
							 orderby halidom.HalidomId
							 select halidom;

			foreach (var fearAltarHalidom in enumerable)
			{
				Transform trImageHalidom = trHalidomContent.Find(fearAltarHalidom.HalidomId.ToString());

				if (trImageHalidom != null)
				{
					Image imageHalidom = trImageHalidom.GetComponent<Image>();

					if (CsGameData.Instance.MyHeroInfo.HaveFearAltarHalidom(fearAltarHalidom.HalidomId))
					{
						imageHalidom.color = new Color(1f, 1f, 1f, 1f);
						nReceiveCount++;
					}
					else
					{
						// 해당 성물을 보유하지 않은 경우 투명하게 표시
						imageHalidom.color = new Color(1f, 1f, 1f, 0.3f);
					}
				}

				nMaxCount++;
			}

			bool bReceievable = nReceiveCount >= nMaxCount && !CsGameData.Instance.MyHeroInfo.RewardReceivedFearAltarHalidomElemental(csFearAltarHalidomElemental.HalidomElementalId);

			trHalidomElementalItem.Find("ButtonReceive").GetComponent<Button>().interactable = bReceievable;
			trHalidomElementalItem.Find("ButtonReceive/ImageDim").gameObject.SetActive(!bReceievable);
		}
	}
}
