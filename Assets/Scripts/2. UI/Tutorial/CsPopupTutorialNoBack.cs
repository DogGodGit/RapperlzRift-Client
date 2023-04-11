using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CsPopupTutorialNoBack : MonoBehaviour
{
    Text m_textTutorial;
    RectTransform m_rtrTextFrame;
    RectTransform m_rtrArrow;
    RectTransform m_rtrimageTarget;

    int m_nStep = 1;
    //Sprite m_spriteSquare;
    //Sprite m_spriteCircle;
    //float m_flLifeTime = 0;
    //bool m_bCheck = false;

    EnTutorialType m_enTutorialType;
    CsClientTutorialStep m_csClientTutorialStep;


    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
		CsGameEventToUI.Instance.EventHeroDead += OnEventHeroDead;

        m_rtrTextFrame = transform.Find("ImageFrmTuto").GetComponent<RectTransform>();
        m_textTutorial = m_rtrTextFrame.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTutorial);
        m_rtrArrow = transform.Find("Arrow").GetComponent<RectTransform>();
        m_rtrimageTarget = transform.Find("ImageTarget").GetComponent<RectTransform>();

        //m_spriteSquare = CsUIData.Instance.LoadAsset<Sprite>("GUI/Tutorial/frm_tuto_square");
        //m_spriteCircle = CsUIData.Instance.LoadAsset<Sprite>("GUI/Tutorial/frm_tuto_circle");
    }

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventToUI.Instance.EventHeroDead -= OnEventHeroDead;
	}

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Input.mousePosition.x > (m_csClientTutorialStep.ClickXPosition - (m_csClientTutorialStep.ClickWidth / 2) + (Screen.width / 2)) && Input.mousePosition.x < (m_csClientTutorialStep.ClickXPosition + (m_csClientTutorialStep.ClickWidth / 2) + (Screen.width / 2))
                && Input.mousePosition.y > (m_csClientTutorialStep.ClickYPosition - (m_csClientTutorialStep.ClickHeight / 2) + (Screen.height / 2)) && Input.mousePosition.y < (m_csClientTutorialStep.ClickYPosition + (m_csClientTutorialStep.ClickHeight / 2)) + (Screen.height / 2))
            {
                CloseTutorial();
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetTutorial(EnTutorialType enTutorialType)
    {
        //m_flLifeTime = 5f + Time.realtimeSinceStartup;
        m_enTutorialType = enTutorialType;
        m_nStep = 1;
        UpdateTypeTutorial();
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateTypeTutorial()
    {
        List<CsClientTutorialStep> ListCsClientTutorialStep = CsGameData.Instance.GetClientTutorialStepList((int)m_enTutorialType);

        if (ListCsClientTutorialStep == null)
        {
            CsUIData.Instance.StopSound();
            CloseTutorial();
            return;
        }

        m_csClientTutorialStep = ListCsClientTutorialStep.Find(a => a.Step == m_nStep);

        if (m_csClientTutorialStep == null)
        {
            CsUIData.Instance.StopSound();
            CloseTutorial();
            return;
        }

        m_rtrTextFrame.anchoredPosition = new Vector2(m_csClientTutorialStep.TextXPosition, m_csClientTutorialStep.TextYPosition);

        m_textTutorial.text = m_csClientTutorialStep.Text;

        m_rtrArrow.anchoredPosition = new Vector2(m_csClientTutorialStep.ArrowXPosition, m_csClientTutorialStep.ArrowYPosition);
        m_rtrArrow.eulerAngles = new Vector3(0, 0, m_csClientTutorialStep.ArrowYRotation);

        m_rtrimageTarget.anchoredPosition = new Vector2(m_csClientTutorialStep.ClickXPosition, m_csClientTutorialStep.ClickYPosition);
        m_rtrimageTarget.sizeDelta = new Vector2(m_csClientTutorialStep.ClickWidth, m_csClientTutorialStep.ClickHeight);

        m_rtrimageTarget.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Tutorial/" + m_csClientTutorialStep.EffectName);
        m_rtrimageTarget.GetComponent<Image>().type = Image.Type.Sliced;

        CsUIData.Instance.PlayTutorialSound(m_enTutorialType, (int)m_enTutorialType, m_nStep);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickNextTutorial()
    {
        CloseTutorial();
    }

    //---------------------------------------------------------------------------------------------------
    void CloseTutorial()
    {
		CsGameEventUIToUI.Instance.OnEventTutorialEnd();
        Destroy(gameObject);
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDead(string strName)
	{
		Destroy(gameObject);
	}
}
