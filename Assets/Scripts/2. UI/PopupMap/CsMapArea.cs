using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-29)
//---------------------------------------------------------------------------------------------------

public class CsMapArea : CsPopupSub
{
    [SerializeField] GameObject m_goMinimapText;
    [SerializeField] GameObject m_goMinimapSlot;
    [SerializeField] GameObject m_goMinimapIcon;
    [SerializeField] GameObject m_goImageMovePoint;
    // 국가전 몬스터
    [SerializeField] GameObject m_goMinimapNationWarMonsterIcon;

    Transform m_trAreaList;
    Transform m_trContent;
    Transform m_trIconList;
    Transform m_trEnableMoveList;
    Transform m_trDisableMoveList;

    Image m_imageMap;
    Image m_imagePlayer;
    Image m_imageLastPoint;

    ScrollRect m_scrollRectList;

    int m_nToggleNumber = 0;
    int m_nLocationId = 0;
    int m_nNationId = 0;

    float m_flMinimapMagnification = 0f;
    float m_flX = 0f;
    float m_flZ = 0f;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMiniMapSelected += OnEventMiniMapSelected;
        CsGameEventToUI.Instance.EventPortalEnter += OnEventPortalEnter;

        // 국가전 몬스터 배틀
        CsNationWarManager.Instance.EventNationWarMonsterBattleModeStart += OnEventNationWarMonsterBattleModeStart;
        CsNationWarManager.Instance.EventNationWarMonsterBattleModeEnd += OnEventNationWarMonsterBattleModeEnd;
        CsNationWarManager.Instance.EventNationWarMonsterDead += OnEventNationWarMonsterDead;

        CsNationWarManager.Instance.EventNationWarStart += OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished += OnEventNationWarFinished;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMiniMapSelected -= OnEventMiniMapSelected;
        CsGameEventToUI.Instance.EventPortalEnter -= OnEventPortalEnter;

        // 국가전 몬스터 배틀
        CsNationWarManager.Instance.EventNationWarMonsterBattleModeStart -= OnEventNationWarMonsterBattleModeStart;
        CsNationWarManager.Instance.EventNationWarMonsterBattleModeEnd -= OnEventNationWarMonsterBattleModeEnd;
        CsNationWarManager.Instance.EventNationWarMonsterDead -= OnEventNationWarMonsterDead;

        CsNationWarManager.Instance.EventNationWarStart -= OnEventNationWarStart;
        CsNationWarManager.Instance.EventNationWarFinished -= OnEventNationWarFinished;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        m_nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
        m_nNationId = CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam;
        AllPathClear();
        DisplayMap();
        DisplayList();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        Transform trPlayer = CsGameData.Instance.MyHeroTransform;

