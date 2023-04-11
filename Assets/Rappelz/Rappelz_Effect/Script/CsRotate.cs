using UnityEngine;

public class CsRotate : MonoBehaviour
{
	public float RotationSpeed = 50f;

	void LateUpdate()
	{
		transform.eulerAngles = new Vector3(90, Mathf.Repeat(Time.time * RotationSpeed, 360), 0);
	}
}
