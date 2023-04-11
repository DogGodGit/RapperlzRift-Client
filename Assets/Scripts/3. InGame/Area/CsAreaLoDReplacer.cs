using UnityEngine;

public class CsAreaLoDReplacer : MonoBehaviour 
{
	public GameObject goReplacer;
	
	void Start () 
	{
		if (goReplacer != null)
		{
			Instantiate(goReplacer, transform);
			goReplacer.transform.parent = gameObject.transform;
		}
	}
}
