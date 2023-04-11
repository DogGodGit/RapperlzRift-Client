using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;
using UnityEngine;

public class CsCostumeManager
{
	bool m_bWaitResponse = false;

	List<CsHeroCostume> m_listCsHeroCostume;
	int m_nEquippedHeroCostumeId;
	int m_nCostumeCollectionId;
	bool m_bCostumeCollectionActivated;

	int m_nHeroCostumeId;
	int m_nCostumeEffectId;

	//---------------------------------------------------------------------------------------------------
	public static CsCostumeManager Instance
	{
		get { return CsSingleton<CsCostumeManager>.GetInstance(); }
	}

	public List<CsHeroCostume> HeroCostumeList
	{
		get { return m_listCsHeroCostume; }
	}

	public int EquippedHeroCostumeId
	{
		get { return m_nEquippedHeroCostumeId; }
	}

	public int CostumeCollectionId
	{
		get { return m_nCostumeCollectionId; }
	}

	public bool CostumeCollectionActivated
	{
		get { return m_bCostumeCollectionActivated; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventCostumeItemUse;
	public event Delegate<int, int> EventCostumeEquip;
	public event Delegate EventCostumeUnequip;
	public event Delegate<int> EventCostumeEffectApply;
	public event Delegate<bool> EventCostumeEnchant;
	public event Delegate EventCostumeCollectionShuffle;
	public event Delegate EventCostumeCollectionActivate;

	public event Delegate EventCostumePeriodExpired;
	public event Delegate<Guid, int,int> EventHeroCostumeEquipped;
	public event Delegate<Guid> EventHeroCostumeUnequipped;
	public event Delegate<Guid, int> EventHeroCostumeEffectApplied;

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroCostume[] costumes, int equippedHeroCostumeId, int nCostumeCollectionId, bool bCostumeCollectionActivated)
	{
		UnInit();

		m_listCsHeroCostume = new List<CsHeroCostume>();

		for (int i = 0; i < costumes.Length; i++)
		{
			m_listCsHeroCostume.Add(new CsHeroCostume(costumes[i]));
		}

		m_nEquippedHeroCostumeId = equippedHeroCostumeId;

		m_nCostumeCollectionId = nCostumeCollectionId;
		m_bCostumeCollectionActivated = bCostumeCollectionActivated;

		// Command
		CsRplzSession.Instance.EventResCostumeItemUse += OnEventResCostumeItemUse;
		CsRplzSession.Instance.EventResCostumeEquip += OnEventResCostumeEquip;
		CsRplzSession.Instance.EventResCostumeUnequip += OnEventResCostumeUnequip;
		CsRplzSession.Instance.EventResCostumeEffectApply += OnEventResCostumeEffectApply;
		CsRplzSession.Instance.EventResCostumeEnchant += OnEventResCostumeEnchant;
		CsRplzSession.Instance.EventResCostumeCollectionShuffle += OnEventResCostumeCollectionShuffle;
		CsRplzSession.Instance.EventResCostumeCollectionActivate += OnEventResCostumeCollectionActivate;

		// Event
		CsRplzSession.Instance.EventEvtCostumePeriodExpired += OnEventEvtCostumePeriodExpired;
		CsRplzSession.Instance.EventEvtHeroCostumeEquipped += OnEventEvtHeroCostumeEquipped;
		CsRplzSession.Instance.EventEvtHeroCostumeUnequipped += OnEventEvtHeroCostumeUnequipped;
		CsRplzSession.Instance.EventEvtHeroCostumeEffectApplied += OnEventEvtHeroCostumeEffectApplied;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResCostumeItemUse -= OnEventResCostumeItemUse;
		CsRplzSession.Instance.EventResCostumeEquip -= OnEventResCostumeEquip;
		CsRplzSession.Instance.EventResCostumeUnequip -= OnEventResCostumeUnequip;
		CsRplzSession.Instance.EventResCostumeEffectApply -= OnEventResCostumeEffectApply;
		CsRplzSession.Instance.EventResCostumeEnchant -= OnEventResCostumeEnchant;
		CsRplzSession.Instance.EventResCostumeCollectionShuffle -= OnEventResCostumeCollectionShuffle;
		CsRplzSession.Instance.EventResCostumeCollectionActivate -= OnEventResCostumeCollectionActivate;

		// Event
		CsRplzSession.Instance.EventEvtCostumePeriodExpired -= OnEventEvtCostumePeriodExpired;
		CsRplzSession.Instance.EventEvtHeroCostumeEquipped -= OnEventEvtHeroCostumeEquipped;
		CsRplzSession.Instance.EventEvtHeroCostumeUnequipped -= OnEventEvtHeroCostumeUnequipped;
		CsRplzSession.Instance.EventEvtHeroCostumeEffectApplied -= OnEventEvtHeroCostumeEffectApplied;

		m_bWaitResponse = false;

		if (m_listCsHeroCostume != null)
		{
			m_listCsHeroCostume.Clear();
			m_listCsHeroCostume = null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	CsHeroCostume GetHeroCostume(int nHeroCostumeId)
	{
		for (int i = 0; i < m_listCsHeroCostume.Count; i++)
		{
			if (m_listCsHeroCostume[i].HeroCostumeId == nHeroCostumeId)
				return m_listCsHeroCostume[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void RemoveHeroCostume(int nHeroCostumeId)
	{
		CsHeroCostume csHeroCostume = GetHeroCostume(nHeroCostumeId);

		if (csHeroCostume != null)
		{
			m_listCsHeroCostume.Remove(csHeroCostume);
		}
	}


	//---------------------------------------------------------------------------------------------------
	public int GetMyHeroCostumeId()
	{
		CsHeroCostume csHeroCostume = GetHeroCostume(m_nEquippedHeroCostumeId);
		if (csHeroCostume != null)
		{
			return csHeroCostume.Costume.CostumeId;
		}
		return 0;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetMyHeroCostumeEffectId()
	{
		CsHeroCostume csHeroCostume = GetHeroCostume(m_nEquippedHeroCostumeId);
		if (csHeroCostume != null)
		{
			if (csHeroCostume.CostumeEffect != null)
			{
				return csHeroCostume.CostumeEffect.CostumeEffectId;
			}
		}
		return 0;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 코스튬아이템사용
	public void SendCostumeItemUse(int nSlotIndex)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeItemUseCommandBody cmdBody = new CostumeItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.CostumeItemUse, cmdBody);
		}
	}

	void OnEventResCostumeItemUse(int nReturnCode, CostumeItemUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCostume csHeroCostume = new CsHeroCostume(resBody.costumeId, resBody.remainingTime);
			m_listCsHeroCostume.Add(csHeroCostume);

            PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			if (EventCostumeItemUse != null)
			{
				EventCostumeItemUse();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 코스튬장착
	public void SendCostumeEquip(int nHeroCostumeId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeEquipCommandBody cmdBody = new CostumeEquipCommandBody();
			cmdBody.costumeId = m_nHeroCostumeId = nHeroCostumeId;
			CsRplzSession.Instance.Send(ClientCommandName.CostumeEquip, cmdBody);
		}
	}

	void OnEventResCostumeEquip(int nReturnCode, CostumeEquipResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nEquippedHeroCostumeId = m_nHeroCostumeId;

			CsHeroCostume csHeroCostume = GetHeroCostume(m_nEquippedHeroCostumeId);
			int nCostumeEffectId = csHeroCostume.CostumeEffect == null ? 0 : csHeroCostume.CostumeEffect.CostumeEffectId;

			if (EventCostumeEquip != null)
			{
				EventCostumeEquip(csHeroCostume.Costume.CostumeId, nCostumeEffectId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 영웅레벨이 부족합니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 코스튬장착해제
	public void SendCostumeUnequip()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeUnequipCommandBody cmdBody = new CostumeUnequipCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CostumeUnequip, cmdBody);
		}
	}

	void OnEventResCostumeUnequip(int nReturnCode, CostumeUnequipResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nEquippedHeroCostumeId = -1;

			if (EventCostumeUnequip != null)
			{
				EventCostumeUnequip();
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 효스튬효과적용
	public void SendCostumeEffectApply(int nHeroCostumeId, int costumeEffectId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeEffectApplyCommandBody cmdBody = new CostumeEffectApplyCommandBody();
			cmdBody.costumeId = m_nHeroCostumeId = nHeroCostumeId;
			cmdBody.costumeEffectId = m_nCostumeEffectId = costumeEffectId;
			CsRplzSession.Instance.Send(ClientCommandName.CostumeEffectApply, cmdBody);
		}
	}

	void OnEventResCostumeEffectApply(int nReturnCode, CostumeEffectApplyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCostume csHeroCostume = GetHeroCostume(m_nHeroCostumeId);
			csHeroCostume.EffectId = m_nCostumeEffectId;

            PDInventorySlot[] slots = new PDInventorySlot[] { resBody.changedInventorySlot };
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(slots);

			if (EventCostumeEffectApply != null)
			{
				EventCostumeEffectApply(csHeroCostume.EffectId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 코스튬강화
	public void SendCostumeEnchant(int nCostumeId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeEnchantCommandBody cmdBody = new CostumeEnchantCommandBody();
			cmdBody.costumeId = m_nHeroCostumeId = nCostumeId;
			CsRplzSession.Instance.Send(ClientCommandName.CostumeEnchant, cmdBody);
		}
	}

	void OnEventResCostumeEnchant(int nReturnCode, CostumeEnchantResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsHeroCostume csHeroCostume = GetHeroCostume(m_nHeroCostumeId);
            bool bEnchant = csHeroCostume.EnchantLevel != resBody.enchantLevel;
			csHeroCostume.EnchantLevel = resBody.enchantLevel;
			csHeroCostume.LuckyValue = resBody.luckyValue;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCostumeEnchant != null)
			{
                EventCostumeEnchant(bEnchant);
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00501"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 코스튬콜렉션셔플
	public void SendCostumeCollectionShuffle()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeCollectionShuffleCommandBody cmdBody = new CostumeCollectionShuffleCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CostumeCollectionShuffle, cmdBody);
		}
	}

	void OnEventResCostumeCollectionShuffle(int nReturnCode, CostumeCollectionShuffleResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nCostumeCollectionId = resBody.collectionId;
			m_bCostumeCollectionActivated = false;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCostumeCollectionShuffle != null)
			{
				EventCostumeCollectionShuffle();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00601"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 코스튬콜렉션활성화
	public void SendCostumeCollectionActivate()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CostumeCollectionActivateCommandBody cmdBody = new CostumeCollectionActivateCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CostumeCollectionActivate, cmdBody);
		}
	}

	void OnEventResCostumeCollectionActivate(int nReturnCode, CostumeCollectionActivateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_bCostumeCollectionActivated = true;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCostumeCollectionActivate != null)
			{
				EventCostumeCollectionActivate();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00701"));
		}
		else if (nReturnCode == 102)
		{
			// 필요코스튬을 보유하고 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A146_ERROR_00702"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 코스튬기간만료
	void OnEventEvtCostumePeriodExpired(SEBCostumePeriodExpiredEventBody eventBody)
	{
		if (eventBody.costumeId == m_nEquippedHeroCostumeId)
		{
			m_nEquippedHeroCostumeId = -1;
		}

		RemoveHeroCostume(eventBody.costumeId);

		if (EventCostumePeriodExpired != null)
		{
			EventCostumePeriodExpired();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅코스튬장착
	void OnEventEvtHeroCostumeEquipped(SEBHeroCostumeEquippedEventBody eventBody)
	{
		if (EventHeroCostumeEquipped != null)
		{
			EventHeroCostumeEquipped(eventBody.heroId, eventBody.costumeId, eventBody.costumeEffectId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅코스튬장착해제
	void OnEventEvtHeroCostumeUnequipped(SEBHeroCostumeUnequippedEventBody eventBody)
	{
		if (EventHeroCostumeUnequipped != null)
		{
			EventHeroCostumeUnequipped(eventBody.heroId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅코스튬효과적용
	void OnEventEvtHeroCostumeEffectApplied(SEBHeroCostumeEffectAppliedEventBody eventBody)
	{
		if (EventHeroCostumeEffectApplied != null)
		{
			EventHeroCostumeEffectApplied(eventBody.heroId, eventBody.costumeEffectId);
		}
	}


	#endregion Protocol.Event

    //---------------------------------------------------------------------------------------------------
    public bool CheckCostumeEnchant()
    {
        if (m_listCsHeroCostume.Count == 0)
        {
            return false;
        }
        else
        {
            bool bCostumeEnchant = false;

            CsHeroCostume csHeroCostume = null;
            CsCostumeEnchantLevel csCostumeEnchantLevel = null;

            for (int i = 0; i < m_listCsHeroCostume.Count; i++)
            {
                csHeroCostume = m_listCsHeroCostume[i];
                csCostumeEnchantLevel = CsGameData.Instance.CostumeEnchantLevelList.Find(a => a.EnchantLevel == csHeroCostume.EnchantLevel);

                // 최대 레벨이 아니며 강화에 필요한 아이템 개수가 충분하면
                if (csCostumeEnchantLevel.EnchantLevel != CsGameData.Instance.CostumeEnchantLevelList[CsGameData.Instance.CostumeEnchantLevelList.Count - 1].EnchantLevel &&
                    csCostumeEnchantLevel.NextLevelRequiredItemCount <= CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.CostumeEnchantItemId))
                {
                    bCostumeEnchant = true;
                    break;
                }
            }

            return bCostumeEnchant;
        }
    }
}
