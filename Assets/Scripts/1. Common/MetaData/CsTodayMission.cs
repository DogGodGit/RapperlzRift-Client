using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-02-09)
//---------------------------------------------------------------------------------------------------

/*
1 : 낚시퀘스트
2 : 현상금사냥퀘스트
3 : 오시리스의방(골드던전)
4 : 수련동굴(경험치던전)
5 : 검투대회(PVP)
6 : 공포의제단
7 : 차원습격퀘스트
8 : 의문의상자퀘스트
9 : 보급지원
10 : 튜토리얼(목표0회)
*/

public enum EnTodayMissionType
{
	FishingQuest = 1,
	BountyHunterQuest = 2,
	GoldDungeon = 3,
	ExpDungeon = 4,
    PVP = 5,
    DiemnsionRaidQuest = 7, 
	MysteryBoxQuest = 8,
    SupplySupportQuest = 9, 
	Tutorial = 10
}

public class CsTodayMission
{
	int m_nMissionId;
	string m_strName;
	int m_nTargetCount;

	List<CsTodayMissionReward> m_listCsTodayMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public EnTodayMissionType TodayMissionType
	{
		get { return (EnTodayMissionType)m_nMissionId; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public string Name
	{
		get { return m_strName; }
	}


	public List<CsTodayMissionReward> TodayMissionRewardList
	{
		get { return m_listCsTodayMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsTodayMission(WPDTodayMission todayMission)
	{
		m_nMissionId = todayMission.missionId;
		m_strName = CsConfiguration.Instance.GetString(todayMission.nameKey);
		m_nTargetCount = todayMission.targetCount;

		m_listCsTodayMissionReward = new List<CsTodayMissionReward>();
	}
}
