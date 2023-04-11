using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2017-12-07)
//---------------------------------------------------------------------------------------------------

public enum EnResourceType
{
    Item = 0,
    OwnDia = 1,
    UnOwnDia = 2,
    Gold = 3,
    Honor = 4,
    SoulPowder = 7, 
}

public class CsPopupCalculator : MonoBehaviour
{
    EnResourceType m_enResourceType;

    CsItem m_csItem;
    CsHeroCreatureCard m_csHeroCreatureCard;

    Button m_buttonBuy;

    int m_nOwnCount = 0;
    int m_nBuyCount = 0;
    int m_nResourceItemId = 0;

    bool m_bOwned;

    Text m_textItemName;
    Text m_textBuyCountValue;
    Text m_textBuyGoldValue;

    public event Delegate<int> EventBuyItem;
    public event Delegate EventCloseCalculator;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);

        //종료버튼 연결
        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickClosePopup);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Transform trButtonList = transform.Find("ButtonList");

        //숫자버튼 세팅
        for (int i = 0; i < trButtonList.childCount; i++)
        {
            int nButtonIndex = i;
            Transform trButton = trButtonList.Find("Button" + i);

            Button button = trButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClickNum(nButtonIndex));
            button.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textButton = trButton.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButton);

            if (i < 9)
            {
                textButton.text = (i + 1).ToString();
            }
            else if (i == 9)
            {
                textButton.text = "0";
            }
            else if (i == 10)
            {
                textButton.text = "00";
            }
            else if (i == 11)
            {
                textButton.text = CsConfiguration.Instance.GetString("PUBLIC_DIAL_RESET");
            }
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    public void OnEventBuyItem(int nCount)
    {
        if (EventBuyItem != null)
        {
            EventBuyItem(nCount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OnEventCloseCalculator()
    {
        if (EventCloseCalculator != null)
        {
            EventCloseCalculator();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopup()
    {
        OnEventCloseCalculator();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickBuyItem()
    {
        switch (m_enResourceType)
        {
            case EnResourceType.Item:

                if (m_nBuyCount * m_nItemSaleValue <= CsGameData.Instance.MyHeroInfo.GetItemCount(m_nResourceItemId))
                {
                    //인벤토리 슬롯검사
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        //슬롯이 가득차있으면 같은아이템이 인벤토리에 있는지 검사
                        if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csItem.ItemId, m_bOwned) >= m_nBuyCount)
                        {
                            OnEventBuyItem(m_nBuyCount);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                        }
                    }
                    else
                    {
                        OnEventBuyItem(m_nBuyCount);
                    }
                }
                else
                {
                    // 아이템 부족
                }

                break;

            case EnResourceType.Gold:

                //골드검사
                if (m_nBuyCount * m_nItemSaleValue <= CsGameData.Instance.MyHeroInfo.Gold)
                {
                    //인벤토리 슬롯검사
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        //슬롯이 가득차있으면 같은아이템이 인벤토리에 있는지 검사
                        if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csItem.ItemId, m_bOwned) >= m_nBuyCount)
                        {
                            OnEventBuyItem(m_nBuyCount);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                        }
                    }
                    else
                    {
                        OnEventBuyItem(m_nBuyCount);
                    }
                }
                else
                {
                    //골드부족
                }

                break;

            case EnResourceType.Honor:

                if (m_nBuyCount * m_nItemSaleValue <= CsGameData.Instance.MyHeroInfo.HonorPoint)
                {
                    //인벤토리 슬롯검사
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        //슬롯이 가득차있으면 같은아이템이 인벤토리에 있는지 검사
                        if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csItem.ItemId, m_bOwned) >= m_nBuyCount)
                        {
                            OnEventBuyItem(m_nBuyCount);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                        }
                    }
                    else
                    {
                        OnEventBuyItem(m_nBuyCount);
                    }
                }
                else
                {
                    //명예점수 부족
                }

                break;
            case EnResourceType.SoulPowder:

                if (m_csHeroCreatureCard.Count >= m_nBuyCount)
                {
                    OnEventBuyItem(m_nBuyCount);
                }
                else
                {
                    //카드 수량 부족
                }

                break;

            case EnResourceType.UnOwnDia:

                if (m_nBuyCount * m_nItemSaleValue <= CsGameData.Instance.MyHeroInfo.UnOwnDia)
                {
                    //인벤토리 슬롯검사
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        //슬롯이 가득차있으면 같은아이템이 인벤토리에 있는지 검사
                        if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csItem.ItemId, m_bOwned) >= m_nBuyCount)
                        {
                            OnEventBuyItem(m_nBuyCount);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                        }
                    }
                    else
                    {
                        OnEventBuyItem(m_nBuyCount);
                    }
                }
                else
                {

                }

                break;

            case EnResourceType.OwnDia:

                if (m_nBuyCount * m_nItemSaleValue <= CsGameData.Instance.MyHeroInfo.Dia)
                {
                    //인벤토리 슬롯검사
                    if (CsGameData.Instance.MyHeroInfo.InventorySlotList.Count >= CsGameData.Instance.MyHeroInfo.InventorySlotCount)
                    {
                        //슬롯이 가득차있으면 같은아이템이 인벤토리에 있는지 검사
                        if (CsGameData.Instance.MyHeroInfo.GetRemainingItemCount(m_csItem.ItemId, m_bOwned) >= m_nBuyCount)
                        {
                            OnEventBuyItem(m_nBuyCount);
                        }
                        else
                        {
                            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A02_TXT_02004"));
                        }
                    }
                    else
                    {
                        OnEventBuyItem(m_nBuyCount);
                    }
                }
                else
                {

                }

                break;
        }

        OnClickClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNum(int nButtonIndex)
    {
        if (m_enResourceType != EnResourceType.SoulPowder)
        {
            switch (nButtonIndex)
            {
                case 9:
                    // 0
                    string strTemp = m_nBuyCount.ToString() + "0";

                    if (m_csItem.ItemType.MaxCountPerInventorySlot < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_csItem.ItemType.MaxCountPerInventorySlot;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;

                case 10:
                    // 00
                    strTemp = m_nBuyCount.ToString() + "00";

                    if (m_csItem.ItemType.MaxCountPerInventorySlot < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_csItem.ItemType.MaxCountPerInventorySlot;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;

                case 11:
                    m_nBuyCount = 0;
                    break;

                default:
                    int nTemp = nButtonIndex + 1;
                    strTemp = m_nBuyCount.ToString() + nTemp.ToString();

                    if (m_csItem.ItemType.MaxCountPerInventorySlot < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_csItem.ItemType.MaxCountPerInventorySlot;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;
            }
        }
        else
        {
            switch (nButtonIndex)
            {
                case 9:
                    // 0
                    string strTemp = m_nBuyCount.ToString() + "0";

                    if (m_nOwnCount < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_nOwnCount;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;

                case 10:
                    // 00
                    strTemp = m_nBuyCount.ToString() + "00";

                    if (m_nOwnCount < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_nOwnCount;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;

                case 11:
                    //초기화
                    m_nBuyCount = 0;
                    break;

                default:
                    int nTemp = nButtonIndex + 1;
                    strTemp = m_nBuyCount.ToString() + nTemp.ToString();

                    if (m_nOwnCount < int.Parse(strTemp))
                    {
                        m_nBuyCount = m_nOwnCount;
                    }
                    else
                    {
                        m_nBuyCount = int.Parse(strTemp);
                    }
                    break;
            }
        }

        ChangeCountText();
    }

    #endregion EventHandler

    Color32 m_colorRed = new Color32(229, 115, 115, 255);
    Color32 m_colorWhite = new Color32(222, 222, 222, 255);

    //---------------------------------------------------------------------------------------------------
    void ChangeCountText()
    {
        if (m_nBuyCount == 0)
        {
            CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
            m_textBuyCountValue.color = m_colorWhite;
            m_textBuyCountValue.text = "0";
            m_textBuyGoldValue.color = m_colorWhite;
            m_textBuyGoldValue.text = "0";
        }
        else
        {
            m_textBuyCountValue.text = m_nBuyCount.ToString();

            int nResourceValue = m_nBuyCount * m_nItemSaleValue;

            switch (m_enResourceType)
            {
                case EnResourceType.Item:
                    if (nResourceValue > CsGameData.Instance.MyHeroInfo.GetItemCount(m_nResourceItemId))
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorRed;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorWhite;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorWhite;
                    }

                    m_textBuyGoldValue.text = nResourceValue.ToString("#,##0");
                    break;

                case EnResourceType.OwnDia:
                    if (nResourceValue > CsGameData.Instance.MyHeroInfo.Dia)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorRed;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorWhite;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorWhite;
                    }

                    m_textBuyGoldValue.text = nResourceValue.ToString("#,##0");
                    break;

                case EnResourceType.UnOwnDia:
                    if (nResourceValue > CsGameData.Instance.MyHeroInfo.UnOwnDia)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorRed;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = CsUIData.Instance.ColorWhite;
                        m_textBuyGoldValue.color = CsUIData.Instance.ColorWhite;
                    }

                    m_textBuyGoldValue.text = nResourceValue.ToString("#,##0");
                    break;

                case EnResourceType.Gold:

                    if (nResourceValue > CsGameData.Instance.MyHeroInfo.Gold)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = m_colorRed;
                        m_textBuyGoldValue.color = m_colorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = m_colorWhite;
                        m_textBuyGoldValue.color = m_colorWhite;
                    }

                    m_textBuyGoldValue.text = nResourceValue.ToString("#,##0");
                    break;

                case EnResourceType.Honor:

                    if (nResourceValue > CsGameData.Instance.MyHeroInfo.HonorPoint)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = m_colorRed;
                        m_textBuyGoldValue.color = m_colorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = m_colorWhite;
                        m_textBuyGoldValue.color = m_colorWhite;
                    }

                    m_textBuyGoldValue.text = nResourceValue.ToString("#,##0");
                    break;

                case EnResourceType.SoulPowder:
                    if (m_nBuyCount > m_nOwnCount)
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);
                        m_textBuyCountValue.color = m_colorRed;
                        m_textBuyGoldValue.color = m_colorRed;
                    }
                    else
                    {
                        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, true);
                        m_textBuyCountValue.color = m_colorWhite;
                        m_textBuyGoldValue.color = m_colorWhite;
                    }

                    m_textBuyGoldValue.text = (m_csHeroCreatureCard.CreatureCard.CreatureCardGrade.DisassembleSoulPowder * m_nBuyCount).ToString("#,###");
                    break;
            }
        }
    }

    int m_nItemSaleValue = 0;

    public void DisplayCard(CsHeroCreatureCard csHeroCreatureCard, EnResourceType enResourceType)
    {
        m_csHeroCreatureCard = csHeroCreatureCard;
        m_enResourceType = enResourceType;

        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("A25_NAME_00002");

        //보유 수량
        Text textCount0 = transform.Find("TextBuyCount0").GetComponent<Text>();
        textCount0.text = CsConfiguration.Instance.GetString("A25_TXT_00005");
        CsUIData.Instance.SetFont(textCount0);
        textCount0.gameObject.SetActive(true);

        Text textCountValue0 = transform.Find("TextBuyCountValue0").GetComponent<Text>();
        textCountValue0.text = csHeroCreatureCard.Count.ToString("#,##0");
        CsUIData.Instance.SetFont(textCountValue0);
        textCountValue0.gameObject.SetActive(true);

        m_nOwnCount = csHeroCreatureCard.Count;

        //분해 카드 이름
        m_textItemName = transform.Find("TextItemName").GetComponent<Text>();
        m_textItemName.text = csHeroCreatureCard.CreatureCard.Name;
        CsUIData.Instance.SetFont(m_textItemName);

        //분해 수량
        Text textCount1 = transform.Find("TextBuyCount1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount1);
        textCount1.text = CsConfiguration.Instance.GetString("A25_TXT_00006");

        m_textBuyCountValue = transform.Find("TextBuyCountValue1").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuyCountValue);
        m_textBuyCountValue.text = "0";

        //분해획득
        Text textResource = transform.Find("TextBuyGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(textResource);
        textResource.text = CsConfiguration.Instance.GetString("A25_TXT_00007");

        m_textBuyGoldValue = transform.Find("TextBuyGoldValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuyGoldValue);
        m_textBuyGoldValue.text = "0";

        m_buttonBuy = transform.Find("ButtonBuy").GetComponent<Button>();
        m_buttonBuy.onClick.RemoveAllListeners();
        m_buttonBuy.onClick.AddListener(OnClickBuyItem);
        m_buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text m_textBuy = m_buttonBuy.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuy);
        m_textBuy.text = CsConfiguration.Instance.GetString("A25_BTN_00005");

        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);

        Transform trItemSlot = transform.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroCreatureCard.CreatureCard, false);

        ChangeResourceImage();
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayItem(CsItem csitem, bool bOwned, int nItemSaleValue, EnResourceType enResourceType, int nResourceItemId = 0)
    {
        Text textPopupName = transform.Find("TextPopupName").GetComponent<Text>();
        textPopupName.text = CsConfiguration.Instance.GetString("A14_NAME_00002");

        //구매수량
        Text textCount = transform.Find("TextBuyCount1").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount);
        textCount.text = CsConfiguration.Instance.GetString("A14_TXT_00005");

        //구매금액
        Text textResource = transform.Find("TextBuyGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(textResource);
        textResource.text = CsConfiguration.Instance.GetString("A14_TXT_00006");

        m_buttonBuy = transform.Find("ButtonBuy").GetComponent<Button>();
        m_buttonBuy.onClick.RemoveAllListeners();
        m_buttonBuy.onClick.AddListener(OnClickBuyItem);
        m_buttonBuy.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text m_textBuy = m_buttonBuy.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuy);
        m_textBuy.text = CsConfiguration.Instance.GetString("A14_BTN_00005");

        CsUIData.Instance.DisplayButtonInteractable(m_buttonBuy, false);

        //텍스트 - 아이템이름, 현재수량, 현재가격

        m_textItemName = transform.Find("TextItemName").GetComponent<Text>();
        m_textItemName.text = csitem.Name;
        CsUIData.Instance.SetFont(m_textItemName);

        m_textBuyCountValue = transform.Find("TextBuyCountValue1").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuyCountValue);
        m_textBuyCountValue.text = "0";

        m_textBuyGoldValue = transform.Find("TextBuyGoldValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textBuyGoldValue);
        m_textBuyGoldValue.text = "0";

        //아이템과 관련된 재화 세팅
        m_csItem = csitem;
        m_nItemSaleValue = nItemSaleValue;
        m_bOwned = bOwned;
        m_nResourceItemId = nResourceItemId;

        Transform trItemSlot = transform.Find("ItemSlot");
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csitem, bOwned, 0);

        m_enResourceType = enResourceType;
        ChangeResourceImage();

        m_nBuyCount = 1;
        ChangeCountText();
    }

    //---------------------------------------------------------------------------------------------------
    void ChangeResourceImage()
    {
        Transform trImageFrame = transform.Find("ImageResourceFrame");

        Image imageResourceTop = trImageFrame.Find("ImageIcon").GetComponent<Image>();
        imageResourceTop.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods0" + (int)m_enResourceType);

        Text textResource = trImageFrame.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textResource);

        switch (m_enResourceType)
        {
            case EnResourceType.Item:
                textResource.text = CsGameData.Instance.MyHeroInfo.GetItemCount(m_nResourceItemId).ToString("#,##0");
                break;

            case EnResourceType.OwnDia:
                Transform trImageOwnDiaFrame = transform.Find("ImageResourceFrameOwnDia");

                Image imageResourceOwnDia = trImageOwnDiaFrame.Find("ImageIcon").GetComponent<Image>();
                imageResourceOwnDia.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods0" + (int)EnResourceType.OwnDia);

                Text textResourceOwnDia = trImageOwnDiaFrame.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textResourceOwnDia);
                textResourceOwnDia.text = CsGameData.Instance.MyHeroInfo.OwnDia.ToString("#,##0");

                trImageOwnDiaFrame.gameObject.SetActive(true);

                imageResourceTop.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods0" + (int)EnResourceType.UnOwnDia);
                textResource.text = CsGameData.Instance.MyHeroInfo.UnOwnDia.ToString("#,##0");
                break;

            case EnResourceType.UnOwnDia:
                textResource.text = CsGameData.Instance.MyHeroInfo.UnOwnDia.ToString("#,##0");
                break;

            case EnResourceType.Gold:
                textResource.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");
                break;

            case EnResourceType.Honor:
                textResource.text = CsGameData.Instance.MyHeroInfo.HonorPoint.ToString("#,##0");
                break;

            case EnResourceType.SoulPowder:
                textResource.text = CsGameData.Instance.MyHeroInfo.SoulPowder.ToString("#,##0");
                break;
        }

        Image imageResource = transform.Find("ImageGold").GetComponent<Image>();

        if (m_enResourceType == EnResourceType.Item)
        {
            CsItem csItem = CsGameData.Instance.GetItem(m_nResourceItemId);

            if (csItem == null)
            {
                return;
            }
            else
            {
                imageResourceTop.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);
                imageResource.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csItem.Image);
            }
        }
        else
        {
            imageResource.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods0" + (int)m_enResourceType);
        }
    }
}
