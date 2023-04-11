using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-19)
//---------------------------------------------------------------------------------------------------

public class CsPopupCreatureBook : MonoBehaviour
{
	GameObject m_goItemSlot;

	bool m_bInitialized = false;
	
	public Delegate EventClosePopupCreatureBook;

	Transform m_trContent;

	CsCreatureCharacter m_csCreatureCharacter;
	CsCreatureGrade m_csCreatureGrade;


	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_goItemSlot = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/ItemSlot");

		Transform trImageBackground = transform.Find("ImageBackground");

		CsUIData.Instance.SetText(trImageBackground.Find("TextTitle"), "A146_TXT_00039", true);
		CsUIData.Instance.SetButton(trImageBackground.Find("ButtonClose"), PopupClose);

		m_trContent = trImageBackground.Find("Scroll View/Viewport/Content");

		Transform trFrameToggles = trImageBackground.Find("FrameToggles");

		for (int i = 0; i < trFrameToggles.childCount; i++)
		{
			Transform trToggle = trFrameToggles.GetChild(i);

			CsCreatureGrade csCreatureGrade = CsGameData.Instance.GetCreatureGrade(i + 1);

			if (csCreatureGrade != null)
			{
				CsUIData.Instance.SetText(trToggle.Find("TextGrade"),
					string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), csCreatureGrade.ColorCode, csCreatureGrade.Name), false);

				Toggle toggle = trToggle.GetComponent<Toggle>();
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggleGrade(isOn, csCreatureGrade));
			}
		}

		SetCreatures();

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		if (EventClosePopupCreatureBook != null)
		{
			EventClosePopupCreatureBook();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetCreatures()
	{
		ToggleGroup toggleGroup = m_trContent.GetComponent<ToggleGroup>();

		foreach (CsCreatureCharacter csCreatureCharacter in CsGameData.Instance.CreatureCharacterList)
		{
			GameObject goCreature = Instantiate(m_goItemSlot, m_trContent);
			goCreature.name = csCreatureCharacter.CreatureCharacterId.ToString();

			CsCreature csCreature = CsGameData.Instance.CreatureList.Find(creature => creature.CreatureCharacter.CreatureCharacterId == csCreatureCharacter.CreatureCharacterId && creature.CreatureGrade.Grade == 1);

			if (csCreature != null)
			{
				CsUIData.Instance.DisplayCreature(goCreature.transform, csCreature);	
			}

			Toggle toggle = goCreature.GetComponent<Toggle>();

			toggle.group = toggleGroup;
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((isOn) => OnValueChangedToggleCreature(isOn, csCreatureCharacter));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SetCreature(CsCreature csCreature)
	{
		StartCoroutine(SelectCreature(csCreature));
	}

	//---------------------------------------------------------------------------------------------------
	void PopupClose()
	{
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureInfo()
	{
		if (m_csCreatureCharacter != null && m_csCreatureGrade != null)
		{
			LoadCreatureModel(m_csCreatureCharacter);

			Transform trImageFrameCreatureInfo = transform.Find("ImageBackground/ImageFrameCreatureInfo");

			CsUIData.Instance.SetText(trImageFrameCreatureInfo.Find("TextName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), m_csCreatureGrade.ColorCode, m_csCreatureCharacter.Name), false);

			CsUIData.Instance.SetText(trImageFrameCreatureInfo.Find("TextRequiredLevel"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00040"), m_csCreatureCharacter.RequiredHeroLevel), false);

			CsCreature csCreature = CsGameData.Instance.CreatureList.Find(creature => creature.CreatureCharacter.CreatureCharacterId == m_csCreatureCharacter.CreatureCharacterId && creature.CreatureGrade.Grade == m_csCreatureGrade.Grade);

			if (csCreature != null)
			{
				int nIndex = 1;

				foreach (CsCreatureBaseAttrValue csCreatureBaseAttrValue in csCreature.CreatureBaseAttrValueList)
				{
					CsUIData.Instance.SetText(trImageFrameCreatureInfo.Find("TextAttr" + nIndex.ToString()), csCreatureBaseAttrValue.CreatureBaseAttr.Attr.Name, false);

					CsUIData.Instance.SetText(trImageFrameCreatureInfo.Find("TextValueAttr" + nIndex.ToString()), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00041"), csCreatureBaseAttrValue.MinAttrValue, csCreatureBaseAttrValue.MaxAttrValue), false);

					nIndex++;
				}
			}

			CsUIData.Instance.SetText(trImageFrameCreatureInfo.Find("TextContent"), m_csCreatureCharacter.Description, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SelectCreature(CsCreature csCreature)
	{
		yield return new WaitUntil(() => m_trContent != null && m_trContent.childCount == CsGameData.Instance.CreatureCharacterList.Count && m_bInitialized);

		Transform trCreature = m_trContent.Find(csCreature.CreatureCharacter.CreatureCharacterId.ToString());

		if (trCreature != null)
		{
			Toggle toggle = trCreature.GetComponent<Toggle>();
			
			if (toggle == null)
			{
				yield return new WaitUntil (() => toggle != null);

				toggle = trCreature.GetComponent<Toggle>();
			}
			
			toggle.isOn = true;
		}

		Transform trFrameToggles = transform.Find("ImageBackground/FrameToggles");

		Transform trToggleGrade = trFrameToggles.Find("ToggleGrade" + csCreature.CreatureGrade.Grade);

		if (trToggleGrade != null)
		{
			Toggle toggleGrade = trToggleGrade.GetComponent<Toggle>();

			if (toggleGrade != null)
			{
				toggleGrade.isOn = true;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleCreature(bool bIsOn, CsCreatureCharacter csCreatureCharacter)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			m_csCreatureCharacter = csCreatureCharacter;

			UpdateCreatureInfo();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleGrade(bool bIsOn, CsCreatureGrade csCreatureGrade)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			m_csCreatureGrade = csCreatureGrade;

			UpdateCreatureInfo();
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
}
