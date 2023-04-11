using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class CsPopupCreatureInjection : CsPopupSub 
{
	bool m_bInitialized = false;
	bool m_bAutoInjection = false;
	Coroutine m_coroutineSetContents = null;

	float m_flAutoInjectionDelay = 0.1f;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsCreatureManager.Instance.EventCreatureInject += OnEventCreatureInject;
		CsCreatureManager.Instance.EventCreatureInjectionRetrieval += OnEventCreatureInjectionRetrieval;

		CsGameEventUIToUI.Instance.EventSelectCreatureToggle += OnEventSelectCreatureToggle;

		InitializeUI();

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsCreatureManager.Instance.EventCreatureInject -= OnEventCreatureInject;
		CsCreatureManager.Instance.EventCreatureInjectionRetrieval -= OnEventCreatureInjectionRetrieval;

		CsGameEventUIToUI.Instance.EventSelectCreatureToggle -= OnEventSelectCreatureToggle;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		// 회수버튼
		CsUIData.Instance.SetButton(transform.Find("ButtonRetrieval"), OnClickButtonRetrieval);
		CsUIData.Instance.SetText(transform.Find("ButtonRetrieval/TextRetrieval"), "A146_BTN_00009", true);

		// 에너지주입 버튼
		CsUIData.Instance.SetButton(transform.Find("ButtonInjection"), OnClickButtonInjection);
		CsUIData.Instance.SetText(transform.Find("ButtonInjection/TextInjection"), "A146_BTN_00010", true);

		CsUIData.Instance.SetButton(transform.Find("ButtonInjectionAll"), OnClickButtonInjectionAll);
		CsUIData.Instance.SetText(transform.Find("ButtonInjectionAll/TextInjectionAll"), "A146_BTN_00011", true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		m_bAutoInjection = false;

		CsGameEventUIToUI.Instance.OnEventCreatureSubMenuChanged();

		if (m_coroutineSetContents != null)
		{
			StopCoroutine(m_coroutineSetContents);
			m_coroutineSetContents = null;
		}

		m_coroutineSetContents = StartCoroutine(SetContents());
	}

	//---------------------------------------------------------------------------------------------------
	void OnDisable()
	{
		m_bAutoInjection = false;
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
		
		UpdateCreatureInjectionInfo(csHeroCreature);
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
	void UpdateCreatureInjectionInfo(CsHeroCreature csHeroCreature)
	{
		Transform trFrameGauge = transform.Find("FrameGauge");
		CsUIData.Instance.SetText(trFrameGauge.Find("TextInjectionValue"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), csHeroCreature.InjectionExp, csHeroCreature.CreatureInjectionLevel.NextLevelUpRequiredExp), false);

		Slider slider = trFrameGauge.Find("Slider").GetComponent<Slider>();
		slider.value = (float)csHeroCreature.InjectionExp / csHeroCreature.CreatureInjectionLevel.NextLevelUpRequiredExp;

		Transform trFrameAdditionalAttribute = transform.Find("FrameAdditionalAttribute");

		int nChildIndex = 0;

		foreach (int nAttrId in csHeroCreature.AdditionalAttrIdList)
		{
			CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(nAttrId);

			if (csCreatureAdditionalAttr != null)
			{
				CsCreatureAdditionalAttrValue currentAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.InjectionLevel);
				CsCreatureAdditionalAttrValue nextAttrValue = csCreatureAdditionalAttr.GetCreatureAdditionalAttrValue(csHeroCreature.InjectionLevel + 1);

				if (currentAttrValue != null)
				{
					Transform trAttribute = trFrameAdditionalAttribute.GetChild(nChildIndex);
					CsUIData.Instance.SetImage(trAttribute.Find("ImageIcon"), GetAttributeImagePath(nAttrId));
					CsUIData.Instance.SetText(trAttribute.Find("TextName"), csCreatureAdditionalAttr.Attr.Name, false);
					CsUIData.Instance.SetText(trAttribute.Find("TextCurrentValue"), currentAttrValue.AttrValue.Value.ToString(), false);

					trAttribute.Find("ImageIconArrow").gameObject.SetActive(nextAttrValue != null);

					if (nextAttrValue != null)
					{
						CsUIData.Instance.SetText(trAttribute.Find("TextNextValue"), nextAttrValue.AttrValue.Value.ToString(), false);
					}
				}
			}

			nChildIndex++;
		}

		Transform trImageFrameRequiredItem = transform.Find("ImageFrameRequiredItem");

		CsItem csItem = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.CreatureEssence);

		if (csItem != null)
		{
			int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);

			CsUIData.Instance.SetImage(trImageFrameRequiredItem.Find("ImageIconRequiredItem"), "GUI/Items/" + csItem.Image);
			CsUIData.Instance.SetText(trImageFrameRequiredItem.Find("TextValueRequiredItem"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), nItemCount, csHeroCreature.CreatureInjectionLevel.RequiredItemCount), false);
			CsUIData.Instance.SetText(trImageFrameRequiredItem.Find("TextValueRequiredGold"), csHeroCreature.CreatureInjectionLevel.RequiredGold.ToString("#,##0"), false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButtons(CsHeroCreature csHeroCreature)
	{
		int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureEssence);

		bool bMaxLevel = csHeroCreature.InjectionLevel < csHeroCreature.CreatureLevel.MaxInjectionLevel && csHeroCreature.InjectionLevel < CsGameData.Instance.CreatureInjectionLevelList.Max(injectionLevel => injectionLevel.InjectionLevel);
		
		bool bEnoughWealth = nItemCount >= csHeroCreature.CreatureInjectionLevel.RequiredItemCount &&
							CsGameData.Instance.MyHeroInfo.Gold >= csHeroCreature.CreatureInjectionLevel.RequiredGold;

		transform.Find("ButtonInjection").GetComponent<Button>().interactable = bMaxLevel && bEnoughWealth;

		transform.Find("ButtonInjectionAll").GetComponent<Button>().interactable = bMaxLevel && bEnoughWealth;
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
	void SendCreatureInjection()
	{
		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureEssence);

			if (csHeroCreature.CreatureInjectionLevel.RequiredGold <= CsGameData.Instance.MyHeroInfo.Gold &&
				csHeroCreature.CreatureInjectionLevel.RequiredItemCount <= nItemCount)
			{
				CsCreatureManager.Instance.SendCreatureInject(guidCreatureInstanceId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CreatureAutoInjection()
	{
		yield return new WaitForSeconds(m_flAutoInjectionDelay);

		if (m_bAutoInjection)
		{
			SendCreatureInjection();
		}
	}

	#region event
	//---------------------------------------------------------------------------------------------------
	void OnClickButtonRetrieval()
	{
		m_bAutoInjection = false;

		Guid guidCreatureInstanceId = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();

		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		CsItem csItem = CsGameData.Instance.ItemList.Find(item => item.ItemType.EnItemType == EnItemType.CreatureEssence);

		if (csItem != null && csHeroCreature != null)
		{
			CsGameEventUIToUI.Instance.OnEventConfirm(string.Format(CsConfiguration.Instance.GetString("A146_TXT_00029"), csItem.Name, (int)(csHeroCreature.InjectionItemCount * (CsGameConfig.Instance.CreatureInjectionExpRetrivalRate / 10000.0f))),
												CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsCreatureManager.Instance.SendCreatureInjectionRetrieval(guidCreatureInstanceId),
												CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonInjection()
	{
		m_bAutoInjection = false;

		SendCreatureInjection();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonInjectionAll()
	{
		m_bAutoInjection = true;

		SendCreatureInjection();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSelectCreatureToggle(Guid guidCreatureInstanceId)
	{
		m_bAutoInjection = false;

		if (!m_bInitialized)
			return;

		CsHeroCreature selectedHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);
		UpdateCreatureInfo(selectedHeroCreature);
	}
	#endregion event

	#region protocol.event
	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureInject(bool bIsCritical, bool bLevelUp)
	{
		Guid guid = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guid);

		if (csHeroCreature != null)
		{
			UpdateCreatureLevel(csHeroCreature);
			UpdateCreatureInjectionInfo(csHeroCreature);
			UpdateButtons(csHeroCreature);
		}

		if (m_bAutoInjection)
		{
			if (bLevelUp)
			{
				m_bAutoInjection = false;
			}
			else
			{
				StartCoroutine(CreatureAutoInjection());
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureInjectionRetrieval()
	{
		Guid guid = CsGameEventUIToUI.Instance.OnEventGetSelectedCreature();
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guid);

		if (csHeroCreature != null)
		{
			UpdateCreatureLevel(csHeroCreature);
			UpdateCreatureInjectionInfo(csHeroCreature);
			UpdateButtons(csHeroCreature);
		}
	}

	#endregion protocol.event

}
