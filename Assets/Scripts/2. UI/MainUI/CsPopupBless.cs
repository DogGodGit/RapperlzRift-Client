using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsPopupBless : MonoBehaviour
{
	[SerializeField]
	GameObject m_goItemSlot64;

	Transform m_trFrameRightBlessing;
	Transform m_trContentBlessing;
	Transform m_trFrameEmptyBlessing;

	Transform m_trFrameRightBlessingQuest;
	Transform m_trContentBlessingQuest;
	Transform m_trFrameEmptyBlessingQuest;

    Transform m_trFrameRightPresent;
    Transform m_trContentPresent;
    Transform m_trFrameEmptyPresent;

    Transform m_trPopupPresent;
    Transform m_trPopupList;

    bool m_bOpenPopupPresent = false;

    CsHeroReceivedPresent m_csHeroReceivedPresent = null;

    enum EnPopupBless
    {
        Blessing = 0, 
        BlessingQuest = 1, 
        Present = 2, 
    }

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;
		
		CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend += OnEventBlessingQuestBlessingSend;
		CsBlessingQuestManager.Instance.EventBlessingQuestDeleteAll += OnEventBlessingQuestDeleteAll;
		CsBlessingQuestManager.Instance.EventBlessingRewardReceive += OnEventBlessingRewardReceive;
		CsBlessingQuestManager.Instance.EventBlessingDeleteAll += OnEventBlessingDeleteAll;

        CsPresentManager.Instance.EventPresentReceived += OnEventPresentReceived;
        CsPresentManager.Instance.EventPresentReply += OnEventPresentReply;
        CsPresentManager.Instance.EventPresentSend += OnEventPresentSend;

		InitailizeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;

		CsBlessingQuestManager.Instance.EventBlessingQuestBlessingSend -= OnEventBlessingQuestBlessingSend;
		CsBlessingQuestManager.Instance.EventBlessingQuestDeleteAll -= OnEventBlessingQuestDeleteAll;
		CsBlessingQuestManager.Instance.EventBlessingRewardReceive -= OnEventBlessingRewardReceive;
		CsBlessingQuestManager.Instance.EventBlessingDeleteAll -= OnEventBlessingDeleteAll;

        CsPresentManager.Instance.EventPresentReceived -= OnEventPresentReceived;
        CsPresentManager.Instance.EventPresentReply -= OnEventPresentReply;
        CsPresentManager.Instance.EventPresentSend -= OnEventPresentSend;
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PopupClose();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitailizeUI()
	{
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

		Transform trImageBackground = transform.Find("ImageBackground");

		Text textTitle = trImageBackground.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A108_NAME_00007");

		Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(PopupClose);

		// 축복
		Toggle toggleBlessing = trImageBackground.Find("FrameLeft/ToggleBlessing").GetComponent<Toggle>();
		toggleBlessing.onValueChanged.RemoveAllListeners();
		toggleBlessing.onValueChanged.AddListener((isOn) => OnValueChangedToggle(isOn, EnPopupBless.Blessing));

		Text textToggleBlessing = toggleBlessing.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textToggleBlessing);
		textToggleBlessing.text = CsConfiguration.Instance.GetString("A108_BTN_00035");

		m_trFrameRightBlessing = trImageBackground.Find("FrameRightBlessing");
		m_trContentBlessing = m_trFrameRightBlessing.Find("Scroll View/Viewport/Content");
		m_trFrameEmptyBlessing = m_trFrameRightBlessing.Find("Scroll View/Viewport/FrameEmpty");

		Text textEmptyBlessing = m_trFrameEmptyBlessing.Find("TextEmpty").GetComponent<Text>();
		CsUIData.Instance.SetFont(textEmptyBlessing);
		textEmptyBlessing.text = CsConfiguration.Instance.GetString("A108_TXT_00023");

		Button buttonDeleteAllBlessings = m_trFrameRightBlessing.Find("ButtonDeleteAll").GetComponent<Button>();
        buttonDeleteAllBlessings.onClick.RemoveAllListeners();
		buttonDeleteAllBlessings.onClick.AddListener(OnClickButtonDeleteAllBlessings);

		Text textDeleteAllBlessings = buttonDeleteAllBlessings.transform.Find("TextDeleteAll").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDeleteAllBlessings);
		textDeleteAllBlessings.text = CsConfiguration.Instance.GetString("A108_BTN_00038");

		// 축복 퀘스트
		Toggle toggleBlessingQuest = trImageBackground.Find("FrameLeft/ToggleBlessingQuest").GetComponent<Toggle>();
		toggleBlessingQuest.onValueChanged.RemoveAllListeners();
		toggleBlessingQuest.onValueChanged.AddListener((isOn) => OnValueChangedToggle(isOn, EnPopupBless.BlessingQuest));

		Text textToggleBlessingQuest = toggleBlessingQuest.transform.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textToggleBlessingQuest);
		textToggleBlessingQuest.text = CsConfiguration.Instance.GetString("A108_BTN_00036");

		m_trFrameRightBlessingQuest = trImageBackground.Find("FrameRightBlessingQuest");
		m_trContentBlessingQuest = m_trFrameRightBlessingQuest.Find("Scroll View/Viewport/Content");
		m_trFrameEmptyBlessingQuest = m_trFrameRightBlessingQuest.Find("Scroll View/Viewport/FrameEmpty");

		Text textEmptyBlessingQuest = m_trFrameEmptyBlessingQuest.Find("TextEmpty").GetComponent<Text>();
		CsUIData.Instance.SetFont(textEmptyBlessingQuest);
		textEmptyBlessingQuest.text = CsConfiguration.Instance.GetString("A108_TXT_00023");

		Button buttonDeleteAllBlessingQuests = m_trFrameRightBlessingQuest.Find("ButtonDeleteAll").GetComponent<Button>();
        buttonDeleteAllBlessingQuests.onClick.RemoveAllListeners();
		buttonDeleteAllBlessingQuests.onClick.AddListener(OnClickButtonDeleteAllBlessingQuests);

		Text textDeleteAllBlessingQuests = buttonDeleteAllBlessingQuests.transform.Find("TextDeleteAll").GetComponent<Text>();
		CsUIData.Instance.SetFont(textDeleteAllBlessingQuests);
		textDeleteAllBlessingQuests.text = CsConfiguration.Instance.GetString("A108_BTN_00038");

        // 증정
        Toggle togglePresent = trImageBackground.Find("FrameLeft/TogglePresent").GetComponent<Toggle>();
        togglePresent.onValueChanged.RemoveAllListeners();
        togglePresent.onValueChanged.AddListener((isOn) => OnValueChangedToggle(isOn, EnPopupBless.Present));

        Text textTogglePresent = togglePresent.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTogglePresent);
        textTogglePresent.text = CsConfiguration.Instance.GetString("A108_BTN_00045");

        m_trFrameRightPresent = trImageBackground.Find("FrameRightPresent");
        m_trContentPresent = m_trFrameRightPresent.Find("Scroll View/Viewport/Content");
        m_trFrameEmptyPresent = m_trFrameRightPresent.Find("Scroll View/Viewport/FrameEmpty");

        Text textEmptyPresent = m_trFrameEmptyPresent.Find("TextEmpty").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEmptyPresent);
        textEmptyPresent.text = CsConfiguration.Instance.GetString("A108_TXT_00023");

        Button buttonDeleteAllPresents = m_trFrameRightPresent.Find("ButtonDeleteAll").GetComponent<Button>();
        buttonDeleteAllPresents.onClick.RemoveAllListeners();
        buttonDeleteAllPresents.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonDeleteAllPresents.onClick.AddListener(OnClickButtonDeleteAllPresent);

        Text textDeleteAllPresents = buttonDeleteAllPresents.transform.Find("TextDeleteAll").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDeleteAllPresents);
        textDeleteAllPresents.text = CsConfiguration.Instance.GetString("A108_BTN_00038");

		if (CsBlessingQuestManager.Instance.HeroBlessingReceivedList.Count > 0)
		{
			toggleBlessing.isOn = true;
		}
		else if (CsBlessingQuestManager.Instance.HeroBlessingQuestList.Count > 0)
		{
			toggleBlessingQuest.isOn = true;
		}
		else if (CsPresentManager.Instance.CsHeroReceivedPresentList.Count > 0)
		{
			togglePresent.isOn = true;
		}
		else
		{
			toggleBlessing.isOn = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeDisplay(EnPopupBless enPopupBless)
	{
        m_trFrameRightBlessing.gameObject.SetActive(enPopupBless == EnPopupBless.Blessing);
        m_trFrameRightBlessingQuest.gameObject.SetActive(enPopupBless == EnPopupBless.BlessingQuest);
		m_trFrameRightPresent.gameObject.SetActive(enPopupBless == EnPopupBless.Present);

        UpdateDisplay(enPopupBless);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDisplay(EnPopupBless enPopupBless)
	{
        Transform trContent = null;

        switch (enPopupBless)
        {
            case EnPopupBless.Blessing:
                trContent = m_trContentBlessing;
                break;

            case EnPopupBless.BlessingQuest:
                trContent = m_trContentBlessingQuest;
                break;

            case EnPopupBless.Present:
                trContent = m_trContentPresent;
                break;
        }
		
		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
		}
		
		int nListIndex = 0;
		Transform trItem;

		// 축복
        if (enPopupBless == EnPopupBless.Blessing)
		{
			GameObject goItemBlessing = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/ItemBlessing");

			foreach (CsHeroBlessing csHeroBlessing in CsBlessingQuestManager.Instance.HeroBlessingReceivedList)
			{
				if (nListIndex < trContent.childCount)
				{
					trItem = trContent.GetChild(nListIndex);
					trItem.gameObject.SetActive(true);
				}
				else
				{
					trItem = Instantiate(goItemBlessing, trContent).transform;

					Text textDescription = trItem.Find("TextDescription").GetComponent<Text>();
					CsUIData.Instance.SetFont(textDescription);
					textDescription.text = CsConfiguration.Instance.GetString("A108_TXT_00022");

					Text textReward = trItem.Find("ButtonReward/TextReward").GetComponent<Text>();
					CsUIData.Instance.SetFont(textReward);
					textReward.text = CsConfiguration.Instance.GetString("A108_BTN_00037");
				}

				trItem.name = csHeroBlessing.InstanceId.ToString();

				Text textContent = trItem.Find("TextContent").GetComponent<Text>();
				CsUIData.Instance.SetFont(textContent);
				textContent.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01032"), csHeroBlessing.SenderName, csHeroBlessing.Blessing.ReceiverGoldReward.Value);

				Button buttonReward = trItem.Find("ButtonReward").GetComponent<Button>();
				buttonReward.onClick.RemoveAllListeners();
				buttonReward.onClick.AddListener(() => OnClickButtonRewardBlessing(csHeroBlessing));

                nListIndex++;
			}
		}
		// 축복 퀘스트(인연&만남)
		else if(enPopupBless == EnPopupBless.BlessingQuest)
		{
			GameObject goItemBlessingQuest = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/ItemBlessingQuest");

			CsBlessing blessingGold = CsGameData.Instance.GetBlessing(1);
			CsBlessing blessingDia = CsGameData.Instance.GetBlessing(2);

			foreach (CsHeroBlessingQuest csHeroBlessingQuest in CsBlessingQuestManager.Instance.HeroBlessingQuestList)
			{
				if (nListIndex < trContent.childCount)
				{
					trItem = trContent.GetChild(nListIndex);
					trItem.gameObject.SetActive(true);
				}
				else
				{
					trItem = Instantiate(goItemBlessingQuest, trContent).transform;

					Text textRewardGoldBlessing = trItem.Find("TextRewardGoldBlessing").GetComponent<Text>();
					CsUIData.Instance.SetFont(textRewardGoldBlessing);
					textRewardGoldBlessing.text = CsConfiguration.Instance.GetString("A108_TXT_00019");

					Text textGoldBlessing = trItem.Find("ButtonGoldBlessing/TextGoldBlessing").GetComponent<Text>();
					CsUIData.Instance.SetFont(textGoldBlessing);
					textGoldBlessing.text = CsConfiguration.Instance.GetString("A108_BTN_00039");

					Text textTitleDiaBlessing = trItem.Find("TextTitleDiaBlessing").GetComponent<Text>();
					CsUIData.Instance.SetFont(textTitleDiaBlessing);
					textTitleDiaBlessing.text = CsConfiguration.Instance.GetString("A108_TXT_00020");

					Text textRewardDiaBlessing = trItem.Find("TextRewardDiaBlessing").GetComponent<Text>();
					CsUIData.Instance.SetFont(textRewardDiaBlessing);
					textRewardDiaBlessing.text = CsConfiguration.Instance.GetString("A108_TXT_00019");

					Text textAdditionalReward = trItem.Find("TextAdditionalReward").GetComponent<Text>();
					CsUIData.Instance.SetFont(textAdditionalReward);
					textAdditionalReward.text = CsConfiguration.Instance.GetString("A108_TXT_00021");

					Text textDiaBlessing = trItem.Find("ButtonDiaBlessing/TextDiaBlessing").GetComponent<Text>();
					CsUIData.Instance.SetFont(textDiaBlessing);
					textDiaBlessing.text = CsConfiguration.Instance.GetString("A108_BTN_00040");
				}

				trItem.name = csHeroBlessingQuest.Id.ToString();

				// 골드
				Text textContentGoldBlessing = trItem.Find("TextContentGoldBlessing").GetComponent<Text>();
				CsUIData.Instance.SetFont(textContentGoldBlessing);
				textContentGoldBlessing.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01034"), csHeroBlessingQuest.TargetName, csHeroBlessingQuest.BlessingTargetLevel.TargetHeroLevel.ToString());

				Transform trItemSlotGoldBlessing = trItem.Find("ItemSlotGoldBlessing");

				CsUIData.Instance.DisplaySmallItemSlot(trItemSlotGoldBlessing, blessingGold.SenderItemReward.Item, blessingGold.SenderItemReward.ItemOwned, blessingGold.SenderItemReward.ItemCount);

				Button buttonGoldBlessing = trItem.Find("ButtonGoldBlessing").GetComponent<Button>();
				buttonGoldBlessing.onClick.RemoveAllListeners();
				buttonGoldBlessing.onClick.AddListener(() => OnClickButtonBlessing(csHeroBlessingQuest.Id, blessingGold.BlessingId));

				Text textGoldValue = trItem.Find("ButtonGoldBlessing/TextGoldValue").GetComponent<Text>();
				CsUIData.Instance.SetFont(textGoldValue);
				textGoldValue.text = blessingGold.MoneyAmount.ToString("#,##0");

				// 다이아
				Text textContentDiaBlessing = trItem.Find("TextContentDiaBlessing").GetComponent<Text>();
				CsUIData.Instance.SetFont(textContentDiaBlessing);
				textContentDiaBlessing.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01035"), csHeroBlessingQuest.BlessingTargetLevel.ProspectQuestObjectiveLevel);

				Transform trItemSlotDiaBlessing = trItem.Find("ItemSlotDiaBlessing");

				CsUIData.Instance.DisplaySmallItemSlot(trItemSlotDiaBlessing, blessingDia.SenderItemReward.Item, blessingDia.SenderItemReward.ItemOwned, blessingDia.SenderItemReward.ItemCount);

				Transform trFrameAdditionalReward = trItem.Find("FrameAdditionalReward");

				for (int j = 0; j < trFrameAdditionalReward.childCount; j++)
				{
					trFrameAdditionalReward.GetChild(j).gameObject.SetActive(false);
				}

				int nRewardIndex = 0;
				Transform trItemSlot = null;

				foreach (CsProspectQuestOwnerReward csProspectQuestOwnerReward in csHeroBlessingQuest.BlessingTargetLevel.ProspectQuestOwnerRewardList)
				{
					if (nRewardIndex < trFrameAdditionalReward.childCount)
					{
						trItemSlot = trFrameAdditionalReward.GetChild(nRewardIndex);
						trItemSlot.gameObject.SetActive(true);
					}
					else
					{
						trItemSlot = Instantiate(m_goItemSlot64, trFrameAdditionalReward).transform;
					}

					trItemSlot.name = csProspectQuestOwnerReward.RewardNo.ToString();
					CsUIData.Instance.DisplaySmallItemSlot(trItemSlot, csProspectQuestOwnerReward.ItemReward.Item, csProspectQuestOwnerReward.ItemReward.ItemOwned, csProspectQuestOwnerReward.ItemReward.ItemCount);
				}

				Button buttonDiaBlessing = trItem.Find("ButtonDiaBlessing").GetComponent<Button>();
				buttonDiaBlessing.onClick.RemoveAllListeners();
				buttonDiaBlessing.onClick.AddListener(() => OnClickButtonBlessing(csHeroBlessingQuest.Id, blessingDia.BlessingId));

				Text textDiaValue = trItem.Find("ButtonDiaBlessing/TextDiaValue").GetComponent<Text>();
				CsUIData.Instance.SetFont(textDiaValue);
				textDiaValue.text = blessingDia.MoneyAmount.ToString("#,##0");

                nListIndex++;
			}
		}
        // 증정
        else if (enPopupBless == EnPopupBless.Present)
        {
            GameObject goItemPresent = CsUIData.Instance.LoadAsset<GameObject>("GUI/MainUI/ItemPresent");

            for (int i = 0; i < m_trContentPresent.childCount; i++)
            {
                m_trContentPresent.GetChild(i).gameObject.SetActive(false);
            }

            Transform trItemPresent = null;

            for (int i = 0; i < CsPresentManager.Instance.CsHeroReceivedPresentList.Count; i++)
            {
                CsHeroReceivedPresent csHeroReceivedPresent = CsPresentManager.Instance.CsHeroReceivedPresentList[i];

                trItemPresent = m_trContentPresent.Find("ItemPresent" + i);

                if (trItemPresent == null)
                {
                    trItemPresent = Instantiate(goItemPresent, m_trContentPresent).transform;
                    trItemPresent.name = "ItemPresent" + i;
                }
                else
                {
                    trItemPresent.gameObject.SetActive(true);
                }

                Text textNewPresent = trItemPresent.Find("TextNewPresent").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNewPresent);
                textNewPresent.text = CsConfiguration.Instance.GetString("A108_TXT_00025");

                Text textDescription = trItemPresent.Find("TextDescription").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDescription);
                textDescription.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01038"), csHeroReceivedPresent.SenderName, csHeroReceivedPresent.Present.Name);

                Button buttonReply = trItemPresent.Find("ButtonReply").GetComponent<Button>();
                buttonReply.onClick.RemoveAllListeners();
                buttonReply.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                buttonReply.onClick.AddListener(() => OnClickPresentReply(csHeroReceivedPresent));

                Text textReply = buttonReply.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textReply);
                textReply.text = CsConfiguration.Instance.GetString("A108_BTN_00046");

                Button buttonPresent = trItemPresent.Find("ButtonPresent").GetComponent<Button>();
                buttonPresent.onClick.RemoveAllListeners();
                buttonPresent.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                buttonPresent.onClick.AddListener(() => OnClickPresent(csHeroReceivedPresent));

                Text textPresent = buttonPresent.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textPresent);
                textPresent.text = CsConfiguration.Instance.GetString("A108_BTN_00047");
            }
        }

		UpdateTextCount(enPopupBless);
	}

	//---------------------------------------------------------------------------------------------------
	void DeleteItem(Transform trItem, EnPopupBless enPopupBless)
	{
		Transform trContent = trItem.parent;

		trItem.gameObject.SetActive(false);

        UpdateTextCount(enPopupBless);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateTextCount(EnPopupBless enPopupBless)
	{
        if (enPopupBless == EnPopupBless.Blessing)
		{
			Text textCount = m_trFrameRightBlessing.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCount);
			textCount.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01033"), CsBlessingQuestManager.Instance.HeroBlessingReceivedList.Count, CsGameConfig.Instance.BlessingListMaxCount);
			
			m_trContentBlessing.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroBlessingReceivedList.Count > 0);
			m_trFrameEmptyBlessing.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroBlessingReceivedList.Count <= 0);
		}
		else if(enPopupBless == EnPopupBless.BlessingQuest)
		{
			Text textCount = m_trFrameRightBlessingQuest.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCount);
			textCount.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01033"), CsBlessingQuestManager.Instance.HeroBlessingQuestList.Count, CsGameConfig.Instance.BlessingQuestListMaxCount);

			m_trContentBlessingQuest.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroBlessingQuestList.Count > 0);
			m_trFrameEmptyBlessingQuest.gameObject.SetActive(CsBlessingQuestManager.Instance.HeroBlessingQuestList.Count <= 0);
		}
        else if (enPopupBless == EnPopupBless.Present)
        {
            Text textCount = m_trFrameRightPresent.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = string.Format(CsConfiguration.Instance.GetString("A108_TXT_01033"), CsPresentManager.Instance.CsHeroReceivedPresentList.Count, 99);
            
            m_trContentPresent.gameObject.SetActive(CsPresentManager.Instance.CsHeroReceivedPresentList.Count > 0);
            m_trFrameEmptyPresent.gameObject.SetActive(CsPresentManager.Instance.CsHeroReceivedPresentList.Count <= 0);
        }
	}

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupPresent(System.Guid guidTargetHeroId)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupFriend/PopupPresent");
        yield return resourceRequest;

        m_trPopupPresent = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        m_trPopupPresent.name = "PopupPresent";

        CsGameEventUIToUI.Instance.OnEventOpenPopupPresent(guidTargetHeroId);
        m_bOpenPopupPresent = false;
    }

	#region Event
	//---------------------------------------------------------------------------------------------------
	// 좌측 토글 이벤트
	void OnValueChangedToggle(bool bIsOn, EnPopupBless enPopupBless)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            ChangeDisplay(enPopupBless);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 사례 버튼 클릭 이벤트(보상받기)
	void OnClickButtonRewardBlessing(CsHeroBlessing csHeroBlessing)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A108_TXT_02003"), csHeroBlessing.SenderName));

		CsBlessingQuestManager.Instance.SendBlessingRewardReceive(csHeroBlessing.InstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	// 축복
	void OnClickButtonBlessing(long lQuestId, int nBlessingId)
	{
		CsBlessing csBlessing = CsGameData.Instance.GetBlessing(nBlessingId);

		if (csBlessing != null)
		{
			if (csBlessing.MoneyType == 1)
			{
				if (CsGameData.Instance.MyHeroInfo.Gold < csBlessing.MoneyAmount)
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03001"));
					return;
				}
			}
			else if (csBlessing.MoneyType == 2)
			{
				if (CsGameData.Instance.MyHeroInfo.Dia < csBlessing.MoneyAmount)
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A107_TXT_03002"));
					return;
				}

				if (CsBlessingQuestManager.Instance.HeroProspectQuestOwnerList.Count >= CsGameConfig.Instance.OwnerProspectQuestListMaxCount)
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_00024"));
					return;
				}
			}
		}

		CsBlessingQuestManager.Instance.SendBlessingQuestBlessingSend(lQuestId, nBlessingId);
	}

	//---------------------------------------------------------------------------------------------------
	// 축복 전체삭제
	void OnClickButtonDeleteAllBlessings()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsBlessingQuestManager.Instance.SendBlessingDeleteAll();
	}

	//---------------------------------------------------------------------------------------------------
	// 축복 퀘스트(인연&만남) 전체삭제
	void OnClickButtonDeleteAllBlessingQuests()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsBlessingQuestManager.Instance.SendBlessingQuestDeleteAll();
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonDeleteAllPresent()
    {
        CsPresentManager.Instance.DeleteAllHeroReceivedPresentList();
        UpdateDisplay(EnPopupBless.Present);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingRewardReceive(long lInstanceId)
	{
		Transform trItemBlessing = m_trContentBlessing.Find(lInstanceId.ToString());

		if (trItemBlessing != null)
		{
			DeleteItem(trItemBlessing, EnPopupBless.Blessing);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingDeleteAll()
	{
		for (int i = 0; i < m_trContentBlessing.childCount; i++)
		{
			m_trContentBlessing.GetChild(i).gameObject.SetActive(false);
		}

		UpdateTextCount(EnPopupBless.Blessing);
	}
	
	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestBlessingSend(long lQuestId)
	{
		Transform trItemBlessingQuest = m_trContentBlessingQuest.Find(lQuestId.ToString());

		if (trItemBlessingQuest != null)
		{
			DeleteItem(trItemBlessingQuest, EnPopupBless.BlessingQuest);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBlessingQuestDeleteAll()
	{
		for (int i = 0; i < m_trContentBlessingQuest.childCount; i++)
		{
			m_trContentBlessingQuest.GetChild(i).gameObject.SetActive(false);
		}

		UpdateTextCount(EnPopupBless.BlessingQuest);
	}

    // 선물 수신
	//---------------------------------------------------------------------------------------------------
    void OnEventPresentReceived()
    {
        UpdateDisplay(EnPopupBless.Present);
        UpdateTextCount(EnPopupBless.Present);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentReply()
    {
        CsPresentManager.Instance.RemoveHeroReceivedPresentList(m_csHeroReceivedPresent);

        UpdateDisplay(EnPopupBless.Present);
        UpdateTextCount(EnPopupBless.Present);

        CsGameEventUIToUI.Instance.OnEventCheckPresentButtonDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPresentSend(int nPresentId)
    {
        CsPresentManager.Instance.RemoveHeroReceivedPresentList(m_csHeroReceivedPresent);
        Destroy(m_trPopupPresent.gameObject);

        UpdateDisplay(EnPopupBless.Present);
        UpdateTextCount(EnPopupBless.Present);

        CsGameEventUIToUI.Instance.OnEventCheckPresentButtonDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPresentReply(CsHeroReceivedPresent csHeroReceivedPresent)
    {
        m_csHeroReceivedPresent = csHeroReceivedPresent;
        CsPresentManager.Instance.SendPresentReply(csHeroReceivedPresent.SenderId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPresent(CsHeroReceivedPresent csHeroReceivedPresent)
    {
        m_csHeroReceivedPresent = csHeroReceivedPresent;

        if (!m_bOpenPopupPresent)
        {
            m_bOpenPopupPresent = true;
            StartCoroutine(LoadPopupPresent(csHeroReceivedPresent.SenderId));
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
	#endregion Event
}
