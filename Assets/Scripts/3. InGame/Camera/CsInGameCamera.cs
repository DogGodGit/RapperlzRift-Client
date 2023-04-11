using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

public enum EnCameraPlay
{
	None = 0,
	Normal,
	Acceleartion,
	AttackRush,
	EnterAction1,
	EnterAction2,
	BossAppear,
	BossDead,
	Domination,
	Clear
}

public enum EnDashState
{
	None = 0,
	Dash, 
	DashEnd 
}

public class CsInGameCamera : MonoBehaviour
{
	protected class CsCameraValue
	{
		float m_flDuration = 3f; //카메라 복귀시간
		float m_flHeightOffset = 0f;
		float m_flWidthOffset = 0f;
		float m_flForwardOffset = 0.15f;
		float m_flLength = 7f;
		float m_flHeight = 5f;
		float m_flScreenYHeight = 0f;
		float m_flPivot2D_X = 0f;
		float m_flPivot2D_Y = 0f;
		float m_flUpAndDownValue = 0.0f;
		float m_flRightAndLeftValue = 1.57079f;
		float m_flZoom = 1.15f;
		float m_flFieldOfView = 45;

		public float Duration { get { return m_flDuration; } }
		public float HeightOffset { get { return m_flHeightOffset; } }
		public float WidthOffset { get { return m_flWidthOffset; } }
		public float ForwardOffset { get { return m_flForwardOffset; } }
		public float Length { get { return m_flLength; } }
		public float Height { get { return m_flHeight; } }
		public float ScreenYHeight { get { return m_flScreenYHeight; } }
		public float Pivot2D_X { get { return m_flPivot2D_X; } }
		public float Pivot2D_Y { get { return m_flPivot2D_Y; } }
		public float UpAndDown { get { return m_flUpAndDownValue; } }
		public float RightAndLeft { get { return m_flRightAndLeftValue; } }
		public float Zoom { get { return m_flZoom; } }
		public float FieldOfView { get { return m_flFieldOfView; } }

		public CsCameraValue(float flDuration, float flHeightOffset, float flWidthOffset, float flForwardOffset, float flLength, float flHeight, float flScreenYHeight,
							 float flPivot2D_X, float flPivot2D_Y, float flUpAndDown, float flRightAndLeft, float flZoom, float flFieldOfView)
		{
			m_flDuration = flDuration;
			m_flHeightOffset = flHeightOffset;
			m_flWidthOffset = flWidthOffset;
			m_flForwardOffset = flForwardOffset;
			m_flLength = flLength;
			m_flHeight = flHeight;
			m_flScreenYHeight = flScreenYHeight;
			m_flPivot2D_X = flPivot2D_X;
			m_flPivot2D_Y = flPivot2D_Y;
			m_flUpAndDownValue = flUpAndDown;
			m_flRightAndLeftValue = flRightAndLeft;
			m_flZoom = flZoom;
			m_flFieldOfView = flFieldOfView;
		}
	}

	protected enum EnCameraValueType
	{
		HeightOffset = 0,
		WidthOffset = 1,
		ForwardOffset = 2,
		ZoomOffset = 3,
		Length = 4,
		Height = 5,
		ScreenYHeight = 6,
		Pivot2D_X = 7,
		Pivot2D_Y = 8,
		UpAndDown = 9,
		RightAndLeft = 10,
		Zoom = 11,
		FieldOfView = 12
	}

	const float c_flAdditionalHeight = 1.05f;
	protected Camera m_camera;
	AmplifyColorEffect m_amplifyColorEffect;
	PostProcessingBehaviour m_postProcessing;
	Texture m_txBlackWhite;
	Texture m_txOirgin;

	[SerializeField]
	protected Transform m_trCameraTarget;

	protected Vector3 m_vtCenter = Vector3.zero;
	protected Vector2 m_vt2PrevPos = Vector2.zero;

	protected float m_flRadius = 10f;
	protected float m_flDelayWatingTime = 0.0f;

	float m_flDefaultFarClipPlane = 0f;
	float m_flDefaultNearClipPlane;

	float c_flCameraDragSpeed = 0.002f;
	int m_nLayerMask;

	float m_flMinZoom = 1.4f;
	float m_flMaxZoom = 0.9f;

	[SerializeField]
	bool m_bFirstEnter = false;
	[SerializeField]
	bool m_bHeightMapPlay = false;
	bool m_bChangeNewState = false;

	float m_flTempUpAndDownValue;
	bool m_bSetSleepMode = false;
	float m_flSetTimer = 1.1f;
	float m_flTimerValueControl = 1.0f;
	float m_flDashTime = 0.0f;
	float m_flCameraOffsetY = 1000f;

	protected bool m_bZoom = false;
	protected bool m_bShaking = false;
	protected float m_flShakeDecay;
	protected float m_flShakeIntensity;

	[SerializeField]
	float m_flObject_Distance = 30;
	[SerializeField]
	float m_fl1D_GDistance = 25;
	[SerializeField]
	float m_fl2D_GDistance = 45;
	[SerializeField]
	float m_fl3D_GDistance = 100;

	protected float m_flOrgLength;
	protected float m_flOrgHeight;
	protected float m_flOrgPivot2D_X;
	protected float m_flOrgPivot2D_Y;
	protected float m_flOrgUpAndDownValue;
	protected float m_flOrgZoom;
	
	[SerializeField]
	protected float m_flUpAndDownOffest = 0;
	[SerializeField]
	protected float m_flWidthOffset = 0f;
	[SerializeField]
	protected float m_flZoomOffset = 0f;
	[SerializeField]
	protected float m_flForwardOffset = 0.15f;
	[SerializeField]
	protected float m_flScreenYHeight = 0f;
	[SerializeField]
	protected float m_flHeightOffset = 0f;
	[SerializeField]
	protected float m_flHeightPlayerCenter = 1f;
	[SerializeField]
	protected float m_flFieldOfView = 45;
	[SerializeField]
	protected float m_flLength;
	[SerializeField]
	protected float m_flHeight;
	[SerializeField]
	protected float m_flPivot2D_X;
	[SerializeField]
	protected float m_flPivot2D_Y;
	[SerializeField]
	protected float m_flUpAndDownValue;
	[SerializeField]
	protected float m_flRightAndLeftValue;
	[SerializeField]
	protected float m_flZoom;

