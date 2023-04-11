using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CsUIJoyStick : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	RectTransform m_rtfJoystick;
    RectTransform m_rtfHandle;
	RectTransform m_rtfLine;
	Transform m_trJoystickEffect;
	
    Vector2 m_vtResolutionRatio;
	Vector2 m_vtJoystickPanelHalfSize;
	Vector2 m_vtJoystickStart;
	Vector2 m_vtPointerDown;

	float m_flRange;

	bool m_bJoystic = false;
    bool m_bJoysticEvent = false;
	bool m_bDisplayJoystickEffect = false;

	const float m_flLineWidth = 2.0f;

	int m_nDefaultDragThreshold = 0;
	int m_nJoystickDragThresholdPixel;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		CsGameEventToUI.Instance.EventJoystickReset += OnEventJoystickReset;
		CsGameEventToUI.Instance.EventChangeResolution += OnEventChangeResolution;
		CsGameEventToUI.Instance.EventJoystickStartAutoMove += OnEventJoystickStartAutoMove;
		CsGameEventUIToUI.Instance.EventDisplayJoystickEffect += OnEventDisplayJoystickEffect;

		m_nDefaultDragThreshold = EventSystem.current.pixelDragThreshold;
	}

    //---------------------------------------------------------------------------------------------------
    void Start()
    {
		m_rtfJoystick = transform.Find("Joystick").GetComponent<RectTransform>();
		m_rtfHandle = transform.Find("Joystick/Handle").GetComponent<RectTransform>();
		m_rtfLine = transform.Find("Line").GetComponent<RectTransform>();
		m_trJoystickEffect = m_rtfJoystick.Find("JoystickEffect");

		m_vtJoystickPanelHalfSize = transform.GetComponent<RectTransform>().sizeDelta * 0.5f;
		m_vtJoystickStart = m_rtfJoystick.anchoredPosition;

        m_flRange = Mathf.Abs(transform.Find("Joystick/Handle").GetComponent<RectTransform>().sizeDelta.x);
    }

    //---------------------------------------------------------------------------------------------------
    void OnDestroy()
    {
		m_rtfJoystick = null;
        m_rtfHandle = null;
        CsGameEventToUI.Instance.EventJoystickReset -= OnEventJoystickReset;
		CsGameEventToUI.Instance.EventChangeResolution -= OnEventChangeResolution;
		CsGameEventToUI.Instance.EventJoystickStartAutoMove -= OnEventJoystickStartAutoMove;
		CsGameEventUIToUI.Instance.EventDisplayJoystickEffect -= OnEventDisplayJoystickEffect;
    }

    //---------------------------------------------------------------------------------------------------
    void IPointerDownHandler.OnPointerDown(PointerEventData pointerEventData)
	{
		m_vtPointerDown = new Vector2(pointerEventData.position.x * m_vtResolutionRatio.x, pointerEventData.position.y * m_vtResolutionRatio.y) -m_vtJoystickPanelHalfSize;
		SetJoystickPosition(m_vtPointerDown);
		m_rtfLine.gameObject.SetActive(true);

		CsGameData.Instance.JoystickDown = true;

		if (EventSystem.current != null)
		{
			EventSystem.current.pixelDragThreshold = m_nJoystickDragThresholdPixel;
		}

		DisplayJoystickEffect();
    }

    //---------------------------------------------------------------------------------------------------
	void IDragHandler.OnDrag(PointerEventData pointerEventData)
    {
		if (!m_bJoysticEvent)
		{
			if (!m_bJoystic)
			{
				CsGameData.Instance.JoystickDown = false;
				CsGameData.Instance.JoystickDragging = m_bJoystic = true;
			}

			Vector2 vt = new Vector2(pointerEventData.position.x * m_vtResolutionRatio.x, pointerEventData.position.y * m_vtResolutionRatio.y) - m_vtJoystickPanelHalfSize;
			Vector2 vt2 = vt - m_rtfJoystick.anchoredPosition;
			SetHandlePosition(vt2);

			DrawLine(vt);
		}

		DisplayJoystickEffect();
    }

	//---------------------------------------------------------------------------------------------------
	void SetJoystickPosition(Vector2 vtPoint)
	{
		m_rtfJoystick.anchoredPosition = vtPoint;
		m_flRange = Mathf.Abs(transform.Find("Joystick/Handle").GetComponent<RectTransform>().sizeDelta.x);
	}

    //---------------------------------------------------------------------------------------------------
    void SetHandlePosition(Vector2 vtPoint)
    {
		float flValue = Vector2.Distance(Vector2.zero, vtPoint);

		if (flValue < m_flRange * 0.5f)
		{
			CsGameData.Instance.JoysticWalk = true;
		}
		else
		{
			CsGameData.Instance.JoysticWalk = false;
		}

		m_rtfHandle.anchoredPosition = vtPoint;

		CsGameData.Instance.JoystickAngle = Mathf.Atan2(m_rtfHandle.anchoredPosition.y, m_rtfHandle.anchoredPosition.x);
    }

    //---------------------------------------------------------------------------------------------------
    void IEndDragHandler.OnEndDrag(PointerEventData data)
    {
		m_bJoysticEvent = false;
        Reset();
    }

    //---------------------------------------------------------------------------------------------------
    void IPointerUpHandler.OnPointerUp(PointerEventData pointerEventData)
    {
        m_bJoysticEvent = false;
        Reset();
    }

    //---------------------------------------------------------------------------------------------------
    void OnEventJoystickReset()
	{
		if (m_bJoystic)
		{
			m_bJoysticEvent = true;
		}
        Reset();
    }

	//---------------------------------------------------------------------------------------------------
	void OnEventJoystickStartAutoMove()
	{
		const float m_flInchToCm = 2.54f;
		const float m_flJoysitckDragThresholdInch = 0.5f;
		EventSystem.current.pixelDragThreshold = (int)(m_flJoysitckDragThresholdInch * Screen.dpi / m_flInchToCm);
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventChangeResolution()
	{
		StartCoroutine(ChangeResolution());
	}

	//---------------------------------------------------------------------------------------------------
	public void OnEventDisplayJoystickEffect(bool bEnable)
	{
		m_bDisplayJoystickEffect = bEnable;
		DisplayJoystickEffect();
	}

	//---------------------------------------------------------------------------------------------------
	void DrawLine(Vector2 vtPoint)
	{
		Vector2 vt = vtPoint - m_vtPointerDown;
		//m_rtfLine.sizeDelta = new Vector2(Mathf.Clamp(vt.magnitude, 0, m_flRange), m_flLineWidth);
		m_rtfLine.sizeDelta = new Vector2(vt.magnitude, m_flLineWidth);
		m_rtfLine.pivot = new Vector2(0, 0.5f);
		m_rtfLine.anchoredPosition = m_vtPointerDown;

		float flAngle = Mathf.Atan2(vt.y, vt.x) * Mathf.Rad2Deg;
		m_rtfLine.rotation = Quaternion.Euler(0, 0, flAngle);
	}

	//---------------------------------------------------------------------------------------------------
	public void Reset()
	{
		CsGameData.Instance.JoystickDown = false;

		m_vtPointerDown = Vector2.zero;

		CsGameData.Instance.JoystickDragging = m_bJoystic = false;
		m_rtfJoystick.anchoredPosition = m_vtJoystickStart;
		m_rtfHandle.anchoredPosition = Vector2.zero;
		CsGameData.Instance.JoysticWalk = false;

		m_rtfLine.sizeDelta = Vector2.zero;
		m_rtfLine.gameObject.SetActive(false);

		if (EventSystem.current != null)
		{
			EventSystem.current.pixelDragThreshold = m_nDefaultDragThreshold;
		}

		DisplayJoystickEffect();
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ChangeResolution()
	{	
		yield return null;

		Vector2 vtCanvasResolution = GameObject.Find("Canvas").GetComponent<CanvasScaler>().referenceResolution;
		m_vtResolutionRatio = new Vector2(vtCanvasResolution.x / Screen.width, vtCanvasResolution.y / Screen.height);

		const float m_flInchToCm = 2.54f;
		const float m_flJoysitckDragThresholdInch = 0.1f;

		m_nJoystickDragThresholdPixel = (int)(m_flJoysitckDragThresholdInch * Screen.dpi / m_flInchToCm);
	}

	//---------------------------------------------------------------------------------------------------
	void DisplayJoystickEffect()
	{
		if (m_trJoystickEffect != null)
			m_trJoystickEffect.gameObject.SetActive(m_bDisplayJoystickEffect && !CsGameData.Instance.JoystickDown && !CsGameData.Instance.JoystickDragging);
	}
}