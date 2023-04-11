using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-02)
//---------------------------------------------------------------------------------------------------

public enum EnMenuGroup
{
	MainMenu1 = 1,
	MainMenu2 = 2,
	MainMenu3 = 3,
    MainMenu4 = 4,
    MainMenu5 = 5,
	MainMenu6 = 6,
}

public enum EnMenuId
{
    Inventory = 1,          // 인벤토리
    Mail = 2,               // 우편
    Dungeon = 3,            // 던전
    TodayTask = 4,          // 오늘의 할일
    Attaniment = 5,         // 도달보상
    Character = 6,          // 캐릭터정보
    Skill = 7,              // 스킬
    Achievement = 8,        // 업적
    Rank = 9,               // 계급
    Collection = 10,        // 컬렉션
    MainGear = 11,          // 장비
    SubGear = 12,           // 보조장비
    Mount = 13,             // 탈 것
    Wing = 14,              // 날개
    Creature = 15,          // 크리처
    Soul = 16,              // 정령
    Constellation = 17,     // 별자리
    Ranking = 18,           // 랭킹
    Friend = 19,            // 친구
    Guild = 20,             // 길드
    Nation = 21,            // 국가
    Shop = 22,              // 상점
    Support = 23,           // 지원
    Retrieval = 24,         // 회수
    Setting = 25,           // 설정
    Vip = 27,               // VIP
	OpenGift = 28,			// 오픈 선물
	RookieGift = 29,		// 신병 선물
    Open7Day = 30,          // 7일 이벤트
    LuckyShop = 31,         // 행운 상점
	ChargingEvent = 32,			// 충전
}

public class CsMenu : IComparable
{
	int m_nMenuId;
	string m_strName;
	string m_strImageName;
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;
	int m_nMenuGroup;
	int m_nSortNo;

	//---------------------------------------------------------------------------------------------------
	public int MenuId
	{
		get { return m_nMenuId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public int MenuGroup
	{
		get { return m_nMenuGroup; }
	}

	public int SortNo
	{
		get { return m_nSortNo; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenu(WPDMenu menu)
	{
		m_nMenuId = menu.menuId;
		m_strName = CsConfiguration.Instance.GetString(menu.nameKey);
		m_strImageName = menu.imageName;
		m_nRequiredHeroLevel = menu.requiredHeroLevel;
		m_nRequiredMainQuestNo = menu.requiredMainQuestNo;
		m_nMenuGroup = menu.menuGroup;
		m_nSortNo = menu.sortNo;
	}

	#region Interface(IComparable) implement
	//---------------------------------------------------------------------------------------------------
	public int CompareTo(object obj)
	{
		return m_nSortNo.CompareTo(((CsMenu)obj).SortNo);
	}
	#endregion Interface(IComparable) implement
}
