using System.Collections.Generic;
using ClientCommon;
using System;

public class CsCreatureCardManager
{
	bool m_bWaitResponse = false;

	List<CsHeroCreatureCard> m_listCsHeroCreatureCard;      // 보유한 크리쳐카드 목록
	List<int> m_listActivatedCreatureCardCollection;        // 활성화된 크리쳐카드컬렉션 목록	

	int m_nCreatureCardCollectionFamePoint;                 // 크리쳐카드컬렉션명성점수
	List<int> m_listPurchasedCreatureCardShopFixedProduct;  // 구매한 크리쳐카드상점고정상품 목록. 항목 : 고정상품ID
	List<CsHeroCreatureCardShopRandomProduct> m_listCsHeroCreatureCardShopRandomProduct;    // 크리쳐카드상점랜덤상품목록
	int m_nDailyCreatureCardShopPaidRefreshCount;           // 금일크리쳐카드상점유료갱신횟수
	DateTime m_dtCreatureCardShopPaidRefreshCountDate;      // 금일크리쳐카드상점유료갱신날짜

	int m_nCollectionId;
	int m_nProductId;

	//---------------------------------------------------------------------------------------------------
	public static CsCreatureCardManager Instance
	{
		get { return CsSingleton<CsCreatureCardManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate<int> EventCreatureCardCollectionActivate;
	public event Delegate EventCreatureCardCompose;
	public event Delegate<int> EventCreatureCardDisassemble;
	public event Delegate<int> EventCreatureCardDisassembleAll;

	public event Delegate EventCreatureCardShopPaidRefresh;
	public event Delegate EventCreatureCardShopFixedProductBuy;
	public event Delegate EventCreatureCardShopRandomProductBuy;
	public event Delegate EventCreatureCardShopRefreshed;

	//---------------------------------------------------------------------------------------------------
	public List<CsHeroCreatureCard> HeroCreatureCardList
	{
		get { return m_listCsHeroCreatureCard; }
	}

	public List<int> ActivatedCreatureCardCollectionList
	{
		get { return m_listActivatedCreatureCardCollection; }
	}

	public int CreatureCardCollectionFamePoint
	{
		get { return m_nCreatureCardCollectionFamePoint; }
		set { m_nCreatureCardCollectionFamePoint = value; }
	}

	public List<int> PurchasedCreatureCardShopFixedProductList
	{
		get { return m_listPurchasedCreatureCardShopFixedProduct; }
	}

	public List<CsHeroCreatureCardShopRandomProduct> HeroCreatureCardShopRandomProductList
	{
		get { return m_listCsHeroCreatureCardShopRandomProduct; }
	}

	public int DailyCreatureCardShopPaidRefreshCount
	{
		get { return m_nDailyCreatureCardShopPaidRefreshCount; }
		set { m_nDailyCreatureCardShopPaidRefreshCount = value; }
	}

	public DateTime CreatureCardShopPaidRefreshCountDate
	{
		get { return m_dtCreatureCardShopPaidRefreshCountDate; }
		set { m_dtCreatureCardShopPaidRefreshCountDate = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroCreatureCard[] creatureCards,		int[] activatedCreatureCardCollections, int nCreatureCardCollectionFamePoint, int[] purchasedCreatureCardShopFixedProducts, PDHeroCreatureCardShopRandomProduct[] creatureCardShopRandomProducts,
					 int nDailyCreatureCardShopPaidRefreshCount, DateTime dtDate)
	{
		UnInit();

		m_listCsHeroCreatureCard = new List<CsHeroCreatureCard>();

		for (int i = 0; i < creatureCards.Length; i++)
		{
			m_listCsHeroCreatureCard.Add(new CsHeroCreatureCard(creatureCards[i]));
		}

		m_listActivatedCreatureCardCollection = new List<int>(activatedCreatureCardCollections);

		m_nCreatureCardCollectionFamePoint = nCreatureCardCollectionFamePoint;
		m_listPurchasedCreatureCardShopFixedProduct = new List<int>(purchasedCreatureCardShopFixedProducts);
		m_listCsHeroCreatureCardShopRandomProduct = new List<CsHeroCreatureCardShopRandomProduct>();

		for (int i = 0; i < creatureCardShopRandomProducts.Length; i++)
		{
			m_listCsHeroCreatureCardShopRandomProduct.Add(new CsHeroCreatureCardShopRandomProduct(creatureCardShopRandomProducts[i]));
		}

		m_nDailyCreatureCardShopPaidRefreshCount = nDailyCreatureCardShopPaidRefreshCount;
		m_dtCreatureCardShopPaidRefreshCountDate = dtDate;

		// Command
		CsRplzSession.Instance.EventResCreatureCardCollectionActivate += OnEventResCreatureCardCollectionActivate;
		CsRplzSession.Instance.EventResCreatureCardCompose += OnEventResCreatureCardCompose;
		CsRplzSession.Instance.EventResCreatureCardDisassemble += OnEventResCreatureCardDisassemble;
		CsRplzSession.Instance.EventResCreatureCardDisassembleAll += OnEventResCreatureCardDisassembleAll;
		CsRplzSession.Instance.EventResCreatureCardShopPaidRefresh += OnEventResCreatureCardShopPaidRefresh;
		CsRplzSession.Instance.EventResCreatureCardShopFixedProductBuy += OnEventResCreatureCardShopFixedProductBuy;
		CsRplzSession.Instance.EventResCreatureCardShopRandomProductBuy += OnEventResCreatureCardShopRandomProductBuy;

		// Event
		CsRplzSession.Instance.EventEvtCreatureCardShopRefreshed += OnEventEvtCreatureCardShopRefreshed;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResCreatureCardCollectionActivate -= OnEventResCreatureCardCollectionActivate;
		CsRplzSession.Instance.EventResCreatureCardCompose -= OnEventResCreatureCardCompose;
		CsRplzSession.Instance.EventResCreatureCardDisassemble -= OnEventResCreatureCardDisassemble;
		CsRplzSession.Instance.EventResCreatureCardDisassembleAll -= OnEventResCreatureCardDisassembleAll;
		CsRplzSession.Instance.EventResCreatureCardShopPaidRefresh -= OnEventResCreatureCardShopPaidRefresh;
		CsRplzSession.Instance.EventResCreatureCardShopFixedProductBuy -= OnEventResCreatureCardShopFixedProductBuy;
		CsRplzSession.Instance.EventResCreatureCardShopRandomProductBuy -= OnEventResCreatureCardShopRandomProductBuy;

		// Event
		CsRplzSession.Instance.EventEvtCreatureCardShopRefreshed -= OnEventEvtCreatureCardShopRefreshed;

		m_bWaitResponse = false;
		m_listCsHeroCreatureCard = null;
		m_listActivatedCreatureCardCollection = null;
		m_nCreatureCardCollectionFamePoint = 0;
		m_listPurchasedCreatureCardShopFixedProduct = null;
		m_listCsHeroCreatureCardShopRandomProduct = null;
		m_nDailyCreatureCardShopPaidRefreshCount = 0;
		m_dtCreatureCardShopPaidRefreshCountDate = DateTime.Now;
		m_nCollectionId = 0;
		m_nProductId = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroCreatureCards(PDHeroCreatureCard[] creatureCards)
	{
		for (int i = 0; i < creatureCards.Length; i++)
		{
			AddHeroCreatureCard(creatureCards[i]);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void AddHeroCreatureCard(PDHeroCreatureCard creatureCard)
	{
		CsHeroCreatureCard csHeroCreatureCard = m_listCsHeroCreatureCard.Find(a => a.CreatureCard.CreatureCardId == creatureCard.creatureCardId);

		if (creatureCard.count > 0)
		{
			if (csHeroCreatureCard != null)
			{
				// 카운트 갱신
				csHeroCreatureCard.Count = creatureCard.count;
			}
			else
			{
				// 신규 추가
				m_listCsHeroCreatureCard.Add(new CsHeroCreatureCard(creatureCard));
			}
		}
		else
		{
			// 삭제
			m_listCsHeroCreatureCard.Remove(csHeroCreatureCard);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void RefreshProductList(PDHeroCreatureCardShopRandomProduct[] randomProducts)
	{
		m_listPurchasedCreatureCardShopFixedProduct.Clear();

		m_listCsHeroCreatureCardShopRandomProduct.Clear();

		for (int i = 0; i < randomProducts.Length; i++)
		{
			m_listCsHeroCreatureCardShopRandomProduct.Add(new CsHeroCreatureCardShopRandomProduct(randomProducts[i]));
        }
    }

    //---------------------------------------------------------------------------------------------------
    public CsHeroCreatureCard GetHeroCreatureCard(int nCreatureCardId)
    {
        for (int i = 0; i < m_listCsHeroCreatureCard.Count; i++)
        {
            if (m_listCsHeroCreatureCard[i].CreatureCard.CreatureCardId == nCreatureCardId)
                return m_listCsHeroCreatureCard[i];
        }

        return null;
    }

    //---------------------------------------------------------------------------------------------------
    public bool GetActivatedCreatureCardCollection(int nCollectionId)
    {
        if (m_listActivatedCreatureCardCollection.Find(a => a == nCollectionId) != 0)
            return true;
        else
            return false;
    }

	//---------------------------------------------------------------------------------------------------
	public bool CheckCreatureCardCollictionNotice()
	{
        if (!CsUIData.Instance.MenuOpen((int)EnMenuId.Collection))
            return false;

        List<CsCreatureCardCollectionCategory> listCategory = CsGameData.Instance.CreatureCardCollectionCategoryList;

		for (int i = 0; i < listCategory.Count; ++i)
		{
			int nCategoryId = listCategory[i].CategoryId;
			bool bActiveCheck = false;

			//해당카테고리의 컬렉션을 모두 가져옴
			List<CsCreatureCardCollection> list = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == nCategoryId);

			for (int j = 0; j < list.Count; ++j)
			{
				bool bActiveCollection = false;

				//만약 활성화된 컬렉션이라면 continue;
				for (int k = 0; k < m_listActivatedCreatureCardCollection.Count; ++k)
				{
					if (m_listActivatedCreatureCardCollection[k] == list[j].CollectionId)
					{
						bActiveCollection = true;
						break;
					}
				}

				if (bActiveCollection) continue;

				//해당 컬렉션의 Entry을 가져와 보유중인 카드와 비교한다.
				List<CsCreatureCardCollectionEntry> listEntry = CsGameData.Instance.GetCreatureCardCollectionEntryListByCollection(list[j].CollectionId);

				for (int k = 0; k < listEntry.Count; ++k)
				{
					if (GetHeroCreatureCard(listEntry[k].CreatureCard.CreatureCardId) != null)
					{
						bActiveCheck = true;
					}
					else
					{
						bActiveCheck = false;
						break;
					}
				}

				if (bActiveCheck)
					return true;
			}
		}

		return false;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 크리처카드컬렉션활성
	public void SendCreatureCardCollectionActivate(int nCollectionId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardCollectionActivateCommandBody cmdBody = new CreatureCardCollectionActivateCommandBody();
			cmdBody.collectionId = m_nCollectionId = nCollectionId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardCollectionActivate, cmdBody);
		}
	}

	void OnEventResCreatureCardCollectionActivate(int nReturnCode, CreatureCardCollectionActivateResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listActivatedCreatureCardCollection.Add(m_nCollectionId);
			AddHeroCreatureCards(resBody.changedCreatureCards);
			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			m_nCreatureCardCollectionFamePoint = resBody.creatureCardCollectionFamePoint;
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventCreatureCardCollectionActivate != null)
			{
				EventCreatureCardCollectionActivate(m_nCollectionId);
			}
		}
		else if (nReturnCode == 101)
		{
			// 컬렉션이 완성되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A86_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드합성
	public void SendCreatureCardCompose(int nCardId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardComposeCommandBody cmdBody = new CreatureCardComposeCommandBody();
			cmdBody.cardId = nCardId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardCompose, cmdBody);
		}
	}

	void OnEventResCreatureCardCompose(int nReturnCode, CreatureCardComposeResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			AddHeroCreatureCard(resBody.changedCreatureCard);
			CsGameData.Instance.MyHeroInfo.SoulPowder = resBody.soulPowder;

			if (EventCreatureCardCompose != null)
			{
				EventCreatureCardCompose();
			}
		}
		else if (nReturnCode == 101)
		{
			// VIP레벨이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A86_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 영혼가루가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A86_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드분해
	public void SendCreatureCardDisassemble(int nCardId, int nCount)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardDisassembleCommandBody cmdBody = new CreatureCardDisassembleCommandBody();
			cmdBody.cardId = nCardId;
			cmdBody.count = nCount;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardDisassemble, cmdBody);
		}
	}

	void OnEventResCreatureCardDisassemble(int nReturnCode, CreatureCardDisassembleResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            int nAcquiredSoulPowder =  resBody.soulPowder - CsGameData.Instance.MyHeroInfo.SoulPowder;
            AddHeroCreatureCard(resBody.changedCreatureCard);
			CsGameData.Instance.MyHeroInfo.SoulPowder = resBody.soulPowder;

			if (EventCreatureCardDisassemble != null)
			{
				EventCreatureCardDisassemble(nAcquiredSoulPowder);
			}
		}
		else if (nReturnCode == 101)
		{
			//  크리처카드 수량이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A86_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드전체분해
	public void SendCreatureCardDisassembleAll(int nCategoryId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardDisassembleAllCommandBody cmdBody = new CreatureCardDisassembleAllCommandBody();
            cmdBody.categoryId = nCategoryId;
            CsRplzSession.Instance.Send(ClientCommandName.CreatureCardDisassembleAll, cmdBody);
		}
	}

	void OnEventResCreatureCardDisassembleAll(int nReturnCode, CreatureCardDisassembleAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
            int nAcquiredSoulPowder = resBody.soulPowder - CsGameData.Instance.MyHeroInfo.SoulPowder;
            AddHeroCreatureCards(resBody.changedCreatureCards);
			CsGameData.Instance.MyHeroInfo.SoulPowder = resBody.soulPowder;

			if (EventCreatureCardDisassembleAll != null)
			{
				EventCreatureCardDisassembleAll(nAcquiredSoulPowder);
			}
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점유료갱신
	public void SendCreatureCardShopPaidRefresh()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardShopPaidRefreshCommandBody cmdBody = new CreatureCardShopPaidRefreshCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardShopPaidRefresh, cmdBody);
		}
	}

