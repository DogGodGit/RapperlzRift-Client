using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-03-22)
//---------------------------------------------------------------------------------------------------

public class CsPopupGuildBuilding : CsPopupSub
{
    public enum EnBuilding
    {
        None = 0,
        Lobby = 1,
        Laboratory = 2,
        Shop = 3,
        TankFaction = 4,
    }

    [SerializeField] GameObject m_goGuildSkillItem;

    Transform m_trPanelBuildingList;
    Transform m_trPanelLobby;
    Transform m_trPanelLaboratory;

    Transform m_trSkillContent;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGuildManager.Instance.EventGuildBuildingLevelUp += OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent += OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildFundChanged += OnEventGuildFundChanged;
        CsGuildManager.Instance.EventGuildSkillLevelUp += OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventGuildDonate += OnEventGuildDonate;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGuildManager.Instance.EventGuildBuildingLevelUp -= OnEventGuildBuildingLevelUp;
        CsGuildManager.Instance.EventGuildBuildingLevelUpEvent -= OnEventGuildBuildingLevelUpEvent;
        CsGuildManager.Instance.EventGuildFundChanged -= OnEventGuildFundChanged;
        CsGuildManager.Instance.EventGuildSkillLevelUp -= OnEventGuildSkillLevelUp;
        CsGuildManager.Instance.EventGuildDonate -= OnEventGuildDonate;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        OnClickPopupActive(EnBuilding.None);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPanelBuildingList = transform.Find("PanelBuildingList");
        m_trPanelLobby = transform.Find("PanelLobby");
        m_trPanelLaboratory = transform.Find("PanelLaboratory");
        m_trSkillContent = m_trPanelLaboratory.Find("Scroll View/Viewport/Content");

