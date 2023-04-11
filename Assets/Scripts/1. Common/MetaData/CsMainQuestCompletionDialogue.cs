using WebCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-09-04)
//---------------------------------------------------------------------------------------------------

public class CsMainQuestCompletionDialogue
{
	int m_nMainQuestNo;
	int m_nDialogueNo;
	int m_nNpcId;
	string m_strDialogue;

	//---------------------------------------------------------------------------------------------------
	public int MainQuestNo
	{
		get { return m_nMainQuestNo; }
	}

	public int DialogueNo
	{
		get { return m_nDialogueNo; }
	}

	public int NpcId
	{
		get { return m_nNpcId; }
	}

	public string Dialogue
	{
		get { return m_strDialogue; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsMainQuestCompletionDialogue(WPDMainQuestCompletionDialogue mainQuestCompletionDialogue)
	{
		m_nMainQuestNo = mainQuestCompletionDialogue.mainQuestNo;
		m_nDialogueNo = mainQuestCompletionDialogue.dialogueNo;
		m_nNpcId = mainQuestCompletionDialogue.npcId;
		m_strDialogue = CsConfiguration.Instance.GetString(mainQuestCompletionDialogue.dialogueKey);
	}
}
