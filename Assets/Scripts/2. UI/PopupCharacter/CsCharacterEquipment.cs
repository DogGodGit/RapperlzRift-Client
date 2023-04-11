using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-11-24)
//---------------------------------------------------------------------------------------------------

public class CsCharacterEquipment : CsPopupSub
{
    [SerializeField] GameObject m_goItemSlot;

    GameObject m_goPopupItemInfo;
	GameObject m_goPopupImprovement;

    Transform m_trCanvas2;
    Transform m_trEquipment;
    Transform m_trPopupList;
    Transform m_trItemInfoLeft;
    Transform m_trPopupSetInfo;
    Transform m_trPopupHeroAttrPotion;
	
    Text m_textLevelName;
    Text m_textBattlePower;

    CsHeroMainGear m_csHeroMainGearSelect;
    CsHeroSubGear m_csHeroSubGearSelect;

    GameObject m_goPopupSetInfo;
    GameObject m_goPopupHeroAttrPotion;
	
    CsPopupItemInfo m_csPopupItemInfo;

    Camera m_uiCamera;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trEquipment = transform.Find("Equipment");
        m_uiCamera = m_trEquipment.Find("3DCharacter/UIChar_Camera").GetComponent<Camera>();
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");

        InitializeUI();

        CsGameEventUIToUI.Instance.EventMainGearEquip += OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip += OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip += OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip += OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventVisibleSection += OnEventVisibleSection;

        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate += EventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate += EventSubGearSoulstoneLevelSetActivate;

        CsGameEventUIToUI.Instance.EventWingEquip += OnEventWingEquip;
        CsGameEventUIToUI.Instance.EventMountGearEquip += OnEventMountGearEquip;

