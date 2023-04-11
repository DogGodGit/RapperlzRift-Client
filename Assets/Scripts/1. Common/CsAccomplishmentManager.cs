using System.Collections.Generic;
using ClientCommon;
using System;

public enum EnAccomplishment
{
	MaxLevel = 1,
	MaxBattlePower = 2,
	MaxGold = 3,
	MaxMainQuest = 4,
	AccMonsterKill = 5,
	AccSoulCoveterPlay = 6,
	AccEpicBaitItemUse = 7,
	AccLegendBaitItemUse = 8,
	AccNationWarWin = 9,
	AccNationWarKill = 10,
	AccNationWarCommanderKill = 11,
	AccNationWarImmediateRevival = 12,
	MaxAcquisitionMainGearGrade = 13,
	MaxEquippedMainGearEnchantLevel = 14,
	ActivateCreatureCardCollection = 15,
}


public class CsAccomplishmentManager
{
	bool m_bWaitResponse = false;

	List<int> m_listRewardedAccomplishments;        // 보상받은업적목록. 배열항목 : 업적ID
	int m_nAccMonsterKillCount;                     // 누적몬스터킬카운트
	int m_nAccSoulCoveterPlayCount;                 // 누적영혼을탐하는자플레이횟수
	int m_nAccEpicBaitItemUseCount;                 // 누적에픽미끼아이템사용횟수
	int m_nAccLegendBaitItemUseCount;               // 누적전설미끼아이템사용횟수
	int m_nAccNationWarWinCount;                    // 누적국가전승리횟수
	int m_nAccNationWarKillCount;                   // 누적국가전킬수
	int m_nAccNationWarCommanderKillCount;          // 누적국가전총사령관킬수
	int m_nAccNationWarImmediateRevivalCount;       // 누적국가전즉시부활횟수
	long m_lMaxGold;                                // 최대골드
	long m_lMaxBattlePower;                         // 최대전투력
	int m_nMaxAcquisitionMainGearGrade;             // 최대획득메인장비등급
	int m_nMaxEquippedMainGearEnchantLevel;         // 최대장착메인장비강화레벨

	int m_nAccomplishmentLevel;
	int m_nAccomplishmentPoint;

	int m_nAccomplishmentId;
    List<int> m_listAccomplishmentComplete;
	int m_nTargetLevel;
	//---------------------------------------------------------------------------------------------------
	public static CsAccomplishmentManager Instance
	{
		get { return CsSingleton<CsAccomplishmentManager>.GetInstance(); }
	}

	//---------------------------------------------------------------------------------------------------
	public event Delegate EventAccomplishmentRewardReceive;
	public event Delegate EventAccomplishmentRewardReceiveAll;
	public event Delegate EventAccMonsterKillCountUpdated;
	public event Delegate EventAccNationWarCommanderKillCountUpdated;
	public event Delegate EventAccomplishmentLevelUp;
	public event Delegate EventAccomplishmentPointItemUse;

	//---------------------------------------------------------------------------------------------------

	public List<int> RewardedAccomplishmentList
	{
		get { return m_listRewardedAccomplishments; }
	}

	public int AccMonsterKillCount
	{
		get { return m_nAccMonsterKillCount; }
		set { m_nAccMonsterKillCount = value; }
	}

	public int AccSoulCoveterPlayCount
	{
		get { return m_nAccSoulCoveterPlayCount; }
		set { m_nAccSoulCoveterPlayCount = value; }
	}

	public int AccEpicBaitItemUseCount
	{
		get { return m_nAccEpicBaitItemUseCount; }
		set { m_nAccEpicBaitItemUseCount = value; }
	}

	public int AccLegendBaitItemUseCount
	{
		get { return m_nAccLegendBaitItemUseCount; }
		set { m_nAccLegendBaitItemUseCount = value; }
	}

	public int AccNationWarWinCount
	{
		get { return m_nAccNationWarWinCount; }
		set { m_nAccNationWarWinCount = value; }
	}

	public int AccNationWarKillCount
	{
		get { return m_nAccNationWarKillCount; }
		set { m_nAccNationWarKillCount = value; }
	}

	public int AccNationWarCommanderKillCount
	{
		get { return m_nAccNationWarCommanderKillCount; }
		set { m_nAccNationWarCommanderKillCount = value; }
	}

