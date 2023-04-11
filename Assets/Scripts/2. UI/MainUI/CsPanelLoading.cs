using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPanelLoading : MonoBehaviour
{
    bool m_bLoadingComplete;
    bool m_bTutorialStart = true;

    Text m_textLoadingPercent;
    Slider m_sliderLoading;

    IEnumerator m_IEnumeratorLoad;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
        //로딩창
        m_textLoadingPercent = transform.Find("TextPercent").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textLoadingPercent);

        m_sliderLoading = transform.Find("Slider").GetComponent<Slider>();
        m_sliderLoading.maxValue = 100f;

        Text textLoading = transform.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(textLoading);

        CsGameEventToUI.Instance.EventSceneLoadComplete += OnEventSceneLoadComplete;    // 인게임 로딩완료
        CsGameEventUIToUI.Instance.EventHeroLogin += OnEventHeroLogin;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEnable()
    {
        Text textLoading = transform.Find("Text").GetComponent<Text>();

        if (CsUIData.Instance.FirstLoading)
        {
            textLoading.text = CsConfiguration.Instance.GetString("A01_TXT_00052");
            CsUIData.Instance.FirstLoading = false;
        }
        else
        {
            textLoading.text = CsConfiguration.Instance.GetString("A01_TXT_00053");
        }

        int nImageNo = UnityEngine.Random.Range(1, 5);
		Image imageLoading = transform.Find("ImageLoading").GetComponent<Image>();

		imageLoading.sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/MainUI/img_loadingimg" + nImageNo.ToString("00"));

        if (m_IEnumeratorLoad != null)
        {
            StopCoroutine(m_IEnumeratorLoad);
            m_IEnumeratorLoad = null;
        }

		m_bLoadingComplete = false;
		m_IEnumeratorLoad = LoadingSliderCoroutine();
        StartCoroutine(m_IEnumeratorLoad);

		CsGameEventUIToUI.Instance.OnEventSleepModeReset();
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
        CsGameEventToUI.Instance.EventSceneLoadComplete -= OnEventSceneLoadComplete;    // 인게임 로딩완료
        CsGameEventUIToUI.Instance.EventHeroLogin -= OnEventHeroLogin;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventSceneLoadComplete(bool bSceneLoad)
    {
		m_bLoadingComplete = true;
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventHeroLogin()
    {
        m_bTutorialStart = true;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator LoadingSliderCoroutine()
    {
        m_textLoadingPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), "0");
        m_sliderLoading.maxValue = 100;
        m_sliderLoading.value = 0;

        while (m_sliderLoading.value < 100)
        {
            if (m_bLoadingComplete)
            {
				m_sliderLoading.value += 5;

                if (m_sliderLoading.value >= 100)
                {
                    m_sliderLoading.value = 100;
                }
            }
            else
            {
                if (m_sliderLoading.value >= 90)
                {
                    m_sliderLoading.value = 90;
                }
                else
                {
                    m_sliderLoading.value += 1;
                }
            }

            m_textLoadingPercent.text = string.Format(CsConfiguration.Instance.GetString("A01_TXT_01003"), m_sliderLoading.value);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

		transform.Find("ImageLoading").GetComponent<Image>().sprite = null;
		transform.gameObject.SetActive(false);
        CsGameEventUIToUI.Instance.OnEventLoadingSliderComplete();
        CsGameEventToIngame.Instance.OnEventLoadingUIComplete();

        if (CsGameData.Instance.MyHeroInfo.IsCreateEnter && m_bTutorialStart)
        {
            m_bTutorialStart = false;
            CsGameEventToUI.Instance.OnEventStartTutorial();
        }

		Resources.UnloadUnusedAssets();
        m_bLoadingComplete = false;

		//---------------------------------------------------------------------------------------------------
		// 헬프시프트 공지사항
		CsHelpshiftManager.Instance.DisplayHelpshift(EnHelpshiftType.FirstAccessNotice);
    }
}
