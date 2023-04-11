using System.Collections.Generic;
using ClientCommon;
using System;

public class CsTitleManager
{
	bool m_bWaitResponse = false;

	List<CsHeroTitle> m_listCsHeroTitle;        // 칭호 목록
	int m_nDisplayTitleId;                      // 표시칭호ID. 없으면 0
	int m_nActivationTitleId;                   // 활성칭호ID. 없으면 0

	int m_nTitleId;

	//---------------------------------------------------------------------------------------------------
	public static CsTitleManager Instance
	{
		get { return CsSingleton<CsTitleManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventTitleItemUse;
	public event Delegate EventActivationTitleSet;
	public event Delegate EventDisplayTitleSet;

	public event Delegate<int> EventTitleLifetimeEnded;
	public event Delegate<Guid, int> EventHeroDisplayTitleChanged;

	//---------------------------------------------------------------------------------------------------
	public List<CsHeroTitle> HeroTitleList
	{
		get { return m_listCsHeroTitle; }
	}

	public int DisplayTitleId
	{
		get { return m_nDisplayTitleId; }
		set { m_nDisplayTitleId = value; }
	}

	public int ActivationTitleId
	{
		get { return m_nActivationTitleId; }
		set { m_nActivationTitleId = value; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(PDHeroTitle[] titles, int nDisplayTitleId, int nActivationTitleId)
	{
		UnInit();

		m_listCsHeroTitle = new List<CsHeroTitle>();

		for (int i = 0; i < titles.Length; i++)
		{
			m_listCsHeroTitle.Add(new CsHeroTitle(titles[i]));
		}

		m_nDisplayTitleId = nDisplayTitleId;
		m_nActivationTitleId = nActivationTitleId;

		// Command
		CsRplzSession.Instance.EventResTitleItemUse += OnEventResTitleItemUse;
		CsRplzSession.Instance.EventResActivationTitleSet += OnEventResActivationTitleSet;
		CsRplzSession.Instance.EventResDisplayTitleSet += OnEventResDisplayTitleSet;

		// Event
		CsRplzSession.Instance.EventEvtTitleLifetimeEnded += OnEventEvtTitleLifetimeEnded;
		CsRplzSession.Instance.EventEvtHeroDisplayTitleChanged += OnEventEvtHeroDisplayTitleChanged;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResTitleItemUse -= OnEventResTitleItemUse;
		CsRplzSession.Instance.EventResActivationTitleSet -= OnEventResActivationTitleSet;
		CsRplzSession.Instance.EventResDisplayTitleSet -= OnEventResDisplayTitleSet;

		// Event
		CsRplzSession.Instance.EventEvtTitleLifetimeEnded -= OnEventEvtTitleLifetimeEnded;
		CsRplzSession.Instance.EventEvtHeroDisplayTitleChanged -= OnEventEvtHeroDisplayTitleChanged;

		m_bWaitResponse = false;
		m_listCsHeroTitle = null;
		m_nDisplayTitleId = 0;
		m_nActivationTitleId = 0;
		m_nTitleId = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public CsHeroTitle GetHeroTitle(int nTitleId)
	{
		for (int i = 0; i < m_listCsHeroTitle.Count; i++)
		{
			if (m_listCsHeroTitle[i].Title.TitleId == nTitleId)
				return m_listCsHeroTitle[i];
		}

		return null;
	}

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 칭호아이템사용
	public void SendTitleItemUse(int nSlotIndex)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			TitleItemUseCommandBody cmdBody = new TitleItemUseCommandBody();
			cmdBody.slotIndex = nSlotIndex;
			CsRplzSession.Instance.Send(ClientCommandName.TitleItemUse, cmdBody);
		}
	}

	void OnEventResTitleItemUse(int nReturnCode, TitleItemUseResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_listCsHeroTitle.Add(new CsHeroTitle(resBody.titleId, resBody.remainingTime));

			PDInventorySlot[] inventorySlots = new PDInventorySlot[] { resBody.changedInventorySlot };
			CsGameData.Instance.MyHeroInfo.AddInventorySlots(inventorySlots);

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventTitleItemUse != null)
			{
				EventTitleItemUse();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아이템이 부족합니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A83_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 이미 칭호를 보유중입니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A83_ERROR_00102"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 활성칭호설정
	public void SendActivationTitleSet(int nTitleId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			ActivationTitleSetCommandBody cmdBody = new ActivationTitleSetCommandBody();
			cmdBody.titleId = m_nTitleId = nTitleId;
			CsRplzSession.Instance.Send(ClientCommandName.ActivationTitleSet, cmdBody);
		}
	}

	void OnEventResActivationTitleSet(int nReturnCode, ActivationTitleSetResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nActivationTitleId = m_nTitleId;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventActivationTitleSet != null)
			{
				EventActivationTitleSet();
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 칭호를 보유하고 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A83_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 칭호를 활성화할 수 없습니다.
			//CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString(""));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 표시칭호설정
	public void SendDisplayTitleSet(int nTitleId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			DisplayTitleSetCommandBody cmdBody = new DisplayTitleSetCommandBody();
			cmdBody.titleId = m_nTitleId = nTitleId;
			CsRplzSession.Instance.Send(ClientCommandName.DisplayTitleSet, cmdBody);
		}
	}

	void OnEventResDisplayTitleSet(int nReturnCode, DisplayTitleSetResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nDisplayTitleId = m_nTitleId;

			if (EventDisplayTitleSet != null)
			{
				EventDisplayTitleSet();
			}
		}
		else if (nReturnCode == 101)
		{
			// 해당 칭호를 보유하고 있지 않습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A83_ERROR_00301"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 칭호수명종료
	void OnEventEvtTitleLifetimeEnded(SEBTitleLifetimeEndedEventBody eventBody)
	{
		CsHeroTitle csHeroTitle = m_listCsHeroTitle.Find(a => a.Title.TitleId == eventBody.titleId);

		if (csHeroTitle != null)
		{
			m_listCsHeroTitle.Remove(csHeroTitle);

			if (m_nDisplayTitleId == eventBody.titleId)
			{
				m_nDisplayTitleId = 0;
			}

			if (m_nActivationTitleId == eventBody.titleId)
			{
				m_nActivationTitleId = 0;
			}

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();
		}

		if (EventTitleLifetimeEnded != null)
		{
			EventTitleLifetimeEnded(eventBody.titleId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅표시칭호변경
	void OnEventEvtHeroDisplayTitleChanged(SEBHeroDisplayTitleChangedEventBody eventBody)
	{
		if (EventHeroDisplayTitleChanged != null)
		{
			EventHeroDisplayTitleChanged(eventBody.heroId, eventBody.titleId);
		}
	}

	#endregion Protocol.Event
}
