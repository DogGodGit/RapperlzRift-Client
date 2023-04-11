using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-13)
//---------------------------------------------------------------------------------------------------

public class CsPopupHelp : CsUpdateableMonoBehaviour
{
	Transform m_trContentToggles;
	Transform m_trContent;

	GameObject m_goToggleHelp;
	GameObject m_goTextMidTitleHelp;
	GameObject m_goContentHelp;
	GameObject m_goVaccumHelp;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup += PopupClose;

		InitializeUI();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsGameEventUIToUI.Instance.EventCloseAllPopup -= PopupClose;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_goToggleHelp = Resources.Load<GameObject>("GUI/PopupHelp/ToggleHelp");
		m_goTextMidTitleHelp = Resources.Load<GameObject>("GUI/PopupHelp/TextMidTitleHelp");
		m_goContentHelp = Resources.Load<GameObject>("GUI/PopupHelp/TextContentHelp");
		m_goVaccumHelp = Resources.Load<GameObject>("GUI/PopupHelp/VaccumHelp");

		m_trContentToggles = transform.Find("ImageBackground/FrameToggles/Viewport/Content");
		m_trContent = transform.Find("ImageBackground/FrameContent/Scroll View/Viewport/Content");

		Text textTitle = transform.Find("ImageBackground/TextTitle").GetComponent<Text>();
		CsUIData.Instance.SetFont(textTitle);
		textTitle.text = CsConfiguration.Instance.GetString("A146_TXT_00026");

