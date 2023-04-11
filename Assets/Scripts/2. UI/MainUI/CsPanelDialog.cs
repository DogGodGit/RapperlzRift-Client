using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-08-28)
//---------------------------------------------------------------------------------------------------

enum EnQuestDialogType : int
{
	None = 0,
	MainQuest = 1,
	Biography = 2,
}

public class CsPanelDialog : MonoBehaviour {

	struct StDialog
	{
		public int NpcId;
		public string Dialog;
	}

	EnQuestDialogType m_enQuestDialogType;

	Text m_textNpcName;
	Text m_textNpcDialog;
	Transform m_trImageNextRight;

	Text m_textHeroName;
	Text m_textHeroDialog;
	Transform m_trImageNextLeft;

	Text m_textAccept;

	Transform m_trButtonSkip;
	Transform m_trButtonAccept;
	Transform m_trButtonCancel;
	Transform m_trImageFrameReward;

	// 아이템 정보창
	Transform m_trFrame;
	Transform m_trPopupList;
	Transform m_trItemInfo;
	GameObject m_goPopupItemInfo;
	CsPopupItemInfo m_csPopupItemInfo;
	
	List<StDialog> m_listDialog;

	// 전기 퀘스트
	CsBiographyQuest m_csBiographyQuest;

	bool m_bDialogFinished = true;
	bool m_bHidePanel = false;
	bool m_bOnEventVisibleMainUICallBySelf = false;
	
	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsBiographyManager.Instance.EventNpcStartDialog += OnEventNpcStartDialog;
		CsGameEventUIToUI.Instance.EventMainQuestNpcDialog += OnEventMainQuestNpcDialog;
		
		CsGameEventToUI.Instance.EventStartTutorial += OnEventStartTutorial;
		CsGameEventUIToUI.Instance.EventReferenceTutorial += OnEventReferenceTutorial;
		CsGameEventToUI.Instance.EventHideMainUI += OnEventHideMainUI;
		CsGameEventUIToUI.Instance.EventVisibleMainUI += OnEventVisibleMainUI;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsBiographyManager.Instance.EventNpcStartDialog -= OnEventNpcStartDialog;
		CsGameEventUIToUI.Instance.EventMainQuestNpcDialog -= OnEventMainQuestNpcDialog;
		
		CsGameEventToUI.Instance.EventStartTutorial -= OnEventStartTutorial;
		CsGameEventUIToUI.Instance.EventReferenceTutorial -= OnEventReferenceTutorial;
		CsGameEventToUI.Instance.EventHideMainUI -= OnEventHideMainUI;
		CsGameEventUIToUI.Instance.EventVisibleMainUI -= OnEventVisibleMainUI;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_trFrame = transform.Find("Frame");
		m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;
		m_enQuestDialogType = EnQuestDialogType.None;
		m_listDialog = new List<StDialog>();

		Transform trFrame = transform.Find("Frame");

		Transform trFrameDialog = trFrame.Find("FrameDialog");

