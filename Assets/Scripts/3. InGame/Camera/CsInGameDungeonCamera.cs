using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CsInGameDungeonCamera : CsInGameCamera
{
	CsCameraValue m_StoryDungeonDefault;
	CsCameraValue m_StoryDungeonStartAction;
	CsCameraValue m_OsirisRoomDungeonDefault;
	CsCameraValue m_AnkouDungeonDefault;
	CsCameraValue m_AnkouDungeonStartAction;
	DepthOfField m_depthOfField;

	float m_flStartTime;
	float m_flDelayTime;

	bool m_bTaming = false;

	[SerializeField]
	EnDungeonPlay m_enDungeonPlay = EnDungeonPlay.None;

	//---------------------------------------------------------------------------------------------------
	protected override void Awake()
	{
		base.Awake();

		CsIngameData.Instance.InGameDungeonCamera = this;

		m_depthOfField = transform.GetComponent<DepthOfField>();
		if (m_depthOfField != null)
		{
			m_depthOfField.enabled = false;
		}
	}
	//---------------------------------------------------------------------------------------------------
	protected override void Start()
	{
		m_enDungeonPlay = CsDungeonManager.Instance.DungeonPlay;
		if (m_enDungeonPlay == EnDungeonPlay.Story)
		{
			Debug.Log("스토리 던전 세팅.");			
			m_StoryDungeonDefault = new CsCameraValue(2f, 1f, 0f, 0f, 7f, 5.5f, 0.1f, 0f, 0f, 0f, 1.57079f, 1.15f, 40f);

			m_flWidthOffset = m_StoryDungeonDefault.WidthOffset;
			m_flForwardOffset = m_StoryDungeonDefault.ForwardOffset;
			m_flScreenYHeight = m_StoryDungeonDefault.ScreenYHeight;
			m_flHeightOffset = m_StoryDungeonDefault.HeightOffset;

			m_flLength = m_flOrgLength = m_StoryDungeonDefault.Length;
			m_flHeight = m_flOrgHeight = m_StoryDungeonDefault.Height;
			m_flPivot2D_X = m_flOrgPivot2D_X = m_StoryDungeonDefault.Pivot2D_X;
			m_flPivot2D_Y = m_flOrgPivot2D_Y = m_StoryDungeonDefault.Pivot2D_Y;
			m_flUpAndDownValue = m_flOrgUpAndDownValue = m_StoryDungeonDefault.UpAndDown;
			m_flRightAndLeftValue = m_StoryDungeonDefault.RightAndLeft;
			m_flZoom = m_flOrgZoom = m_StoryDungeonDefault.Zoom;

			ChangeDungeonCameraState(EnCameraPlay.Normal);
		}
		else if (m_enDungeonPlay == EnDungeonPlay.OsirisRoom)
		{
			Debug.Log("오시리스룸 던전 세팅.");			
			m_OsirisRoomDungeonDefault = new CsCameraValue(2f, 1f, 0f, 0f, 7f, 5.5f, 0.1f, 0f, 0f, 0f, 1.57079f, 1.15f, 40f);

			m_flWidthOffset = m_OsirisRoomDungeonDefault.WidthOffset;
			m_flForwardOffset = m_OsirisRoomDungeonDefault.ForwardOffset;
			m_flScreenYHeight = m_OsirisRoomDungeonDefault.ScreenYHeight;
			m_flHeightOffset = m_OsirisRoomDungeonDefault.HeightOffset;

			m_flLength = m_flOrgLength = m_OsirisRoomDungeonDefault.Length;
			m_flHeight = m_flOrgHeight = m_OsirisRoomDungeonDefault.Height;
			m_flPivot2D_X = m_flOrgPivot2D_X = m_OsirisRoomDungeonDefault.Pivot2D_X;
			m_flPivot2D_Y = m_flOrgPivot2D_Y = m_OsirisRoomDungeonDefault.Pivot2D_Y;
			m_flUpAndDownValue = m_flOrgUpAndDownValue = m_OsirisRoomDungeonDefault.UpAndDown;
			m_flRightAndLeftValue = m_OsirisRoomDungeonDefault.RightAndLeft;
			m_flZoom = m_flOrgZoom = m_OsirisRoomDungeonDefault.Zoom;

			m_flHeightPlayerCenter = 0;
			m_trCameraTarget = CsGameData.Instance.MyHeroTransform;

			ChangeDungeonCameraState(EnCameraPlay.Normal);
		}
		else if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
		{
			base.Start();
			m_AnkouDungeonDefault = new CsCameraValue(2f, 0f, 0f, 0f, 7f, 5.5f, 0.1f, 0f, 0f, 0f, 1.57079f, 1.15f, 40f);
			ChangeDungeonCameraState(EnCameraPlay.Normal);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void Update()
	{
		if (CsGameData.Instance.MyHeroTransform == null) return;
		if (m_trCameraTarget == null)
		{
			m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
			return;
		}	

		if (m_enDungeonPlay == EnDungeonPlay.Story)
		{
			m_flHeightPlayerCenter = 0;

			if (m_enDungeonCameraPlay == EnCameraPlay.Normal)
			{
				CameraDefault();
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.Acceleartion)
			{
				Accelerate();
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.AttackRush)
			{
				return;
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.EnterAction1)
			{
				EnterAction(EnCameraPlay.EnterAction1);
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.EnterAction2)
			{
				EnterAction(EnCameraPlay.EnterAction2);
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.Clear)
			{
				DungeonClear();
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.Domination)
			{
				Domination();
			}
		}
		else if (m_enDungeonPlay == EnDungeonPlay.OsirisRoom)
		{
			if (m_enDungeonCameraPlay == EnCameraPlay.Clear)
			{
				DungeonClear();
			}
		}
		else if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
		{
			if (m_enDungeonCameraPlay == EnCameraPlay.Normal)
			{
				CameraDefault();
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.EnterAction1)
			{
				EnterAction(EnCameraPlay.EnterAction1);
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.EnterAction2)
			{
				EnterAction(EnCameraPlay.EnterAction2);
			}
			else if (m_enDungeonCameraPlay == EnCameraPlay.Clear)
			{
				DungeonClear();
			}
			else
			{
				base.Update();
			}
		}

		UpdateCameraPos();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		base.OnDestroy();
		m_StoryDungeonDefault = null;
		m_StoryDungeonStartAction = null;
		m_OsirisRoomDungeonDefault = null;
		m_AnkouDungeonDefault = null;
		m_AnkouDungeonStartAction = null;
		m_depthOfField = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeDungeonCameraState(EnCameraPlay enCameraPlay, bool bHangOn = false)
	{
		if (m_enDungeonPlay == EnDungeonPlay.Story)
		{
			ChangeStoryDungeonCameraState(enCameraPlay, bHangOn);
		}
		else if (m_enDungeonPlay == EnDungeonPlay.OsirisRoom)
		{
			m_flDelayWatingTime = 0.0f;
			m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
			m_flHeightPlayerCenter = 0;
			m_enDungeonCameraPlay = enCameraPlay;
		}
		else if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
		{
			switch (enCameraPlay)
			{
				case EnCameraPlay.Normal:
					m_depthOfField.enabled = false;
					m_flStartTime = 0;
					m_flDelayTime = 0;					
					break;
				case EnCameraPlay.EnterAction1:
					m_flHeight = 1f;
					m_flLength = 2.5f;
					m_flScreenYHeight = 0.6f;
					m_flZoom = 1.25f;
					m_depthOfField.focalTransform = CsGameData.Instance.MyHeroTransform;
					m_depthOfField.focalSize = 2.0f;
					m_depthOfField.aperture = 0.9f;
					m_depthOfField.enabled = true;

					m_AnkouDungeonStartAction = new CsCameraValue(m_flDelayTime, m_flHeightOffset, m_flWidthOffset, m_flForwardOffset, m_flLength, m_flHeight, m_flScreenYHeight, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, m_flFieldOfView);
					m_flStartTime = Time.time;
					m_flDelayTime = 1.3f;
					break;
				case EnCameraPlay.EnterAction2:
					m_AnkouDungeonStartAction = new CsCameraValue(m_flDelayTime, m_flHeightOffset, m_flWidthOffset, m_flForwardOffset, m_flLength, m_flHeight, m_flScreenYHeight, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, m_flFieldOfView);
					m_flStartTime = Time.time;
					m_flDelayTime = 0.2f;
					break;
				case EnCameraPlay.Clear:
					m_flHeightPlayerCenter = 0;
					break;
			}

			m_flDelayWatingTime = 0.0f;
			m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
			m_flHeightPlayerCenter = CsIngameData.Instance.HeroCenter;
			m_enDungeonCameraPlay = enCameraPlay;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeStoryDungeonCameraState(EnCameraPlay enCameraPlay, bool bHangOn = false)
	{
		Debug.Log("#####################                              ChangeCameraPlay     enCameraPlay = " + enCameraPlay + "  //  bHangOn = " + bHangOn);
		if (m_enDungeonCameraPlay == enCameraPlay && m_bTaming == bHangOn && m_depthOfField == null) return;

		m_flDelayWatingTime = 0.0f;

		switch (m_enDungeonCameraPlay)
		{
			case EnCameraPlay.Acceleartion:
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
				m_depthOfField.enabled = false;
				break;
			case EnCameraPlay.AttackRush:
			case EnCameraPlay.EnterAction1:
			case EnCameraPlay.EnterAction2:
				break;
			case EnCameraPlay.Domination:
				ResetNearClipPlane();
				if (m_bTaming == false) // 내릴때.
				{
					CsGameEventToUI.Instance.OnEventHideMainUI(false);
				}
				break;
			case EnCameraPlay.BossAppear:
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
				m_depthOfField.enabled = false;
				break;
			case EnCameraPlay.BossDead:
				Time.timeScale = 1f;
				break;
			case EnCameraPlay.Clear:
				break;
		}


		switch (enCameraPlay)
		{
			case EnCameraPlay.None:
			case EnCameraPlay.Normal:
				Time.timeScale = 1f;
				m_depthOfField.enabled = false;
				m_flStartTime = 0;
				m_flDelayTime = 0;

				if (m_bTaming)
				{
					Transform trTarget = CsIngameData.Instance.TameMonster.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Thigh");
					m_flScreenYHeight = m_flHeightPlayerCenter - (trTarget.position.y - CsGameData.Instance.MyHeroTransform.position.y);
					m_trCameraTarget = trTarget;
				}
				else
				{
					m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
				}
				break;
			case EnCameraPlay.Acceleartion:
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform.GetComponent<CsHero>().HeroPivot.Find("RunTarget");
				Debug.Log("ChangeCameraPlay  >>   EnCameraPlay.Acceleartion    m_trCameraTarget = " + m_trCameraTarget + " // m_trCameraTarget.name" + m_trCameraTarget.name);
				m_depthOfField.focalTransform = m_trCameraTarget;
				m_depthOfField.focalSize = 2f;
				m_depthOfField.aperture = 0.8f;
				m_depthOfField.enabled = true;
				break;
			case EnCameraPlay.AttackRush:
				break;
			case EnCameraPlay.EnterAction1:
				m_flHeight = 1f;
				m_flLength = 2.5f;
				m_flScreenYHeight = 0.6f;
				m_flZoom = 1.25f;
				m_depthOfField.focalTransform = CsGameData.Instance.MyHeroTransform;
				m_depthOfField.focalSize = 2.0f;
				m_depthOfField.aperture = 0.9f;
				m_depthOfField.enabled = true;
				m_StoryDungeonStartAction = new CsCameraValue(m_flDelayTime, m_flHeightOffset, m_flWidthOffset, m_flForwardOffset, m_flLength, m_flHeight, m_flScreenYHeight, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, m_flFieldOfView);
				m_flStartTime = Time.time;
				m_flDelayTime = 1.3f;
				break;
			case EnCameraPlay.EnterAction2:
				m_StoryDungeonStartAction = new CsCameraValue(m_flDelayTime, m_flHeightOffset, m_flWidthOffset, m_flForwardOffset, m_flLength, m_flHeight, m_flScreenYHeight, m_flPivot2D_X, m_flPivot2D_Y, m_flUpAndDownValue, m_flRightAndLeftValue, m_flZoom, m_flFieldOfView);
				m_flStartTime = Time.time;
				m_flDelayTime = 0.2f;
				break;
			case EnCameraPlay.Domination:
				Debug.Log("ChangeCameraPlay     enCameraPlay = " + enCameraPlay + " // bHangOn = " + bHangOn);
				CsGameEventToUI.Instance.OnEventHideMainUI(true);
				m_camera.nearClipPlane = 2f;
				m_flScreenYHeight = 0.1f;
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
				
				break;
			case EnCameraPlay.BossAppear:
				m_flForwardOffset = 7f;//8f;
				m_flLength = 3.5f;
				m_flHeight = 0f;
				m_flScreenYHeight = 1.8f;
				m_flPivot2D_X = 0f;
				m_flPivot2D_Y = 0.9f;
				m_flUpAndDownValue = 0f;
				m_flRightAndLeftValue = 1.57079f;
				m_flZoom = 1.0f; // 1.38f;
				m_flFieldOfView = m_StoryDungeonDefault.FieldOfView;
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform;

				m_depthOfField.focalTransform = CsIngameData.Instance.TargetTransform;
				m_depthOfField.focalSize = 5f;
				m_depthOfField.aperture = 0.847f;
				m_depthOfField.enabled = true;
				break;
			case EnCameraPlay.BossDead:
				Time.timeScale = 0.5f;
				break;
			case EnCameraPlay.Clear:
				break;
		}

		m_bTaming = bHangOn;
		m_enDungeonCameraPlay = enCameraPlay;
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
		Vector3 vtTarget = m_vtCenter + (vtForward * m_flPivot2D_Y) + (vtRight * m_flPivot2D_X);
		Vector3 vtCameraPos = vtTarget + vtOffset * (2f - (m_flZoom + m_flZoomOffset));

		transform.position = vtCameraPos;
		m_camera.fieldOfView = m_flFieldOfView;
		transform.LookAt(m_vtCenter);
	}

	//---------------------------------------------------------------------------------------------------
	void CameraDefault() // 기본 카메라.
	{
		if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
		{
			float flTimeRate = m_flDelayWatingTime / m_AnkouDungeonDefault.Duration;

			ChangeCameraValue(EnCameraValueType.HeightOffset, m_AnkouDungeonDefault.HeightOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.ForwardOffset, m_AnkouDungeonDefault.ForwardOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Pivot2D_X, m_AnkouDungeonDefault.Pivot2D_X, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Pivot2D_Y, m_AnkouDungeonDefault.Pivot2D_Y, flTimeRate);
			ChangeCameraValue(EnCameraValueType.WidthOffset, m_AnkouDungeonDefault.WidthOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Length, m_AnkouDungeonDefault.Length, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Height, m_AnkouDungeonDefault.Height, flTimeRate);
			ChangeCameraValue(EnCameraValueType.ScreenYHeight, m_AnkouDungeonDefault.ScreenYHeight, flTimeRate);
			ChangeCameraValue(EnCameraValueType.UpAndDown, m_AnkouDungeonDefault.UpAndDown, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Zoom, m_AnkouDungeonDefault.Zoom, flTimeRate);
			ChangeCameraValue(EnCameraValueType.FieldOfView, m_AnkouDungeonDefault.FieldOfView, flTimeRate);

			m_flDelayWatingTime += Time.deltaTime;

			if (m_flDelayWatingTime >= m_AnkouDungeonDefault.Duration)
			{
				m_flDelayWatingTime = 0.0f;
				SetCameraValue(m_AnkouDungeonDefault);
				ChangeDungeonCameraState(EnCameraPlay.None);
			}
		}
		else
		{
			if (!m_bTaming)  // 탑승중이 아닐때.
			{
				m_trCameraTarget = CsGameData.Instance.MyHeroTransform;
			}

			float flTimeRate = m_flDelayWatingTime / m_StoryDungeonDefault.Duration;


			ChangeCameraValue(EnCameraValueType.HeightOffset, m_StoryDungeonDefault.HeightOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.ForwardOffset, m_StoryDungeonDefault.ForwardOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Pivot2D_X, m_StoryDungeonDefault.Pivot2D_X, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Pivot2D_Y, m_StoryDungeonDefault.Pivot2D_Y, flTimeRate);
			ChangeCameraValue(EnCameraValueType.WidthOffset, m_StoryDungeonDefault.WidthOffset, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Length, m_StoryDungeonDefault.Length, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Height, m_StoryDungeonDefault.Height, flTimeRate);
			ChangeCameraValue(EnCameraValueType.ScreenYHeight, m_StoryDungeonDefault.ScreenYHeight, flTimeRate);
			ChangeCameraValue(EnCameraValueType.UpAndDown, m_StoryDungeonDefault.UpAndDown, flTimeRate);
			ChangeCameraValue(EnCameraValueType.RightAndLeft, m_StoryDungeonDefault.RightAndLeft, flTimeRate);
			ChangeCameraValue(EnCameraValueType.Zoom, m_StoryDungeonDefault.Zoom, flTimeRate);
			ChangeCameraValue(EnCameraValueType.FieldOfView, m_StoryDungeonDefault.FieldOfView, flTimeRate);

			m_flDelayWatingTime += Time.deltaTime;

			if (m_flDelayWatingTime >= m_StoryDungeonDefault.Duration)
			{
				m_flDelayWatingTime = 0.0f;
				SetCameraValue(m_StoryDungeonDefault);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Accelerate() // 이동중 가속.
	{
		float flTimeRate = m_flDelayWatingTime / m_StoryDungeonDefault.Duration;

		ChangeCameraValue(EnCameraValueType.HeightOffset, m_StoryDungeonDefault.HeightOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ForwardOffset, m_StoryDungeonDefault.ForwardOffset, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_X, m_StoryDungeonDefault.Pivot2D_X, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Pivot2D_Y, m_StoryDungeonDefault.Pivot2D_Y, flTimeRate);

		ChangeCameraValue(EnCameraValueType.WidthOffset, -0.75f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Length, 3.5f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Height, 1.3f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.ScreenYHeight, -0.75f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.UpAndDown, 0.0f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Zoom, 1.65f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.FieldOfView, 80f, flTimeRate);
		
		m_flDelayWatingTime += Time.deltaTime;

		if (m_flDelayWatingTime >= m_StoryDungeonDefault.Duration)
		{
			m_flDelayWatingTime = 0.8f;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Domination() // 몬스터 탑승 연출 카메라.
	{
		float flTimeRate = m_flDelayWatingTime / 40f;

		ChangeCameraValue(EnCameraValueType.ForwardOffset, 2.5f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Length, 8f, flTimeRate);		
		ChangeCameraValue(EnCameraValueType.ScreenYHeight, 0.1f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.UpAndDown, 0.1f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.RightAndLeft, 2.7f, flTimeRate);
		ChangeCameraValue(EnCameraValueType.Zoom, 1.2f, flTimeRate);
		
		m_flDelayWatingTime += Time.deltaTime;

		if (m_flDelayWatingTime >= m_StoryDungeonDefault.Duration)
		{
			m_flDelayWatingTime = m_StoryDungeonDefault.Duration;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void EnterAction(EnCameraPlay enCameraPlay)
	{
		if (Time.time >= m_flStartTime + m_flDelayTime)
		{
			if (m_enDungeonPlay == EnDungeonPlay.Story)
			{
				if (enCameraPlay == EnCameraPlay.EnterAction1)
				{
					float flTimeRate = m_flDelayWatingTime / 1.2f;

					m_flLength = Mathf.Lerp(m_StoryDungeonStartAction.Length, 3.5f, flTimeRate);
					m_flHeight = Mathf.Lerp(m_StoryDungeonStartAction.Height, 2.2f, flTimeRate);
					m_flScreenYHeight = Mathf.Lerp(m_StoryDungeonStartAction.ScreenYHeight, 0.3f, flTimeRate);
					m_flZoom = Mathf.Lerp(m_StoryDungeonStartAction.Zoom, 1.05f, flTimeRate);

					m_flDelayWatingTime += Time.deltaTime;
					if (m_flDelayWatingTime >= 1.2f)
					{
						m_flDelayWatingTime = 1.2f;
					}
				}
				else if (enCameraPlay == EnCameraPlay.EnterAction2)
				{
					float flTimeRate = m_flDelayWatingTime / 0.6f;

					m_flHeight = Mathf.Lerp(m_StoryDungeonStartAction.Height, 5.5f, flTimeRate);
					m_flLength = Mathf.Lerp(m_StoryDungeonStartAction.Length, 7f, flTimeRate);
					m_flScreenYHeight = Mathf.Lerp(m_StoryDungeonStartAction.ScreenYHeight, 0.1f, flTimeRate);
					m_flZoom = Mathf.Lerp(m_StoryDungeonStartAction.Zoom, 1.15f, flTimeRate);

					m_flDelayWatingTime += Time.deltaTime;
					if (m_flDelayWatingTime >= 0.6f)
					{
						m_flDelayWatingTime = 0.6f;
					}
				}
			}
			else if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
			{
				if (enCameraPlay == EnCameraPlay.EnterAction1)
				{
					float flTimeRate = m_flDelayWatingTime / 1.2f;

					m_flLength = Mathf.Lerp(m_AnkouDungeonStartAction.Length, 3.5f, flTimeRate);
					m_flHeight = Mathf.Lerp(m_AnkouDungeonStartAction.Height, 2.2f, flTimeRate);
					m_flScreenYHeight = Mathf.Lerp(m_AnkouDungeonStartAction.ScreenYHeight, 0.3f, flTimeRate);
					m_flZoom = Mathf.Lerp(m_AnkouDungeonStartAction.Zoom, 1.05f, flTimeRate);

					m_flDelayWatingTime += Time.deltaTime;
					if (m_flDelayWatingTime >= 1.2f)
					{
						m_flDelayWatingTime = 1.2f;
					}
				}
				else if (enCameraPlay == EnCameraPlay.EnterAction2)
				{
					float flTimeRate = m_flDelayWatingTime / 0.6f;

					m_flHeight = Mathf.Lerp(m_AnkouDungeonStartAction.Height, 5.5f, flTimeRate);
					m_flLength = Mathf.Lerp(m_AnkouDungeonStartAction.Length, 7f, flTimeRate);
					m_flScreenYHeight = Mathf.Lerp(m_AnkouDungeonStartAction.ScreenYHeight, 0.1f, flTimeRate);
					m_flZoom = Mathf.Lerp(m_AnkouDungeonStartAction.Zoom, 1.15f, flTimeRate);

					m_flDelayWatingTime += Time.deltaTime;
					if (m_flDelayWatingTime >= 0.6f)
					{
						m_flDelayWatingTime = 0.6f;
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DungeonClear()
	{
		float flTimeRate = m_flDelayWatingTime / 3f;

		if (m_enDungeonPlay == EnDungeonPlay.Story)
		{
			m_flWidthOffset = Mathf.Lerp(-0.3f, 0.0f, flTimeRate);
			m_flForwardOffset = Mathf.Lerp(0.25f, 0.0f, flTimeRate);
			m_flLength = Mathf.Lerp(3.5f, 3f, flTimeRate);
			m_flHeight = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flScreenYHeight = Mathf.Lerp(0.3f, 0.53f, flTimeRate);
			m_flPivot2D_X = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flPivot2D_Y = Mathf.Lerp(0.9f, 2.3f, flTimeRate);
			m_flUpAndDownValue = Mathf.Lerp(0.2f, 0f, flTimeRate);
			m_flRightAndLeftValue = Mathf.Lerp(3.3f, 1.57079f, flTimeRate);
			m_flZoom = Mathf.Lerp(1.38f, 1f, flTimeRate);
		}
		else if (m_enDungeonPlay == EnDungeonPlay.OsirisRoom)
		{
			m_flWidthOffset = Mathf.Lerp(-0.3f, 0.0f, flTimeRate);
			m_flForwardOffset = Mathf.Lerp(0.25f, 0.0f, flTimeRate);
			m_flLength = Mathf.Lerp(3.5f, 3f, flTimeRate);
			m_flHeight = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flScreenYHeight = Mathf.Lerp(0.3f, 0.53f, flTimeRate);
			m_flPivot2D_X = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flPivot2D_Y = Mathf.Lerp(0.9f, 2.3f, flTimeRate);
			m_flUpAndDownValue = Mathf.Lerp(0.2f, 0f, flTimeRate);
			m_flRightAndLeftValue = Mathf.Lerp(4.8f, 3.2f, flTimeRate);
			m_flZoom = Mathf.Lerp(1.38f, 1f, flTimeRate);
		}
		else if (m_enDungeonPlay == EnDungeonPlay.AnkouTomb || m_enDungeonPlay == EnDungeonPlay.TradeShip)
		{
			m_flWidthOffset = Mathf.Lerp(-0.3f, 0.0f, flTimeRate);
			m_flForwardOffset = Mathf.Lerp(0.25f, 0.0f, flTimeRate);
			m_flLength = Mathf.Lerp(3.5f, 3f, flTimeRate);
			m_flHeight = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flScreenYHeight = Mathf.Lerp(0.3f, 0.53f, flTimeRate);
			m_flPivot2D_X = Mathf.Lerp(0f, 0f, flTimeRate);
			m_flPivot2D_Y = Mathf.Lerp(0.9f, 2.3f, flTimeRate);
			m_flUpAndDownValue = Mathf.Lerp(0.2f, 0f, flTimeRate);
			m_flRightAndLeftValue = Mathf.Lerp(1.6f, 0, flTimeRate);
			m_flZoom = Mathf.Lerp(1.38f, 1f, flTimeRate);
		}

		m_flDelayWatingTime += Time.deltaTime;

		if (m_flDelayWatingTime >= 3)
		{
			m_flDelayWatingTime = 3;
		}
	}
}
