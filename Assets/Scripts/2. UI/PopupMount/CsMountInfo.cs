using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsMountInfo : CsPopupSub
{
    GameObject m_goPopupItemInfo;
    GameObject m_goPopupMountPotionAttr;

    Transform m_trPopupList;
    Transform m_trItemInfo;
    Transform m_trPopupMountPotionAttr;
    Transform m_trMountGearList;
    Transform m_trNoHeroMount;
    Transform m_trImageNotice;

    Button m_buttonPotionAttr;

    int m_nMountId = -1;

    CsPopupItemInfo m_csPopupItemInfo;
    CsPopupMountPotionAttr m_csPopupMountPotionAttr;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountSelected += OnEventMountSelected;
        CsGameEventUIToUI.Instance.EventMountGearUnequip += OnEventMountGearUnequip;
        CsGameEventUIToUI.Instance.EventMountEquip += OnEventMountEquip;
        CsGameEventUIToUI.Instance.EventMountLevelUp += OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp += OnEventMountAwakeningLevelUp;
        CsGameEventUIToUI.Instance.EventMountAttrPotionUse += OnEventMountAttrPotionUse;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMountSelected -= OnEventMountSelected;
        CsGameEventUIToUI.Instance.EventMountGearUnequip -= OnEventMountGearUnequip;
        CsGameEventUIToUI.Instance.EventMountEquip -= OnEventMountEquip;
        CsGameEventUIToUI.Instance.EventMountLevelUp -= OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp -= OnEventMountAwakeningLevelUp;
        CsGameEventUIToUI.Instance.EventMountAttrPotionUse -= OnEventMountAttrPotionUse;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountSelected(int nMountId)
    {
        m_nMountId = nMountId;
        string strPrefabName = string.Empty;

        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            //DisplayMount(CsGameData.Instance.GetMount(m_nMountId));
            m_trNoHeroMount.gameObject.SetActive(true);
            CsMount csMount = CsGameData.Instance.GetMount(m_nMountId);

            if (csMount == null)
            {
                return;
            }
            else
            {
                if (0 < csMount.MountQualityList.Count)
                {
                    strPrefabName = csMount.MountQualityList[0].PrefabName;
                }
                else
                {
                    return;
                }
            }
        }
        else
        {
            //DisplayMount(csHeroMount);
            m_trNoHeroMount.gameObject.SetActive(false);
            strPrefabName = csHeroMount.PrefabName;
        }

        UpdateAttr();
        UpdateAttrFactor();
        UpdateButtonPotionAttr();
        LoadMountModel(strPrefabName);

        m_trImageNotice.gameObject.SetActive(CheckMountPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountGearUnequip(Guid guid)
    {
        if (guid != Guid.Empty)
        {
            DisplayMountGear(CsGameData.Instance.MyHeroInfo.GetHeroMountGear(guid).MountGear.MountGearType.SlotIndex);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountEquip()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        UpdateAttrFactor();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountLevelUp(bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateAttr();
            m_trImageNotice.gameObject.SetActive(CheckMountPotionAttr());
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountAwakeningLevelUp()
    {
        UpdateAttrFactor();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountAttrPotionUse()
    {
        m_trImageNotice.gameObject.SetActive(CheckMountPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMountGearInfo(CsHeroMountGear csHeroMountGear)
    {
        if (csHeroMountGear != null)
        {
            if (m_goPopupItemInfo == null)
            {
                StartCoroutine(LoadPopupItemInfo(csHeroMountGear));
            }
            else
            {
                OpenPopupItemInfo(csHeroMountGear);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMountAttrPotion()
    {
        CsGameEventUIToUI.Instance.OnEventOpenPopupMountAttrPotion(m_nMountId);
    }
    
    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");

        m_trMountGearList = transform.Find("MountGearList");

        for (int i = 0; i < m_trMountGearList.childCount; i++)
        {
            DisplayMountGear(i);
        }

        m_buttonPotionAttr = transform.Find("ButtonPotionAttr").GetComponent<Button>();
        m_buttonPotionAttr.onClick.RemoveAllListeners();
        m_buttonPotionAttr.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonPotionAttr.onClick.AddListener(OnClickMountAttrPotion);

        m_nMountId = CsGameData.Instance.MyHeroInfo.EquippedMountId;
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            m_trNoHeroMount = transform.Find("TextNoHeroMount");
            Text textNoHeroMount = m_trNoHeroMount.GetComponent<Text>();
            CsUIData.Instance.SetFont(textNoHeroMount);
            textNoHeroMount.text = CsConfiguration.Instance.GetString("A19_TXT_00009");

            LoadMountModel(csHeroMount.PrefabName);
        }

        UpdateButtonPotionAttr();
        UpdateAttrFactor();
        UpdateAttr();

        m_trImageNotice = m_buttonPotionAttr.transform.Find("ImageNotice");
        m_trImageNotice.gameObject.SetActive(CheckMountPotionAttr());
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPotionAttr()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        bool bInteractable = csHeroMount == null;
        m_buttonPotionAttr.interactable = !bInteractable;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttrFactor()
    {
        Text textAttrFactor = transform.Find("ImageIcoMark/TextEquipmentAttrFactor").GetComponent<Text>();
        CsUIData.Instance.SetFont(textAttrFactor);

        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            textAttrFactor.text = CsConfiguration.Instance.GetString("A158_TXT_00001");
        }
        else
        {
            if (m_nMountId == CsGameData.Instance.MyHeroInfo.EquippedMountId)
            {
                textAttrFactor.text = CsConfiguration.Instance.GetString("A158_TXT_00001");
            }
            else
            {
                textAttrFactor.text = string.Format(CsConfiguration.Instance.GetString("A158_TXT_00002"), csHeroMount.MountAwakeningLevelMaster.UnequippedAttrFactor * 100);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAttr()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        Text textMountName = transform.Find("TextMountName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textMountName);

        Transform trMountAttrList = transform.Find("MountAttrList");

        if (csHeroMount == null)
        {
            CsMount csMount = CsGameData.Instance.GetMount(m_nMountId);
            textMountName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), 1, csMount.Name);

            trMountAttrList.gameObject.SetActive(false);
        }
        else
        {
            textMountName.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_NAME"), csHeroMount.Level, csHeroMount.Mount.Name);

            for (int i = 0; i < 6; ++i)
            {
                Text trMountAttrName = trMountAttrList.Find("MountAttr" + i + "/TextName").GetComponent<Text>();
                Text trMountAttrValue = trMountAttrList.Find("MountAttr" + i + "/TextValue").GetComponent<Text>();

                if (i == 0)
                {
                    trMountAttrName.text = CsConfiguration.Instance.GetString("A19_TXT_00005");
                    trMountAttrValue.text = string.Format(CsConfiguration.Instance.GetString("A19_TXT_01003"), csHeroMount.Mount.MoveSpeed / 100f);
                }
                else
                {
                    trMountAttrName.text = CsGameData.Instance.GetAttr((EnAttr)i).Name;
                    switch ((EnAttr)i)
                    {
                        case EnAttr.MaxHp:
                            trMountAttrValue.text = csHeroMount.MountLevel.MaxHp.ToString("#,##0");
                            break;
                        case EnAttr.PhysicalOffense:
                            trMountAttrValue.text = csHeroMount.MountLevel.PhysicalOffense.ToString("#,##0");
                            break;
                        case EnAttr.MagicalOffense:
                            trMountAttrValue.text = csHeroMount.MountLevel.MagicalOffense.ToString("#,##0");
                            break;
                        case EnAttr.PhysicalDefense:
                            trMountAttrValue.text = csHeroMount.MountLevel.PhysicalDefense.ToString("#,##0");
                            break;
                        case EnAttr.MagicalDefense:
                            trMountAttrValue.text = csHeroMount.MountLevel.MagicalDefense.ToString("#,##0");
                            break;
                    }
                }
                CsUIData.Instance.SetFont(trMountAttrName);
                CsUIData.Instance.SetFont(trMountAttrValue);
            }

            trMountAttrList.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountGear(int nSlotIndex)
    {
        Transform trMountGearSlot = m_trMountGearList.Find("MountGearSlot" + nSlotIndex);
        Transform trItemSlot = trMountGearSlot.Find("ItemSlot");
        Transform trLock = trMountGearSlot.Find("ImageLock");

        Image imageEquip = trMountGearSlot.Find("ImageEquip").GetComponent<Image>();

        int nOpenHeroLevel = CsGameData.Instance.GetMountGearTypeBySlotIndex(nSlotIndex).MountGearSlot.OpenHeroLevel;

        if (nOpenHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
        {
            CsHeroMountGear csHeroMountGear = CsGameData.Instance.MyHeroInfo.GetEquippedMountGearBySlotIndex(nSlotIndex);

            if (csHeroMountGear == null)
            {
                trItemSlot.gameObject.SetActive(false);

                imageEquip.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupMount/frm_pet_equip" + nSlotIndex);
                imageEquip.gameObject.SetActive(true);
            }
            else
            {
                CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMountGear);
                trItemSlot.gameObject.SetActive(true);

                imageEquip.gameObject.SetActive(false);

                Button buttonItem = trItemSlot.GetComponent<Button>();
                buttonItem.onClick.RemoveAllListeners();
                buttonItem.onClick.AddListener(() => OnClickMountGearInfo(csHeroMountGear));
                buttonItem.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            }

            trLock.gameObject.SetActive(false);
        }
        else
        {
            trLock.gameObject.SetActive(true);
            trItemSlot.gameObject.SetActive(false);
            imageEquip.gameObject.SetActive(false);

            Text textLockLevel = trLock.Find("TextLock").GetComponent<Text>();
            CsUIData.Instance.SetFont(textLockLevel);
            textLockLevel.text = string.Format(CsConfiguration.Instance.GetString("A19_TXT_01001"), nOpenHeroLevel);
        }
    }

    #region Mount3DLoad

    //---------------------------------------------------------------------------------------------------
    void LoadMountModel(string strPrefabName)
    {
        Transform tr3DMount = transform.Find("3DMount");

        for (int i = 0; i < tr3DMount.childCount; ++i)
        {
            if (tr3DMount.GetChild(i).GetComponent<Camera>() != null)
            {
                continue;
            }

            tr3DMount.GetChild(i).gameObject.SetActive(false);
        }

        if (strPrefabName == string.Empty)
        {
            return;
        }

        Transform trMountModel = transform.Find("3DMount/" + strPrefabName);

        if (trMountModel == null)
        {
            StartCoroutine(LoadMountModelCoroutine(strPrefabName));
        }
        else
        {
            trMountModel.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadMountModelCoroutine(string strPrefabName)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("Prefab/MountObject/" + strPrefabName);
        yield return resourceRequest;

        transform.Find("3DMount/UIChar_Camera_Horse").gameObject.SetActive(true);

        GameObject goMount = Instantiate<GameObject>((GameObject)resourceRequest.asset, transform.Find("3DMount"));

        int nLayer = LayerMask.NameToLayer("UIChar");
        Transform[] atrMount = goMount.GetComponentsInChildren<Transform>();

        for (int i = 0; i < atrMount.Length; ++i)
        {
            atrMount[i].gameObject.layer = nLayer;
        }

        goMount.transform.localPosition = new Vector3(40, -161, 440);
        goMount.transform.eulerAngles = new Vector3(0, 260, 0);
        goMount.transform.localScale = new Vector3(150, 150, 150);
        goMount.name = strPrefabName;
        goMount.gameObject.SetActive(true);
    }

    #endregion Mount3DLoad

    #region MountGearInfoPopup

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsHeroMountGear csHeroMountGear)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csHeroMountGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsHeroMountGear csHeroMountGear)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;

        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();
        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;
        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csHeroMountGear, true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

    #endregion MountGearInfoPopup

    //---------------------------------------------------------------------------------------------------
    bool CheckMountPotionAttr()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return false;
        }
        else
        {
            CsMountQuality csMountQuality = csHeroMount.Mount.GetMountQuality(csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality);

            if (csMountQuality != null && csHeroMount.PotionAttrCount < csMountQuality.PotionAttrMaxCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
