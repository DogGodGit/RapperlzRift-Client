using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsMonsterBossOsiris : CsMonsterBoss
{
	Transform m_trHand_Left = null;
	Transform m_trHand_Right = null;
	Dictionary<string, GameObject> m_dicEffect = new Dictionary<string, GameObject>();

	//------------------------------------------------------------------------------------------
	protected override void Start()
	{
		base.Start();

		m_trHand_Left = transform.Find(FindLeftHandString());
		m_trHand_Right = transform.Find(FindRightHandString());

		string[] astr = { "Osiris_Attack_02", "Osiris_Attack_02_L", "Osiris_Skill_01_01", "Osiris_Skill_01_Casting", "Osiris_Skill_01_L", "Osiris_Skill_01_R", "Osiris_Skill_02_01", "Osiris_Skill_02_Casting" };
		StartCoroutine(StartLoadEffect(astr));
	}

	//------------------------------------------------------------------------------------------
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	//------------------------------------------------------------------------------------------
	IEnumerator StartLoadEffect(string[] astr)
	{
		for (int i = 0; i < astr.Length; i++)
		{
			ResourceRequest reqPrefab = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Effect/" + astr[i]);
			yield return reqPrefab;

			GameObject go = GameObject.Instantiate((GameObject)reqPrefab.asset, Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
			go.transform.name = go.transform.name.Replace("(Clone)", "");
			go.transform.SetParent(gameObject.transform);
			go.SetActive(false);
			m_dicEffect.Add(go.transform.name, go);
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator NormalEffect(string strEffectName, Transform trOwner, Vector3 vtPosition, Quaternion qtnRotation, float flSec)
	{
		if (m_dicEffect.ContainsKey(strEffectName))
		{
			GameObject goEffect = Instantiate(m_dicEffect[strEffectName].gameObject, vtPosition, qtnRotation, trOwner);

			goEffect.gameObject.SetActive(true);

			yield return new WaitForSeconds(flSec);

			if (goEffect != null)
			{
				Destroy(goEffect);
			}
		}
	}

	//------------------------------------------------------------------------------------------
	string FindLeftHandString()
	{
		return "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand";
	}

	//------------------------------------------------------------------------------------------
	string FindRightHandString()
	{
		return "Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand";
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Attack02()	// 손으로 나가는 레이져
	{
		StartCoroutine(NormalEffect("Osiris_Attack_02", transform.parent, transform.position, transform.rotation, 0.8f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Attack02_L()
	{
		StartCoroutine(NormalEffect("Osiris_Attack_02_L", m_trHand_Left, m_trHand_Left.position, transform.rotation, 2.5f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill01_01()
	{
		// 위치 타겟팅 지정
		Vector3 vtTargetPos = CsGameData.Instance.MyHeroTransform.position;

		if (m_guidOwnerId != CsGameData.Instance.MyHeroInfo.HeroId)
		{
			if (CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId) == null) return;

			vtTargetPos = CsIngameData.Instance.IngameManagement.GetHero(m_guidOwnerId).transform.position;
		}

		StartCoroutine(NormalEffect("Osiris_Skill_01_01", transform.parent, vtTargetPos, transform.rotation, 0.8f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill01_Casting()
	{
		StartCoroutine(NormalEffect("Osiris_Skill_01_Casting", transform.parent, transform.position, transform.rotation, 2.5f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill_01_L()
	{
		StartCoroutine(NormalEffect("Osiris_Skill_01_L", m_trHand_Left, m_trHand_Left.position, transform.rotation, 2.5f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill_01_R()
	{
		StartCoroutine(NormalEffect("Osiris_Skill_01_R", m_trHand_Right, m_trHand_Right.position, transform.rotation, 2.5f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill02_01()
	{
		StartCoroutine(NormalEffect("Osiris_Skill_02_01", transform.parent, transform.position, transform.rotation, 2.5f));
	}

	//------------------------------------------------------------------------------------------
	void OnAnimEffect_Skill02_Casting()
	{
		StartCoroutine(NormalEffect("Osiris_Skill_02_Casting", transform.parent, transform.position, transform.rotation, 0.8f));
	}
}
