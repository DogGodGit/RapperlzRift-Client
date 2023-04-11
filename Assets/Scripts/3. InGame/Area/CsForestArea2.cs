using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CsForestArea2 : CsBaseArea
{
	Color m_colGizmo = Color.cyan;
	Coroutine m_corutine;
	bool m_bCameraMove = false;

	//---------------------------------------------------------------------------------------------------
	public override void OnDrawGizmos()
	{
		GetComponent<Collider>().isTrigger = true;
		BoxCollider col = GetComponent<BoxCollider>();
		if (col != null)
		{
			Vector3 center, size;
			center = col.center;
			size = col.size;

			Gizmos.color = m_colGizmo;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(center, size);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		if (m_corutine == null)
		{
			m_bCameraMove = true;
			RenderSettings.fogEndDistance = 100f;
			Camera.main.farClipPlane = 55f;
			m_corutine = StartCoroutine(CameraMove(GameObject.FindGameObjectWithTag("Player")));
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void StayAction()
	{
		m_bCameraMove = true;
	}

	//---------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		m_bCameraMove = false;
		RenderSettings.fogEndDistance = 60;
		Camera.main.farClipPlane = 45f;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CameraMove(GameObject playerObj)
	{
		float flOldZoom = CsIngameData.Instance.InGameCamera.Zoom;
		while (m_bCameraMove)
		{
			float flDistance = Vector3.Distance(playerObj.transform.position, transform.position);
			float flZoom = 0.8f - (1.3f * Mathf.InverseLerp(40f, 20f, flDistance)); // fDistance

			if (flZoom > 1)
			{
				flZoom = 1;
			}

			if (flOldZoom != flZoom)
			{
				CsIngameData.Instance.InGameCamera.ZoomPlay = true;
				CsIngameData.Instance.InGameCamera.Zoom = flZoom;
				flOldZoom = flZoom;
			}

			yield return new WaitForEndOfFrame();
		}

		CsIngameData.Instance.InGameCamera.ZoomPlay = false;
		CsIngameData.Instance.InGameCamera.Zoom = flOldZoom;
		m_corutine = null;
	}
}
