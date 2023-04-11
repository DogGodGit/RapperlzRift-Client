using UnityEngine;

public class CsWaterUVScroller : MonoBehaviour
{
	public int m_nTargetMaterialSlot = 0;	
	public float m_flSpeedX = 0.0f;
	public float m_flSpeedY = 0.5f;

	float m_flTimeWentX = 0;
	float m_flTimeWentY = 0;
	bool m_bVisible = false;

	//---------------------------------------------------------------------------------------------------
	void Update ()
	{
		if (m_bVisible)
		{
			m_flTimeWentX += Time.deltaTime * m_flSpeedX;
			m_flTimeWentY += Time.deltaTime * m_flSpeedY;			
			GetComponent<Renderer>().materials[m_nTargetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(m_flTimeWentX, m_flTimeWentY));
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnBecameVisible()
	{
		m_flTimeWentX = 0;
		m_flTimeWentY = 0;
		GetComponent<Renderer>().materials[m_nTargetMaterialSlot].SetTextureOffset("_MainTex", new Vector2(m_flTimeWentX, m_flTimeWentY));

		m_bVisible = true;
	}

	//---------------------------------------------------------------------------------------------------
	void OnBecameInvisible()
	{
		m_bVisible = false;
	}
}
