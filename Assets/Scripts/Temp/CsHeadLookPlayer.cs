using UnityEngine;


[System.Serializable]
public class BendingSegment
{
	public Transform firstTransform;
	public Transform lastTransform;
	public float thresholdAngleDifference = 0;
	public float bendingMultiplier = 0.7f;
	public float maxAngleDifference = 30;
	public float maxBendingAngle = 80;
	public float responsiveness = 4;

	internal float angleH;
	internal float angleV;
	internal Vector3 dirUp;
	internal Vector3 referenceLookDir;
	internal Vector3 referenceUpDir;
	internal int chainLength;
	internal Quaternion[] origRotations;
}


public enum EnCustomState { Normal, Zoom, Far }

public class CsHeadLookPlayer : MonoBehaviour
{
	public BendingSegment m_bendingSegment;
	public Vector3 m_vtHeadLookVector = Vector3.forward;
	public Vector3 m_vtHeadUpVector = Vector3.up;
	public Transform m_trTarget = null;

	public Vector3 m_vtOffset = Vector3.zero;
	public Vector3 m_vtZoomOffset = Vector3.zero;

	public float m_flEffect = 1;

	CapsuleCollider m_capsuleCollider;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		m_capsuleCollider = transform.GetComponent<CapsuleCollider>();
		m_flRadius = m_capsuleCollider.radius;
		m_trTarget = transform.parent.Find("IntroCamera/CharacterCamera");
		Quaternion parentRot = m_bendingSegment.firstTransform.parent.rotation;
		Quaternion parentRotInv = Quaternion.Inverse(parentRot);
		m_bendingSegment.referenceLookDir = parentRotInv * transform.rotation * m_vtHeadLookVector.normalized;
		m_bendingSegment.referenceUpDir = parentRotInv * transform.rotation * m_vtHeadUpVector.normalized;

		m_bendingSegment.angleH = 0;
		m_bendingSegment.angleV = 0;
		m_bendingSegment.dirUp = m_bendingSegment.referenceUpDir;
		m_bendingSegment.chainLength = 1;

		Transform trLastTransform = m_bendingSegment.lastTransform;

		while (trLastTransform != m_bendingSegment.firstTransform && trLastTransform != trLastTransform.root)
		{
			m_bendingSegment.chainLength++;
			trLastTransform = trLastTransform.parent;
		}

		m_bendingSegment.origRotations = new Quaternion[m_bendingSegment.chainLength];
		trLastTransform = m_bendingSegment.lastTransform;

