using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-24)
//---------------------------------------------------------------------------------------------------

public enum EnPopupSetInfoType
{
    MainGear = 0,
    SubGear = 1,
}

public enum EnPopupSetInfoPosition
{
    MainGear = 0,
    SubGear = 1,
    InvenMainGear = 2,
    InvenSubGear = 3,
}

public class CsPopupSetInfo : MonoBehaviour
{
    [SerializeField] GameObject m_goTextAttrName;

    Transform m_trBack;
    Transform m_trNowAttrList;
    Transform m_trNextAttrList;

    Button m_buttonActivation;

    Text m_textSetName;
    Text m_textBattlePower;
    Text m_textActivation;
    Text m_textNowAttr;
    Text m_textNextAttr;
    Text m_textButtonActivation;

    public event Delegate EventClosePopupSetInfo;  // 아이템 정보창 종료

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate += OnEventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate += OnEventSubGearSoulstoneLevelSetActivate;

        InitialIzeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMainGearEnchantLevelSetActivate -= OnEventMainGearEnchantLevelSetActivate;
        CsGameEventUIToUI.Instance.EventSubGearSoulstoneLevelSetActivate -= OnEventSubGearSoulstoneLevelSetActivate;
    }

    //---------------------------------------------------------------------------------------------------
    void InitialIzeUI()
    {
        m_trBack = transform.Find("ImageBackground");

        m_textSetName = m_trBack.Find("TextSetName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textSetName);

        m_textBattlePower = m_trBack.Find("TextBattlePower").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBattlePower);

        m_textActivation = m_trBack.Find("TextActivation").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textActivation);

        m_textNowAttr = m_trBack.Find("NowAttr/TextNowAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNowAttr);

        m_trNowAttrList = m_trBack.Find("NowAttr/NowAttrList");
        m_trNextAttrList = m_trBack.Find("NextAttr/NextAttrList");

        m_textNextAttr = m_trBack.Find("NextAttr/TextNextAttr").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNextAttr);

        m_buttonActivation = m_trBack.Find("ButtonActivation").GetComponent<Button>();
        m_buttonActivation.onClick.RemoveAllListeners();

        m_textButtonActivation = m_buttonActivation.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textButtonActivation);

        Button buttonCancel = transform.Find("ButtonCancel").GetComponent<Button>();
        buttonCancel.onClick.RemoveAllListeners();
        buttonCancel.onClick.AddListener(OnClickClosePopup);
        buttonCancel.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

    }

    #region Event

    //----------------------------------------------------------------------------------------------------
    public void OnEventClosePopupSetInfo()
    {
        if (EventClosePopupSetInfo != null)
        {
            EventClosePopupSetInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMainGearEnchantLevelSetActivate()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A06_TXT_02004"));
        DisplayType(EnPopupSetInfoType.MainGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSoulstoneLevelSetActivate()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A10_TXT_02002"));
        DisplayType(EnPopupSetInfoType.SubGear);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopupSetInfo();
    }

    #endregion Event

    #region Event Handler

    //----------------------------------------------------------------------------------------------------
    void OnClickActivation(EnPopupSetInfoType enPoupSetInfoType)
    {
        if (enPoupSetInfoType == EnPopupSetInfoType.MainGear)
        {
            if (CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet())
            {
                CsCommandEventManager.Instance.SendMainGearEnchantLevelSetActivate(CsGameData.Instance.MyHeroInfo.MainGearEnchantLevelSetNo + 1);
            }
        }
        else if (enPoupSetInfoType == EnPopupSetInfoType.SubGear)
        {
            if (CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet())
            {
                CsCommandEventManager.Instance.SendSubGearSoulstoneLevelSetActivate(CsGameData.Instance.MyHeroInfo.SubGearSoulstoneLevelSetNo + 1);
            }
        }
    }

    //--------------------------------------------------------------------------------------------------- 
    void OnClickClosePopup()
    {
        ClosePopupSetInfo();
    }

    #endregion Event Handler

    public void SetPosition(EnPopupSetInfoPosition enPopupSetInfoPosition)
    {
        RectTransform rectTransform = m_trBack.GetComponent<RectTransform>();

        switch (enPopupSetInfoPosition)
        {
            case EnPopupSetInfoPosition.MainGear:
                rectTransform.anchorMin = new Vector2(0.5f, 0f);
                rectTransform.anchorMax = new Vector2(0.5f, 0f);
                rectTransform.pivot = new Vector2(0.5f, 0f);
                rectTransform.anchoredPosition = new Vector2(435, -222f);
                break;
            case EnPopupSetInfoPosition.SubGear:
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = new Vector2(88f, 165f);
                break;
            case EnPopupSetInfoPosition.InvenMainGear:
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = new Vector2(-272f, 111f);
                break;
            case EnPopupSetInfoPosition.InvenSubGear:
                rectTransform.anchorMin = new Vector2(0.5f, 1f);
                rectTransform.anchorMax = new Vector2(0.5f, 1f);
                rectTransform.pivot = new Vector2(0.5f, 1f);
                rectTransform.anchoredPosition = new Vector2(-272f, 173f);
                break;
            default:
                break;
        }
    }

    //----------------------------------------------------------------------------------------------------
    public void DisplayType(EnPopupSetInfoType enPoupSetInfoType, CsHeroInfo csHeroInfo = null)
    {
        m_buttonActivation.onClick.RemoveAllListeners();

        if (enPoupSetInfoType == EnPopupSetInfoType.MainGear)
        {
            m_textNowAttr.text = CsConfiguration.Instance.GetString("A06_TXT_00008");
            m_textNextAttr.text = CsConfiguration.Instance.GetString("A06_TXT_00009");
            m_textButtonActivation.text = CsConfiguration.Instance.GetString("A06_BTN_00005");

            if (csHeroInfo != null)
            {
                m_trBack.Find("NextAttr").gameObject.SetActive(false);
                m_buttonActivation.gameObject.SetActive(false);
                m_textActivation.gameObject.SetActive(false);
            }
            else if (CsGameData.Instance.MyHeroInfo.CheckMainGearEnchantLevelSet())
            {
                m_buttonActivation.gameObject.SetActive(true);
                m_buttonActivation.onClick.AddListener(() => { OnClickActivation(enPoupSetInfoType); });
                m_buttonActivation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                m_textActivation.gameObject.SetActive(false);
            }
            else
            {
                m_buttonActivation.gameObject.SetActive(false);
                m_textActivation.gameObject.SetActive(true);
            }

            for (int i = 0; i < m_trNowAttrList.childCount; ++i)
            {
                m_trNowAttrList.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < m_trNextAttrList.childCount; ++i)
            {
                m_trNextAttrList.GetChild(i).gameObject.SetActive(false);
            }

            CsMainGearEnchantLevelSet csMainGearEnchantLevelSetNow;
            CsMainGearEnchantLevelSet csMainGearEnchantLevelSetNext;

            if (csHeroInfo == null)
            {
                csMainGearEnchantLevelSetNow = CsGameData.Instance.GetMainGearEnchantLevelSet(CsGameData.Instance.MyHeroInfo.MainGearEnchantLevelSetNo);
                csMainGearEnchantLevelSetNext = CsGameData.Instance.GetMainGearEnchantLevelSet(CsGameData.Instance.MyHeroInfo.MainGearEnchantLevelSetNo + 1);
            }
            else
            {
                csMainGearEnchantLevelSetNow = CsGameData.Instance.GetMainGearEnchantLevelSet(csHeroInfo.MainGearEnchantLevelSetNo);
                csMainGearEnchantLevelSetNext = null;
            }

            if (csMainGearEnchantLevelSetNow != null)
            {
                m_textSetName.text = csMainGearEnchantLevelSetNow.Name;
                int nBattlePowerTotal = 0;

                for (int i = 0; i < csMainGearEnchantLevelSetNow.MainGearEnchantLevelSetAttrList.Count; ++i)
                {
                    CreateAttr(csMainGearEnchantLevelSetNow.MainGearEnchantLevelSetAttrList[i], m_trNowAttrList);
                    nBattlePowerTotal += csMainGearEnchantLevelSetNow.MainGearEnchantLevelSetAttrList[i].BattlePower;
                }

                m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), nBattlePowerTotal.ToString("#,###"));
            }
            else
            {
                m_textSetName.text = CsConfiguration.Instance.GetString("A06_TXT_00010");
                m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), "0");
            }

            int nTotalEnchantLevel = 0;

            if (csMainGearEnchantLevelSetNext != null)
            {
                for (int i = 0; i < csMainGearEnchantLevelSetNext.MainGearEnchantLevelSetAttrList.Count; ++i)
                {
                    CreateAttr(csMainGearEnchantLevelSetNext.MainGearEnchantLevelSetAttrList[i], m_trNextAttrList);
                }

                List<CsHeroMainGear> listHeroMainGear = CsGameData.Instance.MyHeroInfo.HeroMainGearEquippedList;

                for (int i = 0; i < listHeroMainGear.Count; i++)
                {
                    if (listHeroMainGear[i] != null)
                    {
                        nTotalEnchantLevel += listHeroMainGear[i].EnchantLevel;
                    }
                }

                if (csMainGearEnchantLevelSetNext != null)
                {
                    m_textActivation.text = string.Format(CsConfiguration.Instance.GetString("A06_TXT_01004"), csMainGearEnchantLevelSetNext.RequiredTotalEnchantLevel, nTotalEnchantLevel, csMainGearEnchantLevelSetNext.RequiredTotalEnchantLevel);
                }
            }
            else
            {
                m_textActivation.gameObject.SetActive(false);
            }
        }
        else if (enPoupSetInfoType == EnPopupSetInfoType.SubGear)
        {
            m_textNowAttr.text = CsConfiguration.Instance.GetString("A10_TXT_00003");
            m_textNextAttr.text = CsConfiguration.Instance.GetString("A10_TXT_00004");
            m_textButtonActivation.text = CsConfiguration.Instance.GetString("A10_BTN_00007");

            if (csHeroInfo != null)
            {
                m_trBack.Find("NextAttr").gameObject.SetActive(false);
                m_buttonActivation.gameObject.SetActive(false);
                m_textActivation.gameObject.SetActive(false);
            }
            else if (CsGameData.Instance.MyHeroInfo.CheckSubGearSoulstoneLevelSet())
            {
                m_buttonActivation.gameObject.SetActive(true);
                m_buttonActivation.onClick.AddListener(() => { OnClickActivation(enPoupSetInfoType); });
                m_buttonActivation.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                m_textActivation.gameObject.SetActive(false);
            }
            else
            {
                m_buttonActivation.gameObject.SetActive(false);
                m_textActivation.gameObject.SetActive(true);
            }

            for (int i = 0; i < m_trNowAttrList.childCount; ++i)
            {
                m_trNowAttrList.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < m_trNextAttrList.childCount; ++i)
            {
                m_trNextAttrList.GetChild(i).gameObject.SetActive(false);
            }

            CsSubGearSoulstoneLevelSet csSubGearSoulstoneLevelSetNow;
            CsSubGearSoulstoneLevelSet csSubGearSoulstoneLevelSetNext;

            if (csHeroInfo == null)
            {
                csSubGearSoulstoneLevelSetNow = CsGameData.Instance.GetSubGearSoulstoneLevelSet(CsGameData.Instance.MyHeroInfo.SubGearSoulstoneLevelSetNo);
                csSubGearSoulstoneLevelSetNext = CsGameData.Instance.GetSubGearSoulstoneLevelSet(CsGameData.Instance.MyHeroInfo.SubGearSoulstoneLevelSetNo + 1);
            }
            else
            {
                csSubGearSoulstoneLevelSetNow = CsGameData.Instance.GetSubGearSoulstoneLevelSet(csHeroInfo.SubGearSoulstoneLevelSetNo);
                csSubGearSoulstoneLevelSetNext = null;
            }

            if (csSubGearSoulstoneLevelSetNow != null)
            {
                m_textSetName.text = csSubGearSoulstoneLevelSetNow.Name;
                int nBattlePowerTotal = 0;

                for (int i = 0; i < csSubGearSoulstoneLevelSetNow.SubGearSoulstoneLevelSetAttrList.Count; ++i)
                {
                    CreateAttr(csSubGearSoulstoneLevelSetNow.SubGearSoulstoneLevelSetAttrList[i], m_trNowAttrList);
                    nBattlePowerTotal += csSubGearSoulstoneLevelSetNow.SubGearSoulstoneLevelSetAttrList[i].BattlePower;
                }

                m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), nBattlePowerTotal.ToString("#,###"));
            }
            else
            {
                m_textSetName.text = CsConfiguration.Instance.GetString("A10_TXT_00005");
                m_textBattlePower.text = string.Format(CsConfiguration.Instance.GetString("INPUT_CP"), "0");
            }

            int nTotalLevel = 0;

            if (csSubGearSoulstoneLevelSetNext != null)
            {

                for (int i = 0; i < csSubGearSoulstoneLevelSetNext.SubGearSoulstoneLevelSetAttrList.Count; ++i)
                {
                    CreateAttr(csSubGearSoulstoneLevelSetNext.SubGearSoulstoneLevelSetAttrList[i], m_trNextAttrList);
                }

                List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;

                for (int i = 0; i < listHeroSubGear.Count; i++)
                {
                    if (listHeroSubGear[i].Equipped)
                    {
                        for (int j = 0; j < listHeroSubGear[i].SoulstoneSocketList.Count; j++)
                        {
                            nTotalLevel += listHeroSubGear[i].SoulstoneSocketList[j].Item.Level;
                        }
                    }
                }

                m_textActivation.text = string.Format(CsConfiguration.Instance.GetString("A10_TXT_01003"), csSubGearSoulstoneLevelSetNext.RequiredTotalLevel, nTotalLevel, csSubGearSoulstoneLevelSetNext.RequiredTotalLevel);
            }
            else
            {
                m_textActivation.gameObject.SetActive(false);
            }
        }

    }

    //----------------------------------------------------------------------------------------------------
    void CreateAttr(CsMainGearEnchantLevelSetAttr csMainGearEnchantLevelSetAttr, Transform trParent)
    {
        Transform trAttr = trParent.Find(csMainGearEnchantLevelSetAttr.Attr.AttrId.ToString());

        if (trAttr == null)
        {
            trAttr = Instantiate(m_goTextAttrName, trParent).transform;
            trAttr.name = csMainGearEnchantLevelSetAttr.Attr.AttrId.ToString();
        }

        Text textAttrName = trAttr.GetComponent<Text>();
        textAttrName.text = csMainGearEnchantLevelSetAttr.Attr.Name;
        CsUIData.Instance.SetFont(textAttrName);

        Text textAttrValue = textAttrName.transform.Find("TextAttrValue").GetComponent<Text>();
        textAttrValue.text = csMainGearEnchantLevelSetAttr.AttrValueInfo.Value.ToString("#,##0");
        CsUIData.Instance.SetFont(textAttrValue);

        trAttr.gameObject.SetActive(true);
    }

    //----------------------------------------------------------------------------------------------------
    void CreateAttr(CsSubGearSoulstoneLevelSetAttr csSubGearSoulstoneLevelSetAttr, Transform trParent)
    {
        Transform trAttr = trParent.Find(csSubGearSoulstoneLevelSetAttr.Attr.AttrId.ToString());

        if (trAttr == null)
        {
            trAttr = Instantiate(m_goTextAttrName, trParent).transform;
            trAttr.name = csSubGearSoulstoneLevelSetAttr.Attr.AttrId.ToString();
        }

        Text textAttrName = trAttr.GetComponent<Text>();
        textAttrName.text = csSubGearSoulstoneLevelSetAttr.Attr.Name;
        CsUIData.Instance.SetFont(textAttrName);

        Text textAttrValue = textAttrName.transform.Find("TextAttrValue").GetComponent<Text>();
        textAttrValue.text = csSubGearSoulstoneLevelSetAttr.AttrValueInfo.Value.ToString("#,##0");
        CsUIData.Instance.SetFont(textAttrValue);

        trAttr.gameObject.SetActive(true);
    }

    //----------------------------------------------------------------------------------------------------
    void ClosePopupSetInfo()
    {
        Destroy(gameObject);
    }
}
