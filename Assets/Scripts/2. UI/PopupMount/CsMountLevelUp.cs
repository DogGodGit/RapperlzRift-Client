using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsMountLevelUp : CsPopupSub
{
    Transform m_trMaxLevel = null;

    int m_nMountId = -1;
    int m_nPreSatiety = 0;
    int m_nTotalSatiety = 0;
    int m_nNextLevelSatiety = 0;

    bool m_bLevelupIng = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventMountSelected += OnEventMountSelected;
        CsGameEventUIToUI.Instance.EventMountLevelUp += OnEventMountLevelUp;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventMountSelected -= OnEventMountSelected;
        CsGameEventUIToUI.Instance.EventMountLevelUp -= OnEventMountLevelUp;

        ToastSatiety();
    }

    void OnDisable()
    {
        if (m_bLevelupIng)
        {
            ToastSatiety();
            m_bLevelupIng = false;
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
        DisplayLevelUpItemCount();
        UpdateMountQuality();

        if (m_bLevelupIng)
        {
            ToastSatiety();
            m_bLevelupIng = false;
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountLevelUp(bool bLevelUp)
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (bLevelUp)
        {
            DisplayLevelUpItemCount();
            UpdateMountQuality();
            //레벨업시 레벨업 후 오른 포만감과 레벨업 전에 포만감을 더해준다.
            m_nTotalSatiety += csHeroMount.Satiety;
            m_nTotalSatiety += m_nNextLevelSatiety - m_nPreSatiety;

            ToastSatiety();

            m_bLevelupIng = false;
        }
        else if (m_bLevelupIng)
        {
            //육성전 포만감에서 육성후 포만감을 뺀 값을 누적
            m_nTotalSatiety += csHeroMount.Satiety - m_nPreSatiety;
            m_nPreSatiety = csHeroMount.Satiety;

            UpdateSatiety(csHeroMount);
            DisplayLevelUpItemCount();

            if (CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountLevelUpItemId) > 0)
            {
                StartCoroutine(WaitingLevelUp());
            }
            else
            {
                ToastSatiety();
            }
        }
        else
        {
            if (m_nTotalSatiety != 0)
            {
                UpdateSatiety(csHeroMount);
                DisplayLevelUpItemCount();
                ToastSatiety();
            }
            else
            {
                return;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    //탈것 레벨업
    void OnClickLevelUp()
    {
        if (!m_bLevelupIng && 0 < CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountLevelUpItemId))
        {
            m_bLevelupIng = true;
            //현재 포만감을 저장
            m_nPreSatiety = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId).Satiety;
            m_nTotalSatiety = 0;
            m_nNextLevelSatiety = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId).MountLevel.MountLevelMaster.NextLevelUpRequiredSatiety;
            CsCommandEventManager.Instance.SendMountLevelUp(m_nMountId);
        }
        else
        {
            return;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        if (CsUIData.Instance.SelectMountId == 0)
        {
            m_nMountId = CsGameData.Instance.MyHeroInfo.EquippedMountId;
        }
        else
        {
            m_nMountId = CsUIData.Instance.SelectMountId;
        }

        m_trMaxLevel = transform.Find("TextMaxLevel");

        Text textMaxLevel = m_trMaxLevel.GetComponent<Text>();
        CsUIData.Instance.SetFont(textMaxLevel);
        textMaxLevel.text = CsConfiguration.Instance.GetString("A158_TXT_00003");

        Button buttonLevelUp = transform.Find("ButtonLevelUp").GetComponent<Button>();
        buttonLevelUp.onClick.RemoveAllListeners();
        buttonLevelUp.onClick.AddListener(OnClickLevelUp);
        buttonLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textLevelUp = buttonLevelUp.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLevelUp);
        textLevelUp.text = CsConfiguration.Instance.GetString("A19_BTN_00002");

        //강화 재료 이미지
        Image imageMaterial = buttonLevelUp.transform.Find("ImageMaterial").GetComponent<Image>();
        imageMaterial.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/items/item_" + CsGameConfig.Instance.MountLevelUpItemId);

        UpdateMountQuality();
        DisplayLevelUpItemCount();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateMountQuality()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);
        CsMountLevelMaster csMountLevelMaster = null;

        if (csHeroMount == null)
        {
            CsMount csMount = CsGameData.Instance.GetMount(m_nMountId);
            csMountLevelMaster = csMount.GetMountLevel(1).MountLevelMaster;

            Text textQuality = transform.Find("TextQuality").GetComponent<Text>();
            textQuality.text = string.Format(CsConfiguration.Instance.GetString("A19_TXT_01002"), csMountLevelMaster.MountQualityMaster.ColorCode, csMountLevelMaster.MountQualityMaster.Name, csMountLevelMaster.QualityLevel);
            CsUIData.Instance.SetFont(textQuality);

            Transform trSatrList = textQuality.transform.Find("StarList");

            for (int i = 0; i < CsGameConfig.Instance.MountQualityUpRequiredLevelUpCount; ++i)
            {
                Transform trStarOn = trSatrList.Find("ImageStar" + i + "/ImageOn");
                if (i <= 0)
                {
                    trStarOn.gameObject.SetActive(true);
                }
                else
                {
                    trStarOn.gameObject.SetActive(false);
                }
            }

            Slider sliderQualityGuage = transform.Find("TextQuality/QualityGuage").GetComponent<Slider>();
            int nSatiety = 0;
            int nNextSatiety = csMount.GetMountLevel(1).MountLevelMaster.NextLevelUpRequiredSatiety;
            sliderQualityGuage.value = (float)nSatiety / (float)nNextSatiety;

            Text textSatiety = sliderQualityGuage.transform.Find("TextSatiety").GetComponent<Text>();
            textSatiety.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nSatiety, nNextSatiety);
            CsUIData.Instance.SetFont(textSatiety);
        }
        else
        {
            csMountLevelMaster = csHeroMount.MountLevel.MountLevelMaster;

            Text textQuality = transform.Find("TextQuality").GetComponent<Text>();
            textQuality.text = string.Format(CsConfiguration.Instance.GetString("A19_TXT_01002"), csMountLevelMaster.MountQualityMaster.ColorCode, csMountLevelMaster.MountQualityMaster.Name, csMountLevelMaster.QualityLevel);
            CsUIData.Instance.SetFont(textQuality);

            Transform trSatrList = textQuality.transform.Find("StarList");
            int nLevel = (csHeroMount.Level - 1) % CsGameConfig.Instance.MountQualityUpRequiredLevelUpCount;

            for (int i = 0; i < CsGameConfig.Instance.MountQualityUpRequiredLevelUpCount; ++i)
            {
                Transform trStarOn = trSatrList.Find("ImageStar" + i + "/ImageOn");
                if (i <= nLevel)
                {
                    trStarOn.gameObject.SetActive(true);
                }
                else
                {
                    trStarOn.gameObject.SetActive(false);
                }
            }

            UpdateSatiety(csHeroMount);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //포만감 갱신
    void UpdateSatiety(CsHeroMount csHeroMount)
    {
        Slider sliderQualityGuage = transform.Find("TextQuality/QualityGuage").GetComponent<Slider>();
        int nSatiety = csHeroMount.Satiety;
        int nNextSatiety = csHeroMount.Mount.GetMountLevel(csHeroMount.Level).MountLevelMaster.NextLevelUpRequiredSatiety;
        sliderQualityGuage.value = (float)nSatiety / (float)nNextSatiety;

        Text textSatiety = sliderQualityGuage.transform.Find("TextSatiety").GetComponent<Text>();
        textSatiety.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nSatiety, nNextSatiety);
    }

    //---------------------------------------------------------------------------------------------------
    //획득한 포만감 토스트 출력
    void ToastSatiety()
    {
        if (m_nTotalSatiety != 0)
        {
            CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A19_TXT_02001"), m_nTotalSatiety));
        }

        m_nPreSatiety = 0;
        m_nTotalSatiety = 0;
        m_nNextLevelSatiety = 0;
    }

    //---------------------------------------------------------------------------------------------------
    //이벤트 보내기 시간 지연
    IEnumerator WaitingLevelUp()
    {
        yield return new WaitForSeconds(0.1f);

        if (m_bLevelupIng)
        {
            CsCommandEventManager.Instance.SendMountLevelUp(m_nMountId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    //레벨업 아이템 업데이트
    void DisplayLevelUpItemCount()
    {
        CsHeroMount csHeroMount = CsGameData.Instance.MyHeroInfo.GetHeroMount(m_nMountId);

        if (csHeroMount == null)
        {
            m_trMaxLevel.gameObject.SetActive(false);
        }
        else
        {
            CsMountLevelMaster csMountLevelMaster = CsGameData.Instance.MountLevelMasterList[(CsGameData.Instance.MountLevelMasterList.Count - 1)];

            if (csHeroMount.Level >= csMountLevelMaster.Level)
            {
                m_trMaxLevel.gameObject.SetActive(true);
            }
            else
            {
                m_trMaxLevel.gameObject.SetActive(false);
            }
        }

        Button buttonLevelUp = transform.Find("ButtonLevelUp").GetComponent<Button>();

        if (!m_trMaxLevel.gameObject.activeSelf)
        {
            buttonLevelUp.gameObject.SetActive(true);

            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(CsGameConfig.Instance.MountLevelUpItemId);

            Text textMaterialCount = transform.transform.Find("ButtonLevelUp/ImageMaterial/TextMaterial").GetComponent<Text>();
            textMaterialCount.text = nItemCount.ToString("#,##0");
            textMaterialCount.color = nItemCount > 0 ? CsUIData.Instance.ColorWhite : CsUIData.Instance.ColorRed;
            CsUIData.Instance.SetFont(textMaterialCount);

            if (csHeroMount == null || nItemCount <= 0)
            {
                textMaterialCount.color = nItemCount > 0 ? CsUIData.Instance.ColorGray : CsUIData.Instance.ColorRed;
                buttonLevelUp.transform.Find("ImageMaterial").GetComponent<Image>().color = CsUIData.Instance.ColorButtonOff;
                CsUIData.Instance.DisplayButtonInteractable(buttonLevelUp, false);
            }
            else
            {
                textMaterialCount.color = CsUIData.Instance.ColorWhite;
                buttonLevelUp.transform.Find("ImageMaterial").GetComponent<Image>().color = CsUIData.Instance.ColorButtonOn;
                CsUIData.Instance.DisplayButtonInteractable(buttonLevelUp, true);
            }
        }
        else
        {
            buttonLevelUp.gameObject.SetActive(false);
        }
    }
}