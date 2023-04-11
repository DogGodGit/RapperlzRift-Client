using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-07-16)
//---------------------------------------------------------------------------------------------------

public class CsOpen7DayEventMission
{
	int m_nMissionId;
	int m_nDay;

	/*
		1 : 레벨
		2 : 스토리던전 참여
		3 : 보조장비레벨
		4 : 경험치던전 참여
		5 : 현상금사냥
		6 : 보석레벨 합
		7 : 전투력
		8 : 용맹의증명 참여
		9 : 길드미션 
		10 : 메인장비 강화
		11 : 낚시
		12 : 계급
		13 : 밀서유출
		14 : 의문의상자
		15 : 차원습격
		16 : 위대한성전
	*/
	int m_nType;
	long m_lTargetValue;

	List<CsOpen7DayEventMissionReward> m_listCsOpen7DayEventMissionReward;

	//---------------------------------------------------------------------------------------------------
	public int MissionId
	{
		get { return m_nMissionId; }
	}

	public int Day
	{
		get { return m_nDay; }
	}

	public int Type
	{
		get { return m_nType; }
	}

	public long TargetValue
	{
		get { return m_lTargetValue; }
	}

	public List<CsOpen7DayEventMissionReward> Open7DayEventMissionRewardList
	{
		get { return m_listCsOpen7DayEventMissionReward; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsOpen7DayEventMission(WPDOpen7DayEventMission open7DayEventMission)
	{
		m_nMissionId = open7DayEventMission.missionId;
		m_nDay = open7DayEventMission.day;
		m_nType = open7DayEventMission.type;
		m_lTargetValue = open7DayEventMission.targetValue;

		m_listCsOpen7DayEventMissionReward = new List<CsOpen7DayEventMissionReward>();
	}
}
