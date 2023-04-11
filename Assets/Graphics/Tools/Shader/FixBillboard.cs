using UnityEngine;
using System.Collections;

public class FixBillboard : MonoBehaviour 
{
    public Camera m_camera;
    public bool m_flFixedX = false;
    public bool m_flFixedY = false;
	public bool m_flFixedZ = false;
	bool m_bVisible = false;

	void Update()
    {
		if (m_bVisible)
		{
			if (m_camera == null)
			{
				m_camera = Camera.main;
			}
			else
			{
				if (m_flFixedX || m_flFixedY || m_flFixedZ)
				{
					Vector3 fixedVector = m_camera.transform.position - transform.position;
					fixedVector.x = (m_flFixedX) ? fixedVector.x : 0;
					fixedVector.y = (m_flFixedY) ? fixedVector.y : 0;
					fixedVector.z = (m_flFixedZ) ? fixedVector.z : 0;

					transform.LookAt(m_camera.transform.position - fixedVector);
				}
				else
				{
					transform.LookAt(m_camera.transform.position);
				}
			}
		}
    }

	//---------------------------------------------------------------------------------------------------
	void OnBecameVisible()
	{
		m_bVisible = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnBecameInvisible()
	{
		m_bVisible = false;
	}
}
