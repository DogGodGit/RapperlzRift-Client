using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2017-12-22)
//---------------------------------------------------------------------------------------------------

public class CsPopupRestReward : MonoBehaviour
{
    Transform m_trImageBackground;

    //long m_lAcceptExpReward;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventRestRewardReceiveFree += OnEventRestRewardReceiveFree;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold += OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia += OnEventRestRewardReceiveDia;
    }

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(gameObject);
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventRestRewardReceiveFree -= OnEventRestRewardReceiveFree;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveGold -= OnEventRestRewardReceiveGold;
        CsGameEventUIToUI.Instance.EventRestRewardReceiveDia -= OnEventRestRewardReceiveDia;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveFree(bool bLevelUp, long lAcquiredExp)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveGold(bool bLevelUp, long lAcquiredExp)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventRestRewardReceiveDia(bool bLevelUp, long lAcquiredExp)
    {
        ClosePopup();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRestRewardReceiveFree()
    {
        //m_lAcceptExpReward = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpReward;
        CsCommandEventManager.Instance.SendRestRewardReceiveFree();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRestRewardReceiveGold()
    {
        //m_lAcceptExpReward = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpRewardByGold;
        CsCommandEventManager.Instance.SendRestRewardReceiveGold();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickRestRewardReceiveDia()
    {
        //m_lAcceptExpReward = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpRewardByDia;
        CsCommandEventManager.Instance.SendRestRewardReceiveDia();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickClose()
    {
        ClosePopup();
    }

    #endregion EventHndler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsRestRewardTime csRestRewardTime = CsGameData.Instance.GetRestRewardTime(CsGameData.Instance.MyHeroInfo.RestTime);

        if (csRestRewardTime != null)
        {
            m_trImageBackground = transform.Find("ImageBackground");

            Text textPopupName = m_trImageBackground.Find("TextPopupName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPopupName);
            textPopupName.text = CsConfiguration.Instance.GetString("A33_NAME_00001");

            Button buttonClose = m_trImageBackground.Find("ButtonClose").GetComponent<Button>();
            buttonClose.onClick.RemoveAllListeners();
            buttonClose.onClick.AddListener(OnClickClose);
            buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textRestRewardEXPName = m_trImageBackground.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRestRewardEXPName);
            textRestRewardEXPName.text = CsConfiguration.Instance.GetString("A33_TXT_00001");

            Text textRestRewardGetEXP = m_trImageBackground.Find("TextExpValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textRestRewardGetEXP);
            long lExpReward = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpReward;
            textRestRewardGetEXP.text = string.Format(CsConfiguration.Instance.GetString("A33_TXT_01001"), lExpReward.ToString("#,###"));

            Transform trRewardList = m_trImageBackground.Find("RewardList");
            // PanelFree
            Transform trPanelFree = trRewardList.Find("RewardFree");

            Text textPanelFreeName = trPanelFree.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelFreeName);
            textPanelFreeName.text = CsConfiguration.Instance.GetString("A33_TXT_00002");

            Text textPanelFreeEXP = trPanelFree.Find("TextExpValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelFreeEXP);
            textPanelFreeEXP.text = lExpReward.ToString("#,##0");

            Button buttonFreeAccept = trPanelFree.Find("ButtonAccept").GetComponent<Button>();
            buttonFreeAccept.onClick.RemoveAllListeners();
            buttonFreeAccept.onClick.AddListener(OnClickRestRewardReceiveFree);
            buttonFreeAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            Text textButtonFreeAccept = buttonFreeAccept.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonFreeAccept);
            textButtonFreeAccept.text = CsConfiguration.Instance.GetString("A33_BTN_00001");

            // PanelGold
            Transform trPanelGold = trRewardList.Find("RewardGold");

            Text textPanelGoldName = trPanelGold.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelGoldName);
            textPanelGoldName.text = string.Format(CsConfiguration.Instance.GetString("A33_TXT_01002"), CsGameConfig.Instance.RestRewardGoldReceiveExpPercentage); // {0}% 획득

            Text textPanelGoldExpValue = trPanelGold.Find("TextExpValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelGoldExpValue);
            long lExpRewardByGold = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpRewardByGold;
            textPanelGoldExpValue.text = lExpRewardByGold.ToString("#,##0");

            Button buttonGoldAccept = trPanelGold.Find("ButtonAccept").GetComponent<Button>();
            buttonGoldAccept.onClick.RemoveAllListeners();
            buttonGoldAccept.onClick.AddListener(OnClickRestRewardReceiveGold);
            buttonGoldAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            if (CsGameData.Instance.MyHeroInfo.Gold < csRestRewardTime.RequiredGold)
            {
                DisplayButtonInteractable(buttonGoldAccept, false);
            }

            Text textButtonGoldAccept = buttonGoldAccept.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonGoldAccept);
            textButtonGoldAccept.text = csRestRewardTime.RequiredGold.ToString("#,##0");

            // PanelDia
            Transform trPanelDia = trRewardList.Find("RewardDia");

            Text textPanelDiaName = trPanelDia.Find("TextName").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelDiaName);
            textPanelDiaName.text = string.Format(CsConfiguration.Instance.GetString("A33_TXT_01002"), CsGameConfig.Instance.RestRewardDiaReceiveExpPercentage); // {0}% 획득

            Text textPanelDiaEXP = trPanelDia.Find("TextExpValue").GetComponent<Text>();
            CsUIData.Instance.SetFont(textPanelDiaEXP);
            long lExpRewardByDia = CsGameData.Instance.MyHeroInfo.JobLevel.LevelMaster.ExpRewardByDia;
            textPanelDiaEXP.text = lExpRewardByDia.ToString("#,##0");

            Button buttonDiaAccept = trPanelDia.Find("ButtonAccept").GetComponent<Button>();
            buttonDiaAccept.onClick.RemoveAllListeners();
            buttonDiaAccept.onClick.AddListener(OnClickRestRewardReceiveDia);
            buttonDiaAccept.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

            if (CsGameData.Instance.MyHeroInfo.Dia < csRestRewardTime.RequiredDia)
            {
                DisplayButtonInteractable(buttonDiaAccept, false);
            }

            Text textButtonDiaAccept = buttonDiaAccept.transform.Find("Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textButtonDiaAccept);
            textButtonDiaAccept.text = csRestRewardTime.RequiredDia.ToString("#,##0");

            Text textGold = m_trImageBackground.Find("ImageGold/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textGold);
            textGold.text = CsGameData.Instance.MyHeroInfo.Gold.ToString("#,##0");

            Text textDia = m_trImageBackground.Find("ImageDia/Text").GetComponent<Text>();
            CsUIData.Instance.SetFont(textDia);
            textDia.text = CsGameData.Instance.MyHeroInfo.Dia.ToString("#,##0");
        }
        else
        {
            ClosePopup();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopup()
    {
        transform.parent.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void DisplayButtonInteractable(Button button, bool bIsOn)
    {
        button.interactable = bIsOn;
        Text textButton = button.transform.Find("Text").GetComponent<Text>();
        Image ImageButton = button.transform.Find("Image").GetComponent<Image>();

        if (bIsOn)
        {
            textButton.color = new Color32(255, 255, 255, 255);
            ImageButton.color = new Color32(255, 255, 255, 255);
        }
        else
        {
            textButton.color = new Color32(108, 113, 117, 255);
            ImageButton.color = new Color32(255, 255, 255, 127);
        }
    }
}