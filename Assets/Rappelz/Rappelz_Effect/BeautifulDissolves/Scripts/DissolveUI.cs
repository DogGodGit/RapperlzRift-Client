using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BeautifulDissolves {
	public class DissolveUI : Dissolve {

		protected override Material[] GetMaterials()
		{
			Graphic graphic = GetComponentInChildren<Graphic>();

			return graphic != null ? new Material[]{graphic.materialForRendering} : null;
		}
	}
}
