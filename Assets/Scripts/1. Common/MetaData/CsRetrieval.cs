using System.Collections.Generic;
using WebCommon;
using System;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-19)
//---------------------------------------------------------------------------------------------------

enum EnRetrieval
{
	ExpDungeon = 1,			// 경험치 던전
	FieldOfHonor = 2,		// 결투장
	FearAltar = 3,			// 공포의 제단
	BountyHunterQuest = 4,	// 현상금 사냥
	DimensionRaidQuest = 5,	// 차원 습격
	SupplySupportQuest = 6,	// 보급 지원
	Fishing = 7,			// 낚시
	MysteryBoxQuest = 8,	// 의문의 상자
	SecretLetterQuest = 9,	// 밀서 유출
	ThreatOfFarmQuest = 10,	// 농장의 위협
	//AnkouTomb = 98,			// 안쿠의무덤(황금의무덤)
	//TradeShip = 99,			// 산체르코호(망자의유물)
}

public class CsRetrieval
{
	int m_nRetrievalId;
	string m_strName;
	int m_nRewardDisplayType;      // 1:경험치,2:아이템
	string m_strGoldRetrievalText;
	long m_lGoldRetrievalRequiredGold;
	string m_strDiaRetrievalText;
	int m_nDiaRetrievalRequiredDia;
	int m_nMaxCount;

	List<CsRetrievalReward> m_listCsRetrievalReward;

