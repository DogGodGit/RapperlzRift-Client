using System;
using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-13)
//---------------------------------------------------------------------------------------------------

/*
1 : 수련동굴(경험치던전) - vip
2 : 공포의제단
3 : 차원습격퀘스트 
4 : 의문의상자퀘스트
5 : 밀서유출퀘스트
6 : 낚시퀘스트
7 : 현상금사냥퀘스트
8 : 지하미로 - 사용시간/전체시간
9 : 용맹의증명 - n/a
10 : 스토리던전 - 플레이할수있는 스토리던전 입장회수 합
11 : 고대인의유적 - vip
12 : 검투대회(PVP)
13 : 농장의위협퀘스트
14 : 고대유물의방 - clientText(제한없음)
15 : 영혼을탐하는자
16 : 오시리스의방(골드던전) - vip
17 : 차원잠입
18 : 위대한성전퀘스트
19 : 보급지원퀘스트
20 : 길드미션
21 : 길드제단
22 : 길드물자지원
23 : 길드농장
24 : 길드군량
25 : 길드헌팅
26 : 일일퀘스트
27 : 주간퀘스트
28 : 지혜의신전
29 : 유적탈환
30 : 진정한영웅
31 : 필드보스
32 : 무한대전
33 : 전장지원
34 : 차원전쟁
35 : 크리처농장퀘스트
36 : 안전시간
37 : 전쟁의 기억
38 : 용의 둥지
39 : 망자의 유물(산체르크호)
40 : 황금의 무덤(안쿠 무덤)
*/

public enum EnTodayTaskType
{
	ExpDungeon = 1,
	FearAltar = 2,
	DimensionRaidQuest = 3,
	MysteryBoxQuest = 4,
	SecretLetterQuest = 5,
	FishingQuest = 6,
	BountyHunterQuest = 7,
	UndergroundMaze = 8,
	ProofOfValor = 9,
	StoryDungeon = 10,
	AncientRelic = 11,
	FieldOfHonor = 12,
	ThreatOfFarmQuest = 13,
	ArtifactRoom = 14,
    SoulCoveter = 15,
	OsirisRoom = 16,
	DimensionInfiltrationEvent = 17,
	HolyWarQuest = 18,
    SupplySupportQuest = 19,
    GuildMissionQuest = 20, 
    GuildAltar = 21, 
    GuildSupply = 22, 
    GuildFarmQuest = 23, 
    GuildFoodWarehouse = 24,
    GuildHunting = 25, 
    DailyQuest = 26, 
    WeeklyQuest = 27,
	WisdomTemple = 28,
	RuinsReclaim = 29,
	TrueHero = 30,
	FieldBoss = 31,
    InfiniteWar = 32, 
    BattleFieldSupportEvent = 33, 
    NationWar = 34, 
	CreatureFarm = 35,
    SafeTimeEvent = 36, // 안전시간
    WarMemory = 37, // 전쟁의 기억
    DragonNest = 38, // 용의둥지
    AnkouTomb = 39,
    TradeShip = 40,
}

public class CsTodayTask : IComparable
{
	int m_nTaskId;
	CsTodayTaskCategory m_csTodayTaskCategory;
	string m_strName;
	string m_strDescription;
	string m_strRewardText;
	string m_strEventTimeText;
	string m_strLockTextKey;
	int m_nRank;
	int m_nAchievementPoint;
	int m_nSortNo;
	bool m_bIsRecommend;

	List<CsTodayTaskAvailableReward> m_listCsTodayTaskAvailableReward;

	int m_nMaxCount;

	//---------------------------------------------------------------------------------------------------
	public int TaskId
	{
		get { return m_nTaskId; }
	}

	public EnTodayTaskType TodayTaskType
	{
		get { return (EnTodayTaskType)m_nTaskId; }
	}

