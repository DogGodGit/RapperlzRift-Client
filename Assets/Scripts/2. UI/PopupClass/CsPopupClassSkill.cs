using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Linq;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-07-03)
//---------------------------------------------------------------------------------------------------

public class CsPopupClassSkill : CsPopupSub
{
	CsRankActiveSkill m_csRankActiveSkill = null;
	CsRankPassiveSkill m_csRankPassiveSkill = null;

	Text m_textMySpiritStone;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsGameEventUIToUI.Instance.EventRankActiveSkillLevelUp += OnEventRankActiveSkillLevelUp;
		CsGameEventUIToUI.Instance.EventRankActiveSkillSelect += OnEventRankActiveSkillSelect;
		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp += OnEventEventRankPassiveSkillLevelUp;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		// Start
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsGameEventUIToUI.Instance.EventRankActiveSkillLevelUp -= OnEventRankActiveSkillLevelUp;
		CsGameEventUIToUI.Instance.EventRankActiveSkillSelect -= OnEventRankActiveSkillSelect;
		CsGameEventUIToUI.Instance.EventRankPassiveSkillLevelUp -= OnEventEventRankPassiveSkillLevelUp;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		UpdateActiveSkillList();
		UpdatePassiveSkillList();
		SelectFirstToggle();
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		Text textActiveSkillName = transform.Find("FrameActiveSkill/ImageFrameName/TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textActiveSkillName);
		textActiveSkillName.text = CsConfiguration.Instance.GetString("A28_TXT_00001");

		Text textPassiveSkillName = transform.Find("FramePassiveSkill/ImageFrameName/TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textPassiveSkillName);
		textPassiveSkillName.text = CsConfiguration.Instance.GetString("A28_TXT_00002");

		// 스킬 정보창
		Transform trFrameSkillDetail = transform.Find("FrameSkillDetail");

		// 영혼석
		m_textMySpiritStone = trFrameSkillDetail.Find("FrameSkill/FrameSpiritStone/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textMySpiritStone);

		// 현재 효과
		Text textCurrentEffect = trFrameSkillDetail.Find("FrameActiveSkillEffect/TextNameCurrentEffect").GetComponent<Text>();
		CsUIData.Instance.SetFont(textCurrentEffect);
		textCurrentEffect.text = CsConfiguration.Instance.GetString("A28_TXT_00003");

		// 레벨업 효과
		Text textNextEffect = trFrameSkillDetail.Find("FrameActiveSkillEffect/TextNameNextEffect").GetComponent<Text>();
		CsUIData.Instance.SetFont(textNextEffect);
		textNextEffect.text = CsConfiguration.Instance.GetString("A28_TXT_00004");

		// 레벨업 필요 조건
		Text textNameLevelUpRequired = trFrameSkillDetail.Find("TextNameLevelUpRequired").GetComponent<Text>();
		CsUIData.Instance.SetFont(textNameLevelUpRequired);
		textNameLevelUpRequired.text = CsConfiguration.Instance.GetString("A28_TXT_00005");

		Text textMaxLevel = trFrameSkillDetail.Find("TextMaxLevel").GetComponent<Text>();
		CsUIData.Instance.SetFont(textMaxLevel);
		textMaxLevel.text = CsConfiguration.Instance.GetString("A28_TXT_00007");

		// 액티브 스킬 레벨업 버튼
		Transform trButtonLevelUpActiveSkill = transform.Find("FrameButtonActiveSkill/ButtonLevelUp");
		
		Text textButtonLevelUpActiveSkill = trButtonLevelUpActiveSkill.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonLevelUpActiveSkill);
		textButtonLevelUpActiveSkill.text = CsConfiguration.Instance.GetString("A28_BTN_00001");

		Button buttonLevelUpActiveSkill = trButtonLevelUpActiveSkill.GetComponent<Button>();
		buttonLevelUpActiveSkill.onClick.RemoveAllListeners();
		buttonLevelUpActiveSkill.onClick.AddListener(OnClickButtonActiveSkillLevelUp);
		buttonLevelUpActiveSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		// 액티브 스킬 사용 버튼
		Transform trButtonUseActiveSkill = transform.Find("FrameButtonActiveSkill/ButtonUseSkill");

		Text textButtonUseActiveSkill = trButtonUseActiveSkill.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonUseActiveSkill);
		textButtonUseActiveSkill.text = CsConfiguration.Instance.GetString("A28_BTN_00002");

		Button buttonUseActiveSkill = trButtonUseActiveSkill.GetComponent<Button>();
		buttonUseActiveSkill.onClick.RemoveAllListeners();
		buttonUseActiveSkill.onClick.AddListener(OnClickButtonUseActiveSkill);
		buttonLevelUpActiveSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		// 패시브 스킬 레벨업 버튼
		Transform trButtonLevelUpPassiveSkill = transform.Find("FrameButtonPassiveSkill/ButtonLevelUp");

		Text textButtonLevelUpPassiveSkill = trButtonLevelUpPassiveSkill.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonLevelUpPassiveSkill);
		textButtonLevelUpPassiveSkill.text = CsConfiguration.Instance.GetString("A28_BTN_00003");

