using ClientCommon;
using System;
using UnityEngine;

public enum EnCartState { Idle = 0, Move, Acceleration }
public enum EnCartQuestType { None = 0, Main, SupplySupport, GuildSupplySupport }

public class CsCartObject : CsMoveUnit, ICartObjectInfo
{
	enum EnCartAnimStatus { Idle = 0, Walk, Move }
	static int s_nAnimatorHash_status = Animator.StringToHash("status");

	CsCart m_csCart = null;
	CapsuleCollider m_capsuleCollider = null;
	PDCartInstance m_pDCartInstance;

	RectTransform m_rtfHUD = null;
	Transform m_trRidindCart = null;
	Transform m_trCart = null;
	Transform m_trOwner = null;

	Guid m_guidOwnerId;
	string m_strOwnerName;
	int m_nOwnerNationId;

	bool m_bRidingCart = false;

	EnCartState m_enCartState = EnCartState.Idle;
	public EnCartQuestType m_enCartQuestType = EnCartQuestType.None;
	//---------------------------------------------------------------------------------------------------
	public EnCartState CartState { get { return m_enCartState; } }
	public EnCartQuestType CartQuestType { get { return m_enCartQuestType; } }
	public PDCartInstance CartInstance { get { return m_pDCartInstance; } set { { m_pDCartInstance = value; } } }
	public Transform RidindCart { get { return m_trRidindCart; } }
	public Guid OwnerId { get { return m_guidOwnerId; } }
	public string OwnerName { get { return m_strOwnerName; } }
	public int OwnerNationId { get { return m_nOwnerNationId; } }
	public CsCart Cart { get { return m_csCart; } }
	public bool RidingCart { get { return m_bRidingCart; } }

