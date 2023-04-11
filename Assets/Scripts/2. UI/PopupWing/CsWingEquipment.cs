using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsWingEquipment : CsPopupSub
{
    [SerializeField] GameObject m_goToggleWing;

    Transform m_trContent;
    Transform m_trWingMemoryPieceSlotList;
    Transform m_trMemoryPieceList;
    Transform m_trImageLine;

    Text m_TextWingStep;
    Text m_TextWingName;
    Text m_TextMemoryPieceItem;
    Text m_TextDescription;
    Text m_TextNoInstall;

    Button m_buttonInstall;
    Button m_buttonAllInstall;

    int m_nSelectWingId = 0;
    int m_nSelectMemoryPieceType = 0;

    Dictionary<int, int> m_dicWingMemoryPieceSlotAccAttrValue = new Dictionary<int, int>();
    IEnumerator m_IEnumeratorAllMemoryPieceEquipment = null;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall += OnEventWingMemoryPieceInstall;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventWingMemoryPieceInstall -= OnEventWingMemoryPieceInstall;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventWingMemoryPieceInstall()
    {
        UpdateWingText();
        UpdateMemoryPiece();
        UpdateMemoryPieceInstallFrame();
        UpdateMemoryPieceSlotList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedWingItem(bool bIson, CsWing csWing)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_nSelectWingId = csWing.WingId;
            LoadWingModel(csWing);

            UpdateWingText();
            UpdateMemoryPiece();
            UpdateMemoryPieceInstallFrame();
            UpdateMemoryPieceSlotList();
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMemoryPiece(bool bIson, CsWingMemoryPieceType csWingMemoryPieceType)
    {
        if (bIson)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);

            m_nSelectMemoryPieceType = csWingMemoryPieceType.Type;
            m_TextDescription.text = CsConfiguration.Instance.GetString(csWingMemoryPieceType.Description);
        }
        else
        {

        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingMemoryPieceInstall()
    {
        // 전체 강화 중일때는 취소
        if (m_IEnumeratorAllMemoryPieceEquipment != null)
        {
            StopCoroutine(m_IEnumeratorAllMemoryPieceEquipment);
            m_IEnumeratorAllMemoryPieceEquipment = null;
        }

        if (CheckMemoryPieceTypeItemCount(CsGameData.Instance.GetWingMemoryPieceType(m_nSelectMemoryPieceType)))
        {
            CsCommandEventManager.Instance.SendWingMemoryPieceInstall(m_nSelectWingId, m_nSelectMemoryPieceType);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A118_TXT_00006"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWingAllMemoryPieceInstall()
    {
        if (m_IEnumeratorAllMemoryPieceEquipment != null)
        {
            StopCoroutine(m_IEnumeratorAllMemoryPieceEquipment);
            m_IEnumeratorAllMemoryPieceEquipment = null;
        }

        m_IEnumeratorAllMemoryPieceEquipment = AllMemoryPieceEquipment();
        StartCoroutine(m_IEnumeratorAllMemoryPieceEquipment);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsWing csWing = CsGameData.Instance.GetWing(CsGameData.Instance.MyHeroInfo.EquippedWingId);

        if (csWing != null && csWing.MemoryPieceInstallationEnabled)
        {
            m_nSelectWingId = csWing.WingId;
        }
        else
        {
            List<CsHeroWing> listCsHeroWing = new List<CsHeroWing>(CsGameData.Instance.MyHeroInfo.HeroWingList.FindAll(a => a.Wing.MemoryPieceInstallationEnabled == true).OrderBy(a => a.Wing.WingId).ToList());
            List<CsWing> listCsWing = new List<CsWing>(CsGameData.Instance.WingList.FindAll(a => a.MemoryPieceInstallationEnabled == true).OrderBy(a => a.WingId).ToList());

            for (int i = 0; i < listCsHeroWing.Count; i++)
            {
                if (listCsHeroWing[i].Wing.MemoryPieceInstallationEnabled)
                {
                    m_nSelectWingId = listCsHeroWing[i].Wing.WingId;
                    break;
                }
                else
                {
                    continue;
                }
            }

            if (m_nSelectWingId == 0)
            {
                for (int i = 0; i < listCsWing.Count; i++)
                {
                    if (listCsWing[i].MemoryPieceInstallationEnabled)
                    {
                        m_nSelectWingId = listCsWing[i].WingId;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        m_trContent = transform.Find("ImageBackground/Scroll View/Viewport/Content");

        UpdateWingContent();

        Transform trEquipmentFrame = transform.Find("EquipmentFrame");

        m_TextWingStep = trEquipmentFrame.Find("ImageNameBack/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextWingStep);

        m_TextWingName = trEquipmentFrame.Find("ImageNameBack/TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextWingName);

        UpdateWingText();

        // 3D Model Load
        LoadWingModel(CsGameData.Instance.GetWing(m_nSelectWingId));

        // Wing Attr List
        m_trWingMemoryPieceSlotList = trEquipmentFrame.Find("WingMemoryPieceSlotList");
        UpdateMemoryPieceSlotList();

        m_TextMemoryPieceItem = trEquipmentFrame.Find("TextMemoryPieceItem").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextMemoryPieceItem);
        m_TextMemoryPieceItem.text = CsConfiguration.Instance.GetString("A118_TXT_00007");

        m_TextDescription = trEquipmentFrame.Find("TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextDescription);

        // Button
        m_buttonInstall = trEquipmentFrame.Find("ButtonEquipment").GetComponent<Button>();
        m_buttonInstall.onClick.RemoveAllListeners();
        m_buttonInstall.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonInstall.onClick.AddListener(() => OnClickWingMemoryPieceInstall());

        m_buttonAllInstall = trEquipmentFrame.Find("ButtonAllEquipment").GetComponent<Button>();
        m_buttonAllInstall.onClick.RemoveAllListeners();
        m_buttonAllInstall.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonAllInstall.onClick.AddListener(() => OnClickWingAllMemoryPieceInstall());

        Text textButtonEquipment = m_buttonInstall.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonEquipment);
        textButtonEquipment.text = CsConfiguration.Instance.GetString("A118_TXT_00001");

        Text textButtonAllEquipment = m_buttonAllInstall.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAllEquipment);
        textButtonAllEquipment.text = CsConfiguration.Instance.GetString("A118_TXT_00002");

        m_trMemoryPieceList = trEquipmentFrame.Find("MemoryPieceList");

        m_TextNoInstall = trEquipmentFrame.Find("TextNoInstall").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_TextNoInstall);

        m_trImageLine = trEquipmentFrame.Find("ImageLine");
        
        Transform trItemSlot = null;

        for (int i = 0; i < CsGameData.Instance.WingMemoryPieceTypeList.Count; i++)
        {
            trItemSlot = m_trMemoryPieceList.Find("ItemSlot" + i);

            if (trItemSlot == null)
            {
                continue;
            }
            else
            {
                CsWingMemoryPieceType csWingMemoryPieceType = CsGameData.Instance.WingMemoryPieceTypeList[i];
                
                Toggle toggleItemSlot = trItemSlot.GetComponent<Toggle>();
                toggleItemSlot.onValueChanged.RemoveAllListeners();
                
                if (i == 0)
                {
                    toggleItemSlot.isOn = true;
                    m_nSelectMemoryPieceType = CsGameData.Instance.WingMemoryPieceTypeList[i].Type;

                    m_TextDescription.text = csWingMemoryPieceType.Description;
                }
                else
                {
                    toggleItemSlot.isOn = false;
                }

                toggleItemSlot.onValueChanged.AddListener((ison) => OnValueChangedMemoryPiece(ison, csWingMemoryPieceType));

                CsItem csItem = CsGameData.Instance.WingMemoryPieceTypeList[i].RequiredItem;

                Image imageFrameRank = trItemSlot.Find("ImageFrameRank").GetComponent<Image>();
                Image imageIcon = trItemSlot.Find("ImageIcon").GetComponent<Image>();

                imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csItem.Grade.ToString("00"));
                imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);

                trItemSlot.gameObject.SetActive(true);
            }
        }

        UpdateMemoryPiece();
        UpdateMemoryPieceInstallFrame();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingContent()
    {
        CsWing csWing = null;

        List<CsWing> listCsWing = new List<CsWing>(CsGameData.Instance.WingList.FindAll(a => a.MemoryPieceInstallationEnabled == true).OrderBy(a => a.WingId).ToList());
        List<CsHeroWing> listCsHeroWing = new List<CsHeroWing>(CsGameData.Instance.MyHeroInfo.HeroWingList.FindAll(a => a.Wing.MemoryPieceInstallationEnabled == true).OrderBy(a => a.Wing.WingId).ToList());

        Text textNoHeroWing = m_trContent.Find("TextNoHeroWing").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNoHeroWing);
        textNoHeroWing.text = CsConfiguration.Instance.GetString("A23_TXT_00003");
        
        // MyHeroWing
        for (int i = 0; i < listCsHeroWing.Count; i++)
        {
            csWing = listCsHeroWing[i].Wing;

            if (csWing == null)
            {
                continue;
            }
            else
            {
                CreateToggleWing(csWing, true, i);
                listCsWing.Remove(csWing);
            }
        }

        if (listCsWing.Count == 0)
        {
            textNoHeroWing.gameObject.SetActive(false);
        }
        else
        {
            textNoHeroWing.transform.SetSiblingIndex(listCsWing.Count);
            textNoHeroWing.gameObject.SetActive(true);

            // Wing
            for (int i = 1; i <= listCsWing.Count; i++)
            {
                csWing = listCsWing[i - 1];

                if (csWing == null)
                {
                    continue;
                }
                else
                {
                    CreateToggleWing(csWing, false, i + listCsHeroWing.Count);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateToggleWing(CsWing csWing, bool bMyHeroWing, int nSiblingIndex = 0)
    {
        Transform trToggleWing = m_trContent.Find("ToggleWing" + csWing.WingId);

        if (trToggleWing == null)
        {
            trToggleWing = Instantiate(m_goToggleWing, m_trContent).transform;
            trToggleWing.name = "ToggleWing" + csWing.WingId;
        }
        else
        {
            trToggleWing.gameObject.SetActive(true);
        }

        Image ImageIcon = trToggleWing.Find("ItemSlot/ImageIcon").GetComponent<Image>();
        ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/wing_" + csWing.WingId);

        Text textName = trToggleWing.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = CsConfiguration.Instance.GetString(csWing.Name);

        Text textPossesion = trToggleWing.Find("TextPossesion").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPossesion);

        if (bMyHeroWing)
        {
            textPossesion.text = CsConfiguration.Instance.GetString("A23_TXT_00001");
        }
        else
        {
            textPossesion.text = CsConfiguration.Instance.GetString(csWing.AcquisitionText);
        }

        Text textEquipment = trToggleWing.Find("TextEquipment").GetComponent<Text>();
        CsUIData.Instance.SetFont(textEquipment);

        Toggle toggleWing = trToggleWing.GetComponent<Toggle>();
        toggleWing.group = m_trContent.GetComponent<ToggleGroup>();
        toggleWing.onValueChanged.RemoveAllListeners();

        if (csWing.WingId == m_nSelectWingId)
        {
            textEquipment.text = CsConfiguration.Instance.GetString("A23_TXT_00002");
            textEquipment.transform.gameObject.SetActive(true);
            toggleWing.isOn = true;

            m_nSelectWingId = csWing.WingId;
        }
        else
        {
            textEquipment.text = "";
            textEquipment.transform.gameObject.SetActive(false);
            toggleWing.isOn = false;
        }

        toggleWing.onValueChanged.AddListener((ison) => OnValueChangedWingItem(ison, csWing));

        trToggleWing.SetSiblingIndex(nSiblingIndex);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWingText()
    {
        CsWing csWing = CsGameData.Instance.GetWing(m_nSelectWingId);
        CsHeroWing csHeroWing = CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.MemoryPieceInstallationEnabled == true && a.Wing.WingId == m_nSelectWingId);

        if (csWing == null) 
        {
            return;
        }
        else
        {
            m_TextWingName.text = csWing.Name;

            if (csHeroWing == null)
            {
                if (csWing.WingMemoryPieceStepList.Count > 0)
                {
                    m_TextWingStep.text = string.Format(CsConfiguration.Instance.GetString("A118_TXT_00003"), csWing.WingMemoryPieceStepList[0].Step);
                }
                else
                {
                    m_TextWingStep.text = "";
                }
            }
            else
            {
                m_TextWingStep.text = string.Format(CsConfiguration.Instance.GetString("A118_TXT_00003"), csHeroWing.MemoryPieceStep);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMemoryPieceSlotList()
    {
        CsWing csWing = CsGameData.Instance.GetWing(m_nSelectWingId);

        if (csWing == null)
        {
            return;
        }
        else
        {
            Transform trImageMomoryPiece = null;
            CsHeroWing csHeroWing = CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.MemoryPieceInstallationEnabled == true && a.Wing.WingId == m_nSelectWingId);

            for (int i = 0; i < m_trWingMemoryPieceSlotList.childCount; i++)
            {
                trImageMomoryPiece = m_trWingMemoryPieceSlotList.GetChild(i);

                if (i < csWing.WingMemoryPieceSlotList.Count)
                {
                    CsWingMemoryPieceSlot csWingMemoryPieceSlot = csWing.WingMemoryPieceSlotList[i];
                    Image imageAttrIcon = trImageMomoryPiece.Find("Image").GetComponent<Image>();

                    switch (csWingMemoryPieceSlot.Attr.EnAttr)
                    {
                        case EnAttr.PhysicalOffense:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_1");
                            break;

                        case EnAttr.MagicalOffense:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_3");
                            break;

                        case EnAttr.PhysicalDefense:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_2");
                            break;

                        case EnAttr.MagicalDefense:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_4");
                            break;

                        case EnAttr.EnchantFire:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_5");
                            break;

                        case EnAttr.ProtectFire:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_6");
                            break;

                        case EnAttr.EnchantElectric:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_7");
                            break;

                        case EnAttr.ProtectElectric:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_8");
                            break;

                        case EnAttr.EnchantDark:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_9");
                            break;

                        case EnAttr.ProtectDark:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_10");
                            break;

                        case EnAttr.EnchantLight:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_11");
                            break;

                        case EnAttr.ProtectLight:
                            imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_12");
                            break;

                        case EnAttr.Attack:
                            if (CsGameData.Instance.MyHeroInfo.Job.OffenseType == EnOffenseType.Physical)
                            {
                                imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_1");
                            }
                            else
                            {
                                imageAttrIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupWing/ico_wing_ability_3");
                            }
                            break;
                    }

                    Transform trWingAttrInfo = trImageMomoryPiece.Find("WingAttrInfo");
                    Transform trImageLock = imageAttrIcon.transform.Find("ImageLock");

                    Text textName = trWingAttrInfo.Find("TextName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textName);
                    textName.text = csWingMemoryPieceSlot.Attr.Name;

                    // 미획득
                    if (csHeroWing == null)
                    {
                        trWingAttrInfo.gameObject.SetActive(false);
                        trImageLock.gameObject.SetActive(true);
                    }
                    else
                    {
                        // 잠금
                        if (csHeroWing.MemoryPieceStep < csWingMemoryPieceSlot.OpenStep)
                        {
                            trWingAttrInfo.gameObject.SetActive(false);
                            trImageLock.gameObject.SetActive(true);
                        }
                        else
                        {
                            CsWingMemoryPieceSlotStep csWingMemoryPieceSlotStep = null;

                            for (int j = csWingMemoryPieceSlot.WingMemoryPieceSlotStepList.Count - 1; j >= 0; j--)
                            {
                                if (csWingMemoryPieceSlot.WingMemoryPieceSlotStepList[j].Step <= csHeroWing.MemoryPieceStep)
                                {
                                    csWingMemoryPieceSlotStep = csWingMemoryPieceSlot.WingMemoryPieceSlotStepList[j];
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            Transform trUiEffect = imageAttrIcon.transform.Find("SubGear_Soulstone_Equip");

                            Text textCount = imageAttrIcon.transform.Find("TextCount").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textCount);

                            int nAccAttrValue = csHeroWing.GetHeroWingMemoryPieceSlot(csWingMemoryPieceSlotStep.SlotIndex).AccAttrValue;
                            AddMemoryPieceSlotAccAttrValue(csWing.WingMemoryPieceSlotList[i].SlotIndex, nAccAttrValue, textCount, trUiEffect);

                            Slider sliderAccAttrValue = trWingAttrInfo.Find("Slider").GetComponent<Slider>();
                            sliderAccAttrValue.maxValue = csWingMemoryPieceSlotStep.AttrMaxValue;
                            sliderAccAttrValue.value = nAccAttrValue;

                            Text textAccAttrValue = trWingAttrInfo.Find("Text").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textAccAttrValue);
                            textAccAttrValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nAccAttrValue, csWingMemoryPieceSlotStep.AttrMaxValue);

                            trWingAttrInfo.gameObject.SetActive(true);
                            trImageLock.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    trImageMomoryPiece.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMemoryPiece()
    {
        Transform trItemSlot = null;
        CsHeroWing csHeroWing = CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.MemoryPieceInstallationEnabled == true && a.Wing.WingId == m_nSelectWingId);

        if (csHeroWing == null)
        {
            m_trMemoryPieceList.gameObject.SetActive(false);
        }
        else
        {
            if (CheckAllMemorySlotInstall(csHeroWing))
            {
                m_trMemoryPieceList.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < CsGameData.Instance.WingMemoryPieceTypeList.Count; i++)
                {
                    trItemSlot = m_trMemoryPieceList.Find("ItemSlot" + i);

                    if (trItemSlot == null)
                    {
                        continue;
                    }
                    else
                    {
                        CsWingMemoryPieceStep csWingMemoryPieceStep = CsGameData.Instance.GetWing(m_nSelectWingId).WingMemoryPieceStepList.Find(a => a.Step == csHeroWing.MemoryPieceStep);

                        if (csWingMemoryPieceStep == null)
                        {
                            continue;
                        }
                        else
                        {
                            int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.WingMemoryPieceTypeList[i].RequiredItem.ItemId);

                            Text textCount = trItemSlot.Find("Text").GetComponent<Text>();
                            CsUIData.Instance.SetFont(textCount);
                            textCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nCount, csWingMemoryPieceStep.RequiredMemoryPieceCount);

                            if (nCount < csWingMemoryPieceStep.RequiredMemoryPieceCount)
                            {
                                textCount.color = CsUIData.Instance.ColorRed;
                            }
                            else
                            {
                                textCount.color = CsUIData.Instance.ColorWhite;
                            }
                        }
                    }
                }

                m_trMemoryPieceList.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckMemoryPieceTypeItemCount(CsWingMemoryPieceType csWingMemoryPieceType)
    {
        if (csWingMemoryPieceType == null)
        {
            return false;
        }
        else
        {
            CsHeroWing csHeroWing = CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.MemoryPieceInstallationEnabled == true && a.Wing.WingId == m_nSelectWingId);

            if (csHeroWing == null)
            {
                return false;
            }
            else
            {
                CsWingMemoryPieceStep csWingMemoryPieceStep = CsGameData.Instance.GetWing(m_nSelectWingId).WingMemoryPieceStepList.Find(a => a.Step == csHeroWing.MemoryPieceStep);

                if (csWingMemoryPieceStep == null)
                {
                    return false;
                }
                else
                {
                    int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csWingMemoryPieceType.RequiredItem.ItemId);

                    if (nCount < csWingMemoryPieceStep.RequiredMemoryPieceCount)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AddMemoryPieceSlotAccAttrValue(int nSlotIndex, int nAccAttrValue, Text textCount = null, Transform trUiEffect = null)
    {
        if (m_dicWingMemoryPieceSlotAccAttrValue.ContainsKey(nSlotIndex))
        {
            int nValueChange = nAccAttrValue - m_dicWingMemoryPieceSlotAccAttrValue[nSlotIndex];
            m_dicWingMemoryPieceSlotAccAttrValue[nSlotIndex] = nAccAttrValue;

            if (nValueChange == 0)
            {
                return;
            }
            else if (nValueChange < 0)
            {
                textCount.text = nValueChange.ToString();
                trUiEffect.gameObject.SetActive(false);
            }
            else 
            {
                textCount.text = "+" + nValueChange.ToString();
                trUiEffect.gameObject.SetActive(false);
                trUiEffect.gameObject.SetActive(true);
            }

            if (textCount.GetComponent<Animation>().isPlaying)
            {
                textCount.GetComponent<Animation>().Stop();
                textCount.gameObject.SetActive(false);
            }

            textCount.gameObject.SetActive(true);
            textCount.GetComponent<Animation>().Play();
        }
        else
        {
            m_dicWingMemoryPieceSlotAccAttrValue.Add(nSlotIndex, nAccAttrValue);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator AllMemoryPieceEquipment()
    {
        CsWingMemoryPieceType csWingMemoryPieceType = CsGameData.Instance.GetWingMemoryPieceType(m_nSelectMemoryPieceType);

        if (csWingMemoryPieceType == null)
        {
            StopCoroutine(m_IEnumeratorAllMemoryPieceEquipment);
            m_IEnumeratorAllMemoryPieceEquipment = null;
            yield return null;
        }
        else
        {
            while (CheckMemoryPieceTypeItemCount(csWingMemoryPieceType) && !CheckAllMemorySlotInstall(CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.WingId == m_nSelectWingId)))
            {
                CsCommandEventManager.Instance.SendWingMemoryPieceInstall(m_nSelectWingId, m_nSelectMemoryPieceType);
                yield return new WaitForSeconds(1.2f);
            }

            StopCoroutine(m_IEnumeratorAllMemoryPieceEquipment);
            m_IEnumeratorAllMemoryPieceEquipment = null;

            if (CheckMemoryPieceTypeItemCount(csWingMemoryPieceType) == false)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A118_TXT_00006"));
            }
            else
            {
                yield return null;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMemoryPieceInstallFrame()
    {
        CsHeroWing csHeroWing = CsGameData.Instance.MyHeroInfo.HeroWingList.Find(a => a.Wing.MemoryPieceInstallationEnabled == true && a.Wing.WingId == m_nSelectWingId);

        // 획득 못함
        if (csHeroWing == null)
        {
            m_buttonInstall.gameObject.SetActive(false);
            m_buttonAllInstall.gameObject.SetActive(false);

            m_TextMemoryPieceItem.gameObject.SetActive(false);
            m_TextDescription.gameObject.SetActive(false);

            m_trImageLine.gameObject.SetActive(false);
            
            m_TextNoInstall.color = new Color32(229, 155, 155, 255);
            m_TextNoInstall.text = CsConfiguration.Instance.GetString("A118_TXT_00004");
            m_TextNoInstall.gameObject.SetActive(true);
        }
        else
        {
            // 최대 강화
            if (CheckAllMemorySlotInstall(csHeroWing))
            {
                m_buttonInstall.gameObject.SetActive(false);
                m_buttonAllInstall.gameObject.SetActive(false);

                m_TextMemoryPieceItem.gameObject.SetActive(false);
                m_TextDescription.gameObject.SetActive(false);

                m_trImageLine.gameObject.SetActive(false);

                m_TextNoInstall.color = new Color32(51, 153, 0, 255);
                m_TextNoInstall.text = CsConfiguration.Instance.GetString("A118_TXT_00005");
                m_TextNoInstall.gameObject.SetActive(true);
            }
            else
            {
                m_TextNoInstall.text = "";
                m_TextNoInstall.gameObject.SetActive(false);

                m_buttonInstall.gameObject.SetActive(true);
                m_buttonAllInstall.gameObject.SetActive(true);

                m_TextMemoryPieceItem.gameObject.SetActive(true);
                m_TextDescription.gameObject.SetActive(true);

                m_trImageLine.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckAllMemorySlotInstall(CsHeroWing csHeroWing)
    {
        if (csHeroWing == null)
        {
            return false;
        }
        else
        {
            CsWing csWing = CsGameData.Instance.GetWing(csHeroWing.Wing.WingId);
            List<CsWingMemoryPieceStep> listCsWingMemoryPieceStep = csWing.WingMemoryPieceStepList;

            if (csHeroWing.MemoryPieceStep == listCsWingMemoryPieceStep[listCsWingMemoryPieceStep.Count - 1].Step)
            {
                bool bAllInstall = true;

                for (int i = 0; i < csHeroWing.HeroWingMemoryPieceSlotList.Count; i++)
                {
                    if (csHeroWing.HeroWingMemoryPieceSlotList[i].AccAttrValue < csWing.GetWingMemoryPieceSlot(csHeroWing.HeroWingMemoryPieceSlotList[i].Index).GetWingMemoryPieceSlotStep(csHeroWing.MemoryPieceStep).AttrMaxValue)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                return bAllInstall;
            }
            else
            {
                return false;
            }
        }
    }

    #region 3DModel

    //---------------------------------------------------------------------------------------------------
    void LoadWingModel(CsWing csWing)
    {
        Transform tr3DWing = transform.Find("EquipmentFrame/3DWing");

        for (int i = 0; i < tr3DWing.childCount; i++)
        {
            tr3DWing.GetChild(i).gameObject.SetActive(false);
        }

        tr3DWing.Find("UIChar_Camera_Wing").gameObject.SetActive(true);

        if (csWing == null)
        {
            return;
        }
        else
        {
            Transform trWingModel = tr3DWing.Find(csWing.PrefabName);

            if (trWingModel == null)
            {
                StartCoroutine(LoadWingModelCoroutine(csWing));
            }
            else
            {
                trWingModel.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadWingModelCoroutine(CsWing csWing)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/WingObject/" + csWing.PrefabName);
        yield return resourceRequest;
        GameObject goWing = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("EquipmentFrame/3DWing"));

        int nLayer = LayerMask.NameToLayer("UIChar");
        Transform[] atrWing = goWing.GetComponentsInChildren<Transform>();

        for (int i = 0; i < atrWing.Length; ++i)
        {
            atrWing[i].gameObject.layer = nLayer;
        }

        goWing.name = csWing.PrefabName;

        goWing.transform.localPosition = new Vector3(0, -120, 500);
        goWing.transform.eulerAngles = new Vector3(0, 90.0f, -90.0f);
        goWing.transform.localScale = new Vector3(350, 350, 350);

        goWing.gameObject.SetActive(true);
    }

    #endregion 3DModel
}