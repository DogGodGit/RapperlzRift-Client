using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageCreatureAcquirement : CsSystemMessage
{
	CsCreature m_csCreature;

	//---------------------------------------------------------------------------------------------------
	public CsCreature Creature
	{
		get { return m_csCreature; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageCreatureAcquirement(PDCreatureAcquirementSystemMessage creatureAcquirementSystemMessage)
		: base(creatureAcquirementSystemMessage)
	{
		m_csCreature = CsGameData.Instance.GetCreature(creatureAcquirementSystemMessage.creatureId);
	}
}
