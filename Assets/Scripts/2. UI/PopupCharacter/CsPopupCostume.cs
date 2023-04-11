using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupCostume : CsPopupSub
{
    GameObject m_goCostumeItem;

    Transform m_trCanvas2;
    Transform m_trCostume;
    Transform m_trContent;
    Transform m_trPopupList;
    Transform m_trPopupCostumeEffect;
    Transform m_trPopupCostumeEnchant;
    Transform m_trPopupCostumeCollection;
    Transform m_trImageNotice;

    Button m_buttonEffect;
    Button m_buttonEnchant;
    Button m_buttonDyeing;

    Text m_textNoCostume;

    int m_nPrevEquippedHeroCostumeId = -1;

    bool m_bLoadPopupCostumeEffect = false;
    bool m_bLoadPopupCostumeEnchant = false;
    bool m_bLoadPopupCostumeCollection = false;

    float m_flTime = 0f;

    Camera m_uiCamera;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsCostumeManager.Instance.EventCostumeEquip += OnEventCostumeEquip;
        CsCostumeManager.Instance.EventCostumeUnequip += OnEventCostumeUnequip;
        CsCostumeManager.Instance.EventCostumeEffectApply += OnEventCostumeEffectApply;
        CsCostumeManager.Instance.EventCostumePeriodExpired += OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeItemUse += OnEventCostumeItemUse;
        CsCostumeManager.Instance.EventCostumeEnchant += OnEventCostumeEnchant;
        CsGameEventUIToUI.Instance.EventOpenPopupCostumeEnchant += OnEventOpenPopupCostumeEnchant;
        
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsCostumeManager.Instance.EventCostumeEquip -= OnEventCostumeEquip;
        CsCostumeManager.Instance.EventCostumeUnequip -= OnEventCostumeUnequip;
        CsCostumeManager.Instance.EventCostumeEffectApply -= OnEventCostumeEffectApply;
        CsCostumeManager.Instance.EventCostumePeriodExpired -= OnEventCostumePeriodExpired;
        CsCostumeManager.Instance.EventCostumeItemUse -= OnEventCostumeItemUse;
        CsCostumeManager.Instance.EventCostumeEnchant -= OnEventCostumeEnchant;
        CsGameEventUIToUI.Instance.EventOpenPopupCostumeEnchant -= OnEventOpenPopupCostumeEnchant;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1f <= Time.time)
        {
            UpdateHeroCostumeRemainingTime();
            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        OnEventClosePopupCostumeEffect();
        OnEventClosePopupCostumeEnchant();
        OnEventClosePopupCostumeCollection();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEquip(int nCostumeId, int nCostumeEffectId)
    {
		CsHeroCostume csHeroCostumeEquipped = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId);
        CsHeroCostume csHeroCostumePrevEquipped = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == m_nPrevEquippedHeroCostumeId);

        if (csHeroCostumeEquipped == null)
        {

        }
        else
        {
            UpdateHeroCostumeItem(csHeroCostumeEquipped);
            UpdateCharacterModel();
        }

        if (csHeroCostumePrevEquipped == null)
        {
            
        }
        else
        {
            UpdateHeroCostumeItem(csHeroCostumePrevEquipped);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeUnequip()
    {
        CsHeroCostume csHeroCostumePrevEquipped = CsCostumeManager.Instance.HeroCostumeList.Find(a => a.HeroCostumeId == m_nPrevEquippedHeroCostumeId);

        if (csHeroCostumePrevEquipped == null)
        {

        }
        else
        {
            UpdateHeroCostumeItem(csHeroCostumePrevEquipped);
            UpdateCharacterModel();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEffectApply(int nCostumeEffectId)
    {
        UpdateCharacterModel();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumePeriodExpired()
    {
        DisplayHeroCostume();
        DisplayButtonCostumeContent();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeItemUse()
    {
        DisplayButtonCostumeContent();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCostumeEnchant(bool bEnchant)
    {
        m_trImageNotice.gameObject.SetActive(CsCostumeManager.Instance.CheckCostumeEnchant());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventOpenPopupCostumeEnchant()
    {
        OpenPopupCostumeEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHeroCostumeEquip(CsHeroCostume csHeroCostume)
    {
        m_nPrevEquippedHeroCostumeId = CsCostumeManager.Instance.EquippedHeroCostumeId;

        if (csHeroCostume.HeroCostumeId == CsCostumeManager.Instance.EquippedHeroCostumeId)
        {
            CsCostumeManager.Instance.SendCostumeUnequip();
        }
        else
        {
            CsCostumeManager.Instance.SendCostumeEquip(csHeroCostume.HeroCostumeId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDiaShop()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.DiaShop, EnSubMenu.DiaShop);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeEffect()
    {
        OpenPopupCostumeEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeEnchant()
    {
        OpenPopupCostumeEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCostumeDyeing()
    {
        if (!m_bLoadPopupCostumeEffect || !m_bLoadPopupCostumeEnchant || !m_bLoadPopupCostumeCollection)
        {
            m_bLoadPopupCostumeCollection = true;
            m_trCostume.gameObject.SetActive(false);
            StartCoroutine(LoadPopupCostumeCollection());
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_uiCamera = transform.Find("3DCharacter/UIChar_Camera").GetComponent<Camera>();
        
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = m_trCanvas2.Find("PopupList");

        m_trCostume = transform.Find("Costume");

        m_trContent = m_trCostume.Find("Scroll View/Viewport/Content");

        m_textNoCostume = m_trCostume.Find("TextNoCostume").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoCostume);
        m_textNoCostume.text = CsConfiguration.Instance.GetString("A151_TXT_00013");

        Button buttonShop = m_textNoCostume.transform.Find("ButtonShop").GetComponent<Button>();
        buttonShop.onClick.RemoveAllListeners();
        buttonShop.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonShop.onClick.AddListener(OnClickDiaShop);

        Text textButtonShop = buttonShop.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonShop);
        textButtonShop.text = CsConfiguration.Instance.GetString("A151_TXT_00014");

        Text textSetAttr = m_trCostume.Find("TextSetAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSetAttr);
        textSetAttr.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00001"), 000, 000);

        m_buttonEffect = m_trCostume.Find("ButtonEffect").GetComponent<Button>();
        m_buttonEffect.onClick.RemoveAllListeners();
        m_buttonEffect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonEffect.onClick.AddListener(OnClickCostumeEffect);

        Text textButtonEffect = m_buttonEffect.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEffect);
        textButtonEffect.text = CsConfiguration.Instance.GetString("A151_TXT_00010");

        m_buttonEnchant = m_trCostume.Find("ButtonEnchant").GetComponent<Button>();
        m_buttonEnchant.onClick.RemoveAllListeners();
        m_buttonEnchant.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonEnchant.onClick.AddListener(OnClickCostumeEnchant);

        Text textButtonEnchant = m_buttonEnchant.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEnchant);
        textButtonEnchant.text = CsConfiguration.Instance.GetString("A151_TXT_00011");

        m_buttonDyeing = m_trCostume.Find("ButtonDyeing").GetComponent<Button>();
        m_buttonDyeing.onClick.RemoveAllListeners();
        m_buttonDyeing.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonDyeing.onClick.AddListener(OnClickCostumeDyeing);

        Text textButtonDyeing = m_buttonDyeing.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDyeing);
        textButtonDyeing.text = CsConfiguration.Instance.GetString("A151_TXT_00012");

        m_trImageNotice = m_buttonEnchant.transform.Find("ImageNotice");
        m_trImageNotice.gameObject.SetActive(CsCostumeManager.Instance.CheckCostumeEnchant());

        LoadCharacterModel();
        DisplayHeroCostume();
        DisplayButtonCostumeContent();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayButtonCostumeContent()
    {
        bool bInteractable = CsCostumeManager.Instance.HeroCostumeList.Count != 0;

        CsUIData.Instance.DisplayButtonInteractable(m_buttonEnchant, bInteractable);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayHeroCostume()
    {
        if (m_goCostumeItem == null)
        {
            StartCoroutine(LoadHeroCostumeItem());
        }
        else
        {
            UpdateHeroCostumeItems();
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadHeroCostumeItem()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/CostumeItem");
        yield return resourceRequest;

        m_goCostumeItem = (GameObject)resourceRequest.asset;
        UpdateHeroCostumeItems();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroCostumeItems()
    {
        for (int i = 0; i < m_trContent.childCount; i++)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }

        if (CsCostumeManager.Instance.HeroCostumeList.Count == 0)
        {
            m_textNoCostume.gameObject.SetActive(true);
        }
        else
        {
            m_textNoCostume.gameObject.SetActive(false);

            Transform trCostumeItem = null;

            for (int i = 0; i < CsCostumeManager.Instance.HeroCostumeList.Count; i++)
            {
                CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList[i];

                trCostumeItem = m_trContent.Find("CostumeItem" + csHeroCostume.HeroCostumeId);

                if (trCostumeItem == null)
                {
                    trCostumeItem = Instantiate(m_goCostumeItem, m_trContent).transform;
                    trCostumeItem.name = "CostumeItem" + csHeroCostume.HeroCostumeId;
                }
                else
                {
                    trCostumeItem.gameObject.SetActive(true);
                }

                UpdateHeroCostumeItem(csHeroCostume);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroCostumeItem(CsHeroCostume csHeroCostume)
    {
        Transform trCostumeItem = m_trContent.Find("CostumeItem" + csHeroCostume.HeroCostumeId);

        if (trCostumeItem == null)
        {
            return;
        }
        else
        {
            int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

            Image imageCostume = trCostumeItem.Find("ImageCostume").GetComponent<Image>();
            imageCostume.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCharacter/frm_costume_" + csHeroCostume.HeroCostumeId + "_" + nJobId);

            Text textName = trCostumeItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csHeroCostume.Costume.Name;

            Text textAttr = trCostumeItem.Find("TextAttr").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttr);
            textAttr.text = CsConfiguration.Instance.GetString("A151_TXT_00003");

            if (csHeroCostume.Costume.CostumeAttrList != null && 0 < csHeroCostume.Costume.CostumeAttrList.Count)
            {
                textAttr.gameObject.SetActive(true);
            }
            else
            {
                textAttr.gameObject.SetActive(false);
            }

            Text textCostumeTimer = trCostumeItem.Find("CostumeTime/TextTimer").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCostumeTimer);

            if (csHeroCostume.Costume.PeriodLimitDay == 0)
            {
                // 영구
                textCostumeTimer.text = CsConfiguration.Instance.GetString("A151_TXT_00006");
            }
            else
            {
                // 남은 시간
                System.TimeSpan tsRemainingCostumeTime = System.TimeSpan.FromSeconds(csHeroCostume.RemainingTime - Time.realtimeSinceStartup);

                if (86400 <= tsRemainingCostumeTime.TotalSeconds)
                {
                    // 일, 시
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00007"), tsRemainingCostumeTime.Days, tsRemainingCostumeTime.Hours);
                }
                else if (3600 <= tsRemainingCostumeTime.TotalSeconds)
                {
                    // 시, 분
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00008"), tsRemainingCostumeTime.Hours, tsRemainingCostumeTime.Minutes);
                }
                else
                {
                    // 분, 초
                    textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00009"), tsRemainingCostumeTime.Minutes, tsRemainingCostumeTime.Seconds);
                }
            }

            // 장착
            Transform trImageEquip = trCostumeItem.Find("ImageEquip");

            Text textEquip = trImageEquip.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textEquip);
            textEquip.text = CsConfiguration.Instance.GetString("A151_TXT_00004");

            Button buttonEquip = trCostumeItem.Find("ButtonEquip").GetComponent<Button>();
            buttonEquip.onClick.RemoveAllListeners();
            buttonEquip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonEquip.onClick.AddListener(() => OnClickHeroCostumeEquip(csHeroCostume));

            Text textButtonEquip = buttonEquip.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonEquip);

            if (CsCostumeManager.Instance.EquippedHeroCostumeId == csHeroCostume.HeroCostumeId)
            {
                // 장착
                buttonEquip.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_03");
                textButtonEquip.text = CsConfiguration.Instance.GetString("A151_TXT_00005");
                trImageEquip.gameObject.SetActive(true);
            }
            else
            {
                buttonEquip.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/btn_01");
                textButtonEquip.text = CsConfiguration.Instance.GetString("A151_TXT_00004");
                trImageEquip.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroCostumeRemainingTime()
    {
        Transform trCostumeItem = null;

        for (int i = 0; i < CsCostumeManager.Instance.HeroCostumeList.Count; i++)
        {
            trCostumeItem = m_trContent.Find("CostumeItem" + CsCostumeManager.Instance.HeroCostumeList[i].HeroCostumeId);

            CsHeroCostume csHeroCostume = CsCostumeManager.Instance.HeroCostumeList[i];

            if (trCostumeItem == null)
            {
                return;
            }
            else
            {
                if (csHeroCostume.Costume.PeriodLimitDay == 0)
                {
                    // 영구
                    continue;
                }
                else
                {
                    // 남은 시간
                    Text textCostumeTimer = trCostumeItem.Find("CostumeTime/TextTimer").GetComponent<Text>();
                    System.TimeSpan tsRemainingCostumeTime = System.TimeSpan.FromSeconds(csHeroCostume.RemainingTime - Time.realtimeSinceStartup);

                    if (86400 <= tsRemainingCostumeTime.TotalSeconds)
                    {
                        // 일, 시
                        textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00007"), tsRemainingCostumeTime.Days, tsRemainingCostumeTime.Hours);
                    }
                    else if (3600 <= tsRemainingCostumeTime.TotalSeconds)
                    {
                        // 시, 분
                        textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00008"), tsRemainingCostumeTime.Hours, tsRemainingCostumeTime.Minutes);
                    }
                    else
                    {
                        // 분, 초
                        textCostumeTimer.text = string.Format(CsConfiguration.Instance.GetString("A151_TXT_00009"), tsRemainingCostumeTime.Minutes, tsRemainingCostumeTime.Seconds);
                    }
                }
            }
        }
    }

    #region PopupCostumeEnchant

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCostumeEnchant()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupCostumeEnchant");
        yield return resourceRequest;

        m_trPopupCostumeEnchant = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        m_trPopupCostumeEnchant.name = "PopupCostumeEnchant";

        CsPopupCostumeEnchant csPopupCostumeEnchant = m_trPopupCostumeEnchant.GetComponent<CsPopupCostumeEnchant>();
        csPopupCostumeEnchant.EventClosePopupCostumeEnchant += OnEventClosePopupCostumeEnchant;

        m_bLoadPopupCostumeEnchant = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeEnchant()
    {
        if (m_trPopupCostumeEnchant == null)
        {
            return;
        }
        else
        {
            CsPopupCostumeEnchant csPopupCostumeEnchant = m_trPopupCostumeEnchant.GetComponent<CsPopupCostumeEnchant>();
            csPopupCostumeEnchant.EventClosePopupCostumeEnchant -= OnEventClosePopupCostumeEnchant;

            Destroy(m_trPopupCostumeEnchant.gameObject);
            m_trPopupCostumeEnchant = null;

            m_trCostume.gameObject.SetActive(true);
        }
    }

    #endregion PopupCostumeEnchant

    #region PopupCostumeEffect

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCostumeEffect()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupCostumeEffect");
        yield return resourceRequest;

        m_trPopupCostumeEffect = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        m_trPopupCostumeEffect.name = "PopupCostumeEffect";

        CsPopupCostumeEffect csPopupCostumeEffect = m_trPopupCostumeEffect.GetComponent<CsPopupCostumeEffect>();
        csPopupCostumeEffect.EventClosePopupCostumeEffect += OnEventClosePopupCostumeEffect;

        m_bLoadPopupCostumeEffect = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeEffect()
    {
        if (m_trPopupCostumeEffect == null)
        {
            return;
        }
        else
        {
            CsPopupCostumeEffect csPopupCostumeEffect = m_trPopupCostumeEffect.GetComponent<CsPopupCostumeEffect>();
            csPopupCostumeEffect.EventClosePopupCostumeEffect -= OnEventClosePopupCostumeEffect;

            Destroy(m_trPopupCostumeEffect.gameObject);
            m_trPopupCostumeEffect = null;

            m_trCostume.gameObject.SetActive(true);
        }
    }

    #endregion PopupCostumeEffect

    #region PopupCostumeCollection

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupCostumeCollection()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCharacter/PopupCostumeCollection");
        yield return resourceRequest;

        m_trPopupCostumeCollection = Instantiate((GameObject)resourceRequest.asset, m_trPopupList).transform;
        m_trPopupCostumeCollection.name = "PopupCostumeCollection";

        CsPopupCostumeCollection csPopupCostumeCollection = m_trPopupCostumeCollection.GetComponent<CsPopupCostumeCollection>();

        if (csPopupCostumeCollection == null)
        {
            
        }
        else
        {
            csPopupCostumeCollection.EventClosePopupCostumeCollection += OnEventClosePopupCostumeCollection;
        }

        m_bLoadPopupCostumeCollection = false;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupCostumeCollection()
    {
        if (m_trPopupCostumeCollection == null)
        {
            return;
        }
        else
        {
            CsPopupCostumeCollection csPopupCostumeCollection = m_trPopupCostumeCollection.GetComponent<CsPopupCostumeCollection>();

            if (csPopupCostumeCollection == null)
            {

            }
            else
            {
                csPopupCostumeCollection.EventClosePopupCostumeCollection -= OnEventClosePopupCostumeCollection;
            }

            Destroy(m_trPopupCostumeCollection.gameObject);
            m_trPopupCostumeCollection = null;

            m_trCostume.gameObject.SetActive(true);
        }
    }

    #endregion PopupCostumeCollection

    //---------------------------------------------------------------------------------------------------
    void LoadCharacterModel()		//캐릭터모델 동적로드함수
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = transform.Find("3DCharacter/Character" + nJobId);

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
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DCharacter"));

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
        int nJobId = 0;

        if (CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 || CsGameData.Instance.MyHeroInfo.Job.JobId == CsGameData.Instance.MyHeroInfo.Job.ParentJobId)
        {
            nJobId = CsGameData.Instance.MyHeroInfo.JobId;
        }
        else
        {
            nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId;
        }

        Transform trCharacterModel = transform.Find("3DCharacter/Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
            CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);

            csEquipment.MidChangeEquipments(csHeroCustomData, false);
            csEquipment.CreateWing(csHeroCustomData, null, true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OpenPopupCostumeEffect()
    {
        Debug.Log("OnEventOpenPopupCostumeEffect");
        if (!m_bLoadPopupCostumeEffect || !m_bLoadPopupCostumeEnchant || !m_bLoadPopupCostumeCollection)
        {
            m_bLoadPopupCostumeEffect = true;
            m_trCostume.gameObject.SetActive(false);
            StartCoroutine(LoadPopupCostumeEffect());
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupCostumeEnchant()
    {
        if (!m_bLoadPopupCostumeEffect || !m_bLoadPopupCostumeEnchant || !m_bLoadPopupCostumeCollection)
        {
            m_bLoadPopupCostumeEnchant = true;
            m_trCostume.gameObject.SetActive(false);
            StartCoroutine(LoadPopupCostumeEnchant());
        }
        else
        {
            return;
        }
    }
}