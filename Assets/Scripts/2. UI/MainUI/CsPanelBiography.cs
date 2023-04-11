using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPanelBiography : CsUpdateableMonoBehaviour 
{
	int m_nLastCompletedBiogrpahyId = 0;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize() 
	{
		CsBiographyManager.Instance.EventBiographyStart += OnEventBiographyStart;
		CsBiographyManager.Instance.EventBiographyComplete += OnEventBiographyComplete;
		CsGameEventUIToUI.Instance.EventClosePanelBiography += OnEventClosePanelBiography;
		CsGameEventUIToUI.Instance.EventOpenPanelBiographyComplete += OnEventOpenPanelBiographyComplete;
		
		CsUIData.Instance.SetText(transform.Find("ImageBackground/ButtonOpen/TextOpen"), "A122_BTN_00002", true);
		CsUIData.Instance.SetText(transform.Find("ImageBackground/ButtonComplete/TextComplete"), "A122_BTN_00003", true);
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsBiographyManager.Instance.EventBiographyStart -= OnEventBiographyStart;
		CsBiographyManager.Instance.EventBiographyComplete -= OnEventBiographyComplete;
		CsGameEventUIToUI.Instance.EventClosePanelBiography -= OnEventClosePanelBiography;
		CsGameEventUIToUI.Instance.EventOpenPanelBiographyComplete -= OnEventOpenPanelBiographyComplete;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SelectTogglePopupBiography(int nBiographyId)
	{
		Transform trCanvas2 = GameObject.Find("Canvas2").transform;
		yield return new WaitUntil(() => trCanvas2.transform.Find("MainPopupSubMenu/PopupBiography") != null);

		CsGameEventUIToUI.Instance.OnEventSelectToggleBiography(nBiographyId);
	}

	//---------------------------------------------------------------------------------------------------
	void SwitchButtonBiography(bool bEnable)
	{
		transform.Find("ImageBackground").gameObject.SetActive(bEnable);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonOpen(int nBiographyId)
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		if (CsBiographyManager.Instance.GetHeroBiography(nBiographyId) == null)
			return;

		CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Collection, EnSubMenu.Biography);

		StartCoroutine(SelectTogglePopupBiography(nBiographyId));

		SwitchButtonBiography(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonComplete(int nBiographyId)
	{
		m_nLastCompletedBiogrpahyId = nBiographyId;

		CsBiographyManager.Instance.SendBiographyComplete(nBiographyId);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyStart(CsBiography csBiography)
	{
		Transform trImageBackground = transform.Find("ImageBackground");

		Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textName);
		textName.text = csBiography.Name;

		trImageBackground.Find("ButtonComplete").gameObject.SetActive(false);

		Button buttonOpen = trImageBackground.Find("ButtonOpen").GetComponent<Button>();
		buttonOpen.onClick.RemoveAllListeners();
		buttonOpen.onClick.AddListener(() => OnClickButtonOpen(csBiography.BiographyId));
		buttonOpen.gameObject.SetActive(true);

		SwitchButtonBiography(true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventBiographyComplete(int nBiographyId)
	{
		if (m_nLastCompletedBiogrpahyId == nBiographyId)
		{
			SwitchButtonBiography(false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventClosePanelBiography()
	{
		SwitchButtonBiography(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenPanelBiographyComplete(int nBiographyId)
	{
		m_nLastCompletedBiogrpahyId = nBiographyId;

		CsBiography csBiography = CsGameData.Instance.GetBiography(nBiographyId);

		if (csBiography != null)
		{
			Transform trImageBackground = transform.Find("ImageBackground");

			Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = csBiography.Name;

			Button buttonComplete = trImageBackground.Find("ButtonComplete").GetComponent<Button>();
			buttonComplete.onClick.RemoveAllListeners();
			buttonComplete.onClick.AddListener(() => OnClickButtonComplete(csBiography.BiographyId));

			trImageBackground.Find("ButtonOpen").gameObject.SetActive(false);

			SwitchButtonBiography(true);
		}
	}
}
