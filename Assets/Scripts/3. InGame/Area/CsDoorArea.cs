using UnityEngine;

public class CsDoorArea : CsBaseArea 
{
	//---------------------------------------------------------------------------------------------------
	public override void OnDrawGizmos()
	{
		GetComponent<Collider>().isTrigger = true;
		BoxCollider col = GetComponent<BoxCollider>();
		if (col != null)
		{
			Vector3 center, size;
			center = col.center;
			size = col.size;

			Gizmos.color = Color.cyan;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(center, size);
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		this.GetComponent<Animator>().SetInteger("Status", 1);
	}

	//---------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		this.GetComponent<Animator>().SetInteger("Status", 0);
	}
}
