using UnityEngine;

public class CsRotateCamera : MonoBehaviour
{
	Transform m_trTarget;

	[SerializeField]
	Vector3 m_vtOffset = Vector3.zero;
	[SerializeField]
	Vector3 m_vtZoomOffset = Vector3.zero;

	float m_flCameraRotSide;
	float m_flCameraRotUp;
	float m_flCameraRotSideCur;
	float m_flCameraRotUpCur;

	[SerializeField]
	float m_flDistance;

	float m_flFarDistance;

	[SerializeField]
	bool m_bCreate = false;

	public float Distance { get { return m_flDistance; } set { m_flDistance = value; } }

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		m_trTarget = transform.parent.parent.Find("Character");
		
		if (m_bCreate)
		{
			Distance = m_flDistance = 3.8f;
			transform.position = m_trTarget.position + m_vtOffset + (m_trTarget.forward * m_flDistance);
			transform.LookAt(m_trTarget);
		}
		else
		{
			m_flFarDistance = m_flDistance = Vector3.Distance(transform.position, m_trTarget.position + m_vtOffset);
		}

		m_flCameraRotSide = m_flCameraRotSideCur = transform.eulerAngles.y;
		m_flCameraRotUp = m_flCameraRotUpCur = transform.eulerAngles.x;
		Debug.Log("CsRotateCamera : " + Vector3.Distance(transform.position, m_trTarget.position + m_vtOffset));
	}

	//---------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		if (Input.GetMouseButton(0))
		{
			Ray cursorRay = transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit;
			if (Physics.Raycast(cursorRay, out raycastHit, 50))
			{
				m_flCameraRotSide += Input.GetAxis("Mouse X") * 10;
				m_flCameraRotUp -= Input.GetAxis("Mouse Y") * 10;
			}

			m_flCameraRotSideCur = Mathf.LerpAngle(m_flCameraRotSideCur, m_flCameraRotSide, Time.deltaTime * 10); 
			m_flCameraRotUpCur = Mathf.Lerp(m_flCameraRotUpCur, m_flCameraRotUp, Time.deltaTime * 10); 
		}

		if (m_flCameraRotUpCur < 0)
		{
			m_flCameraRotUpCur = m_flCameraRotUp = 0f;
		}
		else if (m_flCameraRotUpCur > 55)
		{
			m_flCameraRotUpCur = m_flCameraRotUp = 55f;
		}

		Vector3 vtCenterPos = m_bZoomCamera? m_trTarget.position + m_vtZoomOffset :m_trTarget.position + m_vtOffset;
		Quaternion qtn = Quaternion.Euler(m_flCameraRotUpCur, m_flCameraRotSideCur, 0);

		Vector3 vtCameraPos = (qtn * (Vector3.back * m_flDistance)) + m_vtOffset;
		transform.position = vtCameraPos;//Vector3.Lerp(transform.position, vtCameraPos, Time.deltaTime * 10);
		transform.LookAt(vtCenterPos);
	}

	bool m_bZoomCamera = false;
	//---------------------------------------------------------------------------------------------------
	public void ZoomCamera()
	{
		m_bZoomCamera = true;
		m_flDistance = 1.5f;
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetCamera()
	{
		m_bZoomCamera = false;
		m_flDistance = m_flFarDistance;

		if (m_trTarget == null)
		{
			m_trTarget = transform.parent.parent.Find("Character");
		}

		if (m_bCreate == false)
		{
			transform.position = m_trTarget.position + m_vtOffset + (m_trTarget.forward * m_flDistance);
			transform.LookAt(m_trTarget);
		}
	}
}
