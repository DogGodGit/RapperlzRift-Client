using WebCommon;
using UnityEngine;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-03-21)
//---------------------------------------------------------------------------------------------------

public class CsGuildTerritoryNpc
{
	int m_nNpcId;
	string m_strName;
	string m_strDialogue;
	bool m_bDialogueEnabled;
    Vector3 m_vtPosition;
	float m_flYRotation;
	float m_flInteractionMaxRange;
	string m_strPrefabName;
	float m_flScale;
	int m_nHeight;
	float m_flRadius;

	//---------------------------------------------------------------------------------------------------
	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public string Name
	{
		get { return m_strName; }
	}

	public string Dialogue
	{
		get { return m_strDialogue; }
	}

	public bool DialogueEnabled
	{
		get { return m_bDialogueEnabled; }
	}

    public Vector3 Position
    {
        get { return m_vtPosition; }
    }

	public float YRotation
	{
		get { return m_flYRotation; }
	}

	public float InteractionMaxRange
	{
		get { return m_flInteractionMaxRange; }
	}

	public string PrefabName
	{
		get { return m_strPrefabName; }
	}

	public float Scale
	{
		get { return m_flScale; }
	}

	public int Height
	{
		get { return m_nHeight; }
	}

	public float Radius
	{
		get { return m_flRadius; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsGuildTerritoryNpc(WPDGuildTerritoryNpc guildTerritoryNpc)
	{
		m_nNpcId = guildTerritoryNpc.npcId;
		m_strName = CsConfiguration.Instance.GetString(guildTerritoryNpc.nameKey);
		m_strDialogue = CsConfiguration.Instance.GetString(guildTerritoryNpc.dialogueKey);
		m_bDialogueEnabled = guildTerritoryNpc.dialogueEnabled;
        m_vtPosition = new Vector3(guildTerritoryNpc.xPosition, guildTerritoryNpc.yPosition, guildTerritoryNpc.zPosition);
		m_flYRotation = guildTerritoryNpc.yRotation;
		m_flInteractionMaxRange = guildTerritoryNpc.interactionMaxRange;
		m_strPrefabName = guildTerritoryNpc.prefabName;
		m_flScale = guildTerritoryNpc.scale;
		m_nHeight = guildTerritoryNpc.height;
		m_flRadius = guildTerritoryNpc.radius;
	}
}