	//---------------------------------------------------------------------------------------------------
	public int RetrievalId
	{
		get { return m_nRetrievalId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int RewardDisplayType
	{
		get { return m_nRewardDisplayType; }
	}

	public string GoldRetrievalText
	{
		get { return m_strGoldRetrievalText; }
	}

	public long GoldRetrievalRequiredGold
	{
		get { return m_lGoldRetrievalRequiredGold; }
	}

	public string DiaRetrievalText
	{
		get { return m_strDiaRetrievalText; }
	}

	public int DiaRetrievalRequiredDia
	{
		get { return m_nDiaRetrievalRequiredDia; }
	}

	public List<CsRetrievalReward> RetrievalRewardList
	{
		get { return m_listCsRetrievalReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsRetrieval(WPDRetrieval retrieval)
	{
		m_nRetrievalId = retrieval.retrievalId;
		m_strName = CsConfiguration.Instance.GetString(retrieval.nameKey);
		m_nRewardDisplayType = retrieval.rewardDisplayType; 
		m_strGoldRetrievalText = CsConfiguration.Instance.GetString(retrieval.goldRetrievalTextKey);
		m_lGoldRetrievalRequiredGold = retrieval.goldRetrievalRequiredGold;
		m_strDiaRetrievalText = CsConfiguration.Instance.GetString(retrieval.diaRetrievalTextKey);
		m_nDiaRetrievalRequiredDia = retrieval.diaRetrievalRequiredDia;

		m_listCsRetrievalReward = new List<CsRetrievalReward>();
	}

	//---------------------------------------------------------------------------------------------------
	public CsRetrievalReward GetRetrievalReward(int nLevel)
	{
		for (int i = 0; i < m_listCsRetrievalReward.Count; i++)
		{
			if (m_listCsRetrievalReward[i].Level == nLevel)
				return m_listCsRetrievalReward[i];
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	int GetMaxCount()
	{
		CsVipLevel csVipLevel = CsGameData.Instance.MyHeroInfo.VipLevel;

		int nRequiredConditionType = 0;	// 던전 개방조건(1 : 레벨, 2 : 메인퀘스트 완료, 3 : 메인퀘스트 수락)
		int nRequiredConditionValue = 0;
		int nHeroLevel = CsGameData.Instance.MyHeroInfo.Level;
		int nMainQuestNo = CsMainQuestManager.Instance.MainQuest.MainQuestNo;
		int nRemainingCount = 0;

		switch ((EnRetrieval)m_nRetrievalId)
		{
			case EnRetrieval.ExpDungeon:

				nRequiredConditionType = CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredConditionType;

				if (nRequiredConditionType == 1)
				{
					nRequiredConditionValue = CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredHeroLevel;
				}
				else if (nRequiredConditionType == 2 || nRequiredConditionType == 3)
				{
					nRequiredConditionValue = CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredMainQuestNo;
				}
				
				nRemainingCount = csVipLevel.ExpDungeonEnterCount;

				break;

			case EnRetrieval.FieldOfHonor:

				nRequiredConditionType = CsGameData.Instance.FieldOfHonor.RequiredConditionType;

				if (nRequiredConditionType == 1)
				{
					nRequiredConditionValue = CsGameData.Instance.FieldOfHonor.RequiredHeroLevel;
				}
				else if (nRequiredConditionType == 2 || nRequiredConditionType == 3)
				{
					nRequiredConditionValue = CsGameData.Instance.FieldOfHonor.RequiredMainQuestNo;
				}

				nRemainingCount = csVipLevel.FieldOfHonorEnterCount;

				break;

			case EnRetrieval.FearAltar:

				nRequiredConditionType = CsGameData.Instance.FearAltar.RequiredConditionType;

				if (nRequiredConditionType == 1)
				{
					nRequiredConditionValue = CsGameData.Instance.FearAltar.RequiredHeroLevel;
				}
				else if (nRequiredConditionType == 2 || nRequiredConditionType == 3)
				{
					nRequiredConditionValue = CsGameData.Instance.FearAltar.RequiredMainQuestNo;
				}

				nRemainingCount = csVipLevel.FearAltarEnterCount;

				break;

			case EnRetrieval.BountyHunterQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.BountyHunterQuestRequiredHeroLevel)
					return 0;

				return CsGameConfig.Instance.BountyHunterQuestMaxCount;

			case EnRetrieval.DimensionRaidQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.DimensionRaidQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.DimensionRaidQuest.LimitCount;

			case EnRetrieval.SupplySupportQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SupplySupportQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.SupplySupportQuest.LimitCount;

			case EnRetrieval.Fishing:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.FishingQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.FishingQuest.LimitCount;

			case EnRetrieval.MysteryBoxQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.MysteryBoxQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.MysteryBoxQuest.LimitCount;

			case EnRetrieval.SecretLetterQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.SecretLetterQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.SecretLetterQuest.LimitCount;

			case EnRetrieval.ThreatOfFarmQuest:

				if (CsGameData.Instance.MyHeroInfo.Level < CsGameData.Instance.ThreatOfFarmQuest.RequiredHeroLevel)
					return 0;

				return CsGameData.Instance.ThreatOfFarmQuest.LimitCount;

			//case EnRetrieval.AnkouTomb:
				
			//    nRequiredConditionType = CsGameData.Instance.AnkouTomb.RequiredConditionType;

			//    if (nRequiredConditionType == 1)
			//    {
			//        nRequiredConditionValue = CsGameData.Instance.AnkouTomb.RequiredHeroLevel;
			//    }
			//    else if (nRequiredConditionType == 2 || nRequiredConditionType == 3)
			//    {
			//        nRequiredConditionValue = CsGameData.Instance.AnkouTomb.RequiredMainQuestNo;
			//    }

			//    nRemainingCount = CsGameData.Instance.AnkouTomb.EnterCount;

			//    break;
				
			//case EnRetrieval.TradeShip:

			//    nRequiredConditionType = CsGameData.Instance.TradeShip.RequiredConditionType;

			//    if (nRequiredConditionType == 1)
			//    {
			//        nRequiredConditionValue = CsGameData.Instance.TradeShip.RequiredHeroLevel;
			//    }
			//    else if (nRequiredConditionType == 2 || nRequiredConditionType == 3)
			//    {
			//        nRequiredConditionValue = CsGameData.Instance.TradeShip.RequiredMainQuestNo;
			//    }

			//    nRemainingCount = csVipLevel.TradeShipEnterCount;

			//    break;

			default:
				return 0;
		}

		switch (nRequiredConditionType)
		{
			case 1:

				if (nHeroLevel < nRequiredConditionValue)
					return 0;

				break;
				
			case 2:

				if (nMainQuestNo <= nRequiredConditionValue)
					return 0;

				break;

			case 3:

				if (nMainQuestNo < nRequiredConditionValue)
					return 0;

				break;

			default:
				return 0;
		}

		return nRemainingCount;
	}

	//---------------------------------------------------------------------------------------------------
	public int GetRemainingCount()
	{
		if (CsGameData.Instance.MyHeroInfo.RegDate >= CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date)
		{
			return 0;
		}

		int nMaxCount = GetMaxCount();

		if (nMaxCount <= 0)
		{
			return 0;
		}

		CsHeroRetrieval csHeroRetrieval = CsGameData.Instance.MyHeroInfo.GetHeroRetrieval(m_nRetrievalId);
		CsHeroRetrievalProgressCount csHeroRetrievalProgressCount = CsGameData.Instance.MyHeroInfo.GetHeroRetrievalProgressCount(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date.AddDays(-1), m_nRetrievalId);

		int nRetrievalCount = csHeroRetrieval == null ? 0 : csHeroRetrieval.Count;
		int nRetrievalProgressCount = csHeroRetrievalProgressCount == null ? 0 : csHeroRetrievalProgressCount.ProgressCount;

		return Math.Max(0, nMaxCount - nRetrievalCount - nRetrievalProgressCount);
	}
}
