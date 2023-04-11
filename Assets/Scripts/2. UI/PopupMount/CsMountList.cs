using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsMountList : CsPopupSub
{
    [SerializeField] GameObject m_goToggleMount;

    Transform m_trMountListContent;

    int m_nMountId = -1;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountLevelUp += OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountEquip += OnEventMountEquip;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMountLevelUp -= OnEventMountLevelUp;
        CsGameEventUIToUI.Instance.EventMountEquip -= OnEventMountEquip;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountLevelUp(bool bLevelUp)
    {
        UpdateToggleMount();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountEquip()
    {
        List<CsHeroMount> listCsHeroMount = new List<CsHeroMount>(CsGameData.Instance.MyHeroInfo.HeroMountList);

        Transform trTextEquip = null;
        Transform trButtonEquip = null;

        for (int i = 0; i < listCsHeroMount.Count; ++i)
        {
            Transform trToggleMount = m_trMountListContent.Find(listCsHeroMount[i].Mount.MountId.ToString());

            if (trToggleMount == null)
            {
                continue;
            }
            else
            {
                trTextEquip = trToggleMount.Find("TextEquip");
                trButtonEquip = trToggleMount.Find("ButtonEquip");

                if (listCsHeroMount[i].Mount.MountId == CsGameData.Instance.MyHeroInfo.EquippedMountId)
                {
                    trButtonEquip.gameObject.SetActive(false);
                    trTextEquip.gameObject.SetActive(true);
                }
                else
                {
                    trButtonEquip.gameObject.SetActive(true);
                    trTextEquip.gameObject.SetActive(false);
                }
            }
        }

        listCsHeroMount.Clear();
        listCsHeroMount = null;
    }

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedMount(bool bIson, int nMountId)
    {
        if (bIson && m_nMountId != nMountId)
        {
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            m_nMountId = nMountId;
            CsGameEventUIToUI.Instance.OnEventMountSelected(nMountId);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMountEquip(int nMountId)
    {
        CsCommandEventManager.Instance.SendMountEquip(nMountId);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        if (CsGameData.Instance.MyHeroInfo.EquippedMountId == 0)
        {
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            return;
        }

        m_nMountId = CsGameData.Instance.MyHeroInfo.EquippedMountId;

        m_trMountListContent = transform.Find("MountListPanel/ImageBackground/Scroll View/Viewport/Content");
        DisplayMountList();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMountList()
    {
        //모든 탈것을 보유할 경우
        bool bAllHeroMount = true;

        List<CsHeroMount> listCsHeroMount = new List<CsHeroMount>(CsGameData.Instance.MyHeroInfo.HeroMountList).OrderBy(a => a.Mount.MountId).ToList();
        List<CsMount> listCsMount = new List<CsMount>(CsGameData.Instance.MountList).OrderBy(a => a.MountId).ToList();

        //플레이어가 가지고 있는 탈것
        for (int i = 0; i < listCsHeroMount.Count; ++i)
        {
            CreateMount(listCsHeroMount[i], listCsHeroMount[i].Mount.MountId == m_nMountId ? true : false);
        }

        for (int i = 0; i < listCsMount.Count; ++i)
        {
            //플레이어가 가지고 있지 않으면
            if (CsGameData.Instance.MyHeroInfo.GetHeroMount(listCsMount[i].MountId) == null)
            {
                CreateMount(listCsMount[i]);
                
                if (bAllHeroMount)
                {
                    bAllHeroMount = false;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
        }

        Text textNoMount = m_trMountListContent.Find("TextNoMount").GetComponent<Text>();

        if (bAllHeroMount)
        {
            textNoMount.gameObject.SetActive(false);
        }
        else
        {
            textNoMount.gameObject.SetActive(true);
            textNoMount.text = CsConfiguration.Instance.GetString("A19_TXT_00006");
            CsUIData.Instance.SetFont(textNoMount);
            textNoMount.transform.SetSiblingIndex(listCsHeroMount.Count);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMount(CsHeroMount csHeroMount, bool bFirst = false)
    {
        Transform trHeroMount = m_trMountListContent.Find(csHeroMount.Mount.MountId.ToString());
        Toggle toggleHeroMount;

        if (trHeroMount == null)
        {
            toggleHeroMount = Instantiate(m_goToggleMount, m_trMountListContent).GetComponent<Toggle>();
            toggleHeroMount.name = csHeroMount.Mount.MountId.ToString();
            toggleHeroMount.group = m_trMountListContent.GetComponent<ToggleGroup>();
        }
        else
        {
            toggleHeroMount = trHeroMount.GetComponent<Toggle>();
        }

        toggleHeroMount.onValueChanged.RemoveAllListeners();

        if (bFirst)
        {
            toggleHeroMount.isOn = true;
        }

        int nMountId = csHeroMount.Mount.MountId;
        toggleHeroMount.onValueChanged.AddListener((ison) => { OnValueChangedMount(ison, nMountId); });

        Text textLevel = toggleHeroMount.transform.Find("TextList/TextLevel").GetComponent<Text>();
        textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroMount.Level);
        CsUIData.Instance.SetFont(textLevel);

        Text textName = toggleHeroMount.transform.Find("TextList/TextName").GetComponent<Text>();
        textName.text = string.Format("<color={0}>{1}</color>", csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.ColorCode, csHeroMount.Mount.Name);
        CsUIData.Instance.SetFont(textName);

        Text textEquip = toggleHeroMount.transform.Find("TextEquip").GetComponent<Text>();
        textEquip.text = CsConfiguration.Instance.GetString("A19_TXT_00001");
        CsUIData.Instance.SetFont(textEquip);

        Text textAcquisition = toggleHeroMount.transform.Find("TextAcquisition").GetComponent<Text>();
        textAcquisition.gameObject.SetActive(false);
        CsUIData.Instance.SetFont(textAcquisition);

        Image ImageIcon = toggleHeroMount.transform.Find("ItemSlot/ImageIcon").GetComponent<Image>();
        ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/mount_" + csHeroMount.Mount.MountId + "_" + csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality);

        Button buttonEquip = toggleHeroMount.transform.Find("ButtonEquip").GetComponent<Button>();
        buttonEquip.onClick.RemoveAllListeners();
        buttonEquip.onClick.AddListener(() => { OnClickMountEquip(csHeroMount.Mount.MountId); });
        buttonEquip.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textButtonEquip = buttonEquip.transform.Find("Text").GetComponent<Text>();
        textButtonEquip.text = CsConfiguration.Instance.GetString("A19_BTN_00001");
        CsUIData.Instance.SetFont(textButtonEquip);

        if (csHeroMount.Mount.MountId == CsGameData.Instance.MyHeroInfo.EquippedMountId)
        {
            buttonEquip.gameObject.SetActive(false);
            textEquip.gameObject.SetActive(true);
        }
        else
        {
            buttonEquip.gameObject.SetActive(true);
            textEquip.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CreateMount(CsMount csMount)
    {
        Transform trHeroMount = m_trMountListContent.Find(csMount.MountId.ToString());
        Toggle toggleHeroMount;

        if (trHeroMount == null)
        {
            toggleHeroMount = Instantiate(m_goToggleMount, m_trMountListContent).GetComponent<Toggle>();
            toggleHeroMount.name = csMount.MountId.ToString();
            toggleHeroMount.group = m_trMountListContent.GetComponent<ToggleGroup>();
        }
        else
        {
            toggleHeroMount = trHeroMount.GetComponent<Toggle>();
        }

        toggleHeroMount.onValueChanged.RemoveAllListeners();
        int nMountId = csMount.MountId;
        toggleHeroMount.onValueChanged.AddListener((ison) => { OnValueChangedMount(ison, nMountId); });

        toggleHeroMount.transform.Find("TextList/TextLevel").gameObject.SetActive(false);

        Text textName = toggleHeroMount.transform.Find("TextList/TextName").GetComponent<Text>();
        textName.text = csMount.Name;
        CsUIData.Instance.SetFont(textName);

        Image ImageIcon = toggleHeroMount.transform.Find("ItemSlot/ImageIcon").GetComponent<Image>();
        ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/mount_" + csMount.MountId + "_" + csMount.MountQualityList[0].MountQualityMaster.Quality);

        Text textAcquisition = toggleHeroMount.transform.Find("TextAcquisition").GetComponent<Text>();
        textAcquisition.text = CsConfiguration.Instance.GetString(csMount.AcquisitionText);
        CsUIData.Instance.SetFont(textAcquisition);
        textAcquisition.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateToggleMount()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            Toggle toggleHeroMount = m_trMountListContent.Find(m_nMountId.ToString()).GetComponent<Toggle>();

            if (toggleHeroMount != null)
            {
                Text textLevel = toggleHeroMount.transform.Find("TextList/TextLevel").GetComponent<Text>();
                textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroMount.Level);
                Text textName = toggleHeroMount.transform.Find("TextList/TextName").GetComponent<Text>();
                textName.text = string.Format("<color={0}>{1}</color>", csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.ColorCode, csHeroMount.Mount.Name);
                Image ImageIcon = toggleHeroMount.transform.Find("ItemSlot/ImageIcon").GetComponent<Image>();
                ImageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/mount_" + csHeroMount.Mount.MountId + "_" + csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality);
            }
        }
    }
}