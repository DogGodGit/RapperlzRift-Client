using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsWingAppearance : CsPopupSub
{
    [SerializeField] GameObject m_goToggleWing;
    [SerializeField] GameObject m_goOptionAttr;
    GameObject m_goWing;

    Transform m_trCanvas2;
    Transform m_trWingItemContent;
    Transform m_trPopupAccAttr;
    Transform m_trPivotHUD;

    Text m_textWingName;
    Text m_textAttrName;
    Text m_textAttrValue;

    Button m_buttonEquipment;
    Button m_buttonDetail;

    CsWing m_csWing;

    bool m_bFirst = true;
    bool m_bPopupFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventWingEquip += OnEventWingEquip;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventWingEquip -= OnEventWingEquip;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }

        Toggle toggleWingItem = null;

        for (int i = 0; i < CsGameData.Instance.WingList.Count; i++)
        {
            toggleWingItem = m_trWingItemContent.Find("ToggleWing" + CsGameData.Instance.WingList[i].WingId).GetComponent<Toggle>();

            if (toggleWingItem.isOn)
            {
                toggleWingItem.isOn = false;
            }
        }

        if (CsGameData.Instance.MyHeroInfo.EquippedWingId == 0)
        {
            return;
        }
        else
        {
            toggleWingItem = m_trWingItemContent.Find("ToggleWing" + CsGameData.Instance.MyHeroInfo.EquippedWingId).GetComponent<Toggle>();
            toggleWingItem.isOn = true;
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventWingEquip()
    {
        UpdateWingItemList();
        UpdateWingInfo();

        CsGameEventToIngame.Instance.OnEventWingEquip();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWingItem(bool bIson, CsWing csWing)
    {
        if (bIson)
        {
            m_csWing = csWing;
            UpdateWingInfo();
            LoadWingModel(csWing);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingEquip()
    {
        CsCommandEventManager.Instance.SendWingEquip(m_csWing.WingId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupOpen()
    {
        if (m_bPopupFirst)
        {
            InitializePopupUI();
            m_bPopupFirst = false;
        }
        m_trPopupAccAttr.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        m_trPopupAccAttr.gameObject.SetActive(false);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trCanvas2 = GameObject.Find("Canvas2").transform;
        
        Transform trImageFrameName = transform.Find("ImageFrameName");

        m_textWingName = trImageFrameName.Find("TextWingName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textWingName);

        m_textAttrName = transform.Find("TextAttrName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAttrName);

        m_textAttrValue = transform.Find("TextAttrValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAttrValue);

        m_buttonEquipment = transform.Find("ButtonEquipment").GetComponent<Button>();
        m_buttonEquipment.onClick.RemoveAllListeners();
        m_buttonEquipment.onClick.AddListener(() => OnClickWingEquip());
        m_buttonEquipment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonEquipment = m_buttonEquipment.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEquipment);
        textButtonEquipment.text = CsConfiguration.Instance.GetString("A23_BTN_00001");

        m_trWingItemContent = transform.Find("Scroll View/Viewport/Content");

        Text textNoPossesion = m_trWingItemContent.Find("TextNoPossesion").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoPossesion);
        textNoPossesion.text = CsConfiguration.Instance.GetString("A23_TXT_00003");

        UpdateWingItemList();

        m_buttonDetail = transform.Find("ButtonDetail").GetComponent<Button>();
        m_buttonDetail.onClick.RemoveAllListeners();
        m_buttonDetail.onClick.AddListener(() => OnClickPopupOpen());
        m_buttonDetail.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonDetail = m_buttonDetail.transform.Find("TextDetail").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonDetail);
        textButtonDetail.text = CsConfiguration.Instance.GetString("A23_TXT_00005");

        m_trPopupAccAttr = transform.Find("PopupAccumAttr");

        CsWing csWing = CsGameData.Instance.GetWing(CsGameData.Instance.MyHeroInfo.EquippedWingId);
        LoadWingModel(csWing);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializePopupUI()
    {
        Button buttonClose = m_trPopupAccAttr.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => OnClickPopupClose());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trImageBackground = m_trPopupAccAttr.Find("ImageBackground");

        Text textName = trImageBackground.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsConfiguration.Instance.GetString("A23_TXT_00005");

        Transform trOptionAttrContent = trImageBackground.Find("Scroll View/Viewport/Content");

        Dictionary<int, int> dicAttr = new Dictionary<int, int>();

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingList.Count; i++)
        {
            CsWing csWing = CsGameData.Instance.MyHeroInfo.HeroWingList[i].Wing;

            if (dicAttr.ContainsKey(csWing.Attr.AttrId))
            {
                dicAttr[csWing.Attr.AttrId] += csWing.AttrValueInfo.Value;
            }
            else
            {
                dicAttr.Add(csWing.Attr.AttrId, csWing.AttrValueInfo.Value);
            }
        }

        foreach (KeyValuePair<int, int> kv in dicAttr)
        {
            Transform trOptionAttr = Instantiate(m_goOptionAttr, trOptionAttrContent).transform;
            trOptionAttr.name = "OptionAttr" + kv.Key;

            Text textAttrName = trOptionAttr.Find("TextAttrName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrName);
            textAttrName.text = CsGameData.Instance.GetAttr(kv.Key).Name;

            Text textAttrValue = trOptionAttr.Find("TextAttrValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAttrValue);
            textAttrValue.text = kv.Value.ToString("#,##0");
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateToggleWing(CsWing csWing, bool isPossesion)
    {
        Transform trToggleWing = m_trWingItemContent.Find("ToggleWing" + csWing.WingId);

        if (trToggleWing == null)
        {
            trToggleWing = Instantiate(m_goToggleWing, m_trWingItemContent).transform;
            trToggleWing.name = "ToggleWing" + csWing.WingId;
        }

        Image ImageIcon = trToggleWing.Find("ItemSlot/ImageIcon").GetComponent<Image>();
        ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/wing_" + csWing.WingId);

        Text textName = trToggleWing.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsConfiguration.Instance.GetString(csWing.Name);

        Text textPossesion = trToggleWing.Find("TextPossesion").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPossesion);

        if (isPossesion)
        {
            textPossesion.text = CsConfiguration.Instance.GetString("A23_TXT_00001");
        }
        else
        {
            // 획득 정보
            textPossesion.text = CsConfiguration.Instance.GetString(csWing.AcquisitionText);
        }

        Toggle toggleWing = trToggleWing.GetComponent<Toggle>();
        toggleWing.group = m_trWingItemContent.GetComponent<ToggleGroup>();
        toggleWing.onValueChanged.RemoveAllListeners();
        toggleWing.onValueChanged.AddListener((ison) => OnValueChangedWingItem(ison, csWing));

        Text textEquipment = trToggleWing.Find("TextEquipment").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEquipment);
        textEquipment.text = CsConfiguration.Instance.GetString("A23_TXT_00002");

        if (csWing.WingId == CsGameData.Instance.MyHeroInfo.EquippedWingId)
        {
            textEquipment.transform.gameObject.SetActive(true);
            toggleWing.isOn = true;
        }
        else
        {
            textEquipment.transform.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingItemList()
    {
        Text textNoPossesion = m_trWingItemContent.Find("TextNoPossesion").GetComponent<Text>();

        List<CsWing> listPossesionWing = new List<CsWing>();
        List<CsWing> listNoPossesionWing = new List<CsWing>();

        // 전체 날개
        for (int i = 0; i < CsGameData.Instance.WingList.Count; i++)
        {
            if (IsPossesionWing(CsGameData.Instance.WingList[i].WingId))
            {
                CreateToggleWing(CsGameData.Instance.WingList[i], true);
                listPossesionWing.Add(CsGameData.Instance.WingList[i]);
            }
            else
            {
                CreateToggleWing(CsGameData.Instance.WingList[i], false);
                listNoPossesionWing.Add(CsGameData.Instance.WingList[i]);
            }
        }

        // 날개 아이템 리스트 오름차순 정렬
        listPossesionWing.Sort(CompareTo);
        listNoPossesionWing.Sort(CompareTo);

        for (int i = 0; i < listPossesionWing.Count; i++)
        {
            Transform trToggleWing = m_trWingItemContent.Find("ToggleWing" + listPossesionWing[i].WingId);
            trToggleWing.SetSiblingIndex(i);
        }

        if (listNoPossesionWing.Count == 0)
        {
            textNoPossesion.gameObject.SetActive(false);
        }
        else
        {
            textNoPossesion.transform.SetSiblingIndex(listPossesionWing.Count);

            for (int i = 0; i < listNoPossesionWing.Count; i++)
            {
                Transform trToggleWing = m_trWingItemContent.Find("ToggleWing" + listNoPossesionWing[i].WingId);
                trToggleWing.SetSiblingIndex(i + listPossesionWing.Count + 1);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingInfo()
    {
        m_textWingName.text = CsConfiguration.Instance.GetString(m_csWing.Name);
        m_textAttrName.text = CsGameData.Instance.GetAttr(m_csWing.Attr.AttrId).Name;
        m_textAttrValue.text = m_csWing.AttrValueInfo.Value.ToString("#,##0");

        if (IsPossesionWing(m_csWing.WingId))
        {
            if (m_csWing.WingId == CsGameData.Instance.MyHeroInfo.EquippedWingId)
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEquipment, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(m_buttonEquipment, true);
            }
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonEquipment, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool IsPossesionWing(int nWingId)
    {
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroWingList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWingList[i].Wing.WingId == nWingId)
            {
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(CsWing x, CsWing y)
    {
        if (x.WingId > y.WingId) return 1;
        else if (x.WingId < y.WingId) return -1;
        return 0;
    }

    //---------------------------------------------------------------------------------------------------
    void LoadWingModel(CsWing csWing)
    {
        Transform tr3DWing = transform.Find("3DWing");

        for (int i = 0; i < tr3DWing.childCount; i++)
        {
            tr3DWing.GetChild(i).gameObject.SetActive(false);
        }

        tr3DWing.Find("UIChar_Camera_WingedChar").gameObject.SetActive(true);

        if (csWing == null)
        {
            return;
        }
        else
        {
            int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

            Transform trCharacterModel = tr3DWing.Find("Character" + nJobId);
            
            if (trCharacterModel == null)
            {
                StartCoroutine(LoadWingModelCoroutine(csWing));
            }
            else
            {
                UpdateCharacterModel(csWing);
                trCharacterModel.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadWingModelCoroutine(CsWing csWing)
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/Common/Character" + nJobId);
        yield return resourceRequest;
        GameObject goCharacter = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DWing"));

        float flScale = 1 / m_trCanvas2.GetComponent<RectTransform>().localScale.x;

        int nLayer = LayerMask.NameToLayer("UIChar");
        Transform[] atrCharacter = goCharacter.GetComponentsInChildren<Transform>();

        for (int i = 0; i < atrCharacter.Length; ++i)
        {
            atrCharacter[i].gameObject.layer = nLayer;
        }

        switch (nJobId)
        {
            case (int)EnJob.Gaia:
                goCharacter.transform.localPosition = new Vector3(0, -20, 120);
                goCharacter.transform.eulerAngles = new Vector3(0, 160, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Asura:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 160, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Deva:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 160, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
            case (int)EnJob.Witch:
                goCharacter.transform.localPosition = new Vector3(0, -20, 100);
                goCharacter.transform.eulerAngles = new Vector3(0, 160, 0);
                goCharacter.transform.localScale = new Vector3(flScale, flScale, flScale);
                break;
        }

        goCharacter.name = "Character" + nJobId;
        goCharacter.gameObject.SetActive(true);

        UpdateCharacterModel(csWing);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCharacterModel(CsWing csWing)
    {
        int nJobId = CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 ? CsGameData.Instance.MyHeroInfo.Job.JobId : CsGameData.Instance.MyHeroInfo.Job.ParentJobId;

        Transform trCharacterModel = transform.Find("3DWing/Character" + nJobId);

        if (trCharacterModel != null)
        {
            CsEquipment csEquipment = trCharacterModel.GetComponent<CsEquipment>();
			CsHeroCustomData csHeroCustomData = new CsHeroCustomData(CsGameData.Instance.MyHeroInfo);
            csHeroCustomData.WingId = csWing.WingId;

			csEquipment.MidChangeEquipments(csHeroCustomData);
            csEquipment.CreateWing(csHeroCustomData, null, true);
        }
    }
}