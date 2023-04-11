using ClientCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CsMoveUnit : MonoBehaviour
{
	enum EnAbnormalState
	{
		Stun = 1,							// 기절
		Blockade = 2,                       // 속박
		Bleed = 3,							// 출혈						// 이팩트 없음
		Burn = 4,                           // 화상						// 이팩트 없음
		DecreaseAttack = 5,                 // 공격력 저하				// 이팩트 없음
		DecreaseDefense = 6,                // 방어력 저하				// 이팩트 없음
		DecreaseElementDefense = 7,         // 원소 방어력 저하			// 이팩트 없음
		IncreaseDamage = 8,                 // 받는 피해 증가			// 이팩트 없음
		Invincible = 9,                     // 무적

		// 계급스킬
		Shield = 10,                        // 피해흡수쉴드
		WindWalk = 11,						// 풍행
		CurseDestruction = 12,				// 저주파괴
		CommonPlatelet = 13,				// 분노폭발

		StateImmune = 101,                  // 상태이상면역										// 이팩트 없음
		AbsorptionShield = 102,             // 피해흡수쉴드										// 이팩트 없음
		IncreasePDam = 103,                 // 물리공격력증가									// 이팩트 없음
		IncreasePDamAdd = 104,              // 물리공격력증가 + HIT 지송시간 추가(최대2초)		// 이팩트 없음
		IncreasePDamPenetrate = 105,        // 물리공격력증가 + 관통							// 이팩트 없음
		IncreasePDamPenetrateAdd = 106,     // 물리공격력증가 + 관통 + 불사						// 이팩트 없음
		IncreasePDamCritical = 107,         // 물리공격력증가 + 치명							// 이팩트 없음
		IncreaseMDamDef = 108,              // 마법 공격력 + 방어력 증가						// 이팩트 없음
		IncreaseMDamDefShield = 109,        // 마법 공격력 + 방어력 증가 + 피해 습수 쉴드		// 이팩트 없음
		IncreaseMDamDefElementDam = 110,    // 마법 공격력 + 방어력 증가 + 피해 습수 쉴드		// 이팩트 없음
		Suppression = 111					// 제압
	}

	protected Animator m_animator;
	protected NavMeshAgent m_navMeshAgent;
	protected GameObject m_goShadow = null;
	protected long m_lInstanceId;
	protected int m_nLevel;
	protected int m_nVipLevel;
	protected int m_nMaxHp;
	protected int m_nHp;
	protected string m_strName;
	protected bool m_bApplicationQuit = false;

	[SerializeField]
	protected float m_flRadius = 0.5f;
	[SerializeField]
	protected float m_flHeght = 1.0f;
	[SerializeField]
	protected bool m_bDead = false;
	[SerializeField]
	protected float m_flWindWalkMoveSpeed = 0;	
	[SerializeField]
	protected int m_nDamageAbsorbShieldRemain = 0;

	[SerializeField]
	protected bool m_bStun = false;         // 기절.
	[SerializeField]
	protected bool m_bBlockade = false;     // 속박.
	[SerializeField]
	protected bool m_bSuppression = false;  // 제압
	protected float m_flStunTimer = 0f;
	protected float m_flBlockadeTimer = 0f;
	protected float m_flSuppressionTimer = 0f;

	protected CsSimpleTimer m_timer = new CsSimpleTimer();
	protected Dictionary<long, GameObject> m_dicAbnormalEffect = new Dictionary<long, GameObject>();
	//---------------------------------------------------------------------------------------------------
	public long InstanceId { get { return m_lInstanceId; } set { m_lInstanceId = value; } }
	public int VipLevel { get { return m_nVipLevel; } set { m_nVipLevel = value; } }
	public float Radius { get { return m_flRadius; } set { m_flRadius = value; } }

	public virtual float Height { get { return m_flHeght; } set { m_flHeght = value; } }
	public virtual string Name { get { return m_strName; } set { m_strName = value; } }
	public virtual int Level { get { return m_nLevel; } set { m_nLevel = value; OnStateChanged(); } }
	public virtual int MaxHp { get { return m_nMaxHp; } set { m_nMaxHp = value; OnStateChanged(); } }
	public virtual int Hp { get { return m_nHp; } set { m_nHp = value; OnStateChanged(); } }
	public virtual bool Dead { get { return m_bDead; } set { m_bDead = value; } }

	//----------------------------------------------------------------------------------------------------
	void OnApplicationQuit()
	{
		m_bApplicationQuit = true;
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		m_animator = null;
		m_navMeshAgent = null;
		m_goShadow = null;

		m_timer = null;
		m_dicAbnormalEffect.Clear();
	}

	protected virtual void DamageByAbnormal(bool bSkillCancel) { }
	public virtual void OnStateChanged() { }
	public virtual void AttackByHit(PDHitResult HitResult, float flHitTime, PDHitResult AddHitResult, bool bMyHero, bool bKnockback) { }

	//---------------------------------------------------------------------------------------------------
	protected Coroutine Invoke(Delegate dlgtFunc, float fl)
	{
		return StartCoroutine(CoroutinDelayCall(dlgtFunc, fl));
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CoroutinDelayCall(Delegate dlgtFunc, float fl)
	{
		yield return new WaitForSeconds(fl);
		dlgtFunc();
	}

	//---------------------------------------------------------------------------------------------------
	protected void CreateShadow(bool bMyHero = false)
	{
		if (m_goShadow != null) return;
		if (bMyHero)
		{
			if (CsIngameData.Instance.IngameManagement.IsContinent() || CsIngameData.Instance.Scene.name == "Area1_Marduka")
			{
				m_goShadow = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Shadow"), transform) as GameObject;

				string strName = CsGameData.Instance.MyHeroInfo.LocationId.ToString();

				if (CsIngameData.Instance.Scene.name == "Area1_Marduka")
				{
					strName = CsGameData.Instance.ContinentList.Find(a => a.SceneName == CsIngameData.Instance.Scene.name).LocationId.ToString();
				}

				GameObject goArea = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Area/" + strName), transform) as GameObject; // 효과 생성.
				goArea.transform.position = transform.position;
				goArea.layer = transform.gameObject.layer;
			}
			else
			{
				m_goShadow = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Shadow_dPlayer"), transform) as GameObject;
			}
		}
		else
		{
			m_goShadow = Instantiate(CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Shadow"), transform) as GameObject;
			m_goShadow.layer = transform.gameObject.layer;
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected Vector3 RandomMovePos(Vector3 vtCenterPos, float flDistance)
	{
		Vector3 randomPos = UnityEngine.Random.insideUnitSphere * flDistance;
		randomPos.y = 0f;

		NavMeshHit navMeshHit;
		NavMesh.SamplePosition(vtCenterPos + randomPos, out navMeshHit, flDistance, NavMesh.AllAreas);
		return navMeshHit.position;
	}

	//---------------------------------------------------------------------------------------------------
	protected Vector3 ForcedMoveBySkill(Transform trTarget, float flDistance, bool bPush) // 스킬에 의한 강제 이동(밀림, 당김)
	{
		Vector3 vtDir;
		transform.LookAt(trTarget);

		if (bPush)
		{
			vtDir = transform.forward;
			vtDir.y = 0;
			vtDir.Normalize();
			vtDir = transform.position - vtDir * flDistance;
		}
		else
		{
			vtDir = trTarget.forward;
			vtDir.y = 0;
			vtDir.Normalize();
			vtDir = trTarget.position + vtDir * flDistance;
		}

		NavMeshHit navMeshHit;
		NavMesh.Raycast(transform.position, vtDir, out navMeshHit, NavMesh.AllAreas);
		return navMeshHit.position;
	}

	//---------------------------------------------------------------------------------------------------
	protected void SetDamageText(PDHitResult HitResult, bool bMyHero = false)
	{
		EnDamageTextType enDamageTextType = EnDamageTextType.None;
		if (HitResult.isBlocked)
		{
			enDamageTextType = EnDamageTextType.Blocked;
		}
		else if (HitResult.isCritical)
		{
			enDamageTextType = EnDamageTextType.Critical;
		}
		else if (HitResult.isPenetration)
		{
			enDamageTextType = EnDamageTextType.Penetration;
		}
		else if (HitResult.isImmortal)
		{
			enDamageTextType = EnDamageTextType.Immortal;
		}

		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			float flHeight = Height > 3 ? 3 : Height;
			Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + flHeight, transform.position.z);
			CsGameEventToUI.Instance.OnEventCreatDamageText(enDamageTextType, HitResult.damage, Camera.main.WorldToScreenPoint(vtPos), bMyHero);  // 데미지 Text 처리.
		}
	}

	//---------------------------------------------------------------------------------------------------
	public float GetDistanceFormTarget(Vector3 vtTarget)
	{
		return Vector3.Distance(transform.position, vtTarget);
	}

	//---------------------------------------------------------------------------------------------------
	public bool IsTargetInDistance(Vector3 vtTarget, float flDistance)
	{
		return Vector3.Distance(transform.position, vtTarget) <= flDistance;
	}

	//---------------------------------------------------------------------------------------------------
	public void LookAtPosition(Vector3 vtPos)
	{
		Vector3 dir = vtPos - this.transform.position;
		dir.y = 0f;
		dir.Normalize();
		if (dir == Vector3.zero) return;

		transform.rotation = Quaternion.LookRotation(dir);
	}

	//----------------------------------------------------------------------------------------------------
	public void ChangeEulerAngles(float flRotationY)
	{
		transform.eulerAngles = new Vector3(0f, flRotationY, 0f);
	}

	//---------------------------------------------------------------------------------------------------
	protected void ChangeHeroMoveSpeed(float flMoveSpeed)
	{
		m_navMeshAgent.speed = flMoveSpeed;
	}

	#region Abnormal 상태이상 관련.

	//---------------------------------------------------------------------------------------------------
	protected void AbnormalSet(int nAbnormalStateId, long lAbnormalStateEffectInstanceId, int nSourceJobId, int nAbnormalLevel, int nDamageAbsorbShieldRemain, float flRemainTime, bool bHero)
	{
		//Debug.Log("######                AbnormalSet   : " + transform.name +" : " + (EnAbnormalState)nAbnormalStateId);
		m_nDamageAbsorbShieldRemain = nDamageAbsorbShieldRemain;

		switch ((EnAbnormalState)nAbnormalStateId)
		{
			case EnAbnormalState.Stun: // 1 : 기절
				StartCoroutine(CreatAbnormalEffect(nAbnormalStateId, lAbnormalStateEffectInstanceId, flRemainTime, bHero, true));

				if (m_flStunTimer == 0 || m_bStun == false)
				{
					m_flStunTimer = flRemainTime;
					StartCoroutine(StunTimer());
				}
				else
				{
					m_flStunTimer = flRemainTime;
				}
				break;

			case EnAbnormalState.Blockade: // 2 : 속박
				StartCoroutine(CreatAbnormalEffect(nAbnormalStateId, lAbnormalStateEffectInstanceId, flRemainTime, bHero, false));

				if (m_flBlockadeTimer == 0 || m_bBlockade == false)
				{
					m_flBlockadeTimer = flRemainTime;
					StartCoroutine(BlockadeTimer());
				}
				else
				{
					m_flBlockadeTimer = flRemainTime;
				}
				break;

			case EnAbnormalState.WindWalk:	// 11 : 풍행
				CsAbnormalState csAbnormalState = CsGameData.Instance.GetAbnormalState(nAbnormalStateId);
				m_flWindWalkMoveSpeed = (float)csAbnormalState.GetAbnormalStateRankSkillLevel(nAbnormalLevel).Value1 / 10000;
				StartCoroutine(CreatAbnormalEffect(nAbnormalStateId, lAbnormalStateEffectInstanceId, flRemainTime, bHero, false));
				break;

			case EnAbnormalState.Suppression: // 111 : 제압
				StartCoroutine(CreatAbnormalEffect(nAbnormalStateId, lAbnormalStateEffectInstanceId, flRemainTime, bHero, false));				// 제압 Effect
				StartCoroutine(CreatAbnormalEffect((int)EnAbnormalState.Stun, 0, flRemainTime, bHero, true));									// 제압 후 스턴상태 Effect
				
				if (m_flSuppressionTimer == 0 || m_bSuppression == false)
				{
					m_flSuppressionTimer = flRemainTime;
					StartCoroutine(SuppressionTimer());
				}
				else
				{
					m_flSuppressionTimer = flRemainTime;
				}
				break;

			default:
				StartCoroutine(CreatAbnormalEffect(nAbnormalStateId, lAbnormalStateEffectInstanceId, flRemainTime, bHero, false));
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangedAbnormalStateEffectDamageAbsorbShields(PDAbnormalStateEffectDamageAbsorbShield[] apDAbnormalStateEffectDamageAbsorbShield)
	{
		if (apDAbnormalStateEffectDamageAbsorbShield == null || apDAbnormalStateEffectDamageAbsorbShield.Length == 0) return;

		for (int i = 0; i < apDAbnormalStateEffectDamageAbsorbShield.Length; i++)
		{
			if (m_dicAbnormalEffect.ContainsKey(apDAbnormalStateEffectDamageAbsorbShield[i].abnormalStateEffectInstanceId))
			{
				m_nDamageAbsorbShieldRemain = apDAbnormalStateEffectDamageAbsorbShield[i].remainingAbsorbShieldAmount;
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CreatAbnormalEffect(int nAbnormalStateId, long lAbnormalStateEffectInstanceId, float flRemainTime, bool bHero, bool bAddHeight) // 상태이상 Effect 생성.
	{
		//Debug.Log("CreatAbnormalEffect    nAbnormalStateId = " + nAbnormalStateId + " // flRemainTime = " + flRemainTime+ " // lAbnormalStateEffectInstanceId = " + lAbnormalStateEffectInstanceId);
		Vector3 vtPos = transform.position;
		if (bAddHeight)
		{
			vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);
		}

		GameObject goAbnormalEffect = CsEffectPoolManager.Instance.CreateAbnormalEffect(nAbnormalStateId, transform, vtPos, bHero);

		if (goAbnormalEffect != null)
		{
			goAbnormalEffect.name = nAbnormalStateId.ToString();
			m_dicAbnormalEffect.Add(lAbnormalStateEffectInstanceId, goAbnormalEffect);
			yield return new WaitForSeconds(flRemainTime);

			if ((EnAbnormalState)nAbnormalStateId == EnAbnormalState.WindWalk)
			{
				m_flWindWalkMoveSpeed = 0;
			}
			RemoveAbnormalEffect(lAbnormalStateEffectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	protected virtual bool HeroAbnormalEffectHit(int nHp, Transform trAttacker, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects)
	{
		if (alRemovedAbnormalStateEffects != null)
		{
			for (int i = 0; i < alRemovedAbnormalStateEffects.Length; i++) // 상태이상 이팩트 삭제.
			{
				RemoveAbnormalEffect(alRemovedAbnormalStateEffects[i]);
			}
		}

		if (trAttacker != null)
		{
			if (nHp == 0 && !Dead)
			{
				return false;
			}
		}
		return true;
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveAbnormalEffect(long[] alAbnormalStateEffectInstanceId) // 상태이상 Effect 제거.
	{
		if (alAbnormalStateEffectInstanceId == null) return;

		for (int i = 0; i < alAbnormalStateEffectInstanceId.Length; i++)
		{
			if (m_dicAbnormalEffect.ContainsKey(alAbnormalStateEffectInstanceId[i]))
			{
				if ((EnAbnormalState)int.Parse(m_dicAbnormalEffect[alAbnormalStateEffectInstanceId[i]].name) == EnAbnormalState.WindWalk)
				{
					m_flWindWalkMoveSpeed = 0;
				}

				AbnormalEffectFinish((EnAbnormalState)int.Parse(m_dicAbnormalEffect[alAbnormalStateEffectInstanceId[i]].name));
				Destroy(m_dicAbnormalEffect[alAbnormalStateEffectInstanceId[i]]);
				m_dicAbnormalEffect.Remove(alAbnormalStateEffectInstanceId[i]);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void RemoveAbnormalEffect(long lAbnormalStateEffectInstanceId) // 상태이상 Effect 제거.
	{
		if (m_dicAbnormalEffect.ContainsKey(lAbnormalStateEffectInstanceId))
		{
			if ((EnAbnormalState)int.Parse(m_dicAbnormalEffect[lAbnormalStateEffectInstanceId].name) == EnAbnormalState.WindWalk)
			{
				m_flWindWalkMoveSpeed = 0;
			}

			AbnormalEffectFinish((EnAbnormalState)int.Parse(m_dicAbnormalEffect[lAbnormalStateEffectInstanceId].name));
			Destroy(m_dicAbnormalEffect[lAbnormalStateEffectInstanceId]);
			m_dicAbnormalEffect.Remove(lAbnormalStateEffectInstanceId);
		}
	}

	//---------------------------------------------------------------------------------------------------
	void AbnormalEffectFinish(EnAbnormalState enAbnormalState)
	{
		switch (enAbnormalState)
		{
			case EnAbnormalState.Stun:
				m_flStunTimer = 0f;
				m_bStun = false;
				break;
			case EnAbnormalState.Blockade:
				m_flBlockadeTimer = 0f;
				m_bBlockade = false;
				break;
			case EnAbnormalState.Suppression:
				m_flSuppressionTimer = 0f;
				m_bSuppression = false;
				break;
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator StunTimer() // 기절 상태 처리.
	{
		m_bStun = true;
		DamageByAbnormal(true);
		while (m_flStunTimer > 0)
		{
			m_flStunTimer -= Time.deltaTime;
			yield return null;
		}

		m_flStunTimer = 0f;
		m_bStun = false;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator BlockadeTimer()	// 속박 상태 처리.
	{
		m_bBlockade = true;
		DamageByAbnormal(false);
		while (m_flBlockadeTimer > 0)
		{
			m_flBlockadeTimer -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		m_flBlockadeTimer = 0f;
		m_bBlockade = false;
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator SuppressionTimer() // 제압 상태 처리.
	{
		m_bSuppression = true;
		DamageByAbnormal(true);
		while (m_flSuppressionTimer > 0)
		{
			m_flSuppressionTimer -= Time.deltaTime;
			yield return null;
		}

		m_flSuppressionTimer = 0f;
		m_bSuppression = false;
	}

	#endregion Abnormal

}

//---------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------
public class CsHitInfoNode
{
	float m_nHitTime;
	Transform m_trTarget;
	PDHitResult m_pDHitResult;
	PDHitResult m_pDAddHitResult;

	//public int AddDamage { get { return m_nAddDamage; } set { m_nAddDamage = value; } }
	public float HitTime { get { return m_nHitTime; } set { m_nHitTime = value; } }
	public Transform Target { get { return m_trTarget; } set { m_trTarget = value; } }
	public PDHitResult HitResult { get { return m_pDHitResult; } set { m_pDHitResult = value; } }
	public PDHitResult AddHitResult { get { return m_pDAddHitResult; } set { m_pDAddHitResult = value; } }

	public CsHitInfoNode(Transform trTarget, PDHitResult pDHitResult, float HitTime)
	{
		m_trTarget = trTarget;
		m_nHitTime = HitTime;
		m_pDHitResult = pDHitResult;
	}
}

//---------------------------------------------------------------------------------------------------
//---------------------------------------------------------------------------------------------------
public class CsHitSkillCast
{
	int m_nSkillId;
	int m_nChainSkillId;
	List<CsHitInfoNode> m_listcsHitInfoNode;

	public int SkillId { get { return m_nSkillId; } set { m_nSkillId = value; } }
	public int ChainSkillId { get { return m_nChainSkillId; } set { m_nChainSkillId = value; } }
	public List<CsHitInfoNode> HitInfoNodeList { get { return m_listcsHitInfoNode; } }

	public CsHitSkillCast(int nSkillId, int nChainSkillId)
	{
		m_nSkillId = nSkillId;
		m_nChainSkillId = nChainSkillId;
		m_listcsHitInfoNode = new List<CsHitInfoNode>();
	}
}




