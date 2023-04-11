using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-06)
//---------------------------------------------------------------------------------------------------

public class CsMainGearList : CsPopupSub
{
    [SerializeField] GameObject m_goToggleMainGear;

    Transform m_trContent;
    Transform m_trToggleShowAllMainGear;

    Text m_textEmpty;

    bool m_bFirst = true;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMainGearEnchant += OnEventMainGearEnchant;
        CsGameEventUIToUI.Instance.EventMainGearRefine += OnEventMainGearRefine;
        CsGameEventUIToUI.Instance.EventMainGearRefinementApply += OnEventMainGearRefineApply;
        CsGameEventUIToUI.Instance.EventMainGearTransit += OnEventMainGearTransit;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMainGearEnchant -= OnEventMainGearEnchant;
        CsGameEventUIToUI.Instance.EventMainGearRefine -= OnEventMainGearRefine;
        CsGameEventUIToUI.Instance.EventMainGearRefinementApply -= OnEventMainGearRefineApply;
        CsGameEventUIToUI.Instance.EventMainGearTransit -= OnEventMainGearTransit;

        CsUIData.Instance.MainGearId = Guid.Empty;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }
        else
        {
            UpdateMainGearList();
        }
    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchant(bool bSuccess, Guid guidMainGearId)
    {
        // 강화 메세지 전달
        UpdateMainGearSlot(guidMainGearId);
        CheckEquippedMainGearEnchant();

        // 강화 성공 / 실패시 토스트 메세지
        if (bSuccess)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A06_TXT_02001"));
            CsUIData.Instance.PlayUISound(EnUISoundType.MaingearReinforceSuccess);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A06_TXT_02002"));
            CsUIData.Instance.PlayUISound(EnUISoundType.MaingearReinforceFail);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearRefine(Guid guidMainGearId)
    {
        UpdateMainGearSlot(guidMainGearId);

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A07_TXT_02001"));
        CsUIData.Instance.PlayUISound(EnUISoundType.MaingearEnchanting);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearRefineApply(Guid guidMainGearId)
    {
        UpdateMainGearSlot(guidMainGearId);

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A07_TXT_02003"));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearTransit(Guid guidMainGearId, Guid guidMainGearMaterialId)
    {
        UpdateMainGearList();
        UpdateMainGearSlot(guidMainGearId);

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A08_TXT_02001"));
        CsUIData.Instance.PlayUISound(EnUISoundType.MaingearTransition);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMainGear(bool bIson, Guid guidMainGearId)
    {
        if (bIson)
        {
            CsUIData.Instance.MainGearId = guidMainGearId;
            CsGameEventUIToUI.Instance.OnEventMainGearSelected(guidMainGearId);
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedDisplayAll(bool bIson)
    {
        Text textShowAllMainGear = m_trToggleShowAllMainGear.Find("Label").GetComponent<Text>();

        if (bIson)
        {
            DisplayAll();
            textShowAllMainGear.color = CsUIData.Instance.ColorWhite;
        }
        else
        {
            if (!CheckEquipMainGear(CsUIData.Instance.MainGearId))
            {
                CsUIData.Instance.MainGearId = Guid.Empty;
            }

            DisplayEquipList();

            if (CsUIData.Instance.MainGearId == Guid.Empty)
            {
                CsGameEventUIToUI.Instance.OnEventMainGearSelected(CsUIData.Instance.MainGearId);
            }

            textShowAllMainGear.color = CsUIData.Instance.ColorGray;
        }

        UpdateHeroMainGearEmptyText();
    }

    #endregion

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        // 모든 장비 보여주기 토글 연결 및 초기화
        m_trToggleShowAllMainGear = transform.Find("Scroll View/Toggle");

        Text textShowAllMainGear = m_trToggleShowAllMainGear.Find("Label").GetComponent<Text>();
        CsUIData.Instance.SetFont(textShowAllMainGear);
        textShowAllMainGear.text = CsConfiguration.Instance.GetString("A06_TXT_00002");
        textShowAllMainGear.color = CsUIData.Instance.ColorGray;

        Toggle toggleShowAllMainGear = m_trToggleShowAllMainGear.GetComponent<Toggle>();
        toggleShowAllMainGear.isOn = false;
        toggleShowAllMainGear.onValueChanged.AddListener((ison) => OnValueChangedDisplayAll(ison));
        toggleShowAllMainGear.onValueChanged.AddListener((ison) => CsUIData.Instance.PlayUISound(EnUISoundType.Toggle));

        // 장비 리스트 토글 창 연결
        m_trContent = transform.Find("Scroll View/Viewport/Content");

        m_textEmpty = transform.Find("TextEmpty").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textEmpty);
        m_textEmpty.text = CsConfiguration.Instance.GetString("A06_TXT_00007");

        FirstMainGearCheck();
        CheckEquippedMainGearEnchant();
    }

    //---------------------------------------------------------------------------------------------------
    void FirstMainGearCheck()
    {
        // 장착 장비 리스트
        List<CsHeroMainGear> listHeroMainGearEquip = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

        Transform trToggleMainGear;

        bool bEquip = true;

        for (int i = 0; i < listHeroMainGearEquip.Count; i++)
        {
            // 장착 장비가 비어 있을 때
            if (listHeroMainGearEquip[i] == null)
                continue;

            Toggle toggleMainGear = null;

            trToggleMainGear = CreateMainGear(listHeroMainGearEquip[i]);
            toggleMainGear = trToggleMainGear.GetComponent<Toggle>();

            if (CsUIData.Instance.MainGearId == Guid.Empty)
            {
                CsUIData.Instance.MainGearId = listHeroMainGearEquip[i].Id;
                toggleMainGear.isOn = true;
            }

            CsHeroMainGear csHeroMainGear = listHeroMainGearEquip[i];
            toggleMainGear.onValueChanged.AddListener((ison) => OnValueChangedMainGear(ison, csHeroMainGear.Id));
            toggleMainGear.group = m_trContent.GetComponent<ToggleGroup>();

            DisplayMainGear(trToggleMainGear, listHeroMainGearEquip[i], bEquip);
        }

        UpdateHeroMainGearEmptyText();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearSlot(Guid guidMainGearId)
    {
        Transform trToggleMainGear = m_trContent.Find(guidMainGearId.ToString());

        bool bEquip = true;

        if (trToggleMainGear != null)
        {
            CsHeroMainGear csHeroMainGear = CsGameData.Instance.MyHeroInfo.GetHeroMainGear(guidMainGearId);

            if (CheckEquipMainGear(csHeroMainGear.Id) == true)
            {
                bEquip = true;
            }
            else
            {
                bEquip = false;
            }

            DisplayMainGear(trToggleMainGear, csHeroMainGear, bEquip);
        }

        CsGameEventUIToUI.Instance.OnEventMainGearSelected(guidMainGearId);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMainGearList()
    {
        Toggle toggleShowAllMainGear = m_trToggleShowAllMainGear.GetComponent<Toggle>();

        if (toggleShowAllMainGear.isOn)
        {
            List<CsHeroMainGear> listHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearList;

            for (int i = 0; i < listHeroMainGear.Count; i++)
            {
                Guid guidMainGearId = listHeroMainGear[i].Id;
                Transform trMainGear = m_trContent.Find(guidMainGearId.ToString());
                Toggle toggleMainGear;

                if (trMainGear == null)
                {
                    trMainGear = CreateMainGear(listHeroMainGear[i]);

                    toggleMainGear = trMainGear.GetComponent<Toggle>();
                    toggleMainGear.group = m_trContent.GetComponent<ToggleGroup>();
                    toggleMainGear.onValueChanged.RemoveAllListeners();
                    toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));
                }
                else
                {
                    toggleMainGear = trMainGear.GetComponent<Toggle>();
                    toggleMainGear.onValueChanged.RemoveAllListeners();

                    if (CsUIData.Instance.MainGearId == guidMainGearId)
                    {
                        toggleMainGear.isOn = true;
                        CsGameEventUIToUI.Instance.OnEventMainGearSelected(guidMainGearId);
                        toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));

                        CsUIData.Instance.MainGearId = guidMainGearId;
                    }
                    else
                    {
                        toggleMainGear.isOn = false;
                        toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));
                    }
                }

                DisplayMainGear(trMainGear, listHeroMainGear[i], CheckEquipMainGear(guidMainGearId));
            }
        }
        else
        {
            List<CsHeroMainGear> listHeroMainGearEquip = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

            for (int i = 0; i < listHeroMainGearEquip.Count; i++)
            {
                if (listHeroMainGearEquip[i] == null)
                {
                    continue;
                }

                Guid guidMainGearId = listHeroMainGearEquip[i].Id;
                Transform trMainGear = m_trContent.Find(guidMainGearId.ToString());
                Toggle toggleMainGear;

                if (trMainGear == null)
                {
                    trMainGear = CreateMainGear(listHeroMainGearEquip[i]);

                    toggleMainGear = trMainGear.GetComponent<Toggle>();
                    toggleMainGear.group = m_trContent.GetComponent<ToggleGroup>();
                    toggleMainGear.onValueChanged.RemoveAllListeners();
                    toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));
                }
                else
                {
                    toggleMainGear = trMainGear.GetComponent<Toggle>();
                    toggleMainGear.onValueChanged.RemoveAllListeners();

                    if (CsUIData.Instance.MainGearId == guidMainGearId)
                    {
                        toggleMainGear.isOn = true;
                        CsGameEventUIToUI.Instance.OnEventMainGearSelected(guidMainGearId);
                        toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));

                        CsUIData.Instance.MainGearId = guidMainGearId;
                    }
                    else
                    {
                        toggleMainGear.isOn = false;
                        toggleMainGear.onValueChanged.AddListener((bIson) => OnValueChangedMainGear(bIson, guidMainGearId));
                    }
                }

                DisplayMainGear(trMainGear, listHeroMainGearEquip[i], true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckEquipMainGear(Guid guidHeroMainGearId)
    {
        List<CsHeroMainGear> listHeroMainGearEquip = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

        for (int i = 0; i < listHeroMainGearEquip.Count; i++)
        {
            if (listHeroMainGearEquip[i] == null)
                continue;

            if (listHeroMainGearEquip[i].Id == guidHeroMainGearId)
                return true;
        }

        return false;
    }

    // 장착 장비 리스트 보여주기
    //---------------------------------------------------------------------------------------------------
    void DisplayEquipList()
    {
        List<CsHeroMainGear> listHeroMainGearEqip = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

        m_trContent.localPosition = Vector3.zero;

        Transform trToggleMainGear = null;

        for (int i = 0; i < listHeroMainGearEqip.Count; i++)
        {
            if (listHeroMainGearEqip[i] == null)
                continue;

            if (CsUIData.Instance.MainGearId == Guid.Empty)
            {
                CsUIData.Instance.MainGearId = listHeroMainGearEqip[i].Id;

                trToggleMainGear = m_trContent.Find(listHeroMainGearEqip[i].Id.ToString());

                if (trToggleMainGear)
                {
                    Toggle toggleMainGear = trToggleMainGear.GetComponent<Toggle>();
                    toggleMainGear.isOn = true;
                }
            }
        }

        List<CsHeroMainGear> listHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearList;

        for (int i = 0; i < listHeroMainGear.Count; i++)
        {
            // 장착 장비일 경우
            if (CheckEquipMainGear(listHeroMainGear[i].Id))
                continue;

            trToggleMainGear = m_trContent.Find(listHeroMainGear[i].Id.ToString());

            if (trToggleMainGear)
            {
                trToggleMainGear.gameObject.SetActive(false);

                Toggle toggleMainGear = trToggleMainGear.GetComponent<Toggle>();

                if (toggleMainGear.isOn)
                {
                    toggleMainGear.isOn = false;
                }
            }
        }
    }

    // 보유 장비 리스트 보여주기
    //---------------------------------------------------------------------------------------------------
    void DisplayAll()
    {
        List<CsHeroMainGear> listHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearList;

        Transform trToggleMainGear = null;

        bool bEquip = false;

        // 장착 장비 외 모든 장비 보여주기
        for (int i = 0; i < listHeroMainGear.Count; i++)
        {
            // 장착 장비일 경우
            if (CheckEquipMainGear(listHeroMainGear[i].Id) == true)
                continue;

            trToggleMainGear = m_trContent.Find(listHeroMainGear[i].Id.ToString());
            Toggle toggleMainGear = null;

            if (trToggleMainGear)
            {
                trToggleMainGear.gameObject.SetActive(true);
                toggleMainGear = trToggleMainGear.GetComponent<Toggle>();
                toggleMainGear.onValueChanged.RemoveAllListeners();
                CsHeroMainGear csHeroMainGear = listHeroMainGear[i];
                toggleMainGear.onValueChanged.AddListener((ison) => OnValueChangedMainGear(ison, csHeroMainGear.Id));
            }
            else
            {
                trToggleMainGear = CreateMainGear(listHeroMainGear[i]);
                toggleMainGear = trToggleMainGear.GetComponent<Toggle>();
                toggleMainGear.onValueChanged.RemoveAllListeners();
                CsHeroMainGear csHeroMainGear = listHeroMainGear[i];
                toggleMainGear.onValueChanged.AddListener((ison) => OnValueChangedMainGear(ison, csHeroMainGear.Id));
                toggleMainGear.group = m_trContent.GetComponent<ToggleGroup>();
            }

            if (CsUIData.Instance.MainGearId == Guid.Empty)
            {
                CsUIData.Instance.MainGearId = listHeroMainGear[i].Id;
                toggleMainGear.isOn = true;
            }

            DisplayMainGear(trToggleMainGear, listHeroMainGear[i], bEquip);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroMainGearEmptyText()
    {
        if (CsUIData.Instance.MainGearId == Guid.Empty)
        {
            m_textEmpty.gameObject.SetActive(true);
        }
        else
        {
            m_textEmpty.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    Transform CreateMainGear(CsHeroMainGear csHeroMainGear)
    {
        Transform trMainGear = Instantiate(m_goToggleMainGear, m_trContent).transform;
        trMainGear.name = csHeroMainGear.Id.ToString();

        return trMainGear;
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMainGear(Transform trToggleMainGear, CsHeroMainGear csHeroMainGear, bool bEquip)
    {
        Transform trItemSlot = trToggleMainGear.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroMainGear);

        Text textParts = trToggleMainGear.Find("TextParts").GetComponent<Text>();
        CsUIData.Instance.SetFont(textParts);
        textParts.text = csHeroMainGear.MainGear.MainGearType.Name;

        Text textName = trToggleMainGear.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textName);
        textName.text = csHeroMainGear.MainGear.Name;

        Transform trEnchantList = trToggleMainGear.Find("EnchantList");
        int nEnchantLevel = csHeroMainGear.EnchantLevel;
        CsUIData.Instance.UpdateEnchantLevelIcon(trEnchantList, nEnchantLevel);

        // 장착중 텍스트 표시
        if (bEquip)
        {
            Text textEquipment = trToggleMainGear.Find("TextEquipment").GetComponent<Text>();
            CsUIData.Instance.SetFont(textEquipment);
            textEquipment.text = CsConfiguration.Instance.GetString("A06_TXT_00001");
            textEquipment.gameObject.SetActive(true);
        }

        switch (m_iPopupMain.GetCurrentSubMenu().EnSubMenu)
        {
            case EnSubMenu.MainGearEnchant:
                trEnchantList.gameObject.SetActive(true);
                break;

            case EnSubMenu.MainGearRefine:
                trEnchantList.gameObject.SetActive(false);
                break;

            case EnSubMenu.MainGearTransit:
                trEnchantList.gameObject.SetActive(true);
                break;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckEquippedMainGearEnchant()
    {   
        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList.Count; i++)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i] == null)
            {
                continue;
            }
            else
            {
                Transform trToggleMainGear = m_trContent.Find(CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].Id.ToString());
                Transform trImageNotice = trToggleMainGear.Find("ImageNotice");

                if (CsUIData.Instance.MenuContentOpen((int)EnMenuContentId.MainGearEnchant))
                {
                    if (CsGameData.Instance.MyHeroInfo.VipLevel.MainGearEnchantMaxCount > CsGameData.Instance.MyHeroInfo.MainGearEnchantDailyCount)
                    {
                        if (CsGameData.Instance.GetMainGearEnchantLevel(CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].EnchantLevel + 1) != null)
                        {
                            int nItemId = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList[i].MainGearEnchantLevel.MainGearEnchantStep.NextEnchantMaterialItem.ItemId;
                            int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(nItemId);

                            if (nCount > 0)
                            {
                                trImageNotice.gameObject.SetActive(true);
                            }
                            else
                            {
                                trImageNotice.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            trImageNotice.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        trImageNotice.gameObject.SetActive(false);
                    }
                }
                else
                {
                    trImageNotice.gameObject.SetActive(false);
                }
            }
        }
    }
}