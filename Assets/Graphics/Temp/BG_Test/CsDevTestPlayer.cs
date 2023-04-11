using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsDevTestPlayer2 : CsPanelSkill 
{
	
	float c_flMoveSpeed = 10.0f;
	Vector3 m_vtCameraDefaultPos;
	Quaternion m_quaternionCameraDefault;
	Vector3 m_vtDir;
	Camera m_cameraMain;
	float m_flCamOffsetY = -44.422f;



	void Awake()
	{
		m_cameraMain = Camera.main;
		m_vtCameraDefaultPos = m_cameraMain.transform.localPosition;
		m_quaternionCameraDefault = m_cameraMain.transform.localRotation;


	

	}

	void Update ()
	{
		cMove();

	
	}
	void cMove()
	{
		CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.TransformDirection(Vector3.forward) * Time.deltaTime * 5f;
			this.GetComponent<Animator>().SetInteger("status", 1);
			UpdateDir ();

		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			this.GetComponent<Animator>().SetInteger("status", 0);
	
		}
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * c_flMoveSpeed;
			this.GetComponent<Animator>().SetInteger("status", 2);
			csInGameCamera.Zoom = 1.05f;
			UpdateDir ();
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			this.GetComponent<Animator>().SetInteger("status", 0);
			this.GetComponent<Animator>().SetBool("enterrun", false);
		}
		if (Input.GetKey(KeyCode.A))
		{
			csInGameCamera.RightAndLeft -= 0.15f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			csInGameCamera.RightAndLeft += 0.15f;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			this.GetComponent<Animator> ().Play ("Skill02");
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			this.GetComponent<Animator>().SetInteger("status", 0);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{

			c_flMoveSpeed = 20f;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{

			c_flMoveSpeed = 10f;
		}

		else
		{

			return;
		}

	}
	void UpdateDir()
	{
		CsInGameCamera csInGameCamera = Camera.main.GetComponent<CsInGameCamera> ();
		float flY = csInGameCamera.RightAndLeft;
		//		float flY = 0.7853f;
		transform.localRotation = Quaternion.Euler(new Vector3(0,(flY * 57.324f) ,0));
	}
}
