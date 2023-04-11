using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-11)
//---------------------------------------------------------------------------------------------------

public enum EnToastType
{
    Normal = 1,
    Error = 2,
    ChangeAreaDungeon = 3,
    ChangeAreaContinent = 4,
    ChangeBattlePower = 5,
    SkillOpen = 6,
    ContentOpen = 7,
}

struct StructTextToast
{
    public EnToastType enToastType;
    public string strMessage;

    public StructTextToast(EnToastType enToastTypeTemp, string strTemp)
    {
        enToastType = enToastTypeTemp;
        strMessage = strTemp;
    }
}

public class CsPanelToast : MonoBehaviour
{
    Transform m_trTextList;
    Transform m_trPopup;
    Transform m_trSystemToast;

    GameObject m_goNormalToast;
    GameObject m_goErrorToast;
    GameObject m_goAreaToast;
    GameObject m_goChangeBattleToast;
    GameObject m_goToastContentsOpen;
    GameObject m_goSystemToast;
    GameObject m_goLevelUpToast;

    GameObject m_goTutorial;
    GameObject m_goTutorialNoBack;

    bool m_bIsToastPlaying = false;

    IEnumerator m_iEnumeratorSystemToastScrollPositionToast;

    List<StructTextToast> m_listTextToast = new List<StructTextToast>();
    List<CsMenuContent> m_listCsMenuContent = new List<CsMenuContent>();    // 데이터용 토스트 해야하는 리스트
    Queue<string> m_queueSystemMessage = new Queue<string>();

    

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        Transform trCanvas = GameObject.Find("Canvas").transform;
        m_trPopup = trCanvas.Find("Popup");
        m_trTextList = transform.Find("TextList");
        StartCoroutine(CreateTextToastCoroutine());
        
        m_listCsMenuContent = CsGameData.Instance.MenuContentList.FindAll(a => a.IsDisplay == true);

        CsGameEventUIToUI.Instance.EventToastMessage += OnEventToastMessage;
        CsGameEventUIToUI.Instance.EventToastChangeArea += OnEventToastChangeArea;
        CsGameEventUIToUI.Instance.EventToastChangeBattlePower += OnEventToastChangeBattlePower;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        
        CsOpenToastManager.Instance.EventToastOpenSkill += OnEventToastOpenSkill;
		CsOpenToastManager.Instance.EventOpenContentAnimation += OnEventOpenContentAnimation;
		CsOpenToastManager.Instance.EventOpenTutorialAnimation += OnEventOpenTutorialAnimation;
		CsOpenToastManager.Instance.EventOpenTypeTutorialAnimation += OnEventOpenTypeTutorialAnimation;
        CsGameEventUIToUI.Instance.EventToastSystem += OnEventToastSystem;
        CsGameEventUIToUI.Instance.EventNotice += OnEventNotice;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventToastMessage -= OnEventToastMessage;
        CsGameEventUIToUI.Instance.EventToastChangeArea -= OnEventToastChangeArea;
        CsGameEventUIToUI.Instance.EventToastChangeBattlePower -= OnEventToastChangeBattlePower;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;

