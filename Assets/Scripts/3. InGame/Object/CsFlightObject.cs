using UnityEngine;
using UnityEngine.AI;

public enum EnFlightState { Idle = 0, Walk, Run }

public class CsFlightObject : MonoBehaviour
{
	enum EnFlightAnimStatus { Idle = 0, Walk, Run }

	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	const float c_flMaxBaseOffset = 20f;
	const float c_flMinBaseOffset = 4f;

	Animator m_animator = null;
	NavMeshAgent m_navMeshAgent;

	bool m_bHasPath = false;
	float flOrgHalfDistance;
	float m_flStartBaseOffset;

	EnFlightState m_enFlightState = EnFlightState.Idle;

	//---------------------------------------------------------------------------------------------------
	public void Init(Transform trParent, float Height)
	{
		Debug.Log("CsFlightObject.Init()");
		m_animator = transform.GetComponent<Animator>();

		gameObject.tag = transform.tag;
		gameObject.layer = trParent.gameObject.layer;
		transform.Find("Vehicle_eagle").gameObject.layer = trParent.gameObject.layer;

		transform.position = new Vector3(trParent.position.x, trParent.position.y + Height, trParent.position.z);
		transform.eulerAngles = trParent.eulerAngles;

		transform.SetParent(trParent);
		m_navMeshAgent = trParent.GetComponent<CsHero>().MyHeroNavMeshAgent;
		m_navMeshAgent.baseOffset = 4f;
	}
	
	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_navMeshAgent == null) return;

		if (m_enFlightState == EnFlightState.Run)
		{
			if (m_navMeshAgent.hasPath)
			{
				if (m_bHasPath == false)
				{
					m_bHasPath = true;
					flOrgHalfDistance = (Vector3.Distance(transform.position, m_navMeshAgent.destination) - m_navMeshAgent.baseOffset) * 0.5f;
					m_flStartBaseOffset = m_navMeshAgent.baseOffset;
					Debug.Log("2. flOrgHalfDistance = "+ flOrgHalfDistance + " // m_flStartBaseOffset = " + m_flStartBaseOffset);
				}
				else
				{
					if (flOrgHalfDistance > 20)	// 최소 거리. 20m
					{
						float flDistance = Vector3.Distance(transform.position, m_navMeshAgent.destination) - m_navMeshAgent.baseOffset;
						float flBassOffset;

						if (flDistance < flOrgHalfDistance)
						{
							if (m_navMeshAgent.baseOffset > c_flMinBaseOffset)
							{
								float flDistanceVlaue = Mathf.InverseLerp(flOrgHalfDistance, 4, flDistance);
								flBassOffset = Mathf.Lerp(c_flMaxBaseOffset, c_flMinBaseOffset, flDistanceVlaue);
							}
							else
							{
								flBassOffset = c_flMinBaseOffset;
							}
						}
						else
						{
							if (m_navMeshAgent.baseOffset < c_flMaxBaseOffset)
							{
								float flDistanceVlaue = Mathf.InverseLerp(flOrgHalfDistance * 2, flOrgHalfDistance, flDistance);
								flBassOffset = Mathf.Lerp(m_flStartBaseOffset, c_flMaxBaseOffset, flDistanceVlaue);
							}
							else
							{
								flBassOffset = 20;
							}
						}

						m_navMeshAgent.baseOffset = flBassOffset;
					}
				}
			}
			else
			{
				m_bHasPath = false;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnFlightState enNewFlightState)
	{
		//Debug.Log("CsFlightObject.ChangeState     EnFlightState = " + enNewFlightState);
		if (enNewFlightState == EnFlightState.Idle)
		{
			SetAnimStatus(EnFlightAnimStatus.Idle);
		}
		else if (enNewFlightState == EnFlightState.Walk)
		{
			SetAnimStatus(EnFlightAnimStatus.Run);
		}
		else if (enNewFlightState == EnFlightState.Run)
		{
			if (m_navMeshAgent.hasPath)
			{
				m_bHasPath = true;
				flOrgHalfDistance = (Vector3.Distance(transform.position, m_navMeshAgent.destination) - m_navMeshAgent.baseOffset) * 0.5f;
				m_flStartBaseOffset = m_navMeshAgent.baseOffset;
				Debug.Log("1. flOrgHalfDistance = " + flOrgHalfDistance + " // m_flStartBaseOffset = " + m_flStartBaseOffset);
			}
			else
			{
				m_bHasPath = false;
			}
			SetAnimStatus(EnFlightAnimStatus.Run);
		}

		m_enFlightState = enNewFlightState;
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnFlightAnimStatus enAnimStatus)
	{
		if (enAnimStatus == (EnFlightAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status)) return;
		m_animator.SetInteger(s_nAnimatorHash_status, (int)enAnimStatus);
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimRunEffect(int nNo)
	{
		if (nNo == 1)
		{
			if (CsIngameData.Instance.Directing)  // 연출시에만 포스트사용
			{
				CsIngameData.Instance.InGameCamera.PostProcessingStart(false, true, 0.5f);
			}
		}
		else if (nNo == 2)
		{
			if (CsIngameData.Instance.Directing)  // 연출시에만 포스트사용
			{
				CsIngameData.Instance.InGameCamera.PostProcessingStart(true, false, 0.2f);
			}
			CsEffectPoolManager.Instance.PlayEffect(CsEffectPoolManager.EnEffectOwner.User, transform, transform.position, "Flight_Accelerate", 0.5f);
		}
	}
}

