using System.Collections;
using UnityEngine;


public class CsInteractionObject : MonoBehaviour, IInteractionObject
{
	static int s_nAnimatorHash_finish = Animator.StringToHash("finish");

	CsContinentObjectArrange m_csContinentObjectArrange;
	CsContinentObject m_csContinentObject;

	Transform m_trQuestEffect;
	Animator m_animator;
	CapsuleCollider m_capsuleCollider;

	CsSimpleTimer m_timer = new CsSimpleTimer();
	CsSimpleTimer m_timerTrigger = new CsSimpleTimer();

	[SerializeField]
	long m_lInstanceId;
	[SerializeField]
	int m_nObjectId;
	[SerializeField]
	int m_nArrangeNo;
	[SerializeField]
	bool m_bFinish = false;
	[SerializeField]
	EnInteractionQuestType m_enQuestType = EnInteractionQuestType.None;

	public long InstanceId { get { return m_lInstanceId; } }
	public int ObjectId { get { return m_nObjectId; } }
	public int ArrangeNo { get { return m_nArrangeNo; } }

	public CsContinentObject ContinentObject { get { return m_csContinentObject; } }
	public EnInteractionQuestType QuestType { get { return m_enQuestType; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(long lnstanceId, CsContinentObjectArrange csContinentObjectArrange, string strName)
	{
		m_animator = transform.GetComponent<Animator>();
		m_trQuestEffect = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/RFX_Triangle"), transform).transform;
		m_trQuestEffect.gameObject.SetActive(false);
		m_capsuleCollider = GetComponent<CapsuleCollider>();

		m_lInstanceId = lnstanceId;
		m_csContinentObjectArrange = csContinentObjectArrange;
		m_nObjectId = m_csContinentObjectArrange.ObjectId;
		m_csContinentObject = CsGameData.Instance.GetContinentObject(m_nObjectId);
		m_nArrangeNo = m_csContinentObjectArrange.ArrangeNo;

		transform.position = m_csContinentObjectArrange.Position;
		transform.eulerAngles = new Vector3(0f, m_csContinentObjectArrange.YRotation, 0f);
		transform.tag = "InteractionObject";
		gameObject.SetActive(true);
		name = m_nArrangeNo.ToString();
		m_capsuleCollider.radius = m_csContinentObject.InteractionMaxRange - 1f;
		m_capsuleCollider.isTrigger = true;
		m_capsuleCollider.enabled = false;

		m_trQuestEffect.transform.position = new Vector3(transform.position.x, transform.position.y + m_csContinentObject.Height, transform.position.z);

		StartCoroutine(NavSetting());
		CheckQuestObject();

		m_timer.Init(1f);
		m_timerTrigger.Init(0.1f);
	}

	//---------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		if (m_timer.CheckSetTimer())
		{
			CheckQuestObject();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (CsContinentObjectManager.Instance.IsInteractionStateViewButton && CsContinentObjectManager.Instance.InteractionInstanceId == m_lInstanceId)
		{
			CsContinentObjectManager.Instance.ContinentObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소.
		}


		m_csContinentObjectArrange = null;
		m_csContinentObject = null;
		m_animator = null;
		m_trQuestEffect = null;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator NavSetting()
	{
		UnityEngine.AI.NavMeshAgent navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		navMeshAgent.enabled = false;
		navMeshAgent.enabled = true;
		yield return null;
		Destroy(navMeshAgent);
	}

	//---------------------------------------------------------------------------------------------------
	void CheckQuestObject()
	{
		if (m_bFinish) return;
		if (m_trQuestEffect == null) return;

		if (CsMainQuestManager.Instance.IsInteractionQuest(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.Main;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsSubQuestManager.Instance.IsSubQuestObject(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.Sub;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsDailyQuestManager.Instance.IsDailyQuestObject(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.Daily;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsWeeklyQuestManager.Instance.IsInteractionQuest(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.Weekly;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsGuildManager.Instance.IsGuildMissionQuestObject(m_nObjectId)) //  길드미션 알리기의 경우 별도 Area가 있어서 충돌 확인 필요 없음.
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.GuildMission;
				m_capsuleCollider.enabled = true;
				m_capsuleCollider.isTrigger = false;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsGuildManager.Instance.IsGuildHuntingQuestObject(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.GuildHunting;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsBiographyManager.Instance.IsInteractionQuest(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.Biography;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsCreatureFarmQuestManager.Instance.IsInteractionQuest(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.CreatureFarm;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else if (CsJobChangeManager.Instance.IsInteractionQuest(m_nObjectId))
		{
			if (m_capsuleCollider.enabled == false)
			{
				m_enQuestType = EnInteractionQuestType.CreatureFarm;
				m_capsuleCollider.enabled = true;
				m_trQuestEffect.gameObject.SetActive(true);
			}
		}
		else // 비활성화.
		{
			if (m_capsuleCollider.enabled)
			{
				if (CsContinentObjectManager.Instance.IsInteractionStateViewButton)
				{
					CsContinentObjectManager.Instance.ContinentObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능취소.
				}
				m_enQuestType = EnInteractionQuestType.None;
				m_capsuleCollider.enabled = false;
				m_trQuestEffect.gameObject.SetActive(false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionFinish()
	{
		Debug.Log("CsInteractionObject.InteractionFinish()     InteractionCompletionAnimationEnabled = " + m_csContinentObject.InteractionCompletionAnimationEnabled);
		m_bFinish = true;
		m_capsuleCollider.enabled = false;
		m_trQuestEffect.gameObject.SetActive(false);
		CsContinentObjectManager.Instance.ContinentObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능취소.																											

		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (m_csContinentObject.InteractionCompletionAnimationEnabled)	// 상호작용 완료 모션 출력.
		{
			Transform tr = transform.Find("FX_ObjFire");

			if (tr != null)
			{
				tr.gameObject.SetActive(true);
			}
			else if (m_animator != null)
			{
				m_animator.SetBool(s_nAnimatorHash_finish, true);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionRegeneration()	// 길드 알리기 퀘스트 완료후 바로 다시 알리기 미션인경우 재갱신 처리.
	{
		Debug.Log("##############                 CsInteractionObject.InteractionRegeneration()                   #####################");
		m_bFinish = false;
		transform.GetComponent<CapsuleCollider>().enabled = true;

		if (m_animator != null)
		{
			m_animator.SetBool(s_nAnimatorHash_finish, false);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (CsContinentObjectManager.Instance.IsInteractionStateNone && m_capsuleCollider.isTrigger)
			{
				Debug.Log("CsInteractionObject        >>>>        OnTriggerEnter;");
				CsContinentObjectManager.Instance.ContinentObjectInteractionAble(true, m_nObjectId, m_lInstanceId); // 상호작용 가능 전달.
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (m_timerTrigger.CheckSetTimer())
			{
				if (CsContinentObjectManager.Instance.IsInteractionStateNone && m_capsuleCollider.isTrigger)
				{
					Debug.Log("CsInteractionObject        >>>>        OnTriggerStay;   m_lInstanceId = "+ m_lInstanceId);
					CsContinentObjectManager.Instance.ContinentObjectInteractionAble(true, m_nObjectId, m_lInstanceId); // 상호작용 가능 전달.
					CheckQuestObject();
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (CsContinentObjectManager.Instance.IsInteractionStateViewButton && m_capsuleCollider.isTrigger)
			{
				Debug.Log("CsInteractionObject        >>>>        OnTriggerExit;");
				CsContinentObjectManager.Instance.ContinentObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 취소.
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	int IInteractionObject.GetSpecificId()
	{
		return m_nObjectId;
	}

	//---------------------------------------------------------------------------------------------------
	long IInteractionObject.GetInstanceId()
	{
		return InstanceId;
	}
}

