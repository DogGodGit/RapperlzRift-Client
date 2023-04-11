using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CsPopupCreatureComposition : CsPopupSub
{
	bool m_bInitialized = false;
	Coroutine m_coroutineSetContents = null;

	CsHeroCreature m_mainCreature = null;
	CsHeroCreature m_subCreature = null;
	List<int> m_listProtectedIndices;

	Transform m_trContent;
	Transform m_trFrameNoCreature;

	Transform m_trMainCreatureSkillInfo;
	Transform m_trSubCreatureSkillInfo;

	Coroutine m_coroutineCheckEvent = null;

	const float m_flCreatureSkillProtectionDelayTime = 0.5f;
	float m_flPointerDownEndTime = 0f;
	int m_nSelectedSkillSlotId = -1;

	//---------------------------------------------------------------------------------------------------
	protected override void Initialize()
	{
		CsCreatureManager.Instance.EventCreatureCompose += OnEventCreatureCompose;
		CsGameEventUIToUI.Instance.EventPointerDownCreatureSkill += OnEventPointerDownCreatureSkill;
		CsGameEventUIToUI.Instance.EventPointerUpCreatureSkill += OnEventPointerUpCreatureSkill;
		CsGameEventUIToUI.Instance.EventPointerExitCreatureSkill += OnEventPointerExitCreatureSkill;

		InitializeUI();

		m_bInitialized = true;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnFinalize()
	{
		CsCreatureManager.Instance.EventCreatureCompose -= OnEventCreatureCompose;
		CsGameEventUIToUI.Instance.EventPointerDownCreatureSkill -= OnEventPointerDownCreatureSkill;
		CsGameEventUIToUI.Instance.EventPointerUpCreatureSkill -= OnEventPointerUpCreatureSkill;
		CsGameEventUIToUI.Instance.EventPointerExitCreatureSkill -= OnEventPointerExitCreatureSkill;
	}

	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
	{
		m_listProtectedIndices = new List<int>();
		
		// 메인 크리쳐
		Transform trFrameMainCreature = transform.Find("FrameMainCreature");

		CsUIData.Instance.SetText(trFrameMainCreature.Find("ImageFrameTitle/TextTitle"), "A146_TXT_00019", true);
		CsUIData.Instance.SetText(trFrameMainCreature.Find("FrameNonSelected/TextContent"), "A146_TXT_00021", true);
		CsUIData.Instance.SetButton(trFrameMainCreature.Find("FrameNonSelected/ButtonPlus"), () => OpenPopupSelectingCreature(true));
		CsUIData.Instance.SetButton(trFrameMainCreature.Find("FrameSelected"), () => OpenPopupSelectingCreature(true));

		m_trMainCreatureSkillInfo = trFrameMainCreature.Find("FrameSelected/PopupSkillInfo");
		CsUIData.Instance.SetButton(m_trMainCreatureSkillInfo, () => m_trMainCreatureSkillInfo.gameObject.SetActive(false));

		// 보조 크리쳐
		Transform trFrameSubCreature = transform.Find("FrameSubCreature");

		CsUIData.Instance.SetText(trFrameSubCreature.Find("ImageFrameTitle/TextTitle"), "A146_TXT_00020", true);
		CsUIData.Instance.SetText(trFrameSubCreature.Find("FrameNonSelected/TextContent"), "A146_TXT_00022", true);
		CsUIData.Instance.SetButton(trFrameSubCreature.Find("FrameNonSelected/ButtonPlus"), () => OpenPopupSelectingCreature(false));
		CsUIData.Instance.SetButton(trFrameSubCreature.Find("FrameSelected"), () => OpenPopupSelectingCreature(false));

		m_trSubCreatureSkillInfo = trFrameSubCreature.Find("FrameSelected/PopupSkillInfo");
		CsUIData.Instance.SetButton(m_trSubCreatureSkillInfo, () => m_trSubCreatureSkillInfo.gameObject.SetActive(false));

		// 메세지
		CsUIData.Instance.SetText(transform.Find("TextContent"), "A146_TXT_00023", true);

		// 스킬 잠금 아이템
		CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.CreatureCompositionSkillProtectionItemId);

		if (csItem != null)
		{
			Transform trImageFrameLockItem = transform.Find("ImageFrameLockItem");
			CsUIData.Instance.SetImage(trImageFrameLockItem.Find("ImageIcon"), "GUI/Items/" + csItem.Image);
			CsUIData.Instance.SetText(trImageFrameLockItem.Find("TextName"), csItem.Name, false);
		}

		// 버튼
		CsUIData.Instance.SetButton(transform.Find("ButtonComposition"), OnClickButtonComposition);
		CsUIData.Instance.SetText(transform.Find("ButtonComposition/TextComposition"), "A146_BTN_00012", true);

		// 선택 창
		Transform trImageBackgroundPopupSelectingCreature = transform.Find("PopupSelectingCreature/ImageBackground");

		CsUIData.Instance.SetText(trImageBackgroundPopupSelectingCreature.Find("TextTitle"), "A146_TXT_00024", true);
		CsUIData.Instance.SetButton(trImageBackgroundPopupSelectingCreature.Find("ButtonClose"), ClosePopupCreature);
		CsUIData.Instance.SetText(trImageBackgroundPopupSelectingCreature.Find("Scroll View/Viewport/FrameNoCreature/TextNoCreature"), "A146_TXT_00031", true);

		m_trContent = trImageBackgroundPopupSelectingCreature.Find("Scroll View/Viewport/Content");
		m_trFrameNoCreature = trImageBackgroundPopupSelectingCreature.Find("Scroll View/Viewport/FrameNoCreature");
	}

	//---------------------------------------------------------------------------------------------------
	void OnEnable()
	{
		m_flPointerDownEndTime = 0f;
		m_nSelectedSkillSlotId = -1;
		
		if (m_coroutineSetContents != null)
		{
			StopCoroutine(m_coroutineSetContents);
			m_coroutineSetContents = null;
		}

		m_coroutineSetContents = StartCoroutine(SetContents());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SetContents()
	{
		yield return new WaitUntil(() => m_bInitialized);

		ResetPopup();

		m_coroutineSetContents = null;
	}

	//---------------------------------------------------------------------------------------------------
	void ResetPopup()
	{
		m_mainCreature = null;
		m_subCreature = null;

		if (m_listProtectedIndices != null)
		{
			m_listProtectedIndices.Clear();
		}

		UpdateSelectedCreature(null);
		UpdateProtectionItemCount();
		UpdateButton();
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupSelectingCreature(bool bMainCreature)
	{
		GameObject goCreatureItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/CreatureComposeSelectItem");

		for (int i = 0; i < m_trContent.childCount; i++)
		{
			m_trContent.GetChild(i).gameObject.SetActive(false);
		}

		int nChildCount = 0;
		foreach (CsHeroCreature csHeroCreature in CsCreatureManager.Instance.HeroCreatureList)
		{
			if (bMainCreature)
			{
				if (m_subCreature != null &&
					m_subCreature.InstanceId.CompareTo(csHeroCreature.InstanceId) == 0)
					continue;
			}
			else
			{
				if (m_mainCreature != null &&
					m_mainCreature.InstanceId.CompareTo(csHeroCreature.InstanceId) == 0)
					continue;
			}

			Transform trCreatureItem = null;

			if (nChildCount < m_trContent.childCount)
			{
				trCreatureItem = m_trContent.GetChild(nChildCount);
				trCreatureItem.gameObject.SetActive(true);
			}
			else
			{
				trCreatureItem = Instantiate(goCreatureItem, m_trContent).transform;
			}

			if (trCreatureItem != null)
			{
				trCreatureItem.name = csHeroCreature.InstanceId.ToString();

				CsUIData.Instance.DisplayCreature(trCreatureItem.Find("ItemSlot"), csHeroCreature.Creature);
				CsUIData.Instance.SetText(trCreatureItem.Find("TextLevel"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01001"), csHeroCreature.Level.ToString()), false);
				CsUIData.Instance.SetText(trCreatureItem.Find("TextName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), csHeroCreature.Creature.CreatureGrade.ColorCode, csHeroCreature.Creature.CreatureCharacter.Name), false);
				CsUIData.Instance.SetText(trCreatureItem.Find("TextGrade"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00030"), csHeroCreature.GetCreatureGrade().ToString("#,##0")), false);

				Transform trImageFrameSkill = trCreatureItem.Find("ImageFrameSkill");

				for (int i = 0; i < trImageFrameSkill.childCount; i++)
				{
					Transform trSkill = trImageFrameSkill.GetChild(i);

					trSkill.Find("ImageLock").gameObject.SetActive(i >= CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + csHeroCreature.AdditionalOpenSkillSlotCount);

					CsHeroCreatureSkill csHeroCreatureSkill = csHeroCreature.HeroCreatureSkillList.Find(heroCreatureSkill => heroCreatureSkill.SlotIndex == i);
					trSkill.GetComponent<Image>().enabled = csHeroCreatureSkill != null;

					if (csHeroCreatureSkill != null)
					{
						CsUIData.Instance.SetImage(trSkill, "GUI/PopupCreature/" + csHeroCreatureSkill.CreatureSkill.ImageName);
					}
				}

				CsUIData.Instance.SetButton(trCreatureItem.Find("ButtonSelect"), () => OnClickSelectCreature(bMainCreature, csHeroCreature));
				CsUIData.Instance.SetText(trCreatureItem.Find("ButtonSelect/TextSelect"), "A146_BTN_00013", true);
			}

			nChildCount++;
		}

		m_trContent.gameObject.SetActive(nChildCount > 0);
		m_trFrameNoCreature.gameObject.SetActive(nChildCount <= 0);

		transform.Find("PopupSelectingCreature").gameObject.SetActive(true);
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateSelectedCreature(bool? bMainCreature)
	{
		Transform trFrameMainCreature = transform.Find("FrameMainCreature");
		Transform trFrameSubCreature = transform.Find("FrameSubCreature");

		if (bMainCreature == null)
		{
			trFrameMainCreature.Find("FrameNonSelected").gameObject.SetActive(true);
			trFrameMainCreature.Find("FrameSelected").gameObject.SetActive(false);

			trFrameSubCreature.Find("FrameNonSelected").gameObject.SetActive(true);
			trFrameSubCreature.Find("FrameSelected").gameObject.SetActive(false);
		}
		else if (bMainCreature == true)
		{
			trFrameMainCreature.Find("FrameNonSelected").gameObject.SetActive(false);
			trFrameMainCreature.Find("FrameSelected").gameObject.SetActive(true);

			if (m_mainCreature != null)
			{
				Transform trFrameSelected = trFrameMainCreature.Find("FrameSelected");

				CsUIData.Instance.DisplayCreature(trFrameSelected.Find("ItemSlot"), m_mainCreature.Creature);
				CsUIData.Instance.SetText(trFrameSelected.Find("TextLevel"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01001"), m_mainCreature.Level.ToString()), false);
				CsUIData.Instance.SetText(trFrameSelected.Find("TextName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), m_mainCreature.Creature.CreatureGrade.ColorCode, m_mainCreature.Creature.CreatureCharacter.Name), false);
				CsUIData.Instance.SetText(trFrameSelected.Find("TextGrade"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00030"), m_mainCreature.GetCreatureGrade().ToString("#,##0")), false);

				Transform trImageFrameSkill = trFrameSelected.Find("ImageFrameSkill");

				for (int i = 0; i < trImageFrameSkill.childCount; i++)
				{
					Transform trSkill = trImageFrameSkill.GetChild(i);

					trSkill.Find("ImageLock").gameObject.SetActive(i >= CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + m_mainCreature.AdditionalOpenSkillSlotCount);
					trSkill.Find("ImageProtect").gameObject.SetActive(false);

					CsHeroCreatureSkill csHeroCreatureSkill = m_mainCreature.HeroCreatureSkillList.Find(heroCreatureSkill => heroCreatureSkill.SlotIndex == i);
					trSkill.GetComponent<Image>().enabled = csHeroCreatureSkill != null;

					if (csHeroCreatureSkill != null)
					{
						CsUIData.Instance.SetImage(trSkill, "GUI/PopupCreature/" + csHeroCreatureSkill.CreatureSkill.ImageName);
					}
				}
			}
		}
		else
		{
			trFrameSubCreature.Find("FrameNonSelected").gameObject.SetActive(false);
			trFrameSubCreature.Find("FrameSelected").gameObject.SetActive(true);

			Transform trFrameSelected = trFrameSubCreature.Find("FrameSelected");

			CsUIData.Instance.DisplayCreature(trFrameSelected.Find("ItemSlot"), m_subCreature.Creature);
			CsUIData.Instance.SetText(trFrameSelected.Find("TextLevel"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01001"), m_subCreature.Level.ToString()), false);
			CsUIData.Instance.SetText(trFrameSelected.Find("TextName"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_01002"), m_subCreature.Creature.CreatureGrade.ColorCode, m_subCreature.Creature.CreatureCharacter.Name), false);
			CsUIData.Instance.SetText(trFrameSelected.Find("TextGrade"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00030"), m_subCreature.GetCreatureGrade().ToString("#,##0")), false);

			Transform trImageFrameSkill = trFrameSelected.Find("ImageFrameSkill");

			for (int i = 0; i < trImageFrameSkill.childCount; i++)
			{
				Transform trSkill = trImageFrameSkill.GetChild(i);

				trSkill.Find("ImageLock").gameObject.SetActive(i >= CsGameConfig.Instance.CreatureSkillSlotBaseOpenCount + m_mainCreature.AdditionalOpenSkillSlotCount);

				CsHeroCreatureSkill csHeroCreatureSkill = m_subCreature.HeroCreatureSkillList.Find(heroCreatureSkill => heroCreatureSkill.SlotIndex == i);
				trSkill.GetComponent<Image>().enabled = csHeroCreatureSkill != null;

				if (csHeroCreatureSkill != null)
				{
					CsUIData.Instance.SetImage(trSkill, "GUI/PopupCreature/" + csHeroCreatureSkill.CreatureSkill.ImageName);

					CsUIData.Instance.SetButton(trSkill, () => OnClickButtonSubCreatureSkill(csHeroCreatureSkill.SlotIndex));
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateProtectionItemCount()
	{
		int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureCompositionSkillProtectionItemId);

		CsCreatureSkillSlotProtection csCreatureSkillSlotProtection = CsGameData.Instance.GetCreatureSkillSlotProtection(m_listProtectedIndices.Count);

		Transform trImageFrameLockItem = transform.Find("ImageFrameLockItem");

		if (csCreatureSkillSlotProtection != null)
		{
			CsUIData.Instance.SetText(trImageFrameLockItem.Find("TextCount"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), nItemCount, csCreatureSkillSlotProtection.RequiredItemCount), false);
		}
		else
		{
			CsUIData.Instance.SetText(trImageFrameLockItem.Find("TextCount"), string.Format(CsConfiguration.Instance.GetString("A146_TXT_00004"), 0, 0), false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateButton()
	{
		int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureCompositionSkillProtectionItemId);

		CsCreatureSkillSlotProtection csCreatureSkillSlotProtection = CsGameData.Instance.GetCreatureSkillSlotProtection(m_listProtectedIndices.Count);

		if (csCreatureSkillSlotProtection != null)
		{
			transform.Find("ButtonComposition").GetComponent<Button>().interactable = m_mainCreature != null && m_subCreature != null && nItemCount >= csCreatureSkillSlotProtection.RequiredItemCount;
		}
		else
		{
			transform.Find("ButtonComposition").GetComponent<Button>().interactable = m_mainCreature != null && m_subCreature != null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ClosePopupCreature()
	{
		transform.Find("PopupSelectingCreature").gameObject.SetActive(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupCreatureCompositionResult(Guid guidCreatureInstanceId)
	{
		CsHeroCreature csHeroCreature = CsCreatureManager.Instance.GetHeroCreature(guidCreatureInstanceId);

		if (csHeroCreature != null)
		{
			GameObject goPopupCreatureCompositionResult = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreature/PopupCreatureCompositionResult");

			Transform trPopupCreatureCompositionResult = Instantiate(goPopupCreatureCompositionResult, transform).transform;

			CsPopupCreatureCompositionResult csPopupCreatureCompositionResult = trPopupCreatureCompositionResult.GetComponent<CsPopupCreatureCompositionResult>();

			if (csPopupCreatureCompositionResult != null)
			{
				csPopupCreatureCompositionResult.DisplayResult(csHeroCreature);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CheckEvent()
	{
		yield return new WaitForSeconds(m_flCreatureSkillProtectionDelayTime);

		bool bError = false;

		if (m_nSelectedSkillSlotId > -1)
		{
			bool bContains = m_listProtectedIndices.Contains(m_nSelectedSkillSlotId);

			if (bContains)
			{
				m_listProtectedIndices.Remove(m_nSelectedSkillSlotId);
			}
			else
			{
				CsCreatureSkillSlotProtection csCreatureSkillSlotProtection = CsGameData.Instance.GetCreatureSkillSlotProtection(m_listProtectedIndices.Count + 1);

				if (csCreatureSkillSlotProtection == null)
				{
					CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00043"));
					bError = true;
				}
				else
				{
					if (m_mainCreature != null)
					{
						if (m_mainCreature.HeroCreatureSkillList.Count < csCreatureSkillSlotProtection.RequiredSkillCount)
						{
							CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_00042"));
							bError = true;
						}
					}
				}

				if (!bError)
				{
					m_listProtectedIndices.Add(m_nSelectedSkillSlotId);
				}
			}

			if (!bError)
			{
				Transform trImageFrameSkill = transform.Find("FrameMainCreature/FrameSelected/ImageFrameSkill");

				if (trImageFrameSkill != null)
				{
					Transform trButtonSkill = trImageFrameSkill.GetChild(m_nSelectedSkillSlotId);
					trButtonSkill.Find("ImageProtect").gameObject.SetActive(!bContains);
				}

				UpdateProtectionItemCount();
				UpdateButton();
			}
		}

		m_coroutineCheckEvent = null;
		m_nSelectedSkillSlotId = -1;
	}

	//---------------------------------------------------------------------------------------------------
	void DisplayCreatureSkillInfo(bool bMainCreature, int nSkillSlotIndex)
	{
		Transform trPopupSkillInfo = bMainCreature ? m_trMainCreatureSkillInfo : m_trSubCreatureSkillInfo;

		CsHeroCreature csHeroCreature = bMainCreature ? m_mainCreature : m_subCreature;

		CsHeroCreatureSkill csHeroCreatureSkill = csHeroCreature.HeroCreatureSkillList.Find(skill => skill.SlotIndex == nSkillSlotIndex);

		if (csHeroCreatureSkill != null)
		{
			CsUIData.Instance.SetText(trPopupSkillInfo.Find("ImageBackground/TextLevelName"),
				string.Format(CsConfiguration.Instance.GetString("A146_TXT_00027"), csHeroCreatureSkill.CreatureSkillGrade.SkillGrade, csHeroCreatureSkill.CreatureSkillGrade.ColorCode, csHeroCreatureSkill.CreatureSkill.Name),
				false);

			CsCreatureSkillAttr csCreatureSkillAttr = csHeroCreatureSkill.CreatureSkill.GetCreatureSkillAttr(csHeroCreatureSkill.CreatureSkillGrade.SkillGrade);

			if (csCreatureSkillAttr != null)
			{
				CsUIData.Instance.SetText(trPopupSkillInfo.Find("ImageBackground/TextDescription"),
					string.Format(csHeroCreatureSkill.CreatureSkill.EffectText, csHeroCreatureSkill.CreatureSkill.Attr.Name, csCreatureSkillAttr.AttrValue.Value.ToString()),
					false);
			}

			trPopupSkillInfo.gameObject.SetActive(true);

			// 위치 조정
			RectTransform rtfImageBackground = trPopupSkillInfo.Find("ImageBackground").GetComponent<RectTransform>();

			if (bMainCreature)
			{
				rtfImageBackground.anchoredPosition = new Vector2(-350 + (100 * (nSkillSlotIndex % 3)), -130 + (-100 * (nSkillSlotIndex / 3)));
			}
			else
			{
				rtfImageBackground.anchoredPosition = new Vector2(160 + (-100 * (nSkillSlotIndex % 3)), -130 + (-100 * (nSkillSlotIndex / 3)));
			}
		}
	}

	#region event

	//---------------------------------------------------------------------------------------------------
	void OnClickSelectCreature(bool bMainCreature, CsHeroCreature csHeroCreature)
	{
		if (bMainCreature)
		{
			m_mainCreature = csHeroCreature;
			m_listProtectedIndices.Clear();

			UpdateSelectedCreature(bMainCreature);
			UpdateProtectionItemCount();

			UpdateButton();
			ClosePopupCreature();
		}
		else
		{
			if (csHeroCreature.InstanceId.CompareTo(CsCreatureManager.Instance.ParticipatedCreatureId) == 0)
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_03003"));
				return;
			}

			if (csHeroCreature.Cheered)
			{
				CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A146_TXT_03004"));
				return;
			}

			CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A146_TXT_00025"),
													CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"),
													() =>
													{
														m_subCreature = csHeroCreature;
														UpdateSelectedCreature(bMainCreature);
														UpdateButton();
														ClosePopupCreature();
													},
													CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null,
													true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonSubCreatureSkill(int nSelectedSkillSlotIndex)
	{
		if (m_trMainCreatureSkillInfo.gameObject.activeSelf)
			return;

		DisplayCreatureSkillInfo(false, nSelectedSkillSlotIndex);
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonComposition()
	{
		CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A146_TXT_00047"),
			CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), () => CsCreatureManager.Instance.SendCreatureCompose(m_mainCreature.InstanceId, m_subCreature.InstanceId, m_listProtectedIndices.ToArray()),
			CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), null, true);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerDownCreatureSkill(int nSlotId)
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;
		}

		m_nSelectedSkillSlotId = nSlotId;
		m_flPointerDownEndTime = Time.realtimeSinceStartup + m_flCreatureSkillProtectionDelayTime;
		m_coroutineCheckEvent = StartCoroutine(CheckEvent());
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerUpCreatureSkill()
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;
		}

		if (m_flPointerDownEndTime - Time.realtimeSinceStartup > 0)
		{
			// 메인크리쳐 스킬 정보 표시
			if (m_trSubCreatureSkillInfo.gameObject.activeSelf)
				return;

			DisplayCreatureSkillInfo(true, m_nSelectedSkillSlotId);
			m_nSelectedSkillSlotId = -1;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventPointerExitCreatureSkill()
	{
		if (m_coroutineCheckEvent != null)
		{
			StopCoroutine(m_coroutineCheckEvent);
			m_coroutineCheckEvent = null;

			m_nSelectedSkillSlotId = -1;
		}
	}

	#endregion event

	#region protocol.event

	//---------------------------------------------------------------------------------------------------
	void OnEventCreatureCompose(Guid guidCreatureInstanceId)
	{
		ResetPopup();

		OpenPopupCreatureCompositionResult(guidCreatureInstanceId);
	}

	//---------------------------------------------------------------------------------------------------

	#endregion protocol.event
}
