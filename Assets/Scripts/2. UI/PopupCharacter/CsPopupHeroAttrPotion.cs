using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupHeroAttrPotion : MonoBehaviour 
{
    Transform m_trHeroAttrPotionList;

    Button m_buttonHeroAttrPotionUseAll;

    public event Delegate EventClosePopupHeroAttrPotion;

    //---------------------------------------------------------------------------------------------------
    void Awake() 
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventHeroAttrPotionUse += OnEventHeroAttrPotionUse;
        CsGameEventUIToUI.Instance.EventHeroAttrPotionUseAll += OnEventHeroAttrPotionUseAll;

        InitializeUI();
	}

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopupHeroAttrPotion();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventHeroAttrPotionUse -= OnEventHeroAttrPotionUse;
        CsGameEventUIToUI.Instance.EventHeroAttrPotionUseAll -= OnEventHeroAttrPotionUseAll;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopupHeroAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroAttrPotionUse()
    {
        UpdateHeroAttrPotionList();
        UpdateInteractableButtonHeroAttrPotionUseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroAttrPotionUseAll()
    {
        UpdateHeroAttrPotionList();
        UpdateInteractableButtonHeroAttrPotionUseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClosePopupHeroAttrPotion()
    {
        ClosePopupHeroAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHeroAttrPotionUse(CsPotionAttr csPotionAttr)
    {
        if (CsGameData.Instance.MyHeroInfo.GetItemCount(csPotionAttr.ItemRequired.ItemId) == 0)
        {
            // 아이템이 없음
            Debug.Log("아이템이 없음");
        }
        else
        {
            int nHeroAttrPotionUseCount = GetHeroAttrPotionUseCount(csPotionAttr);

            if (CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.PotionAttrMaxCount <= nHeroAttrPotionUseCount)
            {
                // 포션 사용 개수가 초과
                Debug.Log("포션 사용 개수가 초과");
            }
            else
            {
                CsCommandEventManager.Instance.SendHeroAttrPotionUse(csPotionAttr.PotionAttrId);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickHeroAttrPotionUseAll()
    {
        CsCommandEventManager.Instance.SendHeroAttrPotionUseAll();
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trImageBackground = transform.Find("ImageBackground");

        Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A155_TXT_00001");

        Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        buttonClose.onClick.AddListener(OnClickClosePopupHeroAttrPotion);

        m_trHeroAttrPotionList = trImageBackground.Find("HeroAttrPotionList");

        UpdateHeroAttrPotionList();

        Text textDescription = trImageBackground.Find("ImageLine/TextDescription").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDescription);
        textDescription.text = CsConfiguration.Instance.GetString("A155_TXT_00005");

        m_buttonHeroAttrPotionUseAll = trImageBackground.Find("ImageLine/ButtonHeroAttrPotionUseAll").GetComponent<Button>();
        m_buttonHeroAttrPotionUseAll.onClick.RemoveAllListeners();
        m_buttonHeroAttrPotionUseAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_buttonHeroAttrPotionUseAll.onClick.AddListener(() => OnClickHeroAttrPotionUseAll());
        UpdateInteractableButtonHeroAttrPotionUseAll();

        Text textHeroAttrPotionUseAll = m_buttonHeroAttrPotionUseAll.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textHeroAttrPotionUseAll);
        textHeroAttrPotionUseAll.text = CsConfiguration.Instance.GetString("A155_TXT_00004");
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateHeroAttrPotionList()
    {
        Transform trHeroAttrPotion = null;
        Transform trItemSlot = null;

        CsHeroPotionAttr csHeroPotionAttr = null;

        Button buttonHeroAttrPotionUse = null;

        Text textAttrName = null;
        Text textAttrValue = null;
        Text textDailyUseCount = null;
        Text textDailyUseCountValue = null;
        Text textButtonHeroAttrPotionUse = null;

        for (int i = 0; i < m_trHeroAttrPotionList.childCount; i++)
        {
            trHeroAttrPotion = m_trHeroAttrPotionList.Find("HeroAttrPotion" + i);

            if (trHeroAttrPotion == null)
            {
                continue;
            }
            else
            {
                if (i < CsGameData.Instance.PotionAttrList.Count)
                {
                    CsPotionAttr csPotionAttr = CsGameData.Instance.PotionAttrList[i];
                    csHeroPotionAttr = CsGameData.Instance.MyHeroInfo.HeroPotionAttrList.Find(a => a.PotionAttr.PotionAttrId == csPotionAttr.PotionAttrId);

                    textAttrName = trHeroAttrPotion.Find("TextAttrName").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttrName);
                    textAttrName.text = csPotionAttr.Attr.Name;

                    // 마신 개수 * 증가량
                    textAttrValue = trHeroAttrPotion.Find("TextAttrValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttrValue);

                    if (csHeroPotionAttr == null)
                    {
                        textAttrValue.text = "0";
                    }
                    else
                    {
                        textAttrValue.text = (csHeroPotionAttr.Count * csHeroPotionAttr.PotionAttr.AttrValueInc.Value).ToString("#,##0");
                    }

                    trItemSlot = trHeroAttrPotion.Find("ItemSlot");
                    CsItem csItem = csPotionAttr.ItemRequired;

                    if (csItem == null)
                    {
                        continue;
                    }
                    else
                    {
                        int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);
                        CsUIData.Instance.DisplayItemSlot(trItemSlot, csItem, true, nCount, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);

                        Text textCount = trItemSlot.Find("Item/TextCount").GetComponent<Text>();
                        Image imageCooltime = trItemSlot.Find("ImageCooltime").GetComponent<Image>();

                        if (nCount == 0)
                        {
                            textCount.text = nCount.ToString("#,##0");
                            textCount.color = CsUIData.Instance.ColorRed;
                            textCount.gameObject.SetActive(true);
                            imageCooltime.fillAmount = 1f;
                        }
                        else
                        {
                            textCount.color = CsUIData.Instance.ColorWhite;
                            imageCooltime.fillAmount = 0f;
                        }
                    }

                    textDailyUseCount = trHeroAttrPotion.Find("ImageDailyUseCount/Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textDailyUseCount);
                    textDailyUseCount.text = CsConfiguration.Instance.GetString("A155_TXT_00002");

                    int nUseHeroAttrPotionCount = GetHeroAttrPotionUseCount(csPotionAttr);

                    textDailyUseCountValue = trHeroAttrPotion.Find("ImageDailyUseCount/TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textDailyUseCountValue);
                    textDailyUseCountValue.text = string.Format(CsConfiguration.Instance.GetString("A155_TXT_00006"), nUseHeroAttrPotionCount, CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.PotionAttrMaxCount);

                    buttonHeroAttrPotionUse = trHeroAttrPotion.Find("ButtonHeroAttrPotionUse").GetComponent<Button>();
                    buttonHeroAttrPotionUse.onClick.RemoveAllListeners();
                    buttonHeroAttrPotionUse.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    buttonHeroAttrPotionUse.onClick.AddListener(() => OnClickHeroAttrPotionUse(csPotionAttr));

                    bool bInteractable = true;

                    if (0 < CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId))
                    {
                        if (nUseHeroAttrPotionCount < CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.PotionAttrMaxCount)
                        {
                            bInteractable = true;
                        }
                        else
                        {
                            bInteractable = false;
                        }
                    }
                    else
                    {
                        bInteractable = false;
                    }

                    CsUIData.Instance.DisplayButtonInteractable(buttonHeroAttrPotionUse, bInteractable);

                    Transform trImageNotice = buttonHeroAttrPotionUse.transform.Find("ImageNotice");
                    trImageNotice.gameObject.SetActive(bInteractable);

                    textButtonHeroAttrPotionUse = buttonHeroAttrPotionUse.transform.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textButtonHeroAttrPotionUse);
                    textButtonHeroAttrPotionUse.text = CsConfiguration.Instance.GetString("A155_TXT_00003");

                    trHeroAttrPotion.gameObject.SetActive(true);
                }
                else
                {
                    trHeroAttrPotion.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupHeroAttrPotion()
    {
        if (EventClosePopupHeroAttrPotion != null)
        {
            EventClosePopupHeroAttrPotion();
        }
    }

    //---------------------------------------------------------------------------------------------------
    int GetHeroAttrPotionUseCount(CsPotionAttr csPotionAttr)
    {
        if (csPotionAttr == null)
        {
            return 0;
        }
        else
        {
            int nHeroPotionAttrUseCount = 0;
            CsHeroPotionAttr csHeroPotionAttr = CsGameData.Instance.MyHeroInfo.HeroPotionAttrList.Find(a => a.PotionAttr.PotionAttrId == csPotionAttr.PotionAttrId);

            if (csHeroPotionAttr == null)
            {
                nHeroPotionAttrUseCount = 0;
            }
            else
            {
                nHeroPotionAttrUseCount = csHeroPotionAttr.Count;
            }

            return nHeroPotionAttrUseCount;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateInteractableButtonHeroAttrPotionUseAll()
    {
        int nUseCount = 0;
        bool bInteractable = false;
        
        for (int i = 0; i < CsGameData.Instance.PotionAttrList.Count; i++)
        {
            nUseCount = GetHeroAttrPotionUseCount(CsGameData.Instance.PotionAttrList[i]);

            if (nUseCount < CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.PotionAttrMaxCount && 
                0 < CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameData.Instance.PotionAttrList[i].ItemRequired.ItemId))
            {
                bInteractable = true;
            }
            else
            {
                continue;
            }
        }

        CsUIData.Instance.DisplayButtonInteractable(m_buttonHeroAttrPotionUseAll, bInteractable);
    }
}