        CsCostumeManager.Instance.EventCostumeEquip += OnEventCostumeEquip;
        CsCostumeManager.Instance.EventCostumeUnequip += OnEventCostumeUnequip;
        CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeEffectApply += OnEventCostumeEffectApply;

    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        m_trEquipment.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trItemInfoLeft != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Left);
        }

        if (m_trPopupSetInfo != null)
        {
            OnEventClosePopupSetInfo();
        }
    }

	//---------------------------------------------------------------------------------------------------
	public override void OnUpdate(float flTime)
	{
		UpdateButtonOrdealQuest();
        UpdateButtonPotionAttr();
	}

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        //if (m_trItemInfoLeft != null)
        //{
        //    m_trItemInfoLeft.gameObject.SetActive(false);
        //}

        CsGameEventUIToUI.Instance.EventMainGearEquip -= OnEventMainGearEquip;
        CsGameEventUIToUI.Instance.EventMainGearUnequip -= OnEventMainGearUnequip;

        CsGameEventUIToUI.Instance.EventSubGearEquip -= OnEventSubGearEquip;
        CsGameEventUIToUI.Instance.EventSubGearUnequip -= OnEventSubGearUnequip;

        CsGameEventUIToUI.Instance.EventVisibleSection -= OnEventVisibleSection;

        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate -= EventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate -= EventSubGearSoulstoneLevelSetActivate;

        CsGameEventUIToUI.Instance.EventWingEquip -= OnEventWingEquip;
        CsGameEventUIToUI.Instance.EventMountGearEquip -= OnEventMountGearEquip;

        CsCostumeManager.Instance.EventCostumeEquip -= OnEventCostumeEquip;
        CsCostumeManager.Instance.EventCostumeUnequip -= OnEventCostumeUnequip;
        CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeEffectApply -= OnEventCostumeEffectApply;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEquip(int nCostumeId, int nCostumeEffectId)
    {
        UpdateCharacterModel();
        UpdateCostume();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeUnequip()
    {
        UpdateCharacterModel();
        UpdateCostume();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumePeriodExpired()
    {
        UpdateCharacterModel();
        UpdateCostume();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEffectApply(int nCostumeEffectId)
    {
        UpdateCharacterModel();
        UpdateCostume();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEquip()
    {
        UpdateWingChanged();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearEquip(Guid guid)
    {
        UpdateCharacterBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    void EventMainGearEnchantLevelSetActivate()
    {
        UpdateNoticeSetEquipment(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void EventSubGearSoulstoneLevelSetActivate()
    {
        UpdateNoticeSetSoulStone(CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet());
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

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEquip(Guid guidHeroGearId)
    {
        CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroGearId);
        UpdateMainGearChanged(csHeroMainGear);
        UpdateNoticeSetEquipment(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearUnequip(Guid guidHeroGearId)
    {
        CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidHeroGearId);
        UpdateMainGearChanged(csHeroMainGear);
        UpdateNoticeSetEquipment(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearEquip(int nSubGearId)
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
        UpdateSubGearChanged(csHeroSubGear);
        UpdateNoticeSetSoulStone(CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearUnequip(int nSubGearId)
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
        UpdateSubGearChanged(csHeroSubGear);
        UpdateNoticeSetSoulStone(CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet());
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSoultoneSet()
    {
        Debug.Log("소울스톤 세트 팝업창 오픈");

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
        Debug.Log("장비 세트 팝업창 오픈");

        if (m_goPopupSetInfo == null)
        {
            StartCoroutine(LoadPopupSetInfo(EnPopupSetInfoType.MainGear));
        }
        else
        {
            OpenPopupSetInfo(EnPopupSetInfoType.MainGear);
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickButtonOrdeal()
	{
		CsUIData.Instance.PlayUISound(EnUISoundType.Button);

		CsGameEventUIToUI.Instance.OnEventOpenPopupOrdealQuest();
	}

    //---------------------------------------------------------------------------------------------------
    void OnClickHeroAttrPotion()
    {
        CsUIData.Instance.PlayUISound(EnUISoundType.Button);

        LoadPopupHeroAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGearEquipped(int nSlotIndex)
    {
        if (nSlotIndex == (int)EnMainGearSlotIndex.Weapon || nSlotIndex == (int)EnMainGearSlotIndex.Armor)
        {
            m_csHeroMainGearSelect = CsGameData.Instance.MyHeroInfo.GetHeroMainGearBySlotIndex(nSlotIndex);

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
            m_csHeroSubGearSelect = CsGameData.Instance.MyHeroInfo.GetHeroSubGearBySlotIndex(nSlotIndex);

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
    void OnEventVisibleSection(bool bVisible)
    {
        if (!bVisible)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Left);
        }

        m_trEquipment.gameObject.SetActive(bVisible);
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
    void OnClickCostumeInfo()
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(OpenPopupItemInfoCostume));
        }
        else
        {
            OpenPopupItemInfoCostume();
        }
    }

	//---------------------------------------------------------------------------------------------------
	void OnClickImprovment()
	{
		if (m_goPopupImprovement == null)
		{
			StartCoroutine(LoadPopupImprovement(OpenPopupImprovement));
		}
		else
		{
			OpenPopupImprovement();
		}
	}

    #endregion EventHandler

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

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, m_csHeroMainGearSelect, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoSubGear()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, m_csHeroSubGearSelect);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoWing()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        List<CsWing> listMyHeroWing = new List<CsWing>();

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWingList[i].Wing == null)
            {
                continue;
            }
            else
            {
                listMyHeroWing.Add(CsGameData.Instance.MyHeroInfo.HeroWingList[i].Wing);
            }
        }

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, CsGameData.Instance.MyHeroInfo.EquippedWingId, CsGameData.Instance.MyHeroInfo.WingLevel,
            CsGameData.Instance.MyHeroInfo.WingStep, listMyHeroWing, CsGameData.Instance.MyHeroInfo.HeroWingPartList, true);

        listMyHeroWing.Clear();
        listMyHeroWing = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfoCostume()
    {
        OpenPopupItemInfo();

        m_csPopupItemInfo = m_trItemInfoLeft.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);

        if (csHeroCostume == null)
        {
            return;
        }
        else
        {
            m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Left, csHeroCostume);
        }

        csHeroCostume = null;
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateButtonOrdealQuest()
	{
		int nMinRequiredLevel = CsGameData.Instance.OrdealQuestList.Min(ordealQeust => ordealQeust.RequiredHeroLevel);

		Transform trButtonOrdealQuest = m_trEquipment.Find("ButtonOrdealQuest");
		trButtonOrdealQuest.gameObject.SetActive(CsGameData.Instance.MyHeroInfo.Level >= nMinRequiredLevel);

		if (CsGameData.Instance.MyHeroInfo.Level >= nMinRequiredLevel)
		{
			Button buttonOrdealQuest = trButtonOrdealQuest.GetComponent<Button>();

			if (CsOrdealQuestManager.Instance.CsHeroOrdealQuest == null ||
				CsOrdealQuestManager.Instance.CsHeroOrdealQuest.Completed)
			{
				buttonOrdealQuest.interactable = false;
				trButtonOrdealQuest.Find("Notice").gameObject.SetActive(false);
			}
			else
			{
				buttonOrdealQuest.interactable = true;
				trButtonOrdealQuest.Find("Notice").gameObject.SetActive(CsGameData.Instance.MyHeroInfo.CheckOrdealQuest());
			}
		}
	}

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPotionAttr()
    {
        Transform trPotionAttrNotice = m_trEquipment.Find("ButtonHeroAttrPotion/ImageNotice");
        trPotionAttrNotice.gameObject.SetActive(CsGameData.Instance.MyHeroInfo.CheckPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearEuquipped()
    {
        Transform trItemList = m_trEquipment.Find("ItemList");

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i] != null)
            {
                CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i];

                int nSlotIndex = csHeroMainGear.MainGear.MainGearType.SlotIndex;

                Transform trEquipSlot = trItemList.Find("Equip" + nSlotIndex);

                Transform trItemSlot = trEquipSlot.Find("ItemSlot");

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

                CsUIData.Instance.DisplayItemSlot(trItemSlot.transform, csHeroMainGear);
            }
            else
            {
                if (i == (int)EnMainGearIndex.Weapon)
                {
                    Transform trItemSlotWeapown = trItemList.Find("Equip" + (int)EnMainGearSlotIndex.Weapon + "/ItemSlot");

                    if (trItemSlotWeapown != null)
                    {
                        trItemSlotWeapown.gameObject.SetActive(false);
                    }
                }
                else if (i == (int)EnMainGearIndex.Armor)
                {
                    Transform trItemSlotArmour = trItemList.Find("Equip" + (int)EnMainGearSlotIndex.Armor + "/ItemSlot");
                    if (trItemSlotArmour != null)
                    {
                        trItemSlotArmour.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearEquipped()
    {
        //Transform trItemList = m_trEquipment.Find("ItemList");

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSubGearList.Count; i++)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList[i];

            UpdateSubGearSlot(csHeroSubGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;

        Button buttonSetSoulStone = m_trEquipment.Find("ButtonSoulStone").GetComponent<Button>();
        buttonSetSoulStone.onClick.RemoveAllListeners();
        buttonSetSoulStone.onClick.AddListener(OnClickSoultoneSet);
        buttonSetSoulStone.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Button buttonSetEquipment = m_trEquipment.Find("ButtonSetEquipment").GetComponent<Button>();
        buttonSetEquipment.onClick.RemoveAllListeners();
        buttonSetEquipment.onClick.AddListener(OnClickEnchantSet);
        buttonSetEquipment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		Button buttonOrdeal = m_trEquipment.Find("ButtonOrdealQuest").GetComponent<Button>();
		buttonOrdeal.onClick.RemoveAllListeners();
		buttonOrdeal.onClick.AddListener(OnClickButtonOrdeal);
		
		Text textButtonOrdeal = m_trEquipment.Find("ButtonOrdealQuest/Text").GetComponent<Text>();
		CsUIData.Instance.SetFont(textButtonOrdeal);
		textButtonOrdeal.text = CsConfiguration.Instance.GetString("A120_BTN_00001");

        Button buttonHeroAttrPotion = m_trEquipment.Find("ButtonHeroAttrPotion").GetComponent<Button>();
        buttonHeroAttrPotion.onClick.RemoveAllListeners();
        buttonHeroAttrPotion.onClick.AddListener(OnClickHeroAttrPotion);

		UpdateButtonOrdealQuest();
        UpdateButtonPotionAttr();

        UpdateMainGearEuquipped();
        UpdateSubGearEquipped();
        UpdateWing();
        UpdateCostume();

        Transform trCharacterInfo = m_trEquipment.Find("CharacterInfo");

        m_textLevelName = trCharacterInfo.Find("TextLevelName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevelName);
        UpdateCharacterLevel();

        m_textBattlePower = trCharacterInfo.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBattlePower);
        UpdateCharacterBattlePower();

        Button buttonWing = transform.Find("Equipment/ItemList/Equip0/ButtonWing").GetComponent<Button>();
        buttonWing.onClick.RemoveAllListeners();
        buttonWing.onClick.AddListener(OnClickWingInfo);
        buttonWing.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

		CsUIData.Instance.SetButton(transform.Find("Equipment/ButtonImprovment"), OnClickImprovment);

        UpdateNoticeSetSoulStone(CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet());
        UpdateNoticeSetEquipment(CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet());

        LoadCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void LoadCharacterModel()		//캐릭터모델 동적로드함수
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = m_trEquipment.Find("3DCharacter/Character" + nJobId);

        if (trCharacterModel == null)
        {
            StartCoroutine(LoadCharacterModelCoroutine(nJobId));
        }
        else
        {
            trCharacterModel.gameObject.SetActive(true);
            UpdateCharacterModel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadCharacterModelCoroutine(int nJobId)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
        yield return resourceRequest;
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, m_trEquipment.Find("3DCharacter"));

        float flScale = 1 / m_trCanvas2.GetComponent<RectTransform>().localScale.x;

        switch (nJobId)
        {
            case (int)EnJob.Gaia:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Asura:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 185, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Deva:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 175, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Witch:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 180, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
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
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = m_trEquipment.Find("3DCharacter/Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
			CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

			csEquipment.MidChangeEquipments(csHeroCustomData, false);            
            csEquipment.CreateWing(csHeroCustomData, null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterLevel()
    {
        m_textLevelName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), CsGameData.Instance.MyHeroInfo.Level, CsGameData.Instance.MyHeroInfo.Name);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterBattlePower()
    {
        m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), CsGameData.Instance.MyHeroInfo.BattlePower.ToString("#,##0"));
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNoticeSetSoulStone(bool bIsOn)
    {
        Transform trSetSoulStoneNotice = m_trEquipment.Find("ButtonSoulStone/ImageNotice");
        trSetSoulStoneNotice.gameObject.SetActive(bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNoticeSetEquipment(bool bIsOn)
    {
        Transform trSetEquipmentNotice = m_trEquipment.Find("ButtonSetEquipment/ImageNotice");
        trSetEquipmentNotice.gameObject.SetActive(bIsOn);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearChanged(CsHeroMainGear csHeroMainGear)
    {
        // 장착슬롯 갱신
        UpdateMainGearSlot(csHeroMainGear.MainGear.MainGearType.SlotIndex, csHeroMainGear);

        // 모델장비 갱신
        UpdateCharacterModel();

        // 전투력 갱신
        UpdateCharacterBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateSubGearChanged(CsHeroSubGear csHeroSubGear)
    {
        // 장착슬롯 갱신
        UpdateSubGearSlot(csHeroSubGear);

        // 전투력 갱신
        UpdateCharacterBattlePower();
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateWingChanged()
    {
        UpdateWing();

        // 모델장비 갱신
        UpdateCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearSlot(int nSlotIndex, CsHeroMainGear csHeroMainGear)
    {
        Transform trItemList = m_trEquipment.Find("ItemList");
        Transform trEquipSlot = trItemList.Find("Equip" + nSlotIndex);
        Transform trItemSlot = trEquipSlot.Find("ItemSlot");

        CsHeroMainGear csHeroMainGearChanged = CsGameData.Instance.MyHeroInfo.GetHeroMainGearEquipped(csHeroMainGear.Id);

        if (csHeroMainGearChanged != null)
        {
            //메인기어 장착

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

            CsUIData.Instance.DisplayItemSlot(trItemSlot.transform, csHeroMainGear);
        }
        else
        {
            //메인기어 해제

            if (trItemSlot != null)
            {
                trItemSlot.gameObject.SetActive(false);
            }
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
    void UpdateWing()
    {
        Transform trItemList = m_trEquipment.Find("ItemList");
        Transform trEquipSlot = trItemList.Find("Equip0");
        Button buttonWing = trEquipSlot.Find("ButtonWing").GetComponent<Button>();
        Image imageButtonWing = buttonWing.transform.Find("ImageIcon").GetComponent<Image>();

        if (CsGameData.Instance.MyHeroInfo.EquippedWingId == 0)
        {
            buttonWing.gameObject.SetActive(false);
        }
        else
        {
            buttonWing.gameObject.SetActive(true);
            imageButtonWing.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/Wing_" + CsGameData.Instance.MyHeroInfo.EquippedWingId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCostume()
    {
        Transform trItemList = m_trEquipment.Find("ItemList");
        Transform trEquipSlot = trItemList.Find("Equip5");

        Transform trItemSlot = trEquipSlot.Find("ItemSlot");

        Button buttonCostume = trItemSlot.GetComponent<Button>();
        buttonCostume.onClick.RemoveAllListeners();
        buttonCostume.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonCostume.onClick.AddListener(OnClickCostumeInfo);

        CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);

        if (csHeroCostume == null)
        {
            trItemSlot.gameObject.SetActive(false);
        }
        else
        {
            CsItem csItem = CsGameData.Instance.ItemList.Find(a => a.ItemType.EnItemType == EnItemType.Costume && a.Value1 == csHeroCostume.Costume.CostumeId);

            if (csItem == null)
            {
                trItemSlot.gameObject.SetActive(false);
            }
            else
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, true, 0, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
            }

            trItemSlot.gameObject.SetActive(true);
        }
    }

	//---------------------------------------------------------------------------------------------------
	IEnumerator LoadPopupImprovement(UnityAction unityAction)
	{
		ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupImprovement/PopupImprovement");
		yield return resourceRequest;
		m_goPopupImprovement = (GameObject)resourceRequest.asset;

		unityAction();
	}

	//---------------------------------------------------------------------------------------------------
	void OpenPopupImprovement()
	{
		Transform trPopupImprovment = m_trPopupList.Find("PopupImprovement");

		if (trPopupImprovment == null)
		{
			GameObject goPopupImprovement = Instantiate(m_goPopupImprovement, m_trPopupList);
			goPopupImprovement.name = "PopupImprovement";
		}
		else
		{
			trPopupImprovment.gameObject.SetActive(true);
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
        m_trPopupSetInfo = goPopupSetInfo.transform;
        CsPopupSetInfo csPopupSetInfo = m_trPopupSetInfo.GetComponent<CsPopupSetInfo>();
        csPopupSetInfo.EventClosePopupSetInfo += OnEventClosePopupSetInfo;
        csPopupSetInfo.DisplayType(enPoupSetInfoType);

        if (enPoupSetInfoType == EnPopupSetInfoType.MainGear)
        {
            csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.InvenMainGear);
        }
        else
        {
            csPopupSetInfo.SetPosition(EnPopupSetInfoPosition.InvenSubGear);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupSetInfo()
    {
        if (m_trPopupSetInfo == null)
        {
            return;
        }
        else
        {
            CsPopupSetInfo csPopupSetInfo = m_trPopupSetInfo.GetComponent<CsPopupSetInfo>();
            csPopupSetInfo.EventClosePopupSetInfo -= OnEventClosePopupSetInfo;

            Destroy(m_trPopupSetInfo.gameObject);
            m_trPopupSetInfo = null;
        }
    }

    #endregion

    #region HeroAttrPotion

    //---------------------------------------------------------------------------------------------------
    public void LoadPopupHeroAttrPotion()
    {
        if (m_goPopupHeroAttrPotion == null)
        {
            StartCoroutine(LoadPopupHeroAttrPotionCoroutine());
        }
        else
        {
            OpenPopupHeroAttrPotion();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupHeroAttrPotionCoroutine()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupHeroAttrPotion");
        yield return resourceRequest;
        m_goPopupHeroAttrPotion = (GameObject)resourceRequest.asset;

        OpenPopupHeroAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupHeroAttrPotion()
    {
        if (m_trPopupHeroAttrPotion != null)
        {
            Destroy(m_trPopupHeroAttrPotion.gameObject);
            m_trPopupHeroAttrPotion = null;
        }

        m_trPopupHeroAttrPotion = Instantiate(m_goPopupHeroAttrPotion, m_trPopupList).transform;
        m_trPopupHeroAttrPotion.name = "PopupHeroAttrPotion";

        CsPopupHeroAttrPotion csPopupHeroAttrPotion = m_trPopupHeroAttrPotion.GetComponent<CsPopupHeroAttrPotion>();
        csPopupHeroAttrPotion.EventClosePopupHeroAttrPotion += OnEventClosePopupHeroAttrPotion;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupHeroAttrPotion()
    {
        if (m_trPopupHeroAttrPotion == null)
        {
            return;
        }
        else
        {
            CsPopupHeroAttrPotion csPopupHeroAttrPotion = m_trPopupHeroAttrPotion.GetComponent<CsPopupHeroAttrPotion>();
            csPopupHeroAttrPotion.EventClosePopupHeroAttrPotion -= OnEventClosePopupHeroAttrPotion;

            Destroy(m_trPopupHeroAttrPotion.gameObject);
            m_trPopupHeroAttrPotion = null;
        }
    }

    #endregion HeroAttrPotion
}