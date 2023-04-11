using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;
using System.Linq;

public class CsCreatureManager
{
	const int m_nCreatureVariationRequireItemCount = 1;
	const int m_nCreatureSwitchRequireItemCount = 1;

	const float m_flCreatureFeedStartDelayTime = 0.5f;
	const float m_flCreatureFeedDelayTime = 0.1f;

	bool m_bWaitResponse = false;

	List<CsHeroCreature> m_listCsHeroCreature;
	Guid m_guidParticipatedCreatureId;
	DateTime m_dtCreatureVariationCountDate;
	int m_nDailyCreatureVariationCount;

	Guid m_guidInstanceId;
	Guid m_guidMaterialInstanceId;
	//---------------------------------------------------------------------------------------------------
	public static CsCreatureManager Instance
	{
		get { return CsSingleton<CsCreatureManager>.GetInstance(); }
	}

	public static int CreatureVariationRequireItemCount
	{
		get { return m_nCreatureVariationRequireItemCount; }
	}

	public static int CreatureSwitchRequireItemCount
	{
		get { return m_nCreatureSwitchRequireItemCount; }
	}

	public static float CreatureFeedStartDelayTime
	{
		get { return m_flCreatureFeedStartDelayTime; }
	}

	public static float CreatureFeedDelayTime
	{
		get { return m_flCreatureFeedDelayTime; }
	}

	public List<CsHeroCreature> HeroCreatureList
	{
		get { return m_listCsHeroCreature; }
	}

	public Guid ParticipatedCreatureId
	{
		get { return m_guidParticipatedCreatureId; }
		set { m_guidParticipatedCreatureId = value; }
	}

	public DateTime CreatureVariationCountDate
	{
		get { return m_dtCreatureVariationCountDate; }
		set { m_dtCreatureVariationCountDate = value; }
	}

