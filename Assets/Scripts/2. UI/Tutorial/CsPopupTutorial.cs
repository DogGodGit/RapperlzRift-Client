using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum EnTutorialType
{
	OpenContents = 0,
    FirstStart = 1,
    Attainment = 2,
    Cart = 3,
    MainGearEnchant = 4,
    Dialog = 6,
    Interaction = 7,
}

public class CsPopupTutorial : MonoBehaviour, ICanvasRaycastFilter, IPointerDownHandler
{
    BoxCollider2D m_boxCollider2D;

    RectTransform m_rtPopupTutorial;
    RectTransform m_rtrTextFrame;
    RectTransform m_rtrArrow;
    RectTransform m_rtrEffect;

    Transform m_trImageBlock;

    Text m_textTutorial;

    CsMenuContent m_csMenuContent;
    EnTutorialType m_enTutorialType;

    int m_nStep = 1;

    bool m_bCheck = true;
    bool m_bIsContent = true;
    bool m_bModal = false;
    bool m_bCo = false;

    IEnumerator m_enumerator;

    //---------------------------------------------------------------------------------------------------
    void Awake()
    {
		CsGameEventToUI.Instance.EventHeroDead += OnEventHeroDead;

        InitializeUI();
    }

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		CsGameEventToUI.Instance.EventHeroDead -= OnEventHeroDead;
	}

    //---------------------------------------------------------------------------------------------------
    void InitializeUI()
    {
        m_boxCollider2D = GetComponent<BoxCollider2D>();
        m_rtPopupTutorial = GetComponent<RectTransform>();

        Button buttonTutorial = GetComponent<Button>();
        buttonTutorial.onClick.RemoveAllListeners();
        buttonTutorial.onClick.AddListener(OnClickButtonSkipTutorial);
        buttonTutorial.onClick.AddListener(() => CsUIData.Instance.PlayUISound(EnUISoundType.Button));

        m_rtrTextFrame = transform.Find("ImageFrmTuto").GetComponent<RectTransform>();
        m_rtrArrow = transform.Find("Arrow").GetComponent<RectTransform>();
        m_rtrEffect = transform.Find("ImageEffect").GetComponent<RectTransform>();

        m_textTutorial = m_rtrTextFrame.Find("Text").GetComponent<Text>();
        CsUIData.Instance.SetFont(m_textTutorial);

        m_trImageBlock = transform.Find("ImageBlock");
        if (m_trImageBlock != null) m_trImageBlock.gameObject.SetActive(false);
    }

    //---------------------------------------------------------------------------------------------------
    public bool IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {
        Vector3 v3worldPoint = Vector3.zero;

        bool bInside = RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rtPopupTutorial, screenPos, eventCamera, out v3worldPoint);

        if (bInside)
            bInside = m_boxCollider2D.OverlapPoint(v3worldPoint);

        if (m_bCheck != bInside)
        {
            m_bCheck = bInside;
        }

        return !bInside;
    }

    //---------------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_enTutorialType == EnTutorialType.FirstStart && m_nStep == 2)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_bClickCheck)
                {
                    m_bClickCheck = false;
                    return;
                }

                if (m_bCo)
                {
                    return;
                }
                else if (m_bCheck && !m_bModal)
                {
                    m_nStep++;

                    if (m_bIsContent)
                    {
                        StartCoroutine(coUpdateTutorial());
                    }
                    else
                    {
                        StartCoroutine(coUpdateTypeTutorial());
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(m_bClickCheck)
            {
                m_bClickCheck = false;
                return;
            }

            if (m_bCo)
            {
                return;
            }
            else if (m_bCheck && !m_bModal)
            {
                m_nStep++;
                if (m_bIsContent)
                {
                    StartCoroutine(coUpdateTutorial());
                }
                else
                {
                    StartCoroutine(coUpdateTypeTutorial());
                }
            }
        }
    }

    bool m_bClickCheck = false;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        m_bClickCheck = true;
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator coUpdateTutorial()
    {
        m_bCo = true;
        m_rtrTextFrame.gameObject.SetActive(false);
        m_rtrArrow.gameObject.SetActive(false);
        m_rtrEffect.gameObject.SetActive(false);

        CsMenuContentTutorialStep csMenuContentTutorialStep = m_csMenuContent.MenuContentTutorialStepList.Find(a => a.Step == m_nStep);

        if (csMenuContentTutorialStep == null)
        {
            CsUIData.Instance.StopSound();
            CloseTutorial();
            yield return null;
        }
        else
        {
            m_trImageBlock.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.7f);
            UpdateTutorial(csMenuContentTutorialStep);
        }
    }

    //---------------------------------------------------------------------------------------------------
    IEnumerator coUpdateTypeTutorial()
    {
        m_bCo = true;
        m_rtrTextFrame.gameObject.SetActive(false);
        m_rtrArrow.gameObject.SetActive(false);
        m_rtrEffect.gameObject.SetActive(false);

        List<CsClientTutorialStep> ListCsClientTutorialStep = CsGameData.Instance.GetClientTutorialStepList((int)m_enTutorialType);

        if (ListCsClientTutorialStep != null)
        {
            CsClientTutorialStep csClientTutorialStep = ListCsClientTutorialStep.Find(a => a.Step == m_nStep);

            if (csClientTutorialStep != null)
            {
                m_trImageBlock.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.7f);
                UpdateTypeTutorial(csClientTutorialStep);
            }
            else
            {
                CsUIData.Instance.StopSound();
                CloseTutorial();
            }
        }
        else
        {
            CsUIData.Instance.StopSound();
            CloseTutorial();
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetTutorial(CsMenuContent csMenuContent)
    {
        m_nStep = 1;
        m_bIsContent = true;
        m_csMenuContent = csMenuContent;
		
        CsMenuContentTutorialStep csMenuContentTutorialStep = m_csMenuContent.MenuContentTutorialStepList.Find(a => a.Step == m_nStep);

        if (csMenuContentTutorialStep != null)
        {
            UpdateTutorial(csMenuContentTutorialStep);
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void SetTutorial(EnTutorialType enTutorialType)
    {
        m_nStep = 1;
        m_bIsContent = false;
        m_enTutorialType = enTutorialType;
		
        List<CsClientTutorialStep> ListCsClientTutorialStep = CsGameData.Instance.GetClientTutorialStepList((int)m_enTutorialType);

        if (ListCsClientTutorialStep != null)
        {
            CsClientTutorialStep csClientTutorialStep = ListCsClientTutorialStep.Find(a => a.Step == m_nStep);

            if (csClientTutorialStep != null)
            {
                UpdateTypeTutorial(csClientTutorialStep);
            }
        }
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateTutorial(CsMenuContentTutorialStep csMenuContentTutorialStep)
    {
        m_trImageBlock.gameObject.SetActive(false);

        m_rtrTextFrame.gameObject.SetActive(true);
        m_rtrArrow.gameObject.SetActive(true);
        m_rtrEffect.gameObject.SetActive(true);

        m_rtrTextFrame.anchoredPosition = new Vector2(csMenuContentTutorialStep.TextXPosition, csMenuContentTutorialStep.TextYPosition);

        m_textTutorial.text = csMenuContentTutorialStep.Text;

        m_rtrArrow.anchoredPosition = new Vector2(csMenuContentTutorialStep.ArrowXPosition, csMenuContentTutorialStep.ArrowYPosition);
        m_rtrArrow.eulerAngles = new Vector3(0, 0, csMenuContentTutorialStep.ArrowZRotation);

        m_rtrEffect.anchoredPosition = new Vector2(csMenuContentTutorialStep.EffectXPosition, csMenuContentTutorialStep.EffectYPosition);
        m_boxCollider2D.offset = new Vector2(csMenuContentTutorialStep.ClickXPosition, csMenuContentTutorialStep.ClickYPosition);

        m_rtrEffect.sizeDelta = new Vector2(csMenuContentTutorialStep.EffectWidth, csMenuContentTutorialStep.EffectHeight);
        m_boxCollider2D.size = new Vector2(csMenuContentTutorialStep.ClickWidth, csMenuContentTutorialStep.ClickHeight);

        m_rtrEffect.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Tutorial/" + csMenuContentTutorialStep.EffectName);
        m_rtrEffect.GetComponent<Image>().type = Image.Type.Sliced;

        CsUIData.Instance.PlayTutorialSound(EnTutorialType.OpenContents, m_csMenuContent.ContentId, m_nStep);
        m_bCo = false;
    }

    //---------------------------------------------------------------------------------------------------
    public void UpdateTypeTutorial(CsClientTutorialStep csClientTutorialStep)
    {
        if (m_trImageBlock != null) m_trImageBlock.gameObject.SetActive(false);

        m_rtrTextFrame.gameObject.SetActive(true);
        m_rtrArrow.gameObject.SetActive(true);
        m_rtrEffect.gameObject.SetActive(true);

        m_rtrTextFrame.anchoredPosition = new Vector2(csClientTutorialStep.TextXPosition, csClientTutorialStep.TextYPosition);

        m_textTutorial.text = csClientTutorialStep.Text;

        m_rtrArrow.anchoredPosition = new Vector2(csClientTutorialStep.ArrowXPosition, csClientTutorialStep.ArrowYPosition);
        m_rtrArrow.eulerAngles = new Vector3(0, 0, csClientTutorialStep.ArrowYRotation);

        m_rtrEffect.anchoredPosition = new Vector2(csClientTutorialStep.EffectXPosition, csClientTutorialStep.EffectYPosition);
        m_boxCollider2D.offset = new Vector2(csClientTutorialStep.ClickXPosition, csClientTutorialStep.ClickYPosition);

        m_rtrEffect.sizeDelta = new Vector2(csClientTutorialStep.EffectWidth, csClientTutorialStep.EffectHeight);
        m_boxCollider2D.size = new Vector2(csClientTutorialStep.ClickWidth, csClientTutorialStep.ClickHeight);

        m_rtrEffect.GetComponent<Image>().sprite = CsUIData.Instance.LoadAsset<Sprite>("GUI/Tutorial/" + csClientTutorialStep.EffectName);
        m_rtrEffect.GetComponent<Image>().type = Image.Type.Sliced;

        CsUIData.Instance.PlayTutorialSound(m_enTutorialType, (int)m_enTutorialType, m_nStep);

        m_bCo = false;

        if (m_enTutorialType == EnTutorialType.Dialog || m_enTutorialType == EnTutorialType.Interaction)
            CsGameEventToUI.Instance.OnEventJoystickReset();
    }

    //---------------------------------------------------------------------------------------------------
    void CloseTutorial()
    {
		CsGameEventUIToUI.Instance.OnEventTutorialEnd();
        Destroy(gameObject);
    }

    //---------------------------------------------------------------------------------------------------
    void OnClickButtonSkipTutorial()
    {
        m_bModal = true;

        CsGameEventUIToUI.Instance.OnEventConfirm(CsConfiguration.Instance.GetString("A69_TXT_00001"), CsConfiguration.Instance.GetString("PUBLIC_BTN_YES"), CloseTutorial, CsConfiguration.Instance.GetString("PUBLIC_BTN_NO"), TutorialSkipCancel, true);
    }

    //---------------------------------------------------------------------------------------------------
    void TutorialSkipCancel()
    {
        m_bModal = false;
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventHeroDead(string strName)
	{
		Destroy(gameObject);
	}
}