using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 추한영 (2018-09-10)
//---------------------------------------------------------------------------------------------------

public class CsPopupCreatureList : CsPopupSub
{
	Guid m_guidSelectedCreature = Guid.Empty;
	Transform m_trContent;
	
	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_trContent = transform.Find("Scroll View/Viewport/Content");

		CsGameEventUIToUI.Instance.EventCreatureSubMenuChanged += OnEventCreatureSubMenuChanged;
		CsGameEventUIToUI.Instance.EventGetSelectedCreature += OnEventGetSelectedCreature;

		CsCreatureManager.Instance.EventCreatureParticipate += OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel += OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer += OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel += OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear += OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureRelease += OnEventCreatureRelease;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize() 
	{
		CsGameEventUIToUI.Instance.EventCreatureSubMenuChanged -= OnEventCreatureSubMenuChanged;
		CsGameEventUIToUI.Instance.EventGetSelectedCreature -= OnEventGetSelectedCreature;

		CsCreatureManager.Instance.EventCreatureParticipate -= OnEventCreatureParticipate;
		CsCreatureManager.Instance.EventCreatureParticipationCancel -= OnEventCreatureParticipationCancel;
		CsCreatureManager.Instance.EventCreatureCheer -= OnEventCreatureCheer;
		CsCreatureManager.Instance.EventCreatureCheerCancel -= OnEventCreatureCheerCancel;
		CsCreatureManager.Instance.EventCreatureRear -= OnEventCreatureRear;
		CsCreatureManager.Instance.EventCreatureRelease -= OnEventCreatureRelease;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		for (int i = 0; i < m_trContent.childCount; i++)
		{
			m_trContent.GetChild(i).gameObject.SetActive(false);
			m_trContent.GetChild(i).name = "";

			m_trContent.GetChild(i).GetComponent<Toggle>().isOn = false;
		}

		ToggleGroup toggleGroup = m_trContent.GetComponent<ToggleGroup>();
		GameObject goCreatureItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/CreatureItem");

		int nChildIndex = 0;

		foreach (CsHeroCreature csHeroCreature in CsCreatureManager.Instance.HeroCreatureList)
		{
			Transform trCreature = null;

			if (nChildIndex < m_trContent.childCount)
			{
				trCreature = m_trContent.GetChild(nChildIndex);
				trCreature.gameObject.SetActive(true);
			}
			else
			{
				trCreature = Instantiate(goCreatureItem, m_trContent).transform;
			}

			if (trCreature != null)
			{
				trCreature.name = csHeroCreature.InstanceId.ToString();

				Transform trItemSlot = trCreature.Find("ItemSlot");

				CsUIData.Instance.DisplayCreature(trItemSlot, csHeroCreature.Creature);

				Toggle toggle = trCreature.GetComponent<Toggle>();
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((bIsOn) => OnValueChangedCreatureToggle(bIsOn, csHeroCreature.InstanceId));

				toggle.group = toggleGroup;

				UpdateCreatureInfo(csHeroCreature);
			}

			nChildIndex++;
		}

		if (CsCreatureManager.Instance.HeroCreatureList.Count > 0 &&
			m_trContent.childCount > 0)
		{
			Toggle toggleFirst = m_trContent.GetChild(0).GetComponent<Toggle>();
			toggleFirst.isOn = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCreatureInfo(CsHeroCreature csHeroCreature)
	{
		Transform trCreature = null;

		trCreature = m_trContent.Find(csHeroCreature.InstanceId.ToString());

		if (trCreature != null)
		{
			Text textLevel = trCreature.Find("TextLevel").GetComponent<Text>();
			CsUIData.Instance.SetFont(textLevel);
			textLevel.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_01001"), csHeroCreature.Level);

			Text textName = trCreature.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textName);
			textName.text = string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name);

			Text textState = trCreature.Find("TextState").GetComponent<Text>();
			CsUIData.Instance.SetFont(textState);

			if (CsCreatureManager.Instance.ParticipatedCreatureId == csHeroCreature.InstanceId)
			{
				textState.text = CsConfiguration.Instance.GetString("A146_TXT_01003");
			}
			else if (csHeroCreature.Cheered)
			{
				textState.text = CsConfiguration.Instance.GetString("A146_TXT_01004");
			}
			else
			{
				textState.text = "";
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedCreatureToggle(bool bIsOn, Guid guidCreatureInstanceId)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			m_guidSelectedCreature = guidCreatureInstanceId;

			CsGameEventUIToUI.Instance.OnEventSelectCreatureToggle(guidCreatureInstanceId);
		}
	}

	#region Event

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureSubMenuChanged()
	{
		transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
	}

	//---------------------------------------------------------------------------------------------------
	Guid OnEventGetSelectedCreature()
	{
		return m_guidSelectedCreature;
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipate()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(m_guidSelectedCreature);

		if (csHeroCreature != null)
		{
			UpdateCreatureInfo(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureParticipationCancel()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(m_guidSelectedCreature);

		if (csHeroCreature != null)
		{
			UpdateCreatureInfo(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheer()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(m_guidSelectedCreature);

		if (csHeroCreature != null)
		{
			UpdateCreatureInfo(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCheerCancel()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(m_guidSelectedCreature);

		if (csHeroCreature != null)
		{
			UpdateCreatureInfo(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureRear()
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(m_guidSelectedCreature);

		if (csHeroCreature != null)
		{
			UpdateCreatureInfo(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureRelease()
	{
		Transform trCreature = m_trContent.Find(m_guidSelectedCreature.ToString());

		if (trCreature != null)
		{
			trCreature.gameObject.SetActive(false);

			for (int i = 0; i < m_trContent.childCount; i++)
			{
				if (m_trContent.GetChild(i).gameObject.activeSelf)
				{
					m_trContent.GetChild(i).GetComponent<Toggle>().isOn = true;
					transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

					break;
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	#endregion Event
}
