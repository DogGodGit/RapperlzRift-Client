using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public enum EnRequiredConditionType
{
    HeroLevel = 1, 
    MainQuestNo = 2, 
}

public enum EnIndividualDungeonType
{
    Exp = 1,
    OsirisRoom = 2, 
    ArtifactRoom = 3,
    FieldOfHonor = 4,
    ProofOfValor = 5,
    WisdomTemple = 6,
    Gold = 7,
}

public enum EnPartyDungeonType
{
	FearAltar = 1,
    AncientRelic = 2,
    SoulCoveter = 3,
    DragonNest = 4, 
}

public enum EnTimeLimitDungeonType
{
    UndergroundMaze = 1,
	RuinsReclaim = 2,
    InfiniteWar = 3, 
    WarMemory = 4, 
    TradeShip = 5, 
    AnkouTomb =  6,
}

public enum EnEventDungeonType
{
	TeamBattlefield = 1,
}

public class CsDungeonCartegory : CsPopupSub
{
    Transform m_trDungeonInfo;
    Transform m_trContents;
    Transform m_trSubMenu2;
    Transform m_trArtifactRoomInfo;
    Transform m_trFieldOfHonorInfo;
    Transform m_trProofOfValorInfo;

    GameObject m_goFieldOfHonorInfo;
    GameObject m_goArtifactRoomInfo;
    GameObject m_goProofOfValorInfo;
	GameObject m_goRuinsReclaimInfo;
    GameObject m_goDungeonCartegory;
    GameObject m_goDungeonInfo;

    CsSubMenu m_csSubMenu;
    CsPopupDungeonInfo m_csPopupDungeonInfo;

    ScrollRect m_srView;

    //float m_flTime = 0;
    bool m_bIsFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trSubMenu2 = trCanvas2.Find("MainPopupSubMenu/SubMenu2");
        m_srView = transform.Find("Scroll View").GetComponent<ScrollRect>();

