// Amplify Color - Advanced Color Grading for Unity Pro
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>

using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof( BoxCollider ) )]
[AddComponentMenu( "Image Effects/Amplify Color Volume" )]




public class AmplifyColorVolume : AmplifyColorVolumeBase
{
	public Color targetColor;
	Color currentColor;
	public Color DefaultColor;
	public Transform GOLight;
	Light DLight;
	bool cChangeActive=false;

	void Awake()
	{
	//	Transform GOLightFind = GameObject.Find("Light").transform;
	//	GOLight = GOLightFind.Find("Directional light");
	//	Color currentColor = GOLight.GetComponent<Light> ().color;
	}
	void Update ()
	{
	//	GOLight.GetComponent<Light> ().color = currentColor;
	//	changeColor ();


	}

	void changeColor ()
	{
		if (cChangeActive) {
			currentColor = Color.Lerp(currentColor,targetColor, Time.deltaTime * EnterBlendTime);
		}
		else{
			currentColor = Color.Lerp(currentColor,DefaultColor, Time.deltaTime * EnterBlendTime);
		}
	}

	void OnTriggerEnter( Collider other )
	{
		
		AmplifyColorTriggerProxy tp = other.GetComponent<AmplifyColorTriggerProxy>();
		if (tp != null && tp.OwnerEffect.UseVolumes && (tp.OwnerEffect.VolumeCollisionMask & (1 << gameObject.layer)) != 0) 
		{
			tp.OwnerEffect.EnterVolume (this);
	
		//	cChangeActive = true;
		
		}
	}

	void OnTriggerExit( Collider other )
	{
		AmplifyColorTriggerProxy tp = other.GetComponent<AmplifyColorTriggerProxy>();
		if (tp != null && tp.OwnerEffect.UseVolumes && (tp.OwnerEffect.VolumeCollisionMask & (1 << gameObject.layer)) != 0) 
		{
			tp.OwnerEffect.ExitVolume (this);

		//	cChangeActive = false;
		}
	}
}
