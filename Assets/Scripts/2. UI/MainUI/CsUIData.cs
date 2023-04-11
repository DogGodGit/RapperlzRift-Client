using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-06-13)
//---------------------------------------------------------------------------------------------------

public enum EnUISoundType
{
    Button = 0,
    ToggleGroup = 1,
    Toggle = 2,
    MaingearEnchanting = 3,
    MaingearReinforceFail = 4,
    MaingearReinforceSuccess = 5,
    MaingearTransition = 6,
    SkillLevelup = 7,
    SubgearRuneEquip = 8,
    SubgearRuneUnEquip = 9,
    SubgearSoulstoneEquip = 10,
    SubgearSoulstoneUnEquip = 11,
    SubgearUpgrade = 12,
    BattlePowerUp = 13,
    BattlePowerDown = 14,
    CharacterLevelUp = 15,
    PowerCounting = 16,
    Weapon = 17,
    Armor = 18,
    Ring = 19,
    Gold = 20,
    Item = 21,
    ReleaseWeapon = 22, 
    ReleaseArmor = 23, 
    ReleaseSubGear = 24, 
    QuestComplete = 25, 
}

public enum EnDungeon
{
    None = 0,
    MainQuestDungeon = 1,
    StoryDungeon = 2,
    ExpDungeon = 3,
    GoldDungeon = 4, 
    UndergroundMaze = 5,
    ArtifactRoom = 6,
    AncientRelic = 7,
    FieldOfHonor = 8,
    SoulCoveter = 9,
    EliteDungeon = 10,
    ProofOfValor = 11,
    WisdomTemple = 12,
	RuinsReclaim = 13,
    InfiniteWar = 14, 
	FearAltar = 15,
    WarMemory = 16,
    OsirisRoom = 17,
 	BiographyDungeon = 18,
    DragonNest = 19,
    AnkouTomb = 20, 
    TradeShip = 21, 
	TeamBattlefield = 22,
}

public enum EnAutoStateType
{
    None = 0,
    MainQuest = 1,
    Move = 2,
    Battle = 3,
    Hunter = 4,
    SecretLetter = 5,
    MysteryBox = 6,
    DimensionRaid = 7,
    Fishing = 8,
    ThreatOfFarm = 9,
    HolyWar = 10, 
    SupplySupport = 11,
    GuildMission = 12,
    GuildAlter = 13,
    GuildFarm = 14,
    GuildFoodWareHouse = 15,
    GuildSupplySupport = 16,
    GuildHunting = 17,
    GuildFishing = 18,
    NationWar = 19,
    GuildAlterDefence = 20,
    DailyQuest01 = 21,
    DailyQuest02 = 22,
    DailyQuest03 = 23,
    WeeklyQuest = 24,
    TrueHero = 25,
    TrueHeroTaunted = 26,
    SubQuest = 27,
    FindingPath = 28,
	Biography = 29,
	CreatureFarm = 30,
	JobChange = 31,
}

public enum EnItemSlotSize
{
    Medium = 0,
    Small = 1,
    Large = 2
}

public enum EnItemSlotType
{
    MainGear = 1,
    SubGear = 2,
    Material = 3,
    Consume = 4,
}

public enum EnIntroShortCutType
{
    Logo = 1,				// 초기
    LogOut = 2,				// 계정변경
    CharacterSelect = 3,	// 캐릭터선택
}

public enum EnServerLanguage : int
{
    None = -1,
    Unknown = 0,
    Afrikaans = 1,
    Arabic = 2,
    Basque = 3,
    Belarusian = 4,
    Bulgarian = 5,
    Catalan = 6,
    Chinese = 7,
    Czech = 8,
    Danish = 9,
    Dutch = 10,
    English = 11,
    Estonian = 12,
    Faroese = 13,
    Finnish = 14,
    French = 15,
    German = 16,
    Greek = 17,
    Hebrew = 18,
    Icelandic = 19,
    Indonesian = 20,
    Italian = 21,
    Japanese = 22,
    Korean = 23,
    Latvian = 24,
    Lithuanian = 25,
    Norwegian = 26,
    Polish = 27,
    Portuguese = 28,
    Romanian = 29,
    Russian = 30,
    SerboCroatian = 31,
    Slovak = 32,
    Slovenian = 33,
    Spanish = 34,
    Swedish = 35,
    Thai = 36,
    Turkish = 37,
    Ukrainian = 38,
    Vietnamese = 39,
    ChineseSimplified = 40,
    ChineseTraditional = 41,
    Hungarian = 42,
}

public class CsUIData
{
	Vector2 m_vt2DeviceResolution;

    EnServerLanguage m_enLanguage;
	string m_strMaintenanceInfoUrl;

    Font m_fontResource;
    Font m_fontDamageResource;
    Font m_fontDungeonDamageResource;
    EnIntroShortCutType m_enIntroShortCutType = EnIntroShortCutType.Logo;

    int m_nSubGearId = 0;
    System.Guid m_guidMainGearId = System.Guid.Empty;
    int m_nSelectMountId = 0;

    Color32 m_colorRed = new Color32(229, 115, 115, 255);
    Color32 m_colorGreen = new Color32(51, 153, 0, 255);
    Color32 m_colorGray = new Color32(133, 141, 148, 255);
    Color32 m_colorWhite = new Color32(222, 222, 222, 255);		
    Color32 m_colorSkyblue = new Color32(127, 213, 246, 255);
    Color32 m_colorButtonOn = new Color32(255, 255, 255, 255);
    Color32 m_colorBtnOff = new Color32(108, 113, 117, 255);
    Color32 m_colorMellow = new Color32(171, 130, 255, 255);

	// 등급 별 색상
	Color32 m_colorNormal = new Color32(222, 222, 222, 255);// 일반
	Color32 m_colorLuxury = new Color32(57, 142, 61, 255);	// 고급
	Color32 m_colorMagic = new Color32(40, 121, 255, 255);	// 마법
	Color32 m_colorRare = new Color32(126, 87, 194, 255);	// 희귀
	Color32 m_colorLegend = new Color32(254, 148, 0, 255);	// 전설

    EnDungeon m_enDungeon = EnDungeon.None;
    EnAutoStateType m_enAutoStateType = EnAutoStateType.None;

    Sprite m_spriteUpArrow;
    Sprite m_spriteDownArrow;

    // 물약ID
    const int c_nHpPotionId = 101;
    float m_flHpPotionRemainingCoolTime = 0;

