using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupTodayTask : CsUpdateableMonoBehaviour, IPopupMain
{
    GameObject m_goToggleSubMenu;
    GameObject m_goButtonTodayTask;
    GameObject m_goPopupItemInfo;

    Transform m_trBackground;
    Transform m_trToggleList;
    Transform m_trPopupItemInfo;
    Transform m_trPopupList;

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

        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        //m_trPopupList = Canvas2.Find("PopupList");

        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        // 버튼 초기화
        Button buttonClosePopup = m_trBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClosePopup.onClick.RemoveAllListeners();
        buttonClosePopup.onClick.AddListener(OnClickPopupClose);
        buttonClosePopup.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopupClose();
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickTaskConsignment()
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel < CsGameConfig.Instance.TaskConsignmentRequiredVipLevel)
        {
			CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("A109_TXT_00015"));
        }
        else
        {
            StartCoroutine(LoadPopupTaskConsignment());
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        PopupClose();
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSubmenuTab(Toggle toggleSubmenuTab, int nToggleIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nToggleIndex];

        Transform[] atrSubMenus = new Transform[3];

        for (int i = 0; i < atrSubMenus.Length; i++)
        {
            atrSubMenus[i] = transform.Find("PopupSubMenu" + (i + 1));
        }

        if (toggleSubmenuTab.isOn)
        {
            for (int i = 0; i < m_csSubMenu.Layout; i++)
            {
                LoadSubMenu(m_csSubMenu.Prefabs[i], atrSubMenus[i]);
            }

            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
        }
        else
        {
            for (int i = 0; i < atrSubMenus.Length; i++)
            {
                Transform trPopupSubMenu = atrSubMenus[i];

                for (int j = 0; j < trPopupSubMenu.childCount; j++)
                {
                    trPopupSubMenu.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
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
        ResourceRequest resourceRequestToggle = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupTodayTask/ToggleTodayTaskContent");
        yield return resourceRequestToggle;
        m_goToggleSubMenu = (GameObject)resourceRequestToggle.asset;

        ResourceRequest resourceRequestButton = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupTodayTask/ButtonTodayTask");
        yield return resourceRequestButton;
        m_goButtonTodayTask = (GameObject)resourceRequestButton.asset;

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

            //디폴트설정
            if (m_enSubMenu == EnSubMenu.Default)
            {
                if (m_csMainMenu.SubMenuList[i].IsDefault)
                {
                    bSelected = toggle.isOn = true;
                    CreateSubMenu(nToggleIndex);
                }
            }
            else
            {
                if (m_csMainMenu.SubMenuList[i].SubMenuId == (int)m_enSubMenu)
                {
                    bSelected = toggle.isOn = true;
                    CreateSubMenu(nToggleIndex);
                }
            }

            toggle.group = m_trToggleList.GetComponent<ToggleGroup>();
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((ison) => OnValueChangedSubmenuTab(toggle, nToggleIndex));
            toggle.gameObject.SetActive(true);

            //탭이름
            Text textSubmenuTab = toggle.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textSubmenuTab);
            textSubmenuTab.text = m_csMainMenu.SubMenuList[i].Name;
        }
        // 디플트값이 없을 경우, 첫번째 탭을 선택한다.
        if (!bSelected)
        {
            Toggle toggle = m_trToggleList.Find("Toggle0").GetComponent<Toggle>();
            toggle.isOn = true;
        }

        Button buttonTaskConsignment = Instantiate(m_goButtonTodayTask, m_trToggleList).transform.GetComponent<Button>();
        buttonTaskConsignment.onClick.RemoveAllListeners();
        buttonTaskConsignment.onClick.AddListener(OnClickTaskConsignment);
        buttonTaskConsignment.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textTaskConsignment = buttonTaskConsignment.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTaskConsignment);
        textTaskConsignment.text = CsConfiguration.Instance.GetString("A109_TXT_00001");
    }

    //---------------------------------------------------------------------------------------------------
    void CreateSubMenu(int nIndex)
    {
        m_csSubMenu = m_csMainMenu.SubMenuList[nIndex];
        Transform[] atrSubMenus = new Transform[3];

        for (int i = 0; i < atrSubMenus.Length; i++)
        {
            atrSubMenus[i] = transform.Find("PopupSubMenu" + (i + 1));
        }

        for (int i = 0; i < m_csSubMenu.Layout; i++)
        {
            LoadSubMenu(m_csSubMenu.Prefabs[i], atrSubMenus[i]);
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

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupTaskConsignment()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupTaskConsignment/PopupTaskConsignment");
        yield return resourceRequest;

        GameObject goPopupTaskConsignment = (GameObject)resourceRequest.asset;
        Transform trPopupTaskConsignment = Instantiate(goPopupTaskConsignment, m_trPopupList).transform;
        trPopupTaskConsignment.name = "PopupTaskConsignment";
    }


    //---------------------------------------------------------------------------------------------------
    void PopupClose()
    {
        Destroy(gameObject);
    }
}
