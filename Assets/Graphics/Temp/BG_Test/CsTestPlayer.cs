using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CsTestPlayer : MonoBehaviour {

	public float t_flMoveSpeed = 0.0f;
    float c_flMoveSpeed = 5.0f;
	Vector3 m_vtCameraDefaultPos;
	Quaternion m_quaternionCameraDefault;
	Vector3 m_vtDir;
	Camera m_cameraMain;
	float m_flCamOffsetY = -44.422f;


	public Button Up;
	public Button Down;
	public Button Left;
	public Button Right;

	bool bUp;
	bool bDown;
	bool bLeft;
	bool bRight;

	void Awake()
	{
		if (SceneManager.GetAllScenes().Length == 1)
		{
			m_cameraMain = Camera.main;
			m_vtCameraDefaultPos = m_cameraMain.transform.localPosition;
			m_quaternionCameraDefault = m_cameraMain.transform.localRotation;
			CsGameData.Instance.MyHeroTransform = transform;
		}
	}

	public void UpButtonDown()
	{
		bUp = true;
	}
	public void UpButtonUp()
	{
		bUp = false;
	}

	public void DownButtonDown()
	{
		bDown = true;
	}
	public void DownButtonUp()
	{
		bDown = false;
	}

	public void LeftButtonDown()
	{
		bLeft = true;
	}
	public void LeftButtonUp()
	{
		bLeft = false;
	}

	public void RightButtonDown()
	{
		bRight = true;
	}
	public void RightButtonUp()
	{
		bRight = false;
	}


    void Update ()
    {
        cMove();
//        cRotate();
		UpdateDir ();
		//pForward (t_flMoveSpeed);
		//pBackward (t_flMoveSpeed);

		if (bUp) {
			pForward (5);
		}

		if (bDown) {
			pBackward (5);
		}

		if (bLeft) {
			pLeft ();
		}

		if (bRight) {
			pRight ();
		}
	}
    void cMove()
    {
		if (Input.GetKey(KeyCode.S))
        {
         //   transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * c_flMoveSpeed;
			transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * c_flMoveSpeed;

        }
        if (Input.GetKey(KeyCode.W))
        {
			//transform.position -= transform.TransformDirection(Vector3.forward) * Time.deltaTime * c_flMoveSpeed;
			transform.position += transform.TransformDirection(Vector3.right) * Time.deltaTime * c_flMoveSpeed;
                    }
        if (Input.GetKey(KeyCode.A))
        {
			CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
			csInGameCamera.RightAndLeft -= 0.075f;
        }
        if (Input.GetKey(KeyCode.D))
        {
			CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
			csInGameCamera.RightAndLeft += 0.075f;
  
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
            c_flMoveSpeed = 10f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            
            c_flMoveSpeed = 5f;
        }

        else
        {

            return;
        }

    }
	public void pForward(float t_flMoveSpeed)
	{
		if (t_flMoveSpeed > 1)
			transform.position += transform.TransformDirection (Vector3.right) * Time.deltaTime * t_flMoveSpeed;
		else
			return;
	}

	public void pBackward(float t_flMoveSpeed)
	{
		if (t_flMoveSpeed > 1)
		transform.position += transform.TransformDirection(Vector3.left) * Time.deltaTime * t_flMoveSpeed;
		else
			return;
	}

	public void pLeft()
	{
		transform.Rotate(Vector3.down, Time.deltaTime * c_flMoveSpeed * 15);
//		CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
//		float flY = csInGameCamera.RightAndLeft++;

	}
	public void pRight()
	{
		transform.Rotate(Vector3.up, Time.deltaTime * c_flMoveSpeed * 15);
//		CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
//		float flY = csInGameCamera.RightAndLeft--;

	}

    void cRotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.down, Time.deltaTime * c_flMoveSpeed * 15);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, Time.deltaTime * c_flMoveSpeed * 15);
        }

		if (Input.GetKey(KeyCode.Z))
		{
			m_cameraMain.transform.Rotate(Vector3.left, Time.deltaTime * c_flMoveSpeed * 10f);
			m_vtDir = transform.position.normalized - m_cameraMain.transform.position.normalized;
			m_cameraMain.transform.position += m_vtDir * 1f;
		}
		else if (Input.GetKey(KeyCode.X))
		{
			m_cameraMain.transform.Rotate(Vector3.right, Time.deltaTime * c_flMoveSpeed * 10f);
			m_vtDir = transform.position.normalized - m_cameraMain.transform.position.normalized;
			m_cameraMain.transform.position += m_vtDir * -1f;
		}

		else if (Input.GetKey(KeyCode.C))
		{
			m_cameraMain.transform.localPosition = m_vtCameraDefaultPos;
			m_cameraMain.transform.localRotation = m_quaternionCameraDefault;
		}
    }

	void UpdateDir()
	{
		CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
		float flY = csInGameCamera.RightAndLeft;
//		float flY = 0.7853f;
		transform.localRotation = Quaternion.Euler(new Vector3(0,(flY * 57.324f)-90f ,0));
	}
}
