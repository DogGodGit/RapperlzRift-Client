using System.Collections.Generic;
using ClientCommon;
using System;
using SimpleDebugLog;

public class CsArtifactManager
{
	bool m_bWaitResponse = false;

	int m_nArtifactNo;
	int m_nArtifactLevel;
	int m_nArtifactExp;
	int m_nEquippedArtifactNo;

	CsArtifact m_csArtifact;

	Guid[] m_aGuidMainGears;
	int m_nArtNo;

	//---------------------------------------------------------------------------------------------------
	public static CsArtifactManager Instance
	{
		get { return CsSingleton<CsArtifactManager>.GetInstance(); }
	}

	public int ArtifactNo
	{
		get { return m_nArtifactNo; }
	}

	public int ArtifactLevel
	{
		get { return m_nArtifactLevel; }
	}

	public int ArtifactExp
	{
		get { return m_nArtifactExp; }
	}

	public int EquippedArtifactNo
	{
		get { return m_nEquippedArtifactNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventArtifactLevelUp;
	public event Delegate EventArtifactEquip;
	public event Delegate EventArtifactOpened;
	public event Delegate<Guid, int> EventHeroEquippedArtifactChanged;

	//---------------------------------------------------------------------------------------------------
	public void Init(int artifactNo, int artifactLevel, int artifactExp, int equippedArtifactNo)
	{
		UnInit();

		m_nArtifactNo = artifactNo;
		m_nArtifactLevel = artifactLevel;
		m_nArtifactExp = artifactExp;

		m_nEquippedArtifactNo = equippedArtifactNo;

		// Command
		CsRplzSession.Instance.EventResArtifactLevelUp += OnEventResArtifactLevelUp;
		CsRplzSession.Instance.EventResArtifactEquip += OnEventResArtifactEquip;

		// Event
		CsRplzSession.Instance.EventEvtArtifactOpened += OnEventEvtArtifactOpened;
		CsRplzSession.Instance.EventEvtHeroEquippedArtifactChanged += OnEventEvtHeroEquippedArtifactChanged;
}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		// Command
		CsRplzSession.Instance.EventResArtifactLevelUp -= OnEventResArtifactLevelUp;
		CsRplzSession.Instance.EventResArtifactEquip -= OnEventResArtifactEquip;

		// Event
		CsRplzSession.Instance.EventEvtArtifactOpened -= OnEventEvtArtifactOpened;
		CsRplzSession.Instance.EventEvtHeroEquippedArtifactChanged -= OnEventEvtHeroEquippedArtifactChanged;

		m_bWaitResponse = false;

	}

	//---------------------------------------------------------------------------------------------------
	void AddAttrValue(Dictionary<int, long> dic, int nAttrId, long lValue)
	{
		if (dic.ContainsKey(nAttrId))
		{
			dic[nAttrId] += lValue;
		}
		else
		{
			dic.Add(nAttrId, lValue);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public long GetBattlePower(int nArtifactNo)
	{
		long lBattlePower = 0;

		if (m_nArtifactNo > 0)
		{
			Dictionary<int, long> dic = new Dictionary<int, long>();

			CsArtifact csArtifact = CsGameData.Instance.GetArtifact(nArtifactNo);

			CsArtifactLevel csArtifactLevel = null;

			if (nArtifactNo == CsArtifactManager.Instance.ArtifactNo)
			{
				// 현재 레벨의 속성 리스트
				csArtifactLevel = csArtifact.GetArtifactLevel(CsArtifactManager.Instance.ArtifactLevel);
			}
			else if (nArtifactNo > CsArtifactManager.Instance.ArtifactNo)
			{
				csArtifactLevel = csArtifact.ArtifactLevelList[0];
			}
			else
			{
				// 최대 레벨의 속성 리스트
				csArtifactLevel = csArtifact.ArtifactLevelList[csArtifact.ArtifactLevelList.Count - 1];
			}

			for (int j = 0; j < csArtifactLevel.ArtifactLevelAttrList.Count; j++)
			{
				AddAttrValue(dic, csArtifactLevel.ArtifactLevelAttrList[j].Attr.AttrId, csArtifactLevel.ArtifactLevelAttrList[j].AttrValue.Value);
			}

			for (int i = 0; i < CsGameData.Instance.AttrList.Count; i++)
			{
				if (dic.ContainsKey(CsGameData.Instance.AttrList[i].AttrId))
				{
					lBattlePower += dic[CsGameData.Instance.AttrList[i].AttrId] * CsGameData.Instance.GetAttr(CsGameData.Instance.AttrList[i].AttrId).BattlePowerFactor;
				}
			}

			return lBattlePower;
		}
		else
		{
			return lBattlePower;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public int GetRequireExpForCurrentLevelToMaxLevel()
	{
		int nMaxLevelExp = 0;
		int nCurrentLevelExp = 0;

		for (int i = 0; i < CsGameData.Instance.ArtifactList.Count; i++)
		{
			CsArtifact csArtifact = CsGameData.Instance.GetArtifact(CsGameData.Instance.ArtifactList[i].ArtifactNo);

			// MaxLevel Require Exp
			for (int j = 0; j < csArtifact.ArtifactLevelList.Count; j++)
			{
				nMaxLevelExp += csArtifact.ArtifactLevelList[j].NextLevelUpRequiredExp;
			}

			if (csArtifact.ArtifactNo <= m_nArtifactNo)
			{
				// Curent Level Require Exp
				for (int j = 0; j < csArtifact.ArtifactLevelList.Count; j++)
				{
					if (csArtifact.ArtifactNo == CsArtifactManager.Instance.ArtifactNo && csArtifact.ArtifactLevelList[j].Level == m_nArtifactLevel)
					{
						break;
					}

					nCurrentLevelExp += csArtifact.ArtifactLevelList[j].NextLevelUpRequiredExp;
				}
			}
		}

		// 최대 레벨에서 현재 레벨까지 필요한 경험치랑을 뺀 후, 현재 내가 가지고 있는 경험치를 뺀다.
		return nMaxLevelExp - (nCurrentLevelExp + CsArtifactManager.Instance.ArtifactExp);
	}


	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 아티팩트레벨업
	public void SendArtifactLevelUp(Guid[] mainGears)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ArtifactLevelUpCommandBody cmdBody = new ArtifactLevelUpCommandBody();
			cmdBody.mainGears = m_aGuidMainGears = mainGears;
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactLevelUp, cmdBody);
		}
	}

	void OnEventResArtifactLevelUp(int nReturnCode, ArtifactLevelUpResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nArtifactNo = resBody.artifactNo;
			m_nArtifactLevel = resBody.artifactLevel;
			m_nArtifactExp = resBody.artifactExp;

			CsGameData.Instance.MyHeroInfo.MaxHp = resBody.maxHP;
			CsGameData.Instance.MyHeroInfo.Hp = resBody.hp;

			for (int i = 0; i < m_aGuidMainGears.Length; i++)
			{
				CsGameData.Instance.MyHeroInfo.RemoveInventorySlot(m_aGuidMainGears[i]);
			}

			CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

			if (EventArtifactLevelUp != null)
			{
				EventArtifactLevelUp();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아티팩트가 개방되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00101"));
		}
		else if (nReturnCode == 102)
		{
			// 아티팩트가 최대레벨입니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00102"));
		}
		else if (nReturnCode == 103)
		{
			// 메인장비가 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00103"));
		}
		else if (nReturnCode == 104)
		{
			// 메인장비가 인벤토리에 존재하지 않습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00104"));
		}
		else if (nReturnCode == 105)
		{
			// 메인장비가 중복됩니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00105"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 아티팩트장착
	public void SendArtifactEquip(int nArtifactNo)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;

			ArtifactEquipCommandBody cmdBody = new ArtifactEquipCommandBody();
			cmdBody.artifactNo = m_nArtNo = nArtifactNo;
			CsRplzSession.Instance.Send(ClientCommandName.ArtifactEquip, cmdBody);
		}
	}

	void OnEventResArtifactEquip(int nReturnCode, ArtifactEquipResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			m_nEquippedArtifactNo = m_nArtNo;

			if (EventArtifactEquip != null)
			{
				EventArtifactEquip();
			}
		}
		else if (nReturnCode == 101)
		{
			// 아티팩트가 개방되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00201"));
		}
		else if (nReturnCode == 102)
		{
			// 대상아티팩트가 개방되지 않았습니다.
			CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A157_ERROR_00202"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 아티팩트개방
	void OnEventEvtArtifactOpened(SEBArtifactOpenedEventBody eventBody)
	{
		m_nArtifactNo = eventBody.artifactNo;
		m_nArtifactLevel = eventBody.artifactLevel;
		m_nArtifactExp = eventBody.artifactExp;

		m_nEquippedArtifactNo = eventBody.equippedArtifactNo;

		CsGameData.Instance.MyHeroInfo.MaxHp = eventBody.maxHP;
		CsGameData.Instance.MyHeroInfo.Hp = eventBody.hp;

		CsGameData.Instance.MyHeroInfo.UpdateBattlePower();

		if (EventArtifactOpened != null)
		{
			EventArtifactOpened();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 영웅장작아티팩트변경
	void OnEventEvtHeroEquippedArtifactChanged(SEBHeroEquippedArtifactChangedEventBody eventBody)
	{
		if (EventHeroEquippedArtifactChanged != null)
		{
			EventHeroEquippedArtifactChanged(eventBody.heroId, eventBody.equippedArtifactNo);
		}
	}
	#endregion Protocol.Event
}
