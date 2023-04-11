using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class CsCreatureObject : CsMoveUnit
{
	public enum EnState { Idle, Attack, Chase, Walk }
	public enum EnAnimStatus { Idle = 0, Walk, Run , Skill01, Skill02 }
	
	static int s_nAnimatorHash_status = Animator.StringToHash("status");
	const float c_flStopDistance = 0.3f;

	Transform m_trOwner = null;
	NavMeshAgent m_OwerNavMeshAgent;
	Vector3 m_vtMovePos;
	Vector3 m_vtPrevPos;

	int m_nSamePosMoveCount;

	[SerializeField]
	EnState m_enState = EnState.Idle;

	//---------------------------------------------------------------------------------------------------
	public void Init(CsCreature csCreature, Transform trOwner)
	{
		transform.name = csCreature.CreatureCharacter.Name;
		m_trOwner = trOwner;

		transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
		transform.position = trOwner.transform.position;
		transform.rotation = (trOwner.rotation * Quaternion.Euler(0, 90, 0));
		transform.position = transform.position + (transform.forward * 1.5f);
		transform.LookAt(trOwner);

		gameObject.layer = m_trOwner.gameObject.layer;
		transform.tag = m_trOwner.tag;

		m_animator = transform.GetComponent<Animator>();
		m_navMeshAgent = transform.GetComponent<NavMeshAgent>();
		m_OwerNavMeshAgent = m_trOwner.GetComponent<NavMeshAgent>();
		NavMeshSetting();
		m_timer.Init(10f);

		if (m_animator != null)
		{
			CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

			if (acs.Length > 0)
			{
				for (int i = 0; i < acs.Length; i++)
				{
					if ((EnAnimStatus)acs[i].Key == EnAnimStatus.Idle)
					{
						acs[i].EventStateEnter += OnEventAnimStartIdle;
					}
					else
					{
						acs[i].EventStateEnter += OnEventAnimStartAttack;
					}
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Update()
	{
		if (m_enState == EnState.Idle)
		{
			IdleState();
		}
		else if (m_enState == EnState.Walk)
		{
			WalkState();
		}
		else if (m_enState == EnState.Chase)
		{
			ChaseState();
		}

		CheckHero();
		m_vtPrevPos = transform.position;
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_animator != null)
		{
			CsAnimStateBehaviour[] acs = m_animator.GetBehaviours<CsAnimStateBehaviour>();

			if (acs.Length > 0)
			{
				for (int i = 0; i < acs.Length; i++)
				{
					if ((EnAnimStatus)acs[i].Key == EnAnimStatus.Idle)
					{
						acs[i].EventStateEnter -= OnEventAnimStartIdle;
					}
					else
					{
						acs[i].EventStateEnter -= OnEventAnimStartAttack;
					}
				}
			}
		}

		m_trOwner = null;
		m_OwerNavMeshAgent = null;
		base.OnDestroy();
	}

	//---------------------------------------------------------------------------------------------------
	void CheckHero()
	{
		if (m_bAttack) return;
		if (m_enState == EnState.Chase) return;

		if (IsTargetInDistance(m_trOwner.position, 5f) == false)
		{
			SetDestination(m_trOwner.position, EnState.Chase);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void IdleState()
	{
		if (m_timer.CheckSetTimer())
		{
			if (Random.Range(0, 3) > 1 ? true : false)
			{
				ChangeState(EnState.Attack);
			}
			else
			{
				Vector3 vtMovePos = RandomMovePos(m_trOwner.position, 2f);
				SetDestination(vtMovePos, EnState.Walk);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void WalkState()
	{
		if (UpdateMove())
		{
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChaseState()
	{
		if (m_vtMovePos != m_trOwner.position && IsTargetInDistance(m_trOwner.position, 1f) == false)
		{
			SetDestination(m_trOwner.position, EnState.Chase);
			return;
		}

		if (UpdateMove())
		{
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ChangeState(EnState enNewState)
	{
		if (enNewState == EnState.Idle)
		{
			if (m_navMeshAgent.hasPath)
			{
				m_navMeshAgent.ResetPath();
			}

			m_timer.ResetTimer();
			m_vtMovePos = Vector3.zero;

			SetAnimStatus(EnAnimStatus.Idle);
		}
		else if (enNewState == EnState.Walk)
		{
			SetAnimStatus(EnAnimStatus.Walk);
		}
		else if (enNewState == EnState.Chase)
		{
			SetAnimStatus(EnAnimStatus.Run);
		}
		else if (enNewState == EnState.Attack)
		{
			if (Random.Range(0, 3) > 1 ? true : false)
			{
				SetAnimStatus(EnAnimStatus.Skill01);
			}
			else
			{
				SetAnimStatus(EnAnimStatus.Skill02);
			}
		}

		m_enState = enNewState;
		SetMoveSpeed();
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnAnimStatus en) { m_animator.SetInteger(s_nAnimatorHash_status, (int)en); }	
	EnAnimStatus GetAnimStatus() { return (EnAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status); }

	//---------------------------------------------------------------------------------------------------
	bool UpdateMove()
	{
		if (m_vtPrevPos == transform.position)
		{
			m_nSamePosMoveCount++;
			if (m_nSamePosMoveCount > 50)
			{
				m_vtMovePos = transform.position;
				m_nSamePosMoveCount = 0;
				return true;
			}
		}
		else
		{
			if (m_enState == EnState.Walk)
			{
				if (IsTargetInDistance(m_vtMovePos, c_flStopDistance))
				{
					return true;
				}
			}
			else
			{
				if (IsTargetInDistance(m_vtMovePos, 2f))
				{
					return true;
				}
			}

			SetMoveSpeed();
		}
		return false;
	}

	//---------------------------------------------------------------------------------------------------
	void SetDestination(Vector3 vtPos, EnState enNewState)
	{
		if (m_vtMovePos != vtPos || !m_navMeshAgent.hasPath) // 같은 값 중복 입력 방지.
		{
			m_nSamePosMoveCount = 0;
			m_vtMovePos = vtPos;

			SetMoveSpeed();
			m_navMeshAgent.SetDestination(vtPos);

			if (m_enState != enNewState)
			{
				ChangeState(enNewState);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void SetMoveSpeed()
	{
		if (m_enState == EnState.Walk)
		{
			m_navMeshAgent.speed = 1.5f;
		}
		else if (m_enState == EnState.Chase)
		{
			m_navMeshAgent.speed = m_OwerNavMeshAgent.speed + (m_OwerNavMeshAgent.speed * 0.1f);
		}
	}

	#region Anim
	bool m_bAttack = false;

	//---------------------------------------------------------------------------------------------------
	protected void OnEventAnimStartIdle(AnimatorStateInfo asi, int nKey)
	{
		if (m_bAttack)
		{
			m_bAttack = false;
			ChangeState(EnState.Idle);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected void OnEventAnimStartAttack(AnimatorStateInfo asi, int nKey)
	{
		m_bAttack = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimApplyDamage(int nHitId) { }
	void OnAnimSkillEffect() { }
	void OnAnimAttackSound() { }
	void OnAnimBulletEffect() { }
	void OnAnimAttackSound(int nIndex) { }

	void OnAnimRunRightSound() { }
	void OnAnimRunLeftSound() { }

	void OnAnimDeadSound() { }
	void OnAnimDeadEnd() { }

	#endregion Anim

	//---------------------------------------------------------------------------------------------------
	public void NavMeshSetting()
	{
		m_navMeshAgent.enabled = false;
		m_navMeshAgent.height = 2f;
		m_navMeshAgent.angularSpeed = 720f;
		m_navMeshAgent.acceleration = 100f;
		m_navMeshAgent.stoppingDistance = 0.1f;
		m_navMeshAgent.autoBraking = true;
		m_navMeshAgent.autoRepath = true;
		m_navMeshAgent.avoidancePriority = 50;
		m_navMeshAgent.autoTraverseOffMeshLink = false;
		m_navMeshAgent.autoRepath = false;
		m_navMeshAgent.speed = m_OwerNavMeshAgent.speed;
		m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		m_navMeshAgent.enabled = true;
	}

}
