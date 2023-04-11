using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CsLightArea : CsBaseArea
{
	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		transform.GetComponent<Light> ().enabled = false;
		transform.GetComponent<SphereCollider>().isTrigger = true;
		
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
		transform.GetComponent<Light>().enabled = true;
		}
	}

	//---------------------------------------------------------------------------------------------------

	public void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			transform.GetComponent<Light>().enabled = true;
		}
	}


		public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			transform.GetComponent<Light>().enabled = false;
		}
	}
}
