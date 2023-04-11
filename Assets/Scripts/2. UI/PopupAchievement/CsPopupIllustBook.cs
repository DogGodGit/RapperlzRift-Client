using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-04-19)
//---------------------------------------------------------------------------------------------------

public class CsPopupIllustBook : CsPopupSub
{
    Transform m_trExplorationDisplay;
    Transform m_trIllustratedBookDisplay;
    Transform m_trPopupList;
    Transform m_trItemInfo;
	Transform m_trCumulative;

    GameObject m_goCategory;
    GameObject m_goType;
    GameObject m_goBook;
    GameObject m_goPopupItemInfo;

    int m_nSelectExplorationStep;
    int m_nSelectBookType;
    int m_nSelectCategoryId;
    bool m_bFirst = true;

    CsPopupItemInfo m_csPopupItemInfo;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepAcquire += OnEventIllustratedBookExplorationStepAcquire;
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepRewardReceive += OnEventIllustratedBookExplorationStepRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }
        else
        {
            Transform trCategoryList = transform.Find("CategoryList");

            Toggle toggleExploration = trCategoryList.Find("ToggleIllustBookCategory").GetComponent<Toggle>();
            toggleExploration.isOn = true;

            for (int i = 0; i < trCategoryList.childCount; i++)
            {
                if (trCategoryList.GetChild(i).name != "ToggleIllustBookCategory")
                {
                    Toggle toggle = trCategoryList.GetChild(i).GetComponent<Toggle>();
                    toggle.isOn = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepAcquire -= OnEventIllustratedBookExplorationStepAcquire;
        CsIllustratedBookManager.Instance.EventIllustratedBookExplorationStepRewardReceive -= OnEventIllustratedBookExplorationStepRewardReceive;
    }

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookExplorationStepAcquire()
    {
        Toggle toggleExploration = transform.Find("CategoryList/ToggleIllustBookCategory").GetComponent<Toggle>();
        if (toggleExploration.isOn)
        {
            OnValueChangedExploration(toggleExploration);
        }
        else
        {
            toggleExploration.isOn = true;
        }

		UpdateCumulativeDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventIllustratedBookExplorationStepRewardReceive(ClientCommon.PDItemBooty[] pDItemBooty)
    {
        UpdateExplorationReward();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedIllustratedBookCategory(Toggle toggleIllustratedBook, int nIndex)
    {
        Text textCategory = toggleIllustratedBook.transform.Find("TextContentName").GetComponent<Text>();

        if (toggleIllustratedBook.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_trExplorationDisplay.gameObject.SetActive(false);
            m_trIllustratedBookDisplay.gameObject.SetActive(true);


            m_nSelectCategoryId = CsGameData.Instance.IllustratedBookCategoryList[nIndex].CategoryId;
            //서브메뉴 세팅 디폴트 -> 도감.
            List<CsIllustratedBookType> listcsIllustratedBookType = CsGameData.Instance.IllustratedBookTypeList.FindAll(a => a.IllustratedBookCategory.CategoryId == CsGameData.Instance.IllustratedBookCategoryList[nIndex].CategoryId);

            Transform trTypeList = transform.Find("Scroll View/Viewport/Content");
            trTypeList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            for (int i = 0; i < trTypeList.childCount; i++)
            {
                trTypeList.GetChild(i).gameObject.SetActive(false);
            }

            //카테고리에 맞는 타입리스트 생성

            for (int i = 0; i < listcsIllustratedBookType.Count; i++)
            {
                int nIndexBookType = listcsIllustratedBookType[i].Type;

                Transform trToggle = trTypeList.Find("Type" + i);

                if (trToggle == null)
                {
                    GameObject goCategory = Instantiate(m_goType, trTypeList);
                    goCategory.name = "Type" + i;
                    trToggle = goCategory.transform;
                }
                else
                {
                    trToggle.gameObject.SetActive(true);
                }

                Toggle toggle = trToggle.GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.group = trTypeList.GetComponent<ToggleGroup>();

                if (i == 0)
                {
                    toggle.isOn = true;
                }
                else
                {
                    toggle.isOn = false;
                }

                toggle.onValueChanged.AddListener((ison) => OnValueChangedBookType(toggle, nIndexBookType));

                List<CsIllustratedBook> listCsIllustratedBook = CsGameData.Instance.IllustratedBookList.FindAll(a => a.IllustratedBookType.Type == listcsIllustratedBookType[i].Type && a.IllustratedBookType.IllustratedBookCategory.CategoryId == m_nSelectCategoryId);

                int nCount = 0;

                for (int j = 0; j < listCsIllustratedBook.Count; j++)
                {
                    if (CsIllustratedBookManager.Instance.ActivationIllustratedBookIdList.Find(a => a == listCsIllustratedBook[j].IllustratedBookId) != 0)
                    {
                        nCount++;
                    }
                }

                Text textType = trToggle.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textType);
                textType.text = string.Format(listcsIllustratedBookType[i].Name, nCount, listCsIllustratedBook.Count);
                textType.color = CsUIData.Instance.ColorWhite;
            }

            m_nSelectBookType = listcsIllustratedBookType[0].Type;
            UpdateIllustratedBookDisplay();

            textCategory.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textCategory.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedBookType(Toggle toggleBookType, int nIndexBookType)
    {
        if (toggleBookType.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nSelectBookType = nIndexBookType;
            UpdateIllustratedBookDisplay();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedBook(Toggle toggleBook, int nIndex)
    {
        if (toggleBook.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            UpdateIllustratedBookInfoDisplay(nIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedExploration(Toggle toggleExploration)
    {
        Text textExploration = toggleExploration.transform.Find("TextContentName").GetComponent<Text>();

        if (toggleExploration.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_trExplorationDisplay.gameObject.SetActive(true);
            m_trIllustratedBookDisplay.gameObject.SetActive(false);

            Transform trTypeList = transform.Find("Scroll View/Viewport/Content");
            trTypeList.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            for (int i = 0; i < trTypeList.childCount; i++)
            {
                trTypeList.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < CsGameData.Instance.IllustratedBookExplorationStepList.Count; i++)
            {
                int nIndex = i;

                Transform trToggle = trTypeList.Find("Type" + i);

                if (trToggle == null)
                {
                    GameObject goCategory = Instantiate(m_goType, trTypeList);
                    goCategory.name = "Type" + i;
                    trToggle = goCategory.transform;
                }
                else
                {
                    trToggle.gameObject.SetActive(true);
                }

                Toggle toggle = trToggle.GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();
                toggle.group = trTypeList.GetComponent<ToggleGroup>();

                if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo == CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1)
                {
                    toggle.isOn = true;
                }
                else
                {
                    toggle.isOn = false;
                }

                toggle.onValueChanged.AddListener((ison) => OnValueChangedExplorationStep(toggle, nIndex));

                Text textType = trToggle.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textType);
                textType.text = CsGameData.Instance.IllustratedBookExplorationStepList[i].Name;

                if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo > CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo)
                {
                    textType.color = CsUIData.Instance.ColorGray;
                }
                else
                {
                    textType.color = CsUIData.Instance.ColorWhite;
                }
            }

            m_nSelectExplorationStep = CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1;
            UpdateExplorationDisplay();

            textExploration.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            textExploration.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedExplorationStep(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nSelectExplorationStep = CsGameData.Instance.IllustratedBookExplorationStepList[nIndex].StepNo;
            UpdateExplorationDisplay();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGetExplorationReward()
    {
        //조건검사 추가
		if (System.DateTime.Compare(CsIllustratedBookManager.Instance.IllustratedBookExplorationStepRewardReceivedDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0)
		{
			if (CsIllustratedBookManager.Instance.IllustratedBookExplorationStepRewardReceivedStepNo == CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo)
			{
				CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A79_ERROR_00301"));
			}
			else
			{
				CsIllustratedBookManager.Instance.SendIllustratedBookExplorationStepRewardReceive();
			}
		}
		else
		{
			CsIllustratedBookManager.Instance.SendIllustratedBookExplorationStepRewardReceive();
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickExplorationRewardItem(int nIndex)
    {
        //아이템 정보창 띄우게끔
        CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo);

        if (csIllustratedBookExplorationStep != null)
        {
            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(() => OpenPopupItemInfo(csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList[nIndex])));
            }
            else
            {
                OpenPopupItemInfo(csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList[nIndex]);
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
    void OnClickExplorationActive()
    {
        //활성화조건 검사.
        Debug.Log((CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1) + " /// " + m_nSelectExplorationStep + " /// " + CsGameData.Instance.MyHeroInfo.ExplorationPoint);
        if (CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1 == m_nSelectExplorationStep)
        {
            CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(m_nSelectExplorationStep);

            if (csIllustratedBookExplorationStep.ActivationExplorationPoint <= CsGameData.Instance.MyHeroInfo.ExplorationPoint)
            {
                CsIllustratedBookManager.Instance.SendIllustratedBookExplorationStepAcquire(m_nSelectExplorationStep);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickCumulative(bool bIson)
	{
		m_trCumulative.gameObject.SetActive(bIson);
	}

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;
        //카테고리 세팅
        Transform trCategoryList = transform.Find("CategoryList");

        if (m_goCategory == null)
        {
            m_goCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleIllustBookCategory");
        }

        Toggle toggleExploration = trCategoryList.Find("ToggleIllustBookCategory").GetComponent<Toggle>();
        toggleExploration.onValueChanged.RemoveAllListeners();
        toggleExploration.isOn = true;
        toggleExploration.onValueChanged.AddListener((ison) => OnValueChangedExploration(toggleExploration));

        Text textExploration = toggleExploration.transform.Find("TextContentName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExploration);
        textExploration.text = CsConfiguration.Instance.GetString("A84_CATE_00001");
        textExploration.color = CsUIData.Instance.ColorWhite;

        for (int i = 0; i < CsGameData.Instance.IllustratedBookCategoryList.Count; i++)
        {
            int nIndex = i;

            GameObject goCategory = Instantiate(m_goCategory, trCategoryList);
            goCategory.name = "Cartegory" + i;

            Toggle toggle = goCategory.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.group = trCategoryList.GetComponent<ToggleGroup>();
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((ison) => OnValueChangedIllustratedBookCategory(toggle, nIndex));

            Text textCategory = goCategory.transform.Find("TextContentName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCategory);
            textCategory.text = CsGameData.Instance.IllustratedBookCategoryList[i].Name;
            textCategory.color = CsUIData.Instance.ColorGray;

            if (CsGameData.Instance.IllustratedBookCategoryList[i].CategoryId == 1 && (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo < CsGameConfig.Instance.SceneryQuestRequiredMainQuestNo))
            {
                toggle.gameObject.SetActive(false);
            }
        }

        Transform trTypeList = transform.Find("Scroll View/Viewport/Content");

        if (m_goType == null)
        {
            m_goType = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleIllustBookContent");
        }

        for (int i = 0; i < CsGameData.Instance.IllustratedBookExplorationStepList.Count; i++)
        {
            int nIndex = i;

            GameObject goCategory = Instantiate(m_goType, trTypeList);
            goCategory.name = "Type" + i;

            Toggle toggle = goCategory.GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.group = trTypeList.GetComponent<ToggleGroup>();

            if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo == CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1)
            {
                toggle.isOn = true;
            }
            else
            {
                toggle.isOn = false;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedExplorationStep(toggle, nIndex));

            Text textType = goCategory.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textType);
            textType.text = CsGameData.Instance.IllustratedBookExplorationStepList[i].Name;

            if (CsGameData.Instance.IllustratedBookExplorationStepList[i].StepNo > CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo)
            {
                textType.color = CsUIData.Instance.ColorGray;
            }
            else
            {
                textType.color = CsUIData.Instance.ColorWhite;
            }
        }

        InitializeExplorationDisplay();
        InitializeIllustratedBookDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeExplorationDisplay()
    {
        m_trExplorationDisplay = transform.Find("Display0");

        Text textName = m_trExplorationDisplay.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);

        Text textDescription = m_trExplorationDisplay.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);

        Text textAttr = m_trExplorationDisplay.Find("TextAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttr);
        textAttr.text = CsConfiguration.Instance.GetString("A84_TXT_00003");

        Text textAttrName0 = m_trExplorationDisplay.Find("TextAttrName0").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrName0);

        Text textAttrValue0 = m_trExplorationDisplay.Find("TextAttrValue0").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrValue0);

        Text textAttrName1 = m_trExplorationDisplay.Find("TextAttrName1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrName1);

        Text textAttrValue1 = m_trExplorationDisplay.Find("TextAttrValue1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrValue1);

        Text textGrade = m_trExplorationDisplay.Find("TextGrade").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGrade);
        textGrade.text = CsConfiguration.Instance.GetString("A84_TXT_00004");

        Text textGradeDescription = m_trExplorationDisplay.Find("TextGradeDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGradeDescription);

		Text textRewardGold = m_trExplorationDisplay.Find("RewardGold/TextRewardGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRewardGold);

        Button buttonReward = m_trExplorationDisplay.Find("ButtonReward").GetComponent<Button>();
        buttonReward.onClick.RemoveAllListeners();
        buttonReward.onClick.AddListener(OnClickGetExplorationReward);
        buttonReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textReward = buttonReward.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textReward);

        Text textPoint = m_trExplorationDisplay.Find("IllustPoint/TextPoint").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPoint);

        Button buttonActive = m_trExplorationDisplay.Find("ButtonActive").GetComponent<Button>();
        buttonActive.onClick.RemoveAllListeners();
        buttonActive.onClick.AddListener(OnClickExplorationActive);
        buttonActive.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textActive = buttonActive.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textActive);
        textActive.text = CsConfiguration.Instance.GetString("A84_TXT_00005");

		m_trCumulative = transform.Find("CumulativeAttr");

		Button buttonCumulative = m_trExplorationDisplay.Find("ButtonCumulative").GetComponent<Button>();
		buttonCumulative.onClick.RemoveAllListeners();
		buttonCumulative.onClick.AddListener(() => OnClickCumulative(true));
		buttonCumulative.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Text textCumulative = buttonCumulative.GetComponentInChildren<Text>();
		CsUIData.Instance.SetFont(textCumulative);
		textCumulative.text = CsConfiguration.Instance.GetString("PUBLIC_ACC_ATTRV");

        m_nSelectExplorationStep = CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1;
        UpdateExplorationDisplay();
		UpdateCumulativeDisplay();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExplorationDisplay()
    {
        CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(m_nSelectExplorationStep);

        if (csIllustratedBookExplorationStep == null)
        {
            return;
        }

        Text textName = m_trExplorationDisplay.Find("TextName").GetComponent<Text>();
        textName.text = csIllustratedBookExplorationStep.Name;

        Text textDescription = m_trExplorationDisplay.Find("TextDescription").GetComponent<Text>();
        textDescription.text = csIllustratedBookExplorationStep.Description;

        Text textAttrName0 = m_trExplorationDisplay.Find("TextAttrName0").GetComponent<Text>();
        textAttrName0.text = csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[0].Attr.Name;

        Text textAttrValue0 = m_trExplorationDisplay.Find("TextAttrValue0").GetComponent<Text>();
        textAttrValue0.text = csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[0].AttrValue.Value.ToString("#,##0");

        Text textAttrName1 = m_trExplorationDisplay.Find("TextAttrName1").GetComponent<Text>();
        textAttrName1.text = csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[1].Attr.Name;

        Text textAttrValue1 = m_trExplorationDisplay.Find("TextAttrValue1").GetComponent<Text>();
        textAttrValue1.text = csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList[1].AttrValue.Value.ToString("#,##0");

        Text textGradeDescription = m_trExplorationDisplay.Find("TextGradeDescription").GetComponent<Text>();

        Button buttonActive = m_trExplorationDisplay.Find("ButtonActive").GetComponent<Button>();

        if (m_nSelectExplorationStep < CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1)
        {
            // 활성화
            CsUIData.Instance.DisplayButtonInteractable(buttonActive, false);
            textGradeDescription.text = CsConfiguration.Instance.GetString("A84_TXT_00008");
        }
        else if (m_nSelectExplorationStep == CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1)
        {
            // 현재 포인트 / 필요포인트
            textGradeDescription.text = string.Format(CsConfiguration.Instance.GetString("A84_TXT_00006"), CsGameData.Instance.MyHeroInfo.ExplorationPoint, csIllustratedBookExplorationStep.ActivationExplorationPoint);

            if (csIllustratedBookExplorationStep.ActivationExplorationPoint <= CsGameData.Instance.MyHeroInfo.ExplorationPoint)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonActive, true);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonActive, false);
            }
        }
        else
        {
            // 활성화 불가
            CsUIData.Instance.DisplayButtonInteractable(buttonActive, false);
            textGradeDescription.text = CsConfiguration.Instance.GetString("A84_TXT_00009");
        }

        Text textRewardGold = m_trExplorationDisplay.Find("RewardGold/TextRewardGold").GetComponent<Text>();
        textRewardGold.text = csIllustratedBookExplorationStep.GoldReward.Value.ToString("#,##0");

		Text textPoint = m_trExplorationDisplay.Find("IllustPoint/TextPoint").GetComponent<Text>();
        textPoint.text = CsGameData.Instance.MyHeroInfo.ExplorationPoint.ToString("#,##0");

        UpdateExplorationReward();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExplorationReward()
    {
        //CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo);

        CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(m_nSelectExplorationStep);

        if (csIllustratedBookExplorationStep == null)
        {
            return;
        }

        //보상버튼 활성화조건 검사
        Button buttonReward = m_trExplorationDisplay.Find("ButtonReward").GetComponent<Button>();
        Text textReward = buttonReward.transform.Find("Text").GetComponent<Text>();

        if (csIllustratedBookExplorationStep == null)
        {
            csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo + 1);

			CsUIData.Instance.DisplayButtonInteractable(buttonReward, false);
            textReward.text = CsConfiguration.Instance.GetString("A84_TXT_00007");
			buttonReward.GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
        else
        {
            if (CsIllustratedBookManager.Instance.IllustratedBookExplorationStepRewardReceivedStepNo == CsIllustratedBookManager.Instance.IllustratedBookExplorationStepNo &&
                (System.DateTime.Compare(CsIllustratedBookManager.Instance.IllustratedBookExplorationStepRewardReceivedDate.Date, CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date) == 0))
            {
				CsUIData.Instance.DisplayButtonInteractable(buttonReward, false);
                textReward.text = CsConfiguration.Instance.GetString("A84_TXT_00010");
				buttonReward.GetComponent<Image>().color = new Color(255, 255, 255, 0);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonReward, true);
                textReward.text = CsConfiguration.Instance.GetString("A84_TXT_00007");
				buttonReward.GetComponent<Image>().color = new Color(255, 255, 255, 255);
            }
        }

        for (int i = 0; i < csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList.Count; i++)
        {
            Transform trReward = m_trExplorationDisplay.Find("RewardItem" + i);

            if (trReward != null)
            {
                int nIndex = i;

                Button button = trReward.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnClickExplorationRewardItem(nIndex));
                button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

				Image imageGrade = trReward.GetComponent<Image>();
				imageGrade.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank0" + csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList[i].ItemReward.Item.ItemGrade.Grade);

                Image image = trReward.Find("ImageIcon").GetComponent<Image>();
                image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList[i].ItemReward.Item.Image);

                Text textCount = trReward.Find("TextCount").GetComponent<Text>();
                textCount.text = csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList[i].ItemReward.ItemCount.ToString("#,##0");
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateCumulativeDisplay()
	{
		Transform trBackground = m_trCumulative.Find("ImageBackground");

		Button buttonCumulative = m_trCumulative.GetComponent<Button>();
		buttonCumulative.onClick.RemoveAllListeners();
		buttonCumulative.onClick.AddListener(() => OnClickCumulative(false));
		buttonCumulative.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		CsUIData.Instance.SetText(trBackground.Find("TextTitle"), CsConfiguration.Instance.GetString("PUBLIC_ACC_ATTR"), true);

		Transform trContent = trBackground.Find("Scroll View/Viewport/Content");

		GameObject goCumulativeItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/CumulativeItem");

		Dictionary<int, long> dicCumulativeAttr = CsIllustratedBookManager.Instance.GetCumulativeAttr();

		if (dicCumulativeAttr != null)
		{
			for (int i = 0; i < CsGameData.Instance.AttrList.Count; i++)
			{
				if (dicCumulativeAttr.ContainsKey(CsGameData.Instance.AttrList[i].AttrId))
				{
					Instantiate(goCumulativeItem, trContent);
					CsUIData.Instance.SetText(goCumulativeItem.transform.Find("TextAttrName"), CsGameData.Instance.AttrList[i].Name, false);
					CsUIData.Instance.SetText(goCumulativeItem.transform.Find("TextAttrValue"), dicCumulativeAttr[CsGameData.Instance.AttrList[i].AttrId].ToString("#,##0"), false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeIllustratedBookDisplay()
    {
        m_trIllustratedBookDisplay = transform.Find("Display1");

		Text textPoint = m_trIllustratedBookDisplay.Find("ImagePoint/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textPoint);
		textPoint.text = CsGameData.Instance.MyHeroInfo.ExplorationPoint.ToString("#,##0");

        Text textBookName = m_trIllustratedBookDisplay.Find("Item/TextItemName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBookName);

        Transform trAttr = m_trIllustratedBookDisplay.Find("ItemAttr");

        Text textAttr = trAttr.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttr);
        textAttr.text = CsConfiguration.Instance.GetString("A84_TXT_00001");

        Text textOptionName0 = trAttr.Find("OptionAttr0/TextAttrName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionName0);

        Text textOptionValue0 = trAttr.Find("OptionAttr0/TextAttrValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionValue0);

        Text textOptionName1 = trAttr.Find("OptionAttr1/TextAttrName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionName1);

        Text textOptionValue1 = trAttr.Find("OptionAttr1/TextAttrValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textOptionValue1);

        Transform trInfo = m_trIllustratedBookDisplay.Find("ItemInfo");

        Text textInfo = trInfo.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfo);
        textInfo.text = CsConfiguration.Instance.GetString("A84_TXT_00002");

        //Text textInfoDes = trInfo.Find("TextInfo").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textInfo);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateIllustratedBookDisplay()
    {
        //해당 타입 인덱스를 가져와서 북리스트 생성 및 업데이트

        List<CsIllustratedBook> listCsIllustratedBook = CsGameData.Instance.IllustratedBookList.FindAll(a => a.IllustratedBookType.Type == m_nSelectBookType && a.IllustratedBookType.IllustratedBookCategory.CategoryId == m_nSelectCategoryId);

        Transform trContent = m_trIllustratedBookDisplay.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < trContent.childCount; i++)
        {
            trContent.GetChild(i).gameObject.SetActive(false);
        }

        if (m_goBook == null)
        {
            m_goBook = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupAchievement/ToggleIllustBookItem");
        }

        for (int i = 0; i < listCsIllustratedBook.Count; i++)
        {
            int nBookIndex = i;
            Transform trBook = trContent.Find("Book" + i);

            if (trBook == null)
            {
                GameObject goBook = Instantiate(m_goBook, trContent);
                goBook.name = "Book" + i;
                trBook = goBook.transform;
            }
            else
            {
                trBook.gameObject.SetActive(true);
            }

            Toggle toggleBook = trBook.GetComponent<Toggle>();
            toggleBook.onValueChanged.RemoveAllListeners();
            toggleBook.group = trContent.GetComponent<ToggleGroup>();

            if (i == 0)
            {
                toggleBook.isOn = true;
            }
            else
            {
                toggleBook.isOn = false;
            }

            toggleBook.onValueChanged.AddListener((ison) => OnValueChangedBook(toggleBook, nBookIndex));

            Image imageIcon = trBook.Find("Background/ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + listCsIllustratedBook[i].ImageName);

            Transform trLock = trBook.Find("Background/ImageLock");

            if (CsIllustratedBookManager.Instance.ActivationIllustratedBookIdList.Find(a => a == listCsIllustratedBook[i].IllustratedBookId) != 0)
            {
                trLock.gameObject.SetActive(false);
            }
            else
            {
                trLock.gameObject.SetActive(true);
            }

            UpdateIllustratedBookInfoDisplay(0);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateIllustratedBookInfoDisplay(int nIndex)
    {
        //해당 북 정보를 가져와서 보여줌
        List<CsIllustratedBook> listCsIllustratedBook = CsGameData.Instance.IllustratedBookList.FindAll(a => a.IllustratedBookType.Type == m_nSelectBookType && a.IllustratedBookType.IllustratedBookCategory.CategoryId == m_nSelectCategoryId);

        CsIllustratedBook csIllustratedBook = listCsIllustratedBook[nIndex];

		Image image = m_trIllustratedBookDisplay.Find("Item/ImageItem/ImageIcon").GetComponent<Image>();
        image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csIllustratedBook.ImageName);

		Text textPoint = m_trIllustratedBookDisplay.Find("ImagePoint/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textPoint);
		textPoint.text = CsGameData.Instance.MyHeroInfo.ExplorationPoint.ToString("#,##0");

		Text textBookName = m_trIllustratedBookDisplay.Find("Item/TextItemName").GetComponent<Text>();
        textBookName.text = csIllustratedBook.Name;

        Transform trAttr = m_trIllustratedBookDisplay.Find("ItemAttr");

        for (int i = 0; i < csIllustratedBook.IllustratedBookAttrList.Count; i++)
        {
            Text textOptionName0 = trAttr.Find("OptionAttr" + i + "/TextAttrName").GetComponent<Text>();
            textOptionName0.text = string.Format("<color={0}>{1}</color>", csIllustratedBook.IllustratedBookAttrList[i].IllustratedBookAttrGrade.ColorCode, csIllustratedBook.IllustratedBookAttrList[i].Attr.Name);

            Text textOptionValue0 = trAttr.Find("OptionAttr" + i + "/TextAttrValue").GetComponent<Text>();
            textOptionValue0.text = string.Format("<color={0}>{1}</color>", csIllustratedBook.IllustratedBookAttrList[i].IllustratedBookAttrGrade.ColorCode, csIllustratedBook.IllustratedBookAttrList[i].AttrValue.Value);
        }

        Text textInfoDes = m_trIllustratedBookDisplay.Find("ItemInfo/TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfoDes);
        textInfoDes.text = csIllustratedBook.Description;
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
    void OpenPopupItemInfo(CsIllustratedBookExplorationStepReward csIllustratedBookExplorationStepReward)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csIllustratedBookExplorationStepReward.ItemReward.Item, 0, csIllustratedBookExplorationStepReward.ItemReward.ItemOwned, -1, false);
    }
}
