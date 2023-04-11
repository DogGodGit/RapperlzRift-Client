using UnityEngine;
using UnityEngine.AI;

public enum EnWayPointType { None, Move, Interaction, Npc, Battle }

public class CsWayPointObject : MonoBehaviour
{
	Transform m_trPoint;

	Projector m_proCircleCenter;
	Projector m_proCircleLine;
	Projector m_proCircleUp;

	float m_flRadius = 1f;

	[SerializeField]
	EnWayPointType m_enWayPointType = EnWayPointType.None;

	public float Radius { get { return m_flRadius; } }
	public EnWayPointType WayPointType { get { return m_enWayPointType; } }
	public Vector3 WayPointPos { get { return m_trPoint == null ? Vector3.zero : m_trPoint.position; } }

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_trPoint = transform.Find("Point");
		m_proCircleCenter = m_trPoint.Find("CircleCenter").GetComponent<Projector>();
		m_proCircleLine = m_trPoint.Find("CircleLine").GetComponent<Projector>();
		m_proCircleUp = m_trPoint.Find("CircleUp").GetComponent<Projector>();

		Material newMaterial1 = new Material(m_proCircleCenter.material);
		Material newMaterial2 = new Material(m_proCircleLine.material);
		Material newMaterial3 = new Material(m_proCircleUp.material);
		m_proCircleCenter.material = newMaterial1;
		m_proCircleLine.material = newMaterial2;
		m_proCircleUp.material = newMaterial3;
	}

	//---------------------------------------------------------------------------------------------------
	public void SetWayPoint(EnWayPointType enNewWayPointType,Vector3 vtPos, float flRadius = 1)
	{
		ViewSetting(enNewWayPointType, vtPos, flRadius);
	}

	//---------------------------------------------------------------------------------------------------
	void ViewSetting(EnWayPointType enWayPointType, Vector3 vtPos, float flRadius)
	{
		if (m_trPoint == null || m_proCircleCenter == null || m_proCircleLine  == null ||  m_proCircleUp == null) return;

		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(vtPos, out navMeshHit, 5.0f, NavMesh.AllAreas))
		{
			vtPos.y = navMeshHit.position.y;	// Y값 보정(메타를 정확하게 입력하지 않은경우를 위해.
		}

		m_trPoint.gameObject.SetActive(false);
		m_trPoint.position = vtPos;

		if (enWayPointType != EnWayPointType.None)
		{
			Color color = GetColor(enWayPointType);
	
			m_proCircleLine.material.SetColor("_TintColor", color);
			m_proCircleLine.orthographicSize = flRadius;
			m_proCircleUp.material.SetColor("_TintColor", color);
			m_proCircleUp.orthographicSize = flRadius;

			color.a = 0.2f;
			m_proCircleCenter.material.SetColor("_TintColor", color);
			m_proCircleCenter.orthographicSize = flRadius;

			m_trPoint.gameObject.SetActive(true);
		}

		m_enWayPointType = enWayPointType;
		m_flRadius = flRadius;
	}

	//---------------------------------------------------------------------------------------------------
	Color GetColor(EnWayPointType enWayPointType)
	{
		switch (enWayPointType)
		{
			case EnWayPointType.Move:
				return new Color(0f, 0.938f, 1f, 0.247f);
			case EnWayPointType.Interaction:
				return new Color(1.0f, 0.702f, 0.052f, 0.294f);
			case EnWayPointType.Npc:
				return new Color(0.352f, 1.0f, 0.222f, 0.235f);
			case EnWayPointType.Battle:
				return new Color(0.921f, 0.254f, 0.335f, 0.25f);
		}
		return new Color(0, 0, 0, 0);
	}
}
