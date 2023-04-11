using System;
using System.Collections.Generic;
using ClientCommon;
using UnityEngine;

public class CsLuckyShopManager
{
	bool m_bWaitResponse = false;

	DateTime m_dtItemLuckyShopPickDate;
	float m_flItemLuckyShopFreePickRemainingTime;
	int m_nItemLuckyShopFreePickCount;
	int m_nItemLuckyShopPick1TimeCount;
	int m_nItemLuckyShopPick5TimeCount;

	DateTime m_dtCreatureCardLuckyShopPickDate;
	float m_flCreatureCardLuckyShopFreePickRemainingTime;
	int m_nCreatureCardLuckyShopFreePickCount;
	int m_nCreatureCardLuckyShopPick1TimeCount;
	int m_nCreatureCardLuckyShopPick5TimeCount;

	//---------------------------------------------------------------------------------------------------
	public static CsLuckyShopManager Instance
	{
		get { return CsSingleton<CsLuckyShopManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public DateTime ItemLuckyShopPickDate
	{
		get { return m_dtItemLuckyShopPickDate; }
		set { m_dtItemLuckyShopPickDate = value; }
	}

	public float ItemLuckyShopFreePickRemainingTime
	{
		get { return m_flItemLuckyShopFreePickRemainingTime; }
	}

	public int ItemLuckyShopFreePickCount
	{
		get { return m_nItemLuckyShopFreePickCount; }
		set { m_nItemLuckyShopFreePickCount = value; }
	}

	public int ItemLuckyShopPick1TimeCount
	{
		get { return m_nItemLuckyShopPick1TimeCount; }
		set { m_nItemLuckyShopPick1TimeCount = value; }
	}

	public int ItemLuckyShopPick5TimeCount
	{
		get { return m_nItemLuckyShopPick5TimeCount; }
		set { m_nItemLuckyShopPick5TimeCount = value; }
	}

	public DateTime CreatureCardLuckyShopPickDate
	{
		get { return m_dtCreatureCardLuckyShopPickDate; }
		set { m_dtCreatureCardLuckyShopPickDate = value; }
	}

	public float CreatureCardLuckyShopFreePickRemainingTime
	{
		get { return m_flCreatureCardLuckyShopFreePickRemainingTime; }
	}

	public int CreatureCardLuckyShopFreePickCount
	{
		get { return m_nCreatureCardLuckyShopFreePickCount; }
		set { m_nCreatureCardLuckyShopFreePickCount = value; }
	}

	public int CreatureCardLuckyShopPick1TimeCount
	{
		get { return m_nCreatureCardLuckyShopPick1TimeCount; }
		set { m_nCreatureCardLuckyShopPick1TimeCount = value; }
	}

	public int CreatureCardLuckyShopPick5TimeCount
	{
		get { return m_nCreatureCardLuckyShopPick5TimeCount; }
		set { m_nCreatureCardLuckyShopPick5TimeCount = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<PDItemLuckyShopPickResult> EventItemLuckyShopFreePick;
	public event Delegate<PDItemLuckyShopPickResult> EventItemLuckyShop1TimePick;
	public event Delegate<PDItemLuckyShopPickResult[]> EventItemLuckyShop5TimePick;
	public event Delegate<CsHeroCreatureCard> EventCreatureCardLuckyShopFreePick;
	public event Delegate<CsHeroCreatureCard> EventCreatureCardLuckyShop1TimePick;
	public event Delegate<List<CsHeroCreatureCard>> EventCreatureCardLuckyShop5TimePick;

	//---------------------------------------------------------------------------------------------------
	public void Init(DateTime dtDate, float itemLuckyShopFreePickRemainingTime, int itemLuckyShopFreePickCount, int itemLuckyShopPick1TimeCount, int itemLuckyShopPick5TimeCount,
					 float creatureCardLuckyShopFreePickRemainingTime, int creatureCardLuckyShopFreePickCount, int creatureCardLuckyShopPick1TimeCount, int creatureCardLuckyShopPick5TimeCount)
	{
		UnInit();

		m_dtItemLuckyShopPickDate = dtDate;
		m_flItemLuckyShopFreePickRemainingTime = itemLuckyShopFreePickRemainingTime + Time.realtimeSinceStartup;
		m_nItemLuckyShopFreePickCount = itemLuckyShopFreePickCount;
		m_nItemLuckyShopPick1TimeCount = itemLuckyShopPick1TimeCount;
		m_nItemLuckyShopPick5TimeCount = itemLuckyShopPick5TimeCount;

		m_dtCreatureCardLuckyShopPickDate = dtDate;
		m_flCreatureCardLuckyShopFreePickRemainingTime = creatureCardLuckyShopFreePickRemainingTime + Time.realtimeSinceStartup;
		m_nCreatureCardLuckyShopFreePickCount = creatureCardLuckyShopFreePickCount;
		m_nCreatureCardLuckyShopPick1TimeCount = creatureCardLuckyShopPick1TimeCount;
		m_nCreatureCardLuckyShopPick5TimeCount = creatureCardLuckyShopPick5TimeCount;

		// Command
		CsRplzSession.Instance.EventResItemLuckyShopFreePick += OnEventResItemLuckyShopFreePick;
		CsRplzSession.Instance.EventResItemLuckyShop1TimePick += OnEventResItemLuckyShop1TimePick;
		CsRplzSession.Instance.EventResItemLuckyShop5TimePick += OnEventResItemLuckyShop5TimePick;

		CsRplzSession.Instance.EventResCreatureCardLuckyShopFreePick += OnEventResCreatureCardLuckyShopFreePick;
		CsRplzSession.Instance.EventResCreatureCardLuckyShop1TimePick += OnEventResCreatureCardLuckyShop1TimePick;
		CsRplzSession.Instance.EventResCreatureCardLuckyShop5TimePick += OnEventResCreatureCardLuckyShop5TimePick;

		// Event
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResItemLuckyShopFreePick -= OnEventResItemLuckyShopFreePick;
		CsRplzSession.Instance.EventResItemLuckyShop1TimePick -= OnEventResItemLuckyShop1TimePick;
		CsRplzSession.Instance.EventResItemLuckyShop5TimePick -= OnEventResItemLuckyShop5TimePick;

		CsRplzSession.Instance.EventResCreatureCardLuckyShopFreePick -= OnEventResCreatureCardLuckyShopFreePick;
		CsRplzSession.Instance.EventResCreatureCardLuckyShop1TimePick -= OnEventResCreatureCardLuckyShop1TimePick;
		CsRplzSession.Instance.EventResCreatureCardLuckyShop5TimePick -= OnEventResCreatureCardLuckyShop5TimePick;

		// Event

		m_bWaitResponse = false;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 아이템행운상점무료뽑기
	public void SendItemLuckyShopFreePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ItemLuckyShopFreePickCommandBody cmdBody = new ItemLuckyShopFreePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ItemLuckyShopFreePick, cmdBody);
		}
	}

	void OnEventResItemLuckyShopFreePick(int nReturnCode, ItemLuckyShopFreePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nItemLuckyShopFreePickCount = resBody.freePickCount;
			m_flItemLuckyShopFreePickRemainingTime = resBody.freePickRemainingTime + Time.realtimeSinceStartup;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);
            
			if (EventItemLuckyShopFreePick != null)
			{
				EventItemLuckyShopFreePick(resBody.pickResult);
			}
		}
		else if (nReturnCode == 101)
		{
			// 아직 무료뽑기시간이 경과하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 무료뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00103"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 아이템행운상점1회뽑기
	public void SendItemLuckyShop1TimePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ItemLuckyShop1TimePickCommandBody cmdBody = new ItemLuckyShop1TimePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ItemLuckyShop1TimePick, cmdBody);
		}
	}

	void OnEventResItemLuckyShop1TimePick(int nReturnCode, ItemLuckyShop1TimePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nItemLuckyShopPick1TimeCount = resBody.pick1TimeCount;

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventItemLuckyShop1TimePick != null)
			{
				EventItemLuckyShop1TimePick(resBody.pickResult);
			}
		}
		else if (nReturnCode == 101)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 1회뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 아이템행운상점5회뽑기
	public void SendItemLuckyShop5TimePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ItemLuckyShop5TimePickCommandBody cmdBody = new ItemLuckyShop5TimePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.ItemLuckyShop5TimePick, cmdBody);
		}
	}

	void OnEventResItemLuckyShop5TimePick(int nReturnCode, ItemLuckyShop5TimePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nItemLuckyShopPick5TimeCount = resBody.pick5TimeCount;

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			CsGameData.Instance.MyHeroInfo.AddInventorySlots(resBody.changedInventorySlots);

			if (EventItemLuckyShop5TimePick != null)
			{
				EventItemLuckyShop5TimePick(resBody.pickResults);
			}
		}
		else if (nReturnCode == 101)
		{
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00301"));
		}
        else if (nReturnCode == 102)
        {
            // 5회뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00302"));
        }
        else if (nReturnCode == 103)
        {
            // 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00303"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점행운상점무료뽑기
	public void SendCreatureCardLuckyShopFreePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureCardLuckyShopFreePickCommandBody cmdBody = new CreatureCardLuckyShopFreePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardLuckyShopFreePick, cmdBody);
		}
	}

	void OnEventResCreatureCardLuckyShopFreePick(int nReturnCode, CreatureCardLuckyShopFreePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nCreatureCardLuckyShopFreePickCount = resBody.freePickCount;
			m_flCreatureCardLuckyShopFreePickRemainingTime = resBody.freePickRemainingTime + Time.realtimeSinceStartup;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			PDHeroCreatureCard[] creatureCards = new PDHeroCreatureCard[] { resBody.changedCreatureCard };
			CsCreatureCardManager.Instance.AddHeroCreatureCards(creatureCards);

			if (EventCreatureCardLuckyShopFreePick != null)
			{
				EventCreatureCardLuckyShopFreePick(new CsHeroCreatureCard(resBody.changedCreatureCard));
			}
		}
		else if (nReturnCode == 101)
		{
			// 아직 무료뽑기시간이 경과하지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 무료뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점행운상점1회뽑기
	public void SendCreatureCardLuckyShop1TimePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureCardLuckyShop1TimePickCommandBody cmdBody = new CreatureCardLuckyShop1TimePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardLuckyShop1TimePick, cmdBody);
		}
	}

	void OnEventResCreatureCardLuckyShop1TimePick(int nReturnCode, CreatureCardLuckyShop1TimePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nCreatureCardLuckyShopPick1TimeCount = resBody.pick1TimeCount;

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			PDHeroCreatureCard[] creatureCards = new PDHeroCreatureCard[] { resBody.changedCreatureCard };
			CsCreatureCardManager.Instance.AddHeroCreatureCards(creatureCards);

			if (EventCreatureCardLuckyShop1TimePick != null)
			{
				EventCreatureCardLuckyShop1TimePick(new CsHeroCreatureCard(resBody.changedCreatureCard));
			}
		}
		else if (nReturnCode == 101)
		{
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00201"));
		}
        else if (nReturnCode == 102)
        {
            // 1회뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00202"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점행운상점5회뽑기
	public void SendCreatureCardLuckyShop5TimePick()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			CreatureCardLuckyShop5TimePickCommandBody cmdBody = new CreatureCardLuckyShop5TimePickCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardLuckyShop5TimePick, cmdBody);
		}
	}

	void OnEventResCreatureCardLuckyShop5TimePick(int nReturnCode, CreatureCardLuckyShop5TimePickResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nCreatureCardLuckyShopPick5TimeCount = resBody.pick5TimeCount;

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			CsGameData.Instance.MyHeroInfo.Gold = resBody.gold;
			CsAccomplishmentManager.Instance.MaxGold = resBody.maxGold;

			CsCreatureCardManager.Instance.AddHeroCreatureCards(resBody.changedCreatureCards);

            List<CsHeroCreatureCard> list = new List<CsHeroCreatureCard>();
            for (int i = 0; i < resBody.changedCreatureCards.Length; i++)
            {
                list.Add(new CsHeroCreatureCard(resBody.changedCreatureCards[i]));
            }
            
			if (EventCreatureCardLuckyShop5TimePick != null)
			{
                EventCreatureCardLuckyShop5TimePick(list);
			}
		}
		else if (nReturnCode == 101)
		{
            // 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00301"));
		}
        else if (nReturnCode == 102)
        {
            // 5회뽑기횟수가 최대횟수를 넘어갑니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00302"));
        }
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event
	#endregion Protocol.Event

    public bool CheckNoticeLuckyShop()
    {
        if (CsUIData.Instance.MenuOpen((int)EnMenuId.LuckyShop))
        {
            if ((m_flItemLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup <= 0f && m_nItemLuckyShopFreePickCount < CsGameData.Instance.ItemLuckyShop.FreePickCount) ||
                (m_flCreatureCardLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup <= 0f && m_nCreatureCardLuckyShopFreePickCount < CsGameData.Instance.CreatureCardLuckyShop.FreePickCount))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
