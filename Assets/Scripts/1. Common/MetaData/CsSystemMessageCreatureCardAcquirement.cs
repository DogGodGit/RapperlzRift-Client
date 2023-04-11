using ClientCommon;

//---------------------------------------------------------------------------------------------------
// 작성 : 오하영 (2018-10-08)
//---------------------------------------------------------------------------------------------------

public class CsSystemMessageCreatureCardAcquirement : CsSystemMessage
{
	CsCreatureCard m_csCreatureCard;

	//---------------------------------------------------------------------------------------------------
	public CsCreatureCard CreatureCard
	{
		get { return m_csCreatureCard; }
	}

	//---------------------------------------------------------------------------------------------------
	public CsSystemMessageCreatureCardAcquirement(PDCreatureCardAcquirementSystemMessage creatureCardAcquirementSystemMessage) 
		: base(creatureCardAcquirementSystemMessage)
	{
		m_csCreatureCard = CsGameData.Instance.GetCreatureCard(creatureCardAcquirementSystemMessage.creatureCardId);
	}
}