        if (trPlayer != null)
        {
            Vector3 vPlayer = trPlayer.position;
            m_imagePlayer.rectTransform.anchoredPosition = new Vector2((vPlayer.x - m_flX) * m_flMinimapMagnification, (vPlayer.z - m_flZ) * m_flMinimapMagnification);
            m_imagePlayer.rectTransform.eulerAngles = new Vector3(0, 0, 180 - CsGameData.Instance.MyHeroTransform.rotation.eulerAngles.y);
            UpdatePathPos();
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedList(bool bIson, int nListNumber, Text text)
    {
        if (bIson && m_nToggleNumber != nListNumber)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nToggleNumber = nListNumber;
            DisplayList();
            text.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            text.color = CsUIData.Instance.ColorGray;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMapMove(int nContinentId, int nNationId, Vector3 vtPosition)
    {
        AllPathClear();
        if (CsGameEventToIngame.Instance.OnEventMapMove(nContinentId, nNationId, vtPosition))
        {
            CsUIData.Instance.AutoStateType = EnAutoStateType.Move;
            CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPortalMove(int nContinentId, int nNationId, Vector3 vtPosition, int nPortalId)
    {
        AllPathClear();

        if (CsGameEventToIngame.Instance.OnEventMapMove(nContinentId, nNationId, vtPosition, nPortalId))
        {
            CsUIData.Instance.AutoStateType = EnAutoStateType.Move;
            CsGameEventUIToUI.Instance.OnEventAutoCancelButtonOpen(EnAutoStateType.Move);
        }
    }

    #endregion Event Handler

    #region Event
    //---------------------------------------------------------------------------------------------------
    void OnEventMiniMapSelected(int nLocationId)
    {
        m_nLocationId = nLocationId;
        DisplayMap();
        DisplayList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventPortalEnter(int nPortalId)
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMonsterBattleModeStart(int nArrangeId)
    {
        DisplyNationWarMonsterBattleIcon(nArrangeId, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMonsterBattleModeEnd(int nArrangeId)
    {
        DisplyNationWarMonsterBattleIcon(nArrangeId, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarMonsterDead(int nArrangeId)
    {
        DisplyNationWarMonsterBattleIcon(nArrangeId, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarStart(System.Guid guidDeclarationId)
    {
        CreateNationWarMonsterIcon();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationWarFinished(System.Guid guidDeclarationId, int nWinNationId)
    {
        for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
        {
            Transform trMapNationWarMonsterIcon = m_trIconList.Find("MapNationWarMonsterIcon" + CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].ArrangeId);

            if (trMapNationWarMonsterIcon != null)
            {
                Destroy(trMapNationWarMonsterIcon.gameObject);
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_imagePlayer = transform.Find("PanelMap/ImageBackGround/ImageMap/ImagePlayer").GetComponent<Image>();
        m_nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
        m_nNationId = CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam;
        m_imageMap = transform.Find("PanelMap/ImageBackGround/ImageMap").GetComponent<Image>();
        m_imageMap.GetComponent<CsMapLocation>().MapArea = this;
        m_trAreaList = transform.Find("PanelMap/ImageBackGround/AreaList");
        m_scrollRectList = transform.Find("PanelList/Scroll View").GetComponent<ScrollRect>();
        m_trContent = m_scrollRectList.transform.Find("Viewport/Content");
        m_trIconList = m_imageMap.transform.Find("IconList");
        m_trEnableMoveList = m_imageMap.transform.Find("EnableMoveList");
        m_trDisableMoveList = m_imageMap.transform.Find("DisableMoveList");
        m_imageLastPoint = m_imageMap.transform.Find("ImageLastPoint").GetComponent<Image>();

        Toggle toggleNpc = transform.Find("PanelList/ToggleList/ToggleNPC").GetComponent<Toggle>();
        Text textNpc = toggleNpc.transform.Find("Text").GetComponent<Text>();

        toggleNpc.onValueChanged.RemoveAllListeners();
        toggleNpc.isOn = true;
        toggleNpc.onValueChanged.AddListener((ison) => { OnValueChangedList(ison, 0, textNpc); });
        textNpc.text = CsConfiguration.Instance.GetString("A29_TXT_00001");
        CsUIData.Instance.SetFont(textNpc);

        Toggle toggleMonster = transform.Find("PanelList/ToggleList/ToggleMonster").GetComponent<Toggle>();
        Text textMonster = toggleMonster.transform.Find("Text").GetComponent<Text>();

        toggleMonster.onValueChanged.RemoveAllListeners();
        toggleMonster.onValueChanged.AddListener((ison) => { OnValueChangedList(ison, 1, textMonster); });
        textMonster.text = CsConfiguration.Instance.GetString("A29_TXT_00002");
        CsUIData.Instance.SetFont(textMonster);

        Toggle togglePortal = transform.Find("PanelList/ToggleList/TogglePortal").GetComponent<Toggle>();
        Text textPortal = togglePortal.transform.Find("Text").GetComponent<Text>();

        togglePortal.onValueChanged.RemoveAllListeners();
        togglePortal.onValueChanged.AddListener((ison) => { OnValueChangedList(ison, 2, textPortal); });
        textPortal.text = CsConfiguration.Instance.GetString("A29_TXT_00003");
        CsUIData.Instance.SetFont(textPortal);

        DisplayMap();
        DisplayList();
        InitializePath();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayList()
    {
        int nCount = m_trContent.childCount;
        for (int i = 0; i < nCount; ++i)
        {
            DestroyImmediate(m_trContent.GetChild(0).gameObject);
        }

        nCount = m_trIconList.childCount;
        for (int i = 0; i < nCount; ++i)
        {
            DestroyImmediate(m_trIconList.GetChild(0).gameObject);
        }

        Sprite spriteIcon;

        if (m_nLocationId == 201)
        {
            if (m_nToggleNumber == 0)
            {
                spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_npc");
                List<CsGuildTerritoryNpc> listNpc = CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList;

                for (int i = 0; i < listNpc.Count; ++i)
                {
                    CreateSlot(spriteIcon, listNpc[i]);
                    CreateIcon(spriteIcon, listNpc[i]);
                }
            }
        }
        else
        {
            int nContinetnId = CsGameData.Instance.GetContinentByLocationId(m_nLocationId).ContinentId;
            List<CsPortal> listPortal = CsGameData.Instance.GetPortalList(nContinetnId);

            switch (m_nToggleNumber)
            {
                case 0:
                    spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_npc");
                    List<CsNpcInfo> listNpc = CsGameData.Instance.NpcInfoList;

                    for (int i = 0; i < listNpc.Count; ++i)
                    {
                        if (listNpc[i].ContinentId == nContinetnId)
                        {
                            CreateSlot(spriteIcon, listNpc[i]);
                            CreateIcon(spriteIcon, listNpc[i]);
                        }
                    }
                    break;
                case 1:
                    spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_monster");
                    List<CsContinentMapMonster> listMonster = CsGameData.Instance.GetContinentByLocationId(m_nLocationId).ContinentMapMonsterList;

                    for (int i = 0; i < listMonster.Count; ++i)
                    {
                        CreateSlot(spriteIcon, listMonster[i]);
                        CreateIcon(spriteIcon, listMonster[i]);
                    }

                    break;
                case 2:
                    spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_portal");

                    for (int i = 0; i < listPortal.Count; ++i)
                    {
                        CreateSlot(spriteIcon, listPortal[i]);
                    }
                    break;
                default:
                    break;
            }

            spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_portal");

            for (int i = 0; i < listPortal.Count; ++i)
            {
                CreateIcon(spriteIcon, listPortal[i]);
            }

            CreateNationWarMonsterIcon();

            Transform trImageBackground = transform.Find("PanelMap/ImageBackGround");

            for (int i = 0; i < trImageBackground.childCount; i++)
            {
                if (trImageBackground.GetChild(i).name.Contains("NationWarArrow"))
                {
                    if (trImageBackground.GetChild(i).gameObject.activeSelf)
                    {
                        trImageBackground.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            if (CsNationWarManager.Instance.MyNationWarDeclaration != null && CsNationWarManager.Instance.MyNationWarDeclaration.Status == EnNationWarDeclaration.Current &&
                CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
            {
                Transform trNationWarArrow = trImageBackground.Find("NationWarArrow" + m_nLocationId);

                if (trNationWarArrow != null)
                {
                    if (CsNationWarManager.Instance.MyNationWarDeclaration.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                    {
                        for (int i = 0; i < trNationWarArrow.childCount; i++)
                        {
                            Image imageArrow = trNationWarArrow.GetChild(i).GetComponent<Image>();
                            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_world_war_arrow1");
                        }

                        trNationWarArrow.gameObject.SetActive(true);
                    }
                    else
                    {
                        for (int i = 0; i < trNationWarArrow.childCount; i++)
                        {
                            Image imageArrow = trNationWarArrow.GetChild(i).GetComponent<Image>();
                            imageArrow.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_world_war_arrow3");

                            RectTransform rectTransformArrow = trNationWarArrow.GetChild(i).GetComponent<RectTransform>();
                            rectTransformArrow.localRotation = Quaternion.Euler(0, 0, rectTransformArrow.eulerAngles.z - 180);
                        }

                        trNationWarArrow.gameObject.SetActive(true);
                    }
                }
            }
        }

        m_scrollRectList.verticalNormalizedPosition = 1;
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSlot(Sprite spriteIcon, CsNpcInfo csNpcInfo)
    {
        Transform trSlot = Instantiate(m_goMinimapSlot, m_trContent).transform;

        Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textName = trSlot.Find("Text").GetComponent<Text>();
        textName.text = string.Format(CsConfiguration.Instance.GetString("A29_TXT_01002"), csNpcInfo.Nick, csNpcInfo.Name);
        CsUIData.Instance.SetFont(textName);

        Button buttonMove = trSlot.Find("ButtonMove").GetComponent<Button>();
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(() => { OnClickMapMove(csNpcInfo.ContinentId, m_nNationId, csNpcInfo.Position); });
        buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMove = buttonMove.transform.Find("Text").GetComponent<Text>();
        textButtonMove.text = CsConfiguration.Instance.GetString("A29_BTN_00001");
        CsUIData.Instance.SetFont(textButtonMove);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSlot(Sprite spriteIcon, CsGuildTerritoryNpc csGuildTerritoryNpc)
    {
        Transform trSlot = Instantiate(m_goMinimapSlot, m_trContent).transform;

        Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textName = trSlot.Find("Text").GetComponent<Text>();
        textName.text = csGuildTerritoryNpc.Name;
        CsUIData.Instance.SetFont(textName);

        Button buttonMove = trSlot.Find("ButtonMove").GetComponent<Button>();
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(() => { OnClickMapMove(m_nLocationId, m_nNationId, csGuildTerritoryNpc.Position); });
        buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMove = buttonMove.transform.Find("Text").GetComponent<Text>();
        textButtonMove.text = CsConfiguration.Instance.GetString("A29_BTN_00001");
        CsUIData.Instance.SetFont(textButtonMove);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSlot(Sprite spriteIcon, CsContinentMapMonster csContinentMapMonster)
    {
        Transform trSlot = Instantiate(m_goMinimapSlot, m_trContent).transform;

        Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textName = trSlot.Find("Text").GetComponent<Text>();
        textName.text = string.Format(CsConfiguration.Instance.GetString("A29_TXT_01001"), csContinentMapMonster.MonsterInfo.Level, csContinentMapMonster.MonsterInfo.Name);
        CsUIData.Instance.SetFont(textName);

        Button buttonMove = trSlot.Find("ButtonMove").GetComponent<Button>();
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(() => { OnClickMapMove(csContinentMapMonster.ContinentId, m_nNationId, new Vector3(csContinentMapMonster.XPosition, csContinentMapMonster.YPosition, csContinentMapMonster.ZPosition)); });
        buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMove = buttonMove.transform.Find("Text").GetComponent<Text>();
        textButtonMove.text = CsConfiguration.Instance.GetString("A29_BTN_00001");
        CsUIData.Instance.SetFont(textButtonMove);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSlot(Sprite spriteIcon, CsPortal csPortal)
    {
        Transform trSlot = Instantiate(m_goMinimapSlot, m_trContent).transform;

        Image imageIcon = trSlot.Find("Image").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textName = trSlot.Find("Text").GetComponent<Text>();
        textName.text = csPortal.Name;
        CsUIData.Instance.SetFont(textName);

        Button buttonMove = trSlot.Find("ButtonMove").GetComponent<Button>();
        buttonMove.onClick.RemoveAllListeners();
        buttonMove.onClick.AddListener(() => { OnClickPortalMove(csPortal.ContinentId, m_nNationId, csPortal.Position, csPortal.PortalId); });
        buttonMove.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonMove = buttonMove.transform.Find("Text").GetComponent<Text>();
        textButtonMove.text = CsConfiguration.Instance.GetString("A29_BTN_00001");
        CsUIData.Instance.SetFont(textButtonMove);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateIcon(Sprite spriteIcon, CsNpcInfo csNpcInfo)
    {
        Transform trIcon = Instantiate(m_goMinimapIcon, m_trIconList).transform;
        trIcon.name = csNpcInfo.Name;
        trIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2((csNpcInfo.Position.x - m_flX) * m_flMinimapMagnification, (csNpcInfo.Position.z - m_flZ) * m_flMinimapMagnification);

        Image imageIcon = trIcon.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textIcon = trIcon.Find("Text").GetComponent<Text>();
        textIcon.text = csNpcInfo.Name;
        CsUIData.Instance.SetFont(textIcon);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateIcon(Sprite spriteIcon, CsGuildTerritoryNpc csGuildTerritoryNpc)
    {
        Transform trIcon = Instantiate(m_goMinimapIcon, m_trIconList).transform;
        trIcon.name = csGuildTerritoryNpc.Name;
        trIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2((csGuildTerritoryNpc.Position.x - m_flX) * m_flMinimapMagnification, (csGuildTerritoryNpc.Position.z - m_flZ) * m_flMinimapMagnification);

        Image imageIcon = trIcon.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textIcon = trIcon.Find("Text").GetComponent<Text>();
        textIcon.text = csGuildTerritoryNpc.Name;
        CsUIData.Instance.SetFont(textIcon);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateIcon(Sprite spriteIcon, CsContinentMapMonster csContinentMapMonster)
    {
        Transform trIcon = Instantiate(m_goMinimapIcon, m_trIconList).transform;
        trIcon.name = csContinentMapMonster.MonsterInfo.Name;
        trIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2((csContinentMapMonster.XPosition - m_flX) * m_flMinimapMagnification, (csContinentMapMonster.ZPosition - m_flZ) * m_flMinimapMagnification);

        Image imageIcon = trIcon.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textIcon = trIcon.Find("Text").GetComponent<Text>();
        textIcon.text = csContinentMapMonster.MonsterInfo.Name;
        CsUIData.Instance.SetFont(textIcon);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateIcon(Sprite spriteIcon, CsPortal csPortal)
    {
        Transform trIcon = Instantiate(m_goMinimapIcon, m_trIconList).transform;
        trIcon.name = csPortal.Name;
        trIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2((csPortal.Position.x - m_flX) * m_flMinimapMagnification, (csPortal.Position.z - m_flZ) * m_flMinimapMagnification);

        Image imageIcon = trIcon.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Text textIcon = trIcon.Find("Text").GetComponent<Text>();
        textIcon.text = csPortal.Name;
        CsUIData.Instance.SetFont(textIcon);
    }

    //---------------------------------------------------------------------------------------------------
    void CreateNationWarMonsterIcon()
    {
        if (CsNationWarManager.Instance.MyNationWarDeclaration != null && CsNationWarManager.Instance.MyNationWarDeclaration.Status == EnNationWarDeclaration.Current &&
            CsNationWarManager.Instance.MyNationWarDeclaration.TargetNationId == CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam)
        {
            for (int i = 0; i < CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Count; i++)
            {
                Sprite spriteIcon = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_world_war_position_mini_" + CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].ArrangeId);

                if (CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i].Continent.LocationId == m_nLocationId)
                {
                    CreateIcon(spriteIcon, CsGameData.Instance.NationWar.NationWarMonsterArrangeList[i]);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateIcon(Sprite spriteIcon, CsNationWarMonsterArrange csNationWarMonsterArrange)
    {
        Transform trIcon = Instantiate(m_goMinimapNationWarMonsterIcon, m_trIconList).transform;
        trIcon.name = "MapNationWarMonsterIcon" + csNationWarMonsterArrange.ArrangeId;
        trIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2((csNationWarMonsterArrange.Position.x - m_flX) * m_flMinimapMagnification, (csNationWarMonsterArrange.Position.z - m_flZ) * m_flMinimapMagnification);

        Image imageIcon = trIcon.Find("ImageIcon").GetComponent<Image>();
        imageIcon.sprite = spriteIcon;

        Image imageBattleIcoin = trIcon.Find("ImageBattleIcon").GetComponent<Image>();

        ClientCommon.PDSimpleNationWarMonsterInstance simpleNationWarMonsterInstance = CsNationWarManager.Instance.ListSimpleNationWarMonsterInstance.Find(a => a.monsterArrangeId == csNationWarMonsterArrange.ArrangeId);

        if (simpleNationWarMonsterInstance == null)
        {
            imageBattleIcoin.gameObject.SetActive(false);
        }
        else
        {
            if (simpleNationWarMonsterInstance.isBattleMode)
            {
                imageBattleIcoin.gameObject.SetActive(true);
            }
            else
            {
                imageBattleIcoin.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMap()
    {
        CsLocation csLocation = CsGameData.Instance.GetLocation(m_nLocationId);
        int nCount = m_trAreaList.childCount;

        for (int i = 0; i < nCount; ++i)
        {
            DestroyImmediate(m_trAreaList.GetChild(0).gameObject);
        }

        if (CsUIData.Instance.DungeonInNow == EnDungeon.UndergroundMaze)
        {
            CsUndergroundMaze csUndergroundMaze = CsDungeonManager.Instance.UndergroundMaze;
            m_imageMap.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/img_minimap_" + m_nLocationId);
            m_imageMap.rectTransform.sizeDelta = new Vector2((csUndergroundMaze.XSize * csLocation.MinimapMagnification), (csUndergroundMaze.ZSize * csLocation.MinimapMagnification));
            m_flMinimapMagnification = csLocation.MinimapMagnification;
            m_flX = csUndergroundMaze.X;
            m_flZ = csUndergroundMaze.Z;
            m_imageMap.GetComponent<CsMapLocation>().SettingValue(m_nLocationId, m_nNationId, m_flMinimapMagnification, m_flX, m_flZ);
        }
        else if (m_nLocationId == 201)
        {
            CsGuildTerritory csGuildTerritory = CsGameData.Instance.GuildTerritory;
            m_imageMap.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/img_minimap_" + m_nLocationId);
            m_imageMap.rectTransform.sizeDelta = new Vector2((csGuildTerritory.XSize * csLocation.MinimapMagnification), (csGuildTerritory.ZSize * csLocation.MinimapMagnification));
            m_flMinimapMagnification = csLocation.MinimapMagnification;
            m_flX = csGuildTerritory.X;
            m_flZ = csGuildTerritory.Z;
            m_imageMap.GetComponent<CsMapLocation>().SettingValue(m_nLocationId, m_nNationId, m_flMinimapMagnification, m_flX, m_flZ);

            for (int i = 0; i < csLocation.LocationAreaList.Count; ++i)
            {
                DisplayLocation(csLocation.LocationAreaList[i]);
            }
        }
        else
        {
            CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(m_nLocationId);
            m_imageMap.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/img_minimap_" + m_nLocationId);
            m_imageMap.rectTransform.sizeDelta = new Vector2((csContinent.XSize * csLocation.MinimapMagnification), (csContinent.ZSize * csLocation.MinimapMagnification));
            m_flMinimapMagnification = csLocation.MinimapMagnification;
            m_flX = csContinent.X;
            m_flZ = csContinent.Z;
            m_imageMap.GetComponent<CsMapLocation>().SettingValue(m_nLocationId, m_nNationId, m_flMinimapMagnification, m_flX, m_flZ);

            for (int i = 0; i < csLocation.LocationAreaList.Count; ++i)
            {
                DisplayLocation(csLocation.LocationAreaList[i]);
            }
        }

        if (CsGameData.Instance.MyHeroInfo.LocationId == m_nLocationId)
        {
            m_imagePlayer.gameObject.SetActive(true);
        }
        else
        {
            m_imagePlayer.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLocation(CsLocationArea csLocationArea)
    {
        Transform trArea = Instantiate(m_goMinimapText, m_trAreaList).transform;
        trArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(csLocationArea.MinimapX, csLocationArea.MinimapY);
        Text textArea = trArea.Find("Text").GetComponent<Text>();
        textArea.text = string.Format("<color={0}>{1}</color>", csLocationArea.MinimapTextColorCode, csLocationArea.Name);
        CsUIData.Instance.SetFont(textArea);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplyNationWarMonsterBattleIcon(int nArrangeId, bool bIson)
    {
        Transform trMapNationWarMonsterIcon = m_trIconList.Find("MapNationWarMonsterIcon" + nArrangeId);

        if (trMapNationWarMonsterIcon != null)
        {
            Transform trBattleIcon = trMapNationWarMonsterIcon.Find("ImageBattleIcon");
            trBattleIcon.gameObject.SetActive(bIson);
        }
    }

    #region Path

    Vector3[] m_avtPrePath;
    bool m_bDrawing = false;
    float m_flPathIconDistance;
    List<Vector3> m_listMove = null;
    List<RectTransform> m_listRect = new List<RectTransform>();

    //---------------------------------------------------------------------------------------------------
    //Path 설정
    void InitializePath()
    {
        m_avtPrePath = CsGameData.Instance.PathCornerByAutoMove;

        //설정된 패스가 없을 경우 리턴한다.
        if (m_avtPrePath == null) return;

        Vector3[] corners = ConvertPathCorners(m_avtPrePath);

        m_listMove = corners.ToList();

        m_flPathIconDistance = 16f / m_flMinimapMagnification;
        for (int i = 1; i < m_listMove.Count; i++)
        {
            Vector2 startPos = m_listMove[i - 1];
            Vector2 endPos = m_listMove[i];

            float t = 0;
            float distance = Vector3.Distance(startPos, endPos);
            float increament = m_flPathIconDistance / distance;

            while (increament > 1f)
            {
                //마지막 인덱스는 지우지 않는다.
                if (i == m_listMove.Count - 1)
                    break;
                m_listMove.RemoveAt(i);
                endPos = m_listMove[i];
                distance = Vector3.Distance(startPos, endPos);
                increament = m_flPathIconDistance / distance;
            }

            for (t = 0; t < 1; t += increament)
            {
                m_listMove[i] = AddPathPos(Vector2.Lerp(startPos, endPos, t));
            }
        }

        if (CsGameData.Instance.MyHeroInfo.LocationId == m_nLocationId && CsGameData.Instance.AutoMoveLocationId == m_nLocationId)
        {
            m_imageLastPoint.rectTransform.anchoredPosition = new Vector2((m_listMove[m_listMove.Count - 1].x - m_flX) * m_flMinimapMagnification, (m_listMove[m_listMove.Count - 1].y - m_flZ) * m_flMinimapMagnification);
            m_imageLastPoint.gameObject.SetActive(true);
        }
        else if (CsGameData.Instance.AutoMoveLocationId == m_nLocationId)
        {
            Vector3 vtPos = CsGameData.Instance.AutoMoveObjectPos;
            m_imageLastPoint.rectTransform.anchoredPosition = new Vector2((vtPos.x - m_flX) * m_flMinimapMagnification, (vtPos.z - m_flZ) * m_flMinimapMagnification);
            m_imageLastPoint.gameObject.SetActive(true);
        }
        m_bDrawing = true;

    }

    //---------------------------------------------------------------------------------------------------
    //Pos값을 2D좌표계로 변환
    Vector3[] ConvertPathCorners(Vector3[] corners)
    {
        Vector3[] converted = new Vector3[corners.Length];
        for (int i = 0; i < corners.Length; i++)
        {
            converted[i] = new Vector3(corners[i].x, corners[i].z);
        }
        return converted;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePathPos()
    {
        //만약 현재 나의 위치와 켜져 있는 맵이 다를 경우 표시하지 않는다.
        //도착 지점이 현재 맵과 동일한 경우에만 도착지점만 표시 해준다.
        if (CsGameData.Instance.MyHeroInfo.LocationId != m_nLocationId)
        {
            if (!m_imageLastPoint.gameObject.activeSelf && CsGameData.Instance.AutoMoveLocationId == m_nLocationId)
            {
                Vector3 vtPos = CsGameData.Instance.AutoMoveObjectPos;
                m_imageLastPoint.rectTransform.anchoredPosition = new Vector2((vtPos.x - m_flX) * m_flMinimapMagnification, (vtPos.z - m_flZ) * m_flMinimapMagnification);
                m_imageLastPoint.gameObject.SetActive(true);
            }
            return;
        }

        if (m_avtPrePath != null)
        {
            if (m_bDrawing)
            {
                //도착지점
                if (m_listRect.Count == 1)
                {
                    if (Vector2.Distance(m_imagePlayer.rectTransform.anchoredPosition, m_imageLastPoint.rectTransform.anchoredPosition) < 1f)
                    {
                        m_listRect[0].SetParent(m_trDisableMoveList);
                        m_listRect.RemoveAt(0);
                    }
                }
                else if (m_listRect.Count > 1)
                {
                    if (Vector3.Distance(m_listRect[0].anchoredPosition, m_imagePlayer.rectTransform.anchoredPosition) < (25f / m_flMinimapMagnification))
                    {
                        m_listRect[0].SetParent(m_trDisableMoveList);
                        m_listRect.RemoveAt(0);
                    }
                }
                else
                {
                    AllPathClear();
                    m_bDrawing = false;
                }
            }
        }
        else
        {
            InitializePath();
        }
    }

    //---------------------------------------------------------------------------------------------------
    //Path 추가(생성한 이미지가 부족할때 생성해서 추가)
    Vector3 AddPathPos(Vector3 vtPos)
    {
        if (m_trDisableMoveList.childCount == 0)
        {
            Instantiate(m_goImageMovePoint, m_trDisableMoveList);
        }

        Transform trPoint = m_trDisableMoveList.GetChild(0);
        trPoint.SetParent(m_trEnableMoveList);
        RectTransform rectTransform = trPoint.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2((vtPos.x - m_flX) * m_flMinimapMagnification, (vtPos.y - m_flZ) * m_flMinimapMagnification);
        m_listRect.Add(rectTransform);
        return vtPos;
    }

    //---------------------------------------------------------------------------------------------------
    //Path 초기화
    public void AllPathClear()
    {
        m_imageLastPoint.gameObject.SetActive(false);
        m_avtPrePath = null;
        m_bDrawing = false;
        for (int i = 0; i < m_listRect.Count; ++i)
        {
            m_listRect[i].SetParent(m_trDisableMoveList);
        }
        m_listRect.Clear();
    }

    #endregion Path

}
