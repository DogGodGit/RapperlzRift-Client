using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class cloudCycle : MonoBehaviour {
	public Slider mainSlider;
	public Material psrm;
	public void cloudSlider () {
		float value2 = ((mainSlider.value) * 2F - 0.3F) * 1.4F;
		if (mainSlider.value>0.5) {//when slider value is greater than 1, value 2 pingpong backward instead of growing greater.
			value2 = (1F-value2)*2.2F+value2;
		}
		psrm.SetColor("_TintColor", new Color(Mathf.Clamp(value2*1.2F, 0F, 0.5F),Mathf.Clamp(value2*1.1F, 0F, 0.5F),Mathf.Clamp(value2, 0F, 0.5F),0.5F));
	}
}