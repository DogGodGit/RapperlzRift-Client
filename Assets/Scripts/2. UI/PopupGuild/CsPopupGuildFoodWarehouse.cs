using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupGuildFoodWarehouse : MonoBehaviour
{
    [SerializeField] GameObject m_goGuildFoodWarehouseItem;

    Transform m_trImageBackground;

    Image m_imageGuildFoodWarehouse;

    int m_nFoodWarehouseLevel;
    int m_nFoodWarehouseExp;

    Text m_textGuildFoodWarehouseName;
    Text m_textDailyCount;
    Text m_textNoMaterial;

    Slider m_sliderExpGuage;

    Button m_buttonCollect;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGuildManager.Instance.EventGuildFoodWarehouseInfo += OnEventGuildFoodWarehouseInfo;
        CsGuildManager.Instance.EventGuildFoodWarehouseStock += OnEventGuildFoodWarehouseStock;
        CsGuildManager.Instance.EventGuildFoodWarehouseCollect += OnEventGuildFoodWarehouseCollect;

        InitiallizeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGuildManager.Instance.EventGuildFoodWarehouseInfo -= OnEventGuildFoodWarehouseInfo;
        CsGuildManager.Instance.EventGuildFoodWarehouseStock -= OnEventGuildFoodWarehouseStock;
        CsGuildManager.Instance.EventGuildFoodWarehouseCollect -= OnEventGuildFoodWarehouseCollect;
    }

    //---------------------------------------------------------------------------------------------------
    void InitiallizeUI()
    {
        Transform trImageBackground = transform.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A65_TXT_00007");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClose);

        m_textGuildFoodWarehouseName = trImageBackground.Find("TextGuildFoodWarehouseName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textGuildFoodWarehouseName);

        m_sliderExpGuage = trImageBackground.Find("SliderExpGuage").GetComponent<Slider>();

        // 소환 버튼
        m_buttonCollect = trImageBackground.Find("ButtonCollect").GetComponent<Button>();
        m_buttonCollect.onClick.RemoveAllListeners();
        m_buttonCollect.onClick.AddListener(OnClickGuildFoodWarehouseCollect);

        Text textButtonCollect = m_buttonCollect.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCollect);
        textButtonCollect.text = CsConfiguration.Instance.GetString("A65_TXT_00002");

        // 남은 횟수
        m_textDailyCount = trImageBackground.Find("TextDailyCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDailyCount);

        m_trImageBackground = trImageBackground.Find("ImageBackground");

        m_textNoMaterial = trImageBackground.Find("TextNoMaterial").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNoMaterial);
        m_textNoMaterial.text = CsConfiguration.Instance.GetString("A65_TXT_00006");

        m_imageGuildFoodWarehouse = trImageBackground.Find("ImageGuildFoodWarehouse").GetComponent<Image>();

        CsGuildManager.Instance.SendGuildFoodWarehouseInfo();
    }

    #region Event

    // 현재 길드 군량 창고 정보
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWarehouseInfo(int nFoodWarehouseLevel, int nFoodWarehouseExp)
    {
        // 현재 레벨 경험치
        m_nFoodWarehouseLevel = nFoodWarehouseLevel;
        m_nFoodWarehouseExp = nFoodWarehouseExp;

        CsGuildFoodWarehouseLevel csGuildFoodWarehouseLevel = CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList.Find(a => a.Level == nFoodWarehouseLevel);

        if (csGuildFoodWarehouseLevel == null)
        {
            return;
        }
        else
        {
            UpdateTextGuildFoodWarehouseName();
            UpdateImageGuildFoodWarehouse();
            UpdateSliderGuildFoodWarehouseExpGuage();
            UpdateTextDailyCount();
            UpdateButtonCollect();
            UpdateGuildFoodWarehouseItemList();
        }
    }

    // 길드 군량 창고 양육
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWarehouseStock(bool bLevelUp, long lAcquiredExp, int nAddedFoodWarehouseExp, int nFoodWarehouseLevel, int nFoodWarehouseExp)
    {
        m_nFoodWarehouseLevel = nFoodWarehouseLevel;
        m_nFoodWarehouseExp = nFoodWarehouseExp;

        UpdateTextGuildFoodWarehouseName();
        UpdateImageGuildFoodWarehouse();
        UpdateSliderGuildFoodWarehouseExpGuage();
        UpdateTextDailyCount();
        UpdateButtonCollect();
        UpdateGuildFoodWarehouseItemList();
    }

    // 길드 군량 창고 소환
    //---------------------------------------------------------------------------------------------------
    void OnEventGuildFoodWarehouseCollect()
    {
        CsGuildManager.Instance.SendGuildFoodWarehouseInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildFoodWarehouseStock(int nItemId)
    {
        // 길드 신수 양육
        CsGuildManager.Instance.SendGuildFoodWarehouseStock(nItemId);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGuildFoodWarehouseCollect()
    {
        // 길드 신수 소환
        CsGuildManager.Instance.SendGuildFoodWarehouseCollect();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void UpdateTextGuildFoodWarehouseName()
    {
        // 길드 군량 창고 텍스트 갱신
        m_textGuildFoodWarehouseName.text = string.Format(CsConfiguration.Instance.GetString("A65_TXT_00001"), m_nFoodWarehouseLevel, CsGuildManager.Instance.GuildName);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageGuildFoodWarehouse()
    {
        m_imageGuildFoodWarehouse.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupGuild/frm_guild_warehouse_" + m_nFoodWarehouseLevel);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSliderGuildFoodWarehouseExpGuage()
    {
        int nMaxValue = 0;
        CsGuildFoodWarehouseLevel csGuildFoodWarehouseLevel = null;

        for (int i = 0; i < m_nFoodWarehouseLevel; i++)
        {
            if (CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList.Count <= i)
            {
                continue;
            }
            else
            {
                csGuildFoodWarehouseLevel = CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList[i];
                nMaxValue += csGuildFoodWarehouseLevel.NextLevelUpRequiredExp;
            }
        }

        int nValue = 0;

        if (m_nFoodWarehouseLevel - 1 < CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList.Count)
        {
            nValue = m_nFoodWarehouseExp + (nMaxValue - CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList[m_nFoodWarehouseLevel - 1].NextLevelUpRequiredExp);
        }

        m_sliderExpGuage.maxValue = nMaxValue;
        m_sliderExpGuage.value = nValue;

        Text textExpGuage = m_sliderExpGuage.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textExpGuage);
        textExpGuage.text = string.Format(CsConfiguration.Instance.GetString("A65_TXT_00005"), nValue, nMaxValue);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateTextDailyCount()
    {
        // 남은 횟수 / 최대 횟수
        m_textDailyCount.text = string.Format(CsConfiguration.Instance.GetString("A65_TXT_00003"), CsGameData.Instance.GuildFoodWarehouse.LimitCount - CsGuildManager.Instance.DailyGuildFoodWarehouseStockCount, CsGameData.Instance.GuildFoodWarehouse.LimitCount);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonCollect()
    {
        int nCount = CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList.Count;
        CsGuildFoodWarehouseLevel csGuildFoodWarehouseLevel = CsGameData.Instance.GuildFoodWarehouse.GuildFoodWarehouseLevelList[nCount - 1];

        if (m_nFoodWarehouseExp >= csGuildFoodWarehouseLevel.NextLevelUpRequiredExp)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonCollect, true);
        }
        else
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonCollect, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGuildFoodWarehouseItemList()
    {
        // 군량 아이템 타입
        List<CsInventoryObjectItem> listGuildFoodWarehouseItem = new List<CsInventoryObjectItem>();

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.InventorySlotList.Count; i++)
        {
            CsInventorySlot csInventorySlot = CsGameData.Instance.MyHeroInfo.InventorySlotList[i];

            if (csInventorySlot.EnType == EnInventoryObjectType.Item && csInventorySlot.InventoryObjectItem.Item.ItemType.ItemType == CsGameData.Instance.GuildFoodWarehouse.LevelUpRequiredItemType)
            {
                listGuildFoodWarehouseItem.Add(csInventorySlot.InventoryObjectItem);
            }
        }

        listGuildFoodWarehouseItem.Sort(CompareTo);

        for (int i = 0; i < m_trImageBackground.childCount; i++)
        {
            m_trImageBackground.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < listGuildFoodWarehouseItem.Count; i++)
        {
            Transform trGuildFoodWarehouseItem = m_trImageBackground.Find("GuildFoodWarehouseItem" + i);

            if (trGuildFoodWarehouseItem == null)
            {
                trGuildFoodWarehouseItem = Instantiate(m_goGuildFoodWarehouseItem, m_trImageBackground).transform;
            }

            CsItem csItem = listGuildFoodWarehouseItem[i].Item;

            Transform trItemSlot = trGuildFoodWarehouseItem.Find("ItemSlot");
            CsUIData.Instance.DisplayItemSlot(trItemSlot, listGuildFoodWarehouseItem[i]);

            Text textName = trGuildFoodWarehouseItem.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textName);
            textName.text = csItem.Name;

            Text textValue = trGuildFoodWarehouseItem.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textValue);
            textValue.text = string.Format(CsConfiguration.Instance.GetString("A65_TXT_00008"), csItem.Value1);

            Button buttonStock = trGuildFoodWarehouseItem.Find("ButtonStock").GetComponent<Button>();
            buttonStock.onClick.RemoveAllListeners();
            buttonStock.onClick.AddListener(() => OnClickGuildFoodWarehouseStock(csItem.ItemId));

            if (CsGameData.Instance.GuildFoodWarehouse.LimitCount - CsGuildManager.Instance.DailyGuildFoodWarehouseStockCount <= 0)
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonStock, false);
            }
            else
            {
                CsUIData.Instance.DisplayButtonInteractable(buttonStock, true);
            }

            Text textButtonStock = buttonStock.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonStock);
            textButtonStock.text = CsConfiguration.Instance.GetString("A65_TXT_00004");

            trGuildFoodWarehouseItem.gameObject.SetActive(true);
        }

        if (listGuildFoodWarehouseItem.Count == 0)
        {
            m_textNoMaterial.gameObject.SetActive(true);
        }
        else
        {
            m_textNoMaterial.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    int CompareTo(CsInventoryObjectItem x, CsInventoryObjectItem y)
    {
        if (x.Item.ItemId > y.Item.ItemId) return 1;
        else if (x.Item.ItemId < y.Item.ItemId) return -1;
        return 0;
    }
}