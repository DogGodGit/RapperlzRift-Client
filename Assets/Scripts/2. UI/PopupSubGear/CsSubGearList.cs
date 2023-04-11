using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-06)
//---------------------------------------------------------------------------------------------------

public class CsSubGearList : CsPopupSub
{
    [SerializeField] GameObject m_goToggleSubGear;

    Transform m_trContent;

    bool m_bFirst = true;
    bool m_bTabCheck = false;
    float m_flTime = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventSubGearLevelUp += OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally += OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp += OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp += OnEventSubGearQualityUp;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount += OnEventSoulstoneSocketMount;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketUnmount += OnEventSoulstoneSocketUnmount;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose += OnEventMountedSoulstoneCompose;
        CsGameEventUIToUI.Instance.EventRuneSocketMount += OnEventRuneSocketMount;
        CsGameEventUIToUI.Instance.EventRuneSocketUnmount += OnEventRuneSocketUnMount;
        CsGameEventUIToUI.Instance.EventExpPotionUse += OnEventExpPotionUse;
        CsGameEventUIToUI.Instance.EventExpAcquisition += OnEventExpAcquisition;
        CsMainQuestManager.Instance.EventCompleted += OnEventCompleted;
        //던전 퀘스트 완료 추가 예정
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventSubGearLevelUp -= OnEventSubGearLevelUp;
        CsGameEventUIToUI.Instance.EventSubGearLevelUpTotally -= OnEventSubGearLevelUpTotally;
        CsGameEventUIToUI.Instance.EventSubGearGradeUp -= OnEventSubGearGradeUp;
        CsGameEventUIToUI.Instance.EventSubGearQualityUp -= OnEventSubGearQualityUp;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketMount -= OnEventSoulstoneSocketMount;
        CsGameEventUIToUI.Instance.EventSoulstoneSocketUnmount -= OnEventSoulstoneSocketUnmount;
        CsGameEventUIToUI.Instance.EventMountedSoulstoneCompose -= OnEventMountedSoulstoneCompose;
        CsGameEventUIToUI.Instance.EventRuneSocketMount -= OnEventRuneSocketMount;
        CsGameEventUIToUI.Instance.EventRuneSocketUnmount -= OnEventRuneSocketUnMount;
        CsGameEventUIToUI.Instance.EventExpPotionUse -= OnEventExpPotionUse;
        CsGameEventUIToUI.Instance.EventExpAcquisition -= OnEventExpAcquisition;
        CsMainQuestManager.Instance.EventCompleted -= OnEventCompleted;
        CsUIData.Instance.SubGearId = 0;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_bFirst)
        {
            m_bFirst = false;
            return;
        }
        else
        {
            UpdateSubGearList();
            SubGearLeveulUpCheckUpdate();
        }
    }

    void Update()
    {
        // 1초마다 실행.
        if (m_flTime + 1f < Time.time)
        {
            SubGearLeveulUpCheckUpdate();

            m_flTime = Time.time;
        }
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnValueChangedSubGear(bool bIson, int nSubGearId)
    {
        if (bIson)
        {
            if (!m_bTabCheck && CsUIData.Instance.SubGearId == nSubGearId)
            {
                return;
            }
            CsUIData.Instance.PlayUISound(EnUISoundType.ToggleGroup);
            CsUIData.Instance.SubGearId = nSubGearId;
            CsGameEventUIToUI.Instance.OnEventSubGearSelected(nSubGearId);
        }
    }

    #endregion EventHandler

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUp(int nSubGearId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A09_TXT_02001"));
        AllCheck(nSubGearId);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearLevelUpTotally(int nSubGearId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A09_TXT_02002"));
        AllCheck(nSubGearId);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearGradeUp(int nSubGearId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A09_TXT_02003"));
        AllCheck(nSubGearId);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSubGearQualityUp(int nSubGearId)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A09_TXT_02004"));
        AllCheck(nSubGearId);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulstoneSocketMount(int nSubGearId)
    {
        UpdateSubGear(nSubGearId);
        CsUIData.Instance.PlayUISound(EnUISoundType.SubgearSoulstoneEquip);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSoulstoneSocketUnmount(int nSubGearId)
    {
        UpdateSubGear(nSubGearId);
        CsUIData.Instance.PlayUISound(EnUISoundType.SubgearSoulstoneUnEquip);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuneSocketMount(int nSubGearId)
    {
        UpdateSubGear(nSubGearId);
        CsUIData.Instance.PlayUISound(EnUISoundType.SubgearRuneEquip);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRuneSocketUnMount(int nSubGearId)
    {
        UpdateSubGear(nSubGearId);
        CsUIData.Instance.PlayUISound(EnUISoundType.SubgearRuneUnEquip);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMountedSoulstoneCompose(int nSubGearId, int nSocketIndex)
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, CsConfiguration.Instance.GetString("A10_BTN_00001"));
        UpdateSubGear(nSubGearId);
        SubGearLeveulUpCheckUpdate();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpPotionUse(bool bLevelUp, long lAcquiredExp)
    {
        if (bLevelUp)
        {
            UpdateSubGearList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventExpAcquisition(long lExp, bool bLevelUp)
    {
        if (bLevelUp)
        {
            UpdateSubGearList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventCompleted(CsMainQuest csMainQuest, bool bLevelUp, long lAcquiredExp)
    {
        if (bLevelUp)
        {
            UpdateSubGearList();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventUpdateSubGear(int nSubGearID)
    {
        UpdateSubGear(nSubGearID);
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        bool bSubGearCheck = false;

        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;
        m_trContent = transform.Find("ImageBackground/Scroll View/Viewport/Content");

        int nSelectSubGearId = CsGameData.Instance.SubGearList.Count + 1;

        for (int i = 0; i < listHeroSubGear.Count; i++)
        {
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }
            else
            {
                bSubGearCheck = true;
            }
        }

        if (bSubGearCheck)
        {
            for (int i = 0; i < listHeroSubGear.Count; i++)
            {
                if (!listHeroSubGear[i].Equipped)
                {
                    continue;
                }
                else
                {
                    if (CheckNextSubGear(listHeroSubGear[i].SubGear.SubGearId))
                    {
                        nSelectSubGearId = listHeroSubGear[i].SubGear.SubGearId;
                        break;
                    }
                    else
                    {
                        if (nSelectSubGearId > listHeroSubGear[i].SubGear.SubGearId)
                        {
                            nSelectSubGearId = listHeroSubGear[i].SubGear.SubGearId;
                        }
                    }
                }
            }
        }
        else
        {
            //장착된 장비가 없으면 '보조장비 없습니다' 표시
            Text textNoGear = transform.Find("ImageBackground/TextNoGear").GetComponent<Text>();
            textNoGear.text = CsConfiguration.Instance.GetString("A09_TXT_00003");
            CsUIData.Instance.SetFont(textNoGear);
            textNoGear.gameObject.SetActive(true);
        }

        for (int i = 0; i < listHeroSubGear.Count; ++i)
        {
            //장착 여부 확인
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }
            else
            {
                bSubGearCheck = true;
            }

            int nSubGearId = listHeroSubGear[i].SubGear.SubGearId;
            //객체 생성 
            Transform trSubGear = CreateSubGear(listHeroSubGear[i]);

            //토글 설정
            Toggle toggle = trSubGear.GetComponent<Toggle>();
            toggle.group = m_trContent.GetComponent<ToggleGroup>();

            if (nSelectSubGearId == nSubGearId)
            {
                toggle.isOn = true;
                CsUIData.Instance.SubGearId = nSelectSubGearId;
            }

            toggle.onValueChanged.AddListener((ison) => OnValueChangedSubGear(toggle, nSubGearId));
        }
    }

    //보조 장비 화면 Update
    //---------------------------------------------------------------------------------------------------
    void DisplaySubGear(Transform trSubGear, CsHeroSubGear csHeroSubGear)
    {
        Transform trItemSlot = trSubGear.Find("ItemSlot");
        EnSubMenu enSubMenu = m_iPopupMain.GetCurrentSubMenu().EnSubMenu;

        //아이템 정보 업데이트
        CsUIData.Instance.DisplayItemSlot(trItemSlot, csHeroSubGear);

        Transform trSubGearInfo = trSubGear.Find("SubGearInfo");
        //장비 타입
        Text textTextType = trSubGearInfo.Find("TextType").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTextType);
        textTextType.text = csHeroSubGear.SubGear.Name;

        //장비 이름
        Text textTextName = trSubGearInfo.Find("TextName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTextName);
        textTextName.text = csHeroSubGear.Name;

        //승품 여부 확인
        CheckQuality(csHeroSubGear, trSubGear);

        Transform trSocketList = trSubGearInfo.Find("SocketList");
        Transform trRuneList = trSubGearInfo.Find("RuneList");

        //제련
        if (enSubMenu == EnSubMenu.SubGearLevelUp)
        {
            trSocketList.gameObject.SetActive(false);
            trRuneList.gameObject.SetActive(false);
        }
        //세공
        else if (enSubMenu == EnSubMenu.SubGearSoulstone)
        {
            trRuneList.gameObject.SetActive(false);
            int nSoulstoneSocketCount = csHeroSubGear.SubGear.SubGearSoulstoneSocketList.Count;
            int nSoulstoneEquipCount = csHeroSubGear.SoulstoneSocketList.Count;

            //소켓이 안뚤려있으면 소켓리스트 비활성화
            if (nSoulstoneSocketCount == 0)
            {
                trSocketList.gameObject.SetActive(false);
            }
            else
            {
                //먼저 소켓 전체 초기화
                for (int i = 0; i < 6; ++i)
                {
                    Transform trSoulstoneSocket = trSocketList.Find("ImageSocket" + i);
                    trSoulstoneSocket.gameObject.SetActive(false);
                    //보석 장착 이미지 비활성화
                    trSoulstoneSocket.Find("ImageIcon").gameObject.SetActive(false);
                }

                //뚫려 있는 소켓 표시
                for (int i = 0; i < nSoulstoneSocketCount; ++i)
                {
                    Transform trSoulstoneSocket = trSocketList.Find("ImageSocket" + csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].SocketIndex);

                    if (csHeroSubGear.SubGearLevel.SubGearGrade.Grade >= csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].RequiredSubGearGrade.Grade)
                    {
                        Image imageSoulstoneSocket = trSoulstoneSocket.GetComponent<Image>();
                        imageSoulstoneSocket.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_mini_socket" + (csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].ItemType).ToString());
                        trSoulstoneSocket.gameObject.SetActive(true);
                    }
                    else
                    {
                        Image imageSoulstoneSocket = trSoulstoneSocket.GetComponent<Image>();
                        imageSoulstoneSocket.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/frm_mini_socket" + (csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i].ItemType).ToString());
                        trSoulstoneSocket.gameObject.SetActive(false);
                    }

                }

                //장착 된 보석 표시
                for (int i = 0; i < nSoulstoneEquipCount; ++i)
                {
                    Transform trSoulstoneSocket = trSocketList.Find("ImageSocket" + csHeroSubGear.SoulstoneSocketList[i].Index);
                    Image imageIcon = trSoulstoneSocket.Find("ImageIcon").GetComponent<Image>();
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csHeroSubGear.SoulstoneSocketList[i].Item.Image);
                    imageIcon.gameObject.SetActive(true);
                }

                //소켓 리스트 활성화
                trSocketList.gameObject.SetActive(true);
            }
        }
        else if (enSubMenu == EnSubMenu.SubGearRune)
        {
            trSocketList.gameObject.SetActive(false);

            int nRuneSocketCount = csHeroSubGear.SubGear.SubGearRuneSocketList.Count;   // 서브기어 전체 룬 소켓 리스트
            int nRuneEquipCount = csHeroSubGear.RuneSocketList.Count;                   // 서브기어에 장착된 룬 리스트

            if (nRuneSocketCount == 0)
            {
                trRuneList.gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < nRuneSocketCount; ++i)
                {
                    Transform trRuneSocket = trRuneList.Find("ImageRune" + i);
                    trRuneSocket.gameObject.SetActive(false);
                    trRuneSocket.Find("ImageIcon").gameObject.SetActive(false);
                }

                for (int i = 0; i < nRuneSocketCount; i++)
                {
                    Transform trRuneSocket = trRuneList.Find("ImageRune" + i);

                    Image imageRuneSocket = trRuneSocket.GetComponent<Image>();
                    imageRuneSocket.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/" + csHeroSubGear.SubGear.SubGearRuneSocketList[i].MiniBackgroundImageName);

                    if (csHeroSubGear.SubGearLevel.Level < csHeroSubGear.SubGear.SubGearRuneSocketList[i].RequiredSubGearLevel)
                    {
                        imageRuneSocket.gameObject.SetActive(false);
                    }
                    else
                    {
                        imageRuneSocket.gameObject.SetActive(true);
                    }
                }

                for (int i = 0; i < nRuneEquipCount; i++)
                {
                    Transform trRuneSocket = trRuneList.Find("ImageRune" + csHeroSubGear.RuneSocketList[i].Index);
                    Image imageIcon = trRuneSocket.Find("ImageIcon").GetComponent<Image>();
                    imageIcon.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csHeroSubGear.RuneSocketList[i].Item.Image);
                    imageIcon.gameObject.SetActive(true);
                }

                trRuneList.gameObject.SetActive(true);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void CheckQuality(CsHeroSubGear csHeroSubGear, Transform trSubGear)
    {
        Text textQualityUp = trSubGear.Find("TextQualityUp").GetComponent<Text>();

        if (m_iPopupMain.GetCurrentSubMenu().EnSubMenu == EnSubMenu.SubGearLevelUp && csHeroSubGear.NextStep == EnNextStep.Quality)
        {
            //다음 승품 재료 
            CsSubGearLevelQuality csLevelQuality = csHeroSubGear.SubGearLevel.GetSubGearLevelQuality(csHeroSubGear.Quality);
            int nNextItemId1 = csLevelQuality.NextQualityUpItem1.ItemId;
            int nNettItemCount1 = csLevelQuality.NextQualityUpItem1Count;
            int nNextItemId2 = csLevelQuality.NextQualityUpItem2.ItemId;
            int nNettItemCount2 = csLevelQuality.NextQualityUpItem2Count;

            //승품 가능
            if (CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId1) >= nNettItemCount1
                && CsGameData.Instance.MyHeroInfo.GetItemCount(nNextItemId2) >= nNettItemCount2)
            {
                textQualityUp.text = CsConfiguration.Instance.GetString("A09_TXT_00002");
                CsUIData.Instance.SetFont(textQualityUp);
            }
            //승품 대기
            else
            {
                textQualityUp.text = CsConfiguration.Instance.GetString("A09_TXT_00001");
                CsUIData.Instance.SetFont(textQualityUp);
            }
        }
        else
        {
            textQualityUp.text = "";
        }

    }

    //제련, 세공에 따른 보조장비 리스트 갱신
    //---------------------------------------------------------------------------------------------------
    void UpdateSubGearList()
    {
        m_bTabCheck = true;
        EnSubMenu enSubMenu = m_iPopupMain.GetCurrentSubMenu().EnSubMenu;
        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;
        for (int i = 0; i < listHeroSubGear.Count; ++i)
        {
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }

            int nSubGearId = listHeroSubGear[i].SubGear.SubGearId;
            Transform trSubGear = m_trContent.Find("ToggleSubGear" + nSubGearId);
            Toggle toggle;
			
            if (trSubGear == null)
            {
                trSubGear = CreateSubGear(listHeroSubGear[i]);
                toggle = trSubGear.GetComponent<Toggle>();
                toggle.group = m_trContent.GetComponent<ToggleGroup>();
                toggle.onValueChanged.AddListener((bIson) => OnValueChangedSubGear(bIson, nSubGearId));
            }
            else
            {
                toggle = trSubGear.GetComponent<Toggle>();
                toggle.onValueChanged.RemoveAllListeners();

                if (CsUIData.Instance.SubGearId == nSubGearId)
                {
                    toggle.isOn = true;
                    CsGameEventUIToUI.Instance.OnEventSubGearSelected(nSubGearId);
                    toggle.onValueChanged.AddListener((bIson) => OnValueChangedSubGear(bIson, nSubGearId));
                    m_bTabCheck = false;
                    CsUIData.Instance.SubGearId = nSubGearId;
                }
                else
                {
                    toggle.isOn = false;
                    toggle.onValueChanged.AddListener((bIson) => OnValueChangedSubGear(bIson, nSubGearId));
                }

            }

            DisplaySubGear(trSubGear, listHeroSubGear[i]);
        }


    }

    //객체 생성
    //---------------------------------------------------------------------------------------------------
    Transform CreateSubGear(CsHeroSubGear csHeroSubGear)
    {
        int nSubGearId = csHeroSubGear.SubGear.SubGearId;
        Transform trSubGear = Instantiate(m_goToggleSubGear, m_trContent).transform;
        trSubGear.name = m_goToggleSubGear.name + nSubGearId;

        //장비 표시
        DisplaySubGear(trSubGear, csHeroSubGear);

        return trSubGear;
    }

    //보조장비 갱신
    //---------------------------------------------------------------------------------------------------
    void UpdateSubGear(int nSubGearId)
    {
        Transform trSubGear = m_trContent.Find("ToggleSubGear" + nSubGearId);

        if (trSubGear != null)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
            DisplaySubGear(trSubGear, csHeroSubGear);
            CsGameEventUIToUI.Instance.OnEventSubGearSelected(nSubGearId);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void AllCheck(int nSubGearId)
    {
        List<CsHeroSubGear> listSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;

        if (CheckNextSubGear(nSubGearId))
        {
            UpdateSubGear(nSubGearId);
            return;
        }

        Transform trSubGear = m_trContent.Find("ToggleSubGear" + nSubGearId);

        if (trSubGear != null)
        {
            CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
            DisplaySubGear(trSubGear, csHeroSubGear);
        }

        //순차적 이동
        for (int i = 0; i < listSubGear.Count; ++i)
        {
            if (!listSubGear[i].Equipped)
                continue;

            if (nSubGearId == listSubGear[i].SubGear.SubGearId)
                continue;

            if (CheckNextSubGear(listSubGear[i].SubGear.SubGearId))
            {
                m_trContent.Find("ToggleSubGear" + listSubGear[i].SubGear.SubGearId).GetComponent<Toggle>().isOn = true;
                return;
            }
        }

        UpdateSubGear(nSubGearId);
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

    //---------------------------------------------------------------------------------------------------
    bool CheckSoulStone(int nSubGearId)
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);


        // 소켓리스트 장착가능한 빈슬롯이 있는지 체크
        for (int i = 0; i < csHeroSubGear.SubGear.SubGearSoulstoneSocketList.Count; i++)
        {
            CsSubGearSoulstoneSocket csSubGearSoulstoneSocket = csHeroSubGear.SubGear.SubGearSoulstoneSocketList[i];

            // 요구등급체크
            if (csSubGearSoulstoneSocket.RequiredSubGearGrade.Grade <= csHeroSubGear.SubGearLevel.SubGearGrade.Grade)
            {
                CsHeroSubGearSoulstoneSocket csHeroSubGearSoulstoneSocket = csHeroSubGear.GetHeroSubGearSoulstoneSocket(csSubGearSoulstoneSocket.SocketIndex);

                // 빈슬롯
                if (csHeroSubGearSoulstoneSocket == null)
                {
                    int nCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType(csSubGearSoulstoneSocket.ItemType);

                    if (nCount > 0)
                    {
                        return true;
                    }
                }
				else
				{
					// 장착 중인 소울스톤보다 높은 레벨의 소울스톤이 있는 경우
					List<CsInventorySlot> listSoulStone = CsGameData.Instance.MyHeroInfo.InventorySlotList.FindAll(inventorySlot => inventorySlot.EnType == EnInventoryObjectType.Item &&
																																	inventorySlot.InventoryObjectItem.Item.ItemType.ItemType == csHeroSubGearSoulstoneSocket.Item.ItemType.ItemType &&
																																	inventorySlot.InventoryObjectItem.Item.Level > csHeroSubGearSoulstoneSocket.Item.Level);

					if (listSoulStone.Count > 0)
					{
						return true;
					}
				}
            }
        }

        // 아이템 합성 가능한 장착 소켓이 있는지 체크
        for (int i = 0; i < csHeroSubGear.SoulstoneSocketList.Count; i++)
        {
            CsItemCompositionRecipe csItemCompositionRecipe = CsGameData.Instance.GetItemCompositionRecipe(csHeroSubGear.SoulstoneSocketList[i].Item.ItemId);

            if (csItemCompositionRecipe == null)
                continue;

            int nCount = CsGameData.Instance.MyHeroInfo.GetItemCount(csItemCompositionRecipe.MaterialItemId);

            if (nCount >= csItemCompositionRecipe.MaterialItemCount - 1 && CsGameData.Instance.MyHeroInfo.Gold >= csItemCompositionRecipe.Gold)
            {
                return true;
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    bool CheckRun(int nSubGearId)
    {
        CsHeroSubGear csHeroSubGear = CsGameData.Instance.MyHeroInfo.GetHeroSubGear(nSubGearId);
        // 소켓리스트 장착가능한 빈슬롯이 있는지 체크
        for (int i = 0; i < csHeroSubGear.SubGear.SubGearRuneSocketList.Count; i++)
        {
            CsSubGearRuneSocket csSubGearRuneSocket = csHeroSubGear.SubGear.SubGearRuneSocketList[i];

            // 요구레벨
            if (csHeroSubGear.Level >= csSubGearRuneSocket.RequiredSubGearLevel)
            {
                CsHeroSubGearRuneSocket csHeroSubGearRuneSocket = csHeroSubGear.GetHeroSubGearRuneSocket(csSubGearRuneSocket.SocketIndex);

                if (csHeroSubGearRuneSocket == null)
                {
                    for (int j= 0; j < csSubGearRuneSocket.SubGearRuneSocketAvailableItemTypeList.Count; j++)
                    {
                        int nCount = CsGameData.Instance.MyHeroInfo.GetItemCountByItemType(csSubGearRuneSocket.SubGearRuneSocketAvailableItemTypeList[j].ItemType);

                        if (nCount > 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    //---------------------------------------------------------------------------------------------------
    void SubGearLeveulUpCheckUpdate()
    {
        List<CsHeroSubGear> listHeroSubGear = CsGameData.Instance.MyHeroInfo.HeroSubGearList;
        EnSubMenu enSubMenu = m_iPopupMain.GetCurrentSubMenu().EnSubMenu;

        for (int i = 0; i < listHeroSubGear.Count; i++)
        {
            if (!listHeroSubGear[i].Equipped)
            {
                continue;
            }

            Transform trSubGear = m_trContent.Find("ToggleSubGear" + listHeroSubGear[i].SubGear.SubGearId);
            Transform trImageNotice = trSubGear.Find("ImageNotice");

            switch (enSubMenu)
            {
                case EnSubMenu.SubGearLevelUp:
                    trImageNotice.gameObject.SetActive(CheckNextSubGear(listHeroSubGear[i].SubGear.SubGearId));
                    break;
                    
                case EnSubMenu.SubGearSoulstone:
                    trImageNotice.gameObject.SetActive(CheckSoulStone(listHeroSubGear[i].SubGear.SubGearId));
                    break;

                case EnSubMenu.SubGearRune:
                    trImageNotice.gameObject.SetActive(CheckRun(listHeroSubGear[i].SubGear.SubGearId));
                    break;
            }
        }


    }
}


