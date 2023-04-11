using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsWarMemoryObject : MonoBehaviour
{
	static int s_nAnimatorHash_finish = Animator.StringToHash("finish");

	Animator m_animator;
	CapsuleCollider m_capsuleCollider;

	CsSimpleTimer m_timerTrigger = new CsSimpleTimer();
	CsWarMemoryTransformationObject m_csWarMemoryTransformationObject;

	[SerializeField]
	int m_nObjectId;
	[SerializeField]
	long m_lInstanceId;
	float m_flInteractionMaxRange;

	public long InstanceId { get { return m_lInstanceId; } }
	public int ObjectId { get { return m_nObjectId; } }
	public float InteractionMaxRange { get { return m_flInteractionMaxRange; } }
	public CsWarMemoryTransformationObject WarMemoryTransformationObject { get { return m_csWarMemoryTransformationObject; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(long lInatenceId, Vector3 vtPos, CsWarMemoryTransformationObject csWarMemoryTransformationObject)
	{
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_csWarMemoryTransformationObject = csWarMemoryTransformationObject;
		m_lInstanceId = lInatenceId;
		m_nObjectId = csWarMemoryTransformationObject.TransformationObjectId;
		m_flInteractionMaxRange = m_csWarMemoryTransformationObject.ObjectInteractionMaxRange - 0.5f;

		transform.localScale = new Vector3(m_csWarMemoryTransformationObject.ObjectScale, m_csWarMemoryTransformationObject.ObjectScale, m_csWarMemoryTransformationObject.ObjectScale);
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

		m_csWarMemoryTransformationObject = null;
		m_animator = null;
		m_capsuleCollider = null;
		m_timerTrigger = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionFinish()
	{
		Debug.Log("CsMemoryWarObject.InteractionFinish()");
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