	//---------------------------------------------------------------------------------------------------
	public void Init(PDCartInstance pDCartInstance)
	{
		Debug.Log("CsCartObject.Init()  " + pDCartInstance.instanceId);
		m_pDCartInstance = pDCartInstance;
		InstanceId = m_pDCartInstance.instanceId;
		transform.name = InstanceId.ToString();
		MaxHp = m_pDCartInstance.maxHP;
		Hp = m_pDCartInstance.hp;
		Level = m_pDCartInstance.level;

		m_csCart = CsGameData.Instance.GetCart(m_pDCartInstance.cartId);
		m_guidOwnerId = m_pDCartInstance.ownerId;
		m_strOwnerName = m_pDCartInstance.ownerName;
		m_nOwnerNationId = m_pDCartInstance.ownerNationId;
		if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) != null)
		{
			m_trOwner = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).transform;
		}

		transform.position = CsRplzSession.Translate(m_pDCartInstance.position);
		ChangeEulerAngles(m_pDCartInstance.rotationY);
		CsGameData.Instance.ListCartObjectInfo.Add(this);

		m_capsuleCollider.radius = 1f;
		m_rtfHUD = CsGameEventToUI.Instance.OnEventCreateCartHUD(InstanceId);		

		if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) != null) // Layer 설정.
		{
			gameObject.layer = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).gameObject.layer;
		}
		else
		{
			gameObject.layer = LayerMask.NameToLayer("Hero");
		}

		if (CsMainQuestManager.Instance.MainQuestState != EnMainQuestState.None && CsMainQuestManager.Instance.MainQuest.MainQuestType == EnMainQuestType.Cart) // 메인퀘스트 중일때.
		{
			m_enCartQuestType = EnCartQuestType.Main;
			transform.tag = "Cart";
		}
		else
		{
			if (CsGuildManager.Instance.GuildSupplySupportState == EnGuildSupplySupportState.Accepted)
			{
				m_enCartQuestType = EnCartQuestType.GuildSupplySupport;
			}
			else // 보금 지원 카트퀘스트 중일때.
			{
				m_enCartQuestType = EnCartQuestType.SupplySupport;
			}

			if (CsNationWarManager.Instance.IsEnemyNation(m_nOwnerNationId)) // 적국이면.
			{
				transform.tag = "EnemyCart";
			}
			else // 같은 국가이면 공격 대상 안되게 변경.
			{
				transform.tag = "Cart";
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_trRidindCart = transform.Find("Horse_Cart");
		m_trCart = transform.Find("Cart");
		m_animator = m_trRidindCart.GetComponent<Animator>();
		m_capsuleCollider = GetComponent<CapsuleCollider>();
	}

	//---------------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		if (m_bApplicationQuit) return;

		if (CsGameData.Instance.ListCartObjectInfo.Contains(this))
		{
			CsGameData.Instance.ListCartObjectInfo.Remove(this);
		}

		if (CsIngameData.Instance.TargetTransform == transform)
		{
			CsGameEventToUI.Instance.OnEventSelectCartInfoStop();
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (m_rtfHUD != null)
		{
			m_rtfHUD = null;
			CsGameEventToUI.Instance.OnEventDeleteCartHUD(InstanceId);
		}

		m_animator = null;
		m_trRidindCart = null;
		m_trCart = null;
		base.OnDestroy();
	}

	//----------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		HUDUpdatePos();
	}

	//----------------------------------------------------------------------------------------------------
	void HUDUpdatePos()
	{
		if (CsIngameData.Instance.InGameCamera == null) return;
		if (m_rtfHUD == null) return;

		if (m_trOwner == null)
		{
			if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) != null)
			{
				m_trOwner = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).transform;
			}
			return;
		}

		if (m_bRidingCart)
		{
			m_rtfHUD.position = new Vector3(transform.position.x, m_trOwner.position.y + (CsIngameData.Instance.IngameManagement.GetHeroHeight(m_guidOwnerId)), transform.position.z);
		}
		else
		{
			m_rtfHUD.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
		}
		
		float flDistance = (Vector3.Distance(CsIngameData.Instance.InGameCamera.transform.position, transform.position) - 10) / 25;
		m_rtfHUD.localScale = new Vector3(0.6f + flDistance, 0.6f + flDistance, 0.6f + flDistance);
	}

	//---------------------------------------------------------------------------------------------------
	void SetAnimStatus(EnCartAnimStatus enAnimStatus)
	{
		if (enAnimStatus != (EnCartAnimStatus)m_animator.GetInteger(s_nAnimatorHash_status))
		{
			m_animator.SetInteger(s_nAnimatorHash_status, (int)enAnimStatus);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnCartState enNewCartState)
	{
		if (enNewCartState == EnCartState.Idle)
		{
			SetAnimStatus(EnCartAnimStatus.Idle);
		}
		else if (enNewCartState == EnCartState.Move)
		{
			SetAnimStatus(EnCartAnimStatus.Walk);
		}
		else if (enNewCartState == EnCartState.Acceleration) // 가속.
		{
			SetAnimStatus(EnCartAnimStatus.Move);
		}
		m_enCartState = enNewCartState;
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartHit(PDHitResult HitResult)
	{
		if (CsGameData.Instance.MyHeroInfo.HeroId == m_guidOwnerId)
		{
			SetDamageText(HitResult, true);
		}
		else if (CsGameData.Instance.MyHeroInfo.HeroId == CsIngameData.Instance.IngameManagement.GetHeroId(CsIngameData.Instance.IngameManagement.GetAttacker(HitResult.attacker)))
		{
			SetDamageText(HitResult);
		}

		Hp = HitResult.hp;

		if (CsIngameData.Instance.TargetTransform == transform) // 선택된 플레이어 정보 전달.
		{
			CsGameEventToUI.Instance.OnEventSelectInfoUpdate();
		}

		if (Hp == 0)
		{
			if (CsGameData.Instance.MyHeroInfo.HeroId != m_guidOwnerId)
			{
				CsIngameData.Instance.IngameManagement.RemoveHeroCart(m_guidOwnerId, InstanceId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartAbnormalStateEffectStart(long lAbnormalStateEffectId, int nAbnormalStateId, int nSourceJobId, int nAbnormalLevel, int nDamageAbsorbShieldRemain, float flRemainTime,  long[] alRemovedAbnormalEffects)
	{
		RemoveAbnormalEffect(alRemovedAbnormalEffects);
		AbnormalSet(nAbnormalStateId, lAbnormalStateEffectId, nSourceJobId, nAbnormalLevel, nDamageAbsorbShieldRemain, flRemainTime, true);
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartAbnormalStateEffectHit(int nHp, long[] alRemovedAbnormalStateEffects, long lAbnormalStateEffectInstanceId, int nDamage, int nHpDamage, Transform trAttacker)
	{
		Debug.Log("NetEventCartAbnormalStateEffectHit   lAbnormalStateEffectInstanceId = "+ lAbnormalStateEffectInstanceId);
		if (!HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects))
		{
			if (CsGameData.Instance.MyHeroInfo.HeroId != m_guidOwnerId)
			{
				CsIngameData.Instance.IngameManagement.RemoveHeroCart(m_guidOwnerId, InstanceId);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void NetEventCartAbnormalStateEffectFinished(long lAbnormalStateEffectInstanceId) // 상태이상 종료.
	{
		Debug.Log("NetEventCartAbnormalStateEffectFinished   lAbnormalStateEffectInstanceId = "+ lAbnormalStateEffectInstanceId);
		RemoveAbnormalEffect(lAbnormalStateEffectInstanceId);
	}

	//---------------------------------------------------------------------------------------------------
	protected override bool HeroAbnormalEffectHit(int nHp, Transform trAttacker, int nDamage, int nHpDamage, long[] alRemovedAbnormalStateEffects)
	{
		if (CsIngameData.Instance.InGameCamera.SleepMode == false)
		{
			Vector3 vtPos = new Vector3(transform.position.x, transform.position.y + Height, transform.position.z);

			if (CsGameData.Instance.MyHeroInfo.HeroId == m_guidOwnerId)
			{
				CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), true);  // 데미지 Text 처리.
			}
			else if (CsGameData.Instance.MyHeroInfo.HeroId == CsIngameData.Instance.IngameManagement.GetHeroId(trAttacker))
			{
				CsGameEventToUI.Instance.OnEventCreatDamageText(EnDamageTextType.None, nDamage, Camera.main.WorldToScreenPoint(vtPos), false);  // 데미지 Text 처리.
			}
		}

		Hp = nHp;

		if (CsIngameData.Instance.TargetTransform == transform) // 선택된 플레이어 정보 전달.
		{
			CsGameEventToUI.Instance.OnEventSelectInfoUpdate();
		}
		return base.HeroAbnormalEffectHit(nHp, trAttacker, nDamage, nHpDamage, alRemovedAbnormalStateEffects);
	}

	//---------------------------------------------------------------------------------------------------
	public void ChangeRiding(bool bRiding)
	{
		m_bRidingCart = bRiding;

		if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) != null)
		{
			m_trOwner = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).transform;
		}

		if (CsIngameData.Instance.TargetTransform == transform)
		{
			CsIngameData.Instance.IngameManagement.TartgetReset();
		}

		if (bRiding)
		{
			CsJob csJob = CsIngameData.Instance.IngameManagement.GetHeroJob(m_trOwner);
			if (csJob != null)
			{
				if ((EnJob)csJob.ParentJobId == EnJob.Gaia)
				{
					m_trRidindCart.localScale = new Vector3(1.1f, 1.1f, 1.1f);
				}
				else if ((EnJob)csJob.ParentJobId == EnJob.Asura)
				{
					m_trRidindCart.localScale = new Vector3(0.9f, 0.9f, 0.9f);
				}
				else if ((EnJob)csJob.ParentJobId == EnJob.Deva)
				{
					m_trRidindCart.localScale = new Vector3(1f, 1f, 1f);
				}
				else if ((EnJob)csJob.ParentJobId == EnJob.Witch)
				{
					m_trRidindCart.localScale = new Vector3(0.8f, 0.8f, 0.8f);
				}
			}

			m_capsuleCollider.height = 2f;
			m_capsuleCollider.center = new Vector3(0, 1, 0);
			m_trCart.gameObject.SetActive(false);
			m_trRidindCart.gameObject.SetActive(true);
		}
		else
		{
			if (CsIngameData.Instance.TargetTransform == transform)
			{
				CsGameEventToUI.Instance.OnEventSelectCartInfoStop();
				CsIngameData.Instance.IngameManagement.TartgetReset();
			}

			m_capsuleCollider.height = 1f;
			m_capsuleCollider.center = new Vector3(0, 0, 0);
			m_trRidindCart.gameObject.SetActive(false);
			m_trCart.gameObject.SetActive(true);
		}
	}

	//---------------------------------------------------------------------------------------------------
	long ICartObjectInfo.GetInstanceId()
	{
		return InstanceId;
	}

	//---------------------------------------------------------------------------------------------------
	CsCartObject ICartObjectInfo.GetCartObject()
	{
		return this;
	}

	//---------------------------------------------------------------------------------------------------
	Transform ICartObjectInfo.GetTransform()
	{
		return transform;
	}
}
