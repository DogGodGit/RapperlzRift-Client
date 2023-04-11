using UnityEngine;

public class CsBaseArea : MonoBehaviour
{
	public void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			EnterAction(col);
			EnterAction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerStay(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			StayAction(col);
			StayAction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void OnTriggerExit(Collider col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			ExitAction();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public virtual void OnDrawGizmos() { }
	public virtual void EnterAction(Collider col) { }
	public virtual void EnterAction() { }
	public virtual void StayAction() { }
	public virtual void StayAction(Collider col) { }
	public virtual void ExitAction() { }
}
