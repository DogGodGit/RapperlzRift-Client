using UnityEngine;

public class CsAltarArea : CsBaseArea
{
    CsGuildManager m_CsGuildManager;

    //----------------------------------------------------------------------------------------------------
	void Start ()
    {
		gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        m_CsGuildManager = CsGuildManager.Instance;
		transform.position = CsGameData.Instance.GuildAltar.GuildTerritoryNpc.Position;
		transform.GetComponent<SphereCollider>().radius = CsGameData.Instance.GuildAltar.GuildTerritoryNpc.InteractionMaxRange;
	}

	//----------------------------------------------------------------------------------------------------
    void OnDestroy()
	{
		if (m_CsGuildManager.AltarEnter)
		{
			m_CsGuildManager.GuildAltarGoOut(false);
		}

        m_CsGuildManager = null;
    }

	//----------------------------------------------------------------------------------------------------
	public override void EnterAction()
	{
		Debug.Log("AltarEnter  TriggerIn " + m_CsGuildManager.AltarEnter);
		m_CsGuildManager.GuildAltarGoOut(true);
	}

	//----------------------------------------------------------------------------------------------------
	public override void StayAction()
	{
		if (m_CsGuildManager.AltarEnter == false)
		{
			m_CsGuildManager.GuildAltarGoOut(true);
			Debug.Log("AltarEnter  TriggerIn " + m_CsGuildManager.AltarEnter);
		}
	}

    //----------------------------------------------------------------------------------------------------
	public override void ExitAction()
    {
		Debug.Log("AltarEnter  TriggerExit " + m_CsGuildManager.AltarEnter);
		m_CsGuildManager.GuildAltarGoOut(false);
	}
}
