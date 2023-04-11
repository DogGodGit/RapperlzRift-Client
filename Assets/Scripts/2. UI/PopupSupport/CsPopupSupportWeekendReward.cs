using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 김용재 (2018-08-01)
//---------------------------------------------------------------------------------------------------

public class CsPopupSupportWeekendReward : CsPopupSub
{
    Transform m_trCardList;
    Transform m_trImageDiaReceive;

    Text m_textTotalGetDiaNumber;

    Button m_buttonReceiveReward;

    int m_nRewardDiaCount = 0;

    enum EnWeekendReward
    {
        Friday = 1,
        Saturday = 2,
        SunDay = 3, 
    }

    //---------------------------------------------------------------------------------------------------
    protected override void Initialize()
    {
        CsGameEventUIToUI.Instance.EventWeekendRewardReceive += OnEventWeekendRewardReceive;
        CsGameEventUIToUI.Instance.EventWeekendRewardSelect += OnEventWeekendRewardSelect;

        CsGameEventUIToUI.Instance.EventDateChanged += OnEventDateChanged;

        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    protected override void OnFinalize()
    {
        CsGameEventUIToUI.Instance.EventWeekendRewardReceive -= OnEventWeekendRewardReceive;
        CsGameEventUIToUI.Instance.EventWeekendRewardSelect -= OnEventWeekendRewardSelect;

        CsGameEventUIToUI.Instance.EventDateChanged -= OnEventDateChanged;
    }

    //---------------------------------------------------------------------------------------------------
    public override void OnUpdate(float flTime)
    {

    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventWeekendRewardReceive()
    {
        UpdateGetDia();

        CsGameEventUIToUI.Instance.OnEventToastMessage(EnToastType.Normal, string.Format(CsConfiguration.Instance.GetString("A103_TXT_02002"), m_nRewardDiaCount));
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventWeekendRewardSelect()
    {
        UpdateGetDia();
        UpdateCardList();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventDateChanged()
    {
        UpdateGetDia();
        UpdateCardList();
        UpdateImageDiaReceive();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCardSelect(int nSelectionNo)
    {
        CsCommandEventManager.Instance.SendWeekendRewardSelect(nSelectionNo);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickWeekendReward()
    {
        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Monday)
        {
            CsCommandEventManager.Instance.SendWeekendRewardReceive();
        }
        else
        {

        }
    }
    
    #endregion Event

    #region Event Handler

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        Text textInfo = transform.Find("TextInfo").GetComponent<Text>();
        CsUIData.Instance.SetFont(textInfo);
        textInfo.text = CsConfiguration.Instance.GetString(CsGameData.Instance.WeekendReward.Description);

        Text textTotalGetDia = transform.Find("ImageGlow/TextTotalGetDia").GetComponent<Text>();
        CsUIData.Instance.SetFont(textTotalGetDia);
        textTotalGetDia.text = CsConfiguration.Instance.GetString("A103_TXT_01001");

        m_textTotalGetDiaNumber = transform.Find("ImageGlow/TextTotalGetDiaNumber").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTotalGetDiaNumber);

        m_trCardList = transform.Find("CardList");

        Transform trBottomFrame = transform.Find("BottomFrame");

        Text textRewardDetail = trBottomFrame.Find("TextRewardDetail").GetComponent<Text>();
        CsUIData.Instance.SetFont(textRewardDetail);
        textRewardDetail.text = CsConfiguration.Instance.GetString(CsGameData.Instance.WeekendReward.ScheduleText);

        m_buttonReceiveReward = trBottomFrame.Find("ButtonReceiveReward").GetComponent<Button>();
        m_buttonReceiveReward.onClick.RemoveAllListeners();
        m_buttonReceiveReward.onClick.AddListener(OnClickWeekendReward);
        m_buttonReceiveReward.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        Text textButtonReceiveReward = m_buttonReceiveReward.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonReceiveReward);
        textButtonReceiveReward.text = CsConfiguration.Instance.GetString("A103_BTN_00002");

        m_trImageDiaReceive = transform.Find("ImageDiaReceive");

        Text textDiaReceive = m_trImageDiaReceive.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textDiaReceive);
        textDiaReceive.text = CsConfiguration.Instance.GetString("A103_TXT_00007");

        UpdateGetDia();
        UpdateCardList();
        UpdateImageDiaReceive();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateGetDia()
    {
        string strRewardDiaNumber = "";

        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Monday)
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 != -1)
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3.ToString();
            }

            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 != -1)
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2.ToString();
            }

            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 != -1)
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1.ToString();
            }

            if (strRewardDiaNumber == "")
            {
                m_textTotalGetDiaNumber.text = "???";

                CsUIData.Instance.DisplayButtonInteractable(m_buttonReceiveReward, false);
            }
            else
            {
                m_nRewardDiaCount = int.Parse(strRewardDiaNumber);
                m_textTotalGetDiaNumber.text = m_nRewardDiaCount.ToString();

                if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Rewarded)
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonReceiveReward, false);
                }
                else
                {
                    CsUIData.Instance.DisplayButtonInteractable(m_buttonReceiveReward, true);
                }
            }
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 == -1)
            {
                strRewardDiaNumber += "?";
            }
            else
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3.ToString();
            }

            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 == -1)
            {
                strRewardDiaNumber += "?";
            }
            else
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2.ToString();
            }

            if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 == -1)
            {
                strRewardDiaNumber += "?";
            }
            else
            {
                strRewardDiaNumber += CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1.ToString();
            }

            m_textTotalGetDiaNumber.text = strRewardDiaNumber;

            CsUIData.Instance.DisplayButtonInteractable(m_buttonReceiveReward, false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateCardList()
    {
        Transform trCard = null;

        for (int i = 1; i <= m_trCardList.childCount; i++)
        {
            trCard = m_trCardList.Find("ButtonCard" + i);

            if (trCard == null)
            {
                continue;
            }
            else
            {
                Transform trImageCardSelect = trCard.Find("ImageCardSelect");

                Image imageCardBackground = trCard.Find("ImageCardBackground").GetComponent<Image>();

                Text textTouch = trCard.Find("TextTouch").GetComponent<Text>();
                CsUIData.Instance.SetFont(textTouch);
                textTouch.text = CsConfiguration.Instance.GetString("A103_TXT_00002");

                Text textNumber = trCard.Find("TextNumber").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNumber);

                Text textDayOfWeek = trCard.Find("TextDayOfWeek").GetComponent<Text>();
                CsUIData.Instance.SetFont(textDayOfWeek);

                int nSelectCardNo = i;

                Button buttonCardSelect = trCard.GetComponent<Button>();
                buttonCardSelect.onClick.RemoveAllListeners();
                buttonCardSelect.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                buttonCardSelect.onClick.AddListener(() => OnClickCardSelect(nSelectCardNo));
                buttonCardSelect.transition = Selectable.Transition.None;

                switch ((EnWeekendReward)i)
                {
                    case EnWeekendReward.Friday:

                        if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1 == -1)
                        {
                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_back");
                            imageCardBackground.gameObject.SetActive(true);

                            textNumber.gameObject.SetActive(false);

                            if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Friday)
                            {
                                trImageCardSelect.gameObject.SetActive(true);
                                buttonCardSelect.interactable = true;
                                buttonCardSelect.transition = Selectable.Transition.ColorTint;
                                textTouch.gameObject.SetActive(true);
                            }
                            else
                            {
                                trImageCardSelect.gameObject.SetActive(false);
                                buttonCardSelect.interactable = false;
                                buttonCardSelect.transition = Selectable.Transition.None;
                                textTouch.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            trImageCardSelect.gameObject.SetActive(false);
                            buttonCardSelect.interactable = false;
                            buttonCardSelect.transition = Selectable.Transition.None;

                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_front");
                            imageCardBackground.gameObject.SetActive(true);

                            textTouch.gameObject.SetActive(false);

                            textNumber.text = CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection1.ToString();
                            textNumber.gameObject.SetActive(true);
                        }

                        textDayOfWeek.text = CsConfiguration.Instance.GetString("A103_TXT_00003");
                        textDayOfWeek.gameObject.SetActive(true);

                        break;

                    case EnWeekendReward.Saturday:

                        if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2 == -1)
                        {
                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_back");
                            imageCardBackground.gameObject.SetActive(true);

                            textNumber.gameObject.SetActive(false);

                            if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Saturday)
                            {
                                trImageCardSelect.gameObject.SetActive(true);
                                buttonCardSelect.interactable = true;
                                buttonCardSelect.transition = Selectable.Transition.ColorTint;
                                textTouch.gameObject.SetActive(true);
                            }
                            else
                            {
                                trImageCardSelect.gameObject.SetActive(false);
                                buttonCardSelect.interactable = false;
                                buttonCardSelect.transition = Selectable.Transition.None;
                                textTouch.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            trImageCardSelect.gameObject.SetActive(false);
                            buttonCardSelect.interactable = false;
                            buttonCardSelect.transition = Selectable.Transition.None;

                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_front");
                            imageCardBackground.gameObject.SetActive(true);

                            textTouch.gameObject.SetActive(false);

                            textNumber.text = CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection2.ToString();
                            textNumber.gameObject.SetActive(true);
                        }

                        textDayOfWeek.text = CsConfiguration.Instance.GetString("A103_TXT_00004");
                        textDayOfWeek.gameObject.SetActive(true);

                        break;

                    case EnWeekendReward.SunDay:

                        if (CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 == -1)
                        {
                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_back");
                            imageCardBackground.gameObject.SetActive(true);

                            textNumber.gameObject.SetActive(false);

                            if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Sunday)
                            {
                                trImageCardSelect.gameObject.SetActive(true);
                                buttonCardSelect.interactable = true;
                                buttonCardSelect.transition = Selectable.Transition.ColorTint;
                                textTouch.gameObject.SetActive(true);
                            }
                            else
                            {
                                trImageCardSelect.gameObject.SetActive(false);
                                buttonCardSelect.interactable = false;
                                buttonCardSelect.transition = Selectable.Transition.None;
                                textTouch.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            trImageCardSelect.gameObject.SetActive(false);
                            buttonCardSelect.interactable = false;
                            buttonCardSelect.transition = Selectable.Transition.None;

                            imageCardBackground.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/PopupSupport/frm_card_front");
                            imageCardBackground.gameObject.SetActive(true);

                            textTouch.gameObject.SetActive(false);

                            textNumber.text = CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3.ToString();
                            textNumber.gameObject.SetActive(true);
                        }

                        textDayOfWeek.text = CsConfiguration.Instance.GetString("A103_TXT_00005");
                        textDayOfWeek.gameObject.SetActive(true);

                        break;
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateImageDiaReceive()
    {
        if (CsGameData.Instance.MyHeroInfo.CurrentDateTime.DayOfWeek == System.DayOfWeek.Sunday && CsGameData.Instance.MyHeroInfo.HeroWeekendReward.Selection3 != -1)
        {
            m_trImageDiaReceive.gameObject.SetActive(true);
        }
        else
        {
            m_trImageDiaReceive.gameObject.SetActive(false);
        }
    }
}
