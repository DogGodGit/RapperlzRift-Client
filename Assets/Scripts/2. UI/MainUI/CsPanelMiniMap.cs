using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ClientCommon;

public enum EnCameraMode
{
    CameraAuto = 1,
    Camera3D,
    Camera2D
}

public class CsPanelMiniMap : MonoBehaviour
{
    [SerializeField] GameObject m_goMiniObject;

    Transform m_trMonsterList;
    Transform m_trNpcList;
    Transform m_trPortalList;
    Transform m_trDisableList;

    RectTransform m_rtrQuestPoint;
    RectTransform m_rtrQuestNpcPoint;
    RectTransform m_rtrQuestArrow;

    Toggle m_toggleMinimapRotation;

    Image m_imageMiniMap;
    Image m_imagePlayer;
    Image m_imageCameraMode;

    Text m_textName;
    Text m_textNation;
    Text m_textLocation;

    //미니맵 비율
    float m_flMinimapMagnification;

    //미니맵 X,Z 사이즈
    float m_flX = 0f;
    float m_flZ = 0f;

    float m_flMinimapDistance;

    //오브젝트 이미지 
    Sprite m_spriteMonster;
    Sprite m_spriteNpc;
    Sprite m_spritePortal;

    List<IMonsterObjectInfo> m_listMonster = new List<IMonsterObjectInfo>();
    List<INpcObjectInfo> m_listNpc = new List<INpcObjectInfo>();

    List<IMonsterObjectInfo> m_listGameDataMonster;
    List<INpcObjectInfo> m_listGameDataNpc;

    int m_nLocationId = 0;
    int m_nLocationParam = 0;

    EnCameraMode m_enCameraMode = EnCameraMode.CameraAuto;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventToUI.Instance.EventPrevContinentEnter += OnEventPrevContinentEnter;
        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;
        CsGameEventUIToUI.Instance.EventNationTransmission += OnEventNationTransmission;

        CsMainQuestManager.Instance.EventAccepted += OnEventAccepted;
        CsMainQuestManager.Instance.EventExecuteDataUpdated += OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge += OnEventArtifactRoomNextFloorChallenge;
		CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge += OnEventArtifactRoomBanishedForNextFloorChallenge;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        QuestArrow();

        //몬스터 갱신
        if (m_listGameDataMonster.SequenceEqual(m_listMonster))
        {
            for (int i = 0; i < m_listGameDataMonster.Count; ++i)
            {
                UpdateObject(m_listGameDataMonster[i]);
            }
        }
        else
        {
            m_listMonster.Clear();
            int nCount = m_trMonsterList.childCount;

            for (int i = 0; i < nCount; ++i)
            {
                m_trMonsterList.GetChild(0).SetParent(m_trDisableList);
            }

            for (int i = 0; i < m_listGameDataMonster.Count; ++i)
            {
                CreateObject(m_listGameDataMonster[i]);
                m_listMonster.Add(m_listGameDataMonster[i]);
            }
        }

        //NPC 갱신
        if (m_listGameDataNpc.SequenceEqual(m_listNpc))
        {
            for (int i = 0; i < m_listGameDataNpc.Count; ++i)
            {
                UpdateObject(m_listGameDataNpc[i]);
            }
        }
        else
        {
            m_listNpc.Clear();
            int nCount = m_trNpcList.childCount;

            for (int i = 0; i < nCount; ++i)
            {
                m_trNpcList.GetChild(0).SetParent(m_trDisableList);
            }

            for (int i = 0; i < m_listGameDataNpc.Count; ++i)
            {
                CreateObject(m_listGameDataNpc[i]);
                m_listNpc.Add(m_listGameDataNpc[i]);
            }
        }

        Transform trPlayer = CsGameData.Instance.MyHeroTransform;

