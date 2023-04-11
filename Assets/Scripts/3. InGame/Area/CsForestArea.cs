using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CsForestArea : CsBaseArea 
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
		m_bCameraMove = true;
		if (m_corutine == null)
		{
			if (CsIngameData.Instance.InGameCamera == null) return;
			m_corutine = StartCoroutine(CameraMove());
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
    }

	//---------------------------------------------------------------------------------------------------
    IEnumerator CameraMove()
    {
		CsInGameCamera csIngameCamera = CsIngameData.Instance.InGameCamera;
		float flOldZoom = csIngameCamera.Zoom;
		float flZoomValue = 1.4f - flOldZoom; // 최대 줌 값에서 현재 값을 빼서 변경될 폭을 지정.

		float flOldRightAndLeft = csIngameCamera.RightAndLeft;
		float flRightAndLeftValue = 0;

		if (Vector3.Distance(CsGameData.Instance.MyHeroTransform.position, new Vector3(213f, 10f, 226f)) < 10f)
		{
			flRightAndLeftValue = 0.9f;
		}
		else
		{
			flRightAndLeftValue = -2.3f;
		}

		flRightAndLeftValue = flRightAndLeftValue - flOldRightAndLeft;
	
		float flOldUpAndDown = csIngameCamera.UpAndDown;
		float flUpAndDownValue = 0.3f - flOldUpAndDown;      // > 0 ? 0.3f - flOldUpAndDown : 0.3f + flOldUpAndDown;

		int nCount = 0;
		while (m_bCameraMove)
        {
			float fDistance = Vector3.Distance(CsGameData.Instance.MyHeroTransform.position, transform.position);

			float flZoom = flOldZoom + (flZoomValue * Mathf.InverseLerp(22f, 18f, fDistance)); // fDistance
			float flUpAndDown = flOldUpAndDown + (flUpAndDownValue * Mathf.InverseLerp(22f, 20f, fDistance));

			if (nCount < 11)
			{
				nCount++;
				//float flRightAndLeft = flOldRightAndLeft + (flRightAndLeftValue * Mathf.InverseLerp(22f, 18f, fDistance));  // 값을 강제로 변경하고 다시 원래 값 복귀 안하는게 좋을 것 같음.
				float flRightAndLeft = flOldRightAndLeft + (flRightAndLeftValue * Mathf.InverseLerp(0, 15, nCount));  // 값을 강제로 변경하고 다시 원래 값 복귀 안하는게 좋을 것 같음 . 이야기 필요.
				csIngameCamera.ZoomPlay = true;
				csIngameCamera.RightAndLeft = flRightAndLeft;
			}

			if (flZoom >= 1.4f)
			{
				flZoom = 1.4f;
			}
			else if (flZoom <= 0.9f)
			{
				flZoom = 0.9f;
			}

			csIngameCamera.ZoomPlay = true;
			csIngameCamera.Zoom = flZoom;
			csIngameCamera.UpAndDown = flUpAndDown;

            yield return new WaitForEndOfFrame();
        }

		csIngameCamera.ZoomPlay = false;
		csIngameCamera.Zoom = flOldZoom;
		csIngameCamera.UpAndDown = flOldUpAndDown;
		m_corutine = null;
    }
}
