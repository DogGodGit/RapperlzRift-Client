using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-21)
//---------------------------------------------------------------------------------------------------

public class CsOrdealQuestMission
{
	int m_nQuestNo;
	int m_nSlotIndex;
	int m_nMissionNo;
	string m_strName;
	int m_nType;
	/* 
	1 : 보조장비소울스톤 총레벨
	2 : 스태미나 구입
	3 : 탈것 레벨
	4 : 크리처 레벨
	5 : 날개 레벨
	6 : 현상금사냥
	7 : 몬스터처치
	8 : 현상금사냥(전설)
	9 : 낚시미끼 사용(전설)
	10 : 플래그 사용(전설)
	11 : 유적탈환 던전
	12 : 전쟁의 기억 던전
	13 : 차원습격
	14 : 위대한성전
	15 : 국가전
	16 : 밀서유출(전설)
	17 : 공적점수
	18 : 계급번호
	19 : 업적점수
	*/
	int m_nTargetCount;
	int m_nAutoCompletionRequiredTime;
	CsItem m_csItemAvailableReward;

	//---------------------------------------------------------------------------------------------------
	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int SlotIndex
	{
		get { return m_nSlotIndex; }
	}

	public int MissionNo
	{
		get { return m_nMissionNo; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public int TargetCount
	{
		get { return m_nTargetCount; }
	}

	public int AutoCompletionRequiredTime
	{
		get { return m_nAutoCompletionRequiredTime; }
	}

	public CsItem AvailableRewardItem
	{
		get { return m_csItemAvailableReward; }
	}

	public bool AutoCompletable
	{
		get { return m_nAutoCompletionRequiredTime > 0; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOrdealQuestMission(WPDOrdealQuestMission ordealQuestMission)
	{
		m_nQuestNo = ordealQuestMission.questNo;
		m_nSlotIndex = ordealQuestMission.slotIndex;
		m_nMissionNo = ordealQuestMission.missionNo;
		m_strName = CsConfiguration.Instance.GetString(ordealQuestMission.nameKey);
		m_nType = ordealQuestMission.type;
		m_nTargetCount = ordealQuestMission.targetCount;
		m_nAutoCompletionRequiredTime = ordealQuestMission.autoCompletionRequiredTime;
		m_csItemAvailableReward = CsGameData.Instance.GetItem(ordealQuestMission.availableRewardItemId);
	}
}
