using UnityEngine;
using System.Collections;

public class AlphaBehaviour : MonoBehaviour
{
	public float MinAlpha = .5f;
	public float MaxAlpha = 1f;

    void Update()
    {
		
		float random = Random.Range (MinAlpha, MaxAlpha);
		float intensity = Mathf.PerlinNoise (random, Time.time);
		Color cAlpha = GetComponent<Renderer> ().material.color;
		cAlpha.a = intensity;
		GetComponent<Renderer> ().material.color = cAlpha;
//		GetComponent<Renderer> ().material.SetColor ("_Alpha", intensity);

   
    }
}
