using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CsEffectPoolManager : MonoBehaviour
{
    public enum EnEffectOwner { None = 0, User }

	static CsEffectPoolManager s_instance;
	public static CsEffectPoolManager Instance
	{
		get { return s_instance; }
	}

    Dictionary<string, GameObject> m_dicObject = new Dictionary<string, GameObject>();
	Coroutine m_crtn = null;

	int m_nTotalCount = 0;
	int m_nCurrentCount = 0;

	//----------------------------------------------------------------------------------------------------
	public int TotalCount
	{
		get { return m_nTotalCount; }
	}

	public int CurrentCount
	{
		get { return m_nCurrentCount; }
	}

	//----------------------------------------------------------------------------------------------------
	public void Init() // Intro에서 한번만 호출.
	{
		if (m_crtn == null)
		{
			Debug.Log("CsEffectPoolManager.Init()");
			m_crtn = StartCoroutine(StartLoadBundleAssetAsync());
		}
	}

	//----------------------------------------------------------------------------------------------------
	void Awake()
	{
		if (s_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		s_instance = this;
		DontDestroyOnLoad(gameObject);
	}

    //----------------------------------------------------------------------------------------------------
    IEnumerator StartLoadBundleAssetAsync()
    {
  //      DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/Resources/Prefab/Effect");
		//if (di.Exists)
		//{
		//	FileInfo[] fi = di.GetFiles("*.prefab");

		//	for (int i = 0; i < fi.Length; i++)
		//	{
		//		ResourceRequest reqPrefab = Resources.LoadAsync("Prefab/Effect/" + fi[i].Name.Replace(".prefab", ""));
		//		yield return reqPrefab;

		//		GameObject go = GameObject.Instantiate((GameObject)reqPrefab.asset, Vector3.zero, Quaternion.identity) as GameObject;
		//		go.transform.name = go.transform.name.Replace("(Clone)", "");
		//		go.transform.SetParent(gameObject.transform);
		//		go.SetActive(false);

		//		m_dicObject.Add(go.transform.name, go);
		//	}
		//}

		string[] astr = {   "Asura_Attack_01", "Asura_Attack_02", "Asura_Attack_03", 
							"Asura_Skill_01_01", "Asura_Skill_01_02", "Asura_Skill_02", "Asura_Skill_03", "Asura_Skill_03_Casting", "Asura_Skill_04", "Asura_Skill_04_Casting", "Asura_Hit",
							"Deva_Attack_00" ,"Deva_Attack_01", "Deva_Attack_01_Smoke", "Deva_Attack_03_01", "Deva_Attack_03_02",
							"Deva_Skill_01", "Deva_Skill_02", "Deva_Skill_03", "Deva_Skill_03_Casting", "Deva_Skill_04_01", "Deva_Skill_04_Hand", "Deva_Hit",
							"Gaia_Attack_01", "Gaia_Attack_02", "Gaia_Attack_03_01", "Gaia_Attack_03_02", 
							"Gaia_Skill_01_01", "Gaia_Skill_01_02", "Gaia_Skill_01_03",	"Gaia_Skill_02", "Gaia_Skill_03", "Gaia_Skill_03_Casting", "Gaia_Skill_04", "Gaia_Hit",
							"Witch_Attack_01", "Witch_Attack_02", "Witch_Attack_03_01", "Witch_Attack_03_02", 
							"Witch_Skill_01_01", "Witch_Skill_01_02", "Witch_Skill_02_01", "Witch_Skill_02_02", "Witch_Skill_02_03",
							"Witch_Skill_03", "Witch_Skill_03_Casting", "Witch_Skill_04_01", "Witch_Skill_04_01_Ball", "Witch_Skill_04_02", "Witch_Hit",
							"LevelUP", "Spawn", "Aggro", "HeroEnter", "HeroRevive", 
							"Abnormal_1", "Abnormal_2", "Abnormal_9", "Abnormal_10", "Abnormal_11", "Abnormal_12", "Abnormal_13", "Abnormal_111", "Dash01",
							"Monster_Hit", "Monster_Bullet_Hit", "Monster_3_Skill01", "Monster_3_Skill01_Hit", "Monster_3_Skill02", "Monster_15_Skill01", "Monster_15_Skill02",
							"Monster_19_Skill01", "Monster_19_Skill02", "Monster_20_Skill01", "Monster_20_Skill02" , "Monster_67_Skill01", "Monster_Smoke",
							"Tayming01", "Tayming02", "Tayming03", "RFX_darkFog", "MetalSkill", "Flight_Accelerate", "Flight_Direction"};

		m_nTotalCount = astr.Length;
		m_nCurrentCount = 0;

        for (int i = 0; i < astr.Length; i++)
        {
			ResourceRequest reqPrefab = CsIngameData.Instance.LoadAssetAsync<GameObject>("Prefab/Effect/" + astr[i]);
            yield return reqPrefab;
			if (reqPrefab == null)
			{
				Debug.Log("StartLoadBundleAssetAsync        astr[i] = " + astr[i] + " // reqPrefab = " + reqPrefab + "  //  Prefab Name 확인 필요.");
			}

			GameObject go = GameObject.Instantiate((GameObject)reqPrefab.asset, Vector3.zero, Quaternion.identity) as GameObject;
			go.transform.name = go.transform.name.Replace("(Clone)", "");
			go.transform.SetParent(gameObject.transform);
			go.SetActive(false);
			m_dicObject.Add(go.transform.name, go);
			m_nCurrentCount = i + 1;
        }
    }

	//----------------------------------------------------------------------------------------------------
	public GameObject CreateAbnormalEffect(int nAbnormalStateId, Transform trOwner, Vector3 vtPos, bool bHero)
	{
		string strEffectName = "Abnormal_" + nAbnormalStateId.ToString();

		if (CheckEffectName(strEffectName))
		{
			GameObject goEffect = Instantiate(m_dicObject[strEffectName].gameObject, vtPos, trOwner.rotation, trOwner);
			goEffect.name = strEffectName;
			goEffect.SetActive(true);
			return goEffect;
		}
		return null;
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayEffect(bool bEffectOwner, Transform trOwner, Vector3 vtCreatePos, string strEffectName, float flSec, float flRotationY = 0f)
	{
		StartCoroutine(NormalEffect(strEffectName, bEffectOwner ? trOwner : transform, vtCreatePos, trOwner.rotation, flSec, flRotationY));
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayBulletEffect(bool bEffectOwner, Transform trOwner, Vector3 vtCreatePos, Vector3 vtTargetPos, string strEffectName, float flSec, float flRotationY = 0f)
	{
		StartCoroutine(BulletEffect(strEffectName, bEffectOwner ? trOwner : transform, vtCreatePos, vtTargetPos, flSec, flRotationY));
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayHitEffect(bool bEffectOwner, Transform trOwner, Vector3 vtPos, Quaternion qtnRotation, string strEffectName, float flSec, float flRotationY = 0f)
	{
		StartCoroutine(NormalEffect(strEffectName, bEffectOwner ? trOwner : transform, vtPos, qtnRotation, flSec));
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayEffect(EnEffectOwner enEffectOwner, Transform trOwner, Vector3 vtCreatePos, string strEffectName, float flSec, float flRotationY = 0f)
	{
		PlayEffect(enEffectOwner == EnEffectOwner.User, trOwner, vtCreatePos, strEffectName, flSec, flRotationY);
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayBulletEffect(EnEffectOwner enEffectOwner, Transform trOwner, Vector3 vtCreatePos, Vector3 vtTargetPos, string strEffectName, float flSec, float flRotationY = 0f)
	{
		PlayBulletEffect(enEffectOwner == EnEffectOwner.User, trOwner, vtCreatePos, vtTargetPos, strEffectName, flSec, flRotationY);
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayHitEffect(EnEffectOwner enEffectOwner, Transform trOwner, Vector3 vtPos, Quaternion qtnRotation, string strEffectName, float flSec, float flRotationY = 0f)
	{
		PlayHitEffect(enEffectOwner == EnEffectOwner.User, trOwner, vtPos, qtnRotation, strEffectName, flSec, flRotationY);
	}

	//----------------------------------------------------------------------------------------------------
	public void PlayMoveHitEffect(Transform trHero, Vector3 vtTargetPos, string strArrow, string strHit, float flHeightOffset = 0, bool bSmoke =  false)
	{
		StartCoroutine(MoveByHitEffect(trHero, vtTargetPos, trHero.rotation, strArrow, strHit, flHeightOffset, bSmoke));
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator NormalEffect(string strEffectName, Transform trOwner, Vector3 vtPosition, Quaternion qtnRotation, float flSec ,float flRotationY = 0f)
	{
		if (CheckEffectName(strEffectName))
		{
			GameObject goEffect = Instantiate(m_dicObject[strEffectName].gameObject, vtPosition, qtnRotation, trOwner);

			if (flRotationY != 0)
			{
				goEffect.transform.eulerAngles = new Vector3(0f, flRotationY, 0f);
			}

			goEffect.gameObject.SetActive(true);

			yield return new WaitForSeconds(flSec);

			if (goEffect != null)
			{
				Destroy(goEffect);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator BulletEffect(string strEffectName, Transform trOwner, Vector3 vtPosition, Vector3 vtTarget, float flSec, float flRotationY)
	{
		if (CheckEffectName(strEffectName))
		{
			GameObject goEffect = Instantiate(m_dicObject[strEffectName].gameObject, vtPosition, trOwner.rotation, trOwner);
			goEffect.transform.eulerAngles = new Vector3(0f, flRotationY, 0f);
			goEffect.gameObject.SetActive(true);

			yield return new WaitForSeconds(flSec);

			if (goEffect != null)
			{
				Destroy(goEffect);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator MoveByHitEffect(Transform trAttacker, Vector3 vtTargetPos, Quaternion qtnRotation, string strEffectArrow, string strEffectHit, float flHeightOffset, bool bSmoke)
	{
		if (CheckEffectName(strEffectArrow))
		{
			float flDistance = 1f;
			float flTimer = Time.time;
			GameObject goHit = null;
			GameObject goSmoke = null;

			Vector3 vtCreatePos = new Vector3(trAttacker.position.x, trAttacker.position.y + flHeightOffset, trAttacker.position.z);
			GameObject goArrow = Instantiate(m_dicObject[strEffectArrow].gameObject, vtCreatePos, qtnRotation, transform);
			goArrow.transform.eulerAngles = new Vector3(0f, GetAngle(trAttacker.position, vtTargetPos), 0f);
			goArrow.SetActive(true);

			if (trAttacker == CsGameData.Instance.MyHeroTransform && Vector3.Distance(trAttacker.position, vtTargetPos) > 5)
			{
				flDistance = 1.5f;
			}

			if (CheckEffectName(strEffectHit)) // 피격 Effect 생성.
			{
				goHit = Instantiate(m_dicObject[strEffectHit].gameObject, vtTargetPos, qtnRotation, transform);
			}

			if (bSmoke && CsIngameData.Instance.Graphic != 0) // 투사체 스모크 생성(그래픽설정이 중or상 일때만).
			{
				string strEffectSmoke = "Monster_Smoke";
				if (CheckEffectName(strEffectSmoke))
				{
					goSmoke = Instantiate(m_dicObject[strEffectSmoke].gameObject, vtCreatePos, qtnRotation, transform);
					goSmoke.transform.eulerAngles = new Vector3(0f, GetAngle(trAttacker.position, vtTargetPos), 0f);
					goSmoke.SetActive(true);
				}
			}


			while (goArrow.transform.position != vtTargetPos)
			{
				if (Vector3.Distance(goArrow.transform.position, vtTargetPos) < flDistance)
				{
					if (goHit != null)
					{
						goHit.transform.position = goArrow.transform.position;
					}
					break;
				}
				else
				{
					if (Time.time - flTimer > 2)
					{
						break;
					}
				}

				goArrow.transform.position = Vector3.MoveTowards(goArrow.transform.position, vtTargetPos, Time.deltaTime * 28); // 제일 긴 거리의 0.3초 동안 속도 = 28 .
				if (goSmoke != null)
				{
					goSmoke.transform.position = goArrow.transform.position;
				}
				yield return new WaitForEndOfFrame();
			}

			Destroy(goArrow);

			if (goHit != null)
			{
				goHit.SetActive(true);
			}

			yield return new WaitForSeconds(1f);

			if (goSmoke != null) // 투사체 스모크 지연 삭제.
			{
				Destroy(goSmoke); 
			}

			if (goHit != null)
			{
				yield return new WaitForSeconds(0.5f);
				Destroy(goHit);
			}
		}
	}

	//----------------------------------------------------------------------------------------------------
	float GetAngle(Vector3 vtStart, Vector3 vtEnd)
	{
		Vector3 vt = vtEnd - vtStart;
		float flRotationY = Mathf.Atan2(vt.x, vt.z) * Mathf.Rad2Deg;
		if (flRotationY < 0)
		{
			flRotationY = 360f + flRotationY;
		}
		return flRotationY;
	}

	//----------------------------------------------------------------------------------------------------
	bool CheckEffectName(string stEffectName)
	{
		if (stEffectName == null) return false;
		if (m_dicObject.ContainsKey(stEffectName))
		{
			return true;
		}
		return false;
	}
}
