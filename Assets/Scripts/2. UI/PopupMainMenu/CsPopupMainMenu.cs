using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-06)
//---------------------------------------------------------------------------------------------------

public class CsPopupMainMenu : MonoBehaviour
{
    Transform m_trList;
    Transform m_trButtonList1;
    Transform m_trButtonList2;
    Transform m_trButtonList3;
    Transform m_trButtonList4;
    Transform m_trButtonList5;

    GameObject m_goButton;

    List<CsMenu> m_listCsMenuGroup1 = new List<CsMenu>();
    List<CsMenu> m_listCsMenuGroup2 = new List<CsMenu>();
    List<CsMenu> m_listCsMenuGroup3 = new List<CsMenu>();
    List<CsMenu> m_listCsMenuGroup4 = new List<CsMenu>();
    List<CsMenu> m_listCsMenuGroup5 = new List<CsMenu>();
    CsPanelAttainment m_csPanelAttainment;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMainButtonUpdate += OnEventMainButtonUpdate;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive += OnEventAttainmentRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        for (int i = 0; i < m_listCsMenuGroup1.Count; i++)
        {
            int nIndex = i;

            if (CsUIData.Instance.MenuOpen(m_listCsMenuGroup1[nIndex]))
            {
                Button buttonMenu = m_trButtonList1.Find("Button" + m_listCsMenuGroup1[i].MenuId).GetComponent<Button>();

                switch ((EnMenuId)m_listCsMenuGroup1[nIndex].MenuId)
                {
                    case EnMenuId.Dungeon:
                        if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId) == null)
                        {
                            buttonMenu.gameObject.SetActive(false);
                        }
                        else
                        {
                            buttonMenu.gameObject.SetActive(true);
                        }

                        break;
                    case EnMenuId.TodayTask:
                        if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId) == null)
                        {
                            buttonMenu.gameObject.SetActive(false);
                        }
                        else
                        {
                            buttonMenu.gameObject.SetActive(true);
                        }

                        break;
                }
            }
            else
            {
                continue;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMainButtonUpdate -= OnEventMainButtonUpdate;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive -= OnEventAttainmentRewardReceive;
    }

    #region EventHandler
    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        UpdateAllButtonLock();
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lacquiredExp)
    {
        UpdateAllButtonLock();
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAttainmentRewardReceive()
    {
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopupMainMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMenu(int nMenuId)
    {
        switch ((EnMenuId)nMenuId)
        {
			case EnMenuId.Retrieval:
				CsGameEventUIToUI.Instance.OnEventSinglePopupOpen((EnMenuId)nMenuId);
                break;

            case EnMenuId.Inventory:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.Inventory);
                break;

            case EnMenuId.Mail:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mail, EnSubMenu.Mail);
                break;

            case EnMenuId.Dungeon:
                if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
                {
                    if (CsGameData.Instance.MyHeroInfo.Nation.NationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
                    }
                    else
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_DUN_NOENT"));
                    }
                }
                break;

            case EnMenuId.TodayTask:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.TodayTask, EnSubMenu.TodayMission);
                break;
            case EnMenuId.Attaniment:
                m_csPanelAttainment.OpenPanelAttainment();
                break;

            case EnMenuId.Character:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Character, EnSubMenu.CharacterInfo);
                break;

            case EnMenuId.Skill:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Skill, EnSubMenu.Skill);
                break;

            case EnMenuId.Achievement:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Achievement, EnSubMenu.Accomplishment);
                break;

            case EnMenuId.Rank:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Class, EnSubMenu.Class);
                break;

            case EnMenuId.Collection:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Collection, EnSubMenu.CardCollection);
                break;

            case EnMenuId.MainGear:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.MainGear, EnSubMenu.MainGearEnchant);
                break;

            case EnMenuId.SubGear:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.SubGear, EnSubMenu.SubGearLevelUp);
                break;

            case EnMenuId.Mount:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Mount, EnSubMenu.MountLevelUp);
                break;

            case EnMenuId.Wing:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Wing, EnSubMenu.WingEnchant);
                break;

            case EnMenuId.Creature:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Creature, EnSubMenu.CreatureTraining);
                break;

            case EnMenuId.Soul:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Soul, EnSubMenu.Soul);
                break;

            case EnMenuId.Constellation:
				CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Constellation, EnSubMenu.Constellation);
                break;

            case EnMenuId.Ranking:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Ranking, EnSubMenu.RankingIndividual);
                break;

            case EnMenuId.Friend:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Friend, EnSubMenu.Friend);
                break;

            case EnMenuId.Guild:
                if (CsGuildManager.Instance.Guild != null)
                    CsGuildManager.Instance.SendGuildMemberTabInfo();
                else
                    CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Guild, EnSubMenu.GuildMember);
                break;

            case EnMenuId.Nation:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Nation, EnSubMenu.NationInfo);
                break;

            case EnMenuId.Shop:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
                break;

            case EnMenuId.Support:
                if (CsUIData.Instance.MenuOpen((int)EnMenuId.Support))
                {
                    if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.TodayMission))
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.TodayMission);
                    }
                    else if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.SeriesMisson))
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.SeriesMission);
                    }
                    else if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AccessReward))
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.AccessReward);
                    }
                    else if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.AttendReward))
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.AttendReward);
                    }
                    else if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.LevelUpReward))
                    {
                        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Support, EnSubMenu.LevelUpReward);
                    }
                }
                break;

            case EnMenuId.Setting:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Setting, EnSubMenu.Default);
                break;

            case EnMenuId.LuckyShop:
                CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.LuckyShop, EnSubMenu.LuckyShop);
                break;
        }

        ClosePopupMainMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        ClosePopupMainMenu();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainButtonUpdate(int nGroupNo, int nMenuId, bool bVisible)
    {
        UpdateMenuNotice(nGroupNo, nMenuId, bVisible);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas = GameObject.Find("Canvas").transform;
        m_csPanelAttainment = trCanvas.Find("PanelAttainment").GetComponent<CsPanelAttainment>();

        Transform trBack = transform.Find("ImageDim/ImageBackground");

        if (m_goButton == null)
        {
            m_goButton = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupMainMenu/Button");
        }

        //팝업종료버튼
        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopup);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trList = trBack.Find("List");

        m_listCsMenuGroup1 = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 1);
        m_listCsMenuGroup2 = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 2);
        m_listCsMenuGroup3 = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 3);
        m_listCsMenuGroup4 = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 4);
        m_listCsMenuGroup5 = CsGameData.Instance.MenuList.FindAll(a => a.MenuGroup == 5);

        //1번그룹
        m_trButtonList1 = m_trList.Find("ButtonList1");

        for (int i = 0; i < m_listCsMenuGroup1.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trButtonList1.Find("Button" + m_listCsMenuGroup1[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, m_trButtonList1);
                goButton.name = "Button" + m_listCsMenuGroup1[i].MenuId;
                trButton = goButton.transform;
            }

            Image image = trButton.Find("Image").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuGroup1[i].ImageName);

            Button buttonMenu = trButton.GetComponent<Button>();
            buttonMenu.onClick.RemoveAllListeners();
            buttonMenu.onClick.AddListener(() => OnClickMenu(m_listCsMenuGroup1[nIndex].MenuId));
            buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textName = trButton.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = m_listCsMenuGroup1[i].Name;
        }

        //2번그룹
        m_trButtonList2 = m_trList.Find("ButtonList2");

        for (int i = 0; i < m_listCsMenuGroup2.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trButtonList2.Find("Button" + m_listCsMenuGroup2[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, m_trButtonList2);
                goButton.name = "Button" + m_listCsMenuGroup2[i].MenuId;
                trButton = goButton.transform;
            }

            Image image = trButton.Find("Image").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuGroup2[i].ImageName);

            Button buttonMenu = trButton.GetComponent<Button>();
            buttonMenu.onClick.RemoveAllListeners();
            buttonMenu.onClick.AddListener(() => OnClickMenu(m_listCsMenuGroup2[nIndex].MenuId));
            buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textName = trButton.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = m_listCsMenuGroup2[i].Name;
        }

        //3번그룹
        m_trButtonList3 = m_trList.Find("ButtonList3");

        for (int i = 0; i < m_listCsMenuGroup3.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trButtonList3.Find("Button" + m_listCsMenuGroup3[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, m_trButtonList3);
                goButton.name = "Button" + m_listCsMenuGroup3[i].MenuId;
                trButton = goButton.transform;
            }

            Image image = trButton.Find("Image").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuGroup3[i].ImageName);

            Button buttonMenu = trButton.GetComponent<Button>();
            buttonMenu.onClick.RemoveAllListeners();
            buttonMenu.onClick.AddListener(() => OnClickMenu(m_listCsMenuGroup3[nIndex].MenuId));
            buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textName = trButton.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = m_listCsMenuGroup3[i].Name;
        }

        //4번 그룹
        m_trButtonList4 = m_trList.Find("ButtonList4");

        for (int i = 0; i < m_listCsMenuGroup4.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trButtonList4.Find("Button" + m_listCsMenuGroup4[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, m_trButtonList4);
                goButton.name = "Button" + m_listCsMenuGroup4[i].MenuId;
                trButton = goButton.transform;
            }

            Image image = trButton.Find("Image").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuGroup4[i].ImageName);

            Button buttonMenu = trButton.GetComponent<Button>();
            buttonMenu.onClick.RemoveAllListeners();
            buttonMenu.onClick.AddListener(() => OnClickMenu(m_listCsMenuGroup4[nIndex].MenuId));
            buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textName = trButton.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = m_listCsMenuGroup4[i].Name;
        }

        //5번 그룹
        m_trButtonList5 = m_trList.Find("ButtonList5");

        for (int i = 0; i < m_listCsMenuGroup5.Count; i++)
        {
            int nIndex = i;
            Transform trButton = m_trButtonList5.Find("Button" + m_listCsMenuGroup5[i].MenuId);

            if (trButton == null)
            {
                GameObject goButton = Instantiate(m_goButton, m_trButtonList5);
                goButton.name = "Button" + m_listCsMenuGroup5[i].MenuId;
                trButton = goButton.transform;
            }

            Image image = trButton.Find("Image").GetComponent<Image>();
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + m_listCsMenuGroup5[i].ImageName);

            Button buttonMenu = trButton.GetComponent<Button>();
            buttonMenu.onClick.RemoveAllListeners();
            buttonMenu.onClick.AddListener(() => OnClickMenu(m_listCsMenuGroup5[nIndex].MenuId));
            buttonMenu.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textName = trButton.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = m_listCsMenuGroup5[i].Name;
        }

        UpdateAllButtonLock();
        UpdateAttaniment();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttaniment()
    {
        Transform trButtonAttaniment = m_trButtonList1.Find("Button" + (int)EnMenuId.Attaniment);

        CsAttainmentEntry csAttainmentEntry = CsGameData.Instance.GetAttainmentEntry(CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo + 1);
        trButtonAttaniment.gameObject.SetActive(csAttainmentEntry == null ? false : true);
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupMainMenu()
    {
        this.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAllButtonLock()
    {
        for (int i = 0; i < m_listCsMenuGroup1.Count; i++)
        {
            Transform trButton = m_trButtonList1.Find("Button" + m_listCsMenuGroup1[i].MenuId);

            Button buttonMenu = trButton.GetComponent<Button>();

            if (CsGameData.Instance.MyHeroInfo.Level >= m_listCsMenuGroup1[i].RequiredHeroLevel)
            {
                if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > m_listCsMenuGroup1[i].RequiredMainQuestNo)
                {
                    buttonMenu.gameObject.SetActive(true);
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, true);
                }
                else
                {
                    buttonMenu.gameObject.SetActive(false);
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
                }
            }
            else
            {
                buttonMenu.gameObject.SetActive(false);
                CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
            }
        }

        //2번그룹
        for (int i = 0; i < m_listCsMenuGroup2.Count; i++)
        {
            Transform trButton = m_trButtonList2.Find("Button" + m_listCsMenuGroup2[i].MenuId);

            Button buttonMenu = trButton.GetComponent<Button>();

            if (CsGameData.Instance.MyHeroInfo.Level >= m_listCsMenuGroup2[i].RequiredHeroLevel)
            {
                if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > m_listCsMenuGroup2[i].RequiredMainQuestNo)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
            }
        }

        //3번그룹
        m_trButtonList3 = m_trList.Find("ButtonList3");

        for (int i = 0; i < m_listCsMenuGroup3.Count; i++)
        {
            Transform trButton = m_trButtonList3.Find("Button" + m_listCsMenuGroup3[i].MenuId);

            Button buttonMenu = trButton.GetComponent<Button>();
			
            if (CsGameData.Instance.MyHeroInfo.Level >= m_listCsMenuGroup3[i].RequiredHeroLevel)
            {
                if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > m_listCsMenuGroup3[i].RequiredMainQuestNo)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
            }
        }

        //4번 그룹
        m_trButtonList4 = m_trList.Find("ButtonList4");

        for (int i = 0; i < m_listCsMenuGroup4.Count; i++)
        {
            Transform trButton = m_trButtonList4.Find("Button" + m_listCsMenuGroup4[i].MenuId);

            Button buttonMenu = trButton.GetComponent<Button>();

            if (CsGameData.Instance.MyHeroInfo.Level >= m_listCsMenuGroup4[i].RequiredHeroLevel)
            {
                if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > m_listCsMenuGroup4[i].RequiredMainQuestNo)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
            }
        }


        //5번 그룹
        m_trButtonList5 = m_trList.Find("ButtonList5");

        for (int i = 0; i < m_listCsMenuGroup5.Count; i++)
        {
            Transform trButton = m_trButtonList5.Find("Button" + m_listCsMenuGroup5[i].MenuId);

            Button buttonMenu = trButton.GetComponent<Button>();

            if (CsGameData.Instance.MyHeroInfo.Level >= m_listCsMenuGroup5[i].RequiredHeroLevel)
            {
                if (CsMainQuestManager.Instance.MainQuest == null || CsMainQuestManager.Instance.MainQuest.MainQuestNo > m_listCsMenuGroup5[i].RequiredMainQuestNo)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, true);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
                }
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonMenu, false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNotice(int nMenuId, bool bIsOn)
    {
        CsMenu csMenu = CsGameData.Instance.GetMenu(nMenuId);

        Transform trNotice = null;

        switch (csMenu.MenuGroup)
        {
            case 1:
                trNotice = m_trButtonList1.Find("Button" + nMenuId + "/ImageNotice");
                break;

            case 2:
                trNotice = m_trButtonList2.Find("Button" + nMenuId + "/ImageNotice");
                break;

            case 3:
                trNotice = m_trButtonList3.Find("Button" + nMenuId + "/ImageNotice");
                break;
        }

        if (trNotice != null)
        {
            trNotice.gameObject.SetActive(bIsOn);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMenuNotice(int nGroupNo, int nMenuId, bool bIsOn)
    {
        Transform trButton = null;

        switch (nGroupNo)
        {
            case 1:
                trButton = m_trButtonList1.Find("Button" + nMenuId);
                break;

            case 2:
                trButton = m_trButtonList2.Find("Button" + nMenuId);
                break;

            case 3:
                trButton = m_trButtonList3.Find("Button" + nMenuId);
                break;

            case 4:
                trButton = m_trButtonList4.Find("Button" + nMenuId);
                break;

            case 5:
                trButton = m_trButtonList5.Find("Button" + nMenuId);
                break;

        }

        if (trButton != null)
        {
            Transform trNotice = trButton.Find("ImageNotice");
            trNotice.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            trNotice.gameObject.SetActive(bIsOn);
        }
    }

}