	CsCameraValue m_FirstEnterState;
	CsCameraValue m_AutoState;
	CsCameraValue m_QuarterViewState;
	CsCameraValue m_FlightState;

	[SerializeField]
	protected EnCameraMode m_enCameraMode = EnCameraMode.CameraAuto;
	[SerializeField]
	protected EnCameraPlay m_enDungeonCameraPlay = EnCameraPlay.Normal;
	[SerializeField]
	protected EnDashState m_enDashState = EnDashState.None;

	public Camera Camera { get { return m_camera; } }
	public float UpAndDown { get { return m_flUpAndDownValue; } set { m_flUpAndDownValue = value; } }
	public float RightAndLeft { get { return m_flRightAndLeftValue; } set { m_flRightAndLeftValue = value; } }
	public float Zoom { get { return m_flZoom; } set { m_flZoom = value; } }
	
	public bool ZoomPlay { get { return m_bZoom; } set { m_bZoom = value; } }
	public float UpAndDownOffest { get { return m_flUpAndDownOffest; } set { m_flUpAndDownOffest = value; } }
	public float HeightPlayerCenter { get { return m_flHeightPlayerCenter; } set { m_flHeightPlayerCenter = value; } }
	public bool FirstEnter { get { return m_bFirstEnter; } set { FirstEnterCamera(value); } }
	public bool SleepMode { get { return m_bSetSleepMode; } set { m_bSetSleepMode = value; } }
	
	public EnCameraMode CameraMode { get { return m_enCameraMode; } set { m_enCameraMode = CsIngameData.Instance.CameraMode = value; } }
	public EnCameraPlay CameraPlay { get { return m_enDungeonCameraPlay; } }

	[SerializeField]	// 임시 컬링 변경 적용.
	bool bChangeCulling = false;

	//---------------------------------------------------------------------------------------------------
	protected virtual void Awake()
	{
		CsGameEventToIngame.Instance.EventChangeCameraState += OnEventChangeCameraState;
		CsGameEventToIngame.Instance.EventSleepMode += OnEventSleepMode;
		m_camera = GetComponent<Camera>();	
		CsIngameData.Instance.InGameCamera = this;
		m_postProcessing = GetComponent<PostProcessingBehaviour>();
		m_amplifyColorEffect = GetComponent<AmplifyColorEffect>();
		if (m_amplifyColorEffect.enabled)
		{
			m_bActivtyAmplifyColorEffect = true;
		}

		m_txOirgin = m_amplifyColorEffect.LutTexture;
		m_txBlackWhite = CsIngameData.Instance.LoadAsset<Texture>("BlackWhite");
		
		m_flDefaultNearClipPlane = m_camera.nearClipPlane = 0.5f;
		m_flDefaultFarClipPlane = m_camera.farClipPlane;

		m_nLayerMask = m_camera.cullingMask;
		m_camera.depthTextureMode = DepthTextureMode.Depth; //동작 안하면 물이 정상적으로 처리가 안됨. AQUAS
		InitializeHeightInfo(); // Heightmap 값 세팅.

		CameraCullDistance(m_flObject_Distance);
	}

