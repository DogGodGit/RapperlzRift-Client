using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupLuckyShop : CsPopupSub
{
    enum EnLuckyShopType
    {
        Item = 0, 
        Card = 1, 
    }

    enum EnLuckyShopPickType
    {
        Pick1Time = 0, 
        Pick5Time = 1, 
    }

    GameObject m_goPopupLuckyShopResult;
    GameObject m_goResultItem;
    GameObject m_goCreatureCardItem;

    Transform m_trPopupLuckyShopResult;

    Button m_buttonLuckShopItemPick1Time;
    Button m_buttonLuckShopCardPick1Time;
    Button m_buttonLuckShopItemPick5Time;
    Button m_buttonLuckShopCardPick5Time;

    bool m_bOpenResult = false;
    bool m_bFreeLuckyShopItem = false;
    bool m_bFreeLuckyShopCard = false;

    List<CsHeroCreatureCard> m_listCsHeroCreatureCard = new List<CsHeroCreatureCard>();
    List<ClientCommon.PDItemLuckyShopPickResult> m_listPDItemLuckyShopPickResult = new List<ClientCommon.PDItemLuckyShopPickResult>();

    float m_flTime = 0f;

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShop1TimePick += OnEventCreatureCardLuckyShop1TimePick;
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShop5TimePick += OnEventCreatureCardLuckyShop5TimePick;
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShopFreePick += OnEventCreatureCardLuckyShopFreePick;
        CsLuckyShopManager.Instance.EventItemLuckyShop1TimePick += OnEventItemLuckyShop1TimePick;
        CsLuckyShopManager.Instance.EventItemLuckyShop5TimePick += OnEventItemLuckyShop5TimePick;
        CsLuckyShopManager.Instance.EventItemLuckyShopFreePick += OnEventItemLuckyShopFreePick;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShop1TimePick -= OnEventCreatureCardLuckyShop1TimePick;
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShop5TimePick -= OnEventCreatureCardLuckyShop5TimePick;
        CsLuckyShopManager.Instance.EventCreatureCardLuckyShopFreePick -= OnEventCreatureCardLuckyShopFreePick;
        CsLuckyShopManager.Instance.EventItemLuckyShop1TimePick -= OnEventItemLuckyShop1TimePick;
        CsLuckyShopManager.Instance.EventItemLuckyShop5TimePick -= OnEventItemLuckyShop5TimePick;
        CsLuckyShopManager.Instance.EventItemLuckyShopFreePick -= OnEventItemLuckyShopFreePick;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {
        if (m_flTime + flTime < Time.time)
        {
            if (CsLuckyShopManager.Instance.CreatureCardLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup <= 0f && !m_bFreeLuckyShopCard)
            {
                m_bFreeLuckyShopCard = true;
                UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);
            }
            else
            {
                UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);
            }

            if (CsLuckyShopManager.Instance.ItemLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup <= 0f && !m_bFreeLuckyShopItem)
            {
                m_bFreeLuckyShopItem = true;
                UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
            }
            else
            {
                UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
            }

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShop1TimePick(CsHeroCreatureCard csHeroCreatureCard)
    {
        m_listCsHeroCreatureCard.Clear();
        m_listCsHeroCreatureCard.Add(csHeroCreatureCard);

        OpenPopupLuckyShopResult(EnLuckyShopType.Card, EnLuckyShopPickType.Pick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);
        UpdateButtonPick5Time(m_buttonLuckShopCardPick5Time, EnLuckyShopType.Card);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShop5TimePick(List<CsHeroCreatureCard> listCsHeroCreatureCard)
    {
        m_listCsHeroCreatureCard.Clear();
        m_listCsHeroCreatureCard.AddRange(listCsHeroCreatureCard);

        OpenPopupLuckyShopResult(EnLuckyShopType.Card, EnLuckyShopPickType.Pick5Time);
        UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);
        UpdateButtonPick5Time(m_buttonLuckShopCardPick5Time, EnLuckyShopType.Card);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCreatureCardLuckyShopFreePick(CsHeroCreatureCard csHeroCreatureCard)
    {
        m_listCsHeroCreatureCard.Clear();
        m_listCsHeroCreatureCard.Add(csHeroCreatureCard);

        OpenPopupLuckyShopResult(EnLuckyShopType.Card, EnLuckyShopPickType.Pick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);
        UpdateButtonPick5Time(m_buttonLuckShopCardPick5Time, EnLuckyShopType.Card);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemLuckyShop1TimePick(ClientCommon.PDItemLuckyShopPickResult PDItemLuckyShopPickResult)
    {
        m_listPDItemLuckyShopPickResult.Clear();
        m_listPDItemLuckyShopPickResult.Add(PDItemLuckyShopPickResult);

        OpenPopupLuckyShopResult(EnLuckyShopType.Item, EnLuckyShopPickType.Pick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
        UpdateButtonPick5Time(m_buttonLuckShopItemPick5Time, EnLuckyShopType.Item);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemLuckyShop5TimePick(ClientCommon.PDItemLuckyShopPickResult[] arrPDItemLuckyShopPickResult)
    {
        m_listPDItemLuckyShopPickResult.Clear();
        m_listPDItemLuckyShopPickResult.AddRange(arrPDItemLuckyShopPickResult);

        OpenPopupLuckyShopResult(EnLuckyShopType.Item, EnLuckyShopPickType.Pick5Time);
        UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
        UpdateButtonPick5Time(m_buttonLuckShopItemPick5Time, EnLuckyShopType.Item);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventItemLuckyShopFreePick(ClientCommon.PDItemLuckyShopPickResult PDItemLuckyShopPickResult)
    {
        m_listPDItemLuckyShopPickResult.Clear();
        m_listPDItemLuckyShopPickResult.Add(PDItemLuckyShopPickResult);

        OpenPopupLuckyShopResult(EnLuckyShopType.Item, EnLuckyShopPickType.Pick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
        UpdateButtonPick5Time(m_buttonLuckShopItemPick5Time, EnLuckyShopType.Item);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLuckyShopItemPick1Time()
    {
        if (m_bOpenResult == false)
        {
            if (m_bFreeLuckyShopItem)
            {
                CsLuckyShopManager.Instance.SendItemLuckyShopFreePick();
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount <= CsLuckyShopManager.Instance.ItemLuckyShopPick1TimeCount)
                {
                    CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00202"));
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.Dia < CsGameData.Instance.ItemLuckyShop.Pick1TimeDia)
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
                    }
                    else
                    {
                        CsLuckyShopManager.Instance.SendItemLuckyShop1TimePick();
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLuckyShopItemPick5Time()
    {
        if (m_bOpenResult == false)
        {
            if (CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount <= CsLuckyShopManager.Instance.ItemLuckyShopPick5TimeCount)
            {
                CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A139_ERROR_00302"));
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.Dia < CsGameData.Instance.ItemLuckyShop.Pick5TimeDia)
                {
                    CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
                }
                else
                {
                    CsLuckyShopManager.Instance.SendItemLuckyShop5TimePick();
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLuckyShopCardPick1Time()
    {
        if (m_bOpenResult == false)
        {
            if (m_bFreeLuckyShopCard)
            {
                CsLuckyShopManager.Instance.SendCreatureCardLuckyShopFreePick();
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount <= CsLuckyShopManager.Instance.CreatureCardLuckyShopPick1TimeCount)
                {
                    CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00202"));
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.Dia < CsGameData.Instance.CreatureCardLuckyShop.Pick1TimeDia)
                    {
                        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
                    }
                    else
                    {
                        CsLuckyShopManager.Instance.SendCreatureCardLuckyShop1TimePick();
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLuckyShopCardPick5Time()
    {
        if (CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount <= CsLuckyShopManager.Instance.CreatureCardLuckyShopPick5TimeCount)
        {
            CsGameEventUIToUI.Instance.OnEventAlert(CsConfiguration.Instance.GetString("A140_ERROR_00302"));
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.Dia < CsGameData.Instance.CreatureCardLuckyShop.Pick5TimeDia)
            {
                CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("PUBLIC_DIAERROR"));
            }
            else
            {
                CsLuckyShopManager.Instance.SendCreatureCardLuckyShop5TimePick();
            }
        }
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trLuckyShopItem = transform.Find("ImageBackground/LuckyShopItem");

        Transform trImageLuckyShopItem = trLuckyShopItem.Find("ImageLuckyShopItem");

        Text textLuckyShopItem = trImageLuckyShopItem.Find("ImageGlow/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLuckyShopItem);
        textLuckyShopItem.text = CsGameData.Instance.ItemLuckyShop.Name;

        Text textLuckyShopItemDesc = trImageLuckyShopItem.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLuckyShopItemDesc);
        textLuckyShopItemDesc.text = CsConfiguration.Instance.GetString("A114_TXT_00003");

        m_buttonLuckShopItemPick1Time = trLuckyShopItem.Find("ButtonPick1Time").GetComponent<Button>();
        m_buttonLuckShopItemPick1Time.onClick.RemoveAllListeners();
        m_buttonLuckShopItemPick1Time.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonLuckShopItemPick1Time.onClick.AddListener(OnClickLuckyShopItemPick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopItemPick1Time, EnLuckyShopType.Item);
        
        m_buttonLuckShopItemPick5Time = trLuckyShopItem.Find("ButtonPick5Time").GetComponent<Button>();
        m_buttonLuckShopItemPick5Time.onClick.RemoveAllListeners();
        m_buttonLuckShopItemPick5Time.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonLuckShopItemPick5Time.onClick.AddListener(OnClickLuckyShopItemPick5Time);
        UpdateButtonPick5Time(m_buttonLuckShopItemPick5Time, EnLuckyShopType.Item);
        
        Transform trLuckyShopCard = transform.Find("ImageBackground/LuckyShopCard");

        Transform trImageLuckyShopCard = trLuckyShopCard.Find("ImageLuckyShopCard");

        Text textLuckyShopCard = trImageLuckyShopCard.Find("ImageGlow/Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLuckyShopCard);
        textLuckyShopCard.text = CsGameData.Instance.CreatureCardLuckyShop.Name;

        Text textLuckyShopCardDesc = trImageLuckyShopCard.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLuckyShopCardDesc);
        textLuckyShopCardDesc.text = CsConfiguration.Instance.GetString("A114_TXT_00004");

        m_buttonLuckShopCardPick1Time = trLuckyShopCard.Find("ButtonPick1Time").GetComponent<Button>();
        m_buttonLuckShopCardPick1Time.onClick.RemoveAllListeners();
        m_buttonLuckShopCardPick1Time.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonLuckShopCardPick1Time.onClick.AddListener(OnClickLuckyShopCardPick1Time);
        UpdateButtonPick1Time(m_buttonLuckShopCardPick1Time, EnLuckyShopType.Card);

        m_buttonLuckShopCardPick5Time = trLuckyShopCard.Find("ButtonPick5Time").GetComponent<Button>();
        m_buttonLuckShopCardPick5Time.onClick.RemoveAllListeners();
        m_buttonLuckShopCardPick5Time.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonLuckShopCardPick5Time.onClick.AddListener(OnClickLuckyShopCardPick5Time);

        UpdateButtonPick5Time(m_buttonLuckShopCardPick5Time, EnLuckyShopType.Card);
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPick1Time(Button buttonPick1Time, EnLuckyShopType enLuckyShopType)
    {
        if (buttonPick1Time == null)
        {
            return;
        }
        else
        {
            Text textFree = buttonPick1Time.transform.Find("TextFree").GetComponent<Text>();
            CsUIData.Instance.SetFont(textFree);
            textFree.text = CsConfiguration.Instance.GetString("A114_BTN_00001");

            Transform trPaidPick1Time = buttonPick1Time.transform.Find("PaidPick1Time");

            Text textDiaValue = trPaidPick1Time.Find("TextDia").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDiaValue);

            Text textCount = trPaidPick1Time.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = CsConfiguration.Instance.GetString("A114_BTN_00003");

            Text textAllow = buttonPick1Time.transform.Find("TextAllow").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAllow);

            Transform trImageFree = buttonPick1Time.transform.Find("ImageFree");

            Transform trImageClockIcon = trImageFree.Find("Image");

            Text textFreeCount = trImageFree.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textFreeCount);

            System.TimeSpan tsFreePickRemainingTime;
            int nRemainingLuckyShopPick1TimeCount = 0;

            if (enLuckyShopType == EnLuckyShopType.Item)
            {
                if (0.0f < CsLuckyShopManager.Instance.ItemLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup)
                {
                    // 제한 시간이 없다
                    m_bFreeLuckyShopItem = false;

                    if (CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount < CsGameData.Instance.ItemLuckyShop.FreePickCount)
                    {
                        trImageClockIcon.gameObject.SetActive(true);

                        tsFreePickRemainingTime = System.TimeSpan.FromSeconds(CsLuckyShopManager.Instance.ItemLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup);
                        textFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A114_TXT_01002"), tsFreePickRemainingTime.Hours.ToString("00"), tsFreePickRemainingTime.Minutes.ToString("00"), tsFreePickRemainingTime.Seconds.ToString("00"));
                    }
                    else
                    {
                        //
                        trImageClockIcon.gameObject.SetActive(false);
                        textFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A114_TXT_01003"), CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount, CsGameData.Instance.ItemLuckyShop.FreePickCount);
                    }

                    trImageFree.gameObject.SetActive(true);

                    // 유료 뽑기로 전환
                    nRemainingLuckyShopPick1TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.ItemLuckyShopPick1TimeCount;

                    textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick1TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
                    textDiaValue.text = CsGameData.Instance.ItemLuckyShop.Pick1TimeDia.ToString("#,##0");

                    trPaidPick1Time.gameObject.SetActive(true);
                    textFree.gameObject.SetActive(false);
                    textAllow.gameObject.SetActive(true);
                }
                else
                {
                    // 제한 시간이 없다
                    if (CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount < CsGameData.Instance.ItemLuckyShop.FreePickCount)
                    {
                        m_bFreeLuckyShopItem = true;

                        trImageFree.gameObject.SetActive(false);

                        // 무료 뽑기 남음
                        trPaidPick1Time.gameObject.SetActive(false);
                        textFree.gameObject.SetActive(true);
                        textAllow.gameObject.SetActive(false);
                    }
                    else
                    {
                        m_bFreeLuckyShopItem = false;

                        //
                        trImageClockIcon.gameObject.SetActive(false);
                        textFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A114_TXT_01003"), CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount, CsGameData.Instance.ItemLuckyShop.FreePickCount);

                        trImageFree.gameObject.SetActive(true);

                        // 유료 뽑기로 전환
                        nRemainingLuckyShopPick1TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.ItemLuckyShopPick1TimeCount;

                        textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick1TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
                        textDiaValue.text = CsGameData.Instance.ItemLuckyShop.Pick1TimeDia.ToString("#,##0");

                        trPaidPick1Time.gameObject.SetActive(true);
                        textFree.gameObject.SetActive(false);
                        textAllow.gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (0.0f < CsLuckyShopManager.Instance.CreatureCardLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup)
                {
                    m_bFreeLuckyShopCard = false;

                    // 무료 뽑기 제한 시간이 있다
                    trImageClockIcon.gameObject.SetActive(true);

                    tsFreePickRemainingTime = System.TimeSpan.FromSeconds(CsLuckyShopManager.Instance.CreatureCardLuckyShopFreePickRemainingTime - Time.realtimeSinceStartup);
                    textFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A114_TXT_01002"), tsFreePickRemainingTime.Hours.ToString("00"), tsFreePickRemainingTime.Minutes.ToString("00"), tsFreePickRemainingTime.Seconds.ToString("00"));

                    trImageFree.gameObject.SetActive(true);

                    // 유료 뽑기로 전환
                    nRemainingLuckyShopPick1TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.CreatureCardLuckyShopPick1TimeCount;

                    textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick1TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
                    textDiaValue.text = CsGameData.Instance.CreatureCardLuckyShop.Pick1TimeDia.ToString("#,##0");

                    trPaidPick1Time.gameObject.SetActive(true);
                    textFree.gameObject.SetActive(false);
                    textAllow.gameObject.SetActive(true);
                }
                else
                {
                    // 제한 시간이 없다.
                    if (CsLuckyShopManager.Instance.CreatureCardLuckyShopFreePickCount < CsGameData.Instance.CreatureCardLuckyShop.FreePickCount)
                    {
                        m_bFreeLuckyShopCard = true;

                        trImageFree.gameObject.SetActive(false);

                        // 무료 뽑기 남음
                        trPaidPick1Time.gameObject.SetActive(false);
                        textFree.gameObject.SetActive(true);
                        textAllow.gameObject.SetActive(false);
                    }
                    else
                    {
                        m_bFreeLuckyShopCard = false;

                        //
                        trImageClockIcon.gameObject.SetActive(false);
                        textFreeCount.text = string.Format(CsConfiguration.Instance.GetString("A114_TXT_01003"), CsLuckyShopManager.Instance.ItemLuckyShopFreePickCount, CsGameData.Instance.ItemLuckyShop.FreePickCount);

                        trImageFree.gameObject.SetActive(true);

                        // 유료 뽑기로 전환
                        nRemainingLuckyShopPick1TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.CreatureCardLuckyShopPick1TimeCount;

                        textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick1TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
                        textDiaValue.text = CsGameData.Instance.CreatureCardLuckyShop.Pick1TimeDia.ToString("#,##0");

                        trPaidPick1Time.gameObject.SetActive(true);
                        textFree.gameObject.SetActive(false);
                        textAllow.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPick5Time(Button buttonPick5Time, EnLuckyShopType enLuckyShopType)
    {
        if (buttonPick5Time == null)
        {
            return;
        }
        else
        {
            Text textDiaValue = buttonPick5Time.transform.Find("TextDia").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDiaValue);

            Text textCount = buttonPick5Time.transform.Find("TextCount").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCount);
            textCount.text = CsConfiguration.Instance.GetString("A114_BTN_00002");

            Text textAllow = buttonPick5Time.transform.Find("TextAllow").GetComponent<Text>();
            CsUIData.Instance.SetFont(textAllow);

            int nRemainingLuckyShopPick5TimeCount = 0;

            if (enLuckyShopType == EnLuckyShopType.Item)
            {
                nRemainingLuckyShopPick5TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.ItemLuckyShopPick5TimeCount;
                
                textDiaValue.text = CsGameData.Instance.ItemLuckyShop.Pick5TimeDia.ToString("#,##0");
                textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick5TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
            }
            else
            {
                nRemainingLuckyShopPick5TimeCount = CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount - CsLuckyShopManager.Instance.CreatureCardLuckyShopPick5TimeCount;

                textDiaValue.text = CsGameData.Instance.CreatureCardLuckyShop.Pick5TimeDia.ToString("#,##0");
                textAllow.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nRemainingLuckyShopPick5TimeCount, CsGameData.Instance.MyHeroInfo.VipLevel.LuckyShopPickMaxCount);
            }
        }
    }

    #region Result
    
    //---------------------------------------------------------------------------------------------------
    void OpenPopupLuckyShopResult(EnLuckyShopType enLuckyShopType, EnLuckyShopPickType enLuckyShopPickType)
    {
        m_bOpenResult = true;

        if (m_trPopupLuckyShopResult == null)
        {
            if (m_goPopupLuckyShopResult == null)
            {
                StartCoroutine(LoadPopupLuckyShopResult(enLuckyShopType, enLuckyShopPickType));
            }
            else
            {
                CreatePopupLuckyShopResult(enLuckyShopType, enLuckyShopPickType);
            }
        }
        else
        {
            UpdatePopupLuckyShopResult(enLuckyShopType);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupLuckyShopResult(EnLuckyShopType enLuckyShopType, EnLuckyShopPickType enLuckyShopPickType)
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupLuckyShop/PopupLuckyShopResult");
        yield return resourceRequest;

        m_goPopupLuckyShopResult = (GameObject)resourceRequest.asset;
        CreatePopupLuckyShopResult(enLuckyShopType, enLuckyShopPickType);
    }

    //---------------------------------------------------------------------------------------------------
    void CreatePopupLuckyShopResult(EnLuckyShopType enLuckyShopType, EnLuckyShopPickType enLuckyShopPickType)
    {
        if (m_trPopupLuckyShopResult == null)
        {
            m_trPopupLuckyShopResult = Instantiate(m_goPopupLuckyShopResult, transform).transform;
            m_trPopupLuckyShopResult.name = "PopupLuckyShopResult";

            m_bOpenResult = false;

            Text textResult = m_trPopupLuckyShopResult.Find("ImageResultGlow/TextResult").GetComponent<Text>();
            CsUIData.Instance.SetFont(textResult);
            textResult.text = CsConfiguration.Instance.GetString("A114_TXT_00005");

            Button buttonBack = m_trPopupLuckyShopResult.Find("ButtonBack").GetComponent<Button>();
            buttonBack.onClick.RemoveAllListeners();
            buttonBack.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonBack.onClick.AddListener(OnClickLuckyShopResultBack);

            Text textButtonBack = buttonBack.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonBack);
            textButtonBack.text = CsConfiguration.Instance.GetString("A114_BTN_00006");

            Button buttonMore = m_trPopupLuckyShopResult.Find("ButtonMore").GetComponent<Button>();
            buttonMore.onClick.RemoveAllListeners();
            buttonMore.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textDia = buttonMore.transform.Find("TextDia").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDia);

            Text textMore = buttonMore.transform.Find("TextMore").GetComponent<Text>();
            CsUIData.Instance.SetFont(textMore);

            if (enLuckyShopPickType == EnLuckyShopPickType.Pick1Time)
            {
                if (enLuckyShopType == EnLuckyShopType.Item)
                {
                    textDia.text = CsGameData.Instance.ItemLuckyShop.Pick1TimeDia.ToString("#,##0");
                    buttonMore.onClick.AddListener(OnClickLuckyShopItemPick1Time);
                }
                else
                {
                    textDia.text = CsGameData.Instance.CreatureCardLuckyShop.Pick1TimeDia.ToString("#,##0");
                    buttonMore.onClick.AddListener(OnClickLuckyShopCardPick1Time);
                }

                textMore.text = CsConfiguration.Instance.GetString("A114_BTN_00007");
            }
            else
            {
                if (enLuckyShopType == EnLuckyShopType.Item)
                {
                    textDia.text = CsGameData.Instance.ItemLuckyShop.Pick5TimeDia.ToString("#,##0");
                    buttonMore.onClick.AddListener(OnClickLuckyShopItemPick5Time);
                }
                else
                {
                    textDia.text = CsGameData.Instance.CreatureCardLuckyShop.Pick5TimeDia.ToString("#,##0");
                    buttonMore.onClick.AddListener(OnClickLuckyShopCardPick5Time);
                }

                textMore.text = CsConfiguration.Instance.GetString("A114_BTN_00008");
            }

            UpdatePopupLuckyShopResult(enLuckyShopType);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdatePopupLuckyShopResult(EnLuckyShopType enLuckyShopType)
    {
        if (m_trPopupLuckyShopResult == null)
        {
            return;
        }
        else
        {
            Transform trResult = m_trPopupLuckyShopResult.Find("ResultArea/Result");
            HorizontalLayoutGroup HorizontalLayoutGroupResult = trResult.GetComponent<HorizontalLayoutGroup>();

            if (enLuckyShopType == EnLuckyShopType.Item)
            {
                HorizontalLayoutGroupResult.spacing = 40f;

                for (int i = 0; i < trResult.childCount; i++)
                {
                    trResult.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < m_listPDItemLuckyShopPickResult.Count; i++)
                {
                    CsItem csItem = CsGameData.Instance.GetItem(m_listPDItemLuckyShopPickResult[i].itemId);

                    if (csItem == null)
                    {
                        continue;
                    }
                    else
                    {
                        Transform trResultItem = trResult.Find("ResultItem" + i);

                        if (trResultItem == null)
                        {
                            if (m_goResultItem == null)
                            {
                                m_goResultItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupLuckyShop/ResultItem");
                            }

                            trResultItem = Instantiate(m_goResultItem, trResult).transform;
                            trResultItem.name = "ResultItem" + i;
                        }
                        else
                        {
                            trResultItem.gameObject.SetActive(true);
                        }

                        Transform trItemSlot = trResultItem.Find("ItemSlot");
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, false, m_listPDItemLuckyShopPickResult[i].count, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                        Text textItemName = trResultItem.Find("TextItem").GetComponent<Text>();
                        CsUIData.Instance.SetFont(textItemName);
                        textItemName.text = string.Format("<color={0}>{1}</color>", csItem.ItemGrade.ColorCode, csItem.Name);
                        
                    }
                }
            }
            else
            {
                HorizontalLayoutGroupResult.spacing = 5f;

                for (int i = 0; i < trResult.childCount; i++)
                {
                    trResult.GetChild(i).gameObject.SetActive(false);
                }

                for (int i = 0; i < m_listCsHeroCreatureCard.Count; i++)
                {
                    CsHeroCreatureCard csHeroCreatureCard = m_listCsHeroCreatureCard[i];

                    Transform trCard = trResult.Find("CreatureCard" + i);

                    if (trCard == null)
                    {
                        if (m_goCreatureCardItem == null)
                        {
                            m_goCreatureCardItem = CsUIData.Instance.LoadAsset<GameObject>("GUI/PopupCreatureCard/CreatureCardItem");
                        }

                        trCard = Instantiate(m_goCreatureCardItem, trResult).transform;
                        trCard.name = "CreatureCard" + i;
                    }
                    else
                    {
                        trCard.gameObject.SetActive(true);
                    }

                    Image imageCard = trCard.Find("ImageCard").GetComponent<Image>();
                    imageCard.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Card/card_" + csHeroCreatureCard.CreatureCard.CreatureCardId);

                    Image imageFrm = trCard.Find("ImageFrm").GetComponent<Image>();
                    imageFrm.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/frm_card_rank_" + csHeroCreatureCard.CreatureCard.CreatureCardGrade.Grade);

                    Image imageIcon = trCard.Find("ImageIcon").GetComponent<Image>();
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupCreatureCard/ico_Card_creature_" + csHeroCreatureCard.CreatureCard.CreatureCardCategory.CategoryId);

                    Text textHp = trCard.Find("TextHp").GetComponent<Text>();
                    textHp.text = csHeroCreatureCard.CreatureCard.Life.ToString("#,##0");
                    CsUIData.Instance.SetFont(textHp);

                    Text textAttack = trCard.Find("TextAttack").GetComponent<Text>();
                    textAttack.text = csHeroCreatureCard.CreatureCard.Attack.ToString("#,##0");
                    CsUIData.Instance.SetFont(textAttack);

                    Text textCardName = trCard.Find("TextName").GetComponent<Text>();
                    textCardName.text = csHeroCreatureCard.CreatureCard.Name;
                    CsUIData.Instance.SetFont(textCardName);

                    Text textDescription = trCard.Find("TextDescription").GetComponent<Text>();
                    textDescription.text = csHeroCreatureCard.CreatureCard.Description;
                    CsUIData.Instance.SetFont(textDescription);

                    Text textCount = trCard.Find("TextCount").GetComponent<Text>();
                    textCount.text = csHeroCreatureCard.Count.ToString("#,##0");
                    CsUIData.Instance.SetFont(textCount);
                }
            }

            m_bOpenResult = false;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLuckyShopResultBack()
    {
        if (m_trPopupLuckyShopResult == null)
        {
            return;
        }
        else
        {
            Destroy(m_trPopupLuckyShopResult.gameObject);
            m_trPopupLuckyShopResult = null;
        }
    }

    #endregion Result
}
