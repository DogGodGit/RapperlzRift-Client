using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using WebCommon;
using ClientCommon;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.Video;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-05-23)
//---------------------------------------------------------------------------------------------------

public enum EnSoundTypeIntro
{
    Intro = 0,
    Main = 1,
    Chracter = 2,
}

public enum EnUISoundTypeIntro
{
    Button = 0,
    Exit = 1,
    PopupOpen = 2,
    PopupClose = 3,
}

public class CsSceneIntro : CsScene
{
    UserCredential m_userCredential = null;         // 사용자 증명
    UserCredential m_userCredentialGuest = null;    // 게스트 사용자 증명
    User m_user = null;
    Transform m_trCanvas;                           // 메인캔버스// 현재 사용자
	Transform m_trPopupServer;
    Transform m_trIntroRobby;
    Transform m_trCharacterCustomizing;
    Transform m_trFade;
    Transform m_trLightList;
    Transform m_trLoading;
    AudioClip m_audioClipIntroLoading;
    AudioClip m_audioClipIntroMain;
    AudioClip m_audioClipIntroCharacter;
    AudioSource m_audioSource;
    bool m_bIsFirstVisit = true;
    bool m_bIsEndLoading = false;
    bool m_bIsEndDataLoading = false;
    bool m_bIsProcess = false;
    bool m_bIsTouched = false;
    bool m_bIsFirstPopupLogin = true;               // 로그인팝업 이니셜라이즈
    bool m_bIsFirstLogin = true;                    // 게스트로그인 처음인지 재접속인지
	bool m_bIsFirstPopupNotice = true;				// 공지 팝업 이니셜라이즈
	int m_nFrame;
    int m_nGraphics;
    int m_nResolution;
    int m_nEffactVisible;
    int m_nHeroCreationDefaultNationId;             // 영웅생성기본국가ID

    string m_strBuildDate = "2018/05/25";

    Button m_buttonGameStart;

    List<CsGameAssetBundle> m_csGameAssetBundleList;
    int m_nDownloadAssetBundleCount = 0;

	List<CsGameServerRegion> m_listCsGameServerRegion = null;
    //List<CsGameServerGroup> m_csGameServerGroupList = null;
	int m_nSelectedServerRegionId = 0;
    int m_nSelectedServerGroupId = 0;
    List<CsUserHero> m_listCsCsUserHero = null;

    CsCustomizing m_csCustomizing;

    CsGameServer m_csGameServerSelected = null;     // 선택 서버
    
    List<CsLobbyHero> m_csLobbyHeroList = new List<CsLobbyHero>();
    CsLobbyHero m_csLobbyHeroNew;
    CsLobbyHero m_csLobbyHeroSelected = null;
    int m_nLobbyHeroJobIdTemp;                      //직업토글을 누를때마다 직업Id를 임시로 저장

    IEnumerator m_iEnumerator;

    bool m_bMute = false;
    bool m_bIsCreateEnter = false;

    CsPanelModal m_csPanelModal;

	List<CsAnnouncement> m_listCsAnnouncement = new List<CsAnnouncement>();
	
    [SerializeField] VideoClip m_VideoclipIntro;                  // 인트로 영상

	int m_nTick = 0;
	float m_flDeltaTime = 0;
	bool m_bConnected = false;
	float m_flTime = 0;

	bool m_bClickSkipButton = false;

	//---------------------------------------------------------------------------------------------------


	//---------------------------------------------------------------------------------------------------
	void Awake()
    {
		CsUIData.Instance.SetDeviceResolution();
		
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		switch (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.Graphic))
		{
			case 0:
				Screen.SetResolution(1280, 720, true);
				QualitySettings.SetQualityLevel(1);
				break;

			case 1:
				Screen.SetResolution(1280, 720, true);
				QualitySettings.SetQualityLevel(3);
				break;

			case 2:
				Screen.SetResolution(1920, 1080, true);
				QualitySettings.SetQualityLevel(5);
				break;

			default:
				QualitySettings.SetQualityLevel(3);
				break;
		}

        m_trCanvas = GameObject.Find("Canvas").transform;
        m_trFade = m_trCanvas.Find("ImageFade");
        m_audioSource = transform.GetComponent<AudioSource>();
        m_trLoading = m_trCanvas.Find("ImageLoading");
        InitializeLoadingUI();

        // 사용자 증명 로딩
        LoadUserCredential();
        LoadGuestUserCredential();

        CsRplzSession.Instance.EventConnected += OnEventConnected;
        CsRplzSession.Instance.EventDisconnected += OnEventDisconnected;
        CsRplzSession.Instance.EventResGetTime += OnEventResGetTime;

		//폰트로드
		CsUIData.Instance.LoadFont(Resources.Load<Font>("Fonts/RappelzFont"));
        CsUIData.Instance.LoadDamageFont(Resources.Load<Font>("Fonts/damage_font"));
        CsUIData.Instance.LoadDungeonDamageFont(Resources.Load<Font>("Fonts/WCManoNegraBta"));

        //StartCoroutine(LoadFontCoroutine());
        //BGM로드
        StartCoroutine(LoadBGMSoundIntro());

        CsUIData.Instance.LoadUISound();
        CsUIData.Instance.SetAudio(true);

#if !UNITY_EDITOR
		Debug.unityLogger.logEnabled = false;
#endif
	}

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        m_csPanelModal = m_trCanvas.Find("PanelModal").GetComponent<CsPanelModal>();

        // 시스템 정보 요청

		RequestAuthServerInfo();
		//RequestStageFarmVersion(); // RequestAuthServerInfo 2018/05/28 적용. Native에서 데이터를 받아온 후 진행.
        //RequestSystemSetttings();  // RequestStageFarmVersion  11/7 신규 추가. Gate 연결 후 진행.
        CsCustomizingManager.Instance.Init();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        CsRplzSession.Instance.Service();

		m_flDeltaTime += (Time.deltaTime - m_flDeltaTime) * 0.1f;       // 프레임 

		if (m_flTime + 1f < Time.time)
		{
			if (CsConfiguration.Instance.SystemSetting != null && ++m_nTick % CsConfiguration.Instance.SystemSetting.StatusLoggingInterval == 0 && m_bConnected)
			{
				// ping, fps 전송
				if (CsRplzSession.Instance.PhotonPeer != null)
				{
					SendStatusLogging(CsRplzSession.Instance.PhotonPeer.RoundTripTime, (1.0f / m_flDeltaTime));
				}
			}

			m_flTime = Time.time;
		}
	}

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
	{
		m_userCredential = null;  
		m_userCredentialGuest = null;
		m_user = null;
		m_trCanvas = null;
		m_trIntroRobby = null;
		m_trCharacterCustomizing = null;
		m_trFade = null;
		m_trLightList = null;
		m_trLoading = null;
		m_audioClipIntroLoading = null;
		m_audioClipIntroMain = null;
		m_audioClipIntroCharacter = null;
		m_audioSource = null;
		m_buttonGameStart = null;
		if (m_csGameAssetBundleList != null)
		{
			m_csGameAssetBundleList.Clear();
		}
		if (m_listCsGameServerRegion != null)
		{
			m_listCsGameServerRegion.Clear();
		}
		if (m_listCsCsUserHero != null)
		{
			m_listCsCsUserHero.Clear();
		}
		m_csCustomizing = null;
		m_csGameServerSelected = null;
		if (m_csLobbyHeroList != null)
		{
			m_csLobbyHeroList.Clear();
		}
		m_csLobbyHeroNew = null;
		m_csLobbyHeroSelected = null;		
		m_iEnumerator = null;
		m_csPanelModal = null;
		if (m_listCsAnnouncement != null)
		{
			m_listCsAnnouncement.Clear();
		}
		m_VideoclipIntro = null;

		CsRplzSession.Instance.EventResGetTime -= OnEventResGetTime;
		CsRplzSession.Instance.EventDisconnected -= OnEventDisconnected;
        CsRplzSession.Instance.EventConnected -= OnEventConnected;
		Resources.UnloadUnusedAssets();
    }

	DateTimeOffset m_dtoServerTime;

	////---------------------------------------------------------------------------------------------------
	void SendGetTime()
	{
		GetTimeCommandBody cmdBody = new GetTimeCommandBody();
		CsRplzSession.Instance.Send(ClientCommandName.GetTime, cmdBody);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventResGetTime(int nReturnCode, GetTimeResponseBody responseBody)
	{
		if (nReturnCode == 0)
		{
			m_dtoServerTime = responseBody.time;
		}
		else
		{
			m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), null, "OK");
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadBGMSoundIntro()
    {
        if(m_audioClipIntroLoading == null)
        {
            ResourceRequest resourceRequestIntroMain = CsUIData.Instance.LoadAssetAsync<AudioClip>("Sound/UI/BGM_Intro_Loading");
            yield return resourceRequestIntroMain;
            m_audioClipIntroLoading = ((AudioClip)resourceRequestIntroMain.asset);
        }

        if (m_audioClipIntroMain == null)
        {
            ResourceRequest resourceRequestIntroMain = CsUIData.Instance.LoadAssetAsync<AudioClip>("Sound/UI/BGM_Intro_Main");
            yield return resourceRequestIntroMain;
            m_audioClipIntroMain = ((AudioClip)resourceRequestIntroMain.asset);
        }

        if (m_audioClipIntroCharacter == null)
        {
            ResourceRequest resourceRequestIntroCharacter = CsUIData.Instance.LoadAssetAsync<AudioClip>("Sound/UI/BGM_Intro_Character");
            yield return resourceRequestIntroCharacter;
            m_audioClipIntroCharacter = ((AudioClip)resourceRequestIntroCharacter.asset);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadFontCoroutine()
    {
        Font font = null;
        if (CsUIData.Instance.Language == EnServerLanguage.ChineseSimplified)
        {
            ResourceRequest resourceRequestFont = Resources.LoadAsync<Font>("Fonts/RappelzFont_ZH");
            yield return resourceRequestFont;
            font = (Font)resourceRequestFont.asset;
        }
        else
        {
            font = Resources.Load<Font>("Fonts/RappelzFont");
        }

        if (font != null)
        {
            CsUIData.Instance.LoadFont(font);
            InitializeLoadingUI();
            InitializeUI();
            m_csPanelModal.InitializeUI();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator Load3DCharacterCoroutine()
    {
        Debug.Log("Load3DCharacterCoroutine()");
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("IntroCharacter", LoadSceneMode.Additive);
        yield return asyncOperation;
        asyncOperation = null;

        asyncOperation = SceneManager.LoadSceneAsync("IntroLobby", LoadSceneMode.Additive);
        yield return asyncOperation;

        m_trLightList = GameObject.Find("LightList").transform;
        m_trIntroRobby = GameObject.Find("IntroLobby").transform;
        m_trIntroRobby.gameObject.SetActive(false);

        CreateCharacterModel();

        StopCoroutine(Load3DCharacterCoroutine());
    }

    //---------------------------------------------------------------------------------------------------
    void CreateCharacterModel()
    {
		Debug.Log("CreateCharacterModel()");
		StartCoroutine(UpdateCharacterModelCoroutine());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator UpdateCharacterModelCoroutine()     //캐릭터모델 동적로드함수
    {
        Transform trCharacterList = GameObject.Find("CharacterList").transform;

        List<CsJob> listCsJobParent = new List<CsJob>(CsGameData.Instance.JobList).FindAll(a => a.ParentJobId == 0 || a.JobId == a.ParentJobId);

        for (int i = 0; i < listCsJobParent.Count; i++)
        {
            int nIndex = listCsJobParent[i].JobId;

            Transform trCharacter = trCharacterList.Find("Character" + nIndex);
            Transform trCustom = trCharacterList.Find("CustomCharacter" + nIndex);

            ResourceRequest resourceRequest;
            GameObject goCharacter = null;
            GameObject goCustom = null;

            switch ((EnJob)nIndex)
            {
                case EnJob.Gaia:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/Gaia");
                    yield return resourceRequest;
                    goCharacter = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Asura:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/Asura");
                    yield return resourceRequest;
                    goCharacter = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Deva:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/Deva");
                    yield return resourceRequest;
                    goCharacter = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Witch:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/Witch");
                    yield return resourceRequest;
                    goCharacter = (GameObject)resourceRequest.asset;
                    break;
            }

            goCharacter = Instantiate(goCharacter, trCharacterList);
            goCharacter.name = "Character" + nIndex;
            trCharacter = goCharacter.transform;

			Transform trCharacterEquip = trCharacter.Find("Character");
			CsCustomizingManager.Instance.InitCustom(nIndex, trCharacterEquip, true);
			trCharacter.gameObject.SetActive(false);

            switch ((EnJob)nIndex) // 커스텀.
            {
                case EnJob.Gaia:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/CustomGaia");
                    yield return resourceRequest;
                    goCustom = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Asura:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/CustomAsura");
                    yield return resourceRequest;
                    goCustom = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Deva:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/CustomDeva");
                    yield return resourceRequest;
                    goCustom = (GameObject)resourceRequest.asset;
                    break;
                case EnJob.Witch:
                    resourceRequest = Resources.LoadAsync("Prefab/Select/CustomWitch");
                    yield return resourceRequest;
                    goCustom = (GameObject)resourceRequest.asset;
                    break;
            }

            goCustom = Instantiate(goCustom, trCharacterList);
            goCustom.name = "CustomCharacter" + nIndex;
            trCustom = goCustom.transform;
            trCustom.gameObject.SetActive(false);
        }

        m_bIsEndLoading = true;
    }

#region Send_Protocol
    //---------------------------------------------------------------------------------------------------
    void OnEventConnected()
    {
        // 로그인.
        SendLogin();
		SendGetTime();
		m_bConnected = true;
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventDisconnected()
    {
        //OpenPopupError(4);
        m_csPanelModal.Choice(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00001"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
		m_bConnected = false;
	}

#endregion

#region Send_Protocol
    //---------------------------------------------------------------------------------------------------
    public void SendLogin()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResLogin += OnEventResLogin;

            LoginCommandBody cmdBody = new LoginCommandBody();
            cmdBody.virtualGameServerId = m_csGameServerSelected.VirtualGameServerId;
            cmdBody.accessToken = m_user.AccessToken;
            CsRplzSession.Instance.Send(ClientCommandName.Login, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResLogin(int nReturnCode, LoginResponseBody responseBody)
    {
        Debug.Log("OnEventResLogin : " + nReturnCode);

        m_bIsProcess = false;

        if (nReturnCode == 0)
        {
            CsConfiguration.Instance.User = m_user;
            SendLobbyInfo();
        }
        else if (nReturnCode == 101)
        {
            Debug.Log("ERROR (OnEventResLogin) : 101 - 이미 로그인중이거나 로그인되어 있습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00701"), OnClickCreateCharacterError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 102)
        {
            Debug.Log("ERROR (OnEventResLogin) : 102 - 가상게임서버가 존재하지 않습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00702"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 103)
        {
            Debug.Log("ERROR (OnEventResLogin) : 103 - 게임서버가 다릅니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00703"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 104)
        {
            Debug.Log("ERROR (OnEventResLogin) : 104 -  사용자가 존재하지 않습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00704"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 105)
        {
            Debug.Log("ERROR (OnEventResLogin) : 105 - 엑세스시크릿이 다릅니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00705"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 106)
        {
            Debug.Log("ERROR (OnEventResLogin) : 106 - 현재 사용자가 너무 많습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00706"), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else
        {
            Debug.Log("ERROR (OnEventResLogin) : " + nReturnCode);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupError(5, nReturnCode);
        }

        CsRplzSession.Instance.EventResLogin -= OnEventResLogin;
    }

    //---------------------------------------------------------------------------------------------------
    void SendLobbyInfo()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResLobbyInfo += OnEventResLobbyInfo;

            LobbyInfoCommandBody cmdBody = new LobbyInfoCommandBody();
            CsRplzSession.Instance.Send(ClientCommandName.LobbyInfo, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResLobbyInfo(int nReturnCode, LobbyInfoResponseBody responseBody)
    {
        Debug.Log("OnEventResLobbyInfo : " + nReturnCode);
        if (nReturnCode == 0)
        {
            ClosePopupMainLobby();
            m_csLobbyHeroList.Clear();

            for (int i = 0; i < responseBody.heroes.Length; i++)
            {
                CsLobbyHero csLobbyHero = new CsLobbyHero(responseBody.heroes[i]);

                if (csLobbyHero.HeroId == responseBody.lastHeroId)
                    m_csLobbyHeroSelected = csLobbyHero;

                m_csLobbyHeroList.Add(csLobbyHero);
            }

            m_nHeroCreationDefaultNationId = responseBody.heroCreationDefaultNationId;

            if (m_csLobbyHeroList.Count <= 0)
            {
                //캐릭터 생성창 오픈	
				OpenPopupCreateCharacter();
            }
            else
            {
                OpenPopupSelectCharacter();
            }
        }
        else
        {
            Debug.Log("ERROR (OnEventResLobbyInfo) : " + nReturnCode);
            //EnOpenedInventoryPopup(5, nReturnCode);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OnClickServerContact, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }

        CsRplzSession.Instance.EventResLobbyInfo -= OnEventResLobbyInfo;
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendHeroCreate()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResHeroCreate += OnEventResHeroCreate;

            HeroCreateCommandBody cmdBody = new HeroCreateCommandBody();
            cmdBody.jobId = m_csLobbyHeroNew.JobId;
            cmdBody.nationId = m_csLobbyHeroNew.NationId;
            cmdBody.customPresetHair = m_csLobbyHeroNew.CustomPresetHair;
            cmdBody.customFaceJawHeight = m_csLobbyHeroNew.CustomFaceJawHeight;
            cmdBody.customFaceJawWidth = m_csLobbyHeroNew.CustomFaceJawWidth;
            cmdBody.customFaceJawEndHeight = m_csLobbyHeroNew.CustomFaceJawEndHeight;
            cmdBody.customFaceWidth = m_csLobbyHeroNew.CustomFaceWidth;
            cmdBody.customFaceEyebrowHeight = m_csLobbyHeroNew.CustomFaceEyebrowHeight;
            cmdBody.customFaceEyebrowRotation = m_csLobbyHeroNew.CustomFaceEyebrowRotation;
            cmdBody.customFaceEyesWidth = m_csLobbyHeroNew.CustomFaceEyesWidth;
            cmdBody.customFaceNoseHeight = m_csLobbyHeroNew.CustomFaceNoseHeight;
            cmdBody.customFaceNoseWidth = m_csLobbyHeroNew.CustomFaceNoseWidth;
            cmdBody.customFaceMouthHeight = m_csLobbyHeroNew.CustomFaceMouthHeight;
            cmdBody.customFaceMouthWidth = m_csLobbyHeroNew.CustomFaceMouthWidth;
            cmdBody.customBodyHeadSize = m_csLobbyHeroNew.CustomBodyHeadSize;
            cmdBody.customBodyArmsLength = m_csLobbyHeroNew.CustomBodyArmsLength;
            cmdBody.customBodyArmsWidth = m_csLobbyHeroNew.CustomBodyArmsWidth;
            cmdBody.customBodyChestSize = m_csLobbyHeroNew.CustomBodyChestSize;
            cmdBody.customBodyWaistWidth = m_csLobbyHeroNew.CustomBodyWaistWidth;
            cmdBody.customBodyHipsSize = m_csLobbyHeroNew.CustomBodyHipsSize;
            cmdBody.customBodyPelvisWidth = m_csLobbyHeroNew.CustomBodyPelvisWidth;
            cmdBody.customBodyLegsLength = m_csLobbyHeroNew.CustomBodyLegsLength;
            cmdBody.customBodyLegsWidth = m_csLobbyHeroNew.CustomBodyLegsWidth;
            cmdBody.customColorSkin = m_csLobbyHeroNew.CustomColorSkin;
            cmdBody.customColorEyes = m_csLobbyHeroNew.CustomColorEyes;
            cmdBody.customColorBeardAndEyebrow = m_csLobbyHeroNew.CustomColorBeardAndEyebrow;
            cmdBody.customColorHair = m_csLobbyHeroNew.CustomColorHair;

            CsRplzSession.Instance.Send(ClientCommandName.HeroCreate, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResHeroCreate(int nReturnCode, HeroCreateResponseBody responseBody)
    {
        m_bIsProcess = false;

        Debug.Log("OnEventResHeroCreate : " + nReturnCode);
        if (nReturnCode == 0)
        {
            m_csLobbyHeroNew.HeroId = responseBody.heroId;
            m_csLobbyHeroNew.Level = responseBody.level;
            m_csLobbyHeroNew.BattlePower = responseBody.battlePower;

            m_csLobbyHeroList.Add(m_csLobbyHeroNew);
            m_csLobbyHeroSelected = m_csLobbyHeroNew;

            ClosePopupSelectNation();
            OpenPopupTutorial();
        }
        else if (nReturnCode == 101)
        {
            Debug.Log("ERROR (OnEventResHeroCreate) : 101 - 영웅수가 최대보유수를 초과합니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00201"), OnClickCreateCharacterError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupError(1);
        }
        else
        {
            Debug.Log("ERROR (OnEventResHeroCreate) : " + nReturnCode);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OnClickCreateCharacterError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupError(1, nReturnCode);
        }

        CsRplzSession.Instance.EventResHeroCreate -= OnEventResHeroCreate;
    }

    //---------------------------------------------------------------------------------------------------
    void SendHeroNamingTutorialComplete()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResHeroNamingTutorialComplete += OnEventResHeroNamingTutorialComplete;

            HeroNamingTutorialCompleteCommandBody cmdBody = new HeroNamingTutorialCompleteCommandBody();
            cmdBody.heroId = m_csLobbyHeroSelected.HeroId;
            CsRplzSession.Instance.Send(ClientCommandName.HeroNamingTutorialComplete, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResHeroNamingTutorialComplete(int nReturnCode, HeroNamingTutorialCompleteResponseBody responseBody)
    {
        Debug.Log("OnEventResHeroNamingTutorialComplete : " + nReturnCode);
        if (nReturnCode == 0)
        {
            m_csLobbyHeroSelected.NamingTutorialCompleted = true;

            ClosePopupTutorial();
            OpenPopupCreateName();
        }
        else if (nReturnCode == 101)
        {
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 101 - 해당 영웅이 존재하지 않습니다.");
            //OpenPopupError(0);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00301"), OnClickTutorialError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else if (nReturnCode == 102)
        {
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 102 - 이미 이름짓기튜토리얼을 완료했습니다.");
            //OpenPopupError(0);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00302"), OnClickTutorialError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else
        {
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : " + nReturnCode);
            //OpenPopupError(0, nReturnCode);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OnClickTutorialError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }

        CsRplzSession.Instance.EventResHeroNamingTutorialComplete -= OnEventResHeroNamingTutorialComplete;
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendHeroNameSet()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResHeroNameSet += OnEventResHeroNameSet;

            HeroNameSetCommandBody cmdBody = new HeroNameSetCommandBody();
            cmdBody.heroId = m_csLobbyHeroSelected.HeroId;
            cmdBody.name = m_csLobbyHeroSelected.Name;
            CsRplzSession.Instance.Send(ClientCommandName.HeroNameSet, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResHeroNameSet(int nReturnCode, HeroNameSetResponseBody responseBody)
    {
        Debug.Log("OnEventResHeroNameSet : " + nReturnCode);
        if (nReturnCode == 0)
        {
			CsConfiguration.Instance.SendFirebaseLogEvent("character_creation_finish");

            ClosePopupCreateName();

            if (m_iEnumerator != null)
            {
                StopCoroutine(m_iEnumerator);
            }

            m_bIsCreateEnter = true;
            m_iEnumerator = FadeOut(SendHeroLogin);
            StartCoroutine(m_iEnumerator);
        }
        else if (nReturnCode == 101)
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 101 - 이름이 유효하지 않습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00401"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupCreateNameError(0);
        }
        else if (nReturnCode == 102)
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 102 - 해당 영웅이 존재하지 않습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00402"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupCreateNameError(3);
        }
        else if (nReturnCode == 103)
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 103 - 이미 생성완료되었습니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00403"), CreateNameError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupCreateNameError(3);
        }
        else if (nReturnCode == 104)
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 104 - 해당 이름이 이미 존재합니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00404"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            //OpenPopupCreateNameError(2);
        }
        else if (nReturnCode == 105)
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNamingTutorialComplete) : 105 - 해당 이름은 금지어입니다.");
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00051"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else
        {
            m_csLobbyHeroSelected.Name = "";
            Debug.Log("ERROR (OnEventResHeroNameSet) : " + nReturnCode);
            //OpenPopupCreateNameError(0);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }

        CsRplzSession.Instance.EventResHeroNameSet -= OnEventResHeroNameSet;
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendHeroDelete()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResHeroDelete += OnEventResHeroDelete;

            HeroDeleteCommandBody cmdBody = new HeroDeleteCommandBody();
            cmdBody.heroId = m_csLobbyHeroSelected.HeroId;
            CsRplzSession.Instance.Send(ClientCommandName.HeroDelete, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResHeroDelete(int nReturnCode, HeroDeleteResponseBody responseBody)
    {
        Debug.Log("OnEventResHeroDelete : " + nReturnCode);

        if (nReturnCode == 0)
        {
            // 선택된 캐릭터를 삭제한다.
            m_csLobbyHeroList.Remove(m_csLobbyHeroSelected);
            if (m_csLobbyHeroList.Count == 0)
            {
                // 캐릭터 생성화면으로 이동.
				m_csLobbyHeroSelected = null;
                OnClickCancleCharacterDelete();
                ClosePopupSelectCharacter();
                OpenPopupCreateCharacter();
            }
            else
            {
                // 캐릭터 선택화면으로 이동
                OnClickCancleCharacterDelete();
                m_csLobbyHeroSelected = m_csLobbyHeroList[0];
                OpenPopupSelectCharacter();
            }
        }
        else if (nReturnCode == 101)
        {
            Debug.Log("ERROR (OnEventResHeroDelete) : 101 - 해당 영웅이 존재하지 않습니다.");
            //OpenPopupDeleteCharacterError(1);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00501"), DeleteCharacterError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
        else
        {
            Debug.Log("ERROR (OnEventResHeroDelete) : " + nReturnCode);
            //OpenPopupDeleteCharacterError(1);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), DeleteCharacterError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }

        CsRplzSession.Instance.EventResHeroDelete -= OnEventResHeroDelete;
        m_bIsProcess = false;
    }

    //---------------------------------------------------------------------------------------------------
    void SendHeroLogin()
    {
        if (!m_bIsProcess)
        {
            m_bIsProcess = true;

            CsRplzSession.Instance.EventResHeroLogin += OnEventResHeroLogin;

            HeroLoginCommandBody cmdBody = new HeroLoginCommandBody();
            cmdBody.id = m_csLobbyHeroSelected.HeroId;
            CsRplzSession.Instance.Send(ClientCommandName.HeroLogin, cmdBody);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventResHeroLogin(int nReturnCode, HeroLoginResponseBody responseBody)
    {
        Debug.Log("OnEventResHeroLogin : " + nReturnCode);

		m_bIsProcess = false;

		if (nReturnCode == 0)
        {
            CsGameData.Instance.MyHeroInfo = new CsMyHeroInfo(responseBody.id,
                                                              responseBody.name,
                                                              responseBody.nationId,
                                                              responseBody.jobId,
                                                              responseBody.level,
                                                              responseBody.exp,
                                                              responseBody.maxHP,
                                                              responseBody.hp,
                                                              responseBody.date,
															  responseBody.spiritStone,
															  responseBody.paidInventorySlotCount,
                                                              responseBody.initEntranceLocationId,
                                                              responseBody.initEntranceLocationParam,
                                                              responseBody.lak,
                                                              responseBody.mainGearEnchantDailyCount,
                                                              responseBody.mainGearRefinementDailyCount,
                                                              responseBody.ownDia,
                                                              responseBody.unOwnDia,
                                                              responseBody.gold,
                                                              responseBody.freeImmediateRevivalDailyCount,
                                                              responseBody.paidImmediateRevivalDailyCount,
                                                              responseBody.restTime,
                                                              responseBody.party,
                                                              responseBody.vipPoint,
                                                              responseBody.expPotionDailyUseCount,
                                                              responseBody.receivedLevelUpRewards,
                                                              responseBody.receivedDailyAcessRewards,
                                                              responseBody.dailyAccessTime,
                                                              responseBody.receivedAttendRewardDate,
                                                              responseBody.receivedAttendRewardDay,
                                                              responseBody.equippedMountId,
                                                              responseBody.mountGearRefinementDailyCount,
                                                              responseBody.wings,
                                                              responseBody.equippedWingId,
                                                              responseBody.wingStep,
                                                              responseBody.wingLevel,
                                                              responseBody.wingExp,
                                                              responseBody.wingParts,
                                                              responseBody.freeSweepDailyCount,
                                                              responseBody.storyDungeonPlays,
                                                              responseBody.storyDungeonClears,
                                                              responseBody.stamina,
                                                              responseBody.staminaAutoRecoveryRemainingTime,
                                                              responseBody.bIsRiding,
                                                              responseBody.expDungeonDailyPlayCount,
                                                              responseBody.expDungeonClearedDifficulties,
                                                              responseBody.mainGearEnchantLevelSetNo,
                                                              responseBody.subGearSoulstoneLevelSetNo,
                                                              responseBody.expScrollDailyUseCount,
                                                              responseBody.expScrollItemId,
                                                              responseBody.expScrollRemainingTime,
                                                              responseBody.goldDungeonDailyPlayCount,
                                                              responseBody.goldDungeonClearedDifficulties,
                                                              responseBody.undergroundMazeDailyPlayTime,
                                                              responseBody.artifactRoomBestFloor,
                                                              responseBody.artifactRoomCurrentFloor,
                                                              responseBody.artifactRoomDailyInitCount,
                                                              responseBody.artifactRoomSweepProgressFloor,
                                                              responseBody.artifactRoomSweepRemainingTime,
                                                              responseBody.time,
                                                              responseBody.exploitPoint,
                                                              responseBody.dailyExploitPoint,
                                                              responseBody.seriesMissions,
                                                              responseBody.todayMissions,
                                                              responseBody.todayTasks,
                                                              responseBody.achievementDailyPoint,
                                                              responseBody.receivedAchievementRewardNo,
                                                              responseBody.honorPoint,
                                                              responseBody.receivedVipLevelRewards,
                                                              responseBody.rankNo,
                                                              responseBody.rankRewardReceivedDate,
                                                              responseBody.rankRewardReceivedRankNo,
                                                              responseBody.rewardedAttainmentEntryNo,
                                                              responseBody.distortionScrollDailyUseCount,
                                                              responseBody.remainingDistortionTime,
                                                              responseBody.ridingCartInst,
                                                              responseBody.dailyServerLevelRakingNo,
                                                              responseBody.dailyServerLevelRanking,
                                                              responseBody.rewardedDailyServerLevelRankingNo,
                                                              responseBody.previousContinentId,
                                                              responseBody.previousNationId,
                                                              responseBody.nationInsts,
                                                              responseBody.dailyNationDonationCount,
                                                              responseBody.serverOpenDate,
                                                              responseBody.explorationPoint,
                                                              responseBody.soulPowder,
                                                              responseBody.dailyStaminaBuyCount,
                                                              responseBody.lootingItemMinGrade,
                                                              responseBody.customPresetHair,
                                                              responseBody.customFaceJawHeight,
                                                              responseBody.customFaceJawWidth,
                                                              responseBody.customFaceJawEndHeight,
                                                              responseBody.customFaceWidth,
                                                              responseBody.customFaceEyebrowHeight,
                                                              responseBody.customFaceEyebrowRotation,
                                                              responseBody.customFaceEyesWidth,
                                                              responseBody.customFaceNoseHeight,
                                                              responseBody.customFaceNoseWidth,
                                                              responseBody.customFaceMouthHeight,
                                                              responseBody.customFaceMouthWidth,
                                                              responseBody.customBodyHeadSize,
                                                              responseBody.customBodyArmsLength,
                                                              responseBody.customBodyArmsWidth,
                                                              responseBody.customBodyChestSize,
                                                              responseBody.customBodyWaistWidth,
                                                              responseBody.customBodyHipsSize,
                                                              responseBody.customBodyPelvisWidth,
                                                              responseBody.customBodyLegsLength,
                                                              responseBody.customBodyLegsWidth,
                                                              responseBody.customColorSkin,
                                                              responseBody.customColorEyes,
                                                              responseBody.customColorBeardAndEyebrow,
                                                              responseBody.customColorHair, 
                                                              responseBody.todayMissionTutorialStarted,
															  responseBody.serverMaxLevel,
															  responseBody.heroNpcShopProducts,
															  responseBody.rankActiveSkills,
															  responseBody.rankPassiveSkills,
															  responseBody.selectedRankActiveSkillId,
															  responseBody.rankActiveSkillRemainingCoolTime,
															  responseBody.rookieGiftNo,
															  responseBody.rookieGiftRemainingTime,
															  responseBody.receivedOpenGiftRewards,
															  responseBody.regDate,
															  responseBody.rewardedOpen7DayEventMissions,
															  responseBody.purchasedOpen7DayEventProducts,
															  responseBody.open7DayEventProgressCounts,
															  responseBody.retrievalProgressCounts,
															  responseBody.retrievals,
															  responseBody.taskConsignments,
															  responseBody.taskConsignmentStartCounts,
															  responseBody.rewardedLimitationGiftScheduleIds,
															  responseBody.weekendReward,
															  responseBody.paidWarehouseSlotCount,
															  responseBody.dailyDiaShopProductBuyCounts,
															  responseBody.totalDiaShopProductBuyCounts,
															  responseBody.weeklyFearAltarHalidomCollectionRewardNo,
															  responseBody.weeklyFearAltarHalidoms,
															  responseBody.weeklyRewardReceivedFearAltarHalidomElementals,
															  responseBody.open7DayEventRewarded,
															  responseBody.nationPowerRankings,
															  responseBody.potionAttrs);

			// 공유 이벤트
			CsSharingEventManager.Instance.RequestFirebaseDynamicLinkReward();
			CsSharingEventManager.Instance.RequestFirebaseDynamicLinkReceive();
			
            // 메인장비
            CsGameData.Instance.MyHeroInfo.AddHeroMainGears(responseBody.mainGears, true);
            CsGameData.Instance.MyHeroInfo.SetHeroMainGearEquipped(responseBody.equippedWeaponId);
            CsGameData.Instance.MyHeroInfo.SetHeroMainGearEquipped(responseBody.equippedArmorId);

            // 서브장비
            CsGameData.Instance.MyHeroInfo.AddHeroSubGears(responseBody.subGears, true);

            // 영웅스킬.
            CsGameData.Instance.MyHeroInfo.AddHeroSkills(responseBody.skills);

            // 메일
            CsGameData.Instance.MyHeroInfo.AddMails(responseBody.mails);

            // 보유한 탈것 목록
            CsGameData.Instance.MyHeroInfo.AddHeroMounts(responseBody.mounts, true);

            // 보유한 탈것장비 목록
            CsGameData.Instance.MyHeroInfo.AddHeroMountGears(responseBody.mountGears, true);

            // 장착한 탈것장비ID 목록
            CsGameData.Instance.MyHeroInfo.AddEquippedMountGears(responseBody.equippedMountGears, true);

            // 인벤토리
            CsGameData.Instance.MyHeroInfo.AddInventorySlots(responseBody.placedInventorySlots, true);

			// 창고
			CsGameData.Instance.MyHeroInfo.AddWarehouseSlots(responseBody.placedWarehouseSlots, true);

			// 메인퀘스트
			CsMainQuestManager.Instance.Init(responseBody.currentMainQuest);

            // 농장의 위협 퀘스트
            CsThreatOfFarmQuestManager.Instance.Init(responseBody.treatOfFarmQuest);

            // 현상금사냥꾼퀘스트
            CsBountyHunterQuestManager.Instance.Init(responseBody.bountyHunterQuest, responseBody.bountyHunterDailyStartCount, responseBody.date);

            // 낚시퀘스트
            CsFishingQuestManager.Instance.Init(responseBody.fishingQuest, responseBody.fishingQuestDailyStartCount, responseBody.date);

            // 의문의상자퀘스트
            CsMysteryBoxQuestManager.Instance.Init(responseBody.mysteryBoxQuest, responseBody.dailyMysteryBoxQuestStartCount, responseBody.date);

            // 밀서퀘스트
            CsSecretLetterQuestManager.Instance.Init(responseBody.secretLetterQuest, responseBody.dailySecretLetterQuestStartCount, responseBody.date);
            CsSecretLetterQuestManager.Instance.SecretLetterQuestTargetNationId = responseBody.secretLetterQuestTargetNationId;

            // 차원습격퀘스트
            CsDimensionRaidQuestManager.Instance.Init(responseBody.dimensionRaidQuest, responseBody.dailyDimensionRaidQuestStartCount, responseBody.date);

            // 위대한성전퀘스트
            CsHolyWarQuestManager.Instance.Init(responseBody.holyWarQuest, responseBody.dailyHolyWarQuestStartSchedules, responseBody.date);

            // 고대인의 유적
            CsGameData.Instance.AncientRelic.AncientRelicDailyPlayCount = responseBody.ancientRelicDailyPlayCount;
            CsGameData.Instance.AncientRelic.AncientRelicPlayCountDate = responseBody.date;

            // 검투대회
            CsGameData.Instance.FieldOfHonor.RewardedDailyFieldOfHonorRankingNo = responseBody.rewardedDailyFieldOfHonorRankingNo;
            CsGameData.Instance.FieldOfHonor.DailyFieldOfHonorRankingNo = responseBody.dailyFieldOfHonorRankingNo;
            CsGameData.Instance.FieldOfHonor.DailyfieldOfHonorRanking = responseBody.dailyfieldOfHonorRanking;
            CsGameData.Instance.FieldOfHonor.FieldOfHonorDailyPlayCount = responseBody.fieldOfHonorDailyPlayCount;

			// 탑승카트
			CsCartManager.Instance.Init();

            // 길드
            CsGuildManager.Instance.Init(responseBody.guild,
                                         responseBody.guildSkills,
                                         responseBody.guildMemberGrade,
                                         responseBody.totalGuildContributionPoint,
                                         responseBody.guildContributionPoint,
                                         responseBody.guildPoint,
                                         responseBody.guildRejoinRemainingTime,
                                         responseBody.guildApplications,
                                         responseBody.dailyGuildApplicationCount,
                                         responseBody.date,
                                         responseBody.dailyGuildDonationCount,
                                         responseBody.dailyGuildFarmQuestStartCount,
                                         responseBody.guildFarmQuest,
                                         responseBody.dailyGuildFoodWarehouseStockCount,
                                         responseBody.receivedGuildFoodWarehouseCollectionId,
                                         responseBody.guildMoralPoint,
                                         responseBody.guildAltarDefenseMissionRemainingCoolTime,
                                         responseBody.guildAltarRewardReceivedDate,
                                         responseBody.guildMissionQuest,
                                         responseBody.guildDailyRewardReceivedDate,
                                         responseBody.guildSupplySupportQuestPlay,
                                         responseBody.guildHuntingQuest,
                                         responseBody.dailyGuildHuntingQuestStartCount,
                                         responseBody.guildHuntingDonationDate,
                                         responseBody.guildHuntingDonationRewardReceivedDate,
                                         responseBody.guildDailyObjectiveRewardReceivedNo,
                                         responseBody.guildWeeklyObjectiveRewardReceivedDate,
                                         responseBody.guildDailyObjectiveNoticeRemainingCoolTime);

			// 국가전
			CsNationWarManager.Instance.Init(responseBody.nationWarDeclarations,
                                             responseBody.weeklyNationWarDeclarationCount,
                                             responseBody.nationWarJoined,
                                             responseBody.nationWarKillCount,
                                             responseBody.nationWarAssistCount,
                                             responseBody.nationWarDeadCount,
                                             responseBody.nationWarImmediateRevivalCount,
                                             responseBody.dailyNationWarFreeTransmissionCount,
                                             responseBody.dailyNationWarPaidTransmissionCount,
                                             responseBody.dailyNationWarCallCount,
                                             responseBody.nationWarCallRemainingCoolTime,
                                             responseBody.dailyNationWarConvergingAttackCount,
                                             responseBody.nationWarConvergingAttackRemainingCoolTime,
                                             responseBody.nationWarConvergingAttackTargetArrangeId,
                                             responseBody.nationWarMonsterInsts);

            // 보급지원
            CsSupplySupportQuestManager.Instance.Init(responseBody.supplySupportQuest);
            CsSupplySupportQuestManager.Instance.DailySupplySupportQuestCount = responseBody.dailySupplySupportQuestStartCount;

			// 도감
			CsIllustratedBookManager.Instance.Init(responseBody.illustratedBookExplorationStepNo,
                                                   responseBody.illustratedBookExplorationStepRewardReceivedDate,
                                                   responseBody.illustratedBookExplorationStepRewardReceivedStepNo,
                                                   responseBody.activationIllustratedBookIds,
                                                   responseBody.completedSceneryQuests);

            // 업적


            // 칭호
            CsTitleManager.Instance.Init(responseBody.titles,
                                         responseBody.displayTitleId,
                                         responseBody.activationTitleId);

            // 크리처카드
            CsCreatureCardManager.Instance.Init(responseBody.creatureCards,
                                                responseBody.activatedCreatureCardCollections,
                                                responseBody.creatureCardCollectionFamePoint,
                                                responseBody.purchasedCreatureCardShopFixedProducts,
                                                responseBody.creatureCardShopRandomProducts,
                                                responseBody.dailyCreatureCardShopPaidRefreshCount,
                                                responseBody.date);

            // 정예
            CsEliteManager.Instance.Init(responseBody.heroEliteMonsterKills,
                                         responseBody.spawnedEliteMonsters,
                                         responseBody.dailyEliteDungeonPlayCount,
                                         responseBody.date);

            //용맹의 증명
			CsGameData.Instance.ProofOfValor.DailyPlayCount = responseBody.dailyProofOfValorPlayCount;
			CsGameData.Instance.ProofOfValor.MyDailyFreeRefreshCount = responseBody.dailyProofOfValorFreeRefreshCount;
			CsGameData.Instance.ProofOfValor.MyDailyPaidRefreshCount = responseBody.dailyProofOfValorPaidRefreshCount;
			CsGameData.Instance.ProofOfValor.PaidRefreshCount = responseBody.poofOfValorPaidRefreshCount;
			CsGameData.Instance.ProofOfValor.BossMonsterArrangeId = responseBody.heroProofOfValorInstance.bossMonsterArrangeId;
			CsGameData.Instance.ProofOfValor.CreatureCardId = responseBody.heroProofOfValorInstance.creatureCardId;
			CsGameData.Instance.ProofOfValor.DailyPlayCountDate = responseBody.date;
			CsGameData.Instance.ProofOfValor.ProofOfValorCleared = responseBody.proofOfValorCleared;

			//영혼을 탐하는자
			CsGameData.Instance.SoulCoveter.SoulCoveterWeeklyPlayCount = responseBody.weeklySoulCoveterPlayCount;

			// 지혜의 신전
			CsGameData.Instance.WisdomTemple.DailyWisdomTemplePlayCount = responseBody.dailyWisdomTemplePlayCount;
			CsGameData.Instance.WisdomTemple.WisdomTempleCleared = responseBody.wisdomTempleCleared;

			// 유적 탈환
			CsGameData.Instance.RuinsReclaim.FreePlayCount = responseBody.dailyRuinsReclaimFreePlayCount;

            // 무한 대전
            CsGameData.Instance.InfiniteWar.DailyPlayCount = responseBody.dailyInfiniteWarPlayCount;

			// 공포의 제단
			CsGameData.Instance.FearAltar.DailyFearAltarPlayCount = responseBody.dailyFearAltarPlayCount;

            // 전쟁의 기억
            CsGameData.Instance.WarMemory.FreePlayCount = responseBody.dailyWarMemoryFreePlayCount;

            // 오시리스 방
            CsGameData.Instance.OsirisRoom.DailyPlayCount = responseBody.dailyOsirisRoomPlayCount;

            // 무역선 탈환
            CsGameData.Instance.TradeShip.PlayCount = responseBody.dailyTradeShipPlayCount;

            for (int i = 0; i < responseBody.myTradeShipBestRecords.Length; i++)
            {
                CsGameData.Instance.TradeShip.MyHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(responseBody.myTradeShipBestRecords[i]));
            }

            for (int i = 0; i < responseBody.serverTradeShipBestRecords.Length; i++)
            {
                CsGameData.Instance.TradeShip.ServerHeroTradeShipBestRecordList.Add(new CsHeroTradeShipBestRecord(responseBody.serverTradeShipBestRecords[i]));
            }

            // 앙쿠의 무덤
            CsGameData.Instance.AnkouTomb.PlayCount = responseBody.dailyAnkouTombPlayCount;

            for (int i = 0; i < responseBody.myAnkouTombBestRecords.Length; i++)
            {
                CsGameData.Instance.AnkouTomb.MyHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(responseBody.myAnkouTombBestRecords[i]));
            }

            for (int i = 0; i < responseBody.serverAnkouTombBestRecords.Length; i++)
            {
                CsGameData.Instance.AnkouTomb.ServerHeroAnkouTombBestRecordList.Add(new CsHeroAnkouTombBestRecord(responseBody.serverAnkouTombBestRecords[i]));
            }
			
			// 던전
			CsDungeonManager.Instance.Init();

			// 버프 디버프
			CsBuffDebuffManager.Instance.Init();

			// 데일리 퀘스트
			CsDailyQuestManager.Instance.Init(responseBody.dailyQuestAcceptionCount, responseBody.dailyQuestFreeRefreshCount, responseBody.dailyQuests, responseBody.date);

			// 위클리 퀘스트
			CsWeeklyQuestManager.Instance.Init(responseBody.weeklyQuest);

			// 진정한영웅 퀘스트
			CsTrueHeroQuestManager.Instance.Init(responseBody.trueHeroQuest);

			// 서브퀘스트
			CsSubQuestManager.Instance.Init(responseBody.subQuests);

			// 시련퀘스트
			CsOrdealQuestManager.Instance.Init(responseBody.ordealQuest);

			// 전기
			CsBiographyManager.Instance.Init(responseBody.biographies);

			// 친구 블랙리스트...
			CsFriendManager.Instance.Init(responseBody.friends, responseBody.tempFriends, responseBody.blacklistEntries, responseBody.deadRecords);

			// 행운상점
			CsLuckyShopManager.Instance.Init(responseBody.date, responseBody.itemLuckyShopFreePickRemainingTime, responseBody.itemLuckyShopFreePickCount, responseBody.itemLuckyShopPick1TimeCount, responseBody.itemLuckyShopPick5TimeCount,
											 responseBody.creatureCardLuckyShopFreePickRemainingTime, responseBody.creatureCardLuckyShopFreePickCount, responseBody.creatureCardLuckyShopPick1TimeCount, responseBody.creatureCardLuckyShopPick5TimeCount);

			// 축복 퀘스트 / 유망자 퀘스트
			CsBlessingQuestManager.Instance.Init(responseBody.ownerProspectQuests, responseBody.targetProspectQuests);

			// 크리쳐
			CsCreatureManager.Instance.Init(responseBody.creatures, responseBody.participatedCreatureId, responseBody.dailyCreatureVariationCount, responseBody.date);

			// 코스튬
			CsCostumeManager.Instance.Init(responseBody.costumes, 
										   responseBody.equippedCostumeId,
										   responseBody.costumeCollectionId,
										   responseBody.costumeCollectionActivated);

			// 선물
			CsPresentManager.Instance.Init(responseBody.weeklyPresentPopularityPoint, responseBody.weeklyPresentContributionPoint, responseBody.nationWeeklyPresentPopularityPointRankingNo, responseBody.nationWeeklyPresentPopularityPointRanking, responseBody.rewardedNationWeeklyPresentPopularityPointRankingNo,
										   responseBody.nationWeeklyPresentContributionPointRankingNo, responseBody.nationWeeklyPresentContributionPointRanking, responseBody.rewardedNationWeeklyPresentContributionPointRankingNo, responseBody.date);

			// 크리처농장퀘스트
			CsCreatureFarmQuestManager.Instance.Init(responseBody.dailyCreatureFarmQuestAcceptionCount, responseBody.creatureFarmQuest, responseBody.date);

			// 국가동맹
			CsNationAllianceManager.Instance.Init(responseBody.nationAlliances, responseBody.nationAllianceApplications);

			CsOpenToastManager.Instance.Init();

			// 전직퀘스트
			CsJobChangeManager.Instance.Init(responseBody.jobChangeQuest);

			// 캐쉬
			CsCashManager.Instance.Init(responseBody.cashProductPurchaseCounts,
										responseBody.firstChargeEventObjectiveCompleted,
										responseBody.firstChargeEventRewarded,
										responseBody.rechargeEventAccUnOwnDia,
										responseBody.rechargeEventRewarded,
										responseBody.chargeEvent,
										responseBody.dailyChargeEventAccUnOwnDia,
										responseBody.rewardedDailyChargeEventMissions,
										responseBody.consumeEvent,
										responseBody.dailyConsumeEventAccDia,
										responseBody.rewardedDailyConsumeEventMissions,
										responseBody.date);

			// 별자리
			CsConstellationManager.Instance.Init(responseBody.constellations,
												 responseBody.starEssense,
												 responseBody.dailyStarEssenseItemUseCount,
												 responseBody.date);

			// 아티팩트
			CsArtifactManager.Instance.Init(responseBody.artifactNo,
											responseBody.artifactLevel,
											responseBody.artifactExp,
											responseBody.equippedArtifactNo);

			// 전투력 업데이트.
			CsGameData.Instance.MyHeroInfo.UpdateBattlePower(true);

			//if (CsGameData.Instance.HuntingDungeon.LocationId == responseBody.initEntranceLocationId)
			//{
			//	CsGameData.Instance.HuntingDungeon.CurrentFloor = responseBody.initEntranceLocationParam;
			//}

			StartCoroutine(LoadSceneMainUICoroutine());

            CsGameData.Instance.MyHeroInfo.IsCreateEnter = m_bIsCreateEnter;
        }
        else if (nReturnCode == 101)
        {
            if (m_iEnumerator != null)
            {
                StopCoroutine(m_iEnumerator);
            }

            m_iEnumerator = FadeIn();
            StartCoroutine(m_iEnumerator);

            Debug.Log("ERROR (OnEventResHeroLogin) : 101 - 해당 영웅이 존재하지 않습니다.");
            //OpenPopupError(2);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00601"), OnClickGameStartError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));

            m_buttonGameStart.interactable = true;
        }
        else if (nReturnCode == 102)
        {
            if (m_iEnumerator != null)
            {
                StopCoroutine(m_iEnumerator);
            }

            m_iEnumerator = FadeIn();
            StartCoroutine(m_iEnumerator);

            Debug.Log("ERROR (OnEventResHeroLogin) : 102 - 영웅 생성이 완료되지 않았습니다.");
            //OpenPopupError(3);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_ERROR_00602"), OnClickGameStartError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));

            m_buttonGameStart.interactable = true;
        }
        else
        {
            if (m_iEnumerator != null)
            {
                StopCoroutine(m_iEnumerator);
            }

            m_iEnumerator = FadeIn();
            StartCoroutine(m_iEnumerator);

            Debug.Log("ERROR (OnEventResHeroLogin) : " + nReturnCode);
            //OpenPopupError(2, nReturnCode);
            m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_00101"), nReturnCode), OnClickGameStartError, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));

            if (m_buttonGameStart != null) m_buttonGameStart.interactable = true;
        }

        CsRplzSession.Instance.EventResHeroLogin -= OnEventResHeroLogin;
    }

#region Announcement

	//---------------------------------------------------------------------------------------------------
	// 공지사항목록
	public void SendAnnouncementList()
	{
		AnnouncementsASCommand cmd = new AnnouncementsASCommand();
		cmd.Finished += ResponseAnnouncementList;
		cmd.Run();
	}

	void ResponseAnnouncementList(object sender, EventArgs e)
	{
		AnnouncementsASCommand cmd = (AnnouncementsASCommand)sender;

		if (!cmd.isOK)
		{
			m_csPanelModal.Choice("AnnouncementsASCommand : " + cmd.error.Message, null, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		AnnouncementsASResponse res = (AnnouncementsASResponse)cmd.response;

		if (res.isOK)
		{
			m_listCsAnnouncement.Clear();
			m_listCsAnnouncement = res.AnnouncementList;

			if (m_listCsAnnouncement.Count > 0)
			{ 
				OpenPopupNotice();
			}
			else
			{
				if (m_bAnnounceClick)
				{
					m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A93_TXT_00002"), CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
				}
			}
		}
		else
		{
			m_csPanelModal.Choice("AnnouncementsASResponse : " + res.errorMessage, null, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}

		m_bAnnounceClick = false;
	}

#endregion Announcement


	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------
	//---------------------------------------------------------------------------------------------------

#endregion

#region 초기화
	//---------------------------------------------------------------------------------------------------
	void InitializeUI()
    {
		m_trCanvas.Find("Logo").gameObject.SetActive(true);

        //임시
        Text textLanguage = m_trCanvas.Find("Lobby/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLanguage);
        textLanguage.text = Application.systemLanguage.ToString();
        //트레일러
        Text textButtonSkip = m_trCanvas.Find("Trailer/ButtonSkip/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonSkip);
        textButtonSkip.text = CsConfiguration.Instance.GetString("A01_BTN_00001");

        //약관동의
        Text textTermsPopupName = m_trCanvas.Find("Terms/ImageBackground/TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTermsPopupName);
        textTermsPopupName.text = CsConfiguration.Instance.GetString("A01_TXT_00046");

        Toggle toggle1 = m_trCanvas.Find("Terms/ImageBackground/Toggle1").GetComponent<Toggle>();
        toggle1.isOn = false;
        toggle1.onValueChanged.RemoveAllListeners();
        toggle1.onValueChanged.AddListener((isOn) => OnValueChangedAgree(toggle1, 1));
        toggle1.onValueChanged.AddListener((isOn) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textName1 = toggle1.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName1);
        textName1.text = CsConfiguration.Instance.GetString("A01_NAME_00001");

        Button buttonView1 = toggle1.transform.Find("ButtonView").GetComponent<Button>();
        buttonView1.onClick.RemoveAllListeners();
        buttonView1.onClick.AddListener(() => OnClickView(1));
        buttonView1.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Toggle toggle2 = m_trCanvas.Find("Terms/ImageBackground/Toggle2").GetComponent<Toggle>();
        toggle2.isOn = false;
        toggle2.onValueChanged.RemoveAllListeners();
        toggle2.onValueChanged.AddListener((isOn) => OnValueChangedAgree(toggle2, 2));
        toggle2.onValueChanged.AddListener((isOn) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        Text textName2 = toggle2.transform.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName2);
        textName2.text = CsConfiguration.Instance.GetString("A01_NAME_00002");

        Button buttonView2 = toggle2.transform.Find("ButtonView").GetComponent<Button>();
        buttonView2.onClick.RemoveAllListeners();
        buttonView2.onClick.AddListener(() => OnClickView(2));
        buttonView2.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonAllSelect = m_trCanvas.Find("Terms/ImageBackground/ButtonAllSelect").GetComponent<Button>();
        buttonAllSelect.onClick.RemoveAllListeners();
        buttonAllSelect.onClick.AddListener(() => OnClickAllSelect(toggle1, toggle2));
        buttonAllSelect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textAllSelect = buttonAllSelect.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAllSelect);
        textAllSelect.text = CsConfiguration.Instance.GetString("A01_BTN_00026");

        Button buttonNext = m_trCanvas.Find("Terms/ImageBackground/ButtonNext").GetComponent<Button>();
        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(OnClickNext);
        buttonNext.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonNext.interactable = false;

        Text textNext = buttonNext.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNext);
        textNext.text = CsConfiguration.Instance.GetString("A01_BTN_00009");

        //Text textLeftLabel = m_trCanvas.Find("Terms/LeftDisplay/ToggleLeft/Label").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textLeftLabel);
        //textLeftLabel.text = CsConfiguration.Instance.GetString("A01_BTN_00002");

        //Text textRightLabel = m_trCanvas.Find("Terms/RightDisplay/ToggleRight/Label").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textRightLabel);
        //textRightLabel.text = CsConfiguration.Instance.GetString("A01_BTN_00002");

        //로딩
        //Text textPercent = m_trCanvas.Find("ResourcesLoad/Slider/TextPercent").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textPercent);
        //textPercent.text = "";

        Text textState = m_trCanvas.Find("ResourcesLoad/Slider/TextState").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState);
        textState.text = "";

        //서버선택팝업
        Text textServerName = m_trCanvas.Find("Popup/PopupServer/ImageBackground/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textServerName);
        textServerName.text = CsConfiguration.Instance.GetString("A01_NAME_00004");

        Text textRecentServer = m_trCanvas.Find("Popup/PopupServer/ImageBackground/RightDisplay/Display0/TextRecentServer").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRecentServer);
        textRecentServer.text = CsConfiguration.Instance.GetString("A01_TXT_00005");

		Text textRetentionServer = m_trCanvas.Find("Popup/PopupServer/ImageBackground/RightDisplay/Display0/TextRetentionServer").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRetentionServer);
        textRetentionServer.text = CsConfiguration.Instance.GetString("A01_TXT_00006");

        //캐릭터생성
        Text textCerateNext = m_trCanvas.Find("CharacterCreate/ButtonNext/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCerateNext);
        textCerateNext.text = CsConfiguration.Instance.GetString("A01_BTN_00010");

        Text textCerateClose = m_trCanvas.Find("CharacterCreate/ButtonClose/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCerateClose);
        textCerateClose.text = CsConfiguration.Instance.GetString("A01_BTN_00011");

        //커스터마이징
        m_trCharacterCustomizing = m_trCanvas.Find("CharacterCustomizing");

        //국가선택
        Text textSelectClose = m_trCanvas.Find("CountrySelect/ButtonClose/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSelectClose);
        textSelectClose.text = CsConfiguration.Instance.GetString("A01_NAME_00006");

        Text textButtonCreateCharacter = m_trCanvas.Find("CountrySelect/ButtonCreateCharacter/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCreateCharacter);
        textButtonCreateCharacter.text = CsConfiguration.Instance.GetString("A01_BTN_00011");

        //임시튜토리얼
        Text textTutorialSkip = m_trCanvas.Find("Tutorial/ButtonSkip/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTutorialSkip);
        textTutorialSkip.text = CsConfiguration.Instance.GetString("A01_BTN_00001");

        //체크팝업
        Text textButtonLayoutNo = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonLayoutNo);
        textButtonLayoutNo.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_NO");

        Text textButtonLayoutOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonLayoutOk);
        textButtonLayoutOk.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_YES");

        //캐릭터선택창
        Text textCharacterSelectClose = m_trCanvas.Find("CharacterSelect/ButtonClose/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCharacterSelectClose);
        textCharacterSelectClose.text = CsConfiguration.Instance.GetString("A01_NAME_00005");

        Text textButtonDeleteCharacter = m_trCanvas.Find("CharacterSelect/ButtonDeleteCharacter/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDeleteCharacter);
        textButtonDeleteCharacter.text = CsConfiguration.Instance.GetString("A01_BTN_00012");

        Text textButtonStart = m_trCanvas.Find("CharacterSelect/ButtonStart/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonStart);
        textButtonStart.text = CsConfiguration.Instance.GetString("A01_BTN_00013");

        m_trCanvas.Find("Trailer").gameObject.SetActive(false);
        m_trCanvas.Find("Terms").gameObject.SetActive(false);
        m_trCanvas.Find("ResourcesLoad").gameObject.SetActive(false);
        m_trCanvas.Find("Lobby").gameObject.SetActive(false);
        m_trCanvas.Find("CharacterCreate").gameObject.SetActive(false);
        m_trCanvas.Find("CharacterSelect").gameObject.SetActive(false);
        m_trCanvas.Find("CountrySelect").gameObject.SetActive(false);
        m_trCanvas.Find("Tutorial").gameObject.SetActive(false);
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);
        m_trCanvas.Find("Popup/PopupServer").gameObject.SetActive(false);
		m_trCanvas.Find("Popup/PopupNotice").gameObject.SetActive(false);
    }

    int m_nTemp = 0;

    void OnValueChangedAgree(Toggle toggle, int nType)
    {
        if (toggle.isOn)
        {
            m_nTemp++;
        }
        else
        {
            m_nTemp--;
        }

        Button buttonNext = m_trCanvas.Find("Terms/ImageBackground/ButtonNext").GetComponent<Button>();

        if (m_nTemp == 2)
        {
            buttonNext.interactable = true;
        }
        else
        {
            buttonNext.interactable = false;
        }

    }

    void OnClickNext()
    {
		CsConfiguration.Instance.SendFirebaseLogEvent("agreement_submit");
        RequestGameAssetBundles();
    }

    void OnClickAllSelect(Toggle toggle1, Toggle toggle2)
    {
        if (!toggle1.isOn)
            toggle1.isOn = true;

        if (!toggle2.isOn)
            toggle2.isOn = true;
    }

    void OnClickView(int nType)
    {
        if (nType == 1)
        {
            Application.OpenURL(CsConfiguration.Instance.SystemSetting.TermsOfServiceUrl);
        }
        else
        {
            Application.OpenURL(CsConfiguration.Instance.SystemSetting.PrivacyPolicyUrl);
        }
    }

#endregion

#region 로비
    //---------------------------------------------------------------------------------------------------
    void OpenPopupMainLobby(bool Login)
    {
        PlayBGM(EnSoundTypeIntro.Main);

        m_trLightList.gameObject.SetActive(false);

        if (m_trLoading.gameObject.activeSelf)
        {
            m_trLoading.gameObject.SetActive(false);
        }

        Scene scene = SceneManager.GetSceneByName("IntroLobby");

        if (scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
        }

        // 그래픽 퀄리티 세팅
        QualitySettings.shadowDistance = 150;

        Transform trLobby = m_trCanvas.Find("Lobby");
        trLobby.Find("ImageGuard").gameObject.SetActive(true);
        m_bIsTouched = false;
        //BGM 재생


        //		Animator ani = m_trIntroRobby.Find("Elkasia/FX/WaterAnim").GetComponent<Animator>();
        //		ani.SetTrigger("Reset");	

        m_trIntroRobby.gameObject.SetActive(true);
        m_trCanvas.Find("ResourcesLoad").gameObject.SetActive(false);

        Text textVersion = m_trCanvas.Find("Lobby/TextVersion").GetComponent<Text>();
        CsUIData.Instance.SetFont(textVersion);
        textVersion.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01002"), CsConfiguration.Instance.ClientVersion);
        //textVersion.text = m_strBuildDate;

        Text textWarning = m_trCanvas.Find("Lobby/ImageWarning/TextWarning").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWarning);
        textWarning.text = CsConfiguration.Instance.GetString("A01_TXT_00044");

        trLobby.gameObject.SetActive(true);
        CanvasGroup canvasGroup = trLobby.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;

        if (Login)
        {
            m_trCanvas.Find("Lobby/AfterLogin").gameObject.SetActive(true);

			// 설치 후 첫 접속이 아니거나 기존에 영웅이 존재하던 계정으로 처음 접속하는 경우
			// 최근 접속 서버 설정
			if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFirstConnect) ||
				(m_listCsCsUserHero != null && m_listCsCsUserHero.Count > 0))
			{
				if (m_user.LastVirtualGameServer1 != 0)
				{
					m_csGameServerSelected = GetGameServer(m_user.LastVirtualGameServer1);
				}

				Text textButtonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textButtonLoginServer);
                textButtonLoginServer.text = m_csGameServerSelected != null ? m_csGameServerSelected.Name : "";

				if (!PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFirstConnect))
				{
					PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFirstConnect, 1);
					PlayerPrefs.Save();
				}
			}
			else
			{
				Text textButtonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textButtonLoginServer);
				textButtonLoginServer.text = "";
			}
            
            Button buttonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer").GetComponent<Button>();
            buttonLoginServer.onClick.RemoveAllListeners();
            buttonLoginServer.onClick.AddListener(OpenPopupSelectServer);
            buttonLoginServer.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Button buttonLogin = m_trCanvas.Find("Lobby/AfterLogin/ButtonLogin").GetComponent<Button>();
            buttonLogin.onClick.RemoveAllListeners();
            buttonLogin.onClick.AddListener(OnClickOpenPopupLogin);
            buttonLogin.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Button buttonNotice = m_trCanvas.Find("Lobby/AfterLogin/ButtonNotice").GetComponent<Button>();
            buttonNotice.onClick.RemoveAllListeners();
			buttonNotice.onClick.AddListener(OnClickOpenPopupNotice);
            buttonNotice.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Button buttonSetting = m_trCanvas.Find("Lobby/AfterLogin/ButtonSetting").GetComponent<Button>();
            buttonSetting.onClick.RemoveAllListeners();
            buttonSetting.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Button buttonTouch = m_trCanvas.Find("Lobby/AfterLogin/ButtonTouch").GetComponent<Button>();
            buttonTouch.onClick.RemoveAllListeners();
            buttonTouch.onClick.AddListener(OnClickCharacterCheck);
            buttonTouch.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            Text textTouch = buttonTouch.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textTouch);
            textTouch.text = CsConfiguration.Instance.GetString("A01_TXT_00001");
        }
        else
        {
            m_trCanvas.Find("Lobby/AfterLogin").gameObject.SetActive(false);
            Text textButtonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonLoginServer);
            textButtonLoginServer.text = CsConfiguration.Instance.GetString("A01_BTN_00003");

            Button buttonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer").GetComponent<Button>();
            buttonLoginServer.onClick.RemoveAllListeners();
            buttonLoginServer.onClick.AddListener(OnClickOpenPopupLogin);
            buttonLoginServer.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        trLobby.Find("ImageGuard").gameObject.SetActive(false);

        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

		m_iEnumerator = FadeIn();
        StartCoroutine(m_iEnumerator);

		if (CsUIData.Instance.IntroShortCutType == EnIntroShortCutType.Logo)
		{
			SendAnnouncementList();
		}
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartGameCoroutine(UnityAction end)
    {
        yield return new WaitForSeconds(0.5f);
        CanvasGroup canvasGroup = m_trCanvas.Find("Lobby").GetComponent<CanvasGroup>();
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 1.5f;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        end();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupMainLobby()
    {
        m_trCanvas.Find("Lobby").gameObject.SetActive(false);
        m_trIntroRobby.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCharacterCheck()
    {
		if (m_csGameServerSelected == null)
		{
			OpenPopupSelectServer();
		}
		else
		{
			if (!PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyFirstConnect))
			{
				PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFirstConnect, 1);
				PlayerPrefs.Save();
			}

			if (!m_bIsTouched)
			{
				m_bIsTouched = true;

				CharacterCheck();
			}
		}
    }

    //---------------------------------------------------------------------------------------------------
    void CharacterCheck()
    {
       // StartCoroutine(StartGameCoroutine(ConnectGameServerServer));
      //  Animator ani = m_trIntroRobby.Find("Elkasia/FX/WaterAnim").GetComponent<Animator>();
      //  ani.SetTrigger("Go");

        Debug.Log("CharacterCheck()");
        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeOut(ConnectGameServerServer);

        StartCoroutine(m_iEnumerator);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupLogin()
    {
        Transform trPpopupLogin = m_trCanvas.Find("Popup/PopupLogin");

        if (m_bIsTouched)
        {
            return;
        }

        if (m_bIsFirstPopupLogin)
        {
            //이니셜라이즈
            Text textPopupName = trPpopupLogin.Find("TextLogin").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = CsConfiguration.Instance.GetString("A01_TXT_00043");

            Button buttonClose = trPpopupLogin.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(OnClickClosePopupLogin);
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Transform trButtonList = trPpopupLogin.Find("ButtonList");

            Button buttonGuest = trButtonList.Find("ButtonGuest").GetComponent<Button>();
            buttonGuest.onClick.RemoveAllListeners();
            buttonGuest.onClick.AddListener(OnClickLoginGuest);
            buttonGuest.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textGuset = buttonGuest.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGuset);
            textGuset.text = CsConfiguration.Instance.GetString("A01_BTN_00006");

            Button buttonGoogle = trButtonList.Find("ButtonGoogle").GetComponent<Button>();
            buttonGoogle.onClick.RemoveAllListeners();
            buttonGoogle.onClick.AddListener(OnClickLoginGoogle);
            buttonGoogle.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textGoogle = buttonGoogle.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGoogle);
            textGoogle.text = CsConfiguration.Instance.GetString("A01_BTN_00005");

            Button buttonFacebook = trButtonList.Find("ButtonFacebook").GetComponent<Button>();
            buttonFacebook.onClick.RemoveAllListeners();
            buttonFacebook.onClick.AddListener(OnClickLoginFacebook);
            buttonFacebook.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textFacebook = buttonFacebook.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textFacebook);
            textFacebook.text = CsConfiguration.Instance.GetString("A01_BTN_00004");

            Button buttonPublisher = trButtonList.Find("ButtonPublisher").GetComponent<Button>();
            buttonPublisher.onClick.RemoveAllListeners();
            buttonPublisher.onClick.AddListener(OnClickLoginPublish);
            buttonPublisher.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textPublisher = buttonPublisher.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPublisher);
            textPublisher.text = CsConfiguration.Instance.GetString("A01_BTN_00025");

            m_bIsFirstPopupLogin = false;

            //게스트 제외하고 버튼막기(임시)
            //buttonGoogle.gameObject.SetActive(false);
            //buttonFacebook.gameObject.SetActive(false);
            buttonPublisher.gameObject.SetActive(false);
        }
        trPpopupLogin.gameObject.SetActive(true);

		CsConfiguration.Instance.SendFirebaseLogEvent("login_start");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupLogin()
    {
        Transform trPpopupLogin = m_trCanvas.Find("Popup/PopupLogin");
        trPpopupLogin.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLoginFacebook()
    {
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			m_csPanelModal.Choice("유니티 에디터에서는 사용할 수 없습니다.", null, "OK");
			return;
		}

        RequestLoginWithFacebook();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLoginGoogle()
    {
		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			m_csPanelModal.Choice("유니티 에디터에서는 사용할 수 없습니다.", null, "OK");
			return;
		}

		RequestLoginWithGoogle();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLoginPublish()
    {

    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLoginGuest()
    {
        RequestLoginWithGuest();
    }

	bool m_bAnnounceClick = false; 
    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupNotice()
    {
        if (m_bIsTouched)
        {
            return;
        }

		m_bAnnounceClick = true;
		SendAnnouncementList();
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickClosePopupNotice()
	{
		Transform trContent = m_trCanvas.Find("Popup/PopupNotice/Content");
		CsNoticeWebView noticeWebView = trContent.GetComponent<CsNoticeWebView>();
		noticeWebView.Close();
		trContent.gameObject.SetActive(false);

		Transform trPpopupNotice = m_trCanvas.Find("Popup/PopupNotice");
		trPpopupNotice.gameObject.SetActive(false);
	}

#region 공지사항 팝업
	//---------------------------------------------------------------------------------------------------
	void OpenPopupNotice()
	{
		Transform trPopupNotice = m_trCanvas.Find("Popup/PopupNotice");
		Transform trImageBackground = trPopupNotice.Find("ImageBackground");
		Transform trToggleList = trImageBackground.Find("ScrollViewToggle/Viewport/ToggleList");

		if (m_bIsFirstPopupNotice)
		{
			Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
			CsUIData.Instance.SetFont(textPopupName);
			textPopupName.text = CsConfiguration.Instance.GetString("A93_TXT_00001");

			Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
			buttonClose.onClick.RemoveAllListeners();
			buttonClose.onClick.AddListener(OnClickClosePopupNotice);
			buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

			m_bIsFirstPopupNotice = false;
		}

		// 토글 전체 비활성화
		for (int i = 0; i < trToggleList.childCount; i++)
		{
			Toggle toggle = trToggleList.GetChild(i).GetComponent<Toggle>();
			toggle.onValueChanged.RemoveAllListeners();
			toggle.isOn = false;

			trToggleList.GetChild(i).gameObject.SetActive(false);
		}

		// 공지사항 체크
		for (int i = 0; i < m_listCsAnnouncement.Count; i++)
		{
			int nNoticeIndex = i;
			CsAnnouncement csAnnouncement = m_listCsAnnouncement[i];

			// 날짜에 맞는 공지사항이 있는 경우 토글 세팅
			//if (csAnnouncement.StartTime >= m_dtoServerTime.DateTime &&
			//	m_dtoServerTime.DateTime < csAnnouncement.EndTime)
			//{
				Transform trToggle;

				if (nNoticeIndex < trToggleList.childCount)
				{
					trToggle = trToggleList.GetChild(i);
				}
				else
				{
					trToggle = Instantiate(trToggleList.GetChild(0));
					trToggle.name = "ToggleNotice" + nNoticeIndex;
					trToggle.SetParent(trToggleList);

					RectTransform rtrToggle = trToggle.GetComponent<RectTransform>();
					rtrToggle.anchoredPosition3D = Vector3.zero;
					rtrToggle.localScale = Vector3.one;
				}

				if (trToggle != null)
				{
					Transform trText = trToggle.Find("Text");
					Text textToggle = trText.GetComponent<Text>();
					CsUIData.Instance.SetFont(textToggle);
					textToggle.text = csAnnouncement.Title;

					Toggle toggle = trToggle.GetComponent<Toggle>();
					toggle.onValueChanged.RemoveAllListeners();
					toggle.onValueChanged.AddListener((isOn) => OnValueChangedPopupNoticeToggle(toggle, nNoticeIndex));

					trToggle.gameObject.SetActive(true);
				}
			//}
		}

		trPopupNotice.gameObject.SetActive(true);

		// 공지사항이 있는 경우 첫 번째 토글 선택
		if (m_listCsAnnouncement.Count > 0)
		{
			Toggle toggle = trToggleList.GetChild(0).GetComponent<Toggle>();
			toggle.isOn = true;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedPopupNoticeToggle(Toggle toggle, int nIndex)
	{
		if (toggle.isOn)
		{
			ShowNotice(nIndex);

			if (!m_bMute)
			{
				CsUIData.Instance.PlayUISound(EnUISoundType.Toggle);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ShowNotice(int nIndex) // 선택한 공지사항 토글에 따라 공지사항 웹페이지 로드
	{
		Transform trContent = m_trCanvas.Find("Popup/PopupNotice/Content");
		trContent.gameObject.SetActive(true);

		CsNoticeWebView noticeWebView = trContent.GetComponent<CsNoticeWebView>();
		noticeWebView.Show(m_listCsAnnouncement[nIndex].ContentUrl, (int)CsUIData.Instance.DeviceResolution.x, (int)CsUIData.Instance.DeviceResolution.y);
	}


#endregion

	/*
#region 시스템(게임옵션) 탭 토글들

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMaxFrame(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyFrame, nIndex);
            PlayerPrefs.Save();

            switch (nIndex)
            {
                case 0:
                    Application.targetFrameRate = 20;
                    break;

                case 1:
                    Application.targetFrameRate = 30;
                    break;

                case 2:
                    Application.targetFrameRate = 45;
                    break;
            }

            if (!m_bMute)
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.Toggle);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedGraphic(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyGraphic, nIndex);
            PlayerPrefs.Save();

            if (!m_bMute)
            {
                PlayUISound(EnUISoundTypeIntro.Button);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedResolution(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyResolution, nIndex);
            PlayerPrefs.Save();

            if (!m_bMute)
            {
                PlayUISound(EnUISoundTypeIntro.Button);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedEffact(Toggle toggle, int nIndex)
    {
        if (toggle.isOn)
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyEffactVisible, nIndex);
            PlayerPrefs.Save();
            //CsGameEventToIngame.Instance.OnEventSetOptionEffactVisible();

            if (!m_bMute)
            {
                PlayUISound(EnUISoundTypeIntro.Button);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenPopupHelp(Button button, string strName, string strDesc)
    {
        Transform trHelp = m_trCanvas.Find("Popup/Setting/ButtonHelpPopup");
        trHelp.gameObject.SetActive(true);
        Transform trBack = trHelp.Find("ImageHelpPopupBackground");
        trBack.SetParent(button.transform);
        RectTransform rectTransform = trBack.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
        trBack.SetParent(trHelp);

        Text textName = trBack.Find("TextOptionName").GetComponent<Text>();
        textName.text = strName;
        Text textDesc = trBack.Find("TextHelp").GetComponent<Text>();
        textDesc.text = strDesc;

    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseSetting()
    {
        Transform trSetting = m_trCanvas.Find("Popup/Setting");
        trSetting.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseHelpPopup()
    {
        Transform trButtonHelpPopup = m_trCanvas.Find("Popup/Setting/ButtonHelpPopup");
        trButtonHelpPopup.gameObject.SetActive(false);
    }

#endregion
    */

#region 서버선택 팝업
    //---------------------------------------------------------------------------------------------------
    void OpenPopupSelectServer()
    {
        if (m_bIsTouched)
        {
            return;
        }

		if (m_trPopupServer == null)
		{
			m_trPopupServer = m_trCanvas.Find("Popup/PopupServer");
		}

        m_trPopupServer.gameObject.SetActive(true);

		Text textWelcome = m_trPopupServer.Find("TextWelcome").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWelcome);
        textWelcome.text = CsConfiguration.Instance.GetString("A01_TXT_00045");

		Transform trBack = m_trPopupServer.Find("ImageBackground");

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickClosePopupSelectServer());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textState1 = trBack.Find("TextState1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState1);
        textState1.text = CsConfiguration.Instance.GetString("A01_TXT_00002");

        Text textState2 = trBack.Find("TextState2").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState2);
        textState2.text = CsConfiguration.Instance.GetString("A01_TXT_00003");

        Text textState3 = trBack.Find("TextState3").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState3);
        textState3.text = CsConfiguration.Instance.GetString("A01_TXT_00004");

        Text textState4 = trBack.Find("TextState4").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState4);
        textState4.text = CsConfiguration.Instance.GetString("A01_TXT_00042");

		SetServerPopupRegionLists();

		Text textPreparing = trBack.Find("ImageIconPreparing/TextPreparing").GetComponent<Text>();
		CsUIData.Instance.SetFont(textPreparing);
		textPreparing.text = CsConfiguration.Instance.GetString("A01_TXT_00056");
		
		////오른쪽 디스플레이 설정
		//SetServerRightDisplay(0);
    }

	//---------------------------------------------------------------------------------------------------
	void SetServerPopupRegionLists()
	{
		Transform trContent = m_trPopupServer.Find("ImageBackground/ImageFrameServerRegion/Viewport/Content");
		ToggleGroup toggleGroup = trContent.GetComponent<ToggleGroup>();

		for (int i = 0; i < trContent.childCount; i++)
		{
			trContent.GetChild(i).gameObject.SetActive(false);
			trContent.GetChild(i).name = "";
		}

		GameObject goServerRegionToggle = CsUIData.Instance.LoadAsset<GameObject>("GUI/Intro/ServerRegionToggle");

		int nChildIndex = 0;

		foreach (CsGameServerRegion csGameServerRegion in m_listCsGameServerRegion)
		{
			Transform trToggleServerRegion = null;

			if (nChildIndex < trContent.childCount)
			{
				trToggleServerRegion = trContent.GetChild(nChildIndex);
				trToggleServerRegion.gameObject.SetActive(true);
			}
			else
			{
				trToggleServerRegion = Instantiate(goServerRegionToggle, trContent).transform;
			}

			if (trToggleServerRegion != null)
			{
				trToggleServerRegion.name = csGameServerRegion.RegionId.ToString();

				Text textRegionName = trToggleServerRegion.Find("TextRegionName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textRegionName);
				textRegionName.text = csGameServerRegion.Name;

				Toggle toggleRegion = trToggleServerRegion.GetComponent<Toggle>();
				toggleRegion.onValueChanged.RemoveAllListeners();
				toggleRegion.onValueChanged.AddListener((bIsOn) => OnValueChangedToggleRegion(bIsOn, csGameServerRegion, textRegionName));
				toggleRegion.group = toggleGroup;
			}

			nChildIndex++;
		}

		if (trContent.childCount > 0)
		{
			CsGameServer csGameServer = GetGameServer(m_user.LastVirtualGameServer1);
			CsGameServerGroup csGameServerGroup = null;
			CsGameServerRegion csGameServerRegion = null;
			Transform trRegionToggle = null;

			bool bSelectedServerGroup = false;

			if (csGameServer != null)
			{
				csGameServerGroup = GetGameServerGroup(csGameServer.GroupId);

				if (csGameServerGroup != null)
				{
					csGameServerRegion = m_listCsGameServerRegion.Find(region => region.RegionId == csGameServerGroup.RegionId);

					if (csGameServerRegion != null)
					{
						trRegionToggle = trContent.Find(csGameServerRegion.RegionId.ToString());

						if (trRegionToggle != null)
						{
							// 최근 서버(첫 접속 시에는 서버에서 보내주는 추천 서버)가 속한 서버 지역 선택
							trRegionToggle.GetComponent<Toggle>().isOn = true;
							bSelectedServerGroup = true;
						}
					}
				}
			}

			// 선택된 서버가 없을 경우
			if (!bSelectedServerGroup)
			{
				trContent.GetChild(0).GetComponent<Toggle>().isOn = true;
			}
		}
		else
		{
			DisplayServerPreparingMessage(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnValueChangedToggleRegion(bool bIsOn, CsGameServerRegion csGameServerRegion, Text text)
	{
		if (bIsOn)
		{
			CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			text.color = new Color32(255, 255, 255, 255);

			m_nSelectedServerRegionId = csGameServerRegion.RegionId;

			SetServerPopupGroupLists(csGameServerRegion);
		}
		else
		{
			text.color = CsUIData.Instance.ColorGray;
		}
	}

	//---------------------------------------------------------------------------------------------------
	// 서버 준비 중 화면
	void DisplayServerPreparingMessage(bool bDisplay)
	{
		m_trPopupServer.Find("ImageBackground/LeftToggles").gameObject.SetActive(!bDisplay);
		m_trPopupServer.Find("ImageBackground/RightDisplay").gameObject.SetActive(!bDisplay);
		m_trPopupServer.Find("ImageBackground/ImageIconPreparing").gameObject.SetActive(bDisplay);
	}

	//---------------------------------------------------------------------------------------------------
	void SetServerPopupGroupLists(CsGameServerRegion csGameServerRegion)
    {
		if (csGameServerRegion.GameServerGroupList.Count > 0)
		{
			DisplayServerPreparingMessage(false);
		}
		else
		{
			DisplayServerPreparingMessage(true);
			return;
		}

		Transform trContents = m_trPopupServer.Find("ImageBackground/LeftToggles/Scroll View/Viewport/Content");
		ToggleGroup toggleGroup = trContents.GetComponent<ToggleGroup>();

		// 내 서버
        Text textMyServerName = trContents.Find("ToggleMyServer/TextServerName").GetComponent<Text>();
		CsUIData.Instance.SetFont(textMyServerName);
		textMyServerName.text = CsConfiguration.Instance.GetString("A01_BTN_00007");

        trContents.Find("ToggleMyServer/ImageLabel").gameObject.SetActive(false);

        Toggle toggleMyServer = trContents.Find("ToggleMyServer").GetComponent<Toggle>();
		toggleMyServer.group = toggleGroup;
        toggleMyServer.onValueChanged.RemoveAllListeners();
		toggleMyServer.onValueChanged.AddListener((ison) => OnValueChangedServerGroupToggle(toggleMyServer));
		
		// 전체 서버
		for (int i = 1; i < trContents.childCount; i++)
		{
			trContents.GetChild(i).gameObject.SetActive(false);
			trContents.GetChild(i).name = "";
		}

        GameObject goServerGroupToggle = Resources.Load<GameObject>("GUI/Intro/ServerPopupLeftToggles");

		int nChildIndex = 1;

		foreach (CsGameServerGroup csGameServerGroup in csGameServerRegion.GameServerGroupList)
		{
			Transform trToggle = null;

			if (nChildIndex < trContents.childCount)
			{
				trToggle = trContents.GetChild(nChildIndex);
				trToggle.gameObject.SetActive(true);
			}
			else
			{
				trToggle = Instantiate(goServerGroupToggle, trContents).transform;
			}

			if (trToggle != null)
			{
				trToggle.name = csGameServerGroup.GroupId.ToString();

				Text textServerName = trToggle.Find("TextServerName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textServerName);
				textServerName.text = csGameServerGroup.Name;

				trToggle.Find("ImageLabel").gameObject.SetActive(csGameServerGroup.Recommendable);

				Text textRecommend = trToggle.Find("ImageLabel/Text").GetComponent<Text>();
				CsUIData.Instance.SetFont(textRecommend);
				textRecommend.text = CsConfiguration.Instance.GetString("A01_TXT_00054");
				
				Toggle toggle = trToggle.GetComponent<Toggle>();
				toggle.group = toggleGroup;
				toggle.onValueChanged.RemoveAllListeners();
				toggle.onValueChanged.AddListener((ison) => OnValueChangedServerGroupToggle(toggle, csGameServerGroup.GroupId));
			}

			nChildIndex++;
		}

		CsGameServer lastVirtualGameServer = GetGameServer(m_user.LastVirtualGameServer1);
		CsGameServerGroup lastGameServerGroup = null;

		Transform trGroupToggle = null;

		bool bSelectedServerGroup = false;

		if (lastVirtualGameServer != null)
		{
			lastGameServerGroup = GetGameServerGroup(lastVirtualGameServer.GroupId);

			if (lastGameServerGroup != null)
			{
				// 첫 접속인 경우 추천 서버가 속한 그룹 선택
				if (m_csGameServerSelected == null)
				{
					trGroupToggle = trContents.Find(lastGameServerGroup.GroupId.ToString());

					if (trGroupToggle != null)
					{
						for (int i = 0; i < trContents.childCount; i++)
						{
							trContents.GetChild(i).GetComponent<Toggle>().isOn = false;
						}

						trGroupToggle.GetComponent<Toggle>().isOn = true;
						bSelectedServerGroup = true;
					}
				}
				// 첫 접속이 아닌 경우 마지막으로 접속한 지역일 때만 내 서버 선택
				else
				{
					if (lastGameServerGroup.RegionId == csGameServerRegion.RegionId)
					{
						for (int i = 0; i < trContents.childCount; i++)
						{
							trContents.GetChild(i).GetComponent<Toggle>().isOn = false;
						}
						
						toggleMyServer.isOn = true;
						bSelectedServerGroup = true;
					}
				}
			}
		}
		
		if (!bSelectedServerGroup)
		{
			// 첫 번째 추천 서버 그룹 선택
			foreach (CsGameServerGroup csGameServerGroup in csGameServerRegion.GameServerGroupList)
			{
				if (csGameServerGroup.Recommendable)
				{
					Transform trGameServerGroup = trContents.Find(csGameServerGroup.GroupId.ToString());

					if (trGameServerGroup != null)
					{
						for (int i = 0; i < trContents.childCount; i++)
						{
							trContents.GetChild(i).GetComponent<Toggle>().isOn = false;
						}
						
						trGameServerGroup.GetComponent<Toggle>().isOn = true;
						bSelectedServerGroup = true;
						break;
					}
				}
			}

			// 추천 서버 그룹이 없는 경우 첫 번째 서버 그룹 선택
			if (!bSelectedServerGroup)
			{
				for (int i = 0; i < trContents.childCount; i++)
				{
					trContents.GetChild(i).GetComponent<Toggle>().isOn = false;
				}
				
				trContents.GetChild(1).GetComponent<Toggle>().isOn = true;
			}
		}
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedServerGroupToggle(Toggle toggle, int nServerGroupId = -1)
    {
        if (toggle.isOn)
        {
			m_nSelectedServerGroupId = nServerGroupId;
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

			if (nServerGroupId < 0)
			{
				SetServerRightDisplay(0);	
			}
			else
			{
				SetServerRightDisplay(1);
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetServerRightDisplay(int displayNum)
    {
        Transform Display0 = m_trPopupServer.Find("ImageBackground/RightDisplay/Display0");
		Transform Display1 = m_trPopupServer.Find("ImageBackground/RightDisplay/Display1");

        switch (displayNum)
        {
            case 0:
                Display0.gameObject.SetActive(true);
                Display1.gameObject.SetActive(false);
                SetServerPopupMyList();
                break;
            case 1:
                Display1.gameObject.SetActive(true);
                Display0.gameObject.SetActive(false);
                SetServerPopupServerList();
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetServerPopupMyList()
    {
		Transform trDisplay = m_trPopupServer.Find("ImageBackground/RightDisplay/Display0");

        Text textNotServer1 = trDisplay.Find("TextNotServer1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNotServer1);
        textNotServer1.text = CsConfiguration.Instance.GetString("A01_TXT_00040");

        Text textNotServer2 = trDisplay.Find("TextNotServer2").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNotServer2);
        textNotServer2.text = CsConfiguration.Instance.GetString("A01_TXT_00041");

        Transform trRecentServerButton = trDisplay.Find("ButtonRecentServer");

        CsGameServer csGameServer = GetGameServer(m_user.LastVirtualGameServer1);
        GameObject goServerCharacter = Resources.Load<GameObject>("GUI/Intro/ServerCharacter");

		bool bRecentServerInRegion = false;

		if (csGameServer != null)
		{
			CsGameServerGroup csGameServerGroup = GetGameServerGroup(csGameServer.GroupId);

			if (csGameServerGroup != null)
			{
				bRecentServerInRegion = (csGameServerGroup.RegionId == m_nSelectedServerRegionId);
			}
		}

		if (csGameServer == null)
        {
            trRecentServerButton.gameObject.SetActive(false);
            textNotServer1.gameObject.SetActive(true);
        }
        else
        {
			if (m_csGameServerSelected == null || !bRecentServerInRegion)
			{
				trRecentServerButton.gameObject.SetActive(false);
				textNotServer1.gameObject.SetActive(true);
			}
			else
			{
				trRecentServerButton.gameObject.SetActive(true);
				textNotServer1.gameObject.SetActive(false);

				Button buttonRecentServerButton = trRecentServerButton.GetComponent<Button>();
				buttonRecentServerButton.onClick.RemoveAllListeners();
				buttonRecentServerButton.onClick.AddListener(() => OnClickSelectServer(csGameServer));
				buttonRecentServerButton.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

				Text textServerName = trRecentServerButton.Find("TextServerName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textServerName);
				textServerName.text = csGameServer.Name;

				Image imageState = trRecentServerButton.Find("ImageState").GetComponent<Image>();
				SetSeverStatusText(csGameServer.Status, imageState);

				Transform trCharacterList = trRecentServerButton.Find("ImageFrame");

				for (int i = 0; i < trCharacterList.childCount; i++)
				{
					trCharacterList.GetChild(i).gameObject.SetActive(false);
				}

				//서버에 보유중인 캐릭터 표시
				List<CsUserHero> listRecentServerHeros = m_listCsCsUserHero.FindAll(hero => hero.VirtualGameServerId == csGameServer.VirtualGameServerId);

				int nHeroIndex = 0;

				foreach (CsUserHero csUserHero in listRecentServerHeros)
				{
					Transform trCharacter = null;

					if (nHeroIndex < trCharacterList.childCount)
					{
						trCharacter = trCharacterList.GetChild(nHeroIndex);
						trCharacter.gameObject.SetActive(true);
					}
					else
					{
						trCharacter = Instantiate(goServerCharacter, trCharacterList).transform;
					}

					if (trCharacter != null)
					{
						trCharacter.name = csUserHero.HeroId.ToString();

						Text textName = trCharacter.Find("Text").GetComponent<Text>();
						CsUIData.Instance.SetFont(textName);
						textName.text = csUserHero.Name;
					}

					nHeroIndex++;
				}
			}
		}

		// 캐릭터 보유 서버
        GameObject goServerButton = Resources.Load<GameObject>("GUI/Intro/ButtonRetentionServer");
        Transform trServerList = trDisplay.Find("Scroll View/Viewport/Content");
		
		for (int i = 0; i < trServerList.childCount; i++)
        {
            trServerList.GetChild(i).gameObject.SetActive(false);
        }

		CsGameServerRegion csGameServerRegion = m_listCsGameServerRegion.Find(region => region.RegionId == m_nSelectedServerRegionId);

		if (csGameServerRegion != null)
		{
			// 캐릭터 보유 서버 리스트
			var serverList = from gameServerGroup in csGameServerRegion.GameServerGroupList
							from gameServer in gameServerGroup.GameServerList
							join userHero in m_listCsCsUserHero on gameServer.VirtualGameServerId equals userHero.VirtualGameServerId
							select gameServer;
			serverList.ToList();

			int nIndex = 0;

			foreach (CsGameServer server in serverList)
			{
				Transform trServer = trServerList.Find(server.VirtualGameServerId.ToString());

				if (trServer != null && trServer.gameObject.activeSelf)
					continue;

				if (nIndex < trServerList.childCount)
				{
					trServer = trServerList.GetChild(nIndex);
					trServer.gameObject.SetActive(true);
				}
				else
				{
					trServer = Instantiate(goServerButton, trServerList).transform;
				}

				trServer.name = server.VirtualGameServerId.ToString();

				Button buttonRecentServerButton = trServer.GetComponent<Button>();
				buttonRecentServerButton.onClick.RemoveAllListeners();
				buttonRecentServerButton.onClick.AddListener(() => OnClickSelectServer(server));
				buttonRecentServerButton.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

				Text textServerName = trServer.Find("TextServerName").GetComponent<Text>();
				CsUIData.Instance.SetFont(textServerName);
				textServerName.text = server.Name;

				Image imageState = trServer.Find("ImageState").GetComponent<Image>();
				SetSeverStatusText(server.Status, imageState);

				Transform trCharacterList = trServer.Find("ImageFrame");

				for (int j = 0; j < trCharacterList.childCount; j++)
				{
					trCharacterList.GetChild(j).gameObject.SetActive(false);
				}

				//서버에 보유중인 캐릭터 표시
				List<CsUserHero> listRecentServerHeros = m_listCsCsUserHero.FindAll(hero => hero.VirtualGameServerId == csGameServer.VirtualGameServerId);

				int nHeroIndex = 0;

				foreach (CsUserHero csUserHero in listRecentServerHeros)
				{
					Transform trCharacter = null;

					if (nHeroIndex < trCharacterList.childCount)
					{
						trCharacter = trCharacterList.GetChild(nHeroIndex);
						trCharacter.gameObject.SetActive(true);
					}
					else
					{
						trCharacter = Instantiate(goServerCharacter, trCharacterList).transform;
					}

					if (trCharacter != null)
					{
						trCharacter.name = csUserHero.HeroId.ToString();

						Text textName = trCharacter.Find("Text").GetComponent<Text>();
						CsUIData.Instance.SetFont(textName);
						textName.text = csUserHero.Name;
					}

					nHeroIndex++;
				}

				nIndex++;
			}

			if (nIndex > 0)
			{
				textNotServer2.gameObject.SetActive(false);
			}
			else
			{
				textNotServer2.gameObject.SetActive(true);
			}
		}
    }

    //---------------------------------------------------------------------------------------------------
    void SetServerPopupServerList()
    {
		Transform trContent = m_trPopupServer.Find("ImageBackground/RightDisplay/Display1/Scroll View/Viewport/Content");
        GameObject goServerListButton = Resources.Load<GameObject>("GUI/Intro/ButtonServerPopupRight");

		CsGameServerGroup csGameServerGroup = GetGameServerGroup(m_nSelectedServerGroupId);

		if (csGameServerGroup != null)
		{
			for (int i = 0; i < trContent.childCount; i++)
			{
				trContent.GetChild(i).gameObject.SetActive(false);
				trContent.GetChild(i).name = "";
			}

			int nChildIndex = 0;

			bool existRecommendServer = false;

			foreach (CsGameServer csGameServer in csGameServerGroup.GameServerList)
			{
				Transform trServer = null;

				if (nChildIndex < trContent.childCount)
				{
					trServer = trContent.GetChild(nChildIndex);
					trServer.gameObject.SetActive(true);
				}
				else
				{
					trServer = Instantiate(goServerListButton, trContent).transform;
				}

				if (trServer != null)
				{
					trServer.name = csGameServer.VirtualGameServerId.ToString();

					Button buttonServer = trServer.GetComponent<Button>();
					buttonServer.onClick.RemoveAllListeners();
					buttonServer.onClick.AddListener(() => OnClickSelectServer(csGameServer));

					Text textServerName = trServer.Find("TextServerName").GetComponent<Text>();
					CsUIData.Instance.SetFont(textServerName);
					textServerName.text = csGameServer.Name;

					Image imageState = trServer.Find("ImageState").GetComponent<Image>();
					SetSeverStatusText(csGameServer.Status, imageState);

					Transform trImageLabel = trServer.Find("ImageLabel");
					trImageLabel.gameObject.SetActive(csGameServer.Recommend);

					if (csGameServer.Recommend)
					{
						existRecommendServer = true;
					}

					Text textNew = trImageLabel.Find("TextNew").GetComponent<Text>();
					CsUIData.Instance.SetFont(textNew);
					textNew.text = CsConfiguration.Instance.GetString("A01_BTN_00008");
				}

				nChildIndex++;
			}

			// 지정된 추천 서버가 없고, 서버 그룹이 자동 추천 대상일 경우 자동 추천
			if (!existRecommendServer && csGameServerGroup.RecommendServerAuto)
			{
				// 최저접속유저수
				if (csGameServerGroup.RecommendServerConditionType == 1)
				{
					// 서버리스트는 최고접속순서로 정렬
					var listRecommendableServers = from gameServer in csGameServerGroup.GameServerList
												   where gameServer.Recommendable == true
												   orderby gameServer.CurrentUserCount descending
												   select gameServer;

					foreach (CsGameServer recommendableServer in listRecommendableServers)
					{
						Transform trRecommendableServer = trContent.Find(recommendableServer.VirtualGameServerId.ToString());

						if (trRecommendableServer == null)
							continue;

						trRecommendableServer.Find("ImageLabel").gameObject.SetActive(true);

						// 최저접속유저 순으로 정렬(보류)
						//trRecommendableServer.SetAsFirstSibling();
					}
				}
				// 최근오픈서버
				else if (csGameServerGroup.RecommendServerConditionType == 2)
				{
					// 서버리스트는 오픈순서 오래된 순으로 정렬
					var listRecommendableServers = from gameServer in csGameServerGroup.GameServerList
												   where gameServer.Recommendable == true && gameServer.OpenTime != null
												   orderby gameServer.OpenTime descending
												   select gameServer;

					foreach (CsGameServer recommendableServer in listRecommendableServers)
					{
						Transform trRecommendableServer = trContent.Find(recommendableServer.VirtualGameServerId.ToString());

						if (trRecommendableServer == null)
							continue;

						trRecommendableServer.Find("ImageLabel").gameObject.SetActive(true);

						// 최근오픈 순으로 정렬(보류)
						//trRecommendableServer.SetAsFirstSibling();
					}
				}
			}
		}
    }


    //---------------------------------------------------------------------------------------------------
    void SetSeverStatusText(int nServerStatus, Image iamge)
    {
        switch (nServerStatus)
        {
            case 1:
                //원활
                //stServerStatus = CsConfiguration.Instance.GetString("A01_TXT_00002");
                iamge.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Intro/ico_server_1");
                break;
            case 2:
                //혼잡
                //stServerStatus = CsConfiguration.Instance.GetString("A01_TXT_00003");
                iamge.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Intro/ico_server_2");
                break;
            case 3:
                //포화
                //stServerStatus = CsConfiguration.Instance.GetString("A01_TXT_00004");
                iamge.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Intro/ico_server_3");
                break;
            case 4:
                //포화
                //stServerStatus = CsConfiguration.Instance.GetString("A01_TXT_00004");
                iamge.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Intro/ico_server_4");
                break;
        }
    }

    ////---------------------------------------------------------------------------------------------------
    //void OnClickRecommendServer()
    //{
    //    for (int i = 0; i < m_csGameServerGroupList.Count; i++)
    //    {
    //        for (int j = 0; j < m_csGameServerGroupList[i].GameServerList.Count; j++)
    //        {
    //            if (m_csGameServerGroupList[i].GameServerList[j] == GetGameServer(CsConfiguration.Instance.SystemSetting.RecommendGameServerId))
    //            {
    //                m_csGameServerSelectedTemp = m_csGameServerGroupList[i].GameServerList[j];
    //                Transform trPopupServer = m_trCanvas.Find("Popup/PopupServer");
    //                Toggle toggleLeftToggle = trPopupServer.Find("LeftToggles/Scroll View/Viewport/Content/Toggle" + i).GetComponent<Toggle>();
    //                toggleLeftToggle.isOn = true;
    //                Toggle toggleRightToggle = trPopupServer.Find("RightDisplay/Display1/Scroll View/Viewport/Content/Toggle" + i).GetComponent<Toggle>();
    //                toggleRightToggle.isOn = true;
    //            }
    //        }
    //    }
    //}

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectServer(CsGameServer csGameServer)
    {
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

        m_csGameServerSelected = csGameServer;

        Text textButtonLoginServer = m_trCanvas.Find("Lobby/ButtonLoginServer/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonLoginServer);
        textButtonLoginServer.text = m_csGameServerSelected.Name;

        OnClickClosePopupSelectServer();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupSelectServer()
    {
        m_trCanvas.Find("Popup/PopupServer").gameObject.SetActive(false);
    }

#endregion

#endregion

#region 캐릭터선택
    //---------------------------------------------------------------------------------------------------
    void OpenPopupSelectCharacter()
    {
		CsConfiguration.Instance.SendFirebaseLogEvent("lobby_enter");

        m_trLightList.gameObject.SetActive(true);

        if (m_trLoading.gameObject.activeSelf)
        {
            m_trLoading.gameObject.SetActive(false);
        }

        Scene scene = SceneManager.GetSceneByName("IntroCharacter");
        SceneManager.SetActiveScene(scene);
        PlayBGM(EnSoundTypeIntro.Chracter);

        // 그래픽 퀄리티 세팅
        QualitySettings.shadowDistance = 20;

        SetCharacterLight(true);

        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeIn();
        StartCoroutine(m_iEnumerator);

        Init3DCharacter();

        m_trCanvas.Find("CharacterSelect").gameObject.SetActive(true);

        Button buttonClose = m_trCanvas.Find("CharacterSelect/ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickCloseSelectCharacter());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonDeleteCharacter = m_trCanvas.Find("CharacterSelect/ButtonDeleteCharacter").GetComponent<Button>();
        buttonDeleteCharacter.onClick.RemoveAllListeners();
        buttonDeleteCharacter.onClick.AddListener(() => OnClickDeleteCharacter());
        buttonDeleteCharacter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonDeleteCharacter.gameObject.SetActive(false);

        m_buttonGameStart = m_trCanvas.Find("CharacterSelect/ButtonStart").GetComponent<Button>();
        m_buttonGameStart.onClick.RemoveAllListeners();
        m_buttonGameStart.onClick.AddListener(() => OnClickGameStart());
        m_buttonGameStart.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        for (int k = 0; k < 4; k++)
        {
            m_trCanvas.Find("CharacterSelect/CharacterToggle/Toggle" + k).gameObject.SetActive(false);
            m_trCanvas.Find("CharacterSelect/CharacterToggle/Button" + k).gameObject.SetActive(false);
        }

        //토글세팅
        for (int i = 0; i < 4; i++)
        {
            int nIndex = i;
            Toggle toggle = m_trCanvas.Find("CharacterSelect/CharacterToggle/Toggle" + i).GetComponent<Toggle>();

            if (i < m_csLobbyHeroList.Count)
            {
                toggle.gameObject.SetActive(true);
                toggle.transform.Find("Character").gameObject.SetActive(true);
                //리스트에서 캐릭터 레벨과 이름 가져오기			
                Text textLevel = toggle.transform.Find("Character/TextLevel").GetComponent<Text>();
                CsUIData.Instance.SetFont(textLevel);
                textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TXT_LEVEL"), m_csLobbyHeroList[i].Level);

                Text textName = toggle.transform.Find("Character/TextName").GetComponent<Text>();
                CsUIData.Instance.SetFont(textName);

                if (string.IsNullOrEmpty(m_csLobbyHeroList[i].Name))
                {
                    textName.text = CsConfiguration.Instance.GetString("A01_TXT_00021");
                }
                else
                {
                    textName.text = m_csLobbyHeroList[i].Name;
                }

                toggle.onValueChanged.RemoveAllListeners();
                toggle.onValueChanged.AddListener((ison) => onValueChangedSelectRobbyCharacter(nIndex, ison));

                Image imageEmblem = toggle.transform.Find("Character/ImageEmblem").GetComponent<Image>();
                imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + m_csLobbyHeroList[i].JobId);
            }
            else
            {
                Button button = m_trCanvas.Find("CharacterSelect/CharacterToggle/Button" + i).GetComponent<Button>();
                button.gameObject.SetActive(true);
                Text textButton = button.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButton);
                textButton.text = CsConfiguration.Instance.GetString("A01_BTN_00011");
                //캐릭터 생성 보여주기.
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnClickCreateCharacterButton());
                button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
        }

        if (m_csLobbyHeroSelected == null)
        {
            m_bMute = true;
            onValueChangedSelectRobbyCharacter(0);
            m_trCanvas.Find("CharacterSelect/CharacterToggle/Toggle0").GetComponent<Toggle>().isOn = true;
            m_bMute = false;
        }
        else
        {
            for (int j = 0; j < m_csLobbyHeroList.Count; j++)
            {
                if (m_csLobbyHeroList[j].HeroId == m_csLobbyHeroSelected.HeroId)
                {
                    m_bMute = true;
                    onValueChangedSelectRobbyCharacter(j);
                    m_trCanvas.Find("CharacterSelect/CharacterToggle/Toggle" + j).GetComponent<Toggle>().isOn = true;
                    m_bMute = false;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateCharacterButton()
    {
		ClosePopupSelectCharacter();
        OpenPopupCreateCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void onValueChangedSelectRobbyCharacter(int nIndex, bool ison = true)
    {
        //캐릭터 선택창에서 토글이 선택되면 할일.
        if (ison)
        {
            m_csLobbyHeroSelected = m_csLobbyHeroList[nIndex];
            Init3DCharacter();
            Transform trCharList = GameObject.Find("CharacterList").transform;

            int nJobId = 0;

            CsJob csJob = CsGameData.Instance.GetJob(m_csLobbyHeroSelected.JobId);

            if (csJob == null)
            {
                return;
            }
            else
            {
                nJobId = csJob.ParentJobId == 0 ? csJob.JobId : csJob.ParentJobId;
            }

            if (trCharList.Find("Character" + nJobId) == null)
            {
                Debug.Log("해당하는 직업의 모델링이 없음");
            }
            else
            {
                Transform trCharacter = trCharList.Find("Character" + nJobId);

                if (trCharacter != null)
                {
                    //3D 캐릭터 모델링 체인지.
                    Transform trCharacterEquip = trCharacter.Find("Character");
					trCharacterEquip.GetComponent<CsEquipment>().MidChangeEquipments(new CsHeroCustomData(m_csLobbyHeroSelected), false);

                    trCharacter.gameObject.SetActive(true);
                    trCharacter.GetComponent<CsIntroCharacterTimeline>().TimeLineSelectStart(nJobId);
				}
            }

            m_trCanvas.Find("CharacterSelect/ButtonDeleteCharacter").gameObject.SetActive(true);

            Transform trCharacterInfo = m_trCanvas.Find("CharacterSelect/CharacterInfo");

            Text textName = m_trCanvas.Find("CharacterSelect/CharacterInfo/TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), m_csLobbyHeroSelected.Level, m_csLobbyHeroSelected.Name);

            Text texBattlePoint = m_trCanvas.Find("CharacterSelect/CharacterInfo/TexBattlePoint").GetComponent<Text>();
            CsUIData.Instance.SetFont(texBattlePoint);
            texBattlePoint.text = CsConfiguration.Instance.GetString("A01_TXT_00007");

            Text texBattleValue = m_trCanvas.Find("CharacterSelect/CharacterInfo/TexBattleValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(texBattleValue);
            texBattleValue.text = m_csLobbyHeroSelected.BattlePower.ToString("#,##0");

            Text textNation = trCharacterInfo.Find("TextNation").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNation);
            textNation.text = CsConfiguration.Instance.GetString("A05_TXT_00002");

            Text textNationValue = trCharacterInfo.Find("TextNationValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationValue);
            textNationValue.text = CsGameData.Instance.NationList.Find(a => a.NationId == m_csLobbyHeroSelected.NationId).Name;

            Text textJob = trCharacterInfo.Find("TextJob").GetComponent<Text>();
            CsUIData.Instance.SetFont(textJob);
            textJob.text = CsConfiguration.Instance.GetString("A05_TXT_00001");

            Text textJobValue = trCharacterInfo.Find("TextJobValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textJobValue);
            textJobValue.text = CsGameData.Instance.JobList.Find(a => a.JobId == m_csLobbyHeroSelected.JobId).Name;

            if (!m_bMute)
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDeleteCharacter()
    {
        //캐릭터삭제 팝업 띄우기
        OpenPopupCharacterDeleteCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupCharacterDeleteCheck()
    {
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);

        Text textGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGrid);
        textGrid.gameObject.SetActive(true);
        textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00010");

        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(true);
        Text textPlaceholder = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField/Placeholder").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPlaceholder);
        textPlaceholder.text = CsConfiguration.Instance.GetString("A01_TXT_00011");

        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().characterLimit = 0;
        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().text = "";
        Text textInput = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInput);
        textInput.text = "";

        Button buttonNo = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").GetComponent<Button>();
        buttonNo.gameObject.SetActive(true);
        buttonNo.onClick.RemoveAllListeners();
        buttonNo.onClick.AddListener(() => OnClickCancleCharacterDelete());
        buttonNo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();
        buttonOk.gameObject.SetActive(true);
        buttonOk.onClick.RemoveAllListeners();
        buttonOk.onClick.AddListener(() => OnClickCharacterDeleteCheck());
        buttonOk.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCancleCharacterDelete()
    {
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCharacterDeleteCheck()
    {
        if (m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().text == CsConfiguration.Instance.GetString("A01_TXT_00020"))
        {
            SendHeroDelete();
        }
        else
        {
            //에러창띄우기
            //OpenPopupDeleteCharacterError(0);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00012"), OpenPopupCharacterDeleteCheck, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
        }
    }

    /*
	//---------------------------------------------------------------------------------------------------
	void OpenPopupDeleteCharacterError(int nErrorSet)
	{
		m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);

		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").gameObject.SetActive(true);

		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(false);

		m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").gameObject.SetActive(false);
		

		Text textGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGrid);
		Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();
		buttonOk.gameObject.SetActive(true);

		switch (nErrorSet)
		{
			case 0:			//문구입력이 잘못됨
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00012");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickDeleteCharacterError(nErrorSet));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 1:			//캐릭터 삭제진행 실패
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00018");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickDeleteCharacterError(nErrorSet));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickDeleteCharacterError(int nErrorSet)
	{
		switch (nErrorSet)
		{
			case 0:
				OpenPopupCharacterDeleteCheck();
				break;
			case 1:				
				break;
		}
	}
    */

    //---------------------------------------------------------------------------------------------------
    void DeleteCharacterError()
    {
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);
        OpenPopupSelectCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGameStart()
    {
        m_buttonGameStart.interactable = false;

        //캐릭터의 튜토리얼클리어, 이름생성 여부검사 후 인게임실행.
        if (m_csLobbyHeroSelected.NamingTutorialCompleted == false)
        {
            //튜토리얼 진행창으로 이동
            ClosePopupSelectCharacter();
            OpenPopupTutorial();
            m_buttonGameStart.interactable = true;
        }
        else
        {
            if (string.IsNullOrEmpty(m_csLobbyHeroSelected.Name))
            {
                //이름짓기창으로 이동
                ClosePopupSelectCharacter();
                OpenPopupCreateName();
                m_buttonGameStart.interactable = true;
            }
            else
            {
                //인게임으로
                m_bIsCreateEnter = false;
                SendHeroLogin();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseSelectCharacter()
    {
        ClosePopupSelectCharacter();
        Init3DCharacter();
        OpenPopupMainLobby(true);
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupSelectCharacter()
    {
        SetCharacterLight(false);
        m_trCanvas.Find("CharacterSelect").gameObject.SetActive(false);
    }
#endregion

#region 캐릭터생성(직업선택/ 커스터마이징 / 나라선택)
    //---------------------------------------------------------------------------------------------------
    void OpenPopupCreateCharacter()
    {
		CsConfiguration.Instance.SendFirebaseLogEvent("character_creation_start");

        m_trLightList.gameObject.SetActive(true);

        Scene scene = SceneManager.GetSceneByName("IntroCharacter");
        SceneManager.SetActiveScene(scene);
        PlayBGM(EnSoundTypeIntro.Chracter);

        SetCharacterLight(true);

        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeIn();
        StartCoroutine(m_iEnumerator);

        Init3DCharacter();

        Transform trCharacterCreate = m_trCanvas.Find("CharacterCreate");
        trCharacterCreate.gameObject.SetActive(true);

        Button buttonNext = m_trCanvas.Find("CharacterCreate/ButtonNext").GetComponent<Button>();
        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(() => OnClickOpenCustomizing());
        buttonNext.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonClose = m_trCanvas.Find("CharacterCreate/ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickCloseCreateCharacterPopup());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textName = m_trCanvas.Find("CharacterCreate/RightDisplay/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsGameData.Instance.JobList[0].Name;

        Text textExplain = m_trCanvas.Find("CharacterCreate/RightDisplay/TextExplain").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExplain);
        textExplain.text = CsGameData.Instance.JobList[0].Description;

        Text textSkill = m_trCanvas.Find("CharacterCreate/RightDisplay/TextSkill").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSkill);
        textSkill.text = CsConfiguration.Instance.GetString("A01_TXT_00055");

        Toggle toggleDefault = m_trCanvas.Find("CharacterCreate/CharacterToggle/Toggle0").GetComponent<Toggle>();
        m_bMute = true;
        onValueChangedSelectJobToggle(0, toggleDefault);
        toggleDefault.isOn = true;
        m_bMute = false;

        List<CsJob> listCsJobBase = CsGameData.Instance.JobList.FindAll(a => a.ParentJobId == 0 || a.JobId == a.ParentJobId);

        for (int i = 0; i < listCsJobBase.Count; i++)
        {
            int nIndex = i;
            Toggle toggle = m_trCanvas.Find("CharacterCreate/CharacterToggle/Toggle" + i).GetComponent<Toggle>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((ison) => onValueChangedSelectJobToggle(nIndex, toggle));

            Image imageOff = toggle.transform.Find("Background/ImageOff").GetComponent<Image>();
            imageOff.sprite = Resources.Load<Sprite>("GUI/Common/ico_small_emblem_off_" + CsGameData.Instance.JobList[i].JobId);

            if (i == 0)
            {
                imageOff.color = new Color32(255, 255, 255, 255);
            }
            else
            {
                imageOff.color = new Color32(255, 255, 255, 127);
            }

            Text textJobName = toggle.transform.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textJobName);
            textJobName.text = CsGameData.Instance.JobList[i].Name;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void onValueChangedSelectJobToggle(int nIndex, Toggle toggle)
    {
        Text textJobName = toggle.transform.Find("TextName").GetComponent<Text>();
        Image imageOff = toggle.transform.Find("Background/ImageOff").GetComponent<Image>();

        if (toggle.isOn)
        {
            if (!m_bMute)
            {
                CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            }

            Init3DCharacter();
            Transform trCharList = GameObject.Find("CharacterList").transform;
			int nJobId = CsGameData.Instance.JobList[nIndex].ParentJobId == 0 ? CsGameData.Instance.JobList[nIndex].JobId : CsGameData.Instance.JobList[nIndex].ParentJobId;
			Transform trCharacter = trCharList.Find("Character" + nJobId);

            Transform trCharacterEquip = trCharacter.Find("Character");
			CsCustomizingManager.Instance.InitCustom(nJobId, trCharacterEquip, true);

            if (trCharacter == null)
            {
                Debug.Log("해당하는 직업의 모델링이 없음");
            }
            else
            {
                trCharacter.gameObject.SetActive(true);
                CsIntroCharacterTimeline csIntroCharacterTimeline = trCharacter.GetComponent<CsIntroCharacterTimeline>();
                if (csIntroCharacterTimeline != null)
                {
                    csIntroCharacterTimeline.TimeLineCreateStart(CsGameData.Instance.JobList[nIndex].JobId);
                }
            }

            m_nLobbyHeroJobIdTemp = CsGameData.Instance.JobList[nIndex].JobId;

            Transform trRightDisplay = m_trCanvas.Find("CharacterCreate/RightDisplay");
            Text textName = trRightDisplay.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = CsGameData.Instance.JobList[nIndex].Name;

            Text textExplain = trRightDisplay.Find("TextExplain").GetComponent<Text>();
            CsUIData.Instance.SetFont(textExplain);
            textExplain.text = CsGameData.Instance.JobList[nIndex].Description;

            textJobName.color = new Color32(127, 213, 246, 255);
            imageOff.color = new Color32(255, 255, 255, 255);

            Transform trSkillIconList = trRightDisplay.Find("SkillIconList");

            for (int i = 0; i < trSkillIconList.childCount; i++)
            {
                int nSkillToggleIndex = i;

                Toggle toggleSkill = trSkillIconList.Find("Toggle" + i).GetComponent<Toggle>();
                toggleSkill.onValueChanged.RemoveAllListeners();
                toggleSkill.onValueChanged.AddListener((ison) => OnValueChangedSkillInfo(toggleSkill, nIndex, nSkillToggleIndex));
                m_bMute = true;
                toggleSkill.isOn = false;
                m_bMute = false;

                Image image = toggleSkill.transform.Find("Background").GetComponent<Image>();
                image.sprite = Resources.Load<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.JobList[nIndex].JobId + "_" + (i + 2));
            }
        }
        else
        {
            textJobName.color = new Color32(133, 141, 148, 255);
            imageOff.color = new Color32(255, 255, 255, 127);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSkillInfo(Toggle toggleSkill, int nIndex, int nSkillToggleIndex)
    {
        Transform trRightDisplay = m_trCanvas.Find("CharacterCreate/RightDisplay");
        Transform trSkillIconList = trRightDisplay.Find("SkillIconList");
        Text textName = trRightDisplay.Find("TextName").GetComponent<Text>();
        Text textExplain = trRightDisplay.Find("TextExplain").GetComponent<Text>();

        CsJobSkill CsJobSkill = CsGameData.Instance.JobSkillList.Find(a => a.JobId == CsGameData.Instance.JobList[nIndex].JobId && a.SkillId == nSkillToggleIndex + 2);

        if (toggleSkill.isOn)
        {
            textName.text = CsJobSkill.Name;
            textExplain.text = CsJobSkill.Description;

            for (int i = 0; i < trSkillIconList.childCount; i++)
            {
                int nToggleIndex = i;

                if (toggleSkill.name != "Toggle" + i)
                {
                    Toggle toggle = trSkillIconList.Find("Toggle" + i).GetComponent<Toggle>();
                    toggle.onValueChanged.RemoveAllListeners();
                    toggle.isOn = false;
                    toggle.onValueChanged.AddListener((ison) => OnValueChangedSkillInfo(toggle, nIndex, nToggleIndex));
                }
            }
        }
        else
        {
            textName.text = CsGameData.Instance.JobList[nIndex].Name;
            textExplain.text = CsGameData.Instance.JobList[nIndex].Description;
        }

        if (!m_bMute)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupCreateCharacter()
    {
        SetCharacterLight(false);
        m_trCanvas.Find("CharacterCreate").gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseCreateCharacterPopup()
    {
        ClosePopupCreateCharacter();
        Init3DCharacter();

        if (m_csLobbyHeroList.Count <= 0)
        {
            //로비2로 이동
            OpenPopupMainLobby(true);
        }
        else
        {
            //캐릭터선택창으로 이동
            OpenPopupSelectCharacter();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenCustomizing()
    {
        OpenPopupCutomizing();
        //m_csCustomizing.SetDefault();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBackCustomizing()
    {
        ClosePopupSelectNation();
        OpenPopupCutomizing();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupCutomizing()
    {
        ClosePopupCreateCharacter();
        m_csLobbyHeroNew = new CsLobbyHero(m_nLobbyHeroJobIdTemp);
        m_trCharacterCustomizing.gameObject.SetActive(true);
        m_csCustomizing = m_trCharacterCustomizing.Find("PopupCustomizing").GetComponent<CsCustomizing>();
        m_csCustomizing.SetDisplay(m_csLobbyHeroNew.JobId, true);

        m_csCustomizing.EventSaveCustomByIntro += OnEventSaveCustomByIntro;
        m_csCustomizing.EventFaceCamera += OnEventFaceCamera;

        Button ButtonBack = m_trCharacterCustomizing.Find("ButtonBack").GetComponent<Button>();
        ButtonBack.onClick.RemoveAllListeners();
        ButtonBack.onClick.AddListener(OnClickCustomizingBack);
        ButtonBack.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonBack = ButtonBack.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonBack);
        textButtonBack.text = CsConfiguration.Instance.GetString("PUBLIC_BTN_BACK");


        Transform trCharList = GameObject.Find("CharacterList").transform;
        Transform trCharacter = trCharList.Find("Character" + m_nLobbyHeroJobIdTemp);
        trCharacter.gameObject.SetActive(false);

        Transform trCustom = trCharList.Find("CustomCharacter" + m_nLobbyHeroJobIdTemp);

        if (trCustom != null)
        {
            //3D 캐릭터 모델링 체인지
            trCharacter.gameObject.SetActive(false);
            trCustom.gameObject.SetActive(true);

            Transform trCustomCharacter = trCustom.Find("Character");
            CsCustomizingManager.Instance.InitCustom(m_nLobbyHeroJobIdTemp, trCustomCharacter);
            trCustomCharacter.gameObject.SetActive(true);
            SetCharacterLight(true);
			NormalCustomCamera(m_nLobbyHeroJobIdTemp);
		}
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupCustomizing()
    {
        m_trCharacterCustomizing.gameObject.SetActive(false);
        m_csCustomizing.EventSaveCustomByIntro -= OnEventSaveCustomByIntro;
        m_csCustomizing.EventFaceCamera -= OnEventFaceCamera;

        Transform trCharList = GameObject.Find("CharacterList").transform;
        Transform trCustom = trCharList.Find("CustomCharacter" + m_nLobbyHeroJobIdTemp);

        if (trCustom != null)
        {
            trCustom.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCustomizingBack()
    {
        ClosePopupCustomizing();
        OpenPopupCreateCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSaveCustomByIntro()
    {
        ClosePopupCustomizing();
        OpenPopupSelectNation();
    }

	bool m_bCustomZoom = false;
	EnCustomState m_enCustomState = EnCustomState.Normal;
	//---------------------------------------------------------------------------------------------------
	void OnEventFaceCamera(EnCustomState enCustomState)
    {
		Debug.Log("OnEventFaceCamera    enCustomState : " + enCustomState);
		if (m_enCustomState == enCustomState) return;

		if (enCustomState == EnCustomState.Zoom)
		{
			if (m_bCustomZoom == false)
			{
				ZoomCustomCamera();
			}
		}
		else if (enCustomState == EnCustomState.Normal)
		{
			NormalCustomCamera(m_nLobbyHeroJobIdTemp);
		}
		else
		{
			FarCustomCamera();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ZoomCustomCamera()
	{
		Debug.Log("ZoomCustomCamera() : " + m_nLobbyHeroJobIdTemp);

		m_bCustomZoom = true;
		Transform trCharList = GameObject.Find("CharacterList").transform;
		Transform trCustom = trCharList.Find("CustomCharacter" + m_nLobbyHeroJobIdTemp);
		Transform trCamera = trCustom.Find("IntroCamera/CharacterCamera");
		Transform trCharacter = trCustom.Find("Character");
		Transform trCustomCharacter = trCustom.Find("Character");

		float flLegsLengthOffset = trCustomCharacter.GetComponent<CsEquipment>().LegsLengthOffset;
		flLegsLengthOffset = flLegsLengthOffset * 0.5f;

		Vector3 vtPos = Vector3.zero;
		Vector3 vtRot = Vector3.zero;

		Vector3 vtCenterPos = new Vector3(trCustomCharacter.position.x, trCustomCharacter.position.y + 1f, trCustomCharacter.position.z);
		if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Gaia)
		{
			vtPos = new Vector3(0f, 1.45f + flLegsLengthOffset, 0.8f);
			vtCenterPos = new Vector3(trCustomCharacter.position.x - 0.05f, trCustomCharacter.position.y + 1.7f, trCustomCharacter.position.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Asura)
		{
			vtPos = new Vector3(0f, 1.4f + flLegsLengthOffset, 0.8f);
			vtCenterPos = new Vector3(trCustomCharacter.position.x - 0.05f, trCustomCharacter.position.y + 1.5f, trCustomCharacter.position.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Deva)
		{
			vtPos = new Vector3(0f, 1.45f + flLegsLengthOffset, 0.8f);
			vtCenterPos = new Vector3(trCustomCharacter.position.x - 0.05f, trCustomCharacter.position.y + 1.65f, trCustomCharacter.position.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Witch)
		{
			vtPos = new Vector3(0f, 1.1f + flLegsLengthOffset, 0.8f);
			vtCenterPos = new Vector3(trCustomCharacter.position.x - 0.03f, trCustomCharacter.position.y + 1.35f, trCustomCharacter.position.z);
		}

		trCharacter.rotation = Quaternion.identity;
		trCamera.position = vtPos;
		trCamera.LookAt(vtCenterPos);
		m_enCustomState = EnCustomState.Zoom;
	}

	//---------------------------------------------------------------------------------------------------
	void NormalCustomCamera(int nJobId, bool bCustom = true)
	{
		Debug.Log("NormalCustomCamera() : " + nJobId);
		m_bCustomZoom = false;
		Transform trCharList = GameObject.Find("CharacterList").transform;
		Transform trSeletCharacter = null;

		if (bCustom)
		{
			trSeletCharacter = trCharList.Find("CustomCharacter" + nJobId);
		}
		else
		{
			trSeletCharacter = trCharList.Find("Character" + nJobId);
		}

		Transform trCamera = trSeletCharacter.Find("IntroCamera/CharacterCamera");
		Transform trCharacter = trSeletCharacter.Find("Character");

		Vector3 vtCenterPos = trCharacter.position;
		Vector3 vtPos = new Vector3(0, 1.2f, -4);

		if ((EnJob)nJobId == EnJob.Gaia)
		{
			vtPos = new Vector3(0.1f, 1.3f, 3f); // 1.9, 182.726, 0
			vtCenterPos = new Vector3(vtCenterPos.x - 0.15f, vtCenterPos.y + 1.4f, vtCenterPos.z);
		}
		else if ((EnJob)nJobId == EnJob.Asura)
		{
			vtPos = new Vector3(0f, 1.2f, 2.1f); // 4, 180, 0
			vtCenterPos = new Vector3(vtCenterPos.x - 0.1f, vtCenterPos.y + 1.2f, vtCenterPos.z);
		}
		else if ((EnJob)nJobId == EnJob.Deva)
		{
			vtPos = new Vector3(0f, 1.3f, 2.5f); // 4, 180, 0
			vtCenterPos = new Vector3(vtCenterPos.x - 0.1f, vtCenterPos.y + 1.3f, vtCenterPos.z);
		}
		else if ((EnJob)nJobId == EnJob.Witch)
		{
			vtPos = new Vector3(0.1f, 1f, 2.2f); // 9, 182, 0
			vtCenterPos = new Vector3(vtCenterPos.x -0.1f, vtCenterPos.y + 1.2f, vtCenterPos.z);
		}

		trCharacter.rotation = Quaternion.identity;
		trCamera.position = vtPos;
		trCamera.LookAt(vtCenterPos);
		m_enCustomState = EnCustomState.Normal;
	}

	//---------------------------------------------------------------------------------------------------
	void FarCustomCamera()
	{
		Debug.Log("FarCustomCamera() : " + m_nLobbyHeroJobIdTemp);
		m_bCustomZoom = false;
		Transform trCharList = GameObject.Find("CharacterList").transform;
		Transform trCustom = trCharList.Find("CustomCharacter" + m_nLobbyHeroJobIdTemp);
		Transform trCamera = trCustom.Find("IntroCamera/CharacterCamera");
		Transform trCharacter = trCustom.Find("Character");

		Vector3 vtCenterPos = trCharacter.position;
		Vector3 vtPos = new Vector3(0, 1.2f, -4);

		if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Gaia)
		{
			vtPos = new Vector3(0f, 1.5f, 4.5f);
			vtCenterPos = new Vector3(vtCenterPos.x - 0.05f, vtCenterPos.y + 1f, vtCenterPos.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Asura)
		{
			vtPos = new Vector3(0f, 1.5f, 4.5f);
			vtCenterPos = new Vector3(vtCenterPos.x - 0.05f, vtCenterPos.y + 0.9f, vtCenterPos.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Deva)
		{
			vtPos = new Vector3(0f, 1.5f, 4.5f);
			vtCenterPos = new Vector3(vtCenterPos.x - 0.05f, vtCenterPos.y + 1f, vtCenterPos.z);
		}
		else if ((EnJob)m_nLobbyHeroJobIdTemp == EnJob.Witch)
		{
			vtPos = new Vector3(0f, 1.1f, 4.5f);
			vtCenterPos = new Vector3(vtCenterPos.x - 0.05f, vtCenterPos.y + 0.8f, vtCenterPos.z);
		}

		trCharacter.rotation = Quaternion.identity;
		trCamera.position = vtPos;
		trCamera.LookAt(vtCenterPos);
		m_enCustomState = EnCustomState.Far;
	}

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSelectNation()
    {
        m_trCanvas.Find("CountrySelect").gameObject.SetActive(true);

        Transform trBack = m_trCanvas.Find("ImageBackground");
        trBack.gameObject.SetActive(true);

        Transform trSelectNation = m_trCanvas.Find("CountrySelect");

        Transform trButtonList = trSelectNation.Find("ButtonList");

        Text textCountrySelect = trSelectNation.Find("TextCountrySelect").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCountrySelect);
        textCountrySelect.text = CsConfiguration.Instance.GetString("A01_NAME_00007");

        Text textNationDesc = trSelectNation.Find("TextNationDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationDesc);
        textNationDesc.text = "";

        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            int nNationId = CsGameData.Instance.NationList[i].NationId;

            Button buttonNation = trButtonList.Find("ButtonNation" + CsGameData.Instance.NationList[i].NationId).GetComponent<Button>();
            buttonNation.onClick.RemoveAllListeners();
            buttonNation.onClick.AddListener(() => OnClickSelectNation(nNationId));
            buttonNation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textNationName = buttonNation.transform.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNationName);
            textNationName.text = CsGameData.Instance.NationList[i].Name;

            //Text textRecommend = buttonNation.transform.Find("TextRecommend").GetComponent<Text>();
            //CsUIData.Instance.SetFont(textRecommend);
            //textRecommend.text = CsConfiguration.Instance.GetString("A01_TXT_00054");

            Image imageRecommend = buttonNation.transform.Find("ImageRecommend").GetComponent<Image>();

            if (nNationId == m_nHeroCreationDefaultNationId)
            {
                imageRecommend.gameObject.SetActive(true);
            }
            else
            {
                imageRecommend.gameObject.SetActive(false);
            }
        }

        Button buttonClose = m_trCanvas.Find("CountrySelect/ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickBackCustomizing());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonCreateCharacter = m_trCanvas.Find("CountrySelect/ButtonCreateCharacter").GetComponent<Button>();
        buttonCreateCharacter.onClick.RemoveAllListeners();
        buttonCreateCharacter.onClick.AddListener(() => OnClickCreateCharacter());
        buttonCreateCharacter.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonCreateCharacter.gameObject.SetActive(false);

        OnClickSelectNation(m_nHeroCreationDefaultNationId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSelectNation(int nNationId)
    {
        if (m_csLobbyHeroNew.NationId != 0 || m_csLobbyHeroNew.NationId != nNationId)
        {
            Transform trCountrySelect = m_trCanvas.Find("CountrySelect");
            Transform trButtonList = trCountrySelect.Find("ButtonList");
            Text textCountrySelect = trCountrySelect.Find("TextCountrySelect").GetComponent<Text>();
            Text textNationDesc = trCountrySelect.Find("TextNationDesc").GetComponent<Text>();

            for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
            {
                Transform trNation = trButtonList.Find("ButtonNation" + CsGameData.Instance.NationList[i].NationId);
                Image imageNation = trNation.GetComponent<Image>();
                Image imageRecommend = trNation.Find("ImageRecommend").GetComponent<Image>();
                Text textName = trNation.Find("TextName").GetComponent<Text>();

                if (CsGameData.Instance.NationList[i].NationId == nNationId)
                {
                    Transform trButtonCreateCharacter = trCountrySelect.Find("ButtonCreateCharacter");
                    trButtonCreateCharacter.gameObject.SetActive(true);

                    imageNation.color = new Color32(255, 255, 255, 255);
                    imageRecommend.color = new Color32(255, 255, 255, 255);
                    textName.color = new Color32(222, 222, 222, 255);

                    m_csLobbyHeroNew.NationId = nNationId;

                    textCountrySelect.text = CsGameData.Instance.NationList[i].Name;
                    textNationDesc.text = CsGameData.Instance.NationList[i].Description;
                }
                else
                {
                    imageNation.color = new Color32(255, 255, 255, 75);
                    imageRecommend.color = new Color32(255, 255, 255, 75);
                    textName.color = new Color32(222, 222, 222, 75);
                }
            }
        }
        else
        {
            Debug.Log("같은 국가 재선택");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupSelectNation()
    {
        m_trCanvas.Find("CountrySelect").gameObject.SetActive(false);

        Transform trBack = m_trCanvas.Find("ImageBackground");
        trBack.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseSelectNation()
    {
        ClosePopupSelectNation();
        OpenPopupCreateCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateCharacter()
    {
        if (m_csLobbyHeroNew.JobId > 0 && m_csLobbyHeroNew.JobId <= CsGameData.Instance.JobList.Count && m_csLobbyHeroNew.NationId > 0 && m_csLobbyHeroNew.NationId <= CsGameData.Instance.NationList.Count)
        {
			m_csLobbyHeroNew = CsCustomizingManager.Instance.SaveObject(m_csLobbyHeroNew);
            SendHeroCreate();
        }
    }

#endregion

#region 튜토리얼
    //---------------------------------------------------------------------------------------------------
    void OpenPopupTutorial()
    {
        m_trCanvas.Find("Tutorial").gameObject.SetActive(true);

        Transform trBack = m_trCanvas.Find("ImageBackground");
        trBack.gameObject.SetActive(true);

        Button buttonSkip = m_trCanvas.Find("Tutorial/ButtonSkip").GetComponent<Button>();
        buttonSkip.onClick.RemoveAllListeners();
        buttonSkip.onClick.AddListener(() => OnClickTutorialSkip());
        buttonSkip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //임시 튜토리얼 생략
        OnClickTutorialSkip();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupTutorial()
    {
        Transform trBack = m_trCanvas.Find("ImageBackground");
        trBack.gameObject.SetActive(false);

        m_trCanvas.Find("Tutorial").gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTutorialSkip()
    {
        SendHeroNamingTutorialComplete();
    }

#endregion

#region 이름짓기
    //---------------------------------------------------------------------------------------------------
    void OpenPopupCreateName()
    {
        Transform trBack = m_trCanvas.Find("ImageBackground");
        trBack.gameObject.SetActive(true);

        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);
        m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").gameObject.SetActive(false);
        m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").gameObject.SetActive(true);

        Text TextGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(TextGrid);
        TextGrid.gameObject.SetActive(true);
        TextGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00013");

        Text textInput = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInput);
        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(true);

        Text textPlaceholder = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField/Placeholder").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPlaceholder);
        textPlaceholder.text = CsConfiguration.Instance.GetString("A01_TXT_00014");
        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().characterLimit = 8;
        m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().text = "";
        textInput.text = "";

        Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();
        buttonOk.onClick.RemoveAllListeners();
        buttonOk.onClick.AddListener(() => OnClickCheckCharacterName());
        buttonOk.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCheckCharacterName()
    {
        m_csLobbyHeroSelected.Name = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input/InputField").GetComponent<InputField>().text;
        if (m_csLobbyHeroSelected.Name.IndexOf("　") > -1 || m_csLobbyHeroSelected.Name.IndexOf(" ") > -1)  // ㄱ + 한자 중 첫번째 글자 또는 공백.
        {
            // 닉네임에 공백을 넣을 수 없습니다.
            //OpenPopupCreateNameError(0);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00016"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            return;
        }

        string sNickname = m_csLobbyHeroSelected.Name.Replace(" ", "");

        if (sNickname.Length < 2 || sNickname.Length > 8)
        {
            //길이 에러
            //OpenPopupCreateNameError(1);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00022"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            return;
        }

        //금지문자 찾기
        /*
		if (ContainsBanWord(CsConfiguration.Instance.NameBanWordList, sNickname))
		{
			OpenPopupCreateNameError(0);
			return;
		}
		*/

        //특수문자 찾기
        //Regex reg = new Regex("[^a-zA-Z0-9가-힣_]",RegexOptions.Singleline);
        String str = Regex.Replace(sNickname, @"[^0-9a-zA-Z가-힣]", "");

        if (str != sNickname)
        {
            //OpenPopupCreateNameError(0);
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00016"), OpenPopupCreateName, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"));
            return;
        }

        string strNationName = CsGameData.Instance.GetNation(m_csLobbyHeroSelected.NationId).Name;
        m_csPanelModal.Choice(string.Format(CsConfiguration.Instance.GetString("A01_TXT_01005"), strNationName, m_csLobbyHeroSelected.Name), SendHeroNameSet, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), null, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), true);
    }

    /*
	//---------------------------------------------------------------------------------------------------
	void OpenPopupCreateNameError(int nErrorNum = 0)
	{
		m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);
		m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").gameObject.SetActive(false);
		m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").gameObject.SetActive(true);
		
		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(false);
		Text textGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGrid);
		textGrid.gameObject.SetActive(true);

		Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();

		switch (nErrorNum)
		{
			case 0:		//특수문자나 밴리스트, 공백
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00016");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickCreateNameError(0));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 1:		//길이
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00022");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickCreateNameError(0));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 2:		//중복
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00015");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickCreateNameError(0));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 3:		//이름 짓기 진행에 실패하였습니다. 다시 시도해주세요.
				textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00019");
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickCreateNameError(1));
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickCreateNameError(int nErrorNum)
	{
		switch (nErrorNum)
		{
			case 0:
				OpenPopupCreateName();
				break;
            case 1:				
				break;
		}
	}
    */

    //---------------------------------------------------------------------------------------------------
    void CreateNameError()
    {
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);
        OpenPopupSelectCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupCreateName()
    {
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);
    }

#endregion

    /*
	//---------------------------------------------------------------------------------------------------
	void OpenPopupError(int nErrorNum, int nErrorCode = 0)
	{
		m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);
		m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").gameObject.SetActive(false);
		
		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(false);
		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").gameObject.SetActive(true);

		Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();
		buttonOk.gameObject.SetActive(true);
		Text textGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGrid);
		switch (nErrorNum)
		{
			case 0: // 튜토리얼 진행 에러
				if (nErrorCode != 0)
				{
					textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				}
				else
				{
					textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00032");
				}
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickTutorialError());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				
				break;
			case 1: // 영웅생성실패
				if (nErrorCode != 0)
				{
					textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				}
				else
				{
					textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00033");
				}
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickCreateCharacterError());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 2: // 게임시작실패
				if (nErrorCode != 0)
				{
					textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				}
				else
				{
					textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00034");
				}
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickGameStartError());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 3: // 캐릭터생성이 완료되지않았습니다
				if (nErrorCode != 0)
				{
					textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				}
				else
				{
					textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00031");
				}
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickGameStartError());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 4: // 서버접속 실패
				if (nErrorCode != 0)
				{
					textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				}
				else
				{
					textGrid.text = CsConfiguration.Instance.GetString("A01_TXT_00035");
				}
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickServerContact());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
			case 5:
				textGrid.text = string.Format(CsConfiguration.Instance.GetString("PUBLIC_ERROR_001"), nErrorCode);
				buttonOk.onClick.RemoveAllListeners();
				buttonOk.onClick.AddListener(() => OnClickServerContact());
                buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));
				break;
		}
	}
    */

    //---------------------------------------------------------------------------------------------------
    void CloseAll()
    {
        for (int i = 0; i < m_trCanvas.childCount; i++)
        {
            if (m_trCanvas.GetChild(i).name != "ImageFade" && m_trCanvas.GetChild(i).name != "Popup" && m_trCanvas.GetChild(i).name != "PanelToast" && m_trCanvas.GetChild(i).name != "PanelModal")
            {
                m_trCanvas.GetChild(i).gameObject.SetActive(false);
            }
        }

        Init3DCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickServerContact()
    {
        CloseAll();
        OpenPopupMainLobby(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGameStartError()
    {
        CloseAll();
        OpenPopupSelectCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCreateCharacterError()
    {
        CloseAll();
        ClosePopupSelectNation();
        OpenPopupSelectCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTutorialError()
    {
        CloseAll();
        ClosePopupTutorial();
        OpenPopupSelectCharacter();
    }

    //---------------------------------------------------------------------------------------------------
    void LoadUserCredential()
    {
        string strUserId = PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyUserCredentialId);
        string strUserSecret = PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyUserCredentialSecret);

        UserCredential userCredential = null;

        if (!string.IsNullOrEmpty(strUserId) && !string.IsNullOrEmpty(strUserSecret))
        {
            userCredential = new UserCredential(strUserId, strUserSecret);
            m_bIsFirstVisit = false;
        }

        m_userCredential = userCredential;
    }

    //---------------------------------------------------------------------------------------------------
    void LoadGuestUserCredential()
    {
        string strUserId = PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyGuestUserCredentialId);
        string strUserSecret = PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyGuestUserCredentialSecret);

        UserCredential userCredential = null;

        if (!string.IsNullOrEmpty(strUserId) && !string.IsNullOrEmpty(strUserSecret))
        {
            userCredential = new UserCredential(strUserId, strUserSecret);
            m_bIsFirstVisit = false;
        }

        m_userCredentialGuest = userCredential;
    }

    //---------------------------------------------------------------------------------------------------
    void ReloadScene()
    {
        StartCoroutine(ReloadSceneCoroutine());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator ReloadSceneCoroutine()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Intro");
        yield return asyncOperation;
    }

    //---------------------------------------------------------------------------------------------------
    public void RequestAuthServerInfo() // 11/7 광열 인증 추가. Native.
    {
		Debug.Log("RequestAuthServerInfo()");

		//RequestStageFarmVersion();   // Unity APK 빌드시 조건없이 바로 호출해야 접속 가능함.

		if (CsConfiguration.Instance.ConnectMode == CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			RequestStageFarmVersion();
		}
		else
		{
			CsAuthServerInfoNACommand cmd = new CsAuthServerInfoNACommand();
			cmd.Finished += ResponseAuthServerInfo;
			cmd.Run();
		}
    }

    //---------------------------------------------------------------------------------------------------
    public void ResponseAuthServerInfo(object sender, EventArgs e) // 11/7 광열 인증 추가. Native.
    {
		CsAuthServerInfoNACommand cmd = (CsAuthServerInfoNACommand)sender;

		if (!cmd.isOK)
		{
			m_csPanelModal.Choice("CsAuthServerInfoNACommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		CsAuthServerInfoNAResponse res = (CsAuthServerInfoNAResponse)cmd.response;

		if (res.isOK)
		{
			CsConfiguration.Instance.PlatformId = (CsConfiguration.EnPlatformID)res.PlatformId;
			CsConfiguration.Instance.BuildNo = res.BuildNo;
			CsConfiguration.Instance.GateServerApiUrl = res.GateServerApiUrl;
			CsConfiguration.Instance.ClientVersion = res.ClientVersion;

			RequestStageFarmVersion();
		}
		else
		{
			m_csPanelModal.Choice("CsAuthServerInfoNAResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
    }

    //---------------------------------------------------------------------------------------------------
    public void RequestStageFarmVersion() // 11/7 광열 인증 추가. Gate 연결.
    {
        Debug.Log("RequestStageFarmVersion() "+ CsConfiguration.Instance.PlatformId + " , " + CsConfiguration.Instance.ClientVersion + " , " + CsConfiguration.Instance.BuildNo);

        StageFarmVersionGateServerCommand cmd = new StageFarmVersionGateServerCommand();
        cmd.Finished += ResponseStageFarmVersion;
        cmd.PlatformId = (int)CsConfiguration.Instance.PlatformId;
        cmd.VersionName = CsConfiguration.Instance.ClientVersion;
        cmd.BuildNo = CsConfiguration.Instance.BuildNo;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    public void ResponseStageFarmVersion(object sender, EventArgs e) // 11/7 광열 인증 추가. Gate 연결.
    {
        StageFarmVersionGateServerCommand cmd = (StageFarmVersionGateServerCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("StageFarmVersionGateServerCommand : " + cmd.error.Message, ReloadScene, "OK");
            Debug.Log(cmd.error.Message);
            Debug.Log(cmd.Trace());
            return;
        }

        StageFarmVersionGateServerResponse res = (StageFarmVersionGateServerResponse)cmd.response;

        if (res.isOK)
        {
            CsConfiguration.Instance.FarmId = res.FarmId;
            CsConfiguration.Instance.ServerGroupName = res.Name; // 서버군 이름.
            CsConfiguration.Instance.AuthServerApiUrl = res.ServerUrl;
            RequestSystemSetttings();
        }
        else
        {
            if (res.result == 101) // 스토어연결.
            {
                Debug.Log(res.errorMessage); //res.DownloadUrl; // 엡스토어로 연결 필요.
				m_csPanelModal.Choice("The new APK version download is required.\nDo you want to go to the Store?", () => OpenDownloadURL(res.DownloadUrl), "OK", ExitClient, "Cancel", true);
            }
			else
			{
				m_csPanelModal.Choice("StageFarmVersionGateServerResponse : " + res.errorMessage, ReloadScene, "OK");
				Debug.Log(res.errorMessage);
				Debug.Log(res.Trace());
				return;
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenDownloadURL(string strURL)
    {
        Application.OpenURL(strURL);
        Application.Quit();
    }

    //---------------------------------------------------------------------------------------------------
    void ExitClient()
    {
        Application.Quit();
    }

    //---------------------------------------------------------------------------------------------------
    void RequestSystemSetttings()
    {
        Debug.Log("RequestSystemSetttings()");
        SystemSettingASCommand cmd = new SystemSettingASCommand();
        cmd.Finished += ResponseSystemSetttings;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseSystemSetttings(object sender, EventArgs e)
    {
        SystemSettingASCommand cmd = (SystemSettingASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("SystemSettingASCommand : " + cmd.error.Message, ReloadScene, "OK");
            Debug.Log(cmd.error.Message);
            Debug.Log(cmd.Trace());
            return;
        }

        SystemSettingASResponse res = (SystemSettingASResponse)cmd.response;

        if (res.isOK)
        {
			CsConfiguration.Instance.SystemSetting = res.SystemSetting;

			if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyLanguage))
			{
				CsUIData.Instance.Language = (EnServerLanguage)PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyLanguage);
				CsUIData.Instance.MaintenanceInfoUrl = PlayerPrefs.GetString(CsConfiguration.Instance.PlayerPrefsKeyMaintenanceInfoUrl);

				if (CsUIData.Instance.MaintenanceInfoUrl.Length <= 0)
				{
					CsUIData.Instance.MaintenanceInfoUrl = CsConfiguration.Instance.SystemSetting.MaintenanceInfoUrl;
				}

				RequestClientTexts();
			}
			else
			{
				RequestSupportedLanguages();
            }

            StartCoroutine(LoadFontCoroutine());
        }
        else
        {
			m_csPanelModal.Choice("SystemSettingASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestSupportedLanguages()
    {
        Debug.Log("RequestSupportedLanguages()");
        SupportedLanguagesASCommand cmd = new SupportedLanguagesASCommand();
        cmd.Finished += ResponseSupportedLanguages;
        cmd.Run();
    }

    //string m_strSystemLanguage = "ko";

    //---------------------------------------------------------------------------------------------------
    void ResponseSupportedLanguages(object sender, EventArgs e)
    {
        SupportedLanguagesASCommand cmd = (SupportedLanguagesASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("SupportedLanguagesASCommand : " + cmd.error.Message, ReloadScene, "OK");
            Debug.Log(cmd.error.Message);
            Debug.Log(cmd.Trace());
            return;
        }

        SupportedLanguagesASResponse res = (SupportedLanguagesASResponse)cmd.response;

        if (res.isOK)
        {
            /*EnServerLanguage enServerLanguage = CsUIData.Instance.ConvertToServerLanguage(m_strSystemLanguage);

            bool bExists = res.SupportedLanguageList.ContainsKey((int)enServerLanguage);

            if (bExists)
            {
                CsUIData.Instance.Language = enServerLanguage;
                CsUIData.Instance.MaintenanceInfoUrl = res.SupportedLanguageList[(int)enServerLanguage];
            }
            else
            {
                CsUIData.Instance.Language = (EnServerLanguage)res.DefaultLanague;
				CsUIData.Instance.MaintenanceInfoUrl = res.SupportedLanguageList[res.DefaultLanague];
            }*/

            CsUIData.Instance.Language = (EnServerLanguage)res.DefaultLanague;
            CsUIData.Instance.MaintenanceInfoUrl = res.SupportedLanguageList[res.DefaultLanague];

            Debug.Log("CsUIData.Instance.Language : " + CsUIData.Instance.Language);

            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyLanguage, (int)CsUIData.Instance.Language);
			PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyMaintenanceInfoUrl, CsUIData.Instance.MaintenanceInfoUrl);
            PlayerPrefs.Save();

            RequestClientTexts();
        }
        else
        {
			m_csPanelModal.Choice("SupportedLanguagesASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestClientTexts()
    {
        Debug.Log("RequestClientTexts()");
        int nClientTextVersion = -1;

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyClientTextVersion))
        {
            nClientTextVersion = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyClientTextVersion);
        }

        if (nClientTextVersion == -1 || nClientTextVersion != CsConfiguration.Instance.SystemSetting.ClientTextVersion || !File.Exists(CsConfiguration.Instance.ClientTextSavePath))
        {
            ClientTextsASCommand cmd = new ClientTextsASCommand();
            cmd.languageId = (int)CsUIData.Instance.Language;
            cmd.Finished += ResponseClientTexts;
            cmd.Run();
        }
        else
        {
            if (CsConfiguration.Instance.Dic.Count == 0)
            {
                using (FileStream fileStream = File.OpenRead(CsConfiguration.Instance.ClientTextSavePath))
                {
                    BinaryFormatter binary = new BinaryFormatter();
                    string strBase64String = binary.Deserialize(fileStream) as string;

                    fileStream.Close();
                    fileStream.Dispose();

                    WPDClientTexts clientTexts = new WPDClientTexts();
                    clientTexts.DeserializeFromBase64String(strBase64String);

                    CsConfiguration.Instance.Dic.Clear();

                    foreach (WPDClientText clientText in clientTexts.clientTexts)
                    {
                        CsConfiguration.Instance.Dic.Add(clientText.name.Trim(), clientText.value.Trim());
                    }
                }
            }

            CheckClientStatus();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseClientTexts(object sender, EventArgs e)
    {
        ClientTextsASCommand cmd = (ClientTextsASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("ClientTextsASCommand : " + cmd.error.Message, ReloadScene, "OK");
            Debug.Log(cmd.error.Message);
            Debug.Log(cmd.Trace());
            return;
        }

        ClientTextsASResponse res = (ClientTextsASResponse)cmd.response;

        if (res.isOK)
        {
            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyClientTextVersion, CsConfiguration.Instance.SystemSetting.ClientTextVersion);
            PlayerPrefs.Save();

            CheckClientStatus();
        }
        else
        {
			m_csPanelModal.Choice("ClientTextsASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckClientStatus()
    {
        InitializeUI();

		if (CsConfiguration.Instance.SystemSetting.IsMaintenance)
		{
			// 점검 중.
			m_csPanelModal.Choice(CsConfiguration.Instance.GetString("MAINTAIN_TXT_1"), OnClickOpenHomepage, CsConfiguration.Instance.GetString("MAINTAIN_BTN_1"));
			return;
		}

		// 진행코드.

		m_bIsEndLoading = false;
		m_bIsEndDataLoading = false;

		switch (CsUIData.Instance.IntroShortCutType)
		{
			case EnIntroShortCutType.Logo:
				CsConfiguration.Instance.SendFirebaseLogEvent("intro_start");
				m_trCanvas.Find("Logo").gameObject.SetActive(true);
				StartCoroutine(SetTimeLogoShow());
				break;
			case EnIntroShortCutType.CharacterSelect:
				m_trCanvas.Find("Logo").gameObject.SetActive(false);
				m_trLoading.gameObject.SetActive(true);
				StartCoroutine(ShortCutCharacterSelectCoroutine());
				RequestGameData();
				break;
			case EnIntroShortCutType.LogOut:
				m_trCanvas.Find("Logo").gameObject.SetActive(false);
				m_trLoading.gameObject.SetActive(true);
				StartCoroutine(ShortCutLogOutCoroutine());
				RequestGameData();
				break;
		}
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickOpenHomepage()
	{
		Application.OpenURL(CsUIData.Instance.MaintenanceInfoUrl);
		Application.Quit();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ShortCutCharacterSelectCoroutine()
    {
        StartCoroutine(Load3DCharacterCoroutine());
        StartLoadingImgae();

        while (m_bIsEndLoading == false || m_bIsEndDataLoading == false)
        {
            //로딩중
            yield return null;
        }

        Debug.Log("ShortCutCharacterSelectCoroutine()");
        CheckLogin();

        m_csGameServerSelected = CsConfiguration.Instance.GameServerSelected;

        //yield return new WaitForSeconds(1f);

        //if (m_iEnumerator != null)
        //{
        //    StopCoroutine(m_iEnumerator);
        //}

        //m_iEnumerator = FadeOut(CharacterCheck);
        //StartCoroutine(m_iEnumerator);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator ShortCutLogOutCoroutine()
    {
        //m_userCredentialGuest = null;
        StartCoroutine(Load3DCharacterCoroutine());
        StartLoadingImgae();

        while (m_bIsEndLoading == false || m_bIsEndDataLoading == false)
        {
            //로딩중
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeOut(() => OpenPopupMainLobby(false));
        StartCoroutine(m_iEnumerator);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator SetTimeLogoShow()
    {
        CanvasGroup canvasGroup = m_trCanvas.Find("Logo/ImageLogo").GetComponent<CanvasGroup>();

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime / 1.5f;

            yield return null;
        }
        yield return new WaitForSeconds(1f);

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 1.5f;
            yield return null;
        }


        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeOut(OpenPopupTrailer);
        StartCoroutine(m_iEnumerator);
    }

    //---------------------------------------------------------------------------------------------------
	void OpenPopupTrailer()
	{
		m_bClickSkipButton = false;

		Debug.Log("OpenPopupTrailer()");
		if (m_iEnumerator != null)
		{
			StopCoroutine(m_iEnumerator);
		}

		m_iEnumerator = FadeIn();
		StartCoroutine(m_iEnumerator);

		m_trCanvas.Find("Logo").gameObject.SetActive(false);
		CsConfiguration.Instance.SendFirebaseLogEvent("intro_finish");

		Transform trTrailer = m_trCanvas.Find("Trailer");
		trTrailer.gameObject.SetActive(true);

        Transform trCamera = trTrailer.Find("Camera");
        AudioSource audioSourceTrailer = trCamera.GetComponent<AudioSource>();

        if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.BGM) == 1)
        {
            audioSourceTrailer.enabled = true;
        }
        else
        {
            audioSourceTrailer.enabled = false;
        }

		CsConfiguration.Instance.SendFirebaseLogEvent("movie_start");

        VideoPlayer videoPlayer = trCamera.GetComponent<VideoPlayer>();
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = Application.streamingAssetsPath + "/vod_introtrailer.mp4";
		videoPlayer.isLooping = false;
		StartCoroutine(PlayVideo(videoPlayer));

		Button btSkip = trTrailer.Find("ButtonSkip").GetComponent<Button>();
		btSkip.onClick.RemoveAllListeners();
		btSkip.onClick.AddListener(() => OnClickSkipButton(videoPlayer));
		btSkip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator PlayVideo(VideoPlayer videoPlayer)
	{
		var audioSource = videoPlayer.GetComponent<AudioSource>();
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.controlledAudioTrackCount = 1;
		videoPlayer.EnableAudioTrack(0, true);
		videoPlayer.SetTargetAudioSource(0, audioSource);

		videoPlayer.Prepare();
		while (!videoPlayer.isPrepared && !m_bClickSkipButton)
			yield return null;

		if (!m_bClickSkipButton)
		{
			videoPlayer.Play();
			StartCoroutine(TrailerVideoEndCheckCoroutine(videoPlayer));
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator TrailerVideoEndCheckCoroutine(VideoPlayer videoPlayer)
	{
		while (videoPlayer.isPlaying && !m_bClickSkipButton)
		{
			yield return null;
		}

		if (!m_bClickSkipButton)
		{
			CheckAgreement();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnClickSkipButton(VideoPlayer videoPlayer)
	{
		m_bClickSkipButton = true;

		if (videoPlayer.isPlaying)
		{
			videoPlayer.Stop();
		}

		CheckAgreement();
	}

    //---------------------------------------------------------------------------------------------------
    void CheckAgreement()
    {
		CsConfiguration.Instance.SendFirebaseLogEvent("movie_finish");
        m_trCanvas.Find("Trailer").gameObject.SetActive(false);

        if (m_bIsFirstVisit)
        {
            OpenPopupAgreement();
        }
        else
        {
            RequestGameAssetBundles();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupAgreement()
    {
        Transform trAgreementTerms = m_trCanvas.Find("Terms");

        trAgreementTerms.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void SetLeftAgreementToggle(bool ison)
    {
        if (ison)
        {
            if (m_trCanvas.Find("Terms/RightDisplay/ToggleRight").GetComponent<Toggle>().isOn)
            {
                RequestGameAssetBundles();
            }
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetRightAgreementToggle(bool ison)
    {
        if (ison)
        {
            if (m_trCanvas.Find("Terms/LeftDisplay/ToggleLeft").GetComponent<Toggle>().isOn)
            {
                RequestGameAssetBundles();
            }
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestGameAssetBundles()
    {
        GameAssetBundlesASCommand cmd = new GameAssetBundlesASCommand();
        cmd.Finished += ResponseGameAssetBundles;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseGameAssetBundles(object sender, EventArgs e)
    {
        GameAssetBundlesASCommand cmd = (GameAssetBundlesASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("GameAssetBundlesASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
            return;
        }

        GameAssetBundlesASResponse res = (GameAssetBundlesASResponse)cmd.response;

        if (res.isOK)
        {
            m_csGameAssetBundleList = res.GameSetBundleList;

            StartCoroutine(CheckAssetBundleVersionCoroutine());
        }
        else
        {
			m_csPanelModal.Choice("GameAssetBundlesASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator CheckAssetBundleVersionCoroutine()
    {
        for (int i = 0; i < m_csGameAssetBundleList.Count; i++)
        {
            int nVersion = 0;
            StringBuilder sbUrl = new StringBuilder(CsConfiguration.Instance.SystemSetting.AssetBundleUrl);

#if UNITY_EDITOR || UNITY_ANDROID
            nVersion = m_csGameAssetBundleList[i].AndroidVer;
            sbUrl.Append("Android/");
#elif UNITY_IOS || UNITY_EDITOR_OSX
			nVersion = m_csGameAssetBundleList[i].IosVer;
            sbUrl.Append("iOS/");
#endif
            sbUrl.Append(CsConfiguration.Instance.AssetBundleVersion);
            sbUrl.Append(m_csGameAssetBundleList[i].FileName);

            if (!Caching.IsVersionCached(sbUrl.ToString(), nVersion))
                m_nDownloadAssetBundleCount++;

            yield return null;
        }

        OpenPopupDownloadAssetBundles();

    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupDownloadAssetBundles()
    {
        m_trCanvas.Find("Terms").gameObject.SetActive(false);
        m_trCanvas.Find("ResourcesLoad").gameObject.SetActive(true);

        //버전입력
        Text textVersion = m_trCanvas.Find("ResourcesLoad/TextVersion").GetComponent<Text>();
        CsUIData.Instance.SetFont(textVersion);
        textVersion.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01002"), CsConfiguration.Instance.ClientVersion);
        //textVersion.text = m_strBuildDate;

        /*
		//확인팝업
		m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(true);
		m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonNo").gameObject.SetActive(false);
		Button buttonOk = m_trCanvas.Find("Popup/PopupCheck/ButtonLayout/ButtonOk").GetComponent<Button>();
		buttonOk.gameObject.SetActive(true);
		buttonOk.onClick.RemoveAllListeners();
		buttonOk.onClick.AddListener(() => StartCoroutine(DownloadAssetBundlesCoroutine()));
        buttonOk.onClick.AddListener(() => PlayUISound(EnUISoundTypeIntro.Button));

		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").gameObject.SetActive(true);

		Text textGrid = m_trCanvas.Find("Popup/PopupCheck/TextGrid/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textGrid);
		textGrid.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_0100201"), m_nDownloadAssetBundleCount);
		m_trCanvas.Find("Popup/PopupCheck/TextGrid/Input").gameObject.SetActive(false);
        */

        m_bIsEndLoading = false;
        m_bIsEndDataLoading = false;

        StartCoroutine(BackImageRotation());
        StartCoroutine(StartDownloadAssetSlider());
        RequestGameData();
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator BackImageRotation()
    {
        Transform trImageList = m_trCanvas.Find("ResourcesLoad/LoadingImageList");
        int nNowImageIndex = 1;
        while (true)
        {
            int nNextIndex = nNowImageIndex + 1;
            if (nNextIndex > trImageList.childCount)
            {
                nNextIndex = 1;
            }
            CanvasGroup canvasGroupNowImage = trImageList.Find("ImageBackground" + nNowImageIndex).GetComponent<CanvasGroup>();
            CanvasGroup canvasGroupNextImage = trImageList.Find("ImageBackground" + nNextIndex).GetComponent<CanvasGroup>();
            canvasGroupNextImage.alpha = 1;

            yield return new WaitForSeconds(7f);

            while (canvasGroupNowImage.alpha > 0)
            {
                canvasGroupNowImage.alpha -= Time.deltaTime / 1f;
                yield return null;
            }
            canvasGroupNowImage.transform.SetAsFirstSibling();
            nNowImageIndex++;
            if (nNowImageIndex > trImageList.childCount)
            {
                nNowImageIndex = 1;
            }
            if (trImageList.parent.gameObject.activeSelf == false)
            {
                break;
            }
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator DownloadAssetBundlesCoroutine()
    {
        //팝업창 끄기
        m_trCanvas.Find("Popup/PopupCheck").gameObject.SetActive(false);

        //번들다운 연출추가 데이터로딩연출 추가	       
        StartCoroutine(StartDownloadAssetSlider());

        yield return null;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartDownloadAssetSlider()
    {
        PlayBGM(EnSoundTypeIntro.Intro);
        float flNowValue = 0;
        float flMaxValue = 200;
        while (true)
        {
            //yield return new WaitForSeconds(0.02f);
            flNowValue += 1f;
            if (flNowValue > flMaxValue)
            {
                flNowValue = flMaxValue;
            }
            m_trCanvas.Find("ResourcesLoad/Slider").GetComponent<Slider>().value = flNowValue / flMaxValue;
            //Text textPercent = m_trCanvas.Find("ResourcesLoad/Slider/TextPercent").GetComponent<Text>();
            //CsUIData.Instance.SetFont(textPercent);
            //textPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), (int)(flNowValue / flMaxValue * 100f));

            Text textState = m_trCanvas.Find("ResourcesLoad/Slider/TextState").GetComponent<Text>();
            CsUIData.Instance.SetFont(textState);
            textState.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01004"), flNowValue, flMaxValue);

            if (flNowValue >= flMaxValue)
            {
                yield return new WaitForSeconds(0.2f);

                StartCoroutine(StartDataLoadingSlider());
                break;
            }
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator StartDataLoadingSlider()
    {
        //Text textPercent = m_trCanvas.Find("ResourcesLoad/Slider/TextPercent").GetComponent<Text>();
        //CsUIData.Instance.SetFont(textPercent);

        Text textState = m_trCanvas.Find("ResourcesLoad/Slider/TextState").GetComponent<Text>();
        CsUIData.Instance.SetFont(textState);


        CsEffectPoolManager csEffectPoolManager = GameObject.Find("EffectPool").GetComponent<CsEffectPoolManager>();
        csEffectPoolManager.Init();

        Slider slider = m_trCanvas.Find("ResourcesLoad/Slider").GetComponent<Slider>();
        slider.maxValue = 100;// +csEffectPoolManager.TotalCount;

        //while (csEffectPoolManager.CurrentCount < csEffectPoolManager.TotalCount)
        //{
        //    slider.value = csEffectPoolManager.CurrentCount;            
        //    textPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), ((slider.value / slider.maxValue) * 100f).ToString("00"));
        //    textState.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01004"), slider.value.ToString(), slider.maxValue.ToString());

        //    yield return null;
        //}

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("IntroCharacter", LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            slider.value = asyncOperation.progress * 50f; //+ csEffectPoolManager.TotalCount;
            //textPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), ((slider.value / slider.maxValue) * 100f).ToString("00"));
            textState.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01004"), slider.value.ToString("0"), slider.maxValue.ToString());

            yield return null;
        }
        yield return asyncOperation;


        //yield return asyncOperation;
        asyncOperation = null;

		asyncOperation = SceneManager.LoadSceneAsync("IntroLobby", LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
			slider.value = asyncOperation.progress * 50f + 50f; //+ csEffectPoolManager.TotalCount;
            //textPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), ((slider.value / slider.maxValue) * 100f).ToString("00"));
            textState.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01004"), slider.value.ToString("0"), slider.maxValue.ToString());

            yield return null;
        }
        yield return asyncOperation;

		m_trLightList = GameObject.Find("LightList").transform;
        m_trIntroRobby = GameObject.Find("IntroLobby").transform;
        m_trIntroRobby.gameObject.SetActive(false);

        CreateCharacterModel();

        while (m_bIsEndLoading == false || m_bIsEndDataLoading == false)
        {
            yield return null;
        }

        slider.value = slider.maxValue;
        //textPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), 100);
        textState.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01004"), slider.maxValue.ToString(), slider.maxValue.ToString());

        if (m_iEnumerator != null)
        {
            StopCoroutine(m_iEnumerator);
        }

        m_iEnumerator = FadeOut(CheckLogin);
        StartCoroutine(m_iEnumerator);
    }

    //---------------------------------------------------------------------------------------------------
    void RequestGameData()
    {
        Debug.Log("RequestGameData()");
        int nGameMetaDataVersion = -1;

        if (PlayerPrefs.HasKey(CsConfiguration.Instance.PlayerPrefsKeyGameMetaDataVersion))
        {
            nGameMetaDataVersion = PlayerPrefs.GetInt(CsConfiguration.Instance.PlayerPrefsKeyGameMetaDataVersion);
        }

        if (nGameMetaDataVersion == -1 || nGameMetaDataVersion != CsConfiguration.Instance.SystemSetting.MetaDataVersion || !File.Exists(CsConfiguration.Instance.GameMetaDataSavePath))
        {
            GameDatasASCommand cmdData = new GameDatasASCommand();
            cmdData.Finished += ResponseGameData;
            cmdData.Run();
        }
        else
        {
            using (FileStream fsGameMetaData = File.OpenRead(CsConfiguration.Instance.GameMetaDataSavePath))
            {
                BinaryFormatter binary = new BinaryFormatter();
                string strBase64String = binary.Deserialize(fsGameMetaData) as string;
                fsGameMetaData.Close();
                fsGameMetaData.Dispose();

                WPDGameDatas gameData = new WPDGameDatas();
                gameData.DeserializeFromBase64String(strBase64String);

                ParseGameMetaData(gameData);
            }

            // 게임서버 목록.
            RequestGameServers();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseGameData(object sender, EventArgs e)
    {
        GameDatasASCommand cmd = (GameDatasASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("GameDatasASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
            return;
        }

        GameDatasASResponse res = (GameDatasASResponse)cmd.response;

        if (res.isOK)
        {
            ParseGameMetaData(res.GameData);

            PlayerPrefs.SetInt(CsConfiguration.Instance.PlayerPrefsKeyGameMetaDataVersion, CsConfiguration.Instance.SystemSetting.MetaDataVersion);
            PlayerPrefs.Save();

            // 게임서버 목록.
            RequestGameServers();
        }
        else
        {
			m_csPanelModal.Choice("GameDatasASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestGameServers()
    {
        GameServersASCommand cmd = new GameServersASCommand();
        Debug.Log("RequestGameServers()");
        cmd.Finished += ResponseGameServers;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseGameServers(object sender, EventArgs e)
    {
        GameServersASCommand cmd = (GameServersASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("GameServersASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
            return;
        }

        GameServersASResponse res = (GameServersASResponse)cmd.response;

        if (res.isOK)
        {
			m_listCsGameServerRegion = res.GameServerRegionList;
            m_bIsEndDataLoading = true;
        }
        else
        {
			m_csPanelModal.Choice("GameServersASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckLogin()
    {
        Debug.Log("CheckLogin()");

        if (m_userCredential == null)
        {
            m_bIsFirstLogin = true;
            OpenPopupMainLobby(false);
            //RequestLoginWithGuest();
        }
        else
        {
            m_bIsFirstLogin = false;
            RequestLoginAS(m_userCredential.userId, m_userCredential.userSecret);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestLoginAS(string sUserId, string sUserSecret)
    {
        LoginASCommand cmd = new LoginASCommand();
        cmd.Finished += ResponseLoginAS;
        cmd.userId = sUserId;
        cmd.userSecret = sUserSecret;
        cmd.languageId = (int)CsUIData.Instance.Language;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseLoginAS(object sender, EventArgs e)
    {
        LoginASCommand cmd = (LoginASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("LoginASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        LoginASResponse res = (LoginASResponse)cmd.response;

        if (res.isOK)
        {
            // 현재 사용자 저장.
            m_user = res.User;
            m_user.UserId = cmd.userId;

            // 사용자 증명 저장
            SetUserCredential(m_user.UserId, cmd.userSecret);

            // Guest사용자 증명 저장
            if (m_user.UserType == (int)EnUserType.Guest)
            {
                SetGuestUserCredential(m_user.UserId, cmd.userSecret);
            }

            //RequestUserGameServers();
			CsConfiguration.Instance.SendFirebaseLogEvent("login_finish");

            RequestUserHeroes();
        }
        else
        {
			m_csPanelModal.Choice("LoginASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    private void SetUserCredential(string strUserId, string strUserSecret)
    {
        UserCredential userCredential = null;

        if (!string.IsNullOrEmpty(strUserId) && !string.IsNullOrEmpty(strUserSecret))
            userCredential = new UserCredential(strUserId, strUserSecret);

        m_userCredential = userCredential;

        SaveUserCredential();
    }

    //---------------------------------------------------------------------------------------------------
    private void SaveUserCredential()
    {
        string strUserId = null;
        string strUserSecret = null;

        if (m_userCredential != null)
        {
            strUserId = m_userCredential.userId;
            strUserSecret = m_userCredential.userSecret;
        }

        PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyUserCredentialId, strUserId);
        PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyUserCredentialSecret, strUserSecret);

        PlayerPrefs.Save();
    }

    //---------------------------------------------------------------------------------------------------
    private void SetGuestUserCredential(string strUserId, string strUserSecret)
    {
        UserCredential userCredential = null;

        if (!string.IsNullOrEmpty(strUserId) && !string.IsNullOrEmpty(strUserSecret))
            userCredential = new UserCredential(strUserId, strUserSecret);

        m_userCredentialGuest = userCredential;

        SaveGuestUserCredential();
    }

    //---------------------------------------------------------------------------------------------------
    private void SaveGuestUserCredential()
    {
        string strUserId = null;
        string strUserSecret = null;

        if (m_userCredentialGuest != null)
        {
            strUserId = m_userCredentialGuest.userId;
            strUserSecret = m_userCredentialGuest.userSecret;
        }

        PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyGuestUserCredentialId, strUserId);
        PlayerPrefs.SetString(CsConfiguration.Instance.PlayerPrefsKeyGuestUserCredentialSecret, strUserSecret);

        PlayerPrefs.Save();
    }

    //---------------------------------------------------------------------------------------------------
    void RequestLoginWithGoogle()
    {
        LoginWithGoogleNACommand cmd = new LoginWithGoogleNACommand();
        cmd.Finished += ResponseLoginWithGoogle;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseLoginWithGoogle(object sender, EventArgs e)
    {
        LoginWithGoogleNACommand cmd = (LoginWithGoogleNACommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("LoginWithGoogleNACommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
            return;
        }

        LoginWithGoogleNAResponse res = (LoginWithGoogleNAResponse)cmd.response;

        if (res.isOK)
        {
            CreateGoogleUserASCommand createUserCmd = new CreateGoogleUserASCommand();
            createUserCmd.Finished += ResponseCreateGoogleUser;
            createUserCmd.googleUserId = res.googleUserId;
            createUserCmd.Run();
        }
        else
        {
			if (res.result == LoginWithGoogleNAResponse.kResult_Canceled)
			{
				m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A167_TXT_00001"), "OK");
				return;
			}
			else
			{
				m_csPanelModal.Choice("LoginWithGoogleNAResponse : " + res.errorMessage, ReloadScene, "OK");
				Debug.Log(res.errorMessage);
				Debug.Log(res.Trace());
				return;
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseCreateGoogleUser(object sender, EventArgs e)
    {
        CreateGoogleUserASCommand cmd = (CreateGoogleUserASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("CreateGoogleUserASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        CreateGoogleUserASResponse res = (CreateGoogleUserASResponse)cmd.response;

        if (res.isOK)
        {
            RequestLoginAS(res.userId, res.userSecret);
        }
        else
        {
			m_csPanelModal.Choice("CreateGoogleUserASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestLoginWithFacebook()
    {
        LoginWithFacebookNACommand cmd = new LoginWithFacebookNACommand();
        cmd.Finished += ResponseLoginWithFacebook;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseLoginWithFacebook(object sender, EventArgs e)
    {
        LoginWithFacebookNACommand cmd = (LoginWithFacebookNACommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("LoginWithFacebookNACommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
            return;
        }

        LoginWithFacebookNAResponse res = (LoginWithFacebookNAResponse)cmd.response;

        if (res.isOK)
        {
            CreateFacebookUserASCommand createUserCmd = new CreateFacebookUserASCommand();
            createUserCmd.Finished += ResponseCreateFacebookUser;
            createUserCmd.facebookAppId = res.facebookAppId;
            createUserCmd.facebookUserId = res.facebookUserId;
            createUserCmd.Run();
        }
        else
        {
			if (res.result == LoginWithFacebookNAResponse.kResult_Canceled)
			{
				m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A167_TXT_00001"), "OK");
				return;
			}
			else
			{
				m_csPanelModal.Choice("LoginWithFacebookNAResponse : " + res.errorMessage, ReloadScene, "OK");
				Debug.Log(res.errorMessage);
				Debug.Log(res.Trace());
				return;
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseCreateFacebookUser(object sender, EventArgs e)
    {
        CreateFacebookUserASCommand cmd = (CreateFacebookUserASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("CreateFacebookUserASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        CreateFacebookUserASResponse res = (CreateFacebookUserASResponse)cmd.response;

        if (res.isOK)
        {
            RequestLoginAS(res.userId, res.userSecret);
        }
        else
        {
			m_csPanelModal.Choice("CreateFacebookUserASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestLoginWithGuest()
    {
        if (m_userCredentialGuest != null)
        {
            m_bIsFirstLogin = false;
            RequestLoginAS(m_userCredentialGuest.userId, m_userCredentialGuest.userSecret);
        }
        else
        {
            //경고문구 띄우기
            m_bIsFirstLogin = true;
            m_csPanelModal.Choice(CsConfiguration.Instance.GetString("A01_TXT_00050"), OnClickGuestLoginOK, CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), null, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuestLoginOK()
    {
        CreateGuestUserASCommand cmd = new CreateGuestUserASCommand();
        cmd.Finished += ResponseCreateGuestUser;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseCreateGuestUser(object sender, EventArgs e)
    {
        CreateGuestUserASCommand cmd = (CreateGuestUserASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("CreateGuestUserASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        CreateGuestUserASResponse res = (CreateGuestUserASResponse)cmd.response;

        if (res.isOK)
        {
            RequestLoginAS(res.userId, res.userSecret);
        }
        else
        {
			m_csPanelModal.Choice("CreateGuestUserASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void RequestLoginWithEntermate()
    {
        LoginWithEntermateNACommand cmd = new LoginWithEntermateNACommand();
        cmd.Finished += ResponseLoginWithEntermate;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseLoginWithEntermate(object sender, EventArgs e)
    {
        LoginWithEntermateNACommand cmd = (LoginWithEntermateNACommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("LoginWithEntermateNACommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        LoginWithEntermateNAResponse res = (LoginWithEntermateNAResponse)cmd.response;

        if (res.isOK)
        {
            CreateEntermateUserASCommand createUserCmd = new CreateEntermateUserASCommand();
            createUserCmd.Finished += ResponseCreateEntermateUser;
            createUserCmd.entermateUserId = res.entermateUserId;
            createUserCmd.entermatePrivateKey = res.entermatePrivateKey;
            createUserCmd.Run();
        }
        else
        {
			m_csPanelModal.Choice("LoginWithEntermateNAResponse : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseCreateEntermateUser(object sender, EventArgs e)
    {
        CreateEntermateUserASCommand cmd = (CreateEntermateUserASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("CreateEntermateUserASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        CreateEntermateUserASResponse res = (CreateEntermateUserASResponse)cmd.response;

        if (res.isOK)
        {
            RequestLoginAS(res.userId, res.userSecret);
        }
        else
        {
			m_csPanelModal.Choice("CreateEntermateUserASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    //---------------------------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------------------------
    void RequestUserHeroes()
    {
		Debug.Log("RequestUserHeroes");
        UserHerosASCommand cmd = new UserHerosASCommand();
        cmd.UserAccessToken = m_user.AccessToken;
        cmd.Finished += ResponseUserHeroes;
        cmd.Run();
    }

    //---------------------------------------------------------------------------------------------------
    void ResponseUserHeroes(object sender, EventArgs e)
    {
        UserHerosASCommand cmd = (UserHerosASCommand)sender;

        if (!cmd.isOK)
        {
			m_csPanelModal.Choice("UserHerosASCommand : " + cmd.error.Message, ReloadScene, "OK");
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
        }

        UserHerosASResponse res = (UserHerosASResponse)cmd.response;

        if (res.isOK)
        {
            m_listCsCsUserHero = res.UserHeroList;

            for (int i = 0; i < m_listCsCsUserHero.Count; i++)
            {
				m_listCsCsUserHero[i].GameServer = GetGameServer(m_listCsCsUserHero[i].VirtualGameServerId);
            }

            if (CsUIData.Instance.IntroShortCutType == EnIntroShortCutType.Logo)
            {
                if (m_bIsFirstLogin)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A01_TXT_02001"));
                }
                else
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A01_TXT_02002"));
                }

                OnClickClosePopupLogin();
                OpenPopupMainLobby(true);
            }
			else if(CsUIData.Instance.IntroShortCutType == EnIntroShortCutType.LogOut)
			{
				OnClickClosePopupLogin();
				OpenPopupMainLobby(true);
			}
			else if (CsUIData.Instance.IntroShortCutType == EnIntroShortCutType.CharacterSelect)
			{
				if (m_iEnumerator != null)
				{
					StopCoroutine(m_iEnumerator);
				}

				m_iEnumerator = FadeOut(CharacterCheck);
				StartCoroutine(m_iEnumerator);
			}
        }
        else
        {
			m_csPanelModal.Choice("UserHerosASResponse : " + res.errorMessage, ReloadScene, "OK");
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
        }
    }

    ////---------------------------------------------------------------------------------------------------
    //void RequestUserGameServers()
    //{
    //	UserGameServersASCommand cmd = new UserGameServersASCommand();
    //	cmd.UserAccessToken = m_user.AccessToken;
    //	cmd.Finished += ResponseUserGameServers;
    //	cmd.Run();
    //}

    ////---------------------------------------------------------------------------------------------------
    //void ResponseUserGameServers(object sender, EventArgs e)
    //{
    //	UserGameServersASCommand cmd = (UserGameServersASCommand)sender;

    //	if (!cmd.isAllOK)
    //	{
    //		//Debug.Log(cmd.error.Message);
    //		Debug.Log(cmd.Trace());
    //		return;
    //	}

    //	UserGameServersASResponse res = (UserGameServersASResponse)cmd.response;

    //	if (res.isOK)
    //	{
    //		m_csUserGameServerList = res.UserGameServerList;

    //		for (int i = 0; i < m_csUserGameServerList.Count; i++)
    //		{
    //			m_csUserGameServerList[i].GameServer = GetGameServer(m_csUserGameServerList[i].VirtualGameServerId);
    //		}

    //		if (CsUIData.Instance.IntroShortCutType == EnIntroShortCutType.Logo)
    //		{
    //               if (m_bIsFirstLogin)
    //               {
    //                   CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A01_TXT_02001"));
    //               }
    //               else
    //               {
    //                   CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A01_TXT_02002"));
    //               }

    //               OnClickClosePopupLogin();
    //			OpenPopupMainLobby(true);
    //		}
    //	}
    //	else
    //	{

    //	}
    //}

    //---------------------------------------------------------------------------------------------------
    void ConnectGameServerServer()
    {
        m_bIsTouched = false;
        CsRplzSession.Instance.Init(m_csGameServerSelected.ServerAddress, "ProxyServer");
        CsConfiguration.Instance.GameServerSelected = m_csGameServerSelected;
    }

    //---------------------------------------------------------------------------------------------------
    CsGameServer GetGameServer(int nVirtualServerId)
    {
		foreach (CsGameServerRegion csGameServerRegion in m_listCsGameServerRegion)
		{
			foreach (CsGameServerGroup csGameServerGroup in csGameServerRegion.GameServerGroupList)
			{
				CsGameServer csGameServer = csGameServerGroup.GetGameServer(nVirtualServerId);

				if (csGameServer != null)
				{
					return csGameServer;
				}
			}
		}

        return null;
    }

	//---------------------------------------------------------------------------------------------------
	CsGameServerGroup GetGameServerGroup(int nGameServerGroupId)
	{
		foreach (CsGameServerRegion csGameServerRegion in m_listCsGameServerRegion)
		{
			CsGameServerGroup csGameServerGroup = csGameServerRegion.GetGameServerGroup(nGameServerGroupId);

			if (csGameServerGroup != null)
			{
				return csGameServerGroup;
			}
		}

		return null;
	}

	//---------------------------------------------------------------------------------------------------
	void Init3DCharacter()
    {
        Transform trCharList = GameObject.Find("CharacterList").transform;
        for (int i = 0; i < CsGameData.Instance.JobList.Count; i++)
        {
            int nId = CsGameData.Instance.JobList[i].JobId;

            if (trCharList.Find("Character" + nId) != null)
            {
                Transform trCharacter = trCharList.Find("Character" + nId);
                trCharacter.gameObject.SetActive(false);
            }

            if (trCharList.Find("CustomCharacter" + nId) != null)
            {
                trCharList.Find("CustomCharacter" + nId).gameObject.SetActive(false);
            }
        }
    }

    /*
	//---------------------------------------------------------------------------------------------------
	void InitBase3DCharacter()
	{
		Transform trCharList = GameObject.Find("CharacterListBase").transform;
		for (int i = 0; i < CsGameData.Instance.JobList.Count; i++)
		{
			if (trCharList.Find("Character" + (i+1)) != null)
			{
				trCharList.Find("Character" + (i+1)).gameObject.SetActive(false);
			}
		}
	}
    */

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSceneMainUICoroutine()
    {
		CsConfiguration.Instance.SendFirebaseLogEvent("game_enter");

        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("MainUI");
        yield return asyncOperation;

        m_bIsProcess = true;
    }

    //---------------------------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------------------------
    void ParseGameMetaData(WPDGameDatas gameData)
    {
        Debug.Log("ParseGameMetaData()");
        //WPDGameConfig	게임설정 정보
        CsGameConfig.Instance.Initialize(gameData.gameConfig);

        // WPDElemental[] 원소 목록
        CsGameData.Instance.ElementalList.Clear();

        for (int i = 0; i < gameData.elementals.Length; i++)
        {
            CsGameData.Instance.ElementalList.Add(new CsElemental(gameData.elementals[i]));
        }

        // WPDExpReward[] 경험치보상 목록
        CsGameData.Instance.ExpRewardDictionary.Clear();

        for (int i = 0; i < gameData.expRewards.Length; i++)
        {
            CsGameData.Instance.ExpRewardDictionary.Add(gameData.expRewards[i].expRewardId, new CsExpReward(gameData.expRewards[i]));
        }

        // WPDGoldReward[] 골드보상 목록
        CsGameData.Instance.GoldRewardDictionary.Clear();

        for (int i = 0; i < gameData.goldRewards.Length; i++)
        {
            CsGameData.Instance.GoldRewardDictionary.Add(gameData.goldRewards[i].goldRewardId, new CsGoldReward(gameData.goldRewards[i]));
        }

        // WPDJob[] 직업 목록
        CsGameData.Instance.JobList.Clear();

        for (int i = 0; i < gameData.jobs.Length; i++)
        {
            CsGameData.Instance.JobList.Add(new CsJob(gameData.jobs[i]));
        }

        // WPDNation[] 국가 목록
        CsGameData.Instance.NationList.Clear();

        for (int i = 0; i < gameData.nations.Length; i++)
        {
            CsGameData.Instance.NationList.Add(new CsNation(gameData.nations[i]));
        }

        //WPDItemMainCategory[] 아이템 메인 카테고리 목록
        CsGameData.Instance.ItemMainCategoryList.Clear();

        for (int i = 0; i < gameData.itemMainCategories.Length; i++)
        {
            CsGameData.Instance.ItemMainCategoryList.Add(new CsItemMainCategory(gameData.itemMainCategories[i]));
        }

        //WPDItemSubCategory[] 아이템 서브 카테고리 목록
        for (int i = 0; i < gameData.itemSubCategories.Length; i++)
        {
            CsItemMainCategory csItemMainCategory = CsGameData.Instance.GetItemMainCategory(gameData.itemSubCategories[i].mainCategoryId);

            if (csItemMainCategory != null)
            {
                csItemMainCategory.ItemSubCategoryList.Add(new CsItemSubCategory(gameData.itemSubCategories[i]));
            }
        }

        //WPDItemType[]	아이템타입 목록
        CsGameData.Instance.ItemTypeList.Clear();

        for (int i = 0; i < gameData.itemTypes.Length; i++)
        {
            CsGameData.Instance.ItemTypeList.Add(new CsItemType(gameData.itemTypes[i]));
        }

        //WPDItemGrade[]	아이템등급 목록
        CsGameData.Instance.ItemGradeList.Clear();

        for (int i = 0; i < gameData.itemGrades.Length; i++)
        {
            CsGameData.Instance.ItemGradeList.Add(new CsItemGrade(gameData.itemGrades[i]));
        }

        //WPDItem[]	아이템 목록
        CsGameData.Instance.ItemList.Clear();

        for (int i = 0; i < gameData.items.Length; i++)
        {
            CsGameData.Instance.ItemList.Add(new CsItem(gameData.items[i]));
        }

        // WPDItemReward[] 아이템보상 목록
        CsGameData.Instance.ItemRewardDictionary.Clear();

        for (int i = 0; i < gameData.itemRewards.Length; i++)
        {
            CsGameData.Instance.ItemRewardDictionary.Add(gameData.itemRewards[i].itemRewardId, new CsItemReward(gameData.itemRewards[i]));
        }

        // WPDAttr[] 속성 목록
        CsGameData.Instance.AttrList.Clear();

        for (int i = 0; i < gameData.attrs.Length; i++)
        {
            CsGameData.Instance.AttrList.Add(new CsAttr(gameData.attrs[i]));
        }

        // WPDAttrValue[] 속성값 목록
        CsGameData.Instance.AttrValueInfoDictionary.Clear();

        for (int i = 0; i < gameData.attrValues.Length; i++)
        {
            CsGameData.Instance.AttrValueInfoDictionary.Add(gameData.attrValues[i].attrValueId, new CsAttrValueInfo(gameData.attrValues[i]));
        }

        //WPDMainGearCategory[] 메인기어 카테고리
        CsGameData.Instance.MainGearCategoryList.Clear();

        for (int i = 0; i < gameData.mainGearCategories.Length; i++)
        {
            CsGameData.Instance.MainGearCategoryList.Add(new CsMainGearCategory(gameData.mainGearCategories[i]));
        }

        //WPDMainGearType[] 메인장비 타입 목록
        CsGameData.Instance.MainGearTypeList.Clear();

        for (int i = 0; i < gameData.mainGearTypes.Length; i++)
        {
            CsGameData.Instance.MainGearTypeList.Add(new CsMainGearType(gameData.mainGearTypes[i]));
        }

        //WPDMainGearTier[] 메인장비 티어 목록
        CsGameData.Instance.MainGearTierList.Clear();

        for (int i = 0; i < gameData.mainGearTiers.Length; i++)
        {
            CsGameData.Instance.MainGearTierList.Add(new CsMainGearTier(gameData.mainGearTiers[i]));
        }

        //WPDMainGearGrade[] 메인장비 등급 목록
        CsGameData.Instance.MainGearGradeList.Clear();

        for (int i = 0; i < gameData.mainGearGrades.Length; i++)
        {
            CsGameData.Instance.MainGearGradeList.Add(new CsMainGearGrade(gameData.mainGearGrades[i]));
        }

        //WPDMainGearQuality[] 메인장비 품질 목록
        CsGameData.Instance.MainGearQualityList.Clear();

        for (int i = 0; i < gameData.mainGearQualities.Length; i++)
        {
            CsGameData.Instance.MainGearQualityList.Add(new CsMainGearQuality(gameData.mainGearQualities[i]));
        }

        //WPDMainGear[] 메인장비 목록
        CsGameData.Instance.MainGearList.Clear();

        for (int i = 0; i < gameData.mainGears.Length; i++)
        {
            CsGameData.Instance.MainGearList.Add(new CsMainGear(gameData.mainGears[i]));
        }

        //WPDMainGearBaseAttr[] 메인장비 기본속성 목록
        for (int i = 0; i < gameData.mainGearBaseAttrs.Length; i++)
        {
            CsMainGear csMainGear = CsGameData.Instance.GetMainGear(gameData.mainGearBaseAttrs[i].mainGearId);

            if (csMainGear != null)
            {
                csMainGear.MainGearBaseAttrList.Add(new CsMainGearBaseAttr(gameData.mainGearBaseAttrs[i]));
            }
        }

        //WPDMainGearBaseAttrEnchantLevel[] 메인장비 기본속성 강화레벨 목록
        for (int i = 0; i < gameData.mainGearBaseAttrEnchantLevels.Length; i++)
        {
            CsMainGear csMainGear = CsGameData.Instance.GetMainGear(gameData.mainGearBaseAttrEnchantLevels[i].mainGearId);

            if (csMainGear != null)
            {
                CsMainGearBaseAttr csMainGearBaseAttr = csMainGear.GetMainGearBaseAttr(gameData.mainGearBaseAttrEnchantLevels[i].attrId);

                if (csMainGearBaseAttr != null)
                {
                    csMainGearBaseAttr.MainGearBaseAttrEnchantLevelList.Add(new CsMainGearBaseAttrEnchantLevel(gameData.mainGearBaseAttrEnchantLevels[i]));
                }
            }
        }

        //WPDMainGearEnchantStep[] 메인장비 강화 단계 목록
        CsGameData.Instance.MainGearEnchantStepList.Clear();

        for (int i = 0; i < gameData.mainGearEnchantSteps.Length; i++)
        {
            CsGameData.Instance.MainGearEnchantStepList.Add(new CsMainGearEnchantStep(gameData.mainGearEnchantSteps[i]));
        }

        //WPDMainGearEnchantLevel[] 메인장비 강화 레벨 목록
        CsGameData.Instance.MainGearEnchantLevelList.Clear();

        for (int i = 0; i < gameData.mainGearEnchantLevels.Length; i++)
        {
            CsGameData.Instance.MainGearEnchantLevelList.Add(new CsMainGearEnchantLevel(gameData.mainGearEnchantLevels[i]));
        }

        //WPDMainGearOptionAttrGrade[] 메인장비 옵션속성 등급 목록
        CsGameData.Instance.MainGearOptionAttrGradeList.Clear();

        for (int i = 0; i < gameData.mainGearOptionAttrGrades.Length; i++)
        {
            CsGameData.Instance.MainGearOptionAttrGradeList.Add(new CsMainGearOptionAttrGrade(gameData.mainGearOptionAttrGrades[i]));
        }

        // WPDMainGearRefinementRecipe[] 메인장비 세련 레시피 목록
        CsGameData.Instance.MainGearRefinementRecipeList.Clear();

        for (int i = 0; i < gameData.mainGearRefinementRecipes.Length; i++)
        {
            CsGameData.Instance.MainGearRefinementRecipeList.Add(new CsMainGearRefinementRecipe(gameData.mainGearRefinementRecipes[i]));
        }

        //WPDSubGearGrade[] 보조장비 등급 목록
        CsGameData.Instance.SubGearGradeList.Clear();

        for (int i = 0; i < gameData.subGearGrades.Length; i++)
        {
            CsGameData.Instance.SubGearGradeList.Add(new CsSubGearGrade(gameData.subGearGrades[i]));
        }

        //WPDSubGear[] 보조장비 목록
        CsGameData.Instance.SubGearList.Clear();

        for (int i = 0; i < gameData.subGears.Length; i++)
        {
            CsGameData.Instance.SubGearList.Add(new CsSubGear(gameData.subGears[i]));
        }

        //WPDSubGearName[] 보조장비 이름 목록
        for (int i = 0; i < gameData.subGearNames.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearNames[i].subGearId);

            if (csSubGear != null)
            {
                csSubGear.SubGearNameList.Add(new CsSubGearName(gameData.subGearNames[i]));
            }

        }

        // WPDSubGearRuneSocke[] 보조장비 룬 소켓 목록
        for (int i = 0; i < gameData.subGearRuneSockets.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearRuneSockets[i].subGearId);

            if (csSubGear != null)
            {
                csSubGear.SubGearRuneSocketList.Add(new CsSubGearRuneSocket(gameData.subGearRuneSockets[i]));
            }
        }

        // WPDSubGearRuneSocketAvailableItemType[]	보조장비 룬 소켓 장착가능 아이템 타입 목록
        for (int i = 0; i < gameData.subGearRuneSocketAvailableItemTypes.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearRuneSocketAvailableItemTypes[i].subGearId);

            if (csSubGear != null)
            {
                CsSubGearRuneSocket csSubGearRuneSocket = csSubGear.GetSubGearRuneSocket(gameData.subGearRuneSocketAvailableItemTypes[i].socketIndex);

                if (csSubGearRuneSocket != null)
                {
                    csSubGearRuneSocket.SubGearRuneSocketAvailableItemTypeList.Add(new CsSubGearRuneSocketAvailableItemType(gameData.subGearRuneSocketAvailableItemTypes[i]));
                }
            }
        }


        //WPDSubGearGemSocket[] 보조장비 소울스톤 소켓 목록
        for (int i = 0; i < gameData.subGearSoulstoneSockets.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearSoulstoneSockets[i].subGearId);

            if (csSubGear != null)
            {
                csSubGear.SubGearSoulstoneSocketList.Add(new CsSubGearSoulstoneSocket(gameData.subGearSoulstoneSockets[i]));
            }
        }

        //WPDSubGearAttr[] 보조장비 속성 목록
        for (int i = 0; i < gameData.subGearAttrs.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearAttrs[i].subGearId);

            if (csSubGear != null)
            {
                csSubGear.SubGearAttrList.Add(new CsSubGearAttr(gameData.subGearAttrs[i]));
            }
        }

        //WPDSubGearAttrValue[] 보조장비 속성값 목록
        for (int i = 0; i < gameData.subGearAttrValues.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearAttrValues[i].subGearId);

            if (csSubGear != null)
            {
                CsSubGearAttr csSubGearAttr = csSubGear.GetSubGearAttr(gameData.subGearAttrValues[i].attrId);

                if (csSubGearAttr != null)
                {
                    csSubGearAttr.SubGearAttrValueList.Add(new CsSubGearAttrValue(gameData.subGearAttrValues[i]));
                }
            }
        }

        //WPDSubGearLevel[] 보조장비 레벨 목록
        for (int i = 0; i < gameData.subGearLevels.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearLevels[i].subGearId);

            if (csSubGear != null)
            {
                csSubGear.SubGearLevelList.Add(new CsSubGearLevel(gameData.subGearLevels[i]));
            }
        }

        //WPDSubGearLevelQuality[] 보조장비 레벨 품질 목록
        for (int i = 0; i < gameData.subGearLevelQualities.Length; i++)
        {
            CsSubGear csSubGear = CsGameData.Instance.GetSubGear(gameData.subGearLevelQualities[i].subGearId);

            if (csSubGear != null)
            {
                CsSubGearLevel csSubGearLevel = csSubGear.GetSubGearLevel(gameData.subGearLevelQualities[i].level);

                if (csSubGearLevel != null)
                {
                    csSubGearLevel.SubGearLevelQualityList.Add(new CsSubGearLevelQuality(gameData.subGearLevelQualities[i]));
                }
            }
        }

        //WPDMountQualityMaster[] 탈것 품질 마스터 목록
        CsGameData.Instance.MountQualityMasterList.Clear();

        for (int i = 0; i < gameData.mountQualityMasters.Length; i++)
        {
            CsGameData.Instance.MountQualityMasterList.Add(new CsMountQualityMaster(gameData.mountQualityMasters[i]));
        }

        //WPDMountLevelMaster[] 탈것 레벨 마스터 목록
        CsGameData.Instance.MountLevelMasterList.Clear();

        for (int i = 0; i < gameData.mountLevelMasters.Length; i++)
        {
            CsGameData.Instance.MountLevelMasterList.Add(new CsMountLevelMaster(gameData.mountLevelMasters[i]));
        }

        //WPDMount[] 탈것 목록
        CsGameData.Instance.MountList.Clear();

        for (int i = 0; i < gameData.mounts.Length; i++)
        {
            CsGameData.Instance.MountList.Add(new CsMount(gameData.mounts[i]));
        }

        //WPDMountQuality[] 탈것 품질 목록
        for (int i = 0; i < gameData.mountQualities.Length; i++)
        {
            CsMount csMount = CsGameData.Instance.GetMount(gameData.mountQualities[i].mountId);

            if (csMount != null)
            {
                csMount.MountQualityList.Add(new CsMountQuality(gameData.mountQualities[i]));
            }
        }

        //WPDMountLevel[] 탈것 레벨 목록
        for (int i = 0; i < gameData.mountLevels.Length; i++)
        {
            CsMount csMount = CsGameData.Instance.GetMount(gameData.mountLevels[i].mountId);

            if (csMount != null)
            {
                csMount.MountLevelList.Add(new CsMountLevel(gameData.mountLevels[i]));
            }
        }

        //WPDMountGearType[] 탈것 장비 타입 목록
        CsGameData.Instance.MountGearTypeList.Clear();

        for (int i = 0; i < gameData.mountGearTypes.Length; i++)
        {
            CsGameData.Instance.MountGearTypeList.Add(new CsMountGearType(gameData.mountGearTypes[i]));
        }

        //WPDMountGearSlot[] 탈것 장비 슬롯 목록
        for (int i = 0; i < gameData.mountGearSlots.Length; i++)
        {
            CsMountGearType csMountGearType = CsGameData.Instance.GetMountGearTypeBySlotIndex(gameData.mountGearSlots[i].slotIndex);

            if (csMountGearType != null)
            {
                csMountGearType.MountGearSlot = new CsMountGearSlot(gameData.mountGearSlots[i]);
            }
        }

        //WPDMountGearGrade[] 탈것 장비 등급 목록
        CsGameData.Instance.MountGearGradeList.Clear();

        for (int i = 0; i < gameData.mountGearGrades.Length; i++)
        {
            CsGameData.Instance.MountGearGradeList.Add(new CsMountGearGrade(gameData.mountGearGrades[i]));
        }

        //WPDMountGearQuality[] 탈것 장비 품질 목록
        CsGameData.Instance.MountGearQualityList.Clear();

        for (int i = 0; i < gameData.mountGearQualities.Length; i++)
        {
            CsGameData.Instance.MountGearQualityList.Add(new CsMountGearQuality(gameData.mountGearQualities[i]));
        }

        //WPDMountGear[] 탈것 장비 목록
        CsGameData.Instance.MountGearList.Clear();

        for (int i = 0; i < gameData.mountGears.Length; i++)
        {
            CsGameData.Instance.MountGearList.Add(new CsMountGear(gameData.mountGears[i]));
        }

        //WPDMountGearOptionAttrGrade[] 탈것 장비 옵션 속성 등급 목록
        CsGameData.Instance.MountGearOptionAttrGradeList.Clear();

        for (int i = 0; i < gameData.mountGearOptionAttrGrades.Length; i++)
        {
            CsGameData.Instance.MountGearOptionAttrGradeList.Add(new CsMountGearOptionAttrGrade(gameData.mountGearOptionAttrGrades[i]));
        }

        //WPDMountGearPickBoxRecipe[] 탈것 장비 뽑기상자 레시피 목록
        CsGameData.Instance.MountGearPickBoxRecipeList.Clear();

        for (int i = 0; i < gameData.mountGearPickBoxRecipes.Length; i++)
        {
            CsGameData.Instance.MountGearPickBoxRecipeList.Add(new CsMountGearPickBoxRecipe(gameData.mountGearPickBoxRecipes[i]));
        }

        //WPDLocation[] 위치 목록
        CsGameData.Instance.LocationList.Clear();

        for (int i = 0; i < gameData.locations.Length; i++)
        {
            CsGameData.Instance.LocationList.Add(new CsLocation(gameData.locations[i]));
        }

        //WPDMonsterCharacter[] 몬스터캐릭터 목록
        CsGameData.Instance.MonsterCharacterList.Clear();

        for (int i = 0; i < gameData.monsterCharacters.Length; i++)
        {
            CsGameData.Instance.MonsterCharacterList.Add(new CsMonsterCharacter(gameData.monsterCharacters[i]));
        }

        //WPDMonster[] 몬스터 목록
        CsGameData.Instance.MonsterInfoList.Clear();

        for (int i = 0; i < gameData.monsters.Length; i++)
        {
            CsGameData.Instance.MonsterInfoList.Add(new CsMonsterInfo(gameData.monsters[i]));
        }

        //WPDContinent[] 대륙 목록
        CsGameData.Instance.ContinentList.Clear();

        for (int i = 0; i < gameData.continents.Length; i++)
        {
            CsGameData.Instance.ContinentList.Add(new CsContinent(gameData.continents[i]));
        }

        // WPDContinentObjectArrange[]	대륙오브젝트배치 목록
        for (int i = 0; i < gameData.continentObjectArranges.Length; i++)
        {
            CsContinent csContinent = CsGameData.Instance.GetContinent(gameData.continentObjectArranges[i].continentId);

            if (csContinent != null)
            {
                csContinent.ContinentObjectArrangeList.Add(new CsContinentObjectArrange(gameData.continentObjectArranges[i]));
            }
        }

        //WPDMainMenu[] 메인메뉴 목록
        CsGameData.Instance.MainMenuList.Clear();

        for (int i = 0; i < gameData.mainMenus.Length; i++)
        {
            CsGameData.Instance.MainMenuList.Add(new CsMainMenu(gameData.mainMenus[i]));
        }

        //WPDSubMenu[] 서브메뉴 목록
        for (int i = 0; i < gameData.subMenus.Length; i++)
        {
            CsMainMenu csMainMenu = CsGameData.Instance.GetMainMenu(gameData.subMenus[i].menuId);

            if (csMainMenu != null)
            {
                csMainMenu.SubMenuList.Add(new CsSubMenu(gameData.subMenus[i]));
            }
        }

        // 서브메뉴정렬.
        for (int i = 0; i < gameData.mainMenus.Length; i++)
        {
            CsGameData.Instance.MainMenuList[i].SortSubMenuList();
        }

        // WPDJobSkillMaster[] 직업 스킬 마스터 목록
        CsGameData.Instance.JobSkillMasterList.Clear();

        for (int i = 0; i < gameData.jobSkillMasters.Length; i++)
        {
            CsGameData.Instance.JobSkillMasterList.Add(new CsJobSkillMaster(gameData.jobSkillMasters[i]));
        }

        // WPDJobSkill[] 직업 스킬 목록
        CsGameData.Instance.JobSkillList.Clear();

        for (int i = 0; i < gameData.jobSkills.Length; i++)
        {
            CsGameData.Instance.JobSkillList.Add(new CsJobSkill(gameData.jobSkills[i]));
        }

        // WPDJobSkillLevel[] 직업 스킬 레벨 목록
        for (int i = 0; i < gameData.jobSkillLevels.Length; i++)
        {
            CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(gameData.jobSkillLevels[i].jobId, gameData.jobSkillLevels[i].skillId);

            if (csJobSkill != null)
            {
                csJobSkill.JobSkillLevelList.Add(new CsJobSkillLevel(gameData.jobSkillLevels[i]));
            }
        }

        // WPDJobSkillHit[] 직업 스킬 히트 목록
        for (int i = 0; i < gameData.jobSkillHits.Length; i++)
        {
            CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(gameData.jobSkillHits[i].jobId, gameData.jobSkillHits[i].skillId);

            if (csJobSkill != null)
            {
                csJobSkill.JobSkillHitList.Add(new CsJobSkillHit(gameData.jobSkillHits[i]));
            }
        }

        // WPDJobChainSkill[] 직업 연계스킬 목록
        for (int i = 0; i < gameData.jobChainSkills.Length; i++)
        {
            CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(gameData.jobChainSkills[i].jobId, gameData.jobChainSkills[i].skillId);

            if (csJobSkill != null)
            {
                csJobSkill.JobChainSkillList.Add(new CsJobChainSkill(gameData.jobChainSkills[i]));
            }
        }

        // WPDJobChainSkillHit[] 직업 연계스킬 히트 목록
        for (int i = 0; i < gameData.jobChainSkillHits.Length; i++)
        {
            CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(gameData.jobChainSkillHits[i].jobId, gameData.jobChainSkillHits[i].skillId);

            if (csJobSkill != null)
            {
                CsJobChainSkill csJobChainSkill = csJobSkill.GetJobChainSkill(gameData.jobChainSkillHits[i].chainSkillId);

                if (csJobChainSkill != null)
                {
                    csJobChainSkill.JobChainSkillHitList.Add(new CsJobChainSkillHit(gameData.jobChainSkillHits[i]));
                }
            }
        }

        // WPDJobSkillLevelMaster[]	직업 스킬 레벨 마스터 목록
        for (int i = 0; i < gameData.jobSkillLevelMasters.Length; i++)
        {
            CsJobSkillMaster csJobSkillMaster = CsGameData.Instance.GetJobSkillMaster(gameData.jobSkillLevelMasters[i].skillId);

            if (csJobSkillMaster != null)
            {
                csJobSkillMaster.JobSkillLevelMasterList.Add(new CsJobSkillLevelMaster(gameData.jobSkillLevelMasters[i]));
            }
        }

        // WPDPortal[] 포탈 목록
        CsGameData.Instance.PortalList.Clear();

        for (int i = 0; i < gameData.portals.Length; i++)
        {
            CsGameData.Instance.PortalList.Add(new CsPortal(gameData.portals[i]));
        }

        // WPDNpc[] NPC 목록
        CsGameData.Instance.NpcInfoList.Clear();

        for (int i = 0; i < gameData.npcs.Length; i++)
        {
            CsGameData.Instance.NpcInfoList.Add(new CsNpcInfo(gameData.npcs[i]));
        }

        // WPDContinentObject[] 대륙 오브젝트 목록
        CsGameData.Instance.ContinentObjectList.Clear();

        for (int i = 0; i < gameData.continentObjects.Length; i++)
        {
            CsGameData.Instance.ContinentObjectList.Add(new CsContinentObject(gameData.continentObjects[i]));
        }

        // WPDMainQuestDungeon[] 메인퀘스트 던전 목록
        CsGameData.Instance.MainQuestDungeonList.Clear();

        for (int i = 0; i < gameData.mainQuestDungeons.Length; i++)
        {
            CsGameData.Instance.MainQuestDungeonList.Add(new CsMainQuestDungeon(gameData.mainQuestDungeons[i]));
        }

        // WPDMainQuestDungeonObstacle[] 메인퀘스트 던전 장애물 목록
        for (int i = 0; i < gameData.mainQuestDungeonObstacles.Length; i++)
        {
            CsMainQuestDungeon csMainQuestDungeon = CsGameData.Instance.GetMainQuestDungeon(gameData.mainQuestDungeonObstacles[i].dungeonId);

            if (csMainQuestDungeon != null)
            {
                csMainQuestDungeon.MainQuestDungeonObstacleList.Add(new CsMainQuestDungeonObstacle(gameData.mainQuestDungeonObstacles[i]));
            }
        }
        // WPDMainQuestDungeonStep[] 메인퀘스트 던전 단계 목록
        for (int i = 0; i < gameData.mainQuestDungeonSteps.Length; i++)
        {
            CsMainQuestDungeon csMainQuestDungeon = CsGameData.Instance.GetMainQuestDungeon(gameData.mainQuestDungeonSteps[i].dungeonId);

            if (csMainQuestDungeon != null)
            {
                csMainQuestDungeon.MainQuestDungeonStepList.Add(new CsMainQuestDungeonStep(gameData.mainQuestDungeonSteps[i]));
            }
        }

        // WPDMainQuestDungeonGuide[] 메인퀘스트 던전 가이드 목록
        for (int i = 0; i < gameData.mainQuestDungeonGuides.Length; i++)
        {
            CsMainQuestDungeon csMainQuestDungeon = CsGameData.Instance.GetMainQuestDungeon(gameData.mainQuestDungeonGuides[i].dungeonId);

            if (csMainQuestDungeon != null)
            {
                CsMainQuestDungeonStep csMainQuestDungeonStep = csMainQuestDungeon.GetMainQuestDungeonStep(gameData.mainQuestDungeonGuides[i].step);

                if (csMainQuestDungeonStep != null)
                {
                    csMainQuestDungeonStep.MainQuestDungeonGuideList.Add(new CsMainQuestDungeonGuide(gameData.mainQuestDungeonGuides[i]));
                }
            }
        }

        // WPDMainQuest[] 메인퀘스트 목록
        CsGameData.Instance.MainQuestList.Clear();

        for (int i = 0; i < gameData.mainQuests.Length; i++)
        {
            CsGameData.Instance.MainQuestList.Add(new CsMainQuest(gameData.mainQuests[i]));
        }

        // WPDPaidImmediateRevival[]유료즉시부활 목록
        CsGameData.Instance.PaidImmediateRevivalList.Clear();

        for (int i = 0; i < gameData.paidImmediateRevivals.Length; i++)
        {
            CsGameData.Instance.PaidImmediateRevivalList.Add(new CsPaidImmediateRevival(gameData.paidImmediateRevivals[i]));
        }

        //WPDMonsterSkill[] 몬스터스킬 목록
        CsGameData.Instance.MonsterSkillList.Clear();

        for (int i = 0; i < gameData.monsterSkills.Length; i++)
        {
            CsGameData.Instance.MonsterSkillList.Add(new CsMonsterSkill(gameData.monsterSkills[i]));
        }

        //WPDMonsterSkillHit[] 몬스터스킬히트 목록
        for (int i = 0; i < gameData.monsterSkillHits.Length; i++)
        {
            CsMonsterSkill csMonsterSkill = CsGameData.Instance.GetMonsterSkill(gameData.monsterSkillHits[i].skillId);

            if (csMonsterSkill != null)
            {
                csMonsterSkill.MonsterSkillHitList.Add(new CsMonsterSkillHit(gameData.monsterSkillHits[i]));
            }
        }

        //WPDMonsterOwnSkill[] 몬스터보유스킬 목록
        for (int i = 0; i < gameData.monsterOwnSkills.Length; i++)
        {
            CsMonsterInfo csMonsterInfo = CsGameData.Instance.GetMonsterInfo(gameData.monsterOwnSkills[i].monsterId);

            if (csMonsterInfo != null)
            {
                csMonsterInfo.MonsterOwnSkillList.Add(new CsMonsterOwnSkill(gameData.monsterOwnSkills[i]));
            }
        }

        // WPDAbnormalState[] 상태이상 목록
        CsGameData.Instance.AbnormalStateList.Clear();

        for (int i = 0; i < gameData.abnormalStates.Length; i++)
        {
            CsGameData.Instance.AbnormalStateList.Add(new CsAbnormalState(gameData.abnormalStates[i]));
        }

        // WPDAbnormalStateLevel[] 상태이상레벨 목록
        for (int i = 0; i < gameData.abnormalStateLevels.Length; i++)
        {
            CsAbnormalState csAbnormalState = CsGameData.Instance.GetAbnormalState(gameData.abnormalStateLevels[i].abnormalStateId);

            if (csAbnormalState != null)
            {
                csAbnormalState.AbnormalStateLevelList.Add(new CsAbnormalStateLevel(gameData.abnormalStateLevels[i]));
            }
        }

		//WPDTradition[] 전승 목록
		CsGameData.Instance.TraditionList.Clear();


		// WPDJobLevelMaster[] 직업레벨마스터 목록
		CsGameData.Instance.JobLevelMasterList.Clear();

        for (int i = 0; i < gameData.jobLevelMasters.Length; i++)
        {
            CsGameData.Instance.JobLevelMasterList.Add(new CsJobLevelMaster(gameData.jobLevelMasters[i]));
        }

        CsGameData.Instance.JobLevelMasterList.Sort();

        // WPDJobLevel[] 직업레벨 목록
        for (int i = 0; i < gameData.jobLevels.Length; i++)
        {
            CsJob csJob = CsGameData.Instance.GetJob(gameData.jobLevels[i].jobId);

            if (csJob != null)
            {
                csJob.JobLevelList.Add(new CsJobLevel(gameData.jobLevels[i]));
            }
        }

        //WPDMonsterArrange[] 몬스터배치 목록
        CsGameData.Instance.MonsterArrangeDictionary.Clear();

        for (int i = 0; i < gameData.monsterArranges.Length; i++)
        {
            CsGameData.Instance.MonsterArrangeDictionary.Add(gameData.monsterArranges[i].monsterArrangeId, new CsMonsterArrange(gameData.monsterArranges[i]));
        }

        //WPDSimpleShopProduct[] 간편상점 상품 목록
        CsGameData.Instance.SimpleShopProductList.Clear();

        for (int i = 0; i < gameData.simpleShopProducts.Length; i++)
        {
            CsGameData.Instance.SimpleShopProductList.Add(new CsSimpleShopProduct(gameData.simpleShopProducts[i]));
        }

        CsGameData.Instance.SimpleShopProductList.Sort();

        //WPDVipLevel[] VIP레벨 목록
        CsGameData.Instance.VipLevelList.Clear();

        for (int i = 0; i < gameData.vipLevels.Length; i++)
        {
            CsGameData.Instance.VipLevelList.Add(new CsVipLevel(gameData.vipLevels[i]));
        }

        //WPDInventorySlotExtendRecipe[] 인벤토리 확장 레시피 목록
        CsGameData.Instance.InventorySlotExtendRecipeList.Clear();

        for (int i = 0; i < gameData.inventorySlotExtendRecipes.Length; i++)
        {
            CsGameData.Instance.InventorySlotExtendRecipeList.Add(new CsInventorySlotExtendRecipe(gameData.inventorySlotExtendRecipes[i]));
        }

        CsGameData.Instance.InventorySlotExtendRecipeList.Sort();

        //WPDMainGearDisassembleAvailableResultEntry[] 메인장비 분해 획득가능 결과 항목 목록
        CsGameData.Instance.MainGearDisassembleAvailableResultEntryList.Clear();

        for (int i = 0; i < gameData.mainGearDisassembleAvailableResultEntries.Length; i++)
        {
            CsGameData.Instance.MainGearDisassembleAvailableResultEntryList.Add(new CsMainGearDisassembleAvailableResultEntry(gameData.mainGearDisassembleAvailableResultEntries[i]));
        }

        //WPDItemCompositionRecipe[] 아이템합성레시피 목록
        CsGameData.Instance.ItemCompositionRecipeList.Clear();

        for (int i = 0; i < gameData.itemCompositionRecipes.Length; i++)
        {
            CsGameData.Instance.ItemCompositionRecipeList.Add(new CsItemCompositionRecipe(gameData.itemCompositionRecipes[i]));
        }

        // WPDRestRewardTime[]	휴식보상시간 목록
        CsGameData.Instance.RestRewardTimeList.Clear();

        for (int i = 0; i < gameData.restRewardTimes.Length; i++)
        {
            CsGameData.Instance.RestRewardTimeList.Add(new CsRestRewardTime(gameData.restRewardTimes[i]));
        }

        // WPDChattingType[] 채팅타입 목록
        CsGameData.Instance.ChattingTypeList.Clear();

        for (int i = 0; i < gameData.chattingTypes.Length; i++)
        {
            CsGameData.Instance.ChattingTypeList.Add(new CsChattingType(gameData.chattingTypes[i]));
        }

        // WPDLevelUpRewardEntry[] 레벨업 보상 항목 목록
        CsGameData.Instance.LevelUpRewardEntryList.Clear();

        for (int i = 0; i < gameData.levelUpRewardEntries.Length; i++)
        {
            CsGameData.Instance.LevelUpRewardEntryList.Add(new CsLevelUpRewardEntry(gameData.levelUpRewardEntries[i]));
        }

        // WPDLevelUpRewardItem[] 레벨업 보상 아이템 목록
        for (int i = 0; i < gameData.levelUpRewardItems.Length; i++)
        {
            CsLevelUpRewardEntry csLevelUpRewardEntry = CsGameData.Instance.GetLevelUpRewardEntry(gameData.levelUpRewardItems[i].entryId);

            if (csLevelUpRewardEntry != null)
            {
                csLevelUpRewardEntry.LevelUpRewardItemList.Add(new CsLevelUpRewardItem(gameData.levelUpRewardItems[i]));
            }
        }

        // WPDDailyAttendRewardEntry[] 일일출석 보상 항목 목록
        CsGameData.Instance.DailyAttendRewardEntryList.Clear();

        for (int i = 0; i < gameData.dailyAttendRewardEntries.Length; i++)
        {
            CsGameData.Instance.DailyAttendRewardEntryList.Add(new CsDailyAttendRewardEntry(gameData.dailyAttendRewardEntries[i]));
        }

        // WPDWeekendAttendRewardAvailableDayOfWeek[] 주말출석 보상 가능 요일 목록
        CsGameData.Instance.WeekendAttendRewardAvailableDayOfWeekList.Clear();

        for (int i = 0; i < gameData.weekendAttendRewardAvailableDayOfWeeks.Length; i++)
        {
            CsGameData.Instance.WeekendAttendRewardAvailableDayOfWeekList.Add(new CsWeekendAttendRewardAvailableDayOfWeek(gameData.weekendAttendRewardAvailableDayOfWeeks[i]));
        }

        // WPDAccessRewardEntry[] 접속 보상 항목 목록
        CsGameData.Instance.AccessRewardEntryList.Clear();

        for (int i = 0; i < gameData.accessRewardEntries.Length; i++)
        {
            CsGameData.Instance.AccessRewardEntryList.Add(new CsAccessRewardEntry(gameData.accessRewardEntries[i]));
        }

        // WPDAccessRewardItem[] 접속 보상 아이템 목록
        for (int i = 0; i < gameData.accessRewardItems.Length; i++)
        {
            CsAccessRewardEntry csAccessRewardEntry = CsGameData.Instance.GetAccessRewardEntry(gameData.accessRewardItems[i].entryId);

            if (csAccessRewardEntry != null)
            {
                csAccessRewardEntry.AccessRewardItemList.Add(new CsAccessRewardItem(gameData.accessRewardItems[i]));
            }
        }

        // WPDVipLevelReward[] VIP레벨 보상 목록
        for (int i = 0; i < gameData.vipLevelRewards.Length; i++)
        {
            CsVipLevel csVipLevel = CsGameData.Instance.GetVipLevel(gameData.vipLevelRewards[i].vipLevel);

            if (csVipLevel != null)
            {
                csVipLevel.VipLevelRewardList.Add(new CsVipLevelReward(gameData.vipLevelRewards[i]));
            }
        }

        // WPDStoryDungeon[] 스토리던전 목록
        CsGameData.Instance.StoryDungeonList.Clear();

        for (int i = 0; i < gameData.storyDungeons.Length; i++)
        {
            CsGameData.Instance.StoryDungeonList.Add(new CsStoryDungeon(gameData.storyDungeons[i]));
        }

        // WPDStoryDungeonDifficulty[] 스토리던전 난이도 목록
        for (int i = 0; i < gameData.storyDungeonDifficulties.Length; i++)
        {
            CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonDifficulties[i].dungeonNo);

            if (csStoryDungeon != null)
            {
                csStoryDungeon.StoryDungeonDifficultyList.Add(new CsStoryDungeonDifficulty(gameData.storyDungeonDifficulties[i]));
            }
        }

        // WPDStoryDungeonObstacle[] 스토리던전 장애물 목록
        for (int i = 0; i < gameData.storyDungeonObstacles.Length; i++)
        {
            CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonObstacles[i].dungeonNo);

            if (csStoryDungeon != null)
            {
                csStoryDungeon.StoryDungeonObstacleList.Add(new CsStoryDungeonObstacle(gameData.storyDungeonObstacles[i]));
            }
        }

        // WPDStoryDungeonAvailableReward[] 스토리던전 획득가능 보상 목록
        for (int i = 0; i < gameData.storyDungeonAvailableRewards.Length; i++)
        {
            CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonAvailableRewards[i].dungeonNo);

            if (csStoryDungeon != null)
            {
                CsStoryDungeonDifficulty csStoryDungeonDifficulty = csStoryDungeon.GetStoryDungeonDifficulty(gameData.storyDungeonAvailableRewards[i].difficulty);

                if (csStoryDungeonDifficulty != null)
                {
                    csStoryDungeonDifficulty.StoryDungeonAvailableRewardList.Add(new CsStoryDungeonAvailableReward(gameData.storyDungeonAvailableRewards[i]));
                }
            }
        }

        // WPDStoryDungeonStep[] 스토리던전 단계 목록
        for (int i = 0; i < gameData.storyDungeonSteps.Length; i++)
        {
            CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonSteps[i].dungeonNo);

            if (csStoryDungeon != null)
            {
                CsStoryDungeonDifficulty csStoryDungeonDifficulty = csStoryDungeon.GetStoryDungeonDifficulty(gameData.storyDungeonSteps[i].difficulty);

                if (csStoryDungeonDifficulty != null)
                {
                    csStoryDungeonDifficulty.StoryDungeonStepList.Add(new CsStoryDungeonStep(gameData.storyDungeonSteps[i]));
                }
            }
        }

        // WPDStoryDungeonGuide[] 스토리던전 가이드 목록
        for (int i = 0; i < gameData.storyDungeonGuides.Length; i++)
        {
            CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonGuides[i].dungeonNo);

            if (csStoryDungeon != null)
            {
                CsStoryDungeonDifficulty csStoryDungeonDifficulty = csStoryDungeon.GetStoryDungeonDifficulty(gameData.storyDungeonGuides[i].difficulty);

                if (csStoryDungeonDifficulty != null)
                {
                    CsStoryDungeonStep csStoryDungeonStep = csStoryDungeonDifficulty.GetStoryDungeonStep(gameData.storyDungeonGuides[i].step);

                    if (csStoryDungeonStep != null)
                    {
                        csStoryDungeonStep.StoryDungeonGuideList.Add(new CsStoryDungeonGuide(gameData.storyDungeonGuides[i]));
                    }
                }
            }
        }

		// WPDStoryDungeonTrap[] 스토리던전함정 목록
		for (int i = 0; i < gameData.storyDungeonTraps.Length; i++)
		{
			CsStoryDungeon csStoryDungeon = CsGameData.Instance.GetStoryDungeon(gameData.storyDungeonTraps[i].dungeonNo);

			if (csStoryDungeon != null)
			{
				CsStoryDungeonDifficulty csStoryDungeonDifficulty = csStoryDungeon.GetStoryDungeonDifficulty(gameData.storyDungeonTraps[i].difficulty);

				if (csStoryDungeonDifficulty != null)
				{
					csStoryDungeonDifficulty.StoryDungeonTrapList.Add(new CsStoryDungeonTrap(gameData.storyDungeonTraps[i]));
				}
			}
		}

		// WPDWingStep[] 날개단계 목록
		CsGameData.Instance.WingStepList.Clear();

        for (int i = 0; i < gameData.wingSteps.Length; i++)
        {
            CsGameData.Instance.WingStepList.Add(new CsWingStep(gameData.wingSteps[i]));
        }

        // WPDWingPart[] 날개 파트 목록
        CsGameData.Instance.WingPartList.Clear();

        for (int i = 0; i < gameData.wingParts.Length; i++)
        {
            CsGameData.Instance.WingPartList.Add(new CsWingPart(gameData.wingParts[i]));
        }

        // WPDWing[] 날개 목록
        CsGameData.Instance.WingList.Clear();

        for (int i = 0; i < gameData.wings.Length; i++)
        {
            CsGameData.Instance.WingList.Add(new CsWing(gameData.wings[i]));
        }

        // WPDWingStepPart[] 날개단계 파트 목록
        for (int i = 0; i < gameData.wingStepParts.Length; i++)
        {
            CsWingStep csWingStep = CsGameData.Instance.GetWingStep(gameData.wingStepParts[i].step);

            if (csWingStep != null)
            {
                csWingStep.WingStepPartList.Add(new CsWingStepPart(gameData.wingStepParts[i]));
            }
        }

        // WPDWingStepLevel[] 날개단계 레벨 목록
        for (int i = 0; i < gameData.wingStepLevels.Length; i++)
        {
            CsWingStep csWingStep = CsGameData.Instance.GetWingStep(gameData.wingStepLevels[i].step);

            if (csWingStep != null)
            {
                csWingStep.WingStepLevelList.Add(new CsWingStepLevel(gameData.wingStepLevels[i]));
            }
        }

        // WPDStaminaBuyCount[]  체력구입횟수 목록
        CsGameData.Instance.StaminaBuyCountList.Clear();

        for (int i = 0; i < gameData.staminaBuyCounts.Length; i++)
        {
            CsGameData.Instance.StaminaBuyCountList.Add(new CsStaminaBuyCount(gameData.staminaBuyCounts[i]));
        }

        // WPDExpDungeon  경험치던전

        CsGameData.Instance.ExpDungeon = new CsExpDungeon(gameData.expDungeon);

        // WPDExpDungeonDifficulty[] 경험치던전 난이도 목록
        for (int i = 0; i < gameData.expDungeonDifficulties.Length; i++)
        {
            CsGameData.Instance.ExpDungeon.ExpDungeonDifficultyList.Add(new CsExpDungeonDifficulty(gameData.expDungeonDifficulties[i]));
        }

        // WPDExpDungeonDifficultyWave[] 경험치던전 난이도 웨이브 목록
        for (int i = 0; i < gameData.expDungeonDifficultyWaves.Length; i++)
        {
            CsExpDungeonDifficulty csExpDungeonDifficulty = CsGameData.Instance.ExpDungeon.GetExpDungeonDifficulty(gameData.expDungeonDifficultyWaves[i].difficulty);

            if (csExpDungeonDifficulty != null)
            {
                csExpDungeonDifficulty.ExpDungeonDifficultyWaveList.Add(new CsExpDungeonDifficultyWave(gameData.expDungeonDifficultyWaves[i]));
            }
        }

        //WPDGoldDungeon 골드던전
        CsGameData.Instance.GoldDungeon = new CsGoldDungeon(gameData.goldDungeon);

        //WPDGoldDungeonDifficulty[]  골드던전 난이도 목록
        for (int i = 0; i < gameData.goldDungeonDifficulties.Length; i++)
        {
            CsGameData.Instance.GoldDungeon.GoldDungeonDifficultyList.Add(new CsGoldDungeonDifficulty(gameData.goldDungeonDifficulties[i]));
        }

        //WPDGoldDungeonStep[] 골드던전 단계 목록
        for (int i = 0; i < gameData.goldDungeonSteps.Length; i++)
        {
            CsGoldDungeonDifficulty csGoldDungeonDifficulty = CsGameData.Instance.GoldDungeon.GetGoldDungeonDifficulty(gameData.goldDungeonSteps[i].difficulty);

            if (csGoldDungeonDifficulty != null)
            {
                csGoldDungeonDifficulty.GoldDungeonStepList.Add(new CsGoldDungeonStep(gameData.goldDungeonSteps[i]));
            }
        }

        //WPDGoldDungeonStepWave[] 골드던전 단계 웨이브 목록
        for (int i = 0; i < gameData.goldDungeonStepWaves.Length; i++)
        {
            CsGoldDungeonDifficulty csGoldDungeonDifficulty = CsGameData.Instance.GoldDungeon.GetGoldDungeonDifficulty(gameData.goldDungeonStepWaves[i].difficulty);

            if (csGoldDungeonDifficulty != null)
            {
                CsGoldDungeonStep csGoldDungeonStep = csGoldDungeonDifficulty.GetGoldDungeonStep(gameData.goldDungeonStepWaves[i].step);

                if (csGoldDungeonStep != null)
                {
                    csGoldDungeonStep.GoldDungeonStepWaveList.Add(new CsGoldDungeonStepWave(gameData.goldDungeonStepWaves[i]));
                }
            }
        }

        //WPDGoldDungeonStepMonsterArrange[] 골드던전 단계 몬스터 배치 목록 (도망자만)
        for (int i = 0; i < gameData.goldDungeonStepMonsterArranges.Length; i++)
        {
            CsGoldDungeonDifficulty csGoldDungeonDifficulty = CsGameData.Instance.GoldDungeon.GetGoldDungeonDifficulty(gameData.goldDungeonStepMonsterArranges[i].difficulty);

            if (csGoldDungeonDifficulty != null)
            {
                CsGoldDungeonStep csGoldDungeonStep = csGoldDungeonDifficulty.GetGoldDungeonStep(gameData.goldDungeonStepMonsterArranges[i].step);

                if (csGoldDungeonStep != null)
                {
                    csGoldDungeonStep.GoldDungeonStepMonsterArrangeList.Add(new CsGoldDungeonStepMonsterArrange(gameData.goldDungeonStepMonsterArranges[i]));
                }
            }
        }

        //WPDMainGearEnchantLevelSet[] 메인장비 강화레벨 세트 목록
        CsGameData.Instance.MainGearEnchantLevelSetList.Clear();

        for (int i = 0; i < gameData.mainGearEnchantLevelSets.Length; i++)
        {
            CsGameData.Instance.MainGearEnchantLevelSetList.Add(new CsMainGearEnchantLevelSet(gameData.mainGearEnchantLevelSets[i]));
        }

        //WPDMainGearEnchantLevelSetAttr[] 메인장비 강화레벨 세트 속성 목록
        for (int i = 0; i < gameData.mainGearEnchantLevelSetAttrs.Length; i++)
        {
            CsMainGearEnchantLevelSet csMainGearEnchantLevelSet = CsGameData.Instance.GetMainGearEnchantLevelSet(gameData.mainGearEnchantLevelSetAttrs[i].setNo);

            if (csMainGearEnchantLevelSet != null)
            {
                csMainGearEnchantLevelSet.MainGearEnchantLevelSetAttrList.Add(new CsMainGearEnchantLevelSetAttr(gameData.mainGearEnchantLevelSetAttrs[i]));
            }
        }

        //WPDMainGearSet[] 메인장비 세트 목록
        CsGameData.Instance.MainGearSetList.Clear();

        for (int i = 0; i < gameData.mainGearSets.Length; i++)
        {
            CsGameData.Instance.MainGearSetList.Add(new CsMainGearSet(gameData.mainGearSets[i]));
        }

        //WPDMainGearSetAttr[] 메인장비 세트 속성 목록
        for (int i = 0; i < gameData.mainGearSetAttrs.Length; i++)
        {
            CsMainGearSet csMainGearSet = CsGameData.Instance.GetMainGearSet(gameData.mainGearSetAttrs[i].tier, gameData.mainGearSetAttrs[i].grade, gameData.mainGearSetAttrs[i].quality);

            if (csMainGearSet != null)
            {
                csMainGearSet.MainGearSetAttrList.Add(new CsMainGearSetAttr(gameData.mainGearSetAttrs[i]));
            }
        }

        //WPDSubGearSoulstoneLevelSet[] 보조장비 소울스톤레벨 세트 목록
        CsGameData.Instance.SubGearSoulstoneLevelSetList.Clear();

        for (int i = 0; i < gameData.subGearSoulstoneLevelSets.Length; i++)
        {
            CsGameData.Instance.SubGearSoulstoneLevelSetList.Add(new CsSubGearSoulstoneLevelSet(gameData.subGearSoulstoneLevelSets[i]));
        }

        //WPDSubGearSoulstoneLevelSetAttr[] 보조장비 소울스톤레벨 세트 속성 목록
        for (int i = 0; i < gameData.subGearSoulstoneLevelSetAttrs.Length; i++)
        {
            CsSubGearSoulstoneLevelSet csSubGearSoulstoneLevelSet = CsGameData.Instance.GetSubGearSoulstoneLevelSet(gameData.subGearSoulstoneLevelSetAttrs[i].setNo);

            if (csSubGearSoulstoneLevelSet != null)
            {
                csSubGearSoulstoneLevelSet.SubGearSoulstoneLevelSetAttrList.Add(new CsSubGearSoulstoneLevelSetAttr(gameData.subGearSoulstoneLevelSetAttrs[i]));
            }
        }

        //WPDCartGrade[]  수레등급 목록
        CsGameData.Instance.CartGradeList.Clear();

        for (int i = 0; i < gameData.cartGrades.Length; i++)
        {
            CsGameData.Instance.CartGradeList.Add(new CsCartGrade(gameData.cartGrades[i]));
        }

        //WPDCart[]  수레 목록
        CsGameData.Instance.CartList.Clear();

        for (int i = 0; i < gameData.carts.Length; i++)
        {
            CsGameData.Instance.CartList.Add(new CsCart(gameData.carts[i]));
        }

        //WPDWorldLevelExpFactor[] 세계레벨경험치계수 목록
        CsGameData.Instance.WorldLevelExpFactorList.Clear();

        for (int i = 0; i < gameData.worldLevelExpFactors.Length; i++)
        {
            CsGameData.Instance.WorldLevelExpFactorList.Add(new CsWorldLevelExpFactor(gameData.worldLevelExpFactors[i]));
        }

        //WPDLocationArea[] 위치지역 목록
        for (int i = 0; i < gameData.locationAreas.Length; i++)
        {
            CsLocation csLocation = CsGameData.Instance.GetLocation(gameData.locationAreas[i].locationId);

            if (csLocation != null)
            {
                csLocation.LocationAreaList.Add(new CsLocationArea(gameData.locationAreas[i]));
            }
        }

        //WPDContinentMapMonster[] 대륙맵몬스터 목록
        for (int i = 0; i < gameData.continentMapMonsters.Length; i++)
        {
            CsContinent csContinent = CsGameData.Instance.GetContinent(gameData.continentMapMonsters[i].continentId);

            if (csContinent != null)
            {
                csContinent.ContinentMapMonsterList.Add(new CsContinentMapMonster(gameData.continentMapMonsters[i]));
            }
        }

        //WPDTreatOfFarmQuest 농장의위협퀘스트
        CsGameData.Instance.ThreatOfFarmQuest = new CsThreatOfFarmQuest(gameData.treatOfFarmQuest);

        //WPDTreatOfFarmQuestMission[]  농장의위협퀘스트미션 목록
        for (int i = 0; i < gameData.treatOfFarmQuestMissions.Length; i++)
        {
            CsGameData.Instance.ThreatOfFarmQuest.ThreatOfFarmQuestMissionList.Add(new CsThreatOfFarmQuestMission(gameData.treatOfFarmQuestMissions[i]));
        }

        //WPDTreatOfFarmQuestReward[] 농장의위협퀘스트보상 목록
        for (int i = 0; i < gameData.treatOfFarmQuestRewards.Length; i++)
        {
            CsGameData.Instance.ThreatOfFarmQuest.ThreatOfFarmQuestRewardList.Add(new CsThreatOfFarmQuestReward(gameData.treatOfFarmQuestRewards[i]));
        }

        //WPDContinentTransmissionExit[] 대륙전송출구 목록
        for (int i = 0; i < gameData.continentTransmissionExits.Length; i++)
        {
            CsNpcInfo csNpcInfo = CsGameData.Instance.GetNpcInfo(gameData.continentTransmissionExits[i].npcId);

            if (csNpcInfo != null)
            {
                csNpcInfo.ContinentTransmissionExitList.Add(new CsContinentTransmissionExit(gameData.continentTransmissionExits[i]));
            }
        }

        //WPDUndergroundMaze 지하미로
        CsGameData.Instance.UndergroundMaze = new CsUndergroundMaze(gameData.undergroundMaze);

        //WPDUndergroundMazeEntrance[] 지하미로입구 목록
        for (int i = 0; i < gameData.undergroundMazeEntrances.Length; i++)
        {
            CsGameData.Instance.UndergroundMaze.UndergroundMazeEntranceList.Add(new CsUndergroundMazeEntrance(gameData.undergroundMazeEntrances[i]));
        }

        //WPDUndergroundMazeFloor[] 지하미로층 목록
        for (int i = 0; i < gameData.undergroundMazeFloors.Length; i++)
        {
            CsGameData.Instance.UndergroundMaze.UndergroundMazeFloorList.Add(new CsUndergroundMazeFloor(gameData.undergroundMazeFloors[i]));
        }

        //WPDUndergroundMazePortal[] 지하미로포탈 목록
        for (int i = 0; i < gameData.undergroundMazePortals.Length; i++)
        {
            CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.GetUndergroundMazeFloor(gameData.undergroundMazePortals[i].floor);

            if (csUndergroundMazeFloor != null)
            {
                csUndergroundMazeFloor.UndergroundMazePortalList.Add(new CsUndergroundMazePortal(gameData.undergroundMazePortals[i]));
            }
        }

        //WPDUndergroundMazeMapMonster[] 지하미로맵몬스터 목록
        for (int i = 0; i < gameData.undergroundMazeMapMonsters.Length; i++)
        {
            CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.GetUndergroundMazeFloor(gameData.undergroundMazeMapMonsters[i].floor);

            if (csUndergroundMazeFloor != null)
            {
                csUndergroundMazeFloor.UndergroundMazeMapMonsterList.Add(new CsUndergroundMazeMapMonster(gameData.undergroundMazeMapMonsters[i]));
            }
        }

        //WPDUndergroundMazeNpc[] 지하미로NPC 목록
        for (int i = 0; i < gameData.undergroundMazeNpcs.Length; i++)
        {
            CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.GetUndergroundMazeFloor(gameData.undergroundMazeNpcs[i].floor);

            if (csUndergroundMazeFloor != null)
            {
                csUndergroundMazeFloor.UndergroundMazeNpcList.Add(new CsUndergroundMazeNpc(gameData.undergroundMazeNpcs[i]));
            }
        }

        //WPDUndergroundMazeNpcTransmissionEntry[] 지하미로NPC전송항목 목록
        for (int i = 0; i < CsGameData.Instance.UndergroundMaze.UndergroundMazeFloorList.Count; i++)
        {
            for (int j = 0; j < gameData.undergroundMazeNpcTransmissionEntries.Length; j++)
            {
                CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.UndergroundMazeFloorList[i];

                if (csUndergroundMazeFloor != null)
                {
                    CsUndergroundMazeNpc csUndergroundMazeNpc = csUndergroundMazeFloor.GetUndergroundMazeNpc(gameData.undergroundMazeNpcTransmissionEntries[j].npcId);

                    if (csUndergroundMazeNpc != null)
                    {
                        csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList.Add(new CsUndergroundMazeNpcTransmissionEntry(gameData.undergroundMazeNpcTransmissionEntries[j]));
                    }
                }
            }
        }

        /*
		//WPDUndergroundMazeNpcTransmissionEntry[] 지하미로NPC전송항목 목록
		for (int i = 0; i < gameData.undergroundMazeNpcTransmissionEntries.Length; i++)
		{
			CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.GetUndergroundMazeFloor(gameData.undergroundMazeNpcTransmissionEntries[i].floor);

            Debug.Log(gameData.undergroundMazeNpcTransmissionEntries[i].floor);

			if (csUndergroundMazeFloor != null)
			{
				CsUndergroundMazeNpc csUndergroundMazeNpc = csUndergroundMazeFloor.GetUndergroundMazeNpc(gameData.undergroundMazeNpcTransmissionEntries[i].npcId);

				if (csUndergroundMazeNpc != null)
				{
                    Debug.Log(csUndergroundMazeNpc.Name + "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
					csUndergroundMazeNpc.UndergroundMazeNpcTransmissionEntryList.Add(new CsUndergroundMazeNpcTransmissionEntry(gameData.undergroundMazeNpcTransmissionEntries[i]));
				}
			}
		}
        */

        //WPDUndergroundMazeMonsterArrange[] 지하미로몬스터배치 목록
        for (int i = 0; i < gameData.undergroundMazeMonsterArranges.Length; i++)
        {
            CsUndergroundMazeFloor csUndergroundMazeFloor = CsGameData.Instance.UndergroundMaze.GetUndergroundMazeFloor(gameData.undergroundMazeMonsterArranges[i].floor);

            if (csUndergroundMazeFloor != null)
            {
                csUndergroundMazeFloor.UndergroundMazeMonsterArrangeList.Add(new CsUndergroundMazeMonsterArrange(gameData.undergroundMazeMonsterArranges[i]));
            }
        }

        //WPDBountyHunterQuest[] 현상금사냥퀘스트 목록
        CsGameData.Instance.BountyHunterQuestList.Clear();

        for (int i = 0; i < gameData.bountyHunterQuests.Length; i++)
        {
            CsGameData.Instance.BountyHunterQuestList.Add(new CsBountyHunterQuest(gameData.bountyHunterQuests[i]));
        }

        //WPDBountyHunterQuestReward[] 현상금사냥퀘스트보상 목록
        CsGameData.Instance.BountyHunterQuestRewardList.Clear();

        for (int i = 0; i < gameData.bountyHunterQuestRewards.Length; i++)
        {
            CsGameData.Instance.BountyHunterQuestRewardList.Add(new CsBountyHunterQuestReward(gameData.bountyHunterQuestRewards[i]));
        }

        //WPDFishingQuest 낚시퀘스트
        CsGameData.Instance.FishingQuest = new CsFishingQuest(gameData.fishingQuest);

        //WPDFishingQuestSpot[] 낚시퀘스트지점 목록
        for (int i = 0; i < gameData.fishingQuestSpots.Length; i++)
        {
            CsGameData.Instance.FishingQuest.FishingQuestSpotList.Add(new CsFishingQuestSpot(gameData.fishingQuestSpots[i]));
        }

		// WPDFishingQuestGuildTerritorySpot[]	낚시퀘스트길드영지지점 목록
		for (int i = 0; i < gameData.fishingQuestGuildTerritorySpots.Length; i++)
		{
			CsGameData.Instance.FishingQuest.FishingQuestGuildTerritorySpotList.Add(new CsFishingQuestGuildTerritorySpot(gameData.fishingQuestGuildTerritorySpots[i]));
		}

		//WPDArtifactRoom 고대유물의방
		CsGameData.Instance.ArtifactRoom = new CsArtifactRoom(gameData.artifactRoom);

        //WPDArtifactRoomFloor[] 고대유물의방층 목록
        for (int i = 0; i < gameData.artifactRoomFloors.Length; i++)
        {
            CsGameData.Instance.ArtifactRoom.ArtifactRoomFloorList.Add(new CsArtifactRoomFloor(gameData.artifactRoomFloors[i]));
        }

        //WPDExploitPointReward[] 공적점수보상 목록
        CsGameData.Instance.ExploitPointRewardDictionary.Clear();

        for (int i = 0; i < gameData.exploitPointRewards.Length; i++)
        {
            CsGameData.Instance.ExploitPointRewardDictionary.Add(gameData.exploitPointRewards[i].exploitPointRewardId, new CsExploitPointReward(gameData.exploitPointRewards[i]));
        }

        // WPDMysteryBoxGrade[]	의문의상자등급 목록
        CsGameData.Instance.MysteryBoxGradeList.Clear();

        for (int i = 0; i < gameData.mysteryBoxGrades.Length; i++)
        {
            CsGameData.Instance.MysteryBoxGradeList.Add(new CsMysteryBoxGrade(gameData.mysteryBoxGrades[i]));
        }

        // WPDMysteryBoxQuest 의문의상자퀘스트
        CsGameData.Instance.MysteryBoxQuest = new CsMysteryBoxQuest(gameData.mysteryBoxQuest);

        // WPDMysteryBoxQuestReward[] 의문의상자퀘스트보상 목록
        for (int i = 0; i < gameData.mysteryBoxQuestRewards.Length; i++)
        {
            CsGameData.Instance.MysteryBoxQuest.MysteryBoxQuestRewardList.Add(new CsMysteryBoxQuestReward(gameData.mysteryBoxQuestRewards[i]));
        }

        // WPDSecretLetterGrade[] 밀서등급 목록
        CsGameData.Instance.SecretLetterGradeList.Clear();

        for (int i = 0; i < gameData.secretLetterGrades.Length; i++)
        {
            CsGameData.Instance.SecretLetterGradeList.Add(new CsSecretLetterGrade(gameData.secretLetterGrades[i]));
        }

        // WPDSecretLetterQuest 밀서유출퀘스트
        CsGameData.Instance.SecretLetterQuest = new CsSecretLetterQuest(gameData.secretLetterQuest);

        // WPDSecretLetterQuestReward[] 밀서유출퀘스트보상 목록
        for (int i = 0; i < gameData.secretLetterQuestRewards.Length; i++)
        {
            CsGameData.Instance.SecretLetterQuest.SecretLetterQuestRewardList.Add(new CsSecretLetterQuestReward(gameData.secretLetterQuestRewards[i]));
        }

        //WPDTodayMission[] 오늘의미션 목록
        CsGameData.Instance.TodayMissionList.Clear();

        for (int i = 0; i < gameData.todayMissions.Length; i++)
        {
            CsGameData.Instance.TodayMissionList.Add(new CsTodayMission(gameData.todayMissions[i]));
        }

        //WPDTodayMissionReward[] 오늘의미션보상 목록
        for (int i = 0; i < gameData.todayMissionRewards.Length; i++)
        {
            CsTodayMission csTodayMission = CsGameData.Instance.GetTodayMission(gameData.todayMissionRewards[i].missionId);

            if (csTodayMission != null)
            {
                csTodayMission.TodayMissionRewardList.Add(new CsTodayMissionReward(gameData.todayMissionRewards[i]));
            }
        }

        //WPDSeriesMission[] 연속미션 목록
        CsGameData.Instance.SeriesMissionList.Clear();

        for (int i = 0; i < gameData.seriesMissions.Length; i++)
        {
            CsGameData.Instance.SeriesMissionList.Add(new CsSeriesMission(gameData.seriesMissions[i]));
        }

        CsGameData.Instance.SeriesMissionList.Sort();

        //WPDSeriesMissionStep[] 연속미션단계 목록
        for (int i = 0; i < gameData.seriesMissionSteps.Length; i++)
        {
            CsSeriesMission csSeriesMission = CsGameData.Instance.GetSeriesMission(gameData.seriesMissionSteps[i].missionId);

            if (csSeriesMission != null)
            {
                csSeriesMission.CsSeriesMissionStepList.Add(new CsSeriesMissionStep(gameData.seriesMissionSteps[i]));
            }
        }

        //WPDSeriesMissionStepReward[] 연속미션단계보상 목록
        for (int i = 0; i < gameData.seriesMissionStepRewards.Length; i++)
        {
            CsSeriesMission csSeriesMission = CsGameData.Instance.GetSeriesMission(gameData.seriesMissionStepRewards[i].missionId);

            if (csSeriesMission != null)
            {
                CsSeriesMissionStep csSeriesMissionStep = csSeriesMission.GetSeriesMissionStep(gameData.seriesMissionStepRewards[i].step);

                if (csSeriesMissionStep != null)
                {
                    csSeriesMissionStep.SeriesMissionStepRewardList.Add(new CsSeriesMissionStepReward(gameData.seriesMissionStepRewards[i]));
                }
            }
        }

        //WPDAncientRelic 고대인의유적
        CsGameData.Instance.AncientRelic = new CsAncientRelic(gameData.ancientRelic);

        //WPDAncientRelicObstacle[] 고대인의유적장애물 목록
        for (int i = 0; i < gameData.ancientRelicObstacles.Length; i++)
        {
            CsGameData.Instance.AncientRelic.AncientRelicObstacleList.Add(new CsAncientRelicObstacle(gameData.ancientRelicObstacles[i]));
        }

        //WPDAncientRelicAvailableReward[]    Y 고대인의유적획득가능보상 목록
        for (int i = 0; i < gameData.ancientRelicAvailableRewards.Length; i++)
        {
            CsGameData.Instance.AncientRelic.AncientRelicAvailableRewardList.Add(new CsAncientRelicAvailableReward(gameData.ancientRelicAvailableRewards[i]));
        }

        //WPDAncientRelicTrap[]  고대인의유적함정 목록
        for (int i = 0; i < gameData.ancientRelicTraps.Length; i++)
        {
            CsGameData.Instance.AncientRelic.AncientRelicTrapList.Add(new CsAncientRelicTrap(gameData.ancientRelicTraps[i]));
        }

        //WPDAncientRelicStep[]  고대인의유적단계 목록
        for (int i = 0; i < gameData.ancientRelicSteps.Length; i++)
        {
            CsGameData.Instance.AncientRelic.AncientRelicStepList.Add(new CsAncientRelicStep(gameData.ancientRelicSteps[i]));
        }

        //WPDAncientRelicStepWave[]   고대인의유적단계웨이브 목록
        for (int i = 0; i < gameData.ancientRelicStepWaves.Length; i++)
        {
            CsAncientRelicStep csAncientRelicStep = CsGameData.Instance.AncientRelic.GetAncientRelicStep(gameData.ancientRelicStepWaves[i].step);

            if (csAncientRelicStep != null)
            {
                csAncientRelicStep.AncientRelicStepWaveList.Add(new CsAncientRelicStepWave(gameData.ancientRelicStepWaves[i]));
            }
        }

        //WPDAncientRelicStepRoute[]  고대인의유적단계이동경로 목록
        for (int i = 0; i < gameData.ancientRelicStepRoutes.Length; i++)
        {
            CsAncientRelicStep csAncientRelicStep = CsGameData.Instance.AncientRelic.GetAncientRelicStep(gameData.ancientRelicStepRoutes[i].step);

            if (csAncientRelicStep != null)
            {
                csAncientRelicStep.AncientRelicStepRouteList.Add(new CsAncientRelicStepRoute(gameData.ancientRelicStepRoutes[i]));
            }
        }

        //WPDAncientRelicMonsterSkillCastingGuide[] 고대인의유적몬스터스킬발동가이드 목록
        for (int i = 0; i < gameData.ancientRelicMonsterSkillCastingGuides.Length; i++)
        {
            CsAncientRelicStep csAncientRelicStep = CsGameData.Instance.AncientRelic.GetAncientRelicStep(gameData.ancientRelicMonsterSkillCastingGuides[i].step);

            if (csAncientRelicStep != null)
            {
                CsAncientRelicStepWave csAncientRelicStepWave = csAncientRelicStep.GetAncientRelicStepWave(gameData.ancientRelicMonsterSkillCastingGuides[i].waveNo);

                if (csAncientRelicStepWave != null)
                {
                    csAncientRelicStepWave.AncientRelicMonsterSkillCastingGuideList.Add(new CsAncientRelicMonsterSkillCastingGuide(gameData.ancientRelicMonsterSkillCastingGuides[i]));
                }
            }
        }

        //WPDTodayTaskCategory[]  Y 오늘의할일카테고리 목록
        CsGameData.Instance.TodayTaskCategoryList.Clear();

        for (int i = 0; i < gameData.todayTaskCategories.Length; i++)
        {
            CsGameData.Instance.TodayTaskCategoryList.Add(new CsTodayTaskCategory(gameData.todayTaskCategories[i]));
        }

        //WPDTodayTask[]  Y 오늘의할일 목록
        CsGameData.Instance.TodayTaskList.Clear();

        for (int i = 0; i < gameData.todayTasks.Length; i++)
        {
            CsGameData.Instance.TodayTaskList.Add(new CsTodayTask(gameData.todayTasks[i]));
        }

        CsGameData.Instance.TodayTaskList.Sort();

        //WPDTodayTaskAvailableReward[]   Y 오늘의할일획득가능보상 목록
        for (int i = 0; i < gameData.todayTaskAvailableRewards.Length; i++)
        {
            CsTodayTask csTodayTask = CsGameData.Instance.GetTodayTask(gameData.todayTaskAvailableRewards[i].taskId);

            if (csTodayTask != null)
            {
                csTodayTask.TodayTaskAvailableRewardList.Add(new CsTodayTaskAvailableReward(gameData.todayTaskAvailableRewards[i]));
            }
        }

        //WPDAchievementReward[]  Y 달성보상 목록
        CsGameData.Instance.AchievementRewardList.Clear();

        for (int i = 0; i < gameData.achievementRewards.Length; i++)
        {
            CsGameData.Instance.AchievementRewardList.Add(new CsAchievementReward(gameData.achievementRewards[i]));
        }

        //WPDAchievementRewardEntry[] Y 달성보상항목 목록
        for (int i = 0; i < gameData.achievementRewardEntries.Length; i++)
        {
            CsAchievementReward csAchievementReward = CsGameData.Instance.GetAchievementReward(gameData.achievementRewardEntries[i].rewardNo);

            if (csAchievementReward != null)
            {
                csAchievementReward.AchievementRewardEntryList.Add(new CsAchievementRewardEntry(gameData.achievementRewardEntries[i]));
            }
        }

        //WPDDimensionInfiltrationEvent 차원잠입이벤트
        CsGameData.Instance.DimensionInfiltrationEvent = new CsDimensionInfiltrationEvent(gameData.dimensionInfiltrationEvent);

        //WPDDimensionRaidQuest 차원습격퀘스트
        CsGameData.Instance.DimensionRaidQuest = new CsDimensionRaidQuest(gameData.dimensionRaidQuest);

        //WPDDimensionRaidQuestStep[] 차원습격퀘스트단계 목록
        for (int i = 0; i < gameData.dimensionRaidQuestSteps.Length; i++)
        {
            CsGameData.Instance.DimensionRaidQuest.DimensionRaidQuestStepList.Add(new CsDimensionRaidQuestStep(gameData.dimensionRaidQuestSteps[i]));
        }

        //WPDDimensionRaidQuestReward[] 차원습격퀘스트완료보상 목록
        for (int i = 0; i < gameData.dimensionRaidQuestRewards.Length; i++)
        {
            CsGameData.Instance.DimensionRaidQuest.DimensionRaidQuestRewardList.Add(new CsDimensionRaidQuestReward(gameData.dimensionRaidQuestRewards[i]));
        }

        //WPDHolyWarQuest 위대한성전퀘스트
        CsGameData.Instance.HolyWarQuest = new CsHolyWarQuest(gameData.holyWarQuest);

        //WPDHolyWarQuestSchedule[] 위대한성전퀘스트스케줄 목록
        for (int i = 0; i < gameData.holyWarQuestSchedules.Length; i++)
        {
            CsGameData.Instance.HolyWarQuest.HolyWarQuestScheduleList.Add(new CsHolyWarQuestSchedule(gameData.holyWarQuestSchedules[i]));
        }

        //WPDHolyWarQuestGloryLevel[] 위대한성전퀘스트영광레벨 목록
        for (int i = 0; i < gameData.holyWarQuestGloryLevels.Length; i++)
        {
            CsGameData.Instance.HolyWarQuest.HolyWarQuestGloryLevelList.Add(new CsHolyWarQuestGloryLevel(gameData.holyWarQuestGloryLevels[i]));
        }

        // WPDHolyWarQuestReward[]	위대한성전퀘스트보상 목록
        for (int i = 0; i < gameData.holyWarQuestRewards.Length; i++)
        {
            CsGameData.Instance.HolyWarQuest.HolyWarQuestRewardList.Add(new CsHolyWarQuestReward(gameData.holyWarQuestRewards[i]));
        }

        //WPDHonorPointReward[]  명예점수보상 목록
        CsGameData.Instance.HonorPointRewardDictionary.Clear();

        for (int i = 0; i < gameData.honorPointRewards.Length; i++)
        {
            CsGameData.Instance.HonorPointRewardDictionary.Add(gameData.honorPointRewards[i].honorPointRewardId, new CsHonorPointReward(gameData.honorPointRewards[i]));
        }

        //WPDFieldOfHonor 결투장
        CsGameData.Instance.FieldOfHonor = new CsFieldOfHonor(gameData.fieldOfHonor);

        //WPDFieldOfHonorRankingReward[] 결투장랭킹보상 목록
        for (int i = 0; i < gameData.fieldOfHonorRankingRewards.Length; i++)
        {
            CsGameData.Instance.FieldOfHonor.FieldOfHonorRankingRewardList.Add(new CsFieldOfHonorRankingReward(gameData.fieldOfHonorRankingRewards[i]));
        }

        //WPDHonorShopProduct[] 명예상점상품 목록
        CsGameData.Instance.HonorShopProductList.Clear();

        for (int i = 0; i < gameData.honorShopProducts.Length; i++)
        {
            CsGameData.Instance.HonorShopProductList.Add(new CsHonorShopProduct(gameData.honorShopProducts[i]));
        }

        CsGameData.Instance.HonorShopProductList.Sort();

        //WPDRank[] 계급 목록
        CsGameData.Instance.RankList.Clear();

        for (int i = 0; i < gameData.ranks.Length; i++)
        {
            CsGameData.Instance.RankList.Add(new CsRank(gameData.ranks[i]));
        }

        //WPDRankAttr[] 계급속성 목록
        for (int i = 0; i < gameData.rankAttrs.Length; i++)
        {
            CsRank csRank = CsGameData.Instance.GetRank(gameData.rankAttrs[i].rankNo);

            if (csRank != null)
            {
                csRank.RankAttrList.Add(new CsRankAttr(gameData.rankAttrs[i]));
            }
        }

        //WPDRankReward[] 계급보상 목록
        for (int i = 0; i < gameData.rankRewards.Length; i++)
        {
            CsRank csRank = CsGameData.Instance.GetRank(gameData.rankRewards[i].rankNo);

            if (csRank != null)
            {
                csRank.RankRewardList.Add(new CsRankReward(gameData.rankRewards[i]));
            }
        }

        //WPDBattlefieldSupportEvent 전장지원이벤트
        CsGameData.Instance.BattlefieldSupportEvent = new CsBattlefieldSupportEvent(gameData.battlefieldSupportEvent);

        //WPDLevelRankingReward[] 레벨랭킹보상 목록
        CsGameData.Instance.LevelRankingRewardList.Clear();

        for (int i = 0; i < gameData.levelRankingRewards.Length; i++)
        {
            CsGameData.Instance.LevelRankingRewardList.Add(new CsLevelRankingReward(gameData.levelRankingRewards[i]));
        }

        //WPDContentOpenEntry[] 컨텐츠개방항목 목록 ----------------- 보류

        //WPDAttainmentEntry[]  도달항목 목록
        CsGameData.Instance.AttainmentEntryList.Clear();

        for (int i = 0; i < gameData.attainmentEntries.Length; i++)
        {
            CsGameData.Instance.AttainmentEntryList.Add(new CsAttainmentEntry(gameData.attainmentEntries[i]));
        }

        //WPDAttainmentEntryReward[] 도달항목보상 목록
        for (int i = 0; i < gameData.attainmentEntryRewards.Length; i++)
        {
            CsAttainmentEntry csAttainmentEntry = CsGameData.Instance.GetAttainmentEntry(gameData.attainmentEntryRewards[i].entryNo);

            if (csAttainmentEntry != null)
            {
                csAttainmentEntry.AttainmentEntryRewardList.Add(new CsAttainmentEntryReward(gameData.attainmentEntryRewards[i]));
            }
        }

        //WPDMenu[]  메뉴 목록
        CsGameData.Instance.MenuList.Clear();

        for (int i = 0; i < gameData.menus.Length; i++)
        {
            CsGameData.Instance.MenuList.Add(new CsMenu(gameData.menus[i]));
        }

        CsGameData.Instance.MenuList.Sort();

        //WPDMenuContent[]  메뉴컨텐츠 목록
        CsGameData.Instance.MenuContentList.Clear();

        for (int i = 0; i < gameData.menuContents.Length; i++)
        {
            CsGameData.Instance.MenuContentList.Add(new CsMenuContent(gameData.menuContents[i]));
        }

        // WPDMenuContentTutorialStep[]	메뉴컨텐츠튜토리얼단계 목록
        for (int i = 0; i < gameData.menuContentTutorialSteps.Length; i++)
        {
            CsMenuContent csMenuContent = CsGameData.Instance.GetMenuContent(gameData.menuContentTutorialSteps[i].contentId);

            if (csMenuContent != null)
            {
                csMenuContent.MenuContentTutorialStepList.Add(new CsMenuContentTutorialStep(gameData.menuContentTutorialSteps[i]));
            }
        }

        // WPDGuildLevel[] 길드레벨 목록
        CsGameData.Instance.GuildLevelList.Clear();

        for (int i = 0; i < gameData.guildLevels.Length; i++)
        {
            CsGameData.Instance.GuildLevelList.Add(new CsGuildLevel(gameData.guildLevels[i]));
        }

        // WPDGuildMemberGrade[] 길드멤버등급 목록
        CsGameData.Instance.GuildMemberGradeList.Clear();

        for (int i = 0; i < gameData.guildMemberGrades.Length; i++)
        {
            CsGameData.Instance.GuildMemberGradeList.Add(new CsGuildMemberGrade(gameData.guildMemberGrades[i]));

        }

        //WPDSupplySupportQuest  보급지원퀘스트
        CsGameData.Instance.SupplySupportQuest = new CsSupplySupportQuest(gameData.supplySupportQuest);

        //WPDSupplySupportQuestOrder[]  보급지원퀘스트지령 목록
        for (int i = 0; i < gameData.supplySupportQuestOrders.Length; i++)
        {
            CsGameData.Instance.SupplySupportQuest.SupplySupportQuestOrderList.Add(new CsSupplySupportQuestOrder(gameData.supplySupportQuestOrders[i]));
        }

        //WPDSupplySupportQuestWayPoint[] 보급지원퀘스트중간지점 목록
        for (int i = 0; i < gameData.supplySupportQuestWayPoints.Length; i++)
        {
            CsGameData.Instance.SupplySupportQuest.SupplySupportQuestWayPointList.Add(new CsSupplySupportQuestWayPoint(gameData.supplySupportQuestWayPoints[i]));
        }

        //WPDSupplySupportQuestCart[] 보급지원퀘스트수레 목록
        for (int i = 0; i < gameData.supplySupportQuestCarts.Length; i++)
        {
            CsGameData.Instance.SupplySupportQuest.SupplySupportQuestCartList.Add(new CsSupplySupportQuestCart(gameData.supplySupportQuestCarts[i]));
        }

        CsGameData.Instance.SupplySupportQuest.SupplySupportQuestCartList.Sort();

        //WPDSupplySupportQuestReward[] 보급지원퀘스트보상 목록
        for (int i = 0; i < gameData.supplySupportQuestRewards.Length; i++)
        {
            CsSupplySupportQuestCart csSupplySupportQuestCart = CsGameData.Instance.SupplySupportQuest.GetSupplySupportQuestCart(gameData.supplySupportQuestRewards[i].cartId);

            if (csSupplySupportQuestCart != null)
            {
                csSupplySupportQuestCart.SupplySupportQuestRewardList.Add(new CsSupplySupportQuestReward(gameData.supplySupportQuestRewards[i]));
            }
        }

        //WPDGuildContributionPointReward[] 길드공헌점수보상 목록
        CsGameData.Instance.GuildContributionPointRewardDictionary.Clear();

        for (int i = 0; i < gameData.guildContributionPointRewards.Length; i++)
        {
            CsGameData.Instance.GuildContributionPointRewardDictionary.Add(gameData.guildContributionPointRewards[i].guildContributionPointRewardId, new CsGuildContributionPointReward(gameData.guildContributionPointRewards[i]));
        }

        //WPDGuildBuildingPointReward[] 길드건설점수보상 목록
        CsGameData.Instance.GuildBuildingPointRewardDictionary.Clear();

        for (int i = 0; i < gameData.guildBuildingPointRewards.Length; i++)
        {
            CsGameData.Instance.GuildBuildingPointRewardDictionary.Add(gameData.guildBuildingPointRewards[i].guildBuildingPointRewardId, new CsGuildBuildingPointReward(gameData.guildBuildingPointRewards[i]));
        }

        //WPDGuildFundReward[] 길드자금보상 목록
        CsGameData.Instance.GuildFundRewardDictionary.Clear();

        for (int i = 0; i < gameData.guildFundRewards.Length; i++)
        {
            CsGameData.Instance.GuildFundRewardDictionary.Add(gameData.guildFundRewards[i].guildFundRewardId, new CsGuildFundReward(gameData.guildFundRewards[i]));
        }

        //WPDGuildPointReward[] 길드점수보상 목록
        CsGameData.Instance.GuildPointRewardDictionary.Clear();

        for (int i = 0; i < gameData.guildPointRewards.Length; i++)
        {
            CsGameData.Instance.GuildPointRewardDictionary.Add(gameData.guildPointRewards[i].guildPointRewardId, new CsGuildPointReward(gameData.guildPointRewards[i]));
        }

        // WPDOwnDiaReward[]귀속다이아보상 목록
        CsGameData.Instance.OwnDiaRewardDictionary.Clear();

        for (int i = 0; i < gameData.ownDiaRewards.Length; i++)
        {
            CsGameData.Instance.OwnDiaRewardDictionary.Add(gameData.ownDiaRewards[i].ownDiaRewardId, new CsOwnDiaReward(gameData.ownDiaRewards[i]));
        }

        //WPDGuildDonationEntry[] 길드기부항목 목록
        CsGameData.Instance.GuildDonationEntryList.Clear();

        for (int i = 0; i < gameData.guildDonationEntries.Length; i++)
        {
            CsGameData.Instance.GuildDonationEntryList.Add(new CsGuildDonationEntry(gameData.guildDonationEntries[i]));
        }

        //WPDNationFundReward[] 국고자금보상 목록
        CsGameData.Instance.NationFundRewardDictionary.Clear();

        for (int i = 0; i < gameData.nationFundRewards.Length; i++)
        {
            CsGameData.Instance.NationFundRewardDictionary.Add(gameData.nationFundRewards[i].nationFundRewardId, new CsNationFundReward(gameData.nationFundRewards[i]));
        }

        //WPDNationDonationEntry[] 국가기부항목 목록
        CsGameData.Instance.NationDonationEntryList.Clear();

        for (int i = 0; i < gameData.nationDonationEntries.Length; i++)
        {
            CsGameData.Instance.NationDonationEntryList.Add(new CsNationDonationEntry(gameData.nationDonationEntries[i]));
        }

        //WPDNationNoblesse[] 국가관직 목록
        CsGameData.Instance.NationNoblesseList.Clear();

        for (int i = 0; i < gameData.nationNoblesses.Length; i++)
        {
            CsGameData.Instance.NationNoblesseList.Add(new CsNationNoblesse(gameData.nationNoblesses[i]));
        }

        //WPDNationNoblesseAttr[] 국가관직속성 목록
        for (int i = 0; i < gameData.nationNoblesseAttrs.Length; i++)
        {
            CsNationNoblesse csNationNoblesse = CsGameData.Instance.GetNationNoblesse(gameData.nationNoblesseAttrs[i].noblesseId);

            if (csNationNoblesse != null)
            {
                csNationNoblesse.NationNoblesseAttrList.Add(new CsNationNoblesseAttr(gameData.nationNoblesseAttrs[i]));
            }
        }

        //WPDNationNoblesseAppointmentAuthority[] 국가관직임명권한 목록
        for (int i = 0; i < gameData.nationNoblesseAppointmentAuthorities.Length; i++)
        {
            CsNationNoblesse csNationNoblesse = CsGameData.Instance.GetNationNoblesse(gameData.nationNoblesseAppointmentAuthorities[i].noblesseId);

            if (csNationNoblesse != null)
            {
                csNationNoblesse.NationNoblesseAppointmentAuthorityList.Add(new CsNationNoblesseAppointmentAuthority(gameData.nationNoblesseAppointmentAuthorities[i]));
            }
        }

        //WPDGuildBuilding[] 길드건물 목록
        CsGameData.Instance.GuildBuildingList.Clear();

        for (int i = 0; i < gameData.guildBuildings.Length; i++)
        {
            CsGameData.Instance.GuildBuildingList.Add(new CsGuildBuilding(gameData.guildBuildings[i]));
        }

        //WPDGuildBuildingLevel[] 길드건물레벨 목록
        for (int i = 0; i < gameData.guildBuildingLevels.Length; i++)
        {
            CsGuildBuilding csGuildBuilding = CsGameData.Instance.GetGuildBuilding(gameData.guildBuildingLevels[i].buildingId);

            if (csGuildBuilding != null)
            {
                csGuildBuilding.GuildBuildingLevelList.Add(new CsGuildBuildingLevel(gameData.guildBuildingLevels[i]));
            }
        }

        //WPDGuildSkill[] 길드스킬 목록
        CsGameData.Instance.GuildSkillList.Clear();

        for (int i = 0; i < gameData.guildSkills.Length; i++)
        {
            CsGameData.Instance.GuildSkillList.Add(new CsGuildSkill(gameData.guildSkills[i]));
        }

        //WPDGuildSkillAttr[] 길드스킬속성 목록
        for (int i = 0; i < gameData.guildSkillAttrs.Length; i++)
        {
            CsGuildSkill csGuildSkill = CsGameData.Instance.GetGuildSkill(gameData.guildSkillAttrs[i].guildSkillId);

            if (csGuildSkill != null)
            {
                csGuildSkill.GuildSkillAttrList.Add(new CsGuildSkillAttr(gameData.guildSkillAttrs[i]));
            }
        }

        //WPDGuildSkillLevel[] 길드스킬레벨 목록
        for (int i = 0; i < gameData.guildSkillLevels.Length; i++)
        {
            CsGuildSkill csGuildSkill = CsGameData.Instance.GetGuildSkill(gameData.guildSkillLevels[i].guildSkillId);

            if (csGuildSkill != null)
            {
                csGuildSkill.GuildSkillLevelList.Add(new CsGuildSkillLevel(gameData.guildSkillLevels[i]));
            }
        }

        //WPDGuildSkillLevelAttrValue[] 길드스킬레벨속성값 목록
        for (int i = 0; i < gameData.guildSkillLevelAttrValues.Length; i++)
        {
            CsGuildSkill csGuildSkill = CsGameData.Instance.GetGuildSkill(gameData.guildSkillLevelAttrValues[i].guildSkillId);

            if (csGuildSkill != null)
            {
                CsGuildSkillAttr csGuildSkillAttr = csGuildSkill.GetGuildSkillAttr(gameData.guildSkillLevelAttrValues[i].attrId);

                if (csGuildSkillAttr != null)
                {
                    csGuildSkillAttr.GuildSkillLevelAttrValueList.Add(new CsGuildSkillLevelAttrValue(gameData.guildSkillLevelAttrValues[i]));
                }
            }
        }

        //WPDGuildTerritory 길드영지
        CsGameData.Instance.GuildTerritory = new CsGuildTerritory(gameData.guildTerritory);

        //WPDGuildTerritoryNpc[] 길드영지NPC 목록
        for (int i = 0; i < gameData.guildTerritoryNpcs.Length; i++)
        {
            CsGameData.Instance.GuildTerritory.GuildTerritoryNpcList.Add(new CsGuildTerritoryNpc(gameData.guildTerritoryNpcs[i]));
        }

        //WPDGuildFarmQuest 길드농장퀘스트
        CsGameData.Instance.GuildFarmQuest = new CsGuildFarmQuest(gameData.guildFarmQuest);

        //WPDGuildFarmQuestReward[] 길드농장퀘스트보상 목록
        for (int i = 0; i < gameData.guildFarmQuestRewards.Length; i++)
        {
            CsGameData.Instance.GuildFarmQuest.GuildFarmQuestRewardList.Add(new CsGuildFarmQuestReward(gameData.guildFarmQuestRewards[i]));
        }

        //WPDGuildFoodWarehouse 길드군량창고
        CsGameData.Instance.GuildFoodWarehouse = new CsGuildFoodWarehouse(gameData.guildFoodWarehouse);

        //WPDGuildFoodWarehouseLevel[] 길드군량창고레벨 목록
        for (int i = 0; i < gameData.guildFoodWarehouseLevels.Length; i++)
        {
            CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList.Add(new CsGuildFoodWarehouseLevel(gameData.guildFoodWarehouseLevels[i]));
        }

        //WPDGuildAltar 길드제단
        CsGameData.Instance.GuildAltar = new CsGuildAltar(gameData.guildAltar);

        //WPDGuildAltarDefenseMonsterAttrFactor[] 길드제단수비몬스터속성계수 목록
        for (int i = 0; i < gameData.guildAltarDefenseMonsterAttrFactors.Length; i++)
        {
            CsGameData.Instance.GuildAltar.GuildAltarDefenseMonsterAttrFactorList.Add(new CsGuildAltarDefenseMonsterAttrFactor(gameData.guildAltarDefenseMonsterAttrFactors[i]));
        }

        //WPDGuildMissionQuest 길드미션퀘스트
        CsGameData.Instance.GuildMissionQuest = new CsGuildMissionQuest(gameData.guildMissionQuest);

        //WPDGuildMissionQuestReward[] 길드미션퀘스트보상 목록
        for (int i = 0; i < gameData.guildMissionQuestRewards.Length; i++)
        {
            CsGameData.Instance.GuildMissionQuest.GuildMissionQuestRewardList.Add(new CsGuildMissionQuestReward(gameData.guildMissionQuestRewards[i]));
        }

        //WPDGuildMission[] 길드미션 목록
        for (int i = 0; i < gameData.guildMissions.Length; i++)
        {
            CsGameData.Instance.GuildMissionQuest.GuildMissionList.Add(new CsGuildMission(gameData.guildMissions[i]));
        }

        //WPDNationWarRevivalPoint[] 국가전부활포인트 목록
        CsGameData.Instance.NationWarRevivalPointList.Clear();

        for (int i = 0; i < gameData.nationWarRevivalPoints.Length; i++)
        {
            CsGameData.Instance.NationWarRevivalPointList.Add(new CsNationWarRevivalPoint(gameData.nationWarRevivalPoints[i]));
        }

        //WPDNationWar 국가전
        CsGameData.Instance.NationWar = new CsNationWar(gameData.nationWar);

        //WPDNationWarAvailableDayOfWeek[]  국가전가능요일 목록
        for (int i = 0; i < gameData.nationWarAvailableDayOfWeeks.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarAvailableDayOfWeekList.Add(new CsNationWarAvailableDayOfWeek(gameData.nationWarAvailableDayOfWeeks[i]));
        }

        //WPDNationWarNpc[]  국가전NPC 목록
        for (int i = 0; i < gameData.nationWarNpcs.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarNpcList.Add(new CsNationWarNpc(gameData.nationWarNpcs[i]));
        }

        //WPDNationWarTransmissionExit[]  국가전전송출구 목록
        for (int i = 0; i < gameData.nationWarTransmissionExits.Length; i++)
        {
            CsNationWarNpc csNationWarNpc = CsGameData.Instance.NationWar.GetNationWarNpc(gameData.nationWarTransmissionExits[i].npcId);

            if (csNationWarNpc != null)
            {
                csNationWarNpc.NationWarTransmissionExitList.Add(new CsNationWarTransmissionExit(gameData.nationWarTransmissionExits[i]));
            }
        }

        //WPDNationWarMonsterArrange[]  국가전몬스터배치 목록
        for (int i = 0; i < gameData.nationWarMonsterArranges.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarMonsterArrangeList.Add(new CsNationWarMonsterArrange(gameData.nationWarMonsterArranges[i]));
        }

        //WPDNationWarRevivalPointActivationCondition[] 국가전부활포인트활성조건 목록
        for (int i = 0; i < gameData.nationWarRevivalPointActivationConditions.Length; i++)
        {
            CsNationWarMonsterArrange csNationWarMonsterArrange = CsGameData.Instance.NationWar.GetNationWarMonsterArrange(gameData.nationWarRevivalPointActivationConditions[i].arrangeId);

            if (csNationWarMonsterArrange != null)
            {
                csNationWarMonsterArrange.NationWarRevivalPointActivationConditionList.Add(new CsNationWarRevivalPointActivationCondition(gameData.nationWarRevivalPointActivationConditions[i]));
            }
        }

        //WPDNationWarPaidTransmission[]  국가전유료전송 목록
        for (int i = 0; i < gameData.nationWarPaidTransmissions.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarPaidTransmissionList.Add(new CsNationWarPaidTransmission(gameData.nationWarPaidTransmissions[i]));
        }

        //WPDNationWarHeroObjectiveEntry[]  국가전영웅목표항목 목록
        for (int i = 0; i < gameData.nationWarHeroObjectiveEntries.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarHeroObjectiveEntryList.Add(new CsNationWarHeroObjectiveEntry(gameData.nationWarHeroObjectiveEntries[i]));
        }

        //WPDNationWarExpReward[] 국가전경험치보상 목록
        for (int i = 0; i < gameData.nationWarExpRewards.Length; i++)
        {
            CsGameData.Instance.NationWar.NationWarExpRewardList.Add(new CsNationWarExpReward(gameData.nationWarExpRewards[i]));
        }

        //WPDGuildSupplySupportQuest 길드물자지원퀘스트
        CsGameData.Instance.GuildSupplySupportQuest = new CsGuildSupplySupportQuest(gameData.guildSupplySupportQuest);

        //WPDGuildSupplySupportQuestReward[] 길드물자지원퀘스트보상 목록
        for (int i = 0; i < gameData.guildSupplySupportQuestRewards.Length; i++)
        {
            CsGameData.Instance.GuildSupplySupportQuest.GuildSupplySupportQuestRewardList.Add(new CsGuildSupplySupportQuestReward(gameData.guildSupplySupportQuestRewards[i]));
        }

        // WPDGuildContent[] 길드컨텐츠 목록
        CsGameData.Instance.GuildContentList.Clear();

        for (int i = 0; i < gameData.guildContents.Length; i++)
        {
            CsGameData.Instance.GuildContentList.Add(new CsGuildContent(gameData.guildContents[i]));
        }

        // WPDGuildDailyObjectiveReward[] 길드일일목표보상 목록
        CsGameData.Instance.GuildDailyObjectiveRewardList.Clear();

        for (int i = 0; i < gameData.guildDailyObjectiveRewards.Length; i++)
        {
            CsGameData.Instance.GuildDailyObjectiveRewardList.Add(new CsGuildDailyObjectiveReward(gameData.guildDailyObjectiveRewards[i]));
        }

        // WPDGuildWeeklyObjective[] 길드주간목표 목록
        CsGameData.Instance.GuildWeeklyObjectiveList.Clear();

        for (int i = 0; i < gameData.guildWeeklyObjectives.Length; i++)
        {
            CsGameData.Instance.GuildWeeklyObjectiveList.Add(new CsGuildWeeklyObjective(gameData.guildWeeklyObjectives[i]));
        }

        // WPDGuildHuntingQuest 길드헌팅퀘스트
        CsGameData.Instance.GuildHuntingQuest = new CsGuildHuntingQuest(gameData.guildHuntingQuest);

        // WPDGuildHuntingQuestObjective[] 길드헌팅퀘스트목표 목록
        for (int i = 0; i < gameData.guildHuntingQuestObjectives.Length; i++)
        {
            CsGameData.Instance.GuildHuntingQuest.GuildHuntingQuestObjectiveList.Add(new CsGuildHuntingQuestObjective(gameData.guildHuntingQuestObjectives[i]));
        }

        //WPDSoulCoveter 영혼을탐하는자
        CsGameData.Instance.SoulCoveter = new CsSoulCoveter(gameData.soulCoveter);

        //WPDSoulCoveterAvailableReward[] 영혼을탐하는자획득가능보상 목록
        for (int i = 0; i < gameData.soulCoveterAvailableRewards.Length; i++)
        {
            CsGameData.Instance.SoulCoveter.SoulCoveterAvailableRewardList.Add(new CsSoulCoveterAvailableReward(gameData.soulCoveterAvailableRewards[i]));
        }

        //WPDSoulCoveterObstacle[] 영혼을탐하는자장애물 목록
        for (int i = 0; i < gameData.soulCoveterObstacles.Length; i++)
        {
            CsGameData.Instance.SoulCoveter.SoulCoveterObstacleList.Add(new CsSoulCoveterObstacle(gameData.soulCoveterObstacles[i]));
        }

        //WPDSoulCoveterDifficulty[] 영혼을탐하는자난이도 목록
        for (int i = 0; i < gameData.soulCoveterDifficulties.Length; i++)
        {
            CsGameData.Instance.SoulCoveter.SoulCoveterDifficultyList.Add(new CsSoulCoveterDifficulty(gameData.soulCoveterDifficulties[i]));
        }

        //WPDSoulCoveterDifficultyWave[] 영혼을탐하는자난이도웨이브 목록
        for (int i = 0; i < gameData.soulCoveterDifficultyWaves.Length; i++)
        {
            CsSoulCoveterDifficulty csSoulCoveterDifficulty = CsGameData.Instance.SoulCoveter.GetSoulCoveterDifficulty(gameData.soulCoveterDifficultyWaves[i].difficulty);

            if (csSoulCoveterDifficulty != null)
            {
                csSoulCoveterDifficulty.SoulCoveterDifficultyWaveList.Add(new CsSoulCoveterDifficultyWave(gameData.soulCoveterDifficultyWaves[i]));
            }
        }

        //WPDClientTutorialStep[] 클라이언트튜토리얼단계 목록
        CsGameData.Instance.ClientTutorialStepList.Clear();

        for (int i = 0; i < gameData.clientTutorialSteps.Length; i++)
        {
            CsGameData.Instance.ClientTutorialStepList.Add(new CsClientTutorialStep(gameData.clientTutorialSteps[i]));
        }

        //WPDIllustratedBookCategory[] 도감카테고리 목록
        CsGameData.Instance.IllustratedBookCategoryList.Clear();

        for (int i = 0; i < gameData.illustratedBookCategories.Length; i++)
        {
            CsGameData.Instance.IllustratedBookCategoryList.Add(new CsIllustratedBookCategory(gameData.illustratedBookCategories[i]));
        }

        //WPDIllustratedBookType[] 도감타입 목록
        CsGameData.Instance.IllustratedBookTypeList.Clear();

        for (int i = 0; i < gameData.illustratedBookTypes.Length; i++)
        {
            CsGameData.Instance.IllustratedBookTypeList.Add(new CsIllustratedBookType(gameData.illustratedBookTypes[i]));
        }

        //WPDIllustratedBook[] 도감 목록
        CsGameData.Instance.IllustratedBookList.Clear();

        for (int i = 0; i < gameData.illustratedBooks.Length; i++)
        {
            CsGameData.Instance.IllustratedBookList.Add(new CsIllustratedBook(gameData.illustratedBooks[i]));
        }

        //WPDIllustratedBookAttrGrade[] 도감속성등급 목록
        CsGameData.Instance.IllustratedBookAttrGradeList.Clear();

        for (int i = 0; i < gameData.illustratedBookAttrGrades.Length; i++)
        {
            CsGameData.Instance.IllustratedBookAttrGradeList.Add(new CsIllustratedBookAttrGrade(gameData.illustratedBookAttrGrades[i]));
        }

        //WPDIllustratedBookAttr[] 도감속성 목록
        for (int i = 0; i < gameData.illustratedBookAttrs.Length; i++)
        {
            CsIllustratedBook csIllustratedBook = CsGameData.Instance.GetIllustratedBook(gameData.illustratedBookAttrs[i].illustratedBookId);

            if (csIllustratedBook != null)
            {
                csIllustratedBook.IllustratedBookAttrList.Add(new CsIllustratedBookAttr(gameData.illustratedBookAttrs[i]));
            }
        }

        //WPDIllustratedBookExplorationStep[] 도감탐험단계 목록
        CsGameData.Instance.IllustratedBookExplorationStepList.Clear();

        for (int i = 0; i < gameData.illustratedBookExplorationSteps.Length; i++)
        {
            CsGameData.Instance.IllustratedBookExplorationStepList.Add(new CsIllustratedBookExplorationStep(gameData.illustratedBookExplorationSteps[i]));
        }

        //WPDIllustratedBookExplorationStepAttr[] 도감탐험단계속성 목록
        for (int i = 0; i < gameData.illustratedBookExplorationStepAttrs.Length; i++)
        {
            CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(gameData.illustratedBookExplorationStepAttrs[i].stepNo);

            if (csIllustratedBookExplorationStep != null)
            {
                csIllustratedBookExplorationStep.IllustratedBookExplorationStepAttrList.Add(new CsIllustratedBookExplorationStepAttr(gameData.illustratedBookExplorationStepAttrs[i]));
            }
        }

        //WPDIllustratedBookExplorationStepReward[] 도감탐험단계보상 목록
        for (int i = 0; i < gameData.illustratedBookExplorationStepRewards.Length; i++)
        {
            CsIllustratedBookExplorationStep csIllustratedBookExplorationStep = CsGameData.Instance.GetIllustratedBookExplorationStep(gameData.illustratedBookExplorationStepRewards[i].stepNo);

            if (csIllustratedBookExplorationStep != null)
            {
                csIllustratedBookExplorationStep.IllustratedBookExplorationStepRewardList.Add(new CsIllustratedBookExplorationStepReward(gameData.illustratedBookExplorationStepRewards[i]));
            }
        }

        //WPDSceneryQuest[] 풍광퀘스트 목록
        CsGameData.Instance.SceneryQuestList.Clear();

        for (int i = 0; i < gameData.sceneryQuests.Length; i++)
        {
            CsGameData.Instance.SceneryQuestList.Add(new CsSceneryQuest(gameData.sceneryQuests[i]));
        }

        //WPDTitleCategory[] 칭호카테고리 목록
        CsGameData.Instance.TitleCategoryList.Clear();

        for (int i = 0; i < gameData.titleCategories.Length; i++)
        {
            CsGameData.Instance.TitleCategoryList.Add(new CsTitleCategory(gameData.titleCategories[i]));
        }

        //WPDTitleType[] 칭호타입 목록
        CsGameData.Instance.TitleTypeList.Clear();

        for (int i = 0; i < gameData.titleTypes.Length; i++)
        {
            CsGameData.Instance.TitleTypeList.Add(new CsTitleType(gameData.titleTypes[i]));
        }

        //WPDTitleGrade[] 칭호등급 목록
        CsGameData.Instance.TitleGradeList.Clear();

        for (int i = 0; i < gameData.titleGrades.Length; i++)
        {
            CsGameData.Instance.TitleGradeList.Add(new CsTitleGrade(gameData.titleGrades[i]));
        }

        //WPDTitle[] 칭호 목록
        CsGameData.Instance.TitleList.Clear();

        for (int i = 0; i < gameData.titles.Length; i++)
        {
            CsGameData.Instance.TitleList.Add(new CsTitle(gameData.titles[i]));
        }

		CsGameData.Instance.TitleList.Sort();

		//WPDTitleActiveAttr[] 칭호액티브속성 목록
		for (int i = 0; i < gameData.titleActiveAttrs.Length; i++)
        {
            CsTitle csTitle = CsGameData.Instance.GetTitle(gameData.titleActiveAttrs[i].titleId);

            if (csTitle != null)
            {
                csTitle.TitleActiveAttrList.Add(new CsTitleActiveAttr(gameData.titleActiveAttrs[i]));
            }
        }

        //WPDTitlePassiveAttr[] 칭호패시브속성 목록
        for (int i = 0; i < gameData.titlePassiveAttrs.Length; i++)
        {
            CsTitle csTitle = CsGameData.Instance.GetTitle(gameData.titlePassiveAttrs[i].titleId);

            if (csTitle != null)
            {
                csTitle.TitlePassiveAttrList.Add(new CsTitlePassiveAttr(gameData.titlePassiveAttrs[i]));
            }
        }

		//WPDAccomplishmentPointReward[] 업적점수보상 목록
		CsGameData.Instance.AccomplishmentPointRewardDictionary.Clear();


		//WPDAccomplishmentCategory[] 업적카테고리 목록
		CsGameData.Instance.AccomplishmentCategoryList.Clear();

        for (int i = 0; i < gameData.accomplishmentCategories.Length; i++)
        {
            CsGameData.Instance.AccomplishmentCategoryList.Add(new CsAccomplishmentCategory(gameData.accomplishmentCategories[i]));
        }

        //WPDAccomplishment[] 업적 목록
        CsGameData.Instance.AccomplishmentList.Clear();

        for (int i = 0; i < gameData.accomplishments.Length; i++)
        {
            CsGameData.Instance.AccomplishmentList.Add(new CsAccomplishment(gameData.accomplishments[i]));
        }

        //WPDEliteMonsterCategory[] 정예몬스터카테고리 목록
        CsGameData.Instance.EliteMonsterCategoryList.Clear();

        for (int i = 0; i < gameData.eliteMonsterCategories.Length; i++)
        {
            CsGameData.Instance.EliteMonsterCategoryList.Add(new CsEliteMonsterCategory(gameData.eliteMonsterCategories[i]));
        }

        //WPDEliteMonsterMaster[] 정예몬스터마스터 목록
        CsGameData.Instance.EliteMonsterMasterList.Clear();

        for (int i = 0; i < gameData.eliteMonsterMasters.Length; i++)
        {
            CsGameData.Instance.EliteMonsterMasterList.Add(new CsEliteMonsterMaster(gameData.eliteMonsterMasters[i]));
        }

        //WPDEliteMonsterSpawnSchedule[] 정예몬스터출몰스케줄 목록
        for (int i = 0; i < gameData.eliteMonsterSpawnSchedules.Length; i++)
        {
            CsEliteMonsterMaster csEliteMonsterMaster = CsGameData.Instance.GetEliteMonsterMaster(gameData.eliteMonsterSpawnSchedules[i].eliteMonsterMasterId);

            if (csEliteMonsterMaster != null)
            {
                csEliteMonsterMaster.EliteMonsterSpawnScheduleList.Add(new CsEliteMonsterSpawnSchedule(gameData.eliteMonsterSpawnSchedules[i]));
            }
        }

        //WPDEliteMonster[] 정예몬스터 목록
        CsGameData.Instance.EliteMonsterList.Clear();

        for (int i = 0; i < gameData.eliteMonsters.Length; i++)
        {
            CsGameData.Instance.EliteMonsterList.Add(new CsEliteMonster(gameData.eliteMonsters[i]));
        }

        //WPDEliteMonsterKillAttrValue[] 정예몬스터처치속성 목록
        for (int i = 0; i < gameData.eliteMonsterKillAttrValues.Length; i++)
        {
            CsEliteMonster csEliteMonster = CsGameData.Instance.GetEliteMonster(gameData.eliteMonsterKillAttrValues[i].eliteMonsterId);

            if (csEliteMonster != null)
            {
                csEliteMonster.EliteMonsterKillAttrValueList.Add(new CsEliteMonsterKillAttrValue(gameData.eliteMonsterKillAttrValues[i]));
            }
        }

        //WPDEliteDungeon 정예던전
        CsGameData.Instance.EliteDungeon = new CsEliteDungeon(gameData.eliteDungeon);

        //WPDCreatureCardCategory[] 크리처카드카테고리 목록
        CsGameData.Instance.CreatureCardCategoryList.Clear();

        for (int i = 0; i < gameData.creatureCardCategories.Length; i++)
        {
            CsGameData.Instance.CreatureCardCategoryList.Add(new CsCreatureCardCategory(gameData.creatureCardCategories[i]));
        }

        //WPDCreatureCardGrade[]  크리처카드등급 목록
        CsGameData.Instance.CreatureCardGradeList.Clear();

        for (int i = 0; i < gameData.creatureCardGrades.Length; i++)
        {
            CsGameData.Instance.CreatureCardGradeList.Add(new CsCreatureCardGrade(gameData.creatureCardGrades[i]));
        }

        //WPDCreatureCard[] 크리처카드 목록
        CsGameData.Instance.CreatureCardList.Clear();

        for (int i = 0; i < gameData.creatureCards.Length; i++)
        {
            CsGameData.Instance.CreatureCardList.Add(new CsCreatureCard(gameData.creatureCards[i]));
        }


        //WPDCreatureCardCollectionCategory[] 크리처카드콜렉션카테고리 목록
        CsGameData.Instance.CreatureCardCollectionCategoryList.Clear();

        for (int i = 0; i < gameData.creatureCardCollectionCategories.Length; i++)
        {
            CsGameData.Instance.CreatureCardCollectionCategoryList.Add(new CsCreatureCardCollectionCategory(gameData.creatureCardCollectionCategories[i]));
        }

        //WPDCreatureCardCollectionGrade[] 크리처카드콜렉션등급 목록
        CsGameData.Instance.CreatureCardCollectionGradeList.Clear();

        for (int i = 0; i < gameData.creatureCardCollectionGrades.Length; i++)
        {
            CsGameData.Instance.CreatureCardCollectionGradeList.Add(new CsCreatureCardCollectionGrade(gameData.creatureCardCollectionGrades[i]));
        }

        //WPDCreatureCardCollection[] 크리처카드콜렉션 목록
        CsGameData.Instance.CreatureCardCollectionList.Clear();

        for (int i = 0; i < gameData.creatureCardCollections.Length; i++)
        {
            CsGameData.Instance.CreatureCardCollectionList.Add(new CsCreatureCardCollection(gameData.creatureCardCollections[i]));
        }

        //WPDCreatureCardCollectionAttr[] 크리처카드콜렉션속성 목록
        for (int i = 0; i < gameData.creatureCardCollectionAttrs.Length; i++)
        {
            CsCreatureCardCollection csCreatureCardCollection = CsGameData.Instance.GetCreatureCardCollection(gameData.creatureCardCollectionAttrs[i].collectionId);

            if (csCreatureCardCollection != null)
            {
                csCreatureCardCollection.CreatureCardCollectionAttrList.Add(new CsCreatureCardCollectionAttr(gameData.creatureCardCollectionAttrs[i]));
            }
        }

        //WPDCreatureCardCollectionEntry[] 크리처카드콜렉션항목 목록
        CsGameData.Instance.CreatureCardCollectionEntryList.Clear();

        for (int i = 0; i < gameData.creatureCardCollectionEntries.Length; i++)
        {
            CsGameData.Instance.CreatureCardCollectionEntryList.Add(new CsCreatureCardCollectionEntry(gameData.creatureCardCollectionEntries[i]));
        }

        //WPDCreatureCardShopRefreshSchedule[] 크리처카드상점갱신스케줄 목록
        CsGameData.Instance.CreatureCardShopRefreshScheduleList.Clear();

        for (int i = 0; i < gameData.creatureCardShopRefreshSchedules.Length; i++)
        {
            CsGameData.Instance.CreatureCardShopRefreshScheduleList.Add(new CsCreatureCardShopRefreshSchedule(gameData.creatureCardShopRefreshSchedules[i]));
        }

        //WPDCreatureCardShopFixedProduct[] 크리처카드상점고정상품 목록
        CsGameData.Instance.CreatureCardShopFixedProductList.Clear();

        for (int i = 0; i < gameData.creatureCardShopFixedProducts.Length; i++)
        {
            CsGameData.Instance.CreatureCardShopFixedProductList.Add(new CsCreatureCardShopFixedProduct(gameData.creatureCardShopFixedProducts[i]));
        }

        //WPDCreatureCardShopRandomProduct[] 크리처카드상점랜덤상품 목록
        CsGameData.Instance.CreatureCardShopRandomProductList.Clear();

        for (int i = 0; i < gameData.creatureCardShopRandomProducts.Length; i++)
        {
            CsGameData.Instance.CreatureCardShopRandomProductList.Add(new CsCreatureCardShopRandomProduct(gameData.creatureCardShopRandomProducts[i]));
        }

		// WPDMainQuestRewardItem[] 메인퀘스트 보상아이템 목록
		for (int i = 0; i < gameData.mainQuestRewards.Length; i++)
		{
			CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest(gameData.mainQuestRewards[i].mainQuestNo);

			if (csMainQuest != null)
			{
				csMainQuest.MainQuestRewardItemList.Add(new CsMainQuestReward(gameData.mainQuestRewards[i]));
			}
		}

		//WPDProofOfValor 용맹의증명
		CsGameData.Instance.ProofOfValor = new CsProofOfValor(gameData.proofOfValor);

        //WPDProofOfValorBuffBox[] 용맹의증명버프상자 목록
        for (int i = 0; i < gameData.proofOfValorBuffBoxs.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorBuffBoxList.Add(new CsProofOfValorBuffBox(gameData.proofOfValorBuffBoxs[i]));
        }

        //WPDProofOfValorBuffBoxArrange[] 용맹의증명버프상자배치 목록
        for (int i = 0; i < gameData.proofOfValorBuffBoxArranges.Length; i++)
        {
            CsProofOfValorBuffBox csProofOfValorBuffBox = CsGameData.Instance.ProofOfValor.GetProofOfValorBuffBox(gameData.proofOfValorBuffBoxArranges[i].buffBoxId);

            if (csProofOfValorBuffBox != null)
            {
                csProofOfValorBuffBox.ProofOfValorBuffBoxArrangeList.Add(new CsProofOfValorBuffBoxArrange(gameData.proofOfValorBuffBoxArranges[i]));
            }
        }

        //WPDProofOfValorBossMonsterArrange[] 용맹의증명보스몬스터배치 목록
        for (int i = 0; i < gameData.proofOfValorBossMonsterArranges.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorBossMonsterArrangeList.Add(new CsProofOfValorBossMonsterArrange(gameData.proofOfValorBossMonsterArranges[i]));
        }

        //WPDProofOfValorPaidRefresh[]  용맹의증명유료갱신 목록
        for (int i = 0; i < gameData.proofOfValorPaidRefreshs.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorPaidRefreshList.Add(new CsProofOfValorPaidRefresh(gameData.proofOfValorPaidRefreshs[i]));
        }

        //WPDProofOfValorRefreshSchedule[] 용맹의증명갱신스케줄 목록
        for (int i = 0; i < gameData.proofOfValorRefreshSchedules.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorRefreshScheduleList.Add(new CsProofOfValorRefreshSchedule(gameData.proofOfValorRefreshSchedules[i]));
        }

        //WPDProofOfValorReward[] 용맹의증명보상 목록
        for (int i = 0; i < gameData.proofOfValorRewards.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorRewardList.Add(new CsProofOfValorReward(gameData.proofOfValorRewards[i]));
        }

        //WPDProofOfValorClearGrade[] 용맹의증명클리어등급 목록
        for (int i = 0; i < gameData.proofOfValorClearGrades.Length; i++)
        {
            CsGameData.Instance.ProofOfValor.ProofOfValorClearGradeList.Add(new CsProofOfValorClearGrade(gameData.proofOfValorClearGrades[i]));
        }

        // WPDStaminaRecoverySchedule[] 체력회복스케줄 목록
        CsGameData.Instance.StaminaRecoveryScheduleList.Clear();

        for (int i = 0; i < gameData.staminaRecoverySchedules.Length; i++)
        {
            CsGameData.Instance.StaminaRecoveryScheduleList.Add(new CsStaminaRecoverySchedule(gameData.staminaRecoverySchedules[i]));
        }

        // WPDBanWord[]	금지어 목록
        CsGameData.Instance.BanWordList.Clear();

        for (int i = 0; i < gameData.banWords.Length; i++)
        {
            CsGameData.Instance.BanWordList.Add(new CsBanWord(gameData.banWords[i]));
        }

		// WPDGuildContentAvailableReward[]	길드컨텐츠획득가능보상 목록
		for (int i = 0; i < gameData.guildContentAvailableRewards.Length; i++)
		{
			CsGuildContent csGuildContent = CsGameData.Instance.GetGuildContent(gameData.guildContentAvailableRewards[i].guildContentId);

			if (csGuildContent != null)
			{
				csGuildContent.GuildContentAvailableRewardList.Add(new CsGuildContentAvailableReward(gameData.guildContentAvailableRewards[i]));
			}
		}

		// WPDMenuContentOpenPreview[]	메뉴컨텐츠개방미리보기 목록
		CsGameData.Instance.MenuContentOpenPreviewList.Clear();

		for (int i = 0; i < gameData.menuContentOpenPreviews.Length; i++)
		{
			CsGameData.Instance.MenuContentOpenPreviewList.Add(new CsMenuContentOpenPreview(gameData.menuContentOpenPreviews[i]));
		}

		CsGameData.Instance.MenuContentOpenPreviewList.Sort((a, b) => { return a.MenuContent.RequiredMainQuestNo.CompareTo(b.MenuContent.RequiredMainQuestNo); });
		
		// WPDJobCommonSkill[]  직업공통스킬 목록
		CsGameData.Instance.JobCommonSkillList.Clear();

		for (int i = 0; i < gameData.jobCommonSkills.Length; i++)
		{
			CsGameData.Instance.JobCommonSkillList.Add(new CsJobCommonSkill(gameData.jobCommonSkills[i]));
		}

		//WPDNpcShop[]  NPC상점 목록
		CsGameData.Instance.NpcShopList.Clear();

		for (int i = 0; i < gameData.npcShops.Length; i++)
		{
			CsGameData.Instance.NpcShopList.Add(new CsNpcShop(gameData.npcShops[i]));
		}

		//WPDNpcShopCategory[] NPC상점카테고리 목록
		for (int i = 0; i < gameData.npcShopCategories.Length; i++)
		{
			CsNpcShop csNpcShop = CsGameData.Instance.GetNpcShop(gameData.npcShopCategories[i].shopId);

			if (csNpcShop != null)
			{
				csNpcShop.NpcShopCategoryList.Add(new CsNpcShopCategory(gameData.npcShopCategories[i]));
			}
		}

		//WPDNpcShopProduct[] NPC상점상품 목록
		for (int i = 0; i < gameData.npcShopProducts.Length; i++)
		{
			CsNpcShop csNpcShop = CsGameData.Instance.GetNpcShop(gameData.npcShopProducts[i].shopId);

			if (csNpcShop != null)
			{
				CsNpcShopCategory csNpcShopCategory = csNpcShop.GetNpcShopCategory(gameData.npcShopProducts[i].categoryId);

				if (csNpcShopCategory != null)
				{
					csNpcShopCategory.NpcShopProductList.Add(new CsNpcShopProduct(gameData.npcShopProducts[i]));
				}
			}
		}

		for (int i = 0; i < CsGameData.Instance.NpcShopList.Count; i++)
		{
			CsGameData.Instance.NpcShopList[i].NpcShopCategoryList.Sort();

			for (int j = 0; j < CsGameData.Instance.NpcShopList[i].NpcShopCategoryList.Count; j++)
			{
				CsGameData.Instance.NpcShopList[i].NpcShopCategoryList[j].NpcShopProductList.Sort();
			}
		}

		//WPDAbnormalStateRankSkillLevel[] 상태이상계급스킬레벨 목록
		for (int i = 0; i < gameData.abnormalStateRankSkillLevels.Length; i++)
		{
			CsAbnormalState csAbnormalState = CsGameData.Instance.GetAbnormalState(gameData.abnormalStateRankSkillLevels[i].abnormalStateId);

			if (csAbnormalState != null)
			{
				csAbnormalState.AbnormalStateRankSkillLevelList.Add(new CsAbnormalStateRankSkillLevel(gameData.abnormalStateRankSkillLevels[i]));
			}
		}

		//WPDRankActiveSkill[] 계급액티브스킬 목록
		CsGameData.Instance.RankActiveSkillList.Clear();

		for (int i = 0; i < gameData.rankActiveSkills.Length; i++)
		{
			CsGameData.Instance.RankActiveSkillList.Add(new CsRankActiveSkill(gameData.rankActiveSkills[i]));
		}

		CsGameData.Instance.RankActiveSkillList.Sort();

		//WPDRankActiveSkillLevel[] 계급액티브스킬레벨 목록
		for (int i = 0; i < gameData.rankActiveSkillLevels.Length; i++)
		{
			CsRankActiveSkill csRankActiveSkill = CsGameData.Instance.GetRankActiveSkill(gameData.rankActiveSkillLevels[i].skillId);

			if (csRankActiveSkill != null)
			{
				csRankActiveSkill.RankActiveSkillLevelList.Add(new CsRankActiveSkillLevel(gameData.rankActiveSkillLevels[i]));
			}
		}

		//WPDRankPassiveSkill[] 계급패시브스킬 목록
		CsGameData.Instance.RankPassiveSkillList.Clear();

		for (int i = 0; i < gameData.rankPassiveSkills.Length; i++)
		{
			CsGameData.Instance.RankPassiveSkillList.Add(new CsRankPassiveSkill(gameData.rankPassiveSkills[i]));
		}

		//WPDRankPassiveSkillAttr[] 계급패시브스킬속성 목록
		for (int i = 0; i < gameData.rankPassiveSkillAttrs.Length; i++)
		{
			CsRankPassiveSkill csRankPassiveSkill = CsGameData.Instance.GetRankPassiveSkill(gameData.rankPassiveSkillAttrs[i].skillId);

			if (csRankPassiveSkill != null)
			{
				csRankPassiveSkill.RankPassiveSkillAttrList.Add(new CsRankPassiveSkillAttr(gameData.rankPassiveSkillAttrs[i]));
			}
		}

		//WPDRankPassiveSkillLevel[] 계급패시브스킬레벨 목록
		for (int i = 0; i < gameData.rankPassiveSkillLevels.Length; i++)
		{
			CsRankPassiveSkill csRankPassiveSkill = CsGameData.Instance.GetRankPassiveSkill(gameData.rankPassiveSkillLevels[i].skillId);

			if (csRankPassiveSkill != null)
			{
				csRankPassiveSkill.RankPassiveSkillLevelList.Add(new CsRankPassiveSkillLevel(gameData.rankPassiveSkillLevels[i]));
			}
		}

		//WPDRankPassiveSkillAttrLevel[] 계급패시브스킬속성레벨 목록
		for (int i = 0; i < gameData.rankPassiveSkillAttrLevels.Length; i++)
		{
			CsRankPassiveSkill csRankPassiveSkill = CsGameData.Instance.GetRankPassiveSkill(gameData.rankPassiveSkillAttrLevels[i].skillId);

			if (csRankPassiveSkill != null)
			{
				CsRankPassiveSkillLevel csRankPassiveSkillLevel = csRankPassiveSkill.GetRankPassiveSkillLevel(gameData.rankPassiveSkillAttrLevels[i].level);

				if (csRankPassiveSkillLevel != null)
				{
					csRankPassiveSkillLevel.RankPassiveSkillAttrLevelList.Add(new CsRankPassiveSkillAttrLevel(gameData.rankPassiveSkillAttrLevels[i]));
				}
			}
		}

		//WPDRookieGift[] 신병선물 목록
		CsGameData.Instance.RookieGiftList.Clear();

		for (int i = 0; i < gameData.rookieGifts.Length; i++)
		{
			CsGameData.Instance.RookieGiftList.Add(new CsRookieGift(gameData.rookieGifts[i]));
		}

		//WPDRookieGiftReward[] 신병선물보상 목록
		for (int i = 0; i < gameData.rookieGiftRewards.Length; i++)
		{
			CsRookieGift csRookieGift = CsGameData.Instance.GetRookieGift(gameData.rookieGiftRewards[i].giftNo);

			if (csRookieGift != null)
			{
				csRookieGift.RookieGiftRewardList.Add(new CsRookieGiftReward(gameData.rookieGiftRewards[i]));
			}
		}

		//WPDOpenGiftReward[] 오픈선물보상 목록
		CsGameData.Instance.OpenGiftRewardList.Clear();

		for (int i = 0; i < gameData.openGiftRewards.Length; i++)
		{
			CsGameData.Instance.OpenGiftRewardList.Add(new CsOpenGiftReward(gameData.openGiftRewards[i]));
		}

		//WPDQuickMenu[] 퀵메뉴 목록
		CsGameData.Instance.QuickMenuList.Clear();

		for (int i = 0; i < gameData.quickMenus.Length; i++)
		{
			CsGameData.Instance.QuickMenuList.Add(new CsQuickMenu(gameData.quickMenus[i]));
		}

		//WPDDailyQuest 일일퀘스트
		CsGameData.Instance.DailyQuest = new CsDailyQuest(gameData.dailyQuest);

		//WPDDailyQuestReward[] 일일퀘스트보상 목록
		for (int i = 0; i < gameData.dailyQuestRewards.Length; i++)
		{
			CsGameData.Instance.DailyQuest.DailyQuestRewardList.Add(new CsDailyQuestReward(gameData.dailyQuestRewards[i]));
		}

		//WPDDailyQuestGrade[]  일일퀘스트등급 목록
		CsGameData.Instance.DailyQuestGradeList.Clear();

		for (int i = 0; i < gameData.dailyQuestGrades.Length; i++)
		{
			CsGameData.Instance.DailyQuestGradeList.Add(new CsDailyQuestGrade(gameData.dailyQuestGrades[i]));
		}
		
		//WPDDailyQuestMission[] 일일퀘스트미션 목록
		for (int i = 0; i < gameData.dailyQuestMissions.Length; i++)
		{
			CsGameData.Instance.DailyQuest.DailyQuestMissionList.Add(new CsDailyQuestMission(gameData.dailyQuestMissions[i]));
		}

		//WPDWisdomTemple 지혜의신전
		CsGameData.Instance.WisdomTemple = new CsWisdomTemple(gameData.wisdomTemple);

		//WPDWisdomTempleMonsterAttrFactor[] 지혜의신전몬스터속성계수 목록
		for (int i = 0; i < gameData.wisdomTempleMonsterAttrFactors.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleMonsterAttrFactorList.Add(new CsWisdomTempleMonsterAttrFactor(gameData.wisdomTempleMonsterAttrFactors[i]));
		}

		//WPDWisdomTempleColorMatchingObject[] 지혜의신전색맞추기오브젝트 목록
		for (int i = 0; i < gameData.wisdomTempleColorMatchingObjects.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleColorMatchingObjectList.Add(new CsWisdomTempleColorMatchingObject(gameData.wisdomTempleColorMatchingObjects[i]));
		}

		//WPDWisdomTempleArrangePosition[] 지혜의신전배치위치 목록
		for (int i = 0; i < gameData.wisdomTempleArrangePositions.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleArrangePositionList.Add(new CsWisdomTempleArrangePosition(gameData.wisdomTempleArrangePositions[i]));
		}

		//WPDWisdomTempleSweepReward[] 지혜의신전소탕보상 목록
		for (int i = 0; i < gameData.wisdomTempleSweepRewards.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleSweepRewardList.Add(new CsWisdomTempleSweepReward(gameData.wisdomTempleSweepRewards[i]));
		}
		
		//WPDWisdomTempleStep[] 지혜의신전단계 목록
		for (int i = 0; i < gameData.wisdomTempleSteps.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleStepList.Add(new CsWisdomTempleStep(gameData.wisdomTempleSteps[i]));
		}

		//WPDWisdomTempleQuizMonsterPosition[] 지혜의신전단계퀴즈위치 목록
		for (int i = 0; i < gameData.wisdomTempleQuizMonsterPositions.Length; i++)
		{
			CsWisdomTempleStep csWisdomTempleStep = CsGameData.Instance.WisdomTemple.GetWisdomTempleStep(gameData.wisdomTempleQuizMonsterPositions[i].stepNo);

			if (csWisdomTempleStep != null)
			{
				csWisdomTempleStep.WisdomTempleQuizMonsterPositionList.Add(new CsWisdomTempleQuizMonsterPosition(gameData.wisdomTempleQuizMonsterPositions[i]));
			}

		}
		
		//WPDWisdomTempleQuizPoolEntry[]  지혜의신전퀴즈풀항목 목록
		for (int i = 0; i < gameData.wisdomTempleQuizPoolEntries.Length; i++)
		{
			CsWisdomTempleStep csWisdomTempleStep = CsGameData.Instance.WisdomTemple.GetWisdomTempleStep(gameData.wisdomTempleQuizPoolEntries[i].stepNo);

			if (csWisdomTempleStep != null)
			{
				csWisdomTempleStep.WisdomTempleQuizPoolEntryList.Add(new CsWisdomTempleQuizPoolEntry(gameData.wisdomTempleQuizPoolEntries[i]));
			}
		}

		//WPDWisdomTemplePuzzle[] 지혜의신전퍼즐 목록
		for (int i = 0; i < gameData.wisdomTemplePuzzles.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTemplePuzzleList.Add(new CsWisdomTemplePuzzle(gameData.wisdomTemplePuzzles[i]));
		}

		//WPDWisdomTempleStepReward[] 지혜의신전단계보상 목록
		for (int i = 0; i < gameData.wisdomTempleStepRewards.Length; i++)
		{
			CsGameData.Instance.WisdomTemple.WisdomTempleStepRewardList.Add(new CsWisdomTempleStepReward(gameData.wisdomTempleStepRewards[i]));
		}

		//WPDWeeklyQuest 주간퀘스트
		CsGameData.Instance.WeeklyQuest = new CsWeeklyQuest(gameData.weeklyQuest);

		//WPDWeeklyQuestRoundReward[] 주간퀘스트라운드보상 목록
		for (int i = 0; i < gameData.weeklyQuestRoundRewards.Length; i++)
		{
			CsGameData.Instance.WeeklyQuest.WeeklyQuestRoundRewardList.Add(new CsWeeklyQuestRoundReward(gameData.weeklyQuestRoundRewards[i]));
		}

		//WPDWeeklyQuestMission[] 주간퀘스트미션 목록
		for (int i = 0; i < gameData.weeklyQuestMissions.Length; i++)
		{
			CsGameData.Instance.WeeklyQuest.WeeklyQuestMissionList.Add(new CsWeeklyQuestMission(gameData.weeklyQuestMissions[i]));
		}

		//WPDWeeklyQuestTenRoundReward[] 주간퀘스트10라운드보상 목록
		for (int i = 0; i < gameData.weeklyQuestTenRoundRewards.Length; i++)
		{
			CsGameData.Instance.WeeklyQuest.WeeklyQuestTenRoundRewardList.Add(new CsWeeklyQuestTenRoundReward(gameData.weeklyQuestTenRoundRewards[i]));
		}

		//WPDOpen7DayEventDay[] 오픈7일이벤트일차 목록
		CsGameData.Instance.Open7DayEventDayList.Clear();

		for (int i = 0; i < gameData.open7DayEventDaies.Length; i++)
		{
			CsGameData.Instance.Open7DayEventDayList.Add(new CsOpen7DayEventDay(gameData.open7DayEventDaies[i]));
		}

		//WPDOpen7DayEventMission[] 오픈7일이벤트미션 목록
		for (int i = 0; i < gameData.open7DayEventMissions.Length; i++)
		{
			CsOpen7DayEventDay csOpen7DayEventDay = CsGameData.Instance.GetOpen7DayEventDay(gameData.open7DayEventMissions[i].day);

			if (csOpen7DayEventDay != null)
			{
				csOpen7DayEventDay.Open7DayEventMissionList.Add(new CsOpen7DayEventMission(gameData.open7DayEventMissions[i]));
			}
		}

		//WPDOpen7DayEventMissionReward[] 오픈7일이벤트미션보상 목록
		for (int i = 0; i < gameData.open7DayEventMissionRewards.Length; i++)
		{
			CsOpen7DayEventMission csOpen7DayEventMission = CsGameData.Instance.GetOpen7DayEventMission(gameData.open7DayEventMissionRewards[i].missionId);

			if (csOpen7DayEventMission != null)
			{
				csOpen7DayEventMission.Open7DayEventMissionRewardList.Add(new CsOpen7DayEventMissionReward(gameData.open7DayEventMissionRewards[i]));
			}
		}

		//WPDOpen7DayEventProduct[] 오픈7일이벤트상품 목록
		for (int i = 0; i < gameData.open7DayEventProducts.Length; i++)
		{
			CsOpen7DayEventDay csOpen7DayEventDay = CsGameData.Instance.GetOpen7DayEventDay(gameData.open7DayEventProducts[i].day);

			if (csOpen7DayEventDay != null)
			{
				csOpen7DayEventDay.Open7DayEventProductList.Add(new CsOpen7DayEventProduct(gameData.open7DayEventProducts[i]));
			}
		}

		//WPDRetrieval[] 회수 목록
		CsGameData.Instance.RetrievalList.Clear();

		for (int i = 0; i < gameData.retrievals.Length; i++)
		{
			CsGameData.Instance.RetrievalList.Add(new CsRetrieval(gameData.retrievals[i]));
		}

		//WPDRetrievalReward[] 회수보상 목록
		for (int i = 0; i < gameData.retrievalRewards.Length; i++)
		{
			CsRetrieval csRetrieval = CsGameData.Instance.GetRetrieval(gameData.retrievalRewards[i].retrievalId);

			if (csRetrieval != null)
			{
				csRetrieval.RetrievalRewardList.Add(new CsRetrievalReward(gameData.retrievalRewards[i]));
			}
		}

		//WPDRuinsReclaim 유적탈환
		CsGameData.Instance.RuinsReclaim = new CsRuinsReclaim(gameData.ruinsReclaim);

		//WPDRuinsReclaimMonsterAttrFactor[] 유적탈환몬스터속성계수 목록
		for (int i = 0; i < gameData.ruinsReclaimMonsterAttrFactors.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimMonsterAttrFactorList.Add(new CsRuinsReclaimMonsterAttrFactor(gameData.ruinsReclaimMonsterAttrFactors[i]));
		}

		//WPDRuinsReclaimAvailableReward[]  유적탈환획득가능보상 목록
		for (int i = 0; i < gameData.ruinsReclaimAvailableRewards.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimAvailableRewardList.Add(new CsRuinsReclaimAvailableReward(gameData.ruinsReclaimAvailableRewards[i]));
		}

		//WPDRuinsReclaimRevivalPoint[]  유적탈환부활포인트 목록
		for (int i = 0; i < gameData.ruinsReclaimRevivalPoints.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimRevivalPointList.Add(new CsRuinsReclaimRevivalPoint(gameData.ruinsReclaimRevivalPoints[i]));
		}

		//WPDRuinsReclaimObstacle[]  유적탈환장애물 목록
		for (int i = 0; i < gameData.ruinsReclaimObstacles.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimObstacleList.Add(new CsRuinsReclaimObstacle(gameData.ruinsReclaimObstacles[i]));
		}

		//WPDRuinsReclaimTrap[]  유적탈환함정 목록
		for (int i = 0; i < gameData.ruinsReclaimTraps.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimTrapList.Add(new CsRuinsReclaimTrap(gameData.ruinsReclaimTraps[i]));
		}

		//WPDRuinsReclaimPortal[] 유적탈환포털 목록
		for (int i = 0; i < gameData.ruinsReclaimPortals.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimPortalList.Add(new CsRuinsReclaimPortal(gameData.ruinsReclaimPortals[i]));
		}

		//WPDRuinsReclaimOpenSchedule[]  유적탈환오픈스케줄 목록
		for (int i = 0; i < gameData.ruinsReclaimOpenSchedules.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimOpenScheduleList.Add(new CsRuinsReclaimOpenSchedule(gameData.ruinsReclaimOpenSchedules[i]));
		}

		//WPDRuinsReclaimStep[]  유적탈환단계 목록
		for (int i = 0; i < gameData.ruinsReclaimSteps.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimStepList.Add(new CsRuinsReclaimStep(gameData.ruinsReclaimSteps[i]));
		}

		//WPDRuinsReclaimObjectArrange[]  유적탈환오브젝트배치 목록
		for (int i = 0; i < gameData.ruinsReclaimObjectArranges.Length; i++)
		{
			CsRuinsReclaimStep csRuinsReclaimStep = CsGameData.Instance.RuinsReclaim.GetRuinsReclaimStep(gameData.ruinsReclaimObjectArranges[i].stepNo);

			if (csRuinsReclaimStep != null)
			{
				csRuinsReclaimStep.RuinsReclaimObjectArrangeList.Add(new CsRuinsReclaimObjectArrange(gameData.ruinsReclaimObjectArranges[i]));
			}
		}

		//WPDRuinsReclaimStepReward[] 유적탈환단계보상 목록
		for (int i = 0; i < gameData.ruinsReclaimStepRewards.Length; i++)
		{
			CsRuinsReclaimStep csRuinsReclaimStep = CsGameData.Instance.RuinsReclaim.GetRuinsReclaimStep(gameData.ruinsReclaimStepRewards[i].stepNo);

			if (csRuinsReclaimStep != null)
			{
				csRuinsReclaimStep.RuinsReclaimStepRewardList.Add(new CsRuinsReclaimStepReward(gameData.ruinsReclaimStepRewards[i]));
			}
		}

		//WPDRuinsReclaimStepWave[]   유적탈환단계웨이브 목록
		for (int i = 0; i < gameData.ruinsReclaimStepWaves.Length; i++)
		{
			CsRuinsReclaimStep csRuinsReclaimStep = CsGameData.Instance.RuinsReclaim.GetRuinsReclaimStep(gameData.ruinsReclaimStepWaves[i].stepNo);

			if (csRuinsReclaimStep != null)
			{
				csRuinsReclaimStep.RuinsReclaimStepWaveList.Add(new CsRuinsReclaimStepWave(gameData.ruinsReclaimStepWaves[i]));
			}
		}

		//WPDRuinsReclaimStepWaveSkill[] 유적탈환단계웨이브스킬 목록
		for (int i = 0; i < gameData.ruinsReclaimStepWaveSkills.Length; i++)
		{
			CsRuinsReclaimStep csRuinsReclaimStep = CsGameData.Instance.RuinsReclaim.GetRuinsReclaimStep(gameData.ruinsReclaimStepWaveSkills[i].stepNo);

			if (csRuinsReclaimStep != null)
			{
				csRuinsReclaimStep.RuinsReclaimStepWaveSkillList.Add(new CsRuinsReclaimStepWaveSkill(gameData.ruinsReclaimStepWaveSkills[i]));
			}
		}

		// WPDRuinsReclaimRandomRewardPoolEntry[] 유적탈환랜덤보상풀항목 목록
		for (int i = 0;i < gameData.ruinsReclaimRandomRewardPoolEntries.Length; i++)
		{
			CsGameData.Instance.RuinsReclaim.RuinsReclaimRandomRewardPoolEntryList.Add(new CsRuinsReclaimRandomRewardPoolEntry(gameData.ruinsReclaimRandomRewardPoolEntries[i]));
		}

		//WPDTaskConsignment[] 할일위탁 목록
		CsGameData.Instance.TaskConsignmentList.Clear();

		for (int i = 0; i < gameData.taskConsignments.Length; i++)
		{
			CsGameData.Instance.TaskConsignmentList.Add(new CsTaskConsignment(gameData.taskConsignments[i]));
		}

		//WPDTaskConsignmentAvailableReward[] 할일위탁획득가능보상 목록
		for (int i = 0; i < gameData.taskConsignmentAvailableRewards.Length; i++)
		{
			CsTaskConsignment csTaskConsignment = CsGameData.Instance.GetTaskConsignment(gameData.taskConsignmentAvailableRewards[i].consignmentId);

			if (csTaskConsignment != null)
			{
				csTaskConsignment.TaskConsignmentAvailableRewardList.Add(new CsTaskConsignmentAvailableReward(gameData.taskConsignmentAvailableRewards[i]));
			}
		}

		//WPDTaskConsignmentExpReward[] 할일위탁경험치보상 목록
		for (int i = 0; i < gameData.taskConsignmentExpRewards.Length; i++)
		{
			CsTaskConsignment csTaskConsignment = CsGameData.Instance.GetTaskConsignment(gameData.taskConsignmentExpRewards[i].consignmentId);

			if (csTaskConsignment != null)
			{
				csTaskConsignment.TaskConsignmentExpRewardList.Add(new CsTaskConsignmentExpReward(gameData.taskConsignmentExpRewards[i]));
			}
		}

		//WPDInfiniteWar 무한대전
		CsGameData.Instance.InfiniteWar = new CsInfiniteWar(gameData.infiniteWar);

		//WPDInfiniteWarBuffBox[] 무한대전버프상자 목록
		for (int i = 0; i < gameData.infiniteWarBuffBoxs.Length; i++)
		{
			CsGameData.Instance.InfiniteWar.InfiniteWarBuffBoxList.Add(new CsInfiniteWarBuffBox(gameData.infiniteWarBuffBoxs[i]));
		}

		//WPDInfiniteWarMonsterAttrFactor[] 무한대전몬스터속성계수 목록
		for (int i = 0; i < gameData.infiniteWarMonsterAttrFactors.Length; i++)
		{
			CsGameData.Instance.InfiniteWar.InfiniteWarMonsterAttrFactorList.Add(new CsInfiniteWarMonsterAttrFactor(gameData.infiniteWarMonsterAttrFactors[i]));
		}

		//WPDInfiniteWarOpenSchedule[] 무한대전오픈스케줄 목록
		for (int i = 0; i < gameData.infiniteWarOpenSchedules.Length; i++)
		{
			CsGameData.Instance.InfiniteWar.InfiniteWarOpenScheduleList.Add(new CsInfiniteWarOpenSchedule(gameData.infiniteWarOpenSchedules[i]));
		}

		//WPDInfiniteWarAvailableReward[] 무한대전획득가능보상 목록
		for (int i = 0; i < gameData.infiniteWarAvailableRewards.Length; i++)
		{
			CsGameData.Instance.InfiniteWar.InfiniteWarAvailableRewardList.Add(new CsInfiniteWarAvailableReward(gameData.infiniteWarAvailableRewards[i]));
		}

		//WPDTrueHeroQuest  진정한영웅퀘스트
		CsGameData.Instance.TrueHeroQuest = new CsTrueHeroQuest(gameData.trueHeroQuest);

		//WPDTrueHeroQuestStep[] 진정한영웅퀘스트단계 목록
		for (int i = 0; i < gameData.trueHeroQuestSteps.Length; i++)
		{
			CsGameData.Instance.TrueHeroQuest.TrueHeroQuestStepList.Add(new CsTrueHeroQuestStep(gameData.trueHeroQuestSteps[i]));
		}

		//WPDTrueHeroQuestReward[] 진정한영웅퀘스트보상 목록
		for (int i = 0; i < gameData.trueHeroQuestRewards.Length; i++)
		{
			CsGameData.Instance.TrueHeroQuest.TrueHeroQuestRewardList.Add(new CsTrueHeroQuestReward(gameData.trueHeroQuestRewards[i]));
		}

		//WPDLimitationGift 한정선물
		CsGameData.Instance.LimitationGift = new CsLimitationGift(gameData.limitationGift);

		//WPDLimitationGiftRewardDayOfWeek[]  한정선물보상요일 목록
		for (int i = 0; i < gameData.limitationGiftRewardDayOfWeeks.Length; i++)
		{
			CsGameData.Instance.LimitationGift.LimitationGiftRewardDayOfWeekList.Add(new CsLimitationGiftRewardDayOfWeek(gameData.limitationGiftRewardDayOfWeeks[i]));
		}
		
		//WPDLimitationGiftRewardSchedule[]   한정선물보상스케줄 목록
		for (int i = 0; i < gameData.limitationGiftRewardSchedules.Length; i++)
		{
			CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Add(new CsLimitationGiftRewardSchedule(gameData.limitationGiftRewardSchedules[i]));
		}

		//WPDLimitationGiftAvailableReward[]  한정선물획득가능보상 목록
		for (int i = 0; i < gameData.limitationGiftAvailableRewards.Length; i++)
		{
			CsLimitationGiftRewardSchedule csLimitationGiftRewardSchedule = CsGameData.Instance.LimitationGift.GetLimitationGiftRewardSchedule(gameData.limitationGiftAvailableRewards[i].scheduleId);

			if (csLimitationGiftRewardSchedule != null)
			{
				csLimitationGiftRewardSchedule.LimitationGiftAvailableRewardList.Add(new CsLimitationGiftAvailableReward(gameData.limitationGiftAvailableRewards[i]));
			}
		}

		for (int i = 0; i < CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList.Count; i++)
		{
			CsGameData.Instance.LimitationGift.LimitationGiftRewardScheduleList[i].LimitationGiftAvailableRewardList.Sort();
		}

		//WPDWeekendReward  주말보상
		CsGameData.Instance.WeekendReward = new CsWeekendReward(gameData.weekendReward);

		//WPDFieldBossEvent 필드보스이벤트
		CsGameData.Instance.FieldBossEvent = new CsFieldBossEvent(gameData.fieldBossEvent);

		//WPDFieldBossEventSchedule[] 필드보스이벤트스케줄 목록
		for (int i = 0; i < gameData.fieldBossEventSchedules.Length; i++)
		{
			CsGameData.Instance.FieldBossEvent.FieldBossEventScheduleList.Add(new CsFieldBossEventSchedule(gameData.fieldBossEventSchedules[i]));
		}

		//WPDFieldBossEventAvailableReward[] 필드보스이벤트획득가능보상 목록
		for (int i = 0; i < gameData.fieldBossEventAvailableRewards.Length; i++)
		{
			CsGameData.Instance.FieldBossEvent.FieldBossEventAvailableRewardList.Add(new CsFieldBossEventAvailableReward(gameData.fieldBossEventAvailableRewards[i]));
		}
		
		//WPDFieldBoss[] 필드보스 목록
		for (int i = 0; i < gameData.fieldBosss.Length; i++)
		{
			CsGameData.Instance.FieldBossEvent.FieldBossList.Add(new CsFieldBoss(gameData.fieldBosss[i]));
		}

		CsGameData.Instance.FieldBossEvent.FieldBossList.Sort();

		//WPDWarehouseSlotExtendRecipe[]	창고슬롯확장레시피 목록
		CsGameData.Instance.WarehouseSlotExtendRecipeList.Clear();

		for (int i = 0; i < gameData.warehouseSlotExtendRecipes.Length; i++)
		{
			CsGameData.Instance.WarehouseSlotExtendRecipeList.Add(new CsWarehouseSlotExtendRecipe(gameData.warehouseSlotExtendRecipes[i]));
		}

		//WPDFearAltar 공포의제단
		CsGameData.Instance.FearAltar = new CsFearAltar(gameData.fearAltar);

		//WPDFearAltarReward[]  공포의제단보상 목록
		for (int i = 0; i < gameData.fearAltarRewards.Length; i++)
		{
			CsGameData.Instance.FearAltar.FearAltarRewardList.Add(new CsFearAltarReward(gameData.fearAltarRewards[i]));
		}

		//WPDFearAltarHalidomCollectionReward[] 공포의제단성물수집보상 목록
		for (int i = 0; i < gameData.fearAltarHalidomCollectionRewards.Length; i++)
		{
			CsGameData.Instance.FearAltar.FearAltarHalidomCollectionRewardList.Add(new CsFearAltarHalidomCollectionReward(gameData.fearAltarHalidomCollectionRewards[i]));
		}

		//WPDFearAltarHalidomElemental[] 공포의제단성물원소 목록
		CsGameData.Instance.FearAltarHalidomElementalList.Clear();

		for (int i = 0; i < gameData.fearAltarHalidomElementals.Length; i++)
		{
			CsGameData.Instance.FearAltarHalidomElementalList.Add(new CsFearAltarHalidomElemental(gameData.fearAltarHalidomElementals[i]));
		}

		//WPDFearAltarHalidomLevel[]  공포의제단성물레벨 목록
		CsGameData.Instance.FearAltarHalidomLevelList.Clear();

		for (int i = 0; i < gameData.fearAltarHalidomLevels.Length; i++)
		{
			CsGameData.Instance.FearAltarHalidomLevelList.Add(new CsFearAltarHalidomLevel(gameData.fearAltarHalidomLevels[i]));
		}

		//WPDFearAltarHalidom[]  공포의제단성물 목록
		for (int i = 0; i < gameData.fearAltarHalidoms.Length; i++)
		{
			CsGameData.Instance.FearAltar.FearAltarHalidomList.Add(new CsFearAltarHalidom(gameData.fearAltarHalidoms[i]));
		}

		//WPDFearAltarStage[] 공포의제단스테이지 목록
		for (int i = 0; i < gameData.fearAltarStages.Length; i++)
		{
			CsGameData.Instance.FearAltar.FearAltarStageList.Add(new CsFearAltarStage(gameData.fearAltarStages[i]));
		}

		//WPDFearAltarStageWave[] 공포의제단스테이지웨이브 목록
		for (int i = 0; i < gameData.fearAltarStageWaves.Length; i++)
		{
			CsFearAltarStage csFearAltarStage = CsGameData.Instance.FearAltar.GetFearAltarStage(gameData.fearAltarStageWaves[i].stageId);

			if (csFearAltarStage != null)
			{
				csFearAltarStage.FearAltarStageWaveList.Add(new CsFearAltarStageWave(gameData.fearAltarStageWaves[i]));
			}
		}

		//WPDDiaShopCategory[]  다이아상점카테고리 목록
		CsGameData.Instance.DiaShopCategoryList.Clear();

		for (int i = 0; i < gameData.diaShopCategories.Length; i++)
		{
			CsGameData.Instance.DiaShopCategoryList.Add(new CsDiaShopCategory(gameData.diaShopCategories[i]));
		}

		CsGameData.Instance.DiaShopCategoryList.Sort();

		//WPDDiaShopProduct[] 다이아상점상품 목록
		CsGameData.Instance.DiaShopProductList.Clear();

		for (int i = 0; i < gameData.diaShopProducts.Length; i++)
		{
			CsGameData.Instance.DiaShopProductList.Add(new CsDiaShopProduct(gameData.diaShopProducts[i]));
		}

		//WPDWingMemoryPieceSlot[] 날개기억조각슬롯 목록
		for (int i = 0; i < gameData.wingMemoryPieceSlots.Length; i++)
		{
			CsWing csWing = CsGameData.Instance.GetWing(gameData.wingMemoryPieceSlots[i].wingId);

			if (csWing != null)
			{
				csWing.WingMemoryPieceSlotList.Add(new CsWingMemoryPieceSlot(gameData.wingMemoryPieceSlots[i]));
			}
		}

		//WPDWingMemoryPieceStep[] 날개기억조각단계 목록
		for (int i = 0; i < gameData.wingMemoryPieceSteps.Length; i++)
		{
			CsWing csWing = CsGameData.Instance.GetWing(gameData.wingMemoryPieceSteps[i].wingId);

			if (csWing != null)
			{
				csWing.WingMemoryPieceStepList.Add(new CsWingMemoryPieceStep(gameData.wingMemoryPieceSteps[i]));
			}
		}

		//WPDWingMemoryPieceSlotStep[] 날개기억조각슬롯단계 목록
		for (int i = 0; i < gameData.wingMemoryPieceSlotSteps.Length; i++)
		{
			CsWing csWing = CsGameData.Instance.GetWing(gameData.wingMemoryPieceSlotSteps[i].wingId);

			if (csWing != null)
			{
				CsWingMemoryPieceSlot csWingMemoryPieceSlot = csWing.GetWingMemoryPieceSlot(gameData.wingMemoryPieceSlotSteps[i].slotIndex);

				if (csWingMemoryPieceSlot != null)
				{
					csWingMemoryPieceSlot.WingMemoryPieceSlotStepList.Add(new CsWingMemoryPieceSlotStep(gameData.wingMemoryPieceSlotSteps[i]));
				}
			}
		}

		//WPDWingMemoryPieceType[] 날개기억조각타입 목록
		CsGameData.Instance.WingMemoryPieceTypeList.Clear();

		for (int i = 0; i < gameData.wingMemoryPieceTypes.Length; i++)
		{
			CsGameData.Instance.WingMemoryPieceTypeList.Add(new CsWingMemoryPieceType(gameData.wingMemoryPieceTypes[i]));
		}

		//WPDSubQuest[] 서브퀘스트 목록
		CsGameData.Instance.SubQuestList.Clear();

		for (int i = 0; i < gameData.subQuests.Length; i++)
		{
			CsGameData.Instance.SubQuestList.Add(new CsSubQuest(gameData.subQuests[i]));
		}

		//WPDSubQuestReward[] 서브퀘스트보상 목록
		for (int i = 0; i < gameData.subQuestRewards.Length; i++)
		{
			CsSubQuest csSubQuest = CsGameData.Instance.GetSubQuest(gameData.subQuestRewards[i].questId);

			if (csSubQuest != null)
			{
				csSubQuest.SubQuestRewardList.Add(new CsSubQuestReward(gameData.subQuestRewards[i]));
			}
		}

		//WPDWarMemory 전쟁의기억
		CsGameData.Instance.WarMemory = new CsWarMemory(gameData.warMemory);

		//WPDWarMemoryMonsterAttrFactor[] 전쟁의기억몬스터속성계수 목록
		for (int i = 0; i < gameData.warMemoryMonsterAttrFactors.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryMonsterAttrFactorList.Add(new CsWarMemoryMonsterAttrFactor(gameData.warMemoryMonsterAttrFactors[i]));
		}

		//WPDWarMemoryStartPosition[] 전쟁의기억시작지점 목록
		for (int i = 0; i < gameData.warMemoryStartPositions.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryStartPositionList.Add(new CsWarMemoryStartPosition(gameData.warMemoryStartPositions[i]));
		}

		//WPDWarMemorySchedule[] 전쟁의기억스케줄 목록
		for (int i = 0; i < gameData.warMemorySchedules.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryScheduleList.Add(new CsWarMemorySchedule(gameData.warMemorySchedules[i]));
		}

		//WPDWarMemoryAvailableReward[] 전쟁의기억획득가능보상 목록
		for (int i = 0; i < gameData.warMemoryAvailableRewards.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryAvailableRewardList.Add(new CsWarMemoryAvailableReward(gameData.warMemoryAvailableRewards[i]));
		}

		//WPDWarMemoryReward[]  전쟁의기억보상 목록
		for (int i = 0; i < gameData.warMemoryRewards.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryRewardList.Add(new CsWarMemoryReward(gameData.warMemoryRewards[i]));
		}

		//WPDWarMemoryRankingReward[] 전쟁의기억랭킹보상 목록
		for (int i = 0; i < gameData.warMemoryRankingRewards.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryRankingRewardList.Add(new CsWarMemoryRankingReward(gameData.warMemoryRankingRewards[i]));
		}

		//WPDWarMemoryWave[] 전쟁의기억웨이브 목록
		for (int i = 0; i < gameData.warMemoryWaves.Length; i++)
		{
			CsGameData.Instance.WarMemory.WarMemoryWaveList.Add(new CsWarMemoryWave(gameData.warMemoryWaves[i]));
		}

		//WPDWarMemoryTransformationObject[] 전쟁의기억변신오브젝트 목록
		for (int i = 0; i < gameData.warMemoryTransformationObjects.Length; i++)
		{
			CsWarMemoryWave csWarMemoryWave = CsGameData.Instance.WarMemory.GetWarMemoryWave(gameData.warMemoryTransformationObjects[i].waveNo);

			if (csWarMemoryWave != null)
			{
				csWarMemoryWave.WarMemoryTransformationObjectList.Add(new CsWarMemoryTransformationObject(gameData.warMemoryTransformationObjects[i]));
			}
		}

		//WPDOrdealQuest[] 시련퀘스트 목록
		CsGameData.Instance.OrdealQuestList.Clear();

		for (int i = 0; i < gameData.ordealQuests.Length; i++)
		{
			CsGameData.Instance.OrdealQuestList.Add(new CsOrdealQuest(gameData.ordealQuests[i]));
		}

		//WPDOrdealQuestMission[] 시련퀘스트미션 목록
		for (int i = 0; i < gameData.ordealQuestMissions.Length; i++)
		{
			CsOrdealQuest csOrdealQuest = CsGameData.Instance.GetOrdealQuest(gameData.ordealQuestMissions[i].questNo);

			if (csOrdealQuest != null)
			{
				csOrdealQuest.OrdealQuestMissionList.Add(new CsOrdealQuestMission(gameData.ordealQuestMissions[i]));
			}
		}

		//WPDOsirisRoom 오시리스의방
		CsGameData.Instance.OsirisRoom = new CsOsirisRoom(gameData.osirisRoom);

		//WPDOsirisRoomAvailableReward[] 오시리스의방획득가능보상 목록
		for (int i = 0; i < gameData.osirisRoomAvailableRewards.Length; i++)
		{
			CsGameData.Instance.OsirisRoom.OsirisRoomAvailableRewardList.Add(new CsOsirisRoomAvailableReward(gameData.osirisRoomAvailableRewards[i]));
		}

		//WPDOsirisRoomDifficulty[]  오시리스의방난이도 목록
		for (int i = 0; i < gameData.osirisRoomDifficulties.Length; i++)
		{
			CsGameData.Instance.OsirisRoom.OsirisRoomDifficultyList.Add(new CsOsirisRoomDifficulty(gameData.osirisRoomDifficulties[i]));
		}

		//WPDOsirisRoomDifficultyWave[] 오시리스의방난이도웨이브 목록
		for (int i = 0; i < gameData.osirisRoomDifficultyWaves.Length; i++)
		{
			CsOsirisRoomDifficulty csOsirisRoomDifficulty = CsGameData.Instance.OsirisRoom.GetOsirisRoomDifficulty(gameData.osirisRoomDifficultyWaves[i].difficulty);

			if (csOsirisRoomDifficulty != null)
			{
				csOsirisRoomDifficulty.OsirisRoomDifficultyWaveList.Add(new CsOsirisRoomDifficultyWave(gameData.osirisRoomDifficultyWaves[i]));
			}
		}

		//WPDMoneyBuff[] 재화버프 목록
		CsGameData.Instance.MoneyBuffList.Clear();

		for (int i = 0; i < gameData.moneyBuffs.Length; i++)
		{
			CsGameData.Instance.MoneyBuffList.Add(new CsMoneyBuff(gameData.moneyBuffs[i]));
		}

		//WPDMoneyBuffAttr[] 재화버프속성 목록
		for (int i = 0; i < gameData.moneyBuffAttrs.Length; i++)
		{
			CsMoneyBuff csMoneyBuff = CsGameData.Instance.GetMoneyBuff(gameData.moneyBuffAttrs[i].buffId);

			if (csMoneyBuff != null)
			{
				csMoneyBuff.MoneyBuffAttrList.Add(new CsMoneyBuffAttr(gameData.moneyBuffAttrs[i]));
			}
		}

		//WPDBiography[] 전기 목록
		CsGameData.Instance.BiographyList.Clear();

		for (int i = 0; i < gameData.biographies.Length; i++)
		{
			CsGameData.Instance.BiographyList.Add(new CsBiography(gameData.biographies[i]));
		}

		CsGameData.Instance.BiographyList.Sort();

		//WPDBiographyReward[] 전기보상 목록
		for (int i = 0; i < gameData.biographyRewards.Length; i++)
		{
			CsBiography csBiography = CsGameData.Instance.GetBiography(gameData.biographyRewards[i].biographyId);

			if (csBiography != null)
			{
				csBiography.BiographyRewardList.Add(new CsBiographyReward(gameData.biographyRewards[i]));
			}
		}

		//WPDBiographyQuest[] 전기퀘스트 목록
		for (int i = 0; i < gameData.biographyQuests.Length; i++)
		{
			CsBiography csBiography = CsGameData.Instance.GetBiography(gameData.biographyQuests[i].biographyId);

			if (csBiography != null)
			{
				csBiography.BiographyQuestList.Add(new CsBiographyQuest(gameData.biographyQuests[i]));
			}
		}

		//WPDBiographyQuestStartDialogue[] 전기퀘스트시작대화 목록
		for (int i = 0; i < gameData.biographyQuestStartDialogues.Length; i++)
		{
			CsBiography csBiography = CsGameData.Instance.GetBiography(gameData.biographyQuestStartDialogues[i].biographyId);

			if (csBiography != null)
			{
				CsBiographyQuest csBiographyQuest = csBiography.GetBiographyQuest(gameData.biographyQuestStartDialogues[i].questNo);

				if (csBiographyQuest != null)
				{
					csBiographyQuest.BiographyQuestStartDialogueList.Add(new CsBiographyQuestStartDialogue(gameData.biographyQuestStartDialogues[i]));
				}
			}
		}

		//WPDBiographyQuestDungeon[] 전기퀘스트던전 목록
		CsGameData.Instance.BiographyQuestDungeonList.Clear();

		for (int i = 0; i < gameData.biographyQuestDungeons.Length; i++)
		{
			CsGameData.Instance.BiographyQuestDungeonList.Add(new CsBiographyQuestDungeon(gameData.biographyQuestDungeons[i]));
		}

		//WPDBiographyQuestDungeonWave[] 전기퀘스트던전웨이브 목록
		for (int i = 0; i < gameData.biographyQuestDungeonWaves.Length; i++)
		{
			CsBiographyQuestDungeon csBiographyQuestDungeon = CsGameData.Instance.GetBiographyQuestDungeon(gameData.biographyQuestDungeonWaves[i].dungeonId);

			if (csBiographyQuestDungeon != null)
			{
				csBiographyQuestDungeon.BiographyQuestDungeonWaveList.Add(new CsBiographyQuestDungeonWave(gameData.biographyQuestDungeonWaves[i]));
			}
		}

		//WPDBlessing[] Y   축복 목록
		CsGameData.Instance.BlessingList.Clear();

		for (int i = 0; i < gameData.blessings.Length; i++)
		{
			CsGameData.Instance.BlessingList.Add(new CsBlessing(gameData.blessings[i]));
		}

		//WPDBlessingTargetLevel[] Y   축복대상레벨 목록
		CsGameData.Instance.BlessingTargetLevelList.Clear();

		for (int i = 0; i < gameData.blessingTargetLevels.Length; i++)
		{
			CsGameData.Instance.BlessingTargetLevelList.Add(new CsBlessingTargetLevel(gameData.blessingTargetLevels[i]));
		}

		//WPDProspectQuestOwnerReward[] Y   유망자퀘스트소유자보상 목록
		for (int i = 0; i < gameData.prospectQuestOwnerRewards.Length; i++)
		{
			CsBlessingTargetLevel csBlessingTargetLevel = CsGameData.Instance.GetBlessingTargetLevel(gameData.prospectQuestOwnerRewards[i].targetLevelId);

			if (csBlessingTargetLevel != null)
			{
				csBlessingTargetLevel.ProspectQuestOwnerRewardList.Add(new CsProspectQuestOwnerReward(gameData.prospectQuestOwnerRewards[i]));
			}
		}

		//WPDProspectQuestTargetReward[] Y   유망자퀘스트대상보상 목록
		for (int i = 0; i < gameData.prospectQuestTargetRewards.Length; i++)
		{
			CsBlessingTargetLevel csBlessingTargetLevel = CsGameData.Instance.GetBlessingTargetLevel(gameData.prospectQuestTargetRewards[i].targetLevelId);

			if (csBlessingTargetLevel != null)
			{
				csBlessingTargetLevel.ProspectQuestTargetRewardList.Add(new CsProspectQuestTargetReward(gameData.prospectQuestTargetRewards[i]));
			}
		}

		//WPDItemLuckyShop 아이템행운상점
		CsGameData.Instance.ItemLuckyShop = new CsItemLuckyShop(gameData.itemLuckyShop);

		//WPDCreatureCardLuckyShop 크리처카드행운상점
		CsGameData.Instance.CreatureCardLuckyShop = new CsCreatureCardLuckyShop(gameData.creatureCardLuckyShop);

		//WPDDragonNest 용의둥지
		CsGameData.Instance.DragonNest = new CsDragonNest(gameData.dragonNest);

		//WPDDragonNestAvailableReward[]  용의둥지획득가능보상 목록
		for (int i = 0; i < gameData.dragonNestAvailableRewards.Length; i++)
		{
			CsGameData.Instance.DragonNest.DragonNestAvailableRewardList.Add(new CsDragonNestAvailableReward(gameData.dragonNestAvailableRewards[i]));
		}

		//WPDDragonNestObstacle[] 용의둥지장애물 목록
		for (int i = 0; i < gameData.dragonNestObstacles.Length; i++)
		{
			CsGameData.Instance.DragonNest.DragonNestObstacleList.Add(new CsDragonNestObstacle(gameData.dragonNestObstacles[i]));
		}

		//WPDDragonNestTrap[] 용의둥지함정 목록
		for (int i = 0; i < gameData.dragonNestTraps.Length; i++)
		{
			CsGameData.Instance.DragonNest.DragonNestTrapList.Add(new CsDragonNestTrap(gameData.dragonNestTraps[i]));
		}

		//WPDDragonNestStep[] 용의둥지단계 목록
		for (int i = 0; i < gameData.dragonNestSteps.Length; i++)
		{
			CsGameData.Instance.DragonNest.DragonNestStepList.Add(new CsDragonNestStep(gameData.dragonNestSteps[i]));
		}

		//WPDDragonNestStepReward[] 용의둥지단계보상 목록
		for (int i = 0; i < gameData.dragonNestStepRewards.Length; i++)
		{
			CsDragonNestStep csDragonNestStep = CsGameData.Instance.DragonNest.GetDragonNestStep(gameData.dragonNestStepRewards[i].stepNo);

			if (csDragonNestStep != null)
			{
				csDragonNestStep.DragonNestStepRewardList.Add(new CsDragonNestStepReward(gameData.dragonNestStepRewards[i]));
			}
		}

		//WPDCreatureCharacter[] 크리처캐릭터 목록
		CsGameData.Instance.CreatureCharacterList.Clear();

		for (int i = 0; i < gameData.creatureCharacters.Length; i++)
		{
			CsGameData.Instance.CreatureCharacterList.Add(new CsCreatureCharacter(gameData.creatureCharacters[i]));
		}

		//WPDCreatureGrade[] 크리처등급 목록
		CsGameData.Instance.CreatureGradeList.Clear();

		for (int i = 0; i < gameData.creatureGrades.Length; i++)
		{
			CsGameData.Instance.CreatureGradeList.Add(new CsCreatureGrade(gameData.creatureGrades[i]));
		}

		//WPDCreature[] Y   크리처 목록
		CsGameData.Instance.CreatureList.Clear();

		for (int i = 0; i < gameData.creatures.Length; i++)
		{
			CsGameData.Instance.CreatureList.Add(new CsCreature(gameData.creatures[i]));
		}

		//WPDCreatureSkillGrade[] Y   크리처스킬등급 목록
		CsGameData.Instance.CreatureSkillGradeList.Clear();

		for (int i = 0; i < gameData.creatureSkillGrades.Length; i++)
		{
			CsGameData.Instance.CreatureSkillGradeList.Add(new CsCreatureSkillGrade(gameData.creatureSkillGrades[i]));
		}

		//WPDCreatureSkill[] Y   크리처스킬 목록
		CsGameData.Instance.CreatureSkillList.Clear();

		for (int i = 0; i < gameData.creatureSkills.Length; i++)
		{
			CsGameData.Instance.CreatureSkillList.Add(new CsCreatureSkill(gameData.creatureSkills[i]));
		}

		//WPDCreatureSkillAttr[] Y   크리처스킬속성 목록
		for (int i = 0; i < gameData.creatureSkillAttrs.Length; i++)
		{
			CsCreatureSkill csCreatureSkill = CsGameData.Instance.GetCreatureSkill(gameData.creatureSkillAttrs[i].skillId);

			if (csCreatureSkill != null)
			{
				csCreatureSkill.CreatureSkillAttrList.Add(new CsCreatureSkillAttr(gameData.creatureSkillAttrs[i]));
			}
		}

		//WPDCreatureBaseAttr[] Y   크리처기본속성 목록
		CsGameData.Instance.CreatureBaseAttrList.Clear();

		for (int i = 0; i < gameData.creatureBaseAttrs.Length; i++)
		{
			CsGameData.Instance.CreatureBaseAttrList.Add(new CsCreatureBaseAttr(gameData.creatureBaseAttrs[i]));
		}

		//WPDCreatureLevel[] Y   크리처레벨 목록
		CsGameData.Instance.CreatureLevelList.Clear();

		for (int i = 0; i < gameData.creatureLevels.Length; i++)
		{
			CsGameData.Instance.CreatureLevelList.Add(new CsCreatureLevel(gameData.creatureLevels[i]));
		}

		//WPDCreatureBaseAttrValue[] Y   크리처기본속성값 목록
		for (int i = 0; i < gameData.creatureBaseAttrValues.Length; i++)
		{
			CsCreature csCreature = CsGameData.Instance.GetCreature(gameData.creatureBaseAttrValues[i].creatureId);

			if (csCreature != null)
			{
				csCreature.CreatureBaseAttrValueList.Add(new CsCreatureBaseAttrValue(gameData.creatureBaseAttrValues[i]));
			}
		}

		//WPDCreatureAdditionalAttr[] Y   크리처추가속성 목록
		CsGameData.Instance.CreatureAdditionalAttrList.Clear();

		for (int i = 0; i < gameData.creatureAdditionalAttrs.Length; i++)
		{
			CsGameData.Instance.CreatureAdditionalAttrList.Add(new CsCreatureAdditionalAttr(gameData.creatureAdditionalAttrs[i]));
		}

		//WPDCreatureInjectionLevel[] Y   크리처주입레벨 목록
		CsGameData.Instance.CreatureInjectionLevelList.Clear();

		for (int i = 0; i < gameData.creatureInjectionLevels.Length; i++)
		{
			CsGameData.Instance.CreatureInjectionLevelList.Add(new CsCreatureInjectionLevel(gameData.creatureInjectionLevels[i]));
		}

		//WPDCreatureAdditionalAttrValue[] Y   크리처추가속성값 목록
		for (int i = 0; i < gameData.creatureAdditionalAttrValues.Length; i++)
		{
			CsCreatureAdditionalAttr csCreatureAdditionalAttr = CsGameData.Instance.GetCreatureAdditionalAttr(gameData.creatureAdditionalAttrValues[i].attrId);

			if (csCreatureAdditionalAttr != null)
			{
				csCreatureAdditionalAttr.CreatureAdditionalAttrValueList.Add(new CsCreatureAdditionalAttrValue(gameData.creatureAdditionalAttrValues[i]));
			}
		}

		//WPDCreatureSkillSlotOpenRecipe[] Y   크리처스킬슬롯개방레시피 목록
		CsGameData.Instance.CreatureSkillSlotOpenRecipeList.Clear();

		for (int i = 0; i < gameData.creatureSkillSlotOpenRecipes.Length; i++)
		{
			CsGameData.Instance.CreatureSkillSlotOpenRecipeList.Add(new CsCreatureSkillSlotOpenRecipe(gameData.creatureSkillSlotOpenRecipes[i]));
		}

		//WPDCreatureSkillSlotProtection[] Y   크리처스킬슬롯보호 목록
		CsGameData.Instance.CreatureSkillSlotProtectionList.Clear();

		for (int i = 0; i < gameData.creatureSkillSlotProtections.Length; i++)
		{
			CsGameData.Instance.CreatureSkillSlotProtectionList.Add(new CsCreatureSkillSlotProtection(gameData.creatureSkillSlotProtections[i]));
		}

		//WPDMainQuestStartDialogue[] Y   메인퀘스트시작대화 목록
		for (int i = 0; i < gameData.mainQuestStartDialogues.Length; i++)
		{
			CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest(gameData.mainQuestStartDialogues[i].mainQuestNo);

			if (csMainQuest != null)
			{
				csMainQuest.MainQuestStartDialogueList.Add(new CsMainQuestStartDialogue(gameData.mainQuestStartDialogues[i]));
			}
		}

		//WPDMainQuestCompletionDialogue[] Y   메인퀘스트완료대화 목록
		for (int i = 0; i < gameData.mainQuestCompletionDialogues.Length; i++)
		{
			CsMainQuest csMainQuest = CsGameData.Instance.GetMainQuest(gameData.mainQuestCompletionDialogues[i].mainQuestNo);

			if (csMainQuest != null)
			{
				csMainQuest.MainQuestCompletionDialogueList.Add(new CsMainQuestCompletionDialogue(gameData.mainQuestCompletionDialogues[i]));
			}
		}

		//WPDPresent[] 선물 목록
		CsGameData.Instance.PresentList.Clear();

		for (int i = 0; i < gameData.presents.Length; i++)
		{
			CsGameData.Instance.PresentList.Add(new CsPresent(gameData.presents[i]));
		}

		//WPDWeeklyPresentPopularityPointRankingRewardGroup[] 주간선물인기점수랭킹보상그룹 목록
		CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList.Clear();

		for (int i = 0; i < gameData.weeklyPresentPopularityPointRankingRewardGroups.Length; i++)
		{
			CsGameData.Instance.WeeklyPresentPopularityPointRankingRewardGroupList.Add(new CsWeeklyPresentPopularityPointRankingRewardGroup(gameData.weeklyPresentPopularityPointRankingRewardGroups[i]));
		}

		//WPDWeeklyPresentPopularityPointRankingReward[] 주간선물인기점수랭킹보상 목록
		for (int i = 0; i < gameData.weeklyPresentPopularityPointRankingRewards.Length; i++)
		{
			CsWeeklyPresentPopularityPointRankingRewardGroup csWeeklyPresentPopularityPointRankingRewardGroup = CsGameData.Instance.GetWeeklyPresentPopularityPointRankingRewardGroup(gameData.weeklyPresentPopularityPointRankingRewards[i].groupNo);

			if (csWeeklyPresentPopularityPointRankingRewardGroup != null)
			{
				csWeeklyPresentPopularityPointRankingRewardGroup.WeeklyPresentPopularityPointRankingRewardList.Add(new CsWeeklyPresentPopularityPointRankingReward(gameData.weeklyPresentPopularityPointRankingRewards[i]));
			}
		}

		//WPDWeeklyPresentContributionPointRankingRewardGroup[] 주간선물공헌점수랭킹보상그룹 목록
		CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList.Clear();

		for (int i = 0; i < gameData.weeklyPresentContributionPointRankingRewardGroups.Length; i++)
		{
			CsGameData.Instance.WeeklyPresentContributionPointRankingRewardGroupList.Add(new CsWeeklyPresentContributionPointRankingRewardGroup(gameData.weeklyPresentContributionPointRankingRewardGroups[i]));
		}

		//WPDWeeklyPresentContributionPointRankingReward[] 주간선물공헌점수랭킹보상 목록
		for (int i = 0; i < gameData.weeklyPresentContributionPointRankingRewards.Length; i++)
		{
			CsWeeklyPresentContributionPointRankingRewardGroup csWeeklyPresentContributionPointRankingRewardGroup = CsGameData.Instance.GetWeeklyPresentContributionPointRankingRewardGroup(gameData.weeklyPresentContributionPointRankingRewards[i].groupNo);

			if (csWeeklyPresentContributionPointRankingRewardGroup != null)
			{
				csWeeklyPresentContributionPointRankingRewardGroup.WeeklyPresentContributionPointRankingRewardList.Add(new CsWeeklyPresentContributionPointRankingReward(gameData.weeklyPresentContributionPointRankingRewards[i]));
			}
		}

		//WPDCostume[] 코스튬 목록
		CsGameData.Instance.CostumeList.Clear();

		for (int i = 0; i < gameData.costumes.Length; i++)
		{
			CsGameData.Instance.CostumeList.Add(new CsCostume(gameData.costumes[i]));
		}

		//WPDCostumeEffect[] 코스튬효과 목록
		CsGameData.Instance.CostumeEffectList.Clear();

		for (int i = 0; i < gameData.costumeEffects.Length; i++)
		{
			CsGameData.Instance.CostumeEffectList.Add(new CsCostumeEffect(gameData.costumeEffects[i]));
		}

		//WPDCreatureFarmQuest 크리처농장퀘스트
		CsGameData.Instance.CreatureFarmQuest = new CsCreatureFarmQuest(gameData.creatureFarmQuest);

		//WPDCreatureFarmQuestExpReward[] 크리처농장퀘스트경험치보상 목록
		for (int i = 0; i < gameData.creatureFarmQuestExpRewards.Length; i++)
		{
			CsGameData.Instance.CreatureFarmQuest.CreatureFarmQuestExpRewardList.Add(new CsCreatureFarmQuestExpReward(gameData.creatureFarmQuestExpRewards[i]));
		}
		
		//WPDCreatureFarmQuestItemReward[] 크리처농장퀘스트아이템보상 목록
		for (int i = 0; i < gameData.creatureFarmQuestItemRewards.Length; i++)
		{
			CsGameData.Instance.CreatureFarmQuest.CreatureFarmQuestItemRewardList.Add(new CsCreatureFarmQuestItemReward(gameData.creatureFarmQuestItemRewards[i]));
		}

		//WPDCreatureFarmQuestMission[] 크리처농장퀘스트미션 목록
		for (int i = 0; i < gameData.creatureFarmQuestMissions.Length; i++)
		{
			CsGameData.Instance.CreatureFarmQuest.CreatureFarmQuestMissionList.Add(new CsCreatureFarmQuestMission(gameData.creatureFarmQuestMissions[i]));
		}

		// WPDSafeTimeEvent	안전시간이벤트
		CsGameData.Instance.SafeTimeEvent = new CsSafeTimeEvent(gameData.safeTimeEvent);

		// WPDGuildBlessingBuff[] 길드축복버프 목록
		CsGameData.Instance.GuildBlessingBuffList.Clear();

		for (int i = 0; i < gameData.guildBlessingBuffs.Length; i++)
		{
			CsGameData.Instance.GuildBlessingBuffList.Add(new CsGuildBlessingBuff(gameData.guildBlessingBuffs[i]));
		}

		//WPDJobChangeQuest[] 전직퀘스트 목록
		CsGameData.Instance.JobChangeQuestList.Clear();

		for (int i = 0; i < gameData.jobChangeQuests.Length; i++)
		{
			CsGameData.Instance.JobChangeQuestList.Add(new CsJobChangeQuest(gameData.jobChangeQuests[i]));
		}

		//WPDJobChangeQuestDifficulty[] 전직퀘스트난이도 목록
		for (int i = 0; i < gameData.jobChangeQuestDifficulties.Length; i++)
		{
			CsJobChangeQuest csJobChangeQuest = CsGameData.Instance.GetJobChangeQuest(gameData.jobChangeQuestDifficulties[i].questNo);

			if (csJobChangeQuest != null)
			{
				csJobChangeQuest.JobChangeQuestDifficultyList.Add(new CsJobChangeQuestDifficulty(gameData.jobChangeQuestDifficulties[i]));
			}
		}

		//WPDImprovementContent[] 향상컨텐츠 목록
		CsGameData.Instance.ImprovementContentList.Clear();

		for (int i = 0; i < gameData.improvementContents.Length; i++)
		{
			CsGameData.Instance.ImprovementContentList.Add(new CsImprovementContent(gameData.improvementContents[i]));
		}

		//WPDImprovementContentAchievementLevel[] 향상컨텐츠달성도레벨 목록
		for (int i = 0; i < gameData.improvementContentAchievementLevels.Length; i++)
		{
			CsImprovementContent csImprovementContent = CsGameData.Instance.GetImprovementContent(gameData.improvementContentAchievementLevels[i].contentId);

			if (csImprovementContent != null)
			{
				csImprovementContent.ImprovementContentAchievementLevelList.Add(new CsImprovementContentAchievementLevel(gameData.improvementContentAchievementLevels[i]));
			}
		}

		//WPDImprovementContentAchievement[] 향상컨텐츠달성도 목록
		CsGameData.Instance.ImprovementContentAchievementList.Clear();

		for (int i = 0; i < gameData.improvementContentAchievements.Length; i++)
		{
			CsGameData.Instance.ImprovementContentAchievementList.Add(new CsImprovementContentAchievement(gameData.improvementContentAchievements[i]));
		}

		//WPDRecommendBattlePowerLevel[] 추천전투력레벨 목록
		CsGameData.Instance.RecommendBattlePowerLevelList.Clear();

		for (int i = 0; i < gameData.recommendBattlePowerLevels.Length; i++)
		{
			CsGameData.Instance.RecommendBattlePowerLevelList.Add(new CsRecommendBattlePowerLevel(gameData.recommendBattlePowerLevels[i]));
		}

		//WPDImprovementMainCategory[] 향상메인카테고리 목록
		CsGameData.Instance.ImprovementMainCategoryList.Clear();

		for (int i = 0; i < gameData.improvementMainCategories.Length; i++)
		{
			CsGameData.Instance.ImprovementMainCategoryList.Add(new CsImprovementMainCategory(gameData.improvementMainCategories[i]));
		}

		CsGameData.Instance.ImprovementMainCategoryList.Sort();

		//WPDImprovementMainCategoryContent[] 향상메인카테고리컨텐츠 목록
		for (int i = 0; i < gameData.improvementMainCategoryContents.Length; i++)
		{
			CsImprovementMainCategory csImprovementMainCategory = CsGameData.Instance.GetImprovementMainCategory(gameData.improvementMainCategoryContents[i].mainCategoryId);

			if (csImprovementMainCategory != null)
			{
				csImprovementMainCategory.ImprovementMainCategoryContentList.Add(new CsImprovementMainCategoryContent(gameData.improvementMainCategoryContents[i]));
			}
		}

		//WPDImprovementSubCategory[] 향상서브카테고리 목록
		for (int i = 0; i < gameData.improvementSubCategories.Length; i++)
		{
			CsImprovementMainCategory csImprovementMainCategory = CsGameData.Instance.GetImprovementMainCategory(gameData.improvementSubCategories[i].mainCategoryId);

			if (csImprovementMainCategory != null)
			{
				csImprovementMainCategory.ImprovementSubCategoryList.Add(new CsImprovementSubCategory(gameData.improvementSubCategories[i]));
			}
		}

		//WPDImprovementSubCategoryContent[] 향상서브카테고리컨텐츠 목록
		for (int i = 0; i < gameData.improvementSubCategoryContents.Length; i++)
		{
			for (int j = 0; j < CsGameData.Instance.ImprovementMainCategoryList.Count; j++)
			{
				CsImprovementSubCategory csImprovementSubCategory = CsGameData.Instance.ImprovementMainCategoryList[j].GetImprovementSubCategory(gameData.improvementSubCategoryContents[i].subCategoryId);

				if (csImprovementSubCategory != null)
				{
					csImprovementSubCategory.ImprovementSubCategoryContentList.Add(new CsImprovementSubCategoryContent(gameData.improvementSubCategoryContents[i]));
					break;
				}
			}
		}

		for (int i = 0; i < CsGameData.Instance.ImprovementMainCategoryList.Count; i++)
		{
			CsGameData.Instance.ImprovementMainCategoryList[i].ImprovementMainCategoryContentList.Sort();
			CsGameData.Instance.ImprovementMainCategoryList[i].ImprovementSubCategoryList.Sort();

			for (int j = 0; j < CsGameData.Instance.ImprovementMainCategoryList[i].ImprovementSubCategoryList.Count; j++)
			{
				CsGameData.Instance.ImprovementMainCategoryList[i].ImprovementSubCategoryList[j].ImprovementSubCategoryContentList.Sort();
			}
		}

		// WPDPotionAttr[] 물약속성 목록
		CsGameData.Instance.PotionAttrList.Clear();

		for (int i = 0; i < gameData.potionAttrs.Length; i++)
		{
			CsGameData.Instance.PotionAttrList.Add(new CsPotionAttr(gameData.potionAttrs[i]));
		}

		//WPDInAppProduct[] 인앱상품 목록
		CsGameData.Instance.InAppProductList.Clear();

		for (int i = 0; i < gameData.inAppProducts.Length; i++)
		{
			CsGameData.Instance.InAppProductList.Add(new CsInAppProduct(gameData.inAppProducts[i]));
		}

		//WPDInAppProductPrice[] 인앱상품가격 목록
		for (int i = 0; i < gameData.inAppProductPrices.Length; i++)
		{
			CsInAppProduct csInAppProduct = CsGameData.Instance.GetInAppProduct(gameData.inAppProductPrices[i].inAppProductKey);

			if (csInAppProduct != null)
			{
				csInAppProduct.InAppProductPriceList.Add(new CsInAppProductPrice(gameData.inAppProductPrices[i]));
			}
		}

		//WPDCashProduct[] 캐시상품 목록
		CsGameData.Instance.CashProductList.Clear();

		for (int i = 0; i < gameData.cashProducts.Length; i++)
		{
			CsGameData.Instance.CashProductList.Add(new CsCashProduct(gameData.cashProducts[i]));
		}

		CsGameData.Instance.CashProductList.Sort();

		//WPDFirstChargeEvent 첫충전이벤트
		CsGameData.Instance.FirstChargeEvent = new CsFirstChargeEvent(gameData.firstChargeEvent);

		//WPDFirstChargeEventReward[] 첫충전이벤트보상 목록
		for (int i = 0; i < gameData.firstChargeEventRewards.Length; i++)
		{
			CsGameData.Instance.FirstChargeEvent.FirstChargeEventRewardList.Add(new CsFirstChargeEventReward(gameData.firstChargeEventRewards[i]));
		}

		//WPDRechargeEvent 재충전이벤트
		CsGameData.Instance.RechargeEvent = new CsRechargeEvent(gameData.rechargeEvent);

		//WPDRechargeEventReward[] 재충전이벤트보상 목록
		for (int i = 0; i < gameData.rechargeEventRewards.Length; i++)
		{
			CsGameData.Instance.RechargeEvent.RechargeEventRewardList.Add(new CsRechargeEventReward(gameData.rechargeEventRewards[i]));
		}

		//WPDChargeEvent[] 충전이벤트 목록
		CsGameData.Instance.ChargeEventList.Clear();

		for (int i = 0; i < gameData.chargeEvents.Length; i++)
		{
			CsGameData.Instance.ChargeEventList.Add(new CsChargeEvent(gameData.chargeEvents[i]));
		}
		
		//WPDChargeEventMission[] 충전이벤트미션 목록
		for (int i = 0; i < gameData.chargeEventMissions.Length; i++)
		{
			CsChargeEvent csChargeEvent = CsGameData.Instance.GetChargeEvent(gameData.chargeEventMissions[i].eventId);

			if (csChargeEvent != null)
			{
				csChargeEvent.ChargeEventMissionList.Add(new CsChargeEventMission(gameData.chargeEventMissions[i]));
			}
		}

		//WPDChargeEventMissionReward[] 충전이벤트미션보상 목록
		for (int i = 0; i < gameData.chargeEventMissionRewards.Length; i++)
		{
			CsChargeEvent csChargeEvent = CsGameData.Instance.GetChargeEvent(gameData.chargeEventMissionRewards[i].eventId);

			if (csChargeEvent != null)
			{
				CsChargeEventMission csChargeEventMission = csChargeEvent.GetChargeEventMission(gameData.chargeEventMissionRewards[i].missionNo);

				if (csChargeEventMission != null)
				{
					csChargeEventMission.ChargeEventMissionRewardList.Add(new CsChargeEventMissionReward(gameData.chargeEventMissionRewards[i]));
				}
			}
		}

		//WPDDailyChargeEvent 일일충전이벤트
		CsGameData.Instance.DailyChargeEvent = new CsDailyChargeEvent(gameData.dailyChargeEvent);

		//WPDDailyChargeEventMission[] 일일충전이벤트미션 목록
		for (int i = 0; i < gameData.dailyChargeEventMissions.Length; i++)
		{
			CsGameData.Instance.DailyChargeEvent.DailyChargeEventMissionList.Add(new CsDailyChargeEventMission(gameData.dailyChargeEventMissions[i]));
		}
		
		//WPDDailyChargeEventMissionReward[] 일일충전이벤트미션보상 목록
		for (int i = 0; i < gameData.dailyChargeEventMissionRewards.Length; i++)
		{
			CsDailyChargeEventMission csDailyChargeEventMission = CsGameData.Instance.DailyChargeEvent.GetDailyChargeEventMission(gameData.dailyChargeEventMissionRewards[i].missionNo);

			if (csDailyChargeEventMission != null)
			{
				csDailyChargeEventMission.DailyChargeEventMissionRewardList.Add(new CsDailyChargeEventMissionReward(gameData.dailyChargeEventMissionRewards[i]));
			}
		}

		//WPDConsumeEvent[] 소비이벤트 목록
		CsGameData.Instance.ConsumeEventList.Clear();

		for (int i = 0; i < gameData.consumeEvents.Length; i++)
		{
			CsGameData.Instance.ConsumeEventList.Add(new CsConsumeEvent(gameData.consumeEvents[i]));
		}

		//WPDConsumeEventMission[] 소비이벤트미션 목록
		for (int i = 0; i < gameData.consumeEventMissions.Length; i++)
		{
			CsConsumeEvent csConsumeEvent = CsGameData.Instance.GetConsumeEvent(gameData.consumeEventMissions[i].eventId);

			if (csConsumeEvent != null)
			{
				csConsumeEvent.ConsumeEventMissionList.Add(new CsConsumeEventMission(gameData.consumeEventMissions[i]));
			}
		}

		//WPDConsumeEventMissionReward[] 소비이벤트미션보상 목록
		for (int i = 0; i < gameData.consumeEventMissionRewards.Length; i++)
		{
			CsConsumeEvent csConsumeEvent = CsGameData.Instance.GetConsumeEvent(gameData.consumeEventMissionRewards[i].eventId);

			if (csConsumeEvent != null)
			{
				CsConsumeEventMission csConsumeEventMission = csConsumeEvent.GetConsumeEventMission(gameData.consumeEventMissionRewards[i].missionNo);

				if (csConsumeEventMission != null)
				{
					csConsumeEventMission.ConsumeEventMissionRewardList.Add(new CsConsumeEventMissionReward(gameData.consumeEventMissionRewards[i]));
				}
			}
		}

		//WPDDailyConsumeEvent 일일소비이벤트
		CsGameData.Instance.DailyConsumeEvent = new CsDailyConsumeEvent(gameData.dailyConsumeEvent);

		//WPDDailyConsumeEventMission[] 일일소비이벤트미션 목록
		for (int i = 0; i < gameData.dailyConsumeEventMissions.Length; i++)
		{
			CsGameData.Instance.DailyConsumeEvent.DailyConsumeEventMissionList.Add(new CsDailyConsumeEventMission(gameData.dailyConsumeEventMissions[i]));
		}

		//WPDDailyConsumeEventMissionReward[] 일일소비이벤트미션보상 목록
		for (int i = 0; i < gameData.dailyConsumeEventMissionRewards.Length; i++)
		{
			CsDailyConsumeEventMission csDailyConsumeEventMission = CsGameData.Instance.DailyConsumeEvent.GetDailyConsumeEventMission(gameData.dailyConsumeEventMissionRewards[i].missionNo);

			if (csDailyConsumeEventMission != null)
			{
				csDailyConsumeEventMission.DailyConsumeEventMissionRewardList.Add(new CsDailyConsumeEventMissionReward(gameData.dailyConsumeEventMissionRewards[i]));
			}
		}

		//WPDAnkouTomb 안쿠의무덤
		CsGameData.Instance.AnkouTomb = new CsAnkouTomb(gameData.ankouTomb);

		//WPDAnkouTombSchedule[] 안쿠의무덤스케줄 목록
		for (int i = 0; i < gameData.ankouTombSchedules.Length; i++)
		{
			CsGameData.Instance.AnkouTomb.AnkouTombScheduleList.Add(new CsAnkouTombSchedule(gameData.ankouTombSchedules[i]));
		}

		//WPDAnkouTombDifficulty[] 안쿠의무덤난이도 목록
		for (int i = 0; i < gameData.ankouTombDifficulties.Length; i++)
		{
			CsGameData.Instance.AnkouTomb.AnkouTombDifficultyList.Add(new CsAnkouTombDifficulty(gameData.ankouTombDifficulties[i]));
		}

		//WPDAnkouTombAvailableReward[] 안쿠의모덤획득가능보상 목록
		for (int i = 0; i < gameData.ankouTombAvailableRewards.Length; i++)
		{
			CsAnkouTombDifficulty csAnkouTombDifficulty = CsGameData.Instance.AnkouTomb.GetAnkouTombDifficulty(gameData.ankouTombAvailableRewards[i].difficulty);

			if (csAnkouTombDifficulty != null)
			{
				csAnkouTombDifficulty.AnkouTombAvailableRewardList.Add(new CsAnkouTombAvailableReward(gameData.ankouTombAvailableRewards[i]));
			}
		}

		//WPDConstellation[] 별자리 목록
		CsGameData.Instance.ConstellationList.Clear();

		for (int i = 0; i < gameData.constellations.Length; i++)
		{
			CsGameData.Instance.ConstellationList.Add(new CsConstellation(gameData.constellations[i]));
		}

		//WPDConstellationStep[] 별자리단계 목록
		for (int i = 0; i < gameData.constellationSteps.Length; i++)
		{
			CsConstellation csConstellation = CsGameData.Instance.GetConstellation(gameData.constellationSteps[i].constellationId);

			if (csConstellation != null)
			{
				csConstellation.ConstellationStepList.Add(new CsConstellationStep(gameData.constellationSteps[i]));
			}
		}

		//WPDConstellationCycle[] 별자리사이클 목록
		for (int i = 0; i < gameData.constellationCycles.Length; i++)
		{
			CsConstellation csConstellation = CsGameData.Instance.GetConstellation(gameData.constellationCycles[i].constellationId);

			if (csConstellation != null)
			{
				CsConstellationStep csConstellationStep = csConstellation.GetConstellationStep(gameData.constellationCycles[i].step);

				if (csConstellationStep != null)
				{
					csConstellationStep.ConstellationCycleList.Add(new CsConstellationCycle(gameData.constellationCycles[i]));
				}
			}
		}

		//WPDConstellationCycleBuff[] 별자리사이클버프 목록
		for (int i = 0; i < gameData.constellationCycleBuffs.Length; i++)
		{
			CsConstellation csConstellation = CsGameData.Instance.GetConstellation(gameData.constellationCycleBuffs[i].constellationId);

			if (csConstellation != null)
			{
				CsConstellationStep csConstellationStep = csConstellation.GetConstellationStep(gameData.constellationCycleBuffs[i].step);

				if (csConstellationStep != null)
				{
					CsConstellationCycle csConstellationCycle = csConstellationStep.GetConstellationCycle(gameData.constellationCycleBuffs[i].cycle);

					if (csConstellationCycle != null)
					{
						csConstellationCycle.ConstellationCycleBuffList.Add(new CsConstellationCycleBuff(gameData.constellationCycleBuffs[i]));
					}
				}
			}
		}

		//WPDConstellationEntry[] 별자리항목 목록
		for (int i = 0; i < gameData.constellationEntries.Length; i++)
		{
			CsConstellation csConstellation = CsGameData.Instance.GetConstellation(gameData.constellationEntries[i].constellationId);

			if (csConstellation != null)
			{
				CsConstellationStep csConstellationStep = csConstellation.GetConstellationStep(gameData.constellationEntries[i].step);

				if (csConstellationStep != null)
				{
					CsConstellationCycle csConstellationCycle = csConstellationStep.GetConstellationCycle(gameData.constellationEntries[i].cycle);

					if (csConstellationCycle != null)
					{
						csConstellationCycle.ConstellationEntryList.Add(new CsConstellationEntry(gameData.constellationEntries[i]));
					}
				}
			}
		}

		//WPDConstellationEntryBuff[] 별자리항목버프 목록
		for (int i = 0; i < gameData.constellationEntryBuffs.Length; i++)
		{
			CsConstellation csConstellation = CsGameData.Instance.GetConstellation(gameData.constellationEntryBuffs[i].constellationId);

			if (csConstellation != null)
			{
				CsConstellationStep csConstellationStep = csConstellation.GetConstellationStep(gameData.constellationEntryBuffs[i].step);

				if (csConstellationStep != null)
				{
					CsConstellationCycle csConstellationCycle = csConstellationStep.GetConstellationCycle(gameData.constellationEntryBuffs[i].cycle);

					if (csConstellationCycle != null)
					{
						CsConstellationEntry csConstellationEntry = csConstellationCycle.GetConstellationEntry(gameData.constellationEntryBuffs[i].entryNo);

						if (csConstellationEntry != null)
						{
							csConstellationEntry.ConstellationEntryBuffList.Add(new CsConstellationEntryBuff(gameData.constellationEntryBuffs[i]));
						}
					}
				}
			}
		}

		//WPDArtifact[] 아티팩트 목록
		CsGameData.Instance.ArtifactList.Clear();

		for (int i = 0; i < gameData.artifacts.Length; i++)
		{
			CsGameData.Instance.ArtifactList.Add(new CsArtifact(gameData.artifacts[i]));
		}

		//WPDArtifactAttr[] 아티팩트속성 목록
		for (int i = 0; i < gameData.artifactAttrs.Length; i++)
		{
			CsArtifact csArtifact = CsGameData.Instance.GetArtifact(gameData.artifactAttrs[i].artifactNo);

			if (csArtifact != null)
			{
				csArtifact.ArtifactAttrList.Add(new CsArtifactAttr(gameData.artifactAttrs[i]));
			}
		}

		//WPDArtifactLevel[] 아티팩트레벨 목록
		for (int i = 0; i < gameData.artifactLevels.Length; i++)
		{
			CsArtifact csArtifact = CsGameData.Instance.GetArtifact(gameData.artifactLevels[i].artifactNo);

			if (csArtifact != null)
			{
				csArtifact.ArtifactLevelList.Add(new CsArtifactLevel(gameData.artifactLevels[i]));
			}
		}

		//WPDArtifactLevelAttr[] 아티팩트레벨속성 목록
		for (int i = 0; i < gameData.artifactLevelAttrs.Length; i++)
		{
			CsArtifact csArtifact = CsGameData.Instance.GetArtifact(gameData.artifactLevelAttrs[i].artifactNo);

			if (csArtifact != null)
			{
				CsArtifactLevel csArtifactLevel = csArtifact.GetArtifactLevel(gameData.artifactLevelAttrs[i].level);

				if (csArtifactLevel != null)
				{
					csArtifactLevel.ArtifactLevelAttrList.Add(new CsArtifactLevelAttr(gameData.artifactLevelAttrs[i]));
				}
			}
		}

		//WPDArtifactLevelUpMaterial[] 아티팩트레벨업재료 목록
		CsGameData.Instance.ArtifactLevelUpMaterialList.Clear();

		for (int i = 0; i < gameData.artifactLevelUpMaterials.Length; i++)
		{
			CsGameData.Instance.ArtifactLevelUpMaterialList.Add(new CsArtifactLevelUpMaterial(gameData.artifactLevelUpMaterials[i]));
		}

		//WPDMountAwakeningLevelMaster[] 탈것각성레벨마스터 목록
		CsGameData.Instance.MountAwakeningLevelMasterList.Clear();

		for (int i = 0; i < gameData.mountAwakeningLevelMasters.Length; i++)
		{
			CsGameData.Instance.MountAwakeningLevelMasterList.Add(new CsMountAwakeningLevelMaster(gameData.mountAwakeningLevelMasters[i]));
		}

		//WPDMountAwakeningLevel[] 탈것각성레벨 목록
		for (int i = 0; i < gameData.mountAwakeningLevels.Length; i++)
		{
			CsMount csMount = CsGameData.Instance.GetMount(gameData.mountAwakeningLevels[i].mountId);

			if (csMount != null)
			{
				csMount.MountAwakeningLevelList.Add(new CsMountAwakeningLevel(gameData.mountAwakeningLevels[i]));
			}
		}

		//WPDMountPotionAttrCount[] 탈것물약속성횟수 목록
		CsGameData.Instance.MountPotionAttrCountList.Clear();

		for (int i = 0; i < gameData.mountPotionAttrCounts.Length; i++)
		{
			CsGameData.Instance.MountPotionAttrCountList.Add(new CsMountPotionAttrCount(gameData.mountPotionAttrCounts[i]));
		}

		//WPDTradeShip 무역선탈환
		CsGameData.Instance.TradeShip = new CsTradeShip(gameData.tradeShip);

		//WPDTradeShipSchedule[] 무역선탈환스케줄 목록
		for (int i = 0; i < gameData.tradeShipSchedules.Length; i++)
		{
			CsGameData.Instance.TradeShip.TradeShipScheduleList.Add(new CsTradeShipSchedule(gameData.tradeShipSchedules[i]));
		}

		//WPDTradeShipObstacle[] 무역선탈환장애물 목록
		for (int i = 0; i < gameData.tradeShipObstacles.Length; i++)
		{
			CsGameData.Instance.TradeShip.TradeShipObstacleList.Add(new CsTradeShipObstacle(gameData.tradeShipObstacles[i]));
		}

		//WPDTradeShipStep[]  무역선탈환단계 목록
		for (int i = 0; i < gameData.tradeShipSteps.Length; i++)
		{
			CsGameData.Instance.TradeShip.TradeShipStepList.Add(new CsTradeShipStep(gameData.tradeShipSteps[i]));
		}

		//WPDTradeShipDifficulty[] 무역선탈환난이도 목록
		for (int i = 0; i < gameData.tradeShipDifficulties.Length; i++)
		{
			CsGameData.Instance.TradeShip.TradeShipDifficultyList.Add(new CsTradeShipDifficulty(gameData.tradeShipDifficulties[i]));
		}

		//WPDTradeShipAvailableReward[] 무역선탈환획득가능보상 목록
		for (int i = 0; i < gameData.tradeShipAvailableRewards.Length; i++)
		{
			CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.GetTradeShipDifficulty(gameData.tradeShipAvailableRewards[i].difficulty);

			if (csTradeShipDifficulty != null)
			{
				csTradeShipDifficulty.TradeShipAvailableRewardList.Add(new CsTradeShipAvailableReward(gameData.tradeShipAvailableRewards[i]));
			}
		}

		//WPDTradeShipObject[] 무역선탈환오브젝트 목록
		for (int i = 0; i < gameData.tradeShipObjects.Length; i++)
		{
			CsTradeShipDifficulty csTradeShipDifficulty = CsGameData.Instance.TradeShip.GetTradeShipDifficulty(gameData.tradeShipObjects[i].difficulty);

			if (csTradeShipDifficulty != null)
			{
				csTradeShipDifficulty.TradeShipObjectList.Add(new CsTradeShipObject(gameData.tradeShipObjects[i]));
			}
		}


		//WPDCostumeDisplay[] 코스튬표시 목록
		for (int i = 0; i < gameData.costumeDisplays.Length; i++)
		{
			CsCostume csCostume = CsGameData.Instance.GetCostume(gameData.costumeDisplays[i].costumeId);

			if (csCostume != null)
			{
				csCostume.CostumeDisplayList.Add(new CsCostumeDisplay(gameData.costumeDisplays[i]));
			}
		}

		//WPDCostumeCollection[] 코스튬콜렉션 목록
		CsGameData.Instance.CostumeCollectionList.Clear();

		for (int i = 0; i < gameData.costumeCollections.Length; i++)
		{
			CsGameData.Instance.CostumeCollectionList.Add(new CsCostumeCollection(gameData.costumeCollections[i]));
		}

		//WPDCostumeCollectionAttr[] 코스튬콜렉션속성 목록
		for (int i = 0; i < gameData.costumeCollectionAttrs.Length; i++)
		{
			CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(gameData.costumeCollectionAttrs[i].costumeCollectionId);

			if (csCostumeCollection != null)
			{
				csCostumeCollection.CostumeCollectionAttrList.Add(new CsCostumeCollectionAttr(gameData.costumeCollectionAttrs[i]));
			}
		}

		//WPDCostumeCollectionEntry[] 코스튬콜렉션항목 목록
		for (int i = 0; i < gameData.costumeCollectionEntries.Length; i++)
		{
			CsCostumeCollection csCostumeCollection = CsGameData.Instance.GetCostumeCollection(gameData.costumeCollectionEntries[i].costumeCollectionId);

			if (csCostumeCollection != null)
			{
				csCostumeCollection.CostumeCollectionEntryList.Add(new CsCostumeCollectionEntry(gameData.costumeCollectionEntries[i]));
			}
		}

		for (int i = 0; i < CsGameData.Instance.CostumeCollectionList.Count; i++)
		{
			CsGameData.Instance.CostumeCollectionList[i].CostumeCollectionEntryList.Sort();
		}

		//WPDCostumeAttr[] 코스튬속성 목록
		for (int i = 0; i < gameData.costumeAttrs.Length; i++)
		{
			CsCostume csCostume = CsGameData.Instance.GetCostume(gameData.costumeAttrs[i].costumeId);

			if (csCostume != null)
			{
				csCostume.CostumeAttrList.Add(new CsCostumeAttr(gameData.costumeAttrs[i]));
			}
		}

		//WPDCostumeEnchantLevel[] 코스튬강화레벨 목록
		CsGameData.Instance.CostumeEnchantLevelList.Clear();

		for (int i = 0; i < gameData.costumeEnchantLevels.Length; i++)
		{
			CsGameData.Instance.CostumeEnchantLevelList.Add(new CsCostumeEnchantLevel(gameData.costumeEnchantLevels[i]));
		}

		//WPDCostumeEnchantLevelAttr[] 코스튬강화레벨속성 목록
		CsGameData.Instance.CostumeEnchantLevelAttrList.Clear();

		for (int i = 0; i < gameData.costumeEnchantLevelAttrs.Length; i++)
		{
			CsGameData.Instance.CostumeEnchantLevelAttrList.Add(new CsCostumeEnchantLevelAttr(gameData.costumeEnchantLevelAttrs[i]));
		}

		//WPDScheduleNotice[] 스케줄알림 목록
		CsGameData.Instance.ScheduleNoticeList.Clear();

		for (int i = 0; i < gameData.scheduleNotices.Length; i++)
		{
			CsGameData.Instance.ScheduleNoticeList.Add(new CsScheduleNotice(gameData.scheduleNotices[i]));
		}

		//WPDSharingEvent[] 공유이벤트 목록
		CsGameData.Instance.SharingEventList.Clear();

		for (int i = 0; i < gameData.sharingEvents.Length; i++)
		{
			CsGameData.Instance.SharingEventList.Add(new CsSharingEvent(gameData.sharingEvents[i]));
		}

		//WPDSystemMessage[] 시스템메세지 목록
		CsGameData.Instance.SystemMessageInfoList.Clear();

		for (int i = 0; i < gameData.systemMessages.Length; i++)
		{
			CsGameData.Instance.SystemMessageInfoList.Add(new CsSystemMessageInfo(gameData.systemMessages[i]));
		}


		//WPDSharingEventSenderReward[] 공유이벤트발신자보상 목록
		for (int i = 0; i < gameData.sharingEventSenderRewards.Length; i++)
		{
			CsSharingEvent csSharingEvent = CsGameData.Instance.GetSharingEvent(gameData.sharingEventSenderRewards[i].eventId);

			if (csSharingEvent != null)
			{
				csSharingEvent.SharingEventSenderRewardList.Add(new CsSharingEventSenderReward(gameData.sharingEventSenderRewards[i]));
			}
		}

		//WPDSharingEventReceiverReward[] 공유이벤트수신자보상 목록
		for (int i = 0; i < gameData.sharingEventReceiverRewards.Length; i++)
		{
			CsSharingEvent csSharingEvent = CsGameData.Instance.GetSharingEvent(gameData.sharingEventReceiverRewards[i].eventId);

			if (csSharingEvent != null)
			{
				csSharingEvent.SharingEventReceiverRewardList.Add(new CsSharingEventReceiverReward(gameData.sharingEventReceiverRewards[i]));
			}
		}

		//WPDTeamBattlefield 팀전장

		//WPDTeamBattlefieldAvailableReward[] 팀전장획득가능보상 목록


		//WPDAccomplishmentLevel[] 업적레벨 목록
		CsGameData.Instance.AccomplishmentLevelList.Clear();



		//WPDAccomplishmentLevelAttr[] 업적레벨속성 목록

	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator FadeOut(UnityAction end)
    {

        m_trFade.gameObject.SetActive(true);

        CanvasGroup canvasGroup = m_trFade.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
		
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        end();
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator FadeIn()
    {
        m_trFade.gameObject.SetActive(true);

        CanvasGroup canvasGroup = m_trFade.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;
        /*
		float duration = 1f;

		for (float t = 0f; t <= duration; t += Time.deltaTime)
		{
			canvasGroup.alpha -= t / duration;
			yield return null;
		}
		canvasGroup.alpha = 0f;
		*/
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void SetCharacterLight(bool bIsOn)
    {
        Transform trLight = m_trLightList.Find("Light");
        trLight.gameObject.SetActive(bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    void PlayBGM(EnSoundTypeIntro enSoundTypeIntro)
    {
        if (CsConfiguration.Instance.GetSettingKey(EnPlayerPrefsKey.BGM) == 0)
            return;

        switch (enSoundTypeIntro)
        {
            case EnSoundTypeIntro.Intro:
                if (m_audioSource.clip == null)
                {
                    m_audioSource.clip = m_audioClipIntroLoading;
                    m_audioSource.Play();
                }
                else
                {
                    if (m_audioSource.clip.name != m_audioClipIntroLoading.name)
                    {
                        m_audioSource.clip = m_audioClipIntroLoading;
                        m_audioSource.Play();
                    }
                }
                break;

            case EnSoundTypeIntro.Main:
                if (m_audioSource.clip == null)
                {
                    m_audioSource.clip = m_audioClipIntroMain;
                    m_audioSource.Play();
                }
                else
                {
                    if (m_audioSource.clip.name != m_audioClipIntroMain.name)
                    {
                        m_audioSource.clip = m_audioClipIntroMain;
                        m_audioSource.Play();
                    }
                }

                break;
            case EnSoundTypeIntro.Chracter:
                if (m_audioSource.clip == null)
                {
                    m_audioSource.clip = m_audioClipIntroCharacter;
                    m_audioSource.Play();
                }
                else
                {
                    if (m_audioSource.clip.name != m_audioClipIntroCharacter.name)
                    {
                        m_audioSource.clip = m_audioClipIntroCharacter;
                        m_audioSource.Play();
                    }
                }

                break;
        }
        m_audioSource.loop = true;
    }

	#region ErrorLogging

	//---------------------------------------------------------------------------------------------------
	void SendErrorLogging(string strMessage)
	{
		if (CsConfiguration.Instance.SystemSetting.LoggingEnabled)
		{
			if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
			{
				LoggingNACommand cmd = new LoggingNACommand();
				cmd.Content = strMessage;
				cmd.Finished += ResponseErrorLogging;
				cmd.Run();
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ResponseErrorLogging(object sender, EventArgs e)
	{
		LoggingNACommand cmd = (LoggingNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		LoggingNAResponse res = (LoggingNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseErrorLogging OK");
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	#endregion ErrorLogging

	#region  StatusLogging

	void SendStatusLogging(int nPing, float fFps)
	{
		if (CsConfiguration.Instance.ConnectMode != CsConfiguration.EnConnectMode.UNITY_ONLY)
		{
			StatusLoggingNACommand cmd = new StatusLoggingNACommand();
			cmd.ping = nPing.ToString();
			cmd.frameRate = fFps.ToString();
			cmd.UserId = CsConfiguration.Instance.User.UserId;
			cmd.HeroId = CsGameData.Instance.MyHeroInfo.HeroId.ToString();
			cmd.Finished += ResponseStatusLogging;
			cmd.Run();
		}
	}

	void ResponseStatusLogging(object sender, EventArgs e)
	{
		StatusLoggingNACommand cmd = (StatusLoggingNACommand)sender;

		if (!cmd.isOK)
		{
			Debug.Log(cmd.error.Message);
			Debug.Log(cmd.Trace());
			return;
		}

		StatusLoggingNAResponse res = (StatusLoggingNAResponse)cmd.response;

		if (res.isOK)
		{
			Debug.Log("ResponseStatusLogging OK");
		}
		else
		{
			Debug.Log(res.errorMessage);
			Debug.Log(res.Trace());
			return;
		}
	}

	#endregion StatusLogging


	#region 캐릭터 선택 및 계정 변경에 대한 로딩창

	bool m_bLoadingComplete;

    Text m_textLoadingPercent;
    Slider m_sliderLoading;

    IEnumerator m_IEnumeratorLoad;

    //---------------------------------------------------------------------------------------------------
    void InitializeLoadingUI()
    {
        //로딩창
        m_textLoadingPercent = m_trLoading.Find("TextPercent").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLoadingPercent);

        m_sliderLoading = m_trLoading.Find("Slider").GetComponent<Slider>();
        m_sliderLoading.maxValue = 100f;

        Text textLoading = m_trLoading.Find("Text").GetComponent<Text>();
        textLoading.text = CsConfiguration.Instance.GetString("A01_TXT_00053");
        CsUIData.Instance.SetFont(textLoading);

    }

    //---------------------------------------------------------------------------------------------------
    void StartLoadingImgae()
    {
        Transform trImageList = m_trLoading.Find("ImageList");
        int nImageNo = UnityEngine.Random.Range(0, trImageList.childCount - 1);

        for (int i = 0; i < trImageList.childCount; i++)
        {
            Transform trImage = trImageList.Find("Image" + i);

            if (i == nImageNo)
            {
                trImage.gameObject.SetActive(true);
            }
            else
            {
                if (trImage.gameObject.activeSelf)
                {
                    trImage.gameObject.SetActive(false);
                }
            }
        }

        if (m_IEnumeratorLoad != null)
        {
            StopCoroutine(m_IEnumeratorLoad);
            m_IEnumeratorLoad = null;
        }

        m_IEnumeratorLoad = LoadingSliderCoroutine();
        StartCoroutine(m_IEnumeratorLoad);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadingSliderCoroutine()
    {
        m_textLoadingPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), "0");
        m_sliderLoading.maxValue = 100;
        m_sliderLoading.value = 0;

        while (m_sliderLoading.value < 100)
        {
            if (m_bIsEndLoading == false || m_bIsEndDataLoading == false)
            {
                if (m_sliderLoading.value >= 90)
                {
                    m_sliderLoading.value = 90;
                }
                else
                {
                    m_sliderLoading.value += 1;
                }
            }
            else
            {
                m_sliderLoading.value += 5;

                if (m_sliderLoading.value >= 100)
                {
                    m_sliderLoading.value = 100;
                }
                
            }

            m_textLoadingPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), m_sliderLoading.value);

            yield return new WaitForSeconds(0.05f);
        }
    }

#endregion 캐릭터 선택 및 계정 변경에 대한 로딩창

}
