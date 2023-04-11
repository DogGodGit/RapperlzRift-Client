using System;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-23)
//---------------------------------------------------------------------------------------------------

public enum EnSubMenu
{
    Default = 0,
    CharacterInfo = 101, 
    Inventory = 102, 
    Warehouse = 103, 
    Costume = 104, 
    Skill = 201,
    MainGearEnchant = 301,
    MainGearRefine = 302,
    MainGearTransit = 303,
    SubGearLevelUp = 401,
    SubGearSoulstone = 402,
    SubGearRune = 403,
    Mail = 501,
    SurroundingHero = 601,
    SurroundingParty = 602,
    MyParty = 603,
    PartyMatching = 604,
    TodayMission = 701,
    SeriesMission = 702,
    AccessReward = 703,
    AttendReward = 704,
    LevelUpReward = 705,
    LimitationGift = 706, 
    WeekendReward = 707, 
	SharingEventReward = 708,
    Receipt = 801,
    MountLevelUp = 901,
    MountGearRefine = 902,
    MountGearPickBoxMake = 903,
    MountAwakening = 904, 
    WingEnchant = 1001,
    WingAppearance = 1002,
    WingEquipment = 1003, 
    StoryDungeon = 1101,
    IndividualDungeon = 1102,
    PartyDungeon = 1103,
    TimeLimitDungeon = 1104,
	EventDungeon = 1105,
    VipInfo = 1201,
    MinimapArea = 1301,
    MinimapMap = 1302,
    MinimapWorld = 1303,
    TodayTaskExp = 1401,
    TodayTaskItem = 1402,
    TodayTaskLimit = 1403,
    Class = 1501,
	ClassSkill = 1502,
    RankingIndividual = 1601,
    RankingNation = 1602,
    RankingJob = 1603,
    RankingGuild = 1604,
    OtherUsers = 1701,
    BattleSetting = 1801,
    GameSetting = 1802,
    GraphicSetting = 1803,
    InfoSetting = 1804,
    GuildMember = 1901,
    GuildEvent = 1902,
    GuildBuilding = 1903,
    NationInfo = 2001,
    NationDiplomacy = 2002,
    NationAlliance = 2003, 
    NationWarInfo = 2101,
    NationWarHeroObjective = 2102,
    Accomplishment = 2201,
    Title = 2202,
    IllustBook = 2203,
    CardCollection = 2301,
    CardInventory = 2302,
    CardShop = 2303,
	Biography = 2304,
    DailyQuest = 2401, 
    WeeklyQuest = 2501, 
    DiaShop = 2601, 
    LuckyShop = 2701, 
    Friend = 2801,
	FriendBlessing = 2802,
    TempFriend = 2803, 
    BlackList = 2804, 
	CreatureTraining = 2901,
	CreatureInjection = 2902,
	CreatureComposition = 2903,
	Soul = 3001,
	Constellation = 3101,
}

public class CsSubMenu : IComparable
{
    int m_nMenuId;
    int m_nSubMenuId;
    string m_strName;
    string[] m_astrPrefabs;
    int m_nLayout;
    bool m_bIsDefault;
    int m_nSortNo;
    int m_nContentId;

    //---------------------------------------------------------------------------------------------------
    public int MenuId
    {
        get { return m_nMenuId; }
    }

    public int SubMenuId
    {
        get { return m_nSubMenuId; }
    }

    public EnSubMenu EnSubMenu
    {
        get { return (EnSubMenu)m_nSubMenuId; }
    }

    public string Name
    {
        get { return m_strName; }
    }

    public string[] Prefabs
    {
        get { return m_astrPrefabs; }
    }

    public int Layout
    {
        get { return m_nLayout; }
    }

    public bool IsDefault
    {
        get { return m_bIsDefault; }
    }

    public int SortNo
    {
        get { return m_nSortNo; }
    }

    public int ContentId
    {
        get { return m_nContentId; }
    }

    //---------------------------------------------------------------------------------------------------
    public CsSubMenu(WPDSubMenu subMenu)
    {
        m_nMenuId = subMenu.menuId;
        m_nSubMenuId = subMenu.subMenuId;
        m_strName = CsConfiguration.Instance.GetString(subMenu.nameKey);
        m_astrPrefabs = new string[3];
        m_astrPrefabs[0] = subMenu.prefab1;
        m_astrPrefabs[1] = subMenu.prefab2;
        m_astrPrefabs[2] = subMenu.prefab3;
        m_nLayout = subMenu.layout;
        m_bIsDefault = subMenu.isDefault;
        m_nSortNo = subMenu.sortNo;
        m_nContentId = subMenu.contentId;
    }

    #region Interface(IComparable) implement
    //---------------------------------------------------------------------------------------------------
    public int CompareTo(object obj)
    {
        return m_nSortNo.CompareTo(((CsSubMenu)obj).SortNo);
    }

    #endregion Interface(IComparable) implement
}