		// Npc
		m_textNpcName = trFrameDialog.Find("TextNpcName").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textNpcName);

		m_textNpcDialog = trFrameDialog.Find("TextNpcDialog").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textNpcDialog);

		m_trImageNextRight = trFrameDialog.Find("ImageNextRight");

		// Hero
		m_textHeroName = trFrameDialog.Find("TextHeroName").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textHeroName);

		m_textHeroDialog = trFrameDialog.Find("TextHeroDialog").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textHeroDialog);

		m_trImageNextLeft = trFrameDialog.Find("ImageNextLeft");

		m_trButtonSkip = trFrame.Find("FrameImage/ButtonSkip");
		CsUIData.Instance.SetButton(m_trButtonSkip, OnClickButtonSkip);
		CsUIData.Instance.SetText(m_trButtonSkip.Find("TextSkip"), "PUBLIC_SKIP", true);

		Button buttonNext = transform.Find("Frame").GetComponent<Button>();
		buttonNext.onClick.RemoveAllListeners();
		buttonNext.onClick.AddListener(OnClickButtonNext);

		CsUIData.Instance.SetButton(trFrameDialog.Find("ButtonAccept"), OnClickButtonAccept);
		m_trButtonAccept = trFrameDialog.Find("ButtonAccept");

		CsUIData.Instance.SetButton(trFrameDialog.Find("ButtonCancel"), OnClickButtonCancel);
		CsUIData.Instance.SetText(trFrameDialog.Find("ButtonCancel/TextCancel"), "PUBLIC_BTN_NO", true);
		m_trButtonCancel = trFrameDialog.Find("ButtonCancel");

		m_textAccept = trFrameDialog.Find("ButtonAccept/TextAccept").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textAccept);
		
		m_trImageFrameReward = trFrame.Find("ImageFrameReward");
		CsUIData.Instance.SetText(m_trImageFrameReward.Find("TextReward"), "PUBLIC_REWARD", true);
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeButtons()
	{
		m_trButtonAccept.gameObject.SetActive(false);
		m_trButtonCancel.gameObject.SetActive(false);
		m_trImageFrameReward.gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void PanelDisplay(bool bActive, bool bChangeMainUIVisibility = true)
	{
		m_bDialogFinished = !bActive;

		if (!bActive)
		{
			m_bHidePanel = false;
		}

		// 튜토리얼 이벤트로 인해 다이얼로그 창이 종료되는 경우가 아닐 때만 메인UI 전환
		if (bChangeMainUIVisibility)
		{
			m_bOnEventVisibleMainUICallBySelf = true;

			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(!bActive);
		}

		if (!bActive)
		{
			m_enQuestDialogType = EnQuestDialogType.None;
			m_listDialog.Clear();

			Transform tr3DCharacter = transform.Find("Frame/3DCharacter");
			for (int i = 0; i < tr3DCharacter.childCount; ++i)
			{
				if (tr3DCharacter.GetChild(i).GetComponent<Camera>() != null)
				{
					continue;
				}

				Destroy(tr3DCharacter.GetChild(i).gameObject);
			}

			Transform tr3DNpc = transform.Find("Frame/3DNpc");
			for (int i = 0; i < tr3DNpc.childCount; ++i)
			{
				if (tr3DNpc.GetChild(i).GetComponent<Camera>() != null)
				{
					continue;
				}

				Destroy(tr3DNpc.GetChild(i).gameObject);
			}
		}

		m_trFrame.gameObject.SetActive(bActive && !m_bHidePanel);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDialog()
	{
		if (m_listDialog.Count > 0)
		{
			UpdateDialogText();
			m_listDialog.RemoveAt(0);

			m_trButtonSkip.gameObject.SetActive(m_listDialog.Count > 0);

			if (m_enQuestDialogType == EnQuestDialogType.MainQuest)
			{
				if (m_listDialog.Count <= 0)
				{
					m_trImageNextRight.gameObject.SetActive(false);
					m_trImageNextLeft.gameObject.SetActive(false);

					m_trButtonAccept.gameObject.SetActive(true);
					m_trButtonCancel.gameObject.SetActive(CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.Executed);
					m_trImageFrameReward.gameObject.SetActive(true);

					// 보상, 버튼 세팅
					if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None)
					{
						m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_ACCEPT");
					}
					else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed)
					{
						m_textAccept.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_COMPLETE");
					}

					// 골드, 경험치
					Transform trFrameReward = m_trImageFrameReward.Find("FrameReward");

					for (int i = 0; i < trFrameReward.childCount; i++)
					{
						trFrameReward.GetChild(i).gameObject.SetActive(false);
					}

					trFrameReward.Find("RewardExp").gameObject.SetActive(true);
					trFrameReward.Find("RewardGold").gameObject.SetActive(true);

					CsUIData.Instance.SetText(trFrameReward.Find("RewardExp/TextExp"), CsMainQuestManager.Instance.MainQuest.RewardExp.ToString("#,##0"), false);
					CsUIData.Instance.SetText(trFrameReward.Find("RewardGold/TextGold"), CsMainQuestManager.Instance.MainQuest.RewardGold.ToString("#,##0"), false);

					// 아이템
					m_trImageFrameReward.Find("FrameItemReward").gameObject.SetActive(CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList.Count > 0);

					Transform trFrameItemReward = m_trImageFrameReward.Find("FrameItemReward");

					for (int i = 0; i < trFrameItemReward.childCount; i++)
					{
						trFrameItemReward.GetChild(i).gameObject.SetActive(false);
					}

					for (int i = 0; i < CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList.Count; i++)
					{
						int nRewardIndex = i;

						if (i < trFrameItemReward.childCount)
						{
							Transform trSlot = trFrameItemReward.GetChild(i);
							
							CsUIData.Instance.SetButton(trSlot, () => OnClickRewardItem(nRewardIndex));

							if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.MainGear)
							{
								//if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.JobId == CsGameData.Instance.MyHeroInfo.Job.JobId ||
								//    CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear.JobId == 0)
								//{
								//    CsUIData.Instance.DisplayItemSlot(trSlot, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGear, 0, 0, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].MainGearOwned,  EnItemSlotSize.Medium);
								//}
							}
							else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.SubGear)
							{
								CsUIData.Instance.DisplayItemSlot(trSlot, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].SubGear, 1, 1, EnItemSlotSize.Medium);
								trSlot.gameObject.SetActive(true);
							}
							else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.Item)
							{
								CsUIData.Instance.DisplayItemSlot(trSlot, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Item, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].ItemOwned, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].ItemCount, false, EnItemSlotSize.Medium, false);
								trSlot.gameObject.SetActive(true);
							}
							else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.Mount)
							{
								CsUIData.Instance.DisplayMount(trSlot, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Mount);
								trSlot.gameObject.SetActive(true);
							}
							else if (CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].Type == (int)EnMainQuestRewardType.CreatureCard)
							{
								CsUIData.Instance.DisplayCreatureCard(trSlot, CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[i].CreatureCard);
								trSlot.gameObject.SetActive(true);
							}
						}
					}
				}
			}
		}
		else
		{
			if (m_enQuestDialogType == EnQuestDialogType.Biography)
			{
				PanelDisplay(false);
				
				// 퀘스트 수락 창
				CsGameEventUIToUI.Instance.OnEventBiographyNpcDialog(m_csBiographyQuest);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDialogText()
	{
		if (m_listDialog[0].NpcId == 0)
		{
			OffModels();

			m_textNpcName.gameObject.SetActive(false);
			m_textNpcDialog.gameObject.SetActive(false);
			m_trImageNextRight.gameObject.SetActive(false);

			m_textHeroName.gameObject.SetActive(true);
			m_textHeroDialog.gameObject.SetActive(true);
			m_trImageNextLeft.gameObject.SetActive(true);

			// 영웅모델링
			LoadHeroModel();

			m_textHeroDialog.text = m_listDialog[0].Dialog;
		}
		else
		{
			CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(m_listDialog[0].NpcId);

			if (csNpcInfo != null)
			{
				OffModels();

				m_textNpcName.gameObject.SetActive(true);
				m_textNpcDialog.gameObject.SetActive(true);
				m_trImageNextRight.gameObject.SetActive(true);

				m_textHeroName.gameObject.SetActive(false);
				m_textHeroDialog.gameObject.SetActive(false);
				m_trImageNextLeft.gameObject.SetActive(false);

				// NPC모델링
				LoadNpcModel(csNpcInfo);

				m_textNpcName.text = csNpcInfo.Name;
				m_textNpcDialog.text = m_listDialog[0].Dialog;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupItemInfo(UnityAction unityAction)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
		yield return resourceRequest;
		m_goPopupItemInfo = (GameObject)resourceRequest.asset;

		unityAction();
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo(CsMainQuestReward csMainQuestReward)
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

		switch ((EnMainQuestRewardType)csMainQuestReward.Type)
		{
			case EnMainQuestRewardType.MainGear:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.MainGear, false);
				break;
			case EnMainQuestRewardType.SubGear:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.SubGear, false);
				break;
			case EnMainQuestRewardType.Item:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.Item, 0, csMainQuestReward.ItemOwned, -1, false);
				break;
			case EnMainQuestRewardType.CreatureCard:
				m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csMainQuestReward.CreatureCard);
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OffModels()
	{
		m_trFrame.Find("3DCharacter").gameObject.SetActive(false);
		m_trFrame.Find("3DNpc").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void LoadHeroModel()
	{
		Transform tr3DCharacter = m_trFrame.Find("3DCharacter");
		tr3DCharacter.gameObject.SetActive(true);

		for (int i = 0; i < tr3DCharacter.childCount; ++i)
		{
			if (tr3DCharacter.GetChild(i).GetComponent<Camera>() != null)
			{
				continue;
			}

			tr3DCharacter.GetChild(i).gameObject.SetActive(false);
		}

		Transform trMountModel = m_trFrame.Find("3DCharacter/MyHero");

		if (trMountModel == null)
		{
			StartCoroutine(LoadHeroModelCoroutine());
		}
		else
		{
			trMountModel.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadHeroModelCoroutine()
	{
        // %% JobId 추후 수정
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
		yield return resourceRequest;

		m_trFrame.Find("3DCharacter/UIChar_Camera").gameObject.SetActive(true);

		GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_trFrame.Find("3DCharacter"));

		int nLayer = LayerMask.NameToLayer("UIChar");

		Transform[] atrMount = goCharacter.GetComponentsInChildren<Transform>();

		for (int i = 0; i < atrMount.Length; ++i)
		{
			atrMount[i].gameObject.layer = nLayer;
		}

		switch (nJobId)
		{
			case (int)EnJob.Gaia:
				goCharacter.transform.localPosition = new Vector3(0, -218, 600);
				goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
				goCharacter.transform.localScale = new Vector3(210, 210, 210);
				break;
			case (int)EnJob.Asura:
				goCharacter.transform.localPosition = new Vector3(0, -175, 450);
				goCharacter.transform.eulerAngles = new Vector3(0, 185, 0);
				goCharacter.transform.localScale = new Vector3(210, 210, 210);
				break;
			case (int)EnJob.Deva:
				goCharacter.transform.localPosition = new Vector3(0, -210, 600);
				goCharacter.transform.eulerAngles = new Vector3(0, 175, 0);
				goCharacter.transform.localScale = new Vector3(220, 220, 220);
				break;
			case (int)EnJob.Witch:
				goCharacter.transform.localPosition = new Vector3(0, -150, 430);
				goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
				goCharacter.transform.localScale = new Vector3(190, 190, 190);
				break;
		}

		goCharacter.GetComponent<CsUICharcterRotate>().UICamera = m_trFrame.Find("3DCharacter/UIChar_Camera").GetComponent<Camera>();

		goCharacter.name = "MyHero";
		goCharacter.tag = "Untagged";
		goCharacter.gameObject.SetActive(true);

		UpdateCharacterModel();
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCharacterModel()
	{
		Transform trCharacterModel = m_trFrame.Find("3DCharacter/MyHero");

		if (trCharacterModel != null)
		{
			CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
			CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

			csEquipment.MidChangeEquipments(csHeroCustomData, false);
			csEquipment.CreateWing(csHeroCustomData, null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void LoadNpcModel(CsNpcInfo csNpcInfo)
	{
		Transform tr3DNpc = m_trFrame.Find("3DNpc");
		tr3DNpc.gameObject.SetActive(true);

		for (int i = 0; i < tr3DNpc.childCount; ++i)
		{
			if (tr3DNpc.GetChild(i).GetComponent<Camera>() != null)
			{
				continue;
			}

			tr3DNpc.GetChild(i).gameObject.SetActive(false);
		}

		Transform trMountModel = m_trFrame.Find("3DNpc/" + csNpcInfo.Name);

		if (trMountModel == null)
		{
			StartCoroutine(LoadNpcModelCoroutine(csNpcInfo));
		}
		else
		{
			trMountModel.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadNpcModelCoroutine(CsNpcInfo csNpcInfo)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/NpcObject/" + csNpcInfo.PrefabName);
		yield return resourceRequest;

		m_trFrame.Find("3DNpc/UIChar_Camera").gameObject.SetActive(true);

		GameObject goNpc = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_trFrame.Find("3DNpc"));

		int nLayer = LayerMask.NameToLayer("UIChar");

		Transform[] atrMount = goNpc.GetComponentsInChildren<Transform>();

		for (int i = 0; i < atrMount.Length; ++i)
		{
			atrMount[i].gameObject.layer = nLayer;
		}

		goNpc.transform.localPosition = new Vector3(0, -130, 500);
		goNpc.transform.eulerAngles = new Vector3(0, 180, 0);

		goNpc.transform.localScale = new Vector3(150, 150, 150);
		goNpc.name = csNpcInfo.Name;
		goNpc.tag = "Untagged";
		goNpc.gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator HidePanel()
	{
		m_trFrame.gameObject.SetActive(false);

		yield return new WaitUntil(() => !m_bHidePanel);

		if (!m_bDialogFinished)
		{
			m_trFrame.gameObject.SetActive(true);

			m_bOnEventVisibleMainUICallBySelf = true;

			CsGameEventUIToUI.Instance.OnEventVisibleMainUI(true);
		}
	}

	//---------------------------------------------------------------------------------------------------

	#region event

	//---------------------------------------------------------------------------------------------------
	void OnEventNpcStartDialog(CsBiographyQuest csBiographyQuest)
	{
		InitializeButtons();
		
		if (csBiographyQuest.BiographyQuestStartDialogueList == null ||
			csBiographyQuest.BiographyQuestStartDialogueList.Count <= 0)
		{
			CsGameEventUIToUI.Instance.OnEventBiographyNpcDialog(csBiographyQuest);
			return;
		}

		PanelDisplay(true);

		m_enQuestDialogType = EnQuestDialogType.Biography;

		m_csBiographyQuest = csBiographyQuest;

		m_textHeroName.text = CsGameData.Instance.MyHeroInfo.Name;

		foreach (var dialog in csBiographyQuest.BiographyQuestStartDialogueList)
		{
			m_listDialog.Add(new StDialog() { NpcId = dialog.Npc == null ? 0 : dialog.Npc.NpcId, Dialog = dialog.Dialogue });
		}

		UpdateDialog();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMainQuestNpcDialog()
	{

		InitializeButtons();

		m_enQuestDialogType = EnQuestDialogType.MainQuest;

		m_textHeroName.text = CsGameData.Instance.MyHeroInfo.Name;

		m_listDialog.Clear();

		if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None && CsMainQuestManager.Instance.MainQuest.StartNpc != null)
		{
			if (CsMainQuestManager.Instance.MainQuest.MainQuestStartDialogueList.Count > 0)
			{
				foreach (CsMainQuestStartDialogue csMainQuestStartDialog in CsMainQuestManager.Instance.MainQuest.MainQuestStartDialogueList)
				{
					m_listDialog.Add(new StDialog() { NpcId = csMainQuestStartDialog.NpcId, Dialog = csMainQuestStartDialog.Dialogue });
				}

				UpdateDialog();

				PanelDisplay(true);
			}
		}
		else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed && CsMainQuestManager.Instance.MainQuest.CompletionNpc != null)
		{
			if (CsMainQuestManager.Instance.MainQuest.MainQuestCompletionDialogueList.Count > 0)
			{
				foreach (CsMainQuestCompletionDialogue csMainQuestCompletionDialogue in CsMainQuestManager.Instance.MainQuest.MainQuestCompletionDialogueList)
				{
					m_listDialog.Add(new StDialog() { NpcId = csMainQuestCompletionDialogue.NpcId, Dialog = csMainQuestCompletionDialogue.Dialogue });
				}

				UpdateDialog();

				PanelDisplay(true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventStartTutorial()
	{
		if (m_bDialogFinished)
			return;

		PanelDisplay(false, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventReferenceTutorial(EnTutorialType enTutorialType)
	{
		if (m_bDialogFinished)
			return;

		PanelDisplay(false, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSkip()
	{
		if (m_listDialog.Count > 0)
		{
			m_listDialog.RemoveRange(0, m_listDialog.Count - 1);
		}

		UpdateDialog();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonAccept()
	{
		PanelDisplay(false);
		
		//if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.None)
		//{
		//    CsMainQuestManager.Instance.Accept();
		//}
		//else if (CsMainQuestManager.Instance.MainQuestState == EnMainQuestState.Executed)
		//{
		//    CsMainQuestManager.Instance.Complete();
		//}

		CsGameEventUIToUI.Instance.OnEventOnClickPanelDialogAccept();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCancel()
	{
		PanelDisplay(false);

		CsGameEventUIToUI.Instance.OnEventOnClickPanelDialogCancel();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonNext()
	{
		UpdateDialog();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickRewardItem(int nRewardIndex)
	{
		CsMainQuestReward csMainQuestReward = CsMainQuestManager.Instance.MainQuest.MainQuestRewardItemList[nRewardIndex];

		if (csMainQuestReward != null && csMainQuestReward.Type != (int)EnMainQuestRewardType.Mount)
		{
			if (m_goPopupItemInfo == null)
			{
				StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csMainQuestReward)));
			}
			else
			{
				OpenPopupItemInfo(csMainQuestReward);
			}
		}
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
	void OnEventHideMainUI(bool bHide)
	{
		m_bHidePanel = bHide;

		if (m_bHidePanel)
		{
			StartCoroutine(HidePanel());
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventVisibleMainUI(bool bVisible)
	{
		if (m_bDialogFinished)
			return;

		if (m_bOnEventVisibleMainUICallBySelf)
		{
			m_bOnEventVisibleMainUICallBySelf = false;
			return;
		}

		m_bHidePanel = !bVisible;

		if (m_bHidePanel)
		{
			StartCoroutine(HidePanel());
		}
	}

	//---------------------------------------------------------------------------------------------------

	#endregion event
}
