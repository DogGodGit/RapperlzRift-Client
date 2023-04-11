using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationTransmission : MonoBehaviour
{
    [SerializeField] GameObject m_goButtonNationTransmission;

    Transform m_trImageBackground;
    Transform m_trNationTransmissionList;

    int m_nNpcId;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup += OnEventCloseAllPopup;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept += OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted += OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceBreak += OnEventNationAllianceBreak;
        CsNationAllianceManager.Instance.EventNationAllianceBroken += OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded += OnEventNationAllianceConcluded;

        InitiallizeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventUIToUI.Instance.EventCloseAllPopup -= OnEventCloseAllPopup;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccept -= OnEventNationAllianceApplicationAccept;
        CsNationAllianceManager.Instance.EventNationAllianceApplicationAccepted -= OnEventNationAllianceApplicationAccepted;
        CsNationAllianceManager.Instance.EventNationAllianceBreak -= OnEventNationAllianceBreak;
        CsNationAllianceManager.Instance.EventNationAllianceBroken -= OnEventNationAllianceBroken;
        CsNationAllianceManager.Instance.EventNationAllianceConcluded -= OnEventNationAllianceConcluded;
    }

    #region EventHandler

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseAllPopup()
    {
        ClosePopupNationTransmission();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccept(CsNationAlliance csNationAlliance)
    {
        UpdateNationAlliance();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceApplicationAccepted()
    {
        UpdateNationAlliance();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceBreak()
    {
        UpdateNationAlliance();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceBroken()
    {
        UpdateNationAlliance();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventNationAllianceConcluded(CsNationAlliance csNationAlliance)
    {
        UpdateNationAlliance();
    }

    //---------------------------------------------------------------------------------------------------
    void ClosePopupNationTransmission()
    {
        transform.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNationTransmission(int nNationId)
    {
        if (CsGameData.Instance.MyHeroInfo.Level < CsGameConfig.Instance.NationTransmissionRequiredHeroLevel)
        {
            // 국가 이동 실패
            CsGameEventUIToUI.Instance.OnEventAlert(string.Format(CsConfiguration.Instance.GetString("A44_TXT_03001"), CsGameConfig.Instance.NationTransmissionRequiredHeroLevel));
        }
        else
        {
            ClosePopupNationTransmission();

            CsGameData.Instance.MyHeroInfo.MyHeroEnterType = EnMyHeroEnterType.NationTransmission;
            CsCommandEventManager.Instance.SendNationTransmission(m_nNpcId, nNationId);
        }
    }

    #endregion EventHandler

    //---------------------------------------------------------------------------------------------------
    void InitiallizeUI()
    {
        Button buttonClose = transform.Find("ButtonClose").GetComponent<Button>();
        buttonClose.onClick.RemoveAllListeners();
        buttonClose.onClick.AddListener(() => ClosePopupNationTransmission());
        buttonClose.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_trImageBackground = transform.Find("ImageBackground");

        Text textPopupName = m_trImageBackground.Find("ImageGlow/TextPopupName").GetComponent<Text>();
        CsUIData.Instance.SetFont(textPopupName);
        textPopupName.text = CsConfiguration.Instance.GetString("A44_TXT_00001");

        Text textCurrentNation = m_trImageBackground.Find("TextCurrentNation").GetComponent<Text>();
        CsUIData.Instance.SetFont(textCurrentNation);

        m_trNationTransmissionList = m_trImageBackground.Find("NationTransmissionList");

        // 국가 버튼을 만들고 초기화
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];

            GameObject goButtonNationTransmission = Instantiate(m_goButtonNationTransmission, m_trNationTransmissionList);

            Transform trButtonNationTransmission = goButtonNationTransmission.transform;
            trButtonNationTransmission.name = "ButtonNationTransmission" + csNation.NationId;

            Image imageNation = trButtonNationTransmission.Find("ImageNation").GetComponent<Image>();
            imageNation.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csNation.NationId);

            Text textNation = trButtonNationTransmission.Find("TextNation").GetComponent<Text>();
            CsUIData.Instance.SetFont(textNation);
            textNation.text = csNation.Name;

            Text textOwnNation = trButtonNationTransmission.Find("TextOwnNation").GetComponent<Text>();
            CsUIData.Instance.SetFont(textOwnNation);

            // 본국(자기 자신 국가)
            if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
            {
                textOwnNation.text = CsConfiguration.Instance.GetString("A44_TXT_00003");
                textOwnNation.color = new Color32(127, 213, 246, 255);
                textOwnNation.gameObject.SetActive(true);
            }
            else if (csNation.NationId == CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId))
            {
                textOwnNation.text = CsConfiguration.Instance.GetString("PUBLIC_ALLYDM");
                textOwnNation.color = new Color32(255, 214, 80, 255);
                textOwnNation.gameObject.SetActive(true);
            }
            else
            {
                textOwnNation.gameObject.SetActive(false);
            }

            int nNationId = CsGameData.Instance.NationList[i].NationId;
            Button buttonNationTransmission = trButtonNationTransmission.GetComponent<Button>();
            buttonNationTransmission.onClick.RemoveAllListeners();
            buttonNationTransmission.onClick.AddListener(() => OnClickNationTransmission(nNationId));
            buttonNationTransmission.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));
        }
    }

    //---------------------------------------------------------------------------------------------------
    void UpdateNationAlliance()
    {
        Transform trButtonNationTransmission = null;

        // 국가 버튼을 만들고 초기화
        for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
        {
            CsNation csNation = CsGameData.Instance.NationList[i];

            trButtonNationTransmission = m_trNationTransmissionList.Find("ButtonNationTransmission" + csNation.NationId);

            if (trButtonNationTransmission == null)
            {
                continue;
            }
            else
            {
                Text textOwnNation = trButtonNationTransmission.Find("TextOwnNation").GetComponent<Text>();

                // 본국(자기 자신 국가)
                if (csNation.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId)
                {
                    textOwnNation.text = CsConfiguration.Instance.GetString("A44_TXT_00003");
                    textOwnNation.color = new Color32(127, 213, 246, 255);
                    textOwnNation.gameObject.SetActive(true);
                }
                else if (csNation.NationId == CsNationAllianceManager.Instance.GetNationAllianceId(CsGameData.Instance.MyHeroInfo.Nation.NationId))
                {
                    textOwnNation.text = CsConfiguration.Instance.GetString("PUBLIC_ALLYDM");
                    textOwnNation.color = new Color32(255, 214, 80, 255);
                    textOwnNation.gameObject.SetActive(true);
                }
                else
                {
                    textOwnNation.gameObject.SetActive(false);
                }
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdatePopupNationTransmission(CsNpcInfo csNpcInfo)
    {
        if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam == 0)
        {
            return;
        }
        else
        {
            // 현재 국가 이름 텍스트
            CsNation csNation = CsGameData.Instance.GetNation(CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam);

            Text textCurrentNation = m_trImageBackground.Find("TextCurrentNation").GetComponent<Text>();
            CsUIData.Instance.SetFont(textCurrentNation);
            textCurrentNation.text = string.Format(CsConfiguration.Instance.GetString("A44_TXT_00002"), csNation.Name);

            for (int i = 0; i < CsGameData.Instance.NationList.Count; i++)
            {
                Transform trButtonNationTransmission = m_trNationTransmissionList.Find("ButtonNationTransmission" + CsGameData.Instance.NationList[i].NationId);

                // 현재 국가 비교
                if (csNation.NationId == CsGameData.Instance.NationList[i].NationId)
                {
                    trButtonNationTransmission.gameObject.SetActive(false);
                }
                // 현재 국가가 아니면
                else
                {
                    trButtonNationTransmission.gameObject.SetActive(true);
                }
            }

            m_nNpcId = csNpcInfo.NpcId;
        }

        UpdateNationAlliance();
    }
}