        CsGameEventUIToUI.Instance.EventGoBackDungeonCartegoryList += OnEventGoBackDungeonCartegoryList;
    }

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        UpdateCartegoryList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bIsFirst)
        {
            m_bIsFirst = false;
            return;
        }

        if (m_csSubMenu != m_iPopupMain.GetCurrentSubMenu())
        {
            m_trContents.gameObject.SetActive(true);
            UpdateCartegoryList();
            m_srView.horizontalNormalizedPosition = 0;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventGoBackDungeonCartegoryList -= OnEventGoBackDungeonCartegoryList;
		
        for (int i = 0; i < m_trSubMenu2.childCount; i++)
        {
            Destroy(m_trSubMenu2.GetChild(i).gameObject);
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventGoBackDungeonCartegoryList()
    {
        UpdateCartegoryList();
        m_trContents.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectDungeon(int nDungeonIndex)
    {
        ShortCutDungeonInfo(nDungeonIndex);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    public void ShortCutDungeonInfo(int nDungeonIndex)
    {
		EnDungeon enDungeon = EnDungeon.None;

        switch ((EnSubMenu)m_csSubMenu.SubMenuId)
        {
            case EnSubMenu.StoryDungeon:
				enDungeon = EnDungeon.StoryDungeon;
                break;

            case EnSubMenu.IndividualDungeon:

                switch ((EnIndividualDungeonType)nDungeonIndex)
                {
                    case EnIndividualDungeonType.Exp:
                    case EnIndividualDungeonType.OsirisRoom:
						enDungeon = EnDungeon.ExpDungeon;
                        break;
                    case EnIndividualDungeonType.ArtifactRoom:
						enDungeon = EnDungeon.ArtifactRoom;
                        break;

                    case EnIndividualDungeonType.FieldOfHonor:
						enDungeon = EnDungeon.FieldOfHonor;
                        break;
                    case EnIndividualDungeonType.ProofOfValor:
						enDungeon = EnDungeon.ProofOfValor;
                        break;
					case EnIndividualDungeonType.WisdomTemple:
						enDungeon = EnDungeon.WisdomTemple;
						break;

                    case EnIndividualDungeonType.Gold:
                        break;
                }
                break;

            case EnSubMenu.PartyDungeon:

                switch ((EnPartyDungeonType)nDungeonIndex)
                {
					case EnPartyDungeonType.FearAltar:
						enDungeon = EnDungeon.FearAltar;
						break;

                    case EnPartyDungeonType.AncientRelic:
						enDungeon = EnDungeon.AncientRelic;
                        break;

                    case EnPartyDungeonType.SoulCoveter:
						enDungeon = EnDungeon.SoulCoveter;
						break;

                    case EnPartyDungeonType.DragonNest:
						enDungeon = EnDungeon.DragonNest;
                        break;
                }
                break;

            case EnSubMenu.TimeLimitDungeon:
				
                switch ((EnTimeLimitDungeonType)nDungeonIndex)
                {
                    case EnTimeLimitDungeonType.UndergroundMaze:
						enDungeon = EnDungeon.UndergroundMaze;
                        break;

					case EnTimeLimitDungeonType.RuinsReclaim:
						enDungeon = EnDungeon.RuinsReclaim;
						break;

                    case EnTimeLimitDungeonType.InfiniteWar:
						enDungeon = EnDungeon.InfiniteWar;
                        break;

                    case EnTimeLimitDungeonType.WarMemory:
						enDungeon = EnDungeon.WarMemory;
                        break;

                    case EnTimeLimitDungeonType.TradeShip:
						enDungeon = EnDungeon.TradeShip;
                        break;

                    case EnTimeLimitDungeonType.AnkouTomb:
						enDungeon = EnDungeon.AnkouTomb;
                        break;
                }

                break;

			case EnSubMenu.EventDungeon:

				switch ((EnEventDungeonType)nDungeonIndex)
				{
					case EnEventDungeonType.TeamBattlefield:
						enDungeon = EnDungeon.TeamBattlefield;
						break;
				}

				break;
        }

		if (enDungeon != EnDungeon.None)
		{
			LoadAndOpenDungeonInfo(nDungeonIndex, enDungeon);
		}

        m_trContents.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
	void LoadAndOpenDungeonInfo(int nDungeonIndex, EnDungeon enDungeon)
	{
		if (m_goDungeonInfo == null)
		{
			StartCoroutine(LoadDungeonInfoCoroutine(() => OpenPopupDungeonInfo(nDungeonIndex), enDungeon));
		}
		else
		{
			OpenPopupDungeonInfo(nDungeonIndex);
		}
	}

	//---------------------------------------------------------------------------------------------------
    void OpenPopupDungeonInfo(int nDungeonIndex)
    {
        m_trDungeonInfo = m_trSubMenu2.Find("DungeonInfo");

        if (m_trDungeonInfo == null)
        {
            GameObject goDungeonInfo = Instantiate(m_goDungeonInfo, m_trSubMenu2);
            goDungeonInfo.name = "DungeonInfo";
            m_trDungeonInfo = goDungeonInfo.transform;
        }
        else
        {
            m_trDungeonInfo.gameObject.SetActive(true);
        }

        //이벤트연결 및 디스플레이 호출
        m_csPopupDungeonInfo = m_trDungeonInfo.gameObject.GetComponent<CsPopupDungeonInfo>();
        m_csPopupDungeonInfo.DisplaySelectDungeonCartegory(nDungeonIndex, m_csSubMenu);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadDungeonInfoCoroutine(UnityAction unityAction, EnDungeon enDungeon)
    {
        ResourceRequest resourceRequest = null;

        switch (enDungeon)
        {
            default:
                resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/PopupDungeonInfo");
                yield return resourceRequest;
                m_goDungeonInfo = (GameObject)resourceRequest.asset;
                break;

            case EnDungeon.ArtifactRoom:
                resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/ArtifactRoomInfo");
                yield return resourceRequest;
                m_goArtifactRoomInfo = (GameObject)resourceRequest.asset;
                break;

            case EnDungeon.FieldOfHonor:
                resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/FieldOfHonorInfo");
                yield return resourceRequest;
                m_goFieldOfHonorInfo = (GameObject)resourceRequest.asset;
                break;

            case EnDungeon.ProofOfValor:
                resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupDungeon/ProofOfValorInfo");
                yield return resourceRequest;
                m_goProofOfValorInfo = (GameObject)resourceRequest.asset;
                break;
        }

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupArtifactRoomInfo(int nDungeonIndex)
    {
        m_trArtifactRoomInfo = m_trSubMenu2.Find("ArtifactRoomInfo");

        if (m_trArtifactRoomInfo == null)
        {
            GameObject goDungeonInfo = Instantiate(m_goArtifactRoomInfo, m_trSubMenu2);
            goDungeonInfo.name = "ArtifactRoomInfo";
            m_trArtifactRoomInfo = goDungeonInfo.transform;
        }
        else
        {
            m_trArtifactRoomInfo.gameObject.SetActive(true);
        }

        //이벤트연결 및 디스플레이 호출
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupFieldOfHonorInfo(int nDungeonIndex)
    {
        m_trFieldOfHonorInfo = m_trSubMenu2.Find("FieldOfHonorInfo");

        if (m_trFieldOfHonorInfo == null)
        {
            GameObject goDungeonInfo = Instantiate(m_goFieldOfHonorInfo, m_trSubMenu2);
            goDungeonInfo.name = "FieldOfHonorInfo";
            m_trFieldOfHonorInfo = goDungeonInfo.transform;
        }
        else
        {
            m_trFieldOfHonorInfo.gameObject.SetActive(true);
        }

        //이벤트연결 및 디스플레이 호출
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupProofOfValorInfo(int nDungeonIndex)
    {
        m_trProofOfValorInfo = m_trSubMenu2.Find("ProofOfValorInfo");

        if (m_trProofOfValorInfo == null)
        {
            GameObject goDungeonInfo = Instantiate(m_goProofOfValorInfo, m_trSubMenu2);
            goDungeonInfo.name = "ProofOfValorInfo";
            m_trProofOfValorInfo = goDungeonInfo.transform;
        }
        else
        {
            m_trProofOfValorInfo.gameObject.SetActive(true);
        }

        //이벤트연결 및 디스플레이 호출
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCartegoryList()
    {
        m_csSubMenu = m_iPopupMain.GetCurrentSubMenu();
        //다끄고 필요한거 키게끔 만들기.

        m_trContents = transform.Find("Scroll View/Viewport/Content");

        for (int i = 0; i < m_trContents.childCount; i++)
        {
            m_trContents.GetChild(i).gameObject.SetActive(false);
        }

        switch ((EnSubMenu)m_csSubMenu.SubMenuId)
        {
            case EnSubMenu.StoryDungeon:

                for (int i = 0; i < CsGameData.Instance.StoryDungeonList.Count; i++)
                {
                    int nDungeonIndex = i;
                    CreateDungeonCartegory(nDungeonIndex);
                }

                break;
            case EnSubMenu.IndividualDungeon:
				foreach (var en in Enum.GetValues(typeof(EnIndividualDungeonType)).Cast<int>())
				{
                    if (en == (int)EnIndividualDungeonType.Gold)
                    {
                        continue;
                    }
                    else
                    {
                        CreateDungeonCartegory(en);
                    }
				}
                break;
            case EnSubMenu.PartyDungeon:
				foreach (var en in Enum.GetValues(typeof(EnPartyDungeonType)).Cast<int>())
				{
					CreateDungeonCartegory(en);
				}
                break;
            case EnSubMenu.TimeLimitDungeon:
				foreach (var en in Enum.GetValues(typeof(EnTimeLimitDungeonType)).Cast<int>())
				{
					CreateDungeonCartegory(en);
				}
                break;
			case EnSubMenu.EventDungeon:
				foreach (var en in Enum.GetValues(typeof(EnEventDungeonType)).Cast<int>())
				{
					CreateDungeonCartegory(en);
				}
				break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateDungeonCartegory(int nDungeonIndex)
    {
        if (m_goDungeonCartegory == null)
        {
            m_goDungeonCartegory = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupDungeon/ButtonDungeon");
        }

        Transform trDungeonCartegory = null;
        Transform trFrameDungeon;
        Transform trLock;
        Button buttonCartegory = null;
        Text textChapter;
        Text textDungeonName;
        Text textInfo;
        Text textEnterCount;
        Text textLock;

		if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.StoryDungeon)
		{
			trDungeonCartegory = m_trContents.Find("Story" + nDungeonIndex);

			if (trDungeonCartegory == null)
			{
				GameObject goDungeonCartegory = Instantiate(m_goDungeonCartegory, m_trContents);
				goDungeonCartegory.name = "Story" + nDungeonIndex;
				trDungeonCartegory = goDungeonCartegory.transform;
			}
			else
			{
				trDungeonCartegory.gameObject.SetActive(true);
			}

			buttonCartegory = trDungeonCartegory.GetComponent<Button>();

			trLock = trDungeonCartegory.Find("FrameLock");

			textLock = trLock.Find("TextLock").GetComponent<Text>();
			CsUIData.Instance.SetFont(textLock);

			if (CsGameData.Instance.StoryDungeonList[nDungeonIndex].RequiredHeroMinLevel <= CsGameData.Instance.MyHeroInfo.Level && CsGameData.Instance.StoryDungeonList[nDungeonIndex].RequiredHeroMaxLevel >= CsGameData.Instance.MyHeroInfo.Level)
			{
				trLock.gameObject.SetActive(false);
				buttonCartegory.interactable = true;
			}
			else if (CsGameData.Instance.StoryDungeonList[nDungeonIndex].RequiredHeroMinLevel > CsGameData.Instance.MyHeroInfo.Level)
			{
				trLock.gameObject.SetActive(true);
				buttonCartegory.interactable = false;
				textLock.text = string.Format(CsConfiguration.Instance.GetString("A17_BTN_00010"), CsGameData.Instance.StoryDungeonList[nDungeonIndex].RequiredHeroMinLevel);
			}
			else if (CsGameData.Instance.StoryDungeonList[nDungeonIndex].RequiredHeroMaxLevel < CsGameData.Instance.MyHeroInfo.Level)
			{
				trLock.gameObject.SetActive(true);
				buttonCartegory.interactable = false;
				textLock.text = CsConfiguration.Instance.GetString("A17_BTN_00011");
			}

			trFrameDungeon = trDungeonCartegory.Find("FrameDungeon");

			textChapter = trFrameDungeon.Find("TextChapter").GetComponent<Text>();
			CsUIData.Instance.SetFont(textChapter);
			textChapter.text = string.Format(CsConfiguration.Instance.GetString("A17_BTN_00001"), CsGameData.Instance.StoryDungeonList[nDungeonIndex].DungeonNo);
			textChapter.gameObject.SetActive(true);

			textDungeonName = trFrameDungeon.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textDungeonName);
			textDungeonName.text = CsGameData.Instance.StoryDungeonList[nDungeonIndex].Name;

			textInfo = trFrameDungeon.Find("TextInfo").GetComponent<Text>();
			textInfo.gameObject.SetActive(false);

			textEnterCount = trFrameDungeon.Find("TextCount").GetComponent<Text>();
			CsUIData.Instance.SetFont(textEnterCount);
			textEnterCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.StoryDungeonList[nDungeonIndex].EnterCount - CsGameData.Instance.StoryDungeonList[nDungeonIndex].PlayCount, CsGameData.Instance.StoryDungeonList[nDungeonIndex].EnterCount);

		}
		else
		{
			string strContent = string.Empty;
            EnRequiredConditionType enRequiredConditionType = EnRequiredConditionType.HeroLevel;
            int nRequiredMainQuestNo = -1;
			int nRequiredHeroLevel = -1;
			string strDungeonName = string.Empty;
			string strBtnInfo = string.Empty;
			string strEnterCount = string.Empty;

			if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.IndividualDungeon)
			{
				switch ((EnIndividualDungeonType)nDungeonIndex)
					{
						case EnIndividualDungeonType.Exp:
							strContent = "Exp";

                            enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredConditionType;

                            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                            {
                                nRequiredHeroLevel = CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredHeroLevel;
                            }
                            else
                            {
                                nRequiredMainQuestNo = CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList[0].RequiredMainQuestNo;
                            }

							strDungeonName = CsGameData.Instance.ExpDungeon.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A32_BTN_00004");
							strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount - CsGameData.Instance.ExpDungeon.ExpDungeonDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.ExpDungeonEnterCount);
							break;

						case EnIndividualDungeonType.Gold:
							strContent = "Gold";
							nRequiredHeroLevel = CsGameData.Instance.GoldDungeon.GoldDungeonDifficultyList[0].RequiredHeroLevel;
							strDungeonName = CsGameData.Instance.GoldDungeon.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A39_BTN_00002");
							strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.GoldDungeonEnterCount - CsGameData.Instance.GoldDungeon.GoldDungeonDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.GoldDungeonEnterCount);
							break;

                        case EnIndividualDungeonType.OsirisRoom:
                            strContent = "OsirisRoom";

                            if (CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList.Count > 0)
                            {
                                enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList[0].RequiredConditionType;

                                if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                                {
                                    nRequiredHeroLevel = CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList[0].RequiredHeroLevel;
                                }
                                else
                                {
                                    nRequiredMainQuestNo = CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList[0].RequiredMainQuestNo;
                                }
                            }
                            else
                            {
                                nRequiredHeroLevel = 0;
                                nRequiredMainQuestNo = 0;
                            }

                            strDungeonName = CsGameData.Instance.OsirisRoom.Name;
                            strBtnInfo = CsConfiguration.Instance.GetString("A39_BTN_00002");
                            strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount - CsGameData.Instance.OsirisRoom.DailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.OsirisRoomEnterCount);
                            break;

						case EnIndividualDungeonType.ArtifactRoom:
							strContent = "ArtifactRoom";

                            enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.ArtifactRoom.RequiredConditionType;
                            
                            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                            {
                                nRequiredHeroLevel = CsGameData.Instance.ArtifactRoom.RequiredHeroLevel;   
                            }
                            else
                            {
                                nRequiredMainQuestNo = CsGameData.Instance.ArtifactRoom.RequiredMainQuestNo;
                            }

							strDungeonName = CsGameData.Instance.ArtifactRoom.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A47_BTN_00001");
							break;

						case EnIndividualDungeonType.FieldOfHonor:
							strContent = "FieldOfHonor";

                            enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.FieldOfHonor.RequiredConditionType;

                            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                            {
                                nRequiredHeroLevel = CsGameData.Instance.FieldOfHonor.RequiredHeroLevel;   
                            }
                            else
                            {
                                nRequiredMainQuestNo = CsGameData.Instance.FieldOfHonor.RequiredMainQuestNo;
                            }

							strDungeonName = CsGameData.Instance.FieldOfHonor.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A31_BTN_00006");
							strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.FieldOfHonorEnterCount - CsGameData.Instance.FieldOfHonor.FieldOfHonorDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.FieldOfHonorEnterCount);
							break;

						case EnIndividualDungeonType.ProofOfValor:
							strContent = "ProofOfValor";

                            enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.ProofOfValor.RequiredConditionType;

                            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                            {
                                nRequiredHeroLevel = CsGameData.Instance.ProofOfValor.RequiredHeroLevel;
                            }
                            else
                            {
                                nRequiredMainQuestNo = CsGameData.Instance.ProofOfValor.RequiredMainQuestNo;
                            }

							strDungeonName = CsGameData.Instance.ProofOfValor.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A89_TXT_00001");
							strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.ProofOfValorEnterCount - CsGameData.Instance.ProofOfValor.DailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.ProofOfValorEnterCount);
							break;

						case EnIndividualDungeonType.WisdomTemple:
							strContent = "WisdomTemple";

                            enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.WisdomTemple.RequiredConditionType;

                            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                            {
                                nRequiredHeroLevel = CsGameData.Instance.WisdomTemple.RequiredHeroLevel;
                            }
                            else
                            {
                                nRequiredMainQuestNo = CsGameData.Instance.WisdomTemple.RequiredMainQuestNo;
                            }

							strDungeonName = CsGameData.Instance.WisdomTemple.Name;
							strBtnInfo = CsConfiguration.Instance.GetString("A105_TXT_00001");
							strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, 1 - CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount), 1);
							break;
					}
			}
			else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.PartyDungeon)
			{
				switch ((EnPartyDungeonType)nDungeonIndex)
				{
					case EnPartyDungeonType.FearAltar:

						strContent = "FearAltar";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.FearAltar.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.FearAltar.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.FearAltar.RequiredMainQuestNo;
                        }

						strDungeonName = CsGameData.Instance.FearAltar.Name;
						strBtnInfo = CsConfiguration.Instance.GetString("A116_TXT_00001");
						strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount - CsGameData.Instance.FearAltar.DailyFearAltarPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.FearAltarEnterCount);
						
                        break;

					case EnPartyDungeonType.AncientRelic:

						strContent = "AncientRelic";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.AncientRelic.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.AncientRelic.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.AncientRelic.RequiredMainQuestNo;
                        }

						strDungeonName = CsGameData.Instance.AncientRelic.Name;
						strBtnInfo = CsConfiguration.Instance.GetString("A40_BTN_00003");
						strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount - CsGameData.Instance.AncientRelic.AncientRelicDailyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.AncientRelicEnterCount);
						
                        break;

					case EnPartyDungeonType.SoulCoveter:

						strContent = "SoulCoveter";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.SoulCoveter.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.SoulCoveter.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.SoulCoveter.RequiredMainQuestNo;
                        }

						strDungeonName = CsGameData.Instance.SoulCoveter.Name;
						strBtnInfo = CsConfiguration.Instance.GetString("A74_BTN_00001");
						strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount - CsGameData.Instance.SoulCoveter.SoulCoveterWeeklyPlayCount, CsGameData.Instance.MyHeroInfo.VipLevel.SoulCoveterWeeklyEnterCount);
						
                        break;

                    case EnPartyDungeonType.DragonNest:

                        strContent = "DragonNest";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.DragonNest.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.DragonNest.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.DragonNest.RequiredMainQuestNo;
                        }

                        strDungeonName = CsGameData.Instance.DragonNest.Name;
                        strBtnInfo = CsConfiguration.Instance.GetString("A144_TXT_00002");
                        strEnterCount = "";

                        break;
				}
			}
			else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.TimeLimitDungeon)
            {
                TimeSpan timeSpanCurrnt = CsGameData.Instance.MyHeroInfo.CurrentDateTime.Subtract(CsGameData.Instance.MyHeroInfo.CurrentDateTime.Date);
                int nCurrentSec = (int)timeSpanCurrnt.TotalSeconds;

                DateTime dtStartTime;
                DateTime dtEndTime;

				switch ((EnTimeLimitDungeonType)nDungeonIndex)
				{
					case EnTimeLimitDungeonType.UndergroundMaze:
						strContent = "UndergroundMaz";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.UndergroundMaze.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.UndergroundMaze.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.UndergroundMaze.RequiredMainQuestNo;
                        }

						strDungeonName = CsGameData.Instance.UndergroundMaze.Name;
						strBtnInfo = CsConfiguration.Instance.GetString("A43_BTN_00002");
                        
                        TimeSpan timeSpan;
                        
                        if (CsGameData.Instance.MyHeroInfo.GetHeroTaskConsignmentStartCount((int)EnTaskConsignment.UndergroundMaze) == null)
                        {
                            timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.UndergroundMaze.LimitTime - CsGameData.Instance.UndergroundMaze.UndergroundMazeDailyPlayTime);
                        }
                        else
                        {
                            timeSpan = TimeSpan.FromSeconds(0.0f);
                        }

						strEnterCount = string.Format(CsConfiguration.Instance.GetString("A43_BTN_00003"), (timeSpan.Minutes + timeSpan.Hours * 60).ToString("00"), timeSpan.Seconds.ToString("00"));
						break;

					case EnTimeLimitDungeonType.RuinsReclaim:
						strContent = "RuinsReclaim";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.RuinsReclaim.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.RuinsReclaim.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.RuinsReclaim.RequiredMainQuestNo;
                        }

						strDungeonName = CsGameData.Instance.RuinsReclaim.Name;

						CsRuinsReclaimOpenSchedule csRuinsReclaimOpenSchedule = null;

						foreach (var openSchedule in CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList)
						{
							if (nCurrentSec < openSchedule.EndTime)
							{
								csRuinsReclaimOpenSchedule = openSchedule;
								break;
							}
						}

						if (csRuinsReclaimOpenSchedule == null)
						{
							csRuinsReclaimOpenSchedule = CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList[0];
						}

						dtStartTime = new DateTime().AddSeconds(csRuinsReclaimOpenSchedule.StartTime);
						dtEndTime = new DateTime().AddSeconds(csRuinsReclaimOpenSchedule.EndTime);

						strBtnInfo = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00001"), dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));
						strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, CsGameData.Instance.RuinsReclaim.FreeEnterCount - CsGameData.Instance.RuinsReclaim.FreePlayCount), CsGameData.Instance.RuinsReclaim.FreeEnterCount);
						break;

                    case EnTimeLimitDungeonType.InfiniteWar:
                        strContent = "InfiniteWar";

                        enRequiredConditionType = (EnRequiredConditionType)CsGameData.Instance.InfiniteWar.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = CsGameData.Instance.InfiniteWar.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = CsGameData.Instance.InfiniteWar.RequiredMainQuestNo;
                        }

                        strDungeonName = CsGameData.Instance.InfiniteWar.Name;

                        CsInfiniteWarOpenSchedule csInfiniteWarOpenSchedule = null;

                        for (int i = 0; i < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Count; i++)
                        {
                            if (nCurrentSec < CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i].EndTime)
                            {
                                csInfiniteWarOpenSchedule = CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[i];
                                break;
                            }
                        }

                        if (csInfiniteWarOpenSchedule == null)
                        {
                            csInfiniteWarOpenSchedule = CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList[0];
                        }
                        
                        dtStartTime = new DateTime().AddSeconds(csInfiniteWarOpenSchedule.StartTime);
                        dtEndTime = new DateTime().AddSeconds(csInfiniteWarOpenSchedule.EndTime);

                        strBtnInfo = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00001"), dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));

                        strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, CsGameData.Instance.InfiniteWar.EnterCount - CsGameData.Instance.InfiniteWar.DailyPlayCount), CsGameData.Instance.InfiniteWar.EnterCount);
                        break;

                    case EnTimeLimitDungeonType.WarMemory:

                        strContent = "WarMemory";

                        CsWarMemory csWarMemory = CsGameData.Instance.WarMemory;

                        enRequiredConditionType = (EnRequiredConditionType)csWarMemory.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = csWarMemory.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = csWarMemory.RequiredMainQuestNo;
                        }

                        strDungeonName = csWarMemory.Name;

                        CsWarMemorySchedule csWarMemorySchedule = null;

                        for (int i = 0; i < csWarMemory.WarMemoryScheduleList.Count; i++)
                        {
                            if (nCurrentSec < csWarMemory.WarMemoryScheduleList[i].EndTime)
                            {
                                csWarMemorySchedule = csWarMemory.WarMemoryScheduleList[i];
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (csWarMemorySchedule == null && csWarMemory.WarMemoryScheduleList.Count > 0)
                        {
                            csWarMemorySchedule = csWarMemory.WarMemoryScheduleList[0];
                        }

                        dtStartTime = new DateTime().AddSeconds(csWarMemorySchedule.StartTime);
                        dtEndTime = new DateTime().AddSeconds(csWarMemorySchedule.EndTime);

                        strBtnInfo = string.Format(CsConfiguration.Instance.GetString("A110_TXT_00001"), dtStartTime.Hour.ToString("00"), dtStartTime.Minute.ToString("00"), dtEndTime.Hour.ToString("00"), dtEndTime.Minute.ToString("00"));
                        strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, CsGameData.Instance.WarMemory.FreeEnterCount - CsGameData.Instance.WarMemory.FreePlayCount), CsGameData.Instance.WarMemory.FreeEnterCount);
                        
                        break;

                    case EnTimeLimitDungeonType.TradeShip:

                        strContent = "TradeShip";

                        CsTradeShip csTradeShip = CsGameData.Instance.TradeShip;

                        enRequiredConditionType = (EnRequiredConditionType)csTradeShip.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = csTradeShip.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredHeroLevel = csTradeShip.RequiredMainQuestNo;
                        }

                        strDungeonName = csTradeShip.Name;

                        strBtnInfo = CsConfiguration.Instance.GetString("A162_TXT_00003");

                        strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount - csTradeShip.PlayCount), CsGameData.Instance.MyHeroInfo.VipLevel.TradeShipEnterCount);

                        break;

                    case EnTimeLimitDungeonType.AnkouTomb:

                        strContent = "AnkouTomb";

                        CsAnkouTomb csAnkouTomb = CsGameData.Instance.AnkouTomb;

                        enRequiredConditionType = (EnRequiredConditionType)csAnkouTomb.RequiredConditionType;

                        if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
                        {
                            nRequiredHeroLevel = csAnkouTomb.RequiredHeroLevel;
                        }
                        else
                        {
                            nRequiredMainQuestNo = csAnkouTomb.RequiredMainQuestNo;
                        }

                        strDungeonName = csAnkouTomb.Name;

                        strBtnInfo = CsConfiguration.Instance.GetString("A162_TXT_00001");
                        
                        strEnterCount = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), Math.Max(0, csAnkouTomb.EnterCount - csAnkouTomb.PlayCount), csAnkouTomb.EnterCount);

                        break;
				}
			}
			else if ((EnSubMenu)m_csSubMenu.SubMenuId == EnSubMenu.EventDungeon)
			{
				switch ((EnEventDungeonType)nDungeonIndex)
				{
					case EnEventDungeonType.TeamBattlefield:

						strContent = "TeamBattlefield";

						CsTeamBattlefield csTeamBattlefield = CsGameData.Instance.TeamBattlefield;

						enRequiredConditionType = (EnRequiredConditionType)csTeamBattlefield.RequiredConditionType;

						if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
						{
							nRequiredHeroLevel = csTeamBattlefield.RequiredConditionValue;
						}
						else
						{
							nRequiredMainQuestNo = csTeamBattlefield.RequiredConditionValue;
						}

						strDungeonName = csTeamBattlefield.Name;

						// %%% 설명 수정
						strBtnInfo = CsConfiguration.Instance.GetString("A162_TXT_00001");

						strEnterCount = "";

						break;
				}
			}

			trDungeonCartegory = m_trContents.Find(strContent);

			if (trDungeonCartegory == null)
			{
				GameObject goDungeonCartegory = Instantiate(m_goDungeonCartegory, m_trContents);
				goDungeonCartegory.name = strContent;
				trDungeonCartegory = goDungeonCartegory.transform;
			}
			else
			{
				trDungeonCartegory.gameObject.SetActive(true);
			}

			buttonCartegory = trDungeonCartegory.GetComponent<Button>();
			trLock = trDungeonCartegory.Find("FrameLock");
			textLock = trLock.Find("TextLock").GetComponent<Text>();

            if (enRequiredConditionType == EnRequiredConditionType.HeroLevel)
            {
                if (nRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
                {
                    //오픈
                    trLock.gameObject.SetActive(false);
                    buttonCartegory.interactable = true;
                }
                else
                {
                    //잠금
                    trLock.gameObject.SetActive(true);
                    buttonCartegory.interactable = false;
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A17_BTN_00010"), nRequiredHeroLevel);
                }
            }
            else
            {
                if ((CsMainQuestManager.Instance.MainQuest == null) || (nRequiredMainQuestNo <= CsMainQuestManager.Instance.MainQuest.MainQuestNo))
                {
                    //오픈
                    trLock.gameObject.SetActive(false);
                    buttonCartegory.interactable = true;
                }
                else
                {
                    //잠금
                    trLock.gameObject.SetActive(true);
                    buttonCartegory.interactable = false;
                    textLock.text = string.Format(CsConfiguration.Instance.GetString("A43_BTN_00006"), CsGameData.Instance.GetMainQuest(nRequiredMainQuestNo).Title);
                }
            }

			trFrameDungeon = trDungeonCartegory.Find("FrameDungeon");

			textChapter = trFrameDungeon.Find("TextChapter").GetComponent<Text>();
			textChapter.gameObject.SetActive(false);

			textDungeonName = trFrameDungeon.Find("TextName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textDungeonName);
			textDungeonName.text = strDungeonName;

			textInfo = trFrameDungeon.Find("TextInfo").GetComponent<Text>();
			CsUIData.Instance.SetFont(textInfo);
			textInfo.text = strBtnInfo;
			textInfo.gameObject.SetActive(true);

			textEnterCount = trFrameDungeon.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textEnterCount);
            textEnterCount.text = strEnterCount;
		}

        buttonCartegory.onClick.RemoveAllListeners();
        buttonCartegory.onClick.AddListener(() => OnClickSelectDungeon(nDungeonIndex));
        buttonCartegory.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        int DungoenNum = m_csSubMenu.SubMenuId - 1100;
        Image imageFrame = trDungeonCartegory.Find("ImageDungeon").GetComponent<Image>();
        imageFrame.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupDungeon/frm_dungeon_" + DungoenNum + "_" + (nDungeonIndex));
    }
}