using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsRotateCharacter : MonoBehaviour
{
    bool m_bTrueZone = false;
    Camera m_camera;
    CsIntroCharacterTimeline m_csIntroCharacterTimeline;
    Transform m_trParent = null;

    //----------------------------------------------------------------------------------------------
    void Awake()
    {
        m_trParent = transform.parent;
        SetCharacterCamera();
        SetTimeline();
    }

    //----------------------------------------------------------------------------------------------
    void OnEnable()
    {
        if (m_camera == null)
        {
            SetCharacterCamera();
            return;
        }

        if (m_csIntroCharacterTimeline == null)
        {
            SetTimeline();
        }
    }

    //----------------------------------------------------------------------------------------------
    void Update()
    {
        if (m_camera == null)
        {
            SetCharacterCamera();
            return;
        }

        if (m_csIntroCharacterTimeline == null || !m_csIntroCharacterTimeline.IsPlaying())
        {
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit, 100))
				{
					if (hit.transform.tag == "Player")
					{
						m_bTrueZone = true;
					}
					else
					{
						m_bTrueZone = false;
					}
				}
			}

			if (Input.GetMouseButton(0))
			{
				if (m_bTrueZone)
				{
					float m_flYDeg = transform.eulerAngles.y - Input.GetAxis("Mouse X") * 8;
					Quaternion quaternionFrom = transform.rotation;
					Quaternion quaternionTo = Quaternion.Euler(0, m_flYDeg, 0);
					transform.rotation = Quaternion.Lerp(quaternionFrom, quaternionTo, Time.deltaTime * 60);
				}
			}
			else
			{
				m_bTrueZone = false;
			}
        }
    }

    //----------------------------------------------------------------------------------------------
    void SetCharacterCamera()
    {
        m_camera = m_trParent.Find("IntroCamera/CharacterCamera").GetComponent<Camera>();
    }

    //----------------------------------------------------------------------------------------------
    void SetTimeline()
    {
        m_csIntroCharacterTimeline = m_trParent.GetComponent<CsIntroCharacterTimeline>();
    }
}
