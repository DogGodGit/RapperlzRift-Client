using System.Collections.Generic;
using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-02)
//---------------------------------------------------------------------------------------------------

public enum EnMenuContentId
{
    CharacterInfo = 11,
    Inventory = 12,
    Warehouse = 13, 
    Skill = 31,
    Rank = 51,
    MainGearEnchant = 61,
    MainGearRefine = 62,
    MainGearTransit = 63,
    SubGearLevelUp = 71,
    SubGearSoulstone = 72,
    SubGearRune = 73,
    MountLevelUp = 81,
    MountGearRefine = 82,
    MountGearPickBoxMake = 83,
    WingEnchant = 91,
    WingEquip = 92,
	Biography = 104,
    RankingIndividual = 141,
    RankingNation = 142,
    RankingJob = 143,
    NationDonation = 171,
    TodayMission = 191,
    SeriesMisson = 192,
    AccessReward = 193,
    AttendReward = 194,
    LevelUpReward = 195,
    LimitationGift = 196, 
    TodayTask = 241,
    Dungeon = 251,
    StoryDungeon = 1001,
    ExpDungeon = 1002,
    FieldOfHonor = 1003,
    GoldDungeon = 1004,
    PartyDungeon2 = 1005,
    ArtifactRoom = 1006,
    UndergroundMaze = 1007,
    AncientRelic = 1008,
    BountyHunterQuest = 2001,
    DimensionRaidQuest = 2002,
    SupportQuest = 2003,
    HolyWarQuest = 2004,
    Fishing = 2005,
    CsMysteryBoxQuest = 2006,
    SecretLetterQuest = 2007,
    ThreatOfFarmQuest = 2008,
    HpPotion = 3001,
    ReturnScroll = 3002,
    Mount = 3003,
}

public class CsMenuContent
{
	int m_nContentId;
	CsMenu m_csMenu;
	string m_strName;
	string m_strDescription;
	string m_strImageName;
	int m_nRequiredConditionType;   //필요조건타입		1:영웅레벨,2:메인퀘스트(완료),3:메인퀘스트(수락)
	int m_nRequiredHeroLevel;
	int m_nRequiredMainQuestNo;
	bool m_bIsDisplay;

	List<CsMenuContentTutorialStep> m_listCsMenuContentTutorialStep;

	//---------------------------------------------------------------------------------------------------
	public int ContentId
	{
		get { return m_nContentId; }
	}

	public CsMenu Menu
	{
		get { return m_csMenu; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Description
	{
		get { return m_strDescription; }
	}

	public string ImageName
	{
		get { return m_strImageName; }
	}

	public int RequiredConditionType
	{
		get { return m_nRequiredConditionType; }
	}

	public int RequiredHeroLevel
	{
		get { return m_nRequiredHeroLevel; }
	}

	public int RequiredMainQuestNo
	{
		get { return m_nRequiredMainQuestNo; }
	}

	public bool IsDisplay
	{
		get { return m_bIsDisplay; }
	}

	public List<CsMenuContentTutorialStep> MenuContentTutorialStepList
	{
		get { return m_listCsMenuContentTutorialStep; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMenuContent(WPDMenuContent menuContent)
	{
		m_nContentId = menuContent.contentId;
		m_csMenu = CsGameData.Instance.GetMenu(menuContent.menuId);
		m_strName = CsConfiguration.Instance.GetString(menuContent.nameKey);
		m_strDescription = CsConfiguration.Instance.GetString(menuContent.descriptionKey);
		m_strImageName = menuContent.imageName;
		m_nRequiredHeroLevel = menuContent.requiredHeroLevel;
		m_nRequiredMainQuestNo = menuContent.requiredMainQuestNo;
		m_bIsDisplay = menuContent.isDisplay;

		m_listCsMenuContentTutorialStep = new List<CsMenuContentTutorialStep>();
	}
}
