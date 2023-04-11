using UnityEngine;

public class CsInteractionObjectArea : CsBaseArea
{
	//---------------------------------------------------------------------------------------------------
	public override void EnterAction( )
	{
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished += OnEventMyHeroContinentObjectInteractionFinished;
	}

	//---------------------------------------------------------------------------------------------------
	public override  void ExitAction( )
	{
		CsContinentObjectManager.Instance.EventMyHeroContinentObjectInteractionFinished -= OnEventMyHeroContinentObjectInteractionFinished;
	}

	//---------------------------------------------------------------------------------------------------
	void OnAnimSound()
	{
		AudioClip audioClipDead = CsIngameData.Instance.LoadAsset<AudioClip>("Sound/Monster/SFX_Mon_Magicstone_Die01");
		gameObject.GetComponent<AudioSource>().PlayOneShot(audioClipDead);
	}

	//---------------------------------------------------------------------------------------------------
	void OnEventMyHeroContinentObjectInteractionFinished(long lInstanceId)
	{
		if (gameObject.GetComponent<Animator>() != null)
		{
			gameObject.GetComponent<Animator>().SetTrigger("dead");
		}
		else if (transform.name == "FX_ObjFire")
		{
			transform.Find("Burn").gameObject.SetActive(true);
		}
	}
}
