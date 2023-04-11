using SimpleDebugLog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CsDisplayPlayerPath : MonoBehaviour
{
	const float c_flPathIconDistance = 3.0f; // 화살표 간격
	const float c_flRecreationDistance = 3.5f;
//	const float c_flTargetMarginDistance = 1.7f;
	const int c_nPathIconCount = 15;

	bool m_bEnablePath = false;
	Vector3 m_vtMoveTarget;
	Vector3 m_vtMoveSource;
	Vector3[] m_avt;
	int m_nPathInconIndex = -1;
	int m_nLayerMask;

	CsHero m_csHero;

	NavMeshAgent m_navMeshAgent;
	CsSimpleTimer m_timer = new CsSimpleTimer();

	public Vector3 MoveTarget { get { return m_vtMoveTarget; } set { m_vtMoveTarget = value; } } // 이동위치를 세팅함
	List<Transform> m_listTrPathIcon = new List<Transform>();

	//---------------------------------------------------------------------------------------------------
	public static CsDisplayPlayerPath Create(string strName, Transform trParent)
	{
		GameObject go = new GameObject();
		go.name = strName;
		go.transform.position = CsGameData.Instance.MyHeroTransform.position;
		go.transform.SetParent(trParent);
		go.AddComponent<NavMeshAgent>();
		return go.AddComponent<CsDisplayPlayerPath>();
	}

	//---------------------------------------------------------------------------------------------------
	void Awake()
	{
		m_navMeshAgent = GetComponent<NavMeshAgent>();
		Transform trPrefab = CsIngameData.Instance.LoadAsset<Transform>("Prefab/PathIcon");
		for (int i = 0; i < c_nPathIconCount; i++)
		{
			Transform trPathIcon = MonoBehaviour.Instantiate(trPrefab); // 화살표 생성
			trPathIcon.gameObject.SetActive(false);
			m_listTrPathIcon.Add(trPathIcon);
		}

		if (CsGameData.Instance.MyHeroTransform != null)
		{
			m_csHero = CsGameData.Instance.MyHeroTransform.GetComponent<CsHero>();
		}
		m_timer.Init(0.05f);
		m_nLayerMask = LayerMask.GetMask("Terrain");
	}

	//---------------------------------------------------------------------------------------------------
	public void Update()
	{
		if (m_timer.CheckSetTimer())
		{
			OnTimer();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_csHero = null;
		m_navMeshAgent = null;
//		m_navMeshAgentHero = null;
	}

	//---------------------------------------------------------------------------------------------------
	void MakePath(bool bFirst = false)
	{
		m_bEnablePath = true;
		m_avt = null;
		m_nPathInconIndex = -1;

		m_navMeshAgent.enabled = false;
		m_vtMoveSource = transform.position = m_csHero.transform.position;
		NavMeshSetting(m_csHero.MyHeroNavMeshAgent.speed);
		m_navMeshAgent.baseOffset = m_csHero.MyHeroNavMeshAgent.baseOffset;
		m_navMeshAgent.enabled = true;
		
		StartCoroutine(CoroutineMakePath(bFirst));
	}

	//---------------------------------------------------------------------------------------------------
	public void SetPath(Vector3 vtDesination)
	{
		//dd.d("SetPath  vtDesination = "+ vtDesination);
		ResetPath();
		m_vtMoveTarget = vtDesination;
		MakePath(true);
	}

	//---------------------------------------------------------------------------------------------------
	public void ResetPath()
	{
		//dd.d("ResetPath");
		m_vtMoveTarget = Vector3.zero;
		m_bEnablePath = false;
		m_avt = null;
		ResetPathIcon();
	}

	//---------------------------------------------------------------------------------------------------
	void CornerToPoints(Vector3[] avtCor)
	{
//		dd.d("CornerToPoints1", avtCor.Length);
		List<Vector3> list = new List<Vector3>();
		int nCount = avtCor.Length - 1;
		float flRemainder = 1.0f;
		for (int i = nCount; i >= 1; i--)
		{
			//	int j = nCount - i;
			flRemainder = MakeTrailers(list, flRemainder, avtCor[i], avtCor[i - 1]);
		}

		m_avt = list.ToArray();
	}

	//---------------------------------------------------------------------------------------------------
	float MakeTrailers(List<Vector3> list, float flBeforeRemainder, Vector3 vtPrev, Vector3 vtNext)
	{
		//		dd.d("MakeTrailers", flBeforeRemainder, vtPrev, vtNext);
		float flDist = Vector3.Distance(vtPrev, vtNext);
		float flDistTotal = flDist + flBeforeRemainder;
		if (flDistTotal < c_flPathIconDistance)
		{
			return flDistTotal;
		}

		Vector3 vtPrev2 = Vector3.LerpUnclamped(vtPrev, vtNext, -flBeforeRemainder / flDist);

		flDistTotal = Vector3.Distance(vtPrev2, vtNext);


		int nPointCount = (int)(flDistTotal / c_flPathIconDistance);
		//		dd.d(nPointCount, flDistTotal % c_flPathIconDistance);

		for (int i = 1; i <= nPointCount; i++)
		{
			Vector3 vt = Vector3.Lerp(vtPrev2, vtNext, i * c_flPathIconDistance / flDistTotal);
			//dd.d("MakeTrailers", vt);

//			Vector3 vtSource = new Vector3(vt.x, vt.y + 5.0f, vt.z);
//			RaycastHit rh;
//			if (Physics.Raycast(vtSource, Vector3.down, out rh, 10, m_nLayerMask))
//			{
//				//dd.d(rh.collider.gameObject.layer, rh.point);
//				vt.y = rh.point.y;
//			}

			NavMeshHit nh;
			if (NavMesh.SamplePosition(vt, out nh, 5.0f, NavMesh.AllAreas))
			{				
				//vt.y = nh.position.y + m_csHero.MyHeroNavMeshAgent.baseOffset;  // 비행이 공중에 표시하는 부분 테스트용.
				vt.y = nh.position.y;
			}


//			list.Add(vt);
			list.Insert(0, vt);
		}

		return flDistTotal % c_flPathIconDistance;
	}

	//---------------------------------------------------------------------------------------------------
	float GetDistance(int nIndex, Vector3 vt)
	{
		if (m_avt != null && nIndex < m_avt.Length)
		{
			Vector2 vt2A = new Vector2(vt.x, vt.z);
			Vector2 vt2B = new Vector2(m_avt[nIndex].x, m_avt[nIndex].z);
			return Vector2.Distance(vt2A, vt2B);
		}
		return 0;
	}

	//---------------------------------------------------------------------------------------------------
	int FindNearIndex2(Vector3 vtPlayer, ref float flDistance)
	{
		if (m_avt == null || m_avt.Length == 0)
		{
			//dd.d("FindNearIndex 1", m_avt == null);
			return -1;
		}

		Vector2 vt2Player = new Vector2(vtPlayer.x, vtPlayer.z);
		Vector2 vt2 = new Vector2();

		int nMinIndex = -1;
		float flMinDist = 1000000.0f;
		List<float> list = new List<float>();

		for (int i = 0; i < m_avt.Length; i++)
		{
			vt2.Set(m_avt[i].x, m_avt[i].z);
			float flDist = Vector2.Distance(vt2, vt2Player);

			if (flDist < flMinDist)
			{
				flMinDist = flDist;
				nMinIndex = i;
			}
		}

		flDistance = flMinDist;

		if (m_avt.Length > 1)
		{
			if (nMinIndex == 0)
			{
				float fl1 = GetAngleWithFront(nMinIndex, vt2Player);

				if (fl1 <= 90.0f)
				{
					flDistance = GetDistance(nMinIndex + 1, vtPlayer);
					return nMinIndex + 1;
				}
			}
			else // if (nMinIndex == m_avt.Length - 1)
			{
				float fl1 = GetAngleWithRear(nMinIndex, vt2Player);

				if (fl1 >= 90.0f)
				{
					if (nMinIndex == m_avt.Length - 1)
					{
						return m_avt.Length;
					}

					flDistance = GetDistance(nMinIndex + 1, vtPlayer);
					return nMinIndex + 1;
				}
				//			dd.d("@@@@@@@@@@@@", fl1, fl2, fl1 + fl2);
			}
		}
		else
		{
			float fl = GetAngleWithBothEnd(vt2Player);
			if (fl <= 90.0f)
			{
				return m_avt.Length;
			}
		}

		return nMinIndex;
	}

	//---------------------------------------------------------------------------------------------------
	Vector2 GetDirectionVector2WithFront(int nIndex)
	{
		if (nIndex + 1 < m_avt.Length)
		{
			Vector2 vt2O = new Vector2(m_avt[nIndex].x, m_avt[nIndex].z);
			Vector2 vt2F = new Vector2(m_avt[nIndex + 1].x, m_avt[nIndex + 1].z);
			Vector2 vtRet = vt2F - vt2O;
			vtRet.Normalize();
			return vtRet;
		}
		return Vector2.zero;
	}

	//---------------------------------------------------------------------------------------------------
	Vector2 GetDirectionVector2WithRear(int nIndex)
	{
		if (nIndex < m_avt.Length)
		{
			Vector2 vt2R = new Vector2(m_avt[nIndex - 1].x, m_avt[nIndex - 1].z);
			Vector2 vt2O = new Vector2(m_avt[nIndex].x, m_avt[nIndex].z);
			Vector2 vtRet = vt2R - vt2O;
			vtRet.Normalize();
			return vtRet;
		}
		return Vector2.zero;
	}

	//---------------------------------------------------------------------------------------------------
	Vector2 GetDirectionVector2(int nIndex, Vector2 vt2)
	{
		if (nIndex < m_avt.Length)
		{
			Vector2 vt2O = new Vector2(m_avt[nIndex].x, m_avt[nIndex].z);
			Vector2 vtRet = vt2 - vt2O;
			vtRet.Normalize();
			return vtRet;
		}
		return Vector2.zero;
	}

	//---------------------------------------------------------------------------------------------------
	float GetAngleWithFront(int nIndex, Vector3 vt2)
	{
		Vector2 vt2DF = GetDirectionVector2WithFront(nIndex);
		Vector2 vt2DP = GetDirectionVector2(nIndex, vt2);
		return Vector2.Angle(vt2DF, vt2DP);
	}

	//---------------------------------------------------------------------------------------------------
	float GetAngleWithRear(int nIndex, Vector3 vt2)
	{
		Vector2 vt2DF = GetDirectionVector2WithRear(nIndex);
		Vector2 vt2DP = GetDirectionVector2(nIndex, vt2);
		return Vector2.Angle(vt2DF, vt2DP);
	}

	//---------------------------------------------------------------------------------------------------
	float GetAngleWithBothEnd(Vector3 vt2)
	{
		Vector2 vt2DF = GetDirectionVector2BetweenBothEnds();
		Vector2 vt2DP = GetDirectionVector2(0, vt2);
		return Vector2.Angle(vt2DF, vt2DP);
	}

	//---------------------------------------------------------------------------------------------------
	Vector2 GetDirectionVector2BetweenBothEnds()
	{
		Vector2 vt2Src = new Vector2(m_vtMoveSource.x, m_vtMoveSource.z);
		Vector2 vt2Tgt = new Vector2(m_vtMoveTarget.x, m_vtMoveTarget.z);
		Vector2 vtRet = vt2Tgt - vt2Src;
		vtRet.Normalize();
		return vtRet;
	}

	//---------------------------------------------------------------------------------------------------
	void OnTimer()
	{
//		dd.d("OnTimer()", m_bEnablePath, m_avt == null);
		if (m_bEnablePath)
		{
			if (m_avt != null)
			{
				float flDist = 0;
				int nPathIconIndex = FindNearIndex2(m_csHero.transform.position, ref flDist);
				//dd.d("OnTimer A", nPathIconIndex, m_nPathInconIndex, m_avt.Length);

				if (nPathIconIndex < 0 || nPathIconIndex >= m_avt.Length)
				{
					if ((Vector3.Distance(m_vtMoveSource, m_csHero.transform.position) > c_flRecreationDistance))
					{
						MakePath();
					}
					else
					{
						ResetPathIcon();
					}
					return;
				}

				if (flDist > c_flRecreationDistance)
				{
					MakePath();
					return;
				}

//				dd.d("OnTimer E", m_avt.Length, m_listTrPathIcon.Count, flDist, nPathIconIndex, m_nPathInconIndex);
				if (nPathIconIndex == m_nPathInconIndex) return;

				m_nPathInconIndex = nPathIconIndex;
				DrawPathIcon(nPathIconIndex);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void ResetPathIcon()
	{
		for (int i = 0; i < c_nPathIconCount; i++)
		{
			if (m_listTrPathIcon != null)
			{
				if (m_listTrPathIcon[i] != null)
				{
					m_listTrPathIcon[i].gameObject.SetActive(false);
				}
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	void DrawPathIcon(int nFirstIconIndex)
	{
//		if (m_avt == null)
//		{
//		    dd.d("DrawPathIcon1", nFirstIconIndex);
//		}
//		else
//		{
//		    dd.d("DrawPathIcon2", nFirstIconIndex, m_avt.Length);
//		}

		for (int i = 0; i < m_listTrPathIcon.Count; i++)
		{
			int nIndex = nFirstIconIndex + i;
			Transform tr = m_listTrPathIcon[i];
//			if (tr == null) return;
			if (m_avt != null && nIndex < m_avt.Length)
			{
				if (!tr.gameObject.activeSelf)
				{
					tr.gameObject.SetActive(true);
				}
				tr.position = m_avt[nIndex];
				if (nIndex + 1 < m_avt.Length)
				{
					tr.LookAt(m_avt[nIndex + 1]);
				}
				else
				{
					tr.LookAt(m_vtMoveTarget);
				}
			}
			else
			{
				if (!tr.gameObject.activeSelf)
				{
					break;
				}
				tr.gameObject.SetActive(false);
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator CoroutineMakePath(bool bFirst)
	{
		NavMeshAgent nma = m_navMeshAgent;

		nma.enabled = false;
		nma.enabled = true;
		yield return null;

		nma.SetDestination(m_vtMoveTarget);

		while (m_bEnablePath)
		{
			if (nma.hasPath && !nma.pathPending) break;
			yield return null;
		}


//		dd.d("CoroutineMakePath() 2", nma.pathPending, nma.pathStatus);
		if (m_bEnablePath)
		{
			CsGameData.Instance.PathCornerByAutoMove = nma.path.corners;

			CornerToPoints(nma.path.corners);

			if (bFirst)
			{
				if (CsIngameData.Instance.IngameManagement.IsContinent() || CsDungeonManager.Instance.DungeonPlay != EnDungeonPlay.Story)
				{
					yield return new WaitUntil(() => CsIngameData.Instance.IngameManagement.IsHeroStateIdle());
					if (nma.path.corners.Length > 1)
					{
						CsIngameData.Instance.IngameManagement.LookAtTarget(nma.path.corners[1]);
					}
				}
			}
		}

		if (m_navMeshAgent.hasPath)
		{
			m_navMeshAgent.ResetPath();
		}
	}

	//---------------------------------------------------------------------------------------------------
	void NavMeshSetting(float fl)
	{
		m_navMeshAgent.radius = 0.2f;
		m_navMeshAgent.height = 2f;
		m_navMeshAgent.baseOffset = 0f;

		m_navMeshAgent.angularSpeed = 720f;
		m_navMeshAgent.acceleration = 100f;
		m_navMeshAgent.stoppingDistance = 0.1f;
		m_navMeshAgent.autoBraking = true;
		m_navMeshAgent.autoRepath = true;
		m_navMeshAgent.avoidancePriority = 70;
		m_navMeshAgent.autoTraverseOffMeshLink = false;
		m_navMeshAgent.autoRepath = false;

		m_navMeshAgent.speed = fl;
		m_navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		m_navMeshAgent.enabled = false;
		m_navMeshAgent.enabled = true;
	}
}
