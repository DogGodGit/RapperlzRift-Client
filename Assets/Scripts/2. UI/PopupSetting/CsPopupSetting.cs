using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CsPopupSetting : CsUpdateableMonoBehaviour, IPopupMain
{
    GameObject m_goToggleSubMenu;

    Transform m_trBackground;
    Transform m_trToggleList;
    //Transform m_trTodayTaskPopupSubMenu;
    //Transform m_trPopupList;
    
    Text m_textPopupName;

    CsMainMenu m_csMainMenu;

    CsSubMenu m_csSubMenu;
    EnSubMenu m_enSubMenu;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trBackground = transform.Find("ImageBackground");

        m_textPopupName = m_trBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textPopupName);

        m_trToggleList = m_trBackground.Find("ToggleSettingMenuList");

        //m_trTodayTaskPopupSubMenu = transform.Find("PopupSubMenu");

        //Transform Canvas2 = GameObject.Find("Canvas2").transform;
        //m_trPopupList = Canvas2.Find("PopupList");

        CsGameEventUIToUI.Instance.EventPopupClose += OnEventPopupClose;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // 버튼 초기화
        Button buttonClosePopup = m_trBackground.Find("ButtonExit").GetComponent<Button>();
        buttonClosePopup.onClick.RemoveAllListeners();
        buttonClosePopup.onClick.AddListener(OnClickClosePopup);
        buttonClosePopup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventPopupClose -= OnEventPopupClose;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventPopupClose()
    {
        Destroy(gameObject);
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSubmenuTab(Toggle toggleSubmenuTab, int nToggleIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nToggleIndex];
        Transform trSubMenus = transform.Find("PopupSubMenu1");

        if (toggleSubmenuTab.isOn)
        {
            for (int i = 0; i < m_csSubMenu.Layout; i++)
            {
                LoadSubMenu(m_csSubMenu.Prefabs[i], trSubMenus);
            }
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            for (int j = 0; j < trSubMenus.childCount; j++)
            {
                trSubMenus.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    #endregion Event


    #region Implement Interface

    //---------------------------------------------------------------------------------------------------
    public CsSubMenu GetCurrentSubMenu()
    {
        return m_csSubMenu;
    }

    #endregion Implement Interface

    //---------------------------------------------------------------------------------------------------
    public void DisplayMenu(CsMainMenu csMainMenu, EnSubMenu enSubMenuID)
    {
        m_csMainMenu = csMainMenu;
        m_enSubMenu = enSubMenuID;
        m_textPopupName.text = m_csMainMenu.Name;

        StartCoroutine(LoadResourceToggleSubMenu());
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadResourceToggleSubMenu()
    {
        ResourceRequest resourceRequestToggle = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupSetting/ToggleSettingMenu");
        yield return resourceRequestToggle;
        m_goToggleSubMenu = (GameObject)resourceRequestToggle.asset;

        DisplaySubmenuTab();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplaySubmenuTab()
    {
        bool bSelected = false;

        for (int i = 0; i < m_csMainMenu.SubMenuList.Count; i++)
        {
            int nToggleIndex = i;

            Toggle toggle = Instantiate(m_goToggleSubMenu, m_trToggleList).GetComponent<Toggle>();
            toggle.name = "Toggle" + nToggleIndex;
            toggle.isOn = false;
            toggle.group = m_trToggleList.GetComponent<ToggleGroup>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((ison) => OnValueChangedSubmenuTab(toggle, nToggleIndex));
            toggle.gameObject.SetActive(true);

            //탭이름
            Text textSubmenuTab = toggle.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSubmenuTab);
            textSubmenuTab.text = m_csMainMenu.SubMenuList[i].Name;

            //디폴트설정
            if (m_enSubMenu == EnSubMenu.Default)
            {
                if (m_csMainMenu.SubMenuList[i].IsDefault)
                {
                    bSelected = toggle.isOn = true;
                }
            }
            else
            {
                if (m_csMainMenu.SubMenuList[i].SubMenuId == (int)m_enSubMenu)
                {
                    bSelected = toggle.isOn = true;
                }
            }
        }
        // 디플트값이 없을 경우, 첫번째 탭을 선택한다.
        if (!bSelected)
        {
            Toggle toggle = m_trToggleList.Find("Toggle0").GetComponent<Toggle>();
            toggle.isOn = true;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void LoadSubMenu(string strPath, Transform trSubMenu)
    {
        if (!string.IsNullOrEmpty(strPath))
        {
            string strSubName = strPath;

            if (strPath.LastIndexOf("/") > -1)
            {
                strSubName = strPath.Substring(strPath.LastIndexOf("/") + 1);
            }

            Transform trSubMenuPrefab = trSubMenu.Find(strSubName);

            if (trSubMenuPrefab == null)
            {
                StartCoroutine(LoadSubMenuCoroutine(strPath, trSubMenu, strSubName));
            }
            else
            {
                trSubMenuPrefab.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadSubMenuCoroutine(string strPath, Transform trSubMenu, string strSubMenuName)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>(strPath);
        yield return resourceRequest;
        GameObject goSubMenu = Instantiate((GameObject)resourceRequest.asset, trSubMenu);
        CsPopupSub csPopupSub = goSubMenu.GetComponent<CsPopupSub>();
        csPopupSub.PopupMain = this;
        goSubMenu.name = strSubMenuName;
    }

}