	void OnEventResCreatureCardShopPaidRefresh(int nReturnCode, CreatureCardShopPaidRefreshResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_dtCreatureCardShopPaidRefreshCountDate = resBody.date;
			m_nDailyCreatureCardShopPaidRefreshCount = resBody.dailyPaidRefreshCount;

			RefreshProductList(resBody.randomProducts);

			CsGameData.Instance.MyHeroInfo.OwnDia = resBody.ownDia;
			CsGameData.Instance.MyHeroInfo.UnOwnDia = resBody.unOwnDia;

			if (EventCreatureCardShopPaidRefresh != null)
			{
				EventCreatureCardShopPaidRefresh();
			}
		}
		else if (nReturnCode == 101)
		{
			// 금일갱신횟수가 최대입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 다이아가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점고정상품구매
	public void SendCreatureCardShopFixedProductBuy(int nProductId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardShopFixedProductBuyCommandBody cmdBody = new CreatureCardShopFixedProductBuyCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardShopFixedProductBuy, cmdBody);
		}
	}

	void OnEventResCreatureCardShopFixedProductBuy(int nReturnCode, CreatureCardShopFixedProductBuyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listPurchasedCreatureCardShopFixedProduct.Add(m_nProductId);

			CsGameData.Instance.MyHeroInfo.SoulPowder = resBody.soulPowder;
			PDInventorySlot[] inventorySlots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

			if (EventCreatureCardShopFixedProductBuy != null)
			{
				EventCreatureCardShopFixedProductBuy();
			}
		}
		else if (nReturnCode == 101)
		{
			// 이미 구매한 상품입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 영혼가루가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00202"));
		}
		else if (nReturnCode == 103)
		{
			// 인벤토리가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00203"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리처카드상점랜덤상품구매
	public void SendCreatureCardShopRandomProductBuy(int nProductId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			CreatureCardShopRandomProductBuyCommandBody cmdBody = new CreatureCardShopRandomProductBuyCommandBody();
			cmdBody.productId = m_nProductId = nProductId;
			CsRplzSession.Instance.Send(ClientCommandName.CreatureCardShopRandomProductBuy, cmdBody);
		}
	}

	void OnEventResCreatureCardShopRandomProductBuy(int nReturnCode, CreatureCardShopRandomProductBuyResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			CsGameData.Instance.MyHeroInfo.SoulPowder = resBody.soulPowder;
			AddHeroCreatureCard(resBody.changedCreatureCard);
            for (int i = 0; i < m_listCsHeroCreatureCardShopRandomProduct.Count; ++i)
            {
                if (m_listCsHeroCreatureCardShopRandomProduct[i].CreatureCardShopRandomProduct.ProductId == m_nProductId)
                {
                    m_listCsHeroCreatureCardShopRandomProduct[i].Purchased = true;
                }
            }

            if (EventCreatureCardShopRandomProductBuy != null)
			{
				EventCreatureCardShopRandomProductBuy();
			}
		}
		else if (nReturnCode == 101)
		{
			// 존재하지 않는 상품입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00301"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 구매한 상품입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00302"));
		}
		else if (nReturnCode == 103)
		{
			// 영혼가루가 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A88_ERROR_00303"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 크리쳐카드상점갱신
	void OnEventEvtCreatureCardShopRefreshed(SEBCreatureCardShopRefreshedEventBody eventBody)
	{
		RefreshProductList(eventBody.randomProducts);

		if (EventCreatureCardShopRefreshed != null)
		{
			EventCreatureCardShopRefreshed();
		}
	}

	#endregion Protocol.Event

}
