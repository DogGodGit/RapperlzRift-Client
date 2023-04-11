using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-08-27)
//---------------------------------------------------------------------------------------------------

public class CsPopupBiography : CsPopupSub 
{
	[SerializeField]
	GameObject m_goItemSlot;

	Transform m_trContent;
	bool m_bInitialized = false;

	Button m_buttonStart;
	Button m_buttonFinish;

	CsBiography m_csBiography;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsGameEventUIToUI.Instance.EventSelectToggleBiography += OnEventSelectToggleBiography;
		CsBiographyManager.Instance.EventBiographyComplete += OnEventBiographyComplete;

		m_trContent = transform.Find("Scroll View/Viewport/Content");

		InitializeUI();
		CreateBiographyList();

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsGameEventUIToUI.Instance.EventSelectToggleBiography -= OnEventSelectToggleBiography;
		CsBiographyManager.Instance.EventBiographyComplete -= OnEventBiographyComplete;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Transform trFrameInfo = transform.Find("FrameInfo");

		Text textAcquisitionRoute = trFrameInfo.Find("TextAcquisitionRoute").GetComponent<Text>();
		CsUIData.Instance.SetFont(textAcquisitionRoute);
		textAcquisitionRoute.text = CsConfiguration.Instance.GetString("A122_TXT_00004");

		Text textReward = trFrameInfo.Find("TextReward").GetComponent<Text>();
		CsUIData.Instance.SetFont(textReward);
		textReward.text = CsConfiguration.Instance.GetString("A122_TXT_00005");