        DisplayBuildingList();
        DisplayLobby();
        DisplayLaboratory();
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUp()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A59_TXT_02001"));
        UpdateBuildingList();
        UpdateLobby();
        UpdateLaboratory();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildBuildingLevelUpEvent()
    {
        UpdateBuildingList();
        UpdateLobby();
        UpdateLaboratory();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFundChanged()
    {
        UpdateLobby();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildSkillLevelUp()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A59_TXT_02002"));
        UpdateLaboratory();
        UpdateLobby();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventGuildDonate()
    {
        UpdateLaboratory();
        UpdateLobby();
    }

    #endregion Event 

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupActive(EnBuilding enBuilding)
    {
        m_trPanelLobby.gameObject.SetActive(false);
        m_trPanelLaboratory.gameObject.SetActive(false);
        m_trPanelBuildingList.gameObject.SetActive(false);

        switch (enBuilding)
        {
            case EnBuilding.None:
                m_trPanelBuildingList.gameObject.SetActive(true);
                break;
            case EnBuilding.Lobby:
                m_trPanelLobby.gameObject.SetActive(true);
                break;
            case EnBuilding.Laboratory:
                m_trPanelLaboratory.gameObject.SetActive(true);
                break;
            case EnBuilding.Shop:
                m_trPanelLobby.gameObject.SetActive(true);
                break;
            case EnBuilding.TankFaction:
                m_trPanelLobby.gameObject.SetActive(true);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBuildingLevelUp(EnBuilding enBuilding)
    {
        if (CsGuildManager.Instance.MyGuildMemberGrade.GuildBuildingLevelUpEnabled)
        {
            CsGuildManager.Instance.SendGuildBuildingLevelUp((int)enBuilding);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A58_ERROR_001902"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildSkillLevelUp(int nSkillId)
    {
        CsGuildManager.Instance.SendGuildSkillLevelUp(nSkillId);
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void DisplayBuildingList()
    {
        Transform trList = m_trPanelBuildingList.Find("GuildContentList");

        List<CsGuildBuildingInstance> listBuilding = CsGuildManager.Instance.Guild.GuildBuildingInstanceList;

        for (int i = 0; i < listBuilding.Count; ++i)
        {
            int nBuildingId = listBuilding[i].BuildingId;
            Button ButtonGuildContent = trList.Find("ButtonGuildContent" + nBuildingId).GetComponent<Button>();
            ButtonGuildContent.onClick.RemoveAllListeners();
            ButtonGuildContent.onClick.AddListener(() => OnClickPopupActive((EnBuilding)nBuildingId));
            ButtonGuildContent.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Image imageBack = ButtonGuildContent.transform.Find("Image").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/frm_guild_content_" + nBuildingId);

            Text textContentName = ButtonGuildContent.transform.Find("TextContentName").GetComponent<Text>();
            textContentName.text = CsGameData.Instance.GetGuildBuilding(nBuildingId).Name;
            CsUIData.Instance.SetFont(textContentName);

            Text textLevel = ButtonGuildContent.transform.Find("TextLevel").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLevel);
            textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), listBuilding[i].Level);
        }

        UpdateBuildingList();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBuildingList()
    {
        Transform trList = m_trPanelBuildingList.Find("GuildContentList");

        List<CsGuildBuildingInstance> listBuilding = CsGuildManager.Instance.Guild.GuildBuildingInstanceList;

        for (int i = 0; i < listBuilding.Count; ++i)
        {
            int nBuildingId = listBuilding[i].BuildingId;
            Transform trGuildContent = trList.Find("ButtonGuildContent" + nBuildingId);
            Text textLevel = trGuildContent.Find("TextLevel").GetComponent<Text>();
            textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), listBuilding[i].Level);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLobby()
    {
        Button buttonExit = m_trPanelLobby.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(() => OnClickPopupActive(EnBuilding.None));
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
        textExit.text = CsConfiguration.Instance.GetString("A59_BTN_00001");
        CsUIData.Instance.SetFont(textExit);

        Text textLobby = m_trPanelLobby.Find("TextLobby").GetComponent<Text>();
        textLobby.text = CsGameData.Instance.GetGuildBuilding((int)(EnBuilding.Lobby)).Name;
        CsUIData.Instance.SetFont(textLobby);

        Text textGuildFund = m_trPanelLobby.Find("ImageGuildFund/Text").GetComponent<Text>();
        textGuildFund.text = CsConfiguration.Instance.GetString("A59_TXT_00001");
        CsUIData.Instance.SetFont(textGuildFund);

        Text textGuildFundValue = m_trPanelLobby.Find("ImageGuildFund/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textGuildFundValue);

        Text textBuildingPoint = m_trPanelLobby.Find("ImageBuildingPoint/Text").GetComponent<Text>();
        textBuildingPoint.text = CsConfiguration.Instance.GetString("A59_TXT_00002");
        CsUIData.Instance.SetFont(textBuildingPoint);

        Text textBuildingPointValue = m_trPanelLobby.Find("ImageBuildingPoint/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuildingPointValue);

        Text textDescription = m_trPanelLobby.Find("TextDescription").GetComponent<Text>();
        textDescription.text = CsConfiguration.Instance.GetString("A59_TXT_00005");
        CsUIData.Instance.SetFont(textDescription);

        Transform trList = m_trPanelLobby.Find("BuildingList");

        List<CsGuildBuildingInstance> listBuilding = CsGuildManager.Instance.Guild.GuildBuildingInstanceList;

        for (int i = 0; i < listBuilding.Count; ++i)
        {
            int nBuildingId = listBuilding[i].BuildingId;
            Transform trBuilding = trList.Find("BuildingItem" + nBuildingId);

            Image imageBack = trBuilding.Find("Image").GetComponent<Image>();
            imageBack.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/frm_guild_lobby_" + nBuildingId);

            Text textName = trBuilding.Find("TextName").GetComponent<Text>();
            textName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), listBuilding[i].Level, CsGameData.Instance.GetGuildBuilding(nBuildingId).Name);
            CsUIData.Instance.SetFont(textName);

            Text textBuildingPointRequiredValue = trBuilding.Find("BuildingPointRequired/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textBuildingPointRequiredValue);

            Text textUsedFundsValue = trBuilding.Find("UsedFunds/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textUsedFundsValue);

            Button buttonLevelUp = trBuilding.Find("ButtonLevelUp").GetComponent<Button>();
            buttonLevelUp.onClick.RemoveAllListeners();
            buttonLevelUp.onClick.AddListener(() => OnClickBuildingLevelUp((EnBuilding)nBuildingId));
            buttonLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text TextLevelUp = buttonLevelUp.transform.Find("Text").GetComponent<Text>();
            TextLevelUp.text = CsConfiguration.Instance.GetString("A59_BTN_00002");
            CsUIData.Instance.SetFont(TextLevelUp);

            Text textMaxLevel = trBuilding.Find("TextMaxLevel").GetComponent<Text>();
            textMaxLevel.text = CsConfiguration.Instance.GetString("A59_TXT_00007");
            CsUIData.Instance.SetFont(textMaxLevel);
        }

        UpdateLobby();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLobby()
    {
        Text textGuildFundValue = m_trPanelLobby.Find("ImageGuildFund/TextValue").GetComponent<Text>();
        textGuildFundValue.text = CsGuildManager.Instance.Fund.ToString("#,##0");

        Text textBuildingPointValue = m_trPanelLobby.Find("ImageBuildingPoint/TextValue").GetComponent<Text>();
        textBuildingPointValue.text = CsGuildManager.Instance.BuildingPoint.ToString("#,##0");

        Transform trList = m_trPanelLobby.Find("BuildingList");

        List<CsGuildBuildingInstance> listBuilding = CsGuildManager.Instance.Guild.GuildBuildingInstanceList;

        for (int i = 0; i < listBuilding.Count; ++i)
        {
            int nBuildingId = listBuilding[i].BuildingId;
            Transform trBuilding = trList.Find("BuildingItem" + nBuildingId);

            CsGuildBuildingLevel csGuildBuildingLevel = CsGameData.Instance.GetGuildBuilding(nBuildingId).GuildBuildingLevelList.Find(a => a.Level == listBuilding[i].Level);
            //최대레벨 확인용
            CsGuildBuildingLevel csGuildBuildingNextLevel = CsGameData.Instance.GetGuildBuilding(nBuildingId).GuildBuildingLevelList.Find(a => a.Level == listBuilding[i].Level + 1);

            Text textName = trBuilding.Find("TextName").GetComponent<Text>();
            textName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), listBuilding[i].Level, CsGameData.Instance.GetGuildBuilding(nBuildingId).Name);

            Text textBuildingPointRequiredValue = trBuilding.Find("BuildingPointRequired/TextValue").GetComponent<Text>();
            Text textUsedFundsValue = trBuilding.Find("UsedFunds/TextValue").GetComponent<Text>();

            Button buttonLevelUp = trBuilding.Find("ButtonLevelUp").GetComponent<Button>();

            if (csGuildBuildingNextLevel != null)
            {
                bool bBuildingPointCheck = CsGuildManager.Instance.BuildingPoint >= csGuildBuildingLevel.NextLevelUpGuildBuildingPoint;
                bool bFundCheck = CsGuildManager.Instance.Fund >= csGuildBuildingLevel.NextLevelUpGuildFund;

                textBuildingPointRequiredValue.text = csGuildBuildingLevel.NextLevelUpGuildBuildingPoint.ToString("#,##0");
                textUsedFundsValue.text = csGuildBuildingLevel.NextLevelUpGuildFund.ToString("#,##0");
                textBuildingPointRequiredValue.color = bBuildingPointCheck ? CsUIData.Instance.ColorGreen : CsUIData.Instance.ColorRed;
                textUsedFundsValue.color = bFundCheck ? CsUIData.Instance.ColorGreen : CsUIData.Instance.ColorRed;
                CsUIData.Instance.DisplayButtonInteractable(buttonLevelUp, bBuildingPointCheck && bFundCheck);
            }
            else
            {
                buttonLevelUp.gameObject.SetActive(false);
                trBuilding.Find("BuildingPointRequired").gameObject.SetActive(false);
                trBuilding.Find("UsedFunds").gameObject.SetActive(false);
                trBuilding.Find("TextMaxLevel").gameObject.SetActive(true);
            }

			if (nBuildingId == 3 || nBuildingId == 4)
			{
				CsUIData.Instance.DisplayButtonInteractable(buttonLevelUp, false);
			}
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLaboratory()
    {
        Button buttonExit = m_trPanelLaboratory.Find("ButtonExit").GetComponent<Button>();
        buttonExit.onClick.RemoveAllListeners();
        buttonExit.onClick.AddListener(() => OnClickPopupActive(EnBuilding.None));
        buttonExit.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));


        Text textExit = buttonExit.transform.Find("Text").GetComponent<Text>();
        textExit.text = CsConfiguration.Instance.GetString("A59_BTN_00001");
        CsUIData.Instance.SetFont(textExit);

        Text textLaboratory = m_trPanelLaboratory.Find("TextLaboratory").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLaboratory);

        Text textContribution = m_trPanelLaboratory.Find("ImageContribution/Text").GetComponent<Text>();
        textContribution.text = CsConfiguration.Instance.GetString("A59_TXT_00006");
        CsUIData.Instance.SetFont(textContribution);

        Text textContributionValue = m_trPanelLaboratory.Find("ImageContribution/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textContributionValue);

        List<CsGuildSkill> listSkill = CsGameData.Instance.GuildSkillList;

        for (int i = 0; i < listSkill.Count; ++i)
        {
            int nSkillId = listSkill[i].GuildSkillId;

            Transform trSkill = Instantiate(m_goGuildSkillItem, m_trSkillContent).transform;
            trSkill.name = nSkillId.ToString();

            Image imageIcon = trSkill.Find("ImageIcon").GetComponent<Image>();
            imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/guild_skill_" + nSkillId);

            Text textName = trSkill.Find("TextName").GetComponent<Text>();
            textName.text = listSkill[i].Name;
            CsUIData.Instance.SetFont(textName);

            Button buttonSkillUp = trSkill.Find("ButtonSkillUp").GetComponent<Button>();
            buttonSkillUp.onClick.RemoveAllListeners();
            buttonSkillUp.onClick.AddListener(() => OnClickGuildSkillLevelUp(nSkillId));
            buttonSkillUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textSkillUp = buttonSkillUp.transform.Find("Text").GetComponent<Text>();
            textSkillUp.text = CsConfiguration.Instance.GetString("A59_BTN_00003");
            CsUIData.Instance.SetFont(textSkillUp);

            Text textMaxLevel = trSkill.Find("TextMaxLevel").GetComponent<Text>();
            textMaxLevel.text = CsConfiguration.Instance.GetString("A59_TXT_00008");
            CsUIData.Instance.SetFont(textMaxLevel);

            CsUIData.Instance.SetFont(trSkill.Find("ButtonSkillUp/TextValue").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("TextLevel").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("TextSkillLock").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr0/TextName").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr0/TextValue").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr0/TextUpgradeValue").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr1/TextName").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr1/TextValue").GetComponent<Text>());
            CsUIData.Instance.SetFont(trSkill.Find("Attr1/TextUpgradeValue").GetComponent<Text>());
        }

        UpdateLaboratory();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLaboratory()
    {
        int nLaboratoryLevel = CsGuildManager.Instance.Guild.GuildBuildingInstanceList.Find(a => a.BuildingId == (int)EnBuilding.Laboratory).Level;

        Text textLaboratory = m_trPanelLaboratory.Find("TextLaboratory").GetComponent<Text>();
        textLaboratory.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), nLaboratoryLevel, CsGameData.Instance.GetGuildBuilding((int)EnBuilding.Laboratory).Name);

        Text textContributionValue = m_trPanelLaboratory.Find("ImageContribution/TextValue").GetComponent<Text>();
        textContributionValue.text = CsGuildManager.Instance.GuildContributionPoint.ToString("#,##0");

        List<CsGuildSkill> listSkill = CsGameData.Instance.GuildSkillList;

        for (int i = 0; i < listSkill.Count; ++i)
        {
            int nSkillId = listSkill[i].GuildSkillId;
            bool bMaxLevel = false;

            Transform trSkill = m_trSkillContent.Find(nSkillId.ToString());

            Transform trLock = trSkill.Find("ImageIcon/ImageLock");
            Text textSkillLevel = trSkill.Find("TextLevel").GetComponent<Text>();
            Text textSkillLock = trSkill.Find("TextSkillLock").GetComponent<Text>();
            Text textPointValue = trSkill.Find("ButtonSkillUp/TextValue").GetComponent<Text>();

            trLock.gameObject.SetActive(false);
            textSkillLevel.gameObject.SetActive(false);
            textSkillLock.gameObject.SetActive(false);

            CsHeroGuildSkill csHeroGuildSkill = CsGuildManager.Instance.GetHeroGulildSkill(nSkillId);

            //CsGuildSkillLevel csGuildSkillLevel;
            CsGuildSkillLevel csGuildNextSkillLevel;

            int nSkillMaxLevel = nLaboratoryLevel * 10;
            int nSkillLevel = 0;

            //배우지 않은 스킬
            if (csHeroGuildSkill == null)
            {
                csGuildNextSkillLevel = listSkill[i].GuildSkillLevelList.Find(a => a.Level == nSkillLevel + 1);
            }
            else
            {
                //배운 스킬 레벨이 연구소 레벨보다 초과 된 경우
                if (csHeroGuildSkill.Level > nSkillMaxLevel)
                {
                    nSkillLevel = nSkillMaxLevel;
                }
                else
                {
                    nSkillLevel = csHeroGuildSkill.Level;
                }

                //csGuildSkillLevel = listSkill[i].GuildSkillLevelList.Find(a => a.Level == csHeroGuildSkill.Level);
                csGuildNextSkillLevel = listSkill[i].GuildSkillLevelList.Find(a => a.Level == csHeroGuildSkill.Level + 1);

                //최대 레벨
                if (csGuildNextSkillLevel == null)
                {
                    bMaxLevel = true;
                }
            }

            //최대레벨
            if (bMaxLevel)
            {
                trSkill.Find("TextMaxLevel").gameObject.SetActive(true);
                trSkill.Find("ButtonSkillUp").gameObject.SetActive(false);

                for (int j = 0; j < listSkill[i].GuildSkillAttrList.Count; ++j)
                {
                    Transform trAttr = trSkill.Find("Attr" + j);
                    trAttr.gameObject.SetActive(true);

                    Text textAttrName = trAttr.Find("TextName").GetComponent<Text>();
                    textAttrName.text = listSkill[i].GuildSkillAttrList[j].Attr.Name;

                    Text textValue = trAttr.Find("TextValue").GetComponent<Text>();
                    textValue.text = listSkill[i].GuildSkillAttrList[j].GuildSkillLevelAttrValueList.Find(a => a.Level == nSkillLevel).AttrValue.Value.ToString("#,##0");

                    trAttr.Find("TextUpgradeValue").gameObject.SetActive(false);
                    trAttr.Find("ImageAttr").gameObject.SetActive(false);
                }
            }
            else
            {
                //제한 조건 체크
                bool bRequiredLaboratoryLevelCheck = CsGuildManager.Instance.Guild.GetGuildBuildingInstance((int)EnBuilding.Laboratory).Level >= csGuildNextSkillLevel.RequiredLaboratoryLevel;
                bool bRequiredGuildContributionPointCheck = CsGuildManager.Instance.GuildContributionPoint >= csGuildNextSkillLevel.RequiredGuildContributionPoint;

                if (!bRequiredLaboratoryLevelCheck && nSkillLevel == 0)
                {
                    textSkillLock.text = string.Format(CsConfiguration.Instance.GetString("A59_TXT_01002"), csGuildNextSkillLevel.RequiredLaboratoryLevel);
                    textSkillLock.gameObject.SetActive(true);
                    trLock.gameObject.SetActive(true);
                }
                else
                {
                    textSkillLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nSkillLevel, nSkillMaxLevel);
                    textSkillLevel.gameObject.SetActive(true);
                }

                for (int j = 0; j < listSkill[i].GuildSkillAttrList.Count; ++j)
                {
                    Transform trAttr = trSkill.Find("Attr" + j);
                    trAttr.gameObject.SetActive(true);

                    Text textAttrName = trAttr.Find("TextName").GetComponent<Text>();
                    textAttrName.text = listSkill[i].GuildSkillAttrList[j].Attr.Name;

                    Text textValue = trAttr.Find("TextValue").GetComponent<Text>();
                    Text textUpgradeValue = trAttr.Find("TextUpgradeValue").GetComponent<Text>();

                    if (nSkillLevel == 0)
                    {
                        textValue.text = "0";
                        textUpgradeValue.text = listSkill[i].GuildSkillAttrList[j].GuildSkillLevelAttrValueList.Find(a => a.Level == nSkillLevel + 1).AttrValue.Value.ToString("#,##0");
                    }
                    else
                    {
                        int nValue = listSkill[i].GuildSkillAttrList[j].GuildSkillLevelAttrValueList.Find(a => a.Level == nSkillLevel).AttrValue.Value;
                        int nNextValue = listSkill[i].GuildSkillAttrList[j].GuildSkillLevelAttrValueList.Find(a => a.Level == nSkillLevel + 1).AttrValue.Value - nValue;
                        textValue.text = nValue.ToString("#,##0");
                        textUpgradeValue.text = nNextValue.ToString("#,##0");
                    }
                }


                Button buttonSkillUp = trSkill.Find("ButtonSkillUp").GetComponent<Button>();
                textPointValue.text = csGuildNextSkillLevel.RequiredGuildContributionPoint.ToString("#,##0");
                Image ImageIcon = buttonSkillUp.transform.Find("Image").GetComponent<Image>();

                if (bRequiredGuildContributionPointCheck && bRequiredLaboratoryLevelCheck)
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonSkillUp, true);
                    textPointValue.color = CsUIData.Instance.ColorWhite;
                    ImageIcon.color = CsUIData.Instance.ColorButtonOn;
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(buttonSkillUp, false);
                    textPointValue.color = CsUIData.Instance.ColorGray;
                    ImageIcon.color = CsUIData.Instance.ColorButtonOff;
                }

            }
        }
    }
}
