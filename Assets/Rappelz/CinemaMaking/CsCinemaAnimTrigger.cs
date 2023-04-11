using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class CsCinemaAnimTrigger : MonoBehaviour 
{
	public int m_nAnimStatus;
	Transform m_trChar;

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerEnter(Collider col)
	{
        if (col.name.Equals("Char"))
		{

		}
	}
}