	public CsTodayTaskCategory TodayTaskCategory
	{
		get { return m_csTodayTaskCategory; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string RewardText
	{
		get { return m_strRewardText; }
	}

	public string EventTimeText
	{
		get { return m_strEventTimeText; }
	}

	public string LockTextKey
	{
		get { return m_strLockTextKey; }
	}

	public int Rank
	{
		get { return m_nRank; }
	}

	public int AchievementPoint
	{
		get { return m_nAchievementPoint; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	public List<CsTodayTaskAvailableReward> TodayTaskAvailableRewardList
	{
		get { return m_listCsTodayTaskAvailableReward; }
	}

	public int MaxCount
	{
		get { return m_nMaxCount; }
	}

	public bool IsRecommend
	{
		get { return m_bIsRecommend; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayTask(WPDTodayTask todayTask)
	{
		m_nTaskId = todayTask.taskId;
		m_csTodayTaskCategory = CsGameData.Instance.GetTodayTaskCategory(todayTask.categoryId);
		m_strName = CsConfiguration.Instance.GetString(todayTask.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(todayTask.descriptionKey);
		m_strRewardText = CsConfiguration.Instance.GetString(todayTask.rewardTextKey);
		m_strEventTimeText = CsConfiguration.Instance.GetString(todayTask.eventTimeTextKey);
		m_strLockTextKey = CsConfiguration.Instance.GetString(todayTask.lockTextKey);
		m_nRank = todayTask.rank;
		m_nAchievementPoint = todayTask.achievementPoint;
		m_nSortNo = todayTask.sortNo;
		m_bIsRecommend = todayTask.isRecommend;

		m_listCsTodayTaskAvailableReward = new List<CsTodayTaskAvailableReward>();

		m_nMaxCount = 0;
	}

	//---------------------------------------------------------------------------------------------------
	public void Init()
	{
		switch (TodayTaskType)
		{
			case EnTodayTaskType.ExpDungeon:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount;
				break;

			case EnTodayTaskType.FearAltar:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount;
				break;

			case EnTodayTaskType.DimensionRaidQuest:
				m_nMaxCount = CsGameData.Instance.DimensionRaidQuest.LimitCount;
				break;

			case EnTodayTaskType.MysteryBoxQuest:
				m_nMaxCount = CsGameData.Instance.MysteryBoxQuest.LimitCount;
				break;

			case EnTodayTaskType.SecretLetterQuest:
				m_nMaxCount = CsGameData.Instance.SecretLetterQuest.LimitCount;
				break;

			case EnTodayTaskType.FishingQuest:
				m_nMaxCount = CsGameData.Instance.FishingQuest.LimitCount;
				break;

			case EnTodayTaskType.BountyHunterQuest:
				m_nMaxCount = CsGameConfig.Instance.BountyHunterQuestMaxCount;
				break;

			case EnTodayTaskType.UndergroundMaze:
				m_nMaxCount = 0;
				break;

			case EnTodayTaskType.ProofOfValor:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.ProofOfValorEnterCount;
				break;

			case EnTodayTaskType.StoryDungeon:
                m_nMaxCount = 0;

				for (int i = 0; i < CsGameData.Instance.StoryDungeonList.Count; i++)
				{
					if (CsGameData.Instance.StoryDungeonList[i].RequiredHeroMinLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.StoryDungeonList[i].RequiredHeroMaxLevel >= CsGameData.Instance.MyHeroInfo.Level)
					{
						m_nMaxCount += CsGameData.Instance.StoryDungeonList[i].EnterCount;
					}
				}
				break;

			case EnTodayTaskType.AncientRelic:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount;
				break;

			case EnTodayTaskType.FieldOfHonor:
				m_nMaxCount = 0;
				break;

			case EnTodayTaskType.ThreatOfFarmQuest:
				m_nMaxCount = CsGameData.Instance.ThreatOfFarmQuest.LimitCount;
				break;

			case EnTodayTaskType.ArtifactRoom:
				m_nMaxCount = 0;
				break;

			case EnTodayTaskType.SoulCoveter:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount;
				break;

			case EnTodayTaskType.OsirisRoom:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount;
				break;

			case EnTodayTaskType.DimensionInfiltrationEvent:
				m_nMaxCount = 0;
				break;

			case EnTodayTaskType.HolyWarQuest:
				m_nMaxCount = 0;
				break;
            
            case EnTodayTaskType.SupplySupportQuest:
                m_nMaxCount = CsGameData.Instance.SupplySupportQuest.LimitCount;
                break;

            case EnTodayTaskType.GuildMissionQuest:
                m_nMaxCount = CsGameData.Instance.GuildMissionQuest.LimitCount;
                break;
            
            case EnTodayTaskType.GuildAltar:
                m_nMaxCount = 1;
                break;
            case EnTodayTaskType.GuildSupply:
                m_nMaxCount = 1;
                break;
            
            case EnTodayTaskType.GuildFarmQuest:
                m_nMaxCount = CsGameData.Instance.GuildFarmQuest.LimitCount;
                break;
            
            case EnTodayTaskType.GuildFoodWarehouse:
                m_nMaxCount = CsGameData.Instance.GuildFoodWarehouse.LimitCount;
                break;

            case EnTodayTaskType.GuildHunting:
                m_nMaxCount = CsGuildManager.Instance.GuildHuntingQuest.LimitCount;
                break;

            case EnTodayTaskType.DailyQuest:
                m_nMaxCount = CsGameData.Instance.DailyQuest.PlayCount;
                break;

            case EnTodayTaskType.WeeklyQuest:
                m_nMaxCount = CsGameData.Instance.WeeklyQuest.RoundCount;
                break;

			case EnTodayTaskType.WisdomTemple:
				m_nMaxCount = 1;
				break;

			case EnTodayTaskType.RuinsReclaim:
				m_nMaxCount = CsGameData.Instance.RuinsReclaim.FreeEnterCount;
				break;

			case EnTodayTaskType.TrueHero:
				m_nMaxCount = 1;
				break;

			case EnTodayTaskType.FieldBoss:
				m_nMaxCount = 1;
				break;

            case EnTodayTaskType.InfiniteWar:
                m_nMaxCount = CsGameData.Instance.InfiniteWar.EnterCount;
                break;

            case EnTodayTaskType.BattleFieldSupportEvent:
                m_nMaxCount = 0;
                break;

            case EnTodayTaskType.NationWar:
                m_nMaxCount = 0;
                break;

			case EnTodayTaskType.CreatureFarm:
				m_nMaxCount = CsGameData.Instance.CreatureFarmQuest.LimitCount;
				break;

            case EnTodayTaskType.SafeTimeEvent:
                m_nMaxCount = 0;
                break;

            case EnTodayTaskType.WarMemory:
                m_nMaxCount = CsGameData.Instance.WarMemory.FreeEnterCount;
                break;

            case EnTodayTaskType.DragonNest:
                m_nMaxCount = 0;
                break;

			case EnTodayTaskType.TradeShip:
				m_nMaxCount = CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount;
				break;

			case EnTodayTaskType.AnkouTomb:
				m_nMaxCount = CsGameData.Instance.AnkouTomb.EnterCount;
				break;
		}
	}


	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsTodayTask)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}