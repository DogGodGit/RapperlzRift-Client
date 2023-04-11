using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupMountPotionAttr : MonoBehaviour
{
    Transform m_trItemSlot = null;
    Transform m_trImageAttrBack = null;
    Transform m_trImageNextMaxPotionattrUseCount = null;

    Text m_textPotionAttrUseCountValue = null;
    Text m_textNextMaxPotionAttrUseCount = null;
    Text m_textNextMaxPotionAttrUseCountValue = null;

    Button m_buttonPotionAttr = null;

    int m_nMountId = 0;

    public event Delegate EventClosePopupMountAttrPotion;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMountAttrPotionUse += OnEventMountAttrPotionUse;
        CsGameEventUIToUI.Instance.EventMountLevelUp += OnEventMountLevelUp;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEventClosePopupMountAttrPotion();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMountAttrPotionUse -= OnEventMountAttrPotionUse;
        CsGameEventUIToUI.Instance.EventMountLevelUp -= OnEventMountLevelUp;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventClosePopupMountAttrPotion()
    {
        if (EventClosePopupMountAttrPotion != null)
        {
            EventClosePopupMountAttrPotion();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        OnEventClosePopupMountAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountAttrPotionUse()
    {
        UpdateButtonPotionAttr();
        UpdateImageAttr();
        UpdateItemSlot();
        UpdateMountAttrPotionText();
    }

    void OnEventMountLevelUp(bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateButtonPotionAttr();
            UpdateImageAttr();
            UpdateItemSlot();
            UpdateMountAttrPotionText();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPopupClose()
    {
        OnEventClosePopupMountAttrPotion();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPotionAttrUse()
    {
        CsCommandEventManager.Instance.SendMountAttrPotionUse(m_nMountId);
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            Transform trImageBackground = transform.Find("ImageBackground");

            Text textPopupName = trImageBackground.Find("TextPopupName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = CsConfiguration.Instance.GetString("A158_TXT_00007");

            Button buttonClose = trImageBackground.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            buttonClose.onClick.AddListener(OnClickPopupClose);

            m_trItemSlot = trImageBackground.Find("ImageIcon/ItemSlot");

            UpdateItemSlot();

            m_trImageNextMaxPotionattrUseCount = trImageBackground.Find("ImageNextMaxPotionAttrUseCount");

            Text textPotionAttrUseCount = trImageBackground.Find("ImagePotionAttrUseCount/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPotionAttrUseCount);
            textPotionAttrUseCount.text = CsConfiguration.Instance.GetString("A158_TXT_00008");

            m_textNextMaxPotionAttrUseCount = m_trImageNextMaxPotionattrUseCount.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textNextMaxPotionAttrUseCount);

            m_textPotionAttrUseCountValue = trImageBackground.Find("ImagePotionAttrUseCount/TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textPotionAttrUseCountValue);

            m_textNextMaxPotionAttrUseCountValue = m_trImageNextMaxPotionattrUseCount.Find("TextValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(m_textNextMaxPotionAttrUseCountValue);

            UpdateMountAttrPotionText();

            m_buttonPotionAttr = trImageBackground.Find("ButtonPotionAttr").GetComponent<Button>();
            m_buttonPotionAttr.onClick.RemoveAllListeners();
            m_buttonPotionAttr.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
            m_buttonPotionAttr.onClick.AddListener(OnClickPotionAttrUse);

            Text textButtonPotionAttr = m_buttonPotionAttr.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonPotionAttr);
            textButtonPotionAttr.text = CsConfiguration.Instance.GetString("A158_TXT_00010");

            UpdateButtonPotionAttr();

            m_trImageAttrBack = trImageBackground.Find("ImageAttrBack");

            UpdateImageAttr();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateItemSlot()
    {
        CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.MountPotionAttrItemId);
        int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);

        if (csItem == null)
        {
            m_trItemSlot.gameObject.SetActive(false);
        }
        else
        {
            CsUIData.Instance.DisplayItemSlot(m_trItemSlot, csItem, true, nItemCount, csItem.UsingRecommendationEnabled, EnItemSlotSize.Medium, false);
            m_trItemSlot.gameObject.SetActive(true);

            Text textCount = m_trItemSlot.Find("Item/TextCount").GetComponent<Text>();
            Image imageCooltime = m_trItemSlot.Find("ImageCooltime").GetComponent<Image>();

            if (nItemCount == 0)
            {
                textCount.text = nItemCount.ToString("#,##0");
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
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMountAttrPotionText()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            CsMountQuality csMountQuality = null;

            csMountQuality = csHeroMount.Mount.GetMountQuality(csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality);

            if (csMountQuality == null)
            {
                m_textPotionAttrUseCountValue.text = "";
            }
            else
            {
                m_textPotionAttrUseCountValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), csHeroMount.PotionAttrCount, csMountQuality.PotionAttrMaxCount);
            }

            csMountQuality = csHeroMount.Mount.GetMountQuality(csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality + 1);

            if (csMountQuality == null)
            {
                m_textNextMaxPotionAttrUseCount.text = "";
                m_trImageNextMaxPotionattrUseCount.gameObject.SetActive(false);
            }
            else
            {
                string strMountQuality = string.Format("<color={0}>{1}</color>", csMountQuality.MountQualityMaster.ColorCode, csMountQuality.MountQualityMaster.Name);
                m_textNextMaxPotionAttrUseCount.text = string.Format(CsConfiguration.Instance.GetString("A158_TXT_00009"), strMountQuality);
                m_textNextMaxPotionAttrUseCountValue.text = csMountQuality.PotionAttrMaxCount.ToString("#,##0");
                m_trImageNextMaxPotionattrUseCount.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPotionAttr()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            bool bInteractable = false;
            CsMountQuality csMountQuality = csHeroMount.Mount.GetMountQuality(csHeroMount.MountLevel.MountLevelMaster.MountQualityMaster.Quality);

            if (csMountQuality == null)
            {
                return;
            }
            else
            {
                if (csHeroMount.PotionAttrCount < csMountQuality.PotionAttrMaxCount)
                {
                    // 사용 가능
                    if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountPotionAttrItemId) <= 0)
                    {
                        bInteractable = false;
                    }
                    else
                    {
                        bInteractable = true;
                    }
                }
                else
                {
                    bInteractable = false;
                }
            }

            CsUIData.Instance.DisplayButtonInteractable(m_buttonPotionAttr, bInteractable);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageAttr()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            Transform trAttr = null;
            
            float flFactor = 1.0f;

            if (csHeroMount.Mount.MountId == CsGameData.Instance.MyHeroInfo.EquippedMountId)
            {
                flFactor = 1.0f;
            }
            else
            {
                flFactor = csHeroMount.MountAwakeningLevelMaster.UnequippedAttrFactor;
            }

            List<CsMountPotionAttrCount> list = CsGameData.Instance.GetMountPotionAttrCountList(csHeroMount.PotionAttrCount);
            List<CsAttr> listAttr = new List<CsAttr>();

            for (int i = 0; i < CsGameData.Instance.MountPotionAttrCountList.Count; i++)
            {
                CsMountPotionAttrCount csMountPotionAttrCount = CsGameData.Instance.MountPotionAttrCountList[i];

                if (listAttr.Find(a => a == csMountPotionAttrCount.Attr) == null)
                {
                    listAttr.Add(csMountPotionAttrCount.Attr);
                }
                else
	            {
                    continue;
	            }
            }

            listAttr.OrderBy(a => a.AttrId).ToList();

            for (int i = 0; i < m_trImageAttrBack.childCount; i++)
            {
                trAttr = m_trImageAttrBack.GetChild(i);

                if (i < listAttr.Count)
                {
                    Text textAttr = trAttr.Find("Text").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttr);
                    textAttr.text = listAttr[i].Name;

                    Text textAttrValue = trAttr.Find("TextValue").GetComponent<Text>();
                    CsUIData.Instance.SetFont(textAttrValue);

                    int nAttrValue = 0;
                    CsMountPotionAttrCount csMountPotionAttrCount = list.Find(a => a.Attr.AttrId == listAttr[i].AttrId);
                    
                    if (csMountPotionAttrCount == null)
                    {
                        nAttrValue = 0;
                    }
                    else
                    {
                        nAttrValue = csMountPotionAttrCount.AttrValue.Value;
                    }

                    textAttrValue.text = nAttrValue.ToString("#,##0");
                }
                else
                {
                    trAttr.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void DisplayPopupMountAttrPotion(int nMountId)
    {
        m_nMountId = nMountId;
        InitializeUI();
    }
}
