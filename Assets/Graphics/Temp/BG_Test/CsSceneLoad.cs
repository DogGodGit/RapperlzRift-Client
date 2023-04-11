using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class CsSceneLoad : MonoBehaviour 
{
	Transform GOCam;
	public float fHeroMid = 1f;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		if (SceneManager.GetAllScenes().Length != 1)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}
		else
		{
			Transform GOCamFind = GameObject.Find("Camera").transform;
			GOCam = GOCamFind.Find("Main Camera");

			Transform trCmera = Camera.main.transform;

			if (trCmera != null)
			{
				CsInGameDungeonCamera csInGameDungeonCamera = trCmera.GetComponent<CsInGameDungeonCamera>();
				if (csInGameDungeonCamera != null)
				{
					Destroy(csInGameDungeonCamera);
					trCmera.gameObject.AddComponent<CsInGameCamera>();
				}
	
				CsIngameData.Instance.InGameCamera = trCmera.GetComponent<CsInGameCamera>();
				CsIngameData.Instance.InGameCamera.CameraMode = EnCameraMode.Camera3D;

				GOCam.GetComponent<PostProcessingBehaviour>().enabled = true;
				CsIngameData.Instance.HeroCenter = fHeroMid;
				Application.targetFrameRate = 100;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		sceneLoad();
	}

	//---------------------------------------------------------------------------------------------------
	public void sceneLoad()
	{
		if (Input.GetKey(KeyCode.Y))
		{
			SceneManager.LoadScene("Area1_Marduka");
		}
		if (Input.GetKey(KeyCode.U))
		{
			SceneManager.LoadScene("Area2_Alliance");
		}
		if (Input.GetKey(KeyCode.I))
		{
			SceneManager.LoadScene("Area3_PalmirGorge");
		}
		if (Input.GetKey(KeyCode.O))
		{
			SceneManager.LoadScene("Area4_Desert");
		}
		if (Input.GetKey(KeyCode.P))
		{
			SceneManager.LoadScene("Area5_Sirag");
		}
	}

	public void postLoad()
	{
		if (Input.GetKey(KeyCode.K))
		{
			GOCam.GetComponent<PostProcessingBehaviour>().enabled = true;
		}
		if (Input.GetKey(KeyCode.L))
		{
			GOCam.GetComponent<PostProcessingBehaviour>().enabled = false;
		}
	}


	public void OnBtnSelectScene(string str)
	{
		// 동기
		SceneManager.LoadScene(str);
	}
	public void postTurnOn()
	{

		GOCam.GetComponent<PostProcessingBehaviour>().enabled = true;


	}

	public void postTurnOff()
	{

		GOCam.GetComponent<PostProcessingBehaviour>().enabled = false;
	}
}