	//---------------------------------------------------------------------------------------------------
	public void CameraCullDistance(float flObjectDistance)
	{
		m_flObject_Distance = flObjectDistance;

		float[] aflDistance = new float[32]; // 거리에 따른 원경 처리 레이어 설정.
		aflDistance = Camera.main.layerCullDistances;
		aflDistance[LayerMask.NameToLayer("1D_G")] = m_fl1D_GDistance;
		aflDistance[LayerMask.NameToLayer("2D_Terrain")] = m_fl2D_GDistance;
		aflDistance[LayerMask.NameToLayer("2D_G")] = m_fl2D_GDistance;
		aflDistance[LayerMask.NameToLayer("3D_G")] = m_fl3D_GDistance;
		aflDistance[LayerMask.NameToLayer("Terrain")] = m_fl3D_GDistance;

		aflDistance[LayerMask.NameToLayer("Player")] = m_fl3D_GDistance;
		aflDistance[LayerMask.NameToLayer("Hero")] = m_flObject_Distance;
		aflDistance[LayerMask.NameToLayer("Npc")] = m_flObject_Distance;
		aflDistance[LayerMask.NameToLayer("Monster")] = m_flObject_Distance;
		aflDistance[LayerMask.NameToLayer("Object")] = m_flObject_Distance;

		Camera.main.layerCullDistances = aflDistance;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void Start()
	{
		m_flForwardOffset = 0.15f;
		m_flHeightOffset = 0f;
		m_flWidthOffset = 0.05f;
		m_flLength = m_flOrgLength = 5.5f;
		m_flHeight = m_flOrgHeight = 4.5f;
		m_flPivot2D_X = m_flOrgPivot2D_X = 0f;
		m_flPivot2D_Y = m_flOrgPivot2D_Y = 0.5f;
		m_flUpAndDownValue = m_flOrgUpAndDownValue = 0.2f;
		m_flRightAndLeftValue = 0.7853f;   //  Mathf.PI / 4;
		m_flZoom = m_flOrgZoom = 1.05f;

		m_FirstEnterState = new CsCameraValue(0f, -0.4f, m_flWidthOffset, m_flForwardOffset, 2.3f, 3.2f, 0.3f, 0.3f, 0.43f, 0.25f, 0f, 1.5f, 40f);
		m_AutoState = new CsCameraValue(0.5f, 0f, m_flWidthOffset, m_flForwardOffset, m_flOrgLength, m_flOrgHeight, 0.2f, m_flOrgPivot2D_X, m_flOrgPivot2D_Y, m_flOrgUpAndDownValue, m_flRightAndLeftValue, 1.05f, 45f);
		m_QuarterViewState = new CsCameraValue(0.5f, 0f, 0f, m_flForwardOffset, 6.5f, 8f, -0.35f, m_flOrgPivot2D_X, m_flOrgPivot2D_Y, m_flOrgUpAndDownValue, m_flRightAndLeftValue, 0.9f, 45f);
		m_FlightState = new CsCameraValue(1f, 0f, 0.05f, 0.15f, 5.5f, 4.5f, 0.2f, 0f, 0.5f, -0.038f, 0.9512911f, 1.1f, 45f);
		ChangeNewState(CsIngameData.Instance.CameraMode);
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void Update()
	{
		if (CsGameData.Instance.MyHeroTransform == null) return;

		if (m_trCameraTarget == null)
		{
			m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
			return;
		}

		if (bChangeCulling)	// 임시 거리 변경 테스트용.
		{
			CameraCullDistance(m_flObject_Distance);
			bChangeCulling = false;
		}

		if (!ZoomByTouch())
		{
			DragByTouch();
		}

		if (m_bFirstEnter == false) // 최초 입장 효과중이 아닐때.
		{
			if (m_bChangeNewState)
			{
				if (m_enCameraMode == EnCameraMode.CameraAuto)
				{
					UpdateAutoState();
				}
				else if (m_enCameraMode == EnCameraMode.Camera2D)
				{
					UpdateQuarterViewState();
				}
			}
			else
			{
				UpdateDash();
				AutoChaseCamera();
			}
		}
		
		UpdateCameraPos();
	}

	//---------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		if (CsGameData.Instance.MyHeroTransform == null) return;

		CameraShake();

#if UNITY_EDITOR
		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (m_flZoom > m_flMaxZoom + 0.02f)
			{
				m_flZoom -= 0.03f;
			}
			else
			{
				m_flZoom = m_flMaxZoom;
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (m_flZoom < m_flMinZoom - 0.02f)
			{
				m_flZoom += 0.03f;
			}
			else
			{
				m_flZoom = m_flMinZoom;
			}
		}
#endif
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnDestroy()
	{
		CsGameEventToIngame.Instance.EventChangeCameraState -= OnEventChangeCameraState;
		CsGameEventToIngame.Instance.EventSleepMode -= OnEventSleepMode;
		m_trCameraTarget = null;
		m_camera = null;
		m_amplifyColorEffect = null;
		m_txBlackWhite = null;
		m_txOirgin = null;
		CsIngameData.Instance.InGameCamera = null;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateDash()
	{
		if (m_enDashState == EnDashState.Dash)
		{
			float flTimeRate = m_flDashTime / 1f;

			if (flTimeRate > 1f) return;

			if (m_enCameraMode == EnCameraMode.Camera2D)
			{
				if (ChangeCameraValue(EnCameraValueType.ZoomOffset, -0.2f, flTimeRate))
				{
					m_flDashTime += Time.deltaTime * 0.25f;
				}
			}
			else
			{
				if (ChangeCameraValue(EnCameraValueType.ZoomOffset, 0.2f, flTimeRate) && ChangeCameraValue(EnCameraValueType.FieldOfView, m_AutoState.FieldOfView + 15, flTimeRate))
				{
					m_flDashTime += Time.deltaTime * 0.25f;
				}
			}
		}
		else if (m_enDashState == EnDashState.DashEnd)
		{
			float flTimeRate = m_flDashTime / 1f;

			ChangeCameraValue(EnCameraValueType.FieldOfView, m_AutoState.FieldOfView, flTimeRate);

			if (ChangeCameraValue(EnCameraValueType.ZoomOffset, 0, flTimeRate))
			{
				m_flDashTime += Time.deltaTime * 0.25f;
			}

			if (flTimeRate == 1)
			{
				ChangeDashState(EnDashState.None);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateCameraPos()
	{
		Vector3 vtCenterOffset = new Vector3(0f, m_flHeightOffset + m_flHeightPlayerCenter, m_flWidthOffset) + (m_trCameraTarget.forward * m_flForwardOffset) + (m_trCameraTarget.up * m_flScreenYHeight);
		m_vtCenter = m_trCameraTarget.position + vtCenterOffset;

		m_flRadius = Mathf.Sqrt(m_flLength * m_flLength * 2 + m_flHeight * m_flHeight);
		Quaternion qtn = Quaternion.Euler((Mathf.Asin(m_flHeight / m_flRadius) - (m_flUpAndDownValue + m_flUpAndDownOffest)) * Mathf.Rad2Deg, m_flRightAndLeftValue * Mathf.Rad2Deg, 0);
		Vector3 vtOffset = qtn * (Vector3.back * m_flRadius);
		Vector3 vtForward = (new Vector3(-vtOffset.x, 0, -vtOffset.z)).normalized;
		Vector3 vtRight = new Vector3(vtForward.z, 0, -vtForward.x);

		m_vtCenter = m_vtCenter + (vtForward * m_flPivot2D_Y) + (vtRight * m_flPivot2D_X);
		Vector3 vtCameraPos = m_vtCenter + vtOffset * (2f - (m_flZoom + m_flZoomOffset));

		if (m_bHeightMapPlay && m_bFirstEnter == false)
		{
			float flCameraOffsetY = Mathf.Clamp(GetMapHeight(vtCameraPos) * c_flAdditionalHeight, vtCameraPos.y, float.MaxValue);
			float flValue = flCameraOffsetY - vtCameraPos.y;
			//Debug.Log("1. UpdateCameraPos    flValue = " + flValue + " // flCameraOffsetY " + flCameraOffsetY + " // Y" + vtCameraPos.y);

			Vector3 vtHeroPos = m_trCameraTarget.position;
			float flHeroOffsetY = Mathf.Clamp(GetMapHeight(vtHeroPos) * c_flAdditionalHeight, vtHeroPos.y, float.MaxValue);

			if (flValue > 3) // 경사에 의한 보정이 3 이상이면 위치 변경.
			{
				//vtCameraPos = vtTarget + (vtOffset * 0.6f); // 최대 줌 값(2f - 1.4f) 으로 적용.
				if (m_flCameraOffsetY + 0.2f < flCameraOffsetY)
				{
					flCameraOffsetY = m_flCameraOffsetY;	// 특정 값보다 작으면 이전값 적용.
				}

				vtCameraPos = new Vector3(vtCameraPos.x, flCameraOffsetY, vtCameraPos.z);
				transform.position = Vector3.Slerp(transform.position, vtCameraPos, Time.deltaTime * 10); // 카메라 이동을 부드럽게 이동.
				m_flCameraOffsetY = flCameraOffsetY;
			}
			else
			{
				Vector3 vtCameraOffsetPos = Vector3.Slerp(transform.position, vtCameraPos, Time.deltaTime * 10); // 카메라 이동을 부드럽게 이동.
				if (vtCameraOffsetPos.y - vtCameraPos.y > 0.3f)	//	 되돌아 가는 경우 부드럽게 처리.
				{
					m_flCameraOffsetY = 1000;
					transform.position = vtCameraOffsetPos;
				}
				else
				{
					transform.position = vtCameraPos;
				}
			}
		}
		else
		{
			transform.position = vtCameraPos;
		}

		m_camera.fieldOfView = m_flFieldOfView;
		transform.LookAt(m_vtCenter);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventChangeCameraState(EnCameraMode enCameraMode)
	{
		Debug.Log("OnEventChangeCameraState          enCameraMode = " + enCameraMode);
		if (FirstEnter)
		{
			FirstEnterCamera(false);
		}

		ChangeNewState(enCameraMode);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventSleepMode(bool bIsOn) // UI로 부터 받은 절전 모드 이벤트 (06/04 hun)
	{
		if (bIsOn == true)
		{
			m_camera.enabled = false;
			m_bSetSleepMode = true;

			Resources.UnloadUnusedAssets();
			System.GC.Collect();
		}
		else
		{
			m_camera.enabled = true;
			m_bSetSleepMode = false;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsClearCameraPlay() { return m_enDungeonCameraPlay == EnCameraPlay.Clear; }
	public bool IsAcceleartionCameraPlay() { return m_enDungeonCameraPlay == EnCameraPlay.Acceleartion; }
	public bool IsActionCameraPlay() { return (m_enDungeonCameraPlay == EnCameraPlay.EnterAction1 || m_enDungeonCameraPlay == EnCameraPlay.EnterAction2); }


	//---------------------------------------------------------------------------------------------------
	public void ChangeNewState(EnCameraMode enNewCameraMode)
	{
		Debug.Log("#####     CsInGameCamera.ChangeState         enNewCameraState = " + enNewCameraMode);

		m_flDelayWatingTime = 0;
		SaveStateValue(m_enCameraMode);

		if (enNewCameraMode == EnCameraMode.CameraAuto)
		{
			m_bChangeNewState = true;
			AutoCamera();
		}
		else if (enNewCameraMode == EnCameraMode.Camera3D)
		{
			AutoCamera();
		}
		else if (enNewCameraMode == EnCameraMode.Camera2D)
		{
			m_bChangeNewState = true;
			QuarterViewCamera();
		}

		m_enCameraMode = CsIngameData.Instance.CameraMode = enNewCameraMode;
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeDashState(EnDashState enNewDashState)
	{
		m_flDashTime = 0;
		m_enDashState = enNewDashState;
	}

	//---------------------------------------------------------------------------------------------------
	void FirstEnterCamera(bool bFirstEnter)
	{
		if (bFirstEnter)
		{
			float flSetRightAndLeft = 3.0f + CsGameData.Instance.MyHeroTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
			if (flSetRightAndLeft > 6.28f)
			{
				flSetRightAndLeft = flSetRightAndLeft - 6.28f;
			}

			m_flHeightOffset = m_FirstEnterState.HeightOffset;
			m_flWidthOffset = m_FirstEnterState.WidthOffset;
			m_flForwardOffset = m_FirstEnterState.ForwardOffset;
			m_flScreenYHeight = m_FirstEnterState.ScreenYHeight;
			m_flLength = m_FirstEnterState.Length;
			m_flHeight = m_FirstEnterState.Height;
			m_flPivot2D_X = m_FirstEnterState.Pivot2D_X;
			m_flPivot2D_Y = m_FirstEnterState.Pivot2D_Y;
			m_flUpAndDownValue = m_FirstEnterState.UpAndDown;
			m_flRightAndLeftValue = flSetRightAndLeft;
			m_flZoom = m_FirstEnterState.Zoom;
			m_camera.nearClipPlane = 0.8f;
			m_flFieldOfView = 40f;
		}
		else
		{
			m_flHeightOffset = m_AutoState.HeightOffset;
			m_flWidthOffset = m_AutoState.WidthOffset;
			m_flForwardOffset = m_AutoState.ForwardOffset;
			m_flScreenYHeight = m_AutoState.ScreenYHeight;
			m_flLength = m_AutoState.Length;
			m_flHeight = m_AutoState.Height;
			m_flPivot2D_X = m_AutoState.Pivot2D_X;
			m_flPivot2D_Y = m_AutoState.Pivot2D_Y;
			m_flUpAndDownValue = m_AutoState.UpAndDown;
			m_flZoom = m_AutoState.Zoom;
			m_camera.nearClipPlane = 0.8f;
			m_flFieldOfView = 40f;
			ResetNearClipPlane();
		}

		m_bChangeNewState = false;
		m_bFirstEnter = bFirstEnter;
	}

	//---------------------------------------------------------------------------------------------------
	void SaveStateValue(EnCameraMode enCameraMode)
	{
		if (enCameraMode == EnCameraMode.Camera3D)
		{
			m_AutoState = new CsCameraValue(0.5f, 0f, 0f, m_flForwardOffset, m_flLength, m_flHeight, 0.2f, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, 45f);
		}
		else if (enCameraMode == EnCameraMode.Camera2D)
		{
			m_QuarterViewState = new CsCameraValue(0.5f, 0f, 0f, m_flForwardOffset, m_flLength, m_flHeight, -0.35f, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, 45f);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AutoCamera()
	{
		m_flMinZoom = 1.28f;
		m_flMaxZoom = 0.65f;
		m_flAutoUpDownStartTime = Time.time;
		m_flAutoUpDownDelayTime = 0f;
	}

	//---------------------------------------------------------------------------------------------------
	void QuarterViewCamera()
	{
		m_flMinZoom = 0.8f;
		m_flMaxZoom = 0.6f;
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateAutoState()
	{
		float flTimeRate = m_flDelayWatingTime / m_AutoState.Duration;

		ChangeCameraValue(EnCameraValueType.HeightOffset, m_AutoState.HeightOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ForwardOffset, m_AutoState.ForwardOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_X, m_AutoState.Pivot2D_X, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_Y, m_AutoState.Pivot2D_Y, flTimeRate);
		ChangeCameraValue(EnCameraValueType.WidthOffset, m_AutoState.WidthOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Length, m_AutoState.Length, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Height, m_AutoState.Height, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ScreenYHeight, m_AutoState.ScreenYHeight, flTimeRate);
		ChangeCameraValue(EnCameraValueType.UpAndDown, m_AutoState.UpAndDown, flTimeRate);		
		ChangeCameraValue(EnCameraValueType.Zoom, m_AutoState.Zoom, flTimeRate);
		ChangeCameraValue(EnCameraValueType.FieldOfView, m_AutoState.FieldOfView, flTimeRate);

		m_flDelayWatingTime += Time.deltaTime;

		if (m_flDelayWatingTime >= m_AutoState.Duration)
		{
			m_flDelayWatingTime = 0;
			m_bChangeNewState = false;
			SetCameraValue(m_AutoState);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void UpdateQuarterViewState()
	{
		float flTimeRate = m_flDelayWatingTime / m_QuarterViewState.Duration;

		ChangeCameraValue(EnCameraValueType.HeightOffset, m_QuarterViewState.HeightOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ForwardOffset, m_QuarterViewState.ForwardOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_X, m_QuarterViewState.Pivot2D_X, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_Y, m_QuarterViewState.Pivot2D_Y, flTimeRate);
		ChangeCameraValue(EnCameraValueType.WidthOffset, m_QuarterViewState.WidthOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Length, m_QuarterViewState.Length, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Height, m_QuarterViewState.Height, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ScreenYHeight, m_QuarterViewState.ScreenYHeight, flTimeRate);
		ChangeCameraValue(EnCameraValueType.UpAndDown, m_QuarterViewState.UpAndDown, flTimeRate);		
		ChangeCameraValue(EnCameraValueType.Zoom, m_QuarterViewState.Zoom, flTimeRate);
		ChangeCameraValue(EnCameraValueType.FieldOfView, m_QuarterViewState.FieldOfView, flTimeRate);

		m_flDelayWatingTime += Time.deltaTime;

		if (m_flDelayWatingTime >= m_QuarterViewState.Duration)
		{
			m_flDelayWatingTime = 0;
			m_bChangeNewState = false;
			SetCameraValue(m_QuarterViewState);
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool ZoomByTouch()
	{
		if (m_bZoom) return false; //if (m_bZoom || m_bShaking) return false;

		if (CsTouchInfo.Instance.ZoomByTouch()) // Zoom Touch 관련 처리
		{
			if (FirstEnter)
			{
				FirstEnterCamera(false);
				return true;
			}

			float fl = m_flZoom;
			fl += (CsTouchInfo.Instance.ZoomDelta / 1000);
			m_flZoom = Mathf.Max(Mathf.Min(fl, m_flMinZoom), m_flMaxZoom);
			return true;
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void DragByTouch()
	{
		if (m_bZoom) return; //  || m_bShaking) return;
		
		if (CsTouchInfo.Instance.DragByTouch())
		{
			if (FirstEnter)
			{
				FirstEnterCamera(false);
				return;
			}

			Vector2 vtPos = CsTouchInfo.Instance.TouchedScreenPosition;

			if (m_vt2PrevPos != Vector2.zero && vtPos != m_vt2PrevPos)
			{
				float flChangeValueX = c_flCameraDragSpeed * 2 * Mathf.Abs((m_vt2PrevPos - vtPos).x);
				float flChangeValueY = c_flCameraDragSpeed * Mathf.Abs((m_vt2PrevPos - vtPos).y);

				if ((m_vt2PrevPos - vtPos).x < 0)		// X값 변경에 대한 처리.
				{
					if (m_flRightAndLeftValue + flChangeValueX > 6.28f)
					{
						m_flRightAndLeftValue = 0;
					}
					else
					{
						m_flRightAndLeftValue += flChangeValueX;
					}
				}
				else // if ((m_vt2PrevPos - vtPos).x > 0)
				{
					if (m_flRightAndLeftValue - flChangeValueX < 0)
					{
						m_flRightAndLeftValue = 6.28f;
					}
					else
					{
						m_flRightAndLeftValue -= flChangeValueX;
					}
				}

				if (m_enCameraMode != EnCameraMode.Camera2D) // 쿼터뷰 아닐때만 드래그로 변경.
				{
					if ((m_vt2PrevPos - vtPos).y < 0)		//Y값 변경에 대한 처리.
					{
						if (m_flUpAndDownValue + 0.01f < 0.3f) // Max 0.4f
						{
							if (m_flUpAndDownValue + flChangeValueY > 0.3f)
							{
								m_flUpAndDownValue = 0.3f;
							}
							else
							{
								m_flUpAndDownValue += flChangeValueY;
							}
						}
					}
					else // if ((m_vt2PrevPos - vtPos).y > 0)
					{
						if (m_flUpAndDownValue - 0.01f > -0.6f) // Min -0.8f
						{
							if (m_flUpAndDownValue - flChangeValueY < -0.6f)
							{
								m_flUpAndDownValue = -0.6f;
							}
							else
							{
								m_flUpAndDownValue -= flChangeValueY;
							}
						}
					}
				}
			}

			m_vt2PrevPos = vtPos;
			return;
		}

		m_vt2PrevPos = Vector2.zero;
	}

	#region CameraEvent

	Coroutine m_corPostProcess;

	//---------------------------------------------------------------------------------------------------
	public void PostProcessingStart(bool bBlur, bool bMotionBlur, float flTime = 0.2f)
	{
		if (CsIngameData.Instance.Graphic == 0) return; // 그래픽 하일때 사용 안하기.
		if (m_corPostProcess != null) return;

		m_corPostProcess = StartCoroutine(PostProcess(bBlur, bMotionBlur, flTime));
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator PostProcess(bool bBlur, bool bMotionBlur, float flTime)
	{
		if (m_postProcessing != null || m_postProcessing.profile.chromaticAberration != null)
		{
			bool bEnabled = false;
			if (m_postProcessing.enabled)
			{
				bEnabled = true;
			}

			m_postProcessing.enabled = true;

			if (bBlur)
			{
				m_postProcessing.profile.chromaticAberration.OnValidate();
				m_postProcessing.profile.chromaticAberration.enabled = true;
			}

			if (bMotionBlur && CsIngameData.Instance.Graphic == 2)
			{

			}

			yield return new WaitForSeconds(flTime);

			if (bBlur)
			{
				m_postProcessing.profile.chromaticAberration.Reset();
				m_postProcessing.profile.chromaticAberration.enabled = false;
			}

			if (bMotionBlur && CsIngameData.Instance.Graphic == 2)
			{
				m_postProcessing.profile.motionBlur.Reset();
				m_postProcessing.profile.motionBlur.enabled = false;
			}

			m_postProcessing.enabled = bEnabled;
		}
		m_corPostProcess = null;
	}
	
	float m_flBlendAmount;
	bool m_bActivtyAmplifyColorEffect = false;
	//---------------------------------------------------------------------------------------------------
	public void ChangeTexture(bool bDead)
	{
		if (bDead)
		{
			if (m_bActivtyAmplifyColorEffect == false)
			{
				m_amplifyColorEffect.enabled = true;
			}

			m_flBlendAmount = m_amplifyColorEffect.BlendAmount;
			m_amplifyColorEffect.BlendAmount = 0;
			m_amplifyColorEffect.LutTexture = m_txBlackWhite;
		}
		else
		{
			m_amplifyColorEffect.BlendAmount = m_flBlendAmount;
			m_amplifyColorEffect.LutTexture = m_txOirgin;
			if (m_bActivtyAmplifyColorEffect == false)
			{
				m_amplifyColorEffect.enabled = false;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetCamera()
	{
		m_bZoom = false;
		m_camera.cullingMask = m_nLayerMask;
		this.m_flLength = m_flOrgLength;
		this.m_flHeight = m_flOrgHeight;
		this.m_flPivot2D_X = m_flOrgPivot2D_X;
		this.m_flPivot2D_Y = m_flOrgPivot2D_Y;
		this.m_flUpAndDownValue = m_flOrgUpAndDownValue;
		this.m_flRightAndLeftValue = CsGameData.Instance.MyHeroTransform.eulerAngles.y * Mathf.Deg2Rad;
		this.m_flZoom = m_flOrgZoom;
		m_flHeightPlayerCenter = CsIngameData.Instance.HeroCenter;
	}

	//---------------------------------------------------------------------------------------------------
	public void CameraSetValue(float flUpAndDownValue, float flRightAndLeftValue, float flZoom, float flHeightPlayerCenter) // 카메라 이동만.
	{
		Debug.Log("CameraSetValue     flUpAndDown = " + flUpAndDownValue + " // flRightAndLeft = " + flRightAndLeftValue + " // flZoom = " + flZoom + " // PlayerCenter = " + flHeightPlayerCenter);
		m_bZoom = true;
		m_flUpAndDownValue = flUpAndDownValue;
		m_flRightAndLeftValue = flRightAndLeftValue;
		m_flZoom = flZoom;
		m_flHeightPlayerCenter = flHeightPlayerCenter;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetFlightCamera(EnCameraMode enCameraMode)
	{
		ChangeNewState(enCameraMode);

		m_flHeightPlayerCenter = 1f;
		m_flHeightOffset = m_FlightState.HeightOffset;
		m_flForwardOffset = m_FlightState.ForwardOffset;
		m_flPivot2D_X = m_FlightState.Pivot2D_X;
		m_flPivot2D_Y = m_FlightState.Pivot2D_Y;
		m_flWidthOffset = m_FlightState.WidthOffset;
		m_flLength = m_FlightState.Length;
		m_flHeight = m_FlightState.Height;
		m_flScreenYHeight = m_FlightState.ScreenYHeight;
		m_flUpAndDownValue = m_FlightState.UpAndDown;
		m_flRightAndLeftValue = m_FlightState.RightAndLeft;
		m_flZoom = m_FlightState.Zoom;
		m_flFieldOfView = m_FlightState.FieldOfView;
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeCamera(bool bUpDown, bool bRightLeft, bool bZoom, float flUpAndDown, float flRightAndLeft, float flZoom, float flDuration)
	{
		if (bUpDown)
		{
			m_flAutoUpDownDelayTime = 0f;
			StartCoroutine(ChangeUpAndDownValue(flUpAndDown, flDuration));
		}

		if (bRightLeft)
		{
			m_flAutoDelayTime = Time.time;
			StartCoroutine(ChangeRightAndLeftValue(flRightAndLeft));
		}

		if (bZoom)
		{
			StartCoroutine(ChangeZoomValue(flZoom, flDuration));
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ChangeZoomValue(float flZoom, float flDuration)
	{
		yield return new WaitUntil(() => m_bShaking == false);
		float flTimer = 0f;

		while (flTimer <= flDuration)
		{
			float flTimeRate = flTimer / flDuration;
			ChangeCameraValue(EnCameraValueType.Zoom, flZoom, flTimeRate);
			flTimer += Time.deltaTime;

			if (CsTouchInfo.Instance.Touching || CsTouchInfo.Instance.ZoomByTouch())
			{
				break;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ChangeUpAndDownValue(float flUpAndDown, float flDuration)
	{
		yield return new WaitUntil(() => m_bShaking == false);
		float flTimer = 0f;

		while (flTimer <= flDuration)
		{
			float flTimeRate = flTimer / flDuration;
			ChangeCameraValue(EnCameraValueType.UpAndDown, flUpAndDown, flTimeRate);
			flTimer += Time.deltaTime;

			if (CsTouchInfo.Instance.Touching || CsTouchInfo.Instance.ZoomByTouch())
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ChangeRightAndLeftValue(float flRightAndLeft)
	{
		float flDuration = 1f;
		float flTimer = 0f;
		m_flAutoCameraSpeed = 4f;

		while (flTimer <= flDuration)
		{
			ChangeRightAndLeft(flRightAndLeft); // MyHero 로테이션 값 변환(0 ~ 6.28f)
			flTimer += Time.deltaTime;

			if (CsTouchInfo.Instance.Touching || CsTouchInfo.Instance.ZoomByTouch())
			{
				break;
			}
			yield return new WaitForEndOfFrame();
		}
	}

#endregion CameraEvent

	#region CameraShake

	//---------------------------------------------------------------------------------------------------
	bool CameraShake()
	{
		if (m_bShaking)
		{
			if (CsTouchInfo.Instance.DragByTouch() == false)
			{
				if (Time.time < m_flShakeEndTime)
				{
					transform.position = m_vtOriginalPos + UnityEngine.Random.insideUnitSphere * m_flShakeAmount;
					return true;
				}
			}

			m_bShaking = false;
			transform.position = m_vtOriginalPos;
		}
		
		return false;
	}

	float m_flShakeEndTime = 0f;
	float m_flShakeAmount = 0.1f;
	Vector3 m_vtOriginalPos;
	//---------------------------------------------------------------------------------------------------
	public void DoShake(int nShakeLevel, bool bBlur)
	{
		if (m_bShaking) return;
		if (CsTouchInfo.Instance.DragByTouch()) return;

		switch (nShakeLevel)
		{
		case 1:
			m_flShakeEndTime = Time.time + 0.1f;
			m_flShakeAmount = 0.05f;
			break;
		case 2:
			if (bBlur)
			{
				//PostProcessingStart(false, true);
			}
			m_flShakeEndTime = Time.time + 0.2f;
			m_flShakeAmount = 0.1f;
			break;
		case 3:
			m_flShakeEndTime = Time.time + 0.2f;
			m_flShakeAmount = 0.2f;
			break;
		case 4:		// 테임 몬스터 공격
			m_flShakeEndTime = Time.time + 0.2f;
			m_flShakeAmount = 0.4f;
			break;
		case 10:	// 연출1
			m_flShakeEndTime = Time.time + 0.8f;
			m_flShakeAmount = 0.3f;
			break;
		case 11:	// 연출2
			m_flShakeEndTime = Time.time + 1.2f;
			m_flShakeAmount = 0.4f;
			break;
		}

		m_vtOriginalPos = transform.position;
		m_bShaking = true;
	}

	#endregion CameraShake

	#region HeightMap

	byte[] m_abyHeightInfo = null;
	int m_nHeightMapWidth = 0;
	int m_nHeightMapHeight = 0;
	int m_nHeightMapZeroX = 0;
	int m_nHeightMapZeroY = 0;
	int m_nHeightMapZeroZ = 0;
	//	int m_nMaxHeight = 0;

	//---------------------------------------------------------------------------------------------------
	public float GetMapHeight(Vector3 vtPos)
	{
		if (m_abyHeightInfo == null) return 0;

		int xPos = Mathf.RoundToInt(vtPos.x - m_nHeightMapZeroX);
		int zPos = Mathf.RoundToInt(vtPos.z - m_nHeightMapZeroZ);

		if (xPos < 0 || zPos < 0 || xPos >= m_nHeightMapWidth || zPos >= m_nHeightMapHeight)
		{
			return 0;
		}

		return m_abyHeightInfo[zPos * m_nHeightMapWidth + xPos] + m_nHeightMapZeroY;
	}

	//---------------------------------------------------------------------------------------------------
	private void InitializeHeightInfo() //  해당 씬의 HeightInfo 세팅.
	{
		Texture2D texture = CsIngameData.Instance.LoadAsset<Texture2D>("HeightMap/HeightMap_" + gameObject.scene.name);

		if (texture == null)
		{
			m_bHeightMapPlay = false;
			Debug.Log("InGameCamera.InitializeHeightInfo()       texture == null");
			return;
		}

		m_bHeightMapPlay = true;
		m_nHeightMapWidth = texture.width;
		m_nHeightMapHeight = texture.height;

		GameObject goZero = GameObject.Find("HeightMap_Zero");
		if (goZero != null)
		{
			m_nHeightMapZeroX = Mathf.RoundToInt(goZero.transform.position.x);
			m_nHeightMapZeroY = Mathf.RoundToInt(goZero.transform.position.y);
			m_nHeightMapZeroZ = Mathf.RoundToInt(goZero.transform.position.z);
		}

		Color32[] colors = texture.GetPixels32();
		m_abyHeightInfo = new byte[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			m_abyHeightInfo[i] = colors[i].a;
//			m_nMaxHeight = Mathf.Max(m_abyHeightInfo[i], m_nMaxHeight);
		}

		Debug.Log("InitializeHeightInfo     m_nHeightMapZeroY = "+ m_nHeightMapZeroY);
	}

	#endregion HeightMap

	float m_flAutoDelayTime = 0;
	float m_flAutoCameraSpeed = 4f;

	//---------------------------------------------------------------------------------------------------
	void AutoChaseCamera()
	{
		if (m_enCameraMode != EnCameraMode.CameraAuto) return;
				
		if (CsTouchInfo.Instance.Touching || CsTouchInfo.Instance.ZoomByTouch())
		{
			m_flAutoUpDownDelayTime = 0f;
			m_flAutoDelayTime = Time.time;
		}

		if (m_bShaking || m_bZoom) return;

		if (Time.time > m_flAutoDelayTime + 3f)
		{
			AutoChangeUpAndDown();
			AutoChangeRightAndLeft();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AutoChangeRightAndLeft()
	{
		if (CsIngameData.Instance.IngameManagement.IsHeroStateIdle()) return;

		if (CsIngameData.Instance.IngameManagement.IsHeroStateAttack())	// 이동 가능 공격중 조이스틱 이동시.
		{
			if (CsGameData.Instance.JoystickDragging)
			{
				return;
			}

			m_flAutoCameraSpeed = 13.0f;
		}
		else if (CsIngameData.Instance.IngameManagement.IsHeroStateNoJoyOfMove() || CsGameData.Instance.JoystickDown)	// 조이스틱이동이 아닌 이동상태.
		{
			m_flAutoCameraSpeed = 2.0f;
		}
		else
		{
			if (CsGameData.Instance.JoystickDragging)
			{
				m_flAutoCameraSpeed = 0.5f;
			}
			else
			{
				m_flAutoCameraSpeed = 4.0f;
			}

			if (CsGameData.Instance.JoystickAngle < -0.5f && CsGameData.Instance.JoystickAngle > -2.5f) return;
			if (CsGameData.Instance.JoystickAngle < 2.5f && CsGameData.Instance.JoystickAngle > 0.5f) return;
		}

		ChangeRightAndLeft(CsGameData.Instance.MyHeroTransform.rotation.eulerAngles.y * Mathf.Deg2Rad); // MyHero 로테이션 값 변환(0 ~ 6.28f)
	}

	float m_flAutoUpDownStartTime = 0;
	float m_flAutoUpDownDelayTime = 0;
	//---------------------------------------------------------------------------------------------------
	void AutoChangeUpAndDown()
	{
		if (CsIngameData.Instance.IngameManagement.IsHeroStateIdle())
		{
			if (Time.time > m_flAutoUpDownStartTime + 1.5f)
			{
				float flTimeRate = m_flAutoUpDownDelayTime / 1.5f;

				ChangeCameraValue(EnCameraValueType.UpAndDown, 0.2f, flTimeRate);
				m_flAutoUpDownDelayTime += Time.deltaTime;

				if (m_flAutoUpDownDelayTime >= 1.5f)
				{
					m_flAutoUpDownDelayTime = 0f;
				}
			}
		}
		else
		{
			m_flAutoUpDownStartTime = Time.time;
			m_flAutoUpDownDelayTime = 0f;
			return;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeRightAndLeft(float flRightAndLeftValue) // 플레이어 뒤를 쫓아가는 카메라.
	{
		float flRightAndLeftDelta = flRightAndLeftValue - m_flRightAndLeftValue; // 변경할 값과 변경전 값의 차이.
		float flChangeValue = 0;
		bool bInCrease = false;

		if (flRightAndLeftDelta > 0.01f) // 변경될 값이 현재 값보다 클때
		{
			if (flRightAndLeftDelta > 3.14) // 변경될 각도가 180도가 넘어갈때 반대쪽으로 값 변경.
			{
				flChangeValue = Mathf.Lerp(0, 6.28f - flRightAndLeftDelta, m_flAutoCameraSpeed * Time.deltaTime);
			}
			else // 변경될 각도가 양수로 증가하며 180도가 안될때.
			{
				flChangeValue = Mathf.Lerp(0, flRightAndLeftDelta, m_flAutoCameraSpeed * Time.deltaTime);
				bInCrease = true;
			}
		}
		else if (flRightAndLeftDelta < -0.01f) // 변경될 값이 현재 값보다 작을때.
		{
			if (flRightAndLeftDelta < -3.14)  // 변경될 각도가 180도가 넘어갈때 반대쪽으로 값 변경.
			{
				flChangeValue = Mathf.Lerp(0, 6.28f + flRightAndLeftDelta, m_flAutoCameraSpeed * Time.deltaTime);
				bInCrease = true;
			}
			else// 변경될 각도가 음수로 감소하며 180도가 안될때.
			{
				flChangeValue = Mathf.Lerp(0, flRightAndLeftDelta, m_flAutoCameraSpeed * Time.deltaTime);
			}
		}

		ChangeRightandLeftValue(flChangeValue, bInCrease);
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeRightandLeftValue(float flChangeValue, bool bInCrease)
	{
		if (m_bZoom || m_bShaking) return;
		
		flChangeValue = Mathf.Abs(flChangeValue);
		if (bInCrease) // 값 증가.
		{
			if (m_flRightAndLeftValue + flChangeValue > 6.28f)
			{
				m_flRightAndLeftValue = 0;
			}
			else
			{
				m_flRightAndLeftValue += flChangeValue;
			}
		}
		else // 값 감소.
		{
			if (m_flRightAndLeftValue + flChangeValue < 0)
			{
				m_flRightAndLeftValue = 6.28f;
			}
			else
			{
				m_flRightAndLeftValue -= flChangeValue;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetNearClipPlane()
	{
		m_camera.nearClipPlane = m_flDefaultNearClipPlane;
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetFarClipPlane()
	{
		m_camera.farClipPlane = m_flDefaultFarClipPlane;
	}

	#region Setting

	//---------------------------------------------------------------------------------------------------
	protected bool ChangeCameraValue(EnCameraValueType enCameraValueType, float flVlue, float flTimeRate)
	{
		if (m_bZoom || m_bShaking) return false;

		switch (enCameraValueType)
		{
			case EnCameraValueType.HeightOffset:
				if (m_flHeightOffset != flVlue)
				{
					m_flHeightOffset = Mathf.Lerp(m_flHeightOffset, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.WidthOffset:
				if (m_flWidthOffset != flVlue)
				{
					m_flWidthOffset = Mathf.Lerp(m_flWidthOffset, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.ForwardOffset:
				if (m_flForwardOffset != flVlue)
				{
					m_flForwardOffset = Mathf.Lerp(m_flForwardOffset, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.ZoomOffset:
				if (m_flZoomOffset != flVlue)
				{
					m_flZoomOffset = Mathf.Lerp(m_flZoomOffset, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.Length:
				if (m_flLength != flVlue)
				{
					m_flLength = Mathf.Lerp(m_flLength, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.Height:
				if (m_flHeight != flVlue)
				{
					m_flHeight = Mathf.Lerp(m_flHeight, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.ScreenYHeight:
				if (m_flScreenYHeight != flVlue)
				{
					m_flScreenYHeight = Mathf.Lerp(m_flScreenYHeight, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.Pivot2D_X:
				if (m_flPivot2D_X != flVlue)
				{
					m_flPivot2D_X = Mathf.Lerp(m_flPivot2D_X, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.Pivot2D_Y:
				if (m_flPivot2D_Y != flVlue)
				{
					m_flPivot2D_Y = Mathf.Lerp(m_flPivot2D_Y, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.UpAndDown:
				if (m_flUpAndDownValue != flVlue)
				{
					m_flUpAndDownValue = Mathf.Lerp(m_flUpAndDownValue, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.RightAndLeft:
				if (m_flRightAndLeftValue != flVlue)
				{
					m_flRightAndLeftValue = Mathf.Lerp(m_flRightAndLeftValue, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.Zoom:
				if (m_flZoom != flVlue)
				{
					m_flZoom = Mathf.Lerp(m_flZoom, flVlue, flTimeRate);
					return true;
				}
				break;
			case EnCameraValueType.FieldOfView:
				if (m_flFieldOfView != flVlue)
				{
					m_flFieldOfView = Mathf.Lerp(m_flFieldOfView, flVlue, flTimeRate);
					return true;
				}
				break;
		}

		return false;
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetCameraValue(CsCameraValue csCameraValue)
	{
		m_flHeightOffset = csCameraValue.HeightOffset;
		m_flForwardOffset = csCameraValue.ForwardOffset;
		m_flPivot2D_X = csCameraValue.Pivot2D_X;
		m_flPivot2D_Y = csCameraValue.Pivot2D_Y;
		m_flWidthOffset = csCameraValue.WidthOffset;
		m_flLength = csCameraValue.Length;
		m_flHeight = csCameraValue.Height;
		m_flScreenYHeight = csCameraValue.ScreenYHeight;
		m_flUpAndDownValue = csCameraValue.UpAndDown;
		m_flZoom = csCameraValue.Zoom;
		m_flFieldOfView = csCameraValue.FieldOfView;
	}

	#endregion Setting
}
