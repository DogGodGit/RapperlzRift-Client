using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-02-27)
//---------------------------------------------------------------------------------------------------

public class CsPopupViewOtherUsers : CsPopupSub
{
    [SerializeField] GameObject m_goItemSlot;
    [SerializeField] GameObject m_goAttrValue;

    Transform m_trEquipment;
    Transform m_trCharacterInfo;
    Transform m_trPopupList;
    Transform m_trItemInfoLeft;

    CsHeroMainGear m_csHeroMainGearSelect;
    CsHeroSubGear m_csHeroSubGearSelect;

    GameObject m_goPopupSetInfo;
    GameObject m_goPopupItemInfo;

    CsHeroInfo m_csHeroInfo;
    CsPopupItemInfo m_csPopupItemInfo;
    Camera m_uiCamera;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    protected override void OnFinalize()
    {
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trItemInfoLeft != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Left);
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickGearEquipped(int nSlotIndex)
    {
        if (nSlotIndex == (int)EnMainGearSlotIndex.Weapon)
        {
            m_csHeroMainGearSelect = m_csHeroInfo.HeroMainGearEquippedWeapon;

            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfoMainGear));
            }
            else
            {
                OpenPopupItemInfoMainGear();
            }
        }
        else if (nSlotIndex == (int)EnMainGearSlotIndex.Armor)
        {
            m_csHeroMainGearSelect = m_csHeroInfo.HeroMainGearEquippedArmor;

            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfoMainGear));
            }
            else
            {
                OpenPopupItemInfoMainGear();
            }
        }
        else
        {
            m_csHeroSubGearSelect = m_csHeroInfo.EquippedHeroSubGearsList.Find(a => a.SubGear.SlotIndex == nSlotIndex);

            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfoSubGear));
            }
            else
            {
                OpenPopupItemInfoSubGear();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingInfo()
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfoWing));
        }
        else
        {
            OpenPopupItemInfoWing();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickFriend()
    {
        // 보낸 리스트
        if (CsFriendManager.Instance.FriendApplicationList.Find(a => a.TargetId == m_csHeroInfo.HeroId) != null)
        {
            // 이미 신청을 보냄
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02006"));
        }
        else if (CsFriendManager.Instance.FriendList.Find(a => a.Id == m_csHeroInfo.HeroId) != null)
        {
            // 이미 친구
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02007"));
        }
        else if (CsGameConfig.Instance.FriendMaxCount <= CsFriendManager.Instance.FriendList.Count)
        {
            // 친구 Max
        }
        else
        {
            if (CsFriendManager.Instance.BlacklistEntryList.Find(a => a.HeroId == m_csHeroInfo.HeroId) != null)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A108_TXT_02011"));
            }
            else
            {
                // 신청 보냄
                CsFriendManager.Instance.SendFriendApply(m_csHeroInfo.HeroId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOneToOne()
    {
        CsGameEventUIToUI.Instance.OnEventOpenOneToOneChat(m_csHeroInfo.HeroId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTip()
    {
        Transform trTip = m_trCharacterInfo.Find("ButtonTip");

        if (trTip != null)
        {
            trTip.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickJob()
    {
        Transform trTip = m_trCharacterInfo.Find("ButtonTip");
        Text textTip = trTip.Find("ImageBackground/Text").GetComponent<Text>();
        textTip.text = m_csHeroInfo.Job.Description;
        trTip.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNation()
    {
        Transform trTip = m_trCharacterInfo.Find("ButtonTip");
        Text textTip = trTip.Find("ImageBackground/Text").GetComponent<Text>();
        textTip.text = m_csHeroInfo.Nation.Description;
        trTip.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSoultoneSet()
    {
        if (m_goPopupSetInfo == null)
        {
            StartCoroutine(LoadPopupSetInfo(EnPopupSetInfoType.SubGear));
        }
        else
        {
            OpenPopupSetInfo(EnPopupSetInfoType.SubGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickEnchantSet()
    {
        if (m_goPopupSetInfo == null)
        {
            StartCoroutine(LoadPopupSetInfo(EnPopupSetInfoType.MainGear));
        }
        else
        {
            OpenPopupSetInfo(EnPopupSetInfoType.MainGear);
        }
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    public void DisplaySetting(CsHeroInfo csHeroInfo)
    {
        InitializeUI();
        m_csHeroInfo = csHeroInfo;

        if (m_csHeroInfo != null)
        {
            LoadCharacterModel();
            UpdateInfo();
            UpdateWing();
            UpdateMainGearEuquipped();
            UpdateSubGearEquipped();
            UpdateAttr();
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        m_trEquipment = transform.Find("Equipment");
        m_uiCamera = m_trEquipment.Find("3DCharacter/UIChar_Camera").GetComponent<Camera>();
        m_trCharacterInfo = transform.Find("CharacterInfo");

        Text textAttrInfo = m_trCharacterInfo.Find("TextAttrInfo").GetComponent<Text>();
        textAttrInfo.text = CsConfiguration.Instance.GetString("A15_TXT_00007");
        CsUIData.Instance.SetFont(textAttrInfo);

        Button buttonWing = m_trEquipment.Find("ItemList/Equip0/ButtonWing").GetComponent<Button>();
        buttonWing.onClick.RemoveAllListeners();
        buttonWing.onClick.AddListener(OnClickWingInfo);
        buttonWing.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //친구추가
        Button buttonFriend = m_trCharacterInfo.Find("ButtonFriend").GetComponent<Button>();
        buttonFriend.onClick.RemoveAllListeners();
        buttonFriend.onClick.AddListener(OnClickFriend);
        buttonFriend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textFriend = buttonFriend.transform.Find("Text").GetComponent<Text>();
        textFriend.text = CsConfiguration.Instance.GetString("A15_BTN_00001");
        CsUIData.Instance.SetFont(textFriend);

        //귓속말
        Button buttonOneToOne = m_trCharacterInfo.Find("ButtonOneToOne").GetComponent<Button>();
        buttonOneToOne.onClick.RemoveAllListeners();
        buttonOneToOne.onClick.AddListener(OnClickOneToOne);
        buttonOneToOne.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textOneToOne = buttonOneToOne.transform.Find("Text").GetComponent<Text>();
        textOneToOne.text = CsConfiguration.Instance.GetString("A15_BTN_00002");
        CsUIData.Instance.SetFont(textOneToOne);

        //팁
        Button buttonTip = m_trCharacterInfo.Find("ButtonTip").GetComponent<Button>();
        buttonTip.onClick.RemoveAllListeners();
        buttonTip.onClick.AddListener(OnClickTip);
        buttonTip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textTip = buttonTip.transform.Find("ImageBackground/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTip);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInfo()
    {
        Text textLevelName = m_trEquipment.Find("OtherUsers/TextLevelName").GetComponent<Text>();
        textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), m_csHeroInfo.Level, m_csHeroInfo.Name);
        CsUIData.Instance.SetFont(textLevelName);

        Text textBattlePower = m_trEquipment.Find("OtherUsers/TextBattlePower").GetComponent<Text>();
        textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), m_csHeroInfo.BattlePower.ToString("#,###"));
        CsUIData.Instance.SetFont(textBattlePower);

        //세트효과
        Button buttonSetEquipment = m_trEquipment.Find("ButtonSetEquipment").GetComponent<Button>();
        buttonSetEquipment.onClick.RemoveAllListeners();

        if (m_csHeroInfo.MainGearEnchantLevelSetNo == 0)
        {
            buttonSetEquipment.gameObject.SetActive(false);
        }
        else
        {
            buttonSetEquipment.onClick.AddListener(OnClickEnchantSet);
            buttonSetEquipment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        Button buttonSoulStone = m_trEquipment.Find("ButtonSoulStone").GetComponent<Button>();
        buttonSoulStone.onClick.RemoveAllListeners();

        if (m_csHeroInfo.SubGearSoulstoneLevelSetNo == 0)
        {
            buttonSoulStone.gameObject.SetActive(false);
        }
        else
        {
            buttonSoulStone.onClick.AddListener(OnClickSoultoneSet);
            buttonSoulStone.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }

        //직업
        Button buttonJob = m_trCharacterInfo.Find("ButtonJob").GetComponent<Button>();
        buttonJob.onClick.RemoveAllListeners();
        buttonJob.onClick.AddListener(OnClickJob);
        buttonJob.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonJob.gameObject.SetActive(true);

        Image imageJob = buttonJob.transform.Find("ImageIcon").GetComponent<Image>();
        imageJob.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + m_csHeroInfo.Job.JobId);

        Text textJob = buttonJob.transform.Find("TextJob").GetComponent<Text>();
        textJob.text = CsConfiguration.Instance.GetString("A15_TXT_00001");
        CsUIData.Instance.SetFont(textJob);

        Text textJobName = buttonJob.transform.Find("TextValue").GetComponent<Text>();
        textJobName.text = m_csHeroInfo.Job.Name;
        CsUIData.Instance.SetFont(textJobName);

        //국가
        Button buttonNation = m_trCharacterInfo.Find("ButtonNation").GetComponent<Button>();
        buttonNation.onClick.RemoveAllListeners();
        buttonNation.onClick.AddListener(OnClickNation);
        buttonNation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonNation.gameObject.SetActive(true);

        Image imageNation = buttonNation.transform.Find("ImageIcon").GetComponent<Image>();
        imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + m_csHeroInfo.Nation.NationId);

        Text textNation = buttonNation.transform.Find("TextNation").GetComponent<Text>();
        textNation.text = CsConfiguration.Instance.GetString("A15_TXT_00002");
        CsUIData.Instance.SetFont(textNation);

        Text textNationName = buttonNation.transform.Find("TextValue").GetComponent<Text>();
        textNationName.text = m_csHeroInfo.Nation.Name;
        CsUIData.Instance.SetFont(textNationName);

        Transform trInfoList = m_trCharacterInfo.Find("InfoList");

        //길드
        Text textGuildName = trInfoList.Find("ImageGuild/TextName").GetComponent<Text>();
        textGuildName.text = CsConfiguration.Instance.GetString("A15_TXT_00003");
        CsUIData.Instance.SetFont(textGuildName);

        Text textGuildValue = trInfoList.Find("ImageGuild/TextValue").GetComponent<Text>();
        //길드 나오면 수정
        if (m_csHeroInfo.GuildId == System.Guid.Empty)
        {
            textGuildValue.text = CsConfiguration.Instance.GetString("A15_TXT_00009");
        }
        else
        {
            textGuildValue.text = m_csHeroInfo.GuildName;
        }
        CsUIData.Instance.SetFont(textGuildValue);

        //계급
        Text textClassName = trInfoList.Find("ImageClass/TextName").GetComponent<Text>();
        textClassName.text = CsConfiguration.Instance.GetString("A15_TXT_00004");
        CsUIData.Instance.SetFont(textClassName);

        Text textClassValue = trInfoList.Find("ImageClass/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textClassValue);
        textClassValue.text = m_csHeroInfo.RankNo == 0 ? CsConfiguration.Instance.GetString("A15_TXT_00009") : CsGameData.Instance.GetRank(m_csHeroInfo.RankNo).Name;

        //번호
        Text textIDName = trInfoList.Find("ImageID/TextName").GetComponent<Text>();
        textIDName.text = CsConfiguration.Instance.GetString("A15_TXT_00005");
        CsUIData.Instance.SetFont(textIDName);

        Text textIDValue = trInfoList.Find("ImageID/TextValue").GetComponent<Text>();
        //추후 변경
        textIDValue.text = CsConfiguration.Instance.GetString("A15_TXT_00009");
        CsUIData.Instance.SetFont(textIDValue);

        //금주 인기도
        Text textPopularityName = trInfoList.Find("ImagePopularity/TextName").GetComponent<Text>();
        textPopularityName.text = CsConfiguration.Instance.GetString("A15_TXT_00006");
        CsUIData.Instance.SetFont(textPopularityName);

        Text textPopularityValue = trInfoList.Find("ImagePopularity/TextValue").GetComponent<Text>();
        //추후 변경
        textPopularityValue.text = CsConfiguration.Instance.GetString("A15_TXT_00009");
        CsUIData.Instance.SetFont(textPopularityValue);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWing()
    {
        Transform trItemList = m_trEquipment.Find("ItemList");
        Transform trEquipSlot = trItemList.Find("Equip0");
        Button buttonWing = trEquipSlot.Find("ButtonWing").GetComponent<Button>();
        Image imageButtonWing = buttonWing.transform.Find("ImageIcon").GetComponent<Image>();

        if (m_csHeroInfo.EquippedWingId == 0)
        {
            buttonWing.gameObject.SetActive(false);
        }
        else
        {
            buttonWing.gameObject.SetActive(true);
            imageButtonWing.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/Wing_" + m_csHeroInfo.EquippedWingId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttr()
    {
        Transform trAttrList = m_trCharacterInfo.Find("AttrList/Viewport/Content");

        for (int i = 0; i < m_csHeroInfo.RealAttrValuesList.Count; ++i)
        {
            Transform trAttr = trAttrList.Find("Attr" + i);

            if (trAttr == null)
            {
                trAttr = Instantiate(m_goAttrValue, trAttrList).transform;
                trAttr.name = m_goAttrValue.name + i;
                Text textAttrName = trAttr.Find("TextAttrName").GetComponent<Text>();
                textAttrName.text = m_csHeroInfo.RealAttrValuesList[i].Attr.Name;
                CsUIData.Instance.SetFont(textAttrName);

                Text textAttrValue = trAttr.Find("TextAttrValue").GetComponent<Text>();
                textAttrValue.text = m_csHeroInfo.RealAttrValuesList[i].Value.ToString("#,##0");
                CsUIData.Instance.SetFont(textAttrValue);
            }
            else
            {
                Text textAttrName = trAttr.Find("TextAttrName").GetComponent<Text>();
                textAttrName.text = m_csHeroInfo.RealAttrValuesList[i].Attr.Name;
                CsUIData.Instance.SetFont(textAttrName);

                Text textAttrValue = trAttr.Find("TextAttrValue").GetComponent<Text>();
                textAttrValue.text = m_csHeroInfo.RealAttrValuesList[i].Value.ToString("#,##0");
                CsUIData.Instance.SetFont(textAttrValue);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearEuquipped()
    {
        Transform trItemList = m_trEquipment.Find("ItemList");

        //Transform trItemSlotWeapown = trItemList.Find("Equip" + (int)EnMainGearSlotIndex.Weapon + "/ItemSlot");

        CsHeroMainGear csHeroMainGear = m_csHeroInfo.HeroMainGearEquippedWeapon;

        Transform trEquipSlot;
        Transform trItemSlot;

        if (csHeroMainGear != null)
        {
            int nSlotIndex = csHeroMainGear.MainGear.MainGearType.SlotIndex;
            trEquipSlot = trItemList.Find("Equip" + nSlotIndex);
            trItemSlot = trEquipSlot.Find("ItemSlot");
            if (trItemSlot == null)
            {
                GameObject goItemSlot = Instantiate(m_goItemSlot, trEquipSlot);
                goItemSlot.name = "ItemSlot";
                trItemSlot = goItemSlot.transform;

                Button buttonItemslot = trItemSlot.GetComponent<Button>();
                buttonItemslot.onClick.RemoveAllListeners();
                buttonItemslot.onClick.AddListener(() => OnClickGearEquipped(nSlotIndex));
                buttonItemslot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            CsUIData.Instance.DisplayItemSlot(trItemSlot.transform, csHeroMainGear.MainGear, csHeroMainGear.EnchantLevel, csHeroMainGear.BattlePower, csHeroMainGear.Owned);
        }

        csHeroMainGear = m_csHeroInfo.HeroMainGearEquippedArmor;

        if (csHeroMainGear != null)
        {
            int nSlotIndex = csHeroMainGear.MainGear.MainGearType.SlotIndex;
            trEquipSlot = trItemList.Find("Equip" + nSlotIndex);
            trItemSlot = trEquipSlot.Find("ItemSlot");
            if (trItemSlot == null)
            {
                GameObject goItemSlot = Instantiate(m_goItemSlot, trEquipSlot);
                goItemSlot.name = "ItemSlot";
                trItemSlot = goItemSlot.transform;

                Button buttonItemslot = trItemSlot.GetComponent<Button>();
                buttonItemslot.onClick.RemoveAllListeners();
                buttonItemslot.onClick.AddListener(() => OnClickGearEquipped(nSlotIndex));
                buttonItemslot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            CsUIData.Instance.DisplayItemSlot(trItemSlot.transform, csHeroMainGear.MainGear, csHeroMainGear.EnchantLevel, csHeroMainGear.BattlePower, csHeroMainGear.Owned);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearEquipped()
    {
        //Transform trItemList = m_trEquipment.Find("ItemList");

        for (int i = 0; i < m_csHeroInfo.EquippedHeroSubGearsList.Count; i++)
        {
            CsHeroSubGear csHeroSubGear = m_csHeroInfo.EquippedHeroSubGearsList[i];

            UpdateSubGearSlot(csHeroSubGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearSlot(CsHeroSubGear csHeroSubGear)
    {
        Transform trItemList = m_trEquipment.Find("ItemList");
        Transform trEquipSlot = trItemList.Find("Equip" + csHeroSubGear.SubGear.SlotIndex);
        Transform trItemSlot = trEquipSlot.Find("ItemSlot");

        if (csHeroSubGear.Equipped)
        {
            //장착

            if (trItemSlot == null)
            {
                GameObject goItemSlot = Instantiate(m_goItemSlot, trEquipSlot);
                goItemSlot.name = "ItemSlot";
                trItemSlot = goItemSlot.transform;

                Button buttonItemslot = trItemSlot.GetComponent<Button>();
                buttonItemslot.onClick.RemoveAllListeners();
                buttonItemslot.onClick.AddListener(() => OnClickGearEquipped(csHeroSubGear.SubGear.SlotIndex));
                buttonItemslot.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            }
            else
            {
                trItemSlot.gameObject.SetActive(true);
            }

            CsUIData.Instance.DisplayItemSlot(trItemSlot.transform, csHeroSubGear);
        }
        else
        {
            //해제

            if (trItemSlot != null)
            {
                trItemSlot.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(UnityAction unityAction)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        unityAction();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo()
    {
        m_trItemInfoLeft = m_trPopupList.Find("PopupItemInfoLeft");

        if (m_trItemInfoLeft == null)
        {
            GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
            m_trItemInfoLeft = goPopupItemInfo.transform;
        }
        else
        {
            m_trItemInfoLeft.gameObject.SetActive(true);
        }

        m_trEquipment.gameObject.SetActive(false);
    }


    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoMainGear()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, m_csHeroMainGearSelect, false, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoSubGear()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, m_csHeroSubGearSelect, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoWing()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        List<CsWing> listHeroInfoWing = new List<CsWing>();

        for (int i = 0; i < m_csHeroInfo.WingList.Count; i++)
        {
            if (CsGameData.Instance.GetWing(m_csHeroInfo.WingList[i]) == null)
            {
                continue;
            }
            else
            {
                listHeroInfoWing.Add(CsGameData.Instance.GetWing(m_csHeroInfo.WingList[i]));
            }
        }

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, m_csHeroInfo.EquippedWingId, m_csHeroInfo.WingLevel, m_csHeroInfo.WingStep, listHeroInfoWing, m_csHeroInfo.HeroWingPartsList, false);

        listHeroInfoWing.Clear();
        listHeroInfoWing = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        if (m_trItemInfoLeft != null)
        {
            if (enPopupItemInfoPositionType == EnPopupItemInfoPositionType.Left)
            {
                m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
                Destroy(m_trItemInfoLeft.gameObject);
                m_trEquipment.gameObject.SetActive(true);
                m_csPopupItemInfo = null;
                m_trItemInfoLeft = null;
            }
        }
    }

    #region PopupSet

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSetInfo(EnPopupSetInfoType enPoupSetInfoType)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSetInfo/PopupSetInfo");
        yield return resourceRequest;
        m_goPopupSetInfo = (GameObject)resourceRequest.asset;

        OpenPopupSetInfo(enPoupSetInfoType);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupSetInfo(EnPopupSetInfoType enPoupSetInfoType)
    {
        GameObject goPopupSetInfo = Instantiate(m_goPopupSetInfo, m_trPopupList);
        CsPopupSetInfo csPopupSetInfo = goPopupSetInfo.GetComponent<CsPopupSetInfo>();
        csPopupSetInfo.DisplayType(enPoupSetInfoType, m_csHeroInfo);

        if (enPoupSetInfoType == EnPopupSetInfoType.MainGear)
        {
            csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.InvenMainGear);
        }
        else
        {
            csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.InvenSubGear);
        }
    }

    #endregion

    #region Model

    //---------------------------------------------------------------------------------------------------
    void LoadCharacterModel()		//캐릭터모델 동적로드함수
    {
		int nJobId = m_csHeroInfo.Job.ParentJobId == 0 ? m_csHeroInfo.Job.JobId : m_csHeroInfo.Job.ParentJobId;

		Transform trCharacterModel = m_trEquipment.Find("3DCharacter/Character" + nJobId);

        if (trCharacterModel == null)
        {
            StartCoroutine(LoadCharacterModelCoroutine());
        }
        else
        {
            trCharacterModel.gameObject.SetActive(true);
            UpdateCharacterModel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCharacterModelCoroutine()
    {
        int nJobId = m_csHeroInfo.Job.ParentJobId == 0 ? m_csHeroInfo.Job.JobId : m_csHeroInfo.Job.ParentJobId;

        Debug.Log("#@#@ LoadCharacterModelCoroutine #@#@ : " + nJobId);
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
        yield return resourceRequest;
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_trEquipment.Find("3DCharacter"));

        switch (nJobId)
        {
            case (int)EnJob.Gaia:
                goCharacter.transform.localPosition = new Vector3(-15, -218, 500);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(210, 210, 210);
                break;
            case (int)EnJob.Asura:
                goCharacter.transform.localPosition = new Vector3(0, -175, 400);
                goCharacter.transform.eulerAngles = new Vector3(0, 185, 0);
                goCharacter.transform.localScale = new Vector3(210, 210, 210);
                break;
            case (int)EnJob.Deva:
                goCharacter.transform.localPosition = new Vector3(0, -210, 514);
                goCharacter.transform.eulerAngles = new Vector3(0, 175, 0);
                goCharacter.transform.localScale = new Vector3(220, 220, 220);
                break;
            case (int)EnJob.Witch:
                goCharacter.transform.localPosition = new Vector3(0, -170, 400);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(190, 190, 190);
                break;
        }

        goCharacter.GetComponent<CsUICharcterRotate>().UICamera = m_uiCamera;

		goCharacter.name = "Character" + nJobId;
        goCharacter.gameObject.SetActive(true);

        UpdateCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel()
    {
        int nJobId = m_csHeroInfo.Job.ParentJobId == 0 ? m_csHeroInfo.Job.JobId : m_csHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = m_trEquipment.Find("3DCharacter/Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
			CsHeroCustomData csHeroCustomData = new CsHeroCustomData(m_csHeroInfo);

			csEquipment.LowChangEquipments(csHeroCustomData);
            csEquipment.CreateWing(csHeroCustomData, null, true);
        }
    }

    #endregion Model
}