		CsOpenToastManager.Instance.EventToastOpenSkill -= OnEventToastOpenSkill;
		CsOpenToastManager.Instance.EventOpenContentAnimation -= OnEventOpenContentAnimation;
		CsOpenToastManager.Instance.EventOpenTutorialAnimation -= OnEventOpenTutorialAnimation;
		CsOpenToastManager.Instance.EventOpenTypeTutorialAnimation -= OnEventOpenTypeTutorialAnimation;
        CsGameEventUIToUI.Instance.EventToastSystem -= OnEventToastSystem;
        CsGameEventUIToUI.Instance.EventNotice -= OnEventNotice;
    }

    #region EventHandler
    
    //---------------------------------------------------------------------------------------------------
    void OnEventToastSystem(string strMessage)
    {
        OpenSystemToast(strMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNotice(string strMessage)
    {
        CsGameEventUIToUI.Instance.OnEventToastSystem(strMessage);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        OpenLevelUpToast();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventToastOpenSkill(int nSkillId)
    {
        CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		StartCoroutine(OpenSkill(nSkillId));
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenContentAnimation(CsMenuContent csMenuContent)
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		StartCoroutine(OpenMenuContent(csMenuContent));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenTutorialAnimation(CsMenuContent csMenuContent)
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		StartCoroutine(OpenContentsTutorial(csMenuContent));
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventOpenTypeTutorialAnimation(EnTutorialType enTutorialType)
	{
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		StartCoroutine(OpenTypeTutorial(enTutorialType));
	}

    //---------------------------------------------------------------------------------------------------
    void OnEventToastChangeBattlePower(long lOldBattlePower)
    {
        OpenChangeBattleToast(lOldBattlePower);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventToastMessage(EnToastType enToastType, string strMessage)
    {
        StructTextToast structTextToast = new StructTextToast(enToastType, strMessage);
        m_listTextToast.Add(structTextToast);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventToastChangeArea(EnToastType enToastType, string strMessage1, string strMessage2, bool bMyNation = true)
    {
        switch (enToastType)
        {
            case EnToastType.ChangeAreaDungeon:
                OpenAreaToast(strMessage1, strMessage2, true);
                break;
            case EnToastType.ChangeAreaContinent:
                OpenAreaToast(strMessage1, strMessage2, bMyNation);
                break;
        }
    }

    #endregion EventHandler

    #region 일반/에러 메시지 토스트

    //---------------------------------------------------------------------------------------------------
    IEnumerator CreateTextToastCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => m_listTextToast.Count > 0);

            switch (m_listTextToast[0].enToastType)
            {
                case EnToastType.Normal:
                    CreateNormalToast(m_listTextToast[0].strMessage);
                    break;
                case EnToastType.Error:
                    CreateErrorToast(m_listTextToast[0].strMessage);
                    break;
            }

            m_listTextToast.Remove(m_listTextToast[0]);

            yield return new WaitForSeconds(0.2f);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateNormalToast(string strMessage)
    {
        if (m_goNormalToast == null)
        {
            m_goNormalToast = CsUIData.Instance.LoadAsset<GameObject>("GUI/Toast/NormalToast");
        }

        if (m_trTextList.childCount == 3)
        {
            Destroy(m_trTextList.GetChild(2).gameObject);
        }

        GameObject goToast = Instantiate(m_goNormalToast, m_trTextList);
        goToast.transform.SetAsFirstSibling();
        Destroy(goToast, CsGameConfig.Instance.DefaultToastDisplayDuration);

        Text text = goToast.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(text);
        text.text = strMessage;
    }

    //---------------------------------------------------------------------------------------------------
    void CreateErrorToast(string strMessage)
    {
        if (m_goErrorToast == null)
        {
            m_goErrorToast = CsUIData.Instance.LoadAsset<GameObject>("GUI/Toast/ErrorToast");
        }

        if (m_trTextList.childCount == 3)
        {
            Destroy(m_trTextList.GetChild(2).gameObject);
        }

        GameObject goToast = Instantiate(m_goErrorToast, m_trTextList);
        goToast.transform.SetAsFirstSibling();
        Destroy(goToast, CsGameConfig.Instance.DefaultToastDisplayDuration);

        Text text = goToast.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(text);
        text.text = strMessage;
    }

    #endregion 일반/에러 메시지 토스트

    #region 레벨업토스트

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadLevelUpToast(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/LevelUpToast");
        yield return resourceRequest;
        m_goLevelUpToast = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenLevelUpToast()
    {
        if (m_goLevelUpToast == null)
        {
            StartCoroutine(LoadLevelUpToast(() => InitializeLevelUpToast()));
        }
        else
        {
            InitializeLevelUpToast();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeLevelUpToast()
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.CharacterLevelUp);
        GameObject goAreaToast = Instantiate(m_goLevelUpToast, transform);
        goAreaToast.name = "LevelUpToast";

        Destroy(goAreaToast, 2.5f);
    }

    #endregion 레벨업토스트

    #region 지역이동토스트

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadAreaToast(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/AreaEnterToast");
        yield return resourceRequest;
        m_goAreaToast = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenAreaToast(string str1, string str2, bool bIsMyNation)
    {
        if (m_goAreaToast == null)
        {
            StartCoroutine(LoadAreaToast(() => InitializeAreaToast(str1, str2, bIsMyNation)));
        }
        else
        {
            InitializeAreaToast(str1, str2, bIsMyNation);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeAreaToast(string str1, string str2, bool bIsMyNation)
    {
        GameObject goAreaToast = null;

        if (CsUIData.Instance.DungeonInNow == EnDungeon.StoryDungeon)
        {
            goAreaToast = Instantiate(m_goAreaToast, transform);
        }
        else
        {
            goAreaToast = Instantiate(m_goAreaToast, m_trPopup);
        }

        goAreaToast.name = "AreaToast";

        Text textName1 = goAreaToast.transform.Find("TextName1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName1);
        textName1.text = str1;

        Text textName2 = goAreaToast.transform.Find("TextName2").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName2);
        textName2.text = str2;

        if (!bIsMyNation)
        {
            textName1.color = CsUIData.Instance.ColorRed;
            textName2.color = CsUIData.Instance.ColorRed;
        }

        StartCoroutine(LoadAreaToast(goAreaToast));
    }

    IEnumerator LoadAreaToast(GameObject goAreaToast)
    {
        int nTime = CsGameConfig.Instance.LocationAreaToastDisplayDuration - 1;
        yield return new WaitForSeconds(nTime);

        Animator animator = goAreaToast.GetComponent<Animator>();

        if (animator != null)
        {
            animator.SetBool("Fade", true);
            Destroy(goAreaToast, 1);
        }

    }

    #endregion 지역이동토스트

    #region 전투력 토스트

    //---------------------------------------------------------------------------------------------------
    void OpenChangeBattleToast(long lOldBattlePower)
    {
        if (m_goChangeBattleToast == null)
        {
            StartCoroutine(LoadChangeBattleToastCoroutine(() => InitializeChangeBattleToast(lOldBattlePower)));
        }
        else
        {
            InitializeChangeBattleToast(lOldBattlePower);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadChangeBattleToastCoroutine(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/BattleChangeToast");
        yield return resourceRequest;
        m_goChangeBattleToast = (GameObject)resourceRequest.asset;

        unityAction();
    }

    GameObject goToast;

    //---------------------------------------------------------------------------------------------------
    void InitializeChangeBattleToast(long lOldValue)
    {
        Transform trBattleToast = transform.Find("BattleChangeToast");

        if (trBattleToast != null)
        {
            Destroy(trBattleToast.gameObject);

            if (m_iEnumerator != null)
            {
                StopCoroutine(m_iEnumerator);
                m_iEnumerator = null;
            }
        }

        m_iEnumerator = BattleToastCountingCoroutine(lOldValue);
        StartCoroutine(m_iEnumerator);
    }

    IEnumerator m_iEnumerator;

    //---------------------------------------------------------------------------------------------------
    IEnumerator BattleToastCountingCoroutine(long lOldValue)
    {
        long lChangeValue = CsGameData.Instance.MyHeroInfo.BattlePower;

        int nFrame = 0;
        float nValue = lOldValue;
        float nChangeValue = lChangeValue - lOldValue;
        float fTime = 0;

        goToast = Instantiate(m_goChangeBattleToast, transform);
        goToast.name = "BattleChangeToast";

        Transform trBattlePowerBack = goToast.transform.Find("ImageBackground");

        Text textBattlePower = trBattlePowerBack.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBattlePower);
        textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP_TOAST"), lOldValue.ToString("#,##0"));

        Text textChangeValue = trBattlePowerBack.Find("TextChangeValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textChangeValue);

        Image image = trBattlePowerBack.Find("ImageArrow").GetComponent<Image>();

        if (lChangeValue - lOldValue > 0)
        {
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_up");
            textChangeValue.text = (lChangeValue - lOldValue).ToString("#,##0");
            textChangeValue.color = CsUIData.Instance.ColorGreen;
            CsUIData.Instance.PlayUISound(EnUISoundType.BattlePowerUp);
        }
        else if (lChangeValue - lOldValue < 0)
        {
            image.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_down");
            textChangeValue.text = (lOldValue - lChangeValue).ToString("#,##0");
            textChangeValue.color = CsUIData.Instance.ColorRed;
            CsUIData.Instance.PlayUISound(EnUISoundType.BattlePowerDown);
        }

        CsUIData.Instance.PlayUISound(EnUISoundType.PowerCounting);

        while (nFrame <= 30)
        {
            fTime += Time.deltaTime;
            nValue += nChangeValue / 30f;

            if (nChangeValue > 0)
            {
                if (nValue >= lChangeValue)
                {
                    nValue = lChangeValue;
                }
            }
            else
            {
                if (nValue <= lChangeValue)
                {
                    nValue = lChangeValue;
                }
            }

            textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP_TOAST"), nValue.ToString("#,##0"));
            nFrame++;
            yield return null;
        }
        yield return new WaitForSeconds(Mathf.Abs( CsGameConfig.Instance.BattlePowerToastDisplayDuration - fTime));

        goToast.transform.Find("ImageBackground").gameObject.SetActive(false);
        goToast.transform.Find("FX_level up messege").gameObject.SetActive(true);
        Destroy(goToast, 1f);
    }

    #endregion 전투력 토스트

    #region  컨텐츠 오픈 토스트

	//---------------------------------------------------------------------------------------------------
	IEnumerator OpenSkill(int nSkillId)
	{
		if (m_goToastContentsOpen == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/ContentsOpenToast");
			yield return resourceRequest;
			m_goToastContentsOpen = (GameObject)resourceRequest.asset;
		}

		InitializeSkillOpenToast(nSkillId);

		yield return new WaitForSeconds(CsGameConfig.Instance.ContentOpenToastDisplayDuration);

		CsOpenToastManager.Instance.OpenSkillAnimationFinished();
	}

    //---------------------------------------------------------------------------------------------------
    IEnumerator OpenMenuContent(CsMenuContent csMenuContent)
    {
		if (m_goToastContentsOpen == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/ContentsOpenToast");
			yield return resourceRequest;
			m_goToastContentsOpen = (GameObject)resourceRequest.asset;
		}

		InitializeContentsOpenToast(csMenuContent);

		yield return new WaitForSeconds(CsGameConfig.Instance.ContentOpenToastDisplayDuration);

		CsOpenToastManager.Instance.OpenContentAnimationFinished();
    }

    //---------------------------------------------------------------------------------------------------
	IEnumerator OpenContentsTutorial(CsMenuContent csMenuContent)
    {
		if (m_goTutorial == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Tutorial/PopupTutorial");
			yield return resourceRequest;
			m_goTutorial = (GameObject)resourceRequest.asset;
		}

		if (m_goTutorialNoBack == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Tutorial/PopupTutorialNoBack");
			yield return resourceRequest;
			m_goTutorialNoBack = (GameObject)resourceRequest.asset;
		}

		OpenPopupTutorial(EnTutorialType.OpenContents, csMenuContent);
    }

	//---------------------------------------------------------------------------------------------------
	IEnumerator OpenTypeTutorial(EnTutorialType enTutorialType)
	{
		if (m_goTutorial == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Tutorial/PopupTutorial");
			yield return resourceRequest;
			m_goTutorial = (GameObject)resourceRequest.asset;
		}

		if (m_goTutorialNoBack == null)
		{
			ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Tutorial/PopupTutorialNoBack");
			yield return resourceRequest;
			m_goTutorialNoBack = (GameObject)resourceRequest.asset;
		}

		OpenPopupTutorial(enTutorialType);
	}

    //---------------------------------------------------------------------------------------------------
	void InitializeContentsOpenToast(CsMenuContent csMenuContent)
    {
        GameObject goContentsToast = Instantiate(m_goToastContentsOpen, transform);
        goContentsToast.name = "ConTentsOpenToast";
        Transform trContentsOpenToast = goContentsToast.transform;

        Destroy(goContentsToast, CsGameConfig.Instance.ContentOpenToastDisplayDuration);

        Transform trBack = trContentsOpenToast.Find("ImageBackground");

        Image imageIcon = trBack.Find("ImageIcon").GetComponent<Image>();
		imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csMenuContent.ImageName);

        Text textContent = trBack.Find("TextContent").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContent);
		textContent.text = csMenuContent.Name;

        Text textGuide = trBack.Find("TextGuide").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuide);
		textGuide.text = csMenuContent.Description;
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeSkillOpenToast(int nSkillId)
    {
        GameObject goContentsToast = Instantiate(m_goToastContentsOpen, transform);
        goContentsToast.name = "SkillOpenToast";
        Transform trContentsOpenToast = goContentsToast.transform;

        Destroy(goContentsToast, CsGameConfig.Instance.ContentOpenToastDisplayDuration);

        Transform trBack = trContentsOpenToast.Find("ImageBackground");

        Image imageIcon = trBack.Find("ImageIcon").GetComponent<Image>();
		imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.Job.JobId + "_" + nSkillId);

        Text textContent = trBack.Find("TextContent").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContent);

		CsJobSkill csJobSkill = CsGameData.Instance.GetJobSkill(CsGameData.Instance.MyHeroInfo.Job.JobId, nSkillId);

        if (csJobSkill != null)
        {
            textContent.text = csJobSkill.Name;
        }
        else
        {
            textContent.text = "";
        }

        Text textGuide = trBack.Find("TextGuide").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuide);
        textGuide.text = CsConfiguration.Instance.GetString("A04_TXT_00001");
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupTutorial(EnTutorialType enTutorialType, CsMenuContent csMenuContent = null)
    {
		Transform trCanvas1 = GameObject.Find("Canvas1").transform;

        GameObject goTutorial = null;

        switch (enTutorialType)
        {
            case EnTutorialType.OpenContents:
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                CsGameEventToIngame.Instance.OnEventStartTutorial();

				goTutorial = Instantiate(m_goTutorial, transform);

                if (csMenuContent != null)
                {
                    goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(csMenuContent);

                    if (csMenuContent.ContentId == (int)EnMenuContentId.TodayMission && !CsGameData.Instance.MyHeroInfo.TodayMissionTutorialStarted)
                    {
                        CsCommandEventManager.Instance.SendTodayMissionTutorialStart();
                        Debug.Log(">>>OnEventTodayMissionTutorialStart<<<");
                    }
                }
                else
                {
                    Debug.Log("csMenuContent is null !!!!");
                }
                break;

            case EnTutorialType.FirstStart:
                if (CsGameData.Instance.MyHeroInfo.Level > 1) return;

                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                CsGameEventToIngame.Instance.OnEventStartTutorial();
				goTutorial = Instantiate(m_goTutorial, transform);
                goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(enTutorialType);
                break;

            case EnTutorialType.Attainment:
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
                CsGameEventToIngame.Instance.OnEventStartTutorial();

				goTutorial = Instantiate(m_goTutorial, transform);
                goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(enTutorialType);
                break;

            case EnTutorialType.Cart:
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

				goTutorial = Instantiate(m_goTutorialNoBack, transform);
                goTutorial.GetComponent<CsPopupTutorialNoBack>().SetTutorial(enTutorialType);
                Destroy(goTutorial, 5f);
                break;

            case EnTutorialType.MainGearEnchant:
                CsGameEventToIngame.Instance.OnEventStartTutorial();

				goTutorial = Instantiate(m_goTutorial, transform);
                goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(enTutorialType);
                break;

            case EnTutorialType.Dialog:
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

				goTutorial = Instantiate(m_goTutorial, transform);
                goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(enTutorialType);
                break;

            case EnTutorialType.Interaction:
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();

				goTutorial = Instantiate(m_goTutorial, transform);
                goTutorial.GetComponent<CsPopupTutorial>().SetTutorial(enTutorialType);
                break;
        }
    }

    #endregion

    #region 시스템 토스트

    //---------------------------------------------------------------------------------------------------
    void OpenSystemToast(string strMessage)
    {
        m_queueSystemMessage.Enqueue(strMessage);

        if (m_trSystemToast == null)
        {
            if (m_goSystemToast == null)
            {
                StartCoroutine(LoadSystemToast(() => InitializeSystemToast()));
            }
            else
            {
                InitializeSystemToast();
            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSystemToast(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Toast/SystemToast");
        yield return resourceRequest;
        m_goSystemToast = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeSystemToast()
    {
        string strMessage = m_queueSystemMessage.Dequeue();

        m_trSystemToast = transform.Find("SystemToast");

        if (m_trSystemToast == null)
        {
            m_trSystemToast = Instantiate(m_goSystemToast, transform).transform;
            m_trSystemToast.name = "SystemToast";
            m_trSystemToast.SetAsLastSibling();

            Text textToastMessage = m_trSystemToast.Find("Scroll View/Viewport/Content/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textToastMessage);
            textToastMessage.text = strMessage;

            float flDuration = 0.0f;

            if (strMessage.Length < 45)
            {
                flDuration = 45 * 0.4f;
            }
            else
            {
                flDuration = strMessage.Length * 0.4f;
            }

            m_iEnumeratorSystemToastScrollPositionToast = SystemToastScrollPositionCoroutine(flDuration);
            StartCoroutine(m_iEnumeratorSystemToastScrollPositionToast);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator SystemToastScrollPositionCoroutine(float flDuration)
    {
        float flToastDisplayDuration = flDuration + Time.realtimeSinceStartup;

        ScrollRect scrollRectSystemToast = m_trSystemToast.Find("Scroll View").GetComponent<ScrollRect>();
        scrollRectSystemToast.horizontalNormalizedPosition = 0.0f;

        while (true)
        {
            scrollRectSystemToast.horizontalNormalizedPosition = 1 - ((flToastDisplayDuration - Time.realtimeSinceStartup) / flDuration);

            if (flToastDisplayDuration - Time.realtimeSinceStartup <= 0)
            {
                if (m_queueSystemMessage.Count > 0)
                {
                    string strMessage = m_queueSystemMessage.Dequeue();
                    float flMessageDuration = 0.0f;

                    if (strMessage.Length < 45)
                    {
                        flMessageDuration = 45 * 0.4f;
                    }
                    else
                    {
                        flMessageDuration = strMessage.Length * 0.4f;
                    }

                    Text textToastMessage = m_trSystemToast.Find("Scroll View/Viewport/Content/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textToastMessage);
                    textToastMessage.text = strMessage;

                    if (m_iEnumeratorSystemToastScrollPositionToast != null)
                    {
                        StopCoroutine(m_iEnumeratorSystemToastScrollPositionToast);
                        m_iEnumeratorSystemToastScrollPositionToast = null;
                    }

                    m_iEnumeratorSystemToastScrollPositionToast = SystemToastScrollPositionCoroutine(flMessageDuration);
                    StartCoroutine(m_iEnumeratorSystemToastScrollPositionToast);
                }
                else
                {
                    Destroy(m_trSystemToast.gameObject);
                    m_trSystemToast = null;
                }

                yield break;
            }

            yield return null;
        }
    }

    #endregion 시스템 토스트
}
