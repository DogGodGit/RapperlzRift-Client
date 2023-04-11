using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-25)
//---------------------------------------------------------------------------------------------------

public class CsVipInfo : CsPopupSub
{

    [SerializeField] GameObject m_goToggleVip;
    [SerializeField] GameObject m_goTextVip;

    GameObject m_goPopupItemInfo;

    Transform m_trVipInfo;
    Transform m_trContentList;

    int m_nSelectVipLevel;

    Transform m_trContent;
    Transform m_trItemList;
    Transform m_trPopupList;
    Transform m_trItemInfo;

    Button m_buttonReceive;
    Text m_textReceived;

    CsPopupItemInfo m_csPopupItemInfo;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventVipLevelRewardReceive += OnEventVipLevelRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trItemInfo != null)
        {
            OnEventClosePopupItemInfo(EnPopupItemInfoPositionType.Center);
        }
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventVipLevelRewardReceive -= OnEventVipLevelRewardReceive;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedVip(bool bIson, int nVipLevel)
    {
        if (bIson && m_nSelectVipLevel != nVipLevel)
        {
            m_nSelectVipLevel = nVipLevel;
            DisplayVipInfo();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickDiaCharge()
    {
		CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
		CsGameEventUIToUI.Instance.OnEventOpenPopupCharging();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickReceive()
    {
        int nReceived = 0;
        nReceived = CsGameData.Instance.MyHeroInfo.ReceivedVipLevelRewardList.Find(a => a == m_nSelectVipLevel);

        if (CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel >= m_nSelectVipLevel && nReceived == 0)
        {
            CsCommandEventManager.Instance.SendVipLevelRewardReceive(m_nSelectVipLevel);
        }
        else
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Error, CsConfiguration.Instance.GetString("PUBLIC_PREPARING"));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupItemInfo(CsItemReward csItemReward)
    {
        if (m_goPopupItemInfo == null)
        {
            StartCoroutine(LoadPopupItemInfo(csItemReward));
        }
        else
        {
            OpenPopupItemInfo(csItemReward);
        }
    }

    #endregion Event

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventVipLevelRewardReceive()
    {
		UpdateVipNotice(m_nSelectVipLevel);
        DisplayVipInfo();
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform Canvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = Canvas2.Find("PopupList");
        m_nSelectVipLevel = CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel;

        m_trVipInfo = transform.Find("ImageTopBackground/VipInfo");

        m_trContentList = transform.Find("ImageLeftBackground/Scroll View/Viewport/Content");

        List<CsVipLevel> listVipLevel = CsGameData.Instance.VipLevelList;

        for (int i = 0; i < listVipLevel.Count; ++i)
        {
            CreateVip(listVipLevel[i]);
        }

        Button buttonDiaCharge = transform.Find("ImageTopBackground/ButtonDiaCharge").GetComponent<Button>();
        buttonDiaCharge.onClick.RemoveAllListeners();
        buttonDiaCharge.onClick.AddListener(OnClickDiaCharge);
        Text textDiaCharge = buttonDiaCharge.transform.Find("Text").GetComponent<Text>();
        textDiaCharge.text = CsConfiguration.Instance.GetString("A54_BTN_00001");
        CsUIData.Instance.SetFont(textDiaCharge);

        Transform trRightBack = transform.Find("ImageRightBackground");
        m_trItemList = trRightBack.Find("ItemList");
        m_trContent = trRightBack.Find("Scroll View/Viewport/Content");

        m_textReceived = trRightBack.Find("TextReceived").GetComponent<Text>();
        m_textReceived.text = CsConfiguration.Instance.GetString("A54_BTN_00002");
        CsUIData.Instance.SetFont(m_textReceived);

        m_buttonReceive = trRightBack.Find("ButtonReceive").GetComponent<Button>();
        m_buttonReceive.onClick.RemoveAllListeners();
        m_buttonReceive.onClick.AddListener(OnClickReceive);
        Text textButtonReceive = m_buttonReceive.transform.Find("Text").GetComponent<Text>();
        textButtonReceive.text = CsConfiguration.Instance.GetString("A54_BTN_00002");
        CsUIData.Instance.SetFont(textButtonReceive);

        DisplayVipList();
        DisplayVipInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void CreateVip(CsVipLevel csVipLevel)
    {
        int nVipLevel = csVipLevel.VipLevel;
        Transform trVip = m_trContentList.Find(nVipLevel.ToString());
        Toggle toggleVip;

        if (trVip == null)
        {
            trVip = Instantiate(m_goToggleVip, m_trContentList).transform;
            trVip.name = nVipLevel.ToString();
            toggleVip = trVip.GetComponent<Toggle>();
            toggleVip.group = m_trContentList.GetComponent<ToggleGroup>();
        }
        else
        {
            toggleVip = trVip.GetComponent<Toggle>();
        }

        toggleVip.onValueChanged.RemoveAllListeners();

        if (m_nSelectVipLevel == nVipLevel)
        {
            toggleVip.isOn = true;
        }

        toggleVip.onValueChanged.AddListener((ison) => { OnValueChangedVip(ison, nVipLevel); });

        Text textVip = trVip.Find("Text").GetComponent<Text>();
        textVip.text = string.Format(CsConfiguration.Instance.GetString("A54_TXT_01001"), csVipLevel.VipLevel);
        CsUIData.Instance.SetFont(textVip);

		UpdateVipNotice(nVipLevel);
    }

	//---------------------------------------------------------------------------------------------------
	void UpdateVipNotice(int nVipLevel)
	{
		Transform trVip = m_trContentList.Find(nVipLevel.ToString());

		if (trVip != null)
		{
			trVip.Find("ImageNotice").gameObject.SetActive(CsGameData.Instance.MyHeroInfo.CheckVipRewardReceivable(nVipLevel));
		}
	}

    //---------------------------------------------------------------------------------------------------
    void DisplayVipList()
    {
        int nHeroVipLevel = CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel;
        int nHeroVipPoint = CsGameData.Instance.MyHeroInfo.VipPoint;

        CsVipLevel csVipLevelNext = CsGameData.Instance.GetVipLevel(nHeroVipLevel + 1);
        int nHeroVipNextPoint = 0;
        
        if (csVipLevelNext == null)
        {
            nHeroVipNextPoint = CsGameData.Instance.GetVipLevel(nHeroVipLevel).RequiredAccVipPoint;
        }
        else
        {
            nHeroVipNextPoint = csVipLevelNext.RequiredAccVipPoint;
        }

        if (nHeroVipNextPoint < nHeroVipPoint)
        {
            nHeroVipPoint = nHeroVipNextPoint;
        }

        Text textVip = m_trVipInfo.Find("TextVip").GetComponent<Text>();
        textVip.text = string.Format(CsConfiguration.Instance.GetString("A54_TXT_01002"), nHeroVipLevel);
        CsUIData.Instance.SetFont(textVip);

        Slider sliderVipGuage = m_trVipInfo.Find("SliderVipGuage").GetComponent<Slider>();
        sliderVipGuage.value = (float)nHeroVipPoint / (float)nHeroVipNextPoint;
        Text textVipGuage = sliderVipGuage.transform.Find("Text").GetComponent<Text>();
        textVipGuage.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroVipPoint, nHeroVipNextPoint);
        CsUIData.Instance.SetFont(textVipGuage);

		m_trVipInfo.Find("ImageDia").gameObject.SetActive(csVipLevelNext != null);

        Text textDia = m_trVipInfo.Find("ImageDia/Text").GetComponent<Text>();
        textDia.text = string.Format(CsConfiguration.Instance.GetString("A54_TXT_01003"), (nHeroVipNextPoint - nHeroVipPoint), (nHeroVipLevel + 1));
        CsUIData.Instance.SetFont(textDia);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayVipInfo()
    {
        for (int i = 0; i < m_trItemList.childCount; ++i)
        {
            m_trItemList.GetChild(i).gameObject.SetActive(false);
        }

        CsVipLevel csVipLevel = CsGameData.Instance.GetVipLevel(m_nSelectVipLevel);
        List<CsVipLevelReward> listVipLevelReward = CsGameData.Instance.GetVipLevel(m_nSelectVipLevel).VipLevelRewardList;

        for (int i = 0; i < listVipLevelReward.Count; ++i)
        {
            Button buttonItem = m_trItemList.GetChild(i).GetComponent<Button>();
            buttonItem.onClick.RemoveAllListeners();
            CsItemReward csItemReward = listVipLevelReward[i].ItemReward;
            buttonItem.onClick.AddListener(() => { OnClickPopupItemInfo(csItemReward); });
            CsUIData.Instance.DisplayItemSlot(m_trItemList.GetChild(i), csItemReward.Item, csItemReward.ItemOwned, csItemReward.ItemCount, csItemReward.Item.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
            m_trItemList.GetChild(i).gameObject.SetActive(true);
        }

        if (m_nSelectVipLevel == 0)
        {
            m_buttonReceive.gameObject.SetActive(false);
            m_textReceived.gameObject.SetActive(false);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.ReceivedVipLevelRewardList.Contains(m_nSelectVipLevel))
			{
				m_buttonReceive.gameObject.SetActive(false);
				m_textReceived.gameObject.SetActive(true);
			}
			else
			{
				m_buttonReceive.gameObject.SetActive(true);
				m_textReceived.gameObject.SetActive(false);

				m_buttonReceive.interactable = m_nSelectVipLevel <= CsGameData.Instance.MyHeroInfo.VipLevel.VipLevel;
			}
        }

        for(int i = 0; i < m_trContent.childCount; ++i)
        {
            m_trContent.GetChild(i).gameObject.SetActive(false);
        }


        string[] astrDescription = csVipLevel.Description.Split(',');

        for (int i = 0; i < astrDescription.Length; ++i)
        {
            Transform trDes = m_trContent.Find(i.ToString());

            if (trDes == null)
            {
                trDes = Instantiate(m_goTextVip, m_trContent).transform;
                trDes.name = i.ToString();
            }

            Text textVip = trDes.Find("Text").GetComponent<Text>();
            textVip.text = astrDescription[i];
            CsUIData.Instance.SetFont(textVip);
            trDes.gameObject.SetActive(true);
        }
    }

    #region PopupItemInfo

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupItemInfo(CsItemReward csItemReward)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupItemInfo/PopupItemInfo");
        yield return resourceRequest;
        m_goPopupItemInfo = (GameObject)resourceRequest.asset;

        OpenPopupItemInfo(csItemReward);
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupItemInfo(CsItemReward csItemReward)
    {
        GameObject goPopupItemInfo = Instantiate(m_goPopupItemInfo, m_trPopupList);
        m_trItemInfo = goPopupItemInfo.transform;
        m_csPopupItemInfo = goPopupItemInfo.GetComponent<CsPopupItemInfo>();

        m_csPopupItemInfo.EventClosePopupItemInfo += OnEventClosePopupItemInfo;

        m_csPopupItemInfo.DisplayType(EnPopupItemInfoPositionType.Center, csItemReward.Item, csItemReward.ItemCount, csItemReward.ItemOwned, -1, false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupItemInfo(EnPopupItemInfoPositionType enPopupItemInfoPositionType)
    {
        m_csPopupItemInfo.EventClosePopupItemInfo -= OnEventClosePopupItemInfo;
        Destroy(m_trItemInfo.gameObject);
        m_csPopupItemInfo = null;
        m_trItemInfo = null;
    }

    #endregion PopupItemInfo
}
