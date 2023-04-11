using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupNationWarAttend : MonoBehaviour
{
    Transform m_trImageBackground;

    float m_flTime = 0.0f;
    float m_flRemainingTime = 0.0f;

    public event Delegate EventCloseNationWarAttend;
    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        InitializeUI();
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_flTime + 1.0f < Time.time)
        {
            if (m_flRemainingTime - Time.realtimeSinceStartup < 0.0f)
            {
                OnEventCloseNationWarAttend();
            }

            m_flTime = Time.time;
        }
    }

    #region Event

    //---------------------------------------------------------------------------------------------------
    void OnEventCloseNationWarAttend()
    {
        if (EventCloseNationWarAttend != null)
        {
            EventCloseNationWarAttend();
        }
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNonAttend()
    {
        OnEventCloseNationWarAttend();
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickAttend()
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.GetMyHeroNationWarDeclaration();

        if (csNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            // Status == 1 : 전쟁중 && 영웅이 살아 있음 && 카트 타고 있는 상태가 아님 && 영웅 레벨이 pvp 최소 레벨보다 높음
            if (csNationWarDeclaration.Status == EnNationWarDeclaration.Current && !CsIngameData.Instance.MyHeroDead && !CsCartManager.Instance.IsMyHeroRidingCart &&
                CsGameConfig.Instance.PvpMinHeroLevel <= CsGameData.Instance.MyHeroInfo.Level)
            {
                // Join 가능
                CsNationWarManager.Instance.SendNationWarJoin();
                CsGameEventUIToUI.Instance.OnEventCloseAllPopup();
            }
        }

        OnEventCloseNationWarAttend();
    }

    #endregion Event

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        CsNationWarDeclaration csNationWarDeclaration = CsNationWarManager.Instance.NationWarDeclarationList.Find(a => a.NationId == CsGameData.Instance.MyHeroInfo.Nation.NationId || a.TargetNationId == CsGameData.Instance.MyHeroInfo.Nation.NationId);

        // 국가전 선전이 없음
        if (csNationWarDeclaration == null)
        {
            return;
        }
        else
        {
            CsNation csNationOffense = CsGameData.Instance.GetNation(csNationWarDeclaration.NationId);
            CsNation csNationDefense = CsGameData.Instance.GetNation(csNationWarDeclaration.TargetNationId);

            if (csNationOffense == null || csNationDefense == null)
            {
                return;
            }
            else
            {
                m_trImageBackground = transform.Find("ImageBackground");

                Transform trImageGlow = m_trImageBackground.Find("ImageGlow");

                // 팝업 이름
                Text textPopupName = trImageGlow.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textPopupName);
                textPopupName.text = CsConfiguration.Instance.GetString("A70_TXT_00001");

                Transform trNationWarInfo = m_trImageBackground.Find("NationWarInfo");

                // 공격 국가
                Image imageNationOffense = trNationWarInfo.Find("ImageNationOffense").GetComponent<Image>();
                imageNationOffense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csNationOffense.NationId);

                Text textNationOffense = imageNationOffense.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNationOffense);
                textNationOffense.text = csNationOffense.Name;

                // 수비 국가
                Image imageNationDefense = trNationWarInfo.Find("ImageNationDefense").GetComponent<Image>();
                imageNationDefense.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Common/ico_nation" + csNationDefense.NationId);

                Text textNationDefense = imageNationDefense.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textNationDefense);
                textNationDefense.text = csNationDefense.Name;

                Transform trImageAttend = m_trImageBackground.Find("ImageAttend");

                Text textAttend = trImageAttend.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textAttend);
                textAttend.text = CsConfiguration.Instance.GetString("A70_TXT_00002");

                Button buttonNonAttend = trImageAttend.Find("ButtonNonAttend").GetComponent<Button>();
                buttonNonAttend.onClick.RemoveAllListeners();
                buttonNonAttend.onClick.AddListener(OnClickNonAttend);
                buttonNonAttend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textButtonNonAttend = buttonNonAttend.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButtonNonAttend);
                textButtonNonAttend.text = CsConfiguration.Instance.GetString("A70_BTN_00001");

                Button buttonAttend = trImageAttend.Find("ButtonAttend").GetComponent<Button>();
                buttonAttend.onClick.RemoveAllListeners();
                buttonAttend.onClick.AddListener(OnClickAttend);
                buttonAttend.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

                Text textButtonAttend = buttonAttend.transform.Find("Text").GetComponent<Text>();
                CsUIData.Instance.SetFont(textButtonAttend);
                textButtonAttend.text = CsConfiguration.Instance.GetString("A70_BTN_00002");

                m_flRemainingTime = CsGameData.Instance.NationWar.JoinPopupDisplayDuration + Time.realtimeSinceStartup;
            }
        }
    }
}