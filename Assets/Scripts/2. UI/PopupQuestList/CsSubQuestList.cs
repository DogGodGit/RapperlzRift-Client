using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-08-24)
//---------------------------------------------------------------------------------------------------

public class CsSubQuestList : CsPopupSub
{
	Transform m_trQuestList;

    GameObject m_goCategory;
    GameObject m_goToggleQuest;

	ToggleGroup m_toggleGroup;

    //---------------------------------------------------------------------------------------------------
	void Awake()
	{
		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		UpdateSubQuestList();
	}

    #region Event

   //---------------------------------------------------------------------------------------------------
    void OnValueChangedQuestSelect(bool bIsOn, int nSubQuestId)
    {
		if (bIsOn)
        {
            CsGameEventUIToUI.Instance.OnEventSelectQuest(EnQuestType.SubQuest, nSubQuestId, true);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trContent = transform.Find("Scroll View/Viewport/Content");
		m_toggleGroup = trContent.GetComponent<ToggleGroup>();

		m_goCategory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupQuestList/QuestCartegory");
        m_goToggleQuest = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupQuestList/ToggleQuest");

		Transform trCategory = Instantiate(m_goCategory, trContent).transform;
		trCategory.name = "QuestCategory";

		m_trQuestList = trCategory.Find("QuestList");

		trCategory.Find("ImageBack/ToggleAllSelect").gameObject.SetActive(false);
		trCategory.Find("ImageBack/ToggleCartegoryOpen").gameObject.SetActive(false);

		Text textCategoryName = trCategory.Find("ImageBack/TextCartegoryName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textCategoryName);
		textCategoryName.text = CsConfiguration.Instance.GetString("A38_BTN_00012");

    }

	//---------------------------------------------------------------------------------------------------
    void UpdateSubQuestList()
    {
		for (int i = 0; i < m_trQuestList.childCount; i++)
		{
			m_trQuestList.GetChild(i).gameObject.SetActive(false);
		}

		int nIndex = 0;

		foreach (var csSubQuest in CsSubQuestManager.Instance.GetAcceptableSubQuestList())
		{
			Transform trToggleQuest = null;

			if (nIndex < m_trQuestList.childCount)
			{
				trToggleQuest = m_trQuestList.GetChild(nIndex);
			}
			else
			{
				trToggleQuest = Instantiate(m_goToggleQuest, m_trQuestList).transform;
				trToggleQuest.name = nIndex.ToString();

				trToggleQuest.Find("ToggleSwitch").gameObject.SetActive(false);
				trToggleQuest.Find("ImageComplete").gameObject.SetActive(false);
			}

			if (trToggleQuest != null)
			{
				trToggleQuest.gameObject.SetActive(true);

				Text textQuestName = trToggleQuest.Find("TextQuestName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textQuestName);
				textQuestName.text = csSubQuest.Title;

				Toggle toggle = trToggleQuest.GetComponent<Toggle>();
				toggle.group = m_toggleGroup;
				toggle.isOn = false;

				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((isOn) => OnValueChangedQuestSelect(isOn, csSubQuest.QuestId));
			}

			nIndex++;
		}

		if (nIndex > 0)
		{
			Toggle toggle = m_trQuestList.GetChild(0).GetComponent<Toggle>();
			toggle.isOn = true;
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAcceptableSubQuestEmpty();
		}
    }
}