using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-10-01)
//---------------------------------------------------------------------------------------------------

public class CsMountAwakeningLevelUp : CsPopupSub
{
    Transform m_trAwakeningFrame;
    Transform m_trMaxLevel;

    Button m_ButtonAwakening = null;

    int m_nMountId = -1;
    int m_nOldAwakeningLevel = -1;

    bool m_bLevelupIng = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp += OnEventMountAwakeningLevelUp;
        CsGameEventUIToUI.Instance.EventMountSelected += OnEventMountSelected;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMountAwakeningLevelUp -= OnEventMountAwakeningLevelUp;
        CsGameEventUIToUI.Instance.EventMountSelected -= OnEventMountSelected;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_bLevelupIng)
        {
            m_bLevelupIng = false;
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountAwakeningLevelUp()
    {
        Debug.Log("OnEventMountAwakeningLevelUp");
        UpdateAwakeningLevel();
        UpdateSliderAwakening();
        DisplayButtonAwakening();
        
        if (AwakeningLevelUp())
        {
            m_bLevelupIng = false;
        }
        else if (m_bLevelupIng)
        {
            if (0 < CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountAwakeningItemId))
            {
                StartCoroutine(WaitingLevelUp());
            }
            else
            {

            }
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountSelected(int nMountId)
    {
        m_nMountId = nMountId;
        CsUIData.Instance.SelectMountId = nMountId;

        UpdateAwakeningLevel();
        UpdateSliderAwakening();
        DisplayButtonAwakening();

        if (m_bLevelupIng)
        {
            m_bLevelupIng = false;
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAwakening()
    {
        // 마지막 각성 레벨 체크
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return;
        }
        else
        {
            if (csHeroMount.AwakeningLevel == CsGameData.Instance.MountAwakeningLevelMasterList.Last().AwakeningLevel)
            {
                return;
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.MountAwakeningRequiredHeroLevel)
                {
                    // 레벨 부족
                    return;
                }
                else
                {
                    if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountAwakeningItemId) <= 0)
                    {
                        // 아이템 부족
                        return;
                    }
                    else
                    {
                        if (!m_bLevelupIng)
                        {
                            m_bLevelupIng = true;
                            m_nOldAwakeningLevel = csHeroMount.AwakeningLevel;
                            CsCommandEventManager.Instance.SendMountAwakeningLevelUp(m_nMountId);
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        if (CsGameData.Instance.MyHeroInfo.EquippedMountId == 0)
        {
            CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            return;
        }

        if (CsUIData.Instance.SelectMountId == 0)
        {
            m_nMountId = CsGameData.Instance.MyHeroInfo.EquippedMountId;
        }
        else
        {
            m_nMountId = CsUIData.Instance.SelectMountId;
        }

        m_trAwakeningFrame = transform.Find("AwakeningFrame");

        m_ButtonAwakening = transform.Find("ButtonAwakening").GetComponent<Button>();
        m_ButtonAwakening.onClick.RemoveAllListeners();
        m_ButtonAwakening.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        m_ButtonAwakening.onClick.AddListener(OnClickAwakening);

        Text textButtonAwakening = m_ButtonAwakening.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonAwakening);
        textButtonAwakening.text = CsConfiguration.Instance.GetString("A158_TXT_00006");

        Image imageItemIcon = m_ButtonAwakening.transform.Find("ImageItemIcon").GetComponent<Image>();
        imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/items/item_" + CsGameConfig.Instance.MountAwakeningItemId);

        m_trMaxLevel = transform.Find("TextMaxLevel");
        Text textMaxLevel = m_trMaxLevel.GetComponent<Text>();
        CsUIData.Instance.SetFont(textMaxLevel);
        textMaxLevel.text = CsConfiguration.Instance.GetString("A158_TXT_00003");

        UpdateAwakeningLevel();
        UpdateSliderAwakening();
        DisplayButtonAwakening();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateAwakeningLevel()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
        Transform trAwakeningLevel = m_trAwakeningFrame.Find("AwakeningLevel");

        Text textCurrentLevel = trAwakeningLevel.Find("TextCurrentLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCurrentLevel);

        Text textNextLevel = trAwakeningLevel.Find("TextNextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNextLevel);

        Transform trImageIcon = trAwakeningLevel.Find("Image");

        if (csHeroMount == null)
        {
            textCurrentLevel.alignment = TextAnchor.MiddleRight;
            textCurrentLevel.text = string.Format(CsConfiguration.Instance.GetString("A158_TXT_00004"), CsGameData.Instance.MountAwakeningLevelMasterList.First().AwakeningLevel);
            textNextLevel.text = (CsGameData.Instance.MountAwakeningLevelMasterList.First().AwakeningLevel + 1).ToString("#,##0");

            trImageIcon.gameObject.SetActive(true);
            textNextLevel.gameObject.SetActive(true);
        }
        else
        {

            if (csHeroMount.AwakeningLevel == CsGameData.Instance.MountAwakeningLevelMasterList.Last().AwakeningLevel)
            {
                textCurrentLevel.text = string.Format(CsConfiguration.Instance.GetString("A158_TXT_00005"), csHeroMount.AwakeningLevel);
                textCurrentLevel.alignment = TextAnchor.MiddleCenter;
                trImageIcon.gameObject.SetActive(false);
                textNextLevel.gameObject.SetActive(false);
            }
            else
            {
                textCurrentLevel.text = string.Format(CsConfiguration.Instance.GetString("A158_TXT_00004"), csHeroMount.AwakeningLevel);
                textCurrentLevel.alignment = TextAnchor.MiddleRight;
                textNextLevel.text = (csHeroMount.AwakeningLevel + 1).ToString("#,##0");
                trImageIcon.gameObject.SetActive(true);
                textNextLevel.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSliderAwakening()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        Slider sliderAwakening = m_trAwakeningFrame.Find("SliderAwakening").GetComponent<Slider>();

        Text textValue = sliderAwakening.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);

        if (csHeroMount == null)
        {
            CsMount csMount = CsGameData.Instance.GetMount(m_nMountId);

            sliderAwakening.maxValue = csMount.GetMountAwakeningLevel(CsGameData.Instance.MountAwakeningLevelMasterList.First().AwakeningLevel).NextLevelUpRequiredAwakeningExp;
            sliderAwakening.value = 0f;
        }
        else
        {
            if (csHeroMount.AwakeningLevel == CsGameData.Instance.MountAwakeningLevelMasterList.Last().AwakeningLevel)
            {
                sliderAwakening.maxValue = csHeroMount.Mount.GetMountAwakeningLevel(csHeroMount.AwakeningLevel).NextLevelUpRequiredAwakeningExp;
                sliderAwakening.value = sliderAwakening.maxValue;
            }
            else
            {
                sliderAwakening.maxValue = csHeroMount.Mount.GetMountAwakeningLevel(csHeroMount.AwakeningLevel).NextLevelUpRequiredAwakeningExp;
                sliderAwakening.value = csHeroMount.AwakeningExp;
            }
        }

        textValue.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), sliderAwakening.value, sliderAwakening.maxValue);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayButtonAwakening()
    {
        CsItem csItem = CsGameData.Instance.GetItem(CsGameConfig.Instance.MountAwakeningItemId);
        int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItem.ItemId);

        Text textValue = m_ButtonAwakening.transform.Find("ImageItemIcon/TextValue").GetComponent<Text>();
        CsUIData.Instance.SetFont(textValue);
        textValue.text = nItemCount.ToString("#,##0");

        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            // 획득 탈 것이 아닐 때
            m_ButtonAwakening.interactable = false;

            m_trMaxLevel.gameObject.SetActive(false);
            m_ButtonAwakening.gameObject.SetActive(true);
        }
        else
        {
            // 최고 레벨일 때
            if (csHeroMount.AwakeningLevel == CsGameData.Instance.MountAwakeningLevelMasterList.Last().AwakeningLevel)
            {
                m_ButtonAwakening.gameObject.SetActive(false);
                m_trMaxLevel.gameObject.SetActive(true);
            }
            else
            {
                if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.MountAwakeningRequiredHeroLevel)
                {
                    m_ButtonAwakening.interactable = false;
                }
                else
                {
                    if (nItemCount <= 0)
                    {
                        m_ButtonAwakening.interactable = false;
                    }
                    else
                    {
                        m_ButtonAwakening.interactable = true;
                    }
                }

                m_trMaxLevel.gameObject.SetActive(false);
                m_ButtonAwakening.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //이벤트 보내기 시간 지연
    IEnumerator WaitingLevelUp()
    {
        yield return new WaitForSeconds(0.1f);

        if (m_bLevelupIng)
        {
            CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
            m_nOldAwakeningLevel = csHeroMount.AwakeningLevel;
            CsCommandEventManager.Instance.SendMountAwakeningLevelUp(m_nMountId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    bool AwakeningLevelUp()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            return false;
        }
        else
        {
            if (csHeroMount.AwakeningLevel != m_nOldAwakeningLevel)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}