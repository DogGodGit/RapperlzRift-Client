using UnityEngine;
using System.Collections;

public class ScrollBehaviour : MonoBehaviour
{
    public int materialIndex = 0;
    public Vector2 uvAnimationRate = new Vector2(1.0f, 0.0f);
    public string textureName = "_MainTex";

	public bool AlphaAnim = false;

	Color cAlpha;
    Vector2 uvOffset = Vector2.zero;


	public float maxIntensity = 1f;
	public float minIntensity = 0f;
	public float pulseSpeed = 1f; //here, a value of 0.5f would take 2 seconds and a value of 2f would take half a second
	private float targetIntensity = 1f;
	private float currentIntensity;    


    void LateUpdate()
    {
        uvOffset += (uvAnimationRate * Time.deltaTime);
		if (GetComponent<Renderer>().enabled)
        {
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset(textureName, uvOffset);
        }


		if (AlphaAnim)
			AlphaBehavior ();
    }



	void AlphaBehavior()
	{
		Color cAlpha = GetComponent<Renderer>().sharedMaterial.GetColor("_TintColor");

		currentIntensity = Mathf.MoveTowards(cAlpha.a,targetIntensity, Time.deltaTime*pulseSpeed);
		if(currentIntensity >= maxIntensity){
			currentIntensity = maxIntensity;
			targetIntensity = minIntensity;
		}
		else if(currentIntensity <= minIntensity){
			currentIntensity = minIntensity;
			targetIntensity = maxIntensity;
		}

	
		cAlpha.a = currentIntensity;
		GetComponent<Renderer>().sharedMaterial.SetColor("_TintColor",cAlpha);


	
	}
}