		Button buttonClose = transform.Find("ImageBackground/ButtonClose").GetComponent<Button>();
		buttonClose.onClick.RemoveAllListeners();
		buttonClose.onClick.AddListener(PopupClose);
	}

	//---------------------------------------------------------------------------------------------------
	public void SetHelp(EnMainMenu enMainMenu)
	{
		StartCoroutine(SetToggles(enMainMenu));
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SetToggles(EnMainMenu enMainMenu)
	{
		yield return new WaitUntil(() => m_trContentToggles != null && m_trContent != null && m_goToggleHelp != null);
		
		for (int i = 0; i < m_trContentToggles.childCount; i++)
		{
			m_trContentToggles.GetChild(i).gameObject.SetActive(false);
			m_trContentToggles.GetChild(i).name = "";
		}

		ToggleGroup toggleGroup = m_trContentToggles.GetComponent<ToggleGroup>();

		Transform trToggle = null;

		switch (enMainMenu)
		{
			case EnMainMenu.Creature:

				for (int i = 0; i < 3; i++)
				{
					int nToggleIndex = i;

					if (nToggleIndex < m_trContentToggles.childCount)
					{
						trToggle = m_trContentToggles.GetChild(nToggleIndex);
						trToggle.gameObject.SetActive(true);
					}
					else
					{
						trToggle = Instantiate(m_goToggleHelp, m_trContentToggles).transform;
					}

					if (trToggle != null)
					{
						trToggle.name = nToggleIndex.ToString();

						Text text = trToggle.Find("Text").GetComponent<Text>();
						CsUIData.Instance.SetFont(text);
						text.text = CsConfiguration.Instance.GetString("A146_BTN_0200" + (1 + nToggleIndex).ToString());

						Toggle toggle = trToggle.GetComponent<Toggle>();
						toggle.group = toggleGroup;
						toggle.onValueChanged.RemoveAllListeners();
						toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggle(isOn, enMainMenu, nToggleIndex));
					}
				}

				if (m_trContentToggles.childCount > 0)
				{
					m_trContentToggles.GetChild(0).GetComponent<Toggle>().isOn = true;
				}

				break;

			case EnMainMenu.Soul:

				if (0 < m_trContentToggles.childCount)
				{
					trToggle = m_trContentToggles.GetChild(0);
					trToggle.gameObject.SetActive(true);
				}
				else
				{
					trToggle = Instantiate(m_goToggleHelp, m_trContentToggles).transform;
				}

				if (trToggle != null)
				{
					trToggle.name = "0";

					Text text = trToggle.Find("Text").GetComponent<Text>();
					CsUIData.Instance.SetFont(text);
					text.text = CsConfiguration.Instance.GetString("A157_BTN_00003");

					Toggle toggle = trToggle.GetComponent<Toggle>();
					toggle.group = toggleGroup;
					toggle.onValueChanged.RemoveAllListeners();
					toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggle(isOn, enMainMenu, 0));
				}

				m_trContentToggles.GetChild(0).GetComponent<Toggle>().isOn = true;

				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetContent(EnMainMenu enMainMenu, int nToggleIndex)
	{
		if (m_goTextMidTitleHelp == null || m_goContentHelp == null || m_goVaccumHelp == null)
			return;

		switch (enMainMenu)
		{
			case EnMainMenu.Creature:

				switch (nToggleIndex)
				{
					case 0:
						for (int i = 0; i < 5; i++)
						{
							// 성장
							Transform trMidTitle = Instantiate(m_goTextMidTitleHelp, m_trContent).transform;

							CsUIData.Instance.SetText(trMidTitle, "A146_HELP_000" + (1 + i * 2).ToString("00"), true);

							string strContent = CsConfiguration.Instance.GetString("A146_HELP_000" + (2 + i * 2).ToString("00"));

							string[] aStrContent = strContent.Split(',');

							for (int j = 0; j < aStrContent.Length; j++)
							{
								Transform trContent = Instantiate(m_goContentHelp, m_trContent).transform;

								CsUIData.Instance.SetText(trContent, "- " + aStrContent[j].Trim(), false);
							}

							if (i < 4)
							{
								Instantiate(m_goVaccumHelp, m_trContent);
							}
						}

						break;
					case 1:
						for (int i = 0; i < 2; i++)
						{
							// 주입
							Transform trMidTitle = Instantiate(m_goTextMidTitleHelp, m_trContent).transform;

							CsUIData.Instance.SetText(trMidTitle, "A146_HELP_000" + (11 + i * 2).ToString("00"), true);

							string strContent = CsConfiguration.Instance.GetString("A146_HELP_000" + (12 + i * 2).ToString("00"));

							string[] aStrContent = strContent.Split(',');

							for (int j = 0; j < aStrContent.Length; j++)
							{
								Transform trContent = Instantiate(m_goContentHelp, m_trContent).transform;

								CsUIData.Instance.SetText(trContent, "- " + aStrContent[j].Trim(), false);
							}

							if (i < 2)
							{
								Instantiate(m_goVaccumHelp, m_trContent);
							}
						}
						break;
					case 2:
					{
						// 합성
						Transform trMidTitle = Instantiate(m_goTextMidTitleHelp, m_trContent).transform;

						CsUIData.Instance.SetText(trMidTitle, "A146_HELP_00015", true);

						string strContent = CsConfiguration.Instance.GetString("A146_HELP_00016");

						string[] aStrContent = strContent.Split(',');

						for (int j = 0; j < aStrContent.Length; j++)
						{
							Transform trContent = Instantiate(m_goContentHelp, m_trContent).transform;

							CsUIData.Instance.SetText(trContent, "- " + aStrContent[j].Trim(), false);
						}
					}
						break;
				}

				break;

			case EnMainMenu.Soul:
				{
					// 분해
					Transform trMidTitle = Instantiate(m_goTextMidTitleHelp, m_trContent).transform;

					CsUIData.Instance.SetText(trMidTitle, "A157_TXT_00005", true);

					string strContent = CsConfiguration.Instance.GetString("A157_TXT_00006");

					string[] aStrContent = strContent.Split(',');

					for (int j = 0; j < aStrContent.Length; j++)
					{
						Transform trContent = Instantiate(m_goContentHelp, m_trContent).transform;

						CsUIData.Instance.SetText(trContent, "- " + aStrContent[j].Trim(), false);
					}
				}
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------

	#region event

	void OnValueChangedToggle(bool bIsOn, EnMainMenu enMainMenu, int nToggleIndex)
	{
		if (bIsOn)
		{
			for (int i = 0; i < m_trContent.childCount; i++)
			{
				Destroy(m_trContent.GetChild(i).gameObject);
			}

			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			SetContent(enMainMenu, nToggleIndex);
		}
	}

	#endregion event
}