	public int AccNationWarImmediateRevivalCount
	{
		get { return m_nAccNationWarImmediateRevivalCount; }
		set { m_nAccNationWarImmediateRevivalCount = value; }
	}

	public long MaxGold
	{
		get { return m_lMaxGold; }
		set { m_lMaxGold = value; }
	}

	public long MaxBattlePower
	{
		get { return m_lMaxBattlePower; }
		set { m_lMaxBattlePower = value; }
	}

	public int MaxAcquisitionMainGearGrade
	{
		get { return m_nMaxAcquisitionMainGearGrade; }
		set { m_nMaxAcquisitionMainGearGrade = value; }
	}

	public int MaxEquippedMainGearEnchantLevel
	{
		get { return m_nMaxEquippedMainGearEnchantLevel; }
		set { m_nMaxEquippedMainGearEnchantLevel = value; }
	}

	public int AccomplishmentLevel
	{
		get { return m_nAccomplishmentLevel; }
	}

	public int AccomplishmentPoint
	{
		get { return m_nAccomplishmentPoint; }
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(int[] rewardedAccomplishments, int nAccMonsterKillCount,			int nAccSoulCoveterPlayCount,			int nAccEpicBaitItemUseCount,				int nAccLegendBaitItemUseCount,
					 int nAccNationWarWinCount,		int nAccNationWarKillCount,			int nAccNationWarCommanderKillCount,	int nAccNationWarImmediateRevivalCount,		long lMaxGold,
					 long lMaxBattlePower,			int nMaxAcquisitionMainGearGrade,	int nMaxEquippedMainGearEnchantLevel,	int nAccomplishmentLevel,					int nAccomplishmentPoint)
	{
		UnInit();
		m_listRewardedAccomplishments = new List<int>(rewardedAccomplishments);
		m_nAccMonsterKillCount = nAccMonsterKillCount;
		m_nAccSoulCoveterPlayCount = nAccSoulCoveterPlayCount;
		m_nAccEpicBaitItemUseCount = nAccEpicBaitItemUseCount;
		m_nAccLegendBaitItemUseCount = nAccLegendBaitItemUseCount;
		m_nAccNationWarWinCount = nAccNationWarWinCount;
		m_nAccNationWarKillCount = nAccNationWarKillCount;
		m_nAccNationWarCommanderKillCount = nAccNationWarCommanderKillCount;
		m_nAccNationWarImmediateRevivalCount = nAccNationWarImmediateRevivalCount;
		m_lMaxGold = lMaxGold;
		m_lMaxBattlePower = lMaxBattlePower;
		m_nMaxAcquisitionMainGearGrade = nMaxAcquisitionMainGearGrade;
		m_nMaxEquippedMainGearEnchantLevel = nMaxEquippedMainGearEnchantLevel;

		m_nAccomplishmentLevel = nAccomplishmentLevel;
		m_nAccomplishmentPoint = nAccomplishmentPoint;

		// Command 
		CsRplzSession.Instance.EventResAccomplishmentRewardReceive += OnEventResAccomplishmentRewardReceive;
		CsRplzSession.Instance.EventResAccomplishmentRewardReceiveAll += OnEventResAccomplishmentRewardReceiveAll;


		// Event
		CsRplzSession.Instance.EventEvtAccMonsterKillCountUpdated += OnEventEvtAccMonsterKillCountUpdated;
		CsRplzSession.Instance.EventEvtAccNationWarCommanderKillCountUpdated += OnEventEvtAccNationWarCommanderKillCountUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	void UnInit()
	{
		m_bWaitResponse = false;
		m_listRewardedAccomplishments = null;
		m_nAccMonsterKillCount = 0;
		m_nAccSoulCoveterPlayCount = 0;
		m_nAccEpicBaitItemUseCount = 0;
		m_nAccLegendBaitItemUseCount = 0;
		m_nAccNationWarWinCount = 0;
		m_nAccNationWarKillCount = 0;
		m_nAccNationWarCommanderKillCount = 0;
		m_nAccNationWarImmediateRevivalCount = 0;
		m_lMaxGold = 0;
		m_lMaxBattlePower = 0;
		m_nMaxAcquisitionMainGearGrade = 0;
		m_nMaxEquippedMainGearEnchantLevel = 0;
		m_nAccomplishmentId = 0;
        m_listAccomplishmentComplete = new List<int>();

		m_nAccomplishmentLevel = 0;
		m_nAccomplishmentPoint = 0;

		// Command 
		CsRplzSession.Instance.EventResAccomplishmentRewardReceive -= OnEventResAccomplishmentRewardReceive;
		CsRplzSession.Instance.EventResAccomplishmentRewardReceiveAll -= OnEventResAccomplishmentRewardReceiveAll;


		// Event
		CsRplzSession.Instance.EventEvtAccMonsterKillCountUpdated -= OnEventEvtAccMonsterKillCountUpdated;
		CsRplzSession.Instance.EventEvtAccNationWarCommanderKillCountUpdated -= OnEventEvtAccNationWarCommanderKillCountUpdated;
	}

	//---------------------------------------------------------------------------------------------------
	/*
	1 : 레벨 달성
	2 : 전투력 달성
	3 : 보유골드 달성
	4 : 메인퀘스트 완료
	5 : 몬스터 처치 달성
	6 : 영혼을탐하는자입장횟수
	7 : 에픽미끼사용 x회
	8 : 전설미끼사용 x회
	9 : 국가전승리 횟수
	10 : 국가전적처치수
	11 : 국가전사령관처치
	12 : 국가전즉시부활(무료유료)
	13 : 획득메인장비등급
	14 : 강화된메인장비장착
	15 : 카드조합모두활성화
	*/

    //---------------------------------------------------------------------------------------------------
	public bool CheckAccomplishmentNotice()
	{
		for (int i = 0; i < CsGameData.Instance.AccomplishmentList.Count; i++)
		{
			long lAccValue = 0;
            long lObjectiveValue = 0;

			switch ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type)
			{
				case EnAccomplishment.MaxLevel:
					lAccValue = CsGameData.Instance.MyHeroInfo.Level;
					break;
				case EnAccomplishment.MaxBattlePower:
					lAccValue = m_lMaxBattlePower;
					break;
				case EnAccomplishment.MaxGold:
					lAccValue = m_lMaxGold;
					break;
				case EnAccomplishment.MaxMainQuest:
                    if (CsMainQuestManager.Instance.MainQuest != null)
                    {
                        lAccValue = CsMainQuestManager.Instance.MainQuest.MainQuestNo - 1;
                    }
                    else
                    {
                        lAccValue = CsGameData.Instance.MainQuestList.Count;
                    }
					break;
				case EnAccomplishment.AccMonsterKill:
					lAccValue = m_nAccMonsterKillCount;
					break;
				case EnAccomplishment.AccSoulCoveterPlay:
					lAccValue = m_nAccSoulCoveterPlayCount;
					break;
				case EnAccomplishment.AccEpicBaitItemUse:
					lAccValue = m_nAccEpicBaitItemUseCount;
					break;
				case EnAccomplishment.AccLegendBaitItemUse:
					lAccValue = m_nAccLegendBaitItemUseCount;
					break;
				case EnAccomplishment.AccNationWarWin:
					lAccValue = m_nAccNationWarWinCount;
					break;
				case EnAccomplishment.AccNationWarKill:
					lAccValue = m_nAccNationWarKillCount;
					break;
				case EnAccomplishment.AccNationWarCommanderKill:
					lAccValue = m_nAccNationWarCommanderKillCount;
					break;
				case EnAccomplishment.AccNationWarImmediateRevival:
					lAccValue = m_nAccNationWarImmediateRevivalCount;
					break;
				case EnAccomplishment.MaxAcquisitionMainGearGrade:
					lAccValue = m_nMaxAcquisitionMainGearGrade;
					break;
				case EnAccomplishment.MaxEquippedMainGearEnchantLevel:
					lAccValue = m_nMaxEquippedMainGearEnchantLevel;
					break;
				case EnAccomplishment.ActivateCreatureCardCollection:
                    CsCreatureCardCategory csCreatureCardCategory = CsGameData.Instance.GetCreatureCardCategory((int)CsGameData.Instance.AccomplishmentList[i].ObjectiveValue);

                    if (csCreatureCardCategory != null)
                    {
                        List<CsCreatureCardCollection> listCsCreatureCardCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == csCreatureCardCategory.CategoryId);
                        lObjectiveValue = listCsCreatureCardCollection.Count;

                        for (int j = 0; j < listCsCreatureCardCollection.Count; j++)
                        {
                            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCsCreatureCardCollection[j].CollectionId))
                            {
                                lAccValue++;
                            }
                        }
                    }
                    else
                    {
                        lAccValue = -1;
                    }
					break;
			}