		Button buttonLevelUpPassiveSkill = trButtonLevelUpPassiveSkill.GetComponent<Button>();
		buttonLevelUpPassiveSkill.onClick.RemoveAllListeners();
		buttonLevelUpPassiveSkill.onClick.AddListener(OnClickButtonPassiveLevelUp);
		buttonLevelUpActiveSkill.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		UpdateSpiritStoneCount();
		UpdateActiveSkillList();
		UpdatePassiveSkillList();
		SelectFirstToggle();
	}

	//---------------------------------------------------------------------------------------------------
	void SelectFirstToggle()
	{
		Toggle toggle = transform.Find("FrameActiveSkill/ImageFrameScrollView/Scroll View/Viewport/Content").GetChild(0).GetComponent<Toggle>();
		toggle.isOn = true;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateSpiritStoneCount()
	{
		m_textMySpiritStone.text = CsGameData.Instance.MyHeroInfo.SpiritStone.ToString("#,##0");
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateActiveSkillList()
	{
		ToggleGroup toggleGroup = transform.GetComponent<ToggleGroup>();
		GameObject goToggleClassSkill = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupClass/ToggleClassSkill");

		Transform trContentActiveSkill = transform.Find("FrameActiveSkill/ImageFrameScrollView/Scroll View/Viewport/Content");

		foreach (var csRankActiveSkill in CsGameData.Instance.RankActiveSkillList)
		{
			string strName = new StringBuilder("ActiveSkill").Append(csRankActiveSkill.SkillId).ToString();
			Transform trToggleClassSkill = trContentActiveSkill.Find(strName);

			if (trToggleClassSkill == null)
			{
				trToggleClassSkill = Instantiate(goToggleClassSkill).transform;
				RectTransform rtrToggleClassSkill = trToggleClassSkill.GetComponent<RectTransform>();

				trToggleClassSkill.name = strName;
				trToggleClassSkill.SetParent(trContentActiveSkill);
				rtrToggleClassSkill.anchoredPosition3D = Vector3.zero;
				rtrToggleClassSkill.localScale = Vector3.one;

				Image imageSkillIcon = trToggleClassSkill.Find("ImageSkillIcon").GetComponent<Image>();
				imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/" + csRankActiveSkill.ImageName);
				
				// 토글
				Toggle toggle = trToggleClassSkill.GetComponent<Toggle>();
				toggle.group = toggleGroup;
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggleActiveSkill(csRankActiveSkill, isOn));
			}

			var csHeroRankActiveSkill = CsGameData.Instance.MyHeroInfo.HeroRankActiveSkillList.Find(skill => skill.SkillId == csRankActiveSkill.SkillId);

			if (IsOpenActiveSkill(csHeroRankActiveSkill))
			{
				Text textSkillLevel = trToggleClassSkill.Find("TextSkillLevel").GetComponent<Text>();
				CsUIData.Instance.SetFont(textSkillLevel);
				textSkillLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroRankActiveSkill.Level);
			}

			trToggleClassSkill.Find("TextSkillLevel").gameObject.SetActive(IsOpenActiveSkill(csHeroRankActiveSkill));
			trToggleClassSkill.Find("ImageLock").gameObject.SetActive(!IsOpenActiveSkill(csHeroRankActiveSkill));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdatePassiveSkillList()
	{
		ToggleGroup toggleGroup = transform.GetComponent<ToggleGroup>();
		GameObject goToggleClassSkill = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupClass/ToggleClassSkill");

		Transform trContentPassiveSkill = transform.Find("FramePassiveSkill/ImageFrameScrollView/Scroll View/Viewport/Content");

		foreach (var csRankPassiveSkill in CsGameData.Instance.RankPassiveSkillList)
		{
			string strName = new StringBuilder("PassiveSkill").Append(csRankPassiveSkill.SkillId).ToString();
			Transform trToggleClassSkill = trContentPassiveSkill.Find(strName);

			if (trToggleClassSkill == null)
			{
				trToggleClassSkill = Instantiate(goToggleClassSkill).transform;
				RectTransform rtrToggleClassSkill = trToggleClassSkill.GetComponent<RectTransform>();

				trToggleClassSkill.name = strName;
				trToggleClassSkill.SetParent(trContentPassiveSkill);
				rtrToggleClassSkill.anchoredPosition3D = Vector3.zero;
				rtrToggleClassSkill.localScale = Vector3.one;

				Image imageSkillIcon = trToggleClassSkill.Find("ImageSkillIcon").GetComponent<Image>();
				imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/" + csRankPassiveSkill.ImageName);
				
				// 토글
				Toggle toggle = trToggleClassSkill.GetComponent<Toggle>();
				toggle.group = toggleGroup;
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((isOn) => OnValueChangedTogglePassiveSkill(csRankPassiveSkill, isOn));
			}

			var csHeroRankPassiveSkill = CsGameData.Instance.MyHeroInfo.HeroRankPassiveSkillList.Find(skill => skill.SkillId == csRankPassiveSkill.SkillId);

			if (IsOpenPassiveSkill(csHeroRankPassiveSkill))
			{
				Text textSkillLevel = trToggleClassSkill.Find("TextSkillLevel").GetComponent<Text>();
				CsUIData.Instance.SetFont(textSkillLevel);
				textSkillLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroRankPassiveSkill.Level);
			}

			trToggleClassSkill.Find("TextSkillLevel").gameObject.SetActive(IsOpenPassiveSkill(csHeroRankPassiveSkill));
			trToggleClassSkill.Find("ImageLock").gameObject.SetActive(!IsOpenPassiveSkill(csHeroRankPassiveSkill));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateSkillDetail()
	{
		Transform trSkillDetail = transform.Find("FrameSkillDetail");
		Transform trFrameSkill = trSkillDetail.Find("FrameSkill");

		Image imageSkillIcon = trFrameSkill.Find("ImageFrameSkillIcon/ImageSkillIcon").GetComponent<Image>();
		Transform trImageSkillLock = trFrameSkill.Find("ImageFrameSkillIcon/ImageSkillLock");

		Text textSkillLevel = trFrameSkill.Find("TextSkillLevel").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSkillLevel);
		Text textSkillName = trFrameSkill.Find("TextSkillName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSkillName);
		Text textSkillDescription = trSkillDetail.Find("TextDescription").GetComponent<Text>();
		CsUIData.Instance.SetFont(textSkillDescription);

		Transform trFrameCoolTime = trSkillDetail.Find("FrameCoolTime");
		Transform trFrameActiveSkillEffect = trSkillDetail.Find("FrameActiveSkillEffect");
		Transform trFramePassiveSkillEffect = trSkillDetail.Find("FramePassiveSkillEffect");
		Transform trFrameLevelUpRequiredGold = trSkillDetail.Find("FrameLevelUpRequiredGold");
		Transform trFrameLevelUpRequiredItem = trSkillDetail.Find("FrameLevelUpRequiredItem");
		Transform trTextMaxLevel = trSkillDetail.Find("TextMaxLevel");

		if (m_csRankActiveSkill != null)
		{
			CsHeroRankActiveSkill csHeroRankActiveSkill = CsGameData.Instance.MyHeroInfo.GetHeroRankActiveSkill(m_csRankActiveSkill.SkillId);
			CsRankActiveSkillLevel csRankActiveSkillLevel = m_csRankActiveSkill.GetRankActiveSkillLevel(IsOpenActiveSkill(csHeroRankActiveSkill) ? csHeroRankActiveSkill.Level : 1);
			CsRankActiveSkillLevel csRankActiveSkillNextLevel = m_csRankActiveSkill.GetRankActiveSkillLevel(IsOpenActiveSkill(csHeroRankActiveSkill) ? csHeroRankActiveSkill.Level + 1 : 1);

			// 스킬 아이콘, 이름, 레벨, 설명
			imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/" + m_csRankActiveSkill.ImageName);
			trImageSkillLock.gameObject.SetActive(!IsOpenActiveSkill(csHeroRankActiveSkill));

			textSkillLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csRankActiveSkillLevel.Level);
			textSkillName.text = m_csRankActiveSkill.Name;
			textSkillDescription.gameObject.SetActive(IsOpenActiveSkill(csHeroRankActiveSkill));

			if (IsOpenActiveSkill(csHeroRankActiveSkill))
			{
				textSkillDescription.text = m_csRankActiveSkill.Description;
			}

			// 쿨타임 & 잠금해제 조건
			trFrameCoolTime.Find("ImageCoolTimeIcon").gameObject.SetActive(IsOpenActiveSkill(csHeroRankActiveSkill));
			trFrameCoolTime.Find("ImageLockIcon").gameObject.SetActive(!IsOpenActiveSkill(csHeroRankActiveSkill));
			trFrameCoolTime.Find("TextCoolTime").gameObject.SetActive(true);
			trSkillDetail.Find("Blank").gameObject.SetActive(!IsOpenActiveSkill(csHeroRankActiveSkill));

			Text textCoolTime = trFrameCoolTime.Find("TextCoolTime").GetComponent<Text>();

			if (IsOpenActiveSkill(csHeroRankActiveSkill))
			{
				textCoolTime.text = string.Format(CsConfiguration.Instance.GetString("A28_TXT_01001"), m_csRankActiveSkill.CoolTime);
			}
			else
			{
				CsRank csRank = CsGameData.Instance.GetRank(m_csRankActiveSkill.RequiredRankNo);

				if (csRank != null)
				{
					textCoolTime.text = string.Format(CsConfiguration.Instance.GetString("A28_TXT_01002"), csRank.Name);
				}
			}

			//효과
			trFrameActiveSkillEffect.gameObject.SetActive(true);
			trFramePassiveSkillEffect.gameObject.SetActive(false);

			trFrameActiveSkillEffect.Find("TextNameNextEffect").gameObject.SetActive(!IsMaxLevelActiveSkill(csRankActiveSkillNextLevel));
			trFrameActiveSkillEffect.Find("TextNextEffect").gameObject.SetActive(!IsMaxLevelActiveSkill(csRankActiveSkillNextLevel));

			Text textCurrentEffect = trFrameActiveSkillEffect.Find("TextCurrentEffect").GetComponent<Text>();
			CsUIData.Instance.SetFont(textCurrentEffect);
			textCurrentEffect.text = csRankActiveSkillLevel.EffectDescription;

			Text textNextEffect = trFrameActiveSkillEffect.Find("TextNextEffect").GetComponent<Text>();
			CsUIData.Instance.SetFont(textNextEffect);
			textNextEffect.text = IsMaxLevelActiveSkill(csRankActiveSkillNextLevel) ? "" : csRankActiveSkillNextLevel.EffectDescription;

			// 레벨 업 필요조건 & 버튼
			trFrameLevelUpRequiredGold.gameObject.SetActive(!IsMaxLevelActiveSkill(csRankActiveSkillNextLevel));
			trFrameLevelUpRequiredItem.gameObject.SetActive(!IsMaxLevelActiveSkill(csRankActiveSkillNextLevel));
			trTextMaxLevel.gameObject.SetActive(IsMaxLevelActiveSkill(csRankActiveSkillNextLevel));

			if (!IsMaxLevelActiveSkill(csRankActiveSkillNextLevel))
			{
				Image imageGoldIcon = trFrameLevelUpRequiredGold.Find("ImageGoldIcon").GetComponent<Image>();
				imageGoldIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");

				Text textGold = trFrameLevelUpRequiredGold.Find("TextGold").GetComponent<Text>();
				CsUIData.Instance.SetFont(textGold);
				textGold.text = csRankActiveSkillLevel.NextLevelUpRequiredGold.ToString("###,###,##0");

				Image imageItemIcon = trFrameLevelUpRequiredItem.Find("ImageItemIcon").GetComponent<Image>();
				imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/item_" + csRankActiveSkillLevel.NextLevelUpRequiredItem.ItemId.ToString());

				trFrameLevelUpRequiredItem.Find("ImageItemIcon").gameObject.SetActive(true);
				trFrameLevelUpRequiredItem.Find("ImageSpiritStoneIcon").gameObject.SetActive(false);

				Text textItem = trFrameLevelUpRequiredItem.Find("TextItem").GetComponent<Text>();
				CsUIData.Instance.SetFont(textItem);
				textItem.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.GetItemCount(csRankActiveSkillLevel.NextLevelUpRequiredItem.ItemId), csRankActiveSkillLevel.NextLevelUpRequiredItemCount);
			}

			transform.Find("FrameButtonActiveSkill/ButtonLevelUp").GetComponent<Button>().interactable = IsAvailableLevelUpActiveSkill(csHeroRankActiveSkill, csRankActiveSkillLevel, csRankActiveSkillNextLevel);
			transform.Find("FrameButtonActiveSkill/ButtonUseSkill").GetComponent<Button>().interactable = IsAvailableSelectionActiveSkill(csHeroRankActiveSkill);

			transform.Find("FrameButtonActiveSkill").gameObject.SetActive(true);
			transform.Find("FrameButtonPassiveSkill").gameObject.SetActive(false);
		}
		else if (m_csRankPassiveSkill != null)
		{
			CsHeroRankPassiveSkill csHeroRankPassiveSkill = CsGameData.Instance.MyHeroInfo.GetHeroRankPassiveSkill(m_csRankPassiveSkill.SkillId);
			CsRankPassiveSkillLevel csRankPassiveSkillLevel = m_csRankPassiveSkill.GetRankPassiveSkillLevel(IsOpenPassiveSkill(csHeroRankPassiveSkill) ? csHeroRankPassiveSkill.Level : 1);
			CsRankPassiveSkillLevel csRankPassiveSkillNextLevel = m_csRankPassiveSkill.GetRankPassiveSkillLevel(IsOpenPassiveSkill(csHeroRankPassiveSkill) ? csHeroRankPassiveSkill.Level + 1 : 1);

			// 스킬 아이콘, 이름, 레벨, 설명
			imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupClass/" + m_csRankPassiveSkill.ImageName);
			trImageSkillLock.gameObject.SetActive(!IsOpenPassiveSkill(csHeroRankPassiveSkill));

			textSkillLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csRankPassiveSkillLevel.Level);
			textSkillName.text = m_csRankPassiveSkill.Name;
			textSkillDescription.gameObject.SetActive(IsOpenPassiveSkill(csHeroRankPassiveSkill));

			if (IsOpenPassiveSkill(csHeroRankPassiveSkill))
			{
				textSkillDescription.text = m_csRankPassiveSkill.Description;
			}

			// 쿨타임 & 잠금해제 조건
			trFrameCoolTime.Find("ImageCoolTimeIcon").gameObject.SetActive(false);
			trFrameCoolTime.Find("ImageLockIcon").gameObject.SetActive(!IsOpenPassiveSkill(csHeroRankPassiveSkill));
			trFrameCoolTime.Find("TextCoolTime").gameObject.SetActive(!IsOpenPassiveSkill(csHeroRankPassiveSkill));
			trSkillDetail.Find("Blank").gameObject.SetActive(!IsOpenPassiveSkill(csHeroRankPassiveSkill));

			Text textCoolTime = trFrameCoolTime.Find("TextCoolTime").GetComponent<Text>();

			if (!IsOpenPassiveSkill(csHeroRankPassiveSkill))
			{
				CsRank csRank = CsGameData.Instance.GetRank(m_csRankPassiveSkill.RequiredRankNo);

				if (csRank != null)
				{
					textCoolTime.text = string.Format(CsConfiguration.Instance.GetString("A28_TXT_01002"), csRank.Name);
				}
			}

			//효과
			trFrameActiveSkillEffect.gameObject.SetActive(false);
			trFramePassiveSkillEffect.gameObject.SetActive(true);

			Transform trPassiveSkillEffectContent = trFramePassiveSkillEffect.Find("Viewport/Content");

			// 콘텐츠 초기화
			for (int i = 0; i < trPassiveSkillEffectContent.childCount; i++)
			{
				trPassiveSkillEffectContent.GetChild(i).gameObject.SetActive(false);
			}

			GameObject goPassiveSkillAttribute = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupClass/ItemPassiveSkillAttribute");

			int nCount = 0;
			foreach (var attr in m_csRankPassiveSkill.RankPassiveSkillAttrList)
			{
				Transform trPassiveSkillAttribute;

				if (nCount < trPassiveSkillEffectContent.childCount)
				{
					trPassiveSkillAttribute = trPassiveSkillEffectContent.GetChild(nCount);
				}
				else
				{
					trPassiveSkillAttribute = Instantiate(goPassiveSkillAttribute).transform;
					trPassiveSkillAttribute.SetParent(trPassiveSkillEffectContent);

					RectTransform rtrPassiveSkillAttribute = trPassiveSkillAttribute.GetComponent<RectTransform>();
					rtrPassiveSkillAttribute.anchoredPosition3D = Vector3.zero;
					rtrPassiveSkillAttribute.localScale = Vector3.one;
				}
				
				trPassiveSkillAttribute.gameObject.SetActive(true);
				trPassiveSkillAttribute.name = attr.Attr.Name;

				Text textName = trPassiveSkillAttribute.Find("TextName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textName);
				textName.text = attr.Attr.Name;

				var attrCurrentLevel = csRankPassiveSkillLevel.GetRankPassiveSkillAttrLevel(attr.Attr.AttrId);
				Text textCurrentValue = trPassiveSkillAttribute.Find("TextCurrentValue").GetComponent<Text>();
				CsUIData.Instance.SetFont(textCurrentValue);
				textCurrentValue.text = (attrCurrentLevel == null ? "0" : attrCurrentLevel.AttrValue.Value.ToString("###,###,##0"));

				trPassiveSkillAttribute.Find("ImageArrow").gameObject.SetActive(!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel));
				trPassiveSkillAttribute.Find("TextNextValue").gameObject.SetActive(!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel));

				if (!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel))
				{
					var attrNextLevel = csRankPassiveSkillNextLevel.GetRankPassiveSkillAttrLevel(attr.Attr.AttrId);
					Text textNextValue = trPassiveSkillAttribute.Find("TextNextValue").GetComponent<Text>();
					CsUIData.Instance.SetFont(textNextValue);
					textNextValue.text = (attrNextLevel == null ? "0" : attrNextLevel.AttrValue.Value.ToString("###,###,##0"));
				}

				nCount++;
			}

			// 레벨 업 필요조건 & 버튼
			trFrameLevelUpRequiredGold.gameObject.SetActive(!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel));
			trFrameLevelUpRequiredItem.gameObject.SetActive(!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel));
			trTextMaxLevel.gameObject.SetActive(IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel));

			if (!IsMaxLevelPassiveSkill(csRankPassiveSkillNextLevel))
			{
				Image imageGoldIcon = trFrameLevelUpRequiredGold.Find("ImageGoldIcon").GetComponent<Image>();
				imageGoldIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");

				Text textGold = trFrameLevelUpRequiredGold.Find("TextGold").GetComponent<Text>();
				CsUIData.Instance.SetFont(textGold);
				textGold.text = csRankPassiveSkillLevel.NextLevelUpRequiredGold.ToString("###,###,##0");

				trFrameLevelUpRequiredItem.Find("ImageItemIcon").gameObject.SetActive(false);
				trFrameLevelUpRequiredItem.Find("ImageSpiritStoneIcon").gameObject.SetActive(true);

				Text textItem = trFrameLevelUpRequiredItem.Find("TextItem").GetComponent<Text>();
				CsUIData.Instance.SetFont(textItem);
				textItem.text = csRankPassiveSkillLevel.NextLevelUpRequiredSpiritStone.ToString("#,##0");
			}

			transform.Find("FrameButtonPassiveSkill/ButtonLevelUp").GetComponent<Button>().interactable = IsAvailablePassiveSkillLevelUp(csHeroRankPassiveSkill, csRankPassiveSkillLevel, csRankPassiveSkillNextLevel);
			
			transform.Find("FrameButtonActiveSkill").gameObject.SetActive(false);
			transform.Find("FrameButtonPassiveSkill").gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleActiveSkill(CsRankActiveSkill csRankActiveSkill, bool bIsOn)
	{
		if (bIsOn)
		{
			m_csRankActiveSkill = csRankActiveSkill;
			m_csRankPassiveSkill = null;

			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			UpdateSkillDetail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedTogglePassiveSkill(CsRankPassiveSkill csRankPassiveSkill, bool bIsOn)
	{
		if (bIsOn)
		{
			m_csRankActiveSkill = null;
			m_csRankPassiveSkill = csRankPassiveSkill;

			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			UpdateSkillDetail();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonActiveSkillLevelUp()
	{
		if (m_csRankActiveSkill != null)
		{
			CsCommandEventManager.Instance.SendRankActiveSkillLevelUp(m_csRankActiveSkill.SkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonUseActiveSkill()
	{
		if (m_csRankActiveSkill != null)
		{
			// 쿨타임 전 변경 불가
			if (CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime > 0)
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A28_TXT_02004"));
				return;
			}

			CsCommandEventManager.Instance.SendRankActiveSkillSelect(m_csRankActiveSkill.SkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPassiveLevelUp()
	{
		if (m_csRankPassiveSkill != null)
		{
			CsCommandEventManager.Instance.SendRankPassiveSkillLevelUp(m_csRankPassiveSkill.SkillId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRankActiveSkillLevelUp()
	{
		UpdateActiveSkillList();
		UpdateSkillDetail();

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A28_TXT_02002"));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventRankActiveSkillSelect()
	{
		UpdateSkillDetail();

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A28_TXT_02001"));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventEventRankPassiveSkillLevelUp()
	{
		UpdateSpiritStoneCount();
		UpdatePassiveSkillList();
		UpdateSkillDetail();

		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A28_TXT_02003"));
	}

	//---------------------------------------------------------------------------------------------------
	bool IsAvailableLevelUpActiveSkill(CsHeroRankActiveSkill heroSkill, CsRankActiveSkillLevel current, CsRankActiveSkillLevel next)
	{
		// 스킬 개방
		if (!IsOpenActiveSkill(heroSkill))
			return false;

		// 골드 부족
		if (CsGameData.Instance.MyHeroInfo.Gold < current.NextLevelUpRequiredGold)
			return false;

		// 아이템 부족
		if (CsGameData.Instance.MyHeroInfo.GetItemCount(current.NextLevelUpRequiredItem.ItemId) < current.NextLevelUpRequiredItemCount)
			return false;

		// 최고 레벨 도달
		if (IsMaxLevelActiveSkill(next))
			return false;

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsAvailableSelectionActiveSkill(CsHeroRankActiveSkill heroSkill)
	{
		// 스킬 개방
		if (!IsOpenActiveSkill(heroSkill))
			return false;

		// 이미 사용 중
		if (heroSkill.SkillId == CsGameData.Instance.MyHeroInfo.SelectedRankActiveSkillId)
			return false;

		// 쿨타임
		if (CsGameData.Instance.MyHeroInfo.RankActiveSkillRemainingCoolTime > 0)
			return false;

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsAvailablePassiveSkillLevelUp(CsHeroRankPassiveSkill heroSkill, CsRankPassiveSkillLevel current, CsRankPassiveSkillLevel next)
	{
		// 스킬 개방
		if (!IsOpenPassiveSkill(heroSkill))
			return false;

		// 골드 부족
		if (CsGameData.Instance.MyHeroInfo.Gold < current.NextLevelUpRequiredGold)
			return false;

		// 아이템 부족
		if (CsGameData.Instance.MyHeroInfo.SpiritStone < current.NextLevelUpRequiredSpiritStone)
			return false;

		// 최고 레벨 도달
		if (IsMaxLevelPassiveSkill(next))
			return false;

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsOpenActiveSkill(CsHeroRankActiveSkill heroSkill)
	{
		return heroSkill != null;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsOpenPassiveSkill(CsHeroRankPassiveSkill heroSkill)
	{
		return heroSkill != null;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsMaxLevelActiveSkill(CsRankActiveSkillLevel nextLevel)
	{
		return nextLevel == null;
	}

	//---------------------------------------------------------------------------------------------------
	bool IsMaxLevelPassiveSkill(CsRankPassiveSkillLevel nextLevel)
	{
		return nextLevel == null;
	}
}