		Text textButtonStart = trFrameInfo.Find("ButtonStart/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonStart);
		textButtonStart.text = CsConfiguration.Instance.GetString("A122_BTN_00001");

		Text textButtonFinish = trFrameInfo.Find("ButtonFinish/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonFinish);
		textButtonFinish.text = CsConfiguration.Instance.GetString("A122_BTN_00004");

		m_buttonStart = trFrameInfo.Find("ButtonStart").GetComponent<Button>();
		m_buttonStart.onClick.RemoveAllListeners();
		m_buttonStart.onClick.AddListener(OnClickBiographyStart);

		m_buttonFinish = trFrameInfo.Find("ButtonFinish").GetComponent<Button>();
		m_buttonFinish.onClick.RemoveAllListeners();
		m_buttonFinish.onClick.AddListener(OnClickBiographyFinish);
	}

	//---------------------------------------------------------------------------------------------------
	void CreateBiographyList()
	{
		GameObject goBiographyItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupBiography/BiographyItem");

		ToggleGroup toggleGroup = m_trContent.GetComponent<ToggleGroup>();

		foreach (var biography in CsGameData.Instance.BiographyList)
		{
			Transform trBiographyItem = Instantiate(goBiographyItem, m_trContent).transform;
			trBiographyItem.name = biography.BiographyId.ToString();

			Transform trItemSlot = trBiographyItem.Find("ItemSlot");
			CsUIData.Instance.DisplayItemSlot(trItemSlot, biography.RequiredItem, false, 0, false, EnItemSlotSize.Medium, false);

			Text textTitle = trBiographyItem.Find("TextTitle").GetComponent<Text>();
			CsUIData.Instance.SetFont(textTitle);
			textTitle.text = string.Format(CsConfiguration.Instance.GetString("A122_TXT_00001"), biography.BiographyId);

			Text textName = trBiographyItem.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = biography.Name;

			trBiographyItem.Find("ImageLock").gameObject.SetActive(CsBiographyManager.Instance.GetHeroBiography(biography.BiographyId) == null);

			Toggle toggle = trBiographyItem.GetComponent<Toggle>();
			toggle.group = toggleGroup;
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((isOn) => OnValueChangedBiographyToggle(isOn, biography));

			UpdateNotice(biography);
		}

		Toggle toggleFirst = m_trContent.GetChild(0).GetComponent<Toggle>();
		toggleFirst.isOn = true;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateNotice(CsBiography csBiography)
	{
		Transform trBiography = m_trContent.Find(csBiography.BiographyId.ToString());

		if (trBiography != null)
		{
			CsHeroBiography csHeroBiography = CsBiographyManager.Instance.GetHeroBiography(csBiography.BiographyId);

			if (csHeroBiography != null)
			{
				trBiography.Find("ImageNotice").gameObject.SetActive(CsBiographyManager.Instance.CheckBiographyNotice(csHeroBiography));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateBiographyInfo()
	{
		Transform trFrameInfo = transform.Find("FrameInfo");

		Transform trItemSlot = trFrameInfo.Find("ImageFrame/ItemSlot");

		CsUIData.Instance.DisplayItemSlot(trItemSlot, m_csBiography.RequiredItem, false, 0, false, EnItemSlotSize.Medium, false);

		Text textTitle = trFrameInfo.Find("TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = string.Format(CsConfiguration.Instance.GetString("A122_TXT_00002"), m_csBiography.BiographyId, m_csBiography.Name);

		CsHeroBiography csHeroBiography = CsBiographyManager.Instance.GetHeroBiography(m_csBiography.BiographyId);
		
		if (csHeroBiography == null)
		{
			trFrameInfo.Find("IconLock").gameObject.SetActive(true);
			trFrameInfo.Find("TextLock").gameObject.SetActive(true);
			trFrameInfo.Find("TextContent").gameObject.SetActive(false);

			Text textLock = trFrameInfo.Find("TextLock").GetComponent<Text>();
			CsUIData.Instance.SetFont(textLock);
			textLock.text = CsConfiguration.Instance.GetString("A122_TXT_00003");
		}
		else
		{
			if (csHeroBiography.Completed)
			{
				trFrameInfo.Find("IconLock").gameObject.SetActive(false);
				trFrameInfo.Find("TextLock").gameObject.SetActive(false);
				trFrameInfo.Find("TextContent").gameObject.SetActive(true);

				Text textContent = trFrameInfo.Find("TextContent").GetComponent<Text>();
				CsUIData.Instance.SetFont(textContent);
				textContent.text = m_csBiography.Description;
			}
			else
			{
				trFrameInfo.Find("IconLock").gameObject.SetActive(true);
				trFrameInfo.Find("TextLock").gameObject.SetActive(true);
				trFrameInfo.Find("TextContent").gameObject.SetActive(false);

				Text textLock = trFrameInfo.Find("TextLock").GetComponent<Text>();
				CsUIData.Instance.SetFont(textLock);
				textLock.text = CsConfiguration.Instance.GetString("A122_TXT_00003");
			}
		}

		Text textValueAcquisitionRoute = trFrameInfo.Find("TextValueAcquisitionRoute").GetComponent<Text>();
		CsUIData.Instance.SetFont(textValueAcquisitionRoute);
		textValueAcquisitionRoute.text = m_csBiography.OpenConditionText;

		Transform trFrameReward = trFrameInfo.Find("FrameReward");

		for (int i = 0 ; i < trFrameReward.childCount; i++)
		{
			trFrameReward.GetChild(i).gameObject.SetActive(false);
		}

		int nIndex = 0;

		foreach (var reward in m_csBiography.BiographyRewardList)
		{
			Transform trRewardItemSlot;

			if (nIndex < trFrameReward.childCount)
			{
				trRewardItemSlot = trFrameReward.GetChild(nIndex);
				trRewardItemSlot.gameObject.SetActive(true);
			}
			else
			{
				trRewardItemSlot = Instantiate(m_goItemSlot, trFrameReward).transform;
				trRewardItemSlot.name = reward.RewardNo.ToString();
			}

			CsUIData.Instance.DisplayItemSlot(trRewardItemSlot, reward.ItemReward.Item, reward.ItemReward.ItemOwned, reward.ItemReward.ItemCount, false, EnItemSlotSize.Medium, false);

			trRewardItemSlot.Find("ImageDim").gameObject.SetActive(csHeroBiography != null && csHeroBiography.Completed);

			nIndex++;
		}

		int nLastQuestNo = m_csBiography.BiographyQuestList.Max(quest => quest.QuestNo);
		CsHeroBiographyQuest csHeroBiographyQuest = csHeroBiography == null ? null : csHeroBiography.HeroBiograhyQuest;
		
		// 전기를 완료한 경우 버튼 비활성화
		if (csHeroBiography != null && csHeroBiography.Completed)
		{
			m_buttonStart.gameObject.SetActive(false);
			m_buttonFinish.gameObject.SetActive(false);
		}
		else
		{
			// 마지막 퀘스트를 완료한 경우 전기 완료 버튼 활성화
			if (csHeroBiographyQuest != null && csHeroBiographyQuest.BiographyQuest.QuestNo >= nLastQuestNo &&
				csHeroBiographyQuest.Completed)
			{
				m_buttonStart.gameObject.SetActive(false);
				m_buttonFinish.gameObject.SetActive(true);
			}
			else
			{
				m_buttonStart.gameObject.SetActive(true);
				m_buttonFinish.gameObject.SetActive(false);
			}
		}

		m_buttonStart.interactable = csHeroBiography != null;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SelectBiography(int nBiographyId)
	{
		Transform trBiography = null;

		yield return new WaitUntil(() => 
		{
			trBiography = m_trContent.Find(nBiographyId.ToString());
			return trBiography != null;
		});

		if (trBiography != null)
		{
			Toggle toggle = trBiography.GetComponent<Toggle>();
			toggle.isOn = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedBiographyToggle(bool bIsOn, CsBiography csBiography)
	{
		if (bIsOn)
		{
			m_csBiography = csBiography;

			UpdateBiographyInfo();

			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSelectToggleBiography(int nBiographyId)
	{
		StartCoroutine(SelectBiography(nBiographyId));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyComplete(int nBiographyId)
	{
		m_csBiography = CsGameData.Instance.GetBiography(nBiographyId);
		UpdateNotice(m_csBiography);
		UpdateBiographyInfo();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickBiographyStart()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

		CsHeroBiography csHeroBiogrpahy = CsBiographyManager.Instance.GetHeroBiography(m_csBiography.BiographyId);
		
		if (csHeroBiogrpahy.HeroBiograhyQuest == null)
		{
			CsBiographyQuest nextQuest = m_csBiography.GetBiographyQuest(1);
			
			// 시작 NPC 경로 표시
			if (nextQuest != null && nextQuest.StartNpc != null)
			{
				CsGameEventToIngame.Instance.OnEventNpcAutoMove(CsGameData.Instance.MyHeroInfo.Nation.NationId, nextQuest.StartNpc);
				CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.FindingPath);

				// 전기 바로가기 버튼 닫기
				if (CsBiographyManager.Instance.OpenedBiographyId == m_csBiography.BiographyId)
				{
					CsGameEventUIToUI.Instance.OnEventClosePanelBiography();
				}
			}
		}
		else
		{

			if (csHeroBiogrpahy.Completed)
			{
				CsBiographyQuest nextQuest = csHeroBiogrpahy.Biography.GetBiographyQuest(csHeroBiogrpahy.HeroBiograhyQuest.QuestNo + 1);

				// 다음 퀘스트가 있는 경우 시작 NPC 경로 표시
				if (nextQuest != null && nextQuest.StartNpc != null)
				{
					CsGameEventToIngame.Instance.OnEventNpcAutoMove(CsGameData.Instance.MyHeroInfo.Nation.NationId, nextQuest.StartNpc);
					CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.FindingPath);
				}
			}
			else
			{
				// 자동 진행
				CsBiographyManager.Instance.StartAutoPlay(m_csBiography.BiographyId);
				CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Biography);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickBiographyFinish()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsBiographyManager.Instance.SendBiographyComplete(m_csBiography.BiographyId);
	}
}