            if ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type != EnAccomplishment.ActivateCreatureCardCollection)
            {
                lObjectiveValue = CsGameData.Instance.AccomplishmentList[i].ObjectiveValue;
            }

            if (lObjectiveValue <= lAccValue)
            {
                int nAccomplishmentId = CsGameData.Instance.AccomplishmentList[i].AccomplishmentId;

                if (m_listRewardedAccomplishments.FindIndex(a => a == nAccomplishmentId) == -1)
                {
                    if (CsGameData.Instance.AccomplishmentList[i].RewardType == 2)
                    {
                        return true;
                    }
                }
            }
		}

		return false;
	}

    //---------------------------------------------------------------------------------------------------
    public void CheckAccomplishmentComplete()
    {
        for (int i = 0; i < CsGameData.Instance.AccomplishmentList.Count; i++)
        {
            long lAccValue = 0;
            long lObjectiveValue = 0;

            switch ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type)
            {
                case EnAccomplishment.MaxLevel:
                    lAccValue = CsGameData.Instance.MyHeroInfo.Level;
                    break;
                case EnAccomplishment.MaxBattlePower:
                    lAccValue = m_lMaxBattlePower;
                    break;
                case EnAccomplishment.MaxGold:
                    lAccValue = m_lMaxGold;
                    break;
                case EnAccomplishment.MaxMainQuest:
                    if (CsMainQuestManager.Instance.MainQuest != null)
                    {
                        lAccValue = CsMainQuestManager.Instance.MainQuest.MainQuestNo - 1;
                    }
                    else
                    {
                        lAccValue = CsGameData.Instance.MainQuestList.Count;
                    }
                    break;
                case EnAccomplishment.AccMonsterKill:
                    lAccValue = m_nAccMonsterKillCount;
                    break;
                case EnAccomplishment.AccSoulCoveterPlay:
                    lAccValue = m_nAccSoulCoveterPlayCount;
                    break;
                case EnAccomplishment.AccEpicBaitItemUse:
                    lAccValue = m_nAccEpicBaitItemUseCount;
                    break;
                case EnAccomplishment.AccLegendBaitItemUse:
                    lAccValue = m_nAccLegendBaitItemUseCount;
                    break;
                case EnAccomplishment.AccNationWarWin:
                    lAccValue = m_nAccNationWarWinCount;
                    break;
                case EnAccomplishment.AccNationWarKill:
                    lAccValue = m_nAccNationWarKillCount;
                    break;
                case EnAccomplishment.AccNationWarCommanderKill:
                    lAccValue = m_nAccNationWarCommanderKillCount;
                    break;
                case EnAccomplishment.AccNationWarImmediateRevival:
                    lAccValue = m_nAccNationWarImmediateRevivalCount;
                    break;
                case EnAccomplishment.MaxAcquisitionMainGearGrade:
                    lAccValue = m_nMaxAcquisitionMainGearGrade;
                    break;
                case EnAccomplishment.MaxEquippedMainGearEnchantLevel:
                    lAccValue = m_nMaxEquippedMainGearEnchantLevel;
                    break;
                case EnAccomplishment.ActivateCreatureCardCollection:
                    CsCreatureCardCategory csCreatureCardCategory = CsGameData.Instance.GetCreatureCardCategory((int)CsGameData.Instance.AccomplishmentList[i].ObjectiveValue);

                    if (csCreatureCardCategory != null)
                    {
                        List<CsCreatureCardCollection> listCsCreatureCardCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == csCreatureCardCategory.CategoryId);
                        lObjectiveValue = listCsCreatureCardCollection.Count;

                        for (int j = 0; j < listCsCreatureCardCollection.Count; j++)
                        {
                            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCsCreatureCardCollection[j].CollectionId))
                            {
                                lAccValue++;
                            }
                        }
                    }
                    else
                    {
                        lAccValue = -1;
                    }
                    break;
            }

            if ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type != EnAccomplishment.ActivateCreatureCardCollection)
            {
                lObjectiveValue = CsGameData.Instance.AccomplishmentList[i].ObjectiveValue;
            }

            if (lObjectiveValue <= lAccValue)
            {
                int nAccomplishmentId = CsGameData.Instance.AccomplishmentList[i].AccomplishmentId;
                if (m_listRewardedAccomplishments.FindIndex(a => a == nAccomplishmentId) == -1)
                {
                    if (m_listAccomplishmentComplete.FindIndex(a => a == nAccomplishmentId) == -1)
                    {
                        m_listAccomplishmentComplete.Add(nAccomplishmentId);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateAccomplishmentComplete()
    {
        for (int i = 0; i < CsGameData.Instance.AccomplishmentList.Count; i++)
        {
            long lAccValue = 0;
            long lObjectiveValue = 0;

            switch ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type)
            {
                case EnAccomplishment.MaxLevel:
                    lAccValue = CsGameData.Instance.MyHeroInfo.Level;
                    break;
                case EnAccomplishment.MaxBattlePower:
                    lAccValue = m_lMaxBattlePower;
                    break;
                case EnAccomplishment.MaxGold:
                    lAccValue = m_lMaxGold;
                    break;
                case EnAccomplishment.MaxMainQuest:
                    if (CsMainQuestManager.Instance.MainQuest != null)
                    {
                        lAccValue = CsMainQuestManager.Instance.MainQuest.MainQuestNo - 1;
                    }
                    else
                    {
                        lAccValue = CsGameData.Instance.MainQuestList.Count;
                    }
                    break;
                case EnAccomplishment.AccMonsterKill:
                    lAccValue = m_nAccMonsterKillCount;
                    break;
                case EnAccomplishment.AccSoulCoveterPlay:
                    lAccValue = m_nAccSoulCoveterPlayCount;
                    break;
                case EnAccomplishment.AccEpicBaitItemUse:
                    lAccValue = m_nAccEpicBaitItemUseCount;
                    break;
                case EnAccomplishment.AccLegendBaitItemUse:
                    lAccValue = m_nAccLegendBaitItemUseCount;
                    break;
                case EnAccomplishment.AccNationWarWin:
                    lAccValue = m_nAccNationWarWinCount;
                    break;
                case EnAccomplishment.AccNationWarKill:
                    lAccValue = m_nAccNationWarKillCount;
                    break;
                case EnAccomplishment.AccNationWarCommanderKill:
                    lAccValue = m_nAccNationWarCommanderKillCount;
                    break;
                case EnAccomplishment.AccNationWarImmediateRevival:
                    lAccValue = m_nAccNationWarImmediateRevivalCount;
                    break;
                case EnAccomplishment.MaxAcquisitionMainGearGrade:
                    lAccValue = m_nMaxAcquisitionMainGearGrade;
                    break;
                case EnAccomplishment.MaxEquippedMainGearEnchantLevel:
                    lAccValue = m_nMaxEquippedMainGearEnchantLevel;
                    break;
                case EnAccomplishment.ActivateCreatureCardCollection:
                    CsCreatureCardCategory csCreatureCardCategory = CsGameData.Instance.GetCreatureCardCategory((int)CsGameData.Instance.AccomplishmentList[i].ObjectiveValue);

                    if (csCreatureCardCategory != null)
                    {
                        List<CsCreatureCardCollection> listCsCreatureCardCollection = CsGameData.Instance.CreatureCardCollectionList.FindAll(a => a.CreatureCardCollectionCategory.CategoryId == csCreatureCardCategory.CategoryId);
                        lObjectiveValue = listCsCreatureCardCollection.Count;

                        for (int j = 0; j < listCsCreatureCardCollection.Count; j++)
                        {
                            if (CsCreatureCardManager.Instance.GetActivatedCreatureCardCollection(listCsCreatureCardCollection[j].CollectionId))
                            {
                                lAccValue++;
                            }
                        }
                    }
                    else
                    {
                        lAccValue = -1;
                    }
                    break;
            }

            if ((EnAccomplishment)CsGameData.Instance.AccomplishmentList[i].Type != EnAccomplishment.ActivateCreatureCardCollection)
            {
                lObjectiveValue = CsGameData.Instance.AccomplishmentList[i].ObjectiveValue;
            }

            if (lObjectiveValue <= lAccValue)
            {
                int nAccomplishmentId = CsGameData.Instance.AccomplishmentList[i].AccomplishmentId;

                if (m_listRewardedAccomplishments.FindIndex(a => a == nAccomplishmentId) == -1)
                {
                    if (m_listAccomplishmentComplete.FindIndex(a => a == nAccomplishmentId) == -1)
                    {
                        UnityEngine.Debug.Log(">>>CsGameEventUIToUI.Instance.OnEventToastMessage<<< : " + CsGameData.Instance.AccomplishmentList[i].AccomplishmentId);
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A82_TXT_02004"), CsGameData.Instance.GetAccomplishment(nAccomplishmentId).Name));
                        m_listAccomplishmentComplete.Add(nAccomplishmentId);
                    }

                    if (CsGameData.Instance.AccomplishmentList[i].RewardType == 1)
                    {
                        SendAccomplishmentRewardReceive(nAccomplishmentId);
                    }
                }
            }
        }
    }

	#region Protocol.Command

	//---------------------------------------------------------------------------------------------------
	// 업적보상받기
	public void SendAccomplishmentRewardReceive(int nAccomplishmentId)
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AccomplishmentRewardReceiveCommandBody cmdBody = new AccomplishmentRewardReceiveCommandBody();
			cmdBody.accomplishmentId = m_nAccomplishmentId = nAccomplishmentId;
			CsRplzSession.Instance.Send(ClientCommandName.AccomplishmentRewardReceive, cmdBody);
		}
	}

	void OnEventResAccomplishmentRewardReceive(int nReturnCode, AccomplishmentRewardReceiveResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
		
		}
		else if (nReturnCode == 101)
		{
			// 업적의 목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A82_ERROR_00101"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 업적보상전체받기
	public void SendAccomplishmentRewardReceiveAll()
	{
		if (!m_bWaitResponse)
		{
			m_bWaitResponse = true;
			AccomplishmentRewardReceiveAllCommandBody cmdBody = new AccomplishmentRewardReceiveAllCommandBody();
			CsRplzSession.Instance.Send(ClientCommandName.AccomplishmentRewardReceiveAll, cmdBody);
		}
	}

	void OnEventResAccomplishmentRewardReceiveAll(int nReturnCode, AccomplishmentRewardReceiveAllResponseBody resBody)
	{
		m_bWaitResponse = false;

		if (nReturnCode == 0)
		{
			
		}
		else if (nReturnCode == 101)
		{
			// 업적의 목표가 완료되지 않았습니다.
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A82_ERROR_00201"));
		}
		else
		{
			CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 업적레벨업
	public void SendAccomplishmentLevelUp(int nTargetLevel)
	{

	}

	void OnEventResAccomplishmentLevelUp(int nReturnCode, AccomplishmentLevelUpResponseBody resBody)
	{
	
	}

	//---------------------------------------------------------------------------------------------------
	// 업적점수아이템사용
	public void SendAccomplishmentPointItemUse(int nSlotIndex, int nUseCount)
	{

	}

	void OnEventResAccomplishmentPointItemUse(int nReturnCode, AccomplishmentPointItemUseResponseBody resBody)
	{


	}

	#endregion Protocol.Command

	#region Protocol.Event

	//---------------------------------------------------------------------------------------------------
	// 누적몬스터킬횟수갱신
	void OnEventEvtAccMonsterKillCountUpdated(SEBAccMonsterKillCountUpdatedEventBody eventBody)
	{
		m_nAccMonsterKillCount = eventBody.count;

		if (EventAccMonsterKillCountUpdated != null)
		{
			EventAccMonsterKillCountUpdated();
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 누적국가전총사령관킬횟수갱신
	void OnEventEvtAccNationWarCommanderKillCountUpdated(SEBAccNationWarCommanderKillCountUpdatedEventBody eventBody)
	{
		m_nAccNationWarCommanderKillCount = eventBody.count;

		if (EventAccNationWarCommanderKillCountUpdated != null)
		{
			EventAccNationWarCommanderKillCountUpdated();
		}
	}

	#endregion Protocol.Event
}
