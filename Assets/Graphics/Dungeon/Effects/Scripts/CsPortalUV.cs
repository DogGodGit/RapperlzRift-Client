using UnityEngine;
using System.Collections;

public class CsPortalUV : MonoBehaviour
{
    public float m_flXSpeed = 0;
    public float m_flYSpeed = 0;

    Vector2 m_vt2Value;

	//----------------------------------------------------------------------------------------------------
    void Start()
    {
        m_vt2Value = Vector2.zero;
    }

	//----------------------------------------------------------------------------------------------------
	void Update()
	{
		m_vt2Value.x += Time.fixedDeltaTime * m_flXSpeed;
		m_vt2Value.y += Time.fixedDeltaTime * m_flYSpeed;
		GetComponent<Renderer>().materials[0].mainTextureOffset = m_vt2Value;
	}
}
