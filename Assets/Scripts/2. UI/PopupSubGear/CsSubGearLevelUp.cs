using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-07)
//---------------------------------------------------------------------------------------------------

public class CsSubGearLevelUp : CsPopupSub
{
    Transform m_trSubGearSlot;
    Transform m_trSloMaterial1;
    Transform m_trSloMaterial2;
    Transform m_trImagePlus;
    Transform[] m_atrAttribute = new Transform[3];
    Transform m_trEffect;
	Transform m_trQuality;

    Button m_buttonLevelUp;
    Button m_buttonLevelUpAll;
    Button m_buttonGradeUp;
    Button m_buttonQualityUp;
    Button m_buttonShortCut;

    Text m_textNoLevel;
    Text m_textMaxLevel;
	Text m_textQualityCount;

	Image m_imageQualityProgress;

    bool m_bNoSubGear = false;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSubGearSelected += OnEventSubGearSelected;
        CsGameEventUIToUI.Instance.EventSubGearLevelUp += OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally += OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp += OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp += OnEventSubGearQualityUp;
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventSubGearSelected -= OnEventSubGearSelected;
        CsGameEventUIToUI.Instance.EventSubGearLevelUp -= OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally -= OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp -= OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp -= OnEventSubGearQualityUp;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bNoSubGear)
        {
            gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDisable()
    {
        if (m_trEffect != null)
        {
            m_trEffect.gameObject.SetActive(false);
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnClickLevelUp()
    {
        //플레이어의 보조 장비 가져오기
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        //다음레벨 필요 골드랑 비교
        if (CsGameData.Instance.MyHeroInfo.Gold >= csHeroSubGear.SubGearLevel.NextLevelUpRequiredGold
            && CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            CsCommandEventManager.Instance.SendSubGearLevelUp(CsUIData.Instance.SubGearId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickLevelUpTotally()
    {
        //플레이어의 보조 장비 가져오기
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        //다음레벨 필요 골드랑 비교
        if (CsGameData.Instance.MyHeroInfo.Gold >= csHeroSubGear.SubGearLevel.NextLevelUpRequiredGold
            && CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            CsCommandEventManager.Instance.SendSubGearLevelUpTotally(CsUIData.Instance.SubGearId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickQualityUp()
    {
        //플레이어의 보조 장비 가져오기
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        //플레이어가 가지고 있는 재료 수량
        int nHeroItemCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality).NextQualityUpItem1.ItemId);
        int nHeroItemCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality).NextQualityUpItem2.ItemId);

        //승급업 재료 수량
        int nItemCount1 = csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality).NextQualityUpItem1Count;
        int nItemCount2 = csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality).NextQualityUpItem2Count;

        //다음 승품 재료 비교
        if (nHeroItemCount1 >= nItemCount1 && nHeroItemCount2 >= nItemCount2
             && CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            CsCommandEventManager.Instance.SendSubGearQualityUp(CsUIData.Instance.SubGearId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickGradeUp()
    {
        //플레이어의 보조 장비 가져오기
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(CsUIData.Instance.SubGearId);

        //플레이어가 가지고 있는 재료 수량
        int nHeroItemCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csHeroSubGear.SubGearLevel.NextGradeUpItem1.ItemId);
        int nHeroItemCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csHeroSubGear.SubGearLevel.NextGradeUpItem1.ItemId);

        //승급업 재료 수량
        int nItemCount1 = csHeroSubGear.SubGearLevel.NextGradeUpItem1Count;
        int nItemCount2 = csHeroSubGear.SubGearLevel.NextGradeUpItem2Count;

        //다음 승급 재료 비교
        if (nHeroItemCount1 >= nItemCount1 && nHeroItemCount2 >= nItemCount2
            && CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            CsCommandEventManager.Instance.SendSubGearGradeUp(CsUIData.Instance.SubGearId);
        }
    }
    //---------------------------------------------------------------------------------------------------
    void OnClickDungeonMove()
    {
        CsGameEventUIToUI.Instance.OnEventPopupOpen(EnMainMenu.Dungeon, EnSubMenu.StoryDungeon);
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearSelected(int nSubGearId)
    {
        DisplayUpdate(CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUp(int nSubGearId)
    {
        UpdateLeveupEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUpTotally(int nSubGearId)
    {
        UpdateLeveupEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearGradeUp(int nSubGearId)
    {
        UpdateLeveupEffect();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearQualityUp(int nSubGearId)
    {
        UpdateLeveupEffect();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        //레벨업 버튼
        m_buttonLevelUp = transform.Find("ButtonLevelUp").GetComponent<Button>();
        m_buttonLevelUp.onClick.RemoveAllListeners();
        m_buttonLevelUp.onClick.AddListener(OnClickLevelUp);
        m_buttonLevelUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonLevelUp = m_buttonLevelUp.transform.Find("Text").GetComponent<Text>();
        textButtonLevelUp.text = CsConfiguration.Instance.GetString("A09_BTN_00001");
        CsUIData.Instance.SetFont(textButtonLevelUp);

        //전체레벨업 버튼
        m_buttonLevelUpAll = transform.Find("ButtonLevelUpAll").GetComponent<Button>();
        m_buttonLevelUpAll.onClick.RemoveAllListeners();
        m_buttonLevelUpAll.onClick.AddListener(OnClickLevelUpTotally);
        m_buttonLevelUpAll.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textButtonLevelUpAll = m_buttonLevelUpAll.transform.Find("Text").GetComponent<Text>();
        textButtonLevelUpAll.text = CsConfiguration.Instance.GetString("A09_BTN_00002");
        CsUIData.Instance.SetFont(textButtonLevelUpAll);

        //승급 버튼
        m_buttonGradeUp = transform.Find("ButtonGradeUp").GetComponent<Button>();
        m_buttonGradeUp.onClick.RemoveAllListeners();
        m_buttonGradeUp.onClick.AddListener(OnClickGradeUp);
        m_buttonGradeUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textButtonGradeUp = m_buttonGradeUp.transform.Find("Text").GetComponent<Text>();
        textButtonGradeUp.text = CsConfiguration.Instance.GetString("A09_BTN_00003");
        CsUIData.Instance.SetFont(textButtonGradeUp);

        //승품 버튼
        m_buttonQualityUp = transform.Find("ButtonQualityUp").GetComponent<Button>();
        m_buttonQualityUp.onClick.RemoveAllListeners();
        m_buttonQualityUp.onClick.AddListener(OnClickQualityUp);
        m_buttonQualityUp.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textButtonQualityUp = m_buttonQualityUp.transform.Find("Text").GetComponent<Text>();
        textButtonQualityUp.text = CsConfiguration.Instance.GetString("A09_BTN_00004");
        CsUIData.Instance.SetFont(textButtonQualityUp);

        //획득 이동 버튼
        m_buttonShortCut = transform.Find("ButtonShortCut").GetComponent<Button>();
        m_buttonShortCut.onClick.RemoveAllListeners();
        m_buttonShortCut.onClick.AddListener(OnClickDungeonMove);
        m_buttonShortCut.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        Text textButtonShortCut = m_buttonShortCut.transform.Find("Text").GetComponent<Text>();
        textButtonShortCut.text = CsConfiguration.Instance.GetString("A09_BTN_00005");
        CsUIData.Instance.SetFont(textButtonShortCut);

        //레벨 제한 텍스트
        m_textNoLevel = transform.Find("TextNoLevel").GetComponent<Text>();
        m_textNoLevel.text = CsConfiguration.Instance.GetString("A09_TXT_00005");
        CsUIData.Instance.SetFont(m_textNoLevel);

        //최대 레벨 텍스트
        m_textMaxLevel = transform.Find("TextMaxLevel").GetComponent<Text>();
        m_textMaxLevel.text = CsConfiguration.Instance.GetString("A09_TXT_00004");
        CsUIData.Instance.SetFont(m_textMaxLevel);

        //속성 리스트
        Transform trAttrList = transform.Find("NextAttrList");

        for (int i = 0; i < 3; ++i)
        {
            m_atrAttribute[i] = trAttrList.Find("LevelUpAttribute" + i);
            m_atrAttribute[i].gameObject.SetActive(false);
        }

        Transform trItemGrid1 = transform.Find("ItemGrid");
        Transform trItemGrid2 = trItemGrid1.Find("ItemGrid2");
        m_trSubGearSlot = trItemGrid2.Find("ItemSlotMain");
        m_trSloMaterial1 = trItemGrid2.Find("ItemSlotMaterial1");
        m_trImagePlus = trItemGrid2.Find("ImagePlus");
        m_trSloMaterial2 = trItemGrid1.Find("ItemSlotMaterial2");

        m_trEffect = m_trSubGearSlot.Find("SubGear_Levelup_Result");

		m_trQuality = transform.Find("Quality");

		CsUIData.Instance.SetText(m_trQuality.Find("TextQuality"), CsConfiguration.Instance.GetString("A09_TXT_00006"), true);

		m_imageQualityProgress = m_trQuality.Find("ImageProgressBack/ImageProgress").GetComponent<Image>();

		m_textQualityCount = m_trQuality.Find("TextQualityCount").GetComponent<Text>();
		CsUIData.Instance.SetFont(m_textQualityCount);

        FirstSubGearCheck();

    }

    //---------------------------------------------------------------------------------------------------
    void FirstSubGearCheck()
    {
        bool bSubGearCheck = false;
        bool bFirstSubGearCheck = false;

        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;

        for (int i = 0; i < listHeroSubGear.Count; ++i)
        {
            //장착 여부 확인
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }

            bSubGearCheck = true;

            if (CheckNextSubGear(listHeroSubGear[i].SubGear.SubGearId))
            {
                DisplayUpdate(listHeroSubGear[i]);
                bFirstSubGearCheck = true;
                break;
            }
        }

        if (bSubGearCheck && !bFirstSubGearCheck)
        {
            for (int i = 0; i < listHeroSubGear.Count; ++i)
            {
                //장착 여부 확인
                if (!listHeroSubGear[i].Equipped)
                {
                    continue;
                }

                DisplayUpdate(listHeroSubGear[i]);
                break;
            }
        }
        else if (!bSubGearCheck)
        {
            m_bNoSubGear = true;
            gameObject.SetActive(false);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayUpdate(CsHeroSubGear csHeroSubGear)
    {
		m_trQuality.gameObject.SetActive(false);

        //버튼 초기화
        m_buttonLevelUp.gameObject.SetActive(false);
        m_buttonLevelUpAll.gameObject.SetActive(false);
        m_buttonQualityUp.gameObject.SetActive(false);
        m_buttonGradeUp.gameObject.SetActive(false);
        m_buttonShortCut.gameObject.SetActive(false);

        //슬롯 초기화
        m_trSloMaterial1.gameObject.SetActive(false);
        m_trSloMaterial2.gameObject.SetActive(false);
        m_trImagePlus.gameObject.SetActive(false);

        m_textNoLevel.gameObject.SetActive(false);


        //보조 장비 이미지 셋팅
        CsUIData.Instance.DisplayItemSlot(m_trSubGearSlot, csHeroSubGear, EnItemSlotSize.Large);
        Text textSubGearName = m_trSubGearSlot.Find("TextName").GetComponent<Text>();
        textSubGearName.text = csHeroSubGear.Name;
        CsUIData.Instance.SetFont(textSubGearName);
        EnNextStep enNextStep = csHeroSubGear.NextStep;

        switch (enNextStep)
        {
            case EnNextStep.Quality:
                DisplayQuailtyUp(csHeroSubGear);
                break;
            case EnNextStep.Level:
                DisplayLevelUp(csHeroSubGear);
                break;
            case EnNextStep.Grade:
                DisplayGreadUp(csHeroSubGear);
                break;
        }

        DisplayLevel(csHeroSubGear, enNextStep);
        DisplayAttr(csHeroSubGear, enNextStep);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLevelUp(CsHeroSubGear csHeroSubGear)
    {
        bool bLevelUpCheck = false;
        //사용하지 않는 이미지들 비활성화
        m_trSloMaterial1.Find("Gear").gameObject.SetActive(false);
        m_trSloMaterial1.Find("Item").gameObject.SetActive(false);
        m_trSloMaterial1.Find("ImageOwn").gameObject.SetActive(false);

        //아이템아이콘
        Image imageItemIcon = m_trSloMaterial1.Find("ImageIcon").GetComponent<Image>();
        imageItemIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_goods03");

        //랭크테두리
        Image imageFrameRank = m_trSloMaterial1.Find("ImageFrameRank").GetComponent<Image>();
        imageFrameRank.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank01");

        //골드 표시
        Text textSlotMaterial1 = m_trSloMaterial1.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textSlotMaterial1);
        CsSubGearLevel csSubGearLevel = csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level);

        //골드 조건O : 하얀색, 조건X : 빨강색
        if (CsGameData.Instance.MyHeroInfo.Gold >= csHeroSubGear.SubGearLevel.NextLevelUpRequiredGold)
        {
            textSlotMaterial1.color = CsUIData.Instance.ColorWhite;
            textSlotMaterial1.text = csSubGearLevel.NextLevelUpRequiredGold.ToString("#,##0");
            if (CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
            {
                bLevelUpCheck = true;
            }
            else
            {
                m_textNoLevel.gameObject.SetActive(true);
            }
        }
        else
        {
            textSlotMaterial1.color = CsUIData.Instance.ColorRed;
            textSlotMaterial1.text = csSubGearLevel.NextLevelUpRequiredGold.ToString("#,##0");
        }

        //조건에 따라 활성/비활성
        CsUIData.Instance.DisplayButtonInteractable(m_buttonLevelUp, bLevelUpCheck);
        CsUIData.Instance.DisplayButtonInteractable(m_buttonLevelUpAll, bLevelUpCheck);


        //슬롯 활성화
        m_trSloMaterial1.gameObject.SetActive(true);
        m_trSloMaterial1.Find("ImageCooltime").gameObject.SetActive(false);
        m_trImagePlus.gameObject.SetActive(true);

        //버튼 활성화
        m_buttonLevelUp.gameObject.SetActive(true);
        m_buttonLevelUpAll.gameObject.SetActive(true);

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayGreadUp(CsHeroSubGear csHeroSubGear)
    {
        CsSubGearLevel csSubGearLevel = csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level);

        //아이템 표시 셋팅
        CsUIData.Instance.DisplayItemSlot(m_trSloMaterial1, csSubGearLevel.NextGradeUpItem1, false, csSubGearLevel.NextGradeUpItem1Count, true, EnItemSlotSize.Large);
        CsUIData.Instance.DisplayItemSlot(m_trSloMaterial2, csSubGearLevel.NextGradeUpItem2, false, csSubGearLevel.NextGradeUpItem2Count, true, EnItemSlotSize.Large);

        //현재 보유량
        int nHeroItemCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevel.NextGradeUpItem1.ItemId);
        int nHeroItemCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevel.NextGradeUpItem2.ItemId);

        //필요량
        int nItemCount1 = csSubGearLevel.NextGradeUpItem1Count;
        int nItemCount2 = csSubGearLevel.NextGradeUpItem2Count;

        //조건 충족 확인
        bool bItemCheck1 = false;
        bool bItemCheck2 = false;
        bool bLevelCheck = false;

        //문자열 {0} / {1}
        string strItemCount1 = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount1, nItemCount1);
        string strItemCount2 = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount2, nItemCount2);

        //승급 재료1 이름 표시
        Text textSlotMaterial1 = m_trSloMaterial1.Find("TextName").GetComponent<Text>();
        textSlotMaterial1.text = csSubGearLevel.NextGradeUpItem1.Name;
        textSlotMaterial1.color = CsUIData.Instance.ColorWhite;
        CsUIData.Instance.SetFont(textSlotMaterial1);

        //승급 재료1 개수 표시
        Text textCount1 = m_trSloMaterial1.Find("Item/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount1);

        //DIM 이미지1
        Image imageDim1 = m_trSloMaterial1.Find("ImageCooltime").GetComponent<Image>();

        //승급 재료1 조건O : 하얀색, 조건X : 빨강색
        if (nHeroItemCount1 >= nItemCount1)
        {
            textCount1.color = CsUIData.Instance.ColorWhite;
            textCount1.text = strItemCount1;
            bItemCheck1 = true;
            imageDim1.gameObject.SetActive(false);
        }
        else
        {
            textCount1.color = CsUIData.Instance.ColorRed;
            textCount1.text = strItemCount1;
            imageDim1.gameObject.SetActive(true);
            imageDim1.fillAmount = 1f;
        }

        //승급 재료2 이름 표시
        Text textSlotMaterial2 = m_trSloMaterial2.Find("TextName").GetComponent<Text>();
        textSlotMaterial2.text = csSubGearLevel.NextGradeUpItem2.Name;
        CsUIData.Instance.SetFont(textSlotMaterial2);

        //승급 재료2 개수 표시
        Text textCount2 = m_trSloMaterial2.Find("Item/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount2);

        //DIM 이미지2
        Image imageDim2 = m_trSloMaterial2.Find("ImageCooltime").GetComponent<Image>();

        //승급 재료2 조건O : 하얀색, 조건X : 빨강색
        if (nHeroItemCount2 >= nItemCount2)
        {
            textCount2.color = CsUIData.Instance.ColorWhite;
            textCount2.text = strItemCount2;
            bItemCheck2 = true;
            imageDim2.gameObject.SetActive(false);
        }
        else
        {
            textCount2.color = CsUIData.Instance.ColorRed;
            textCount2.text = strItemCount2;
            imageDim2.gameObject.SetActive(true);
            imageDim2.fillAmount = 1f;
        }

        //레벨 체크
        if (CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            bLevelCheck = true;
        }
        else
        {
            m_textNoLevel.gameObject.SetActive(true);
        }

        //이미지 활성화
        m_trSloMaterial1.gameObject.SetActive(true);
        m_trSloMaterial2.gameObject.SetActive(true);
        m_trImagePlus.gameObject.SetActive(true);

        //버튼 활성화
        if (bItemCheck1 && bItemCheck2 && bLevelCheck)
        {
            m_buttonGradeUp.gameObject.SetActive(true);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
            {
                m_buttonShortCut.interactable = false;
            }
            else if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
            {
                m_buttonShortCut.interactable = true;
            }
            else
            {
                m_buttonShortCut.interactable = false;
            }

            m_buttonShortCut.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayQuailtyUp(CsHeroSubGear csHeroSubGear)
    {
        CsSubGearLevelQuality csSubGearLevelQuality = csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level).GetSubGearLevelQuality(csHeroSubGear.Quality);

        //아이템 표시 셋팅
        CsUIData.Instance.DisplayItemSlot(m_trSloMaterial1, csSubGearLevelQuality.NextQualityUpItem1, false, csSubGearLevelQuality.NextQualityUpItem1Count, true, EnItemSlotSize.Large);
        CsUIData.Instance.DisplayItemSlot(m_trSloMaterial2, csSubGearLevelQuality.NextQualityUpItem2, false, csSubGearLevelQuality.NextQualityUpItem2Count, true, EnItemSlotSize.Large);

        //현재 보유량
        int nHeroItemCount1 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem1.ItemId);
        int nHeroItemCount2 = CsGameData.Instance.MyHeroInfo.GetItemCount(csSubGearLevelQuality.NextQualityUpItem2.ItemId);

        //필요량
        int nItemCount1 = csSubGearLevelQuality.NextQualityUpItem1Count;
        int nItemCount2 = csSubGearLevelQuality.NextQualityUpItem2Count;

        //조건 충족 확인
        bool bItemCheck1 = false;
        bool bItemCheck2 = false;

        //문자열 {0} / {1}
        string strItemCount1 = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount1, nItemCount1);
        string strItemCount2 = string.Format(CsConfiguration.Instance.GetString("INPUT_SLASH"), nHeroItemCount2, nItemCount2);

        //승품 재료1 이름 표시
        Text textSlotMaterial1 = m_trSloMaterial1.Find("TextName").GetComponent<Text>();
        textSlotMaterial1.text = csSubGearLevelQuality.NextQualityUpItem1.Name;
        textSlotMaterial1.color = CsUIData.Instance.ColorWhite;
        CsUIData.Instance.SetFont(textSlotMaterial1);

        //승품 재료1 개수 표시
        Text textCount1 = m_trSloMaterial1.Find("Item/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount1);

        //DIM 이미지1
        Image imageDim1 = m_trSloMaterial1.Find("ImageCooltime").GetComponent<Image>();

        //승품 재료1 조건O : 하얀색, 조건X : 빨강색
        if (nHeroItemCount1 >= nItemCount1)
        {
            textCount1.color = CsUIData.Instance.ColorWhite;
            textCount1.text = strItemCount1;
            bItemCheck1 = true;
            imageDim1.gameObject.SetActive(false);
        }
        else
        {
            textCount1.color = CsUIData.Instance.ColorRed;
            textCount1.text = strItemCount1;
            imageDim1.gameObject.SetActive(true);
            imageDim1.fillAmount = 1f;
        }

        //승품 재료2 이름 표시
        Text textSlotMaterial2 = m_trSloMaterial2.Find("TextName").GetComponent<Text>();
        textSlotMaterial2.text = csSubGearLevelQuality.NextQualityUpItem2.Name;
        CsUIData.Instance.SetFont(textSlotMaterial2);

        //승품 재료2 개수 표시
        Text textCount2 = m_trSloMaterial2.Find("Item/TextCount").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCount2);

        //DIM 이미지2
        Image imageDim2 = m_trSloMaterial2.Find("ImageCooltime").GetComponent<Image>();

        //승품 재료2 조건O : 하얀색, 조건X : 빨강색
        if (nHeroItemCount2 >= nItemCount2)
        {
            textCount2.color = CsUIData.Instance.ColorWhite;
            textCount2.text = strItemCount2;
            bItemCheck2 = true;
            imageDim2.gameObject.SetActive(false);
        }
        else
        {
            textCount2.color = CsUIData.Instance.ColorRed;
            textCount2.text = strItemCount2;
            imageDim2.gameObject.SetActive(true);
            imageDim2.fillAmount = 1f;
        }

        //이미지 활성화
        m_trSloMaterial1.gameObject.SetActive(true);
        m_trSloMaterial2.gameObject.SetActive(true);
        m_trImagePlus.gameObject.SetActive(true);

        //버튼 활성화
        if (bItemCheck1 && bItemCheck2 && CsGameData.Instance.MyHeroInfo.Level > csHeroSubGear.Level)
        {
            m_buttonQualityUp.gameObject.SetActive(true);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.LocationId == 201)
            {
                m_buttonShortCut.interactable = false;
            }
            else if (CsUIData.Instance.DungeonInNow == EnDungeon.None)
            {
                m_buttonShortCut.interactable = true;
            }
            else
            {
                m_buttonShortCut.interactable = false;
            }

            m_buttonShortCut.gameObject.SetActive(true);
        }

		m_trQuality.gameObject.SetActive(true);

		int nMaxQuality = csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level).SubGearLevelQualityList[csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level).SubGearLevelQualityList.Count - 1].Quality;

		float flQualityFillamount = (csSubGearLevelQuality.Quality == 0) ? 0.0f : ((float)csSubGearLevelQuality.Quality / (float)nMaxQuality);

		m_imageQualityProgress.fillAmount = flQualityFillamount;

		m_textQualityCount.text = string.Format(CsConfiguration.Instance.GetString("A09_TXT_00007"), csSubGearLevelQuality.Quality, nMaxQuality);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayLevel(CsHeroSubGear csHeroSubGear, EnNextStep enNextStep)
    {
        Transform trLevelGrid = transform.Find("LevelTextGrid");

        //현재 레벨
        Text textLevel = trLevelGrid.Find("TextLevel").GetComponent<Text>();
        textLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL"), csHeroSubGear.Level.ToString());
        CsUIData.Instance.SetFont(textLevel);

        //다음 레벨
        Text textNextLevel = trLevelGrid.Find("TextNextLevel").GetComponent<Text>();
        Transform trArrow = trLevelGrid.Find("ImageArrow");

        //최대레벨일경우 현재레벨만 표시
        if (enNextStep == EnNextStep.MaxLevel)
        {
            trArrow.gameObject.SetActive(false);
            textNextLevel.gameObject.SetActive(false);
        }
        else
        {
            textNextLevel.text = string.Format(CsConfiguration.Instance.GetString("INPUT_LEVEL_BLUE"), csHeroSubGear.Level + 1);
            CsUIData.Instance.SetFont(textNextLevel);
            trArrow.gameObject.SetActive(true);
            textNextLevel.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayAttr(CsHeroSubGear csHeroSubGear, EnNextStep enNextStep)
    {
        //속성 초기화
        for (int i = 0; i < 3; ++i)
        {
            m_atrAttribute[i].gameObject.SetActive(false);
        }

        //최대 레벨일 경우 '최대 레벨입니다' 문구 표시
        if (enNextStep == EnNextStep.MaxLevel)
        {
            m_textMaxLevel.gameObject.SetActive(true);
            return;
        }
        else
        {
            m_textMaxLevel.gameObject.SetActive(false);
        }

        for (int i = 0; i < csHeroSubGear.AttrValueList.Count; ++i)
        {
            //설정 값
            CsSubGearAttr csSubGearAttr = csHeroSubGear.SubGear.GetSubGearAttr(csHeroSubGear.AttrValueList[i].Attr.AttrId);
            int nLevel = csHeroSubGear.Level;
            int nQuality = csHeroSubGear.Quality;
            int nValue = csHeroSubGear.AttrValueList[i].Value;
            int nNextValue;

            switch (enNextStep)
            {
                case EnNextStep.Quality:
                    nNextValue = csSubGearAttr.GetSubGearAttrValue(nLevel, nQuality + 1).Value;
                    break;
                case EnNextStep.Level:
                    nNextValue = csSubGearAttr.GetSubGearAttrValue(nLevel + 1, 0).Value;
                    break;
                case EnNextStep.Grade:
                    nNextValue = csSubGearAttr.GetSubGearAttrValue(nLevel + 1, 0).Value;
                    break;
                default:
                    nNextValue = 0;
                    break;
            }

            //속성 이름
            Text textAttributeName = m_atrAttribute[i].Find("TextName").GetComponent<Text>();
            textAttributeName.text = csHeroSubGear.AttrValueList[i].Attr.Name;
            CsUIData.Instance.SetFont(textAttributeName);

            //속성 현재 값
            Text textAttributeValue = m_atrAttribute[i].Find("TextValue").GetComponent<Text>();
            textAttributeValue.text = csHeroSubGear.AttrValueList[i].Value.ToString("#.###");
            CsUIData.Instance.SetFont(textAttributeName);

            //속성 증가 값
            Text textAttributeNextValue = m_atrAttribute[i].Find("TextChangeValue").GetComponent<Text>();

            if ((nNextValue - nValue) > 0)
            {
                textAttributeNextValue.text = (nNextValue - nValue).ToString("#.###");
                CsUIData.Instance.SetFont(textAttributeName);
                textAttributeNextValue.gameObject.SetActive(true);
            }
            else
            {
                textAttributeNextValue.gameObject.SetActive(false);
            }

            m_atrAttribute[i].gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateLeveupEffect()
    {
        if (m_trEffect.gameObject.activeSelf)
        {
            m_trEffect.gameObject.SetActive(false);
            m_trEffect.gameObject.SetActive(true);
        }
        else
        {
            m_trEffect.gameObject.SetActive(true);
        }

        CsUIData.Instance.PlayUISound(EnUISoundType.SubgearUpgrade);
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckNextSubGear(int nSubGearId)
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);

        if (csHeroSubGear != null)
        {
            int nNextItemId1 = 0;
            int nNextItemCount1 = 0;
            int nNextItemId2 = 0;
            int nNextItemCount2 = 0;

            if (CsGameData.Instance.MyHeroInfo.Level <= csHeroSubGear.SubGearLevel.Level)
                return false;

            switch (csHeroSubGear.NextStep)
            {
                case EnNextStep.MaxLevel:
                    return false;
                case EnNextStep.Quality:
                    CsSubGearLevelQuality csLevelQuality = csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality);
                    nNextItemId1 = csLevelQuality.NextQualityUpItem1.ItemId;
                    nNextItemCount1 = csLevelQuality.NextQualityUpItem1Count;
                    nNextItemId2 = csLevelQuality.NextQualityUpItem2.ItemId;
                    nNextItemCount2 = csLevelQuality.NextQualityUpItem2Count;

                    if (CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId1) >= nNextItemCount1 && CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId2) >= nNextItemCount2)
                        return true;
                    else
                        return false;
                case EnNextStep.Level:
                    if (CsGameData.Instance.MyHeroInfo.Gold >= csHeroSubGear.SubGearLevel.NextLevelUpRequiredGold)
                        return true;
                    else
                        return false;
                case EnNextStep.Grade:
                    CsSubGearLevel csSubGearLevel = csHeroSubGear.SubGear.GetSubGearLevel(csHeroSubGear.Level);
                    nNextItemId1 = csSubGearLevel.NextGradeUpItem1.ItemId;
                    nNextItemCount1 = csSubGearLevel.NextGradeUpItem1Count;
                    nNextItemId2 = csSubGearLevel.NextGradeUpItem2.ItemId;
                    nNextItemCount2 = csSubGearLevel.NextGradeUpItem2Count;

                    if (CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId1) >= nNextItemCount1 && CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId2) >= nNextItemCount2)
                        return true;
                    else
                        return false;
                default:
                    return false;
            }
        }
        else
        {
            return false;
        }
    }


}
