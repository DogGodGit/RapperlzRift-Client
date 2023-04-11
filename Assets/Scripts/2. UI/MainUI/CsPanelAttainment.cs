using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 임채영 (2018-02-21)
//---------------------------------------------------------------------------------------------------

public class CsPanelAttainment : MonoBehaviour
{

    Transform m_trAttainment;
    Transform m_trItem;

    Text m_textTitleName;
    Text m_textCondition;
    Text m_textAttainmentName;
    Text m_textAttainmentScript;

    Button m_buttonCheck;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp += OnEventMyHeroLevelUp;
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive += OnEventAttainmentRewardReceive;
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsGameEventUIToUI.Instance.EventMyHeroLevelUp -= OnEventMyHeroLevelUp;
        CsGameEventUIToUI.Instance.EventAttainmentRewardReceive -= OnEventAttainmentRewardReceive;
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePanelAttainment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventMyHeroLevelUp()
    {
        DisplayAttainment();
        TutorialCheck();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventAttainmentRewardReceive()
    {
        DisplayAttainment();
        TutorialCheck();
    }

    #endregion Event

    #region Event Handler

    //---------------------------------------------------------------------------------------------------
    void OnClickPanelClose()
    {
        ClosePanelAttainment();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAttainmentRewardReceive()
    {
        CsCommandEventManager.Instance.SendAttainmentRewardReceive(CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo + 1);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickMove()
    {
        CsGameEventUIToUI.Instance.OnEventAutoQuestStart(EnAutoStateType.MainQuest);
        ClosePanelAttainment();
    }

    #endregion Event Handler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_trAttainment = transform.Find("Attainment");

        Transform trBack = m_trAttainment.Find("ImageBackground");

        m_textTitleName = trBack.Find("TextTitleName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTitleName);

        Transform trWreath = trBack.Find("ImageWreath");
        m_trItem = trWreath.Find("ImageItem");

        m_textAttainmentName = trWreath.Find("TextAttainmentName").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAttainmentName);

        m_textAttainmentScript = trWreath.Find("TextAttainmentScript").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textAttainmentScript);

        Text textCompleteCondition = trBack.Find("TextConditionName").GetComponent<Text>();
        textCompleteCondition.text = CsConfiguration.Instance.GetString("A30_TXT_00001");
        CsUIData.Instance.SetFont(textCompleteCondition);

        m_textCondition = trBack.Find("TextCondition").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textCondition);

        Button buttonClose = trBack.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(OnClickPanelClose);
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonCheck = trBack.Find("ButtonCheck").GetComponent<Button>();
        m_buttonCheck.onClick.RemoveAllListeners();

        Text textButtonCheck = m_buttonCheck.transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textButtonCheck);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayAttainment()
    {
        CsAttainmentEntry csAttainmentEntry = CsGameData.Instance.GetAttainmentEntry(CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo + 1);

        if (csAttainmentEntry != null)
        {
            m_textTitleName.text = csAttainmentEntry.Name;
            m_textAttainmentName.text = csAttainmentEntry.Name;
            m_textAttainmentScript.text = csAttainmentEntry.Description;

            //리워드 1번인 데이터만 화면에 표시
            CsAttainmentEntryReward csAttainmentEntryReward = csAttainmentEntry.AttainmentEntryRewardList.Find(a => a.RewardNo == 1);

            if (csAttainmentEntryReward != null)
            {
                //메인장비
                if (csAttainmentEntryReward.Type == 1)
                {
                    m_trItem.Find("ImageIcon").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/" + csAttainmentEntryReward.MainGear.Image);
                    m_trItem.Find("ImageOwn").gameObject.SetActive(csAttainmentEntryReward.MainGearOwned);
                    m_trItem.Find("ImageFrameRank").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csAttainmentEntryReward.MainGear.MainGearGrade.Grade.ToString("00"));
                }
                //아이템
                else if (csAttainmentEntryReward.Type == 2)
                {
                    m_trItem.Find("ImageIcon").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/Item_" + csAttainmentEntryReward.ItemReward.Item.ItemId);
                    m_trItem.Find("ImageOwn").gameObject.SetActive(csAttainmentEntryReward.ItemReward.ItemOwned);
                    m_trItem.Find("ImageFrameRank").GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Items/frm_rank" + csAttainmentEntryReward.ItemReward.Item.ItemGrade.Grade.ToString("00"));
                }
            }
            //레벨 조건
            if (csAttainmentEntry.Type == 1)
            {
                m_buttonCheck.onClick.RemoveAllListeners();
                m_textCondition.text = string.Format(CsConfiguration.Instance.GetString("A30_TXT_01001"), csAttainmentEntry.RequiredHeroLevel);
                Text textButtonCheck = m_buttonCheck.transform.Find("Text").GetComponent<Text>();

                if (CsGameData.Instance.MyHeroInfo.Level >= csAttainmentEntry.RequiredHeroLevel)
                {
                    m_textCondition.color = CsUIData.Instance.ColorGreen;
                    m_buttonCheck.onClick.AddListener(OnClickAttainmentRewardReceive);
                    m_buttonCheck.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    textButtonCheck.text = CsConfiguration.Instance.GetString("A30_BTN_00002");
                }
                else
                {
                    m_textCondition.color = CsUIData.Instance.ColorRed;
                    m_buttonCheck.onClick.AddListener(OnClickMove);
                    m_buttonCheck.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    textButtonCheck.text = CsConfiguration.Instance.GetString("A30_BTN_00001");
                }
            }
            //퀘스트 조건
            else if (csAttainmentEntry.Type == 2)
            {
                m_buttonCheck.onClick.RemoveAllListeners();
                m_textCondition.text = string.Format(CsConfiguration.Instance.GetString("A30_TXT_01002"), CsGameData.Instance.GetMainQuest(csAttainmentEntry.RequiredMainQuestNo).Title);
                Text textButtonCheck = m_buttonCheck.transform.Find("Text").GetComponent<Text>();

                if (CsMainQuestManager.Instance.MainQuest != null && CsMainQuestManager.Instance.MainQuest.MainQuestNo >= csAttainmentEntry.RequiredMainQuestNo)
                {
                    m_textCondition.color = CsUIData.Instance.ColorGreen;
                    m_buttonCheck.onClick.AddListener(OnClickAttainmentRewardReceive);
                    m_buttonCheck.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    textButtonCheck.text = CsConfiguration.Instance.GetString("A30_BTN_00002");
                }
                else
                {
                    m_textCondition.color = CsUIData.Instance.ColorRed;
                    m_buttonCheck.onClick.AddListener(OnClickMove);
                    m_buttonCheck.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
                    textButtonCheck.text = CsConfiguration.Instance.GetString("A30_BTN_00001");
                }
            }
        }
        else
        {
            ClosePanelAttainment();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void OpenPanelAttainment()
    {
        if (!m_trAttainment.gameObject.activeSelf)
        {
            m_trAttainment.gameObject.SetActive(true);
            DisplayAttainment();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePanelAttainment()
    {
        if (m_trAttainment.gameObject.activeSelf)
        {
            m_trAttainment.gameObject.SetActive(false);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void TutorialCheck()
    {
        if (CsConfiguration.Instance.GetTutorialKey(EnTutorialType.Attainment))
        {
            if (CsGameData.Instance.MyHeroInfo.RewardedAttainmentEntryNo == 0)
            {
                if (CsGameData.Instance.MyHeroInfo.CheckAttainment())
                {
                    CsGameEventUIToUI.Instance.OnEventReferenceTutorial(EnTutorialType.Attainment);
                }
            }
        }
    }
}