    // 귀환주문서ID
    const int c_nReturnScrollId = 201;
    float m_flReturnScrollRemainingCoolTime = 0;
    float m_flReturnScrollRemainingCastTime = 0;
    float m_flPotionCoolTime;
    float m_flReturnScrollCoolTime;

    bool m_bIsFirstLoading = true;

	//// 첫 접속 시 공지사항
	//bool m_bIsFirstVisit = true;

    // 채팅
    List<CsChattingMessage> m_listCsChattingMessage = new List<CsChattingMessage>();
    // 일대일
    List<CsChattingMessage> m_listOneToOne = new List<CsChattingMessage>();
    // UI사운드
    Dictionary<EnUISoundType, AudioClip> m_dicAudioClip = new Dictionary<EnUISoundType, AudioClip>();

    AudioSource m_audioSourceUI;

    //---------------------------------------------------------------------------------------------------
	public Vector2 DeviceResolution
	{
		get { return m_vt2DeviceResolution; }
	}

    public float HpPotionCoolTime
    {
        get { return m_flPotionCoolTime; }
        set { m_flPotionCoolTime = value; }
    }

    public float ReturnScrollCoolTime
    {
        get { return m_flReturnScrollCoolTime; }
        set { m_flReturnScrollCoolTime = value; }
    }

    public int HpPotionId
    {
        get { return c_nHpPotionId; }
    }

    public float HpPotionRemainingCoolTime
    {
        get { return m_flHpPotionRemainingCoolTime; }
        set { m_flHpPotionRemainingCoolTime = value; }
    }

    public int ReturnScrollId
    {
        get { return c_nReturnScrollId; }
    }

    public float ReturnScrollRemainingCoolTime
    {
        get { return m_flReturnScrollRemainingCoolTime; }
        set { m_flReturnScrollRemainingCoolTime = value; }
    }

    public float ReturnScrollRemainingCastTime
    {
        get { return m_flReturnScrollRemainingCastTime; }
        set { m_flReturnScrollRemainingCastTime = value; }
    }

    public System.Guid MainGearId
    {
        get { return m_guidMainGearId; }
        set { m_guidMainGearId = value; }
    }

    public int SubGearId
    {
        get { return m_nSubGearId; }
        set { m_nSubGearId = value; }
    }

    public EnServerLanguage Language
    {
        get { return m_enLanguage; }
        set { m_enLanguage = value; }
    }

	public string MaintenanceInfoUrl
	{
		get { return m_strMaintenanceInfoUrl; }
		set { m_strMaintenanceInfoUrl = value; }
	}

    public static CsUIData Instance
    {
        get { return CsSingleton<CsUIData>.GetInstance(); }
    }

    public EnIntroShortCutType IntroShortCutType
    {
        get { return m_enIntroShortCutType; }
        set { m_enIntroShortCutType = value; }
    }

    public Color32 ColorRed
    {
        get { return m_colorRed; }
    }

    public Color32 ColorGreen
    {
        get { return m_colorGreen; }
    }

    public Color32 ColorGray
    {
        get { return m_colorGray; }
    }

    public Color32 ColorWhite
    {
        get { return m_colorWhite; }
    }
    public Color32 ColorSkyblue
    {
        get { return m_colorSkyblue; }
    }

    public Color32 ColorButtonOn
    {
        get { return m_colorButtonOn; }
    }

    public Color32 ColorButtonOff
    {
        get { return m_colorBtnOff; }
    }

    public Color32 ColorMellow
    {
        get { return m_colorMellow; }
    }

	#region 등급 별 색상
	public Color32 ColorNormal
    {
        get { return m_colorNormal; }
    }

	public Color32 ColorLuxury
    {
        get { return m_colorLuxury; }
    }

	public Color32 ColorMagic
    {
        get { return m_colorMagic; }
    }

	public Color32 ColorRare
    {
        get { return m_colorRare; }
    }

	public Color32 ColorLegend
    {
        get { return m_colorLegend; }
    }
	#endregion 등급 별 색상

	public List<CsChattingMessage> ChattingMessageList
    {
        get { return m_listCsChattingMessage; }
    }

    public List<CsChattingMessage> OneToOneList
    {
        get { return m_listOneToOne; }
    }

    public EnDungeon DungeonInNow
    {
        get { return m_enDungeon; }
        set { m_enDungeon = value; }
    }

    public bool FirstLoading
    {
        get { return m_bIsFirstLoading; }
        set { m_bIsFirstLoading = value; }
    }

    public EnAutoStateType AutoStateType
    {
        get { return m_enAutoStateType; }
        set { m_enAutoStateType = value; }
    }

    public int SelectMountId
    {
        get { return m_nSelectMountId; }
        set { m_nSelectMountId = value; }
    }

	//public bool FirstVisit
	//{
	//	get { return m_bIsFirstVisit; }
	//	set { m_bIsFirstVisit = value; }
	//}

	//---------------------------------------------------------------------------------------------------
	public void SetDeviceResolution()
	{
		m_vt2DeviceResolution = new Vector2(Screen.width, Screen.height);
	}

    //---------------------------------------------------------------------------------------------------
    public EnServerLanguage ConvertToServerLanguage(string sLanguage)
    {
        switch (sLanguage)
        {
            case "af":
                return EnServerLanguage.Afrikaans;
            case "ar":
                return EnServerLanguage.Arabic;
            case "bas":
                return EnServerLanguage.Basque;
            case "be":
                return EnServerLanguage.Belarusian;
            case "bg":
                return EnServerLanguage.Bulgarian;
            case "ca":
                return EnServerLanguage.Catalan;
            case "zh":
                return EnServerLanguage.Chinese;
            case "cs":
                return EnServerLanguage.Czech;
            case "da":
                return EnServerLanguage.Danish;
            case "nl":
                return EnServerLanguage.Dutch;
            case "en":
                return EnServerLanguage.English;
            case "et":
                return EnServerLanguage.Estonian;
            case "fo":
                return EnServerLanguage.Faroese;
            case "fi":
                return EnServerLanguage.Finnish;
            case "fr":
                return EnServerLanguage.French;
            case "de":
                return EnServerLanguage.German;
            case "el":
                return EnServerLanguage.Greek;
            case "iw":
                return EnServerLanguage.Hebrew;
            case "is":
                return EnServerLanguage.Icelandic;
            case "in":
                return EnServerLanguage.Indonesian;
            case "it":
                return EnServerLanguage.Italian;
            case "ja":
                return EnServerLanguage.Japanese;
            case "ko":
                return EnServerLanguage.Korean;
            case "lv":
                return EnServerLanguage.Latvian;
            case "lt":
                return EnServerLanguage.Lithuanian;
            case "nn":
                return EnServerLanguage.Norwegian;
            case "pl":
                return EnServerLanguage.Polish;
            case "pt":
                return EnServerLanguage.Portuguese;
            case "ro":
                return EnServerLanguage.Romanian;
            case "ru":
                return EnServerLanguage.Russian;
            case "sr":
                return EnServerLanguage.SerboCroatian;
            case "sk":
                return EnServerLanguage.Slovak;
            case "sl":
                return EnServerLanguage.Slovenian;
            case "es":
                return EnServerLanguage.Spanish;
            case "sv":
                return EnServerLanguage.Swedish;
            case "th":
                return EnServerLanguage.Thai;
            case "tr":
                return EnServerLanguage.Turkish;
            case "uk":
                return EnServerLanguage.Ukrainian;
            case "vi":
                return EnServerLanguage.Vietnamese;
            case "hu":
                return EnServerLanguage.Hungarian;
            default:
                return EnServerLanguage.English;
        }
    }

