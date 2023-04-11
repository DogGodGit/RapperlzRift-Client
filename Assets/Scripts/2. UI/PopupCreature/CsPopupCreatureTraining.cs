using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-10)
//---------------------------------------------------------------------------------------------------

public class CsPopupCreatureTraining : CsPopupSub
{
	bool m_bInitialized = false;
	Coroutine m_coroutineSetContents = null;
	Coroutine m_coroutineCheckEvent = null;

	int m_nSelectedItemId = 0;
	List<int> m_listPrevAdditionalAttrId;
	
	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsCreatureManager.Instance.EventCreatureParticipate += OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel += OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer += OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel += OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear += OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureVariation += OnEventCreatureVariation;
		CsCreatureManager.Instance.EventCreatureAdditionalAttrSwitch += OnEventCreatureAdditionalAttrSwitch;
		CsCreatureManager.Instance.EventCreatureSkillSlotOpen += OnEventCreatureSkillSlotOpen;

		CsGameEventUIToUI.Instance.EventSelectCreatureToggle += OnEventSelectCreatureToggle;
		CsGameEventUIToUI.Instance.EventPointerDownCreatureFood += OnEventPointerDownCreatureFood;
		CsGameEventUIToUI.Instance.EventPointerUpCreatureFood += OnEventPointerUpCreatureFood;
		CsGameEventUIToUI.Instance.EventPointerExitCreatureFood += OnEventPointerExitCreatureFood;

		InitializeUI();

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsCreatureManager.Instance.EventCreatureParticipate -= OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel -= OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer -= OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel -= OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear -= OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureVariation -= OnEventCreatureVariation;
		CsCreatureManager.Instance.EventCreatureAdditionalAttrSwitch -= OnEventCreatureAdditionalAttrSwitch;
		CsCreatureManager.Instance.EventCreatureSkillSlotOpen -= OnEventCreatureSkillSlotOpen;

		CsGameEventUIToUI.Instance.EventSelectCreatureToggle -= OnEventSelectCreatureToggle;
		CsGameEventUIToUI.Instance.EventPointerDownCreatureFood -= OnEventPointerDownCreatureFood;
		CsGameEventUIToUI.Instance.EventPointerUpCreatureFood -= OnEventPointerUpCreatureFood;
		CsGameEventUIToUI.Instance.EventPointerExitCreatureFood -= OnEventPointerExitCreatureFood;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		m_nSelectedItemId = 0;

		CsGameEventUIToUI.Instance.OnEventCreatureSubMenuChanged();

		if (m_coroutineSetContents != null)
		{
			StopCoroutine(m_coroutineSetContents);
			m_coroutineSetContents = null;
		}

		m_coroutineSetContents = StartCoroutine(SetContents());
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		// 도감
		CsUIData.Instance.SetButton(transform.Find("ButtonBook"), OnClickButtonBook);
		CsUIData.Instance.SetText(transform.Find("ButtonBook/TextBook"), "A146_BTN_00001", true);

		// 방생
		CsUIData.Instance.SetButton(transform.Find("ButtonRelease"), OnClickButtonRelease);
		CsUIData.Instance.SetText(transform.Find("ButtonRelease/TextRelease"), "A146_BTN_00002", true);

		// 평점
		CsUIData.Instance.SetText(transform.Find("FrameGrade/TextGrade"), "A146_TXT_00001", true);

		// 자질
		Transform trFrameQuality = transform.Find("FrameQuality");
		CsUIData.Instance.SetText(trFrameQuality.Find("TextQuality"), "A146_TXT_00011", true);

		// 변이
		CsUIData.Instance.SetButton(trFrameQuality.Find("ButtonPopupVariation"), OnClickButtonPopupVariation);
		CsUIData.Instance.SetText(trFrameQuality.Find("ButtonPopupVariation/TextPopupVariation"), "A146_BTN_00003", true);

		// 경험치
		Transform trFrameExp = transform.Find("FrameExp");
		CsUIData.Instance.SetText(trFrameExp.Find("TextExp"), "A146_TXT_00003", true);	

		// 양육
		CsUIData.Instance.SetButton(trFrameExp.Find("ButtonPopupRear"), OnClickButtonPopupRear);
		CsUIData.Instance.SetText(trFrameExp.Find("ButtonPopupRear/TextPopupRear"), "A146_BTN_00004", true);

		// 전환
		Transform trButtonPopupSwitch = transform.Find("FrameAdditionalAttribute/ButtonPopupSwitch");
		CsUIData.Instance.SetButton(trButtonPopupSwitch, OnClickButtonPopupSwitch);
		CsUIData.Instance.SetText(trButtonPopupSwitch.transform.Find("TextPopupSwitch"), "A146_BTN_00005", true);

		// 응원
		CsUIData.Instance.SetButton(transform.Find("ButtonCheer"), OnClickButtonCheer);
		CsUIData.Instance.SetText(transform.Find("ButtonCheer/TextCheer"), "A146_BTN_00006", true);

		// 출전
		CsUIData.Instance.SetButton(transform.Find("ButtonParticipate"), OnClickButtonParticipate);
		CsUIData.Instance.SetText(transform.Find("ButtonParticipate/TextParticipate"), "A146_BTN_00007", true);

		// 휴전
		CsUIData.Instance.SetButton(transform.Find("ButtonRest"), OnClickButtonRest);
		CsUIData.Instance.SetText(transform.Find("ButtonRest/TextRest"), "A146_BTN_00008", true);

		// 방생 팝업
		Transform trPopupReleaseImageBackground = transform.Find("PopupRelease/ImageBackground");

		CsUIData.Instance.SetText(trPopupReleaseImageBackground.Find("TextReward"), "A146_TXT_00034", true);

		CsUIData.Instance.SetButton(trPopupReleaseImageBackground.Find("ButtonCancel"), OnClickButtonCancelPopupRelease);
		CsUIData.Instance.SetText(trPopupReleaseImageBackground.Find("ButtonCancel/Text"), "PUBLIC_BTN_NO", true);
		CsUIData.Instance.SetButton(trPopupReleaseImageBackground.Find("ButtonOK"), OnClickButtonOKPopupRelease);
		CsUIData.Instance.SetText(trPopupReleaseImageBackground.Find("ButtonOK/Text"), "PUBLIC_BTN_YES", true);

		// 스킬 정보창
		CsUIData.Instance.SetButton(transform.Find("PopupSkillInfo"), OnClickBackgroundPopupSkillInfo);

