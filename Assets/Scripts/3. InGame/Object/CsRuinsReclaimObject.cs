using System.Collections;
using UnityEngine;

public class CsRuinsReclaimObject : MonoBehaviour
{
	enum EnObjectType { None, Reward, Cancel }
	static int s_nAnimatorHash_finish = Animator.StringToHash("finish");	

	Animator m_animator;
	CapsuleCollider m_capsuleCollider;	
	Coroutine m_coroutine;

	CsRuinsReclaimObjectArrange m_csRuinsReclaimObjectArrange;
	CsRuinsReclaimStepWaveSkill m_csRuinsReclaimStepWaveSkill;
	CsSimpleTimer m_timerTrigger = new CsSimpleTimer();

	[SerializeField]
	int m_nObjectId;
	[SerializeField]
	long m_lInstanceId;
	float m_flInteractionMaxRange;

	EnObjectType m_enObjectType = EnObjectType.None;
	public long InstanceId { get { return m_lInstanceId; } }
	public int ObjectId { get { return m_nObjectId; } }
	public float InteractionMaxRange { get { return m_flInteractionMaxRange; } }
	public CsRuinsReclaimObjectArrange RuinsReclaimObjectArrange { get { return m_csRuinsReclaimObjectArrange; } }
	public CsRuinsReclaimStepWaveSkill RuinsReclaimStepWaveSkill { get { return m_csRuinsReclaimStepWaveSkill; } }

	//---------------------------------------------------------------------------------------------------
	public void RewardObjectInit(long lInatenceId, Vector3 vtPos, CsRuinsReclaimObjectArrange csRuinsReclaimObjectArrange)
	{
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();

		m_enObjectType = EnObjectType.Reward;
		m_csRuinsReclaimObjectArrange = csRuinsReclaimObjectArrange;
		m_lInstanceId = lInatenceId;
		m_nObjectId = 0;
		m_flInteractionMaxRange = m_csRuinsReclaimObjectArrange.ObjectInteractionMaxRange - 0.5f;

		transform.localScale = new Vector3(csRuinsReclaimObjectArrange.ObjectScale, csRuinsReclaimObjectArrange.ObjectScale, csRuinsReclaimObjectArrange.ObjectScale);
		transform.position = vtPos;
		m_capsuleCollider.radius = m_flInteractionMaxRange;
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_timerTrigger.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	public void CancelObjectInit(long lInatenceId, Vector3 vtPos, CsRuinsReclaimStepWaveSkill csRuinsReclaimStepWaveSkill)
	{
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();

		m_enObjectType = EnObjectType.Cancel;
		m_csRuinsReclaimStepWaveSkill = csRuinsReclaimStepWaveSkill;
		m_lInstanceId = lInatenceId;
		m_nObjectId = 0;
		m_flInteractionMaxRange = m_csRuinsReclaimStepWaveSkill.ObjectInteractionMaxRange - 0.5f;

		transform.localScale = new Vector3(m_csRuinsReclaimStepWaveSkill.ObjectScale, m_csRuinsReclaimStepWaveSkill.ObjectScale, m_csRuinsReclaimStepWaveSkill.ObjectScale);
		transform.position = vtPos;
		m_capsuleCollider.radius = m_flInteractionMaxRange;
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		m_timerTrigger.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (CsDungeonManager.Instance.IsStateViewButton && CsDungeonManager.Instance.InteractionInstanceId == m_lInstanceId)
		{
			CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소.
		}

		m_csRuinsReclaimObjectArrange = null;
		m_csRuinsReclaimStepWaveSkill = null;

		m_animator = null;
		m_capsuleCollider = null;
		m_coroutine = null;
		m_timerTrigger = null;
	}


	//---------------------------------------------------------------------------------------------------
	public void InteractionFinish()
	{
		Debug.Log("CsRuinsReclaimObject.InteractionFinish()");
		m_capsuleCollider.enabled = false;

		CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소																								

		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (m_animator != null)
		{
			m_animator.SetTrigger(s_nAnimatorHash_finish);
		}

		StartCoroutine(DelayRemove());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayRemove()
	{
		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (CsDungeonManager.Instance.IsStateNone)
			{
				CsDungeonManager.Instance.DungeonObjectInteractionAble(true, m_nObjectId, m_lInstanceId); // 상호작용 가능 전달.
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
				if (CsDungeonManager.Instance.IsStateNone)
				{
					CsDungeonManager.Instance.DungeonObjectInteractionAble(true, m_nObjectId, m_lInstanceId); // 상호작용 가능 전달.
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			if (CsDungeonManager.Instance.IsStateViewButton)
			{
				CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소
			}
		}
	}
}
