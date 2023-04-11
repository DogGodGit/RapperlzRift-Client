using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//-------------------------------------------------------------------------------------------------------
//작성: 최민수 (2018-10-02)
//-------------------------------------------------------------------------------------------------------

public class CsPopupArtifact : CsPopupSub 
{
	[SerializeField]
	GameObject m_goInventorySlot;
	[SerializeField]
	GameObject m_goItemSlot;

	GameObject m_goGeneralItemSelect;
	GameObject m_goHighItemSelect;
	GameObject m_goMagicItemSelect;
	GameObject m_goRareItemSelect;
	GameObject m_goLegendItemSelect;
	GameObject m_goPopupItemInfo;

	Transform m_trAttributeContents;
	Transform m_trInventoryContents;
	Transform m_trPopupList;
	Transform m_trItemInfo;
	Transform m_trArtifactModel;
	Transform m_trCumulativePassive;

	Image m_imageProgressExp;
	Image m_imagePreviewProgresExp;

	Button m_buttonDecomposition;
	Button m_buttonPrev;
	Button m_buttonNext;

	Text m_textArtifact;
	Text m_textProgressExp;
	Text m_textArtifactBattlePower;
	Text m_textArtifactTier;
	Text m_textDecomposition;

	Camera m_uiCamera;

	CsPopupItemInfo m_csPopupItemInfo;
	CsArtifactLevel m_csArtifactLevel;
	CsArtifact m_csArtifact;

	bool m_bIsContainGeneral;
	bool m_bIsContainHigh;
	bool m_bIsContainMagic;
	bool m_bIsContainRare;
	bool m_bIsContainLegend;
	bool m_bProcessingButton;

	int m_nSelectInventoryListIndex = -1;
	int m_nCurrentArtifactExp;
	int m_nDisplayArtifactNo;

	List<int> m_listSelectItemIndex = new List<int>();
	List<CsInventorySlot> m_listCsInventorySlot;