		// 변이 팝업
		Transform trImageBackgroundPopupVariation = transform.Find("PopupVariation/ImageBackground");

		CsUIData.Instance.SetButton(trImageBackgroundPopupVariation.Find("ButtonClose"), () => { transform.Find("PopupVariation").gameObject.SetActive(false); });

		CsUIData.Instance.SetText(trImageBackgroundPopupVariation.Find("TextTitle"), "A146_BTN_00003", true);
		CsUIData.Instance.SetText(trImageBackgroundPopupVariation.Find("ImageFrameGrade/TextGrade"), "A146_TXT_00001", true);
		CsUIData.Instance.SetText(trImageBackgroundPopupVariation.Find("FrameQuality/TextQuality"), "A146_TXT_00011", true);

		CsItem csVariationRequiredItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.CreatureVariationRequiredItemId);

		if (csVariationRequiredItem != null)
		{
			Transform trFrameRequiredItem = trImageBackgroundPopupVariation.Find("FrameBottom/FrameRequiredItem");
			CsUIData.Instance.SetImage(trFrameRequiredItem.Find("ImageIcon"), "GUI/Items/" + csVariationRequiredItem.Image);
			CsUIData.Instance.SetText(trFrameRequiredItem.Find("TextName"), csVariationRequiredItem.Name, false);
		}

		CsUIData.Instance.SetButton(trImageBackgroundPopupVariation.Find("FrameBottom/ButtonVariation"), OnClickButtonVariation);
		CsUIData.Instance.SetText(trImageBackgroundPopupVariation.Find("FrameBottom/ButtonVariation/TextVariation"), "A146_BTN_00003", true);

		// 양육 팝업
		CsUIData.Instance.SetButton(transform.Find("PopupRear"), OnClickBackgroundPopupRear);
		CsUIData.Instance.SetText(transform.Find("PopupRear/ImageBackground/TextDescription"), "A146_TXT_00005", true);
		CsUIData.Instance.SetText(transform.Find("PopupRear/ImageBackground/FrameNoItem/TextNoItem"), "A146_TXT_00006", true);

		// 전환 팝업
		Transform trImageBackgroundPopupSwitch = transform.Find("PopupSwitch/ImageBackground");

		CsUIData.Instance.SetButton(trImageBackgroundPopupSwitch.Find("ButtonClose"), () => { transform.Find("PopupSwitch").gameObject.SetActive(false); });
		CsUIData.Instance.SetText(trImageBackgroundPopupSwitch.Find("TextTitle"), "A146_BTN_00005", true);

		Transform trFrameCreatureInfoPopupSwitch = trImageBackgroundPopupSwitch.Find("FrameCreatureInfo");

		CsUIData.Instance.SetText(trFrameCreatureInfoPopupSwitch.Find("TextGrade"), "A146_TXT_00001", true);
		CsUIData.Instance.SetText(trFrameCreatureInfoPopupSwitch.Find("TextMainAttribute"), "A146_TXT_00013", true);

		CsUIData.Instance.SetText(trImageBackgroundPopupSwitch.Find("FrameAdditionalAttribute/FrameNext/TextContent"), "A146_TXT_00015", true);

		CsItem csSwitchRequiredItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.CreatureAdditionalAttrSwitchRequiredItemId);

		if (csSwitchRequiredItem != null)
		{
			Transform trFrameRequiredItem = trImageBackgroundPopupSwitch.Find("FrameBottom/FrameRequiredItem");
			CsUIData.Instance.SetImage(trFrameRequiredItem.Find("ImageIcon"), "GUI/Items/" + csSwitchRequiredItem.Image);
			CsUIData.Instance.SetText(trFrameRequiredItem.Find("TextName"), csSwitchRequiredItem.Name, false);
		}

		CsUIData.Instance.SetButton(trImageBackgroundPopupSwitch.Find("FrameBottom/ButtonSwitch"), OnClickButtonSwitch);
		CsUIData.Instance.SetText(trImageBackgroundPopupSwitch.Find("FrameBottom/ButtonSwitch/TextSwitch"), "A146_BTN_00005", true);

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SetContents()
	{
		yield return new WaitUntil(() => m_bInitialized);

		Guid guidSelectedCreature = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature selectedCreature = CsCreatureManager.Instance.GetHeroCreature(guidSelectedCreature);
		UpdateCreatureInfo(selectedCreature);

		m_coroutineSetContents = null;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureInfo(CsHeroCreature csHeroCreature)
	{
		if (csHeroCreature == null)
			return;

		UpdateCreatureLevel(csHeroCreature);
		UpdateCreatureModel(csHeroCreature);

		UpdateCreatureGrade(csHeroCreature);

		UpdateCreatureQuality(csHeroCreature);
		UpdateNoticeButtonPopupVaritaion(csHeroCreature);
		UpdateCreatureAttributes(csHeroCreature);

		UpdateCreatureSkills(csHeroCreature);

		UpdateCreatureExp(csHeroCreature);
		UpdateNoticeButtonPopupRear(csHeroCreature);
		
		UpdateCreatureAdditionalAttributes(csHeroCreature);
		UpdateNoticeButtonPopupSwitch();

		UpdateButtons(csHeroCreature);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureLevel(CsHeroCreature csHeroCreature)
	{
		Text textValueLevelName = transform.Find("TextValueLevelName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textValueLevelName);
		textValueLevelName.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_00027"), csHeroCreature.Level, csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name);

		Transform trLayoutLevel = transform.Find("LayoutLevel");

		SetCreatureLevelStar(trLayoutLevel, csHeroCreature);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureModel(CsHeroCreature csHeroCreature)
	{
		LoadCreatureModel(csHeroCreature.Creature.CreatureCharacter);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureGrade(CsHeroCreature csHeroCreature)
	{
		Transform trFrameGrade = transform.Find("FrameGrade");

		if (trFrameGrade != null)
		{
			CsUIData.Instance.SetText(trFrameGrade.Find("TextValue"), csHeroCreature.GetCreatureGrade().ToString("#,##0"), false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureSkills(CsHeroCreature csHeroCreature)
	{
		Transform trFrameSkills = transform.Find("FrameSkills");

		for (int i = 0; i < CsGameConfig.Instance.CreatureSkillSlotMaxCount; i++)
		{
			int nSlotIndex = i;

			Transform trSkill = trFrameSkills.Find("Skill" + i.ToString());

			if (trSkill != null)
			{
				trSkill.Find("ImageLock").gameObject.SetActive(i >= CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + csHeroCreature.AdditionalOpenSkillSlotCount);

				CsHeroCreatureSkill csHeroCreatureSkill = csHeroCreature.HeroCreatureSkillList.Find(heroCreatureSkill => heroCreatureSkill.SlotIndex == nSlotIndex);
				trSkill.GetComponent<Image>().enabled = csHeroCreatureSkill != null;

				if (csHeroCreatureSkill != null)
				{
					CsUIData.Instance.SetImage(trSkill, "GUI/PopupCreature/" + csHeroCreatureSkill.CreatureSkill.ImageName);
				}

				CsUIData.Instance.SetButton(trSkill, () => OnClickButtonSkill(nSlotIndex));
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureQuality(CsHeroCreature csHeroCreature)
	{
		Transform trFrameQuality = transform.Find("FrameQuality");

		if (trFrameQuality != null)
		{
			Text textValueQuality = trFrameQuality.Find("TextValueQuality").GetComponent<Text>();
			CsUIData.Instance.SetFont(textValueQuality);
			textValueQuality.text = csHeroCreature.Quality.ToString("#,##0");
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateNoticeButtonPopupVaritaion(CsHeroCreature csHeroCreature)
	{
		bool bItemRemaining = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureVariationRequiredItemId) >= CsCreatureManager.CreatureVariationRequireItemCount;

		bool bCountRemaining = CsCreatureManager.Instance.DailyCreatureVariationCount < CsGameData.Instance.MyHeroInfo.VipLevel.CreatureVariationMaxCount;

		bool bMaxValue = true;

		foreach (CsHeroCreatureBaseAttr csHeroCreatureBaseAttr in csHeroCreature.HeroCreatureBaseAttrList)
		{
			CsCreatureBaseAttrValue csCreatureBaseAttrValue = csHeroCreature.Creature.GetCreatureBaseAttrValue(csHeroCreatureBaseAttr.CreatureBaseAttr.Attr.AttrId);

			if (csCreatureBaseAttrValue != null &&
				csHeroCreatureBaseAttr.BaseValue < csCreatureBaseAttrValue.MaxAttrValue)
			{
				bMaxValue = false;
				break;
			}
		}

		transform.Find("FrameQuality/ButtonPopupVariation/ImageNotice").gameObject.SetActive(bCountRemaining && !bMaxValue && bItemRemaining);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureExp(CsHeroCreature csHeroCreature)
	{
		Transform trFrameExp = transform.Find("FrameExp");

		Text textValueExp = trFrameExp.Find("TextValueExp").GetComponent<Text>();
		CsUIData.Instance.SetFont(textValueExp);
		textValueExp.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), csHeroCreature.Exp, csHeroCreature.CreatureLevel.NextLevelUpRequiredExp);

		Slider slider = trFrameExp.Find("Slider").GetComponent<Slider>();
		slider.value = (float)csHeroCreature.Exp / csHeroCreature.CreatureLevel.NextLevelUpRequiredExp;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateNoticeButtonPopupRear(CsHeroCreature csHeroCreature)
	{
		bool bMaxLevel = csHeroCreature.Level >= CsGameData.Instance.CreatureLevelList.Max(creatureLevel => creatureLevel.Level);

		bool bExistCreatureFood = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureFood) > 0;

		transform.Find("FrameExp/ButtonPopupRear/ImageNotice").gameObject.SetActive(!bMaxLevel && bExistCreatureFood);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureAttributes(CsHeroCreature csHeroCreature)
	{
		Transform trFrameAttribute = transform.Find("FrameAttribute");

		for (int i = 0; i < trFrameAttribute.childCount; i++)
		{
			trFrameAttribute.GetChild(i).GetComponent<Text>().text = "";
		}

		int nStatIndex = 0;

		foreach (CsHeroCreatureBaseAttr csHeroCreatureBaseAttr in csHeroCreature.HeroCreatureBaseAttrList)
		{
			int nAttrValue = csHeroCreatureBaseAttr.BaseValue;

			CsCreatureBaseAttrValue csCreatureBaseAttrValue = csHeroCreature.Creature.GetCreatureBaseAttrValue(csHeroCreatureBaseAttr.CreatureBaseAttr.Attr.AttrId);

			if (csCreatureBaseAttrValue != null)
			{
				nAttrValue += (int)((csHeroCreature.Level * csCreatureBaseAttrValue.IncAttrValue) * (csHeroCreature.Quality / 1000.0));
			}

			Text textStat = trFrameAttribute.Find("TextAttr" + nStatIndex.ToString()).GetComponent<Text>();
			CsUIData.Instance.SetFont(textStat);
			textStat.text = csHeroCreatureBaseAttr.CreatureBaseAttr.Attr.Name;

			Text textValueStat = trFrameAttribute.Find("TextValueAttr" + nStatIndex.ToString()).GetComponent<Text>();
			CsUIData.Instance.SetFont(textValueStat);
			textValueStat.text = nAttrValue.ToString("#,##0");

			nStatIndex++;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureAdditionalAttributes(CsHeroCreature csHeroCreature)
	{
		Transform trFrameAdditionalAttribute = transform.Find("FrameAdditionalAttribute");

		for (int i = 0; i < trFrameAdditionalAttribute.childCount; i++)
		{
			if (trFrameAdditionalAttribute.GetChild(i).name.Contains("Text"))
			{
				trFrameAdditionalAttribute.GetChild(i).GetComponent<Text>().text = "";
			}
		}

		int nStatIndex = 0;

		foreach (int nAttrId in csHeroCreature.AdditionalAttrIdList)
		{
			CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(nAttrId);

			if (csCreatureAdditionalAttr != null)
			{
				CsCreatureAdditionalAttrValue csCreatureAdditionalAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.CreatureInjectionLevel.InjectionLevel);

				if (csCreatureAdditionalAttrValue != null)
				{
					Text textStat = trFrameAdditionalAttribute.Find("TextAttr" + nStatIndex.ToString()).GetComponent<Text>();
					CsUIData.Instance.SetFont(textStat);
					textStat.text = csCreatureAdditionalAttr.Attr.Name;

					Text textValueStat = trFrameAdditionalAttribute.Find("TextValueAttr" + nStatIndex.ToString()).GetComponent<Text>();
					CsUIData.Instance.SetFont(textValueStat);
					textValueStat.text = csCreatureAdditionalAttrValue.AttrValue.Value.ToString("#,##0");
				}
			}

			nStatIndex++;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateNoticeButtonPopupSwitch()
	{
		transform.Find("FrameAdditionalAttribute/ButtonPopupSwitch/ImageNotice").gameObject.SetActive(CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureAdditionalAttrSwitchRequiredItemId) >= CsCreatureManager.CreatureSwitchRequireItemCount);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButtons(Guid guidCreatureInstanceId)
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			UpdateButtons(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButtons(CsHeroCreature csHeroCreature)
	{
		Transform trButtonCheer = transform.Find("ButtonCheer");
		Transform trButtonParticipate = transform.Find("ButtonParticipate");
		Transform trButtonRest = transform.Find("ButtonRest");
		
		if (trButtonCheer != null)
		{
			trButtonCheer.gameObject.SetActive(!csHeroCreature.Cheered);
		}

		if (trButtonParticipate != null)
		{
			trButtonParticipate.gameObject.SetActive(!csHeroCreature.InstanceId.Equals(CsCreatureManager.Instance.ParticipatedCreatureId));
		}

		if (trButtonRest != null)
		{
			trButtonRest.gameObject.SetActive(csHeroCreature.Cheered || csHeroCreature.InstanceId.Equals(CsCreatureManager.Instance.ParticipatedCreatureId));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void LoadCreatureModel(CsCreatureCharacter csCreatureCharacter)
	{
		Transform tr3DMosnter = transform.Find("3DCreature");

		for (int i = 0; i < tr3DMosnter.childCount; ++i)
		{
			if (tr3DMosnter.GetChild(i).GetComponent<Camera>() != null)
			{
				continue;
			}

			tr3DMosnter.GetChild(i).gameObject.SetActive(false);
		}

		if (csCreatureCharacter == null)
		{
			return;
		}

		Transform trMountModel = transform.Find("3DCreature/" + csCreatureCharacter.PrefabName);

		if (trMountModel == null)
		{
			StartCoroutine(LoadCreatureModelCoroutine(csCreatureCharacter));
		}
		else
		{
			trMountModel.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadCreatureModelCoroutine(CsCreatureCharacter csCreatureCharacter)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/MonsterObject/" + csCreatureCharacter.PrefabName);
		yield return resourceRequest;

		transform.Find("3DCreature/UIChar_Camera").gameObject.SetActive(true);

		GameObject goCreature = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DCreature"));

		int nLayer = LayerMask.NameToLayer("UIChar");

		Transform[] atrMount = goCreature.GetComponentsInChildren<Transform>();

		for (int i = 0; i < atrMount.Length; ++i)
		{
			atrMount[i].gameObject.layer = nLayer;
		}

		goCreature.transform.localPosition = new Vector3(0, -130, 500);
		goCreature.transform.eulerAngles = new Vector3(0, 180, 0);

		goCreature.transform.localScale = new Vector3(150, 150, 150);
		goCreature.name = csCreatureCharacter.PrefabName;
		goCreature.tag = "Untagged";
		goCreature.gameObject.SetActive(true);

	}

	//---------------------------------------------------------------------------------------------------
	void UpdatePopupVariation(Guid guidCreatureInstanceId)
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			Transform trImageBackgroundPopupVariation = transform.Find("PopupVariation/ImageBackground");

			// 평점
			CsUIData.Instance.SetText(trImageBackgroundPopupVariation.Find("ImageFrameGrade/TextValueGrade"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00002"), csHeroCreature.GetCreatureGrade().ToString("#,##0")), false);

			// 자질
			Transform trFrameQuality = trImageBackgroundPopupVariation.Find("FrameQuality");

			CsUIData.Instance.SetText(trFrameQuality.Find("TextValueQuality"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00012"), csHeroCreature.Quality, csHeroCreature.Creature.MaxQuality), false);

			Transform trLayoutGauge = trFrameQuality.Find("LayoutGauge");

			float flPointPerGauge = csHeroCreature.Creature.MaxQuality / trLayoutGauge.childCount;

			float flCurrentQuality = csHeroCreature.Quality;

			int nQualityLevel = (int)(csHeroCreature.Quality / flPointPerGauge) + 1;

			if (nQualityLevel > 5)
			{
				nQualityLevel = 5;
			}

			for (int i = 0; i < trLayoutGauge.childCount; i++)
			{
				Slider slider = trLayoutGauge.GetChild(i).GetComponent<Slider>();

				if (flCurrentQuality > flPointPerGauge)
				{
					slider.value = 1.0f;
					flCurrentQuality -= flPointPerGauge;
				}
				else
				{
					slider.value = flCurrentQuality / flPointPerGauge;
					flCurrentQuality = 0f;
				}

				CsUIData.Instance.SetImage(trLayoutGauge.GetChild(i).Find("ImageFill"), "GUI/PopupCreature/frm_guage_creature_" + nQualityLevel.ToString());
			}

			// 기본 능력치
			Transform trFrameAttribute = trImageBackgroundPopupVariation.Find("FrameAttribute");

			for (int i = 0; i < trFrameAttribute.childCount; i++)
			{
				trFrameAttribute.GetChild(i).gameObject.SetActive(false);
			}

			int nChildIndex = 0;

			foreach (CsHeroCreatureBaseAttr csHeroCreatureBaseAttr in csHeroCreature.HeroCreatureBaseAttrList)
			{
				if (nChildIndex < trFrameAttribute.childCount)
				{
					Transform trAttribute = trFrameAttribute.GetChild(nChildIndex);

					if (trAttribute != null)
					{
						int nAttributeMaxValue = csHeroCreature.Creature.GetCreatureBaseAttrValue(csHeroCreatureBaseAttr.CreatureBaseAttr.Attr.AttrId).MaxAttrValue;

						CsUIData.Instance.SetText(trAttribute.Find("TextAttribute"), csHeroCreatureBaseAttr.CreatureBaseAttr.Attr.Name, false);
						CsUIData.Instance.SetText(trAttribute.Find("TextValueAttribute"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"),
							csHeroCreatureBaseAttr.BaseValue.ToString("#,##0"), nAttributeMaxValue.ToString("#,##0")), false);

						Slider slider = trAttribute.Find("Gauge").GetComponent<Slider>();
						slider.value = (float)csHeroCreatureBaseAttr.BaseValue / nAttributeMaxValue;

						int nAttrLevel = (int)(csHeroCreatureBaseAttr.BaseValue / (nAttributeMaxValue / 5.0f)) + 1;

						if (nAttrLevel > 5)
						{
							nAttrLevel = 5;
						}

						CsUIData.Instance.SetImage(trAttribute.Find("Gauge/ImageFill"), "GUI/PopupCreature/frm_guage_creature_" + nAttrLevel.ToString());
					}

					trAttribute.gameObject.SetActive(true);
				}

				nChildIndex++;
			}

			// 필요아이템
			Transform trFrameRequiredItem = trImageBackgroundPopupVariation.Find("FrameBottom/FrameRequiredItem");

			if (trFrameRequiredItem != null)
			{
				int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureVariationRequiredItemId);

				CsUIData.Instance.SetText(trFrameRequiredItem.Find("TextCount"),
						string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), nItemCount, CsCreatureManager.CreatureVariationRequireItemCount), false);

				bool bRemainCount = CsCreatureManager.Instance.DailyCreatureVariationCount < CsGameData.Instance.MyHeroInfo.VipLevel.CreatureVariationMaxCount;

				trImageBackgroundPopupVariation.Find("FrameBottom/ButtonVariation").GetComponent<Button>().interactable = bRemainCount && nItemCount >= CsCreatureManager.CreatureVariationRequireItemCount;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdatePopupRear()
	{
		int nfoodItemCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureFood);

		transform.Find("PopupRear/ImageBackground/Scroll View").gameObject.SetActive(nfoodItemCount > 0);
		transform.Find("PopupRear/ImageBackground/FrameNoItem").gameObject.SetActive(nfoodItemCount <= 0);

		if (nfoodItemCount > 0)
		{
			Transform trContent = transform.Find("PopupRear/ImageBackground/Scroll View/Viewport/Content");

			for (int i = 0; i < trContent.childCount; i++)
			{
				trContent.GetChild(i).gameObject.SetActive(false);
			}

			var ienumerable = from inventorySlot in CsGameData.Instance.MyHeroInfo.InventorySlotList
							  where inventorySlot.EnType == EnInventoryObjectType.Item && inventorySlot.InventoryObjectItem.Item.ItemType.EnItemType == EnItemType.CreatureFood
							  select inventorySlot.InventoryObjectItem;

			Dictionary<int, int> m_dicCreatureFood = new Dictionary<int,int>();

			foreach (CsInventoryObjectItem csInventoryObjectItem in ienumerable)
			{
				if (m_dicCreatureFood.ContainsKey(csInventoryObjectItem.Item.ItemId))
				{
					m_dicCreatureFood[csInventoryObjectItem.Item.ItemId] += csInventoryObjectItem.Count;
				}
				else
				{
					m_dicCreatureFood.Add(csInventoryObjectItem.Item.ItemId, csInventoryObjectItem.Count);
				}
			}

			int nChildIndex = 0;

			GameObject goRearItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/RearItem");

			foreach (var pair in m_dicCreatureFood)
			{
				Transform trRearItem = null;

				if (nChildIndex < trContent.childCount)
				{
					trRearItem = trContent.GetChild(nChildIndex);
					trRearItem.gameObject.SetActive(true);
				}
				else
				{
					trRearItem = Instantiate(goRearItem, trContent).transform;
				}

				CsItem csItem = CsGameData.Instance.GetItem(pair.Key);

				if (csItem != null)
				{
					trRearItem.name = csItem.ItemId.ToString();

					CsUIData.Instance.DisplayItemSlot(trRearItem.Find("ItemSlot"), csItem, false, pair.Value, false, EnItemSlotSize.Medium, false);
					CsUIData.Instance.SetText(trRearItem.Find("TextName"), csItem.Name, false);
					CsUIData.Instance.SetText(trRearItem.Find("TextExp"), "A146_TXT_00003", true);
					CsUIData.Instance.SetText(trRearItem.Find("TextValueExp"), csItem.Value1.ToString(), false);
				}

				nChildIndex++;
			}
		}
		else
		{
			GameObject goRearItemDungeon = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/RearItemDungeon");

			Transform trFrameNoItem = transform.Find("PopupRear/ImageBackground/FrameNoItem");

			Transform trDragonNest = trFrameNoItem.Find("DragonNest");

			if (trDragonNest == null)
			{
				trDragonNest = Instantiate(goRearItemDungeon, trFrameNoItem).transform;
				trDragonNest.name = "DragonNest";
				CsUIData.Instance.SetImage(trDragonNest.Find("ImageIcon"), "GUI/Common/todaytask_13"); // %%% 이미지 변경
				CsUIData.Instance.SetText(trDragonNest.Find("TextContent"), "A146_TXT_00007", true);
			}

			CsUIData.Instance.SetButton(trDragonNest, () => CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.PartyDungeon));
			
			Transform trCreatureFarm = trFrameNoItem.Find("CreatureFarm");

			if (trCreatureFarm == null)
			{
				trCreatureFarm = Instantiate(goRearItemDungeon, trFrameNoItem).transform;
				trCreatureFarm.name = "CreatureFarm";
				CsUIData.Instance.SetImage(trCreatureFarm.Find("ImageIcon"), "GUI/Common/todaytask_13"); // %%% 이미지 변경
				CsUIData.Instance.SetText(trCreatureFarm.Find("TextContent"), "A146_TXT_00008", true);
			}

			// 크리처 농장
			CsUIData.Instance.SetButton(trCreatureFarm, () => { CsGameEventUIToUI.Instance.OnEventCloseAllPopup(); CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.CreatureFarm); });
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdatePopupSwitch(bool bInitialize)
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		Transform trImageBackgroundPopupSwitch = transform.Find("PopupSwitch/ImageBackground");

		// 크리처 정보
		Transform trFrameCreatureInfo = trImageBackgroundPopupSwitch.Find("FrameCreatureInfo");

		Transform trItemSlot = trFrameCreatureInfo.Find("ItemSlot");

		CsUIData.Instance.DisplayCreature(trItemSlot, csHeroCreature.Creature);
		CsUIData.Instance.SetText(trFrameCreatureInfo.Find("TextLevel"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01001"), csHeroCreature.Level.ToString()), false);
		CsUIData.Instance.SetText(trFrameCreatureInfo.Find("TextName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name), false);

		// 별
		Transform trLayoutLevel = trFrameCreatureInfo.Find("LayoutLevel");

		SetCreatureLevelStar(trLayoutLevel, csHeroCreature);

		// 평점
		CsUIData.Instance.SetText(trFrameCreatureInfo.Find("TextValueGrade"), csHeroCreature.GetCreatureGrade().ToString("#,##0"), false);

		CsElemental csElemental = CsGameData.Instance.GetElemental(CsGameData.Instance.MyHeroInfo.Job.Elemental.ElementalId);

		if (csElemental != null)
		{
			CsUIData.Instance.SetImage(trFrameCreatureInfo.Find("ImageIconMainAttribute"), "GUI/PopupCreature/ico_skill_property" + csElemental.ElementalId.ToString("00"));
			CsUIData.Instance.SetText(trFrameCreatureInfo.Find("TextValueMainAttribute"), csElemental.Name, false);
		}

		// 추가 속성
		GameObject goSwitchItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/SwitchItem");

		Transform trFrameAdditionalAttribute = trImageBackgroundPopupSwitch.Find("FrameAdditionalAttribute");

		// 변환 전
		Transform trFrameCurrent = trFrameAdditionalAttribute.Find("FrameCurrent");

		for (int i = 0; i < trFrameCurrent.childCount; i++)
		{
			trFrameCurrent.GetChild(i).gameObject.SetActive(false);
		}

		List<int> listAdditionalAttr = bInitialize ? csHeroCreature.AdditionalAttrIdList : m_listPrevAdditionalAttrId;

		int nChildIndex = 0;

		foreach (int nAttrId in listAdditionalAttr)
		{
			CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(nAttrId);

			if (csCreatureAdditionalAttr != null)
			{
				CsCreatureAdditionalAttrValue csCreatureAdditionalAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.InjectionLevel);
				
				if (csCreatureAdditionalAttrValue != null)
				{
					Transform trSwitchItem = null;

					if (nChildIndex < trFrameCurrent.childCount)
					{
						trSwitchItem = trFrameCurrent.GetChild(nChildIndex);
						trSwitchItem.gameObject.SetActive(true);
					}
					else
					{
						trSwitchItem = Instantiate(goSwitchItem, trFrameCurrent).transform;
					}

					if (trSwitchItem != null)
					{
						trSwitchItem.name = csCreatureAdditionalAttr.Attr.AttrId.ToString();

						CsUIData.Instance.SetImage(trSwitchItem.Find("ImageIcon"), GetAttributeImagePath(csCreatureAdditionalAttr.Attr.AttrId));
						CsUIData.Instance.SetText(trSwitchItem.Find("TextName"), csCreatureAdditionalAttr.Attr.Name, false);
						CsUIData.Instance.SetText(trSwitchItem.Find("TextValue"), csCreatureAdditionalAttrValue.AttrValue.Value.ToString(), false);
					}
				}
			}

			nChildIndex++;
		}

		// 변환 후
		Transform trFrameNext = trFrameAdditionalAttribute.Find("FrameNext");

		for (int i = 2; i < trFrameNext.childCount; i++)
		{
			trFrameNext.GetChild(i).gameObject.SetActive(false);
		}

		trFrameNext.Find("ImageIcon").gameObject.SetActive(bInitialize);
		trFrameNext.Find("TextContent").gameObject.SetActive(bInitialize);

		if (!bInitialize)
		{
			nChildIndex = 2;

			foreach (int nAttrId in csHeroCreature.AdditionalAttrIdList)
			{
				CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(nAttrId);

				if (csCreatureAdditionalAttr != null)
				{
					CsCreatureAdditionalAttrValue csCreatureAdditionalAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.InjectionLevel);

					if (csCreatureAdditionalAttrValue != null)
					{
						Transform trSwitchItem = null;

						if (nChildIndex < trFrameNext.childCount)
						{
							trSwitchItem = trFrameNext.GetChild(nChildIndex);
							trSwitchItem.gameObject.SetActive(true);
						}
						else
						{
							trSwitchItem = Instantiate(goSwitchItem, trFrameNext).transform;
						}

						if (trSwitchItem != null)
						{
							trSwitchItem.name = csCreatureAdditionalAttr.Attr.AttrId.ToString();

							CsUIData.Instance.SetImage(trSwitchItem.Find("ImageIcon"), GetAttributeImagePath(csCreatureAdditionalAttr.Attr.AttrId));
							CsUIData.Instance.SetText(trSwitchItem.Find("TextName"), csCreatureAdditionalAttr.Attr.Name, false);
							CsUIData.Instance.SetText(trSwitchItem.Find("TextValue"), csCreatureAdditionalAttrValue.AttrValue.Value.ToString(), false);
						}
					}
				}

				nChildIndex++;
			}
		}
		
		m_listPrevAdditionalAttrId = new List<int>(csHeroCreature.AdditionalAttrIdList);

		// 재료 개수
		int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureAdditionalAttrSwitchRequiredItemId);

		CsUIData.Instance.SetText(trImageBackgroundPopupSwitch.Find("FrameBottom/FrameRequiredItem/TextCount"),
			string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), nItemCount, CsCreatureManager.CreatureSwitchRequireItemCount), false);

		// 재료 부족 시 전환 버튼 비활성
		Button buttonSwitch = trImageBackgroundPopupSwitch.Find("FrameBottom/ButtonSwitch").GetComponent<Button>();
		buttonSwitch.interactable = nItemCount >= CsCreatureManager.CreatureSwitchRequireItemCount;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CheckEvent()
	{
		yield return new WaitForSeconds(CsCreatureManager.CreatureFeedStartDelayTime);

		if (m_nSelectedItemId > 0)
		{
			CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

			while(true)
			{
				if (csHeroCreature.Level < CsGameData.Instance.CreatureLevelList.Max(creatureLevel => creatureLevel.Level))
				{
					if (CsGameData.Instance.MyHeroInfo.GetItemCount(m_nSelectedItemId) > 0)
					{
						CsCreatureManager.Instance.SendCreatureRear(csHeroCreature.InstanceId, m_nSelectedItemId);
					}
					else
					{
						break;
					}
				}
				else
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00033"));
					break;
				}

				yield return new WaitForSeconds(CsCreatureManager.CreatureFeedDelayTime);
			}
		}

		m_nSelectedItemId = 0;
		m_coroutineCheckEvent = null;
	}

	//---------------------------------------------------------------------------------------------------
	string GetAttributeImagePath(int nAttrId)
	{
		switch (nAttrId)
		{
			case 12:
			case 13:
				// 화염
				return "GUI/PopupCreature/ico_skill_property01";
			case 14:
			case 15:
				// 번개
				return "GUI/PopupCreature/ico_skill_property02";
			case 16:
			case 17:
				// 암흑
				return "GUI/PopupCreature/ico_skill_property03";
			case 18:
			case 19:
				// 신성
				return "GUI/PopupCreature/ico_skill_property04";

			default:
				return string.Empty;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetCreatureLevelStar(Transform trLayoutLevel, CsHeroCreature csHeroCreature)
	{
		for (int i = 0; i < trLayoutLevel.childCount; i++)
		{
			trLayoutLevel.GetChild(i).gameObject.SetActive(false);
		}

		GameObject goImageStar = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/ImageStar");

		int nStarLevel = csHeroCreature.InjectionLevel / 10 + 1;
		int nStarCount = csHeroCreature.InjectionLevel % 10;

		if (nStarCount == 0)
		{
			nStarCount += 10;
			nStarLevel -= 1;
		}

		int nFullStarCount = nStarCount / 2;

		Sprite spriteStarFull = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreature/ico_star" + nStarLevel.ToString() + "_equip_on");
		Sprite spriteStarHalf = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreature/ico_star" + nStarLevel.ToString() + "_equip_off");

		// 1개짜리 별
		for (int i = 0; i < nFullStarCount; i++)
		{
			Transform trStar = null;

			if (i < trLayoutLevel.childCount)
			{
				trStar = trLayoutLevel.GetChild(i);
				trStar.gameObject.SetActive(true);
			}
			else
			{
				trStar = Instantiate(goImageStar, trLayoutLevel).transform;
			}

			if (trStar != null)
			{
				trStar.name = "Star" + i.ToString();

				Image imageStar = trStar.GetComponent<Image>();
				imageStar.sprite = spriteStarFull;
			}
		}

		// 반개짜리 별
		if (nStarCount % 2 == 1)
		{
			Transform trStarHalf = null;

			if (nFullStarCount + 1 < trLayoutLevel.childCount)
			{
				trStarHalf = trLayoutLevel.GetChild(nFullStarCount + 1);
				trStarHalf.gameObject.SetActive(true);
			}
			else
			{
				trStarHalf = Instantiate(goImageStar, trLayoutLevel).transform;
			}

			if (trStarHalf != null)
			{
				trStarHalf.name = "Star" + nFullStarCount.ToString();

				Image imageStarHalf = trStarHalf.GetComponent<Image>();
				imageStarHalf.sprite = spriteStarHalf;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupRelease()
	{
		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			int nAccumulatedExp = 0;

			var levelUpRequiredExps = from creatureLevel in CsGameData.Instance.CreatureLevelList
									  where creatureLevel.Level < csHeroCreature.Level
									  select creatureLevel.NextLevelUpRequiredExp;

			foreach (int nLevelUpRequiredExp in levelUpRequiredExps)
			{
				nAccumulatedExp += nLevelUpRequiredExp;
			}

			nAccumulatedExp += csHeroCreature.Exp;

			int nExp = (int)(nAccumulatedExp * (CsGameConfig.Instance.CreatureReleaseExpRetrievalRate / 10000.0f));
			
			List<CsItem> listCreatureFood = CsGameData.Instance.ItemList.FindAll(item => item.ItemType.EnItemType == EnItemType.CreatureFood);
			var listOrder = listCreatureFood.OrderByDescending(item => item.Value1);

			Transform trFrameReward = transform.Find("PopupRelease/ImageBackground/FrameReward");

			for (int i = 0; i < trFrameReward.childCount; i++)
			{
				trFrameReward.GetChild(i).gameObject.SetActive(false);
			}

			int nSlotIndex = 0;
			foreach (var csItem in listOrder)
			{
				int nCount = nExp / csItem.Value1;
				
				if (nCount > 0)
				{
					Transform trSlot = trFrameReward.GetChild(nSlotIndex);
					trSlot.gameObject.SetActive(true);

					CsUIData.Instance.DisplayItemSlot(trSlot, csItem, false, nCount, false, EnItemSlotSize.Medium, false);
					CsUIData.Instance.SetButton(trSlot, () => CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(csItem));

					nSlotIndex++;

					nExp -= (nCount * csItem.Value1);
				}
			}

			CsUIData.Instance.SetText(transform.Find("PopupRelease/ImageBackground/TextContent"), "A146_TXT_00028", true);
		}

		transform.Find("PopupRelease").gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	void ClosePopupRelease()
	{
		transform.Find("PopupRelease").gameObject.SetActive(false);
	}

	#region event

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonOKPopupRelease()
	{
		if (CsGameEventUIToUI.Instance.OnEventGetSelectedCreature() == CsCreatureManager.Instance.ParticipatedCreatureId)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00045"));

			return;
		}

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null && csHeroCreature.Cheered)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00044"));

			return;
		}

		ClosePopupRelease();

		CsCreatureManager.Instance.SendCreatureRelease(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCancelPopupRelease()
	{
		ClosePopupRelease();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSkill(int nSlotIndex)
	{
		Transform trImageBackground = transform.Find("PopupSkillInfo/ImageBackground");

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			if (nSlotIndex < CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + csHeroCreature.AdditionalOpenSkillSlotCount)
			{
				CsHeroCreatureSkill csHeroCreatureSkill = csHeroCreature.HeroCreatureSkillList.Find(skill => skill.SlotIndex == nSlotIndex);

				if (csHeroCreatureSkill != null)
				{
					Text textLevelName = trImageBackground.Find("TextLevelName").GetComponent<Text>();
					CsUIData.Instance.SetFont(textLevelName);
					textLevelName.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_00027"), csHeroCreatureSkill.CreatureSkillGrade.SkillGrade, csHeroCreatureSkill.CreatureSkillGrade.ColorCode, csHeroCreatureSkill.CreatureSkill.Name);

					CsCreatureSkillAttr csCreatureSkillAttr = csHeroCreatureSkill.CreatureSkill.GetCreatureSkillAttr(csHeroCreatureSkill.CreatureSkillGrade.SkillGrade);

					if (csCreatureSkillAttr != null)
					{
						Text textDescription = trImageBackground.Find("TextDescription").GetComponent<Text>();
						CsUIData.Instance.SetFont(textDescription);
						textDescription.text = string.Format(csHeroCreatureSkill.CreatureSkill.EffectText, csHeroCreatureSkill.CreatureSkill.Attr.Name, csCreatureSkillAttr.AttrValue.Value.ToString());
					}

					transform.Find("PopupSkillInfo").gameObject.SetActive(true);

					RectTransform rtfImageBackground = transform.Find("PopupSkillInfo/ImageBackground").GetComponent<RectTransform>();
					rtfImageBackground.anchoredPosition = new Vector2(-110 + -90 * (csHeroCreatureSkill.SlotIndex / 3 == 0 ? 1 : 0), -160 + -90 * (csHeroCreatureSkill.SlotIndex % 3));
				}
			}
			else
			{
				CsCreatureSkillSlotOpenRecipe csCreatureSkillSlotOpenRecipe = CsGameData.Instance.GetCreatureSkillSlotOpenRecipe(CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + csHeroCreature.AdditionalOpenSkillSlotCount);

				if (csCreatureSkillSlotOpenRecipe != null)
				{
					CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("A146_TXT_00035"), csCreatureSkillSlotOpenRecipe.RequiredItemCount, csCreatureSkillSlotOpenRecipe.ItemRequired.Name),
																CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), 
																() => 
																{
																	if (CsGameData.Instance.MyHeroInfo.GetItemCount(csCreatureSkillSlotOpenRecipe.ItemRequired.ItemId) < csCreatureSkillSlotOpenRecipe.RequiredItemCount)
																	{
																		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A146_TXT_00036"), csCreatureSkillSlotOpenRecipe.ItemRequired.Name));
																	}
																	else
																	{
																		CsCreatureManager.Instance.SendCreatureSkillSlotOpen(csHeroCreature.InstanceId);
																	}
																},
																CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonBook()
	{
		Transform trPopupList = transform.parent.parent.parent.Find("PopupList");

		Transform trCreatureBook = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/PopupCreatureBook"), trPopupList).transform;

		CsPopupCreatureBook csPopupCreatureBook = trCreatureBook.GetComponent<CsPopupCreatureBook>();
		csPopupCreatureBook.EventClosePopupCreatureBook += OnEventClosePopupCreatureBook;

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			csPopupCreatureBook.SetCreature(csHeroCreature.Creature);
		}
		
		transform.Find("3DCreature").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRelease()
	{
		if (CsCreatureManager.Instance.HeroCreatureList.Count <= 1)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00032"));

			return;
		}

		OpenPopupRelease();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPopupVariation()
	{
		transform.Find("PopupVariation").gameObject.SetActive(true);

		UpdatePopupVariation(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonVariation()
	{
		CsCreatureManager.Instance.SendCreatureVary(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPopupRear()
	{
		transform.Find("PopupRear").gameObject.SetActive(true);

		UpdatePopupRear();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPopupSwitch()
	{
		transform.Find("PopupSwitch").gameObject.SetActive(true);

		UpdatePopupSwitch(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSwitch()
	{
		CsCreatureManager.Instance.SendCreatureAdditionalAttrSwitch(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonCheer()
	{
		if (CsCreatureManager.Instance.HeroCreatureList.FindAll(csHeroCreature => csHeroCreature.Cheered).Count >= CsGameConfig.Instance.CreatureCheerMaxCount)
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_03002"));
			return;
		}

		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsCreatureManager.Instance.SendCreatureCheer(guidCreatureInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonParticipate()
	{
		if (!CsCreatureManager.Instance.ParticipatedCreatureId.Equals(Guid.Empty))
		{
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_03001"));
			return;
		}

		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsCreatureManager.Instance.SendCreatureParticipate(guidCreatureInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRest()
	{
		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			if (csHeroCreature.Cheered)
			{
				CsCreatureManager.Instance.SendCreatureCheerCancel(guidCreatureInstanceId);
			}
			else if (guidCreatureInstanceId.Equals(CsCreatureManager.Instance.ParticipatedCreatureId))
			{
				CsCreatureManager.Instance.SendCreatureParticipationCancel(guidCreatureInstanceId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickBackgroundPopupSkillInfo()
	{
		transform.Find("PopupSkillInfo").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickBackgroundPopupRear()
	{
		transform.Find("PopupRear").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSelectCreatureToggle(Guid guidCreatureInstanceId)
	{
		if (!m_bInitialized)
			return;

		CsHeroCreature selectedHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);
		UpdateCreatureInfo(selectedHeroCreature);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerDownCreatureFood(int nItemId)
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;
		}

		m_nSelectedItemId = nItemId;
		m_coroutineCheckEvent = StartCoroutine(CheckEvent());
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerUpCreatureFood()
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;
		}

		m_nSelectedItemId = 0;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerExitCreatureFood()
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;
		}

		m_nSelectedItemId = 0;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClosePopupCreatureBook()
	{
		transform.Find("3DCreature").gameObject.SetActive(true);
	}
	
	#endregion event

	#region protocol.event

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipate()
	{
		UpdateButtons(CsCreatureManager.Instance.ParticipatedCreatureId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipationCancel()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			UpdateButtons(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheer()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			UpdateButtons(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheerCancel()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			UpdateButtons(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureRear()
	{
		UpdatePopupRear();

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(CsGameEventUIToUI.Instance.OnEventGetSelectedCreature());

		if (csHeroCreature != null)
		{
			UpdateNoticeButtonPopupRear(csHeroCreature);
			UpdateCreatureLevel(csHeroCreature);
			UpdateCreatureExp(csHeroCreature);
			UpdateCreatureAttributes(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureVariation()
	{
		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			UpdatePopupVariation(guidCreatureInstanceId);
			UpdateCreatureGrade(csHeroCreature);
			UpdateCreatureQuality(csHeroCreature);
			UpdateNoticeButtonPopupVaritaion(csHeroCreature);
			UpdateCreatureAttributes(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureAdditionalAttrSwitch()
	{
		UpdatePopupSwitch(false);

		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			UpdateCreatureAdditionalAttributes(csHeroCreature);
			UpdateNoticeButtonPopupSwitch();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureSkillSlotOpen()
	{
		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			UpdateCreatureSkills(csHeroCreature);
		}
	}

	#endregion protocol.event
}
