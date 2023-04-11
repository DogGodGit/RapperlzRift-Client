using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsUICharcterRotate : MonoBehaviour {

    Camera m_camera;

    float m_flYDeg = -180f;
    bool m_bTrueZone = false;

    public Camera UICamera
    {
        set { m_camera = value; }
    }

    void Update ()
    {
        if (m_camera != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (hit.transform == transform)
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
                    m_flYDeg -= Input.GetAxis("Mouse X") * 6;
                    Quaternion quaternionFrom = transform.rotation;
                    Quaternion quaternionTo = Quaternion.Euler(0, m_flYDeg, 0);
                    transform.rotation = Quaternion.Lerp(quaternionFrom, quaternionTo, Time.deltaTime * 30);
                }
            }
            else
            {
                m_bTrueZone = false;
            }
        }
    }
}
