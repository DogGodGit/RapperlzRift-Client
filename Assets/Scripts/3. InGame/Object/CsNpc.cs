using UnityEngine;
using UnityEngine.AI;

public class CsNpc : CsMoveUnit, INpcObjectInfo
{
	public enum EnState { Idle, Talk }
	public enum EnAnimStatus { Idle01 = 0, Idle02, Talk }

	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	static int s_nAnimatorHash_talk = Animator.StringToHash("talk");

	CsNpcInfo m_csNpcInfo;
	CsUndergroundMazeNpc m_csUndergroundMazeNpc;
    CsGuildTerritoryNpc m_csGuildTerritoryNpc;
	CsNationWarNpc m_csNationWarNpc;

	CapsuleCollider m_capsuleCollider;
	AudioSource m_audioSource;
	AudioClip m_acVoice = null;
	RectTransform m_rtfHUD = null;

	[SerializeField]
	bool m_bAreaEnter = false;
	[SerializeField]
	int m_nNpcId;
	[SerializeField]
	bool m_bInteractionNpc = false;

	float m_flIdleTimer = 0f;
	float m_flRotationY;

	EnState m_enState = EnState.Idle;
	public CsNpcInfo NpcInfo { get { return m_csNpcInfo; } }
	public int NpcId { get { return m_nNpcId; }  }

