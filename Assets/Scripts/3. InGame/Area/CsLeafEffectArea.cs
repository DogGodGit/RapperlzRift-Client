using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CsLeafEffectArea : CsBaseArea
{
	ParticleSystem m_particleSystem;
	//---------------------------------------------------------------------------------------------------
	void Start()
	{
		transform.GetComponent<BoxCollider>().isTrigger = true;
		transform.GetComponent<BoxCollider>().size = new Vector3(40, 40, 40);

		m_particleSystem = transform.GetComponent<ParticleSystem>();
		if (m_particleSystem != null)
		{
			m_particleSystem.playOnAwake = false;
			m_particleSystem.Stop();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		if (m_particleSystem != null)
		{
			m_particleSystem.Play();
		}
	}

	//---------------------------------------------------------------------------------------------------
	public override void ExitAction()
	{
		if (m_particleSystem != null)
		{
			m_particleSystem.Stop();
		}
	}
}
