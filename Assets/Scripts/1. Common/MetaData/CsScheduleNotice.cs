using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public enum EnScheduleNotice
{
    DimensionInfiltration = 1, // 차원 잠입
    BattleFieldSupport = 2, // 전장 지원
    GuildFram = 3, // 길드 농장
    RuinsReclaim = 4, // 유적 탈환
    FieldBoss = 5, // 필드 보스
    NationWar = 6, // 차원 전쟁
    WarMemory = 7, // 전쟁의기억
    InfiniteWar = 8, // 무한대전
    AnkouTomb = 9, // 안쿠의무덤
    TrandeShip = 10, // 무역선탈환
    SafeTime = 11, // 안전시간
    HolyWar = 12, // 위대한성전
}

public class CsScheduleNotice
{
	/*
	1 : 차원잠입
	2 : 전장지원
	3 : 길드농장
	4 : 유적탈환
	5 : 필드보스
	6 : 차원전쟁
	7 : 전쟁의기억
	8 : 무한대전
	9 : 안쿠의무덤
	10 : 무역선탈환
	11 : 안전시간
	12 : 위대한성전
	 */
	int m_nNoticeId;
	int m_nBeforeStartNoticeTime;
	string m_strBeforeStartNotice;
	string m_strStartNotice;
	string m_strEndNotice;

	//---------------------------------------------------------------------------------------------------
	public int NoticeId
	{
		get { return m_nNoticeId; }
	}

	public int BeforeStartNoticeTime
	{
		get { return m_nBeforeStartNoticeTime; }
	}

	public string BeforeStartNotice
	{
		get { return m_strBeforeStartNotice; }
	}

	public string StartNotice
	{
		get { return m_strStartNotice; }
	}

	public string EndNotice
	{
		get { return m_strEndNotice; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsScheduleNotice(WPDScheduleNotice scheduleNotice)
	{
		m_nNoticeId = scheduleNotice.noticeId;
		m_nBeforeStartNoticeTime = scheduleNotice.beforeStartNoticeTime;
		m_strBeforeStartNotice = CsConfiguration.Instance.GetString(scheduleNotice.beforeStartNoticeKey);
		m_strStartNotice = CsConfiguration.Instance.GetString(scheduleNotice.startNoticeKey);
		m_strEndNotice = CsConfiguration.Instance.GetString(scheduleNotice.endNoticeKey);
	}
}
