using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-07)
//---------------------------------------------------------------------------------------------------

public class CsSkillLevelUp : CsPopupSub
{
    Transform m_trBack;
    Transform m_trPopupList;
    Transform m_trLevelUpResource;

    GameObject m_goPopupCalculator;
    GameObject m_goPopupJobChange;

    Text m_textLevel;
    Text m_textName;
    Text m_textDesc;
    Text m_textCoolTime;
    Text m_textNowLevelDesc;
    Text m_textNextLevelDesc;
    Text m_textLevelUpGold;
    Text m_textLevelUpSkillBookName;
    Text m_textLevelUpSkillBookCount;
    Text m_textNeedLevel;

    Button m_buttonPreview;
    Button m_buttonLevelUp;
    Button m_buttonAllLevelUp;
    Button m_buttonBuySkillBook;

    CsHeroSkill m_csHeroSkill;
    CsHonorShopProduct m_csHonorShopProduct;
    CsPopupCalculator m_csPopupCalculator;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSkillSelected += OnEventSkillSelected;
        CsJobChangeManager.Instance.EventHeroJobChange += OnEventHeroJobChange;
        CsGameEventUIToUI.Instance.EventHonorShopProductBuy += OnEventHonorShopProductBuy;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_csPopupCalculator != null)
        {
            OnEventCloseCalculator();
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
        CsGameEventUIToUI.Instance.EventSkillSelected -= OnEventSkillSelected;
        CsJobChangeManager.Instance.EventHeroJobChange -= OnEventHeroJobChange;
        CsGameEventUIToUI.Instance.EventHonorShopProductBuy -= OnEventHonorShopProductBuy;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventSkillSelected(int nSkillId)
    {
        m_csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(nSkillId);
        m_csHonorShopProduct = CsGameData.Instance.GetHonorShopProduct(nSkillId);

        UpdateSkillInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroJobChange()
    {
        UpdateButtonPreview();
    }

    void OnEventHonorShopProductBuy()
    {
        CsJobSkillLevelMaster csJobSkillLevelMaster = m_csHeroSkill.JobSkillMaster.GetJobSkillLevelMaster(m_csHeroSkill.JobSkillLevel.Level);

        if (csJobSkillLevelMaster == null)
        {
            return;
        }
        else
        {
            //수량
            int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csJobSkillLevelMaster.NextLevelUpItem.ItemId);

            m_textLevelUpSkillBookCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nItemCount, csJobSkillLevelMaster.NextLevelUpItemCount);

            if (nItemCount >= csJobSkillLevelMaster.NextLevelUpItemCount)
            {
                //아이템 수량 충분
                m_textLevelUpSkillBookCount.color = CsUIData.Instance.ColorWhite;
            }
            else
            {
                //아이템부족
                m_textLevelUpSkillBookCount.color = CsUIData.Instance.ColorRed;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSkillLevelUp()
    {
        if (m_csHeroSkill.IsLevelUp)
        {
            //레벨업
            CsCommandEventManager.Instance.SendSkillLevelUp(m_csHeroSkill.JobSkill.SkillId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickSkillLevelUpTotally()
    {
        if (m_csHeroSkill.IsLevelUp)
        {
            //전체레벨업
            CsCommandEventManager.Instance.SendSkillLevelUpTotally(m_csHeroSkill.JobSkill.SkillId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenCalculator()
    {
        //스킬북 구매 팝업 오픈.
        if (m_goPopupCalculator == null)
        {
            StartCoroutine(LoadPopupSkillBookShop());
        }
        else
        {
            OpenPopupBuyItem();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickPreview()
    {
        //전직미리보기
        if (m_goPopupJobChange == null)
        {
            StartCoroutine(LoadPopupJobChange());
        }
        else
        {
            OpenPopupJobChange();
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Transform trCanvas2 = GameObject.Find("Canvas2").transform;
        m_trPopupList = trCanvas2.Find("PopupList");
        m_trBack = transform.Find("Background");

        //전직 미리보기 버튼
        m_buttonPreview = m_trBack.Find("ButtonPreview").GetComponent<Button>();
        m_buttonPreview.onClick.RemoveAllListeners();
        m_buttonPreview.onClick.AddListener(OnClickPreview);
        m_buttonPreview.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        UpdateButtonPreview();

        Text textPreview = m_buttonPreview.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPreview);
        textPreview.text = CsConfiguration.Instance.GetString("A14_BTN_00006");

        //현재레벨
        Text textNowLevel = m_trBack.Find("TextNowState").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNowLevel);
        textNowLevel.text = CsConfiguration.Instance.GetString("A14_TXT_00001");

        //다음레벨
        Text textNextLevel = m_trBack.Find("TextNextState").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNextLevel);
        textNextLevel.text = CsConfiguration.Instance.GetString("A14_TXT_00002");

        //레벨업 필요재료
        Text textNeedLevelup = m_trBack.Find("TextLevelNeed").GetComponent<Text>();
        CsUIData.Instance.SetFont(textNeedLevelup);
        textNeedLevelup.text = CsConfiguration.Instance.GetString("A14_TXT_00003");

        //레벨업 버튼
        m_buttonLevelUp = m_trBack.Find("ButtonLevelUp").GetComponent<Button>();
        m_buttonLevelUp.onClick.RemoveAllListeners();
        m_buttonLevelUp.onClick.AddListener(OnClickSkillLevelUp);
        m_buttonLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textLevelUp = m_buttonLevelUp.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLevelUp);
        textLevelUp.text = CsConfiguration.Instance.GetString("A14_BTN_00001");

        //전체레벨업 버튼
        m_buttonAllLevelUp = m_trBack.Find("ButtonAllLevelUp").GetComponent<Button>();
        m_buttonAllLevelUp.onClick.RemoveAllListeners();
        m_buttonAllLevelUp.onClick.AddListener(OnClickSkillLevelUpTotally);
        m_buttonAllLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textLevelUpAll = m_buttonAllLevelUp.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLevelUpAll);
        textLevelUpAll.text = CsConfiguration.Instance.GetString("A14_BTN_00002");

        //스킬북 구매 버튼
        m_buttonBuySkillBook = m_trBack.Find("ButtonBuySkillBook").GetComponent<Button>();
        m_buttonBuySkillBook.onClick.RemoveAllListeners();
        m_buttonBuySkillBook.onClick.AddListener(OnClickOpenCalculator);
        m_buttonBuySkillBook.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textBuySkillBook = m_buttonBuySkillBook.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textBuySkillBook);
        textBuySkillBook.text = CsConfiguration.Instance.GetString("A14_BTN_00003");

        m_textLevel = m_trBack.Find("TextLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevel);

        m_textName = m_trBack.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textName);

        m_textDesc = m_trBack.Find("TextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textDesc);

        m_textCoolTime = m_trBack.Find("TextCoolTime").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCoolTime);

        m_textNowLevelDesc = m_trBack.Find("TextNowDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNowLevelDesc);

        m_textNextLevelDesc = m_trBack.Find("TextNextDesc").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNextLevelDesc);

        m_trLevelUpResource = m_trBack.Find("LevelUpResource");

        m_textLevelUpGold = m_trLevelUpResource.Find("TextNeedGold").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevelUpGold);

        m_textLevelUpSkillBookName = m_trLevelUpResource.Find("TextNeedSkillBook").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevelUpSkillBookName);

        m_textLevelUpSkillBookCount = m_textLevelUpSkillBookName.transform.Find("TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLevelUpSkillBookCount);

        m_textNeedLevel = m_trBack.Find("TextNeedLevel").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textNeedLevel);

        //디폴트 1번스킬 디스플레이
        int nSkillId = 1;

        for (int i = 0; i < CsGameData.Instance.MyHeroInfo.HeroSkillList.Count; i++)
        {
            CsHeroSkill csHeroSkill = CsGameData.Instance.MyHeroInfo.HeroSkillList[i];
            
            if (csHeroSkill == null)
            {
                continue;
            }
            else
            {
                if (csHeroSkill.IsLevelUp)
                {
                    nSkillId = csHeroSkill.JobSkill.SkillId;
                    break;
                }
            }
        }

        m_csHeroSkill = CsGameData.Instance.MyHeroInfo.GetHeroSkill(nSkillId);
        m_csHonorShopProduct = CsGameData.Instance.GetHonorShopProduct(nSkillId);

        UpdateSkillInfo();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateButtonPreview()
    {
        bool bInteractable = false;

        if (CsGameData.Instance.MyHeroInfo.Job.ParentJobId == 0 || CsGameData.Instance.MyHeroInfo.JobId == CsGameData.Instance.MyHeroInfo.Job.ParentJobId)
        {
            bInteractable = true;
        }
        else
        {
            bInteractable = false;
        }

        m_buttonPreview.interactable = bInteractable;
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateSkillInfo()
    {
        //아이콘설정
        Image imageSkillIcon = m_trBack.Find("ImageSkillIcon").GetComponent<Image>();
        imageSkillIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Skills/skill_" + CsGameData.Instance.MyHeroInfo.Job.JobId + "_" + m_csHeroSkill.JobSkill.SkillId);

        Transform trSkillIconFrame = m_trBack.Find("ImageFrame");

        if (m_csHeroSkill.JobSkill.SkillId == 1)
        {
            trSkillIconFrame.gameObject.SetActive(false);
        }
        else
        {
            trSkillIconFrame.gameObject.SetActive(true);
        }

        m_textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), m_csHeroSkill.JobSkillLevel.Level);
        m_textName.text = m_csHeroSkill.JobSkill.Name;
        m_textDesc.text = m_csHeroSkill.JobSkill.Description;
        m_textCoolTime.text = string.Format(CsConfiguration.Instance.GetString("A14_TXT_01002"), m_csHeroSkill.JobSkill.CoolTime);
        m_textNowLevelDesc.text = m_csHeroSkill.Summary;

        //다음레벨효과 텍스트
        Text textNextLevel = m_trBack.Find("TextNextState").GetComponent<Text>();

        if (m_csHeroSkill.JobSkillMaster.IsMaxLevel(m_csHeroSkill.JobSkillLevel.Level))
        {
            //만렙이면

            //버튼들 끄기
            m_buttonLevelUp.gameObject.SetActive(false);
            m_buttonAllLevelUp.gameObject.SetActive(false);
            m_buttonBuySkillBook.gameObject.SetActive(false);

            //다음레벨 효과 텍스트
            textNextLevel.gameObject.SetActive(false);
            m_textNextLevelDesc.text = "";

            //다음레벨 재료
            m_trLevelUpResource.gameObject.SetActive(false);
            m_textNeedLevel.text = CsConfiguration.Instance.GetString("A14_TXT_00007");
            m_textNeedLevel.color = CsUIData.Instance.ColorGray;
        }
        else
        {
            //다음레벨 효과 텍스트
            textNextLevel.gameObject.SetActive(true);

            CsJobSkillLevel csJobSkillLevel = m_csHeroSkill.JobSkill.GetJobSkillLevel(m_csHeroSkill.JobSkillLevel.Level + 1);
            m_textNextLevelDesc.text = csJobSkillLevel.Summary;

            CsJobSkillLevelMaster csJobSkillLevelMaster = m_csHeroSkill.JobSkillMaster.GetJobSkillLevelMaster(m_csHeroSkill.JobSkillLevel.Level);

            m_buttonLevelUp.gameObject.SetActive(true);
            m_buttonAllLevelUp.gameObject.SetActive(true);

            //레벨업이 가능여부에 따라 버튼 텍스트 색 변경 및 버튼 활성화 처리
            if (m_csHeroSkill.IsLevelUp)
            {
                //레벨업 가능
                CsUIData.Instance.DisplayButtonInteractable(m_buttonLevelUp, true);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonAllLevelUp, true);

                m_buttonBuySkillBook.gameObject.SetActive(false);
            }
            else
            {
                //불가
                CsUIData.Instance.DisplayButtonInteractable(m_buttonLevelUp, false);
                CsUIData.Instance.DisplayButtonInteractable(m_buttonAllLevelUp, false);

                //상점버튼(돈/레벨이 충분하고 스킬북만없을때 스킬북을 살 수 있으면)
                if (csJobSkillLevelMaster.NextLevelUpGold <= CsGameData.Instance.MyHeroInfo.Gold)
                {
                    if (csJobSkillLevelMaster.NextLevelUpItem != null && csJobSkillLevelMaster.NextLevelUpItemCount > CsGameData.Instance.MyHeroInfo.GetItemCount(csJobSkillLevelMaster.NextLevelUpItem.ItemId))
                    {
                        m_buttonBuySkillBook.gameObject.SetActive(true);
                    }
                    else
                    {
                        m_buttonBuySkillBook.gameObject.SetActive(false);
                    }
                }
                else
                {
                    m_buttonBuySkillBook.gameObject.SetActive(false);
                }
            }

            if (CsMainQuestManager.Instance.MainQuest != null && m_csHeroSkill.JobSkillMaster.OpenRequiredMainQuestNo >= CsMainQuestManager.Instance.MainQuest.MainQuestNo)
            {
                //스킬미개방
                m_trLevelUpResource.gameObject.SetActive(false);

                m_textNeedLevel.text = CsConfiguration.Instance.GetString("A14_TXT_00008");
                m_textNeedLevel.color = CsUIData.Instance.ColorRed;
                return;
            }

            //제한레벨체크
            if (csJobSkillLevelMaster.NextLevelUpRequiredHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                m_textNeedLevel.text = "";

                //다음레벨 재료
                m_trLevelUpResource.gameObject.SetActive(true);

                //골드
                m_textLevelUpGold.text = csJobSkillLevelMaster.NextLevelUpGold.ToString();

                if (CsGameData.Instance.MyHeroInfo.Gold >= csJobSkillLevelMaster.NextLevelUpGold)
                {
                    //골드 수량 충분
                    m_textLevelUpGold.color = CsUIData.Instance.ColorWhite;
                }
                else
                {
                    //부족
                    m_textLevelUpGold.color = CsUIData.Instance.ColorRed;
                }

                //강화재료
                if (csJobSkillLevelMaster.NextLevelUpItem == null || csJobSkillLevelMaster.NextLevelUpItemCount == 0)
                {
                    m_textLevelUpSkillBookName.gameObject.SetActive(false);
                }
                else
                {
                    m_textLevelUpSkillBookName.gameObject.SetActive(true);

                    //아이템이름
                    m_textLevelUpSkillBookName.text = csJobSkillLevelMaster.NextLevelUpItem.Name;

                    //수량
                    int nItemCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csJobSkillLevelMaster.NextLevelUpItem.ItemId);

                    m_textLevelUpSkillBookCount.text = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nItemCount, csJobSkillLevelMaster.NextLevelUpItemCount);

                    if (nItemCount >= csJobSkillLevelMaster.NextLevelUpItemCount)
                    {
                        //아이템 수량 충분
                        m_textLevelUpSkillBookCount.color = CsUIData.Instance.ColorWhite;
                    }
                    else
                    {
                        //아이템부족
                        m_textLevelUpSkillBookCount.color = CsUIData.Instance.ColorRed;
                    }
                }
            }
            else
            {
                m_trLevelUpResource.gameObject.SetActive(false);

                m_textNeedLevel.text = string.Format(CsConfiguration.Instance.GetString("A14_TXT_01001"), csJobSkillLevelMaster.NextLevelUpRequiredHeroLevel);
                m_textNeedLevel.color = CsUIData.Instance.ColorRed;
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupSkillBookShop()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/PopupCalculator/PopupCalculator");
        yield return resourceRequest;
        m_goPopupCalculator = (GameObject)resourceRequest.asset;

        OpenPopupBuyItem();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupBuyItem()
    {
        Transform trPopup = m_trPopupList.Find("PopupCalculator");

        if (trPopup == null)
        {
            GameObject goPopupBuyCount = Instantiate(m_goPopupCalculator, m_trPopupList);
            goPopupBuyCount.name = "PopupCalculator";
            trPopup = goPopupBuyCount.transform;
        }
        else
        {
            trPopup.gameObject.SetActive(false);
        }

        m_csPopupCalculator = trPopup.GetComponent<CsPopupCalculator>();
        m_csPopupCalculator.EventBuyItem += OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator += OnEventCloseCalculator;

        CsJobSkillLevelMaster csJobSkillLevelMaster = m_csHeroSkill.JobSkillMaster.GetJobSkillLevelMaster(m_csHeroSkill.JobSkillLevel.Level);
        
        if (csJobSkillLevelMaster != null)
        {
            if (csJobSkillLevelMaster.NextLevelUpItem != null)
            {
                m_csPopupCalculator.DisplayItem(csJobSkillLevelMaster.NextLevelUpItem, false, m_csHonorShopProduct.RequiredHonorPoint, EnResourceType.Honor);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventBuyItem(int nCount)
    {
        CsCommandEventManager.Instance.SendHonorShopProductBuy(m_csHonorShopProduct.ProductId, nCount);
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseCalculator()
    {
        m_csPopupCalculator.EventBuyItem -= OnEventBuyItem;
        m_csPopupCalculator.EventCloseCalculator -= OnEventCloseCalculator;

        Transform trPopupCalculator = m_trPopupList.Find("PopupCalculator");

        if (trPopupCalculator != null)
        {
            Destroy(trPopupCalculator.gameObject);
        }
    }

    #region PopupJobChange

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadPopupJobChange()
    {
        ResourceRequest resourceRequest = CsUIData.Instance.LoadAssetAsync<GameObject>("GUI/MainUI/PopupJobChange");
        yield return resourceRequest;
        m_goPopupJobChange = (GameObject)resourceRequest.asset;

        OpenPopupJobChange();
    }

    //---------------------------------------------------------------------------------------------------
    void OpenPopupJobChange()
    {
        Transform trPopupJobChange = Instantiate(m_goPopupJobChange, m_trPopupList).transform;
        trPopupJobChange.name = "PopupJobChange";
    }

    #endregion PopupJobChange
}
