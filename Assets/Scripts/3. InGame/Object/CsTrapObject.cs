using UnityEngine;

public class CsTrapObject : MonoBehaviour
{
	CsStoryDungeonTrap m_csStoryDungeonTrap;
	Animator m_animator;

	public CsStoryDungeonTrap StoryDungeonTrap { get { return m_csStoryDungeonTrap; } }
	//---------------------------------------------------------------------------------------------------
	public void Init(CsStoryDungeonTrap csStoryDungeonTrap)
	{
		m_animator = transform.GetComponent<Animator>();
		transform.position = new Vector3(csStoryDungeonTrap.XPosition, csStoryDungeonTrap.YPosition, csStoryDungeonTrap.ZPosition);
		transform.eulerAngles = new Vector3(0f, csStoryDungeonTrap.YRotation, 0f);
		transform.name = csStoryDungeonTrap.PrefabName;
		transform.localScale = new Vector3(csStoryDungeonTrap.PrefabScale, csStoryDungeonTrap.PrefabScale, csStoryDungeonTrap.PrefabScale);

		m_csStoryDungeonTrap = csStoryDungeonTrap;
		if (transform.name == "Fire_Trap")
		{
			transform.Find("Trap/Ready").gameObject.SetActive(true);
		}
		StartTrap(false);
	}

	//---------------------------------------------------------------------------------------------------
	void OnDestroy()
	{
		m_csStoryDungeonTrap = null;
		m_animator = null;
	}

	//---------------------------------------------------------------------------------------------------
	public void ReadyTrap()
	{
		if (transform.name == "Fire_Trap")
		{
			transform.Find("Trap/Ready").gameObject.SetActive(true);
			transform.Find("Trap/Fire").gameObject.SetActive(false);
		}
		else
		{
			if (m_animator != null)
			{
				m_animator.SetTrigger("trap");
			}
		}
	}

	//---------------------------------------------------------------------------------------------------
	public void StartTrap(bool bStart)
	{
		if (transform.name == "Fire_Trap")
		{
			if (bStart)
			{
				transform.Find("Trap/Ready").gameObject.SetActive(false);
				transform.Find("Trap/Fire").gameObject.SetActive(true);
			}
			else
			{
				transform.Find("Trap/Ready").gameObject.SetActive(false);
				transform.Find("Trap/Fire").gameObject.SetActive(false);
			}
		}
	}
}
