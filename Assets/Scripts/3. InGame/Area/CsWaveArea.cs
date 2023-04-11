using System.Collections;
using UnityEngine;

public class CsWaveArea : CsBaseArea
{
	public float Looptime = 0.25f;
	public int WaveType = 0;
	GameObject m_goGroundWave;
	float m_flTimer = 0;
	Vector3 m_vtPlayerPrevPos = Vector3.zero;

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		switch (WaveType) 
		{
		case 0:
			m_goGroundWave = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Area/Wave");
			break;
		case 1:
			m_goGroundWave = CsIngameData.Instance.LoadAsset<GameObject>("Prefab/Area/WaveBig");
			break;
		
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		m_flTimer = 0;
		m_vtPlayerPrevPos = Vector3.zero;
	}

	//---------------------------------------------------------------------------------------------------
	public override void StayAction(Collider col) 
	{
		if (m_flTimer + Looptime < Time.time)
		{
			if (m_vtPlayerPrevPos != col.transform.position)
			{
				if (m_goGroundWave == null) return;
				GameObject goWave = Instantiate(m_goGroundWave, col.transform.position, Quaternion.identity) as GameObject;
				StartCoroutine(DestroyGameObject(goWave, 1.5f));
				m_vtPlayerPrevPos = col.transform.position;
			}

			m_flTimer = Time.time;
		}
	}

	//----------------------------------------------------------------------------------------------------
	IEnumerator DestroyGameObject(GameObject go, float flTime)
	{
		yield return new WaitForSeconds(flTime);
		Destroy(go);
	}
}
