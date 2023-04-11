using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsCinemaSwapCam : MonoBehaviour 
{
	public GameObject cam1, cam2;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1)) 
		{
			cam1.SetActive(true);
			cam2.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) 
		{
			cam1.SetActive(false);
			cam2.SetActive(true);
		}
	}
}
