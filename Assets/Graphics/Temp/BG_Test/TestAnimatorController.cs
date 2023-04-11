using UnityEngine;
using System.Collections;

public class TestAnimatorController : MonoBehaviour {
	public Animator animator;
	// Use this for initialization
	Vector3 m_vtPrevPos;
	void Start ()
	{
		m_vtPrevPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float y;
		y = Input.GetAxis("Vertical");
		animator.SetFloat ("Vertical", y);

		float x;
		x = Input.GetAxis("Horizontal");
		animator.SetFloat ("Horizontal", x);

		m_vtPrevPos = transform.position;

		cJump ();
	
	}

	void cJump()
	{
		if (Input.GetKey (KeyCode.Space)) 
		{
			animator.Play ("Jump");
		} 
	}
}
