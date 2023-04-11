using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-24)
//---------------------------------------------------------------------------------------------------

public class CsCharacterInfo : CsPopupSub
{
    Transform m_trDetailInfo;
    Transform m_trBackground;
    Transform m_trPopupList;
    Text m_textJobName;
    Text m_textGuildName;
    Text m_textTitle;
    Text m_textExploit;
    Text m_textDayExploit;
    Text m_textPartner;
    Text m_textWeeklyFame;
    Text m_textCharacterID;
    Slider m_sliderExp;
    Text m_textExpValue;

    GameObject m_goCharacterDetailInfo;

    bool m_bIsFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMainGearEquip += OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip += OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip += OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip += OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventMyHeroExpUp += OnEventMyHeroExpUp;

        CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        //m_trBackground.gameObject.SetActive(true);

        if (!m_bIsFirst)
        {
            UpdateCharacterInfo();

            if (m_trDetailInfo != null)
            {
                UpdateDetailInfo();
            }
        }

        OnClickTip();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        //m_trDetailInfo.gameObject.SetActive(false);
        OnClickCloseDetailInfo();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        if (m_trDetailInfo != null)
        {
            Destroy(m_trDetailInfo.gameObject);
        }

        CsGameEventUIToUI.Instance.EventMainGearEquip -= OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip -= OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip -= OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip -= OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventMyHeroExpUp -= OnEventMyHeroExpUp;

        CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;
    }


    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChange()
    {
        UpdateJob();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroExpUp()
    {
        UpdateExp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEquip(Guid guidHeroGearId)
    {
        UpdateBaseState();
        OnClickTip();

        if (m_trDetailInfo != null)
        {
            UpdateDetailInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearUnequip(Guid guidHeroGearId)
    {
        UpdateBaseState();
        OnClickTip();

        if (m_trDetailInfo != null)
        {
            UpdateDetailInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearEquip(int nSubGearId)
    {
        UpdateBaseState();
        OnClickTip();

        if (m_trDetailInfo != null)
        {
            UpdateDetailInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearUnequip(int nSubGearId)
    {
        UpdateBaseState();
        OnClickTip();

        if (m_trDetailInfo != null)
        {
            UpdateDetailInfo();
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trBackground = transform.Find("ImageBackground");
        m_trPopupList = GameObject.Find("Canvas2/PopupList").transform;

        //직업버튼
        Button buttonJob = m_trBackground.Find("ButtonJob").GetComponent<Button>();
        buttonJob.onClick.RemoveAllListeners();
        buttonJob.onClick.AddListener(OnClickJob);
        buttonJob.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textJob = buttonJob.transform.Find("TextJob").GetComponent<Text>();
        CsUIData.Instance.SetFont(textJob);
        textJob.text = CsConfiguration.Instance.GetString("A05_TXT_00001");

        m_textJobName = buttonJob.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textJobName);

        UpdateJob();

        //국가버튼
        Button buttonNation = m_trBackground.Find("ButtonNation").GetComponent<Button>();
        buttonNation.onClick.RemoveAllListeners();
        buttonNation.onClick.AddListener(OnClickNation);
        buttonNation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Image imageNation = buttonNation.transform.Find("ImageIcon").GetComponent<Image>();
        imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + CsGameData.Instance.MyHeroInfo.Nation.NationId);

        Text textNation = buttonNation.transform.Find("TextNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNation);
        textNation.text = CsConfiguration.Instance.GetString("A05_TXT_00002");

        Text textNationName = buttonNation.transform.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNationName);
        CsNation csNation = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.Nation.NationId);
        textNationName.text = csNation.Name;

        Transform trInfoList = m_trBackground.Find("InfoList");

        //길드
        Transform trGuild = trInfoList.Find("ImageGuild");
        Text textGuild = trGuild.Find("TextName").GetComponent<Text>();
        textGuild.text = CsConfiguration.Instance.GetString("A05_TXT_00003");
        CsUIData.Instance.SetFont(textGuild);

        m_textGuildName = trGuild.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuildName);

        //칭호
        Transform trTitle = trInfoList.Find("ImageTitle");
        Text textTitle = trTitle.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTitle);
        textTitle.text = CsConfiguration.Instance.GetString("A05_TXT_00007");

        m_textTitle = trTitle.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTitle);

        //공적
        Transform trExploit = trInfoList.Find("ImageExploit");
        Text textExploit = trExploit.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExploit);
        textExploit.text = CsConfiguration.Instance.GetString("A05_TXT_00004");

        m_textExploit = trExploit.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textExploit);

        //일일공적
        Transform trDayExploit = trInfoList.Find("ImageDayExploit");
        Text textDayExploit = trDayExploit.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDayExploit);
        textDayExploit.text = CsConfiguration.Instance.GetString("A05_TXT_00008");

        m_textDayExploit = trDayExploit.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDayExploit);

        //파트너
        Transform trPartner = trInfoList.Find("ImagePartner");
        Text textPartner = trPartner.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPartner);
        textPartner.text = CsConfiguration.Instance.GetString("A05_TXT_00005");

        m_textPartner = trPartner.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPartner);

        //금주 인기
        Transform trWeeklyFame = trInfoList.Find("ImageWeeklyFame");
        Text textWeeklyFame = trWeeklyFame.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textWeeklyFame);
        textWeeklyFame.text = CsConfiguration.Instance.GetString("A05_TXT_00009");

        m_textWeeklyFame = trWeeklyFame.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWeeklyFame);

        //번호
        Transform trCharacterID = trInfoList.Find("ImageID");
        Text textCharacterID = trCharacterID.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCharacterID);
        textCharacterID.text = CsConfiguration.Instance.GetString("A05_TXT_00006");

        m_textCharacterID = trCharacterID.Find("TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCharacterID);

        //스텟리스트
        Text textAttrList = m_trBackground.Find("TextAttrInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrList);
        textAttrList.text = CsConfiguration.Instance.GetString("A05_NAME_00001");

        Transform trAttrList = m_trBackground.Find("AttrList");

        for (int i = 0; i < trAttrList.childCount; i++)
        {
            Text textName = trAttrList.Find("Attr" + i + "/TextAttrName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);

            //임시 이름
            textName.text = CsGameData.Instance.GetAttr(i + 1).Name;

            Text textValue = trAttrList.Find("Attr" + i + "/TextAttrValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textValue);

            //임시값
            if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 1))
            {
                textValue.text = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 1].ToString("#,##0");
            }
            else
            {
                textValue.text = "0";
            }

            switch (i)
            {
                case 0:
                    //textName.text = CsConfiguration.Instance.GetString("");
                    break;
                case 1:
                    //textName.text = CsConfiguration.Instance.GetString("");
                    break;
                case 2:
                    //textName.text = CsConfiguration.Instance.GetString("");
                    break;
                case 3:
                    //textName.text = CsConfiguration.Instance.GetString("");
                    break;
                case 4:
                    //textName.text = CsConfiguration.Instance.GetString("");
                    break;
            }
        }

        //상세정보 버튼
        Button buttonDetailInfo = m_trBackground.Find("ButtonDetailInfo").GetComponent<Button>();
        buttonDetailInfo.onClick.RemoveAllListeners();
        buttonDetailInfo.onClick.AddListener(OnClickDetailInfo);
        buttonDetailInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textDetailInfo = buttonDetailInfo.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDetailInfo);
        textDetailInfo.text = CsConfiguration.Instance.GetString("A05_TXT_00010");

        //경험치 슬라이더
        Text textExp = m_trBackground.Find("TextExpName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExp);
        textExp.text = CsConfiguration.Instance.GetString("A05_TXT_00011");

        m_sliderExp = m_trBackground.Find("SliderExp").GetComponent<Slider>();
        m_textExpValue = m_sliderExp.transform.Find("TextExpValue").GetComponent<Text>();

        //팁
        Button buttonTip = m_trBackground.Find("ButtonTip").GetComponent<Button>();
        buttonTip.onClick.RemoveAllListeners();
        buttonTip.onClick.AddListener(OnClickTip);
        buttonTip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textTip = buttonTip.transform.Find("ImageBackground/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTip);

        UpdateCharacterInfo();
        m_bIsFirst = false;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterInfo()
    {
        UpdateGuildName();
        UpdateTitle();
        UpdateExploit();
        UpdateDayExploit();
        UpdatePartner();
        UpdateWeeklyFame();
        UpdateCharacterID();
        UpdateBaseState();
        UpdateExp();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJob()
    {
        Transform trTip = m_trBackground.Find("ButtonTip");
        Text textTip = trTip.Find("ImageBackground/Text").GetComponent<Text>();
        textTip.text = CsGameData.Instance.MyHeroInfo.Job.Description;
        trTip.gameObject.SetActive(true);
        Debug.Log("직업버튼 클릭");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNation()
    {
        Transform trTip = m_trBackground.Find("ButtonTip");
        Text textTip = trTip.Find("ImageBackground/Text").GetComponent<Text>();
        textTip.text = CsGameData.Instance.MyHeroInfo.Nation.Description;
        trTip.gameObject.SetActive(true);
        Debug.Log("나라버튼 클릭");
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDetailInfo()
    {
        if (m_trDetailInfo == null)
        {
            if (m_goCharacterDetailInfo == null)
            {
                StartCoroutine(LoadCharacterDetailCoroutine());
            }
            else
            {
                GameObject goDetail = Instantiate(m_goCharacterDetailInfo, m_trPopupList);
                goDetail.name = "CharacterDetailInfo";
                m_trDetailInfo = goDetail.transform;

                InitialIzeDetailInfo();
            }
        }
        else
        {
            m_trDetailInfo.gameObject.SetActive(true);
            UpdateDetailInfo();
        }

        m_trBackground.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCharacterDetailCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/CharacterDetailInfo");
        yield return resourceRequest;
        m_goCharacterDetailInfo = (GameObject)resourceRequest.asset;

        GameObject goDetail = Instantiate(m_goCharacterDetailInfo, m_trPopupList);
        goDetail.name = "CharacterDetailInfo";
        m_trDetailInfo = goDetail.transform;

        InitialIzeDetailInfo();
    }


    //---------------------------------------------------------------------------------------------------
    void UpdateJob()
    {
        Image imageJob = m_trBackground.Find("ButtonJob/ImageIcon").GetComponent<Image>();
        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + CsGameData.Instance.MyHeroInfo.Job.JobId);

        CsJob csJob = CsGameData.Instance.GetJob(CsGameData.Instance.MyHeroInfo.Job.JobId);
        m_textJobName.text = csJob.Name;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildName()
    {
        if (CsGuildManager.Instance.GuildId == Guid.Empty)
        {
            m_textGuildName.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
        }
        else
        {
            m_textGuildName.text = CsGuildManager.Instance.GuildName;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTitle()
    {
        m_textTitle.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExploit()
    {
        m_textExploit.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDayExploit()
    {
        m_textDayExploit.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePartner()
    {
        m_textPartner.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWeeklyFame()
    {
        m_textWeeklyFame.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterID()
    {
        m_textCharacterID.text = CsConfiguration.Instance.GetString("A05_TXT_00012");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBaseState()
    {
        Transform trAttrList = m_trBackground.Find("AttrList");

        for (int i = 0; i < trAttrList.childCount; i++)
        {
            Text textValue = trAttrList.Find("Attr" + i + "/TextAttrValue").GetComponent<Text>();

            if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 1))
            {
				float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 1];

				flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 1));
				flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 1));

				textValue.text = flValue.ToString("#,##0");
            }
            else
            {
                textValue.text = "0";
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExp()
    {
        m_sliderExp.maxValue = CsGameData.Instance.MyHeroInfo.RequiredExp;
        m_sliderExp.value = CsGameData.Instance.MyHeroInfo.Exp;
        m_textExpValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), CsGameData.Instance.MyHeroInfo.Exp.ToString("#,###"), CsGameData.Instance.MyHeroInfo.RequiredExp.ToString("#,###"));
    }

    //---------------------------------------------------------------------------------------------------
    void InitialIzeDetailInfo()
    {
        Transform trBackground = m_trDetailInfo.Find("ImageBackground");

        Text textDetailInfo = trBackground.Find("TextDetailInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDetailInfo);
        textDetailInfo.text = CsConfiguration.Instance.GetString("A05_NAME_00002");

        Button buttonClose = trBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickCloseDetailInfo);

        //상세속성 리스트
        Transform trDetailAttrList = trBackground.Find("DetailAttrList");

        for (int i = 0; i < trDetailAttrList.childCount; i++)
        {
            //int ntrDetailAttrIndex = i;
            Transform trAttr = trDetailAttrList.Find("Attr" + i);

            Text textAttrName = trAttr.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);

            Text textAttrValue = trAttr.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);

            if (i < 2)
            {
                textAttrName.text = CsGameData.Instance.GetAttr(i + 6).Name;

                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 6))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 6];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 6));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 6));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
            else if (i < 4)
            {
                textAttrName.text = CsGameData.Instance.GetAttr(i + 8).Name;

                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 8))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 8];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 8));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 8));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
            else
            {
                textAttrName.text = CsGameData.Instance.GetAttr(i + 16).Name;

                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 16))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 16];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 16));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 16));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
        }

        //원소속성리스트
        Transform trElementAttrList = trBackground.Find("ElementAttrList");

        for (int i = 0; i < trElementAttrList.childCount; i++)
        {
            //int nElementIndex = i;
            Transform trElementAttr = trElementAttrList.Find("ElementAttr" + i);

            Text textAttrName = trElementAttr.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);

            //임시 이름
            textAttrName.text = CsGameData.Instance.GetAttr(i + 12).Name;

            Text textAttrValue = trElementAttr.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);

            if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 12))
            {
				float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 12];

				flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 12));
				flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 12));

				textAttrValue.text = flValue.ToString("#,##0");
            }
            else
            {
                textAttrValue.text = "0";
            }

            Image imageIcon = trElementAttr.Find("ImageIcon").GetComponent<Image>();

            switch (i)
            {
                case 0:
                case 1:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCharacter/ico_skill_property01");
                    break;
                case 2:
                case 3:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCharacter/ico_skill_property02");
                    break;
                case 4:
                case 5:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCharacter/ico_skill_property03");
                    break;
                case 6:
                case 7:
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCharacter/ico_skill_property04");
                    break;
            }
        }

        UpdateDetailInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateDetailInfo()
    {
        Transform trBackground = m_trDetailInfo.Find("ImageBackground");

        //상세속성 리스트
        Transform trDetailAttrList = trBackground.Find("DetailAttrList");

        for (int i = 0; i < trDetailAttrList.childCount; i++)
        {
            //int ntrDetailAttrIndex = i;
            Transform trAttr = trDetailAttrList.Find("Attr" + i);

            Text textAttrValue = trAttr.Find("TextValue").GetComponent<Text>();

            if (i < 2)
            {
                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 6))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 6];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 6));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 6));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
            else if (i < 4)
            {
                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 8))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 8];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 8));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 8));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 16))
                {
					float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 16];

					flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 16));
					flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 16));

					textAttrValue.text = flValue.ToString("#,##0");
                }
                else
                {
                    textAttrValue.text = "0";
                }
            }
        }

        //원소속성리스트
        Transform trElementAttrList = trBackground.Find("ElementAttrList");

        for (int i = 0; i < trElementAttrList.childCount; i++)
        {
            //int nElementIndex = i;
            Transform trElementAttr = trElementAttrList.Find("ElementAttr" + i);


            Text textAttrValue = trElementAttr.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);

            if (CsGameData.Instance.MyHeroInfo.DicAttrValue.ContainsKey(i + 12))
            {
				float flValue = CsGameData.Instance.MyHeroInfo.DicAttrValue[i + 12];

				flValue *= CsBuffDebuffManager.Instance.GetBuffDebuffAttrFactor(CsGameData.Instance.GetAttr(i + 12));
				flValue += CsBuffDebuffManager.Instance.GetBuffDebuffAttrValue(CsGameData.Instance.GetAttr(i + 12));

				textAttrValue.text = flValue.ToString("#,##0");
            }
            else
            {
                textAttrValue.text = "0";
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseDetailInfo()
    {
        m_trBackground.gameObject.SetActive(true);

        if (m_trDetailInfo != null)
        {
            m_trDetailInfo.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTip()
    {
        if (m_trBackground != null)
        {
            Transform trTip = m_trBackground.Find("ButtonTip");

            if (trTip != null)
            {
                trTip.gameObject.SetActive(false);
            }
        }
    }
}