    public EnServerLanguage ConvertToServerLanguage(SystemLanguage systemLanguage)
    {
        switch (systemLanguage)
        {
            case SystemLanguage.Unknown:
                return EnServerLanguage.Unknown;
            case SystemLanguage.Afrikaans:
                return EnServerLanguage.Afrikaans;
            case SystemLanguage.Arabic:
                return EnServerLanguage.Arabic;
            case SystemLanguage.Basque:
                return EnServerLanguage.Basque;
            case SystemLanguage.Belarusian:
                return EnServerLanguage.Belarusian;
            case SystemLanguage.Bulgarian:
                return EnServerLanguage.Bulgarian;
            case SystemLanguage.Catalan:
                return EnServerLanguage.Catalan;
            case SystemLanguage.Chinese:
                return EnServerLanguage.Chinese;
            case SystemLanguage.Czech:
                return EnServerLanguage.Czech;
            case SystemLanguage.Danish:
                return EnServerLanguage.Danish;
            case SystemLanguage.Dutch:
                return EnServerLanguage.Dutch;
            case SystemLanguage.English:
                return EnServerLanguage.English;
            case SystemLanguage.Estonian:
                return EnServerLanguage.Estonian;
            case SystemLanguage.Faroese:
                return EnServerLanguage.Faroese;
            case SystemLanguage.Finnish:
                return EnServerLanguage.Finnish;
            case SystemLanguage.French:
                return EnServerLanguage.French;
            case SystemLanguage.German:
                return EnServerLanguage.German;
            case SystemLanguage.Greek:
                return EnServerLanguage.Greek;
            case SystemLanguage.Hebrew:
                return EnServerLanguage.Hebrew;
            case SystemLanguage.Icelandic:
                return EnServerLanguage.Icelandic;
            case SystemLanguage.Indonesian:
                return EnServerLanguage.Indonesian;
            case SystemLanguage.Italian:
                return EnServerLanguage.Italian;
            case SystemLanguage.Japanese:
                return EnServerLanguage.Japanese;
            case SystemLanguage.Korean:
                return EnServerLanguage.Korean;
            case SystemLanguage.Latvian:
                return EnServerLanguage.Latvian;
            case SystemLanguage.Lithuanian:
                return EnServerLanguage.Lithuanian;
            case SystemLanguage.Norwegian:
                return EnServerLanguage.Norwegian;
            case SystemLanguage.Polish:
                return EnServerLanguage.Polish;
            case SystemLanguage.Portuguese:
                return EnServerLanguage.Portuguese;
            case SystemLanguage.Romanian:
                return EnServerLanguage.Romanian;
            case SystemLanguage.Russian:
                return EnServerLanguage.Russian;
            case SystemLanguage.SerboCroatian:
                return EnServerLanguage.SerboCroatian;
            case SystemLanguage.Slovak:
                return EnServerLanguage.Slovak;
            case SystemLanguage.Slovenian:
                return EnServerLanguage.Slovenian;
            case SystemLanguage.Spanish:
                return EnServerLanguage.Spanish;
            case SystemLanguage.Swedish:
                return EnServerLanguage.Swedish;
            case SystemLanguage.Thai:
                return EnServerLanguage.Thai;
            case SystemLanguage.Turkish:
                return EnServerLanguage.Turkish;
            case SystemLanguage.Ukrainian:
                return EnServerLanguage.Ukrainian;
            case SystemLanguage.Vietnamese:
                return EnServerLanguage.Vietnamese;
            case SystemLanguage.ChineseSimplified:
                return EnServerLanguage.ChineseSimplified;
            case SystemLanguage.ChineseTraditional:
                return EnServerLanguage.ChineseTraditional;
            case SystemLanguage.Hungarian:
                return EnServerLanguage.Hungarian;
            default:
                return EnServerLanguage.English;
        }
    }



    //---------------------------------------------------------------------------------------------------
    public void LoadFont(Font font)
    {
        m_fontResource = font;
    }

    //---------------------------------------------------------------------------------------------------
    public void LoadDamageFont(Font font)
    {
        m_fontDamageResource = font;
    }

    //---------------------------------------------------------------------------------------------------
    public void LoadDungeonDamageFont(Font font)
    {
        m_fontDungeonDamageResource = font;
    }