        //좌표갱신
        if (trPlayer != null)
        {
            m_textLocation.text = string.Format("{0},{1}", (int)trPlayer.position.x, (int)trPlayer.position.z);

            float fMinimapImageWidth = m_imageMiniMap.rectTransform.rect.width;
            float fMinimapImageHeight = m_imageMiniMap.rectTransform.rect.height;

            float fPivotX = 1.0f - (fMinimapImageWidth - ((trPlayer.position.x - m_flX) * m_flMinimapMagnification)) / fMinimapImageWidth;
            float fPivotY = 1.0f - (fMinimapImageHeight - ((trPlayer.position.z - m_flZ) * m_flMinimapMagnification)) / fMinimapImageHeight;

            m_imageMiniMap.rectTransform.pivot = new Vector2(fPivotX, fPivotY);

            if (m_toggleMinimapRotation.isOn)
            {
                m_imageMiniMap.rectTransform.eulerAngles = new Vector3(0f, 0f, (CsIngameData.Instance.InGameCamera.transform.rotation.eulerAngles.y));
                m_imagePlayer.rectTransform.eulerAngles = new Vector3(0f, 0f, (180f - trPlayer.rotation.eulerAngles.y + m_imageMiniMap.rectTransform.localEulerAngles.z));
            }
            else
            {
                m_imageMiniMap.rectTransform.eulerAngles = new Vector3(0f, 0f, 0f);
                m_imagePlayer.rectTransform.eulerAngles = new Vector3(0f, 0f, (180f - trPlayer.rotation.eulerAngles.y));
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventToUI.Instance.EventPrevContinentEnter -= OnEventPrevContinentEnter;
        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;
        CsGameEventUIToUI.Instance.EventNationTransmission -= OnEventNationTransmission;

        CsMainQuestManager.Instance.EventAccepted -= OnEventAccepted;
        CsMainQuestManager.Instance.EventExecuteDataUpdated -= OnEventExecuteDataUpdated;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsDungeonManager.Instance.EventArtifactRoomNextFloorChallenge -= OnEventArtifactRoomNextFloorChallenge;
		CsDungeonManager.Instance.EventArtifactRoomBanishedForNextFloorChallenge -= OnEventArtifactRoomBanishedForNextFloorChallenge;
    }

    #region Event Handler

    //지하미로 조건 추가
    //---------------------------------------------------------------------------------------------------
    void OnClickOpenMap()
    {
        if (CsUIData.Instance.DungeonInNow != EnDungeon.None)
            return;
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Minimap, EnSubMenu.MinimapArea);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCameraModeChange()
    {
        if (m_enCameraMode == EnCameraMode.Camera2D)
        {
            m_enCameraMode = EnCameraMode.CameraAuto;
        }
        else
        {
            m_enCameraMode++;
        }

        m_imageCameraMode.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_camera_" + (int)m_enCameraMode);
        CsGameEventToIngame.Instance.OnEventChangeCameraState(m_enCameraMode);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMinimapRotation(bool bIson, Image imageToggleMinimapRotationBackground)
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Toggle);

        if (imageToggleMinimapRotationBackground == null)
        {
            return;
        }
        else
        {
            if (bIson)
            {
                imageToggleMinimapRotationBackground.color = new Color32(255, 255, 255, 0);
            }
            else
            {
                imageToggleMinimapRotationBackground.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedOtherUserView(bool bIson, Image imageToggleOtherUserViewBackground)
    {
        CsGameEventToIngame.Instance.OnEventOtherHeroView(bIson);
        CsUIData.Instance.PlayUISound(EnUISoundType.Toggle);

        if (imageToggleOtherUserViewBackground == null)
        {
            return;
        }
        else
        {
            if (bIson)
            {
                imageToggleOtherUserViewBackground.color = new Color32(255, 255, 255, 0);
            }
            else
            {
                imageToggleOtherUserViewBackground.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    #endregion Event Handler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventPrevContinentEnter()
    {
        DisPlayMiniMap();
        UpdatePathPos();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAccepted(int nTransformationMonsterId, long[] alRemovedAbnormalStateEffects)
    {
        DisPlayMiniMap();
        UpdatePathPos();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExecuteDataUpdated(int nProgressCount)
    {
        DisPlayMiniMap();
        UpdatePathPos();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        DisPlayMiniMap();
        UpdatePathPos();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
        DisPlayMiniMap();
        UpdatePathPos();
    }

	//---------------------------------------------------------------------------------------------------
    void OnEventNationTransmission(string strSceneName)
    {
        DisPlayMiniMap(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventArtifactRoomNextFloorChallenge()
    {
        DisplayNextArtifacRoom();
        UpdatePathPos();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventArtifactRoomBanishedForNextFloorChallenge()
	{
		DisplayNextArtifacRoom();
		UpdatePathPos();
	}

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trMinimap = transform.Find("ImageMask/ImageMinimap");

        m_trMonsterList = trMinimap.Find("MonsterList");
        m_trNpcList = trMinimap.Find("NpcList");
        m_trPortalList = trMinimap.Find("PortalList");
        m_trDisableList = trMinimap.Find("DisableList");
        m_rtrQuestPoint = trMinimap.Find("ImageQusetPoint").GetComponent<RectTransform>();
        m_rtrQuestNpcPoint = trMinimap.Find("ImageQusetNpcPoint").GetComponent<RectTransform>();
        m_rtrQuestArrow = transform.Find("ImageMask/ImageArrow").GetComponent<RectTransform>();
        m_spriteMonster = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_minimap_monster");
        m_spriteNpc = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_minimap_npc");
        m_spritePortal = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/ico_map_portal");

        Button buttonMap = transform.Find("ImageFront").GetComponent<Button>();
        buttonMap.onClick.RemoveAllListeners();
        buttonMap.onClick.AddListener(OnClickOpenMap);
        buttonMap.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonCameraMode = transform.Find("ButtonCamera").GetComponent<Button>();
        buttonCameraMode.onClick.RemoveAllListeners();
        buttonCameraMode.onClick.AddListener(OnClickCameraModeChange);
        buttonCameraMode.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_imageCameraMode = buttonCameraMode.transform.Find("Image").GetComponent<Image>();
        m_imageCameraMode.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_camera_" + (int)m_enCameraMode);

        m_toggleMinimapRotation = transform.Find("ToggleMinimapRotation").GetComponent<Toggle>();

        Image imageToggleMinimapRotationBackground = m_toggleMinimapRotation.transform.Find("Background").GetComponent<Image>();
        imageToggleMinimapRotationBackground.color = new Color32(255, 255, 255, 0);

        m_toggleMinimapRotation.onValueChanged.RemoveAllListeners();
        m_toggleMinimapRotation.isOn = true;
        m_toggleMinimapRotation.onValueChanged.AddListener((ison) => OnValueChangedMinimapRotation(ison, imageToggleMinimapRotationBackground));

        Toggle toggleOtherUserView = transform.Find("ToggleOtherUserView").GetComponent<Toggle>();

        Image imageToggleOtherUserViewBackground = toggleOtherUserView.transform.Find("Background").GetComponent<Image>();
        imageToggleOtherUserViewBackground.color = new Color32(255, 255, 255, 0);

        toggleOtherUserView.onValueChanged.RemoveAllListeners();
        toggleOtherUserView.isOn = true;
        toggleOtherUserView.onValueChanged.AddListener((ison) => OnValueChangedOtherUserView(ison, imageToggleOtherUserViewBackground));

        m_flMinimapDistance = buttonMap.GetComponent<RectTransform>().rect.width / 2f;

        m_textName = transform.Find("ImageName/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textName);

        m_textNation = transform.Find("ImageName/TextNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNation);

        m_textLocation = transform.Find("TextLocation").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLocation);

        m_imagePlayer = transform.Find("ImageMask/ImagePlayer").GetComponent<Image>();
        m_imageMiniMap = transform.Find("ImageMask/ImageMinimap").GetComponent<Image>();
        m_listGameDataMonster = CsGameData.Instance.ListMonsterObjectInfo;
        m_listGameDataNpc = CsGameData.Instance.ListNpcObjectInfo;

        DisPlayMiniMap();
        UpdatePathPos();
    }

    //---------------------------------------------------------------------------------------------------
    void DisPlayMiniMap(bool bReset = false)
    {
        if (m_nLocationId != CsGameData.Instance.MyHeroInfo.LocationId || m_nLocationParam != CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam || bReset)
        {
            m_nLocationId = CsGameData.Instance.MyHeroInfo.LocationId;
            m_nLocationParam = CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam;

            Transform trButtonCameraMode = transform.Find("ButtonCamera");
            trButtonCameraMode.gameObject.SetActive(true);

            CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(m_nLocationId);

            //길드영지
            if (m_nLocationId == 201)
            {
                CsGuildTerritory csGuildTerritory = CsGameData.Instance.GuildTerritory;
                m_flX = csGuildTerritory.X;
                m_flZ = csGuildTerritory.Z;
                MinimapChange(csGuildTerritory.XSize, csGuildTerritory.ZSize, CsConfiguration.Instance.GetString("A67_TXT_00001"));
            }
            else if (m_nLocationId == 110)
            {
                CsUndergroundMaze csUndergroundMaze = CsDungeonManager.Instance.UndergroundMaze;
                m_flX = csUndergroundMaze.X;
                m_flZ = csUndergroundMaze.Z;
                MinimapChange(csUndergroundMaze.XSize, csUndergroundMaze.ZSize, csUndergroundMaze.Name);
            }
            else if (csContinent != null)
            {
                m_flX = csContinent.X;
                m_flZ = csContinent.Z;
                MinimapChange(csContinent.XSize, csContinent.ZSize, csContinent.Name, true);
            }
            else
            {
                Debug.Log("##@@ DisplayMinimap @@## : " + CsDungeonManager.Instance.DungeonPlay);
                switch (CsDungeonManager.Instance.DungeonPlay)
                {
                    case EnDungeonPlay.MainQuest:
                        CsMainQuestDungeon csMainQuestDungeon = CsMainQuestDungeonManager.Instance.MainQuestDungeon;
                        m_flX = csMainQuestDungeon.X;
                        m_flZ = csMainQuestDungeon.Z;
                        MinimapChange(csMainQuestDungeon.XSize, csMainQuestDungeon.ZSize, csMainQuestDungeon.Name);
                        break;

                    case EnDungeonPlay.Story:
                        CsStoryDungeon csStoryDungeon = CsDungeonManager.Instance.StoryDungeon;
                        m_flX = csStoryDungeon.X;
                        m_flZ = csStoryDungeon.Z;
                        MinimapChange(csStoryDungeon.XSize, csStoryDungeon.ZSize, csStoryDungeon.Name);
                        break;

                    case EnDungeonPlay.Exp:
                        CsExpDungeon csExpDungeon = CsDungeonManager.Instance.ExpDungeon;
                        m_flX = csExpDungeon.X;
                        m_flZ = csExpDungeon.Z;
                        MinimapChange(csExpDungeon.XSize, csExpDungeon.ZSize, csExpDungeon.Name);
                        break;

                    case EnDungeonPlay.Gold:
                        CsGoldDungeon csGoldDungeon = CsDungeonManager.Instance.GoldDungeon;
                        m_flX = csGoldDungeon.X;
                        m_flZ = csGoldDungeon.Z;
                        MinimapChange(csGoldDungeon.XSize, csGoldDungeon.ZSize, csGoldDungeon.Name);
                        break;

                    case EnDungeonPlay.UndergroundMaze:
                        CsUndergroundMaze csUndergroundMaze = CsDungeonManager.Instance.UndergroundMaze;
                        m_flX = csUndergroundMaze.X;
                        m_flZ = csUndergroundMaze.Z;
                        MinimapChange(csUndergroundMaze.XSize, csUndergroundMaze.ZSize, csUndergroundMaze.Name);
                        break;

                    case EnDungeonPlay.AncientRelic:
                        CsAncientRelic csAncientRelic = CsDungeonManager.Instance.AncientRelic;
                        m_flX = csAncientRelic.X;
                        m_flZ = csAncientRelic.Z;
                        MinimapChange(csAncientRelic.XSize, csAncientRelic.ZSize, csAncientRelic.Name);
                        break;

                    case EnDungeonPlay.ArtifactRoom:
                        CsArtifactRoom csArtifactRoom = CsDungeonManager.Instance.ArtifactRoom;
                        m_flX = csArtifactRoom.X;
                        m_flZ = csArtifactRoom.Z;
                        MinimapChange(csArtifactRoom.XSize, csArtifactRoom.ZSize, csArtifactRoom.Name, false, CsDungeonManager.Instance.ArtifactRoomFloor.Floor);
                        break;

                    case EnDungeonPlay.FieldOfHonor:
                        CsFieldOfHonor csFieldOfHonor = CsDungeonManager.Instance.FieldOfHonor;
                        m_flX = csFieldOfHonor.X;
                        m_flZ = csFieldOfHonor.Z;
                        MinimapChange(csFieldOfHonor.XSize, csFieldOfHonor.ZSize, csFieldOfHonor.Name);
                        break;

                    case EnDungeonPlay.SoulCoveter:
                        CsSoulCoveter csSoulCoveter = CsDungeonManager.Instance.SoulCoveter;
                        m_flX = csSoulCoveter.X;
                        m_flZ = csSoulCoveter.Z;
                        MinimapChange(csSoulCoveter.XSize, csSoulCoveter.ZSize, csSoulCoveter.Name);
                        break;

                    case EnDungeonPlay.Elite:
                        CsEliteDungeon csEliteDungeon = CsDungeonManager.Instance.EliteDungeon;
                        m_flX = csEliteDungeon.X;
                        m_flZ = csEliteDungeon.Z;
                        MinimapChange(csEliteDungeon.XSize, csEliteDungeon.ZSize, csEliteDungeon.Name);
                        break;

                    case EnDungeonPlay.ProofOfValor:
                        CsProofOfValor csProofOfValor = CsDungeonManager.Instance.ProofOfValor;
                        m_flX = csProofOfValor.X;
                        m_flZ = csProofOfValor.Z;
                        MinimapChange(csProofOfValor.XSize, csProofOfValor.ZSize, csProofOfValor.Name);
                        break;

                    case EnDungeonPlay.WisdomTemple:
						CsWisdomTemple csWisdomTemple = CsDungeonManager.Instance.WisdomTemple;
						m_flX = csWisdomTemple.X;
						m_flZ = csWisdomTemple.Z;
						MinimapChange(csWisdomTemple.XSize, csWisdomTemple.ZSize, csWisdomTemple.Name);
						break;

                    case EnDungeonPlay.RuinsReclaim:
						CsRuinsReclaim csRuinsClaim = CsDungeonManager.Instance.RuinsReclaim;
						m_flX = csRuinsClaim.X;
						m_flZ = csRuinsClaim.Z;
						MinimapChange(csRuinsClaim.XSize, csRuinsClaim.ZSize, csRuinsClaim.Name);
						break;

                    case EnDungeonPlay.InfiniteWar:
                        CsInfiniteWar csInfiniteWar = CsDungeonManager.Instance.InfiniteWar;
                        m_flX = csInfiniteWar.X;
                        m_flZ = csInfiniteWar.Z;
                        MinimapChange(csInfiniteWar.XSize, csInfiniteWar.ZSize, csInfiniteWar.Name);
                        break;

                    case EnDungeonPlay.FearAltar:
						CsFearAltarStage csFearAltarStage = CsDungeonManager.Instance.FearAltarStage;
						m_flX = csFearAltarStage.X;
						m_flZ = csFearAltarStage.Z;
						MinimapChange(csFearAltarStage.XSize, csFearAltarStage.ZSize, csFearAltarStage.Name);
						break;

                    case EnDungeonPlay.WarMemory:
                        CsWarMemory csWarMemory = CsDungeonManager.Instance.WarMemory;
                        m_flX = csWarMemory.X;
                        m_flZ = csWarMemory.Z;
                        MinimapChange(csWarMemory.XSize, csWarMemory.ZSize, csWarMemory.Name);
                        break;

                    case EnDungeonPlay.Biography:
						CsBiographyQuestDungeon csBiographyQuestDungeon = CsDungeonManager.Instance.BiographyQuestDungeon;
						m_flX = csBiographyQuestDungeon.X;
						m_flZ = csBiographyQuestDungeon.Z;
						MinimapChange(csBiographyQuestDungeon.XSize, csBiographyQuestDungeon.ZSize, csBiographyQuestDungeon.Name);
						break;

                    case EnDungeonPlay.OsirisRoom:
                        trButtonCameraMode.gameObject.SetActive(false);
                        CsOsirisRoom csOsirisRoom = CsDungeonManager.Instance.OsirisRoom;
                        m_flX = csOsirisRoom.X;
                        m_flZ = csOsirisRoom.Z;
                        MinimapChange(csOsirisRoom.XSize, csOsirisRoom.ZSize, csOsirisRoom.Name);
                        break;

                    case EnDungeonPlay.DragonNest:

                        CsDragonNest csDragonNest = CsDungeonManager.Instance.DragonNest;

                        m_flX = csDragonNest.X;
                        m_flZ = csDragonNest.Z;

                        MinimapChange(csDragonNest.XSize, csDragonNest.ZSize, csDragonNest.Name);

                        break;

                    case EnDungeonPlay.TradeShip:

                        CsTradeShip csTradeShip = CsDungeonManager.Instance.TradeShip;

                        m_flX = csTradeShip.X;
                        m_flZ = csTradeShip.Z;

                        MinimapChange(csTradeShip.XSize, csTradeShip.ZSize, csTradeShip.Name);

                        break;

                    case EnDungeonPlay.AnkouTomb:

                        CsAnkouTomb csAnkouTomb = CsDungeonManager.Instance.AnkouTomb;

                        m_flX = csAnkouTomb.X;
                        m_flZ = csAnkouTomb.Z;

                        MinimapChange(csAnkouTomb.XSize, csAnkouTomb.ZSize, csAnkouTomb.Name);

                        break;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayNextArtifacRoom()
    {
        CsArtifactRoom csArtifactRoom = CsDungeonManager.Instance.ArtifactRoom;
        m_flX = csArtifactRoom.X;
        m_flZ = csArtifactRoom.Z;
        MinimapChange(csArtifactRoom.XSize, csArtifactRoom.ZSize, csArtifactRoom.Name, false, CsDungeonManager.Instance.ArtifactRoomFloor.Floor);
    }

    //---------------------------------------------------------------------------------------------------
    void MinimapChange(float flXSize, float flZSize, string strName, bool bContinent = false, int bArtifactRoomFloor = 0)
    {
        CsLocation csLocation = CsGameData.Instance.GetLocation(m_nLocationId);
        m_imageMiniMap.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMap/img_minimap_" + m_nLocationId);
        m_flMinimapMagnification = csLocation.MinimapMagnification;

        if (bArtifactRoomFloor != 0)
        {
            m_textName.text = string.Format(CsConfiguration.Instance.GetString("A47_TXT_03002"), bArtifactRoomFloor);
        }
        else
        {
            m_textName.text = strName;
        }

        m_imageMiniMap.rectTransform.sizeDelta = new Vector2((flXSize * m_flMinimapMagnification), (flZSize * m_flMinimapMagnification));
        m_imageMiniMap.rectTransform.anchoredPosition = Vector2.zero;

        if (bContinent)
        {
            m_textNation.gameObject.SetActive(true);
            CsNation csNation = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam);
            m_textNation.text = csNation.Name;

            if (CsGameData.Instance.MyHeroInfo.Nation.NationId == csNation.NationId)
            {
                m_textNation.color = new Color32(206, 170, 139, 255);
            }
            else
            {
                m_textNation.color = CsUIData.Instance.ColorRed;
            }
        }
        else
        {
            m_textNation.gameObject.SetActive(false);
        }

        int nCount = m_trPortalList.childCount;
        for (int i = 0; i < nCount; ++i)
        {
            m_trPortalList.GetChild(0).SetParent(m_trDisableList);
        }

        CsContinent csContinent = CsGameData.Instance.GetContinentByLocationId(m_nLocationId);

        if (csContinent != null)
        {
            List<CsPortal> listPortal = CsGameData.Instance.GetPortalList(csContinent.ContinentId);
            for (int i = 0; i < listPortal.Count; ++i)
            {
                CreateObject(listPortal[i]);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateObject(IMonsterObjectInfo monsterObjectInfo)
    {
        Transform trMonster = m_trMonsterList.Find(monsterObjectInfo.GetInstanceId().ToString());

        if (trMonster != null)
        {
            RectTransform rectTransformMonster = trMonster.GetComponent<RectTransform>();
            rectTransformMonster.anchoredPosition = ConvertPosition(monsterObjectInfo.GetTransform().position);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateObject(INpcObjectInfo npcObjectInfo)
    {
        Transform trNpc = m_trNpcList.Find(npcObjectInfo.GetInstanceId().ToString());

        if (trNpc != null)
        {
            RectTransform rectTransformNpc = trNpc.GetComponent<RectTransform>();
            rectTransformNpc.anchoredPosition = ConvertPosition(npcObjectInfo.GetTransform().position);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateObject(IMonsterObjectInfo monsterObjectInfo)
    {
        Transform trMonster = AddObject();

        if (trMonster != null)
        {
            trMonster.SetParent(m_trMonsterList);
            trMonster.name = monsterObjectInfo.GetInstanceId().ToString();
            Image imageMonster = trMonster.GetComponent<Image>();
            imageMonster.sprite = m_spriteMonster;
            imageMonster.SetNativeSize();
            imageMonster.rectTransform.anchoredPosition = ConvertPosition(monsterObjectInfo.GetTransform().position);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateObject(INpcObjectInfo npcObjectInfo)
    {
        Transform trNpc = AddObject();

        if (trNpc != null)
        {
            trNpc.SetParent(m_trNpcList);
            trNpc.name = npcObjectInfo.GetInstanceId().ToString();
            Image imageNpc = trNpc.GetComponent<Image>();
            imageNpc.sprite = m_spriteNpc;
            imageNpc.SetNativeSize();
            imageNpc.rectTransform.anchoredPosition = ConvertPosition(npcObjectInfo.GetTransform().position);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateObject(CsPortal csPortal)
    {
        Transform trPortal = AddObject();
        if (trPortal != null)
        {
            trPortal.SetParent(m_trPortalList);
            trPortal.name = csPortal.Name;
            Image imagePortal = trPortal.GetComponent<Image>();
            imagePortal.sprite = m_spritePortal;
            imagePortal.rectTransform.sizeDelta = new Vector2(32f, 32f);
            imagePortal.rectTransform.anchoredPosition = ConvertPosition(csPortal.Position);

        }
    }

    //---------------------------------------------------------------------------------------------------
    Transform AddObject()
    {
        if (m_trDisableList.childCount == 0)
            return Instantiate(m_goMiniObject, m_trDisableList).transform;
        else
            return m_trDisableList.GetChild(0);
    }

    #region Path

    //---------------------------------------------------------------------------------------------------
    void UpdatePathPos()
    {
        CsMainQuest csMainQuest = CsMainQuestManager.Instance.MainQuest;
        EnMainQuestState enMainQuestState = CsMainQuestManager.Instance.MainQuestState;

        m_rtrQuestPoint.gameObject.SetActive(false);
        m_rtrQuestNpcPoint.gameObject.SetActive(false);

        //더이상 퀘스트가 없을 경우
        if (csMainQuest == null)
        {
            return;
        }

        //플레이어가 필드일 경우
        if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId) != null)
        {
            if (enMainQuestState == EnMainQuestState.Accepted)
            {
                switch (csMainQuest.MainQuestType)
                {
                    case EnMainQuestType.Move:
                    case EnMainQuestType.Kill:
                    case EnMainQuestType.Collect:
                    case EnMainQuestType.Interaction:
                        if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.TargetContinent.ContinentId)
                        {
                            QuestPoint(csMainQuest.TargetPosition);
                        }
                        else
                        {
                            ChaseContinent(csMainQuest.TargetContinent.ContinentId);
                        }
                        break;

                    case EnMainQuestType.Dungeon:
                        if (csMainQuest.StartNpc != null)
                        {
                            if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.StartNpc.ContinentId)
                            {
                                QuestPoint(csMainQuest.StartNpc.Position, true);
                            }
                            else
                            {
                                ChaseContinent(csMainQuest.StartNpc.ContinentId);
                            }
                        }

                        break;
                }
            }
            else if (enMainQuestState == EnMainQuestState.Executed)
            {
                if (csMainQuest.CompletionNpc != null)
                {
                    if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.CompletionNpc.ContinentId)
                    {
                        QuestPoint(csMainQuest.CompletionNpc.Position, true);
                    }
                    else
                    {
                        ChaseContinent(csMainQuest.CompletionNpc.ContinentId);
                    }
                }
                else if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.TargetContinent.ContinentId)
                {
                    QuestPoint(csMainQuest.TargetPosition);
                }
                else
                {
                    ChaseContinent(csMainQuest.TargetContinent.ContinentId);
                }
            }
            else if (enMainQuestState == EnMainQuestState.None)
            {
                if (csMainQuest.StartNpc != null)
                {
                    if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.StartNpc.ContinentId)
                    {
                        QuestPoint(csMainQuest.StartNpc.Position, true);
                    }
                    else
                    {
                        ChaseContinent(csMainQuest.StartNpc.ContinentId);
                    }
                }
                else if (CsGameData.Instance.GetContinentByLocationId(CsGameData.Instance.MyHeroInfo.LocationId).ContinentId == csMainQuest.TargetContinent.ContinentId)
                {
                    QuestPoint(csMainQuest.TargetPosition);
                }
                else
                {
                    ChaseContinent(csMainQuest.TargetContinent.ContinentId);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //퀘스트 포인트가 멀 경우 퀘스트 방향을 표시해준다.
    void QuestArrow()
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.MyHeroInfo.Nation.NationId)
        {
            Transform trHero = CsGameData.Instance.MyHeroTransform;

            if (trHero != null && m_rtrQuestPoint.gameObject.activeSelf)
            {
                float flDistance = Vector2.Distance(m_rtrQuestPoint.anchoredPosition, ConvertPosition(trHero.position));

                if (flDistance > m_flMinimapDistance)
                {
                    float flAngle = 0;

                    if (m_toggleMinimapRotation.isOn)
                    {
                        flAngle = GetAngle(m_rtrQuestPoint.anchoredPosition, ConvertPosition(trHero.position)) + m_imageMiniMap.rectTransform.localEulerAngles.z;
                    }
                    else
                    {
                        flAngle = GetAngle(m_rtrQuestPoint.anchoredPosition, ConvertPosition(trHero.position));
                    }

                    m_rtrQuestArrow.eulerAngles = Vector3.forward * flAngle;

                    float flX = Mathf.Cos((flAngle + 180) * Mathf.Deg2Rad) * (m_flMinimapDistance - 35f);
                    float flY = Mathf.Sin((flAngle + 180) * Mathf.Deg2Rad) * (m_flMinimapDistance - 35f);

                    m_rtrQuestArrow.anchoredPosition = new Vector2(flX, flY);

                    if (!m_rtrQuestArrow.gameObject.activeSelf)
                        m_rtrQuestArrow.gameObject.SetActive(true);
                }
                else
                {
                    if (m_rtrQuestArrow.gameObject.activeSelf)
                        m_rtrQuestArrow.gameObject.SetActive(false);
                }
            }
            else
            {
                m_rtrQuestArrow.gameObject.SetActive(false);
            }
        }
        else
        {
            m_rtrQuestPoint.gameObject.SetActive(false);
            m_rtrQuestArrow.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    float GetAngle(Vector2 vStart, Vector2 vEnd)
    {
        Vector2 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    //---------------------------------------------------------------------------------------------------
    Vector2 ConvertPosition(Vector3 vtPos)
    {
        return new Vector2((vtPos.x - m_flX) * m_flMinimapMagnification, (vtPos.z - m_flZ) * m_flMinimapMagnification);
    }

    //---------------------------------------------------------------------------------------------------
    void QuestPoint(Vector3 vtPos, bool bNpc = false)
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == CsGameData.Instance.MyHeroInfo.Nation.NationId)
        {
            m_rtrQuestPoint.anchoredPosition = ConvertPosition(vtPos);

            if (!m_rtrQuestPoint.gameObject.activeSelf)
                m_rtrQuestPoint.gameObject.SetActive(true);

            if (bNpc)
            {
                m_rtrQuestNpcPoint.anchoredPosition = ConvertPosition(vtPos);
                if (!m_rtrQuestNpcPoint.gameObject.activeSelf)
                    m_rtrQuestNpcPoint.gameObject.SetActive(true);
            }
            else
            {
                if (!m_rtrQuestNpcPoint.gameObject.activeSelf)
                    m_rtrQuestNpcPoint.gameObject.SetActive(false);
            }
        }
        else
        {
            m_rtrQuestPoint.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void ChaseContinent(int nContinentId)
    {
        List<CsPortal> listPortal = CsGameData.Instance.GetPortalList(nContinentId); // 1. 목적 대륙에 존재하는 포탈을 리스트에 저장.
        if (listPortal == null || listPortal.Count == 0) return;

        listPortal = GetDirection(listPortal); // 다음 대륙에 존재 여부 확인.
        if (listPortal != null)
        {
            listPortal = GetDirection(listPortal); // 다음 다음 대륙에 존재 여부 확인.
            if (listPortal != null)
            {
                listPortal = GetDirection(listPortal); // 다음 다음 다음 대륙에 존재 여부 확인.
                if (listPortal != null)
                {
                    listPortal = GetDirection(listPortal);// 다음 다음 다음 다음 대륙에 존재 여부 확인.
                }
            }
        }

        if (listPortal != null)
        {
            Debug.Log("ChaseContinent     길을 찾을수가 없습니다 추가 길찾기 필요.");
        }
    }

    //---------------------------------------------------------------------------------------------------
    List<CsPortal> GetDirection(List<CsPortal> listPortal)
    {
        List<CsPortal> listCurrentPortal = new List<CsPortal>();
        List<CsPortal> listNextPortal = new List<CsPortal>();
        for (int i = 0; i < listPortal.Count; i++)
        {
            listCurrentPortal = CsGameData.Instance.GetPortalList(listPortal[i].ContinentId); // 2. 목적 대륙과 연결된 대륙에 있는 모든 포탈 정보.
            for (int j = 0; j < listCurrentPortal.Count; j++)
            {
                CsPortal csPortal = CsGameData.Instance.GetPortal(listCurrentPortal[j].LinkedPortalId); // 2. 목적 포탈에 링크로 연결된 다른 대륙에 위치해 있는 포탈 정보.
                if (csPortal == null) continue;
                if (csPortal.ContinentId == CsGameData.Instance.MyHeroInfo.LocationId) // 3. 링크로 연결된 포탈이 위치한 대륙과 현재 내 대륙과 같은지 확인.
                {
                    QuestPoint(csPortal.Position);
                    return null;
                }
                else
                {
                    listNextPortal.Add(csPortal); // 3-2. 위치가 다르면 다음 체크 포탈 리스트에 저장.
                }
            }
        }
        return listNextPortal;
    }

    #endregion Path
}
