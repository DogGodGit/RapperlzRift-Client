using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-01-03)
//---------------------------------------------------------------------------------------------------

public class CsMatchingManagement : CsPopupSub
{

    Transform m_trPartyList;
    Transform m_trButtonList;
    Transform m_trSelect;

    Button m_buttonFriend;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {

    }

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    //친구 추가
    void OnClickAddFriend()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, "준비중입니다");
    }

    //---------------------------------------------------------------------------------------------------
    //멤버 조회
    void OnClickMatchingMemberLookup()
    {
        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, "준비중입니다");
    }

    //---------------------------------------------------------------------------------------------------
    //선택 취소
    void OnClickPartyMemberCancle()
    {
        m_trSelect.gameObject.SetActive(false);
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trPartyList = transform.Find("PartyList");
        m_trSelect = transform.Find("ImageSelect");
        m_trButtonList = m_trSelect.Find("ButtonList");

        Button buttonSelect = m_trSelect.GetComponent<Button>();
        buttonSelect.onClick.RemoveAllListeners();
        buttonSelect.onClick.AddListener(OnClickPartyMemberCancle);
        buttonSelect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        //친구 추가 버튼
        m_buttonFriend = m_trButtonList.Find("ButtonFriend").GetComponent<Button>();
        m_buttonFriend.onClick.RemoveAllListeners();
        m_buttonFriend.onClick.AddListener(OnClickAddFriend);
        m_buttonFriend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textFriend = m_buttonFriend.transform.Find("Text").GetComponent<Text>();
        textFriend.text = CsConfiguration.Instance.GetString("A36_BTN_00006");
        CsUIData.Instance.SetFont(textFriend);

        //조회 버튼
        Button buttonInfo = m_trButtonList.Find("ButtonInfo").GetComponent<Button>();
        buttonInfo.onClick.RemoveAllListeners();
        buttonInfo.onClick.AddListener(OnClickMatchingMemberLookup);
        buttonInfo.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textInfo = buttonInfo.transform.Find("Text").GetComponent<Text>();
        textInfo.text = CsConfiguration.Instance.GetString("A36_BTN_00009");
        CsUIData.Instance.SetFont(textInfo);

        DisplayMatching();
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMatching()
    {
        DisplayMatchingDefault();

        List<IHeroObjectInfo> listMatching = CsGameData.Instance.ListHeroObjectInfo;
        MatchingMember();

        for (int i = 0; i < listMatching.Count; ++i)
        {
            MatchingMember(listMatching[i].GetHeroBase(), i + 1);
        }

    }

    //---------------------------------------------------------------------------------------------------
    void DisplayMatchingDefault()
    {
        for (int i = 0; i < CsGameConfig.Instance.PartyMemberMaxCount; ++i)
        {
            Transform trMember = m_trPartyList.Find("PartyHero" + i);

            if (trMember != null)
            {
                trMember.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void MatchingMember(CsHeroBase csHeroBase = null, int nMemberCount = 0)
    {
        Transform trMember = m_trPartyList.Find("PartyHero" + nMemberCount);

        if (csHeroBase == null)
        {
            csHeroBase = CsGameData.Instance.MyHeroInfo;
        }

        if (trMember != null)
        {
            int nIndex = nMemberCount;
            Text textLevel = trMember.Find("ImageBackGround/TextLevel").GetComponent<Text>();
            textLevel.text = string.Format(CsConfiguration.Instance.GetString("A36_TXT_01005"), csHeroBase.Level);
            CsUIData.Instance.SetFont(textLevel);

            Text textName = trMember.Find("ImageBackGround/ImageBottom/TextName").GetComponent<Text>();
            textName.text = csHeroBase.Name;
            CsUIData.Instance.SetFont(textName);

            Text textJob = trMember.Find("ImageBackGround/ImageBottom/TextJob").GetComponent<Text>();
            textJob.text = csHeroBase.Job.Name;
            CsUIData.Instance.SetFont(textJob);

            Text textBattlePoint = trMember.Find("ImageBackGround/ImageBottom/TextBattlePoint").GetComponent<Text>();
            //textBattlePoint.text = csHeroBase..ToString();
            CsUIData.Instance.SetFont(textBattlePoint);

            Image imageEmblem = trMember.Find("ImageBackGround/ImageEmblem").GetComponent<Image>();
            imageEmblem.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_small_emblem_off_" + csHeroBase.Job.JobId);

            Image imageNation = trMember.Find("ImageBackGround/ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_mini_nation" + csHeroBase.Nation.NationId);

            Button buttonPartyHero = trMember.GetComponent<Button>();
            buttonPartyHero.onClick.RemoveAllListeners();
            buttonPartyHero.onClick.AddListener(() => OnClickMatchingMember(csHeroBase.HeroId, nIndex));

            trMember.gameObject.SetActive(true);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMatchingMember(Guid guidPartyId, int nIndex)
    {
        //자기 자신을 클릭한 경우
        if (CsGameData.Instance.MyHeroInfo.HeroId == guidPartyId)
        {
            return;
        }

        //친구일 경우 친구 추가버튼 꺼줘야됨(추후)

        Transform trPartyMember = m_trPartyList.Find("PartyHero" + nIndex);

        m_trButtonList.transform.SetParent(trPartyMember.transform);
        m_trButtonList.GetComponent<RectTransform>().anchoredPosition = new Vector2(60f, -90f);
        m_trButtonList.transform.SetParent(m_trSelect.transform);

        m_trSelect.gameObject.SetActive(true);
    }
}