	//---------------------------------------------------------------------------------------------------
	void Awake () 
	{
		CsGameEventUIToUI.Instance.EventInventoryLongClick += OnEventInventoryLongClick;
		CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;

		CsArtifactManager.Instance.EventArtifactLevelUp += OnEventArtifactLevelUp;
		CsArtifactManager.Instance.EventArtifactOpened += OnEventArtifactOpened;	
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_bIsContainGeneral = false;
		m_bIsContainHigh = false;
		m_bIsContainMagic = false;
		m_bIsContainRare = false;
		m_bIsContainLegend = false;
		m_bProcessingButton = false;

		m_nCurrentArtifactExp = CsArtifactManager.Instance.ArtifactExp;

		Transform Canvas2 = GameObject.Find("Canvas2").transform;
		m_trPopupList = Canvas2.Find("PopupList");

		m_uiCamera = transform.Find("3DArtifact/UIArtifact_Camera").GetComponent<Camera>();

		m_csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);
		m_csArtifactLevel = m_csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel);
		m_nDisplayArtifactNo = CsArtifactManager.Instance.ArtifactNo;

		m_textArtifact = transform.Find("TextArtifact").GetComponent<Text>();
		CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), CsArtifactManager.Instance.ArtifactLevel, m_csArtifact.Name), false);

		m_textArtifactBattlePower = transform.Find("TextBattlePower").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textArtifactBattlePower);
		m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(CsArtifactManager.Instance.ArtifactNo).ToString("#,##0"));

		m_buttonPrev = transform.Find("ButtonPrev").GetComponent<Button>();
		CsUIData.Instance.SetButton(m_buttonPrev.transform, OnClickPrevArtifact);

		if (m_nDisplayArtifactNo == 1)
			m_buttonPrev.gameObject.SetActive(false);

		m_buttonNext = transform.Find("ButtonNext").GetComponent<Button>();
		CsUIData.Instance.SetButton(m_buttonNext.transform, OnClickNextArtifact);

		if (m_nDisplayArtifactNo == CsGameData.Instance.ArtifactList.Count)
			m_buttonNext.gameObject.SetActive(false);

		Transform trProgress = transform.Find("ProgressBar");

		m_imagePreviewProgresExp = trProgress.Find("PreviewProgress").GetComponent<Image>();
		m_imageProgressExp = trProgress.Find("Progress").GetComponent<Image>();

		m_textProgressExp = trProgress.Find("TextProgress").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textProgressExp);

		if (m_csArtifactLevel.NextLevelUpRequiredExp == 0)
		{
			m_imagePreviewProgresExp.fillAmount = 0.0f;
			m_imageProgressExp.fillAmount = 1.0f;
			m_textProgressExp.text = string.Format("{0}/{1}", 0, 0);
		}
		else
		{
			m_imagePreviewProgresExp.fillAmount = CsArtifactManager.Instance.ArtifactExp / m_csArtifactLevel.NextLevelUpRequiredExp;
			m_imageProgressExp.fillAmount = CsArtifactManager.Instance.ArtifactExp / m_csArtifactLevel.NextLevelUpRequiredExp;
			m_textProgressExp.text = string.Format("{0}/{1}", CsArtifactManager.Instance.ArtifactExp, m_csArtifactLevel.NextLevelUpRequiredExp);
		}
		
		Transform trAttribute = transform.Find("Attribute");

		m_trAttributeContents = trAttribute.Find("Scroll View/Viewport/Content");

		InitializeAttr(CsArtifactManager.Instance.ArtifactNo, CsArtifactManager.Instance.ArtifactLevel);

		Transform trInventory = transform.Find("Inventory");

		m_trInventoryContents = trInventory.Find("Scroll View/Viewport/Content");

		Text textInfoDescription = trInventory.Find("Bottom/TextDesc").GetComponent<Text>();
		CsUIData.Instance.SetFont(textInfoDescription);
		textInfoDescription.text = CsConfiguration.Instance.GetString("A157_TXT_00004");

		CsUIData.Instance.SetButton(transform.Find("ButtonList/ButtonNormal"), OnClickGeneralItem);
		CsUIData.Instance.SetText(transform.Find("ButtonList/ButtonNormal/Text"), "IGRADE_NAME_1", true);

		m_goGeneralItemSelect = transform.Find("ButtonList/ButtonNormal/ImageSelect").gameObject;
		m_goGeneralItemSelect.gameObject.SetActive(false);

		CsUIData.Instance.SetButton(transform.Find("ButtonList/ButtonHigh"), OnClickHighItem);
		CsUIData.Instance.SetText(transform.Find("ButtonList/ButtonHigh/Text"), "IGRADE_NAME_2", true);

		m_goHighItemSelect = transform.Find("ButtonList/ButtonHigh/ImageSelect").gameObject;
		m_goHighItemSelect.gameObject.SetActive(false);

		CsUIData.Instance.SetButton(transform.Find("ButtonList/ButtonMagic"), OnClickMagicItem);
		CsUIData.Instance.SetText(transform.Find("ButtonList/ButtonMagic/Text"), "IGRADE_NAME_3", true);

		m_goMagicItemSelect = transform.Find("ButtonList/ButtonMagic/ImageSelect").gameObject;
		m_goMagicItemSelect.SetActive(false);

		CsUIData.Instance.SetButton(transform.Find("ButtonList/ButtonRare"), OnClickRareItem);
		CsUIData.Instance.SetText(transform.Find("ButtonList/ButtonRare/Text"), "IGRADE_NAME_4", true);

		m_goRareItemSelect = transform.Find("ButtonList/ButtonRare/ImageSelect").gameObject;
		m_goRareItemSelect.SetActive(false);

		CsUIData.Instance.SetButton(transform.Find("ButtonList/ButtonLegend"), OnClickLegendItem);
		CsUIData.Instance.SetText(transform.Find("ButtonList/ButtonLegend/Text"), "IGRADE_NAME_5", true);

		m_goLegendItemSelect = transform.Find("ButtonList/ButtonLegend/ImageSelect").gameObject;
		m_goLegendItemSelect.SetActive(false);

		m_textArtifactTier = transform.Find("ArtifactInfo/TextTier").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textArtifactTier);
		m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), CsArtifactManager.Instance.ArtifactNo);

		CsUIData.Instance.SetButton(transform.Find("ArtifactInfo/ButtonBonusAttr"), () => OnClickCumulativePassive(true));
		CsUIData.Instance.SetText(transform.Find("ArtifactInfo/ButtonBonusAttr/Text"), CsConfiguration.Instance.GetString("A157_TXT_00007"), true);

		m_buttonDecomposition = transform.Find("ButtonDecomposition").GetComponent<Button>();
		CsUIData.Instance.SetButton(m_buttonDecomposition.transform, OnClickDecomposition);
		CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);

		m_textDecomposition = m_buttonDecomposition.GetComponentInChildren<Text>();
		CsUIData.Instance.SetFont(m_textDecomposition);
		m_textDecomposition.text = CsConfiguration.Instance.GetString("A157_BTN_00001");

		m_trCumulativePassive = transform.Find("CumulativePassive");

		InitializeCumulative();

		m_listCsInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindAll(a => a.EnType == EnInventoryObjectType.MainGear);

		InitializeInventory();

		LoadArtifactModel();
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeCumulative()
	{
		CsUIData.Instance.SetButton(m_trCumulativePassive, () => OnClickCumulativePassive(false));

		CsUIData.Instance.SetText(m_trCumulativePassive.Find("ImageBackground/TextBonusAttr"), CsConfiguration.Instance.GetString("A157_TXT_00008"), true);

		Transform trArtifactBonusAttr = m_trCumulativePassive.Find("ImageBackground/ArtifactBonusAttr");

		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);

		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("SafeRevivalAdditionalHpRecoveryRate/TextAttr"), CsConfiguration.Instance.GetString("A157_TXT_00009"), true);
		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("SafeRevivalAdditionalHpRecoveryRate/TextValue"), csArtifact.SafeRevivalAdditionalHpRecoveryRate.ToString(), false);

		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("FreeImmediateRevivalAdditionalDailyCount/TextAttr"), CsConfiguration.Instance.GetString("A157_TXT_00010"), true);
		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("FreeImmediateRevivalAdditionalDailyCount/TextValue"), csArtifact.FreeImmediateRevivalAdditionalDailyCount.ToString(), false);

		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("SafeRevivalWaitingDecRate/TextAttr"), CsConfiguration.Instance.GetString("A157_TXT_00011"), true);
		CsUIData.Instance.SetText(trArtifactBonusAttr.Find("SafeRevivalWaitingDecRate/TextValue"), csArtifact.SafeRevivalWaitingDecRate.ToString(), false);
	}

	//---------------------------------------------------------------------------------------------------
	void LoadArtifactModel()
	{
		m_trArtifactModel = transform.Find("3DArtifact/Artifact" + m_nDisplayArtifactNo);

		if (m_trArtifactModel == null)
		{
			StartCoroutine(LoadArtifactModelCoroutine());
		}
		else
		{
			m_trArtifactModel.gameObject.SetActive(true);
		}
		
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadArtifactModelCoroutine()
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/ArteffectObject/10" + m_nDisplayArtifactNo);
		yield return resourceRequest;
		GameObject goArtifact = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DArtifact"));

		goArtifact.name = "Artifact" + m_nDisplayArtifactNo;
		goArtifact.transform.localPosition = new Vector3(0, -30, 0);
		ChangeLayers(goArtifact, LayerMask.NameToLayer("UIChar"));
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeAttr(int nArtifactNo, int nLevel)
	{
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(nArtifactNo);
		CsArtifactLevel csArtifactLevel = csArtifact.GetArtifactLevel(nLevel);
		List<CsArtifactLevelAttr> listCsArtifactLevelAttr = csArtifactLevel.ArtifactLevelAttrList;

		foreach (Transform trChild in m_trAttributeContents)
		{
			Destroy(trChild.gameObject);
		}

		for (int i = 0; i < listCsArtifactLevelAttr.Count; i++)
		{
			GameObject goAttr = Instantiate(Resources.Load<GameObject>("GUI/PopupArtifact/ArtifactAttr"), m_trAttributeContents);

			CsUIData.Instance.SetText(goAttr.transform.Find("TextName"), listCsArtifactLevelAttr[i].Attr.Name, false);

			CsUIData.Instance.SetText(goAttr.transform.Find("Attr/TextCurrentAttr"), listCsArtifactLevelAttr[i].AttrValue.Value.ToString(), false);

			Text textBonusAttr = goAttr.transform.Find("Attr/TextBonusAttr").GetComponent<Text>();
			CsUIData.Instance.SetFont(textBonusAttr);

			CsArtifactLevelAttr csArtifactLevelAttr = m_csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel).ArtifactLevelAttrList[i];

			if (csArtifactLevelAttr != null)
				textBonusAttr.text = csArtifactLevelAttr.AttrValue.Value.ToString();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeInventory()
	{
		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			CreateInventorySlot(i);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 슬롯생성함수
	void CreateInventorySlot(int nSlotIndex)
	{
		Transform trSlot = m_trInventoryContents.Find(m_listCsInventorySlot[nSlotIndex].Index.ToString());

		if (trSlot == null)
		{
			GameObject goInventorySlot = Instantiate(m_goInventorySlot, m_trInventoryContents);
			goInventorySlot.name = m_listCsInventorySlot[nSlotIndex].Index.ToString();
			trSlot = goInventorySlot.transform;
			
			Button buttonInventorySlot = trSlot.GetComponent<Button>();
			buttonInventorySlot.onClick.RemoveAllListeners();
			buttonInventorySlot.onClick.AddListener(() => OnClickInventorySlot(nSlotIndex));
			buttonInventorySlot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}

		UpdateInventorySlot(nSlotIndex);
	}

	//---------------------------------------------------------------------------------------------------
	// 슬롯업데이트 함수
	void UpdateInventorySlot(int nSlotIndex)
	{
		Transform trInventorySlot = m_trInventoryContents.Find(m_listCsInventorySlot[nSlotIndex].Index.ToString());

		if (trInventorySlot == null)
		{
			return;
		}

		Transform TrCheck = trInventorySlot.Find("ImageCheck");

		if (TrCheck.gameObject.activeSelf)
		{
			TrCheck.gameObject.SetActive(false);
		}

		//아이템이 있는 슬롯
		Transform trItemSlot = trInventorySlot.Find("ItemSlot");

		if (trItemSlot == null)
		{
			GameObject goItemSlot = Instantiate(m_goItemSlot, trInventorySlot);
			goItemSlot.name = "ItemSlot";
			trItemSlot = goItemSlot.transform;

			goItemSlot.GetComponent<Button>().enabled = false;
		}
		else
		{
			trItemSlot.gameObject.SetActive(true);
		}
	
		CsHeroMainGear csHeroMainGear = m_listCsInventorySlot[nSlotIndex].InventoryObjectMainGear.HeroMainGear;
		CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGear);	
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateInventorySlotSelected(CsInventorySlot csInventorySlot, bool bSelect)
	{
		if (m_csArtifactLevel.NextLevelUpRequiredExp == 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A157_ERROR_00102"));
			return;
		}

		if (m_listSelectItemIndex.Count == 0)
		{
			CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);
		}
		else
		{
			CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, true);
		}
			
		Transform trInventorySlot = m_trInventoryContents.Find(csInventorySlot.Index.ToString());

		if (trInventorySlot != null)
		{
			Transform trItemSlot = trInventorySlot.Find("ItemSlot");
			Transform trCheck = trItemSlot.Find("ImageCheck");

			Button buttonInventorySlot = trInventorySlot.GetComponent<Button>();

			CsArtifactLevelUpMaterial csArtifactLevelUpMaterial = CsGameData.Instance.GetArtifactLevelUpMaterial(csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearTier.Tier, csInventorySlot.InventoryObjectMainGear.HeroMainGear.MainGear.MainGearGrade.Grade);
			int nItemExp = 0;

			if (csArtifactLevelUpMaterial != null)
				nItemExp = csArtifactLevelUpMaterial.Exp;

			if (bSelect)
			{
				trCheck.gameObject.SetActive(true);
				m_nCurrentArtifactExp += nItemExp;
			}
			else
			{
				trCheck.gameObject.SetActive(false);				
				m_nCurrentArtifactExp -= nItemExp;
			}

			m_textProgressExp.text = string.Format("{0}/{1}", m_nCurrentArtifactExp, m_csArtifactLevel.NextLevelUpRequiredExp);
			m_imagePreviewProgresExp.fillAmount = m_nCurrentArtifactExp / m_csArtifactLevel.NextLevelUpRequiredExp;
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupItemInfo(CsItem csitem = null, bool bOwned = false)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
		yield return resourceRequest;
		m_goPopupItemInfo = (GameObject)resourceRequest.asset;

		if (csitem == null)
		{
			OpenPopupItemInfo();
		}
		else
		{
			OpenPopupItemInfo(csitem, bOwned);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo()
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
		m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Right, m_listCsInventorySlot.Find(a => a.Index == m_nSelectInventoryListIndex).InventoryObjectMainGear.HeroMainGear, false);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupItemInfo(CsItem csitem, bool bOwned)
	{
		GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
		m_trItemInfo = goPopupItemInfo.transform;
		m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

		m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
		m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csitem, 0, bOwned, m_nSelectInventoryListIndex);
	}

	//---------------------------------------------------------------------------------------------------
	void MultiSelectInventoryItem(bool bIsSelect, EnMainGearGrade enMainGearGrade)
	{
		m_listCsInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindAll(a => a.EnType == EnInventoryObjectType.MainGear);

		for (int i = 0; i < m_listCsInventorySlot.Count; i++)
		{
			if (m_listCsInventorySlot[i].InventoryObjectMainGear.HeroMainGear.MainGear.MainGearGrade.EnGrade == enMainGearGrade)
			{
				if (bIsSelect)
				{
					if (CsArtifactManager.Instance.GetRequireExpForCurrentLevelToMaxLevel() < m_nCurrentArtifactExp)
					{
						CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A157_TXT_03001"));
						break;
					}
				}
				
				CsArtifactInventorySlot csArtifactInventorySlot = m_trInventoryContents.Find(m_listCsInventorySlot[i].Index.ToString()).GetComponent<CsArtifactInventorySlot>();
				csArtifactInventorySlot.IsSelected = bIsSelect;

				int nSelectInventoryIndex = m_listCsInventorySlot[i].Index;

				if (bIsSelect)
				{
					if (!m_listSelectItemIndex.Contains(nSelectInventoryIndex))
					{
						m_listSelectItemIndex.Add(nSelectInventoryIndex);
						UpdateInventorySlotSelected(m_listCsInventorySlot[i], bIsSelect);
					}
				}
				else
				{
					if (m_listSelectItemIndex.Contains(nSelectInventoryIndex))
					{
						m_listSelectItemIndex.Remove(nSelectInventoryIndex);
						UpdateInventorySlotSelected(m_listCsInventorySlot[i], bIsSelect);
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ModifySelectItemList(bool bIsSelected, int nInventorySlotIndex)
	{
		if (bIsSelected)
		{
			if (m_listSelectItemIndex.Contains(nInventorySlotIndex))
			{
				m_listSelectItemIndex.Remove(nInventorySlotIndex);
			}
		}
		else
		{
			if (!m_listSelectItemIndex.Contains(nInventorySlotIndex))
			{
				m_listSelectItemIndex.Add(nInventorySlotIndex);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SwitchMainGearMultiSelect(GameObject goSelectButton, ref bool bIsContainItem)
	{
		if (m_csArtifactLevel.NextLevelUpRequiredExp == 0)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A157_ERROR_00102"));
		}

		if (bIsContainItem)
		{
			goSelectButton.SetActive(false);
			bIsContainItem = false;
		}
		else
		{
			goSelectButton.SetActive(true);
			bIsContainItem = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeLayers(GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		foreach (Transform trChild in gameObject.transform)
		{
			ChangeLayers(trChild.gameObject, layer);
		}
	}

	#region EventHandler

	//---------------------------------------------------------------------------------------------------
	void OnEventCloseAllPopup()
	{
		Destroy(this.gameObject);
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
	void OnEventInventoryLongClick(int nInventorySlotIndex)
	{
		m_nSelectInventoryListIndex = nInventorySlotIndex;

		//아이템정보창 보여주기
		if (m_goPopupItemInfo == null)
		{
			StartCoroutine(LoadPopupItemInfo());
		}
		else
		{
			OpenPopupItemInfo();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactLevelUp()
	{
		CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);

		m_listCsInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindAll(a => a.EnType == EnInventoryObjectType.MainGear);

		m_goGeneralItemSelect.SetActive(false);
		m_goHighItemSelect.SetActive(false);
		m_goMagicItemSelect.SetActive(false);
		m_goRareItemSelect.SetActive(false);
		m_goLegendItemSelect.SetActive(false);

		m_bIsContainGeneral = false;
		m_bIsContainHigh = false;
		m_bIsContainMagic = false;
		m_bIsContainRare = false;
		m_bIsContainLegend = false;

		InitializeInventory();

		m_nSelectInventoryListIndex = -1;
		m_listSelectItemIndex.Clear();

		m_csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);
		m_csArtifactLevel = m_csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel);

		m_textArtifact.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), CsArtifactManager.Instance.ArtifactLevel, m_csArtifact.Name);

		m_nCurrentArtifactExp = CsArtifactManager.Instance.ArtifactExp;

		m_imagePreviewProgresExp.fillAmount = 0.0f;
		if (m_csArtifactLevel.NextLevelUpRequiredExp == 0)
		{
			m_imageProgressExp.fillAmount = 1.0f;
			m_textProgressExp.text = string.Format("{0}/{1}", 0, 0);
		}
		else
		{
			m_imageProgressExp.fillAmount = CsArtifactManager.Instance.ArtifactExp / m_csArtifactLevel.NextLevelUpRequiredExp;
			m_textProgressExp.text = string.Format("{0}/{1}", CsArtifactManager.Instance.ArtifactExp, m_csArtifactLevel.NextLevelUpRequiredExp);
		}

		m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), CsArtifactManager.Instance.ArtifactNo);

		m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(CsArtifactManager.Instance.EquippedArtifactNo));

		InitializeAttr(CsArtifactManager.Instance.ArtifactNo, CsArtifactManager.Instance.ArtifactLevel);
		m_nDisplayArtifactNo = CsArtifactManager.Instance.ArtifactNo;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactOpened()
	{
		if (CsArtifactManager.Instance.ArtifactNo == 0)
		{
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00201"));
		}
		else
		{
			CsArtifactManager.Instance.SendArtifactEquip(CsArtifactManager.Instance.ArtifactNo);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickInventorySlot(int nSlotIndex)
	{
		if (m_bProcessingButton)
		{
			return;
		}

		m_bProcessingButton = true;

		// 인벤토리 슬롯이 눌렸을때 체크박스 설정 및 아이템 인덱스 저장 / 이미 있을 경우 인덱스에서 제외.
		CsArtifactInventorySlot csArtifactInventorySlot = m_trInventoryContents.Find(m_listCsInventorySlot[nSlotIndex].Index.ToString()).GetComponent<CsArtifactInventorySlot>();

		bool bIsSelected = csArtifactInventorySlot.IsSelected;

		if (!bIsSelected)
		{
			if (CsArtifactManager.Instance.GetRequireExpForCurrentLevelToMaxLevel() < m_nCurrentArtifactExp)
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A157_TXT_03001"));
				m_bProcessingButton = false;
				return;
			}
		}

		ModifySelectItemList(bIsSelected, m_listCsInventorySlot[nSlotIndex].Index);
		csArtifactInventorySlot.IsSelected = !bIsSelected;

		UpdateInventorySlotSelected(m_listCsInventorySlot[nSlotIndex], csArtifactInventorySlot.IsSelected);

		m_bProcessingButton = false;
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickPrevArtifact()
	{
		if (!m_buttonNext.IsActive())
			m_buttonNext.gameObject.SetActive(true);

		if (m_nDisplayArtifactNo > 1)
		{
			m_trArtifactModel = transform.Find("3DArtifact/Artifact" + m_nDisplayArtifactNo);

			if (m_trArtifactModel != null)
				m_trArtifactModel.gameObject.SetActive(false);	

			if (m_nDisplayArtifactNo == 2)
				m_buttonPrev.gameObject.SetActive(false);

			m_nDisplayArtifactNo--;

			if (m_nDisplayArtifactNo == CsArtifactManager.Instance.ArtifactNo)
			{
				CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), CsArtifactManager.Instance.ArtifactLevel, m_csArtifact.Name), false);
				m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(CsArtifactManager.Instance.ArtifactNo));
				m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), CsArtifactManager.Instance.ArtifactNo);
				InitializeAttr(CsArtifactManager.Instance.ArtifactNo, CsArtifactManager.Instance.ArtifactLevel);
				CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, true);
			}
			else
			{
				CsArtifact csArtifact = CsGameData.Instance.GetArtifact(m_nDisplayArtifactNo);
				CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1].Level, csArtifact.Name), false);
				m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(m_nDisplayArtifactNo));
				m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), m_nDisplayArtifactNo);
				InitializeAttr(csArtifact.ArtifactNo, csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1].Level);
				CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);
			}
			
			m_textDecomposition.text = CsConfiguration.Instance.GetString("A157_BTN_00001");

			LoadArtifactModel();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickNextArtifact()
	{
		if (!m_buttonPrev.IsActive())
			m_buttonPrev.gameObject.SetActive(true);

		if (m_nDisplayArtifactNo < CsGameData.Instance.ArtifactList.Count)
		{
			if (m_nDisplayArtifactNo == CsArtifactManager.Instance.ArtifactNo + 1)
				return;

			m_trArtifactModel = transform.Find("3DArtifact/Artifact" + m_nDisplayArtifactNo);

			if (m_trArtifactModel != null)
				m_trArtifactModel.gameObject.SetActive(false);	

			if (m_nDisplayArtifactNo == CsGameData.Instance.ArtifactList.Count - 1)
				m_buttonNext.gameObject.SetActive(false);
			
			m_nDisplayArtifactNo++;

			if (m_nDisplayArtifactNo == CsArtifactManager.Instance.ArtifactNo)
			{
				CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), CsArtifactManager.Instance.ArtifactLevel, m_csArtifact.Name), false);
				m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(CsArtifactManager.Instance.ArtifactNo));
				m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), CsArtifactManager.Instance.ArtifactNo);
				InitializeAttr(CsArtifactManager.Instance.ArtifactNo, CsArtifactManager.Instance.ArtifactLevel);

				CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, true);
				m_textDecomposition.text = CsConfiguration.Instance.GetString("A157_BTN_00001");
			}
			else if (m_nDisplayArtifactNo == CsArtifactManager.Instance.ArtifactNo + 1)
			{
				CsArtifact csArtifact = CsGameData.Instance.GetArtifact(m_nDisplayArtifactNo);
				CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), csArtifact.ArtifactLevelList[0].Level, csArtifact.Name), false);
				m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(m_nDisplayArtifactNo));
				m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), m_nDisplayArtifactNo);
				InitializeAttr(csArtifact.ArtifactNo, csArtifact.ArtifactLevelList[0].Level);

				CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);
				m_textDecomposition.text = CsConfiguration.Instance.GetString("A157_BTN_00002");
			}
			else
			{
				CsArtifact csArtifact = CsGameData.Instance.GetArtifact(m_nDisplayArtifactNo);
				CsUIData.Instance.SetText(m_textArtifact.transform, string.Format(CsConfiguration.Instance.GetString("A157_TXT_00001"), csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1].Level, csArtifact.Name), false);
				m_textArtifactBattlePower.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00002"), CsArtifactManager.Instance.GetBattlePower(m_nDisplayArtifactNo));
				m_textArtifactTier.text = string.Format(CsConfiguration.Instance.GetString("A157_TXT_00003"), m_nDisplayArtifactNo);
				InitializeAttr(csArtifact.ArtifactNo, csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1].Level);

				CsUIData.Instance.DisplayButtonInteractable(m_buttonDecomposition, false);
				m_textDecomposition.text = CsConfiguration.Instance.GetString("A157_BTN_00001");
			}
		}

		LoadArtifactModel();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickGeneralItem()
	{
		SwitchMainGearMultiSelect(m_goGeneralItemSelect, ref m_bIsContainGeneral);

		MultiSelectInventoryItem(m_bIsContainGeneral, EnMainGearGrade.General);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickHighItem()
	{
		SwitchMainGearMultiSelect(m_goHighItemSelect, ref m_bIsContainHigh);

		MultiSelectInventoryItem(m_bIsContainHigh, EnMainGearGrade.High);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickMagicItem()
	{
		SwitchMainGearMultiSelect(m_goMagicItemSelect, ref m_bIsContainMagic);

		MultiSelectInventoryItem(m_bIsContainMagic, EnMainGearGrade.Magic);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickRareItem()
	{
		SwitchMainGearMultiSelect(m_goRareItemSelect, ref m_bIsContainRare);

		MultiSelectInventoryItem(m_bIsContainRare, EnMainGearGrade.Rare);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickLegendItem()
	{
		SwitchMainGearMultiSelect(m_goLegendItemSelect, ref m_bIsContainLegend);

		MultiSelectInventoryItem(m_bIsContainLegend, EnMainGearGrade.Legend);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickDecomposition()
	{
		CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsArtifactManager.Instance.ArtifactNo);

		if (CsArtifactManager.Instance.ArtifactNo == 0)
		{
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00101"));
		}
		else if (csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel).NextLevelUpRequiredExp == 0)
		{
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_TXT_03001"));
		}
		else if (m_listSelectItemIndex.Count == 0)
		{
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00103"));
		}
		else
		{
			Guid[] aGuidMainGear = new Guid[m_listSelectItemIndex.Count];

			for (int i = 0; i < m_listSelectItemIndex.Count; i++)
			{
				aGuidMainGear[i] = m_listCsInventorySlot.Find(a => a.Index == m_listSelectItemIndex[i]).InventoryObjectMainGear.HeroMainGear.Id;
			}

			CsArtifactManager.Instance.SendArtifactLevelUp(aGuidMainGear);

			foreach (Transform trChild in m_trInventoryContents)
			{
				Destroy(trChild.gameObject);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickCumulativePassive(bool bIson)
	{
		m_trCumulativePassive.gameObject.SetActive(bIson);
	}

	#endregion EventHandler

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventInventoryLongClick -= OnEventInventoryLongClick;
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;

		CsArtifactManager.Instance.EventArtifactLevelUp -= OnEventArtifactLevelUp;
		CsArtifactManager.Instance.EventArtifactOpened -= OnEventArtifactOpened;

		if (m_trItemInfo != null)
		{
			Destroy(m_trItemInfo.gameObject);
			m_csPopupItemInfo = null;
			m_trItemInfo = null;
		}
	}
}