		for (int i = m_bendingSegment.chainLength - 1; i >= 0; i--)
		{
			m_bendingSegment.origRotations[i] = trLastTransform.localRotation;
			trLastTransform = trLastTransform.parent;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void LateUpdate()
	{
		if (Time.deltaTime == 0) return;

		Transform trSegment = m_bendingSegment.lastTransform;

		Quaternion parentRot = m_bendingSegment.firstTransform.parent.rotation;
		Quaternion parentRotInv = Quaternion.Inverse(parentRot);

		// Desired look direction in world space
		Vector3 vtTargetPos = m_trTarget.position + m_vtOffset;
		if (m_enCustomState == EnCustomState.Zoom)
		{
			vtTargetPos = m_trTarget.position + m_vtZoomOffset;
		}

		Vector3 lookDirWorld = (vtTargetPos - m_bendingSegment.lastTransform.position).normalized;

		// Desired look directions in neck parent space
		Vector3 lookDirGoal = (parentRotInv * lookDirWorld);

		// Get the horizontal and vertical rotation angle to look at the target
		float hAngle = AngleAroundAxis(m_bendingSegment.referenceLookDir, lookDirGoal, m_bendingSegment.referenceUpDir);
		Vector3 rightOfTarget = Vector3.Cross(m_bendingSegment.referenceUpDir, lookDirGoal);
		Vector3 lookDirGoalinHPlane = lookDirGoal - Vector3.Project(lookDirGoal, m_bendingSegment.referenceUpDir);
		float vAngle = AngleAroundAxis(lookDirGoalinHPlane, lookDirGoal, rightOfTarget);

		//Debug.Log("1. CsHeadLookPlayer      hAngle : " + hAngle + " , vAngle : " + vAngle);
		if (hAngle > 120)   // 뒷쪽으로 넘어가면 정면 보고 있도록 설정.
		{
			hAngle = 0;
		}
		else if (hAngle < -120)
		{
			hAngle = 0;
		}

		// Handle threshold angle difference, bending multiplier,
		// and max angle difference here
		float hAngleThr = Mathf.Max(0, Mathf.Abs(hAngle) - m_bendingSegment.thresholdAngleDifference) * Mathf.Sign(hAngle);
		float vAngleThr = Mathf.Max(0, Mathf.Abs(vAngle) - m_bendingSegment.thresholdAngleDifference) * Mathf.Sign(vAngle);
		hAngle = Mathf.Max(Mathf.Abs(hAngleThr) * Mathf.Abs(m_bendingSegment.bendingMultiplier), Mathf.Abs(hAngle) - m_bendingSegment.maxAngleDifference) * Mathf.Sign(hAngle) * Mathf.Sign(m_bendingSegment.bendingMultiplier);
		vAngle = Mathf.Max(Mathf.Abs(vAngleThr) * Mathf.Abs(m_bendingSegment.bendingMultiplier), Mathf.Abs(vAngle) - m_bendingSegment.maxAngleDifference) * Mathf.Sign(vAngle) * Mathf.Sign(m_bendingSegment.bendingMultiplier);

		// Handle max bending angle here
		hAngle = Mathf.Clamp(hAngle, -m_bendingSegment.maxBendingAngle, m_bendingSegment.maxBendingAngle);
		vAngle = Mathf.Clamp(vAngle, -m_bendingSegment.maxBendingAngle, m_bendingSegment.maxBendingAngle);
		Vector3 referenceRightDir = Vector3.Cross(m_bendingSegment.referenceUpDir, m_bendingSegment.referenceLookDir);

		// Lerp angles
		m_bendingSegment.angleH = Mathf.Lerp(m_bendingSegment.angleH, hAngle, Time.deltaTime * m_bendingSegment.responsiveness);
		m_bendingSegment.angleV = Mathf.Lerp(m_bendingSegment.angleV, vAngle, Time.deltaTime * m_bendingSegment.responsiveness);

		// Get direction
		lookDirGoal = Quaternion.AngleAxis(m_bendingSegment.angleH, m_bendingSegment.referenceUpDir) * Quaternion.AngleAxis(m_bendingSegment.angleV, referenceRightDir) * m_bendingSegment.referenceLookDir;

		// Make look and up perpendicular
		Vector3 upDirGoal = m_bendingSegment.referenceUpDir;
		Vector3.OrthoNormalize(ref lookDirGoal, ref upDirGoal);

		// Interpolated look and up directions in neck parent space
		Vector3 lookDir = lookDirGoal;
		m_bendingSegment.dirUp = Vector3.Slerp(m_bendingSegment.dirUp, upDirGoal, Time.deltaTime * 5);
		Vector3.OrthoNormalize(ref lookDir, ref m_bendingSegment.dirUp);

		// Look rotation in world space
		Quaternion lookRot = ((parentRot * Quaternion.LookRotation(lookDir, m_bendingSegment.dirUp)) * Quaternion.Inverse(parentRot * Quaternion.LookRotation(m_bendingSegment.referenceLookDir, m_bendingSegment.referenceUpDir)));

		// Distribute rotation over all joints in segment
		Quaternion dividedRotation = Quaternion.Slerp(Quaternion.identity, lookRot, m_flEffect / m_bendingSegment.chainLength);
		trSegment = m_bendingSegment.lastTransform;
		
		for (int i = 0; i < m_bendingSegment.chainLength; i++)
		{
			trSegment.rotation = dividedRotation * trSegment.rotation;
			trSegment = trSegment.parent;
		}

		//Debug.Log("2. CsHeadLookPlayer    hAngle: " + hAngle + ", vAngle: " + vAngle + lookDir + ", " + lookDirGoal);
	}

	EnCustomState m_enCustomState = EnCustomState.Normal;
	//---------------------------------------------------------------------------------------------------
	public void ChangeState(EnCustomState enCustomState, Vector3 vtCameraPos, Vector3 vtCameraTargetPos)
	{
		Debug.Log("ChangeState : "+ enCustomState);
		if (m_capsuleCollider != null)
		{
			if (enCustomState == EnCustomState.Zoom)
			{
				m_capsuleCollider.radius = m_flRadius;
			}
			else if (enCustomState == EnCustomState.Normal)
			{
				m_capsuleCollider.radius = m_flRadius * 1.5f;
			}
			else if (enCustomState == EnCustomState.Far)
			{
				m_capsuleCollider.radius = 1;
			}
		}

		m_trTarget.position = vtCameraPos;
		m_trTarget.LookAt(vtCameraTargetPos);
		m_enCustomState = enCustomState;
	}

	// The angle between dirA and dirB around axis
	//---------------------------------------------------------------------------------------------------
	static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
	{
		dirA = dirA - Vector3.Project(dirA, axis);
		dirB = dirB - Vector3.Project(dirB, axis);
		float angle = Vector3.Angle(dirA, dirB);

		return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
	}
}
