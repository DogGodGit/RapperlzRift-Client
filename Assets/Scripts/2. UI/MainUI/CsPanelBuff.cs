using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//---------------------------------------------------------------------------------------------------
// 작성 : 김경훈 (2018-01-31)
//---------------------------------------------------------------------------------------------------

public class CsPanelBuff : MonoBehaviour
{
    Transform m_trImageBack;
    Transform m_trImageScrollBuff;

    Button m_buttonBuff;
    Button m_buttonClose;

    Text m_textScrollBuff;

    float m_flTime = 0;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();

        CsGameEventUIToUI.Instance.EventExpScrollUse += OnEventExpScrollUse;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_textScrollBuff != null)
            {
                UpdateExpScrollBuffTime();
            }

            m_flTime = Time.time;
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventExpScrollUse -= OnEventExpScrollUse;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventExpScrollUse()
    {
        UpdateBuffCount();

        if (m_textScrollBuff != null)
        {
            UpdateExpScrollBuffTime();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickOpenBuff()
    {
        UpdateWorldBuff();
        m_buttonClose.gameObject.SetActive(true);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickCloseBuff()
    {
        m_buttonClose.gameObject.SetActive(false);
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_buttonBuff = transform.Find("ButtonBuff").GetComponent<Button>();
        m_buttonBuff.onClick.RemoveAllListeners();
        m_buttonBuff.onClick.AddListener(OnClickOpenBuff);
        m_buttonBuff.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        m_buttonClose.onClick.RemoveAllListeners();
        m_buttonClose.onClick.AddListener(OnClickCloseBuff);
        m_buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trImageBack = m_buttonClose.transform.Find("ImageBack");

        UpdateWorldBuff();

        Text textCondition = m_trImageBack.Find("TextCondition").GetComponent<Text>();
        Text textCondition1 = m_trImageBack.Find("TextCondition1").GetComponent<Text>();
        Text textCondition2 = m_trImageBack.Find("TextCondition2").GetComponent<Text>();
        Text textDescription = m_trImageBack.Find("TextDescription").GetComponent<Text>();

        CsUIData.Instance.SetFont(textCondition);
        CsUIData.Instance.SetFont(textCondition1);
        CsUIData.Instance.SetFont(textCondition2);
        CsUIData.Instance.SetFont(textDescription);

        textCondition.text = CsConfiguration.Instance.GetString("A73_TXT_00002");
        textCondition1.text = CsConfiguration.Instance.GetString("A73_TXT_00003");
        textCondition2.text = CsConfiguration.Instance.GetString("A73_TXT_00004");
        textDescription.text = CsConfiguration.Instance.GetString("A73_TXT_00007");

        m_trImageScrollBuff = transform.Find("ImageScrollBuff");

        m_textScrollBuff = m_trImageScrollBuff.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textScrollBuff);

        UpdateBuffCount();
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateWorldBuff()
    {
        Transform trImageBuff = m_trImageBack.transform.Find("ImageBuff");

        Text textName = trImageBuff.Find("TextName").GetComponent<Text>();
        Text textServerMaxLevel = trImageBuff.Find("TextServerMaxLevel").GetComponent<Text>();
        Text textBuff = trImageBuff.Find("TextBuff").GetComponent<Text>();

        CsUIData.Instance.SetFont(textName);
        CsUIData.Instance.SetFont(textServerMaxLevel);
        CsUIData.Instance.SetFont(textBuff);

        textName.text = CsConfiguration.Instance.GetString("A73_TXT_00001");

        // 서버 최고 레벨
        if (CsGameData.Instance.MyHeroInfo.ServerMaxLevel == 0)
        {
            // 아직 추산 중 경험치 버프 없음
            textServerMaxLevel.text = CsConfiguration.Instance.GetString("A73_TXT_00005");
            textBuff.text = CsConfiguration.Instance.GetString("A73_TXT_00006");
        }
        else
        {
            textServerMaxLevel.text = string.Format(CsConfiguration.Instance.GetString("A73_TXT_01001"), CsGameData.Instance.MyHeroInfo.ServerMaxLevel);

            if (CsGameData.Instance.MyHeroInfo.Level < 30)
            {
                // 경험치 버프 없음
                textBuff.text = CsConfiguration.Instance.GetString("A73_TXT_00006");
            }
            else
            {
                CsWorldLevelExpFactor csWorldLevelExpFactor = CsGameData.Instance.GetWorldLevelExpFactor(CsGameData.Instance.MyHeroInfo.ServerMaxLevel - CsGameData.Instance.MyHeroInfo.Level);

                if (csWorldLevelExpFactor == null)
                {
                    // 경험치 버프 없음
                    textBuff.text = CsConfiguration.Instance.GetString("A73_TXT_00006");
                }
                else
                {
                    textBuff.text = string.Format(CsConfiguration.Instance.GetString("A73_TXT_01002"), csWorldLevelExpFactor.ExpFactor * 100);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateBuffCount()
    {
        CsItem csItem = CsGameData.Instance.GetItem(CsGameData.Instance.MyHeroInfo.ExpScrollItemId);

        if (csItem == null)
        {
            m_trImageScrollBuff.gameObject.SetActive(false);
        }
        else
        {
            if (CsGameData.Instance.MyHeroInfo.ExpScrollRemainingTime - Time.realtimeSinceStartup > 0.0f)
            {
                Image imageScrollBuff = m_trImageScrollBuff.GetComponent<Image>();
                imageScrollBuff.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/ico_exp_buff_" + csItem.Level);
                m_trImageScrollBuff.gameObject.SetActive(true);
            }
            else
            {
                m_trImageScrollBuff.gameObject.SetActive(false);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateExpScrollBuffTime()
    {
        if (CsGameData.Instance.MyHeroInfo.ExpScrollRemainingTime - Time.realtimeSinceStartup > 0)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(CsGameData.Instance.MyHeroInfo.ExpScrollRemainingTime - Time.realtimeSinceStartup);
            
            if (timeSpan.TotalMinutes < 100)
            {
                m_textScrollBuff.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("0#"), timeSpan.Seconds.ToString("0#"));
            }
            else
            {
                m_textScrollBuff.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), timeSpan.Minutes.ToString("00#"), timeSpan.Seconds.ToString("0#"));
            }
        }
        else
        {
            m_textScrollBuff.text = string.Format(CsConfiguration.Instance.GetString("INPUT_TIME"), "00", "00");
            m_trImageScrollBuff.gameObject.SetActive(false);
        }
    }
}