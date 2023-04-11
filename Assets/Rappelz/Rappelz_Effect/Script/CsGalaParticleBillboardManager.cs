using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsGalaParticleBillboardManager : MonoBehaviour {


    public bool m_bCameraLookAt;
    public bool m_bFixedObjectUp;
    public bool m_bFixedStand;
    public enum AXIS_TYPE { AXIS_FORWARD, AXIS_BACK, AXIS_RIGHT, AXIS_LEFT, AXIS_UP, AXIS_DOWN };
    public AXIS_TYPE m_FrontAxis;
    public enum ROTATION { NONE, RND, RNDMINMAX, ROTATE }
    public ROTATION m_RatationMode;
    public enum AXIS { X = 0, Y, Z };
    public AXIS m_RatationAxis = AXIS.Z;
    public float m_fRotationValue = 0;

	public float m_fRandomValueA = 180;
	public float m_fRandomValueB = 180;


    protected float m_fRndValue;
    protected float m_fTotalRotationValue;
    protected Quaternion m_qOiginal;

    // Use this for initialization

    bool IsCreatingEditObject()
    {
        GameObject main = GameObject.Find("_FXMaker");
        if (main == null)
            return false;
        GameObject effroot = GameObject.Find("_CurrentObject");
        if (effroot == null)
            return false;
        return (effroot.transform.childCount == 0);
    }

    void OnEnable()
    {
        #if UNITY_EDITOR
            if (IsCreatingEditObject() == false)
                UpdateBillboard();
        #else
 		    UpdateBillboard();
        #endif
    }

    public void UpdateBillboard()
    {
		if (m_RatationMode == ROTATION.RND) {
			m_fRndValue = Random.Range (0, 360.0f);
		} else {
			m_fRndValue = Random.Range (m_fRandomValueA, m_fRandomValueB);
		}
        if (enabled)
            Update();
    }


    // Update is called once per frame
    void Update()
    {
        if (Camera.main == null)
            return;
        Vector3 vecUp;

        // 카메라 업벡터를 무시하고 오젝의 업벡터를 유지한다
        if (m_bFixedObjectUp)
            //  			vecUp		= m_qOiginal * Vector3.up;
            vecUp = transform.up;
        else vecUp = Camera.main.transform.rotation * Vector3.up;

        if (m_bCameraLookAt)
            transform.LookAt(Camera.main.transform, vecUp);
        else transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.back, vecUp);

        switch (m_FrontAxis)
        {
            case AXIS_TYPE.AXIS_FORWARD: break;
            case AXIS_TYPE.AXIS_BACK: transform.Rotate(transform.up, 180, Space.World); break;
            case AXIS_TYPE.AXIS_RIGHT: transform.Rotate(transform.up, 270, Space.World); break;
            case AXIS_TYPE.AXIS_LEFT: transform.Rotate(transform.up, 90, Space.World); break;
            case AXIS_TYPE.AXIS_UP: transform.Rotate(transform.right, 90, Space.World); break;
            case AXIS_TYPE.AXIS_DOWN: transform.Rotate(transform.right, 270, Space.World); break;
        }

        if (m_bFixedStand)
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        if (m_RatationMode == ROTATION.RND)
            transform.localRotation *= Quaternion.Euler((m_RatationAxis == AXIS.X ? m_fRndValue : 0), (m_RatationAxis == AXIS.Y ? m_fRndValue : 0), (m_RatationAxis == AXIS.Z ? m_fRndValue : 0));
        if (m_RatationMode == ROTATION.ROTATE)
        {
			float fRotValue = m_fRotationValue * Time.deltaTime;
            transform.Rotate((m_RatationAxis == AXIS.X ? fRotValue : 0), (m_RatationAxis == AXIS.Y ? fRotValue : 0), (m_RatationAxis == AXIS.Z ? fRotValue : 0), Space.Self);
        }
		if (m_RatationMode == ROTATION.RNDMINMAX)
		{
			transform.localRotation *= Quaternion.Euler((m_RatationAxis == AXIS.X ? m_fRndValue : 0), (m_RatationAxis == AXIS.Y ? m_fRndValue : 0), (m_RatationAxis == AXIS.Z ? m_fRndValue : 0));
		}
    }

}

