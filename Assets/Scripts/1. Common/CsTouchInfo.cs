using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class CsTouchInfo
{
	public static CsTouchInfo Instance
	{
		get { return CsSingleton<CsTouchInfo>.GetInstance(); }
	}

	enum EnTouchStatus { None, Zoom, Drag }

	bool m_bTouching = false;

	int m_nLayerMask;
	int m_nTouchFingerId = -1;
	int m_nTouchUpdateCount = 0;

	float m_flZoomDelta;
	float m_flZoomSpan;
	float m_flTimePreviousFirstTouched;
	float m_flTimeCurrentFirstTouched;

	GameObject m_goTouchedObject = null;
	Vector3 m_vtTouchPosition = Vector3.zero;
	//Vector3 m_vtFirstTouchedPosition = Vector3.zero;
	Vector3 m_vtLastTouchedPosition = Vector3.zero;

	EnTouchStatus m_enTouchStatus = EnTouchStatus.None;

	public bool Touching { get { return m_bTouching; } }
	public float ZoomDelta { get { return m_flZoomDelta; } }
	public Vector3 TouchedScreenPosition { get { return m_vtLastTouchedPosition; } } // 드레그시 현재의 손가락의 스크릭 좌표 반환.

	public event Delegate<Vector3, GameObject, bool> EventClickByTouch;

	//----------------------------------------------------------------------------------------------------
	public void OnEventClickByTouch(Vector3 vtTouchPos, GameObject goTouchObject, bool bDoubleClick)
	{
		if (EventClickByTouch != null)
		{
			EventClickByTouch(vtTouchPos, goTouchObject, bDoubleClick);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsDoubleClick()
	{
		float fl = m_flTimeCurrentFirstTouched - m_flTimePreviousFirstTouched;
		return fl > 0 && fl < 0.4f; // 처을 클릭과 두번째 클릭 차이.
	}

	//---------------------------------------------------------------------------------------------------
	public bool ZoomByTouch()
	{
		return (m_enTouchStatus == EnTouchStatus.Zoom && m_flZoomSpan > 0);
	}

	//---------------------------------------------------------------------------------------------------
	public bool DragByTouch()
	{
		return (m_enTouchStatus == EnTouchStatus.Drag);
	}

	//---------------------------------------------------------------------------------------------------
	public CsTouchInfo()
	{
		m_flTimePreviousFirstTouched = Time.time;
		m_flTimeCurrentFirstTouched = Time.time;
		m_nLayerMask = ~(1 << LayerMask.NameToLayer("1D_G") | 1 << LayerMask.NameToLayer("2D_G") | 1 << LayerMask.NameToLayer("3D_G") | 1 << LayerMask.NameToLayer("FreeD_G") |
						 1 << LayerMask.NameToLayer("ShadowCaster") | 1 << LayerMask.NameToLayer("Ignore Raycast"));
	}

	//---------------------------------------------------------------------------------------------------
	public void Update()
	{
#if UNITY_EDITOR
		if (m_bTouching)
		{
			CheckRayCast(Input.mousePosition);
			m_nTouchUpdateCount++;
		}
#else
        UpdateMobile();
#endif
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateMobile()
	{
		if (Input.touches == null) return;

		if (m_bTouching)
		{
			Touch[] mobileTouch = Input.touches;

			for (int i = 0; i < mobileTouch.Length; i++)
			{
				if (mobileTouch[i].fingerId == m_nTouchFingerId)
				{
					CheckRayCast(mobileTouch[i].position);
					m_nTouchUpdateCount++;
					break;
				}
			}
		}
		else
		{
			if (m_flZoomSpan > 0) // ZoomPlay
			{
				m_enTouchStatus = EnTouchStatus.Zoom;
				float flZoomSpan = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
				m_flZoomDelta = flZoomSpan - m_flZoomSpan;
				m_flZoomSpan = flZoomSpan;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void CheckRayCast(Vector3 vtPos)
	{
		Camera camera = Camera.main;

		if (camera == null) return;

		Ray ray = camera.ScreenPointToRay(vtPos);
		RaycastHit hitInfo;

		if (m_nTouchUpdateCount == 0)
		{
			if (Physics.Raycast(ray.origin, ray.direction, out hitInfo, 50, m_nLayerMask))
			{
				m_goTouchedObject = hitInfo.collider.gameObject;
				//m_vtFirstTouchedPosition = m_vtLastTouchedPosition = vtPos;
				m_vtLastTouchedPosition = vtPos;

				if (m_goTouchedObject.CompareTag("NavMesh"))
				{
					m_vtTouchPosition = hitInfo.point; // 이동할 InGame 좌표.
				}
			}
		}
		else // Draging.
		{
			if (m_enTouchStatus == EnTouchStatus.None && Vector3.Distance(m_vtLastTouchedPosition, vtPos) > 5) // 5값은 손가락 이 터치 눌리는 범위를 보정.
			{
				m_enTouchStatus = EnTouchStatus.Drag;
			}
			m_vtLastTouchedPosition = vtPos;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerDown(PointerEventData eventData)
	{
		//Debug.Log("CsTouchInfo.OnPointerDown");
#if UNITY_EDITOR
		m_bTouching = true;
		m_nTouchUpdateCount = 0;
#else
        MobileTouchDown();
#endif
		m_flTimePreviousFirstTouched = m_flTimeCurrentFirstTouched;
		m_flTimeCurrentFirstTouched = Time.time;
	}

	//---------------------------------------------------------------------------------------------------
	void MobileTouchDown()
	{
		if (Input.touchCount == 1) // 첫번째 손가락 터치.
		{
			TouchStart(Input.GetTouch(0).fingerId);
		}
		else
		{
			if (Input.touchCount == 2) // 두번째 손가락 터치.
			{
				if (!m_bTouching || CsGameData.Instance.JoystickDragging) //  첫번째 터치가 UI or 조이스틱 드레깅인 경우.
				{
					TouchStart(Input.GetTouch(1).fingerId); // 터치 ID를 조이스틱입력하고 있지 않은 ID로 변경.

					if (m_enTouchStatus == EnTouchStatus.Zoom)
					{
						m_flZoomSpan = 0;
						m_enTouchStatus = EnTouchStatus.None;
					}
				}
				else
				{
					TouchEnd();
					m_flZoomSpan = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
				}
			}
			else
			{
				TouchEnd();
				m_flZoomSpan = 0;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log("CsTouchInfo.OnPointerUp " +  m_nTouchUpdateCount + " " + m_bTouching);

#if UNITY_EDITOR
		CheckTouchEvent();
		m_bTouching = false;
		m_nTouchUpdateCount = 0;
#else
        if (m_bTouching) // EnTouchStatus.Zoom 인경우 Down이 있어도 m_bTouching 상태가 false일 수 있음.
        {
			CheckTouchEvent();
            TouchEnd();
        }

        m_flZoomSpan = 0;
#endif
		//m_vtFirstTouchedPosition = Vector3.zero;.
		m_vtLastTouchedPosition = Vector3.zero;
		m_goTouchedObject = null;
		m_enTouchStatus = EnTouchStatus.None;
	}

	//---------------------------------------------------------------------------------------------------
	void CheckTouchEvent()
	{
		if (m_enTouchStatus != EnTouchStatus.None) return;

		if (m_goTouchedObject != null)
		{
			OnEventClickByTouch(m_vtTouchPosition, m_goTouchedObject, IsDoubleClick()); // 인게임에 터치 관련 정보 전달.
		}
		else
		{
			if (m_bTouching && m_nTouchUpdateCount == 0)  // Update에 한번도 걸리지 않았을때   >>>>   터치 관련 값 생성.
			{
				CheckRayCast(Input.mousePosition);
				if (m_goTouchedObject != null)
				{
					OnEventClickByTouch(m_vtTouchPosition, m_goTouchedObject, IsDoubleClick()); // 인게임에 터치 관련 정보 전달.
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void TouchStart(int nFingerId)
	{
		m_bTouching = true;
		m_nTouchUpdateCount = 0;
		m_nTouchFingerId = nFingerId;
	}

	//---------------------------------------------------------------------------------------------------
	void TouchEnd()
	{
		m_nTouchFingerId = -1;
		m_nTouchUpdateCount = -1;
		m_bTouching = false;
	}
}