    //---------------------------------------------------------------------------------------------------
    public void SetFont(Text text)
    {
        if (m_fontResource != null)
        {
            if (text.font == null || text.font.name != m_fontResource.name)
            {
                text.font = m_fontResource;
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
	public void SetText(Transform trText, string strString, bool bStringIsKey)
	{
		Text text = trText.GetComponent<Text>();

		if (text != null)
		{
			SetFont(text);

			if (bStringIsKey)
			{
				text.text = CsConfiguration.Instance.GetString(strString);
			}
			else
			{
				text.text = strString;
			}
		}
		else
		{
			Debug.Log("Text component is null");
		}
	}

    //---------------------------------------------------------------------------------------------------
    public void SetDamageFont(Text text)
    {
        if (m_fontDamageResource != null)
        {
            if (text.font == null || text.font.name != m_fontDamageResource.name)
            {
                text.font = m_fontDamageResource;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetDungeonDamageFont(Text text)
    {
        if (m_fontDungeonDamageResource != null)
        {
            if (text.font == null || text.font.name != m_fontDungeonDamageResource.name)
            {
                text.font = m_fontDungeonDamageResource;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public T LoadAsset<T>(string strPath) where T : Object
    {
        return Resources.Load<T>(strPath);
    }

    public ResourceRequest LoadAssetAsync<T>(string strPath) where T : Object
    {
        return Resources.LoadAsync<T>(strPath);
    }

    public T LoadAssetNation<T>(string strImageName) where T : Object
    {
        return Resources.Load<T>("GUINation/" + CsConfiguration.Instance.ServiceCode.ToString() + "/" + strImageName);
    }

    #region 아이템 슬롯 디스플레이 함수

    //---------------------------------------------------------------------------------------------------
    public void InitializeItemSlot(Transform trSlot)
    {
        if (m_spriteUpArrow == null)
        {
            m_spriteUpArrow = LoadAsset<Sprite>("GUI/Items/ico_up");
        }

        if (m_spriteDownArrow == null)
        {
            m_spriteDownArrow = LoadAsset<Sprite>("GUI/Items/ico_down");
        }

        Transform trGear = trSlot.Find("Gear");
        Transform trItem = trSlot.Find("Item");

        Text textEnchantLevel = trGear.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEnchantLevel);

        Text textCount = trItem.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);

        trSlot.Find("ImageRed").gameObject.SetActive(false);

        //Image imageCoolTime = trSlot.Find("ImageCooltime").GetComponent<Image>();
        //imageCoolTime.fillAmount = 0;
    }

    //---------------------------------------------------------------------------------------------------
    //메인장비용
    public void DisplayItemSlot(Transform trSlot, CsHeroMainGear csHeroMainGear, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplayMainGear(trSlot, csHeroMainGear.MainGear, csHeroMainGear.EnchantLevel, csHeroMainGear.BattlePower, csHeroMainGear.Owned, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsMainGear csMainGear, int nEnchantLevel, int nBattlePower, bool bOwned, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplayMainGear(trSlot, csMainGear, nEnchantLevel, nBattlePower, bOwned, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlotOtherUser(Transform trSlot, CsMainGear csMainGear, int nEnchantLevel, int nBattlePower, bool bOwned, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        InitializeItemSlot(trSlot);

        Transform trGear = trSlot.Find("Gear");
        trGear.gameObject.SetActive(true);
        Transform trItem = trSlot.Find("Item");
        trItem.gameObject.SetActive(false);

        trSlot.Find("ImageOwn").gameObject.SetActive(bOwned);

        //아이템아이콘
        Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csMainGear.Image);

        Text textEnchantLevel = trGear.Find("TextLevel").GetComponent<Text>();
        textEnchantLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_ENCHANTLEVEL"), nEnchantLevel);
        textEnchantLevel.color = new Color32(175, 213, 122, 255);

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csMainGear.MainGearGrade.Grade.ToString("00"));

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainGear(Transform trSlot, CsMainGear csMainGear, int nEnchantLevel, int nBattlePower, bool bOwned, EnItemSlotSize itemSlotSize)
    {
        InitializeItemSlot(trSlot);

        Transform trGear = trSlot.Find("Gear");
        trGear.gameObject.SetActive(true);

        Transform trItem = trSlot.Find("Item");
        trItem.gameObject.SetActive(false);

        //귀속여부
        trSlot.Find("ImageOwn").gameObject.SetActive(bOwned);

        //아이템아이콘
        Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csMainGear.Image);

        /*
        //인챈트레벨
        Image imageEnchantLevel = trGear.Find("ImageEnchantLevel").GetComponent<Image>();
        imageEnchantLevel.gameObject.SetActive(true);
        DisplayMainGearEnchantLevlImage(imageEnchantLevel, csHeroMainGear.EnchantLevel);
        */

        Text textEnchantLevel = trGear.Find("TextLevel").GetComponent<Text>();
        textEnchantLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_ENCHANTLEVEL"), nEnchantLevel);
        textEnchantLevel.color = new Color32(175, 213, 122, 255);

        //전투력에 의한 화살표 표시
        Image imageBattlePower = trGear.Find("ImageBattlePower").GetComponent<Image>();
        CsHeroMainGear csHeroMainGear = null;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i] != null)
            {
                if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].MainGear.MainGearType.EnMainGearType == csMainGear.MainGearType.EnMainGearType)
                {
                    csHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i];
                    break;
                }
            }
        }

        if (csHeroMainGear != null)
        {
            if (nBattlePower > csHeroMainGear.BattlePower)
            {
                imageBattlePower.gameObject.SetActive(true);
                imageBattlePower.sprite = m_spriteUpArrow;
            }
            else if (nBattlePower == csHeroMainGear.BattlePower)
            {
                imageBattlePower.gameObject.SetActive(false);
            }
            else
            {
                imageBattlePower.gameObject.SetActive(true);
                imageBattlePower.sprite = m_spriteDownArrow;
            }
        }
        else
        {
            if (nBattlePower == 0)
            {
                imageBattlePower.gameObject.SetActive(false);
            }
            else
            {
                imageBattlePower.gameObject.SetActive(true);
                imageBattlePower.sprite = m_spriteUpArrow;
            }
        }

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csMainGear.MainGearGrade.Grade.ToString("00"));

        //쿨타임
        //Image imageCooltime = trSlot.Find("ImageCooltime").GetComponent<Image>();
        //imageCooltime.fillAmount = 0;

        //착용불가 빨간 프레임
        Transform trRed = trSlot.Find("ImageRed");

        if (csMainGear.MainGearTier.RequiredHeroLevel > CsGameData.Instance.MyHeroInfo.Level)
        {
            trRed.gameObject.SetActive(true);
        }
        else
        {
            if (csMainGear.Job != null)
            {
                int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

                if (csMainGear.Job.JobId == nJobId)
                {
                    trRed.gameObject.SetActive(false);
                }
                else
                {
                    trRed.gameObject.SetActive(true);
                }
            }
            else
            {
                trRed.gameObject.SetActive(false);
            }
        }

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    //보조장비용
    public void DisplayItemSlot(Transform trSlot, CsHeroSubGear csHeroSubGear, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplaySubGear(trSlot, csHeroSubGear.SubGear, csHeroSubGear.SubGearLevel.SubGearGrade.Grade, csHeroSubGear.Level, itemSlotSize);

        Transform trGear = trSlot.Find("Gear");

        //전투력에 의한 화살표 표시
        Image imageBattlePower = trGear.Find("ImageBattlePower").GetComponent<Image>();

        if (csHeroSubGear.Equipped)
        {
            imageBattlePower.gameObject.SetActive(false);
        }
        else
        {
            imageBattlePower.gameObject.SetActive(true);
            imageBattlePower.sprite = m_spriteUpArrow;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsSubGear csSubGear, int nSubGearGrade, int nSubGearLevel, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplaySubGear(trSlot, csSubGear, nSubGearGrade, nSubGearLevel, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySubGear(Transform trSlot, CsSubGear csSubGear, int nSubGearGrade, int nSubGearLevel, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        InitializeItemSlot(trSlot);

        Transform trGear = trSlot.Find("Gear");
        trGear.gameObject.SetActive(true);

        Transform trItem = trSlot.Find("Item");
        trItem.gameObject.SetActive(false);

        //귀속여부
        trSlot.Find("ImageOwn").gameObject.SetActive(true);

        //아이템아이콘
        Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + string.Format("sub_{0}_{1}", csSubGear.SubGearId, nSubGearGrade));

        Text textEnchantLevel = trGear.Find("TextLevel").GetComponent<Text>();
        textEnchantLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TXT_LEVEL"), nSubGearLevel);
        textEnchantLevel.color = new Color32(144, 167, 242, 255);

        //전투력에 의한 화살표 표시
        Transform imageBattlePower = trGear.Find("ImageBattlePower");
        imageBattlePower.gameObject.SetActive(false);

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + nSubGearGrade.ToString("00"));

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //탈것장비

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsHeroMountGear csHeroMountGear, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplayMountGear(trSlot, csHeroMountGear.MountGear, csHeroMountGear.Owned, itemSlotSize);

        Image imageBattlePower = trSlot.Find("Gear/ImageBattlePower").GetComponent<Image>();
        imageBattlePower.gameObject.SetActive(true);
        imageBattlePower.sprite = m_spriteUpArrow;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.EquippedMountGearList.Count; i++)
        {
            CsHeroMountGear csHeroMountGearEqquip = CsGameData.Instance.MyHeroInfo.GetHeroMountGear(CsGameData.Instance.MyHeroInfo.EquippedMountGearList[i]);

            if (csHeroMountGearEqquip != null && csHeroMountGearEqquip.MountGear.MountGearType.SlotIndex == csHeroMountGear.MountGear.MountGearType.SlotIndex)
            {
                if (csHeroMountGearEqquip.BattlePower < csHeroMountGear.BattlePower)
                {
                    imageBattlePower.sprite = m_spriteUpArrow;

                }
                else if (csHeroMountGearEqquip.BattlePower == csHeroMountGear.BattlePower)
                {
                    imageBattlePower.gameObject.SetActive(false);
                }
                else
                {
                    imageBattlePower.sprite = m_spriteDownArrow;
                }

                break;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsMountGear csMountGear, bool bOwned, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplayMountGear(trSlot, csMountGear, bOwned, itemSlotSize);

        //전투력에 의한 화살표 표시
        Image imageBattlePower = trSlot.Find("Gear/ImageBattlePower").GetComponent<Image>();
        imageBattlePower.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountGear(Transform trSlot, CsMountGear csMountGear, bool bOwned, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        InitializeItemSlot(trSlot);

        Transform trGear = trSlot.Find("Gear");
        trGear.gameObject.SetActive(true);

        Transform trItem = trSlot.Find("Item");
        trItem.gameObject.SetActive(false);

        //귀속여부
        trSlot.Find("ImageOwn").gameObject.SetActive(bOwned);

        //아이템아이콘
        Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csMountGear.ImageName);

        Text textEnchantLevel = trGear.Find("TextLevel").GetComponent<Text>();
        textEnchantLevel.text = "";

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csMountGear.MountGearGrade.Grade.ToString("00"));

        // 착용 불가
        Transform trRed = trSlot.Find("ImageRed");

        if (CsGameData.Instance.MyHeroInfo.HeroMountList.Count > 0)
        {
            if (CsGameData.Instance.MyHeroInfo.Level < csMountGear.RequiredHeroLevel)
            {
                trRed.gameObject.SetActive(true);
            }
            else
            {
                trRed.gameObject.SetActive(false);
            }
        }
        else
        {
            trRed.gameObject.SetActive(false);
        }

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsInventoryObjectItem csInventoryObjectItem, bool bIsMine = true, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium, bool bDisplayRecommend = true)
    {
        DisplayItem(trSlot, csInventoryObjectItem.Item, csInventoryObjectItem.Owned, csInventoryObjectItem.Count, bIsMine, itemSlotSize, bDisplayRecommend);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsItem csItem, bool bOwned, int nCount, bool bIsMine = true, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium, bool bDisplayRecommend = true)
    {
        DisplayItem(trSlot, csItem, bOwned, nCount, bIsMine, itemSlotSize, bDisplayRecommend);
    }

	//---------------------------------------------------------------------------------------------------
	public void DisplayMount(Transform trSlot, CsMount csMount, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
	{
		InitializeItemSlot(trSlot);

		Transform trGear = trSlot.Find("Gear");
		trGear.gameObject.SetActive(false);

		Transform trItem = trSlot.Find("Item");
		trItem.gameObject.SetActive(true);

		//귀속여부
		trSlot.Find("ImageOwn").gameObject.SetActive(false);

		//아이템아이콘
		Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
		imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Common/mount_" + csMount.MountId.ToString() + "_1");

		//수량
		Text textCount = trItem.Find("TextCount").GetComponent<Text>();
		textCount.text = "";

		Transform trRed = trSlot.Find("ImageRed");
		trRed.gameObject.SetActive(false);

		//권장사용여부
		Transform trNotice = trItem.Find("ImageNotice");
		trNotice.gameObject.SetActive(false);


		//랭크테두리
		Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
		imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank01");

		DisplayItemSlotSize(trSlot, itemSlotSize);
	}

	//---------------------------------------------------------------------------------------------------
	public void DisplayCreatureCard(Transform trSlot, CsCreatureCard csCreatureCard, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
	{
		InitializeItemSlot(trSlot);

		Transform trGear = trSlot.Find("Gear");
		trGear.gameObject.SetActive(false);

		Transform trItem = trSlot.Find("Item");
		trItem.gameObject.SetActive(true);

		//귀속여부
		trSlot.Find("ImageOwn").gameObject.SetActive(false);

		//아이템아이콘
		Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
		imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Common/ico_card_" + csCreatureCard.CreatureCardGrade.Grade.ToString());

		//수량
		Text textCount = trItem.Find("TextCount").GetComponent<Text>();
		textCount.text = "";

		Transform trRed = trSlot.Find("ImageRed");
		trRed.gameObject.SetActive(false);

		//권장사용여부
		Transform trNotice = trItem.Find("ImageNotice");
		trNotice.gameObject.SetActive(false);


		//랭크테두리
		Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
		imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank01");

		DisplayItemSlotSize(trSlot, itemSlotSize);
	}

    //---------------------------------------------------------------------------------------------------
    void DisplayItem(Transform trSlot, CsItem csItem, bool bOwned, int nCount, bool bIsMine, EnItemSlotSize itemSlotSize, bool bDisplayRecommend)
    {
        InitializeItemSlot(trSlot);

        Transform trGear = trSlot.Find("Gear");
        trGear.gameObject.SetActive(false);

        Transform trItem = trSlot.Find("Item");
        trItem.gameObject.SetActive(true);

        //귀속여부
        trSlot.Find("ImageOwn").gameObject.SetActive(bOwned);

        //아이템아이콘
        Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();

        switch (csItem.ItemType.EnItemType)
        {
            case EnItemType.Title:
                imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/item_" + csItem.ItemType.ItemType);
                break;

            case EnItemType.Wing:
                imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/wing_" + csItem.Value1);
                break;

            default:
                imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csItem.Image);
                break;
        }

        //수량
        Text textCount = trItem.Find("TextCount").GetComponent<Text>();
        textCount.text = nCount.ToString();
        textCount.color = new Color32(222, 222, 222, 255);

        if (nCount == 0)
        {
            textCount.text = "";
        }

        Transform trRed = trSlot.Find("ImageRed");

        switch (csItem.UsingType)
        {
            case EnUsingType.OnlyOne:
            case EnUsingType.Multiple:

                if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= csItem.RequiredMaxHeroLevel)
                {
                    trRed.gameObject.SetActive(false);
                }
                else
                {
                    trRed.gameObject.SetActive(true);
                }

                break;

            case EnUsingType.NotAvailable:
                trRed.gameObject.SetActive(false);
                break;
        }

        //권장사용여부
        Transform trNotice = trItem.Find("ImageNotice");

		if (bDisplayRecommend)
		{
			if (csItem.RequiredMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.MyHeroInfo.Level <= csItem.RequiredMaxHeroLevel)
			{
				if (bIsMine)
				{
					if (csItem.ItemType.ItemType == (int)EnItemType.ExpPotion)
					{
						if (CsGameData.Instance.MyHeroInfo.ExpPotionDailyUseCount == CsGameData.Instance.MyHeroInfo.VipLevel.ExpPotionUseMaxCount)
						{
							trNotice.gameObject.SetActive(false);
						}
						else
						{
							trNotice.gameObject.SetActive(csItem.UsingRecommendationEnabled);
						}
					}
					else
					{
						trNotice.gameObject.SetActive(csItem.UsingRecommendationEnabled);
					}
				}
				else
				{
					trNotice.gameObject.SetActive(false);
				}
			}
			else
			{
				trNotice.gameObject.SetActive(false);
			}
		}
		else
		{
			trNotice.gameObject.SetActive(false);
		}

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csItem.Grade.ToString("00"));

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItemSlot(Transform trSlot, CsCreatureCard csCreatureCard, bool bDim, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        DisplayCard(trSlot, csCreatureCard, bDim);
    }

	//---------------------------------------------------------------------------------------------------
	public void DisplaySmallItemSlot(Transform trSlot, CsItem csItem, bool bOwned, int nCount)
	{
		Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
		imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csItem.Grade.ToString("00"));

		//귀속여부
		trSlot.Find("ImageOwn").gameObject.SetActive(bOwned);

		//아이템아이콘
		Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
		imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csItem.Image);

		//수량
		Text textCount = trSlot.Find("Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textCount);
		textCount.text = nCount.ToString();
		textCount.color = new Color32(222, 222, 222, 255);

		if (nCount == 0)
		{
			textCount.text = "";
		}

		if (csItem != null)
		{
			Button button = trSlot.GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => OnClickItemInfo(csItem));
			button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 크리쳐
	public void DisplayCreature(Transform trSlot, CsCreature csCreature)
	{
		InitializeItemSlot(trSlot);

		Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
		imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csCreature.CreatureGrade.Grade.ToString("00"));

		//귀속여부
		trSlot.Find("ImageOwn").gameObject.SetActive(false);

		//아이템아이콘
		Image imageItemIcon = trSlot.Find("ImageIcon").GetComponent<Image>();
		imageItemIcon.sprite = LoadAsset<Sprite>("GUI/Items/" + csCreature.CreatureCharacter.ImageName);

		trSlot.Find("Gear").gameObject.SetActive(false);
		trSlot.Find("Item").gameObject.SetActive(false);
		trSlot.Find("ImageCard").gameObject.SetActive(false);
		trSlot.Find("ImageRed").gameObject.SetActive(false);
		trSlot.Find("ImageCooltime").gameObject.SetActive(false);
		trSlot.Find("ImageCheck").gameObject.SetActive(false);
		trSlot.Find("ImageDim").gameObject.SetActive(false);
	}

    //---------------------------------------------------------------------------------------------------
    void DisplayCard(Transform trSlot, CsCreatureCard csCreatureCard, bool bDim, EnItemSlotSize itemSlotSize = EnItemSlotSize.Medium)
    {
        InitializeItemSlot(trSlot);

        trSlot.Find("Gear").gameObject.SetActive(false);
        trSlot.Find("Item").gameObject.SetActive(false);
        trSlot.Find("ImageOwn").gameObject.SetActive(false);
        trSlot.Find("ImageIcon").gameObject.SetActive(false);

        Transform trDim = trSlot.Find("ImageCooltime");

        if (bDim)
        {
            trDim.gameObject.SetActive(true);
            trDim.GetComponent<Image>().fillAmount = 1;
        }
        else
        {
            trDim.gameObject.SetActive(false);
        }


        Image imageCard = trSlot.Find("ImageCard").GetComponent<Image>();
        imageCard.sprite = LoadAsset<Sprite>("GUI/Card/card_" + csCreatureCard.CreatureCardId);
        imageCard.gameObject.SetActive(true);

        //랭크테두리
        Image imageFrameRank = trSlot.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csCreatureCard.CreatureCardGrade.Grade.ToString("00"));

        DisplayItemSlotSize(trSlot, itemSlotSize);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayItemSlotSize(Transform trSlot, EnItemSlotSize itemSlotSize)
    {
        switch (itemSlotSize)
        {
            case EnItemSlotSize.Small:
                trSlot.transform.localScale = new Vector3(0.64f, 0.64f, 0.64f);
                break;
            case EnItemSlotSize.Medium:
                trSlot.transform.localScale = new Vector3(1, 1, 1);
                break;
            case EnItemSlotSize.Large:
                trSlot.transform.localScale = new Vector3(1.27f, 1.27f, 1.27f);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainGearEnchantLevlImage(Image imageEnchantLevel, int nEnChantLevel)                                 //메인장비 인챈트 레벨에 따른 이미지 변경
    {
        if (nEnChantLevel < 13)
        {
            imageEnchantLevel.sprite = LoadAsset<Sprite>("GUI/Common/ico_star1_equip_on");
        }
        else if (nEnChantLevel < 25)
        {
            imageEnchantLevel.sprite = LoadAsset<Sprite>("GUI/Common/ico_star2_equip_on");
        }
        else if (nEnChantLevel < 37)
        {
            imageEnchantLevel.sprite = LoadAsset<Sprite>("GUI/Common/ico_star3_equip_on");
        }
        else
        {
            imageEnchantLevel.sprite = LoadAsset<Sprite>("GUI/Common/ico_star4_equip_on");
        }
    }

    #endregion 아이템 슬롯 디스플레이 함수

    /*
    #region Auto

    //---------------------------------------------------------------------------------------------------
    //AutoManager Auto플레이가 시작되면 기존에 실행중이던 이전 Auto플레이를 취소한다.
    public void AutoManager(EnAutoStateType enAutoStateType)
    {
        switch (CsUIData.Instance.AutoStateType)
        {
            case EnAutoStateType.Move:
                CsGameEventUIToUI.Instance.OnEventEndAutoMove();
                break;
            case EnAutoStateType.MainQuest:
                CsGameEventUIToUI.Instance.OnEventEndAutoMainQuest();
                break;
            case EnAutoStateType.Battle:
                CsGameEventUIToUI.Instance.OnEventEndAutoBattle();
                break;
        }

        CsUIData.Instance.AutoStateType = enAutoStateType;
    }

    //---------------------------------------------------------------------------------------------------
    //인게임에서 Auto캔슬이 올때
    public void AutoStopByIngame(EnAutoStateType enAutoStateType)
    {
        switch (enAutoStateType)
        {
            case EnAutoStateType.Move:
                CsGameEventUIToUI.Instance.OnEventEndAutoMove();
                break;
            case EnAutoStateType.MainQuest:
                CsGameEventUIToUI.Instance.OnEventEndAutoMainQuest();
                break;
            case EnAutoStateType.Battle:
                CsGameEventUIToUI.Instance.OnEventEndAutoBattle();
                break;
        }

        CsUIData.Instance.AutoStateType = EnAutoStateType.None;
    }

    #endregion
    
    */

    //---------------------------------------------------------------------------------------------------
    public void UpdateEnchantLevelIcon(Transform trEnchantLevelIconList, int nEnchantLevel)
    {
        int nStep = (nEnchantLevel / 12) + 1;
        int nLevelInstep = nEnchantLevel % 12;

        int nIconCount = 0;
        bool bLastOn = true;

        if (nLevelInstep % 2 == 0)
        {
            if (nLevelInstep != 0)
            {
                nIconCount = nLevelInstep / 2;

            }
            else
            {
                if (nEnchantLevel != 0)
                {
                    nIconCount = 6;
                    nStep = (nEnchantLevel / 12);
                }
            }

            bLastOn = true;
        }
        else
        {
            nIconCount = (nLevelInstep / 2) + 1;
            bLastOn = false;
        }

        for (int i = 0; i < nIconCount; i++)
        {
            Image imageEnchantIcon = trEnchantLevelIconList.GetChild(i).GetComponent<Image>();
            imageEnchantIcon.gameObject.SetActive(true);

            if (i == nIconCount - 1)
            {
                if (bLastOn)
                {
                    imageEnchantIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_star" + nStep + "_equip_on");
                }
                else
                {
                    imageEnchantIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_star" + nStep + "_equip_off");
                }
            }
            else
            {
                imageEnchantIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_star" + nStep + "_equip_on");
            }
        }

        for (int i = 0; i < trEnchantLevelIconList.childCount - nIconCount; i++)
        {
            trEnchantLevelIconList.GetChild(i + nIconCount).gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayButtonInteractable(Button button, bool bIsOn)
    {
        button.interactable = bIsOn;
        Text textButton = button.transform.Find("Text").GetComponent<Text>();

        if (bIsOn)
        {
            textButton.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            textButton.color = new Color32(133, 141, 148, 255);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public bool MenuOpen(int nMenuId)
    {
        CsMenu csMenu = CsGameData.Instance.GetMenu(nMenuId);

        if (csMenu != null)
        {
            return MenuOpen(csMenu);
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public bool MenuOpen(CsMenu csMenu)
    {
        if (csMenu.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > csMenu.RequiredMainQuestNo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public bool MenuContentOpen(int nMenuContentId)
    {
        CsMenuContent csMenuContent = CsGameData.Instance.GetMenuContent(nMenuContentId);

        if (csMenuContent != null)
        {
            return MenuContentOpen(csMenuContent);
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public bool MenuContentOpen(CsMenuContent csMenuContent)
    {
        if (csMenuContent.RequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > csMenuContent.RequiredMainQuestNo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void LoadUISound()
    {
		m_dicAudioClip.Clear();

		m_dicAudioClip.Add(EnUISoundType.Button, LoadAsset<AudioClip>("Sound/UI/SFX_ui_button_01"));
        m_dicAudioClip.Add(EnUISoundType.ToggleGroup, LoadAsset<AudioClip>("Sound/UI/SFX_ui_button_02"));
        m_dicAudioClip.Add(EnUISoundType.Toggle, LoadAsset<AudioClip>("Sound/UI/SFX_ui_toggle_01"));
        m_dicAudioClip.Add(EnUISoundType.MaingearEnchanting, LoadAsset<AudioClip>("Sound/UI/SFX_ui_maingear_enchanting"));
        m_dicAudioClip.Add(EnUISoundType.MaingearReinforceFail, LoadAsset<AudioClip>("Sound/UI/SFX_ui_maingear_reinforce_fail"));
        m_dicAudioClip.Add(EnUISoundType.MaingearReinforceSuccess, LoadAsset<AudioClip>("Sound/UI/SFX_ui_maingear_reinforce_success"));
        m_dicAudioClip.Add(EnUISoundType.MaingearTransition, LoadAsset<AudioClip>("Sound/UI/SFX_ui_maingear_transition"));
        m_dicAudioClip.Add(EnUISoundType.SkillLevelup, LoadAsset<AudioClip>("Sound/UI/SFX_ui_skill_levelup"));
        m_dicAudioClip.Add(EnUISoundType.SubgearRuneEquip, LoadAsset<AudioClip>("Sound/UI/SFX_ui_subgear_rune_epuip"));
        m_dicAudioClip.Add(EnUISoundType.SubgearRuneUnEquip, LoadAsset<AudioClip>("Sound/UI/SFX_ui_subgear_rune_separation"));
        m_dicAudioClip.Add(EnUISoundType.SubgearSoulstoneEquip, LoadAsset<AudioClip>("Sound/UI/SFX_ui_subgear_soulstone_epuip"));
        m_dicAudioClip.Add(EnUISoundType.SubgearSoulstoneUnEquip, LoadAsset<AudioClip>("Sound/UI/SFX_ui_subgear_soulstone_separation"));
        m_dicAudioClip.Add(EnUISoundType.SubgearUpgrade, LoadAsset<AudioClip>("Sound/UI/SFX_ui_subgear_upgrade"));
        m_dicAudioClip.Add(EnUISoundType.BattlePowerUp, LoadAsset<AudioClip>("Sound/UI/SFX_ui_powerup"));
        m_dicAudioClip.Add(EnUISoundType.BattlePowerDown, LoadAsset<AudioClip>("Sound/UI/SFX_ui_powerdown"));
        m_dicAudioClip.Add(EnUISoundType.CharacterLevelUp, LoadAsset<AudioClip>("Sound/UI/SFX_Cha_LevelUp"));
        m_dicAudioClip.Add(EnUISoundType.PowerCounting, LoadAsset<AudioClip>("Sound/UI/SFX_ui_powercounting"));
        m_dicAudioClip.Add(EnUISoundType.Weapon, LoadAsset<AudioClip>("Sound/UI/SFX_ui_equip_weapon"));
        m_dicAudioClip.Add(EnUISoundType.Armor, LoadAsset<AudioClip>("Sound/UI/SFX_ui_equip_armor"));
        m_dicAudioClip.Add(EnUISoundType.Ring, LoadAsset<AudioClip>("Sound/UI/SFX_ui_equip_ring"));
        m_dicAudioClip.Add(EnUISoundType.Gold, LoadAsset<AudioClip>("Sound/UI/SFX_ui_acquisition_gold"));
        m_dicAudioClip.Add(EnUISoundType.Item, LoadAsset<AudioClip>("Sound/UI/SFX_ui_acquisition_item"));
        m_dicAudioClip.Add(EnUISoundType.ReleaseWeapon, LoadAsset<AudioClip>("Sound/UI/SFX_ui_release_weapon"));
        m_dicAudioClip.Add(EnUISoundType.ReleaseArmor, LoadAsset<AudioClip>("Sound/UI/SFX_ui_release_armor"));
        m_dicAudioClip.Add(EnUISoundType.ReleaseSubGear, LoadAsset<AudioClip>("Sound/UI/SFX_ui_release_ring"));
        m_dicAudioClip.Add(EnUISoundType.QuestComplete, LoadAsset<AudioClip>("Sound/UI/SFX_ui_quest_complete"));
    }

    //---------------------------------------------------------------------------------------------------
    public void SetAudio(bool bIntro)
    {
        m_audioSourceUI = new GameObject("UIAudio").AddComponent<AudioSource>();

        if (bIntro)
        {
            Transform trSceneIntro = GameObject.Find("SceneIntro").transform;

            if (trSceneIntro != null)
            {
                m_audioSourceUI = new GameObject("UIAudio").AddComponent<AudioSource>();
                m_audioSourceUI.transform.SetParent(trSceneIntro);
                m_audioSourceUI.playOnAwake = false;
                m_audioSourceUI.loop = false;
            }
            else
            {
                Debug.Log("AudioSource Create Fail !!!!!!!!!!!!!");
            }
        }
        else
        {
            Transform trSceneMainUI = GameObject.Find("SceneMainUI").transform;

            if (trSceneMainUI != null)
            {
                m_audioSourceUI = new GameObject("UIAudio").AddComponent<AudioSource>();
                m_audioSourceUI.transform.SetParent(trSceneMainUI);
                m_audioSourceUI.playOnAwake = false;
                m_audioSourceUI.loop = false;
            }
            else
            {
                Debug.Log("AudioSource Create Fail !!!!!!!!!!!!!");
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void PlayUISound(EnUISoundType enUISoundType)
    {
        if (m_audioSourceUI != null)
        {
            if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.EffectSound) == 1)
            {
                m_audioSourceUI.PlayOneShot(m_dicAudioClip[enUISoundType]);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void PlayTutorialSound(EnTutorialType enTutorialType, int nContentId, int nStep = 1)
    {
        m_audioSourceUI.Stop();
        string strLoadSound = "";

        switch (enTutorialType)
        {
            case EnTutorialType.OpenContents:
                strLoadSound = string.Format("mt{0}_{1}", nContentId, nStep);
                break;
            case EnTutorialType.FirstStart:
            case EnTutorialType.Attainment:
            case EnTutorialType.Cart:
            case EnTutorialType.MainGearEnchant:
                strLoadSound = string.Format("ct{0}_{1}", nContentId, nStep);
                break;
        }

        AudioClip audioClip = LoadAsset<AudioClip>("Sound/Tutorial/" + strLoadSound);
        if (audioClip != null)
        {
            if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.EffectSound) == 1)
            {
                m_audioSourceUI.PlayOneShot(audioClip);
            }
        }
    }

	//---------------------------------------------------------------------------------------------------
    public void StopSound()
    {
        m_audioSourceUI.Stop();
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickItemInfo(CsItem csItem)
	{
		CsGameEventUIToUI.Instance.OnEventOpenPopupItemInfo(csItem);
	}

	//---------------------------------------------------------------------------------------------------
	public void SetButton(Transform trButton, UnityEngine.Events.UnityAction action)
	{
		Button button = trButton.GetComponent<Button>();

		if (button != null)
		{
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => PlayUISound(EnUISoundType.Button));
			button.onClick.AddListener(action);
		}
		else
		{
			Debug.Log("Button component is null");
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void SetImage(Transform trImage, string strPath)
	{
		Image image = trImage.GetComponent<Image>();
		
		if (image != null)
		{
			image.sprite = LoadAsset<Sprite>(strPath);
		}
	}
}

