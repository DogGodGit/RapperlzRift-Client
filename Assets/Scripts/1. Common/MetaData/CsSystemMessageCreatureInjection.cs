using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageCreatureInjection : CsSystemMessage
{
	CsCreature m_csCreature;
	int m_nInjectionLevel;

	//---------------------------------------------------------------------------------------------------
	public CsCreature Creature
	{
		get { return m_csCreature; }
	}

	public int InjectionLevel
	{
		get { return m_nInjectionLevel; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageCreatureInjection(PDCreatureInjectionSystemMessage creatureInjectionSystemMessage) 
		: base(creatureInjectionSystemMessage)
	{
		m_csCreature = CsGameData.Instance.GetCreature(creatureInjectionSystemMessage.creatureId);
		m_nInjectionLevel = creatureInjectionSystemMessage.injectionLevel;
	}
}