	//---------------------------------------------------------------------------------------------------
	public void InitNpc(CsNpcInfo csNpcInfo)
	{
		CsDimensionRaidQuestManager.Instance.EventUpdateState += OnEventUpdateState;

		SetComponent();
		m_acVoice = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Object/" + csNpcInfo.SoundName);

		m_csNpcInfo = csNpcInfo;
		m_nNpcId = csNpcInfo.NpcId;
		InstanceId = csNpcInfo.NpcId;
		Name = csNpcInfo.PrefabName;
		Height = csNpcInfo.Height;
		Radius = csNpcInfo.Radius;

		transform.name = InstanceId.ToString();
		transform.position = csNpcInfo.Position;
		m_flRotationY = csNpcInfo.YRotation;
		ChangeEulerAngles(m_flRotationY);
		transform.localScale = new Vector3(csNpcInfo.Scale, csNpcInfo.Scale, csNpcInfo.Scale);

		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateNpcHUD((int)InstanceId);
		NavMeshSetting();

		CsGameData.Instance.ListNpcObjectInfo.Add(this);

		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;

		//m_capsuleCollider.center = new Vector3(0f,0f, 0f);
		//m_capsuleCollider.height = 0;
		m_capsuleCollider.radius = csNpcInfo.InteractionMaxRange - 1f;

		if (CsSecretLetterQuestManager.Instance.CheckInteractionNpc((int)InstanceId) ||
			CsMysteryBoxQuestManager.Instance.CheckInteractionNpc((int)InstanceId) ||
			CsDimensionRaidQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
		{
			if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam != CsGameData.Instance.MyHeroInfo.Nation.NationId)
			{
				m_bInteractionNpc = true;
			}
			else
			{
				m_capsuleCollider.enabled = false;
				m_capsuleCollider.isTrigger = false;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InitDungeonNpc(CsUndergroundMazeNpc csUndergroundMazeNpc)
	{
		SetComponent();
		
		m_csUndergroundMazeNpc = csUndergroundMazeNpc;

		m_nNpcId = csUndergroundMazeNpc.NpcId;
		InstanceId = csUndergroundMazeNpc.NpcId;
		m_flRotationY = csUndergroundMazeNpc.YRotation;
		Name = csUndergroundMazeNpc.Name;
		Radius = csUndergroundMazeNpc.Radius;
		Height = csUndergroundMazeNpc.Height;

		transform.name = InstanceId.ToString();
		transform.position = csUndergroundMazeNpc.Position;
		ChangeEulerAngles(m_flRotationY);
		transform.localScale = new Vector3(csUndergroundMazeNpc.Scale, csUndergroundMazeNpc.Scale, csUndergroundMazeNpc.Scale);

		m_rtfHUD = CsDungeonManager.Instance.CreateUndergroundMazeNpcHUD((int)InstanceId);
		NavMeshSetting();

		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_capsuleCollider.radius = m_csUndergroundMazeNpc.InteractionMaxRange - 1f;

		CsGameData.Instance.ListNpcObjectInfo.Add(this);
	}

    //---------------------------------------------------------------------------------------------------
    public void InitGuildTerritoryNpc(CsGuildTerritoryNpc csGuildTerritoryNpc)
    {
		SetComponent();

		CsGuildManager.Instance.EventUpdateFarmQuestState += OnEventUpdateFarmQuestState;
		m_csGuildTerritoryNpc = csGuildTerritoryNpc;

		m_nNpcId = csGuildTerritoryNpc.NpcId;
		InstanceId = csGuildTerritoryNpc.NpcId;
		Name = csGuildTerritoryNpc.Name;
		Radius = csGuildTerritoryNpc.Radius;
		Height = csGuildTerritoryNpc.Height;
	
        transform.name = InstanceId.ToString();
        transform.position = csGuildTerritoryNpc.Position;
		m_flRotationY = csGuildTerritoryNpc.YRotation;
		ChangeEulerAngles(m_flRotationY);
        transform.localScale = new Vector3(csGuildTerritoryNpc.Scale, csGuildTerritoryNpc.Scale, csGuildTerritoryNpc.Scale);

        m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateGuildNpcHUD((int)InstanceId);
		NavMeshSetting();

		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_capsuleCollider.radius = m_csGuildTerritoryNpc.InteractionMaxRange - 1f;

		
		if (CsGameData.Instance.GuildFarmQuest.TargetGuildTerritoryNpc != null)
		{
			if (CsGameData.Instance.GuildFarmQuest.TargetGuildTerritoryNpc.NpcId == m_nNpcId)
			{
				m_bInteractionNpc = true;
			}
		}

        CsGameData.Instance.ListNpcObjectInfo.Add(this);
    }

	//---------------------------------------------------------------------------------------------------
	public void InitNationWarNpc(CsNationWarNpc csNationWarNpc)
	{
		Debug.Log("###################### InitNationWarNpc ####################");
		SetComponent();

		m_csNationWarNpc = csNationWarNpc;
		m_nNpcId = csNationWarNpc.NpcId;
		InstanceId = csNationWarNpc.NpcId;
		Name = csNationWarNpc.Name;
		Radius = csNationWarNpc.Radius;
		Height = csNationWarNpc.Height;

		transform.name = InstanceId.ToString();
		transform.position = csNationWarNpc.Position;

		m_flRotationY = csNationWarNpc.YRotation;
		ChangeEulerAngles(m_flRotationY);
		transform.localScale = new Vector3(csNationWarNpc.Scale, csNationWarNpc.Scale, csNationWarNpc.Scale);

		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateNationWarNpcHUD((int)InstanceId);
		NavMeshSetting();

		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_capsuleCollider.radius = m_csNationWarNpc.InteractionMaxRange - 1f;

		CsGameData.Instance.ListNpcObjectInfo.Add(this);
	}

	//----------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		if (m_animator == null || CsGameData.Instance.MyHeroTransform == null) return;

		if (m_rtfHUD != null && m_rtfHUD.gameObject.activeInHierarchy)
		{
			if (m_timer.CheckSetTimer())
			{
				if (CheckNpcInfo())
				{
					if (m_enState == EnState.Idle) // 모션 렌덤 재생.
					{
						if (m_flIdleTimer + Random.Range(8, 10) < Time.time)
						{
							m_flIdleTimer = Time.time;

							if (Random.Range(0, 10) > 4)
							{
								if (!GetAnimStatus().Equals(EnAnimStatus.Idle02))
								{
									SetAnimStatus(EnAnimStatus.Idle02);
								}
							}
						}
						else
						{
							if (GetAnimStatus().Equals(EnAnimStatus.Idle02))
							{
								SetAnimStatus(EnAnimStatus.Idle01);
							}
						}
					}
				}
			}
		}

		if (CsIngameData.Instance.ActiveScene)
		{
			HUDUpdatePos();
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		CsDimensionRaidQuestManager.Instance.EventUpdateState -= OnEventUpdateState;
		CsGuildManager.Instance.EventUpdateFarmQuestState -= OnEventUpdateFarmQuestState;

		if (m_bInteractionNpc)
		{
			if (CsSecretLetterQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
			{
				CsSecretLetterQuestManager.Instance.InteractionArea(false);
			}
			else if (CsMysteryBoxQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
			{
				CsMysteryBoxQuestManager.Instance.InteractionArea(false);
			}
			else if (CsDimensionRaidQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
			{
				CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted -= OnEventDimensionRaidInteractionCompleted;
				CsDimensionRaidQuestManager.Instance.InteractionArea(false);
			}
			else if (CsGuildManager.Instance.IsGuildFarmQuestInteractionNpc(m_nNpcId))
			{
				CsGuildManager.Instance.FarmInteractionArea(false);
			}
		}
		else
		{
			if (m_bAreaEnter)
			{
				CsGameEventToUI.Instance.OnEventNpcInteractionArea(false, m_nNpcId);
			}
		}

		if (m_rtfHUD != null)
		{
			m_rtfHUD = null;
			CsGameEventToUI.Instance.OnEventDeleteNpcHUD((int)InstanceId);
		}

		m_csNpcInfo = null;
		m_csUndergroundMazeNpc = null;
		m_csGuildTerritoryNpc = null;
		m_csNationWarNpc = null;

		m_capsuleCollider = null;
		m_audioSource = null;
		m_acVoice = null;

		CsGameData.Instance.ListNpcObjectInfo.Remove(this);
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			m_bAreaEnter = true;

			if (m_bInteractionNpc)
			{
				if (CsSecretLetterQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsSecretLetterQuestManager.Instance.InteractionArea(true);
				}
				else if (CsMysteryBoxQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsMysteryBoxQuestManager.Instance.InteractionArea(true);
				}
				else if (CsDimensionRaidQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsDimensionRaidQuestManager.Instance.EventDimensionRaidInteractionCompleted += OnEventDimensionRaidInteractionCompleted;
					CsDimensionRaidQuestManager.Instance.InteractionArea(true);
				}
				else if (CsGuildManager.Instance.IsGuildFarmQuestInteractionNpc(m_nNpcId))
				{
					CsGuildManager.Instance.FarmInteractionArea(true);
				}
			}
			else
			{
				CsGameEventToUI.Instance.OnEventNpcInteractionArea(m_bAreaEnter, m_nNpcId);	
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			m_bAreaEnter = false;

			if (m_bInteractionNpc)
			{
				if (CsSecretLetterQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsSecretLetterQuestManager.Instance.InteractionArea(false);
				}
				else if (CsMysteryBoxQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsMysteryBoxQuestManager.Instance.InteractionArea(false);
				}
				else if (CsDimensionRaidQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					CsDimensionRaidQuestManager.Instance.InteractionArea(false);
				}
				else if (CsGuildManager.Instance.IsGuildFarmQuestInteractionNpc(m_nNpcId))
				{
					CsGuildManager.Instance.FarmInteractionArea(false);
				}
			}
			else
			{
				CsGameEventToUI.Instance.OnEventNpcInteractionArea(m_bAreaEnter, m_nNpcId);	
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	void HUDUpdatePos()
	{
		if (m_rtfHUD != null)
		{
			if (CsIngameData.Instance.InGameCamera == null) return;

			if (CsIngameData.Instance.InGameCamera.FirstEnter)
			{
				if (m_rtfHUD.gameObject.activeInHierarchy)
				{
					m_rtfHUD.gameObject.SetActive(false);
				}
				return;
			}

			float flCameraDistance = GetDistanceFormTarget(CsIngameData.Instance.InGameCamera.transform.position);

			if (flCameraDistance < 34.5f)
			{
				if (m_rtfHUD.gameObject.activeInHierarchy == false)
				{
					m_rtfHUD.gameObject.SetActive(true);
				}
			}
			else
			{
				if (m_rtfHUD.gameObject.activeInHierarchy)
				{
					m_rtfHUD.gameObject.SetActive(false);
				}
			}

			if (m_rtfHUD.gameObject.activeInHierarchy)
			{
				m_rtfHUD.position = new Vector3(transform.position.x, +transform.position.y + Height, transform.position.z);

				// 거리에 따른 스케일 변화.
				float flDistance = (flCameraDistance - 10) / 15;
				m_rtfHUD.localScale = new Vector3(0.8f + flDistance, 0.8f + flDistance, 0.8f + flDistance);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnAnimStatus en) { m_animator.SetInteger(s_nAnimatorHash_status, (int)en); }
	EnAnimStatus GetAnimStatus() { return (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status); }

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnState enNewState)
	{
		if (enNewState == EnState.Idle)
		{
			SetAnimStatus(EnAnimStatus.Idle01);
		}
		else if (enNewState == EnState.Talk)
		{
			m_animator.SetTrigger(s_nAnimatorHash_talk);
		}

		m_enState = enNewState;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetDialog()
	{
		if (m_csNpcInfo != null)	// 임시 사물Npc 예외 처리.
		{
			if (m_csNpcInfo.PrefabName == "7020" || m_csNpcInfo.PrefabName == "7021" || m_csNpcInfo.PrefabName == "8000" || m_csNpcInfo.PrefabName == "8001" || m_csNpcInfo.PrefabName == "8002") return;
		}

		LookAtPosition(CsGameData.Instance.MyHeroTransform.position);

        if (CsIngameData.Instance.EffectSound)
		{
			if (m_audioSource == null || m_audioSource.isPlaying) return;

			string strPrefabName = "";

			if (m_csNpcInfo != null)
			{
				strPrefabName = m_csNpcInfo.PrefabName;
			}
			else if (m_csGuildTerritoryNpc != null)
			{
				strPrefabName = m_csGuildTerritoryNpc.PrefabName;
			}
			else if (m_csNationWarNpc != null)
			{
				strPrefabName = m_csNationWarNpc.PrefabName;
			}
			else if (m_csUndergroundMazeNpc != null)
			{
				strPrefabName = m_csUndergroundMazeNpc.PrefabName;
			}

			AudioClip audioClip = CsIngameData.Instance.IngameManagement.GetNpcVoice(strPrefabName);

			if (audioClip == null)
			{
				if (m_acVoice == null) return;
				m_audioSource.PlayOneShot(m_acVoice);
			}
			else
			{
				m_audioSource.PlayOneShot(audioClip);
			}
        }
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateState()
	{
		if (m_bInteractionNpc)
		{
			m_capsuleCollider.isTrigger = false;
		}
		else
		{
			if (CheckNpcInfo())
			{
				if (CsDimensionRaidQuestManager.Instance.CheckInteractionNpc((int)InstanceId))
				{
					if (CsGameData.Instance.MyHeroInfo.InitEntranceLocationParam != CsGameData.Instance.MyHeroInfo.Nation.NationId)
					{
						m_bInteractionNpc = true;
						m_capsuleCollider.radius = m_csNpcInfo.InteractionMaxRange;
						m_capsuleCollider.isTrigger = true;
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventUpdateFarmQuestState()
	{
		if (CsGuildManager.Instance.IsGuildFarmQuestInteractionNpc(m_nNpcId))
		{			
			if (m_bInteractionNpc == false)
			{
				m_bInteractionNpc = true;

				if (m_bAreaEnter)
				{
					CsGuildManager.Instance.FarmInteractionArea(true);
				}
			}
		}
		else
		{
			if (m_bInteractionNpc)
			{
				m_bInteractionNpc = false;

				if (m_bAreaEnter)
				{
					CsGuildManager.Instance.FarmInteractionArea(false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventDimensionRaidInteractionCompleted()
	{
		if (m_bInteractionNpc)
		{
			Debug.Log("OnEventDimensionRaidInteractionCompleted       >>>>>>>>>>>            Fire Start");
			m_bInteractionNpc = false;
			transform.Find("FX_ObjFire").gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	bool CheckNpcInfo()
	{
		return (m_csNpcInfo != null || m_csUndergroundMazeNpc != null || m_csGuildTerritoryNpc != null || m_csNationWarNpc != null);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimTalk()
	{

	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimDefault()
	{

	}

	//---------------------------------------------------------------------------------------------------
	void SetComponent()
	{
		m_audioSource = transform.parent.GetComponent<AudioSource>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_navMeshAgent = GetComponent<NavMeshAgent>();
		m_animator = GetComponent<Animator>();
		m_timer.Init(0.2f);
	}

	//---------------------------------------------------------------------------------------------------
	void NavMeshSetting()
	{
		m_navMeshAgent.enabled = false;
		m_navMeshAgent.angularSpeed = 720f;
		m_navMeshAgent.acceleration = 100f;
		m_navMeshAgent.stoppingDistance = 0.1f;
		m_navMeshAgent.autoBraking = true;
		m_navMeshAgent.autoRepath = true;
		m_navMeshAgent.autoTraverseOffMeshLink = false;
		m_navMeshAgent.baseOffset = 0f;
		m_navMeshAgent.radius = 0.5f;
		m_navMeshAgent.height = 2;
		m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
		m_navMeshAgent.avoidancePriority = 40;
		m_navMeshAgent.speed = 3.5f;
		m_navMeshAgent.enabled = true;
	}

	//---------------------------------------------------------------------------------------------------
	long INpcObjectInfo.GetInstanceId()
	{
		return InstanceId;
	}

	//---------------------------------------------------------------------------------------------------
	CsNpcInfo INpcObjectInfo.GetNpcInfo()
	{
		return m_csNpcInfo;
	}

	//---------------------------------------------------------------------------------------------------
	Transform INpcObjectInfo.GetTransform()
	{
		return transform;
	}
}