using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-08-23)
//---------------------------------------------------------------------------------------------------

public class CsBiographyQuestStartDialogue
{
	int m_nBiographyId;
	int m_nQuestNo;
	int m_nDialogueNo;
	CsNpcInfo m_csNpc;
	string m_strDialogue;

	//---------------------------------------------------------------------------------------------------
	public int BiographyId
	{
		get { return m_nBiographyId; }
	}

	public int QuestNo
	{
		get { return m_nQuestNo; }
	}

	public int DialogueNo
	{
		get { return m_nDialogueNo; }
	}

	public CsNpcInfo Npc
	{
		get { return m_csNpc; }
	}

	public string Dialogue
	{
		get { return m_strDialogue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsBiographyQuestStartDialogue(WPDBiographyQuestStartDialogue biographyQuestStartDialogue)
	{
		m_nBiographyId = biographyQuestStartDialogue.biographyId;
		m_nQuestNo = biographyQuestStartDialogue.questNo;
		m_nDialogueNo = biographyQuestStartDialogue.dialogueNo;
		m_csNpc = CsGameData.Instance.GetNpcInfo(biographyQuestStartDialogue.npcId);
		m_strDialogue = CsConfiguration.Instance.GetString(biographyQuestStartDialogue.dialogueKey);
	}
}
