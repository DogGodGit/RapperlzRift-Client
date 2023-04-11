using System.Collections;
using UnityEngine;

/*
[RequireComponent(typeof(ParticleSystem))]
public class CsGloldParticleMove : MonoBehaviour
{
	public float speed = 7f;
    public float delay = 1f;
	
	Coroutine m_coroutine;

	//---------------------------------------------------------------------------------------------------
	void Start ()
    {
		m_coroutine = StartCoroutine(GoldMove());
	}

	//---------------------------------------------------------------------------------------------------
	IEnumerator GoldMove()
	{
		ParticleSystem particleSystem = GetComponent<ParticleSystem>();
		ParticleSystem.Particle[] aparticles = new ParticleSystem.Particle[particleSystem.main.maxParticles];

		yield return new WaitForSeconds(delay);

		while (gameObject.activeSelf)
		{
			if (CsGameData.Instance.MyHeroTransform != null)
			{
				float step = speed * Time.deltaTime;
				Vector3 vtPos = CsGameData.Instance.MyHeroTransform.position - (CsGameData.Instance.MyHeroTransform.position - transform.position).normalized;

				for (int i = 0; i < aparticles.Length; i++)
				{
					aparticles[i].position = Vector3.LerpUnclamped(aparticles[i].position, vtPos, step);

				}
				particleSystem.SetParticles(aparticles, aparticles.Length);
			}
			yield return null;
		}
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		StopCoroutine(m_coroutine);
		m_coroutine = null;
	}
}
*/