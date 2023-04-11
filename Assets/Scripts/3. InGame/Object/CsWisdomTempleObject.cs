using ClientCommon;
using System.Collections;
using UnityEngine;

public class CsWisdomTempleObject : MonoBehaviour
{
	static int s_nAnimatorHash_finish = Animator.StringToHash("finish");
	static int s_nAnimatorHash_start = Animator.StringToHash("start");

	Animator m_animator;
	CapsuleCollider m_capsuleCollider;
	CsWisdomTempleColorMatchingObject m_csWisdomTempleColorMatchingObject;
	Transform m_trCrystal_Light;
	Coroutine m_coroutine;

	CsSimpleTimer m_timerTrigger = new CsSimpleTimer();

	int m_nRow; // 0,1,2
	int m_nCol; // 0,1,2

	[SerializeField]
	bool m_bRewardObject = false;
	[SerializeField]
	long m_lInstanceId;
	[SerializeField]
	int m_nObjectId;
	[SerializeField]
	float m_flInteractionDuration;

	public long InstanceId { get { return m_lInstanceId; } }
	public int ObjectId { get { return m_nObjectId; } }
	public float InteractionDuration { get { return m_flInteractionDuration; } }
	public CsWisdomTempleColorMatchingObject WisdomTempleColorMatchingObject { get { return m_csWisdomTempleColorMatchingObject; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(long lInatenceId, int nObjectId, int nRow, int nCol, Vector3 vtPos)
	{
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_trCrystal_Light = transform.Find("Crystal_Light");
		m_csWisdomTempleColorMatchingObject = CsDungeonManager.Instance.WisdomTemple.GetWisdomTempleColorMatchingObject(nObjectId);
		m_lInstanceId = lInatenceId;
		m_flInteractionDuration = m_csWisdomTempleColorMatchingObject.InteractionDuration;
		m_nObjectId = nObjectId;
		m_nRow = nRow;
		m_nCol = nCol;


		transform.position = vtPos;
		transform.name = m_nRow.ToString() + m_nCol.ToString();
		m_capsuleCollider.radius = m_csWisdomTempleColorMatchingObject.InteractionMaxRange - 1;
		m_capsuleCollider.enabled = true;
		m_capsuleCollider.isTrigger = true;
		SetColorMatchingObjectColor();
		m_timerTrigger.Init(0.5f);
	}

	//---------------------------------------------------------------------------------------------------
	public void Init(long lInatenceId,float flMaxRange, float flScale, Vector3 vtPos)
	{
		m_animator = transform.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
		m_bRewardObject = true;

		m_lInstanceId = lInatenceId;
		m_flInteractionDuration = CsDungeonManager.Instance.WisdomTemple.PuzzleRewardObjectInteractionDuration;
		transform.position = vtPos;
		transform.name = lInatenceId.ToString();
		transform.localScale = new Vector3(flScale, flScale, flScale);

		m_capsuleCollider.radius = flMaxRange - 1;
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

		m_animator = null;
		m_capsuleCollider = null;
		m_coroutine = null;
		m_timerTrigger = null;
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsIdle() { return m_animator != null && m_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"); }

	//---------------------------------------------------------------------------------------------------
	public void ChangeObject(long lInatenceId, int nObjectId, bool bMatching, bool bMonsterKill)
	{
		Debug.Log("ChangeObject   lInatenceId " + lInatenceId + " // nObjectId = " + nObjectId + " // bMatching = " + bMatching);
		m_capsuleCollider.enabled = false;
	
		if (CsDungeonManager.Instance.IsStateViewButton && CsDungeonManager.Instance.InteractionInstanceId == m_lInstanceId)
		{
			CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소.
		}

		m_lInstanceId = lInatenceId;
		m_nObjectId = nObjectId;

		if (m_coroutine != null)
		{
			StopCoroutine(m_coroutine);
		}

		if (bMonsterKill)
		{
			m_coroutine = StartCoroutine(ColorMatchingMonsterKill());
		}
		else
		{
			m_coroutine = StartCoroutine(DelayChangeObject(bMatching));
		}		
	}

	//---------------------------------------------------------------------------------------------------
	public void ColorMatchingFinish()
	{
		if (m_coroutine != null)
		{
			StopCoroutine(m_coroutine);
			m_coroutine = null;
		}

		m_capsuleCollider.enabled = false;
		CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소.
		if (m_animator != null)
		{
			m_animator.SetTrigger(s_nAnimatorHash_finish);
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator ColorMatchingMonsterKill()
	{
		m_capsuleCollider.enabled = false;
		CsDungeonManager.Instance.DungeonObjectInteractionAble(false, m_nObjectId, m_lInstanceId); // 상호작용 가능 취소.

		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		SetEmissionColor(new Color(2.017f, 0.0f, 0.0f, 1.0f));

		yield return new WaitForSeconds(1f);
		m_capsuleCollider.enabled = true;
		m_coroutine = null;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator DelayChangeObject(bool bMatching)
	{
		if (CsIngameData.Instance.TargetTransform == this.transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (bMatching)
		{
			if (m_animator != null)
			{
				m_animator.SetTrigger(s_nAnimatorHash_finish);
			}

			yield return new WaitForSeconds(2f);
		}

		SetColorMatchingObjectColor();
		yield return null;

		if (bMatching)
		{
			if (m_animator != null)
			{
				m_animator.SetTrigger(s_nAnimatorHash_start);
			}
			yield return new WaitForSeconds(2f);
		}
		else
		{
			yield return new WaitForSeconds(0.5f);
		}

		m_capsuleCollider.enabled = true;
		m_coroutine = null;
	}

	//---------------------------------------------------------------------------------------------------
	void SetColorMatchingObjectColor()
	{
		switch (m_nObjectId)
		{
			case 1:  // 파랑 
				//SetColor(new Color(0.0f, 1.236f, 2.018f, 1.0f));
				SetAlbedoColor(new Color(0.34f, 0.0f, 0.058f, 1.0f));
				SetEmissionColor(new Color(0.0f, 1.836f, 2.89f, 1.0f));
				break;
			case 2: // 초록
				//SetColor(new Color(0.031f, 1.056f, 0.316f, 1.0f));
				SetAlbedoColor(new Color(0.032f, 0.005f, 0.005f, 1.0f));
				SetEmissionColor(new Color(0.0f, 2.095f, 0.78f, 1.0f));
				break;
			case 3: // 노랑
				//SetColor(new Color(1.838f, 0.855f, 0.221f, 1.0f));
				SetAlbedoColor(new Color(0.528f, 0.0f, 0.203f, 1.0f));
				SetEmissionColor(new Color(2.976f, 1.963f, 0.0f, 1.0f));
				break;
			case 4: // 보라
				//SetColor(new Color(1.225f, 0.0f, 2.017f, 1));
				SetAlbedoColor(new Color(0.048f, 0.217f, 0.0f, 1.0f));
				SetEmissionColor(new Color(1.17f, 0.0f, 2.48f, 1));
				break;
			case 5: // 빨강				
				//SetColor(new Color(2.017f, 0.0f, 0.0f, 1.0f));
				SetAlbedoColor(new Color(0.0f, 0.117f, 0.301f, 1.0f));
				SetEmissionColor(new Color(1.816f, 0.05f, 0.0f, 1.0f));
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetAlbedoColor(Color color)
	{
		Renderer renderer = transform.Find("dungeon_offence_crystal_01_main").GetComponent<Renderer>();
		Renderer renderer0 = transform.Find("dungeon_offence_crystal_idle_1").GetComponent<Renderer>();

		if (renderer.material.HasProperty("_Color"))
		{
			renderer.material.SetColor("_Color", color);
			renderer0.material.SetColor("_Color", color);
		}

		for (int i = 1; i < 17; i++)
		{
			string strName = "dungeon_offence_crystal_01_main_Part_" + i.ToString();
			Renderer renderer1 = transform.Find(strName).GetComponent<Renderer>();

			if (renderer1.material.HasProperty("_Color"))
			{
				renderer1.material.SetColor("_Color", color);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetEmissionColor(Color color)
	{
		Renderer renderer = transform.Find("dungeon_offence_crystal_01_main").GetComponent<Renderer>();
		Renderer renderer0 = transform.Find("dungeon_offence_crystal_idle_1").GetComponent<Renderer>();
		
		if (renderer.material.HasProperty("_EmissionColor"))
		{
			renderer.material.SetColor("_EmissionColor", color);
			renderer0.material.SetColor("_EmissionColor", color);
		}

		for (int i = 1; i < 17; i++)
		{
			string strName = "dungeon_offence_crystal_01_main_Part_" + i.ToString();
			Renderer renderer1 = transform.Find(strName).GetComponent<Renderer>();

			if (renderer1 != null)
			{
				if (renderer1.material.HasProperty("_EmissionColor"))
				{
					renderer1.material.SetColor("_EmissionColor", color);
				}
			}
		}

		m_trCrystal_Light.GetComponent<Light>().color = color;
	}

	//---------------------------------------------------------------------------------------------------
	public void InteractionFinish()
	{
		Debug.Log("CsWisdomTempleObject.InteractionFinish()");
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