	public int DailyCreatureVariationCount
	{
		get { return m_nDailyCreatureVariationCount; }
		set { m_nDailyCreatureVariationCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventCreatureParticipate;
	public event Delegate EventCreatureParticipationCancel;
	public event Delegate EventCreatureCheer;
	public event Delegate EventCreatureCheerCancel;
	public event Delegate EventCreatureRear;
	public event Delegate EventCreatureRelease;
	public event Delegate<bool, bool> EventCreatureInject;
	public event Delegate EventCreatureInjectionRetrieval;
	public event Delegate EventCreatureVariation;
	public event Delegate EventCreatureAdditionalAttrSwitch;
	public event Delegate EventCreatureSkillSlotOpen;
	public event Delegate<Guid> EventCreatureCompose;
	public event Delegate EventCreatureEggUse;

	public event Delegate<Guid, int> EventHeroCreatureParticipated;
	public event Delegate<Guid> EventHeroCreatureParticipationCanceled;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroCreature[] creatures, Guid participatedCreatureId, int dailyCreatureVariationCount, DateTime dtDate)
	{
		UnInit();

		m_listCsHeroCreature = new List<CsHeroCreature>();

		for (int i = 0; i < creatures.Length; i++)
		{
			m_listCsHeroCreature.Add(new CsHeroCreature(creatures[i]));
		}

		m_guidParticipatedCreatureId = participatedCreatureId;
		m_nDailyCreatureVariationCount = dailyCreatureVariationCount;
		m_dtCreatureVariationCountDate = dtDate;

		// Command
		CsRplzSession.Instance.EventResCreatureParticipate += OnEventResCreatureParticipate;
		CsRplzSession.Instance.EventResCreatureParticipationCancel += OnEventResCreatureParticipationCancel;
		CsRplzSession.Instance.EventResCreatureCheer += OnEventResCreatureCheer;
		CsRplzSession.Instance.EventResCreatureCheerCancel += OnEventResCreatureCheerCancel;
		CsRplzSession.Instance.EventResCreatureRear += OnEventResCreatureRear;
		CsRplzSession.Instance.EventResCreatureRelease += OnEventResCreatureRelease;
		CsRplzSession.Instance.EventResCreatureInject += OnEventResCreatureInject;
		CsRplzSession.Instance.EventResCreatureInjectionRetrieval += OnEventResCreatureInjectionRetrieval;
		CsRplzSession.Instance.EventResCreatureVary += OnEventResCreatureVary;
		CsRplzSession.Instance.EventResCreatureAdditionalAttrSwitch += OnEventResCreatureAdditionalAttrSwitch;
		CsRplzSession.Instance.EventResCreatureSkillSlotOpen += OnEventResCreatureSkillSlotOpen;
		CsRplzSession.Instance.EventResCreatureCompose += OnEventResCreatureCompose;
		CsRplzSession.Instance.EventResCreatureEggUse += OnEventResCreatureEggUse;

		// Event
		CsRplzSession.Instance.EventEvtHeroCreatureParticipated += OnEventEvtHeroCreatureParticipated;
		CsRplzSession.Instance.EventEvtHeroCreatureParticipationCanceled += OnEventEvtHeroCreatureParticipationCanceled;

	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResCreatureParticipate -= OnEventResCreatureParticipate;
		CsRplzSession.Instance.EventResCreatureParticipationCancel -= OnEventResCreatureParticipationCancel;
		CsRplzSession.Instance.EventResCreatureCheer -= OnEventResCreatureCheer;
		CsRplzSession.Instance.EventResCreatureCheerCancel -= OnEventResCreatureCheerCancel;
		CsRplzSession.Instance.EventResCreatureRear -= OnEventResCreatureRear;
		CsRplzSession.Instance.EventResCreatureRelease -= OnEventResCreatureRelease;
		CsRplzSession.Instance.EventResCreatureInject -= OnEventResCreatureInject;
		CsRplzSession.Instance.EventResCreatureInjectionRetrieval -= OnEventResCreatureInjectionRetrieval;
		CsRplzSession.Instance.EventResCreatureVary -= OnEventResCreatureVary;
		CsRplzSession.Instance.EventResCreatureAdditionalAttrSwitch -= OnEventResCreatureAdditionalAttrSwitch;
		CsRplzSession.Instance.EventResCreatureSkillSlotOpen -= OnEventResCreatureSkillSlotOpen;
		CsRplzSession.Instance.EventResCreatureCompose -= OnEventResCreatureCompose;
		CsRplzSession.Instance.EventResCreatureEggUse -= OnEventResCreatureEggUse;

		// Event
		CsRplzSession.Instance.EventEvtHeroCreatureParticipated -= OnEventEvtHeroCreatureParticipated;
		CsRplzSession.Instance.EventEvtHeroCreatureParticipationCanceled -= OnEventEvtHeroCreatureParticipationCanceled;

		m_bWaitResponse = false;

		if (m_listCsHeroCreature != null)
		{
			m_listCsHeroCreature.Clear();
			m_listCsHeroCreature = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroCreature GetHeroCreature(Guid guidInstanceId)
	{
		for (int i = 0; i < m_listCsHeroCreature.Count; i++)
		{
			if (m_listCsHeroCreature[i].InstanceId == guidInstanceId)
				return m_listCsHeroCreature[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroCreature(Guid guidInstanceId)
	{
		CsHeroCreature csHeroCreature = GetHeroCreature(guidInstanceId);

		if (csHeroCreature != null)
		{
			m_listCsHeroCreature.Remove(csHeroCreature);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroCreatures(PDHeroCreature[] heroCreatures)
	{
		for (int i = 0; i < heroCreatures.Length; i++)
		{ 
			m_listCsHeroCreature.Add(new CsHeroCreature(heroCreatures[i]));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckCreatureRear()
	{
		foreach (CsHeroCreature csHeroCreature in m_listCsHeroCreature)
		{
			bool bMaxLevel = csHeroCreature.Level >= CsGameData.Instance.CreatureLevelList.Max(creatureLevel => creatureLevel.Level);

			if (!bMaxLevel && CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureFood) > 0)
			{
				return true;
			}
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckCreatureVaritaion()
	{
		bool bItemRemaining = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureVariationRequiredItemId) >= m_nCreatureVariationRequireItemCount;

		if (!bItemRemaining)
			return false;

		bool bCountRemaining = CsCreatureManager.Instance.DailyCreatureVariationCount < CsGameData.Instance.MyHeroInfo.VipLevel.CreatureVariationMaxCount;

		if (!bCountRemaining)
			return false;

		bool bMaxValue = true;

		foreach (CsHeroCreature csHeroCreature in m_listCsHeroCreature)
		{
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
		}

		if (bMaxValue)
			return false;

		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckCreatureSwitch()
	{
		return CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CreatureAdditionalAttrSwitchRequiredItemId) >= m_nCreatureSwitchRequireItemCount;
	}

	//---------------------------------------------------------------------------------------------------
	public bool CheckCreatureInjection()
	{
		int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType((int)EnItemType.CreatureEssence);

		int nMaxInjectionLevel = CsGameData.Instance.CreatureInjectionLevelList.Max(injectionLevel => injectionLevel.InjectionLevel);

		bool bMaxLevel = true;

		foreach (CsHeroCreature csHeroCreature in m_listCsHeroCreature)
		{
			if (csHeroCreature.InjectionLevel < csHeroCreature.CreatureLevel.MaxInjectionLevel &&
				csHeroCreature.InjectionLevel < nMaxInjectionLevel &&
				nItemCount >= csHeroCreature.CreatureInjectionLevel.RequiredItemCount &&
				CsGameData.Instance.MyHeroInfo.Gold >= csHeroCreature.CreatureInjectionLevel.RequiredGold)
			{
				bMaxLevel = false;
				break;
			}
		}

		if (bMaxLevel)
			return false;

		return true;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 크리처출전
	public void SendCreatureParticipate(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureParticipateCommandBody cmdBody = new CreatureParticipateCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureParticipate, cmdBody);
		}
	}

	void OnEventResCreatureParticipate(int nReturnCode, CreatureParticipateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_guidParticipatedCreatureId = m_guidInstanceId;
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureParticipate != null)
			{
				EventCreatureParticipate();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처출전취소
	public void SendCreatureParticipationCancel(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureParticipationCancelCommandBody cmdBody = new CreatureParticipationCancelCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureParticipationCancel, cmdBody);
		}
	}

	void OnEventResCreatureParticipationCancel(int nReturnCode, CreatureParticipationCancelResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_guidParticipatedCreatureId = Guid.Empty;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureParticipationCancel != null)
			{
				EventCreatureParticipationCancel();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처응원
	public void SendCreatureCheer(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureCheerCommandBody cmdBody = new CreatureCheerCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCheer, cmdBody);
		}
	}

	void OnEventResCreatureCheer(int nReturnCode, CreatureCheerResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.Cheered = true;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureCheer != null)
			{
				EventCreatureCheer();
			}
		}
		else if (nReturnCode == 101)
		{
			//  응원중인 크리처 수가 최대입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처응원취소
	public void SendCreatureCheerCancel(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureCheerCancelCommandBody cmdBody = new CreatureCheerCancelCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCheerCancel, cmdBody);
		}
	}

	void OnEventResCreatureCheerCancel(int nReturnCode, CreatureCheerCancelResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.Cheered = false;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureCheerCancel != null)
			{
				EventCreatureCheerCancel();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처양육
	public void SendCreatureRear(Guid guidInstanceId, int nItemId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureRearCommandBody cmdBody = new CreatureRearCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			cmdBody.itemId = nItemId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureRear, cmdBody);
		}
	}

	void OnEventResCreatureRear(int nReturnCode, CreatureRearResponseBody resBody)
	{
		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.Level = resBody.creatureLevel;
			csHeroCreature.Exp = resBody.creatureExp;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureRear != null)
			{
				EventCreatureRear();
			}
		}
		else if (nReturnCode == 101)
		{
			// 크리처의 레벨이 최대레벨입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00501"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00502"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}

		m_bWaitResponse = false;
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처방생
	public void SendCreatureRelease(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureReleaseCommandBody cmdBody = new CreatureReleaseCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureRelease, cmdBody);
		}
	}

	void OnEventResCreatureRelease(int nReturnCode, CreatureReleaseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveHeroCreature(m_guidInstanceId);

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventCreatureRelease != null)
			{
				EventCreatureRelease();
			}
		}
		else if (nReturnCode == 101)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00601"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처주입
	public void SendCreatureInject(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureInjectCommandBody cmdBody = new CreatureInjectCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureInject, cmdBody);
		}
	}

	void OnEventResCreatureInject(int nReturnCode, CreatureInjectResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			bool bLevelUp = csHeroCreature.InjectionLevel < resBody.injectionLevel;

			csHeroCreature.InjectionLevel = resBody.injectionLevel;
			csHeroCreature.InjectionExp = resBody.injectionExp;
			csHeroCreature.InjectionItemCount = resBody.injectionItemCount;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureInject != null)
			{
				EventCreatureInject(resBody.isCritical, bLevelUp);
			}
		}
		else if (nReturnCode == 101)
		{
			// 크리처의 주입레벨이 최대레벨입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00702"));
		}
		else if (nReturnCode == 103)
		{
			// 골드가 부족합니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처주입회수
	public void SendCreatureInjectionRetrieval(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureInjectionRetrievalCommandBody cmdBody = new CreatureInjectionRetrievalCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureInjectionRetrieval, cmdBody);
		}
	}

	void OnEventResCreatureInjectionRetrieval(int nReturnCode, CreatureInjectionRetrievalResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.InjectionLevel = 1;
			csHeroCreature.InjectionExp = 0;
			csHeroCreature.InjectionItemCount = 0;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureInjectionRetrieval != null)
			{
				EventCreatureInjectionRetrieval();
			}
		}
		else if (nReturnCode == 101)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00801"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처변이
	public void SendCreatureVary(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureVaryCommandBody cmdBody = new CreatureVaryCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureVary, cmdBody);
		}
	}

	void OnEventResCreatureVary(int nReturnCode, CreatureVaryResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtCreatureVariationCountDate = resBody.date;
			m_nDailyCreatureVariationCount = resBody.dailyCreatureVariationCount;

			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.Quality = resBody.quality;
			csHeroCreature.UpdateCreatureBaseAttrs(resBody.baseAttrs);

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureVariation != null)
			{
				EventCreatureVariation();
			}
		}
		else if (nReturnCode == 101)
		{
			// 변이횟수가 최대횟수를 넘어갑니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00901"));
		}
		else if (nReturnCode == 102)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_00902"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처추가속성변환
	public void SendCreatureAdditionalAttrSwitch(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureAdditionalAttrSwitchCommandBody cmdBody = new CreatureAdditionalAttrSwitchCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureAdditionalAttrSwitch, cmdBody);
		}
	}

	void OnEventResCreatureAdditionalAttrSwitch(int nReturnCode, CreatureAdditionalAttrSwitchResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.UpdateCreatureAdditionalAttrs(resBody.additionalAttrIds);

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureAdditionalAttrSwitch != null)
			{
				EventCreatureAdditionalAttrSwitch();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01001"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처스킬슬롯개방
	public void SendCreatureSkillSlotOpen(Guid guidInstanceId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureSkillSlotOpenCommandBody cmdBody = new CreatureSkillSlotOpenCommandBody();
			cmdBody.instanceId = m_guidInstanceId = guidInstanceId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureSkillSlotOpen, cmdBody);
		}
	}

	void OnEventResCreatureSkillSlotOpen(int nReturnCode, CreatureSkillSlotOpenResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.AdditionalOpenSkillSlotCount = resBody.additionalOpenSkillSlotCount;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureSkillSlotOpen != null)
			{
				EventCreatureSkillSlotOpen();
			}
		}
		else if (nReturnCode == 101)
		{
			//  아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처합성
	public void SendCreatureCompose(Guid guidMainInstanceId, Guid guidMaterialInstanceId, int[] anProtectedIndices)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureComposeCommandBody cmdBody = new CreatureComposeCommandBody();
			cmdBody.mainInstanceId = m_guidInstanceId = guidMainInstanceId;
			cmdBody.materialInstanceId = m_guidMaterialInstanceId = guidMaterialInstanceId;
			cmdBody.protectedIndices = anProtectedIndices;

			CsRplzSession.Instance.Send(ClientCommandName.CreatureCompose, cmdBody);
		}
	}

	void OnEventResCreatureCompose(int nReturnCode, CreatureComposeResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			RemoveHeroCreature(m_guidMaterialInstanceId);

			CsHeroCreature csHeroCreature = GetHeroCreature(m_guidInstanceId);
			csHeroCreature.UpdateCreatureSkills(resBody.mainHeroCreatureSkills);

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureCompose != null)
			{
				EventCreatureCompose(m_guidInstanceId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 오픈되지 않은 스킬슬롯입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01201"));
		}
		else if (nReturnCode == 102)
		{
			// 빈 스킬슬롯입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01202"));
		}
		else if (nReturnCode == 103)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01203"));
		}
		else if (nReturnCode == 104)
		{
			// 인벤토리가 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01204"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처알사용
	public void SendCreatureEggUse(int nSlotIndex)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureEggUseCommandBody cmdBody = new CreatureEggUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			cmdBody.useCount = 1;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureEggUse, cmdBody);
		}
	}

	void OnEventResCreatureEggUse(int nReturnCode, CreatureEggUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			// 추가된 영웅크리처 목록
			CsCreatureManager.Instance.AddHeroCreatures(resBody.addedHeroCreatures);

			List<CsHeroCreature> heroCreatureList = new List<CsHeroCreature>();

			for (int i = 0; i < resBody.addedHeroCreatures.Length; i++)
			{
				heroCreatureList.Add(new CsHeroCreature(resBody.addedHeroCreatures[i]));
			}

			// 획득표시
			CsGameEventUIToUI.Instance.OnEventGetHeroCreature(heroCreatureList);

			if (EventCreatureEggUse != null)
			{
				EventCreatureEggUse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A145_ERROR_01203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 크리처출전
	void OnEventEvtHeroCreatureParticipated(SEBHeroCreatureParticipatedEventBody eventBody)
	{
		if (EventHeroCreatureParticipated != null)
		{
			EventHeroCreatureParticipated(eventBody.heroId, eventBody.creatureId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처출전취소
	void OnEventEvtHeroCreatureParticipationCanceled(SEBHeroCreatureParticipationCanceledEventBody eventBody)
	{
		if (EventHeroCreatureParticipationCanceled != null)
		{
			EventHeroCreatureParticipationCanceled(eventBody.heroId);
		}
	}

	#endregion Protocol.Event
}
