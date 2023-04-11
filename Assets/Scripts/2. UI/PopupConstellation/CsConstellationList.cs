using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-10-04)
//---------------------------------------------------------------------------------------------------

public class CsConstellationList : CsPopupSub
{
	const int m_nStepCount = 4;

	CsConstellation m_SelectedConstellation = null;
	int m_nSelectedStep = 0;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize() 
	{
		CsConstellationManager.Instance.EventConstellationEntryActivate += OnEventConstellationEntryActivate;
		CsConstellationManager.Instance.EventConstellationStepOpen += OnEventConstellationStepOpen;
		CsConstellationManager.Instance.EventConstellationOpened += OnEventConstellationOpened;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsConstellationManager.Instance.EventConstellationEntryActivate -= OnEventConstellationEntryActivate;
		CsConstellationManager.Instance.EventConstellationStepOpen -= OnEventConstellationStepOpen;
		CsConstellationManager.Instance.EventConstellationOpened -= OnEventConstellationOpened;

	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		GameObject goConstellationItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/ConstellationItem");

		Transform trConstellationLayout = transform.Find("ConstellationLayout");

		foreach (CsConstellation csConstellation in CsGameData.Instance.ConstellationList)
		{
			Transform trConstellation = Instantiate(goConstellationItem, trConstellationLayout).transform;
			trConstellation.name = csConstellation.ConstellationId.ToString();

			CsUIData.Instance.SetButton(trConstellation, () => OnClickConstellation(csConstellation));
			CsUIData.Instance.SetImage(trConstellation.Find("ImageSquare/ImageIcon"), "GUI/PopupConstellation/ico_constellation_" + csConstellation.ConstellationId.ToString());
			CsUIData.Instance.SetText(trConstellation.Find("Text"), csConstellation.Name, false);

			CsHeroConstellation csHeroConstellation = CsConstellationManager.Instance.GetHeroConstellation(csConstellation.ConstellationId);
			Button button = trConstellation.GetComponent<Button>();
			button.interactable = csHeroConstellation != null && csHeroConstellation.HeroConstellationStepList.Count > 0;

			trConstellation.Find("ImageSquare/ImageLock").gameObject.SetActive(csHeroConstellation == null || csHeroConstellation.HeroConstellationStepList.Count <= 0);
			trConstellation.Find("TextLock").gameObject.SetActive(csHeroConstellation == null || csHeroConstellation.HeroConstellationStepList.Count <= 0);
			
			string strRequiredCondition;
			
			if (csConstellation.RequiredConditionType == 1)
			{
				strRequiredCondition = string.Format(CsConfiguration.Instance.GetString("A163_TXT_00021"), csConstellation.RequiredConditionValue);
			}
			else
			{
				strRequiredCondition = string.Format(CsConfiguration.Instance.GetString("A163_TXT_00022"), csConstellation.RequiredConditionValue);
			}

			CsUIData.Instance.SetText(trConstellation.Find("TextLock"), strRequiredCondition, false);
		}

		UpdateConstellationAttribute();
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateConstellationAttribute()
	{
		Transform trConstellationAttr = transform.Find("ConstellationAttr");

		CsUIData.Instance.SetText(trConstellationAttr.Find("ImageBackground/ImageGlow/TextAttr"), "A163_TXT_00001", true);

		// 사이클버프, 엔트리버프 속성별 총합
		Dictionary<CsAttr, int> dicConstellationAttr = new Dictionary<CsAttr,int>();

		foreach (CsHeroConstellation csHeroConstellation in CsConstellationManager.Instance.HeroConstellationList)
		{
			foreach (CsHeroConstellationStep csHeroConstellationStep in csHeroConstellation.HeroConstellationStepList)
			{
				// 사이클 버프
				CsConstellationManager.Instance.GetCycleBuff(ref dicConstellationAttr, csHeroConstellation.Id, csHeroConstellationStep.Step);

				// 엔트리 버프\
				CsConstellationManager.Instance.GetEntryBuff(ref dicConstellationAttr, csHeroConstellation.Id, csHeroConstellationStep.Step);
			}
		}

		// 속성 세팅
		Transform trConstellationAttrList = trConstellationAttr.Find("ImageBackground/ConstellationAttrList");

		for (int i = 0; i < trConstellationAttrList.childCount; i++)
		{
			trConstellationAttrList.GetChild(i).gameObject.SetActive(false);
			trConstellationAttrList.GetChild(i).name = "";
		}

		int nChildCount = 0;

		GameObject goPermanentIncreaseAttr = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/PermanentIncreaseAttr");

		foreach (CsAttr csAttr in dicConstellationAttr.Keys)
		{
			if (dicConstellationAttr[csAttr] <= 0)
				continue;

			Transform trAttr = null;

			if (nChildCount < trConstellationAttrList.childCount)
			{
				trAttr = trConstellationAttrList.GetChild(nChildCount);
				trAttr.gameObject.SetActive(true);
			}
			else
			{
				trAttr = Instantiate(goPermanentIncreaseAttr, trConstellationAttrList).transform;
			}

			trAttr.name = csAttr.AttrId.ToString();

			CsUIData.Instance.SetText(trAttr.Find("Text"), csAttr.Name, false);
			CsUIData.Instance.SetText(trAttr.Find("TextDetail"), dicConstellationAttr[csAttr].ToString("#,##0"), false);

			nChildCount++;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDisplay()
	{
		CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		Transform trConstellationDetail = transform.Find("ConstellationDetail");

		CsUIData.Instance.SetText(trConstellationDetail.Find("ButtonPrev/Description/TextLevel"), string.Format(CsConfiguration.Instance.GetString("A163_TXT_00002"), m_nSelectedStep.ToString()), false);

		CsUIData.Instance.SetImage(trConstellationDetail.Find("ConstellationMap/" + m_SelectedConstellation.ConstellationId.ToString() + "/ImageBackground"), "GUI/PopupConstellation/frm_constellation_back_" + m_nSelectedStep.ToString());

		UpdateStarEssence();

		UpdateToggleStep();

		UpdateStarIcon();

		UpdateAttrInfo();
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateStarIcon()
	{
		CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		Transform trConstellation = transform.Find("ConstellationDetail/ConstellationMap/" + m_SelectedConstellation.ConstellationId.ToString());
		Transform trIconList = trConstellation.Find("IconList");

		List<Sprite> listStarIcon = new List<Sprite>();

		listStarIcon.Add(CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupConstellation/ico_constellation_star_off"));
		listStarIcon.Add(CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupConstellation/ico_constellation_star_1"));
		listStarIcon.Add(CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupConstellation/ico_constellation_star_2"));
		listStarIcon.Add(CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupConstellation/ico_constellation_star_3"));

		for (int i = 0; i < trIconList.childCount; i++)
		{
			int nEntryNo = i + 1;

			Transform trIcon = trIconList.Find("Icon" + i.ToString());

			if (csHeroConstellationStep.Activated)
			{
				trIcon.GetComponent<Image>().sprite = listStarIcon[3];
			}
			else if (nEntryNo < csHeroConstellationStep.EntryNo)
			{
				trIcon.GetComponent<Image>().sprite = listStarIcon[csHeroConstellationStep.Cycle];
			}
			else
			{
				trIcon.GetComponent<Image>().sprite = listStarIcon[csHeroConstellationStep.Cycle - 1];
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateToggleStep()
	{
		Transform trLevelLayout = transform.Find("ConstellationDetail/LevelLayout");

		for (int i = 0; i < m_nStepCount; i++)
		{
			int nStepNo = i + 1;

			Transform trToggle = trLevelLayout.Find(nStepNo.ToString());

			CsConstellationStep csConstellationStep = m_SelectedConstellation.GetConstellationStep(nStepNo);

			CsHeroConstellation csHeroConstellation = CsConstellationManager.Instance.GetHeroConstellation(m_SelectedConstellation.ConstellationId);

			if (trToggle != null && csConstellationStep != null && csHeroConstellation != null)
			{
				CsHeroConstellationStep csHeroConstellationStep = csHeroConstellation.HeroConstellationStepList.Find(step => step.Step == nStepNo);

				if (csHeroConstellationStep == null)
				{
					// 단계 미개방
					CsHeroConstellationStep heroPrevStep = csHeroConstellation.GetHeroConstellationStep(nStepNo - 1);

					// 활성화되지 않은 경우 조건 검사
					if (heroPrevStep != null && heroPrevStep.Cycle > 1)
					{
						trToggle.Find("Description").gameObject.SetActive(true);
						trToggle.Find("Description/ImageDia").gameObject.SetActive(true);

						CsUIData.Instance.SetText(trToggle.Find("Description/TextDesc"), csConstellationStep.RequiredDia.ToString("#,##0"), false);

						trToggle.Find("ImageDim").gameObject.SetActive(false);

						UpdateNoticeStepToggle(true, nStepNo);
					}
					else
					{
						trToggle.Find("Description").gameObject.SetActive(false);

						trToggle.Find("ImageDim").gameObject.SetActive(true);

						CsUIData.Instance.SetText(trToggle.Find("ImageDim/TextDesc"), string.Format(CsConfiguration.Instance.GetString("A163_TXT_00006"), nStepNo - 1), false);

						UpdateNoticeStepToggle(false, nStepNo);
					}
				}
				else
				{
					// 단계 개방
					trToggle.Find("Description").gameObject.SetActive(true);
					trToggle.Find("Description/ImageDia").gameObject.SetActive(false);

					if (csHeroConstellationStep.Activated)
					{
						CsUIData.Instance.SetText(trToggle.Find("Description/TextDesc"), "A163_TXT_00004", true);
					}
					else
					{
						CsUIData.Instance.SetText(trToggle.Find("Description/TextDesc"), "A163_TXT_00005", true);
					}

					trToggle.Find("ImageDim").gameObject.SetActive(false);

					UpdateNoticeStepToggle(false, nStepNo);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateAttrInfo()
	{
		Transform trAttrInfo = transform.Find("ConstellationDetail/AttrInfo");

		Transform trAttrLayout = trAttrInfo.Find("AttrLayout");

		Dictionary<CsAttr, int> dicConstellationAttr = new Dictionary<CsAttr, int>();
		Dictionary<CsAttr, int> dicConstellationAttrNextLevel = new Dictionary<CsAttr, int>();

		CsConstellationManager.Instance.GetCycleBuff(ref dicConstellationAttr, m_SelectedConstellation.ConstellationId, m_nSelectedStep);
		CsConstellationManager.Instance.GetEntryBuff(ref dicConstellationAttr, m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		CsConstellationManager.Instance.GetNextEntryCycleBuff(ref dicConstellationAttrNextLevel, m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		GameObject goAttr = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/ConstellationAttr");

		for (int i = 0; i < trAttrLayout.childCount; i++)
		{
			trAttrLayout.GetChild(i).gameObject.SetActive(false);
			trAttrLayout.GetChild(i).name = "";
		}

		int nChildIndex = 0;

		foreach (CsAttr csAttr in CsGameData.Instance.AttrList)
		{
			Transform trAttr = null;

			int nCurrentValue = dicConstellationAttr.ContainsKey(csAttr) ? dicConstellationAttr[csAttr] : 0;
			int nNextValue = dicConstellationAttrNextLevel.ContainsKey(csAttr) ? dicConstellationAttrNextLevel[csAttr] : 0;

			if (nCurrentValue == 0 && nNextValue == 0)
				continue;

			if (nChildIndex < trAttrLayout.childCount)
			{
				trAttr = trAttrLayout.GetChild(nChildIndex);
				trAttr.gameObject.SetActive(true);
			}
			else
			{
				trAttr = Instantiate(goAttr, trAttrLayout).transform;
			}

			trAttr.name = csAttr.AttrId.ToString();

			CsUIData.Instance.SetText(trAttr.Find("TextTitle"), csAttr.Name, false);
			CsUIData.Instance.SetText(trAttr.Find("TextCurrentAttr"), nCurrentValue.ToString(), false);

			trAttr.Find("ImageUp").gameObject.SetActive(nNextValue > 0);
			trAttr.Find("TextBonusAttr").gameObject.SetActive(nNextValue > 0);

			if (nNextValue > 0)
			{
				CsUIData.Instance.SetText(trAttr.Find("TextBonusAttr"), (nNextValue - nCurrentValue).ToString(), false);
			}

			nChildIndex++;
		}

		// 활성조건
		CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		trAttrInfo.Find("FrameNonActivation").gameObject.SetActive(!csHeroConstellationStep.Activated);
		trAttrInfo.Find("FrameActivation").gameObject.SetActive(csHeroConstellationStep.Activated);

		if (!csHeroConstellationStep.Activated)
		{
			Transform trActiveCondition = trAttrInfo.Find("FrameNonActivation/ActiveCondition");
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionLevel/TextDetail"), string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), m_SelectedConstellation.RequiredConditionValue.ToString()), false);
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionSoul/TextDetail"), csHeroConstellationStep.ConstellationEntry.RequiredStarEssense.ToString("#,##0"), false);
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionRuppy/TextDetail"), csHeroConstellationStep.ConstellationEntry.RequiredGold.ToString("#,##0"), false);

			Transform trActiveRate = trAttrInfo.Find("FrameNonActivation/ActiveRate");
			CsUIData.Instance.SetText(trActiveRate.Find("TextPercent"), string.Format(CsConfiguration.Instance.GetString("A163_TXT_00013"), (csHeroConstellationStep.ConstellationEntry.SuccessRate / 100.0f).ToString("#.#0"), (csHeroConstellationStep.FailPoint / 100.0f).ToString("#0.0")), false);
			
			Transform trButtonActive = trAttrInfo.Find("FrameNonActivation/ButtonActive");
			Button buttonActive = trButtonActive.GetComponent<Button>();
			buttonActive.interactable = !csHeroConstellationStep.Activated && csHeroConstellationStep.ConstellationEntry.RequiredGold <= CsGameData.Instance.MyHeroInfo.Gold && csHeroConstellationStep.ConstellationEntry.RequiredStarEssense <= CsConstellationManager.Instance.StarEssense;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateStarEssence()
	{
		Transform trConstellationDetail = transform.Find("ConstellationDetail");
		CsUIData.Instance.SetText(trConstellationDetail.Find("UserSoul/TextSoul"), CsConstellationManager.Instance.StarEssense.ToString("#,##0"), false);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateNoticeStepToggle(bool bOpenable, int nStepNo)
	{
		Transform trImageNew = transform.Find("ConstellationDetail/LevelLayout/" + nStepNo.ToString() + "/ImageNew");

		if (bOpenable)
		{
			trImageNew.gameObject.SetActive(m_SelectedConstellation.GetConstellationStep(nStepNo).RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia);
		}
		else
		{
			trImageNew.gameObject.SetActive(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupBonus()
	{
		Transform trPopupBonus = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/PopupConstellationBonus"), transform).transform;

		trPopupBonus.name = "PopupConstellationBonus";

		Transform trImagePopup = trPopupBonus.Find("ImagePopup");

		CsUIData.Instance.SetText(trImagePopup.Find("TextTitle"), "A163_TXT_00007", true);
		CsUIData.Instance.SetButton(trImagePopup.Find("ButtonEsc"), ClosePopupBonus);
		CsUIData.Instance.SetText(trImagePopup.Find("TextDesc"), "A163_TXT_00015", true);

		CsConstellationCycle cycleLeft = null;
		CsConstellationCycle cycleRight = null;

		CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		if (csHeroConstellationStep != null)
		{
			if (csHeroConstellationStep.Cycle <= 1)
			{
				cycleLeft = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(1);
				cycleRight = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(2);
			}
			else
			{
				cycleLeft = csHeroConstellationStep.ConstellationCycle;
				cycleRight = csHeroConstellationStep.ConstellationStep.GetConstellationCycle(csHeroConstellationStep.Cycle + 1);
			}

			GameObject goAttr = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/ConstellationBonusAttr");

			if (cycleLeft != null)
			{
				Transform trActiveConstellation = trImagePopup.Find("ActiveConstellation");

				if (cycleLeft.Cycle == csHeroConstellationStep.Cycle - 1)
				{
					CsUIData.Instance.SetText(trActiveConstellation.Find("TextTitle"),
						string.Format(CsConfiguration.Instance.GetString("A163_TXT_00016"), m_SelectedConstellation.Name, cycleLeft.Cycle), false);

					Text textTitle = trActiveConstellation.Find("TextTitle").GetComponent<Text>();
					textTitle.color = new Color32(255, 214, 80, 255);
				}
				else
				{
					CsUIData.Instance.SetText(trActiveConstellation.Find("TextTitle"),
					string.Format(CsConfiguration.Instance.GetString("A163_TXT_00017"), m_SelectedConstellation.Name, cycleLeft.Cycle), false);

					Text textTitle = trActiveConstellation.Find("TextTitle").GetComponent<Text>();
					textTitle.color = new Color(133, 141, 148, 255);
				}
				
				Transform trAttrList = trActiveConstellation.Find("AttrList");

				foreach (CsConstellationCycleBuff csConstellationCycleBuff in cycleLeft.ConstellationCycleBuffList)
				{
					if (csConstellationCycleBuff.AttrValue.Value <= 0)
						continue;

					Transform trAttr = Instantiate(goAttr, trAttrList).transform;
					trAttr.name = csConstellationCycleBuff.Attr.AttrId.ToString();

					CsUIData.Instance.SetText(trAttr.Find("TextName"), csConstellationCycleBuff.Attr.Name, false);
					CsUIData.Instance.SetText(trAttr.Find("TextDesc"), csConstellationCycleBuff.AttrValue.Value.ToString("#,##0"), false);
				}
			}

			if (cycleRight != null)
			{
				Transform trInActiveConstellation = trImagePopup.Find("InActiveConstellation");

				CsUIData.Instance.SetText(trInActiveConstellation.Find("TextTitle"),
					string.Format(CsConfiguration.Instance.GetString("A163_TXT_00017"), m_SelectedConstellation.Name, cycleRight.Cycle), false);

				Text textTitle = trInActiveConstellation.Find("TextTitle").GetComponent<Text>();
				textTitle.color = new Color(133, 141, 148, 255);

				Transform trAttrList = trInActiveConstellation.Find("AttrList");

				foreach (CsConstellationCycleBuff csConstellationCycleBuff in cycleRight.ConstellationCycleBuffList)
				{
					if (csConstellationCycleBuff.AttrValue.Value <= 0)
						continue;

					Transform trAttr = Instantiate(goAttr, trAttrList).transform;
					trAttr.name = csConstellationCycleBuff.Attr.AttrId.ToString();

					CsUIData.Instance.SetText(trAttr.Find("TextName"), csConstellationCycleBuff.Attr.Name, false);
					CsUIData.Instance.SetText(trAttr.Find("TextDesc"), csConstellationCycleBuff.AttrValue.Value.ToString("#,##0"), false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ClosePopupBonus()
	{
		Transform trPopupConstellationBonus = transform.Find("PopupConstellationBonus");

		if (trPopupConstellationBonus != null)
		{
			Destroy(trPopupConstellationBonus.gameObject);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickConstellation(CsConstellation csConstellation)
	{
		m_SelectedConstellation = csConstellation;

		transform.Find("ConstellationLayout").gameObject.SetActive(false);
		transform.Find("ConstellationAttr").gameObject.SetActive(false);

		Transform trConstellationDetail = transform.Find("ConstellationDetail");
		Transform trLevelLayout = null;

		if (trConstellationDetail == null)
		{
			trConstellationDetail = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/ConstellationDetail"), transform).transform;
			trConstellationDetail.name = "ConstellationDetail";

			CsUIData.Instance.SetButton(trConstellationDetail.Find("ButtonPrev"), OnClickButtonPrev);

			CsUIData.Instance.SetButton(trConstellationDetail.Find("AttrInfo/ButtonDetail"), OnClickButtonDetail);
			CsUIData.Instance.SetText(trConstellationDetail.Find("AttrInfo/ButtonDetail/Text"), "A163_TXT_00007", true);

			Transform trActiveCondition = trConstellationDetail.Find("AttrInfo/FrameNonActivation/ActiveCondition");
			CsUIData.Instance.SetText(trActiveCondition.Find("ImageBackground/TextActive"), "A163_TXT_00008", true);
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionLevel/TextInfo"), "A163_TXT_00009", true);
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionSoul/TextInfo"), "A163_TXT_00010", true);
			CsUIData.Instance.SetText(trActiveCondition.Find("ConditionRuppy/TextInfo"), "A163_TXT_00011", true);

			CsUIData.Instance.SetText(trConstellationDetail.Find("AttrInfo/FrameNonActivation/ActiveRate/TextRate"), "A163_TXT_00012", true);
			CsUIData.Instance.SetButton(trConstellationDetail.Find("AttrInfo/FrameNonActivation/ButtonActive"), OnClickButtonActive);
			CsUIData.Instance.SetText(trConstellationDetail.Find("AttrInfo/FrameNonActivation/ButtonActive/Text"), "A163_TXT_00014", true);
			CsUIData.Instance.SetText(trConstellationDetail.Find("AttrInfo/FrameActivation/TextDescription"), "A163_TXT_00018", true);

			CsUIData.Instance.SetText(trConstellationDetail.Find("Description/TextDesc"), "A163_TXT_00003", true);

			trLevelLayout = trConstellationDetail.Find("LevelLayout");

			if (trLevelLayout != null)
			{
				for (int i = 0; i < m_nStepCount; i++)
				{
					int nStep = i + 1;

					Transform trToggle = trLevelLayout.Find(nStep.ToString());

					CsUIData.Instance.SetText(trToggle.Find("TextLevel"), string.Format(CsConfiguration.Instance.GetString("A163_TXT_00002"), nStep), false);

					Toggle toggle = trToggle.GetComponent<Toggle>();
					toggle.onValueChanged.RemoveAllListeners();
					toggle.onValueChanged.AddListener((bIsOn) => OnValueChangedToggleConstellationStep(bIsOn, nStep));
				}
			}
		}
		else
		{
			trConstellationDetail.gameObject.SetActive(true);
		}

		Transform trConstellationMap = trConstellationDetail.Find("ConstellationMap");
		Transform trConstellation = trConstellationMap.Find(csConstellation.ConstellationId.ToString());

		if (trConstellation == null)
		{
			trConstellation = Instantiate(CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupConstellation/Constellation" + csConstellation.ConstellationId.ToString()), trConstellationMap).transform;
			trConstellation.name = csConstellation.ConstellationId.ToString();
		}
		else
		{
			trConstellation.gameObject.SetActive(true);
		}

		CsUIData.Instance.SetImage(trConstellationDetail.Find("ButtonPrev/ImageConstellation"), "GUI/PopupConstellation/ico_constellation_" + csConstellation.ConstellationId.ToString());
		CsUIData.Instance.SetText(trConstellationDetail.Find("ButtonPrev/Description/TextConstellation"), csConstellation.Name, false);

		trLevelLayout = trConstellationDetail.Find("LevelLayout");

		if (trLevelLayout != null)
		{
			trLevelLayout.GetChild(0).GetComponent<Toggle>().isOn = false;
			trLevelLayout.GetChild(0).GetComponent<Toggle>().isOn = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonPrev()
	{
		Transform trConstellationDetail = transform.Find("ConstellationDetail");

		Transform trConstellationMap = trConstellationDetail.Find("ConstellationMap");

		for (int i = 0; i < trConstellationMap.childCount; i++)
		{
			trConstellationMap.GetChild(i).gameObject.SetActive(false);
		}

		trConstellationDetail.gameObject.SetActive(false);

		transform.Find("ConstellationLayout").gameObject.SetActive(true);
		transform.Find("ConstellationAttr").gameObject.SetActive(true);

		UpdateConstellationAttribute();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonDetail()
	{
		OpenPopupBonus();
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonActive()
	{
		CsHeroConstellationStep csHeroConstellationStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, m_nSelectedStep);

		CsConstellationManager.Instance.SendConstellationEntryActivate(csHeroConstellationStep.Constellation.ConstellationId, csHeroConstellationStep.Step, csHeroConstellationStep.Cycle, csHeroConstellationStep.EntryNo);
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleConstellationStep(bool bIsOn, int nStepNo)
	{
		if (bIsOn)
		{
			if (CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, nStepNo) == null)
			{
				transform.Find("ConstellationDetail/LevelLayout/" + m_nSelectedStep.ToString()).GetComponent<Toggle>().isOn = true;

				CsHeroConstellationStep heroPrevStep = CsConstellationManager.Instance.GetHeroConstellationStep(m_SelectedConstellation.ConstellationId, nStepNo - 1);

				if (heroPrevStep != null && heroPrevStep.Cycle > 1)
				{
					CsConstellationStep csConstellationStep = m_SelectedConstellation.GetConstellationStep(nStepNo);

					if (csConstellationStep.RequiredDia <= CsGameData.Instance.MyHeroInfo.Dia)
					{
						CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A163_TXT_00019"),
							CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsConstellationManager.Instance.SendConstellationStepOpen(m_SelectedConstellation.ConstellationId, nStepNo),
						CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
					}
				}

				return;
			}

			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			m_nSelectedStep = nStepNo;

			UpdateDisplay();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationEntryActivate(bool bSuccess)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString(bSuccess ? "A163_TXT_00023" : "A163_TXT_00024"));

		UpdateStarEssence();

		UpdateToggleStep();

		UpdateStarIcon();

		UpdateAttrInfo();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationStepOpen(int nStepNo)
	{
		CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A163_TXT_00020"), nStepNo));

		UpdateToggleStep();

		//transform.Find("ConstellationDetail/LevelLayout/" + nStepNo.ToString()).GetComponent<Toggle>().isOn = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventConstellationOpened()
	{
		Transform trConstellationLayout = transform.Find("ConstellationLayout");

		foreach (CsConstellation csConstellation in CsGameData.Instance.ConstellationList)
		{
			Transform trConstellation = trConstellationLayout.Find(csConstellation.ConstellationId.ToString());

			if (trConstellationLayout != null)
			{
				CsHeroConstellation csHeroConstellation = CsConstellationManager.Instance.GetHeroConstellation(csConstellation.ConstellationId);
				Button button = trConstellation.GetComponent<Button>();
				button.interactable = csHeroConstellation != null && csHeroConstellation.HeroConstellationStepList.Count > 0;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
}
